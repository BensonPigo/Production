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
    public partial class P32_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtBorrow;
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private int grid2SelectIndex = 0;

        public P32_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
            this.di_stocktype.Add("O", "Scrap");
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        // Form Load

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            string sqlcmd;
            base.OnFormLoaded();

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            this.gridFromPoId.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridFromPoId.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridFromPoId)
                .Text("FromPoId", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 0
                .Text("fromseq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(2)) // 1
                .Text("fromseq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(2)) // 2
                .ComboBox("fromstockType", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false, width: Widths.AnsiChars(6)).Get(out cbb_stocktype) // 3
                .Numeric("BorrowQty", header: "Borrow" + Environment.NewLine + "Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 4
                .Numeric("AccuReturn", header: "Accu." + Environment.NewLine + "Return", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 5
                .Numeric("balance", header: "Accu. Diff." + Environment.NewLine + "ReturnQty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 6
                .Numeric("ReturnQty", header: "Return" + Environment.NewLine + "Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6));
            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            #region -- 依借料單號搜尋內容 --
            sqlcmd = string.Format(
                @"
;with cte1 as(
    select  bd.FromPoId
            , bd.FromSeq1
            , bd.FromSeq2
            , bd.FromStocktype
            , qty = sum(bd.qty) 
    from borrowback_detail bd WITH (NOLOCK) 
	inner join Orders orders on bd.FromPOID = orders.ID
	inner join Factory factory on orders.FtyGroup = factory.ID
    where bd.id = '{0}' and factory.MDivisionID = '{1}'  and orders.Category <> 'A'
    group by bd.FromPoId, bd.FromSeq1, bd.FromSeq2, bd.FromStocktype
    )
,cte2 as(
    select  bd.ToPoid
            , bd.ToSeq1
            , bd.ToSeq2
            , bd.ToStocktype
            , qty = sum(bd.qty) 
    from borrowback b WITH (NOLOCK) 
    inner join borrowback_detail bd WITH (NOLOCK) on b.Id= bd.ID 
	inner join Orders orders on bd.FromPOID = orders.ID
	inner join Factory factory on orders.FtyGroup = factory.ID
    where b.BorrowId='{0}' and b.Status = 'Confirmed' and factory.MDivisionID = '{1}'
    group by bd.ToPoid, bd.ToSeq1, bd.ToSeq2, bd.ToStocktype
    )
select  cte1.FromPoId
        , cte1.FromSeq1
        , cte1.FromSeq2
        , fromseq = concat(Ltrim(Rtrim(cte1.FromSeq1)), ' ', cte1.FromSeq2) 
        , cte1.FromStocktype
        , BorrowQty = cte1.qty
        , AccuReturn = isnull(cte2.qty,0.00) 
        , ReturnQty = 0.00
        , balance = cte1.qty - isnull(cte2.qty,0.00) 
from cte1 
left join cte2 on cte2.ToPoid = cte1.FromPoId and cte2.ToSeq1 = cte1.FromSeq1 and cte2.ToSeq2 =  cte1.FromSeq2 
    and cte2.ToStocktype = cte1.FromStocktype;
", this.dr_master["BorrowId"], Env.User.Keyword);
            DataTable datas;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out datas);

            this.listControlBindingSource2.DataSource = datas;

            #endregion

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CheckBorrowReturnQty(e.RowIndex, Convert.ToDecimal(e.FormattedValue));
                    }
                };

            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (this.gridImport.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        this.CheckBorrowReturnQty(e.RowIndex, Convert.ToDecimal(dr["balance"]));
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        this.CheckBorrowReturnQty(e.RowIndex, Convert.ToDecimal(0));
                    }

                    dr.EndEdit();
                }
            };

            // Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("frompoid", header: "From" + Environment.NewLine + "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 1
                .Text("fromseq", header: "From" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 2
                .Text("FromRoll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .Text("FromDyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 4
                .Text("location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(10)) // 5
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) // 6
                .Text("StockUnit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(4)) // 7
                .Text("toseq", header: "To" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 8
                .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6)) // 9
                .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 10
                .Numeric("AccuDiffQty", header: "Accu. Diff. Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 11
                .Numeric("qty", header: "Issue" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6)) // 12
                .ComboBox("FromStocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype) // 13
                ;

            this.gridImport.Columns["ToRoll"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            #region -- Return Mtl List --
            sqlcmd = string.Format(
                @"
;with cte1 as(
    select  bd.FromPoId
            , bd.FromSeq1
            , bd.FromSeq2
            , bd.FromStocktype
                  , FromFactoryID = orders.FactoryID
            , qty = sum(bd.qty) 
            ,bd.ToPOID
    from borrowback_detail bd WITH (NOLOCK) 
      inner join Orders orders on bd.FromPOID = orders.ID
      inner join Factory factory on orders.FtyGroup = factory.ID
    where bd.id = '{0}' and factory.MDivisionID = '{1}' and orders.Category <> 'A'
    group by bd.FromPoId, bd.FromSeq1, bd.FromSeq2, bd.FromStocktype, orders.FactoryID ,bd.ToPOID
    )
,cte2 as(
    select  bd.ToPoid
            , bd.ToSeq1
            , bd.ToSeq2
            , bd.ToStocktype
            , qty = sum(bd.qty) 
    from borrowback b WITH (NOLOCK) 
    inner join borrowback_detail bd WITH (NOLOCK) on b.Id= bd.ID 
      inner join Orders orders on bd.FromPOID = orders.ID
      inner join Factory factory on orders.FtyGroup = factory.ID
    where b.BorrowId='{0}' and b.Status = 'Confirmed' and factory.MDivisionID = '{1}'
    group by bd.ToPoid, bd.ToSeq1, bd.ToSeq2, bd.ToStocktype
    )
select  cte1.FromPoId
        , cte1.FromSeq1
        , cte1.FromSeq2 
        , cte1.FromStocktype
		, cte1.FromFactoryID
		,psd.Refno, psd.SizeSpec, psd.ColorID ,psd.BrandId,cte1.ToPOID
into #tmp
from cte1 
left join cte2 on cte2.ToPoid = cte1.FromPoId and cte2.ToSeq1 = cte1.FromSeq1 and cte2.ToSeq2 =  cte1.FromSeq2 
    and cte2.ToStocktype = cte1.FromStocktype
INNER JOIN PO_Supp_Detail psd ON psd.ID = cte1.FromPoId and psd.Seq1 = cte1.FromSeq1 and psd.Seq2 =  cte1.FromSeq2 ;

SELECT DISTINCT Refno, SizeSpec, ColorID ,BrandId INTO #tmp2 FROM #tmp

select  distinct selected = 0 
        , id = '' 
        , FromFtyinventoryUkey = c.ukey 
        , FromPoId = p.id 
        , FromSeq1 = p.Seq1 
        , FromSeq2 = p.Seq2 
        , fromseq =concat(Ltrim(Rtrim(p.Seq1)), ' ', p.Seq2) 
        , FromDyelot = c.dyelot 
        , FromRoll = c.roll 
        , FromFactoryID = orders.FactoryID
        , FromStocktype = c.StockType 
        , AccuDiffQty = c.InQty - c.OutQty + c.AdjustQty 
        , balance = c.InQty - c.OutQty + c.AdjustQty 
        , qty = 0.00 
        , tostocktype = bd.FromStocktype  
        , topoid = bd.FromPoId 
        , toseq1 = bd.FromSeq1 
        , toseq2 = bd.FromSeq2 
        , toRoll = iif (toSP.Roll is not null, toSP.Roll, c.roll)
        , toDyelot = iif (toSP.Roll is not null, toSP.Dyelot, c.dyelot)
        , ToFactoryID = #tmp.FromFactoryID
        , toseq = concat(Ltrim(Rtrim(bd.FromSeq1)), ' ', bd.FromSeq2) 
        , location = dbo.Getlocation(c.ukey)
        , [description] = dbo.getMtlDesc(bd.topoid,bd.toseq1,bd.toseq2,2,0) 
        , p.StockUnit
        , p.FabricType
from dbo.BorrowBack_Detail as bd WITH (NOLOCK) 
INNER join #tmp on bd.FromPoId = #tmp.FromPOID and bd.FromSeq1 = #tmp.FromSeq1 and bd.FromSeq2 = #tmp.FromSeq2
INNER join PO_Supp_Detail p WITH (NOLOCK) on p.ID= #tmp.ToPOID AND #tmp.BrandId=p.BrandId  and #tmp.Refno=p.Refno and #tmp.ColorID=p.ColorID  and #tmp.SizeSpec=p.SizeSpec
INNER join ftyinventory c WITH (NOLOCK) on  p.id = c.poid and  p.Seq1  = c.seq1 and  p.Seq2 = c.seq2 --and bd.toDyelot = c.Dyelot
INNER join Orders orders on c.POID = orders.ID
INNER join Factory factory on orders.FtyGroup = factory.ID
OUTER apply(
	select	Top 1 Roll
			, Dyelot
	From FtyInventory
	where	POID =p.id
			and Seq1 = p.SEQ1
			and Seq2 = p.SEQ2
			and Roll = c.Roll
            and Dyelot = c.Dyelot
) toSP
where bd.id='{0}' and c.lock = 0 and c.inqty-c.OutQty+c.AdjustQty > 0 and factory.MDivisionID = '{1}' and orders.Category <> 'A'
      and  c.StockType='B'
drop table #tmp,#tmp2
", this.dr_master["BorrowId"], Env.User.Keyword);

            DataTable grid1Datas;
            result = DBProxy.Current.Select(null, sqlcmd, out grid1Datas);

            this.listControlBindingSource1.DataSource = grid1Datas;
            #endregion
        }

        // Close
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            StringBuilder warningmsg = new StringBuilder();
            this.dtBorrow = this.gridImport.GetTable();

            this.gridImport.ValidateControl();
            this.gridImport.EndEdit();

            if (MyUtility.Check.Empty(this.dtBorrow) || this.dtBorrow.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = this.dtBorrow.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = this.dtBorrow.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = this.dtBorrow.Select("qty <> 0 and Selected = 1");
            foreach (DataRow row in dr2)
            {
                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["toroll"]) || MyUtility.Check.Empty(row["todyelot"])))
                {
                    warningmsg.Append(string.Format(
                        @"To SP#: {0} To Seq#: {1}-{2} To Roll#:{3} To Dyelot:{4} Roll and Dyelot can't be empty",
                        row["topoid"], row["toseq1"], row["toseq2"], row["toroll"], row["todyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["toroll"] = string.Empty;
                    row["todyelot"] = string.Empty;
                }

                if (decimal.Parse(row["balance"].ToString()) < decimal.Parse(row["qty"].ToString()))
                {
                    warningmsg.Append(string.Format(
                        @"From SP#: {0} From Seq#: {1}-{2} From Roll#:{3} From Dyelot:{4} Issue Qty can't over Stock Qty!",
                        row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["fromftyinventoryukey"].EqualString(tmp["fromftyinventoryukey"])
                                       && row["topoid"].EqualString(tmp["topoid"].ToString()) && row["toseq1"].EqualString(tmp["toseq1"])
                                       && row["toseq2"].EqualString(tmp["toseq2"].ToString()) && row["toroll"].EqualString(tmp["toroll"])
                                       && row["todyelot"].EqualString(tmp["todyelot"]) && row["tostocktype"].EqualString(tmp["tostocktype"])).ToArray();

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }

        private bool CheckAndShowInfo(string sp, string seq1, string seq2)
        {
            this.displaySizeSpec.Value = string.Empty;
            this.displayRefno.Value = string.Empty;
            this.displayColorID.Value = string.Empty;
            this.editDesc.Text = string.Empty;

            DataRow tmp;
            if (!MyUtility.Check.Seek(
                string.Format(
                @"
select  sizespec
        ,refno
        ,colorid
        ,[description] = dbo.getmtldesc(id,seq1,seq2,2,0) 
from po_supp_detail WITH (NOLOCK) 
where id ='{0}' and seq1 = '{1}' and seq2 = '{2}'", sp, seq1, seq2), out tmp, null))
            {
                MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                return true;
            }
            else
            {
                this.displaySizeSpec.Value = tmp["sizespec"];
                this.displayRefno.Value = tmp["refno"];
                this.displayColorID.Value = tmp["colorid"];
                this.editDesc.Text = tmp["description"].ToString();
                return false;
            }
        }

        private void Grid2_RowSelecting(object sender, Ict.Win.UI.DataGridViewRowSelectingEventArgs e)
        {
            this.grid2SelectIndex = e.RowIndex;
            DataRow dr = this.gridFromPoId.GetDataRow(this.grid2SelectIndex);
            this.CheckAndShowInfo(dr["FromPoid"].ToString(), dr["FromSeq1"].ToString(), dr["FromSeq2"].ToString());

            this.listControlBindingSource1.Filter = string.Format("ToPoid = '{0}' and ToSeq1 = '{1}' and ToSeq2 = '{2}'", dr["FromPoid"].ToString(), dr["FromSeq1"].ToString(), dr["FromSeq2"].ToString());
        }

        private void Grid1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.gridImport.ValidateControl();
        }

        private void CheckBorrowReturnQty(int rowIndex, decimal qty)
        {
            // check 判斷總數在 Stock or Return 超過
            bool checkGrid2Return = false, checkReturnQty = false;

            // 紀錄 Stock and Return 總數
            decimal sumGrid2eturn = 0, sumReturn = 0;

            // Get Grid1 目前選取的Data
            DataRow grid1Dr = this.gridImport.GetDataRow(rowIndex);
            grid1Dr["Qty"] = 0;

            #region Grid2.Return + Grid2.AccuReturn <= BorrowQty
            DataRow grid2Dr = this.gridFromPoId.GetDataRow(this.grid2SelectIndex);

            DataRow[] findrow = this.gridImport.GetTable().Select(string.Format("toPoid = '{0}' and toSeq1 = '{1}' and toSeq2 = '{2}' and toStockType = '{3}'", grid2Dr["FromPoid"], grid2Dr["FromSeq1"], grid2Dr["FromSeq2"], grid2Dr["FromStockType"]));
            foreach (DataRow dr in findrow)
            {
                sumGrid2eturn += Convert.ToDecimal(dr["qty"]);
            }

            if ((sumGrid2eturn + Convert.ToDecimal(qty)) + Convert.ToDecimal(grid2Dr["AccuReturn"]) <= Convert.ToDecimal(grid2Dr["BorrowQty"]))
            {
                checkGrid2Return = true;
            }

            findrow = null;
            #endregion

            #region sum(Grid1.Qty) <= balance
            findrow = this.gridImport.GetTable().Select(string.Format(
                "toPoid = '{0}' and toSeq1 = '{1}' and toSeq2 = '{2}' and toRoll = '{3}' and toDyelot = '{4}' and FromStockType = '{5}'",
                grid1Dr["toPoid"], grid1Dr["toSeq1"], grid1Dr["toSeq2"], grid1Dr["toRoll"], grid1Dr["toDyelot"], grid1Dr["FromStockType"]));
            foreach (DataRow dr in findrow)
            {
                sumReturn += Convert.ToDecimal(dr["qty"]);
            }

            if ((sumReturn + Convert.ToDecimal(qty)) <= Convert.ToDecimal(grid1Dr["balance"]))
            {
                checkReturnQty = true;
            }
            #endregion

            if (checkReturnQty & checkGrid2Return)
            {
                grid1Dr["Qty"] = qty;

                foreach (DataRow dr in findrow)
                {
                    dr["AccuDiffQty"] = Convert.ToDecimal(grid1Dr["balance"]) - (sumReturn + Convert.ToDecimal(grid1Dr["Qty"]));
                }

                grid2Dr["ReturnQty"] = sumGrid2eturn + Convert.ToDecimal(grid1Dr["Qty"]);
            }
            else
            {
                string errStr = string.Empty;
                errStr += checkGrid2Return ? string.Empty : string.Format("<ReturnQty> : {0} can't more than <Accu. Diff. ReturnQty> : {1}\n", sumGrid2eturn + Convert.ToDecimal(qty), grid2Dr["balance"]);
                errStr += checkReturnQty ? string.Empty : string.Format("<Accu. Qty> : {0} can't more than <StockQty> : {1}", sumReturn + Convert.ToDecimal(qty), grid1Dr["balance"]);

                grid1Dr["Qty"] = 0;

                foreach (DataRow dr in findrow)
                {
                    dr["AccuDiffQty"] = Convert.ToDecimal(grid1Dr["balance"]) - (sumReturn + Convert.ToDecimal(grid1Dr["Qty"]));
                }

                grid2Dr["ReturnQty"] = sumGrid2eturn + Convert.ToDecimal(grid1Dr["Qty"]);
                MyUtility.Msg.InfoBox(errStr);
            }

            grid1Dr["selected"] = Convert.ToDecimal(grid1Dr["Qty"]) > 0;

            this.gridImport.RefreshEdit();
            this.gridFromPoId.RefreshEdit();
        }
    }
}
