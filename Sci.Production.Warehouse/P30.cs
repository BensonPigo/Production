using Ict;
using Ict.Win;
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
using Sci.Production.Class;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Production.Automation;
using System.Threading.Tasks;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P30 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk2 = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;

        private DataSet dataSet;
        private DataTable master;
        private DataTable detail;

        private Msg p30_msg = new Msg();

        /// <inheritdoc/>
        public P30(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboCategory.SelectedIndex = 0;
            this.comboFabricType.SelectedIndex = 0;

            #region -- Grid1 設定 --
            this.gridComplete.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridComplete.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridComplete)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk)
                 .Text("complete", header: "Complete" + Environment.NewLine + "Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(3), iseditingreadonly: true, alignment: DataGridViewContentAlignment.MiddleCenter)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                 .Numeric("poqty", header: "Order Qty", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("InQty", header: "Accu Trans.", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("total_qty", header: "Trans. Qty", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("requestqty", header: "Balance", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Text("TransID", header: "Trans. ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                  ;
            #endregion
            this.col_chk.CellClick += (s, e) =>
          {
              if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
              {
                  return;
              }

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
            #region -- transfer qty valid --
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            ns.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource) || MyUtility.Check.Empty(this.listControlBindingSource2.DataSource))
                {
                    return;
                }

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
            DataGridViewGeneratorTextColumnSettings ts2 = TxtMtlLocation.CellMtlLocation.GetGridCell("O");
            this.gridRel.CellValueChanged += (s, e) =>
            {
                if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource) || MyUtility.Check.Empty(this.listControlBindingSource2.DataSource))
                {
                    return;
                }

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

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.gridRel.GetDataRow(e.RowIndex);
                    dr["tolocation"] = e.FormattedValue;
                    dr.EndEdit();
                }
            };

            #region -- Grid2 設定 --
            this.gridRel.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridRel.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridRel)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk2)
                 .Text("fromroll", header: "Roll#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Numeric("balanceQty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("qty", header: "Trans. Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, settings: ns).Get(out this.col_Qty)
                  .Text("fromlocation", header: "From Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: true)
                  .Text("tolocation", header: "To Scarp" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: false, settings: ts2).Get(out this.col_tolocation)
                  ;
            this.col_Qty.DefaultCellStyle.BackColor = Color.Pink;
            this.col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            this.Chp();
        }

        private void Chp()
        {
            if (MyUtility.Check.Empty(this.listControlBindingSource2.DataSource))
            {
                return;
            }
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

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.dataSet = null;
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource2.DataSource = null;

            #region Filter 表頭條件

            int selectindex = this.comboCategory.SelectedIndex;
            int selectindex2 = this.comboFabricType.SelectedIndex;
            string strCfmDate1 = null;
            string strCfmDate2 = null;
            string strSP_s = this.txtSP_s.Text;
            string strSP_e = this.txtSP_e.Text;
            string strFactory = this.txtmfactory.Text;

            if (this.dateCfmDate.Value1 != null && this.dateCfmDate.Value2 != null)
            {
                strCfmDate1 = this.dateCfmDate.Text1;
                strCfmDate2 = this.dateCfmDate.Text2;
            }

            if (MyUtility.Check.Empty(strSP_s) &&
                MyUtility.Check.Empty(strSP_e) &&
                MyUtility.Check.Empty(strCfmDate1) &&
                MyUtility.Check.Empty(strCfmDate2))
            {
                MyUtility.Msg.WarningBox("<SP#> , <Cfm Date> cannot be empty!");
                return;
            }
            #endregion

            StringBuilder sqlcmd = new StringBuilder();
            #region -- sql command --

            sqlcmd.Append($@"
;with cte as (
    select  
	        convert(bit,0) as selected
            , iif(y.cnt > 0 ,'Y','') complete
            , f.MDivisionID
            , rtrim(o.id) poid
            , o.Category
            , o.FtyGroup
            , o.CFMDate
            , o.CutInLine
            , o.ProjectID
            , o.FactoryID 
            , pd.seq1
            , pd.seq2
            , pd.ID AS StockPOID
            , pd.SEQ1 AS StockSeq1
            , pd.SEQ2 AS StockSeq2
            ,ROUND(dbo.GetUnitQty(pd.POUnit,pd.StockUnit,x.taipei_qty), 2) N'PoQty'
            , pd.POUnit
            , pd.StockUnit
            , InQty = isnull(xx.InQty,0)
    from dbo.orders o WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = o.ID
    inner join dbo.Factory f WITH (NOLOCK) on f.id = o.FtyGroup
    inner join dbo.Factory checkProduceFty With (NoLock) on o.FactoryID = checkProduceFty.ID 
    outer apply (
        select  count(1) cnt 
        from FtyInventory fi WITH (NOLOCK) 
        left join FtyInventory_Detail fid WITH (NOLOCK) on fid.Ukey = fi.Ukey 
	    where   fi.POID = pd.ID 
                and fi.Seq1 = pd.Seq1 
                and fi.Seq2 = pd.Seq2 
                and fi.StockType = 'I' 
	            and fid.MtlLocationID is not null 
                and fi.Lock = 0 
                and fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty > 0
    ) y--Detail有MD為null數量,沒有則為0,沒資料也為0
    cross apply (
        select sum(i.qty) taipei_qty 
        from dbo.Invtrans i WITH (NOLOCK) 
        where   i.InventoryPOID = pd.ID 
                and i.InventorySeq1  = pd.Seq1 
                and i.InventorySeq2 = pd.Seq2 
				and i.Type=5
               
    ) x -- 需要轉的數量
    cross apply (
	    select sum(s2.Qty) as InQty 
        from dbo.SubTransfer s1 WITH (NOLOCK) 
        inner join dbo.SubTransfer_Detail s2 WITH (NOLOCK) on s2.Id= s1.Id 
	    where   s1.type ='E' 
                and s1.Status ='Confirmed' 
                and s2.ToStockType = 'O' 
                and s2.ToPOID = pd.id 
                and s2.ToSeq1 = pd.seq1 
                and s2.ToSeq2 = pd.seq2 
    ) xx --已轉的數量
    where    1=1
	and f.MDivisionID = '{Env.User.Keyword}'
    and checkProduceFty.IsProduceFty = '1'");
            #endregion

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

            switch (selectindex2)
            {
                case 0:
                    break;
                case 1:
                    sqlcmd.Append(@" 
            AND pd.FabricType ='F'");
                    break;
                case 2:
                    sqlcmd.Append(@" 
            AND pd.FabricType ='A'");
                    break;
            }

            if (!MyUtility.Check.Empty(strSP_s) && !MyUtility.Check.Empty(strSP_e))
            {
                sqlcmd.Append($@" 
            and pd.id >= '{strSP_s}'
            and pd.id <= '{strSP_e}'");
            }
            else if (!MyUtility.Check.Empty(strSP_s) && MyUtility.Check.Empty(strSP_e))
            {
                sqlcmd.Append($@" 
            and pd.id = '{strSP_s}'");
            }
            else if (MyUtility.Check.Empty(strSP_s) && !MyUtility.Check.Empty(strSP_e))
            {
                sqlcmd.Append($@" 
            and pd.id = '{strSP_e}'");
            }

            if (!MyUtility.Check.Empty(strFactory))
            {
                sqlcmd.Append($@" 
            and o.FtyGroup = '{strFactory}'");
            }

            if (!string.IsNullOrWhiteSpace(strCfmDate2))
            {
                sqlcmd.Append($@" 
			AND EXISTS(
				SELECt 1
				FROM Invtrans i WITH (NOLOCK)
				WHERE i.InventoryPOID = pd.ID
					AND i.InventorySeq1 = pd.Seq1
					AND i.InventorySeq2 = pd.Seq2 
					AND i.Type=5
					AND i.ConfirmDate between '{strCfmDate1}' and '{strCfmDate2}'
			)");
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
        , fi.Seq1 FromSeq1
        , fi.Seq2 Fromseq2
        , fi.Roll FromRoll
        , fi.Dyelot FromDyelot
        , fi.StockType FromStockType
        , fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty BalanceQty
        , 0.00 as Qty
        , t.FactoryID  toFactoryID 
        , rtrim(t.poID) topoid
        , t.seq1 toseq1
        , t.seq2 toseq2
        , fi.Roll toRoll
        , fi.Dyelot toDyelot
        ,'O' tostocktype 
        , dbo.Getlocation(fi.ukey) fromlocation
        , '' tolocation
        , GroupQty = Sum(fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty) over(partition by t.poid,t.seq1,t.SEQ2,t.FactoryID,t.POID,t.Seq1,t.Seq2,fi.Dyelot)
from #tmp t 
inner join FtyInventory fi WITH (NOLOCK) on  fi.POID = t.poid 
                                             and fi.seq1 = t.seq1 
                                             and fi.Seq2 = t.Seq2
inner join dbo.orders o WITH (NOLOCK) on fi.POID=o.id
where   fi.StockType = 'I' 
        and fi.Lock = 0 
        and fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty > 0 
order by topoid, toseq1, toseq2, GroupQty DESC, fi.Dyelot, BalanceQty DESC
drop table #tmp

");
            this.ShowWaitMessage("Data Loading....");

            if (!SQL.Selects(string.Empty, sqlcmd.ToString(), out this.dataSet))
            {
                MyUtility.Msg.WarningBox(sqlcmd.ToString(), "DB error!!");
                return;
            }

            this.master = this.dataSet.Tables[0];
            this.master.TableName = "Master";
            this.master.DefaultView.Sort = "poid,seq1,seq2";
            this.dataSet.Tables[0].DefaultView.Sort = "poid,seq1,seq2";

            this.detail = this.dataSet.Tables[1];
            this.detail.TableName = "Detail";

            DataRelation relation = new DataRelation(
                "rel1",
                new DataColumn[] { this.master.Columns["poid"], this.master.Columns["seq1"], this.master.Columns["seq2"] },
                new DataColumn[] { this.detail.Columns["toPoid"], this.detail.Columns["toseq1"], this.detail.Columns["toseq2"] });

            this.dataSet.Relations.Add(relation);

            this.master.Columns.Add("total_qty", typeof(decimal));
            this.master.Columns.Add("requestqty", typeof(decimal), "poqty - inqty - sum(child.qty)");

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

        private void CheckOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                return;
            }

            if (this.checkOnly.Checked)
            {
                this.listControlBindingSource1.Filter = "complete = 'Y'";
            }
            else
            {
                this.listControlBindingSource1.Filter = string.Empty;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void GridComplete_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
             this.gridRel.ValidateControl();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
                            @"FromftyInventoryUkey = {0} and topoid = '{1}'
                                                                          and toseq1 = '{2}' and toseq2 = '{3}'",
                            dr2["ftyinventoryukey"], dr["poid"], dr["seq1"], dr["seq2"]));
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

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detail))
            {
                MyUtility.Msg.WarningBox("Please select data first!!");
                return;
            }

            DataRow[] findrow = this.detail.Select(@"selected = true and qty <> 0");
            if (findrow.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!!");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to create data?");
            if (dResult == DialogResult.No)
            {
                return;
            }

            /*
             * 依照 To POID 建立 P24
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
from #tmp
;

";

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

            for (int i = 0; i < listPoid.Count; i++)
            {
                DataRow drNewMaster = dtMaster.NewRow();
                drNewMaster["poid"] = listPoid[i].ToString();
                drNewMaster["id"] = tmpId[i].ToString();
                drNewMaster["type"] = "E";
                drNewMaster["issuedate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                drNewMaster["mdivisionid"] = Env.User.Keyword;
                drNewMaster["FactoryID"] = Env.User.Factory;
                drNewMaster["status"] = "New";
                drNewMaster["addname"] = Env.User.UserID;
                drNewMaster["adddate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                drNewMaster["remark"] = "Batch create by P30";
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
                dtDetail.Rows.Add(drNewDetail);
            }
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            DualResult result;
            using (transactionscope)
            {
                try
                {
                    DataTable dtResult;
                    if ((result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, insertMaster, out dtResult)) == false)
                    {
                        transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.ToString(), "Create failed");
                        return;
                    }

                    if ((result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, insertDetail, out dtResult)) == false)
                    {
                        transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.ToString(), "Create failed");
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    this.SentToGensong_AutoWHFabric(dtMaster);
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope = null;

            #region confirm save成功的P24單子
            List<string> success_list = new List<string>();
            List<string> fail_list = new List<string>();
            foreach (DataRow dr in dtMaster.Rows)
            {
                if (Prgs.P24confirm(dr["ID"].ToString()))
                {
                    success_list.Add(dr["ID"].ToString());
                }
                else
                {
                    fail_list.Add(dr["ID"].ToString());
                }
            }

            #region New Barcode
            DataTable dt = new DataTable();
            string sqlcmd = string.Empty;
            string upd_Fty_Barcode_V1 = string.Empty;
            string upd_Fty_Barcode_V2 = string.Empty;

            foreach (DataRow dr in dtMaster.Rows)
            {
                #region From
                sqlcmd = $@"
select i2.ID
,[Barcode1] = f.Barcode
,[OriBarcode] = fbOri.Barcode
,[balanceQty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[NewBarcode] = iif(f.Barcode ='',fbOri.Barcode,f.Barcode)
,[Poid] = i2.FromPOID
,[Seq1] = i2.FromSeq1
,[Seq2] = i2.FromSeq2
,[Roll] = i2.FromRoll
,[Dyelot] = i2.FromDyelot
,[StockType] = i2.FromStockType
from Production.dbo.SubTransfer_Detail i2
inner join Production.dbo.SubTransfer i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.FromPOID
    and f.Seq1 = i2.FromSeq1 and f.Seq2 = i2.FromSeq2
    and f.Roll = i2.FromRoll and f.Dyelot = i2.FromDyelot
    and f.StockType = i2.FromStockType
outer apply(
	select *
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
	and t.TransactionID = '{dr["ID"]}'
)fbOri
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.FromPoid and seq1=i2.FromSeq1 and seq2=i2.FromSeq2 
	and FabricType='F'
)
and i2.id = '{dr["ID"]}'
";
                DBProxy.Current.Select(string.Empty, sqlcmd, out dt);
                var data_From_FtyBarcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                            select new
                                            {
                                                TransactionID = dr["ID"].ToString(),
                                                poid = m.Field<string>("poid"),
                                                seq1 = m.Field<string>("seq1"),
                                                seq2 = m.Field<string>("seq2"),
                                                stocktype = m.Field<string>("stocktype"),
                                                roll = m.Field<string>("roll"),
                                                dyelot = m.Field<string>("dyelot"),
                                                Barcode = m.Field<string>("NewBarcode"),
                                            }).ToList();

                // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
                upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(70, null, false);
                upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, true);
                DataTable resultFrom;
                if (data_From_FtyBarcode.Count >= 1)
                {
                    // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_From_FtyBarcode, string.Empty, upd_Fty_Barcode_V1, out resultFrom, "#TmpSource")))
                    {
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_From_FtyBarcode, string.Empty, upd_Fty_Barcode_V2, out resultFrom, "#TmpSource")))
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
                #endregion

                #region To
                sqlcmd = $@"
select f.Ukey
,[ToBarcode] = isnull(f.Barcode,'')
,[ToBarcode2] = isnull(Tofb.Barcode,'')
,[FromBarcode] = isnull(fromBarcode.Barcode,'')
,[FromBarcode2] = isnull(Fromfb.Barcode,'')
,[ToBalanceQty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[FromBalanceQty] = fromBarcode.BalanceQty
,[NewBarcode] = ''
,[Poid] = i2.ToPOID
,[Seq1] = i2.ToSeq1
,[Seq2] = i2.ToSeq2
,[Roll] = i2.ToRoll
,[Dyelot] = i2.ToDyelot
,[StockType] = i2.ToStockType
from Production.dbo.SubTransfer_Detail i2
inner join Production.dbo.SubTransfer i on i2.Id=i.Id 
left join FtyInventory f on f.POID = i2.ToPOID
    and f.Seq1 = i2.ToSeq1 and f.Seq2 = i2.ToSeq2
    and f.Roll = i2.ToRoll and f.Dyelot = i2.ToDyelot
    and f.StockType = i2.ToStockType

outer apply(
	select Barcode = MAX(Barcode)
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
)Tofb
outer apply(
	select f2.Barcode 
	,BalanceQty = f2.InQty - f2.OutQty + f2.AdjustQty - f2.ReturnQty
	,f2.Ukey
	from FtyInventory f2	
	where f2.POID = i2.FromPOID
	and f2.Seq1 = i2.FromSeq1 and f2.Seq2 = i2.FromSeq2
	and f2.Roll = i2.FromRoll and f2.Dyelot = i2.FromDyelot
	and f2.StockType = i2.FromStockType
)fromBarcode
outer apply(
	select Barcode = MAX(Barcode)
	from FtyInventory_Barcode t
	where t.Ukey = fromBarcode.Ukey
)Fromfb
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.ToPoid and seq1=i2.ToSeq1 and seq2=i2.ToSeq2 
	and FabricType='F'
)
and i2.id = '{dr["ID"]}'
";
                DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

                foreach (DataRow dr2 in dt.Rows)
                {
                    string strBarcode = string.Empty;

                    // 目標有自己的Barcode, 則Ftyinventory跟記錄都是用自己的
                    if (!MyUtility.Check.Empty(dr2["ToBarcode"]) || !MyUtility.Check.Empty(dr2["ToBarcode2"]))
                    {
                        strBarcode = MyUtility.Check.Empty(dr2["ToBarcode2"]) ? dr2["ToBarcode"].ToString() : dr2["ToBarcode2"].ToString();
                        dr2["NewBarcode"] = strBarcode;
                    }
                    else
                    {
                        // 目標沒Barcode, 則 來源有餘額(部分轉)就用來源Barocde_01++, 如果全轉就用來源Barocde
                        strBarcode = MyUtility.Check.Empty(dr2["FromBarcode2"]) ? dr2["FromBarcode"].ToString() : dr2["FromBarcode2"].ToString();

                        // InQty-Out+Adj != 0 代表非整卷, 要在Barcode後+上-01,-02....
                        if (!MyUtility.Check.Empty(dr2["FromBalanceQty"]))
                        {
                            if (strBarcode.Contains("-"))
                            {
                                dr2["NewBarcode"] = strBarcode.Substring(0, 13) + "-" + Prgs.GetNextValue(strBarcode.Substring(14, 2), 1);
                            }
                            else
                            {
                                dr2["NewBarcode"] = MyUtility.Check.Empty(strBarcode) ? string.Empty : strBarcode + "-01";
                            }
                        }
                        else
                        {
                            // 如果InQty-Out+Adj = 0 代表整卷發出就使用原本Barcode
                            dr2["NewBarcode"] = strBarcode;
                        }
                    }
                }

                var data_To_FtyBarcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                          select new
                                          {
                                              TransactionID = dr["ID"].ToString(),
                                              poid = m.Field<string>("poid"),
                                              seq1 = m.Field<string>("seq1"),
                                              seq2 = m.Field<string>("seq2"),
                                              stocktype = m.Field<string>("stocktype"),
                                              roll = m.Field<string>("roll"),
                                              dyelot = m.Field<string>("dyelot"),
                                              Barcode = m.Field<string>("NewBarcode"),
                                          }).ToList();

                // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
                upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(70, null, true);
                upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, true);
                DataTable resultTo;
                if (data_To_FtyBarcode.Count >= 1)
                {
                    // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_To_FtyBarcode, string.Empty, upd_Fty_Barcode_V1, out resultTo, "#TmpSource")))
                    {
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_To_FtyBarcode, string.Empty, upd_Fty_Barcode_V2, out resultTo, "#TmpSource")))
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
                #endregion
            }
            #endregion

            string msg = string.Empty;
            msg += success_list.Count > 0 ? "Trans. ID" + Environment.NewLine + success_list.JoinToString(Environment.NewLine) + Environment.NewLine + "be created!! and Confirm Success!!" + Environment.NewLine : string.Empty;
            msg += fail_list.Count > 0 ? "Trans. ID" + Environment.NewLine + fail_list.JoinToString(Environment.NewLine) + Environment.NewLine + "be created!!, Confirm fail, please go to P24 manual Confirm" : string.Empty;
            this.p30_msg.Show(msg);
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

            // AutoWHAccessory WebAPI for Vstrong
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                Task.Run(() => new Vstrong_AutoWHAccessory().SentSubTransfer_DetailToVstrongAutoWHAccessory(dtMaster, true))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            // Create後Btn失效，需重新Qurey才能再使用。
            this.btnCreate.Enabled = false;
            this.gridRel.ValidateControl();
            this.gridComplete.ValidateControl();
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.master))
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Scarp");
                return;
            }

            if (!this.master.Columns.Contains("TransID"))
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Scarp");
                return;
            }

            this.master.DefaultView.RowFilter = "TransID<>''";
            if (MyUtility.Check.Empty(this.master))
            {
                return;
            }

            DataTable exceldt = this.master.DefaultView.ToTable();
            if (exceldt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Scarp");
                return;
            }

            // 篩選報表所顯示的欄位
            exceldt.Columns.Remove("selected");
            exceldt.Columns.Remove("CutInLine");
            exceldt.Columns.Remove("ProjectID");
            exceldt.Columns.Remove("FactoryID");
            exceldt.Columns.Remove("stockpoid");
            exceldt.Columns.Remove("stockseq1");
            exceldt.Columns.Remove("stockseq2");
            exceldt.Columns.Remove("qty");

            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P30.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(exceldt, string.Empty, "Warehouse_P30.xltx", 2, showExcel: true, showSaveMsg: true, excelApp: excelApp);      // 將datatable copy to excel
            Marshal.ReleaseComObject(excelApp);
        }

        private void SentToGensong_AutoWHFabric(DataTable dtMaster)
        {
            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                Task.Run(() => new Gensong_AutoWHFabric().SentSubTransfer_DetailToGensongAutoWHFabric(dtMaster, true))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}
