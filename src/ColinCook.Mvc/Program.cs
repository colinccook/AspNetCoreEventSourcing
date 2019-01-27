using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.Operatives.Entities;
using ColinCook.VisitWorkflow.Operatives.Identities;
using LiteDB;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

                var database = services.GetRequiredService<LiteRepository>();

                    IEnumerable<OperativeEntity> operatives = new List<OperativeEntity>
                    {
                        new OperativeEntity {Forename = "Bob", Surname = "Smith"},
                        new OperativeEntity {Forename = "Simon", Surname = "Jenkins"},
                        new OperativeEntity {Forename = "Phillip", Surname = "Sea-more"}
                    };

                    database.Insert(operatives);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
