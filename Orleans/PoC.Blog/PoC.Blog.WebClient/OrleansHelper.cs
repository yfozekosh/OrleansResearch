using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Logging;
using Orleans.Runtime;

namespace PoC.Blog.WebClient
{
    public static class OrleansHelper
    {
        private static int _attempt;
        private const int InitializeAttemptsBeforeFailing = 10;

        public static async Task<IClusterClient> Connect()
        {
            var clientBuilder = new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "PoC.Blog.BloggingCluster";
                    options.ServiceId = "PoC.Blog.SiloHost";
                })
                .UseAdoNetClustering((AdoNetClusteringClientOptions options) =>
                {
                    options.ConnectionString = "Host=orleans.db.srv;Port=5432;Database=PoC_Blog_ClusteringDb;Username=postgres;Password=Orleans!Bpasswor_D";
                    options.Invariant = "Npgsql";
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.SetMinimumLevel(LogLevel.Information).AddFile("log.info.txt");
                    loggingBuilder.SetMinimumLevel(LogLevel.Debug).AddFile("log.debug.txt");
                });

            IClusterClient client = clientBuilder.Build();

            await client.Connect(RetryFilter);

            return client;
        }

        private static async Task<bool> RetryFilter(Exception exception)
        {
            if (exception.GetType() != typeof(SiloUnavailableException))
            {
                Console.WriteLine($"Cluster client failed to connect to cluster with unexpected error.  Exception: {exception}");
                return false;
            }
            _attempt++;
            Console.WriteLine($"Cluster client attempt {_attempt} of {InitializeAttemptsBeforeFailing} failed to connect to cluster.  Exception: {exception}");
            if (_attempt > InitializeAttemptsBeforeFailing)
            {
                return false;
            }
            await Task.Delay(TimeSpan.FromSeconds(4));
            return true;
        }
    }
}
