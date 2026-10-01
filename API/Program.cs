
using Application;
using Infrastructure;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add infrastructure Service
            builder.Services.AddInfrastructure(builder.Configuration);

            // Add Application Service

            builder.Services.AddApplication();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            // Swagger Configration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            // Global exception handling
            app.UseMiddleware<API.Middlewares.ExceptionHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            // Ensure roles exist and optional admin user
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var roleManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
                    var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Infrastructure.Identity.ApplicationUser>>();
                    var config = services.GetRequiredService<IConfiguration>();
                    var roles = new[] { "Admin", "User" };
                    foreach (var role in roles)
                    {
                        var exists = roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();
                        if (!exists)
                        {
                            roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(role)).GetAwaiter().GetResult();
                        }
                    }

                    var adminEmail = config.GetValue<string>("AdminUser:Email");
                    var adminPassword = config.GetValue<string>("AdminUser:Password");
                    if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
                    {
                        var admin = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
                        if (admin == null)
                        {
                            admin = new Infrastructure.Identity.ApplicationUser { UserName = adminEmail, Email = adminEmail };
                            var result = userManager.CreateAsync(admin, adminPassword).GetAwaiter().GetResult();
                            if (result.Succeeded)
                            {
                                userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while seeding roles and admin user.", ex);
                }
            }


            app.MapControllers();

            app.Run();
        }
    }
}
