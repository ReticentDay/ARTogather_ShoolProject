using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace Server
{
    class handleClient
    {
        
        Socket clientSocket;
        Thread clientThread;
        int No;
        public delegate void CallAndCatch(string messagem,int no);
        CallAndCatch _CAC;
        public void startClient(Socket clientSocket, int No,Server server)
        {
            _CAC = new CallAndCatch(server.CallAndCatch);
            this.clientSocket = clientSocket;
            this.No = No;
            clientThread = new Thread(reply);
            clientThread.IsBackground = true;
            clientThread.Start();
        }

        private void reply()
        {
            while (true)
            {
                try
                {
                    int dataLength;
                    byte[] myBufferBytes = new byte[1000];
                    //取得用戶端寫入的資料
                    dataLength = clientSocket.Receive(myBufferBytes);
                    Log.WriteTime("取出用戶端寫入網路資料流的資料內容 :");
                    Log.WriteTime(Encoding.ASCII.GetString(myBufferBytes, 0, dataLength) + "\n");
                    _CAC.Invoke(Encoding.ASCII.GetString(myBufferBytes, 0, dataLength),No);
                }
                catch (Exception e)
                {
                    clientSocket.Close();
                    Log.WriteTime("用戶(" + No + ")已中斷連線");
                    Log.WriteTime("Error:"+e.Message);
                    break;
                }
            }
            Stop();
        }
        public void SendMessage(string message)
        {
            byte[] myBufferBytes = Encoding.UTF8.GetBytes(message);
            try
            {
                clientSocket.Send(myBufferBytes, 0);
                Log.WriteTime("Info:向用戶(" + No + ")傳送訊息 " + message);
            }
            catch (Exception e)
            {
            }
        }
        public void Stop()
        {
            try
            {
                clientSocket.Close();
                clientThread.Abort();
                clientThread.Join();
                clientThread = null;
            }
            catch (Exception e)
            {
                Log.WriteTime("Error:" + e.Message);
            }
        }
        ~handleClient()
        {
            Stop();
        }
    }
}
