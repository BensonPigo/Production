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
    public partial class R14 : Sci.Win.Tems.PrintForm
    {
        string artworktype, factory, style, mdivision, spno1, spno2, ordertype,ratetype;
        int ordertypeindex,statusindex;
        DateTime? Issuedate1, Issuedate2, GLdate1,GLdate2;
        DataTable printData;

        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            txtMdivisionM.Text = Sci.Env.User.Keyword;
            txtdropdownlistOrderType.SelectedIndex = 0;
            //MyUtility.Tool.SetupCombox(txtFinanceEnReason1, 2, 1, "FX,Fixed Exchange Rate,KP,KPI Exchange Rate,DL,Daily Exchange Rate,3S,Custom Exchange Rate,RV,Currency Revaluation Rate,OT,One-time Exchange Rate");
            if (txtFinanceEnReasonRateType.Items.Count > 0)
            { txtFinanceEnReasonRateType.SelectedIndex = 0; }

            MyUtility.Tool.SetupCombox(comboStatus, 1, 1, "Only Approved,Only Unapproved,All");
            comboStatus.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {

            if (comboStatus.SelectedIndex != 1 && MyUtility.Check.Empty(dateAPDate.Value1) && MyUtility.Check.Empty(dateAPDate.Value2))
            {
                MyUtility.Msg.WarningBox("AP Date can't empty!!");
                return false;
            }
            Issuedate1 = dateAPDate.Value1;
            Issuedate2 = dateAPDate.Value2;
            GLdate1 = dateGLDate.Value1;
            GLdate2 = dateGLDate.Value2;
            spno1 = txtSpnoStart.Text;
            spno2 = txtSpnoEnd.Text;

            artworktype = txtartworktype_ftyArtworkType.Text;
            mdivision = txtMdivisionM.Text;
            factory = comboFactory.Text;
            ordertypeindex = txtdropdownlistOrderType.SelectedIndex;
            ratetype = txtFinanceEnReasonRateType.SelectedValue.ToString();
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
            
            style = txtstyle.Text;
            
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sqlparameter delcare --
            //System.Data.SqlClient.SqlParameter sp_apdate1 = new System.Data.SqlClient.SqlParameter();
            //sp_apdate1.ParameterName = "@apdate1";

            //System.Data.SqlClient.SqlParameter sp_apdate2 = new System.Data.SqlClient.SqlParameter();
            //sp_apdate2.ParameterName = "@apdate2";

            System.Data.SqlClient.SqlParameter sp_gldate1 = new System.Data.SqlClient.SqlParameter();
            sp_gldate1.ParameterName = "@gldate1";

            System.Data.SqlClient.SqlParameter sp_gldate2 = new System.Data.SqlClient.SqlParameter();
            sp_gldate2.ParameterName = "@gldate2";

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
            sqlCmd.Append(@"select t.*,ArtworkType.ID artworktypeid
                            into #cte
                            from dbo.ArtworkType WITH (NOLOCK) ,(select distinct (select orders.poid from orders WITH (NOLOCK) where id=b.OrderId) orderid 
                            from dbo.ArtworkAP a WITH (NOLOCK) inner join dbo.ArtworkAP_Detail b WITH (NOLOCK) on b.id = a.id ");

            #region -- 條件組合 --
            switch (statusindex)
            {
                case 0:  //Only Approve
                    if (!MyUtility.Check.Empty(Issuedate1) && !MyUtility.Check.Empty(Issuedate2))
                    {
                        sqlCmd.Append(string.Format(@" where a.apvdate is not null and a.issuedate between '{0}' and '{1}'"
                            , Convert.ToDateTime(Issuedate1).ToString("d"), Convert.ToDateTime(Issuedate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(Issuedate1))
                        {
                            sqlCmd.Append(string.Format(@" where a.apvdate is not null and a.issuedate >= '{0}' ", Convert.ToDateTime(Issuedate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(Issuedate2))
                        {
                            sqlCmd.Append(string.Format(@" where a.apvdate is not null and  a.issuedate <= '{0}' ", Convert.ToDateTime(Issuedate2).ToString("d")));
                        }
                    }
                    break;

                case 1:  //Only Unapprove
                    sqlCmd.Append(@" where a.apvdate is null");
                    break;

                case 2:  //All
                    if (!MyUtility.Check.Empty(Issuedate1) && !MyUtility.Check.Empty(Issuedate2))
                    {
                        sqlCmd.Append(string.Format(@" where (a.issuedate between '{0}' and '{1}')"
                            , Convert.ToDateTime(Issuedate1).ToString("d"), Convert.ToDateTime(Issuedate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(Issuedate1))
                        {
                            sqlCmd.Append(string.Format(@" where (a.issuedate >= '{0}') ", Convert.ToDateTime(Issuedate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(Issuedate2))
                        {
                            sqlCmd.Append(string.Format(@" where (a.issuedate <= '{0}') ", Convert.ToDateTime(Issuedate2).ToString("d")));
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
                sqlCmd.Append(" and a.VoucherDate >= @GLdate1");
                sp_gldate1.Value = GLdate1;
                cmds.Add(sp_gldate1);
            }
            if (!MyUtility.Check.Empty(GLdate2))
            {
                sqlCmd.Append(" and a.VoucherDate <= @GLdate2");
                sp_gldate2.Value = GLdate2;
                cmds.Add(sp_gldate2);
            }


            if (!MyUtility.Check.Empty(artworktype))
            {
                sqlCmd.Append(" and a.artworktypeid = @artworktype");
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

            sqlCmd.Append(@")t
	where Artworktype.IsSubprocess=1 ");

            // 指定繡花條件時，有多撈取繡花線的成本
            if (artworktype.ToLower().TrimEnd() == "embroidery")
            {
                if (!MyUtility.Check.Empty(artworktype))
                {
                    sqlCmd.Append(" and artworktype.id = @artworktype");
                }

                sqlCmd.Append(string.Format(@"
                select aa.FactoryID
                ,#cte.artworktypeid
                ,aa.POID
                ,aa.StyleID
                ,cc.BuyerID
                ,aa.BrandID
                ,dbo.getTPEPass1(aa.SMR) smr
                ,xx.Stitch
                ,y.order_qty
                ,x.ap_qty
                ,round(isnull(x.ap_amt,0.0)+isnull(z.localap_amt,0.0),2) amount
                ,round(isnull(x.ap_amt,0.0) / iif(y.order_qty=0,1,y.order_qty),3) + round(z.localap_amt / iif(y.order_qty=0,1,y.order_qty),3) ttl_price --P3=P1+P2
                ,round(y.order_amt/iif(y.order_qty=0,1,y.order_qty),3) std_price
                ,[percentage]=convert(varchar,round((round((isnull(x.ap_amt,0.0)+isnull(z.localap_amt,0.0)) / iif(y.order_qty=0,1,y.order_qty),3))/(round(y.order_amt/iif(y.order_qty=0,1,y.order_qty),3))*100,0))+'%'
                ,round(x.ap_amt,2)
                ,round(isnull(x.ap_amt,0.0) / iif(y.order_qty=0,1,y.order_qty),3) ap_price  --P1
                ,round(isnull(x.ap_amt,0.0) / iif(y.order_amt=0,1,y.order_amt),2) ap_percentage
                ,round(z.localap_amt,2) localap_amt
                ,round(z.localap_amt / iif(y.order_qty=0,1,y.order_qty),3) localap_price  --P2
                ,round(z.localap_amt / iif(y.order_amt=0,1,y.order_amt),2) local_percentage
                from #cte
                left join orders aa on aa.id = #cte.orderid
                left join Order_TmsCost bb on bb.id = aa.ID and bb.ArtworkTypeID = #cte.artworktypeid
                left join Brand cc on aa.BrandID=cc.id
                outer apply (
	                select isnull(sum(t.ap_amt),0.00) ap_amt
                , isnull(sum(t.ap_qty),0) ap_qty 
	                from (
	                select ap.currencyid,
			                apd.Price,
			                apd.apQty ap_qty
			                ,apd.apQty*apd.Price*dbo.getRate('{0}',ap.CurrencyID,'USD',ap.issuedate) ap_amt
			                ,dbo.getRate('{0}',ap.CurrencyID,'USD',ap.issuedate) rate
	                from ArtworkAP ap WITH (NOLOCK) 
                    inner join ArtworkAP_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
                    inner join orders WITH (NOLOCK) ON orders.id = apd.orderid
		                where ap.ArtworkTypeID = #cte.artworktypeid and orders.POId = aa.POID AND ap.Status = 'Approved') t
		                ) x
                outer apply(
	                select orders.POID
	                ,sum(orders.qty) order_qty
	                ,sum(orders.qty*Price) order_amt 
	                from orders WITH (NOLOCK) 
	                inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	                where poid= aa.POID and ArtworkTypeID= #cte.artworktypeid
	                group by orders.poid,ArtworkTypeID) y
                outer apply (
	                select isnull(sum(tt.localap_amt),0.00) localap_amt,isnull(sum(tt.localap_qty),0) localap_qty 
	                from (
	                            select ap.currencyid,
			                            apd.Price,
			                            apd.Qty localap_qty
			                            ,apd.Qty*apd.Price*dbo.getRate('{0}',ap.CurrencyID,'USD',ap.issuedate) localap_amt
			                            ,dbo.getRate('{0}',ap.CurrencyID,'USD',ap.issuedate) rate
	                            from localap ap WITH (NOLOCK) 
                                inner join Localap_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
                                inner join orders WITH (NOLOCK) ON orders.id = apd.orderid
		                            where ap.Category = 'EMB_THREAD' and orders.POId = aa.POID AND ap.Status = 'Approved'
                            ) tt
		                ) z
                outer apply(
                     select top 1 OART.qty as Stitch
                            from orders O WITH (NOLOCK) 
                            left join Order_article OA on OA.id=O.ID
                            left join Order_Artwork OART on OART.id=O.ID and OART.article=OA.article
                            WHERE O.ID = aa.POID and  artworktypeid in ('EMBROIDERY','PRINTING')
                        ) xx
                where ap_qty > 0
                ", ratetype,ordertype));

            }
            else
            {
                if (!MyUtility.Check.Empty(artworktype))
                {
                    sqlCmd.Append(" and artworktype.id = @artworktype");
                }

                sqlCmd.Append(string.Format(@"
                select aa.FactoryID
                ,#cte.artworktypeid
                ,aa.POID
                ,aa.StyleID
                ,cc.BuyerID
                ,aa.BrandID
                ,dbo.getTPEPass1(aa.SMR) smr
                ,xx.Stitch
                ,y.order_qty
                ,x.ap_qty
                ,round(x.ap_amt,2) ap_amt
                ,round(x.ap_amt / iif(y.order_qty=0,1,y.order_qty),3) ap_price
                ,round(y.order_amt/iif(y.order_qty=0,1,y.order_qty),3) std_price
                ,round(x.ap_amt / iif(y.order_qty=0,1,y.order_qty) / iif(y.order_amt=0 or y.order_qty = 0,1,(y.order_amt/y.order_qty)),2) percentage
                from #cte
                left join orders aa WITH (NOLOCK) on aa.id = #cte.orderid
                left join Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.ID and bb.ArtworkTypeID = #cte.artworktypeid
                left join Brand cc on aa.BrandID=cc.id
                outer apply (
	                select isnull(sum(t.ap_amt),0.00) ap_amt
                , isnull(sum(t.ap_qty),0) ap_qty from (
	                select ap.currencyid,
			                apd.Price,
			                apd.apQty ap_qty
			                ,apd.apQty * apd.Price * dbo.getRate('{0}',ap.CurrencyID,'USD',ap.issuedate) ap_amt
			                ,dbo.getRate('{0}',ap.CurrencyID,'USD',ap.issuedate) rate
	                from ArtworkAP ap WITH (NOLOCK) 
                    inner join ArtworkAP_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
                    inner join orders WITH (NOLOCK) on orders.id = apd.orderid
		                where ap.ArtworkTypeID = #cte.artworktypeid and orders.POId = aa.POID AND ap.Status = 'Approved') t
		                ) x		
                outer apply(
	                select orders.POID
	                ,sum(orders.qty) order_qty
	                ,sum(orders.qty*Price) order_amt 
	                from orders WITH (NOLOCK) 
	                inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	                where poid= aa.POID and ArtworkTypeID= #cte.artworktypeid
	                group by orders.poid,ArtworkTypeID) y
                outer apply(
                     select top 1 OART.qty as Stitch
                            from orders O WITH (NOLOCK) 
                            left join Order_article OA on OA.id=O.ID
                            left join Order_Artwork OART on OART.id=O.ID and OART.article=OA.article
                            WHERE O.ID = aa.POID and  artworktypeid in ('EMBROIDERY','PRINTING')
                        ) xx
                where ap_qty > 0
                ", ratetype,ordertype));
            }
            #endregion

            #region -- sqlCmd 條件組合 --
            switch (statusindex)
            {
                case 0:  //Only Approve
                    break;

                case 1:  //Only Unapprove
                    sqlCmd.Replace("AND ap.Status = 'Approved'", "AND ap.Status = 'New'");
                    break;

                case 2:  //All
                    sqlCmd.Replace("AND ap.Status = 'Approved'", " ");
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
                sqlCmd.Append(" and styleid = @style");
                sp_style.Value = style;
                cmds.Add(sp_style);
            }

            //ORDER BY
            sqlCmd.Append(" ORDER BY aa.FactoryID, #cte.artworktypeid, aa.POID ");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(),cmds, out printData);
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

            if (artworktype.ToLower().TrimEnd() == "embroidery")
            {
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R14_Embroidery.xltx", 5);
            }
            else
            {
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R14.xltx", 4);
            }
            return true;

        }

        private void comboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateAPDate.Enabled = !(comboStatus.SelectedIndex == 1);
        }
    }
}
