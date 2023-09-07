using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicForm;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.MyUtility;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P12 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MdivisionID = '" + Env.User.Keyword + "'";
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.txtAccountant.TextBox1.ReadOnly = true;
            this.txtAccountant.TextBox1.IsSupportEditMode = false;
            this.InsertDetailGridOnDoubleClick = false;

            this.txtSupplier.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtSupplier.TextBox1.Text != this.txtSupplier.TextBox1.OldValue)
                {
                    this.CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtSupplier.TextBox1.Text, "LocalSupp", "ID");
                    this.CurrentMaintain["Paytermid"] = MyUtility.GetValue.Lookup("paytermid", this.txtSupplier.TextBox1.Text, "LocalSupp", "ID");
                }
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.txtFactory.Enabled = !this.EditMode || this.IsDetailInserting;
            this.txtSupplier.Enabled = !this.EditMode || this.IsDetailInserting;
            this.date_IssueDate.Enabled = !this.EditMode || this.IsDetailInserting;

            #region  表頭Amount、Vat 計算
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            decimal sumAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("Amount"));

            string sqlcmdCurrencyExact = $@"
            select Exact from Currency where ID = '{this.CurrentMaintain["CurrencyID"]}'
            ";
            int currencyExact = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlcmdCurrencyExact));

            decimal x = 0;
            decimal x1 = 0;
            decimal x2 = 0;
            x = MyUtility.Math.Round(sumAmount, 2);
            x2 = MyUtility.Math.Round(x * (decimal)this.CurrentMaintain["VatRate"] / 100, currencyExact);
            x1 += x + x2;
            this.CurrentMaintain["amount"] = x.ToString();
            this.numTotal.Text = x1.ToString();
            this.CurrentMaintain["vat"] = x2.ToString();

            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand =
                $@"SELECT
                [ContractNumber] = SCAD.ContractNumber
                , [OrderID] = SCAD.OrderID
                , [ComboType] = SCAD.ComboType
                , [Article] = SCAD.Article
                , [Qty] = isnull(SCAD.Qty,0)
                , [Price] = isnull(SCAD.Price,0)
                , [Amount] = isnull(SCAD.Amount,0)
                , [AccuSewingQty] = isnull(AccuSewingQty.val,0)
                , [AccuPaidQty] =  isnull(AccuPaidQty.val,0)
                , [BalQty] = cast(isnull(AccuSewingQty.val,0) as int) -cast( isnull(AccuPaidQty.val,0) as int)
                , SCAD.UKEY
                , SCAD.ID
                FROM SubconOutContractAP_Detail SCAD WITH(NOLOCK)
                LEFT JOIN SubconOutContractAP SCA WITH(NOLOCK) on SCA.ID = SCAD.ID
                OUTER APPLY
                (
	                select val = sum(sd.QAQty)
	                from SewingOutput s
	                inner join SewingOutput_Detail sd on s.ID = sd.ID
	                where s.SubconOutFty = SCA.LocalSuppID
	                and s.SubConOutContractNumber = SCAD.ContractNumber
	                and sd.OrderId = SCAD.OrderID
	                and sd.ComboType = SCAD.ComboType
	                and sd.Article = SCAD.Article
                )AccuSewingQty
                OUTER APPLY
                (
	                select val = sum(Qty)
	                from SubconOutContractAP_Detail sd
	                inner join SubconOutContractAP s on sd.ID = s.ID
	                where sd.ContractNumber = SCAD.ContractNumber
	                and sd.OrderId = SCAD.OrderID
	                and sd.ComboType = SCAD.ComboType
	                and sd.Article = SCAD.Article
	                and s.LocalSuppID = SCA.LocalSuppID
	                and s.ID <> SCA.ID
	                and s.Status <> 'New'
                )AccuPaidQty
                WHERE SCA.ID = '{masterID}'";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region qtygarment Valid
            DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    decimal b = MyUtility.Check.Empty(this.CurrentDetailData["BalQty"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentDetailData["BalQty"]);
                    if ((decimal)e.FormattedValue > b)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("can't over balance qty", "Warning");
                        return;
                    }

                    this.CurrentDetailData["Qty"] = e.FormattedValue;
                    this.CurrentDetailData["amount"] = MyUtility.Convert.GetDecimal(e.FormattedValue) * MyUtility.Convert.GetDecimal(this.CurrentDetailData["price"]);
                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("ContractNumber", header: "Contract No.", width: Widths.AnsiChars(20), iseditingreadonly: true) // 0
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 1
            .Text("ComboType", header: "Combo Type", iseditingreadonly: true) // 2
            .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), settings: ns2) // 4
            .Numeric("Price", header: "Price", width: Widths.AnsiChars(5), integer_places: 12, decimal_places: 4, iseditingreadonly: true) // 5
            .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(12), integer_places: 12, decimal_places: 4, iseditingreadonly: true) // 6
            .Numeric("AccuSewingQty", header: "Accu. Sewing Qty", width: Widths.AnsiChars(6), iseditingreadonly: true) // 7
            .Numeric("AccuPaidQty", header: "Accu. Paid Qty", width: Widths.AnsiChars(6), iseditingreadonly: true) // 8
            .Numeric("BalQty", header: "Bal. Qty", width: Widths.AnsiChars(6), iseditingreadonly: true);    // 9
            #endregion

            #region 可編輯欄位變色
            this.detailgrid.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink; // qty
            #endregion
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["BalQty"] = string.Empty;
            this.CurrentDetailData["qty"] = 0;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["LocalSuppID"]))
            {
                MyUtility.Msg.WarningBox("< Suppiler >  can't be empty!", "Warning");
                this.txtSupplier.TextBox1.Focus();
                return;
            }

            DataRow dr = this.CurrentMaintain;
            var windows = new P12_Import(dr, (DataTable)this.detailgridbs.DataSource);
            windows.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["ISSUEDATE"] = DateTime.Today;
            this.CurrentMaintain["VatRate"] = 0;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Mdivisionid"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["HANDLE"] = Env.User.UserID;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必要欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["ISSUEDATE"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.date_IssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("< Factory ID >  can't be empty!", "Warning");
                this.txtFactory.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["LocalSuppID"]))
            {
                MyUtility.Msg.WarningBox("< Suppiler >  can't be empty!", "Warning");
                this.txtSupplier.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CurrencyID"]))
            {
                MyUtility.Msg.WarningBox("< Currency >  can't be empty!", "Warning");
                this.txtCurrency.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["PaytermID"]))
            {
                MyUtility.Msg.WarningBox("< Terms >  can't be empty!", "Warning");
                this.txtTerms.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                this.txtHandle.Focus();
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            dt = dt.AsEnumerable()
             .Where(a => a.RowState != DataRowState.Deleted).CopyToDataTable();

            List<DataRow> list = dt.Select("Qty = 0").ToList();
            if (list.Count > 0)
            {
                string strMessage = $@"Qty of selected row can't be zero!";
                var errorTable = list.CopyToDataTable();
                errorTable.Columns.Remove("ID");
                errorTable.Columns.Remove("UKEY");
                errorTable.Columns["ContractNumber"].ColumnName = "Contract No.";
                errorTable.Columns["OrderID"].ColumnName = "SP#";
                errorTable.Columns["AccuSewingQty"].ColumnName = "Accu. Sewing Qty";
                errorTable.Columns["AccuPaidQty"].ColumnName = "Accu. Paid Qty";
                errorTable.Columns["BalQty"].ColumnName = "Bal. Qty";
                MessageYESNO win = new MessageYESNO(strMessage, errorTable, "Warning", true);
                win.ShowDialog(this);
                return false;
            }

            #endregion

            #region 檢查重複值

            if (!MyUtility.Check.Empty(this.CurrentMaintain["InvNo"]))
            {
                var sqlcmd_invno = $@"
                select Count(*)
                from SubconOutContractAP 
                where 
                Invno = '{MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"])}' and
                Id <> '{MyUtility.Convert.GetString(this.CurrentMaintain["ID"])}'";

                var count = MyUtility.GetValue.Lookup(sqlcmd_invno);

                if (MyUtility.Convert.GetInt(count) > 0)
                {
                    MyUtility.Msg.WarningBox("< InvNo > value is duplicated!");
                    return false;
                }
            }

            var duplicateRows = dt.AsEnumerable()
            .GroupBy(row => new
            {
                ContractNumber = row.Field<string>("ContractNumber"),
                OrderID = row.Field<string>("OrderID"),
                ComboType = row.Field<string>("ComboType"),
                Article = row.Field<string>("Article"),
            })
            .Where(group => group.Count() > 1)
            .Select(group => group.First())
            .ToArray();

            if (duplicateRows.Length > 0)
            {
                DataTable errorDataTable = new DataTable();
                errorDataTable.Columns.Add("Contract No.", typeof(string));
                errorDataTable.Columns.Add("SP#", typeof(string));
                errorDataTable.Columns.Add("Combo Type", typeof(string));
                errorDataTable.Columns.Add("Article", typeof(string));

                foreach (var row in duplicateRows)
                {
                    errorDataTable.Rows.Add(row["ContractNumber"], row["OrderID"], row["ComboType"], row["Article"]);
                }

                string strMessage = "< Contract No. , SP#, Combo Type, Article > value is duplicated!";
                MessageYESNO win = new MessageYESNO(strMessage, errorDataTable, "Warning", true);
                win.ShowDialog(this);
                return false;
            }

            #endregion

            #region 檢查LocalSupp_Bank
            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["localsuppid"].ToString(), Prgs.CallFormAction.Save);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return false;
            }
            #endregion

            #region 檢查 Qty > Bal. Qty
            List<DataRow> listQty = dt.Select("Qty > BalQty").ToList();
            if (listQty.Count > 0)
            {
                string strMessage = $@"Qty can not more than Bal. Qty!";

                var errorTable = listQty.CopyToDataTable();

                MessageYESNO win = new MessageYESNO(strMessage, errorTable, "Warning", true);
                win.ShowDialog(this);
                return false;
            }
            #endregion

            #region  表頭Amount、Vat 計算

            decimal sumAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("Amount"));

            string sqlcmdCurrencyExact = $@"
            select Exact from Currency where ID = '{this.CurrentMaintain["CurrencyID"]}'
            ";
            int currencyExact = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlcmdCurrencyExact));

            decimal x = 0;
            decimal x1 = 0;
            decimal x2 = 0;
            x = MyUtility.Math.Round(sumAmount, 2);
            x2 = MyUtility.Math.Round(x * (decimal)this.CurrentMaintain["VatRate"] / 100, currencyExact);
            x1 += x + x2;
            this.CurrentMaintain["amount"] = x.ToString();
            this.numTotal.Text = x1.ToString();
            this.CurrentMaintain["vat"] = x2.ToString();

            #endregion

            if (this.IsDetailInserting)
            {
                this.CurrentMaintain["id"] = MyUtility.GetValue.GetID(Env.User.Keyword + "GA", "SubconOutContractAP", (DateTime)this.CurrentMaintain["issuedate"]);
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Approved")
            {
                var frm = new PublicForm.EditRemark("SubconOutContractAP", "Remark", this.CurrentMaintain);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() == "APPROVED")
            {
                MyUtility.Msg.WarningBox("Data is approved, can't delete.", "Warning");
                this.OnRefreshClick();
                return false;
            }

            if (!MyUtility.Check.Seek($"select 1 from SubconOutContractAP where status = 'New' and id = '{this.CurrentMaintain["id"]}'"))
            {
                MyUtility.Msg.WarningBox("Status is not New, can't delete.", "Warning");
                this.OnRefreshClick();
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            bool chk_status = MyUtility.Check.Seek(string.Format("select status from dbo.SubconOutContractAP  where id = '{0}' and status = 'New' ", this.CurrentMaintain["id"]));
            if (chk_status == false)
            {
                MyUtility.Msg.WarningBox(string.Format("This ID<{0}> is already Approved", this.CurrentMaintain["id"]));
                return;
            }

            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["localsuppid"].ToString(), Prgs.CallFormAction.Confirm);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return;
            }

            #region 檢查 Qty > Bal. Qty
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            List<DataRow> listQty = dt.Select("Qty > BalQty").ToList();
            if (listQty.Count > 0)
            {
                string strMessage = $@"Qty can not more than Bal. Qty!";

                var errorTable = listQty.CopyToDataTable();

                MessageYESNO win = new MessageYESNO(strMessage, errorTable, "Warning", true);
                win.ShowDialog(this);
                return;
            }

            #endregion

            string sqlcmd =
            $@"update SubconOutContractAP
            set 
            status='Approved',
            apvname='{Env.User.UserID}',
            apvdate = GETDATE() ,
            editname = '{Env.User.UserID}' ,
            editdate = GETDATE()
            where id = '{this.CurrentMaintain["id"]}'";

            DualResult dualResult = DBProxy.Current.Execute(null, sqlcmd);
            if (!dualResult)
            {
                this.ShowErr(sqlcmd, dualResult);
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            bool chk_status = MyUtility.Check.Seek(string.Format("select status from dbo.SubconOutContractAP  where id = '{0}' and status = 'Approved' ", this.CurrentMaintain["id"]));
            if (chk_status == false)
            {
                MyUtility.Msg.WarningBox(string.Format("This ID<{0}> is already Unapproved", this.CurrentMaintain["id"]));
                return;
            }

            base.ClickUnconfirm();
            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string sqlcmd =
            $@"update SubconOutContractAP
            set 
            status='New',
            apvname='',
            apvdate = null ,
            editname = '' ,
            editdate = null
            where id = '{this.CurrentMaintain["id"]}'";

            DualResult dualResult = DBProxy.Current.Execute(null, sqlcmd);
            if (!dualResult)
            {
                this.ShowErr(sqlcmd, dualResult);
                return;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));

            string sqlcmd = $@"
            select b.nameEn 
		    ,AddressEN = REPLACE(REPLACE(b.AddressEN,Char(13),''),Char(10),'')
		    ,b.Tel
		    ,a.Id
            ,c.name
		    ,c.Address
		    ,c.Tel
		    ,a.PaytermID+d.Name [Terms]
		    ,a.InvNo [Invoice]
		    ,a.Remark [Remark]
		    ,LocalSuppBank.AccountNo [AC_No]
            ,LocalSuppBank.AccountName [AC_Name]
            ,LocalSuppBank.BankName [Bank_Name]
            ,LocalSuppBank.CountryID [Country]
            ,LocalSuppBank.city [city] 
            ,LocalSuppBank.swiftcode [SwiftCode]
            ,f.ID+'-'+f.Name [Prepared_by]
            ,a.CurrencyID[CurrencyID]
		    ,a.VatRate[VatRate]
		    ,a.Vat
		    ,a.CurrencyID
		    ,a.Amount
		    ,a.Vat
		    ,[GrandTotal] = format(isnull(a.Amount,0) + isnull(a.Vat,0),'#,###,###,##0.00')
            from dbo.SubconOutContractAP a WITH (NOLOCK) 
            left join dbo.factory  b WITH (NOLOCK) on b.id = a.factoryid
            inner join dbo.LocalSupp c WITH (NOLOCK) on c.id=a.LocalSuppID
            left join dbo.PayTerm d WITH (NOLOCK) on d.id=a.PaytermID
            left join dbo.Pass1 f WITH (NOLOCK) on f.id=a.Handle
            left join dbo.Currency cr WITH (NOLOCK) on cr.ID = a.CurrencyID
            OUTER APPLY(
	            SELECT    [AccountNo]= IIF(lb.ByCheck=1,'',lbd.Accountno )
		                , [AccountName]=IIF(lb.ByCheck=1,'',lbd.AccountName )
		                , [BankName]=IIF(lb.ByCheck=1,'',lbd.BankName )
		                , [CountryID]=IIF(lb.ByCheck=1,'',lbd.CountryID)
		                , [City]=IIF(lb.ByCheck=1,'',lbd.City)
		                , [SwiftCode]=IIF(lb.ByCheck=1,'',lbd.SwiftCode )
	            FROM LocalSupp_Bank lb
	            INNER JOIN LocalSupp_Bank_Detail lbd ON lb.ID=lbd.ID AND lb.PKey=lbd.Pkey
	            WHERE lb.ID=a.LocalSuppID  
		            AND lb.ApproveDate = (SElECT MAX(ApproveDate) FROM  LocalSupp_Bank WHERE Status='Confirmed' AND ID= a.LocalSuppID )
		            AND lbd.IsDefault=1
            )LocalSuppBank
            where a.id = @ID";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            ReportDefinition report = new ReportDefinition();
            string rptTitle = dt.Rows[0]["nameEn"].ToString();
            string address = dt.Rows[0]["AddressEN"].ToString();
            string tel = dt.Rows[0]["Tel"].ToString();
            string barcode = dt.Rows[0]["Id"].ToString();
            string supplier = dt.Rows[0]["name"].ToString();
            string address1 = dt.Rows[0]["Address"].ToString();
            string tEL1 = dt.Rows[0]["Tel"].ToString();
            string terms = dt.Rows[0]["Terms"].ToString();
            string invoice = dt.Rows[0]["Invoice"].ToString();
            string remark = dt.Rows[0]["Remark"].ToString();
            string aC_No = dt.Rows[0]["AC_No"].ToString();
            string aC_Name = dt.Rows[0]["AC_Name"].ToString();
            string bank_Name = dt.Rows[0]["Bank_Name"].ToString();
            string country = dt.Rows[0]["Country"].ToString();
            string city = dt.Rows[0]["city"].ToString();
            string swiftCode = dt.Rows[0]["SwiftCode"].ToString();
            string total = this.numAmount.Text;
            string vat = this.numVat.Text;
            string grand_Total = dt.Rows[0]["GrandTotal"].ToString();
            string prepared_by = dt.Rows[0]["Prepared_by"].ToString();
            string currencyID = dt.Rows[0]["CurrencyID"].ToString();
            string vatRate = dt.Rows[0]["VatRate"].ToString();
            int totalQty = this.DetailDatas.AsEnumerable().Sum(x => MyUtility.Convert.GetInt(x["Qty"]));

            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("address", address));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Tel", tel));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", barcode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", supplier));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Address1", address1));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL1", tEL1));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("id", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Terms", terms));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", invoice));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TotalQty", MyUtility.Convert.GetString(totalQty)));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AC_No", aC_No));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AC_Name", aC_Name));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Bank_Name", bank_Name));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Country", country + " / " + city));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SwiftCode", swiftCode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Total", total));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Vat", vat));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Grand_Total", grand_Total));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Prepared_by", prepared_by));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CurrencyID", currencyID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("VatRate", vatRate));


            List<P12_PrintData> data = this.DetailDatas.AsEnumerable()
            .Select(row1 => new P12_PrintData()
            {
                ContractNo = row1["ContractNumber"].ToString(),
                SP = row1["orderid"].ToString(),
                ComboType = row1["ComboType"].ToString(),
                Article = row1["Article"].ToString(),
                Price = $"{MyUtility.Convert.GetDecimal(row1["Price"]):###,###,###,##0.00}",
                Qty = row1["Qty"].ToString(),
                Amount = $"{MyUtility.Convert.GetDecimal(row1["Amount"]):###,###,###,##0.00}",
            }).ToList();

            report.ReportDataSource = data;

            #region  指定是哪個 RDLC

            // DualResult result;
            Type reportResourceNamespace = typeof(P12_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P12_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();
            return true;
        }

        private void TxtFactory_Validated(object sender, EventArgs e)
        {
            if (this.txtFactory.OldValue != this.txtFactory.Text)
            {
                DataTable dt = (DataTable)this.detailgridbs.DataSource;

                string sqlcmd_MDivision = $@" Select MDivisionID from Factory where ID = '{this.txtFactory.Text}'";
                this.txtMDivision.Text = MyUtility.GetValue.Lookup(sqlcmd_MDivision);

                if (dt.Rows.Count == 0 || this.txtFactory.OldValue == string.Empty)
                {
                    return;
                }

                DataRow dataRow = dt.Rows[0];
                string sqlcmd_Factory = $@"
                select 
                FactoryID 
                from SubconOutContract 
                where 
                SubConOutFty = '{this.CurrentMaintain["LocalSuppID"]}' and
                ContractNumber = '{dataRow["ContractNumber"]}'";
                string checkFactory = MyUtility.GetValue.Lookup(sqlcmd_Factory);

                if (this.txtFactory.Text != checkFactory)
                {
                    DialogResult confirmResult = MessageBoxEX.Show($@"The factory is not the same as before, do you want to clear it?", "Warning", MessageBoxButtons.YesNo, new string[] { "Yes", "No" }, MessageBoxDefaultButton.Button2);
                    if (confirmResult.EqualString("Yes"))
                    {
                        ((DataTable)this.detailgridbs.DataSource).Rows.Clear();
                    }
                }
            }
        }

        private void TxtSupplier_Validated(object sender, EventArgs e)
        {
            if (this.txtSupplier.TextBox1.OldValue != this.txtSupplier.TextBox1.Text)
            {
                DataTable dt = (DataTable)this.detailgridbs.DataSource;
                if (dt.Rows.Count == 0 || this.txtSupplier.TextBox1.OldValue == string.Empty)
                {
                    return;
                }

                DataRow dataRow = dt.Rows[0];
                string sqlcmd_SubConOutFty = $@"
                select 
                SubConOutFty  
                from SubconOutContract 
                where
                ContractNumber = '{dataRow["ContractNumber"]}'";
                string checkFactory = MyUtility.GetValue.Lookup(sqlcmd_SubConOutFty);

                if (this.txtSupplier.TextBox1.Text != checkFactory)
                {
                    DialogResult confirmResult = MessageBoxEX.Show($@"The new supplier dis-match with the Contract No., do you still want to change the supplier?", "Warning", MessageBoxButtons.YesNo, new string[] { "Yes", "No" }, MessageBoxDefaultButton.Button2);
                    if (confirmResult == DialogResult.Yes)
                    {
                        ((DataTable)this.detailgridbs.DataSource).Rows.Clear();
                    }
                    else
                    {
                        this.txtSupplier.TextBox1.Text = this.txtSupplier.TextBox1.OldValue;
                        return;
                    }
                }
            }
        }
    }
}
