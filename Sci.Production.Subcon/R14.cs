using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class R14 : Win.Tems.PrintForm
    {
        private string artworktype;
        private string factory;
        private string style;
        private string mdivision;
        private string spno1;
        private string spno2;
        private string ordertype;
        private string ratetype;
        private int ordertypeindex;
        private int statusindex;
        private DateTime? Issuedate1;
        private DateTime? Issuedate2;
        private DateTime? GLdate1;
        private DateTime? GLdate2;
        private DataTable printData;

        /// <inheritdoc/>
        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            this.txtMdivisionM.Text = Env.User.Keyword;
            this.txtdropdownlistOrderType.SelectedIndex = 0;

            // MyUtility.Tool.SetupCombox(txtFinanceEnReason1, 2, 1, "FX,Fixed Exchange Rate,KP,KPI Exchange Rate,DL,Daily Exchange Rate,3S,Custom Exchange Rate,RV,Currency Revaluation Rate,OT,One-time Exchange Rate");
            if (this.txtFinanceEnReasonRateType.Items.Count > 0)
            {
                this.txtFinanceEnReasonRateType.SelectedIndex = 0;
            }

            MyUtility.Tool.SetupCombox(this.comboStatus, 1, 1, "Only Approved,Only Unapproved,All");
            this.comboStatus.SelectedIndex = 0;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.comboStatus.SelectedIndex != 1 && MyUtility.Check.Empty(this.dateAPDate.Value1) && MyUtility.Check.Empty(this.dateAPDate.Value2))
            {
                MyUtility.Msg.WarningBox("AP Date can't empty!!");
                return false;
            }

            this.Issuedate1 = this.dateAPDate.Value1;
            this.Issuedate2 = this.dateAPDate.Value2;
            this.GLdate1 = this.dateGLDate.Value1;
            this.GLdate2 = this.dateGLDate.Value2;
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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sqlparameter delcare --
            // System.Data.SqlClient.SqlParameter sp_apdate1 = new System.Data.SqlClient.SqlParameter();
            // sp_apdate1.ParameterName = "@apdate1";

            // System.Data.SqlClient.SqlParameter sp_apdate2 = new System.Data.SqlClient.SqlParameter();
            // sp_apdate2.ParameterName = "@apdate2";
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
            sqlCmd.Append(
                @"
                    select distinct c.POID, a.IssueDate ,a.ArtworkTypeID ,a.FactoryID,a.MDivisionID
                    into #tmp1
                    from ArtworkAP a WITH (NOLOCK) 
                    inner join ArtworkAP_Detail b WITH (NOLOCK) on b.id = a.id  
                    inner join Orders c WITH (NOLOCK) on b.OrderID = c.ID
                ");

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

                case 2: // All
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

            if (!MyUtility.Check.Empty(this.GLdate1))
            {
                sqlCmd.Append(" and a.VoucherDate >= @GLdate1");
                sp_gldate1.Value = this.GLdate1;
                cmds.Add(sp_gldate1);
            }

            if (!MyUtility.Check.Empty(this.GLdate2))
            {
                sqlCmd.Append(" and a.VoucherDate <= @GLdate2");
                sp_gldate2.Value = this.GLdate2;
                cmds.Add(sp_gldate2);
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

            sqlCmd.Append(@"

                    select  DISTINCT  POID ,ArtworkTypeID ,FactoryID ,MDivisionID
                    into #tmp
                    from #tmp1                                        
                ");

            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlCmd.Append(" where #tmp1.ArtworkTypeID = @artworktype");
            }

            // 指定繡花條件時，有多撈取繡花線的成本
            if (this.artworktype.ToLower().TrimEnd() == "embroidery")
            {
                sqlCmd.Append(string.Format(
                    @"

                select t.FactoryID
                ,t.artworktypeid
                ,aa.POID
                ,[Category] = (SELECT Category FROM Orders WHERE ID = t.POID)
                ,[Cancel] = IIF((SELECT Junk FROM Orders WHERE ID = t.POID)=1,'Y','N')
                ,aa.StyleID
				,aa.SeasonID
                ,cc.BuyerID
                ,aa.BrandID
                ,[SMR]=dbo.getTPEPass1(aa.SMR) 
                ,xx.PCS
                ,y.order_qty
                ,x.ap_qty
                ,[amount]=round(isnull(x.ap_amt,0.0)+isnull(z.localap_amt,0.0),2) 
                ,[ttl_price]= CASE WHEN  y.order_qty = 0 THEN NULL
                                   ELSE round(isnull(x.ap_amt,0.0) / y.order_qty,3) + round(z.localap_amt / y.order_qty,3)  --P3=P1+P2
                                   END
                ,[std_price]= CASE WHEN y.order_qty=0 THEN NULL
                                   ELSE round(y.order_amt/y.order_qty,3) 
                                   END               
                ,[percentage]=CASE WHEN y.order_qty=0 OR y.order_amt=0 THEN NULL 
                                   ELSE convert(varchar,
                                                        round(
                                                            (
                                                                round(
                                                                        (isnull(x.ap_amt,0.0) + isnull(z.localap_amt,0.0)) 
                                                                          / 
                                                                         y.order_qty
                                                                    ,3)) 
                                                                / 
                                                                (round( 
                                                                        y.order_amt/y.order_qty
                                                                        ,3)
                                                            )*100
                                                        ,0)

                                                  )+'%'
                                   END

                ,round(x.ap_amt,2)
                ,[ap_price]= CASE WHEN  y.order_qty=0 THEN NULL
                                   ELSE round(isnull(x.ap_amt,0.0) / y.order_qty,3)   --P1
                                   END
                ,[ap_percentage]= CASE WHEN y.order_amt=0 THEN NULL 
                                   ELSE round(isnull(x.ap_amt,0.0) / y.order_amt,2) 
                                   END
                ,[localap_amt]= round(z.localap_amt,2) 
                ,[localap_price]= CASE WHEN y.order_qty=0 THEN NULL
                                   ELSE round(z.localap_amt / y.order_qty,3)   --P2
                                   END
                ,[local_percentage]=CASE WHEN y.order_amt = 0 THEN NULL
                                   ELSE round(z.localap_amt / y.order_amt,2) 
                                   END
                ,[Responsible_Reason]=IIF(  IrregularPrice.Responsible IS NULL OR IrregularPrice.Responsible = '' or (x.ap_amt + z.localap_amt) <= y.order_amt,
                                            '',
                                            ISNULL(IrregularPrice.Responsible,'')+' - '+ ISNULL(IrregularPrice.Reason,'')
                                         ) 
                
                from #tmp as t
                left join orders aa on aa.id = t.POID
                left join Order_TmsCost bb on bb.id = aa.ID and bb.ArtworkTypeID = t.artworktypeid
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
		                where ap.ArtworkTypeID = t.artworktypeid and orders.POId = aa.POID AND ap.Status = 'Approved') t
		                ) x
                outer apply(
	                select orders.POID
	                ,sum(orders.qty) order_qty
	                ,sum(orders.qty*Price) order_amt 
	                from orders WITH (NOLOCK) 
	                inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	                where poid= aa.POID and ArtworkTypeID= t.artworktypeid
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
                     select count(1) as PCS
                            from orders O WITH (NOLOCK) 
                            left join Order_article OA on OA.id=O.ID
                            left join Order_Artwork OART on OART.id=O.ID and (OART.article=OA.article or OART.article='----')
                            WHERE O.ID =aa.POID and artworktypeid ='EMBROIDERY'
                        ) xx
				outer apply(
					SELECT  [Responsible]=d.Name ,sr.Reason , [ReasonID]=al.SubconReasonID , [IrregularPricePoid]=al.POID 
					FROM ArtworkPO_IrregularPrice al
					LEFT JOIN SubconReason sr ON al.SubconReasonID=sr.ID AND sr.Type='IP'
	                LEFT JOIN DropDownList  d ON d.type = 'Pms_PoIr_Responsible' AND d.ID=sr.Responsible
					WHERE al.POId = aa.POID AND al.ArtworkTypeId=t.ArtworkTypeId
				)IrregularPrice 
                where ap_qty > 0
                ", this.ratetype, this.ordertype));
            }
            else
            {
                sqlCmd.Append(string.Format(
                    @"
                select t.FactoryID
                ,t.artworktypeid
                ,aa.POID
                ,[Category] = (SELECT Category FROM Orders WHERE ID = t.POID)
                ,[Cancel] = IIF((SELECT Junk FROM Orders WHERE ID = t.POID)=1,'Y','N')
                ,aa.StyleID
				,aa.SeasonID
                ,cc.BuyerID
                ,aa.BrandID
                ,[SMR]= dbo.getTPEPass1(aa.SMR) 
                ,[PCS]= iif(t.artworktypeid ='EMBROIDERY',xx.PCS,yy.PCS) 
                ,y.order_qty
                ,x.ap_qty
                ,[ap_amt]= round(x.ap_amt,2) 
                ,[ap_price]= CASE WHEN y.order_qty=0 THEN NULL
                             ELSE round(x.ap_amt / y.order_qty,3) 
                             END
                ,[std_price]= CASE WHEN y.order_qty=0 THEN NULL
                              ELSE round(y.order_amt / y.order_qty,3) 
                              END
                ,[percentage]= CASE WHEN y.order_amt=0 or y.order_qty=0 THEN NULL
                               ELSE round(
                                           (x.ap_amt / y.order_qty) / (y.order_amt / y.order_qty)
                                        ,2) 
                               END
                ,[Responsible_Reason]=IIF(  IrregularPrice.Responsible IS NULL OR IrregularPrice.Responsible = '' or x.ap_amt <= y.order_amt,
                                            '',
                                            ISNULL(IrregularPrice.Responsible,'')+' - '+ ISNULL(IrregularPrice.Reason,'')
                                         ) 

                from #tmp as t
                left join orders aa on aa.id = t.POID
                left join Order_TmsCost bb on bb.id = aa.ID and bb.ArtworkTypeID = t.artworktypeid
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
		                where ap.ArtworkTypeID = t.artworktypeid and orders.POId = aa.POID AND ap.Status = 'Approved') t
		                ) x		
                outer apply(
	                select orders.POID
	                ,sum(orders.qty) order_qty
	                ,sum(orders.qty*Price) order_amt 
	                from orders WITH (NOLOCK) 
	                inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	                where poid= aa.POID and ArtworkTypeID= t.artworktypeid
	                group by orders.poid,ArtworkTypeID) y
                outer apply(
                     select count(1) as PCS
                            from orders O WITH (NOLOCK) 
                            left join Order_article OA on OA.id=O.ID
                            left join Order_Artwork OART on OART.id=O.ID and (OART.article=OA.article or OART.article='----')
                            WHERE O.ID =aa.POID and artworktypeid = 'EMBROIDERY'
                        ) xx
                outer apply(
                     select sum(OART.qty) as PCS
                            from orders O WITH (NOLOCK) 
                            left join Order_article OA on OA.id=O.ID
                            left join Order_Artwork OART on OART.id=O.ID and (OART.article=OA.article or OART.article='----')
                            WHERE O.ID =aa.POID and artworktypeid = 'PRINTING'
                        ) yy
				outer apply(
					SELECT  [Responsible]=d.Name ,sr.Reason , [ReasonID]=al.SubconReasonID , [IrregularPricePoid]=al.POID 
					FROM ArtworkPO_IrregularPrice al
					LEFT JOIN SubconReason sr ON al.SubconReasonID=sr.ID AND sr.Type='IP'
	                LEFT JOIN DropDownList  d ON d.type = 'Pms_PoIr_Responsible' AND d.ID=sr.Responsible
					WHERE al.POId = aa.POID AND al.ArtworkTypeId=t.ArtworkTypeId
				)IrregularPrice 
                where ap_qty > 0
                ", this.ratetype, this.ordertype));
            }
            #endregion

            #region -- sqlCmd 條件組合 --
            switch (this.statusindex)
            {
                case 0: // Only Approve
                    break;

                case 1: // Only Unapprove
                    sqlCmd.Replace("AND ap.Status = 'Approved'", "AND ap.Status = 'New'");
                    break;

                case 2: // All
                    sqlCmd.Replace("AND ap.Status = 'Approved'", " ");
                    break;
            }
            #endregion

            if (this.ordertypeindex >= 4)
            {
                // include Forecast
                sqlCmd.Append(string.Format(@" and (aa.category in {0} OR aa.IsForecast =1)", this.ordertype));
            }
            else
            {
                sqlCmd.Append(string.Format(@" and aa.category in {0} ", this.ordertype));
            }

            if (this.chk_IrregularPriceReason.Checked)
            {
                if (this.artworktype.ToLower().TrimEnd() == "embroidery")
                {
                    // 價格異常的資料存在，卻沒有ReasonID
                    sqlCmd.Append(string.Format(@" AND IIF(y.order_qty=0, NULL,round(y.order_amt/y.order_qty,3)) < "));
                    sqlCmd.Append(string.Format(@" IIF(y.order_qty=0 OR (x.ap_amt=0 AND z.localap_amt=0),NULL, round(isnull(x.ap_amt,0.0) / y.order_qty,3) + round(z.localap_amt / y.order_qty,3)) "));
                    sqlCmd.Append(string.Format(@"  AND (IrregularPrice.ReasonID IS NULL OR IrregularPrice.ReasonID ='')  "));
                }
                else
                {
                    // 價格異常的資料存在，卻沒有ReasonID
                    sqlCmd.Append(string.Format(@"  AND IIF (y.order_qty=0,NULL,round(y.order_amt/y.order_qty,3)) < IIF (y.order_qty=0,NULL,round(x.ap_amt /y.order_qty,3))  "));
                    sqlCmd.Append(string.Format(@"  AND (IrregularPrice.ReasonID IS NULL OR IrregularPrice.ReasonID ='')  "));
                }
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(" and styleid = @style");
                sp_style.Value = this.style;
                cmds.Add(sp_style);
            }

            // ORDER BY
            sqlCmd.Append(" ORDER BY t.FactoryID, t.artworktypeid, aa.POID ");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel

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

            if (this.artworktype.ToLower().TrimEnd() == "embroidery")
            {
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R14_Embroidery.xltx", 5);
            }
            else
            {
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R14.xltx", 4);
            }

            return true;
        }

        private void ComboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dateAPDate.Enabled = !(this.comboStatus.SelectedIndex == 1);
        }
    }
}
