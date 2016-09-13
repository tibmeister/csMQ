using System;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace csMQ
{
    public class Program
    {
        public FileSystemWatcher fsWatch;

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            var redisConfig = configuration.GetSection("Server");
            var redisQueues = configuration.GetSection("Queues");

            ConfigSettings.Host = redisConfig["Host"];
            ConfigSettings.CompletedQueue = redisQueues["CompletedQueue"];
            ConfigSettings.ErrorQueue = redisQueues["ErrorQueue"];
            ConfigSettings.JobQueue = redisQueues["JobQueue"];
            ConfigSettings.RunQueue = redisQueues["RunQueue"];

            var redis = ConnectionMultiplexer.Connect(ConfigSettings.Host);

            var db = redis.GetDatabase();
            var t = "";
            bool doCheck = true;
	    Console.WriteLine("Now listening\n\n");

            while (true)
            {
                while (doCheck)
                {
                    try
                    {
                        t = db.ListLeftPop(ConfigSettings.JobQueue);
                        if (t.ToString().Length > 0)
                        {
                            doCheck = false;
                        }
                    }
                    catch
                    { }
                    System.Threading.Thread.Sleep(500);
                }
                Console.WriteLine(t);
                doCheck = true;
            }
        }
    }
}
