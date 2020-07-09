using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;
using System.Transactions;
using System.Reflection;
using System.Data.SqlClient;
using Sci.Win;

namespace Sci.Production.Subcon
{
    public partial class P36 : Win.Tems.Input6
    {
        public P36(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
        }

        private bool isTaipeiDBC = false;

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        // Refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["cfmdate"]))
            {
                this.displayConfirmed.Text = Convert.ToDateTime(this.CurrentMaintain["cfmdate"]).ToString("yyyy/MM/dd");
            }
            else
            {
                this.displayConfirmed.Text = string.Empty;
            }

            this.lblStatus.Text = this.CurrentMaintain["status"].ToString();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["amtrevisedate"]))
            {
                this.displayAmtReceived.Text = Convert.ToDateTime(this.CurrentMaintain["amtrevisedate"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                this.displayAmtReceived.Text = string.Empty;
            }

            this.lblTaipeiDebitNote.Visible = !MyUtility.Check.Empty(this.CurrentMaintain["TaipeiDBC"]);

            this.isTaipeiDBC = !MyUtility.Check.Empty(this.CurrentMaintain["TaipeiDBC"]);

            this.numTotalAmt.Value = decimal.Parse(this.CurrentMaintain["amount"].ToString()) + decimal.Parse(this.CurrentMaintain["tax"].ToString());
            this.btnDebitSchedule.Enabled = !this.EditMode && this.CurrentMaintain["status"].ToString().ToUpper() == "CONFIRMED";

            // 剛好settle的 voucher# & Date 顯示
            DataRow dr;
            MyUtility.Check.Seek(
                string.Format(
                @";WITH cte as 
(select t.VoucherID,(select a.voucherdate from dbo.SciFMS_voucher a where a.id = t.VoucherID) voucherdate , sum(amount ) 
over (order by issuedate
      rows between unbounded preceding and current row) as running_total 
												 from dbo.Debit_Schedule T WITH (NOLOCK) where id='{0}' and voucherid !='' and voucherid is not null 
)
SELECT TOP 1 * FROM CTE  WHERE running_total >= {1} ", this.CurrentMaintain["id"], this.numTotalAmt.Value.ToString()), out dr);
            this.displaySettleVoucher.Text = dr == null ? string.Empty : dr["voucherid"].ToString();
            if (dr != null)
            {
                if (!MyUtility.Check.Empty(dr["voucherdate"]))
                {
                    this.displaySettleDate.Text = Convert.ToDateTime(dr["voucherdate"]).ToString("yyyy/MM/dd");
                }
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain))
            {
                return;
            }

            if (this.CurrentMaintain["TaipeiDBC"].ToString() == "True")
            {
                this.InsertDetailGridOnDoubleClick = false;
            }
            else
            {
                this.InsertDetailGridOnDoubleClick = true;
            }

            #region 依台北轉入 or 工廠建立 開放表頭欄位編輯

            // TaipeiDBC = true or 1 >> 台北轉入, 其餘為工廠建立
            if (this.EditMode == true)
            {
                if (MyUtility.Check.Empty(this.CurrentMaintain["TaipeiDBC"]))
                {
                    this.numExchange.ReadOnly = true;

                    // this.numAmount.ReadOnly = false;
                }
                else
                {
                    this.numExchange.ReadOnly = false;
                    this.numAmount.ReadOnly = true;
                }
            }
            else
            {
                this.numExchange.ReadOnly = true;
                this.numAmount.ReadOnly = true;
            }

            #endregion
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
                    if (e.RowIndex == -1)
                    {
                        return;
                    }

                    DataRow dr = this.DetailDatas[e.RowIndex];
                    string reason_desc = string.Format("select concat(id,name) from dbo.reason WITH (NOLOCK) where ReasonTypeID='DebitNote_Factory' and id = '{0}'", dr["reasonid"]);
                    dr["reason_desc"] = MyUtility.GetValue.Lookup(reason_desc);
                    if (!this.EditMode)
                    {
                        return;
                    }
                    #region 加總明細金額至表頭
                    string strExact = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["currencyId"]), null);
                    if (strExact == null || string.IsNullOrWhiteSpace(strExact))
                    {
                        MyUtility.Msg.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , summary amout failed!", this.CurrentMaintain["currencyID"]), "Warning");

                        // return;
                    }

                    int exact = int.Parse(MyUtility.Check.Empty(strExact) ? "0" : strExact);
                    decimal exchange = Convert.ToDecimal(this.CurrentMaintain["exchange"]);
                    object detail_Amount = ((DataTable)this.detailgridbs.DataSource).Compute("sum(AMOUNT)", string.Empty);
                    object detail_Addition = ((DataTable)this.detailgridbs.DataSource).Compute("sum(ADDITION)", string.Empty);

                    this.CurrentMaintain["amount"] = MyUtility.Math.Round(((decimal)detail_Amount + (decimal)detail_Addition) * exchange, exact);

                    this.ReCalculateTax();
                    #endregion
                }
            };

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("qty", header: "Affect Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 9, iseditingreadonly: true)
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
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["issuedate"] = DateTime.Today;
            this.CurrentMaintain["handle"] = Env.User.UserID;
            this.CurrentMaintain["Amount"] = 0;
            this.CurrentMaintain["Tax"] = 0;
            this.CurrentMaintain["TaxRate"] = 0;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["SMR"] = MyUtility.GetValue.Lookup("Supervisor", Env.User.UserID, "Pass1", "ID");
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["exchange"] = 1;
            this.dateReceiveDate.ReadOnly = true;
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            this.detailgridbs.EndEdit();
            this.dateReceiveDate.ReadOnly = true;
            #region 必輸檢查
            if (this.CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(this.CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                this.txtuserHandle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SMR"]))
            {
                MyUtility.Msg.WarningBox("< SMR >  can't be empty!", "Warning");
                this.txtuserSMR.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["LocalSuppID"]))
            {
                MyUtility.Msg.WarningBox("< Suppiler >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CURRENCYID"]))
            {
                MyUtility.Msg.WarningBox("< Currency >  can't be empty!", "Warning");
                return false;
            }

            #endregion

            // 計算TAX
            this.ReCalculateTax();

            // 刪除 qty,amount,addtion皆為零的資料
            foreach (DataRow row in ((DataTable)this.detailgridbs.DataSource).Select("qty = 0 and amount = 0 and addition = 0"))
            {
                row.Delete();
            }

            // 明細資料不可為空
            if (((DataTable)this.detailgridbs.DataSource).Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            DataRow[] drchk = ((DataTable)this.detailgridbs.DataSource).Select("[reasonid] ='' or [reasonid] is null ");
            if (drchk.Length > 0)
            {
                MyUtility.Msg.WarningBox("Detail of Reason can't be empty", "Warning");
                return false;
            }

            // 取單號：
            if (this.IsDetailInserting)
            {
                string factorykeyword = MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where ID ='{0}'", this.CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }

                this.CurrentMaintain["id"] = MyUtility.GetValue.GetID(this.CurrentMaintain["factoryid"].ToString() + "LD", "localdebit", (DateTime)this.CurrentMaintain["issuedate"]);
            }

            return base.ClickSaveBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() == "JUNKED")
            {
                MyUtility.Msg.WarningBox("This record is Junked, can't edit!!");
                return false;
            }

            if (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record is confirmed, can't edit!!");
                return false;
            }

            if (this.CurrentMaintain["status"].ToString().ToUpper() == "SENT")
            {
                var frm = new P36_ModifyAfterSent(this.CurrentMaintain);
                frm.ShowDialog(this);
                this.RenewData();
                this.Refresh();
                this.ReCalculateTax();
                return false;
            }

            if (this.CurrentMaintain["status"].ToString().ToUpper() == "RECEIVED")
            {
                var frm = new P36_ModifyAfterSent(this.CurrentMaintain);
                frm.ShowDialog(this);
                this.RenewData();
                this.Refresh();
                this.ReCalculateTax();
                return false;
            }

            this.dateReceiveDate.ReadOnly = true;
            return base.ClickEditBefore();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"
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
            List<SqlParameter> paraList = new List<SqlParameter>();
            if (seleReason) // 要選原因的代表狀態回復到上一個狀態或Junk。
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to do it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dResult == DialogResult.No)
                {
                    return;
                }

                var frm = new Win.UI.SelectReason(reasonType);
                frm.ShowDialog();
                if (MyUtility.Check.Empty(frm.ReturnReason))
                {
                    return;
                }

                insertCmd = @"Insert into LocalDebit_History 
