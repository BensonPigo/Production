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

namespace Sci.Production.Warehouse
{
    public partial class P02 : Sci.Win.Tems.Input2
    {
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            detailgrid.AllowUserToOrderColumns = true;
        }
        protected override bool ClickEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            string sqlcmd;
            #region -- 檢查export detail是否含有成衣物料 --

            sqlcmd = string.Format(@"
select 1 chk 
where exists(   
    select * 
    from dbo.Export_Detail ed WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = ed.PoID and pd.seq1 = ed.seq1 and pd.seq2 = ed.Seq2 
    where ed.ID='{0}'
)", CurrentMaintain["id"]);
            if (MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.WarningBox("The shipment have material, can't revise < Arrive W/H Date >.", "Warning");
                return false;
            }

            #endregion

            return base.ClickEditBefore();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"
select  FactoryID = iif(ed.potype='M'
                        , (case when ed.FabricType = 'M' then ( select mpo.FactoryID 
                                                                from [Machine].dbo.MachinePO mpo
                                                                     , [Machine].dbo.MachinePO_Detail mpod 
                                                                where   mpo.ID = mpod.ID 
                                                                        and mpod.ID = ed.PoID 
                                                                        and mpod.Seq1 = ed.Seq1 
                                                                        and mpod.seq2 = ed.Seq2)
                                when ed.FabricType = 'P' then ( select top 1 ppo.FactoryID 
                                                                from [Machine].dbo.PartPO ppo
                                                                     , [Machine].dbo.PartPO_Detail ppod 
                                                                where   ppo.ID = ppod.ID 
                                                                        and ppod.TPEPOID = ed.PoID 
                                                                        and ppod.Seq1 = ed.Seq1 
                                                                        and ppod.seq2 = ed.Seq2) 
			                    when ed.FabricType = 'O' then ( select mpo.Factoryid 
                                                                from [Machine].dbo.MiscPO mpo
                                                                     , [Machine].dbo.MiscPO_Detail mpod 
                                                                where   mpo.ID = mpod.ID 
                                                                        and mpod.TPEPOID = ed.PoID 
                                                                        and mpod.Seq1 = ed.Seq1 
                                                                        and mpod.seq2 = ed.Seq2) 
			                    else o.FactoryID 
                            end)
                        ,o.FactoryID)
        , o.ProjectID
        , ed.PoID
        , SCIDlv = (select min(SciDelivery) 
                    from Orders WITH (NOLOCK) 
                    where POID = ed.PoID and (Category = 'B' or Category = o.Category))
        , Category = (case  when o.Category = 'B' then 'Bulk' 
                            when o.Category = 'S' then 'Sample' 
                            when o.Category = 'M' then 'Material' 
                            else '' 
                      end)
        , InspDate =iif(o.PFOrder = 1, dateadd(day,-10,o.SciDelivery)
                        , iif(( select CountryID 
                                from Factory WITH (NOLOCK) 
                                where ID = o.factoryID)='PH'
                               , iif((  select MrTeam 
                                        from Brand WITH (NOLOCK) 
                                        where ID = o.BrandID) = '01'
                                    , dateadd(day,-15,o.SciDelivery)
                                    , dateadd(day,-24,o.SciDelivery))
                                , dateadd(day,-34
                        , o.SciDelivery))) 
        , (SUBSTRING(ed.Seq1,1,3)+' '+ed.Seq2) as Seq
        , (ed.SuppID+'-'+s.AbbEN) as Supp
        , Description = iif(ed.Description = '', isnull(f.DescDetail,'')
                                               , ed.Description)
        , FabricType = iif(ed.potype='M', (case when ed.FabricType = 'M' then 'Machine' 
                                                when ed.FabricType = 'P' then 'Part' 
                                                when ed.FabricType = 'O' then 'Miscellaneous' 
                                                else '' 
                                           end)
                                        , '')
        , ed.UnitId
        , ColorID = isnull(psd.ColorID,'')
        , SizeSpec = isnull(psd.SizeSpec,'')
        , ed.Qty
        , ed.Foc
        , ed.BalanceQty
        , ed.NetKg
        , ed.WeightKg
        , IsFormA = iif(ed.IsFormA = 1,'Y','')
        , ed.FormXType
        , ed.FormXReceived
        , ed.FormXDraftCFM
        , ed.FormXINV
        , ed.ID
        , ed.Seq1
        , ed.Seq2
        , ed.Ukey
        , PoidSeq1 = rtrim(ed.Poid) + Ltrim(Rtrim(ed.Seq1))
        , PoidSeq = rtrim(ed.PoID)+(Ltrim(Rtrim(ed.Seq1)) + ' ' + ed.Seq2)
        , Preshrink = iif(f.Preshrink = 1, 'V' ,'')
from Export_Detail ed WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = ed.PoID
left join Supp s WITH (NOLOCK) on s.id = ed.SuppID 
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
where ed.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            txtLocateSP.ReadOnly = false;
            label21.Visible = MyUtility.Convert.GetString(CurrentMaintain["Junk"]) == "True" ? true : false;
            switch (MyUtility.Convert.GetString(CurrentMaintain["Payer"]))
            {
                case "S":
                    displayPayer.Value = "By Sci Taipei Office(Sender)";
                    break;
                case "M":
                    displayPayer.Value = "By Mill(Sender)";
                    break;
                case "F":
                    displayPayer.Value = "By Factory(Receiver)";
                    break;
                default:
                    displayPayer.Value = "";
                    break;
            }
            btnShippingMark.Enabled = !MyUtility.Check.Empty(MyUtility.Convert.GetString(CurrentMaintain["ShipMarkDesc"])) ;
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("FactoryID", header: "Prod. Factory", width: Widths.AnsiChars(7))
                .Text("ProjectID", header: "Project Name", width: Widths.AnsiChars(5))
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13))
                .Date("SCIDlv", header: "Earliest SCI Del", width: Widths.AnsiChars(9))
                .Text("Category", header: "Category", width: Widths.AnsiChars(8))
                .Date("InspDate", header: "Inspect Dead Line", width: Widths.AnsiChars(9))
                .Text("Seq", header: "SEQ", width: Widths.AnsiChars(3))
                .Text("Preshrink", header: "Preshrink", iseditingreadonly: true)
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

        protected override bool ClickSaveBefore()
        {
            DateTime ArrivePortDate;
            DateTime WhseArrival;
            DateTime ETA;
            bool chk;
            String msg;

            //Arrive Port Date 不可晚於 Arrive W/H Date
            if (!MyUtility.Check.Empty(CurrentMaintain["PortArrival"]) && !MyUtility.Check.Empty(CurrentMaintain["WhseArrival"]))
            {
                ArrivePortDate = DateTime.Parse(CurrentMaintain["PortArrival"].ToString());
                WhseArrival = DateTime.Parse(CurrentMaintain["WhseArrival"].ToString());
                if (!(chk = Production.PublicPrg.Prgs.CheckArrivedWhseDateWithArrivedPortDate(ArrivePortDate, WhseArrival, out msg)))
                {
                    MyUtility.Msg.WarningBox(msg);
                    return false;
                }
            }

            ETA = DateTime.Parse(CurrentMaintain["eta"].ToString());//eta
            WhseArrival = DateTime.Parse(CurrentMaintain["WhseArrival"].ToString());
            // 到倉日如果早於ETA 3天，則提示窗請USER再確認是否存檔。
            // 到倉日如果晚於ETA 15天，則提示窗請USER再確認是否存檔。
            if (!(chk = Production.PublicPrg.Prgs.CheckArrivedWhseDateWithEta(ETA, WhseArrival, out msg)))
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox(msg);
                if (dResult == DialogResult.No) return false;
            }

            return base.ClickSaveBefore();
        }

        protected override bool ClickPrint()
        {
            Sci.Production.Shipping.P03_Print callNextForm = new Sci.Production.Shipping.P03_Print(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        //Find
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = -1;

            //判斷 Poid
            if (txtSeq1.checkEmpty(showErrMsg: false))
            {
                index = detailgridbs.Find("poid", txtLocateSP.Text.TrimEnd());
            }
            //判斷 Poid + Seq1
            else if (txtSeq1.checkSeq2Empty())
            {
                index = detailgridbs.Find("PoidSeq1", txtLocateSP.Text.TrimEnd() + txtSeq1.seq1);
            }
            //判斷 Poid + Seq1 + Seq2
            else
            {
                index = detailgridbs.Find("PoidSeq", txtLocateSP.Text.TrimEnd() + txtSeq1.getSeq());
            }

            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        //Shipping Mark
        private void btnShippingMark_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(CurrentMaintain["ShipMarkDesc"]), "Shipping Mark", false, null);
            callNextForm.ShowDialog(this);
        }

    }
}
