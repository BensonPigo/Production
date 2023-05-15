using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public static class HTTPFileDownloadANDUpData
    {

        private static string _filePath;
        private static long _fileSize;

        /// <inheritdoc/>
        public static async void FileUPDate(string uploadUrl)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All Files (*.*)|*.*";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _filePath = openFileDialog.FileName;
                _fileSize = new FileInfo(_filePath).Length;

                if (_fileSize > 1024 * 1024 * 1024)
                {
                    MessageBox.Show("File size exceeds the limit (1G).");
                    return;
                }
            }

            var progress = new Progress<int>(value =>
            {
                var test = value; // 進度條
            });

            try
            {
                await UploadFileAsync(uploadUrl, _filePath, progress);
                MessageBox.Show("Upload complete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upload failed: " + ex.Message);
            }
        }

        /// <inheritdoc/>
        public static async void FileDownload(string downloadUrl)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var file = saveFileDialog.FileName;

                    Debug.WriteLine("Downloading...");

                    var progress = new Progress<int>(value =>
                    {
                        var s = value;
                    });

                    try
                    {
                        await DownloadFileAsync(downloadUrl, file, progress);

                        Debug.WriteLine("Download complete.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Download failed: {ex.Message}");
                    }
                }
            }
        }

        /// <inheritdoc/>
        private static async Task UploadFileAsync(string url, string filePath, IProgress<int> progress)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/octet-stream");

                var fileInfo = new FileInfo(filePath);

                if (fileInfo.Exists)
                {
                    client.Headers.Add("Content-Range", $"bytes {fileInfo.Length}-*/{fileInfo.Length}");
                }

                var fileStream = fileInfo.OpenRead();

                client.UploadProgressChanged += (sender, args) =>
                {
                    // var value = (int)(args.BytesSent * 100 / fileInfo.Length);
                    var value = 0;
                    progress?.Report(value);
                };

                await client.UploadDataTaskAsync(url, "PUT", ConvertFileStreamToByteArray(fileStream));

                fileStream.Close();
            }
        }

        /// <inheritdoc/>
        private static async Task DownloadFileAsync(string url, string filePath, IProgress<int> progress)
        {
            var fileInfo = new FileInfo(filePath);

            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            if (fileInfo.Exists)
            {
                request.AddRange(fileInfo.Length);
            }

            using (var response = await request.GetResponseAsync())
            {
                _fileSize = response.ContentLength;

                using (var stream = response.GetResponseStream())
                using (var fileStream = new FileStream(filePath, FileMode.Append))
                {
                    var buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);

                        var value = (int)((fileStream.Length * 100) / _fileSize);
                        progress?.Report(value);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public static byte[] ConvertFileStreamToByteArray(FileStream fileStream)
        {
            byte[] byteArray = null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                byteArray = memoryStream.ToArray();
            }

            return byteArray;
        }
    }
}
