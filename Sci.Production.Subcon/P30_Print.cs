using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class P30_Print : Win.Tems.PrintForm
    {
        DualResult result;
        DataTable dtHeader;
        DataTable dtBody;
        DataTable dtexcel;
        DataRow CurrentDataRow;
        string currentID;
        string currentdate;

        public P30_Print(DataRow row, string ID, string issuedate)
        {
            this.InitializeComponent();
            this.CurrentDataRow = row;
            this.currentID = ID;
            this.currentdate = issuedate;
        }

        // 設定畫面
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.print.Visible = true;
            this.toexcel.Visible = false;
        }

        // 設定參數
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            // print Rdlc
            if (this.radioNmrmalFormat.Checked == true)
            {
                #region  抓表頭資料
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", this.currentID));
                DualResult result = DBProxy.Current.Select(
                    string.Empty,
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
            and a.id = @ID", pars, out this.dtHeader);
                if (!result)
                {
                    this.ShowErr(result);
                }

                #endregion

                #region  抓表身資料
                pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", this.currentID));
                result = DBProxy.Current.Select(
                    string.Empty,
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
		,b.Category,b.apvname,b.Lockname
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
--drop table #tmp", pars, out this.dtBody);
                if (!result)
                {
                    this.ShowErr(result);
                }

                #endregion

            }
            else if (this.radioByRefno.Checked == true)
            {
                #region  抓表頭資料
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", this.currentID));
                DualResult result = DBProxy.Current.Select(
                    string.Empty,
                    @"select b.NameEN [RptTitle]
	                ,a.LocalSuppID+'-'+c.Name [Supplier]
	                ,c.Tel [Tel]
	                ,c.Address [Address]
                    ,a.FactoryID
                    ,b.AddressEN
                    ,[fTel] =b.Tel
                    ,c.Fax
                    , AddName = p.Name
            from dbo.localpo a WITH (NOLOCK) 
            inner join dbo.factory  b WITH (NOLOCK) on b.id = a.factoryid   
	        left join dbo.LocalSupp c WITH (NOLOCK) on c.id=a.LocalSuppID
            left join dbo.Pass1 p with (nolock) on p.id = a.AddName
            where b.id = a.factoryid
            and a.id = @ID", pars, out this.dtHeader);
                if (!result)
                {
                    this.ShowErr(result);
                }

                #endregion

                #region 表身資料
                pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", this.currentID));
                result = DBProxy.Current.Select(
                    string.Empty,
                    @"
select 	 
		[Refno] = a.Refno 
		,[Delivery] = a.delivery
        ,[Description] = dbo.getItemDesc(b.Category,a.Refno)
		,[Order_Qty] = sum(a.Qty) 
		,[Unit] = a.UnitId 
        ,[UPrice] = a.Price 
        ,[Amount] = format(Cast(sum(a.Price*a.Qty) as decimal(20,4)),'#,###,###,##0.0000') 
        ,b.apvname,b.Lockname
into #temp
from dbo.LocalPO_Detail a WITH (NOLOCK) 
left join dbo.LocalPO b WITH (NOLOCK) on  a.id=b.id
where a.id=@ID
group by a.refno,b.Category,a.Price ,a.UnitId,a.delivery,b.apvname,b.Lockname

select Sort = ROW_NUMBER() Over (Partition By Delivery Order By Delivery),* 
from #temp
order by delivery,refno
", pars, out this.dtBody);
                if (!result)
                {
                    this.ShowErr(result);
                }
                #endregion
            }

            // To Excel Coats
            else
            {
                #region excel
                string sqlcmd = string.Empty;
                sqlcmd = string.Format(
                    @"
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
order by orderid,a.refno,threadcolorid", this.currentID);

                this.result = DBProxy.Current.Select(string.Empty, sqlcmd, out this.dtexcel);
                if (!this.result)
                {
                    return this.result;
                }
                #endregion
            }

            return Result.True;
        }

        // To Rdlc Print
        protected override bool OnToPrint(ReportDefinition report)
        {
            if ((this.dtBody == null || this.dtBody.Rows.Count == 0) || (this.dtHeader == null || this.dtHeader.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            if (this.radioNmrmalFormat.Checked == true)
            {
                #region 表頭
                string RptTitle = this.dtHeader.Rows[0]["RptTitle"].ToString().Trim();
                string Supplier = this.dtHeader.Rows[0]["Supplier"].ToString().Trim();
                string FactoryID = this.dtHeader.Rows[0]["FactoryID"].ToString().Trim();
                string Tel = this.dtHeader.Rows[0]["Tel"].ToString().Trim();
                string Address = this.dtHeader.Rows[0]["Address"].ToString().Trim();
                string AddressEN = this.dtHeader.Rows[0]["AddressEN"].ToString().Trim();
                string fTel = this.dtHeader.Rows[0]["fTel"].ToString().Trim();
                string Fax = this.dtHeader.Rows[0]["Fax"].ToString().Trim();
                decimal amount = MyUtility.Convert.GetDecimal(this.CurrentDataRow["amount"]);
                decimal vat = MyUtility.Convert.GetDecimal(this.CurrentDataRow["vat"]);
                string CurrencyID = this.CurrentDataRow["CurrencyID"].ToString();
                string vatrate = this.CurrentDataRow["vatrate"].ToString() + "%";
                string Remark = this.CurrentDataRow["remark"].ToString();

                decimal Total = (decimal)this.CurrentDataRow["amount"] + (decimal)this.CurrentDataRow["vat"];
                report = new ReportDefinition();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", this.currentID));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", this.currentdate));
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
                List<P30_PrintData> data = this.dtBody.AsEnumerable()
                    .Select(row1 => new P30_PrintData()
                    {
                        Sort = row1["Sort"].ToString().Trim(),
                        SP = row1["SP"].ToString().Trim(),
                        Delivery = (row1["Delivery"] == DBNull.Value) ? string.Empty : Convert.ToDateTime(row1["Delivery"]).ToShortDateString().Trim(),
                        Refno = row1["Refno"].ToString().Trim(),
                        Color_Shade = row1["Color_Shade"].ToString().Trim(),
                        Description = row1["Description"].ToString().Trim(),
                        UPrice = row1["UPrice"].ToString().Trim(),
                        Order_Qty = Convert.ToDecimal(row1["Order_Qty"]),
                        Unit = row1["Unit"].ToString().Trim(),
                        Amount = Convert.ToDecimal(row1["Amount"]),
                    }).ToList();

                data[0].ApvName = Production.Class.UserESignature.getUserESignature(this.dtBody.Rows[0]["apvname"].ToString(), 207, 83);
                data[0].Lockname = Production.Class.UserESignature.getUserESignature(this.dtBody.Rows[0]["LockName"].ToString(), 207, 83);

                report.ReportDataSource = data;
                #endregion
            }
            else
            {
                #region 表頭
                string RptTitle = this.dtHeader.Rows[0]["RptTitle"].ToString().Trim();
                string Supplier = this.dtHeader.Rows[0]["Supplier"].ToString().Trim();
                string Tel = this.dtHeader.Rows[0]["Tel"].ToString().Trim();
                string AddressEN = this.dtHeader.Rows[0]["AddressEN"].ToString().Trim();
                string fTel = this.dtHeader.Rows[0]["fTel"].ToString().Trim();
                string Fax = this.dtHeader.Rows[0]["Fax"].ToString().Trim();
                decimal amount = MyUtility.Convert.GetDecimal(this.CurrentDataRow["amount"]);
                string CurrencyID = this.CurrentDataRow["CurrencyID"].ToString();
                decimal vat = MyUtility.Convert.GetDecimal(this.CurrentDataRow["vat"]);
                string Remark = this.CurrentDataRow["remark"].ToString();
                decimal Total = (decimal)this.CurrentDataRow["amount"] + (decimal)this.CurrentDataRow["vat"];
                string AddName = this.dtHeader.Rows[0]["AddName"].ToString();
                report = new ReportDefinition();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", this.currentID));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", this.currentdate));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Delivery", string.Empty));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", Supplier));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Tel", Tel));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddressEN", AddressEN));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("fTel", fTel));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Fax", Fax));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("amount", amount.ToString("#,0.00")));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("total", Total.ToString("#,0.00")));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("currency", CurrencyID));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("remark", Remark));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddName", AddName));

                #endregion

                #region 表身

                // 傳 list 資料
                List<P30_PrintData> data = this.dtBody.AsEnumerable()
                    .Select(row1 => new P30_PrintData()
                    {
                        Sort = row1["Sort"].ToString().Trim(),
                        Refno = row1["Refno"].ToString().Trim(),
                        Delivery = (row1["Delivery"] == DBNull.Value) ? string.Empty : Convert.ToDateTime(row1["Delivery"]).ToShortDateString().Trim(),
                        Description = row1["Description"].ToString().Trim(),
                        UPrice = row1["UPrice"].ToString().Trim(),
                        Order_Qty = Convert.ToDecimal(row1["Order_Qty"]),
                        Unit = row1["Unit"].ToString().Trim(),
                        Amount = Convert.ToDecimal(row1["Amount"]),
                    }).ToList();

                data[0].ApvName = Production.Class.UserESignature.getUserESignature(this.dtBody.Rows[0]["apvname"].ToString(), 207, 83);
                data[0].Lockname = Production.Class.UserESignature.getUserESignature(this.dtBody.Rows[0]["LockName"].ToString(), 127, 83);

                report.ReportDataSource = data;
                #endregion
            }

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            Type ReportResourceNamespace = typeof(P30_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = this.radioNmrmalFormat.Checked ? "P30_Print.rdlc" : "P30_Print_ByRefno.rdlc";

            IReportResource reportresource;
            if (!(this.result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                this.ShowException(this.result);
                return this.result;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.TopMost = true;
            frm.Show();
            this.HideWaitMessage();

            return true;
        }

        // To Excel
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtexcel == null || this.dtexcel.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dtexcel.Rows.Count);
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_P30.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            MyUtility.Excel.CopyToXls(this.dtexcel, string.Empty, "Subcon_P30.xltx", 1, false, null, objApp);      // 將datatable copy to excel

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

        // 變更radio後調整畫面顯示
        private void radioPanel1_ValueChanged(object sender, EventArgs e)
        {
            if (this.radioCoatsOrderFormat.Checked == true)
            {
                this.print.Visible = false;
                this.toexcel.Visible = true;
            }
            else
            {
                this.print.Visible = true;
                this.toexcel.Visible = false;
            }
        }
    }
}
