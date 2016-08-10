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
       
        string name;
        string front;
        string mb;
        string left;
        string right;
        //DataRow CurrentDataRow;
        //public R26(DataRow row)
        //{

        //    this.CurrentDataRow = row;
        //}
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
            this.checkBox1.Enabled = false;
        }
        List<SqlParameter> lis = new List<SqlParameter>();
        string sqlWhere = "";
        List<string> sqlWheres = new List<string>();
        DataTable dtt;
        DualResult result;
        DataTable dt;
        DataTable da;
        DataTable shm;
        DateTime? SCI_Delivery;
        DateTime? SCI_Delivery2;
        DateTime? Issue_Date1;
        DateTime? Issue_Date2;
        string SP;
        string SP2;
        string Location_Poid;
        string Location_Poid2;
        string Factory1;
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
            SP2 = textBox2.Text.ToString();
            Location_Poid = textBox3.Text.ToString();
            Location_Poid2 = textBox4.Text.ToString();
            Factory1 = comboBox2.Text.ToString();
            Category = txtartworktype_fty1.Text.ToString();
            Supplier = txtsubcon1.TextBox1.Text;
            Report_Type = comboBox1.SelectedItem.ToString();
            Shipping_Mark = checkBox1.Checked.ToString();

            #region where 條件

            if (!this.SP.Empty() && !this.SP2.Empty())
            {
                sqlWheres.Add("c.orderid between @SP and @SP2");
                lis.Add(new SqlParameter("@SP", SP));
                lis.Add(new SqlParameter("@SP2", SP2));
            }
            if (!this.SCI_Delivery.Empty() && !this.SCI_Delivery2.Empty())
            {
                sqlWheres.Add("a.scidelivery between @SCI_Delivery and @SCI_Delivery2");
                lis.Add(new SqlParameter("@SCI_Delivery", SCI_Delivery));
                lis.Add(new SqlParameter("@SCI_Delivery2", SCI_Delivery2));
            }
            if (!this.Issue_Date1.Empty() && !this.Issue_Date2.Empty())
            {
                sqlWheres.Add("b.issuedate between @Issue_Date1 and @Issue_Date2");
                lis.Add(new SqlParameter("@Issue_Date1", Issue_Date1));
                lis.Add(new SqlParameter("@Issue_Date2", Issue_Date2));
            }
            if (!this.Location_Poid.Empty() && !this.Location_Poid2.Empty())
            {
                sqlWheres.Add("b.id between @Location_Poid and @Location_Poid2");
                lis.Add(new SqlParameter("@Location_Poid", Location_Poid));
                lis.Add(new SqlParameter("@Location_Poid2", Location_Poid2));
            }
            if (!this.Factory1.Empty())//(Factory != "")
            {
                sqlWheres.Add("b.factoryid =@Factory");
                lis.Add(new SqlParameter("@Factory", Factory1));
            }
            if (!this.Supplier.Empty())
            {
                sqlWheres.Add("b.localsuppid =@Supplier");
                lis.Add(new SqlParameter("@Supplier", Supplier));
            }
            if (!this.Category.Empty())
            {
                sqlWheres.Add("b.category =@Category");
                lis.Add(new SqlParameter("@Category", Category));
            }

            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }
            #endregion

            if (this.Report_Type == "PO List")
            #region Po List
            {
                string sqlcd = string.Format(@"select  a.FactoryID [Factory]
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
                                           left join dbo.LocalSupp d on b.LocalSuppID=d.ID  " + sqlWhere);
                result = DBProxy.Current.Select("", sqlcd, lis, out dtt);
                if (!result)
                {
                    return result;
                }
            #endregion
            }
            else if (this.Report_Type == "PO Order")
            {
                #region Po Order
                string cmd = string.Format(@"select  b.id [LocalPOID]
                                                ,b.FactoryId [Factory]
		                                        ,b.FactoryId+'-'+b.Id [TheOrderID]--bb.TheOrderID
                                                ,c.OrderId [SP]
                                                ,b.LocalSuppID [Supp]
                                                ,c.Delivery [Delivery]
                                                ,c.Refno [Code]
                                                ,c.ThreadColorID [Color_Shade]
                                                ,b.IssueDate [Issue_Date]
                                                ,[Description]=dbo.getItemDesc(b.Category,c.Refno)
                                                ,sum(c.Qty) [Order_Qty]
                                                ,c.UnitId [Unit]
                                                ,sum(c.price) [Price]
                                                ,sum(c.qty * c.price) [Amount]
                                                ,sum(c.InQty) [In-Coming]
                                                ,sum(c.APQty) [AP_Qty]
                                       from dbo. LocalPO b
                                       inner join Orders a on b.FactoryId = a.FactoryID
                                       left join dbo.LocalPO_Detail c on b.id=c.Id
                                       " + sqlWhere + @"
		                               group by b.id, b.FactoryId, c.OrderId, b.LocalSuppID, c.Delivery, c.Refno, c.ThreadColorID, b.IssueDate, b.Category, c.Refno, c.UnitId
		                               order by b.id, b.FactoryId");
                result = DBProxy.Current.Select("", cmd, lis, out da);
                if (!result)
                {
                    return result;
                }
                #endregion
            }
            else //if (this.comboBox1.Text == "PO Form")
            {
                #region PO Form
                //DataRow row = this.CurrentDataRow;
                //string id = row["ID"].ToString();
                //    List<SqlParameter> pars = new List<SqlParameter>();
                //pars.Add(new SqlParameter("@ID", id));

                result = DBProxy.Current.Select("", @"select e.NameEN [Title1] 
                                                       ,e.AddressEN [Title2]
                                                       ,e.Tel [Title3]
                                                       ,b.LocalSuppID [To]
                                                       ,d.Tel [Tel]
                                                       ,d.Fax [Fax]
                                                       ,b.IssueDate [Issue_Date]
                                                       ,c.OrderId [PO]
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
                                             inner join dbo.orders a on a.FactoryID=b.FactoryId
                                             left join dbo.Factory  e on e.id = b.factoryid 
	                                         left join dbo.LocalPO_Detail c on b.id=c.Id
	                                         left join dbo.LocalSupp d on b.LocalSuppID=d.ID " + sqlWhere, lis, out dt);
                //where b.id = @ID 
                if (!result) { this.ShowErr(result); }
                //if (!result)
                //{
                //    return result;
                //}



                #endregion
            }

            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
         


           

            string Title1 = dt.Rows[0]["Title1"].ToString();
            string Title2 = dt.Rows[0]["Title2"].ToString();
            string Title3 = dt.Rows[0]["Title3"].ToString();
            string To = dt.Rows[0]["To"].ToString();
            string Tel = dt.Rows[0]["Tel"].ToString();
            string Fax = dt.Rows[0]["Fax"].ToString();
            string Issue_Date = dt.Rows[0]["Issue_Date"].ToString();
            string Total1 = dt.Rows[0]["Total1"].ToString();
            string Total2 = dt.Rows[0]["Total2"].ToString();
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


            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(R26_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "R26_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
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




            if (Category.TrimEnd().Equals("CARTON", StringComparison.OrdinalIgnoreCase) && checkBox1.Checked == true)
            #region Shipping Mark
            {
                string scmd = string.Format(@"select a.id 
                                                    ,co.alias [name]
                                                    ,a.MarkFront [front]
                                                    ,a.MarkBack [mb]
                                                    ,a.markleft [left]
                                                    ,a.Markright [right]
                                             from orders a
                                             left join (select distinct OrderId from +sqlwhere) m  on m.OrderId = a.poid 
                                             left join country co on co.id = a.dest");

                result = DBProxy.Current.Select("", scmd, lis, out shm);
                if (!result)
                {
                    return result;
                }
                name = shm.Rows[0]["name"].ToString();
                front = shm.Rows[0]["front"].ToString();
                mb = shm.Rows[0]["mb"].ToString();
                left = shm.Rows[0]["left"].ToString();
                right = shm.Rows[0]["right"].ToString();
            }
            #endregion
           
            return result;//base.OnAsyncDataLoad(e);  
        }


        
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PO List")
            {
                print.Enabled = false;
                toexcel.Enabled = true;

            }
            else if (comboBox1.Text == "PO Form")
            {
                toexcel.Enabled = false;
                print.Enabled = true;
            }
            else if (comboBox1.Text == "PO Order")
            {
                print.Enabled = false;
                toexcel.Enabled = true;

            }
        }
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.Report_Type == "PO List" && (dtt == null || dtt.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            } if (this.Report_Type == "PO Order" && (da == null || da.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            } if (this.Report_Type == "PO Form" && (shm == null || shm.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

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

                List<string> lis = new List<string>();
                foreach (DataRow row in da.Rows)
                {
                    string TheOrderID = row["TheOrderID"].ToString();
                    if (!lis.Contains(TheOrderID)) //lis "不"包含 TheOrderID
                        lis.Add(TheOrderID);
                }

                //copy sheet by TheOrderID count.
                x1.CopySheet.Add(1, lis.Count - 1);
                x1.VarToSheetName = "##theorderid";
                int idx = 0;
                foreach (string TheOrderID in lis)
                {
                    string idxstr = (idx == 0) ? "" : idx.ToString(); //為了讓第一筆idx是空值

                    DataTable finalda = da.Select(string.Format("TheOrderID = '{0}'", TheOrderID)).CopyToDataTable();

                    finalda.Columns.RemoveAt(2);
                    finalda.Columns.RemoveAt(1);
                    finalda.Columns.RemoveAt(0);

                    x1.dicDatas.Add("##LocalPOID" + idxstr, Location_Poid);
                    x1.dicDatas.Add("##Factory" + idxstr, Factory1);
                    x1.dicDatas.Add("##theorderid" + idxstr, TheOrderID);
                    x1.dicDatas.Add("##date" + idxstr, date);
                    x1.dicDatas.Add("##SP" + idxstr, finalda);
                    idx += 1;
                }

                x1.Save(outpath, false);

            }

            if (checkBox1.Checked == true)
            {
                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R26_Shipping_Mark.xltx");

                x1.dicDatas.Add("##name", name);
                x1.dicDatas.Add("##A", front);
                x1.dicDatas.Add("##B", mb);
                x1.dicDatas.Add("##C", left);
                x1.dicDatas.Add("##D", right);
                x1.Save(outpath, false);
            }
           return true; //return base.OnToExcel(report);
        } 
        private void txtartworktype_fty1_TextChanged(object sender, EventArgs e)
        {
            if (txtartworktype_fty1.Text.TrimEnd().Equals("CARTON", StringComparison.OrdinalIgnoreCase))
            //if (txtartworktype_fty1.Text.EqualString ("CARTON"))
            {
                checkBox1.Enabled = true;
            }
            else
            {
                checkBox1.Enabled = false;
            }
        }

        //public string test()
        //{
        //    string ss;
        //    if (comboBox1.Text == "PO List")
        //    {
        //        print.Enabled = false;
        //        toexcel.Enabled = true;
        //        ss = "A";
        //    }
        //    else if (comboBox1.Text == "PO Form")
        //    {
        //        toexcel.Enabled = false;
        //        print.Enabled = true;
        //        ss = "B";
        //    }
        //    else //if (comboBox1.Text == "PO Order")
        //    {
        //        print.Enabled = false;
        //        toexcel.Enabled = true;
        //        ss = "C";
        //    }
        //    return ss;
    }
}

