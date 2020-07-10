using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P01
    /// </summary>
    public partial class P01 : Win.Tems.Input1
    {
        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        /// <param name="type">Type</param>
        public P01(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Text = type == "1" ? "P01. Packing Master List" : "P011. Packing Master List (History)";
            this.DefaultFilter = type == "1" ? string.Format("MDivisionID = '{0}' AND IsForecast = 0 AND GMTClose is null", Env.User.Keyword) : string.Format("MDivisionID = '{0}' AND IsForecast = 0 AND GMTClose is not null", Env.User.Keyword);
            this.txtcountryDestination.TextBox1.ReadOnly = true;
            this.txtcountryDestination.TextBox1.IsSupportEditMode = false;
            this.txtuserLocalMR.TextBox1.ReadOnly = true;
            this.txtuserLocalMR.TextBox1.IsSupportEditMode = false;
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displayDescription.Value = MyUtility.GetValue.Lookup(string.Format("select Description from style WITH (NOLOCK) where Ukey = {0}", this.CurrentMaintain["StyleUkey"].ToString()));
            this.displayPOcombo.Value = MyUtility.GetValue.Lookup(string.Format("select [dbo].getPOComboList('{0}','{1}') as PoList from Orders WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["POID"].ToString()));
            this.btnbdown.Enabled = this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["CtnType"]) == "2" && !this.EditMode;

            // 按鈕變色
            if (MyUtility.Convert.GetString(this.CurrentMaintain["CtnType"]) == "2")
            {
                this.btnbdown.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyCTN WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            }

            this.btnQuantityBreakdown.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            this.btnPackingMethod.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["Packing"]) ? Color.Blue : Color.Black;
            this.btnCartonStatus.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' and ReceiveDate is not null", this.CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            this.btnMaterialImport.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Export_Detail WITH (NOLOCK) where PoID = '{0}'", this.CurrentMaintain["POID"].ToString())) ? Color.Blue : Color.Black;
            this.btnCartonBooking.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_CTNData WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            bool gmtClose = !MyUtility.Check.Empty(this.CurrentMaintain["GMTClose"]);
            this.btnOverrunGarmentRecord.Visible = gmtClose;
            if (gmtClose)
            {
                this.btnOverrunGarmentRecord.ForeColor = MyUtility.Check.Seek(string.Format("select ID from OverrunGMT WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            }
        }

        // b'down
        private void Btnbdown_Click(object sender, EventArgs e)
        {
            PPIC.P01_QtyCTN callNextForm = new PPIC.P01_QtyCTN(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Quantity breakdown
        private void BtnQuantityBreakdown_Click(object sender, EventArgs e)
        {
            PPIC.P01_Qty callNextForm = new PPIC.P01_Qty(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["POID"]), MyUtility.Convert.GetString(this.displayPOcombo.Value));
            callNextForm.ShowDialog(this);
        }

        // Packing method
        private void BtnPackingMethod_Click(object sender, EventArgs e)
        {
            Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(this.CurrentMaintain["Packing"].ToString(), "Packing method", false, null);
            callNextForm.ShowDialog(this);
        }

        // Carton Status
        private void BtnCartonStatus_Click(object sender, EventArgs e)
        {
            PPIC.P01_CTNStatus callNextForm = new PPIC.P01_CTNStatus(this.CurrentMaintain["ID"].ToString(), false);
            callNextForm.ShowDialog(this);
        }

        // Material import
        private void BtnMaterialImport_Click(object sender, EventArgs e)
        {
            PPIC.P01_MTLImport callNextForm = new PPIC.P01_MTLImport(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Carton Booking
        private void BtnCartonBooking_Click(object sender, EventArgs e)
        {
            P01_CTNData callNextForm = new P01_CTNData(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Overrun garment record
        private void BtnOverrunGarmentRecord_Click(object sender, EventArgs e)
        {
            P01_OverrunGMTRecord callNextForm = new P01_OverrunGMTRecord(this.CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }
    }
}
