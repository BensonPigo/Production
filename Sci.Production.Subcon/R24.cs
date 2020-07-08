using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class R24 : Sci.Win.Tems.PrintForm
    {
        string artworktype;
        string factory;
        string brandid;
        string style;
        string mdivision;
        string spno1;
        string spno2;
        string ordertype;
        string ratetype;
        int ordertypeindex, statusindex;
        DateTime? APdate1, APdate2, GLdate1, GLdate2;
        DataTable printData;

        public R24(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.txtMdivisionM.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboOrderType, 1, 1, "Bulk,Sample,Material,Bulk+Sample,Bulk+Sample+Forecast,Bulk+Sample+Material+Forecast");
            this.comboOrderType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboRateType, 2, 1, "FX,Fixed Exchange Rate,KP,KPI Exchange Rate,DL,Daily Exchange Rate,3S,Custom Exchange Rate,RV,Currency Revaluation Rate,OT,One-time Exchange Rate");
            this.comboRateType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboStatus, 1, 1, "Only Approved,Only Unapproved,All");
            this.comboStatus.SelectedIndex = 0;

            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            int year = DateTime.Today.Year;
            this.dateAPDate.Value1 = DateTime.Today.AddMonths(-1);
            this.dateAPDate.Value2 = DateTime.Now;
            this.dateGLDate.Value1 = DateTime.Today.AddMonths(-1);
            this.dateGLDate.Value2 = DateTime.Now;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (this.comboStatus.SelectedIndex != 1)
            {
                if (MyUtility.Check.Empty(this.dateAPDate.Value1) && MyUtility.Check.Empty(this.dateAPDate.Value2) && MyUtility.Check.Empty(this.dateGLDate.Value1) && MyUtility.Check.Empty(this.dateGLDate.Value2))
                {
                    MyUtility.Msg.WarningBox("[A/P Date] or [GL Date] must input one !!");
                    return false;
                }
            }

            this.APdate1 = this.dateAPDate.Value1;
            this.APdate2 = this.dateAPDate.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            this.GLdate1 = this.dateGLDate.Value1;
            this.GLdate2 = this.dateGLDate.Value2;

            this.artworktype = this.txtartworktype_ftyCategory.Text;
            this.mdivision = this.txtMdivisionM.Text;
            this.factory = this.comboFactory.Text;
            this.ordertypeindex = this.comboOrderType.SelectedIndex;
            this.ratetype = this.comboRateType.SelectedValue.ToString();
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

            this.brandid = this.txtbrand.Text;
            this.style = this.txtstyle.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sqlparameter delcare --
            System.Data.SqlClient.SqlParameter sp_apdate1 = new System.Data.SqlClient.SqlParameter();
            sp_apdate1.ParameterName = "@apdate1";

            System.Data.SqlClient.SqlParameter sp_apdate2 = new System.Data.SqlClient.SqlParameter();
            sp_apdate2.ParameterName = "@apdate2";

            System.Data.SqlClient.SqlParameter sp_GLdate1 = new System.Data.SqlClient.SqlParameter();
            sp_GLdate1.ParameterName = "@GLdate1";

            System.Data.SqlClient.SqlParameter sp_GLdate2 = new System.Data.SqlClient.SqlParameter();
            sp_GLdate2.ParameterName = "@GLdate2";

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

            System.Data.SqlClient.SqlParameter sp_brandid = new System.Data.SqlClient.SqlParameter();
            sp_brandid.ParameterName = "@brandid";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            #endregion

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            #region -- Sql Command --
            string whereIncludeCancelOrder = this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"
select distinct o.poid 
        , a.FactoryID
		, a.MDivisionID
		, artworktypeid = a.Category
