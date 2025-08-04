using System;
using System.Drawing;
using System.Data;
using System.Linq;
using Sci.Production.Class;
using Sci.Production.Class.Commons;
using System.Configuration;

namespace Sci.Production.Win
{
    /// <inheritdoc />
    public partial class MainNotification : Sci.Win.UI._UserControl
    {
        private int panel2Width = 212;
        private int refreshSecond = MyUtility.Convert.GetInt(ConfigurationManager.AppSettings["RefreshSeconds"]);
        private int AutorefreshSecond = MyUtility.Convert.GetInt(ConfigurationManager.AppSettings["AutoRefreshSeconds"]);

        /// <inheritdoc />
        public enum RefreshEnum
        {
            /// <summary>
            /// 手動刷新
            /// </summary>
            ByClick,

            /// <summary>
            /// 自動刷新
            /// </summary>
            ByAuto
        }

        /// <inheritdoc />
        public MainNotification()
        {
            this.InitializeComponent();
            this.panel_Main.Visible = false;
            this.panel_Main.Width = 0;
            this.Width = this.panel_Side.Width;
            this.numAuto.Value = 600;
            this.numClick.Value = 300;
            this.pictureBox1.Image = Properties.Resources.open;
            this.timeLeft = this.refreshSecond;
            this.timerAuto.Interval = this.AutorefreshSecond * 1000;

            #if DEBUG
                this.panel_Set.Visible = true;
            #endif
        }

        private int timeLeft;

        /// <inheritdoc />
        public void Panel_Side_Click(object sender, EventArgs e)
        {
            if (this.panel_Main.Visible)
            {
                this.MainPanelClose();
            }
            else
            {
                this.MainPanelOpen();
            }

            // this.lblsumCount.Visible = false;  // 縮放側邊欄時將提醒筆數的紅字隱藏
        }

        /// <inheritdoc />
        /// 手動刷新,隔5分鐘才可使用
        public void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.timerClick.Start();
            this.timerClick.Interval = 1000;
            this.btnRefresh.Enabled = false;
            this.LoadNotify(RefreshEnum.ByClick);
            this.TimerReset();
        }

        /// <inheritdoc />
        public void TimerClick_Tick(object sender, EventArgs e)
        {
            if (this.timeLeft > 0)
            {
                this.timeLeft = this.timeLeft - 1;
                this.btnRefresh.Text = (this.timeLeft / 60).ToString() + "'" + (this.timeLeft % 60); // Btn秒數倒數
            }
            else
            {
                /* 倒數時間到執行 */
                this.btnRefresh.Enabled = true;
                this.btnRefresh.Text = "Refresh";
                this.timerClick.Stop();
            }
        }

        /// <inheritdoc />
        public void LoadNotify(RefreshEnum refreshType)
        {
            // 自動刷新時不可按Refresh按鈕
            if (refreshType == RefreshEnum.ByAuto)
            {
                this.btnRefresh.Enabled = false;
            }

            this.panel3.Controls.Clear();
            int controlCnt = 0;
            int sumCount = 0;

            // 抓取設定檔Table
            DataTable dt = NotificationPrg.dtNotifySetting;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.AsEnumerable())
                {
                    DataTable notifyData;

                    int count = NotificationPrg.GetNotifyCount(dr["Module"].ToString(), dr["ID"].ToString(), out notifyData);
                    if (count == 0)
                    {
                        continue;
                    }

                    SubNotification uc = new SubNotification();
                    uc.dt = notifyData;
                    this.panel3.Controls.Add(uc);
                    uc.SetValue(dr["Module"].ToString(), dr["Name"].ToString(), dr["Classname"].ToString(), dr["Parameter"].ToString(), count.ToString());

                    // 設定每個提醒Panel的間距與高度
                    if (controlCnt == 0)
                    {
                        uc.Top = 0;
                    }
                    else
                    {
                        uc.Top = (controlCnt * 50) + 2;
                    }

                    uc.Left = 11;
                    controlCnt = controlCnt + 1;
                    sumCount += count;
                }

                this.lblsumCount.Text = sumCount > 99 ? "99+" : sumCount.ToString();
                this.lblsumCount.Visible = sumCount > 0 ? true : false;

                // 如果沒有任何提醒,先縮小側邊欄再隱藏
                if (sumCount == 0 && this.Visible)
                {
                    if (this.panel_Main.Visible)
                    {
                        this.MainPanelClose();
                    }

                    this.Visible = false;
                }
                else if (sumCount > 0)
                {
                    this.Visible = true;
                }
            }
            else
            {
                // 沒有設定任何系統側邊欄提醒,不計時不刷新,且隱藏側邊欄
                this.timerAuto.Stop();
                this.Visible = false;
            }

            this.lblRefreshTime.Text = DateTime.Now.ToString("HH:mm:ss");
            if (refreshType == RefreshEnum.ByAuto)
            {
                this.btnRefresh.Enabled = true;
            }
        }

        // 10分鐘自動刷新
        private void TimerAuto_Tick(object sender, EventArgs e)
        {
            this.LoadNotify(RefreshEnum.ByAuto);
        }

        /// <summary>
        /// 自動刷新側邊欄重新計時
        /// </summary>
        private void TimerReset()
        {
            this.timerAuto.Stop();
            this.timerAuto.Start();
        }

        /// <inheritdoc />
        /// Debug用,可直接調整Refresh/Auto刷新秒數
        private void BtnSet_Click(object sender, EventArgs e)
        {
            this.TimerReset();
            this.timeLeft = (int)this.numClick.Value;
            this.timerAuto.Interval = (int)this.numAuto.Value * 1000;
            this.btnRefresh.Enabled = true;
            this.btnRefresh.Text = "Refresh";
            this.timerClick.Stop();
        }

        /// <inheritdoc />
        /// 關閉側邊欄
        private void MainPanelClose()
        {
            UIClassPrg.Animate(this.panel_Main, UIClassPrg.Effect.Slide, 150, 180);
            this.panel_Main.Width = 0;

            this.Width = this.Width - this.panel2Width;
            this.pictureBox1.Image = Sci.Production.Properties.Resources.open;
            this.panel_Side.Location = new Point(this.Width - this.panel_Side.Width, this.panel_Side.Location.Y);
        }

        /// <inheritdoc />
        /// 打開側邊欄
        private void MainPanelOpen()
        {
            this.panel_Main.Width = this.panel2Width;
            UIClassPrg.Animate(this.panel_Main, UIClassPrg.Effect.Slide, 150, 180);

            this.Width = this.Width + this.panel_Main.Width;
            this.pictureBox1.Image = Sci.Production.Properties.Resources.close;
            this.panel_Side.Location = new Point(this.Width - this.panel_Main.Width - this.panel_Side.Width, this.panel_Side.Location.Y);
        }
    }
}
