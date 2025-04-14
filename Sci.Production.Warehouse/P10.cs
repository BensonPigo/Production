using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

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

            #region Batch MIND Releaser 按鈕
            Point loc = this.queryfors.Location;
            this.queryfors.Dispose();
            Sci.Win.UI.Button btnMIND = new Win.UI.Button();
            btnMIND.Text = "Batch MIND Releaser";
            btnMIND.Location = loc;
            btnMIND.Width = 160;
            btnMIND.ForeColor = Color.Black;
            btnMIND.Click += this.BtnMIND_Click;
            this.browsetop.Controls.Add(btnMIND);
            #endregion
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
            .Text("WeaveTypeID", header: "Weave Type", width: Widths.AnsiChars(10), iseditingreadonly: true) // 1
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
            .EditText("Remark", header: "Remark", width: Widths.AnsiChars(20))
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

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? string.Empty : e.Master["cutplanID"].ToString();
            this.DetailSelectParameters = new List<SqlParameter>()
            {
                new SqlParameter("@issueID", masterID),
                new SqlParameter("@cutPlanID", cutplanID),
            };

            this.DetailSelectCommand = $@"
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
		                             inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID=c.Poid and psd.SEQ1 = c.Seq1 and psd.SEQ2 = c.Seq2
                                     inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
		                             where  c.id = @cutplanID
                                            and c.poid = a.poid
                                            and a.SciRefno = psd.SciRefno
                                            and a.ColorID = isnull(psdsC.SpecValue, '')), 0.00)
	        , [accu_issue] =isnull ((select  sum(qty) 
                                    from Issue c WITH (NOLOCK) 
                                    inner join Issue_Summary b WITH (NOLOCK) on c.Id=b.Id 
                                    where   a.poid = b.poid 
                                            and a.SCIRefno = b.SCIRefno 
                                            and a.Colorid = b.Colorid 
                                            and c.CutplanID = @cutplanID
                                            and c.status='Confirmed'
                                            and c.id != @issueID )
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
			,f.WeaveTypeID 
            ,a.remark
            ,Remark_old = ISNULL(a.remark, '')
	from dbo.Issue_Summary a WITH (NOLOCK) 
    left join Fabric f on a.SciRefno = f.SciRefno
    outer apply (
        select top 1 NetQty = isnull (psd.NetQty, 0)
        from  PO_Supp_Detail psd WITH (NOLOCK) 
        inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
        where psd.NetQty != 0
              and psd.NetQty is not null
              and psd.StockPoid = ''
              and a.Poid = psd.ID
              and a.SciRefno = psd.SciRefno
              and a.ColorID = isnull(psdsC.SpecValue, '')
              and psd.FabricType = 'F'
    ) Normal
    outer apply (
        select top 1 NetQty = isnull (psd.NetQty, 0)
        from  PO_Supp_Detail psd WITH (NOLOCK)
        inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
        where psd.NetQty != 0
              and psd.NetQty is not null
              and psd.StockPoid != ''
              and a.Poid = psd.ID
              and a.SciRefno = psd.SciRefno
              and a.ColorID = isnull(psdsC.SpecValue, '')
              and psd.FabricType = 'F'
    ) NonNormal
    outer apply (
        select value = iif (Normal.NetQty != 0, Normal.NetQty, NonNormal.NetQty)
    ) NetQty
	Where a.id = @issueID
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
                                    inner join Issue i WITH (NOLOCK) on s.Id=i.Id and i.CutplanID <> @cutplanID and i.status='Confirmed' 
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
) as tmpQty
";

            return base.OnDetailSelectCommandPrepare(e);
        }

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

            // ISP20240493 Confirm時還是會再去確認一次數量，Issue_Summary就算寫入Qty為0的資料列也不影響結構
            //double sum = 0.00;
            //foreach (DataRow dr in this.DetailDatas)
            //{
            //    sum += Convert.ToDouble(dr["qty"]);
            //}

            //if (sum == 0)
            //{
            //    MyUtility.Msg.WarningBox("All Issue_Qty are zero", "Warning");
            //    return false;
            //}

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

                    if (!MyUtility.Check.Empty(row["Remark"]))
                    {
                        row["RemarkEditDate"] = DateTime.Now;
                        row["RemarkEditName"] = Sci.Env.User.UserID;
                    }
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

            string dbStatus = MyUtility.GetValue.Lookup($"SELECT Status FROM Issue WHERE ID ='{this.CurrentMaintain["ID"]}'");
            if (dbStatus == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Already Confirmed");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            string sqlcmd = $@"
UPDATE S
SET
    RemarkEditDate = GETDATE()
    ,RemarkEditName = '{Sci.Env.User.UserID}'
FROM Issue_Summary S WITH(NOLOCK)
INNER JOIN #tmp t ON t.UKey = s.Ukey
WHERE t.Remark_old <> s.Remark
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, "Ukey,Remark,Remark_old", sqlcmd, out DataTable dt);
            if (!result)
            {
                return result;
            }

            return base.ClickSavePost();
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

        private void TxtRequest_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt;
            string sqlcmd;
            if (!MyUtility.Check.Empty(this.txtRequest.Text) && this.txtRequest.Text != this.txtRequest.OldValue)
            {
                dt = (DataTable)this.detailgridbs.DataSource;
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    dt.Rows[i].Delete();
                }

                this.CurrentMaintain["cutplanid"] = this.txtRequest.Text;
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@CutPlanID", this.txtRequest.Text),
                    new SqlParameter("@M", Env.User.Keyword),
                    new SqlParameter("@IssueID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])),
                };
                if (!MyUtility.Check.Seek("select id from dbo.cutplan WITH (NOLOCK) where id = @CutPlanID and mdivisionid = @M", paras, null))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Request not existe");
                    return;
                }
                else
                {
                    sqlcmd = $@" 
with main as(
    select poid
           , t.SCIRefno
           , ColorID = isnull(tC.SpecValue, '')
           , t.Refno
           , requestqty = sum(cons)
           , qty = 0.00
           , accu_issue = isnull ((select sum(qty) 
                                   from Issue a WITH (NOLOCK) 
                                   inner join Issue_Summary b WITH (NOLOCK) on a.Id=b.Id 
                                   where c.poid = b.poid 
                                         and a.CutplanID = @CutPlanID
                                         and t.SCIRefno = b.SCIRefno 
                                         and isnull(tC.SpecValue, '') = b.Colorid 
                                         and a.status = 'Confirmed'
                                         and a.id <> @IssueID ), 0.00)
           , id = @IssueID 
           , NetQty = isnull(NetQty.value, 0)
           , [description] = (select DescDetail 
                              from fabric WITH (NOLOCK) 
                              where scirefno = t.scirefno)
            ,f.WeaveTypeID  
    from dbo.Cutplan_Detail_Cons c WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail t WITH (NOLOCK) on t.id = c.Poid and t.seq1 = c.seq1 and t.seq2 = c.Seq2
    left join PO_Supp_Detail_Spec tC WITH (NOLOCK) on tC.ID = t.id and tC.seq1 = t.seq1 and tC.seq2 = t.seq2 and tC.SpecColumnID = 'Color'
    left join Fabric f WITH (NOLOCK) on t.SCIRefno = f.SCIRefno 
    outer apply (
        select top 1 NetQty = isnull (psdAll.NetQty, 0)
        from Cutplan_Detail_Cons cdc WITH (NOLOCK) 
        inner join PO_Supp_Detail psd WITH (NOLOCK) on psd.id = cdc.Poid and psd.seq1 = cdc.seq1 and psd.seq2 = cdc.Seq2
        inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
        inner join Po_Supp_Detail psdAll WITH (NOLOCK) on psd.ID = psdAll.ID and psd.SciRefno = psdAll.SciRefno
        inner join PO_Supp_Detail_Spec psdsCAll WITH (NOLOCK) on psdsCAll.ID = psd.id and psdsCAll.seq1 = psd.seq1 and psdsCAll.seq2 = psd.seq2 and psdsCAll.SpecColumnID = 'Color' and psdsC.SpecValue = psdsCAll.SpecValue
        where psdAll.NetQty != 0
              and psdAll.NetQty is not null
              and psdAll.StockPoid = ''
			  and cdc.ID = c.ID
			  and t.SCIRefno = psd.SCIRefno
			  and tC.SpecValue = psdsC.SpecValue
    ) Normal
    outer apply (
        select top 1 NetQty = isnull (psdAll.NetQty, 0)
        from Cutplan_Detail_Cons cdc WITH (NOLOCK) 
        inner join PO_Supp_Detail psd WITH (NOLOCK) on psd.id = cdc.Poid and psd.seq1 = cdc.seq1 and psd.seq2 = cdc.Seq2
        inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
        inner join Po_Supp_Detail psdAll WITH (NOLOCK) on psd.ID = psdAll.ID and psd.SciRefno = psdAll.SciRefno
        inner join PO_Supp_Detail_Spec psdsCAll WITH (NOLOCK) on psdsCAll.ID = psd.id and psdsCAll.seq1 = psd.seq1 and psdsCAll.seq2 = psd.seq2 and psdsCAll.SpecColumnID = 'Color' and psdsC.SpecValue = psdsCAll.SpecValue
        where psdAll.NetQty != 0
              and psdAll.NetQty is not null
              and psdAll.StockPoid != ''
			  and cdc.ID = c.ID
			  and t.SCIRefno = psd.SCIRefno
			  and tC.SpecValue = psdsC.SpecValue
    ) NonNormal
	outer apply (
		select value = iif (Normal.NetQty != 0, Normal.NetQty, NonNormal.NetQty)
	) NetQty
    where c.ID = @CutPlanID
    group by poid, t.SCIRefno, tC.SpecValue, t.Refno, NetQty.value, f.WeaveTypeID  
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
                                                                      and i.CutplanID != @CutPlanID
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
) as tmpQty
";
                    DBProxy.Current.Select(null, sqlcmd, paras, out dt);
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

                string sqlcmd = string.Format(
                    @";with cte as
(Select WorkOrder.FabricCombo,Cutplan_Detail.CutNo from Cutplan_Detail WITH (NOLOCK) inner join dbo.workorder WITH (NOLOCK) on WorkOrder.Ukey = Cutplan_Detail.WorkorderUkey 
where Cutplan_Detail.ID='{0}' )
select distinct FabricCombo ,(select convert(varchar,CutNo)+',' 
from (select CutNo from cte where cte.FabricCombo = a.FabricCombo )t order by CutNo for xml path('')) cutnos from cte a
", this.CurrentMaintain["cutplanid"]);
                DBProxy.Current.Select(null, sqlcmd, out dt);
                this.editCutNo.Text = string.Join(" / ", dt.AsEnumerable().Select(row => row["FabricCombo"].ToString() + "-" + row["cutnos"].ToString()));
            }

            #region Status Label

            this.labelNotApprove.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            // System.Automation=1 和confirmed 且 有P99 Use 權限的人才可以看到此按紐
            if (UtilityAutomation.IsAutomationEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED") &&
                MyUtility.Check.Seek($@"
select * from Pass1
where (FKPass0 in (select distinct FKPass0 from Pass2 where BarPrompt = 'P99. Send to WMS command Status' and Used = 'Y') or IsMIS = 1 or IsAdmin = 1)
and ID = '{Sci.Env.User.UserID}'"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            string dbStatus = MyUtility.GetValue.Lookup($"SELECT Status FROM Issue WHERE ID ='{this.CurrentMaintain["ID"]}'");
            if (dbStatus == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Already Confirmed");
                return;
            }

            base.ClickConfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            // 第3層才是 issue_detail
            DualResult result = DBProxy.Current.Select(null, $"select * from issue_detail WITH (NOLOCK) where id = '{this.CurrentMaintain["ID"]}'", out DataTable dtIssue_Detail);
            if (!result)
            {
                this.ShowErr(result);
            }
            #region Check Issue_Detail isn't Empty
            if (MyUtility.Convert.GetDecimal(dtIssue_Detail.Compute("sum(Qty)", string.Empty)) == 0)
            {
                MyUtility.Msg.WarningBox("All Issue_Qty are zero", "Warning");
                return;
            }
            #endregion

            #region 檢查 FtyInventory.SubConStatus
            List<long> listFtyInventoryUkey = dtIssue_Detail.AsEnumerable().Select(s => MyUtility.Convert.GetLong(s["FtyInventoryUkey"])).ToList();
            if (!Prgs_WMS.CheckFtyInventorySubConStatus(listFtyInventoryUkey))
            {
                return;
            }
            #endregion

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            result = Prgs.GetFtyInventoryData(dtIssue_Detail, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;
            DataTable datacheck;

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region 檢查庫存項lock
            string sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)  as balanceQty
    ,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine, tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "Issue_Detail") == false)
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    , isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine, tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in dtIssue_Detail.AsEnumerable()
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
            StringBuilder sqlupd2_B = new StringBuilder();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }

            #region 是否攤布表身 Ukey
            string sqlisNeedUnroll = $@"
    select sd.ukey
    from Issue_Detail sd with(nolock)
    inner join PO_Supp_Detail psd with(nolock) on psd.id= sd.poid and psd.SEQ1 = sd.Seq1 and psd.seq2 = sd.Seq2
    inner join [SciMES_RefnoRelaxtime] rr with(nolock) on rr.Refno = psd.Refno
    inner join [SciMES_FabricRelaxation] fr with(nolock) on fr.ID = rr.FabricRelaxationID and fr.Junk = 0
    where sd.id = '{this.CurrentMaintain["ID"]}'
    and fr.NeedUnroll = 1  -- 攤布
