using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;


namespace Sci.Production.Shipping
{
    public partial class P02_Print : Sci.Win.Tems.PrintForm
    {
        DataRow masterData;
        DataTable detailData,detailSummary;
        string reportType, handleName, managerName, destination, carrier, mdivisionName, mdivisionAddr, mdivisionTel, messrs,courierAWB,shipmentPort,destinationPort;
        public P02_Print(DataRow MasterData, DataTable DetailData)
        {
            InitializeComponent();
            masterData = MasterData;
            detailData = DetailData;
            radioButton1.Checked = true;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            reportType = radioButton1.Checked ? "1" : radioButton2.Checked ? "2" : "3";
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (reportType == "1")
            {
                handleName = MyUtility.GetValue.Lookup(string.Format("select Name+ ' #' + ExtNo as Incharge from Pass1 where ID = '{0}'", MyUtility.Convert.GetString(masterData["AddName"])));
                managerName = MyUtility.GetValue.Lookup(string.Format("select Name+ ' #' + ExtNo as Incharge from Pass1 where ID = '{0}'", MyUtility.Convert.GetString(masterData["Manager"])));
                destination = MyUtility.GetValue.Lookup(string.Format("select Alias from Country where ID = '{0}'", MyUtility.Convert.GetString(masterData["Dest"])));
                carrier = MyUtility.GetValue.Lookup(string.Format("select (c.SuppID + '-' + s.AbbEN) as Supplier from Carrier c left join Supp s on c.SuppID = s.ID where c.ID = '{0}'", MyUtility.Convert.GetString(masterData["CarrierID"])));
            }
            else
            {
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format("select NameEN,AddressEN,Tel from Factory where ID = '{0}'", MyUtility.Convert.GetString(masterData["MDivisionID"])), out dr))
                {
                    mdivisionName = MyUtility.Convert.GetString(dr["NameEN"]);
                    mdivisionAddr = MyUtility.Convert.GetString(dr["AddressEN"]);
                    mdivisionTel = MyUtility.Convert.GetString(dr["Tel"]);
                }
                else
                {
                    mdivisionName = "";
                    mdivisionAddr = "";
                    mdivisionTel = "";
                }

                messrs = "";
                if (MyUtility.Convert.GetString(masterData["ToTag"]) == "1" || MyUtility.Convert.GetString(masterData["ToTag"]) == "3")
                {
                    if (MyUtility.Check.Seek(string.Format("select AbbEN,AddressEN,Tel from Supp where ID = '{0}'", MyUtility.Convert.GetString(masterData["ToTag"])), out dr))
                    {
                        messrs = string.Format("{0}\r\n{1}\r\nTEL: {2}", MyUtility.Convert.GetString(dr["AbbEN"]), MyUtility.Convert.GetString(dr["AddressEN"]), MyUtility.Convert.GetString(dr["Tel"]));
                    }
                }
                else if (MyUtility.Convert.GetString(masterData["ToTag"]) == "4")
                {
                    if (MyUtility.Check.Seek(string.Format("select NameEN,AddressEN,Tel from Brand where ID = '{0}'", MyUtility.Convert.GetString(masterData["ToTag"])), out dr))
                    {
                        messrs = string.Format("{0}\r\n{1}\r\nTEL: {2}", MyUtility.Convert.GetString(dr["NameEN"]), MyUtility.Convert.GetString(dr["AddressEN"]), MyUtility.Convert.GetString(dr["Tel"]));
                    }
                }
                else if (MyUtility.Convert.GetString(masterData["ToTag"]) == "2")
                {
                    if (MyUtility.Check.Seek(string.Format("select Abb,AddressEN,Tel from SCIFty where ID = '{0}'", MyUtility.Convert.GetString(masterData["ToTag"])), out dr))
                    {
                        messrs = string.Format("{0}\r\n{1}\r\nTEL: {2}", MyUtility.Convert.GetString(dr["Abb"]), MyUtility.Convert.GetString(dr["AddressEN"]), MyUtility.Convert.GetString(dr["Tel"]));
                    }
                }

                courierAWB = string.Format("{0}# {1}", MyUtility.GetValue.Lookup(string.Format("select AbbEN from Supp where ID = (select SuppID from Carrier where ID = '{0}')", MyUtility.Convert.GetString(masterData["CarrierID"]))), MyUtility.Convert.GetString(masterData["BLNo"]));
                shipmentPort = MyUtility.Convert.GetString(masterData["FromTag"]) == "1" ? MyUtility.GetValue.Lookup(string.Format("select PortAir+', '+CountryID from SCIFty where ID = '{0}'", MyUtility.Convert.GetString(masterData["FromSite"]))) : "";
                destinationPort = MyUtility.Convert.GetString(masterData["ToTag"]) == "2" ? MyUtility.GetValue.Lookup(string.Format("select PortAir+', '+CountryID from SCIFty where ID = '{0}'", MyUtility.Convert.GetString(masterData["ToSite"]))) : "";

