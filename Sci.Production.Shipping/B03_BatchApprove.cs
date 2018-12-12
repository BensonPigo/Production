using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class B03_BatchApprove : Sci.Win.Forms.Base
    {
        Action reloadParant;

        /// <inheritdoc/>
        public B03_BatchApprove(Action ReloadParant)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.reloadParant = ReloadParant;
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk2 = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private DataTable master;
        private DataTable detail;

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk)
                .Text("ID", header: "Code", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("AccountIDN", header: "Account No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("LocalSuppID", header: "Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                .Text("NewSupp", header: "New Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("NewCurrency", header: "New Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NewPrice", header: "New Price", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk2)
                .Text("LocalSuppID", header: "Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SuppAbb", header: "Supp Abb", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                ;

            this.Query();
        }

        /// <inheritdoc/>
        public string Sqlcmd(string id = "", string ukey = "")
        {
            string wheresql = string.Empty;
            if (!MyUtility.Check.Empty(id))
            {
                wheresql = $" and l.id = '{id}'";
            }

            if (!MyUtility.Check.Empty(ukey))
            {
                wheresql = $" and lq.ukey = '{ukey}'";
            }

            string sqlcmd;
            return sqlcmd = $@"
select l.*,
	NewSupp = case when ChooseSupp = 1 then localsuppid1
				   when ChooseSupp = 2 then localsuppid2
				   when ChooseSupp = 3 then localsuppid3
				   when ChooseSupp = 4 then localsuppid4 end,
	NewCurrency = case 
				   when ChooseSupp = 1 then currencyid1
				   when ChooseSupp = 2 then currencyid2
				   when ChooseSupp = 3 then currencyid3
				   when ChooseSupp = 4 then currencyid4 end,
	NewPrice = case 
				   when ChooseSupp = 1 then price1
				   when ChooseSupp = 2 then price2
				   when ChooseSupp = 3 then price3
				   when ChooseSupp = 4 then price4 end,
    Selected = 0,
	lq.Ukey,
	CanvassDate1=lq.AddDate,
	AccountIDN=concat(AccountID,' ',(select Name from  FinanceEN.dbo.AccountNo with (nolock) where junk = 0 and id = AccountID))
from ShipExpense l
inner join ShipExpense_CanVass lq on l.ID = lq.ID
where lq.status <> 'Confirmed'
and l.junk = 0
{wheresql}
ORDER BY l.ID
";
        }

        private void Query()
        {
            DataSet datas = null;
            this.master = null;
            this.detail = null;
            #region
            string sqlCmd = this.Sqlcmd() +
                $@"
select	sc.ID
		, sc.Ukey
		, sc.ChooseSupp
		, sc.[Status]
into #bas
from ShipExpense_CanVass sc
inner join ShipExpense s on s.id = sc.id
where sc.[Status] != 'Confirmed'  and s.junk=0

select	[Selected] = iif (tmp.ChooseSupp = tmp.seq, 1, 0)
		, *
from (
	select *
	from #bas bas
	outer apply (
		select	seq = 1
				, LocalSuppID = liq.LocalSuppID1
				, CurrencyID = liq.CurrencyID1
				, Price = liq.Price1
		from ShipExpense_CanVass liq
		where bas.Ukey = liq.Ukey
	) one

	union all 
	select *
	from #bas bas
	outer apply (
		select	seq = 2
				, LocalSuppID = liq.LocalSuppID2
				, CurrencyID = liq.CurrencyID2
				, Price = liq.Price2
		from ShipExpense_CanVass liq
		where bas.Ukey = liq.Ukey
	) two

	union all 
	select *
	from #bas bas
	outer apply (
		select	seq = 3
				, LocalSuppID = liq.LocalSuppID3
				, CurrencyID = liq.CurrencyID3
				, Price = liq.Price3
		from ShipExpense_CanVass liq
		where bas.Ukey = liq.Ukey
	) three

	union all 
	select *
	from #bas bas
	outer apply (
		select	seq = 4
				, LocalSuppID = liq.LocalSuppID4
				, CurrencyID = liq.CurrencyID4
				, Price = liq.Price4
		from ShipExpense_CanVass liq
		where bas.Ukey = liq.Ukey
	) four
) tmp
outer apply(select SuppAbb = abb from LocalSupp l where l.id=LocalSuppID)abb
order by Ukey, seq

drop table #bas
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

            var query = from t in datas.Tables[0].AsEnumerable()
                        group t by new { t1 = MyUtility.Convert.GetString(t["id"]) } into m
                        select new
                        {
                            id = m.Key.t1,
                            ct = m.Count()
                        };

            List<string> msg = new List<string>();
            if (query.ToList().Count > 0)
            {
                query.ToList().ForEach(q =>
                {
                    if (q.ct > 1)
                    {
                        msg.Add(q.id);
                        foreach (var item in datas.Tables[0].Select($"id = '{q.id}'"))
                        {
                            item.Delete();
                        }

                        foreach (var item in datas.Tables[1].Select($"id = '{q.id}'"))
                        {
                            item.Delete();
                        }
                    }
                });
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
                    new DataColumn[] { this.master.Columns["ID"] },
                    new DataColumn[] { this.detail.Columns["ID"] });

            datas.Relations.Add(relation);

            this.listControlBindingSource1.DataSource = datas;
            this.listControlBindingSource1.DataMember = "Master";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource2.DataMember = "rel1";
            this.grid1.AutoResizeColumns();
            this.grid1.Columns["Description"].Width = 100;
            this.grid2.AutoResizeColumns();
            if (msg.Count > 0)
            {
                MyUtility.Msg.WarningBox($@"Code have more than one new quotation, please handle those individually. 
Code{ string.Join(",", msg)}.");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Btnconfirm_Click(object sender, EventArgs e)
        {
            if (this.master == null || this.master.Rows.Count == 0)
            {
                return;
            }

            this.grid1.ValidateControl();
            if (this.master.Select("Selected = 1").Length == 0)
            {
                MyUtility.Msg.WarningBox("Must select datas!");
                return;
            }

            DataTable selectdt = this.master.Select("Selected = 1").CopyToDataTable();

            List<string> chkmsg = new List<string>();
            foreach (DataRow drow in selectdt.Rows)
            {
                if (MyUtility.Check.Empty(drow["NewSupp"]) || MyUtility.Check.Empty(drow["NewCurrency"]) || MyUtility.Check.Empty(drow["NewPrice"]))
                {
                    chkmsg.Add($"[{drow["ID"]}]");
                }
            }

            if (chkmsg.Count > 0)
            {
                MyUtility.Msg.WarningBox(@" [NewSupp] or [NewCurrency] or [NewPrice] can not empty!
" + "Code: " + string.Join(",", chkmsg));
                return;
            }

            this.Confirm(selectdt);
            this.Query();

            this.reloadParant();
        }

        /// <inheritdoc/>
        public bool Confirm(DataTable selectdt)
        {
            DualResult upResult;
            string chkstatus = $@"
select s.*
from #tmp s
inner join ShipExpense_CanVass t on s.ukey = t.ukey
where t.status = 'New'
";
            DataTable dt;
            if (!(upResult = MyUtility.Tool.ProcessWithDatatable(selectdt, string.Empty, chkstatus, out dt)))
            {
                this.ShowErr(upResult);
                return false;
            }

            // 若有單已經被其他使用者先approve則跳過, 加上status = 'New' 為更新條件
            string mergeSql = $@"
Update ShipExpense_CanVass set Status = 'Confirmed' ,editname = '{Env.User.UserID}', editdate = GETDATE() where ukey in (select ukey from #tmp)

merge ShipExpense t
using #tmp s
on t.ID = s.ID
when matched then update set
    t.localsuppid = s.NewSupp,
    t.currencyid = s.NewCurrency,
    t.price = s.NewPrice,
    t.CanvassDate = s.CanvassDate1, 
    t.editname = '{Env.User.UserID}',
    t.editdate = GETDATE()
;
";

            using (TransactionScope scope = new TransactionScope())
            {
                if (!(upResult = MyUtility.Tool.ProcessWithDatatable(dt, "ID,Ukey,NewSupp,NewCurrency,NewPrice,CanvassDate1", mergeSql, out dt)))
                {
                    this.ShowErr(upResult);
                    return false;
                }

                scope.Complete();
            }

            MyUtility.Msg.InfoBox("Success!");

            return true;
        }
    }
}
