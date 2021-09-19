// Company: Antecipa
// Project: AppSample
// Solution: Innovt.Contrib.Authorization
// Date: 2021-08-07

using Innovt.Contrib.Authorization.AspNetCore;
using Innovt.Contrib.AuthorizationRoles.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AppSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AppSample", Version = "v1" });
            });

            services.AddAuthentication("Token");

            services.AddAuthorization();


            //services.AddMvc(s =>
            //{
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            //    s.Filters.Add(new AuthorizeFilter(policy));

            //    s.Filters.Add(typeof(AuthorizationFilter));
            //});

            //services.AddScoped<IAuthorizationService, SampleAuthorizationService>();

            //services.AddScoped<IdentityUserRole, SampleAuthorizationService>();

            //services.AddInnovtAuthorization("Sample");
            services.AddInnovtRolesAuthorization("Sample");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppSample v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}