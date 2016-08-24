using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P10_Print : Sci.Win.Tems.PrintForm
    {
        public P10_Print(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        DataRow CurrentDataRow;
        public P10_Print(DataRow row)
        {
            this.CurrentDataRow = row;
        }
        string Bundle_Card;
        string Bundle_Check_list;
        string Extend_All_Parts;
        protected override bool ValidateInput()
        {
            if(radioButton1.Checked==true)
            {
                print.Enabled = true;
                toexcel.Enabled = false;
            }
            else if(radioButton2.Checked==true)
            {
               toexcel.Enabled=true;
                print.Enabled = false;
            }
            Bundle_Card = radioButton1.Checked.ToString();
            Bundle_Check_list = radioButton2.Checked.ToString();
            Extend_All_Parts = checkBox1.Checked.ToString();
            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return base.OnAsyncDataLoad(e);
        }
       


        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result;
            ReportDefinition report = new ReportDefinition();

            
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            string sqlcmd = @"select a.BundleGroup [Group_right]
                                    ,left(b.ID,3) [Group_left]
                                    ,b.Sewinglineid [Line]
                                    ,b.SewingCell [Cell]
                                    ,b.Orderid [SP]
                                    ,c.StyleID [Style]
                                    ,b.Item [Item]
                                    ,isnull(b.PatternPanel,'')+''+convert(varchar,b.Cutno) [Body_Cut]
                                    ,b.Colorid [Color]
                                    ,a.SizeCode [Size]
                                    ,a.Qty [Quantity]
                                    ,b.id [Barcode]
                                    from dbo.Bundle_Detail a
                                    left join dbo.Bundle b on a.id=b.id
                                    left join dbo.orders c on c.id=b.Orderid
                                    where a.ID= @ID";
            result = DBProxy.Current.Select("", sqlcmd, pars, out dt);
            if (!result) { this.ShowErr(sqlcmd, result); }
     
            // 傳 list 資料            
            List<P10_PrintData> data = dt.AsEnumerable()
                .Select(row1 => new P10_PrintData()
                {
                    Group_right = row1["Group_right"].ToString(),
                    Group_left = row1["Group_left"].ToString(),
                    Line = row1["Line"].ToString(),
                    Cell = row1["Cell"].ToString(),
                    SP = row1["SP"].ToString(),
                    Style = row1["Style"].ToString(),
                    Item = row1["Item"].ToString(),
                    Body_Cut = row1["Body_Cut"].ToString(),
                    Color = row1["Color"].ToString(),
                    Size = row1["Size"].ToString(),
                    Quantity = row1["Quantity"].ToString(),
                    Barcode = row1["Barcode"].ToString()

                }).ToList();

            report.ReportDataSource = data;
           
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P10_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P10_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            return true;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dtt == null || dtt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P10.xltx"); //預先開啟excel app                         
            MyUtility.Excel.CopyToXls(dtt, "", "Cutting_P10.xltx", 1, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
      
    }
}
