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
        ManualResetEvent _shutdownEvent;
        
        Socket clientSocket;
        Thread clientThread;
        int No;
        public void startClient(Socket clientSocket, int No)
        {
            this.clientSocket = clientSocket;
            this.No = No;
            clientThread = new Thread(reply);
            clientThread.Start();
        }
        private void reply()
        {
            while (true)
            {
                try
                {
                    if (_shutdownEvent.WaitOne(0)) throw new Exception("中斷該使用者");
                    int dataLength;
                    byte[] myBufferBytes = new byte[1000];
                    //取得用戶端寫入的資料
                    dataLength = clientSocket.Receive(myBufferBytes);
                    //Log.WriteTime("取出用戶端寫入網路資料流的資料內容 :");
                    //Log.WriteTime(Encoding.ASCII.GetString(myBufferBytes, 0, dataLength) + "\n");
                }
                catch (Exception e)
                {
                    clientSocket.Close();
                   // Log.WriteTime("用戶(" + No + ")已中斷連線");
                   // Log.WriteTime("Error:"+e.Message);
                    break;
                }
            }
        }
        public void Stop()
        {
            _shutdownEvent.Set();
            clientThread.Join();
            clientThread = null;
        }
        ~handleClient()
        {
            Stop();
        }
    }
}
