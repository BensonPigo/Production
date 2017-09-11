﻿using System;
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
        StringBuilder sbSizecode, sbSizecode2, strsbIssueBreakDown;
       // StringBuilder sbIssueBreakDown;
        DataTable dtSizeCode = null, dtIssueBreakDown = null;
        DataRow dr;
        string poid = "";
        Boolean Ismatrix_Reload=true; //是否需要重新抓取資料庫資料

        P11_Detail subform = new P11_Detail();
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.gridicon.Location = new System.Drawing.Point(891, 128); //此gridcon位置會跑掉，需強制設定gridcon位置        
            this.gridicon.Anchor = AnchorStyles.Right;

            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            //Issue此為PMS自行建立的資料，MDivisionID皆會有寫入值

            WorkAlias = "Issue";                        // PK: ID
            GridAlias = "Issue_detail";           // PK: ID+UKey
            SubGridAlias = "Issue_size";          // PK: ID+Issue_DetailUkey+SizeCode

            KeyField1 = "ID"; //Issue PK
            KeyField2 = "ID"; // Summary FK

            //SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            SubDetailKeyField1 = "id,Ukey";    // second PK
            SubDetailKeyField2 = "id,Issue_DetailUkey"; // third FK
            //SubDetailKeyField1 = "Ukey";    // second PK
            //SubDetailKeyField2 = "Issue_SummaryUkey"; // third FK

            DoSubForm = subform;
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
                DoSubForm.IsSupportDelete = false;
                DoSubForm.IsSupportNew = false;

                subform.master = CurrentMaintain;
                subform.combo = checkByCombo.Checked;
                #endregion 
                #region keep SubDt
                /*
                 * 如果要實現 Reject 第三層
                 * 必須先 keep 原始資料
                 */
                Dictionary<DataRow, DataTable> originSub = new Dictionary<DataRow, DataTable>();
                foreach (DataRow dr in DetailDatas)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        DataTable subDt;
                        GetSubDetailDatas(dr, out subDt);
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
                GetSubDetailDatas(out FinalSubDt);
                if (!subform.isSave)
                {
                    /*
                     * 第三層做 undo 則實現 Reject
                     * 將進入第三層前 keep 的資料，重新塞回第三層
                     */
                    foreach (DataRow dr in DetailDatas)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            DataTable originDt = originSub[dr];
                            DataTable unchangeSubDt;
                            GetSubDetailDatas(dr, out unchangeSubDt);
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
                foreach (DataRow dr in DetailDatas)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        GetSubDetailDatas(dr, out FinalSubDt);
                        dr["output"] = string.Join(", ",
                            FinalSubDt.AsEnumerable()
                                 .Where(row => !MyUtility.Check.Empty(row["Qty"]))
                                 .Select(row => row["SizeCode"].ToString() + "*" + Convert.ToDecimal(row["qty"]).ToString("0.00"))

                        );

                        dr["qty"] = Math.Round(FinalSubDt.AsEnumerable()
                                                    .Where(row => !MyUtility.Check.Empty(row["Qty"]))
                                                    .Sum(row => Convert.ToDouble(row["Qty"].ToString()))
                                                    , 2);
                    }
                }        
                #endregion
            };
            #endregion
            //DoSubForm
            #region -- Seq 右鍵開窗 --

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable bulkItems;
                    string sqlcmd = string.Format(@"
select  a.*
        , FabricType = CASE b.FabricType 
                            WHEN 'A' THEN 'Accessory' 
                            WHEN 'F' THEN 'Fabric'  
                            WHEN 'O' THEN 'Other' 
                       END
        , b.SCIRefno
        , f.MtlTypeID
        , m.IssueType
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.seq2) seq
        , b.Colorid
        , b.SizeSpec
        , b.UsedQty
        , b.SizeUnit
        , dbo.Getlocation(a.ukey) [location]
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0)[description]
        , b.StockUnit
        , [accu_issue] = isnull(( select sum(Issue_Detail.qty) 
                                  from dbo.issue WITH (NOLOCK) 
                                  inner join dbo.Issue_Detail WITH (NOLOCK) on Issue_Detail.id = Issue.Id 
                                  where Issue.type = 'B' 
                                        and Issue.Status = 'Confirmed' 
                                        and issue.id != a.POId 
                                        and Issue_Detail.poid = a.poid 
                                        and Issue_Detail.seq1 = a.seq1 
                                        and Issue_Detail.seq2 = a.seq2
                                        and Issue_Detail.roll = a.roll 
                                        and Issue_Detail.stocktype = a.stocktype),0.00) 
        , balanceqty = isnull((  select fi.inqty - fi.outqty + fi.adjustqty 
                                 from dbo.ftyinventory FI WITH (NOLOCK) 
                                 where a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
                                         and a.roll = fi.roll and a.stocktype = fi.stocktype)
                             ,0.00)
