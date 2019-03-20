﻿using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace ColinCook.RazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();

            SeedDatabase(host);

            host.Run();
        }

        private static void SeedDatabase(IWebHost host)
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                System.IServiceProvider services = scope.ServiceProvider;

                ICommandBus commandBus = services.GetService<ICommandBus>();

                commandBus.Publish(new OperativeHiredCommand(OperativeId.New, "Phillip", "Johnson"),
                    CancellationToken.None);
                commandBus.Publish(new OperativeHiredCommand(OperativeId.New, "Robert", "Smith"),
                    CancellationToken.None);
                commandBus.Publish(new OperativeHiredCommand(OperativeId.New, "James", "Law"), CancellationToken.None);

                commandBus.Publish(
                    new SiteAcquiredCommand(SiteId.New, "10 Sunny Way", "Sunnytown", "ST1 0ZF", "01662 123 313"),
                    CancellationToken.None);
                commandBus.Publish(
                    new SiteAcquiredCommand(SiteId.New, "11 Sunny Way", "Sunnytown", "ST1 0ZF", "01662 123 535"),
                    CancellationToken.None);
                commandBus.Publish(
                    new SiteAcquiredCommand(SiteId.New, "12 Sunny Way", "Sunnytown", "ST1 0ZF", "01662 123 878"),
                    CancellationToken.None);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}