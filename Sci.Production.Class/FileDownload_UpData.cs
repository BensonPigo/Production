using Ict;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Ict.Win.UI.FileView;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public static class FileDownload_UpData
    {
        /// <inheritdoc/>
        public static async Task DownloadFileAsync(string url, string filePath, string fileName, string saveFilePath)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Timeout = 60000; // 60秒
                httpWebRequest.Headers.Add("FilePath", filePath);
                httpWebRequest.Headers.Add("FileName", Convert.ToBase64String(Encoding.UTF8.GetBytes(fileName)));
                httpWebRequest.KeepAlive = false; // 關閉 Keep-Alive
                var httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync().ConfigureAwait(false);

                // 限制檔案大小
                // if (httpWebResponse.ContentLength > 15 * 1024 * 1024)
                // {
                //    throw new Exception("檔案大小超過 15MB 的上限！");
                // }
                using (var respStream = httpWebResponse.GetResponseStream())
                {
                    var savePath = Path.Combine(saveFilePath, fileName);
                    using (var fileStream = new FileStream(savePath, FileMode.Create))
                    {
                        await respStream.CopyToAsync(fileStream);
                    }
                }

                httpWebResponse.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public static async Task UploadFile(string url, string filePath, string fileName, string sourceFile)
        {
            try
            {
                // 限制檔案大小
                var fileInfo = new FileInfo(sourceFile);
                if (fileInfo.Length > 15 * 1024 * 1024)
                {
                    throw new Exception("檔案大小超過 15MB 的上限！");
                }

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Headers.Add("FilePath", filePath);
                request.Headers.Add("FileName", fileName);
                request.Timeout = 60000; // 60秒

                // 設定ContentType
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                // 寫入資料到RequestStream
                using (var fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                {
                    using (var br = new BinaryReader(fs))
                    {
                        using (var requestStream = request.GetRequestStream())
                        {
                            // 寫入Boundary
                            string boundaryString = "--" + boundary + "\r\n";
                            byte[] boundaryBytes = Encoding.ASCII.GetBytes(boundaryString);
                            await requestStream.WriteAsync(boundaryBytes, 0, boundaryBytes.Length);

                            // 寫入ContentDisposition
                            string contentDisposition = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n", "file", fileName);
                            byte[] contentDispositionBytes = Encoding.UTF8.GetBytes(contentDisposition);
                            await requestStream.WriteAsync(contentDispositionBytes, 0, contentDispositionBytes.Length);

                            // 寫入ContentType
                            byte[] contentTypeBytes = Encoding.ASCII.GetBytes("Content-Type: application/octet-stream\r\n\r\n");
                            await requestStream.WriteAsync(contentTypeBytes, 0, contentTypeBytes.Length);

                            // 寫入檔案內容
                            byte[] buffer = new byte[1024 * 1024];
                            int bytesRead = 0;
                            while ((bytesRead = br.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                await requestStream.WriteAsync(buffer, 0, bytesRead);
                            }

                            // 寫入Boundary
                            boundaryString = "\r\n--" + boundary + "--\r\n";
                            boundaryBytes = Encoding.ASCII.GetBytes(boundaryString);
                            await requestStream.WriteAsync(boundaryBytes, 0, boundaryBytes.Length);
                        }
                    }
                }

                // 發送Request
                using (var response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // 上傳成功，執行後續的程式邏輯
                    }
                    else
                    {
                        // 上傳失敗，顯示錯誤訊息
                        MessageBox.Show(string.Format("上傳失敗，HTTP狀態碼：{0}", response.StatusCode));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Download failed: {ex.Message}");
            }
        }
    }
}
