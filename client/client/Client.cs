using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace client
{
    public class Client
    {
        //宣告網路資料流變數
        NetworkStream myNetworkStream;
        //宣告 Tcp 用戶端物件
        TcpClient myTcpClient;
        public void StartClient()
        {
            Thread readData;
            //取得主機名稱
            string hostName = "127.0.0.1";
            //取得連線 IP 位址
            int connectPort = 36000;
            //建立 TcpClient 物件
            myTcpClient = new TcpClient();
            readData = new Thread(new ThreadStart(ReadData));
            readData.Start();
            try
            {
                //測試連線至遠端主機
                myTcpClient.Connect(hostName, connectPort);
                Console.WriteLine("連線成功 !!\n");
            }
            catch
            {
                Console.Write("主機 {0} 通訊埠 {1} 無法連接  !!", hostName, connectPort);

            }
            while (true)
            {
                String strings = Console.ReadLine();
                WriteData(strings);
            }
        }

        //寫入資料
        void WriteData(String strTest)
        {
            //將字串轉 byte 陣列，使用 ASCII 編碼
            Byte[] myBytes = Encoding.ASCII.GetBytes(strTest);

            Console.WriteLine("建立網路資料流 !!");
            //建立網路資料流
            myNetworkStream = myTcpClient.GetStream();

            Console.WriteLine("將字串寫入資料流　!!");
            //將字串寫入資料流
            myNetworkStream.Write(myBytes, 0, myBytes.Length);
        }

        //讀取資料
        void ReadData()
        {
            Console.WriteLine("從網路資料流讀取資料 !!");
            //從網路資料流讀取資料
            myNetworkStream = myTcpClient.GetStream();
            int bufferSize = myTcpClient.ReceiveBufferSize;
            byte[] myBufferBytes = new byte[bufferSize];
            myNetworkStream.Read(myBufferBytes, 0, bufferSize);
            //取得資料並且解碼文字
            int i = 0;
            while (myBufferBytes[i] != 0)
            {
                Console.Write(Encoding.ASCII.GetString(myBufferBytes, i, 1));
                i++;
            }
        }
        public void GetData(string data) { }
    }
}
