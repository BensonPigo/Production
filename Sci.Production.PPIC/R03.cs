using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R03
    /// </summary>
    public partial class R03 : Win.Tems.PrintForm
    {
        private PPIC_R03_ViewModel ppic_R03_ViewModel;

        // 最後一欄 , 有新增欄位要改這
        // 注意!新增欄位也要新增到StandardReport_Detail(Customized)。
        private int lastColA = 159;

        private DataTable printData;
        private DataTable subprocessColumnName;
        private DataTable orderArtworkData;
        private DataTable printingDetailDatas;
        private decimal stdTMS;
        private int subtrue = 0;

        /// <summary>
        /// Subtrue
        /// </summary>
        public int Subtrue
        {
            get
            {
                return this.subtrue;
            }

            set
            {
                this.subtrue = value;
            }
        }

        /// <summary>
        /// R03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        /// <param name="type">type</param>
        public R03(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.Text = type == "1" ? "R03. PPIC master list report" : "R031. PPIC master list report (Artwork)";
            this.checkIncludeArtworkdata.Enabled = type != "1";
            this.checkIncludeArtworkdataKindIsPAP.Enabled = type != "1";
            this.checkByCPU.Enabled = type != "1";

            DataTable zone, mDivision, factory, subprocess;
            string strSelectSql = @"select '' as Zone,'' as Fty union all
select distinct f.Zone,f.Zone+' - '+(select CONCAT(ID,'/') from Factory WITH (NOLOCK) where Zone = f.Zone for XML path('')) as Fty
from Factory f WITH (NOLOCK) where Zone <> ''";

            DBProxy.Current.Select(null, strSelectSql, out zone);
            MyUtility.Tool.SetupCombox(this.comboZone, 2, zone);
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            DBProxy.Current.Select(null, "select '' as ID union all select ID from ArtworkType WITH (NOLOCK) where ReportDropdown = 1", out subprocess);
            MyUtility.Tool.SetupCombox(this.comboSubProcess, 1, subprocess);

            this.comboZone.SelectedIndex = 0;
            this.comboM.Text = Env.User.Keyword;
            this.comboFactory.Text = Env.User.Factory;
            this.comboSubProcess.SelectedIndex = 0;
            this.checkBulk.Checked = true;

            if (type != "1")
            {
                this.checkIncludeArtworkdata.Checked = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateCutOffDate.Value1) && MyUtility.Check.Empty(this.dateCutOffDate.Value2) &&
                MyUtility.Check.Empty(this.dateCustRQSDate.Value1) && MyUtility.Check.Empty(this.dateCustRQSDate.Value2) &&
                MyUtility.Check.Empty(this.datePlanDate.Value1) && MyUtility.Check.Empty(this.datePlanDate.Value2) &&
                MyUtility.Check.Empty(this.dateOrderCfmDate.Value1) && MyUtility.Check.Empty(this.dateOrderCfmDate.Value2))
            {
                MyUtility.Msg.WarningBox("All date can't empty!!");
                this.dateBuyerDelivery.TextBox1.Focus();
                return false;
            }

            this.ppic_R03_ViewModel = new PPIC_R03_ViewModel()
            {
                IsPowerBI = false,
                ColumnsNum = this.lastColA,
                BuyerDelivery1 = this.dateBuyerDelivery.Value1,
                BuyerDelivery2 = this.dateBuyerDelivery.Value2,
                SciDelivery1 = this.dateSCIDelivery.Value1,
                SciDelivery2 = this.dateSCIDelivery.Value2,
                SDPDate1 = this.dateCutOffDate.Value1,
                SDPDate2 = this.dateCutOffDate.Value2,
                CRDDate1 = this.dateCustRQSDate.Value1,
                CRDDate2 = this.dateCustRQSDate.Value2,
                PlanDate1 = this.datePlanDate.Value1,
                PlanDate2 = this.datePlanDate.Value2,
                CFMDate1 = this.dateOrderCfmDate.Value1,
                CFMDate2 = this.dateOrderCfmDate.Value2,
                SP1 = this.txtSp1.Text.Trim(),
                SP2 = this.txtSp2.Text.Trim(),
                StyleID = this.txtstyle.Text.Trim(),
                Article = this.txtArticle.Text.Trim(),
                SeasonID = this.txtseason.Text.Trim(),
                BrandID = this.txtbrand.Text.Trim(),
                CustCDID = this.txtcustcd.Text.Trim(),
                Zone = MyUtility.Convert.GetString(this.comboZone.SelectedValue),
                MDivisionID = this.comboM.Text,
                Factory = this.comboFactory.Text,
                Bulk = this.checkBulk.Checked,
                Sample = this.checkSample.Checked,
                Material = this.checkMaterial.Checked,
                Forecast = this.checkForecast.Checked,
                Garment = this.checkGarment.Checked,
                SMTL = this.checkSMTL.Checked,
                ArtworkTypeID = this.txtArticle.Text.Trim(),
                IncludeHistoryOrder = this.checkIncludeHistoryOrder.Checked,
                IncludeArtworkData = this.checkIncludeArtworkdata.Checked,
                PrintingDetail = this.chkPrintingDetail.Checked,
                ByCPU = this.checkByCPU.Checked,
                IncludeArtworkDataKindIsPAP = this.checkIncludeArtworkdataKindIsPAP.Checked,
                SeparateByQtyBdownByShipmode = this.checkQtyBDownByShipmode.Checked,
                ListPOCombo = this.checkListPOCombo.Checked,
                IncludeCancelOrder = this.chkIncludeCancelOrder.Checked,
            };

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.stdTMS = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("select StdTMS from System WITH (NOLOCK) "));
            PPIC_R03 biModel = new PPIC_R03();
            Base_ViewModel resultReport = biModel.GetPPICMasterList(this.ppic_R03_ViewModel);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.printData = resultReport.DtArr.ElementAtOrDefault(0);
            this.subprocessColumnName = resultReport.DtArr.ElementAtOrDefault(1);
            this.orderArtworkData = resultReport.DtArr.ElementAtOrDefault(2);
            this.printingDetailDatas = resultReport.DtArr.ElementAtOrDefault(3);

            DBProxy.Current.DefaultTimeout = 0;
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            string strXltName = Env.Cfg.XltPathDir + "\\PPIC_R03_PPICMasterList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            int lastCol = this.lastColA;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Name = "PPIC_Master_List";

            // excel.Visible = true;

            // 填Subprocess欄位名稱
            int poSubConCol = 9999, subConCol = 9999, ttlTMS = lastCol + 1; // 紀錄SubCon與TTL_TMS的欄位
            int eMBROIDERYPOSubcon = 9999, eMBROIDERYSubcon = 9999;
            int printingDetailCol = 9999;
            string excelColEng = string.Empty;
            if (this.ppic_R03_ViewModel.IncludeArtworkData || this.ppic_R03_ViewModel.IncludeArtworkDataKindIsPAP)
            {
                foreach (DataRow dr in this.subprocessColumnName.Rows)
                {
                    worksheet.Cells[1, MyUtility.Convert.GetInt(dr["rno"])] = MyUtility.Convert.GetString(dr["ColumnN"]);
                    lastCol = MyUtility.Convert.GetInt(dr["rno"]);

                    if (this.ppic_R03_ViewModel.PrintingDetail && MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "PRINTING LT")
                    {
                        printingDetailCol = MyUtility.Convert.GetInt(dr["rno"]);
                    }

                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "POSUBCON")
                    {
                        poSubConCol = MyUtility.Convert.GetInt(dr["rno"]);
                        this.Subtrue = 1;
                    }

                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "SUBCON")
                    {
                        subConCol = MyUtility.Convert.GetInt(dr["rno"]);
                    }

                    if (MyUtility.Convert.GetString(dr["ColumnN"]).ToUpper() == "TTL_TMS")
                    {
                        ttlTMS = MyUtility.Convert.GetInt(dr["rno"]);
                    }

                    if (MyUtility.Convert.GetString(dr["ColumnN"]) == "EMBROIDERY(POSubcon)")
                    {
                        eMBROIDERYPOSubcon = MyUtility.Convert.GetInt(dr["rno"]);
                    }

                    if (MyUtility.Convert.GetString(dr["ColumnN"]) == "EMBROIDERY(SubCon)")
                    {
                        eMBROIDERYSubcon = MyUtility.Convert.GetInt(dr["rno"]);
                    }
                }

                // 算出Excel的Column的英文位置
                excelColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(lastCol);
            }
            else
            {
                worksheet.Cells[1, ttlTMS] = "TTL_TMS";

                // 算出Excel的Column的英文位置
                excelColEng = PublicPrg.Prgs.GetExcelEnglishColumnName(lastCol + 1);
            }

            // 填內容值
            int intRowsStart = 0;
            int maxRow = 0;
            int tRow = 10000;
            object[,] objArray = new object[tRow + 1, lastCol + 1];

            string kPIChangeReasonName;  // CLOUMN[CC]:dr["KPIChangeReason"]+dr["KPIChangeReasonName"]

            // Dictionary<string, DataRow> tmp_a = orderArtworkData.AsEnumerable().ToDictionary<DataRow, string, DataRow>(r => r["ID"].ToString(),r => r);
            if (this.orderArtworkData == null)
            {
                this.orderArtworkData = new DataTable();
                this.orderArtworkData.ColumnsStringAdd("ID");
            }

            var lookupID = this.orderArtworkData.AsEnumerable().ToLookup(row => row["ID"].ToString());
            excel.Cells.EntireColumn.AutoFit(); // 所有列最適列高
            foreach (DataRow dr in this.printData.Rows)
            {
                // EMBROIDERY 如果Qty price都是0該筆資料不show
                if (this.orderArtworkData.Rows.Count > 0 && !MyUtility.Check.Empty(this.ppic_R03_ViewModel.ArtworkTypeID))
                {
                    // DataRow[] find_subprocess = orderArtworkData.Select(string.Format("ID = '{0}' and ArtworkTypeID = '{1}' and (Price > 0 or Qty > 0)", MyUtility.Convert.GetString(dr["ID"]), subProcess));
                    var records = from record in lookupID[MyUtility.Convert.GetString(dr["ID"])]
                                  where record.Field<string>("ArtworkTypeID").ToUpper() == this.ppic_R03_ViewModel.ArtworkTypeID.ToUpper()
                                           && (record.Field<decimal>("Price") > 0 || record.Field<decimal>("Qty") > 0)
                                  select record;
                    if (records.Count() == 0)
                    {
                        continue;
                    }

                    records = null;
                }

                string key = Convert.ToDateTime(dr["SciDelivery"]).ToString("yyyyMM");
                if (Convert.ToDateTime(dr["SciDelivery"]).Day <= 7)
                {
                    key = Convert.ToDateTime(dr["SciDelivery"]).AddMonths(-1).ToString("yyyyMM");
                }

                #region 填固定欄位資料
                kPIChangeReasonName = dr["KPIChangeReason"].ToString().Trim() + "-" + dr["KPIChangeReasonName"].ToString().Trim();

                objArray[intRowsStart, 0] = dr["MDivisionID"];
                objArray[intRowsStart, 1] = dr["FactoryID"];
                objArray[intRowsStart, 2] = dr["BuyerDelivery"];
                objArray[intRowsStart, 3] = MyUtility.Check.Empty(dr["BuyerDelivery"]) ? string.Empty : Convert.ToDateTime(dr["BuyerDelivery"]).ToString("yyyyMM");
                objArray[intRowsStart, 4] = dr["EarliestSCIDlv"];
                objArray[intRowsStart, 5] = dr["SciDelivery"];
                objArray[intRowsStart, 6] = key;
                objArray[intRowsStart, 7] = dr["IDD"];
                objArray[intRowsStart, 8] = dr["CRDDate"];
                objArray[intRowsStart, 9] = MyUtility.Check.Empty(dr["CRDDate"]) ? string.Empty : Convert.ToDateTime(dr["CRDDate"]).ToString("yyyyMM");
                objArray[intRowsStart, 10] = MyUtility.Convert.GetDate(dr["BuyerDelivery"]) != MyUtility.Convert.GetDate(dr["CRDDate"]) ? "Y" : string.Empty;
                objArray[intRowsStart, 11] = dr["CFMDate"];
                objArray[intRowsStart, 12] = MyUtility.Check.Empty(dr["CRDDate"]) || MyUtility.Check.Empty(dr["CFMDate"]) ? 0 : Convert.ToInt32((Convert.ToDateTime(dr["CRDDate"]) - Convert.ToDateTime(dr["CFMDate"])).TotalDays);
                objArray[intRowsStart, 13] = dr["ID"];
                objArray[intRowsStart, 14] = dr["3rd_Party_Inspection"];
                objArray[intRowsStart, 15] = dr["Category"];
                objArray[intRowsStart, 16] = MyUtility.Check.Empty(dr["isForecast"]) ? string.Empty : dr["BuyMonth"];
                objArray[intRowsStart, 17] = dr["BuyBack"];
                objArray[intRowsStart, 18] = MyUtility.Convert.GetString(dr["Junk"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 19] = dr["Cancelled"];
                objArray[intRowsStart, 20] = dr["DestAlias"];
                objArray[intRowsStart, 21] = dr["StyleID"];
                objArray[intRowsStart, 22] = dr["StyleName"];
                objArray[intRowsStart, 23] = dr["ModularParent"];
                objArray[intRowsStart, 24] = dr["CPUAdjusted"];
                objArray[intRowsStart, 25] = dr["SimilarStyle"];
                objArray[intRowsStart, 26] = dr["SeasonID"];
                objArray[intRowsStart, 27] = dr["GMTLT"];
                objArray[intRowsStart, 28] = dr["OrderTypeID"];
                objArray[intRowsStart, 29] = dr["ProjectID"];
                objArray[intRowsStart, 30] = dr["PackingMethod"];
                objArray[intRowsStart, 31] = dr["HangerPack"];
                objArray[intRowsStart, 32] = dr["Customize1"];
                objArray[intRowsStart, 33] = MyUtility.Check.Empty(dr["isForecast"]) ? dr["BuyMonth"] : string.Empty;
                objArray[intRowsStart, 34] = dr["CustPONo"];
                objArray[intRowsStart, 35] = MyUtility.Convert.GetString(dr["VasShas"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 36] = dr["MnorderApv2"];
                objArray[intRowsStart, 37] = dr["VasShasCutOffDate"];
                objArray[intRowsStart, 38] = dr["MnorderApv"];
                objArray[intRowsStart, 39] = dr["KpiMNotice"];
                objArray[intRowsStart, 40] = MyUtility.Convert.GetString(dr["TissuePaper"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 41] = dr["AirFreightByBrand"];
                objArray[intRowsStart, 42] = dr["FactoryDisclaimer"];
                objArray[intRowsStart, 43] = dr["FactoryDisclaimerRemark"];
                objArray[intRowsStart, 44] = dr["ApprovedRejectedDate"];
                objArray[intRowsStart, 45] = MyUtility.Convert.GetString(dr["GFR"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 46] = dr["BrandID"];
                objArray[intRowsStart, 47] = dr["CustCDID"];
                objArray[intRowsStart, 48] = dr["Kit"];
                objArray[intRowsStart, 49] = dr["BrandFTYCode"];
                objArray[intRowsStart, 50] = dr["ProgramID"];
                objArray[intRowsStart, 51] = dr["NonRevenue"];
                objArray[intRowsStart, 52] = dr["CDCodeNew"];
                objArray[intRowsStart, 53] = dr["ProductType"];
                objArray[intRowsStart, 54] = dr["FabricType"];
                objArray[intRowsStart, 55] = dr["Lining"];
                objArray[intRowsStart, 56] = dr["Gender"];
                objArray[intRowsStart, 57] = dr["Construction"];
                objArray[intRowsStart, 58] = dr["CPU"];
                objArray[intRowsStart, 59] = dr["Qty"];
                objArray[intRowsStart, 60] = dr["FOCQty"];
                objArray[intRowsStart, 61] = MyUtility.Convert.GetDecimal(dr["CPU"]) * MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPUFactor"]);
                objArray[intRowsStart, 62] = dr["SewQtyTop"];
                objArray[intRowsStart, 63] = dr["SewQtyBottom"];
                objArray[intRowsStart, 64] = dr["SewQtyInner"];
                objArray[intRowsStart, 65] = dr["SewQtyOuter"];
                objArray[intRowsStart, 66] = dr["TtlSewQty"];
                objArray[intRowsStart, 67] = dr["CutQty"];
                objArray[intRowsStart, 68] = MyUtility.Convert.GetString(dr["WorkType"]) == "1" ? "Y" : string.Empty;
                objArray[intRowsStart, 69] = MyUtility.Convert.GetDecimal(dr["CutQty"]) >= MyUtility.Convert.GetDecimal(dr["Qty"]) ? "Y" : string.Empty;
                objArray[intRowsStart, 70] = dr["PackingQty"];
                objArray[intRowsStart, 71] = dr["PackingFOCQty"];
                objArray[intRowsStart, 72] = dr["BookingQty"];
                objArray[intRowsStart, 73] = dr["FOCAdjQty"];
                objArray[intRowsStart, 74] = dr["NotFOCAdjQty"];
                objArray[intRowsStart, 75] = dr["PoPrice"];
                objArray[intRowsStart, 76] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["PoPrice"]);
                objArray[intRowsStart, 77] = dr["KPILETA"];  // BG
                objArray[intRowsStart, 78] = dr["PFETA"];
                objArray[intRowsStart, 79] = dr["PFRemark"]; // Pull Forward Remark
                objArray[intRowsStart, 80] = dr["PackLETA"];
                objArray[intRowsStart, 81] = dr["LETA"];
                objArray[intRowsStart, 82] = dr["MTLETA"];
                objArray[intRowsStart, 83] = dr["Fab_ETA"];
                objArray[intRowsStart, 84] = dr["Acc_ETA"];
                objArray[intRowsStart, 85] = dr["SewingMtlComplt"];
                objArray[intRowsStart, 86] = dr["PackingMtlComplt"];
                objArray[intRowsStart, 87] = dr["SewETA"];
                objArray[intRowsStart, 88] = dr["PackETA"];
                objArray[intRowsStart, 89] = MyUtility.Convert.GetString(dr["MTLDelay"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 90] = MyUtility.Check.Empty(dr["MTLExport"]) ? dr["MTLExportTimes"] : dr["MTLExport"];
                objArray[intRowsStart, 91] = MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : "N";
                objArray[intRowsStart, 92] = dr["ArriveWHDate"];
                objArray[intRowsStart, 93] = dr["SewInLine"];
                objArray[intRowsStart, 94] = dr["SewOffLine"];
                objArray[intRowsStart, 95] = dr["FirstOutDate"];
                objArray[intRowsStart, 96] = dr["LastOutDate"]; // CA
                objArray[intRowsStart, 97] = dr["FirstProduction"]; // CB
                objArray[intRowsStart, 98] = dr["LastProductionDate"]; // CC
                objArray[intRowsStart, 99] = dr["EachConsApv"];
                objArray[intRowsStart, 100] = dr["KpiEachConsCheck"];
                objArray[intRowsStart, 101] = dr["CutInLine"];
                objArray[intRowsStart, 102] = dr["CutOffLine"];
                objArray[intRowsStart, 103] = dr["CutInLine_SP"];
                objArray[intRowsStart, 104] = dr["CutOffLine_SP"];
                objArray[intRowsStart, 105] = dr["FirstCutDate"];
                objArray[intRowsStart, 106] = dr["LastCutDate"];
                objArray[intRowsStart, 107] = dr["PulloutDate"];
                objArray[intRowsStart, 108] = dr["ActPulloutDate"];
                objArray[intRowsStart, 109] = dr["PulloutQty"];
                objArray[intRowsStart, 110] = dr["ActPulloutTime"];
                objArray[intRowsStart, 111] = MyUtility.Convert.GetString(dr["PulloutComplete"]).ToUpper() == "TRUE" ? "OK" : string.Empty;
                objArray[intRowsStart, 112] = dr["FtyKPI"];
                objArray[intRowsStart, 113] = !MyUtility.Check.Empty(dr["KPIChangeReason"]) ? kPIChangeReasonName : string.Empty; // cc
                objArray[intRowsStart, 114] = dr["PlanDate"];
                objArray[intRowsStart, 115] = dr["OrigBuyerDelivery"];
                objArray[intRowsStart, 116] = dr["SMR"];
                objArray[intRowsStart, 117] = dr["SMRName"];
                objArray[intRowsStart, 118] = dr["MRHandle"];
                objArray[intRowsStart, 119] = dr["MRHandleName"];
                objArray[intRowsStart, 120] = dr["POSMR"];
                objArray[intRowsStart, 121] = dr["POSMRName"];
                objArray[intRowsStart, 122] = dr["POHandle"];
                objArray[intRowsStart, 123] = dr["POHandleName"];
                objArray[intRowsStart, 124] = dr["PCHandle"];
                objArray[intRowsStart, 125] = dr["PCHandleName"];
                objArray[intRowsStart, 126] = dr["MCHandle"];
                objArray[intRowsStart, 127] = dr["MCHandleName"];
                objArray[intRowsStart, 128] = dr["DoxType"];
                objArray[intRowsStart, 129] = dr["PackingCTN"];
                objArray[intRowsStart, 130] = dr["TotalCTN1"];
                objArray[intRowsStart, 131] = dr["PackErrorCtn"];
                objArray[intRowsStart, 132] = dr["FtyCtn1"];
                objArray[intRowsStart, 133] = dr["ClogCTN1"];
                objArray[intRowsStart, 134] = dr["CFACTN"];
                objArray[intRowsStart, 135] = dr["ClogRcvDate"];
                objArray[intRowsStart, 136] = dr["InspDate"];
                objArray[intRowsStart, 137] = dr["InspResult"];
                objArray[intRowsStart, 138] = dr["InspHandle"];
                objArray[intRowsStart, 139] = dr["SewLine"];
                objArray[intRowsStart, 140] = dr["ShipModeList"];
                objArray[intRowsStart, 141] = dr["Customize2"];
                objArray[intRowsStart, 142] = dr["Article"];
                objArray[intRowsStart, 143] = dr["ColorID"];
                objArray[intRowsStart, 144] = dr["SpecialMarkName"];
                objArray[intRowsStart, 145] = dr["FTYRemark"];
                objArray[intRowsStart, 146] = dr["SampleReasonName"];
                objArray[intRowsStart, 147] = dr["IsMixMarker"];
                objArray[intRowsStart, 148] = dr["CuttingSP"];
                objArray[intRowsStart, 149] = MyUtility.Convert.GetString(dr["RainwearTestPassed"]).ToUpper() == "TRUE" ? "Y" : string.Empty;
                objArray[intRowsStart, 150] = MyUtility.Convert.GetDecimal(dr["CPU"]) * this.stdTMS;
                objArray[intRowsStart, 151] = dr["MdRoomScanDate"];
                objArray[intRowsStart, 152] = dr["DryRoomRecdDate"];
                objArray[intRowsStart, 153] = dr["DryRoomTransDate"];
                objArray[intRowsStart, 154] = dr["LastCTNTransDate"];
                objArray[intRowsStart, 155] = dr["ScanEditDate"];
                objArray[intRowsStart, 156] = dr["LastCTNRecdDate"];
                objArray[intRowsStart, 157] = dr["OrganicCotton"];
                objArray[intRowsStart, 158] = dr["DirectShip"];
                objArray[intRowsStart, 159] = dr["StyleCarryover"];
                #endregion

                if (this.ppic_R03_ViewModel.IncludeArtworkData || this.ppic_R03_ViewModel.IncludeArtworkDataKindIsPAP)
                {
                    var finRow = lookupID[dr["ID"].ToString()];
                    if (finRow.Count() > 0)
                    {
                        foreach (DataRow sdr in finRow)
                        {
                            if (!MyUtility.Check.Empty(sdr["AUnitRno"]))
                            {
                                objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["AUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }

                            if (!MyUtility.Check.Empty(sdr["PUnitRno"]))
                            {
                                if (MyUtility.Convert.GetString(sdr["ProductionUnit"]).ToUpper() == "TMS")
                                {
                                    objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["PUnitRno"]) - 1] = sdr["TMS"];
                                }
                                else
                                {
                                    objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["PUnitRno"]) - 1] = sdr["Price"];
                                }
                            }

                            if (!MyUtility.Check.Empty(sdr["NRno"]))
                            {
                                objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["NRno"]) - 1] = MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }

                            // TTL
                            if (!MyUtility.Check.Empty(sdr["TAUnitRno"]))
                            {
                                objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["TAUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }

                            if (!MyUtility.Check.Empty(sdr["TPUnitRno"]))
                            {
                                if (MyUtility.Convert.GetString(sdr["ProductionUnit"]).ToUpper() == "TMS")
                                {
                                    objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["TPUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["TMS"]);
                                }
                                else
                                {
                                    objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["TPUnitRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["Price"]);
                                }
                            }

                            if (!MyUtility.Check.Empty(sdr["TNRno"]))
                            {
                                objArray[intRowsStart, MyUtility.Convert.GetInt(sdr["TNRno"]) - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(sdr["Qty"]);
                            }

                            if (poSubConCol != 9999)
                            {
                                if (!MyUtility.Check.Empty(sdr["PoSupp"]))
                                {
                                    objArray[intRowsStart, poSubConCol - 1] = sdr["PoSupp"];
                                }

                                if (MyUtility.Check.Empty(objArray[intRowsStart, poSubConCol - 1]))
                                {
                                    objArray[intRowsStart, poSubConCol - 1] = string.Empty;
                                }
                            }

                            if (subConCol != 9999)
                            {
                                if (!MyUtility.Check.Empty(sdr["Supp"]))
                                {
                                    objArray[intRowsStart, subConCol - 1] = sdr["Supp"];
                                }

                                if (MyUtility.Check.Empty(objArray[intRowsStart, subConCol - 1]))
                                {
                                    objArray[intRowsStart, subConCol - 1] = string.Empty;
                                }
                            }

                            if (eMBROIDERYPOSubcon != 9999)
                            {
                                if (!MyUtility.Check.Empty(sdr["EMBROIDERYPOSubcon"]))
                                {
                                    objArray[intRowsStart, eMBROIDERYPOSubcon - 1] = sdr["EMBROIDERYPOSubcon"];
                                }

                                if (MyUtility.Check.Empty(objArray[intRowsStart, eMBROIDERYPOSubcon - 1]))
                                {
                                    objArray[intRowsStart, eMBROIDERYPOSubcon - 1] = string.Empty;
                                }
                            }

                            if (eMBROIDERYSubcon != 9999)
                            {
                                if (!MyUtility.Check.Empty(sdr["EMBROIDERYSubcon"]))
                                {
                                    objArray[intRowsStart, eMBROIDERYSubcon - 1] = sdr["EMBROIDERYSubcon"];
                                }

                                if (MyUtility.Check.Empty(objArray[intRowsStart, eMBROIDERYSubcon - 1]))
                                {
                                    objArray[intRowsStart, eMBROIDERYSubcon - 1] = string.Empty;
                                }
                            }
                        }
                    }

                    objArray[intRowsStart, ttlTMS - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * this.stdTMS;

                    if (this.ppic_R03_ViewModel.PrintingDetail)
                    {
                        DataRow pdr = this.printingDetailDatas.Select($"ID = '{dr["ID"]}'").FirstOrDefault();
                        if (pdr != null)
                        {
                            objArray[intRowsStart, printingDetailCol - 1] = pdr["PrintingLT"];
                            objArray[intRowsStart, printingDetailCol] = pdr["InkTypecolorsize"];
                        }
                    }
                }
                else
                {
                    objArray[intRowsStart, ttlTMS - 1] = MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["CPU"]) * this.stdTMS;
                }

                // 每一萬筆資料就先塞入excel並清空array
                switch (maxRow % tRow)
                {
                    case 0:
                        if (maxRow == 0)
                        {
                            intRowsStart++;
                            break;
                        }

                        // 空值給0
                        if (this.ppic_R03_ViewModel.IncludeArtworkData || this.ppic_R03_ViewModel.IncludeArtworkDataKindIsPAP)
                        {
                            for (int j = 0; j < intRowsStart; j++)
                            {
                                for (int i = this.lastColA; i < lastCol; i++)
                                {
                                    if (objArray[j, i] == null)
                                    {
                                        objArray[j, i] = 0;
                                        if (i == poSubConCol - 1 || i == subConCol - 1 || i == eMBROIDERYPOSubcon - 1 || i == eMBROIDERYSubcon - 1)
                                        {
                                            objArray[j, i] = string.Empty;
                                        }
                                    }
                                }
                            }
                        }

                        worksheet.Range[string.Format("A{0}:{1}{2}", maxRow / tRow == 1 ? 2 : maxRow - tRow + 3, excelColEng, maxRow + 2)].Value2 = objArray;
                        intRowsStart = 0;
                        objArray = new object[tRow + 1, lastCol + 1];
                        break;
                    default:
                        intRowsStart++;
                        break;
                }

                maxRow++;
            }

            if (maxRow == 0)
            {
                Microsoft.Office.Interop.Excel.Workbook workbook_close = excel.ActiveWorkbook;
                workbook_close.Close(false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook_close);
                this.HideWaitMessage();
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 空值給0
            if (this.ppic_R03_ViewModel.IncludeArtworkData || this.ppic_R03_ViewModel.IncludeArtworkDataKindIsPAP)
            {
                for (int j = 0; j < intRowsStart; j++)
                {
                    for (int i = this.lastColA; i < lastCol; i++)
                    {
                        if (objArray[j, i] == null)
                        {
                            objArray[j, i] = 0;
                            if (i == poSubConCol - 1 || i == subConCol - 1 || i == eMBROIDERYPOSubcon - 1 || i == eMBROIDERYSubcon - 1)
                            {
                                objArray[j, i] = string.Empty;
                            }
                        }
                    }
                }
            }

            worksheet.Range[string.Format("A{0}:{1}{0}", 1, excelColEng)].AutoFilter(1); // 篩選
            worksheet.Range[string.Format("A{0}:{1}{0}", 1, excelColEng)].Interior.Color = Color.FromArgb(191, 191, 191); // 底色
            worksheet.Range[string.Format("A{0}:{1}{2}", maxRow < tRow ? 2 : (maxRow / tRow * tRow) + 3, excelColEng, maxRow + 2)].Value2 = objArray;
            this.Subtrue = 0;
            this.CreateCustomizedExcel(ref worksheet);

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(maxRow);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R03_PPICMasterList");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            this.HideWaitMessage();
            #endregion
            return true;
        }

        private void CheckIncludeArtworkdata_CheckedChanged(object sender, EventArgs e)
        {
            this.chkPrintingDetail.Enabled = this.checkIncludeArtworkdata.Checked;
            if (!this.checkIncludeArtworkdata.Checked)
            {
                this.chkPrintingDetail.Checked = false;
            }
        }
    }
}
