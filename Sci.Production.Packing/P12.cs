using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using System.Linq;

namespace Sci.Production.Packing
{
    public partial class P12 : Sci.Win.Tems.QueryForm
    {
        DualResult result;
        DataTable gridData;
        DataGridViewGeneratorNumericColumnSettings clogctn = new DataGridViewGeneratorNumericColumnSettings();
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            //Exp P/out date預設帶出下個月的最後一天
            dateExpPoutDate.Value = (DateTime.Today.AddMonths(2)).AddDays(1 - (DateTime.Today.AddMonths(2)).Day - 1);
            comboDropDownListCategory.SelectedIndex = 0;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //Grid設定
            this.gridDetail.IsEditingReadOnly = true;
            this.gridDetail.DataSource = listControlBindingSource1;

            //當欄位值為0時，顯示空白
            clogctn.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            Helper.Controls.Grid.Generator(this.gridDetail)
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
                .Numeric("InClogCTN", header: "% in CLOG", iseditingreadonly: true, settings: clogctn)
                .Numeric("CBM", header: "Ttl CBM",decimal_places:3, iseditingreadonly: true)
                .Text("ClogLocationId", header: "Bin Location", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateExpPoutDate.Value))
            {
                dateExpPoutDate.Focus();
                MyUtility.Msg.WarningBox("Exp P/out Date can't be empty!");
                return;
            }
            if (MyUtility.Check.Empty(comboDropDownListCategory.SelectedValue))
            {
                comboDropDownListCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't be empty!");
                return;
            }
            StringBuilder sqlCmd = new StringBuilder();
            string category = comboDropDownListCategory.SelectedValue.ToString();

