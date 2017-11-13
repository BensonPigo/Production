using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production;

using Sci.Production.PublicPrg;
using System.Linq;
using System.Transactions;
using System.Reflection;
using System.Data.SqlClient;
using Sci.Win;
using System.Globalization;

namespace Sci.Production.Subcon
{
    public partial class P36 : Sci.Win.Tems.Input6
    {
        public P36(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        // Refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (!MyUtility.Check.Empty(CurrentMaintain["cfmdate"]))
            {
                this.displayConfirmed.Text = Convert.ToDateTime(CurrentMaintain["cfmdate"]).ToString("yyyy/MM/dd");
            }
            else this.displayConfirmed.Text = "";
          
            lblStatus.Text = CurrentMaintain["status"].ToString();
            if (!MyUtility.Check.Empty(CurrentMaintain["amtrevisedate"]))
            {
                this.displayAmtReceived.Text = Convert.ToDateTime(CurrentMaintain["amtrevisedate"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
                this.displayAmtReceived.Text = "";
            lblTaipeiDebitNote.Visible = (!MyUtility.Check.Empty(CurrentMaintain["TaipeiDBC"]));
            numTotalAmt.Value = decimal.Parse(CurrentMaintain["amount"].ToString()) + decimal.Parse(CurrentMaintain["tax"].ToString());
            btnDebitSchedule.Enabled = !this.EditMode && CurrentMaintain["status"].ToString().ToUpper() == "CONFIRMED";
            // 剛好settle的 voucher# & Date 顯示
            DataRow dr;
            MyUtility.Check.Seek(string.Format(@";WITH cte as 
(select t.VoucherID,(select a.voucherdate from FinanceEN.dbo.voucher a where a.id = t.VoucherID) voucherdate , sum(amount ) 
over (order by issuedate
      rows between unbounded preceding and current row) as running_total 
												 from dbo.Debit_Schedule T WITH (NOLOCK) where id='{0}' and voucherid !='' and voucherid is not null 
)
SELECT TOP 1 * FROM CTE  WHERE running_total >= {1} ", CurrentMaintain["id"], numTotalAmt.Value.ToString()), out dr);
            displaySettleVoucher.Text = null == dr ? "" : dr["voucherid"].ToString();
            if (dr != null)
            {
                if (!MyUtility.Check.Empty(dr["voucherdate"]))
                {
                    displaySettleDate.Text = Convert.ToDateTime(dr["voucherdate"]).ToString("yyyy/MM/dd");
                }
            }
            if (MyUtility.Check.Empty(CurrentMaintain)) return;
            if (CurrentMaintain["TaipeiDBC"].ToString() == "True")
            {
                InsertDetailGridOnDoubleClick = false;
            }
            else
            {
                InsertDetailGridOnDoubleClick = true;
            }
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {

            this.detailgrid.CellDoubleClick += (s, e) =>
            {
                if (e.ColumnIndex == 0)
                {
                    P36_ModifyDetail DoForm = new P36_ModifyDetail();
                    DoForm.Set(this.EditMode, this.DetailDatas, this.CurrentDetailData);
                    DoForm.ShowDialog(this);
                    DataRow dr = DetailDatas[e.RowIndex];
                    string reason_desc = string.Format("select concat(id,name) from dbo.reason WITH (NOLOCK) where ReasonTypeID='DebitNote_Factory' and id = '{0}'", dr["reasonid"]);
                    dr["reason_desc"] = MyUtility.GetValue.Lookup(reason_desc);
                  if (!EditMode) return;
                    #region 加總明細金額至表頭
                    string strExact = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", CurrentMaintain["currencyId"]), null);
                    if (strExact == null || string.IsNullOrWhiteSpace(strExact))
                    {
                        MyUtility.Msg.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , summary amout failed!", CurrentMaintain["currencyID"]), "Warning");
                        return;
                    }
                    int exact = int.Parse(strExact);
                    object detail_a = ((DataTable)detailgridbs.DataSource).Compute("sum(total)", "");
                    CurrentMaintain["amount"] = MyUtility.Math.Round((decimal)detail_a, exact);
                    CurrentMaintain["tax"] = MyUtility.Math.Round((decimal)detail_a * (decimal)CurrentMaintain["taxrate"] / 100, exact);
                    #endregion
                }
            };


            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("qty", header: "Affect Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 6, iseditingreadonly: true)
                .Text("unitid", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("amount", header: "Claim Amt", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                .Numeric("addition", header: "Addition", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                .Text("taipeireason", header: "Ori. Reason", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("reason_desc", header: "Reason", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .EditText("description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20));
            #endregion


        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["issuedate"] = System.DateTime.Today;
            CurrentMaintain["handle"] = Sci.Env.User.UserID;
            CurrentMaintain["Amount"] = 0;
            CurrentMaintain["Tax"] = 0;
            CurrentMaintain["TaxRate"] = 0;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["SMR"] = MyUtility.GetValue.Lookup("Supervisor", Sci.Env.User.UserID, "Pass1", "ID");
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            dateReceiveDate.ReadOnly = true;
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            detailgridbs.EndEdit();
            dateReceiveDate.ReadOnly = true;
            #region 必輸檢查
            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                txtuserHandle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SMR"]))
            {
                MyUtility.Msg.WarningBox("< SMR >  can't be empty!", "Warning");
                txtuserSMR.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SMR"]))
            {
                MyUtility.Msg.WarningBox("< SMR >  can't be empty!", "Warning");
                txtuserSMR.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["CURRENCYID"]))
            {
                MyUtility.Msg.WarningBox("< Currency >  can't be empty!", "Warning");
                return false;
            }

            #endregion

            // 刪除 qty,amount,addtion皆為零的資料
            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("qty = 0 and amount = 0 and addition = 0"))
            {
                row.Delete();
            }

            //明細資料不可為空
            if (((DataTable)detailgridbs.DataSource).Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            DataRow[] drchk = ((DataTable)detailgridbs.DataSource).Select("[reasonid] ='' or [reasonid] is null ");
            if (drchk.Length > 0)
            {
                MyUtility.Msg.WarningBox("Detail of Reason can't be empty", "Warning");
                return false;
            }

            //取單號： 
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(CurrentMaintain["factoryid"].ToString() + "LD", "localdebit", (DateTime)CurrentMaintain["issuedate"]);
            }



            return base.ClickSaveBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString().ToUpper() == "JUNKED")
            {
                MyUtility.Msg.WarningBox("This record is Junked, can't edit!!");
                return false;
            }

            if (CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record is confirmed, can't edit!!");
                return false;
            }

            //調成跟舊系統一樣，不管誰都可以編輯
            //if (!PublicPrg.Prgs.GetAuthority(CurrentMaintain["HANDLE"].ToString()))
            //{
            //    MyUtility.Msg.WarningBox("You don't have permission to modify!!");
            //    return false;
            //}

            if (CurrentMaintain["status"].ToString().ToUpper() == "SENT")
            {
                var frm = new P36_ModifyAfterSent(CurrentMaintain);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            if (CurrentMaintain["status"].ToString().ToUpper() == "RECEIVED")
            {
                var frm = new P36_ModifyAfterSent(CurrentMaintain);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }
            dateReceiveDate.ReadOnly = true;
            return base.ClickEditBefore();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"
select a.ID,Ukey
,TaipeiUkey
,orderid
,reasonid
,reasonid+isnull((select name from dbo.reason WITH (NOLOCK) where ReasonTypeID='DebitNote_Factory' and id = reasonid),'') reason_desc
,0.00 as total 
,QTY,UNITID
,a.AMOUNT
,ADDITION
,TAIPEIREASON
,a.DESCRIPTION 
,b.TaipeiDBC
from localdebit_detail a WITH (NOLOCK) 
inner join LocalDebit b with(nolock) on a.ID=b.ID
Where a.id = '{0}' order by orderid ", masterID);
            
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void updateStatus(string oldvalue, string newValue, bool seleReason, string reasonType = "DebitNote_LS")
        {
            DualResult result;
            string insertCmd, updateCmd;

            if (seleReason) // 要選原因的代表狀態回復到上一個狀態或Junk。
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to do it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dResult == DialogResult.No) return;
                var frm = new Sci.Win.UI.SelectReason(reasonType);
                frm.ShowDialog();
                if (MyUtility.Check.Empty(frm.ReturnReason)) return;
                insertCmd = string.Format(@"Insert into LocalDebit_History 
(histype,id,oldvalue,newvalue,reasonid,remark,addname,adddate)
values ('LocalDebit','{5}','{0}','{1}','{3}','{4}','{2}',getdate())", oldvalue, newValue, Sci.Env.User.UserID, frm.ReturnReason, frm.ReturnRemark, CurrentMaintain["id"]);
            }
            else
            {
                insertCmd = string.Format(@"Insert into LocalDebit_History 
(histype,id,oldvalue,newvalue,reasonid,remark,addname,adddate)
values ('LocalDebit','{3}','{0}','{1}','','','{2}',getdate())", oldvalue, newValue, Sci.Env.User.UserID, CurrentMaintain["id"]);
            }

            updateCmd = string.Format(@"update LocalDebit set status='{0}', statuseditdate = GETDATE() , editname = '{1}' , editdate = GETDATE() "
                , newValue, Env.User.UserID);
            //狀態變成Received要update 2個欄位
            if (oldvalue.ToUpper() == "SENT" && newValue.ToUpper() == "RECEIVED") updateCmd += string.Format(@" ,receivename = '{0}' , receiveDate = getdate()", Env.User.UserID);
            if (oldvalue.ToUpper() == "RECEIVED" && newValue.ToUpper() == "SENT") updateCmd += string.Format(@" ,receivename = '' , receiveDate = null");
            //狀態變成Confirmed要update 2個欄位
            if (oldvalue.ToUpper() == "RECEIVED" && newValue.ToUpper() == "CONFIRMED") updateCmd += string.Format(@" ,cfmname = '{0}' , cfmDate = getdate()", Env.User.UserID);
            if (oldvalue.ToUpper() == "CONFIRMED" && newValue.ToUpper() == "RECEIVED") updateCmd += string.Format(@" ,cfmname = '' , cfmDate = null");
            updateCmd += string.Format(@" where id = '{0}'", CurrentMaintain["id"]);

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, insertCmd)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(insertCmd, result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, updateCmd)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(updateCmd, result);
                        return;
                    }

                    if (newValue.ToUpper() == "CONFIRMED")  // Confirm時，要回寫資料至Debit
                    {
                        if (!(result = DBProxy.Current.Execute(null, string.Format(@"update debit set LCLName ='{0}' , LCLCurrency='{1}' ,LCLAmount={2}, LCLRate={3} ,editdate=getdate()
where id = '{4}'"
                            , txtsubconSupplier.DisplayBox1.Text
                            , CurrentMaintain["currencyid"]
                            , decimal.Parse(CurrentMaintain["amount"].ToString()) + decimal.Parse(CurrentMaintain["tax"].ToString())
                            , CurrentMaintain["exchange"]
                            , CurrentMaintain["id"]))))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }

                    if (oldvalue.ToUpper() == "CONFIRMED") // UnConfirm時，要清掉Debit & Debit_Schedule資料
                    {
                        if (!(result = DBProxy.Current.Execute(null, string.Format(@"update debit set LCLName ='' , LCLCurrency='' ,LCLAmount=0, LCLRate=0  
                            where id = '{0}'"
                            , CurrentMaintain["id"]))))
                        {
                            _transactionscope.Dispose();
                            ShowErr(string.Format(@"update debit set LCLName ='' , LCLCurrency='' ,LCLAmount=0, LCLRate=0 where id = '{0}'"
                            , CurrentMaintain["id"]), result);
                            return;
                        }

                        if (!(result = DBProxy.Current.Execute(null, string.Format("delete from Debit_Schedule where id = '{0}'", CurrentMaintain["id"]))))
                        {
                            _transactionscope.Dispose();
                            ShowErr(string.Format("delete from Debit_Schedule where id = '{0}'", CurrentMaintain["id"]), result);
                            return;
                        }
                    }

                    _transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
          
        }

        protected override void ClickSend()
        {
            base.ClickSend();

            updateStatus(CurrentMaintain["status"].ToString(), "Sent", false);

        }

        protected override void ClickRecall()
        {
            base.ClickRecall();
            updateStatus(CurrentMaintain["status"].ToString(), "New", true);
        }

        protected override void ClickReceive()
        {
            base.ClickReceive();
            updateStatus(CurrentMaintain["status"].ToString(), "Received", false);
        }

        protected override void ClickReturn()
        {
            base.ClickReturn();
            updateStatus(CurrentMaintain["status"].ToString(), "Sent", true);
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            updateStatus(CurrentMaintain["status"].ToString(), "Confirmed", false);
        }

        protected override bool ClickNew()
        {
            dateReceiveDate.ReadOnly = true;
            return base.ClickNew();
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            updateStatus(CurrentMaintain["status"].ToString(), "Received", true);
        }
        
        protected override void ClickJunk()
        {
            base.ClickJunk();
            if (!MyUtility.Check.Empty(CurrentMaintain["TaipeiDBC"]))
            {
                MyUtility.Msg.WarningBox("Taipei Debit note can't be junked!!");
                return;
            }

            if (!PublicPrg.Prgs.GetAuthority(CurrentMaintain["handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("No authority to Junk!!");
                return;
            }

            updateStatus(CurrentMaintain["status"].ToString(), "Junked", true, "DebitNote_Factory");
        }

        private void btnStatusHistory_Click(object sender, EventArgs e)
        {
            var showhis = new Sci.Win.UI.ShowHistory("localdebit_history", CurrentMaintain["ID"].ToString(), "LocalDebit", "DebitNote_LS");
            showhis.ShowDialog();
        }

        private void txtsubconSupplier_Validated(object sender, EventArgs e)
        {
            CurrentMaintain["CURRENCYID"] = MyUtility.GetValue.Lookup(string.Format(@"SELECT CurrencyID FROM DBO.LocalSupp WITH (NOLOCK) WHERE ID='{0}'", CurrentMaintain["LOCALSUPPID"]));
        }

        private void btnDebitSchedule_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain; if (null == dr) return;
            //var frm = new Sci.Production.Subcon.P37_DebitSchedule(Production.PublicPrg.Prgs.GetAuthority(dr["cfmname"].ToString()), dr["ID"].ToString(), null, null);
            var frm = new Sci.Production.Subcon.P37_DebitSchedule(true, dr["ID"].ToString(), null, null);  //調成跟舊系統一樣，不管誰都可以編輯
            frm.ShowDialog(this);
            this.RenewData();

        }

        //print
        protected override bool ClickPrint()
        {
           
            if (CurrentMaintain["status"].ToString() == "Junked"){return false;}
            if (Sci.Env.User.UserID!=CurrentMaintain["handle"].ToString()){return false;}

            //如果已經列印過的，則提醒是否再次列印。
            string printdate = Sci.MyUtility.GetValue.Lookup(string.Format("select convert(varchar, printdate, 120) from localdebit WITH (NOLOCK) where ID ='{0}'", CurrentMaintain["id"]));
            if (!MyUtility.Check.Empty(printdate))
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox("In" + ' ' + printdate + ' ' + " has been printed ,Do you want to print again?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dResult.ToString().ToUpper() == "NO") return false;
            }
          
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
           
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
    
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result;
            ReportDefinition report = new ReportDefinition();
            
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
           
            #endregion
            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string sqlcmd = @"select 
				 Ldeb.ID,dbo.getPass1(Ldeb.handle)[FROM],F.nameEn,Ldeb.LocalSuppID+L.name AS Supplier,
				 (Select Ldetail.orderid+',' from localdebit_detail WITH (NOLOCK) where id = Ldeb.id for xml path('')) [poid],
				 (Select t.name +',' from (SELECT name FROM DBO.Reason WITH (NOLOCK) 
					inner join dbo.localdebit_detail WITH (NOLOCK) on dbo.localdebit_detail.reasonid = DBO.Reason.id
					WHERE reason.ReasonTypeID = 'DebitNote_Factory' 
					and localdebit_detail.id =Ldeb.id) t for xml path(''))[Subject],
					Ldeb.Description,Ldeb.Currencyid,format(Ldeb.Amount,'#,###,###,##0.00') Amount,
					Iif(Ldeb.Exchange=0,'',FORMAT(Ldeb.Exchange,'###.00'))as ExchangeRate,
					CONCAT( Ldeb.taxrate , ' %TAX')as titletaxrate,FORMAT(Ldeb.tax,'#,##0.00')as taxrate
					,FORMAT(Ldeb.Amount+Ldeb.Tax,'#,##0.00')as total,dbo.getpass1(Ldeb.Handle)+ '/' +  dbo.getpass1 (Ldeb.SMR) as Purchaser
					
                   from DBO.LocalDebit Ldeb WITH (NOLOCK) 
	               LEFT JOIN dbo.factory F WITH (NOLOCK) 
	               ON  F.ID = Ldeb.factoryid
	               LEFT JOIN dbo.LocalSupp L WITH (NOLOCK) 
	               ON  L.ID = Ldeb.LocalSuppID
	               LEFT JOIN dbo.localdebit_detail Ldetail WITH (NOLOCK) 
	               ON  Ldetail.ID = Ldeb.ID where Ldeb.ID= @ID";
            result = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result) { this.ShowErr(sqlcmd, result); }
            string Barcode = dtDetail.Rows[0]["ID"].ToString();
            string FROM = dtDetail.Rows[0]["FROM"].ToString();
            string title = dtDetail.Rows[0]["nameEn"].ToString();
            string titletaxrate = dtDetail.Rows[0]["titletaxrate"].ToString();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", Barcode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FROM", FROM));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("title", title));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("titletaxrate", titletaxrate));
               
            // 傳 list 資料            
            List<P36_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P36_PrintData()
                {
                    Supplier = row1["Supplier"].ToString(),
                    No = row1["ID"].ToString(),
                    SP = row1["poid"].ToString(),
                    currencyid = row1["Currencyid"].ToString(),
                    Subject = row1["Subject"].ToString(),
                    DESC = row1["Description"].ToString(),
                    Amount = row1["Amount"].ToString(),
                    Exchange = row1["ExchangeRate"].ToString(),
                    taxrate = row1["taxrate"].ToString(),
                    Total = row1["total"].ToString(),
                    Purchaser = row1["Purchaser"].ToString()
                
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P36_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P36_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            //有按才更新列印日期printdate。
            frm.viewer.Print += (s, eArgs) => { var result3 = DBProxy.Current.Execute(null, string.Format("update localdebit set printdate=getdate() where id = '{0}'", CurrentMaintain["id"])); }; 
            if (MdiParent != null) frm.MdiParent = MdiParent;
            frm.Show();

            return true;
        }
    }
}
