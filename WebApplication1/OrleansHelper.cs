using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;

namespace WebApplication1
{
    public static class OrleansHelper
    {
        private const int InitializeAttemptsBeforeFailing = 5;
        private static int _attempt;

        public static async Task<IClusterClient> Connect()
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
