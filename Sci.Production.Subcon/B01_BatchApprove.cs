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

namespace Sci.Production.Subcon
{
    public partial class B01_BatchApprove : Sci.Win.Forms.Base
    {
        public B01_BatchApprove()
        {
            InitializeComponent();
            this.EditMode = true;
        }

        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk2 = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        DataTable master, detail;

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out col_chk)
                .Text("Category", header: "Category", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("LocalSuppID", header: "Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                .Text("NewSupp", header: "New Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("NewCurrency", header: "New Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NewPrice", header: "New Price", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                ;
            
            Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out col_chk2)
                .Text("LocalSuppID", header: "Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SuppAbb", header: "Supp Abb", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                ;

            // 按Header沒有排序功能
            for (int i = 0; i < this.grid1.ColumnCount; i++)
            {
                this.grid1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // 按Header沒有排序功能
            for (int i = 0; i < this.grid2.ColumnCount; i++)
            {
                this.grid2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            query();
        }
        
        public string sqlcmd(string Refno = "",string ukey = "")
        {
            string wheresql = string.Empty;
            if (!MyUtility.Check.Empty(Refno))
            {
                wheresql = $" and l.RefNo = '{Refno}'";
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
	lq.Ukey
	
from LocalItem l
inner join LocalItem_Quot lq on l.RefNo = lq.RefNo
where lq.status <> 'Approved'
{wheresql}
";
        }

        private void query()
        {
            DataSet datas = null;
            this.master = null;
            this.detail = null;
            #region
            string sqlCmd = sqlcmd() +
                $@"
select	RefNo
		, IssueDate
		, Ukey
		, ChooseSupp
		, [Status]
into #bas
from LocalItem_Quot
where [Status] != 'Approved'

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
		from LocalItem_Quot liq
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
		from LocalItem_Quot liq
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
		from LocalItem_Quot liq
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
		from LocalItem_Quot liq
		where bas.Ukey = liq.Ukey
	) four
) tmp
outer apply(select SuppAbb = abb from LocalSupp l where l.id=LocalSuppID)abb
order by Ukey, seq

drop table #bas
";
            #endregion
            if (!SQL.Selects("", sqlCmd, out datas))
            {
                MyUtility.Msg.WarningBox(sqlCmd, "DB error!!");
                return;
            }

            if (listControlBindingSource1.DataSource != null)
            {
                listControlBindingSource1.DataSource = null;
            }
            if (listControlBindingSource2.DataSource != null)
            {
                listControlBindingSource2.DataSource = null;
            }

            var query = from t in datas.Tables[0].AsEnumerable()
                        group t by new { t1 = MyUtility.Convert.GetString(t["Refno"]) } into m
                        select new
                        {
                            refno = m.Key.t1,
                            ct = m.Count()
                        };
            List<string> msg = new List<string>();
            if (query.ToList().Count > 0)
            {
                query.ToList().ForEach(q =>
                {
                    if (q.ct > 1)
                    {
                        msg.Add(q.refno);
                        foreach (var item in datas.Tables[0].Select($"Refno = '{q.refno}'"))
                        {
                            item.Delete();
                        }

                        foreach (var item in datas.Tables[1].Select($"Refno = '{q.refno}'"))
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

            master = datas.Tables[0];
            master.TableName = "Master";

            detail = datas.Tables[1];
            detail.TableName = "Detail";

            DataRelation relation = new DataRelation("rel1"
                    , new DataColumn[] { master.Columns["Refno"] }
                    , new DataColumn[] { detail.Columns["Refno"] }
                    );

            datas.Relations.Add(relation);

            listControlBindingSource1.DataSource = datas;
            listControlBindingSource1.DataMember = "Master";
            listControlBindingSource2.DataSource = listControlBindingSource1;
            listControlBindingSource2.DataMember = "rel1";
            this.grid1.AutoResizeColumns();
            this.grid1.Columns["Description"].Width = 100;
            this.grid2.AutoResizeColumns();
            if (msg.Count > 0)
            {
                MyUtility.Msg.WarningBox($@"Refno have more than one new quotation, please handle those individually.
Refno {string.Join(",", msg)} ");
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            query();
        }

        private void btnconfirm_Click(object sender, EventArgs e)
        {
            if (this.master == null || this.master.Rows.Count == 0)
            {
                return;
            }
            this.grid1.ValidateControl();
            if (master.Select("Selected = 1").Length == 0)
            {
                MyUtility.Msg.WarningBox("Must select datas!");
                return;
            }
            DataTable selectdt = master.Select("Selected = 1").CopyToDataTable();

            List<string> chkmsg = new List<string>();
            foreach (DataRow drow in selectdt.Rows)
            {
                if (MyUtility.Check.Empty(drow["NewSupp"]) || MyUtility.Check.Empty(drow["NewCurrency"]) || MyUtility.Check.Empty(drow["NewPrice"]))
                {
                    chkmsg.Add($"[{drow["Refno"]}]");
                }
            }

            if (chkmsg.Count > 0)
            {
                MyUtility.Msg.WarningBox(@" [NewSupp] or [NewCurrency] or [NewPrice] can not empty!
" + "Refno: " + string.Join(",", chkmsg));
                return;
            }

            confirm(selectdt);
            query();
        }

        public bool confirm(DataTable selectdt)
        {
            DualResult upResult;
            string chkstatus = $@"
select s.*
from #tmp s
inner join localitem_quot t on s.ukey = t.ukey
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
Update localitem_quot set Status = 'Approved' ,editname = '{Env.User.UserID}', editdate = GETDATE() where ukey in (select ukey from #tmp)

merge localitem t
using #tmp s
on t.Refno = s.Refno
when matched then update set
    t.localsuppid = s.NewSupp,
    t.currencyid = s.NewCurrency,
    t.price = s.NewPrice,
    t.quotdate = GETDATE(),
    t.editname = '{Env.User.UserID}',
    t.editdate = GETDATE()
;
";
            using (TransactionScope scope = new TransactionScope())
            {
                if (!(upResult = MyUtility.Tool.ProcessWithDatatable(dt, "Refno,Ukey,NewSupp,NewCurrency,NewPrice", mergeSql, out dt)))
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
