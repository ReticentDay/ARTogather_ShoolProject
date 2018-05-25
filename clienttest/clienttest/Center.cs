using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace clienttest
{
    class Center
    {
        Client client = new Client();
        public void Start()
        {
            client.StartClient();
        }
    }
}
