using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P04_Detail : Win.Forms.Base
    {
        private readonly DataRow Deatilrow;
        private readonly DataRow MasterRow;
        private bool IsNewData = true;

        /// <inheritdoc/>
        public P04_Detail(bool editmode, DataRow masterrow, DataRow deatilrow)
        {
            this.InitializeComponent();
            this.MasterRow = masterrow;
            this.Deatilrow = deatilrow;
            this.EditMode = editmode;
            this.gridFGPT.CellPainting += this.GridFGPT_CellPainting;
            this.gridFGPT.CellFormatting += this.GridFGPT_CellFormatting;
        }

        private void GridFGPT_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0 || this.gridFGPT.Columns[e.ColumnIndex].Name != "Description")
            {
                return;
            }

            if (this.IsTheSamePreviousCellValue("Description", e.RowIndex, this.gridFGPT))
            {
                e.Value = string.Empty;
                e.FormattingApplied = true;
            }
        }

        private void GridFGPT_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex == this.gridFGPT.Rows.Count - 1 || e.ColumnIndex < 0)
            {
                return;
            }

            if (this.gridFGPT.Columns[e.ColumnIndex].Name != "Description")
            {
                return;
            }

            e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            if (this.IsTheSameNextCellValue("Description", e.RowIndex, this.gridFGPT))
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Bottom = this.gridFGPT.AdvancedCellBorderStyle.Bottom;
            }
        }

        private bool IsTheSamePreviousCellValue(string column, int row, DataGridView tarGrid)
        {
            DataGridViewCell cell1 = tarGrid[column, row];
            DataGridViewCell cell2 = tarGrid[column, row - 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private bool IsTheSameNextCellValue(string column, int row, DataGridView tarGrid)
        {
            DataGridViewCell cell1 = tarGrid[column, row];
            DataGridViewCell cell2 = tarGrid[column, row + 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private void P04_Detail_Load(object sender, EventArgs e)
        {
            this.Btnenable();

            this.comboTemperature.SelectedIndex = 0;
            this.comboMachineModel.SelectedIndex = 0;
            this.comboNeck.SelectedIndex = 0;

            DataGridViewGeneratorNumericColumnSettings beforeWash = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings afterWash1Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings afterWash1Cell2 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings afterWash1Cell3 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings afterWash1Cell4 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings beforeWash1 = new DataGridViewGeneratorNumericColumnSettings();

            #region 避免除數為0的檢查
            beforeWash.CellValidating += (s, eve) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (eve.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                DataRow dr = this.gridActualShrinkage.GetDataRow(eve.RowIndex);
                bool isAllEmpty = MyUtility.Check.Empty(dr["AfterWash1"]) && MyUtility.Check.Empty(dr["AfterWash2"]) && MyUtility.Check.Empty(dr["AfterWash3"]);

                if (MyUtility.Check.Empty(eve.FormattedValue) && !isAllEmpty)
                {
                    MyUtility.Msg.WarningBox("<BeforeWash> can not be empty or 0 !!");
                    dr["BeforeWash"] = dr["BeforeWash"];
                }
                else if (MyUtility.Check.Empty(eve.FormattedValue))
                {
                    dr["BeforeWash"] = dr["BeforeWash"];
                }
                else
                {
                    double beforeWashNum1 = Convert.ToDouble(eve.FormattedValue);

                    dr["BeforeWash"] = eve.FormattedValue;

                    if (!MyUtility.Check.Empty(dr["AfterWash1"]))
                    {
                        double afterWash1Num = Convert.ToDouble(dr["AfterWash1"]);
                        dr["Shrinkage1"] = (afterWash1Num - beforeWashNum1) / beforeWashNum1 * 100;
                    }

                    if (!MyUtility.Check.Empty(dr["AfterWash2"]))
                    {
                        double afterWash2Num = Convert.ToDouble(dr["AfterWash2"]);
                        dr["Shrinkage2"] = (afterWash2Num - beforeWashNum1) / beforeWashNum1 * 100;
                    }

                    if (!MyUtility.Check.Empty(dr["AfterWash3"]))
                    {
                        double afterWash3Num = Convert.ToDouble(dr["AfterWash3"]);
                        dr["Shrinkage3"] = (afterWash3Num - beforeWashNum1) / beforeWashNum1 * 100;
                    }
                }
            };

            afterWash1Cell.CellValidating += (s, eve) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (eve.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(eve.FormattedValue))
                {
                    return; // 沒資料 return
                }

                DataRow dr = this.gridActualShrinkage.GetDataRow(eve.RowIndex);

                if (MyUtility.Check.Empty(dr["BeforeWash"]))
                {
                    MyUtility.Msg.WarningBox("<BeforeWash> can not be empty or 0 !!");
                    dr["AfterWash1"] = dr["AfterWash1"];
                }
                else
                {
                    double beforeWashNum1 = Convert.ToDouble(dr["BeforeWash"]);
                    double afterWash1Num = Convert.ToDouble(eve.FormattedValue);
                    dr["AfterWash1"] = eve.FormattedValue;
                    dr["Shrinkage1"] = (afterWash1Num - beforeWashNum1) / beforeWashNum1 * 100;
                }
            };

            afterWash1Cell2.CellValidating += (s, eve) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (eve.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(eve.FormattedValue))
                {
                    return; // 沒資料 return
                }

                DataRow dr = this.gridActualShrinkage.GetDataRow(eve.RowIndex);

                if (MyUtility.Check.Empty(dr["BeforeWash"]))
                {
                    MyUtility.Msg.WarningBox("<BeforeWash> can not be empty or 0 !!");
                    dr["AfterWash2"] = dr["AfterWash2"];
                }
                else
                {
                    double beforeWashNum1 = Convert.ToDouble(dr["BeforeWash"]);
                    double afterWash2Num = Convert.ToDouble(eve.FormattedValue);
                    dr["AfterWash2"] = eve.FormattedValue;
                    dr["Shrinkage2"] = (afterWash2Num - beforeWashNum1) / beforeWashNum1 * 100;
                }
            };

            afterWash1Cell3.CellValidating += (s, eve) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (eve.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(eve.FormattedValue))
                {
                    return; // 沒資料 return
                }

                DataRow dr = this.gridActualShrinkage.GetDataRow(eve.RowIndex);

                if (MyUtility.Check.Empty(dr["BeforeWash"]))
                {
                    MyUtility.Msg.WarningBox("<BeforeWash> can not be empty or 0 !!");
                    dr["AfterWash3"] = dr["AfterWash3"];
                }
                else
                {
                    double beforeWashNum1 = Convert.ToDouble(dr["BeforeWash"]);
                    double afterWash3Num = Convert.ToDouble(eve.FormattedValue);
                    dr["AfterWash3"] = eve.FormattedValue;
                    dr["Shrinkage3"] = (afterWash3Num - beforeWashNum1) / beforeWashNum1 * 100;
                }
            };

            beforeWash1.CellValidating += (s, eve) =>
            {
                if (!this.EditMode || MyUtility.Check.Empty(eve.FormattedValue) || eve.RowIndex == -1)
                {
                    return; // 非編輯模式
                }

                DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);

                if (MyUtility.Check.Empty(eve.FormattedValue))
                {
                    dr["BeforeWash"] = eve.FormattedValue;
                    dr["Shrinkage"] = DBNull.Value;
                }
                else
                {
                    // 分母不為0才計算Shrikage(%)
                    double beforeWashNum = MyUtility.Convert.GetDouble(eve.FormattedValue);
                    double afterWash = MyUtility.Convert.GetDouble(dr["AfterWash"]);
                    dr["BeforeWash"] = beforeWashNum;
                    dr["Shrinkage"] = (afterWash - beforeWashNum) / beforeWashNum * 100.0;
                }
            };

            beforeWash1.CellEditable += (s, eve) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);
                if ((dr["Criteria"] != DBNull.Value || dr["Criteria2"] != DBNull.Value) && !MyUtility.Convert.GetBool(dr["IsInPercentage"]))
                {
                    eve.IsEditable = true;
                }
                else
                {
                    eve.IsEditable = false;
                }
            };

            afterWash1Cell4.CellValidating += (s, eve) =>
            {
                if (!this.EditMode || MyUtility.Check.Empty(eve.FormattedValue) || eve.RowIndex == -1)
                {
                    return; // 非編輯模式
                }

                DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);

                double afterWash = Convert.ToDouble(eve.FormattedValue);
                dr["AfterWash"] = afterWash;

                // 分母不為0才計算Shrikage(%)
                if (MyUtility.Check.Empty(dr["BeforeWash"]))
                {
                    dr["Shrinkage"] = DBNull.Value;
                }
                else
                {
                    double beforeWashNum = MyUtility.Convert.GetDouble(dr["BeforeWash"]);
                    dr["Shrinkage"] = (afterWash - beforeWashNum) / beforeWashNum * 100.0;
                }
            };

            #endregion

            this.Helper.Controls.Grid.Generator(this.gridActualShrinkage)
            .Text("Location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Type", header: "Type", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Numeric("BeforeWash", header: "Before Wash", width: Widths.AnsiChars(6), decimal_places: 2, settings: beforeWash)
            .Numeric("SizeSpec", header: "Size Spec Meas.", width: Widths.AnsiChars(8), decimal_places: 2)
            .Numeric("AfterWash1", header: "After Wash 1", width: Widths.AnsiChars(6), decimal_places: 2, settings: afterWash1Cell)
            .Numeric("Shrinkage1", header: "Shrinkage 1(%)", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999, iseditable: false)
            .Numeric("AfterWash2", header: "After Wash 3", width: Widths.AnsiChars(6), decimal_places: 2, settings: afterWash1Cell2)
            .Numeric("Shrinkage2", header: "Shrinkage 3(%)", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999, iseditable: false)
            .Numeric("AfterWash3", header: "After Wash 15", width: Widths.AnsiChars(6), decimal_places: 2, settings: afterWash1Cell3)
            .Numeric("Shrinkage3", header: "Shrinkage 15(%)", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999, iseditable: false);

            Dictionary<string, string> resultPF = new Dictionary<string, string>
            {
                { "P", "Pass" },
                { "F", "Fail" },
            };
            this.comboResult.DataSource = new BindingSource(resultPF, null);
            this.comboResult.ValueMember = "Key";
            this.comboResult.DisplayMember = "Value";

            DataGridViewGeneratorComboBoxColumnSettings resultComboCell = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings artworkCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings textColumnSetting = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings scaleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings sizeSpecCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings mm_N_ComboCell = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings threeTestCell = new DataGridViewGeneratorTextColumnSettings();

            artworkCell.CellMouseClick += (s, eve) =>
            {
                if (!this.IsNewData)
                {
                    if (eve.RowIndex == -1 || (eve.RowIndex != 0 && eve.RowIndex != 3))
                    {
                        return;
                    }
                }
                else
                {
                    if (eve.RowIndex == -1 || (eve.RowIndex != 0 && eve.RowIndex != 2))
                    {
                        return;
                    }
                }

                if (this.EditMode == false)
                {
                    return;
                }

                // 第一列跟第三列開啟的Type不一樣
                if (eve.Button == MouseButtons.Right && eve.RowIndex == 0)
                {
                    DataRow dr = this.gridAppearance.GetDataRow(eve.RowIndex);

                    string sql = "select ID, Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' order by Seq";

                    string defaultSelected = string.Empty; // dr["Type"].ToString().Replace(" / ", ",");
                    string[] selectedNames = dr["Type"].ToString().Replace("/", ",").Split(',');
                    List<string> tmpList = new List<string>();

                    foreach (var name in selectedNames)
                    {
                        string iD = MyUtility.GetValue.Lookup($"select TOP 1 ID  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' AND Name LIKE '{name.Trim()}%' ");
                        tmpList.Add(iD);
                    }

                    defaultSelected = tmpList.JoinToString(",");

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sql, "ID,Name", "ID,Name", defaultSelected, null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string insertString = string.Empty;
                    string[] selectedIDs = item.GetSelectedString().ToString().Split(',');
                    List<string> tmpList2 = new List<string>();

                    foreach (var iD in selectedIDs)
                    {
                        string name = MyUtility.GetValue.Lookup($"select TOP 1 Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' AND ID = '{iD.Trim()}' ");
                        tmpList2.Add(name);
                    }

                    insertString = tmpList2.JoinToString(" / ");

                    dr["Type"] = insertString;

                    dr.EndEdit();
                }

                if (eve.Button == MouseButtons.Right && ((!this.IsNewData && eve.RowIndex == 3) || (this.IsNewData && eve.RowIndex == 2)))
                {
                    DataRow dr = this.gridAppearance.GetDataRow(eve.RowIndex);

                    string sql = "select ID, Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' order by Seq";

                    string defaultSelected = string.Empty; // dr["Type"].ToString().Replace(" / ", ",");
                    string[] selectedNames = dr["Type"].ToString().Replace("/", ",").Split(',');
                    List<string> tmpList = new List<string>();

                    foreach (var name in selectedNames)
                    {
                        string iD = MyUtility.GetValue.Lookup($"select TOP 1 ID  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' AND Name = '{name.Trim()}' ");
                        tmpList.Add(iD);
                    }

                    defaultSelected = tmpList.JoinToString(",");

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sql, "ID,Name", "ID,Name", defaultSelected.ToString(), null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string insertString = string.Empty;
                    string[] selectedIDs = item.GetSelectedString().ToString().Split(',');
                    List<string> tmpList2 = new List<string>();

                    foreach (var iD in selectedIDs)
                    {
                        string name = MyUtility.GetValue.Lookup($"select TOP 1 Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' AND ID = '{iD.Trim()}' ");
                        tmpList2.Add(name);
                    }

                    insertString = tmpList2.JoinToString(" / ");

                    dr["Type"] = insertString;

                    dr.EndEdit();
                }
            };

            artworkCell.EditingMouseDown += (s, eve) =>
            {
                if (!this.IsNewData)
                {
                    if (eve.RowIndex == -1 || (eve.RowIndex != 0 && eve.RowIndex != 3))
                    {
                        return;
                    }
                }
                else
                {
                    if (eve.RowIndex == -1 || (eve.RowIndex != 0 && eve.RowIndex != 2))
                    {
                        return;
                    }
                }

                if (this.EditMode == false)
                {
                    return;
                }

                // 第一列跟第三列開啟的Type不一樣
                if (eve.Button == MouseButtons.Right && eve.RowIndex == 0)
                {
                    DataRow dr = this.gridAppearance.GetDataRow(eve.RowIndex);

                    string sql = "select ID, Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' order by Seq";

                    string defaultSelected = string.Empty; // dr["Type"].ToString().Replace(" / ", ",");
                    string[] selectedNames = dr["Type"].ToString().Replace("/", ",").Split(',');
                    List<string> tmpList = new List<string>();

                    foreach (var name in selectedNames)
                    {
                        string iD = MyUtility.GetValue.Lookup($"select TOP 1 ID  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' AND Name LIKE '{name.Trim()}%' ");
                        tmpList.Add(iD);
                    }

                    defaultSelected = tmpList.JoinToString(",");

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sql, "ID,Name", "ID,Name", defaultSelected, null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string insertString = string.Empty;
                    string[] selectedIDs = item.GetSelectedString().ToString().Split(',');
                    List<string> tmpList2 = new List<string>();

                    foreach (var iD in selectedIDs)
                    {
                        string name = MyUtility.GetValue.Lookup($"select TOP 1 Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' AND ID = '{iD.Trim()}' ");
                        tmpList2.Add(name);
                    }

                    insertString = tmpList2.JoinToString(" / ");

                    dr["Type"] = insertString;

                    dr.EndEdit();
                }

                if (eve.Button == MouseButtons.Right && ((!this.IsNewData && eve.RowIndex == 3) || (this.IsNewData && eve.RowIndex == 2)))
                {
                    DataRow dr = this.gridAppearance.GetDataRow(eve.RowIndex);

                    string sql = "select ID, Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' order by Seq";

                    string defaultSelected = string.Empty; // dr["Type"].ToString().Replace(" / ", ",");
                    string[] selectedNames = dr["Type"].ToString().Replace("/", ",").Split(',');
                    List<string> tmpList = new List<string>();

                    foreach (var name in selectedNames)
                    {
                        string iD = MyUtility.GetValue.Lookup($"select TOP 1 ID  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' AND Name = '{name.Trim()}' ");
                        tmpList.Add(iD);
                    }

                    defaultSelected = tmpList.JoinToString(",");

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sql, "ID,Name", "ID,Name", defaultSelected.ToString(), null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string insertString = string.Empty;
                    string[] selectedIDs = item.GetSelectedString().ToString().Split(',');
                    List<string> tmpList2 = new List<string>();

                    foreach (var iD in selectedIDs)
                    {
                        string name = MyUtility.GetValue.Lookup($"select TOP 1 Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' AND ID = '{iD.Trim()}' ");
                        tmpList2.Add(name);
                    }

                    insertString = tmpList2.JoinToString(" / ");

                    dr["Type"] = insertString;

                    dr.EndEdit();
                }
            };

            scaleCell.CellMouseClick += (s, eve) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                // 第一列跟第三列開啟的Type不一樣
                if (eve.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);

                    if (dr["Scale"] == DBNull.Value)
                    {
                        return;
                    }

                    string sql = "select ID from Scale WHERE Junk=0";

                    string defaultSelected = MyUtility.Convert.GetString(dr["Scale"]);

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "ID,Name", "ID,Name", defaultSelected, null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Scale"] = item.GetSelectedString().ToString();
                    dr.EndEdit();
                }
            };

            scaleCell.EditingMouseDown += (s, eve) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                if (eve.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);

                    if (dr["Scale"] == DBNull.Value)
                    {
                        return;
                    }

                    string sql = "select ID from Scale  WHERE Junk=0";

                    string defaultSelected = MyUtility.Convert.GetString(dr["Scale"]);

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "ID,Name", "ID,Name", defaultSelected, null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Scale"] = item.GetSelectedString().ToString();
                    dr.EndEdit();
                }
            };

            scaleCell.CellValidating += (s, eve) =>
            {
                if (!this.EditMode || MyUtility.Check.Empty(eve.FormattedValue))
                {
                    return; // 非編輯模式
                }

                if (eve.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);
                string scale = eve.FormattedValue.ToString();

                string sql = "select ID from Scale  WHERE Junk=0 AND ID =@Scale";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Scale", scale),
                };

                if (!MyUtility.Check.Seek(sql, parameters))
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                    dr.EndEdit();
                    eve.Cancel = true;
                    return;
                }

                dr["Scale"] = eve.FormattedValue;
            };

            sizeSpecCell.CellEditable += (s, eve) =>
            {
                DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);

                if ((dr["Criteria"] != DBNull.Value || dr["Criteria2"] != DBNull.Value) && !MyUtility.Convert.GetBool(dr["IsInPercentage"]))
                {
                    eve.IsEditable = true;
                }
                else
                {
                    eve.IsEditable = false;
                }
            };

            afterWash1Cell4.CellEditable += (s, eve) =>
            {
                DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);

                if ((dr["Criteria"] != DBNull.Value || dr["Criteria2"] != DBNull.Value) && !MyUtility.Convert.GetBool(dr["IsInPercentage"]))
                {
                    eve.IsEditable = true;
                }
                else
                {
                    eve.IsEditable = false;
                }
            };

            scaleCell.CellEditable += (s, eve) =>
            {
                DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);

                if (dr["Scale"] != DBNull.Value && !MyUtility.Convert.GetBool(dr["IsInPercentage"]))
                {
                    eve.IsEditable = true;
                }
                else
                {
                    eve.IsEditable = false;
                }
            };

            threeTestCell.CellMouseClick += (s, eve) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                // 第一列跟第三列開啟的Type不一樣
                if (eve.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridFGPT.GetDataRow(eve.RowIndex);
                    string testUnit = MyUtility.Convert.GetString(dr["TestUnit"]);

                    if (testUnit != "mm" && testUnit != "Pass/Fail")
                    {
                        return;
                    }

                    string sql = string.Empty;

                    if (testUnit.ToLower() == "mm")
                    {
                        sql = "select ID ='≦4' UNION select ID ='>4' ";
                    }

                    if (testUnit == "Pass/Fail")
                    {
                        sql = "select ID ='Pass' UNION select ID ='Fail' ";
                    }

                    string defaultSelected = MyUtility.Convert.GetString(dr["TestResult"]);

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "ID,Name", "ID,Name", defaultSelected, null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["TestResult"] = item.GetSelectedString().ToString();
                    dr.EndEdit();
                }
            };

            threeTestCell.EditingMouseDown += (s, eve) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                // 第一列跟第三列開啟的Type不一樣
                if (eve.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridFGPT.GetDataRow(eve.RowIndex);
                    string testUnit = MyUtility.Convert.GetString(dr["TestUnit"]);

                    if (testUnit != "mm" && testUnit != "Pass/Fail")
                    {
                        return;
                    }

                    string sql = string.Empty;

                    if (testUnit.ToLower() == "mm")
                    {
                        sql = "select ID ='≦4' UNION select ID ='>4' ";
                    }

                    if (testUnit == "Pass/Fail")
                    {
                        sql = "select ID ='Pass' UNION select ID ='Fail' ";
                    }

                    string defaultSelected = MyUtility.Convert.GetString(dr["TestResult"]);

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "ID,Name", "ID,Name", defaultSelected, null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["TestResult"] = item.GetSelectedString().ToString();
                    dr.EndEdit();
                }
            };

            threeTestCell.CellValidating += (s, eve) =>
            {
                if (!this.EditMode || MyUtility.Check.Empty(eve.FormattedValue))
                {
                    return; // 非編輯模式
                }

                if (eve.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                DataRow dr = this.gridFGPT.GetDataRow(eve.RowIndex);
                string testUnit = MyUtility.Convert.GetString(dr["TestUnit"]);

                if (testUnit.ToLower() == "mm")
                {
                    if (MyUtility.Convert.GetString(eve.FormattedValue) != "≦4" && MyUtility.Convert.GetString(eve.FormattedValue) != "<=4" && MyUtility.Convert.GetString(eve.FormattedValue) != ">4")
                    {
                        MyUtility.Msg.WarningBox("Must be [<=4] or [≦4] or [>4] !!");
                        eve.FormattedValue = string.Empty;
                        dr["TestResult"] = string.Empty;
                        return;
                    }

                    dr["TestResult"] = eve.FormattedValue;
                }

                if (testUnit.ToUpper() == "N")
                {
                    if (!decimal.TryParse(eve.FormattedValue.ToString(), out decimal t) ||
                    !Prgs.CheckFloat(MyUtility.Convert.GetString(eve.FormattedValue), 0, 3))
                    {
                        MyUtility.Msg.WarningBox(@"third digit after the decimal point)
ex: 150.423");
                        eve.FormattedValue = string.Empty;
                        dr["TestResult"] = string.Empty;
                        return;
                    }

                    dr["TestResult"] = MyUtility.Convert.GetString(eve.FormattedValue);
                }

                if (testUnit == "Pass/Fail")
                {
                    if (MyUtility.Convert.GetString(eve.FormattedValue).ToUpper() != "PASS" && MyUtility.Convert.GetString(eve.FormattedValue) != "FAIL")
                    {
                        MyUtility.Msg.WarningBox("Must be [Pass] or [Fail] !!");
                        eve.FormattedValue = string.Empty;
                        dr["TestResult"] = string.Empty;
                        return;
                    }

                    switch (MyUtility.Convert.GetString(eve.FormattedValue).ToUpper())
                    {
                        case "PASS":
                            dr["TestResult"] = "Pass";
                            break;
                        case "FAIL":
                            dr["TestResult"] = "Fail";
                            break;

                        default:
                            break;
                    }
                }
            };

            mm_N_ComboCell.CellValidating += (s, eve) =>
            {
                if (!this.EditMode || MyUtility.Check.Empty(eve.FormattedValue))
                {
                    return; // 非編輯模式
                }

                if (eve.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                DataRow dr = this.gridFGPT.GetDataRow(eve.RowIndex);
                string oldTestUnit = MyUtility.Convert.GetString(dr["TestUnit"]);
                string newTestUnit = MyUtility.Convert.GetString(eve.FormattedValue);

                if (newTestUnit != oldTestUnit)
                {
                    dr["TestResult"] = string.Empty;
                    dr["TestUnit"] = newTestUnit;
                }
            };

            mm_N_ComboCell.CellEditable += (s, eve) =>
            {
                DataRow dr = this.gridFGPT.GetDataRow(eve.RowIndex);

                if (MyUtility.Convert.GetString(dr["TestUnit"]) == "Pass/Fail")
                {
                    eve.IsEditable = false;
                }
                else
                {
                    eve.IsEditable = true;
                }
            };

            Dictionary<string, string> resultCombo = new Dictionary<string, string>
            {
                { "N/A", "N/A" },
                { "Accepted", "Accepted" },
                { "Rejected", "Rejected" },
            };
            resultComboCell.DataSource = new BindingSource(resultCombo, null);
            resultComboCell.ValueMember = "Key";
            resultComboCell.DisplayMember = "Value";

            Dictionary<string, string> mm_NCombo = new Dictionary<string, string>
            {
                { "mm", "mm" },
                { "N", "N" },
            };
            mm_N_ComboCell.DataSource = new BindingSource(mm_NCombo, null);
            mm_N_ComboCell.ValueMember = "Key";
            mm_N_ComboCell.DisplayMember = "Value";

            // 預設選取的時候會全部變成大寫，關掉這個設定。
            textColumnSetting.CharacterCasing = CharacterCasing.Normal;
            textColumnSetting.MaxLength = 500;
            artworkCell.MaxLength = 200;
            scaleCell.MaxLength = 5;

            this.Helper.Controls.Grid.Generator(this.gridAppearance)
            .Text("Type", header: "After Wash Appearance Check list", width: Widths.AnsiChars(40), iseditingreadonly: true, settings: artworkCell)
            .ComboBox("Wash1", header: "Wash1", width: Widths.AnsiChars(10), settings: resultComboCell)
            .ComboBox("Wash2", header: "Wash3", width: Widths.AnsiChars(10), settings: resultComboCell)
            .ComboBox("Wash3", header: "Wash15", width: Widths.AnsiChars(10), settings: resultComboCell)
            .Text("Comment", header: "Comment", width: Widths.AnsiChars(20), settings: textColumnSetting)
            ;

            this.Helper.Controls.Grid.Generator(this.gridFGWT)
            .Text("LocationText", header: "Location", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Text("SystemType", header: "Type", width: Widths.AnsiChars(40), iseditingreadonly: true)
            .Numeric("BeforeWash", header: "Before Wash", width: Widths.AnsiChars(6), decimal_places: 2, settings: beforeWash1)
            .Numeric("AfterWash", header: "After Wash", width: Widths.AnsiChars(6), decimal_places: 2, settings: afterWash1Cell4)
            .Numeric("Shrinkage", header: "Shrikage(%)", width: Widths.AnsiChars(6), iseditingreadonly: true, decimal_places: 2)
            .Text("Scale", header: "Scale", width: Widths.AnsiChars(10), settings: scaleCell)
            .Text("Result", header: "Result", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;

            DataGridViewGeneratorTextColumnSettings type = new DataGridViewGeneratorTextColumnSettings();
            type.CellPainting += (s, eve) =>
            {
                if (eve.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridFGPT.GetDataRow(eve.RowIndex);
                eve.CellStyle.BackColor = MyUtility.Convert.GetInt(dr["TypeSelection_VersionID"]) == 0 ? Color.White : Color.Yellow;
            };

            type.EditingMouseDown += (s, eve) =>
            {
                if (eve.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridFGPT.GetDataRow(eve.RowIndex);
                if (this.EditMode && eve.Button == MouseButtons.Right && MyUtility.Convert.GetInt(dr["TypeSelection_VersionID"]) > 0)
                {
                    string sqlcmd = $"select Seq,Code from TypeSelection where VersionID = {dr["TypeSelection_VersionID"]} order by seq";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, string.Empty, dr["TypeSelection_Seq"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["TypeDisplay"] = string.Format(MyUtility.Convert.GetString(dr["Type"]), item.GetSelecteds()[0]["Code"].ToString());
                    dr["Code"] = item.GetSelecteds()[0]["Code"].ToString();
                    dr["TypeSelection_Seq"] = item.GetSelectedString();

                    dr.EndEdit();
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridFGPT)
            .Text("LocationText", header: "Location", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Text("Description", header: "Test Name", width: Widths.AnsiChars(23), iseditingreadonly: true)
            .Text("TypeDisplay", header: "Type", width: Widths.AnsiChars(70), iseditingreadonly: true, settings: type)
            .Text("TestResult", header: "TestResult", width: Widths.AnsiChars(10), settings: threeTestCell)
            .ComboBox("TestUnit", header: "Test Detail", width: Widths.AnsiChars(10), iseditable: false, settings: mm_N_ComboCell)
            .Text("Result", header: "Result", width: Widths.AnsiChars(6), iseditingreadonly: true)
            ;

            this.Tab1Load();
            this.Tab2Load();
            this.TabSpirality();
            this.Tab3Load();
            this.Tab4Load();
            this.Tab5Load();

            // 為了繞過底層一個Grid Bug, 在 Load 完後所有分頁都顯示一次
            for (int i = 0; i < this.tabControl1.TabPages.Count; i++)
            {
                this.tabControl1.SelectedIndex = i;
            }

            this.tabControl1.SelectedIndex = 0;
        }

        private void Tab1Load()
        {
            this.txtStyle.Text = MyUtility.Convert.GetString(this.MasterRow["StyleID"]);
            this.txtSeason.Text = MyUtility.Convert.GetString(this.MasterRow["SeasonID"]);
            this.txtBrand.Text = MyUtility.Convert.GetString(this.MasterRow["BrandID"]);
            this.txtSP.Text = MyUtility.Convert.GetString(this.MasterRow["OrderID"]);
            this.txtArticle.Text = MyUtility.Convert.GetString(this.MasterRow["Article"]);
            this.txtSize.Text = MyUtility.Convert.GetString(this.Deatilrow["SizeCode"]);

            this.dateSubmit.Value = MyUtility.Convert.GetDate(this.Deatilrow["SubmitDate"]);
            this.numArriveQty.Value = MyUtility.Convert.GetInt(this.Deatilrow["ArrivedQty"]);
            this.txtLotoFactory.Text = MyUtility.Convert.GetString(this.Deatilrow["LOtoFactory"]);
            this.comboResult.Text = MyUtility.Convert.GetString(this.Deatilrow["Result"]);
            this.txtRemark.Text = MyUtility.Convert.GetString(this.Deatilrow["Remark"]);
            this.rdbtnLine.Checked = MyUtility.Convert.GetBool(this.Deatilrow["LineDry"]);
            this.rdbtnTumble.Checked = MyUtility.Convert.GetBool(this.Deatilrow["TumbleDry"]);
            this.rdbtnHand.Checked = MyUtility.Convert.GetBool(this.Deatilrow["HandWash"]);
            this.comboTemperature.Text = MyUtility.Convert.GetString(this.Deatilrow["Temperature"]);
            this.comboMachineModel.Text = MyUtility.Convert.GetString(this.Deatilrow["Machine"]);
            this.txtFibreComposition.Text = MyUtility.Convert.GetString(this.Deatilrow["Composition"]);
            this.comboNeck.Text = MyUtility.Convert.GetBool(this.Deatilrow["Neck"]) ? "YES" : "No";
            this.comboResult.Text = MyUtility.Convert.GetString(this.Deatilrow["Result"]) == "P" ? "Pass" : "Fail";

            this.radioNaturalFibres.Checked = MyUtility.Convert.GetBool(this.Deatilrow["Above50NaturalFibres"]);
            this.radioSyntheticFibres.Checked = MyUtility.Convert.GetBool(this.Deatilrow["Above50SyntheticFibres"]);

            // 如果兩個都未勾選，設選擇 synthetic fibres
            if (!this.radioNaturalFibres.Checked && !this.radioSyntheticFibres.Checked)
            {
                this.radioSyntheticFibres.Checked = true;
            }
        }

        private DataTable dtShrinkage;

        private void Tab2Load()
        {
            this.gridActualShrinkage.IsEditingReadOnly = false;

            string sqlShrinkage = $@"select [ID]      ,[No]
      ,[Location]=case when Location='T' then 'TOP'
                               when Location='I' then 'INNER'
                               when Location='O' then 'OUTER'
                               when Location='B' then 'BOTTOM' end
      ,[Type]      ,[BeforeWash]      ,[SizeSpec]      ,[AfterWash1]      ,[Shrinkage1]      ,[AfterWash2]      ,[Shrinkage2]      ,[AfterWash3]      ,[Shrinkage3]
from[GarmentTest_Detail_Shrinkage] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}
order by Location desc, seq";

            DBProxy.Current.Select(null, sqlShrinkage, out this.dtShrinkage);
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource1.DataSource = this.dtShrinkage;
            int i = 4;

            this.flowLayoutPanel1.Height = 36 * i;
        }

        private DataTable dtSpirality;

        private void TabSpirality()
        {
            string sqlcmd = $@"select * from Garment_Detail_Spirality where id = '{this.Deatilrow["ID"]}' and No = '{this.Deatilrow["No"]}'";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtSpirality);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dtSpirality.Select("Location = 'T'").Any())
            {
                this.panelTop.Visible = true;
                DataRow dr = this.dtSpirality.Select("Location = 'T'")[0];
                this.numTOPMehtodA.Value = MyUtility.Convert.GetDecimal(dr["MethodA"]);
                this.numTOPMehtodB.Value = MyUtility.Convert.GetDecimal(dr["MethodB"]);
                this.numTopAAp.Value = MyUtility.Convert.GetDecimal(dr["AAPrime"]);
                this.numTopApB.Value = MyUtility.Convert.GetDecimal(dr["APrimeB"]);
                this.numTopAB.Value = MyUtility.Convert.GetDecimal(dr["AB"]);
                this.numTopCM.Value = MyUtility.Convert.GetDecimal(dr["CM"]);
            }

            if (this.dtSpirality.Select("Location = 'B'").Any())
            {
                this.panelBottom.Visible = true;
                DataRow dr = this.dtSpirality.Select("Location = 'B'")[0];
                this.numBottomMethodA.Value = MyUtility.Convert.GetDecimal(dr["MethodA"]);
                this.numBottomMethodB.Value = MyUtility.Convert.GetDecimal(dr["MethodB"]);
                this.numBottomAAp.Value = MyUtility.Convert.GetDecimal(dr["AAPrime"]);
                this.numBottomApB.Value = MyUtility.Convert.GetDecimal(dr["APrimeB"]);
                this.numBottomAB.Value = MyUtility.Convert.GetDecimal(dr["AB"]);
                this.numBottomCM.Value = MyUtility.Convert.GetDecimal(dr["CM"]);
            }
        }

        private DataTable dtApperance;

        private void Tab3Load()
        {
            this.gridAppearance.IsEditingReadOnly = false;

            // string sqlApperance = $@"select * from[GarmentTest_Detail_Apperance] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} order by seq";
            string sqlApperance = $@"
SELECT * 
FROM GarmentTest_Detail_Apperance
where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} 
order by seq";

            DBProxy.Current.Select(null, sqlApperance, out this.dtApperance);
            this.listControlBindingSource2.DataSource = null;
            this.listControlBindingSource2.DataSource = this.dtApperance;

            if (this.dtApperance.Rows.Count == 9)
            {
                this.IsNewData = false;
            }
            else
            {
                this.IsNewData = true;
            }

            // ISP20190838 第二項 只有第一列跟第三列可以修改,且背景顏色改為粉色
            for (int i = 0; i <= this.gridAppearance.Rows.Count - 1; i++)
            {
                // 由於程式修改，新寫的資料筆數會少一筆，因此要跟著變INDEX  //ISP20190838
                if (!this.IsNewData)
                {
                    if (i == 0 || i == 3)
                    {
                        this.gridAppearance.Rows[i].Cells["Type"].Style.BackColor = Color.White;
                    }
                }
                else
                {
                    if (i == 0 || i == 2)
                    {
                        this.gridAppearance.Rows[i].Cells["Type"].Style.BackColor = Color.White;
                    }
                }
            }
        }

        private DataTable dtFGWT;

        private void Tab4Load()
        {
            this.gridFGWT.IsEditingReadOnly = false;

            string sqlFGWT = $@"
select [LocationText]= CASE WHEN Location='B' THEN 'Bottom'
						WHEN Location='T' THEN 'Top'
						WHEN Location='S' THEN 'Top+Bottom'
						ELSE ''
					END
		,Location
        ,f.Type
        ,f.SystemType
		,f.ID
		,f.No
        ,BeforeWash 
        ,SizeSpec 
        ,AfterWash
        ,f.Shrinkage
        ,f.Scale
        ,f.TestDetail
        ,[Result]=IIF(f.Scale IS NOT NULL
	        ,IIF( f.Scale='4-5' OR f.Scale ='5','Pass',IIF(f.Scale='','','Fail'))
	        ,IIF( (f.BeforeWash IS NOT NULL AND f.AfterWash IS NOT NULL AND f.Criteria IS NOT NULL AND f.Shrinkage IS NOT NULL)
                  or (f.Type = 'spirality: Garment - in percentage (average)')
                  or (f.Type = 'spirality: Garment - in percentage (average) (Top Method A)')
                  or (f.Type = 'spirality: Garment - in percentage (average) (Top Method B)')
                  or (f.Type = 'spirality: Garment - in percentage (average) (Bottom Method A)')
                  or (f.Type = 'spirality: Garment - in percentage (average) (Bottom Method B)')
					,( IIF( TestDetail = '%' OR TestDetail = 'Range%'   -- % 為ISP20201331舊資料、Range% 為ISP20201606加上的新資料，兩者都視作百分比
								---- 百分比 判斷方式
								,IIF( ISNULL(f.Criteria,0)  <= ISNULL(f.Shrinkage,0) AND ISNULL(f.Shrinkage,0) <= ISNULL(f.Criteria2,0)
									, 'Pass'
									, 'Fail'
								)
								---- 非百分比 判斷方式
								,IIF( ISNULL(f.AfterWash,0) - ISNULL(f.BeforeWash,0) <= ISNULL(f.Criteria,0)
									,'Pass'
									,'Fail'
								)
						)
					)
					,''
		        )
        )
		,gd.MtlTypeID
		,f.Criteria
		,f.Criteria2
        ,[IsInPercentage] =cast((
                            case when f.Type = 'spirality: Garment - in percentage (average)'
                                   or f.Type = 'spirality: Garment - in percentage (average) (Top Method A)'
                                   or f.Type = 'spirality: Garment - in percentage (average) (Top Method B)' 
                                   or f.Type = 'spirality: Garment - in percentage (average) (Bottom Method A)'
                                   or f.Type = 'spirality: Garment - in percentage (average) (Bottom Method B)' then 1
                                 else 0 end) as bit)
from GarmentTest_Detail_FGWT f 
LEFT JOIN GarmentTest_Detail gd ON f.ID = gd.ID AND f.No = gd.NO
where f.id = {this.Deatilrow["ID"]} and f.No = {this.Deatilrow["No"]} 
order by Seq ASC, LocationText DESC";

            DBProxy.Current.Select(null, sqlFGWT, out this.dtFGWT);
            this.gridFGWT.DataSource = this.dtFGWT;

            if (this.dtFGWT.Rows.Count > 0 || this.EditMode)
            {
                this.btnGenerateFGWT.Enabled = false;
            }
            else
            {
                this.btnGenerateFGWT.Enabled = true;
            }
        }

        private void Tab4Save()
        {
            DataTable gridFGWT = (DataTable)this.gridFGWT.DataSource;

            string cmd = $@"
update gf
	set gf.[BeforeWash]  = t.[BeforeWash],
		gf.[SizeSpec]  = t.[SizeSpec],
		gf.[AfterWash]	= t.[AfterWash],
		gf.[Shrinkage]	= iif(gf.Type = 'spirality: Garment - in percentage (average)', iif(sl.Location in ('B','T','S') , gt.Twisting, 0), t.Shrinkage),
		gf.[Scale]	= t.[Scale] 
from GarmentTest_Detail_FGWT gf
inner join #tmp t on gf.ID =t.ID and gf.No = t.No and gf.Location = t.Location and gf.Type = t.Type
outer apply (
	select distinct
		[Location] = iif (slC.cnt > 1, 'S', sl.Location)
	from GarmentTest g
	inner join Style s on g.StyleID = s.ID and g.BrandID = s.BrandID and g.SeasonID = s.SeasonID
	inner join Style_Location sl on s.Ukey = sl.StyleUkey
	outer apply (
		select cnt = count(*)
		from Style_Location sl 
		where s.Ukey = sl.StyleUkey
		and sl.Location in ('B', 'T')
	)slC
	where gf.ID = g.ID
)sl 
outer apply (
	select Twisting = sum(Twisting)
	from (
		select Twisting = case when sl.Location in ('B','T') then gt.Twisting
					when sl.Location = 'S' and gt.Location = 'B' then gt.Twisting
					else 0
					end
		from GarmentTest_Detail_Twisting gt
		where gf.ID = gt.ID and gf.No = gt.No
	)gt
)gt

{this.UpdateGarmentTest_Detail_FGWTShrinkage()}
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(gridFGWT, string.Empty, cmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.Tab4Load();
        }

        private string UpdateGarmentTest_Detail_FGWTShrinkage()
        {
            return $@"
update GarmentTest_Detail_FGWT set Shrinkage = {this.numTOPMehtodA.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and type = 'spirality: Garment - in percentage (average) (Top Method A)'
update GarmentTest_Detail_FGWT set Shrinkage = {this.numTOPMehtodB.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and type = 'spirality: Garment - in percentage (average) (Top Method B)'
update GarmentTest_Detail_FGWT set Shrinkage = {this.numBottomMethodA.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and type = 'spirality: Garment - in percentage (average) (Bottom Method A)'
update GarmentTest_Detail_FGWT set Shrinkage = {this.numBottomMethodB.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and type = 'spirality: Garment - in percentage (average) (Bottom Method B)'
";
        }

        private DataTable dtFGPT;

        private void Tab5Load()
        {
            this.gridFGPT.IsEditingReadOnly = false;
            string sqlFGPT = $@"
select [LocationText]= CASE WHEN Location='B' THEN 'Bottom'
						WHEN Location='T' THEN 'Top'
						WHEN Location='S' THEN 'Top+Bottom'
						ELSE ''
					END
		,f.*
        ,[Result]=	CASE WHEN  f.TestUnit = 'N' AND f.[TestResult] !='' THEN IIF( Cast( f.[TestResult] as float) >= cast( f.Criteria as float) ,'Pass' ,'Fail')
						 WHEN  f.TestUnit = 'mm' THEN IIF(  f.[TestResult] = '<=4' OR f.[TestResult] = '≦4','Pass' , IIF( f.[TestResult]='>4','Fail','')  )
						 WHEN  f.TestUnit = 'Pass/Fail'  THEN f.[TestResult]
					 	 ELSE ''
					END
        ,ddl.Description
        ,TypeDisplay = iif(f.TypeSelection_VersionID > 0, Replace(f.type, '{{0}}', ts.Code), f.type)
        ,ts.Code
from GarmentTest_Detail_FGPT f 
left join DropDownList ddl with (nolock) on  ddl.Type = 'PMS_FGPT_TestName' and ddl.ID = f.TestName
left join TypeSelection ts on ts.VersionID = f.TypeSelection_VersionID and ts.Seq = f.TypeSelection_Seq
where f.id = {this.Deatilrow["ID"]} and f.No = {this.Deatilrow["No"]} 
order by f.TestName,f.Seq,LocationText DESC";

            DBProxy.Current.Select(null, sqlFGPT, out this.dtFGPT);
            this.gridFGPT.DataSource = this.dtFGPT;
        }

        private void Tab5Save()
        {
            string cmd = $@"
merge GarmentTest_Detail_FGPT t
using #tmp s
on s.id = t.id and s.no = t.no and s.Location = t.Location and s.Type = t.Type and s.seq = t.seq and s.TestName = t.TestName
when matched then
update set
	t.[TestResult]  = s.[TestResult],
	t.[TestUnit]  = s.[TestUnit],
    t.TypeSelection_Seq = s.TypeSelection_Seq
	;

delete t
from GarmentTest_Detail_FGPT t
left join #tmp s on s.id = t.id and s.no = t.no and s.Location = t.Location and s.Type	= t.Type and s.seq = t.seq and s.TestName = t.TestName
where s.id is null
and  t.id = {this.Deatilrow["ID"]} and t.No = {this.Deatilrow["No"]} 
";

            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.gridFGPT.DataSource, string.Empty, cmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.Tab5Load();
        }

        private void BtnEncode_Click(object sender, EventArgs e)
        {
            DualResult dr = DBProxy.Current.Execute(null, $"Update [GarmentTest_Detail] set Status='Confirmed' where id = '{this.MasterRow["ID"]}'");
            if (!dr)
            {
                this.ShowErr(dr);
            }
            else
            {
                this.Btnenable();
            }
        }

        private void BtnAmend_Click(object sender, EventArgs e)
        {
            DualResult dr = DBProxy.Current.Execute(null, $"Update [GarmentTest_Detail] set Status='New' where id = '{this.MasterRow["ID"]}'");
            if (!dr)
            {
                this.ShowErr(dr);
            }
            else
            {
                this.Btnenable();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                #region update GarmentTest_Detail 1
                string submitDate = MyUtility.Check.Empty(this.dateSubmit.Value) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.dateSubmit.Value)).ToString("yyyy/MM/dd");
                string updateGarmentTest_Detail = $@"
update GarmentTest_Detail set
    SubmitDate = iif('{submitDate}'='',null,'{submitDate}'),
    ArrivedQty =  {this.numArriveQty.Value},
    LOtoFactory =  '{this.txtLotoFactory.Text}',
    Result = '{this.comboResult.SelectedValue}',
    Remark =  '{this.txtRemark.Text}',
    LineDry =  '{this.rdbtnLine.Checked}',
    Temperature =  '{this.comboTemperature.Text}',
    TumbleDry =  '{this.rdbtnTumble.Checked}',
    Machine =  '{this.comboMachineModel.Text}',
    HandWash =  '{this.rdbtnHand.Checked}',
    Composition =  '{this.txtFibreComposition.Text}',
    Neck ='{MyUtility.Convert.GetString(this.comboNeck.Text).EqualString("YES")}' ,
    Above50NaturalFibres =  {(this.radioNaturalFibres.Checked ? "1" : "0")},
    Above50SyntheticFibres =  {(this.radioSyntheticFibres.Checked ? "1" : "0")}
where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["NO"]}
";
                DualResult dr = DBProxy.Current.Execute(null, updateGarmentTest_Detail);
                if (!dr)
                {
                    this.ShowErr(dr);
                }
                #endregion
                this.Tab2ShrinkageSave();
                this.TabSpiralitySave();
                DBProxy.Current.Execute(null, $"update Garmenttest_Detail set Editname = '{Env.User.UserID}',EditDate = getdate() where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}");
                this.Tab2Load();
                this.TabSpirality();

                this.Tab3ApperanceSave();
                this.Tab3Load();

                this.Tab4Save();
                this.Tab4Load();

                this.Tab5Save();
                this.Tab5Load();

                this.Btnenable();
                this.gridAppearance.ForeColor = Color.Black;

                this.gridFGPT.Columns["TestResult"].DefaultCellStyle.BackColor = Color.White;
            }
            else
            {
                for (int i = 0; i <= this.gridAppearance.Rows.Count - 1; i++)
                {
                    // 由於程式修改，新寫的資料筆數會少一筆，因此要跟著變INDEX  //ISP20190838
                    if (!this.IsNewData)
                    {
                        if (i == 0 || i == 3)
                        {
                            this.gridAppearance.Rows[i].Cells["Type"].Style.BackColor = Color.Pink;
                        }
                        else
                        {
                            this.gridAppearance.Rows[i].Cells["Type"].Style.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        if (i == 0 || i == 2)
                        {
                            this.gridAppearance.Rows[i].Cells["Type"].Style.BackColor = Color.Pink;
                        }
                        else
                        {
                            this.gridAppearance.Rows[i].Cells["Type"].Style.ForeColor = Color.Black;
                        }
                    }
                }

                this.gridFGPT.Columns["TestResult"].DefaultCellStyle.BackColor = Color.Pink;

                this.gridAppearance.ForeColor = Color.Red;
                this.btnEncode.Enabled = false;
                this.btnAmend.Enabled = false;
            }

            this.EditMode = !this.EditMode;
            this.rdbtnLine.Enabled = this.EditMode;
            this.rdbtnTumble.Enabled = this.EditMode;
            this.rdbtnHand.Enabled = this.EditMode;
            this.gridActualShrinkage.ReadOnly = !this.EditMode;
            this.gridAppearance.ReadOnly = !this.EditMode;
            this.gridFGWT.ReadOnly = !this.EditMode;
            this.gridFGPT.ReadOnly = !this.EditMode;
            this.btnToReport.Enabled = !this.EditMode;
            this.btnEdit.Text = this.EditMode ? "Save" : "Edit";
            this.btnDelete.Visible = this.tabControl1.SelectedTab.Name == "tabFGPT" && this.EditMode;
            this.radioNaturalFibres.Enabled = this.EditMode;
            this.radioSyntheticFibres.Enabled = this.EditMode;

            if (this.dtFGWT.Rows.Count > 0 || this.EditMode)
            {
                this.btnGenerateFGWT.Enabled = false;
            }
            else
            {
                this.btnGenerateFGWT.Enabled = true;
            }
        }

        private void Tab2ShrinkageSave()
        {
            string savetab2Shrinkage = $@"
update #tmp set Location = 'B' where Location = 'BOTTOM'
update #tmp set Location = 'T' where Location = 'TOP'
update #tmp set Location = 'I' where Location = 'INNER'
update #tmp set Location = 'O' where Location = 'OUTER'

  merge [GarmentTest_Detail_Shrinkage] t
  using #tmp s
  on s.id = t.id and s.no = t.no and s.Location = t.Location and s.type = t.type
  when matched then
  update set
	t.[BeforeWash]  = s.[BeforeWash],
    t.[SizeSpec]	= s.[SizeSpec]	,
    t.[AfterWash1]	= s.[AfterWash1],
    t.[Shrinkage1]	= s.[Shrinkage1],
    t.[AfterWash2]	= s.[AfterWash2],
    t.[Shrinkage2]	= s.[Shrinkage2],
    t.[AfterWash3]	= s.[AfterWash3],
    t.[Shrinkage3]	= s.[Shrinkage3]
	;

select * from [GarmentTest_Detail_Shrinkage]  where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}";

            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.listControlBindingSource1.DataSource, string.Empty, savetab2Shrinkage, out this.dtShrinkage);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
        }

        private void TabSpiralitySave()
        {
            string sqlcmd = $@"
UPDATE [dbo].[Garment_Detail_Spirality]
   SET [AAPrime] = {this.numTopAAp.Value}
      ,[APrimeB] = {this.numTopApB.Value}
      ,[AB] = {this.numTopAB.Value}
      ,[CM] = {this.numTopCM.Value}
      ,[MethodA] = {this.numTOPMehtodA.Value}
      ,[MethodB] = {this.numTOPMehtodB.Value}
WHERE id = '{this.Deatilrow["ID"]}' and No = '{this.Deatilrow["No"]}' and Location = 'T'

UPDATE [dbo].[Garment_Detail_Spirality]
   SET [AAPrime] = {this.numBottomAAp.Value}
      ,[APrimeB] = {this.numBottomApB.Value}
      ,[AB] = {this.numBottomAB.Value}
      ,[CM] = {this.numBottomCM.Value}
      ,[MethodA] = {this.numBottomMethodA.Value}
      ,[MethodB] = {this.numBottomMethodB.Value}
WHERE id = '{this.Deatilrow["ID"]}' and No = '{this.Deatilrow["No"]}' and Location = 'B'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        private void Tab3ApperanceSave()
        {
            DataTable gridAppearance = (DataTable)this.listControlBindingSource2.DataSource;

            string savetab2Apperance = $@"
  merge [GarmentTest_Detail_Apperance] t
  using #tmp s
  on s.id = t.id and s.no = t.no and s.seq = t.seq
  when matched then
  update set
	t.[Type]  = s.[Type],
	t.[Wash1]  = s.[Wash1],
    t.[Wash2]	= s.[Wash2],
    t.[Wash3]	= s.[Wash3],
    t.[Wash4]	= s.[Wash4],
    t.[Wash5]	= s.[Wash5],
    t.[Comment]	= s.[Comment]
	;

select * from [GarmentTest_Detail_Apperance]  where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} order by seq

";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(gridAppearance, string.Empty, savetab2Apperance, out this.dtApperance);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
        }

        private void Btnenable()
        {
            string detailstatus = MyUtility.GetValue.Lookup($"select Status from GarmentTest_Detail where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["NO"]}");
            this.btnEncode.Enabled = detailstatus.EqualString("New");
            this.btnAmend.Enabled = detailstatus.EqualString("Confirmed");
            this.btnEdit.Enabled = !detailstatus.EqualString("Confirmed");
            this.rdbtnLine.Enabled = this.EditMode;
            this.rdbtnTumble.Enabled = this.EditMode;
            this.rdbtnHand.Enabled = this.EditMode;
            this.radioNaturalFibres.Enabled = this.EditMode;
            this.radioSyntheticFibres.Enabled = this.EditMode;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridFGPT_EditingKeyProcessing(object sender, Ict.Win.UI.DataGridViewEditingKeyProcessingEventArgs e)
        {
            bool isLastRow = this.gridFGPT.CurrentRow.Index == this.gridFGPT.Rows.Count - 1;
            int nextRowIndex = this.gridFGPT.CurrentRow.Index + 1;

            // 在Yardage按下Tab，且是最後一Row
            if (e.KeyData == Keys.Tab /*&& this.gridFGPT.CurrentCell.OwningColumn.Name != "TestResult" && isLastRow*/)
            {
                //// 若是最後一筆，則跳到一列
                if (isLastRow)
                {
                    this.gridFGPT.CurrentCell = this.gridFGPT.Rows[0].Cells[1];
                }
                else
                {
                    this.gridFGPT.CurrentCell = this.gridFGPT.Rows[nextRowIndex].Cells[1];
                }
            }
        }

        private void BtnToReport_Click(object sender, EventArgs e)
        {
            Sci.Production.Quality.P04Data p04Data = new Sci.Production.Quality.P04Data
            {
                DateSubmit = this.dateSubmit.Value,

                NumArriveQty = this.numArriveQty.Value,
                TxtSize = this.txtSize.Text,
                RdbtnLine = this.rdbtnLine.Checked,
                RdbtnTumble = this.rdbtnTumble.Checked,
                RdbtnHand = this.rdbtnHand.Checked,
                ComboTemperature = this.comboTemperature.Text,
                ComboMachineModel = this.comboMachineModel.Text,
                TxtFibreComposition = this.txtFibreComposition.Text,
                ComboNeck = this.comboNeck.Text,
                TxtLotoFactory = this.txtLotoFactory.Text,
            };

            P04_ToReport form = new P04_ToReport(this.MasterRow, this.Deatilrow, this.IsNewData, this.dtApperance, this.dtShrinkage, this.dtFGWT, this.dtFGPT, p04Data, this.dtSpirality);
            form.ShowDialog(this);
            form.Dispose();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnDelete.Visible = this.tabControl1.SelectedTab.Name == "tabFGPT" && this.EditMode;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.gridFGPT.CurrentDataRow != null)
            {
                this.gridFGPT.CurrentDataRow.Delete();
            }
        }

        private void BtnGenerateFGWT_Click(object sender, EventArgs e)
        {
            // 避免多位使用者同時使用
            bool dataExists = MyUtility.Check.Seek($"SELECT 1 FROM GarmentTest_Detail_FGWT WHERE ID = {this.Deatilrow["ID"]}AND NO = {this.Deatilrow["No"]} ");

            if (dataExists)
            {
                MyUtility.Msg.InfoBox("Data already exists!!");
                this.Tab4Load();
                return;
            }

            if (MyUtility.Check.Empty(this.Deatilrow["MtlTypeID"]))
            {
                MyUtility.Msg.InfoBox("Please set Material Type first!!");
                return;
            }

            string washType = string.Empty;
            string fibresType = string.Empty;

            if (this.rdbtnLine.Checked)
            {
                washType = "Line";
            }
            else if (this.rdbtnTumble.Checked)
            {
                washType = "Tumnle";
            }
            else if (this.rdbtnHand.Checked)
            {
                washType = "Hand";
            }

            if (this.radioNaturalFibres.Checked)
            {
                fibresType = "Natural";
            }
            else if (this.radioSyntheticFibres.Checked)
            {
                fibresType = "Synthetic";
            }

            List<string> locations = MyUtility.GetValue.Lookup($@"

SELECT STUFF(
	(select DISTINCT ',' + sl.Location
	from Style s
	INNER JOIN Style_Location sl ON s.Ukey = sl.StyleUkey 
	where s.id='{this.MasterRow["StyleID"]}' AND s.BrandID='{this.MasterRow["BrandID"]}' AND s.SeasonID='{this.MasterRow["SeasonID"]}'
	FOR XML PATH('')
	) 
,1,1,'')").Split(',').ToList();

            string mtlTypeID = MyUtility.Convert.GetString(this.Deatilrow["MtlTypeID"]);

            List<FGWT> fGWTs = new List<FGWT>();
            bool containsT = locations.Contains("T");
            bool containsB = locations.Contains("B");

            // 若只有B則寫入Bottom的項目+ALL的項目，若只有T則寫入TOP的項目+ALL的項目，若有B和T則寫入Top+ Bottom的項目+ALL的項目
            // 若為Hand只寫入第88項
            if (washType == "Hand" && mtlTypeID != "WOVEN")
            {
                fGWTs = GetDefaultFGWT(false, false, false, mtlTypeID, washType, fibresType);
            }
            else
            {
                if (containsT && containsB)
                {
                    fGWTs = GetDefaultFGWT(false, false, true, mtlTypeID, washType, fibresType);
                }
                else if (containsT)
                {
                    fGWTs = GetDefaultFGWT(containsT, false, false, mtlTypeID, washType, fibresType);
                }
                else
                {
                    fGWTs = GetDefaultFGWT(false, containsB, false, mtlTypeID, washType, fibresType);
                }
            }

            string garmentTest_Detail_ID = MyUtility.Convert.GetString(this.Deatilrow["ID"]);
            string garmentTest_Detail_No = MyUtility.Convert.GetString(this.Deatilrow["NO"]);

            StringBuilder insertCmd = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int idx = 0;

            // 組合INSERT SQL
            foreach (var fGWT in fGWTs)
            {
                string location = string.Empty;

                switch (fGWT.Location)
                {
                    case "Top":
                        location = "T";
                        break;
                    case "Bottom":
                        location = "B";
                        break;
                    case "Top+Bottom":
                        location = "S";
                        break;
                    default:
                        break;
                }

                if (fGWT.Scale == null)
                {
                    if (fGWT.TestDetail.ToUpper() == "CM")
                    {
                        insertCmd.Append($@"

INSERT INTO GarmentTest_Detail_FGWT
           (ID, No, Location, Type ,TestDetail ,Criteria, SystemType, Seq)
     VALUES
           ( {garmentTest_Detail_ID}
           , {garmentTest_Detail_No}
           , @Location{idx}
           , @Type{idx}
           , @TestDetail{idx}
           , @Criteria{idx}
           , @SystemType{idx}
           , @Seq{idx})

");
                    }
                    else
                    {
                        if (fGWT.Type.ToUpper() == "DIMENSIONAL CHANGE: FLAT MADE-UP TEXTILE ARTICLES A) OVERALL LENGTH" || fGWT.Type.ToUpper() == "DIMENSIONAL CHANGE: FLAT MADE-UP TEXTILE ARTICLES B) OVERALL WIDTH")
                        {
                            insertCmd.Append($@"

INSERT INTO GarmentTest_Detail_FGWT
           (ID, No, Location, Type ,TestDetail, SystemType, Seq )
     VALUES
           ( {garmentTest_Detail_ID}
           , {garmentTest_Detail_No}
           , @Location{idx}
           , @Type{idx}
           , @TestDetail{idx} 
           , @SystemType{idx}
           , @Seq{idx})

");
                        }
                        else if (fGWT.Type.ToUpper() == "SPIRALITY: GARMENT - IN PERCENTAGE (AVERAGE)")
                        {
                            insertCmd.Append($@"

INSERT INTO GarmentTest_Detail_FGWT
           (ID, No, Location, Type ,TestDetail ,Criteria ,Criteria2, SystemType, Seq, Shrinkage)
     VALUES
           ( {garmentTest_Detail_ID}
           , {garmentTest_Detail_No}
           , @Location{idx}
           , @Type{idx}
           , @TestDetail{idx}
           , @Criteria{idx} 
           , @Criteria2_{idx}
           , @SystemType{idx}
           , @Seq{idx}
           ,iif(@Location{idx} in ('B','T','S') ,(select sum(Twisting)
	                                                from (
	                                                	select Twisting = case when @Location{idx} in ('B','T') then gt.Twisting
	                                                				when @Location{idx} = 'S' and gt.Location = 'B' then gt.Twisting
	                                                				else 0
	                                                				end
	                                                	from GarmentTest_Detail_Twisting gt
	                                                	where gt.ID = {garmentTest_Detail_ID} and gt.No = {garmentTest_Detail_No}
	                                                )gt),0
               )
           )

");
                        }
                        else
                        {
                            insertCmd.Append($@"

INSERT INTO GarmentTest_Detail_FGWT
           (ID, No, Location, Type ,TestDetail ,Criteria ,Criteria2, SystemType, Seq )
     VALUES
           ( {garmentTest_Detail_ID}
           , {garmentTest_Detail_No}
           , @Location{idx}
           , @Type{idx}
           , @TestDetail{idx}
           , @Criteria{idx} 
           , @Criteria2_{idx}
           , @SystemType{idx}
           , @Seq{idx})

");
                        }
                    }
                }
                else
                {
                    insertCmd.Append($@"

INSERT INTO GarmentTest_Detail_FGWT
           (ID, No, Location, Type ,Scale,TestDetail, SystemType, Seq)
     VALUES
           ( {garmentTest_Detail_ID}
           , {garmentTest_Detail_No}
           , @Location{idx}
           , @Type{idx}
           , ''
           , @TestDetail{idx}
           , @SystemType{idx}
           , @Seq{idx})

");
                }

                parameters.Add(new SqlParameter($"@Location{idx}", location));
                parameters.Add(new SqlParameter($"@Type{idx}", fGWT.Type));
                parameters.Add(new SqlParameter($"@TestDetail{idx}", fGWT.TestDetail));
                parameters.Add(new SqlParameter($"@Criteria{idx}", fGWT.Criteria));
                parameters.Add(new SqlParameter($"@Criteria2_{idx}", fGWT.Criteria2));
                parameters.Add(new SqlParameter($"@SystemType{idx}", fGWT.SystemType));
                parameters.Add(new SqlParameter($"@Seq{idx}", fGWT.Seq));
                idx++;
            }

            // 找不到才Insert
            if (!MyUtility.Check.Seek($"SELECT 1 FROM GarmentTest_Detail_FGWT WHERE ID ='{garmentTest_Detail_ID}' AND NO='{garmentTest_Detail_No}'"))
            {
                DualResult r = DBProxy.Current.Execute(null, insertCmd.ToString() + this.UpdateGarmentTest_Detail_FGWTShrinkage(), parameters);
                if (!r)
                {
                    this.ShowErr(r);
                }
                else
                {
                    MyUtility.Msg.InfoBox("Success!!");
                    this.Tab4Load();
                }
            }
        }

        private void NumTop_Validated(object sender, EventArgs e)
        {
            this.numTOPMehtodA.Value = this.numTopApB.Value == 0 ? 0 : this.numTopAAp.Value / this.numTopApB.Value * 100; // (AA’ / A’B) *100
            this.numTOPMehtodB.Value = this.numTopAB.Value == 0 ? 0 : this.numTopAAp.Value / this.numTopAB.Value * 100; // (AA’ / AB) * 100
        }

        private void NumBottom_Validated(object sender, EventArgs e)
        {
            this.numBottomMethodA.Value = this.numBottomApB.Value == 0 ? 0 : this.numBottomAAp.Value / this.numBottomApB.Value * 100; // (AA’ / A’B) *100
            this.numBottomMethodB.Value = this.numBottomAB.Value == 0 ? 0 : this.numBottomAAp.Value / this.numBottomAB.Value * 100; // (AA’ / AB) * 100
        }
    }
}
