using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreAPI.DataAccess;
using CoreAPI.Helpers;
using CoreAPI.Service.Implementation;
using CoreAPI.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CoreAPI
{
    public class Startup
    {
        private readonly IHostEnvironment _env;
        private IConfiguration Configuration { get; }
        private readonly string _appsettingFileName;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            _env = environment;
            string environmentConfig = $"appsettings.{environment?.EnvironmentName}.json";

            _appsettingFileName = !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), environmentConfig))
                ? "appsettings.json"
                : environmentConfig;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile(_appsettingFileName, optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(   "http://localhost:4200",
                                    "https://srsUI.azurewebsites.net",
                                    "https://srsui-staging.azurewebsites.net");

                });
            });
            services.AddControllers();

            services.AddDbContext<CoreDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:CoreDatabase"]));
            services.AddSingleton(_env);
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IAuditService, AuditService>();
            services.AddSingleton<IAuthService, AuthService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //}

            //app.UseExceptionHandler("/api/Error");

            //app.ConfigureExceptionHandler();
            //app.UseExceptionHandler();
            //app.UseExceptionHandler("/api/error/{0}");            

            // app.UseStatusCodePagesWithReExecute("/api/error");
            //app.UseHsts();
            //app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseExceptionHandler("/api/error/{0}");
            app.UseStatusCodePagesWithReExecute("/api/error/{0}");
            app.ConfigureCustomExceptionMiddleware();

            // app.UseHsts();
            //app.UseStatusCodePagesWithReExecute("/api/error");

            //// Handles exceptions and generates a custom response body
            //app.UseExceptionHandler("/api/error/500");

            //// Handles non-success status codes with empty body
            //app.UseStatusCodePagesWithReExecute("/api/error/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowSpecificOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app?.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<CoreDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
