using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 测试EAP转换成TAP
    /// </summary>
    public class TAPOrAPMOrEAPTest2
    {
        static Uri targetUri = new Uri("http://msdn.microsoft.com/zh-CN/");
        public static void TestRun()
        {
            //EAPWay();
            EAPConvertToTAP();
            Console.ReadKey();
        }
        #region 普通使用EAP
        //使用WebClient
        private static void EAPWay()
        {
            Console.WriteLine($"开始同步下载！ |{Thread.CurrentThread.ManagedThreadId}|");
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(targetUri, "fwq msg");
            wc.DownloadStringCompleted += (sender, e) =>
            {
                Console.WriteLine($"异步下载操作完成！结果：[{e.UserState}] {e.Result.Length} |{Thread.CurrentThread.ManagedThreadId}|");
            };
            //Console.WriteLine($"同步下载完成！结果：{respData.Length}");
        }
        #endregion
        //eap转为使用tap 
        private static void EAPConvertToTAP()
        {
            Console.WriteLine($"开始同步下载！ |{Thread.CurrentThread.ManagedThreadId}|");
            WebClient wc = new WebClient();
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            wc.DownloadStringCompleted += (sender, e) =>
            {
                Console.WriteLine($"WebClient的下载完成事件！ |{Thread.CurrentThread.ManagedThreadId}|");
                if (e.Error != null)
                {
                    //报错
                    tcs.TrySetException(e.Error);
                }
                else if (e.Cancelled)
                {
                    //取消
                    tcs.TrySetCanceled();
                }
                else
                {
                    //完成
                    tcs.TrySetResult(e.Result);
                }
            };
            // 当Task完成时继续下面的Task,显示Task的状态
            // 为了让下面的任务在GUI线程上执行，必须标记为TaskContinuationOptions.ExecuteSynchronously
            // 如果没有这个标记，任务代码会在一个线程池线程上运行
            tcs.Task.ContinueWith(t =>
            {
                Console.WriteLine($"在Task的ContinueWith中执行！ |{Thread.CurrentThread.ManagedThreadId}|");
                if (t.IsCanceled)
                {
                    Console.WriteLine("任务被取消！");
                }
                else if (t.IsFaulted)
                {
                    Console.WriteLine($"异常发生，异常信息为：{t.Exception.GetBaseException().Message}");
                }
                else
                {
                    Console.WriteLine(String.Format("操作已完成，结果为：{0}", t.Result.Length));
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            //开始异步操作
            wc.DownloadStringAsync(targetUri);
        }
    }
}
