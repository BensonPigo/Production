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
    public partial class P73_Import : Sci.Win.Subs.Base
    {
        public Sci.Win.Tems.Base P73;
        DataRow dr_master;
        DataTable dt_detail;
        DataSet dsTmp;
        StringBuilder strSQLCmd = new StringBuilder();
        protected DataTable dtBorrow;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataRelation relation;
        public P73_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
            Ict.Win.UI.DataGridViewTextBoxColumn col_Location;
            #region Location 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentRow = grid1.GetDataRow(grid1.GetSelectedRowIndex());
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(currentRow["stocktype"].ToString(), currentRow["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    currentRow["location"] = item.GetSelectedString();
                }
            };
            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = grid1.GetDataRow(e.RowIndex);
                    dr["location"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", dr["stocktype"].ToString(), Sci.Env.User.Keyword);
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["location"].ToString().Split(',').Distinct().ToArray();
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
                    dr["location"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion

            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = TaipeiOutputBS;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4)) 
                .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .EditText("description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) 
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(4))
                .Numeric("qty", header: "Received" + Environment.NewLine + "Qty", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8)).Get(out col_Qty)
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("Location", header: "Location", settings: ts2, iseditingreadonly: false, width: Widths.AnsiChars(30)).Get(out col_Location)
               ;

            col_Location.DefaultCellStyle.BackColor = Color.Pink;

                // 建立可以符合回傳的Cursor
                #region -- Sql Command --
                strSQLCmd.Append(string.Format(@"
select 0 as selected,'' id,d.ToMDivisionID MDivisionID,d.ToPOID poid,d.ToSeq1 seq1,d.ToSeq2 seq2, concat(Ltrim(Rtrim(d.toseq1)), ' ', d.toseq2) as seq
    , d2.Roll roll,d2.Dyelot dyelot,d2.Qty,'B' stocktype 
    , dbo.getMtlDesc(d.ToPOID,d.ToSeq1,d.ToSeq2,2,0) [description]
    , (select stockunit from dbo.po_supp_detail where id = d.topoid and seq1 = d.toseq1 and seq2 = d.toseq2) stockunit
    ,'' location
from dbo.RequestCrossM_Detail d 
inner join dbo.Issue_Detail d2 on d2.POID = d.FromPOID and d2.seq1 = d.FromSeq1 and d2.seq2 = d.FromSeq2
inner join dbo.Issue i on i.Id = d2.Id 
where d.id='{0}' and i.CutplanID = '{0}' and i.Status = 'Confirmed'
", dr_master["id"]));
                #endregion


                P73.ShowWaitMessage("Data Loading....");

                if (!SQL.Selects("", strSQLCmd.ToString(), out dsTmp)) 
                { return; }

                DataTable dtReceive = dsTmp.Tables[0];
                TaipeiOutputBS.DataSource = dtReceive;

                P73.HideWaitMessage();

        }

        // Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

         private void btn_Import_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataTable dt = (DataTable)TaipeiOutputBS.DataSource;
            DataRow[] dr2 = dt.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dt.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dt.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format(@"mdivisionid = '{0}' and poid = '{1}' and seq1 = '{2}' and seq2 = '{3}' 
                        and roll = '{4}' and dyelot = '{5}' and stocktype = '{6}'"
                    , tmp["Mdivisionid"], tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["dyelot"], tmp["stocktype"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["Location"] = tmp["Location"];
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

    }
}
