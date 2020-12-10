using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using System.Transactions;
using Sci.Production.PublicPrg;
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P10 : Win.Tems.Input8
    {
        /// <inheritdoc/>
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and MDivisionID = '{0}'", Env.User.Keyword); // Issue此為PMS自行建立的資料，MDivisionID皆會有寫入值

            this.WorkAlias = "Issue";                        // PK: ID
            this.GridAlias = "Issue_summary";           // PK: ID+UKey
            this.SubGridAlias = "Issue_detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            this.KeyField1 = "ID"; // Issue PK
            this.KeyField2 = "ID"; // Summary FK

            // SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            this.SubDetailKeyField1 = "id,Ukey";    // second PK
            this.SubDetailKeyField2 = "id,Issue_SummaryUkey"; // third FK

            // SubDetailKeyField1 = "Ukey";    // second PK
            // SubDetailKeyField2 = "Issue_SummaryUkey"; // third FK
            this.DoSubForm = new P10_Detail();
        }

        /// <inheritdoc/>
        public P10(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.WorkAlias = "Issue";                        // PK: ID
            this.GridAlias = "Issue_summary";           // PK: ID+UKey
            this.SubGridAlias = "Issue_detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            this.KeyField1 = "ID"; // master PK
            this.KeyField2 = "ID"; // second FK

            this.SubDetailKeyField1 = "ID,Ukey";    // second PK
            this.SubDetailKeyField2 = "ID,Issue_SummaryUkey"; // third FK

            // SubDetailKeyField1 = "Ukey";    // second PK
            // SubDetailKeyField2 = "Issue_SummaryUkey"; // third FK
            this.SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。
            this.DoSubForm = new P10_Detail();
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            Color backDefaultColor = this.detailgrid.DefaultCellStyle.BackColor;
            #region qty 開窗
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellMouseDoubleClick += (s, e) =>
            {
                this.DoSubForm.IsSupportUpdate = false;
                this.OpenSubDetailPage();
            };
            #endregion

            // 使用虛擬欄位顯示 "bal_qty"及"var_qty"
            this.detailgrid.VirtualMode = true;
            this.detailgrid.CellValueNeeded += (s, e) =>
            {
                string sTRrequestqty = this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString();
                string sTRaccu_issue = this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString();
                string sTRqty = this.detailgrid.Rows[e.RowIndex].Cells["qty"].Value.ToString();
                decimal dECrequestqty;
                decimal dECaccu_issue;
                decimal dECqty;
                if (!decimal.TryParse(sTRrequestqty, out dECrequestqty))
                {
                    dECrequestqty = 0;
                }

                if (!decimal.TryParse(sTRaccu_issue, out dECaccu_issue))
                {
                    dECaccu_issue = 0;
                }

                if (!decimal.TryParse(sTRqty, out dECqty))
                {
                    dECqty = 0;
                }

                if (e.ColumnIndex == this.detailgrid.Columns["bal_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                {
                    // e.Value = Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString()) - Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString());
                    e.Value = dECrequestqty - dECaccu_issue;
                }

                if (e.ColumnIndex == this.detailgrid.Columns["var_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                {
                    e.Value = dECrequestqty - dECaccu_issue - dECqty;
                }

                // e.Value = (Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString()) - Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString())) - Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["qty"].Value.ToString());
            };

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(23), iseditingreadonly: true) // 1
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(17), iseditingreadonly: true) // 1
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .EditText("Description", header: "Description", width: Widths.AnsiChars(40), iseditingreadonly: true) // 4
            .Numeric("requestqty", name: "requestqty", header: "Request", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 5
            .Numeric("accu_issue", name: "accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 6
            .Numeric(string.Empty, name: "bal_qty", header: "Bal. Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
            .Numeric("qty", name: "qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns, iseditingreadonly: true) // 8
            .Text("FinalFIR", header: "Final FIR", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Numeric(string.Empty, name: "var_qty", header: "Var Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 9
            .Numeric("arqty", name: "arqty", header: "Accu Req. Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 9
            .Numeric("aiqqty", name: "aiqqty", header: "Accu Issue Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 9
            .Numeric("avqty", name: "avqty", header: "Accu Var by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 9
            .Text("unit", header: "unit", width: Widths.AnsiChars(4), iseditingreadonly: true) // add
            .Numeric("netqty", name: "netqty", header: "Net Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 10
            ;
            #endregion 欄位設定

            #region Grid 變色規則
            this.detailgrid.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                #region 變色規則，若該 Row 已經變色則跳過
                decimal qty, accu_issue, netqty;
                decimal.TryParse(dr["qty"].ToString(), out qty);
                decimal.TryParse(dr["accu_issue"].ToString(), out accu_issue);
                decimal.TryParse(dr["netqty"].ToString(), out netqty);
                if (qty + accu_issue > netqty)
                {
                    if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(255, 128, 192))
                    {
                        this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 128, 192);
                    }
                }
                else
                {
                    if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != backDefaultColor)
                    {
                        this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = backDefaultColor;
                    }
                }
                #endregion
            };
            #endregion
        }

        /// <inheritdoc/>
        protected override void OpenSubDetailPage()
        {
            base.OpenSubDetailPage();
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? string.Empty : e.Master["cutplanID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
;with main as
(
	select  a.Id
	        , a.Poid
            , a.SCIRefno
	        , f.Refno
	        , a.Qty
	        , a.Colorid
	        , [description] = f.DescDetail
	        , [requestqty] = isnull((select sum(cons) 
		                             from dbo.Cutplan_Detail_Cons c WITH (NOLOCK) 
		                             inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.ID=c.Poid 
                                                                                      and p.SEQ1 = c.Seq1 
                                                                                      and p.SEQ2 = c.Seq2
		                             where  c.id = '{1}' 
                                            and c.poid = a.poid
                                            and a.SciRefno = p.SciRefno
                                            and a.ColorID = p.ColorID), 0.00)
	        , [accu_issue] =isnull ((select  sum(qty) 
                                    from Issue c WITH (NOLOCK) 
                                    inner join Issue_Summary b WITH (NOLOCK) on c.Id=b.Id 
                                    where   a.poid = b.poid 
                                            and a.SCIRefno = b.SCIRefno 
                                            and a.Colorid = b.Colorid 
                                            and c.CutplanID = '{1}' 
                                            and c.status='Confirmed'
                                            and c.id != '{0}')
                                    , 0.00)
	        , a.Ukey
            , unit = (select top 1 StockUnit 
                      from Po_Supp_Detail psd 
                      where psd.Id = a.Poid
                            and psd.SciRefno = a.SciRefno)
            , NetQty = isnull(NetQty.value, 0)
            , [FinalFIR] = (SELECT Stuff((select concat( '/',isnull(Result,' '))   
                                from dbo.FIR f with (nolock) 
                                where f.poid = a.poid and f.SCIRefno = a.SCIRefno and
                                      exists(select 1 from Issue_Detail with (nolock) where Issue_SummaryUkey = a.Ukey and f.seq1 = seq1 and f.seq2 = seq2)
                                FOR XML PATH('')),1,1,'') )
	from dbo.Issue_Summary a WITH (NOLOCK) 
    left join Fabric f on a.SciRefno = f.SciRefno
    outer apply (
        select top 1 NetQty = isnull (psd.NetQty, 0)
        from  PO_Supp_Detail psd WITH (NOLOCK) 
        where psd.NetQty != 0
              and psd.NetQty is not null
              and psd.StockPoid = ''
              and a.Poid = psd.ID
              and a.SciRefno = psd.SciRefno
              and a.ColorID = psd.ColorID
              and psd.FabricType = 'F'
    ) Normal
    outer apply (
        select top 1 NetQty = isnull (psd.NetQty, 0)
        from  PO_Supp_Detail psd WITH (NOLOCK) 
        where psd.NetQty != 0
              and psd.NetQty is not null
              and psd.StockPoid != ''
              and a.Poid = psd.ID
              and a.SciRefno = psd.SciRefno
              and a.ColorID = psd.ColorID
              and psd.FabricType = 'F'
    ) NonNormal
    outer apply (
        select value = iif (Normal.NetQty != 0, Normal.NetQty, NonNormal.NetQty)
    ) NetQty
	Where a.id = '{0}'
)
select  a.*
        , tmpQty.arqty 
        , tmpQty.aiqqty
        , tmpQty.arqty -tmpQty.aiqqty as [avqty] 
from main a
outer apply(
    select arqty = a.requestqty 
                    + isnull ((select sum(c.Cons) 
                               from  dbo.Cutplan_Detail_Cons c  WITH (NOLOCK) 
                               inner join (
                                    Select   distinct s.Poid
                                             ,s.seq1
                                             ,s.seq2
                                             ,i.CutplanID 
                                    from Issue_Summary s WITH (NOLOCK) 
                                    inner join Issue i WITH (NOLOCK) on s.Id=i.Id and i.CutplanID!='{1}' and i.status='Confirmed' 
                                    where a.Poid=s.Poid and a.SCIRefno =s.SCIRefno and a.ColorID=s.ColorID
                               ) s on c.Poid=s.poid and c.SEQ1=s.SEQ1 and c.SEQ2=s.SEQ2 and c.ID=s.CutplanID)
                            , 0.00)

            , aiqqty = isnull ((select a.qty 
                                from dbo.Issue c WITH (NOLOCK) 
                                where c.id=a.id and c.status!='Confirmed' ) 
                               , 0.00)
                       +isnull ((select sum(s.Qty) 
                                 from Issue_Summary s WITH (NOLOCK) 
                                 inner join Issue i WITH (NOLOCK) on s.Id=i.Id 
                                 where  s.Poid = a.poid 
                                        and s.SCIRefno = a.SCIRefno 
                                        and s.Colorid = a.ColorID 
                                        and i.status = 'Confirmed')
                                , 0.00)
) as tmpQty", masterID, cutplanID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "A";
            this.CurrentMaintain["issuedate"] = DateTime.Now;
        }

        // delete前檢查

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // edit前檢查

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            // !EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["cutplanId"]))
            {
                MyUtility.Msg.WarningBox("< Request# >  can't be empty!", "Warning");
                this.txtRequest.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            #endregion 必輸檢查

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            double sum = 0.00;
            foreach (DataRow dr in this.DetailDatas)
            {
                sum += Convert.ToDouble(dr["qty"]);
            }

            if (sum == 0)
            {
                MyUtility.Msg.WarningBox("All Issue_Qty are zero", "Warning");
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "IC", "Issue", (DateTime)this.CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;

                // assign 給detail table ID
                DataTable tmp = (DataTable)this.detailgridbs.DataSource;

                foreach (DataRow row in tmp.Rows)
                {
                    row.SetField("ID", tmpId);
                }
            }

            // 將Issue_Detail的數量更新Issue_Summary
            DataTable subDetail;
            foreach (DataRow detailRow in this.DetailDatas)
            {
                this.GetSubDetailDatas(detailRow, out subDetail);
                if (subDetail.Rows.Count == 0)
                {
                    detailRow["Qty"] = 0;
                }
                else
                {
                    decimal detailQty = subDetail.AsEnumerable().Sum(s => s.RowState != DataRowState.Deleted ? (decimal)s["Qty"] : 0);
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            Sum_subDetail(e.Detail, e.SubDetails);

            // 舊寫法
            // DataTable dt;
            // foreach (DataRow dr in DetailDatas)
            // {
            //    if (GetSubDetailDatas(dr, out dt))
            //    {
            //        sum_subDetail(dr, dt);
            //    }
            // }
            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        private static void Sum_subDetail(DataRow target, DataTable source)
        {
            target["aiqqty"] = (decimal)target["aiqqty"] - (decimal)target["qty"];
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
            target["aiqqty"] = (decimal)target["aiqqty"] + (decimal)target["qty"];
            target["avqty"] = (decimal)target["arqty"] - (decimal)target["aiqqty"];
        }

        private void BtnAutoPick_Click(object sender, EventArgs e)
        {
            DataTable subDT;
            foreach (DataRow dr in this.DetailDatas)
            {
                var issued = Prgs.Autopick(dr);
                if (issued == null)
                {
                    break;
                }

                if (this.GetSubDetailDatas(dr, out subDT))
                {
                    foreach (DataRow temp in subDT.ToList())
                    {
                        temp.Delete();
                    }

                    foreach (DataRow dr2 in issued)
                    {
                        dr2.AcceptChanges();
                        dr2.SetAdded();
                        subDT.ImportRow(dr2);
                    }

                    Sum_subDetail(dr, subDT);
                }
            }

            // 強制觸發CellValueNeeded，否則游標在Issue QTY上可能會無法觸發。
            this.detailgrid.SelectRowToNext();
            this.detailgrid.SelectRowToPrev();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void TxtRequest_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt;
            string sqlcmd;
            if (!MyUtility.Check.Empty(this.txtRequest.Text) && this.txtRequest.Text != this.txtRequest.OldValue)
            {
                // foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
                // {
                //    dr.Delete();
                // }
                dt = (DataTable)this.detailgridbs.DataSource;
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    dt.Rows[i].Delete();
                }

                this.CurrentMaintain["cutplanid"] = this.txtRequest.Text;
                if (!MyUtility.Check.Seek(string.Format("select id from dbo.cutplan WITH (NOLOCK) where id='{0}' and mdivisionid = '{1}'", this.txtRequest.Text, Env.User.Keyword), null))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Request not existe");
                    return;
                }
                else
                {
                    sqlcmd = string.Format(
                        @" 
with main as(
    select poid
           , t.SCIRefno
           , t.ColorID
           , t.Refno
           , requestqty = sum(cons)
           , qty = 0.00
           , accu_issue = isnull ((select sum(qty) 
                                   from Issue a WITH (NOLOCK) 
                                   inner join Issue_Summary b WITH (NOLOCK) on a.Id=b.Id 
                                   where c.poid = b.poid 
                                         and a.CutplanID = '{0}' 
                                         and t.SCIRefno = b.SCIRefno 
                                         and t.Colorid = b.Colorid 
                                         and a.status = 'Confirmed'
                                         and a.id != '{1}'), 0.00)
           , id = '{1}'
           , NetQty = isnull(NetQty.value, 0)
           , [description] = (select DescDetail 
                              from fabric WITH (NOLOCK) 
                              where scirefno = t.scirefno)
    from dbo.Cutplan_Detail_Cons c WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail t WITH (NOLOCK) on t.id = c.Poid 
                                                     and t.seq1 = c.seq1 
                                                     and t.seq2 = c.Seq2
    outer apply (
        select top 1 NetQty = isnull (psdAll.NetQty, 0)
        from Cutplan_Detail_Cons cdc WITH (NOLOCK) 
        inner join PO_Supp_Detail psd WITH (NOLOCK) on psd.id = cdc.Poid 
                                                       and psd.seq1 = cdc.seq1 
                                                       and psd.seq2 = cdc.Seq2
        inner join Po_Supp_Detail psdAll WITH (NOLOCK) on psd.ID = psdAll.ID
                                                         and psd.SciRefno = psdAll.SciRefno
                                                         and psd.ColorID = psdAll.ColorID
        where psdAll.NetQty != 0
              and psdAll.NetQty is not null
              and psdAll.StockPoid = ''
			  and cdc.ID = c.ID
			  and t.SCIRefno = psd.SCIRefno
			  and t.ColorID = psd.ColorID
    ) Normal
    outer apply (
        select top 1 NetQty = isnull (psdAll.NetQty, 0)
        from Cutplan_Detail_Cons cdc WITH (NOLOCK) 
        inner join PO_Supp_Detail psd WITH (NOLOCK) on psd.id = cdc.Poid 
                                                       and psd.seq1 = cdc.seq1 
                                                       and psd.seq2 = cdc.Seq2
        inner join Po_Supp_Detail psdAll WITH (NOLOCK) on psd.ID = psdAll.ID
                                                         and psd.SciRefno = psdAll.SciRefno
                                                         and psd.ColorID = psdAll.ColorID
        where psdAll.NetQty != 0
              and psdAll.NetQty is not null
              and psdAll.StockPoid != ''
			  and cdc.ID = c.ID
			  and t.SCIRefno = psd.SCIRefno
			  and t.ColorID = psd.ColorID
    ) NonNormal
	outer apply (
		select value = iif (Normal.NetQty != 0, Normal.NetQty, NonNormal.NetQty)
	) NetQty
    where c.ID = '{0}'
    group by poid, t.SCIRefno, t.ColorID, t.Refno, NetQty.value
)
select a.*
       , unit = (select top 1 StockUnit 
                 from Po_Supp_Detail psd 
                 where psd.Id = a.Poid
                       and psd.SciRefno = a.SciRefno)
       , tmpQty.arqty 
       , tmpQty.aiqqty
       , [avqty] = tmpQty.arqty - tmpQty.aiqqty
from main a 
outer apply(
    select arqty = a.requestqty 
                   + isnull ((select sum(c.Cons) 
                              from  dbo.Cutplan_Detail_Cons c WITH (NOLOCK)  
                              inner join (
                                  Select distinct s.Poid
                                                  , s.seq1
                                                  , s.seq2
                                                  , i.CutplanID 
                                  from Issue_Summary s WITH (NOLOCK) 
                                  inner join Issue i WITH (NOLOCK) on s.Id = i.Id 
                                                                      and i.CutplanID != '{0}' 
                                                                      and i.status = 'Confirmed' 
                                  where a.Poid = s.Poid 
                                        and a.SCIRefno = s.SCIRefno 
                                        and a.ColorID = s.ColorID
                              ) s on c.Poid = s.poid 
                                     and c.SEQ1 = s.SEQ1 
                                     and c.SEQ2 = s.SEQ2 
                                     and c.ID = s.CutplanID
                            ), 0.00)
           , aiqqty = isnull ((select a.qty 
                               from dbo.Issue c WITH (NOLOCK) 
                               where c.id = a.id 
                                     and c.status != 'Confirmed' 
                              ), 0.00)
                      + isnull((select sum(s.Qty) 
                                from Issue_Summary s WITH (NOLOCK) 
                                inner join Issue i WITH (NOLOCK) on s.Id = i.Id 
                                where s.Poid = a.poid 
                                      and s.SCIRefno = a.SCIRefno 
                                      and s.Colorid = a.ColorID 
                                      and i.status = 'Confirmed'), 0.00)
) as tmpQty", this.txtRequest.Text, this.CurrentMaintain["id"]);
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    if (MyUtility.Check.Empty(dt) || MyUtility.Check.Empty(dt.Rows.Count))
                    {
                        MyUtility.Msg.WarningBox("Cutplan Cons Data not found!!");
                        return;
                    }

                    List<DataRow> rows = dt.AsEnumerable().ToList();
                    DataTable gridTable = (DataTable)this.detailgridbs.DataSource;
                    var sameFields = gridTable.GetSameFields(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        var newRow = gridTable.NewRow();
                        item.CopyTo(newRow, sameFields);
                        gridTable.Rows.Add(newRow);
                    }
                }
            }
        }

        // refresh

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataTable dt;
            if (!(this.CurrentMaintain == null))
            {
                this.displayCutCell.Text = MyUtility.GetValue.Lookup(string.Format("select CutCellID from dbo.cutplan  WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["cutplanid"]));
                this.displayLineNo.Text = MyUtility.GetValue.Lookup(string.Format(
                    @"select t.SewLine+','  from (select distinct o.SewLine 
from dbo.Issue_Summary a WITH (NOLOCK) inner join dbo.orders o WITH (NOLOCK) on a.Poid = o.POID where a.id='{0}' and o.sewline !='') t for xml path('')", this.CurrentMaintain["id"]));

                DBProxy.Current.Select(null, string.Format(
                    @";with cte as
(Select WorkOrder.FabricCombo,Cutplan_Detail.CutNo from Cutplan_Detail WITH (NOLOCK) inner join dbo.workorder WITH (NOLOCK) on WorkOrder.Ukey = Cutplan_Detail.WorkorderUkey 
where Cutplan_Detail.ID='{0}' )
select distinct FabricCombo ,(select convert(varchar,CutNo)+',' 
from (select CutNo from cte where cte.FabricCombo = a.FabricCombo )t order by CutNo for xml path('')) cutnos from cte a
", this.CurrentMaintain["cutplanid"]), out dt);
                this.editCutNo.Text = string.Join(" / ", dt.AsEnumerable().Select(row => row["FabricCombo"].ToString() + "-" + row["cutnos"].ToString()));
            }

            #region Status Label

            this.labelNotApprove.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;
            string sqlupd2_FIO = string.Empty;
            StringBuilder sqlupd2_B = new StringBuilder();

            #region Check Issue_Detail isn't Empty
            string checkSQL = string.Format(
                @"
select  isnull(sum(Qty), 0)
from issue_detail WITH (NOLOCK) 
where id = '{0}'", this.CurrentMaintain["ID"]);

            string checkQty = MyUtility.GetValue.Lookup(checkSQL);
            if (Convert.ToDecimal(checkQty) == 0)
            {
                MyUtility.Msg.WarningBox("All Issue_Qty are zero", "Warning");
                return;
            }
            #endregion

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update Issue set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }

            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));

            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(datacheck, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();

                    // 要在confirmed 後才能取得當前Balance
                    this.FtyBarcodeData(true);

                    // AutoWHFabric WebAPI for Gensong
                    this.SentToGensong_AutoWHFabric();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No)
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            string sqlupd2_FIO = string.Empty;
            StringBuilder sqlupd2_B = new StringBuilder();

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update Issue set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }

            var bsfio = (from m in datacheck.AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -m.Field<decimal>("qty"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();

            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();

                    // 刪除Ftyinventory barcode
                    this.FtyBarcodeData(false);

                    // AutoWHFabric WebAPI for Gensong
                    this.SentToGensong_AutoWHFabric();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
        }

        /// <summary>
        ///  AutoWHFabric WebAPI for Gensong
        /// </summary>
        private void SentToGensong_AutoWHFabric()
        {
            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                DataTable dtDetail = new DataTable();
                string sqlGetData = string.Empty;
                sqlGetData = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = 'A'
,[CutPlanID] = isnull(i.CutplanID,'')
,[EstCutdate] = c.EstCutdate
,[SpreadingNoID] = isnull(c.SpreadingNoID,'')
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Roll] = i2.Roll
,[Dyelot] = i2.Dyelot
,[Barcode] = Barcode.value
,[NewBarcode] = NewBarcode.value
,[Qty] = i2.Qty
,[Ukey] = i2.ukey
,[Status] = i.Status
,CmdTime = GetDate()
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.Id=i.Id
left join Production.dbo.Cutplan c on c.ID = i.CutplanID
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey
)Barcode
outer apply(
	select value = fb.Barcode
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey and fb.TransactionID = i2.Id
)NewBarcode
where i.Type = 'A'
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
and exists(
	select 1
	from FtyInventory_Detail fd 
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where f.Ukey = fd.Ukey
	and ml.IsWMS = 1
)
and i.id = '{this.CurrentMaintain["ID"]}'

";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    this.ShowErr(drResult);
                }

                Task.Run(() => new Gensong_AutoWHFabric().SentIssue_DetailToGensongAutoWHFabric(dtDetail))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private void FtyBarcodeData(bool isConfirmed)
        {
            DualResult result;
            DataTable dt = new DataTable();
            string sqlcmd = $@"
select fb.Ukey,fb.TransactionID,fb.Barcode
,[balanceQty] = f.InQty-f.OutQty+f.AdjustQty
,[NewBarcode] = ''
,i2.Id,i2.POID,i2.Seq1,i2.Seq2,i2.StockType,i2.Roll,i2.Dyelot
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.POID
    and f.Seq1 = i2.Seq1 and f.Seq2 = i2.Seq2
    and f.Roll = i2.Roll and f.Dyelot = i2.Dyelot
    and f.StockType = i2.StockType
left join FtyInventory_Barcode fb on f.Ukey = fb.Ukey
where i.Type = 'A'
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
and i2.id ='{this.CurrentMaintain["ID"]}'
";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

            foreach (DataRow dr in dt.Rows)
            {
                // InQty-Out+Adj != 0 代表非整卷, 要在Barcode後+上-01,-02....
                if (!MyUtility.Check.Empty(dr["balanceQty"]))
                {
                    if (dr["Barcode"].ToString().Contains("-"))
                    {
                        dr["NewBarcode"] = Prgs.GetNextValue(dr["Barcode"].ToString().Substring(14, 2), 1);
                    }
                    else
                    {
                        dr["NewBarcode"] = MyUtility.Check.Empty(dr["Barcode"]) ? string.Empty : dr["Barcode"].ToString() + "-01";
                    }
                }
                else
                {
                    // 如果InQty-Out+Adj = 0 代表整卷發出就使用原本Barcode
                    dr["NewBarcode"] = dr["Barcode"];
                }
            }

            var data_Fty_Barcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                    select new
                                    {
                                        TransactionID = m.Field<string>("ID"),
                                        poid = m.Field<string>("poid"),
                                        seq1 = m.Field<string>("seq1"),
                                        seq2 = m.Field<string>("seq2"),
                                        stocktype = m.Field<string>("stocktype"),
                                        roll = m.Field<string>("roll"),
                                        dyelot = m.Field<string>("dyelot"),
                                        Barcode = m.Field<string>("NewBarcode"),
                                    }).ToList();

            string upd_Fty_Barcode = Prgs.UpdateFtyInventory_IO(70, null, isConfirmed);
            DataTable resulttb;
            if (data_Fty_Barcode.Count >= 1)
            {
                if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_Barcode, string.Empty, upd_Fty_Barcode, out resulttb, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }
            }
        }

        private void BtnCutRef_Click(object sender, EventArgs e)
        {
            var frm = new P10_CutRef(this.CurrentMaintain);
            frm.P10 = this;
            frm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P10_Print callForm;
            callForm = new P10_Print(this.CurrentMaintain, this.editCutNo.Text, this.CurrentMaintain);
            callForm.ShowDialog(this);

            return true;
        }

        private void BtnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P13_FabricSticker(this.CurrentMaintain["ID"]).ShowDialog();
        }
    }
}
