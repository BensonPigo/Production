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

namespace Sci.Production.PPIC
{
    public partial class P01_FactoryCMT : Sci.Win.Subs.Base
    {
        private DataRow orderData;

        public P01_FactoryCMT(DataRow OrderData)
        {
            InitializeComponent();
            orderData = OrderData;
            this.Text = "Factory CMT (" + orderData["ID"].ToString() + ")";
            label3.Text = "Sub Process\r\nStd. Cost";
            label4.Text = "Local Purchase\r\nStd. Cost";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable GridData;
            string sqlCmd = string.Format(@"select ot.Seq,ot.ArtworkTypeID,ot.Qty,ot.ArtworkUnit,ot.TMS,ot.Price,iif(a.IsTtlTMS = 1,'Y','N') as ttlTMS,a.Classify
from Order_TmsCost ot WITH (NOLOCK) 
left join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID
where ot.ID = '{0}' 
and (a.Classify = 'I' or a.Classify = 'A' or a.Classify = 'P')
order by ot.Seq", orderData["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out GridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query order tmscost data fail!!" + result.ToString());
            }
            listControlBindingSource1.DataSource = GridData;
            this.gridFactoryCMT.IsEditingReadOnly = true;
            this.gridFactoryCMT.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridFactoryCMT)
                .Text("Seq", header: "Seq#", width: Widths.AnsiChars(4))
                .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20))
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6))
                .Text("ArtworkUnit", header: "Unit", width: Widths.AnsiChars(10))
                .Numeric("TMS", header: "Tms", width: Widths.AnsiChars(6))
                .Numeric("Price", header: "Price", decimal_places: 3, width: Widths.AnsiChars(6))
                .Text("ttlTMS", header: "Ttl TMS", width: Widths.AnsiChars(1));

            numCPU.Value = MyUtility.Convert.GetDecimal(orderData["CPU"]);

            #region [CPU cost]計算
            if (MyUtility.Check.Empty(orderData["OrigBuyerDelivery"]))
            {
                numCPUCost.Value = 0;
            }
            else
            {
                string sql = string.Format(@"select fd.CpuCost
                    from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
                    where fsd.BrandID = '{0}'
                    and fsd.FactoryID = '{1}'
                    and '{2}' between fsd.BeginDate and fsd.EndDate
                    and fsd.ShipperID = fd.ShipperID
                    and '{2}' between fd.BeginDate and fd.EndDate"
                    , orderData["BrandID"].ToString(), orderData["FactoryID"].ToString(), Convert.ToDateTime(orderData["OrigBuyerDelivery"]).ToString("d"));
                numCPUCost.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sql));
            }
            #endregion

            numSubProcess.Value = MyUtility.Convert.GetDecimal(GridData.Compute("sum(Price)", "Classify = 'I' and ttlTMS = 'N'")) + MyUtility.Convert.GetDecimal(GridData.Compute("sum(Price)", "Classify = 'A'"));
            if (MyUtility.GetValue.Lookup(string.Format("select LocalCMT from Factory WITH (NOLOCK) where ID = '{0}'", orderData["FactoryID"].ToString())).ToUpper() == "TRUE")
            {
                numLocalPurchase.Value = MyUtility.Convert.GetDecimal(GridData.Compute("sum(Price)", "Classify = 'P'"));
            }
            else
            {
                numLocalPurchase.Value = 0;
            }
            numStdFtyCMP.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal((numCPU.Value * numCPUCost.Value) + numSubProcess.Value + numLocalPurchase.Value), 2);
        }
    }
}
