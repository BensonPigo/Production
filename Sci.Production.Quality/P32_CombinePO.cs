using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P32_CombinePO : Win.Tems.QueryForm
    {
        private string CFAInspectionRecordID;

        private DataTable MasterCFAInspectionRecord_OrderSEQ;

        /// <inheritdoc/>
        public P32_CombinePO(bool canedit, string cFAInspectionRecordID, DataTable cFAInspectionRecord_OrderSEQ)
        {
            this.InitializeComponent();
            this.CFAInspectionRecordID = cFAInspectionRecordID;
            this.MasterCFAInspectionRecord_OrderSEQ = cFAInspectionRecord_OrderSEQ;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = false;

            DataGridViewGeneratorTextColumnSettings col_OrderID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Seq = new DataGridViewGeneratorTextColumnSettings();

            col_OrderID.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow selectedRow = this.grid.GetDataRow(e.RowIndex);
                    string orderID = e.FormattedValue.ToString();
                    string seq = MyUtility.Convert.GetString(selectedRow["Seq"]);

                    if (MyUtility.Check.Empty(orderID))
                    {
                        return;
                    }

                    List<SqlParameter> paras = new List<SqlParameter>
                    {
                        new SqlParameter("@ID", orderID),
                    };

                    #region OrderID檢查
                    bool exists = MyUtility.Check.Seek(
                        $@"
SELECT 1 
FROM Orders o
WHERE o.ID=@ID 
AND o.Finished = 0
AND o.Category IN('B', 'S', 'G')",
                        paras);
                    if (!exists)
                    {
                        MyUtility.Msg.InfoBox("Data not found!!");
                        e.Cancel = true;
                        return;
                    }

                    bool isSameM = MyUtility.Check.Seek($"SELECT 1 FROM Orders WHERE ID=@ID AND FtyGroup = '{Env.User.Factory}'", paras);

                    if (!isSameM)
                    {
                        MyUtility.Msg.InfoBox("Factory is different!!");
                        e.Cancel = true;
                        return;
                    }
                    #endregion

                    DualResult result;

                    paras.Add(new SqlParameter("@Seq", seq));

                    string cmd = string.Empty;

                    cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID
";

                    result = DBProxy.Current.Select(null, cmd, paras, out DataTable dt);

                    if (result)
                    {
                        if (dt.Rows.Count == 0)
                        {
                            // = 0
                            if (!MyUtility.Check.Empty(seq))
                            {
                                MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                                e.Cancel = true;
                            }
                        }
                        else if (dt.Rows.Count > 1)
                        {
                            // > 1
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "ID,Seq", "15,10", orderID, "ID,Seq")
                            {
                                Width = 600,
                            };
                            DialogResult dresult = item.ShowDialog();
                            if (dresult == DialogResult.OK)
                            {
                                IList<DataRow> selectedDatas = item.GetSelecteds();
                                seq = selectedDatas[0]["Seq"].ToString();
                                selectedRow["Seq"] = seq;
                                selectedRow["orderID"] = orderID;
                            }
                        }
                        else if (dt.Rows.Count == 1)
                        {
                            seq = dt.Rows[0]["Seq"].ToString();
                            selectedRow["Seq"] = seq;
                            selectedRow["orderID"] = orderID;
                        }
                    }
                    else
                    {
                        string msg = string.Empty;

                        foreach (var message in result.Messages)
                        {
                            msg += message + "\r\n";
                        }

                        MyUtility.Msg.WarningBox("DB Query Error : " + msg);
                    }

                    selectedRow.EndEdit();
                }
            };

            col_OrderID.CellMouseClick += (s, e) =>
            {
                // 第二筆資料才開始允許開窗
                if (e.RowIndex > 0 && this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenWindow(e.RowIndex);
                }
            };

            col_OrderID.EditingMouseDown += (s, e) =>
            {
                // 第二筆資料才開始允許開窗
                if (e.RowIndex > 0 && this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenWindow(e.RowIndex);
                }
            };

            col_Seq.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow selectedRow = this.grid.GetDataRow(e.RowIndex);
                    string orderID = MyUtility.Convert.GetString(selectedRow["OrderID"]);
                    string seq = e.FormattedValue.ToString();

                    // SP Seq都不為空才驗證
                    if (MyUtility.Check.Empty(orderID) || MyUtility.Check.Empty(seq))
                    {
                        return;
                    }

                    DualResult result;

                    string cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID AND Seq = @Seq
