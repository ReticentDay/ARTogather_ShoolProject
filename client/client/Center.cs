using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace client
{
    class Center
    {
        Client client = new Client();
        public delegate void SendData(string data);
        SendData _SD;
        public void StartClient()
        {
            client.StartClient();
            _SD = new SendData(client.GetData);
        }
    }
}
