using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicForm;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P29 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk2 = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
        private DataTable master;
        private DataTable detail;
        private Msg p29_msg = new Msg();

        /// <summary>
        /// Initializes a new instance of the <see cref="P29"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P29(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboCategory.SelectedIndex = 0;

            #region -- Grid1 設定 --
            this.gridComplete.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridComplete.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridComplete)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk)
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
                 .Text("TransID", header: "Trans. ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                  ;
            #endregion
            this.col_chk.CellClick += (s, e) =>
            {
                DataRow thisRow = this.gridComplete.GetDataRow(this.listControlBindingSource1.Position);
                if (thisRow == null)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    if ((bool)this.gridComplete.Rows[0].Cells[e.ColumnIndex].Value)
                    {
                        foreach (DataRow dr in this.detail.Rows)
                        {
                            dr["selected"] = false;
                        }
                    }
                }
                else
                {
                    if ((bool)this.gridComplete.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
                    {
                        thisRow["total_qty"] = DBNull.Value;
                        foreach (DataRow dr in thisRow.GetChildRows("rel1"))
                        {
                            dr["selected"] = false;
                            dr["qty"] = 0.00;
                        }
                    }
                }

                this.gridComplete.ValidateControl();
            };
            Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;
            #region -- transfer qty valid --
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings
            {
                IsSupportNegative = true,
            };
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow thisRow = this.gridComplete.GetDataRow(this.listControlBindingSource1.Position);
                    DataRow[] curentgridrowChild = thisRow.GetChildRows("rel1");
                    DataRow currentrow = this.gridRel.GetDataRow(this.gridRel.GetSelectedRowIndex());
                    currentrow["qty"] = e.FormattedValue;
                    currentrow.GetParentRow("rel1")["total_qty"] = curentgridrowChild.Sum(row => (decimal)row["qty"]);
                    if (Convert.ToDecimal(e.FormattedValue) != 0)
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
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridRel.GetDataRow(this.gridRel.GetSelectedRowIndex());
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(dr["tostocktype"].ToString(), dr["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.gridRel.GetDataRow(e.RowIndex);
                    dr["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
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
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueLocation.Sort();
                    dr["tolocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            #endregion
            #region -- Grid2 設定 --
            this.gridRel.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridRel.DataSource = this.listControlBindingSource2;

            this.gridRel.CellValueChanged += (s, e) =>
            {
                if (this.gridRel.Columns[e.ColumnIndex].Name == this.col_chk2.Name)
                {
                    DataRow dr = this.gridRel.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true)
                    {
                        if (MyUtility.Check.Seek($@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{dr["tostocktype"]}'
        and junk != '1'
        and  id ='{dr["fromlocation"]}'
"))
                        {
                            dr["tolocation"] = dr["fromlocation"];
                        }
                        else
                        {
                            dr["tolocation"] = string.Empty;
                        }
                    }

                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balanceQty"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }

                    dr.EndEdit();

                    DataRow thisRow = this.gridComplete.GetDataRow(this.listControlBindingSource1.Position);
                    DataRow[] curentgridrowChild = thisRow.GetChildRows("rel1");
                    DataRow currentrow = this.gridRel.GetDataRow(this.gridRel.GetSelectedRowIndex());
                    currentrow.GetParentRow("rel1")["total_qty"] = curentgridrowChild.Sum(row => (decimal)row["qty"]);
                    currentrow.EndEdit();
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridRel)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk2)
                 .Text("fromroll", header: "Roll#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Numeric("balanceQty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("qty", header: "Trans. Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, settings: ns).Get(out this.col_Qty)
                 .Text("fromlocation", header: "From Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: true)
                 .Text("tolocation", header: "To Bulk" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: false, settings: ts2).Get(out col_tolocation)
                  ;
            this.col_Qty.DefaultCellStyle.BackColor = Color.Pink;
            col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            this.Chp();
        }

        private void Chp()
        {
            #region selected
            this.col_chk2.CellClick += (s, e) =>
            {
                DataRow thisRow = this.gridRel.GetDataRow(this.listControlBindingSource2.Position);
                if (thisRow == null)
                {
                    return;
                }

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

                                // thisRow.GetParentRow("rel1")["total_qty"] = temp.Sum(row => (decimal)row["qty"]);
                                thisRow.GetParentRow("rel1")["total_qty"] = DBNull.Value;
                            }
                            else
                            {
                                thisRow.GetParentRow("rel1")["total_qty"] = temp.Sum(row => (decimal)row["qty"]);
                            }
                        }
                    }
                }

                this.gridRel.ValidateControl();
                this.gridComplete.ValidateControl();
            };

            #endregion
        }

        private DataSet dataSet;

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.dataSet = null;
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource2.DataSource = null;

            int selectindex = this.comboCategory.SelectedIndex;
            string mT = this.comboxMaterialTypeAndID.comboMaterialType.SelectedValue.ToString();
            string mtlTypeID = this.comboxMaterialTypeAndID.comboMtlTypeID.SelectedValue.ToString();
            string cuttingInline_b, cuttingInline_e, orderCfmDate_b, orderCfmDate_e, invCfmDate_s, invCfmDate_e, sP1, sP2, stockSP1, stockSP2, projectID, factory;
            cuttingInline_b = null;
            cuttingInline_e = null;
            orderCfmDate_b = null;
            orderCfmDate_e = null;
            invCfmDate_s = null;
            invCfmDate_e = null;
            sP1 = this.txtIssueSP1.Text;
            sP2 = this.txtIssueSP2.Text;
            stockSP1 = this.txtStockSP1.Text;
            stockSP2 = this.txtStockSP2.Text;
            projectID = this.txtProjectID.Text;
            factory = this.txtmfactory.Text;

            if (this.dateCuttingInline.Value1 != null)
            {
                cuttingInline_b = this.dateCuttingInline.Text1;
            }

            if (this.dateCuttingInline.Value2 != null)
            {
                cuttingInline_e = this.dateCuttingInline.Text2;
            }

            if (this.dateOrderCfmDate.Value1 != null)
            {
                orderCfmDate_b = this.dateOrderCfmDate.Text1;
            }

            if (this.dateOrderCfmDate.Value2 != null)
            {
                orderCfmDate_e = this.dateOrderCfmDate.Text2;
            }

            if (this.dateInventoryCfm.Value1 != null)
            {
                invCfmDate_s = this.dateInventoryCfm.Value1.Value.ToAppDateTimeFormatString();
            }

            if (this.dateInventoryCfm.Value2 != null)
            {
                invCfmDate_e = this.dateInventoryCfm.Value2.Value.AddDays(1).AddSeconds(-1).ToAppDateTimeFormatString();
            }

            #region ConfirmDate條件才會使用的SQL
            string sqlcmdInv = $@"
SELECT  DISTINCT 
         InventoryPOID
        ,InventorySeq1
        ,InventorySeq2
        ,POID
        ,Type
        ,Seq70Poid
        ,Seq70Seq1
        ,Seq70Seq2
        ,qty
INTO #tmpInvtrans
FROM dbo.Invtrans i WITH (NOLOCK) 
WHERE 1=1
";

            string invCfmDate_Where = string.Empty;
            string sqlcmdInMainSQL = $@"
            AND pd.ID IN (SELECT POID FROM #tmpInvtrans)
			AND pd.StockPOID IN (SELECT InventoryPOID FROM #tmpInvtrans)";

            if (!string.IsNullOrEmpty(invCfmDate_s))
            {
                invCfmDate_Where += $"      AND i.ConfirmDate >= '{invCfmDate_s}'" + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(invCfmDate_e))
            {
                invCfmDate_Where += $"      AND i.ConfirmDate <= '{invCfmDate_e}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(sP1))
            {
                if (!MyUtility.Check.Empty(sP2))
                {
                    invCfmDate_Where += $"      AND POID BETWEEN '{sP1}' and '{sP2}'" + Environment.NewLine;
                }
                else
                {
                    invCfmDate_Where += $"      AND POID = '{sP1}' " + Environment.NewLine;
                }
            }

            if (!MyUtility.Check.Empty(stockSP1))
            {
                if (!MyUtility.Check.Empty(stockSP2))
                {
                    invCfmDate_Where += $"      AND InventoryPOID BETWEEN '{stockSP1}' and '{stockSP2}'" + Environment.NewLine;
                }
                else
                {
                    invCfmDate_Where += $"      AND InventoryPOID = '{stockSP1}'" + Environment.NewLine;
                }
            }

            #endregion

            if ((cuttingInline_b == null && cuttingInline_e == null) &&
                 MyUtility.Check.Empty(sP1) && MyUtility.Check.Empty(stockSP1) && MyUtility.Check.Empty(projectID) &&
                (orderCfmDate_b == null && orderCfmDate_e == null) &&
                (invCfmDate_s == null && invCfmDate_e == null))
            {
                MyUtility.Msg.WarningBox("< Project ID >\r\n< Cutting Inline >\r\n< Order Confirm Date >\r\n< Issue SP# >\r\n< Stock SP# >\r\ncan't be empty!!");
                this.txtIssueSP1.Focus();
                return;
            }

            StringBuilder sqlcmd = new StringBuilder();
            #region -- sql command --
            sqlcmd.Append($@"
{(!string.IsNullOrEmpty(invCfmDate_s) && !string.IsNullOrEmpty(invCfmDate_e) ? sqlcmdInv + invCfmDate_Where : string.Empty)}

;with cte as (
    select  convert(bit,0) as selected
            , iif(y.cnt > 0 ,'Y','') complete
            , f.MDivisionID
            , rtrim(o.id) poid
            , o.Category
            , o.FtyGroup
            , o.CFMDate
            , o.CutInLine
            , o.ProjectID
            , o.FactoryID 
            , rtrim(pd.seq1) seq1
            , pd.seq2
            , pd.StockPOID
            , pd.StockSeq1
            , pd.StockSeq2
            , ROUND(dbo.GetUnitQty(pd.POUnit, pd.StockUnit, x.taipei_qty), 2) N'PoQty'
            , pd.POUnit
            , pd.StockUnit
            , InQty = isnull(xx.InQty,0)
    from View_WH_Orders o WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = o.ID
    inner join dbo.Factory f WITH (NOLOCK) on f.id = o.FtyGroup
    inner join dbo.Factory checkProduceFty With (NoLock) on o.FactoryID = checkProduceFty.ID 
    left join Fabric WITH (NOLOCK) on pd.SCIRefno = fabric.SCIRefno
    outer apply (
        select  count(1) cnt 
        from FtyInventory fi WITH (NOLOCK) 
        left join FtyInventory_Detail fid WITH (NOLOCK) on fid.Ukey = fi.Ukey 
	    where   fi.POID = pd.stockpoID 
                and fi.Seq1 = pd.stockSeq1 
                and fi.Seq2 = pd.stockSeq2 
                and fi.StockType = 'I' 
	            and fid.MtlLocationID is not null 
                and fi.Lock = 0
                and fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty > 0
    ) y--Detail有MD為null數量,沒有則為0,沒資料也為0
    cross apply (
        select  sum(iif(i.type='2',i.qty,0-i.qty)) taipei_qty 
        from {(!string.IsNullOrEmpty(invCfmDate_s) && !string.IsNullOrEmpty(invCfmDate_e) ? "#tmpInvtrans" : "dbo.Invtrans")}
                i WITH (NOLOCK) 
        where   i.InventoryPOID = pd.StockPOID 
                and i.InventorySeq1 = pd.StockSeq1 
                and i.PoID = pd.ID 
                and i.InventorySeq2 = pd.StockSeq2 
                and (i.type='2' or i.type='6')
                and Seq70Poid = rtrim(o.id)
                and Seq70Seq1 = rtrim(pd.seq1)
                and Seq70Seq2 = pd.seq2                
    ) x -- 需要轉的數量
    cross apply (
	    select sum(s2.Qty) as InQty 
        from dbo.SubTransfer s1 WITH (NOLOCK) 
        inner join dbo.SubTransfer_Detail s2 WITH (NOLOCK) on s2.Id= s1.Id 
	    where   s1.type ='B' 
                and s1.Status ='Confirmed' 
                and s2.ToStockType = 'B' 
                and s2.ToPOID = pd.id 
                and s2.ToSeq1 = pd.seq1 
                and s2.ToSeq2 = pd.seq2 
    ) xx --已轉的數量
    where exists 
          (
            select 1 
	        from View_WH_Orders o2 WITH (NOLOCK)
	        inner join Factory f WITH (NOLOCK) on o2.FactoryID = f.ID
	        where pd.StockPOID = o2.ID
	        and f.IsProduceFty = 1
          )
          and pd.seq1 like '7%' 
          and f.MDivisionID = '{Env.User.Keyword}'
          and checkProduceFty.IsProduceFty = '1'            
			{(!string.IsNullOrEmpty(invCfmDate_s) && !string.IsNullOrEmpty(invCfmDate_e) ? sqlcmdInMainSQL : string.Empty)}
            ");

            #region -- 條件 --
            switch (selectindex)
            {
                case 0:
                    sqlcmd.Append(@" 
            and (o.Category = 'B')");
                    break;
                case 1:
                    sqlcmd.Append(@" 
            and o.Category = 'S' ");
                    break;
                case 2:
                    sqlcmd.Append(@" 
            and (o.Category = 'M')");
                    break;
                case 3:
                    sqlcmd.Append(@" 
            and (o.Category = 'B' or o.Category = 'S' or o.Category = 'M')");
                    break;
            }

            if (!MyUtility.Check.Empty(mT))
            {
                if (mT != "All")
                {
                    sqlcmd.Append(string.Format(" and pd.FabricType = '{0}'", mT));
                }
            }

            if (!MyUtility.Check.Empty(mtlTypeID))
            {
                sqlcmd.Append(string.Format(" and fabric.MtlTypeID = '{0}'", mtlTypeID));
            }

            if (!MyUtility.Check.Empty(sP1))
            {
                if (!MyUtility.Check.Empty(sP2))
                {
                    sqlcmd.Append(string.Format(@"and pd.id between '{0}' and '{1}'", sP1, sP2));
                }
                else
                {
                    sqlcmd.Append(string.Format(@"and pd.id = '{0}'", sP1));
                }
            }

            if (!MyUtility.Check.Empty(stockSP1))
            {
                if (!MyUtility.Check.Empty(stockSP2))
                {
                    sqlcmd.Append(string.Format(@"and pd.StockPOID between '{0}' and '{1}'", stockSP1, stockSP2));
                }
                else
                {
                    sqlcmd.Append(string.Format(@"and pd.StockPOID = '{0}'", stockSP1));
                }
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd.Append(string.Format(
                    @" 
            and o.FtyGroup = '{0}'", factory));
            }

            if (!string.IsNullOrWhiteSpace(projectID))
            {
                sqlcmd.Append(string.Format(
                    @" 
            and o.ProjectID = '{0}'", projectID));
            }

            if (!string.IsNullOrWhiteSpace(cuttingInline_b))
            {
                sqlcmd.Append(string.Format(
                    @" 
            and not(o.CutInLine > '{1}' or  o.CutInLine < '{0}')",
                    cuttingInline_b,
                    cuttingInline_e));
            }

            if (!string.IsNullOrWhiteSpace(orderCfmDate_b))
            {
                sqlcmd.Append(string.Format(
                    @" 
            and o.CFMDate between '{0}' and '{1}'",
                    orderCfmDate_b,
                    orderCfmDate_e));
            }
            #endregion
            sqlcmd.Append($@"
)
select  *
        , 0.00 qty 
into #tmp 
from cte
where PoQty > InQty

select * 
from #tmp 
order by poid,seq1,seq2 ;

select  convert(bit,0) as selected
        , fi.Ukey FromFtyInventoryUkey
        , o.FactoryID FromFactoryID
        , fi.POID FromPoid
        , concat(Ltrim(Rtrim(fi.Seq1)), ' ', fi.Seq2) as Fromseq
        , fi.Seq1 FromSeq1
        , fi.Seq2 Fromseq2
        , fi.Roll FromRoll
        , fi.Dyelot FromDyelot
        , fi.StockType FromStockType
        , fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty BalanceQty
        , 0.00 as Qty
        , t.FactoryID  toFactoryID 
        , rtrim(t.poID) topoid
        , concat(Ltrim(Rtrim(t.Seq1)), ' ', t.Seq2) as toseq
        , rtrim(t.seq1) toseq1
        , t.seq2 toseq2
        , fi.Roll toRoll
        , fi.Dyelot toDyelot
        ,'B' tostocktype 
        , dbo.Getlocation(fi.ukey) fromlocation
        , '' tolocation
        , GroupQty = Sum(fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty) over(partition by t.poid,t.seq1,t.SEQ2,t.FactoryID,t.StockPOID,t.StockSeq1,t.StockSeq2,fi.Dyelot)
        , t.StockUnit
from #tmp t 
inner join FtyInventory fi WITH (NOLOCK) on  fi.POID = t.StockPOID 
                                             and fi.seq1 = t.StockSeq1 
                                             and fi.Seq2 = t.StockSeq2
inner join View_WH_Orders o WITH (NOLOCK) on fi.POID=o.id
where   fi.StockType = 'I' 
        and fi.Lock = 0
        and fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty > 0 
order by topoid, toseq1, toseq2, GroupQty DESC, fi.Dyelot, BalanceQty DESC

drop table #tmp  {(!string.IsNullOrEmpty(invCfmDate_s) && !string.IsNullOrEmpty(invCfmDate_e) ? ",#tmpInvtrans" : string.Empty)}
");

            this.ShowWaitMessage("Data Loading....");
            #endregion
            if (!SQL.Selects(string.Empty, sqlcmd.ToString(), out this.dataSet))
            {
                MyUtility.Msg.WarningBox(sqlcmd.ToString(), "DB error!!");
                return;
            }

            this.master = this.dataSet.Tables[0];
            this.master.TableName = "Master";

            // master.DefaultView.Sort = "poid,seq1,seq2,poqty";
            // dataSet.Tables[0].DefaultView.Sort = "poid,seq1,seq2,poqty";
            this.detail = this.dataSet.Tables[1];
            this.detail.TableName = "Detail";

            // dataSet.Tables[1].DefaultView.Sort = "fromdyelot,balanceQty";
            DataRelation relation = new DataRelation(
                "rel1",
                new DataColumn[] { this.master.Columns["poid"], this.master.Columns["seq1"], this.master.Columns["seq2"] },
                new DataColumn[] { this.detail.Columns["toPoid"], this.detail.Columns["toseq1"], this.detail.Columns["toseq2"] });

            this.dataSet.Relations.Add(relation);

            // master.Columns.Add("total_qty", typeof(decimal), "sum(child.qty)");
            this.master.Columns.Add("requestqty", typeof(decimal), "poqty - inqty - sum(child.qty)");
            this.master.Columns.Add("total_qty", typeof(decimal));

            this.listControlBindingSource1.DataSource = this.dataSet;
            this.listControlBindingSource1.DataMember = "Master";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource2.DataMember = "rel1";

            if (this.dataSet.Tables[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("NO Data!");
            }

            this.btnCreate.Enabled = true;
            this.HideWaitMessage();
        }

        private void BtnAutoPick_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.master))
            {
                return;
            }

            if (this.master.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow dr in this.master.Rows)
            {
                if (dr["selected"].ToString().ToUpper() == "TRUE")
                {
                    DataRow[] curentgridrowChild = dr.GetChildRows("rel1");
                    foreach (DataRow temp in curentgridrowChild)
                    {
                        temp["qty"] = 0.00;
                        temp["selected"] = false;
                    }

                    // dr["total_qty"] = curentgridrowChild.Sum(row => (decimal)row["qty"]);
                    dr["total_qty"] = 0.00;
                }

                if (dr["selected"].ToString() == "True" && !MyUtility.Check.Empty(dr["requestqty"]))
                {
                    var issued = Prgs.Autopick(dr, false, "I");
                    if (issued == null)
                    {
                        return;
                    }

                    foreach (DataRow dr2 in issued)
                    {
                        DataRow[] findrow = this.detail.Select(string.Format(
                            @"FromftyInventoryUkey = {0} and topoid = '{1}' and toseq1 = '{2}' and toseq2 = '{3}'",
                            dr2["ftyinventoryukey"],
                            dr["poid"],
                            dr["seq1"],
                            dr["seq2"]));
                        if (findrow.Length > 0)
                        {
                            findrow[0]["qty"] = dr2["qty"];
                            if (findrow[0]["stockunit"].EqualString("PCS"))
                            {
                                findrow[0]["qty"] = Math.Ceiling(MyUtility.Convert.GetDecimal(dr2["qty"]));
                            }

                            findrow[0]["selected"] = true;
                            if (MyUtility.Check.Seek($@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{findrow[0]["tostocktype"]}'
        and junk != '1'
        and  id ='{findrow[0]["fromlocation"]}'
"))
                            {
                                findrow[0]["tolocation"] = findrow[0]["fromlocation"];
                            }
                            else
                            {
                                findrow[0]["tolocation"] = string.Empty;
                            }
                        }
                    }

                    var tempchildrows = dr.GetChildRows("rel1");
                    dr["total_qty"] = tempchildrows.Sum(row => (decimal)row["qty"]);
                    this.gridRel.ValidateControl();
                    this.gridComplete.ValidateControl();
                }
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detail))
            {
                MyUtility.Msg.WarningBox("Please select data first!!");
                return;
            }

            if (MyUtility.Msg.QuestionBox("Do you want to create data?") == DialogResult.No)
            {
                return;
            }

            DataRow[] findrow = this.detail.Select(@"selected = true and qty <> 0");
            if (findrow.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!!");
                return;
            }

            /*
             * 依照 To POID 建立 P23
             */
            var listPoid = findrow.Select(row => row["ToPOID"]).Distinct().ToList();
            var tmpId = MyUtility.GetValue.GetBatchID(Env.User.Keyword + "PI", "SubTransfer", DateTime.Now, batchNumber: listPoid.Count);
            if (MyUtility.Check.Empty(tmpId))
            {
                MyUtility.Msg.WarningBox("Get document ID fail!!");
                return;
            }

            #region 準備 insert subtransfer & subtransfer_detail 語法與 DataTable dtMaster & dtDetail

            // insert 欄位順序必須與 dtMaster, dtDetail 一致
            string insertMaster = @"
insert into subtransfer
        (id      , type   , issuedate, mdivisionid, FactoryID
         , status, addname, adddate,  remark)
select   id      , type   , issuedate, mdivisionid, FactoryID
         , status, addname, adddate,   remark
from #tmp";
            string insertDetail = @"
insert into subtransfer_detail
(ID        , FromFtyInventoryUkey, FromFactoryID, FromPOID  , FromSeq1
 , FromSeq2, FromRoll            , FromStockType, FromDyelot, ToFactoryID
 , ToPOID  , ToSeq1              , ToSeq2       , ToRoll    , ToStockType
 , ToDyelot, Qty                 , ToLocation)
select *
from #tmp";

            DataTable dtMaster = new DataTable();
            dtMaster.Columns.Add("Poid");
            dtMaster.Columns.Add("id");
            dtMaster.Columns.Add("type");
            dtMaster.Columns.Add("issuedate");
            dtMaster.Columns.Add("mdivisionid");
            dtMaster.Columns.Add("FactoryID");
            dtMaster.Columns.Add("status");
            dtMaster.Columns.Add("addname");
            dtMaster.Columns.Add("adddate");
            dtMaster.Columns.Add("remark");

            DataTable dtDetail = new DataTable();
            dtDetail.Columns.Add("ID");
            dtDetail.Columns.Add("FromFtyInventoryUkey");
            dtDetail.Columns.Add("FromFactoryID");
            dtDetail.Columns.Add("FromPOID");
            dtDetail.Columns.Add("FromSeq1");
            dtDetail.Columns.Add("FromSeq2");
            dtDetail.Columns.Add("FromRoll");
            dtDetail.Columns.Add("FromStockType");
            dtDetail.Columns.Add("FromDyelot");
            dtDetail.Columns.Add("ToFactoryID");
            dtDetail.Columns.Add("ToPOID");
            dtDetail.Columns.Add("ToSeq1");
            dtDetail.Columns.Add("ToSeq2");
            dtDetail.Columns.Add("ToRoll");
            dtDetail.Columns.Add("ToStockType");
            dtDetail.Columns.Add("ToDyelot");
            dtDetail.Columns.Add("Qty");
            dtDetail.Columns.Add("ToLocation");
            dtDetail.Columns.Add("FromLocation");
            dtDetail.Columns.Add("FromSeq");
            dtDetail.Columns.Add("toSeq");

            for (int i = 0; i < listPoid.Count; i++)
            {
                DataRow drNewMaster = dtMaster.NewRow();
                drNewMaster["poid"] = listPoid[i].ToString();
                drNewMaster["id"] = tmpId[i].ToString();
                drNewMaster["type"] = "B";
                drNewMaster["issuedate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                drNewMaster["mdivisionid"] = Env.User.Keyword;
                drNewMaster["FactoryID"] = Env.User.Factory;
                drNewMaster["status"] = "New";
                drNewMaster["addname"] = Env.User.UserID;
                drNewMaster["adddate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                drNewMaster["remark"] = "Batch create by P29";
                dtMaster.Rows.Add(drNewMaster);
            }

            foreach (DataRow item in findrow)
            {
                DataRow[] drGetID = dtMaster.AsEnumerable().Where(row => row["poid"].EqualString(item["ToPOID"])).ToArray();
                DataRow drNewDetail = dtDetail.NewRow();
                drNewDetail["ID"] = drGetID[0]["ID"];
                drNewDetail["FromFtyInventoryUkey"] = item["fromftyinventoryukey"];
                drNewDetail["FromFactoryID"] = item["FromFactoryID"];
                drNewDetail["FromPOID"] = item["frompoid"];
                drNewDetail["FromSeq1"] = item["fromseq1"];
                drNewDetail["FromSeq2"] = item["fromseq2"];
                drNewDetail["FromRoll"] = item["fromroll"];
                drNewDetail["FromStockType"] = item["fromstocktype"];
                drNewDetail["FromDyelot"] = item["fromdyelot"];
                drNewDetail["ToFactoryID"] = item["ToFactoryID"];
                drNewDetail["ToPOID"] = item["topoid"];
                drNewDetail["ToSeq1"] = item["toseq1"];
                drNewDetail["ToSeq2"] = item["toseq2"];
                drNewDetail["ToRoll"] = item["toroll"];
                drNewDetail["ToStockType"] = item["toStocktype"];
                drNewDetail["ToDyelot"] = item["toDyelot"];
                drNewDetail["Qty"] = item["qty"];
                drNewDetail["ToLocation"] = item["tolocation"];
                drNewDetail["Fromlocation"] = item["Fromlocation"];
                drNewDetail["FromSeq"] = item["FromSeq"];
                drNewDetail["toSeq"] = item["toSeq"];
                dtDetail.Rows.Add(drNewDetail);
            }
            #endregion

            DualResult result;
            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, insertMaster, out DataTable dtResult)))
                    {
                        throw result.GetException();
                    }

                    DataTable dtcopy = dtDetail.Copy();
                    dtcopy.Columns.Remove("FromLocation");
                    dtcopy.Columns.Remove("FromSeq");
                    dtcopy.Columns.Remove("toSeq");

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtcopy, null, insertDetail, out dtResult)))
                    {
                        throw result.GetException();
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            #region confirm save成功的P23單子
            List<string> success_list = new List<string>();
            List<string> fail_list = new List<string>();
            DataTable dtP23confirmResult = new DataTable();
            dtP23confirmResult.Columns.Add("ID", typeof(string));
            dtP23confirmResult.Columns.Add("Message", typeof(string));

            foreach (DataRow dr in dtMaster.Rows)
            {
                #region 檢查物料Location 是否存在WMS
                if (!PublicPrg.Prgs.Chk_WMS_Location(dr["ID"].ToString(), "P23"))
                {
                    return;
                }
                #endregion

                string detailbyIDCmd = $@"
select *
    ,Fromlocation = Fromlocation.listValue
from SubTransfer_Detail sd with(nolock)
left join FtyInventory FI on sd.fromPoid = fi.poid and sd.fromSeq1 = fi.seq1 and sd.fromSeq2 = fi.seq2 and sd.fromDyelot = fi.Dyelot
    and sd.fromRoll = fi.roll and sd.fromStocktype = fi.stocktype
outer apply(
	select listValue = Stuff((
			select concat(',',MtlLocationID)
			from (
					select 	distinct
						fd.MtlLocationID
					from FtyInventory_Detail fd
					left join MtlLocation ml on ml.ID = fd.MtlLocationID
					where fd.Ukey = fi.Ukey
					and ml.Junk = 0 
					and ml.StockType = sd.ToStockType
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
where id = '{dr["ID"]}'";
                result = DBProxy.Current.Select(null, detailbyIDCmd, out DataTable dtDetail_byid);
                if (!result)
                {
                    this.ShowErr(result);
                }

                DataRow drP23confirmResult = dtP23confirmResult.NewRow();
                result = Prgs.P23confirm(dr["ID"].ToString(), dtDetail_byid);

                drP23confirmResult["ID"] = dr["ID"].ToString();

                if (result)
                {
                    drP23confirmResult["Message"] = "Be created!! and Confirm Success!!";

                    // AutoWHFabric WebAPI
                    Gensong_AutoWHFabric.Sent(true, dtDetail_byid, "P23", EnumStatus.New, EnumStatus.Confirm);
                    Vstrong_AutoWHAccessory.Sent(true, dtDetail_byid, "P23", EnumStatus.New, EnumStatus.Confirm);
                }
                else
                {
                    drP23confirmResult["Message"] = $@"Be created!!, Confirm fail, please go to P23 manual Confirm
{result.Description}
{result.GetException()}";
                }

                dtP23confirmResult.Rows.Add(drP23confirmResult);
            }

            MyUtility.Msg.ShowMsgGrid_LockScreen(dtP23confirmResult, "P29.Create & Confirm result", "Information");
            #endregion

            if (!this.master.Columns.Contains("TransID"))
            {
                this.master.Columns.Add("TransID", typeof(string));
            }

            foreach (DataRow alldetailrows in this.detail.Rows)
            {
                if (alldetailrows["selected"].ToString().ToUpper() == "TRUE")
                {
                    DataRow[] drGetID = dtMaster.AsEnumerable().Where(row => row["poid"].EqualString(alldetailrows["ToPOID"])).ToArray();
                    alldetailrows.GetParentRow("rel1")["selected"] = true;
                    alldetailrows.GetParentRow("rel1")["TransID"] = drGetID[0]["ID"];
                }
            }

            // Create後Btn失效，需重新Qurey才能再使用。
            this.btnCreate.Enabled = false;
            this.gridRel.ValidateControl();
            this.gridComplete.ValidateControl();
        }

        private void CheckOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkOnly.Checked)
            {
                this.listControlBindingSource1.Filter = "complete = 'Y'";
            }
            else
            {
                this.listControlBindingSource1.Filter = string.Empty;
            }
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.master))
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Bulk");
                return;
            }

            if (!this.master.Columns.Contains("TransID"))
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Bulk");
                return;
            }

            this.master.DefaultView.RowFilter = "TransID<>''";
            DataTable exceldt = this.master.DefaultView.ToTable();
            if (exceldt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Bulk");
                return;
            }

            string excelName = "Warehouse_P29";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx");

            // excelApp.DisplayAlerts = false;
            MyUtility.Excel.CopyToXls(exceldt, string.Empty, $"{excelName}.xltx", 2, false, null, excelApp, wSheet: excelApp.Sheets[1], DisplayAlerts_ForSaveFile: false);

            excelApp.Sheets[1].Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            excelApp.ActiveWorkbook.SaveAs(strExcelName);
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);

            strExcelName.OpenFile();
            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridComplete_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.gridRel.ValidateControl();
        }

        private void GridRel_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataRow dr = this.gridRel.GetDataRow<DataRow>(e.RowIndex);
            if (this.gridRel.Columns[e.ColumnIndex].Name == "qty")
            {
                this.col_Qty.DecimalPlaces = 2;
                if (MyUtility.Convert.GetString(dr["stockunit"]).EqualString("PCS"))
                {
                    this.col_Qty.DecimalPlaces = 0;
                }
            }
        }

        private void P29_Load(object sender, EventArgs e)
        {
        }
    }
}
