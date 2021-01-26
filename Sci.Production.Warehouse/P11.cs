using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Utility.Excel;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P11 : Win.Tems.Input8
    {
        private StringBuilder sbSizecode;
        private StringBuilder sbSizecode2;
        private DataTable dtSizeCode = null;
        private DataTable dtIssueBreakDown = null;
        private DataRow dr;
        private string poid = string.Empty;
        private bool Ismatrix_Reload = true; // 是否需要重新抓取資料庫資料
        private P11_Detail subform = new P11_Detail();

        /// <inheritdoc/>
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Location = new Point(this.btnBreakDown.Location.X + 20, 128); // 此gridcon位置會跑掉，需強制設定gridcon位置
            this.gridicon.Anchor = AnchorStyles.Right;

            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Env.User.Keyword);
            this.DefaultWhere = string.Format("Type='B' and MDivisionID = '{0}'", Env.User.Keyword);

            // Issue此為PMS自行建立的資料，MDivisionID皆會有寫入值
            this.WorkAlias = "Issue";                        // PK: ID
            this.GridAlias = "Issue_detail";           // PK: ID+UKey
            this.SubGridAlias = "Issue_size";          // PK: ID+Issue_DetailUkey+SizeCode

            this.KeyField1 = "ID"; // Issue PK
            this.KeyField2 = "ID"; // Summary FK

            this.SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            this.SubDetailKeyField1 = "id,Ukey";    // second PK
            this.SubDetailKeyField2 = "id,Issue_DetailUkey"; // third FK

            this.DoSubForm = this.subform;

            #region 新增 Print Kanban Card 按鈕
            Win.UI.Button btnPrintKanbanCard = new Win.UI.Button
            {
                Text = "Print Kanban Card",
                Size = new Size(137, 30),
            };

            btnPrintKanbanCard.Click += new EventHandler(this.BtnPrintKanbanCard_Click);
            this.browsetop.Controls.Add(btnPrintKanbanCard);
            #endregion
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region -- outqty 開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellMouseDoubleClick += (s, e) =>
            {
                #region DoSubForm & subform 參數設定
                this.DoSubForm.IsSupportDelete = false;
                this.DoSubForm.IsSupportNew = false;

                this.subform.Master = this.CurrentMaintain;
                this.subform.Combo = this.checkByCombo.Checked;
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
                        this.GetSubDetailDatas(dr, out DataTable subDt);

                        DataTable keepDt = subDt.Clone();

                        foreach (DataRow subDr in subDt.Rows)
                        {
                            keepDt.ImportRow(subDr);
                        }

                        originSub.Add(dr, keepDt);
                    }
                }
                #endregion
                this.OpenSubDetailPage();

                #region Final
                this.GetSubDetailDatas(out DataTable finalSubDt);
                if (!this.subform.IsSave)
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
                            this.GetSubDetailDatas(dr, out DataTable unchangeSubDt);
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
                        this.GetSubDetailDatas(dr, out finalSubDt);
                        dr["output"] = string.Join(
                            ", ",
                            finalSubDt.AsEnumerable()
                                 .Where(row => !MyUtility.Check.Empty(row["Qty"]))
                                 .Select(row => row["SizeCode"].ToString() + "*" + Convert.ToDecimal(row["qty"]).ToString("0.00")));

                        dr["qty"] = Math.Round(
                            finalSubDt.AsEnumerable()
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

            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = string.Format(
                        @"
select  poid = b.ID
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
                    if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable bulkItems)))
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(
                        bulkItems,
                        "FabricType,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,inqty,outqty,adjustqty",
                        "4,14,10,10,13,4,3,6,6,6,10",
                        this.CurrentDetailData["seq"].ToString(),
                        "FabricType,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,In Qty,Out Qty,Adjust Qty,Ukey")
                    {
                        Width = 1024,
                    };

                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = selepoitem.GetSelecteds();
                    this.CurrentDetailData["seq"] = x[0]["seq"];
                    this.CurrentDetailData["seq1"] = x[0]["seq1"];
                    this.CurrentDetailData["seq2"] = x[0]["seq2"];
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
this.CurrentDetailData["poid"],
seq[0],
seq[1]), out this.dr))
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
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts2)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("usedqty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
            .Text("SizeUnit", header: "SizeUnit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("qty", header: "Pick Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
            .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("output", header: "Pick Output", width: Widths.AnsiChars(20), iseditingreadonly: true, settings: ts)
            .Numeric("balanceqty", header: "Balance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("AutoPickqty", header: "AutoPick \r\n Calculation \r\n Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
            .Text("OutputAutoPick", header: "AutoPick \r\n Calculation \r\n Output", width: Widths.AnsiChars(20), iseditingreadonly: true)
            ;
            #endregion 欄位設定

            #region 可編輯欄位變色
            this.detailgrid.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["output"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion 可編輯欄位變色
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
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
        , AutoPickqty=(select SUM(AutoPickQty)  from Issue_Size iss where iss.Issue_DetailUkey = a.ukey)
        , OutputAutoPick=(
			select  STUFF((
				select CONCAT(',',rtrim(iss.SizeCode),'*',iss.AutoPickQty)
				from Issue_Size iss
				where iss.Issue_DetailUkey = a.ukey
                and iss.AutoPickQty <> 0
				for xml path('')
			),1,1,'')
			)
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

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
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

            this.txtOrderID.IsSupportEditMode = false;
            this.txtRequest.IsSupportEditMode = false;
            this.txtOrderID.ReadOnly = true;
            this.txtRequest.ReadOnly = true;
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
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

            foreach (DataRow checkduplicate in this.DetailDatas)
            {
                DataRow[] findrow = this.DetailDatas.AsEnumerable().Where(row => row["poid"].EqualString(checkduplicate["poid"].ToString()) && row["seq1"].EqualString(checkduplicate["seq1"])
                                                                          && row["seq2"].EqualString(checkduplicate["seq2"].ToString())).ToArray();
                if (findrow.Length > 1)
                {
                    MyUtility.Msg.WarningBox(string.Format(@"SP#: {0} Seq#: {1}-{2} duplicate, SP# and Seq# can't duplicate", checkduplicate["poid"], checkduplicate["seq1"], checkduplicate["seq2"]));
                    return false;
                }
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "IS", "Issue", (DateTime)this.CurrentMaintain["IssueDate"]);
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
;delete from dbo.issue_breakdown where id='{0}' and qty = 0; ", this.CurrentMaintain["id"],
                    this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1));

                string aaa = this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1).Replace("[", string.Empty).Replace("]", string.Empty);

                ProcessWithDatatable2(this.dtIssueBreakDown, "OrderID,Article," + aaa, sqlcmd, out result, "#tmp");
            }

            // 將需新增的資料狀態更改為新增
            foreach (DataRow dr in this.DetailDatas)
            {
                this.GetSubDetailDatas(dr, out DataTable subDT);
                foreach (DataRow temp in subDT.Rows)
                {
                    if (temp["isvirtual"].ToString() == "1")
                    {
                        temp.AcceptChanges();
                        temp.SetAdded();
                    }
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePre()
        {
            return base.ClickSavePre();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            string deleteSql = $@"delete Issue_Size where id = '{this.CurrentMaintain["id"]}' and Qty = 0 and AutoPickQty = 0";
            DualResult result = DBProxy.Current.Execute(null, deleteSql);
            if (!result)
            {
                return result;
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            foreach (DataRow dr in this.DetailDatas)
            {
                if (this.GetSubDetailDatas(dr, out DataTable dt))
                {
                    Sum_subDetail(dr, dt);
                }
            }

            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        private static void Sum_subDetail(DataRow target, DataTable source)
        {
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
        }

        private void BtnAutoPick_Click(object sender, EventArgs e)
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

            var frm = new P11_AutoPick(this.CurrentMaintain["id"].ToString(), this.poid, this.txtOrderID.Text.ToString(), this.dtIssueBreakDown, this.sbSizecode, this.checkByCombo.Checked)
            {
                DictionaryDatas = new Dictionary<DataRow, DataTable>(),
            };

            DialogResult result = frm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                DataTable detail = (DataTable)this.detailgridbs.DataSource;

                // 刪除表身重新匯入
                this.DetailDatas.ToList().ForEach(row => row.Delete());

                // 批次匯入
                foreach (KeyValuePair<DataRow, DataTable> item in frm.DictionaryDatas)
                {
                    DataRow tmp = item.Key;
                    if (tmp["selected"].ToString() == "1")
                    {
                        // 匯入Issue_detail layer
                        tmp["id"] = this.CurrentMaintain["id"];
                        detail.ImportRowAdded(tmp);

                        // 匯入Issue_Size layer
                        if (this.GetSubDetailDatas(detail.Rows[detail.Rows.Count - 1], out DataTable subDetail))
                        {
                            item.Value.AsEnumerable().ToList().ForEach(row => subDetail.ImportRowAdded(row));
                        }
                    }
                }

                detail.DefaultView.Sort = string.Empty;
            }
        }

        private void TxtRequest_Validating(object sender, CancelEventArgs e)
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
", this.CurrentMaintain["cutplanid"],
                Env.User.Keyword));

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
            this.Matrix_Reload();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (!(this.CurrentMaintain == null))
            {
                this.displayCutCell.Text = MyUtility.GetValue.Lookup(string.Format("select CutCellID from dbo.cutplan WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["cutplanid"]));
                this.displayLineNo.Text = MyUtility.GetValue.Lookup(string.Format(
                    @"select t.SewLine+','  from (select distinct o.SewLine 
from dbo.Issue_detail a WITH (NOLOCK) inner join dbo.orders o WITH (NOLOCK) on a.Poid = o.POID where a.id='{0}' and o.sewline !='') t for xml path('')", this.CurrentMaintain["id"]));

                string sqlcmd = string.Format(
                    @";with cte as
(Select WorkOrder.FabricCombo,Cutplan_Detail.CutNo from Cutplan_Detail WITH (NOLOCK) inner join dbo.workorder WITH (NOLOCK) on WorkOrder.Ukey = Cutplan_Detail.WorkorderUkey 
where Cutplan_Detail.ID='{0}' )
select distinct FabricCombo ,(select convert(varchar,CutNo)+',' 
from (select CutNo from cte where cte.FabricCombo = a.FabricCombo )t order by CutNo for xml path('')) cutnos from cte a
", this.CurrentMaintain["cutplanid"]);
                DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                this.editCutNo.Text = string.Join(" / ", dt.AsEnumerable().Select(row => row["FabricCombo"].ToString() + "-" + row["cutnos"].ToString()));

                #region -- Status Label --

                this.label25.Text = this.CurrentMaintain["status"].ToString();

                #endregion Status Label
                #region -- POID
                this.Getpoid();
                this.displayPOID.Text = this.poid;
                this.displayCustCD.Text = MyUtility.GetValue.Lookup(string.Format("select CustCDID from dbo.Orders WITH (NOLOCK) where id='{0}'", this.txtOrderID.Text));
                #endregion

                #region -- matrix breakdown
                DualResult result;
                if (!(result = this.Matrix_Reload()))
                {
                    this.ShowErr(result);
                }
                #endregion
            }
        }

        private DualResult Matrix_Reload()
        {
            if (this.EditMode == true && this.Ismatrix_Reload == false)
            {
                return Ict.Result.True;
            }

            this.Ismatrix_Reload = false;
            string sqlcmd;
            StringBuilder sbIssueBreakDown;
            DualResult result;

            string orderID = this.txtOrderID.Text;

            sqlcmd = string.Format(
                @"select sizecode from dbo.order_sizecode WITH (NOLOCK) 
where id = (select poid from dbo.orders WITH (NOLOCK) where id='{0}') order by seq", orderID);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out this.dtSizeCode)))
            {
                this.ShowErr(sqlcmd, result);
                return Ict.Result.True;
            }

            if (this.dtSizeCode.Rows.Count == 0)
            {
                // MyUtility.Msg.WarningBox(string.Format("Becuase there no sizecode data belong this OrderID {0} , can't show data!!", CurrentDataRow["orderid"]));
                this.dtIssueBreakDown = null;
                this.gridIssueBreakDown.DataSource = null;
                return Ict.Result.True;
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
            order by [OrderID],[Article]", orderID,
                this.CurrentMaintain["id"],
                this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1)));
            if (!(result = DBProxy.Current.Select(null, sbIssueBreakDown.ToString(), out this.dtIssueBreakDown)))
            {
                this.ShowErr(sqlcmd, result);
                return Ict.Result.True;
            }

            this.gridIssueBreakDown.AutoGenerateColumns = true;
            this.gridIssueBreakDownBS.DataSource = this.dtIssueBreakDown;
            this.gridIssueBreakDown.DataSource = this.gridIssueBreakDownBS;
            this.gridIssueBreakDown.IsEditingReadOnly = true;
            this.gridIssueBreakDown.ReadOnly = true;

            this.CheckByCombo_CheckedChanged(null, null);

            return Ict.Result.True;
        }

        private void Getpoid()
        {
            this.CurrentMaintain["cutplanid"] = this.txtRequest.Text;
            this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.cutplan WITH (NOLOCK) where id='{0}' and mdivisionid = '{1}'", this.CurrentMaintain["cutplanid"], Env.User.Keyword));
            if (MyUtility.Check.Empty(this.poid))
            {
                this.poid = MyUtility.GetValue.Lookup(string.Format("select orders.poid from dbo.orders WITH (NOLOCK) left join dbo.Factory on orders.FtyGroup=Factory.ID where orders.id='{0}' and Factory.mdivisionid = '{1}'", this.CurrentMaintain["orderid"], Env.User.Keyword));
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            string strSort = ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort;
            ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = string.Empty;
            base.OnDetailGridInsert(index);

            // CurrentDetailData["mdivisionid"] = Sci.Env.User.Keyword;
            this.CurrentDetailData["poid"] = this.poid;
            if (this.GetSubDetailDatas(this.CurrentDetailData, out DataTable subDetails))
            {
                string sqlcmd = string.Format(
                    @"select a.SizeCode,'{1}' AS Id,0.00 AS QTY
from dbo.Order_SizeCode a WITH (NOLOCK) 
where a.id='{0}' order by Seq", this.poid,
                    this.CurrentMaintain["id"]);
                DBProxy.Current.Select(null, sqlcmd, out DataTable sizeRange);
                foreach (DataRow dr in sizeRange.Rows)
                {
                    subDetails.ImportRowAdded(dr);
                }
            }

            ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = strSort;
        }

        /// <inheritdoc/>
        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? string.Empty : e.Detail["ID"].ToString();
            string ukey = (e.Detail == null || MyUtility.Check.Empty(e.Detail["ukey"])) ? "0" : e.Detail["ukey"].ToString();
            string poid1 = MyUtility.GetValue.Lookup($"select poid from dbo.cutplan WITH (NOLOCK) where id='{this.CurrentMaintain["cutplanid"]}' and mdivisionid = '{Env.User.Keyword}'");
            string poid2 = MyUtility.GetValue.Lookup($"select orders.poid from dbo.orders WITH (NOLOCK) left join dbo.Factory on orders.FtyGroup=Factory.ID where orders.id='{this.CurrentMaintain["orderid"]}' and Factory.mdivisionid = '{Env.User.Keyword}'");
            string poid = poid1.Empty() ? poid2 : poid1;
            this.SubDetailSelectCommand = $@"
;with aaa as(
    select  
         a.SizeCode
        , b.Id
        , Issue_DetailUkey =  b.Issue_DetailUkey
        , QTY = isnull(b.Qty,0)
        , isvirtual = IIF(b.Qty IS NULL , 1 ,0)
        , seq
        , AutoPickQty = isnull(AutoPickQty, 0)
        , Diffqty = isnull(AutoPickQty, 0) - isnull(b.Qty,0)
    from  dbo.Issue_Size b WITH (NOLOCK) 
    inner join dbo.Order_SizeCode a WITH (NOLOCK) on b.SizeCode = a.SizeCode
    where a.id= '{poid}'
    and b.id = '{masterID}' and b.Issue_DetailUkey = {ukey}
)
,bbb as(
	select distinct os.sizecode,ID = '{this.CurrentMaintain["orderid"]}',Issue_DetailUkey = '{ukey}',QTY=0,isvirtual = 1,seq, AutoPickQty=0,Diffqty = 0
	from dbo.Order_SizeCode os WITH(NOLOCK)
	inner join orders o WITH(NOLOCK) on o.POID = os.Id
	inner join dbo.Order_Qty oq WITH(NOLOCK) on o.id = oq.ID and os.SizeCode = oq.SizeCode
	where o.POID = '{this.poid}' 
	and not exists(select SizeCode from aaa where aaa.SizeCode = os.sizecode)
)
select SizeCode,Id,Issue_DetailUkey,QTY,isvirtual,AutoPickQty,Diffqty
from(
	select * from aaa
	union all
	select * from bbb
)ccc
order by seq
";

            return base.OnSubDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
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
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
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
                            tmp["poid"],
                            tmp["seq1"],
                            tmp["seq2"],
                            tmp["roll"],
                            tmp["Dyelot"]);
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
                            tmp["poid"],
                            tmp["seq1"],
                            tmp["seq2"],
                            tmp["roll"],
                            tmp["balanceqty"],
                            tmp["qty"],
                            tmp["Dyelot"]);
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
where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["id"]);

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
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion
            #endregion
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out DataTable resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        datacheck, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
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
            this.SentToVstrong_AutoWH_ACC(true);
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
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
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
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
                            tmp["poid"],
                            tmp["seq1"],
                            tmp["seq2"],
                            tmp["roll"],
                            tmp["Dyelot"]);
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
                            tmp["poid"],
                            tmp["seq1"],
                            tmp["seq2"],
                            tmp["roll"],
                            tmp["balanceqty"],
                            tmp["qty"],
                            tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update Issue set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID,
                this.CurrentMaintain["id"]);

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
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out DataTable resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(
                        bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
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
            this.SentToVstrong_AutoWH_ACC(false);
        }

        private void BtnBOA_Click(object sender, EventArgs e)
        {
            var frm = new P11_BOA(this.CurrentMaintain["id"].ToString(), this.poid, this.CurrentMaintain["cutplanid"].ToString(), this.txtOrderID.Text.ToString());
            frm.ShowDialog(this);
        }

        private void BtnClear_Click(object sender, EventArgs e)
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

        private void BtnBreakDown_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["cutplanID"]) && MyUtility.Check.Empty(this.CurrentMaintain["OrderId"]))
            {
                MyUtility.Msg.WarningBox("Please key-in Request or Order ID first!!");
                return;
            }

            var frm = new P11_IssueBreakDown(this.CurrentMaintain, this.dtIssueBreakDown, this.dtSizeCode);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
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
            string lineNo = this.displayLineNo.Text;
            string orderID = this.txtOrderID.Text;
            string cellNo = this.displayCutCell.Text;
            List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
                new SqlParameter("@MDivision", Env.User.Keyword),
                new SqlParameter("@OrderID", orderID),
            };
            #region Title
            DBProxy.Current.Select(string.Empty, @"select NameEN from MDivision where id = @MDivision", pars, out DataTable dt);
            string rptTitle = dt.Rows[0]["NameEN"].ToString();

            #endregion
            #region SP
            string poID;
            string sqlcmd = @"
