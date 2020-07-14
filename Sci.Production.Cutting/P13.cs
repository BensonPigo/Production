using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P13 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.grid.IsEditingReadOnly = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("Sel", header: "Sel", trueValue: 1, falseValue: 0, iseditable: true)
                .Text("ID", header: "ID", iseditingreadonly: true)
                .Text("POID", header: "POID", iseditingreadonly: true)
                .Text("M", header: "M", iseditingreadonly: true)
                .Text("Factory", header: "Factory", iseditingreadonly: true)
                .Text("Style", header: "Style", iseditingreadonly: true)
                .Text("CutRefNo", header: "Cut Ref#", iseditingreadonly: true)
                .Text("SPNo", header: "SP#", iseditingreadonly: true)
                .Text("Size", header: "Size", iseditingreadonly: true)
                .Text("Color", header: "Color", iseditingreadonly: true)
                .Text("Article", header: "Article", iseditingreadonly: true)
                .Text("PatternPanel", header: "PatternPanel", iseditingreadonly: true)
                .Text("CutNo", header: "CutNo", iseditingreadonly: true)
                .Date("CreateDate", header: "CreateDate", iseditingreadonly: true)
                .Date("EstCutDate", header: "Est. cut date", iseditingreadonly: true)
                .Text("LineID", header: "Line#", iseditingreadonly: true)
                .Text("Item", header: "Item", iseditingreadonly: true)
                .Text("SewingCell", header: "SewingCell", iseditingreadonly: true)
                .Text("SizeRatio", header: "SizeRatio", iseditingreadonly: true)
                .Date("PrintDate", header: "PrintDate", iseditingreadonly: true);
        }

        private void ButtonQuery_Click(object sender, EventArgs e)
        {
            if (this.textBoxPOID.Text.Empty() && this.txtCutNo.Text.Empty() &&
                this.dateRangeCreateDate.Value1.Empty() && this.dateRangeCreateDate.Value2.Empty())
            {
                MyUtility.Msg.WarningBox("[POID],[CutNo],[Create Date] can not all be empty.");
                this.bindingSource1.DataSource = null;
                return;
            }

            string where = string.Empty;

            if (!this.textBoxPOID.Text.Empty())
            {
                where += "\r\n" + $"and b.POID = '{this.textBoxPOID.Text}'";
            }

            if (!this.txtCutNo.Text.Empty())
            {
                where += "\r\n" + $@"and b.cutno  = '{this.txtCutNo.Text}'";
            }

            if (!this.dateRangeCreateDate.Value1.Empty())
            {
                where += $"and '{((DateTime)this.dateRangeCreateDate.Value1).ToString("yyyy/MM/dd")}' <= b.CDate ";
            }

            if (!this.dateRangeCreateDate.Value2.Empty())
            {
                where += $"and b.CDate <= '{((DateTime)this.dateRangeCreateDate.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!this.textBoxPatterPanel.Text.Empty())
            {
                where += "\r\n" + $"and b.PatternPanel = '{this.textBoxPatterPanel.Text}'";
            }

            string strQuerySQL = $@"
select Sel = 0
	   , b.ID
	   , b.POID
	   , M = b.MDivisionid
	   , Factory =  O.FtyGroup 
	   , Style = o.StyleID 
	   , CutRefNo = b.CutRef
	   , SPNo = b.Orderid
	   , Size = b.Sizecode
	   , Color = b.Colorid
	   , b.Article
	   , b.PatternPanel
	   , b.CutNo
	   , CreateDate = b.CDate
	   , EstCutDate = (Select Max(estcutdate)
					   from workorder WITH (NOLOCK) 
					   where workorder.id = b.POID 
							 and workorder.cutref = b.CutRef and workorder.MDivisionID  = b.MDivisionID)
	   , LineID = b.Sewinglineid
	   , b.Item
	   , b.SewingCell
	   , SizeRatio = b.Ratio
	   , b.PrintDate
       , b.SewingCell
from Bundle b
inner join Orders O with(nolock) on  b.Orderid = o.id  and o.MDivisionID  = b.MDivisionID 
where 1=1
{where}
";

            DataTable resultDt;
            DualResult result = DBProxy.Current.Select(null, strQuerySQL, out resultDt);
            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                this.bindingSource1.DataSource = null;
                return;
            }

            this.bindingSource1.DataSource = resultDt;
            this.grid.ColumnsAutoSize();
        }

        private void ButtonPrint_Click(object sender, EventArgs e)
        {
            List<string> openExcelFile = this.ExcelProcess("Print Processing");
            if (openExcelFile != null)
            {
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    string printer = printDialog.PrinterSettings.PrinterName;
                    foreach (string workBook in openExcelFile)
                    {
                        Microsoft.Office.Interop.Excel.Workbook objBook = MyUtility.Excel.ConnectExcel(workBook).ActiveWorkbook;
                        objBook.PrintOutEx(ActivePrinter: printer);
                        objBook.Close();
                        Marshal.FinalReleaseComObject(objBook);

                        // 刪除暫存檔案
                        System.IO.File.Delete(workBook);
                    }
                }
            }
        }

        private void ButtonToExcel_Click(object sender, EventArgs e)
        {
            List<string> openExcelFile = this.ExcelProcess("Excel Processing");
            if (openExcelFile != null)
            {
                foreach (string workBook in openExcelFile)
                {
                    workBook.OpenFile();
                }
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ExcelProcess
        /// </summary>
        /// <param name="processName">string processName</param>
        /// <returns>workBook List</returns>
        private List<string> ExcelProcess(string processName)
        {
            DataTable printDt = (DataTable)((BindingSource)this.grid.DataSource).DataSource;
            if (printDt.AsEnumerable().Any(row => row["Sel"].EqualDecimal(1)) == false)
            {
                MyUtility.Msg.InfoBox("Please select data first.");
                return null;
            }
            else
            {
                printDt = printDt.AsEnumerable().Where(row => row["Sel"].EqualDecimal(1)).CopyToDataTable();
            }

            string strExcelDataSQL = string.Empty;
            string strExtend = string.Empty;
            DataTable resultDt;
            DualResult result;

            if (this.checkBoxExtendAllParts.Checked == true)
            {
                #region ExtendAllParts = true
                strExcelDataSQL = @"
select distinct [Group]
     , [Bundle]
     , [Size]
     , [Cutpart]
     , [Description]
     , [SubProcess]
     , [Parts],[Qty]
from (
  select b.id [Bundle_ID]
       , b.Orderid [SP]
       , b.POID [POID]
       , c.StyleID [Style]
       , b.Sewinglineid [Line]
       , b.SewingCell [Cell]
       , b.Cutno [Cut]
       , b.Item [Item]
       , b.Article+' / '+b.Colorid [Article_Color]
       , a.BundleGroup [Group]
       , a.BundleNo [Bundle]
       , a.SizeCode [Size]
       , qq.Cutpart [Cutpart]
       , a.PatternDesc [Description]
       , Artwork.Artwork [SubProcess]
       , a.Parts [Parts]
       , a.Qty [Qty]
  from dbo.Bundle_Detail a WITH (NOLOCK) 
  left join dbo.Bundle b WITH (NOLOCK) on a.id = b.id
  left join dbo.orders c WITH (NOLOCK) on c.id = b.Orderid and c.MDivisionID  = b.MDivisionID 
  outer apply ( 
    select [Cutpart] = iif (a.PatternCode = 'ALLPARTS', iif (@extend = '1', a.PatternCode
                                                                          , a.PatternCode)
                                                      , a.PatternCode) 
  )[qq]
  outer apply (
    select Artwork = (select iif (e1.SubprocessId is null or e1.SubprocessId = '', ''
                                                                                 , e1.SubprocessId+'+')
              from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
              where e1.id = b.id 
                    and e1.PatternCode = qq.Cutpart 
                    and e1.Bundleno=a.BundleNo
              for xml path(''))
  ) Artwork
  where a.ID = @ID 
        and a.Patterncode != 'ALLPARTS'

  union all
  select b.id [Bundle_ID]
         , b.Orderid [SP]
         , b.POID [POID]
         , c.StyleID [Style]
         , b.Sewinglineid [Line]
         , b.SewingCell [Cell]
         , b.Cutno [Cut]
         , b.Item [Item]
         , b.Article+' / '+b.Colorid [Article_Color]
         , a.BundleGroup [Group]
         , a.BundleNo [Bundle]
         , a.SizeCode [Size]
         , qq.Cutpart [Cutpart]
         , d.PatternDesc [Description]
         , Artwork.Artwork [SubProcess]
         , d.Parts [Parts]
         , a.Qty [Qty]
  from dbo.Bundle_Detail a WITH (NOLOCK) 
  left join dbo.Bundle b WITH (NOLOCK) on a.id = b.id
  left join dbo.orders c WITH (NOLOCK) on c.id = b.Orderid and c.MDivisionID  = b.MDivisionID 
  left join dbo.Bundle_Detail_Allpart d WITH (NOLOCK) on d.id = a.Id
  outer apply (
    select [Cutpart] = iif (a.PatternCode = 'ALLPARTS', iif(@extend = '1', d.PatternCode
                                                                         , a.PatternCode)
                                                      , a.PatternCode) 
  )[qq]
  outer apply (
    select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId = '', ''
                                                                                , e1.SubprocessId+'+')
                      from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
                      where e1.id = b.id 
                            and e1.PatternCode = qq.Cutpart 
                            and e1.Bundleno=a.BundleNo
                      for xml path(''))
  ) Artwork
  where a.ID = @ID and a.Patterncode = 'ALLPARTS'
) x
outer apply (
  select SizeSpec = SizeSpec.value
  from MNOrder m
  inner join Production.dbo.MNOrder_SizeItem msi on msi.ID = m.OrderComboID
  left join Production.dbo.MNOrder_SizeCode msc on msi.Id = msc.Id
  left join Production.dbo.MNOrder_SizeSpec mss on msi.Id = mss.Id 
                                                   and msi.SizeItem = mss.SizeItem 
                                                   and mss.SizeCode = msc.SizeCode
  left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso on msi.Id = msso.Id 
                                                               and msso.OrderComboID = m.id 
                                                               and msi.SizeItem = msso.SizeItem 
                                                               and msso.SizeCode = msc.SizeCode
outer apply (
		select value = iif(mss.SizeCode is not null, mss.SizeCode
												    , msso.SizeCode)
	) SizeSpec
  where (mss.SizeCode is not null or msso.SizeCode  is not null) 
      and msi.SizeItem = 'S01' 
      and m.ID = x.[SP]
      and SizeSpec.value = x.[Size]
) cu
order by x.[Bundle]";
                #endregion
                strExtend = "1";
            }
            else
            {
                #region ExtendAllPart = False
                strExcelDataSQL = @"
select distinct [Group]
	   , [Bundle]
	   , [Size]
	   , [Cutpart]
	   , [Description]
	   , [SubProcess]
	   , [Parts]
	   , [Qty]
from (
	select b.id [Bundle_ID]
			,b.Orderid [SP]
			,b.POID [POID]
			,c.StyleID [Style]
			,b.Sewinglineid [Line]
			,b.SewingCell [Cell]
			,b.Cutno [Cut]
			,b.Item [Item]
			,b.Article+' / '+b.Colorid [Article_Color]
			,a.BundleGroup [Group]
			,a.BundleNo [Bundle]
			,a.SizeCode [Size]
			,qq.Cutpart [Cutpart]
			,a.PatternDesc [Description]
			,Artwork.Artwork [SubProcess]
			,a.Parts [Parts]
			,a.Qty [Qty]
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
	left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
	outer apply ( 
		select [Cutpart]  = iif (a.PatternCode = 'ALLPARTS', iif (@extend = '1', a.PatternCode
																			   , a.PatternCode)
														   , a.PatternCode) 
	) [qq]
	outer apply ( 
		select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='', ''
																				  , e1.SubprocessId+'+')
						  from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
						  where e1.id = b.id 
						  		and e1.PatternCode = qq.Cutpart 
						  		and e1.Bundleno = a.BundleNo
						  for xml path(''))
	)as Artwork
	where a.ID = @ID 
		  and a.Patterncode != 'ALLPARTS'

	union all
	select b.id [Bundle_ID]
		   , b.Orderid [SP]
		   , b.POID [POID]
		   , c.StyleID [Style]
		   , b.Sewinglineid [Line]
		   , b.SewingCell [Cell]
		   , b.Cutno [Cut]
		   , b.Item [Item]
		   , b.Article+' / '+b.Colorid [Article_Color]
		   , a.BundleGroup [Group]
		   , a.BundleNo [Bundle]
		   , a.SizeCode [Size]
		   , qq.Cutpart [Cutpart]
		   , a.PatternDesc [Description]
		   , Artwork.Artwork [SubProcess]
		   , a.Parts [Parts]
		   , a.Qty [Qty]
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
	left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid and c.MDivisionID  = b.MDivisionID 
	outer apply (
		select [Cutpart] = iif (a.PatternCode = 'ALLPARTS', iif (@extend = '1', a.PatternCode
																			  , a.PatternCode)
														  , a.PatternCode)
	) [qq]
	outer apply ( 
		select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId = '', ''
																				    , e1.SubprocessId+'+')
						  from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
						  where e1.id = b.id 
						  		and e1.PatternCode = qq.Cutpart 
						  		and e1.Bundleno = a.BundleNo
						  for xml path(''))
	) Artwork
	where a.ID = @ID 
	and a.Patterncode = 'ALLPARTS'
) x
outer apply (
	select SizeSpec = SizeSpec.value
	from MNOrder m
	inner join Production.dbo.MNOrder_SizeItem msi on msi.ID = m.OrderComboID
	left join Production.dbo.MNOrder_SizeCode msc on msi.Id = msc.Id
	left join Production.dbo.MNOrder_SizeSpec mss on msi.Id = mss.Id 
												     and msi.SizeItem = mss.SizeItem 
												     and mss.SizeCode = msc.SizeCode
	left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso on msi.Id = msso.Id 
																 and msso.OrderComboID = m.id 
																 and msi.SizeItem = msso.SizeItem 
																 and msso.SizeCode = msc.SizeCode
		outer apply (
		select value = iif(mss.SizeCode is not null, mss.SizeCode
												    , msso.SizeCode)
	) SizeSpec
	where (mss.SizeCode is not null or msso.SizeCode  is not null) 
		  and msi.SizeItem = 'S01' 
		  and m.ID = x.[SP]
		  and SizeSpec.value = x.[Size]
)cu
order by x.[Bundle]";
                #endregion
                strExtend = "0";
            }

            int printSheetNum = 0;
            List<string> openWorkBook = new List<string>();
            string printExcelName = Class.MicrosoftFile.GetName("Cutting_P13");
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_P10.xltx"); // 預先開啟excel app

            foreach (DataRow printDr in printDt.Rows)
            {
                this.ShowWaitMessage($"{processName} ({printSheetNum + 1} / {printDt.Rows.Count})", 500);

                /*
                 * Sheet 上限 225
                 * 一但超過開啟新的 Excel
                 */
                if (printSheetNum % 255 == 0 && printSheetNum != 0)
                {
                    objApp.Sheets.Move(objApp.Worksheets[1]);
                    objApp.ActiveWorkbook.SaveAs(printExcelName);
                    objApp.Quit();
                    openWorkBook.Add(printExcelName);

                    objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_P10.xltx");
                    printExcelName = Class.MicrosoftFile.GetName("Cutting_P13");
                }

                #region 取得每一個 ID 的資料
                List<SqlParameter> listSqlParameter = new List<SqlParameter>();
                listSqlParameter.Add(new SqlParameter("@ID", printDr["ID"]));
                listSqlParameter.Add(new SqlParameter("@extend", strExtend));

                result = DBProxy.Current.Select(string.Empty, strExcelDataSQL, listSqlParameter, out resultDt);
                if (result == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    this.HideWaitMessage();
                    return null;
                }
                #endregion

                // 取得工作表
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[(printSheetNum % 255) + 1];

                /*
                 * 新增資料前
                 * 將所有格式複製到下一個 WorkSheet
                 * 條件如下 :
                 *      1. WorkSheet 不可為第 255 個
                 *      2. 此筆資料不是最後一筆
                 */
                if (printSheetNum % 255 != 254 && printSheetNum < printDt.Rows.Count - 1)
                {
                    objSheets.Copy(objApp.Sheets[(printSheetNum % 255) + 2]);
                }

                objSheets.Name = printDr["ID"].ToString();

                #region Set WorkSheet
                objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where id = '{0}'", printDr["ID"].ToString().Substring(0, 3)));
                objSheets.Cells[3, 1] = "To Line: " + printDr["LineID"].ToString();
                objSheets.Cells[3, 3] = "Cell: " + printDr["SewingCell"].ToString();
                objSheets.Cells[3, 4] = "Comb: " + printDr["PatternPanel"].ToString();
                objSheets.Cells[3, 5] = "Marker No: " + (printDr["CutRefNo"].ToString() == string.Empty ? string.Empty : MyUtility.GetValue.Lookup(string.Format(@"select top 1 MarkerNo from WorkOrder where  CutRef='{0}'", printDr["CutRefNo"].ToString())));
                objSheets.Cells[3, 7] = "Item: " + printDr["Item"].ToString();
                objSheets.Cells[3, 9] = "Article/Color: " + printDr["Article"].ToString() + "/ " + printDr["Color"].ToString();
                objSheets.Cells[3, 11] = "ID: " + printDr["ID"].ToString();
                objSheets.Cells[4, 1] = "SP#: " + printDr["SPNo"].ToString();
                objSheets.Cells[4, 4] = "Style#: " + MyUtility.GetValue.Lookup(string.Format("Select Styleid from Orders WITH (NOLOCK) Where id='{0}'", printDr["SPNo"].ToString()));
                objSheets.Cells[4, 7] = "Cutting#: " + printDr["CutNo"].ToString();
                objSheets.Cells[4, 9] = "MasterSP#: " + printDr["POID"].ToString();
                objSheets.Cells[4, 11] = "DATE: " + DateTime.Today.ToShortDateString();
                MyUtility.Excel.CopyToXls(resultDt, string.Empty, "Cutting_P10.xltx", 5, false, null, objApp, wSheet: objSheets);      // 將datatable copy to excel
                objSheets.get_Range("D1:D1").ColumnWidth = 11;
                objSheets.get_Range("E1:E1").Columns.AutoFit();
                objSheets.get_Range("G1:H1").ColumnWidth = 9;
                objSheets.get_Range("I1:L1").ColumnWidth = 15;

                objSheets.Range[string.Format("A6:L{0}", resultDt.Rows.Count + 5)].Borders.Weight = 2; // 設定全框線
                #endregion

                Marshal.ReleaseComObject(objSheets);
                printSheetNum++;
            }

            objApp.Sheets.Move(objApp.Worksheets[1]);
            objApp.ActiveWorkbook.SaveAs(printExcelName);
            objApp.Quit();
            openWorkBook.Add(printExcelName);
            Marshal.ReleaseComObject(objApp);

            this.HideWaitMessage();

            if (openWorkBook.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return null;
            }

            return openWorkBook;
        }

        private void TxtCutNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int n = 0;
            if (!int.TryParse(this.txtCutNo.Text, out n))
            {
                this.txtCutNo.Text = string.Empty;
                MyUtility.Msg.WarningBox("[Cutno] must enter number");
            }
            else
            {
                this.txtCutNo.Text = n.ToString();
            }
        }
    }
}
