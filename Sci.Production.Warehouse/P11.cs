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
using Sci.Trade.Class.Commons;
using Sci.Win;
using Sci.Utility.Excel;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P11 : Sci.Win.Tems.Input8
    {
        StringBuilder sbSizecode, sbSizecode2, strsbIssueBreakDown;
        StringBuilder sbIssueBreakDown;
        DataTable dtSizeCode = null, dtIssueBreakDown = null;
        DataRow dr;
        string poid = "";
        Boolean Ismatrix_Reload; //是否需要重新抓取資料庫資料

        P11_Detail subform = new P11_Detail();
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Sci.Env.User.Keyword);

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
                DoSubForm.IsSupportDelete = false;
                DoSubForm.IsSupportNew = false;
                subform.parentData = this.CurrentDetailData;
                OpenSubDetailPage();
            };
            #endregion
            //DoSubForm
            #region -- Seq 右鍵開窗 --

            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings ts2 = new DataGridViewGeneratorMaskedTextColumnSettings();
            ts2.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable bulkItems;
                    string sqlcmd = string.Format(@"select a.*,
CASE b.FabricType WHEN 'A' THEN 'Accessory' WHEN 'F' THEN 'Fabric'  WHEN 'O' THEN 'Other' END AS FabricType
,b.SCIRefno,f.MtlTypeID,m.IssueType,left(a.seq1+'   ',3)+a.seq2 seq
from dbo.ftyinventory a inner join dbo.po_supp_detail b on b.id=a.POID and b.seq1=a.seq1 and b.seq2 = a.Seq2
inner join Fabric f on f.SCIRefno = b.SCIRefno
inner join MtlType m on m.ID = f.MtlTypeID
where lock=0 and inqty-outqty+adjustqty > 0 
and mdivisionid='{0}' and poid='{1}' --and stocktype='B'
and b.FabricType='A'
and m.IssueType='Sewing' order by poid,seq1,seq2", Sci.Env.User.Keyword, CurrentDetailData["poid"]);
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
                    CurrentDetailData["mdivisionid"] = x[0]["mdivisionid"];
                    CurrentDetailData["stocktype"] = x[0]["stocktype"];
                    CurrentDetailData["ftyinventoryukey"] = x[0]["ukey"];
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
                        CurrentDetailData["mdivisionid"] = "";
                        CurrentDetailData["stocktype"] = "";
                        CurrentDetailData["ftyinventoryukey"] = 0;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(string.Format(@"select a.*,b.FabricType,b.SCIRefno,f.MtlTypeID,m.IssueType,left(a.seq1+'   ',3)+a.seq2 seq
from dbo.ftyinventory a inner join dbo.po_supp_detail b on b.id=a.POID and b.seq1=a.seq1 and b.seq2 = a.Seq2
inner join Fabric f on f.SCIRefno = b.SCIRefno
inner join MtlType m on m.ID = f.MtlTypeID
where poid = '{0}' and a.seq1 ='{1}' and a.seq2 = '{2}' and lock=0 and mdivisionid='{3}'  and inqty-outqty+adjustqty > 0  --and stocktype='B' "
                            , CurrentDetailData["poid"], e.FormattedValue.ToString().PadRight(5).Substring(0, 3)
                                                 , e.FormattedValue.ToString().PadRight(5).Substring(3, 2), Sci.Env.User.Keyword), out dr, null))
                        {
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            CurrentDetailData["seq"] = e.FormattedValue;
                            CurrentDetailData["seq1"] = e.FormattedValue.ToString().Substring(0, 3);
                            CurrentDetailData["seq2"] = e.FormattedValue.ToString().Substring(3, 2);
                            CurrentDetailData["mdivisionid"] = dr["mdivisionid"];
                            CurrentDetailData["stocktype"] = dr["stocktype"];
                            CurrentDetailData["ftyinventoryukey"] = dr["ukey"];
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗

            #region -- 欄位設定 --
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .MaskedText("seq", "CCC-CC", "Seq#", width: Widths.AnsiChars(6), settings: ts2)  //1
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //2
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)  //3
            .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)  //4
            .Numeric("usedqty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)    //5
            .Text("SizeUnit", header: "SizeUnit", width: Widths.AnsiChars(6), iseditingreadonly: true)  //6          
            .Text("location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true)  //7
            .Numeric("accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //8
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10)    //5
            .Text("output", header: "Output", width: Widths.AnsiChars(20), iseditingreadonly: true, settings: ts) //9
            .Numeric("balanceqty", header: "Balance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //11
                //.Text("ftyinventoryukey", header: "FtyInventoryUkey", width: Widths.AnsiChars(10), iseditingreadonly: true)  //12
            ;     //
            #endregion 欄位設定

            #region 可編輯欄位變色

            detailgrid.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink;

            #endregion 可編輯欄位變色
        }

        protected override void OpenSubDetailPage()
        {
            subform.master = CurrentMaintain;
            subform.parentData = this.CurrentDetailData;
            base.OpenSubDetailPage();

        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? "" : e.Master["cutplanID"].ToString();
            this.DetailSelectCommand = string.Format(@"  select
a.Id
,isnull(a.FtyInventoryUkey,0) [FtyInventoryUkey]
,a.MDivisionID
,a.Poid
,a.seq1
,a.seq2
,left(a.seq1+'   ',3)+a.seq2 as seq
,a.StockType
,a.Qty
,p.Colorid
,p.SizeSpec
,p.UsedQty
,p.SizeUnit
,isnull((select t.MtlLocationID+',' from (select mtllocationid from dbo.FtyInventory_Detail where ukey=a.FtyInventoryUkey)t for xml path('')),'') [location]
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0)[description]
,isnull((select sum(Issue_Detail.qty) from dbo.issue inner join dbo.Issue_Detail on Issue_Detail.id = Issue.Id where Issue.type = 'B' and Issue.Status='Confirmed' and issue.id!=a.Id and Issue_Detail.FtyInventoryUkey = a.FtyInventoryUkey),0.00) [accu_issue]
,isnull((select v.sizeqty+', ' from (select (rtrim(Issue_Size.SizeCode) +'*'+convert(varchar,Issue_Size.Qty)) as sizeqty from dbo.Issue_Size where Issue_Size.Issue_DetailUkey = a.ukey and Issue_Size.Qty != '0.00') v for xml path('')),'') [output]
,a.Ukey
,isnull((select inqty-outqty+adjustqty from dbo.ftyinventory where ukey = a.ftyinventoryukey),0.00) as balanceqty
from dbo.Issue_Detail a left join dbo.po_supp_detail p on p.id  = a.poid and p.seq1= a.seq1 and p.seq2 =a.seq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "B";
            CurrentMaintain["issuedate"] = DateTime.Now;
            dtIssueBreakDown = null;
            gridIssueBreakDown.DataSource = null;
            textBox1.IsSupportEditMode = true;
            txtRequest.IsSupportEditMode = true;
            textBox1.ReadOnly = false;
            txtRequest.ReadOnly = false;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
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
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            textBox1.IsSupportEditMode = false;
            txtRequest.IsSupportEditMode = false;
            textBox1.ReadOnly = true;
            txtRequest.ReadOnly = true;
            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            DataTable result = null;
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(textBox1.Text.ToString()))
            {
                MyUtility.Msg.WarningBox("< Request# > or < Order ID >  can't be empty!", "Warning");
                textBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            #endregion 必輸檢查



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
                if (checkBox1.Checked == false)
                {
                    foreach (DataRow data in dtIssueBreakDown.ToList())
                    {
                        if (data.ItemArray[0].ToString() != textBox1.Text)
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
            var frm = new Sci.Production.Warehouse.P11_AutoPick(CurrentMaintain["id"].ToString(), this.poid, CurrentMaintain["cutplanid"].ToString(),textBox1.Text.ToString(),dtIssueBreakDown, sbSizecode);
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
            }
        }

        private void txtRequest_Validating(object sender, CancelEventArgs e)
        {
            if (txtRequest.Text == txtRequest.OldValue) return;
            //DBProxy.Current.Execute(null, string.Format("delete from dbo.issue_breakdown where id='{0}';", CurrentMaintain["id"].ToString()));
            CurrentMaintain["cutplanid"] = txtRequest.Text;
            textBox1.Text = "";
            CurrentMaintain["orderid"] = "";
            this.disPOID.Text = "";
            this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.cutplan where id='{0}' and mdivisionid = '{1}'", CurrentMaintain["cutplanid"], Sci.Env.User.Keyword));

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
                MyUtility.Msg.WarningBox("Can't found data");
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
                return;
            }
            //getpoid();
            this.disPOID.Text = this.poid;
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
                disCutCell.Text = MyUtility.GetValue.Lookup(string.Format("select CutCellID from dbo.cutplan  where id='{0}'", CurrentMaintain["cutplanid"]));
                disLine.Text = MyUtility.GetValue.Lookup(string.Format(@"select t.SewLine+','  from (select distinct o.SewLine 
from dbo.Issue_detail a inner join dbo.orders o on a.Poid = o.POID where a.id='{0}' and o.sewline !='') t for xml path('')", CurrentMaintain["id"]));

                DBProxy.Current.Select(null, string.Format(@";with cte as
(Select WorkOrder.FabricCombo,Cutplan_Detail.CutNo from Cutplan_Detail inner join dbo.workorder on WorkOrder.Ukey = Cutplan_Detail.WorkorderUkey 
where Cutplan_Detail.ID='{0}' )
select distinct FabricCombo ,(select convert(varchar,CutNo)+',' 
from (select CutNo from cte where cte.FabricCombo = a.FabricCombo )t order by CutNo for xml path('')) cutnos from cte a
", CurrentMaintain["cutplanid"]), out dt);
                ebCut.Text = String.Join(" / ", dt.AsEnumerable().Select(row => row["FabricCombo"].ToString() + "-" + row["cutnos"].ToString()));
                //ebArticle.Text = MyUtility.GetValue.Lookup(string.Format(@"select t.article+','  from (select distinct article 
                //from dbo.cutplan_detail  where id='{0}') t for xml path('')", CurrentMaintain["cutplanid"]));


                #region -- Status Label --

                label25.Text = CurrentMaintain["status"].ToString();

                #endregion Status Label
                #region -- POID
                this.getpoid();
                this.disPOID.Text = this.poid;
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

            string OrderID = textBox1.Text;

            sqlcmd = string.Format(@"select sizecode from dbo.order_sizecode 
where id = (select poid from dbo.orders where id='{0}') order by seq", OrderID);

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
            (select a.ID [orderid],a.Article,a.SizeCode,a.Qty from dbo.order_qty a
            inner join dbo.orders b on b.id = a.id
            where b.POID=(select poid from dbo.orders where id = '{0}')
            )
            ,Issue_Bdown as
            (
            	select isnull(Bdown.orderid,ib.OrderID) [OrderID],isnull(Bdown.Article,ib.Article) Article,isnull(Bdown.SizeCode,ib.sizecode) sizecode,isnull(ib.Qty,0) qty
            	from Bdown full outer join (select * from dbo.Issue_Breakdown where id='{1}') ib
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

            checkBox1_CheckedChanged(null,null);

            return Result.True;
        }
        private void getpoid()
        {
            CurrentMaintain["cutplanid"] = txtRequest.Text;
            this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.cutplan where id='{0}' and mdivisionid = '{1}'", CurrentMaintain["cutplanid"], Sci.Env.User.Keyword));
            if (MyUtility.Check.Empty(this.poid))
            {
                this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.orders where id='{0}' and mdivisionid = '{1}'", CurrentMaintain["orderid"], Sci.Env.User.Keyword));
            }

        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["mdivisionid"] = Sci.Env.User.Keyword;
            CurrentDetailData["poid"] = this.poid;
            DataTable sizeRange, subDetails;
            if (GetSubDetailDatas(CurrentDetailData, out subDetails))
            {
                DBProxy.Current.Select(null, string.Format(@"select a.SizeCode,b.Id,b.Issue_DetailUkey,isnull(b.Qty,0) QTY
from dbo.Order_SizeCode a left join dbo.Issue_Size b on b.SizeCode = a.SizeCode and b.id = '{1}' --and b.Issue_DetailUkey = {2}
where a.id='{0}' order by Seq", this.poid, CurrentMaintain["id"], CurrentDetailData["ukey"]), out sizeRange);
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
            this.getpoid();
            this.SubDetailSelectCommand = string.Format(@"select a.SizeCode,b.Id,b.Issue_DetailUkey,isnull(b.Qty,0) QTY
from dbo.Order_SizeCode a left join dbo.Issue_Size b on b.SizeCode = a.SizeCode and b.id = '{1}' and b.Issue_DetailUkey = {2}
where a.id='{0}' order by Seq", this.poid, masterID, ukey);
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
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d inner join FtyInventory f
on d.MDivisionID = f.MDivisionID and d.POID = f.POID  AND D.StockType = F.StockType
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
from dbo.Issue_Detail d left join FtyInventory f
on d.MDivisionID = f.MDivisionID and d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
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
            sqlcmd = string.Format(@"select * from issue_detail where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            #region -- 更新mdivisionpodetail B倉數 --
            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new Prgs_POSuppDetailData_B
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
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
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
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
from dbo.Issue_Detail d inner join FtyInventory f
on d.MDivisionID = f.MDivisionID and d.POID = f.POID  AND D.StockType = F.StockType
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
from dbo.Issue_Detail d left join FtyInventory f
on d.MDivisionID = f.MDivisionID and d.POID = f.POID  AND D.StockType = F.StockType
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
            sqlcmd = string.Format(@"select * from issue_detail where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new Prgs_POSuppDetailData_B
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
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
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        private void btnBOA_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P11_BOA(CurrentMaintain["id"].ToString(), this.poid, CurrentMaintain["cutplanid"].ToString(),textBox1.Text.ToString());
            frm.ShowDialog(this);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
            //刪除表身重新匯入
            foreach (DataRow del in DetailDatas)
            {
                del.Delete();
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
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }


            DataRow issue = this.CurrentDataRow;
            string id = issue["ID"].ToString();
            string request = issue["cutplanid"].ToString();
            string issuedate = issue["issuedate"].ToString();
            string remark = issue["remark"].ToString();
            string cutno = this.ebCut.Text;
            string article = this.ebArticle.Text;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));


            DataTable dt;
            DBProxy.Current.Select("",
                @"select    
             b.name 
            from dbo.Issue as a 
            inner join dbo.mdivision as b on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID
            ", pars, out dt);
            string RptTitle = dt.Rows[0]["name"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("name", RptTitle));

            DataTable dtsp;
            string poID;
            DBProxy.Current.Select("",
                @"select (select poid+',' from 
             (select distinct cd.POID from Cutplan_Detail cd where id =(select CutplanID from dbo.Issue where id='@id')  ) t
			  for xml path('')) as [poid]", pars, out dtsp);
            if (dtsp.Rows.Count == 0)
                poID = "";
            else
                poID = dtsp.Rows[0]["POID"].ToString();

            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("POID", poID));
            DualResult result;
            DataTable dtSizecode;
            string sqlcmd1 = string.Format(@"select distinct sizecode
	                    from dbo.Issue_Size
	                    where id = @ID order by sizecode");
            string sizecodes = "";
            result = DBProxy.Current.Select("", sqlcmd1, pars, out dtSizecode);
            foreach (DataRow dr in dtSizecode.Rows)
            {
                sizecodes += "[" + dr["sizecode"].ToString() + "]" + ",";
            }
            if (sizecodes.Length != 0)
            {
                sizecodes = sizecodes.Substring(0, sizecodes.Length - 1);
            }


            DataTable dtseq;

            string sqlcmd = string.Format(@"select Issue_detail.Seq1 + '-' + Issue_detail.Seq2 as SEQ
                    ,dbo.getMtlDesc(poid,Issue_detail.Seq1,Issue_detail.Seq2,2,0) as Description
                   ,Po_supp_detail.sizeunit as Unit,Po_supp_detail.colorid as Color,
                   Issue_detail.Qty as TransferQTY,dbo.Getlocation(ftyinventoryUkey) as Location,
                   s.*
                    from(
                    select * 
                    from (
	                    select sizecode,Issue_DetailUkey, qty
	                    from dbo.Issue_Size
	                    where id = @ID
	                    ) as s
	                    PIVOT
	                    (
	                     Sum(qty)
	                     FOR sizecode  IN ({0})
                    ) AS PivotTable) as s
                   left join dbo.Issue_Detail on ukey = s.Issue_DetailUkey
                    left join dbo.po_supp_detail on po_supp_detail.id = Issue_detail.POID and po_supp_detail.seq1 = Issue_detail.seq1 and po_supp_detail.seq2=Issue_detail.seq2
", sizecodes);
            result = DBProxy.Current.Select("", sqlcmd, pars, out dtseq);

            if (!result)
            {
                ShowErr(result);
                return true;
            }
            dtseq.Columns.Remove(dtseq.Columns["Issue_DetailUkey"]);
            string SEQ = dtseq.Rows[0]["SEQ"].ToString();
            //string tQty = dtseq.Rows[0]["tQTY"].ToString();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SEQ", SEQ));
            //report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("tQTY", tQty));


            DataTable dtlineno;
            string cLineNo;
            result = DBProxy.Current.Select("",
                @"select o.sewline from dbo.Orders o 
                    where id in (select distinct poid from issue_detail where id=@ID ) ", pars, out dtlineno);
            if (!result)
            {
                ShowErr(result);
                return true;
            }
            if (dtlineno.Rows.Count == 0)
                cLineNo = "";
            else
                cLineNo = dtlineno.Rows[0]["sewline"].ToString();

            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("sewline", cLineNo));

            DataTable dtcutcell;
            string cCellNo;
            result = DBProxy.Current.Select("",
                @"select    
             b.CutCellID 
            from dbo.Issue as a 
             inner join dbo.cutplan as b on b.id = a.cutplanid
            where b.id = a.CutplanID
            ", pars, out dtcutcell);
            if (!result)
            {
                ShowErr(result);
                return true;
            }

            if (dtlineno.Rows.Count == 0)
                cCellNo = "";
            else
                cCellNo = dtcutcell.Rows[0]["CutCellID"].ToString();

            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CutCellID", cCellNo));

            string xlt = @"Warehouse_P11.xltx";
            SaveXltReportCls xl = new SaveXltReportCls(xlt);

            xl.dicDatas.Add("##name", RptTitle);
            xl.dicDatas.Add("##ID", id);
            xl.dicDatas.Add("##cutplanid", request);
            xl.dicDatas.Add("##issuedate", issuedate);
            xl.dicDatas.Add("##remark", remark);
            xl.dicDatas.Add("##cCutNo", cutno);
            SaveXltReportCls.xltRptTable xlTable = new SaveXltReportCls.xltRptTable(dtseq);
            int allColumns = dtseq.Columns.Count;
            int sizeColumns = dtSizecode.Rows.Count;
            xlTable.lisTitleMerge.Add(new Dictionary<string, string> { 
            { "SIZE", string.Format("{0},{1}", allColumns-sizeColumns+1, allColumns) }}
           );
            //xlTable.Borders.AllCellsBorders = true;
            //xlTable.HeaderColor = Color.AliceBlue;
            //xlTable.ContentColor = Color.LightGreen;
            xlTable.Borders.OnlyHeaderBorders = true;
            xl.dicDatas.Add("##SEQ", xlTable);


            xl.Save(outpath, true);

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
        private void textBox1_Validating(object sender, EventArgs e)
        {
            if (textBox1.Text == textBox1.OldValue) return;
            CurrentMaintain["orderid"] = textBox1.Text;
            this.disPOID.Text = "";
            this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.orders where id='{0}' and mdivisionid = '{1}'", CurrentMaintain["orderid"], Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                if (MyUtility.Check.Empty(this.poid))
                {
                    MyUtility.Msg.WarningBox("Can't found data");
                    CurrentMaintain["cutplanid"] = "";
                    txtRequest.Text = "";
                    textBox1.Text = "";
                    return;
                }
                this.disPOID.Text = this.poid;
                //CurrentMaintain["orderid"] = this.poid;    
            }
        }

        private void textBox1_Validated(object sender, EventArgs e) //若order ID有變，重新撈取資料庫。
        {
            if (textBox1.Text == textBox1.OldValue) return;
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
            DBProxy.Current.Select(null, string.Format(@"select 
a.POID
,a.Ukey
,a.Seq1
,a.Seq2
,left(a.seq1+'   ',3)+a.seq2 seq
,a.StockType
,b.ColorID
,b.SizeSpec
,b.UsedQty
,b.SizeUnit
,isnull((select t.MtlLocationID+',' from (select mtllocationid from [Production].[dbo].FtyInventory_Detail where ukey=a.Ukey)t for xml path('')),'') [location]
,[Production].[dbo].getmtldesc(a.poid,a.seq1,a.seq2,2,0)[description]
,isnull((select a.InQty-a.OutQty+a.AdjustQty ),0.00) as balanceqty
from [Production].[dbo].ftyinventory a 
inner join [Production].[dbo].po_supp_detail b on b.id=a.POID and b.seq1=a.seq1 and b.seq2 = a.Seq2
inner join [Production].[dbo].Fabric f on f.SCIRefno = b.SCIRefno
inner join [Production].[dbo].MtlType m on m.ID = f.MtlTypeID
where a.Lock=0 and a.InQty-a.OutQty+a.AdjustQty > 0 
and a.MDivisionID='{0}' and a.POID='{1}' --and stocktype='B'
and b.FabricType='A'
and m.IssueType='Sewing' order by poid,seq1,seq2", Sci.Env.User.Keyword, this.poid, 0), out subData);
            //將資料塞入表身
            foreach (DataRow dr in subData.Rows)
            {
                base.OnDetailGridInsert();
                //DataRow ndr = ((DataTable)detailgridbs.DataSource).NewRow();
                CurrentDetailData["poid"] = dr["poid"];
                CurrentDetailData["seq"] = dr["seq"];
                CurrentDetailData["Description"] = dr["Description"];
                CurrentDetailData["Colorid"] = dr["Colorid"];
                CurrentDetailData["SizeSpec"] = dr["SizeSpec"];
                CurrentDetailData["usedqty"] = dr["usedqty"];
                CurrentDetailData["SizeUnit"] = dr["SizeUnit"];
                CurrentDetailData["location"] = dr["location"];
                CurrentDetailData["balanceqty"] = dr["balanceqty"];
                //((DataTable)detailgridbs.DataSource).Rows.Add(ndr);

                CurrentDetailData["seq1"] = dr["seq1"];
                CurrentDetailData["seq2"] = dr["seq2"];
                CurrentDetailData["mdivisionid"] = Sci.Env.User.Keyword;
                CurrentDetailData["stocktype"] = dr["stocktype"];
                CurrentDetailData["ftyinventoryukey"] = dr["ukey"];

//                DetailDatas.Add(ndr);

                DataTable sizeRange, subDetails;
                if (GetSubDetailDatas(CurrentDetailData, out subDetails))
                {
                    DBProxy.Current.Select(null, string.Format(@"select a.SizeCode,b.Id,b.Issue_DetailUkey,isnull(b.Qty,0) QTY
from dbo.Order_SizeCode a left join dbo.Issue_Size b on b.SizeCode = a.SizeCode and b.id = '{1}' --and b.Issue_DetailUkey = {2}
where a.id='{0}' order by Seq ", this.poid, CurrentMaintain["id"], CurrentDetailData["ukey"]), out sizeRange);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (dtIssueBreakDown == null) return;
            if (checkBox1.Checked)
            {
                dtIssueBreakDown.DefaultView.RowFilter = string.Format("");
            }
            else
            {
                dtIssueBreakDown.DefaultView.RowFilter = string.Format("OrderID='{0}'", textBox1.Text);
            }

        }

    }
}
