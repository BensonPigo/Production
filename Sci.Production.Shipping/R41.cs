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
            MyUtility.Tool.SetupCombox(comboType, 1, 1, ",Import,Export,Adjust");
            comboType.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            //if (MyUtility.Check.Empty(dateRange1.Value1))
            //{
            //    MyUtility.Msg.WarningBox("Date can't empty!!");
            //    dateRange1.TextBox1.Focus();
            //    return false;
            //}

            date1 = dateDate.Value1;
            date2 = dateDate.Value2;
            nlCode = txtNLCode.Text;
            contractNo = txtContractNo.Text;
            type = comboType.SelectedIndex == -1 || comboType.SelectedIndex == 0 ? "" : comboType.SelectedIndex == 1 ? "Import" : comboType.SelectedIndex == 2 ? "Export" : "Adjust";
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCondition = new StringBuilder();
            if (!MyUtility.Check.Empty(date1))
            {
                sqlCondition.Append(string.Format(" and v.CDate >= '{0}' ", Convert.ToDateTime(date1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(date2))
            {
                sqlCondition.Append(string.Format(" and v.CDate <= '{0}' ", Convert.ToDateTime(date2).ToString("d")));
            }

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
            if ((type == "Import" || type == "")
                 && (printImport == null || printImport.Rows.Count <= 0))
            {
                MyUtility.Msg.WarningBox("Import data not found!");
            }

            if ((type == "Export" || type == "") 
                 && (printExport == null || printExport.Rows.Count <= 0))
            {
                MyUtility.Msg.WarningBox("Export data not found!");
            }

            if ((type == "Adjust" || type == "") 
                 && (printAdjust == null || printAdjust.Rows.Count <= 0))
            {
                MyUtility.Msg.WarningBox("Adjust data not found!");
            }

            this.ShowWaitMessage("Starting EXCEL...");
            bool result;
            if ((type == "Import" || type == "") 
                 && printImport != null 
                 && printImport.Rows.Count > 0)
            {
                result = MyUtility.Excel.CopyToXls(printImport, Sci.Production.Class.MicrosoftFile.GetName("Shipping_R41_Import"), xltfile: "Shipping_R41_Import.xltx", headerRow: 1, showSaveMsg: false);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            if ((type == "Export" || type == "") 
                 && printExport != null 
                 && printExport.Rows.Count > 0)
            {
                result = MyUtility.Excel.CopyToXls(printExport, Sci.Production.Class.MicrosoftFile.GetName("Shipping_R41_Export"), xltfile: "Shipping_R41_Export.xltx", headerRow: 1, showSaveMsg:false);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            if ((type == "Adjust" || type == "") 
                 && printAdjust  != null 
                 && printAdjust.Rows.Count > 0)
            {
                result = MyUtility.Excel.CopyToXls(printAdjust, Sci.Production.Class.MicrosoftFile.GetName("Shipping_R41_Adjust"), xltfile: "Shipping_R41_Adjust.xltx", headerRow: 1, showSaveMsg: false);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            this.HideWaitMessage();
            return true;
        }

        //查詢Import資料
        private Ict.DualResult QueryImport(string sqlCondition)
        {
            string sqlCmd = string.Format(@"select v.ID,v.CDate,v.VNContractID,v.DeclareNo,IIF(v.BLNo='',v.WKNo,v.BLNo) as BLWK,vd.NLCode,vd.HSCode,vd.Qty,vd.UnitID,vd.Remark
from VNImportDeclaration v WITH (NOLOCK) 
inner join VNImportDeclaration_Detail vd WITH (NOLOCK) on v.ID = vd.ID
where 1=1 {0} and v.Status = 'Confirmed'
order by v.ID", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printImport);
            return result;
        }

        //查詢Export資料
        private Ict.DualResult QueryExport(string sqlCondition)
        {
            string sqlCmd = string.Format(@"select v.ID,v.CDate,v.VNContractID,v.DeclareNo,v.InvNo,ed.StyleID,ed.SeasonID,ed.BrandID,ed.ExportQty,
isnull(vd.NLCode,'') as NLCode,isnull(vd.HSCode,'') as HSCode,isnull(vd.Qty,0) as Usage,
isnull(vd.UnitID,'') as UnitID,isnull(vd.Waste,0) as Waste,
Round(ed.ExportQty*isnull(vd.Qty,0)*(1+isnull(vd.Waste,0)),3) as Total,
IIF(v.Status = 'Junked','Y','') as Cancel
from VNExportDeclaration v WITH (NOLOCK) 
inner join VNExportDeclaration_Detail ed WITH (NOLOCK) on v.ID = ed.ID
left join VNConsumption c WITH (NOLOCK) on c.VNContractID = v.VNContractID and c.CustomSP = ed.CustomSP
left join VNConsumption_Detail vd WITH (NOLOCK) on c.ID = vd.ID
left join VNContract_Detail vcd WITH (NOLOCK) on vcd.ID = v.VNContractID and vcd.NLCode = vd.NLCode
where 1=1 {0} and (v.Status = 'Confirmed' or v.Status = 'Junked')
order by v.ID", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printExport);
            return result;
        }

        //查詢Adjust資料
        private Ict.DualResult QueryAdjust(string sqlCondition)
        {
            string sqlCmd = string.Format(@"select v.CDate,v.VNContractID,v.DeclareNo,vd.NLCode,isnull(cd.HSCode,'') as HSCode,vd.Qty,isnull(cd.UnitID,'') as UnitID,v.Remark
from VNContractQtyAdjust v WITH (NOLOCK) 
inner join VNContractQtyAdjust_Detail vd WITH (NOLOCK) on v.ID =vd.ID
left join VNContract_Detail cd WITH (NOLOCK) on cd.ID = v.VNContractID and cd.NLCode = vd.NLCode
where 1=1  {0} and v.Status = 'Confirmed'
order by v.CDate", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printAdjust);
            return result;
        }

        private void txtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(@"select id,startdate,EndDate from [Production].[dbo].[VNContract]", "20,10,10", this.Text, false, ",", headercaptions: "Contract No, Start Date, End Date");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtContractNo.Text = item.GetSelectedString();            
            
        }
    }
}
