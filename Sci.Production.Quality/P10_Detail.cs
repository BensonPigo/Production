using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P10_Detail : Sci.Win.Forms.Base
    {
        private DataRow Deatilrow;
        private DataRow MasterRow;
        private string style = "";
        private string season = "";
        private string brand = "";

        public P10_Detail(bool editmode, DataRow masterrow, DataRow deatilrow)
        {
            InitializeComponent();
            this.MasterRow = masterrow;
            this.Deatilrow = deatilrow;
            this.EditMode = editmode;
        }

        private void P10_Detail_Load(object sender, EventArgs e)
        {
            btnenable();

            comboTemperature.SelectedIndex = 0;
            comboMachineModel.SelectedIndex = 0;
            comboNeck.SelectedIndex = 0;

            Helper.Controls.Grid.Generator(this.gridShrinkage)
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

            //Result 選單
            Dictionary<string, string> ResultPF = new Dictionary<string, string>();
            ResultPF.Add("P", "Pass");
            ResultPF.Add("F", "Fail");
            comboResult.DataSource = new BindingSource(ResultPF, null);
            comboResult.ValueMember = "Key";
            comboResult.DisplayMember = "Value";

            DataGridViewGeneratorComboBoxColumnSettings ResultComboCell = new DataGridViewGeneratorComboBoxColumnSettings();

            DataGridViewGeneratorTextColumnSettings TextColumnSetting = new DataGridViewGeneratorTextColumnSettings();

            Dictionary<string, string> ResultCombo = new Dictionary<string, string>();
            ResultCombo.Add("N/A", "N/A");
            ResultCombo.Add("Accepted", "Accepted");
            ResultCombo.Add("Rejected", "Rejected");
            ResultComboCell.DataSource = new BindingSource(ResultCombo, null);
            ResultComboCell.ValueMember = "Key";
            ResultComboCell.DisplayMember = "Value";

            //預設選取的時候會全部變成大寫，關掉這個設定。
            TextColumnSetting.CharacterCasing= CharacterCasing.Normal;

            Helper.Controls.Grid.Generator(this.gridAppearance)
            .Text("Type", header: "After Wash Appearance Check list", width: Widths.AnsiChars(40), iseditingreadonly: false,settings:TextColumnSetting)
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



        private void btnenable()
        {
            string detailstatus = MyUtility.GetValue.Lookup($"select Status from SampleGarmentTest_Detail where id = {Deatilrow["ID"]} and No = {Deatilrow["NO"]}");
            btnEncode.Enabled = detailstatus.EqualString("New");
            btnAmend.Enabled = detailstatus.EqualString("Confirmed");
            btnEdit.Enabled = !detailstatus.EqualString("Confirmed");
            rdbtnLine.Enabled = this.EditMode;
            rdbtnTumble.Enabled = this.EditMode;
            rdbtnHand.Enabled = this.EditMode;
        }

        #region tab載入

        private void tab1Load()
        {
            //主檔資料
            txtStyle.Text = MyUtility.Convert.GetString(MasterRow["StyleID"]);
            style = MyUtility.Convert.GetString(MasterRow["StyleID"]);
            txtSeason.Text = MyUtility.Convert.GetString(MasterRow["SeasonID"]);
            season = MyUtility.Convert.GetString(MasterRow["SeasonID"]); 
            txtBrand.Text = MyUtility.Convert.GetString(MasterRow["BrandID"]);
            brand = MyUtility.Convert.GetString(MasterRow["BrandID"]);

            txtReportNo.Text = MyUtility.Convert.GetString(Deatilrow["ReportNo"]);
            txtArticle.Text = MyUtility.Convert.GetString(MasterRow["Article"]);

            //明細檔資料
            string sqlShrinkage = $@"select * from[SampleGarmentTest_Detail] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} ";
            DataTable tmp;
            DBProxy.Current.Select(null, sqlShrinkage, out tmp);
            DataRow dr = tmp.Rows[0];


            txtSize.Text = MyUtility.Convert.GetString(dr["SizeCode"]);
            txtColour.Text = MyUtility.Convert.GetString(dr["Colour"]);
            txtReportDate.Value = MyUtility.Convert.GetDate(dr["ReportDate"]);
            comboResult.Text = MyUtility.Convert.GetString(dr["Result"]);
            
            txtRemark.Text = MyUtility.Convert.GetString(dr["Remark"]);

            //如果三個都沒選，預設選第一個
            //rdbtnLine.Checked = MyUtility.Convert.GetBool(Deatilrow["LineDry"]);
            if (!(MyUtility.Convert.GetBool(dr["LineDry"]) && MyUtility.Convert.GetBool(dr["TumbleDry"]) && MyUtility.Convert.GetBool(dr["HandWash"])))
            {
                rdbtnLine.Checked = true;
            }
            

            rdbtnTumble.Checked = MyUtility.Convert.GetBool(dr["TumbleDry"]);
            rdbtnHand.Checked = MyUtility.Convert.GetBool(dr["HandWash"]);

            comboTemperature.Text = MyUtility.Convert.GetString(dr["Temperature"]);
            comboMachineModel.Text = MyUtility.Convert.GetString(dr["Machine"]);
            txtFibreComposition.Text = MyUtility.Convert.GetString(dr["Composition"]);
            comboNeck.Text = MyUtility.Convert.GetBool(dr["Neck"]) ? "YES" : "No";
        }


        DataTable dtShrinkage;
        private void tab2Load()
        {
            gridShrinkage.IsEditingReadOnly = false;

            string sqlShrinkage = $@"select [ID]      ,[No]
      ,[Location]=case when Location='T' then 'TOP'
                               when Location='I' then 'INNER'
                               when Location='O' then 'OUTER'
                               when Location='B' then 'BOTTOM' end
      ,[Type]      ,[BeforeWash]      ,[SizeSpec]      ,[AfterWash1]      ,[Shrinkage1]      ,[AfterWash2]      ,[Shrinkage2]      ,[AfterWash3]      ,[Shrinkage3]
from[SampleGarmentTest_Detail_Shrinkage] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}
order by Seq,Location desc";

            DBProxy.Current.Select(null, sqlShrinkage, out dtShrinkage);
            listControlBindingSource1.DataSource = null;
            listControlBindingSource1.DataSource = dtShrinkage;
            int i = 4;
            if (dtShrinkage.Select("Location = 'TOP'").Length == 0)
            {
                panel4.Visible = false;
                i--;
            }
            if (dtShrinkage.Select("Location = 'INNER'").Length == 0)
            {
                panel5.Visible = false;
                i--;
            }
            if (dtShrinkage.Select("Location = 'OUTER'").Length == 0)
            {
                panel6.Visible = false;
                i--;
            }
            if (dtShrinkage.Select("Location = 'BOTTOM'").Length == 0)
            {
                panel7.Visible = false;
                i--;
            }
            flowLayoutPanel1.Height = 36 * i;

            numTopS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='TOP'"));
            numTopS2.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S2 from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='TOP'"));
            numTopL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select sum(L) from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='TOP'"));
            numTwisTingTop.Value = numTopL.Value.Empty() ? 0 : (((numTopS1.Value + numTopS2.Value) / 2) / numTopL.Value) * 100;

            numInnerS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='INNER'"));
            numInnerS2.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S2 from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='INNER'"));
            numInnerL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select sum(L) from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='INNER'"));
            numTwisTingInner.Value = numInnerL.Value.Empty() ? 0 : (((numInnerS1.Value + numInnerS2.Value) / 2) / numInnerL.Value) * 100;

            numOuterS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='OUTER'"));
            numOuterS2.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S2 from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='OUTER'"));
            numOuterL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select sum(L) from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='OUTER'"));
            numTwisTingOuter.Value = numOuterL.Value.Empty() ? 0 : (((numOuterS1.Value + numOuterS2.Value) / 2) / numOuterL.Value) * 100;

            numBottomS1.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select S1 from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='BOTTOM'"));
            numBottomL.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"select L   from[SampleGarmentTest_Detail_Twisting] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='BOTTOM'"));
            numTwisTingBottom.Value = numBottomL.Value.Empty() ? 0 : numBottomS1.Value / numBottomL.Value * 100;
        }


        DataTable dtApperance;
        private void tab3Load()
        {
            gridAppearance.IsEditingReadOnly = false;

            string sqlApperance = $@"select * from[SampleGarmentTest_Detail_Appearance] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} order by seq";

            DBProxy.Current.Select(null, sqlApperance, out dtApperance);
            listControlBindingSource2.DataSource = null;
            listControlBindingSource2.DataSource = dtApperance;
        }

        #endregion



        #region Edit存檔


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {

                #region 更新 SampleGarmentTest_Detail 1
                string ReportDate = MyUtility.Check.Empty(txtReportDate.Value) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(txtReportDate.Value)).ToString("yyyy/MM/dd");
                string updateGarmentTest_Detail = $@"
                        update SampleGarmentTest_Detail set
                            ReportDate = iif('{ReportDate}'='',null,'{ReportDate}'),
                            Colour =  '{txtColour.Text}',
                            Result = '{comboResult.SelectedValue}',
                            SizeCode='{txtSize.Text}',
                            Remark =  '{txtRemark.Text}',
                            LineDry =  '{rdbtnLine.Checked}',
                            TumbleDry =  '{rdbtnTumble.Checked}',
                            HandWash =  '{rdbtnHand.Checked}',
                            Temperature =  {comboTemperature.Text},
                            Machine =  '{comboMachineModel.Text}',
                            Composition =  '{txtFibreComposition.Text}',
                            Neck ='{MyUtility.Convert.GetString(comboNeck.Text).EqualString("YES")}'
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
                DBProxy.Current.Execute(null, $"update SampleGarmentTest_Detail set Editname = '{Sci.Env.User.UserID}',EditDate = getdate() where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}");
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

                //Seq=4的Row全部都可以編輯
                gridAppearance.Rows[3].DefaultCellStyle.ForeColor = Color.Red;

                btnEncode.Enabled = false;
                btnAmend.Enabled = false;
            }

            this.EditMode = !this.EditMode;
            rdbtnLine.Enabled = this.EditMode;
            rdbtnTumble.Enabled = this.EditMode;
            rdbtnHand.Enabled = this.EditMode;
            btnPDF.Enabled = !this.EditMode;
            gridShrinkage.ReadOnly = !this.EditMode;
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

  merge [SampleGarmentTest_Detail_Shrinkage] t
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

