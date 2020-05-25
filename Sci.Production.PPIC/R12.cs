using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R12
    /// </summary>
    public partial class R12 : Sci.Win.Tems.PrintForm
    {
        private string season;
        private string brand;
        private string mDivision;
        private string factory;
        private bool bulk;
        private bool sample;
        private bool material;
        private bool forecast;
        private bool garment;
        private bool smtl;
        private DateTime? buyerDlv1;
        private DateTime? buyerDlv2;
        private DateTime? sciDlv1;
        private DateTime? sciDlv2;
        private DataTable[] printData;

        /// <summary>
        /// R12
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);

            this.comboM.Text = Sci.Env.User.Keyword;
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.checkBulk.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                MyUtility.Msg.WarningBox("All date can't empty!!");
                this.dateBuyerDelivery.TextBox1.Focus();
                return false;
            }

            this.buyerDlv1 = this.dateBuyerDelivery.Value1;
            this.buyerDlv2 = this.dateBuyerDelivery.Value2;
            this.sciDlv1 = this.dateSCIDelivery.Value1;
            this.sciDlv2 = this.dateSCIDelivery.Value2;
            this.season = this.txtseason.Text.Trim();
            this.brand = this.txtbrand.Text.Trim();
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.bulk = this.checkBulk.Checked;
            this.sample = this.checkSample.Checked;
            this.material = this.checkMaterial.Checked;
            this.forecast = this.checkForecast.Checked;
            this.garment = this.checkGarment.Checked;
            this.smtl = this.checkSMTL.Checked;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlwhere = string.Empty;
            string sqlwhere2 = string.Empty;
            List<SqlParameter> listSqlPara = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.buyerDlv1))
            {
                listSqlPara.Add(new SqlParameter("@buyerDlv1", this.buyerDlv1));
                listSqlPara.Add(new SqlParameter("@buyerDlv2", this.buyerDlv2));
                sqlwhere += " and o.BuyerDelivery between @buyerDlv1 and @buyerDlv2";
            }

            if (!MyUtility.Check.Empty(this.sciDlv1))
            {
                listSqlPara.Add(new SqlParameter("@sciDlv1", this.sciDlv1));
                listSqlPara.Add(new SqlParameter("@sciDlv2", this.sciDlv2));
                sqlwhere += " and o.SciDelivery between @sciDlv1 and @sciDlv2";
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                listSqlPara.Add(new SqlParameter("@season", this.season));
                sqlwhere += " and o.SeasonID = @season ";
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                listSqlPara.Add(new SqlParameter("@brand", this.brand));
                sqlwhere += " and o.BrandID = @brand ";
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                listSqlPara.Add(new SqlParameter("@mDivision", this.mDivision));
                sqlwhere += " and o.MDivisionID = @mDivision ";
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                listSqlPara.Add(new SqlParameter("@factory", this.factory));
                sqlwhere += " and o.FtyGroup = @factory ";
            }

            List<string> category = new List<string>();
            if (this.bulk) category.Add("'B'");
            if (this.sample) category.Add("'S'");
            if (this.material) category.Add("'M'");
            //if (this.forecast) category.Add("''");
            if (this.garment) category.Add("'G'");
            if (this.smtl) category.Add("'T'");
            string categorys = string.Join(",", category);

            if (category.Count > 0)
            {
                sqlwhere2 += $" and o.Category in ({categorys}) ";
            }

            sqlwhere += this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            string sqlCmd = $@"
select 
	o.MDivisionID,
	o.FactoryID,
	o.BuyerDelivery,
	o.SciDelivery,
	Mon = o.SciDelivery,
	o.id,
    [Cancel] = IIF(o.Junk=1,'Y','N'),
	o.Category,
	c.Alias,
	o.styleid,
    o.brandid,
	o.SeasonID,
	oq.Article,
	oq.Qty,
	oap.ColorID,
	ttlqt = isnull(oq.Qty*oap.Qty,0),
	PADPRINTINGPCS = oap.Qty,
	o.StyleUkey
