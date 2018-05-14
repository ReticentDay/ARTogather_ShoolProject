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
        public delegate void PrintHandler(TextBox tb, string text);
        public static void Print(TextBox tb, string text)
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (tb.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                PrintHandler ph = new PrintHandler(Print);
                tb.Invoke(ph, tb, text);
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                tb.Text = tb.Text + text + Environment.NewLine;
                tb.ScrollBars = ScrollBars.Vertical;
                tb.SelectionStart = tb.Text.Length;
                tb.ScrollToCaret();
            }
        }

        static public void SetLogShow(TextBox textbox)
        {
            _textbox = textbox;
        }
        static public void Write(string info)
        {
            Print(_textbox, info);
        }
        static public void WriteTime(string info)
        {
            info = "【" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "】" + info;
            Print(_textbox,info);
        }
    }
}
