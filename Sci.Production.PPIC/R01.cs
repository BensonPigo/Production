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
        private DateTime? inline;
        private DateTime? offline;
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
            this.inline = this.dateInlineAfter.Value;
            this.offline = this.dateOfflineBefore.Value;
            this.buyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.buyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.brand = this.txtbrand.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
with tmpAllArtwork as (
    select  ot.ID
            , at.Abbreviation
            , ot.Qty
            , ot.TMS
            , at.Classify
    from Order_TmsCost ot WITH (NOLOCK) 
         , ArtworkType at WITH (NOLOCK) 
    where   ot.ArtworkTypeID = at.ID
            and (ot.Price > 0 or at.Classify in ('O','I') )
            and (at.Classify in ('S','I') or at.IsSubprocess = 1)
            and (ot.TMS > 0 or ot.Qty > 0)
            and at.Abbreviation !=''
), 
tmpArtWork as (
    select * from (
        select  ID
                , Abbreviation+':'+Convert(varchar,Qty) as Artwork 
        from tmpAllArtwork 
        where Qty > 0
        
        union all
        select  ID
                , Abbreviation+':'+Convert(varchar,TMS) as Artwork 
        from tmpAllArtwork 
        where TMS > 0 and Classify in ('O','I')
    ) a
), 
tmpOrderArtwork as (
    select  tmpArtWorkID.ID
            , Artwork = (select   CONCAT(Artwork,', ') 
						 from tmpArtWork 
						 where ID = tmpArtWorkID.ID 
						 order by Artwork for xml path(''))  
    from (
		select distinct ID
		from tmpArtWork
	) tmpArtWorkID
),
cte As(
	select o.MDivisionID, o.FactoryID, OrderID = O.ID ,Oq.Qty, oq.Article,oq.SizeCode     
	from dbo.Orders o WITH (NOLOCK) 
	left join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
	where o.id IN (SELECT  ID FROM tmpOrderArtwork)
),
tmp as(
		Select DISTINCT
		[Bundleno] = bd.BundleNo,
		[Cut Ref#] = b.CutRef,
		[M] = b.MDivisionid,
		[Factory] = o.FtyGroup,
		[SP] = b.Orderid,
		SubProcessId = s.Id,
		b.article,
		[Size] = bd.SizeCode,
		[Comb] = b.PatternPanel,
		b.FabricPanelCode,
		bd.PatternCode,
		bd.Qty,
		bio.InComing,
		bio.OutGoing
	from Bundle b WITH (NOLOCK) 
	inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id
	left join Bundle_Detail_Art bda WITH (NOLOCK) on bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
	inner join orders o WITH (NOLOCK) on o.Id = b.OrderId
	inner join SubProcess s WITH (NOLOCK) on (s.IsRFIDDefault = 1 or s.Id = bda.SubprocessId) 
	left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
	inner join Order_EachCons oe WITH (NOLOCK) on oe.id = o.poid and oe.FabricPanelCode = b.FabricPanelCode
	inner join Order_BOF bof WITH (NOLOCK) on bof.Id = oe.Id and bof.FabricCode = oe.FabricCode
	where 1=1
	AND EXISTS (SELECT 1 FROM cte WHERE OrderID= b.Orderid AND MDivisionID= b.MDivisionID AND FactoryID=o.FactoryID)
	
	and bof.kind != 0
),
tmp2 as(
	select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode,accuQty = sum(iif(InComing is null ,0,Qty))
	from tmp
	group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,PatternCode
),
tmp3 AS(
	select [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode,accuQty = min(accuQty)
	from tmp2
	group by [M],[Factory],[SP],[Subprocessid],article,[Size],[Comb],FabricPanelCode
),
tmp4 AS(
	select [M],[Factory],[SP],[Subprocessid],article,[Size],accuQty = min(accuQty)
	from tmp3
	group by [M],[Factory],[SP],[Subprocessid],article,[Size]
),
LoadingOutput AS(
	select [OrderID]=t.OrderID,[AccuInCome]=SUM(loading.[AccuInCome])
	from cte t 
	outer apply (
		select [AccuInCome] = iif (AccuQty > t.Qty, t.Qty, AccuQty) 
		from tmp4 
		where tmp4.SP = t.OrderID 
			  and tmp4.Factory = t.FactoryID
			  and tmp4.SubProcessId = 'loading'
			  and tmp4.Article = t.Article 
			  and tmp4.Size = t.SizeCode
	) loading
	GROUP BY t.OrderID
)



select  SewingLineID
        , MDivisionID
        , FactoryID
        , OrderID
        , ComboType
        , IIF(Article = '','',SUBSTRING(Article,1,LEN(Article)-1)) as Article
        , CdCodeID
        , StyleID
        , Qty
        , AlloQty
        , CutQty
		, LoadingOutPut
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
        , CPU
        , VasShas
        , ShipModeList
        , Alias
        , ArtWork
        , IIF(Remark = '','',SUBSTRING(Remark,1,LEN(Remark)-1)) as Remark
from (
    select  s.SewingLineID
            , s.MDivisionID
            , o.FactoryID
            , s.OrderID
            , s.ComboType
            , ( select CONCAT(Article,',') 
                from (  select distinct Article 
                        from SewingSchedule_Detail sd WITH (NOLOCK) 
                        where sd.ID = s.ID
                ) a for xml path('')) as Article
            , o.CdCodeID
            , o.StyleID
            , o.Qty
            , s.AlloQty
            , isnull((  select sum(Qty) 
                        from CuttingOutput_WIP c WITH (NOLOCK) 
                        where   c.OrderID = s.OrderID 
                                and c.Article in (  select Article 
                                                    from SewingSchedule_Detail sd WITH (NOLOCK) 
                                                    where sd.ID = s.ID)
                     ) ,0) as CutQty
			, [LoadingOutPut]=lo.AccuInCome
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
            , ( select IIF(ctn = 0, 0, Hours/ctn) 
                from (  Select  isnull(sum(w.Hours),0) as Hours
                                , Count(w.Date) as ctn 
                        from WorkHour w WITH (NOLOCK) 
                        where   FactoryID = s.FactoryID 
                                and w.SewingLineID = s.SewingLineID 
                                and w.Date between Convert(Date,s.Inline) 
                                and Convert(Date,s.Offline) 
                                and w.Hours > 0
                ) a
              ) as WorkHour
            , s.MaxEff
            , o.KPILETA
            , isnull((  Select top 1 op.Remark 
                        from Order_PFHis op WITH (NOLOCK) 
                        where   op.ID = s.OrderID 
                                and op.AddDate = (  Select Max(AddDate) 
                                                    from Order_PFHis WITH (NOLOCK) 
                                                    where ID = s.OrderID)
                    ),'') as PFRemark
            , o.MTLETA
            , o.MTLExport
            , O.CutInLine
            , s.Inline
            , s.Offline
            , o.SciDelivery
            , o.BuyerDelivery
            , o.CPU * o.CPUFactor * ( isnull(isnull(ol_rate.value,sl_rate.value), 100) / 100) as CPU
            , IIF(o.VasShas=1, 'Y', '') as VasShas
            , o.ShipModeList,isnull(c.Alias, '') as Alias
            , isnull(SUBSTRING(ta.Artwork, 1, LEN(ta.Artwork) - 1), '') as ArtWork
            , isnull((  select CONCAT(Remark, ', ') 
                        from (  select s1.SewingLineID+'('+s1.ComboType+'):'+CONVERT(varchar,s1.AlloQty) as Remark 
                                from SewingSchedule s1 WITH (NOLOCK) 
                                where   s1.OrderID = s.OrderID 
                                        and s1.ID != s.ID
                        ) a for xml path('')
                    ), '') as Remark
    from SewingSchedule s WITH (NOLOCK) 
    inner join Orders o WITH (NOLOCK) on o.ID = s.OrderID
   -- left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey
	--											 and s.ComboType = sl.Location
    left join tmpOrderArtwork ta on ta.ID = s.OrderID
	left join LoadingOutput lo on lo.OrderID = s.OrderID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    outer apply(select value = dbo.GetOrderLocation_Rate(o.id,s.ComboType) ) ol_rate
    outer apply(select value = dbo.GetStyleLocation_Rate(o.StyleUkey,s.ComboType) ) sl_rate
    where 1 = 1 
");
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

            if (!MyUtility.Check.Empty(this.inline))
            {
                sqlCmd.Append(string.Format(" and s.Inline >= '{0}'", Convert.ToDateTime(this.inline).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.offline))
            {
                sqlCmd.Append(string.Format(" and s.Offline < '{0}'", Convert.ToDateTime(this.offline).AddDays(1).ToString("d")));
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

            sqlCmd.Append(@" 
) a
order by SewingLineID,MDivisionID,FactoryID,Inline,StyleID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
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

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            bool result = false;

            if (this.checkForPrintOut.Checked == true)
            {
                #region PPIC_R01_PrintOut
                this.printData.Columns.Remove("MDivisionID");
                this.printData.Columns.Remove("PFRemark");
                this.printData.Columns.Remove("BuyerDelivery");
                this.printData.Columns.Remove("VasShas");
                this.printData.Columns.Remove("ShipModeList");
                this.printData.Columns.Remove("Alias");

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
                result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "PPIC_R01_SewingLineScheduleReport.xltx", headerRow: 1);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    return false;
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
    }
}
