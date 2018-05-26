using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client;
using System.Threading;

namespace clienttest
{
    static class Program
    {
        static void Main()
        {
            Client myClist = new Client();
            try
            {
                myClist.StartClient();
                myClist.WriteData("try");
                //Thread.Sleep(1000);
                myClist.WriteData("try2");
                myClist.StopClient();
            }
            catch (NetException e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