            # region 組SQL
            sqlCmd.Append(string.Format(@"
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
            Sci.Env.User.Keyword, Convert.ToDateTime(dateExpPoutDate.Value).ToString("d"), category));            
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
            # endregion
            # region 組SQL BackUP 20161205
//            sqlCmd.Append(string.Format(@"with OrderData
//as
//(select o.FactoryID,o.BrandID,o.SewLine,o.ID,o.StyleID,o.CustPONo,o.CustCDID,o.Customize2,o.DoxType,
//        oq.Qty,o.Dest,o.SewOffLine,oq.SDPDate,oq.EstPulloutDate,oq.Seq,oq.ShipmodeID,oq.BuyerDelivery,
//	    o.SciDelivery,o.BuyMonth,o.Customize1,iif(o.ScanAndPack = 1,'Y','') as ScanAndPack,
//	    iif(o.RainwearTestPassed = 1,'Y','') as RainwearTestPassed,oq.ProdRemark,oq.ShipRemark,
//	    isnull(o.InspDate,iif(oq.EstPulloutDate is null,dateadd(day,2,o.SewOffLine),dateadd(day,-2,oq.EstPulloutDate))) as InspDate,
//	    iif(oq.BuyerDelivery > o.CRDDate, o.CRDDate, null) as CRDDate
// from Orders o, Order_QtyShip oq
// where o.MDivisionID = '{0}'
// and o.PulloutComplete = 0
// and o.Finished = 0
// and o.Qty > 0
// and o.ID = oq.Id
// and (oq.EstPulloutDate <= '{1}' or dateadd(day,4,o.SewOffLine) <= '{1}')", Sci.Env.User.Keyword,Convert.ToDateTime(dateBox1.Value).ToString("d")));
//            if (txtdropdownlist1.SelectedValue.ToString() == "BS")
//            {
//                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S')");
//            }
//            else
//            {
//                sqlCmd.Append(string.Format(" and o.Category = '{0}'",txtdropdownlist1.SelectedValue));
//            }
//            sqlCmd.Append(@"),
//distinctOrder
//as
//(select distinct ID,Seq
// from OrderData
//),
//CTNData
//as
//(select a.ID,a.Seq,a.CTNQty,iif(a.CTNQty = 0,0,Round((cast(a.ClogQty as float)/a.CTNQty)*100,0)) as InClogCTN
// from (select do.ID,do.Seq,isnull(sum(pd1.CTNQty),0) as CTNQty,isnull(sum(pd2.CTNQty),0) as ClogQty
//       from distinctOrder do
//       left join PackingList_Detail pd1 on do.ID = pd1.OrderID and do.Seq = pd1.OrderShipmodeSeq
//       left join PackingList_Detail pd2 on pd1.OrderID = pd2.OrderID and pd1.OrderShipmodeSeq = pd2.OrderShipmodeSeq and pd1.CTNStartNo = pd2.CTNStartNo and pd1.Article = pd2.Article and pd1.SizeCode = pd2.SizeCode and pd2.ReceiveDate is not null
//       group by do.ID,do.Seq) a
// where a.CTNQty > 0
//),
//CBM
//as
//(select do.ID,do.Seq,isnull(sum(p.CBM),0) as CBM
// from distinctOrder do, PackingList p
// where do.ID = p.OrderID
// and do.Seq = p.OrderShipmodeSeq
// group by do.ID,do.Seq
//),
//RefNo
//as
//(select distinct do.ID,do.Seq,pd.RefNo,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension
// from distinctOrder do
// left join PackingList_Detail pd on pd.OrderID = do.ID and pd.OrderShipmodeSeq = do.Seq
// left join LocalItem li on li.RefNo = pd.RefNo
// where pd.RefNo is not null and pd.RefNo <> ''
//),
//CTNDim
//as
//(select distinct a.ID,a.Seq,(select cast(b.Dimension as nvarchar)+'; ' from RefNo b where a.ID = b.ID and a.Seq = b.Seq for xml path('')) as Dimension
// from RefNo a
//),
//Location
//as
//(select distinct do.ID,do.Seq,pd.ClogLocationId
// from distinctOrder do
// left join PackingList_Detail pd on pd.OrderID = do.ID and pd.OrderShipmodeSeq = do.Seq
// where pd.ClogLocationId is not null and pd.ClogLocationId <> ''
//),
//BinLocation
//as
//(select distinct a.ID,a.Seq,(select cast(b.ClogLocationId as nvarchar)+'; ' from Location b where a.ID = b.ID and a.Seq = b.Seq for xml path('')) as ClogLocationId
// from Location a
//),
//MtlFormA
//as
//(select distinct do.ID,ed.Seq1+'-'+iif(min(ed.FormXReceived) is null,iif(min(ed.FormXReceived)is null, '  /  /    ',convert(char(10),min(ed.FormXReceived),111)) ,iif(max(ed.FormXReceived) is null,'  /  /    ',convert(char(10),max(ed.FormXReceived),111))) as Seq
// from distinctOrder do, Export_Detail ed
// where do.ID = ed.PoID
// group by do.ID,ed.Seq1
//),
//LastMtlFormA
//as
//(select distinct mf1.ID, (select CAST(mf2.Seq as nvarchar)+' ; ' from MtlFormA mf2 where mf2.ID = mf1.ID for xml path('')) as MtlFormA
// from MtlFormA mf1
//)
//select od.*,isnull(cd.CTNQty,0) as CTNQty,isnull(cd.InClogCTN,0) as InClogCTN,isnull(CBM.CBM,0) as CBM,isnull(CTNDim.Dimension,'') as Dimension,isnull(bl.ClogLocationId,'') as ClogLocationId,isnull(lm.MtlFormA,'') as MtlFormA,c.Alias
//from OrderData od
//left join CTNData cd on od.ID = cd.ID and od.Seq = cd.Seq
//left join CBM on od.ID = cbm.ID and od.Seq = cbm.Seq
//left join CTNDim on od.ID = CTNDim.ID and od.Seq = CTNDim.Seq
//left join BinLocation bl on od.ID = bl.ID and od.Seq = bl.Seq
//left join LastMtlFormA lm on od.id = lm.ID
//left join Country c on c.ID = od.Dest");
            #endregion

            if (result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData))
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Sql connection fail!\r\n"+result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if ((DataTable)listControlBindingSource1.DataSource == null || ((DataTable)listControlBindingSource1.DataSource).Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }
            Sci.Production.Packing.P12_Print callNextForm = new Sci.Production.Packing.P12_Print((DataTable)listControlBindingSource1.DataSource);
            callNextForm.ShowDialog(this);
        }

        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Find Now
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("ID", txtLocateForSP.Text.ToString());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }
    }
}
