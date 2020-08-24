using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Transactions;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class B03_BatchApprove : Win.Forms.Base
    {
        private Action reloadParant;

        /// <inheritdoc/>
        public B03_BatchApprove(Action reloadParant)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.reloadParant = reloadParant;
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
                .Text("IsApproved", header: "Is Approved", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("AccountIDN", header: "Account No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("sLocalSuppID", header: "Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(7), decimal_places: 5, iseditingreadonly: true)
                .Text("sNewSupp", header: "New Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("NewCurrency", header: "New Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NewPrice", header: "New Price", width: Widths.AnsiChars(7), decimal_places: 5, iseditingreadonly: true)
                ;

            // this.Helper.Controls.Grid.Generator(this.grid2)
            //    .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk2)
            //    .Text("LocalSuppID", header: "Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
            //    .Text("SuppAbb", header: "Supp Abb", width: Widths.AnsiChars(6), iseditingreadonly: true)
            //    .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
            //    .Numeric("Price", header: "Price", width: Widths.AnsiChars(7), decimal_places: 5, iseditingreadonly: true)
            //    .Date("QuotDate", header: "QuotDate", width: Widths.AnsiChars(10), iseditingreadonly: true)
            //    ;

            // for (int i = 0; i < this.grid2.Columns.Count; i++)
            // {
            //    this.grid2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // }
            this.Query();
            this.listControlBindingSource1.Filter = "IsApproved='N'";
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
select l.*
	,iif(isnull(ls.Abb,'') = '',l.LocalSuppID ,l.LocalSuppID + '-' +ls.Abb) as sLocalSuppID
	,iif(isnull(lsn.Abb,'') = '',l.NewSupp ,l.NewSupp + '-' +lsn.Abb) as sNewSupp
from(
	select l.ID
		,l.Description
        ,[IsApproved]=IIF(lq.Status='Confirmed','Y','N')
		,l.UnitID
		,l.AccountID
		,l.LocalSuppID
		,l.CurrencyID
		,l.Price
		,NewSupp = case when ChooseSupp = 1 then localsuppid1
					   when ChooseSupp = 2 then localsuppid2
					   when ChooseSupp = 3 then localsuppid3
					   when ChooseSupp = 4 then localsuppid4 end
		,NewCurrency = case 
					   when ChooseSupp = 1 then currencyid1
					   when ChooseSupp = 2 then currencyid2
					   when ChooseSupp = 3 then currencyid3
					   when ChooseSupp = 4 then currencyid4 end
		,NewPrice = case 
					   when ChooseSupp = 1 then price1
					   when ChooseSupp = 2 then price2
					   when ChooseSupp = 3 then price3
					   when ChooseSupp = 4 then price4 end
		,Selected = 0
		,lq.Ukey
		,CanvassDate1=lq.AddDate
		,AccountIDN=concat(AccountID,' ',(select Name from  dbo.SciFMS_AccountNo with (nolock) where junk = 0 and id = AccountID))
	from ShipExpense l
	inner join ShipExpense_CanVass lq on l.ID = lq.ID
	where 1=1 --lq.status <> 'Confirmed'
	and l.junk = 0
	{wheresql}
)l
left join LocalSupp ls on l.LocalSuppID = ls.ID
left join LocalSupp lsn on l.NewSupp = lsn.ID
ORDER BY l.ID 
";
        }

        private void Query()
        {
            DataSet datas = null;
            this.master = null;
            this.detail = null;

            // 將表頭跟表身一次撈完
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
where 1=1 --sc.[Status] != 'Confirmed'  
and s.junk=0

select	[Selected] = iif (tmp.ChooseSupp = tmp.seq, 1, 0)
		, ID
		, Ukey
		, ChooseSupp
		, Status
		, LocalSuppID
		, CurrencyID
		, Price
		, QuotDate
		, SuppAbb
from (
	select *,[QuotDate]=one.QuotDate1
	from #bas bas
	outer apply (
		select	seq = 1
				, LocalSuppID = liq.LocalSuppID1
				, CurrencyID = liq.CurrencyID1
				, Price = liq.Price1
				, QuotDate1=liq.QuotDate1
		from ShipExpense_CanVass liq
		where bas.Ukey = liq.Ukey
	) one

	union all 
	select *,[QuotDate]=two.QuotDate2
	from #bas bas
	outer apply (
		select	seq = 2
				, LocalSuppID = liq.LocalSuppID2
				, CurrencyID = liq.CurrencyID2
				, Price = liq.Price2
				, QuotDate2=liq.QuotDate2
		from ShipExpense_CanVass liq
		where bas.Ukey = liq.Ukey
	) two

	union all 
	select *,[QuotDate]=three.QuotDate3
	from #bas bas
	outer apply (
		select	seq = 3
				, LocalSuppID = liq.LocalSuppID3
				, CurrencyID = liq.CurrencyID3
				, Price = liq.Price3
				, QuotDate3=liq.QuotDate3
		from ShipExpense_CanVass liq
		where bas.Ukey = liq.Ukey
	) three

	union all 
	select *,[QuotDate]=four.QuotDate4
	from #bas bas
	outer apply (
		select	seq = 4
				, LocalSuppID = liq.LocalSuppID4
				, CurrencyID = liq.CurrencyID4
				, Price = liq.Price4
				, QuotDate4=liq.QuotDate4
		from ShipExpense_CanVass liq
		where bas.Ukey = liq.Ukey
	) four
) tmp
outer apply(select SuppAbb = abb from LocalSupp l where l.id=LocalSuppID)abb
WHERE tmp.ChooseSupp = tmp.seq  --只搜尋出ChooseSupp 的那一筆就好
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

            this.master = datas.Tables[0];
            this.detail = datas.Tables[1];

            this.listControlBindingSource1.DataSource = datas.Tables[0];
            this.grid1.AutoResizeColumns();

            for (int i = 0; i < this.grid1.Rows.Count; i++)
            {
                if (this.grid1.Rows[i].Cells["IsApproved"].Value.ToString() == "Y")
                {
                    this.grid1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
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
            if (this.master.Select("Selected = 1 AND IsApproved='N'").Length == 0)
            {
                MyUtility.Msg.WarningBox("Must select datas!");
                return;
            }

            DataTable selectdt = this.master.Select("Selected = 1 AND IsApproved='N'").CopyToDataTable();

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

        private void ChkIncludeApproved_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIncludeApproved.Checked)
            {
                this.listControlBindingSource1.Filter = "IsApproved='Y' OR IsApproved='N'";

                for (int i = 0; i < this.grid1.Rows.Count; i++)
                {
                    if (this.grid1.Rows[i].Cells["IsApproved"].Value.ToString() == "Y")
                    {
                        this.grid1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }
            }
            else
            {
                this.listControlBindingSource1.Filter = "IsApproved='N'";

                for (int i = 0; i < this.grid1.Rows.Count; i++)
                {
                    this.grid1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
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

            this.ShowWaitMessage("Starting Excel");
            Microsoft.Office.Interop.Excel._Application excel = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Shipping_B03.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];   // 取得工作表
            worksheet.Cells[3, 1] = MyUtility.GetValue.Lookup("select RgCode from System", "Production");
            Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A7:A7").EntireRow; // 複製格式後插入
            Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A8", Type.Missing).EntireRow;
            for (int i = 0; i < selectdt.Rows.Count - 1; i++)
            {
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
            }

            for (int i = 0; i < selectdt.Rows.Count; i++)
            {
                int irow = 7 + i;

                worksheet.Cells[irow, 1] = selectdt.Rows[i]["ID"];
                worksheet.Cells[irow, 2] = selectdt.Rows[i]["Description"];
                worksheet.Cells[irow, 3] = selectdt.Rows[i]["IsApproved"];
                worksheet.Cells[irow, 4] = selectdt.Rows[i]["UnitID"];
                worksheet.Cells[irow, 5] = selectdt.Rows[i]["AccountIDN"];

                worksheet.Cells[irow, 10] = selectdt.Rows[i]["sLocalSuppID"];
                worksheet.Cells[irow, 11] = selectdt.Rows[i]["currencyid"];
                worksheet.Cells[irow, 12] = MyUtility.Convert.GetDecimal(selectdt.Rows[i]["price"]).ToString("#,#.####");

                DataView dv = this.detail.Select($"ID = '{selectdt.Rows[i]["ID"]}' and Ukey = {selectdt.Rows[i]["ukey"]}").CopyToDataTable().DefaultView;
                dv.Sort = "Selected desc, Price";
                DataTable ddt = dv.ToTable();
                for (int j = 0; j < ddt.Rows.Count; j++)
                {
                    worksheet.Cells[irow + j, 6] = MyUtility.Convert.GetString(ddt.Rows[j]["LocalSuppID"]) + "-" + MyUtility.Convert.GetString(ddt.Rows[j]["SuppAbb"]);
                    worksheet.Cells[irow + j, 7] = ddt.Rows[j]["CurrencyID"];
                    worksheet.Cells[irow + j, 8] = MyUtility.Convert.GetDecimal(ddt.Rows[j]["Price"]).ToString("#,#.####");
                    worksheet.Cells[irow + j, 9] = MyUtility.Convert.GetDate(ddt.Rows[j]["QuotDate"]);
                }
            }

            worksheet.Columns.AutoFit();
            #region Save Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_B03");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbook);
            strExcelName.OpenFile();
            #endregion

            this.HideWaitMessage();
        }
    }
}
