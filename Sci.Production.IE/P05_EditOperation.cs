using Ict;
using Ict.Win;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Sci.Production.IE
{
    /// <summary>
    /// P05_EditOperation
    /// </summary>
    public partial class P05_EditOperation : Sci.Win.Tems.QueryForm
    {
        private DataTable dtAutomatedLineMapping_DetailCopy;

        /// <summary>
        /// dtAutomatedLineMapping_Detail
        /// </summary>
        public DataTable dtAutomatedLineMapping_Detail;
        private DataTable dtSelectItemSource;
        private DataTable dtNoSelectItem = new DataTable();

        /// <summary>
        /// P05_EditOperation
        /// </summary>
        /// <param name="dtAutomatedLineMapping_Detail">dtAutomatedLineMapping_Detail</param>
        public P05_EditOperation(DataTable dtAutomatedLineMapping_Detail)
        {
            this.DialogResult = DialogResult.No;
            var sortDataView = dtAutomatedLineMapping_Detail.AsDataView();
            sortDataView.Sort = "No ASC";
            this.dtAutomatedLineMapping_DetailCopy = sortDataView.ToTableKeepRowState();

            if (!this.dtAutomatedLineMapping_DetailCopy.Columns.Contains("UpdSewerDiffPercentage"))
            {
                this.dtAutomatedLineMapping_DetailCopy.Columns.Add("UpdSewerDiffPercentage", typeof(int));
            }

            if (!this.dtAutomatedLineMapping_DetailCopy.Columns.Contains("UpdDivSewer"))
            {
                this.dtAutomatedLineMapping_DetailCopy.Columns.Add("UpdDivSewer", typeof(decimal));
            }

            this.InitializeComponent();
            this.EditMode = true;

            this.gridIconEditOperation.InsertClick += this.GridIconEditOperation_InsertClick;
            this.gridEditOperation.CellFormatting += this.GridEditOperation_CellFormatting;

            this.dtNoSelectItem.Columns.Add(new DataColumn("No", typeof(string)));
            this.dtNoSelectItem = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable()
                .Where(s => MyUtility.Convert.GetBool(s["Selected"]))
                .Select(s => s["No"].ToString())
                .Distinct()
                .Select(s =>
                {
                    DataRow dr = this.dtNoSelectItem.NewRow();
                    dr["No"] = s;
                    return dr;
                }).CopyToDataTable();

            var checkedNo = this.dtNoSelectItem.AsEnumerable().Select(s => s["No"].ToString()).ToList();
            this.dtSelectItemSource = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable()
                .Where(s => checkedNo.Contains(s["No"].ToString()))
                .GroupBy(s => s["TimeStudyDetailUkey"])
                .Select(s => s.First())
                .TryCopyToDataTable(this.dtAutomatedLineMapping_DetailCopy);

            this.gridEditOperation.DataSource = this.gridEditOperationBs;
            this.gridEditOperationBs.DataSource = this.dtAutomatedLineMapping_DetailCopy;

            this.gridEditOperationBs.Filter = $" No in ({checkedNo.Select(s => $"'{s}'").JoinToString(",")}) or Selected = 1";

            this.gridIconEditOperation.Append.Visible = false;
            this.gridIconEditOperation.Remove.Visible = false;

            new GridRowDrag(this.gridEditOperation, this.AfterRowDragDo, excludeDragCols: new List<string>() { "UpdSewerDiffPercentage", "UpdDivSewer" });
        }

        private void GridEditOperation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataRow drTarget = this.gridEditOperation.GetDataRow(e.RowIndex);
            string colDataPropertyName = this.gridEditOperation.Columns[e.ColumnIndex].DataPropertyName;

            if (colDataPropertyName == "UpdSewerDiffPercentage" || colDataPropertyName == "UpdDivSewer")
            {
                return;
            }

            if (drTarget.RowState == DataRowState.Added &&
                (colDataPropertyName == "No" ||
                colDataPropertyName == "Location" ||
                colDataPropertyName == "MachineTypeID" ||
                colDataPropertyName == "MasterPlusGroup" ||
                colDataPropertyName == "OperationDesc"))
            {
                this.gridEditOperation.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Pink;
            }
            else if (colDataPropertyName == "No" && MyUtility.Check.Empty(this.gridEditOperation.Rows[e.RowIndex].Cells["No"].Value))
            {
                this.gridEditOperation.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Pink;
            }
            else
            {
                this.gridEditOperation.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = this.gridEditOperation.DefaultCellStyle.BackColor;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorTextColumnSettings colNo = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings colGroupOperation = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings colUpdSewer = new DataGridViewGeneratorNumericColumnSettings();

            colNo.CellMouseClick += (s, e) =>
            {
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.gridEditOperation.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.Pink)
                {
                    return;
                }

                DataRow curRow = this.gridEditOperation.GetDataRow(e.RowIndex);

                Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.dtNoSelectItem, "No", null, null, false, null)
                {
                    Width = 250,
                };

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                curRow["No"] = item.GetSelectedString();
                curRow.EndEdit();
            };

            colNo.EditingMouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.gridEditOperation.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.Pink)
                {
                    return;
                }

                DataRow curRow = this.gridEditOperation.GetDataRow(e.RowIndex);

                Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.dtNoSelectItem, "No", null, null, false, null)
                {
                    Width = 250,
                };

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                curRow["No"] = item.GetSelectedString();
                curRow.EndEdit();
            };

            colGroupOperation.EditingMouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.gridEditOperation.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.Pink)
                {
                    return;
                }

                DataRow curRow = this.gridEditOperation.GetDataRow(e.RowIndex);

                Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.dtSelectItemSource, "Location,MachineTypeID,MasterPlusGroup,OperationDesc,OriSewer", null, null, false, null, "Location,ST/MC,MC Group,Operation,Original Sewer", columndecimals: ",,,,4")
                {
                    Width = 780,
                };

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                DataRow selectedResult = item.GetSelecteds()[0];
                curRow["ID"] = selectedResult["ID"];
                curRow["Location"] = selectedResult["Location"];
                curRow["PPA"] = selectedResult["PPA"];
                curRow["MachineTypeID"] = selectedResult["MachineTypeID"];
                curRow["MasterPlusGroup"] = selectedResult["MasterPlusGroup"];
                curRow["OperationID"] = selectedResult["OperationID"];
                curRow["Annotation"] = selectedResult["Annotation"];
                curRow["Attachment"] = selectedResult["Attachment"];
                curRow["SewingMachineAttachmentID"] = selectedResult["SewingMachineAttachmentID"];
                curRow["Template"] = selectedResult["Template"];
                curRow["GSD"] = selectedResult["GSD"];
                curRow["OriSewer"] = selectedResult["OriSewer"];
                curRow["TimeStudyDetailUkey"] = selectedResult["TimeStudyDetailUkey"];
                curRow["ThreadComboID"] = selectedResult["ThreadComboID"];
                curRow["IsNonSewingLine"] = selectedResult["IsNonSewingLine"];
                curRow["PPADesc"] = selectedResult["PPADesc"];
                curRow["OperationDesc"] = selectedResult["OperationDesc"];
                curRow.EndEdit();
            };

            colUpdSewer.CellValidating += (sender, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.gridEditOperation.GetDataRow<DataRow>(e.RowIndex);

                dr[this.gridEditOperation.Columns[e.ColumnIndex].DataPropertyName] = e.FormattedValue;

                this.CaculateUpdSewerColumn(this.gridEditOperation.Columns[e.ColumnIndex].DataPropertyName, dr);
            };

            this.Helper.Controls.Grid.Generator(this.gridEditOperation)
               .Text("No", header: "No", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: colNo)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: colGroupOperation)
               .Text("MachineTypeID", header: "ST/MC", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: colGroupOperation)
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: colGroupOperation)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: colGroupOperation)
               .Numeric("OriSewer", header: "Original" + Environment.NewLine + "Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "Ori. %", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Original" + Environment.NewLine + "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("UpdSewerDiffPercentage", header: "Update %", width: Widths.AnsiChars(5), iseditingreadonly: false, settings: colUpdSewer)
               .Numeric("UpdDivSewer", header: "Update" + Environment.NewLine + "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: false, settings: colUpdSewer);

            this.gridEditOperation.Columns["UpdSewerDiffPercentage"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridEditOperation.Columns["UpdSewerDiffPercentage"].DefaultCellStyle.ForeColor = Color.Red;
            this.gridEditOperation.Columns["UpdDivSewer"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridEditOperation.Columns["UpdDivSewer"].DefaultCellStyle.ForeColor = Color.Red;
        }

        private void CaculateUpdSewerColumn(string colName, DataRow caculateRow)
        {
            if (colName == "UpdSewerDiffPercentage")
            {
                caculateRow["UpdDivSewer"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(caculateRow["OriSewer"]) * MyUtility.Convert.GetDecimal(caculateRow["UpdSewerDiffPercentage"]) / 100, 4);
            }
            else
            {
                caculateRow["UpdSewerDiffPercentage"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(caculateRow["UpdDivSewer"]) / MyUtility.Convert.GetDecimal(caculateRow["OriSewer"]) * 100, 0);
            }

            // 如果有同TimeStudyDetailUkey剩餘一筆未維護，自動計算填入
            var sameTimeStudyDetailUkeyData = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable()
                          .Where(s => MyUtility.Convert.GetLong(s["TimeStudyDetailUkey"]) == MyUtility.Convert.GetLong(caculateRow["TimeStudyDetailUkey"]));

            var stillNotUpdDivSewer = sameTimeStudyDetailUkeyData.Where(s => MyUtility.Check.Empty(s["UpdDivSewer"]));
            if (stillNotUpdDivSewer.Count() == 1)
            {
                DataRow targetFixRow = stillNotUpdDivSewer.First();
                decimal accuUpdDivSewer = sameTimeStudyDetailUkeyData.Sum(s => MyUtility.Convert.GetDecimal(s["UpdDivSewer"]));
                decimal accuUpdDivSewerPercentage = sameTimeStudyDetailUkeyData.Sum(s => MyUtility.Convert.GetDecimal(s["UpdSewerDiffPercentage"]));
                decimal oriSewer = MyUtility.Convert.GetDecimal(targetFixRow["OriSewer"]);
                targetFixRow["UpdDivSewer"] = oriSewer < accuUpdDivSewer ? 0 : oriSewer - accuUpdDivSewer;
                targetFixRow["UpdSewerDiffPercentage"] = accuUpdDivSewerPercentage >= 100 ? 0 : 100 - accuUpdDivSewerPercentage;
            }
        }

        private void GridIconEditOperation_InsertClick(object sender, EventArgs e)
        {
            if (this.gridEditOperation.SelectedRows.Count == 0)
            {
                return;
            }

            DataRow selectedRow = this.gridEditOperation.GetDataRow(this.gridEditOperation.SelectedRows[0].Index);
            DataRow newRow = this.dtAutomatedLineMapping_DetailCopy.NewRow();
            newRow["Selected"] = true;

            int insertIndex = this.dtAutomatedLineMapping_DetailCopy.Rows.IndexOf(selectedRow);
            this.dtAutomatedLineMapping_DetailCopy.Rows.InsertAt(newRow, insertIndex);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AfterRowDragDo(DataRow dragRow)
        {
            if (dragRow == null)
            {
                return;
            }

            foreach (DataRowView rowView in this.gridEditOperationBs)
            {
                if (ReferenceEquals(rowView.Row, dragRow))
                {
                    int curIndex = this.gridEditOperationBs.IndexOf(rowView);

                    if (curIndex == 0)
                    {
                        dragRow["No"] = ((DataRowView)this.gridEditOperationBs[1]).Row["No"];
                    }
                    else if (curIndex == this.gridEditOperationBs.Count - 1)
                    {
                        dragRow["No"] = ((DataRowView)this.gridEditOperationBs[curIndex - 1]).Row["No"];
                    }
                    else
                    {
                        string nextNo = ((DataRowView)this.gridEditOperationBs[curIndex + 1]).Row["No"].ToString();
                        string preNo = ((DataRowView)this.gridEditOperationBs[curIndex - 1]).Row["No"].ToString();
                        dragRow["No"] = nextNo == preNo ? nextNo : string.Empty;
                        dragRow["Selected"] = true;
                    }

                    break;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridEditOperationBs.EndEdit();
            var checkedNo = this.dtNoSelectItem.AsEnumerable().Select(s => s["No"].ToString()).ToList();
            var curEditRows = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable().Where(s => checkedNo.Contains(s["No"].ToString()));
            #region 檢查DivSewer與UpdSewerDiffPercentage總和是否超過或不足
            var checkDivSewerBalance = curEditRows
                .GroupBy(s => s["TimeStudyDetailUkey"])
                .Select(groupItem => new
                {
                    OperationDesc = groupItem.First()["OperationDesc"].ToString(),
                    OriSewer = MyUtility.Convert.GetDecimal(groupItem.First()["OriSewer"]),
                    SumDivSewer = groupItem.Sum(s => MyUtility.Check.Empty(s["UpdDivSewer"]) ? MyUtility.Convert.GetDecimal(s["DivSewer"]) : MyUtility.Convert.GetDecimal(s["UpdDivSewer"])),
                    SumSewerDiffPercentage = groupItem.Sum(s => MyUtility.Check.Empty(s["UpdSewerDiffPercentage"]) ? MyUtility.Convert.GetDecimal(s["SewerDiffPercentageDesc"]) : MyUtility.Convert.GetDecimal(s["UpdSewerDiffPercentage"])),
                });

            foreach (var checkItem in checkDivSewerBalance)
            {
                if (checkItem.SumSewerDiffPercentage != 100)
                {
                    MyUtility.Msg.WarningBox($"The sum of [Update %] of Operation [{checkItem.OperationDesc}] needs to be equal to 100.");
                    return;
                }

                if (checkItem.OriSewer != checkItem.SumDivSewer)
                {
                    MyUtility.Msg.WarningBox($"The sum of [Update Div. Sewer] of Operation [{checkItem.OperationDesc}] needs to be equal to [Original Sewer].");
                    return;
                }
            }
            #endregion

            #region 檢查是否有TimeStudyDetailUkey遺漏
            List<long> curTimeStudyDetailUkey = curEditRows.Select(s => MyUtility.Convert.GetLong(s["TimeStudyDetailUkey"])).Distinct().ToList();
            var listLoseTimeStudyDetailUkey = this.dtSelectItemSource.AsEnumerable().Where(s => !curTimeStudyDetailUkey.Contains(MyUtility.Convert.GetLong(s["TimeStudyDetailUkey"]))).ToList();
            if (listLoseTimeStudyDetailUkey.Any())
            {
                MyUtility.Msg.WarningBox($@"The following Operation is missing.
{listLoseTimeStudyDetailUkey.Select(s => s["OperationDesc"].ToString()).JoinToString(Environment.NewLine)}");
                return;
            }
            #endregion

            #region 檢查No是否完整
            List<string> curNo = curEditRows.Select(s => s["No"].ToString()).Distinct().ToList();
            var listLoseNo = this.dtNoSelectItem.AsEnumerable().Where(s => !curNo.Contains(s["No"].ToString())).ToList();
            if (listLoseNo.Any())
            {
                MyUtility.Msg.WarningBox($@"The following No is missing.
{listLoseNo.Select(s => s["No"].ToString()).JoinToString(",")}");
                return;
            }
            #endregion

            foreach (var needUpdRow in curEditRows.Where(s => !MyUtility.Check.Empty(s["UpdDivSewer"])))
            {
                needUpdRow["DivSewer"] = needUpdRow["UpdDivSewer"];
                needUpdRow["SewerDiffPercentageDesc"] = needUpdRow["UpdSewerDiffPercentage"];
                needUpdRow["SewerDiffPercentage"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(needUpdRow["UpdSewerDiffPercentage"]) / 100, 2);
            }

            // 將No空白資料清除
            foreach (var needRemoveRow in curEditRows.Where(s => MyUtility.Check.Empty(s["No"])).ToList())
            {
                this.dtAutomatedLineMapping_DetailCopy.Rows.Remove(needRemoveRow);
            }

            // 將selected都改成false
            foreach (var needCancelCheck in curEditRows.Where(s => MyUtility.Convert.GetBool(s["Selected"])))
            {
                needCancelCheck["Selected"] = false;
            }

            this.dtAutomatedLineMapping_Detail = this.dtAutomatedLineMapping_DetailCopy.Copy();

            this.dtAutomatedLineMapping_Detail.Columns.Remove("UpdDivSewer");
            this.dtAutomatedLineMapping_Detail.Columns.Remove("UpdSewerDiffPercentage");

            this.DialogResult = DialogResult.OK;
        }
    }
}