select * from [SampleGarmentTest_Detail_Shrinkage]  where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]}";

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
update [SampleGarmentTest_Detail_Twisting] set S1={numTopS1.Value},S2={numTopS2.Value},L={numTopL.Value},Twisting={numTwisTingTop.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='TOP'
update [SampleGarmentTest_Detail_Twisting] set S1={numInnerS1.Value},S2={numInnerS2.Value},L={numInnerL.Value},Twisting={numTwisTingInner.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='INNER'
update [SampleGarmentTest_Detail_Twisting] set S1={numOuterS1.Value},S2={numOuterS2.Value},L={numOuterL.Value},Twisting={numTwisTingOuter.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='OUTER'
update [SampleGarmentTest_Detail_Twisting] set S1={numBottomS1.Value},L={numBottomL.Value},Twisting={numTwisTingBottom.Value} where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} and Location ='BOTTOM'
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
  merge [SampleGarmentTest_Detail_Appearance] t
  using #tmp s
  on s.id = t.id and s.no = t.no and s.seq = t.seq
  when matched then
  update set
	t.[Type]  = s.[Type],
	t.[Wash1]  = s.[Wash1],
    t.[Wash2]	= s.[Wash2]	,
    t.[Wash3]	= s.[Wash3],
    t.[Comment]	= s.[Comment]
	;

select * from [SampleGarmentTest_Detail_Appearance]  where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} order by seq";

            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource2.DataSource, string.Empty, savetab2Apperance, out dtApperance);
            if (!result)
            {
                ShowErr(result);
                return;
            }
        }

        #endregion

        /// <summary>
        /// Size欄位右鍵開啟視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSize_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string sql = $@"
                SELECT SizeCode
                FROM  Style_SizeCode ss
                INNER JOIN Style s
                ON ss.StyleUkey=s.Ukey
                WHERE s.ID='{style}' AND s.SeasonID='{season}' AND s.BrandID='{brand}'
