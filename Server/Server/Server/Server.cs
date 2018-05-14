using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace Server
{
    class Server
    {
        static int port = 36000;
        static string ip = "127.0.0.1";
        System.Net.IPAddress theIPAddress;
        TcpListener myTcpListener;
        Thread startServer;
        ManualResetEvent _shutdownEvent;
        Socket clientSocket;

        public Server()
        {
            
        }

        public void StartServer()
        {
            startServer = new Thread(new ThreadStart(ServerStart));
            _shutdownEvent = new ManualResetEvent(false);
            startServer.IsBackground = true;
            startServer.Start();
        }

        private void ServerStart()
        {
            theIPAddress = System.Net.IPAddress.Parse(ip);
            myTcpListener = new TcpListener(theIPAddress, port);
            myTcpListener.Start();
            Log.WriteTime("Info:通訊埠" + port + "等待用戶端連線...... !!");
            int counter = 0;
            do
            {
                clientSocket = myTcpListener.AcceptSocket();
                if (_shutdownEvent.WaitOne(0)) break;
                try
                {
                    if (clientSocket.Connected)
                    {
                        counter++;
                        Log.WriteTime("Info:用戶(" + counter + ")連線成功 !!");
                        handleClient client = new handleClient();
                        client.startClient(clientSocket, counter);
                    }
                }
                catch (Exception e)
                {
                }
            } while (true);
            
            myTcpListener.Stop();
        }

        public void StopServer()
        {
            _shutdownEvent.Set();
            try
            {
                clientSocket.Close();
            }
            catch (Exception e) { }
            myTcpListener.Stop();
            startServer.Abort();
            startServer.Join();
            startServer = null;
        }
    }
}
