using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P21 : Sci.Win.Tems.Input6
    {

        public P21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.btnBatchConfirmResponsibilityDept.Enabled = this.Perm.Confirm;
            this.gridReplacement.ReadOnly = true;
        }

        protected override void OnDetailEntered()
        {
            if (this.CurrentMaintain != null)
            {
                this.lblConfirmDept.Visible = !MyUtility.Check.Empty(this.CurrentMaintain["RespDeptConfirmDate"]);

                this.txtSDPKPICode.Text = MyUtility.GetValue.Lookup($@"select KpiCode from Factory where id = '{this.CurrentMaintain["Department"]}'");
                this.numTotal.Value = (decimal)this.CurrentMaintain["RMtlAmtUSD"] + (decimal)this.CurrentMaintain["ActFreightUSD"] + (decimal)this.CurrentMaintain["OtherAmtUSD"];

                DataRow drOrder;
                string sqlcmd = $@"
select o.Poid,o.StyleID , o.Qty, [OrderID] = o.id, o.BrandID, o.FactoryID, o.ProgramID, o.MRHandle, [OrderSMR] = o.SMR
,[PoCombo] = (select isnull([dbo].getPOComboList(o.ID, o.POID),''))
,PO.POHandle, [POSMR] = PO.POSMR
,[SumQty] = PoQty.value
from orders o WITH (NOLOCK)
inner join po WITH (NOLOCK) on po.id = o.poid
outer apply(
	select value = sum(Qty)
	from Orders 
	where POID = po.ID
)PoQty
where o.id = '{this.CurrentMaintain["OrderID"]}'
";
                if (MyUtility.Check.Seek(sqlcmd, out drOrder))
                {
                    this.txtBrand.Text = drOrder["BrandID"].ToString();
                    this.txtFactory.Text = drOrder["FactoryID"].ToString();
                    this.txtStyle.Text = drOrder["StyleID"].ToString();
                    this.txtProgram.Text = drOrder["ProgramID"].ToString();
                    this.editPoCombo.Text = drOrder["PoCombo"].ToString();
                    this.txtPOHandle.DisplayBox1Binding = drOrder["POHandle"].ToString();
                    this.txtPoSMR.DisplayBox1Binding = drOrder["POSMR"].ToString();
                    this.txtMR.DisplayBox1Binding = drOrder["MRHandle"].ToString();
                    this.txtSMR.DisplayBox1Binding = drOrder["OrderSMR"].ToString();
                    this.numTotalQty.Value = MyUtility.Convert.GetDecimal(drOrder["SumQty"]);
                }
                else
                {
                    this.txtBrand.Text = string.Empty;
                    this.txtFactory.Text = string.Empty;
                    this.txtStyle.Text = string.Empty;
                    this.txtProgram.Text = string.Empty;
                    this.editPoCombo.Text = string.Empty;
                    this.txtPOHandle.DisplayBox1Binding = string.Empty;
                    this.txtPoSMR.DisplayBox1Binding = string.Empty;
                    this.txtMR.DisplayBox1Binding = string.Empty;
                    this.txtSMR.DisplayBox1Binding = string.Empty;
                    this.numTotalQty.Value = 0;
                }

                this.txtIssueSubject.Text = MyUtility.GetValue.Lookup($@"
select CONCAT(ID,' - ', Name) 
from Reason where ID = '{this.CurrentMaintain["IrregularPOCostID"]}' 
And Reason.ReasonTypeID = 'PO_IrregularCost'");

                this.btnResponsibilitydept.ForeColor = MyUtility.Check.Seek($"select 1 from ICR_ResponsibilityDept where id = '{this.CurrentMaintain["id"]}'") ? Color.Blue : Color.Black;

                // 取得ICR_ReplacementReport Grid 資料
                DataTable dt;
                DualResult result;
                string sqlcmd_Replace = $@"select ReplacementNo from ICR_ReplacementReport where id ='{this.CurrentMaintain["id"]}'";
                if (!(result = DBProxy.Current.Select(null, sqlcmd_Replace, out dt)))
                {
                    this.ShowErr(result);
                    return;
                }

                this.gridReplacement.DataSource = dt;
                this.gridReplacement.ClearSelection();

            }
            else
            {
                this.btnResponsibilitydept.ForeColor = Color.Black;
                this.txtSDPKPICode.Text = string.Empty;
                this.numTotal.Value = null;
                this.txtBrand.Text = string.Empty;
                this.txtFactory.Text = string.Empty;
                this.txtStyle.Text = string.Empty;
                this.txtProgram.Text = string.Empty;
                this.numTotalQty.Value = null;
                this.editPoCombo.Text = string.Empty;
                this.txtPOHandle.DisplayBox1Binding = string.Empty;
                this.txtPoSMR.DisplayBox1Binding = string.Empty;
                this.txtMR.DisplayBox1Binding = string.Empty;
                this.txtSMR.DisplayBox1Binding = string.Empty;
                this.gridReplacement.DataSource = null;
                this.txtIssueSubject.Text = string.Empty;
            }

            base.OnDetailEntered();
        }

        protected override void ClickLocate()
        {
            base.ClickLocate();
            this.OnDetailEntered();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"

select  
ICR2.Seq1
,ICR2.Seq2
,[SourceType] =  DropDownList.Name 
,ICR2.MtltypeID
,ICR2.ICRQty
,ICR2.ICRFoc
,ICR2.PriceUSD
,[IrgAmt] = DropDownList.amt
,[CreateBy] = (Select dbo.getTPEPass1_ExtNo(ICR2.AddName))
,[EditBy] = (Select dbo.getTPEPass1_ExtNo(ICR2.EditName))
,[WeaveType] = (SELECT WeaveTypeID FROM Fabric WHERE SCIRefno = 
	(SELECT SCIRefno FROM PO_Supp_Detail WHERE ID = o.POID AND Seq1 = ICR2.Seq1 AND Seq2 = ICR2.Seq2))
from ICR_Detail ICR2
inner join ICR on ICR.Id = ICR2.ID
inner join Orders o on o.ID = ICR.OrderID
outer apply(
	select DropDownList.Name
	, [amt] = (Select * from GetAmountByUnit(ICR2.PriceUSD, ICR2.ICRQty, PO_Supp_Detail.POUnit,2))
	, [WaveType] = WeaveTypeID
	from PO_Supp_Detail, Fabric, DropDownList 
	where PO_Supp_Detail.SCIRefno = Fabric.SCIRefno 
	and Fabric.Type = DropDownList.ID 
	and DropDownList.type = 'FabricType' 
	and PO_Supp_Detail.id= o.POID
	and PO_Supp_Detail.SEQ1= ICR2.Seq1
	and PO_Supp_Detail.SEQ2= icr2.Seq2
)DropDownList
where ICR.ID = '{masterID}';
";

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(6))
            .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(6))
            .Text("SourceType", header: "Source Type", width: Widths.AnsiChars(12))
            .Text("MtltypeID", header: "Irregular Mtl Type", width: Widths.AnsiChars(16))
            .Numeric("ICRQty", header: "Irregular Q'ty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8)
            .Numeric("ICRFoc", header: "Irregular FOC", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8)
            .Numeric("PriceUSD", header: "Irregular Price (USD)", width: Widths.AnsiChars(10), decimal_places: 4, integer_places: 16)
            .Numeric("IrgAmt", header: "Irregular Amt (USD)", width: Widths.AnsiChars(10), decimal_places: 4, integer_places: 16)
            .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(25))
            .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(25))
             .Text("WeaveType", header: "Weave Type", width: Widths.AnsiChars(16))
            ;

            // 設定Replacement Grid
            this.Helper.Controls.Grid.Generator(this.gridReplacement)
            .Text("ReplacementNo", header: "ReplacementNo", width: Widths.AnsiChars(16), alignment: DataGridViewContentAlignment.MiddleCenter)
            ;
            #endregion 欄位設定
        }

        private void BtnResponsibilitydept_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            bool canEdit = Prgs.GetAuthority(Sci.Env.User.UserID, "P21. Irregular Cost Report", "CanEdit");

            if (canEdit)
            {
                canEdit = MyUtility.Check.Seek($"select 1 from ICR with (nolock) where ID = '{this.CurrentMaintain["ID"]}' and VoucherDate is null and RespDeptConfirmDate is null ");
            }

            var frm = new P21_ResponsibilityDept(canEdit, this.CurrentMaintain["ID"].ToString(), null, null, string.Empty, this.Perm.Confirm, this.Perm.Unconfirm);
            frm.ShowDialog(this);
            frm.Dispose();
            this.OnRefreshClick();
            this.OnDetailEntered();
        }

        private void BtnBatchConfirmResponsibilityDept_Click(object sender, EventArgs e)
        {
            new P21_BatchConfirmRespDept().ShowDialog();
            this.ReloadDatas();
        }

        private void BtnBatchReCalculateResponsibilityDeptAmt_Click(object sender, EventArgs e)
        {
            new P21_BatchConfirmRespDept(true).ShowDialog();
            this.ReloadDatas();
        }
    }
}
