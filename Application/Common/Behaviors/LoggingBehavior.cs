using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>:IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
      
    {
       
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Get Operation Name
            var requestName = typeof(TRequest).Name;

            //Log Operation Start
            _logger.LogInformation($"Handling Request {requestName}");

            // Start Watch time
            var stopwatch = Stopwatch.StartNew();

            // excute real Opertion
            var response = await next();

            // Stop Watch time
            stopwatch.Stop();

            //Log Operation end 
            _logger.LogInformation($"Finished: {requestName} in {stopwatch.ElapsedMilliseconds} ms");

            // Return result (Handler Excution)
            return response;
        }
    }
}
