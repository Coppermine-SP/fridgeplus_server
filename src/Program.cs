using fridgeplus_server.Context;
using fridgeplus_server.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace fridgeplus_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromHours(8));
            builder.Services.AddAuthorization();
            builder.Services.AddDbContext<ServerDbContext>();
            builder.Services.AddSingleton(typeof(IReceiptRecognizeService), typeof(AzureReceiptRecognizeService));
            builder.Services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });
            
            var app = builder.Build();

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
