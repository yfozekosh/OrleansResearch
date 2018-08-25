using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;

namespace PoC.Blog.SiloHost
{
    internal static class Program
    {
        public static IConfiguration Configuration { get; set; }

        internal static async Task Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var configDirectory = Path.Combine(currentDirectory, "..", "config");
            var entries = Directory.GetFileSystemEntries(currentDirectory);

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("db-settings.json");
            Configuration = builder.Build();

            string orleDbConnection = Configuration.GetConnectionString("OrleDb");
            string persistenceConnection = Configuration.GetConnectionString("BloggingContext");
            string invariant = "Npgsql";

            await Task.WhenAll(WaitUntilReady(orleDbConnection), WaitUntilReady(persistenceConnection));

            var siloHostBuilder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "PoC.Blog.BloggingCluster";
                    options.ServiceId = "PoC.Blog.SiloHost";
                })
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = orleDbConnection;
                    options.Invariant = invariant;
                })
                .AddAdoNetGrainStorageAsDefault(delegate(AdoNetGrainStorageOptions options)
                {
                    options.Invariant = invariant;
                    options.ConnectionString = persistenceConnection;
                    options.UseJsonFormat = true;
                })
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.SetMinimumLevel(LogLevel.Information).AddFile("log.info.txt");
                    loggingBuilder.SetMinimumLevel(LogLevel.Debug).AddFile("log.debug.txt");
                });


            ISiloHost host = siloHostBuilder.Build();

            await host.StartAsync();

            await Task.Delay(TimeSpan.FromTicks(int.MaxValue));
        }

        internal static async Task WaitUntilReady(string connectionString)
        {
            using (Npgsql.NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                while (true)
                {
                    try
                    {
                        await connection.OpenAsync();
                        connection.Close();
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Connection refused. Waiting 5 sec...");
                        await Task.Delay(TimeSpan.FromSeconds(5));
                    }
                }
                
            }
        }
    }
}
