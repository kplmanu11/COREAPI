using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Seva.API
{
    using ActionFilters;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Services;
    using Seva.API.Helper;
    using Seva.API.Infrastructure;
    using System.Text;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }
        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            ConfigureSwagger(services);

            services.AddDbContext<AppDbContext>(it =>
            {
                it.UseSqlServer(Configuration["Database:ConnectionString"]);
            },
                 ServiceLifetime.Transient
            );

            CreateIdentityIfNotCreated(services);

            ConfigureAuthenticationSettings(services);

            services.AddScoped<IUserService, UserService>();

            services.AddMvc(options => options.RespectBrowserAcceptHeader = true);
        }
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Google AUTH Seva Employee API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseCors(it => it.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandler>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            IdentityModelEventSource.ShowPII = true;
        }

        #region Private Methods
        private static void CreateIdentityIfNotCreated(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var existingUserManager = scope.ServiceProvider.GetService<UserManager<LoginUser>>();
                if (existingUserManager == null)
                {
                    services.AddIdentity<LoginUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders();
                }
            }
        }
        private void ConfigureAuthenticationSettings(IServiceCollection services)
        {
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Authentication:Jwt:Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            })
              .AddGoogle(options =>
              {
                  options.ClientId = Configuration["Authentication:Google:ClientId"];
                  options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
              });
        }
        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WEB API",
                    Description = "Employeebase User Directory API for Seva Development",
                    Contact = new OpenApiContact
                    {
                        Name = "Tech Coaches (Seva Development)",
                        Email = "techcoaches@sevadev.com",
                        Url = new Uri("https://www.sevadevelopment.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Seva Development",
                    }
                });
                c.SchemaFilter<SwaggerExcludeFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                                    Enter 'Bearer' [space] and then your token in the text input below. 
                                    Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });
        }
        #endregion
    }
}
