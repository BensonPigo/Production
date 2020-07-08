using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Subcon
{
    public partial class R50 : Sci.Win.Tems.PrintForm
    {
        public R50(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        string FromBundleNO;
        string ToBundleNO;
        DateTime? Date1;
        DateTime? Date2;

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            this.Date1 = this.Date.Value1;
            this.Date2 = this.Date.Value2;
            this.FromBundleNO = this.txtFromBundleNo.Text;
            this.ToBundleNO = this.txtToBundleNo.Text;

            // Factory = txtfactory.Text;
            return base.ValidateInput();
        }

        // 非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            sql.Append(@"
Select 
	o.FtyGroup,o.FactoryID,BD.BundleNo,bd.BundleGroup,o.StyleID,o.SeasonID,o.brandid,bd.Patterncode,bd.PatternDesc,sub.sub,bd.SizeCode,bd.Qty,
	b.poid,b.Colorid,b.Article,b.Cdate,b.Orderid,b.Item  ,b.AddDate,B.AddName,B.EditDate,b.EditName
From Bundle_Detail BD 
Left join bundle b on (bd.Id = b.ID)
Left join Orders O on(b.Orderid = O.id)
outer apply(
	select sub= stuff((
	Select distinct concat('+', bda.SubprocessId)
	from Bundle_Detail_Art bda WITH (NOLOCK) 
	where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.PatternCode=bd.Patterncode
	for xml path('')
	),1,1,'')
) as sub
Where sub.sub like '%PRT%'   
");
            sql2.Append(@"
Select count(*) ct
From Bundle_Detail BD 
Left join bundle b on (bd.Id = b.ID)
Left join Orders O on(b.Orderid = O.id)
outer apply(
	select sub= stuff((
	Select distinct concat('+', bda.SubprocessId)
	from Bundle_Detail_Art bda WITH (NOLOCK) 
	where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.PatternCode=bd.Patterncode
	for xml path('')
	),1,1,'')
) as sub
Where sub.sub like '%PRT%'   
");
            if (!MyUtility.Check.Empty(this.Date1))
            {
                sql.Append(string.Format("And (Convert (date, b.AddDate)  >= '{0}' Or Convert (date, b.EditDate) >= '{0}')", Convert.ToDateTime(this.Date1).ToString("d")));
                sql2.Append(string.Format("And (Convert (date, b.AddDate)  >= '{0}' Or Convert (date, b.EditDate) >= '{0}')", Convert.ToDateTime(this.Date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.Date2))
            {
                sql.Append(string.Format("And (Convert (date, b.AddDate)  <= '{0}' Or Convert (date, b.EditDate) <= '{0}')", Convert.ToDateTime(this.Date2).ToString("d")));
                sql2.Append(string.Format("And (Convert (date, b.AddDate)  <= '{0}' Or Convert (date, b.EditDate) <= '{0}')", Convert.ToDateTime(this.Date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.FromBundleNO))
            {
                sql.Append(string.Format("And BD.BundleNo >= '{0}'", this.FromBundleNO));
                sql2.Append(string.Format("And BD.BundleNo >= '{0}'", this.FromBundleNO));
            }

            if (!MyUtility.Check.Empty(this.ToBundleNO))
            {
                sql.Append(string.Format("And BD.BundleNo <= '{0}'", this.ToBundleNO));
                sql2.Append(string.Format("And BD.BundleNo <= '{0}'", this.ToBundleNO));
            }

            // if (!MyUtility.Check.Empty(Factory))
            // {
            //    sql.Append(string.Format("And O.FtyGroup = '{0}'", Factory));
            //    sql2.Append(string.Format("And O.FtyGroup = '{0}'", Factory));
            // }
            sql.Append(" order by o.FtyGroup,o.FactoryID,BD.BundleNo,bd.BundleGroup,o.StyleID,o.SeasonID,o.brandid");

            // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R50.xltx");

            DataRow dr;
            MyUtility.Check.Seek(sql2.ToString(), out dr, null);
            int ct = MyUtility.Convert.GetInt(dr[0]);
            if (ct == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }

            int num = 200000;
            int Cpage = ct / num;
            for (int i = 0; i < Cpage; i++)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1];
                worksheet1.Copy(worksheetn);
                Marshal.ReleaseComObject(worksheet1);
                Marshal.ReleaseComObject(worksheetn);
            }

            SqlConnection sqlConnection = null;
            DBProxy.Current.OpenConnection(null, out sqlConnection);
            int c = 1;
            using (var cn = new SqlConnection(sqlConnection.ConnectionString))
            using (var cm = cn.CreateCommand())
            {
                cm.CommandText = sql.ToString();
                var adp = new System.Data.SqlClient.SqlDataAdapter(cm);
                var cnt = 0;
                var start = 0;
                using (var ds = new DataSet())
                {
                    while ((cnt = adp.Fill(ds, start, num, "Bundle_Detail")) > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("load {0} records", cnt);
                        start += num;

                        // do some jobs
                        Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[c];
                        MyUtility.Excel.CopyToXls(ds.Tables[0], string.Empty, "Subcon_R50.xltx", 2, false, null, objApp, wSheet: objSheets);
                        Excel.Worksheet worksheet = objApp.Sheets[c];
                        if (!MyUtility.Check.Empty(this.Date1))
                        {
                            worksheet.Cells[1, 2] = this.Date1.Value.ToShortDateString();
                        }

                        if (!MyUtility.Check.Empty(this.Date2))
                        {
                            worksheet.Cells[1, 4] = this.Date2.Value.ToShortDateString();
                        }

                        worksheet.Cells[1, 6] = MyUtility.GetValue.Lookup(@"SELECT TOP 1 PrintingSuppID FROM [Production].[dbo].SYSTEM");
                        worksheet.Columns.AutoFit();
                        c++;

                        // if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                        ds.Tables[0].Dispose();
                        ds.Tables.Clear();
                        Marshal.ReleaseComObject(objSheets);
                    }
                }
            }

            if (Cpage > 0)
            {
                objApp.ActiveWorkbook.Worksheets[Cpage].Columns.AutoFit(); // 這頁需要重新調整欄寬
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R50");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
