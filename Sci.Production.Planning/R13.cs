using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
using Sci.Production.Class.Commons;
using System.IO;
using Sci.Utility.Excel;
using System.Globalization;
using Sci.Production.Report;
using Sci.Production.Class;

namespace Sci.Production.Planning
{
    public partial class R13 : Sci.Win.Tems.PrintForm
    { 
        private DataTable dt_source; //For Grid用
        private DataTable[] dsData;
        private IDictionary<string, IList<DataRow>> id_to_AdidasKPITarget = new Dictionary<string, IList<DataRow>>();

        private decimal decYear;
        private decimal decMonth;
        private int intReportType;
        private int intByType;
        private int intSourceType;
        private bool boDetail;

        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            EditMode = true;
            print.Visible = false;

            radioAGC.CheckedChanged += rdRportType_CheckedChanged;
            radioFactory.CheckedChanged += rdRportType_CheckedChanged;
            radioMDP.CheckedChanged += rdRportType_CheckedChanged;
        }

        void rdRportType_CheckedChanged(object sender, EventArgs e)
        {
            panelByType.Visible = radioMDP.Checked;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Text = PrivUtils.GetVersion(this.Text);

            if (IsFormClosed) return;

            btnUndo.Visible = false;
            btnSave.Visible = false;

            numYear.Value = DateTime.Now.Year;
            numMonth.Value = DateTime.Now.Month;

            setGrid();
            RefreshData();
        }