into #tmpo
from orders o with(nolock)
inner join Order_TmsCost ot with(nolock) on o.id = ot.id and ot.ArtworkTypeID = 'PAD PRINTING' and ot.Price > 0 
cross apply(select oq.id,oq.Article,Qty=sum(oq.Qty) from Order_Qty oq with(nolock) where oq.id = o.id group by oq.id,oq.Article)oq
left join Order_Article_PadPrint oap with(nolock) on oap.ID = o.id and oap.Article = oq.Article
left join Country c with(nolock) on c.id = o.Dest
where 1 = 1
{sqlwhere}
{sqlwhere2}
";
            if (this.forecast)
            {
                sqlCmd += $@"
union all
select 
	o.MDivisionID,
	o.FactoryID,
	o.BuyerDelivery,
	o.SciDelivery,
	Mon = o.SciDelivery,
	o.id,
     [Cancel] = IIF(o.Junk=1,'Y','N'),
	o.Category,
	c.Alias,
	o.styleid,
    o.brandid,
	o.SeasonID,
	Article='',
	o.Qty,
	ColorID='',
	ttlqt =o.Qty,
	PADPRINTINGPCS =1,
	o.StyleUkey
from orders o with(nolock)
inner join Order_TmsCost ot with(nolock) on o.id = ot.id and ot.ArtworkTypeID = 'PAD PRINTING' and ot.Price > 0 
left join Country c with(nolock) on c.id = o.Dest
where 1 = 1
and o.Category = ''
{sqlwhere}
";
            }

            sqlCmd += $@"
select * from #tmpo
select * into #tmp from #tmpo where not(isnull(Category,'') = ''and isnull(ColorID,'') = '')

select distinct s.ID,s.SeasonID,s.BrandID,sa.Article,sap.ColorID
from #tmpo t
inner join style s with(nolock) on s.ukey = t.StyleUkey
inner join Style_TmsCost st with(nolock) on st.StyleUkey = s.Ukey and st.ArtworkTypeID = 'PAD PRINTING' and st.Price > 0 
left join Style_Article sa with(nolock) on sa.StyleUkey = s.Ukey and sa.Article = t.Article
left join Style_Article_PadPrint sap with(nolock) on sap.StyleUkey = s.ukey

