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
        Center _center;
        broadcast _broadcast;
        LoadFile LF;

        public Form1(Center center,broadcast broadcast)
        {
            InitializeComponent();
            Log.SetLogShow(logText);
            _center = center;
            _broadcast = broadcast;
            LF = new LoadFile();
            //LF.CreatFile("GGININ");
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _center.start();
            //logText.Text = "伺服器啟動!";
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            _center.stop();
        }

        private void broadcastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _broadcast.Show();
        }
    }
}
