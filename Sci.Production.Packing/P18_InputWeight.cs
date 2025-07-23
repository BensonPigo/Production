using System;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P18_InputWeight
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
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            this.ActWeight = this.numWeight.Value.HasValue ? this.numWeight.Value : 0;
            this.DialogResult = DialogResult.OK;
        }

        private void BtnReadWeight_Click(object sender, EventArgs e)
        {
            this.TryReadWeight("COM4", this.numWeight);
        }

        /// <summary>
        /// 嘗試讀取機器上數值
        /// </summary>
        /// <param name="comPort">port : COM3</param>
        /// <param name="targetNumericbox">數值</param>
        public void TryReadWeight(string comPort, Ict.Win.UI.NumericBox targetNumericbox)
        {
            SerialPort scalePort = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);
            scalePort.ReadTimeout = 1000;

            try
            {
                scalePort.Open();
                System.Threading.Thread.Sleep(1000); // 等一下讓設備有機會回傳

                // 讀取一筆資料
                string raw = scalePort.ReadLine(); // 範例："ST. NET. 3.50 kg"
                var match = Regex.Match(raw, @"(\d+(\.\d+)?)");
                if (match.Success)
                {
                    string value = match.Value;
                    targetNumericbox.Value = decimal.Parse(value);
                }
                else
                {
                    // 若格式異常，視情況顯示錯誤或略過
                    MessageBox.Show($"讀到無效資料：{raw}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Port 被其他程式用掉");
            }
            catch (TimeoutException)
            {
                MessageBox.Show("沒回應");
            }
            catch (IOException ioEx)
            {
                var ports = SerialPort.GetPortNames();
                if (Array.Exists(ports, p => p == comPort))
                {
                    MessageBox.Show($"Port {comPort} 存在，但無法開啟，可能被佔用或硬體異常。\n錯誤訊息：{ioEx.Message}");
                }
                else
                {
                    MessageBox.Show($"Port {comPort} 根本不存在");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("秤重失敗：" + ex.Message);
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
