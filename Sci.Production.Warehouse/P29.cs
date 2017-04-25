using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci;
using Sci.Data;
using System.Linq;
using Sci.Production.PublicPrg;
using System.Transactions;

namespace Sci.Production.Warehouse
{
    public partial class P29 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk2 = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        DataTable master, detail;
        public P29(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            comboCategory.SelectedIndex = 0;
            comboFabricType.SelectedIndex = 0;

            #region -- Grid1 設定 --
            this.gridComplete.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridComplete.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridComplete)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out col_chk)
                 .Text("complete", header: "Complete" + Environment.NewLine + "Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(3), iseditingreadonly: true, alignment: DataGridViewContentAlignment.MiddleCenter)
                 .Text("poid", header: "Issue SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("seq1", header: "Issue" + Environment.NewLine + "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("seq2", header: "Issue" + Environment.NewLine + "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                 .Text("stockPOID", header: "Stock SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("stockseq1", header: "Stock" + Environment.NewLine + "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("stockseq2", header: "Stock" + Environment.NewLine + "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)

                 .Numeric("poqty", header: "Order Qty", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("InQty", header: "Accu Trans.", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("total_qty", header: "Trans. Qty", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("requestqty", header: "Balance", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Text("TransID", header: "Trans. ID" , width: Widths.AnsiChars(13), iseditingreadonly: true)
                  ;
            #endregion
            col_chk.CellClick += (s, e) =>
            {
                DataRow thisRow = this.gridComplete.GetDataRow(this.listControlBindingSource1.Position);
                if (null == thisRow) { return; }
                if (e.RowIndex==-1)
                {
                    if (((bool)this.gridComplete.Rows[0].Cells[e.ColumnIndex].Value))
                    {
                        foreach (DataRow dr in detail.Rows)
                        {
                            dr["selected"] = false;
                        }
                    }
                }
                else
                {
                    if (((bool)this.gridComplete.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                    {
                        thisRow["total_qty"] = 0.00;
                        foreach (DataRow dr in thisRow.GetChildRows("rel1"))
                        {
                            dr["selected"] = false;
                            dr["qty"] = 0.00;
                        }
                    }
                }
                this.gridComplete.ValidateControl();
            };
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
            Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;
            #region -- transfer qty valid --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow thisRow = this.gridComplete.GetDataRow(this.listControlBindingSource1.Position);
                    DataRow[] curentgridrowChild = thisRow.GetChildRows("rel1");
                    DataRow currentrow = gridRel.GetDataRow(gridRel.GetSelectedRowIndex());
                    currentrow["qty"] = e.FormattedValue; 
                    currentrow.GetParentRow("rel1")["total_qty"] = curentgridrowChild.Sum(row => (decimal)row["qty"]);
                    if (Convert.ToDecimal(e.FormattedValue) > 0)
                    {
                        currentrow["selected"] = true;
                        currentrow.GetParentRow("rel1")["selected"] = true;
                    }
                    else
                    {
                        currentrow["selected"] = false;
                        currentrow.GetParentRow("rel1")["selected"] = false;
                    }                 
                    currentrow["qty"] = e.FormattedValue;  
                }
            };
            #endregion
            #region -- To Location 右鍵開窗 --
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = gridRel.GetDataRow(gridRel.GetSelectedRowIndex());
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(dr["tostocktype"].ToString(), dr["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = gridRel.GetDataRow(e.RowIndex);
                    dr["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", dr["tostocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["tolocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }
                    trueLocation.Sort();
                    dr["tolocation"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion
            #region -- Grid2 設定 --
            this.gridRel.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridRel.DataSource = listControlBindingSource2;

            this.gridRel.CellValueChanged += (s, e) =>
            {
                if (gridRel.Columns[e.ColumnIndex].Name == col_chk2.Name)
                {
                    DataRow dr = gridRel.GetDataRow(e.RowIndex);
                    if(Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0){
                        dr["qty"] = dr["balanceQty"];
                    }else if(Convert.ToBoolean(dr["selected"]) == false){
                        dr["qty"] = 0;
                    }
                    dr.EndEdit();

                    DataRow thisRow = this.gridComplete.GetDataRow(this.listControlBindingSource1.Position);
                    DataRow[] curentgridrowChild = thisRow.GetChildRows("rel1");
                    DataRow currentrow = gridRel.GetDataRow(gridRel.GetSelectedRowIndex());
                    currentrow.GetParentRow("rel1")["total_qty"] = curentgridrowChild.Sum(row => (decimal)row["qty"]);
                    currentrow.EndEdit();
                }
            };

            Helper.Controls.Grid.Generator(this.gridRel)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out col_chk2)
                 .Text("fromroll", header: "Roll#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(2), iseditingreadonly: true)
                 .Numeric("balanceQty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("qty", header: "Trans. Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, settings: ns).Get(out col_Qty)
                  .Text("fromlocation", header: "From Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: true)
                  .Text("tolocation", header: "To Bulk" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: false, settings: ts2).Get(out col_tolocation)
                  ;
            col_Qty.DefaultCellStyle.BackColor = Color.Pink;
            col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            chp();
        }
        private void chp()
        {
            #region selected
            col_chk2.CellClick += (s, e) =>
            {
                DataRow thisRow = this.gridRel.GetDataRow(this.listControlBindingSource2.Position);
                if (null == thisRow) { return; }
                if (e.RowIndex == -1)
                {
                    if (!((bool)this.gridRel.Rows[0].Cells[e.ColumnIndex].Value))
                    {
                        // 原本沒selected , 會變selected , 就直接勾選parentRow
                        thisRow.GetParentRow("rel1")["selected"] = true;
                      
                    }
                }
                else
                {
                    if (!((bool)this.gridRel.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                    {
                        // 原本沒selected , 會變selected , 就直接勾選parentRow
                        thisRow.GetParentRow("rel1")["selected"] = true;                        
                    }
                    else
                    {
                        thisRow["qty"] = 0.00;
                        DataRow y = thisRow.GetParentRow("rel1");
                        var temp = y.GetChildRows("rel1");
                        if (temp != null)
                        {
                            var selected = temp.Where(row => (bool)row["selected"]).ToList();
                            if (selected.Count <= 1)
                            {
                                thisRow.GetParentRow("rel1")["selected"] = false;
                                //thisRow.GetParentRow("rel1")["total_qty"] = temp.Sum(row => (decimal)row["qty"]);
                                thisRow.GetParentRow("rel1")["total_qty"] = 0.00;
                            }
                            else
                                thisRow.GetParentRow("rel1")["total_qty"] = temp.Sum(row => (decimal)row["qty"]);
                        }

                    }
                }
                this.gridRel.ValidateControl();
                this.gridComplete.ValidateControl();
            };

            #endregion
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            int selectindex = comboCategory.SelectedIndex;
            int selectindex2 = comboFabricType.SelectedIndex;
            string CuttingInline_b, CuttingInline_e, OrderCfmDate_b, OrderCfmDate_e, SP, ProjectID, factory;
            CuttingInline_b = null;
            CuttingInline_e = null;
            OrderCfmDate_b = null;
            OrderCfmDate_e = null;
            SP = txtIssueSP.Text;
            ProjectID = txtProjectID.Text;
            factory = txtmfactory.Text;

            if (dateCuttingInline.Value1 != null) CuttingInline_b = this.dateCuttingInline.Text1;
            if (dateCuttingInline.Value2 != null) { CuttingInline_e = this.dateCuttingInline.Text2; }

            if (dateOrderCfmDate.Value1 != null) { OrderCfmDate_b = this.dateOrderCfmDate.Text1; }
            if (dateOrderCfmDate.Value2 != null) { OrderCfmDate_e = this.dateOrderCfmDate.Text2; }

            if ((CuttingInline_b == null && CuttingInline_e == null) &&
                MyUtility.Check.Empty(SP) && MyUtility.Check.Empty(ProjectID) &&
                (OrderCfmDate_b == null && OrderCfmDate_e == null))
            {
                MyUtility.Msg.WarningBox("< Project ID > or < Cutting Inline > or < Order Confirm Date > or < Issue SP# > can't be empty!!");
                txtIssueSP.Focus();
                return;
            }

            StringBuilder sqlcmd = new StringBuilder();
            #region -- sql command --
            sqlcmd.Append(string.Format(@";with cte
as
(
select convert(bit,0) as selected,iif(y.cnt >0 or yz.cnt=0 ,'Y','') complete,f.MDivisionID,rtrim(o.id) poid,o.Category,o.FtyGroup,o.CFMDate,o.CutInLine,o.ProjectID,o.FactoryID 
,rtrim(pd.seq1) seq1,pd.seq2,pd.StockPOID,pd.StockSeq1,pd.StockSeq2
,ROUND(x.taipei_qty*isnull(v.RateValue,1),2,1) N'PoQty'
,pd.POUnit,pd.StockUnit
,InQty = isnull(xx.InQty,0)
from dbo.orders o WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = o.ID
left join View_Unitrate v on v.FROM_U = pd.POUnit and v.TO_U = pd.StockUnit
inner join dbo.Factory f WITH (NOLOCK) on f.id = o.FtyGroup
outer apply
(select count(1) cnt from FtyInventory fi WITH (NOLOCK) left join FtyInventory_Detail fid WITH (NOLOCK) on fid.Ukey = fi.Ukey 
	where  fi.POID = pd.stockpoID and fi.Seq1 = pd.stockSeq1 and fi.Seq2 = pd.stockSeq2 and fi.StockType = 'I' 
	and fid.MtlLocationID is null and fi.Lock = 0 and fi.InQty - fi.OutQty + fi.AdjustQty > 0
) y--Detail有MD為null數量,沒有則為0,沒資料也為0
outer apply(
	select count(1) cnt from FtyInventory fi WITH (NOLOCK) left join FtyInventory_Detail fid WITH (NOLOCK) on fid.Ukey = fi.Ukey 
	where  fi.POID = pd.ID and fi.Seq1 = pd.Seq1 and fi.Seq2 = pd.Seq2 and fi.StockType = 'B'
) yz--Detail資料數量
cross apply
(
select sum(iif(i.type=2,i.qty,0-i.qty)) taipei_qty 
 from dbo.Invtrans i WITH (NOLOCK) 
 where i.InventoryPOID = pd.StockPOID and i.InventorySeq1 = pd.StockSeq1 and i.PoID = pd.ID and i.InventorySeq2 = pd.StockSeq2 and (i.type=2 or i.type=6)
)x -- 需要轉的數量
cross apply
(
	select sum(s2.Qty) as InQty 
    from dbo.SubTransfer s1 WITH (NOLOCK) 
    inner join dbo.SubTransfer_Detail s2 WITH (NOLOCK) on s2.Id= s1.Id 
	where s1.type ='B' and s1.Status ='Confirmed' and s2.ToStockType = 'B' and s2.ToPOID = pd.id and s2.ToSeq1 = pd.seq1 and s2.ToSeq2 = pd.seq2 
) xx --已轉的數量
where pd.seq1 like '7%' and f.MDivisionID = '{0}'", Env.User.Keyword));

            #region -- 條件 --
            switch (selectindex)
            {
                case 0:
                    sqlcmd.Append(@" and (o.Category = 'B')");
                    break;
                case 1:
                    sqlcmd.Append(@" and o.Category = 'S' ");
                    break;
                case 2:
                    sqlcmd.Append(@" and (o.Category = 'M')");
                    break;
            }

            switch (selectindex2)
            {
                case 0:
                    sqlcmd.Append(@" AND pd.FabricType ='F'");
                    break;
                case 1:
                    sqlcmd.Append(@" AND pd.FabricType ='A'");
                    break;
                case 2:
                    break;
            }

            if (!MyUtility.Check.Empty(SP)) sqlcmd.Append(string.Format(@" and pd.id = '{0}'", SP));

            if (!MyUtility.Check.Empty(factory)) { sqlcmd.Append(string.Format(@" and o.FtyGroup = '{0}'", factory)); }

            if (!(string.IsNullOrWhiteSpace(ProjectID))) { sqlcmd.Append(string.Format(@" and o.ProjectID = '{0}'", ProjectID)); }

            if (!(string.IsNullOrWhiteSpace(CuttingInline_b)))
            {
                sqlcmd.Append(string.Format(@" and not(o.CutInLine > '{1}' or  o.CutInLine < '{0}')", CuttingInline_b, CuttingInline_e));
            }
            if (!(string.IsNullOrWhiteSpace(OrderCfmDate_b)))
            {
                sqlcmd.Append(string.Format(@" and o.CFMDate between '{0}' and '{1}'", OrderCfmDate_b, OrderCfmDate_e));
            }
            #endregion
            sqlcmd.Append(@")
select *,0.00 qty into #tmp from cte
where PoQty > InQty

select * from #tmp order by poid,seq1,seq2 ;

select 
convert(bit,0) as selected,
fi.Ukey FromFtyInventoryUkey,
o.FactoryID FromFactoryID,
fi.POID FromPoid,
fi.Seq1 FromSeq1,
fi.Seq2 Fromseq2,
fi.Roll FromRoll,
fi.Dyelot FromDyelot,
fi.StockType FromStockType,
fi.InQty - fi.OutQty + fi.AdjustQty BalanceQty,
0.00 as Qty,
t.FactoryID  toFactoryID ,rtrim(t.poID) topoid,rtrim(t.seq1) toseq1,t.seq2 toseq2, fi.Roll toRoll, fi.Dyelot toDyelot,'B' tostocktype 
,stuff((select ',' + mtllocationid from (select MtlLocationid from dbo.FtyInventory_Detail WITH (NOLOCK) where ukey = fi.Ukey)t for xml path('')), 1, 1, '') fromlocation
,'' tolocation
,GroupQty = Sum(fi.InQty - fi.OutQty + fi.AdjustQty) over(partition by t.poid,t.seq1,t.SEQ2,t.FactoryID,t.StockPOID,t.StockSeq1,t.StockSeq2,fi.Dyelot)
from #tmp t 
inner join FtyInventory fi WITH (NOLOCK) on fi.POID = t.StockPOID and fi.seq1 = t.StockSeq1 and fi.Seq2 = t.StockSeq2
inner join dbo.orders o WITH (NOLOCK) on fi.POID=o.id
where fi.StockType ='I' and fi.Lock = 0 and fi.InQty - fi.OutQty + fi.AdjustQty > 0 
order by topoid,toseq1,toseq2,GroupQty DESC,fi.Dyelot,BalanceQty DESC
drop table #tmp");

            this.ShowWaitMessage("Data Loading....");
            #endregion
            DataSet dataSet;
            if (!SQL.Selects("", sqlcmd.ToString(), out dataSet))
            {
                MyUtility.Msg.WarningBox(sqlcmd.ToString(), "DB error!!");
                return;
            }

            master = dataSet.Tables[0];
            master.TableName = "Master";
            //master.DefaultView.Sort = "poid,seq1,seq2,poqty";
            //dataSet.Tables[0].DefaultView.Sort = "poid,seq1,seq2,poqty";

            detail = dataSet.Tables[1];
            detail.TableName = "Detail";
            //dataSet.Tables[1].DefaultView.Sort = "fromdyelot,balanceQty";

            DataRelation relation = new DataRelation("rel1"
                    , new DataColumn[] { master.Columns["poid"], master.Columns["seq1"], master.Columns["seq2"] }
                    , new DataColumn[] { detail.Columns["toPoid"], detail.Columns["toseq1"], detail.Columns["toseq2"] }
                    );

            dataSet.Relations.Add(relation);

            //master.Columns.Add("total_qty", typeof(decimal), "sum(child.qty)");
            master.Columns.Add("requestqty", typeof(decimal), "poqty - inqty - sum(child.qty)");
            master.Columns.Add("total_qty", typeof(decimal));

            listControlBindingSource1.DataSource = dataSet;
            listControlBindingSource1.DataMember = "Master";
            listControlBindingSource2.DataSource = listControlBindingSource1;
            listControlBindingSource2.DataMember = "rel1";

            if (dataSet.Tables[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("NO Data!");
                return;
            }
            btnCreate.Enabled = true;
            this.HideWaitMessage();
        }

        private void btnAutoPick_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(master)) return;
            if (master.Rows.Count == 0) return;

            foreach (DataRow dr in master.Rows)
            {
                if (dr["selected"].ToString().ToUpper() == "TRUE")
                {
                    DataRow[] curentgridrowChild = dr.GetChildRows("rel1");
                    foreach (DataRow temp in curentgridrowChild)
                    {
                        temp["qty"] = 0.00;
                        temp["selected"] = false;
                    }
                    //dr["total_qty"] = curentgridrowChild.Sum(row => (decimal)row["qty"]);
                    dr["total_qty"] = 0.00;
                }

                if (dr["selected"].ToString() == "True" && !MyUtility.Check.Empty(dr["requestqty"]))
                {
                    var issued = PublicPrg.Prgs.autopick(dr, false,"I");
                    if (issued == null) return;
                    
                    foreach (DataRow dr2 in issued)
                    {
                        DataRow[] findrow = detail.Select(string.Format(@"FromftyInventoryUkey = {0} and topoid = '{1}'
                                                                          and toseq1 = '{2}' and toseq2 = '{3}'"
                            , dr2["ftyinventoryukey"], dr["poid"], dr["seq1"], dr["seq2"]));
                        if (findrow.Length > 0)
                        {
                            findrow[0]["qty"] = dr2["qty"];
                            findrow[0]["selected"] = true;
                        }
                    }
                    var tempchildrows = dr.GetChildRows("rel1");
                    dr["total_qty"] = tempchildrows.Sum(row => (decimal)row["qty"]);
                    this.gridRel.ValidateControl();
                    this.gridComplete.ValidateControl();
                }               
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detail))
            {
                MyUtility.Msg.WarningBox("Please select data first!!");
                return;
            }
            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to create data?");
            if (dResult == DialogResult.No) return;

            DataRow[] findrow = detail.Select(@"selected = true and qty > 0");
            if (findrow.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!!");
                return;
            }

            string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Factory + "PI", "SubTransfer", System.DateTime.Now);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return ;
                }

            StringBuilder insertMaster = new StringBuilder();
            StringBuilder insertDetail = new StringBuilder();

            insertMaster.Append(string.Format(@"insert into dbo.subtransfer (id,type,issuedate,mdivisionid,FactoryID,status,addname,adddate,remark)
            values ('{0}','B',getdate(),'{1}','{3}','New','{2}',getdate(),'Batch create by P29')", tmpId, Env.User.Keyword, Env.User.UserID,Sci.Env.User.Factory));

            foreach (DataRow item in findrow)
            {
                insertDetail.Append(string.Format(@"insert into dbo.subtransfer_detail ([ID]
           ,[FromFtyInventoryUkey]
           ,[FromFactoryID]
           ,[FromPOID]
           ,[FromSeq1]
           ,[FromSeq2]
           ,[FromRoll]
           ,[FromStockType]
           ,[FromDyelot]
           ,[toFactoryID]
           ,[ToPOID]
           ,[ToSeq1]
           ,[ToSeq2]
           ,[ToRoll]
           ,[ToStockType]
           ,[ToDyelot]
           ,[Qty]
           ,[ToLocation])
values ('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}'
,'{9}','{10}','{11}','{12}','{13}','{14}','{15}',{16},'{17}');", tmpId, item["fromftyinventoryukey"], item["FromFactoryID"], item["frompoid"], item["fromseq1"], item["fromseq2"], item["fromroll"], item["fromstocktype"], item["fromdyelot"]
          , item["toFactoryID"], item["topoid"], item["toseq1"], item["toseq2"], item["toroll"], item["toStocktype"], item["toDyelot"], item["qty"], item["tolocation"]));
            }

            TransactionScope _transactionscope = new TransactionScope();
            DualResult result;
            using (_transactionscope)
            {
                try
                {
                    if (!(result = Sci.Data.DBProxy.Current.Execute(null, insertMaster.ToString())))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Create failed");
                        return;
                    }
                    if (!(result = Sci.Data.DBProxy.Current.Execute(null, insertDetail.ToString())))
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Create failed");
                        return; ;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox(string.Format("Trans. ID {0} be created!!",tmpId),"Complete!");
                }
                catch(Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope = null;

            if (!master.Columns.Contains("TransID")) master.Columns.Add("TransID", typeof(string));
            foreach (DataRow Alldetailrows in detail.Rows)
            {
                if (Alldetailrows["selected"].ToString().ToUpper() == "TRUE" && Convert.ToDecimal(Alldetailrows["qty"]) > 0)
                {
                    Alldetailrows.GetParentRow("rel1")["selected"] = true;
                    Alldetailrows.GetParentRow("rel1")["TransID"] = tmpId;
                }
            }
            //Create後Btn失效，需重新Qurey才能再使用。
            btnCreate.Enabled = false;
            this.gridRel.ValidateControl();
            this.gridComplete.ValidateControl();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkOnly.Checked)
            {
                listControlBindingSource1.Filter = "complete = 'Y'";
            }
            else
            {
                listControlBindingSource1.Filter = "";
            }

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(master))
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Bulk");
                return;
            }
            if (!master.Columns.Contains("TransID"))
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Bulk");
                return;
            }
            master.DefaultView.RowFilter = "TransID<>''";
            DataTable Exceldt = master.DefaultView.ToTable();
            if (Exceldt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Bulk");
                return;
            }
            Sci.Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(Exceldt);
            sdExcel.Save();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
