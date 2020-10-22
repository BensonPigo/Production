using System;
using System.Collections.Generic;
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
    public partial class P22_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private DataSet dsTmp;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
        private DataRelation relation;

        /// <summary>
        /// Initializes a new instance of the <see cref="P22_Import"/> class.
        /// </summary>
        /// <param name="master">Master DataRow</param>
        /// <param name="detail">Detail Table</param>
        public P22_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                this.txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                #region -- Sql Command --

                bool mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
                string where = string.Empty;
                if (!mtlAutoLock)
                {
                    where = "where fi.Lock = 0";
                }

                strSQLCmd.Append(string.Format(
                    @"
with cte as 
(
    select rtrim(pd.id) as poid
    , rtrim(pd.seq1) seq1
    ,pd.seq2
    ,pd.Qty
    ,pd.ShipQty
    ,pd.StockQty
    ,pd.InputQty
    ,pd.OutputQty
    ,o.FactoryID ToFactoryID
    ,x.taipei_issue_date
    ,x.taipei_qty
    ,pd.POUnit
    ,pd.StockUnit  
    ,pd.Refno
	from dbo.PO_Supp_Detail pd WITH (NOLOCK) 
	inner join dbo.orders o WITH (NOLOCK) on o.id = pd.id
    inner join dbo.Factory f WITH (NOLOCK) on f.id = o.FtyGroup
	cross apply
	(select max(i.ConfirmDate) taipei_issue_date,sum(i.Qty) taipei_qty
		from dbo.Invtrans i WITH (NOLOCK) 
		where (i.type='1' OR I.TYPE='4') and i.InventoryPOID = pd.ID and i.InventorySeq1 = pd.seq1 and i.InventorySeq2 = pd.SEQ2
	) x
	where f.MDivisionID ='{0}' and pd.id = @poid --AND X.taipei_qty > 0
)
select  m.ToFactoryID
        , m.poid
        , m.seq1
        , m.seq2
        , m.StockUnit
        , dbo.GetUnitQty(POUnit, StockUnit, m.Qty) as poqty
        , dbo.GetUnitQty(POUnit, StockUnit, m.InputQty) as inputQty
        , m.Refno
        , dbo.getMtlDesc(poid,seq1,seq2,2,0) as [description]
        , m.taipei_issue_date
        , dbo.GetUnitQty(POUnit, StockUnit, m.taipei_qty) as taipei_qty 
        , m.POUnit
        , accu_qty 
into #tmp
from cte m 
outer apply
(select isnull(sum(qty) ,0) as accu_qty
	from (
		select sum(r2.StockQty) as qty from dbo.Receiving r1 WITH (NOLOCK) inner join dbo.Receiving_Detail r2 WITH (NOLOCK) on r2.Id= r1.Id 
			where r1.Status ='Confirmed' and r2.StockType = 'I' 
				and r2.PoId = m.poid and r2.seq1 = m.seq1 and r2.seq2 = m.seq2
		union 
		select sum(s2.Qty) as qty from dbo.SubTransfer s1 WITH (NOLOCK) inner join dbo.SubTransfer_Detail s2 WITH (NOLOCK) on s2.Id= s1.Id 
			where s1.type ='A' and s1.Status ='Confirmed' and s2.ToStockType = 'I' 
				and s2.ToPOID = m.poid and s2.ToSeq1 = m.seq1 and s2.ToSeq2 = m.seq2 and s1.Id !='{1}'
		) xx
  ) xxx;

select distinct cte.* 
from #tmp cte
inner join dbo.FtyInventory fi WITH (NOLOCK) 
on fi.POID = cte.poid 
and fi.seq1 = cte.seq1 
and fi.seq2 = cte.SEQ2 
and fi.StockType = 'B'
{2}
order by cte.poid,cte.seq1,cte.seq2 ;

select 0 AS selected,'' as id,o.FactoryID FromFactoryID
,fi.POID FromPOID
,fi.seq1 Fromseq1
,fi.seq2 Fromseq2
,concat(Ltrim(Rtrim(fi.seq1)), ' ', fi.seq2) as fromseq
,fi.roll FromRoll,fi.dyelot FromDyelot,fi.stocktype FromStockType,fi.Ukey as fromftyinventoryukey 
,fi.InQty,fi.OutQty,fi.AdjustQty
,fi.InQty - fi.OutQty + fi.AdjustQty as balanceQty
,0.00 as qty
,cte.StockUnit
,isnull((
            select inqty 
            from dbo.FtyInventory t WITH (NOLOCK) 
            where t.POID = fi.POID and t.seq1 = fi.seq1 and t.seq2 = fi.seq2 and t.StockType = 'I' 
	            and t.Roll = fi.Roll and t.Dyelot = fi.Dyelot
        ),0) as accu_qty
,dbo.Getlocation(fi.ukey) as [Location]
,rtrim(cte.poid) ToPOID
,rtrim(cte.seq1) ToSeq1
, cte.seq2 ToSeq2 
, cte.ToFactoryID
,fi.roll ToRoll,fi.dyelot ToDyelot,'I' as [ToStockType]
,stuff((select ',' + t1.MtlLocationID from (select MtlLocationid from dbo.FtyInventory_Detail f WITH (NOLOCK)  inner join MtlLocation m WITH (NOLOCK) on f.MtlLocationID=m.ID  where f.Ukey = fi.Ukey and m.StockType='I' and m.Junk !='1')t1 for xml path('')), 1, 1, '') as [ToLocation]
,GroupQty = Sum(fi.InQty - fi.OutQty + fi.AdjustQty) over(partition by cte.ToFactoryID,fi.POID,fi.seq1,fi.seq2,fi.dyelot)
,dbo.getMtlDesc(fi.poid, fi.seq1, fi.seq2, 2, 0) as [description]
from #tmp cte 
inner join dbo.FtyInventory fi WITH (NOLOCK) on 
                                                fi.POID = cte.poid 
                                                and fi.seq1 = cte.seq1 
                                                and fi.seq2 = cte.SEQ2 
                                                and fi.StockType = 'B'
left join dbo.orders o WITH (NOLOCK) on fi.poid=o.id 
{2}
Order by GroupQty desc,fromdyelot,balanceQty desc
drop table #tmp", Env.User.Keyword, this.dr_master["id"], where));
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

                DataTable taipeiInput = this.dsTmp.Tables[0];
                this.dsTmp.Tables[0].TableName = "TaipeiInput";
                DataTable ftyDetail = this.dsTmp.Tables[1];
                foreach (DataRow dr in ftyDetail.Rows)
                {
                    string toLocation = dr["ToLocation"].ToString();
                    string sqlcheckToLocation = string.Format(@"SELECT id FROM DBO.MtlLocation WITH (NOLOCK) WHERE StockType='I' and junk != '1' and id = '{0}'", toLocation);
                    string checkToLocation = string.Empty;
                    checkToLocation = MyUtility.GetValue.Lookup(sqlcheckToLocation);
                    if (checkToLocation == string.Empty)
                    {
                        dr["ToLocation"] = string.Empty;
                    }
                }

                this.relation = new DataRelation(
                    "rel1",
                    new DataColumn[] { taipeiInput.Columns["poid"], taipeiInput.Columns["seq1"], taipeiInput.Columns["seq2"] },
                    new DataColumn[] { ftyDetail.Columns["toPoid"], ftyDetail.Columns["toseq1"], ftyDetail.Columns["toseq2"] });
                this.dsTmp.Relations.Add(this.relation);
                this.TaipeiInputBS.DataSource = this.dsTmp;
                this.TaipeiInputBS.DataMember = "TaipeiInput";
                this.FtyDetailBS.DataSource = this.TaipeiInputBS;
                this.FtyDetailBS.DataMember = "rel1";

                taipeiInput.Columns.Add("total_qty", typeof(decimal), "sum(child.qty)");
                taipeiInput.Columns.Add("balanceqty", typeof(decimal), "Taipei_qty - accu_qty - sum(child.qty)");
                this.MyFilter();
                this.HideWaitMessage();
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid_TaipeiInput.IsEditingReadOnly = true;

            // this.grid_TaipeiInput.AutoGenerateColumns = true;
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
                .Date("taipei_issue_date", header: "Taipei" + Environment.NewLine + "Input Date", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 6
                .Numeric("Taipei_qty", header: "Taipei" + Environment.NewLine + "Input", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 6
                .Numeric("accu_qty", header: "Accu." + Environment.NewLine + "Transfered", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 6
                .Numeric("total_qty", header: "Total" + Environment.NewLine + "Transfer", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 7
                .Numeric("balanceqty", header: "Balance", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 8
               ;
            this.grid_ftyDetail.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.grid_ftyDetail.DataSource = this.FtyDetailBS;

            Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;

            #region -- transfer qty valid --
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow currentrow = this.grid_ftyDetail.GetDataRow(this.grid_ftyDetail.GetSelectedRowIndex());
                    currentrow["qty"] = e.FormattedValue;
                    currentrow["selected"] = true;
                }
            };
            #endregion
            #region -- Location 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentrow = this.grid_ftyDetail.GetDataRow(e.RowIndex);
                    Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation(currentrow["ToStocktype"].ToString(), currentrow["ToLocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    currentrow["tolocation"] = item.GetSelectedString();
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

            this.grid_ftyDetail.CellValueChanged += (s, e) =>
            {
                if (this.grid_ftyDetail.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.grid_ftyDetail.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true)
                    {
                        if (MyUtility.Check.Seek($@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{dr["tostocktype"]}'
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

                    dr.EndEdit();
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid_ftyDetail)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("Fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 1
                .Text("Fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 2
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .Text("Fromstocktype", header: "Stock" + Environment.NewLine + "Type", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 4
                .Numeric("accu_qty", header: "Accu." + Environment.NewLine + "Transfered", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(10)) // 5
                .Numeric("inqty", header: "Stock" + Environment.NewLine + "In", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 6
                .Numeric("outqty", header: "Stock" + Environment.NewLine + "Out", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 7
                .Numeric("adjustqty", header: "Stock" + Environment.NewLine + "Adjust", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 8
                .Numeric("balanceqty", header: "Stock" + Environment.NewLine + "Balance", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)) // 9
                .Numeric("qty", header: "Transfer" + Environment.NewLine + "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, settings: ns).Get(out this.col_Qty) // 10
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

                // FtyDetailBS.Filter = "balanceQty > 0";
            }
            else
            {
                this.TaipeiInputBS.Filter = "taipei_qty > accu_qty";

                // FtyDetailBS.Filter = "outqty >0";
            }
        }

        private void BtnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            this.FtyDetailBS.EndEdit();
            DataRow dr = this.grid_TaipeiInput.GetDataRow(this.grid_TaipeiInput.GetSelectedRowIndex());
            if (dr == null)
            {
                return;
            }

            var drs = dr.GetChildRows(this.relation);

            foreach (DataRow dr2 in drs)
            {
                if (dr2["selected"].ToString() == "1")
                {
                    dr2["tolocation"] = this.txtLocation.Text;
                }
            }
        }

        private void TxtLocation_MouseDown(object sender, MouseEventArgs e)
        {
            Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation("I", string.Empty);
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

            DataRow[] drs;
            if (MyUtility.Check.Empty(this.dsTmp))
            {
                return;
            }

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

        private void Grid_ftyDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
