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

namespace Sci.Production.Subcon
{
    public partial class P37_DebitSchedule : Sci.Win.Subs.Input4
    {
        private DataRow Master;
        public P37_DebitSchedule(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow _Master)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            this.Master = _Master;
            this.KeyField1 = "ID";
            this.WorkAlias = "Debit_Schedule";
        }
        
        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Date("issuedate", header: "Debit Date", width: Widths.AnsiChars(10))
                .Text("CurrencyID", header: "Debit Currency", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Numeric("amount", header: "Deibt Amount", integer_places: 12, decimal_places: 2)
                .Text("voucherid", header: "Voucher No.", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .DateTime("VOUCHERDATE", header: "Voucher Date", width: Widths.AnsiChars(10), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMdd)
                .Text("ExVoucherID", header: "Ex Voucher No.", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .DateTime("addDate", header: "Create Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
                .Text("addName", header: "Create Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("editDate", header: "Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
                .Text("editName", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;


            grid.Columns["issuedate"].DefaultCellStyle.BackColor = Color.Pink;
            grid.Columns["amount"].DefaultCellStyle.BackColor = Color.Pink;

            return true;
        }
     
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);

            datas.Columns.Add("VOUCHERDATE");
            foreach (DataRow dr in datas.Rows)
            {
                dr["VOUCHERDATE"] = MyUtility.GetValue.Lookup(string.Format("SELECT VoucherDate from [FinanceEN].[dbo].[Voucher] WITH (NOLOCK) where id = '{0}'", dr["voucherid"]));

                dr["CurrencyID"] = "USD";
            }
            //this.grid.AutoResizeColumns();


            grid.Columns["issuedate"].DefaultCellStyle.ForeColor = Color.Red;
            grid.Columns["amount"].DefaultCellStyle.ForeColor = Color.Red;
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            revise.Enabled = false;
            revise.Visible = false;
        }

        protected override void OnDelete()
        {
            if (CurrentData != null)
            {
                if (!MyUtility.Check.Empty(CurrentData["voucherid"]))
                {
                    MyUtility.Msg.WarningBox("< Voucher No. > is not empty, can't delete!!");
                    return;
                }
            }
            base.OnDelete();
        }

        protected override bool OnSaveBefore()
        {
            decimal Amount = 0;
            foreach (DataRow dr in Datas)
            {
                if (MyUtility.Check.Empty(dr["IssueDate"]) || MyUtility.Check.Empty(dr["Amount"]))
                {
                    MyUtility.Msg.WarningBox("< Debit Date >  & < Debit Amount > can't be empty!!");
                    return false;
                }
                Amount += (decimal)dr["Amount"];
            }

            if ((decimal)Master["TaipeiAMT"] < Amount)
            {
                MyUtility.Msg.WarningBox("Total deibt amount more than DBC amount, cann't save!!");
                return false;
            }

            return base.OnSaveBefore();
        }
    }
}
