using System;
using System.Data;
using System.Drawing;
using Ict.Win;

namespace Sci.Production.Subcon
{
    public partial class P37_DebitSchedule : Win.Subs.Input4
    {
        private DataRow Master;
        private string _FromFuncton;
        private bool _isTaipeiDBC;
        private string _CurrencyID;
        private string _TaipeiCurrencyID;

        public P37_DebitSchedule(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow _Master, string FromFuncton, bool isTaipeiDBC = true)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.Master = _Master;
            this.KeyField1 = "ID";
            this.WorkAlias = "Debit_Schedule";
            this._FromFuncton = FromFuncton;
            this._isTaipeiDBC = isTaipeiDBC;
            this._CurrencyID = this.Master["CurrencyID"].ToString();

            // 固定是USD，如果有需要換從這邊改
            this._TaipeiCurrencyID = "USD";
            this.numericBoxTotal.ReadOnly = true;
        }

        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings amountSetting = new DataGridViewGeneratorNumericColumnSettings();

            amountSetting.CellValidating += (s, e) =>
             {
                 if (this.EditMode)
                 {
                     DataTable dt = (DataTable)this.gridbs.DataSource;
                     DataRow currDr = dt.Rows[e.RowIndex];
                     currDr["amount"] = e.FormattedValue;

                     decimal ttl = 0;
                     foreach (DataRow dr in dt.Rows)
                     {
                         ttl += MyUtility.Check.Empty(dr["amount"]) ? 0 : Convert.ToDecimal(dr["amount"]);
                     }

                     this.numericBoxTotal.Text = ttl.ToString();
                 }
             };

            this.Helper.Controls.Grid.Generator(this.grid)
                .Date("issuedate", header: "Debit Date", width: Widths.AnsiChars(10))
                .Text("CurrencyID", header: "Debit Currency", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Numeric("amount", header: "Deibt Amount", integer_places: 12, decimal_places: 2, settings: amountSetting)
                .Text("voucherid", header: "Voucher No.", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .DateTime("VOUCHERDATE", header: "Voucher Date", width: Widths.AnsiChars(10), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMdd)
                .Text("ExVoucherID", header: "Ex Voucher No.", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .DateTime("addDate", header: "Create Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
                .Text("addName", header: "Create Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("editDate", header: "Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
                .Text("editName", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;

            if (this._FromFuncton == "P36")
            {
                this.grid.Columns["issuedate"].DefaultCellStyle.BackColor = Color.Pink;
                this.grid.Columns["amount"].DefaultCellStyle.BackColor = Color.Pink;
            }

            return true;
        }

        protected override void OnRequeryPost(DataTable datas)
        {
            decimal ttl = 0;
            datas.Columns.Add("VOUCHERDATE");
            foreach (DataRow dr in datas.Rows)
            {
                dr["VOUCHERDATE"] = MyUtility.GetValue.Lookup(string.Format("SELECT VoucherDate from SciFMS_Voucher WITH (NOLOCK) where id = '{0}'", dr["voucherid"]));

                // 根據Debit是否為台北建立的，修改自動帶入的幣別
                /*if (_isTaipeiDBC)
                    dr["CurrencyID"] = this._TaipeiCurrencyID;
                else
                    dr["CurrencyID"] = this._CurrencyID;
                    */
                ttl += MyUtility.Check.Empty(dr["amount"]) ? 0 : Convert.ToDecimal(dr["amount"]);
            }

            // this.grid.AutoResizeColumns();
            this.numericBoxTotal.Text = ttl.ToString();

            if (this._FromFuncton == "P36")
            {
                this.grid.Columns["issuedate"].DefaultCellStyle.ForeColor = Color.Red;
                this.grid.Columns["amount"].DefaultCellStyle.ForeColor = Color.Red;
            }

            base.OnRequeryPost(datas);
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.revise.Enabled = false;
            this.revise.Visible = false;
        }

        protected override void OnDelete()
        {
            if (this.CurrentData != null)
            {
                if (!MyUtility.Check.Empty(this.CurrentData["voucherid"]))
                {
                    MyUtility.Msg.WarningBox("< Voucher No. > is not empty, can't delete!!");
                    return;
                }
            }

            base.OnDelete();
        }

        protected override bool OnSaveBefore()
        {
            decimal GridAmount = 0;
            foreach (DataRow dr in this.Datas)
            {
                if (MyUtility.Check.Empty(dr["IssueDate"]) || MyUtility.Check.Empty(dr["Amount"]))
                {
                    MyUtility.Msg.WarningBox("< Debit Date >  & < Debit Amount > can't be empty!!");
                    return false;
                }

                GridAmount += (decimal)dr["Amount"];
            }

            // 根據Debit是否為台北建立的，修改判斷的數字
            if (this._isTaipeiDBC)
            {
                if ((decimal)this.Master["TaipeiAMT"] < GridAmount)
                {
                    MyUtility.Msg.WarningBox("Total deibt amount more than DBC amount, cann't save!!");
                    return false;
                }
            }
            else
            {
                if ((decimal)this.Master["Amount"] < GridAmount)
                {
                    MyUtility.Msg.WarningBox("Total deibt amount more than DBC amount, cann't save!!");
                    return false;
                }
            }

            return base.OnSaveBefore();
        }

        protected override void OnInsertPrepare(DataRow data)
        {
            base.OnInsertPrepare(data);

            // 根據Debit是否為台北建立的，修改自動帶入的幣別
            if (this._isTaipeiDBC)
            {
                data["CurrencyID"] = this._TaipeiCurrencyID;
            }
            else
            {
                data["CurrencyID"] = this._CurrencyID;
            }
        }
    }
}
