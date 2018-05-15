﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Center allCenter = new Center();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            broadcast _broadcast = new broadcast(allCenter);
            Application.Run(new Form1(allCenter,_broadcast));
        }
    }
}