from dbo.ftyinventory a WITH (NOLOCK) 
inner join dbo.po_supp_detail b WITH (NOLOCK) on b.id=a.POID and b.seq1=a.seq1 and b.seq2 = a.Seq2
inner join Fabric f WITH (NOLOCK) on f.SCIRefno = b.SCIRefno
inner join MtlType m WITH (NOLOCK) on m.ID = f.MtlTypeID
where   lock=0 
        and inqty-outqty+adjustqty > 0 
        and poid='{1}' 
        and stocktype='B'
        and b.FabricType='A'
        and m.IssueType='Sewing' 
order by poid,seq1,seq2", Sci.Env.User.Keyword, CurrentDetailData["poid"]);
                    IList<DataRow> x;
                    DualResult result2;
                    if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out bulkItems)))
                    {
                        ShowErr(sqlcmd, result2);
                        return;
                    }

                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(bulkItems
                        //, "Type,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,inqty,outqty,adjustqty,ukey"
                            , "FabricType,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,inqty,outqty,adjustqty"
                            , "4,14,10,10,13,4,3,6,6,6,10", CurrentDetailData["seq"].ToString()
                            , "FabricType,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,In Qty,Out Qty,Adjust Qty,Ukey");
                    selepoitem.Width = 1024;

                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = selepoitem.GetSelecteds();
                    CurrentDetailData["seq"] = x[0]["seq"];
                    CurrentDetailData["seq1"] = x[0]["seq1"];
                    CurrentDetailData["seq2"] = x[0]["seq2"];
                    //CurrentDetailData["mdivisionid"] = x[0]["mdivisionid"];
                    CurrentDetailData["stocktype"] = x[0]["stocktype"];
                    CurrentDetailData["ftyinventoryukey"] = x[0]["ukey"];
                    CurrentDetailData["Colorid"] = x[0]["Colorid"];
                    CurrentDetailData["SizeSpec"] = x[0]["SizeSpec"];
                    CurrentDetailData["UsedQty"] = x[0]["UsedQty"];
                    CurrentDetailData["SizeUnit"] = x[0]["SizeUnit"];
                    CurrentDetailData["location"] = x[0]["location"];
                    CurrentDetailData["description"] = x[0]["description"];
                    CurrentDetailData["accu_issue"] = x[0]["accu_issue"];
                    CurrentDetailData["balanceqty"] = x[0]["balanceqty"];
                    CurrentDetailData["StockUnit"] = x[0]["StockUnit"];
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["seq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        CurrentDetailData["seq"] = "";
                        CurrentDetailData["seq1"] = "";
                        CurrentDetailData["seq2"] = "";
                        //CurrentDetailData["mdivisionid"] = "";
                        CurrentDetailData["stocktype"] = "";
                        CurrentDetailData["ftyinventoryukey"] = 0;
                    }
                    else
                    {
                        //check Seq Length
                        string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (seq.Length < 2)
                        {
                            
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }

                        if (!MyUtility.Check.Seek(string.Format(@"
select  a.*
        , FabricType = CASE b.FabricType 
                            WHEN 'A' THEN 'Accessory' 
                            WHEN 'F' THEN 'Fabric'  
                            WHEN 'O' THEN 'Other' 
                       END
        , b.SCIRefno
        , f.MtlTypeID
        , m.IssueType
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.seq2) seq
        , b.Colorid
        , b.SizeSpec
        , b.UsedQty
        , b.SizeUnit
        , dbo.Getlocation(a.ukey) [location]
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0)[description]
        , b.StockUnit
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
from dbo.ftyinventory a WITH (NOLOCK) 
inner join dbo.po_supp_detail b WITH (NOLOCK) on b.id = a.POID 
                                                 and b.seq1 = a.seq1 
                                                 and b.seq2 = a.Seq2
inner join Fabric f WITH (NOLOCK) on f.SCIRefno = b.SCIRefno
inner join MtlType m WITH (NOLOCK) on m.ID = f.MtlTypeID
where   poid = '{0}' 
        and a.seq1 = '{1}' 
        and a.seq2 = '{2}' 
        and lock = 0 
        and inqty - outqty + adjustqty > 0  
        and stocktype = 'B' 
        and b.FabricType='A'
        and m.IssueType='Sewing' "
                            , CurrentDetailData["poid"], seq[0], seq[1]), out dr, null))
                        {
                           e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }
                        else
                        {
                            CurrentDetailData["seq"] = seq[0] + " " + seq[1];
                            CurrentDetailData["seq1"] = seq[0];
                            CurrentDetailData["seq2"] = seq[1];
                            //CurrentDetailData["mdivisionid"] = dr["mdivisionid"];
                            CurrentDetailData["stocktype"] = dr["stocktype"];
                            CurrentDetailData["ftyinventoryukey"] = dr["ukey"];
                            CurrentDetailData["Colorid"] = dr["Colorid"];
                            CurrentDetailData["SizeSpec"] = dr["SizeSpec"];
                            CurrentDetailData["UsedQty"] = dr["UsedQty"];
                            CurrentDetailData["SizeUnit"] = dr["SizeUnit"];
                            CurrentDetailData["location"] = dr["location"];
                            CurrentDetailData["description"] = dr["description"];
                            CurrentDetailData["accu_issue"] = dr["accu_issue"];
                            CurrentDetailData["balanceqty"] = dr["balanceqty"];
                            CurrentDetailData["StockUnit"] = dr["StockUnit"];
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗

            #region -- 欄位設定 --
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts2)  //1
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //2
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)  //3
            .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)  //4
            .Numeric("usedqty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)    //5
            .Text("SizeUnit", header: "SizeUnit", width: Widths.AnsiChars(6), iseditingreadonly: true)  //6          
            .Text("location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true)  //7
            .Numeric("accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //8
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)    //5
            .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)  //7
            .Text("output", header: "Output", width: Widths.AnsiChars(20), iseditingreadonly: true, settings: ts) //9
            .Numeric("balanceqty", header: "Balance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //11
            ; 
            #endregion 欄位設定

            #region 可編輯欄位變色
            detailgrid.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["output"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion 可編輯欄位變色
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? "" : e.Master["cutplanID"].ToString();
            Ismatrix_Reload = true;
            this.DetailSelectCommand = string.Format(@"  
select  a.Id
        , isnull(a.FtyInventoryUkey,0) [FtyInventoryUkey]
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.seq2) as seq
        , a.StockType
        , a.Qty
        , p.Colorid
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
                                where   Issue_Size.Issue_DetailUkey = a.ukey 
                             ) v for xml path(''))
                            ,'') 
        , a.Ukey
        , balanceqty = isnull((fi.inqty - fi.outqty + fi.adjustqty),0.00)
