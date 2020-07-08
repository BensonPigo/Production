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
using Sci.Win;
using Sci.Utility.Excel;
using System.Data.SqlClient;
using System.Reflection;

namespace Sci.Production.Warehouse
{
    public partial class P11 : Sci.Win.Tems.Input8
    {
        StringBuilder sbSizecode;
        StringBuilder sbSizecode2;
        StringBuilder strsbIssueBreakDown;
        DataTable dtSizeCode = null, dtIssueBreakDown = null;
        DataRow dr;
        string poid = string.Empty;
        bool Ismatrix_Reload = true; // 是否需要重新抓取資料庫資料

        P11_Detail subform = new P11_Detail();

        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Location = new System.Drawing.Point(this.btnBreakDown.Location.X + 20, 128); // 此gridcon位置會跑掉，需強制設定gridcon位置
            this.gridicon.Anchor = AnchorStyles.Right;

            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            this.DefaultWhere = string.Format("Type='B' and MDivisionID = '{0}'", Sci.Env.User.Keyword);

            // Issue此為PMS自行建立的資料，MDivisionID皆會有寫入值
            this.WorkAlias = "Issue";                        // PK: ID
            this.GridAlias = "Issue_detail";           // PK: ID+UKey
            this.SubGridAlias = "Issue_size";          // PK: ID+Issue_DetailUkey+SizeCode

            this.KeyField1 = "ID"; // Issue PK
            this.KeyField2 = "ID"; // Summary FK

            // SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            this.SubDetailKeyField1 = "id,Ukey";    // second PK
            this.SubDetailKeyField2 = "id,Issue_DetailUkey"; // third FK

            // SubDetailKeyField1 = "Ukey";    // second PK
            // SubDetailKeyField2 = "Issue_SummaryUkey"; // third FK
            this.DoSubForm = this.subform;
        }

