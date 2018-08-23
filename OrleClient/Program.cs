using System;
using System.Net;
using System.Threading.Tasks;
using GrainContracts;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace OrleClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IClientBuilder clientBuilder = new Orleans.ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "HelloWorldApp";
                })
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = "System.Data.SqlClient";
                    options.ConnectionString =
                        "Data Source=localhost,1435;Initial Catalog=OrleDb;Persist Security Info=True;User ID=sa;Password=OrleDBP@asswor1d";
                })
                .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.Warning).AddConsole());

            using (IClusterClient client = clientBuilder.Build())
            {
                await client.Connect(exception => Task.FromResult(true));
                while (true)
                {
                    Console.Write("please write ID\r\n>>>");
                    int id = int.Parse(Console.ReadLine());
                    if (id==-1) break;
                    
                    ICounterGrain counterGrain = client.GetGrain<ICounterGrain>(id);

                    var res = await counterGrain.GetCount();

                    Console.WriteLine($"Received: {res}");
                }
            }
        }
    }
}