from dbo.Issue_Detail a WITH (NOLOCK) 
left join dbo.po_supp_detail p WITH (NOLOCK) on p.id  = a.poid 
                                                and p.seq1= a.seq1 
                                                and p.seq2 =a.seq2
left join dbo.FtyInventory FI on    a.Poid = Fi.Poid 
                                    and a.Seq1 = fi.seq1 
                                    and a.seq2 = fi.seq2 
                                    and a.roll = fi.roll 
                                    and a.stocktype = fi.stocktype
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "B";
            CurrentMaintain["issuedate"] = DateTime.Now;
           //leo等確認完工廠有此欄位後開啟 
            CurrentMaintain["combo"] = 0;
            dtIssueBreakDown = null;
            gridIssueBreakDown.DataSource = null;
            txtOrderID.IsSupportEditMode = true;
            txtRequest.IsSupportEditMode = true;
            txtOrderID.ReadOnly = false;
            txtRequest.ReadOnly = false;
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
            txtOrderID.IsSupportEditMode = false;
            txtRequest.IsSupportEditMode = false;
            txtOrderID.ReadOnly = true;
            txtRequest.ReadOnly = true;
            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            DataTable result = null;
            StringBuilder warningmsg = new StringBuilder();

            #region 表頭 必輸檢查
            if (MyUtility.Check.Empty(txtOrderID.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Request# > or < Order ID >  can't be empty!", "Warning");
                txtOrderID.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateIssueDate.Focus();
                return false;
            }
            #endregion 必輸檢查
            #region 表身不可出現 Qty = 0 的資料
            foreach (DataRow dr in DetailDatas)
            {
                if (dr["Qty"].ToString().Empty() || dr["Qty"].EqualDecimal(0))
                {
                    dr.Delete();
                }
            }
            #endregion 
            #region 表身 必輸檢查
            foreach (DataRow dr in DetailDatas)
            {
                if (dr["Seq"].ToString().Empty())
                {
                    MyUtility.Msg.WarningBox("Seq can't be empty.");
                    return false;
                }
            }
            #endregion

            foreach (DataRow Checkduplicate in DetailDatas)
            {
                DataRow[] findrow = DetailDatas.AsEnumerable().Where(row => row["poid"].EqualString(Checkduplicate["poid"].ToString()) && row["seq1"].EqualString(Checkduplicate["seq1"])
                                                                          && row["seq2"].EqualString(Checkduplicate["seq2"].ToString())).ToArray();
                if (findrow.Length > 1)
                {
                    MyUtility.Msg.WarningBox(string.Format(@"SP#: {0} Seq#: {1}-{2} duplicate, SP# and Seq# can't duplicate", Checkduplicate["poid"], Checkduplicate["seq1"], Checkduplicate["seq2"]));
                    return false;                        
                }
            }         

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IS", "Issue", (DateTime)CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            if (dtSizeCode != null && dtSizeCode.Rows.Count != 0)
            {
                if (checkByCombo.Checked == false)
                {
                    foreach (DataRow data in dtIssueBreakDown.ToList())
                    {
                        if (data.ItemArray[0].ToString() != txtOrderID.Text)
                            dtIssueBreakDown.Rows.Remove(data);                                          
                    }
                }                
                string sqlcmd;
                sqlcmd = string.Format(@";delete from dbo.issue_breakdown where id='{0}'
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
;delete from dbo.issue_breakdown where id='{0}' and qty = 0; ", CurrentMaintain["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1));//.Replace("[", "[_")

                string aaa = sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1).Replace("[", "").Replace("]", "");//.Replace("[", "").Replace("]", "")



                //            sqlcmd = string.Format(string.Format(@";WITH UNPIVOT_1
                //AS
                //(
                //SELECT * FROM #tmp
                //UNPIVOT
                //(
                //QTY
                //FOR SIZECODE IN ({1})
                //)
                //AS PVT
                //)SELECT * FROM UNPIVOT_1;", Master["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)));



                ProcessWithDatatable2(dtIssueBreakDown, "OrderID,Article," + aaa
                    , sqlcmd, out result, "#tmp");
                //MyUtility.Msg.InfoBox("Save completed!!");
            }


            DataTable subDT;
            
            //將需新增的資料狀態更改為新增
            foreach (DataRow dr in DetailDatas)
            {
                GetSubDetailDatas(dr, out subDT);
                foreach (DataRow temp in subDT.Rows)
                {
                    if (temp["isvirtual"].ToString() == "1" && Convert.ToDecimal(temp["QTY"].ToString()) > 0)
                    {
                        temp.AcceptChanges();
                        temp.SetAdded();
                    }
                }
            }

            //刪除第三層qty為0的資料
            foreach (DataRow dr in DetailDatas)
            {
                if (GetSubDetailDatas(dr, out subDT))
                {
                    foreach (DataRow dr2 in subDT.ToList())
                    {
                        if (dr2.RowState != DataRowState.Deleted && Convert.ToDecimal(dr2["QTY"].ToString()) == 0 && dr2.RowState != DataRowState.Modified)
                        {
                            subDT.Rows.Remove(dr2);
                        }
                    }
                }
            }

            foreach (DataRow dr in DetailDatas)
            {
                if (GetSubDetailDatas(dr, out subDT))
                {
                    foreach (DataRow dr2 in subDT.Rows)
                    {
                        if (Convert.ToDecimal(dr2["QTY"].ToString()) == 0 && dr2.RowState == DataRowState.Modified)
                        {
                            dr2.Delete();
                        }
                    }
                }
            }
       

            return base.ClickSaveBefore();
        }

        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            DataTable dt;
            foreach (DataRow dr in DetailDatas)
            {
                if (GetSubDetailDatas(dr, out dt))
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
            //檢查是否有勾選Combo，處理傳入AutoPick資料篩選
            if (!checkByCombo.Checked && dtIssueBreakDown!=null)
            {
                foreach (DataRow tempRow in dtIssueBreakDown.Rows)
                {
                    if (tempRow["OrderID"].ToString() != txtOrderID.Text.ToString())
                    {
                        foreach (DataColumn tempColumn in dtIssueBreakDown.Columns)
                        {
                            if("Decimal"==tempRow[tempColumn].GetType().Name)                         
                                tempRow[tempColumn] = 0;
                        }
                    }
                }
            }

            var frm = new Sci.Production.Warehouse.P11_AutoPick(CurrentMaintain["id"].ToString(), this.poid, CurrentMaintain["cutplanid"].ToString(), txtOrderID.Text.ToString(), dtIssueBreakDown, sbSizecode, checkByCombo.Checked);
            DialogResult result = frm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                DataRow tmp;
                DataTable _detail, _subDetail;
                _detail = (DataTable)detailgridbs.DataSource;

                //刪除表身重新匯入
                foreach (DataRow del in DetailDatas)
                {
                    del.Delete();
                }

                //批次匯入
                foreach (KeyValuePair<DataRow, DataTable> item in frm.dictionaryDatas)
                {
                    tmp = item.Key;
                    if (tmp["selected"].ToString() == "1")
                    {
                        // 匯入Issue_detail layer
                        tmp["id"] = CurrentMaintain["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        _detail.ImportRow(tmp);

                        // 匯入Issue_Size layer
                        if (GetSubDetailDatas(_detail.Rows[_detail.Rows.Count - 1], out _subDetail))
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
                _detail.DefaultView.Sort = "seq";
            }
        }

        private void txtRequest_Validating(object sender, CancelEventArgs e)
        {
            if (txtRequest.Text == txtRequest.OldValue) return;
            //DBProxy.Current.Execute(null, string.Format("delete from dbo.issue_breakdown where id='{0}';", CurrentMaintain["id"].ToString()));
            CurrentMaintain["cutplanid"] = txtRequest.Text;
            txtOrderID.Text = "";
            CurrentMaintain["orderid"] = "";
            this.displayPOID.Text = "";
            this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.cutplan WITH (NOLOCK) where id='{0}' and mdivisionid = '{1}'", CurrentMaintain["cutplanid"], Sci.Env.User.Keyword));

            if (MyUtility.Check.Empty(txtRequest.Text))
            {
                dtIssueBreakDown = null;
                gridIssueBreakDown.DataSource = null;
                foreach (DataRow dr in DetailDatas)
                {
                    //刪除SubDetail資料
                    ((DataTable)detailgridbs.DataSource).Rows.Remove(dr);
                    dr.Delete();
                }
                return;
            }
            if (MyUtility.Check.Empty(this.poid))
            {
               
                CurrentMaintain["cutplanid"] = "";
                txtRequest.Text = "";
                dtIssueBreakDown = null;
                gridIssueBreakDown.DataSource = null;
                foreach (DataRow dr in DetailDatas)
                {
                    //刪除SubDetail資料
                    ((DataTable)detailgridbs.DataSource).Rows.Remove(dr);
                    dr.Delete();
                } 
                MyUtility.Msg.WarningBox("Can't found data");
                return;
            }
            //getpoid();
            this.displayPOID.Text = this.poid;
            CurrentMaintain["orderid"] = this.poid;
            Detail_Reload();
            Ismatrix_Reload = true;
            matrix_Reload();
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataTable dt;
            if (!(CurrentMaintain == null))
            {
                displayCutCell.Text = MyUtility.GetValue.Lookup(string.Format("select CutCellID from dbo.cutplan WITH (NOLOCK) where id='{0}'", CurrentMaintain["cutplanid"]));
                displayLineNo.Text = MyUtility.GetValue.Lookup(string.Format(@"select t.SewLine+','  from (select distinct o.SewLine 
from dbo.Issue_detail a WITH (NOLOCK) inner join dbo.orders o WITH (NOLOCK) on a.Poid = o.POID where a.id='{0}' and o.sewline !='') t for xml path('')", CurrentMaintain["id"]));

                DBProxy.Current.Select(null, string.Format(@";with cte as
(Select WorkOrder.FabricCombo,Cutplan_Detail.CutNo from Cutplan_Detail WITH (NOLOCK) inner join dbo.workorder WITH (NOLOCK) on WorkOrder.Ukey = Cutplan_Detail.WorkorderUkey 
where Cutplan_Detail.ID='{0}' )
select distinct FabricCombo ,(select convert(varchar,CutNo)+',' 
from (select CutNo from cte where cte.FabricCombo = a.FabricCombo )t order by CutNo for xml path('')) cutnos from cte a
", CurrentMaintain["cutplanid"]), out dt);
                editCutNo.Text = String.Join(" / ", dt.AsEnumerable().Select(row => row["FabricCombo"].ToString() + "-" + row["cutnos"].ToString()));
                //ebArticle.Text = MyUtility.GetValue.Lookup(string.Format(@"select t.article+','  from (select distinct article 
                //from dbo.cutplan_detail  where id='{0}') t for xml path('')", CurrentMaintain["cutplanid"]));


                #region -- Status Label --

                label25.Text = CurrentMaintain["status"].ToString();

                #endregion Status Label
                #region -- POID
                this.getpoid();
                this.displayPOID.Text = this.poid;
                #endregion

                #region -- matrix breakdown
                DualResult result;
                if (!(result = matrix_Reload()))
                {
                    ShowErr(result);
                }
                #endregion
            }
        }

        private DualResult matrix_Reload()
        {
            if (EditMode == true && Ismatrix_Reload == false)
                return Result.True;

            Ismatrix_Reload = false;
            string sqlcmd;
            StringBuilder sbIssueBreakDown;
            DualResult result;

            string OrderID = txtOrderID.Text;

            sqlcmd = string.Format(@"select sizecode from dbo.order_sizecode WITH (NOLOCK) 
where id = (select poid from dbo.orders WITH (NOLOCK) where id='{0}') order by seq", OrderID);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out dtSizeCode)))
            {
                ShowErr(sqlcmd, result);
                return Result.True;
            }
            if (dtSizeCode.Rows.Count == 0)
            {
                //MyUtility.Msg.WarningBox(string.Format("Becuase there no sizecode data belong this OrderID {0} , can't show data!!", CurrentDataRow["orderid"]));               
                dtIssueBreakDown = null;
                gridIssueBreakDown.DataSource = null;
                return Result.True;
            }


            sbSizecode = new StringBuilder();
            sbSizecode2 = new StringBuilder();
            sbSizecode.Clear();
            sbSizecode2.Clear();
            for (int i = 0; i < dtSizeCode.Rows.Count; i++)
            {
                sbSizecode.Append(string.Format(@"[{0}],", dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
                sbSizecode2.Append(string.Format(@"{0},", dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
            }
            sbIssueBreakDown = new StringBuilder();
            sbIssueBreakDown.Append(string.Format(@";with Bdown as 
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
            order by [OrderID],[Article]", OrderID, CurrentMaintain["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)));//.Replace("[", "[_")
            strsbIssueBreakDown = sbIssueBreakDown;//多加一個變數來接 不改變欄位
            if (!(result = DBProxy.Current.Select(null, sbIssueBreakDown.ToString(), out dtIssueBreakDown)))
            {
                ShowErr(sqlcmd, result);
                return Result.True;
            }

            gridIssueBreakDown.AutoGenerateColumns = true;
            gridIssueBreakDownBS.DataSource = dtIssueBreakDown;
            gridIssueBreakDown.DataSource = gridIssueBreakDownBS;
            gridIssueBreakDown.IsEditingReadOnly = true;
            gridIssueBreakDown.ReadOnly = true;

            checkByCombo_CheckedChanged(null, null);

            return Result.True;
        }

        private void getpoid()
        {
            CurrentMaintain["cutplanid"] = txtRequest.Text;
            this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.cutplan WITH (NOLOCK) where id='{0}' and mdivisionid = '{1}'", CurrentMaintain["cutplanid"], Sci.Env.User.Keyword));
            if (MyUtility.Check.Empty(this.poid))
            {
                this.poid = MyUtility.GetValue.Lookup(string.Format("select orders.poid from dbo.orders WITH (NOLOCK) left join dbo.Factory on orders.FtyGroup=Factory.ID where orders.id='{0}' and Factory.mdivisionid = '{1}'", CurrentMaintain["orderid"], Sci.Env.User.Keyword));
            }

        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            //CurrentDetailData["mdivisionid"] = Sci.Env.User.Keyword;
            CurrentDetailData["poid"] = this.poid;
            DataTable sizeRange, subDetails;
            if (GetSubDetailDatas(CurrentDetailData, out subDetails))
            {
                DBProxy.Current.Select(null, string.Format(@"select a.SizeCode,'{1}' AS Id,0.00 AS QTY
from dbo.Order_SizeCode a WITH (NOLOCK) 
where a.id='{0}' order by Seq", this.poid, CurrentMaintain["id"]), out sizeRange);
                foreach (DataRow dr in sizeRange.Rows)
                {
                    dr.AcceptChanges();
                    dr.SetAdded();
                    subDetails.ImportRow(dr);
                }
            }

        }

        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "" : e.Detail["ID"].ToString();
            string ukey = (e.Detail == null || MyUtility.Check.Empty(e.Detail["ukey"])) ? "0" : e.Detail["ukey"].ToString();
            this.SubDetailSelectCommand = string.Format(@"
select  a.SizeCode
        , b.Id
        , '{2}' AS Issue_DetailUkey
        , isnull(b.Qty,0) QTY
        , IIF(b.Qty IS NULL , 1 ,0) isvirtual

from  dbo.Issue_Size b WITH (NOLOCK) 
inner join dbo.Order_SizeCode a WITH (NOLOCK) on b.SizeCode = a.SizeCode
outer apply(select poid from dbo.cutplan WITH (NOLOCK) where id='{0}' and mdivisionid = '{3}')poid1
outer apply(select orders.poid from dbo.orders WITH (NOLOCK) left join dbo.Factory on orders.FtyGroup=Factory.ID where orders.id='{4}' and Factory.mdivisionid = '{3}')poid2
where a.id= iif(isnull(poid1.POID,'')='',poid2.POID,poid1.poid)
and b.id = '{1}' and b.Issue_DetailUkey = {2}
order by Seq"
            , CurrentMaintain["cutplanid"].ToString(), masterID, ukey, Sci.Env.User.Keyword, CurrentMaintain["orderid"].ToString());
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return;
            }
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            string sqlupd2_FIO = "";
            StringBuilder sqlupd2_B = new StringBuilder();

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"
Select  d.poid  
        , d.seq1
        , d.seq2
        , d.Roll
        , d.Qty
        , isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.POID = f.POID  
                                            and D.StockType = F.StockType
                                            and d.Roll = f.Roll 
                                            and d.Seq1 =f.Seq1 
                                            and d.Seq2 = f.Seq2
where   f.lock = 1 
        and d.Id = '{0}'", CurrentMaintain["id"]);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"
Select  d.poid
        , d.seq1
        , d.seq2
        , d.Roll,d.Qty
        , isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on d.POID = f.POID  
                                          and D.StockType = F.StockType
                                          and d.Roll = f.Roll 
                                          and d.Seq1 = f.Seq1 
                                          and d.Seq2 = f.Seq2
where   (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) 
        and d.Id = '{0}'", CurrentMaintain["id"]);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"
update Issue 
set status = 'Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            #region -- 更新mdivisionpodetail B倉數 --
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
                    if (!(result = MyUtility.Tool.ProcessWithDatatable
                        (datacheck, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
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
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
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

            var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = - (m.Field<decimal>("qty")),
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
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
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
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject
                        (bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
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

        private void btnBOA_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P11_BOA(CurrentMaintain["id"].ToString(), this.poid, CurrentMaintain["cutplanid"].ToString(),txtOrderID.Text.ToString());
            frm.ShowDialog(this);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
            //刪除表身重新匯入
            foreach (DataRow del in DetailDatas)
            {
                if (del["qty"].EqualDecimal(0)) del.Delete();              
            }
        }

        private void btnBreakDown_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P11_IssueBreakDown(CurrentMaintain, dtIssueBreakDown, dtSizeCode);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }

        protected override bool ClickPrint()
        {
            label25.Text = CurrentMaintain["status"].ToString();
            if (label25.Text.ToUpper() != "CONFIRMED")
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
            #region Title
            DataTable dt;
            DBProxy.Current.Select("", @"
select NameEN 
from MDivision 
where id = @MDivision", pars, out dt);
            string RptTitle = dt.Rows[0]["NameEN"].ToString();
           
            #endregion
            #region SP
            DataTable dtsp;
            string poID;
            DBProxy.Current.Select("",
                @"select (select poid+',' from 
             (select distinct cd.POID from Cutplan_Detail cd WITH (NOLOCK) where id =(select CutplanID from dbo.Issue WITH (NOLOCK) where id='@id')  ) t
			  for xml path('')) as [poid]", pars, out dtsp);
            if (dtsp.Rows.Count == 0)
                poID = "";
            else
                poID = dtsp.Rows[0]["POID"].ToString();

            //report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("POID", poID));
            #endregion
            #region SizeCode
            DualResult result;
            DataTable dtSizecode;
            string sqlcmd1 = string.Format(@"select distinct sizecode
	                    from dbo.Issue_Size WITH (NOLOCK) 
	                    where id = @ID order by sizecode");
            string sizecodes = "";
            result = DBProxy.Current.Select("", sqlcmd1, pars, out dtSizecode);

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

            string sqlcmd = string.Format(@"
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
            result = DBProxy.Current.Select("", sqlcmd, pars, out dtseq);

            if (!result)
            {
                ShowErr(result);
                return true;
            }

            if (dtseq == null || dtseq.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dtseq");
                return false;
            }

            dtseq.Columns.Remove(dtseq.Columns["Issue_DetailUkey"]);
            string SEQ = dtseq.Rows[0]["SEQ"].ToString();
            //string tQty = dtseq.Rows[0]["tQTY"].ToString();
            //report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SEQ", SEQ));
            //report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("tQTY", tQty));
            #endregion
            #region LineNo
            DataTable dtlineno;
            string cLineNo;
            result = DBProxy.Current.Select("",
                @"select o.sewline from dbo.Orders o WITH (NOLOCK) 
                    where id in (select distinct poid from issue_detail WITH (NOLOCK) where id=@ID ) ", pars, out dtlineno);
            if (!result)
            {
                ShowErr(result);
                return true;
            }

            if (dtlineno == null || dtlineno.Rows.Count == 0)
                cLineNo = "";
            else
                cLineNo = dtlineno.Rows[0]["sewline"].ToString();

            //report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("sewline", cLineNo));
            #endregion
            #region CellNo
            DataTable dtcutcell;
            string cCellNo;
            result = DBProxy.Current.Select("",
                @"select    
             b.CutCellID 
            from dbo.Issue as a WITH (NOLOCK) 
             inner join dbo.cutplan as b WITH (NOLOCK) on b.id = a.cutplanid
            where b.id = a.CutplanID
            ", pars, out dtcutcell);
            if (!result)
            {
                ShowErr(result);
                return true;
            }

            if (dtcutcell == null || dtcutcell.Rows.Count == 0)
                cCellNo = "";
            else
                cCellNo = dtcutcell.Rows[0]["CutCellID"].ToString();

            //report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CutCellID", cCellNo));
            #endregion 

         
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
            //取得size欄位名稱
            for (int i = 6; i < dtseq.Columns.Count; i++)
            {
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("size" +( i  - 5).ToString(), dtseq.Columns[i].ColumnName));
            }


            //將size的欄位加到10個
            for (int i = dtseq.Columns.Count - 1; i < 16; i++)
            {
                dtseq.Columns.Add("#!#!##"+i.ToString());
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
                                           size10 = row1[15].ToString().Trim()
                                       }).ToList();

            report.ReportDataSource = data;

            #region  指定是哪個 RDLC
            Type ReportResourceNamespace = typeof(P11_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P11_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }
            
            report.ReportResource = reportresource;


            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();
            #endregion

            //string xlt = @"Warehouse_P11.xltx";
            //SaveXltReportCls xl = new SaveXltReportCls(xlt);
            //xl.BoOpenFile = true;

            //xl.DicDatas.Add("##RptTitle", RptTitle);
            //xl.DicDatas.Add("##ID", id);
            //xl.DicDatas.Add("##cutplanid", request);
            //xl.DicDatas.Add("##issuedate", issuedate);
            //xl.DicDatas.Add("##remark", remark);
            //xl.DicDatas.Add("##cCutNo", cutno);
            //xl.DicDatas.Add("##cLineNo", LineNo);
            //xl.DicDatas.Add("##OrderID", OrderID);
            //xl.DicDatas.Add("##cCellNo", CellNo);
            //SaveXltReportCls.XltRptTable xlTable = new SaveXltReportCls.XltRptTable(dtseq);
            //int allColumns = dtseq.Columns.Count;
            //int sizeColumns = dtSizecode.Rows.Count;
            //Microsoft.Office.Interop.Excel.Worksheet wks = xl.ExcelApp.ActiveSheet;
            //string cc = MyUtility.Excel.ConvertNumericToExcelColumn(dtseq.Columns.Count);
            //// 合併儲存格
            //wks.get_Range("G9", cc + "9").Merge(false);
            //wks.Cells[9, 7] = "SIZE";
            ////框線
            //wks.Range["G9", cc + "10"].Borders.LineStyle = 1;
            ////置中
            //wks.get_Range("G9", cc + "9").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //for (int i = 6; i < dtseq.Columns.Count; i++)
            //{
            //    wks.Cells[10, i+1] = dtseq.Columns[i].ColumnName;
            //}
            
            //xlTable.Borders.OnlyHeaderBorders = true;
            //xlTable.Borders.AllCellsBorders = true;
            //xlTable.ShowHeader = false;
            //xl.DicDatas.Add("##SEQ", xlTable);

            //xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P11"));
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
                if (MyUtility.Check.Empty(cols[i])) continue;
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
                if (i < cols.Length - 1) { sb.Append(","); }
            }
            sb.Append(")");

            System.Data.SqlClient.SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);

            try
            {
                DualResult result2 = DBProxy.Current.ExecuteByConn(conn, sb.ToString());
                if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }
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
                if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }

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
            if (txtOrderID.Text == txtOrderID.OldValue) return;
            CurrentMaintain["orderid"] = txtOrderID.Text;
            this.displayPOID.Text = "";
            this.poid = MyUtility.GetValue.Lookup(string.Format(@"
select orders.poid 
from dbo.orders WITH (NOLOCK) 
left join dbo.Factory on orders.FtyGroup = Factory.ID 
where   orders.id='{0}' 
        and Factory.mdivisionid = '{1}'", CurrentMaintain["orderid"], Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(txtOrderID.Text))
            {
                if (MyUtility.Check.Empty(this.poid))
                {
                    
                    CurrentMaintain["cutplanid"] = "";
                    txtRequest.Text = "";
                    txtOrderID.Text = "";
                    MyUtility.Msg.WarningBox("Can't found data");
                    return;
                }
                this.displayPOID.Text = this.poid;
                //CurrentMaintain["orderid"] = this.poid;    
            }
        }

        private void txtOrderID_Validated(object sender, EventArgs e) //若order ID有變，重新撈取資料庫。
        {
            if (txtOrderID.Text == txtOrderID.OldValue) return;
           // DBProxy.Current.Execute(null, string.Format("delete from dbo.issue_breakdown where id='{0}';", CurrentMaintain["id"].ToString()));
            CurrentMaintain["cutplanid"] = "";
            if (MyUtility.Check.Empty(this.poid))
            {
                dtIssueBreakDown = null;
                gridIssueBreakDown.DataSource = null;
                foreach (DataRow dr in DetailDatas)
                {
                    //刪除SubDetail資料
                    ((DataTable)detailgridbs.DataSource).Rows.Remove(dr);
                    dr.Delete();
                }
                return;
            }
            Detail_Reload();
            Ismatrix_Reload = true;
            matrix_Reload();
            detailgridbs.Position = 0;
            detailgrid.Focus();
            detailgrid.CurrentCell = detailgrid[10, 0];
            detailgrid.BeginEdit(true);
        }

        private DualResult Detail_Reload()
        {            
            foreach (DataRow dr in DetailDatas)
            {
                //刪除SubDetail資料
                ((DataTable)detailgridbs.DataSource).Rows.Remove(dr);
                dr.Delete();
            }

            DataTable subData;
            // StockType 必須保留，否則 BalanceQty 會出問題
            DBProxy.Current.Select(null, string.Format(@"
select  poid = b.ID
        , a.Ukey
        , b.Seq1
        , b.Seq2
        , concat (Ltrim (Rtrim (b.seq1)), ' ', b.seq2) seq
        , a.StockType
        , b.ColorID
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
            //將資料塞入表身
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
                if (GetSubDetailDatas(CurrentDetailData, out subDetails))
                {
                    DBProxy.Current.Select(null, string.Format(@"
select  a.SizeCode
        , b.Id
        , b.Issue_DetailUkey
        , isnull(b.Qty,0) QTY 
from dbo.Order_SizeCode a WITH (NOLOCK) 
left join dbo.Issue_Size b WITH (NOLOCK) on b.SizeCode = a.SizeCode 
                                            and b.id = '{1}' 
                                            --and b.Issue_DetailUkey = {2}
where   a.id = '{0}' 
order by Seq ", this.poid, CurrentMaintain["id"], CurrentDetailData["ukey"]), out sizeRange);
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
            if (dtIssueBreakDown == null) return;
            if (checkByCombo.Checked)
            {
                dtIssueBreakDown.DefaultView.RowFilter = string.Format("");
            }
            else
            {
                dtIssueBreakDown.DefaultView.RowFilter = string.Format("OrderID='{0}'", txtOrderID.Text);
            }

        }

        protected override void OnDetailGridRowChanged()
        {
        }
    }
}
