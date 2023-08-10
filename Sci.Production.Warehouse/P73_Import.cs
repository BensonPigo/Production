using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P73_Import : Win.Subs.Base
    {
        private DataTable dt_detail;
        private DataTable dtDetail;
        private Dictionary<string, string> selectedLocation = new Dictionary<string, string>();

        /// <inheritdoc/>
        public P73_Import(DataTable detail)
        {
            this.InitializeComponent();
            this.dt_detail = detail;
            this.EditMode = true;
        }

        // Button Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();
            string color = this.txtColor.Text.TrimEnd();
            string roll = this.txtRoll.Text.TrimEnd();
            string dyelot = this.txtDyelot.Text.TrimEnd();
            string locationid = this.txtLocation.Text.TrimEnd();

            // SP#不可為空
            if (MyUtility.Check.Empty(sp))
            {
                MyUtility.Msg.WarningBox("SP# cannt be empty.");
                return;
            }

            strSQLCmd.Append(
$@"
select * from (

    select  [selected] = 0,
            loi.POID,
            [Seq] = CONCAT(loi.Seq1,' ',loi.Seq2),
		    loi.Seq1,loi.Seq2,
            StockType = 'B',
            StockTypeName = 'Bulk', 
		    loi.Roll,
            loi.Dyelot,
		    lom.Refno,
		    lom.[Desc],
		    lom.Color,        
		    [Qty] = loi.InQty - loi.OutQty + loi.AdjustQty,
            [FromLocation] = Location.val,
		    [ToLocation] = ''
    from    LocalOrderInventory loi with (nolock)
    left join LocalOrderMaterial lom with (nolock) on loi.Poid = lom.Poid and loi.Seq1 = lom.Seq1 and loi.Seq2 = lom.Seq2
    outer apply (
	    SELECT val =  Stuff((select distinct concat( ',',MtlLocationID)   
	    from LocalOrderInventory_Location loil with (nolock)
	    WHERE loil.LocalOrderInventoryUkey	 = loi.Ukey
	    FOR XML PATH('')),1,1,'')  
    ) Location
    where  1=1
    and loi.StockType = 'B' 
) a
where 1=1
");
            if (!MyUtility.Check.Empty(sp))
            {
                strSQLCmd.Append(string.Format(
                    @" 
        and poid like '%{0}%' ", sp));
            }

            if (!this.txtSeq.CheckSeq1Empty() && this.txtSeq.CheckSeq2Empty())
            {
                strSQLCmd.Append(string.Format(
                    @" 
        and seq1 = '{0}'", this.txtSeq.Seq1));
            }
            else if (!this.txtSeq.CheckEmpty(showErrMsg: false))
            {
                strSQLCmd.Append($@"and seq1 = '{this.txtSeq.Seq1}' and seq2='{this.txtSeq.Seq2}'");
            }

            if (!MyUtility.Check.Empty(locationid))
            {
                strSQLCmd.Append(string.Format(
                    @" 
        and FromLocation like '%{0}%' ", locationid));
            }

            if (!MyUtility.Check.Empty(dyelot))
            {
                strSQLCmd.Append(string.Format(
                    @" 
        and dyelot like '%{0}%' ", dyelot));
            }

            if (!MyUtility.Check.Empty(color))
            {
                strSQLCmd.Append(string.Format(
                    @" 
        and Color like '%{0}%'", color));
            }

            if (!MyUtility.Check.Empty(roll))
            {
                strSQLCmd.Append(string.Format(
                    @" 
        and Roll like '%{0}%' ", roll));
            }

            this.ShowWaitMessage("Data Loading....");
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtDetail)))
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }
            else
            {
                if (this.dtDetail.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtDetail;
            }

            this.HideWaitMessage();

            // 全部撈完，再利用Checked change事件，觸發filter過濾資料
            switch (this.BalanceQty.Checked)
            {
                case true:
                    this.BalanceQty.Checked = false;
                    this.BalanceQty.Checked = true;
                    break;
                case false:
                    this.BalanceQty.Checked = true;
                    this.BalanceQty.Checked = false;
                    break;
                default:
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtSPNo.Focus();
            #region Location 右鍵開窗
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentrow = this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex());
                    Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation("B", currentrow["ToLocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    currentrow["ToLocation"] = item.GetSelectedString();
                    currentrow.EndEdit();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    dr["ToLocation"] = e.FormattedValue;
                    string sqlcmd = $@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='B'
        and junk != '1'";
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
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Cannot found Location : " + string.Join(",", errLocation.ToArray()) + " .", "Data not found");
                    }

                    trueLocation.Sort();
                    dr["ToLocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                    dr["selected"] = (!string.IsNullOrEmpty(dr["ToLocation"].ToString())) ? 1 : 0;

                    this.gridImport.RefreshEdit();
                }
            };
            #endregion Location 右鍵開窗

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0) // 0
                .Text("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true) // 1
                .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(9), iseditingreadonly: true) // 3
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 4
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true) // 5
                .EditText("Desc", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 6
                .Text("color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true) // 7
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 8
                .Text("FromLocation", header: "FromLocation", iseditingreadonly: true) // 9
                .Text("ToLocation", header: "ToLocation", settings: ts2, iseditingreadonly: false) // 10
            ;

            this.gridImport.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            this.gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            // 若無勾選，表示Qty <=0 的資料都被隱藏了，在這邊過濾掉
            // Qty > 0 一定都會在
            bool check_BalanceQty = this.BalanceQty.Checked;

            var chkLocationItems = dtGridBS1.AsEnumerable().Where(s => (int)s["selected"] == 1 && MyUtility.Check.Empty(s["ToLocation"]));
            if (chkLocationItems.Any())
            {
                MyUtility.Msg.WarningBox("To Location cannot be empty.", "Warnning");
                return;
            }

            DataRow[] dr2 = check_BalanceQty ? dtGridBS1.Select("Selected = 1 AND Qty>0") : dtGridBS1.Select("Selected = 1");

            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                if (tmp.RowState != DataRowState.Deleted)
                {
                    DataRow[] findrow = this.dt_detail.AsEnumerable().Where(
                    row => row.RowState != DataRowState.Deleted &&
                    row["poid"].EqualString(tmp["poid"].ToString()) &&
                    row["seq1"].EqualString(tmp["seq1"].ToString()) &&
                    row["seq2"].EqualString(tmp["seq2"].ToString()) &&
                    row["roll"].EqualString(tmp["roll"].ToString()) &&
                    row["dyelot"].EqualString(tmp["dyelot"].ToString())
                    ).ToArray();

                    if (findrow.Length > 0)
                    {
                        findrow[0]["qty"] = tmp["qty"];
                        findrow[0]["tolocation"] = tmp["tolocation"];
                        findrow[0]["fromlocation"] = tmp["fromlocation"];
                    }
                    else
                    {
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        this.dt_detail.ImportRow(tmp);
                    }
                }
            }

            this.Close();
        }

        private void TxtLocation2_MouseDown(object sender, MouseEventArgs e)
        {
            Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation("B", string.Empty);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            var select_result = item.GetSelecteds()
                .GroupBy(s => new { StockType = s["StockType"].ToString()})
                .Select(g => new { g.Key.StockType, ToLocation = string.Join(",", g.Select(i => i["id"])) });

            if (select_result.Count() > 0)
            {
                this.selectedLocation.Clear();
                this.txtLocation2.Text = string.Empty;
            }

            foreach (var result_item in select_result)
            {
                this.selectedLocation.Add(result_item.StockType, result_item.ToLocation);
                this.txtLocation2.Text += $"({result_item.StockType}:{result_item.ToLocation})";
            }
        }

        private void BtnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                if (this.selectedLocation.ContainsKey(item["StockTypeName"].ToString()))
                {
                    item["tolocation"] = this.selectedLocation[item["StockTypeName"].ToString()];
                }
            }
        }

        // 動態顯示列表資料
        private void BalanceQty_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void Grid_Filter()
        {
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            string filter = string.Empty;
            if (this.BalanceQty.Checked)
            {
                filter = $@"qty > 0";
            }

            ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
        }
    }
}
