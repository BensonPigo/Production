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
    public partial class R23 : Sci.Win.Tems.PrintForm
    {
        string artworktype, factory, style, mdivision, spno1, spno2, ordertype,ratetype, IrregularPrice;//,status;
        int ordertypeindex,statusindex;
        DateTime? IssueDate1, IssueDate2, SciDelivery1, SciDelivery2;
        DataTable printData;

        public R23(ToolStripMenuItem menuitem)
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
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {

            if (comboStatus.SelectedIndex != 1 && ((MyUtility.Check.Empty(dateIssueDate.Value1) && MyUtility.Check.Empty(dateIssueDate.Value2)) && (MyUtility.Check.Empty(dateSciDelivery.Value1) && MyUtility.Check.Empty(dateSciDelivery.Value2))))
            {
                MyUtility.Msg.WarningBox("[Issue Date] or [Sci Delivery] must input one condition !!");
                return false;
            }
            IssueDate1 = dateIssueDate.Value1;
            IssueDate2 = dateIssueDate.Value2;
            SciDelivery1 = dateSciDelivery.Value1;
            SciDelivery2 = dateSciDelivery.Value2;
            spno1 = txtSpnoStart.Text;
            spno2 = txtSpnoEnd.Text;
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

            style = txtstyle.Text;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sqlparameter delcare --
            System.Data.SqlClient.SqlParameter sp_Podate1 = new System.Data.SqlClient.SqlParameter();
            sp_Podate1.ParameterName = "@Podate1";

            System.Data.SqlClient.SqlParameter sp_Podate2 = new System.Data.SqlClient.SqlParameter();
            sp_Podate2.ParameterName = "@Podate2";

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
            List<string> sqlFilter1 = new List<string>();
            List<string> sqlFilter2 = new List<string>();

            #region -- 條件組合 --
            switch (statusindex)
            {
                case 0:
                    if (!MyUtility.Check.Empty(IssueDate1) && !MyUtility.Check.Empty(IssueDate2))
                    {
                        sqlFilter1.Add(string.Format(@"LP.apvdate is not null and LP.issuedate between '{0}' and '{1}'"
                            , Convert.ToDateTime(IssueDate1).ToString("d"), Convert.ToDateTime(IssueDate2).ToString("d")));
                    }                   
                    else
                    {
                        if (!MyUtility.Check.Empty(IssueDate1))
                        {
                            sqlFilter1.Add(string.Format(@"LP.apvdate is not null and LP.issuedate >= '{0}' ", Convert.ToDateTime(IssueDate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(IssueDate2))
                        {
                            sqlFilter1.Add(string.Format(@"LP.apvdate is not null and  LP.issuedate <= '{0}' ", Convert.ToDateTime(IssueDate2).ToString("d")));
                        }
                    }
                    if (!MyUtility.Check.Empty(SciDelivery1) && !MyUtility.Check.Empty(SciDelivery2))
                    {
                        sqlFilter1.Add(string.Format(@"LP.apvdate is not null and o.SciDelivery between '{0}' and '{1}'"
                            , Convert.ToDateTime(SciDelivery1).ToString("d"), Convert.ToDateTime(SciDelivery2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(SciDelivery1))
                        {
                            sqlFilter1.Add(string.Format(@"LP.apvdate is not null and o.SciDelivery >= '{0}' ", Convert.ToDateTime(SciDelivery1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(SciDelivery2))
                        {
                            sqlFilter1.Add(string.Format(@"LP.apvdate is not null and o.SciDelivery <= '{0}' ", Convert.ToDateTime(SciDelivery2).ToString("d")));
                        }
                    }

                    break;

                case 1:
                    sqlFilter1.Add(@"LP.apvdate is null");
                    break;

                case 2:
                    if (!MyUtility.Check.Empty(IssueDate1) && !MyUtility.Check.Empty(IssueDate2))
                    {
                        sqlFilter1.Add(string.Format(@"(LP.issuedate between '{0}' and '{1}')"
                            , Convert.ToDateTime(IssueDate1).ToString("d"), Convert.ToDateTime(IssueDate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(IssueDate1))
                        {
                            sqlFilter1.Add(string.Format(@"(LP.issuedate >= '{0}') ", Convert.ToDateTime(IssueDate1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(IssueDate2))
                        {
                            sqlFilter1.Add(string.Format(@"(LP.issuedate <= '{0}') ", Convert.ToDateTime(IssueDate2).ToString("d")));
                        }
                    }
                    if (!MyUtility.Check.Empty(SciDelivery1) && !MyUtility.Check.Empty(SciDelivery2))
                    {
                        sqlFilter1.Add(string.Format(@"(o.SciDelivery between '{0}' and '{1}')"
                            , Convert.ToDateTime(SciDelivery1).ToString("d"), Convert.ToDateTime(SciDelivery2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(SciDelivery1))
                        {
                            sqlFilter1.Add(string.Format(@"(o.SciDelivery >= '{0}') ", Convert.ToDateTime(SciDelivery1).ToString("d")));
                        }
                        if (!MyUtility.Check.Empty(SciDelivery2))
                        {
                            sqlFilter1.Add(string.Format(@"(o.SciDelivery <= '{0}') ", Convert.ToDateTime(SciDelivery2).ToString("d")));
                        }
                    }
                    break;
            }
                        
            if (!MyUtility.Check.Empty(spno1))
            {
                sqlFilter1.Add("LPD.POID >= @spno1");
                sp_spno1.Value = spno1;
                cmds.Add(sp_spno1);
            }

            if (!MyUtility.Check.Empty(spno2))
            {
                sqlFilter1.Add("LPD.POID <= @spno2");
                sp_spno2.Value = spno2;
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(artworktype))
            {
                sqlFilter1.Add("LP.category = @artworktype");
                sp_artworktype.Value = artworktype;
                cmds.Add(sp_artworktype);
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlFilter1.Add("LP.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlFilter1.Add("LP.factoryid = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }
            #endregion 

            #region SQL Filter1
            
            if (!MyUtility.Check.Empty(artworktype))
            {
                sqlFilter2.Add("s.Category = @artworktype");
            }

            if (ordertypeindex >= 4) //include Forecast 
            {
                sqlFilter2.Add(string.Format(@"(O.category in {0} OR O.IsForecast =1)", ordertype));
            }
            else
            {
                sqlFilter2.Add(string.Format(@"O.category in {0} ", ordertype));
            }

            if (!MyUtility.Check.Empty(style))
            {
                sqlFilter2.Add("O.styleid = @style");
                sp_style.Value = style;
                cmds.Add(sp_style);
            }
            #endregion

            sqlCmd.Append(string.Format(@"
select  O.FactoryID
		, s.Category
		, O.POID
        , O.StyleID
		, O.BrandID
		, SMR = dbo.getTPEPass1(O.SMR)
		, y.Order_Qty
		, [Po_Qty] = sum (x.Po_Qty)
		, [Po_amt] = ROUND(sum (x.Po_amt), 3)
		, Po_price = round(sum (x.Po_amt) / iif(y.order_qty = 0, 1, y.order_qty), 3) 
		, std_price = round(y.order_amt / iif(y.order_qty = 0, 1, y.order_qty), 3) 
		, percentage = case isnull (y.order_amt / iif(y.order_qty = 0, 1, y.order_qty), 0)
                          when 0 then null
                          else round(round(sum (x.Po_amt) / iif(y.order_qty = 0, 1, y.order_qty), 3) 
                                           / round(y.order_amt / iif(y.order_qty = 0, 1, y.order_qty), 3), 2) 
                       end
        ,[Responsible_Reason]= ISNULL(IrregularPrice.Responsible,'')+ ISNULL(IrregularPrice.Reason,'')
from (
	select	distinct LP.Category
			, LPD.OrderId
	from dbo.LocalPO LP
	inner join dbo.LocalPO_Detail LPD on LP.Id = LPD.Id
    left join Orders O on lpd.OrderId = o.id
	where 1 = 1
          {1}
) s
left join Orders O on s.OrderId = O.ID
left join Order_TmsCost OTC on o.ID = OTC.ID and s.Category = OTC.ArtworkTypeID
outer apply(
	select	Price = sum(LPD.Price)
			, Po_Qty = sum(LPD.Qty)
			, Po_amt = sum(LPD.Qty * LPD.Price * dbo.getRate('{0}', LP.CurrencyID, 'USD', LP.IssueDate))
			, Rate = sum(dbo.getRate('{0}', LP.CurrencyID, 'USD', LP.IssueDate))
	from LocalPO LP
	inner join LocalPO_Detail LPD on LP.Id = LPD.Id
	where LP.Category = s.Category 
          and LPD.OrderId = O.ID
          {1}
) x
outer apply(
	select	Orders.POID
			, Order_Qty = sum(orders.Qty)
			, Order_amt = sum(Orders.Qty * Price)
	from Orders
	inner join Order_TmsCost OTC on OTC.ID = Orders.ID
	where O.POID = POID
          and ArtworkTypeID = s.Category
	group by Orders.POID, ArtworkTypeID
) y
outer apply(
	SELECT sr.Responsible ,sr.Reason , [ReasonID]=al.SubconReasonID , [IrregularPricePoid]=al.POID 
	FROM LocalPO_IrregularPrice al
	LEFT JOIN SubconReason sr ON al.SubconReasonID=sr.ID AND sr.Type='IP'
	WHERE al.POId = y.POID AND al.Category=s.Category
)IrregularPrice 

where Po_qty > 0 {2} {3}
group by O.FactoryID
		, s.Category
		, O.POID
        , O.StyleID
		, O.BrandID
        , O.SMR
		, y.Order_Qty
        , y.order_amt
		,IrregularPrice.Responsible
		,IrregularPrice.Reason
", ratetype
 , ("and " + sqlFilter1.JoinToString(" and "))
 , ("and " + sqlFilter2.JoinToString(" and "))
 , chk_IrregularPriceReason.Checked ? "AND IrregularPrice.IrregularPricePoid IS NOT NULL AND IrregularPrice.ReasonID IS NULL" : ""));
            #endregion

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

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R23.xltx",3);
            return true;
        }

        private void comboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateIssueDate.Enabled = !(comboStatus.SelectedIndex == 1);
        }
    }
}