into #tmp
from dbo.localap a WITH (NOLOCK) 
inner join dbo.LocalAP_Detail b WITH (NOLOCK) on b.id = a.id  
join ArtworkType c on a.Category = c.ID 
inner join orders o WITH (NOLOCK) on o.id = b.OrderId 
where c.Classify='P' {whereIncludeCancelOrder}
");

            #region -- 條件組合 --
            switch (this.statusindex)
            {
                case 0: // Only Approve
                    if (!MyUtility.Check.Empty(this.APdate1) && !MyUtility.Check.Empty(this.APdate2))
                    {
                        sqlCmd.Append(string.Format(
                            @" and a.apvdate is not null and a.issuedate between '{0}' and '{1}'",
                            Convert.ToDateTime(this.APdate1).ToString("d"), Convert.ToDateTime(this.APdate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(this.APdate1))
                        {
                            sqlCmd.Append(string.Format(@" and a.apvdate is not null and a.issuedate >= '{0}' ", Convert.ToDateTime(this.APdate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.APdate2))
                        {
                            sqlCmd.Append(string.Format(@" and a.apvdate is not null and  a.issuedate <= '{0}' ", Convert.ToDateTime(this.APdate2).ToString("d")));
                        }
                    }

                    break;

                case 1: // Only Unapprove
                    sqlCmd.Append(@" and a.apvdate is null");
                    break;

                case 2: // All
                    if (!MyUtility.Check.Empty(this.APdate1) && !MyUtility.Check.Empty(this.APdate2))
                    {
                        sqlCmd.Append(string.Format(
                            @" and (a.issuedate between '{0}' and '{1}')",
                            Convert.ToDateTime(this.APdate1).ToString("d"), Convert.ToDateTime(this.APdate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(this.APdate1))
                        {
                            sqlCmd.Append(string.Format(@" and (a.issuedate >= '{0}') ", Convert.ToDateTime(this.APdate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.APdate2))
                        {
                            sqlCmd.Append(string.Format(@" and (a.issuedate <= '{0}') ", Convert.ToDateTime(this.APdate2).ToString("d")));
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

            if (!MyUtility.Check.Empty(this.GLdate1))
            {
                sqlCmd.Append(" and a.ApvDate >= @GLdate1");
                sp_GLdate1.Value = this.GLdate1;
                cmds.Add(sp_GLdate1);
            }

            if (!MyUtility.Check.Empty(this.GLdate2))
            {
                sqlCmd.Append(" and a.ApvDate <= @GLdate2");
                sp_GLdate2.Value = this.GLdate2;
                cmds.Add(sp_GLdate2);
            }

            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlCmd.Append(" and a.category = @artworktype");
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

            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlCmd.Append(" and c.id = @artworktype");
            }

            sqlCmd.Append(string.Format(
                @"
-- #tmp_localap
select isnull(sum(a.ap_amt),0.00) ap_amt
     , isnull(sum(a.ap_qty),0) ap_qty 
	,a.Category
	,a.FactoryId
	,a.OrderId
	,isnull(sum(a.CartonQty),0) CartonQty
into  #tmp_localap
from(
	select 
			apd.Qty ap_qty
			,apd.Qty*apd.Price*dbo.getRate('{0}',AP.CurrencyID,'USD',AP.ISSUEDATE) ap_amt 
			,ap.Category
			,ap.FactoryId
			,o.POID as OrderId 
			,[CartonQty] = iif(li.IsCarton=1,apd.Qty,0)
	from localap ap WITH (NOLOCK) 
	inner join LocalAP_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
	inner join orders o with (nolock) on apd.OrderID = o.ID	
	left join LocalItem li with (nolock) on li.RefNo=apd.Refno
	where 1=1
	AND AP.Status = 'Approved'
)a 
inner join #tmp t on t.artworktypeid = a.Category and t.FactoryID = a.FactoryId and a.OrderId = t.POID 
where ap_qty > 0  
group by a.Category,a.FactoryId,a.OrderId

-- #tmp_orders
select iif(bb.ArtworkTypeID is null,null,isnull(sum(aa.qty),0)) order_qty
 	,sum(aa.qty *Price) order_amt 
	,t.poid
	,bb.ArtworkTypeID
into #tmp_orders
from orders aa WITH (NOLOCK) 
left join Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.ID 
left join (select distinct poid,artworktypeid from #tmp) t on t.poid =aa.poid and bb.ArtworkTypeID = t.artworktypeid 
group by  bb.ArtworkTypeID,t.poid

-- #tmp_final 
select distinct t.FactoryID
	,t.MDivisionID
    ,t.artworktypeid
    ,t.POID
    ,[Category] = (SELECT Category FROM Orders WHERE ID = t.POID)
    ,[Cancel] = IIF((SELECT Junk FROM Orders WHERE ID = t.POID)=1,'Y','N')
    ,aa.StyleID
    ,cc.BuyerID
    ,aa.BrandID 
    ,dbo.getTPEPass1((select SMR from orders o  WITH (NOLOCK) where o.id = aa.poid)) smr  
	,os.os
    ,x.ap_qty	
	,[CartonQty] = x.CartonQty
    ,x.ap_amt
    ,[ap_price]=IIF(totalSamePoidQty.value IS NULL OR totalSamePoidQty.value=0, NULL, round(x.ap_amt / totalSamePoidQty.value,3))
    ,[std_price]=IIF(y.order_qty  IS NULL OR y.order_qty=0  , NULL, round(y.order_amt/y.order_qty,3) )    
    ,[percentage]=IIF(y.order_qty  IS NULL OR y.order_qty=0 OR y.order_amt IS NULL OR y.order_amt =0 ,NULL, round( (x.ap_amt / y.order_qty)   /   (y.order_amt/y.order_qty),2)  )
    ,[Responsible_Reason]=IIF(IrregularPrice.Responsible IS NULL OR IrregularPrice.Responsible = '' ,'',ISNULL(IrregularPrice.Responsible,'')+' - '+ ISNULL(IrregularPrice.Reason,''))
into #tmp_final
from #tmp t
left join orders aa WITH (NOLOCK) on t.poid =aa.poid  
left join Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.ID and bb.ArtworkTypeID = t.artworktypeid
left join Brand cc WITH (NOLOCK) on aa.BrandID=cc.id  
left join #tmp_localap x on t.artworktypeid = x.Category and t.POID = x.OrderId and t.FactoryID = x.FactoryId  	 
left join #tmp_orders y on t.POID = y.poid  and t.artworktypeid = y.artworktypeid
outer apply(select os=sum(qty) from orders o with(nolock) where o.poid = aa.poid)os
outer apply(select value=sum(qty) from orders o with(nolock) where o.poid = t.poid)totalSamePoidQty
outer apply(
	SELECT [Responsible]=d.Name ,sr.Reason , [ReasonID]=al.SubconReasonID , [IrregularPricePoid]=al.POID 
	FROM LocalPO_IrregularPrice al
	LEFT JOIN SubconReason sr ON al.SubconReasonID=sr.ID AND sr.Type='IP'
    LEFT JOIN DropDownList  d ON d.type = 'Pms_PoIr_Responsible' AND d.ID=sr.Responsible
	WHERE al.POId = aa.poid  AND al.Category=t.artworktypeid
)IrregularPrice 
where 1=1 
", this.ratetype));
            #endregion

            #region -- sqlCmd 條件組合 --
            switch (this.statusindex)
            {
                case 0: // Only Approve
                    break;

                case 1: // Only Unapprove
                    sqlCmd.Replace("AND AP.Status = 'Approved'", "AND AP.Status = 'New'");
                    break;

                case 2: // All
                    sqlCmd.Replace("AND AP.Status = 'Approved'", " ");
                    break;
            }
            #endregion

            if (this.chk_IrregularPriceReason.Checked)
            {
                // 價格異常的資料存在，卻沒有ReasonID
                sqlCmd.Append(string.Format(@" AND (y.order_qty=0 or x.ap_amt > isnull (y.order_amt, 0)) "));
                sqlCmd.Append(string.Format(@" AND (IrregularPrice.ReasonID IS NULL OR IrregularPrice.ReasonID ='') "));
            }

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

            if (!MyUtility.Check.Empty(this.brandid))
            {
                sqlCmd.Append(" and aa.brandid = @brandid");
                sp_brandid.Value = this.brandid;
                cmds.Add(sp_brandid);
            }

            sqlCmd.Append(@" 
select * from #tmp_final
drop table #tmp,#tmp_orders,#tmp_localap,#tmp_final");
            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            DBProxy.Current.DefaultTimeout = 300;
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
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R24.xltx", 4);
            return true;
        }

        private void comboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dateAPDate.Enabled = !(this.comboStatus.SelectedIndex == 1);
            this.dateGLDate.Enabled = !(this.comboStatus.SelectedIndex == 1);
        }
    }
}
