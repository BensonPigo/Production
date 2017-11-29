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

namespace Sci.Production.Subcon
{
    public partial class R24 : Sci.Win.Tems.PrintForm
    {
        string artworktype, factory, brandid, style, mdivision, spno1, spno2, ordertype, ratetype;
        int ordertypeindex, statusindex;
        DateTime? APdate1, APdate2, GLdate1, GLdate2;
        DataTable printData;

        public R24(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            txtMdivisionM.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(comboOrderType, 1, 1, "Bulk,Sample,Material,Bulk+Sample,Bulk+Sample+Forecast,Bulk+Sample+Material+Forecast");
            comboOrderType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(comboRateType, 2, 1, "FX,Fixed Exchange Rate,KP,KPI Exchange Rate,DL,Daily Exchange Rate,3S,Custom Exchange Rate,RV,Currency Revaluation Rate,OT,One-time Exchange Rate");
            comboRateType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(comboStatus, 1, 1, "Only Approved,Only Unapproved,All");
            comboStatus.SelectedIndex = 0;

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

            if (comboStatus.SelectedIndex != 1)
            {
                if (MyUtility.Check.Empty(dateAPDate.Value1) && MyUtility.Check.Empty(dateAPDate.Value2) && MyUtility.Check.Empty(dateGLDate.Value1) && MyUtility.Check.Empty(dateGLDate.Value2))
                {
                    MyUtility.Msg.WarningBox("[A/P Date] or [GL Date] must input one !!");
                    return false;
                }
            }

            APdate1 = dateAPDate.Value1;
            APdate2 = dateAPDate.Value2;
            spno1 = txtSPNoStart.Text;
            spno2 = txtSPNoEnd.Text;
            GLdate1 = dateGLDate.Value1;
            GLdate2 = dateGLDate.Value2;

            artworktype = txtartworktype_ftyCategory.Text;
            mdivision = txtMdivisionM.Text;
            factory = comboFactory.Text;
            ordertypeindex = comboOrderType.SelectedIndex;
            ratetype = comboRateType.SelectedValue.ToString();
            statusindex = comboStatus.SelectedIndex;
            switch (ordertypeindex)
            {
                case 0:
                    ordertype = "('B')";
                    break;
                case 1:
                    ordertype = "('S')";
                    break;
                case 2:
                    ordertype = "('M')";
                    break;
                case 3:
                    ordertype = "('B','S')";
                    break;
                case 4:
                    ordertype = "('B','S')";
                    break;
                case 5:
                    ordertype = "('B','S','M')";
                    break;
            }
            brandid = txtbrand.Text;
            style = txtstyle.Text;

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
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select distinct o.poid 
        , a.FactoryID
		, artworktypeid = a.Category
into #tmp
from dbo.localap a WITH (NOLOCK) 
inner join dbo.LocalAP_Detail b WITH (NOLOCK) on b.id = a.id  
join ArtworkType c on a.Category = c.ID 
inner join orders o WITH (NOLOCK) on o.id = b.OrderId 
where c.Classify='P' 
");

            #region -- 條件組合 --
            switch (statusindex)
            {
                case 0: //Only Approve
                    if (!MyUtility.Check.Empty(APdate1) && !MyUtility.Check.Empty(APdate2))
                    {
                        sqlCmd.Append(string.Format(@" and a.apvdate is not null and a.issuedate between '{0}' and '{1}'"
                            , Convert.ToDateTime(APdate1).ToString("d"), Convert.ToDateTime(APdate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(APdate1))
                        {
                            sqlCmd.Append(string.Format(@" and a.apvdate is not null and a.issuedate >= '{0}' ", Convert.ToDateTime(APdate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(APdate2))
                        {
                            sqlCmd.Append(string.Format(@" and a.apvdate is not null and  a.issuedate <= '{0}' ", Convert.ToDateTime(APdate2).ToString("d")));
                        }
                    }
                    break;

                case 1: // Only Unapprove
                    sqlCmd.Append(@" and a.apvdate is null");
                    break;

                case 2: // All
                    if (!MyUtility.Check.Empty(APdate1) && !MyUtility.Check.Empty(APdate2))
                    {
                        sqlCmd.Append(string.Format(@" and (a.issuedate between '{0}' and '{1}')"
                            , Convert.ToDateTime(APdate1).ToString("d"), Convert.ToDateTime(APdate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(APdate1))
                        {
                            sqlCmd.Append(string.Format(@" and (a.issuedate >= '{0}') ", Convert.ToDateTime(APdate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(APdate2))
                        {
                            sqlCmd.Append(string.Format(@" and (a.issuedate <= '{0}') ", Convert.ToDateTime(APdate2).ToString("d")));
                        }
                    }
                    break;
            }

            if (!MyUtility.Check.Empty(spno1))
            {
                sqlCmd.Append(" and b.orderid >= @spno1");
                sp_spno1.Value = spno1;
                cmds.Add(sp_spno1);
            }

            if (!MyUtility.Check.Empty(spno2))
            {
                sqlCmd.Append(" and b.orderid <= @spno2");
                sp_spno2.Value = spno2;
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(GLdate1))
            {
                sqlCmd.Append(" and a.ApvDate >= @GLdate1");
                sp_GLdate1.Value = GLdate1;
                cmds.Add(sp_GLdate1);
            }

            if (!MyUtility.Check.Empty(GLdate2))
            {
                sqlCmd.Append(" and a.ApvDate <= @GLdate2");
                sp_GLdate2.Value = GLdate2;
                cmds.Add(sp_GLdate2);
            }

            if (!MyUtility.Check.Empty(artworktype))
            {
                sqlCmd.Append(" and a.category = @artworktype");
                sp_artworktype.Value = artworktype;
                cmds.Add(sp_artworktype);
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and a.factoryid = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }

            #endregion

            if (!MyUtility.Check.Empty(artworktype))
            {
                sqlCmd.Append(" and c.id = @artworktype");
            }

            sqlCmd.Append(string.Format(@"
select distinct t.FactoryID
    ,t.artworktypeid
    ,aa.POID
    ,aa.StyleID
    ,cc.BuyerID
    ,aa.BrandID
    ,dbo.getTPEPass1(aa.SMR) smr
    ,y.order_qty
    ,x.ap_qty
    ,x.ap_amt
    ,round(x.ap_amt / iif(y.order_qty=0,1,y.order_qty),3) ap_price
    ,round(y.order_amt/iif(y.order_qty=0,1,y.order_qty),3) std_price
    ,round(x.ap_amt / iif(y.order_qty=0,1,y.order_qty)/ iif(y.order_amt=0 or y.order_qty = 0,1,(y.order_amt/y.order_qty)),2)  percentage
into #tmp2
from #tmp t
left join orders aa WITH (NOLOCK) on aa.id = t.poid
left join Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.ID and bb.ArtworkTypeID = t.artworktypeid
left join Brand cc on aa.BrandID=cc.id
outer apply (
	select isnull(sum(t.ap_amt),0.00) ap_amt
            , isnull(sum(t.ap_qty),0) ap_qty 
from (
	select --currencyid,
			--apd.Price,
			apd.Qty ap_qty
			,apd.Qty*apd.Price*dbo.getRate('{0}',AP.CurrencyID,'USD',AP.ISSUEDATE) ap_amt
			--,dbo.getRate('{0}',AP.CurrencyID,'USD',AP.ISSUEDATE) rate
	from localap ap WITH (NOLOCK) inner join LocalAP_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
    where ap.Category = t.artworktypeid and apd.OrderId = t.POID AND AP.Status = 'Approved'  and t.FactoryID = ap.FactoryId) t
) x		
outer apply(
	select --orders.POID
	isnull(sum(orders.qty),0) order_qty
	,sum(orders.qty*Price) order_amt 
	from orders WITH (NOLOCK) 
	inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	where poid= t.POID and ArtworkTypeID= t.artworktypeid
	group by orders.poid,ArtworkTypeID
) y
where ap_qty > 0 
", ratetype));
            #endregion

            #region -- sqlCmd 條件組合 --
            switch (statusindex)
            {
                case 0:  //Only Approve
                    break;

                case 1:  //Only Unapprove
                    sqlCmd.Replace("AND AP.Status = 'Approved'", "AND AP.Status = 'New'");
                    break;

                case 2:  //All
                    sqlCmd.Replace("AND AP.Status = 'Approved'", " ");
                    break;
            }
            #endregion


            if (ordertypeindex >= 4) //include Forecast 
            {
                sqlCmd.Append(string.Format(@" and (aa.category in {0} OR aa.IsForecast =1)", ordertype));
            }
            else
            {
                sqlCmd.Append(string.Format(@" and aa.category in {0} ", ordertype));
            }

            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(" and aa.styleid = @style");
                sp_style.Value = style;
                cmds.Add(sp_style);
            }

            if (!MyUtility.Check.Empty(brandid))
            {
                sqlCmd.Append(" and aa.brandid = @brandid");
                sp_brandid.Value = brandid;
                cmds.Add(sp_brandid);
            }


            sqlCmd.Append(@" 
select distinct *from #tmp2
drop table #tmp,#tmp2");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
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

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R24.xltx", 4);
            return true;
        }

        private void comboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateAPDate.Enabled = !(comboStatus.SelectedIndex == 1);
            dateGLDate.Enabled = !(comboStatus.SelectedIndex == 1);
        }
    }
}
