using Autofac;
using Autofac.Extensions.DependencyInjection;
using ColinCook.RazorPages.Binders;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.ReadModels;
using ColinCook.VisitWorkflow.AggregateRoots.Visits.ReadModels;
using ColinCook.VisitWorkflow.AggregateRoots.Works.ReadModels;
using EventFlow;
using EventFlow.AspNetCore.Middlewares;
using EventFlow.Autofac.Extensions;
using EventFlow.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ColinCook.RazorPages
{
    public class Startup
    {
        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //        .AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //}

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
            ContainerBuilder builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            IEventFlowOptions container = EventFlowOptions.New
                .UseAutofacContainerBuilder(builder) // Must be the first line!
                .AddDefaults(Assembly.LoadFrom("bin/Debug/netcoreapp2.2/ColinCook.VisitWorkflow.dll"))
                .UseInMemoryReadStoreFor<VisitReadModel>()
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMiddleware<CommandPublishMiddleware>();

            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}