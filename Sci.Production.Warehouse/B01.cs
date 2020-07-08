using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        // 存檔前檢查
        protected override bool ClickSaveBefore()
        {
            DataTable whseReasonDt;
            Ict.DualResult cbResult;

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Desc > can not be empty!");
                this.txtDesc.Focus();
                return false;
            }

            // if (String.IsNullOrWhiteSpace(CurrentMaintain["No"].ToString()))
            // {
            //    MyUtility.Msg.WarningBox("< No > can not be empty!");
            //    this.textBox4.Focus();
            //    return false;
            // }
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                this.CurrentMaintain["type"] = "IR";
                if (cbResult = DBProxy.Current.Select(null, "select max(id) max_id from whsereason WITH (NOLOCK) where type='IR'", out whseReasonDt))
                {
                    string id = whseReasonDt.Rows[0]["max_id"].ToString();
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        this.CurrentMaintain["id"] = "00001";
                    }
                    else
                    {
                        int newID = int.Parse(id) + 1;
                        this.CurrentMaintain["id"] = Convert.ToString(newID).ToString().PadLeft(5, '0');
                    }
                }
                else
                {
                    this.ShowErr(cbResult);
                }
            }

            return base.ClickSaveBefore();
        }

        // copy前清空id
        protected override void ClickCopyAfter()
        {
             this.CurrentMaintain["id"] = string.Empty;
        }
    }
}
