using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class broadcast : Form
    {
        Center _center;
        public broadcast(Center center)
        {
            InitializeComponent();
            _center = center;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            _center.sendMessage(messageText.Text);
        }

        private void broadcast_Close(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
