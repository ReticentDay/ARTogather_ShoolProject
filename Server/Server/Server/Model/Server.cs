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
        static string ip = "192.168.1.110";
        System.Net.IPAddress theIPAddress;
        TcpListener myTcpListener;
        Thread startServer;
        ManualResetEvent _shutdownEvent;
        Socket clientSocket;
        List<handleClient> _clientList;
        LoadFile _LF;
        List<string> FileList;
        public Server()
        {
            _LF = new LoadFile();
            FileList = _LF.ReadIndex();
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
                try
                {
                    clientSocket = myTcpListener.AcceptSocket();
                }
                catch (Exception e)
                {

                }
                try
                {
                    if (clientSocket.Connected)
                    {
                        counter++;
                        Log.WriteTime("Info:用戶(" + counter + ")連線成功 !!");
                        handleClient client = new handleClient();
                        client.startClient(clientSocket, counter,this);
                        _clientList.Add(client);
                        //_clientList[0].SendMessage("123");
                    }
                }
                catch (Exception e)
                {
                }
            } while (true);
            
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
            for (int i = 0; i < _clientList.Count(); i++)
            {
                if(_clientList[i].type == type)
                    _clientList[i].SendMessage(message);
            }
            Log.WriteTime("Info:已全數傳完");
        }

        public void CallAndCatch(string message,int no)
        {
            string[] messages = message.Split(':');
            if (messages[0] == "Need")
            {
                string table = messages[1];
                if (FileList.FindIndex(item => item == table) == -1)
                {
                    SendMessage("Error:No fide data", no);
                    Thread.Sleep(100);
                }
                else
                {
                    List<string> fileData = _LF.ReadData(table);
                    _clientList[no].type = table;
                    for (int i = 0; i < fileData.Count; i++)
                    {
                        SendMessage("add:"+fileData[i],no);
                    }
                }
            }
            else if(messages[0] == "fix")
            {
                if (_clientList[no].type != "none")
                {
                    List<string> IdList = _LF.ReadData(_clientList[no].type, "Id = " + messages[1]);
                    if (IdList.Count > 0)
                    {
                        try
                        {
                            _LF.UpdateData(_clientList[no].type, int.Parse(messages[1]), float.Parse(messages[2]), float.Parse(messages[3]), float.Parse(messages[4]));
                            SendMessage(message, _clientList[no].type);
                        }
                        catch (FormatException e)
                        {
                            SendMessage("Error:Data Type Error", no);
                        }
                    }
                    else
                    {
                        SendMessage("Error:No find data", no);
                    }
                }
                else
                {
                    SendMessage("Error:No have type", no);
                }
            }
            else if (messages[0] == "add")
            {
                if (_clientList[no].type != "none")
                {
                    try
                    {
                        float x = float.Parse(messages[2]);
                        float y = float.Parse(messages[3]);
                        float z = float.Parse(messages[4]);
                        string Data = _LF.InsetData(_clientList[no].type, "Type,X,Y,Z", "'" + messages[1] + "'," + x.ToString() + "," + y.ToString() + "," + z.ToString());
                        SendMessage("add:" + Data, _clientList[no].type);
                    }
                    catch (FormatException e)
                    {
                        SendMessage("Error:Data Type Error", no);
                    }
                }
                else
                {
                    SendMessage("Error:No have type", no);
                }
            }

        }

        public void StopServer()
        {
            myTcpListener.Stop();
            try
            {
                clientSocket.Close();
            }
            catch (Exception e) { }
            try
            {
                startServer.Abort();
                startServer.Join();
                startServer = null;
            }
            catch (Exception e) {
                
            }
           

            Log.WriteTime("Info:Sever服務已被關閉");
        }
        ~Server()
        {
            StopServer();
        }
    }
}
