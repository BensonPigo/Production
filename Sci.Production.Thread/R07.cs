using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Thread
{
    public partial class R07 : Sci.Win.Tems.PrintForm
    {
        string sql;
        DataTable printData;
        List<SqlParameter> sqlPar;
        
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            sqlPar = new List<SqlParameter>();
            printData = null;
            sql = @"SELECT 
                      Date = ti.AddDate,
                      IncomingNo = ti.ID,
                      RefNo = tid.Refno,
                      Description = li.Description,
                      Type = li.Category,
                      item = li.ThreadTypeID,
                      shade = tid.ThreadColorid,
                      ColorDesc = tc.Description,
                      NofPcs = tid.PcsUsed,
                      TtlUsed = tid.TotalWeight,
                      WofOneCone = li.Weight,
                      AxleWeight = li.AxleWeight,
                      NewCone = tid.NewCone,
                      UsedCone = tid.UsedCone,
                      Location = tid.ThreadLocationid
                    FROM Threadincoming ti
                    inner join ThreadIncoming_Detail tid on ti.ID = tid.ID
                    left join LocalItem li on tid.Refno = li.RefNo
                    left join ThreadColor tc on tid.ThreadColorid = tc.id";

            List<string> sqlWhere = new List<string>();

            if (MyUtility.Check.Empty(dateRange1.Value1.ToString()) && MyUtility.Check.Empty(dateRange1.Value2.ToString()))
            {
                MyUtility.Msg.ErrorBox("Date can not be empty!!");
                return false;
            }
            else
            {
                sqlWhere.Add("(ti.AddDate between @date1 and @date2)");
                sqlPar.Add(new SqlParameter("@date1", dateRange1.Text1.ToString()));
                sqlPar.Add(new SqlParameter("@date2", dateRange1.Text2.ToString()));
            }

            if (!MyUtility.Check.Empty(textBox1.Text.ToString()) && !MyUtility.Check.Empty(textBox2.Text.ToString()))
            {
                sqlWhere.Add("(tid.Refno between @refno1 and @refno2)");
                sqlPar.Add(new SqlParameter("@refno1", textBox1.Text.ToString()));
                sqlPar.Add(new SqlParameter("@refno2", textBox2.Text.ToString()));
            }

            if (!MyUtility.Check.Empty(textSHA.Text.ToString()))
            {
                sqlWhere.Add("li.ThreadTypeID = @shade");
                sqlPar.Add(new SqlParameter("@shade", textSHA.Text.ToString()));
            }

            if (!MyUtility.Check.Empty(textTYPE.Text.ToString()))
            {
                sqlWhere.Add("li.Category = @type");
                sqlPar.Add(new SqlParameter("@type", textTYPE.Text.ToString()));
            }

            if (!MyUtility.Check.Empty(textLOC1.Text.ToString()) && !MyUtility.Check.Empty(textLOC2.Text.ToString()))
            {
                sqlWhere.Add("(tid.ThreadLocationid between @loc1 and @loc2)");
                sqlPar.Add(new SqlParameter("@loc1", textLOC1.Text.ToString()));
                sqlPar.Add(new SqlParameter("@loc2", textLOC2.Text.ToString()));
            }

            if (sqlWhere.Count > 0)
                sql += " where " + sqlWhere.JoinToString(" and ");

            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result = DBProxy.Current.Select(null, sql, sqlPar, out printData);
            return result;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R07.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Thread_R07.xltx", 3, showExcel: false, showSaveMsg: false, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            //for (int i = 1; i <= printData.Rows.Count; i++)
            //{
            //    string str = worksheet.Cells[i + 1, 4].Value;
            //    if (!MyUtility.Check.Empty(str))
            //        worksheet.Cells[i + 1, 4] = str.Trim();

            //}

            worksheet.Columns.AutoFit();
            worksheet.Rows.AutoFit();
            objApp.Visible = true;

            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放worksheet

            this.HideWaitMessage();
            return true;
        }

        private void textSHA_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select   distinct
                                        threadcolorid, 
                                        (select tc.Description from dbo.ThreadColor tc where tc.id = ThreadStock.ThreadColorID) [Color_desc] 
                               from ThreadStock 
                               order by threadcolorid ";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "13, 13", null, "Shade, Color desc");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                textSHA.Text = item.GetSelectedString();
            }
        }

        private void textTYPE_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    l.Category 
                               from dbo.ThreadStock ts
                               inner join dbo.LocalItem l on l.refno = ts.Refno
                               order by l.category";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20", null, "Type");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                textTYPE.Text = item.GetSelectedString();
            }
        }

        private void textITEM_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    l.ThreadTypeID
                               from dbo.LocalItem l
                               inner join  dbo.ThreadStock ts on l.refno = ts.refno
                               order by l.ThreadTypeID ";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "35", null, "Thread Item");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                textITEM.Text = item.GetSelectedString();
            }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    Refno,
                                    (select LocalItem.Description from dbo.LocalItem where refno= ThreadStock.Refno) [Description]
                               from dbo.ThreadStock 
                               order by Refno";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20, 40", null, "Refno, Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                textBox1.Text = item.GetSelectedString();
            }
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    Refno,
                                    (select LocalItem.Description from dbo.LocalItem where refno= ThreadStock.Refno) [Description]
                               from dbo.ThreadStock 
                               order by Refno";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20, 40", null, "Refno, Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                textBox2.Text = item.GetSelectedString();
            }
        }

        private void textLOC1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation where ThreadLocation.ID = Threadincoming_Detail.ThreadLocationID) [Description]
                               from dbo.Threadincoming_Detail 
                               order by ThreadlocationID";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15, 15", null, "Location, Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                textLOC1.Text = item.GetSelectedString();
            }
        }

        private void textLOC2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = @"select distinct 
                                    ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation where ThreadLocation.ID = Threadincoming_Detail.ThreadLocationID) [Description]
                               from dbo.Threadincoming_Detail 
                               order by ThreadlocationID";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15, 15", null, "Location, Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                textLOC2.Text = item.GetSelectedString();
            }
        }
    }
}