select ColorID,ym = format(SciDelivery,'yyyy/MM'),qty=sum(qty) into #tmp2 from #tmp group by ColorID,format(SciDelivery,'yyyy/MM')
declare @maxSciDel datetime= (select max(SciDelivery) from #tmp)
declare @minSciDel datetime= (select min(SciDelivery) from #tmp)
create table #mondt(ym datetime)
while @minSciDel <= DATEADD(MONTH, 6,@maxSciDel)
begin
	insert into #mondt values(@minSciDel)	set @minSciDel =  DATEADD(MONTH, 1,@minSciDel)
end
select ym=format(t.ym,'yyyy/MM'),t2.ColorID,t2.qty into #tmp3 from #mondt t left join #tmp2 t2 on format(t.ym,'yyyy/MM') = t2.ym
declare @ym nvarchar(max)=stuff((select distinct concat(',[',ym,']') from #tmp3 for xml path('')),1,1,'')
declare @ex nvarchar(max)='
select * into #sheet1
from(select ColorID,ym,qty from #tmp3)x
pivot(sum(qty) for ym in('+@ym+'))as pvt
select * from #sheet1 where isnull(ColorID,'''') <>''''
select * from #sheet1 where isnull(ColorID,'''') =''''
'
exec(@ex)

select y=DATEPART(year,ym),m=SUBSTRING(convert(nvarchar,ym,106),4,3),ct =count(1) over(partition by DATEPART(year,ym)) from #mondt

select y=DATEPART(year,ym),m=DATEPART(MM,ym),b.Qty
from #mondt a
outer apply(
	select Qty = sum(o.qty)
	from orders o with(nolock)
	inner join Order_TmsCost ot with(nolock) on o.id = ot.id and ot.ArtworkTypeID = 'PAD PRINTING' and ot.Price > 0 
	where isnull(o.Junk,0) =0 and o.Category = ''
    {sqlwhere}
	and DATEPART(year,a.ym) = DATEPART(YEAR,o.SciDelivery) and DATEPART(MONTH,a.ym) = DATEPART(MONTH,o.SciDelivery)
)b

drop table #tmp,#tmp2,#tmp3,#mondt
";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, listSqlPara, out this.printData);
            if (!result)
            {
                return result;
            }

            this.printData[0].Columns.Remove("StyleUkey");
            return Result.True;
        }

        private int nowRow;

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData[0].Rows.Count);

            this.ShowWaitMessage("Excel Processing...");

            string filename = "PPIC_R12_PadPrintInkForecast.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + filename);
            Excel.Worksheet worksheet;
            excel.DisplayAlerts = false;

            // excel.Visible = true;

            MyUtility.Excel.CopyToXls(this.printData[1], string.Empty, filename, 1, false, null, excel, wSheet: excel.Sheets[2]);
            MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, filename, 1, false, null, excel, wSheet: excel.Sheets[3]);

            worksheet = excel.ActiveWorkbook.Worksheets[1];
            this.Pastecolumnname(worksheet, 5, Color.FromArgb(84, 130, 53), 2);
            this.Pastecolumnname(worksheet, 16, Color.FromArgb(84, 130, 53), 3);
            this.Pastecolumnname(worksheet, 28, Color.FromArgb(84, 130, 53), 3);
            this.Pastecolumnname(worksheet, 41, Color.FromArgb(198, 89, 17), 2);

            if (this.printData[2].Rows.Count > 0)
            {
                this.nowRow = 8;
                this.InsertRowbyColor(worksheet, this.nowRow, 1);
                this.nowRow += 2;
                int colorCount = this.printData[2].Rows.Count;
                #region Order % per Ink clr based on historical & forecast orders
                for (int i = 0; i < colorCount; i++)
                {
                    worksheet.Rows[this.nowRow, Type.Missing].Insert(Excel.XlDirection.xlDown);
                    this.nowRow += 1;
                }

                for (int i = 0; i < colorCount; i++)
                {
                    worksheet.Cells[this.nowRow - colorCount + i, 1] = this.printData[2].Rows[i]["ColorID"];
                    string col = MyUtility.Excel.ConvertNumericToExcelColumn(this.printData[2].Columns.Count + 1);
                    worksheet.Cells[this.nowRow - colorCount + i, 2] = $"=SUM(C{i + 8 + (colorCount * 3)}:{col}{i + 8 + (colorCount * 3)})/SUM(C{8 + (colorCount * 3)}:{col}{8 + (colorCount * 4) - 1})";
                    worksheet.get_Range((Excel.Range)worksheet.Cells[this.nowRow - colorCount + i, 2], (Excel.Range)worksheet.Cells[this.nowRow - colorCount + i, 2]).NumberFormat = "0%";
                }

                worksheet.Cells[this.nowRow, 2] = $"=SUM(B{this.nowRow - colorCount}:B{this.nowRow - 1})";
                #endregion
                this.nowRow += 6;
                this.InsertRowbyColor(worksheet, this.nowRow, 1);
                this.nowRow += 8;
                this.InsertRowbyColor(worksheet, this.nowRow, 1);
                this.nowRow += 8;
                this.InsertRowbyColor(worksheet, this.nowRow, 4);

                #region 第一區 Order datas
                for (int i = 0; i < colorCount; i++)
                {
                    for (int j = 1; j < this.printData[2].Columns.Count; j++)
                    {
                        worksheet.Cells[i + 8, j + 2] = MyUtility.Convert.GetString(this.printData[2].Rows[i][j]);
                        string col = MyUtility.Excel.ConvertNumericToExcelColumn(j + 2);
                        worksheet.Cells[i + 8 + colorCount, j + 2] = $"=IF({col}{i + 8}/$B$1=0,\"\",{col}{i + 8}/B1)";
                        worksheet.Cells[i + 8 + (colorCount * 2), j + 2] = $"=IFERROR({col}{i + 8 + colorCount}*$B$2,\"\")";
                        worksheet.Cells[i + 8 + (colorCount * 3), j + 2] = $"=IFERROR(ROUNDUP({col}{i + 8 + (colorCount * 2)}/$B$3,0),\"\")";
                    }
                }
                #endregion
                #region 第2區 FORECAST
                int row2 = 16 + (colorCount * 5);
                if (this.forecast)
                {
                    for (int i = 0; i < this.printData[5].Rows.Count; i++)
                    {
                        worksheet.Cells[row2 - 1, i + 3] = this.printData[5].Rows[i]["Qty"];
                    }
                }

                for (int i = 0; i < colorCount; i++)
                {
                    for (int j = 1; j < this.printData[2].Columns.Count; j++)
                    {
                        string col = MyUtility.Excel.ConvertNumericToExcelColumn(j + 2);
                        worksheet.Cells[i + row2, j + 2] = $"=IF({col}${row2 - 1}*$B${(colorCount * 4) + 10 + i}=0,\"\",{col}${row2 - 1}*$B${(colorCount * 4) + 10 + i})";
                        worksheet.Cells[i + row2 + colorCount, j + 2] = $"=IFERROR(IF({col}{i + row2}/$B$1=0,\"\",{col}{i + row2}/$B$1),\"\")";
                        worksheet.Cells[i + row2 + (colorCount * 2), j + 2] = $"=IFERROR({col}{i + row2 + colorCount}*$B$2,\"\")";
                        worksheet.Cells[i + row2 + (colorCount * 3), j + 2] = $"=IFERROR(ROUNDUP({col}{i + row2 + (colorCount * 2)}/$B$3,0),\"\")";
                    }
                }
                #endregion
                #region 第3區 SMS/Sample Order
                int row3 = 24 + (colorCount * 9);
                for (int i = 0; i < colorCount; i++)
                {
                    for (int j = 1; j < this.printData[2].Columns.Count; j++)
                    {
                        string col = MyUtility.Excel.ConvertNumericToExcelColumn(j + 2);
                        worksheet.Cells[i + row3, j + 2] = $"=IF({col}${row3 - 1}*$B${(colorCount * 4) + 10 + i}=0,\"\",{col}${row3 - 1}*$B${(colorCount * 4) + 10 + i})";
                        worksheet.Cells[i + row3 + colorCount, j + 2] = $"=IFERROR(IF({col}{i + row3}/$B$1=0,\"\",{col}{i + row3}/$B$1),\"\")";
                        worksheet.Cells[i + row3 + (colorCount * 2), j + 2] = $"=IFERROR({col}{i + row3 + colorCount}*$B$2,\"\")";
                        worksheet.Cells[i + row3 + (colorCount * 3), j + 2] = $"=IFERROR(ROUNDUP({col}{i + row3 + (colorCount * 2)}/$B$3,0),\"\")";
                    }
                }
                #endregion
                #region 第4區 TTL Cans to purchase+% Replacement
                int row4 = 32 + (colorCount * 13);
                for (int i = 0; i < colorCount; i++)
                {
                    for (int j = 1; j < this.printData[2].Columns.Count; j++)
                    {
                        string col = MyUtility.Excel.ConvertNumericToExcelColumn(j + 2);
                        worksheet.Cells[i + row4, j + 2] = $"={col}{i + 8 + (colorCount * 3)}";
                        worksheet.Cells[i + row4 + colorCount, j + 2] = $"={col}{i + row2 + (colorCount * 3)}";
                        worksheet.Cells[i + row4 + (colorCount * 2), j + 2] = $"={col}{i + row3 + (colorCount * 3)}";
                        worksheet.Cells[i + row4 + (colorCount * 3), j + 2] = $"=IF(ROUNDUP(SUM({col}{i + row4},{col}{i + row4},{col}{i + row4 + (colorCount * 2)})*B4+SUM({col}{i + row4},{col}{i + row4 + colorCount},{col}{i + row4 + (colorCount * 2)}),0)=0,\"\",ROUNDUP(SUM({col}{i + row4},{col}{i + row4 + colorCount},{col}{i + row4 + (colorCount * 2)})*B4+SUM({col}{i + row4},{col}{i + row4 + colorCount},{col}{i + row4 + (colorCount * 2)}),0))";
                    }
                }
                #endregion
                #region 顏色
                worksheet.get_Range((Excel.Range)worksheet.Cells[8, 3], (Excel.Range)worksheet.Cells[8 + (colorCount * 4) - 1, 3 + this.printData[4].Rows.Count - 1]).Interior.Color = Color.FromArgb(189, 215, 238);
                worksheet.get_Range((Excel.Range)worksheet.Cells[row2, 3], (Excel.Range)worksheet.Cells[row2 + (colorCount * 4) - 1, 3 + this.printData[4].Rows.Count - 1]).Interior.Color = Color.FromArgb(189, 215, 238);
                worksheet.get_Range((Excel.Range)worksheet.Cells[row3, 3], (Excel.Range)worksheet.Cells[row3 + (colorCount * 4) - 1, 3 + this.printData[4].Rows.Count - 1]).Interior.Color = Color.FromArgb(189, 215, 238);
                worksheet.get_Range((Excel.Range)worksheet.Cells[row4, 3], (Excel.Range)worksheet.Cells[row4 + (colorCount * 4) - 1, 3 + this.printData[4].Rows.Count - 1]).Interior.Color = Color.FromArgb(217, 217, 217);

                worksheet.get_Range((Excel.Range)worksheet.Cells[row2 - 1, 3], (Excel.Range)worksheet.Cells[row2 - 1, 3 + this.printData[4].Rows.Count - 1]).Interior.Color = Color.FromArgb(217, 217, 217);
                worksheet.get_Range((Excel.Range)worksheet.Cells[row3 - 1, 3], (Excel.Range)worksheet.Cells[row3 - 1, 3 + this.printData[4].Rows.Count - 1]).Interior.Color = Color.FromArgb(217, 217, 217);
                #endregion
                #region 隱藏列
                worksheet.Rows[$"{row2 + colorCount}:{row2 + (colorCount * 3) - 1}", System.Type.Missing].Hidden = true;
                worksheet.Rows[$"{row3 + colorCount}:{row3 + (colorCount * 3) - 1}", System.Type.Missing].Hidden = true;
                worksheet.Rows[$"{row4}:{row4 + (colorCount * 3) - 1}", System.Type.Missing].Hidden = true;
                #endregion
            }
            #region
            worksheet.Rows[8, Type.Missing].Insert(Excel.XlDirection.xlDown);
            worksheet.get_Range((Excel.Range)worksheet.Cells[8, 1], (Excel.Range)worksheet.Cells[8 + this.printData[2].Rows.Count - 1, 1]).Merge(false);
            worksheet.get_Range((Excel.Range)worksheet.Cells[8, 2], (Excel.Range)worksheet.Cells[8, 2]).Interior.Color = Color.FromArgb(217, 217, 217);
            worksheet.get_Range((Excel.Range)worksheet.Cells[8, 3], (Excel.Range)worksheet.Cells[8, 3 + this.printData[2].Columns.Count - 2]).Interior.Color = Color.FromArgb(189, 215, 238);
            worksheet.get_Range((Excel.Range)worksheet.Cells[8, 3], (Excel.Range)worksheet.Cells[8, 3 + this.printData[2].Columns.Count - 2]).Font.Bold = false;

            if (this.printData[3].Rows.Count > 0)
            {
                for (int i = 1; i < this.printData[2].Columns.Count; i++)
                {
                    worksheet.Cells[8, i + 2] = MyUtility.Convert.GetString(this.printData[3].Rows[0][i]);
                }
            }
            #endregion
            worksheet.Columns[1].AutoFit();
            worksheet.Range[worksheet.Cells[6, 3], worksheet.Cells[6, this.printData[2].Columns.Count]].AutoFilter();
            worksheet = excel.ActiveWorkbook.Worksheets[2];
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[this.printData[1].Rows.Count + 1, 5]].Borders.Weight = 2;
            worksheet.Columns.AutoFit();
            worksheet = excel.ActiveWorkbook.Worksheets[3];
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R12_PadPrintInkForecast");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            this.HideWaitMessage();
            #endregion
            return true;
        }

        private void Pastecolumnname(Excel.Worksheet worksheet, int row, Color color, int headerrow)
        {
            int yct = 0;
            for (int i = 0; i < this.printData[4].Rows.Count; i++)
            {
                worksheet.Cells[row + 1, i + 3] = this.printData[4].Rows[i]["m"];
                if (yct == 0 || yct == i)
                {
                    worksheet.Cells[row, i + 3] = this.printData[4].Rows[i]["y"];
                    worksheet.get_Range((Excel.Range)worksheet.Cells[row, 3 + i], (Excel.Range)worksheet.Cells[row, 3 + i + (int)this.printData[4].Rows[i]["ct"] - 1]).Merge(false);
                    yct += (int)this.printData[4].Rows[i]["ct"];
                }
            }

            worksheet.get_Range((Excel.Range)worksheet.Cells[row + 2, 3], (Excel.Range)worksheet.Cells[row + 2, 3 + this.printData[4].Rows.Count - 1]).Merge(false);

            // 範圍背景顏色
            worksheet.get_Range((Excel.Range)worksheet.Cells[row, 3], (Excel.Range)worksheet.Cells[row, 3 + this.printData[4].Rows.Count - 1]).Interior.Color = color;
            worksheet.get_Range((Excel.Range)worksheet.Cells[row + 1, 3], (Excel.Range)worksheet.Cells[row + 1, 3 + this.printData[4].Rows.Count - 1]).Interior.Color = Color.Black;
            #region 設定全框線
            worksheet.get_Range((Excel.Range)worksheet.Cells[row, 1], (Excel.Range)worksheet.Cells[row + headerrow + 4, 2 + this.printData[4].Rows.Count]).Borders.Weight = 2;
            #endregion
        }

        private void InsertRowbyColor(Excel.Worksheet worksheet, int row, int type)
        {
            int colorCount = this.printData[2].Rows.Count;
            #region 插入需要row數量
            for (int i = 4; i < colorCount * 4; i++)
            {
                worksheet.Rows[row + 1, Type.Missing].Insert(Excel.XlDirection.xlDown);
            }
            #endregion
            #region 合併第一欄
            for (int i = 0; i < 4; i++)
            {
                worksheet.get_Range((Excel.Range)worksheet.Cells[row + (i * colorCount), 1], (Excel.Range)worksheet.Cells[row + (i * colorCount) + colorCount - 1, 1]).Merge(false);

                // 填colorID
                for (int j = 0; j < this.printData[2].Rows.Count; j++)
                {
                    worksheet.Cells[row + (i * colorCount) + j, 2] = this.printData[2].Rows[j]["ColorID"];
                }
            }
            #endregion
            #region 第1,2欄顏色
            worksheet.get_Range((Excel.Range)worksheet.Cells[row, 1], (Excel.Range)worksheet.Cells[row - 1 + colorCount, 2]).Interior.Color = Color.FromArgb(217, 217, 217);
            if (colorCount > 1)
            {
                for (int i = 0; i < colorCount; i++)
                {
                    if (i % 2 == 1)
                    {
                        worksheet.get_Range((Excel.Range)worksheet.Cells[row + i + colorCount, 2], (Excel.Range)worksheet.Cells[row + i + colorCount, 2]).Interior.Color = Color.FromArgb(217, 217, 217);
                        worksheet.get_Range((Excel.Range)worksheet.Cells[row + i + (colorCount * 2), 2], (Excel.Range)worksheet.Cells[row + i + (colorCount * 2), 2]).Interior.Color = Color.FromArgb(217, 217, 217);
                    }
                }
            }
            #endregion
            #region 第一欄內容
            if (type == 4)
            {
                worksheet.Cells[row, 1] = "Bulk";
                worksheet.Cells[row + (1 * colorCount), 1] = "FC Order";
                worksheet.Cells[row + (2 * colorCount), 1] = "SMS/Sample Order";
                worksheet.Cells[row + (3 * colorCount), 1] = "# of Cans";
                worksheet.get_Range((Excel.Range)worksheet.Cells[row + (3 * colorCount), 1], (Excel.Range)worksheet.Cells[row - 1 + (colorCount * 4), 2]).Interior.Color = Color.FromArgb(217, 217, 217);
            }
            else
            {
                worksheet.Cells[row, 1] = "Order QTY";
                worksheet.Cells[row + (1 * colorCount), 1] = "# of mix";
                worksheet.Cells[row + (2 * colorCount), 1] = "TTL Grams";
                worksheet.Cells[row + (3 * colorCount), 1] = "# of Cans";
                worksheet.get_Range((Excel.Range)worksheet.Cells[row + (3 * colorCount), 1], (Excel.Range)worksheet.Cells[row - 1 + (colorCount * 4), 2]).Interior.Color = Color.FromArgb(189, 215, 238);
            }
            #endregion
            this.nowRow += colorCount * 4;
        }
    }
}
