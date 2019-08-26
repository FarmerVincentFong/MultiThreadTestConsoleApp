//======================================================================
// Copyright (C) 2010-2019 广州市宏光软件科技有限公司 
// 版权所有 
// 
// 文件名: SemaphoreAndMutexTest
// 创建人: 方文谦
// 创建时间: 2019/8/15 15:15:05
//======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 测试信号量和互斥锁
    /// </summary>
    public class SemaphoreAndMutexTest
    {
        public static void TestRun()
        {
            //TestChild1();
            TestChild2();
        }

        #region 信号量

        private static Semaphore sema1 = new Semaphore(4, 10,"fwq1");
        public static int time1 = 0;
        public static string msg1 = "信号量保护对象信息";
        private static void TestChild1()
        {
            for (var i = 0; i < 3; i++)
            {
                Thread t1 = new Thread((number) =>
                {
                    // 设置一个时间间隔让输出有顺序
                    int span = Interlocked.Add(ref time1, 100);
                    span = 500 * (int)number;
                    Thread.Sleep(1000 + span);
                    //信号量计数减1
                    sema1.WaitOne();
                    Console.WriteLine("Thread {0} run ;{1}", number, msg1);
                });
                t1.IsBackground = true;
                t1.Start(i);
            }
            // 等待1秒让所有线程开始并阻塞在信号量上
            Thread.Sleep(1000);
            // 信号量计数加4
            // 最后可以看到输出结果次数为4次
            //sema1.Release(1);
            Console.ReadKey();
        }
        #endregion

        #region Mutex

        private static Mutex m1 = new Mutex(false,"fwq2");
        public static int countNum;
        private static void TestChild2()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread test = new Thread(() =>
                {
                    m1.WaitOne();
                    Thread.Sleep(500);
                    countNum++;
                    Console.WriteLine("Current Cout Number is {0}", countNum);
                    m1.ReleaseMutex();
                });
                // 开始线程，并传递参数
                test.Start();
            }

            Console.Read();
        }

        #endregion
    }
}
