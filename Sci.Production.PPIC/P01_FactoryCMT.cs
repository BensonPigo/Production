using System;
using System.Collections.Generic;
using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_FactoryCMT
    /// </summary>
    public partial class P01_FactoryCMT : Win.Subs.Base
    {
        private DataRow orderData;

        /// <summary>
        /// P01_FactoryCMT
        /// </summary>
        /// <param name="orderData">DataRow OrderData</param>
        public P01_FactoryCMT(DataRow orderData)
        {
            this.InitializeComponent();
            this.orderData = orderData;
            this.Text = "Factory CMT (" + this.orderData["ID"].ToString() + ")";
            this.label3.Text = "Sub Process\r\nStd. Cost";
            this.label4.Text = "Local Purchase\r\nStd. Cost";
            this.label15.Text = "Sewing\r\nCPU";
            this.label1.Text = "Sub Process\r\nCPU";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable gridData;
            this.Init();

            string sqlCmd = string.Format(
                @"
select ot.Seq,ot.ArtworkTypeID,ot.Qty,ot.ArtworkUnit,ot.TMS,ot.Price,iif(a.IsTtlTMS = 1,'Y','N') as ttlTMS,a.Classify
from Order_TmsCost ot WITH (NOLOCK) 
left join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID
where ot.ID = '{0}' 
and (a.Classify = 'I' or a.Classify = 'A' or a.Classify = 'P')
order by ot.Seq", this.orderData["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);

            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query order tmscost data fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
            this.gridFactoryCMT.IsEditingReadOnly = true;
            this.gridFactoryCMT.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridFactoryCMT)
                .Text("Seq", header: "Seq#", width: Widths.AnsiChars(4))
                .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20))
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6))
                .Text("ArtworkUnit", header: "Unit", width: Widths.AnsiChars(10))
                .Numeric("TMS", header: "Tms", width: Widths.AnsiChars(6))
                .Numeric("Price", header: "Price", decimal_places: 3, width: Widths.AnsiChars(6))
                .Text("ttlTMS", header: "Ttl TMS", width: Widths.AnsiChars(1));

            this.numSewingCPU.Value = MyUtility.Convert.GetDecimal(this.orderData["CPU"]);
            this.CalculatedCPUcost();
            #region 取得 Sub Process Std. Cost
            string sqlGetStdCost = @"
declare @cpuCost numeric(6,3) = @inputCPUCost
declare @orderid varchar(13) = @inputOrderID
declare @subProcessAMT numeric(16,4)
declare @subProcessCPU numeric(16,4)

select @subProcessAMT = sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(@orderid,'AMT') 
select @subProcessCPU = sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(@orderid,'CPU')

select  [SubProcessCPU] = @subProcessCPU,
        [SubProcessAMT] = @subProcessAMT";

            List<SqlParameter> parGetStdCost = new List<SqlParameter>()
            {
                new SqlParameter("@inputCPUCost", this.numCPUCost.Value),
                new SqlParameter("@inputOrderID", this.orderData["ID"].ToString()),
            };

            DataRow drSubprocessCost;
            MyUtility.Check.Seek(sqlGetStdCost, parGetStdCost, out drSubprocessCost);
            #endregion
            this.numSubProcessAMT.Value = MyUtility.Convert.GetDecimal(drSubprocessCost["SubProcessAMT"]);
            this.numSubProcessCPU.Value = MyUtility.Convert.GetDecimal(drSubprocessCost["SubProcessCPU"]);
            this.CalculatedLocalCMT();
            this.numStdFtyCMP.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(((this.numSewingCPU.Value + this.numSubProcessCPU.Value) * this.numCPUCost.Value) + this.numSubProcessAMT.Value + this.numLocalPurchase.Value), 3);
        }

        private void Init()
        {
            this.numCPUCost.Value = 0;
            this.numSubProcessAMT.Value = 0;
            this.numLocalPurchase.Value = 0;
            this.numStdFtyCMP.Value = 0;
        }

        // [CPU cost]計算
        private void CalculatedCPUcost()
        {
            string sql;
            if (!MyUtility.Check.Empty(this.orderData["OrigBuyerDelivery"]))
            {
                string whereSeasonID = $" and fsd.SeasonID = '{this.orderData["SeasonID"]}'";
                sql = string.Format(
                    @"select fd.CpuCost
                                    from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
                                    where fsd.BrandID = '{0}'
                                    and fsd.FactoryID = '{1}'
                                    and '{2}' between fsd.BeginDate and fsd.EndDate
                                    and fsd.ShipperID = fd.ShipperID
                                    and '{2}' between fd.BeginDate and fd.EndDate",
                    this.orderData["BrandID"].ToString(),
                    this.orderData["FactoryID"].ToString(),
                    Convert.ToDateTime(this.orderData["OrigBuyerDelivery"]).ToString("d"));
                this.numCPUCost.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sql + whereSeasonID));
                if (MyUtility.Check.Empty(this.numCPUCost.Value))
                {
                    this.numCPUCost.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sql + " and fsd.SeasonID = '' "));
                }
            }
        }

        // [Local CMT]計算
        private void CalculatedLocalCMT()
        {
            string sql;
            bool bolLocalCMT = false;

            bolLocalCMT = MyUtility.GetValue.Lookup(string.Format("select LocalCMT from Factory WITH (NOLOCK) where ID = '{0}'", this.orderData["FactoryID"].ToString())).ToUpper() == "TRUE";
            if (bolLocalCMT)
            {
                sql = string.Format(
                    " select dbo.GetLocalPurchaseStdCost('{0}')",
                    this.orderData["ID"].ToString());

                this.numLocalPurchase.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sql));
            }
        }
    }
}