";
            if (!(result = DBProxy.Current.Select(null, sqlisNeedUnroll, out DataTable dtNeedUnroll)))
            {
                this.ShowErr(result);
                return;
            }
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtIssue_Detail, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(true, dtIssue_Detail, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = this.SentSpreadingSchedule()))
                    {
                        throw new Exception(result.Messages[0].Message);
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Issue set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // 更新完庫存後 RemainingQty
                    string sqlUpdateRemainingQty = $@"
Update sd set
    RemainingQty = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
from Issue_Detail sd with(nolock)
inner join Production.dbo.FtyInventory f with(nolock) on f.POID = isnull(sd.PoId, '')
    and f.Seq1 = isnull(sd.Seq1, '')
    and f.Seq2 = isnull(sd.Seq2, '')
    and f.Roll = isnull(sd.Roll, '')
	and f.Dyelot = isnull(sd.Dyelot, '')
    and f.StockType = isnull(sd.StockType, '')
    and sd.id = '{this.CurrentMaintain["ID"]}'
";
                    if (!(result = DBProxy.Current.Execute(null, sqlUpdateRemainingQty)))
                    {
                        throw result.GetException();
                    }

                    string sqUnrollActualQty = $@"
SELECT DISTINCT w.To_NewBarcode, sd.POID, sd.Seq1, sd.Seq2, sd.Roll, sd.Dyelot, sd.StockType, sd.Qty
into #tmp
FROM Issue_Detail sd WITH (NOLOCK)
LEFT JOIN WHBarcodeTransaction w WITH (NOLOCK) 
    ON w.TransactionID = sd.ID
    AND w.TransactionUkey = sd.Ukey
    AND w.Action = 'Confirm'
LEFT JOIN Fabric_UnrollandRelax fu ON fu.Barcode = w.To_NewBarcode
left join FtyInventory on FtyInventory.Ukey = sd.FtyInventoryUkey
WHERE sd.id = '{this.CurrentMaintain["ID"]}' 
    AND fu.Barcode IS NULL

INSERT INTO dbo.Fabric_UnrollandRelax (Barcode, POID, Seq1, Seq2, Roll, Dyelot, StockType,UnrollActualQty)
select a.To_NewBarcode, b.POID, b.Seq1, b.Seq2, b.Roll, b.Dyelot, b.StockType, b.Qty
from(select distinct To_NewBarcode from #tmp) a
outer apply(
    select top 1 *
    from #tmp b
    where b.To_NewBarcode = a.To_NewBarcode
)b

";
                    if (!(result = DBProxy.Current.Execute(null, sqUnrollActualQty)))
                    {
                        throw result.GetException();
                    }

                    if (dtNeedUnroll.Rows.Count > 0)
                    {
                        string dtUkey = dtNeedUnroll.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["Ukey"])).ToList().JoinToString(",");
                        string sqlUnroll = $@"
-- 先全部清空，除了已經完成的
update Issue_Detail set NeedUnroll = 0 where id = '{this.CurrentMaintain["ID"]}'
--再重新更新需要的
update Issue_Detail set NeedUnroll = 1 where Ukey in ({dtUkey})

update Issue set IncludeUnrollRelaxationRoll = 1 where id = '{this.CurrentMaintain["ID"]}'
";
                        if (!(result = DBProxy.Current.Execute(null, sqlUnroll)))
                        {
                            throw result.GetException();
                        }
                    }
                    else
                    {
                        string sqlUnroll = $@"
update Issue_Detail set NeedUnroll = 0 where id = '{this.CurrentMaintain["ID"]}'
update Issue set IncludeUnrollRelaxationRoll = 0 where id = '{this.CurrentMaintain["id"]}'
";
                        if (!(result = DBProxy.Current.Execute(null, sqlUnroll)))
                        {
                            throw result.GetException();
                        }
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, dtIssue_Detail, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        private DualResult SentSpreadingSchedule()
        {
            string sqlGetSpreadingSchedule = $@"
declare @today date = getdate()

select distinct ss.FactoryID, ss.EstCutDate, ss.CutCellID
from Issue i with (nolock)
inner join WorkOrder w with (nolock) on i.CutplanID = w.CutplanID
inner join SpreadingSchedule_Detail ssd with (nolock) on w.CutRef = ssd.CutRef
inner join SpreadingSchedule ss with (nolock) on ssd.SpreadingScheduleUkey = ss.Ukey
where	i.Id = '{this.CurrentMaintain["ID"]}' and
		ss.EstCutDate > @today
";
            DualResult result = new DualResult(true);
            DataTable dtSpreadingSchedule;
            result = DBProxy.Current.Select(null, sqlGetSpreadingSchedule, out dtSpreadingSchedule);

            if (!result)
            {
                return result;
            }

            foreach (DataRow dr in dtSpreadingSchedule.Rows)
            {
                result = new Gensong_SpreadingSchedule().SendSpreadingSchedule(dr["FactoryID"].ToString(), (DateTime)dr["EstCutDate"], dr["CutCellID"].ToString());
                if (!result)
                {
                    return result;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();

            string strSeek = $@"select * 
                                from SciProduction_Issue_Detail pms_id with(nolock)
                                inner join SpreadingInspection_InsCutRef_Fabric sif with(nolock) on sif.IssueDetailUkey = pms_id.Ukey
                                where pms_id.id = '{this.CurrentMaintain["ID"]}'";

            if (MyUtility.Check.Seek(strSeek, "ManufacturingExecution"))
            {
                MyUtility.Msg.WarningBox("QA_R14. Spreading Inspection data already exists, cannot unconfirm.");
                return;
            }

            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            // 第3層才是 issue_detail
            DualResult result = DBProxy.Current.Select(null, $"select * from issue_detail WITH (NOLOCK) where id = '{this.CurrentMaintain["ID"]}'", out DataTable dtIssue_Detail);
            if (!result)
            {
                this.ShowErr(result);
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            result = Prgs.GetFtyInventoryData(dtIssue_Detail, this.Name, out DataTable dtOriFtyInventory);
            DataTable datacheck;
            string ids = string.Empty;

            #region 檢查庫存項lock
            string sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine, tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    , isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) + d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine, tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime(dtIssue_Detail, "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region 更新庫存數量  ftyinventory
            var bsfio = (from m in dtIssue_Detail.AsEnumerable()
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

            var bs1 = (from b in dtIssue_Detail.AsEnumerable()
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

            StringBuilder sqlupd2_B = new StringBuilder();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion 更新庫存數量  ftyinventory

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock(dtIssue_Detail, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $@"update Issue set status = 'New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = this.SentSpreadingSchedule()))
                    {
                        throw new Exception(result.Messages[0].Message);
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(false, dtIssue_Detail, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                    Prgs_WMS.WMSprocess(false, dtIssue_Detail, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSUnLock(false, dtIssue_Detail, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, dtIssue_Detail, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
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
            callForm = new P10_Print(this.CurrentMaintain, this.editCutNo.Text, this.CurrentMaintain, this.editby.Text);
            callForm.ShowDialog(this);

            return true;
        }

        private void BtnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P13_FabricSticker(this.CurrentMaintain["ID"], "Issue_Detail").ShowDialog();
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), this.Name, this);
        }

        private void BtnMINDReleaser_Click(object sender, EventArgs e)
        {
            new P10_AssignReleaser(this.CurrentMaintain["ID"].ToString()).ShowDialog();
        }

        private void BtnIssueSummary_Click(object sender, EventArgs e)
        {
            P10_IssueSummary callNextFrom;
            callNextFrom = new P10_IssueSummary(this.CurrentMaintain["ID"].ToString());
            DialogResult result = callNextFrom.ShowDialog(this);
        }

        private void BtnMIND_Click(object sender, EventArgs e)
        {
            P10_Batch_MIND_Releaser win = new P10_Batch_MIND_Releaser();
            win.ShowDialog(this);
        }
    }
}
