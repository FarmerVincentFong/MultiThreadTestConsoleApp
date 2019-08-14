using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 测试线程的方法Interrupt，Abort
    /// </summary>
    public class ThreadBaseTest2
    {
        public static void TestRun()
        {
            Thread t1 = new Thread(TestMethod);
            t1.Start();
            Thread.Sleep(100);
            t1.Abort();
            Thread.Sleep(3000);
            Console.WriteLine("after finnally block, the Thread1 status is:{0}", t1.ThreadState);
            Console.ReadKey();
        }
        private static void TestMethod()
        {

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    Thread.Sleep(2000);
                    Console.WriteLine("Thread is Running");
                }
                catch (Exception e)
                {
                    if (e != null)
                    {
                        Console.WriteLine("Exception {0} throw ", e.GetType().Name);
                    }
                }
                finally
                {
                    Console.WriteLine("Current Thread status is:{0} ", Thread.CurrentThread.ThreadState);
                }
            }
        }
    }
}
