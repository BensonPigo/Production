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
//using Ict.Win.Tools;

namespace Sci.Production.Subcon
{
    public partial class R26 : Sci.Win.Tems.PrintForm
    {
       
        public R26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select DISTINCT ftygroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox2, 1, factory);
            comboBox2.Text = Sci.Env.User.Factory;
            this.comboBox1.SelectedIndex = 0;
            this.checkBox1.Enabled = false;
        }
        List<SqlParameter> lis;
        string sqlWhere = "";
        string all = "";
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
        string id;
        string name;
        string A;
        string B;
        string C;
        string D;
        
        protected override bool ValidateInput()
        {
            if (!this.dateRange1.HasValue && !this.dateRange2.HasValue)
            {
                MyUtility.Msg.ErrorBox("[SCI Delivery] or [Issue_Date] one of the inputs must be selected");
                dateRange1.Focus();

                return false;
            }

            if (this.comboBox1.SelectedItem.Empty())
            {
                MyUtility.Msg.ErrorBox("[Report_Type] can't empty!!");
                textBox1.Focus();
                return false;
            }

            lis = new List<SqlParameter>();
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
            Supplier = txtsubcon1.Text.ToString();
            Report_Type = comboBox1.SelectedItem.ToString();
            Shipping_Mark = checkBox1.Checked.ToString();
            
            sqlWheres.Clear();

            #region where 條件

            if (!this.SP.Empty() && !this.SP2.Empty())
            {
                sqlWheres.Add("b.orderid between @SP and @SP2");
                lis.Add(new SqlParameter("@SP", SP));
                lis.Add(new SqlParameter("@SP2", SP2));
            }
            if (!this.SCI_Delivery.Empty() && !this.SCI_Delivery2.Empty())
            {
                sqlWheres.Add("c.scidelivery between @SCI_Delivery and @SCI_Delivery2");
                lis.Add(new SqlParameter("@SCI_Delivery", SCI_Delivery));
                lis.Add(new SqlParameter("@SCI_Delivery2", SCI_Delivery2));
            }
            if (!this.Issue_Date1.Empty() && !this.Issue_Date2.Empty())
            {
                sqlWheres.Add("a.issuedate between @Issue_Date1 and @Issue_Date2");
                lis.Add(new SqlParameter("@Issue_Date1", Issue_Date1));
                lis.Add(new SqlParameter("@Issue_Date2", Issue_Date2));
            }
            if (!this.Location_Poid.Empty() && !this.Location_Poid2.Empty())
            {
                sqlWheres.Add("a.id between @Location_Poid and @Location_Poid2");
                lis.Add(new SqlParameter("@Location_Poid", Location_Poid));
                lis.Add(new SqlParameter("@Location_Poid2", Location_Poid2));
            }
            if (!this.Factory1.Empty())//(Factory != "")
            {
                sqlWheres.Add("a.factoryid =@Factory");
                lis.Add(new SqlParameter("@Factory", Factory1));
            }
            if (!this.Supplier.Empty())
            {
                sqlWheres.Add("a.localsuppid =@Supplier");
                lis.Add(new SqlParameter("@Supplier", Supplier));
            }
            if (!this.Category.Empty())
            {
                sqlWheres.Add("a.category =@Category");
                lis.Add(new SqlParameter("@Category", Category));
            }

            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }
            all = "select Title1,Title2,Title3,To#,Tel,Fax,Issue_Date,Delivery,PO,Code,Color_Shade,Description,Quantity,Unit,Unit_Price,Amount,[Total_Quantity]=sum(Quantity) over (PARTITION BY TO#),Remark,[Total1]=SUM(Amount)OVER (PARTITION BY to#,po,Issue_Date,Delivery)*VatRate,[Total2]=SUM(amount)OVER (PARTITION BY to#,po,Issue_Date,Delivery) ,CurrencyId,VatRate,[Grand_Total]=sum(Amount) OVER (PARTITION BY to#,po,Issue_Date,Delivery)+SUM(Amount)OVER (PARTITION BY to#,po,Issue_Date,Delivery)*VatRate  from #temp";
            #endregion


            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (this.Report_Type == "PO List")
            {
                #region Po List
                string sqlcd = string.Format(@"select DISTINCT c.FactoryID
	                                                 ,a.FactoryId
	                                                 ,b.OrderId
	                                                 ,c.StyleID
	                                                 ,c.SeasonID
	                                                 ,a.LocalSuppID+'-'+d.Abb [Supp]
	                                                 ,b.Delivery
	                                                 ,b.Refno
	                                                 ,b.ThreadColorID
	                                                 ,a.IssueDate
	                                                 ,dbo.getItemDesc(a.Category,b.Refno) [Description]
	                                                 ,b.qty
	                                                 ,b.UnitId
	                                                 ,b.Price
	                                                 ,b.Qty*b.Price[Amount]
	                                                 ,b.InQty
	                                                 ,b.APQty
	                                                 ,b.Remark
                                                       ,c.Delivery
                                            from localpo a
                                            inner join LocalPO_Detail b on a.id=b.id
                                            inner join orders c on c.poid=b.OrderId
                                            left join localsupp d  on  d.id =a.LocalSuppID " + sqlWhere);
                result = DBProxy.Current.Select("", sqlcd, lis, out dtt);
                if (!result)
                { return result; }
                #endregion
            }
            else if (this.Report_Type == "PO Order")
            {
                #region Po Order
                string cmd = string.Format(@"select  a.id [LocalPOID]
                                                    ,a.FactoryId [Factory]
		                                            ,a.FactoryId+'-'+a.Id [TheOrderID]
                                                    ,b.OrderId [SP]
                                                    ,a.LocalSuppID [Supp]
                                                    ,b.Delivery [Delivery]
                                                    ,b.Refno [Code]
                                                    ,b.ThreadColorID [Color_Shade]
                                                    ,a.IssueDate [Issue_Date]
                                                    ,[Description]=dbo.getItemDesc(a.Category,b.Refno)
                                                    ,sum(b.Qty) [Order_Qty]
                                                    ,b.UnitId [Unit]
                                                    ,sum(b.price) [Price]
                                                    ,sum(b.qty * b.price) [Amount]
                                                    ,sum(b.InQty) [In-Coming]
                                                    ,sum(b.APQty) [AP_Qty]
                                       from localpo a
                                       inner join LocalPO_Detail b on a.id=b.id
                                       inner join orders c on c.poid=b.OrderId
                                       left join localsupp d  on  d.id =a.LocalSuppID
                                       " + sqlWhere + @"
		                               group by a.id, a.FactoryId, b.OrderId,a.LocalSuppID, b.Delivery, b.Refno, b.ThreadColorID, a.IssueDate, a.Category, b.Refno, b.UnitId
		                               order by a.id, a.FactoryId");
                
                
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
                result = DBProxy.Current.Select("", @"select distinct e.NameEN [Title1] 
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
													   ,a.id [id]
													   ,a.FactoryId [ftyid] 
													   ,a.LocalSuppID [lospid] 
													   ,a.Category[Category]   
                                                        into #temp  
	                                         from dbo.localpo a 
											 inner join LocalPO_Detail b on b.id=a.Id
                                             inner join orders c on c.poid=b.OrderId
											 left join LocalSupp d on a.LocalSuppID=d.ID
                                             left join Factory  e on e.id = a.factoryid" + sqlWhere+" "+all, lis, out dt);


                if (!result )
                {
                    return result;
                }

                 
                ReportDefinition report = e.Report;

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
                            Remark = row1["Remark"].ToString(),
                            Title1 = row1["Title1"].ToString(),
                            Issue_Date = row1["Issue_Date"].ToString(),
                            To = row1["To#"].ToString(),
                            Delivery_Date = row1["Delivery"].ToString(),
                            Title2 = row1["Title2"].ToString(),
                            Title3 = row1["Title3"].ToString(),
                            Tel = row1["Tel"].ToString(),
                            Fax = row1["Fax"].ToString(),
                            Total1 = row1["Total1"].ToString(),
                            Total2 = row1["Total2"].ToString(),
                            CurrencyId = row1["currencyid"].ToString(),
                            vat = row1["VatRate"].ToString(),
                            Grand_Total =row1["Grand_Total"].ToString()
                        }).ToList();

                    report.ReportDataSource = data;
               
                #endregion
            }


            if (Category.TrimEnd().Equals("CARTON", StringComparison.OrdinalIgnoreCase) && checkBox1.Checked == true)
            {
            #region Shipping Mark
                string scmd = string.Format(@"select distinct c.id [id]
                                                    ,co.alias [name]
                                                    ,left(RTRIM(c.id)+' '+RTRIM(co.Alias),30) [theorderid]
                                                    ,c.MarkFront [A]
                                                    ,c.MarkBack [B]
                                                    ,c.markleft [C]
                                                    ,c.Markright [D]
                                             from orders c
                                             inner join (select distinct OrderId from localpo a
                                             inner join localpo_detail b on a.id = b.id
                                             inner join Orders c on c.poid = b.orderid  " + sqlWhere + @") m  on m.OrderId = c.poid 
                                             inner join country co on co.id = c.dest"
                                             );
                result = DBProxy.Current.Select("", scmd, lis, out shm);


              
                if (!result)
                {
                    return result;
                } 
            }
            #endregion
           
            return result;//base.OnAsyncDataLoad(e);  
        }


       
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.Report_Type == "PO List" && (dtt == null || dtt.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            } 
            if (this.Report_Type == "PO Order" && (da == null || da.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            if (this.Report_Type == "PO Form" && (shm == null || shm.Rows.Count == 0))
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }


            #region PO List
            if ("PO List".EqualString(this.comboBox1.Text))
            {
                var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog.ShowDialog();
                string outpath = saveDialog.FileName;
                if (outpath.Empty())
                {
                    return false;
                }

                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R26_Local_PO_List.xltx");
                xl.dicDatas.Add("##Factory", dtt);
                xl.Save(outpath, false);
            }
            #endregion
                
            #region PO Order
            else if ("PO Order".EqualString(this.comboBox1.Text))
            {
                var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog.ShowDialog();
                string outpath = saveDialog.FileName;
                if (outpath.Empty())
                {
                    return false;
                }

                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R26_Local_PO_Order.xltx");

                List<string> lis = new List<string>();
                List<string> listt = new List<string>();
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

                    x1.dicDatas.Add("##LocalPOID" + idxstr, TheOrderID.Substring(4));
                    x1.dicDatas.Add("##Factory" + idxstr, Factory1);
                    x1.dicDatas.Add("##theorderid" + idxstr, TheOrderID);
                    x1.dicDatas.Add("##date" + idxstr, date);
                    x1.dicDatas.Add("##SP" + idxstr, finalda);
                    idx += 1;
                }
                x1.Save(outpath, false);
            }
            #endregion

            #region Shipping Mark
            if (checkBox1.Checked == true)
            {
                var saveDialog1 = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog1.ShowDialog();
                string outpath1 = saveDialog1.FileName;
                if (outpath1.Empty())
                {
                    return false;
                }

                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R26_Shipping_Mark.xltx");

                //copy sheet by TheOrderID count.
                x1.CopySheet.Add(1, shm.Rows.Count - 1);
                x1.VarToSheetName = "##theorderid";

                List<string> ls = new List<string>();
                int idx = 0;
                foreach (DataRow row in shm.Rows)
                {
                    string idxstr = (idx == 0) ? "" : idx.ToString(); //為了讓第一筆idx是空值
                    id = row["id"].ToString();
                    name = row["name"].ToString();
                    A = row["A"].ToString();
                    B = row["B"].ToString();
                    C = row["C"].ToString();
                    D = row["D"].ToString();
                    string theorderid = row["theorderid"].ToString();
                    if (!ls.Contains(theorderid)) //lis "不"包含 TheOrderID
                        ls.Add(theorderid);
                    x1.dicDatas.Add("##id" + idxstr, id);
                    x1.dicDatas.Add("##name" + idxstr, name);
                    x1.dicDatas.Add("##theorderid" + idxstr, theorderid);
                    x1.dicDatas.Add("##A" + idxstr, A);
                    x1.dicDatas.Add("##B" + idxstr, B);
                    x1.dicDatas.Add("##C" + idxstr, C);
                    x1.dicDatas.Add("##D" + idxstr, D);
                    idx += 1;
                }
                x1.Save(outpath1, true);
            }
            #endregion
           return true; //return base.OnToExcel(report);
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
                if (checkBox1.Checked==true)
                    toexcel.Enabled = true;
            }
            else if (comboBox1.Text == "PO Order")
            {
                print.Enabled = false;
                toexcel.Enabled = true;

            }
        }

        private void txtartworktype_fty1_TextChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;

            if (txtartworktype_fty1.Text.TrimEnd().Equals("CARTON", StringComparison.OrdinalIgnoreCase))
            //if (txtartworktype_fty1.Text.EqualString ("CARTON"))
            {
                checkBox1.Enabled = true;
                toexcel.Enabled = true;
            }
            else
            {
                checkBox1.Enabled = false;

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text == "PO Form")
                toexcel.Enabled = checkBox1.Checked;
        }

    }
}

