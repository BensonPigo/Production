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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R25 : Sci.Win.Tems.PrintForm
    {
        public R25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory = null;

            DBProxy.Current.Select(null, "select '' as ID union all select DISTINCT ftygroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboBox1, 1, factory);
            comboBox1.SelectedValue = Sci.Env.User.Factory;
            this.print.Enabled = false;
        }

        DateTime? ReceiveDate;
        DateTime? ReceiveDate2;
        string SP;
        string Refno;
        string Category;
        string Supplier;
        string Factory;

        protected override bool ValidateInput()
        {
            if (!this.dateRange1.HasValue && this.textBox1.Text.Empty())
            {
                MyUtility.Msg.ErrorBox("[Receive Date] or [SP#] must input one !!");
                dateRange1.Focus();
                textBox1.Focus();
                return false;
            }
           
             ReceiveDate = dateRange1.Value1;
             ReceiveDate2 = dateRange1.Value2;
             SP = textBox1.Text.ToString();
             Refno = textBox2.Text.ToString();
             Category = txtartworktype_fty1.Text.ToString();
             Supplier = txtsubcon1.TextBox1.Text;
             Factory = comboBox1.SelectedValue.ToString();
            
            return base.ValidateInput();
        }
       

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = ""; string order = "";
            List<string> sqlWheres = new List<string>();
            if (!this.dateRange1.Value1.Empty())
            {
                sqlWheres.Add("lr.issuedate >= @ReceiveDate");
                lis.Add(new SqlParameter("@ReceiveDate", ReceiveDate));
            }
            if (!this.dateRange1.Value2.Empty())
            {
                sqlWheres.Add("lr.issuedate <= @ReceiveDate2");
                lis.Add(new SqlParameter("@ReceiveDate2", ReceiveDate2));
            }
            if (!this.textBox1.Text.Empty())
            {
                sqlWheres.Add("lrd.orderid=@SP");
                lis.Add(new SqlParameter("@SP", SP));
            }
            if (!this.textBox2.Text.Empty())
            {
                sqlWheres.Add("lrd.refno=@Refno");
                lis.Add(new SqlParameter("@Refno", Refno));
            }
            if (!this.txtartworktype_fty1.Text.Empty())
            {
                sqlWheres.Add("lrd.category=@Category");
                lis.Add(new SqlParameter("@Category", Category));
            }
            if (!this.txtsubcon1.TextBox1.Text.Empty())
            {
                sqlWheres.Add("lr.localsuppid=@Supplier");
                lis.Add(new SqlParameter("@Supplier", Supplier));
            }
            if (Factory != "") //(!this.comboBox1.Text.Empty())
            {
                sqlWheres.Add("lr.factoryid =@Factory");
                lis.Add(new SqlParameter("@Factory", Factory));
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
        ,lr.IssueDate [Receive_Date]
        ,lr.LocalSuppID + ' - ' + LS.Abb [Supplier]
        ,lr.InvNo [Invoice]
        ,lrd.OrderId [SP]
        ,lrd.Category [Category]
        ,lrd.Refno [Refno]
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
left join dbo.LocalPO_Detail c WITH (NOLOCK) on lrd.LocalPo_detailukey=c.Ukey  " + sqlWhere + " " + order);
            result = DBProxy.Current.Select("", sqlcmd,lis, out dtt);
        
            return result; //base.OnAsyncDataLoad(e);
            }
            DataTable dtt;

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dtt == null || dtt.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R25.xltx");
            MyUtility.Excel.CopyToXls(dtt, "", "Subcon_R25.xltx", 3, showExcel: false, showSaveMsg: false, excelApp : objApp);

            this.ShowWaitMessage("Excel Processing...");
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Cells[2, 1] = string.Format("Receive Date: {0}~{1}  ,SP#:{2}  ,Refno:{3} Category:{4}  Supplier:{5}  ,Factory:{6}  ",
                                                    (MyUtility.Check.Empty(ReceiveDate)) ? "" : Convert.ToDateTime(ReceiveDate).ToString("yyyy/MM/dd"),
                                                    (MyUtility.Check.Empty(ReceiveDate2)) ? "" : Convert.ToDateTime(ReceiveDate2).ToString("yyyy/MM/dd"),
                                                    SP, Refno, Category, Supplier, Factory);            

            objApp.Visible = true;
            this.HideWaitMessage();

            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放worksheet
            return false;
        }
    }
 }