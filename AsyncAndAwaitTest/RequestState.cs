//======================================================================
// Copyright (C) 2010-2019 广州市宏光软件科技有限公司 
// 版权所有 
// 
// 文件名: RequestState
// 创建人: 方文谦
// 创建时间: 2019/8/19 11:44:59
//======================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAndAwaitTest
{
    /// <summary>
    /// 封装过的状态信息，请求响应等使用
    /// </summary>
    public class RequestState
    {
        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream streamResponse;
        public FileStream filestream;
        //缓存
        public byte[] BufferRead = new byte[1024];

        public string savepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+ @"\fwqtest.exe";
    }
}
