using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Thread
{
    /// <summary>
    /// R21
    /// </summary>
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        private string RefN1; private string RefN2; private string sha; private string TYPE; private string Thread; private string LOC1; private string LOC2;
        private List<SqlParameter> lis;
        private DataTable dt; private string cmd;

        /// <summary>
        /// R21
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.print.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.RefN1 = this.txtRefnoStart.Text.ToString();
            this.RefN2 = this.txtRefnoEnd.Text.ToString();
            this.sha = this.txtShade.Text.ToString();
            this.TYPE = this.txtType.Text.ToString();
            this.Thread = this.txtThreadItem.Text.ToString();
            this.LOC1 = this.txtLocationStart.Text.ToString();
            this.LOC2 = this.txtLocationEnd.Text.ToString();
            this.lis = new List<SqlParameter>();
            string sqlWhere = string.Empty;
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!MyUtility.Check.Empty(this.txtRefnoStart.Text.ToString()) || !MyUtility.Check.Empty(this.txtRefnoEnd.Text.ToString()))
            {
                if (!MyUtility.Check.Empty(this.txtRefnoStart.Text.ToString()))
                {
                    sqlWheres.Add("ThreadStock.refno >= @RefN1");
                    this.lis.Add(new SqlParameter("@RefN1", this.RefN1));
                }

                if (!MyUtility.Check.Empty(this.txtRefnoEnd.Text.ToString()))
                {
                    sqlWheres.Add("ThreadStock.refno <= @RefN2");
                    this.lis.Add(new SqlParameter("@RefN2", this.RefN2));
                }
            }

            if (!this.txtShade.Text.Empty())
            {
                sqlWheres.Add("ThreadStock.threadcolorid = @sha");
                this.lis.Add(new SqlParameter("@sha", this.sha));
            }

            if (!this.txtType.Text.Empty())
            {
                sqlWheres.Add("(select LocalItem.Category from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) = @TYPE");
                this.lis.Add(new SqlParameter("@TYPE", this.TYPE));
            }

            if (!this.txtThreadItem.Text.Empty())
            {
                sqlWheres.Add("(select LocalItem.ThreadTypeID from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) = @Thread");
                this.lis.Add(new SqlParameter("@Thread", this.Thread));
            }

            if (!this.txtLocationStart.Text.Empty() || !this.txtLocationEnd.Text.Empty())
            {
                if (!this.txtLocationStart.Text.Empty())
                {
                    sqlWheres.Add("ThreadStock.threadlocationid >= @LOC1");
                    this.lis.Add(new SqlParameter("@LOC1", this.LOC1));
                }

                if (!this.txtLocationEnd.Text.Empty())
                {
                    sqlWheres.Add("ThreadStock.threadlocationid <= @LOC2");
                    this.lis.Add(new SqlParameter("@LOC2", this.LOC2));
                }
            }

            if (sqlWheres.Count > 0)
            {
                sqlWhere = "and " + string.Join(" and ", sqlWheres);
            }
            #endregion

            this.cmd = string.Format(@"
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

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.dt);
            if (!res)
            {
                return res;
            }

            return res;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt.Rows.Count);

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R21.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Thread_R21.xltx", 2, showExcel: false, showSaveMsg: false, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns.AutoFit();
            worksheet.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Thread_R21");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private void TxtRefnoStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = @"select distinct 
                                    Refno,
                                    (select LocalItem.Description from dbo.LocalItem WITH (NOLOCK) where refno= ThreadStock.Refno) [Description]
                               from dbo.ThreadStock  WITH (NOLOCK) 
                               order by Refno";
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20, 40", null, "Refno, Description");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtRefnoStart.Text = item.GetSelectedString();
        }

        private void TxtRefnoEnd_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
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

            this.txtRefnoEnd.Text = item.GetSelectedString();
        }

        private void TxtRefnoStart_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtRefnoStart.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                    @"
select distinct Refno
	   , [Description] = (select LocalItem.Description 
	   					  from dbo.LocalItem WITH (NOLOCK) 
	   					  where refno= ThreadStock.Refno)
from dbo.ThreadStock WITH (NOLOCK) 
where Refno = '{0}'",
                    this.txtRefnoStart.Text),
                null))
            {
                e.Cancel = true;
                this.txtRefnoStart.Text = string.Empty;
                MyUtility.Msg.WarningBox("Refno is not exist!!", "Data not found");
            }
        }

        private void TxtRefnoEnd_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtRefnoEnd.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                    @"
