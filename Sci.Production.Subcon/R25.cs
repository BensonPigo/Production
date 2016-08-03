using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R25 : Sci.Win.Tems.PrintForm
    {
        public R25()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }
        protected override bool ValidateInput()
        {

            

            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            
            return base.OnAsyncDataLoad(e);
        }

//        private void toexcel_Click(object sender, EventArgs e)
//        {
//            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
//            saveDialog.ShowDialog();
//            string outpath = saveDialog.FileName;
//            if (outpath.Empty())
//            {
//                return;
//            }

//            DualResult result;
//            DataTable dtt;
//            string sqlcmd = string.Format(@"select a.FactoryId [Factory] 
//                                                  ,a.Id [ID]
//                                                  ,a.IssueDate [Receive_Date]
//                                                  ,a.LocalSuppID [Supplier]
//                                                  ,a.InvNo [Invoice]
//                                                  ,b.OrderId [SP]
//                                                  ,b.Category [Category]
//                                                  ,b.Refno [Refno]
//                                                  ,[Description]=dbo.getItemDesc(b.Category,b.Refno)
//                                                --,[Color_Shade]
//                                                  ,c.UnitId [Unit]
//                                                  ,c.Qty [PO_Qty]
//                                                  ,b.Qty [Qty]
//                                                  ,c.Qty-b.Qty [On_Road]
//                                                  ,b.Location [Location]
//                                                  ,a.Remark [Remark]
//                                                  from dbo.LocalReceiving a
//                                                  left join dbo.LocalReceiving_Detail b on  a.id=b.Id
//                                                  left join dbo.LocalPO_Detail c on b.LocalPo_detailukey=c.Ukey ");
//            result = DBProxy.Current.Select("", sqlcmd, out dtt);
//            if (!result)
//            {
//                ShowErr(result);
//                return;
//            }
//            string Factory = dtt.Rows[0]["Factory"].ToString();
//            ReportDefinition report = new ReportDefinition();
//            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Factory", Factory));


//            string xlt = @"Subcon_R25.xltx";
//            SaveXltReportCls xl = new SaveXltReportCls(xlt);
//            SaveXltReportCls.xltRptTable xlTable = new SaveXltReportCls.xltRptTable(dtt);
//            int allColumns = dtt.Columns.Count;
//            xlTable.Borders.OnlyHeaderBorders = true;
//            SaveXltReportCls.xltRptTable xdt_All = new SaveXltReportCls.xltRptTable(xlTable);
//            xdt_All.ShowHeader = false;
//            xl.dicDatas.Add("##Factory", xdt_All);
//            xl.Save(outpath);
//            return; 

//        }

       
    }
}
