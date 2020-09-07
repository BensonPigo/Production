using Ict;
using Ict.Win;
using Sci.Data;
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

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P10_ToReport : Win.Tems.QueryForm
    {
        private readonly DataRow Deatilrow;
        private readonly DataRow MasterRow;
        private readonly DataTable dtApperance;
        private readonly DataTable dtShrinkage;
        private readonly DataTable dtFGWT;
        private readonly DataTable dtFGPT;
        private readonly bool IsNewData;
        private readonly P10Data data;

        /// <inheritdoc/>
        public P10_ToReport(DataRow masterrow, DataRow deatilrow, bool isNewData, DataTable dataApperance, DataTable dataShrinkage, DataTable dataFGWT, DataTable dataFGPT, P10Data p10Data)
        {
            this.InitializeComponent();
            this.MasterRow = masterrow;
            this.Deatilrow = deatilrow;
            this.dtApperance = dataApperance;
            this.dtShrinkage = dataShrinkage;
            this.dtFGWT = dataFGWT;
            this.dtFGPT = dataFGPT;
            this.IsNewData = isNewData;
            this.data = p10Data;
        }

        private void BtnToPDF_Click(object sender, EventArgs e)
        {
            string reportType = this.GetReportType();
            this.ToPDF(reportType);
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            string reportType = this.GetReportType();
            this.ToExcel(reportType);
        }

        private string GetReportType()
        {
            string type = string.Empty;
            if (this.radioWash18.Checked)
            {
                type = "Wash18";
            }
            else if (this.radioWash20.Checked)
            {
                type = "Wash20";
            }
            else if (this.radioPhysical.Checked)
            {
                type = "Physical";
            }

            return type;
        }

        private void ToPDF(string reportType)
        {
            if (reportType == "Wash18")
            {
                this.PrintWash18("ToPDF");
            }

            if (reportType == "Wash20")
            {
                this.PrintWash20("ToPDF");
            }

            if (reportType == "Physical")
            {
                this.PrintPhysical("ToPDF");
            }
        }

        private void ToExcel(string reportType)
        {
            if (reportType == "Wash18")
            {
                this.PrintWash18("ToExcel");
            }

            if (reportType == "Wash20")
            {
                this.PrintWash20("ToExcel");
            }

            if (reportType == "Physical")
            {
                this.PrintPhysical("ToExcel");
            }
        }

        private void PrintWash18(string to)
        {
            if (this.dtApperance.Rows.Count == 0 || this.dtShrinkage.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P10_SampleGarmentWash.xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            string sqlShrinkage = $@"select * from[SampleGarmentTest_Detail] where id = {this.Deatilrow["ID"]} and No = {this.Deatilrow["No"]} ";
            DataTable tmp;
            DBProxy.Current.Select(null, sqlShrinkage, out tmp);
            DataRow dr = tmp.Rows[0];

            DateTime? dateSend = MyUtility.Convert.GetDate(dr["SendDate"]);

            // Submit Date
            if (dateSend.HasValue)
            {
                worksheet.Cells[4, 4] = MyUtility.Convert.GetDate(dateSend.Value).Value.Year + "/" + MyUtility.Convert.GetDate(dateSend.Value).Value.Month + "/" + MyUtility.Convert.GetDate(dateSend.Value).Value.Day;
            }

            // ReportDate
            if (!MyUtility.Check.Empty(dr["inspdate"]))
            {
                worksheet.Cells[4, 7] = MyUtility.Convert.GetDate(dr["inspdate"]).Value.Year + "/" + MyUtility.Convert.GetDate(dr["inspdate"]).Value.Month + "/" + MyUtility.Convert.GetDate(dr["inspdate"]).Value.Day;
            }

            // Report  No
            worksheet.Cells[4, 9] = MyUtility.Convert.GetString(dr["ReportNo"]);

            // Brand
            worksheet.Cells[4, 11] = MyUtility.Convert.GetString(this.MasterRow["BrandID"]);

            // Working No
            worksheet.Cells[6, 4] = MyUtility.Convert.GetString(this.MasterRow["StyleID"]);

            // PO Number
            worksheet.Cells[6, 8] = "SAMPLE";

            // Colour
            worksheet.Cells[6, 10] = MyUtility.Convert.GetString(dr["Colour"]);

            // Article No
            worksheet.Cells[7, 4] = MyUtility.Convert.GetString(this.MasterRow["Article"]);

            // Quantity  沒有ArrivedQty欄位 因此為空
            worksheet.Cells[7, 8] = string.Empty;

            // Size
            worksheet.Cells[7, 10] = MyUtility.Convert.GetString(dr["SizeCode"]);

            // Style Name
            worksheet.Cells[8, 4] = MyUtility.GetValue.Lookup($"select StyleName from Style with(nolock) where id = '{this.MasterRow["Styleid"]}' and seasonid = '{this.MasterRow["seasonid"]}' and brandid = '{this.MasterRow["brandid"]}'");

            // Delivery Date
            worksheet.Cells[8, 8] = string.Empty;

            // Customer No 不寫
            worksheet.Cells[8, 10] = "SAMPLE";

            // Line Dry
            worksheet.Cells[11, 4] = this.data.RdbtnLine ? "V" : string.Empty;

            // Tumble Dry
            worksheet.Cells[12, 4] = this.data.RdbtnTumble ? "V" : string.Empty;

            // Hand Wash
            worksheet.Cells[13, 4] = this.data.RdbtnHand ? "V" : string.Empty;

            // Temperature
            worksheet.Cells[11, 8] = this.data.ComboTemperature + "˚C ";

            // Machine Model
            worksheet.Cells[12, 8] = this.data.ComboMachineModel;

            // Fibre Composition
            worksheet.Cells[13, 8] = this.data.TxtFibreComposition;

            /*開始塞PDF，注意！！！！！！！！！！！！！！！！！！！！！！！！有新舊資料區分，最簡單的方式寫if else

            新舊資料差異：新資料沒有Seq = 9 ，只到8，所以新資料 69行不見，下面的往上推
             */

            #region 舊資料

            if (!this.IsNewData)
            {
                #region 最下面 Signature
                if (MyUtility.Convert.GetString(dr["Result"]).EqualString("Pass"))
                {
                    worksheet.Cells[72, 4] = "V";
                }
                else
                {
                    worksheet.Cells[72, 6] = "V";
                }

                #region 插入圖片與Technician名字
                if (to == "ToPDF")
                {
                    string sql_cmd = $@"select p.name,[SignaturePic] = s.PicPath + t.SignaturePic
                                            from Technician t WITH (NOLOCK)
                                            inner join pass1 p WITH (NOLOCK) on t.ID = p.ID  
                                            outer apply (select PicPath from system) s 
                                            where t.ID = '{this.Deatilrow["Technician"]}'";
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

                    // Name
                    worksheet.Cells[73, 9] = technicianName;

                    // 插入圖檔
                    if (!MyUtility.Check.Empty(picSource))
                    {
                        if (File.Exists(picSource))
                        {
                            img = Image.FromFile(picSource);
                            Microsoft.Office.Interop.Excel.Range cellPic = worksheet.Cells[71, 9];

                            worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellPic.Left, cellPic.Top, 100, 24);
                        }
                    }
                }

                if (to == "ToExcel")
                {
                    worksheet.Cells[70, 8] = string.Empty;
                }
                #endregion

                #endregion

                #region After Wash Appearance Check list
                string tmpAR;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash1"]);

                worksheet.Cells[61, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Type"]);

                // 大約21個字換行
                int widhthBase = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Type"]).Length / 20;

                worksheet.get_Range("61:61", Type.Missing).RowHeight = 19 * widhthBase;

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

                string strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Comment"]);
                this.RowHeight(worksheet, 61, strComment);
                worksheet.Cells[61, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["Comment"]);
                this.RowHeight(worksheet, 62, strComment);
                worksheet.Cells[62, 10] = strComment;

                worksheet.Cells[63, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["Type"]);
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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["Comment"]);
                this.RowHeight(worksheet, 63, strComment);
                worksheet.Cells[63, 10] = strComment;

                worksheet.Cells[64, 3] = this.dtApperance.Select("seq=4")[0]["Type"].ToString(); // type;

                // 大約21個字換行
                int widhthBase2 = this.dtApperance.Select("seq=4")[0]["Type"].ToString().Length / 20;

                worksheet.get_Range("64:64", Type.Missing).RowHeight = 19 * widhthBase2;

                if ((
                        worksheet.get_Range("61:61", Type.Missing).RowHeight
                        + worksheet.get_Range("62:62", Type.Missing).RowHeight
                        + worksheet.get_Range("63:63", Type.Missing).RowHeight
                        + worksheet.get_Range("64:64", Type.Missing).RowHeight) < 81)
                {
                    worksheet.get_Range("61:61", Type.Missing).RowHeight = worksheet.get_Range("61:61", Type.Missing).RowHeight > 28 ? worksheet.get_Range("61:61", Type.Missing).RowHeight : 28;
                    worksheet.get_Range("62:62", Type.Missing).RowHeight = 28;
                    worksheet.get_Range("63:63", Type.Missing).RowHeight = 28;
                    worksheet.get_Range("64:64", Type.Missing).RowHeight = worksheet.get_Range("64:64", Type.Missing).RowHeight > 28 ? worksheet.get_Range("64:64", Type.Missing).RowHeight : 28;
                }

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["Comment"]);
                this.RowHeight(worksheet, 64, strComment);
                worksheet.Cells[64, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["Comment"]);
                this.RowHeight(worksheet, 65, strComment);
                worksheet.Cells[65, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["Comment"]);
                this.RowHeight(worksheet, 66, strComment);
                worksheet.Cells[66, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["Comment"]);
                this.RowHeight(worksheet, 67, strComment);
                worksheet.Cells[67, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["Comment"]);
                this.RowHeight(worksheet, 68, strComment);
                worksheet.Cells[68, 10] = strComment;

                worksheet.Cells[69, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=9")[0]["Type"]);
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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=9")[0]["Comment"]);
                this.RowHeight(worksheet, 69, strComment);
                worksheet.Cells[69, 10] = strComment;
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
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    worksheet.Cells[56, 4] = this.data.NumTwisTingBottom + "%";
                    worksheet.Cells[56, 7] = this.data.NumBottomS1.Value;
                    worksheet.Cells[56, 9] = this.data.NumBottomL.Value;
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
                    worksheet.Cells[54, 4] = this.data.NumTwisTingOuter + "%";
                    worksheet.Cells[54, 7] = this.data.NumOuterS1.Value;
                    worksheet.Cells[54, 9] = this.data.NumOuterS2.Value;
                    worksheet.Cells[54, 11] = this.data.NumOuterL.Value;
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
                    worksheet.Cells[52, 4] = this.data.NumTwisTingInner + "%";
                    worksheet.Cells[52, 7] = this.data.NumInnerS1.Value;
                    worksheet.Cells[52, 9] = this.data.NumInnerS2.Value;
                    worksheet.Cells[52, 11] = this.data.NumInnerL.Value;
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
                    worksheet.Cells[50, 4] = this.data.NumTwisTingTop + "%";
                    worksheet.Cells[50, 7] = this.data.NumTopS1.Value;
                    worksheet.Cells[50, 9] = this.data.NumTopS2.Value;
                    worksheet.Cells[50, 11] = this.data.NumTopL.Value;
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

                // 先BOTTOM
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[44, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Waistband (relax)'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Waistband (relax)'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[45, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Hip Width'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Hip Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[46, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Thigh Width'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Thigh Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[47, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Side Seam'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Side Seam'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[48, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Leg Opening'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Leg Opening'")[0][i+1];
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
                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[34, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Chest Width'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Chest Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[35, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Sleeve Width'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Sleeve Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[36, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Sleeve Length'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Sleeve Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[37, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Back Length'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Back Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[38, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Hem Opening'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Hem Opening'")[0][i + 1];
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
                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[26, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Chest Width'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Chest Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[27, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Sleeve Width'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Sleeve Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[28, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Sleeve Length'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Sleeve Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[29, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Back Length'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Back Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[30, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Hem Opening'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Hem Opening'")[0][i + 1];
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
                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[18, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Chest Width'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Chest Width'")[0][i+1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[19, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Width'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Sleeve Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[20, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Length'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Sleeve Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[21, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Back Length'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Back Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[22, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Hem Opening'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Hem Opening'")[0][i + 1];
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
                if (MyUtility.Convert.GetString(dr["Result"]).EqualString("Pass"))
                {
                    worksheet.Cells[72, 4] = "V";
                }
                else
                {
                    worksheet.Cells[72, 6] = "V";
                }

                #region 插入圖片與Technician名字
                if (to == "ToPDF")
                {
                    string sql_cmd = $@"select p.name,[SignaturePic] = s.PicPath + t.SignaturePic
                                        from Technician t WITH (NOLOCK)
                                        inner join pass1 p WITH (NOLOCK) on t.ID = p.ID  
                                        outer apply (select PicPath from system) s 
                                        where t.ID = '{this.Deatilrow["Technician"]}'";
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

                    // Name
                    worksheet.Cells[73, 9] = technicianName;

                    // 插入圖檔
                    if (!MyUtility.Check.Empty(picSource))
                    {
                        if (File.Exists(picSource))
                        {
                            img = Image.FromFile(picSource);
                            Microsoft.Office.Interop.Excel.Range cellPic = worksheet.Cells[71, 9];

                            worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellPic.Left, cellPic.Top, 100, 24);
                        }
                    }
                }

                if (to == "ToExcel")
                {
                    worksheet.Cells[70, 8] = string.Empty;
                }
                #endregion

                #endregion

                #region After Wash Appearance Check list
                string tmpAR;

                tmpAR = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["wash1"]);

                worksheet.Cells[61, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Type"]).ToString();

                // 大約21個字換行
                int widhthBase = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Type"]).ToString().Length / 20;

                worksheet.get_Range("61:61", Type.Missing).RowHeight = 19 * widhthBase;

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

                string strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=1")[0]["Comment"]);
                this.RowHeight(worksheet, 61, strComment);
                worksheet.Cells[61, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=2")[0]["Comment"]);
                this.RowHeight(worksheet, 62, strComment);
                worksheet.Cells[62, 10] = strComment;

                worksheet.Cells[63, 3] = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["Type"]).ToString(); // type;

                // 大約21個字換行
                int widhthBase2 = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["Type"]).ToString().Length / 20;

                worksheet.get_Range("63:63", Type.Missing).RowHeight = 19 * widhthBase2;

                if ((
                        worksheet.get_Range("61:61", Type.Missing).RowHeight
                        + worksheet.get_Range("62:62", Type.Missing).RowHeight
                        + worksheet.get_Range("63:63", Type.Missing).RowHeight) < 81)
                {
                    worksheet.get_Range("61:61", Type.Missing).RowHeight = worksheet.get_Range("61:61", Type.Missing).RowHeight > 28 ? worksheet.get_Range("61:61", Type.Missing).RowHeight : 28;
                    worksheet.get_Range("62:62", Type.Missing).RowHeight = 28;
                    worksheet.get_Range("63:63", Type.Missing).RowHeight = worksheet.get_Range("63:63", Type.Missing).RowHeight > 28 ? worksheet.get_Range("63:63", Type.Missing).RowHeight : 28;
                }

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=3")[0]["Comment"]);
                this.RowHeight(worksheet, 63, strComment);
                worksheet.Cells[63, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=4")[0]["Comment"]);
                this.RowHeight(worksheet, 64, strComment);
                worksheet.Cells[64, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=5")[0]["Comment"]);
                this.RowHeight(worksheet, 65, strComment);
                worksheet.Cells[65, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=6")[0]["Comment"]);
                this.RowHeight(worksheet, 66, strComment);
                worksheet.Cells[66, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=7")[0]["Comment"]);
                this.RowHeight(worksheet, 67, strComment);
                worksheet.Cells[67, 10] = strComment;

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

                strComment = MyUtility.Convert.GetString(this.dtApperance.Select("seq=8")[0]["Comment"]);
                this.RowHeight(worksheet, 68, strComment);
                worksheet.Cells[68, 10] = strComment;

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
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    worksheet.Cells[56, 4] = this.data.NumTwisTingBottom + "%";
                    worksheet.Cells[56, 7] = this.data.NumBottomS1.Value;
                    worksheet.Cells[56, 9] = this.data.NumBottomL.Value;
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
                    worksheet.Cells[54, 4] = this.data.NumTwisTingOuter + "%";
                    worksheet.Cells[54, 7] = this.data.NumOuterS1.Value;
                    worksheet.Cells[54, 9] = this.data.NumOuterS2.Value;
                    worksheet.Cells[54, 11] = this.data.NumOuterL.Value;
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
                    worksheet.Cells[52, 4] = this.data.NumTwisTingInner + "%";
                    worksheet.Cells[52, 7] = this.data.NumInnerS1.Value;
                    worksheet.Cells[52, 9] = this.data.NumInnerS2.Value;
                    worksheet.Cells[52, 11] = this.data.NumInnerL.Value;
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
                    worksheet.Cells[50, 4] = this.data.NumTwisTingTop + "%";
                    worksheet.Cells[50, 7] = this.data.NumTopS1.Value;
                    worksheet.Cells[50, 9] = this.data.NumTopS2.Value;
                    worksheet.Cells[50, 11] = this.data.NumTopL.Value;
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

                // 先BOTTOM
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[44, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Waistband (relax)'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Waistband (relax)'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[45, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Hip Width'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Hip Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[46, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Thigh Width'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Thigh Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[47, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Side Seam'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Side Seam'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[48, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'BOTTOM'and type ='Leg Opening'", i + 1);

                        // dtShrinkage.Select("Location = 'BOTTOM'and type ='Leg Opening'")[0][i+1];
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
                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[34, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Chest Width'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Chest Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[35, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Sleeve Width'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Sleeve Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[36, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Sleeve Length'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Sleeve Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[37, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Back Length'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Back Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[38, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'OUTER'and type ='Hem Opening'", i + 1);

                        // dtShrinkage.Select("Location = 'OUTER'and type ='Hem Opening'")[0][i + 1];
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
                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[26, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Chest Width'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Chest Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[27, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Sleeve Width'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Sleeve Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[28, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Sleeve Length'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Sleeve Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[29, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Back Length'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Back Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[30, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'INNER'and type ='Hem Opening'", i + 1);

                        // dtShrinkage.Select("Location = 'INNER'and type ='Hem Opening'")[0][i + 1];
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
                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[18, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Chest Width'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Chest Width'")[0][i+1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[19, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Width'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Sleeve Width'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[20, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Sleeve Length'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Sleeve Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[21, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Back Length'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Back Length'")[0][i + 1];
                    }

                    for (int i = 4; i < this.dtShrinkage.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[22, i] = this.AddShrinkageUnit(this.dtShrinkage, @"Location = 'TOP'and type ='Hem Opening'", i + 1);

                        // dtShrinkage.Select("Location = 'TOP'and type ='Hem Opening'")[0][i + 1];
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

            if (to == "ToPDF")
            {
                string strFileName = string.Empty;
                string strPDFFileName = string.Empty;
                strFileName = Class.MicrosoftFile.GetName("Quality_P10_SampleGarmentWash");
                strPDFFileName = Class.MicrosoftFile.GetName("Quality_P10_SampleGarmentWash", Class.PDFFileNameExtension.PDF);
                objApp.ActiveWorkbook.SaveAs(strFileName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);

                if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
                    Process.Start(startInfo);
                }
            }

            if (to == "ToExcel")
            {
                string strExcelName = Class.MicrosoftFile.GetName("Quality_P10_SampleGarmentWash");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(objApp);
                strExcelName.OpenFile();
            }
            #endregion
        }

        private void PrintWash20(string to)
        {
            if (this.dtFGWT.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P10_FGWT.xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            // objApp.Visible = true;

            #region 插入圖片與Technician名字

            if (to == "ToPDF")
            {
                string sql_cmd = $@"select p.name,[SignaturePic] = s.PicPath + t.SignaturePic
                                        from Technician t WITH (NOLOCK)
                                        inner join pass1 p WITH (NOLOCK) on t.ID = p.ID  
                                        outer apply (select PicPath from system) s 
                                        where t.ID = '{this.Deatilrow["Technician"]}'
";
                DataRow drTechnicianInfo;
                string technicianName = string.Empty;
                string picSource = string.Empty;
                Image img = null;

                if (MyUtility.Check.Seek(sql_cmd, out drTechnicianInfo))
                {
                    technicianName = drTechnicianInfo["name"].ToString();
                    picSource = drTechnicianInfo["SignaturePic"].ToString();
                }

                // Name
                worksheet.Cells[30, 7] = technicianName;

                // 插入圖檔
                if (!MyUtility.Check.Empty(picSource))
                {
                    if (File.Exists(picSource))
                    {
                        img = Image.FromFile(picSource);
                        Microsoft.Office.Interop.Excel.Range cellPic = worksheet.Cells[28, 6];

                        worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellPic.Left, cellPic.Top, 100, 24);
                    }
                }
            }

            if (to == "ToExcel")
            {
                worksheet.Cells[27, 6] = string.Empty;
            }

            #endregion

            // 若為QA 10產生則顯示New Development Testing ( V )，若為QA P10產生則顯示1st Bulk Testing ( V )
            worksheet.Cells[4, 1] = "New Development Testing ( V )";

            worksheet.Cells[5, 1] = "adidas Article No.: " + MyUtility.Convert.GetString(this.MasterRow["Article"]);
            worksheet.Cells[5, 3] = "adidas Working No.: " + MyUtility.Convert.GetString(this.MasterRow["StyleID"]);
            worksheet.Cells[5, 4] = "adidas Model No.: " + MyUtility.GetValue.Lookup($"SELECT StyleName FROM Style WHERE ID='{this.MasterRow["StyleID"]}'");

            worksheet.Cells[6, 4] = "LO to Factory: " + this.data.TxtLotoFactory;

            if (this.data.TxtReportDate.HasValue)
            {
                worksheet.Cells[8, 1] = "Date: " + this.data.TxtReportDate.Value.ToString("yyyy/MM/dd");
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

            worksheet.get_Range($"B12", $"B{ this.dtFGWT.Rows.Count + 11}").Merge(false);

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

            if (to == "ToPDF")
            {
                string strFileName = string.Empty;
                string strPDFFileName = string.Empty;
                strFileName = Class.MicrosoftFile.GetName("QA_P10_FGWT");
                strPDFFileName = Class.MicrosoftFile.GetName("QA_P10_FGWT", Class.PDFFileNameExtension.PDF);

                objApp.ActiveWorkbook.SaveAs(strFileName);
                objApp.Quit();
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(objApp);

                if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
                    Process.Start(startInfo);
                }
            }

            if (to == "ToExcel")
            {
                string strExcelName = Class.MicrosoftFile.GetName("QA_P10_FGWT");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(objApp);
                strExcelName.OpenFile();
            }
            #endregion
        }

        private void PrintPhysical(string to)
        {
            if (this.dtFGPT.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P10_FGPT.xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            // objApp.Visible = true;

            #region 插入圖片與Technician名字
            if (to == "ToPDF")
            {
                string sql_cmd = $@"select p.name,[SignaturePic] = s.PicPath + t.SignaturePic
                                        from Technician t WITH (NOLOCK)
                                        inner join pass1 p WITH (NOLOCK) on t.ID = p.ID  
                                        outer apply (select PicPath from system) s 
                                        where t.ID = '{this.Deatilrow["Technician"]}'
";
                DataRow drTechnicianInfo;
                string technicianName = string.Empty;
                string picSource = string.Empty;
                Image img = null;

                if (MyUtility.Check.Seek(sql_cmd, out drTechnicianInfo))
                {
                    technicianName = drTechnicianInfo["name"].ToString();
                    picSource = drTechnicianInfo["SignaturePic"].ToString();

                }

                // Name
                worksheet.Cells[159, 7] = technicianName;

                // 插入圖檔
                if (!MyUtility.Check.Empty(picSource))
                {
                    if (File.Exists(picSource))
                    {
                        img = Image.FromFile(picSource);
                        Microsoft.Office.Interop.Excel.Range cellPic = worksheet.Cells[157, 6];

                        worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellPic.Left, cellPic.Top, 100, 24);
                    }
                }

            }

            if (to == "ToExcel")
            {
                worksheet.Cells[156, 6] = string.Empty;
            }
            #endregion

            // 若為QA 10產生則顯示New Development Testing ( V )，若為QA P04產生則顯示1st Bulk Testing ( V )
            worksheet.Cells[4, 1] = "1st Bulk Testing ( V )";

            worksheet.Cells[5, 1] = "adidas Article No.: " + MyUtility.Convert.GetString(this.MasterRow["Article"]);
            worksheet.Cells[5, 3] = "adidas Working No.: " + MyUtility.Convert.GetString(this.MasterRow["StyleID"]);
            worksheet.Cells[5, 4] = "adidas Model No.: " + MyUtility.GetValue.Lookup($"SELECT StyleName FROM Style WHERE ID='{this.MasterRow["StyleID"]}'");

            worksheet.Cells[6, 4] = "LO to Factory: " + this.data.TxtLotoFactory;

            if (this.data.TxtReportDate.HasValue)
            {
                worksheet.Cells[8, 1] = "Date: " + this.data.TxtReportDate.Value.ToString("yyyy/MM/dd");
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

            if (to == "ToPDF")
            {
                string strFileName = string.Empty;
                string strPDFFileName = string.Empty;
                strFileName = Class.MicrosoftFile.GetName("QA_P10_FGPT");
                strPDFFileName = Class.MicrosoftFile.GetName("QA_P10_FGPT", Class.PDFFileNameExtension.PDF);

                objApp.ActiveWorkbook.SaveAs(strFileName);
                objApp.Quit();
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(objApp);

                if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
                    Process.Start(startInfo);
                }
            }

            if (to == "ToExcel")
            {
                string strExcelName = Class.MicrosoftFile.GetName("QA_P10_FGPT");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(objApp);
                strExcelName.OpenFile();
            }
            #endregion

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
        /// <param name="dt"></param>
        /// <param name="strFilter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
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
                    strValie = strValie + "%";
                }
            }

            return strValie;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    /// <inheritdoc/>
    public class P10Data
    {
        public DateTime? TxtReportDate { get; set; }

        public decimal? NumArriveQty { get; set; }

        public string TxtSize { get; set; }

        public bool RdbtnLine { get; set; }

        public bool RdbtnTumble { get; set; }

        public bool RdbtnHand { get; set; }

        public string ComboTemperature { get; set; }

        public string ComboMachineModel { get; set; }

        public string TxtFibreComposition { get; set; }

        public string ComboNeck { get; set; }

        public string NumTwisTingBottom { get; set; }

        public decimal? NumBottomS1 { get; set; }

        public decimal? NumBottomL { get; set; }

        public string NumTwisTingOuter { get; set; }

        public decimal? NumOuterS1 { get; set; }

        public decimal? NumOuterS2 { get; set; }

        public decimal? NumOuterL { get; set; }

        public string NumTwisTingInner { get; set; }

        public decimal? NumInnerS1 { get; set; }

        public decimal? NumInnerS2 { get; set; }

        public decimal? NumInnerL { get; set; }

        public string NumTwisTingTop { get; set; }

        public decimal? NumTopS1 { get; set; }

        public decimal? NumTopS2 { get; set; }

        public decimal? NumTopL { get; set; }

        public string TxtLotoFactory { get; set; }

    }
}
