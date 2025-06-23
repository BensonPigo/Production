using System;
using System.IO;
using System.IO.Ports;
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
            this.TryReadWeight("COM3", this.numWeight);
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

                // 讀取一筆資料
                string raw = scalePort.ReadLine(); // 範例："  12.50kg\r\n"
                string cleaned = raw.Replace("kg", string.Empty).Trim();

                if (decimal.TryParse(cleaned, out decimal weight))
                {
                    targetNumericbox.Value = weight;
                }
                else
                {
                    // 若格式異常，視情況顯示錯誤或略過
                    MessageBox.Show($"讀到無效資料：{raw}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Port 被其他程式用掉，略過
                MessageBox.Show("Port 被其他程式用掉：");
            }
            catch (TimeoutException)
            {
                // 等太久沒回應，略過
            }
            catch (IOException)
            {
                // Port 根本不存在，略過
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
