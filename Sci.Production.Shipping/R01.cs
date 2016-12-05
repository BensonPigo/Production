using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        string shipper, brand, shipmode, shipterm, dest, status,reportType;
        DateTime? invdate1, invdate2, etd1, etd2;
        DataTable printData;

        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox1, 1, factory);
            comboBox1.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(comboBox2, 1, 1, "All,Confirmed,UnConfirmed");
            comboBox2.SelectedIndex = 0;
            txtshipmode1.SelectedIndex = -1;
            radioButton1.Checked = true;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Invoice Date can't empty!!");
                return false;
            }

            shipper = comboBox1.Text;
            brand = txtbrand1.Text;
            shipmode = txtshipmode1.Text;
            shipterm = txtshipterm1.Text;
            dest = txtcountry1.TextBox1.Text;
            status = comboBox2.Text;
            reportType = radioButton1.Checked ? "1" : "2";
            invdate1 = dateRange1.Value1;
            invdate2 = dateRange1.Value2;
            etd1 = dateRange2.Value1;
            etd2 = dateRange2.Value2;
           
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            if (reportType == "1")
            {
                sqlCmd.Append(string.Format(@"select g.ID,g.Shipper,g.BrandID,g.InvDate,g.FCRDate,g.CustCDID,(g.Dest+' - '+isnull(c.Alias,'')) as Dest,g.ShipModeID,g.ShipTermID,
g.Handle,(g.Forwarder+' - '+isnull(ls.Abb,'')) as Forwarder,g.Vessel,g.ETD,g.ETA,g.SONo,g.SOCFMDate,g.TotalShipQty,g.TotalCTNQty,
isnull((select CTNRNo+'/'+TruckNo+',' from GMTBooking_CTNR where ID = g.ID for xml path('')),'') as CTNTruck,
g.TotalGW,g.TotalCBM,g.AddDate,IIF(g.Status = 'Confirmed',g.EditDate,null) as ConfirmDate,g.Remark
from GMTBooking g
left join Country c on c.ID = g.Dest
left join LocalSupp ls on ls.ID = g.Forwarder
where g.InvDate between '{0}' and '{1}'", Convert.ToDateTime(invdate1).ToString("d"), Convert.ToDateTime(invdate2).ToString("d")));
            }
            else
            {
                sqlCmd.Append(string.Format(@"select g.ID,g.Shipper,g.BrandID,g.InvDate,pl.MDivisionID,isnull(pl.ID,'') as PackID,pl.CargoReadyDate,pl.PulloutDate,
isnull(pl.ShipQty,0) as ShipQty,isnull(pl.CTNQty,0) as CTNQty,isnull(pl.GW,0) as GW,isnull(pl.CBM,0) as CBM,g.CustCDID,
(g.Dest+' - '+isnull(c.Alias,'')) as Dest,IIF(g.Status = 'Confirmed',g.EditDate,null) as ConfirmDate,
g.AddName+' '+isnull(p.Name,'') as AddName,g.AddDate,g.Remark,
isnull((select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd where pd.ID = pl.ID) a for xml path('')),'') as OrderID,
(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = pl.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as BuyerDelivery,
(select oq.SDPDate from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = pl.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as SDPDate
from GMTBooking g
left join PackingList pl on pl.INVNo = g.ID
left join Country c on c.ID = g.Dest
left join Pass1 p on p.ID = g.AddName
where pl.ID<>'' and g.InvDate between '{0}' and '{1}' ", Convert.ToDateTime(invdate1).ToString("d"), Convert.ToDateTime(invdate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(shipper))
            {
                sqlCmd.Append(string.Format(" and g.Shipper = '{0}'", shipper));
            }
            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", brand));
            }
            if (!MyUtility.Check.Empty(shipmode))
            {
                sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", shipmode));
            }
            if (!MyUtility.Check.Empty(shipterm))
            {
                sqlCmd.Append(string.Format(" and g.ShipTermID = '{0}'", shipterm));
            }
            if (!MyUtility.Check.Empty(dest))
            {
                sqlCmd.Append(string.Format(" and g.Dest = '{0}'", dest));
            }
            if (!MyUtility.Check.Empty(etd1))
            {
                sqlCmd.Append(string.Format(" and g.ETD between '{0}' and '{1}'", Convert.ToDateTime(etd1).ToString("d"), Convert.ToDateTime(etd2).ToString("d")));
            }
            if (status == "Confirmed")
            {
                sqlCmd.Append(" and g.Status = 'Confirmed'");
            }
            else if (status == "UnConfirmed")
            {
                sqlCmd.Append(" and g.Status <> 'Confirmed'");
            }
            sqlCmd.Append(" order by g.ID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + (reportType == "1" ? "\\Shipping_R01_MainList.xltx" : "\\Shipping_R01_DetailList.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            
            //填內容值
            if (reportType == "1")
            {
                int intRowsStart = 3;
                object[,] objArray = new object[1, 24];
                foreach (DataRow dr in printData.Rows)
                {
                    objArray[0, 0] = dr["ID"];
                    objArray[0, 1] = dr["Shipper"];
                    objArray[0, 2] = dr["BrandID"];
                    objArray[0, 3] = dr["InvDate"];
                    objArray[0, 4] = dr["FCRDate"];
                    objArray[0, 5] = dr["CustCDID"];
                    objArray[0, 6] = dr["Dest"];
                    objArray[0, 7] = dr["ShipModeID"];
                    objArray[0, 8] = dr["ShipTermID"];
                    objArray[0, 9] = dr["Handle"];
                    objArray[0, 10] = dr["Forwarder"];
                    objArray[0, 11] = dr["Vessel"];
                    objArray[0, 12] = dr["ETD"];
                    objArray[0, 13] = dr["ETA"];
                    objArray[0, 14] = dr["SONo"];
                    objArray[0, 15] = dr["SOCFMDate"];
                    objArray[0, 16] = MyUtility.Check.Empty(dr["CTNTruck"]) ? dr["CTNTruck"] : MyUtility.Convert.GetString(dr["CTNTruck"]).Substring(0, MyUtility.Convert.GetString(dr["CTNTruck"]).Length - 1);
                    objArray[0, 17] = dr["TotalShipQty"];
                    objArray[0, 18] = dr["TotalCTNQty"];
                    objArray[0, 19] = dr["TotalGW"];
                    objArray[0, 20] = dr["TotalCBM"];
                    objArray[0, 21] = dr["AddDate"];
                    objArray[0, 22] = dr["ConfirmDate"];
                    objArray[0, 23] = dr["Remark"];
                    worksheet.Range[String.Format("A{0}:X{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            else
            {
                int intRowsStart = 3;
                object[,] objArray = new object[1, 21];
                foreach (DataRow dr in printData.Rows)
                {
                    objArray[0, 0] = dr["ID"];
                    objArray[0, 1] = dr["Shipper"];
                    objArray[0, 2] = dr["BrandID"];
                    objArray[0, 3] = dr["InvDate"];
                    objArray[0, 4] = dr["MDivisionID"];
                    objArray[0, 5] = dr["PackID"];
                    objArray[0, 6] = MyUtility.Check.Empty(dr["OrderID"]) ? dr["OrderID"] : MyUtility.Convert.GetString(dr["OrderID"]).Substring(0, MyUtility.Convert.GetString(dr["OrderID"]).Length - 1);
                    objArray[0, 7] = dr["CargoReadyDate"];
                    objArray[0, 8] = dr["BuyerDelivery"];
                    objArray[0, 9] = dr["SDPDate"];
                    objArray[0, 10] = dr["PulloutDate"];
                    objArray[0, 11] = dr["ShipQty"];
                    objArray[0, 12] = dr["CTNQty"];
                    objArray[0, 13] = dr["GW"];
                    objArray[0, 14] = dr["CBM"];
                    objArray[0, 15] = dr["CustCDID"];
                    objArray[0, 16] = dr["Dest"];
                    objArray[0, 17] = dr["ConfirmDate"];
                    objArray[0, 18] = dr["AddName"];
                    objArray[0, 19] = dr["AddDate"];
                    objArray[0, 20] = dr["Remark"];
                    worksheet.Range[String.Format("A{0}:U{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            MyUtility.Msg.WaitClear();
            excel.Visible = true;
            return true;
        }
    }
}
