using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.CrossCutting.IOC.StructureMap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApiTest
{
    public class Startup:Innovt.AspNetCore.ApiStartupBase  
    {
       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    app.UseRouting();

        //    app.UseAuthorization();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllers();
        //    });
        //}
        public Startup(IConfiguration configuration) : base(configuration,"Teste","teste michel","v1")
        {
        }

        protected override IContainer ConfigureIoc(IServiceCollection services)
        {
            var container = new StructureMapContainer();

            return container;
        }

        protected override void AddDefaultServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
