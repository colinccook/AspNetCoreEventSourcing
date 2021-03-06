﻿using System.Threading;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives.Commands;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites.Commands;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow;
using EventFlow.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            SeedDatabase(host);

            host.Run();
        }

        private static void SeedDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var commandBus = services.GetService<ICommandBus>();

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
            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                    {
                        logging.AddConsole();
                        logging.AddDebug();
                    }
                )
                .UseStartup<Startup>();
        }
    }
}