";
                SelectItem item = new SelectItem(sql, "30,30", null);
                DialogResult dresult = item.ShowDialog();
                if (dresult == DialogResult.Cancel) return;
                txtSize.Text = item.GetSelectedString();
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {

            if (dtApperance.Rows.Count == 0 || dtShrinkage.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
                return;
            }
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_P10_SampleGarmentWash.xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            string sqlShrinkage = $@"select * from[SampleGarmentTest_Detail] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} ";
            DataTable tmp;
            DBProxy.Current.Select(null, sqlShrinkage, out tmp);
            DataRow dr = tmp.Rows[0];

            DateTime? dateSend =MyUtility.Convert.GetDate(dr["SendDate"]);


            //Submit Date
            if (dateSend.HasValue)
                worksheet.Cells[4, 4] = MyUtility.Convert.GetDate(dateSend.Value).Value.Year + "/" + MyUtility.Convert.GetDate(dateSend.Value).Value.Month + "/" + MyUtility.Convert.GetDate(dateSend.Value).Value.Day;
            //ReportDate
            if (!MyUtility.Check.Empty(dr["inspdate"]))
                worksheet.Cells[4, 7] = MyUtility.Convert.GetDate(dr["inspdate"]).Value.Year + "/" + MyUtility.Convert.GetDate(dr["inspdate"]).Value.Month + "/" + MyUtility.Convert.GetDate(dr["inspdate"]).Value.Day;
            //Report  No
            worksheet.Cells[4, 9] = MyUtility.Convert.GetString(dr["ReportNo"]);
            //Brand
            worksheet.Cells[4, 11] = MyUtility.Convert.GetString(MasterRow["BrandID"]);
            
            //Working No
            worksheet.Cells[6, 4] = MyUtility.Convert.GetString(MasterRow["StyleID"]);

            //PO Number 不寫

            //Colour
            worksheet.Cells[6, 10] = MyUtility.Convert.GetString(dr["Colour"]);

            //Article No
            worksheet.Cells[7, 4] = MyUtility.Convert.GetString(MasterRow["Article"]);

            //Quantity 不寫

            //Size
            worksheet.Cells[7, 10] = MyUtility.Convert.GetString(dr["SizeCode"]);

            //Style Name
            worksheet.Cells[8, 4] = MyUtility.GetValue.Lookup($"select StyleName from Style with(nolock) where id = '{MasterRow["Styleid"]}' and seasonid = '{MasterRow["seasonid"]}' and brandid = '{MasterRow["brandid"]}'");

            //Delivery Date 不寫
            //Customer No 不寫

            //Line Dry
            worksheet.Cells[11, 4] = rdbtnLine.Checked ? "V" : string.Empty;
            //Tumble Dry
            worksheet.Cells[12, 4] = rdbtnTumble.Checked ? "V" : string.Empty;
            //Hand Wash
            worksheet.Cells[13, 4] = rdbtnHand.Checked ? "V" : string.Empty;
            //Temperature
            worksheet.Cells[11, 8] = comboTemperature.Text + "˚C ";
            //Machine Model
            worksheet.Cells[12, 8] = comboMachineModel.Text;
            //Fibre Composition
            worksheet.Cells[13, 8] = txtFibreComposition.Text;

            #region 最下面 Signature
            if (MyUtility.Convert.GetString(dr["Result"]).EqualString("P"))
            {
                worksheet.Cells[73, 4] = "V";
            }
            else
            {
                worksheet.Cells[73, 6] = "V";
            }

            #region 插入圖片與Technician名字
            string sql_cmd = $@"select p.name,[SignaturePic] = s.PicPath + t.SignaturePic
                                    from Technician t WITH (NOLOCK)
                                    inner join pass1 p WITH (NOLOCK) on t.ID = p.ID  
                                    outer apply (select PicPath from system) s 
                                    where t.ID = '{Deatilrow["Technician"]}'";
            DataRow drTechnicianInfo;
            string technicianName = string.Empty;
            string picSource = string.Empty;
            Image img = null;
            Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[12, 2];

            if (MyUtility.Check.Seek(sql_cmd, out drTechnicianInfo))
            {
                technicianName = drTechnicianInfo["name"].ToString();
                picSource = drTechnicianInfo["SignaturePic"].ToString();
            }
            //Name
            worksheet.Cells[74, 9] = technicianName;

            //插入圖檔
            if (!MyUtility.Check.Empty(picSource))
            {
                if (File.Exists(picSource))
                {
                    img = Image.FromFile(picSource);
                    Microsoft.Office.Interop.Excel.Range cellPic = worksheet.Cells[72, 9];

                    worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellPic.Left, cellPic.Top, 100, 24);
                }
            }
            #endregion

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
            string type= MyUtility.Convert.GetString(dtApperance.Select("seq=4")[0]["type"]);
            worksheet.Cells[64, 3] = type;

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

            #region Streched Neck Opening is OK according to size spec?
            if ((bool)dr["Neck"])
            {
                worksheet.Cells[40, 9] = "V";
            }
            else
            {
                worksheet.Cells[40, 11] = "V";
            }
            #endregion

            #region %
            if (dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
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

                worksheet.Cells[54, 4] = numTwisTingOuter.Text + "%";
                worksheet.Cells[54, 7] = numOuterS1.Value;
                worksheet.Cells[54, 9] = numOuterS2.Value;
                worksheet.Cells[54, 11] = numOuterL.Value;
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
            //先BOTTOM
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
            strFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P10_SampleGarmentWash");
            strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P10_SampleGarmentWash", Sci.Production.Class.PDFFileNameExtension.PDF);
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


        private void btnEncode_Click(object sender, EventArgs e)
        {
            DualResult dr = DBProxy.Current.Execute(null, $"Update [SampleGarmentTest_Detail] set Status='Confirmed' where id = '{this.MasterRow["ID"]}' AND No='{this.Deatilrow["No"]}'");
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

            DualResult dr = DBProxy.Current.Execute(null, $"Update [SampleGarmentTest_Detail] set Status='New'  where id = '{this.MasterRow["ID"]}' AND No='{this.Deatilrow["No"]}'");
            if (!dr)
            {
                ShowErr(dr);
            }
            else
            {
                btnenable();
            }
        }

        private void gridAppearance_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // 非第 0 column 則取消編輯動作
            if (e.ColumnIndex == 0 && e.RowIndex!=3)
            {
                e.Cancel = true;
            }
        }
    }
}
