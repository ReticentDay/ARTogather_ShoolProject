using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server;

namespace Server
{
    public partial class Form1 : Form
    {
        Server server = new Server();
        public Form1()
        {
            InitializeComponent();
            Log.SetLogShow(logText);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            server.StartServer();
            //logText.Text = "伺服器啟動!";
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            server.StopServer();
        }
    }
}
