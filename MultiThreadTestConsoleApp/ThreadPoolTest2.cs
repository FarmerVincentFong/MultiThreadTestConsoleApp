using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTestConsoleApp
{
    public class ThreadPoolTest2
    {
        /// <summary>
        /// 测试委托实现异步，异步编程模型APM
        /// </summary>
        public static void TestRun()
        {
            ThreadPool.SetMaxThreads(500, 500);
            PrintMessage("Main Thread Start");
            //使用委托异步
            MyTestDelegate d1 = new MyTestDelegate(AsyncMethod1);
            // 异步调用委托
            IAsyncResult result = d1.BeginInvoke((ar)=>Console.WriteLine($"{ar.AsyncState}\n{ar.CompletedSynchronously}\n{ar.IsCompleted}"), null);
            // 获取结果并打印出来
            //string strData = d1.EndInvoke(result);
            //Console.WriteLine(strData);
            Console.WriteLine("Main Thread End");
            Console.ReadKey();
        }

        private delegate string MyTestDelegate();

        //异步执行的测试方法，带返回结果
        private static string AsyncMethod1()
        {
            Thread.Sleep(1000);
            PrintMessage("FWQ Asynchoronous Method");
            return "FWQ Method has completed";
        }

        private static void PrintMessage(string data)
        {
            int workthreadnumber;
            int iothreadnumber;
            // 获得线程池中可用的线程，把获得的可用工作者线程数量赋给workthreadnumber变量
            // 获得的可用I/O线程数量给iothreadnumber变量
            ThreadPool.GetAvailableThreads(out workthreadnumber, out iothreadnumber);
            Console.WriteLine("Msg: {0}\n CurrentThreadId is {1}\n CurrentThread is background :{2}\n WorkerThreadNumber is:{3}\n IOThreadNumbers is: {4}\n",
               data,
               Thread.CurrentThread.ManagedThreadId,
               Thread.CurrentThread.IsBackground.ToString(),
               workthreadnumber.ToString(),
               iothreadnumber.ToString());
        }
    }
}
