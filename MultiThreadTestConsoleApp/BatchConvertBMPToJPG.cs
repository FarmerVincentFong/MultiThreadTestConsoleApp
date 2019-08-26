//======================================================================
// Copyright (C) 2010-2019 广州市宏光软件科技有限公司 
// 版权所有 
// 
// 文件名: BatchConvertBMPToJPG
// 创建人: 方文谦
// 创建时间: 2019/8/15 17:49:27
//======================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Encoder = System.Drawing.Imaging.Encoder;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 多线程测试，将当前目录中的所有bmp图片转为jpg图片
    /// </summary>
    public class BatchConvertBMPToJPG
    {
        private static string currentDir = Environment.CurrentDirectory;
        private static int allFileCount;
        private static int completeFileCount;
        private static object lockobj = new object();
        private static Stopwatch sw = new Stopwatch();
        /// <summary>
        /// 开始转换
        /// </summary>
        public static void StartTransform()
        {
            Console.WriteLine("将当前文件夹的所有bmp图片转换为jpg图片！请输入开启的线程数：");
            int taskNum;
            if (!int.TryParse(Console.ReadLine(), out taskNum))
            {
                taskNum = 1;
            };
            sw.Start();
            //获取所有bmp图片文件
            string[] fileNames = Directory.GetFiles(currentDir, "*.bmp", SearchOption.AllDirectories);
            allFileCount = fileNames.Length;
            Console.WriteLine($"bmp图片总文件数：{allFileCount}");
            //划分文件到各个线程
            string[][] ss = new string[taskNum][];  //不规则二维数组
            for (var i = 0; i < taskNum; i++)
            {
                ss[i] = fileNames.Skip(i * (allFileCount + 1) / taskNum).Take((allFileCount + 1) / taskNum).ToArray();
            }
            //使用多线程来转换
            List<Task> tlist = new List<Task>();
            foreach (var tempFilesNames in ss)
            {
                var tt = Task.Factory.StartNew(() =>
                {
                    foreach (var bmpfilename in tempFilesNames)
                    {
                        string jpgfileName;
                        //执行出错后，将继续执行
                        while (string.IsNullOrEmpty(jpgfileName = SimpleTransformBMPToJPG(bmpfilename)))
                        {
                            Thread.Sleep(200);
                        }
                        lock (lockobj)
                        {
                            Console.WriteLine($"{++completeFileCount}/{allFileCount}：{jpgfileName}  |转换完成");
                        }
                    }
                });
                tlist.Add(tt);
            }
            //等待执行完成
            Task.WaitAll(tlist.ToArray());
            Console.WriteLine($"bmp转jpg格式转换完成！成功数：{completeFileCount} 总数：{allFileCount}");
            sw.Stop();
            Console.WriteLine($"总耗时：{sw.ElapsedMilliseconds } MS");
            Console.ReadKey();
        }
        //转换图片格式（高质量版）
        private static string TransformBMPToJPG(string bmpFilePath)
        {
            string jpgFilePath = null;
            try
            {
                using (Bitmap bm = new Bitmap(bmpFilePath))
                {
                    jpgFilePath = bmpFilePath.Replace(".bmp", ".jpg");
                    EncoderParameters eps = new EncoderParameters(1);
                    eps.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                    bm.Save(jpgFilePath, GetEncoder(ImageFormat.Jpeg), eps);
                }
            }
            catch (OutOfMemoryException ex)
            {
                jpgFilePath = null;
            }
            return jpgFilePath;
        }
        //转换图片格式（简单版）
        private static string SimpleTransformBMPToJPG(string bmpFilePath)
        {
            string jpgFilePath = null;
            try
            {
                using (Bitmap bm = new Bitmap(bmpFilePath))
                {
                    jpgFilePath = bmpFilePath.Replace(".bmp", ".jpg");
                    bm.Save(jpgFilePath, ImageFormat.Jpeg);
                }
            }
            catch (OutOfMemoryException ex)
            {
                jpgFilePath = null;
            }
            return jpgFilePath;
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }
    }
}