";

                    List<SqlParameter> paras = new List<SqlParameter>
                    {
                        new SqlParameter("@ID", orderID),
                        new SqlParameter("@Seq", seq),
                    };

                    result = DBProxy.Current.Select(null, cmd, paras, out DataTable dt);
                    if (dt.Rows.Count == 0)
                    {
                        // 驗證失敗清空
                        MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                        selectedRow["Seq"] = string.Empty;
                    }

                    selectedRow.EndEdit();
                }
            };

            col_Seq.CellMouseClick += (s, e) =>
            {
                // 第二筆資料才開始允許開窗
                if (e.RowIndex > 0 && this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenWindow(e.RowIndex);
                }
            };

            col_Seq.EditingMouseDown += (s, e) =>
            {
                // 第二筆資料才開始允許開窗
                if (e.RowIndex > 0 && this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenWindow(e.RowIndex);
                }
            };

            this.grid.DataSource = this.MasterCFAInspectionRecord_OrderSEQ.Copy();
            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: col_OrderID)
            .Text("Seq", header: "SEQ", width: Widths.AnsiChars(5), iseditingreadonly: false, settings: col_Seq)
            ;

            this.EditModeToggle();
        }

        private void OpenWindow(int rowIndex)
        {
            DataRow topRow = this.grid.GetDataRow(0);
            DataRow currentRow = this.grid.GetDataRow(rowIndex);
            string orderID = MyUtility.Convert.GetString(topRow["OrderID"]);
            DataTable dt;
            string cmd = $@"
SELECT [OrderID]=o.ID ,oq.Seq
FROM Orders o 
INNER JOIN Order_QtyShip oq ON o.ID =oq.ID
WHERE o.Finished = 0 AND
EXISTS
(
	SELECT 1
	FROM Orders
	WHERE ID='{orderID}'
	AND Ftygroup =o.Ftygroup AND SeasonID =o.SeasonID AND BrandID =o.BrandID  AND StyleID =o.StyleID
)
";
            DBProxy.Current.Select(null, cmd, out dt);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "OrderID,Seq", "15,10", orderID, "OrderID,Seq")
            {
                Width = 600,
            };
            DialogResult dresult = item.ShowDialog();
            if (dresult == DialogResult.OK)
            {
                IList<DataRow> selectedDatas = item.GetSelecteds();
                currentRow["OrderID"] = selectedDatas[0]["OrderID"].ToString();
                currentRow["Seq"] = selectedDatas[0]["Seq"].ToString();
            }

            topRow.EndEdit();
        }

        /// <summary>
        /// 取得資料來源
        /// </summary>
        /// <param name="cFAInspectionRecordID">cFAInspectionRecordID</param>
        /// <returns>Grid Data Source</returns>
        public DataTable Get_CFAInspectionRecord_OrderSEQ(string cFAInspectionRecordID)
        {
            string cmd = $@" SELECT * FROM CFAInspectionRecord_OrderSEQ WHERE ID = '{cFAInspectionRecordID}'";
            DataTable dt;
            DualResult r = DBProxy.Current.Select(null, cmd, out dt);

            if (!r)
            {
                this.ShowErr(r);
                return null;
            }

            return dt;
        }

        private void BtnEditSave_Click(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                this.EditModeToggle();
            }
            else
            {
                this.EditModeToggle();
                this.SaveClick();
                this.grid.DataSource = null;
                this.grid.DataSource = this.MasterCFAInspectionRecord_OrderSEQ.Copy();
                this.Close();
            }
        }

        private void BtnCloseUndo_Click(object sender, EventArgs e)
        {
            switch (this.btnCloseUndo.Text)
            {
                case "Close":
                    this.Close();
                    break;
                case "Undo":
                    // this.EditModeToggle();
                    // this.UndoClick();
                    this.Close();
                    break;
                default:
                    break;
            }
        }

        private void SaveClick()
        {
            try
            {
                DataTable gridDt = (DataTable)this.grid.DataSource;

                this.MasterCFAInspectionRecord_OrderSEQ.Clear();
                foreach (DataRow dr in gridDt.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(o["OrderID"]) && !MyUtility.Check.Empty(o["Seq"])))
                {
                    string currentOrderID = MyUtility.Convert.GetString(dr["OrderID"]);
                    string currentSeq = MyUtility.Convert.GetString(dr["Seq"]);

                    // 不存在則加入
                    if (!this.MasterCFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted &&
                            MyUtility.Convert.GetString(o["OrderID"]) == currentOrderID &&
                            MyUtility.Convert.GetString(o["Seq"]) == currentSeq).Any())
                    {
                        this.MasterCFAInspectionRecord_OrderSEQ.ImportRow(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void UndoClick()
        {
            this.grid.DataSource = null;
            this.grid.DataSource = this.MasterCFAInspectionRecord_OrderSEQ.Copy();
        }

        /// <summary>
        /// 切換EditMode，以及按鈕文字、Visiable切換
        /// </summary>
        private void EditModeToggle()
        {
            if (this.EditMode)
            {
                this.EditMode = false;
                this.btnAppend.Visible = false;
                this.btnDelete.Visible = false;
                this.btnEditSave.Text = "Edit";
                this.btnCloseUndo.Text = "Close";
            }
            else
            {
                this.EditMode = true;
                this.btnAppend.Visible = true;
                this.btnDelete.Visible = true;
                this.btnEditSave.Text = "Save";
                this.btnCloseUndo.Text = "Undo";
            }
        }

        private void BtnAppend_Click(object sender, EventArgs e)
        {
            DataTable gridDt = (DataTable)this.grid.DataSource;

            DataRow nRow = gridDt.NewRow();
            nRow["ID"] = this.MasterCFAInspectionRecord_OrderSEQ;
            gridDt.Rows.Add(nRow);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.grid.Rows.Count > 0)
            {
                int currentRowIndex = this.grid.CurrentRow.Index;
                this.grid.Rows.RemoveAt(currentRowIndex);
            }
        }
    }
}
