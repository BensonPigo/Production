using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;

// using Ict.Win.Tools;
namespace Sci.Production.Subcon
{
    public partial class R26 : Win.Tems.PrintForm
    {
        public R26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select DISTINCT ftygroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            this.comboReportType.SelectedIndex = 0;
            this.checkShippingMark.Enabled = false;
        }

        private List<SqlParameter> lis;
        private string sqlWhere = string.Empty;
        private string all = string.Empty;
        private List<string> sqlWheres = new List<string>();
        private DataTable dtt;
        private DualResult result;
        private DataTable dt;
        private DataTable da;
        private DataTable shm;
        private DateTime? SCI_Delivery;
        private DateTime? SCI_Delivery2;
        private DateTime? Issue_Date1;
        private DateTime? Issue_Date2;
        private DateTime? Delivery_Date_start;
        private DateTime? Delivery_Date_end;
        private string SP;
        private string SP2;
        private string Location_Poid;
        private string Location_Poid2;
        private string Factory1;
        private string Category;
        private string Supplier;
        private string Report_Type;
        private string Shipping_Mark;
        private string date = DateTime.Now.ToShortDateString();
        private string id;
        private string name;
        private string A;
        private string B;
        private string C;
        private string D;
        private string sqlByPoOrder;

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateSCIDelivery.HasValue && !this.dateIssueDate.HasValue && !this.dateDeliveryDate.HasValue)
            {
                MyUtility.Msg.ErrorBox("[SCI Delivery] or [Issue_Date] or [Delivery] one of the inputs must be selected");
                this.dateSCIDelivery.Focus();

                return false;
            }

            if (this.comboReportType.SelectedItem.Empty())
            {
                MyUtility.Msg.ErrorBox("[Report_Type] can't empty!!");
                this.txtSPNoStart.Focus();
                return false;
            }

            this.lis = new List<SqlParameter>();
            this.SCI_Delivery = this.dateSCIDelivery.Value1;
            this.SCI_Delivery2 = this.dateSCIDelivery.Value2;
            this.Issue_Date1 = this.dateIssueDate.Value1;
            this.Issue_Date2 = this.dateIssueDate.Value2;
            this.Delivery_Date_start = this.dateDeliveryDate.Value1;
            this.Delivery_Date_end = this.dateDeliveryDate.Value2;

            this.SP = this.txtSPNoStart.Text.ToString();
            this.SP2 = this.txtSPNoEnd.Text.ToString();
            this.Location_Poid = this.txtLocalPoidStart.Text.ToString();
            this.Location_Poid2 = this.txtLocalPoidEnd.Text.ToString();
            this.Factory1 = this.comboFactory.Text.ToString();
            this.Category = this.txtartworktype_ftyCategory.Text.ToString();
            this.Supplier = this.txtsubconSupplier.TextBox1.Text.ToString();
            this.Report_Type = this.comboReportType.SelectedItem.ToString();
            this.Shipping_Mark = this.checkShippingMark.Checked.ToString();

            this.sqlByPoOrder = string.Empty;
            this.sqlWheres.Clear();

            #region where 條件
            if (this.checkBoxNoClosed.Checked)
            {
                this.sqlWheres.Add("a.Status<>'Closed'");
            }

            if (!this.SP.Empty() && !this.SP2.Empty())
            {
                this.sqlWheres.Add("b.orderid between @SP and @SP2");
                this.lis.Add(new SqlParameter("@SP", this.SP));
                this.lis.Add(new SqlParameter("@SP2", this.SP2));
            }

            if (!this.SCI_Delivery.Empty())
            {
                this.sqlWheres.Add("c.scidelivery >= @SCI_Delivery");
                this.lis.Add(new SqlParameter("@SCI_Delivery", this.SCI_Delivery));
            }

            if (!this.SCI_Delivery2.Empty())
            {
                this.sqlWheres.Add("c.scidelivery <= @SCI_Delivery2");
                this.lis.Add(new SqlParameter("@SCI_Delivery2", this.SCI_Delivery2));
            }

            if (!this.Issue_Date1.Empty())
            {
                this.sqlWheres.Add("a.issuedate >= @Issue_Date1");
                this.lis.Add(new SqlParameter("@Issue_Date1", this.Issue_Date1));
            }

            if (!this.Issue_Date2.Empty())
            {
                this.sqlWheres.Add("a.issuedate <= @Issue_Date2");
                this.lis.Add(new SqlParameter("@Issue_Date2", this.Issue_Date2));
            }

            if (!this.Delivery_Date_start.Empty())
            {
                this.sqlWheres.Add("b.Delivery >= @Delivery_Date_start");
                this.lis.Add(new SqlParameter("@Delivery_Date_start", this.Delivery_Date_start));
            }

            if (!this.Delivery_Date_end.Empty())
            {
                this.sqlWheres.Add("b.Delivery <= @Delivery_Date_end");
                this.lis.Add(new SqlParameter("@Delivery_Date_end", this.Delivery_Date_end));
            }

            if (!this.Location_Poid.Empty() && !this.Location_Poid2.Empty())
            {
                this.sqlWheres.Add("a.id between @Location_Poid and @Location_Poid2");
                this.lis.Add(new SqlParameter("@Location_Poid", this.Location_Poid));
                this.lis.Add(new SqlParameter("@Location_Poid2", this.Location_Poid2));
            }

            if (!this.Factory1.Empty()) // (Factory != "")
            {
                this.sqlWheres.Add("a.factoryid =@Factory");
                this.lis.Add(new SqlParameter("@Factory", this.Factory1));
            }

            if (!this.Supplier.Empty())
            {
                this.sqlWheres.Add("a.localsuppid =@Supplier");
                this.lis.Add(new SqlParameter("@Supplier", this.Supplier));
            }

            if (!this.Category.Empty())
            {
                this.sqlWheres.Add("a.category =@Category");
                this.lis.Add(new SqlParameter("@Category", this.Category));
            }

            this.sqlWhere = string.Join(" and ", this.sqlWheres);
            if (!this.sqlWhere.Empty())
            {
                if (this.Report_Type != "PO Order")
                {
                    if (this.rdbtn_payment.Checked)
                    {
                        this.sqlWhere = @" where b.qty > b.APQty and " + this.sqlWhere;
                    }
                    else if (this.rdbtn_incoming.Checked)
                    {
                        this.sqlWhere = @" where b.qty > b.InQty and " + this.sqlWhere;
                    }
                    else if (this.rdbtn_PandI.Checked)
                    {
                        this.sqlWhere = @" where (b.qty > b.APQty or b.qty > b.InQty)and " + this.sqlWhere;
                    }
                    else
                    {
                        this.sqlWhere = " where " + this.sqlWhere;
                    }
                }
                else
                {
                    if (this.rdbtn_payment.Checked)
                    {
                        this.sqlWhere = @" where " + this.sqlWhere;
                        this.sqlByPoOrder = "having  sum(b.Qty) >  sum(b.APQty)";
                    }
                    else if (this.rdbtn_incoming.Checked)
                    {
                        this.sqlWhere = @" where " + this.sqlWhere;
                        this.sqlByPoOrder = "having  sum(b.Qty) >  sum(b.InQty)";
                    }
                    else if (this.rdbtn_PandI.Checked)
                    {
                        this.sqlWhere = @" where " + this.sqlWhere;
                        this.sqlByPoOrder = "having  sum(b.Qty) >  sum(b.APQty) or sum(b.Qty) >  sum(b.InQty) ";
                    }
                    else
                    {
                        this.sqlWhere = " where " + this.sqlWhere;
                    }
                }
            }

            this.all = @"
