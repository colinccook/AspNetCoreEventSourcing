using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ColinCook.RazorPages
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

                commandBus.Publish(new OperativeHiredCommand(OperativeId.New, "Phillip", "Johnson"), CancellationToken.None);
                commandBus.Publish(new OperativeHiredCommand(OperativeId.New, "Robert", "Smith"), CancellationToken.None);
                commandBus.Publish(new OperativeHiredCommand(OperativeId.New, "James", "Law"), CancellationToken.None);

                commandBus.Publish(new SiteAcquiredCommand(SiteId.New, "10 Sunny Way", "Sunnytown", "ST1 0ZF", "01662 123 313"), CancellationToken.None);
                commandBus.Publish(new SiteAcquiredCommand(SiteId.New, "11 Sunny Way", "Sunnytown", "ST1 0ZF", "01662 123 535"), CancellationToken.None);
                commandBus.Publish(new SiteAcquiredCommand(SiteId.New, "12 Sunny Way", "Sunnytown", "ST1 0ZF", "01662 123 878"), CancellationToken.None);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
