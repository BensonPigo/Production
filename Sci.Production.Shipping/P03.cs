using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P03
    /// </summary>
    public partial class P03 : Sci.Win.Tems.Input2
    {
        /// <summary>
        /// P03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.AllowUserToOrderColumns = true;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select 
[FactoryID] = (
	case when ed.PoType = 'M' and ed.FabricType = 'M' 
		then (
			select TOP 1 mpo.FactoryID 
			from SciMachine_MachinePO mpo, SciMachine_MachinePO_Detail mpod 
			where mpo.ID = mpod.ID and mpod.ID = ed.PoID and mpod.Seq1 = ed.Seq1 and mpod.seq2 = ed.Seq2
		)
        when ed.PoType = 'M' and ed.FabricType = 'P' 
		 then (
			select TOP 1 ppo.FactoryID 
			from SciMachine_PartPO ppo, SciMachine_PartPO_Detail ppod 
			where ppo.ID = ppod.ID and ppod.TPEPOID = ed.PoID and ppod.Seq1 = ed.Seq1 and ppod.seq2 = ed.Seq2
		) 
		when ed.PoType = 'M' and ed.FabricType = 'O' 
		then (
			select TOP 1 mpo.Factoryid 
			from SciMachine_MiscPO mpo, SciMachine_MiscPO_Detail mpod 
			where mpo.ID = mpod.ID and mpod.TPEPOID = ed.PoID and mpod.Seq1 = ed.Seq1 and mpod.seq2 = ed.Seq2
		) 
	else o.FactoryID end)
,o.ProjectID
,ed.PoID
,[SCIDlv] = (select min(SciDelivery) from Orders WITH (NOLOCK) where POID = ed.PoID and (Category = 'B' or Category = o.Category))
,[Category] = (
	case when o.Category = 'B' then 'Bulk' 
		 when o.Category = 'S' then 'Sample' 
		 when o.Category = 'M' then 'Material'
		 when o.Category = 'T' then 'Material' 
	else '' end)
,[InspDate] =iif(o.PFOrder = 1,dateadd(day,-10,o.SciDelivery)
	,iif((select CountryID from Factory WITH (NOLOCK) where ID = o.factoryID)='PH'
	,iif((select MrTeam from Brand WITH (NOLOCK) where ID = o.BrandID) = '01',dateadd(day,-15,o.SciDelivery),dateadd(day,-24,o.SciDelivery))
	,dateadd(day,-34,o.SciDelivery)))
,[Seq] = (SUBSTRING(ed.Seq1,1,3)+'-'+ed.Seq2)
,[Supp] = (ed.SuppID+'-'+s.AbbEN) 
,[Description] = iif(ed.Description = '',isnull(f.DescDetail,''),ed.Description)
,[FabricType] = (
	case when ed.PoType = 'M' and ed.FabricType = 'M' then 'Machine' 
		 when ed.PoType = 'M' and ed.FabricType = 'P' then 'Part' 
		 when ed.PoType = 'M' and ed.FabricType = 'O' then 'Miscellaneous' 
	else '' end)
,ed.UnitId
,isnull(psd.ColorID,'') as ColorID
,isnull(psd.SizeSpec,'') as SizeSpec
,ed.Qty
,ed.Foc
,ed.BalanceQty
,ed.NetKg
,ed.WeightKg
,iif(ed.IsFormA = 1,'Y','') as IsFormA
,ed.FormXType,ed.FormXReceived,ed.FormXDraftCFM,ed.FormXINV,ed.ID,ed.Seq1,ed.Seq2,ed.Ukey
,[FindColumn] = rtrim(ed.PoID)+(SUBSTRING(ed.Seq1,1,3)+'-'+ed.Seq2)
,[Preshrink] = iif(f.Preshrink = 1, 'V' ,'')
,c.FormXPayINV
,c.COName
,c.ReceiveDate
,c.SendDate
from Export_Detail ed WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = ed.PoID
left join Supp s WITH (NOLOCK) on s.id = ed.SuppID 
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join CertOfOrigin c WITH (NOLOCK) on c.SuppID=ed.SuppID and c.FormXPayINV=ed.FormXPayINV
where ed.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnCOInfo.Enabled = !this.EditMode;
            this.txtLocateSP.ReadOnly = false;
            this.txtLocateSP2.ReadOnly = false;
            this.chkReplacement.ReadOnly = true;
            this.chkDelay.ReadOnly = true;
            this.label21.Visible = MyUtility.Convert.GetString(this.CurrentMaintain["Junk"]) == "True" ? true : false; 
            this.labelFormE.Visible = MyUtility.Convert.GetString(this.CurrentMaintain["FormE"]) == "True" ? true : false;

            switch (MyUtility.Convert.GetString(this.CurrentMaintain["Payer"]))
            {
                case "S":
                    this.displayPayer.Value = "By Sci Taipei Office(Sender)";
                    break;
                case "M":
                    this.displayPayer.Value = "By Mill(Sender)";
                    break;
                case "F":
                    this.displayPayer.Value = "By Factory(Receiver)";
                    break;
                default:
                    this.displayPayer.Value = string.Empty;
                    break;
            }

            string sqlmainPrepaidFtyImportFee = $@"
select PrepaidFtyImportFee
from Export
where ID = '{this.CurrentMaintain["MainExportID08"]}'
";

            decimal intPrepaidFtyImportFee = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlmainPrepaidFtyImportFee));
            this.chkImportChange.Enabled = !(intPrepaidFtyImportFee > 0);
            #region Declaration ID
            if (!MyUtility.Check.Empty(this.CurrentMaintain["Blno"]))
            {
                string sqlcmd = $@"select ID from VNImportDeclaration where BLNo='{this.CurrentMaintain["Blno"]}' and IsFtyExport = 0";
                this.displayDeclarationID.Text = MyUtility.GetValue.Lookup(sqlcmd);
            }
            else
            {
                this.displayDeclarationID.Text = string.Empty;
            }

            if (MyUtility.Check.Empty(this.displayDeclarationID.Text))
            {
                string sqlcmd = $@"select ID from VNImportDeclaration where WKNo='{this.CurrentMaintain["ID"]}' and IsFtyExport = 0";
                this.displayDeclarationID.Text = MyUtility.GetValue.Lookup(sqlcmd);
            }
            #endregion
            #region CustomsDeclareNo
            if (!MyUtility.Check.Empty(this.CurrentMaintain["Blno"]))
            {
                string sqlcmd = $@"select DeclareNo from VNImportDeclaration where BLNo='{this.CurrentMaintain["Blno"]}' and IsFtyExport = 0";
                this.displayCustomsDeclareNo.Text = MyUtility.GetValue.Lookup(sqlcmd);
            }
            else
            {
                this.displayCustomsDeclareNo.Text = string.Empty;
            }

            if (MyUtility.Check.Empty(this.displayDeclarationID.Text))
            {
                string sqlcmd = $@"select DeclareNo from VNImportDeclaration where WKNo='{this.CurrentMaintain["ID"]}' and IsFtyExport = 0";
                this.displayCustomsDeclareNo.Text = MyUtility.GetValue.Lookup(sqlcmd);
            }
            #endregion
            #region Door to Door
            string chkdtd = $@"
