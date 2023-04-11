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
        public static async Task DownloadFileAsync(string url, string filePath, string fileName,string saveFilePath)
        {
            try
            {
                var fileInfo = new FileInfo(Path.Combine(filePath, fileName));
                if (fileInfo.Length > 15 * 1024 * 1024)
                {
                    throw new Exception("檔案大小超過 15MB 的上限！");
                }

                var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.ContinueTimeout = 10000; // 10秒
                httpWebRequest.Headers.Add("FilePath", filePath);
                httpWebRequest.Headers.Add("FileName", Convert.ToBase64String(Encoding.UTF8.GetBytes(fileName)));
                httpWebRequest.KeepAlive = false; // 關閉 Keep-Alive
                var httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync().ConfigureAwait(false);
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
    }
}
