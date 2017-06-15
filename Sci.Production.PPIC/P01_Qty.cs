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

namespace Sci.Production.PPIC
{
    public partial class P01_Qty : Sci.Win.Subs.Base
    {
        string orderID, poID,poCombo;
        DataTable grid1Data, grid2Data, grid3Data, grid4Data;  //存Qty資料
        DataTable grid1Data_OriQty, grid2Data_OriQty, grid3Data_OriQty, grid4Data_OriQty;  //存OriQty資料


        public P01_Qty(string OrderID, string POID, string POCombo)
        {
            InitializeComponent();
            orderID = OrderID;
            poID = POID;
            poCombo = POCombo;
            Text = Text + " (" + orderID + ")";
            displaySPPOCombination.Value = poCombo;
            displayColorwayPOCombination.Value = poCombo;
            displayDeliveryPOCombination.Value = poCombo;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //撈出所有的Size
            string sqlCmd = string.Format("select * from Order_SizeCode WITH (NOLOCK) where ID = '{0}' order by Seq", poID);
            DataTable headerData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out headerData);
            StringBuilder pivot = new StringBuilder();

            //設定Grid1的顯示欄位
            this.gridQtyBDown.IsEditingReadOnly = true;
            this.gridQtyBDown.DataSource = listControlBindingSource1;
            var gen = Helper.Controls.Grid.Generator(this.gridQtyBDown);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                    pivot.Append(string.Format("[{0}],", MyUtility.Convert.GetString(dr["SizeCode"])));
                }
            }
            //設定Grid2的顯示欄位
            this.gridCombBySPNo.IsEditingReadOnly = true;
            this.gridCombBySPNo.DataSource = listControlBindingSource2;
            gen = Helper.Controls.Grid.Generator(this.gridCombBySPNo);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "ID", "SP#", Widths.AnsiChars(15));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }
            //設定Grid3的顯示欄位
            this.gridColorway.IsEditingReadOnly = true;
            this.gridColorway.DataSource = listControlBindingSource3;
            gen = Helper.Controls.Grid.Generator(this.gridColorway);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }
            //設定Grid4的顯示欄位
            this.gridDelivery.IsEditingReadOnly = true;
            this.gridDelivery.DataSource = listControlBindingSource4;
            gen = Helper.Controls.Grid.Generator(this.gridDelivery);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "date", "BuyerDelivery", "Buyer Delivery", Widths.AnsiChars(10));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }

            //凍結欄位
            gridQtyBDown.Columns[1].Frozen = true;
            gridCombBySPNo.Columns[2].Frozen = true;
            gridColorway.Columns[1].Frozen = true;
            gridDelivery.Columns[2].Frozen = true;

            #region 撈Grid1資料
            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
            from Order_Qty oq WITH (NOLOCK) 
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where oq.ID = '{0}'
            ),
            SubTotal
            as (
            select 'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(Qty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(Qty) from UnionData where Article = p.Article) as TotalQty
            from pivotData p
            order by Seq", orderID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid1Data);

            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.Article,oq.SizeCode,oq.OriQty,oa.Seq
            from Order_Qty oq WITH (NOLOCK) 
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where oq.ID = '{0}'
            ),
            SubTotal
            as (
            select 'TTL' as Article,SizeCode,SUM(OriQty) as Qty, '9999' as Seq
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(OriQty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(OriQty) from UnionData where Article = p.Article) as TotalQty
            from pivotData p
            order by Seq", orderID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid1Data_OriQty);
            #endregion  

            #region 撈Grid2資料
            sqlCmd = string.Format(@"with tmpData
            as (
            select o.ID,oq.Article,oq.SizeCode,oq.Qty,oa.Seq,DENSE_RANK() OVER (ORDER BY o.ID) as rnk
            from Orders o WITH (NOLOCK) 
            inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select '' as ID,'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq,99999 as rnk
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(Qty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(Qty) from UnionData where ID = p.ID and Article = p.Article) as TotalQty
            from pivotData p
            order by rnk,Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);

            sqlCmd = string.Format(@"with tmpData
            as (
            select o.ID,oq.Article,oq.SizeCode,oq.OriQty,oa.Seq,DENSE_RANK() OVER (ORDER BY o.ID) as rnk
            from Orders o WITH (NOLOCK) 
            inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select '' as ID,'TTL' as Article,SizeCode,SUM(OriQty) as Qty, '9999' as Seq,99999 as rnk
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(OriQty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(OriQty) from UnionData where ID = p.ID and Article = p.Article) as TotalQty
            from pivotData p
            order by rnk,Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data_OriQty);
            #endregion

            #region 撈Grid3資料
            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
            from Orders o WITH (NOLOCK) 
            inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select 'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(Qty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(Qty) from UnionData where Article = p.Article) as TotalQty
            from pivotData p
            order by Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid3Data);

            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.Article,oq.SizeCode,oq.OriQty,oa.Seq
            from Orders o WITH (NOLOCK) 
            inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select 'TTL' as Article,SizeCode,SUM(OriQty) as Qty, '9999' as Seq
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(OriQty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(OriQty) from UnionData where Article = p.Article) as TotalQty
            from pivotData p
            order by Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid3Data_OriQty);
            #endregion

            #region 撈Grid4資料
            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.BuyerDelivery,oqd.Article,oqd.SizeCode,oqd.Qty,oa.Seq,DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
            from Orders o WITH (NOLOCK) 
            inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
            inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oqd.ID and oa.Article = oqd.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select null as BuyerDelivery,'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq,99999 as rnk
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(Qty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(isnull(Qty,0)) from UnionData where rnk = p.rnk and Article = p.Article) as TotalQty
            from pivotData p
            order by rnk,Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid4Data);

            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.BuyerDelivery,oqd.Article,oqd.SizeCode,oqd.OriQty,oa.Seq,DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
            from Orders o WITH (NOLOCK) 
            inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
            inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oqd.ID and oa.Article = oqd.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select null as BuyerDelivery,'TTL' as Article,SizeCode,SUM(OriQty) as Qty, '9999' as Seq,99999 as rnk
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(OriQty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(isnull(OriQty,0)) from UnionData where rnk = p.rnk and Article = p.Article) as TotalQty
            from pivotData p
            order by rnk,Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid4Data_OriQty);
            #endregion

            listControlBindingSource1.DataSource = grid1Data;
            listControlBindingSource2.DataSource = grid2Data;
            listControlBindingSource3.DataSource = grid3Data;
            listControlBindingSource4.DataSource = grid4Data;
        }

        public void CreateGrid(IDataGridViewGenerator gen, string datatype, string propname, string header, IWidth width)
        {
            CreateGridCol(gen, datatype
                , propname: propname
                , header: header
                , width: width
            );
        }

        private void CreateGridCol(IDataGridViewGenerator gen, string datatype
            , string propname = null, string header = null, IWidth width = null, bool? iseditingreadonly = null
            , int index = -1)
        {
            switch (datatype)
            {
                case "int":
                    gen.Numeric(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
                case "date":
                    gen.Date(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
                case "string":
                default:
                    gen.Text(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
            }
        }

        private void radioOriQty_CheckedChanged(object sender, EventArgs e)
        {
            if (radioQty.Checked)
            {
                listControlBindingSource1.DataSource = grid1Data;
                listControlBindingSource2.DataSource = grid2Data;
                listControlBindingSource3.DataSource = grid3Data;
                listControlBindingSource4.DataSource = grid4Data;
            }
            else if (radioOriQty.Checked)
            {
                listControlBindingSource1.DataSource = grid1Data_OriQty;
                listControlBindingSource2.DataSource = grid2Data_OriQty;
                listControlBindingSource3.DataSource = grid3Data_OriQty;
                listControlBindingSource4.DataSource = grid4Data_OriQty;
            }
        }

        private void ToExcel_Click(object sender, EventArgs e)
        {
            string sqlcmd1 = string.Format(@"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(oq.SizeCode),N',' + QUOTENAME(oq.SizeCode))
FROM Order_Qty oq WITH (NOLOCK) 
where oq.ID = @ID

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with tmpData as (
	select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
	from Order_Qty oq WITH (NOLOCK) 
	left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
	where oq.ID = '''+@ID+'''
),SubTotal as (
select ''TTL'' as Article,SizeCode,SUM(Qty) as Qty, ''9999'' as Seq
from tmpData
group by SizeCode
),UnionData as (
	select * from tmpData
	union all
	select * from SubTotal
),pivotData	as (
	select *
	from UnionData
	pivot( sum(Qty)
	for SizeCode in ('+@cols+')
	) a
)
select (select sum(Qty) from UnionData where Article = p.Article) as TotalQty,[Colorway] = p.Article,'+@cols+'
from pivotData p
order by Seq'

EXEC sp_executesql @sql
"
                , orderID);
            DataTable ptb1;
            DBProxy.Current.Select(null, sqlcmd1, out ptb1);
            int columns1 = ptb1.Columns.Count;

            string sqlcmd2 = string.Format(@"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
	select distinct oq.SizeCode
	from Orders o WITH (NOLOCK) 
	inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
	where o.POID = @ID
)s

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with tmpData as (
	select o.ID,oq.Article,oq.SizeCode,oq.Qty,oa.Seq,DENSE_RANK() OVER (ORDER BY o.ID) as rnk
	from Orders o WITH (NOLOCK) 
	inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
	left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
	where o.POID = '''+@ID+'''
),SubTotal as (
	select '''' as ID,''TTL'' as Article,SizeCode,SUM(Qty) as Qty, ''9999'' as Seq,99999 as rnk
	from tmpData
	group by SizeCode
),UnionData as (
	select * from tmpData
	union all
	select * from SubTotal
),pivotData as (
	select *
	from UnionData
	pivot( sum(Qty)	for SizeCode in ('+@cols+')) a
)
select (select sum(Qty) from UnionData where ID = p.ID and Article = p.Article) as TotalQty,[Sp#] = ID,[Colorway] = p.Article,'+@cols+'
from pivotData p
order by rnk,Seq'

EXEC sp_executesql @sql
"
                , poID);
            DataTable ptb2;
            DBProxy.Current.Select(null, sqlcmd2, out ptb2);
            int columns2 = ptb2.Columns.Count;

            string sqlcmd3 = string.Format(@"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
	select distinct oq.SizeCode
	from Orders o WITH (NOLOCK) 
	inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
	where o.POID = @ID
)s

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with tmpData as (
	select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
	from Orders o WITH (NOLOCK) 
	inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
	left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
	where o.POID = '''+@ID+'''
),SubTotal as (
    select ''TTL'' as Article,SizeCode,SUM(Qty) as Qty, ''9999'' as Seq
    from tmpData
    group by SizeCode
),UnionData as (
	select * from tmpData
	union all
	select * from SubTotal
),pivotData as (
	select *
	from UnionData
	pivot( sum(Qty)	for SizeCode in ('+@cols+')) a
)
select (select sum(Qty) from UnionData where Article = p.Article) as TotalQty,[Colorway] = p.Article,'+@cols+'
from pivotData p
order by Seq'

EXEC sp_executesql @sql
"
                , poID);
            DataTable ptb3;
            DBProxy.Current.Select(null, sqlcmd3, out ptb3);
            int columns3 = ptb3.Columns.Count;

            string sqlcmd4 = string.Format(@"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
	select distinct oqd.SizeCode
	 from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
	where o.POID = @ID
)s

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with tmpData as (
	select oq.BuyerDelivery,oqd.Article,oqd.SizeCode,oqd.Qty,oa.Seq,DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
    from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
    left join Order_Article oa WITH (NOLOCK) on oa.ID = oqd.ID and oa.Article = oqd.Article
	where o.POID = '''+@ID+'''
),SubTotal as (
    select null as BuyerDelivery,''TTL'' as Article,SizeCode,SUM(Qty) as Qty, ''9999'' as Seq,99999 as rnk
    from tmpData
    group by SizeCode
),UnionData as (
	select * from tmpData
	union all
	select * from SubTotal
),pivotData as (
	select *
	from UnionData
	pivot( sum(Qty)	for SizeCode in ('+@cols+')) a
)
select (select sum(isnull(Qty,0)) from UnionData where rnk = p.rnk and Article = p.Article) as TotalQty
,[Buyer Delivery] =P.BuyerDelivery,[Colorway] = p.Article,'+@cols+'
from pivotData p
order by rnk,Seq'

EXEC sp_executesql @sql
"
                , poID);
            DataTable ptb4;
            DBProxy.Current.Select(null, sqlcmd4, out ptb4);
            int columns4 = ptb4.Columns.Count;


            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_P01_Qtybreakdown.xltx"); //預先開啟excel app

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            for (int i = 0; i < columns1; i++)
            {
                objSheets.Cells[2, i + 1] = ptb1.Columns[i].ColumnName;
            }
            string r1 = MyUtility.Excel.ConvertNumericToExcelColumn(columns1);
            objSheets.get_Range("A1", r1 + "1").Merge(false);
            MyUtility.Excel.CopyToXls(ptb1, "", "PPIC_P01_Qtybreakdown.xltx", 2, false, null, objApp, wSheet: objSheets);
            objSheets.Cells[1, 1] = "Qty breakdown (" + orderID + ")";
            objSheets.get_Range("A1", r1 + "2").Interior.Color = Color.LightGreen;
            objSheets.get_Range("A2", r1 + "2").AutoFilter(1, "<>");

            objSheets = objApp.ActiveWorkbook.Worksheets[2];
            for (int i = 0; i < columns2; i++)
            {
                objSheets.Cells[2, i + 1] = ptb2.Columns[i].ColumnName;
            }
            string r2 = MyUtility.Excel.ConvertNumericToExcelColumn(columns2);
            objSheets.get_Range("A1", r2 + "1").Merge(false);
            MyUtility.Excel.CopyToXls(ptb2, "", "PPIC_P01_Qtybreakdown.xltx", 2, false, null, objApp, wSheet: objSheets);
            objSheets.Cells[1, 1] = "Qty breakdown (" + poID + ")";
            objSheets.get_Range("A1", r2 + "2").Interior.Color = Color.LightGreen;
            objSheets.get_Range("A2", r2 + "2").AutoFilter(1, "<>");

            objSheets = objApp.ActiveWorkbook.Worksheets[3];
            for (int i = 0; i < columns3; i++)
            {
                objSheets.Cells[2, i + 1] = ptb3.Columns[i].ColumnName;
            }
            string r3 = MyUtility.Excel.ConvertNumericToExcelColumn(columns3);
            objSheets.get_Range("A1", r3 + "1").Merge(false);
            MyUtility.Excel.CopyToXls(ptb3, "", "PPIC_P01_Qtybreakdown.xltx", 2, false, null, objApp, wSheet: objSheets);
            objSheets.Cells[1, 1] = "Qty breakdown (" + poID + ")";
            objSheets.get_Range("A1", r3 + "2").Interior.Color = Color.LightGreen;
            objSheets.get_Range("A2", r3 + "2").AutoFilter(1, "<>");

            objSheets = objApp.ActiveWorkbook.Worksheets[4];
            for (int i = 0; i < columns4; i++)
            {
                objSheets.Cells[2, i + 1] = ptb4.Columns[i].ColumnName;
            }
            string r4 = MyUtility.Excel.ConvertNumericToExcelColumn(columns4);
            objSheets.get_Range("A1", r4 + "1").Merge(false);
            MyUtility.Excel.CopyToXls(ptb4, "", "PPIC_P01_Qtybreakdown.xltx", 2, true, null, objApp, wSheet: objSheets);
            objSheets.Cells[1, 1] = "Qty breakdown (" + poID + ")";
            objSheets.get_Range("A1", r4 + "2").Interior.Color = Color.LightGreen;
            objSheets.get_Range("A2", r4 + "2").AutoFilter(1, "<>");
            
            objSheets = objApp.ActiveWorkbook.Worksheets[1];
            objSheets.Columns.AutoFit();
        }        
    }
}
