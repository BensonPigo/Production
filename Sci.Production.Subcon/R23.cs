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
    public partial class R23 : Win.Tems.PrintForm
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
        private DateTime? IssueDate1;
        private DateTime? IssueDate2;
        private DateTime? SciDelivery1;
        private DateTime? SciDelivery2;
        private DataTable printData;

        /// <inheritdoc/>
        public R23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            this.txtMdivisionM.Text = Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboOrderType, 1, 1, ",Bulk,Sample,Material,Bulk+Sample,Bulk+Sample+Forecast,Bulk+Sample+Material+Forecast");
            this.comboOrderType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboRateType, 2, 1, "FX,Fixed Exchange Rate,KP,KPI Exchange Rate,DL,Daily Exchange Rate,3S,Custom Exchange Rate,RV,Currency Revaluation Rate,OT,One-time Exchange Rate");
            this.comboRateType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboStatus, 1, 1, "Only Approved,Only Unapproved,All");
            this.comboStatus.SelectedIndex = 0;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.comboStatus.SelectedIndex != 1 && ((MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2)) && (MyUtility.Check.Empty(this.dateSciDelivery.Value1) && MyUtility.Check.Empty(this.dateSciDelivery.Value2))))
            {
                MyUtility.Msg.WarningBox("[Issue Date] or [Sci Delivery] must input one condition !!");
                return false;
            }

            this.IssueDate1 = this.dateIssueDate.Value1;
            this.IssueDate2 = this.dateIssueDate.Value2;
            this.SciDelivery1 = this.dateSciDelivery.Value1;
            this.SciDelivery2 = this.dateSciDelivery.Value2;
            this.spno1 = this.txtSpnoStart.Text;
            this.spno2 = this.txtSpnoEnd.Text;
            this.artworktype = this.txtartworktype_ftyCategory.Text;
            this.mdivision = this.txtMdivisionM.Text;
            this.factory = this.comboFactory.Text;
            this.ordertypeindex = this.comboOrderType.SelectedIndex;
            this.ratetype = this.comboRateType.SelectedValue.ToString();
            this.statusindex = this.comboStatus.SelectedIndex;
            switch (this.ordertypeindex)
            {
                case 0:
                    this.ordertype = string.Empty;
                    break;
                case 1:
                    this.ordertype = "('B')";
                    break;
                case 2:
                    this.ordertype = "('S')";
                    break;
                case 3:
                    this.ordertype = "('M')";
                    break;
                case 4:
                    this.ordertype = "('B','S')";
                    break;
                case 5:
                    this.ordertype = "('B','S')";
                    break;
                case 6:
                    this.ordertype = "('B','S','M')";
                    break;
            }

            this.style = this.txtstyle.Text;
            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
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

            // List<string> sqlFilter2 = new List<string>();
            #region -- 條件組合 --
            switch (this.statusindex)
            {
                case 0:
                    if (!MyUtility.Check.Empty(this.IssueDate1) && !MyUtility.Check.Empty(this.IssueDate2))
                    {
                        sqlFilter1.Add(string.Format(
                            @"LP.apvdate is not null and LP.issuedate between '{0}' and '{1}'",
                            Convert.ToDateTime(this.IssueDate1).ToString("d"), Convert.ToDateTime(this.IssueDate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(this.IssueDate1))
                        {
                            sqlFilter1.Add(string.Format(@"LP.apvdate is not null and LP.issuedate >= '{0}' ", Convert.ToDateTime(this.IssueDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.IssueDate2))
                        {
                            sqlFilter1.Add(string.Format(@"LP.apvdate is not null and  LP.issuedate <= '{0}' ", Convert.ToDateTime(this.IssueDate2).ToString("d")));
                        }
                    }

                    if (!MyUtility.Check.Empty(this.SciDelivery1) && !MyUtility.Check.Empty(this.SciDelivery2))
                    {
                        sqlFilter1.Add(string.Format(
                            @"LP.apvdate is not null and o.SciDelivery between '{0}' and '{1}'",
                            Convert.ToDateTime(this.SciDelivery1).ToString("d"), Convert.ToDateTime(this.SciDelivery2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(this.SciDelivery1))
                        {
                            sqlFilter1.Add(string.Format(@"LP.apvdate is not null and o.SciDelivery >= '{0}' ", Convert.ToDateTime(this.SciDelivery1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.SciDelivery2))
                        {
                            sqlFilter1.Add(string.Format(@"LP.apvdate is not null and o.SciDelivery <= '{0}' ", Convert.ToDateTime(this.SciDelivery2).ToString("d")));
                        }
                    }

                    break;

                case 1:
                    sqlFilter1.Add(@"LP.apvdate is null");
                    break;

                case 2:
                    if (!MyUtility.Check.Empty(this.IssueDate1) && !MyUtility.Check.Empty(this.IssueDate2))
                    {
                        sqlFilter1.Add(string.Format(
                            @"(LP.issuedate between '{0}' and '{1}')",
                            Convert.ToDateTime(this.IssueDate1).ToString("d"), Convert.ToDateTime(this.IssueDate2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(this.IssueDate1))
                        {
                            sqlFilter1.Add(string.Format(@"(LP.issuedate >= '{0}') ", Convert.ToDateTime(this.IssueDate1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.IssueDate2))
                        {
                            sqlFilter1.Add(string.Format(@"(LP.issuedate <= '{0}') ", Convert.ToDateTime(this.IssueDate2).ToString("d")));
                        }
                    }

                    if (!MyUtility.Check.Empty(this.SciDelivery1) && !MyUtility.Check.Empty(this.SciDelivery2))
                    {
                        sqlFilter1.Add(string.Format(
                            @"(o.SciDelivery between '{0}' and '{1}')",
                            Convert.ToDateTime(this.SciDelivery1).ToString("d"), Convert.ToDateTime(this.SciDelivery2).ToString("d")));
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(this.SciDelivery1))
                        {
                            sqlFilter1.Add(string.Format(@"(o.SciDelivery >= '{0}') ", Convert.ToDateTime(this.SciDelivery1).ToString("d")));
                        }

                        if (!MyUtility.Check.Empty(this.SciDelivery2))
                        {
                            sqlFilter1.Add(string.Format(@"(o.SciDelivery <= '{0}') ", Convert.ToDateTime(this.SciDelivery2).ToString("d")));
                        }
                    }

                    break;
            }

            if (!MyUtility.Check.Empty(this.spno1))
            {
                sqlFilter1.Add("LPD.POID >= @spno1");
                sp_spno1.Value = this.spno1;
                cmds.Add(sp_spno1);
            }

            if (!MyUtility.Check.Empty(this.spno2))
            {
                sqlFilter1.Add("LPD.POID <= @spno2");
                sp_spno2.Value = this.spno2;
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlFilter1.Add("LP.category = @artworktype");
                sp_artworktype.Value = this.artworktype;
                cmds.Add(sp_artworktype);
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlFilter1.Add("LP.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlFilter1.Add("LP.factoryid = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.ordertype))
            {
                if (this.ordertypeindex >= 4)
                {
                    // include Forecast
                    sqlFilter1.Add(string.Format(@"(O.category in {0} OR O.IsForecast =1)", this.ordertype));
                }
                else
                {
                    sqlFilter1.Add(string.Format(@"O.category in {0} ", this.ordertype));
                }
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlFilter1.Add("O.styleid = @style");
                sp_style.Value = this.style;
                cmds.Add(sp_style);
            }
            #endregion

            sqlCmd.Append(string.Format(
                @"
select  O.FactoryID
		, o.MDivisionID
		, s.Category
		, opid=iif(o.id is null,s.OrderId,o.poid)
        , O.StyleID
		, O.BrandID
		, SMR = dbo.getTPEPass1(O.SMR)
		, y.Order_Qty
		, [Po_Qty] = sum (s.Po_Qty)
		, [Caron_Qty] = sum(s.CartonQty)
		, [Po_amt] = ROUND(sum (s.Po_amt), 3)
		, Po_price = round(sum (s.Po_amt) / iif(y.order_qty = 0, 1, y.order_qty), 3) 
		, std_price = round(y.order_amt / iif(y.order_qty = 0, 1, y.order_qty), 3) 
		, percentage = case isnull (y.order_amt / iif(y.order_qty = 0, 1, y.order_qty), 0)
                          when 0 then null
                          else round(round(sum (s.Po_amt) / iif(y.order_qty = 0, 1, y.order_qty), 3) 
                                           / round(y.order_amt / iif(y.order_qty = 0, 1, y.order_qty), 3), 2) 
                       end
        ,[Responsible_Reason]=IIF(IrregularPrice.Responsible IS NULL OR IrregularPrice.Responsible = '' 
								,''
								,ISNULL(IrregularPrice.Responsible,'')+' - '+ ISNULL(IrregularPrice.Reason,'')) 
from (
	select	 LP.Category
			, LPD.OrderId
			, Price = sum(LPD.Price)
			, Po_Qty = sum(LPD.Qty)
			, Po_amt = sum(LPD.Qty * LPD.Price * dbo.getRate('{0}', LP.CurrencyID, 'USD', LP.IssueDate))
			, Rate = sum(dbo.getRate('{0}', LP.CurrencyID, 'USD', LP.IssueDate))
			, [CartonQty] = sum(iif(li.IsCarton=1,LPD.Qty,0))
	from dbo.LocalPO LP
	inner join dbo.LocalPO_Detail LPD on LP.Id = LPD.Id
    left join Orders O on lpd.OrderId = o.id
	left join LocalItem li on li.RefNo=lpd.Refno
	where 1 = 1
          {1}
	group by LP.Category, LPD.OrderId, o.poid
	having sum(LPD.Qty)>0
) s
left join Orders O on s.OrderId = O.ID
outer apply(
	select	 Order_Qty = sum(orders.Qty)
			, Order_amt = sum(Orders.Qty * Price)
	from Orders
	inner join Order_TmsCost OTC on OTC.ID = Orders.ID
	where O.POID = POID and ArtworkTypeID = s.Category
) y
outer apply(
	SELECT [Responsible]=d.Name ,sr.Reason , [ReasonID]=al.SubconReasonID , [IrregularPricePoid]=al.POID 
	FROM LocalPO_IrregularPrice al
	LEFT JOIN SubconReason sr ON al.SubconReasonID=sr.ID AND sr.Type='IP'
    LEFT JOIN DropDownList  d ON d.type = 'Pms_PoIr_Responsible' AND d.ID=sr.Responsible
	WHERE al.POId = o.POID AND al.Category=s.Category
)IrregularPrice 
where 1=1 {2} 
group by O.FactoryID
		, o.MDivisionID
		, s.Category
		, iif(o.id is null,s.OrderId,o.poid)
        , O.StyleID
		, O.BrandID
        , O.SMR
		, y.Order_Qty
        , y.order_amt
		,IrregularPrice.Responsible
		,IrregularPrice.Reason
{3}

", this.ratetype,
                "and " + sqlFilter1.JoinToString(" and "),
                this.chk_IrregularPriceReason.Checked ? " AND (IrregularPrice.ReasonID IS NULL OR IrregularPrice.ReasonID ='') "
                                    : string.Empty,
                this.chk_IrregularPriceReason.Checked ? " HAVING( round(sum (s.Po_amt) / iif(y.order_qty = 0, 1, y.order_qty), 3) >  round(y.order_amt / iif(y.order_qty = 0, 1, y.order_qty), 3) ) "
                                    : string.Empty));
            #endregion

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

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R23.xltx", 3);
            return true;
        }

        private void ComboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dateIssueDate.Enabled = !(this.comboStatus.SelectedIndex == 1);
        }
    }
}
