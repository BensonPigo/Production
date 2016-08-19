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

namespace Sci.Production.Warehouse
{
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        string season, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        DateTime? sciDelivery1, sciDelivery2, suppDelivery1, suppDelivery2, eta1, eta2, ata1, ata2;
        DataTable printData;

        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(cbbFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            cbbFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(cbbOrderBy, 1, 1, "Supplier,SP#");
            cbbOrderBy.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) &&
                MyUtility.Check.Empty(dateRange2.Value1) &&
                MyUtility.Check.Empty(dateRange4.Value1) &&
                MyUtility.Check.Empty(dateRange3.Value1) &&
                (MyUtility.Check.Empty(txtSpno1.Text) || MyUtility.Check.Empty(txtSpno2.Text)))
            {
                MyUtility.Msg.WarningBox("< Supp Delivery > & < SCI Delivery > & < ETA > & < FinalETA >& < SP# > & < Refno > can't be empty!!");
                return false;
            }
            #region -- 擇一必輸的條件 --
            sciDelivery1 = dateRange1.Value1;
            sciDelivery2 = dateRange1.Value2;
            suppDelivery1 = dateRange2.Value1;
            suppDelivery2 = dateRange2.Value2;
            eta1 = dateRange4.Value1;
            eta2 = dateRange4.Value2;
            ata1 = dateRange3.Value1;
            ata2 = dateRange3.Value2;
            spno1 = txtSpno1.Text;
            spno2 = txtSpno2.Text;
            refno1 = txtRefno1.Text;
            refno2 = txtRefno2.Text;
            #endregion

            country = txtcountry1.Text;
            supp = txtsupplier1.TextBox1.Text;
            style = txtstyle1.Text;
            season = txtseason1.Text;
            mdivision = txtMdivision1.Text;
            fabrictype = cbbFabricType.SelectedValue.ToString();
            orderby = cbbOrderBy.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --
            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            System.Data.SqlClient.SqlParameter sp_season = new System.Data.SqlClient.SqlParameter();
            sp_season.ParameterName = "@season";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_refno1 = new System.Data.SqlClient.SqlParameter();
            sp_refno1.ParameterName = "@refno1";

            System.Data.SqlClient.SqlParameter sp_refno2 = new System.Data.SqlClient.SqlParameter();
            sp_refno2.ParameterName = "@refno2";


            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@";with cte as
