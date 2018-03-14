using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        Dictionary<string, Client> clients = new Dictionary<string, Client>();
        TcpListener server;
        Queue<Message> messageQueue = new Queue<Message>();

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.126"), 9999);
            server.Start();
        }
        public void Run()
        {
            Task acceptClients = Task.Run(() => AcceptClient());
            Task queueStep = Task.Run(() => EmptyQueue());
            acceptClients.Wait();
        }
        private void AcceptClient()
        {
            
            while (true)
            {
                Client client;
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                NetworkStream stream = clientSocket.GetStream();
                client = new Client(stream, clientSocket);
                client.SetUserId();
                Console.WriteLine(client.UserId + " has Connected.");
                clients.Add(client.UserId, client);
                Task scanner = Task.Run(() => MessageScan(client));
            }
        }

        public void MessageScan(Client client)
        {
            while (true)
            {
                messageQueue.Enqueue(client.RecieveMessage());
            }
        }
        
        public void EmptyQueue()
        {
            while (true)
            {
                if(messageQueue.Count > 0) Respond(messageQueue.Dequeue());
            }
        }

        private void Respond(Message message)
        {
             foreach(KeyValuePair<string, Client> item in clients)
            {
                if (!(message.UserId == item.Key))
                {
                    item.Value.Send(message.UserId + ": " + message.Body);
                }
            }
        }
    }
}
