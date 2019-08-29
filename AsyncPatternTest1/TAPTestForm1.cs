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
using System.Net;
using System.IO;

namespace AsyncPatternTest1
{
    /// <summary>
    /// 文件下载工具，支持断点续传和暂停
    /// </summary>
    public partial class TAPTestForm1 : Form
    {
        private long DownloadSize;
        private SynchronizationContext sc;  //同步上下文
        private FileStream fileStream;  //下载文件流
        private long totalSize;  //要下载的文件总大小
        private string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "fwqTapFiles.txt");
        private CancellationTokenSource cts;
        public TAPTestForm1()
        {
            InitializeComponent();
            //默认下载路径
            tbxUrl.Text = @"http://download.microsoft.com/download/7/0/3/703455ee-a747-4cc8-bd3e-98a615c3aedb/dotNetFx35setup.exe";
            tbxUrl.Text = @"http://u3.xainjo.com/apk/azqlds.apk";
        }

        //下载操作
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            richTextBox1.Text += $"开始下载文件！初始下载大小：{DownloadSize}  |{Thread.CurrentThread.ManagedThreadId}|\n";
            GetTotalSize(); //先获取文件总大小
            richTextBox1.Text += $"要下载文件的总大小为：{totalSize}\n";
            fileStream = new FileStream(downloadPath, FileMode.OpenOrCreate);
            fileStream.Seek(DownloadSize, SeekOrigin.Begin); //断点续传，偏移
            // 捕捉调用线程的同步上下文派生对象
            sc = SynchronizationContext.Current;
            cts = new CancellationTokenSource();
            // 使用指定的操作初始化新的 Task。
            // Task at = new Task(() =>
            // {
            //     //Progress<int>来定义进度报告回调
            //     DownloadFileTaskAsync(tbxUrl.Text, cts.Token, new Progress<int>(
            //         val =>
            //         {
            //             sc.Post((state) =>
            //{
            //    progressBar1.Value = val;
            //}, null);
            //         }
            //     ));
            // });

            Task at2 = new Task(() => Actionmethod(cts.Token), cts.Token);
            // 启动 Task，并将它安排到当前的 TaskScheduler 中执行。 
            at2.Start();
        }
        //暂停操作
        private void button2_Click(object sender, EventArgs e)
        {
            // 发出一个取消请求
            cts.Cancel();
        }

        // 任务中执行的方法
        private void Actionmethod(CancellationToken ct)
        {
            // 使用同步上文文的Post方法把更新UI的方法让主线程执行
            string str = $"Actionmethod |{Thread.CurrentThread.ManagedThreadId}|\n";
            this.Invoke(new Action(() => { richTextBox1.Text += str; }));
            DownloadFileTaskAsync(tbxUrl.Text, ct, new Progress<int>(val =>
            {
                sc.Post(state =>
                {
                    progressBar1.Value = (int)state;
                }, val);
            }));
        }
        // Get Total Size of File
        private void GetTotalSize()
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(tbxUrl.Text.Trim());
            HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
            totalSize = response.ContentLength;
            response.Close();
        }
        /// <summary>
        /// 使用TAP的异步操作（异步下载）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ct">取消请求标识</param>
        /// <param name="progress">负责进度报告</param>
        private void DownloadFileTaskAsync(string url, CancellationToken ct, IProgress<int> progress)
        {
            string str1 = $"开始异步下载操作！ |{Thread.CurrentThread.ManagedThreadId}|\n";
            sc.Post(new SendOrPostCallback((o => richTextBox1.Text += str1)), null);
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream responseStream = null;
            int bufferSize = 2048;
            byte[] bufferBytes = new byte[bufferSize];
            try
            {
                request = WebRequest.CreateHttp(tbxUrl.Text);
                //端点续传
                if (DownloadSize != 0)
                {
                    request.AddRange(DownloadSize);
                }

                response = (HttpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();
                int readSize = 0;
                //开始读取响应流，并将其写入文件流
                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        // 收到取消请求则退出异步操作
                        response.Close();
                        fileStream.Close();
                        string str2 = $"下载暂停， 已经下载的字节数为: {DownloadSize}字节 |{Thread.CurrentThread.ManagedThreadId}|\n";
                        //改变按钮状态
                        sc.Post((o =>
                        {
                            this.button1.Enabled = true;
                            this.button2.Enabled = false;
                            richTextBox1.Text += str2;
                        }), null);
                        // 退出异步操作
                        break;
                    }
                    readSize = responseStream.Read(bufferBytes, 0, bufferBytes.Length);
                    if (readSize > 0)
                    {
                        //读到数据，写入文件流
                        fileStream.Write(bufferBytes, 0, readSize);
                        DownloadSize += readSize;
                        //报告进度
                        int percentComplete = (int)((double)DownloadSize / (double)totalSize * 100);
                        progress.Report(percentComplete);
                    }
                    else
                    {
                        string str3 = $"下载已完成，下载的文件地址为：{downloadPath}，文件的总字节数为: {totalSize}字节 \n";
                        //下载完成
                        sc.Post(state =>
                        {
                            this.button1.Enabled = true;
                            this.button2.Enabled = false;
                            richTextBox1.Text += str3;
                        }, null);
                        response.Close();
                        fileStream.Close();
                        break;
                    }
                }

            }
            catch (AggregateException ex)
            {
                // 因为调用Cancel方法会抛出OperationCanceledException异常
                // 将任何OperationCanceledException对象都视为以处理
                ex.Handle(e => e is OperationCanceledException);
            }
        }
    }
}
