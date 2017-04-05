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

namespace Sci.Production.Subcon
{
    public partial class R23 : Sci.Win.Tems.PrintForm
    {
        string artworktype, factory, style, mdivision, spno1, spno2, ordertype,ratetype;//,status;
        int ordertypeindex,statusindex;
        DateTime? IssueDate1, IssueDate2;
        DataTable printData;

        public R23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(cbbFactory, 1, factory);
            cbbFactory.Text = Sci.Env.User.Factory;
            txtMdivision1.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(cbbOrderType, 1, 1, "Bulk,Sample,Material,Bulk+Sample,Bulk+Sample+Forecast,Bulk+Sample+Material+Forecast");
            cbbOrderType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(cbbRateType, 2, 1, "FX,Fixed Exchange Rate,KP,KPI Exchange Rate,DL,Daily Exchange Rate,3S,Custom Exchange Rate,RV,Currency Revaluation Rate,OT,One-time Exchange Rate");
            cbbRateType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(cbbStatus, 1, 1, "Only Approved,Only Unapproved,All");
            cbbStatus.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {

            if (cbbStatus.SelectedIndex != 1 && MyUtility.Check.Empty(dateRangePoDate.Value1) && MyUtility.Check.Empty(dateRangePoDate.Value2))
            {
                MyUtility.Msg.WarningBox("Issue Date can't empty!!");
                return false;
            }
            IssueDate1 = dateRangePoDate.Value1;
            IssueDate2 = dateRangePoDate.Value2;
            spno1 = txtSpno1.Text;
            spno2 = txtSpno2.Text;
            artworktype = txtartworktype_fty1.Text;
            mdivision = txtMdivision1.Text;
            factory = cbbFactory.Text;
            ordertypeindex = cbbOrderType.SelectedIndex;
            ratetype = cbbRateType.SelectedValue.ToString();
            statusindex = cbbStatus.SelectedIndex;
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

            style = txtstyle1.Text;
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
                    break;
            }

            
            if (!MyUtility.Check.Empty(spno1))
            {
                sqlFilter1.Add("LPD.OrderID >= @spno1");
                sp_spno1.Value = spno1;
                cmds.Add(sp_spno1);
            }

            if (!MyUtility.Check.Empty(spno2))
            {
                sqlFilter1.Add("LPD.OrderID <= @spno2");
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
select	O.FactoryID
		, s.Category
		, O.POID
        , O.StyleID
		, O.BrandID
		, SMR = dbo.getTPEPass1(O.SMR)
		, y.Order_Qty
		, x.Po_Qty
		, ROUND(x.Po_amt, 3)
		, Po_price = round(x.Po_amt / iif(y.order_qty = 0, 1, y.order_qty), 3) 
		, std_price = round(y.order_amt/iif(y.order_qty = 0, 1, y.order_qty), 3) 
		, percentage = round(x.Po_amt / iif(y.order_qty = 0, 1, y.order_qty) / iif(y.order_amt = 0 or y.order_qty = 0, 1, (y.order_amt / y.order_qty)), 2)  
from (
	select	distinct LP.Category
			, LPD.OrderId
	from dbo.LocalPO LP
	inner join dbo.LocalPO_Detail LPD on LP.Id = LPD.Id
	where 1 = 1 {1}
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
	where LP.Category = s.Category and LPD.OrderId = O.POID and LP.Status = 'Approved' 
        {1}
) x
outer apply(
	select	Orders.POID
			, Order_Qty = sum(orders.Qty)
			, Order_amt = sum(Orders.Qty * Price)
	from Orders
	inner join Order_TmsCost OTC on OTC.ID = Orders.ID
	where O.POID = POID and ArtworkTypeID = s.Category
	group by Orders.POID, ArtworkTypeID
) y
where Po_qty > 0 {2}
", ratetype
 , ("and " + sqlFilter1.JoinToString(" and "))
 , ("and " + sqlFilter2.JoinToString(" and "))));
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

        private void cbbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateRangePoDate.Enabled = !(cbbStatus.SelectedIndex == 1);
        }
    }
}
