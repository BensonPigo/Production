using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P21_QueryRevised : Sci.Win.Forms.Base
    {
        public P21_QueryRevised()
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Seq1;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Seq2;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Roll;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Dyelot;
            Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

            DataGridViewGeneratorTextColumnSettings setSeq1 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings setSeq2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings setRoll = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings setDyelot = new DataGridViewGeneratorTextColumnSettings();

            this.gridP21Query.IsEditingReadOnly = false;
            this.gridP21Query.DataSource = this.listControlBindingSource1;

            #region 事件
            setSeq1.CellValidating += (s, e) =>
            {
                DataRow selectedRow = this.gridP21Query.GetDataRow(e.RowIndex);
                string oldValue = selectedRow["Seq1"].ToString();
                string newValue = e.FormattedValue.ToString();
                string CuttingSpNO = selectedRow["CuttingSpNO"].ToString();

                // 沒有異動
                if (oldValue.Equals(newValue) || e.RowIndex == -1)
                {
                    return;
                }

                // 輸入空，才清空這些欄位，因此if else不能跟前面放一起
                if (MyUtility.Check.Empty(newValue))
                {
                    selectedRow["Seq1"] = string.Empty;
                    selectedRow["Roll"] = string.Empty;
                    selectedRow["Dyelot"] = string.Empty;
                    selectedRow["Yardage"] = DBNull.Value;
                }
                else
                {
                    // 異動or不存在：清空Roll#欄位、Dyelot、Yardage資料
                    selectedRow["Roll"] = string.Empty;
                    selectedRow["Dyelot"] = string.Empty;
                    selectedRow["Yardage"] = DBNull.Value;
                    selectedRow["Seq1"] = newValue;
                }
            };

            setSeq2.CellValidating += (s, e) =>
            {
                DataRow selectedRow = this.gridP21Query.GetDataRow(e.RowIndex);
                string oldValue = selectedRow["Seq2"].ToString();
                string newValue = e.FormattedValue.ToString();
                string CuttingSpNO = selectedRow["CuttingSpNO"].ToString();

                // 沒有異動
                if (oldValue.Equals(newValue) || e.RowIndex == -1)
                {
                    return;
                }

                // 輸入空，才清空這些欄位，因此if else不能跟前面放一起
                if (MyUtility.Check.Empty(newValue))
                {
                    selectedRow["Seq2"] = string.Empty;
                    selectedRow["Roll"] = string.Empty;
                    selectedRow["Dyelot"] = string.Empty;
                    selectedRow["Yardage"] = DBNull.Value;
                }
                else
                {
                    // 異動or不存在：清空Roll#欄位、Dyelot、Yardage資料
                    selectedRow["Roll"] = string.Empty;
                    selectedRow["Dyelot"] = string.Empty;
                    selectedRow["Yardage"] = DBNull.Value;
                    selectedRow["Seq2"] = newValue;
                }
            };

            setRoll.CellValidating += (s, e) =>
            {
                DataRow selectedRow = this.gridP21Query.GetDataRow(e.RowIndex);
                string oldValue = selectedRow["Roll"].ToString();
                string newValue = e.FormattedValue.ToString();
                string CuttingSpNO = selectedRow["CuttingSpNO"].ToString();
                string dyelot = selectedRow["Dyelot"].ToString();

                // 沒有異動，修改Roll時，Dyelot不為空才驗證
                if (oldValue.Equals(newValue) || e.RowIndex == -1 || MyUtility.Check.Empty(dyelot))
                {
                    return;
                }

                // 不允許空白，用舊的填回去
                if (MyUtility.Check.Empty(newValue))
                {
                    selectedRow["Roll"] = oldValue;
                    return;
                }

                selectedRow["Roll"] = newValue;
            };

            setDyelot.CellValidating += (s, e) =>
            {
                DataRow selectedRow = this.gridP21Query.GetDataRow(e.RowIndex);
                string oldValue = selectedRow["Dyelot"].ToString();
                string newValue = e.FormattedValue.ToString();
                string CuttingSpNO = selectedRow["CuttingSpNO"].ToString();
                string Roll = selectedRow["Roll"].ToString();

                // 沒有異動
                if (oldValue.Equals(newValue) || e.RowIndex == -1 || MyUtility.Check.Empty(Roll))
                {
                    return;
                }

                // 不允許空白，用舊的填回去
                if (MyUtility.Check.Empty(newValue))
                {
                    selectedRow["Dyelot"] = oldValue;
                    return;
                }

                selectedRow["Dyelot"] = newValue;
            };
            #endregion

            #region Grid 設定
            this.Helper.Controls.Grid.Generator(this.gridP21Query)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("Factory", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Text("EstCutCell", header: "Est.CutCell", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("CuttingSpNO", header: "Cutting SP#", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("CutRef", header: "CutRef#", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(4), settings: setSeq1).Get(out cbb_Seq1)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(3), settings: setSeq2).Get(out cbb_Seq2)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(7), settings: setRoll).Get(out cbb_Roll)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(7), settings: setDyelot).Get(out cbb_Dyelot)
                .Numeric("Yardage", header: "Yardage", decimal_places: 2, integer_places: 11, width: Widths.AnsiChars(7))
                .Date("EstCutDate", header: "Est.CutDate", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Date("ActCutDate", header: "Act.CutDate", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("AddName", header: "AddName", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .DateTime("AddDate", header: "AddDate", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("EditName", header: "EditName", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .DateTime("EditDate", header: "EditDate", iseditingreadonly: true, width: Widths.AnsiChars(15))
                ;

            // MaxLength 設定
            cbb_Seq1.MaxLength = 3;
            cbb_Seq2.MaxLength = 2;
            cbb_Roll.MaxLength = 8;
            cbb_Dyelot.MaxLength = 8;

            // 底色設定
            this.gridP21Query.Columns["SEQ1"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridP21Query.Columns["SEQ2"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridP21Query.Columns["Roll"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridP21Query.Columns["Dyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridP21Query.Columns["Yardage"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            for (int i = 0; i < this.gridP21Query.Columns.Count; i++)
            {
                this.gridP21Query.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            #region 變數宣告

            this.gridP21Query.ValidateControl();
            DataRow[] errorArr;
            DialogResult dResult;
            DualResult returnResult;
            ITableSchema tableSchema;
            DataRow[] selectedRow;
            DataTable currentTable;
            StringBuilder updateCmd = new StringBuilder();
            #endregion

            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;

            #region 檢查空、詢問視窗
            if (MyUtility.Check.Empty(gridData) || gridData.Rows.Count == 0)
            {
                return;
            }

            selectedRow = gridData.Select("Selected = 1");
            if (selectedRow.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dResult = MyUtility.Msg.QuestionBox("Do you want to update it?");
            if (dResult == DialogResult.No)
            {
                return;
            }

            currentTable = selectedRow.CopyToDataTable();

            errorArr = selectedRow.AsEnumerable().Where(o =>
                                                           MyUtility.Check.Empty(o["Seq1"]) ||
                                                           MyUtility.Check.Empty(o["Seq2"]) ||
                                                           MyUtility.Check.Empty(o["Roll"]) ||
                                                           MyUtility.Check.Empty(o["Dyelot"]) || MyUtility.Check.Empty(o["Yardage"])).ToArray();

            if (errorArr.Length > 0)
            {
                MyUtility.Msg.WarningBox("<Seq1>,<Seq2>, <Roll#>, <Dyelot>, <Yardage> can not be empty or 0 ! ");
                return;
            }
            #endregion

            // 取tableSchema
            returnResult = DBProxy.Current.GetTableSchema(null, "CuttingOutputFabricRecord", out tableSchema);

            // 開始UPDATE
            using (TransactionScope _transactionscope = new TransactionScope())
            {
                try
                {
                    foreach (DataRow item in selectedRow)
                    {
                        bool different;
                        returnResult = DBProxy.Current.UpdateByChanged(null, tableSchema, item, out different);
                        if (!returnResult)
                        {
                            _transactionscope.Dispose();
                            return;
                        }
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");

                    // 將更新的資料從畫面上去掉
                    foreach (DataRow item in selectedRow)
                    {
                        gridData.Rows.Remove(item);
                    }
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            #region 參數宣告
            DataRow[] selectedRow;
            ITableSchema tableSchema;
            DialogResult dResult;
            DualResult returnResult;
            DataTable currentTable;
            #endregion

            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;

            #region 檢查勾選、確認對話框
            if (MyUtility.Check.Empty(gridData) || gridData.Rows.Count == 0)
            {
                return;
            }

            selectedRow = gridData.Select("Selected = 1");
            if (selectedRow.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            currentTable = selectedRow.CopyToDataTable();

            dResult = MyUtility.Msg.QuestionBox("Do you want to delete it?");
            if (dResult == DialogResult.No)
            {
                return;
            }

            #endregion

            // 取tableSchema
            returnResult = DBProxy.Current.GetTableSchema(null, "CuttingOutputFabricRecord", out tableSchema);

            // 開始Delete
            using (TransactionScope _transactionscope = new TransactionScope())
            {
                try
                {
                    returnResult = DBProxy.Current.Deletes(null, tableSchema, selectedRow);
                    if (!returnResult)
                    {
                        _transactionscope.Dispose();
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");

                    this.Query();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
        }

        //---自行定義---
        private void Query()
        {
            #region 參數宣告
            DataTable dt;
            DualResult result;
            DateTime? cutOutput_s = this.dateCutOutput.Value1 ?? null;
            DateTime? cutOutput_e = this.dateCutOutput.Value2 ?? null;
            string cutRefNo_s = this.txtCutRefNo_s.Text;
            string cutRefNo_e = this.txtCutRefNo_e.Text;
            string factory = this.txtfactory.Text;
            string cutCell_s = this.txtCutCell_s.Text;
            string cutCell_e = this.txtCutCell_e.Text;
            string spNo = this.txtSpNo.Text;
            string outerApplyWhere = string.Empty;
            #endregion

            #region 組SQL

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"
                            SELECT  
                                    [Selected]=0,
                                    [Factory]= WorkOrder.FactoryID ,
                                    [EstCutCell]= WorkOrder.CutCellid,
                                    [CuttingSpNO]= WorkOrder.ID,
                                    [CutRef]= cofr.CutRef,
                                    [SEQ1]= cofr.SEQ1 ,
                                    [SEQ2]= cofr.SEQ2 ,
                                    [Roll]= cofr.Roll,
                                    [Dyelot]= cofr.Dyelot,
                                    [Yardage]= cofr.Yardage,
                                    [EstCutDate]= MAX(w.EstCutDate),
                                    [ActCutDate]= MAX(c.cDate),
                                    [AddName]= addInfo.IdAndName,
                                    [AddDate]= cofr.AddDate,
                                    [EditName]=  editInfo.IdAndName,
                                    [EditDate]= cofr.EditDate,
                                    [Ukey]=cofr.Ukey
                            FROM CuttingOutputFabricRecord cofr
                            INNER JOIN WorkOrder W on cofr.CutRef=W.CutRef and cofr.MDivisionId = w.MDivisionId
                            LEFT JOIN CuttingOutput_Detail CD on W.Ukey=CD.WorkOrderUkey
                            LEFT JOIN CuttingOutput C on CD.ID=C.ID
                            OUTER APPLY(
                                SELECT IdAndName FROM GetName WHERE ID=cofr.AddName
                            )addInfo
                            OUTER APPLY(
                                SELECT IdAndName FROM GetName WHERE ID=cofr.EditName
                            )editInfo
							OUTER APPLY(
								SELECT TOP 1 ID,FactoryID,CutCellid FROM WorkOrder WHERE CutRef=cofr.CutRef AND MDivisionId = cofr.MDivisionId
                                                                                         {outerApplyWhere}
							)WorkOrder
                            WHERE cofr.MDivisionId='{Sci.Env.User.Keyword}'
                            ");
            #endregion

            #region Where 條件

            if (!MyUtility.Check.Empty(cutOutput_s))
            {
                sqlCmd.Append(Environment.NewLine + $" And C.cDate >= '{cutOutput_s.Value.ToShortDateString()}'");
            }

            if (!MyUtility.Check.Empty(cutOutput_e))
            {
                sqlCmd.Append(Environment.NewLine + $" And C.cDate <= '{cutOutput_e.Value.ToShortDateString()}'");
            }

            if (!MyUtility.Check.Empty(cutCell_s))
            {
                sqlCmd.Append(Environment.NewLine + $" And w.CutCellid >= '{cutCell_s}'");
            }

            if (!MyUtility.Check.Empty(cutCell_e))
            {
                sqlCmd.Append(Environment.NewLine + $" And w.CutCellid <= '{cutCell_e}'");
            }

            if (!MyUtility.Check.Empty(cutRefNo_s))
            {
                sqlCmd.Append(Environment.NewLine + $" And cofr.CutRef >= '{cutRefNo_s}'");
            }

            if (!MyUtility.Check.Empty(cutRefNo_e))
            {
                sqlCmd.Append(Environment.NewLine + $" And cofr.CutRef <= '{cutRefNo_e}'");
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(Environment.NewLine + $" And W.FactoryID = '{factory}'");
            }

            if (!MyUtility.Check.Empty(spNo))
            {
                sqlCmd.Append(Environment.NewLine + $" And W.ID = '{spNo}'");
            }
            #endregion

            sqlCmd.Append(Environment.NewLine + @" GROUP BY WorkOrder.FactoryID ,WorkOrder.CutCellid,WorkOrder.ID,cofr.CutRef,cofr.SEQ1, cofr.SEQ2 
                                                   , cofr.Roll,cofr.Dyelot,cofr.Yardage,addInfo.IdAndName, cofr.AddDate
                                                   , editInfo.IdAndName, cofr.EditDate ,cofr.Ukey");
            this.ShowWaitMessage("Data Loading....");

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out dt);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.HideWaitMessage();

            this.listControlBindingSource1.DataSource = dt;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
            }
        }
    }
}
