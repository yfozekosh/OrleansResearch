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
                        "Data Source=localhost,1434;Initial Catalog=OrleDb;Persist Security Info=True;User ID=sa;Password=123456";
                })
                .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.Warning).AddConsole());

            using (IClusterClient client = clientBuilder.Build())
            {
                await Task.Delay(TimeSpan.FromSeconds(10));

                await client.Connect();
                while (true)
                {
                    Console.Write("please write ID\r\n>>>");
                    int id = int.Parse(Console.ReadLine());
                    if (id==-1) break;
                    
                    IHello hello = client.GetGrain<IHello>(id);

                    var res = await hello.SayHello("hohoh");

                    Console.WriteLine($"Received: {res}");
                }
            }
        }
    }
}
