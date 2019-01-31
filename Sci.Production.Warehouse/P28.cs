﻿using System;
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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Sci.Production.PublicForm;

namespace Sci.Production.Warehouse
{
    public partial class P28 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk2 = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        DataTable master, detail;
        public P28(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.ActiveControl = txtIssueSP;

            Category.SelectedIndex = 0;
            comboFabricType.SelectedIndex = 0;
            
            #region -- Grid1 設定 --
            this.gridComplete.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridComplete.DataSource = listControlBindingSource1;
            
            Helper.Controls.Grid.Generator(this.gridComplete)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out col_chk)
                .Text("complete", header: "Complete" + Environment.NewLine + "Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(3), iseditingreadonly: true,alignment:DataGridViewContentAlignment.MiddleCenter)
                 .Text("FinalETA", header: "Act. ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("poid", header: "Issue SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("seq1", header: "Issue" + Environment.NewLine + "Seq1", width: Widths.AnsiChars(2), iseditingreadonly: true)
                 .Text("seq2", header: "Issue" + Environment.NewLine + "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                 .Numeric("inputqty", header: "TPE Input", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("accu_qty", header: "Accu Trans.", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("VarianceQty", header: "Variance Qty", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("total_qty", header: "Trans. Qty", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("requestqty", header: "Balance", width: Widths.AnsiChars(6), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Text("TransID", header: "Trans. ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("PRHandle", header: "PR Handle", width: Widths.AnsiChars(2), iseditingreadonly: true)
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

            Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
            Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;
            #region -- transfer qty valid --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            ns.CellValidating += (s, e) =>
            {
                //if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                if (this.EditMode && e.FormattedValue!=null)
                {
                    DataRow thisRow = this.gridComplete.GetDataRow(this.listControlBindingSource1.Position);
                    DataRow[] curentgridrowChild = thisRow.GetChildRows("rel1");
                    DataRow currentrow = gridRel.GetDataRow(gridRel.GetSelectedRowIndex());
                    currentrow["qty"] = e.FormattedValue;
                    decimal total_qty = curentgridrowChild.Sum(row => (decimal)row["qty"]);
                    if (total_qty != 0)
                    {
                        currentrow.GetParentRow("rel1")["total_qty"] = total_qty;
                    }
                    else
                    {
                        currentrow.GetParentRow("rel1")["total_qty"] = DBNull.Value;
                    }
                    //currentrow.GetParentRow("rel1")["total_qty"] = curentgridrowChild.Sum(row => (decimal)row["qty"]);
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
                       e.Cancel = true;
                       MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                       
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
                if(gridRel.Columns[e.ColumnIndex].Name == col_chk2.Name){
                    DataRow dr = gridRel.GetDataRow(e.RowIndex);
                    if(Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0){
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
                    decimal total_qty =curentgridrowChild.Sum(row => (decimal)row["qty"]);
                    if (total_qty != 0)
                    {
                        currentrow.GetParentRow("rel1")["total_qty"] = total_qty;
                    }
                    else {
                        currentrow.GetParentRow("rel1")["total_qty"] = DBNull.Value;
                    }
                    //currentrow.GetParentRow("rel1")["total_qty"] = curentgridrowChild.Sum(row => (decimal)row["qty"]);
                    currentrow.EndEdit();
                }
            };

            Helper.Controls.Grid.Generator(this.gridRel)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out col_chk2)
                .Text("fromroll", header: "Roll#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("balanceQty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                .Numeric("qty", header: "Trans. Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, settings: ns).Get(out col_Qty)
                .Text("fromlocation", header: "From Bulk" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("tolocation", header: "To Inventory" + Environment.NewLine + "Location", width: Widths.AnsiChars(16), iseditingreadonly: false, settings: ts2).Get(out col_tolocation)
                  ;
            col_Qty.DefaultCellStyle.BackColor = Color.Pink;
            col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
            chp();
            #endregion
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
            string selectindex = Category.SelectedValue.ToString();
            int selectindex2 = comboFabricType.SelectedIndex;
            string ATA_b, ATA_e, InputDate_b, InputDate_e, SP,factory;
            ATA_b = null;
            ATA_e = null;
            InputDate_b = null;
            InputDate_e = null;
            SP = txtIssueSP.Text;
            factory = txtfactory.Text;
            if (dateMaterialATA.Value1 != null) ATA_b = this.dateMaterialATA.Text1;
            if (dateMaterialATA.Value2 != null) { ATA_e = this.dateMaterialATA.Text2; }

            if (dateInputDate.Value1 != null) { InputDate_b = this.dateInputDate.Text1; }
            if (dateInputDate.Value2 != null) { InputDate_e = this.dateInputDate.Text2; }

            if ((ATA_b == null && ATA_e == null) &&
                MyUtility.Check.Empty(SP) && 
                (InputDate_b == null && InputDate_e == null))
            {
                MyUtility.Msg.WarningBox(" < Cutting Inline > or < Order Confirm Date > or < Issue SP# > can't be empty!!");
                txtIssueSP.Focus();
                return;
            }

            StringBuilder sqlcmd = new StringBuilder();
            #region -- sql command --
            sqlcmd.Append(string.Format(@"
;with cte as(
    select  convert(bit,0) as selected
            , iif (y.cnt > 0, 'Y', '') complete
            , rtrim(o.id) poid
            , o.Category
            , o.FtyGroup
            , o.FactoryID 
            , rtrim(pd.seq1) seq1
            , pd.seq2
            , pd.id stockpoid
            , pd.seq1 stockseq1
            , pd.seq2 stockseq2
            , ROUND(dbo.GetUnitQty(pd.POUnit, pd.StockUnit, xz.taipei_qty),2) N'inputqty'
            , pd.POUnit
            , pd.StockUnit
            , isnull(x.accu_qty,0.00) accu_qty
            , VarianceQty=ROUND(dbo.GetUnitQty(pd.POUnit, pd.StockUnit, xz.taipei_qty),2) - isnull(x.accu_qty,0.00)
            , pr.PRHandle
            , pd.FinalETA
    from dbo.orders o WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = o.ID
    inner join dbo.Factory f WITH (NOLOCK) on f.id = o.FtyGroup
    inner join dbo.Factory checkProduceFty With (NoLock) on o.FactoryID = checkProduceFty.ID "));
            if (!(string.IsNullOrWhiteSpace(InputDate_b)))
            {
                sqlcmd.Append(string.Format(@" 
    cross apply (
	    select distinct 1 abc 
        from dbo.Invtrans WITH (NOLOCK) 
        where   type = '1'
                and (convert(varchar, ConfirmDate, 111) between convert(varchar, '{0}', 111) and convert(varchar, '{1}', 111)) 
                and poid = pd.id 
                and seq1 = pd.seq1 
                and seq2 = pd.seq2
    ) z ", InputDate_b, InputDate_e));
            }
            bool MtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
            string where = string.Empty;
            if (!MtlAutoLock)
            {
                where = " AND fi.lock = 0 ";
            }

            sqlcmd.Append($@" 
    outer apply (
        select count(1) cnt 
        from FtyInventory fi WITH (NOLOCK) 
        left join FtyInventory_Detail fid WITH (NOLOCK) on fid.Ukey = fi.Ukey 
	    where   fi.POID = pd.ID 
                and fi.Seq1 = pd.Seq1 
                and fi.Seq2 = pd.Seq2 
                and fi.StockType = 'B' 
                and fid.MtlLocationID is not null
                {where}
                and fi.InQty - fi.OutQty + fi.AdjustQty > 0
    ) y--Detail有MD為null數量,沒有則為0,沒資料也為0
    outer apply (
        select sum(sd.Qty) accu_qty 
        from dbo.SubTransfer s WITH (NOLOCK) 
        inner join dbo.SubTransfer_Detail sd WITH (NOLOCK) on sd.ID = s.Id 
        where   s.type = 'A' 
                and s.Status = 'Confirmed' 
                and sd.FromPOID = pd.ID 
                and sd.FromSeq1 = pd.SEQ1 
                and sd.FromSeq2 = pd.SEQ2 
                and FromStockType = 'B' 
                and toStockType='I'
    ) x
    cross apply (   
        select sum(i.Qty) taipei_qty
        from dbo.Invtrans i WITH (NOLOCK) 
        where   (i.type = '1' OR I.TYPE = '4') 
                and i.InventoryPOID = pd.ID 
                and i.InventorySeq1 = pd.seq1 
                and i.InventorySeq2 = pd.SEQ2
    ) xz
    outer apply(
        select PRHandle=concat(TPEPass1.ID,'-',REPLACE(TPEPass1.Name,' ',''), iif(isnull(TPEPass1.ExtNo,'')='','',' #'+TPEPass1.ExtNo))
        from TPEPass1
        inner join po on TPEPass1.id =PO.POHandle
        where PO.ID = o.id
    )pr
    where   f.MDivisionID = '{Env.User.Keyword}'
            and checkProduceFty.IsProduceFty = '1'
            and o.Category in ({selectindex})");
            
            #region -- 條件 --
        
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

            if (!MyUtility.Check.Empty(SP)) 
                sqlcmd.Append(string.Format(@" 
            and pd.id = '{0}'", SP));

            if (!MyUtility.Check.Empty(factory)) 
                sqlcmd.Append(string.Format(@" 
            and o.FtyGroup = '{0}'", factory));

            if (!(string.IsNullOrWhiteSpace(ATA_b)))
            {
                sqlcmd.Append(string.Format(@" 
            and pd.FinalETA between '{0}' and '{1}'", ATA_b, ATA_e));
            }

            #endregion
            sqlcmd.Append($@"
)
select  *
        , 0.00 qty 
into #tmp 
from cte 
where inputqty > accu_qty

select * 
from #tmp 
order by poid, seq1, seq2 ;

select  convert(bit,0) as selected
        , fi.Ukey FromFtyInventoryUkey
        , o.FactoryID fromFactoryID
        , fi.POID FromPoid
        , fi.Seq1 FromSeq1
        , fi.Seq2 Fromseq2
        , fi.Roll FromRoll
        , fi.Dyelot FromDyelot
        , fi.StockType FromStockType
        , fi.InQty - fi.OutQty + fi.AdjustQty BalanceQty
        , 0.00 as Qty
        , rtrim(t.poID) topoid
        , rtrim(t.seq1) toseq1
        , t.seq2 toseq2
        , fi.Roll toRoll
        , fi.Dyelot toDyelot
        , 'I' tostocktype 
        , t.FactoryID ToFactoryID
        , dbo.Getlocation(fi.ukey) fromlocation
        , tolocation = stuff ((select ',' + mtllocationid 
                               from (
                                    select MtlLocationid 
                                    from dbo.FtyInventory_Detail f WITH (NOLOCK)  
                                    inner join MtlLocation m WITH (NOLOCK) on f.MtlLocationID=m.ID 
                                    where   f.Ukey = fi.Ukey 
                                            and m.StockType = 'I' 
                                            and m.Junk != '1'
                               )t 
                               for xml path('')
                             ), 1, 1, '') 
        , GroupQty = Sum(fi.InQty - fi.OutQty + fi.AdjustQty) over(partition by t.poid,t.seq1,t.SEQ2,t.FactoryID,t.StockPOID,t.StockSeq1,t.StockSeq2,fi.Dyelot)
from #tmp t 
inner join FtyInventory fi WITH (NOLOCK) on  fi.POID = t.POID 
                                             and fi.seq1 = t.Seq1 
                                             and fi.Seq2 = t.Seq2
left join orders o on fi.poid=o.id
where   fi.StockType = 'B' 
{where}
        and fi.InQty - fi.OutQty + fi.AdjustQty > 0 
order by topoid, toseq1, toseq2, GroupQty DESC, fi.Dyelot, BalanceQty DESC
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

            detail = dataSet.Tables[1];
            detail.TableName = "Detail";

            DataRelation relation = new DataRelation("rel1"
                    , new DataColumn[] { master.Columns["poid"], master.Columns["seq1"], master.Columns["seq2"] }
                    , new DataColumn[] { detail.Columns["toPoid"], detail.Columns["toseq1"], detail.Columns["toseq2"] }
                    );

            dataSet.Relations.Add(relation);
            
            master.Columns.Add("total_qty", typeof(decimal));
            master.Columns.Add("requestqty", typeof(decimal), "InputQty - accu_qty - sum(child.qty)");

            if (listControlBindingSource1.DataSource != null)
            {
                listControlBindingSource1.DataSource = null;
            }
            if (listControlBindingSource2.DataSource != null)
            {
                listControlBindingSource2.DataSource = null;
            }
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


                if (dr["selected"].ToString().ToUpper() == "TRUE" && !MyUtility.Check.Empty(dr["requestqty"]))
                {
                    var issued = PublicPrg.Prgs.autopick(dr, false,"B");
                    if (issued == null) return;


                    foreach (DataRow dr2 in issued)
                    {
                        DataRow[] findrow = detail.Select(string.Format(@"fromFtyInventoryUkey = {0} and topoid = '{1}' and toseq1 = '{2}' and toseq2 = '{3}'"
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

            DataRow[] findrow = detail.AsEnumerable().Where(row=>row["selected"].EqualString("True")).ToArray();

            if (findrow.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!!");
                return;
            }

            /*
             * 依照 From POID 建立 P22
             */
            var listPoid = findrow.Select(row => row["frompoid"]).Distinct().ToList();
            var tmpId = Sci.MyUtility.GetValue.GetBatchID(Sci.Env.User.Keyword + "ST", "SubTransfer", System.DateTime.Now, batchNumber: listPoid.Count);
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
        , status, addname, adddate  , remark)
select  id      , type   , issuedate, mdivisionid, FactoryID
        , status, addname, adddate  , remark
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

            for (int i = 0; i < listPoid.Count; i++)
            {
                DataRow drNewMaster = dtMaster.NewRow();
                drNewMaster["poid"] = listPoid[i].ToString();
                drNewMaster["id"] = tmpId[i].ToString();
                drNewMaster["type"] = "A";
                drNewMaster["issuedate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                drNewMaster["mdivisionid"] = Env.User.Keyword;
                drNewMaster["FactoryID"] = Sci.Env.User.Factory;
                drNewMaster["status"] = "New";
                drNewMaster["addname"] = Env.User.UserID;
                drNewMaster["adddate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                drNewMaster["remark"] = "Batch create by P28";
                dtMaster.Rows.Add(drNewMaster);
            }

            foreach (DataRow item in findrow)
            {
                DataRow[] drGetID = dtMaster.AsEnumerable().Where(row => row["poid"].EqualString(item["frompoid"])).ToArray();
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
                    // MyUtility.Msg.InfoBox("Trans. ID" + Environment.NewLine + tmpId.JoinToString(Environment.NewLine) + Environment.NewLine + "be created!!", "Complete!");
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
                if (Alldetailrows["selected"].ToString().ToUpper() == "TRUE")
                {
                    DataRow[] drGetID = dtMaster.AsEnumerable().Where(row => row["poid"].EqualString(Alldetailrows["frompoid"])).ToArray();
                    Alldetailrows.GetParentRow("rel1")["selected"] = true;
                    Alldetailrows.GetParentRow("rel1")["TransID"] = drGetID[0]["ID"];
                }
            }
            //Create後Btn失效，需重新Qurey才能再使用。

            foreach (DataRow item in dtMaster.Rows)
            {
                DataTable dtd = dtDetail.Select($" id ='{item["id"]}'").CopyToDataTable();
                if (!Prgs.P22confirm(item, dtd))
                {
                    MyUtility.Msg.InfoBox("Trans. ID" + Environment.NewLine + tmpId.JoinToString(Environment.NewLine) + Environment.NewLine + "be created!!" + " , Confirm fail, please go to P22 manual Confirm ", "Complete!");
                    return;
                }
            }
            // MyUtility.Msg.InfoBox("Trans. ID" + Environment.NewLine + tmpId.JoinToString(Environment.NewLine) + Environment.NewLine + "be created!!"+ " and Confirm Success!! ", "Complete!");

            this.p13_msg.Show("Trans. ID" + Environment.NewLine + tmpId.JoinToString(Environment.NewLine) + Environment.NewLine + "be created!!" + " and Confirm Success!! ");

            btnCreate.Enabled = false;
            this.gridRel.ValidateControl();
            this.gridComplete.ValidateControl();
        }

        private msg p13_msg = new msg();

        private void checkOnly_CheckedChanged(object sender, EventArgs e)
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
                MyUtility.Msg.WarningBox("Did not finish Bulk To Inventory");
                return;
            }
            if (!master.Columns.Contains("TransID"))
            {
                MyUtility.Msg.WarningBox("Did not finish Bulk To Inventory");
                return;
            }
            master.DefaultView.RowFilter = "TransID<>''";
            DataTable Exceldt = master.DefaultView.ToTable();
            if (Exceldt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Did not finish Bulk To Inventory");
                return;
            }
            // 新增Excel物件
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            // 新增workbook
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Application.Workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1];
            // 固定視窗
            worksheet.Application.ActiveWindow.SplitRow = 2;
            worksheet.Application.ActiveWindow.FreezePanes = true;
            // 合併儲存格
            Excel.Range range = worksheet.get_Range((Excel.Range)worksheet.Cells[1, 1], (Excel.Range)worksheet.Cells[1, Exceldt.Columns.Count]);
            range.Merge(false);

            worksheet.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where id = '{0}'", Sci.Env.User.Keyword));
            ((Excel.Range)worksheet.Cells[1, 1]).HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;

            for (int i = 0; i < Exceldt.Columns.Count; i++)
            {
                worksheet.Cells[2, i + 1] = Exceldt.Columns[i].ColumnName;
            }

            int index = 0;
            foreach (DataRow dr in Exceldt.Rows)
            {
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    worksheet.Cells[3 + index, i + 1] = dr[i];
                }
                index++;
            }
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P28");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(range);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridComplete_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            gridRel.ValidateControl();
        }
    }
}