        public P11(ToolStripMenuItem menuitem, string transID)
            : this(menuitem)
        {
            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region -- outqty 開窗 --
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellMouseDoubleClick += (s, e) =>
            {
                #region DoSubForm & subform 參數設定
                this.DoSubForm.IsSupportDelete = false;
                this.DoSubForm.IsSupportNew = false;

                this.subform.master = this.CurrentMaintain;
                this.subform.combo = this.checkByCombo.Checked;
                #endregion
                #region keep SubDt
                /*
                 * 如果要實現 Reject 第三層
                 * 必須先 keep 原始資料
                 */
                Dictionary<DataRow, DataTable> originSub = new Dictionary<DataRow, DataTable>();
                foreach (DataRow dr in this.DetailDatas)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        DataTable subDt;
                        this.GetSubDetailDatas(dr, out subDt);

                        // if (subDt == null)
                        // {
                        //    return;
                        // }
                        DataTable keepDt = subDt.Clone();

                        foreach (DataRow subDr in subDt.Rows)
                        {
                            keepDt.ImportRow(subDr);
                        }

                        originSub.Add(dr, keepDt);
                    }
                }
                #endregion
                base.OpenSubDetailPage();
                #region Final
                DataTable FinalSubDt;
                this.GetSubDetailDatas(out FinalSubDt);
                if (!this.subform.isSave)
                {
                    /*
                     * 第三層做 undo 則實現 Reject
                     * 將進入第三層前 keep 的資料，重新塞回第三層
                     */
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            DataTable originDt = originSub[dr];
                            DataTable unchangeSubDt;
                            this.GetSubDetailDatas(dr, out unchangeSubDt);
                            for (int i = 0; i < unchangeSubDt.Rows.Count; i++)
                            {
                                unchangeSubDt.Rows[i]["Qty"] = originDt.Rows[i]["Qty"];
                            }
                        }
                    }
                }

                /*
                 * 更新 output & qty
                 */
                foreach (DataRow dr in this.DetailDatas)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        this.GetSubDetailDatas(dr, out FinalSubDt);
                        dr["output"] = string.Join(
                            ", ",
                            FinalSubDt.AsEnumerable()
                                 .Where(row => !MyUtility.Check.Empty(row["Qty"]))
                                 .Select(row => row["SizeCode"].ToString() + "*" + Convert.ToDecimal(row["qty"]).ToString("0.00")));

                        dr["qty"] = Math.Round(
                            FinalSubDt.AsEnumerable()
                                                    .Where(row => !MyUtility.Check.Empty(row["Qty"]))
                                                    .Sum(row => Convert.ToDouble(row["Qty"].ToString())),
                            2);
                    }
                }
                #endregion
            };
            #endregion

            // DoSubForm
            #region -- Seq 右鍵開窗 --

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable bulkItems;
                    string sqlcmd = string.Format(
                        @"select  poid = b.ID
        , a.Ukey
        , b.Seq1
        , b.Seq2
        , concat(Ltrim(Rtrim(b.seq1)), ' ', b.seq2) seq
        , a.StockType
        , ColorID = isnull(dbo.GetColorMultipleID(b.BrandId, b.ColorID), '')
        , b.SizeSpec
        , b.UsedQty
        , b.SizeUnit
        , b.StockUnit
        , dbo.Getlocation(a.ukey)[location]
        , [Production].[dbo].getmtldesc(b.id, b.seq1, b.seq2, 2, 0)[description]
        , isnull((a.InQty - a.OutQty + a.AdjustQty), 0.00) as balanceqty
        , b.FabricType,b.SCIRefno,f.MtlTypeID,m.IssueType,a.inqty,a.outqty,a.adjustqty
        , [accu_issue] = isnull((select sum(Issue_Detail.qty) 
                                 from dbo.issue WITH (NOLOCK) 
                                 inner join dbo.Issue_Detail WITH (NOLOCK) on Issue_Detail.id = Issue.Id 
                                 where Issue.type = 'B' and Issue.Status = 'Confirmed' and issue.id != a.POId 
                                        and Issue_Detail.poid = a.poid and Issue_Detail.seq1 = a.seq1 and Issue_Detail.seq2 = a.seq2
                                        and Issue_Detail.roll = a.roll and Issue_Detail.stocktype = a.stocktype),0.00) 
        , balanceqty = isnull(( select fi.inqty - fi.outqty + fi.adjustqty 
                                from dbo.ftyinventory FI WITH (NOLOCK) 
                                where a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
                                        and a.roll = fi.roll and a.stocktype = fi.stocktype)
                              ,0.00)
from[Production].[dbo].po_supp_detail b WITH(NOLOCK)
inner join[Production].[dbo].Fabric f WITH(NOLOCK) on f.SCIRefno = b.SCIRefno
inner join[Production].[dbo].MtlType m WITH(NOLOCK) on m.ID = f.MtlTypeID
left join[Production].[dbo].ftyinventory a WITH(NOLOCK) on b.id = a.POID
                                                             and b.seq1 = a.seq1
                                                             and b.seq2 = a.Seq2
                                                             and stocktype = 'B'
                                                          --   and a.Roll = ''
where   b.ID = '{0}'
        and b.FabricType = 'A'
        and m.IssueType = 'Sewing'
        and b.Junk != 1
order by b.ID, b.seq1, b.seq2", this.CurrentDetailData["poid"]);
                    IList<DataRow> x;
                    DualResult result2;
                    if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out bulkItems)))
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(
                        bulkItems,
                        "FabricType,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,inqty,outqty,adjustqty",
                        "4,14,10,10,13,4,3,6,6,6,10",
                        this.CurrentDetailData["seq"].ToString(),
                        "FabricType,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,In Qty,Out Qty,Adjust Qty,Ukey");
                    selepoitem.Width = 1024;

                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = selepoitem.GetSelecteds();
                    this.CurrentDetailData["seq"] = x[0]["seq"];
                    this.CurrentDetailData["seq1"] = x[0]["seq1"];
                    this.CurrentDetailData["seq2"] = x[0]["seq2"];

                    // CurrentDetailData["mdivisionid"] = x[0]["mdivisionid"];
                    this.CurrentDetailData["stocktype"] = x[0]["stocktype"];
                    this.CurrentDetailData["ftyinventoryukey"] = x[0]["ukey"];
                    this.CurrentDetailData["Colorid"] = x[0]["Colorid"];
                    this.CurrentDetailData["SizeSpec"] = x[0]["SizeSpec"];
                    this.CurrentDetailData["UsedQty"] = x[0]["UsedQty"];
                    this.CurrentDetailData["SizeUnit"] = x[0]["SizeUnit"];
                    this.CurrentDetailData["location"] = x[0]["location"];
                    this.CurrentDetailData["description"] = x[0]["description"];
                    this.CurrentDetailData["accu_issue"] = x[0]["accu_issue"];
                    this.CurrentDetailData["balanceqty"] = x[0]["balanceqty"];
                    this.CurrentDetailData["StockUnit"] = x[0]["StockUnit"];
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["seq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["seq"] = string.Empty;
                        this.CurrentDetailData["seq1"] = string.Empty;
                        this.CurrentDetailData["seq2"] = string.Empty;

                        // CurrentDetailData["mdivisionid"] = "";
                        this.CurrentDetailData["stocktype"] = string.Empty;
                        this.CurrentDetailData["ftyinventoryukey"] = 0;
                    }
                    else
                    {
                        // check Seq Length
                        string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (seq.Length < 2)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }

                        if (!MyUtility.Check.Seek(
                            string.Format(
@"select  poid = b.ID
        , a.Ukey
        , b.Seq1
        , b.Seq2
        , concat(Ltrim(Rtrim(b.seq1)), ' ', b.seq2) seq
        , a.StockType
        , ColorID = isnull(dbo.GetColorMultipleID(b.BrandId, b.ColorID), '')
        , b.SizeSpec
        , b.UsedQty
        , b.SizeUnit
        , b.StockUnit
        , dbo.Getlocation(a.ukey)[location]
        , [Production].[dbo].getmtldesc(b.id, b.seq1, b.seq2, 2, 0)[description]
        , isnull((a.InQty - a.OutQty + a.AdjustQty), 0.00) as balanceqty
        ,b.FabricType,b.SCIRefno,f.MtlTypeID,m.IssueType,a.inqty,a.outqty,a.adjustqty
         , [accu_issue] = isnull((select sum(Issue_Detail.qty) 
                                 from dbo.issue WITH (NOLOCK) 
                                 inner join dbo.Issue_Detail WITH (NOLOCK) on Issue_Detail.id = Issue.Id 
                                 where Issue.type = 'B' and Issue.Status = 'Confirmed' and issue.id != a.POId 
                                        and Issue_Detail.poid = a.poid and Issue_Detail.seq1 = a.seq1 and Issue_Detail.seq2 = a.seq2
                                        and Issue_Detail.roll = a.roll and Issue_Detail.stocktype = a.stocktype),0.00) 
        , balanceqty = isnull(( select fi.inqty - fi.outqty + fi.adjustqty 
                                from dbo.ftyinventory FI WITH (NOLOCK) 
                                where a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
                                        and a.roll = fi.roll and a.stocktype = fi.stocktype)
                              ,0.00)
from[Production].[dbo].po_supp_detail b WITH(NOLOCK)
inner join[Production].[dbo].Fabric f WITH(NOLOCK) on f.SCIRefno = b.SCIRefno
inner join[Production].[dbo].MtlType m WITH(NOLOCK) on m.ID = f.MtlTypeID
left join[Production].[dbo].ftyinventory a WITH(NOLOCK) on b.id = a.POID
                                                             and b.seq1 = a.seq1
                                                             and b.seq2 = a.Seq2
                                                             and stocktype = 'B'
                                                          --   and a.Roll = ''
LEFT JOIN Orders o ON o.ID = b.ID
where   b.ID = '{0}'
        and b.FabricType = 'A'
        and b.seq1 = '{1}' 
        and b.seq2 = '{2}' 
        and m.IssueType = 'Sewing'
        and b.Junk != 1
		and o.Category <> 'A'
order by b.ID, b.seq1, b.seq2",
this.CurrentDetailData["poid"], seq[0], seq[1]), out this.dr, null))
                        {
                           e.Cancel = true;
                           MyUtility.Msg.WarningBox("Data not found!", "Seq");
                           return;
                        }
                        else
                        {
                            this.CurrentDetailData["seq"] = seq[0] + " " + seq[1];
                            this.CurrentDetailData["seq1"] = seq[0];
                            this.CurrentDetailData["seq2"] = seq[1];

                            // CurrentDetailData["mdivisionid"] = dr["mdivisionid"];
                            this.CurrentDetailData["stocktype"] = this.dr["stocktype"];
                            this.CurrentDetailData["ftyinventoryukey"] = this.dr["ukey"];
                            this.CurrentDetailData["Colorid"] = this.dr["Colorid"];
                            this.CurrentDetailData["SizeSpec"] = this.dr["SizeSpec"];
                            this.CurrentDetailData["UsedQty"] = this.dr["UsedQty"];
                            this.CurrentDetailData["SizeUnit"] = this.dr["SizeUnit"];
                            this.CurrentDetailData["location"] = this.dr["location"];
                            this.CurrentDetailData["description"] = this.dr["description"];
                            this.CurrentDetailData["accu_issue"] = this.dr["accu_issue"];
                            this.CurrentDetailData["balanceqty"] = this.dr["balanceqty"];
                            this.CurrentDetailData["StockUnit"] = this.dr["StockUnit"];
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗

            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts2) // 1
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 2
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true) // 3
            .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true) // 4
            .Numeric("usedqty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true) // 5
            .Text("SizeUnit", header: "SizeUnit", width: Widths.AnsiChars(6), iseditingreadonly: true) // 6
            .Text("location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true) // 7
            .Numeric("accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 8
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true) // 5
            .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true) // 7
            .Text("output", header: "Output", width: Widths.AnsiChars(20), iseditingreadonly: true, settings: ts) // 9
            .Numeric("balanceqty", header: "Balance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 11
            ;
            #endregion 欄位設定

            #region 可編輯欄位變色
            this.detailgrid.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["output"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion 可編輯欄位變色
        }

        // 寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? string.Empty : e.Master["cutplanID"].ToString();
            this.Ismatrix_Reload = true;
            this.DetailSelectCommand = string.Format(
                @"  
select  a.Id
        , isnull(a.FtyInventoryUkey,0) [FtyInventoryUkey]
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.seq2) as seq
        , a.StockType
        , a.Qty
        , Colorid = isnull(dbo.GetColorMultipleID(p.BrandId, p.ColorID), '')
        , p.SizeSpec
        , p.UsedQty
        , p.SizeUnit
        , p.StockUnit
        , dbo.Getlocation(fi.ukey) [location]
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0)[description]
        , [accu_issue] = isnull(( select sum(Issue_Detail.qty) 
                                  from dbo.issue WITH (NOLOCK) 
                                  inner join dbo.Issue_Detail WITH (NOLOCK) on Issue_Detail.id = Issue.Id 
                                  where Issue.type = 'B' and Issue.Status = 'Confirmed' and issue.id != a.Id 
                                         and Issue_Detail.poid = a.poid and Issue_Detail.seq1 = a.seq1 and Issue_Detail.seq2 = a.seq2
                                         and Issue_Detail.roll = a.roll and Issue_Detail.stocktype = a.stocktype),0.00) 
        , [output] = isnull ((select v.sizeqty+', ' 
                              from (
                                select (rtrim(Issue_Size.SizeCode) +'*'+convert(varchar,Issue_Size.Qty)) as sizeqty 
                                from dbo.Issue_Size WITH (NOLOCK) 
                                where   Issue_Size.Issue_DetailUkey = a.ukey and qty <>0
                             ) v for xml path(''))
                            ,'') 
        , a.Ukey
        , balanceqty = isnull((fi.inqty - fi.outqty + fi.adjustqty),0.00)
        , a.BarcodeNo
from dbo.Issue_Detail a WITH (NOLOCK) 
left join dbo.po_supp_detail p WITH (NOLOCK) on p.id  = a.poid 
                                                and p.seq1= a.seq1 
                                                and p.seq2 =a.seq2
left join dbo.FtyInventory FI on    a.Poid = Fi.Poid 
                                    and a.Seq1 = fi.seq1 
                                    and a.seq2 = fi.seq2 
                                    and a.roll = fi.roll 
                                    and a.stocktype = fi.stocktype
                                    and a.Dyelot = fi.Dyelot
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "B";
            this.CurrentMaintain["issuedate"] = DateTime.Now;

           // leo等確認完工廠有此欄位後開啟
            this.CurrentMaintain["combo"] = 0;
            this.dtIssueBreakDown = null;
            this.gridIssueBreakDown.DataSource = null;
            this.txtOrderID.IsSupportEditMode = true;
            this.txtRequest.IsSupportEditMode = true;
            this.txtOrderID.ReadOnly = false;
            this.txtRequest.ReadOnly = false;
        }

        // delete前檢查
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
        protected override bool ClickEditBefore()
        {
            // !EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            this.txtOrderID.IsSupportEditMode = false;
            this.txtRequest.IsSupportEditMode = false;
            this.txtOrderID.ReadOnly = true;
            this.txtRequest.ReadOnly = true;
            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            DataTable result = null;
            StringBuilder warningmsg = new StringBuilder();

            #region 表頭 必輸檢查
            if (MyUtility.Check.Empty(this.txtOrderID.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Request# > or < Order ID >  can't be empty!", "Warning");
                this.txtOrderID.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }
            #endregion 必輸檢查
            #region 表身不可出現 Qty = 0 的資料
            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr["Qty"].ToString().Empty() || dr["Qty"].EqualDecimal(0))
                {
                    dr.Delete();
                }
            }
            #endregion
            #region 表身 必輸檢查
            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr["Seq"].ToString().Empty())
                {
                    MyUtility.Msg.WarningBox("Seq can't be empty.");
                    return false;
                }
            }
            #endregion

            foreach (DataRow Checkduplicate in this.DetailDatas)
            {
                DataRow[] findrow = this.DetailDatas.AsEnumerable().Where(row => row["poid"].EqualString(Checkduplicate["poid"].ToString()) && row["seq1"].EqualString(Checkduplicate["seq1"])
                                                                          && row["seq2"].EqualString(Checkduplicate["seq2"].ToString())).ToArray();
                if (findrow.Length > 1)
                {
                    MyUtility.Msg.WarningBox(string.Format(@"SP#: {0} Seq#: {1}-{2} duplicate, SP# and Seq# can't duplicate", Checkduplicate["poid"], Checkduplicate["seq1"], Checkduplicate["seq2"]));
                    return false;
                }
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IS", "Issue", (DateTime)this.CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            if (this.dtSizeCode != null && this.dtSizeCode.Rows.Count != 0)
            {
                if (this.checkByCombo.Checked == false)
                {
                    foreach (DataRow data in this.dtIssueBreakDown.ToList())
                    {
                        if (data.ItemArray[0].ToString() != this.txtOrderID.Text)
                        {
                            this.dtIssueBreakDown.Rows.Remove(data);
                        }
                    }
                }

                string sqlcmd;
                sqlcmd = string.Format(
                    @";delete from dbo.issue_breakdown where id='{0}'
;WITH UNPIVOT_1
AS
(
SELECT * FROM #tmp
UNPIVOT
(
QTY
FOR SIZECODE IN ({1})
)
AS PVT
)
MERGE INTO DBO.ISSUE_BREAKDOWN T
USING UnPivot_1 S
ON T.ID = '{0}' AND T.ORDERID= S.OrderID AND T.ARTICLE = S.ARTICLE AND T.SIZECODE = S.SIZECODE
WHEN MATCHED THEN
UPDATE
SET QTY = S.QTY
WHEN NOT MATCHED THEN
INSERT (ID,ORDERID,ARTICLE,SIZECODE,QTY)
VALUES ('{0}',S.OrderID,S.ARTICLE,S.SIZECODE,S.QTY)
;delete from dbo.issue_breakdown where id='{0}' and qty = 0; ", this.CurrentMaintain["id"], this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1));

                string aaa = this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1).Replace("[", string.Empty).Replace("]", string.Empty);

                ProcessWithDatatable2(this.dtIssueBreakDown, "OrderID,Article," + aaa,
                    sqlcmd, out result, "#tmp");
            }

            DataTable subDT;

            // 將需新增的資料狀態更改為新增
            foreach (DataRow dr in this.DetailDatas)
            {
                this.GetSubDetailDatas(dr, out subDT);
                foreach (DataRow temp in subDT.Rows)
                {
                    if (temp["isvirtual"].ToString() == "1") // && Convert.ToDecimal(temp["QTY"].ToString()) > 0)
                    {
                        temp.AcceptChanges();
                        temp.SetAdded();
                    }
                }
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePre()
        {
            DualResult resultBarcodeNo = Prgs.FillIssueDetailBarcodeNo(this.DetailDatas);

            if (!resultBarcodeNo)
            {
                return resultBarcodeNo;
            }

            return base.ClickSavePre();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            string deleteSql = $@"delete Issue_Size where id = '{this.CurrentMaintain["id"]}' and Qty = 0";
            DualResult result = DBProxy.Current.Execute(null, deleteSql);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            DataTable dt;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (this.GetSubDetailDatas(dr, out dt))
                {
                    sum_subDetail(dr, dt);
                }
            }

            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        static void sum_subDetail(DataRow target, DataTable source)
        {
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
        }

        private void btnAutoPick_Click(object sender, EventArgs e)
        {
            // 檢查是否有勾選Combo，處理傳入AutoPick資料篩選
            if (!this.checkByCombo.Checked && this.dtIssueBreakDown != null)
            {
                foreach (DataRow tempRow in this.dtIssueBreakDown.Rows)
                {
                    if (tempRow["OrderID"].ToString() != this.txtOrderID.Text.ToString())
                    {
                        foreach (DataColumn tempColumn in this.dtIssueBreakDown.Columns)
                        {
                            if (tempRow[tempColumn].GetType().Name == "Decimal")
                            {
                                tempRow[tempColumn] = 0;
                            }
                        }
                    }
                }
            }

            var frm = new Sci.Production.Warehouse.P11_AutoPick(this.CurrentMaintain["id"].ToString(), this.poid, this.CurrentMaintain["cutplanid"].ToString(), this.txtOrderID.Text.ToString(), this.dtIssueBreakDown, this.sbSizecode, this.checkByCombo.Checked);
            DialogResult result = frm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                DataRow tmp;
                DataTable _detail, _subDetail;
                _detail = (DataTable)this.detailgridbs.DataSource;

                // 刪除表身重新匯入
                foreach (DataRow del in this.DetailDatas)
                {
                    del.Delete();
                }

                // 批次匯入
                foreach (KeyValuePair<DataRow, DataTable> item in frm.dictionaryDatas)
                {
                    tmp = item.Key;
                    if (tmp["selected"].ToString() == "1")
                    {
                        // 匯入Issue_detail layer
                        tmp["id"] = this.CurrentMaintain["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        _detail.ImportRow(tmp);

                        // 匯入Issue_Size layer
                        if (this.GetSubDetailDatas(_detail.Rows[_detail.Rows.Count - 1], out _subDetail))
                        {
                            foreach (DataRow dr2 in item.Value.Rows)
                            {
                                dr2.AcceptChanges();
                                dr2.SetAdded();
                                _subDetail.ImportRow(dr2);
                            }
                        }
                    }
                }

                _detail.DefaultView.Sort = string.Empty;
            }
        }

        private void txtRequest_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtRequest.Text == this.txtRequest.OldValue)
            {
                return;
            }

            // DBProxy.Current.Execute(null, string.Format("delete from dbo.issue_breakdown where id='{0}';", CurrentMaintain["id"].ToString()));
            this.CurrentMaintain["cutplanid"] = this.txtRequest.Text;
            this.txtOrderID.Text = string.Empty;
            this.CurrentMaintain["orderid"] = string.Empty;
            this.displayPOID.Text = string.Empty;
            this.poid = MyUtility.GetValue.Lookup(string.Format(
                @"
select Cutplan.poid 
from dbo.cutplan WITH (NOLOCK) 
LEFT JOIN Orders o ON Cutplan.POID = o.ID
where Cutplan.id='{0}' and Cutplan.Mdivisionid = '{1}' AND o.Category != 'A' 
", this.CurrentMaintain["cutplanid"], Sci.Env.User.Keyword));

            if (MyUtility.Check.Empty(this.txtRequest.Text))
            {
                this.dtIssueBreakDown = null;
                this.gridIssueBreakDown.DataSource = null;
                foreach (DataRow dr in this.DetailDatas)
                {
                    // 刪除SubDetail資料
                    ((DataTable)this.detailgridbs.DataSource).Rows.Remove(dr);
                    dr.Delete();
                }

                return;
            }

            if (MyUtility.Check.Empty(this.poid))
            {
                this.CurrentMaintain["cutplanid"] = string.Empty;
                this.txtRequest.Text = string.Empty;
                this.dtIssueBreakDown = null;
                this.gridIssueBreakDown.DataSource = null;
                foreach (DataRow dr in this.DetailDatas)
                {
                    // 刪除SubDetail資料
                    ((DataTable)this.detailgridbs.DataSource).Rows.Remove(dr);
                    dr.Delete();
                }

                MyUtility.Msg.WarningBox("Can't found data");
                return;
            }

            // getpoid();
            this.displayPOID.Text = this.poid;
            this.CurrentMaintain["orderid"] = this.poid;
            this.Detail_Reload();
            this.Ismatrix_Reload = true;
            this.matrix_Reload();
        }

        // refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.getpoid();
            DataTable dt;
            if (!(this.CurrentMaintain == null))
            {
                this.displayCutCell.Text = MyUtility.GetValue.Lookup(string.Format("select CutCellID from dbo.cutplan WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["cutplanid"]));
                this.displayLineNo.Text = MyUtility.GetValue.Lookup(string.Format(
                    @"select t.SewLine+','  from (select distinct o.SewLine 
from dbo.Issue_detail a WITH (NOLOCK) inner join dbo.orders o WITH (NOLOCK) on a.Poid = o.POID where a.id='{0}' and o.sewline !='') t for xml path('')", this.CurrentMaintain["id"]));

                DBProxy.Current.Select(null, string.Format(
                    @";with cte as
(Select WorkOrder.FabricCombo,Cutplan_Detail.CutNo from Cutplan_Detail WITH (NOLOCK) inner join dbo.workorder WITH (NOLOCK) on WorkOrder.Ukey = Cutplan_Detail.WorkorderUkey 
where Cutplan_Detail.ID='{0}' )
select distinct FabricCombo ,(select convert(varchar,CutNo)+',' 
from (select CutNo from cte where cte.FabricCombo = a.FabricCombo )t order by CutNo for xml path('')) cutnos from cte a
", this.CurrentMaintain["cutplanid"]), out dt);
                this.editCutNo.Text = string.Join(" / ", dt.AsEnumerable().Select(row => row["FabricCombo"].ToString() + "-" + row["cutnos"].ToString()));

                // ebArticle.Text = MyUtility.GetValue.Lookup(string.Format(@"select t.article+','  from (select distinct article
                // from dbo.cutplan_detail  where id='{0}') t for xml path('')", CurrentMaintain["cutplanid"]));
                #region -- Status Label --

                this.label25.Text = this.CurrentMaintain["status"].ToString();

                #endregion Status Label
                #region -- POID
                this.getpoid();
                this.displayPOID.Text = this.poid;
                #endregion

                #region -- matrix breakdown
                DualResult result;
                if (!(result = this.matrix_Reload()))
                {
                    this.ShowErr(result);
                }
                #endregion
            }
        }

        private DualResult matrix_Reload()
        {
            if (this.EditMode == true && this.Ismatrix_Reload == false)
            {
                return Result.True;
            }

            this.Ismatrix_Reload = false;
            string sqlcmd;
            StringBuilder sbIssueBreakDown;
            DualResult result;

            string OrderID = this.txtOrderID.Text;

            sqlcmd = string.Format(
                @"select sizecode from dbo.order_sizecode WITH (NOLOCK) 
where id = (select poid from dbo.orders WITH (NOLOCK) where id='{0}') order by seq", OrderID);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out this.dtSizeCode)))
            {
                this.ShowErr(sqlcmd, result);
                return Result.True;
            }

            if (this.dtSizeCode.Rows.Count == 0)
            {
                // MyUtility.Msg.WarningBox(string.Format("Becuase there no sizecode data belong this OrderID {0} , can't show data!!", CurrentDataRow["orderid"]));
                this.dtIssueBreakDown = null;
                this.gridIssueBreakDown.DataSource = null;
                return Result.True;
            }

            this.sbSizecode = new StringBuilder();
            this.sbSizecode2 = new StringBuilder();
            this.sbSizecode.Clear();
            this.sbSizecode2.Clear();
            for (int i = 0; i < this.dtSizeCode.Rows.Count; i++)
            {
                this.sbSizecode.Append(string.Format(@"[{0}],", this.dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
                this.sbSizecode2.Append(string.Format(@"{0},", this.dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
            }

            sbIssueBreakDown = new StringBuilder();
            sbIssueBreakDown.Append(string.Format(
                @";with Bdown as 
            (select a.ID [orderid],a.Article,a.SizeCode,a.Qty from dbo.order_qty a WITH (NOLOCK) 
            inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
            where b.POID=(select poid from dbo.orders WITH (NOLOCK) where id = '{0}')
            )
            ,Issue_Bdown as
            (
            	select isnull(Bdown.orderid,ib.OrderID) [OrderID],isnull(Bdown.Article,ib.Article) Article,isnull(Bdown.SizeCode,ib.sizecode) sizecode,isnull(ib.Qty,0) qty
            	from Bdown full outer join (select * from dbo.Issue_Breakdown WITH (NOLOCK) where id='{1}') ib
            	on Bdown.orderid = ib.OrderID and Bdown.Article = ib.Article and Bdown.SizeCode = ib.SizeCode
            )
            select * from Issue_Bdown
            pivot
            (
            	sum(qty)
            	for sizecode in ({2})
            )as pvt
            order by [OrderID],[Article]", OrderID, this.CurrentMaintain["id"], this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1))); // .Replace("[", "[_")
            this.strsbIssueBreakDown = sbIssueBreakDown; // 多加一個變數來接 不改變欄位
            if (!(result = DBProxy.Current.Select(null, sbIssueBreakDown.ToString(), out this.dtIssueBreakDown)))
            {
                this.ShowErr(sqlcmd, result);
                return Result.True;
            }

            this.gridIssueBreakDown.AutoGenerateColumns = true;
            this.gridIssueBreakDownBS.DataSource = this.dtIssueBreakDown;
            this.gridIssueBreakDown.DataSource = this.gridIssueBreakDownBS;
            this.gridIssueBreakDown.IsEditingReadOnly = true;
            this.gridIssueBreakDown.ReadOnly = true;

            this.checkByCombo_CheckedChanged(null, null);

            return Result.True;
        }

        private void getpoid()
        {
            this.CurrentMaintain["cutplanid"] = this.txtRequest.Text;
            this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.cutplan WITH (NOLOCK) where id='{0}' and mdivisionid = '{1}'", this.CurrentMaintain["cutplanid"], Sci.Env.User.Keyword));
            if (MyUtility.Check.Empty(this.poid))
            {
                this.poid = MyUtility.GetValue.Lookup(string.Format("select orders.poid from dbo.orders WITH (NOLOCK) left join dbo.Factory on orders.FtyGroup=Factory.ID where orders.id='{0}' and Factory.mdivisionid = '{1}'", this.CurrentMaintain["orderid"], Sci.Env.User.Keyword));
            }
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            string strSort = ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort;
            ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = string.Empty;
            base.OnDetailGridInsert(index);

            // CurrentDetailData["mdivisionid"] = Sci.Env.User.Keyword;
            this.CurrentDetailData["poid"] = this.poid;
            DataTable sizeRange, subDetails;
            if (this.GetSubDetailDatas(this.CurrentDetailData, out subDetails))
            {
                DBProxy.Current.Select(null, string.Format(
                    @"select a.SizeCode,'{1}' AS Id,0.00 AS QTY
from dbo.Order_SizeCode a WITH (NOLOCK) 
where a.id='{0}' order by Seq", this.poid, this.CurrentMaintain["id"]), out sizeRange);
                foreach (DataRow dr in sizeRange.Rows)
                {
                    dr.AcceptChanges();
                    dr.SetAdded();
                    subDetails.ImportRow(dr);
                }
            }

            ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = strSort;
        }

        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? string.Empty : e.Detail["ID"].ToString();
            string ukey = (e.Detail == null || MyUtility.Check.Empty(e.Detail["ukey"])) ? "0" : e.Detail["ukey"].ToString();
            this.SubDetailSelectCommand = string.Format(
                @"
;with aaa as(
    select  
         a.SizeCode
        , b.Id
        , Issue_DetailUkey =  '{2}'
        , QTY = isnull(b.Qty,0)
        , isvirtual = IIF(b.Qty IS NULL , 1 ,0)
        , seq
    --into #tmp
    from  dbo.Issue_Size b WITH (NOLOCK) 
    inner join dbo.Order_SizeCode a WITH (NOLOCK) on b.SizeCode = a.SizeCode
    outer apply(select poid from dbo.cutplan WITH (NOLOCK) where id='{0}' and mdivisionid = '{3}')poid1
    outer apply(select orders.poid from dbo.orders WITH (NOLOCK) left join dbo.Factory on orders.FtyGroup=Factory.ID where orders.id='{4}' and Factory.mdivisionid = '{3}')poid2
    where a.id= iif(isnull(poid1.POID,'')='',poid2.POID,poid1.poid)
    and b.id = '{1}' and b.Issue_DetailUkey = {2}
)",
                this.CurrentMaintain["cutplanid"].ToString(), masterID, ukey, Sci.Env.User.Keyword, this.CurrentMaintain["orderid"].ToString());

            // if (!MyUtility.Check.Empty(CurrentMaintain["cutplanid"]))
            // {
            this.SubDetailSelectCommand += $@"
,bbb as(
	select distinct os.sizecode,ID = '{this.CurrentMaintain["orderid"]}',Issue_DetailUkey = '{ukey}',QTY=0,isvirtual = 1,seq
	from dbo.Order_SizeCode os WITH(NOLOCK)
	inner join orders o WITH(NOLOCK) on o.POID = os.Id
	inner join dbo.Order_Qty oq WITH(NOLOCK) on o.id = oq.ID and os.SizeCode = oq.SizeCode
	where o.POID = '{this.poid}' 
	and not exists(select SizeCode from aaa where aaa.SizeCode = os.sizecode)
)
select SizeCode,Id,Issue_DetailUkey,QTY,isvirtual
from(
	select * from aaa
	union all
	select * from bbb
)ccc
order by seq
";

            // }
            // else
            // {
            //    SubDetailSelectCommand += " select SizeCode, Id, Issue_DetailUkey, QTY, isvirtual from aaa order by seq ";
            // }
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
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
            DataTable datacheck;
            string sqlupd2_FIO = string.Empty;
            StringBuilder sqlupd2_B = new StringBuilder();

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"
Select  d.poid  
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.POID = f.POID  
                                            and D.StockType = F.StockType
                                            and d.Roll = f.Roll 
                                            and d.Seq1 =f.Seq1 
                                            and d.Seq2 = f.Seq2
                                            and d.Dyelot = f.Dyelot
where   f.lock = 1 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                @"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll,d.Qty
        , isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on d.POID = f.POID  
                                          and D.StockType = F.StockType
                                          and d.Roll = f.Roll 
                                          and d.Seq1 = f.Seq1 
                                          and d.Seq2 = f.Seq2
                                          and d.Dyelot = f.Dyelot
where   (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                @"
update Issue 
set status = 'Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            #region -- 更新mdivisionpodetail B倉數 --
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
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion
            #endregion
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        datacheck, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
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

            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
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
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(
                        bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;
        }

        private void btnBOA_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P11_BOA(this.CurrentMaintain["id"].ToString(), this.poid, this.CurrentMaintain["cutplanid"].ToString(), this.txtOrderID.Text.ToString());
            frm.ShowDialog(this);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
            // 刪除表身重新匯入
            foreach (DataRow del in this.DetailDatas)
            {
                if (del["qty"].EqualDecimal(0))
                {
                    del.Delete();
                }
            }
        }

        private void btnBreakDown_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["cutplanID"]) && MyUtility.Check.Empty(this.CurrentMaintain["OrderId"]))
            {
                MyUtility.Msg.WarningBox("Please key-in Request or Order ID first!!");
                return;
            }

            var frm = new Sci.Production.Warehouse.P11_IssueBreakDown(this.CurrentMaintain, this.dtIssueBreakDown, this.dtSizeCode);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }

        protected override bool ClickPrint()
        {
            this.label25.Text = this.CurrentMaintain["status"].ToString();
            if (this.label25.Text.ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            DataRow issue = this.CurrentMaintain;
            string id = issue["ID"].ToString();
            string request = issue["cutplanid"].ToString();
            string issuedate = Convert.ToDateTime(issue["issuedate"]).ToString("yyyy/MM/dd");
            string remark = issue["remark"].ToString();
            string cutno = this.editCutNo.Text;
            string article = this.editArticle.Text;
            string LineNo = this.displayLineNo.Text;
            string OrderID = this.txtOrderID.Text;
            string CellNo = this.displayCutCell.Text;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            pars.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
            pars.Add(new SqlParameter("@OrderID", OrderID));
            #region Title
            DataTable dt;
            DBProxy.Current.Select(string.Empty, @"
select NameEN 
from MDivision 
where id = @MDivision", pars, out dt);
            string RptTitle = dt.Rows[0]["NameEN"].ToString();

            #endregion
            #region SP
            DataTable dtsp;
            string poID;
            DBProxy.Current.Select(
                string.Empty,
                @"select (select poid+',' from 
             (select distinct cd.POID from Cutplan_Detail cd WITH (NOLOCK) where id =(select CutplanID from dbo.Issue WITH (NOLOCK) where id='@id')  ) t
			  for xml path('')) as [poid]", pars, out dtsp);
            if (dtsp.Rows.Count == 0)
            {
                poID = string.Empty;
            }
            else
            {
                poID = dtsp.Rows[0]["POID"].ToString();
            }

            // report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("POID", poID));
            #endregion
            #region SizeCode
            DualResult result;
            DataTable dtSizecode;
            string sqlcmd1 = string.Format(@"select  sizecode
	                    from dbo.Order_SizeCode WITH (NOLOCK) 
	                    where id in (select  poid from orders where id =   @OrderID ) and 
                        sizecode in ( select distinct  sizecode from dbo.Issue_Size WITH (NOLOCK)  where id = @ID and qty <>0) order by seq");

            string sizecodes = string.Empty;
            result = DBProxy.Current.Select(string.Empty, sqlcmd1, pars, out dtSizecode);

            if (dtSizecode == null || dtSizecode.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dtSizecode");
                return false;
            }

            foreach (DataRow dr in dtSizecode.Rows)
            {
                sizecodes += "[" + dr["sizecode"].ToString() + "]" + ",";
            }

            if (sizecodes.Length != 0)
            {
                sizecodes = sizecodes.Substring(0, sizecodes.Length - 1);
            }
            #endregion
            #region SEQ
            DataTable dtseq;

            string sqlcmd = string.Format(
                @"
select  a.Seq1 + '-' + a.Seq2 as SEQ
        ,dbo.getMtlDesc(a.poid,a.Seq1,a.Seq2,2,0) as Description
        ,Po_supp_detail.sizeunit as Unit
        ,Po_supp_detail.colorid as Color
        ,a.Qty as TransferQTY
        ,dbo.Getlocation(fi.ukey) as Location
        ,s.*
from(
    select * 
    from (
        select  sizecode
                ,Issue_DetailUkey
                , qty
        from dbo.Issue_Size WITH (NOLOCK) 
        where id = @ID
        and qty <>0
    ) as s
    PIVOT
    (
        Sum(qty)
        FOR sizecode  IN ({0})
    ) AS PivotTable
) as s
left join dbo.Issue_detail a WITH (NOLOCK) on ukey = s.Issue_DetailUkey
left join dbo.po_supp_detail WITH (NOLOCK) on po_supp_detail.id = a.POID and po_supp_detail.seq1 = a.seq1 and po_supp_detail.seq2=a.seq2
left join dbo.FtyInventory FI on a.poid = fi.poid and a.seq1= fi.seq1 and a.seq2 = fi.seq2 
    and a.roll = fi.roll and a.stocktype = fi.stocktype
", sizecodes);
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out dtseq);

            if (!result)
            {
                this.ShowErr(result);
                return true;
            }

            if (dtseq == null || dtseq.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dtseq");
                return false;
            }

            dtseq.Columns.Remove(dtseq.Columns["Issue_DetailUkey"]);
            string SEQ = dtseq.Rows[0]["SEQ"].ToString();

            // string tQty = dtseq.Rows[0]["tQTY"].ToString();
            // report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SEQ", SEQ));
            // report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("tQTY", tQty));
            #endregion
            #region LineNo
            DataTable dtlineno;
            string cLineNo;
            result = DBProxy.Current.Select(
                string.Empty,
                @"select o.sewline from dbo.Orders o WITH (NOLOCK) 
                    where id in (select distinct poid from issue_detail WITH (NOLOCK) where id=@ID ) ", pars, out dtlineno);
            if (!result)
            {
                this.ShowErr(result);
                return true;
            }

            if (dtlineno == null || dtlineno.Rows.Count == 0)
            {
                cLineNo = string.Empty;
            }
            else
            {
                cLineNo = dtlineno.Rows[0]["sewline"].ToString();
            }

            // report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("sewline", cLineNo));
            #endregion
            #region CellNo
            DataTable dtcutcell;
            string cCellNo;
            result = DBProxy.Current.Select(
                string.Empty,
                @"select    
             b.CutCellID 
            from dbo.Issue as a WITH (NOLOCK) 
             inner join dbo.cutplan as b WITH (NOLOCK) on b.id = a.cutplanid
            where b.id = a.CutplanID
            ", pars, out dtcutcell);
            if (!result)
            {
                this.ShowErr(result);
                return true;
            }

            if (dtcutcell == null || dtcutcell.Rows.Count == 0)
            {
                cCellNo = string.Empty;
            }
            else
            {
                cCellNo = dtcutcell.Rows[0]["CutCellID"].ToString();
            }

            // report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CutCellID", cCellNo));
            #endregion

            // 計算SizeCode欄位超過10個以上就產出Excel
            int SizceCount = dtseq.Columns.Count;
            if (SizceCount > 16)
            {
                #region Excel
                string xlt = @"Warehouse_P11.xltx";
                SaveXltReportCls xl = new SaveXltReportCls(xlt);
                xl.BoOpenFile = true;

                xl.DicDatas.Add("##RptTitle", RptTitle);
                xl.DicDatas.Add("##ID", id);
                xl.DicDatas.Add("##cutplanid", request);
                xl.DicDatas.Add("##issuedate", issuedate);
                xl.DicDatas.Add("##remark", remark);
                xl.DicDatas.Add("##cCutNo", cutno);
                xl.DicDatas.Add("##cLineNo", LineNo);
                xl.DicDatas.Add("##OrderID", OrderID);
                xl.DicDatas.Add("##cCellNo", CellNo);
                SaveXltReportCls.XltRptTable xlTable = new SaveXltReportCls.XltRptTable(dtseq);
                int allColumns = dtseq.Columns.Count;
                int sizeColumns = dtSizecode.Rows.Count;
                Microsoft.Office.Interop.Excel.Worksheet wks = xl.ExcelApp.ActiveSheet;
                string cc = MyUtility.Excel.ConvertNumericToExcelColumn(dtseq.Columns.Count);

                // 合併儲存格
                wks.get_Range("G9", cc + "9").Merge(false);
                wks.Cells[9, 7] = "SIZE";

                // 框線
                wks.Range["G9", cc + "10"].Borders.LineStyle = 1;

                // 置中
                wks.get_Range("G9", cc + "9").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                for (int i = 6; i < dtseq.Columns.Count; i++)
                {
                    wks.Cells[10, i + 1] = dtseq.Columns[i].ColumnName;
                }

                xlTable.Borders.OnlyHeaderBorders = true;
                xlTable.Borders.AllCellsBorders = true;
                xlTable.ShowHeader = false;
                xl.DicDatas.Add("##SEQ", xlTable);

                xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P11"));
                #endregion
            }
            else
            {
                #region RDLC
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cutplanid", request));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cCutNo", cutno));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cLineNo", LineNo));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("OrderID", OrderID));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cCellNo", CellNo));

                // 取得size欄位名稱
                for (int i = 6; i < dtseq.Columns.Count; i++)
                {
                    report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("size" + (i - 5).ToString(), dtseq.Columns[i].ColumnName));
                }

                // 將size的欄位加到10個
                for (int i = dtseq.Columns.Count - 1; i < 16; i++)
                {
                    dtseq.Columns.Add("#!#!##" + i.ToString());
                }

                List<P11_PrintData> data = dtseq.AsEnumerable()
                                           .Select(row1 => new P11_PrintData()
                                           {
                                               SEQ = row1["SEQ"].ToString().Trim(),
                                               Description = row1["Description"].ToString().Trim(),
                                               Color = row1["Color"].ToString().Trim(),
                                               Location = row1["Location"].ToString().Trim(),
                                               TransferQTY = row1["TransferQTY"].ToString().Trim(),
                                               Unit = row1["Unit"].ToString().Trim(),
                                               size1 = row1[6].ToString().Trim(),
                                               size2 = row1[7].ToString().Trim(),
                                               size3 = row1[8].ToString().Trim(),
                                               size4 = row1[9].ToString().Trim(),
                                               size5 = row1[10].ToString().Trim(),
                                               size6 = row1[11].ToString().Trim(),
                                               size7 = row1[12].ToString().Trim(),
                                               size8 = row1[13].ToString().Trim(),
                                               size9 = row1[14].ToString().Trim(),
                                               size10 = row1[15].ToString().Trim(),
                                           }).ToList();

                report.ReportDataSource = data;

                #region  指定是哪個 RDLC
                Type ReportResourceNamespace = typeof(P11_PrintData);
                Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
                string ReportResourceName = "P11_Print.rdlc";

                IReportResource reportresource;
                if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
                {
                    // this.ShowException(result);
                    return false;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Sci.Win.Subs.ReportView(report);
                frm.MdiParent = this.MdiParent;
                frm.Show();
                #endregion
                #endregion
            }

            return true;
        }

        public static void ProcessWithDatatable2(DataTable source, string tmp_columns, string sqlcmd, out DataTable result, string temptablename = "#tmp")
        {
            result = null;
            StringBuilder sb = new StringBuilder();
            if (temptablename.TrimStart().StartsWith("#"))
            {
                sb.Append(string.Format("create table {0} (", temptablename));
            }
            else
            {
                sb.Append(string.Format("create table #{0} (", temptablename));
            }

            string[] cols = tmp_columns.Split(',');
            for (int i = 0; i < cols.Length; i++)
            {
                if (MyUtility.Check.Empty(cols[i]))
                {
                    continue;
                }

                switch (Type.GetTypeCode(source.Columns[cols[i]].DataType))
                {
                    case TypeCode.Boolean:
                        sb.Append(string.Format("[{0}] bit", cols[i]));
                        break;

                    case TypeCode.Char:
                        sb.Append(string.Format("[{0}] varchar(1)", cols[i]));
                        break;

                    case TypeCode.DateTime:
                        sb.Append(string.Format("[{0}] datetime", cols[i]));
                        break;

                    case TypeCode.Decimal:
                        sb.Append(string.Format("[{0}] numeric(24,8)", cols[i]));
                        break;

                    case TypeCode.Int32:
                        sb.Append(string.Format("[{0}] int", cols[i]));
                        break;

                    case TypeCode.String:
                        sb.Append(string.Format("[{0}] varchar(max)", cols[i]));
                        break;

                    case TypeCode.Int64:
                        sb.Append(string.Format("[{0}] bigint", cols[i]));
                        break;
                    default:
                        break;
                }

                if (i < cols.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append(")");

            System.Data.SqlClient.SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);

            try
            {
                DualResult result2 = DBProxy.Current.ExecuteByConn(conn, sb.ToString());
                if (!result2)
                {
                    MyUtility.Msg.ShowException(null, result2);
                    return;
                }

                using (System.Data.SqlClient.SqlBulkCopy bulkcopy = new System.Data.SqlClient.SqlBulkCopy(conn))
                {
                    bulkcopy.BulkCopyTimeout = 60;
                    if (temptablename.TrimStart().StartsWith("#"))
                    {
                        bulkcopy.DestinationTableName = temptablename.Trim();
                    }
                    else
                    {
                        bulkcopy.DestinationTableName = string.Format("#{0}", temptablename.Trim());
                    }

                    for (int i = 0; i < cols.Length; i++)
                    {
                        bulkcopy.ColumnMappings.Add(cols[i], cols[i]);
                    }

                    bulkcopy.WriteToServer(source);
                    bulkcopy.Close();
                }

                result2 = DBProxy.Current.SelectByConn(conn, sqlcmd, out result);
                if (!result2)
                {
                    MyUtility.Msg.ShowException(null, result2);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        private void txtOrderID_Validating(object sender, EventArgs e)
        {
            if (this.txtOrderID.Text == this.txtOrderID.OldValue)
            {
                return;
            }

            this.CurrentMaintain["orderid"] = this.txtOrderID.Text;
            this.displayPOID.Text = string.Empty;
            this.poid = MyUtility.GetValue.Lookup(string.Format(
                @"
select orders.poid 
from dbo.orders WITH (NOLOCK) 
left join dbo.Factory on orders.FtyGroup = Factory.ID 
where   orders.id='{0}' and Category !='A'
        and Factory.mdivisionid = '{1}'", this.CurrentMaintain["orderid"], Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(this.txtOrderID.Text))
            {
                if (MyUtility.Check.Empty(this.poid))
                {
                    this.CurrentMaintain["cutplanid"] = string.Empty;
                    this.txtRequest.Text = string.Empty;
                    this.txtOrderID.Text = string.Empty;
                    MyUtility.Msg.WarningBox("Can't found data");
                    return;
                }

                this.displayPOID.Text = this.poid;

                // CurrentMaintain["orderid"] = this.poid;
            }
        }

        private void txtOrderID_Validated(object sender, EventArgs e) // 若order ID有變，重新撈取資料庫。
        {
            if (this.txtOrderID.Text == this.txtOrderID.OldValue)
            {
                return;
            }

            // DBProxy.Current.Execute(null, string.Format("delete from dbo.issue_breakdown where id='{0}';", CurrentMaintain["id"].ToString()));
            this.CurrentMaintain["cutplanid"] = string.Empty;
            if (MyUtility.Check.Empty(this.poid))
            {
                this.dtIssueBreakDown = null;
                this.gridIssueBreakDown.DataSource = null;
                foreach (DataRow dr in this.DetailDatas)
                {
                    // 刪除SubDetail資料
                    ((DataTable)this.detailgridbs.DataSource).Rows.Remove(dr);
                    dr.Delete();
                }

                return;
            }

            DualResult result = this.Detail_Reload();

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            this.Ismatrix_Reload = true;
            this.matrix_Reload();
            this.detailgridbs.Position = 0;
            this.detailgrid.Focus();
            this.detailgrid.CurrentCell = this.detailgrid[10, 0];
            this.detailgrid.BeginEdit(true);
        }

        protected override void OnFormLoaded()
        {
            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup
union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("(SELECT ftygroup FROM orders WHERE ORDERID = orders.id)  = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        private DualResult Detail_Reload()
        {
            foreach (DataRow dr in this.DetailDatas)
            {
                // 刪除SubDetail資料
                ((DataTable)this.detailgridbs.DataSource).Rows.Remove(dr);
                dr.Delete();
            }

            DataTable subData;

            // StockType 必須保留，否則 BalanceQty 會出問題
            DBProxy.Current.Select(null, string.Format(
                @"
select  poid = b.ID
        , a.Ukey
        , b.Seq1
        , b.Seq2
        , concat (Ltrim (Rtrim (b.seq1)), ' ', b.seq2) seq
        , a.StockType
        , ColorID = isnull(dbo.GetColorMultipleID(b.BrandId, b.ColorID), '')
        , b.SizeSpec
        , b.UsedQty
        , b.SizeUnit
        , b.StockUnit
        , dbo.Getlocation (a.ukey) [location]
        , [Production].[dbo].getmtldesc (b.id, b.seq1, b.seq2, 2, 0)[description]
        , isnull ((a.InQty - a.OutQty + a.AdjustQty ),0.00) as balanceqty
from [Production].[dbo].po_supp_detail b WITH (NOLOCK) 
inner join [Production].[dbo].Fabric f WITH (NOLOCK) on f.SCIRefno = b.SCIRefno
inner join [Production].[dbo].MtlType m WITH (NOLOCK) on m.ID = f.MtlTypeID
left join [Production].[dbo].ftyinventory a WITH (NOLOCK) on b.id = a.POID 
                                                             and b.seq1 = a.seq1 
                                                             and b.seq2 = a.Seq2
                                                             and stocktype = 'B'
                                                             and a.Roll = ''
where   b.ID = '{1}' 
        and b.FabricType = 'A'
        and m.IssueType = 'Sewing' 
        and b.Junk != 1
order by b.ID, b.seq1, b.seq2", Sci.Env.User.Keyword, this.poid, 0), out subData);

            if (subData.Rows.Count == 0)
            {
                this.txtOrderID.Text = string.Empty;
                return Result.F("No PO Data !");
            }

            // 將資料塞入表身
            foreach (DataRow dr in subData.Rows)
            {
                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
                DataRow ndr = detailDt.NewRow();
                ndr["poid"] = dr["poid"];
                ndr["seq"] = dr["seq"];
                ndr["Description"] = dr["Description"];
                ndr["Colorid"] = dr["Colorid"];
                ndr["SizeSpec"] = dr["SizeSpec"];
                ndr["usedqty"] = dr["usedqty"];
                ndr["SizeUnit"] = dr["SizeUnit"];
                ndr["location"] = dr["location"];
                ndr["balanceqty"] = dr["balanceqty"];
                ndr["seq1"] = dr["seq1"];
                ndr["seq2"] = dr["seq2"];
                ndr["stocktype"] = dr["stocktype"];
                ndr["ftyinventoryukey"] = dr["ukey"];
                ndr["StockUnit"] = dr["StockUnit"];

                detailDt.Rows.Add(ndr);

                DataTable sizeRange, subDetails;
                if (this.GetSubDetailDatas(ndr, out subDetails))
                {
                    DBProxy.Current.Select(null, string.Format(
                        @"
select  a.SizeCode
        , b.Id
        , b.Issue_DetailUkey
        , isnull(b.Qty,0) QTY 
from dbo.Order_SizeCode a WITH (NOLOCK) 
left join dbo.Issue_Size b WITH (NOLOCK) on b.SizeCode = a.SizeCode 
                                            and b.id = '{1}'
                                            --and b.Issue_DetailUkey = {2}
where   a.id = '{0}' 
order by Seq ", this.poid, this.CurrentMaintain["id"], ndr["ukey"]), out sizeRange);
                    if (sizeRange == null)
                    {
                        continue;
                    }

                    foreach (DataRow drr in sizeRange.Rows)
                    {
                        drr.AcceptChanges();
                        drr.SetAdded();
                        subDetails.ImportRow(drr);
                    }
                }
            }

            return Result.True;
        }

        private void checkByCombo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.dtIssueBreakDown == null)
            {
                return;
            }

            if (this.checkByCombo.Checked)
            {
                this.dtIssueBreakDown.DefaultView.RowFilter = string.Format(string.Empty);
            }
            else
            {
                this.dtIssueBreakDown.DefaultView.RowFilter = string.Format("OrderID='{0}'", this.txtOrderID.Text);
            }

            string sql = string.Empty;

            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (this.EditMode)
            {
            if (this.checkByCombo.Checked)
            {
                sql = $@"
select distinct seq,os.sizecode
from dbo.Order_SizeCode os WITH(NOLOCK)
inner join orders o WITH(NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH(NOLOCK) on o.id = oq.ID and os.SizeCode = oq.SizeCode
where o.POID = '{this.poid}'
";
            }
            else
            {
                sql = $@"
select distinct seq,os.sizecode
from dbo.Order_SizeCode os WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.POID = os.Id
inner join dbo.Order_Qty oq WITH (NOLOCK) on o.id=oq.ID and os.SizeCode = oq.SizeCode
where  o.id ='{this.CurrentMaintain["orderid"]}'
";
            }

            DataTable sizecodeDt;
            DBProxy.Current.Select(null, sql, out sizecodeDt);

            foreach (DataRow dr in this.DetailDatas)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        DataTable subDt;
                        this.GetSubDetailDatas(dr, out subDt);
                        foreach (DataRow subdr in subDt.Rows)
                        {
                            if (!sizecodeDt.AsEnumerable().Any(r => r["Sizecode"].ToString() == MyUtility.Convert.GetString(subdr["sizecode"])))
                            {
                                subdr["Qty"] = 0;
                            }
                        }

                        dr["output"] = string.Join(
                            ", ",
                            subDt.AsEnumerable()
                                     .Where(row => !MyUtility.Check.Empty(row["Qty"]))
                                     .Select(row => row["SizeCode"].ToString() + "*" + Convert.ToDecimal(row["qty"]).ToString("0.00")));
                        dr["qty"] = Math.Round(
                            subDt.AsEnumerable()
                                                    .Where(row => !MyUtility.Check.Empty(row["Qty"]))
                                                    .Sum(row => Convert.ToDouble(row["Qty"].ToString())),
                            2);
                    }
                }
            }
        }

        protected override void OnDetailGridRowChanged()
        {
        }
    }
}
