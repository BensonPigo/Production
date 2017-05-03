using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    public partial class P01 : Sci.Win.Tems.Input1
    {
        public P01(ToolStripMenuItem menuitem, string Type)
            : base(menuitem)
        {
            InitializeComponent();
            this.Text = Type == "1" ? "P01. Clog Master List" : "P011. Clog Master List (History)";
            this.DefaultFilter = Type == "1" ? string.Format("MDivisionID = '{0}' AND IsForecast = 0 AND Finished = 0", Sci.Env.User.Keyword) : string.Format("MDivisionID = '{0}' AND IsForecast = 0 AND Finished = 1", Sci.Env.User.Keyword);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataRow brandData;
            displayDescription.Value = MyUtility.GetValue.Lookup(string.Format("select Description from style WITH (NOLOCK) where Ukey = {0}", CurrentMaintain["StyleUkey"].ToString()));
            displayPOCombo.Value = MyUtility.GetValue.Lookup(string.Format("select [dbo].getPOComboList('{0}','{1}') as PoList from Orders WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString(), CurrentMaintain["POID"].ToString()));
            if (MyUtility.Check.Seek(string.Format("select ID,Customize1,Customize2,Customize3 from Brand WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["BrandID"].ToString()), out brandData))
            {
                labelSpecialId1.Text = brandData["Customize1"].ToString();
                labelSpecialId2.Text = brandData["Customize2"].ToString();
                labelSpecialId3.Text = brandData["Customize3"].ToString();
            }
            else
            {
                labelSpecialId1.Text = "";
                labelSpecialId2.Text = "";
                labelSpecialId3.Text = "";
            }
            displayActPullout.Value = CurrentMaintain["PulloutComplete"].ToString().ToUpper() == "TRUE" ? "OK" : MyUtility.GetValue.Lookup(string.Format("select COUNT(distinct ID) as CntID from Pullout_Detail where OrderID = '{0}' and ShipQty > 0", CurrentMaintain["ID"].ToString()));
            displayCFAFinalInspDate.Value = CurrentMaintain["InspResult"].ToString() == "P" ? "Pass" : CurrentMaintain["InspResult"].ToString() == "F" ? "Fail" : "";

            numCtnQtyOnTransit.Value = MyUtility.Convert.GetInt(CurrentMaintain["FtyCTN"]) - MyUtility.Convert.GetInt(CurrentMaintain["ClogCTN"]);
            numCtnQtyInFactory.Value = MyUtility.Convert.GetInt(CurrentMaintain["TotalCTN"]) - MyUtility.Convert.GetInt(CurrentMaintain["FtyCTN"]);
            numttlCtnTransferred.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCTN"]) == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["ClogCTN"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCTN"]) * 100, 2);

            //按鈕變色
            bool haveOrder_Qty = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString()));
            btnQuantityBreakdown.ForeColor = haveOrder_Qty ? Color.Blue : Color.Black;
            btnQtyBDownByShipmode.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            btnProductionOutput.ForeColor = haveOrder_Qty ? Color.Blue : Color.Black;
            btnGarmentExport.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            btnCartonSize.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_CTNData WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            btnCartonStatus.ForeColor = MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' and ReceiveDate is not null", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            btnOrderRemark.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["OrderRemark"]) ? Color.Blue : Color.Black;
            btnCMPQSheet.Enabled = !MyUtility.Check.Empty(CurrentMaintain["CMPQDate"]);
            if (btnCMPQSheet.Enabled)
            {
                btnCMPQSheet.ForeColor = !MyUtility.Check.Empty(CurrentMaintain["Packing"]) || !MyUtility.Check.Empty(CurrentMaintain["MarkFront"]) || !MyUtility.Check.Empty(CurrentMaintain["Label"]) || haveOrder_Qty || MyUtility.Check.Seek(string.Format("select ID from Order_Article where ID = '{0}'", CurrentMaintain["ID"].ToString())) || MyUtility.Check.Seek(string.Format("select ID from Order_SizeCode where ID = '{0}'", CurrentMaintain["POID"].ToString())) || MyUtility.Check.Seek(string.Format("select ID from Order_ColorCombo where ID = '{0}'", CurrentMaintain["POID"].ToString())) || MyUtility.Check.Seek(string.Format("select ID from Orders where POID = '{0}' and ID != '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            }
            else
            {
                btnCMPQSheet.ForeColor = Color.Black;
            }
            btnFabricInspectionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from FIR WITH (NOLOCK) where POID = '{0}'", CurrentMaintain["POID"].ToString())) ? Color.Blue : Color.Black;
            btnCFARFTList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Cfa WITH (NOLOCK) where OrderID = '{0}'", CurrentMaintain["ID"].ToString())) || MyUtility.Check.Seek(string.Format("select OrderID from Rft where OrderID = '{0}'", CurrentMaintain["ID"].ToString())) ? Color.Blue : Color.Black;
            btnAccessoryInspectionList.ForeColor = MyUtility.Check.Seek(string.Format("select ID from AIR WITH (NOLOCK) where POID = '{0}'", CurrentMaintain["POID"].ToString())) ? Color.Blue : Color.Black;
        }

        //Quantity breakdown
        private void btnQuantityBreakdown_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["POID"]), MyUtility.Convert.GetString(displayPOCombo.Value));
            callNextForm.ShowDialog(this);
        }

        //Q'ty b'down by shipmode
        private void btnQtyBDownByShipmode_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyShip callNextForm = new Sci.Production.PPIC.P01_QtyShip(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["POID"]));
            callNextForm.ShowDialog(this);
        }

        //Production output
        private void btnProductionOutput_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput callNextForm = new Sci.Production.PPIC.P01_ProductionOutput(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Garment Export
        private void btnGarmentExport_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_GMTExport callNextForm = new Sci.Production.PPIC.P01_GMTExport(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Carton Size
        private void btnCartonSize_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P01_CTNData callNextForm = new Sci.Production.Packing.P01_CTNData(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Carton Status
        private void btnCartonStatus_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_CTNStatus callNextForm = new Sci.Production.PPIC.P01_CTNStatus(CurrentMaintain["ID"].ToString(),true);
            callNextForm.ShowDialog(this);
            this.RenewData();
            numCtnQtyOnTransit.Value = MyUtility.Convert.GetInt(CurrentMaintain["FtyCTN"]) - MyUtility.Convert.GetInt(CurrentMaintain["ClogCTN"]);
            numCtnQtyInFactory.Value = MyUtility.Convert.GetInt(CurrentMaintain["TotalCTN"]) - MyUtility.Convert.GetInt(CurrentMaintain["FtyCTN"]);
            numttlCtnTransferred.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCTN"]) == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["ClogCTN"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCTN"]) * 100, 2);
        }

        //Order remark
        private void btnOrderRemark_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["OrderRemark"].ToString(), "Order Remark", false, null);
            callNextForm.ShowDialog(this);
        }

        //Fabric inspection list
        private void btnFabricInspectionList_Click(object sender, EventArgs e)
        {
            //等QA -> P01開發完成後呼叫
            //Sci.Production.QA.P01 callNextForm = new Sci.Production.QA.P01(CurrentMaintain["POID"].ToString());
            //callNextForm.ShowDialog(this);
        }

        //CFA && RFT list
        private void btnCFARFTList_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P01_CFAAndRFTList callNextForm = new Sci.Production.Logistic.P01_CFAAndRFTList(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Accessory inspection list
        private void btnAccessoryInspectionList_Click(object sender, EventArgs e)
        {
            //等QA -> P02開發完成後呼叫
            //Sci.Production.QA.P02 callNextForm = new Sci.Production.QA.P02(CurrentMaintain["POID"].ToString());
            //callNextForm.ShowDialog(this);
        }
    }
}
