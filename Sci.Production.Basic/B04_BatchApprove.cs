using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B04_BatchApprove : Win.Forms.Base
    {
        Action reloadParant;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk2 = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private DataTable master;
        private DataTable detail;

        public B04_BatchApprove(Action ReloadParant)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.reloadParant = ReloadParant;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk)
                .Text("ID", header: "Code", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Abb", header: "Abbreviation", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("ByCheck", header: "Pay by Check", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Status", header: "Status", width: Widths.AnsiChars(6), iseditingreadonly: true)
                /*.Text("AccountIDN", header: "Account No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("sLocalSuppID", header: "Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(7), decimal_places: 5, iseditingreadonly: true)
                .Text("sNewSupp", header: "New Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("NewCurrency", header: "New Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NewPrice", header: "New Price", width: Widths.AnsiChars(7), decimal_places: 5, iseditingreadonly: true)*/
                ;

            this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("IsDefault", header: "Default", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk2)

                // .Text("IsDefault", header: "Default", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("AccountNo", header: "Account No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SWIFTCode", header: "Swift", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("AccountName", header: "Account Name", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BankName", header: "Bank Name", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BranchCode", header: "Branch Code", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BranchName", header: "Branch Name", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("CountryID", header: "Country", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Alias", header: "Country Name", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("City", header: "City", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("MidBankName", header: "Intermediary Bank", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("MidSWIFTCode", header: "Intermediary Bank-SWIFT Code", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(6), iseditingreadonly: true)
                ;

            for (int i = 0; i < this.grid2.Columns.Count; i++)
            {
                this.grid2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            this.Query();
            this.listControlBindingSource1.Filter = "Status='New'";
        }

        private void Query()
        {
            DataSet datas = null;
            this.master = null;
            this.detail = null;
            #region
            string sqlCmd =
                $@"

SELECT  DISTINCT
    Selected = 0
    ,lb.ID
    ,lb.PKey
    ,l.Abb
    ,[ByCheck]=iif(lb.ByCheck=1,'Y','N')
    ,lb.Status
INTO #Master
FROM LocalSupp l
INNER JOIN LocalSupp_Bank lb ON l.ID=lb.ID
INNER JOIN LocalSupp_Bank_Detail lbd ON lb.ID=lbd.ID AND lb.PKey=lbd.PKey
WHERE 1=1
AND(lb.ByCheck=1 OR lbd.IsDefault=1)

SELECT * FROM #Master

SELECT 
    lbd.ID
    ,lbd.AccountNo
    ,IsDefault
    ,SWIFTCode
    ,AccountName
    ,BankName
    ,BranchCode
    ,BranchName
    ,CountryID
    ,c.Alias
    ,City
    ,MidBankName
    ,MidSWIFTCode
    ,Remark
	,l.PKey
FROM LocalSupp_Bank_Detail lbd
INNER JOIN #Master l ON l.ID=lbd.ID AND l.PKey = lbd.PKey
LEFT JOIN Country c ON c.ID=lbd.CountryID
ORDER BY  lbd.ID, lbd.PKey

DROP TABLE #Master




";
            #endregion
            if (!SQL.Selects(string.Empty, sqlCmd, out datas))
            {
                MyUtility.Msg.WarningBox(sqlCmd, "DB error!!");
                return;
            }

            if (this.listControlBindingSource1.DataSource != null)
            {
                this.listControlBindingSource1.DataSource = null;
            }

            if (this.listControlBindingSource2.DataSource != null)
            {
                this.listControlBindingSource2.DataSource = null;
            }

            datas.Tables[0].AcceptChanges();
            datas.Tables[1].AcceptChanges();

            if (datas.Tables[0].Rows.Count == 0)
            {
                return;
            }

            this.master = datas.Tables[0];
            this.master.TableName = "Master";

            this.detail = datas.Tables[1];
            this.detail.TableName = "Detail";

            DataRelation relation = new DataRelation(
                "rel1",
                new DataColumn[] { this.master.Columns["ID"], this.master.Columns["PKey"] },
                new DataColumn[] { this.detail.Columns["ID"], this.detail.Columns["PKey"] });

            datas.Relations.Add(relation);

            this.listControlBindingSource1.DataSource = datas;
            this.listControlBindingSource1.DataMember = "Master";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource2.DataMember = "rel1";
            this.grid1.AutoResizeColumns();

            this.grid2.AutoResizeColumns();

            for (int i = 0; i < this.grid1.Rows.Count; i++)
            {
                if (this.grid1.Rows[i].Cells["Status"].Value.ToString() == "Confirmed")
                {
                    this.grid1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }

        private void chkIncludeApproved_CheckedChanged(object sender, EventArgs e)
        {
            // 若勾選，不判斷[LocalSupp_Bank].Status, 符合條件資料全帶出, Status='Confirmed' 底色反灰
            if (this.chkIncludeApproved.Checked)
            {
                this.listControlBindingSource1.Filter = "Status='New' OR Status='Confirmed'";

                for (int i = 0; i < this.grid1.Rows.Count; i++)
                {
                    if (this.grid1.Rows[i].Cells["Status"].Value.ToString() == "Confirmed")
                    {
                        this.grid1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }
            }
            else
            {
                this.listControlBindingSource1.Filter = "Status='New' ";

                for (int i = 0; i < this.grid1.Rows.Count; i++)
                {
                    this.grid1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void btnconfirm_Click(object sender, EventArgs e)
        {
            if (this.master == null || this.master.Rows.Count == 0)
            {
                return;
            }

            this.grid1.ValidateControl();
            if (this.master.Select("Selected = 1 AND Status='New'").Length == 0)
            {
                MyUtility.Msg.WarningBox("Must select Unconfirmed datas!");
                return;
            }

            DataTable selectdt = this.master.Select("Selected = 1 AND Status='New'").CopyToDataTable();

            DualResult upResult;
            string chkstatus = $@"
SELECT t.ID ,t.PKey
FROM #tmp s
INNER JOIN LocalSupp_Bank t on s.ID=t.ID AND s.PKey = t.PKey
WHERE t.status = 'New'
";
            DataTable dt;
            if (!(upResult = MyUtility.Tool.ProcessWithDatatable(selectdt, string.Empty, chkstatus, out dt)))
            {
                this.ShowErr(upResult);
                return;
            }

            // 若有單已經被其他使用者先approve則跳過, 加上status = 'New' 為更新條件
            string updSql = $@"
Update  l
set l.Status = 'Confirmed' ,l.ApproveName='{Sci.Env.User.UserID}' ,l.ApproveDate=GETDATE() ,l.editname = '{Env.User.UserID}', l.editdate = GETDATE() 
FROM LocalSupp_Bank l
INNER JOIN #tmp t ON t.ID = l.ID AND t.PKey = l.PKey



/*
merge ShipExpense t
using #tmp s
on t.ID = s.ID
when matched then update set
    t.localsuppid = s.NewSupp,
    t.currencyid = s.NewCurrency,
    t.price = s.NewPrice,
    t.CanvassDate = s.CanvassDate1, 
    t.editname = '{Env.User.UserID}',
    t.editdate = GETDATE()*/
;
";

            using (TransactionScope scope = new TransactionScope())
            {
                if (!(upResult = MyUtility.Tool.ProcessWithDatatable(dt, "ID,PKey", updSql, out dt)))
                {
                    this.ShowErr(upResult);
                    return;
                }

                scope.Complete();
            }

            MyUtility.Msg.InfoBox("Success!");

            this.Query();
            this.reloadParant();
        }
    }
}
