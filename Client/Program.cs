using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("192.168.0.126", 9999);
            Console.WriteLine("Enter your User Name:");
            Task send = new Task(() => client.Send());
            Task receive = new Task(() => client.Recieve());
            Parallel.Invoke(() => send.Start(), () => receive.Start());
            send.Wait();
            receive.Wait();
        }
    }
}
