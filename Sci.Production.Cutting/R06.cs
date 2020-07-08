using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private string dateRangeReady1;
        private string dateRangeReady2;
        private string M;
        private string F;
        private string Brand;
        private string OfflineGap;
        private string DeliveryGap;
        private string CutGapDay;
        private string CutGapTime;
        private bool boolexHoliday;
        private DataTable[] dt;

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateReady))
            {
                MyUtility.Msg.WarningBox("ReadyDay must enter!");
                return false;
            }

            this.dateRangeReady1 = MyUtility.Check.Empty(this.dateReady.Value1) ? string.Empty : ((DateTime)this.dateReady.Value1).ToString("yyyy/MM/dd");
            this.dateRangeReady2 = MyUtility.Check.Empty(this.dateReady.Value2) ? string.Empty : ((DateTime)this.dateReady.Value2).ToString("yyyy/MM/dd");
            this.M = this.txtMdivision.Text;
            this.F = this.txtfactory.Text;
            this.Brand = this.txtbrand.Text;
            this.OfflineGap = this.numDateOfflineGap.Text;
            this.DeliveryGap = this.numDateDeliveryGap.Text;
            this.CutGapDay = this.numDateCutGapDay.Text;
            this.CutGapTime = this.txtDateCutGapTime.Text;
            this.boolexHoliday = this.chkHoliday.Checked;

            return true;
        }

        // 非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = string.Empty;
            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.M))
            {
                sqlwhere += $" and o.MDivisionID = '{this.M}' ";
            }

            if (!MyUtility.Check.Empty(this.F))
            {
                sqlwhere += $" and o.FtyGroup = '{this.F}' ";
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                sqlwhere += $" and o.BrandID = '{this.Brand}' ";
            }

            sqlwhere += this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";

            sqlcmd = $@"
declare @ReadyOffline1 date = '{this.dateRangeReady1}'
declare @ReadyOffline2 date = '{this.dateRangeReady2}'
declare @DateO1 date = DATEADD(DAY, -15, @ReadyOffline1)
declare @DateO2 date = DATEADD(DAY, +15 ,@ReadyOffline2)
declare @offlinegap int = {this.OfflineGap}--加幾個工作日
declare @Buyergap int = {this.DeliveryGap}--加幾個工作日
declare @CutGap int = {this.CutGapDay}--加幾個工作日
declare @CutGapTime nvarchar(5) ='{this.CutGapTime}' --加幾個工作日

declare @ReadyBuyer1 date = @ReadyOffline1
declare @ReadyBuyer2 date = @ReadyOffline2
declare @DateB1 date = @DateO1
declare @DateB2 date = @DateO2

create table #dateranges (Readydate DATE)
while @DateO1 <= @DateO2
begin
	insert into #dateranges values(@DateO1)	set @DateO1 =  DATEADD(DAY, 1,@DateO1)
end
set @DateO1 = DATEADD(DAY, -5, @ReadyOffline1)
select d.Readydate,f.ID into #df from #dateranges d,Factory f where f.Junk=0
";
            if (this.boolexHoliday)
            {
                sqlcmd += $@"
--撈工廠設定的Holiday,和星期日,欄位holiday = 1
select d.Readydate,d.ID,h.FactoryID,holiday = iif(h.FactoryID is not null,1,iif(DATEPART(WEEKDAY, Readydate) =1,1,0))
INTO #DHoliday from #df d left join Holiday h on d.Readydate = h.HolidayDate and d.ID = h.FactoryID
";
            }
            else
            {
                sqlcmd += $@"
--星期日,欄位holiday = 1
select d.Readydate,d.ID,holiday = iif(DATEPART(WEEKDAY, Readydate) =1,1,0)
INTO #DHoliday from #df d 
";
            }

            sqlcmd += $@"
