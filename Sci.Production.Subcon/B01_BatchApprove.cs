using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class B01_BatchApprove : Sci.Win.Forms.Base
    {
        Action aa;

        public B01_BatchApprove(Action aa)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.aa = aa;
        }

        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk2 = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        DataTable master;
        DataTable detail;

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk)
                .Text("Category", header: "Category", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("IsApproved", header: "Is Approved", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("AccountIDN", header: "Account No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("sLocalSuppID", header: "Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                .Text("sNewSupp", header: "New Supp", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("NewCurrency", header: "New Currency", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NewPrice", header: "New Price", width: Widths.AnsiChars(6), decimal_places: 4, iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk2)
                .Text("LocalSuppID", header: "Supp", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SuppAbb", header: "Supp Abb", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(8), decimal_places: 4, iseditingreadonly: true)
                .Date("QuotDate", header: "QuotDate", width: Widths.AnsiChars(10), iseditingreadonly: true)
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

            this.query();
            this.listControlBindingSource1.Filter = "IsApproved='N'";
        }

        public string sqlcmd(string Refno = "", string ukey = "")
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
select l.*
	,iif(isnull(ls.Abb,'') = '',l.LocalSuppID ,l.LocalSuppID + '-' +ls.Abb) as sLocalSuppID
	,iif(isnull(lsn.Abb,'') = '',l.NewSupp ,l.NewSupp + '-' +lsn.Abb) as sNewSupp
from
(
	select l.Category
			,l.Refno
			,l.Description
            ,[IsApproved]=IIF(lq.Status='Approved','Y','N')
			,l.UnitID
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
		,AccountIDN=concat(AccountID,' ',(select Name from  dbo.SciFMS_AccountNo with (nolock) where junk = 0 and id = AccountID))
	
	from LocalItem l
	inner join LocalItem_Quot lq on l.RefNo = lq.RefNo
	where 1=1 --lq.status <> 'Approved'
	and l.Junk = 0
	{wheresql}
)l
left join LocalSupp ls on l.LocalSuppID = ls.ID
left join LocalSupp lsn on l.NewSupp = lsn.ID
";
        }

        private void query()
        {
            DataSet datas = null;
            this.master = null;
            this.detail = null;
            #region
            string sqlCmd = this.sqlcmd() +
                $@"
select	lq.RefNo
		, lq.IssueDate
		, lq.Ukey
		, lq.ChooseSupp
		, lq.Status
into #bas
from LocalItem_Quot lq
inner join LocalItem l on lq.RefNo=l.RefNo
where l.Junk = 0
--where [Status] != 'Approved'

select	[Selected] = iif (tmp.ChooseSupp = tmp.seq, 1, 0)
        , RefNo
		, IssueDate
		, Ukey
		, ChooseSupp
        , LocalSuppID
        , CurrencyID
        ,Price
        , QuotDate
        , SuppAbb
from (
	select *, [QuotDate]=one.QuotDate1
	from #bas bas
	outer apply (
		select	seq = 1
				, LocalSuppID = liq.LocalSuppID1
				, CurrencyID = liq.CurrencyID1
				, Price = liq.Price1
                , QuotDate1=liq.QuotDate1
		from LocalItem_Quot liq
		where bas.Ukey = liq.Ukey
	) one

	union all 
	select *, [QuotDate]=two.QuotDate1
	from #bas bas
	outer apply (
		select	seq = 2
				, LocalSuppID = liq.LocalSuppID2
				, CurrencyID = liq.CurrencyID2
				, Price = liq.Price2
                , QuotDate1=liq.QuotDate2
		from LocalItem_Quot liq
		where bas.Ukey = liq.Ukey
	) two

	union all 
	select *, [QuotDate]=three.QuotDate1
	from #bas bas
	outer apply (
		select	seq = 3
				, LocalSuppID = liq.LocalSuppID3
				, CurrencyID = liq.CurrencyID3
				, Price = liq.Price3
                , QuotDate1=liq.QuotDate3
		from LocalItem_Quot liq
		where bas.Ukey = liq.Ukey
	) three

	union all 
	select *, [QuotDate]=four.QuotDate1
	from #bas bas
	outer apply (
		select	seq = 4
				, LocalSuppID = liq.LocalSuppID4
				, CurrencyID = liq.CurrencyID4
				, Price = liq.Price4
                , QuotDate1=liq.QuotDate4
		from LocalItem_Quot liq
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

            // var query = from t in datas.Tables[0].AsEnumerable()
            //            group t by new { t1 = MyUtility.Convert.GetString(t["Refno"]) } into m
            //            select new
            //            {
            //                refno = m.Key.t1,
            //                ct = m.Count()
            //            };
            // List<string> msg = new List<string>();
            // if (query.ToList().Count > 0)
            // {
            //    query.ToList().ForEach(q =>
            //    {
            //        if (q.ct > 1)
            //        {
            //            msg.Add(q.refno);
            //            foreach (var item in datas.Tables[0].Select($"Refno = '{q.refno}'"))
            //            {
            //                item.Delete();
            //            }

            // foreach (var item in datas.Tables[1].Select($"Refno = '{q.refno}'"))
            //            {
            //                item.Delete();
            //            }
            //        }
            //    });
            // }
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

            // DataRelation relation = new DataRelation("rel1"
            //        , new DataColumn[] { master.Columns["Refno"] }
            //        , new DataColumn[] { detail.Columns["Refno"] }
            //        );
            DataRelation relation = new DataRelation(
                "rel1",
                new DataColumn[] { this.master.Columns["Ukey"] },
                new DataColumn[] { this.detail.Columns["Ukey"] });

            datas.Relations.Add(relation);

            this.listControlBindingSource1.DataSource = datas;
            this.listControlBindingSource1.DataMember = "Master";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource2.DataMember = "rel1";
            this.grid1.AutoResizeColumns();
            this.grid1.Columns["Description"].Width = 100;

            // if (msg.Count > 0)
            //            {
            //                MyUtility.Msg.WarningBox($@"Refno have more than one new quotation, please handle those individually.
            // Refno {string.Join(",", msg)} ");
            //            }
            for (int i = 0; i < this.grid1.Rows.Count; i++)
            {
                if (this.grid1.Rows[i].Cells["IsApproved"].Value.ToString() == "Y")
                {
                    this.grid1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.query();
        }

        private void btnconfirm_Click(object sender, EventArgs e)
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
                    chkmsg.Add($"[{drow["Refno"]}]");
                }
            }

            if (chkmsg.Count > 0)
            {
                MyUtility.Msg.WarningBox(@" [NewSupp] or [NewCurrency] or [NewPrice] can not empty!
" + "Refno: " + string.Join(",", chkmsg));
                return;
            }

            this.confirm(selectdt);
            this.query();

            this.aa();
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

        private void btnToExcel_Click(object sender, EventArgs e)
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
            Microsoft.Office.Interop.Excel._Application excel = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_B01.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];   // 取得工作表
            worksheet.Cells[3, 1] = MyUtility.GetValue.Lookup("select RgCode from System", "Production");
            Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A7:A10").EntireRow; // 複製格式後插入
            Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A11", Type.Missing).EntireRow;
            for (int i = 0; i < selectdt.Rows.Count - 1; i++)
            {
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
            }

            for (int i = 0; i < selectdt.Rows.Count; i++)
            {
                int irow = 7 + (i * 4);
                worksheet.Cells[irow, 1] = selectdt.Rows[i]["Category"];
                worksheet.Cells[irow, 2] = selectdt.Rows[i]["Refno"];
                worksheet.Cells[irow, 3] = selectdt.Rows[i]["Description"];
                worksheet.Cells[irow, 4] = selectdt.Rows[i]["IsApproved"];
                worksheet.Cells[irow, 5] = selectdt.Rows[i]["UnitID"];
                worksheet.Cells[irow, 6] = selectdt.Rows[i]["AccountIDN"];

                worksheet.Cells[irow, 12] = selectdt.Rows[i]["sLocalSuppID"];
                worksheet.Cells[irow, 13] = selectdt.Rows[i]["currencyid"];
                worksheet.Cells[irow, 14] = MyUtility.Convert.GetDecimal(selectdt.Rows[i]["price"]).ToString("#,#.####");

                DataView dv = this.detail.Select($"Refno = '{selectdt.Rows[i]["Refno"]}' and Ukey = {selectdt.Rows[i]["ukey"]} and LocalSuppID <> '' and price <> 0 ").CopyToDataTable().DefaultView;
                dv.Sort = "Selected desc, Price";
                DataTable ddt = dv.ToTable();
                for (int j = 0; j < ddt.Rows.Count; j++)
                {
                    worksheet.Cells[irow + j, 8] = MyUtility.Convert.GetString(ddt.Rows[j]["LocalSuppID"]) + "-" + MyUtility.Convert.GetString(ddt.Rows[j]["SuppAbb"]);
                    worksheet.Cells[irow + j, 9] = ddt.Rows[j]["CurrencyID"];
                    worksheet.Cells[irow + j, 10] = MyUtility.Convert.GetDecimal(ddt.Rows[j]["Price"]).ToString("#,#.####");
                    worksheet.Cells[irow + j, 11] = MyUtility.Convert.GetDate(ddt.Rows[j]["QuotDate"]);
                }
            }

            worksheet.Columns.AutoFit();
            #region Save Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_B01");
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

        private void chkIncludeApproved_CheckedChanged(object sender, EventArgs e)
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
    }
}
