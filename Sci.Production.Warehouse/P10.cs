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
    public partial class P10 : Sci.Win.Tems.Input8
    {
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and MDivisionID = '{0}'", Sci.Env.User.Keyword); //Issue此為PMS自行建立的資料，MDivisionID皆會有寫入值

            WorkAlias = "Issue";                        // PK: ID
            GridAlias = "Issue_summary";           // PK: ID+UKey
            SubGridAlias = "Issue_detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            KeyField1 = "ID"; //Issue PK
            KeyField2 = "ID"; // Summary FK

            //SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            SubDetailKeyField1 = "id,Ukey";    // second PK
            SubDetailKeyField2 = "id,Issue_SummaryUkey"; // third FK
            //SubDetailKeyField1 = "Ukey";    // second PK
            //SubDetailKeyField2 = "Issue_SummaryUkey"; // third FK

            DoSubForm = new P10_Detail();
        }

        public P10(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            WorkAlias = "Issue";                        // PK: ID
            GridAlias = "Issue_summary";           // PK: ID+UKey
            SubGridAlias = "Issue_detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            KeyField1 = "ID"; //master PK
            KeyField2 = "ID"; // second FK

            SubDetailKeyField1 = "ID,Ukey";    // second PK
            SubDetailKeyField2 = "ID,Issue_SummaryUkey"; // third FK

            //SubDetailKeyField1 = "Ukey";    // second PK
            //SubDetailKeyField2 = "Issue_SummaryUkey"; // third FK

            SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。
            DoSubForm = new P10_Detail();
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            Color backDefaultColor = detailgrid.DefaultCellStyle.BackColor;
            #region qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellMouseDoubleClick += (s, e) =>
            {
                DoSubForm.IsSupportUpdate = false;
                OpenSubDetailPage();
            };
            #endregion

            // 使用虛擬欄位顯示 "bal_qty"及"var_qty"
            this.detailgrid.VirtualMode = true;
            this.detailgrid.CellValueNeeded += (s, e) =>
            {
                string STRrequestqty = this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString();
                string STRaccu_issue = this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString();
                string STRqty = this.detailgrid.Rows[e.RowIndex].Cells["qty"].Value.ToString();
                decimal DECrequestqty;
                decimal DECaccu_issue;
                decimal DECqty;
                if (!decimal.TryParse(STRrequestqty, out DECrequestqty))
                { DECrequestqty = 0; }
                if (!decimal.TryParse(STRaccu_issue, out DECaccu_issue))
                { DECaccu_issue = 0; }
                if (!decimal.TryParse(STRqty, out DECqty))
                { DECqty = 0; }
                if (e.ColumnIndex == this.detailgrid.Columns["bal_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                    // e.Value = Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString()) - Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString());
                    e.Value = DECrequestqty - DECaccu_issue;
                if (e.ColumnIndex == this.detailgrid.Columns["var_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                    e.Value = DECrequestqty - DECaccu_issue - DECqty;
                // e.Value = (Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString()) - Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString())) - Decimal.Parse(this.detailgrid.Rows[e.RowIndex].Cells["qty"].Value.ToString());
            };


            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(23), iseditingreadonly: true)  //1
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(17), iseditingreadonly: true)  //1
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .EditText("Description", header: "Description", width: Widths.AnsiChars(40), iseditingreadonly: true) //4            
            .Numeric("requestqty", name: "requestqty", header: "Request", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //5
            .Numeric("accu_issue", name: "accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //6
            .Numeric("", name: "bal_qty", header: "Bal. Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //7
            .Numeric("qty", name: "qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns, iseditingreadonly: true)    //8
            .Text("FinalFIR", header: "Final FIR", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Numeric("", name: "var_qty", header: "Var Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //9
            .Numeric("arqty", name: "arqty", header: "Accu Req. Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //9
            .Numeric("aiqqty", name: "aiqqty", header: "Accu Issue Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //9
            .Numeric("avqty", name: "avqty", header: "Accu Var by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //9
            .Text("unit", header: "unit", width: Widths.AnsiChars(4), iseditingreadonly: true)  //add
            .Numeric("netqty", name: "netqty", header: "Net Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //10
            ;     //
            #endregion 欄位設定

            #region Grid 變色規則
            detailgrid.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);

                #region 變色規則，若該 Row 已經變色則跳過
                decimal qty, accu_issue, netqty;
                decimal.TryParse(dr["qty"].ToString(), out qty);
                decimal.TryParse(dr["accu_issue"].ToString(), out accu_issue);
                decimal.TryParse(dr["netqty"].ToString(), out netqty);
                if (qty + accu_issue > netqty)
                {
                    if (detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(255, 128, 192))
                        detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 128, 192);
                }
                else
                {
                    if (detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != backDefaultColor)
                        detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = backDefaultColor;
                }
                #endregion 
            };
            #endregion 
        }

        protected override void OpenSubDetailPage()
        {
            base.OpenSubDetailPage();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? "" : e.Master["cutplanID"].ToString();
            this.DetailSelectCommand = string.Format(@"
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
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "A";
            CurrentMaintain["issuedate"] = DateTime.Now;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(CurrentMaintain["cutplanId"]))
            {
                MyUtility.Msg.WarningBox("< Request# >  can't be empty!", "Warning");
                txtRequest.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateIssueDate.Focus();
                return false;
            }

            #endregion 必輸檢查

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            Double sum = 0.00;
            foreach (DataRow dr in DetailDatas)
                sum += Convert.ToDouble(dr["qty"]);
            if (sum == 0)
            {
                MyUtility.Msg.WarningBox("All Issue_Qty are zero", "Warning");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IC", "Issue", (DateTime)CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;

                //assign 給detail table ID
                DataTable tmp = (DataTable)detailgridbs.DataSource;

                foreach (DataRow row in tmp.Rows)
                {
                    row.SetField("ID", tmpId);
                }

            }

            // 取BarcodeNo
            IList<DataRow> listSubDetail = new List<DataRow>();
            DataTable dtTmp;
            foreach (DataRow dr in this.DetailDatas)
            {
                this.GetSubDetailDatas(dr, out dtTmp);

                foreach (DataRow subDr in dtTmp.Rows)
                {
                    listSubDetail.Add(subDr);
                }
            }

            DualResult resultBarcodeNo = Prgs.FillIssueDetailBarcodeNo(listSubDetail);

            if (!resultBarcodeNo)
            {
                return resultBarcodeNo;
            }

            //將Issue_Detail的數量更新Issue_Summary
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



        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            sum_subDetail(e.Detail, e.SubDetails);

            //舊寫法
            //DataTable dt;
            //foreach (DataRow dr in DetailDatas)
            //{
            //    if (GetSubDetailDatas(dr, out dt))
            //    {
            //        sum_subDetail(dr, dt);
            //    }
            //}
            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        static void sum_subDetail(DataRow target, DataTable source)
        {
            target["aiqqty"] = (Decimal)target["aiqqty"] - (Decimal)target["qty"];
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
            target["aiqqty"] = (Decimal)target["aiqqty"] + (Decimal)target["qty"];
            target["avqty"] = (Decimal)target["arqty"] - (Decimal)target["aiqqty"];
        }

        private void btnAutoPick_Click(object sender, EventArgs e)
        {
            DataTable subDT;
            foreach (DataRow dr in DetailDatas)
            {
                var issued = PublicPrg.Prgs.autopick(dr);
                if (issued == null)
                {
                    break;
                }
                if (GetSubDetailDatas(dr, out subDT))
                {
                    foreach (DataRow temp in subDT.ToList()) temp.Delete();

                    foreach (DataRow dr2 in issued)
                    {
                        dr2.AcceptChanges();
                        dr2.SetAdded();
                        subDT.ImportRow(dr2);
                    }
                    sum_subDetail(dr, subDT);
                }
            }
            //強制觸發CellValueNeeded，否則游標在Issue QTY上可能會無法觸發。
            detailgrid.SelectRowToNext();
            detailgrid.SelectRowToPrev();
        }

        private void txtRequest_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt;
            string sqlcmd;
            if (!MyUtility.Check.Empty(txtRequest.Text) && txtRequest.Text != txtRequest.OldValue)
            {
                //foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
                //{
                //    dr.Delete();
                //}
                dt = (DataTable)this.detailgridbs.DataSource;
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    dt.Rows[i].Delete();
                }
                CurrentMaintain["cutplanid"] = txtRequest.Text;
                if (!MyUtility.Check.Seek(string.Format("select id from dbo.cutplan WITH (NOLOCK) where id='{0}' and mdivisionid = '{1}'", txtRequest.Text, Sci.Env.User.Keyword), null))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Request not existe");
                    return;
                }
                else
                {
                    sqlcmd = string.Format(@" 
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
) as tmpQty", txtRequest.Text, CurrentMaintain["id"]);
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    if (MyUtility.Check.Empty(dt) || MyUtility.Check.Empty(dt.Rows.Count))
                    {
                        MyUtility.Msg.WarningBox("Cutplan Cons Data not found!!");
                        return;
                    }
                    List<DataRow> rows = dt.AsEnumerable().ToList();
                    DataTable gridTable = ((DataTable)detailgridbs.DataSource);
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

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataTable dt;
            if (!(CurrentMaintain == null))
            {
                displayCutCell.Text = MyUtility.GetValue.Lookup(string.Format("select CutCellID from dbo.cutplan  WITH (NOLOCK) where id='{0}'", CurrentMaintain["cutplanid"]));
                displayLineNo.Text = MyUtility.GetValue.Lookup(string.Format(@"select t.SewLine+','  from (select distinct o.SewLine 
from dbo.Issue_Summary a WITH (NOLOCK) inner join dbo.orders o WITH (NOLOCK) on a.Poid = o.POID where a.id='{0}' and o.sewline !='') t for xml path('')", CurrentMaintain["id"]));

                DBProxy.Current.Select(null, string.Format(@";with cte as
(Select WorkOrder.FabricCombo,Cutplan_Detail.CutNo from Cutplan_Detail WITH (NOLOCK) inner join dbo.workorder WITH (NOLOCK) on WorkOrder.Ukey = Cutplan_Detail.WorkorderUkey 
where Cutplan_Detail.ID='{0}' )
select distinct FabricCombo ,(select convert(varchar,CutNo)+',' 
from (select CutNo from cte where cte.FabricCombo = a.FabricCombo )t order by CutNo for xml path('')) cutnos from cte a
", CurrentMaintain["cutplanid"]), out dt);
                editCutNo.Text = String.Join(" / ", dt.AsEnumerable().Select(row => row["FabricCombo"].ToString() + "-" + row["cutnos"].ToString())
        );

            }

            #region Status Label

            labelNotApprove.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label

        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            string sqlupd2_FIO = "";
            StringBuilder sqlupd2_B = new StringBuilder();

            #region Check Issue_Detail isn't Empty
            string checkSQL = string.Format(@"
select  isnull(sum(Qty), 0)
from issue_detail WITH (NOLOCK) 
where id = '{0}'", CurrentMaintain["ID"]);

            string checkQty = MyUtility.GetValue.Lookup(checkSQL);
            if (Convert.ToDecimal(checkQty) == 0)
            {
                MyUtility.Msg.WarningBox("All Issue_Qty are zero", "Warning");
                return;
            }
            #endregion

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));

            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(datacheck, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    // AutoWHFabric WebAPI for Gensong
                    SentToGensong_AutoWHFabric();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No) return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            string sqlupd2_FIO = "";
            StringBuilder sqlupd2_B = new StringBuilder();

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }

            var bsfio = (from m in datacheck.AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -(m.Field<decimal>("qty")),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();

            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = -(m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();


            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    // AutoWHFabric WebAPI for Gensong
                    SentToGensong_AutoWHFabric();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
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
,[Barcode] = fty.Barcode
,[Qty] = i2.Qty
,[Ukey] = i2.ukey
,[Junk] = case when i.Status = 'Confirmed' then convert(bit, 0) else convert(bit, 1) end
,CmdTime = GetDate()
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.Id=i.Id
left join Production.dbo.Cutplan c on c.ID = i.CutplanID
outer apply(
	select Barcode
	from Production.dbo.FtyInventory
	where POID = i2.POID and Seq1=i2.Seq1
	and Seq2=i2.Seq2 and Roll=i2.Roll and Dyelot=i2.Dyelot
)fty
where i.Type = 'A'
and exists(
		select 1 from Production.dbo.PO_Supp_Detail 
		where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
		and FabricType='F'
	)
and i.id = '{CurrentMaintain["ID"]}'

";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    ShowErr(drResult);
                }
                Task.Run(() => new Gensong_AutoWHFabric().SentIssue_DetailToGensongAutoWHFabric(dtDetail))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private void btnCutRef_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P10_CutRef(CurrentMaintain);
            frm.P10 = this;
            frm.ShowDialog(this);
        }

        protected override bool ClickPrint()
        {
            Sci.Production.Warehouse.P10_Print callForm;
            callForm = new P10_Print(CurrentMaintain, this.editCutNo.Text, this.CurrentMaintain);
            callForm.ShowDialog(this);

            return true;
        }

        private void btnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P13_FabricSticker(this.CurrentMaintain["ID"]).ShowDialog();
        }

        private void btn_printBarcode_Click(object sender, EventArgs e)
        {
            P10_PrintBarcode p10_PrintBarcode = new P10_PrintBarcode(this.CurrentMaintain["ID"].ToString());
            p10_PrintBarcode.ShowDialog();
        }
    }
}
