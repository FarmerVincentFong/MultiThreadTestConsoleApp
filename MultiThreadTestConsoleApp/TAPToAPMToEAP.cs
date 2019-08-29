//======================================================================
// Copyright (C) 2010-2019 广州市宏光软件科技有限公司 
// 版权所有 
// 
// 文件名: TAPToAPMToEAP
// 创建人: 方文谦
// 创建时间: 2019/8/22 14:01:08
//======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadTestConsoleApp
{
    public class TAPToAPMToEAP
    {
        public static void TestRun()
        {
            //APMWay();
            APMswitchToTAP();
            Console.ReadKey();
        }

        #region 使用APM的方式，及其转换为TAP的方法
        //典型APM使用
        private static void APMWay()
        {
            Console.WriteLine("使用APM");
            WebRequest webRq = WebRequest.Create("http://msdn.microsoft.com/zh-CN/");
            //apm异步编程
            webRq.BeginGetResponse(result =>
            {
                WebResponse webResp = null;
                try
                {
                    webResp = webRq.EndGetResponse(result);
                    Console.WriteLine($"请求的内容大小为： {webResp.ContentLength}");
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"异常发生，异常信息为： {ex.GetBaseException().Message}");
                }
                finally
                {
                    webResp?.Close();
                }
            }, null);
            Console.WriteLine("主线程显示的");
        }

        //使用FromAsync方法将APM转换为TAP
        private static void APMswitchToTAP()
        {
            Console.WriteLine($"使用TAP转换为APM  |{Thread.CurrentThread.ManagedThreadId}|");
            WebRequest webRq = WebRequest.Create("http://msdn.microsoft.com/zh-CN/");
            var tt1 = Task.Factory.FromAsync(webRq.BeginGetResponse, webRq.EndGetResponse, null, TaskCreationOptions.None)
            .ContinueWith(t =>
            {
                //异步获取响应的延续任务
                WebResponse webResp = null;
                try
                {
                    webResp = t.Result;
                    Console.WriteLine($"请求的内容大小为： {webResp.ContentLength} |{Thread.CurrentThread.ManagedThreadId}|");
                }
                catch (AggregateException ex)
                {
                    if (ex.GetBaseException() is WebException)
                    {
                        Console.WriteLine($"异常发生，异常信息为： {ex.GetBaseException().Message} |{Thread.CurrentThread.ManagedThreadId}|");
                    }
                    else
                    {
                        throw ex;
                    }
                }
                finally
                {
                    webResp?.Close();
                }
            });
            Console.WriteLine($"主线程显示的信息 |{Thread.CurrentThread.ManagedThreadId}|");
        }

        #endregion
    }
}
