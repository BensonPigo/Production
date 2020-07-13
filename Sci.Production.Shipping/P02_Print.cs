using System;
using System.Data;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02_Print
    /// </summary>
    public partial class P02_Print : Win.Tems.PrintForm
    {
        private DataRow masterData;
        private DataTable detailData;
        private DataTable detailSummary;
        private string reportType;
        private string handleName;
        private string managerName;
        private string destination;
        private string carrier;
        private string mdivisionName;
        private string mdivisionAddr;
        private string mdivisionTel;
        private string messrs;
        private string courierAWB;
        private string shipmentPort;
        private string destinationPort;

        /// <summary>
        /// P02_Print
        /// </summary>
        /// <param name="masterData">masterData</param>
        /// <param name="detailData">detailData</param>
        public P02_Print(DataRow masterData, DataTable detailData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.detailData = detailData;
            this.radioDetailList.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.radioDetailList.Checked)
            {
                this.reportType = "1";
            }

            if (this.radioPackingList.Checked)
            {
                this.reportType = "2";
            }

            if (this.radioDetailPackingList.Checked)
            {
                this.reportType = "3";
            }

            if (this.rdbtnDHLcustomsclearance.Checked)
            {
                this.reportType = "4";
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (this.reportType == "1")
            {
                this.handleName = MyUtility.GetValue.Lookup(string.Format("select Name+ ' #' + ExtNo as Incharge from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["AddName"])));
                this.managerName = MyUtility.GetValue.Lookup(string.Format("select Name+ ' #' + ExtNo as Incharge from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["Manager"])));
                this.destination = MyUtility.GetValue.Lookup(string.Format("select Alias from Country WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["Dest"])));
                this.carrier = MyUtility.GetValue.Lookup(string.Format("select (c.SuppID + '-' + s.AbbEN) as Supplier from Carrier c WITH (NOLOCK) left join Supp s WITH (NOLOCK) on c.SuppID = s.ID where c.ID = '{0}'", MyUtility.Convert.GetString(this.masterData["CarrierID"])));
            }
            else if (this.reportType == "2" || this.reportType == "3")
            {
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format("select NameEN,AddressEN,Tel from Factory WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["MDivisionID"])), out dr))
                {
                    this.mdivisionName = MyUtility.Convert.GetString(dr["NameEN"]);
                    this.mdivisionAddr = MyUtility.Convert.GetString(dr["AddressEN"]);
                    this.mdivisionTel = MyUtility.Convert.GetString(dr["Tel"]);
                }
                else
                {
                    this.mdivisionName = string.Empty;
                    this.mdivisionAddr = string.Empty;
                    this.mdivisionTel = string.Empty;
                }

                this.messrs = string.Empty;
                if (MyUtility.Convert.GetString(this.masterData["ToTag"]) == "1" || MyUtility.Convert.GetString(this.masterData["ToTag"]) == "3")
                {
                    if (MyUtility.Check.Seek(string.Format("select AbbEN,AddressEN,Tel from Supp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ToSite"])), out dr))
                    {
                        this.messrs = string.Format("{0}\r\n{1}\r\nTEL: {2}", MyUtility.Convert.GetString(dr["AbbEN"]), MyUtility.Convert.GetString(dr["AddressEN"]), MyUtility.Convert.GetString(dr["Tel"]));
                    }
                }
                else if (MyUtility.Convert.GetString(this.masterData["ToTag"]) == "4")
                {
                    if (MyUtility.Check.Seek(string.Format("select NameEN,AddressEN,Tel from Brand WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ToSite"])), out dr))
                    {
                        this.messrs = string.Format("{0}\r\n{1}\r\nTEL: {2}", MyUtility.Convert.GetString(dr["NameEN"]), MyUtility.Convert.GetString(dr["AddressEN"]), MyUtility.Convert.GetString(dr["Tel"]));
                    }
                }
                else if (MyUtility.Convert.GetString(this.masterData["ToTag"]) == "2")
                {
                    if (MyUtility.Check.Seek(string.Format("select Abb,AddressEN,Tel from SCIFty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ToSite"])), out dr))
                    {
                        this.messrs = string.Format("{0}\r\n{1}\r\nTEL: {2}", MyUtility.Convert.GetString(dr["Abb"]), MyUtility.Convert.GetString(dr["AddressEN"]), MyUtility.Convert.GetString(dr["Tel"]));
                    }
                }

                this.courierAWB = string.Format("{0}# {1}", MyUtility.GetValue.Lookup(string.Format("select AbbEN from Supp where ID = (select SuppID from Carrier WITH (NOLOCK) where ID = '{0}')", MyUtility.Convert.GetString(this.masterData["CarrierID"]))), MyUtility.Convert.GetString(this.masterData["BLNo"]));
                this.shipmentPort = MyUtility.Convert.GetString(this.masterData["FromTag"]) == "1" ? MyUtility.GetValue.Lookup(string.Format("select PortAir+', '+CountryID from SCIFty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["FromSite"]))) : string.Empty;
                this.destinationPort = MyUtility.Convert.GetString(this.masterData["ToTag"]) == "2" ? MyUtility.GetValue.Lookup(string.Format("select PortAir+', '+CountryID from SCIFty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ToSite"]))) : string.Empty;

                string sqlCmd = string.Format(
                    @"select sum(Qty) as ttlQty,UnitID,sum(Qty*Price) as TtlAmount 
from Express_Detail WITH (NOLOCK) where ID = '{0}'
group by UnitID", MyUtility.Convert.GetString(this.masterData["ID"]));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.detailSummary);
            }
            else if (this.reportType == "4")
            {
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.reportType == "1")
            {
                #region Detail List
                string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P02_Print_DetailList.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[2, 2] = MyUtility.Convert.GetString(this.masterData["ID"]);
                worksheet.Cells[2, 6] = MyUtility.Convert.GetString(this.masterData["ShipMark"]);
                worksheet.Cells[2, 10] = MyUtility.Convert.GetString(this.masterData["AddName"]);
                worksheet.Cells[2, 11] = this.handleName;
                worksheet.Cells[3, 2] = MyUtility.Convert.GetString(this.masterData["FromTag"]) == "1" ? "Factory" : "Brand";
                worksheet.Cells[3, 3] = MyUtility.Convert.GetString(this.masterData["FromSite"]);
                worksheet.Cells[3, 10] = MyUtility.Convert.GetString(this.masterData["Manager"]);
                worksheet.Cells[3, 11] = this.managerName;
                worksheet.Cells[4, 2] = MyUtility.Convert.GetString(this.masterData["ToTag"]) == "1" ? "SCI" : MyUtility.Convert.GetString(this.masterData["ToTag"]) == "2" ? "Factory" : MyUtility.Convert.GetString(this.masterData["ToTag"]) == "3" ? "Supplier" : "Brand";
                worksheet.Cells[4, 3] = MyUtility.Convert.GetString(this.masterData["ToSite"]);
                worksheet.Cells[4, 4] = MyUtility.Convert.GetString(this.masterData["ToTag"]) == "3" ? MyUtility.GetValue.Lookup("AbbEN", MyUtility.Convert.GetString(this.masterData["ToSite"]), "Supp", "ID") : MyUtility.Convert.GetString(this.masterData["ToTag"]) == "4" ? MyUtility.GetValue.Lookup("NameEN", MyUtility.Convert.GetString(this.masterData["ToSite"]), "Brand", "ID") : string.Empty;
                worksheet.Cells[4, 10] = MyUtility.Convert.GetString(this.masterData["Dest"]);
                worksheet.Cells[4, 11] = this.destination;
                worksheet.Cells[5, 2] = MyUtility.Check.Empty(this.masterData["ShipDate"]) ? string.Empty : Convert.ToDateTime(this.masterData["ShipDate"]).ToString("d");
                worksheet.Cells[5, 6] = MyUtility.Convert.GetString(this.masterData["CarrierID"]) + "    " + this.carrier;
                worksheet.Cells[5, 10] = MyUtility.Convert.GetString(this.masterData["ExpressACNo"]);
                worksheet.Cells[6, 2] = MyUtility.Check.Empty(this.masterData["ETD"]) ? string.Empty : Convert.ToDateTime(this.masterData["ETD"]).ToString("d");
                worksheet.Cells[6, 6] = MyUtility.Convert.GetString(this.masterData["CTNQty"]);
                worksheet.Cells[6, 10] = MyUtility.Convert.GetString(this.masterData["BLNo"]);
                worksheet.Cells[7, 2] = MyUtility.Check.Empty(this.masterData["ETA"]) ? string.Empty : Convert.ToDateTime(this.masterData["ETA"]).ToString("d");
                worksheet.Cells[7, 6] = MyUtility.Convert.GetString(this.masterData["NW"]);
                worksheet.Cells[7, 10] = MyUtility.Check.Empty(this.masterData["StatusUpdateDate"]) ? string.Empty : Convert.ToDateTime(this.masterData["StatusUpdateDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
                worksheet.Cells[8, 2] = MyUtility.Convert.GetString(this.masterData["Remark"]);
                worksheet.Cells[8, 10] = MyUtility.Check.Empty(this.masterData["SendDate"]) ? string.Empty : Convert.ToDateTime(this.masterData["SendDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));

                int rownum = 11;
                object[,] objArray = new object[1, 16];
                foreach (DataRow dr in this.detailData.Rows)
                {
                    objArray[0, 0] = dr["OrderID"];
                    objArray[0, 1] = dr["Seq1"];
                    objArray[0, 2] = dr["Seq2"];
                    objArray[0, 3] = dr["StyleID"];
                    objArray[0, 4] = dr["RefNo"];
                    objArray[0, 5] = dr["Supplier"];
                    objArray[0, 6] = dr["Description"];
                    objArray[0, 7] = dr["CTNNo"];
                    objArray[0, 8] = dr["NW"];
                    objArray[0, 9] = dr["Price"];
                    objArray[0, 10] = dr["Qty"];
                    objArray[0, 11] = dr["UnitID"];
                    objArray[0, 12] = dr["CategoryName"];
                    objArray[0, 13] = dr["PackingListID"];
                    objArray[0, 14] = dr["AirPPno"];
                    objArray[0, 15] = dr["Remark"];
                    worksheet.Range[string.Format("A{0}:P{0}", rownum)].Value2 = objArray;

                    rownum++;
                }

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Shipping_P02_Print_DetailList");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                #endregion
            }
            else if (this.reportType == "2")
            {
                #region Packing List
                string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P02_Print_PackingList.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[1, 1] = this.mdivisionName;
                worksheet.Cells[2, 1] = this.mdivisionAddr;
                worksheet.Cells[3, 1] = this.mdivisionTel;
                worksheet.Cells[5, 4] = "Date: " + (MyUtility.Check.Empty(this.masterData["ShipDate"]) ? string.Empty : Convert.ToDateTime(this.masterData["ShipDate"]).ToString(string.Format("{0}", Env.Cfg.DateStringFormat)));
                worksheet.Cells[6, 1] = this.messrs;
                worksheet.Cells[6, 2] = "Invoice No.:" + MyUtility.Convert.GetString(this.masterData["FtyInvNo"]);
                worksheet.Cells[9, 1] = this.courierAWB;
                worksheet.Cells[8, 4] = MyUtility.Check.Empty(this.masterData["ETD"]) ? string.Empty : Convert.ToDateTime(this.masterData["ETD"]).ToString("d");
                worksheet.Cells[9, 4] = MyUtility.Convert.GetString(this.masterData["ID"]);
                worksheet.Cells[11, 1] = this.shipmentPort;
                worksheet.Cells[11, 2] = this.destinationPort;

                int rownum = 14;
                string ctnNo = string.Empty;
                object[,] objArray = new object[1, 7];
                foreach (DataRow dr in this.detailData.Rows)
                {
                    if (ctnNo != MyUtility.Convert.GetString(dr["CTNNo"]))
                    {
                        worksheet.Cells[rownum, 1] = "CTN#" + MyUtility.Convert.GetString(dr["CTNNo"]);
                        rownum++;
                        ctnNo = MyUtility.Convert.GetString(dr["CTNNo"]);
                    }

                    objArray[0, 0] = string.Format("{0}-{1}\r\n{2}", MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"]), MyUtility.Convert.GetString(dr["Description"]));
                    objArray[0, 1] = MyUtility.Check.Empty(dr["Qty"]) ? string.Empty : dr["Qty"];
                    objArray[0, 2] = dr["UnitID"];
                    objArray[0, 3] = "$";
                    objArray[0, 4] = MyUtility.Check.Empty(dr["Price"]) ? string.Empty : dr["Price"];
                    objArray[0, 5] = "$";
                    objArray[0, 6] = MyUtility.Check.Empty(dr["Qty"]) || MyUtility.Check.Empty(dr["Price"]) ? string.Empty : MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["Price"]));
                    worksheet.Range[string.Format("A{0}:G{0}", rownum)].Value2 = objArray;
                    rownum++;
                }

                if (this.detailSummary != null && this.detailSummary.Rows.Count > 0)
                {
                    int count = 1;
                    foreach (DataRow dr in this.detailSummary.Rows)
                    {
                        if (count == 1)
                        {
                            count++;
                            worksheet.Cells[rownum, 1] = "Total";

                            // 文字靠左顯示
                            worksheet.Range[string.Format("A{0}:A{0}", rownum)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }

                        worksheet.Cells[rownum, 2] = MyUtility.Convert.GetString(dr["ttlQty"]);
                        worksheet.Cells[rownum, 3] = MyUtility.Convert.GetString(dr["UnitID"]);
                        worksheet.Cells[rownum, 6] = MyUtility.Check.Empty(dr["TtlAmount"]) ? string.Empty : "$";
                        worksheet.Cells[rownum, 7] = MyUtility.Check.Empty(dr["TtlAmount"]) ? string.Empty : MyUtility.Convert.GetString(dr["TtlAmount"]);
                        rownum++;
                    }
                }

                rownum++;
                worksheet.Cells[rownum, 1] = "Samples of No Commercial Value, the value for Customs Purpose only.";
                rownum = rownum + 3;
                worksheet.Cells[rownum, 1] = "Total Carton Qty: " + MyUtility.Convert.GetString(this.masterData["CTNQty"]);
                worksheet.Cells[rownum + 1, 1] = "Total N.W.: " + MyUtility.Convert.GetString(this.masterData["NW"]);
                worksheet.Cells[rownum + 2, 1] = "Total G.W.: " + MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(this.masterData["NW"]) + MyUtility.Convert.GetDecimal(this.masterData["CTNNW"]));

                worksheet.Range[string.Format("A5:A{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("A5:A{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = 1;
                worksheet.Range[string.Format("A{0}:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("A{0}:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
                worksheet.Range[string.Format("G14:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("G14:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = 1;
                worksheet.Range[string.Format("G14:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("G14:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = 1;

                // 合併儲存格,文字置左
                worksheet.Range[string.Format("C{0}:G{0}", MyUtility.Convert.GetString(rownum + 4))].Merge(Type.Missing);
                worksheet.Range[string.Format("C{0}:G{0}", MyUtility.Convert.GetString(rownum + 4))].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.Cells[rownum + 4, 3] = "            BY:";
                worksheet.Range[string.Format("E{0}:F{0}", rownum + 5)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("E{0}:F{0}", rownum + 5)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Shipping_P02_Print_PackingList");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                #endregion
            }
            else if (this.reportType == "3")
            {
                #region Detail Packing List
                string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P02_Print_DetailPackingList.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[1, 1] = this.mdivisionName;
                worksheet.Cells[2, 1] = this.mdivisionAddr;
                worksheet.Cells[3, 1] = this.mdivisionTel;
                worksheet.Cells[5, 9] = "Date: " + (MyUtility.Check.Empty(this.masterData["ShipDate"]) ? string.Empty : Convert.ToDateTime(this.masterData["ShipDate"]).ToString(string.Format("{0}", Env.Cfg.DateStringFormat)));
                worksheet.Cells[6, 1] = this.messrs;
                worksheet.Cells[6, 9] = "Invoice No.:" + MyUtility.Convert.GetString(this.masterData["FtyInvNo"]);
                worksheet.Cells[9, 1] = this.courierAWB;
                worksheet.Cells[8, 9] = MyUtility.Check.Empty(this.masterData["ETD"]) ? string.Empty : Convert.ToDateTime(this.masterData["ETD"]).ToString("d");
                worksheet.Cells[9, 9] = MyUtility.Convert.GetString(this.masterData["ID"]);
                worksheet.Cells[11, 1] = this.shipmentPort;
                worksheet.Cells[11, 7] = this.destinationPort;

                int rownum = 14;
                object[,] objArray = new object[1, 12];
                foreach (DataRow dr in this.detailData.Rows)
                {
                    string sSeq1 = MyUtility.Convert.GetString(dr["Seq1"]);
                    string sSeq2 = MyUtility.Convert.GetString(dr["Seq2"]);
                    int iCategory = MyUtility.Convert.GetInt(dr["Category"]);
                    bool bQty = MyUtility.Check.Empty(dr["Qty"]);
                    bool bPrice = MyUtility.Check.Empty(dr["Price"]);
                    decimal decQty = bQty ? 0 : MyUtility.Convert.GetDecimal(dr["Qty"]);
                    decimal decPrice = bPrice ? 0 : MyUtility.Convert.GetDecimal(dr["Price"]);

                    objArray[0, 0] = dr["CTNNo"];
                    objArray[0, 1] = string.Format("{0}-{1}", sSeq1, sSeq2);
                    objArray[0, 2] = this.CategoryName(iCategory);
                    objArray[0, 3] = dr["StyleID"];
                    objArray[0, 4] = dr["RefNo"];
                    objArray[0, 5] = iCategory == 4 || iCategory == 9 ? dr["MtlDesc"] : dr["Description"];
                    objArray[0, 6] = bQty ? string.Empty : dr["Qty"];
                    objArray[0, 7] = dr["UnitID"];
                    objArray[0, 8] = "$";
                    objArray[0, 9] = bPrice ? string.Empty : dr["Price"];
                    objArray[0, 10] = "$";
                    objArray[0, 11] = bQty || bPrice ? string.Empty : MyUtility.Convert.GetString(decQty * decPrice);
                    worksheet.Range[string.Format("A{0}:L{0}", rownum)].Value2 = objArray;
                    rownum++;
                }

                if (this.detailSummary != null && this.detailSummary.Rows.Count > 0)
                {
                    int count = 1;
                    foreach (DataRow dr in this.detailSummary.Rows)
                    {
                        worksheet.Range[string.Format("A{0}:F{0}", MyUtility.Convert.GetString(rownum))].Merge(Type.Missing);
                        if (count == 1)
                        {
                            count++;
                            worksheet.Cells[rownum, 1] = "Total";

                            // 文字靠左顯示
                            worksheet.Range[string.Format("A{0}:F{0}", rownum)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }

                        worksheet.Cells[rownum, 7] = MyUtility.Convert.GetString(dr["ttlQty"]);
                        worksheet.Cells[rownum, 8] = MyUtility.Convert.GetString(dr["UnitID"]);
                        worksheet.Cells[rownum, 11] = MyUtility.Check.Empty(dr["TtlAmount"]) ? string.Empty : "$";
                        worksheet.Cells[rownum, 12] = MyUtility.Check.Empty(dr["TtlAmount"]) ? string.Empty : MyUtility.Convert.GetString(dr["TtlAmount"]);
                        rownum++;
                    }
                }

                for (int i = 0; i < 7; i++)
                {
                    worksheet.Range[string.Format("A{0}:F{0}", MyUtility.Convert.GetString(rownum + i))].Merge(Type.Missing);
                }

                rownum++;
                worksheet.Cells[rownum, 1] = "Samples of No Commercial Value, the value for Customs Purpose only.";
                rownum = rownum + 3;
                worksheet.Cells[rownum, 1] = "Total Carton Qty: " + MyUtility.Convert.GetString(this.masterData["CTNQty"]);
                worksheet.Cells[rownum + 1, 1] = "Total N.W.: " + MyUtility.Convert.GetString(this.masterData["NW"]);
                worksheet.Cells[rownum + 2, 1] = "Total G.W.: " + MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(this.masterData["NW"]) + MyUtility.Convert.GetDecimal(this.masterData["CTNNW"]));

                worksheet.Range[string.Format("A5:A{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("A5:A{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = 1;
                worksheet.Range[string.Format("A{0}:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("A{0}:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
                worksheet.Range[string.Format("L14:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("L14:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = 1;
                worksheet.Range[string.Format("L14:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("L14:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = 1;

                // 合併儲存格,文字置左
                worksheet.Range[string.Format("H{0}:L{0}", MyUtility.Convert.GetString(rownum + 4))].Merge(Type.Missing);
                worksheet.Range[string.Format("H{0}:L{0}", MyUtility.Convert.GetString(rownum + 4))].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.Cells[rownum + 4, 8] = "            BY:";
                worksheet.Range[string.Format("J{0}:K{0}", rownum + 5)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format("J{0}:K{0}", rownum + 5)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Shipping_P02_Print_DetailPackingList");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                #endregion
            }
            else if (this.reportType == "4")
            {
                string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P02_Print_DHL_XCCA.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[2, 2] = MyUtility.Convert.GetString(this.masterData["BLNo"]);
                worksheet.Cells[4, 10] = MyUtility.Convert.GetDecimal(this.masterData["NW"]) + MyUtility.Convert.GetDecimal(this.masterData["CTNNW"]);

                int rownum = 6;

                // 表身有資料才需要重新排序 ISP20191502
                if (this.detailData.Rows.Count > 0)
                {
                    this.detailData = this.detailData.AsEnumerable().OrderBy(o => Convert.ToInt16(o["OrderNumber"])).CopyToDataTable();
                }

                foreach (DataRow dr in this.detailData.Rows)
                {
                    // string sSeq1 = MyUtility.Convert.GetString(dr["Seq1"]);
                    // string sSeq2 = MyUtility.Convert.GetString(dr["Seq2"]);
                    int iCategory = MyUtility.Convert.GetInt(dr["Category"]);
                    bool bQty = MyUtility.Check.Empty(dr["Qty"]);
                    bool bPrice = MyUtility.Check.Empty(dr["Price"]);
                    decimal decQty = bQty ? 0 : MyUtility.Convert.GetDecimal(dr["Qty"]);
                    decimal decPrice = bPrice ? 0 : MyUtility.Convert.GetDecimal(dr["Price"]);

                    worksheet.Cells[rownum, 1] = MyUtility.Convert.GetString(dr["OrderNumber"]);
                    worksheet.Cells[rownum, 2] = MyUtility.Convert.GetString(dr["CategoryNameFromDD"]);
                    bool isnDescription = false;
                    worksheet.Cells[rownum, 3] = this.GetDescription(iCategory, dr, out isnDescription);
                    if (isnDescription)
                    {
                        worksheet.Cells[rownum, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    }

                    worksheet.Cells[rownum, 8] = "USD";
                    worksheet.Cells[rownum, 9] = decPrice;
                    worksheet.Cells[rownum, 10] = decQty;
                    worksheet.Cells[rownum, 11] = dr["UnitID"];
                    worksheet.Cells[rownum, 12] = dr["NW"];
                    if (!MyUtility.Check.Empty(dr["Orderid"]))
                    {
                        worksheet.Cells[rownum, 13] = MyUtility.GetValue.Lookup($"select CountryID from Factory f with(nolock) join orders o with(nolock) on o.FtyGroup = f.ID where o.id ='{dr["OrderID"]}' ");
                    }
                    else
                    {
                        worksheet.Cells[rownum, 13].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    }

                    rownum++;
                }

                worksheet.Rows.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Shipping_P02_Print_DHL_XCCA");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
            }

            return true;
        }

        private string CategoryName(int iCategory)
        {
            string rtnStr = string.Empty;
            switch (iCategory)
            {
                case 1:
                    rtnStr = "Sample";
                    break;
                case 2:
                    rtnStr = "SMS";
                    break;
                case 3:
                    rtnStr = "Bulk";
                    break;
                case 4:
                    rtnStr = "Material";
                    break;
                case 5:
                    rtnStr = "Dox";
                    break;
                case 6:
                    rtnStr = "Machine/Parts";
                    break;
                case 7:
                    rtnStr = "Mock Up";
                    break;
                case 8:
                    rtnStr = this.reportType == "4" ? "Sample" : "Other Sample";
                    break;
                case 9:
                    rtnStr = this.reportType == "4" ? "Material" : "Other Material";
                    break;
                default:
                    rtnStr = string.Empty;
                    break;
            }

            return rtnStr;
        }

        private string GetDescription(int iCategory, DataRow dr, out bool isnDescription)
        {
            string sql = string.Empty;
            if (iCategory == 4 || iCategory == 9)
            {
                sql = $@"select Description = dbo.getMtlDesc('{dr["OrderID"]}', '{dr["Seq1"]}', '{dr["Seq2"]}', 1, 0)";
            }
            else
            {
                sql = $@"
select Description = dbo.getBOFMtlDesc(isnull(
		(select o.StyleUkey from orders o with(nolock) where o.id ='{dr["OrderID"]}'),
		(select ukey from Style with(nolock) where id='{dr["StyleID"]}' and BrandID='{dr["BrandID"]}' and SeasonID ='{dr["SeasonID"]}')
	)) 
";
            }

            string findDescription = MyUtility.GetValue.Lookup(sql);
            isnDescription = MyUtility.Check.Empty(findDescription);
            return MyUtility.Check.Empty(findDescription) ? MyUtility.Convert.GetString(dr["nDescription"]) : findDescription;
        }
    }
}
