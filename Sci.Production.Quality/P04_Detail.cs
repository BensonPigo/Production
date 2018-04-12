using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P04_Detail : Sci.Win.Forms.Base
    {
        private DataRow Deatilrow;
        private DataRow MasterRow;
        public P04_Detail(bool editmode,DataRow masterrow, DataRow deatilrow)
        {
            InitializeComponent();
            this.MasterRow = masterrow;
            this.Deatilrow = deatilrow;
            this.EditMode = editmode;
        }
        
        private void P04_Detail_Load(object sender, EventArgs e)
        {
            btnenable();

            comboTemperature.SelectedIndex = 0;
            comboMachineModel.SelectedIndex = 0;
            comboNeck.SelectedIndex = 0;

            Helper.Controls.Grid.Generator(this.gridActualShrinkage)
            .Text("Location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Type", header: "Type", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Numeric("BeforeWash", header: "Before Wash", width: Widths.AnsiChars(6), decimal_places: 2)
            .Numeric("SizeSpec", header: "Size Spec Meas.", width: Widths.AnsiChars(8), decimal_places: 2)
            .Numeric("AfterWash1", header: "After Wash 1", width: Widths.AnsiChars(6), decimal_places: 2)
            .Numeric("Shrinkage1", header: "Shrinkage 1", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999)
            .Numeric("AfterWash2", header: "After Wash 2", width: Widths.AnsiChars(6), decimal_places: 2)
            .Numeric("Shrinkage2", header: "Shrinkage 2", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999)
            .Numeric("AfterWash3", header: "After Wash 3", width: Widths.AnsiChars(6), decimal_places: 2)
            .Numeric("Shrinkage3", header: "Shrinkage 3", width: Widths.AnsiChars(8), decimal_places: 2, minimum: -999999999);


            Dictionary<string, string> ResultPF = new Dictionary<string, string>();
            ResultPF.Add("P", "Pass");
            ResultPF.Add("F", "Fail");
            comboResult.DataSource = new BindingSource(ResultPF, null);
            comboResult.ValueMember = "Key";
            comboResult.DisplayMember = "Value";


            DataGridViewGeneratorComboBoxColumnSettings ResultComboCell = new DataGridViewGeneratorComboBoxColumnSettings();
            Dictionary<string, string> ResultCombo = new Dictionary<string, string>();
            ResultCombo.Add("N/A", "N/A");
            ResultCombo.Add("Accepted", "Accepted");
            ResultCombo.Add("Rejected", "Rejected");
            ResultComboCell.DataSource = new BindingSource(ResultCombo, null);
            ResultComboCell.ValueMember = "Key";
            ResultComboCell.DisplayMember = "Value";

            Helper.Controls.Grid.Generator(this.gridAppearance)
            .Text("Type", header: "After Wash Appearance Check list", width: Widths.AnsiChars(40), iseditingreadonly: true)
            .ComboBox("Wash1", header: "Wash1", width: Widths.AnsiChars(10), settings: ResultComboCell)
            .ComboBox("Wash2", header: "Wash2", width: Widths.AnsiChars(10), settings: ResultComboCell)
            .ComboBox("Wash3", header: "Wash3", width: Widths.AnsiChars(10), settings: ResultComboCell)
            .Text("Comment", header: "Comment", width: Widths.AnsiChars(10));

            tab1Load(); 
            tab2Load();
            tab3Load();

            tabControl1.SelectedIndex = 1;
            tabControl1.SelectedIndex = 2;
            tabControl1.SelectedIndex = 0;
        }

        private void tab1Load()
        {
            txtStyle.Text = MyUtility.Convert.GetString(MasterRow["StyleID"]);
            txtSeason.Text = MyUtility.Convert.GetString(MasterRow["SeasonID"]);
            txtBrand.Text = MyUtility.Convert.GetString(MasterRow["BrandID"]);
            txtSP.Text = MyUtility.Convert.GetString(MasterRow["OrderID"]);
            txtArticle.Text = MyUtility.Convert.GetString(MasterRow["Article"]);
            txtSize.Text = MyUtility.Convert.GetString(Deatilrow["SizeCode"]);

            dateSubmit.Value = MyUtility.Convert.GetDate(Deatilrow["SubmitDate"]);
            numArriveQty.Value = MyUtility.Convert.GetInt(Deatilrow["ArrivedQty"]);
            comboResult.Text = MyUtility.Convert.GetString(Deatilrow["Result"]);
            txtRemark.Text = MyUtility.Convert.GetString(Deatilrow["Remark"]);
            rdbtnLine.Checked = MyUtility.Convert.GetBool(Deatilrow["LineDry"]);
            rdbtnTumble.Checked = MyUtility.Convert.GetBool(Deatilrow["TumbleDry"]);
            rdbtnHand.Checked = MyUtility.Convert.GetBool(Deatilrow["HandWash"]);
            comboTemperature.Text = MyUtility.Convert.GetString(Deatilrow["Temperature"]);
            comboMachineModel.Text = MyUtility.Convert.GetString(Deatilrow["Machine"]);
            txtFibreComposition.Text = MyUtility.Convert.GetString(Deatilrow["Composition"]);
            comboNeck.Text = MyUtility.Convert.GetString(Deatilrow["Neck"]);
            comboResult.Text = MyUtility.Convert.GetString(Deatilrow["Result"])=="P"?"Pass":"Fail";
        }

        DataTable dtShrinkage;
        private void tab2Load()
        {
            gridActualShrinkage.IsEditingReadOnly = false;

            string sqlShrinkage = $@"select [ID]      ,[No]
      ,[Location]=case when Location='T' then 'TOP'
                               when Location='I' then 'INNER'
                               when Location='O' then 'OUTER'
                               when Location='B' then 'BOTTOM' end
      ,[Type]      ,[BeforeWash]      ,[SizeSpec]      ,[AfterWash1]      ,[Shrinkage1]      ,[AfterWash2]      ,[Shrinkage2]      ,[AfterWash3]      ,[Shrinkage3]
from[GarmentTest_Detail_Shrinkage] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}
order by seq";
            
            DBProxy.Current.Select(null, sqlShrinkage, out dtShrinkage);
            listControlBindingSource1.DataSource = null;
            listControlBindingSource1.DataSource = dtShrinkage;
            int i = 4;
            if (dtShrinkage.Select("Location = 'TOP'").Length==0)
            {
                panel1.Visible = false;
                i--;
            }
            if (dtShrinkage.Select("Location = 'INNER'").Length == 0)
            {
                panel2.Visible = false;
                i--;
            }
            if (dtShrinkage.Select("Location = 'OUTER'").Length == 0)
            {
                panel3.Visible = false;
                i--;
            }
            if (dtShrinkage.Select("Location = 'BOTTOM'").Length == 0)
            {
                panel4.Visible = false;
                i--;
            }
            flowLayoutPanel1.Height =36*i;

            numTopS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='T'"));
            numTopS2.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S2 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='T'"));
            numTopL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select sum(L) from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='T'"));
            numTwisTingTop.Value = numTopL.Value.Empty() ? 0 : (((numTopS1.Value + numTopS2.Value) / 2) / numTopL.Value) * 100;

            numInnerS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='I'"));
            numInnerS2.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S2 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='I'"));
            numInnerL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select sum(L) from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='I'"));
            numTwisTingInner.Value = numInnerL.Value.Empty() ? 0 : (((numInnerS1.Value + numInnerS2.Value) / 2) / numInnerL.Value) * 100;

            numTopS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='O'"));
            numTopS2.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S2 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='O'"));
            numTopL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select sum(L) from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='O'"));
            numTwisTingTop.Value = numTopL.Value.Empty() ? 0 : (((numTopS1.Value + numTopS2.Value) / 2) / numTopL.Value) * 100;

            numBottomS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='B'"));
            numBottomL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select L   from[GarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='B'"));
            numTwisTingBottom.Value = numBottomL.Value.Empty() ? 0 : numBottomS1.Value / numBottomL.Value * 100;
        }

        DataTable dtApperance;
        private void tab3Load()
        {
            gridAppearance.IsEditingReadOnly = false;

            string sqlApperance = $@"select * from[GarmentTest_Detail_Apperance] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} order by seq";
            
            DBProxy.Current.Select(null, sqlApperance, out dtApperance);
            listControlBindingSource2.DataSource = null;
            listControlBindingSource2.DataSource = dtApperance;
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            DualResult dr =DBProxy.Current.Execute(null, $"Update [GarmentTest_Detail] set Status='Confirmed' where id = '{this.MasterRow["ID"]}'");
            if (!dr)
            {
                ShowErr(dr);
            }
            else
            {
                btnenable();
            }
        }

        private void btnAmend_Click(object sender, EventArgs e)
        {
            DualResult dr = DBProxy.Current.Execute(null, $"Update [GarmentTest_Detail] set Status='New' where id = '{this.MasterRow["ID"]}'");
            if (!dr)
            {
                ShowErr(dr);
            }
            else
            {
                btnenable();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
           {

                #region update GarmentTest_Detail 1
                string SubmitDate = MyUtility.Check.Empty(dateSubmit.Value) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(dateSubmit.Value)).ToString("yyyy/MM/dd");
                string updateGarmentTest_Detail = $@"
update GarmentTest_Detail set
    SubmitDate = iif('{SubmitDate}'='',null,'{SubmitDate}'),
    ArrivedQty =  {numArriveQty.Value},
    Result = '{comboResult.SelectedValue}',
    Remark =  '{txtRemark.Text}',
    LineDry =  '{rdbtnLine.Checked}',
    Temperature =  '{comboTemperature.Text}',
    TumbleDry =  '{rdbtnTumble.Checked}',
    Machine =  '{comboMachineModel.Text}',
    HandWash =  '{rdbtnHand.Checked}',
    Composition =  '{txtFibreComposition.Text}',
    Neck ='{comboMachineModel.Text.EqualString("Yes")}'
where id = {Deatilrow["ID"]} and No = {Deatilrow["NO"]}
";
                DualResult dr = DBProxy.Current.Execute(null, updateGarmentTest_Detail);
                if (!dr)
                {
                    ShowErr(dr);
                }
                #endregion
                tab2ShrinkageSave();
                tab2TwistingSave();
                DBProxy.Current.Execute(null, $"update Garmenttest_Detail set Editname = '{Sci.Env.User.UserID}',EditDate = getdate() where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}");
                tab2Load();

                tab3ApperanceSave();
                tab3Load();

                btnenable();
                gridAppearance.ForeColor = Color.Black;
            }
            else
            {
                gridAppearance.ForeColor = Color.Red;
                gridAppearance.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
                btnEncode.Enabled = false;
                btnAmend.Enabled = false;
            }

            this.EditMode = !this.EditMode;
            rdbtnLine.Enabled = this.EditMode;
            rdbtnTumble.Enabled = this.EditMode;
            rdbtnHand.Enabled = this.EditMode;
            btnPDF.Enabled = !this.EditMode;
            gridActualShrinkage.ReadOnly = !this.EditMode;
            gridAppearance.ReadOnly = !this.EditMode;
            btnEdit.Text = this.EditMode ? "Save" : "Edit";
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
            
            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, string.Empty, savetab2Shrinkage, out dtShrinkage);
            if (!result)
            {
                ShowErr(result);
                return;
            }
        }

        private void tab2TwistingSave()
        {
            string savetab2Twisting = $@"
update [GarmentTest_Detail_Twisting] set S1={numTopS1.Value},S2={numTopS2.Value},L={numTopL.Value},Twisting={numTwisTingTop.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='T'
update [GarmentTest_Detail_Twisting] set S1={numInnerS1.Value},S2={numInnerS2.Value},L={numInnerL.Value},Twisting={numTwisTingInner.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='I'
update [GarmentTest_Detail_Twisting] set S1={numTopS1.Value},S2={numTopS2.Value},L={numTopL.Value},Twisting={numTwisTingTop.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='O'
update [GarmentTest_Detail_Twisting] set S1={numBottomS1.Value},L={numBottomL.Value},Twisting={numTwisTingBottom.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='B'
";
            DualResult result = DBProxy.Current.Execute(null, savetab2Twisting);
            if (!result)
            {
                ShowErr(result);
            }
        }

        private void tab3ApperanceSave()
        {
            string savetab2Apperance = $@"
  merge [GarmentTest_Detail_Apperance] t
  using #tmp s
  on s.id = t.id and s.no = t.no and s.type = t.type
  when matched then
  update set
	t.[Wash1]  = s.[Wash1],
    t.[Wash2]	= s.[Wash2]	,
    t.[Wash3]	= s.[Wash3],
    t.[Comment]	= s.[Comment]
	;

select * from [GarmentTest_Detail_Apperance]  where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} order by seq";
            
            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource2.DataSource, string.Empty, savetab2Apperance, out dtApperance);
            if (!result)
            {
                ShowErr(result);
                return;
            }
        }

        private void btnenable()
        {
            string detailstatus = MyUtility.GetValue.Lookup($"select Status from GarmentTest_Detail where id = {Deatilrow["ID"]} and No = {Deatilrow["NO"]}");
            btnEncode.Enabled = detailstatus.EqualString("New");
            btnAmend.Enabled = detailstatus.EqualString("Confirmed");
            btnEdit.Enabled = !detailstatus.EqualString("Confirmed");
            rdbtnLine.Enabled = this.EditMode;
            rdbtnTumble.Enabled = this.EditMode;
            rdbtnHand.Enabled = this.EditMode;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            if(dtApperance.Rows.Count == 0 || dtShrinkage.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
                return;
            }
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_P04_GarmentWash.xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            if (!MyUtility.Check.Empty(dateSubmit.Value))
                worksheet.Cells[4, 4] = MyUtility.Convert.GetDate(dateSubmit.Value).Value.Year + "/" + MyUtility.Convert.GetDate(dateSubmit.Value).Value.Month + "/" + MyUtility.Convert.GetDate(dateSubmit.Value).Value.Day;
            if (!MyUtility.Check.Empty(Deatilrow["inspdate"]))
                worksheet.Cells[4, 8] = MyUtility.Convert.GetDate(Deatilrow["inspdate"]).Value.Year + "/" + MyUtility.Convert.GetDate(Deatilrow["inspdate"]).Value.Month + "/" + MyUtility.Convert.GetDate(Deatilrow["inspdate"]).Value.Day;
            worksheet.Cells[4, 10] = MyUtility.Convert.GetString(MasterRow["OrderID"]);
            worksheet.Cells[4, 12] = MyUtility.Convert.GetString(MasterRow["BrandID"]);
            worksheet.Cells[6, 4] = MyUtility.Convert.GetString(MasterRow["StyleID"]);
            worksheet.Cells[6, 8] = MyUtility.GetValue.Lookup($"select CustPONo from Orders with(nolock) where id = '{MasterRow["OrderID"]}'");
            worksheet.Cells[6, 10] = MyUtility.Convert.GetString(MasterRow["Article"]);
            worksheet.Cells[7, 4] = MyUtility.GetValue.Lookup($"select StyleName from Style with(nolock) where id = '{MasterRow["Styleid"]}' and seasonid = '{MasterRow["seasonid"]}' and brandid = '{MasterRow["brandid"]}'");
            worksheet.Cells[7, 8] = MyUtility.Convert.GetDecimal(numArriveQty.Value);
            if (!MyUtility.Check.Empty(Deatilrow["SendDate"]))
                worksheet.Cells[8, 4] = MyUtility.Convert.GetDate(Deatilrow["SendDate"]).Value.Year + "/" + MyUtility.Convert.GetDate(Deatilrow["SendDate"]).Value.Month + "/" + MyUtility.Convert.GetDate(Deatilrow["SendDate"]).Value.Day;
            worksheet.Cells[8, 8] = MyUtility.Convert.GetString(txtSize.Text);

            worksheet.Cells[11, 4] = rdbtnLine.Checked ? "V" : string.Empty;
            worksheet.Cells[12, 4] = rdbtnTumble.Checked ? "V" : string.Empty;
            worksheet.Cells[13, 4] = rdbtnHand.Checked ? "V" : string.Empty;
            worksheet.Cells[11, 8] = comboTemperature.Text + "˚C ";
            worksheet.Cells[12, 8] = comboMachineModel.Text;
            worksheet.Cells[13, 8] = txtFibreComposition.Text;

            #region 最下面 Signature
            if (MyUtility.Convert.GetString(Deatilrow["Result"]).EqualString("P"))
            {
                worksheet.Cells[73, 5] = "V";
            }
            else
            {
                worksheet.Cells[73, 6] = "V";
            }
            worksheet.Cells[72, 7] = MyUtility.Convert.GetString(Deatilrow["Showname"]);
            #endregion

            #region After Wash Appearance Check list
            string tmpAR;

            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=1")[0]["wash1"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[61, 4] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[61, 5] = "V";
            else
                worksheet.Cells[61, 4] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=1")[0]["wash2"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[61, 6] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[61, 7] = "V";
            else
                worksheet.Cells[61, 6] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=1")[0]["wash3"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[61, 8] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[61, 9] = "V";
            else
                worksheet.Cells[61, 8] = tmpAR;
            worksheet.Cells[61, 10] = MyUtility.Convert.GetString(dtApperance.Select("seq=1")[0]["Comment"]);
            
            //
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=2")[0]["wash1"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[62, 4] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[62, 5] = "V";
            else
                worksheet.Cells[62, 4] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=2")[0]["wash2"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[62, 6] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[62, 7] = "V";
            else
                worksheet.Cells[62, 6] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=2")[0]["wash3"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[62, 8] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[62, 9] = "V";
            else
                worksheet.Cells[62, 8] = tmpAR;
            worksheet.Cells[62, 10] = MyUtility.Convert.GetString(dtApperance.Select("seq=2")[0]["Comment"]);

            //
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=3")[0]["wash1"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[63, 4] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[63, 5] = "V";
            else
                worksheet.Cells[63, 4] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=3")[0]["wash2"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[63, 6] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[63, 7] = "V";
            else
                worksheet.Cells[63, 6] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=3")[0]["wash3"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[63, 8] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[63, 9] = "V";
            else
                worksheet.Cells[63, 8] = tmpAR;
            worksheet.Cells[63, 10] = MyUtility.Convert.GetString(dtApperance.Select("seq=3")[0]["Comment"]);

            //
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=4")[0]["wash1"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[64, 4] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[64, 5] = "V";
            else
                worksheet.Cells[64, 4] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=4")[0]["wash2"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[64, 6] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[64, 7] = "V";
            else
                worksheet.Cells[64, 6] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=4")[0]["wash3"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[64, 8] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[64, 9] = "V";
            else
                worksheet.Cells[64, 8] = tmpAR;
            worksheet.Cells[64, 10] = MyUtility.Convert.GetString(dtApperance.Select("seq=4")[0]["Comment"]);

            //
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=5")[0]["wash1"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[65, 4] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[65, 5] = "V";
            else
                worksheet.Cells[65, 4] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=5")[0]["wash2"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[65, 6] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[65, 7] = "V";
            else
                worksheet.Cells[65, 6] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=5")[0]["wash3"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[65, 8] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[65, 9] = "V";
            else
                worksheet.Cells[65, 8] = tmpAR;
            worksheet.Cells[65, 10] = MyUtility.Convert.GetString(dtApperance.Select("seq=5")[0]["Comment"]);

            //
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=6")[0]["wash1"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[66, 4] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[66, 5] = "V";
            else
                worksheet.Cells[66, 4] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=6")[0]["wash2"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[66, 6] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[66, 7] = "V";
            else
                worksheet.Cells[66, 6] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=6")[0]["wash3"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[66, 8] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[66, 9] = "V";
            else
                worksheet.Cells[66, 8] = tmpAR;
            worksheet.Cells[66, 10] = MyUtility.Convert.GetString(dtApperance.Select("seq=6")[0]["Comment"]);

            //
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=7")[0]["wash1"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[67, 4] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[67, 5] = "V";
            else
                worksheet.Cells[67, 4] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=7")[0]["wash2"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[67, 6] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[67, 7] = "V";
            else
                worksheet.Cells[67, 6] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=7")[0]["wash3"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[67, 8] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[67, 9] = "V";
            else
                worksheet.Cells[67, 8] = tmpAR;
            worksheet.Cells[67, 10] = MyUtility.Convert.GetString(dtApperance.Select("seq=7")[0]["Comment"]);

            //
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=8")[0]["wash1"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[68, 4] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[68, 5] = "V";
            else
                worksheet.Cells[68, 4] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=8")[0]["wash2"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[68, 6] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[68, 7] = "V";
            else
                worksheet.Cells[68, 6] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=8")[0]["wash3"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[68, 8] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[68, 9] = "V";
            else
                worksheet.Cells[68, 8] = tmpAR;
            worksheet.Cells[68, 10] = MyUtility.Convert.GetString(dtApperance.Select("seq=8")[0]["Comment"]);

            //
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=9")[0]["wash1"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[69, 4] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[69, 5] = "V";
            else
                worksheet.Cells[69, 4] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=9")[0]["wash2"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[69, 6] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[69, 7] = "V";
            else
                worksheet.Cells[69, 6] = tmpAR;
            tmpAR = MyUtility.Convert.GetString(dtApperance.Select("seq=9")[0]["wash3"]);
            if (tmpAR.EqualString("Accepted"))
                worksheet.Cells[69, 8] = "V";
            else if (tmpAR.EqualString("Rejected"))
                worksheet.Cells[69, 9] = "V";
            else
                worksheet.Cells[69, 8] = tmpAR;
            worksheet.Cells[69, 10] = MyUtility.Convert.GetString(dtApperance.Select("seq=9")[0]["Comment"]);
            #endregion

            if (comboNeck.Text.EqualString("Yes"))
            {
                worksheet.Cells[40, 9] = "V";
            }
            else
            {
                worksheet.Cells[40, 11] = "V";
            }

            #region %
            if (dtShrinkage.Select("Location = 'BOTTOM'").Length>0)
            {
                worksheet.Cells[56, 4] = numTwisTingBottom.Text + "%";
                worksheet.Cells[56, 7] = numBottomS1.Value;
                worksheet.Cells[56, 9] = numBottomL.Value;
            }
            else
            {
                Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A56:A57", Type.Missing).EntireRow;
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            if (dtShrinkage.Select("Location = 'OUTER'").Length > 0)
            {

                worksheet.Cells[54, 4] = numTwisTingTop.Text + "%";
                worksheet.Cells[54, 7] = numTopS1.Value;
                worksheet.Cells[54, 9] = numTopS2.Value;
                worksheet.Cells[54, 11] = numTopL.Value;
            }
            else
            {
                Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A54:A55", Type.Missing).EntireRow;
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            if (dtShrinkage.Select("Location = 'INNER'").Length > 0)
            {
                worksheet.Cells[52, 4] = numTwisTingInner.Text + "%";
                worksheet.Cells[52, 7] = numInnerS1.Value;
                worksheet.Cells[52, 9] = numInnerS2.Value;
                worksheet.Cells[52, 11] = numInnerL.Value;
            }
            else
            {
                Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A52:A53", Type.Missing).EntireRow;
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            if (dtShrinkage.Select("Location = 'TOP'").Length > 0)
            {
                worksheet.Cells[50, 4] = numTwisTingTop.Text + "%";
                worksheet.Cells[50, 7] = numTopS1.Value;
                worksheet.Cells[50, 9] = numTopS2.Value;
                worksheet.Cells[50, 11] = numTopL.Value;
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
            if (dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
            {
                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[44, i] = dtShrinkage.Select("Location = 'BOTTOM'and type ='Waistband (relax)'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[45, i] = dtShrinkage.Select("Location = 'BOTTOM'and type ='Hip Width'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[46, i] = dtShrinkage.Select("Location = 'BOTTOM'and type ='Thigh Width'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[47, i] = dtShrinkage.Select("Location = 'BOTTOM'and type ='Side Seam'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[48, i] = dtShrinkage.Select("Location = 'BOTTOM'and type ='Leg Opening'")[0][i];
                }
            }
            else
            {
                Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A42:A49", Type.Missing).EntireRow;
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            if (dtShrinkage.Select("Location = 'OUTER'").Length > 0)
            {
                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[34, i] = dtShrinkage.Select("Location = 'OUTER'and type ='Chest Width'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[35, i] = dtShrinkage.Select("Location = 'OUTER'and type ='Sleeve Width'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[36, i] = dtShrinkage.Select("Location = 'OUTER'and type ='Sleeve Length'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[37, i] = dtShrinkage.Select("Location = 'OUTER'and type ='Back Length'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[38, i] = dtShrinkage.Select("Location = 'OUTER'and type ='Hem Opening'")[0][i];
                }
            }
            else
            {
                Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A32:A39", Type.Missing).EntireRow;
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            if (dtShrinkage.Select("Location = 'INNER'").Length > 0)
            {
                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[26, i] = dtShrinkage.Select("Location = 'INNER'and type ='Chest Width'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[27, i] = dtShrinkage.Select("Location = 'INNER'and type ='Sleeve Width'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[28, i] = dtShrinkage.Select("Location = 'INNER'and type ='Sleeve Length'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[29, i] = dtShrinkage.Select("Location = 'INNER'and type ='Back Length'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[30, i] = dtShrinkage.Select("Location = 'INNER'and type ='Hem Opening'")[0][i];
                }
            }
            else
            {
                Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A24:A31", Type.Missing).EntireRow;
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }

            if (dtShrinkage.Select("Location = 'TOP'").Length > 0)
            {
                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[18, i] = dtShrinkage.Select("Location = 'TOP'and type ='Chest Width'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[19, i] = dtShrinkage.Select("Location = 'TOP'and type ='Sleeve Width'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[20, i] = dtShrinkage.Select("Location = 'TOP'and type ='Sleeve Length'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[21, i] = dtShrinkage.Select("Location = 'TOP'and type ='Back Length'")[0][i];
                }

                for (int i = 4; i < dtShrinkage.Columns.Count; i++)
                {
                    worksheet.Cells[22, i] = dtShrinkage.Select("Location = 'TOP'and type ='Hem Opening'")[0][i];
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
            
            #region Save & Show Excel
            string strFileName = string.Empty;
            string strPDFFileName = string.Empty;
            strFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P04_GarmentWash");
            strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P04_GarmentWash", Sci.Production.Class.PDFFileNameExtension.PDF);
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
    }
}
