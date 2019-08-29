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
    public partial class form2 : Form
    {
        public form2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 使用Async与Await异步编程的例子
        /// UI线程不被阻塞，可响应控件
        /// </summary>
        private async void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            richTextBox1.Text += "等待服务器回复！！！";
            long len = await AccessWebAsync(); //发请求
            this.button1.Enabled = true;
            //做些不需要阻塞的操作

            this.richTextBox1.Text += String.Format("\n 回复的字节长度为:  {0}.\r\n", len);
            textBox1.Text = Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private string testUrl = "http://www.techtimesun.com/";

        /// <summary>
        /// 改为异步方法
        /// 使用async 和await定义异步方法不会创建新线程,
        /// </summary>
        private async Task<long> AccessWebAsync()
        {
            MemoryStream ms = new MemoryStream();
            //发送请求
            HttpWebRequest request = WebRequest.CreateHttp(testUrl);
            //Thread.Sleep(3000); //模拟延迟
            //获取响应
            using (WebResponse response = await request.GetResponseAsync())
            {
                //获取响应流
                using (Stream s = response.GetResponseStream())
                {
                    await s.CopyToAsync(ms);
                }
            }
            textBox2.Text = Thread.CurrentThread.ManagedThreadId.ToString();
            return ms.Length;
        }
    }
}
