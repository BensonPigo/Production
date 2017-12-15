using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using System.Data.SqlClient;
using Sci.Data;
using System.Linq;
using System.Reflection;
using Ict.Win;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    public partial class P30_Print : Sci.Win.Tems.PrintForm
    {
        DualResult result;
        DataTable dtHeader,dtBody,dtexcel;
        DataRow CurrentDataRow;
        string currentID, currentdate;
        public P30_Print(DataRow row, string ID, string issuedate)
        {
            InitializeComponent();
            CurrentDataRow = row;
            currentID = ID;
            currentdate = issuedate;
        }
        //設定畫面
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            print.Visible = true;
            toexcel.Visible = false;
        }
        //設定參數
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        //
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            //print Rdlc
            if (radioNmrmalFormat.Checked==true)
            {
                #region  抓表頭資料
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", currentID));                
                DualResult result = DBProxy.Current.Select("",
                @"select b.NameEN [RptTitle]
	                ,a.LocalSuppID+'-'+c.Name [Supplier]
	                ,c.Tel [Tel]
	                ,c.Address [Address]
                    ,a.FactoryID
                    ,b.AddressEN
                    ,[fTel] =b.Tel
                    ,c.Fax
            from dbo.localpo a WITH (NOLOCK) 
            inner join dbo.factory  b WITH (NOLOCK) on b.id = a.factoryid   
	        left join dbo.LocalSupp c WITH (NOLOCK) on c.id=a.LocalSuppID
            where b.id = a.factoryid
            and a.id = @ID", pars, out dtHeader);
                if (!result) { this.ShowErr(result); }
               

                #endregion

                #region  抓表身資料
                pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", currentID));                
                result = DBProxy.Current.Select("",
                @"
select 	
        Sort = ROW_NUMBER() Over (Partition By a.Delivery, a.Refno Order By a.Delivery, a.Refno)
		,a.OrderId [SP]
        ,a.Delivery [Delivery]
        ,a.Refno [Refno]
        ,a.ThreadColorID [Color_Shade]
        ,dbo.getItemDesc(b.Category,a.Refno) [Description]
        ,a.Price [UPrice]
        ,a.Qty [Order_Qty]
        ,a.UnitId [Unit]
        ,format(Cast(a.Price*a.Qty as decimal(20,2)),'#,###,###,##0.00') [Amount]
		,SortRefno = ROW_NUMBER() Over (Partition By Refno Order By a.Delivery, a.Refno)
		,b.Category
--into #tmp
from dbo.LocalPO_Detail a WITH (NOLOCK) 
left join dbo.LocalPO b WITH (NOLOCK) on  a.id=b.id
where a.id=@ID
order by a.Delivery, a.Refno

--select Sort,[SP],[Delivery]
--	,[Refno]
--	,[Refno2] = iif(SortRefno = 1,[Refno],'')
--	,[Color_Shade]
--	,[Description] = iif(SortRefno = 1,dbo.getItemDesc(Category,Refno),'')
--	,[UPrice],[Order_Qty],[Unit],[Amount]
--from #tmp
--order by Sort
--
--drop table #tmp", pars, out dtBody);
                if (!result) { this.ShowErr(result); }
               
                #endregion
               
            }
            //To Excel Coats
            else
            {
                #region excel
                string sqlcmd = string.Empty;
                sqlcmd = string.Format(@"
select 
a.OrderId
,a.Refno
,c.ArtTkt
,a.ThreadColorID
,c.Description
,a.Qty
,a.Delivery
,a.ID
,b.BrandID
,b.FactoryID
from localpo_detail a WITH (NOLOCK) 
left join orders b WITH (NOLOCK)  on a.orderid = b.id
left join localitem c WITH  (NOLOCK) on c.refno = a.refno 
Where a.id = '{0}' 
order by orderid,a.refno,threadcolorid", currentID);

                result = DBProxy.Current.Select("", sqlcmd, out dtexcel);
                if (!result)
                {
                    return result;
                }
                #endregion
            }

            return Result.True;
        }
        //To Rdlc Print
        protected override bool OnToPrint(ReportDefinition report)
        {
            if ((dtBody == null || dtBody.Rows.Count == 0) || (dtHeader == null || dtHeader.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            #region 表頭
            string RptTitle = dtHeader.Rows[0]["RptTitle"].ToString().Trim();
            string Supplier = dtHeader.Rows[0]["Supplier"].ToString().Trim();
            string FactoryID = dtHeader.Rows[0]["FactoryID"].ToString().Trim();
            string Tel = dtHeader.Rows[0]["Tel"].ToString().Trim();
            string Address = dtHeader.Rows[0]["Address"].ToString().Trim();
            string AddressEN = dtHeader.Rows[0]["AddressEN"].ToString().Trim();
            string fTel = dtHeader.Rows[0]["fTel"].ToString().Trim();
            string Fax = dtHeader.Rows[0]["Fax"].ToString().Trim();
            decimal amount = MyUtility.Convert.GetDecimal(CurrentDataRow["amount"]);
            decimal vat = MyUtility.Convert.GetDecimal(CurrentDataRow["vat"]);
            string CurrencyID = CurrentDataRow["CurrencyID"].ToString();
            string vatrate = CurrentDataRow["vatrate"].ToString() + "%";
            string Remark = CurrentDataRow["remark"].ToString();
            decimal Total = (decimal)CurrentDataRow["amount"] + (decimal)CurrentDataRow["vat"];
            report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", currentID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", currentdate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", Supplier));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Tel", Tel));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Address", Address));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddressEN", AddressEN));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("fTel", fTel));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Fax", Fax));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FactoryID", FactoryID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("amount", amount.ToString("#,0.00")));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("vat", vat.ToString("#,0.00")));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("total", Total.ToString("#,0.00")));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("currency", CurrencyID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("vatrate", vatrate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("remark", Remark));
            #endregion

            #region 表身
            // 傳 list 資料            
            List<P30_PrintData> data = dtBody.AsEnumerable()
                .Select(row1 => new P30_PrintData()
                {
                    Sort = row1["Sort"].ToString().Trim(),
                    SP = row1["SP"].ToString().Trim(),
                    Delivery = (row1["Delivery"] == DBNull.Value) ? "" : Convert.ToDateTime(row1["Delivery"]).ToShortDateString().Trim(),
                    Refno = row1["Refno"].ToString().Trim(),
                        //Refno2 = row1["Refno2"].ToString().Trim(),
                        Color_Shade = row1["Color_Shade"].ToString().Trim(),
                    Description = row1["Description"].ToString().Trim(),
                    UPrice = row1["UPrice"].ToString().Trim(),
                    Order_Qty = Convert.ToDecimal(row1["Order_Qty"]),
                    Unit = row1["Unit"].ToString().Trim(),
                    Amount = Convert.ToDecimal(row1["Amount"]),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC            
            Type ReportResourceNamespace = typeof(P30_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P30_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                this.ShowException(result);
                return result;
            }

            report.ReportResource = reportresource;
            #endregion
            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            return true;
        }
        //To Excel
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (dtexcel == null || dtexcel.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            // 顯示筆數於PrintForm上Count欄位
            SetCount(dtexcel.Rows.Count);
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_P30.xltx"); //預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            MyUtility.Excel.CopyToXls(dtexcel, "", "Subcon_P30.xltx", 1, false, null, objApp);      // 將datatable copy to excel

            #region Save & Shwo Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_P30");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion 
            return true;
        }
        //變更radio後調整畫面顯示
        private void radioPanel1_ValueChanged(object sender, EventArgs e)
        {
            if (radioNmrmalFormat.Checked == true)
            {
                print.Visible = true;
                toexcel.Visible = false;
            }
            else
            {
                print.Visible = false;
                toexcel.Visible = true;
            }
        }
    }
}
