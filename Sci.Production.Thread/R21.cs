using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Thread
{
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        string RefN1; string RefN2; string sha; string TYPE; string Thread; string LOC1; string LOC2; string M;
        List<SqlParameter> lis;
        DataTable dt; string cmd;
        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.comboMDivision.setDefalutIndex(true);
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {

            RefN1 = txtRefnoStart.Text.ToString();
            RefN2 = txtRefnoEnd.Text.ToString();
            sha = txtShade.Text.ToString();
            TYPE = txtType.Text.ToString();
            Thread = txtThreadItem.Text.ToString();
            LOC1 = txtLocationStart.Text.ToString();
            LOC2 = txtLocationEnd.Text.ToString();
            M = comboMDivision.Text.ToString();
            lis = new List<SqlParameter>();
            string sqlWhere = "";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.txtRefnoStart.Text.Empty())
            {
                sqlWheres.Add("ThreadStock.refno between @RefN1 and @RefN2");
                lis.Add(new SqlParameter("@RefN1", RefN1));
                lis.Add(new SqlParameter("@RefN2", RefN2));
            }
            if (!this.txtShade.Text.Empty())
            {
                sqlWheres.Add("ThreadStock.threadcolorid = @sha");
                lis.Add(new SqlParameter("@sha", sha));  
            }
            if (!this.txtType.Text.Empty())
            {
                sqlWheres.Add("(select LocalItem.Category from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) = @TYPE");
                lis.Add(new SqlParameter("@TYPE", TYPE));  
            }
            if (!this.txtThreadItem.Text.Empty())
            {
                sqlWheres.Add("ThreadStock.refno = @Thread");
                lis.Add(new SqlParameter("@Thread", Thread));
            }
            if (!this.txtLocationStart.Text.Empty())
            {
                sqlWheres.Add("ThreadStock.threadlocationid between @LOC1 and @LOC2");
                lis.Add(new SqlParameter("@LOC1", LOC1));
                lis.Add(new SqlParameter("@LOC2", LOC2));
            }
            if (!this.M.Empty())
            {
                sqlWheres.Add("ThreadStock.mDivisionid = @M");
                lis.Add(new SqlParameter("@M", M));
            }
            
            if (sqlWheres.Count > 0)
                sqlWhere = "and " + string.Join(" and ", sqlWheres);
            #endregion

            cmd = string.Format(@"
             select Refno
                    ,(select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                    ,(select LocalItem.Category from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Category]
                    ,(select LocalItem.ThreadTypeID from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [ThreadTypeID]
                    ,ThreadColorID
                    ,(select tc.Description from dbo.ThreadColor tc WITH (NOLOCK) where tc.id = ThreadStock.ThreadColorID) [Color_desc]
                    ,(select LocalItem.Weight from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Weight]
                    ,(select LocalItem.AxleWeight from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [AxleWeight]
                    ,NewCone
                    ,UsedCone
                    ,ThreadLocationID
             from dbo.ThreadStock WITH (NOLOCK) 
             where isnull(NewCone+UsedCone,0)>0  " + sqlWhere);

            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select("", cmd, lis, out dt);
            if (!res)
            {
                return res;
            }
            return res;
        }
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            SetCount(dt.Rows.Count);

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R21.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(dt, "", "Thread_R21.xltx", 2, showExcel: false, showSaveMsg: false, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns.AutoFit();
            worksheet.Rows.AutoFit();
            objApp.Visible = true;

            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放worksheet

            this.HideWaitMessage();
            return true;
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = @"select distinct 
                                    Refno,
                                    (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                               from dbo.ThreadStock  WITH (NOLOCK) 
                               order by Refno";
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20, 40", null, "Refno, Description");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtRefnoStart.Text = item.GetSelectedString();
        }

        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = @"select distinct 
                                    Refno,
                                    (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                               from dbo.ThreadStock WITH (NOLOCK) 
                               order by Refno";
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20, 40", null, "Refno, Description");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtRefnoEnd.Text = item.GetSelectedString();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            
            if (txtRefnoStart.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct Refno,
                                    (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                                       from dbo.ThreadStock WITH (NOLOCK) 
                                       where Refno='{0}'",txtRefnoStart.Text),null))
            {
                MyUtility.Msg.WarningBox("Refno is not exist!!", "Data not found");
                e.Cancel = true;
                txtRefnoStart.Text = "";
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (txtRefnoEnd.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct Refno,
                                    (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                                       from dbo.ThreadStock WITH (NOLOCK) 
                                       where Refno='{0}'", txtRefnoEnd.Text), null))
            {
                MyUtility.Msg.WarningBox("Refno is not exist!!", "Data not found");
                e.Cancel = true;
                txtRefnoEnd.Text = "";
            }
        }

        private void textSHA_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = @"select   distinct
                                        threadcolorid, 
                                        (select tc.Description from dbo.ThreadColor tc where tc.id = ThreadStock.ThreadColorID) [Color_desc] 
                               from ThreadStock WITH (NOLOCK) 
                               order by threadcolorid ";
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "13, 13", null, "Shade, Color desc");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtShade.Text = item.GetSelectedString();
        }
        private void textSHA_Validating(object sender, CancelEventArgs e)
        {
            if (txtShade.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct threadcolorid, 
                                        (select tc.Description from dbo.ThreadColor tc where tc.id = ThreadStock.ThreadColorID) [Color_desc] 
                               from ThreadStock WITH (NOLOCK) 
                               where threadcolorid ='{0}'", txtShade.Text), null))
            {
                MyUtility.Msg.WarningBox("Shade is not exist!!", "Data not found");
                e.Cancel = true;
                txtShade.Text = "";
            }
        }
        private void textTYPE_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
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

        private void textTYPE_Validating(object sender, CancelEventArgs e)
        {
            if (txtType.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct l.Category 
                               from dbo.ThreadStock ts WITH (NOLOCK) 
                               inner join dbo.LocalItem l WITH (NOLOCK) on l.refno = ts.Refno
                               where l.category ='{0}'", txtType.Text), null))
            {
                MyUtility.Msg.WarningBox("Type is not exist!!", "Data not found");
                e.Cancel = true;
                txtType.Text = "";
            }
        }

        private void textITEM_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
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

        private void textITEM_Validating(object sender, CancelEventArgs e)
        {
            if (txtThreadItem.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct l.Category 
                               from dbo.ThreadStock ts WITH (NOLOCK) 
                               inner join dbo.LocalItem l WITH (NOLOCK) on l.refno = ts.Refno
                               where l.category='{0}'", txtThreadItem.Text), null))
            {
                MyUtility.Msg.WarningBox("Thread Item is not exist!!", "Data not found");
                e.Cancel = true;
                txtThreadItem.Text = "";
            }
        }

        private void textLOC1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = @"select distinct 
                                    ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock  WITH (NOLOCK) 
                               order by ThreadlocationID";
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15, 15", null, "Location, Description");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtLocationStart.Text = item.GetSelectedString();
        }

        private void textLOC1_Validating(object sender, CancelEventArgs e)
        {
            if (txtLocationStart.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock  WITH (NOLOCK) 
                               where ThreadlocationID='{0}'", txtLocationStart.Text), null))
            {
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
                e.Cancel = true;
                txtLocationStart.Text = "";
            }
        }

        private void textLOC2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = @"select distinct 
                                    ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock WITH (NOLOCK) 
                               order by ThreadlocationID";
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15, 15", null, "Location, Description");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtLocationEnd.Text = item.GetSelectedString();
        }

        private void textLOC2_Validating(object sender, CancelEventArgs e)
        {
            if (txtLocationEnd.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock  WITH (NOLOCK) 
                               where ThreadlocationID='{0}'", txtLocationEnd.Text), null))
            {
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
                e.Cancel = true;
                txtLocationEnd.Text = "";
            }
        }

        
    }
}
