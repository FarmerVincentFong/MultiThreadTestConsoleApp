using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 线程同步测试1：Interlocked、Monitor、ReaderWriterLock
    /// </summary>
    public class ThreadSynchronizedTest1
    {
        public static void TestRun()
        {
            //TestChild1();
            //TestChild2();
            //TestChild3();
            TestChild4();
        }

        #region Interlocked
        // 比较使用锁和不使用锁锁消耗的时间
        // 通过时间来说明使用锁性能的影响
        private static void TestChild1()
        {
            int x = 0;
            const int iterationNumber = 500000000;
            // 不采用锁的情况
            // StartNew方法 对新的 Stopwatch 实例进行初始化，将运行时间属性设置为零，然后开始测量运行时间。
            Stopwatch sw = Stopwatch.StartNew();
            for (var i = 0; i < iterationNumber; i++)
            {
                x++;
            }
            Console.WriteLine($"Use the all time is :{sw.ElapsedMilliseconds} ms");
            x = 0;
            sw.Restart();
            //使用锁的情况
            for (var i = 0; i < iterationNumber; i++)
            {
                Interlocked.Increment(ref x);
            }
            Console.WriteLine("Use the all time is :{0} ms", sw.ElapsedMilliseconds);
            Console.ReadKey();
        }

        private static void TestChild2()
        {
            //不加锁测试
            for (var i = 0; i < 150; i++)
            {
                Thread t = new Thread(AddTest1);
                t.Start();
            }
            Console.ReadKey();
        }
        // 共享资源
        public static int number1 = 0;
        public static void AddTest1()
        {
            Thread.Sleep(1000);
            Console.WriteLine("the current value of number is:{0}", Interlocked.Increment(ref number1));
        }
        #endregion
        #region Monitor
        private static object lockobj1 = new Object();
        private static int lockCount = 0;
        private static void TestChild3()
        {
            //测试monitor和lock关键字的使用
            for (var i = 0; i < 15; i++)
            {
                Thread tt = new Thread(() =>
                  {
                      Thread.Sleep(1000);
                      //获得排他锁
                      Monitor.Enter(lockobj1);
                      //lock (lockobj1)
                      //{
                      //下面代码也是同步执行的
                      lockCount += 10;
                      Console.WriteLine($"first:{lockobj1}       second:{lockCount}");
                      //}
                      Monitor.Exit(lockobj1);
                  });
                tt.Start();
            }
            Console.ReadKey();
        }
        #endregion
        #region ReaderWriterLock
        private static ReaderWriterLock rwl = new ReaderWriterLock();
        private static List<int> listInt = new List<int>();
        private static void TestChild4()
        {
            //创建一个线程写数据
            Thread tw1 = new Thread(() =>
              {
                  Thread.Sleep(110);
                  //写线程
                  Random ran = new Random();
                  int count = ran.Next(1, 100);
                  rwl.AcquireWriterLock(10);
                  listInt.Add(count);
                  Console.WriteLine("Write Data: " + count);
                  rwl.ReleaseWriterLock();
              });
            tw1.Start();

            //多个线程读数据
            for (var i = 0; i < 15; i++)
            {
                Thread tr = new Thread(() =>
                  {
                      //读线程
                      rwl.AcquireReaderLock(10);
                      foreach(var item in listInt)
                      {
                          Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} : {item}");
                      }
                      rwl.ReleaseReaderLock();
                  });
                tr.Start();
            }
            Console.ReadKey();
        }

        #endregion
    }
}