select (select poid+',' from 
(select distinct cd.POID from Cutplan_Detail cd WITH (NOLOCK) where id =(select CutplanID from dbo.Issue WITH (NOLOCK) where id='@id')  ) t
for xml path('')) as [poid]";
            DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtsp);
            if (dtsp.Rows.Count == 0)
            {
                poID = string.Empty;
            }
            else
            {
                poID = dtsp.Rows[0]["POID"].ToString();
            }

            #endregion
            #region SizeCode
            DualResult result;
            string sqlcmd1 = string.Format(@"select  sizecode
	                    from dbo.Order_SizeCode WITH (NOLOCK) 
	                    where id in (select  poid from orders where id =   @OrderID ) and 
                        sizecode in ( select distinct  sizecode from dbo.Issue_Size WITH (NOLOCK)  where id = @ID and qty <>0) order by seq");

            string sizecodes = string.Empty;
            result = DBProxy.Current.Select(string.Empty, sqlcmd1, pars, out DataTable dtSizecode);

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

            sqlcmd = string.Format(
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
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtseq);

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
            string sEQ = dtseq.Rows[0]["SEQ"].ToString();

            #endregion
            #region LineNo

            // string tQty = dtseq.Rows[0]["tQTY"].ToString();
            // report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SEQ", SEQ));
            // report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("tQTY", tQty));
            #endregion
            #region LineNo
            string cLineNo;
            sqlcmd = @"select o.sewline from dbo.Orders o WITH (NOLOCK) where id in (select distinct poid from issue_detail WITH (NOLOCK) where id=@ID ) ";
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtlineno);
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

            #endregion
            #region CellNo

            #endregion
            #region CellNo
            string cCellNo;
            sqlcmd = @"
