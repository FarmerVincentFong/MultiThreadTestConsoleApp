using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncAndAwaitTest
{
    public partial class AsynchronousProgrammingModelForm1 : Form
    {
        //定义用来实现异步编程的委托
        private delegate string AsyncMethodCaller(string url);
        //同步上下文
        private SynchronizationContext sc;
        /// <summary>
        /// 测试异步编程模型APM
        /// </summary>
        public AsynchronousProgrammingModelForm1()
        {
            InitializeComponent();
            tbxUrl.Text = @"http://download.microsoft.com/download/7/0/3/703455ee-a747-4cc8-bd3e-98a615c3aedb/dotNetFx35setup.exe";
            //// 允许跨线程调用
            //// 实际开发中不建议这样做的，（违背了.NET 安全规范）
            //CheckForIllegalCrossThreadCalls = false;
        }

        //点击后异步下载内容到文件
        private void dlbtn1_Click(object sender, EventArgs e)
        {
            if (tbxUrl.Text == string.Empty)
            {
                MessageBox.Show("Please input valid download file url");
                return;
            }
            btnDownload1.Enabled = false;
            rtbState.Text = "发送请求，下载中！！！";
            ////发送同步请求
            //string result = DownLoadFileSync(tbxUrl.Text);
            //ShowState(result);
            //发送异步请求
            AsyncMethodCaller amc1 = new AsyncMethodCaller(DownLoadFileSync);
            //开始异步调用
            amc1.BeginInvoke(tbxUrl.Text, new AsyncCallback(GetResultCB), null);
            // 捕捉调用线程的同步上下文派生对象
            sc = SynchronizationContext.Current;
        }

        // 同步下载文件的方法
        // 该方法会阻塞主线程，使用户无法对界面进行操作
        // 在文件下载完成之前，用户甚至都不能关闭运行的程序。
        private string DownLoadFileSync(string url)
        {
            // Create an instance of the RequestState 
            RequestState requestState = new RequestState();
            requestState.filestream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\fwqtest.exe", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
            try
            {
                //发送请求，并将响应写入文件
                HttpWebRequest request = WebRequest.CreateHttp(url);
                requestState.request = request;
                requestState.response = (HttpWebResponse)request.GetResponse();
                requestState.streamResponse = requestState.response.GetResponseStream();
                //将响应流写入文件中
                int readsize = requestState.streamResponse.Read(requestState.BufferRead, 0, requestState.BufferRead.Length);
                while (readsize > 0)
                {
                    requestState.filestream.Write(requestState.BufferRead, 0, readsize);
                    readsize = requestState.streamResponse.Read(requestState.BufferRead, 0, requestState.BufferRead.Length);
                }
                // 执行该方法的线程是线程池线程，该线程不是与创建richTextBox控件的线程不是一个线程
                // 如果不把 CheckForIllegalCrossThreadCalls 设置为false，该程序会出现“不能跨线程访问控件”的异常
                return string.Format("The Length of the File is: {0}", requestState.filestream.Length) + string.Format("\nDownLoad Completely, Download path is: {0}", requestState.savepath);
            }
            catch (Exception ex)
            {
                return string.Format("Exception occurs in DownLoadFileSync method, Error Message is:{0}", ex.Message);
            }
            finally
            {
                requestState.response.Close();
                requestState.filestream.Close();
            }
        }

        //异步操作完成回调
        private void GetResultCB(IAsyncResult result)
        {
            //获取异步调用的委托对象
            AsyncMethodCaller amc1 = (AsyncMethodCaller)((AsyncResult)result).AsyncDelegate;
            string resultStr = amc1.EndInvoke(result);
            // 通过获得GUI线程的同步上下文的派生对象，
            // 然后调用Post方法来使更新GUI操作方法由GUI 线程去执行
            sc.Post(ShowState, resultStr);
        }

        //显示请求返回结果
        private void ShowState(object result)
        {
            rtbState.Text = result.ToString();
            btnDownload1.Enabled = true;
        }
    }
}
