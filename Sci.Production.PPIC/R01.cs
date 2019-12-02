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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private string mDivision;
        private string factory;
        private string line1;
        private string line2;
        private string brand;
        private string type;
        private DateTime? SewingDate1;
        private DateTime? SewingDate2;
        private DateTime? buyerDelivery1;
        private DateTime? buyerDelivery2;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FTYGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboM.Text = Sci.Env.User.Keyword;

            // comboBox2.SelectedIndex = 0;
            this.comboFactory.Text = Sci.Env.User.Factory;

            this.comboSummaryBy.Add("SP#", "SP#");
            this.comboSummaryBy.Add("Article / Size", "Article / Size");
            this.comboSummaryBy.Add("Style, per each sewing date", "StylePerEachSewingDate");
            this.comboSummaryBy.SelectedIndex = 0;
        }

        // Sewing Line
        private void TxtSewingLineStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLineStart.Text = this.SelectSewingLine(this.txtSewingLineStart.Text);
        }

        // Sewing Line
        private void TxtSewingLineEnd_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLineEnd.Text = this.SelectSewingLine(this.txtSewingLineEnd.Text);
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine WITH (NOLOCK) {0}  ", MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" where FactoryID = '{0}'", this.comboFactory.Text));
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "3", line, false, ",");
            item.Width = 300;
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return string.Empty;
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.line1 = this.txtSewingLineStart.Text;
            this.line2 = this.txtSewingLineEnd.Text;
            this.SewingDate1 = this.dateSewingDate.Value1;
            this.SewingDate2 = this.dateSewingDate.Value2;
            this.buyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.buyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.brand = this.txtbrand.Text;

            this.type = this.comboSummaryBy.SelectedValue2.ToString();

            if (MyUtility.Check.Empty(SewingDate1) && MyUtility.Check.Empty(SewingDate2) &&
                MyUtility.Check.Empty(buyerDelivery1) && MyUtility.Check.Empty(buyerDelivery2) &&
                MyUtility.Check.Empty(sciDelivery1) && MyUtility.Check.Empty(sciDelivery2)
                )
            {
                MyUtility.Msg.WarningBox("Date can't be all empty!");
                return false;
            }
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result;
            /*
             *  原本的概念是 ArticleSize為細項，SP為細項加總，兩者寫成同一個SQL即可。
             *  無奈發現，細項與SP報表對不起來，只好在區分。
             */
            if (this.type == "SP#")
            {
                result = this.Query_by_SP();
            }
            else if (this.type == "StylePerEachSewingDate")
            {
                result = this.Query_by_StylePerEachSewingDate();
            }
            else
            {
                result = this.Query_by_ArticleSize();
            }

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            bool result = false;
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (this.type == "StylePerEachSewingDate")
            {
                Excel.Application objApp = null;
                Excel.Worksheet worksheet = null;
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_R01_Style_PerEachSewingDate.xltx"); // 預先開啟excel app
                result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "PPIC_R01_Style_PerEachSewingDate.xltx", headerRow: 1, showExcel: false, excelApp: objApp);

                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    return false;
                }

                worksheet = objApp.Sheets[1];

                worksheet.Columns[1].ColumnWidth = 8;
                worksheet.Columns[2].ColumnWidth = 8;
                worksheet.Columns[7].ColumnWidth = 8;
                worksheet.Columns[8].ColumnWidth = 8;
                worksheet.Columns[9].ColumnWidth = 8;
                worksheet.Columns[10].ColumnWidth = 8;
                worksheet.Columns[11].ColumnWidth = 8;
                worksheet.Columns[12].ColumnWidth = 8;
                worksheet.Columns[13].ColumnWidth = 8;
                worksheet.Columns[14].ColumnWidth = 8;
                worksheet.Columns[15].ColumnWidth = 8;
                worksheet.Columns[16].ColumnWidth = 8;
                worksheet.Columns[17].ColumnWidth = 8;
                worksheet.Columns[18].ColumnWidth = 8;
                worksheet.Columns[19].ColumnWidth = 8;
                worksheet.Columns[20].ColumnWidth = 8;
                worksheet.Columns[21].ColumnWidth = 8;
                worksheet.Columns[22].ColumnWidth = 8;
                worksheet.Columns[23].ColumnWidth = 8;
                worksheet.Columns[24].ColumnWidth = 8;
                worksheet.Columns[25].ColumnWidth = 8;
                worksheet.Columns[26].ColumnWidth = 8;
                worksheet.Columns[27].ColumnWidth = 8;
                worksheet.Columns[30].ColumnWidth = 8;
                worksheet.Columns[31].ColumnWidth = 8;
                worksheet.Columns[34].ColumnWidth = 8;
                worksheet.Columns[35].ColumnWidth = 8;
                worksheet.Columns[36].ColumnWidth = 8;
                worksheet.Columns[37].ColumnWidth = 8;
                worksheet.Columns[38].ColumnWidth = 8;
                worksheet.Columns[39].ColumnWidth = 8;
                worksheet.Columns[40].ColumnWidth = 8;

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R01_Style_PerEachSewingDate");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            else
            {
                #region SP# & Article / Size

                if (this.checkForPrintOut.Checked == true)
                {
                    #region PPIC_R01_PrintOut
                    this.printData.Columns.Remove("MDivisionID");
                    this.printData.Columns.Remove("PFRemark");
                    this.printData.Columns.Remove("BuyerDelivery");
                    this.printData.Columns.Remove("VasShas");
                    this.printData.Columns.Remove("ShipModeList");
                    this.printData.Columns.Remove("Alias");
                    this.printData.Columns.Remove("CRDDate");
                    this.printData.Columns.Remove("CustPONo");

                    Excel.Application objApp = null;
                    Excel.Worksheet worksheet = null;

                    objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_R01_PrintOut.xltx"); // 預先開啟excel app
                    result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "PPIC_R01_PrintOut.xltx", headerRow: 4, showExcel: false, excelApp: objApp);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                        return false;
                    }

                    this.ShowWaitMessage("Excel Processing...");
                    worksheet = objApp.Sheets[1];

                    // Summary By = SP# 則刪除欄位Size
                    if (this.type == "SP#")
                    {
                        worksheet.get_Range("F:F").EntireColumn.Delete();
                    }
                    #region Set Excel Title
                    string factoryName = MyUtility.GetValue.Lookup(
                        string.Format(
                            @"
select NameEn 
from Factory 
where id = '{0}'", Env.User.Factory), null);
                    worksheet.Cells[1, 1] = factoryName;
                    worksheet.Cells[2, 1] = "Sewing Line Schedule Report";
                    worksheet.Cells[3, 1] = "Date:" + DateTime.Now.ToString("yyyy/MM/dd");
                    #endregion
                    for (int i = 1; i < this.printData.Rows.Count; i++)
                    {
                        DataRow frontRow = this.printData.Rows[i - 1];
                        DataRow row = this.printData.Rows[i];

                        // 當前後 SyleID 不同時，中間加上虛線
                        if (!frontRow["StyleID"].EqualString(row["StyleID"]))
                        {
                            // [2] = header 所佔的行數 + Excel 從 1 開始編號 = 1 + 1
                            Excel.Range excelRange = worksheet.get_Range("A" + (i + 5) + ":Z" + (i + 5));
                            excelRange.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlDash;
                        }
                    }

                    worksheet.Columns[26].ColumnWidth = 30;

                    #region Save & Show Excel
                    string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R01_PrintOut");
                    Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(strExcelName);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(worksheet);
                    Marshal.ReleaseComObject(workbook);

                    strExcelName.OpenFile();
                    #endregion
                    this.HideWaitMessage();
                    #endregion
                }
                else
                {
                    #region PPIC_R01_SewingLineScheduleReport
                    Excel.Application objApp = null;
                    Excel.Worksheet worksheet = null;
                    objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_R01_SewingLineScheduleReport.xltx"); // 預先開啟excel app
                    result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "PPIC_R01_SewingLineScheduleReport.xltx", headerRow: 1, showExcel: false, excelApp: objApp);

                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                        return false;
                    }

                    worksheet = objApp.Sheets[1];

                    // Summary By = SP# 則刪除欄位Size
                    if (this.type == "SP#")
                    {
                        worksheet.get_Range("H:H").EntireColumn.Delete();
                    }

                    #region Save & Show Excel
                    string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R01_SewingLineScheduleReport");
                    Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(strExcelName);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(worksheet);
                    Marshal.ReleaseComObject(workbook);

                    strExcelName.OpenFile();
                    #endregion

                    #endregion
                }
                #endregion
            }

            return true;
        }

        private void ComboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtSewingLineStart.Text = string.Empty;
            this.txtSewingLineEnd.Text = string.Empty;
        }

        private void TxtSewingLineStart_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtSewingLineStart.Text == this.txtSewingLineStart.OldValue)
            {
                return;
            }

            if (!MyUtility.Check.Empty(this.txtSewingLineStart.Text))
            {
                string sql = string.Format("Select ID From SewingLine WITH (NOLOCK) where id='{0}' {1} ", this.txtSewingLineStart.Text, MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" and FactoryID = '{0}'", this.comboFactory.Text));
                if (!MyUtility.Check.Seek(sql))
                {
                    this.txtSewingLineStart.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", this.txtSewingLineStart.Text));
                    return;
                }
            }
        }

        private void TxtSewingLineEnd_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtSewingLineEnd.Text == this.txtSewingLineEnd.OldValue)
            {
                return;
            }

            if (!MyUtility.Check.Empty(this.txtSewingLineEnd.Text))
            {
                string sql = string.Format("Select ID From SewingLine WITH (NOLOCK) where id='{0}' {1} ", this.txtSewingLineEnd.Text, MyUtility.Check.Empty(this.comboFactory.Text) ? string.Empty : string.Format(" and FactoryID = '{0}'", this.comboFactory.Text));
                if (!MyUtility.Check.Seek(sql))
                {
                    this.txtSewingLineEnd.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", this.txtSewingLineEnd.Text));
                    return;
                }
            }
        }

        private DualResult Query_by_SP()
        {
            DualResult result;
            StringBuilder sqlCmd = new StringBuilder();
            #region Main
            sqlCmd.Append($@"
select  s.SewingLineID
            , s.MDivisionID
            , o.FactoryID
            , s.OrderID
            , o.CustPONo
            , s.ComboType
            , ( select CONCAT(Article,',') 
                from (  select distinct Article 
                        from SewingSchedule_Detail sd WITH (NOLOCK) 
                        where sd.ID = s.ID
                ) a for xml path('')) as Article
            , [SizeCode] = ''
            , o.CdCodeID
            , o.StyleID
            , o.Qty
            , s.AlloQty
            , isnull((select sum(Qty) 
                        from CuttingOutput_WIP c WITH (NOLOCK) 
                        where   c.OrderID = s.OrderID 
                                and c.Article in (  select Article 
                                                    from SewingSchedule_Detail sd WITH (NOLOCK) 
                                                    where sd.ID = s.ID)
                     ) ,0) as CutQty
            , isnull((  select sum(sod.QAQty) 
                        from    SewingOutput so WITH (NOLOCK) 
                                , SewingOutput_Detail sod WITH (NOLOCK) 
                        where   so.ID = sod.ID 
                                and so.SewingLineID = s.SewingLineID 
                                and sod.OrderId = s.OrderID 
                                and sod.ComboType = s.ComboType
                    ), 0) as SewingQty
            , isnull((  select sum(pd.ShipQty) 
                        from PackingList_Detail pd WITH (NOLOCK) 
                        where   pd.OrderID = s.OrderID 
                                and pd.ReceiveDate is not null
                     ), '') as ClogQty
            , o.InspDate
            , s.StandardOutput
            , s.MaxEff
            , o.KPILETA
            , o.MTLETA
            , o.MTLExport
            , s.Inline
            , s.Offline
            , o.SciDelivery
            , o.BuyerDelivery
			, o.CRDDate
            , o.CPU * o.CPUFactor * ( isnull(isnull(ol_rate.value,sl_rate.value), 100) / 100) as CPU
            , IIF(o.VasShas=1, 'Y', '') as VasShas
            , o.ShipModeList,isnull(c.Alias, '') as Alias 
            , isnull((  select CONCAT(Remark, ', ') 
                        from (  select s1.SewingLineID+'('+s1.ComboType+'):'+CONVERT(varchar,s1.AlloQty) as Remark 
                                from SewingSchedule s1 WITH (NOLOCK) 
                                where   s1.OrderID = s.OrderID 
                                        and s1.ID != s.ID
                        ) a for xml path('')
                    ), '') as Remark
            ,o.FtyGroup
	into #tmp_main
    from SewingSchedule s WITH (NOLOCK) 
    inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    outer apply(select value = dbo.GetOrderLocation_Rate(o.id,s.ComboType) ) ol_rate
    outer apply(select value = dbo.GetStyleLocation_Rate(o.StyleUkey,s.ComboType) ) sl_rate
    where 1=1
");
            #endregion
            #region where條件
            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.line1))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID >= '{0}'", this.line1));
            }

            if (!MyUtility.Check.Empty(this.line2))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID <= '{0}'", this.line2));
            }

            if (!MyUtility.Check.Empty(this.SewingDate1))
            {
                sqlCmd.Append(string.Format(" and (s.Inline >= '{0}' or (s.Inline <= '{0}' and s.Offline >= '{0}'))", Convert.ToDateTime(this.SewingDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.SewingDate2))
            {
                sqlCmd.Append(string.Format(" and s.Offline < '{0}'", Convert.ToDateTime(this.SewingDate2).AddDays(1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }
            #endregion

            #region TempTable
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                          TempTable                          */
-----------------------------------------------------------------

Select  w.FactoryID, w.SewingLineID, t.Inline, t.Offline
		,isnull(sum(w.Hours),0) as Hours
        , Count(w.Date) as ctn 
into #tmp_WorkHour
from WorkHour w WITH (NOLOCK) 
inner join (select distinct FtyGroup,SewingLineID,Convert(Date,Inline) Inline,Convert(Date,Offline)Offline from #tmp_main) t 
	on w.FactoryID = t.FtyGroup  and w.SewingLineID =t.SewingLineID and w.Date between Inline and Offline
where w.Hours > 0 
group by w.FactoryID, w.SewingLineID, t.Inline, t.Offline
  
select id, Remark as PFRemark
into #tmp_PFRemark
from
(
	 Select s.Id, s.Remark, s.AddDate, ROW_NUMBER() over(PARTITION BY s.Id order by s.AddDate desc) r_id
     from Order_PFHis s WITH (NOLOCK) 
	 inner join #tmp_main t on s.Id = t.OrderID
) a
where a.r_id = 1

select 
wd.OrderID,
[CutInLine] = MIN(a.EstCutDate)
into #tmp_CutInLine
from WorkOrder_Distribute wd with (nolock)
inner join  WorkOrder a with (nolock) on a.Ukey = wd.WorkOrderUkey
where exists(select 1 from #tmp_main b where wd.OrderID = b.OrderID)
group by wd.OrderID

");
            #endregion

            #region 原CTE
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           原CTE                             */
-----------------------------------------------------------------
select  ot.ID
        , at.Abbreviation
        , ot.Qty
        , ot.TMS
        , at.Classify
into #tmpAllArtwork
from Order_TmsCost ot WITH (NOLOCK) 
        , ArtworkType at WITH (NOLOCK) 
where   ot.ArtworkTypeID = at.ID
        and (ot.Price > 0 or at.Classify in ('O','I') )
        and (at.Classify in ('S','I') or at.IsSubprocess = 1)
        and (ot.TMS > 0 or ot.Qty > 0)
        and at.Abbreviation !=''
		and ot.ID in (select OrderID from #tmp_main) 

select * 
into #tmpArtWork
from (
    select  ID
            , Abbreviation+':'+Convert(varchar,Qty) as Artwork 
    from #tmpAllArtwork 
    where Qty > 0
        
    union all
    select  ID
            , Abbreviation+':'+Convert(varchar,TMS) as Artwork 
    from #tmpAllArtwork 
    where TMS > 0 and Classify in ('O','I')
) a 

select tmpArtWorkID.ID
        , Artwork = (select   CONCAT(Artwork,', ') 
						from #tmpArtWork 
						where ID = tmpArtWorkID.ID 
						order by Artwork for xml path(''))  
into #tmpOrderArtwork
from (
	select distinct ID
	from #tmpArtWork
) tmpArtWorkID

drop table #tmpAllArtwork,#tmpArtWork

");
            #endregion

            #region Final
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           Final                            */
-----------------------------------------------------------------
select  SewingLineID
        , MDivisionID
        , FactoryID
        , OrderID
		, CustPONo
        , ComboType
        , IIF(Article = '', '', SUBSTRING(Article, 1, LEN(Article) - 1)) as Article
        , SizeCode
        , CdCodeID
        , StyleID
        , Qty
        , AlloQty
        , CutQty
        , SewingQty
        , ClogQty
        , InspDate
        , StandardOutput * WorkHour as TotalStandardOutput
        , WorkHour
        , StandardOutput
        , MaxEff
        , KPILETA
        , PFRemark
        , MTLETA
        , MTLExport
        , CutInLine
        , Inline
        , Offline
        , SciDelivery
        , BuyerDelivery
		, CRDDate
        , CPU
        , VasShas
        , ShipModeList
        , Alias
        , ArtWork
        , IIF(Remark = '','',SUBSTRING(Remark,1,LEN(Remark)-1)) as Remark 
from (
	select t.* 
			,isnull(pf.PFRemark,'') PFRemark
			,IIF(w.ctn = 0, 0,w.Hours/w.ctn) WorkHour
			, isnull(SUBSTRING(ta.Artwork, 1, LEN(ta.Artwork) - 1), '') as ArtWork 
            , tc.CutInLine
		from #tmp_main t
		left join #tmp_PFRemark pf on t.OrderID = pf.Id
		left join #tmp_WorkHour w on w.FactoryID = t.FtyGroup  and w.SewingLineID =t.SewingLineID and w.Inline = Convert(Date,t.Inline) and w.Offline = Convert(Date,t.Offline) 
		left join #tmpOrderArtwork ta on ta.ID = t.OrderID 
        left join #tmp_CutInLine tc on tc.OrderID = t.OrderID
) a
order by SewingLineID,MDivisionID,FactoryID,Inline,StyleID


drop table #tmp_main,#tmp_PFRemark,#tmp_WorkHour,#tmpOrderArtwork,#tmp_CutInLine
");
            #endregion

            DBProxy.Current.DefaultTimeout = 900;
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            DBProxy.Current.DefaultTimeout = 300;
            return result;
        }

        private DualResult Query_by_ArticleSize()
        {
            DualResult result;
            StringBuilder sqlCmd = new StringBuilder();

            #region Main
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           Main                              */
-----------------------------------------------------------------
select  s.SewingLineID
            , s.MDivisionID
            , o.FactoryID
            , s.OrderID
			, o.CustPONo
            , s.ComboType 
            , sd.Article
			, sd.SizeCode
            , o.CdCodeID
            , o.StyleID 
            , o.InspDate
            , s.StandardOutput 
            , s.MaxEff
            , o.KPILETA  
            , o.MTLETA
            , o.MTLExport
            , s.Inline
            , s.Offline
            , o.SciDelivery
            , o.BuyerDelivery 
			, o.CRDDate
            , IIF(o.VasShas=1, 'Y', '') as VasShas
            , o.ShipModeList,isnull(c.Alias, '') as Alias 
            , isnull((  select CONCAT(Remark, ', ') 
                        from (  select s1.SewingLineID+'('+s1.ComboType+'):'+CONVERT(varchar,s1.AlloQty) as Remark 
                                from SewingSchedule s1 WITH (NOLOCK) 
                                where   s1.OrderID = s.OrderID 
                                        and s1.ID != s.ID
                        ) a for xml path('')
                    ), '') as Remark
			, o.StyleUkey
			,o.CPU
			,o.CPUFactor
			,s.ID
            ,o.FtyGroup
	into #tmp_main
    from SewingSchedule s WITH (NOLOCK) 
	inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID
	inner join SewingSchedule_Detail sd WITH (NOLOCK) on s.ID=sd.ID 
    left join Country c WITH (NOLOCK) on o.Dest = c.ID 
    where 1 = 1 
");
            #endregion

            #region where條件
            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.line1))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID >= '{0}'", this.line1));
            }

            if (!MyUtility.Check.Empty(this.line2))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID <= '{0}'", this.line2));
            }

            if (!MyUtility.Check.Empty(this.SewingDate1))
            {
                sqlCmd.Append(string.Format(" and (s.Inline >= '{0}' or (s.Inline <= '{0}' and s.Offline >= '{0}'))", Convert.ToDateTime(this.SewingDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.SewingDate2))
            {
                sqlCmd.Append(string.Format(" and s.Offline < '{0}'", Convert.ToDateTime(this.SewingDate2).AddDays(1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }
            #endregion

            #region TempTable
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                          TempTable                          */
-----------------------------------------------------------------

Select  w.FactoryID, w.SewingLineID, t.Inline, t.Offline
		,isnull(sum(w.Hours),0) as Hours
        , Count(w.Date) as ctn 
into #tmp_WorkHour
from WorkHour w WITH (NOLOCK) 
inner join (select distinct FtyGroup,SewingLineID,Convert(Date,Inline) Inline,Convert(Date,Offline)Offline from #tmp_main) t 
	on w.FactoryID = t.FtyGroup  and w.SewingLineID =t.SewingLineID and w.Date between Inline and Offline
where w.Hours > 0 
group by w.FactoryID, w.SewingLineID, t.Inline, t.Offline
  
select id, Remark as PFRemark
into #tmp_PFRemark
from
(
	 Select s.Id, s.Remark, s.AddDate, ROW_NUMBER() over(PARTITION BY s.Id order by s.AddDate desc) r_id
     from Order_PFHis s WITH (NOLOCK) 
	 inner join #tmp_main t on s.Id = t.OrderID
) a
where a.r_id = 1

select
wd.OrderID,
wd.Article,
wd.SizeCode,
[CutInLine] = MIN(w.EstCutDate)
into #tmp_CutInLine
from WorkOrder_Distribute wd with (nolock)
inner join WorkOrder w with (nolock) on wd.WorkOrderUkey = w.Ukey
where exists (select 1 from #tmp_main tw where  wd.OrderID = tw.OrderID and 
                                                    wd.Article = tw.Article and 
                                                    wd.SizeCode = tw.SizeCode)
group by wd.OrderID,
         wd.Article,
         wd.SizeCode

");
            #endregion

            #region 原CTE
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           原CTE                             */
-----------------------------------------------------------------
select  ot.ID
        , at.Abbreviation
        , ot.Qty
        , ot.TMS
        , at.Classify
into #tmpAllArtwork
from Order_TmsCost ot WITH (NOLOCK) 
        , ArtworkType at WITH (NOLOCK) 
where   ot.ArtworkTypeID = at.ID
        and (ot.Price > 0 or at.Classify in ('O','I') )
        and (at.Classify in ('S','I') or at.IsSubprocess = 1)
        and (ot.TMS > 0 or ot.Qty > 0)
        and at.Abbreviation !=''
		and ot.ID in (select OrderID from #tmp_main) 

select * 
into #tmpArtWork
from (
    select  ID
            , Abbreviation+':'+Convert(varchar,Qty) as Artwork 
    from #tmpAllArtwork 
    where Qty > 0
        
    union all
    select  ID
            , Abbreviation+':'+Convert(varchar,TMS) as Artwork 
    from #tmpAllArtwork 
    where TMS > 0 and Classify in ('O','I')
) a 

select tmpArtWorkID.ID
        , Artwork = (select   CONCAT(Artwork,', ') 
						from #tmpArtWork 
						where ID = tmpArtWorkID.ID 
						order by Artwork for xml path(''))  
into #tmpOrderArtwork
from (
	select distinct ID
	from #tmpArtWork
) tmpArtWorkID

drop table #tmpAllArtwork,#tmpArtWork

");
            #endregion

            #region QTY
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           QTY                               */
-----------------------------------------------------------------
select oq.ID as OrderID, oq.Article, oq.SizeCode, sum(qty) qty 
into #tmp_Qty
from Order_Qty oq
inner join (select distinct OrderID,Article,SizeCode from #tmp_main) t on oq.ID=t.OrderID and oq.Article=t.Article and oq.SizeCode=t.SizeCode 
group by oq.ID, oq.Article, oq.SizeCode

select s2.ID,s2.ComboType,s2.Article,s2.SizeCode,sum(AlloQty) AlloQty 
into #tmp_AlloQty
from SewingSchedule_Detail s2
inner join (select distinct ID,ComboType,Article,SizeCode from #tmp_main) t on s2.ID=t.ID and s2.ComboType=t.ComboType and s2.Article=t.Article and s2.SizeCode=t.SizeCode 
group by s2.ID,s2.ComboType,s2.Article,s2.SizeCode


select distinct cow.OrderID, cow.Article, cow.Size, cow.qty as CutQty
into #tmp_CutQty
from CuttingOutput_WIP cow
inner join (select distinct OrderID,Article,SizeCode from #tmp_main) t on cow.OrderID=t.OrderID and cow.Article=t.Article and cow.Size=t.SizeCode 

select sdd.OrderId, sdd.ComboType, sdd.Article, sdd.SizeCode, so.SewingLineID, sum(sdd.QAQty) SewingQty 
into #tmp_SewingQty
from SewingOutput so WITH (NOLOCK) 
inner join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on so.ID = sdd.ID
inner join (select distinct OrderID,ComboType,Article,SizeCode,SewingLineID from #tmp_main) t on sdd.OrderId=t.OrderID and sdd.ComboType=t.ComboType and sdd.Article=t.Article and sdd.SizeCode=t.SizeCode and so.SewingLineID = t.SewingLineID 
group by sdd.OrderId, sdd.ComboType, sdd.Article, sdd.SizeCode, so.SewingLineID


select pkd.OrderId,pkd.Article,pkd.SizeCode,sum(ShipQty) ClogQty 
into #tmp_ClogQty
from PackingList_Detail pkd
inner join (select distinct OrderID,Article,SizeCode from #tmp_main) t on pkd.OrderId=t.OrderID and pkd.Article=t.Article and pkd.SizeCode=t.SizeCode 
where pkd.ReceiveDate is not null
group by pkd.OrderId,pkd.Article,pkd.SizeCode

");

            #endregion

            #region Final
            sqlCmd.Append($@"
-----------------------------------------------------------------
/*                           Final                            */
-----------------------------------------------------------------


select  SewingLineID
        , MDivisionID
        , FactoryID
        , OrderID
		, CustPONo
        , ComboType, Article 
        , SizeCode
        , CdCodeID
        , StyleID
        , Qty
        , AlloQty
        , CutQty
        , SewingQty
        , ClogQty
        , InspDate
        , StandardOutput * WorkHour as TotalStandardOutput
        , WorkHour
        , StandardOutput
        , MaxEff
        , KPILETA
        , PFRemark
        , MTLETA
        , MTLExport
        , CutInLine
        , Inline
        , Offline
        , SciDelivery
        , BuyerDelivery
		, CRDDate
        , t.CPU * t.CPUFactor * ( isnull(isnull(t.ol_rate,t.sl_rate), 100) / 100) as CPU
        , VasShas
        , ShipModeList
        , Alias
        , ArtWork
        , IIF(Remark = '','',SUBSTRING(Remark,1,LEN(Remark)-1)) as Remark 
from 
(
	select t.*
		,dbo.GetOrderLocation_Rate(t.OrderID, t.ComboType) ol_rate
		,dbo.GetStyleLocation_Rate(t.StyleUkey,t.ComboType) sl_rate 
		,isnull(pf.PFRemark,'') PFRemark
		,IIF(w.ctn = 0, 0,w.Hours/w.ctn) WorkHour
		, isnull(SUBSTRING(ta.Artwork, 1, LEN(ta.Artwork) - 1), '') as ArtWork
		, [Qty] = qty.qty
		, [AlloQty] = ISNULL( alloQty.alloQty,0)
		, [CutQty] = ISNULL( cutQty.cutQty,0)
		, [SewingQty] = ISNULL( sewingQty.sewingQty,0)
		, [ClogQty] = ISNULL( clogQty.clogQty,0)
        , tc.CutInLine
	from #tmp_main t
	left join #tmp_PFRemark pf on t.OrderID = pf.Id
	left join #tmp_WorkHour w on w.FactoryID = t.FtyGroup  and w.SewingLineID =t.SewingLineID and w.Inline = Convert(Date,t.Inline) and w.Offline = Convert(Date,t.Offline) 
	left join #tmpOrderArtwork ta on ta.ID = t.OrderID
	left join #tmp_Qty qty on qty.OrderID = t.OrderID and qty.Article = t.Article and qty.SizeCode = t.SizeCode
	left join #tmp_AlloQty alloQty on alloQty.ID = t.ID and alloQty.Article = t.Article and alloQty.SizeCode = t.SizeCode and alloQty.ComboType = t.ComboType
	left join #tmp_CutQty cutQty on cutQty.OrderID = t.OrderID and cutQty.Article =t.Article and cutQty.Size = t.SizeCode
	left join #tmp_SewingQty sewingQty on sewingQty.OrderId = t.OrderID and sewingQty.Article = t.Article and sewingQty.SizeCode = t.SizeCode and sewingQty.ComboType =t.ComboType and sewingQty.SewingLineID =t.SewingLineID
	left join #tmp_ClogQty clogQty on clogQty.OrderID = t.OrderID and clogQty.Article = t.Article and clogQty.SizeCode = t.SizeCode
    left join #tmp_CutInLine tc on tc.OrderID = t.OrderID and tc.Article = t.Article and tc.SizeCode = t.SizeCode
) t
order by SewingLineID,MDivisionID,FactoryID,Inline,StyleID

drop table #tmp_main,#tmp_PFRemark,#tmp_WorkHour,#tmpOrderArtwork,#tmp_Qty,#tmp_AlloQty,#tmp_CutQty,#tmp_SewingQty,#tmp_ClogQty,#tmp_CutInLine
");
            #endregion

            DBProxy.Current.DefaultTimeout = 900;
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            DBProxy.Current.DefaultTimeout = 300;
            return result;
        }

        /// <summary>
        /// ***若邏輯修改, PowerBI StoredProcedure [P_SewingLineSchedule]需一起調整***
        /// </summary>
        /// <returns></returns>
        private DualResult Query_by_StylePerEachSewingDate()
        {
            /*
             若邏輯修改, PowerBI StoredProcedure [P_SewingLineSchedule]需一起調整
             */
            DualResult result;
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            #region where條件
            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlWhere.Append(string.Format(" and s.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlWhere.Append(string.Format(" and s.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.line1))
            {
                sqlWhere.Append(string.Format(" and s.SewingLineID >= '{0}'", this.line1));
            }

            if (!MyUtility.Check.Empty(this.line2))
            {
                sqlWhere.Append(string.Format(" and s.SewingLineID <= '{0}'", this.line2));
            }

            if (!MyUtility.Check.Empty(this.SewingDate1))
            {
                // 搜尋時判斷的是在 Inline - Offline 的期間有包含到 SewingDate1 這一段區間的 Schedule
                sqlWhere.Append(string.Format(
                    @" 
and (s.Inline >= '{0}' or
('{0}' between convert(date, s.Inline) and convert(date, s.Offline)))", Convert.ToDateTime(this.SewingDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.SewingDate2))
            {
                // 搜尋時判斷的是在 Inline - Offline 的期間有包含到 SewingDate2 這一段區間的 Schedule
                sqlWhere.Append(string.Format(
                    @" 
and (convert(date,s.Offline) <= '{0}' or
('{0}' between convert(date,s.Inline) and convert(date,s.Offline)))", Convert.ToDateTime(this.SewingDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery1))
            {
                sqlWhere.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery2))
            {
                sqlWhere.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlWhere.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlWhere.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlWhere.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }
            #endregion

            #region Main
            sqlCmd.Append($@"
--畫面抓取條件，取得APSNo
select
	s.APSNo ,
	s.MDivisionID,
	s.SewingLineID,
	s.FactoryID,
	[InlineDate] = Cast(s.Inline as date),
	[OfflineDate] = Cast(s.Offline as date),
	s.Inline,
	s.Offline,
    [InlineHour] = DATEDIFF(ss,Cast(s.Inline as date),s.Inline) / 3600.0	  ,
    [OfflineHour] = DATEDIFF(ss,Cast(s.Offline as date),s.Offline) / 3600.0	  ,
	s.OriEff,
    s.SewLineEff,
	LearnCurveID,
	Sewer,
	[AlloQty] = sum(s.AlloQty),
	[HourOutput] = iif(isnull(s.TotalSewingTime,0)=0,0,(s.Sewer * 3600.0 * ScheduleEff.val / 100) / s.TotalSewingTime),
	[OriWorkHour] = iif (s.Sewer = 0 or isnull(s.TotalSewingTime,0)=0, 0, sum(s.AlloQty) / ((s.Sewer * 3600.0 * ScheduleEff.val / 100) / s.TotalSewingTime)),
	[CPU] = cast(o.CPU * o.CPUFactor * isnull(dbo.GetOrderLocation_Rate(s.OrderID,s.ComboType),isnull(dbo.GetStyleLocation_Rate(o.StyleUkey,s.ComboType),100)) / 100 as float),
	s.TotalSewingTime,
	s.OrderID,
	s.LNCSERIALNumber,
    s.ComboType
	into #APSListWorkDay
from SewingSchedule s  WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join Factory f with (nolock) on f.id = s.FactoryID and Type <> 'S'
left join Country c WITH (NOLOCK) on o.Dest = c.ID
outer apply(select [val] = iif(isnull(s.OriEff,0) = 0 or isnull(s.SewLineEff,0) = 0,s.MaxEff, isnull(s.OriEff,100) * isnull(s.SewLineEff,100) / 100) ) ScheduleEff
where 1 = 1 {sqlWhere.ToString()} and s.APSno <> 0
group by	s.APSNo ,
			s.MDivisionID,
			s.SewingLineID,
			s.FactoryID,
			s.Inline,
			s.Offline,
			ScheduleEff.val,
            s.OriEff,
            s.SewLineEff,
			s.LearnCurveID,
			s.Sewer,
			s.TotalSewingTime,
			o.CPU,
			o.CPUFactor,
			s.OrderID,
			s.ComboType,
			o.StyleUkey,
			s.LNCSERIALNumber

select 
	APSNo,
	MDivisionID,
	SewingLineID,
	FactoryID,
	Inline,
	Offline,
	LearnCurveID,
	Sewer,
    OriEff,
    SewLineEff,
	[TotalSewingTime]=SUM(TotalSewingTime)/count(1),
	AlloQty = sum(AlloQty)
into #APSList
from #APSListWorkDay
group by APSNo,
		 MDivisionID,
		 SewingLineID,
		 FactoryID,
		 Inline,
		 Offline,
		 LearnCurveID,
		 Sewer,
         OriEff,
         SewLineEff

--取得OrderQty by APSNo
select  aps.APSNo,[OrderQty] =sum(o.Qty) 
into #APSOrderQty
from #APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join Orders o with (nolock) on s.OrderID = o.ID
group by aps.APSNo

--取得Cutting Output by APSNo
select  aps.APSNo,[CuttingOutput] =sum(cw.Qty) 
into #APSCuttingOutput
from #APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join CuttingOutput_WIP cw with (nolock) on s.OrderID = cw.OrderID
group by aps.APSNo

--取得Packing data by APSNo
select  aps.APSNo,[ScannedQty] =sum(pld.ScanQty),[ClogQty] = sum(pld.ShipQty)
into #APSPackingQty
from #APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join PackingList_Detail pld with (nolock) on s.OrderID = pld.OrderID and pld.ReceiveDate is not null
group by aps.APSNo

--取得SewingOutput
select
aps.APSNo,
so.OutputDate,
[SewingOutput] = sum(isnull(sod.QAQty,0))
into #APSSewingOutput
from #APSList aps
inner join SewingSchedule s with (nolock) on aps.APSNo = s.APSNo
inner join SewingOutput_Detail sod with (nolock) on s.OrderID = sod.OrderID and s.ComboType = sod.ComboType
inner join SewingOutput so with (nolock) on so.ID = sod.ID and s.SewingLineID = so.SewingLineID
group by	aps.APSNo,
			so.OutputDate

--取得Artwork
select  distinct
		o.StyleID
        , at.Abbreviation
        , ot.Qty
        , ot.TMS
        , at.Classify
into #tmpAllArtwork
from Order_TmsCost ot WITH (NOLOCK) 
inner join ArtworkType at WITH (NOLOCK)  on ot.ArtworkTypeID = at.ID
inner join Orders o with (nolock) on ot.ID = o.id
where   (ot.Price > 0 or at.Classify in ('O','I') )
        and (at.Classify in ('S','I') or at.IsSubprocess = 1)
        and (ot.TMS > 0 or ot.Qty > 0)
        and at.Abbreviation !=''
		and ot.ID in (select OrderID from SewingSchedule where exists(select 1 from #APSList where APSNo = SewingSchedule.APSNo)) 

select * 
into #tmpArtWork
from (
    select  StyleID
            , Abbreviation+':'+Convert(varchar,Qty) as Artwork 
    from #tmpAllArtwork 
    where Qty > 0
        
    union all
    select  StyleID
            , Abbreviation+':'+Convert(varchar,TMS) as Artwork 
    from #tmpAllArtwork 
    where TMS > 0 and Classify in ('O','I')
) a 

select tmpArtWorkID.StyleID
        , Artwork = Stuff((select   CONCAT(',',Artwork) 
						from #tmpArtWork 
						where StyleID = tmpArtWorkID.StyleID 
						order by Artwork for xml path('')),1,1,'')   
into #tmpOrderArtwork
from (
	select distinct StyleID
	from #tmpArtWork
) tmpArtWorkID

drop table #tmpAllArtwork,#tmpArtWork

--取得order對應的Articl
select
s.APSNo,
[Colorway] = Rtrim(sd.Article) +'(' + cast(SUM(sd.AlloQty) as varchar) + ')'
into #APSListArticle
from SewingSchedule s with (nolock)
inner join SewingSchedule_Detail sd with (nolock) on s.ID = sd.ID
where exists( select 1 from #APSList where APSNo = s.APSNo)
group by s.APSNo,sd.Article

--取得 Remarks欄位
select
s.APSNo,
[Remarks] = s.OrderID + '(' + s_other.SewingLineID + ',' + s.ComboType + ',' + CAST(sum(s_other.AlloQty) as varchar) + ')'
into #APSRemarks
from SewingSchedule s with (nolock)
inner join SewingSchedule s_other on s_other.OrderID = s.OrderID and s_other.ComboType = s.ComboType and s_other.APSNo <> s.APSNo
where  exists( select 1 from #APSList where APSNo = s.APSNo)
group by s.APSNo,s.OrderID,s_other.SewingLineID,s.ComboType

--取得每個計劃需要串接起來的欄位，供後續使用
select
    APSNo,
    o.CustPONo,
    [SP] = s.OrderID+'(' + s.ComboType + ')',
    o.CdCodeID,
    cd.ProductionFamilyID,
    o.StyleID,
    o.PFOrder,
    o.MTLExport,
    o.KPILETA,
    o.MTLETA,
    [Artwork] = o.StyleID+'('  +oa.Artwork + ')',
    o.InspDate,
    o.Category,
    o.SCIDelivery,
    o.BuyerDelivery,
    [BrandID] = o.BrandID
into #APSColumnGroup
from SewingSchedule s with (nolock)
inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID  
inner join CDCode cd with (nolock) on o.CdCodeID = cd.ID
left join #tmpOrderArtwork oa on oa.StyleID = o.StyleID
left join Country c WITH (NOLOCK) on o.Dest = c.ID 
where exists( select 1 from #APSList where APSNo = s.APSNo)

CREATE INDEX IDX_TMP_APSColumnGroup ON #APSColumnGroup (APSNo);
CREATE INDEX IDX_TMP_APSListArticle ON #APSListArticle (APSNo);

--填入資料串連欄位 by APSNo
select
    al.APSNo,
    al.SewingLineID,
    [CustPO] = CustPO.val,
    [CustPoCnt] =  iif(LEN(CustPO.val) > 0,(LEN(CustPO.val) - LEN(REPLACE(CustPO.val, ',', ''))) / LEN(',') + 1,0),  --用,數量計算CustPO數量
    [SP] = SP.val,
    [SpCnt] = (select count(1) from SewingSchedule where APSNo = al.APSNo),
    [Colorway] = Colorway.val,
    [ColorwayCnt] = iif(LEN(Colorway.val) > 0,(LEN(Colorway.val) - LEN(REPLACE(Colorway.val, ',', ''))) / LEN(',') + 1,0),  --用,數量計算Colorway數量
    [CDCode] = CDCode.val,
    [ProductionFamilyID] = ProductionFamilyID.val,
    [Style] = Style.val,
    [StyleCnt] = iif(LEN(Style.val) > 0,(LEN(Style.val) - LEN(REPLACE(Style.val, ',', ''))) / LEN(',') + 1,0),
    aoo.OrderQty,
    al.AlloQty,
    al.LearnCurveID,
    al.Inline,
    al.Offline,
    [PFRemark] = iif(exists(select 1 from #APSColumnGroup where APSNo = al.APSNo and PFOrder = 1),'Y',''),
    [MTLComplete] = iif(exists(select 1 from #APSColumnGroup where APSNo = al.APSNo and MTLExport = ''),'','Y'),
    OrderMax.KPILETA,
    OrderMax.MTLETA,
    [ArtworkType] = ArtworkType.val,
    OrderMax.InspDate,
    [Remarks] = Remarks.val,
    [CuttingOutput] = isnull(aco.CuttingOutput,0),
    [ScannedQty] = isnull(apo.ScannedQty,0),
    [ClogQty] = isnull(apo.ClogQty,0),
    al.MDivisionID,
    al.FactoryID,
    al.Sewer,
    [Category] = Category.val,
    OrderDateInfo.MaxSCIDelivery,
    OrderDateInfo.MinSCIDelivery,
    OrderDateInfo.MaxBuyerDelivery,
    OrderDateInfo.MinBuyerDelivery,
    al.OriEff,
    al.SewLineEff,
    al.TotalSewingTime,
    [BrandID] = BrandID.val
into #APSMain
from #APSList al
left join #APSCuttingOutput aco on al.APSNo = aco.APSNo
left join #APSOrderQty aoo on al.APSNo = aoo.APSNo
left join #APSPackingQty apo on al.APSNo = apo.APSNo
outer apply (SELECT val =  Stuff((select distinct concat( ',',CustPONo)   from #APSColumnGroup where APSNo = al.APSNo and CustPONo <> '' FOR XML PATH('')),1,1,'') ) as CustPO
outer apply (SELECT val =  Stuff((select distinct concat( ',',SP)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as SP
outer apply (SELECT val =  Stuff((select distinct concat( '+',Category)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Category
outer apply (SELECT val =  Stuff((select distinct concat( ',',Colorway)   from #APSListArticle where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Colorway
outer apply (SELECT val =  Stuff((select distinct concat( ',',CdCodeID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as CDCode
outer apply (SELECT val =  Stuff((select distinct concat( ',',ProductionFamilyID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as ProductionFamilyID
outer apply (SELECT val =  Stuff((select distinct concat( ',',StyleID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Style
outer apply (SELECT val =  Stuff((select distinct concat( ',',Artwork)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as ArtworkType
outer apply (select [KPILETA] = MAX(KPILETA),[MTLETA] = MAX(MTLETA),[InspDate] = MAX(InspDate) from #APSColumnGroup where APSNo = al.APSNo) as OrderMax
outer apply (SELECT val =  Stuff((select distinct concat( ',',Remarks)   from #APSRemarks where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as Remarks
outer apply (SELECT MaxSCIDelivery = Max(SCIDelivery),MinSCIDelivery = Min(SCIDelivery),
                    MaxBuyerDelivery = Max(BuyerDelivery),MinBuyerDelivery = Min(BuyerDelivery)
                    from #APSColumnGroup where APSNo = al.APSNo) as OrderDateInfo
outer apply (SELECT val =  Stuff((select distinct concat( ',',BrandID)   from #APSColumnGroup where APSNo = al.APSNo FOR XML PATH('')),1,1,'') ) as BrandID

--組出所有計畫最大Inline,最小Offline之間所有的日期，後面展開個計畫每日資料使用
Declare @StartDate date
Declare @EndDate date
select @StartDate = min(Inline),@EndDate = max(Offline)
from #APSList

SELECT f.FactoryID,cast(DATEADD(DAY,number,@StartDate) as datetime) [WorkDate]
into #WorkDate
FROM master..spt_values s
cross join (select distinct FactoryID from #APSList) f
WHERE s.type = 'P'
AND DATEADD(DAY,number,@StartDate) <= @EndDate

--展開計畫日期資料
select  al.APSNo,
        al.LearnCurveID,
        wkd.SewingLineID,
        wkd.FactoryID,
        [WorkDate] = cast( wkd.Date as datetime),
		al.inline,
		al.Offline,
		al.inlineDate,
		al.OfflineDate,
        [StartHour] = cast(wkd.StartHour as float),
        [EndHour] = cast(wkd.EndHour as float),
        al.InlineHour,
        al.OfflineHour,
		al.HourOutput,
		al.OriWorkHour,
		al.CPU,
		al.TotalSewingTime,
		al.Sewer,
		al.OrderID,
		al.LNCSERIALNumber,
        al.ComboType
into #Workhour_step1
from #APSListWorkDay al
inner join #WorkDate wd on wd.WorkDate >= al.InlineDate and wd.WorkDate <= al.OfflineDate and wd.FactoryID = al.FactoryID
inner join Workhour_Detail wkd with (nolock) on wkd.FactoryID = al.FactoryID and 
                                                wkd.SewingLineID = al.SewingLineID and 
                                                wkd.Date = wd.WorkDate

--刪除每個計畫inline,offline當天超過時間的班表                                                
delete #Workhour_step1 where WorkDate = InlineDate and EndHour <= InlineHour
delete #Workhour_step1 where WorkDate = OfflineDate and StartHour >= OfflineHour

--排出每天班表順序
select  APSNo,
        LearnCurveID,
        SewingLineID,
        FactoryID,
        WorkDate,
		inline,
		Offline,
		inlineDate,
		OfflineDate,
        StartHour,
        EndHour,
        InlineHour,
        OfflineHour,
		[StartHourSort] = ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY StartHour),
		[EndHourSort] = ROW_NUMBER() OVER (PARTITION BY APSNo,WorkDate,OrderID,ComboType ORDER BY EndHour desc),
		HourOutput,
		OriWorkHour,
		CPU,
		TotalSewingTime,
		Sewer,
		OrderID,
		LNCSERIALNumber,
        ComboType
into #Workhour_step2
from #Workhour_step1	

--依照班表順序，將inline,offline當天StartHour與EndHour update與inline,offline相同
update #Workhour_step2 set StartHour = InlineHour where WorkDate = InlineDate and StartHourSort = 1 and InlineHour > StartHour
update #Workhour_step2 set EndHour = OfflineHour where WorkDate = OfflineDate and EndHourSort = 1 and OfflineHour < EndHour

select 
APSNo,
LearnCurveID,
[SewingStart] = DATEADD(mi, min(StartHour) * 60,   WorkDate),
[SewingEnd] = DATEADD(mi, max(EndHour) * 60,   WorkDate),
WorkDate,
[WorkingTime] = sum(EndHour - StartHour),
[OriWorkDateSer] = ROW_NUMBER() OVER (PARTITION BY APSNo,OrderID,ComboType ORDER BY WorkDate),
HourOutput,
OriWorkHour,
CPU,
TotalSewingTime,
Sewer,
LNCSERIALNumber,
Orderid
into #APSExtendWorkDate_step1
from #Workhour_step2 
group by APSNo,LearnCurveID,WorkDate,HourOutput,
OriWorkHour,CPU,TotalSewingTime,Sewer,OrderID,LNCSERIALNumber,ComboType,Orderid

--欄位 LNCSERIALNumber 
--    第一天學習曲線計算方式 = 最後一天對應的工作天數 『減去』（該計畫總生產天數 『減去』一天因為要推出第一天）
--    ※ 但請注意以下特出例外狀況
--       1. LNCSERIALNumber 為空值或零
--       2. 計算後的結果為零或負數
--       遇到以上特殊例外狀況，
--       生產的第一天都對應學習曲線的第一天。
select
APSNo,
LearnCurveID,
SewingStart,
SewingEnd,
WorkDate,
WorkingTime,
OriWorkDateSer,
[WorkDateSer] = case	when isnull(LNCSERIALNumber,0) = 0 then OriWorkDateSer
						when LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) <= 0 then OriWorkDateSer
						else OriWorkDateSer + LNCSERIALNumber - isnull(max(OriWorkDateSer) OVER (PARTITION BY APSNo),0) end,
HourOutput,
OriWorkHour,
CPU,
TotalSewingTime,
Sewer,
LNCSERIALNumber,
Orderid
into #APSExtendWorkDate
from #APSExtendWorkDate_step1

--取得每個計劃去除LearnCurve後的總工時
SELECT
awd.APSNo,
awd.WorkDate,
[TotalWorkHour] = sum(OriWorkHour)
into #OriTotalWorkHour
FROM #APSExtendWorkDate awd
group by awd.APSNo,awd.WorkDate

--取得LearnCurve Efficiency by Work Date
select  awd.APSNo
        , awd.SewingStart
        , awd.SewingEnd
        , [SewingOutput] = isnull(apo.SewingOutput,0)
        , awd.WorkingTime
        , [LearnCurveEff] = ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))
		, StdOutput=s.StdQ
        --, [StdOutput] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour)) * ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0
        , [CPU] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour * awd.CPU)) * ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0
        , [Efficienycy] = SUM(iif (isnull (otw.TotalWorkHour, 0) = 0, 0, awd.WorkingTime * awd.HourOutput * OriWorkHour / otw.TotalWorkHour * awd.TotalSewingTime)) * ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0))/100.0 / (awd.WorkingTime * awd.Sewer * 3600.0)
into #APSExtendWorkDateFin
from #APSExtendWorkDate awd
inner join #OriTotalWorkHour otw on otw.APSNo = awd.APSNo and otw.WorkDate = awd.WorkDate
left join LearnCurve_Detail lcd with (nolock) on awd.LearnCurveID = lcd.ID and awd.WorkDateSer = lcd.Day
left join #APSSewingOutput apo on awd.APSNo = apo.APSNo and awd.WorkDate = apo.OutputDate
outer apply(select top 1 [val] = Efficiency from LearnCurve_Detail where ID = awd.LearnCurveID order by Day desc ) LastEff
outer  apply(select * from dbo.[getDailystdq](OrderID)x where x.APSNo=awd.APSNo and x.Date = cast(awd.SewingStart as date))s
group by awd.APSNo,
		 awd.SewingStart,
		 awd.SewingEnd,
		 isnull(apo.SewingOutput,0),
		 awd.WorkingTime,
		 ISNULL(lcd.Efficiency,ISNULL(LastEff.val,100.0)),
		 awd.Sewer,s.StdQ

--計算這一天的標準產量
--= (工作時數 / 車縫一件成衣需要花費的秒數) * 工人數 * 效率
--= (WorkingTime / SewingTime) * ManPower * Eff
--組合最終table
select
    apm.APSNo,
    apm.SewingLineID,
    apm.Sewer,
    [SewingDay] = cast(apf.SewingStart as date),
    apf.SewingStart,
    apf.SewingEnd,
    apm.MDivisionID,
    apm.FactoryID,
    apm.CustPO,
    apm.CustPoCnt,
    apm.SP,
    apm.SpCnt,
    apm.MinSCIDelivery,
    apm.MaxSCIDelivery,
    apm.MinBuyerDelivery,
    apm.MaxBuyerDelivery,
    apm.Category,
    apm.Colorway,
    apm.ColorwayCnt,
    apm.CDCode,
    apm.ProductionFamilyID,
    apm.Style,
    apm.StyleCnt,
    apm.OrderQty,
    apm.AlloQty,
    [StdOutput] = apf.StdOutput,
    apf.CPU,
    [SewingCPU] = ( apm.TotalSewingTime / (SELECT StdTMS * 1.0 from System) )  ,
    [DayWorkHour] = apf.WorkingTime,
    [HourOutput] = iif(apf.WorkingTime = 0,0,floor(apf.StdOutput / apf.WorkingTime)),
    [Efficienycy]=ROUND( apf.Efficienycy ,2,1),
    apm.OriEff / 100.0,
    apm.SewLineEff / 100.0,
    [LearnCurveEff] = apf.LearnCurveEff / 100.0,
    apm.Inline,
    apm.Offline,
    apm.PFRemark,
    apm.MTLComplete,
    apm.KPILETA,
    apm.MTLETA,
    apm.ArtworkType,
    apm.InspDate,
    apm.Remarks,
    apm.CuttingOutput,
    apf.SewingOutput,
    apm.ScannedQty,
    apm.ClogQty,
    apm.BrandID
from #APSMain apm
inner join #APSExtendWorkDateFin apf on apm.APSNo = apf.APSNo
order by apm.APSNo,apf.SewingStart

drop table	#APSListWorkDay,#APSList,#APSMain,#APSExtendWorkDateFin,#APSOrderQty,#APSCuttingOutput,#APSPackingQty,#APSSewingOutput,#APSRemarks,
			#tmpOrderArtwork,#APSListArticle,#APSColumnGroup,#WorkDate,#APSExtendWorkDate,#Workhour_step1,#Workhour_step2,#OriTotalWorkHour,#APSExtendWorkDate_step1
");
            #endregion

            DBProxy.Current.DefaultTimeout = 900;
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            DBProxy.Current.DefaultTimeout = 300;
            return result;
        }

        private void ComboSummaryBy_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboSummaryBy.SelectedIndex == 2)
            {
                this.checkForPrintOut.Enabled = false;
            }
            else
            {
                this.checkForPrintOut.Enabled = true;
            }
        }
    }
}
