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
    public partial class P04_ToReport : Win.Tems.QueryForm
    {
        private readonly DataRow Deatilrow;
        private readonly DataRow MasterRow;
        private readonly DataTable dtApperance;
        private readonly DataTable dtShrinkage;
        private readonly DataTable dtFGWT;
        private readonly DataTable dtFGPT;
        private readonly bool IsNewData;
        private readonly P04Data data;

        /// <inheritdoc/>
        public P04_ToReport(DataRow masterrow, DataRow deatilrow, bool isNewData, DataTable dataApperance, DataTable dataShrinkage, DataTable dataFGWT, DataTable dataFGPT, P04Data p04Data)
        {
            this.InitializeComponent();
            this.MasterRow = masterrow;
            this.Deatilrow = deatilrow;
            this.dtApperance = dataApperance;
            this.dtShrinkage = dataShrinkage;
            this.dtFGWT = dataFGWT;
            this.dtFGPT = dataFGPT;
            this.IsNewData = isNewData;
            this.data = p04Data;
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P04_GarmentWash.xltx");
            objApp.DisplayAlerts = false; // 設定Excel的警告視窗是否彈出
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表
            if (this.data.DateSubmit.HasValue)
            {
                worksheet.Cells[4, 4] = MyUtility.Convert.GetDate(this.data.DateSubmit.Value).Value.Year + "/" + MyUtility.Convert.GetDate(this.data.DateSubmit.Value).Value.Month + "/" + MyUtility.Convert.GetDate(this.data.DateSubmit.Value).Value.Day;
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
            worksheet.Cells[8, 8] = MyUtility.Convert.GetDecimal(this.data.NumArriveQty.Value);

            // if (!MyUtility.Check.Empty(Deatilrow["SendDate"]))
            //    worksheet.Cells[8, 4] = MyUtility.Convert.GetDate(Deatilrow["SendDate"]).Value.Year + "/" + MyUtility.Convert.GetDate(Deatilrow["SendDate"]).Value.Month + "/" + MyUtility.Convert.GetDate(Deatilrow["SendDate"]).Value.Day;
            string sendDate = Convert.ToDateTime(MyUtility.GetValue.Lookup($"SELECT BuyerDelivery FROM Orders WHERE ID = '{this.MasterRow["OrderID"].ToString()}'")).ToShortDateString();
            worksheet.Cells[8, 4] = sendDate;
            worksheet.Cells[8, 10] = MyUtility.Convert.GetString(this.data.TxtSize);

            worksheet.Cells[11, 4] = this.data.RdbtnLine ? "V" : string.Empty;
            worksheet.Cells[12, 4] = this.data.RdbtnTumble ? "V" : string.Empty;
            worksheet.Cells[13, 4] = this.data.RdbtnHand ? "V" : string.Empty;
            worksheet.Cells[11, 8] = this.data.ComboTemperature + "˚C ";
            worksheet.Cells[12, 8] = this.data.ComboMachineModel;
            worksheet.Cells[13, 8] = this.data.TxtFibreComposition;

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

                if (to == "ToPDF")
                {
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
                }

                if (to == "ToExcel")
                {
                    worksheet.Cells[70, 8] = string.Empty;
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

                if (this.data.ComboNeck.EqualString("Yes"))
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
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    DataTable dt = this.dtShrinkage.Select("Location = 'BOTTOM'").CopyToDataTable();

                    // 超過5個測量點則新增行數
                    if (dt.Rows.Count > 5)
                    {
                        for (int i = 0; i < dt.Rows.Count - 5; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A48:A48", Type.Missing).EntireRow;
                            rng.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        }
                    }

                    // 依不同品牌/套裝塞入資料
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            worksheet.Cells[44 + r, c] = this.AddShrinkageUnit_18(dt, r, c);
                        }
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
                    DataTable dt = this.dtShrinkage.Select("Location = 'OUTER'").CopyToDataTable();

                    // 超過5個測量點則新增行數
                    if (dt.Rows.Count > 5)
                    {
                        for (int i = 0; i < dt.Rows.Count - 5; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A38:A38", Type.Missing).EntireRow;
                            rng.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        }
                    }

                    // 依不同品牌/套裝塞入資料
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            worksheet.Cells[34 + r, c] = this.AddShrinkageUnit_18(dt, r, c);
                        }
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
                    DataTable dt = this.dtShrinkage.Select("Location = 'INNER'").CopyToDataTable();

                    // 超過5個測量點則新增行數
                    if (dt.Rows.Count > 5)
                    {
                        for (int i = 0; i < dt.Rows.Count - 5; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A30:A30", Type.Missing).EntireRow;
                            rng.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        }
                    }

                    // 依不同品牌/套裝塞入資料
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            worksheet.Cells[26 + r, c] = this.AddShrinkageUnit_18(dt, r, c);
                        }
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
                    DataTable dt = this.dtShrinkage.Select("Location = 'TOP'").CopyToDataTable();

                    // 超過5個測量點則新增行數
                    if (dt.Rows.Count > 5)
                    {
                        for (int i = 0; i < dt.Rows.Count - 5; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A22:A22", Type.Missing).EntireRow;
                            rng.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        }
                    }

                    // 依不同品牌/套裝塞入資料
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            worksheet.Cells[18 + r, c] = this.AddShrinkageUnit_18(dt, r, c);
                        }
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

                if (to == "ToPDF")
                {
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
                }

                if (to == "ToExcel")
                {
                    worksheet.Cells[70, 8] = string.Empty;
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

                if (this.data.ComboNeck.EqualString("Yes"))
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
                    worksheet.Cells[56, 4] = this.data.NumTwisTingBottom + "%";
                    worksheet.Cells[56, 7] = this.data.NumBottomS1;
                    worksheet.Cells[56, 9] = this.data.NumBottomL;
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
                if (this.dtShrinkage.Select("Location = 'BOTTOM'").Length > 0)
                {
                    DataTable dt = this.dtShrinkage.Select("Location = 'BOTTOM'").CopyToDataTable();

                    // 超過5個測量點則新增行數
                    if (dt.Rows.Count > 5)
                    {
                        for (int i = 0; i < dt.Rows.Count - 5; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A48:A48", Type.Missing).EntireRow;
                            rng.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        }
                    }

                    // 依不同品牌/套裝塞入資料
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            worksheet.Cells[44 + r, c] = this.AddShrinkageUnit_18(dt, r, c);
                        }
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
                    DataTable dt = this.dtShrinkage.Select("Location = 'OUTER'").CopyToDataTable();

                    // 超過5個測量點則新增行數
                    if (dt.Rows.Count > 5)
                    {
                        for (int i = 0; i < dt.Rows.Count - 5; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A38:A38", Type.Missing).EntireRow;
                            rng.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        }
                    }

                    // 依不同品牌/套裝塞入資料
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            worksheet.Cells[34 + r, c] = this.AddShrinkageUnit_18(dt, r, c);
                        }
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
                    DataTable dt = this.dtShrinkage.Select("Location = 'INNER'").CopyToDataTable();

                    // 超過5個測量點則新增行數
                    if (dt.Rows.Count > 5)
                    {
                        for (int i = 0; i < dt.Rows.Count - 5; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A30:A30", Type.Missing).EntireRow;
                            rng.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        }
                    }

                    // 依不同品牌/套裝塞入資料
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            worksheet.Cells[26 + r, c] = this.AddShrinkageUnit_18(dt, r, c);
                        }
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
                    DataTable dt = this.dtShrinkage.Select("Location = 'TOP'").CopyToDataTable();

                    // 超過5個測量點則新增行數
                    if (dt.Rows.Count > 5)
                    {
                        for (int i = 0; i < dt.Rows.Count - 5; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = worksheet.get_Range("A22:A22", Type.Missing).EntireRow;
                            rng.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        }
                    }

                    // 依不同品牌/套裝塞入資料
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int c = 3; c < dt.Columns.Count; c++)
                        {
                            worksheet.Cells[18 + r, c] = this.AddShrinkageUnit_18(dt, r, c);
                        }
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

            string fileProcessName = "FGWT" + "_"
                + this.MasterRow["seasonid"].ToString() + "_" + this.MasterRow["StyleID"].ToString() + "_" + this.MasterRow["Article"].ToString();
            if (to == "ToPDF")
            {
                string strFileName = Class.MicrosoftFile.GetName(fileProcessName);
                string strPDFFileName = Class.MicrosoftFile.GetName(fileProcessName, Class.PDFFileNameExtension.PDF);
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
                string strExcelName = Class.MicrosoftFile.GetName(fileProcessName);
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P04_FGWT.xltx");
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
                                        where t.ID = '{this.Deatilrow["inspector"]}'
                                        and t.GarmentTest=1
";
                DataRow drTechnicianInfo;
                string technicianName = string.Empty;
                string picSource = string.Empty;
                Image img = null;

                if (MyUtility.Check.Seek(sql_cmd, out drTechnicianInfo))
                {
                    technicianName = drTechnicianInfo["name"].ToString();
                    picSource = drTechnicianInfo["SignaturePic"].ToString();

                    // Name
                    worksheet.Cells[31, 7] = technicianName;

                    // 插入圖檔
                    if (!MyUtility.Check.Empty(picSource))
                    {
                        if (File.Exists(picSource))
                        {
                            img = Image.FromFile(picSource);
                            Microsoft.Office.Interop.Excel.Range cellPic = worksheet.Cells[29, 7];

                            worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellPic.Left, cellPic.Top, 100, 24);
                        }
                    }
                }
                else
                {
                    worksheet.Cells[31, 7] = MyUtility.Convert.GetString(this.Deatilrow["Showname"]);
                }
            }

            if (to == "ToExcel")
            {
                worksheet.Cells[27, 6] = string.Empty;
            }

            #endregion

            // 若為QA 10產生則顯示New Development Testing ( V )，若為QA P04產生則顯示1st Bulk Testing ( V )
            worksheet.Cells[4, 3] = "1st Bulk Testing ( V )";

            worksheet.Cells[5, 1] = "adidas Article No.: " + MyUtility.Convert.GetString(this.MasterRow["Article"]);
            worksheet.Cells[5, 3] = "adidas Working No.: " + MyUtility.Convert.GetString(this.MasterRow["StyleID"]);
            worksheet.Cells[5, 4] = "adidas Model No.: " + MyUtility.GetValue.Lookup($"SELECT StyleName FROM Style WHERE ID='{this.MasterRow["StyleID"]}'");

            worksheet.Cells[6, 1] = "T1 Supplier Ref.: " + MyUtility.GetValue.Lookup($"SELECT FactoryID FROM Orders WHERE ID='{this.MasterRow["OrderID"]}'");
            worksheet.Cells[6, 3] = "T1 Factory Name: " + MyUtility.GetValue.Lookup($"SELECT o.BrandAreaCode FROM GarmentTest g INNER JOIN Orders o ON g.OrderID = o.ID WHERE g.OrderID='{this.MasterRow["OrderID"]}'");
            worksheet.Cells[6, 4] = "LO to Factory: " + this.data.TxtLotoFactory;

            if (this.data.DateSubmit.HasValue)
            {
                worksheet.Cells[8, 1] = "Date: " + this.data.DateSubmit.Value.ToString("yyyy/MM/dd");
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
                    if ((dr["BeforeWash"] != DBNull.Value && dr["AfterWash"] != DBNull.Value && dr["Shrinkage"] != DBNull.Value)
                        || MyUtility.Convert.GetBool(dr["IsInPercentage"]))
                    {
                        // TestDetail  % 或Range% 視作相同
                        if (MyUtility.Convert.GetString(dr["TestDetail"]).Contains("%"))
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
                worksheet.Cells[startRowIndex, 5] = MyUtility.Convert.GetString(dr["TestDetail"]) == "Range%" ? "%" : MyUtility.Convert.GetString(dr["TestDetail"]);

                // adidas pass
                worksheet.Cells[startRowIndex, 6] = MyUtility.Convert.GetString(dr["Result"]);

                startRowIndex++;
            }

            #region Save & Show Excel

            string fileProcessName = "FGWT" + "_"
              + this.MasterRow["seasonid"].ToString() + "_" + this.MasterRow["StyleID"].ToString() + "_" + this.MasterRow["Article"].ToString();
            if (to == "ToPDF")
            {
                string strFileName = Class.MicrosoftFile.GetName(fileProcessName);
                string strPDFFileName = Class.MicrosoftFile.GetName(fileProcessName, Class.PDFFileNameExtension.PDF);

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
                string strFileName = Class.MicrosoftFile.GetName(fileProcessName);
                objApp.ActiveWorkbook.SaveAs(strFileName);
                objApp.Quit();
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(objApp);
                strFileName.OpenFile();
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P04_FGPT.xltx");
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
                                        where t.ID = '{this.Deatilrow["inspector"]}'
                                        and t.GarmentTest=1
";
                DataRow drTechnicianInfo;
                string technicianName = string.Empty;
                string picSource = string.Empty;
                Image img = null;

                if (MyUtility.Check.Seek(sql_cmd, out drTechnicianInfo))
                {
                    technicianName = drTechnicianInfo["name"].ToString();
                    picSource = drTechnicianInfo["SignaturePic"].ToString();

                    // Name
                    worksheet.Cells[159, 7] = technicianName;

                    // 插入圖檔
                    if (!MyUtility.Check.Empty(picSource))
                    {
                        if (File.Exists(picSource))
                        {
                            img = Image.FromFile(picSource);
                            Microsoft.Office.Interop.Excel.Range cellPic = worksheet.Cells[157, 7];

                            worksheet.Shapes.AddPicture(picSource, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, cellPic.Left, cellPic.Top, 100, 24);
                        }
                    }
                }
                else
                {
                    worksheet.Cells[159, 7] = MyUtility.Convert.GetString(this.Deatilrow["Showname"]);
                }
            }

            if (to == "ToExcel")
            {
                worksheet.Cells[156, 6] = string.Empty;
            }
            #endregion

            // 若為QA 10產生則顯示New Development Testing ( V )，若為QA P04產生則顯示1st Bulk Testing ( V )
            worksheet.Cells[4, 3] = "1st Bulk Testing ( V )";

            worksheet.Cells[5, 1] = "adidas Article No.: " + MyUtility.Convert.GetString(this.MasterRow["Article"]);
            worksheet.Cells[5, 3] = "adidas Working No.: " + MyUtility.Convert.GetString(this.MasterRow["StyleID"]);
            worksheet.Cells[5, 4] = "adidas Model No.: " + MyUtility.GetValue.Lookup($"SELECT StyleName FROM Style WHERE ID='{this.MasterRow["StyleID"]}'");

            worksheet.Cells[6, 1] = "T1 Supplier Ref.: " + MyUtility.GetValue.Lookup($"SELECT FactoryID FROM Orders WHERE ID='{this.MasterRow["OrderID"]}'");
            worksheet.Cells[6, 3] = "T1 Factory Name: " + MyUtility.GetValue.Lookup($"SELECT o.BrandAreaCode FROM GarmentTest g INNER JOIN Orders o ON g.OrderID = o.ID WHERE g.OrderID='{this.MasterRow["OrderID"]}'");
            worksheet.Cells[6, 4] = "LO to Factory: " + this.data.TxtLotoFactory;

            if (this.data.DateSubmit.HasValue)
            {
                worksheet.Cells[8, 1] = "Date: " + this.data.DateSubmit.Value.ToString("yyyy/MM/dd");
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
            worksheet.Cells[150, 5] = MyUtility.Convert.GetString(testName_3.FirstOrDefault()["TestDetail"]) == "Range%" ? "%" : MyUtility.Convert.GetString(testName_3.FirstOrDefault()["TestDetail"]);

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
                worksheet.Cells[startRowIndex, 5] = MyUtility.Convert.GetString(dr["TestDetail"]) == "Range%" ? "%" : MyUtility.Convert.GetString(dr["TestDetail"]);

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
                worksheet.Cells[startRowIndex, 5] = MyUtility.Convert.GetString(dr["TestDetail"]) == "Range%" ? "%" : MyUtility.Convert.GetString(dr["TestDetail"]);

                // adidas pass
                worksheet.Cells[startRowIndex, 6] = MyUtility.Convert.GetString(dr["Result"]);

                startRowIndex++;
            }

            #region Save & Show Excel

            string fileProcessName = "FGPT" + "_"
              + this.MasterRow["seasonid"].ToString() + "_" + this.MasterRow["StyleID"].ToString() + "_" + this.MasterRow["Article"].ToString();
            if (to == "ToPDF")
            {
                string strFileName = Class.MicrosoftFile.GetName(fileProcessName);
                string strPDFFileName = Class.MicrosoftFile.GetName(fileProcessName, Class.PDFFileNameExtension.PDF);

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
                string strExcelName = Class.MicrosoftFile.GetName(fileProcessName);
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
        /// <param name="dt">dt</param>
        /// <param name="strFilter">strFilter</param>
        /// <param name="count">count</param>
        /// <returns>string</returns>
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

        /// <summary>
        /// 如果欄位是Shrinkage 就增加%單位符號
        /// </summary>
        /// <param name="dt">dt</param>
        /// <param name="row">row</param>
        /// <param name="columns">columns</param>
        /// <returns>string</returns>
        private string AddShrinkageUnit_18(DataTable dt, int row, int columns)
        {
            string strValie = string.Empty;
            if (dt.Rows.Count > 0)
            {
                strValie = dt.Rows[row][columns].ToString();
                if (((string.Compare(dt.Columns[columns].ColumnName, "Shrinkage1", true) == 0) ||
                    (string.Compare(dt.Columns[columns].ColumnName, "Shrinkage2", true) == 0) ||
                    (string.Compare(dt.Columns[columns].ColumnName, "Shrinkage3", true) == 0)) &&
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public class P04Data
    {
        /// <inheritdoc/>
        public DateTime? DateSubmit { get; set; }

        /// <inheritdoc/>
        public decimal? NumArriveQty { get; set; }

        /// <inheritdoc/>
        public string TxtSize { get; set; }

        /// <inheritdoc/>
        public bool RdbtnLine { get; set; }

        /// <inheritdoc/>
        public bool RdbtnTumble { get; set; }

        /// <inheritdoc/>
        public bool RdbtnHand { get; set; }

        /// <inheritdoc/>
        public string ComboTemperature { get; set; }

        /// <inheritdoc/>
        public string ComboMachineModel { get; set; }

        /// <inheritdoc/>
        public string TxtFibreComposition { get; set; }

        /// <inheritdoc/>
        public string ComboNeck { get; set; }

        /// <inheritdoc/>
        public string NumTwisTingBottom { get; set; }

        /// <inheritdoc/>
        public decimal? NumBottomS1 { get; set; }

        /// <inheritdoc/>
        public decimal? NumBottomL { get; set; }

        /// <inheritdoc/>
        public string NumTwisTingOuter { get; set; }

        /// <inheritdoc/>
        public decimal? NumOuterS1 { get; set; }

        /// <inheritdoc/>
        public decimal? NumOuterS2 { get; set; }

        /// <inheritdoc/>
        public decimal? NumOuterL { get; set; }

        /// <inheritdoc/>
        public string NumTwisTingInner { get; set; }

        /// <inheritdoc/>
        public decimal? NumInnerS1 { get; set; }

        /// <inheritdoc/>
        public decimal? NumInnerS2 { get; set; }

        /// <inheritdoc/>
        public decimal? NumInnerL { get; set; }

        /// <inheritdoc/>
        public string NumTwisTingTop { get; set; }

        /// <inheritdoc/>
        public decimal? NumTopS1 { get; set; }

        /// <inheritdoc/>
        public decimal? NumTopS2 { get; set; }

        /// <inheritdoc/>
        public decimal? NumTopL { get; set; }

        /// <inheritdoc/>
        public string TxtLotoFactory { get; set; }
    }
}
