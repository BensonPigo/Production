using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;

namespace Sci.Production.Subcon
{
    public partial class R50 : Sci.Win.Tems.PrintForm
    {
        public R50(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        string Factory, BundleNO;
        DateTime? Date1, Date2;
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            Date1 = Date.Value1;
            Date2 = Date.Value2;
            BundleNO = txtBundleNo.Text;
            Factory = txtfactory.Text;

            return base.ValidateInput();
        }

        //非同步讀取資料
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
	o.FactoryID,BD.BundleNo,bd.BundleGroup,o.StyleID,o.SeasonID,o.brandid,bd.Patterncode,bd.PatternDesc,bd.SizeCode,bd.Qty,
	b.poid,b.Colorid,b.Article,b.Cdate,b.Orderid,b.Item ,b.AddDate,B.AddName,B.EditDate,b.EditName
From Bundle_Detail BD 
Left join bundle b on (bd.Id = b.ID)
Left join Orders O on(b.Orderid = O.id)
Where 1=1
");
            sql2.Append(@"
Select count(*) ct
From Bundle_Detail BD 
Left join bundle b on (bd.Id = b.ID)
Left join Orders O on(b.Orderid = O.id)
Where 1=1
");
            if (!MyUtility.Check.Empty(Date1))
            {
                sql.Append(string.Format("And (Convert (date, b.AddDate)  >= '{0}' Or Convert (date, b.EditDate) >= '{0}')", Convert.ToDateTime(Date1).ToString("d")));
                sql2.Append(string.Format("And (Convert (date, b.AddDate)  >= '{0}' Or Convert (date, b.EditDate) >= '{0}')", Convert.ToDateTime(Date1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(Date2))
            {
                sql.Append(string.Format("And (Convert (date, b.AddDate)  <= '{0}' Or Convert (date, b.EditDate) <= '{0}')", Convert.ToDateTime(Date2).ToString("d")));
                sql2.Append(string.Format("And (Convert (date, b.AddDate)  <= '{0}' Or Convert (date, b.EditDate) <= '{0}')", Convert.ToDateTime(Date2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(BundleNO))
            {
                sql.Append(string.Format("And BD.BundleNo = '{0}'", BundleNO));
                sql2.Append(string.Format("And BD.BundleNo = '{0}'", BundleNO));
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sql.Append(string.Format("And O.FtyGroup = '{0}'", Factory));
                sql2.Append(string.Format("And O.FtyGroup = '{0}'", Factory));
            }

            //預先開啟excel app
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R50_ProductionBundleTransfer.xltx");
            
            DataRow dr;
            MyUtility.Check.Seek(sql2.ToString(), out dr, null);
            int ct = MyUtility.Convert.GetInt(dr[0]);
            int num = 200000;
            int Cpage = ct / num ;
            for (int i = 0; i < Cpage; i++)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1]);
                Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1]);
                worksheet1.Copy(worksheetn);
            }
            
            int c = 1;
            using (var cn = new SqlConnection(Env.Cfg.GetConnection("").ConnectionString))
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

                        //do some jobs                       
                        Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[c];
                        MyUtility.Excel.CopyToXls(ds.Tables[0], "", "Subcon_R50_ProductionBundleTransfer.xltx", 1, false, null, objApp, wSheet: objSheets);                        
                        c++;

                        //if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                        ds.Tables[0].Dispose();
                        ds.Tables.Clear();
                    }
                }
            }
            if (Cpage > 0)
            {
                objApp.ActiveWorkbook.Worksheets[Cpage].Columns.AutoFit();//這頁需要重新調整欄寬                
            }
            objApp.Visible = true;
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp 

            return true;
        }
    }
}
