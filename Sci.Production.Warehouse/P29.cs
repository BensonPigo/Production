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
            cbxCategory.SelectedIndex = 0;
            cbxFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetGridFrozen(this.grid1);

            #region -- Grid1 設定 --
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out col_chk)
                .CheckBox("complete", header: "Complete" + Environment.NewLine + "Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
                 .Text("poid", header: "Issue SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("seq1", header: "Issue" + Environment.NewLine + "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("seq2", header: "Issue" + Environment.NewLine + "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                 .Text("stockPOID", header: "Stock SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("stockseq1", header: "Stock" + Environment.NewLine + "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("stockseq2", header: "Stock" + Environment.NewLine + "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)

                 .Numeric("poqty", header: "Order Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("InQty", header: "Accu Trans.", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("total_qty", header: "Trans. Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("requestqty", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  ;
            #endregion
            col_chk.CellClick += (s, e) =>
            {
                DataRow thisRow = this.grid1.GetDataRow(this.listControlBindingSource1.Position);
                if (null == thisRow) { return; }
                if (e.RowIndex==-1)
                {
                    if (((bool)this.grid1.Rows[0].Cells[e.ColumnIndex].Value))
                    {
                        foreach (DataRow dr in detail.Rows)
                        {
                            dr["selected"] = false;
                        }
                    }
                }
                else
                {
                    if (((bool)this.grid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                    {
                        thisRow["total_qty"] = 0.00;
                        foreach (DataRow dr in thisRow.GetChildRows("rel1"))
                        {
                            dr["selected"] = false;
                            dr["qty"] = 0.00;
                        }
                    }
                }
                this.grid1.ValidateControl();
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
                    DataRow thisRow = this.grid1.GetDataRow(this.listControlBindingSource1.Position);
                    DataRow[] curentgridrowChild = thisRow.GetChildRows("rel1");
                    DataRow currentrow = grid2.GetDataRow(grid2.GetSelectedRowIndex());
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
                    DataRow dr = grid2.GetDataRow(grid2.GetSelectedRowIndex());
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
                    DataRow dr = grid2.GetDataRow(e.RowIndex);
                    dr["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", dr["tostocktype"].ToString(), Sci.Env.User.Keyword);
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
            this.grid2.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid2.DataSource = listControlBindingSource2;

            Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out col_chk2)
                 .Text("fromroll", header: "Roll#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(2), iseditingreadonly: true)
                 .Numeric("balanceQty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("qty", header: "Trans. Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, settings: ns).Get(out col_Qty)
                  .Text("fromlocation", header: "From Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(20), iseditingreadonly: true)
                  .Text("tolocation", header: "To Bulk" + Environment.NewLine + "Location", width: Widths.AnsiChars(20), iseditingreadonly: false, settings: ts2).Get(out col_tolocation)
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
                DataRow thisRow = this.grid2.GetDataRow(this.listControlBindingSource2.Position);
                if (null == thisRow) { return; }
                if (e.RowIndex == -1)
                {
                    if (!((bool)this.grid2.Rows[0].Cells[e.ColumnIndex].Value))
                    {
                        // 原本沒selected , 會變selected , 就直接勾選parentRow
                        thisRow.GetParentRow("rel1")["selected"] = true;
                      
                    }
                }
                else
                {
                    if (!((bool)this.grid2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
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
                this.grid2.ValidateControl();
                this.grid1.ValidateControl();
            };

            #endregion
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            int selectindex = cbxCategory.SelectedIndex;
            int selectindex2 = cbxFabricType.SelectedIndex;
            string CuttingInline_b, CuttingInline_e, OrderCfmDate_b, OrderCfmDate_e, SP, ProjectID, factory;
            CuttingInline_b = null;
            CuttingInline_e = null;
            OrderCfmDate_b = null;
            OrderCfmDate_e = null;
            SP = txtSP.Text;
            ProjectID = txtProjectID.Text;
            factory = txtmfactory1.Text;

            if (dateRangeCuttingInline.Value1 != null) CuttingInline_b = this.dateRangeCuttingInline.Text1;
            if (dateRangeCuttingInline.Value2 != null) { CuttingInline_e = this.dateRangeCuttingInline.Text2; }

            if (dateRangeOrderCfmDate.Value1 != null) { OrderCfmDate_b = this.dateRangeOrderCfmDate.Text1; }
            if (dateRangeOrderCfmDate.Value2 != null) { OrderCfmDate_e = this.dateRangeOrderCfmDate.Text2; }

            if ((CuttingInline_b == null && CuttingInline_e == null) &&
                MyUtility.Check.Empty(SP) && MyUtility.Check.Empty(ProjectID) &&
                (OrderCfmDate_b == null && OrderCfmDate_e == null))
            {
                MyUtility.Msg.WarningBox("< Project ID > or < Cutting Inline > or < Order Confirm Date > or < Issue SP# > can't be empty!!");
                txtSP.Focus();
                return;
            }

            StringBuilder sqlcmd = new StringBuilder();
            #region -- sql command --
            sqlcmd.Append(string.Format(@";with cte
as
(
select convert(bit,0) as selected,iif(y.cnt >0 or yz.cnt=0 ,0,1) complete,o.MDivisionID,rtrim(o.id) poid,o.Category,o.FtyGroup,o.CFMDate,o.CutInLine,o.ProjectID
,rtrim(pd.seq1) seq1,pd.seq2,pd.StockPOID,pd.StockSeq1,pd.StockSeq2
--,pd.Qty*v.RateValue PoQty
,ROUND(pd.Qty*v.RateValue,2,1) N'PoQty'
,pd.POUnit,pd.StockUnit
,mpd.InQty
from dbo.orders o 
inner join dbo.PO_Supp_Detail pd on pd.id = o.ID
inner join View_Unitrate v on v.FROM_U = pd.POUnit and v.TO_U = pd.StockUnit
left join dbo.MDivisionPoDetail mpd on mpd.MDivisionID = o.MDivisionID 
    and mpd.POID = pd.ID and mpd.Seq1 = pd.SEQ1 and mpd.Seq2 = pd.SEQ2
outer apply
(select count(1) cnt from FtyInventory fi left join FtyInventory_Detail fid on fid.Ukey = fi.Ukey 
	where  fi.POID = pd.stockpoID and fi.Seq1 = pd.stockSeq1 and fi.Seq2 = pd.stockSeq2 and fi.StockType = 'I' and fi.MDivisionID = o.MDivisionID
	and fid.MtlLocationID is null and fi.Lock = 0 and fi.InQty - fi.OutQty + fi.AdjustQty > 0
) y--Detail有MD為null數量,沒有則為0,沒資料也為0
outer apply(
	select count(1) cnt from FtyInventory fi left join FtyInventory_Detail fid on fid.Ukey = fi.Ukey 
	where  fi.POID = pd.ID and fi.Seq1 = pd.Seq1 and fi.Seq2 = pd.Seq2 and fi.StockType = 'B' and fi.MDivisionID = o.MDivisionID
) yz--Detail資料數量
where o.MDivisionID = '{0}'
and pd.seq1 like '7%'", Env.User.Keyword));

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

select * from #tmp;

select 
convert(bit,0) as selected,
fi.Ukey FromFtyInventoryUkey,
fi.MDivisionID FromMdivisionID,
fi.POID FromPoid,
fi.Seq1 FromSeq1,
fi.Seq2 Fromseq2,
fi.Roll FromRoll,
fi.Dyelot FromDyelot,
fi.StockType FromStockType,
fi.InQty - fi.OutQty + fi.AdjustQty BalanceQty,
0.00 as Qty,
fi.MDivisionID toMdivisionID,rtrim(t.poID) topoid,rtrim(t.seq1) toseq1,t.seq2 toseq2, fi.Roll toRoll, fi.Dyelot toDyelot,'B' tostocktype 
,(select mtllocationid+',' from (select MtlLocationid from dbo.FtyInventory_Detail where ukey = fi.Ukey)t for xml path('')) fromlocation
,'' tolocation
from #tmp t inner join FtyInventory fi on fi.MDivisionID = t.MDivisionID and fi.POID = t.StockPOID 
and fi.seq1 = t.StockSeq1 and fi.Seq2 = t.StockSeq2
where fi.StockType ='I' and fi.Lock = 0 and fi.InQty - fi.OutQty + fi.AdjustQty > 0 
drop table #tmp");
            #endregion
            DataSet dataSet;
            if (!SQL.Selects("", sqlcmd.ToString(), out dataSet))
            {
                MyUtility.Msg.WarningBox(sqlcmd.ToString(), "DB error!!");
                return;
            }

            master = dataSet.Tables[0];
            master.TableName = "Master";
            master.DefaultView.Sort = "poid,seq1,seq2";

            detail = dataSet.Tables[1];
            detail.TableName = "Detail";

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
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to create data?");
            if (dResult == DialogResult.No) return;

            DataRow[] findrow = detail.Select(@"selected = true and qty > 0");
            if (findrow.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!!");
                return;
            }

            string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "PI", "SubTransfer", System.DateTime.Now);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return ;
                }

            StringBuilder insertMaster = new StringBuilder();
            StringBuilder insertDetail = new StringBuilder();

            insertMaster.Append(string.Format(@"insert into dbo.subtransfer (id,type,issuedate,mdivisionid,status,addname,adddate,remark)
            values ('{0}','B',getdate(),'{1}','New','{2}',getdate(),'Batch create by P29')",tmpId,Env.User.Keyword,Env.User.UserID));

            foreach (DataRow item in findrow)
            {
                insertDetail.Append(string.Format(@"insert into dbo.subtransfer_detail ([ID]
           ,[FromFtyInventoryUkey]
           ,[FromMDivisionID]
           ,[FromPOID]
           ,[FromSeq1]
           ,[FromSeq2]
           ,[FromRoll]
           ,[FromStockType]
           ,[FromDyelot]
           ,[ToMDivisionID]
           ,[ToPOID]
           ,[ToSeq1]
           ,[ToSeq2]
           ,[ToRoll]
           ,[ToStockType]
           ,[ToDyelot]
           ,[Qty]
           ,[ToLocation])
values ('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}'
,'{9}','{10}','{11}','{12}','{13}','{14}','{15}',{16},'{17}');", tmpId, item["fromftyinventoryukey"], item["fromMdivisionid"], item["frompoid"], item["fromseq1"], item["fromseq2"], item["fromroll"], item["fromstocktype"], item["fromdyelot"]
          , item["toMdivisionid"], item["topoid"], item["toseq1"], item["toseq2"], item["toroll"], item["toStocktype"], item["toDyelot"], item["qty"], item["tolocation"]));
            }

            TransactionScope _transactionscope = new TransactionScope();
            DualResult result;
            using (_transactionscope)
            {
                try
                {
                    if (!(result = Sci.Data.DBProxy.Current.Execute(null, insertMaster.ToString())))
                    {
                        MyUtility.Msg.WarningBox("Create failed");
                        _transactionscope.Dispose();
                        return;
                    }
                    if (!(result = Sci.Data.DBProxy.Current.Execute(null, insertDetail.ToString())))
                    {
                        MyUtility.Msg.WarningBox("Create failed");
                        _transactionscope.Dispose();
                        return; ;
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox(string.Format("Trans. ID {0} be created!!",tmpId),"Complete!");
                }
                catch(Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope = null;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                listControlBindingSource1.Filter = "complete = 1";
            }
            else
            {
                listControlBindingSource1.Filter = "";
            }

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            Sci.Utility.Excel.SaveDataToExcel sdExcel = new Utility.Excel.SaveDataToExcel(master);
            sdExcel.Save();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
