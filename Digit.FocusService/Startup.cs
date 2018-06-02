using System.IdentityModel.Tokens.Jwt;
using System.IO;
using ButlerClient;
using Digit.FocusService.Impl;
using Digit.FocusService.Impl.EF;
using Digit.FocusService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Digit.FocusService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            if (string.IsNullOrWhiteSpace(hostingEnvironment.WebRootPath))
            {
                hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireClaim("scope", "digit.user");
                });
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Service", builder =>
                {
                    builder.RequireClaim("scope", "digit.service");
                });
            });
        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }


        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<FocusServiceContext>(options => options.UseSqlite($"Data Source={HostingEnvironment.WebRootPath}\\App_Data\\focusService.db"));
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["ServiceIdentityUrl"];
                    options.Audience = "digit";
                    options.RequireHttpsMetadata = false;
                });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            ConfigureAuthentication(services);
            ConfigureAuthorization(services);
            ConfigureCors(services);
            ConfigureDatabase(services);

            services.Configure<ButlerOptions>(Configuration);
            services.AddTransient<IButler, Butler>();

            services.AddTransient<IScheduleService, ScheduleService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}
