using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R34 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private StringBuilder sqlcmd = new StringBuilder();

        /// <inheritdoc/>
        public R34(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboReportType.SelectedIndex = 0;
            this.comboMDivision1.SetDefalutIndex();
            this.comboMDivision1.Text = Env.User.Keyword;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlcmd.Clear();
            if (MyUtility.Check.Empty(this.txtSP1.Text) && MyUtility.Check.Empty(this.txtSP2.Text) && MyUtility.Check.Empty(this.dateBuyerdelivery.Value1))
            {
                MyUtility.Msg.WarningBox("SP#, Buyer delivery can't empty!");
                return false;
            }

            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSP1.Text) && !MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $"\r\n and o.ID between '{this.txtSP1.Text}' and '{this.txtSP2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text) && MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $"\r\n and o.ID = '{this.txtSP1.Text}'";
            }

            if (MyUtility.Check.Empty(this.txtSP1.Text) && !MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $"\r\n and o.ID = '{this.txtSP2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.dateBuyerdelivery.Value1))
            {
                where += $"\r\n and oq.BuyerDelivery between '{((DateTime)this.dateBuyerdelivery.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateBuyerdelivery.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtstyle1.Text))
            {
                where += $"\r\n and o.StyleID = '{this.txtstyle1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtbrand1.Text))
            {
                where += $"\r\n and o.BrandID = '{this.txtbrand1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSeason.Text))
            {
                where += $"\r\n and o.SeasonID = '{this.txtSeason.Text}'";
            }

            if (!MyUtility.Check.Empty(this.comboDropDownList1.Text))
            {
                where += $"\r\n and o.Category in ({this.comboDropDownList1.SelectedValue})";
            }

            if (!MyUtility.Check.Empty(this.comboMDivision1.Text))
            {
                where += $"\r\n and o.MDivisionID = '{this.comboMDivision1.Text}'";
            }

            if (this.comboReportType.SelectedIndex == 0)
            {
                // SP, Seq
                this.sqlcmd.Append($@"
select
	o.ID,
	oq.Seq,
	o.StyleID,
	o.SeasonID,
	o.BrandID,
	o.FactoryID,
	oq.BuyerDelivery,
    OrderQty = oq.Qty,
    PackQty = s.ShipQty,
    ScannedQty = s.ScanQty,
	PassRate = 
	    case when s2.ShipQty is null then Null -- 沒有任何一箱掃完, 則會是Null
		when isnull(s2.ShipQty, 0) = 0 then 0
	    else (isnull(s2.ShipQty, 0)	- isnull(ErrQty, 0)) / cast(isnull(s2.ShipQty, 0)as float)
	    end
from Orders o with(nolock)
inner join Order_QtyShip oq with(nolock) on oq.id = o.ID
outer apply(
	select ShipQty = sum(pd.ShipQty), ScanQty = sum(iif(pd.ScanQty > pd.ShipQty, pd.ShipQty, pd.ScanQty))
	from PackingList_Detail pd with(nolock) 
	where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq 
)s
outer apply(
	select ShipQty = sum(pd.ShipQty), ScanQty = sum(pd.ScanQty)
    from(
	    select ShipQty = sum(pd.ShipQty), ScanQty = sum(iif(pd.ScanQty > pd.ShipQty, pd.ShipQty, pd.ScanQty))
	    from PackingList_Detail pd with(nolock) 
	    where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq 
        group by pd.id, pd.CTNStartNo
    )pd
	where pd.ShipQty = pd.ScanQty -- Pass rate只取(已掃完箱號)的資料來計算
)s2
outer apply(
	--各箱首次MDFail加總
	select ErrQty = sum(le.ErrQty)
	from(
		select m.*
		from(
			--找出各箱最找時間
			select AddDate = MIN(le.AddDate),le.PackingListID,le.CTNStartNo
			from PackErrTransfer le with(nolock) 
			inner join PackingList_Detail pd with(nolock) on pd.ID = le.PackingListID and pd.CTNStartNo = le.CTNStartNo
			where le.PackingErrorID = '00006'
			and pd.OrderID = o.ID
			and pd.OrderShipmodeSeq = oq.Seq
			group by le.PackingListID,le.CTNStartNo
		)m
		where exists(
			select 1
			from(
				select pd.id, pd.CTNStartNo, ShipQty = sum(pd.ShipQty), ScanQty = sum(iif(pd.ScanQty > pd.ShipQty, pd.ShipQty, pd.ScanQty))
				from PackingList_Detail pd with(nolock) 
				where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq 
				group by pd.id, pd.CTNStartNo
			)pd
			where pd.ShipQty = pd.ScanQty -- Pass rate只取(已掃完箱號)的資料來計算
			and pd.id = m.PackingListID and pd.CTNStartNo = m.CTNStartNo
		)
	)firstA
	inner join PackErrTransfer le with(nolock) on le.PackingListID = firstA.PackingListID and le.CTNStartNo = firstA.CTNStartNo and le.AddDate = firstA.AddDate
	where le.PackingErrorID = '00006'
)f
where 1=1
--已建立 PackingList, 但不是S,F
and exists (select 1 from PackingList_Detail pd with(nolock) inner join PackingList p with(nolock) on p.ID = pd.ID  where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq and p.Type not in('S','F'))
{where}
");
            }
            else if (this.comboReportType.SelectedIndex == 1)
            {
                // Style
                this.sqlcmd.Append($@"
select
	o.StyleID,
	o.SeasonID,
	o.BrandID,
	o.FactoryID,
    OrderQty = sum(oq.Qty),
    PackQty = sum(s.ShipQty),
    ScannedQty = sum(s.ScanQty),
	PassRate = 
	    case when sum(s2.ShipQty) is null then Null -- 沒有任何一箱掃完, 則會是Null
		when sum(s2.ShipQty) = 0 then 0
	    else (isnull(sum(s2.ShipQty), 0) - isnull(sum(ErrQty), 0)) / cast(isnull(sum(s2.ShipQty), 0)as float)
	    end
from Orders o with(nolock)
inner join Order_QtyShip oq with(nolock) on oq.id = o.ID
outer apply(
	select ShipQty = sum(pd.ShipQty), ScanQty = sum(iif(pd.ScanQty > pd.ShipQty, pd.ShipQty, pd.ScanQty))
	from PackingList_Detail pd with(nolock) 
	where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq 
)s
outer apply(
	select ShipQty = sum(pd.ShipQty), ScanQty = sum(pd.ScanQty)
    from(
	    select ShipQty = sum(pd.ShipQty), ScanQty = sum(iif(pd.ScanQty > pd.ShipQty, pd.ShipQty, pd.ScanQty))
	    from PackingList_Detail pd with(nolock) 
	    where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq 
        group by pd.id, pd.CTNStartNo
    )pd
	where pd.ShipQty = pd.ScanQty -- Pass rate只取(已掃完箱號)的資料來計算
)s2
outer apply(
	--各箱首次MDFail加總
	select ErrQty = sum(le.ErrQty)
	from(
		select m.*
		from(
			--找出各箱最找時間
			select AddDate = MIN(le.AddDate),le.PackingListID,le.CTNStartNo
			from PackErrTransfer le with(nolock) 
			inner join PackingList_Detail pd with(nolock) on pd.ID = le.PackingListID and pd.CTNStartNo = le.CTNStartNo
			where le.PackingErrorID = '00006'
			and pd.OrderID = o.ID
			and pd.OrderShipmodeSeq = oq.Seq
			group by le.PackingListID,le.CTNStartNo
		)m
		where exists(
			select 1
			from(
				select pd.id, pd.CTNStartNo, ShipQty = sum(pd.ShipQty), ScanQty = sum(iif(pd.ScanQty > pd.ShipQty, pd.ShipQty, pd.ScanQty))
				from PackingList_Detail pd with(nolock) 
				where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq 
				group by pd.id, pd.CTNStartNo
			)pd
			where pd.ShipQty = pd.ScanQty -- Pass rate只取(已掃完箱號)的資料來計算
			and pd.id = m.PackingListID and pd.CTNStartNo = m.CTNStartNo
		)
	)firstA
	inner join PackErrTransfer le with(nolock) on le.PackingListID = firstA.PackingListID and le.CTNStartNo = firstA.CTNStartNo and le.AddDate = firstA.AddDate
	where le.PackingErrorID = '00006'
)f
where 1=1
--已建立 PackingList, 但不是S,F
and exists (select 1 from PackingList_Detail pd with(nolock) inner join PackingList p with(nolock) on p.ID = pd.ID  where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq and p.Type not in('S','F'))
{where}
group by o.StyleID,o.SeasonID,o.BrandID,o.FactoryID
");
            }
            else if (this.comboReportType.SelectedIndex == 2)
            {
                // Ctn# current status
                this.sqlcmd.Append($@"
select
	pd.ID,
	pd.CTNStartNo,
	pd.OrderID,
	pd.OrderShipmodeSeq,
	ShipQty = sum(pd.ShipQty),
	ScanQty = sum(iif(pd.ScanEditDate is  not null and pd.Lacking = 0,pd.ScanQty, 0)),
	PackingError = Concat(pd.PackingErrorID, '-', (select top 1 Description from PackingError where Type = 'TP' and id = pd.PackingErrorID)),
	pd.PackingErrQty,
	pd.PackErrTransferDate
into #tmpPD
from  PackingList_Detail pd
inner join orders o on o.ID = pd.OrderID
inner join Order_QtyShip oq on oq.ID = o.ID and oq.Seq = pd.OrderShipmodeSeq
where 1=1
{where}

Group by pd.ID,pd.CTNStartNo,pd.OrderID,pd.OrderShipmodeSeq,pd.PackingErrQty,pd.PackingErrorID,pd.PackErrTransferDate

select
	pd.ID,
	pd.CTNStartNo,
	pd.OrderID,
	pd.OrderShipmodeSeq,
	o.StyleID,
	o.SeasonID,
	o.BrandID,
	o.FactoryID,
	oq.BuyerDelivery,
	Articles,
	SizeCodes,
	ShipQty,
	ScanQty,
	PassRate =
	    case when isnull(ShipQty, 0) = 0 then concat(0, '%')
		when isnull(ScanQty, 0) < isnull(ShipQty, 0) then Null
	    else concat(round((isnull(ShipQty, 0) - isnull(PackingErrQty1st, 0)) / cast(isnull(ShipQty, 0)as float) * 100, 2),  '%')
	    end,
	PackingError,
	PackingErrQty,
	pd.PackErrTransferDate
from #tmpPD pd
inner join orders o on o.ID = pd.OrderID
inner join Order_QtyShip oq on oq.ID = o.ID and oq.Seq = pd.OrderShipmodeSeq
outer apply(select top 1 PackingErrQty1st = ErrQty from PackErrTransfer pe with(nolock) where pe.PackingListID = pd.id and pe.CTNStartNo = pd.CTNStartNo and pe.PackingErrorID  = '00006' order by AddDate)ErrQty
outer apply(
	select Articles = stuff((
		select distinct concat(',', pd2.Article)
		from PackingList_Detail pd2 with(nolock)
		where pd2.OrderID = pd.OrderID and pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq
		for xml path('')
	),1,1,'')
)Article
outer apply(
	select SizeCodes = stuff((
		select distinct concat(',', pd2.SizeCode)
		from PackingList_Detail pd2 with(nolock)
		where pd2.OrderID = pd.OrderID and pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq
		for xml path('')
	),1,1,'')
)SizeCode
order by pd.id,iif(ISNUMERIC(pd.CTNStartNo)=0,'ZZZZZZZZ',RIGHT(REPLICATE('0', 8) + pd.CTNStartNo, 8)), RIGHT(REPLICATE('0', 8) + pd.CTNStartNo, 8)

drop table #tmpPD
");
            }
            else
            {
                this.sqlcmd.Append($@"
select
	pd.ID,
	pd.CTNStartNo,
	pd.OrderID,
	pd.OrderShipmodeSeq,
	ShipQty = sum(pd.ShipQty)
into #tmpPD
from  PackingList_Detail pd
inner join orders o on o.ID = pd.OrderID
inner join Order_QtyShip oq on oq.ID = o.ID and oq.Seq = pd.OrderShipmodeSeq
where 1=1
{where}

Group by pd.ID,pd.CTNStartNo,pd.OrderID,pd.OrderShipmodeSeq,pd.PackingErrQty

select
	pd.ID,
	pd.CTNStartNo,
	pd.OrderID,
	pd.OrderShipmodeSeq,
	o.StyleID,
	o.SeasonID,
	o.BrandID,
	o.FactoryID,
	oq.BuyerDelivery,
	Articles,
	SizeCodes,
	ShipQty,
	ErrorType = concat(pe.PackingErrorID, '-' + per.Description),
	pe.ErrQty,
	pe.TransferDate,
	TransferredBy = dbo.getPass1(pe.AddName)
from #tmpPD pd
inner join orders o on o.ID = pd.OrderID
inner join Order_QtyShip oq on oq.ID = o.ID and oq.Seq = pd.OrderShipmodeSeq
inner join PackErrTransfer pe with(nolock) on pe.PackingListID = pd.id and pe.CTNStartNo = pd.CTNStartNo
left join PackingError per on per.ID = pe.PackingErrorID and per.Type = 'TP'

outer apply(select top 1 PackingErrQty1st = ErrQty from PackErrTransfer pe with(nolock) where pe.PackingListID = pd.id and pe.CTNStartNo = pd.CTNStartNo and pe.PackingErrorID  = '00006' order by AddDate)ErrQty
outer apply(
	select Articles = stuff((
		select distinct concat(',', pd2.Article)
		from PackingList_Detail pd2 with(nolock)
		where pd2.OrderID = pd.OrderID and pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq
		for xml path('')
	),1,1,'')
)Article
outer apply(
	select SizeCodes = stuff((
		select distinct concat(',', pd2.SizeCode)
		from PackingList_Detail pd2 with(nolock)
		where pd2.OrderID = pd.OrderID and pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq
		for xml path('')
	),1,1,'')
)SizeCode
order by pd.id,iif(ISNUMERIC(pd.CTNStartNo)=0,'ZZZZZZZZ',RIGHT(REPLICATE('0', 8) + pd.CTNStartNo, 8)), RIGHT(REPLICATE('0', 8) + pd.CTNStartNo, 8)

drop table #tmpPD
");
            }
            #region

            // // Exception Report
            //            this.sqlcmd.Append($@"
            // select
            // o.ID,
            // oq.Seq,
            // oq.Qty,
            // o.Category,
            // o.StyleID,
            // o.SeasonID,
            // o.BrandID,
            // o.FactoryID,
            // oq.BuyerDelivery,
            // e.ExceptionReason,
            // p.packID,
            // s.ShipQty,
            // s.ScanQty
            // from Orders o with(nolock)
            // inner join Order_QtyShip oq with(nolock) on oq.id = o.ID
            // outer apply(
            // select ShipQty = sum(pd.ShipQty), ScanQty = sum(iif(pd.ScanQty > pd.ShipQty, pd.ShipQty, pd.ScanQty))
            // from PackingList_Detail pd with(nolock)
            // where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq
            // )s
            // outer apply(
            // select ExceptionReason =
            // case
            // when not exists(select 1 from PackingList_Detail pd with(nolock) where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq)
            // then 'Not create packing list yet.'
            // when exists (select 1 from PackingList_Detail pd with(nolock) inner join PackingList p with(nolock) on p.ID = pd.ID where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq and p.Type in('S','F'))
            // then 'Cannot tracking Sample / FOC packing list.'
            // when s.ShipQty > s.ScanQty
            // then 'Not finish scan & pack yet.'
            // end
            // )e
            // outer apply(
            // select packID = stuff((
            // select distinct concat(',', pd.ID)
            // from PackingList_Detail pd with(nolock)
            // where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq
            // for xml path('')
            // ),1,1,'')
            // )p
            // where 1=1
            // and (
            //--未建立 PackingList
            // not exists(select 1 from PackingList_Detail pd with(nolock) where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq)
            //--已建立 PackingList, 是S,F
            // or exists (select 1 from PackingList_Detail pd with(nolock) inner join PackingList p with(nolock) on p.ID = pd.ID where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq and p.Type in('S','F'))
            // or s.ShipQty > s.ScanQty
            // )
            // {where}
            // ");
            #endregion

            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlcmd.ToString(), out this.printData);
        }

        // 產生Excel

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            if (this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (this.printData.Rows.Count > 0)
            {
                string fileName = string.Empty;
                switch (this.comboReportType.SelectedIndex)
                {
                    case 0:
                        fileName = "Quality_R34";
                        break;
                    case 1:
                        fileName = "Quality_R34_Style";
                        break;
                    case 2:
                        fileName = "Quality_R34_Ctn current status";
                        break;
                    case 3:
                        fileName = "Quality_R34_Ctn packing error list";
                        break;
                }

                Microsoft.Office.Interop.Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx");
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, $"{fileName}.xltx", 1, false, null, excelApp);

                string saveName = this.comboReportType.SelectedIndex == 0 ? "Packing R34 MD Pass Rate (SP Seq) " : "Packing R34 MD Pass Rate (Style)";
                string strExcelName = Class.MicrosoftFile.GetName(saveName);
                excelApp.ActiveWorkbook.SaveAs(strExcelName);
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);

                strExcelName.OpenFile();
            }

            #region

            // if (this.printData[1].Rows.Count > 0)
            // {
            //    string fileName = "Quality_R34_Exception";
            //    Microsoft.Office.Interop.Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx");
            //    MyUtility.Excel.CopyToXls(this.printData[1], string.Empty, $"{fileName}.xltx", 1, false, null, excelApp);

            // string saveName = "Packing R34 MD Pass Rate (Exception)";
            //    string strExcelName = Class.MicrosoftFile.GetName(saveName);
            //    excelApp.ActiveWorkbook.SaveAs(strExcelName);
            //    excelApp.Quit();
            //    Marshal.ReleaseComObject(excelApp);

            // strExcelName.OpenFile();
            // }
            #endregion
            return true;
        }
    }
}
