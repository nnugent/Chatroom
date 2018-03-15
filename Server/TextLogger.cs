using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class TextLogger : ILogger
    {
        string filePath;
        public TextLogger()
        {
            filePath = @"\ChatRoom\chatLog.txt";
            using (StreamWriter streamWriter = new StreamWriter(filePath)) streamWriter.WriteLine("Beginning of Chat Room.");
        }
        public void Log(Message message)
        {
            
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(message.UserId + ": " + message.Body);
            }
        }
        public void Log(string message)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(message);
            }
        }
    }
}
