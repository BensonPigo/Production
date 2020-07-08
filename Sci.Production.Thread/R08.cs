using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Thread
{
    /// <summary>
    /// R08
    /// </summary>
    public partial class R08 : Sci.Win.Tems.PrintForm
    {
        private string sql;
        private DataTable printData;
        private List<SqlParameter> sqlPar;
        private Dictionary<string, string> excelHead = new Dictionary<string, string>();

        /// <summary>
        /// R08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision.setDefalutIndex(true);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.excelHead = new Dictionary<string, string>();
            this.sqlPar = new List<SqlParameter>();
            this.printData = null;
            this.sql = @"SELECT 
                      Date = ti.AddDate,
                      IssuNo = ti.ID,
                      SPNo = ti.RequestID,
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
					  Remark = tid.Remark,
	                  HeaderRemark = ti.Remark 
                    FROM ThreadIssue ti WITH (NOLOCK) 
                    inner join ThreadIssue_Detail tid WITH (NOLOCK) on ti.ID = tid.ID
                    left join LocalItem li WITH (NOLOCK) on tid.Refno = li.RefNo
                    left join ThreadColor tc WITH (NOLOCK) on tid.ThreadColorid = tc.id";

            List<string> sqlWhere = new List<string>();

            if (MyUtility.Check.Empty(this.dateDate.Value1.ToString()) && MyUtility.Check.Empty(this.dateDate.Value2.ToString()))
            {
                MyUtility.Msg.ErrorBox("Date can not be empty!!");
                return false;
            }
            else
            {
                string date1 = string.Empty, date2 = string.Empty;
                if (!MyUtility.Check.Empty(this.dateDate.Value1.ToString()))
                {
                    sqlWhere.Add("@date1 <= Convert(datetime, convert(varchar(10), ti.AddDate, 126))");
                    this.sqlPar.Add(new SqlParameter("@date1", Convert.ToDateTime(this.dateDate.Value1).ToString("d")));
                    date1 = Convert.ToDateTime(this.dateDate.Value1).ToString("d");
                }

                if (!MyUtility.Check.Empty(this.dateDate.Value2.ToString()))
                {
                    sqlWhere.Add("Convert(datetime, convert(varchar(10), ti.AddDate, 126)) <= @date2");
                    this.sqlPar.Add(new SqlParameter("@date2", Convert.ToDateTime(this.dateDate.Value2).ToString("d")));
                    date2 = Convert.ToDateTime(this.dateDate.Value2).ToString("d");
                }

                this.excelHead.Add("Date", date1 + " ~ " + date2);
            }

            if (!MyUtility.Check.Empty(this.txtRefNoStart.Text.ToString()) || !MyUtility.Check.Empty(this.txtRefNoEnd.Text.ToString()))
            {
                if (!MyUtility.Check.Empty(this.txtRefNoStart.Text.ToString()))
                {
                    sqlWhere.Add("tid.Refno >= @refno1 ");
                    this.sqlPar.Add(new SqlParameter("@refno1", this.txtRefNoStart.Text.ToString()));
                }

                if (!MyUtility.Check.Empty(this.txtRefNoEnd.Text.ToString()))
                {
                    sqlWhere.Add("tid.Refno <= @refno2 ");
                    this.sqlPar.Add(new SqlParameter("@refno2", this.txtRefNoEnd.Text.ToString()));
                }

                this.excelHead.Add("Refno", this.txtRefNoStart.Text.ToString() + " ~ " + this.txtRefNoEnd.Text.ToString());
            }

            if (!MyUtility.Check.Empty(this.txtShade.Text.ToString()))
            {
                sqlWhere.Add("tid.ThreadColorid = @shade");
                this.sqlPar.Add(new SqlParameter("@shade", this.txtShade.Text.ToString()));
                this.excelHead.Add("Shade", this.txtShade.Text.ToString());
            }

            if (!MyUtility.Check.Empty(this.txtType.Text.ToString()))
            {
                sqlWhere.Add("li.Category = @type");
                this.sqlPar.Add(new SqlParameter("@type", this.txtType.Text.ToString()));
                this.excelHead.Add("Type", this.txtType.Text.ToString());
            }

            if (!MyUtility.Check.Empty(this.txtThreadItem.Text.ToString()))
            {
                sqlWhere.Add("li.ThreadTypeID = @Item");
                this.sqlPar.Add(new SqlParameter("@Item", this.txtThreadItem.Text.ToString()));
                this.excelHead.Add("Item", this.txtThreadItem.Text.ToString());
            }

            if (!MyUtility.Check.Empty(this.txtLocationStart.Text.ToString()) || !MyUtility.Check.Empty(this.txtLocationEnd.Text.ToString()))
            {
                if (!MyUtility.Check.Empty(this.txtLocationStart.Text.ToString()))
                {
                    sqlWhere.Add("tid.ThreadLocationid >= @loc1 ");
                    this.sqlPar.Add(new SqlParameter("@loc1", this.txtLocationStart.Text.ToString()));
                }

                if (!MyUtility.Check.Empty(this.txtLocationEnd.Text.ToString()))
                {
                     sqlWhere.Add("tid.ThreadLocationid <= @loc2 ");
                     this.sqlPar.Add(new SqlParameter("@loc2", this.txtLocationEnd.Text.ToString()));
                }

                this.excelHead.Add("Location", this.txtLocationStart.Text.ToString() + " ~ " + this.txtLocationEnd.Text.ToString());
            }

            if (!MyUtility.Check.Empty(this.comboMDivision.Text.ToString()))
            {
                sqlWhere.Add("ti.MDivisionID = @M");
                this.sqlPar.Add(new SqlParameter("@M", this.comboMDivision.Text.ToString()));
            }

            if (this.radioDetail.Checked == true)
            {
                if (sqlWhere.Count > 0)
                {
                    this.sql += " where ti.Status='Confirmed' and " + sqlWhere.JoinToString(" and ");
                }

                this.sql += " Order by ti.AddDate, ti.ID, tid.Refno, li.Description, li.Category, li.ThreadTypeID, tid.ThreadColorid, tc.Description, tid.ThreadLocationid, tid.Remark";
            }
            else if (this.radioSummary.Checked == true)
            {
                this.sql = @"SELECT 
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
                this.sql += @" Group by tid.Refno, li.Description, li.Category, li.ThreadTypeID, tid.ThreadColorid, tc.Description
                          order by tid.Refno, li.Description, li.Category, li.ThreadTypeID, tid.ThreadColorid, tc.Description";
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result = DBProxy.Current.Select(null, this.sql, this.sqlPar, out this.printData);
            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData == null || this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            Excel.Application objApp;
            if (this.radioDetail.Checked == true)
            {
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R08.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Thread_R08.xltx", 3, showExcel: false, showSaveMsg: false, excelApp: objApp);
            }
            else
            {
                objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R08_Summary.xltx");
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Thread_R08_Summary.xltx", 4, showExcel: false, showSaveMsg: false, excelApp: objApp);
            }

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];

            if (this.radioDetail.Checked == true)
            {
                if (this.excelHead.ContainsKey("Date"))
                {
                    worksheet.Cells[2, 2] = this.excelHead["Date"];
                }

                if (this.excelHead.ContainsKey("Refno"))
                {
                    worksheet.Cells[2, 4] = this.excelHead["Refno"];
                }

                if (this.excelHead.ContainsKey("Shade"))
                {
                    worksheet.Cells[2, 6] = this.excelHead["Shade"];
                }

                if (this.excelHead.ContainsKey("Type"))
                {
                    worksheet.Cells[2, 8] = this.excelHead["Type"];
                }

                if (this.excelHead.ContainsKey("Item"))
                {
                    worksheet.Cells[2, 10] = this.excelHead["Item"];
                }

                if (this.excelHead.ContainsKey("Location"))
                {
                    worksheet.Cells[2, 12] = this.excelHead["Location"];
                }
            }
            else
            {
                if (this.excelHead.ContainsKey("Date"))
                {
                    worksheet.Cells[2, 2] = this.excelHead["Date"];
                }

                if (this.excelHead.ContainsKey("Refno"))
                {
                    worksheet.Cells[2, 4] = this.excelHead["Refno"];
                }

                if (this.excelHead.ContainsKey("Shade"))
                {
                    worksheet.Cells[2, 6] = this.excelHead["Shade"];
                }

                if (this.excelHead.ContainsKey("Type"))
                {
                    worksheet.Cells[3, 2] = this.excelHead["Type"];
                }

                if (this.excelHead.ContainsKey("Item"))
                {
                    worksheet.Cells[3, 4] = this.excelHead["Item"];
                }

                if (this.excelHead.ContainsKey("Location"))
                {
                    worksheet.Cells[3, 6] = this.excelHead["Location"];
                }
            }

            worksheet.Columns.AutoFit();
            worksheet.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(this.radioDetail.Checked == true ? "Thread_R08" : "Thread_R08_Summary");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private void TxtShade_MouseDown(object sender, MouseEventArgs e)
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
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtShade.Text = item.GetSelectedString();
            }
        }

        private void TxtType_MouseDown(object sender, MouseEventArgs e)
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
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtType.Text = item.GetSelectedString();
            }
        }

        private void TxtThreadItem_MouseDown(object sender, MouseEventArgs e)
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
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtThreadItem.Text = item.GetSelectedString();
            }
        }

        private void TxtRefNoStart_MouseDown(object sender, MouseEventArgs e)
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
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtRefNoStart.Text = item.GetSelectedString();
            }
        }

        private void TxtRefNoEnd_MouseDown(object sender, MouseEventArgs e)
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
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtRefNoEnd.Text = item.GetSelectedString();
            }
        }

        private void TxtLocationStart_MouseDown(object sender, MouseEventArgs e)
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
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtLocationStart.Text = item.GetSelectedString();
            }
        }

        private void TxtLocationEnd_MouseDown(object sender, MouseEventArgs e)
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
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtLocationEnd.Text = item.GetSelectedString();
            }
        }
    }
}
