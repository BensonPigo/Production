using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P55_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private DataTable dtArtwork;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private Dictionary<string, string> selectedLocation = new Dictionary<string, string>();

        /// <inheritdoc/>
        public P55_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.CellValueChanged += (s, e) =>
            {
                if (this.grid1.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                    dr.EndEdit();
                    this.Sum_checkedqty();
                }
            };

            #region -- Location 右鍵開窗 --

            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr_nowgridrow = this.grid1.GetDataRow(e.RowIndex);
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(dr_nowgridrow["stocktype"].ToString(), dr_nowgridrow["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.grid1.GetDataRow(e.RowIndex)["location"] = item.GetSelectedString();
                    this.grid1.GetDataRow(e.RowIndex).EndEdit();
                }
            };

            ts.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr_nowgridrow = this.grid1.GetDataRow(e.RowIndex);

                    // 去除錯誤的Location將正確的Location填回
                    string newLocation = string.Empty;
                    DualResult result = CheckDetailStockTypeLocation(dr_nowgridrow["stocktype"].ToString(), e.FormattedValue.ToString(), out newLocation);
                    if (!result)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(result.Description);
                    }

                    /*dr_nowgridrow["Location"] = newLocation;*/
                }
            };
            #endregion Location 右鍵開窗

            this.grid1.IsEditingReadOnly = false; // 開啟CheckBox圖示
            this.grid1.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("POID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(12)) // 1
                .Text("Seq", header: "Seq", iseditingreadonly: true) // 2
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 4
                .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 5
                .Numeric("ReceivingQty", header: "Receiving Qty", decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(15), integer_places: 10) // 6
                .Text("Location", header: "Location", iseditingreadonly: false, width: Widths.AnsiChars(20), settings: ts)　// 7
                .Numeric("Qty", header: "Transfer Out Qty", decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(15), integer_places: 10) // 8
                .Text("StockUnit", header: "Stock Unit", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 9
                .Text("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25))
                .Text("StockTypeDisplay", header: "Stock Type", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Tone", header: "Tone/Grp", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Grade", header: "Grade", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("RecvKg", header: "Recv (Kg)", iseditingreadonly: true, width: Widths.AnsiChars(5))
                ;
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            object localPrice = dt.Compute("Sum(Qty)", "Selected = 1");
            this.displayTotal.Value = localPrice.ToString();
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSP.Text.TrimEnd();
            string tsi = this.txtTransfertoSubconID.Text.Trim();
            string tod = this.dateTransferOutDate.Value.ToString();
            string strSubCon = this.dr_master["SubCon"].ToString();
            string strID = this.dr_master["ID"].ToString();

            if (MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(tsi) && MyUtility.Check.Empty(tod))
            {
                MyUtility.Msg.WarningBox("Transfer to Sub con ID, Transfer Out Date and SP# cannot all be empty.");
                this.txtSP.Focus();
                return;
            }

            strSQLCmd.Append($@"
SELECT
    selected = 0
   ,ID = ''
   ,f.POID
   ,[Seq] = CONCAT(f.Seq1, ' ', f.Seq2)
   ,f.Seq1
   ,f.Seq2
   ,f.Roll
   ,f.Dyelot
   ,f.StockType
   ,StockTypeDisplay =
    CASE f.StockType
        WHEN 'B' THEN 'Bulk'
        WHEN 'I' THEN 'Inventory'
    END
   ,f.Tone
   ,[Description] = Dbo.getMtlDesc(f.POID, f.Seq1, f.Seq2, 2, 0)
   ,f.SubConStatus
   ,psd.Refno
   ,[StockUnit] = psd.StockUnit
   ,[ReceivingQty] = ISNULL(rdQty.ActualQty, 0) + ISNULL(tidQty.Qty, 0)
   ,[Qty] = td.Qty
   ,[TransferToSubcon_DetailUkey] = td.Ukey
   ,fir.Grade
   ,[RecvKG] = td.RecvKG
    ,o.StyleID
    ,[Location] = ''
FROM FtyInventory f WITH (NOLOCK)
INNER JOIN Orders o WITH (NOLOCK) ON o.ID = f.POID
LEFT JOIN PO_Supp_Detail psd WITH (NOLOCK) ON f.POID = psd.ID AND f.Seq1 = psd.SEQ1 AND f.Seq2 = psd.SEQ2
LEFT JOIN TransferToSubcon_Detail td WITH (NOLOCK)
    ON td.POID = f.POID
        AND td.Seq1 = f.Seq1
        AND td.Seq2 = f.Seq2
        AND td.Roll = f.Roll
        AND td.Dyelot = f.Dyelot
        AND td.StockType = f.StockType
INNER JOIN TransferToSubcon t WITH (NOLOCK) ON t.ID = td.ID AND t.Subcon = f.SubConStatus
OUTER APPLY (SELECT Qty = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty) s
OUTER APPLY (
    SELECT
        rd.ActualQty
    FROM Receiving_Detail rd WITH (NOLOCK)
    INNER JOIN Receiving r WITH (NOLOCK) ON rd.Id = r.id
    WHERE r.Type = 'A'
    AND f.POID = rd.PoId
    AND f.Seq1 = rd.Seq1
    AND f.Seq2 = rd.Seq2
    AND f.Roll = rd.Roll
    AND f.Dyelot = rd.Dyelot
) rdQty
OUTER APPLY (
    SELECT
        tid.Qty
    FROM TransferIn_Detail tid WITH (NOLOCK)
    WHERE f.POID = tid.PoId
    AND f.Seq1 = tid.Seq1
    AND f.Seq2 = tid.Seq2
    AND f.Roll = tid.Roll
    AND f.Dyelot = tid.Dyelot
) tidQty
OUTER APPLY (
    SELECT TOP 1--理應只有一筆, 但結構 Pkey串法並不是唯一
        fp.Grade
    FROM FIR WITH (NOLOCK)
    INNER JOIN FIR_Physical fp WITH (NOLOCK) ON fp.ID = FIR.ID
    WHERE f.POID = FIR.POID
    AND f.Seq1 = FIR.Seq1
    AND f.Seq2 = FIR.Seq2
    AND f.Roll = fp.Roll
    AND f.Dyelot = fp.Dyelot
) fir
WHERE NOT EXISTS (
    SELECT 1
    FROM SubconReturn t WITH (NOLOCK)
    INNER JOIN SubconReturn_Detail td WITH (NOLOCK) ON t.ID = td.ID
    WHERE f.POID = td.PoId
    AND f.Seq1 = td.Seq1
    AND f.Seq2 = td.Seq2
    AND f.Roll = td.Roll
    AND f.Dyelot = td.Dyelot
    AND f.StockType = td.StockType
    AND t.Subcon = '{strSubCon}'
    AND t.ID != '{strID}'
)
AND psd.FabricType = 'F'
AND f.SubConStatus = '{strSubCon}'
");

            if (!MyUtility.Check.Empty(this.txtTransfertoSubconID.Text))
            {
                strSQLCmd.Append($@" and t.ID = '{this.txtTransfertoSubconID.Text}'");
            }

            if (!MyUtility.Check.Empty(this.dateTransferOutDate.Value))
            {
                strSQLCmd.Append($@" and t.TransferOutDate = '{this.dateTransferOutDate.Text}'");
            }

            if (!this.txtSeq1.Seq1.Empty())
            {
                strSQLCmd.Append($@" and f.seq1 = '{this.txtSeq1.Seq1}'");
            }

            if (!this.txtSeq1.Seq2.Empty())
            {
                strSQLCmd.Append($@" and f.seq2 = '{this.txtSeq1.Seq2}'");
            }

            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                strSQLCmd.Append($@" and psd.Refno = '{this.txtRefno.Text}'");
            }

            switch (this.comboBoxStockType.Text)
            {
                case "":
                    strSQLCmd.Append($@" and td.StockType in ('B', 'I')");
                    break;
                case "Bulk":
                    strSQLCmd.Append($@" and td.StockType = 'B'");
                    break;
                case "Inventory":
                    strSQLCmd.Append($@" and td.StockType = 'I'");
                    break;
            }

            DualResult dualResult = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtArtwork);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = this.dtArtwork;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();

            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                          && row["poid"].EqualString(tmp["poid"].ToString())
                                                                          && row["seq1"].EqualString(tmp["seq1"])
                                                                          && row["seq2"].EqualString(tmp["seq2"])
                                                                          && row["roll"].EqualString(tmp["roll"])
                                                                          && row["dyelot"].EqualString(tmp["dyelot"])).ToArray();

                if (findrow.Length > 0)
                {
                    findrow[0]["seq"] = tmp["seq"];
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["StockUnit"] = tmp["StockUnit"];
                    findrow[0]["StockType"] = tmp["StockType"];
                    findrow[0]["Description"] = tmp["Description"];
                    findrow[0]["TransferToSubcon_DetailUkey"] = tmp["TransferToSubcon_DetailUkey"];
                    findrow[0]["RecvKG"] = tmp["RecvKG"];
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

        /// <inheritdoc/>
        public static DualResult CheckDetailStockTypeLocation(string stockType, string curLocation, out string newLocation)
        {
            newLocation = string.Empty;
            string sqlcmd = string.Format(
                @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", stockType);
            DataTable dt;
            DBProxy.Current.Select(null, sqlcmd, out dt);
            string[] getLocation = curLocation.Split(',').Distinct().ToArray();
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

            // 去除錯誤的Location將正確的Location填回
            trueLocation.Sort();
            newLocation = string.Join(",", trueLocation.ToArray());

            if (!selectId)
            {
                return new DualResult(false, "Can't found location " + string.Join(",", errLocation.ToArray()));
            }

            return new DualResult(true);
        }

        private void TxtLocation_MouseDown(object sender, MouseEventArgs e)
        {
            Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation(string.Empty, string.Empty);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            var select_result = item.GetSelecteds()
                .GroupBy(s => new { StockType = s["StockType"].ToString(), StockTypeCode = s["StockTypeCode"].ToString() })
                .Select(g => new { g.Key.StockType, g.Key.StockTypeCode, ToLocation = string.Join(",", g.Select(i => i["id"])) });

            if (select_result.Count() > 0)
            {
                this.selectedLocation.Clear();
                this.txtLocation.Text = string.Empty;
            }

            foreach (var result_item in select_result)
            {
                this.selectedLocation.Add(result_item.StockTypeCode, result_item.ToLocation);
                this.txtLocation.Text += $"({result_item.StockType}:{result_item.ToLocation})";
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
                if (this.selectedLocation.ContainsKey(item["stocktype"].ToString()))
                {
                    item["Location"] = this.selectedLocation[item["stocktype"].ToString()];
                }
            }
        }
    }
}
