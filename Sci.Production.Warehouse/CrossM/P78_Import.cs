using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.PublicPrg;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P78_Import : Sci.Win.Subs.Base
    {
        public Sci.Win.Tems.Base P78;
        DataRow dr_master;
        DataTable dt_detail;
        DataSet dsTmp;
        DataSet dsTmp2;
        StringBuilder strSQLCmd = new StringBuilder();
        StringBuilder strSQLCmd2 = new StringBuilder();

        private int grid1SelectIndex = 0;

        protected DataTable dtBorrow;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataRelation relation;
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        public P78_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
            dr_master = master;
            dt_detail = detail;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
            Ict.Win.UI.DataGridViewTextBoxColumn col_Location;

            #region Location 右鍵開窗
            //Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            //ts2.EditingMouseDown += (s, e) =>
            //{
            //    if (this.EditMode && e.Button == MouseButtons.Right)
            //    {
            //        DataRow currentRow = grid1.GetDataRow(grid1.GetSelectedRowIndex());
            //        Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(currentRow["stocktype"].ToString(), currentRow["location"].ToString());
            //        DialogResult result = item.ShowDialog();
            //        if (result == DialogResult.Cancel) { return; }
            //        currentRow["location"] = item.GetSelectedString();
            //    }
            //};

            //ts2.CellValidating += (s, e) =>
            //{
            //    if (this.EditMode && e.FormattedValue != null)
            //    {
            //        DataRow dr = grid1.GetDataRow(e.RowIndex);
            //        dr["location"] = e.FormattedValue;
            //        string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", dr["stocktype"].ToString(), Sci.Env.User.Keyword);
            //        DataTable dt;
            //        DBProxy.Current.Select(null, sqlcmd, out dt);
            //        string[] getLocation = dr["location"].ToString().Split(',').Distinct().ToArray();
            //        bool selectId = true;
            //        List<string> errLocation = new List<string>();
            //        List<string> trueLocation = new List<string>();
            //        foreach (string location in getLocation)
            //        {
            //            if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
            //            {
            //                selectId &= false;
            //                errLocation.Add(location);
            //            }
            //            else if (!(location.EqualString("")))
            //            {
            //                trueLocation.Add(location);
            //            }
            //        }

            //        if (!selectId)
            //        {
            //            MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
            //            e.Cancel = true;
            //        }
            //        trueLocation.Sort();
            //        dr["location"] = string.Join(",", (trueLocation).ToArray());
            //        //去除錯誤的Location將正確的Location填回
            //    }
            //};
            #endregion

            #region StockType setting
            //Ict.Win.DataGridViewGeneratorComboBoxColumnSettings sk = new DataGridViewGeneratorComboBoxColumnSettings();
            //sk.CellValidating += (s, e) =>
            //{
            //    if (this.EditMode && e.FormattedValue != null)
            //    {
            //        DataRow CurrentDetailData = grid1.GetDataRow(e.RowIndex);
            //        CurrentDetailData["stocktype"] = e.FormattedValue;
            //        string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", CurrentDetailData["stocktype"].ToString(), Sci.Env.User.Keyword);
            //        DataTable dt;
            //        DBProxy.Current.Select(null, sqlcmd, out dt);
            //        string[] getLocation = CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
            //        bool selectId = true;
            //        List<string> errLocation = new List<string>();
            //        List<string> trueLocation = new List<string>();
            //        foreach (string location in getLocation)
            //        {
            //            if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
            //            {
            //                selectId &= false;
            //                errLocation.Add(location);
            //            }
            //            else if (!(location.EqualString("")))
            //            {
            //                trueLocation.Add(location);
            //            }
            //        }

            //        if (!selectId)
            //        {
            //            MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
            //            e.Cancel = true;
            //        }
            //        trueLocation.Sort();
            //        CurrentDetailData["location"] = string.Join(",", (trueLocation).ToArray());
            //        //去除錯誤的Location將正確的Location填回
            //    }
            //};
            #endregion

            #region StockType
            Ict.Win.DataGridViewGeneratorTextColumnSettings ns = new DataGridViewGeneratorTextColumnSettings();
            ns.CellFormatting = (s, e) =>
            {
                DataRow dr = grid1.GetDataRow(e.RowIndex);
                switch (dr["StockType"].ToString())
                {
                    case "B":
                        e.Value = "Bulk";
                        break;
                    case "I":
                        e.Value = "Inventory";
                        break;
                }
            };
            #endregion
            #region ReciveQty
            Ict.Win.DataGridViewGeneratorNumericColumnSettings cs = new DataGridViewGeneratorNumericColumnSettings();

            cs.CellValidating += (s, e) =>
            {
                //check 判斷總數在Borrowing or Return 超過
                bool checkBorrowingQty = false, checkReturnQty = false;
                //紀錄 Borrowing and Return 總數
                decimal sumBorrowing = 0, sumReturn = 0;
                //Get Grid2 目前選取的Data
                DataRow grid2Dr = grid2.GetDataRow(e.RowIndex);               
                grid2Dr["ReciveQty"] = 0;

                #region Grid1.Qty <= BorrowingQty
                DataRow grid1Dr = grid1.GetDataRow(grid1SelectIndex);

                //Borrowing 加總條件 BorrowingSP, BorrowingSeq, StockType
                DataRow[] findrow = grid2.GetTable().Select(string.Format("BorrowingSP = '{0}' and BorrowingSeq = '{1}' and StockType = '{2}'", grid1Dr["BorrowingSP"], grid1Dr["BorrowingSeq"], grid1Dr["StockType"]));
                foreach (DataRow dr in findrow)
                    sumBorrowing += Convert.ToDecimal(dr["ReciveQty"]);

                if ((sumBorrowing + Convert.ToDecimal(e.FormattedValue)) <= Convert.ToDecimal(grid1Dr["BorrowingQty"]))
                    checkBorrowingQty = true;

                findrow = null;
                #endregion

                #region Grid2.Accu <= ReturnQty
                //Return 加總條件 ReturnSp, ReturnSeq, Roll, Dyelot
                findrow = grid2.GetTable().Select(string.Format("ReturnSP = '{0}' and ReturnSeq = '{1}' and Roll = '{2}' and Dyelot = '{3}'", grid1Dr["ReturnSP"], grid1Dr["ReturnSeq"], grid2Dr["Roll"], grid2Dr["Dyelot"]));
                foreach (DataRow dr in findrow)
                    sumReturn += Convert.ToDecimal(dr["ReciveQty"]);

                if ((sumReturn + Convert.ToDecimal(e.FormattedValue)) <= Convert.ToDecimal(grid2Dr["ReturnQty"]))
                    checkReturnQty = true;
                #endregion

                if (checkBorrowingQty & checkReturnQty)
                {
                    grid2Dr["ReciveQty"] = e.FormattedValue;

                    //寫回 AccuDiff
                    foreach(DataRow dr in findrow)
                        dr["AccuDiffReciveQty"] = Convert.ToDecimal(grid2Dr["ReturnQty"]) - (sumReturn + Convert.ToDecimal(grid2Dr["ReciveQty"]));

                    grid1Dr["Qty"] = (sumBorrowing + Convert.ToDecimal(grid2Dr["ReciveQty"]));
                }
                else
                {
                    string errStr = "";
                    errStr += (checkBorrowingQty) ? "" : string.Format("<ReciveQty> : {0} can't more than <BorrowingQty> : {1}\n", sumBorrowing + Convert.ToDecimal(e.FormattedValue), grid1Dr["BorrowingQty"]);
                    errStr += (checkReturnQty) ? "" : string.Format("<AccuReciveQty> : {0} can't more than <ReturnQty> : {1}", sumReturn + Convert.ToDecimal(e.FormattedValue), grid2Dr["ReturnQty"]);

                    decimal AccuDiffReciveQty = (Convert.ToDecimal(grid1Dr["BorrowingQty"]) - sumBorrowing);
                    decimal AccuDiffQty = (Convert.ToDecimal(grid2Dr["ReturnQty"]) - sumReturn);
                    grid2Dr["ReciveQty"] = (AccuDiffReciveQty <= AccuDiffQty) ? AccuDiffReciveQty : AccuDiffQty;

                    //寫回 AccuDiff
                    foreach (DataRow dr in findrow)
                        dr["AccuDiffReciveQty"] = Convert.ToDecimal(grid2Dr["ReturnQty"]) - sumReturn - Convert.ToDecimal(grid2Dr["ReciveQty"]);

                    grid1Dr["AccuDiffQty"] = Convert.ToDecimal(grid1Dr["BorrowingQty"]) - sumBorrowing - Convert.ToDecimal(grid2Dr["ReciveQty"]);
                    grid1Dr["Qty"] = sumBorrowing + Convert.ToDecimal(grid2Dr["ReciveQty"]);

                    MyUtility.Msg.ErrorBox(errStr);
                    //
                }
                
                //Grid2 AccuDiffReciveQty == 0 then change Color
                grid2_changeColor();

                //Grid1 AccuDiffQty == 0 then check = 1
                grid1Dr["AccuDiffQty"] = Convert.ToDecimal(grid1Dr["BorrowingQty"]) - (sumBorrowing + Convert.ToDecimal(grid2Dr["ReciveQty"]));
                if (Convert.ToDecimal(grid1Dr["AccuDiffQty"]) == 0)
                {
                    grid1Dr["ReciveCheck"] = 1;
                    //grid1.Rows[grid1SelectIndex].Cells["AccuDiffQty"].Style.BackColor = Color.Gray;
                }
                else
                {
                    grid1Dr["ReciveCheck"] = 0;
                    //grid1.Rows[grid1SelectIndex].Cells["AccuDiffQty"].Style.BackColor = Color.White;
                }

                grid1.RefreshEdit();
                grid2.RefreshEdit();
            };
            #endregion

            Ict.Win.UI.DataGridViewNumericBoxColumn col_ReciveQty;

            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = TaipeiOutputBS;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("BorrowingSP", header: "Borrowing" + Environment.NewLine + "SP#", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("BorrowingSeq", header: "Borrowing" + Environment.NewLine + "Seq", iseditingreadonly: true, width: Widths.AnsiChars(9))
                .Text("StockType", header: "StockType", iseditingreadonly: true, width: Widths.AnsiChars(10), settings: ns)
                .Numeric("BorrowingQty", header: "BorrowingQty", decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
                //.Text("ReturnSP", header: "Return" + Environment.NewLine + "SP#", iseditingreadonly: true, width: Widths.AnsiChars(20))
                //.Text("ReturnSeq", header: "Return" + Environment.NewLine + "Seq", iseditingreadonly: true, width: Widths.AnsiChars(9))
                //.Numeric("AccuDiffQty", header: "Accu. Diff." + Environment.NewLine + "Qty", decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("Qty", header: "Accu. Assign" + Environment.NewLine + "Qty", decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
                .CheckBox("ReciveCheck", header: "Complete", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0);


            this.grid2.IsEditingReadOnly = false;
            this.grid2.DataSource = TaipeiOutputBS_Detail;
            Helper.Controls.Grid.Generator(this.grid2)
                //.Text("BorrowingSP", header: "Borrowing" + Environment.NewLine + "SP#", iseditingreadonly: true, width: Widths.AnsiChars(20))
                //.Text("BorrowingSeq", header: "Borrowing" + Environment.NewLine + "Seq", iseditingreadonly: true, width: Widths.AnsiChars(9))
                //.Text("StockType", header: "StockType", iseditingreadonly: true, width: Widths.AnsiChars(10))
                //.Text("StockUnit", header: "StockUnit", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("ReturnSP", header: "Return" + Environment.NewLine + "SP#", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("ReturnSeq", header: "Return" + Environment.NewLine + "Seq", iseditingreadonly: true, width: Widths.AnsiChars(9))
                .Text("Roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("ReturnQty", header: "Return" + Environment.NewLine + "Qty", decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("AccuDiffReciveQty", header: "Accu. Diff." + Environment.NewLine + "Assign Qty", decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))               
                .CheckBox("AssignCheck", header: "Assign", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
                .Numeric("ReciveQty", header: "Assign" + Environment.NewLine + "Qty", iseditingreadonly: false, integer_places: 10, decimal_places: 2, width: Widths.AnsiChars(8), settings: cs).Get(out col_ReciveQty);

            col_ReciveQty.DefaultCellStyle.BackColor = Color.Pink;

                // 建立可以符合回傳的Cursor
                #region -- Sql Command --
                strSQLCmd.Append(string.Format(@"
with returnSP as(
	select distinct ID, POID, Seq1, Seq2 from RequestCrossM_Receive RCM
	where RCM.Id = (select CutplanID from Issue where ID = '{0}')
)
select 
ReciveCheck     = 0,
BorrowingSP		= ID.POID,
BorrowingSeq	= concat(ID.Seq1, ' ', ID.Seq2),
StockType		= ID.StockType,
BorrowingQty	= sum(ID.Qty),
ReturnSP		= RSP.POID,
ReturnSeq		= concat(RSP.Seq1, ' ', RSP.Seq2),
AccuDiffQty     = sum(ID.Qty),
Qty				= 0.00
--,
--FromSeq1		= ID.Seq1,
--FromSeq2		= ID.Seq2,
--ToSeq1			= RCM.Seq1,
--ToSeq2			= RCM.Seq2
from Issue I
inner join Issue_Detail ID on I.Id = ID.id
inner join returnSP RSP on I.CutplanID = RSP.id

where I.CutplanID = (select CutplanID from Issue where ID = '{0}')
and i.MDivisionID= '{1}'
and I.Status = 'Confirmed'
group by ID.POID, concat(ID.Seq1, ' ', ID.Seq2), ID.StockType, RSP.POID, concat(RSP.Seq1, ' ', RSP.Seq2)
", dr_master["referenceid"], Sci.Env.User.Keyword));

                strSQLCmd2.Append(string.Format(@"
with returnSP as(
	select distinct ID, POID, Seq1, Seq2 from RequestCrossM_Receive RCM
	where RCM.Id = (select CutplanID from Issue where ID = '{0}')
),grid1 as (
    select 
    BorrowingSP		= ID.POID,
    BorrowingSeq	= concat(ID.Seq1, ' ', ID.Seq2),
    StockType		= ID.StockType,
    BorrowingQty	= sum(ID.Qty),
    ReturnSP		= RSP.POID,
    ReturnSeq		= concat(RSP.Seq1, ' ', RSP.Seq2),
    Qty				= 0.00,
	BorrowingSeq1	= ID.Seq1,
	BorrowingSeq2	= ID.Seq2,
	ReturnSeq1		= RSP.Seq1,
	ReturnSeq2		= RSP.Seq2
    --,
    --FromSeq1		= ID.Seq1,
    --FromSeq2		= ID.Seq2,
    --ToSeq1			= RCM.Seq1,
    --ToSeq2			= RCM.Seq2
    from Issue I
    inner join Issue_Detail ID on I.Id = ID.id
    inner join returnSP RSP on I.CutplanID = RSP.id

    where I.CutplanID = (select CutplanID from Issue where ID = '{0}')
    and i.MDivisionID= '{1}'
    and I.Status = 'Confirmed'
    group by ID.POID, concat(ID.Seq1, ' ', ID.Seq2), ID.StockType, RSP.POID, concat(RSP.Seq1, ' ', RSP.Seq2), ID.Seq1, ID.Seq2, RSP.Seq1, RSP.Seq2
),
grid2 as(
	select 
	ReturnSP		    = ID.POID,
	ReturnSeq		    = concat(ID.Seq1, ' ', ID.Seq2),
	Dyelot			    = ID.Dyelot,
	Roll			    = ID.Roll,
	ReturnQty		    = ID.Qty,
	ReciveQty		    = 0.00,
	AccuDiffReciveQty	= ID.Qty,
	ReturnSeq1		    = ID.Seq1,
	ReturnSeq2		    = ID.Seq2
	from Issue I 
	inner join Issue_Detail ID on I.Id = ID.Id
	where I.Id = '{0}' and I.Status = 'Confirmed'
)
select 
    AssignCheck     = 1,
    id              = '',
	g1.BorrowingSP,
	g1.BorrowingSeq,	
	g1.StockType,
	stockunit       = (select stockunit from dbo.po_supp_detail where id = g2.ReturnSP and seq1 = g2.ReturnSeq1 and seq2 = g2.ReturnSeq2),
	g2.ReturnSp,
	g2.ReturnSeq,
	g2.Roll,
	g2.Dyelot,
	g2.ReturnQty, 
	g2.AccuDiffReciveQty,
	g2.ReciveQty,	
    description     = dbo.getmtldesc(g1.BorrowingSP, g1.BorrowingSeq1, g1.BorrowingSeq2, 2, 0),
	g1.BorrowingSeq1,
	g1.BorrowingSeq2
from grid1 g1 
inner join grid2 g2 on g1.ReturnSP = g2.ReturnSP and g1.ReturnSeq = g2.ReturnSeq
order by g1.BorrowingSp, g1.BorrowingSeq, g2.ReturnSP, g2.ReturnSeq, Roll, Dyelot, StockType
", dr_master["referenceid"], Sci.Env.User.Keyword));
                #endregion                        
                
                P78.ShowWaitMessage("Data Loading");
                if (!SQL.Selects("", strSQLCmd.ToString(), out dsTmp)) 
                { return; }

                if (!SQL.Selects("", strSQLCmd2.ToString(), out dsTmp2))
                { return; }

                TaipeiOutputBS.DataSource = dsTmp.Tables[0];
                TaipeiOutputBS_Detail.DataSource = dsTmp2.Tables[0];
                P78.HideWaitMessage();
        }

        // Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

         private void btn_Import_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            grid2.ValidateControl();

            #region check Qty 
            foreach (DataRow dr in grid2.GetTable().Rows)
            {
                if (Convert.ToDecimal(dr["AccuDiffReciveQty"]) > 0)
                {
                    MyUtility.Msg.WarningBox(string.Format("<AccuDiffReciveQty> : {0} can't more than 0", dr["AccuDiffReciveQty"]));
                    return;
                }
            }
            #endregion

            grid2.GetTable().Columns["BorrowingSP"].ColumnName = "POID";
            grid2.GetTable().Columns["BorrowingSeq"].ColumnName = "Seq";
            grid2.GetTable().Columns["BorrowingSeq1"].ColumnName = "Seq1";
            grid2.GetTable().Columns["BorrowingSeq2"].ColumnName = "Seq2";
            grid2.GetTable().Columns["ReciveQty"].ColumnName = "qty";
            DataRow[] findRow = grid2.GetTable().Select("qty > 0");

            foreach (DataRow tmp in findRow)
            {
                DataRow[] findrow = dt_detail.Select(string.Format(@"mdivisionid = '{0}' and poid = '{1}' and seq1 = '{2}' and seq2 = '{3}' 
                        and roll = '{4}' and dyelot = '{5}'"
                    , Sci.Env.User.Keyword, tmp["POID"], tmp["Seq1"], tmp["Seq2"], tmp["Roll"], tmp["Dyelot"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["stocktype"] = tmp["StockType"];
                    findrow[0]["qty"] = tmp["qty"];
                    //findrow[0]["Location"] = tmp["Location"];
                }
                else
                {
                    tmp["id"] = dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }


            this.Close();
        }

         private void grid1_RowSelecting(object sender, Ict.Win.UI.DataGridViewRowSelectingEventArgs e)
         {
             grid1SelectIndex = e.RowIndex;
             DataRow dr = grid1.GetDataRow(grid1SelectIndex);
             TaipeiOutputBS_Detail.Filter = string.Format("BorrowingSP = '{0}' and BorrowingSeq = '{1}' and StockType = '{2}'", dr["BorrowingSP"], dr["BorrowingSeq"], dr["StockType"]);
         }

         private void grid1_RowEnter(object sender, DataGridViewCellEventArgs e)
         {
             grid2.ValidateControl();
             grid2_changeColor();
         }

         private void grid2_changeColor()
         {
             for (int i = 0; i < grid2.Rows.Count; i++)
             {
                 if (Convert.ToDecimal(grid2.Rows[i].Cells["AccuDiffReciveQty"].Value) == 0)
                 {
                     grid2.Rows[i].Cells["AssignCheck"].Value = 0;
                     grid2.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                 }
                 else
                 {
                     grid2.Rows[i].Cells["AssignCheck"].Value = 1;
                     grid2.Rows[i].DefaultCellStyle.BackColor = Color.White;
                 }
             }
         }
    }
}
