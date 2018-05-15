using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Center
    {
        Server server = new Server();
        public void start()
        {
            server.StartServer();
        }
        public void stop()
        {
            server.StopServer();
        }
        public void sendMessage(string message)
        {
            server.SendMessage(message);
        }
    }
}
