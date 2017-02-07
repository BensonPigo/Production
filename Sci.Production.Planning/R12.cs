using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using System.Data.SqlClient;
using Sci.Production.Report;
using Sci.Data;
using Msg = Sci.MyUtility.Msg;
using Sci.Production.Report.GSchemas;
using System.Runtime.InteropServices;

namespace Sci.Production.Planning
{
    public partial class R12 : Sci.Win.Tems.PrintForm
    {
        DataTable dtPrint, dtData, tmpData1, tmpData2, tmpData3, tmpData3_SUM, All_tmpData3, All_tmpData3_SUM, tmpData4, All_tmpData4, tmpStyleDetail, tmpOrderDetail;
        string SqlData1, SqlData2, SqlData3, SqlData3_SUM, All_SqlData3, All_SqlData3_SUM, SqlData4, All_SqlData4, SqlStyleDetail, SqlOrderDetail;//, strType1 = "";
        decimal StandardTms = 0; //int intRowsStart = 2;

        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            EditMode = true;
            print.Visible = false;
            GetStandardTms();
        }

        //欄位檢核
        protected override bool ValidateInput()
        {
            if (txtbrand1.Text.Trim() == "")
            {
                ShowErr("Brand can't be  blank");
                return false;
            }
            if (txtseason1.Text.Trim() == "")
            {
                ShowErr("Season can't be  blank");
                return false;
            }
            return true;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(tmpData1.Rows.Count);

            if (tmpData1.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R12.Matrix.xltx"); //預先開啟excel app
            objApp.Visible = false; string title;
            #region [sheet1]
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1];

            #region 抬頭
            if (rbRegionNo.Checked)
            {
                worksheet.Range["A3"].Value2 = "Region No";
                title = "Season = " + txtseason1.Text + "      , Brand = " + txtbrand1.Text + "  , Grouping by Region No";
            }
            else
            {
                worksheet.Range["A3"].Value2 = "Fty Code";
                title = "Season = " + txtseason1.Text + "      , Brand = " + txtbrand1.Text + "  , Grouping by Factory Code";
            }

            worksheet.Range["A2"].Value2 = title;
            worksheet.Range["C4"].Value2 = txtseason1.Text + "_historical data";
            #endregion

            string FirstColumn = string.Empty, Country = string.Empty;
            int startRow = 5;
            int count = 1;
                for (int i = 0; i < tmpData3.Rows.Count; i++)
                {
                    if (FirstColumn == tmpData3.Rows[i][0].ToString() && Country == tmpData3.Rows[i]["Country"].ToString()) continue;

                    FirstColumn = tmpData3.Rows[i][0].ToString();
                    Country = tmpData3.Rows[i]["Country"].ToString();

                    int SmvStart = 5 + (count - 1) * 8;
                    int SmvEnd = SmvStart + 8;
                    for (startRow = SmvStart; startRow < SmvEnd; startRow++)
                    {
                        worksheet.Cells[startRow, 1] = FirstColumn;
                        worksheet.Cells[startRow, 2] = Country;
                        worksheet.Cells[startRow, 4] = GenSmvColumn(startRow);
                    }
                    count++;
                }
 
            #region ALL
            decimal fractions, numerator;
            int AllEnd = startRow + 8;
            count = 1;
                numerator = Convert.ToDecimal(All_tmpData3_SUM.Rows[0][0].ToString());  //分母
                for (int x = startRow; x < AllEnd; x++)
                {
                    worksheet.Cells[x, 1] = "All";
                    worksheet.Cells[x, 4] = GenSmvColumn(x);

                    if (count == 1 && All_tmpData3.Select("SMVEFFX = 'A'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'A'")[0][1].ToString());  //分子
                        worksheet.Cells[x, 3] = fractions / numerator;  //SMV_RATIO

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 2 && All_tmpData3.Select("SMVEFFX = 'B'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'B'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 3 && All_tmpData3.Select("SMVEFFX = 'C'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'C'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 4 && All_tmpData3.Select("SMVEFFX = 'D'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'D'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 5 && All_tmpData3.Select("SMVEFFX = 'E'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'E'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 6 && All_tmpData3.Select("SMVEFFX = 'F'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'F'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 7 && All_tmpData3.Select("SMVEFFX = 'G'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'G'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 8 && All_tmpData3.Select("SMVEFFX = 'H'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'H'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else
                    {
                        worksheet.Cells[x, 3] = 0;

                        //EFFICIENCY_ base on VOLUME RANGE
                        worksheet.Cells[x, 5] = 0;
                        worksheet.Cells[x, 6] = 0;
                        worksheet.Cells[x, 7] = 0;
                        worksheet.Cells[x, 8] = 0;
                        worksheet.Cells[x, 9] = 0;
                        worksheet.Cells[x, 10] = 0;
                    }
                    count++;
                }
            #endregion

            #region SMV_RATIO塞值
            startRow = 5;
            count = 1;
            int row;
            string filter, QtyEFFX, EFFIC;
                for (int i = 0; i < tmpData3.Rows.Count; i++)
                {
                    if ((FirstColumn != tmpData3.Rows[i][0].ToString() || Country != tmpData3.Rows[i]["Country"].ToString()) && i != 0)
                    {
                        count++;
                        startRow = 5 + (count - 1) * 8;
                    }
                    FirstColumn = tmpData3.Rows[i][0].ToString();
                    Country = tmpData3.Rows[i]["Country"].ToString();

                    row = startRow + GetAddRow(tmpData3.Rows[i]["SMVEFFX"].ToString());

                    if (rbRegionNo.Checked)
                        filter = string.Format("RegionNo = '{0}' and Country = '{1}'", FirstColumn, Country);
                    else
                        filter = string.Format("Factory = '{0}' and Country = '{1}'", FirstColumn, Country);

                    fractions = Convert.ToDecimal(tmpData3.Rows[i][3].ToString());  //分子
                    numerator = (tmpData3_SUM.Select(filter).Length == 0) ? 0 : Convert.ToDecimal(tmpData3_SUM.Select(filter)[0][2].ToString());  //分母
                    worksheet.Cells[row, 3] = (MyUtility.Check.Empty(numerator)) ? 0m : fractions / numerator;  //SMV_RATIO

                    //EFFICIENCY_ base on VOLUME RANGE
                    filter += " and SMVEFFX ='" + tmpData3.Rows[i]["SMVEFFX"].ToString() + "'";
                    QtyEFFX = (tmpData4.Select(filter).Length == 0) ? "" : tmpData4.Select(filter)[0]["QtyEFFX"].ToString();
                    EFFIC = (tmpData4.Select(filter).Length == 0) ? "" : tmpData4.Select(filter)[0]["EFFIC"].ToString();
                    if (QtyEFFX == "")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "AA")
                    {
                        worksheet.Cells[row, 5] = EFFIC;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "BB")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = EFFIC;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "CC")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = EFFIC;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "DD")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = EFFIC;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "EE")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = EFFIC;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "FF")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = EFFIC;
                    }

                }
            #endregion

            #endregion
            #region [sheet2] Style Detail
           
            Microsoft.Office.Interop.Excel.Worksheet worksheet2 = objApp.ActiveWorkbook.Worksheets[2];            // 取得工作表

            BeginInvoke(() => { this.ShowWaitMessage("Create EXCEL Style Detail, please wait..."); });
            MyUtility.Excel.CopyToXls(tmpStyleDetail, "", "Planning_R12.Matrix.xltx", 1, true, null, null, true, worksheet2, false);// 將datatable copy to excel
            BeginInvoke(() => { this.HideWaitMessage(); });
            #endregion

            #region [sheet3] Order Detail
            Microsoft.Office.Interop.Excel.Worksheet worksheet3 = objApp.ActiveWorkbook.Worksheets[3];
            BeginInvoke(() => { this.ShowWaitMessage("Create EXCEL Order Detail, please wait..."); });
            MyUtility.Excel.CopyToXls(tmpOrderDetail, "", "Planning_R12.Matrix.xltx", 1, true, null, null, true, worksheet3, false); // 將datatable copy to excel
                 BeginInvoke(() => { this.HideWaitMessage(); });
            #endregion
            objApp.Visible = true;
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;
            if (dtPrint != null) dtPrint.Rows.Clear();
            string where = " ", GroupBy, select;
            IList<SqlParameter> spList = new List<SqlParameter>();

            #region tmpData1
            if (txtbrand1.Text != "") where += string.Format(" and O.BrandID = '{0}' ", txtbrand1.Text);
          //  if (txtcountry1.TextBox1.Text != "") where += string.Format(" and F.CountryID = '{0}'  ", txtcountry1.TextBox1.Text);
            if (txtseason1.Text != "") where += string.Format(" and SeasonID =  '{0}' ", txtseason1.Text);
            SqlData1 = string.Format(@"Select O.ID , O.CPU , O.Cpu * {0} / 60 as SMV , O.CPU * {0} as TMS 
, O.FactoryID , FB.BrandAreaCode , 
		                                    O.Qty , O.StyleID , F.CountryID
                                    From Orders O WITH (NOLOCK)
                                    Left Outer Join Factory_BrandDefinition FB WITH (NOLOCK) 
on FB.ID = O.FactoryID 
                                    Left Join Factory F WITH (NOLOCK) on O.FactoryID = F.ID
                                    Where 1=1 {1}", StandardTms, where);

            BeginInvoke(() => { this.ShowWaitMessage("Wait – Style,Order Data capture, data may be many, please wait (Step 1/5)"); });
            result = DBProxy.Current.Select("", SqlData1, spList, out tmpData1);
            BeginInvoke(() => { this.HideWaitMessage(); });
            if (!result) return result;
            if (tmpData1 == null || tmpData1.Rows.Count == 0) return new DualResult(false, "Data not found.");
            #endregion

            #region tmpData2
            SqlData2 = string.Format(@"select tmpData1.ID as OrderID , tmpData1.CPU as CPU
, tmpData1.SMV as SMV, tmpData1.TMS as TMS, tmpData1.FactoryID as Factory, 
	                                        tmpData1.BrandAreaCode as AGCCode, tmpData1.Qty as [Order Qty]
, tmpData1.StyleID as Style, tmpData1.CountryID as FactoryCountry, 
SewingOutput_Detail.QAQty as ProdQty
, Round(3600 / iif(tmpData1.TMS * Round(SewingOutput.ManPower * SewingOutput_Detail.WorkHour ,1) = 0,null
,tmpData1.TMS * Round(SewingOutput.ManPower * SewingOutput_Detail.WorkHour ,1)), 0)  as StardQty 
,CASE WHEN SMV < 40 THEN 'A' WHEN SMV >= 40 and SMV < 50  THEN 'B' 
WHEN SMV >= 50 and SMV < 60  THEN 'C' WHEN SMV >= 60 and SMV < 70  THEN 'D'
WHEN SMV >= 70 and SMV < 80  THEN 'E' WHEN SMV >= 80 and SMV < 90  THEN 'F'	
WHEN SMV >= 90 and SMV < 100 THEN 'G'  ELSE 'H' END as SMVEFFX
                                        from 
                                        ({0}) tmpData1
                                        Left Join SewingOutput_Detail WITH (NOLOCK) on OrderID = tmpData1.ID
                                        Left Join SewingOutput WITH (NOLOCK) on SewingOutput.ID = SewingOutput_Detail.ID", SqlData1);

            BeginInvoke(() => { this.ShowWaitMessage("Wait – By Order, Factory Finishing details (Step 2/5)"); });
            result = DBProxy.Current.Select("", SqlData2, spList, out tmpData2);
            BeginInvoke(() => { this.HideWaitMessage(); });
            if (!result) return result;
            #endregion

            #region tmpData3
            if (rbRegionNo.Checked)
            {
                select = " tmpData2.AGCCode as RegionNo ";
                GroupBy = " tmpData2.AGCCode ";
            }
            else
            {
                select = " tmpData2.Factory as Factory ";
                GroupBy = " tmpData2.Factory ";
            }

            SqlData3 = string.Format(@"select {1}, tmpData2.FactoryCountry as Country , tmpData2.SMVEFFX 
, count(*) as SUM_SMVEFFX_COUNT
                                    from ({0}) tmpData2
                                    group by {2}, tmpData2.FactoryCountry, tmpData2.SMVEFFX
                                    order by {2}, tmpData2.FactoryCountry, tmpData2.SMVEFFX", SqlData2, select, GroupBy);
            SqlData3_SUM = string.Format(@"select {1}, tmpData2.FactoryCountry as Country 
,count(*) as SMVEFFX_COUNT
                                    from ({0}) tmpData2
                                    group by {2}, tmpData2.FactoryCountry
                                    order by {2}, tmpData2.FactoryCountry", SqlData2, select, GroupBy);
            All_SqlData3 = string.Format(@"select  tmpData2.SMVEFFX ,count(*) as SMVEFFX_COUNT
                                    from ({0}) tmpData2
                                    group by   tmpData2.SMVEFFX
                                    order by   tmpData2.SMVEFFX", SqlData2);
            All_SqlData3_SUM = string.Format(@"select  count(*) as SMVEFFX_COUNT
                                    from ({0}) tmpData2", SqlData2);

            BeginInvoke(() => { this.ShowWaitMessage("Wait – Group By Style , Country Finishing details (Step 3/5)"); });
            result = DBProxy.Current.Select("", SqlData3, spList, out tmpData3);
            result = DBProxy.Current.Select("", SqlData3_SUM, spList, out tmpData3_SUM);
            result = DBProxy.Current.Select("", All_SqlData3, spList, out All_tmpData3);
            result = DBProxy.Current.Select("", All_SqlData3_SUM, spList, out All_tmpData3_SUM);
            BeginInvoke(() => { this.HideWaitMessage(); });
            if (!result) return result;
            #endregion

            #region tmpEFFIC
            if (rbRegionNo.Checked)
            {
                select = " tmpData2.AGCCode as RegionNo ";
                GroupBy = " tmpData2.AGCCode ";
            }
            else
            {
                select = " tmpData2.Factory as Factory ";
                GroupBy = " tmpData2.Factory ";
            }

            SqlData4 = string.Format(@"select {1}, tmpData2.FactoryCountry as Country, tmpData2.SMVEFFX 
	                                    ,CASE WHEN SUM(tmpData2.ProdQty) <= 3000 THEN 'AA' 
WHEN SUM(tmpData2.ProdQty) >= 3001 and SUM(tmpData2.ProdQty) <= 5000  THEN 'BB' 
WHEN SUM(tmpData2.ProdQty) >= 5001 and SUM(tmpData2.ProdQty) <= 7000  THEN 'CC' 
WHEN SUM(tmpData2.ProdQty) >= 7001 and SUM(tmpData2.ProdQty) <= 9000  THEN 'DD'  
WHEN SUM(tmpData2.ProdQty) >= 9001 and SUM(tmpData2.ProdQty) <= 10000  THEN 'EE' 
WHEN SUM(tmpData2.ProdQty) >= 10001 THEN 'FF'	 END as QtyEFFX
,SUM(tmpData2.ProdQty) as StyleProdQty, SUM(tmpData2.StardQty) as StyleStardQty
,SUM(tmpData2.ProdQty)/iif(SUM(tmpData2.StardQty)=0,null,SUM(tmpData2.StardQty)) as EFFIC
                                      from  ({0}) tmpData2
                                    group by {2}, tmpData2.FactoryCountry, tmpData2.SMVEFFX
                                    order by {2}, tmpData2.FactoryCountry, tmpData2.SMVEFFX"
                , SqlData2, select, GroupBy);

            All_SqlData4 = string.Format(@"select   tmpData2.SMVEFFX 
,CASE WHEN SUM(tmpData2.ProdQty) <= 3000 THEN 'AA' 
WHEN SUM(tmpData2.ProdQty) >= 3001 and SUM(tmpData2.ProdQty) <= 5000  THEN 'BB' 
WHEN SUM(tmpData2.ProdQty) >= 5001 and SUM(tmpData2.ProdQty) <= 7000  THEN 'CC' 
WHEN SUM(tmpData2.ProdQty) >= 7001 and SUM(tmpData2.ProdQty) <= 9000  THEN 'DD'  
WHEN SUM(tmpData2.ProdQty) >= 9001 and SUM(tmpData2.ProdQty) <= 10000  THEN 'EE' 
WHEN SUM(tmpData2.ProdQty) >= 10001 THEN 'FF'	 END as QtyEFFX
, SUM(tmpData2.ProdQty)/iif(SUM(tmpData2.StardQty)=0,null,SUM(tmpData2.StardQty)) as EFFIC
                                                from  ({0}) tmpData2
                                            group by   tmpData2.SMVEFFX
                                            order by   tmpData2.SMVEFFX", SqlData2);

            BeginInvoke(() => { this.ShowWaitMessage("Wait – Produce tmpEFFIC details (Step 3/5)"); });
            result = DBProxy.Current.Select("", SqlData4, spList, out tmpData4);
            result = DBProxy.Current.Select("", All_SqlData4, spList, out All_tmpData4);
            BeginInvoke(() => { this.HideWaitMessage(); });
            if (!result) return result;
            #endregion

            #region tmpStyleDetail
            if (rbRegionNo.Checked)
                GroupBy = " tmpData2.AGCCode ";
            else
                GroupBy = " tmpData2.Factory ";


            SqlStyleDetail = string.Format(@"select tmpData2.Style as Style,
                           tmpData2.CPU as CPU,
                           tmpData2.SMV as SMV, 
                           tmpData2.AGCCode  as AGC,
                           SUM(tmpData2.ProdQty) as SewingOutput, 
                           SUM(tmpData2.StardQty) as StdOutput,
                           ''AS EfficiencyRange, 
                           tmpData2.FactoryCountry as Country
                         from 
                         ({0}) tmpData2
                         group by tmpData2.Style, tmpData2.CPU, tmpData2.AGCCode, tmpData2.SMV, {1} ,tmpData2.FactoryCountry", SqlData2, GroupBy);

            BeginInvoke(() => { this.ShowWaitMessage("Wait – Finishing Style Detail Data (Step 4/5)"); });
            result = DBProxy.Current.Select("", SqlStyleDetail, spList, out tmpStyleDetail);
             if (!result) return result;
            for (int i = 0; i < tmpStyleDetail.Rows.Count; i++)
            {
                tmpStyleDetail.Rows[i]["EfficiencyRange"] = GenEFFX(tmpStyleDetail.Rows[i]["SewingOutput"].ToString());  //橫向區間#
            }
            BeginInvoke(() => { this.HideWaitMessage(); });
        
            #endregion

            #region tmpOrderDetail
            SqlOrderDetail = string.Format(@"select DISTINCT tmpData2.OrderID as SP#, tmpData2.CPU as CPU
, tmpData2.SMV as SMV, tmpData2.TMS as TMS, tmpData2.Factory as Factory
, tmpData2.AGCCode  as AreaCode, tmpData2.[Order Qty] as [SP# Q'ty]
, tmpData2.Style as Style, tmpData2.ProdQty as SewingOutput, tmpData2.StardQty as StdOutput 
                                            from 
                                            ({0}) tmpData2", SqlData2);

            BeginInvoke(() => { this.ShowWaitMessage("Wait – Finishing Order Detail Data (Step 5/5)"); });
            result = DBProxy.Current.Select("", SqlOrderDetail, spList, out tmpOrderDetail);
            BeginInvoke(() => { this.HideWaitMessage(); });
            if (!result) return result;
            #endregion

            e.Report.ReportDataSource = tmpData1;
               
                 //if (tmpData1 != null && tmpData1.Rows.Count > 0)
                 //{
                 // //   顯示筆數
                 //    SetCount(tmpData1.Rows.Count);
                 //    return  OnToExcel();
                 //}
                 //else
                 //{
                 //    return new DualResult(false, "Data not found.");
                 //}
             
            return result;
        }
       
        
       /* private DualResult transferToExcel()
        {
            string temfile = "", title;
            DualResult result = Result.True;
  
            //string strPath = PrivUtils.getPath_XLT(System.Windows.Forms.Application.StartupPath);
            temfile = Sci.Env.Cfg.XltPathDir + @"\Planning_R12.Matrix.xltx";

            Microsoft.Office.Interop.Excel.Application excel = null;
            try
            {
                if (!(result = PrivUtils.Excels.CreateExcel(temfile, out excel))) return result;

                #region [sheet1]
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                #region 抬頭
                if (rbRegionNo.Checked)
                {
                    worksheet.Range["A3"].Value2 = "Region No";
                    if (txtcountry1.TextBox1.Text != "")
                    {
                        title = "Season = " + txtseason1.Text + "      , Brand = " + txtbrand1.Text + "  , Country = " + txtcountry1.TextBox1.Text + ", Grouping by Region No";
                    }
                    else
                    {
                        title = "Season = " + txtseason1.Text + "      , Brand = " + txtbrand1.Text + "  , Grouping by Region No";
                    }
                }
                else
                {
                    worksheet.Range["A3"].Value2 = "Fty Code";
                    if (txtcountry1.TextBox1.Text != "")
                    {
                        title = "Season = " + txtseason1.Text + "      , Brand = " + txtbrand1.Text + "  , Country = " + txtcountry1.TextBox1.Text + ", Grouping by Factory Code";
                    }
                    else
                    {
                        title = "Season = " + txtseason1.Text + "      , Brand = " + txtbrand1.Text + "  , Grouping by Factory Code";
                    }
                }

                worksheet.Range["A2"].Value2 = title;
                worksheet.Range["C4"].Value2 = txtseason1.Text + "_historical data";
                #endregion

                string FirstColumn = string.Empty, Country = string.Empty;
                int startRow = 5;
                int count = 1;
                for (int i = 0; i < tmpData3.Rows.Count; i++)
                {
                    if (FirstColumn == tmpData3.Rows[i][0].ToString() && Country == tmpData3.Rows[i]["Country"].ToString()) continue;

                    FirstColumn = tmpData3.Rows[i][0].ToString();
                    Country = tmpData3.Rows[i]["Country"].ToString();

                    int SmvStart = 5 + (count - 1) * 8;
                    int SmvEnd = SmvStart + 8;
                    for (startRow = SmvStart; startRow < SmvEnd; startRow++)
                    {
                        worksheet.Cells[startRow, 1] = FirstColumn;
                        worksheet.Cells[startRow, 2] = Country;
                        worksheet.Cells[startRow, 4] = GenSmvColumn(startRow);
                    }
                    count++;
                }

                #region ALL
                decimal fractions, numerator;
                int AllEnd = startRow + 8;
                count = 1;
                numerator = Convert.ToDecimal(All_tmpData3_SUM.Rows[0][0].ToString());  //分母
                for (int x = startRow; x < AllEnd; x++)
                {
                    worksheet.Cells[x, 1] = "All";
                    worksheet.Cells[x, 4] = GenSmvColumn(x);

                    if (count == 1 && All_tmpData3.Select("SMVEFFX = 'A'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'A'")[0][1].ToString());  //分子
                        worksheet.Cells[x, 3] = fractions / numerator;  //SMV_RATIO

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'A'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'A'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 2 && All_tmpData3.Select("SMVEFFX = 'B'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'B'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'B'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'B'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 3 && All_tmpData3.Select("SMVEFFX = 'C'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'C'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'C'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'C'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 4 && All_tmpData3.Select("SMVEFFX = 'D'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'D'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'D'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'D'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 5 && All_tmpData3.Select("SMVEFFX = 'E'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'E'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'E'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'E'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 6 && All_tmpData3.Select("SMVEFFX = 'F'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'F'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'F'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'F'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 7 && All_tmpData3.Select("SMVEFFX = 'G'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'G'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'G'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'G'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else if (count == 8 && All_tmpData3.Select("SMVEFFX = 'H'").Length > 0)
                    {
                        fractions = Convert.ToDecimal(All_tmpData3.Select("SMVEFFX = 'H'")[0][1].ToString());
                        worksheet.Cells[x, 3] = fractions / numerator;

                        #region EFFICIENCY_ base on VOLUME RANGE
                        if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "AA")
                        {
                            worksheet.Cells[x, 5] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "BB")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "CC")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "DD")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "EE")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                            worksheet.Cells[x, 10] = 0;
                        }
                        else if (All_tmpData4.Select("SMVEFFX = 'H'")[0]["QtyEFFX"].ToString() == "FF")
                        {
                            worksheet.Cells[x, 5] = 0;
                            worksheet.Cells[x, 6] = 0;
                            worksheet.Cells[x, 7] = 0;
                            worksheet.Cells[x, 8] = 0;
                            worksheet.Cells[x, 9] = 0;
                            worksheet.Cells[x, 10] = All_tmpData4.Select("SMVEFFX = 'H'")[0]["EFFIC"].ToString();
                        }
                        #endregion

                    }
                    else
                    {
                        worksheet.Cells[x, 3] = 0;

                        //EFFICIENCY_ base on VOLUME RANGE
                        worksheet.Cells[x, 5] = 0;
                        worksheet.Cells[x, 6] = 0;
                        worksheet.Cells[x, 7] = 0;
                        worksheet.Cells[x, 8] = 0;
                        worksheet.Cells[x, 9] = 0;
                        worksheet.Cells[x, 10] = 0;
                    }
                    count++;
                }
                #endregion

                #region SMV_RATIO塞值
                startRow = 5;
                count = 1;
                int row;
                string filter, QtyEFFX, EFFIC;
                for (int i = 0; i < tmpData3.Rows.Count; i++)
                {
                    if ((FirstColumn != tmpData3.Rows[i][0].ToString() || Country != tmpData3.Rows[i]["Country"].ToString()) && i != 0)
                    {
                        count++;
                        startRow = 5 + (count - 1) * 8;
                    }
                    FirstColumn = tmpData3.Rows[i][0].ToString();
                    Country = tmpData3.Rows[i]["Country"].ToString();

                    row = startRow + GetAddRow(tmpData3.Rows[i]["SMVEFFX"].ToString());

                    if (rbRegionNo.Checked)
                        filter = string.Format("RegionNo = '{0}' and Country = '{1}'", FirstColumn, Country);
                    else
                        filter = string.Format("Factory = '{0}' and Country = '{1}'", FirstColumn, Country);

                    fractions = Convert.ToDecimal(tmpData3.Rows[i][3].ToString());  //分子
                    numerator = (tmpData3_SUM.Select(filter).Length==0) ? 0 : Convert.ToDecimal(tmpData3_SUM.Select(filter)[0][2].ToString());  //分母
                    worksheet.Cells[row, 3] = (MyUtility.Check.Empty(numerator)) ? 0m : fractions / numerator;  //SMV_RATIO

                    //EFFICIENCY_ base on VOLUME RANGE
                    filter += " and SMVEFFX ='" + tmpData3.Rows[i]["SMVEFFX"].ToString() + "'";
                    QtyEFFX = (tmpData4.Select(filter).Length == 0) ? "" : tmpData4.Select(filter)[0]["QtyEFFX"].ToString();
                    EFFIC = (tmpData4.Select(filter).Length == 0) ? "" : tmpData4.Select(filter)[0]["EFFIC"].ToString();
                    if (QtyEFFX == "")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "AA")
                    {
                        worksheet.Cells[row, 5] = EFFIC;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "BB")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = EFFIC;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "CC")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = EFFIC;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "DD")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = EFFIC;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "EE")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = EFFIC;
                        worksheet.Cells[row, 10] = 0;
                    }
                    else if (QtyEFFX == "FF")
                    {
                        worksheet.Cells[row, 5] = 0;
                        worksheet.Cells[row, 6] = 0;
                        worksheet.Cells[row, 7] = 0;
                        worksheet.Cells[row, 8] = 0;
                        worksheet.Cells[row, 9] = 0;
                        worksheet.Cells[row, 10] = EFFIC;
                    }

                }
                #endregion

                #endregion

                #region [sheet2] Style Detail 
               
                Microsoft.Office.Interop.Excel.Worksheet worksheet2 = excel.ActiveWorkbook.Worksheets[2];            // 取得工作表
              //  MyUtility.Excel.CopyToXls(tmpStyleDetail, "", "Planning_R12.Matrix.xltx", 3, true, null, temfile);   // 將datatable copy to excel
                BeginInvoke(() => { this.ShowWaitMessage("產生EXCEL檔的Style Detail中，請稍後..."); });
                int intRowsStart = 2;

                //foreach (DataRow dr in tmpStyleDetail.Rows)
                //{
                //    //worksheet2.Cells[intRowsStart, 1] = dr["Style"].ToString();
                //    //worksheet2.Cells[intRowsStart, 2] = dr["CPU"].ToString();
                //    //worksheet2.Cells[intRowsStart, 3] = dr["SMV"].ToString();
                //    //worksheet2.Cells[intRowsStart, 4] = dr["AGC"].ToString();
                //    //worksheet2.Cells[intRowsStart, 5] = dr["實際產量"].ToString();
                //    //worksheet2.Cells[intRowsStart, 6] = dr["標準產量"].ToString();
                //    worksheet2.Cells[intRowsStart, 7] = GenEFFX(dr["實際產量"].ToString());
                //    //worksheet2.Cells[intRowsStart, 8] = dr["Country"].ToString();
                    
                //}


                for (int i = 0; i < tmpStyleDetail.Rows.Count; i++)
                {
                    worksheet2.Cells[intRowsStart, 1] = tmpStyleDetail.Rows[i]["Style"].ToString();
                    worksheet2.Cells[intRowsStart, 2] = tmpStyleDetail.Rows[i]["CPU"].ToString();
                    worksheet2.Cells[intRowsStart, 3] = tmpStyleDetail.Rows[i]["SMV"].ToString();
                    worksheet2.Cells[intRowsStart, 4] = tmpStyleDetail.Rows[i]["AGC"].ToString();
                    worksheet2.Cells[intRowsStart, 5] = tmpStyleDetail.Rows[i]["實際產量"].ToString();
                    worksheet2.Cells[intRowsStart, 6] = tmpStyleDetail.Rows[i]["標準產量"].ToString();
                    worksheet2.Cells[intRowsStart, 7] = GenEFFX(tmpStyleDetail.Rows[i]["實際產量"].ToString());  //橫向區間#
                    worksheet2.Cells[intRowsStart, 8] = tmpStyleDetail.Rows[i]["Country"].ToString();  //國別
                    intRowsStart++;
                }
               
                BeginInvoke(() => { this.HideWaitMessage(); });
                #endregion

                #region [sheet3] Order Detail
                Microsoft.Office.Interop.Excel.Worksheet worksheet3 = excel.ActiveWorkbook.Worksheets[3];

                BeginInvoke(() => { this.ShowWaitMessage("產生EXCEL檔的Order Detail中，請稍後..."); });

                //foreach (DataRow dr2 in tmpOrderDetail.Rows)
                //{
                //    worksheet3.Cells[intRowsStart2, 1] = dr2["SP#"].ToString();
                //    worksheet3.Cells[intRowsStart2, 2] = dr2["CPU"].ToString();
                //    worksheet3.Cells[intRowsStart2, 3] = dr2["SMV"].ToString();
                //    worksheet3.Cells[intRowsStart2, 4] = dr2["TMS"].ToString();
                //    worksheet3.Cells[intRowsStart2, 5] = dr2["Factory"].ToString();
                //    worksheet3.Cells[intRowsStart2, 6] = dr2["AreaCode"].ToString();
                //    worksheet3.Cells[intRowsStart2, 7] = dr2["SP# Q'ty"].ToString();
                //    worksheet3.Cells[intRowsStart2, 8] = dr2["Style"].ToString();
                //    worksheet3.Cells[intRowsStart2, 9] = dr2["實際產量"].ToString();
                //    worksheet3.Cells[intRowsStart2, 10] = dr2["標準產量"].ToString();
                //    intRowsStart2++;
                //}

                for (int i = 0; i < tmpOrderDetail.Rows.Count; i++)
                {
                    worksheet3.Cells[intRowsStart, 1] = tmpOrderDetail.Rows[i]["SP#"].ToString();
                    worksheet3.Cells[intRowsStart, 2] = tmpOrderDetail.Rows[i]["CPU"].ToString();
                    worksheet3.Cells[intRowsStart, 3] = tmpOrderDetail.Rows[i]["SMV"].ToString();
                    worksheet3.Cells[intRowsStart, 4] = tmpOrderDetail.Rows[i]["TMS"].ToString();
                    worksheet3.Cells[intRowsStart, 5] = tmpOrderDetail.Rows[i]["Factory"].ToString();
                    worksheet3.Cells[intRowsStart, 6] = tmpOrderDetail.Rows[i]["AreaCode"].ToString();
                    worksheet3.Cells[intRowsStart, 7] = tmpOrderDetail.Rows[i]["SP# Q'ty"].ToString();
                    worksheet3.Cells[intRowsStart, 8] = tmpOrderDetail.Rows[i]["Style"].ToString();
                    worksheet3.Cells[intRowsStart, 9] = tmpOrderDetail.Rows[i]["實際產量"].ToString();
                    worksheet3.Cells[intRowsStart, 10] = tmpOrderDetail.Rows[i]["標準產量"].ToString();
                    intRowsStart++;
                }

                BeginInvoke(() => { this.HideWaitMessage(); });
                #endregion
                excel.Visible = true;
                return Result.True;
            }
            catch (Exception ex)
            {
                if (null != excel) excel.Quit();
                return new DualResult(false, "Export excel error.", ex);
            }
          
        }
        */

        private void GetStandardTms()
        {
            DualResult result;
            Helper ghelper = new Helper();
            if (!(result = ghelper.getProductionSystem(out dtData)))
            {
                ShowErr("Get StandardTms Fail!!");
                return;
            }
            StandardTms = decimal.Parse(dtData.Rows[0]["StdTMS"].ToString());
        }

        private string GenEFFX(string ProdQty)
        {
            string ReturnValue = "";
            if (ProdQty != "")
            {
                int caseSwitch = Convert.ToInt32(ProdQty);
                if (caseSwitch >= 1 && caseSwitch <= 3000)
                    ReturnValue = "1";
                else if (caseSwitch >= 3001 && caseSwitch <= 5000)
                    ReturnValue = "2";
                else if (caseSwitch >= 5001 && caseSwitch <= 7000)
                    ReturnValue = "3";
                else if (caseSwitch >= 7001 && caseSwitch <= 9000)
                    ReturnValue = "4";
                else if (caseSwitch >= 9001 && caseSwitch <= 10000)
                    ReturnValue = "5";
                else if (caseSwitch >= 10001)
                    ReturnValue = "6";
                else
                    ReturnValue = "";
            }
            return ReturnValue;
        }

        private string GenSmvColumn(int row)
        {
            string ReturnValue = "";
            int switchValue = (row - 5) % 8;
            switch (switchValue)
            {
                case 0:
                    ReturnValue = "40 & below";
                    break;
                case 1:
                    ReturnValue = "40-49";
                    break;
                case 2:
                    ReturnValue = "50-59";
                    break;
                case 3:
                    ReturnValue = "60-69";
                    break;
                case 4:
                    ReturnValue = "70-79";
                    break;
                case 5:
                    ReturnValue = "80-89";
                    break;
                case 6:
                    ReturnValue = "90-99";
                    break;
                case 7:
                    ReturnValue = "100 & above";
                    break;
                default:
                    ReturnValue = "";
                    break;
            }
            return ReturnValue;
        }

        private int GetAddRow(string SMVEFFX)
        {
            int ReturnValue = 0;
            switch (SMVEFFX)
            {
                case "A":
                    ReturnValue = 0;
                    break;
                case "B":
                    ReturnValue = 1;
                    break;
                case "C":
                    ReturnValue = 2;
                    break;
                case "D":
                    ReturnValue = 3;
                    break;
                case "E":
                    ReturnValue = 4;
                    break;
                case "F":
                    ReturnValue = 5;
                    break;
                case "G":
                    ReturnValue = 6;
                    break;
                case "H":
                    ReturnValue = 7;
                    break;
                default:
                    ReturnValue = 0;
                    break;
            }
            return ReturnValue;
        }
    }
}
