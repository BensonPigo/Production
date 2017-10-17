using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Thread
{
    public partial class R08 : Sci.Win.Tems.PrintForm
    {
        string sql;
        DataTable printData;
        List<SqlParameter> sqlPar;
        Dictionary<string, string> excelHead = new Dictionary<string, string>();

        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.comboMDivision.setDefalutIndex(true);
        }

        protected override bool ValidateInput()
        {
            excelHead = new Dictionary<string, string>();
            sqlPar = new List<SqlParameter>();
            printData = null;
            sql = @"SELECT 
                      Date = ti.AddDate,
                      IssuNo = ti.ID,
                      RefNo = tid.Refno,
                      Description = li.Description,
                      Type = li.Category,
                      item = li.ThreadTypeID,
                      shade = tid.ThreadColorid,
                      ColorDesc = tc.Description,
                      WofOneCone = li.Weight,
                      AxleWeight = li.AxleWeight,
                      NewCone = tid.NewCone,
                      UsedCone = tid.UsedCone,
                      Location = tid.ThreadLocationid,
					  Remark = tid.Remark
                    FROM ThreadIssue ti WITH (NOLOCK) 
                    inner join ThreadIssue_Detail tid WITH (NOLOCK) on ti.ID = tid.ID
                    left join LocalItem li WITH (NOLOCK) on tid.Refno = li.RefNo
                    left join ThreadColor tc WITH (NOLOCK) on tid.ThreadColorid = tc.id";

            List<string> sqlWhere = new List<string>();

            if (MyUtility.Check.Empty(dateDate.Value1.ToString()) && MyUtility.Check.Empty(dateDate.Value2.ToString()))
            {
                MyUtility.Msg.ErrorBox("Date can not be empty!!");
                return false;
            }
            else
            {
                string date1 = "", date2 = "";
                if (!MyUtility.Check.Empty(dateDate.Value1.ToString()))
                {
                    sqlWhere.Add("@date1 <= Convert(datetime, convert(varchar(10), ti.AddDate, 126))");
                    sqlPar.Add(new SqlParameter("@date1", Convert.ToDateTime(dateDate.Value1).ToString("d")));
                    date1 = Convert.ToDateTime(dateDate.Value1).ToString("d");
                }
                if (!MyUtility.Check.Empty(dateDate.Value2.ToString()))
                {
                    sqlWhere.Add("Convert(datetime, convert(varchar(10), ti.AddDate, 126)) <= @date2");
                    sqlPar.Add(new SqlParameter("@date2", Convert.ToDateTime(dateDate.Value2).ToString("d")));
                    date2 = Convert.ToDateTime(dateDate.Value2).ToString("d");
                }

                excelHead.Add("Date", date1 + " ~ " + date2);
            }

            if (!MyUtility.Check.Empty(txtRefNoStart.Text.ToString()) || !MyUtility.Check.Empty(txtRefNoEnd.Text.ToString()))
            {
                if (!MyUtility.Check.Empty(txtRefNoStart.Text.ToString()))
                {
                    sqlWhere.Add("tid.Refno >= @refno1 ");
                    sqlPar.Add(new SqlParameter("@refno1", txtRefNoStart.Text.ToString()));
                }
                if (!MyUtility.Check.Empty(txtRefNoEnd.Text.ToString()))
                {
                    sqlWhere.Add("tid.Refno <= @refno2 ");
                    sqlPar.Add(new SqlParameter("@refno2", txtRefNoEnd.Text.ToString()));
                }                             
                excelHead.Add("Refno", txtRefNoStart.Text.ToString() + " ~ " + txtRefNoEnd.Text.ToString());
            }

            if (!MyUtility.Check.Empty(txtShade.Text.ToString()))
            {
                sqlWhere.Add("tid.ThreadColorid = @shade");
                sqlPar.Add(new SqlParameter("@shade", txtShade.Text.ToString()));
                excelHead.Add("Shade", txtShade.Text.ToString());
            }

            if (!MyUtility.Check.Empty(txtType.Text.ToString()))
            {
                sqlWhere.Add("li.Category = @type");
                sqlPar.Add(new SqlParameter("@type", txtType.Text.ToString()));
                excelHead.Add("Type", txtType.Text.ToString());
            }

            if (!MyUtility.Check.Empty(txtThreadItem.Text.ToString()))
            {
                sqlWhere.Add("li.ThreadTypeID = @Item");
                sqlPar.Add(new SqlParameter("@Item", txtThreadItem.Text.ToString()));
                excelHead.Add("Item", txtThreadItem.Text.ToString());
            }

            if (!MyUtility.Check.Empty(txtLocationStart.Text.ToString()) || !MyUtility.Check.Empty(txtLocationEnd.Text.ToString()))
            {
                if (!MyUtility.Check.Empty(txtLocationStart.Text.ToString()))
                {
                    sqlWhere.Add("tid.ThreadLocationid >= @loc1 ");
                    sqlPar.Add(new SqlParameter("@loc1", txtLocationStart.Text.ToString()));
                }
                if (!MyUtility.Check.Empty(txtLocationEnd.Text.ToString()))
                {
                     sqlWhere.Add("tid.ThreadLocationid <= @loc2 ");
                     sqlPar.Add(new SqlParameter("@loc2", txtLocationEnd.Text.ToString()));
                }                
                excelHead.Add("Location", txtLocationStart.Text.ToString() + " ~ " + txtLocationEnd.Text.ToString());
            }

            if (!MyUtility.Check.Empty(comboMDivision.Text.ToString()))
            {
                sqlWhere.Add("ti.MDivisionID = @M");
                sqlPar.Add(new SqlParameter("@M", comboMDivision.Text.ToString()));
            }

            if (radioDetail.Checked == true)
            {
                if (sqlWhere.Count > 0)
                    sql += " where ti.Status='Confirmed' and " + sqlWhere.JoinToString(" and ");
                sql += " Order by ti.AddDate, ti.ID, tid.Refno, li.Description, li.Category, li.ThreadTypeID, tid.ThreadColorid, tc.Description, tid.ThreadLocationid, tid.Remark";
            }
            else if (radioSummary.Checked == true)
            {
                sql = @"SELECT 
                          RefNo = tid.Refno,
                          Description = li.Description,
                          Type = li.Category,
                          item = li.ThreadTypeID,
                          shade = tid.ThreadColorid,
                          ColorDesc = tc.Description,
                          NewCone = sum(isnull(tid.NewCone, 0)),
                          UsedCone = sum(isnull(tid.UsedCone, 0))
                        FROM ThreadIssue ti WITH (NOLOCK) 
                        inner join ThreadIssue_Detail tid WITH (NOLOCK) on ti.ID = tid.ID
                        left join LocalItem li WITH (NOLOCK) on tid.Refno = li.RefNo
                        left join ThreadColor tc WITH (NOLOCK) on tid.ThreadColorid = tc.id
                        where ti.Status='Confirmed' and " + sqlWhere.JoinToString(" and ");
                sql += @" Group by tid.Refno, li.Description, li.Category, li.ThreadTypeID, tid.ThreadColorid, tc.Description
                          order by tid.Refno, li.Description, li.Category, li.ThreadTypeID, tid.ThreadColorid, tc.Description";
            }        
           
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result = DBProxy.Current.Select(null, sql, sqlPar, out printData);
            return result;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (printData == null || printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            Excel.Application objApp;
            if (radioDetail.Checked == true)
            {
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R08.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Thread_R08.xltx", 3, showExcel: false, showSaveMsg: false, excelApp: objApp);
            }
            else
            {
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R08_Summary.xltx");
                MyUtility.Excel.CopyToXls(printData, "", "Thread_R08_Summary.xltx", 4, showExcel: false, showSaveMsg: false, excelApp: objApp);
            }



            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];

            if (radioDetail.Checked == true)
            {
                if (excelHead.ContainsKey("Date"))
                    worksheet.Cells[2, 2] = excelHead["Date"];
                if (excelHead.ContainsKey("Refno"))
                    worksheet.Cells[2, 4] = excelHead["Refno"]; ;
                if (excelHead.ContainsKey("Shade"))
                    worksheet.Cells[2, 6] = excelHead["Shade"]; ;
                if (excelHead.ContainsKey("Type"))
                    worksheet.Cells[2, 8] = excelHead["Type"]; ;
                if (excelHead.ContainsKey("Item"))
                    worksheet.Cells[2, 10] = excelHead["Item"]; ;
                if (excelHead.ContainsKey("Location"))
                    worksheet.Cells[2, 12] = excelHead["Location"];
            }
            else
            {
                if (excelHead.ContainsKey("Date"))
                    worksheet.Cells[2, 2] = excelHead["Date"];
                if (excelHead.ContainsKey("Refno"))
                    worksheet.Cells[2, 4] = excelHead["Refno"]; ;
                if (excelHead.ContainsKey("Shade"))
                    worksheet.Cells[2, 6] = excelHead["Shade"]; ;
                if (excelHead.ContainsKey("Type"))
                    worksheet.Cells[3, 2] = excelHead["Type"]; ;
                if (excelHead.ContainsKey("Item"))
                    worksheet.Cells[3, 4] = excelHead["Item"]; ;
                if (excelHead.ContainsKey("Location"))
                    worksheet.Cells[3, 6] = excelHead["Location"];
            }

            worksheet.Columns.AutoFit();
            worksheet.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(radioDetail.Checked == true ? "Thread_R08" : "Thread_R08_Summary");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private void txtShade_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select   distinct
                                        threadcolorid, 
                                        (select tc.Description from dbo.ThreadColor tc WITH (NOLOCK) where tc.id = ThreadStock.ThreadColorID) [Color_desc] 
                               from ThreadStock WITH (NOLOCK) 
                               order by threadcolorid ";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "13, 13", null, "Shade, Color desc");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                txtShade.Text = item.GetSelectedString();
            }
        }

        private void txtType_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    l.Category 
                               from dbo.ThreadStock ts WITH (NOLOCK) 
                               inner join dbo.LocalItem l WITH (NOLOCK) on l.refno = ts.Refno
                               order by l.category";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20", null, "Type");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                txtType.Text = item.GetSelectedString();
            }
        }

        private void txtThreadItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    l.ThreadTypeID
                               from dbo.LocalItem l WITH (NOLOCK) 
                               inner join  dbo.ThreadStock ts WITH (NOLOCK) on l.refno = ts.refno
                               order by l.ThreadTypeID ";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "35", null, "Thread Item");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                txtThreadItem.Text = item.GetSelectedString();
            }
        }

        private void txtRefNoStart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    Refno,
                                    (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                               from dbo.ThreadStock WITH (NOLOCK) 
                               order by Refno";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20, 40", null, "Refno, Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                txtRefNoStart.Text = item.GetSelectedString();
            }
        }

        private void txtRefNoEnd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    Refno,
                                    (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                               from dbo.ThreadStock WITH (NOLOCK) 
                               order by Refno";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20, 40", null, "Refno, Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                txtRefNoEnd.Text = item.GetSelectedString();
            }
        }

        private void txtLocationStart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadIssue_Detail.ThreadLocationID) [Description]
                               from dbo.ThreadIssue_Detail WITH (NOLOCK) 
                               order by ThreadlocationID";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15, 15", null, "Location, Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                txtLocationStart.Text = item.GetSelectedString();
            }
        }

        private void txtLocationEnd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadIssue_Detail.ThreadLocationID) [Description]
                               from dbo.ThreadIssue_Detail WITH (NOLOCK) 
                               order by ThreadlocationID";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15, 15", null, "Location, Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                txtLocationEnd.Text = item.GetSelectedString();
            }
        }
    }
}