                string sqlCmd = string.Format(@"select sum(Qty) as ttlQty,UnitID,sum(Qty*Price) as TtlAmount 
from Express_Detail where ID = '{0}'
group by UnitID", MyUtility.Convert.GetString(masterData["ID"]));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out detailSummary);
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (reportType == "1")
            {
                #region Detail List
                string strXltName = Sci.Env.Cfg.XltPathDir + "Shipping_P02_Print_DetailList.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null) return false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[2, 2] = MyUtility.Convert.GetString(masterData["ID"]);
                worksheet.Cells[2, 6] = MyUtility.Convert.GetString(masterData["ShipMark"]);
                worksheet.Cells[2, 10] = MyUtility.Convert.GetString(masterData["AddName"]);
                worksheet.Cells[2, 11] = handleName;
                worksheet.Cells[3, 2] = MyUtility.Convert.GetString(masterData["FromTag"]) == "1" ? "Factory" : "Brand";
                worksheet.Cells[3, 3] = MyUtility.Convert.GetString(masterData["FromSite"]);
                worksheet.Cells[3, 10] = MyUtility.Convert.GetString(masterData["Manager"]);
                worksheet.Cells[3, 11] = managerName;
                worksheet.Cells[4, 2] = MyUtility.Convert.GetString(masterData["ToTag"]) == "1" ? "SCI" : MyUtility.Convert.GetString(masterData["ToTag"]) == "2" ? "Factory" : MyUtility.Convert.GetString(masterData["ToTag"]) == "3" ? "Supplier" : "Brand";
                worksheet.Cells[4, 3] = MyUtility.Convert.GetString(masterData["ToSite"]);
                worksheet.Cells[4, 10] = MyUtility.Convert.GetString(masterData["Dest"]);
                worksheet.Cells[4, 11] = destination;
                worksheet.Cells[5, 2] = MyUtility.Check.Empty(masterData["ShipDate"]) ? "" : Convert.ToDateTime(masterData["ShipDate"]).ToString("d");
                worksheet.Cells[5, 6] = MyUtility.Convert.GetString(masterData["CarrierID"]) + "    " + carrier;
                worksheet.Cells[5, 10] = MyUtility.Convert.GetString(masterData["ExpressACNo"]);
                worksheet.Cells[6, 2] = MyUtility.Check.Empty(masterData["ETD"]) ? "" : Convert.ToDateTime(masterData["ETD"]).ToString("d");
                worksheet.Cells[6, 6] = MyUtility.Convert.GetString(masterData["CTNQty"]);
                worksheet.Cells[6, 10] = MyUtility.Convert.GetString(masterData["BLNo"]);
                worksheet.Cells[7, 2] = MyUtility.Check.Empty(masterData["ETA"]) ? "" : Convert.ToDateTime(masterData["ETA"]).ToString("d");
                worksheet.Cells[7, 6] = MyUtility.Convert.GetString(masterData["NW"]);
                worksheet.Cells[7, 10] = MyUtility.Check.Empty(masterData["StatusUpdateDate"]) ? "" : Convert.ToDateTime(masterData["StatusUpdateDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat)); ;
                worksheet.Cells[8, 2] = MyUtility.Convert.GetString(masterData["Remark"]);
                worksheet.Cells[8, 10] = MyUtility.Check.Empty(masterData["SendDate"]) ? "" : Convert.ToDateTime(masterData["SendDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat)); ;

                int rownum = 11;
                object[,] objArray = new object[1, 15];
                foreach (DataRow dr in detailData.Rows)
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
                    objArray[0, 13] = dr["DutyNo"];
                    objArray[0, 14] = dr["Remark"];
                    worksheet.Range[String.Format("A{0}:O{0}", rownum)].Value2 = objArray;

                    rownum++;
                }
                excel.Visible = true;
                #endregion
            }
            else if (reportType == "2")
            {
                #region Packing List
                string strXltName = Sci.Env.Cfg.XltPathDir + "Shipping_P02_Print_PackingList.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null) return false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[1, 1] = mdivisionName;
                worksheet.Cells[2, 1] = mdivisionAddr;
                worksheet.Cells[3, 1] = mdivisionTel;
                worksheet.Cells[5, 4] = "Date: " + (MyUtility.Check.Empty(masterData["ShipDate"]) ? "" : Convert.ToDateTime(masterData["ShipDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat)));
                worksheet.Cells[6, 1] = messrs;
                worksheet.Cells[6, 2] = "Invoice No.:" + MyUtility.Convert.GetString(masterData["FtyInvNo"]);
                worksheet.Cells[9, 1] = courierAWB;
                worksheet.Cells[8, 4] = MyUtility.Check.Empty(masterData["ETD"]) ? "" : Convert.ToDateTime(masterData["ETD"]).ToString("d");
                worksheet.Cells[9, 4] = MyUtility.Convert.GetString(masterData["ID"]);
                worksheet.Cells[11, 1] = shipmentPort;
                worksheet.Cells[11, 2] = destinationPort;

                int rownum = 14;
                string ctnNo = "";
                object[,] objArray = new object[1, 7];
                foreach (DataRow dr in detailData.Rows)
                {
                    if (ctnNo != MyUtility.Convert.GetString(dr["CTNNo"]))
                    {
                        worksheet.Cells[rownum, 1] = "CTN#"+MyUtility.Convert.GetString(dr["CTNNo"]);
                        rownum++;
                        ctnNo = MyUtility.Convert.GetString(dr["CTNNo"]);
                    }
                    objArray[0, 0] = string.Format("{0}-{1}\r\n{2}", MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"]), MyUtility.Convert.GetString(dr["Description"]));
                    objArray[0, 1] = MyUtility.Check.Empty(dr["Qty"]) ? "" : dr["Qty"];
                    objArray[0, 2] = dr["UnitID"];
                    objArray[0, 3] = "$";
                    objArray[0, 4] = MyUtility.Check.Empty(dr["Price"]) ? "" : dr["Price"];
                    objArray[0, 5] = "$";
                    objArray[0, 6] = MyUtility.Check.Empty(dr["Qty"]) || MyUtility.Check.Empty(dr["Price"]) ? "" : MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["Price"]));
                    worksheet.Range[String.Format("A{0}:G{0}", rownum)].Value2 = objArray;
                    rownum++;
                }

                if (detailSummary != null && detailSummary.Rows.Count > 0)
                {
                    int count = 1;
                    foreach (DataRow dr in detailSummary.Rows)
                    {
                        if (count == 1)
                        {
                            count++;
                            worksheet.Cells[rownum, 1] = "Total";
                            //文字靠左顯示
                            worksheet.Range[String.Format("A{0}:A{0}", rownum)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }
                        worksheet.Cells[rownum, 2] = MyUtility.Convert.GetString(dr["ttlQty"]);
                        worksheet.Cells[rownum, 3] = MyUtility.Convert.GetString(dr["UnitID"]);
                        worksheet.Cells[rownum, 6] = MyUtility.Check.Empty(dr["TtlAmount"]) ? "" : "$";
                        worksheet.Cells[rownum, 7] = MyUtility.Check.Empty(dr["TtlAmount"]) ? "" : MyUtility.Convert.GetString(dr["TtlAmount"]);
                        rownum++;
                    }
                }
                rownum++;
                worksheet.Cells[rownum, 1] = "Samples of No Commercial Value, the value for Customs Purpose only.";
                rownum = rownum + 3;
                worksheet.Cells[rownum, 1] = "Total Carton Qty: " + MyUtility.Convert.GetString(masterData["CTNQty"]);
                worksheet.Cells[rownum+1, 1] = "Total N.W.: " + MyUtility.Convert.GetString(masterData["NW"]);
                worksheet.Cells[rownum + 2, 1] = "Total G.W.: " + MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(masterData["NW"]) + MyUtility.Convert.GetDecimal(masterData["CTNNW"]));

                worksheet.Range[String.Format("A5:A{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("A5:A{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = 1;
                worksheet.Range[String.Format("A{0}:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("A{0}:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
                worksheet.Range[String.Format("G14:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("G14:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = 1;
                worksheet.Range[String.Format("G14:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("G14:G{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = 1;
                //合併儲存格,文字置左
                worksheet.Range[String.Format("C{0}:G{0}", MyUtility.Convert.GetString(rownum + 4))].Merge(Type.Missing);
                worksheet.Range[String.Format("C{0}:G{0}", MyUtility.Convert.GetString(rownum + 4))].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.Cells[rownum + 4, 3] = "            BY:";
                worksheet.Range[String.Format("E{0}:F{0}", rownum + 5)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("E{0}:F{0}", rownum + 5)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;
                
                excel.Visible = true;
                #endregion
            }
            else
            {
                #region Detail Packing List
                string strXltName = Sci.Env.Cfg.XltPathDir + "Shipping_P02_Print_DetailPackingList.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null) return false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[1, 1] = mdivisionName;
                worksheet.Cells[2, 1] = mdivisionAddr;
                worksheet.Cells[3, 1] = mdivisionTel;
                worksheet.Cells[5, 9] = "Date: " + (MyUtility.Check.Empty(masterData["ShipDate"]) ? "" : Convert.ToDateTime(masterData["ShipDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat)));
                worksheet.Cells[6, 1] = messrs;
                worksheet.Cells[6, 9] = "Invoice No.:" + MyUtility.Convert.GetString(masterData["FtyInvNo"]);
                worksheet.Cells[9, 1] = courierAWB;
                worksheet.Cells[8, 9] = MyUtility.Check.Empty(masterData["ETD"]) ? "" : Convert.ToDateTime(masterData["ETD"]).ToString("d");
                worksheet.Cells[9, 9] = MyUtility.Convert.GetString(masterData["ID"]);
                worksheet.Cells[11, 1] = shipmentPort;
                worksheet.Cells[11, 7] = destinationPort;

                int rownum = 14;
                object[,] objArray = new object[1, 12];
                foreach (DataRow dr in detailData.Rows)
                {
                    objArray[0, 0] = dr["CTNNo"];
                    objArray[0, 1] = string.Format("{0}-{1}", MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"]));
                    objArray[0, 2] = dr["Category"];
                    objArray[0, 3] = dr["StyleID"];
                    objArray[0, 4] = dr["RefNo"];
                    objArray[0, 5] = dr["Description"];
                    objArray[0, 6] = MyUtility.Check.Empty(dr["Qty"]) ? "" : dr["Qty"];
                    objArray[0, 7] = dr["UnitID"];
                    objArray[0, 8] = "$";
                    objArray[0, 9] = MyUtility.Check.Empty(dr["Price"]) ? "" : dr["Price"];
                    objArray[0, 10] = "$";
                    objArray[0, 11] = MyUtility.Check.Empty(dr["Qty"]) || MyUtility.Check.Empty(dr["Price"]) ? "" : MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["Price"]));
                    worksheet.Range[String.Format("A{0}:L{0}", rownum)].Value2 = objArray;
                    rownum++;
                }

                if (detailSummary != null && detailSummary.Rows.Count > 0)
                {
                    int count = 1;
                    foreach (DataRow dr in detailSummary.Rows)
                    {
                        worksheet.Range[String.Format("A{0}:F{0}", MyUtility.Convert.GetString(rownum))].Merge(Type.Missing);
                        if (count == 1)
                        {
                            count++;
                            worksheet.Cells[rownum, 1] = "Total";
                            //文字靠左顯示
                            worksheet.Range[String.Format("A{0}:F{0}", rownum)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }
                        worksheet.Cells[rownum, 7] = MyUtility.Convert.GetString(dr["ttlQty"]);
                        worksheet.Cells[rownum, 8] = MyUtility.Convert.GetString(dr["UnitID"]);
                        worksheet.Cells[rownum, 11] = MyUtility.Check.Empty(dr["TtlAmount"]) ? "" : "$";
                        worksheet.Cells[rownum, 12] = MyUtility.Check.Empty(dr["TtlAmount"]) ? "" : MyUtility.Convert.GetString(dr["TtlAmount"]);
                        rownum++;
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    worksheet.Range[String.Format("A{0}:F{0}", MyUtility.Convert.GetString(rownum + i))].Merge(Type.Missing);
                }
                    
                rownum++;
                worksheet.Cells[rownum, 1] = "Samples of No Commercial Value, the value for Customs Purpose only.";
                rownum = rownum + 3;
                worksheet.Cells[rownum, 1] = "Total Carton Qty: " + MyUtility.Convert.GetString(masterData["CTNQty"]);
                worksheet.Cells[rownum + 1, 1] = "Total N.W.: " + MyUtility.Convert.GetString(masterData["NW"]);
                worksheet.Cells[rownum + 2, 1] = "Total G.W.: " + MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(masterData["NW"]) + MyUtility.Convert.GetDecimal(masterData["CTNNW"]));

                worksheet.Range[String.Format("A5:A{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("A5:A{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = 1;
                worksheet.Range[String.Format("A{0}:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("A{0}:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
                worksheet.Range[String.Format("L14:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("L14:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = 1;
                worksheet.Range[String.Format("L14:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("L14:L{0}", rownum + 2)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = 1;
                //合併儲存格,文字置左
                worksheet.Range[String.Format("H{0}:L{0}", MyUtility.Convert.GetString(rownum + 4))].Merge(Type.Missing);
                worksheet.Range[String.Format("H{0}:L{0}", MyUtility.Convert.GetString(rownum + 4))].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.Cells[rownum + 4, 8] = "            BY:";
                worksheet.Range[String.Format("J{0}:K{0}", rownum + 5)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("J{0}:K{0}", rownum + 5)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;

                excel.Visible = true;
                #endregion
            }

            return true;
        }
    }
}