select 1
from Door2DoorDelivery 
where ExportPort = '{this.CurrentMaintain["ExportPort"]}'
      and ExportCountry ='{this.CurrentMaintain["ExportCountry"]}'
      and ImportCountry = '{this.CurrentMaintain["ImportCountry"]}'
      and ShipModeID = '{this.CurrentMaintain["ShipModeID"]}'
      and Vessel ='{this.CurrentMaintain["Vessel"]}'
union 
select 1
from Door2DoorDelivery
where ExportPort = '{this.CurrentMaintain["ExportPort"]}'
      and ExportCountry ='{this.CurrentMaintain["ExportCountry"]}'
      and ImportCountry = '{this.CurrentMaintain["ImportCountry"]}'
      and ShipModeID = '{this.CurrentMaintain["ShipModeID"]}'
      and Vessel  =''
";
            this.ChkDoortoDoorDelivery.Checked = MyUtility.Check.Seek(chkdtd);
            #endregion

            this.ControlColor();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("FactoryID", header: "Prod. Factory", width: Widths.AnsiChars(7))
                .Text("ProjectID", header: "Project Name", width: Widths.AnsiChars(5))
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13))
                .Date("SCIDlv", header: "Earliest SCI Del", width: Widths.AnsiChars(9))
                .Text("Category", header: "Category", width: Widths.AnsiChars(8))
                .Date("InspDate", header: "Inspect Dead Line", width: Widths.AnsiChars(9))
                .Text("Seq", header: "SEQ", width: Widths.AnsiChars(3))
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(13))
                .EditText("Description", header: "Description", width: Widths.AnsiChars(6))
                .Text("FabricType", header: "MMS Type", width: Widths.AnsiChars(3))
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(4))
                .Text("ColorID", header: "Color", width: Widths.AnsiChars(5))
                .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(5))
                .Numeric("Qty", header: "Export Q'ty", decimal_places: 2, width: Widths.AnsiChars(5))
                .Numeric("Foc", header: "F.O.C.", decimal_places: 2, width: Widths.AnsiChars(2))
                .Numeric("BalanceQty", header: "Balance", decimal_places: 2, width: Widths.AnsiChars(2))
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2)
                .Numeric("WeightKg", header: "G.W.(kg)", decimal_places: 2)
                .Text("FormXPayINV", header: "Payment Invoice#", width: Widths.AnsiChars(16))
                .Text("COName", header: "Form C/O Name", width: Widths.AnsiChars(15))
                .Date("ReceiveDate", header: "Form Rcvd Date", width: Widths.AnsiChars(10))
                .Date("SendDate", header: "Form Send Date", width: Widths.AnsiChars(10))
                ;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DialogResult dResult;

            DataTable gridData;
            string sqlCmd = string.Empty;

            sqlCmd = string.Format(
                        @"select 1
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN SciFMS_AccountNo a on se.AccountID = a.ID
where se.WKNo = '{0}' and se.junk=0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (gridData.Rows.Count > 0 && this.chkImportChange.Checked)
            {
                if (MyUtility.Convert.GetDecimal(this.CurrentMaintain["TPEPaidUSD"]) > 0)
                {
                    MyUtility.Msg.WarningBox("WK has been shared expense,  [No Import Charge] shouldn't tick, please double check.");
                }
                else
                {
                    MyUtility.Msg.WarningBox("[Expense Data] already have data, please reconfirm.");
                    return false;
                }
            }

            // Arrive Port Date 不可晚於 Arrive W/H Date
            if (MyUtility.Convert.GetString(this.CurrentMaintain["ExportCountry"]).ToUpper() == MyUtility.Convert.GetString(this.CurrentMaintain["ImportCountry"]).ToUpper()
                && MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]).ToUpper() == "TRUCK")
            {
            }
            else
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["PortArrival"]) && !MyUtility.Check.Empty(this.CurrentMaintain["WhseArrival"]))
                {
                    // 到港日不可晚於到W/H日期
                    if (Convert.ToDateTime(this.CurrentMaintain["PortArrival"]) > Convert.ToDateTime(this.CurrentMaintain["WhseArrival"]))
                    {
                        MyUtility.Msg.WarningBox("< Arrive Port Date > can't later than < Arrive W/H Date >");
                        return false;
                    }
                }

                // 到港日要在ETA +-10天內 才詢問是否要Save
                // 如果超過+-10就不可存檔
                if (!MyUtility.Check.Empty(this.CurrentMaintain["PortArrival"]) && !MyUtility.Check.Empty(this.CurrentMaintain["Eta"]))
                {
                    // 到港日=ETA 可正常存檔
                    if (DateTime.Compare((DateTime)this.CurrentMaintain["PortArrival"], (DateTime)this.CurrentMaintain["Eta"]) == 0)
                    {
                        return true;
                    }

                    // 到港日早於ETA(到達日) 10天內
                    if (Convert.ToDateTime(this.CurrentMaintain["PortArrival"]) >= Convert.ToDateTime(this.CurrentMaintain["Eta"]).AddDays(-10) &&
                        DateTime.Compare((DateTime)this.CurrentMaintain["PortArrival"], (DateTime)this.CurrentMaintain["Eta"]) < 0)
                    {
                        dResult = MyUtility.Msg.QuestionBox("< Arrive Port Date > earlier than < ETA >." + Environment.NewLine + "Are you sure you want to save this data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                        if (dResult == DialogResult.No)
                        {
                            return false;
                        }
                    }

                    // 到港日不可晚於ETA 10天內
                    else if (Convert.ToDateTime(this.CurrentMaintain["PortArrival"]) <= Convert.ToDateTime(this.CurrentMaintain["Eta"]).AddDays(10) &&
                        DateTime.Compare((DateTime)this.CurrentMaintain["PortArrival"], (DateTime)this.CurrentMaintain["Eta"]) > 0)
                    {
                        dResult = MyUtility.Msg.QuestionBox("< Arrive Prot Date > later than < ETA >." + Environment.NewLine + "Are you sure you want to save this data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                        if (dResult == DialogResult.No)
                        {
                            return false;
                        }
                    }// 超過或小於10天就不可存檔
                    else
                    {
                        MyUtility.Msg.WarningBox("< Arrive Port Date > earlier or later more than <ETA> 10 days, Cannot be saved.");
                        return false;
                    }
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            Sci.Production.Shipping.P03_Print callNextForm = new Sci.Production.Shipping.P03_Print(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            string poID = this.txtLocateSP.Text + this.txtLocateSP2.Text;

            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("FindColumn", poID);
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        // Expense Data
        private void BtnExpenseData_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ExpenseData callNextForm = new Sci.Production.Shipping.P05_ExpenseData(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "WKNo", true);
            callNextForm.ShowDialog(this);
        }

        // Shipping Mark
        private void BtnShippingMark_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["ShipMarkDesc"]), "Shipping Mark", false, null);
            callNextForm.ShowDialog(this);
        }

        private void BtnBatchUpload_Click(object sender, EventArgs e)
        {
            if (!this.Perm.Edit)
            {
                MyUtility.Msg.WarningBox("You have no permission.");
                return;
            }

            P03_BatchUpload callNextForm = new P03_BatchUpload();
            callNextForm.ShowDialog(this);
            this.ReloadDatas();
        }

        private void ControlColor()
        {
            DataTable gridData;
            string sqlCmd = string.Empty;

            sqlCmd = string.Format(
                        @"select 1
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN SciFMS_AccountNo a on se.AccountID = a.ID
where se.WKNo = '{0}' and se.junk=0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (gridData.Rows.Count > 0)
            {
                this.btnExpenseData.ForeColor = Color.Blue;
            }
            else
            {
                this.btnExpenseData.ForeColor = Color.Black;
            }
        }

        private void BtnCOInfo_Click(object sender, EventArgs e)
        {
            new P03_COInformation(this.CurrentMaintain["ID"].ToString()).ShowDialog();
        }

        private void TxtCustomOTRespFty1_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                this.CustomOTCheck();
            }
        }

        private void TxtCustomOTRespFty2_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                this.CustomOTCheck();
            }
        }

        private void CustomOTCheck()
        {
            /*
             * 在寫入CurrentMaintain當下 會觸發TextboxChanged 導致原值被改變。
             * 只好先將原值存下，並且寫回CurrentMaintain。
             */
            bool chkChecked = !MyUtility.Check.Empty(this.txtCustomOTRespFty1.Text) ||
                              !MyUtility.Check.Empty(this.txtCustomOTRespFty2.Text);
            string customOTRespFty1 = this.txtCustomOTRespFty1.Text;
            string customOTRespFty2 = this.txtCustomOTRespFty2.Text;
            this.CurrentMaintain["CustomOTRespFty1"] = customOTRespFty1;
            this.CurrentMaintain["CustomOTRespFty2"] = customOTRespFty2;
            this.CurrentMaintain["CustomOT"] = chkChecked;
            this.CurrentMaintain.EndEdit();
        }
    }
}
