using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P01
    /// </summary>
    public partial class P01 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        /// <param name="type">type</param>
        public P01(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Text = type == "1" ? "P01. Clog Master List" : "P011. Clog Master List (History)";
            this.DefaultFilter = type == "1" ? string.Format("MDivisionID = '{0}' AND IsForecast = 0 AND Finished = 0", Sci.Env.User.Keyword) : string.Format("MDivisionID = '{0}' AND IsForecast = 0 AND Finished = 1", Sci.Env.User.Keyword);
        }

        /// <summary>
        /// OnDetailEntered()
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataRow brandData;
            this.displayDescription.Value = MyUtility.GetValue.Lookup(string.Format("select Description from style WITH (NOLOCK) where Ukey = {0}", this.CurrentMaintain["StyleUkey"].ToString()));
            this.displayPOCombo.Value = MyUtility.GetValue.Lookup(string.Format("select [dbo].getPOComboList('{0}','{1}') as PoList from Orders WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["POID"].ToString()));
            if (MyUtility.Check.Seek(string.Format("select ID,Customize1,Customize2,Customize3 from Brand WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["BrandID"].ToString()), out brandData))
            {
                this.labelSpecialId1.Text = brandData["Customize1"].ToString();
                this.labelSpecialId2.Text = brandData["Customize2"].ToString();
                this.labelSpecialId3.Text = brandData["Customize3"].ToString();
            }
            else
            {
                this.labelSpecialId1.Text = string.Empty;
                this.labelSpecialId2.Text = string.Empty;
                this.labelSpecialId3.Text = string.Empty;
            }

            this.displayActPullout.Value = this.CurrentMaintain["PulloutComplete"].ToString().ToUpper() == "TRUE" ? "OK" : MyUtility.GetValue.Lookup(string.Format("select COUNT(distinct ID) as CntID from Pullout_Detail where OrderID = '{0}' and ShipQty > 0", this.CurrentMaintain["ID"].ToString()));
            this.displayCFAFinalInspDate.Value = this.CurrentMaintain["InspResult"].ToString() == "P" ? "Pass" : this.CurrentMaintain["InspResult"].ToString() == "F" ? "Fail" : string.Empty;
            this.numCtnQtyOnTransit.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(
                $@"
SELECT isnull(sum(b.CTNQty),0)
   FROM PackingList a,
        PackingList_Detail b
   WHERE a.ID = b.ID
     AND (a.Type = 'B'
          OR a.Type = 'L')
     AND b.OrderID = '{this.CurrentMaintain["ID"].ToString()}'
     AND b.TransferDate is not null and b.ReceiveDate is null
", null));
            this.numCtnTransit.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(
                $@"
SELECT isnull(sum(b.CTNQty),0)
   FROM PackingList a,
        PackingList_Detail b
   WHERE a.ID = b.ID
     AND (a.Type = 'B'
          OR a.Type = 'L')
     AND b.OrderID = '{this.CurrentMaintain["ID"].ToString()}'
     AND (b.TransferCFADate is not null and b.CFAReceiveDate is null or b.CFAReturnClogDate is not null and b.ClogReceiveCFADate is null)
", null));
            this.numCtnQtyInFactory.Value = MyUtility.Convert.GetInt(this.CurrentMaintain["TotalCTN"]) - MyUtility.Convert.GetInt(this.CurrentMaintain["FtyCTN"]);
            this.numttlCtnTransferred.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCTN"]) == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["ClogCTN"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCTN"]) * 100, 2);

            this.numCtnCFA.Value = MyUtility.Check.Empty(this.CurrentMaintain["CfaCTN"]) ? 0 : MyUtility.Convert.GetInt(this.CurrentMaintain["CfaCTN"]);
            this.numCtnQtyInClog.Value = MyUtility.Check.Empty(this.CurrentMaintain["ClogCTN"]) ? 0 : MyUtility.Convert.GetInt(this.CurrentMaintain["ClogCTN"]);
            this.numTtlCtnQty.Value = MyUtility.Check.Empty(this.CurrentMaintain["TotalCTN"]) ? 0 : MyUtility.Convert.GetInt(this.CurrentMaintain["TotalCTN"]);

            // 按鈕變色
            bool haveOrder_Qty = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString()));
            this.btnQuantityBreakdown.ForeColor = haveOrder_Qty ? Color.Blue : Color.Black;
            this.btnQtyBDownByShipmode.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            this.btnProductionOutput.ForeColor = haveOrder_Qty ? Color.Blue : Color.Black;
            this.btnGarmentExport.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}'", this.CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            this.btnCartonSize.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_CTNData WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            this.btnCartonStatus.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' and ReceiveDate is not null", this.CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            this.btnOrderRemark.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;
            this.numCtnQtyInClog.BackColor = Color.FromArgb(181, 230, 29);
            this.numCtnQtyOnTransit.BackColor = Color.FromArgb(181, 230, 29);
            this.numCtnCFA.BackColor = Color.FromArgb(255, 174, 201);
            this.numCtnTransit.BackColor = Color.FromArgb(255, 174, 201);

            this.btnFabricInspectionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from FIR WITH (NOLOCK) where POID = '{0}'", this.CurrentMaintain["POID"].ToString())) ? Color.Blue : Color.Black;
            this.btnCFARFTList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Cfa WITH (NOLOCK) where OrderID = '{0}'", this.CurrentMaintain["ID"].ToString())) || MyUtility.Check.Seek(string.Format("select OrderID from Rft where OrderID = '{0}'", this.CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            this.btnAccessoryInspectionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from AIR WITH (NOLOCK) where POID = '{0}'", this.CurrentMaintain["POID"].ToString())) ? Color.Blue : Color.Black;
        }

        // Quantity breakdown
        private void BtnQuantityBreakdown_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["POID"]), MyUtility.Convert.GetString(this.displayPOCombo.Value));
            callNextForm.ShowDialog(this);
        }

        // Q'ty b'down by shipmode
        private void BtnQtyBDownByShipmode_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyShip callNextForm = new Sci.Production.PPIC.P01_QtyShip(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        // Production output
        private void BtnProductionOutput_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput callNextForm = new Sci.Production.PPIC.P01_ProductionOutput(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Garment Export
        private void BtnGarmentExport_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_GMTExport callNextForm = new Sci.Production.PPIC.P01_GMTExport(this.CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        // Carton Size
        private void BtnCartonSize_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P01_CTNData callNextForm = new Sci.Production.Packing.P01_CTNData(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Carton Status
        private void BtnCartonStatus_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CTNStatus callNextForm = new Sci.Production.PPIC.P01_CTNStatus(this.CurrentMaintain["ID"].ToString(), true);
            callNextForm.ShowDialog(this);
            this.RenewData();
            this.numCtnQtyOnTransit.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(
               $@"
SELECT isnull(sum(b.CTNQty),0)
   FROM PackingList a,
        PackingList_Detail b
   WHERE a.ID = b.ID
     AND (a.Type = 'B'
          OR a.Type = 'L')
     AND b.OrderID = '{this.CurrentMaintain["ID"].ToString()}'
     AND b.TransferDate is not null and b.ReceiveDate is null
", null));
            this.numCtnTransit.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(
               $@"
SELECT isnull(sum(b.CTNQty),0)
   FROM PackingList a,
        PackingList_Detail b
   WHERE a.ID = b.ID
     AND (a.Type = 'B'
          OR a.Type = 'L')
     AND b.OrderID = '{this.CurrentMaintain["ID"].ToString()}'
     AND (b.TransferCFADate is not null and b.CFAReceiveDate is null or b.CFAReturnClogDate is not null and b.ClogReceiveCFADate is null)
", null));
            this.numCtnQtyInFactory.Value = MyUtility.Convert.GetInt(this.CurrentMaintain["TotalCTN"]) - MyUtility.Convert.GetInt(this.CurrentMaintain["FtyCTN"]);
            this.numttlCtnTransferred.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCTN"]) == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["ClogCTN"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCTN"]) * 100, 2);
            this.numCtnCFA.Value = MyUtility.Check.Empty(this.CurrentMaintain["CfaCTN"]) ? 0 : MyUtility.Convert.GetInt(this.CurrentMaintain["CfaCTN"]);
            this.numCtnQtyInClog.Value = MyUtility.Check.Empty(this.CurrentMaintain["ClogCTN"]) ? 0 : MyUtility.Convert.GetInt(this.CurrentMaintain["ClogCTN"]);
            this.numTtlCtnQty.Value = MyUtility.Check.Empty(this.CurrentMaintain["TotalCTN"]) ? 0 : MyUtility.Convert.GetInt(this.CurrentMaintain["TotalCTN"]);
        }

        // Order remark
        private void BtnOrderRemark_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(this.CurrentMaintain["OrderRemark"].ToString(), "Order Remark", false, null);
            callNextForm.ShowDialog(this);
        }

        // Fabric inspection list
        private void BtnFabricInspectionList_Click(object sender, EventArgs e)
        {
            // 等QA -> P01開發完成後呼叫
            // Sci.Production.QA.P01 callNextForm = new Sci.Production.QA.P01(this.CurrentMaintain["POID"].ToString());
            // callNextForm.ShowDialog(this);
        }

        // CFA && RFT list
        private void BtnCFARFTList_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P01_CFAAndRFTList callNextForm = new Sci.Production.Logistic.P01_CFAAndRFTList(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        // Accessory inspection list
        private void BtnAccessoryInspectionList_Click(object sender, EventArgs e)
        {
            // 等QA -> P02開發完成後呼叫
            // Sci.Production.QA.P02 callNextForm = new Sci.Production.QA.P02(this.CurrentMaintain["POID"].ToString());
            // callNextForm.ShowDialog(this);
        }
    }
}
