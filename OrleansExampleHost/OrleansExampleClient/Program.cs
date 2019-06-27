using Instana.ManagedTracing.Api;
using Instana.ManagedTracing.Sdk.Spans;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using OrleansExample.Grains.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrleansBasics
{
    public class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    using (var client = await ConnectClient())
                    {
                        await DoClientWork(client);
                    }
                    Thread.Sleep(1000);
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "InstanaOrleansExample";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            await Task.Run(async () =>
            {
                using (var span = CustomSpan.CreateEntry(null, (ISpanContext)null))
                {
                    span.SetTag("service", "OrleansExampleClient");
                    // example of calling grains from the initialized client
                    var friend = client.GetGrain<IHelloGrain>(0);
                    var message = new GrainMessage<string>("Oliver Twist");
                    using (var exitSpan = CustomSpan.CreateExit(friend, (name, value) => { message.Headers.Add(name, value); }))
                    {
                        exitSpan.SetTag("service", "OrleansExampleClient");
                        var response = await friend.GreetMe(message);
                        Console.WriteLine("\n\n{0}\n\n", response);
                    }
                }

            });
        }
    }
}