using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// Shipping_B10
    /// </summary>
    public partial class Shipping_B10 : Win.Tems.Input1
    {
        /// <summary>
        /// Shipping_B10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public Shipping_B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.editDesc.Text))
            {
                this.editDesc.Focus();
                MyUtility.Msg.WarningBox("Please insert <Reason>!!");
                return false;
            }

            DataTable whseReasonDt;
            Ict.DualResult cbResult;

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                this.CurrentMaintain["type"] = "OS";
                if (cbResult = DBProxy.Current.Select(string.Empty, "select max(id) max_id from ShippingReason WITH (NOLOCK) where type='OS'", out whseReasonDt))
                {
                    string id = whseReasonDt.Rows[0]["max_id"].ToString();
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        this.CurrentMaintain["id"] = "OS001";
                    }
                    else
                    {
                        int newID = int.Parse(id.Substring(2, id.Length - 2)) + 1;
                        this.CurrentMaintain["id"] = "OS" + Convert.ToString(newID).ToString().PadLeft(3, '0');
                    }
                }
                else
                {
                    this.ShowErr(cbResult);
                }
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Type"] = "OS";
        }
    }
}
