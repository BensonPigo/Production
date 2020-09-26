using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R25 : Win.Tems.PrintForm
    {
        public R25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory = null;

            DBProxy.Current.Select(null, "select '' as ID union all select DISTINCT ftygroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.SelectedValue = Env.User.Factory;
            this.print.Enabled = false;
        }

        private DateTime? ReceiveDate;
        private DateTime? ReceiveDate2;
        private string SP;
        private string Refno;
        private string Category;
        private string Supplier;
        private string Factory;

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateReceiveDate.HasValue && this.txtSPNo.Text.Empty())
            {
                MyUtility.Msg.ErrorBox("[Receive Date] or [SP#] must input one !!");
                this.dateReceiveDate.Focus();
                this.txtSPNo.Focus();
                return false;
            }

            this.ReceiveDate = this.dateReceiveDate.Value1;
            this.ReceiveDate2 = this.dateReceiveDate.Value2;
            this.SP = this.txtSPNo.Text.ToString();
            this.Refno = this.txtRefno.Text.ToString();
            this.Category = this.txtartworktype_ftyCategory.Text.ToString();
            this.Supplier = this.txtsubconSupplier.TextBox1.Text;
            this.Factory = this.comboFactory.SelectedValue.ToString();

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = string.Empty;
            string order = string.Empty;
            List<string> sqlWheres = new List<string>();
            if (!this.dateReceiveDate.Value1.Empty())
            {
                sqlWheres.Add("lr.issuedate >= @ReceiveDate");
                lis.Add(new SqlParameter("@ReceiveDate", this.ReceiveDate));
            }

            if (!this.dateReceiveDate.Value2.Empty())
            {
                sqlWheres.Add("lr.issuedate <= @ReceiveDate2");
                lis.Add(new SqlParameter("@ReceiveDate2", this.ReceiveDate2));
            }

            if (!this.txtSPNo.Text.Empty())
            {
                sqlWheres.Add("lrd.orderid=@SP");
                lis.Add(new SqlParameter("@SP", this.SP));
            }

            if (!this.txtRefno.Text.Empty())
            {
                sqlWheres.Add("lrd.refno=@Refno");
                lis.Add(new SqlParameter("@Refno", this.Refno));
            }

            if (!this.txtartworktype_ftyCategory.Text.Empty())
            {
                sqlWheres.Add("lrd.category=@Category");
                lis.Add(new SqlParameter("@Category", this.Category));
            }

            if (!this.txtsubconSupplier.TextBox1.Text.Empty())
            {
                sqlWheres.Add("lr.localsuppid=@Supplier");
                lis.Add(new SqlParameter("@Supplier", this.Supplier));
            }

            if (this.Factory != string.Empty) // (!this.comboBox1.Text.Empty())
            {
                sqlWheres.Add("lr.factoryid =@Factory");
                lis.Add(new SqlParameter("@Factory", this.Factory));
            }

            order = "order by lr.issuedate,lr.id";

            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            DualResult result;

            string sqlcmd = string.Format(@"
select  lr.FactoryId [Factory] 
        ,lr.Id [ID]
        ,c.Delivery[EstDelvieryDate]
        ,lr.IssueDate [Receive_Date]
        ,lr.LocalSuppID + ' - ' + LS.Abb [Supplier]
        ,lr.InvNo [Invoice]
        ,lrd.OrderId [SP]
        ,lrd.Category [Category]
        ,lrd.Refno [Refno]
        ,[IsCarton] = iif(li.IsCarton = 1 ,'Y','')
        ,[Description]=dbo.getItemDesc(lrd.Category,lrd.Refno)
        ,lrd.ThreadColorID [Color_Shade]
        ,c.UnitId [Unit]
        ,c.Qty [PO_Qty]
        ,lrd.Qty [Qty]
        ,lrd.Qty [On_Road]
        ,lrd.Location [Location]
        ,lr.Remark [Remark]
from dbo.LocalReceiving lr WITH (NOLOCK) 
left join dbo.LocalSupp LS on lr.LocalSuppID = LS.ID
left join dbo.LocalReceiving_Detail lrd WITH (NOLOCK) on  lr.id=lrd.Id
left join dbo.LocalPO_Detail c WITH (NOLOCK) on lrd.LocalPo_detailukey=c.Ukey  
left join dbo.LocalItem li WITH (NOLOCK) on li.RefNo=lrd.Refno
" + sqlWhere + " " + order);
            result = DBProxy.Current.Select(string.Empty, sqlcmd, lis, out this.dtt);

            return result; // base.OnAsyncDataLoad(e);
        }

        private DataTable dtt;

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dtt == null || this.dtt.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.dtt.Rows.Count);
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R25.xltx");
            MyUtility.Excel.CopyToXls(this.dtt, string.Empty, "Subcon_R25.xltx", 3, showExcel: false, showSaveMsg: false, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Cells[2, 1] = string.Format(
                "Receive Date: {0}~{1}  ,SP#:{2}  ,Refno:{3} Category:{4}  Supplier:{5}  ,Factory:{6}  ",
                MyUtility.Check.Empty(this.ReceiveDate) ? string.Empty : Convert.ToDateTime(this.ReceiveDate).ToString("yyyy/MM/dd"),
                MyUtility.Check.Empty(this.ReceiveDate2) ? string.Empty : Convert.ToDateTime(this.ReceiveDate2).ToString("yyyy/MM/dd"),
                this.SP, this.Refno, this.Category, this.Supplier, this.Factory);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Subcon_R25");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return false;
        }
    }
 }