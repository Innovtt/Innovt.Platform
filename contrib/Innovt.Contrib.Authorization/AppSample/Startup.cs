// Company: Antecipa
// Project: AppSample
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System.Threading.Tasks;
using Innovt.Contrib.AuthorizationRoles.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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



//            services.AddInnovtRolesAdminAuthorization();

            //services.AddInnovtRolesAuthorization();

            //services.AddAuthentication("Token");

            //services.AddAuthorization();

            //services.AddControllers();

            //services.AddMvc(s =>
            //{
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            //    s.Filters.Add(new AuthorizeFilter(policy));

            //    s.Filters.Add(typeof(AuthorizationFilter));
            //});

            //services.AddInnovtRolesAuthorization();
            
            //services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = new AuthorizationPolicyBuilder("")
            //        .RequireAuthenticatedUser()
            //        .Build();
            //});


            services.AddControllers(s =>
            {
                //var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                //s.Filters.Add(new AuthorizeFilter(policy));

                //s.Filters.Add(new AuthorizationFilter());
            });

            //services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
            //services.AddScoped<IdentityUserRole, SampleAuthorizationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(op => { op.SerializeAsV2 = true; });
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppSample v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                context.Request.Headers.Add("X-AppName", "Your Name");
                await next();
            });


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}