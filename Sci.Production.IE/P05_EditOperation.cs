using Ict;
using Ict.Win;
using Sci.Production.CallPmsAPI;
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
        public DataTable dtAutomatedLineMapping_DetailCopy;

        private DataTable dtAutomatedLineMapping_Detail;
        private DataTable dtSelectItemSource;
        private DataTable dtNoSelectItem = new DataTable();
        private bool IsP05;

        /// <summary>
        /// P05_EditOperation
        /// </summary>
        /// <param name="dtAutomatedLineMapping_Detail">dtAutomatedLineMapping_Detail</param>
        /// <param name="isP05">isP05</param>
        public P05_EditOperation(DataTable dtAutomatedLineMapping_Detail, bool isP05 = true)
        {
            this.IsP05 = isP05;
            this.DialogResult = DialogResult.No;
            this.dtAutomatedLineMapping_Detail = dtAutomatedLineMapping_Detail;
            this.dtAutomatedLineMapping_DetailCopy = dtAutomatedLineMapping_Detail.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted && s["PPA"].ToString() != "C").OrderBy(s => s["No"]).CopyToDataTable();

            if (!this.dtAutomatedLineMapping_DetailCopy.Columns.Contains("UpdSewerDiffPercentage"))
            {
                this.dtAutomatedLineMapping_DetailCopy.Columns.Add("UpdSewerDiffPercentage", typeof(int));
            }

            if (this.IsP05 && !this.dtAutomatedLineMapping_DetailCopy.Columns.Contains("UpdDivSewer"))
            {
                this.dtAutomatedLineMapping_DetailCopy.Columns.Add("UpdDivSewer", typeof(decimal));
            }

            this.InitializeComponent();
            this.EditMode = true;

            this.gridIconEditOperation.InsertClick += this.GridIconEditOperation_InsertClick;
            this.gridIconEditOperation.AppendClick += this.GridIconEditOperation_AppendClick;
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
                .Where(s => checkedNo.Contains(s["No"].ToString()) && !MyUtility.Check.Empty(s["OperationID"]))
                .GroupBy(s => s["OperationID"])
                .Select(s => s.First())
                .TryCopyToDataTable(this.dtAutomatedLineMapping_DetailCopy);

            this.gridEditOperation.DataSource = this.gridEditOperationBs;
            this.gridEditOperationBs.DataSource = this.dtAutomatedLineMapping_DetailCopy;
            this.gridEditOperationBs.Filter = $"(No IN ({checkedNo.Select(s => $"'{s}'").JoinToString(",")}) OR Selected = 1)";
            if (isP05)
            {
                this.gridIconEditOperation.Location = new System.Drawing.Point(-30, 3);
                this.gridIconEditOperation.Append.Visible = false;
                this.gridIconEditOperation.Insert.Visible = true;
                this.gridIconEditOperation.Remove.Visible = false;
                new GridRowDrag(this.gridEditOperation, this.AfterRowDragDo, excludeDragCols: new List<string>() { "UpdSewerDiffPercentage", "UpdDivSewer" });
            }
            else
            {
                this.gridIconEditOperation.Location = new System.Drawing.Point(4, 3);
                this.gridIconEditOperation.Append.Visible = true;
                this.gridIconEditOperation.Insert.Visible = false;
                this.gridIconEditOperation.Remove.Visible = false;
                new GridRowDrag(this.gridEditOperation, this.AfterRowDragDo, excludeDragCols: new List<string>() { "UpdSewerDiffPercentage" });
            }

            this.IsP05 = isP05;
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
                (colDataPropertyName == "Location" ||
                colDataPropertyName == "MachineTypeID" ||
                colDataPropertyName == "MasterPlusGroup" ||
                colDataPropertyName == "OperationDesc"))
            {
                this.gridEditOperation.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Pink;
            }
            else if (colDataPropertyName == "No")
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

                if (curRow["No"].ToString() == item.GetSelectedString())
                {
                    return;
                }

                curRow["No"] = item.GetSelectedString();
                curRow.EndEdit();

                this.MergeSameTimeStudy_DetailUkeyByNo();
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

                if (curRow["No"].ToString() == item.GetSelectedString())
                {
                    return;
                }

                curRow["No"] = item.GetSelectedString();
                curRow.EndEdit();

                this.MergeSameTimeStudy_DetailUkeyByNo();
            };

            colGroupOperation.EditingMouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                DataRow curRow = this.gridEditOperation.GetDataRow(e.RowIndex);
                Win.Tools.SelectItem item;
                if (this.IsP05)
                {
                    item = new Win.Tools.SelectItem(this.dtSelectItemSource, "Location,MachineTypeID,MasterPlusGroup,OperationDesc,OriSewer,DivSewer", null, null, false, null, "Location,ST/MC,MC Group,Operation,Original Sewer,Original Div. Sewer", columndecimals: ",,,,4")
                    {
                        Width = 780,
                    };
                }
                else
                {
                    item = new Win.Tools.SelectItem(this.dtSelectItemSource, "Location,MachineTypeID,MasterPlusGroup,OperationDesc,OriSewer,", null, null, false, null, "Location,ST/MC,MC Group,Operation,Original Sewer", columndecimals: ",,,,4")
                    {
                        Width = 780,
                    };
                }

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
                if (!this.IsP05)
                {
                    curRow["Cycle"] = selectedResult["Cycle"];
                    curRow["GroupNo"] = selectedResult["GroupNo"];
                }
                else
                {
                    curRow["SewerDiffPercentageDesc"] = selectedResult["SewerDiffPercentageDesc"];
                    curRow["DivSewer"] = selectedResult["DivSewer"];
                    curRow["IsNotShownInP05"] = false;
                }

                curRow.EndEdit();

                string strColumns = selectedResult["OperationID"].ToString() == "PROCIPF00003" || selectedResult["OperationID"].ToString() == "PROCIPF00004" ? "OperationID" : "TimeStudyDetailUkey";
                DataTable dT_EditOperaror = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable()
                           .Where(x => ((x["TimeStudyDetailUkey"].ToString() == "0" &&
                                        (x["OperationID"].ToString() == "PROCIPF00003" || x["OperationID"].ToString() == "PROCIPF00004") &&
                                        x["OperationID"].ToString() == selectedResult["OperationID"].ToString()) ||
                                        (x["TimeStudyDetailUkey"].ToString() != "0" && x["TimeStudyDetailUkey"].ToString() == selectedResult["TimeStudyDetailUkey"].ToString())) &&
                                        !MyUtility.Check.Empty(x["OperationID"].ToString()) &&
                                        MyUtility.Convert.GetString(x["GroupNo"]) == MyUtility.Convert.GetString(selectedResult["GroupNo"]))
                           .TryCopyToDataTable((DataTable)this.gridEditOperationBs.DataSource);

                int intDT_EditOperaror = dT_EditOperaror.Rows.Count;
                int intUpd = 100 / intDT_EditOperaror;
                int icount = 0;
                for (int i = 0; i < this.dtAutomatedLineMapping_DetailCopy.Rows.Count; i++)
                {
                    if (this.dtAutomatedLineMapping_DetailCopy.Rows[i][strColumns].ToString() == MyUtility.Convert.GetString(selectedResult[strColumns]))
                    {
                        icount++;
                        if (intDT_EditOperaror == icount)
                        {
                            this.dtAutomatedLineMapping_DetailCopy.Rows[i]["UpdSewerDiffPercentage"] = 100 - (intUpd * (icount - 1));
                            if (this.IsP05)
                            {
                                this.dtAutomatedLineMapping_DetailCopy.Rows[i]["UpdDivSewer"] = (MyUtility.Convert.GetDecimal(selectedResult["DivSewer"]) * (intUpd * icount)) / 100;
                            }
                        }
                        else
                        {
                            this.dtAutomatedLineMapping_DetailCopy.Rows[i]["UpdSewerDiffPercentage"] = intUpd;
                            if (this.IsP05)
                            {
                                this.dtAutomatedLineMapping_DetailCopy.Rows[i]["UpdDivSewer"] = (MyUtility.Convert.GetDecimal(selectedResult["DivSewer"]) * intUpd) / 100;
                            }
                        }
                    }
                }

                this.MergeSameTimeStudy_DetailUkeyByNo();
            };

            colUpdSewer.CellValidating += (sender, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.gridEditOperation.GetDataRow<DataRow>(e.RowIndex);

                if (MyUtility.Check.Empty(e.FormattedValue) &&
                    dr[this.gridEditOperation.Columns[e.ColumnIndex].DataPropertyName] == DBNull.Value)
                {
                    return;
                }

                if (MyUtility.Convert.GetDecimal(e.FormattedValue) ==
                    MyUtility.Convert.GetDecimal(dr[this.gridEditOperation.Columns[e.ColumnIndex].DataPropertyName]))
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["UpdSewerDiffPercentage"] = DBNull.Value;
                    if (dr.Table.Columns.Contains("UpdDivSewer"))
                    {
                        dr["UpdDivSewer"] = DBNull.Value;
                    }

                    return;
                }

                dr[this.gridEditOperation.Columns[e.ColumnIndex].DataPropertyName] = e.FormattedValue;

                this.CaculateUpdSewerColumn(this.gridEditOperation.Columns[e.ColumnIndex].DataPropertyName, dr);

                // UpdDivSewer可能會有尾差，如果這筆是最後一筆，就做一次檢查修正
                var sameTimeStudyDetailUkeyDatas = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable()
                          .Where(s => MyUtility.Convert.GetLong(s["TimeStudyDetailUkey"]) == MyUtility.Convert.GetLong(dr["TimeStudyDetailUkey"]));
                int totalUpdSewerDiffPercentage = sameTimeStudyDetailUkeyDatas.Sum(s => MyUtility.Convert.GetInt(s["UpdSewerDiffPercentage"]));
                if (this.IsP05 && totalUpdSewerDiffPercentage == 100)
                {
                    decimal totalUpdDivSewer = sameTimeStudyDetailUkeyDatas.Sum(s => MyUtility.Convert.GetDecimal(s["UpdDivSewer"]));
                    decimal diffUpdDivSewer = MyUtility.Convert.GetDecimal(dr["OriSewer"]) - totalUpdDivSewer;
                    dr["UpdDivSewer"] = MyUtility.Convert.GetDecimal(dr["UpdDivSewer"]) + diffUpdDivSewer;
                }
            };

            if (this.IsP05)
            {
                this.Helper.Controls.Grid.Generator(this.gridEditOperation)
               .Text("No", header: "No", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: colNo)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: colGroupOperation)
               .Text("MachineTypeID", header: "ST/MC", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: colGroupOperation)
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: colGroupOperation)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: colGroupOperation)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: colGroupOperation)
               .Numeric("OriSewer", header: "Original" + Environment.NewLine + "Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "Ori. %", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("DivSewer", header: "Original" + Environment.NewLine + "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("UpdSewerDiffPercentage", header: "Update %", width: Widths.AnsiChars(5), iseditingreadonly: false, settings: colUpdSewer)
               .Numeric("UpdDivSewer", header: "Update" + Environment.NewLine + "Div. Sewer", decimal_places: 4, width: Widths.AnsiChars(5), iseditingreadonly: false, settings: colUpdSewer);
            }
            else
            {
                this.Helper.Controls.Grid.Generator(this.gridEditOperation)
               .Text("No", header: "No", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: colNo)
               .Text("Location", header: "Location", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("MachineTypeID", header: "ST/MC", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("OperationDesc", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Numeric("SewerDiffPercentageDesc", header: "Ori. %", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Numeric("UpdSewerDiffPercentage", header: "Update %", width: Widths.AnsiChars(5), iseditingreadonly: false, settings: colUpdSewer);
            }

            this.gridEditOperation.Columns["UpdSewerDiffPercentage"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridEditOperation.Columns["UpdSewerDiffPercentage"].DefaultCellStyle.ForeColor = Color.Red;

            if (this.IsP05)
            {
                this.gridEditOperation.Columns["UpdDivSewer"].DefaultCellStyle.BackColor = Color.Pink;
                this.gridEditOperation.Columns["UpdDivSewer"].DefaultCellStyle.ForeColor = Color.Red;
            }

            this.Text = this.IsP05 ? "P05. Edit No. Operation" : "P06. Edit No. Operation";
        }

        private void CaculateUpdSewerColumn(string colName, DataRow caculateRow)
        {
            if (!this.IsP05)
            {
                return;
            }

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

            DataRow selectedRow = this.gridEditOperation.GetDataRow(this.gridEditOperation.SelectedRows[0].Index);
            var noUpdSewerDiffPercentage = sameTimeStudyDetailUkeyData.Where(s => MyUtility.Convert.GetString(s["UpdSewerDiffPercentage"]) != MyUtility.Convert.GetString(selectedRow["UpdSewerDiffPercentage"]));

            if (sameTimeStudyDetailUkeyData.Count() == 2)
            {
                if (stillNotUpdDivSewer.Count() == 1)
                {
                    DataRow targetFixRow = stillNotUpdDivSewer.First();
                    decimal accuUpdDivSewer = sameTimeStudyDetailUkeyData.Sum(s => MyUtility.Convert.GetDecimal(s["UpdDivSewer"]));
                    decimal accuUpdDivSewerPercentage = sameTimeStudyDetailUkeyData.Sum(s => MyUtility.Convert.GetDecimal(s["UpdSewerDiffPercentage"]));
                    decimal oriSewer = MyUtility.Convert.GetDecimal(targetFixRow["OriSewer"]);
                    if (this.IsP05)
                    {
                        targetFixRow["UpdDivSewer"] = oriSewer < accuUpdDivSewer ? 0 : oriSewer - accuUpdDivSewer;
                    }
                    else
                    {
                        targetFixRow["UpdDivSewer"] = 0;
                    }

                    targetFixRow["UpdSewerDiffPercentage"] = accuUpdDivSewerPercentage >= 100 ? 0 : 100 - accuUpdDivSewerPercentage;
                }
                else
                {
                    DataRow targetFixRow = noUpdSewerDiffPercentage.First();
                    decimal accuUpdDivSewer = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(caculateRow["OriSewer"]) * MyUtility.Convert.GetDecimal(caculateRow["UpdSewerDiffPercentage"]) / 100, 4);
                    decimal accuUpdDivSewerPercentage = MyUtility.Convert.GetInt(selectedRow["UpdSewerDiffPercentage"]);
                    decimal oriSewer = MyUtility.Convert.GetDecimal(targetFixRow["OriSewer"]);
                    targetFixRow["UpdDivSewer"] = this.IsP05 ? oriSewer - accuUpdDivSewer : 0;
                    targetFixRow["UpdSewerDiffPercentage"] = 100 - accuUpdDivSewerPercentage;
                }
            }
        }

        private void GridIconEditOperation_AppendClick(object sender, EventArgs e)
        {
            if (this.gridEditOperation.SelectedRows.Count == 0)
            {
                return;
            }

            DataRow selectedRow = this.gridEditOperation.GetDataRow(this.gridEditOperation.SelectedRows[0].Index);

            if (MyUtility.Check.Empty(selectedRow["OperationID"].ToString())|| MyUtility.Check.Empty(selectedRow["TimeStudyDetailUkey"].ToString()))
            {
                return;
            }

            DataTable dT_EditOperaror = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable()
                                       .Where(x => ((x["TimeStudyDetailUkey"].ToString() == "0" &&
                                                    (x["OperationID"].ToString() == "PROCIPF00003" || x["OperationID"].ToString() == "PROCIPF00004") &&
                                                    x["OperationID"].ToString() == selectedRow["OperationID"].ToString()) ||
                                                    (x["TimeStudyDetailUkey"].ToString() != "0" && x["TimeStudyDetailUkey"].ToString() == selectedRow["TimeStudyDetailUkey"].ToString())) &&
                                                    !MyUtility.Check.Empty(x["OperationID"].ToString()) &&
                                                    MyUtility.Convert.GetString(x["GroupNo"]) == MyUtility.Convert.GetString(selectedRow["GroupNo"]))
                                       .TryCopyToDataTable((DataTable)this.gridEditOperationBs.DataSource);

            int intDT_EditOperaror = dT_EditOperaror.Rows.Count + 1;
            int intUpd = 100 / intDT_EditOperaror;
            DataRow newRow = this.dtAutomatedLineMapping_DetailCopy.NewRow();
            newRow.ItemArray = selectedRow.ItemArray; // 完整複製 selectedRow 的值
            newRow["Selected"] = true;
            newRow["No"] = string.Empty;
            newRow["UpdSewerDiffPercentage"] = intUpd;
            newRow["DivSewer"] = DBNull.Value;
            newRow["OriSewer"] = DBNull.Value;
            string strColumns = selectedRow["OperationID"].ToString() == "PROCIPF00003" || selectedRow["OperationID"].ToString() == "PROCIPF00004" ? "OperationID" : "TimeStudyDetailUkey";

            int icount = 0;
            for (int i = 0; i < this.dtAutomatedLineMapping_DetailCopy.Rows.Count; i++)
            {
                if (this.dtAutomatedLineMapping_DetailCopy.Rows[i][strColumns].ToString() == MyUtility.Convert.GetString(selectedRow[strColumns]))
                {
                    icount++;
                    if (icount == intDT_EditOperaror - 1)
                    {
                        this.dtAutomatedLineMapping_DetailCopy.Rows[i]["UpdSewerDiffPercentage"] = 100 - (intUpd * icount);
                    }
                    else
                    {
                        this.dtAutomatedLineMapping_DetailCopy.Rows[i]["UpdSewerDiffPercentage"] = intUpd;
                    }
                }
            }

            int insertIndex = this.dtAutomatedLineMapping_DetailCopy.Rows.IndexOf(selectedRow);
            this.dtAutomatedLineMapping_DetailCopy.Rows.InsertAt(newRow, insertIndex);
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
            newRow["UpdDivSewer"] = DBNull.Value;
            newRow["UpdSewerDiffPercentage"] = DBNull.Value;

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

                    if (!MyUtility.Check.Empty(dragRow["No"]))
                    {
                        this.MergeSameTimeStudy_DetailUkeyByNo();
                    }

                    break;
                }
            }
        }

        private decimal GetDecimalValue(object value, object defaultValue)
        {
            return value == DBNull.Value ? MyUtility.Convert.GetDecimal(defaultValue) : MyUtility.Convert.GetDecimal(value);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridEditOperationBs.EndEdit();

            var checkedNo = this.dtNoSelectItem.AsEnumerable().Select(s => s["No"].ToString()).ToList();
            var needKeepRows = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable()
                .Where(s => checkedNo.Contains(s["No"].ToString()) &&
                ((!this.IsP05 && ((s["UpdSewerDiffPercentage"] != DBNull.Value && MyUtility.Convert.GetDecimal(s["UpdSewerDiffPercentage"]) > 0) || s["UpdSewerDiffPercentage"] == DBNull.Value)) ||
                (this.IsP05 && ((s["UpdDivSewer"] != DBNull.Value && MyUtility.Convert.GetDecimal(s["UpdDivSewer"]) > 0) || s["UpdDivSewer"] == DBNull.Value))));

            #region 檢查DivSewer與UpdSewerDiffPercentage總和是否超過或不足

            IEnumerable<dynamic> checkDivSewerBalance;

            if (!this.IsP05)
            {
                checkDivSewerBalance = needKeepRows
                .GroupBy(s => s["TimeStudyDetailUkey"])
                .Select(groupItem => new
                {
                    TimeStudyDetailUkey = groupItem.First()["TimeStudyDetailUkey"].ToString(),
                    OperationId = groupItem.First()["OperationId"].ToString(),
                    OperationDesc = groupItem.First()["OperationDesc"].ToString(),
                    SumSewerDiffPercentage = groupItem.Sum(s => this.GetDecimalValue(s["UpdSewerDiffPercentage"], s["SewerDiffPercentageDesc"])),
                    SumSewerDiff = groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["UpdSewerDiffPercentage"])),
                    TimeStudyDetailUkeyCnt = groupItem.Count(),
                    DetailRows = groupItem,
                });
            }
            else
            {
                checkDivSewerBalance = needKeepRows
                .GroupBy(s => s["TimeStudyDetailUkey"])
                .Select(groupItem => new
                {
                    TimeStudyDetailUkey = groupItem.First()["TimeStudyDetailUkey"].ToString(),
                    OperationId = groupItem.First()["OperationId"].ToString(),
                    OperationDesc = groupItem.First()["OperationDesc"].ToString(),
                    OriSewer = MyUtility.Convert.GetDecimal(groupItem.First()["OriSewer"]),
                    SumDivSewer = groupItem.Sum(s => this.GetDecimalValue(s["UpdDivSewer"], s["DivSewer"])),
                    SumSewerDiffPercentage = groupItem.Sum(s => this.GetDecimalValue(s["UpdSewerDiffPercentage"], s["SewerDiffPercentageDesc"])),
                    SumSewerDiff = groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["UpdSewerDiffPercentage"])),
                    TimeStudyDetailUkeyCnt = groupItem.Count(),
                    DetailRows = groupItem,
                });
            }

            var dt = this.gridEditOperation.GetTable();

            if (dt != null)
            {
                var checkNo = dt.AsEnumerable()
                .Where(x => x.RowState != DataRowState.Deleted) // 排除已刪除的行
                .GroupBy(x => new { No = x["No"].ToString(), OperationCode = x["TimeStudyDetailUkey"].ToString() }) // 根據 No 和 TimeStudyDetailUkey 分組
                .Where(g => g.Count() > 1) // 找出有多筆相同 No 和 Operation Code 的分組
                .ToList().Count;

                if (checkNo > 0)
                {
                    MyUtility.Msg.WarningBox("This Operation already exists in the same No.!!");
                    return;
                }
            }

            foreach (var checkItem in checkDivSewerBalance)
            {
                if (checkItem.SumSewerDiffPercentage != 100)
                {
                    MyUtility.Msg.WarningBox($"The sum of [Update %] of Operation [{checkItem.OperationDesc}] needs to be equal to 100.");
                    return;
                }

                if (this.IsP05 && checkItem.OriSewer != checkItem.SumDivSewer)
                {
                    MyUtility.Msg.WarningBox($"The sum of [Update Div. Sewer] of Operation [{checkItem.OperationDesc}] needs to be equal to [Original Sewer].");
                    return;
                }
            }
            #endregion

            #region 檢查是否有TimeStudyDetailUkey遺漏
            List<long> curTimeStudyDetailUkey = needKeepRows.Select(s => MyUtility.Convert.GetLong(s["TimeStudyDetailUkey"])).Distinct().ToList();

            var listLoseTimeStudyDetailUkey = this.dtSelectItemSource.AsEnumerable()
                .Where(s => s["TimeStudyDetailUkey"].ToString() != "0" && !curTimeStudyDetailUkey.Contains(MyUtility.Convert.GetLong(s["TimeStudyDetailUkey"]))).ToList();
            if (listLoseTimeStudyDetailUkey.Any())
            {
                MyUtility.Msg.WarningBox($@"The following Operation is missing.
        {listLoseTimeStudyDetailUkey.Select(s => s["OperationDesc"].ToString()).JoinToString(Environment.NewLine)}");
                return;
            }
            #endregion
            #region 檢查No是否完整
            List<string> curNo = needKeepRows.Select(s => s["No"].ToString()).Distinct().ToList();

            var listLoseNo = this.dtNoSelectItem.AsEnumerable().Where(s => !curNo.Contains(s["No"].ToString())).ToList();
            if (listLoseNo.Any())
            {
                MyUtility.Msg.WarningBox($@"The following No is missing.
                {listLoseNo.Select(s => s["No"].ToString()).JoinToString(",")}");
                return;
            }
            #endregion

            if (this.IsP05)
            {
                // 更新調整過的DivSewer與SewerDiffPercentage
                foreach (var needUpdRow in needKeepRows.Where(s => !MyUtility.Check.Empty(s["UpdDivSewer"])))
                {
                    foreach (var groupItem in checkDivSewerBalance)
                    {
                        if (groupItem.TimeStudyDetailUkey == needUpdRow["TimeStudyDetailUkey"].ToString() && groupItem.SumSewerDiff == 100)
                        {
                            needUpdRow["DivSewer"] = needUpdRow["UpdDivSewer"];
                            needUpdRow["SewerDiffPercentageDesc"] = needUpdRow["UpdSewerDiffPercentage"];
                            needUpdRow["SewerDiffPercentage"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(needUpdRow["UpdSewerDiffPercentage"]) / 100, 2);
                        }
                    }
                }
            }
            else
            {
                // 更新調整過的DivSewer與SewerDiffPercentage
                foreach (var needUpdRow in needKeepRows)
                {
                    foreach (var groupItem in checkDivSewerBalance)
                    {
                        if (groupItem.TimeStudyDetailUkey == needUpdRow["TimeStudyDetailUkey"].ToString() && groupItem.SumSewerDiff == 100)
                        {
                            needUpdRow["SewerDiffPercentageDesc"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(needUpdRow["UpdSewerDiffPercentage"]) / 100, 2);
                            needUpdRow["SewerDiffPercentage"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(needUpdRow["UpdSewerDiffPercentage"]) / 100, 2);
                        }
                    }
                }
            }

            // 更新TimeStudyDetailUkeyCnt
            foreach (var groupItem in checkDivSewerBalance)
            {
                foreach (DataRow updRow in groupItem.DetailRows)
                {
                    updRow["TimeStudyDetailUkeyCnt"] = groupItem.TimeStudyDetailUkeyCnt;
                }
            }

            // 將No空白資料與UpdDivSewer = 0清除
            var needClearDatas = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable()
                                .Where(s => MyUtility.Check.Empty(s["No"]) ||
                                            (MyUtility.Convert.GetDecimal(s["UpdSewerDiffPercentage"]) == 0 && s["UpdSewerDiffPercentage"] != DBNull.Value) ||
                                            (this.IsP05 && MyUtility.Convert.GetDecimal(s["UpdDivSewer"]) == 0 && s["UpdDivSewer"] != DBNull.Value))
                                .ToList();
            foreach (var needRemoveRow in needClearDatas)
            {
                this.dtAutomatedLineMapping_DetailCopy.Rows.Remove(needRemoveRow);
            }

            foreach (var dr in this.dtAutomatedLineMapping_DetailCopy.AsEnumerable().Where(x => MyUtility.Convert.GetBool(x["Selected"]) && !MyUtility.Check.Empty(x["UpdSewerDiffPercentage"])))
            {
                dr["SewerDiffPercentageDesc"] = dr["UpdSewerDiffPercentage"];
                dr["SewerDiffPercentage"] = MyUtility.Convert.GetDecimal(dr["UpdSewerDiffPercentage"]) / 100;
            }

            foreach (var item in this.dtAutomatedLineMapping_Detail.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted && s["PPA"].ToString() != "C").ToList())
            {
                this.dtAutomatedLineMapping_Detail.Rows.Remove(item);
            }

            this.dtAutomatedLineMapping_DetailCopy.MergeTo(ref this.dtAutomatedLineMapping_Detail);

            this.DialogResult = DialogResult.OK;
        }

        private void MergeSameTimeStudy_DetailUkeyByNo()
        {
            this.backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.backgroundWorker.ReportProgress(0);
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var groupTimeStudy_DetailUkeyNo = this.dtAutomatedLineMapping_DetailCopy.AsEnumerable()
                                                .Where(x => x["Selected"].ToString() == "TRUE")
                                                .GroupBy(s => new
                                                {
                                                    OperationID = s["TimeStudyDetailUkey"].ToString(),
                                                    No = s["No"].ToString(),
                                                });

            if (groupTimeStudy_DetailUkeyNo.Any(s => s.Count() > 1 && !MyUtility.Check.Empty(s.Key.No)))
            {
                DataTable newAutomatedLineMapping_DetailCopy =
                    groupTimeStudy_DetailUkeyNo.Select(groupItem =>
                    {
                        DataRow newRow = this.dtAutomatedLineMapping_DetailCopy.NewRow();
                        newRow = groupItem.First();

                        // 同TimeStudy_DetailUkey, No有多筆情況要合併
                        if (groupItem.Count() > 1)
                        {
                            if (this.IsP05)
                            {
                                newRow["UpdDivSewer"] = groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["UpdDivSewer"]));
                                if (MyUtility.Check.Empty(newRow["UpdDivSewer"]))
                                {
                                    newRow["UpdDivSewer"] = DBNull.Value;
                                }

                                newRow["DivSewer"] = groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["DivSewer"]));
                            }

                            newRow["UpdSewerDiffPercentage"] = groupItem.Sum(s => MyUtility.Convert.GetInt(s["UpdSewerDiffPercentage"]));
                            if (MyUtility.Check.Empty(newRow["UpdSewerDiffPercentage"]))
                            {
                                newRow["UpdSewerDiffPercentage"] = DBNull.Value;
                            }

                            newRow["SewerDiffPercentage"] = groupItem.Sum(s => MyUtility.Convert.GetDecimal(s["SewerDiffPercentage"]));

                            newRow["SewerDiffPercentageDesc"] = MyUtility.Check.Empty(newRow["UpdSewerDiffPercentage"])
                                ? MyUtility.Convert.GetInt(MyUtility.Convert.GetDecimal(newRow["SewerDiffPercentage"]) * 100)
                                : (object)MyUtility.Convert.GetInt(MyUtility.Convert.GetDecimal(newRow["UpdSewerDiffPercentage"]));
                        }

                        return newRow;
                    }).CopyToDataTable();

                this.dtAutomatedLineMapping_DetailCopy = newAutomatedLineMapping_DetailCopy;
                this.gridEditOperationBs.DataSource = this.dtAutomatedLineMapping_DetailCopy;
            }
        }
    }
}
