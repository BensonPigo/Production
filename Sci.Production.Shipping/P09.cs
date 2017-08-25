﻿using System;
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
    public partial class P09 : Sci.Win.Tems.QueryForm
    {
        DataTable MDivision;

        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DualResult result;
            #region Combox
            if (result = DBProxy.Current.Select(null, "select distinct MDivisionID from Factory WITH (NOLOCK) where Junk = 0 ", out MDivision))
            {
                MyUtility.Tool.SetupCombox(comboM, 1, MDivision);
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query Combox fail\r\n"+result.ToString());
            }
            comboM.SelectedIndex = -1;

            MyUtility.Tool.SetupCombox(comboDataType, 1, 1, "Shipped,Not Ship,All");
            comboDataType.SelectedIndex = 0;
            #endregion 

            gridDelayShipmentOrderDetail.DataSource = listControlBindingSource1;
            gridDelayShipmentOrderDetail.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.gridDelayShipmentOrderDetail)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13))
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(2))
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(3))
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(15))
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(15))
                .Text("CustCDID", header: "Cust#", width: Widths.AnsiChars(15))
                .Text("Alias", header: "Dest", width: Widths.AnsiChars(13))
                .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(5))
                .Date("BuyerDelivery", header: "Buyer Delivery")
                .Date("PulloutDate", header: "Extended")
                .Text("OSReason", header: "Outstanding Reason", width: Widths.AnsiChars(20))
                .Text("OSRemark", header: "Outstanding Remark", width: Widths.AnsiChars(20));
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //if (MyUtility.Check.Empty(dateRange1.Value1))
            //{
            //    MyUtility.Msg.WarningBox("< Buyer Delivery > can not empty!");
            //    dateRange1.TextBox1.Focus();
            //    return;
            //}
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"with tempData 
as 
(select o.ID,oq.Seq,o.StyleID,o.BrandID,o.FactoryID,o.CustPONo,o.Customize1,o.CustCDID,
 c.Alias,oq.Qty,oq.BuyerDelivery,oq.EstPulloutDate,
 (select max(PulloutDate) from Pullout_Detail WITH (NOLOCK) where OrderID = o.ID and OrderShipmodeSeq = oq.Seq and ShipQty > 0) as PulloutDate,
 oq.OutstandingReason+' - '+isnull(r.Name,'') as OSReason,o.OutstandingRemark
 from Orders o WITH (NOLOCK) 
 left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID 
 left join Country c WITH (NOLOCK) on c.ID = o.Dest 
 left join Reason r WITH (NOLOCK) on r.ReasonTypeID = 'Delivery_OutStand' and r.ID = oq.OutstandingReason 
 where o.IsForecast = 0 
 and o.LocalOrder = 0 
 and 1=1"));

            if (!MyUtility.Check.Empty(dateBuyerDelivery.Value1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}' ", Convert.ToDateTime(dateBuyerDelivery.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateBuyerDelivery.Value2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}' ", Convert.ToDateTime(dateBuyerDelivery.Value2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(txtbrand.Text))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'",txtbrand.Text));
            }

            if (!MyUtility.Check.Empty(comboM.SelectedValue))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'",MyUtility.Convert.GetString(comboM.SelectedValue).Trim()));
            }
            sqlCmd.Append(@") 
select ID,Seq,StyleID,BrandID,FactoryID,CustPONo,Customize1,CustCDID,Alias,Qty,
BuyerDelivery,iif(PulloutDate is null,EstPulloutDate,PulloutDate) as PulloutDate,
OSReason,OutstandingRemark as OSRemark  
from tempData 
where iif(PulloutDate is null,EstPulloutDate,PulloutDate) > BuyerDelivery
and iif(PulloutDate is null,EstPulloutDate,PulloutDate) is not null ");
            if (comboDataType.SelectedIndex != -1)
            {
                if (MyUtility.Convert.GetString(comboDataType.SelectedValue) == "Shipped")
                {
                    sqlCmd.Append("and PulloutDate is not null");
                }
                else
                {
                    if (MyUtility.Convert.GetString(comboDataType.SelectedValue) == "Not Ship")
                    {
                        sqlCmd.Append("and PulloutDate is null");
                    }
                }
            }
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable GridData = (DataTable)listControlBindingSource1.DataSource;
            if (GridData == null)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }
            int dataRowCount = GridData.Rows.Count;
            if (dataRowCount <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P09.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[2, 1] = "Buyer Delivery: " + Convert.ToDateTime(dateBuyerDelivery.Value1).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat)) + " ~ " + Convert.ToDateTime(dateBuyerDelivery.Value2).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat));
            worksheet.Cells[2, 6] = "Brand: " + (MyUtility.Check.Empty(txtbrand.Text) ? "" : txtbrand.Text);
            worksheet.Cells[2, 9] = "M: " + (MyUtility.Check.Empty(comboM.SelectedValue) ? "" : comboM.SelectedValue.ToString());
            worksheet.Cells[2, 12] = "Data Type: " + (MyUtility.Check.Empty(comboDataType.SelectedValue) ? "" : comboDataType.SelectedValue.ToString());

            int intRowsStart = 4;
            object[,] objArray = new object[1, 14];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = GridData.Rows[i];
                int rownum = intRowsStart + i;
                objArray[0, 0] = dr["ID"];
                objArray[0, 1] = dr["Seq"];
                objArray[0, 2] = dr["StyleID"];
                objArray[0, 3] = dr["BrandID"];
                objArray[0, 4] = dr["FactoryID"];
                objArray[0, 5] = dr["CustPONo"];
                objArray[0, 6] = dr["Customize1"];
                objArray[0, 7] = dr["CustCDID"];
                objArray[0, 8] = dr["Alias"];
                objArray[0, 9] = dr["Qty"];
                objArray[0, 10] = dr["BuyerDelivery"];
                objArray[0, 11] = dr["PulloutDate"];
                objArray[0, 12] = dr["OSReason"];
                objArray[0, 13] = dr["OSRemark"];
                worksheet.Range[String.Format("A{0}:N{0}", rownum)].Value2 = objArray;
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P09");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
        }

        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
