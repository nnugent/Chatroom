using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        NetworkStream stream;
        TcpClient client;
        public string userId;

        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            userId = "";
        }

        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }

        public Message RecieveMessage()
        {
            Message message;
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            int length = recievedMessage.TakeWhile(b => b != 0).Count();
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage, 0, length);
            Console.WriteLine(userId + ": " + recievedMessageString);
            message = new Message(this, recievedMessageString);
            return message;
        }

        public void SetUserId()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            int length = recievedMessage.TakeWhile(b => b != 0).Count();
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage, 0, length);
            userId = recievedMessageString;
        }

        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }
    }
}
