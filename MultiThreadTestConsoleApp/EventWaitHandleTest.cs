using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 测试EventWaitHandle及其派生类
    /// AutoResetEvent ManualResetEvent
    /// </summary>
    public class EventWaitHandleTest
    {
        public static void TestRun()
        {
            //TestChild1();
            //TestChild2();
            TestChild3();
        }
        #region AutoResetEvent
        private static AutoResetEvent autoEvent1 = new AutoResetEvent(false);
        private static void TestChild1()
        {
            Console.WriteLine("Main Thread Start run at: " + DateTime.Now.ToLongTimeString());
            Thread t1 = new Thread(() =>
              {
                  if (autoEvent1.WaitOne(1000))
                  {
                      Console.WriteLine("Get Singal to Work");
                      // 3秒后线程可以运行，所以此时显示的时间应该和主线程显示的时间相差一秒
                      Console.WriteLine("Method Restart run at: " + DateTime.Now.ToLongTimeString());
                  }
                  else
                  {
                      Console.WriteLine("Time Out to work");
                      Console.WriteLine("Method Restart run at: " + DateTime.Now.ToLongTimeString());
                  }
              });
            t1.Start();
            Thread.Sleep(3000);
            // Set 方法就是把事件状态设置为终止状态。
            autoEvent1.Set();
            Console.ReadKey();
        }
        #endregion
        #region ManualResetEvent
        private static ManualResetEvent manualEvent1 = new ManualResetEvent(true);
        private static void TestChild2()
        {
            Console.WriteLine("Main Thread Start run at: " + DateTime.Now.ToLongTimeString());
            Thread t = new Thread(() =>
            {
                // 初始状态为终止状态，则第一次调用WaitOne方法不会堵塞线程
                // 此时运行的时间间隔应该为0秒，但是因为是AutoResetEvent对象
                // 调用WaitOne方法后立即把状态返回为非终止状态。
                manualEvent1.WaitOne();
                Console.WriteLine("Method start at : " + DateTime.Now.ToLongTimeString());

                // 因为此时AutoRestEvent为非终止状态，所以调用WaitOne方法后将阻塞线程1秒，这里设置了超时时间
                // 所以下面语句的和主线程中语句的时间间隔为1秒
                // 当时 ManualResetEvent对象时，因为不会自动重置状态
                // 所以调用完第一次WaitOne方法后状态仍然为非终止状态,所以再次调用不会阻塞线程，所以此时的时间间隔也为0
                // 如果没有设置超时时间的话，下面这行语句将不会执行
                manualEvent1.WaitOne(1000);
                Console.WriteLine("Method start at : " + DateTime.Now.ToLongTimeString());
            });
            t.Start();
            Console.ReadKey();
        }
        #endregion
        #region EventWaitHandle 
        private static EventWaitHandle ewh = new EventWaitHandle(true, EventResetMode.AutoReset, "fwq1");
        private static void TestChild3()
        {
            Console.WriteLine("Main Thread Start run at: " + DateTime.Now.ToLongTimeString());
            Thread t1 = new Thread(() =>
              {
                  ewh.WaitOne(1000);
                  Console.WriteLine("Method start at : " + DateTime.Now.ToLongTimeString());
              });
            Thread.Sleep(2000);
            t1.Start();
            Console.ReadKey();
        }
        #endregion
    }
}
