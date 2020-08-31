using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P23_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private DataSet dsTmp;
        private DataTable dtSort;
        private bool sortFinal;
        private int scrollnb;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
        private DataRelation relation;

        /// <summary>
        /// Initializes a new instance of the <see cref="P23_Import"/> class.
        /// </summary>
        /// <param name="master">Master DataRow</param>
        /// <param name="detail">Detail Table</param>
        public P23_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
            this.dtSort = new DataTable();
            this.dtSort.Columns.Add("Name", typeof(string));
            this.dtSort.Columns.Add("Sort", typeof(string));
            this.sortFinal = false;
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtIssueSP.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                this.txtIssueSP.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                #region -- Sql Command --
                strSQLCmd.Append(string.Format(
                    @"
;with cte as (
    select  rtrim(pd.ID) poid 
            , rtrim(pd.seq1) seq1
            , pd.seq2
            , pd.POUnit
            , StockUnit = StockUnit.value
            , Round(dbo.GetUnitQty(POUnit, StockUnit.value, pd.Qty), 2) poqty
            , o.FactoryID ToFactoryID
	        , dbo.getMtlDesc(poid,seq1,seq2,2,0) as [description]
	        , x.InventoryPOID
            , x.InventorySeq1
            , x.InventorySeq2
	        , x.earliest
            , x.lastest
            , Round(dbo.GetUnitQty(POUnit, StockUnit.value, x.taipei_qty), 2) taipei_qty
            , pd.Refno
    from dbo.PO_Supp_Detail pd WITH (NOLOCK) 
    outer apply (
        select value = dbo.GetStockUnitBySPSeq (pd.id, pd.seq1, pd.seq2)
    ) StockUnit
    inner join dbo.orders o WITH (NOLOCK) on o.id = pd.id
    inner join dbo.factory f WITH (NOLOCK) on f.id = o.FtyGroup
    cross apply(
        select  i.InventoryPOID
                , i.InventorySeq1
                , i.InventorySeq2
                , min(i.ConfirmDate) earliest
                , max(i.confirmdate) lastest
                , sum(iif(i.type='2',i.qty,0-i.qty)) taipei_qty 
        from dbo.Invtrans i WITH (NOLOCK) 
        where   i.InventoryPOID = pd.StockPOID 
                and i.InventorySeq1 = pd.StockSeq1 
                and i.PoID = pd.ID 
                and i.InventorySeq2 = pd.StockSeq2 
                and i.seq70seq1 = pd.seq1
                and i.seq70seq2 = pd.seq2
                and (i.type='2' or i.type='6')
        group by i.InventoryPOID, i.InventorySeq1, i.InventorySeq2
    )x
    where exists (select 1 
	              from orders o2 WITH (NOLOCK)
	              inner join Factory f WITH (NOLOCK) on o2.FactoryID = f.ID
	              where pd.id = o2.ID
	              and f.IsProduceFty = 1
          )
          and f.MDivisionID='{0}' 
          and pd.ID = @poid
)
select  m.*
        ,isnull(xx.accu_qty,0)accu_qty 
into #tmp
from cte m 
cross apply(
    select  sum(s2.Qty) as accu_qty 
    from dbo.SubTransfer s1 WITH (NOLOCK) 
    inner join dbo.SubTransfer_Detail s2 WITH (NOLOCK) on s2.Id= s1.Id 
	where   s1.type ='B' 
            and s1.Status = 'Confirmed' 
            and s2.ToStockType = 'B' 
		    and s2.ToPOID = m.poid 
            and s2.ToSeq1 = m.seq1 
            and s2.ToSeq2 = m.seq2 
            and s1.Id !='{1}'
) xx

select distinct aa.* 
from #tmp aa
inner join dbo.FtyInventory fi WITH (NOLOCK) 
on fi.POID = aa.InventoryPOID 
and fi.seq1 = aa.Inventoryseq1 
and fi.seq2 = aa.InventorySEQ2 
and fi.StockType = 'I'
where fi.Lock = 0
order by aa.poid,aa.seq1,aa.seq2 ;

select  0 AS selected
        , '' as id
        , o.FactoryID FromFactoryID
        , fi.POID FromPOID
        , fi.seq1 Fromseq1
        , fi.seq2 Fromseq2
        , dbo.getmtldesc(fi.POID,fi.seq1,fi.seq2,2,0) as [description]
        , concat(Ltrim(Rtrim(fi.seq1)), ' ', fi.seq2) as fromseq
        , fi.roll FromRoll
        , fi.dyelot FromDyelot
        , fi.stocktype FromStockType
        , fi.Ukey as fromftyinventoryukey 
        , fi.InQty
        , fi.OutQty
        , fi.AdjustQty
        , fi.InQty - fi.OutQty + fi.AdjustQty as balanceQty
        , 0.00 as qty
        , StockUnit
        , [accu_qty] = isnull(( select inqty 
                                from dbo.FtyInventory t WITH (NOLOCK) 
                                where   t.POID = #tmp.POID 
                                        and t.seq1 = #tmp.seq1 
                                        and t.seq2 = #tmp.seq2 
                                        and t.StockType = 'B' 
	                                    and t.Roll = fi.Roll 
                                        and t.Dyelot = fi.Dyelot)
                            , 0)
        , dbo.Getlocation(fi.ukey) as [Location]
        , #tmp.ToFactoryID
        , rtrim(#tmp.poid) ToPOID
        , rtrim(#tmp.seq1) ToSeq1
        , #tmp.seq2 ToSeq2
        , concat(Ltrim(Rtrim(#tmp.seq1)), ' ', #tmp.seq2) as toseq
        , fi.roll ToRoll
        , fi.dyelot ToDyelot
        , 'B' as [ToStockType]
        , dbo.Getlocation(fi.ukey) as [ToLocation]
        , GroupQty = Sum(fi.InQty - fi.OutQty + fi.AdjustQty) over (partition by #tmp.ToFactoryID, #tmp.poid, #tmp.seq1, #tmp.seq2, fi.dyelot)
from #tmp  
inner join dbo.FtyInventory fi WITH (NOLOCK) on fi.POID = InventoryPOID 
                                                and fi.seq1 = Inventoryseq1 
                                                and fi.seq2 = InventorySEQ2 
                                                and fi.StockType = 'I'
left join dbo.orders o WITH (NOLOCK) on o.id = fi.POID 
where fi.Lock = 0
Order by GroupQty desc, fromdyelot, balanceQty desc
drop table #tmp", Env.User.Keyword, this.dr_master["id"]));
                #endregion
                System.Data.SqlClient.SqlParameter sqlp1 = new System.Data.SqlClient.SqlParameter();
                sqlp1.ParameterName = "@poid";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
                sqlp1.Value = sp;
                paras.Add(sqlp1);

                this.ShowWaitMessage("Data Loading....");

                if (!SQL.Selects(string.Empty, strSQLCmd.ToString(), out this.dsTmp, paras))
                {
                    return;
                }

                DataTable TaipeiInput = this.dsTmp.Tables[0];
                this.dsTmp.Tables[0].TableName = "TaipeiInput";
                DataTable FtyDetail = this.dsTmp.Tables[1];
                this.dsTmp.Tables[1].TableName = "FtyDetail";
                foreach (DataRow dr in FtyDetail.Rows)
                {
                    string ToLocation = dr["ToLocation"].ToString();
                    string sqlcheckToLocation = string.Format(@"SELECT id FROM DBO.MtlLocation WITH (NOLOCK) WHERE StockType='B' and junk != '1' and id = '{0}'", ToLocation);
                    string checkToLocation = string.Empty;
                    checkToLocation = MyUtility.GetValue.Lookup(sqlcheckToLocation);
                    if (checkToLocation == string.Empty)
                    {
                        dr["ToLocation"] = string.Empty;
                    }
                }

                this.relation = new DataRelation(
                    "rel1",
                    new DataColumn[] { TaipeiInput.Columns["Poid"], TaipeiInput.Columns["seq1"], TaipeiInput.Columns["seq2"] },
                    new DataColumn[] { FtyDetail.Columns["toPoid"], FtyDetail.Columns["toseq1"], FtyDetail.Columns["toseq2"] });
                this.dsTmp.Relations.Add(this.relation);
                this.TaipeiInputBS.DataSource = this.dsTmp;
                this.TaipeiInputBS.DataMember = "TaipeiInput";
                this.FtyDetailBS.DataSource = this.dsTmp;
                this.FtyDetailBS.DataMember = "FtyDetail";

                TaipeiInput.Columns.Add("total_qty", typeof(decimal), "sum(child.qty)");
                TaipeiInput.Columns.Add("balanceqty", typeof(decimal), "Taipei_qty - accu_qty - sum(child.qty)");
                this.MyFilter();
                this.dtSort.Clear();
                this.Grid_ftyDetail_CellFormatting(null, null);
                this.HideWaitMessage();
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid_TaipeiInput.IsEditingReadOnly = true;
            this.grid_TaipeiInput.DataSource = this.TaipeiInputBS;
            this.Helper.Controls.Grid.Generator(this.grid_TaipeiInput)
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 0
                .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4)) // 1
                .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3)) // 2
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .Numeric("poqty", header: "PO Qty", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 5
                .Numeric("inputqty", header: "Input" + Environment.NewLine + "Qty", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 5
                .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .EditText("description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(16)) // 4
                .Date("lastest", header: "Taipei" + Environment.NewLine + "Last Output", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 6
                .Numeric("Taipei_qty", header: "Taipei" + Environment.NewLine + "Output", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 6
                .Numeric("accu_qty", header: "Accu." + Environment.NewLine + "Transfered", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 6
                .Numeric("total_qty", header: "Total" + Environment.NewLine + "Transfer", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 7
                .Numeric("balanceqty", header: "Balance", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 8
               ;

            this.grid_TaipeiInput.RowSelecting += (s, e) =>
            {
                if (e.RowIndex < 0 || !this.TaipeiInputBS.Current.GetType().FullName.Equals("System.Data.DataRowView"))
                {
                    return;
                }

                DataRow dr = this.grid_TaipeiInput.GetDataRow(e.RowIndex);
                this.FtyDetailBS.Filter = $" toPoid = '{dr["Poid"]}' and toSeq1 = '{dr["Seq1"]}' and toSeq2 = '{dr["Seq2"]}' ";
            };

            this.grid_ftyDetail.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.grid_ftyDetail.DataSource = this.FtyDetailBS;

            Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;

            #region -- Location 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentrow = this.grid_ftyDetail.GetDataRow(this.grid_ftyDetail.GetSelectedRowIndex());
                    Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation(currentrow["ToStocktype"].ToString(), currentrow["ToLocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    currentrow["ToLocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.grid_ftyDetail.GetDataRow(e.RowIndex);
                    dr["ToLocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", dr["ToStocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["ToLocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }

                    trueLocation.Sort();
                    dr["ToLocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            #endregion Location 右鍵開窗
            DataGridViewGeneratorTextColumnSettings ns2 = new DataGridViewGeneratorTextColumnSettings();
            ns2.CellFormatting = (s, e) =>
            {
                DataRow dr = this.grid_ftyDetail.GetDataRow(e.RowIndex);
                switch (dr["Fromstocktype"].ToString())
                {
                    case "B":
                        e.Value = "Bulk";
                        break;
                    case "I":
                        e.Value = "Inventory";
                        break;
                    case "O":
                        e.Value = "Scrap";
                        break;
                }
            };

            this.grid_ftyDetail.CellValueChanged += (s, e) =>
            {
                this.scrollnb = this.grid_ftyDetail.FirstDisplayedScrollingRowIndex;
                DataRow dr = this.grid_ftyDetail.GetDataRow(e.RowIndex);
                if (this.grid_ftyDetail.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    if (Convert.ToBoolean(dr["Selected"]) == true)
                    {
                        if (MyUtility.Check.Seek($@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{dr["toStocktype"]}'
        and junk != '1'
        and  id ='{dr["location"]}'
"))
                        {
                            dr["tolocation"] = dr["location"];
                        }
                        else
                        {
                            dr["tolocation"] = string.Empty;
                        }
                    }

                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        if (dr.GetParentRow("rel1") != null && !dr["balanceqty"].EqualDecimal(0))
                        {
                            decimal masterBalance, detailBalance;
                            decimal.TryParse(dr.GetParentRow("rel1")["balanceqty"].ToString(), out masterBalance);
                            decimal.TryParse(dr["balanceqty"].ToString(), out detailBalance);
                            dr["qty"] = (masterBalance > detailBalance) ? detailBalance : masterBalance;
                        }
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }
                }

                if (this.grid_ftyDetail.Columns[e.ColumnIndex].Name == this.col_Qty.Name)
                {
                    dr["selected"] = true;
                }

                dr.EndEdit();
                this.Sortdirect();
                this.grid_ftyDetail.FirstDisplayedScrollingRowIndex = this.scrollnb;
            };

            DataGridViewGeneratorCheckBoxColumnSettings sel = new DataGridViewGeneratorCheckBoxColumnSettings();

            this.Helper.Controls.Grid.Generator(this.grid_ftyDetail)
                .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1,  falseValue: 0, settings: sel).Get(out this.col_chk) // 0
                .Text("Frompoid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 0
                .Text("Fromseq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4)) // 1
                .Text("Fromseq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3)) // 2
                .Text("Fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 1
                .Text("Fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 2
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .Text("Fromstocktype", header: "Stock" + Environment.NewLine + "Type", iseditingreadonly: true, width: Widths.AnsiChars(8), settings: ns2) // 4
                .Numeric("accu_qty", header: "Accu." + Environment.NewLine + "Transfered", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(10)) // 5
                .Numeric("balanceqty", header: "Stock" + Environment.NewLine + "Balance", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 9
                .Numeric("qty", header: "Transfer" + Environment.NewLine + "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, maximum: 99999999, minimum: -99999999).Get(out this.col_Qty) // 10
                .Text("location", header: "From Location", iseditingreadonly: true) // 11
                .Text("tolocation", header: "To Location", iseditingreadonly: false, settings: ts2).Get(out col_tolocation) // 12
               ;
            this.col_Qty.DefaultCellStyle.BackColor = Color.Pink;
            col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
        }

        // Cancel
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckReturn_Click(object sender, EventArgs e)
        {
            this.MyFilter();
        }

        private void MyFilter()
        {
            if (this.checkReturn.CheckState == CheckState.Checked)
            {
                this.TaipeiInputBS.Filter = "taipei_qty <= accu_qty";
                this.FtyDetailBS.Filter = string.Empty;
            }
            else
            {
                this.TaipeiInputBS.Filter = "taipei_qty > accu_qty";

                // 為了觸發 Grid_ftyDetail_CellFormatting 事件
                this.FtyDetailBS.Filter = this.FtyDetailBS.Filter;
            }
        }

        private void BtnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            this.FtyDetailBS.EndEdit();
            DataRow dr = this.grid_TaipeiInput.GetDataRow(this.grid_TaipeiInput.GetSelectedRowIndex());
            if (dr != null)
            {
                var drs = dr.GetChildRows(this.relation);

                foreach (DataRow dr2 in drs)
                {
                    if (dr2["selected"].ToString() == "1")
                    {
                        dr2["tolocation"] = this.txtLocation.Text;
                    }
                }
            }
        }

        private void TxtLocation_MouseDown(object sender, MouseEventArgs e)
        {
            Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation("B", string.Empty);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtLocation.Text = item.GetSelectedString();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            StringBuilder warningmsg = new StringBuilder();

            this.grid_ftyDetail.ValidateControl();
            this.grid_ftyDetail.EndEdit();

            if (MyUtility.Check.Empty(this.dsTmp))
            {
                return;
            }

            DataRow[] drs;
            DataTable dt = this.dsTmp.Tables["TaipeiInput"];
            if (this.checkReturn.CheckState == CheckState.Checked)
            {
                drs = dt.Select("taipei_qty <= accu_qty");
            }
            else
            {
                drs = dt.Select("taipei_qty > accu_qty");
            }

            bool isSelect = false, isSelectNonQty = false;

            foreach (DataRow dr in drs)
            {
                var childRows = dr.GetChildRows(this.relation);
                if (childRows.Length == 0)
                {
                    continue;
                }

                DataTable child = childRows.CopyToDataTable();
                var dr2 = child.Select("selected=1");
                if (dr2.Length > 0)
                {
                    isSelect = true;
                }

                dr2 = child.Select("qty = 0 and Selected = 1");
                if (dr2.Length > 0)
                {
                    isSelectNonQty = true;
                }
            }

            if (!isSelect)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            if (isSelectNonQty)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            foreach (DataRow dr in drs)
            {
                var childRows = dr.GetChildRows(this.relation);
                if (childRows.Length == 0)
                {
                    continue;
                }

                DataTable child = childRows.CopyToDataTable();

                var dr2 = child.Select("qty <> 0 and Selected = 1");

                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["fromftyinventoryukey"].EqualString(tmp["fromftyinventoryukey"])
                                       && row["topoid"].EqualString(tmp["topoid"].ToString()) && row["toseq1"].EqualString(tmp["toseq1"])
                                       && row["toseq2"].EqualString(tmp["toseq2"].ToString()) && row["toroll"].EqualString(tmp["toroll"])
                                       && row["todyelot"].EqualString(tmp["todyelot"]) && row["tostocktype"].EqualString(tmp["tostocktype"])).ToArray();
                    if (findrow.Length > 0)
                    {
                        findrow[0]["qty"] = tmp["qty"];
                        findrow[0]["tolocation"] = tmp["tolocation"];
                    }
                    else
                    {
                        tmp["id"] = this.dr_master["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        this.dt_detail.ImportRow(tmp);
                    }
                }
            }

            this.Close();
        }

        // 將記憶的排序放到Grid裡
        private void Sortdirect()
        {
            this.sortFinal = true;
            ListSortDirection dir;
            for (int i = 0; i < this.dtSort.Rows.Count; i++)
            {
                dir = (this.dtSort.Rows[i]["Sort"].ToString() == "ASC") ? ListSortDirection.Ascending : ListSortDirection.Descending;
                this.grid_ftyDetail.Sort(this.grid_ftyDetail.Columns[this.dtSort.Rows[i]["Name"].ToString()], dir);
                this.grid_ftyDetail.EndEdit();
            }

            this.sortFinal = false;
        }

        // 將所有排序的欄位記憶起來
        private void Grid_ftyDetail_Sorted(object sender, EventArgs e)
        {
            if (this.sortFinal)
            {
                return;
            }

            DataRow dr;
            dr = this.dtSort.NewRow();
            dr["Name"] = this.grid_ftyDetail.SortedColumn.Name;
            dr["Sort"] = (this.grid_ftyDetail.SortOrder == SortOrder.Ascending) ? "ASC" : "DESC";
            this.dtSort.Rows.Add(dr);
        }

        private void Grid_ftyDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e != null && e.RowIndex > 0)
            {
                DataRow dr = this.grid_ftyDetail.GetDataRow<DataRow>(e.RowIndex);
                if (this.grid_ftyDetail.Columns[e.ColumnIndex].Name == "qty")
                {
                    this.col_Qty.DecimalPlaces = 2;
                    if (MyUtility.Convert.GetString(dr["stockunit"]).EqualString("PCS"))
                    {
                        this.col_Qty.DecimalPlaces = 0;
                    }
                }
            }
        }
    }
}
