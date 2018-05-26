using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

namespace client
{
    public class Client
    {
        //宣告網路資料流變數
        NetworkStream myNetworkStream;
        //宣告 Tcp 用戶端物件
        TcpClient myTcpClient;
        //宣告執行續
        Thread _readData;

        List<string> _messageList;

        string _hostName;
        int _connectPort;

        //初始化Client端
        public Client(string hostName = "127.0.0.1", int connectPort = 36000)
        {
            _hostName = hostName;
            _connectPort = connectPort;
            _messageList = new List<string>();
        }

        public void StartClient()
        {
            myTcpClient = new TcpClient();

            //測試連線至遠端主機
            try
            {
                myTcpClient.Connect(_hostName, _connectPort);
                _readData = new Thread(new ThreadStart(ReadData));
                _readData.IsBackground = true;
                _readData.Start();
            }
            catch
            {
                throw new NetException("主機無法連接");
            }
        }

        //寫入資料
        public void WriteData(String strTest)
        {
            Byte[] myBytes = Encoding.ASCII.GetBytes(strTest);
            myNetworkStream = myTcpClient.GetStream();
            myNetworkStream.Write(myBytes, 0, myBytes.Length);
            //myNetworkStream.Close();
        }

        //讀取資料
        void ReadData()
        {
            while (true)
            {
                myNetworkStream = myTcpClient.GetStream();
                int bufferSize = myTcpClient.ReceiveBufferSize;
                byte[] myBufferBytes = new byte[bufferSize];
                myNetworkStream.Read(myBufferBytes, 0, bufferSize);
                int i = 0;
                string message = "";
                while (myBufferBytes[i] != 0)
                {
                    message += Encoding.ASCII.GetString(myBufferBytes, i, 1);
                    i++;
                }
                _messageList.Add(message);
            }
        }

        public void StopClient()
        {
            try
            {
                myNetworkStream.Close();
                myTcpClient.Close();
                _readData.Abort();
                _readData.Join();
                _readData = null;
            }
            catch (Exception e)
            {
                throw new NetException(e.Message);
            }
        }

        public string ReadMessage()
        {
            if (_messageList.Count <= 0)
                throw new NetException("message list內無任何訊息");
            string message = _messageList[0];
            _messageList.RemoveAt(0);
            return message;
        }

        public int GetMessageListCount()
        {
            return _messageList.Count;
        }
    }
}
