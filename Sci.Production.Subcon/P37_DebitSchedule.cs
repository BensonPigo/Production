using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Win;
using Sci.Production.Class.Command;

namespace Sci.Production.Subcon
{
    public partial class P37_DebitSchedule : Win.Subs.Input4
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataRow Master;
        private string _FromFuncton;
        private bool _isTaipeiDBC;
        private string _CurrencyID;
        private string _TaipeiCurrencyID;

        private bool _isHandlingValidation = false;

        public P37_DebitSchedule(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow master, string fromFuncton, bool isTaipeiDBC = true)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.Master = master;
            this.KeyField1 = "ID";
            this.WorkAlias = "Debit_Schedule";
            this._FromFuncton = fromFuncton;
            this._isTaipeiDBC = isTaipeiDBC;
            this._CurrencyID = this.Master["CurrencyID"].ToString();

            // 固定是USD，如果有需要換從這邊改
            this._TaipeiCurrencyID = "USD";
            this.numericBoxTotal.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings amountSetting = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings badDebtSetting = new DataGridViewGeneratorCheckBoxColumnSettings();

            amountSetting.CellValidating += (sender, args) =>
            {
                if (this.EditMode)
                {
                    DataTable dt = (DataTable)this.gridbs.DataSource;
                    DataRow currDr = dt.Rows[args.RowIndex];
                    currDr["amount"] = args.FormattedValue;

                    decimal ttl = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        ttl += MyUtility.Check.Empty(dr["amount"]) ? 0 : Convert.ToDecimal(dr["amount"]);
                    }

                    this.numericBoxTotal.Text = ttl.ToString();
                }
            };

            this.grid.DataError += (s, e) =>
            {
                e.ThrowException = false;
            };

            badDebtSetting.CellValidating += (sender, args) =>
            {
                if (_isHandlingValidation) return;
                _isHandlingValidation = true;

                try
                {
                    if (this.EditMode)
                    {
                        DataGridViewRow row = this.grid.Rows[args.RowIndex];
                        int colVoucherid = this.grid.Columns["voucherid"].Index;
                        int colBadDebt = this.grid.Columns["BadDebt"].Index;
                        bool check = row.Cells[0].Value != null && row.Cells[0].Value != DBNull.Value && row.Cells[0].Value.ToString().ToLower() == "true";

                        if (row.Cells[colVoucherid].Value != null && !string.IsNullOrWhiteSpace(row.Cells[colVoucherid].Value.ToString()))
                        {
                            row.Cells[colBadDebt].ReadOnly = true;
                            row.Cells[colBadDebt].Style.BackColor = Color.LightGray;
                        }
                        else
                        {
                            row.Cells[colBadDebt].ReadOnly = false;
                            row.Cells[colBadDebt].Style.BackColor = Color.White;
                        }

                        row.Cells[0].Value = (object)check;
                    }
                }
                finally
                {
                    _isHandlingValidation = false;
                }
            };

            this.grid.CellClick += (sender, args) =>
            {
                if (args.RowIndex < 0 || args.RowIndex >= this.grid.Rows.Count) return;

                int colVoucherid = this.grid.Columns["voucherid"].Index;
                int colBadDebt = this.grid.Columns["BadDebt"].Index;

                DataGridViewRow row = this.grid.Rows[args.RowIndex];

                if (row.Cells[colVoucherid].Value != null && !string.IsNullOrWhiteSpace(row.Cells[colVoucherid].Value.ToString()))
                {
                    row.Cells[colBadDebt].ReadOnly = true;
                    row.Cells[colBadDebt].Style.BackColor = Color.LightGray;
                }
                else
                {
                    row.Cells[colBadDebt].ReadOnly = false;
                    row.Cells[colBadDebt].Style.BackColor = Color.White;
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("BadDebt", header: "Bad Debit", width: Widths.AnsiChars(10), settings: badDebtSetting)
                .Date("issuedate", header: "Debit Date", width: Widths.AnsiChars(10))
                .Text("CurrencyID", header: "Debit Currency", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Numeric("amount", header: "Deibt Amount", integer_places: 12, decimal_places: 2, settings: amountSetting)
                .Text("voucherid", header: "Voucher No.", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .DateTime("VOUCHERDATE", header: "Voucher Date", width: Widths.AnsiChars(10), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMdd)
                .Text("ExVoucherID", header: "Ex Voucher No.", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .DateTime("addDate", header: "Create Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
                .Text("addName", header: "Create Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("editDate", header: "Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
                .Text("editName", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true);

            if (this._FromFuncton == "P36")
            {
                this.grid.Columns["issuedate"].DefaultCellStyle.BackColor = Color.Pink;
                this.grid.Columns["amount"].DefaultCellStyle.BackColor = Color.Pink;
            }

            this.grid.Columns.DisableSortable();

            return true;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.revise.Enabled = false;
            this.revise.Visible = false;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            decimal gridAmount = 0;
            foreach (DataRow dr in this.Datas)
            {
                if (MyUtility.Check.Empty(dr["IssueDate"]) || MyUtility.Check.Empty(dr["Amount"]))
                {
                    MyUtility.Msg.WarningBox("< Debit Date >  & < Debit Amount > can't be empty!!");
                    return false;
                }

                gridAmount += (decimal)dr["Amount"];
            }

            // 根據Debit是否為台北建立的，修改判斷的數字
            if (this._isTaipeiDBC)
            {
                if ((decimal)this.Master["TaipeiAMT"] < gridAmount)
                {
                    MyUtility.Msg.WarningBox("Total deibt amount more than DBC amount, cann't save!!");
                    return false;
                }
            }
            else
            {
                if ((decimal)this.Master["Amount"] < gridAmount)
                {
                    MyUtility.Msg.WarningBox("Total deibt amount more than DBC amount, cann't save!!");
                    return false;
                }
            }

            return base.OnSaveBefore();
        }

        /// <inheritdoc/>
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
