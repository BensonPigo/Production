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
        DataRow dr_master;
        DataTable dt_detail;
        DataSet dsTmp;
        StringBuilder strSQLCmd = new StringBuilder();
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

            #region StockType setting
            Ict.Win.DataGridViewGeneratorComboBoxColumnSettings sk = new DataGridViewGeneratorComboBoxColumnSettings();
            sk.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow CurrentDetailData = grid1.GetDataRow(e.RowIndex);
                    CurrentDetailData["stocktype"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", CurrentDetailData["stocktype"].ToString(), Sci.Env.User.Keyword);
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
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
                    CurrentDetailData["location"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
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
                .ComboBox("stocktype", header: "Stock" + Environment.NewLine + "Typt", width: Widths.AnsiChars(10), settings: sk).Get(out cbb_stocktype)
                .Text("Location", header: "Location", settings: ts2, iseditingreadonly: false, width: Widths.AnsiChars(30)).Get(out col_Location)
               ;

            col_Location.DefaultCellStyle.BackColor = Color.Pink;
            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

                // 建立可以符合回傳的Cursor
                #region -- Sql Command --
                strSQLCmd.Append(string.Format(@"
select 0 as selected,'' id,d.FromMDivisionID MDivisionID,d.FromPOID poid,d.FromSeq1 seq1,d.FromSeq2 seq2, left(d.FromSeq1+'   ',3)+d.FromSeq2 as seq
    , d2.Roll roll,d2.Dyelot dyelot,d2.Qty,'I' stocktype 
    , dbo.getMtlDesc(d.ToPOID,d.ToSeq1,d.ToSeq2,2,0) [description]
    , (select stockunit from dbo.po_supp_detail where id = d.topoid and seq1 = d.toseq1 and seq2 = d.toseq2) stockunit
    ,'' location
from dbo.RequestCrossM_Detail d 
inner join dbo.Issue_Detail d2 on d2.POID = d.ToPOID and d2.seq1 = d.ToSeq1 and d2.seq2 = d.ToSeq2
inner join dbo.Issue i on i.Id = d2.Id 
where d.id=i.CutplanID and i.id = '{0}' and i.Status = 'Confirmed' and d.FromMDivisionID = '{1}'
", dr_master["referenceid"], Sci.Env.User.Keyword));
                #endregion


                MyUtility.Msg.WaitWindows("Data Loading....");

                if (!SQL.Selects("", strSQLCmd.ToString(), out dsTmp)) 
                { return; }

                DataTable dtReceive = dsTmp.Tables[0];
                TaipeiOutputBS.DataSource = dtReceive;

                MyUtility.Msg.WaitClear();

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
                        and roll = '{4}' and dyelot = '{5}'"
                    , tmp["Mdivisionid"], tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["dyelot"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["stocktype"] = tmp["stocktype"];
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
