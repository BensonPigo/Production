using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class B03 : Sci.Win.Tems.Input1
    {
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        //存檔前檢查
        protected override bool ClickSaveBefore()
        {
            DataTable whseReasonDt;
            Ict.DualResult cbResult;

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Desc > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["ActionCode"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Action Code > can not be empty!");
                this.textBox4.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["id"].ToString()))
            {
                CurrentMaintain["type"] = "RR";
                if (cbResult = DBProxy.Current.Select(null, "select max(id) max_id from whsereason where type='RR'", out whseReasonDt))
                {
                    string id = whseReasonDt.Rows[0]["max_id"].ToString();
                    if (string.IsNullOrWhiteSpace(id))
                    { CurrentMaintain["id"] = "00001"; }
                    else
                    {
                        int newID = int.Parse(id) + 1;
                        CurrentMaintain["id"] = Convert.ToString(newID).ToString().PadLeft(5, '0');
                    }
                }
                else { ShowErr(cbResult); }

            }
            return base.ClickSaveBefore();
        }

        //copy前清空id
        protected override void ClickCopyAfter()
        {
            CurrentMaintain["id"] = string.Empty;
        }

        private void textBox4_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select id,description from whseReason where type='RA' and junk = 0", "10,20", textBox2.Text,true,",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.CurrentMaintain["ActionCode"] = item.GetSelectedString();
        }
    }
}
