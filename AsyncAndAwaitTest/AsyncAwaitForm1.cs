using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;

namespace AsyncAndAwaitTest
{
    public partial class form1 : Form
    {
        public form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 普通同步发送例子
        /// UI线程被阻塞
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            richTextBox1.Text += "等待服务器回复！！！";
            long len = AccessWeb(); //发请求
            this.button1.Enabled = true;
            //做些不需要阻塞的操作

            this.richTextBox1.Text += String.Format("\n 回复的字节长度为:  {0}.\r\n", len);
            textBox1.Text = Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private string testUrl = "http://www.techtimesun.com/";

        private long AccessWeb()
        {
            MemoryStream ms = new MemoryStream();
            //发送请求
            HttpWebRequest request = WebRequest.CreateHttp(testUrl);
            Thread.Sleep(3000); //模拟延迟
            //获取响应
            using (WebResponse response = request.GetResponse())
            {
                //获取响应流
                using (Stream s = response.GetResponseStream())
                {
                    s.CopyTo(ms);
                }
            }
            textBox2.Text = Thread.CurrentThread.ManagedThreadId.ToString();
            return ms.Length;
        }
    }
}
