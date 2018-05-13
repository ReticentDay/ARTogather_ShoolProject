using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Server
{
    class Log
    {
        static System.Windows.Forms.TextBox _textbox;
        static public void setLogShow(TextBox textbox)
        {
            _textbox = textbox;
        }
        static public void Write(string info)
        {
            _textbox.Text += info + Environment.NewLine;
            _textbox.ScrollBars = ScrollBars.Vertical;
            _textbox.SelectionStart = _textbox.Text.Length;
            _textbox.ScrollToCaret();
        }
        static public void WriteTime(string info)
        {
                info = "【" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "】" + info;
                _textbox.Text += info + Environment.NewLine;
                _textbox.ScrollBars = ScrollBars.Vertical;
                _textbox.SelectionStart = _textbox.Text.Length;
                _textbox.ScrollToCaret();
        }
    }
}
