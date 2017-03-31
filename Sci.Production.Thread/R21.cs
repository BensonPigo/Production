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

            RefN1 = textBox1.Text.ToString();
            RefN2 = textBox2.Text.ToString();
            sha = textSHA.Text.ToString();
            TYPE = textTYPE.Text.ToString();
            Thread = textITEM.Text.ToString();
            LOC1 = textLOC1.Text.ToString();
            LOC2 = textLOC2.Text.ToString();
            M = comboMDivision.Text.ToString();
            lis = new List<SqlParameter>();
            string sqlWhere = "";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.textBox1.Text.Empty())
            {
                sqlWheres.Add("ThreadStock.refno between @RefN1 and @RefN2");
                lis.Add(new SqlParameter("@RefN1", RefN1));
                lis.Add(new SqlParameter("@RefN2", RefN2));
            }
            if (!this.textSHA.Text.Empty())
            {
                sqlWheres.Add("ThreadStock.threadcolorid = @sha");
                lis.Add(new SqlParameter("@sha", sha));  
            }
            if (!this.textTYPE.Text.Empty())
            {
                sqlWheres.Add("(select LocalItem.Category from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) = @TYPE");
                lis.Add(new SqlParameter("@TYPE", TYPE));  
            }
            if (!this.textITEM.Text.Empty())
            {
                sqlWheres.Add("ThreadStock.refno = @Thread");
                lis.Add(new SqlParameter("@Thread", Thread));
            }
            if (!this.textLOC1.Text.Empty())
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
            textBox1.Text = item.GetSelectedString();
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
            textBox2.Text = item.GetSelectedString();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            
            if (textBox1.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct Refno,
                                    (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                                       from dbo.ThreadStock WITH (NOLOCK) 
                                       where Refno='{0}'",textBox1.Text),null))
            {
                MyUtility.Msg.WarningBox("Refno is not exist!!", "Data not found");
                e.Cancel = true;
                textBox1.Text = "";
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (textBox2.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct Refno,
                                    (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                                       from dbo.ThreadStock WITH (NOLOCK) 
                                       where Refno='{0}'", textBox2.Text), null))
            {
                MyUtility.Msg.WarningBox("Refno is not exist!!", "Data not found");
                e.Cancel = true;
                textBox2.Text = "";
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
            textSHA.Text = item.GetSelectedString();
        }
        private void textSHA_Validating(object sender, CancelEventArgs e)
        {
            if (textSHA.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct threadcolorid, 
                                        (select tc.Description from dbo.ThreadColor tc where tc.id = ThreadStock.ThreadColorID) [Color_desc] 
                               from ThreadStock WITH (NOLOCK) 
                               where threadcolorid ='{0}'", textSHA.Text), null))
            {
                MyUtility.Msg.WarningBox("Shade is not exist!!", "Data not found");
                e.Cancel = true;
                textSHA.Text = "";
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
            textTYPE.Text = item.GetSelectedString();
        }

        private void textTYPE_Validating(object sender, CancelEventArgs e)
        {
            if (textTYPE.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct l.Category 
                               from dbo.ThreadStock ts WITH (NOLOCK) 
                               inner join dbo.LocalItem l WITH (NOLOCK) on l.refno = ts.Refno
                               where l.category ='{0}'", textTYPE.Text), null))
            {
                MyUtility.Msg.WarningBox("Type is not exist!!", "Data not found");
                e.Cancel = true;
                textTYPE.Text = "";
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
            textITEM.Text = item.GetSelectedString();
        }

        private void textITEM_Validating(object sender, CancelEventArgs e)
        {
            if (textITEM.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct l.Category 
                               from dbo.ThreadStock ts WITH (NOLOCK) 
                               inner join dbo.LocalItem l WITH (NOLOCK) on l.refno = ts.Refno
                               where l.category='{0}'", textITEM.Text), null))
            {
                MyUtility.Msg.WarningBox("Thread Item is not exist!!", "Data not found");
                e.Cancel = true;
                textITEM.Text = "";
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
            textLOC1.Text = item.GetSelectedString();
        }

        private void textLOC1_Validating(object sender, CancelEventArgs e)
        {
            if (textLOC1.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock  WITH (NOLOCK) 
                               where ThreadlocationID='{0}'", textLOC1.Text), null))
            {
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
                e.Cancel = true;
                textLOC1.Text = "";
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
            textLOC2.Text = item.GetSelectedString();
        }

        private void textLOC2_Validating(object sender, CancelEventArgs e)
        {
            if (textLOC2.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"select distinct ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock  WITH (NOLOCK) 
                               where ThreadlocationID='{0}'", textLOC2.Text), null))
            {
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
                e.Cancel = true;
                textLOC2.Text = "";
            }
        }

        
    }
}