select b.CutCellID 
from dbo.Issue as a WITH (NOLOCK) 
inner join dbo.cutplan as b WITH (NOLOCK) on b.id = a.cutplanid
where b.id = a.CutplanID
";
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtcutcell);
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

            #endregion

            // 計算SizeCode欄位超過10個以上就產出Excel
            int sizceCount = dtseq.Columns.Count;
            if (sizceCount > 16)
            {
                #region Excel
                string xlt = @"Warehouse_P11.xltx";
                SaveXltReportCls xl = new SaveXltReportCls(xlt)
                {
                    BoOpenFile = true,
                };

                xl.DicDatas.Add("##RptTitle", rptTitle);
                xl.DicDatas.Add("##ID", id);
                xl.DicDatas.Add("##cutplanid", request);
                xl.DicDatas.Add("##issuedate", issuedate);
                xl.DicDatas.Add("##remark", remark);
                xl.DicDatas.Add("##cCutNo", cutno);
                xl.DicDatas.Add("##cLineNo", lineNo);
                xl.DicDatas.Add("##OrderID", orderID);
                xl.DicDatas.Add("##cCellNo", cellNo);
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

                xl.Save(Class.MicrosoftFile.GetName("Warehouse_P11"));
                #endregion
            }
            else
            {
                #region RDLC
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cutplanid", request));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cCutNo", cutno));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cLineNo", lineNo));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("OrderID", orderID));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cCellNo", cellNo));

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
                                               Size1 = row1[6].ToString().Trim(),
                                               Size2 = row1[7].ToString().Trim(),
                                               Size3 = row1[8].ToString().Trim(),
                                               Size4 = row1[9].ToString().Trim(),
                                               Size5 = row1[10].ToString().Trim(),
                                               Size6 = row1[11].ToString().Trim(),
                                               Size7 = row1[12].ToString().Trim(),
                                               Size8 = row1[13].ToString().Trim(),
                                               Size9 = row1[14].ToString().Trim(),
                                               Size10 = row1[15].ToString().Trim(),
                                           }).ToList();

                report.ReportDataSource = data;
                #endregion

                #region  指定是哪個 RDLC
                Type reportResourceNamespace = typeof(P11_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P11_Print.rdlc";

                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
                {
                    // this.ShowException(result);
                    return false;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                };
                frm.Show();
                #endregion
            }

            return true;
        }

        /// <inheritdoc/>
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

            DBProxy.Current.OpenConnection(null, out SqlConnection conn);

            try
            {
                DualResult result2 = DBProxy.Current.ExecuteByConn(conn, sb.ToString());
                if (!result2)
                {
                    MyUtility.Msg.ShowException(null, result2);
                    return;
                }

                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
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

        private void TxtOrderID_Validating(object sender, EventArgs e)
        {
            if (this.txtOrderID.Text == this.txtOrderID.OldValue)
            {
                return;
            }

            this.CurrentMaintain["orderid"] = this.txtOrderID.Text;
            this.displayPOID.Text = string.Empty;
            string sqlcmd = $@"
select orders.poid 
from dbo.orders WITH (NOLOCK) 
left join dbo.Factory on orders.FtyGroup = Factory.ID 
where orders.id='{this.CurrentMaintain["orderid"]}' and Category !='A'
and Factory.mdivisionid = '{Env.User.Keyword}'
";
            this.poid = MyUtility.GetValue.Lookup(sqlcmd);
            if (!MyUtility.Check.Empty(this.txtOrderID.Text))
            {
                if (MyUtility.Check.Empty(this.poid))
                {
                    this.CurrentMaintain["cutplanid"] = string.Empty;
                    this.txtRequest.Text = string.Empty;
                    this.txtOrderID.Text = string.Empty;
                    this.displayCustCD.Text = string.Empty;
                    MyUtility.Msg.WarningBox("Can't found data");
                    return;
                }

                this.displayPOID.Text = this.poid;
                this.displayCustCD.Text = MyUtility.GetValue.Lookup(string.Format("select CustCDID from dbo.Orders WITH (NOLOCK) where id='{0}'", this.txtOrderID.Text));

                // CurrentMaintain["orderid"] = this.poid;
            }
        }

        private void TxtOrderID_Validated(object sender, EventArgs e) // 若order ID有變，重新撈取資料庫。
        {
            if (this.txtOrderID.Text == this.txtOrderID.OldValue)
            {
                return;
            }

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
            this.Matrix_Reload();
            this.detailgridbs.Position = 0;
            this.detailgrid.Focus();
            this.detailgrid.CurrentCell = this.detailgrid[10, 0];
            this.detailgrid.BeginEdit(true);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            string querySql = string.Format(
                @"
select '' FTYGroup
union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out DataTable queryDT);
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

            // StockType 必須保留，否則 BalanceQty 會出問題
            string sqlcmd = string.Format(
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
order by b.ID, b.seq1, b.seq2", Env.User.Keyword,
                this.poid,
                0);
            DBProxy.Current.Select(null, sqlcmd, out DataTable subData);
            if (subData.Rows.Count == 0)
            {
                this.txtOrderID.Text = string.Empty;
                return Ict.Result.F("No PO Data !");
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

                if (this.GetSubDetailDatas(ndr, out DataTable subDetails))
                {
                    sqlcmd = $@"
select  a.SizeCode
        , b.Id
        , b.Issue_DetailUkey
        , isnull(b.Qty,0) QTY 
from dbo.Order_SizeCode a WITH (NOLOCK) 
left join dbo.Issue_Size b WITH (NOLOCK) on b.SizeCode = a.SizeCode 
                                            and b.id = '{this.CurrentMaintain["id"]}'
                                            --and b.Issue_DetailUkey = {ndr["ukey"]}
where   a.id = '{this.poid}' 
order by Seq ";
                    DBProxy.Current.Select(null, sqlcmd, out DataTable sizeRange);
                    if (sizeRange == null)
                    {
                        continue;
                    }

                    foreach (DataRow drr in sizeRange.Rows)
                    {
                        subDetails.ImportRowAdded(drr);
                    }
                }
            }

            return Ict.Result.True;
        }

        private void CheckByCombo_CheckedChanged(object sender, EventArgs e)
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

                DBProxy.Current.Select(null, sql, out DataTable sizecodeDt);

                foreach (DataRow dr in this.DetailDatas)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        this.GetSubDetailDatas(dr, out DataTable subDt);
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

        private void BtnPrintKanbanCard_Click(object sender, EventArgs e)
        {
            var frm = new P11_PrintKarbanCard();
            frm.ShowDialog(this);
            this.RenewData();
        }

        /// <summary>
        ///  AutoWH ACC WebAPI for Vstrong
        /// </summary>
        private void SentToVstrong_AutoWH_ACC(bool isConfirmed)
        {
            // AutoWHACC WebAPI for Vstrong
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                DataTable dtDetail = new DataTable();
                string sqlGetData = string.Empty;
                sqlGetData = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = i.Type
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' ,po3.SuppColor,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[SizeCode] = po3.SizeSpec
,[StockType] = i2.StockType
,[Qty] = i2.Qty
,[StockPOID] = po3.StockPOID
,[StockSeq1] = po3.StockSeq1
,[StockSeq2] = po3.StockSeq2
,[Ukey] = i2.ukey
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,CmdTime = GetDate()
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.Id=i.Id
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
left join PO_Supp_Detail po3 on po3.ID = i2.POID
	and po3.SEQ1 = i2.Seq1 and po3.SEQ2 = i2.Seq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
where i.Type = 'B'
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='A'
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

                Task.Run(() => new Vstrong_AutoWHAccessory().SentIssue_DetailToVstrongAutoWHAccessory(dtDetail))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}
