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
    public partial class P03 : Sci.Win.Tems.Input2
    {
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            detailgrid.AllowUserToOrderColumns = true;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select (case when ed.FabricType = 'M' then (select mpo.FactoryID from [Machine].dbo.MachinePO mpo, [Machine].dbo.MachinePO_Detail mpod where mpo.ID = mpod.ID and mpod.TPEPOID = ed.PoID and mpod.Seq1 = ed.Seq1 and mpod.seq2 = ed.Seq2)
             when ed.FabricType = 'P' then (select ppo.FactoryID from [Machine].dbo.PartPO ppo, [Machine].dbo.PartPO_Detail ppod where ppo.ID = ppod.ID and ppod.TPEPOID = ed.PoID and ppod.Seq1 = ed.Seq1 and ppod.seq2 = ed.Seq2) 
			 when ed.FabricType = 'O' then (select mpo.Factoryid from MiscPO mpo, MiscPO_Detail mpod where mpo.ID = mpod.ID and mpod.TPEPOID = ed.PoID and mpod.Seq1 = ed.Seq1 and mpod.seq2 = ed.Seq2) else o.FactoryID end) as FactoryID,
o.ProjectID,ed.PoID,(select min(SciDelivery) from Orders where POID = ed.PoID and (Category = 'B' or Category = o.Category)) as SCIDlv,
(case when o.Category = 'B' then 'Bulk' when o.Category = 'S' then 'Sample' when o.Category = 'M' then 'Material' else '' end) as Category,
iif(o.PFOrder = 1,dateadd(day,-10,o.SciDelivery),iif((select CountryID from Factory where ID = o.factoryID)='PH',iif((select MrTeam from Brand where ID = o.BrandID) = '01',dateadd(day,-15,o.SciDelivery),dateadd(day,-24,o.SciDelivery)),dateadd(day,-34,o.SciDelivery))) as InspDate,
(SUBSTRING(ed.Seq1,1,3)+'-'+ed.Seq2) as Seq,(ed.SuppID+'-'+s.AbbEN) as Supp,
iif(ed.Description = '',isnull(f.DescDetail,''),ed.Description) as Description,
(case when ed.FabricType = 'M' then 'Machine' when ed.FabricType = 'P' then 'Part' when ed.FabricType = 'O' then 'Miscellaneous' else '' end) as FabricType,
ed.UnitId,isnull(psd.ColorID,'') as ColorID,isnull(psd.SizeSpec,'') as SizeSpec,ed.Qty,ed.Foc,ed.BalanceQty,
ed.NetKg,ed.WeightKg,iif(ed.IsFormA = 1,'Y','') as IsFormA,ed.FormXType,ed.FormXReceived,ed.FormXDraftCFM,ed.FormXINV,ed.ID,ed.Seq1,ed.Seq2,ed.Ukey
from Export_Detail ed
left join Orders o on o.ID = ed.PoID
left join Supp s on s.id = ed.SuppID 
left join PO_Supp_Detail psd on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f on f.SCIRefno = psd.SCIRefno
where ed.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            label21.Visible = CurrentMaintain["Junk"].ToString() == "True" ? true : false;
            switch (CurrentMaintain["Payer"].ToString())
            {
                case "S":
                    displayBox7.Value = "By Sci Taipei Office(Sender)";
                    break;
                case "M":
                    displayBox7.Value = "By Mill(Sender)";
                    break;
                case "F":
                    displayBox7.Value = "By Factory(Receiver)";
                    break;
                default:
                    displayBox7.Value = "";
                    break;
            }
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("FactoryID", header: "Prod. Factory", width: Widths.AnsiChars(8))
                .Text("ProjectID", header: "Project Name", width: Widths.AnsiChars(5))
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13))
                .Date("SCIDlv",header:"Earliest SCI Del")
                .Text("Category", header: "Category", width: Widths.AnsiChars(8))
                .Date("InspDate", header: "Inspect Dead Line")
                .Text("Seq", header: "SEQ", width: Widths.AnsiChars(6))
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(20))
                .EditText("Description", header: "Description")
                .Text("FabricType", header: "MMS Type", width: Widths.AnsiChars(13))
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(8))
                .Text("ColorID", header: "Color", width: Widths.AnsiChars(6))
                .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(15))
                .Numeric("Qty", header: "Export Q'ty", decimal_places: 2)
                .Numeric("Foc", header: "F.O.C.", decimal_places: 2)
                .Numeric("BalanceQty", header: "Balance", decimal_places: 2)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2)
                .Numeric("WeightKg", header: "N.W.(kg)", decimal_places: 2)
                .Text("IsFormA", header: "FormX Needed", width: Widths.AnsiChars(1))
                .Text("FormXType", header: "FormX Type", width: Widths.AnsiChars(8))
                .Date("FormXReceived", header: "FoemX Rcvd")
                .Date("FormXDraftCFM", header: "FormX Sent")
                .EditText("FormXINV", header: "FormX Invoice No.");
        }

        protected override bool ClickSaveBefore()
        {
            //Arrive Port Date 不可晚於 Arrive W/H Date
            if (!MyUtility.Check.Empty(CurrentMaintain["PortArrival"]) && !MyUtility.Check.Empty(CurrentMaintain["WhseArrival"]) && Convert.ToDateTime(CurrentMaintain["PortArrival"]) > Convert.ToDateTime(CurrentMaintain["WhseArrival"]))
            {
                MyUtility.Msg.WarningBox("< Arrive Port Date > can't later than < Arrive W/H Date >");
                return false;
            }

            //ETA <= Arrive Port Date <= ETA+10
            if (!MyUtility.Check.Empty(CurrentMaintain["PortArrival"]) && !MyUtility.Check.Empty(CurrentMaintain["Eta"]))
            {
                if (Convert.ToDateTime(CurrentMaintain["PortArrival"]) < Convert.ToDateTime(CurrentMaintain["Eta"]))
                {
                    DialogResult buttonResult = MyUtility.Msg.WarningBox("< Arrive Port Date > earlier than < ETA >. Are you sure you want to save this data?", "Warning", MessageBoxButtons.YesNo);
                    if (buttonResult == System.Windows.Forms.DialogResult.No)
                    {
                        return false;
                    }
                }

                if (Convert.ToDateTime(CurrentMaintain["PortArrival"]) > Convert.ToDateTime(CurrentMaintain["Eta"]).AddDays(10))
                {
                    DialogResult buttonResult = MyUtility.Msg.WarningBox("< Arrive Port Date > later than < ETA > + 10 days. Are you sure you want to save this data?", "Warning", MessageBoxButtons.YesNo);
                    if (buttonResult == System.Windows.Forms.DialogResult.No)
                    {
                        return false;
                    }
                }
            }

            return base.ClickSaveBefore();
        }

        //Find
        private void button3_Click(object sender, EventArgs e)
        {
            string poID, seq;
            poID = textBox1.Text.Trim();
            seq = textBox2.Text.Trim();
            
            DataRow dr = (((DataTable)detailgridbs.DataSource).ToList<DataRow>()).FirstOrDefault<DataRow>(x => x["POID"].ToString().Trim()== poID && x["Seq"].ToString() == seq);
            if (dr != null)
            {
                int pos = (((DataTable)detailgridbs.DataSource).ToList<DataRow>()).IndexOf(dr);
                if (pos != -1)
                {
                    detailgridbs.Position = pos;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Data is not found!");
            }
        }

        //Expense Data
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ExpenseData callNextForm = new Sci.Production.Shipping.P05_ExpenseData(CurrentMaintain["ID"].ToString(), "WKNo");
            callNextForm.ShowDialog(this);
        }

        //Shipping Mark
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["ShipMarkDesc"].ToString(), "Shipping Mark", false, null);
            callNextForm.ShowDialog(this);
        }

    }
}
