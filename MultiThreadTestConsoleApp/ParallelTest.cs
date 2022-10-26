using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 并行编程的测试。
    /// </summary>
    /// <remarks>
    /// Parallel:是并行编程，在Task的基础上做了封装，.NET FrameWork 4.5之后的版本可用，调用Parallel线程参与执行任务。
    /// 与Task区别: 使用Task开启子线程的时候，主线程是属于空闲状态，并不参与执行;
    /// Parallel开启子线程的时候，主线程也会参与计算。
    /// </remarks>
    public class ParallelTest
    {
        //一个比较耗时的方法,循环1000W次
        private static void DoSomething(string name)
        {
            int iResult = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                iResult += i;
            }
            Console.WriteLine($"{name},线程Id:{Thread.CurrentThread.ManagedThreadId},{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}" + Environment.NewLine);
        }

        public static void TestRun()
        {
            //并行编程 
            Console.WriteLine($"并行编程开始，主线程Id:{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("【示例1】");
            //示例1：
            //一次性执行1个或多个线程，效果类似：Task WaitAll，只不过Parallel的主线程也参与了计算
            Parallel.Invoke(() => { DoSomething("并行1-1"); },
                () => { DoSomething("并行1-2"); },    //并行5个操作
                () => { DoSomething("并行1-3"); },
                () => { DoSomething("并行1-4"); },
                () => { DoSomething("并行1-5"); }
                );

            Console.WriteLine("*************并行结束************");
            Console.ReadLine();

            //得出结论：Parallel开启子线程的时候，主线程也会参与计算。
        }

        public static void TestRun2()
        {
            //并行编程 
            Console.WriteLine($"并行编程开始，主线程Id:{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("【示例2】");
            //示例2：
            //定义要执行的线程数量
            Parallel.For(0, 5, (i) =>
            {
                DoSomething($"并行2-{i}");
            });

            Console.WriteLine("*************并行结束************");
            Console.ReadLine();

            //结论：并行可定义迭代（循环）执行多个线程（操作）。
        }

        public static void TestRun3()
        {
            //并行编程 
            Console.WriteLine($"并行编程开始，主线程Id:{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("【示例3】");

            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 3  //最大并行度，执行线程的最大并发数量。即执行完成一个，就接着开启一个
            };

            List<string> strings = new List<string> { "a", "b", "c", "d", "e", "f", "g" };
            //遍历集合，根据集合数量决定启动的线程数量
            Parallel.ForEach(strings, options, (ele, state) =>
            {
                //state.Break();  //尽早停止当前迭代之外的迭代。
                //state.Stop();   //整个Parallel结束掉。Break和Stop不可以共存

                //if (ele == "d")
                //    state.Stop(); //实测：感觉break和stop挺像的，不好区分。

                DoSomething($"并行3-{ele}");
            });

            Console.WriteLine("*************并行结束************");
            Console.ReadLine();

            //结果：可以看到信息是三个、三个的输出，因为限制了最大并发数为3。
        }
    }
}