(histype,id,oldvalue,newvalue,reasonid,remark,addname,adddate)
values ('LocalDebit',@id,@oldvalue,@newvalue,@reasonid,@remark,@addname,getdate())";

                paraList.Add(new SqlParameter("@id", this.CurrentMaintain["id"].ToString()));
                paraList.Add(new SqlParameter("@oldvalue", oldvalue));
                paraList.Add(new SqlParameter("@newvalue", newValue));
                paraList.Add(new SqlParameter("@reasonid", frm.ReturnReason));
                paraList.Add(new SqlParameter("@remark", frm.ReturnRemark));
                paraList.Add(new SqlParameter("@addname", Env.User.UserID));
            }
            else
            {
                insertCmd = @"Insert into LocalDebit_History 
(histype,id,oldvalue,newvalue,reasonid,remark,addname,adddate)
values ('LocalDebit',@id,@oldvalue,@newvalue,'','',@addname,getdate())";

                paraList.Add(new SqlParameter("@id", this.CurrentMaintain["id"].ToString()));
                paraList.Add(new SqlParameter("@oldvalue", oldvalue));
                paraList.Add(new SqlParameter("@newvalue", newValue));
                paraList.Add(new SqlParameter("@addname", Env.User.UserID));
            }

            updateCmd = string.Format(
                @"update LocalDebit set status='{0}', statuseditdate = GETDATE() , editname = '{1}' , editdate = GETDATE() ",
                newValue, Env.User.UserID);

            // 狀態變成Received要update 2個欄位
            if (oldvalue.ToUpper() == "SENT" && newValue.ToUpper() == "RECEIVED")
            {
                updateCmd += string.Format(@" ,receivename = '{0}' , receiveDate = getdate()", Env.User.UserID);
            }

            if (oldvalue.ToUpper() == "RECEIVED" && newValue.ToUpper() == "SENT")
            {
                updateCmd += string.Format(@" ,receivename = '' , receiveDate = null");
            }

            // 狀態變成Confirmed要update 2個欄位
            if (oldvalue.ToUpper() == "RECEIVED" && newValue.ToUpper() == "CONFIRMED")
            {
                updateCmd += string.Format(@" ,cfmname = '{0}' , cfmDate = getdate()", Env.User.UserID);
            }

            if (oldvalue.ToUpper() == "CONFIRMED" && newValue.ToUpper() == "RECEIVED")
            {
                updateCmd += string.Format(@" ,cfmname = '' , cfmDate = null");
            }

            updateCmd += string.Format(@" where id = '{0}'", this.CurrentMaintain["id"]);

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, insertCmd, paraList)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(insertCmd, result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, updateCmd)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(updateCmd, result);
                        return;
                    }

                    if (newValue.ToUpper() == "CONFIRMED") // Confirm時，要回寫資料至Debit
                    {
                        if (!(result = DBProxy.Current.Execute(null, string.Format(
                            @"update debit set LCLName ='{0}' , LCLCurrency='{1}' ,LCLAmount={2}, LCLRate={3} ,editdate=getdate()
where id = '{4}'",
                            this.txtsubconSupplier.DisplayBox1.Text,
                            this.CurrentMaintain["currencyid"],
                            decimal.Parse(this.CurrentMaintain["amount"].ToString()) + decimal.Parse(this.CurrentMaintain["tax"].ToString()),
                            this.CurrentMaintain["exchange"],
                            this.CurrentMaintain["id"]))))
                        {
                            _transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    if (oldvalue.ToUpper() == "CONFIRMED") // UnConfirm時，要清掉Debit & Debit_Schedule資料
                    {
                        if (!(result = DBProxy.Current.Execute(null, string.Format(
                            @"update debit set LCLName ='' , LCLCurrency='' ,LCLAmount=0, LCLRate=0  
                            where id = '{0}'",
                            this.CurrentMaintain["id"]))))
                        {
                            _transactionscope.Dispose();
                            this.ShowErr(
                                string.Format(
                                @"update debit set LCLName ='' , LCLCurrency='' ,LCLAmount=0, LCLRate=0 where id = '{0}'",
                                this.CurrentMaintain["id"]), result);
                            return;
                        }

                        if (!(result = DBProxy.Current.Execute(null, string.Format("delete from Debit_Schedule where id = '{0}'", this.CurrentMaintain["id"]))))
                        {
                            _transactionscope.Dispose();
                            this.ShowErr(string.Format("delete from Debit_Schedule where id = '{0}'", this.CurrentMaintain["id"]), result);
                            return;
                        }
                    }

                    _transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;
        }

        protected override void ClickSend()
        {
            base.ClickSend();

            this.updateStatus(this.CurrentMaintain["status"].ToString(), "Sent", false);
        }

        protected override void ClickRecall()
        {
            base.ClickRecall();
            this.updateStatus(this.CurrentMaintain["status"].ToString(), "New", true);
        }

        protected override void ClickReceive()
        {
            base.ClickReceive();
            this.updateStatus(this.CurrentMaintain["status"].ToString(), "Received", false);
        }

        protected override void ClickReturn()
        {
            base.ClickReturn();
            this.updateStatus(this.CurrentMaintain["status"].ToString(), "Sent", true);
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            this.updateStatus(this.CurrentMaintain["status"].ToString(), "Confirmed", false);
        }

        protected override bool ClickNew()
        {
            this.dateReceiveDate.ReadOnly = true;
            return base.ClickNew();
        }

        protected override void ClickUnconfirm()
        {
            // 有傳票號碼,則不能unconfirm
            if (!MyUtility.Check.Empty(this.displaySettleVoucher.Text))
            {
                MyUtility.Msg.WarningBox("Cannot UnComirm, debit note have voucher no.");
                return;
            }

            base.ClickUnconfirm();
            this.updateStatus(this.CurrentMaintain["status"].ToString(), "Received", true);
        }

        protected override void ClickJunk()
        {
            base.ClickJunk();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["TaipeiDBC"]))
            {
                MyUtility.Msg.WarningBox("Taipei Debit note can't be junked!!");
                return;
            }

            if (!PublicPrg.Prgs.GetAuthority(this.CurrentMaintain["handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("No authority to Junk!!");
                return;
            }

            this.updateStatus(this.CurrentMaintain["status"].ToString(), "Junked", true, "DebitNote_Factory");
        }

        private void btnStatusHistory_Click(object sender, EventArgs e)
        {
            var showhis = new Win.UI.ShowHistory("localdebit_history", this.CurrentMaintain["ID"].ToString(), "LocalDebit", "DebitNote_LS");
            showhis.ShowDialog();
        }

        private void txtsubconSupplier_Validated(object sender, EventArgs e)
        {
            this.CurrentMaintain["CURRENCYID"] = MyUtility.GetValue.Lookup(string.Format(@"SELECT CurrencyID FROM DBO.LocalSupp WITH (NOLOCK) WHERE ID='{0}'", this.CurrentMaintain["LOCALSUPPID"]));
        }

        private void btnDebitSchedule_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            // var frm = new Sci.Production.Subcon.P37_DebitSchedule(Production.PublicPrg.Prgs.GetAuthority(dr["cfmname"].ToString()), dr["ID"].ToString(), null, null);
            var frm = new P37_DebitSchedule(true, dr["ID"].ToString(), null, null, this.CurrentMaintain, "P36", (bool)this.CurrentMaintain["TaipeiDBC"]);  // 調成跟舊系統一樣，不管誰都可以編輯
            frm.ShowDialog(this);
            this.RenewData();
            this.Refresh();
        }

        // print
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["status"].ToString() == "Junked")
            {
                return false;
            }

            // if (Sci.Env.User.UserID!=CurrentMaintain["handle"].ToString()){return false;}//20170416 mark by dyson

            // 如果已經列印過的，則提醒是否再次列印。
            string printdate = MyUtility.GetValue.Lookup(string.Format("select convert(varchar, printdate, 120) from localdebit WITH (NOLOCK) where ID ='{0}'", this.CurrentMaintain["id"]));
            if (!MyUtility.Check.Empty(printdate))
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox("In" + ' ' + printdate + ' ' + " has been printed ,Do you want to print again?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dResult.ToString().ToUpper() == "NO")
                {
                    return false;
                }
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
            DataRow drSubject;

            string sqlSubject = @"
select 
    Ldeb.ID,
    F.nameEn,
    [Supplier] = Ldeb.LocalSuppID+L.name,
    [FROM] = dbo.getPass1(Ldeb.handle),
    [SP] = poids.poid,
    [Subject] = (Select t.name +',' from (SELECT name FROM DBO.Reason WITH (NOLOCK) 
                    inner join dbo.localdebit_detail WITH (NOLOCK) on dbo.localdebit_detail.reasonid = DBO.Reason.id
                    WHERE reason.ReasonTypeID = 'DebitNote_Factory' 
                    and localdebit_detail.id =Ldeb.id) t for xml path('')),
    [DESC] = Ldeb.Description,
    Ldeb.Currencyid,
    [Amount] = format(Ldeb.Amount,'#,###,###,##0.00') ,
    [ExchangeRate] = Iif(Ldeb.Exchange=0,'',FORMAT(Ldeb.Exchange,'###.00')),
    [titletaxrate] = CONCAT( Ldeb.taxrate , ' %TAX'),
    [taxrate] = FORMAT(Ldeb.tax,'#,##0.00') ,
    [total] = FORMAT(Ldeb.Amount+Ldeb.Tax,'#,##0.00'),
    [Purchaser] = dbo.getpass1(Ldeb.Handle)+ '/' +  dbo.getpass1(Ldeb.SMR)					
from DBO.LocalDebit Ldeb WITH (NOLOCK) 
LEFT JOIN dbo.factory F WITH (NOLOCK) ON  F.ID = Ldeb.factoryid
LEFT JOIN dbo.LocalSupp L WITH (NOLOCK) ON  L.ID = Ldeb.LocalSuppID
LEFT JOIN dbo.localdebit_detail Ldetail WITH (NOLOCK) ON  Ldetail.ID = Ldeb.ID
outer apply
(
	select poid =
	stuff((
		select distinct concat(',',orderid)
		from localdebit_detail
		where id = Ldeb.ID
		for xml path('')
	),1,1,'')
)poids
where Ldeb.ID= @ID";

            MyUtility.Check.Seek(sqlSubject, pars, out drSubject);

            string Barcode = drSubject["ID"].ToString();
            string FROM = drSubject["FROM"].ToString();
            string title = drSubject["nameEn"].ToString();
            string titletaxrate = drSubject["titletaxrate"].ToString();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", Barcode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FROM", FROM));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("title", title));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("titletaxrate", titletaxrate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", drSubject["Supplier"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("No", drSubject["ID"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SP", drSubject["SP"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Subject", drSubject["Subject"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("DESC", drSubject["DESC"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Currencyid", drSubject["Currencyid"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Amount", drSubject["Amount"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ExchangeRate", drSubject["ExchangeRate"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("taxrate", drSubject["taxrate"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("total", drSubject["total"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Purchaser", drSubject["Purchaser"].ToString()));

            // 傳 list 資料
            List<P36_PrintData> data = this.DetailDatas
                .Select(row1 => new P36_PrintData()
                {
                    OrderID = row1["OrderID"].ToString(),
                    AffectQty = row1["Qty"].ToString(),
                    Reason = row1["reason_desc"].ToString(),
                    Desc = row1["description"].ToString(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            // DualResult result;
            Type ReportResourceNamespace = typeof(P36_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P36_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);

            // 有按才更新列印日期printdate。
            frm.viewer.Print += (s, eArgs) => { var result3 = DBProxy.Current.Execute(null, string.Format("update localdebit set printdate=getdate() where id = '{0}'", this.CurrentMaintain["id"])); };
            if (this.MdiParent != null)
            {
                frm.MdiParent = this.MdiParent;
            }

            frm.Show();

            return true;
        }

        private void numExchange_Validating(object sender, CancelEventArgs e)
        {
            // 若 Debit Note 屬於工廠建立,則Exchange不須判斷
            if (MyUtility.Check.Empty(this.CurrentMaintain["TaipeiDBC"]))
            {
                return;
            }

            if (MyUtility.Check.Empty(this.numExchange.Value))
            {
                MyUtility.Msg.WarningBox("Exchange value cannot be 0!");
                e.Cancel = true;
                return;
            }

            this.CurrentMaintain["Exchange"] = this.numExchange.Text;
            this.CurrentMaintain["amount"] = Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TaipeiAMT"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["Exchange"]), 2);
        }

        private void numAmount_Validating(object sender, CancelEventArgs e)
        {
            this.CurrentMaintain["amount"] = this.numAmount.Text;
            this.ReCalculateTax();
        }

        private void ReCalculateTax()
        {
            decimal amount = MyUtility.Convert.GetDecimal(this.CurrentMaintain["amount"]);
            decimal TaxRate = MyUtility.Convert.GetDecimal(this.CurrentMaintain["taxrate"]);
            int Exact = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", this.CurrentMaintain["currencyId"]), null));
            if (MyUtility.Check.Empty(Exact))
            {
                Exact = 0;
            }

            this.CurrentMaintain["Tax"] = Math.Round((amount * TaxRate) / 100, Exact);
            this.numTotalAmt.Value = decimal.Parse(this.CurrentMaintain["amount"].ToString()) + decimal.Parse(this.CurrentMaintain["tax"].ToString());
        }

        private void numtaxrate_Validating(object sender, CancelEventArgs e)
        {
            this.CurrentMaintain["taxrate"] = this.numtaxrate.Text;
            this.ReCalculateTax();
        }
    }
}