select distinct Refno
	   , [Description] = (select LocalItem.Description 
	   					  from dbo.LocalItem WITH (NOLOCK) 
	   					  where refno = ThreadStock.Refno)
from dbo.ThreadStock WITH (NOLOCK) 
where Refno = '{0}'",
                    this.txtRefnoEnd.Text),
                null))
            {
                e.Cancel = true;
                this.txtRefnoEnd.Text = string.Empty;
                MyUtility.Msg.WarningBox("Refno is not exist!!", "Data not found");
            }
        }

        private void TxtShade_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = @"select   distinct
                                        threadcolorid, 
                                        (select tc.Description from dbo.ThreadColor tc where tc.id = ThreadStock.ThreadColorID) [Color_desc] 
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

        private void TxtShade_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtShade.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                    @"
select distinct threadcolorid
	   , [Color_desc] = (select tc.Description 
	   					 from dbo.ThreadColor tc 
	   					 where tc.id = ThreadStock.ThreadColorID)
from ThreadStock WITH (NOLOCK) 
where threadcolorid ='{0}'",
                    this.txtShade.Text),
                null))
            {
                e.Cancel = true;
                this.txtShade.Text = string.Empty;
                MyUtility.Msg.WarningBox("Shade is not exist!!", "Data not found");
            }
        }

        private void TxtType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
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

        private void TxtType_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtType.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                    @"
select distinct l.Category 
from dbo.ThreadStock ts WITH (NOLOCK) 
inner join dbo.LocalItem l WITH (NOLOCK) on l.refno = ts.Refno
where l.category ='{0}'",
                    this.txtType.Text),
                null))
            {
                e.Cancel = true;
                this.txtType.Text = string.Empty;
                MyUtility.Msg.WarningBox("Type is not exist!!", "Data not found");
            }
        }

        private void TxtThreadItem_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
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

        private void TxtThreadItem_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtThreadItem.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                    @"
select distinct l.ThreadTypeID 
from dbo.ThreadStock ts WITH (NOLOCK) 
inner join dbo.LocalItem l WITH (NOLOCK) on l.refno = ts.Refno
where l.ThreadTypeID='{0}'",
                    this.txtThreadItem.Text),
                null))
            {
                e.Cancel = true;
                this.txtThreadItem.Text = string.Empty;
                MyUtility.Msg.WarningBox("Thread Item is not exist!!", "Data not found");
            }
        }

        private void TxtLocationStart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = @"select distinct 
                                    ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock  WITH (NOLOCK) 
                               order by ThreadlocationID";
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15, 15", null, "Location, Description");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtLocationStart.Text = item.GetSelectedString();
        }

        private void TxtLocationStart_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtLocationStart.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                    @"
select distinct ThreadlocationID
	   , [Description] = (select distinct Description 
	   					  from dbo.ThreadLocation WITH (NOLOCK) 
	   					  where ThreadLocation.ID = ThreadStock.ThreadLocationID)
from dbo.ThreadStock  WITH (NOLOCK) 
where ThreadlocationID='{0}'",
                    this.txtLocationStart.Text),
                null))
            {
                e.Cancel = true;
                this.txtLocationStart.Text = string.Empty;
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
            }
        }

        private void TxtLocationEnd_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = @"select distinct 
                                    ThreadlocationID,
                                    (select distinct Description from dbo.ThreadLocation WITH (NOLOCK) where ThreadLocation.ID = ThreadStock.ThreadLocationID) [Description]
                               from dbo.ThreadStock WITH (NOLOCK) 
                               order by ThreadlocationID";
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "15, 15", null, "Location, Description");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtLocationEnd.Text = item.GetSelectedString();
        }

        private void TxtLocationEnd_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtLocationEnd.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                    @"
select distinct ThreadlocationID
	   , [Description] = (select distinct Description 
	   					  from dbo.ThreadLocation WITH (NOLOCK) 
	   					  where ThreadLocation.ID = ThreadStock.ThreadLocationID)
from dbo.ThreadStock  WITH (NOLOCK) 
where ThreadlocationID='{0}'",
                    this.txtLocationEnd.Text),
                null))
            {
                e.Cancel = true;
                this.txtLocationEnd.Text = string.Empty;
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
            }
        }
    }
}
