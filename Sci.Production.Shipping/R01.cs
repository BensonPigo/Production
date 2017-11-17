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
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        private string shipper;
        private string brand;
        private string shipmode;
        private string shipterm;
        private string dest;
        private string status;
        private string reportType;
        private DateTime? invdate1;
        private DateTime? invdate2;
        private DateTime? etd1;
        private DateTime? etd2;
        private DataTable printData;

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboShipper, 1, factory);
            this.comboShipper.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboStatus, 1, 1, "All,Confirmed,UnConfirmed");
            this.comboStatus.SelectedIndex = 0;
            this.txtshipmodeShippingMode.SelectedIndex = -1;
            this.radioMainList.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // if (MyUtility.Check.Empty(dateRange1.Value1))
            // {
            //    MyUtility.Msg.WarningBox("Invoice Date can't empty!!");
            //    return false;
            // }
            this.shipper = this.comboShipper.Text;
            this.brand = this.txtbrand.Text;
            this.shipmode = this.txtshipmodeShippingMode.Text;
            this.shipterm = this.txtshiptermShipmentTerm.Text;
            this.dest = this.txtcountryDestination.TextBox1.Text;
            this.status = this.comboStatus.Text;
            this.reportType = this.radioMainList.Checked ? "1" : "2";
            this.invdate1 = this.dateInvoiceDate.Value1;
            this.invdate2 = this.dateInvoiceDate.Value2;
            this.etd1 = this.dateETD.Value1;
            this.etd2 = this.dateETD.Value2;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            if (this.reportType == "1")
            {
                sqlCmd.Append(string.Format(@"select g.ID,g.Shipper,g.BrandID,g.InvDate,g.FCRDate,g.CustCDID,(g.Dest+' - '+isnull(c.Alias,'')) as Dest,g.ShipModeID,g.ShipTermID,
g.Handle,(g.Forwarder+' - '+isnull(ls.Abb,'')) as Forwarder,g.Vessel,g.ETD,g.ETA,g.SONo,g.SOCFMDate,g.TotalShipQty,g.TotalCTNQty,
isnull((select CTNRNo+'/'+TruckNo+',' from GMTBooking_CTNR WITH (NOLOCK) where ID = g.ID for xml path('')),'') as CTNTruck,
g.TotalGW,g.TotalCBM,g.AddDate,IIF(g.Status = 'Confirmed',g.EditDate,null) as ConfirmDate,g.Remark
from GMTBooking g WITH (NOLOCK) 
left join Country c WITH (NOLOCK) on c.ID = g.Dest
left join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
where 1=1"));
            }
            else
            {
                sqlCmd.Append(string.Format(@"select g.ID,g.Shipper,g.BrandID,g.InvDate,pl.MDivisionID,isnull(pl.ID,'') as PackID,pl.CargoReadyDate,pl.PulloutDate,
isnull(pl.ShipQty,0) as ShipQty,isnull(pl.CTNQty,0) as CTNQty,isnull(pl.GW,0) as GW,isnull(pl.CBM,0) as CBM,g.CustCDID,
(g.Dest+' - '+isnull(c.Alias,'')) as Dest,IIF(g.Status = 'Confirmed',g.EditDate,null) as ConfirmDate,
g.AddName+' '+isnull(p.Name,'') as AddName,g.AddDate,g.Remark,
isnull((select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a for xml path('')),'') as OrderID,
(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a, Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as BuyerDelivery,
(select oq.SDPDate from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a, Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as SDPDate
from GMTBooking g WITH (NOLOCK) 
left join PackingList pl WITH (NOLOCK) on pl.INVNo = g.ID
left join Country c WITH (NOLOCK) on c.ID = g.Dest
left join Pass1 p WITH (NOLOCK) on p.ID = g.AddName
where pl.ID<>'' and 1=1 "));
            }

            if (!MyUtility.Check.Empty(this.invdate1))
            {
                sqlCmd.Append(string.Format(" and g.InvDate >= '{0}' ", Convert.ToDateTime(this.invdate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.invdate2))
            {
                sqlCmd.Append(string.Format(" and g.InvDate <= '{0}' ", Convert.ToDateTime(this.invdate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.shipper))
            {
                sqlCmd.Append(string.Format(" and g.Shipper = '{0}'", this.shipper));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.shipmode))
            {
                sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipmode));
            }

            if (!MyUtility.Check.Empty(this.shipterm))
            {
                sqlCmd.Append(string.Format(" and g.ShipTermID = '{0}'", this.shipterm));
            }

            if (!MyUtility.Check.Empty(this.dest))
            {
                sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.dest));
            }

            if (!MyUtility.Check.Empty(this.etd1))
            {
                sqlCmd.Append(string.Format(" and g.ETD >= '{0}' ", Convert.ToDateTime(this.etd1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.etd2))
            {
                sqlCmd.Append(string.Format(" and g.ETD <= '{0}' ", Convert.ToDateTime(this.etd2).ToString("d")));
            }

            if (this.status == "Confirmed")
            {
                sqlCmd.Append(" and g.Status = 'Confirmed'");
            }
            else if (this.status == "UnConfirmed")
            {
                sqlCmd.Append(" and g.Status <> 'Confirmed'");
            }

            sqlCmd.Append(" order by g.ID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + (this.reportType == "1" ? "\\Shipping_R01_MainList.xltx" : "\\Shipping_R01_DetailList.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            if (this.reportType == "1")
            {
                int intRowsStart = 3;
                object[,] objArray = new object[1, 24];
                foreach (DataRow dr in this.printData.Rows)
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
                    worksheet.Range[string.Format("A{0}:X{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            else
            {
                int intRowsStart = 3;
                object[,] objArray = new object[1, 21];
                foreach (DataRow dr in this.printData.Rows)
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
                    worksheet.Range[string.Format("A{0}:U{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(this.reportType == "1" ? "Shipping_R01_MainList" : "Shipping_R01_DetailList");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
