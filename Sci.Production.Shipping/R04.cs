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

namespace Sci.Production.Shipping
{
    public partial class R04 : Sci.Win.Tems.PrintForm
    {
        DateTime? buyerDlv1, buyerDlv2, estPullout1, estPullout2;
        string brand, mDivision, orderNo, factory, category;
        bool includeLO;
        DataTable printData;
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            MyUtility.Tool.SetupCombox(comboCategory, 1, 1, "Bulk+Sample,Bulk,Sample");
            comboM.Text = Sci.Env.User.Keyword;
            comboFactory.SelectedIndex = -1;
            comboCategory.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            //if (MyUtility.Check.Empty(dateRange1.Value1))
            //{
            //    MyUtility.Msg.WarningBox("Buyer Delivery can't empty!!");
            //    return false;
            //}

            mDivision = comboM.Text;
            buyerDlv1 = dateBuyerDelivery.Value1;
            buyerDlv2 = dateBuyerDelivery.Value2;
            estPullout1 = dateEstimatePullout.Value1;
            estPullout2 = dateEstimatePullout.Value2;
            brand = txtbrand.Text;
            factory = comboFactory.Text;
            category = comboCategory.Text;
            orderNo = txtOrderNo.Text;
            includeLO = checkIncludeLocalOrder.Checked;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
select 	oq.BuyerDelivery
		,oq.EstPulloutDate
		,o.BrandID
		,b.BuyerID
		,o.ID
		,Category = IIF(o.Category = 'B', 'Bulk'
										, 'Sample')
		,o.CustPONo
		,o.StyleID
		,o.SeasonID
		,oq.Qty
		,o.MDivisionID
		,o.FactoryID
		,Alias = isnull(c.Alias,'')
		,o.PoPrice
		,o.Customize1
		,o.Customize2
		,oq.ShipmodeID
		,SMP = IIF(o.ScanAndPack = 1,'Y','')
		,VasShas = IIF(o.VasShas = 1,'Y','') 
		,ShipQty = (select isnull(sum(ShipQty), 0) 
					from Pullout_Detail WITH (NOLOCK) 
					where OrderID = o.ID and OrderShipmodeSeq = oq.Seq) - [dbo].getInvAdjQty(o.ID,oq.Seq) 
		,Payment = isnull((select Term 
						   from PayTermAR WITH (NOLOCK) 
						   where ID = o.PayTermARID), '')
		,Handle = o.MRHandle+' - '+isnull((select Name + ' #' + ExtNo 
										   from TPEPass1 WITH (NOLOCK) 
										   where ID = o.MRHandle), '') 
		,SMR = o.SMR+' - '+isnull((select Name + ' #' + ExtNo 
								   from TPEPass1 WITH (NOLOCK) 
								   where ID = o.SMR), '')
		,LocalMR = o.LocalMR+' - '+isnull((select Name + ' #' + ExtNo 
										   from Pass1 WITH (NOLOCK) 
										   where ID = o.LocalMR), '')
		,OSReason = oq.OutstandingReason + ' - ' + isnull((select Name 
														   from Reason WITH (NOLOCK) 
														   where ReasonTypeID = 'Delivery_OutStand' and Id = oq.OutstandingReason), '') 
		,oq.OutstandingRemark
from Orders o WITH (NOLOCK) 
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
left join Country c WITH (NOLOCK) on o.Dest = c.ID
left join Brand b WITH (NOLOCK) on o.BrandID=b.id
where 1=1 and o.PulloutComplete=0 and o.Qty > 0"));

            if (!MyUtility.Check.Empty(buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}' ", Convert.ToDateTime(buyerDlv1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}' ", Convert.ToDateTime(buyerDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(estPullout1))
            {
                sqlCmd.Append(string.Format(" and oq.EstPulloutDate between '{0}' and '{1}'", Convert.ToDateTime(estPullout1).ToString("d"), Convert.ToDateTime(estPullout2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", brand));
            }
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", factory));
            }
            if (!MyUtility.Check.Empty(orderNo))
            {
                sqlCmd.Append(string.Format(" and o.Customize1 = '{0}'", orderNo));
            }
            if (category == "Bulk")
            {
                sqlCmd.Append(" and o.Category = 'B'");
            }
            else if (category == "Sample")
            {
                sqlCmd.Append(" and o.Category = 'S'");
            }
            else
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S')");
            }
            if (!includeLO)
            {
                sqlCmd.Append(" and o.LocalOrder = 0");
            }

            sqlCmd.Append(" order by oq.BuyerDelivery,o.ID");

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

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R04_EstimateOutstandingShipmentReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 26];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["BuyerDelivery"];
                objArray[0, 1] = dr["EstPulloutDate"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["BuyerID"];
                objArray[0, 4] = dr["ID"];
                objArray[0, 5] = dr["Category"];
                objArray[0, 6] = dr["CustPONo"];
                objArray[0, 7] = dr["StyleID"];
                objArray[0, 8] = dr["SeasonID"];
                objArray[0, 9] = dr["Qty"];
                objArray[0, 10] = dr["ShipQty"];
                objArray[0, 11] = dr["MDivisionID"];
                objArray[0, 12] = dr["FactoryID"];
                objArray[0, 13] = dr["Alias"];
                objArray[0, 14] = dr["Payment"];
                objArray[0, 15] = dr["PoPrice"];
                objArray[0, 16] = dr["Customize1"];
                objArray[0, 17] = dr["Customize2"];
                objArray[0, 18] = dr["ShipmodeID"];
                objArray[0, 19] = dr["SMP"];
                objArray[0, 20] = dr["VasShas"];
                objArray[0, 21] = dr["Handle"];
                objArray[0, 22] = dr["SMR"];
                objArray[0, 23] = dr["LocalMR"];
                objArray[0, 24] = dr["OSReason"];
                objArray[0, 25] = dr["OutstandingRemark"];
                worksheet.Range[String.Format("A{0}:Y{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }
    }
}
