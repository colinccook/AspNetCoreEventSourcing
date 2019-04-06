using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives.ReadModels;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites.ReadModels;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Works.ReadModels;
using ColinCCook.AspNetCoreEventSourcing.RazorPages.Binders;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new SiteIdModelBinderProvider());
                options.ModelBinderProviders.Insert(1, new OperativeIdModelBinderProvider());
                options.ModelBinderProviders.Insert(2, new WorkIdModelBinderProvider());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            var container = EventFlowOptions.New
                .UseAutofacContainerBuilder(builder) // Must be the first line!
                .AddDefaults(Assembly.LoadFrom("bin/Debug/netcoreapp2.2/ColinCCook.AspNetCoreEventSourcing.EventFlow.dll"))
                .UseInMemoryReadStoreFor<OperativeReadModel>()
                .UseInMemoryReadStoreFor<SiteReadModel>()
                .UseInMemoryReadStoreFor<WorkReadModel>()
                .UseConsoleLog();

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            // app.UseMiddleware<CommandPublishMiddleware>();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}