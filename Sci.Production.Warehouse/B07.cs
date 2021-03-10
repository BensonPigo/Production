using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class B07 : Sci.Win.Tems.Input1
    {
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override DualResult ClickSavePre()
        {
            return base.ClickSavePre();
        }

        protected override DualResult ClickSave()
        {
            return base.ClickSave();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["StartDate"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["BeginTime"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["EndTime"]))
            {
                MyUtility.Msg.WarningBox("<Start Date, Begin Time, End Time> cannot be empty.");
                return false;
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickNewAfter()
        {
            this.CurrentMaintain["Mdivision"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["StartDate"] = MyUtility.Convert.GetDate(DateTime.Now.ToShortDateString());
            base.ClickNewAfter();
        }

        private void TxtBeginTime_Validating(object sender, CancelEventArgs e)
        {
            this.Time_Validating(sender, e);
        }

        private void TxtEndTime_Validating(object sender, CancelEventArgs e)
        {
            this.Time_Validating(sender, e);
        }

        private void Time_Validating(object sender, CancelEventArgs e)
        {
            Win.UI.TextBox prodTextValue = (Win.UI.TextBox)sender;
            string strBeginTime = this.txtBeginTime.Text;
            string strEndTime = this.txtEndTime.Text;
            if (this.EditMode && !MyUtility.Check.Empty(prodTextValue.Text) && prodTextValue.Text != prodTextValue.OldValue)
            {
                string textValue = prodTextValue.Text.ToString().PadRight(4);
                if ((!MyUtility.Check.Empty(textValue.Substring(0, 2)) && Convert.ToInt32(textValue.Substring(0, 2)) > 24) || ((!MyUtility.Check.Empty(textValue.Substring(0, 2)) && Convert.ToInt32(textValue.Substring(0, 2)) == 24) && (!MyUtility.Check.Empty(textValue.Substring(2, 2)) && Convert.ToInt32(textValue.Substring(2, 2)) != 0)) || (!MyUtility.Check.Empty(textValue.Substring(2, 2)) && Convert.ToInt32(textValue.Substring(2, 2)) >= 60))
                {
                    prodTextValue.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("The time format is wrong, can't exceed '24:00'!");
                    return;
                }
                else
                {
                    string newValue = (MyUtility.Check.Empty(textValue.Substring(0, 2)) ? "00" : Convert.ToInt32(textValue.Substring(0, 2)) < 10 ? "0" + Convert.ToInt32(textValue.Substring(0, 2)).ToString() : textValue.Substring(0, 2)) + (MyUtility.Check.Empty(textValue.Substring(2, 2)) ? "00" : Convert.ToInt32(textValue.Substring(2, 2)) < 10 ? "0" + Convert.ToInt32(textValue.Substring(2, 2)).ToString() : textValue.Substring(2, 2));
                    prodTextValue.Text = newValue;
                }

                if (!MyUtility.Check.Empty(strBeginTime) &&
                    !MyUtility.Check.Empty(strEndTime))
                {
                    int hhBegin = MyUtility.Convert.GetInt(strBeginTime.Substring(0, 2));
                    int hhEnd = MyUtility.Convert.GetInt(strEndTime.Substring(0, 2));
                    int mmBegin = MyUtility.Convert.GetInt(strBeginTime.Substring(2, 2));
                    int mmEnd = MyUtility.Convert.GetInt(strEndTime.Substring(2, 2));
                    if (hhBegin > hhEnd)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(@"The Begin and End Time is not correct," + Environment.NewLine + "Begin Time cannot later than End Time.");
                    }
                    else if (hhBegin == hhEnd && mmBegin > mmEnd)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(@"The Begin and End Time is not correct," + Environment.NewLine + "Begin Time cannot later than End Time.");
                    }
                }
            }

            // format Time Value
            if (!MyUtility.Check.Empty(strBeginTime))
            {
                this.CurrentMaintain["BeginTime"] = MyUtility.Convert.GetInt(strBeginTime.Substring(0, 2)) + ":" + MyUtility.Convert.GetInt(strBeginTime.Substring(2, 2));
            }

            if (!MyUtility.Check.Empty(strEndTime))
            {
                this.CurrentMaintain["EndTime"] = MyUtility.Convert.GetInt(strEndTime.Substring(0, 2)) + ":" + MyUtility.Convert.GetInt(strEndTime.Substring(2, 2));
            }
        }

        private void TxtEndTime_TextChanged(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtEndTime.Text) && this.txtEndTime.Text.Length == 4)
            {
                this.CurrentMaintain["EndTime"] = MyUtility.Convert.GetInt(this.txtEndTime.Text.Substring(0, 2)) + ":" + MyUtility.Convert.GetInt(this.txtEndTime.Text.Substring(2, 2));
            }
        }
    }
}