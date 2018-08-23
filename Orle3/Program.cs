using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using GrainContracts;
using GrainImpl;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Orle3
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            Console.Write("Port\r\n>>>");


            int p = int.Parse(Console.ReadLine());

            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "HelloWorldApp";
                })
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = "System.Data.SqlClient";
                    options.ConnectionString =
                        "Data Source=localhost,1434;Initial Catalog=OrleDb;Persist Security Info=True;User ID=sa;Password=123456";
                })
                .ConfigureEndpoints(siloPort: p, gatewayPort: 30000)
                //.ConfigureApplicationParts(parts =>
                //    parts.AddApplicationPart(typeof(IHello).Assembly).WithReferences())
                .UseDashboard(options => { options.Port = 8090; })
                .ConfigureLogging(loggingBuilder => loggingBuilder.SetMinimumLevel(LogLevel.Warning).AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
