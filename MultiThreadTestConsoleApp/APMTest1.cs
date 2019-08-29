using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreadTestConsoleApp
{
    /// <summary>
    /// 测试异步编程模型 APM
    /// 异步发送请求，并将结果写入文件
    /// </summary>
    public class APMTest1
    {
        public static void TestRun()
        {

        }
        #region use APM to download file asynchronously
        //异步下载文件
        private static void DownloadFileAsync(string url)
        {
            try
            {
                // Initialize an HttpWebRequest object
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                // Create an instance of the RequestState and assign HttpWebRequest instance to its request field.
                RequestState requestState = new RequestState();
                requestState.request = myHttpWebRequest;
                myHttpWebRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message is:{0}", e.Message);
            }
        }

        // The following method is called when each asynchronous operation completes. 
        private static void ResponseCallback(IAsyncResult callbackresult)
        {
            // Get RequestState object
            RequestState myRequestState = (RequestState)callbackresult.AsyncState;

            HttpWebRequest myHttpRequest = myRequestState.request;

            // End an Asynchronous request to the Internet resource
            myRequestState.response = (HttpWebResponse)myHttpRequest.EndGetResponse(callbackresult);

            // Get Response Stream from Server
            Stream responseStream = myRequestState.response.GetResponseStream();
            myRequestState.streamResponse = responseStream;

            IAsyncResult asynchronousRead = responseStream.BeginRead(myRequestState.BufferRead, 0, myRequestState.BufferRead.Length, ReadCallBack, myRequestState);
        }

        // Write bytes to FileStream
        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {
                // Get RequestState object
                RequestState myRequestState = (RequestState)asyncResult.AsyncState;

                // Get Response Stream from Server
                Stream responserStream = myRequestState.streamResponse;

                // 
                int readSize = responserStream.EndRead(asyncResult);
                if (readSize > 0)
                {
                    myRequestState.filestream.Write(myRequestState.BufferRead, 0, readSize);
                    responserStream.BeginRead(myRequestState.BufferRead, 0, myRequestState.BufferRead.Length, ReadCallBack, myRequestState);
                }
                else
                {
                    Console.WriteLine("\nThe Length of the File is: {0}", myRequestState.filestream.Length);
                    Console.WriteLine("DownLoad Completely, Download path is: {0}", myRequestState.savepath);
                    myRequestState.response.Close();
                    myRequestState.filestream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message is:{0}", e.Message);
            }
        }
        #endregion
        #region Download File Synchrously
        private static void DownLoadFile(string url)
        {
            // Create an instance of the RequestState 
            RequestState requestState = new RequestState();
            try
            {
                // Initialize an HttpWebRequest object
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                // assign HttpWebRequest instance to its request field.
                requestState.request = myHttpWebRequest;
                requestState.response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                requestState.streamResponse = requestState.response.GetResponseStream();
                int readSize = requestState.streamResponse.Read(requestState.BufferRead, 0, requestState.BufferRead.Length);
                while (readSize > 0)
                {
                    requestState.filestream.Write(requestState.BufferRead, 0, readSize);
                    readSize = requestState.streamResponse.Read(requestState.BufferRead, 0, requestState.BufferRead.Length);
                }

                Console.WriteLine("\nThe Length of the File is: {0}", requestState.filestream.Length);
                Console.WriteLine("DownLoad Completely, Download path is: {0}", requestState.savepath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message is:{0}", e.Message);
            }
            finally
            {
                requestState.response.Close();
                requestState.filestream.Close();
            }
        }
        #endregion 
        public class RequestState
        {
            public HttpWebRequest request;
            public HttpWebResponse response;
            public Stream streamResponse;
            public FileStream filestream;
            public byte[] BufferRead = new byte[10240];
            public string savepath = "fwq123.txt";
        }
    }
}
