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

namespace Sci.Production.Subcon
{
    public partial class P36 : Sci.Win.Tems.Input6
    {
        public P36(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("factoryid = '{0}'", Sci.Env.User.Factory);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

        }

        // Refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            lblStatus.Text = CurrentMaintain["status"].ToString();
            lblTaipeiDebitNote.Visible = (!MyUtility.Check.Empty(CurrentMaintain["TaipeiDBC"]));
            numTotalAmt.Value = decimal.Parse(CurrentMaintain["amount"].ToString()) + decimal.Parse(CurrentMaintain["tax"].ToString());
            btnDebitSchedule.Enabled = !this.EditMode && CurrentMaintain["status"].ToString().ToUpper() == "CONFIRMED";
            // 剛好settle的 voucher# & Date 顯示
            DataRow dr;
            MyUtility.Check.Seek(string.Format(@";WITH cte as 
(select t.VoucherID,(select a.voucherdate from FinanceEN.dbo.voucher a where a.id = t.VoucherID) voucherdate , sum(amount ) 
over (order by issuedate
      rows between unbounded preceding and current row) as running_total 
												 from dbo.Debit_Schedule T where id='{0}' and voucherid !='' and voucherid is not null 
)
SELECT TOP 1 * FROM CTE  WHERE running_total >= {1} ", CurrentMaintain["id"], numTotalAmt.Value.ToString()), out dr);
            displayBoxVoucherID.Text = null == dr ? "" : dr["voucherid"].ToString();
            displayBoxSettleDate.Text = null == dr ? "" : dr["voucherdate"].ToString();

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
                    #region 加總明細金額至表頭
                    string strExact = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency where id = '{0}'", CurrentMaintain["currencyId"]), null);
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
                .Text("unitid", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
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
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            detailgridbs.EndEdit();

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
                txtuser_Handle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SMR"]))
            {
                MyUtility.Msg.WarningBox("< SMR >  can't be empty!", "Warning");
                txtuser_SMR.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SMR"]))
            {
                MyUtility.Msg.WarningBox("< SMR >  can't be empty!", "Warning");
                txtuser_SMR.TextBox1.Focus();
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
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(factorykeyword + "LD", "localdebit", (DateTime)CurrentMaintain["issuedate"]);
            }



            return base.ClickSaveBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {

            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString().ToUpper() == "JUNKED")
            {
                MyUtility.Msg.WarningBox("This record is Junked, can't edit!!");
                return false;
            }

            if (dr["Status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record is confirmed, can't edit!!");
                return false;
            }

            if (!PublicPrg.Prgs.GetAuthority(dr["HANDLE"].ToString()))
            {
                MyUtility.Msg.WarningBox("You don't have permission to modify!!");
                return false;
            }

            if (dr["status"].ToString().ToUpper() == "SENT")
            {
                var frm = new P36_ModifyAfterSent(dr);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            if (dr["status"].ToString().ToUpper() == "RECEIVED")
            {
                var frm = new P36_ModifyAfterSent(dr);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.ClickEditBefore();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"select id,orderid,reasonid
,reasonid+isnull((select name from dbo.reason where ReasonTypeID='DebitNote_Factory' and id = reasonid),'') reason_desc
,0.00 as total 
,QTY,UNITID,AMOUNT,ADDITION,TAIPEIREASON,DESCRIPTION 
from localdebit_detail 
                                                        Where localdebit_detail.id = '{0}' order by orderid ", masterID);

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
                            , txtsubcon1.DisplayBox1.Text
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
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
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

        private void txtsubcon1_Validated(object sender, EventArgs e)
        {
            CurrentMaintain["CURRENCYID"] = MyUtility.GetValue.Lookup(string.Format(@"SELECT CurrencyID FROM DBO.LocalSupp WHERE ID='{0}'", CurrentMaintain["LOCALSUPPID"]));
        }

        private void btnDebitSchedule_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.Subcon.P37_DebitSchedule(Production.PublicPrg.Prgs.GetAuthority(dr["handle"].ToString()), dr["ID"].ToString(), null, null);
            frm.ShowDialog(this);
            this.RenewData();

        }

        //print
        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string No = row["ID"].ToString();
            string Issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
    
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result;
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("No", No));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", Issuedate));
           
            #endregion
            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string sqlcmd = @"select 
                   F.nameEn,F.AddressEN,F.Tel,ap.LocalSuppID+'-'+L.name AS Supplier,L.Address,L.tel,ap.ID,
	               A.ArtworkPoID,A.OrderID,A.ArtworkId,A.PatternDesc,A.Price,A.ApQty,A.Amount,ap.PayTermID+'-'+P.name as Terms,
	               LOB.AccountNo,LOB.AccountName,LOB.BankName,LOB.CountryID+'/'+LOB.City as Country,LOB.SWIFTCode,
	               ap.Handle+''+pas.name as PreparedBy
                   from DBO.artworkap ap
	               LEFT JOIN dbo.factory F
	               ON  F.ID = ap.factoryid
	               LEFT JOIN dbo.LocalSupp L
	               ON  L.ID = ap.LocalSuppID
	               LEFT JOIN dbo.Artworkap_Detail A
	               ON  A.ID = ap.ID
	               LEFT JOIN dbo.LocalSupp_Bank LOB
	               ON  IsDefault = 1 and LOB.ID = ap.LocalSuppID
	               LEFT JOIN DBO.PayTerm P
	               ON P.ID = ap.PayTermID
	               LEFT JOIN DBO.Pass1 pas
	               ON pas.ID = ap.Handle where ap.ID= @ID";
            result = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result) { this.ShowErr(sqlcmd, result); }
            string RptTitle = dtDetail.Rows[0]["nameEn"].ToString();
            string AddressEN = dtDetail.Rows[0]["AddressEN"].ToString();
            string TEL = dtDetail.Rows[0]["Tel"].ToString();
            string Supplier = dtDetail.Rows[0]["Supplier"].ToString();
            string Address = dtDetail.Rows[0]["Address"].ToString();
            string LTEL = dtDetail.Rows[0]["tel"].ToString();
            string Barcode = dtDetail.Rows[0]["ID"].ToString();
            string BarcodeView = dtDetail.Rows[0]["ID"].ToString();
            string Terms = dtDetail.Rows[0]["Terms"].ToString();
            string ACNO = dtDetail.Rows[0]["AccountNo"].ToString();
            string ACNAME = dtDetail.Rows[0]["AccountName"].ToString();
            string BankName = dtDetail.Rows[0]["BankName"].ToString();
            string Country = dtDetail.Rows[0]["Country"].ToString();
            string SWIFCode = dtDetail.Rows[0]["SWIFTCode"].ToString();
            string PreparedBy = dtDetail.Rows[0]["PreparedBy"].ToString();

            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL", TEL));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", Supplier));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Address", Address));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("LTEL", LTEL));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", Barcode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("BarcodeView", BarcodeView));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Terms", Terms));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ACNO", ACNO));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ACNAME", ACNAME));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("BankName", BankName));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Country", Country));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SWIFCode", SWIFCode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("PreparedBy", PreparedBy));

            if (!AddressEN.EndsWith(Environment.NewLine))
            {
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddressEN", AddressEN + Environment.NewLine));
            }
            else
            {
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddressEN", AddressEN));
            }

            // 傳 list 資料            
            List<P10_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P10_PrintData()
                {
                    POID = row1["ArtworkPoID"].ToString(),
                    OrderID = row1["OrderID"].ToString(),
                    Pattem = row1["ArtworkId"].ToString(),
                    CutPart = row1["PatternDesc"].ToString(),
                    Price = row1["Price"].ToString(),
                    Qty = row1["ApQty"].ToString(),
                    Amt = row1["Amount"].ToString()

                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P10_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P10_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            return true;
        }
    }
}
