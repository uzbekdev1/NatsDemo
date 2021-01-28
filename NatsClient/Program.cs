using System;
using System.Text;
using NATS.Client;
using NatsCommon;
using Newtonsoft.Json;

namespace NatsClient
{
    internal class Program
    {
        private static void Main()
        {

            Console.Title = "Client";

            var opts = ConnectionFactory.GetDefaultOptions();

            opts.Url = Defaults.Url;

            using var c = new ConnectionFactory().CreateConnection(opts);

            while (true)
            {
                var counter = new Random().Next(1, 1000000);
                var item = new LotItem
                {
                    Contract = counter,
                    Date = DateTime.UtcNow,
                    Lot = counter,
                    Price = counter * 100m,
                    User = $"{Guid.NewGuid()}"
                };
                var message = JsonConvert.SerializeObject(item);
                var payload = Encoding.UTF8.GetBytes(message);

                c.Publish("foo", payload);

                Console.WriteLine(message);
            }
        }
    }
}
