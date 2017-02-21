using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            DataTable m = null;
            string sqlm = (@"select ID FROM DBO.MDivision");
            DBProxy.Current.Select("", sqlm, out m);
            m.Rows.Add(new string[] { "" });
            m.DefaultView.Sort = "ID";
            this.comboBox1.DataSource = m;
            this.comboBox1.ValueMember = "ID";
            this.comboBox1.DisplayMember = "ID";
            this.comboBox1.SelectedIndex = 0;
            this.comboBox1.Text = Sci.Env.User.Keyword;
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
            M = comboBox1.SelectedValue.ToString();
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
                sqlWheres.Add("(select LocalItem.Category from dbo.LocalItem where refno= ThreadStock.Refno) = @TYPE");
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
            if (!this.comboBox1.Text.Empty())
            {
                sqlWheres.Add("ThreadStock.mDivisionid = @M");
                lis.Add(new SqlParameter("@M", M));
            }
            
            if (sqlWheres.Count > 0)
                sqlWheres.JoinToString(" and ");
            #endregion

            cmd = string.Format(@"
             select Refno
                    ,(select LocalItem.Description from dbo.LocalItem where refno= ThreadStock.Refno) [Description]
                    ,(select LocalItem.Category from dbo.LocalItem where refno= ThreadStock.Refno) [Category]
                    ,(select LocalItem.ThreadTypeID from dbo.LocalItem where refno= ThreadStock.Refno) [ThreadTypeID]
                    ,ThreadColorID
                    ,(select tc.Description from dbo.ThreadColor tc where tc.id = ThreadStock.ThreadColorID) [Color_desc]
                    ,(select LocalItem.Weight from dbo.LocalItem where refno= ThreadStock.Refno) [Weight]
                    ,(select LocalItem.AxleWeight from dbo.LocalItem where refno= ThreadStock.Refno) [AxleWeight]
                    ,NewCone
                    ,UsedCone
                    ,ThreadLocationID
             from dbo.ThreadStock
             where isnull(NewCone+UsedCone,0)>0" + sqlWhere);

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
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Thread_R21.xltx");
            xl.dicDatas.Add("##TSL", dt);
            xl.Save(outpath, false);
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
                                    (select distinct Description from dbo.ThreadLocation where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock 
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
                                    (select distinct Description from dbo.ThreadLocation where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock 
                               order by ThreadlocationID";
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15, 15", null, "Location, Description");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                textLOC2.Text = item.GetSelectedString();
            }
        }
    }
}
