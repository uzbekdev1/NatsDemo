using System;
using System.Text;
using System.Threading;
using NATS.Client;
using NatsCommon;
using Newtonsoft.Json;

namespace NatsServer
{
    internal class Program
    {
        private static readonly AutoResetEvent _waiter = new AutoResetEvent(false);

        private static void Main( )
        {
            Console.Title = "Server";

            var opts = ConnectionFactory.GetDefaultOptions();

            opts.Url = Defaults.Url;

            using var c = new ConnectionFactory().CreateConnection(opts);
            using var s = c.SubscribeAsync("foo", Handler );

            _waiter.WaitOne();
        }

        private static void Handler(object sender, MsgHandlerEventArgs e)
        {
            var payload = e.Message.Data;
            var message = Encoding.UTF8.GetString(payload);
            var item = JsonConvert.DeserializeObject<LotItem>(message);

            var dateNow = DateTime.UtcNow;

            Console.WriteLine(dateNow.Subtract(item.Date).ToString("g"));
        }
    }
}
