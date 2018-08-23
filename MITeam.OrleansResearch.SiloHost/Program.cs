using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MITeam.OrleansResearch.GrainContracts;
using MITeam.OrleansResearch.GrainImplementation;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;

namespace MITeam.OrleansResearch.SiloHost
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var host = await StartSilo();
                
                // hack
                await Task.Delay(TimeSpan.FromDays(1));

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
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
                        "Data Source=orledb;Initial Catalog=OrleDb;Persist Security Info=True;User ID=sa;Password=OrleDBP@asswor1d";
                })
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                .UseDashboard(options => { options.Port = 8090; })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.SetMinimumLevel(LogLevel.Warning).AddFile("log.warn.txt");
                    loggingBuilder.SetMinimumLevel(LogLevel.Information).AddFile("log.info.txt");
                    loggingBuilder.SetMinimumLevel(LogLevel.Debug).AddFile("log.debug.txt");
                });

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
