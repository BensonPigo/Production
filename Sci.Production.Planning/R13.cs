using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
using Sci.Utility.Excel;
using System.Globalization;
using Sci.Production.Report;
using Sci.Production.Class;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R13
    /// </summary>
    public partial class R13 : Win.Tems.PrintForm
    {
        private DataTable dt_source; // For Grid用
        private DataTable[] dsData;
        private IDictionary<string, IList<DataRow>> id_to_AdidasKPITarget = new Dictionary<string, IList<DataRow>>();

        private decimal decYear;
        private decimal decMonth;
        private int intReportType;
        private int intByType;
        private int intSourceType;
        private bool boDetail;

        /// <summary>
        /// R13
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this.print.Visible = false;

            this.radioAGC.CheckedChanged += this.RdRportType_CheckedChanged;
            this.radioFactory.CheckedChanged += this.RdRportType_CheckedChanged;
            this.radioMDP.CheckedChanged += this.RdRportType_CheckedChanged;
        }

        private void RdRportType_CheckedChanged(object sender, EventArgs e)
        {
            this.panelByType.Visible = this.radioMDP.Checked;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Text = PrivUtils.GetVersion(this.Text);

            if (this.IsFormClosed)
            {
                return;
            }

            this.btnUndo.Visible = false;
            this.btnSave.Visible = false;

            this.numYear.Value = DateTime.Now.Year;
            this.numMonth.Value = DateTime.Now.Month;

            this.SetGrid();
            this.RefreshData();
        }

        private void SetGrid()
        {
            this.gridAdidasKPIReport.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridAdidasKPIReport)
            .Text("KPIItem", header: "KPIItem", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("XlsColumn", header: "XlsColumn", width: Widths.AnsiChars(5), iseditingreadonly: false)
            .Numeric("Target", header: "Target", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: false)
            .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (this.numYear.Text == string.Empty)
            {
                MyUtility.Msg.ErrorBox("Year can't be empty");
                return false;
            }

            if (this.numMonth.Text == string.Empty)
            {
                MyUtility.Msg.ErrorBox("Year can't be empty");
                return false;
            }

            this.decYear = this.numYear.Value;
            this.decMonth = this.numMonth.Value;
            this.intReportType = this.radioAGC.Checked ? 1 : this.radioFactory.Checked ? 2 : 3;
            this.intByType = this.radioByAGC.Checked ? 1 : this.radioByFactory.Checked ? 2 : 3;
            this.intSourceType = this.radioAdidas.Checked ? 1 : this.radioReebok.Checked ? 2 : 3;
            this.boDetail = this.checkDetail.Checked;

            return true;
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Ict.Result.True;
            try
            {
                List<SqlParameter> plis = new List<SqlParameter>();
                plis.Add(new SqlParameter("@Year", this.decYear));
                plis.Add(new SqlParameter("@Month", this.decMonth));
                plis.Add(new SqlParameter("@ReportType", this.intReportType));
                plis.Add(new SqlParameter("@ByType", this.intByType));
                plis.Add(new SqlParameter("@SourceType", this.intSourceType));
                plis.Add(new SqlParameter("@isDetail", this.boDetail));

                DualResult res = DBProxy.Current.SelectSP(string.Empty, "Planning_Report_R13", plis, out this.dsData);

                if (this.intReportType == 1 || this.intReportType == 2)
                {
                    // 0是由年+月三列資料組成，跳過
                    this.SetColumn(this.dsData[1]); // 報表主體

                    if (this.boDetail)
                    {
                        DataTable dt2 = this.dsData[2]; // 明細

                        // 移除SLTPDP欄位之後的欄位
                        int idx = dt2.Columns["SLTPDP"].Ordinal;
                        for (int i = dt2.Columns.Count - 1; i > idx; i--)
                        {
                            dt2.Columns.RemoveAt(i);
                        }

                        this.SetColumn(dt2);
                    }
                }

                if (res && this.dsData != null)
                {
                    // 顯示筆數
                    this.SetCount(this.dsData[0].Rows.Count);
                    return Ict.Result.True;
                }
                else
                {
                    return new DualResult(false, "Data not found.");
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex);
            }
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(ReportDefinition report)
        {
            string strYear = this.decYear.ToString("0000");
            string strMonth = this.decMonth.ToString("00");
            string strReportType = this.radioAGC.Checked ? this.radioAGC.Text : this.radioFactory.Checked ? this.radioFactory.Text : this.radioMDP.Text;
            string strSourceType = this.radioAdidas.Checked ? this.radioAdidas.Text : this.radioReebok.Checked ? this.radioReebok.Text : this.radioAll.Text;

            if (this.intReportType == 1 || this.intReportType == 2)
            {
                SaveXltReportCls sxrc = new SaveXltReportCls("Planning_R13_01.xltx")
                {
                    BoOpenFile = true,
                };
                sxrc.DicDatas.Add("##Year", strYear);
                sxrc.DicDatas.Add("##Month", strMonth);
                sxrc.DicDatas.Add("##ReportType", strReportType);
                sxrc.DicDatas.Add("##SourceType", strSourceType);

                sxrc.DicDatas.Add("##YearMonth1", this.dsData[0].Rows[0]["Month"]);
                sxrc.DicDatas.Add("##YearMonth2", this.dsData[0].Rows[1]["Month"]);
                sxrc.DicDatas.Add("##YearMonth3", this.dsData[0].Rows[2]["Month"]);

                sxrc.DicDatas.Add("##Pct1", this.GetGetTarget("MDP"));
                sxrc.DicDatas.Add("##Pct2", this.GetGetTarget("Dol-MDP"));
                sxrc.DicDatas.Add("##Pct3", this.GetGetTarget("SDP"));
                sxrc.DicDatas.Add("##Pct4", this.GetGetTarget("OCP-New"));
                sxrc.DicDatas.Add("##Pct5", this.GetGetTarget("PDP in Line"));
                sxrc.DicDatas.Add("##Pct6", this.GetGetTarget("SDol 0"));
                sxrc.DicDatas.Add("##Pct7", this.GetGetTarget("SLT PDP"));

                SaveXltReportCls.XltRptTable xrt = new SaveXltReportCls.XltRptTable(this.dsData[1])
                {
                    ShowHeader = false,
                };
                xrt.Borders.DependOnColumn.Add(1, 3);

                sxrc.DicDatas.Add("##tbl", xrt);

                if (this.boDetail)
                {
                    Microsoft.Office.Interop.Excel.Workbook wkb = sxrc.ExcelApp.ActiveWorkbook;
                    Microsoft.Office.Interop.Excel.Worksheet wks = wkb.Sheets[1];
                    Microsoft.Office.Interop.Excel.Worksheet wksnew = wkb.Sheets.Add(After: wks);
                    wksnew.Name = "Detail Log";
                    wksnew.Cells[1, 1].Value = "##detailTbl";

                    SaveXltReportCls.XltRptTable xrt2 = new SaveXltReportCls.XltRptTable(this.dsData[2]);
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(this.GCN("CRDDate")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(this.GCN("BuyerDelivery")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(this.GCN("SciDelivery")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(this.GCN("PlanDate")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(this.GCN("PulloutDate")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(this.GCN("OrigBuyerDelivery")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(this.GCN("FirstProduction")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(this.GCN("dLastProduction")) { NumberFormate = "yyyy/MM/dd" });

                    xrt2.BoAddFilter = true;
                    xrt2.BoTitleBold = true;
                    xrt2.BoAutoFitColumn = true;
                    xrt2.HeaderColor = Color.FromArgb(204, 255, 204); // 背景顏色
                    xrt2.BoFreezePanes = true; // 進行凍結視窗
                    sxrc.DicDatas.Add("##detailTbl", xrt2);
                }

                sxrc.Save(MicrosoftFile.GetName("Planning_R13_01"));
            }
            else
            {
                SaveXltReportCls sxrc = new SaveXltReportCls("Planning_R13_02.xltx")
                {
                    BoOpenFile = true,
                };
                sxrc.DicDatas.Add("##Year", strYear);
                sxrc.DicDatas.Add("##Month", strMonth);
                sxrc.DicDatas.Add("##ReportType", strReportType);
                sxrc.DicDatas.Add("##SourceType", strSourceType);

                Microsoft.Office.Interop.Excel.Workbook wkb = sxrc.ExcelApp.ActiveWorkbook;
                Microsoft.Office.Interop.Excel.Worksheet wks = wkb.Sheets[1];

                DataTable dtList = this.dsData[1]; // 列出AGC或Factroy或CRDDate的項目
                int idxRow = 4; // Excel範本檔案Title結束後的位置
                int idxItem = 2; // dsData的資料開始位置
                foreach (DataRow row in dtList.Rows)
                {
                    string k1 = "##item" + idxItem.ToString();
                    wks.Cells[idxRow, 2].Value = k1;
                    wks.Cells[idxRow, 2].Interior.Color = Color.Yellow;
                    sxrc.DicDatas.Add(k1, row["item"]);

                    DataTable[] dtMons = this.GetTablesByMonth(this.dsData[idxItem]);
                    foreach (DataTable dtMon in dtMons)
                    {
                        string k2 = "##tbl" + idxItem.ToString() + dtMon.Columns[0].ColumnName;
                        wks.Cells[idxRow + 1, 1].Value = k2;

                        SaveXltReportCls.XltRptTable xrt = new SaveXltReportCls.XltRptTable(dtMon);
                        xrt.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo("PO") { NumberFormate = "##,###,##0" });
                        xrt.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo("QTY") { NumberFormate = "##,###,##0" });
                        xrt.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo("MDP") { NumberFormate = "##,###,##0" });
                        xrt.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo("%") { NumberFormate = "###,##0.00%" });
                        xrt.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo("Failed-PO") { NumberFormate = "##,###,##0" });
                        xrt.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo("Failed-QTY") { NumberFormate = "##,###,##0" });
                        xrt.Borders.AllCellsBorders = true;

                        sxrc.DicDatas.Add(k2, xrt);

                        idxRow += 2;    // 多跨一行，所以+2
                    }

                    idxRow += 1; // 再多跨一行
                    idxItem += 1;
                }

                sxrc.Save(MicrosoftFile.GetName("Planning_R13_02"));
            }

            return true;
        }

        private void SetColumn(DataTable dt)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = this.GCN(dt.Columns[i].ColumnName);
            }
        }

        /// <summary>
        /// 從dt_source取得某個項目的Target，因為值為%所以除100
        /// </summary>
        /// <param name="tName">tName</param>
        /// <returns>decimal</returns>
        private decimal GetGetTarget(string tName)
        {
            DataRow[] rows = this.dt_source.Select(string.Format("Description = '{0}'", tName));
            if (rows.Length <= 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(rows[0]["Target"]) / 100;
            }
        }

        /// <summary>
        /// SP抓回來的結果，依照Month欄位分割成子Table，並做額外處裡動作
        /// </summary>
        /// <param name="dt">dt</param>
        /// <returns>DataTable</returns>
        private DataTable[] GetTablesByMonth(DataTable dt)
        {
            DataTable dtMonth = dt.DefaultView.ToTable(true, "Month");
            DataTable[] dts = new DataTable[dtMonth.Rows.Count];

            int idx = 0;

            // For 每個月
            foreach (DataRow row in dtMonth.Rows)
            {
                string strMonth = row["Month"].ToString();
                DataTable tmp = dt.Select(string.Format("Month = '{0}'", strMonth)).CopyToDataTable();

                // 移除年月欄位
                tmp.Columns.Remove("Month");

                // 第一欄Title改為該月份的英文縮寫
                DateTime d = new DateTime(2016, Convert.ToInt32(strMonth.Substring(4, 2)), 1);
                string strMMM = d.ToString("MMM", CultureInfo.CreateSpecificCulture("en-US")) + ".";

                tmp.Columns[0].ColumnName = strMMM;

                dts[idx] = tmp;
                idx += 1;
            }

            return dts;
        }

        private void RefreshData()
        {
            this.dt_source = new DataTable();
            string sql = string.Format("select * from AdidasKPITarget WITH (NOLOCK) order by XlsColumn");
            DualResult result = DBProxy.Current.Select(null, sql, out this.dt_source);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString(), "Error");
                return;
            }

            if (this.dt_source.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("AdidasKPITarget did'n have data.", "Error");
                return;
            }

            this.id_to_AdidasKPITarget = this.dt_source.ToDictionaryList((x) => x.Val<string>("Description"));

            this.bindingSource1.DataSource = this.dt_source;
            this.gridAdidasKPIReport.DataSource = this.bindingSource1;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            this.gridAdidasKPIReport.IsEditingReadOnly = false;
            this.btnUndo.Visible = true;
            this.btnSave.Visible = true;
            this.btnEdit.Visible = false;
            this.RefreshData();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DualResult result;
            ITableSchema dt_schema;
            bool rtn;
            this.gridAdidasKPIReport.EndEdit();
            this.bindingSource1.EndEdit();
            result = DBProxy.Current.GetTableSchema(null, "AdidasKPITarget", out dt_schema);
            foreach (DataRow dr in this.dt_source.Rows)
            {
                if (!(result = DBProxy.Current.UpdateAllByChanged_ForNonPK(null, dt_schema, dr, out rtn)))
                {
                    MyUtility.Msg.ErrorBox(result.ToString(), "Error");
                    return;
                }
            }

            this.ShowInfo(string.Format("Update Completed!", this.dt_source.Rows.Count));
            this.RefreshData();

            this.gridAdidasKPIReport.IsEditingReadOnly = true;
            this.btnUndo.Visible = false;
            this.btnSave.Visible = false;
            this.btnEdit.Visible = true;
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            this.gridAdidasKPIReport.IsEditingReadOnly = true;
            this.btnUndo.Visible = false;
            this.btnSave.Visible = false;
            this.btnEdit.Visible = true;

            this.gridAdidasKPIReport.CancelEdit();
            this.RefreshData();
        }

        private string GCN(string columnName)
        {
            string strColName;

            switch (columnName)
            {
                case "BrandID": strColName = "Cust"; break;
                case "Country": strColName = "Country"; break;
                case "FactoryID": strColName = "Factory"; break;
                case "AGC": strColName = "AGC"; break;
                case "OrderTypeID": strColName = "Order Type"; break;
                case "ID": strColName = "SP#"; break;
                case "Category": strColName = "Category"; break;
                case "ProjectID": strColName = "ProjectID"; break;
                case "CRDDate": strColName = "CRD Date"; break;
                case "BuyerDelivery": strColName = "Delivery"; break;
                case "SciDelivery": strColName = "SCI DLV"; break;
                case "PlanDate": strColName = "PlanDate"; break;
                case "PulloutDate": strColName = "Actual PullOut"; break;
                case "OrigBuyerDelivery": strColName = "First Supplier Date"; break;
                case "FirstProduction": strColName = "First Production Date"; break;
                case "dLastProduction": strColName = "Last Production Date"; break;
                case "Qty": strColName = "Qty"; break;
                case "BalQty_MDP": strColName = "MDP"; break;
                case "BalQty_Dol_MDP": strColName = "Dol-MDP"; break;
                case "SDP": strColName = "SDP"; break;
                case "OCPnew": strColName = "OCP-New"; break;
                case "PDPinLine": strColName = "PDP in Line"; break;
                case "Sdol": strColName = "Sdol 0"; break;
                case "SLTPDP": strColName = "SLT PDP"; break;
                default: strColName = columnName; break;
            }

            return strColName;
        }
    }
}