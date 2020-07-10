using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class R16 : Win.Tems.PrintForm
    {
        string artworktype;
        string factory;
        string style;
        string mdivision;
        string spno1;
        string spno2;
        string ordertype;
        string ratetype;
        int ordertypeindex;
        int statusindex;
        DateTime? Issuedate1;
        DateTime? Issuedate2; // , GLdate1, GLdate2;
        DataTable printData;

        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            this.txtMdivisionM.Text = Env.User.Keyword;

            this.txtdropdownlistOrderType.SelectedIndex = 0;

            this.txtFinanceEnReasonRateType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboStatus, 1, 1, "Only Approved,Only Unapproved,All");
            this.comboStatus.SelectedIndex = 2;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (this.comboStatus.SelectedIndex != 1 && MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2))
            {
                MyUtility.Msg.WarningBox("Issue Date can't empty!!");
                return false;
            }

            this.Issuedate1 = this.dateIssueDate.Value1;
            this.Issuedate2 = this.dateIssueDate.Value2;
            this.spno1 = this.txtSpnoStart.Text;
            this.spno2 = this.txtSpnoEnd.Text;

            this.artworktype = this.txtartworktype_ftyArtworkType.Text;
            this.mdivision = this.txtMdivisionM.Text;
            this.factory = this.comboFactory.Text;
            this.ordertypeindex = this.txtdropdownlistOrderType.SelectedIndex;
            this.ratetype = this.txtFinanceEnReasonRateType.SelectedValue.ToString();
            this.statusindex = this.comboStatus.SelectedIndex;

            switch (this.ordertypeindex)
            {
                case 0:
                    this.ordertype = "('B')";
                    break;
                case 1:
                    this.ordertype = "('S')";
                    break;
                case 2:
                    this.ordertype = "('M')";
                    break;
                case 3:
                    this.ordertype = "('B','S')";
                    break;
                case 4:
                    this.ordertype = "('B','S')";
                    break;
                case 5:
                    this.ordertype = "('B','S','M')";
                    break;
            }

            this.style = this.txtstyle.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sqlparameter delcare --
            System.Data.SqlClient.SqlParameter sp_podate1 = new System.Data.SqlClient.SqlParameter();
            sp_podate1.ParameterName = "@podate1";

            System.Data.SqlClient.SqlParameter sp_podate2 = new System.Data.SqlClient.SqlParameter();
            sp_podate2.ParameterName = "@podate2";

            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            System.Data.SqlClient.SqlParameter sp_artworktype = new System.Data.SqlClient.SqlParameter();
            sp_artworktype.ParameterName = "@artworktype";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";
            #endregion

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            #region -- Sql Command --
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@";with cte
as
(
	select t.*,ArtworkType.ID artworktypeid
	from dbo.ArtworkType WITH (NOLOCK) ,(select distinct (select orders.poid from orders WITH (NOLOCK) where id=b.OrderId) orderid from dbo.artworkpo a	WITH (NOLOCK) 
	inner join dbo.ArtworkPo_Detail b WITH (NOLOCK) on b.id = a.id ");

            #region -- 條件組合 --
            switch (this.statusindex)
            {
                case 0: // Only Approve
                    if (!MyUtility.Check.Empty(this.Issuedate1) && !MyUtility.Check.Empty(this.Issuedate2))
                    {
                    sqlCmd.Append(string.Format(
                        @" where a.apvdate is not null and a.issuedate between '{0}' and '{1}'",
                        Convert.ToDateTime(this.Issuedate1).ToString("d"), Convert.ToDateTime(this.Issuedate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(this.Issuedate1))
                        {
                            sqlCmd.Append(string.Format(@" where a.apvdate is not null and a.issuedate >= '{0}' ", Convert.ToDateTime(this.Issuedate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.Issuedate2))
                        {
                            sqlCmd.Append(string.Format(@" where a.apvdate is not null and  a.issuedate <= '{0}' ", Convert.ToDateTime(this.Issuedate2).ToString("d")));
                        }
                    }

                    break;

                case 1: // Only Unapprove
                    sqlCmd.Append(@" where a.apvdate is null");
                    break;

                case 2: // ALL
                    if (!MyUtility.Check.Empty(this.Issuedate1) && !MyUtility.Check.Empty(this.Issuedate2))
                    {
                        sqlCmd.Append(string.Format(
                            @" where (a.issuedate between '{0}' and '{1}')",
                            Convert.ToDateTime(this.Issuedate1).ToString("d"), Convert.ToDateTime(this.Issuedate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(this.Issuedate1))
                        {
                            sqlCmd.Append(string.Format(@" where (a.issuedate >= '{0}') ", Convert.ToDateTime(this.Issuedate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.Issuedate2))
                        {
                            sqlCmd.Append(string.Format(@" where (a.issuedate <= '{0}') ", Convert.ToDateTime(this.Issuedate2).ToString("d")));
                        }
                    }

                    break;
            }

            if (!MyUtility.Check.Empty(this.spno1))
            {
                sqlCmd.Append(" and b.orderid >= @spno1");
                sp_spno1.Value = this.spno1;
                cmds.Add(sp_spno1);
            }

            if (!MyUtility.Check.Empty(this.spno2))
            {
                sqlCmd.Append(" and b.orderid <= @spno2");
                sp_spno2.Value = this.spno2;
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlCmd.Append(" and a.artworktypeid = @artworktype");
                sp_artworktype.Value = this.artworktype;
                cmds.Add(sp_artworktype);
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and a.factoryid = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            #endregion

            sqlCmd.Append(@")t
	where Artworktype.IsSubprocess=1 ");

            // 指定繡花條件時，有多撈取繡花線的成本
            if (this.artworktype.ToLower().TrimEnd() == "embroidery")
            {
                if (!MyUtility.Check.Empty(this.artworktype))
                {
                    sqlCmd.Append(" and artworktype.id = @artworktype");
                }

                sqlCmd.Append(string.Format(
                    @")
select aa.FactoryID
,cte.artworktypeid
,aa.POID
,aa.StyleID
,aa.BrandID
,dbo.getTPEPass1(aa.SMR) smr
,y.order_qty
,x.po_qty
,round(isnull(x.po_amt,0.0)+isnull(z.localpo_amt,0.0),2) amount
,round((isnull(x.po_amt,0.0)+isnull(z.localpo_amt,0.0)) / iif(y.order_qty=0,1,y.order_qty),3) ttl_price
,round(y.order_amt/iif(y.order_qty=0,1,y.order_qty),3) std_price
,[percentage]=convert(varchar,round((round((isnull(x.po_amt,0.0)+isnull(z.localpo_amt,0.0)) / iif(y.order_qty=0,1,y.order_qty),3))/(round(y.order_amt/iif(y.order_qty=0,1,y.order_qty),3))*100,2))+'%'
,x.po_amt
,round(isnull(x.po_amt,0.0) / iif(x.po_qty=0,1,x.po_qty),3) po_price
,round(isnull(x.po_amt,0.0) / iif(y.order_amt=0,1,y.order_amt),2) po_percentage

,round(z.localpo_amt,2) localpo_amt
,round(z.localpo_amt / iif(y.order_qty=0,1,y.order_qty),3) localpo_price
,round(z.localpo_amt / iif(y.order_amt=0,1,y.order_amt),2) local_percentage
,[Responsible_Reason]=IIF(IrregularPrice.Responsible IS NULL OR IrregularPrice.Responsible = '' ,'',ISNULL(IrregularPrice.Responsible,'')+' - '+ ISNULL(IrregularPrice.Reason,'')) 
from cte
left join orders aa WITH (NOLOCK) on aa.id = cte.orderid
left join Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.ID and bb.ArtworkTypeID = cte.artworktypeid
outer apply (
	select isnull(sum(t.po_amt),0.00) po_amt
,isnull(sum(t.po_qty),0) po_qty 
	from (
	select po.currencyid,
			pod.Price,
			pod.poQty po_qty
			,pod.poQty*pod.Price*dbo.getRate('{0}',po.CurrencyID,'USD',po.issuedate) po_amt
			,dbo.getRate('{0}',po.CurrencyID,'USD',po.issuedate) rate
	from ArtworkPo po WITH (NOLOCK) 
    inner join ArtworkPo_Detail pod WITH (NOLOCK) on pod.id = po.Id 
    inner join orders WITH (NOLOCK) ON orders.id = pod.orderid
		where po.ArtworkTypeID = cte.artworktypeid and orders.POId = aa.POID AND po.Status = 'Approved'  AND Orders.Category=aa.Category ) t
		) x
outer apply(
	select orders.POID
	,sum(orders.qty) order_qty
	,sum(orders.qty*Price) order_amt 
	from orders WITH (NOLOCK) 
	inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	where poid= aa.POID and ArtworkTypeID= cte.artworktypeid  AND Orders.Category=aa.Category
	group by orders.poid,ArtworkTypeID) y
outer apply (
	select isnull(sum(t.localpo_amt),0.00) localpo_amt,isnull(sum(t.localpo_qty),0) localpo_qty 
	from (
	select po.currencyid,
			pod.Price,
			pod.Qty localpo_qty
			,pod.Qty*pod.Price*dbo.getRate('{0}',po.CurrencyID,'USD',po.issuedate) localpo_amt
			,dbo.getRate('{0}',po.CurrencyID,'USD',po.issuedate) rate
	from localPo po WITH (NOLOCK) 
    inner join LocalPO_Detail pod WITH (NOLOCK) on pod.id = po.Id 
    inner join orders WITH (NOLOCK) ON orders.id = pod.orderid
		where po.Category = 'EMB_THREAD' and orders.POId = aa.POID AND po.Status = 'Approved') t
		) z
outer apply(
	SELECT [Responsible]=d.Name ,sr.Reason , [ReasonID]=al.SubconReasonID , [IrregularPricePoid]=al.POID 
	FROM ArtworkPO_IrregularPrice al
	LEFT JOIN SubconReason sr ON al.SubconReasonID=sr.ID AND sr.Type='IP'
	LEFT JOIN DropDownList  d ON d.type = 'Pms_PoIr_Responsible' AND d.ID=sr.Responsible
	WHERE al.POId = aa.POID AND al.ArtworkTypeId=cte.ArtworkTypeId
)IrregularPrice 
where po_qty > 0 
", this.ratetype));
            }
            else
            {
                if (!MyUtility.Check.Empty(this.artworktype))
                {
                    sqlCmd.Append(" and artworktype.id = @artworktype");
                }

                sqlCmd.Append(string.Format(
                    @")
select aa.FactoryID
,cte.artworktypeid
,aa.POID
,aa.StyleID
,aa.BrandID
,dbo.getTPEPass1(aa.SMR) smr
,y.order_qty
,x.po_qty
,x.po_amt
,round(x.po_amt / iif(y.order_qty=0,1,y.order_qty),3) po_price
--,y.order_amt
--,y.order_qty
,round(y.order_amt/iif(y.order_qty=0,1,y.order_qty),3) std_price
,round(x.po_amt / iif(y.order_qty=0,1,y.order_qty) / iif(y.order_amt=0 or y.order_qty = 0,1,(y.order_amt/y.order_qty)),2) percentage
,[Responsible_Reason]=IIF(IrregularPrice.Responsible IS NULL OR IrregularPrice.Responsible = '' ,'',ISNULL(IrregularPrice.Responsible,'')+' - '+ ISNULL(IrregularPrice.Reason,'')) 
from cte
left join orders aa WITH (NOLOCK) on aa.id = cte.orderid
left join Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.ID and bb.ArtworkTypeID = cte.artworktypeid
outer apply (
	select isnull(sum(t.po_amt),0.00) po_amt, isnull(sum(t.po_qty),0) po_qty from (
	select po.currencyid,
			pod.Price,
			pod.poQty po_qty
			,pod.poQty*pod.Price*dbo.getRate('{0}',po.CurrencyID,'USD',po.issuedate) po_amt
			,dbo.getRate('{0}',po.CurrencyID,'USD',po.issuedate) rate
	from ArtworkPo po WITH (NOLOCK) 
    inner join ArtworkPo_Detail pod WITH (NOLOCK) on pod.id = po.Id 
    inner join orders WITH (NOLOCK) on orders.id = pod.orderid
		where po.ArtworkTypeID = cte.artworktypeid and orders.POId = aa.POID AND po.Status = 'Approved'  AND Orders.Category=aa.Category) t
		) x		
outer apply(
	select orders.POID
	,sum(orders.qty) order_qty
	,sum(orders.qty*Price) order_amt 
	from orders WITH (NOLOCK) 
	inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	where poid= aa.POID and ArtworkTypeID= cte.artworktypeid  AND Orders.Category=aa.Category
	group by orders.poid,ArtworkTypeID) y
outer apply(
	SELECT [Responsible]=d.Name ,sr.Reason , [ReasonID]=al.SubconReasonID , [IrregularPricePoid]=al.POID 
	FROM ArtworkPO_IrregularPrice al
	LEFT JOIN SubconReason sr ON al.SubconReasonID=sr.ID AND sr.Type='IP'
	LEFT JOIN DropDownList  d ON d.type = 'Pms_PoIr_Responsible' AND d.ID=sr.Responsible
	WHERE al.POId = aa.POID AND al.ArtworkTypeId=cte.ArtworkTypeId
)IrregularPrice 

where po_qty > 0
", this.ratetype));
            }
            #endregion

            #region -- sqlCmd 條件組合 --
            switch (this.statusindex)
            {
                case 0: // Only Approve
                    break;

                case 1: // Only Unapprove
                    sqlCmd.Replace("AND po.Status = 'Approved'", "AND po.Status = 'New'");
                    break;

                case 2: // All
                    sqlCmd.Replace("AND po.Status = 'Approved'", " ");
                    break;
            }
            #endregion

            if (this.ordertypeindex >= 4) // include Forecast
            {
                sqlCmd.Append(string.Format(@" and (aa.category in {0} OR aa.IsForecast =1)", this.ordertype));
            }
            else
            {
                sqlCmd.Append(string.Format(@" and aa.category in {0} ", this.ordertype));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(" and aa.styleid = @style");
                sp_style.Value = this.style;
                cmds.Add(sp_style);
            }

            if (this.chk_IrregularPriceReason.Checked)
            {
                if (this.artworktype.ToLower().TrimEnd() == "embroidery")
                {
                    // 價格異常，卻沒有存在DB
                    sqlCmd.Append(string.Format(@"  AND round((isnull(x.po_amt,0.0)+isnull(z.localpo_amt,0.0)) / iif(y.order_qty=0,1,y.order_qty),3) > round(y.order_amt/iif(y.order_qty=0,1,y.order_qty),3)   "));
                    sqlCmd.Append(string.Format(@"  AND (IrregularPrice.ReasonID IS NULL OR IrregularPrice.ReasonID ='')   "));
                }
                else
                { // 價格異常，卻沒有存在DB
                    sqlCmd.Append(string.Format(@"  AND round(x.po_amt / iif(y.order_qty=0,1,y.order_qty),3) > round(y.order_amt/iif(y.order_qty=0,1,y.order_qty),3) "));
                    sqlCmd.Append(string.Format(@"  AND (IrregularPrice.ReasonID IS NULL OR IrregularPrice.ReasonID ='')   "));
                }
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (this.artworktype.ToLower().TrimEnd() == "embroidery")
            {
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R16_Embroidery.xltx", 5);
            }
            else
            {
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R16.xltx", 4);
            }

            return true;
        }

        private void comboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dateIssueDate.Enabled = !(this.comboStatus.SelectedIndex == 1);
        }
    }
}