select h1.Readydate,h1.ID,h1.holiday,SewOffLine=CAST(NULL AS date),BuyerDelivery=CAST(NULL AS date),CutGapDay=CAST(NULL AS date)
into #tmp1
from #DHoliday h1

Declare @Readydate date,@factory nvarchar(8),@holiday int,@sewoffline date,@Buyerday date,@CutGapDay date
DECLARE c1 CURSOR FOR select * from #tmp1 order by ID,Readydate
OPEN c1
FETCH NEXT FROM c1 INTO @Readydate,@factory,@holiday,@sewoffline,@Buyerday,@CutGapDay
While @@FETCH_STATUS = 0
Begin
	--sewoffline
	declare @count1 int = 0
	declare @count2 int = 1
	declare @countTTL int = 0
	while @count1 < @offlinegap
	Begin
		if exists(select 1 from #tmp1 where ID = @factory and holiday = 1 and Readydate = DATEADD(DAY,@count1 + @count2,@Readydate))
		begin
			set @countTTL = @countTTL+1
			set @count2 = @count2 + 1
		end
		else
		begin
			set @countTTL = @countTTL+1
			set @count1 = @count1 + 1
		end
	End
	update #tmp1 set SewOffLine = DATEADD(DAY,@countTTL,Readydate) where ID = @factory and Readydate = @Readydate
	
	--BuyerDelivery
	set @count1  = 0
	set @count2  = 1
	set @countTTL  = 0
	while @count1 < @Buyergap
	Begin
		if exists(select 1 from #tmp1 where ID = @factory and holiday = 1 and Readydate = DATEADD(DAY,@count1 + @count2,@Readydate))
		begin
			set @countTTL = @countTTL+1
			set @count2 = @count2 + 1
		end
		else
		begin
			set @countTTL = @countTTL+1
			set @count1 = @count1 + 1
		end
	End
	update #tmp1 set BuyerDelivery = DATEADD(DAY,@countTTL,Readydate) where ID = @factory and Readydate = @Readydate
	
	--Buyer
	set @count1  = 0
	set @count2  = 1
	set @countTTL  = 0
	while @count1 < @CutGap
	Begin
		if exists(select 1 from #tmp1 where ID = @factory and holiday = 1 and Readydate = DATEADD(DAY,@count1 + @count2,@Readydate))
		begin
			set @countTTL = @countTTL+1
			set @count2 = @count2 + 1
		end
		else
		begin
			set @countTTL = @countTTL+1
			set @count1 = @count1 + 1
		end
	End
	update #tmp1 set CutGapDay = DATEADD(DAY,@countTTL,Readydate) where ID = @factory and Readydate = @Readydate

FETCH NEXT FROM c1 INTO @Readydate,@factory,@holiday,@sewoffline,@Buyerday,@CutGapDay
End
CLOSE c1
DEALLOCATE c1

select Readydate,Factory=ID,SewOffLine,BuyerDelivery,CutGapDay into #tmp2 from #tmp1 where holiday =0 and Readydate between @ReadyOffline1 and @ReadyOffline2
------------------------------------------------------------------------------------------------------------------------------------
--從orders撈資料
select o.FtyGroup,o.BuyerDelivery,o.SciDelivery,o.id,o.Junk,o.Category,c.Alias,o.StyleID,o.CustPONo,o.BrandID,o.ProgramID,o.Qty
	,o.SewInLine,o.SewOffLine,o.SewLine,o.ShipModeList,a.Article,t.Readydate,o.MDivisionid,t.CutGapDay
    , [CutGapDatetime] = (cast(t.CutGapDay as datetime)+cast(@CutGapTime as datetime))
into #orderOffline
from Orders o with(nolock)
inner join #tmp2 t on t.SewOffLine = o.SewOffLine and t.Factory = o.FtyGroup--推算的SewOffLine找orders, 以符合SewOffLine
left join Country c with(nolock) on c.id = o.Dest
outer apply(select Article = stuff((select concat(',',Article) from Order_Article oa where oa.id = o.id for xml path('')),1,1,''))a
where o.Category = 'B'
and o.BuyerDelivery >= t.BuyerDelivery--BuyerDelivery要大於等於推算出來的BuyerDelivery
{sqlwhere}
--and o.MDivisionID = '' and o.FtyGroup = '' and o. BrandID = ''

select o.FtyGroup,o.BuyerDelivery,o.SciDelivery,o.id,o.Junk,o.Category,c.Alias,o.StyleID,o.CustPONo,o.BrandID,o.ProgramID,o.Qty
	,o.SewInLine,o.SewOffLine,o.SewLine,o.ShipModeList,a.Article,t.Readydate,o.MDivisionid,t.CutGapDay
    , [CutGapDatetime] = (cast(t.CutGapDay as datetime)+cast(@CutGapTime as datetime))
into #orderBuyer
from Orders o with(nolock)
inner join #tmp2 t on t.BuyerDelivery = o.BuyerDelivery and t.Factory = o.FtyGroup--推算的BuyerDelivery
left join Country c with(nolock) on c.id = o.Dest
outer apply(select Article = stuff((select concat(',',Article) from Order_Article oa where oa.id = o.id for xml path('')),1,1,''))a
where o.Category = 'B' 
and o.id not in(select distinct id from #orderOffline)--要排除上面的訂單號碼, 才不會重複
{sqlwhere}
--and o.MDivisionID = '' and o.FtyGroup = '' and o. BrandID = ''

-----------完成成衣件數-------------------------------------------------------------------------------------------------------------------------
--先找所有部位
Select distinct sp = o.ID,wd.SizeCode,wd.article,occ.PatternPanel,o.MDivisionID,a.Readydate
into #pOffline 
from (select distinct id,Readydate from #orderOffline)a
inner join Orders o WITH (NOLOCK) on o.id = a.ID
inner join WorkOrder_Distribute wd WITH (NOLOCK) on o.id = wd.OrderID
inner join Order_ColorCombo occ on o.poid = occ.id and occ.Article = wd.Article
inner join order_Eachcons cons on occ.id = cons.id and cons.FabricCombo = occ.PatternPanel and cons.CuttingPiece='0'
where occ.FabricCode !='' and occ.FabricCode is not null 

--以及這趟撈的資料sum(Qty) by SizeCode,Article,PatternPanel,sp
select wd.Qty
	,wd.SizeCode
	,wd.Article
	,wp.PatternPanel
	,co.MDivisionid
	,[sp] = a.id
	,a.Readydate 
	, [canDO] =case when co.EditDate <= a.CutGapDatetime then 1 else 0 end 
into #tmp_orderOffline_canDO
from #orderOffline a
inner join WorkOrder_Distribute wd WITH (NOLOCK) on wd.orderid = a.ID 
inner join workorder w WITH (NOLOCK) on w.Ukey = wd.WorkOrderUkey 
inner join CuttingOutput_Detail cod WITH (NOLOCK)on cod.CutRef = w.CutRef
inner join CuttingOutput co WITH (NOLOCK) on co.id = cod.id and co.MDivisionid = w.MDivisionId and co.Status <> 'New' and  co.EditDate is not null
left join WorkOrder_PatternPanel wp WITH (NOLOCK) on wp.WorkOrderUkey = wd.WorkOrderUkey  

select Qty= sum(a.Qty)
	,a.SizeCode
	,a.Article
	,a.PatternPanel
	,a.MDivisionid
	,a.sp
	,a.Readydate
into #tmpc
from #tmp_orderOffline_canDO a
where a.canDO = 1 
group by a.SizeCode,a.Article,a.PatternPanel,a.MDivisionid,a.sp,a.Readydate

--缺的部位null為0
select a.sp,a.SizeCode,a.Article,a.PatternPanel,a.MDivisionID,Qty=isnull(b.Qty,0),a.Readydate
into #tmpP
from #pOffline a left join #tmpc b on a.sp = b.sp and a.Article = b.Article and a.SizeCode = b.SizeCode and a.PatternPanel = b.PatternPanel and a.MDivisionID = b.MDivisionid

select cutqty = min(qty),sp,Article,SizeCode,MDivisionid into #tmpc2 from #tmpP group by sp,Article,SizeCode,MDivisionid--取最小 by size
select cutqty = sum(cutqty),sp,MDivisionid into #tmpc3 from #tmpc2 group by sp,MDivisionid--總和為此orderid完成數

-----#orderBuyer
--先找所有部位
Select distinct sp = o.ID,wd.SizeCode,wd.article,occ.PatternPanel,o.MDivisionID,a.Readydate
into #pBuyerB
from (select distinct id,Readydate from #orderBuyer)a
inner join Orders o WITH (NOLOCK) on o.id = a.ID
inner join WorkOrder_Distribute wd WITH (NOLOCK) on o.id = wd.OrderID
inner join Order_ColorCombo occ on o.poid = occ.id and occ.Article = wd.Article
inner join order_Eachcons cons on occ.id = cons.id and cons.FabricCombo = occ.PatternPanel and cons.CuttingPiece='0'
where occ.FabricCode !='' and occ.FabricCode is not null 


select wd.Qty
	,wd.SizeCode
	,wd.Article
	,wp.PatternPanel
	,co.MDivisionid
	,[sp] = a.id
	,a.Readydate 
	, [canDO] =case when co.EditDate <= a.CutGapDatetime then 1 else 0 end 
into #tmp_orderBuyer_canDO
from #orderBuyer a
inner join WorkOrder_Distribute wd WITH (NOLOCK) on wd.orderid = a.ID 
inner join workorder w WITH (NOLOCK) on w.Ukey = wd.WorkOrderUkey 
inner join CuttingOutput_Detail cod WITH (NOLOCK)on cod.CutRef = w.CutRef
inner join CuttingOutput co WITH (NOLOCK) on co.id = cod.id and co.MDivisionid = w.MDivisionId and co.Status <> 'New' and  co.EditDate is not null
left join WorkOrder_PatternPanel wp WITH (NOLOCK) on wp.WorkOrderUkey = wd.WorkOrderUkey

select Qty= sum(a.Qty)
	,a.SizeCode
	,a.Article
	,a.PatternPanel
	,a.MDivisionid
	,a.sp
	,a.Readydate
into #tmpcB
from #tmp_orderBuyer_canDO a
where a.canDO = 1 
group by a.SizeCode,a.Article,a.PatternPanel,a.MDivisionid,a.sp,a.Readydate

--缺的部位null為0
select a.sp,a.SizeCode,a.Article,a.PatternPanel,a.MDivisionID,Qty=isnull(b.Qty,0),a.Readydate
into #tmpPB
from #pBuyerB a left join #tmpcB b on a.sp = b.sp and a.Article = b.Article and a.SizeCode = b.SizeCode and a.PatternPanel = b.PatternPanel and a.MDivisionID = b.MDivisionid

select cutqty = min(qty),sp,Article,SizeCode,MDivisionid into #tmpcB2 from #tmpPB group by sp,Article,SizeCode,MDivisionid--取最小 by size
select cutqty = sum(cutqty),sp,MDivisionid into #tmpcB3 from #tmpcB2 group by sp,MDivisionid--總和為此orderid完成數
------------------------------------------------------------------------------------------------------------------------------------

select a.FtyGroup,a.BuyerDelivery,a.SciDelivery,a.ID,[Cancel] = IIF(a.Junk=1,'Y','N'),a.Category,a.Alias,a.StyleID,a.CustPONo,a.BrandID,a.ProgramID,a.Qty
	,b.cutqty
	,CuttingStatus=iif(a.Qty<=b.cutqty,'Y','')
	,a.SewInLine,a.SewOffLine,a.SewLine,a.ShipModeList,a.Article
	,CuttingStatus2=iif(a.Qty<=b.cutqty,'close cutting','not yet close')	
	,a.Readydate,a.MDivisionID
into #tmplast
from #orderOffline a
left join #tmpc3 b on a.ID = b.sp and b.MDivisionid = a.MDivisionid

union all
select a.FtyGroup,a.BuyerDelivery,a.SciDelivery,a.ID,[Cancel] = IIF(a.Junk=1,'Y','N'),a.Category,a.Alias,a.StyleID,a.CustPONo,a.BrandID,a.ProgramID,a.Qty
	,b.cutqty
	,CuttingStatus=iif(a.Qty<=b.cutqty,'Y','')
	,a.SewInLine,a.SewOffLine,a.SewLine,a.ShipModeList,a.Article
	,CuttingStatus2=iif(a.Qty<=b.cutqty,'close cutting','not yet close')		
	,a.Readydate,a.MDivisionID
from #orderBuyer a
left join #tmpcB3 b on a.ID = b.sp and b.MDivisionid = a.MDivisionid

select * from #tmplast

select distinct t.MDivisionID from #tmplast t
select t.MDivisionID,t.FtyGroup,ct=count(1) from #tmplast t group by t.FtyGroup,t.MDivisionID
select t.MDivisionID,t.FtyGroup,ct=count(1),closed = sum(iif(t.CuttingStatus='Y',1,0)),Failed = sum(iif(t.CuttingStatus='Y',0,1)),p=cast(sum(iif(t.CuttingStatus='Y',1,0))as float)/count(1)
from #tmplast t group by t.FtyGroup,t.MDivisionID

drop table #dateranges,#df,#DHoliday,#tmp1,#tmp2,#orderOffline,#tmp_orderOffline_canDO,#tmp_orderBuyer_canDO
drop table #orderBuyer,#tmpc,#tmpc2,#tmpc3,#tmpcB,#tmpcB2,#tmpcB3,#pOffline,#tmpP,#pBuyerB,#tmpPB,#tmplast

";
            DBProxy.Current.DefaultTimeout = 2700;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dt);
            DBProxy.Current.DefaultTimeout = 300;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            this.dt[0].Columns.Remove("MDivisionID");

            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt[0].Rows.Count);

            if (this.dt[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string filename = "Cutting_R06 Ready date.xltx";
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + filename); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dt[0], string.Empty, filename, 1, false, null, objApp, wSheet: objApp.Sheets[1]);      // 將datatable copy to excel
            Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            for (int i = 0; i < this.dt[0].Rows.Count; i++)
            {
                if (MyUtility.Convert.GetString(this.dt[0].Rows[i]["CuttingStatus"]).EqualString("Y"))
                {
                    worksheet.get_Range($"A{i + 2}", $"T{i + 2}").Interior.Color = Color.FromArgb(0, 176, 80);
                }
                else
                {
                    worksheet.get_Range($"S{i + 2}", $"S{i + 2}").Font.Color = Color.Red;
                }
            }

            worksheet.Range[$"A2:T{this.dt[0].Rows.Count + 1}"].Borders.Weight = 2; // 設定全框線

            List<string> Ms = new List<string>();
            foreach (DataRow dr in this.dt[1].Rows)
            {
                Ms.Add(MyUtility.Convert.GetString(dr["MDivisionID"]));
            }

            string M = string.Join(" & ", Ms);
            worksheet.Name = M + " Cutting RD";

            #region sheet2
            worksheet = objApp.ActiveWorkbook.Worksheets[2];   // 取得工作表
            int row = 1;
            foreach (DataRow dr in this.dt[1].Rows)
            {
                string m = MyUtility.Convert.GetString(dr["MDivisionID"]);
                worksheet.get_Range("A" + row, "B" + row).Merge(false);
                worksheet.Cells[row, 1] = m;
                worksheet.get_Range("A" + row, "B" + (row + 1)).Font.Bold = true; // 指定粗體
                worksheet.get_Range("A" + row, "B" + (row + 1)).Font.Size = 10; // 字型大小
                worksheet.get_Range("A" + row, "B" + row).BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
                worksheet.get_Range("A" + row, "B" + row).Interior.Color = Color.FromArgb(217, 217, 217);
                row++;
                worksheet.get_Range("A" + row, "B" + row).Borders.Weight = 2;
                worksheet.get_Range("A" + row, "B" + row).BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
                worksheet.get_Range("A" + row, "B" + row).Interior.Color = Color.FromArgb(102, 255, 255);

                worksheet.Cells[row, 1] = "Factory";
                worksheet.Cells[row, 2] = "No. of PO's for Cutting Ready Date";
                int s1 = row + 1;
                DataTable p2 = this.dt[2].Select($"MDivisionID = '{m}'").CopyToDataTable();
                p2.Columns.Remove("MDivisionID");
                MyUtility.Excel.CopyToXls(p2, string.Empty, filename, row, false, null, objApp, wSheet: objApp.Sheets[2]);
                worksheet.get_Range("A" + (row + 1), "B" + (row + p2.Rows.Count)).Borders.Weight = 2;
                worksheet.get_Range("A" + (row + 1), "B" + (row + p2.Rows.Count)).BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
                row += p2.Rows.Count + 1;
                int s2 = row - 1;
                worksheet.Cells[row, 1] = "Grand Total";
                worksheet.Cells[row, 2] = $"=SUM(B{s1}:B{s2})";
                worksheet.get_Range("A" + row, "B" + row).Font.Bold = true; // 指定粗體
                worksheet.get_Range("A" + row, "B" + row).Borders.Weight = 2;
                worksheet.get_Range("A" + row, "B" + row).BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
                worksheet.get_Range("A" + row, "B" + row).Interior.Color = Color.FromArgb(255, 230, 153);

                row += 2;
            }

            #endregion

            #region sheet3
            worksheet = objApp.ActiveWorkbook.Worksheets[3];   // 取得工作表
            row = 1;
            foreach (DataRow dr in this.dt[1].Rows)
            {
                string m = MyUtility.Convert.GetString(dr["MDivisionID"]);
                DataTable p3 = this.dt[3].Select($"MDivisionID = '{m}'").CopyToDataTable();
                p3.Columns.Remove("MDivisionID");
                MyUtility.Excel.CopyToXls(p3, string.Empty, filename, row, false, null, objApp, wSheet: objApp.Sheets[3]);
                worksheet.get_Range("A" + (row + 1), "E" + (row + p3.Rows.Count + 1)).Font.Bold = true; // 指定粗體
                int s1 = row + 1;
                row += p3.Rows.Count + 1;
                int s2 = row - 1;
                worksheet.Cells[row, 1] = "Grand Total";
                worksheet.Cells[row, 2] = $"=SUM(B{s1}:B{s2})";
                worksheet.Cells[row, 3] = $"=SUM(C{s1}:C{s2})";
                worksheet.Cells[row, 4] = $"=SUM(D{s1}:D{s2})";
                worksheet.Cells[row, 5] = $"=C{row}/B{row}";
                worksheet.get_Range("A" + s1, "E" + row).Borders.Weight = 2;
                worksheet.get_Range("A" + s1, "E" + row).BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
                worksheet.get_Range("A" + row, "E" + row).Interior.Color = Color.FromArgb(255, 230, 153);

                row += 1;
            }
            #endregion

            worksheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(filename);
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(worksheet);    // 釋放sheet
            Marshal.ReleaseComObject(objApp);          // 釋放objApp
            Marshal.ReleaseComObject(workbook);
            #endregion

            strExcelName.OpenFile();
            return true;
        }
    }
}
