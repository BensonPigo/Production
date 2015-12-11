using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace Sci.Production.Warehouse
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateRange1.TextBox1.Value))
            {
                MyUtility.Msg.WarningBox("Issue date can't be empty!!");
                return false;
            }
            return base.ValidateInput();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            try
            {
                return true;
                //return MyUtility.Excel.copytoxls(dt, "", "Warehouse_R02.xltx", 1,true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private static bool copytoxls(DataTable data, string fileNM, string xltfile = null, int hdrow = 1, bool showExcel=true)
        //{
        //    MyUtility.Msg.WaitWindows("Excel Processing...");
        //    Microsoft.Office.Interop.Excel.Application objApp;
        //    Microsoft.Office.Interop.Excel._Workbook objBook;

        //    Microsoft.Office.Interop.Excel.Workbooks objBooks;
        //    Microsoft.Office.Interop.Excel.Sheets objSheets;
        //    Microsoft.Office.Interop.Excel._Worksheet objSheet;
        //    Microsoft.Office.Interop.Excel.Range range;

        //    string tempSysPath = Application.StartupPath;//軟體安裝目錄
        //    string xltpath = tempSysPath + @"\xlt\";  //暫存目錄tmp

        //    bool flag = System.IO.File.Exists(xltpath + xltfile);
        //    Object[,] saRet;
        //    try
        //    {
        //        // Instantiate Excel and start a new workbook.
        //        objApp = new Excel.Application();
        //        objBooks = objApp.Workbooks;
        //        string startColumn = "";
        //        if (null == xltfile)
        //        {
        //            objBook = objBooks.Add(Missing.Value);
        //            startColumn = "A1";
        //            saRet = new Object[data.Rows.Count + hdrow, data.Columns.Count];
        //        }
        //        else
        //        {
        //            objBook = objBooks.Add(xltpath + xltfile);
        //            startColumn = "A" + (hdrow + 1).ToString().Trim();
        //            saRet = new Object[data.Rows.Count, data.Columns.Count];
        //        }
        //        objSheets = objBook.Worksheets;
        //        objSheet = (Excel._Worksheet)objSheets.get_Item(1);

        //        //Get the range where the starting cell has the address
        //        //m_sStartingCell and its dimensions are m_iNumRows x m_iNumCols.

        //        range = objSheet.get_Range(startColumn, Missing.Value);
        //        range = range.get_Resize(data.Rows.Count + hdrow, data.Columns.Count);

        //        //Create an array.
        //        if (null == xltfile)
        //        {
        //            for (int i = 0; i < data.Columns.Count; i++)
        //            {
        //                saRet[0, i] = data.Columns[i].ColumnName;
        //            }
        //        }

        //        //Fill the array.
        //        for (int iRow = 0; iRow < data.Rows.Count; iRow++)
        //        {
        //            for (int iCol = 0; iCol < data.Columns.Count; iCol++)
        //            {
        //                //Put the row and column address in the cell.
        //                if (null == xltfile)
        //                {
        //                    saRet[iRow + hdrow, iCol] = data.Rows[iRow][iCol];
        //                }
        //                else
        //                {
        //                    saRet[iRow, iCol] = data.Rows[iRow][iCol];
        //                }
        //            }
        //        }

        //        //Set the range value to the array.
        //        range.set_Value(Missing.Value, saRet);

        //        //Return control of Excel to the user.
        //        objSheet.Activate();
        //        objApp.Selection.ColumnWidth = 44.5;
        //        objSheet.Cells.EntireRow.AutoFit();
        //        objSheet.Cells.EntireColumn.AutoFit();
        //        objApp.Visible = true;
        //        objApp.UserControl = true;

        //    }
        //    catch (Exception theException)
        //    {
        //        String errorMessage;
        //        errorMessage = "Error: ";
        //        errorMessage = String.Concat(errorMessage, theException.Message);
        //        errorMessage = String.Concat(errorMessage, " Line: ");
        //        errorMessage = String.Concat(errorMessage, theException.Source);

        //        MessageBox.Show(errorMessage, "Error");
        //    }
        //    finally
        //    {
        //        MyUtility.Msg.WaitClear();
        //    }
        //    return true;
        //}

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            //return base.OnAsyncDataLoad(e);
            DualResult result = Result.True;

            try
            {
                string sqlcmd = string.Format(@"select a.issuedate, b.poid, b.seq1,b.seq2, sum(b.qty) qty, 
isnull((select stockunit from po_supp_detail where id= b.poid and seq1 = b.seq1 and seq2 = b.seq2),'') as unit
,dbo.getmtldesc(b.poid,b.seq1,b.seq2,2,0) as description
from scrap a inner join scrap_detail b on a.id = b.id
where a.Status = 'Confirmed' and a.type='A' 
and a.issuedate between '{0}' and '{1}'
group by a.issuedate , b.poid, b.seq1,b.Seq2
order by a.issuedate, b.poid, b.seq1,b.seq2
", dateRange1.Text1, dateRange1.Text2);
                result = DBProxy.Current.Select(null, sqlcmd, out dt);
                if (!result) return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
