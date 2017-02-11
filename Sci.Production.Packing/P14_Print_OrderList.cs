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
            if (ListCheck.Checked)
            {
                #region ListCheck
                toExcel("Packing_P14.xltx", 1);
                #endregion
            }
            if (SlipCheck.Checked)
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
                                CartonNum = string.Join(", ", m.Select(r => r["CTNStartNo"].ToString()))
                            }).ToList();
                //
                string sql = @"

                select  t.TTL_Qty, 
                        t.PackID, 
                        t.OrderID, 
                        t.PONo, 
                        ttlCtn = (select count(*) from PackingList_detail pk where t.PackID = pk.ID and t.OrderID = pk.OrderID and ctnQty > 0),
                        t.Dest,
                        t.BuyerDelivery,
                        t.CartonNum
                from  #Tmp t
            ";

                DataTable k;
                MyUtility.Tool.ProcessWithObject(Slip, "", sql, out k, "#Tmp");
                dt = k;
                toExcel("Packing_P14_TransferSlip.xltx", 3);
                #endregion
            }
            return true;
        }

        private void toExcel(string xltFile, int headerRow)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + xltFile); //預先開啟excel app
            bool result = MyUtility.Excel.CopyToXls(dt, "", xltfile: xltFile, headerRow: headerRow, excelApp: objApp);
            if (!result) { 
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }
            else if (xltFile.EqualString("Packing_P14_TransferSlip.xltx"))
            {
                Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                if (date1 != null && date2 != null)
                    objSheets.Cells[2, 2] = date1 + " ~ " + date2;
                if (packID != null)
                    objSheets.Cells[2, 5] = packID;
                if (SPNo != null)
                    objSheets.Cells[2, 8] = SPNo;

                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                
            }
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
