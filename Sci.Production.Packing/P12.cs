using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P12
    /// </summary>
    public partial class P12 : Win.Tems.QueryForm
    {
        private DualResult result;
        private DataTable gridData;
        private DataGridViewGeneratorNumericColumnSettings clogctn = new DataGridViewGeneratorNumericColumnSettings();

        /// <summary>
        /// P12
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // Exp P/out date預設帶出下個月的最後一天
            this.dateExpPoutDate.Value = DateTime.Today.AddMonths(2).AddDays(1 - DateTime.Today.AddMonths(2).Day - 1);
            this.comboDropDownListCategory.SelectedIndex = 0;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.gridDetail.IsEditingReadOnly = true;
            this.gridDetail.DataSource = this.listControlBindingSource1;

            // 當欄位值為0時，顯示空白
            this.clogctn.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SewLine", header: "Sewing Line", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CustCDID", header: "Cust CD", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Customize2", header: "Field2", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("DoxType", header: "Duty Deduction Dox.", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", iseditingreadonly: true)
                .Text("Alias", header: "Dest.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("SewOffLine", header: "Offline", iseditingreadonly: true)
                .Date("InspDate", header: "F-Insp", iseditingreadonly: true)
                .Date("SDPDate", header: "Cut-off Date", iseditingreadonly: true)
                .Date("EstPulloutDate", header: "E. P/Out", iseditingreadonly: true)
                .Text("Seq", header: "Ship Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("ShipmodeID", header: "Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Del", iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Del", iseditingreadonly: true)
                .Date("CRDDate", header: "CRD", iseditingreadonly: true)
                .Text("BuyMonth", header: "Month Buy", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ScanAndPack", header: "S&P", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Text("RainwearTestPassed", header: "Rainwear Test Passed", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("CTNQty", header: "Ctn Qty", iseditingreadonly: true)
                .EditText("Dimension", header: "Carton Dimension", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ProdRemark", header: "Production Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ShipRemark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("MtlFormA", header: "Mtl. FormA", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("InClogCTN", header: "% in CLOG", iseditingreadonly: true, settings: this.clogctn)
                .Numeric("CBM", header: "Ttl CBM", decimal_places: 3, iseditingreadonly: true)
                .Text("ClogLocationId", header: "Bin Location", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateExpPoutDate.Value))
            {
                this.dateExpPoutDate.Focus();
                MyUtility.Msg.WarningBox("Exp P/out Date can't be empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.comboDropDownListCategory.SelectedValue))
            {
                this.comboDropDownListCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't be empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            string category = this.comboDropDownListCategory.SelectedValue.ToString();

            #region 組SQL
            sqlCmd.Append(string.Format(
                @"
select distinct oq.Id, oq.Seq , pd.ClogLocationId
into #tmpClocationids
from Orders o WITH (NOLOCK) 
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID
left join country c WITH (NOLOCK) on c.ID = o.Dest
left join PackingList_Detail pd WITH (NOLOCK) on pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
where 1=1            
and o.MDivisionID = '{0}'
and o.PulloutComplete = 0
and o.Finished = 0
and o.Qty > 0
and (oq.EstPulloutDate <= '{1}' or dateadd(day,4,o.SewOffLine) <= '{1}')
and o.Category in ({2})
",
                Env.User.Keyword,
                Convert.ToDateTime(this.dateExpPoutDate.Value).ToString("d"),
                category));
            sqlCmd.Append(@"
select distinct id,seq into #tmpIDSeq from  #tmpClocationids

select *
into #tmp1
from #tmpIDSeq t
outer apply(
	select ClogLocationId = stuff(
	(
		select concat('; ',a.ClogLocationId)
		from(
			select distinct pd2.ClogLocationId
			from #tmpClocationids pd2 WITH (NOLOCK) 
			where pd2.id = t.ID and pd2.seq = t.seq
		)a
		for xml path('')
	),1,2,'')
)ClogLocationId

select distinct
    o.FactoryID,o.BrandID,o.SewLine,o.Id,o.StyleID,o.CustPONo,o.CustCDID,o.Customize2,o.DoxType,oq.Qty,c.Alias,o.SewOffLine
	,InspDate = isnull(o.InspDate,iif(oq.EstPulloutDate is null,dateadd(day,2,o.SewOffLine),dateadd(day,-2,oq.EstPulloutDate)))
	,oq.SDPDate
    ,EstPulloutDate = iif(oq.EstPulloutDate is null , o.BuyerDelivery , oq.EstPulloutDate)
    ,oq.Seq,oq.ShipmodeID,oq.BuyerDelivery,o.SciDelivery
	,CRDDate = iif(oq.BuyerDelivery > o.CRDDate, o.CRDDate, null)
	,o.BuyMonth,o.Customize1
	,ScanAndPack = iif(o.ScanAndPack = 1,'Y','')
	,RainwearTestPassed = iif(o.RainwearTestPassed = 1,'Y','')
	,Dimension = Dimension.Dimension
	,oq.ProdRemark,oq.ShipRemark
	,MtlFormA = MtlFormA.MtlFormA
	,C3.CTNQty
    ,InClogCTN = iif(isnull(C3.CTNQty,0) = 0,0,Round((cast(isnull(C3.ClogQty,0) as float)/cast(isnull(C3.CTNQty,0) as float)) * 100,0)) 
    ,C3.CBM
into #tmp2
from #tmpIDSeq a 
inner join Orders o WITH (NOLOCK) on o.id = a.Id
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID
left join country c WITH (NOLOCK) on c.ID = o.Dest
outer apply(
	Select top 1 
		CBM = first_value(p.CBM) over (order by (select 1))
		,CTNQty = isnull(sum(pd2.CTNQty) over (partition by (select 1)),0)
		,ClogQty = isnull(sum(iif(pd2.ReceiveDate is not null ,pd2.CTNQty,0)) over (partition by (select 1)),0)
	from PackingList_Detail pd2 WITH (NOLOCK) 
	inner join PackingList p WITH (NOLOCK) on pd2.id = p.id
	where pd2.OrderID = oq.ID and pd2.OrderShipmodeSeq = oq.Seq
)C3
outer apply(
	select Dimension = stuff((
		select CONCAT( '; ',d.d)
		from(
			select distinct d = concat(L.CtnLength,'*',L.CtnWidth,'*',L.CtnHeight)
			from PackingList_Detail pd2 WITH (NOLOCK) 
			inner join LocalItem L WITH (NOLOCK) on L.RefNo = pd2.RefNo
			where pd2.OrderID = oq.id and pd2.OrderShipmodeSeq = oq.seq and pd2.RefNo is not null and pd2.RefNo <> ''
		)d
		for xml path('')
	),1,2,'')
)Dimension
outer apply(
	select MtlFormA = stuff((
		select concat('; ',s.Seq1,'-',iif( maxR is null ,  '  /  /    ' , format(iif(s.count_All = s.count_NoNull, s.minR, s.maxR),  'yyyy/MM/dd')))
		from (
			select ed.Seq1,
				max(ed.FormXReceived) as maxR,	
				min(ed.FormXReceived) as minR,
				count(*) as count_All,
				count(ed.FormXReceived) as count_NoNull
			from Export_Detail ed WITH (NOLOCK) 
			where ed.PoID = oq.Id
			GROUP BY Seq1
		) as s
		order by Seq1
		for xml path('')
	),1,2,'')
)MtlFormA

select FactoryID,BrandID,SewLine,a.Id,StyleID,CustPONo,CustCDID,Customize2,DoxType,Qty,Alias,SewOffLine,InspDate
	,SDPDate,EstPulloutDate,a.Seq,ShipmodeID,BuyerDelivery,SciDelivery,CRDDate,BuyMonth,Customize1,ScanAndPack
	,RainwearTestPassed,Dimension,ProdRemark,ShipRemark, MtlFormA,CTNQty,InClogCTN,CBM,ClogLocationId
from #tmp1 a inner join #tmp2 b on a.Id = b.ID and a.Seq = b.Seq
order by FactoryID,BrandID,SewLine,a.Id,StyleID,CustPONo

drop table #tmpClocationids,#tmpIDSeq,#tmp1,#tmp2
");
            #endregion

            if (this.result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData))
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + this.result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            if ((DataTable)this.listControlBindingSource1.DataSource == null || ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            P12_Print callNextForm = new P12_Print((DataTable)this.listControlBindingSource1.DataSource);
            callNextForm.ShowDialog(this);
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Find Now
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                return;
            }

            int index = this.listControlBindingSource1.Find("ID", this.txtLocateForSP.Text.ToString());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.listControlBindingSource1.Position = index;
            }
        }
    }
}
