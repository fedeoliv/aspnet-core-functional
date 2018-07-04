using System;
using CustomerManagement.Logic.Model;
using CustomerManagement.Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace CustomerManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var connectionString = Configuration.GetConnectionString("CustomerManagementDatabase");
            Initer.Init(connectionString);
        }
        
        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            ApplicationContainer = CreateApplicationContainer(services);

            // Creating the IServiceProvider out of the Autofac container
            return new AutofacServiceProvider(ApplicationContainer);
        }

        private IContainer CreateApplicationContainer(IServiceCollection services)
        {
            // create a Autofac container builder
            var builder = new ContainerBuilder();

            // read service collection to Autofac
            builder.Populate(services);

            builder.RegisterType<UnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<EmailGateway>()
                .As<IEmailGateway>()
                .InstancePerDependency();

            // Build and return the Autofac container
            return builder.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Customer}/{action=Index}");
            });

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            // You can only do this if you have a direct reference to the container,
            // so it won't work with the above ConfigureContainer mechanism.
            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