select  Title1
        ,Title2
        ,Title3
        ,To#
        ,Tel
        ,Fax
        ,Issue_Date
        ,Delivery
        ,PO
        ,Code
        ,Color_Shade
        ,Description
        ,Quantity
        ,Unit
        ,Unit_Price
        ,Amount
        ,[AccuAmount] = sum(Amount) Over (PARTITION BY to#, Title1, Issue_Date 
                                           Order by to#, Title1, Issue_Date, Delivery, PO, Code
                                           rows between unbounded preceding and Current Row)
        ,[Total_Quantity] = sum(Quantity) over (PARTITION BY to#, Title1, Issue_Date, Delivery, TO#, PO)
        ,Remark
        --,[Total1]=SUM(Amount)OVER (PARTITION BY to#,po,Issue_Date,Delivery) * VatRate
        ,[Total1] = sum(Amount * VatRate / 100) Over (PARTITION BY Issue_Date, Delivery 
                                                        Order by Issue_Date, Delivery, to#, Title1, PO, Code 
                                                        rows between unbounded preceding and Current Row)
                     
        ,[Total2]=SUM(amount)OVER (PARTITION BY to#,po,Issue_Date,Delivery) 
        ,CurrencyId
        ,VatRate
        ,[Grand_Total] = sum(Amount) OVER (PARTITION BY to#,po,Issue_Date,Delivery) 
                         + SUM(Amount * VatRate / 100)OVER (PARTITION BY to#,po,Issue_Date,Delivery) 
        ,ID
from #temp
order by to#, Title1, Issue_Date, Delivery, PO, Code
drop table #temp";
            #endregion

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            if (this.Report_Type == "PO List")
            {
                #region Po List
                string sqlcd = string.Format(@"
select DISTINCT c.FactoryID
	        ,a.FactoryId
	        ,b.OrderId
	        ,c.StyleID
            ,c.SciDelivery
            ,c.BuyerDelivery
            ,c.SewInLine
            ,c.brandid
	        ,c.SeasonID
	        ,[Supp] = a.LocalSuppID+'-'+d.Abb 
	        ,b.Delivery
	        ,b.Refno
            ,Category = Iif(a.category = 'CARTON' AND iscarton = 0, 'CARDBOARD', a.category) 
            ,[IsCarton] = iif(li.IsCarton = 1 ,'Y','')
	        ,b.ThreadColorID + ' - ' + ThreadColor.Description		
			,[PRConfirmedDate]=CASE WHEN a.Category = 'CARTON' 
									THEN (SELECT TOP 1 p.ApvToPurchaseDate 
											FROM PackingList p
											WHERE p.ID=b.RequestID)
									WHEN a.Category = 'SP_THREAD' 
									THEN (SELECT TOP 1 t.EditDate
											FROM ThreadRequisition t
											WHERE t.OrderID=b.RequestID )
									ELSE NULL
								END
	        ,a.IssueDate
	        ,[Description] = dbo.getItemDesc(a.Category,b.Refno) 
	        ,b.qty
	        ,b.UnitId
            ,a.CurrencyID 
	        ,b.Price
	        ,[Amount] = b.Qty*b.Price
            ,x.KPIRate
            ,[Amount (USD)] = iif(x.KPIRate = 0, 0, b.Qty*b.Price/x.KPIRate)
	        ,b.InQty
            ,b.qty-b.InQty
	        ,b.APQty
            ,a.id
			,a.Status
			,rec.IssueDate
            ,b.RequestID
	        ,b.Remark
from localpo a WITH (NOLOCK) 
inner join LocalPO_Detail b WITH (NOLOCK) on a.id=b.id
left join orders c WITH (NOLOCK) on c.ID = b.OrderId
left join localsupp d  WITH (NOLOCK) on  d.id =a.LocalSuppID 
left join ThreadColor on b.ThreadColorID = ThreadColor.ID
left join LocalItem li WITH (NOLOCK) on li.RefNo=b.Refno
outer apply(select KPIRate = dbo.getrate('KP','USD',a.CurrencyID ,a.IssueDate))x
OUTER APPLY(
	SELECT TOP 1 l.IssueDate
	FROM LocalReceiving_Detail LD
	INNER JOIN LocalReceiving L on L.Id=LD.Id 
	WHERE LD.LocalPo_detailukey = b.Ukey
	ORDER BY l.AddDate DESC
)rec
" + this.sqlWhere);
                this.result = DBProxy.Current.Select(string.Empty, sqlcd, this.lis, out this.dtt);
                if (!this.result)
                {
                    return this.result;
                }
                #endregion
            }
            else if (this.Report_Type == "PO Order")
            {
                #region Po Order
                string cmd = string.Format(@"
select  [LocalPOID] = a.id 
        ,[Factory] = a.FactoryId 
		,[TheOrderID] = a.FactoryId+'-'+a.Id 
        ,[SP] = b.OrderId 
        ,[Supp] = a.LocalSuppID  
        ,[Delivery] = b.Delivery 
        ,[Code] = b.Refno 
        ,[Color_Shade] = b.ThreadColorID + ' - ' + ThreadColor.Description
        ,[Issue_Date] = a.IssueDate 
        ,[Description] = dbo.getItemDesc(a.Category,b.Refno)
        ,[Order_Qty] = sum(b.Qty) 
        ,[Unit] = b.UnitId 
        ,[Price] = sum(b.price) 
        ,[Amount] = sum(b.qty * b.price) 
        ,[In-Coming] = sum(b.InQty) 
        ,[AP_Qty] = sum(b.APQty) 
into #tmp
from localpo a WITH (NOLOCK) 
inner join LocalPO_Detail b WITH (NOLOCK) on a.id=b.id
left join orders c WITH (NOLOCK) on c.id=b.OrderId
left join ThreadColor on b.ThreadColorID = ThreadColor.ID
" + this.sqlWhere + @"
group by a.id, a.FactoryId, b.OrderId,a.LocalSuppID, b.Delivery, b.Refno, b.ThreadColorID + ' - ' + ThreadColor.Description, a.IssueDate, a.Category, b.UnitId "
+ this.sqlByPoOrder + @"
order by a.id, a.FactoryId, b.OrderId,a.LocalSuppID, b.Delivery, b.Refno, b.ThreadColorID + ' - ' + ThreadColor.Description, a.IssueDate, a.Category, b.UnitId
;

select  tmp.LocalPOID
        , tmp.Factory
		, tmp.TheOrderID
        , SP = IIF(Lag(tmp.SP, 1, 0) over (Partition By tmp.LocalPOID ,tmp.SP  order by tmp.SP) = tmp.SP, ''
                                                                                                , tmp.SP) 
        , tmp.Supp
        , tmp.Delivery
        , tmp.Code
        , tmp.Color_Shade
        , tmp.Issue_Date
        , tmp.Description
        , tmp.Order_Qty
        , tmp.Unit
        , tmp.Price
        , tmp.Amount
        , tmp.[In-Coming]
        ,tmp.Order_Qty-tmp.[In-Coming] as 'On Road/Balance'
        , tmp.AP_Qty
from #tmp tmp;

drop table #tmp;
");

                this.result = DBProxy.Current.Select(string.Empty, cmd, this.lis, out this.da);
                if (!this.result)
                {
                    return this.result;
                }
                #endregion
            }
            else
            {
                // if (this.comboBox1.Text == "PO Form")
                #region PO Form
                this.result = DBProxy.Current.Select(string.Empty, @"
select  e.NameEN [Title1] 
        ,e.AddressEN [Title2]
        ,e.Tel [Title3]
        ,a.LocalSuppID+'-'+d.Abb [To#]
        ,d.Tel [Tel]
        ,d.Fax [Fax]
        ,format(a.IssueDate,'yyyy/MM/dd') [Issue_Date]
        ,b.OrderId [PO]
        ,b.Refno [Code]
        ,b.ThreadColorID [Color_Shade]
        ,[Description]=dbo.getItemDesc(a.Category,b.Refno)
        ,b.Qty [Quantity]
        ,b.UnitId [Unit]
        ,cast(cast(isnull(b.Price , 0 ) as float) as varchar)[Unit_Price]
		,cast(isnull(b.Qty*b.Price , 0 ) as float)[Amount]
        ,[Total_Quantity]=sum(b.Qty) OVER (PARTITION BY b.orderid,b.Refno) 
        ,a.Remark [Remark] 
        ,a.CurrencyId [Total1] 
        ,a.Amount [Total2]
        ,a.CurrencyId [currencyid]
        ,a.VatRate [VatRate]
        ,a.Amount+a.Vat [Grand_Total]
        ,format(b.Delivery,'yyyy/MM/dd')[Delivery] 
		,a.id [ID]
		,a.FactoryId [ftyid] 
		,a.LocalSuppID [lospid] 
		,a.Category[Category]   
        into #temp  
from dbo.localpo a WITH (NOLOCK) 
inner join LocalPO_Detail b WITH (NOLOCK) on b.id=a.Id
left join orders c WITH (NOLOCK) on c.id=b.OrderId
left join LocalSupp d WITH (NOLOCK) on a.LocalSuppID=d.ID
left join Factory  e WITH (NOLOCK) on e.id = a.factoryid
" + this.sqlWhere + " " + this.all, this.lis, out this.dt);

                if (!this.result)
                {
                    return this.result;
                }

                ReportDefinition report = e.Report;

                    // 傳 list 資料
                List<R26_PrintData> data = this.dt.AsEnumerable()
                        .Select(row1 => new R26_PrintData()
                        {
                            ID = row1["ID"].ToString().Trim(),
                            PO = row1["PO"].ToString().Trim(),
                            Code = row1["Code"].ToString().Trim(),
                            Color_Shade = row1["Color_Shade"].ToString().Trim(),
                            Description = row1["Description"].ToString().Trim(),
                            Quantity = Convert.ToDecimal(row1["Quantity"].ToString().Trim()),
                            Unit = row1["Unit"].ToString().Trim(),
                            Unit_Price = row1["Unit_Price"].ToString().Trim(),
                            Amount = row1["Amount"].ToString().Trim(),
                            AccuAmount = row1["AccuAmount"].ToString().Trim(),
                            Total_Quantity = row1["Total_Quantity"].ToString().Trim(),
                            Remark = row1["Remark"].ToString().Trim(),
                            Title1 = row1["Title1"].ToString().Trim(),
                            Issue_Date = row1["Issue_Date"].ToString().Trim(),
                            To = row1["To#"].ToString().Trim(),
                            Delivery_Date = row1["Delivery"].ToString().Trim(),
                            Title2 = row1["Title2"].ToString().Trim(),
                            Title3 = row1["Title3"].ToString().Trim(),
                            Tel = row1["Tel"].ToString().Trim(),
                            Fax = row1["Fax"].ToString().Trim(),
                            Total1 = row1["Total1"].ToString().Trim(),
                            Total2 = row1["Total2"].ToString().Trim(),
                            CurrencyId = row1["currencyid"].ToString().Trim(),
                            Vat = row1["VatRate"].ToString().Trim(),
                            Grand_Total = string.Format("{0}", Convert.ToDecimal(row1["AccuAmount"].ToString()) + Convert.ToDecimal(row1["Total1"].ToString())), // row1["Grand_Total"].ToString()
                        }).ToList();

                report.ReportDataSource = data;

                #endregion
            }

            if (this.Category.TrimEnd().Equals("CARTON", StringComparison.OrdinalIgnoreCase) && this.checkShippingMark.Checked == true)
            {
            #region Shipping Mark
                string scmd = string.Format(@"select distinct c.id [id]
                                                    ,co.alias [name]
                                                    ,left(RTRIM(c.id)+' '+RTRIM(co.Alias),30) [theorderid]
                                                    ,c.MarkFront [A]
                                                    ,c.MarkBack [B]
                                                    ,c.markleft [C]
                                                    ,c.Markright [D]
                                             from orders c WITH (NOLOCK) 
                                             inner join (select distinct OrderId from localpo a WITH (NOLOCK) 
                                             inner join localpo_detail b WITH (NOLOCK) on a.id = b.id
                                             left join Orders c WITH (NOLOCK) on c.id=b.poid  " + this.sqlWhere + @") m  on m.OrderId = c.poid 
                                             inner join country co WITH (NOLOCK) on co.id = c.dest");
                this.result = DBProxy.Current.Select(string.Empty, scmd, this.lis, out this.shm);

                if (!this.result)
                {
                    return this.result;
                }
            }
            #endregion

            return this.result; // base.OnAsyncDataLoad(e);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.Report_Type == "PO List" && (this.dtt == null || this.dtt.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            if (this.Report_Type == "PO Order" && (this.da == null || this.da.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            if (this.Report_Type == "PO Form" && (this.shm == null || this.shm.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            #region PO List
            if ("PO List".EqualString(this.comboReportType.Text))
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R26_Local_PO_List.xltx"); // 預先開啟excel app

                // MyUtility.Excel.CopyToXls(dtt, "", "Subcon_R26_Local_PO_List.xltx", 2, excelApp: objApp, showExcel: false, showSaveMsg: false);      // 將datatable copy to excel
                Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Subcon_R26_Local_PO_List.xltx", objApp);
                com.ColumnsAutoFit = false;
                com.WriteTable(this.dtt, 3);

                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                objSheets.get_Range("B2").ColumnWidth = 9.63;
                objSheets.get_Range("A2").RowHeight = 31.5;

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Subcon_R26_Local_PO_List");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objSheets);    // 釋放sheet
                Marshal.ReleaseComObject(objApp);          // 釋放objApp
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
                return true;
            }
            #endregion

            #region PO Order
            else if ("PO Order".EqualString(this.comboReportType.Text))
            {
                var saveDialog = Utility.Excel.MyExcelPrg.GetSaveFileDialog(Utility.Excel.MyExcelPrg.Filter_Excel);
                Utility.Excel.SaveXltReportCls x1 = new Utility.Excel.SaveXltReportCls("Subcon_R26_Local_PO_Order.xltx");

                List<string> lis = new List<string>();
                List<string> listt = new List<string>();

                // lis "不"包含 TheOrderID
                foreach (DataRow row in this.da.Rows)
                {
                    string theOrderID = row["TheOrderID"].ToString();
                    if (!lis.Contains(theOrderID))
                    {
                        lis.Add(theOrderID);
                    }
                }

                // copy sheet by TheOrderID count.
                x1.CopySheet.Add(1, lis.Count - 1);
                x1.VarToSheetName = "##theorderid";

                int idx = 0;

                foreach (string theOrderID in lis)
                {
                    string idxstr = idx.ToString(); // 為了讓第一筆idx是空值

                    DataTable finalda = this.da.Select(string.Format("TheOrderID = '{0}'", theOrderID)).CopyToDataTable();

                    finalda.Columns.RemoveAt(2);
                    finalda.Columns.RemoveAt(1);
                    finalda.Columns.RemoveAt(0);

                    x1.ExcelApp.Cells[3, 1] = "##SP";
                    x1.ExcelApp.Cells[4, 1] = string.Empty;

                    x1.DicDatas.Add("##LocalPOID" + idxstr, theOrderID.Substring(4));
                    x1.DicDatas.Add("##Factory" + idxstr, this.Factory1);
                    x1.DicDatas.Add("##theorderid" + idxstr, theOrderID);
                    x1.DicDatas.Add("##date" + idxstr, this.date);
                    Utility.Excel.SaveXltReportCls.XltRptTable dt = new Utility.Excel.SaveXltReportCls.XltRptTable(finalda);
                    dt.BoAutoFitColumn = true;
                    x1.DicDatas.Add("##SP" + idxstr, dt);

                    idx += 1;
                }

                x1.Save(Class.MicrosoftFile.GetName("Subcon_R26_Local_PO_Order"));
                return true;
            }
            #endregion

            #region Shipping Mark
            if (this.checkShippingMark.Checked == true)
            {
                var saveDialog1 = Utility.Excel.MyExcelPrg.GetSaveFileDialog(Utility.Excel.MyExcelPrg.Filter_Excel);
                Utility.Excel.SaveXltReportCls x1 = new Utility.Excel.SaveXltReportCls("Subcon_R26_Shipping_Mark.xltx");

                // copy sheet by TheOrderID count.
                x1.CopySheet.Add(1, this.shm.Rows.Count - 1);
                x1.VarToSheetName = "##theorderid";

                List<string> ls = new List<string>();
                int idx = 0;
                foreach (DataRow row in this.shm.Rows)
                {
                    string idxstr = idx.ToString(); // 為了讓第一筆idx是空值
                    this.id = row["id"].ToString();
                    this.name = row["name"].ToString();
                    this.A = row["A"].ToString();
                    this.B = row["B"].ToString();
                    this.C = row["C"].ToString();
                    this.D = row["D"].ToString();
                    string theorderid = row["theorderid"].ToString();

                    // lis "不"包含 TheOrderID
                    if (!ls.Contains(theorderid))
                    {
                        ls.Add(theorderid);
                    }

                    x1.DicDatas.Add("##id" + idxstr, this.id);
                    x1.DicDatas.Add("##name" + idxstr, this.name);
                    x1.DicDatas.Add("##theorderid" + idxstr, theorderid);
                    x1.DicDatas.Add("##A" + idxstr, this.A);
                    x1.DicDatas.Add("##B" + idxstr, this.B);
                    x1.DicDatas.Add("##C" + idxstr, this.C);
                    x1.DicDatas.Add("##D" + idxstr, this.D);
                    idx += 1;
                }

                x1.Save(Class.MicrosoftFile.GetName("Subcon_R26_Shipping_Mark"));
                return true;
            }
            #endregion
            return true; // return base.OnToExcel(report);
        }

        private void Txtartworktype_ftyCategory_TextChanged(object sender, EventArgs e)
        {
            this.checkShippingMark.Checked = false;

            if (this.txtartworktype_ftyCategory.Text.TrimEnd().Equals("CARTON", StringComparison.OrdinalIgnoreCase))

            // if (txtartworktype_fty1.Text.EqualString ("CARTON"))
            {
                this.checkShippingMark.Enabled = true;
                this.toexcel.Enabled = true;
            }
            else
            {
                this.checkShippingMark.Enabled = false;
            }
        }

        private void CheckShippingMark_CheckedChanged(object sender, EventArgs e)
        {
            if (this.comboReportType.Text == "PO Form")
            {
                this.toexcel.Enabled = this.checkShippingMark.Checked;
            }
        }

        private void ComboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboReportType.Text == "PO List")
            {
                this.print.Enabled = false;
                this.toexcel.Enabled = true;
            }
            else if (this.comboReportType.Text == "PO Form")
            {
                this.toexcel.Enabled = false;
                this.print.Enabled = true;
                if (this.checkShippingMark.Checked == true)
                {
                    this.toexcel.Enabled = true;
                }
            }
            else if (this.comboReportType.Text == "PO Order")
            {
                this.print.Enabled = false;
                this.toexcel.Enabled = true;
            }
        }

        private void CheckBoxNoClosed_CheckedChanged(object sender, EventArgs e)
        {
            this.rdbtn_payment.Enabled = this.rdbtn_incoming.Enabled = this.rdbtn_PandI.Enabled = this.checkBoxNoClosed.Checked;
            if (!this.checkBoxNoClosed.Checked)
            {
                this.rdbtn_payment.Checked = false;
                this.rdbtn_incoming.Checked = false;
                this.rdbtn_PandI.Checked = false;
            }
        }
    }
}
