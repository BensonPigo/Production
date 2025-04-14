using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class R22 : Win.Tems.PrintForm
    {
        private string dDate1;
        private string dDate2;
        private string iDate1;
        private string iDate2;
        private string strStyle;
        private string productType;
        private string strSP;
        private string strCategory;
        private string strCell;
        private string strRD;
        private string strFactory;
        private DataTable[] printData = new DataTable[2];

        private IE_R22_ViewModel model;

        /// <inheritdoc/>
        public R22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.cbProductType.SetDataSource();
            this.cbFactoryID.SetDataSource();
        }


        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dtAddEdit.HasValue1 && !this.dtAddEdit.HasValue2 && !this.dtInline.HasValue1 && !this.dtInline.HasValue2 && string.IsNullOrEmpty(this.txtSPNO.Text))
            {
                MyUtility.Msg.InfoBox("Please fill in at least one：<Deadline> or <Inline Date> or <SP>!");
                return false;
            }

            this.model = new IE_R22_ViewModel()
            {
                DeadlineStart = this.dtAddEdit.HasValue1 ? this.dtAddEdit.Value1 : (DateTime?)null,
                DeadlineEnd = this.dtAddEdit.HasValue2 ? this.dtAddEdit.Value2 : (DateTime?)null,
                InlineStart = this.dtInline.HasValue1 ? this.dtInline.Value1 : (DateTime?)null,
                InlineEnd = this.dtInline.HasValue2 ? this.dtInline.Value2 : (DateTime?)null,
                OrderID = this.txtSPNO.Text,
                StyleID = this.txtStyle.Text,
                ProductType = this.cbProductType.Text,
                Category = this.txtCategory.Text,
                SewingCell = this.txtCell.Text,
                ResponseDep = this.txtRD.Text,
                FactoryID = this.cbFactoryID.Text,
                IsOutstanding = this.chkOutstanding.Checked,
                IsPowerBI = false,
            };

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            IE_R22 biModel = new IE_R22();
            Base_ViewModel result = biModel.GetIE_R22_Summary(this.model);
            if (!result.Result)
            {
                return new DualResult(false, "Query data fail\r\n" + result.Result.Messages.ToString());
            }

            this.printData[0] = result.Dt;

            result = biModel.GetIE_R22_Detail(this.model);
            if (!result.Result)
            {
                return new DualResult(false, "Query data fail\r\n" + result.Result.Messages.ToString());
            }

            this.printData[1] = result.Dt;
            return result.Result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData[0].Rows.Count);
            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string filename = "IE_R22.xltx";
            Excel.Application excelapp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename); // 預先開啟excel app
            if (this.printData[0].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, filename, 1, false, null, excelapp, wSheet: excelapp.Sheets[1]);
            }

            if (this.printData[1].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.printData[1], string.Empty, filename, 1, false, null, excelapp, wSheet: excelapp.Sheets[2]);
            }

            excelapp.Columns.AutoFit();
            string excelfile = Class.MicrosoftFile.GetName("IE_R22");
            excelapp.ActiveWorkbook.SaveAs(excelfile);
            excelapp.Visible = true;
            Marshal.ReleaseComObject(excelapp);
            return true;
        }

        private void TxtSPNO_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand = "select OrderID from Production.dbo.ChgOver WITH (NOLOCK) order by ID";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(selectCommand, "OrderID", "30", this.txtSPNO.Text)
            {
                Size = new System.Drawing.Size(400, 530),
            };
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSPNO.Text = item.GetSelectedString();
            this.ValidateControl();
        }

        private void TxtCell_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand = "Select DISTINCT Cell = SewingCell from  SewingLine WITH (NOLOCK) where SewingCell <> '' order by SewingCell ASC";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(selectCommand, string.Empty, this.txtCell.Text)
            {
                Size = new System.Drawing.Size(300, 330),
            };
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCell.Text = item.GetSelectedString();
            this.ValidateControl();
        }

        private void TxtCategory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand = "select DISTINCT Category from ChgOver WITH (NOLOCK) where Category <> '' order by Category ASC";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(selectCommand, string.Empty, this.txtCategory.Text)
            {
                Size = new System.Drawing.Size(300, 330),
            };
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCategory.Text = item.GetSelectedString();
            this.ValidateControl();
        }

        private void TxtRD_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand = "select DISTINCT Dept from Employee WITH (NOLOCK) where dept <> '' order by Dept ASC";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(selectCommand, "Response Dep.", "20", this.txtCategory.Text)
            {
                Size = new System.Drawing.Size(400, 430),
            };
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtRD.Text = item.GetSelectedString();
            this.ValidateControl();
        }
    }
}
