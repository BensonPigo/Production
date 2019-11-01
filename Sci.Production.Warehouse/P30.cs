﻿using Ict;
using Ict.Win;
using Sci.Production.PublicForm;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class P30 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk2 = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
        Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;

        private DataSet dataSet;
        private DataTable master;
        private DataTable detail;

        private msg p30_msg = new msg();

        public P30(ToolStripMenuItem menuitem)
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
            col_chk.CellClick += (s, e) =>
          {
              if (MyUtility.Check.Empty(listControlBindingSource1.DataSource))
              {
                  return;
              }
              DataRow thisRow = this.gridComplete.GetDataRow(this.listControlBindingSource1.Position);
              if (null == thisRow)
              {
                  return;
              }
              if (e.RowIndex == -1)
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
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            ns.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)|| MyUtility.Check.Empty(listControlBindingSource2.DataSource))
                {
                    return;
                }
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow thisRow = this.gridComplete.GetDataRow(this.listControlBindingSource1.Position);
                    DataRow[] curentgridrowChild = thisRow.GetChildRows("rel1");
                    DataRow currentrow = gridRel.GetDataRow(gridRel.GetSelectedRowIndex());
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
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = Production.Class.cellMtlLocation.GetGridCell("O");
            this.gridRel.CellValueChanged += (s, e) =>
{
    if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)||MyUtility.Check.Empty(listControlBindingSource2.DataSource))
    {
        return;
    }
    if (gridRel.Columns[e.ColumnIndex].Name == col_chk2.Name)
    {
        DataRow dr = gridRel.GetDataRow(e.RowIndex);
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
        DataRow currentrow = gridRel.GetDataRow(gridRel.GetSelectedRowIndex());
        currentrow.GetParentRow("rel1")["total_qty"] = curentgridrowChild.Sum(row => (decimal)row["qty"]);
        currentrow.EndEdit();
    }
};
            #region -- Grid2 設定 --
            this.gridRel.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridRel.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.gridRel)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out col_chk2)
                 .Text("fromroll", header: "Roll#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Numeric("balanceQty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("qty", header: "Trans. Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, settings: ns).Get(out col_Qty)
                  .Text("fromlocation", header: "From Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: true)
                  .Text("tolocation", header: "To Scarp" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: false, settings: ts2).Get(out col_tolocation)
                  ;
            col_Qty.DefaultCellStyle.BackColor = Color.Pink;
            col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            chp();
        }

        private void chp()
        {
            if (MyUtility.Check.Empty(listControlBindingSource2.DataSource))
            {
                return;
            }
            #region selected
            col_chk2.CellClick += (s, e) =>
            {
                DataRow thisRow = this.gridRel.GetDataRow(this.listControlBindingSource2.Position);
                if (null == thisRow)
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
            dataSet = null;
            listControlBindingSource1.DataSource = null;
            listControlBindingSource2.DataSource = null;

            #region Filter 表頭條件

            int selectindex = comboCategory.SelectedIndex;
            int selectindex2 = comboFabricType.SelectedIndex;
            string strCfmDate1 = null;
            string strCfmDate2 = null;
            string strSP = this.txtSP.Text;
            string strFactory = this.txtmfactory.Text;

            if (dateCfmDate.Value1 != null && dateCfmDate.Value2 != null)
            {
                strCfmDate1 = this.dateCfmDate.Text1;
                strCfmDate2 = this.dateCfmDate.Text2;
            }

            if (MyUtility.Check.Empty(strSP) && 
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
                and fi.InQty - fi.OutQty + fi.AdjustQty > 0
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
                    sqlcmd.Append(@" 
            AND pd.FabricType ='F'");
                    break;
                case 1:
                    sqlcmd.Append(@" 
            AND pd.FabricType ='A'");
                    break;
                case 2:
                    break;
            }

            if (!MyUtility.Check.Empty(strSP))
            {
                sqlcmd.Append($@" 
            and pd.id = '{strSP}'");
            }

            if (!MyUtility.Check.Empty(strFactory))
            {
                sqlcmd.Append($@" 
            and o.FtyGroup = '{strFactory}'");
            }
            
            if (!(string.IsNullOrWhiteSpace(strCfmDate2)))
            {
                sqlcmd.Append($@" 
            and o.CFMDate between '{strCfmDate1}' and '{strCfmDate2}'");
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
        , fi.InQty - fi.OutQty + fi.AdjustQty BalanceQty
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
        , GroupQty = Sum(fi.InQty - fi.OutQty + fi.AdjustQty) over(partition by t.poid,t.seq1,t.SEQ2,t.FactoryID,t.POID,t.Seq1,t.Seq2,fi.Dyelot)
from #tmp t 
inner join FtyInventory fi WITH (NOLOCK) on  fi.POID = t.poid 
                                             and fi.seq1 = t.seq1 
                                             and fi.Seq2 = t.Seq2
inner join dbo.orders o WITH (NOLOCK) on fi.POID=o.id
where   fi.StockType = 'I' 
        and fi.Lock = 0 
        and fi.InQty - fi.OutQty + fi.AdjustQty > 0 
order by topoid, toseq1, toseq2, GroupQty DESC, fi.Dyelot, BalanceQty DESC
drop table #tmp

");
            this.ShowWaitMessage("Data Loading....");

            if (!SQL.Selects("", sqlcmd.ToString(), out dataSet))
            {
                MyUtility.Msg.WarningBox(sqlcmd.ToString(), "DB error!!");
                return;
            }
            master = dataSet.Tables[0];
            master.TableName = "Master";
            master.DefaultView.Sort = "poid,seq1,seq2";
            dataSet.Tables[0].DefaultView.Sort = "poid,seq1,seq2";

            detail = dataSet.Tables[1];
            detail.TableName = "Detail";

            DataRelation relation = new DataRelation("rel1"
                    , new DataColumn[] { master.Columns["poid"], master.Columns["seq1"], master.Columns["seq2"] }
                    , new DataColumn[] { detail.Columns["toPoid"], detail.Columns["toseq1"], detail.Columns["toseq2"] }
                    );

            dataSet.Relations.Add(relation);
            
            master.Columns.Add("total_qty", typeof(decimal));
            master.Columns.Add("requestqty", typeof(decimal), "poqty - inqty - sum(child.qty)");

            listControlBindingSource1.DataSource = dataSet;
            listControlBindingSource1.DataMember = "Master";
            listControlBindingSource2.DataSource = listControlBindingSource1;
            listControlBindingSource2.DataMember = "rel1";

            if (dataSet.Tables[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("NO Data!");
            }
            btnCreate.Enabled = true;
            this.HideWaitMessage();
        }

        private void checkOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource))
            {
                return;
            }

            if (checkOnly.Checked)
            {
                listControlBindingSource1.Filter = "complete = 'Y'";
            }
            else
            {
                listControlBindingSource1.Filter = "";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void gridComplete_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
             gridRel.ValidateControl();
        }

        private void btnAutoPick_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(master))
                return;
            if (master.Rows.Count == 0)
                return;

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
                    dr["total_qty"] = 0.00;
                }

                if (dr["selected"].ToString() == "True" && !MyUtility.Check.Empty(dr["requestqty"]))
                {
                    var issued = PublicPrg.Prgs.autopick(dr, false, "I");
                    if (issued == null)
                        return;

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
  

            DataRow[] findrow = detail.Select(@"selected = true and qty <> 0");
            if (findrow.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!!");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to create data?");
            if (dResult == DialogResult.No)
                return;

            /*
             * 依照 To POID 建立 P24
             */
            var listPoid = findrow.Select(row => row["ToPOID"]).Distinct().ToList();
            var tmpId = Sci.MyUtility.GetValue.GetBatchID(Sci.Env.User.Keyword + "PI", "SubTransfer", System.DateTime.Now, batchNumber: listPoid.Count);
            if (MyUtility.Check.Empty(tmpId))
            {
                MyUtility.Msg.WarningBox("Get document ID fail!!");
                return;
            }

            #region 準備 insert subtransfer & subtransfer_detail 語法與 DataTable dtMaster & dtDetail
            //insert 欄位順序必須與 dtMaster, dtDetail 一致
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
UPDATE m
SET m.CLocation = t.ToLocation
FROM MDivisionPODetail m
INNER JOIN #tmp t ON t.FromPOID=m.POID AND t.FromSeq1=m.Seq1 AND t.FromSeq2=m.Seq2
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
                drNewMaster["FactoryID"] = Sci.Env.User.Factory;
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

            TransactionScope _transactionscope = new TransactionScope();
            DualResult result;
            using (_transactionscope)
            {
                try
                {
                    DataTable dtResult;
                    if ((result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, insertMaster, out dtResult)) == false)
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.ToString(), "Create failed");
                        return;
                    }

                    if ((result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, insertDetail, out dtResult)) == false)
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.ToString(), "Create failed");
                        return; ;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    
                }
                catch(Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope = null;

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

            string msg = string.Empty;
            msg += success_list.Count > 0 ? "Trans. ID" + Environment.NewLine + success_list.JoinToString(Environment.NewLine) + Environment.NewLine + "be created!! and Confirm Success!!" + Environment.NewLine : string.Empty;
            msg += fail_list.Count > 0 ? "Trans. ID" + Environment.NewLine + fail_list.JoinToString(Environment.NewLine) + Environment.NewLine + "be created!!, Confirm fail, please go to P24 manual Confirm" : string.Empty;
            this.p30_msg.Show(msg);
            #endregion

            if (!master.Columns.Contains("TransID")) master.Columns.Add("TransID", typeof(string));
            foreach (DataRow Alldetailrows in detail.Rows)
            {
                if (Alldetailrows["selected"].ToString().ToUpper() == "TRUE")
                {
                    DataRow[] drGetID = dtMaster.AsEnumerable().Where(row => row["poid"].EqualString(Alldetailrows["ToPOID"])).ToArray();
                    Alldetailrows.GetParentRow("rel1")["selected"] = true;
                    Alldetailrows.GetParentRow("rel1")["TransID"] = drGetID[0]["ID"];
                }
            }

            //Create後Btn失效，需重新Qurey才能再使用。
            btnCreate.Enabled = false;
            this.gridRel.ValidateControl();
            this.gridComplete.ValidateControl();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(master))
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Scarp");
                return;
            }
            if (!master.Columns.Contains("TransID"))
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Scarp");
                return;
            }
            master.DefaultView.RowFilter = "TransID<>''";
            if (MyUtility.Check.Empty(master))
            {
                return;
            }
            DataTable Exceldt = master.DefaultView.ToTable();
            if (Exceldt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Did not finish Inventory To Scarp");
                return;
            }
            // 篩選報表所顯示的欄位
            Exceldt.Columns.Remove("selected");
            Exceldt.Columns.Remove("CutInLine");
            Exceldt.Columns.Remove("ProjectID");
            Exceldt.Columns.Remove("FactoryID");
            Exceldt.Columns.Remove("stockpoid");
            Exceldt.Columns.Remove("stockseq1");
            Exceldt.Columns.Remove("stockseq2");
            Exceldt.Columns.Remove("qty");



            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P30.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(Exceldt, "", "Warehouse_P30.xltx", 2, showExcel: true, showSaveMsg: true, excelApp: excelApp);      // 將datatable copy to excel
            Marshal.ReleaseComObject(excelApp);
            
        }
    }
}
