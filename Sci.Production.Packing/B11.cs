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

namespace Sci.Production.Packing
{
    public partial class B11 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.comboFrequency, 2, 1, @"1 Day,1 Day");
            this.comboFrequency.SelectedIndex = 0;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.dateBuyerDlv.Value = DateTime.Now.AddDays(-3);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["StartTime"] = "08:00";
            this.CurrentMaintain["EndTime"] = "09:00";
            this.comboFrequency.SelectedIndex = 0;
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

                this.CurrentMaintain["Frequency"] = this.comboFrequency.SelectedValue.ToString();
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
                string chkpk = $@"select FactoryID from Lost1stMDNotification where FactoryID = '{this.CurrentMaintain["FactoryID"]}'";
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
