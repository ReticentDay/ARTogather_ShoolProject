using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;

namespace csharp_multi_threaded_server_socket
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.IPAddress theIPAddress;
            //建立 IPAddress 物件(本機)
            theIPAddress = System.Net.IPAddress.Parse("127.0.0.1");
            //建立監聽物件
            TcpListener myTcpListener = new TcpListener(theIPAddress, 36000);
            //啟動監聽
            myTcpListener.Start();
            Console.WriteLine("通訊埠 36000 等待用戶端連線...... !!");
            int counter = 0;
            do
            {
                Socket mySocket = myTcpListener.AcceptSocket();
                try
                {
                    //偵測是否有來自用戶端的連線要求，若是
                    //用戶端請求連線成功，就會秀出訊息。
                    if (mySocket.Connected)
                    {
                        counter++;
                        Console.WriteLine("用戶(" + counter + ")連線成功 !!");
                        handleClient client = new handleClient();
                        client.startClient(mySocket,counter);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    mySocket.Close();
                    Console.WriteLine("通訊埠 36000 等待用戶端連線...... !!");
                    mySocket = myTcpListener.AcceptSocket();
                }

            } while (true);
        }
    }
    public class handleClient
    {
        Socket clientSocket;
        int No;
        public void startClient(Socket clientSocket,int No)
        {
            this.clientSocket = clientSocket;
            this.No = No;
            Thread clientThread = new Thread(reply);
            clientThread.Start();
        }
        private void reply()
        {
            while (true) {
                try
                {
                    int dataLength;
                    byte[] myBufferBytes = new byte[1000];
                    //取得用戶端寫入的資料
                    dataLength = clientSocket.Receive(myBufferBytes);

                    Console.WriteLine("接收到的資料長度 {0} \n ", dataLength.ToString());
                    Console.WriteLine("取出用戶端寫入網路資料流的資料內容 :");
                    Console.WriteLine(Encoding.ASCII.GetString(myBufferBytes, 0, dataLength) + "\n");
                    Console.WriteLine("按下 [任意鍵] 將資料回傳至用戶端 !!");
                    Console.ReadLine();
                    //將接收到的資料回傳給用戶端
                    clientSocket.Send(myBufferBytes, dataLength, 0);
                    Console.WriteLine("已傳回資料");
                }
                catch (Exception e)
                {
                    clientSocket.Close();
                    Console.WriteLine("用戶("+No+")已中斷連線");
                    Console.WriteLine("通訊埠 36000 等待用戶端連線...... !!");
                    break;
                }
            }
        }
    }
}