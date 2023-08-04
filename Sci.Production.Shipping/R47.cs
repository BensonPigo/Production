using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Ict.Win.UI.DataGridView;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R47 : Win.Tems.PrintForm
    {
        private string customSP1;
        private string customSP2;
        private string contractNo;
        private DateTime? date1;
        private DateTime? date2;
        private DataTable printData;

        /// <inheritdoc/>
        public R47(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if ((MyUtility.Check.Empty(this.dateCDDATE.Value1) && MyUtility.Check.Empty(this.dateCDDATE.Value2)) &&
               (MyUtility.Check.Empty(this.txtCustomSPNoStart.Text) && MyUtility.Check.Empty(this.txtCustomSPNoEnd.Text)) &&
                MyUtility.Check.Empty(this.txtCustomsContract1.Text))
            {
                MyUtility.Msg.WarningBox("Please at least fill in one selection!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtCustomSPNoStart.Text) != MyUtility.Check.Empty(this.txtCustomSPNoEnd.Text))
            {
                if (MyUtility.Check.Empty(this.txtCustomSPNoStart.Text))
                {
                    this.txtCustomSPNoStart.Focus();
                }
                else
                {
                    this.txtCustomSPNoEnd.Focus();
                }

                MyUtility.Msg.WarningBox("Custom SP# can't be empty!!");
                return false;
            }

            this.date1 = this.dateCDDATE.Value1;
            this.date2 = this.dateCDDATE.Value2;
            this.customSP1 = this.txtCustomSPNoStart.Text;
            this.customSP2 = this.txtCustomSPNoEnd.Text;
            this.contractNo = this.txtCustomsContract1.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = string.Empty;
            string sqlWhere = string.Empty;
            DualResult dualResult;

            if (!MyUtility.Check.Empty(this.contractNo))
            {
                sqlWhere += $" and vc.VNContractID = '{this.contractNo}'";
            }

            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlWhere += $" and vc.CDate >= '{System.Convert.ToDateTime(this.date1).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlWhere += $" and vc.CDate <= '{System.Convert.ToDateTime(this.date2).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.customSP1))
            {
                sqlWhere += $" and vc.CustomSP between '{this.customSP1}' and '{this.customSP2}'";
            }

            sqlcmd = $@"
            SELECT 
            [CustonSP] = vc.CustomSP,
            [Ver] = vc.[Version],
            [Contractno] = vc.VNContractID,
            [Style] = vc.StyleID,
            [Date] = vc.CDate,
            [Season] = vc.SeasonID,
            [Brand] = vc.BrandID,
            [Category] = vc.Category,
            [Size] = vc.SizeCode,
            [Colorway] = Colorway.val,
            [SizeGroup] = SizeGroup.val,
            [CustomsCode] = vcd.NLCode,
            [Type] = vcdd.FabricType,
            [UsageUnit] = vcdd.UsageUnit,
            [RefNo] = vcdd.RefNo,
            [CustomsUnit] = vcdd.UnitID,
            [SystemQty] = vcdd.SystemQty,
            [ActQty] = vcdd.UsageQty,
            [CustomsQty] = vcdd.Qty,
            [Waste] = vcd.Waste,
            [CreateByUser] = iif(vcdd.UserCreate = 1 , 'Yes','No'),
            [Consumption] = vcd.Qty,
            [Consumption consist of the allowance] = vcd.Qty * ( 1 + vcd.Waste / 100),
            [Remark] = vn.DescVI
            from VNConsumption vc WITH (NOLOCK) 
            inner join VNConsumption_Detail vcd WITH (NOLOCK) on vc.ID = vcd.ID
            inner join VNConsumption_Detail_Detail vcdd WITH (NOLOCK) on vcd.ID= vcdd.ID and vcd.NLCode = vcdd.NLCode
            left join VNContract_Detail vd WITH (NOLOCK) on vc.VNContractID = vd.ID and vcd.NLCode = vd.NLCode
            left join VNNLCodeDesc vn WITH (NOLOCK) on vn.NLCode = vcd.NLCode
            OUTER APPLY
            (
	            select val = stuff(
	            (
		            select concat(',',tmp.Article)
		            from
		            (
			            select distinct va.Article
			            from VNConsumption_Article va
			            where va.ID = vc.ID 
		            ) tmp for xml path('')
	            )
	            ,1,1,'')
            )Colorway
            OUTER APPLY
            (
	            select val = stuff(
	            (
		            select concat(',',tmp.SizeCode)
		            from
		            (
			            select distinct va.SizeCode
			            from VNConsumption_SizeCode va
			            where va.ID = vc.ID 
		            ) tmp for xml path('')
	            )
	            ,1,1,'')
            )SizeGroup
            WHERE 1 = 1
            and vc.Status = 'Confirmed'
            {sqlWhere}
            order by [CustonSP],[CustomsCode]";

            dualResult = DBProxy.Current.Select(null, sqlcmd, out this.printData);

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {

            if (this.printData == null)
            {
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");

            string excelName = "Shipping_R47";
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + $"\\{excelName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: 1, excelApp: objApp);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelName);

            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;

            workbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
