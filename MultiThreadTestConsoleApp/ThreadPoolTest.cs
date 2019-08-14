using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 线程池测试1
    /// </summary>
    public class ThreadPoolTest
    {
        public static void TestRun()
        {
            // 设置线程池中处于活动的线程的最大数目
            // 设置线程池中工作者线程数量为1000，I/O线程数量为1000
            ThreadPool.SetMaxThreads(2000, 2000);
            Console.WriteLine("Main Thread: queue an asynchronous method");
            PrintMessage("Main Thread Start");

            // 把工作项添加到队列中，此时线程池会用工作者线程去执行回调方法
            ThreadPool.QueueUserWorkItem(asyncMethod, "fwq");
            Console.Read();
        }
        // 方法必须匹配WaitCallback委托
        private static void asyncMethod(object state)
        {
            Thread.Sleep(1000);
            PrintMessage("Asynchoronous Method");
            Console.WriteLine("Asynchoronous thread has worked " + state.ToString());
        }

        // 打印线程池信息
        private static void PrintMessage(string data)
        {
            int workthreadnumber;
            int iothreadnumber;

            // 获得线程池中可用的线程，把获得的可用工作者线程数量赋给workthreadnumber变量
            // 获得的可用I/O线程数量给iothreadnumber变量
            ThreadPool.GetAvailableThreads(out workthreadnumber, out iothreadnumber);

            Console.WriteLine("{0}\n CurrentThreadId is {1}\n CurrentThread is background :{2}\n WorkerThreadNumber is:{3}\n IOThreadNumbers is: {4}\n",
                data,
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.IsBackground.ToString(),
                workthreadnumber.ToString(),
                iothreadnumber.ToString());
        }

        /// <summary>
        /// 使用CancellationTokenSource 取消线程池工作（任务）
        /// </summary>
        public static void TestRun2()
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            Console.WriteLine("Main thread run");
            PrintMessage("Start");
            Run();  //开始计数
            Console.ReadKey();
        }

        private static void Run()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.WriteLine("Press Enter key to cancel the operation\n");
            // 这里用Lambda表达式的方式和使用委托的效果一样的，只是用了Lambda后可以少定义一个方法。
            // 这在这里就是让大家明白怎么lambda表达式如何由委托转变的
            ThreadPool.QueueUserWorkItem(o =>
            {
                PrintMessage("Asynchoronous Method Start");
                Count(cts.Token, 1000); //cts使用了闭包
            });
            //ThreadPool.QueueUserWorkItem(callback, cts.Token);
            Console.ReadLine();

            // 传达取消请求
            cts.Cancel();
        }
        // 执行的操作，当受到取消请求时停止数数
        private static void Count(CancellationToken token, int countto)
        {
            for (int i = 0; i < countto; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Count is canceled");
                    break;
                }

                Console.WriteLine(i);
                Thread.Sleep(300);
            }

            Console.WriteLine("Count has done");
        }
    }

}
