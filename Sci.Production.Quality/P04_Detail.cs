using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P04_Detail : Win.Forms.Base
    {
        private DataRow Deatilrow;
        private DataRow MasterRow;
        private bool IsNewData = true;

        public P04_Detail(bool editmode, DataRow masterrow, DataRow deatilrow)
        {
            this.InitializeComponent();
            this.MasterRow = masterrow;
            this.Deatilrow = deatilrow;
            this.EditMode = editmode;
        }

        private void P04_Detail_Load(object sender, EventArgs e)
        {
            this.btnenable();

            this.comboTemperature.SelectedIndex = 0;
            this.comboMachineModel.SelectedIndex = 0;
            this.comboNeck.SelectedIndex = 0;

            DataGridViewGeneratorNumericColumnSettings BeforeWash = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings AfterWash1Cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings AfterWash1Cell2 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings AfterWash1Cell3 = new DataGridViewGeneratorNumericColumnSettings();

            #region 避免除數為0的檢查
            BeforeWash.CellValidating += (s, eve) =>
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
                bool IsAllEmpty = MyUtility.Check.Empty(dr["AfterWash1"]) && MyUtility.Check.Empty(dr["AfterWash2"]) && MyUtility.Check.Empty(dr["AfterWash3"]);

                if (MyUtility.Check.Empty(eve.FormattedValue) && !IsAllEmpty)
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
                    double BeforeWashNum = Convert.ToDouble(eve.FormattedValue);

                    dr["BeforeWash"] = eve.FormattedValue;

                    if (!MyUtility.Check.Empty(dr["AfterWash1"]))
                    {
                        double AfterWash1Num = Convert.ToDouble(dr["AfterWash1"]);
                        dr["Shrinkage1"] = (AfterWash1Num - BeforeWashNum) / BeforeWashNum * 100;
                    }

                    if (!MyUtility.Check.Empty(dr["AfterWash2"]))
                    {
                        double AfterWash2Num = Convert.ToDouble(dr["AfterWash2"]);
                        dr["Shrinkage2"] = (AfterWash2Num - BeforeWashNum) / BeforeWashNum * 100;
                    }

                    if (!MyUtility.Check.Empty(dr["AfterWash3"]))
                    {
                        double AfterWash3Num = Convert.ToDouble(dr["AfterWash3"]);
                        dr["Shrinkage3"] = (AfterWash3Num - BeforeWashNum) / BeforeWashNum * 100;
                    }
                }
            };

            AfterWash1Cell.CellValidating += (s, eve) =>
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
                    double BeforeWashNum = Convert.ToDouble(dr["BeforeWash"]);
                    double AfterWash1Num = Convert.ToDouble(eve.FormattedValue);
                    dr["AfterWash1"] = eve.FormattedValue;
                    dr["Shrinkage1"] = (AfterWash1Num - BeforeWashNum) / BeforeWashNum * 100;
                }
            };

            AfterWash1Cell2.CellValidating += (s, eve) =>
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
                    double BeforeWashNum = Convert.ToDouble(dr["BeforeWash"]);
                    double AfterWash2Num = Convert.ToDouble(eve.FormattedValue);
                    dr["AfterWash2"] = eve.FormattedValue;
                    dr["Shrinkage2"] = (AfterWash2Num - BeforeWashNum) / BeforeWashNum * 100;
                }
            };

            AfterWash1Cell3.CellValidating += (s, eve) =>
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
                    double BeforeWashNum = Convert.ToDouble(dr["BeforeWash"]);
                    double AfterWash3Num = Convert.ToDouble(eve.FormattedValue);
                    dr["AfterWash3"] = eve.FormattedValue;
                    dr["Shrinkage3"] = (AfterWash3Num - BeforeWashNum) / BeforeWashNum * 100;
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.gridActualShrinkage)
            .Text("Location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Type", header: "Type", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Numeric("BeforeWash", header: "Before Wash", width: Widths.AnsiChars(6), decimal_places: 2, settings: BeforeWash)
            .Numeric("SizeSpec", header: "Size Spec Meas.", width: Widths.AnsiChars(8), decimal_places: 2)
            .Numeric("AfterWash1", header: "After Wash 1", width: Widths.AnsiChars(6), decimal_places: 2, settings: AfterWash1Cell)
            .Numeric("Shrinkage1", header: "Shrinkage 1(%)", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999, iseditable: false)
            .Numeric("AfterWash2", header: "After Wash 2", width: Widths.AnsiChars(6), decimal_places: 2, settings: AfterWash1Cell2)
            .Numeric("Shrinkage2", header: "Shrinkage 2(%)", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999, iseditable: false)
            .Numeric("AfterWash3", header: "After Wash 3", width: Widths.AnsiChars(6), decimal_places: 2, settings: AfterWash1Cell3)
            .Numeric("Shrinkage3", header: "Shrinkage 3(%)", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999, iseditable: false);

            Dictionary<string, string> ResultPF = new Dictionary<string, string>();
            ResultPF.Add("P", "Pass");
            ResultPF.Add("F", "Fail");
            this.comboResult.DataSource = new BindingSource(ResultPF, null);
            this.comboResult.ValueMember = "Key";
            this.comboResult.DisplayMember = "Value";

            DataGridViewGeneratorComboBoxColumnSettings ResultComboCell = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings ArtworkCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings TextColumnSetting = new DataGridViewGeneratorTextColumnSettings();

            ArtworkCell.CellMouseClick += (s, eve) =>
            {
                if (!this.IsNewData)
                { if (eve.RowIndex == -1 || (eve.RowIndex != 0 && eve.RowIndex != 3))
                    {
                        return;
                    }
                }
                else
                { if (eve.RowIndex == -1 || (eve.RowIndex != 0 && eve.RowIndex != 2))
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
                    string[] SelectedNames = dr["Type"].ToString().Replace("/", ",").Split(',');
                    List<string> tmpList = new List<string>();

                    foreach (var Name in SelectedNames)
                    {
                        string ID = MyUtility.GetValue.Lookup($"select TOP 1 ID  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' AND Name LIKE '{Name.Trim()}%' ");
                        tmpList.Add(ID);
                    }

                    defaultSelected = tmpList.JoinToString(",");

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sql, "ID,Name", "ID,Name", defaultSelected, null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string insertString = string.Empty;
                    string[] SelectedIDs = item.GetSelectedString().ToString().Split(',');
                    List<string> tmpList2 = new List<string>();

                    foreach (var ID in SelectedIDs)
                    {
                        string Name = MyUtility.GetValue.Lookup($"select TOP 1 Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' AND ID = '{ID.Trim()}' ");
                        tmpList2.Add(Name);
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
                    string[] SelectedNames = dr["Type"].ToString().Replace("/", ",").Split(',');
                    List<string> tmpList = new List<string>();

                    foreach (var Name in SelectedNames)
                    {
                        string ID = MyUtility.GetValue.Lookup($"select TOP 1 ID  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' AND Name = '{Name.Trim()}' ");
                        tmpList.Add(ID);
                    }

                    defaultSelected = tmpList.JoinToString(",");

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sql, "ID,Name", "ID,Name", defaultSelected.ToString(), null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string insertString = string.Empty;
                    string[] SelectedIDs = item.GetSelectedString().ToString().Split(',');
                    List<string> tmpList2 = new List<string>();

                    foreach (var ID in SelectedIDs)
                    {
                        string Name = MyUtility.GetValue.Lookup($"select TOP 1 Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' AND ID = '{ID.Trim()}' ");
                        tmpList2.Add(Name);
                    }

                    insertString = tmpList2.JoinToString(" / ");

                    dr["Type"] = insertString;

                    dr.EndEdit();
                }
            };

            ArtworkCell.EditingMouseDown += (s, eve) =>
            {
                if (!this.IsNewData)
                { if (eve.RowIndex == -1 || (eve.RowIndex != 0 && eve.RowIndex != 3))
                    {
                        return;
                    }
                }
                else
                { if (eve.RowIndex == -1 || (eve.RowIndex != 0 && eve.RowIndex != 2))
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
                    string[] SelectedNames = dr["Type"].ToString().Replace("/", ",").Split(',');
                    List<string> tmpList = new List<string>();

                    foreach (var Name in SelectedNames)
                    {
                        string ID = MyUtility.GetValue.Lookup($"select TOP 1 ID  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' AND Name LIKE '{Name.Trim()}%' ");
                        tmpList.Add(ID);
                    }

                    defaultSelected = tmpList.JoinToString(",");

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sql, "ID,Name", "ID,Name", defaultSelected, null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string insertString = string.Empty;
                    string[] SelectedIDs = item.GetSelectedString().ToString().Split(',');
                    List<string> tmpList2 = new List<string>();

                    foreach (var ID in SelectedIDs)
                    {
                        string Name = MyUtility.GetValue.Lookup($"select TOP 1 Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabSubProcess' AND ID = '{ID.Trim()}' ");
                        tmpList2.Add(Name);
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
                    string[] SelectedNames = dr["Type"].ToString().Replace("/", ",").Split(',');
                    List<string> tmpList = new List<string>();

                    foreach (var Name in SelectedNames)
                    {
                        string ID = MyUtility.GetValue.Lookup($"select TOP 1 ID  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' AND Name = '{Name.Trim()}' ");
                        tmpList.Add(ID);
                    }

                    defaultSelected = tmpList.JoinToString(",");

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sql, "ID,Name", "ID,Name", defaultSelected.ToString(), null, null, null);
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string insertString = string.Empty;
                    string[] SelectedIDs = item.GetSelectedString().ToString().Split(',');
                    List<string> tmpList2 = new List<string>();

                    foreach (var ID in SelectedIDs)
                    {
                        string Name = MyUtility.GetValue.Lookup($"select TOP 1 Name  from DropDownList WITH (NOLOCK) where Type = 'Pms_LabAccessory' AND ID = '{ID.Trim()}' ");
                        tmpList2.Add(Name);
                    }

                    insertString = tmpList2.JoinToString(" / ");

                    dr["Type"] = insertString;

                    dr.EndEdit();
                }
            };

            Dictionary<string, string> ResultCombo = new Dictionary<string, string>();
            ResultCombo.Add("N/A", "N/A");
            ResultCombo.Add("Accepted", "Accepted");
            ResultCombo.Add("Rejected", "Rejected");
            ResultComboCell.DataSource = new BindingSource(ResultCombo, null);
            ResultComboCell.ValueMember = "Key";
            ResultComboCell.DisplayMember = "Value";

            // 預設選取的時候會全部變成大寫，關掉這個設定。
            TextColumnSetting.CharacterCasing = CharacterCasing.Normal;
            TextColumnSetting.MaxLength = 500;
            ArtworkCell.MaxLength = 200;

            this.Helper.Controls.Grid.Generator(this.gridAppearance)
            .Text("Type", header: "After Wash Appearance Check list", width: Widths.AnsiChars(40), iseditingreadonly: true, settings: ArtworkCell)
            .ComboBox("Wash1", header: "Wash1", width: Widths.AnsiChars(10), settings: ResultComboCell)
            .ComboBox("Wash2", header: "Wash2", width: Widths.AnsiChars(10), settings: ResultComboCell)
            .ComboBox("Wash3", header: "Wash3", width: Widths.AnsiChars(10), settings: ResultComboCell)
            .ComboBox("Wash4", header: "Wash4", width: Widths.AnsiChars(10), settings: ResultComboCell)
            .ComboBox("Wash5", header: "Wash5", width: Widths.AnsiChars(10), settings: ResultComboCell)
            .Text("Comment", header: "Comment", width: Widths.AnsiChars(20), settings: TextColumnSetting);

            this.tab1Load();
            this.tab2Load();
            this.tab3Load();

            this.tabControl1.SelectedIndex = 1;
            this.tabControl1.SelectedIndex = 2;
            this.tabControl1.SelectedIndex = 0;
        }

        private void tab1Load()
        {
            this.txtStyle.Text = MyUtility.Convert.GetString(this.MasterRow["StyleID"]);
            this.txtSeason.Text = MyUtility.Convert.GetString(this.MasterRow["SeasonID"]);
            this.txtBrand.Text = MyUtility.Convert.GetString(this.MasterRow["BrandID"]);
            this.txtSP.Text = MyUtility.Convert.GetString(this.MasterRow["OrderID"]);
            this.txtArticle.Text = MyUtility.Convert.GetString(this.MasterRow["Article"]);
            this.txtSize.Text = MyUtility.Convert.GetString(this.Deatilrow["SizeCode"]);

            this.dateSubmit.Value = MyUtility.Convert.GetDate(this.Deatilrow["SubmitDate"]);
            this.numArriveQty.Value = MyUtility.Convert.GetInt(this.Deatilrow["ArrivedQty"]);
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
        }

        DataTable dtShrinkage;

        private void tab2Load()
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

        DataTable dtApperance;

        private void tab3Load()
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

        private void btnEncode_Click(object sender, EventArgs e)
        {
            DualResult dr = DBProxy.Current.Execute(null, $"Update [GarmentTest_Detail] set Status='Confirmed' where id = '{this.MasterRow["ID"]}'");
            if (!dr)
            {
                this.ShowErr(dr);
            }
            else
            {
                this.btnenable();
            }
        }

        private void btnAmend_Click(object sender, EventArgs e)
        {
            DualResult dr = DBProxy.Current.Execute(null, $"Update [GarmentTest_Detail] set Status='New' where id = '{this.MasterRow["ID"]}'");
            if (!dr)
            {
                this.ShowErr(dr);
            }
            else
            {
                this.btnenable();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
           {
                #region update GarmentTest_Detail 1
                string SubmitDate = MyUtility.Check.Empty(this.dateSubmit.Value) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.dateSubmit.Value)).ToString("yyyy/MM/dd");
                string updateGarmentTest_Detail = $@"
update GarmentTest_Detail set
    SubmitDate = iif('{SubmitDate}'='',null,'{SubmitDate}'),
    ArrivedQty =  {this.numArriveQty.Value},
    Result = '{this.comboResult.SelectedValue}',
    Remark =  '{this.txtRemark.Text}',
    LineDry =  '{this.rdbtnLine.Checked}',
    Temperature =  '{this.comboTemperature.Text}',
    TumbleDry =  '{this.rdbtnTumble.Checked}',
    Machine =  '{this.comboMachineModel.Text}',
    HandWash =  '{this.rdbtnHand.Checked}',
    Composition =  '{this.txtFibreComposition.Text}',
    Neck ='{MyUtility.Convert.GetString(this.comboNeck.Text).EqualString("YES")}'
where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["NO"]}
";
                DualResult dr = DBProxy.Current.Execute(null, updateGarmentTest_Detail);
                if (!dr)
                {
                    this.ShowErr(dr);
                }
                #endregion
                this.tab2ShrinkageSave();
                this.tab2TwistingSave();
                DBProxy.Current.Execute(null, $"update Garmenttest_Detail set Editname = '{Env.User.UserID}',EditDate = getdate() where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}");
                this.tab2Load();

                this.tab3ApperanceSave();
                this.tab3Load();

                this.btnenable();
                this.gridAppearance.ForeColor = Color.Black;
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

                this.gridAppearance.ForeColor = Color.Red;
                this.btnEncode.Enabled = false;
                this.btnAmend.Enabled = false;
            }

            this.EditMode = !this.EditMode;
            this.rdbtnLine.Enabled = this.EditMode;
            this.rdbtnTumble.Enabled = this.EditMode;
            this.rdbtnHand.Enabled = this.EditMode;
            this.btnPDF.Enabled = !this.EditMode;
            this.gridActualShrinkage.ReadOnly = !this.EditMode;
            this.gridAppearance.ReadOnly = !this.EditMode;
            this.btnEdit.Text = this.EditMode ? "Save" : "Edit";
        }

        private void tab2ShrinkageSave()
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

        private void tab2TwistingSave()
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

        private void tab3ApperanceSave()
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

        private void btnenable()
        {
            string detailstatus = MyUtility.GetValue.Lookup($"select Status from GarmentTest_Detail where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["NO"]}");
            this.btnEncode.Enabled = detailstatus.EqualString("New");
            this.btnAmend.Enabled = detailstatus.EqualString("Confirmed");
            this.btnEdit.Enabled = !detailstatus.EqualString("Confirmed");
            this.rdbtnLine.Enabled = this.EditMode;
            this.rdbtnTumble.Enabled = this.EditMode;
            this.rdbtnHand.Enabled = this.EditMode;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPDF_Click(object sender, EventArgs e)
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
            string SendDate = Convert.ToDateTime(MyUtility.GetValue.Lookup($"SELECT BuyerDelivery FROM Orders WHERE ID = '{this.MasterRow["OrderID"].ToString()}'")).ToShortDateString();
            worksheet.Cells[8, 4] = SendDate;
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
                DataRow drTechnicianInfo;
                string technicianName = string.Empty;
                string picSource = string.Empty;
                Image img = null;
                Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[12, 2];

                if (MyUtility.Check.Seek(sql_cmd, out drTechnicianInfo))
                {
                    technicianName = drTechnicianInfo["name"].ToString();
                    picSource = drTechnicianInfo["SignaturePic"].ToString();

                    // Name
                    worksheet.Cells[74, 9] = technicianName;

                    // 插入圖檔
                    if (!MyUtility.Check.Empty(picSource))
                    {
                        if (File.Exists(picSource))
                        {
                            img = Image.FromFile(picSource);
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
                this.rowHeight(worksheet, 61, strComment);
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
                this.rowHeight(worksheet, 62, strComment);
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
                this.rowHeight(worksheet, 63, strComment);
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
                this.rowHeight(worksheet, 64, strComment);
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
                this.rowHeight(worksheet, 65, strComment);
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
                this.rowHeight(worksheet, 66, strComment);
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
                this.rowHeight(worksheet, 67, strComment);
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
                this.rowHeight(worksheet, 68, strComment);
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
                this.rowHeight(worksheet, 69, strComment);
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
                        worksheet.Cells[44, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Waistband (relax)'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[45, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Hip Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[46, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Thigh Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[47, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Side Seam'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[48, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Leg Opening'", i);
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
                        worksheet.Cells[34, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[35, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[36, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[37, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[38, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Hem Opening'", i);
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
                        worksheet.Cells[26, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[27, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[28, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[29, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[30, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Hem Opening'", i);
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
                        worksheet.Cells[18, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[19, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[20, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[21, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[22, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Hem Opening'", i);
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
                DataRow drTechnicianInfo;
                string technicianName = string.Empty;
                string picSource = string.Empty;
                Image img = null;
                Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[12, 2];

                if (MyUtility.Check.Seek(sql_cmd, out drTechnicianInfo))
                {
                    technicianName = drTechnicianInfo["name"].ToString();
                    picSource = drTechnicianInfo["SignaturePic"].ToString();

                    // Name
                    worksheet.Cells[74, 9] = technicianName;

                    // 插入圖檔
                    if (!MyUtility.Check.Empty(picSource))
                    {
                        if (File.Exists(picSource))
                        {
                            img = Image.FromFile(picSource);
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
                this.rowHeight(worksheet, 61, strComment);
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
                this.rowHeight(worksheet, 62, strComment);
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
                this.rowHeight(worksheet, 63, strComment);
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
                this.rowHeight(worksheet, 64, strComment);
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
                this.rowHeight(worksheet, 65, strComment);
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
                this.rowHeight(worksheet, 66, strComment);
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
                this.rowHeight(worksheet, 67, strComment);
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
                this.rowHeight(worksheet, 68, strComment);
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
                        worksheet.Cells[44, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Waistband (relax)'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[45, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Hip Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[46, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Thigh Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[47, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Side Seam'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[48, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM' and type ='Leg Opening'", i);
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
                        worksheet.Cells[34, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[35, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[36, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[37, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[38, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER' and type ='Hem Opening'", i);
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
                        worksheet.Cells[26, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[27, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[28, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[29, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[30, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'INNER' and type ='Hem Opening'", i);
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
                        worksheet.Cells[18, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Chest Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[19, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Width'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[20, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[21, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Back Length'", i);
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count; i++)
                    {
                        worksheet.Cells[22, i] = this.addShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Hem Opening'", i);
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
            string strFileName = string.Empty;
            string strPDFFileName = string.Empty;
            strFileName = Class.MicrosoftFile.GetName("Quality_P04_GarmentWash");
            strPDFFileName = Class.MicrosoftFile.GetName("Quality_P04_GarmentWash", Class.PDFFileNameExtension.PDF);
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

        private void rowHeight(Microsoft.Office.Interop.Excel.Worksheet worksheet, int row, string strComment)
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
        /// <param name="dt"></param>
        /// <param name="strFilter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private string addShrinkageUnit(DataTable dt, string strFilter, int count)
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
                    strValie = strValie + "%";
                }
            }

            return strValie;
        }
    }
}
