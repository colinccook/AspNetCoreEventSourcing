using System.Threading;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ColinCook.Mvc
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
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
