using System;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P02 : Win.Tems.Input2
    {
        private string _wkNo;
        private string _spNo;

        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.AllowUserToOrderColumns = true;
        }

        public P02(string wkNo, string spNo, ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.AllowUserToOrderColumns = true;

            this._wkNo = wkNo;
            this._spNo = spNo;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            if (!MyUtility.Check.Empty(this._wkNo))
            {
                this.DefaultWhere = $"ID='{this._wkNo}'";
                this.ReloadDatas();
            }

            if (!MyUtility.Check.Empty(this._spNo))
            {
                this.DefaultWhere = $"(exists( select 1 from Export_Detail where id = Export.Id and Export_Detail.PoID  =  '{this._spNo}' ))";
                this.ReloadDatas();
            }
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            DataRow dr = this.grid.GetDataRow<DataRow>(this.grid.GetSelectedRowIndex());
            string sqlcmd;
            #region -- 檢查export detail是否含有成衣物料 --

            sqlcmd = string.Format(
                @"
select 1 chk 
where exists(   
    select * 
    from dbo.Export_Detail ed WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = ed.PoID and pd.seq1 = ed.seq1 and pd.seq2 = ed.Seq2 
    where ed.ID='{0}'
)", this.CurrentMaintain["id"]);
            if (MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.WarningBox("The shipment have material, can't revise < Arrive W/H Date > or < P/L Rcv Date >.", "Warning");
                return false;
            }

            #endregion

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select  FactoryID = iif(ed.potype='M'
                        , (case when ed.FabricType = 'M' then ( select mpo.FactoryID 
                                                                from SciMachine_MachinePO mpo
                                                                     , SciMachine_MachinePO_Detail mpod 
                                                                where   mpo.ID = mpod.ID 
                                                                        and mpod.ID = ed.PoID 
                                                                        and mpod.Seq1 = ed.Seq1 
                                                                        and mpod.seq2 = ed.Seq2)
                                when ed.FabricType = 'P' then ( select top 1 ppo.FactoryID 
                                                                from SciMachine_PartPO ppo
                                                                     , SciMachine_PartPO_Detail ppod 
                                                                where   ppo.ID = ppod.ID 
                                                                        and ppod.TPEPOID = ed.PoID 
                                                                        and ppod.Seq1 = ed.Seq1 
                                                                        and ppod.seq2 = ed.Seq2) 
			                    when ed.FabricType = 'O' then ( select mpo.Factoryid 
                                                                from SciMachine_MiscPO mpo
                                                                     , SciMachine_MiscPO_Detail mpod 
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
		, [ContainerType]= Container.Val
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
        , ed.Carton
		, o.OrderTypeID
from Export_Detail ed WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = ed.PoID
left join Supp s WITH (NOLOCK) on s.id = ed.SuppID 
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
OUTER APPLY(
		SELECT [Val] = STUFF((
		SELECT DISTINCT ','+esc.ContainerType + '-' +esc.ContainerNo
		FROM Export_ShipAdvice_Container esc
		WHERE esc.Export_Detail_Ukey=ed.Ukey
		AND esc.ContainerType <> '' AND esc.ContainerNo <> ''
		FOR XML PATH('')
	),1,1,'')
)Container
where ed.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.txtLocateSP.ReadOnly = false;
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

            this.btnShippingMark.Enabled = !MyUtility.Check.Empty(MyUtility.Convert.GetString(this.CurrentMaintain["ShipMarkDesc"]));
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                var frm = new P02_Cartondetail(MyUtility.Convert.GetString(dr["Ukey"]));
                frm.ShowDialog(this);
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("FactoryID", header: "Prod. Factory", width: Widths.AnsiChars(7))
                .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(13))
                .Text("ProjectID", header: "Project Name", width: Widths.AnsiChars(5))
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13))
                .Text("Carton", header: "Carton#", width: Widths.AnsiChars(8), iseditingreadonly: true, settings: ts)
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
                .Text("ContainerType", header: "ContainerType & No", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("IsFormA", header: "FormX Needed", width: Widths.AnsiChars(1))
                .Text("FormXType", header: "FormX Type", width: Widths.AnsiChars(8))
                .Date("FormXReceived", header: "FoemX Rcvd")
                .Date("FormXDraftCFM", header: "FormX Sent")
                .EditText("FormXINV", header: "FormX Invoice No.");
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DateTime arrivePortDate;
            DateTime whseArrival;
            DateTime eTA;
            bool chk;
            string msg;

            // Arrive Port Date 不可晚於 Arrive W/H Date
            if (!MyUtility.Check.Empty(this.CurrentMaintain["PortArrival"]) && !MyUtility.Check.Empty(this.CurrentMaintain["WhseArrival"]))
            {
                arrivePortDate = DateTime.Parse(this.CurrentMaintain["PortArrival"].ToString());
                whseArrival = DateTime.Parse(this.CurrentMaintain["WhseArrival"].ToString());
                if (!(chk = PublicPrg.Prgs.CheckArrivedWhseDateWithArrivedPortDate(arrivePortDate, whseArrival, out msg)))
                {
                    MyUtility.Msg.WarningBox(msg);
                    return false;
                }
            }

            eTA = DateTime.Parse(this.CurrentMaintain["eta"].ToString()); // eta
            whseArrival = DateTime.Parse(this.CurrentMaintain["WhseArrival"].ToString());

            // 到倉日如果早於ETA 3天，則提示窗請USER再確認是否存檔。
            // 到倉日如果晚於ETA 15天，則提示窗請USER再確認是否存檔。
            if (!(chk = PublicPrg.Prgs.CheckArrivedWhseDateWithEta(eTA, whseArrival, out msg)))
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox(msg);
                if (dResult == DialogResult.No)
                {
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            Shipping.P03_Print callNextForm = new Shipping.P03_Print(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = -1;

            // 判斷 Poid
            if (this.txtSeq1.CheckEmpty(showErrMsg: false))
            {
                index = this.detailgridbs.Find("poid", this.txtLocateSP.Text.TrimEnd());
            }

            // 判斷 Poid + Seq1
            else if (this.txtSeq1.CheckSeq2Empty())
            {
                index = this.detailgridbs.Find("PoidSeq1", this.txtLocateSP.Text.TrimEnd() + this.txtSeq1.Seq1);
            }

            // 判斷 Poid + Seq1 + Seq2
            else
            {
                index = this.detailgridbs.Find("PoidSeq", this.txtLocateSP.Text.TrimEnd() + this.txtSeq1.GetSeq());
            }

            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        // Shipping Mark
        private void BtnShippingMark_Click(object sender, EventArgs e)
        {
            Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["ShipMarkDesc"]), "Shipping Mark", false, null);
            callNextForm.ShowDialog(this);
        }
    }
}