        void setGrid()
        {
            this.gridAdidasKPIReport.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.gridAdidasKPIReport)
              .Text("KPIItem", header: "KPIItem", width: Widths.AnsiChars(5), iseditingreadonly: true)
              .Text("XlsColumn", header: "XlsColumn", width: Widths.AnsiChars(5), iseditingreadonly: false)
              .Numeric("Target", header: "Target", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: false)
              .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
             ;
        }

        protected override bool ValidateInput()  //欄位檢核
        {

            if (numYear.Text == "")
            {
                MyUtility.Msg.ErrorBox("Year can't be empty");
                return false;
            }
            if (numMonth.Text == "")
            {
                MyUtility.Msg.ErrorBox("Year can't be empty");
                return false;
            }

            decYear = numYear.Value;
            decMonth = numMonth.Value;
            intReportType = radioAGC.Checked ? 1 : radioFactory.Checked ? 2 : 3;
            intByType = radioByAGC.Checked ? 1 : radioByFactory.Checked ? 2 : 3;
            intSourceType = radioAdidas.Checked ? 1 : radioReebok.Checked ? 2 : 3;
            boDetail = checkDetail.Checked;

            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;
            try
            {
                List<SqlParameter> plis = new List<SqlParameter>();
                plis.Add(new SqlParameter("@Year", decYear));
                plis.Add(new SqlParameter("@Month", decMonth));
                plis.Add(new SqlParameter("@ReportType", intReportType));
                plis.Add(new SqlParameter("@ByType", intByType));
                plis.Add(new SqlParameter("@SourceType", intSourceType));
                plis.Add(new SqlParameter("@isDetail", boDetail));

                DualResult res = DBProxy.Current.SelectSP("", "Planning_Report_R13", plis, out dsData);

                if (intReportType == 1 || intReportType == 2)
                {
                    //0是由年+月三列資料組成，跳過

                    setColumn(dsData[1]); //報表主體

                    if (boDetail)
                    {
                        DataTable dt2 = dsData[2];//明細
                        
                        //移除SLTPDP欄位之後的欄位
                        int idx = dt2.Columns["SLTPDP"].Ordinal;
                        for (int i = dt2.Columns.Count - 1; i > idx; i--)
                        {
                            dt2.Columns.RemoveAt(i);
                        }

                        setColumn(dt2); 
                    }
                }
                else
                { 
                
                }

                if (res && dsData != null)
                {
                    //顯示筆數
                    SetCount(dsData[0].Rows.Count);
                    return Result.True;
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

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            string strYear = decYear.ToString("0000");
            string strMonth = decMonth.ToString("00");
            string strReportType = radioAGC.Checked ? radioAGC.Text : radioFactory.Checked ? radioFactory.Text : radioMDP.Text;
            string strSourceType = radioAdidas.Checked ? radioAdidas.Text : radioReebok.Checked ? radioReebok.Text : radioAll.Text;

            if (intReportType == 1 || intReportType == 2)
            {

                SaveXltReportCls sxrc = new SaveXltReportCls("Planning_R13_01.xltx");
                sxrc.BoOpenFile = true;
                sxrc.DicDatas.Add("##Year", strYear);
                sxrc.DicDatas.Add("##Month", strMonth);
                sxrc.DicDatas.Add("##ReportType", strReportType);
                sxrc.DicDatas.Add("##SourceType", strSourceType);

                sxrc.DicDatas.Add("##YearMonth1", dsData[0].Rows[0]["Month"]);
                sxrc.DicDatas.Add("##YearMonth2", dsData[0].Rows[1]["Month"]);
                sxrc.DicDatas.Add("##YearMonth3", dsData[0].Rows[2]["Month"]);

                sxrc.DicDatas.Add("##Pct1", getGetTarget("MDP"));
                sxrc.DicDatas.Add("##Pct2", getGetTarget("Dol-MDP"));
                sxrc.DicDatas.Add("##Pct3", getGetTarget("SDP"));
                sxrc.DicDatas.Add("##Pct4", getGetTarget("OCP-New"));
                sxrc.DicDatas.Add("##Pct5", getGetTarget("PDP in Line"));
                sxrc.DicDatas.Add("##Pct6", getGetTarget("SDol 0"));
                sxrc.DicDatas.Add("##Pct7", getGetTarget("SLT PDP"));

                SaveXltReportCls.XltRptTable xrt = new SaveXltReportCls.XltRptTable(dsData[1]);
                xrt.ShowHeader = false;
                //xrt.Borders.AllCellsBorders = true;
                xrt.Borders.DependOnColumn.Add(1, 3);
                
                sxrc.DicDatas.Add("##tbl", xrt);
                
                if (boDetail)
                {
                    Microsoft.Office.Interop.Excel.Workbook wkb = sxrc.ExcelApp.ActiveWorkbook;
                    Microsoft.Office.Interop.Excel.Worksheet wks = wkb.Sheets[1];
                    Microsoft.Office.Interop.Excel.Worksheet wksnew = wkb.Sheets.Add( After: wks);
                    wksnew.Name = "Detail Log";
                    wksnew.Cells[1, 1].Value = "##detailTbl";
                 
                    SaveXltReportCls.XltRptTable xrt2 = new SaveXltReportCls.XltRptTable(dsData[2]);
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(GCN("CRDDate")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(GCN("BuyerDelivery")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(GCN("SciDelivery")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(GCN("PlanDate")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(GCN("PulloutDate")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(GCN("OrigBuyerDelivery")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(GCN("FirstProduction")) { NumberFormate = "yyyy/MM/dd" });
                    xrt2.LisColumnInfo.Add(new SaveXltReportCls.XlsColumnInfo(GCN("dLastProduction")) { NumberFormate = "yyyy/MM/dd" });
                
                    xrt2.BoAddFilter = true;
                    xrt2.BoTitleBold = true;
                    xrt2.BoAutoFitColumn = true;
                    xrt2.HeaderColor = Color.FromArgb(204, 255, 204);//背景顏色
                    xrt2.BoFreezePanes = true; // 進行凍結視窗
                    sxrc.DicDatas.Add("##detailTbl", xrt2);  
                }

                sxrc.Save(Sci.Production.Class.MicrosoftFile.GetName("Planning_R13_01"));
            }
            else
            {
                SaveXltReportCls sxrc = new SaveXltReportCls("Planning_R13_02.xltx");
                sxrc.BoOpenFile = true;
                sxrc.DicDatas.Add("##Year", strYear);
                sxrc.DicDatas.Add("##Month", strMonth);
                sxrc.DicDatas.Add("##ReportType", strReportType);
                sxrc.DicDatas.Add("##SourceType", strSourceType);

                Microsoft.Office.Interop.Excel.Workbook wkb = sxrc.ExcelApp.ActiveWorkbook;
                Microsoft.Office.Interop.Excel.Worksheet wks = wkb.Sheets[1];

                DataTable dtList = dsData[1]; //列出AGC或Factroy或CRDDate的項目
                int idxRow = 4; //Excel範本檔案Title結束後的位置
                int idxItem = 2; //dsData的資料開始位置
                foreach (DataRow row in dtList.Rows)
                {
                    string k1 = "##item" + idxItem.ToString();
                    wks.Cells[idxRow, 2].Value = k1;
                    wks.Cells[idxRow, 2].Interior.Color = Color.Yellow;
                    sxrc.DicDatas.Add(k1, row["item"]);

                    DataTable[] dtMons = getTablesByMonth(dsData[idxItem]);
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
                        //xrt.boAutoFitColumn = true;
                       
                        sxrc.DicDatas.Add(k2, xrt);

                        idxRow += 2;    //多跨一行，所以+2                       
                    }

                    idxRow += 1; //再多跨一行
                    idxItem += 1;
                }

                sxrc.Save(Sci.Production.Class.MicrosoftFile.GetName("Planning_R13_02"));
            }
            return true;
        }

        void setColumn(DataTable dt)
        {            
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = GCN(dt.Columns[i].ColumnName);
            }
        }

        /// <summary>
        /// 從dt_source取得某個項目的Target，因為值為%所以除100
        /// </summary>
        /// <param name="TName"></param>
        /// <returns></returns>
        private decimal getGetTarget(string TName)
        {
           DataRow[] rows = dt_source.Select(string.Format("Description = '{0}'", TName));
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
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable[] getTablesByMonth(DataTable dt)
        {            
            DataTable dtMonth = dt.DefaultView.ToTable(true, "Month");
            DataTable[] dts = new DataTable[dtMonth.Rows.Count ];

            int idx = 0;
            //For 每個月
            foreach (DataRow row in dtMonth.Rows)
            {
                string strMonth = row["Month"].ToString();
                DataTable tmp = dt.Select(string.Format("Month = '{0}'", strMonth)).CopyToDataTable();
                
                //移除年月欄位
                tmp.Columns.Remove("Month");

                //第一欄Title改為該月份的英文縮寫
                DateTime d = new DateTime(2016, Convert.ToInt32(strMonth.Substring(4,2)), 1);
                string strMMM = d.ToString("MMM", CultureInfo.CreateSpecificCulture("en-US")) + ".";

                tmp.Columns[0].ColumnName = strMMM;

                dts[idx] = tmp;
                idx += 1;
            }

            return dts;
        }

        private void RefreshData()
        {
            dt_source = new DataTable();
            string sql = string.Format("select * from AdidasKPITarget WITH (NOLOCK) order by XlsColumn");
            DualResult result = DBProxy.Current.Select(null, sql, out dt_source);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString(), "Error");
                return;
            }
            if (dt_source.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("AdidasKPITarget did'n have data.", "Error");
                return;
            }

            id_to_AdidasKPITarget = dt_source.ToDictionaryList((x) => x.Val<string>("Description"));

            bindingSource1.DataSource = dt_source;
            gridAdidasKPIReport.DataSource = bindingSource1;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            gridAdidasKPIReport.IsEditingReadOnly = false;
            btnUndo.Visible = true;
            btnSave.Visible = true;
            btnEdit.Visible = false;
            RefreshData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DualResult result;
            ITableSchema dt_schema;
            bool rtn;
            gridAdidasKPIReport.EndEdit();
            bindingSource1.EndEdit();
            result = DBProxy.Current.GetTableSchema(null, "AdidasKPITarget", out dt_schema);
            foreach (DataRow dr in dt_source.Rows)
            {
                if (!(result = DBProxy.Current.UpdateAllByChanged_ForNonPK(null, dt_schema, dr, out rtn)))
                {
                    MyUtility.Msg.ErrorBox(result.ToString(), "Error");
                    return;
                }
            }
            ShowInfo(string.Format("Update Completed!", dt_source.Rows.Count));
            RefreshData();

            gridAdidasKPIReport.IsEditingReadOnly = true;
            btnUndo.Visible = false;
            btnSave.Visible = false;
            btnEdit.Visible = true;

        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            gridAdidasKPIReport.IsEditingReadOnly = true;
            btnUndo.Visible = false;
            btnSave.Visible = false;
            btnEdit.Visible = true;

            gridAdidasKPIReport.CancelEdit();
            RefreshData();
        }
       
        private string GCN(string ColumnName)
        {
            string strColName;

            switch (ColumnName)
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
                default: strColName = ColumnName; break;
            }

            return strColName;
        }
    }
}