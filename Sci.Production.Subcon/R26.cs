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
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Sci.Utility.Excel;
namespace Sci.Production.Subcon
{
    public partial class R26 : Sci.Win.Tems.PrintForm
    {
        DataRow CurrentDataRow;
        public R26(DataRow row)
        {
            this.CurrentDataRow = row;
        }
        public R26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            string sqlcmd = (@"select DISTINCT ftygroup FROM DBO.Factory");
            DBProxy.Current.Select("", sqlcmd, out factory);
            this.comboBox2.DataSource = factory;
            this.comboBox2.ValueMember = "ftygroup";
            this.comboBox2.DisplayMember = "ftygroup";
            this.comboBox2.SelectedIndex = 0;
        }
       
        DateTime? SCI_Delivery;
        DateTime? SCI_Delivery2;
        DateTime? Issue_Date1;
        DateTime? Issue_Date2;
        string SP;
        string SP2;
        string Location_Poid;
        string Location_Poid2;
        string Factory;
        string Category; 
        string Supplier;
        string Report_Type;
        string Shipping_Mark;
        string date = DateTime.Now.ToShortDateString();
        
        protected override bool ValidateInput()
        {
            if (!this.dateRange1.HasValue && !this.dateRange2.HasValue)
            {
                MyUtility.Msg.ErrorBox("[SCI Delivery] or [Issue_Date] one of the inputs must be selected");
                dateRange1.Focus();
               
                return false;
            }
           
            SCI_Delivery = dateRange1.Value1;
            SCI_Delivery2 = dateRange1.Value2;
            Issue_Date1 = dateRange2.Value1;
            Issue_Date2 = dateRange2.Value2; 
            SP = textBox1.Text.ToString();
            SP2= textBox2.Text.ToString();
            Location_Poid = textBox3.Text.ToString();
            Location_Poid2 = textBox4.Text.ToString();
            Factory = comboBox2.SelectedItem.ToString();
            Category = txtartworktype_fty1.Text.ToString();
            Supplier = txtsubcon1.TextBox1.Text;
            Report_Type = comboBox1.SelectedItem.ToString();
            Shipping_Mark = checkBox1.Checked.ToString();
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = ""; string into = "";
            List<string> sqlWheres = new List<string>();
            if (!this.textBox1.Text.Empty() && !this.textBox2.Text.Empty())
            {
                sqlWheres.Add("c.orderid between @SP and @SP2");
                lis.Add(new SqlParameter("@SP", SCI_Delivery));
                lis.Add(new SqlParameter("@SP2", SCI_Delivery2));
            }
            if (!this.dateRange1.Value1.Empty() && !this.dateRange1.Value2.Empty())
            {
                sqlWheres.Add("a.scidelivery between @SCI_Delivery and @SCI_Delivery2");
                lis.Add(new SqlParameter("@SCI_Delivery", SCI_Delivery));
                lis.Add(new SqlParameter("@SCI_Delivery2", SCI_Delivery2));
            }
            if (!this.dateRange2.Value1.Empty() && !this.dateRange2.Value2.Empty())
            {
                sqlWheres.Add("b.issuedate between @Issue_Date1 and @Issue_Date2");
                lis.Add(new SqlParameter("@Issue_Date1", Issue_Date1));
                lis.Add(new SqlParameter("@Issue_Date2", Issue_Date2));
            }
            if (!this.textBox3.Text.Empty() && !this.textBox4.Text.Empty())
            {
                sqlWheres.Add("b.id between @Location_Poid and @Location_Poid2");
                lis.Add(new SqlParameter("@Location_Poid", Location_Poid));
                lis.Add(new SqlParameter("@Location_Poid2", Location_Poid2));
            }
            if (Factory != "")//(!this.comboBox2.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("b.factoryid =@Factory");
                lis.Add(new SqlParameter("@Factory", Factory));
            }
            if (!this.txtsubcon1.Text.Empty())
            {
                sqlWheres.Add("b.localsuppid =@Supplier");
                lis.Add(new SqlParameter("@Supplier", Supplier));
            }
            if (!this.txtartworktype_fty1.Text.Empty())
            {
                sqlWheres.Add("b.category =@Category");
                lis.Add(new SqlParameter("@Category", Category));
            }
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            DualResult result;
            string sqlcmd = string.Format(@"select  a.FactoryID [Factory]
                                                    ,b.FactoryId [Original Factory]
                                                    ,c.OrderId [SP]
                                                    ,a.StyleID [Style]
                                                    ,a.SeasonID [Season]
                                                    ,b.LocalSuppID+'-'+d.Abb [Supp]
                                                    ,c.Delivery [Delivery]
                                                    ,c.Refno [Code]
                                                    ,c.ThreadColorID [Color_Shade]
                                                    ,b.IssueDate [Issue_Date]
                                                    ,[Description]=dbo.getItemDesc(b.Category,c.Refno)
                                                    ,c.Qty [PO_Qty]
                                                    ,c.UnitId [Unit]
                                                    ,c.Price [Price]
                                                    ,c.Qty*c.Price [Amount]
                                                    ,c.InQty [In-Coming]
                                                    ,c.APQty [AP_Qty]
                                                    ,c.Remark [Remark]
                                           from dbo.Orders a
                                           left join dbo.LocalPO b on a.factoryid=b.factoryid
                                           left join dbo.LocalPO_Detail c on a.id=c.OrderId
                                           left join dbo.LocalSupp d on b.LocalSuppID=d.ID  " + sqlWhere + " " + into);
            result = DBProxy.Current.Select("", sqlcmd, lis, out dtt);

          
            string cmd = string.Format(@"select   b.id [LocalPOID]
                                                 ,b.FactoryId [Factory]
                                                 ,c.OrderId [SP]
                                                 ,b.LocalSuppID [Supp]
                                                 ,c.Delivery [Delivery]
                                                 ,c.Refno [Code]
                                                 ,c.ThreadColorID [Color_Shade]
                                                 ,b.IssueDate [Issue_Date]
                                                 ,[Description]=dbo.getItemDesc(b.Category,c.Refno)
                                                 ,c.Qty [Order_Qty]
                                                 ,c.UnitId [Unit]
                                                 ,c.price [Price]
                                                 ,c.qty* c.price [Amount]
                                                 ,c.InQty [In-Coming]
                                                 ,c.APQty [AP_Qty]
                                       from dbo. LocalPO b
                                       left join dbo.LocalPO_Detail c on b.id=c.Id " + sqlWhere + " " + into);
            result = DBProxy.Current.Select("", cmd, lis, out da);


            #region
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
          
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result2 = DBProxy.Current.Select("",
            @"select e.NameEN [Title1] 
                    ,e.AddressEN [Title2]
                    ,e.Tel [Title3]
                    ,b.LocalSuppID [To]
                    ,d.Tel [Tel]
                    ,d.Fax [Fax]
                    ,b.IssueDate [Issue_Date]
                    ,c.OrderId [P/O#]
                    ,c.Refno [Code]
                    ,c.ThreadColorID [Color_Shade]
                    ,[Description]=dbo.getItemDesc(b.Category,c.Refno)
                    ,c.Qty [Quantity]
                    ,c.UnitId [Unit]
                    ,c.Price[Unit_Price]
                    ,c.Qty*c.Price [Amount]
                    ,[Total_Quantity]=sum(c.Qty) OVER (PARTITION BY c.OrderId,c.Refno) 
                    ,b.Remark [Remark] 
                    ,b.CurrencyId [Total1] 
                    ,b.Amount [Total2]
                    ,b.CurrencyId [currencyid]
                    ,b.Vat [vat]
                    ,b.Amount+b.Vat [Grand_Total]     
	           from dbo.localpo b 
               left join dbo.Factory  e on e.id = b.factoryid 
	           left join dbo.LocalPO_Detail c on b.id=c.Id
	           left join dbo.LocalSupp d on b.LocalSuppID=d.ID
              where b.id = @ID", pars, out dt);
            if (!result) { this.ShowErr(result); }
            string Title1 = dt.Rows[0]["NameEN"].ToString();
            string Title2 = dt.Rows[0]["AddressEN"].ToString();
            string Title3 = dt.Rows[0]["Tel"].ToString();
            string To = dt.Rows[0]["LocalSuppID"].ToString();
            string Tel = dt.Rows[0]["Tel"].ToString();
            string Fax = dt.Rows[0]["Fax"].ToString();
            string Issue_Date = dt.Rows[0]["Issue_Date"].ToString();
            string Total1 = dt.Rows[0]["CurrencyId"].ToString();
            string Total2 = dt.Rows[0]["Amount"].ToString();
            string CurrencyId = dt.Rows[0]["currencyid"].ToString();
            string vat = dt.Rows[0]["vat"].ToString();
            string Grand_Total = dt.Rows[0]["Grand_Total"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title1", Title1));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title2", Title2));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title3", Title3));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("To", To));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Tel", Tel));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Fax", Fax));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issue_Date", Issue_Date));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Total1", Total1));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Total2", Total2));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CurrencyId", CurrencyId));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("vat", vat));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Grand_Total", Grand_Total));

            // 傳 list 資料            
            List<R26_PrintData> data = dt.AsEnumerable()
                .Select(row1 => new R26_PrintData()
                {
                    PO = row1["PO"].ToString(),
                    Code = row1["Code"].ToString(),
                    Color_Shade = row1["Color_Shade"].ToString(),
                    Description = row1["Description"].ToString(),
                    Quantity = row1["Quantity"].ToString(),
                    Unit = row1["Unit"].ToString(),
                    Unit_Price = row1["Unit_Price"].ToString(),
                    Amount = row1["Amount"].ToString(),
                    Total_Quantity = row1["Total_Quantity"].ToString(),
                    Remark = row1["Remark"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(R26_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "R26_Print.rdlc";

            IReportResource reportresource;
            if (!(result2 = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                ////this.ShowException(result);
                //return;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            return result;
         
            //return result2;//base.OnAsyncDataLoad(e);
        }
        DataTable dtt;
        DataTable da;
        //按件觸發
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
              if (comboBox1.Text == "PO List")
            {
            print.Enabled = false;
             
            }
            else if (comboBox1.Text == "PO Form")
             {
                toexcel.Enabled = false;
                print.Enabled = true;
            }else  if (comboBox1.Text == "PO Order")
                {
                print.Enabled = false;
                toexcel.Enabled = true;

                 }
        }
        protected override bool OnToExcel(ReportDefinition report)
        {
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }
       
            if ("PO List".EqualString(this.comboBox1.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R26_Local_PO_List.xltx");
                xl.dicDatas.Add("##Factory", dtt);
                xl.Save(outpath, false);
            }
            else if ("PO Order".EqualString(this.comboBox1.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R26_Local_PO_Order.xltx");
                
                x1.dicDatas.Add("##LocalPOID",Location_Poid);
                x1.dicDatas.Add("##Factory", Factory);
                x1.dicDatas.Add("##date", date);
                x1.dicDatas.Add("##SP",da);
                x1.Save(outpath, false);

            }
            return true;
            //return base.OnToExcel(report);
        }
    }
}
