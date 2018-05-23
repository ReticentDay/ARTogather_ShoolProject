using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace clienttest
{
    class Program
    {
        //宣告網路資料流變數
        NetworkStream myNetworkStream;
        //宣告 Tcp 用戶端物件
        TcpClient myTcpClient;

        static void Main(string[] args)
        {
            Program myNetworkClient = new Program();

            //取得主機名稱
            string hostName ="127.0.0.1";
            //取得連線 IP 位址
            int connectPort = 36000;
            //建立 TcpClient 物件
            myNetworkClient.myTcpClient = new TcpClient();
            try
            {
                //測試連線至遠端主機
                myNetworkClient.myTcpClient.Connect(hostName, connectPort);
                Console.WriteLine("連線成功 !!\n");
            }
            catch
            {
                Console.WriteLine
                           ("主機 {0} 通訊埠 {1} 無法連接  !!", hostName, connectPort);
                
            }
            while(true)
            {
                String strings = Console.ReadLine();
                //if(strings == "Exit")
                //{
                //    break;
                //}
                myNetworkClient.WriteData(strings);
                //myNetworkClient.ReadData();
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
                Console.Write(Encoding.ASCII.GetString(myBufferBytes,i,1));
                i++;
            }
            
        }
    }
}
