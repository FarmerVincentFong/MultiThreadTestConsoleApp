using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 线程基础一
    /// </summary>
    public class ThreadBaseTest
    {
        public static void TestRun()
        {
            //测试前台和后台线程
            //创建一个新线程（默认为前台线程）
            Thread backT1 = new Thread(BackWorker);
            backT1.IsBackground = true;
            backT1.Start();
            backT1.Join();  //暂停调用主线程，直到backT1结束
            Thread frontT2 = new Thread(obj =>
            {
                Thread.Sleep(1500);
                Console.WriteLine($"FrontWorker Do!!!!   {obj}");
            });
            frontT2.Start("fwqqqq");
            //主线程的操作
            Console.WriteLine("Main Thread Do!!!!!!");
            Console.ReadKey();
        }
        private static void BackWorker()
        {
            Thread.Sleep(3000);
            Console.WriteLine("BackWorker Do!!!!");
        }
    }
}
