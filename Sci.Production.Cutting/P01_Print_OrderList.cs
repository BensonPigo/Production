using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
//using Sci.Trade.Class.Commons;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Cutting
{
    public partial class P01_Print_OrderList : Sci.Win.Tems.QueryForm
    {
        string _id;

        public P01_Print_OrderList(string args)
        {
            this._id = args;
            InitializeComponent();
            EditMode = true;
        }

        private bool ToExcel()
        {
            if (rdCheck1.Checked)
            {
                #region rdCheck1
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01print_EachConsumption", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) return false;
                if (dts.Length < 2) return false;
                if (dts[0].Rows.Count == 0) return false;

                DataRow dr = dts[0].Rows[0];

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_EachConsumptionCuttingCombo.xlt");
                sxrc sxr = new sxrc(xltPath);
                sxr.CopySheet.Add(1, dts.Length - 2);

                for (int sgIdx = 1; sgIdx < dts.Length; sgIdx++)
                {

                    string idxStr = (sgIdx == 1) ? "" : (sgIdx - 1).ToString();
                    string SizeGroup = dts[sgIdx].Rows[0]["SizeGroup"].ToString();
                    string MarkerDownloadID = dts[sgIdx].Compute("MAX(MarkerDownloadID)", "").ToString();

                    dts[sgIdx].Columns.RemoveAt(1);
                    dts[sgIdx].Columns.RemoveAt(0);

                    extra_P01_EachConsumptionCuttingCombo(dts[sgIdx]);

                    sxr.dicDatas.Add(sxr._v + "APPLYNO" + idxStr, dr["APPLYNO"]);
                    sxr.dicDatas.Add(sxr._v + "MARKERNO" + idxStr, dr["MARKERNO"]);
                    sxr.dicDatas.Add(sxr._v + "CUTTINGSP" + idxStr, dr["CUTTINGSP"]);
                    sxr.dicDatas.Add(sxr._v + "ORDERNO" + idxStr, dr["ORDERNO"]);
                    sxr.dicDatas.Add(sxr._v + "STYLENO" + idxStr, dr["STYLENO"]);
                    sxr.dicDatas.Add(sxr._v + "QTY" + idxStr, dr["QTY"]);
                    sxr.dicDatas.Add(sxr._v + "FACTORY" + idxStr, dr["FACTORY"]);
                    sxrc.xltRptTable dt = new sxrc.xltRptTable(dts[sgIdx]);

                    //欄位水平對齊
                    for (int i = 5; i <= dt.Columns.Count - 8; i++)
                    {
                        sxrc.xlsColumnInfo citbl = new sxrc.xlsColumnInfo(i, false, 6, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);
                        dt.lisColumnInfo.Add(citbl);
                    }
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 7, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 6, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 5, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 4, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 3, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count - 1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(dt.Columns.Count, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight));
                    //合併儲存格
                    dt.lisTitleMerge.Add(new Dictionary<string, string> { { "SIZE RATIO OF MARKER", string.Format("{0},{1}", 5, dt.Columns.Count - 8) } });

                    dt.Borders.AllCellsBorders = true;

                    sxr.dicDatas.Add(sxr._v + "tbl1" + idxStr, dt);
                    sxr.dicDatas.Add(sxr._v + "SizeGroup" + idxStr, SizeGroup);
                    //sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);
                    sxr.dicDatas.Add(sxr._v + "MarkerDownloadID" + idxStr, MarkerDownloadID);

                }

                sxr.VarToSheetName = sxr._v + "SizeGroup";

                sxr.boOpenFile = true;
                sxr.Save();
                //SaveExcel(sxr, xltPath);
                #endregion
            }
            if (rdCheck2.Checked)
            {
                #region rdCheck2
                System.Data.DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP("", "Cutting_P01print_TTLconsumption", new List<SqlParameter> { new SqlParameter("@OrderID", _id) }, out dts);

                if (!res) return false;
                if (dts.Length < 2) return false;
                if (dts[0].Rows.Count == 0) return false;
                DataRow dr = dts[0].Rows[0];
                extra_P01_Report_TTLconsumptionPOCombo(dts[1], Convert.ToInt32(dr["QTY"]));

                string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Cutting_P01_TTLconsumptionPOCombo.xlt");
                sxrc sxr = new sxrc(xltPath);
                sxr.dicDatas.Add(sxr._v + "ORDERNO", dr["ORDERNO"]);
                sxr.dicDatas.Add(sxr._v + "STYLENO", dr["STYLENO"]);

                sxr.dicDatas.Add(sxr._v + "QTY", dr["QTY"]);
                sxr.dicDatas.Add(sxr._v + "FTY", dr["FACTORY"]);

                sxr.dicDatas.Add(sxr._v + "FABTYPE", dr["FABTYPE"]);
                sxr.dicDatas.Add(sxr._v + "FLP", dr["FLP"].ToString());
                sxr.dicDatas.Add(sxr._v + "MarkerDownloadID", dr["MarkerDownloadID"]);

                sxr.dicDatas.Add(sxr._v + "Now", DateTime.Now);

                sxrc.xltRptTable dt = new sxrc.xltRptTable(dts[1]);

                //欄位水平對齊
                for (int i = 3; i <= dt.Columns.Count; i++)
                {
                    sxrc.xlsColumnInfo citbl = new sxrc.xlsColumnInfo(i, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight);

                    if (i == 4 | i == 6 | i == 8 | i == 7)
                    {
                        citbl.PointCnt = 2; //小數點兩位
                    }
                    else if (i == 9)
                    {
                        citbl.PointCnt = 0;
                    }
                    else if (i == 13)
                    {
                        citbl.PointCnt = 3;
                    }
                    dt.lisColumnInfo.Add(citbl);
                }
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(1, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                dt.lisColumnInfo.Add(new sxrc.xlsColumnInfo(2, true, 0, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft));
                //合併儲存格
                //dt.lisTitleMerge.Add(new Dictionary<string, string> { { "Usage", string.Format("{0},{1}", 3, 4) }, { "Purchase", string.Format("{0},{1}", 5, 6) } });
                dt.Borders.DependOnColumn.Add(1, 2);

                //不顯示標題列
                dt.ShowHeader = false;

                sxr.dicDatas.Add(sxr._v + "tbl1", dt);

                sxr.boOpenFile = true;
                sxr.Save();
                //SaveExcel(sxr, xltPath);
                #endregion
            }

            return true;
        }

        void extra_P01_EachConsumptionCuttingCombo(DataTable dt)
        {
            string COMB = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (COMB != dr["COMB"].ToString().Trim())
                {
                    DataRow ndr = dt.NewRow();
                    ndr["COMB"] = dr["COMB"];
                    ndr["REMARK"] = dr["COMBdes"];

                    dt.Rows.InsertAt(ndr, i);

                    i += 1;
                    COMB = dr["COMB"].ToString().Trim();
                }
                dr["COMB"] = "";
            }

            dt.Columns.Remove("COMBdes");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["REMARK"].ToString().Trim() != "" & dr["COMB"].ToString().Trim() == "")
                {
                    DataRow ndr = dt.NewRow();
                    ndr["REMARK"] = dr["REMARK"];
                    dr["REMARK"] = "";
                    dt.Rows.InsertAt(ndr, i);
                    i += 1;
                }

            }
        }

        void extra_P01_Report_TTLconsumptionPOCombo(DataTable dt, int Qty)
        {
            string coltmp = "";
            decimal totaltmp = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string col1content = dt.Rows[i][0].ToString();

                if (coltmp != col1content)
                {

                    if (coltmp == "")
                    {
                        coltmp = col1content;
                    }
                    else
                    {
                        coltmp = col1content;
                        addSubTotalRow(dt, totaltmp, i, Qty);

                        totaltmp = 0;
                        i += 1;
                    }
                }
                else
                {
                    dt.Rows[i][0] = DBNull.Value;
                    dt.Rows[i]["M/WIDTH"] = DBNull.Value;
                    dt.Rows[i]["M/WEIGHT"] = DBNull.Value;
                    dt.Rows[i]["STYLE DATA CONS/PC"] = DBNull.Value;
                }

                totaltmp += decimal.Parse(dt.Rows[i]["TOTAL(Inclcut. use)"].ToString());
            }

            addSubTotalRow(dt, totaltmp, dt.Rows.Count, Qty);
        }

        void addSubTotalRow(DataTable dt, decimal tot, int idx, int Qty)
        {
            DataRow dr = dt.NewRow();
            dr["TOTAL(Inclcut. use)"] = tot;
            dr["M/WEIGHT"] = "SubTotal";
            dr["CONS/PC"] = Qty == 0 ? 0 : Math.Round(tot / Qty, 3);
            dt.Rows.InsertAt(dr, idx);
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            ToExcel();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
