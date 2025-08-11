using System;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Management;
using System.Windows.Forms;
using System.Threading.Tasks;
using static sun.awt.SunHints;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P18_InputWeight
    /// [ISP20250717]-因為不是每個工廠都有電子秤，但可能有多台的COM(不是每一台都是電子秤)
    /// 1.所有的COM都連不到:畫面不顯示 COM的訊息
    /// 2.如果有多台COM連得到並且其中一台能回傳重量的訊息:畫面顯示 此COM的訊息
    /// 3.如果有多台COM連得到，但每一台的訊息都不對:畫面顯示 此第一個可連並且有訊息，但同時要彈出錯誤訊息
    /// </summary>
    public partial class P18_InputWeight : Win.Forms.Base
    {
        /// <summary>
        /// actWeight
        /// </summary>
        public decimal? ActWeight { get; set; }

        /// <summary>
        /// P18_InputWeight
        /// </summary>
        public P18_InputWeight()
        {
            this.InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            this.ActWeight = 0;

            this.labCom.Text = string.Empty;

            Task.Run(() =>
            {
                string firstErrMsg = string.Empty;
                string firstComName = string.Empty;
                foreach (string comName in SerialPort.GetPortNames())
                {
                    string errMsg = this.TryReadWeight(comName, comName);

                    if (!string.IsNullOrEmpty(errMsg) && string.IsNullOrEmpty(firstErrMsg))
                    {
                        firstComName = comName;
                        firstErrMsg = errMsg;
                    }
                }

                if (string.IsNullOrEmpty(this.labCom.Text) && !string.IsNullOrEmpty(firstErrMsg))
                {
                    MessageBox.Show(firstErrMsg);
                }
            });
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            this.ActWeight = this.numWeight.Value.HasValue ? this.numWeight.Value : 0;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 嘗試讀取機器上數值
        /// </summary>
        /// <param name="comPort">comPort</param>
        /// <param name="comName">comName</param>
        /// <returns>錯誤訊息</returns>
        public string TryReadWeight(string comPort, string comName)
        {
            SerialPort scalePort = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);
            scalePort.ReadTimeout = 500;
            try
            {
                scalePort.Open();
                System.Threading.Thread.Sleep(1000); // 等一下讓設備有機會回傳
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

            try
            {
                while (true)
                {
                    string raw = scalePort.ReadLine(); // 讀取一筆資料, 範例："ST. NET. 3.50 kg"
                    var match = Regex.Match(raw, @"(\d+(\.\d+)?)");
                    if (match.Success)
                    {
                        string value = match.Value;

                        // 切回 UI 執行緒設定數值
                        this.Invoke(new Action(() =>
                        {
                            this.numWeight.Value = decimal.Parse(value);
                            this.labCom.Text = comName;
                        }));
                    }
                    else
                    {
                        return $"<{comPort}> Invalid data received: {raw}";
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return $"<{comPort}> is being used by another program";
            }
            catch (TimeoutException)
            {
                // 非秤重機，還是有可能會連接成功，但沒有回應
                return string.Empty;
            }
            catch (IOException ioEx)
            {
                var ports = SerialPort.GetPortNames();
                if (Array.Exists(ports, p => p == comPort))
                {
                    return $"<{comPort}> exists but cannot be opened, possibly occupied or hardware malfunction.\nError message: {ioEx.Message}";
                }
                else
                {
                    return $"<{comPort}> No response";
                }
            }
            catch (Exception ex)
            {
                return $"<{comPort}> Scale reading failed: " + ex.Message;
            }
            finally
            {
                if (scalePort.IsOpen)
                {
                    scalePort.Close();
                }
            }
        }
    }
}
