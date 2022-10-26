//======================================================================
// Copyright (C) 2010-2019 广州市宏光软件科技有限公司 
// 版权所有 
// 
// 文件名: Class1
// 创建人: 方文谦
// 创建时间: 2019/8/17 9:18:07
//======================================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreadTestConsoleApp.Extensions
{
    public static class BitmapExtensions
    {
        //bmp转高质量jpg
        public static void SaveJPG100(this Bitmap bmp, string filename)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            bmp.Save(filename, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }
        //bmp转高质量jpg
        public static void SaveJPG100(this Bitmap bmp, Stream stream)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            bmp.Save(stream, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
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