(
select distinct o.mdivisionid,o.POID,o.StyleID,o.SeasonID from dbo.orders o
where 1=1"));

                if (!MyUtility.Check.Empty(sciDelivery1))
                {
                    sqlCmd.Append(string.Format(@" and o.SciDelivery between '{0}' and '{1}'",
                    Convert.ToDateTime(sciDelivery1).ToString("d"), Convert.ToDateTime(sciDelivery2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(spno1))
                {
                    sqlCmd.Append(" and o.id >= @spno1 and o.id <= @spno2");
                    sp_spno1.Value = spno1;
                    sp_spno2.Value = spno2;
                    cmds.Add(sp_spno1);
                    cmds.Add(sp_spno2);
                }
                if (!MyUtility.Check.Empty(style))
                {
                    sqlCmd.Append(" and o.styleid = @style");
                    sp_style.Value = style;
                    cmds.Add(sp_style);
                }
                if (!MyUtility.Check.Empty(season))
                {
                    sqlCmd.Append(" and o.seasonid = @season");
                    sp_season.Value = season;
                    cmds.Add(sp_season);
                }
                sqlCmd.Append(")");
                sqlCmd.Append(string.Format(@"select isnull(d.mdivisionid,cte.mdivisionid),a.id,
cte.StyleID,
b.FinalETD,
a.suppid+'-'+c.AbbEN supp,
c.CountryID
,b.Refno
,b.SEQ1
,b.SEQ2
,iif(b.fabrictype = 'F','Fabric',iif(b.fabrictype='A','Accessory',b.fabrictype)) fabrictype
,dbo.getMtlDesc(b.id,b.seq1,b.seq2,2,0)
,b.Qty
,b.NETQty
,b.NETQty+b.LossQty
,b.ShipQty
,b.ShipFOC
,b.ApQty
,b.InputQty
,b.POUnit
,iif(b.Complete=1,'Y','N')
,b.ETA
,b.FinalETA
,(select t.orderid+',' from (select OrderID from DBO.PO_Supp_Detail_OrderList where id=b.id and seq1=b.seq1 and seq2 = b.SEQ2) t for xml path('')) orderlist
,b.StockUnit
,d.InQty
,d.OutQty
,d.AdjustQty
,d.InQty - d.OutQty + d.AdjustQty balance
,d.ALocation
,d.BLocation
,case b.FabricType 
	when 'F' then (select x.result+'/' from (select iif(t2.result='P','Pass',iif(t2.result='F','Fail',t2.Result)) result
	from dbo.FIR t2 where t2.POID = b.ID and t2.seq1 = b.seq1 and t2.seq2 = b.seq2) x for xml path(''))
	when 'A' then (select x.result+'/' from (select iif(t3.result='P','Pass',iif(t3.result='F','Fail',t3.Result)) result
	from dbo.AIR t3 where t3.POID = b.ID and t3.seq1 = b.seq1 and t3.seq2 = b.seq2) x for xml path(''))
end
 from cte 
inner join dbo.PO_Supp a on a.id = cte.poid
inner join dbo.PO_Supp_Detail b on b.id = a.id and b.SEQ1 = a.SEQ1
inner join dbo.Supp c on c.id = a.SuppID
left join dbo.MDivisionPoDetail d on d.POID = b.ID and d.seq1 = b.seq1 and d.seq2 = b.SEQ2
where 1= 1 "));
            }
            else
            {
                sqlCmd.Append(string.Format(@"select isnull(d.mdivisionid,(select orders.mdivisionid from dbo.orders where id = a.id)),a.id,
(select StyleID from dbo.orders where id = a.id) style,
b.FinalETD,
a.suppid+'-'+c.AbbEN supp,
c.CountryID
,b.Refno
,b.SEQ1
,b.SEQ2
,iif(b.fabrictype = 'F','Fabric',iif(b.fabrictype='A','Accessory',b.fabrictype)) fabrictype
,dbo.getMtlDesc(b.id,b.seq1,b.seq2,2,0)
,b.Qty
,b.NETQty
,b.NETQty+b.LossQty
,b.ShipQty
,b.ShipFOC
,b.ApQty
,b.InputQty
,b.POUnit
,iif(b.Complete=1,'Y','N')
,b.ETA
,b.FinalETA
,(select t.orderid+',' from (select OrderID from DBO.PO_Supp_Detail_OrderList where id=b.id and seq1=b.seq1 and seq2 = b.SEQ2) t for xml path('')) orderlist
,b.StockUnit
,d.InQty
,d.OutQty
,d.AdjustQty
,d.InQty - d.OutQty + d.AdjustQty balance
,d.ALocation
,d.BLocation
,case b.FabricType 
	when 'F' then (select x.result+'/' from (select iif(t2.result='P','Pass',iif(t2.result='F','Fail',t2.Result)) result
	from dbo.FIR t2 where t2.POID = b.ID and t2.seq1 = b.seq1 and t2.seq2 = b.seq2) x for xml path(''))
	when 'A' then (select x.result+'/' from (select iif(t3.result='P','Pass',iif(t3.result='F','Fail',t3.Result)) result
	from dbo.AIR t3 where t3.POID = b.ID and t3.seq1 = b.seq1 and t3.seq2 = b.seq2) x for xml path(''))
end
 from dbo.PO_Supp a
inner join dbo.PO_Supp_Detail b on b.id = a.id and b.SEQ1 = a.SEQ1
inner join dbo.Supp c on c.id = a.SuppID
left join dbo.MDivisionPoDetail d on d.POID = b.ID and d.seq1 = b.seq1 and d.seq2 = b.SEQ2
where 1=1 "));
            }

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(suppDelivery1))
            {
                sqlCmd.Append(string.Format(@" and b.finaletd between '{0}' and '{1}'"
                , Convert.ToDateTime(suppDelivery1).ToString("d"), Convert.ToDateTime(suppDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(eta1))
            {
                sqlCmd.Append(string.Format(@" and b.ETA between '{0}' and '{1}'"
                , Convert.ToDateTime(eta1).ToString("d"), Convert.ToDateTime(eta2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(ata1))
            {
                sqlCmd.Append(string.Format(@" and b.FinalETA between '{0}' and '{1}'"
                , Convert.ToDateTime(ata1).ToString("d"), Convert.ToDateTime(ata2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(country))
            {
                sqlCmd.Append(string.Format(" and c.country = '(0}'",country));
            }

            if (!MyUtility.Check.Empty(supp))
            {
                sqlCmd.Append(string.Format(" and a.suppid = '{0}'",supp));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and d.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(fabrictype))
            {
                sqlCmd.Append(string.Format(@" and b.FabricType = '{0}'", fabrictype));
            }

            if (!MyUtility.Check.Empty(refno1))
            {
                sqlCmd.Append(" and b.refno >= @refno1 and b.refno <= @refno2");
                sp_refno1.Value = refno1;
                sp_refno2.Value = refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }

            if (orderby.ToUpper().TrimEnd() == "SUPPLIER")
            {
                sqlCmd.Append(" ORDER BY A.SUPPID,B.ID,B.SEQ1,B.SEQ2 ");
            }
            else
            {
                sqlCmd.Append(" ORDER BY B.ID,B.SEQ1,B.SEQ2 ");
            }

            #endregion

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

            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R03.xltx", 1);

            return true;
        }
    }
}
