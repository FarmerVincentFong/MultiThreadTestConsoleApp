using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 测试异步发送Web请求
    /// </summary>
    public class IOThreadTest2
    {
        public static void TestRun()
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            PrintMessage("Main Thread Start");
            //发送一个异步web请求
            WebRequest req = WebRequest.CreateHttp("http://www.cqzk.com.cn");
            req.BeginGetResponse((result) =>
            {
                //回调方法，结束异步操作End
                Thread.Sleep(500);
                PrintMessage("Asynchronous CallBack Method start");
                using (WebResponse resp = req.EndGetResponse(result))
                {
                    Console.WriteLine("Content Length is : " + resp.ContentLength);
                    var stream = resp.GetResponseStream();
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    Console.WriteLine($"读取的内容\n{sr.ReadToEnd()}");
                }
            }, null);
            Console.ReadKey();
        }
        // 打印线程池信息
        private static void PrintMessage(String data)
        {
            int workthreadnumber;
            int iothreadnumber;
            // 获得线程池中可用的线程，把获得的可用工作者线程数量赋给workthreadnumber变量
            // 获得的可用I/O线程数量给iothreadnumber变量
            ThreadPool.GetAvailableThreads(out workthreadnumber, out iothreadnumber);
            Console.WriteLine("FWQ: {0}\n CurrentThreadId is {1}\n CurrentThread is background :{2}\n WorkerThreadNumber is:{3}\n IOThreadNumbers is: {4}\n",
                data,
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.IsBackground.ToString(),
                workthreadnumber.ToString(),
                iothreadnumber.ToString());
        }
    }
}
