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
                @"select (case when ed.PoType = 'M' and ed.FabricType = 'M' then (select TOP 1 mpo.FactoryID from [Machine].dbo.MachinePO mpo, [Machine].dbo.MachinePO_Detail mpod where mpo.ID = mpod.ID and mpod.ID = ed.PoID and mpod.Seq1 = ed.Seq1 and mpod.seq2 = ed.Seq2)
             when ed.PoType = 'M' and ed.FabricType = 'P' then (select TOP 1 ppo.FactoryID from [Machine].dbo.PartPO ppo, [Machine].dbo.PartPO_Detail ppod where ppo.ID = ppod.ID and ppod.TPEPOID = ed.PoID and ppod.Seq1 = ed.Seq1 and ppod.seq2 = ed.Seq2) 
			 when ed.PoType = 'M' and ed.FabricType = 'O' then (select TOP 1 mpo.Factoryid from [Machine].dbo.MiscPO mpo, [Machine].dbo.MiscPO_Detail mpod where mpo.ID = mpod.ID and mpod.TPEPOID = ed.PoID and mpod.Seq1 = ed.Seq1 and mpod.seq2 = ed.Seq2) else o.FactoryID end) as FactoryID,
o.ProjectID,ed.PoID,(select min(SciDelivery) from Orders WITH (NOLOCK) where POID = ed.PoID and (Category = 'B' or Category = o.Category)) as SCIDlv,
(case when o.Category = 'B' then 'Bulk' when o.Category = 'S' then 'Sample' when o.Category = 'M' then 'Material' when o.Category = 'T' then 'Material' else '' end) as Category,
iif(o.PFOrder = 1,dateadd(day,-10,o.SciDelivery),iif((select CountryID from Factory WITH (NOLOCK) where ID = o.factoryID)='PH',iif((select MrTeam from Brand WITH (NOLOCK) where ID = o.BrandID) = '01',dateadd(day,-15,o.SciDelivery),dateadd(day,-24,o.SciDelivery)),dateadd(day,-34,o.SciDelivery))) as InspDate,
(SUBSTRING(ed.Seq1,1,3)+'-'+ed.Seq2) as Seq,(ed.SuppID+'-'+s.AbbEN) as Supp,
iif(ed.Description = '',isnull(f.DescDetail,''),ed.Description) as Description,
(case when ed.PoType = 'M' and ed.FabricType = 'M' then 'Machine' when ed.PoType = 'M' and ed.FabricType = 'P' then 'Part' when ed.PoType = 'M' and ed.FabricType = 'O' then 'Miscellaneous' else '' end) as FabricType,
ed.UnitId,isnull(psd.ColorID,'') as ColorID,isnull(psd.SizeSpec,'') as SizeSpec,ed.Qty,ed.Foc,ed.BalanceQty,
ed.NetKg,ed.WeightKg,iif(ed.IsFormA = 1,'Y','') as IsFormA,ed.FormXType,ed.FormXReceived,ed.FormXDraftCFM,ed.FormXINV,ed.ID,ed.Seq1,ed.Seq2,ed.Ukey,rtrim(ed.PoID)+(SUBSTRING(ed.Seq1,1,3)+'-'+ed.Seq2) as FindColumn,
Preshrink = iif(f.Preshrink = 1, 'V' ,'')
from Export_Detail ed WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = ed.PoID
left join Supp s WITH (NOLOCK) on s.id = ed.SuppID 
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
where ed.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.txtLocateSP.ReadOnly = false;
            this.txtLocateSP2.ReadOnly = false;
            this.label21.Visible = MyUtility.Convert.GetString(this.CurrentMaintain["Junk"]) == "True" ? true : false;
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
                .Text("IsFormA", header: "FormX Needed", width: Widths.AnsiChars(1))
                .Text("FormXType", header: "FormX Type", width: Widths.AnsiChars(8))
                .Date("FormXReceived", header: "FoemX Rcvd")
                .Date("FormXDraftCFM", header: "FormX Sent")
                .EditText("FormXINV", header: "FormX Invoice No.");
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DialogResult dResult;

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

            // 已經有做出口費用分攤，不能勾選[No Import Charge]
            if (MyUtility.Check.Seek(string.Format(@"select WKNO from ShareExpense WITH (NOLOCK) where WKNO = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
                && this.chkImportChange.Checked)
            {
                MyUtility.Msg.WarningBox("This WK# has share expense, please unselect [No Import Charge].");
                return false;
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
    }
}
