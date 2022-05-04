using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;

namespace Sci.Production.Quality
{
    public partial class B26 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.comboFrequency, 2, 1, @"1 Hour,1 Hour,6 Hour,6 Hour,Half Day,Half Day,1 Day,1 Day");
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtFty.ReadOnly = true;
        }

        private void ComboFrequency_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboFrequency.SelectedItem != null && this.EditMode == true)
            {
                if (this.comboFrequency.SelectedValue.ToString().Equals("1 Day"))
                {
                    this.txtEndTime.Text = string.Empty;
                    this.txtEndTime.Enabled = false;
                    this.txtStartTime.Text = string.Empty;
                    this.txtStartTime.Enabled = false;
                }
                else
                {
                    this.txtStartTime.Enabled = true;
                    this.txtEndTime.Enabled = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            string startTimeRight2;
            string endTimeRight2;
            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("<Factory> cannot empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["RFTStandard"]))
            {
                MyUtility.Msg.WarningBox("<RFT Standard> cannot empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ToAddress"]))
            {
                MyUtility.Msg.WarningBox("<To Address> cannot empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["StartTime"]))
            {
                MyUtility.Msg.WarningBox("<Start Time> cannot empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Frequency"]))
            {
                MyUtility.Msg.WarningBox("<Frequency> can not empty!");
                return false;
            }
            else
            {
                #region 檢查日期區間
                string startTimeHour = this.CurrentMaintain["StartTime"].ToString().Left(2);
                string endTimeHour = this.CurrentMaintain["EndTime"].ToString().Left(2);
                string startTimeMin = this.CurrentMaintain["StartTime"].ToString().Right(5).Left(2);
                string endTimeMin = this.CurrentMaintain["EndTime"].ToString().Right(5).Left(2);

                switch (this.CurrentMaintain["Frequency"].ToString())
                {
                    case "1 Hour":
                        if (MyUtility.Convert.GetInt(endTimeHour) - MyUtility.Convert.GetInt(startTimeHour) <= 0)
                        {
                            MyUtility.Msg.WarningBox("Time range cannot lower than 1 hour");
                            return false;
                        }

                        break;
                    case "6 Hour":
                        if (MyUtility.Convert.GetInt(endTimeHour) - MyUtility.Convert.GetInt(startTimeHour) < 6 ||
                            (MyUtility.Convert.GetInt(endTimeHour) - MyUtility.Convert.GetInt(startTimeHour) == 6 &&
                            MyUtility.Convert.GetInt(endTimeMin) - MyUtility.Convert.GetInt(startTimeMin) < 0))
                        {
                            MyUtility.Msg.WarningBox("Time range cannot lower than 6 hour");
                            return false;
                        }

                        break;
                    case "Half Day":
                        if (MyUtility.Convert.GetInt(endTimeHour) - MyUtility.Convert.GetInt(startTimeHour) < 12 ||
                            (MyUtility.Convert.GetInt(endTimeHour) - MyUtility.Convert.GetInt(startTimeHour) == 12 &&
                            MyUtility.Convert.GetInt(endTimeMin) - MyUtility.Convert.GetInt(startTimeMin) < 0))
                        {
                            MyUtility.Msg.WarningBox("Time range cannot lower than 12 hour");
                            return false;
                        }

                        break;

                    // 1 Day 回填時間
                    case "1 Day":
                        this.CurrentMaintain["StartTime"] = DBNull.Value;
                        this.CurrentMaintain["EndTime"] = DBNull.Value;
                        break;
                    default:
                        break;
                }

                #endregion
            }

            string[] toAddressarr = this.CurrentMaintain["ToAddress"].ToString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] ccAddressarr = this.CurrentMaintain["CcAddress"].ToString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string errmsg = string.Empty;
            foreach (string address in toAddressarr)
            {
                if (!address.IsEmail())
                {
                    errmsg += address + ";";
                }
            }

            if (!errmsg.Empty())
            {
                MyUtility.Msg.WarningBox("ToAddress Email :" + errmsg + " Incorrect");
                return false;
            }

            foreach (string address in ccAddressarr)
            {
                if (!address.IsEmail())
                {
                    errmsg += address + ";";
                }
            }

            if (!errmsg.Empty())
            {
                MyUtility.Msg.WarningBox("CcAddressarr Email :" + errmsg + " Incorrect");
                return false;
            }

            if (this.IsDetailInserting)
            {
                string chkpk = $@"select FactoryID from SampleRFTNotification where FactoryID = '{this.CurrentMaintain["FactoryID"]}'";
                if (MyUtility.Check.Seek(chkpk))
                {
                    MyUtility.Msg.WarningBox("FactoryID already exists!");
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }
    }
}
