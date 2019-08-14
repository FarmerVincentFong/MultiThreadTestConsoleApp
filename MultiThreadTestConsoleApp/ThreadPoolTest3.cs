using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTestConsoleApp
{
    public class ThreadPoolTest3
    {
        /// <summary>
        /// 测试Task<T> 使用任务实现异步
        /// </summary>
        public static void TestRun()
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationTokenSource cts2 = new CancellationTokenSource();
            PrintMessage("Main Thread Start");
            // 调用构造函数创建Task对象
            //Task<int> t1 = new Task<int>(obj => AsyncMethodTest1((int)obj, cts.Token), 10);
            Task<int> t1 = new Task<int>(AsyncMethodTest1, 10, cts2.Token);
            // 启动任务 
            t1.Start();
            //延迟取消任务
            Thread.Sleep(3000);
            cts2.Cancel();
            //取消任务
            //cts.Cancel();
            //// 等待任务完成
            //t1.Wait();
            Console.WriteLine("The Method result is: " + t1.Result);
            Console.ReadKey();
        }

        /// <summary>
        /// 使用任务工厂来进行异步操作
        /// </summary>
        public static void TestRun2()
        {
            ThreadPool.SetMaxThreads(1000, 1000);
            PrintMessage("Main Thread Start");
            Task.Factory.StartNew(() =>
            {
                PrintMessage("Async M1 Start");
            });
            Thread.Sleep(500);
            Console.WriteLine("Method Thread End");
            Console.ReadKey();

        }

        /// <summary>
        /// 任务Task的关联方法
        /// </summary>
        /// <returns></returns>
        private static int AsyncMethodTest1(object n2)
        {
            int n = (int)n2;
            Thread.Sleep(1000);
            PrintMessage("Asynchoronous Method Start");
            int sum = 0;
            try
            {
                for (int i = 1; i < n; i++)
                {
                    // 当CancellationTokenSource对象调用Cancel方法时，
                    // 就会引起OperationCanceledException异常
                    // 通过调用CancellationToken的ThrowIfCancellationRequested方法来定时检查操作是否已经取消，
                    // 这个方法和CancellationToken的IsCancellationRequested属性类似
                    //ct.ThrowIfCancellationRequested();
                    Thread.Sleep(500);
                    // 如果n太大，使用checked使下面代码抛出异常
                    checked
                    {
                        sum += i;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception is:" + ex.GetType().Name);
                Console.WriteLine("Operation is Canceled");
            }
            return sum;
        }

        /// <summary>
        /// 打印线程池信息，和当前线程信息
        /// </summary>
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
