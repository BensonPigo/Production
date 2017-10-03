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
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string mDivision, factory, line1, line2, brand;
        DateTime? inline, offline, buyerDelivery1, buyerDelivery2, sciDelivery1, sciDelivery2;

        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FTYGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboM.Text = Sci.Env.User.Keyword;
            
         //   comboBox2.SelectedIndex = 0;
            comboFactory.Text = Sci.Env.User.Factory;
        }

        //Sewing Line
        private void txtSewingLineStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            txtSewingLineStart.Text = SelectSewingLine(txtSewingLineStart.Text);
        }

        //Sewing Line
        private void txtSewingLineEnd_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            txtSewingLineEnd.Text = SelectSewingLine(txtSewingLineEnd.Text);
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine WITH (NOLOCK) {0}  ", MyUtility.Check.Empty(comboFactory.Text) ? "" : string.Format(" where FactoryID = '{0}'", comboFactory.Text));
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "3", line, false, ",");
            item.Width = 300;
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return "";
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            mDivision = comboM.Text;
            factory = comboFactory.Text;
            line1 = txtSewingLineStart.Text;
            line2 = txtSewingLineEnd.Text;
            inline = dateInlineAfter.Value;
            offline = dateOfflineBefore.Value;
            buyerDelivery1 = dateBuyerDelivery.Value1;
            buyerDelivery2 = dateBuyerDelivery.Value2;
            sciDelivery1 = dateSCIDelivery.Value1;
            sciDelivery2 = dateSCIDelivery.Value2;
            brand = txtbrand.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
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
            and (ot.Price > 0 or at.Classify = 'O')
            and (at.Classify = 'S' or at.IsSubprocess = 1)
            and (ot.TMS > 0 or ot.Qty > 0)
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
        where TMS > 0 and Classify = 'O'
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
            , s.FactoryID
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
            , s.Inline
            , s.Offline
            , o.SciDelivery
            , o.BuyerDelivery
            , o.CPU * o.CPUFactor * ( isnull(sl.Rate, 100) / 100) as CPU
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
    left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey
												 and s.ComboType = sl.Location
    left join tmpOrderArtwork ta on ta.ID = s.OrderID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    where 1 = 1 
");
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", mDivision));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", factory));
            }

            if (!MyUtility.Check.Empty(line1))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID >= '{0}'", line1));
            }

            if (!MyUtility.Check.Empty(line2))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID <= '{0}'", line2));
            }

            if (!MyUtility.Check.Empty(inline))
            {
                sqlCmd.Append(string.Format(" and s.Inline >= '{0}'", Convert.ToDateTime(inline).ToString("d")));
            }

            if (!MyUtility.Check.Empty(offline))
            {
                sqlCmd.Append(string.Format(" and s.Offline < '{0}'", Convert.ToDateTime(offline).AddDays(1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(buyerDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(buyerDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", brand));
            }

            sqlCmd.Append(@" 
) a
order by SewingLineID,MDivisionID,FactoryID,Inline,StyleID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            bool result = false;

            if (checkForPrintOut.Checked == true)
            {
                #region PPIC_R01_PrintOut
                printData.Columns.Remove("MDivisionID");
                printData.Columns.Remove("PFRemark");
                printData.Columns.Remove("BuyerDelivery");
                printData.Columns.Remove("VasShas");
                printData.Columns.Remove("ShipModeList");
                printData.Columns.Remove("Alias");

                Excel.Application objApp = null;
                Excel.Worksheet worksheet = null;

                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_R01_PrintOut.xltx"); //預先開啟excel app
                result = MyUtility.Excel.CopyToXls(printData, "", xltfile: "PPIC_R01_PrintOut.xltx", headerRow: 4, showExcel: false, excelApp: objApp);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    return false;
                }
                this.ShowWaitMessage("Excel Processing...");
                worksheet = objApp.Sheets[1];

                #region Set Excel Title
                string factoryName = MyUtility.GetValue.Lookup(string.Format(@"
select NameEn 
from Factory 
where id = '{0}'", Sci.Env.User.Factory), null);
                worksheet.Cells[1, 1] = factoryName; 
                worksheet.Cells[2, 1] = "Sewing Line Schedule Report";
                worksheet.Cells[3, 1] = "Date:" + DateTime.Now.ToString("yyyy/MM/dd");
                #endregion 
                for (int i = 1; i < printData.Rows.Count; i++)
                {
                    DataRow frontRow = printData.Rows[i - 1];
                    DataRow Row = printData.Rows[i];

                    //當前後 SyleID 不同時，中間加上虛線
                    if ( !frontRow["StyleID"].EqualString(Row["StyleID"]))
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
                result = MyUtility.Excel.CopyToXls(printData, "", xltfile: "PPIC_R01_SewingLineScheduleReport.xltx", headerRow: 1);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    return false;
                }
                #endregion
            }            
            return true;
        }

        private void comboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSewingLineStart.Text = "";
            txtSewingLineEnd.Text = "";
        }

        private void txtSewingLineStart_Validating(object sender, CancelEventArgs e)
        {
            if (txtSewingLineStart.Text == txtSewingLineStart.OldValue) return;
            if (!MyUtility.Check.Empty(txtSewingLineStart.Text))
            {
                string sql = string.Format("Select ID From SewingLine WITH (NOLOCK) where id='{0}' {1} ", txtSewingLineStart.Text, MyUtility.Check.Empty(comboFactory.Text) ? "" : string.Format(" and FactoryID = '{0}'", comboFactory.Text)); 
                if (!MyUtility.Check.Seek(sql))
                {
                    txtSewingLineStart.Text = "";
                    MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", txtSewingLineStart.Text));
                    return;
                }
            }
        }

        private void txtSewingLineEnd_Validating(object sender, CancelEventArgs e)
        {
            if (txtSewingLineEnd.Text == txtSewingLineEnd.OldValue) return;
            if (!MyUtility.Check.Empty(txtSewingLineEnd.Text))
            {
                string sql = string.Format("Select ID From SewingLine WITH (NOLOCK) where id='{0}' {1} ", txtSewingLineEnd.Text, MyUtility.Check.Empty(comboFactory.Text) ? "" : string.Format(" and FactoryID = '{0}'", comboFactory.Text));
                if (!MyUtility.Check.Seek(sql))
                {
                    txtSewingLineEnd.Text = "";
                    MyUtility.Msg.WarningBox(string.Format("< Sewing Line: {0} > not found!!!", txtSewingLineEnd.Text));
                    return;
                }
            }
        }
    }
}
