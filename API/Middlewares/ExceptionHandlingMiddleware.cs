using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using FluentValidation;

namespace API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var problem = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Status = statusCode,
                Detail = exception.Message
            };

            switch (exception)
            {
                case ValidationException vex:
                    statusCode = StatusCodes.Status400BadRequest;
                    problem = new ValidationProblemDetails(vex.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                    {
                        Title = "One or more validation errors occurred.",
                        Status = statusCode
                    };
                    break;
                case UnauthorizedAccessException _:
                    statusCode = StatusCodes.Status401Unauthorized;
                    problem.Title = "Unauthorized";
                    problem.Status = statusCode;
                    break;
                case InvalidOperationException _:
                    // Some handlers return InvalidOperationException for business rules
                    statusCode = StatusCodes.Status400BadRequest;
                    problem.Title = "Invalid operation";
                    problem.Status = statusCode;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    problem.Status = statusCode;
                    break;
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(problem, options);
            return context.Response.WriteAsync(json);
        }
    }
}
