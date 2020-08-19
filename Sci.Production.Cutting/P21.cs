using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P21 : Win.Tems.Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P21"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.UI.DataGridViewTextBoxColumn cbb_CutRef;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Seq1;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Seq2;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Roll;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Dyelot;
            Ict.Win.UI.DataGridViewNumericBoxColumn cbb_Yardage;

            DataGridViewGeneratorTextColumnSettings setCutRef = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings setSeq1 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings setSeq2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings setRoll = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings setDyelot = new DataGridViewGeneratorTextColumnSettings();
            this.gridIcon1.Enabled = true;

            #region 事件
            setCutRef.CellValidating += (s, e) =>
            {
                DataRow selectedRow = this.gridP21.GetDataRow(e.RowIndex);
                DataTable dt_CutRef;
                string oldValue = selectedRow["CutRef"].ToString();
                string newValue = e.FormattedValue.ToString();

                // 沒有異動
                if (oldValue.Equals(newValue) || e.RowIndex == -1)
                {
                    return;
                }

                // 輸入空or不存在 才清空這些欄位，因此if else不能跟前面放一起

                // 輸入空判斷
                if (MyUtility.Check.Empty(newValue))
                {
                    selectedRow["CutRef"] = string.Empty;
                    selectedRow["Roll"] = string.Empty;
                    selectedRow["Dyelot"] = string.Empty;
                    selectedRow["Yardage"] = DBNull.Value;
                    selectedRow["Factory"] = string.Empty;
                    selectedRow["EstCutCell"] = string.Empty;
                    selectedRow["CuttingSpNO"] = string.Empty;
                    selectedRow["EstCutDate"] = DBNull.Value;
                    selectedRow["ActCutDate"] = DBNull.Value;

                    selectedRow["Seq1"] = string.Empty;
                    selectedRow["Seq2"] = string.Empty;
                    selectedRow["Ukey"] = 0;
                    selectedRow["AddName"] = string.Empty;
                    selectedRow["AddDate"] = DBNull.Value;
                    selectedRow["EditName"] = string.Empty;
                    selectedRow["EditDate"] = DBNull.Value;
                    selectedRow.AcceptChanges();
                }
                else
                {
                    // 搜尋時若有多筆資料，直接選第一筆
                    string sqlCmd = $@"
                                    select TOP 1 w.ID ,CutCellid ,w.FactoryID ,[EstCutDate]=MAX(EstCutDate) ,[ActCutDate]=Max(c.cDate)
                                    from WorkOrder W 
                                    Left JOIN CuttingOutput_Detail CD on W.Ukey=CD.WorkOrderUkey
                                    Left JOIN CuttingOutput C on CD.ID=C.ID
                                    where w.CutRef = '{newValue}' AND w.MDivisionId='{Env.User.Keyword}'
                                    GROUP BY w.ID,CutCellid,w.FactoryID";
                    DualResult resultCheck = DBProxy.Current.Select(null, sqlCmd, out dt_CutRef);
                    if (!resultCheck)
                    {
                        this.ShowErr(resultCheck);
                        e.Cancel = true;
                        selectedRow["CutRef"] = string.Empty;
                        return;
                    }

                    // 是否存在判斷
                    if (dt_CutRef.Rows.Count == 0)
                    {
                        selectedRow["CutRef"] = string.Empty;
                        selectedRow["Roll"] = string.Empty;
                        selectedRow["Dyelot"] = string.Empty;
                        selectedRow["Yardage"] = DBNull.Value;
                        selectedRow["Factory"] = string.Empty;
                        selectedRow["EstCutCell"] = string.Empty;
                        selectedRow["CuttingSpNO"] = string.Empty;
                        selectedRow["EstCutDate"] = DBNull.Value;
                        selectedRow["ActCutDate"] = DBNull.Value;

                        selectedRow["Seq1"] = string.Empty;
                        selectedRow["Seq2"] = string.Empty;
                        selectedRow["Ukey"] = 0;
                        selectedRow["AddName"] = string.Empty;
                        selectedRow["AddDate"] = DBNull.Value;
                        selectedRow["EditName"] = string.Empty;
                        selectedRow["EditDate"] = DBNull.Value;

                        selectedRow.AcceptChanges();
                        MyUtility.Msg.WarningBox($"<CutRef#: {newValue}> not found!");
                        return;
                    }
                    else
                    {
                        DataRow tmp = dt_CutRef.Rows[0];
                        selectedRow["CutRef"] = newValue;
                        selectedRow["Factory"] = tmp["FactoryID"];
                        selectedRow["EstCutCell"] = tmp["CutCellid"];
                        selectedRow["CuttingSpNO"] = tmp["ID"];
                        selectedRow["EstCutDate"] = tmp["EstCutDate"];
                        selectedRow["ActCutDate"] = tmp["ActCutDate"];
                    }
                }

                selectedRow.AcceptChanges();
            };

            setSeq1.CellValidating += (s, e) =>
            {
                DataRow selectedRow = this.gridP21.GetDataRow(e.RowIndex);
                string oldValue = selectedRow["Seq1"].ToString();
                string newValue = e.FormattedValue.ToString();
                string cuttingSpNO = selectedRow["CuttingSpNO"].ToString();
                string cutRefNo = selectedRow["CutRef"].ToString();

                // 沒有異動
                if (oldValue.Equals(newValue) || e.RowIndex == -1)
                {
                    return;
                }

                // 若CutRef#為空則清空
                if (MyUtility.Check.Empty(cutRefNo))
                {
                    MyUtility.Msg.WarningBox("Please enter CutRef# first.");
                    selectedRow["Seq1"] = string.Empty;
                    selectedRow.AcceptChanges();
                    return;
                }

                // 異動or不存在：清空Roll#欄位、Dyelot、Yardage資料
                selectedRow["Roll"] = string.Empty;
                selectedRow["Dyelot"] = string.Empty;
                selectedRow["Yardage"] = DBNull.Value;
                selectedRow.AcceptChanges();

                // 輸入空，才清空這些欄位，因此if else不能跟前面放一起
                if (MyUtility.Check.Empty(newValue))
                {
                    selectedRow["Seq1"] = string.Empty;
                }
                else
                {
                    selectedRow["Seq1"] = newValue;
                }
            };

            setSeq2.CellValidating += (s, e) =>
            {
                DataRow selectedRow = this.gridP21.GetDataRow(e.RowIndex);
                string oldValue = selectedRow["Seq2"].ToString();
                string newValue = e.FormattedValue.ToString();
                string cuttingSpNO1 = selectedRow["CuttingSpNO"].ToString();
                string cutRefNo1 = selectedRow["CutRef"].ToString();

                // 沒有異動
                if (oldValue.Equals(newValue) || e.RowIndex == -1)
                {
                    return;
                }

                // 若CutRef#為空則清空
                if (MyUtility.Check.Empty(cutRefNo1))
                {
                    MyUtility.Msg.WarningBox("Please enter CutRef# first.");
                    selectedRow["Seq2"] = string.Empty;
                    selectedRow.AcceptChanges();
                    return;
                }

                // 異動or不存在：清空Roll#欄位、Dyelot、Yardage資料
                selectedRow["Roll"] = string.Empty;
                selectedRow["Dyelot"] = string.Empty;
                selectedRow["Yardage"] = DBNull.Value;
                selectedRow.AcceptChanges();

                // 輸入空，才清空這些欄位，因此if else不能跟前面放一起
                if (MyUtility.Check.Empty(newValue))
                {
                    selectedRow["Seq2"] = string.Empty;
                }
                else
                {
                    selectedRow["Seq2"] = newValue;
                }
            };

            setRoll.CellValidating += (s, e) =>
            {
                DataRow selectedRow = this.gridP21.GetDataRow(e.RowIndex);
                string oldValue = selectedRow["Roll"].ToString();
                string newValue = e.FormattedValue.ToString();
                string cuttingSpNO2 = selectedRow["CuttingSpNO"].ToString();
                string dyelot = selectedRow["Dyelot"].ToString();

                // 沒有異動
                if (oldValue.Equals(newValue) || e.RowIndex == -1 || MyUtility.Check.Empty(dyelot))
                {
                    return;
                }

                selectedRow["Roll"] = newValue;
            };

            setDyelot.CellValidating += (s, e) =>
            {
                DataRow selectedRow = this.gridP21.GetDataRow(e.RowIndex);
                string oldValue = selectedRow["Dyelot"].ToString();
                string newValue = e.FormattedValue.ToString();
                string cuttingSpNO3 = selectedRow["CuttingSpNO"].ToString();
                string roll = selectedRow["Roll"].ToString();

                // 沒有異動
                if (oldValue.Equals(newValue) || e.RowIndex == -1 || MyUtility.Check.Empty(roll))
                {
                    return;
                }

                selectedRow["Dyelot"] = newValue;
            };

            #endregion

            #region Grid設定
            this.Helper.Controls.Grid.Generator(this.gridP21)
                .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(7), settings: setCutRef).Get(out cbb_CutRef)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(7), settings: setSeq1).Get(out cbb_Seq1)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(7), settings: setSeq2).Get(out cbb_Seq2)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(7), settings: setRoll).Get(out cbb_Roll)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(7), settings: setDyelot).Get(out cbb_Dyelot)
                .Numeric("Yardage", header: "Yardage", decimal_places: 2, integer_places: 11, width: Widths.AnsiChars(7)).Get(out cbb_Yardage)
                .Text("Factory", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Text("EstCutCell", header: "Est.CutCell", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("CuttingSpNO", header: "Cutting SP#", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Date("EstCutDate", header: "Est.CutDate", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Date("ActCutDate", header: "Act.CutDate", iseditingreadonly: true, width: Widths.AnsiChars(15))
                ;
            this.gridP21.Columns["CutRef"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridP21.Columns["SEQ1"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridP21.Columns["SEQ2"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridP21.Columns["Roll"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridP21.Columns["Dyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridP21.Columns["Yardage"].DefaultCellStyle.BackColor = Color.Pink;

            // MaxLength 設定
            cbb_CutRef.MaxLength = 6;
            cbb_Seq1.MaxLength = 3;
            cbb_Seq2.MaxLength = 2;
            cbb_Roll.MaxLength = 8;
            cbb_Dyelot.MaxLength = 8;
            cbb_Yardage.Maximum = new decimal(999999999.99);

            this.gridP21.IsEditingReadOnly = false;
            this.gridP21.DataSource = this.listControlBindingSource1;

            #endregion

            foreach (DataGridViewColumn index in this.gridP21.Columns)
            {
                index.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            #region 透過SQL取得DB的結構，就不用寫死
            string cmd = @"
 SELECT  
        [CutRef]= cofr.CutRef,
        [Seq1]= cofr.Seq1,
        [Seq2]= cofr.Seq2,
        [Roll]= cofr.Roll,
        [Dyelot]= cofr.Dyelot,
        [Yardage]= cofr.Yardage,
        [Factory]= w.FactoryID ,
        [EstCutCell]= w.CutCellid,
        [CuttingSpNO]= w.ID,
        [EstCutDate]= w.EstCutDate,
        [ActCutDate]= c.cDate,
        [AddName]= addInfo.IdAndName,
        [AddDate]= cofr.AddDate,
        [EditName]=  editInfo.IdAndName,
        [EditDate]= cofr.EditDate,
        [Ukey]=cofr.Ukey,
        [MDivisionId]=''
FROM CuttingOutputFabricRecord cofr
INNER JOIN WorkOrder W on cofr.CutRef=W.CutRef
LEFT JOIN CuttingOutput_Detail CD on W.Ukey=CD.WorkOrderUkey
LEFT JOIN CuttingOutput C on CD.ID=C.ID
OUTER APPLY(
    SELECT IdAndName FROM GetName WHERE ID=cofr.AddName
)addInfo
OUTER APPLY(
    SELECT IdAndName FROM GetName WHERE ID=cofr.EditName
)editInfo
WHERE 1=0
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, cmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            #endregion
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            P21_QueryRevised form = new P21_QueryRevised();
            form.ShowDialog();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataRow[] allEmptyData;
            DataRow[] noEmptyData;
            DualResult returnResult;
            ITableSchema tableSchema;

            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(gridData) || gridData.Rows.Count == 0)
            {
                return;
            }

            // 直接清除CutRef#,SEQ,Roll#,Dyelot,Yardage欄位全部為空的資料
            DataRow[] tmp = gridData.Select();
            allEmptyData = tmp.AsEnumerable().Where(o => MyUtility.Check.Empty(o["CutRef"]) && MyUtility.Check.Empty(o["Seq1"]) && MyUtility.Check.Empty(o["Seq2"]) && MyUtility.Check.Empty(o["Roll"]) &&
                                                    MyUtility.Check.Empty(o["Dyelot"]) && MyUtility.Check.Empty(o["Yardage"])).ToArray();

            foreach (DataRow item in allEmptyData)
            {
                gridData.Rows.Remove(item);
            }

            // 判斷是否有任何一個欄位空
            tmp = gridData.Select();
            noEmptyData = tmp.AsEnumerable().Where(o => !MyUtility.Check.Empty(o["CutRef"]) && !MyUtility.Check.Empty(o["Seq1"]) && !MyUtility.Check.Empty(o["Seq2"]) && !MyUtility.Check.Empty(o["Roll"]) &&
                                                    !MyUtility.Check.Empty(o["Dyelot"]) && !MyUtility.Check.Empty(o["Yardage"])).ToArray();

            if (noEmptyData.Length == 0)
            {
                MyUtility.Msg.WarningBox("< CutRef#> ,<Seq> ,<Roll#> ,<Dyelot> ,<Yardage>  can not be empty or 0 !");
                return;
            }

            // 取tableSchema
            returnResult = DBProxy.Current.GetTableSchema(null, "CuttingOutputFabricRecord", out tableSchema);
            foreach (DataRow dr in noEmptyData)
            {
                dr["MDivisionId"] = Env.User.Keyword;
            }

            // 開始UPDATE
            this.ShowWaitMessage("Data Loading....");
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    returnResult = DBProxy.Current.Inserts(null, tableSchema, noEmptyData);
                    if (!returnResult)
                    {
                        throw new Exception(returnResult.Messages.ToString());
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();

                    // 將更新的資料從畫面上去掉
                    foreach (DataRow item in noEmptyData)
                    {
                        gridData.Rows.Remove(item);
                    }

                    gridData.AcceptChanges();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    returnResult = new DualResult(false, "Commit transaction error.", ex);
                }
                finally
                {
                    this.HideWaitMessage();
                }
            }

            if (!returnResult)
            {
                this.ShowErr(returnResult);
                return;
            }

            MyUtility.Msg.InfoBox("Successfully");
        }

        #region Grid Icon事件

        private void GridIcon1_AppendClick(object sender, EventArgs e)
        {
           DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
           DataRow nRow = gridData.NewRow();
           gridData.Rows.Add(nRow);

            // gridData.ImportRow(nRow);
        }

        private void GridIcon1_InsertClick(object sender, EventArgs e)
        {
            if (this.gridP21.RowCount != 0)
            {
                // 處理背後的DataTable
                DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
                DataRow nRow = gridData.NewRow();

                // 取得控制項反白的Row，的Index
                DataRow currentGridRow = this.gridP21.GetDataRow(this.gridP21.GetSelectedRowIndex());

                // 把被選取的Row，從控制項抓下來丟給背後的DataTable
                nRow = currentGridRow;
                gridData.ImportRow(nRow);
            }
        }

        private void GridIcon1_RemoveClick(object sender, EventArgs e)
        {
            if (this.gridP21.RowCount != 0)
            {
                // 處理背後的DataTable
                DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
                DataRow nRow = gridData.NewRow();

                // 取得控制項反白的Row，的Index
                DataRow currentGridRow = this.gridP21.GetDataRow(this.gridP21.GetSelectedRowIndex());

                // 把被選取的Row，從控制項抓下來丟給背後的DataTable刪除
                nRow = currentGridRow;
                gridData.Rows.Remove(nRow);
            }
        }
        #endregion

        /// <summary>
        /// 觸發事件：游標在Grid cell裡面按下任何按鍵
        /// </summary>
        private void GridP21_EditingKeyProcessing(object sender, Ict.Win.UI.DataGridViewEditingKeyProcessingEventArgs e)
        {
            bool isLastRow = this.gridP21.CurrentRow.Index == this.gridP21.Rows.Count - 1;
            bool isLastColumn = this.gridP21.CurrentCell.IsInEditMode;

            // 因為這兩個情境不完全相同，因此分開寫

            // 按下Enter，最後一Row、且是最後一個可編輯欄位（因為尚未找到動態找出「最後一個可編輯欄位」的方法，只好先寫死Yardage）
            if (e.KeyData == Keys.Enter && this.gridP21.CurrentCell.OwningColumn.Name == "Yardage" && isLastRow)
            {
                this.AddRowAndFocus();
            }

            // 在Yardage按下Tab，且是最後一Row
            if (e.KeyData == Keys.Tab && this.gridP21.CurrentCell.OwningColumn.Name == "Yardage" && isLastRow)
            {
                this.AddRowAndFocus();
            }
        }

        private void AddRowAndFocus()
        {
            // 新增一列
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            DataRow nRow = gridData.NewRow();
            gridData.Rows.Add(nRow);
            int nowIndex = this.gridP21.GetSelectedRowIndex();

            // 直接指定Cell，會selected到「那個Cell的右邊」，因此只好指定「那個Cell的左邊」，讓底層自動跳過去

            // 指定到前一Row的ActCutDate
            this.gridP21.Rows[nowIndex].Cells["ActCutDate"].Selected = true;
        }
    }
}
