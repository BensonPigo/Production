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
                if (!MyUtility.Check.Empty(dr["Criteria"]))
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
            .Text("Type", header: "Type", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Numeric("BeforeWash", header: "Before Wash", width: Widths.AnsiChars(6), decimal_places: 2, settings: beforeWash)
            .Numeric("SizeSpec", header: "Size Spec Meas.", width: Widths.AnsiChars(8), decimal_places: 2)
            .Numeric("AfterWash1", header: "After Wash 1", width: Widths.AnsiChars(6), decimal_places: 2, settings: afterWash1Cell)
            .Numeric("Shrinkage1", header: "Shrinkage 1(%)", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999, iseditable: false)
            .Numeric("AfterWash2", header: "After Wash 2", width: Widths.AnsiChars(6), decimal_places: 2, settings: afterWash1Cell2)
            .Numeric("Shrinkage2", header: "Shrinkage 2(%)", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999, iseditable: false)
            .Numeric("AfterWash3", header: "After Wash 3", width: Widths.AnsiChars(6), decimal_places: 2, settings: afterWash1Cell3)
            .Numeric("Shrinkage3", header: "Shrinkage 3(%)", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999, iseditable: false);

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

                if (dr["Scale"] != DBNull.Value || MyUtility.Check.Empty(dr["Criteria"]))
                {
                    eve.IsEditable = false;
                }
                else
                {
                    eve.IsEditable = true;
                }
            };

            afterWash1Cell4.CellEditable += (s, eve) =>
            {
                DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);

                if (dr["Scale"] != DBNull.Value || MyUtility.Check.Empty(dr["Criteria"]))
                {
                    eve.IsEditable = false;
                }
                else
                {
                    eve.IsEditable = true;
                }
            };

            scaleCell.CellEditable += (s, eve) =>
            {
                DataRow dr = this.gridFGWT.GetDataRow(eve.RowIndex);

                if (dr["Scale"] != DBNull.Value)
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
            .ComboBox("Wash2", header: "Wash2", width: Widths.AnsiChars(10), settings: resultComboCell)
            .ComboBox("Wash3", header: "Wash3", width: Widths.AnsiChars(10), settings: resultComboCell)
            .ComboBox("Wash4", header: "Wash4", width: Widths.AnsiChars(10), settings: resultComboCell)
            .ComboBox("Wash5", header: "Wash5", width: Widths.AnsiChars(10), settings: resultComboCell)
            .Text("Comment", header: "Comment", width: Widths.AnsiChars(20), settings: textColumnSetting);

            this.Helper.Controls.Grid.Generator(this.gridFGWT)
            .Text("LocationText", header: "Location", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Text("Type", header: "Type", width: Widths.AnsiChars(40), iseditingreadonly: true)
            .Numeric("BeforeWash", header: "Before Wash", width: Widths.AnsiChars(6), decimal_places: 2, settings: beforeWash1)
            .Numeric("SizeSpec", header: "Size Spec Meas ", width: Widths.AnsiChars(6), decimal_places: 2, settings: sizeSpecCell)
            .Numeric("AfterWash", header: "After Wash", width: Widths.AnsiChars(6), decimal_places: 2, settings: afterWash1Cell4)
            .Numeric("Shrinkage", header: "Shrikage(%)", width: Widths.AnsiChars(6), iseditingreadonly: true, decimal_places: 2)
            .Text("Scale", header: "Scale", width: Widths.AnsiChars(10), settings: scaleCell)
            .Text("Result", header: "Result", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;

            this.Helper.Controls.Grid.Generator(this.gridFGPT)
            .Text("LocationText", header: "Location", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Text("Type", header: "Type", width: Widths.AnsiChars(70), iseditingreadonly: true)
            .Text("TestResult", header: "TestResult", width: Widths.AnsiChars(10), settings: threeTestCell)
            .ComboBox("TestUnit", header: "Test Detail", width: Widths.AnsiChars(10), iseditable: false, settings: mm_N_ComboCell)
            .Text("Result", header: "Result", width: Widths.AnsiChars(6), iseditingreadonly: true)
            ;

            this.Tab1Load();
            this.Tab2Load();
            this.Tab3Load();
            this.Tab4Load();
            this.Tab5Load();

            this.tabControl1.SelectedIndex = 1;
            this.tabControl1.SelectedIndex = 2;
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
            if (this.dtShrinkage.Select("Location = 'TOP'").Length == 0)
            {
                this.panel1.Visible = false;
                i--;
            }

            if (this.dtShrinkage.Select("Location = 'INNER'").Length == 0)
            {
                this.panel2.Visible = false;
                i--;
            }

            if (this.dtShrinkage.Select("Location = 'OUTER'").Length == 0)
            {
                this.panel3.Visible = false;
                i--;
            }

            if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length == 0)
            {
                this.panel4.Visible = false;
                i--;
            }

            this.flowLayoutPanel1.Height = 36 * i;

            this.numTopS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='T'"));
            this.numTopS2.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S2 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='T'"));
            this.numTopL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select sum(L) from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='T'"));
            this.numTwisTingTop.Value = this.numTopL.Value.Empty() ? 0 : (((this.numTopS1.Value + this.numTopS2.Value) / 2) / this.numTopL.Value) * 100;

            this.numInnerS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='I'"));
            this.numInnerS2.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S2 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='I'"));
            this.numInnerL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select sum(L) from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='I'"));
            this.numTwisTingInner.Value = this.numInnerL.Value.Empty() ? 0 : (((this.numInnerS1.Value + this.numInnerS2.Value) / 2) / this.numInnerL.Value) * 100;

            this.numOuterS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='O'"));
            this.numOuterS2.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S2 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='O'"));
            this.numOuterL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select sum(L) from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='O'"));
            this.numTwisTingOuter.Value = this.numOuterL.Value.Empty() ? 0 : (((this.numOuterS1.Value + this.numOuterS2.Value) / 2) / this.numOuterL.Value) * 100;

            this.numBottomS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='B'"));
            this.numBottomL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select L   from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='B'"));
            this.numTwisTingBottom.Value = this.numBottomL.Value.Empty() ? 0 : this.numBottomS1.Value / this.numBottomL.Value * 100;
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
        ,Type 
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
	        ,IIF( f.BeforeWash IS NOT NULL AND f.AfterWash IS NOT NULL AND f.Criteria IS NOT NULL AND f.Shrinkage IS NOT NULL
					,( IIF( TestDetail ='%'
								---- 百分比 判斷方式
								,IIF( ISNULL(f.Criteria,0) * -1 <= ISNULL(f.Shrinkage,0) AND ISNULL(f.Shrinkage,0) <= ISNULL(f.Criteria,0)
									, 'Pass'
									, 'Fail'
								)
								---- 非百分比 判斷方式
								,IIF( ISNULL(f.AfterWash,0) - ISNULL(f.BeforeWash,0) < ISNULL(f.Criteria,0)
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
from GarmentTest_Detail_FGWT f 
LEFT JOIN GarmentTest_Detail gd ON f.ID = gd.ID AND f.No = gd.NO
where f.id = {this.Deatilrow["ID"]} and f.No = {this.Deatilrow["No"]} 
order by LocationText DESC";

            DBProxy.Current.Select(null, sqlFGWT, out this.dtFGWT);
            this.gridFGWT.DataSource = this.dtFGWT;
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
        ,[Result]=	CASE WHEN  f.TestUnit = 'N' AND f.[TestResult] !='' THEN IIF( Cast( f.[TestResult] as decimal) >= f.Criteria ,'Pass' ,'Fail')
						 WHEN  f.TestUnit = 'mm' THEN IIF(  f.[TestResult] = '<=4' OR f.[TestResult] = '≦4','Pass' , IIF( f.[TestResult]='>4','Fail','')  )
						 WHEN  f.TestUnit = 'Pass/Fail'  THEN f.[TestResult]
					 	 ELSE ''
					END
from GarmentTest_Detail_FGPT f 
where f.id = {this.Deatilrow["ID"]} and f.No = {this.Deatilrow["No"]} 
order by LocationText DESC";

            DBProxy.Current.Select(null, sqlFGPT, out this.dtFGPT);
            this.gridFGPT.DataSource = this.dtFGPT;

            if (this.dtFGPT.Rows.Count > 0 && !this.EditMode)
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
  merge GarmentTest_Detail_FGWT t
  using #tmp s
  on s.id = t.id and s.no = t.no and s.Location = t.Location and s.Type	= t.Type
  when matched then
  update set
	t.[BeforeWash]  = s.[BeforeWash],
	t.[SizeSpec]  = s.[SizeSpec],
    t.[AfterWash]	= s.[AfterWash],
    t.[Shrinkage]	= s.Shrinkage,
    t.[Scale]	= s.[Scale]
	;

select [LocationText]= CASE WHEN Location='B' THEN 'Bottom'
						WHEN Location='T' THEN 'Top'
						WHEN Location='S' THEN 'Top+Bottom'
						ELSE ''
					END
		,Location
        ,Type 
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
	        ,IIF( f.BeforeWash IS NOT NULL AND f.AfterWash IS NOT NULL AND f.Criteria IS NOT NULL AND f.Shrinkage IS NOT NULL
					,( IIF( TestDetail ='%'
								---- 百分比 判斷方式
								,IIF( ISNULL(f.Criteria,0) * -1 <= ISNULL(f.Shrinkage,0) AND ISNULL(f.Shrinkage,0) <= ISNULL(f.Criteria,0)
									, 'Pass'
									, 'Fail'
								)
								---- 非百分比 判斷方式
								,IIF( ISNULL(f.AfterWash,0) - ISNULL(f.BeforeWash,0) < ISNULL(f.Criteria,0)
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
from GarmentTest_Detail_FGWT f 
LEFT JOIN GarmentTest_Detail gd ON f.ID = gd.ID AND f.No = gd.NO
where f.id = {this.Deatilrow["ID"]} and f.No = {this.Deatilrow["No"]} 
order by LocationText DESC

";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(gridFGWT, string.Empty, cmd, out this.dtFGWT);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridFGWT.DataSource = this.dtFGWT;
        }

        private void Tab5Save()
        {
            DataTable gridFGPT = (DataTable)this.gridFGPT.DataSource;

            string cmd = $@"
merge GarmentTest_Detail_FGPT t
using #tmp s
on s.id = t.id and s.no = t.no and s.Location = t.Location and s.Type	= t.Type
when matched then
update set
	t.[TestResult]  = s.[TestResult],
	t.[TestUnit]  = s.[TestUnit]
	;
delete t
from GarmentTest_Detail_FGPT t
left join #tmp s on s.id = t.id and s.no = t.no and s.Location = t.Location and s.Type	= t.Type
where s.id is null
and  t.id = {this.Deatilrow["ID"]} and t.No = {this.Deatilrow["No"]} 

select [LocationText]= CASE WHEN Location='B' THEN 'Bottom'
						WHEN Location='T' THEN 'Top'
						WHEN Location='S' THEN 'Top+Bottom'
						ELSE ''
					END
		,f.*
        ,[Result]=	CASE WHEN  f.TestUnit = 'N' AND f.[TestResult] !='' THEN IIF( Cast( f.[TestResult] as decimal) >= f.Criteria ,'Pass' ,'Fail')
						 WHEN  f.TestUnit = 'mm' THEN IIF(  f.[TestResult] = '<=4' OR f.[TestResult] = '≦4','Pass' , IIF( f.[TestResult]='>4','Fail','')  )
						 WHEN  f.TestUnit = 'Pass/Fail'  THEN f.[TestResult]
					 	 ELSE ''
					END
from GarmentTest_Detail_FGPT f 
where f.id = {this.Deatilrow["ID"]} and f.No = {this.Deatilrow["No"]} 
order by LocationText DESC

";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(gridFGPT, string.Empty, cmd, out this.dtFGPT);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridFGPT.DataSource = this.dtFGPT;
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
    Neck ='{MyUtility.Convert.GetString(this.comboNeck.Text).EqualString("YES")}'
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
                this.Tab2TwistingSave();
                DBProxy.Current.Execute(null, $"update Garmenttest_Detail set Editname = '{Env.User.UserID}',EditDate = getdate() where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}");
                this.Tab2Load();

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
            this.btnGenerateFGWT.Enabled = !this.EditMode;
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

        private void Tab2TwistingSave()
        {
            string savetab2Twisting = $@"
update [GarmentTest_Detail_Twisting] set S1={this.numTopS1.Value},S2={this.numTopS2.Value},L={this.numTopL.Value},Twisting={this.numTwisTingTop.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='T'
update [GarmentTest_Detail_Twisting] set S1={this.numInnerS1.Value},S2={this.numInnerS2.Value},L={this.numInnerL.Value},Twisting={this.numTwisTingInner.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='I'
update [GarmentTest_Detail_Twisting] set S1={this.numOuterS1.Value},S2={this.numOuterS2.Value},L={this.numOuterL.Value},Twisting={this.numTwisTingOuter.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='O'
update [GarmentTest_Detail_Twisting] set S1={this.numBottomS1.Value},L={this.numBottomL.Value},Twisting={this.numTwisTingBottom.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='B'
";
            DualResult result = DBProxy.Current.Execute(null, savetab2Twisting);
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

        private void BtnPDF_Click(object sender, EventArgs e)
        {
            if (this.dtApperance.Rows.Count == 0 || this.dtShrinkage.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P04_GarmentWash.xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            if (!MyUtility.Check.Empty(this.dateSubmit.Value))
            {
                worksheet.Cells[4, 4] = MyUtility.Convert.GetDate(this.dateSubmit.Value).Value.Year + "/" + MyUtility.Convert.GetDate(this.dateSubmit.Value).Value.Month + "/" + MyUtility.Convert.GetDate(this.dateSubmit.Value).Value.Day;
            }

            if (!MyUtility.Check.Empty(this.Deatilrow["inspdate"]))
            {
                worksheet.Cells[4, 7] = MyUtility.Convert.GetDate(this.Deatilrow["inspdate"]).Value.Year + "/" + MyUtility.Convert.GetDate(this.Deatilrow["inspdate"]).Value.Month + "/" + MyUtility.Convert.GetDate(this.Deatilrow["inspdate"]).Value.Day;
            }

            worksheet.Cells[4, 9] = MyUtility.Convert.GetString(this.MasterRow["OrderID"]);
            worksheet.Cells[4, 11] = MyUtility.Convert.GetString(this.MasterRow["BrandID"]);
            worksheet.Cells[6, 4] = MyUtility.Convert.GetString(this.MasterRow["StyleID"]);
            worksheet.Cells[7, 8] = MyUtility.GetValue.Lookup($"select CustPONo from Orders with(nolock) where id = '{this.MasterRow["OrderID"]}'");
            worksheet.Cells[7, 4] = MyUtility.Convert.GetString(this.MasterRow["Article"]);
            worksheet.Cells[6, 8] = MyUtility.GetValue.Lookup($"select StyleName from Style with(nolock) where id = '{this.MasterRow["Styleid"]}' and seasonid = '{this.MasterRow["seasonid"]}' and brandid = '{this.MasterRow["brandid"]}'");
            worksheet.Cells[8, 8] = MyUtility.Convert.GetDecimal(this.numArriveQty.Value);

            // if (!MyUtility.Check.Empty(Deatilrow["SendDate"]))
            //    worksheet.Cells[8, 4] = MyUtility.Convert.GetDate(Deatilrow["SendDate"]).Value.Year + "/" + MyUtility.Convert.GetDate(Deatilrow["SendDate"]).Value.Month + "/" + MyUtility.Convert.GetDate(Deatilrow["SendDate"]).Value.Day;
            string sendDate = Convert.ToDateTime(MyUtility.GetValue.Lookup($"SELECT BuyerDelivery FROM Orders WHERE ID = '{this.MasterRow["OrderID"].ToString()}'")).ToShortDateString();
            worksheet.Cells[8, 4] = sendDate;
            worksheet.Cells[8, 10] = MyUtility.Convert.GetString(this.txtSize.Text);

            worksheet.Cells[11, 4] = this.rdbtnLine.Checked ? "V" : string.Empty;
            worksheet.Cells[12, 4] = this.rdbtnTumble.Checked ? "V" : string.Empty;
            worksheet.Cells[13, 4] = this.rdbtnHand.Checked ? "V" : string.Empty;
            worksheet.Cells[11, 8] = this.comboTemperature.Text + "˚C ";
            worksheet.Cells[12, 8] = this.comboMachineModel.Text;
            worksheet.Cells[13, 8] = this.txtFibreComposition.Text;

            /*開始塞PDF，注意！！！！！！！！！！！！！！！！！！！！！！！！有新舊資料區分，最簡單的方式寫if else

            新舊資料差異：新資料沒有Seq = 9 ，只到8，所以新資料 69行不見，下面的往上推
             */

            #region 舊資料
            if (!this.IsNewData)
            {
                #region 最下面 Signature
                if (MyUtility.Convert.GetString(this.Deatilrow["Result"]).EqualString("P"))
                {
                    worksheet.Cells[73, 4] = "V";
                }
                else
                {
                    worksheet.Cells[73, 6] = "V";
                }

                #endregion

                #region 插入圖片與Technician名字
                string sql_cmd = $@"select p.name,[SignaturePic] = s.PicPath + t.SignaturePic
                                        from Technician t WITH (NOLOCK)
                                        inner join pass1 p WITH (NOLOCK) on t.ID = p.ID  
                                        outer apply (select PicPath from system) s 
                                        where t.ID = '{this.Deatilrow["inspector"]}'
                                        and t.GarmentTest = 1";

                if (MyUtility.Check.Seek(sql_cmd, out DataRow drTechnicianInfo))
                {
                    string technicianName = drTechnicianInfo["name"].ToString();
                    string picSource = drTechnicianInfo["SignaturePic"].ToString();

                    // Name
                    worksheet.Cells[74, 9] = technicianName;

                    // 插入圖檔
                    if (!MyUtility.Check.Empty(picSource))
                    {
                        if (File.Exists(picSource))
                        {
                            Microsoft.Office.Interop.Excel.Range cellPic = worksheet.Cells[72, 9];

                            worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellPic.Left, cellPic.Top, 100, 24);
                        }
                    }
                }
                else
                {
                    worksheet.Cells[74, 9] = MyUtility.Convert.GetString(this.Deatilrow["Showname"]);
                }

                #endregion

                #region After Wash Appearance Check list
                string tmpAR;

                worksheet.Cells[61, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Type"]);

                worksheet.get_Range("61:61", Type.Missing).Rows.AutoFit();

                // 大約21個字換行
                int widhthBase = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Type"]).Length / 20;

                worksheet.get_Range("61:61", Type.Missing).RowHeight = widhthBase == 0 ? 28 : 28 * widhthBase;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 5] = "V";
                }
                else
                {
                    worksheet.Cells[61, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 7] = "V";
                }
                else
                {
                    worksheet.Cells[61, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 9] = "V";
                }
                else
                {
                    worksheet.Cells[61, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 11] = "V";
                }
                else
                {
                    worksheet.Cells[61, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 13] = "V";
                }
                else
                {
                    worksheet.Cells[61, 12] = tmpAR;
                }

                string strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Comment"]);
                this.RowHeight(worksheet, 61, strComment);
                worksheet.Cells[61, 14] = strComment;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 5] = "V";
                }
                else
                {
                    worksheet.Cells[62, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 7] = "V";
                }
                else
                {
                    worksheet.Cells[62, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 9] = "V";
                }
                else
                {
                    worksheet.Cells[62, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 11] = "V";
                }
                else
                {
                    worksheet.Cells[62, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 13] = "V";
                }
                else
                {
                    worksheet.Cells[62, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["Comment"]);
                this.RowHeight(worksheet, 62, strComment);
                worksheet.Cells[62, 14] = strComment;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 5] = "V";
                }
                else
                {
                    worksheet.Cells[63, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 7] = "V";
                }
                else
                {
                    worksheet.Cells[63, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 9] = "V";
                }
                else
                {
                    worksheet.Cells[63, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 11] = "V";
                }
                else
                {
                    worksheet.Cells[63, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 13] = "V";
                }
                else
                {
                    worksheet.Cells[63, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["Comment"]);
                this.RowHeight(worksheet, 63, strComment);
                worksheet.Cells[63, 14] = strComment;

                worksheet.Cells[64, 3] = this.dtApperance.Select("seq=4")[0]["Type"].ToString(); // type;

                // 大約21個字換行
                int widhthBase2 = this.dtApperance.Select("seq=4")[0]["Type"].ToString().Length / 20;

                worksheet.get_Range("64:64", Type.Missing).RowHeight = widhthBase2 == 0 ? 28 : 28 * widhthBase2;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 5] = "V";
                }
                else
                {
                    worksheet.Cells[64, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 7] = "V";
                }
                else
                {
                    worksheet.Cells[64, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 9] = "V";
                }
                else
                {
                    worksheet.Cells[64, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 11] = "V";
                }
                else
                {
                    worksheet.Cells[64, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 13] = "V";
                }
                else
                {
                    worksheet.Cells[64, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["Comment"]);
                this.RowHeight(worksheet, 64, strComment);
                worksheet.Cells[64, 14] = strComment;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 5] = "V";
                }
                else
                {
                    worksheet.Cells[65, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 7] = "V";
                }
                else
                {
                    worksheet.Cells[65, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 9] = "V";
                }
                else
                {
                    worksheet.Cells[65, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 11] = "V";
                }
                else
                {
                    worksheet.Cells[65, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 13] = "V";
                }
                else
                {
                    worksheet.Cells[65, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["Comment"]);
                this.RowHeight(worksheet, 65, strComment);
                worksheet.Cells[65, 14] = strComment;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 5] = "V";
                }
                else
                {
                    worksheet.Cells[66, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 7] = "V";
                }
                else
                {
                    worksheet.Cells[66, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 9] = "V";
                }
                else
                {
                    worksheet.Cells[66, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 11] = "V";
                }
                else
                {
                    worksheet.Cells[66, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 13] = "V";
                }
                else
                {
                    worksheet.Cells[66, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["Comment"]);
                this.RowHeight(worksheet, 66, strComment);
                worksheet.Cells[66, 14] = strComment;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 5] = "V";
                }
                else
                {
                    worksheet.Cells[67, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 7] = "V";
                }
                else
                {
                    worksheet.Cells[67, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 9] = "V";
                }
                else
                {
                    worksheet.Cells[67, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 11] = "V";
                }
                else
                {
                    worksheet.Cells[67, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 13] = "V";
                }
                else
                {
                    worksheet.Cells[67, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["Comment"]);
                this.RowHeight(worksheet, 67, strComment);
                worksheet.Cells[67, 14] = strComment;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 5] = "V";
                }
                else
                {
                    worksheet.Cells[68, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 7] = "V";
                }
                else
                {
                    worksheet.Cells[68, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 9] = "V";
                }
                else
                {
                    worksheet.Cells[68, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 11] = "V";
                }
                else
                {
                    worksheet.Cells[68, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 13] = "V";
                }
                else
                {
                    worksheet.Cells[68, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["Comment"]);
                this.RowHeight(worksheet, 68, strComment);
                worksheet.Cells[68, 14] = strComment;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=9")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[69, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[69, 5] = "V";
                }
                else
                {
                    worksheet.Cells[69, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=9")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[69, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[69, 7] = "V";
                }
                else
                {
                    worksheet.Cells[69, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=9")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[69, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[69, 9] = "V";
                }
                else
                {
                    worksheet.Cells[69, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=9")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[69, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[69, 11] = "V";
                }
                else
                {
                    worksheet.Cells[69, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=9")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[69, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[69, 13] = "V";
                }
                else
                {
                    worksheet.Cells[69, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=9")[0]["Comment"]);
                this.RowHeight(worksheet, 69, strComment);
                worksheet.Cells[69, 14] = strComment;
                #endregion

                if (this.comboNeck.Text.EqualString("Yes"))
                {
                    worksheet.Cells[40, 9] = "V";
                }
                else
                {
                    worksheet.Cells[40, 11] = "V";
                }

                #region %
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    worksheet.Cells[56, 4] = this.numTwisTingBottom.Text + "%";
                    worksheet.Cells[56, 7] = this.numBottomS1.Value;
                    worksheet.Cells[56, 9] = this.numBottomL.Value;
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A56:A57", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'OUTER'").Length > 0)
                {
                    worksheet.Cells[54, 4] = this.numTwisTingOuter.Text + "%";
                    worksheet.Cells[54, 7] = this.numOuterS1.Value;
                    worksheet.Cells[54, 9] = this.numOuterS2.Value;
                    worksheet.Cells[54, 11] = this.numOuterL.Value;
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A54:A55", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'INNER'").Length > 0)
                {
                    worksheet.Cells[52, 4] = this.numTwisTingInner.Text + "%";
                    worksheet.Cells[52, 7] = this.numInnerS1.Value;
                    worksheet.Cells[52, 9] = this.numInnerS2.Value;
                    worksheet.Cells[52, 11] = this.numInnerL.Value;
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A52:A53", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'TOP'").Length > 0)
                {
                    worksheet.Cells[50, 4] = this.numTwisTingTop.Text + "%";
                    worksheet.Cells[50, 7] = this.numTopS1.Value;
                    worksheet.Cells[50, 9] = this.numTopS2.Value;
                    worksheet.Cells[50, 11] = this.numTopL.Value;
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A50:A51", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }
                #endregion

                #region Shrinkage
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[44, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Waistband (relax)'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[45, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Hip Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[46, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Thigh Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[47, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Side Seam'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[48, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Leg Opening'", i);
                    }
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A42:A49", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'OUTER'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[34, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[35, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[36, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[37, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[38, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Hem Opening'", i);
                    }
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A32:A39", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'INNER'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[26, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[27, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[28, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[29, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[30, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Hem Opening'", i);
                    }
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A24:A31", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'TOP'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[18, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[19, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[20, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[21, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[22, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Hem Opening'", i);
                    }
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A16:A23", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }
                #endregion

            }
            #endregion

            #region 新資料
            if (this.IsNewData)
            {
                worksheet.get_Range("62:62", Type.Missing).Delete();

                #region 最下面 Signature
                if (MyUtility.Convert.GetString(this.Deatilrow["Result"]).EqualString("P"))
                {
                    worksheet.Cells[72, 4] = "V";
                }
                else
                {
                    worksheet.Cells[72, 6] = "V";
                }
                #endregion

                #region 插入圖片與Technician名字
                string sql_cmd = $@"select p.name,[SignaturePic] = s.PicPath + t.SignaturePic
                                        from Technician t WITH (NOLOCK)
                                        inner join pass1 p WITH (NOLOCK) on t.ID = p.ID  
                                        outer apply (select PicPath from system) s 
                                        where t.ID = '{this.Deatilrow["inspector"]}'
                                        and t.GarmentTest=1
";

                if (MyUtility.Check.Seek(sql_cmd, out DataRow drTechnicianInfo))
                {
                    string technicianName = drTechnicianInfo["name"].ToString();
                    string picSource = drTechnicianInfo["SignaturePic"].ToString();

                    // Name
                    worksheet.Cells[74, 9] = technicianName;

                    // 插入圖檔
                    if (!MyUtility.Check.Empty(picSource))
                    {
                        if (File.Exists(picSource))
                        {
                            Microsoft.Office.Interop.Excel.Range cellPic = worksheet.Cells[72, 9];

                            worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellPic.Left, cellPic.Top, 100, 24);
                        }
                    }
                }
                else
                {
                    worksheet.Cells[74, 9] = MyUtility.Convert.GetString(this.Deatilrow["Showname"]);
                }

                #endregion

                #region After Wash Appearance Check list
                string tmpAR;

                worksheet.Cells[61, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Type"]).ToString();
                worksheet.get_Range("61:61", Type.Missing).Rows.AutoFit();

                // 大約21個字換行
                int widhthBase = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Type"]).ToString().Length / 20;

                worksheet.get_Range("61:61", Type.Missing).RowHeight = widhthBase == 0 ? 28 : 28 * widhthBase;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 5] = "V";
                }
                else
                {
                    worksheet.Cells[61, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 7] = "V";
                }
                else
                {
                    worksheet.Cells[61, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 9] = "V";
                }
                else
                {
                    worksheet.Cells[61, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 11] = "V";
                }
                else
                {
                    worksheet.Cells[61, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[61, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[61, 13] = "V";
                }
                else
                {
                    worksheet.Cells[61, 12] = tmpAR;
                }

                string strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Comment"]);
                this.RowHeight(worksheet, 61, strComment);
                worksheet.Cells[61, 14] = strComment;

                worksheet.Cells[62, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["Type"]);

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 5] = "V";
                }
                else
                {
                    worksheet.Cells[62, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 7] = "V";
                }
                else
                {
                    worksheet.Cells[62, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 9] = "V";
                }
                else
                {
                    worksheet.Cells[62, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 11] = "V";
                }
                else
                {
                    worksheet.Cells[62, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[62, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[62, 13] = "V";
                }
                else
                {
                    worksheet.Cells[62, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["Comment"]);
                this.RowHeight(worksheet, 62, strComment);
                worksheet.Cells[62, 14] = strComment;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash1"]);

                worksheet.Cells[63, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["Type"]).ToString(); // type;

                // 大約21個字換行
                int widhthBase2 = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["Type"]).ToString().Length / 20;

                worksheet.get_Range("63:63", Type.Missing).RowHeight = widhthBase2 == 0 ? 28 : 28 * widhthBase2;

                if ((
                        worksheet.get_Range("61:61", Type.Missing).RowHeight
                        + worksheet.get_Range("62:62", Type.Missing).RowHeight
                        + worksheet.get_Range("63:63", Type.Missing).RowHeight) < 81)
                {
                    worksheet.get_Range("61:61", Type.Missing).RowHeight = worksheet.get_Range("61:61", Type.Missing).RowHeight > 28 ? worksheet.get_Range("61:61", Type.Missing).RowHeight : 28;
                    worksheet.get_Range("62:62", Type.Missing).RowHeight = 28;
                    worksheet.get_Range("63:63", Type.Missing).RowHeight = worksheet.get_Range("63:63", Type.Missing).RowHeight > 28 ? worksheet.get_Range("63:63", Type.Missing).RowHeight : 28;
                }

                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 5] = "V";
                }
                else
                {
                    worksheet.Cells[63, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 7] = "V";
                }
                else
                {
                    worksheet.Cells[63, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 9] = "V";
                }
                else
                {
                    worksheet.Cells[63, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 11] = "V";
                }
                else
                {
                    worksheet.Cells[63, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[63, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[63, 13] = "V";
                }
                else
                {
                    worksheet.Cells[63, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["Comment"]);
                this.RowHeight(worksheet, 63, strComment);
                worksheet.Cells[63, 14] = strComment;

                worksheet.Cells[64, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["Type"]);
                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 5] = "V";
                }
                else
                {
                    worksheet.Cells[64, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 7] = "V";
                }
                else
                {
                    worksheet.Cells[64, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 9] = "V";
                }
                else
                {
                    worksheet.Cells[64, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 11] = "V";
                }
                else
                {
                    worksheet.Cells[64, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[64, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[64, 13] = "V";
                }
                else
                {
                    worksheet.Cells[64, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["Comment"]);
                this.RowHeight(worksheet, 64, strComment);
                worksheet.Cells[64, 14] = strComment;

                worksheet.Cells[65, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["Type"]);

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 5] = "V";
                }
                else
                {
                    worksheet.Cells[65, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 7] = "V";
                }
                else
                {
                    worksheet.Cells[65, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 9] = "V";
                }
                else
                {
                    worksheet.Cells[65, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 11] = "V";
                }
                else
                {
                    worksheet.Cells[65, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[65, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[65, 13] = "V";
                }
                else
                {
                    worksheet.Cells[65, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["Comment"]);
                this.RowHeight(worksheet, 65, strComment);
                worksheet.Cells[65, 14] = strComment;

                worksheet.Cells[66, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["Type"]);

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 5] = "V";
                }
                else
                {
                    worksheet.Cells[66, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 7] = "V";
                }
                else
                {
                    worksheet.Cells[66, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 9] = "V";
                }
                else
                {
                    worksheet.Cells[66, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 11] = "V";
                }
                else
                {
                    worksheet.Cells[66, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[66, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[66, 13] = "V";
                }
                else
                {
                    worksheet.Cells[66, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["Comment"]);
                this.RowHeight(worksheet, 66, strComment);
                worksheet.Cells[66, 14] = strComment;

                worksheet.Cells[67, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["Type"]);

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 5] = "V";
                }
                else
                {
                    worksheet.Cells[67, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 7] = "V";
                }
                else
                {
                    worksheet.Cells[67, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 9] = "V";
                }
                else
                {
                    worksheet.Cells[67, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 11] = "V";
                }
                else
                {
                    worksheet.Cells[67, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[67, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[67, 13] = "V";
                }
                else
                {
                    worksheet.Cells[67, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["Comment"]);
                this.RowHeight(worksheet, 67, strComment);
                worksheet.Cells[67, 14] = strComment;

                worksheet.Cells[68, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["Type"]);

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash1"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 4] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 5] = "V";
                }
                else
                {
                    worksheet.Cells[68, 4] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash2"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 6] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 7] = "V";
                }
                else
                {
                    worksheet.Cells[68, 6] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash3"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 8] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 9] = "V";
                }
                else
                {
                    worksheet.Cells[68, 8] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash4"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 10] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 11] = "V";
                }
                else
                {
                    worksheet.Cells[68, 10] = tmpAR;
                }

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["wash5"]);
                if (tmpAR.EqualString("Accepted"))
                {
                    worksheet.Cells[68, 12] = "V";
                }
                else if (tmpAR.EqualString("Rejected"))
                {
                    worksheet.Cells[68, 13] = "V";
                }
                else
                {
                    worksheet.Cells[68, 12] = tmpAR;
                }

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["Comment"]);
                this.RowHeight(worksheet, 68, strComment);
                worksheet.Cells[68, 14] = strComment;

                #endregion

                if (this.comboNeck.Text.EqualString("Yes"))
                {
                    worksheet.Cells[40, 9] = "V";
                }
                else
                {
                    worksheet.Cells[40, 11] = "V";
                }

                #region %
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    worksheet.Cells[56, 4] = this.numTwisTingBottom.Text + "%";
                    worksheet.Cells[56, 7] = this.numBottomS1.Value;
                    worksheet.Cells[56, 9] = this.numBottomL.Value;
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A56:A57", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'OUTER'").Length > 0)
                {
                    worksheet.Cells[54, 4] = this.numTwisTingOuter.Text + "%";
                    worksheet.Cells[54, 7] = this.numOuterS1.Value;
                    worksheet.Cells[54, 9] = this.numOuterS2.Value;
                    worksheet.Cells[54, 11] = this.numOuterL.Value;
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A54:A55", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'INNER'").Length > 0)
                {
                    worksheet.Cells[52, 4] = this.numTwisTingInner.Text + "%";
                    worksheet.Cells[52, 7] = this.numInnerS1.Value;
                    worksheet.Cells[52, 9] = this.numInnerS2.Value;
                    worksheet.Cells[52, 11] = this.numInnerL.Value;
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A52:A53", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'TOP'").Length > 0)
                {
                    worksheet.Cells[50, 4] = this.numTwisTingTop.Text + "%";
                    worksheet.Cells[50, 7] = this.numTopS1.Value;
                    worksheet.Cells[50, 9] = this.numTopS2.Value;
                    worksheet.Cells[50, 11] = this.numTopL.Value;
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A50:A51", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }
                #endregion

                #region Shrinkage
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[44, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Waistband (relax)'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[45, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Hip Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[46, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Thigh Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[47, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Side Seam'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[48, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Leg Opening'", i);
                    }
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A42:A49", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'OUTER'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[34, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[35, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[36, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[37, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[38, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Hem Opening'", i);
                    }
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A32:A39", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'INNER'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[26, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[27, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[28, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[29, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[30, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Hem Opening'", i);
                    }
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A24:A31", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                if (this.dtShrinkage.Select("Location = 'TOP'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[18, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[19, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[20, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[21, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[22, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Hem Opening'", i);
                    }
                }
                else
                {
                    Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A16:A23", Type.Missing).EntireRow;
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }
                #endregion
            }
            #endregion

            #region Save & Show Excel
            string strFileName = Class.MicrosoftFile.GetName("Quality_P04_GarmentWash");
            #endregion
            #region Save & Show Excel

            #endregion
            #region Save & Show Excel
            string strPDFFileName = Class.MicrosoftFile.GetName("Quality_P04_GarmentWash", Class.PDFFileNameExtension.PDF);
            objApp.ActiveWorkbook.SaveAs(strFileName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            #endregion

            if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
                Process.Start(startInfo);
            }
        }

        private void RowHeight(Microsoft.Office.Interop.Excel.Worksheet worksheet, int row, string strComment)
        {
            if (strComment.Length > 15)
            {
                decimal n = Math.Ceiling(strComment.Length / (decimal)15.0) * (decimal)12.25;
                worksheet.Range[$"A{row}", $"A{row}"].RowHeight = n;
            }
        }

        /// <summary>
        /// 如果欄位是Shrinkage 就增加%單位符號
        /// </summary>
        /// <inheritdoc/>
        private string AddShrinkageUnit(DataTable dt, string strFilter, int count)
        {
            string strValie = string.Empty;
            if (dt.Select(strFilter).Length > 0)
            {
                strValie = dt.Select(strFilter)[0][count].ToString();
                if (((string.Compare(dt.Columns[count].ColumnName, "Shrinkage1", true) == 0) ||
                    (string.Compare(dt.Columns[count].ColumnName, "Shrinkage2", true) == 0) ||
                    (string.Compare(dt.Columns[count].ColumnName, "Shrinkage3", true) == 0)) &&
                    !MyUtility.Check.Empty(strValie))
                {
                    strValie += "%";
                }
            }

            return strValie;
        }

        private void BtnToFGWT_Click(object sender, EventArgs e)
        {
            if (this.dtFGWT.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P04_FGWT.xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            // objApp.Visible = true;

            // 若為QA 10產生則顯示New Development Testing ( V )，若為QA P04產生則顯示1st Bulk Testing ( V )
            worksheet.Cells[4, 3] = "1st Bulk Testing ( V )";

            worksheet.Cells[5, 1] = "adidas Article No.: " + MyUtility.Convert.GetString(this.MasterRow["Article"]);
            worksheet.Cells[5, 3] = "adidas Working No.: " + MyUtility.Convert.GetString(this.MasterRow["StyleID"]);
            worksheet.Cells[5, 4] = "adidas Model No.: " + MyUtility.GetValue.Lookup($"SELECT StyleName FROM Style WHERE ID='{this.MasterRow["StyleID"]}'");

            worksheet.Cells[6, 1] = "T1 Supplier Ref.: " + MyUtility.GetValue.Lookup($"SELECT FactoryID FROM Orders WHERE ID='{this.MasterRow["OrderID"]}'");
            worksheet.Cells[6, 3] = "T1 Factory Name: " + MyUtility.GetValue.Lookup($"SELECT o.BrandAreaCode FROM GarmentTest g INNER JOIN Orders o ON g.OrderID = o.ID WHERE g.OrderID='{this.MasterRow["OrderID"]}'");
            worksheet.Cells[6, 4] = "LO to Factory: " + this.txtLotoFactory.Text;

            if (this.dateSubmit.Value.HasValue)
            {
                worksheet.Cells[8, 1] = "Date: " + this.dateSubmit.Value.Value.ToString("yyyy/MM/dd");
            }

            int copyCount = this.dtFGWT.Rows.Count - 2;

            for (int i = 0; i <= copyCount - 1; i++)
            {
                // 複製儲存格
                Microsoft.Office.Interop.Excel.Range rgCopy = worksheet.get_Range("A13:A13").EntireRow;

                // 選擇要被貼上的位置
                Microsoft.Office.Interop.Excel.Range rgPaste = worksheet.get_Range("A13:A13", Type.Missing);

                // 貼上
                rgPaste.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rgCopy.Copy(Type.Missing));
            }

            worksheet.get_Range($"B12", $"B{this.dtFGWT.Rows.Count + 11}").Merge(false);

            int startRowIndex = 12;

            // 開始填入表身
            foreach (DataRow dr in this.dtFGWT.Rows)
            {
                // Requirement
                worksheet.Cells[startRowIndex, 3] = MyUtility.Convert.GetString(dr["Type"]);

                // Test Results
                // 若[GarmentTest_Detail_FGWT.Scale]非null則帶入Scale，若為null則帶入 [GarmentTest_Detail_FGWT.AfterWash - GarmentTest_Detail_FGWT.BeforeWash.]
                if (dr["Scale"] != DBNull.Value)
                {
                    worksheet.Cells[startRowIndex, 4] = MyUtility.Convert.GetString(dr["Scale"]);
                }
                else
                {
                    if (dr["BeforeWash"] != DBNull.Value && dr["AfterWash"] != DBNull.Value && dr["Shrinkage"] != DBNull.Value)
                    {
                        if (MyUtility.Convert.GetString(dr["TestDetail"]) == "%")
                        {
                            worksheet.Cells[startRowIndex, 4] = MyUtility.Convert.GetDouble(dr["Shrinkage"]);
                        }
                        else
                        {
                            worksheet.Cells[startRowIndex, 4] = MyUtility.Convert.GetDouble(dr["AfterWash"]) - MyUtility.Convert.GetDouble(dr["BeforeWash"]);
                        }
                    }
                }

                // Test Details
                worksheet.Cells[startRowIndex, 5] = MyUtility.Convert.GetString(dr["TestDetail"]);

                // adidas pass
                worksheet.Cells[startRowIndex, 6] = MyUtility.Convert.GetString(dr["Result"]);

                startRowIndex++;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("QA_P04_FGWT");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            strExcelName.OpenFile();
            #endregion
        }

        private void BtnToFGPT_Click(object sender, EventArgs e)
        {
            if (this.dtFGPT.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P04_FGPT.xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            // objApp.Visible = true;

            // 若為QA 10產生則顯示New Development Testing ( V )，若為QA P04產生則顯示1st Bulk Testing ( V )
            worksheet.Cells[4, 3] = "1st Bulk Testing ( V )";

            worksheet.Cells[5, 1] = "adidas Article No.: " + MyUtility.Convert.GetString(this.MasterRow["Article"]);
            worksheet.Cells[5, 3] = "adidas Working No.: " + MyUtility.Convert.GetString(this.MasterRow["StyleID"]);
            worksheet.Cells[5, 4] = "adidas Model No.: " + MyUtility.GetValue.Lookup($"SELECT StyleName FROM Style WHERE ID='{this.MasterRow["StyleID"]}'");

            worksheet.Cells[6, 1] = "T1 Supplier Ref.: " + MyUtility.GetValue.Lookup($"SELECT FactoryID FROM Orders WHERE ID='{this.MasterRow["OrderID"]}'");
            worksheet.Cells[6, 3] = "T1 Factory Name: " + MyUtility.GetValue.Lookup($"SELECT o.BrandAreaCode FROM GarmentTest g INNER JOIN Orders o ON g.OrderID = o.ID WHERE g.OrderID='{this.MasterRow["OrderID"]}'");
            worksheet.Cells[6, 4] = "LO to Factory: " + this.txtLotoFactory.Text;

            if (this.dateSubmit.Value.HasValue)
            {
                worksheet.Cells[8, 1] = "Date: " + this.dateSubmit.Value.Value.ToString("yyyy/MM/dd");
            }

            var testName_1 = this.dtFGPT.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["TestName"]) == "PHX-AP0413");
            var testName_2 = this.dtFGPT.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["TestName"]) == "PHX-AP0450");
            var testName_3 = this.dtFGPT.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["TestName"]) == "PHX-AP0451");

            #region 儲存格處理

            // 因為PHX-AP0451在最下面，且只會有一筆，因此先複製這個，不然要重算Row index

            // PHX-AP0451

            // Requirement
            worksheet.Cells[150, 3] = MyUtility.Convert.GetString(testName_3.FirstOrDefault()["Type"]);

            // Test Results
            worksheet.Cells[150, 4] = MyUtility.Convert.GetString(testName_3.FirstOrDefault()["TestResult"]);

            // Test Details
            worksheet.Cells[150, 5] = MyUtility.Convert.GetString(testName_3.FirstOrDefault()["TestDetail"]);

            // adidas pass
            worksheet.Cells[150, 6] = MyUtility.Convert.GetString(testName_3.FirstOrDefault()["Result"]);

            // PHX-AP0450
            int copyCount_2 = testName_2.Count() - 2;

            for (int i = 0; i <= copyCount_2 - 1; i++)
            {
                // 複製儲存格
                Microsoft.Office.Interop.Excel.Range rgCopy = worksheet.get_Range("A149:A149").EntireRow;

                // 選擇要被貼上的位置
                Microsoft.Office.Interop.Excel.Range rgPaste = worksheet.get_Range("A149:A149", Type.Missing);

                // 貼上
                rgPaste.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rgCopy.Copy(Type.Missing));
            }

            worksheet.get_Range($"B148", $"B{copyCount_2 + 149}").Merge(false);

            // PHX - AP0413
            int copyCount_1 = testName_1.Count() - 2;

            for (int i = 0; i <= copyCount_1 - 1; i++)
            {
                // 複製儲存格
                Microsoft.Office.Interop.Excel.Range rgCopy = worksheet.get_Range("A135:A135").EntireRow;

                // 選擇要被貼上的位置
                Microsoft.Office.Interop.Excel.Range rgPaste = worksheet.get_Range("A135:A135", Type.Missing);

                // 貼上
                rgPaste.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rgCopy.Copy(Type.Missing));
            }

            worksheet.get_Range($"B134", $"B{copyCount_1 + 135}").Merge(false);

            #endregion

            // 開始填入表身，先填PHX - AP0413
            int startRowIndex = 134;
            foreach (DataRow dr in testName_1)
            {
                // Requirement
                worksheet.Cells[startRowIndex, 3] = MyUtility.Convert.GetString(dr["Type"]);

                // Test Results
                worksheet.Cells[startRowIndex, 4] = MyUtility.Convert.GetString(dr["TestResult"]);

                // Test Details
                worksheet.Cells[startRowIndex, 5] = MyUtility.Convert.GetString(dr["TestDetail"]);

                // adidas pass
                worksheet.Cells[startRowIndex, 6] = MyUtility.Convert.GetString(dr["Result"]);

                startRowIndex++;
            }

            // 開始填入表身，填PHX - AP0450
            startRowIndex = testName_1.Count() + 133 + 12 + 1;
            /*說明PHX - AP0413 這個Test Name最後的Index 為copyCount_1 + 133,與PHX-AP0450起點Index中間差了12 Row*/

            foreach (DataRow dr in testName_2)
            {
                // Requirement
                worksheet.Cells[startRowIndex, 3] = MyUtility.Convert.GetString(dr["Type"]);

                // Test Results
                worksheet.Cells[startRowIndex, 4] = MyUtility.Convert.GetString(dr["TestResult"]);

                // Test Details
                worksheet.Cells[startRowIndex, 5] = MyUtility.Convert.GetString(dr["TestDetail"]);

                // adidas pass
                worksheet.Cells[startRowIndex, 6] = MyUtility.Convert.GetString(dr["Result"]);

                startRowIndex++;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("QA_P04_FGPT");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            strExcelName.OpenFile();
            #endregion
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
                NumTwisTingBottom = this.numTwisTingBottom.Text,
                NumBottomS1 = this.numTwisTingBottom.Value,
                NumBottomL = this.numTwisTingBottom.Value,
                NumTwisTingOuter = this.numTwisTingOuter.Text,
                NumOuterS1 = this.numOuterS1.Value,
                NumOuterS2 = this.numOuterS2.Value,
                NumOuterL = this.numOuterL.Value,
                NumTwisTingInner = this.numTwisTingInner.Text,
                NumInnerS1 = this.numInnerS1.Value,
                NumInnerS2 = this.numInnerS2.Value,
                NumInnerL = this.numTwisTingBottom.Value,
                NumTwisTingTop = this.numTwisTingTop.Text,
                NumTopS1 = this.numTopS1.Value,
                NumTopS2 = this.numTopS2.Value,
                NumTopL = this.numTopL.Value,
                TxtLotoFactory = this.txtLotoFactory.Text,
            };

            P04_ToReport form = new P04_ToReport(this.MasterRow, this.Deatilrow, this.IsNewData, this.dtApperance, this.dtShrinkage, this.dtFGWT, this.dtFGPT, p04Data);
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
            if (this.rdbtnLine.Checked)
            {

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
            if (containsT && containsB)
            {
                fGWTs = GetDefaultFGWT(false, false, true, mtlTypeID);
            }
            else if (containsT)
            {
                fGWTs = GetDefaultFGWT(containsT, false, false, mtlTypeID);
            }
            else
            {
                fGWTs = GetDefaultFGWT(false, containsB, false, mtlTypeID);
            }


        }
    }
}
