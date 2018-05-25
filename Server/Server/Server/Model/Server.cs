using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace Server
{
    public class Server
    {
        static int port = 36000;
        static string ip = "127.0.0.1";
        System.Net.IPAddress theIPAddress;
        TcpListener myTcpListener;
        Thread startServer;
        ManualResetEvent _shutdownEvent;
        Socket clientSocket;
        List<handleClient> _clientList;
        public Server()
        {
            
        }

        public void StartServer()
        {
            _clientList = new List<handleClient>();
            startServer = new Thread(new ThreadStart(ServerStart));
            _shutdownEvent = new ManualResetEvent(false);
            startServer.IsBackground = true;
            startServer.Start();
        }

        private void ServerStart()
        {
            Log.WriteTime("Info:Sever服務已開啟");
            theIPAddress = System.Net.IPAddress.Parse(ip);
            myTcpListener = new TcpListener(theIPAddress, port);
            myTcpListener.Start();
            Log.WriteTime("Info:通訊埠" + port + "等待用戶端連線...... !!");
            int counter = 0;
            do
            {
                clientSocket = myTcpListener.AcceptSocket();
                try
                {
                    if (clientSocket.Connected)
                    {
                        counter++;
                        Log.WriteTime("Info:用戶(" + counter + ")連線成功 !!");
                        handleClient client = new handleClient();
                        client.startClient(clientSocket, counter,this);
                        _clientList.Add(client);
                        _clientList[0].SendMessage("123");
                    }
                }
                catch (Exception e)
                {
                }
            } while (true);
            
            myTcpListener.Stop();
        }

        public void SendMessage(string message)
        {
            for (int i = 0; i < _clientList.Count(); i++)
            {
                _clientList[i].SendMessage(message);
            }
        }

        public void SendMessage(string message, int no)
        {
            _clientList[no].SendMessage(message);
        }

        public void SendMessage(string message, string type)
        {

        }

        public void CallAndCatch(string message,int no)
        {

        }

        public void StopServer()
        {
            try
            {
                clientSocket.Close();
                myTcpListener.Stop();
            }
            catch (Exception e) { }
            try
            {
                startServer.Abort();
                startServer.Join();
                startServer = null;
            }
            catch (Exception e) { }

            Log.WriteTime("Info:Sever服務已被關閉");
        }
        ~Server()
        {
            StopServer();
        }
    }
}
