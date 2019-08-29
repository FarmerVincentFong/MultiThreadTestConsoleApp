using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace AsyncPatternTest1
{
    public partial class EAPTest : Form
    {
        private SynchronizationContext sc;
        /// <summary>
        /// 测试 基于事件的异步模式
        /// （模拟下载操作，可暂停，可报告进度）
        /// 使用BackgroundWorker
        /// </summary>
        public EAPTest()
        {
            InitializeComponent();
        }

        //开始下载
        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                button1.Enabled = false;
                button2.Enabled = true;
                sc = SynchronizationContext.Current;
                //开始异步操作
                backgroundWorker1.RunWorkerAsync("fwq argument");
                richTextBox1.Text += $"开始异步下载！！！|{Thread.CurrentThread.ManagedThreadId}|\n";
            }

        }
        //暂停下载
        private void button2_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.CancellationPending && backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
        }


        #region BackGroundWorker Event
        //DoWork事件句柄
        //此事件处理方法在工作者线程中运行
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = (BackgroundWorker)sender;
            //在新线程执行
            for (var i = 0; i < 20; i++)
            {
                if (bgWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                //if (i == 15)
                //{
                //    throw new Exception("FWQ Cus EXp");
                //}
                Thread.Sleep(500);
                string msg = $"下载一部分，|{Thread.CurrentThread.ManagedThreadId}|\n";
                //显示信息
                //richTextBox1.Invoke(new Action(() => richTextBox1.Text += msg));
                sc.Post(new SendOrPostCallback(ele => { richTextBox1.Text += msg;richTextBox1.Text += ele.ToString(); }),"同步上下文使用");
                //报告进度
                bgWorker.ReportProgress((i + 1)* 5 );
            }
        }

        //ProgressChanged事件句柄
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string msg = $"报告进度，|{Thread.CurrentThread.ManagedThreadId}|\n";
            this.richTextBox1.Text += msg;
            this.progressBar1.Value = e.ProgressPercentage;
        }

        //RunWorkerCompleted事件句柄
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            string msg;
            if (e.Cancelled)
            {
                msg = $"运行被取消，|{Thread.CurrentThread.ManagedThreadId}|\n";
            }else if (e.Error != null)
            {
                msg = $"运行报错，{e.Error.Message}\n|{Thread.CurrentThread.ManagedThreadId}|\n";
            }else
            {
                msg = $"运行结束，|{Thread.CurrentThread.ManagedThreadId}|\n";
            }
            richTextBox1.Text += msg;
        }
        #endregion
    }
}
