using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using client;

namespace clienttest
{
    public class Center
    {
        Client myClient;
        public void tryClient()
        {
            myClient = new Client();
            myClient.StartClient();
            myClient.WriteData("try");
        }
    }
}
