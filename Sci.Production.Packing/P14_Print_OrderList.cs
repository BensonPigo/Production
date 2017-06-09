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
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    public partial class P14_Print_OrderList : Sci.Win.Tems.QueryForm
    {
        DataTable dt;
        string date1, date2, packID, SPNo;
        public P14_Print_OrderList(DataTable dt, string date1, string date2, string packID, string SPNo)
        {
            this.dt = dt;
            this.date1 = date1;
            this.date2 = date2;
            this.packID = packID;
            this.SPNo = SPNo;
            InitializeComponent();
            EditMode = true;
        }

        private bool ToExcel()
        {
            if (radioTransferList.Checked)
            {
                #region ListCheck
                toExcel("Packing_P14.xltx", 4, dt);
                #endregion
            }
            if (radioTransferSlip.Checked)
            {
                #region SlipCheck
                var Slip = (from p in dt.AsEnumerable()
                            group p by new
                            {
                                PackingListID = p["PackingListID"].ToString(),
                                OrderID = p["OrderID"].ToString()
                            } into m
                            select new PackData
                            {
                                TTL_Qty = m.Count(r => !r["CTNStartNo"].Empty()).ToString(),
                                PackID = m.First()["PackingListID"].ToString(),
                                OrderID = m.First()["OrderID"].ToString(),
                                PONo = m.First()["CustPONo"].ToString(),
                                Dest = m.First()["Dest"].ToString(),
                                BuyerDelivery = m.First()["BuyerDelivery"].ToString(),
                                CartonNum = string.Join(", ", m.Select(r => r["CTNStartNo"].ToString().Trim()))
                            }).ToList();
                //
                string sql = @"

                select  t.TTL_Qty, 
                        t.PackID, 
                        t.OrderID, 
                        t.PONo, 
                        ttlCtn = (select count(*) from PackingList_detail pk WITH (NOLOCK) where t.PackID = pk.ID and t.OrderID = pk.OrderID and ctnQty > 0),
                        t.Dest,
                        t.BuyerDelivery,
                        t.CartonNum
                from  #Tmp t
            ";

                DataTable k;
                MyUtility.Tool.ProcessWithObject(Slip, "", sql, out k, "#Tmp");
                toExcel("Packing_P14_TransferSlip.xltx", 4, k);
                #endregion
            }
            return true;
        }

        private void toExcel(string xltFile, int headerRow, DataTable ExcelTable)
        {
            if (ExcelTable == null || ExcelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }
            this.ShowWaitMessage("Excel Processing...");

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + xltFile); //預先開啟excel app
            bool result = MyUtility.Excel.CopyToXls(ExcelTable, "", showExcel: false, xltfile: xltFile, headerRow: headerRow, excelApp: objApp);
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            if (!result) { 
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                return;
            }

            #region Set Login M & Transfer Date
            if (date1 != null && date2 != null)
                objSheets.Cells[3, 2] = date1 + " ~ " + date2;

            string strLoginM = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where ID = '{0}'", Sci.Env.User.Keyword));
            if (!strLoginM.Empty())
            {
                objSheets.Cells[1, 1] = strLoginM;
            }
            #endregion

            if (xltFile.EqualString("Packing_P14_TransferSlip.xltx"))
            {
                decimal sumTTL = 0;
                for (int i = 1; i <= ExcelTable.Rows.Count; i++)
                {
                    sumTTL += Convert.ToDecimal(ExcelTable.Rows[i - 1]["TTL_Qty"]);
                    string str = objSheets.Cells[i + headerRow, 8].Value;
                    if (!MyUtility.Check.Empty(str))
                        objSheets.Cells[i + headerRow, 8] = str.Trim();
                }

                int r = ExcelTable.Rows.Count;
                objSheets.get_Range(string.Format("A5:H{0}", r + 4)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                objSheets.Cells[ExcelTable.Rows.Count + headerRow + 1, 1] = "Sub. TTL CTN:";
                objSheets.Cells[ExcelTable.Rows.Count + headerRow + 1, 2] = sumTTL;
            }
            if (xltFile.EqualString("Packing_P14.xltx"))
            {                
                int r = ExcelTable.Rows.Count;
                objSheets.get_Range(string.Format("A5:L{0}", r + 4)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            }
            objSheets.Columns.AutoFit();
            objSheets.Rows.AutoFit();
            objApp.Visible = true;
            this.HideWaitMessage();

            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            ToExcel();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        public class PackData
        {
            public string TTL_Qty { get; set; }
            public string PackID { get; set; }
            public string OrderID { get; set; }
            public string PONo { get; set; }
            public string Dest { get; set; }
            public string BuyerDelivery { get; set; }
            public string CartonNum { get; set; }
        }
    }
}
