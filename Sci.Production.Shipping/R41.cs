using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class R41 : Sci.Win.Tems.PrintForm
    {
        DataTable printImport, printExport, printAdjust;
        DateTime? date1, date2;
        string nlCode, contractNo, type;
        public R41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, ",Import,Export,Adjust");
            comboBox1.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                dateRange1.TextBox1.Focus();
                return false;
            }

            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            nlCode = textBox1.Text;
            contractNo = textBox2.Text;
            type = comboBox1.SelectedIndex == -1 || comboBox1.SelectedIndex == 0 ? "" : comboBox1.SelectedIndex == 1 ? "Import" : comboBox1.SelectedIndex == 2 ? "Export" : "Adjust";
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCondition = new StringBuilder();
            sqlCondition.Append(string.Format(" v.CDate between '{0}' and '{1}'", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));
            if (!MyUtility.Check.Empty(nlCode))
            {
                sqlCondition.Append(string.Format(" and vd.NLCode = '{0}'", nlCode));
            }
            if (!MyUtility.Check.Empty(contractNo))
            {
                sqlCondition.Append(string.Format(" and v.VNContractID = '{0}'", contractNo));
            }

            DualResult result, failResult;
            if (type == "Import" || type == "")
            {
                result = QueryImport(sqlCondition.ToString());
                if (!result)
                {
                    failResult = new DualResult(false, "Query import data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            if (type == "Export" || type == "")
            {
                result = QueryExport(sqlCondition.ToString());
                if (!result)
                {
                    failResult = new DualResult(false, "Query export data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            if (type == "Adjust" || type == "")
            {
                result = QueryAdjust(sqlCondition.ToString());
                if (!result)
                {
                    failResult = new DualResult(false, "Query adjust data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (printImport.Rows.Count <= 0 && printExport.Rows.Count <= 0 && printAdjust.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if ((type == "Import" || type == "") && printImport.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Import data not found!");
            }

            if ((type == "Export" || type == "") && printExport.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Export data not found!");
            }

            if ((type == "Adjust" || type == "") && printAdjust.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Adjust data not found!");
            }

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            bool result;
            if ((type == "Import" || type == "") && printImport.Rows.Count > 0)
            {
                result = MyUtility.Excel.CopyToXls(printImport, "", xltfile: "Shipping_R41_Import.xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            if ((type == "Export" || type == "") && printExport.Rows.Count > 0)
            {
                result = MyUtility.Excel.CopyToXls(printExport, "", xltfile: "Shipping_R41_Export.xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            if ((type == "Adjust" || type == "") && printAdjust.Rows.Count > 0)
            {
                result = MyUtility.Excel.CopyToXls(printAdjust, "", xltfile: "Shipping_R41_Adjust.xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            MyUtility.Msg.WaitClear();
            return true;
        }

        //查詢Import資料
        private Ict.DualResult QueryImport(string sqlCondition)
        {
            string sqlCmd = string.Format(@"select v.ID,v.CDate,v.VNContractID,v.DeclareNo,IIF(v.BLNo='',v.WKNo,v.BLNo) as BLWK,vd.NLCode,vd.HSCode,vd.Qty,vd.UnitID,vd.Remark
from VNImportDeclaration v
inner join VNImportDeclaration_Detail vd on v.ID = vd.ID
where {0} and v.Status = 'Confirmed'
order by v.ID", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printImport);
            return result;
        }

        //查詢Export資料
        private Ict.DualResult QueryExport(string sqlCondition)
        {
            string sqlCmd = string.Format(@"select v.ID,v.CDate,v.VNContractID,v.DeclareNo,v.InvNo,ed.StyleID,ed.SeasonID,ed.BrandID,ed.ExportQty,
isnull(vd.NLCode,'') as NLCode,isnull(vd.HSCode,'') as HSCode,isnull(vd.Qty,0) as Usage,
isnull(vd.UnitID,'') as UnitID,isnull(vcd.Waste,0) as Waste,
Round(ed.ExportQty*isnull(vd.Qty,0)*(1+isnull(vcd.Waste,0)),3) as Total,
IIF(v.Status = 'Junked','Y','') as Cancel
from VNExportDeclaration v
inner join VNExportDeclaration_Detail ed on v.ID = ed.ID
left join VNConsumption c on c.VNContractID = v.VNContractID and c.CustomSP = ed.CustomSP
left join VNConsumption_Detail vd on c.ID = vd.ID
left join VNContract_Detail vcd on vcd.ID = v.VNContractID and vcd.NLCode = vd.NLCode
where {0} and (v.Status = 'Confirmed' or v.Status = 'Junked')
order by v.ID", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printExport);
            return result;
        }

        //查詢Adjust資料
        private Ict.DualResult QueryAdjust(string sqlCondition)
        {
            string sqlCmd = string.Format(@"select v.CDate,v.VNContractID,v.DeclareNo,vd.NLCode,isnull(cd.HSCode,'') as HSCode,vd.Qty,isnull(cd.UnitID,'') as UnitID,v.Remark
from VNContractQtyAdjust v
inner join VNContractQtyAdjust_Detail vd on v.ID =vd.ID
left join VNContract_Detail cd on cd.ID = v.VNContractID and cd.NLCode = vd.NLCode
where {0} and v.Status = 'Confirmed'
order by v.CDate", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printAdjust);
            return result;
        }
    }
}
