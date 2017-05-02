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
    public partial class R10 : Sci.Win.Tems.PrintForm
    {
        //string    mdivision, factory, orderby, spno1, spno2, refno1, refno2;
        string mdivision, factory, spno1, spno2, refno1, refno2;
        DateTime? deadline1, deadline2, buyerDelivery1, buyerDelivery2, eta1, eta2;
        DataTable printData;

        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            txtMdivision.Text = Sci.Env.User.Keyword;

        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateDeadLine.Value1) && MyUtility.Check.Empty(dateDeadLine.Value2) &&
                MyUtility.Check.Empty(dateBuyerDelivery.Value1) && MyUtility.Check.Empty(dateBuyerDelivery.Value2) &&
                MyUtility.Check.Empty(dateInventoryETA.Value1) && MyUtility.Check.Empty(dateInventoryETA.Value2) &&
                (MyUtility.Check.Empty(txtSPNoStart.Text) && MyUtility.Check.Empty(txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Dead Line > & < Buyer Delivery > & < SP# > & < ETA > can't be empty!!");
                return false;
            }

            deadline1 = dateDeadLine.Value1;
            deadline2 = dateDeadLine.Value2;
            buyerDelivery1 = dateBuyerDelivery.Value1;
            buyerDelivery2 = dateBuyerDelivery.Value2;
            spno1 = txtSPNoStart.Text;
            spno2 = txtSPNoEnd.Text;
            eta1 = dateInventoryETA.Value1;
            eta2 = dateInventoryETA.Value2;
            mdivision = txtMdivision.Text;
            factory = txtfactory.Text;
            refno1 = txtRefnoStart.Text;
            refno2 = txtRefnoEnd.Text;

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

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_refno1 = new System.Data.SqlClient.SqlParameter();
            sp_refno1.ParameterName = "@refno1";

            System.Data.SqlClient.SqlParameter sp_refno2 = new System.Data.SqlClient.SqlParameter();
            sp_refno2.ParameterName = "@refno2";


            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            if (!MyUtility.Check.Empty(buyerDelivery1) || !MyUtility.Check.Empty(buyerDelivery2))
            {
                string sqlBuyerDelivery = "";
                if (!MyUtility.Check.Empty(buyerDelivery1))
                    sqlBuyerDelivery += string.Format(" '{0}' <= o.BuyerDelivery ", Convert.ToDateTime(buyerDelivery1).ToString("d"));
                if (!MyUtility.Check.Empty(buyerDelivery2))
                    sqlBuyerDelivery += (MyUtility.Check.Empty(sqlBuyerDelivery) ? "" : " and ") + string.Format(" o.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDelivery2).ToString("d"));

                sqlCmd.Append(string.Format(@"
;with cte as 
(
    select poid 
    from dbo.orders o WITH (NOLOCK) 
    where {0} 
    group by POID
)
select  orders.factoryID
        ,rtrim(c.BLocation)
        ,a.POID
        ,a.seq1
        ,a.seq2
        ,x.Refno 
        ,x.ColorID 
        ,c.LInvQty 
        ,a.ETA
        ,aging = DATEDIFF(day,a.eta,convert(date,getdate())) 
        ,remark = '' 
from cte
inner join Inventory a WITH (NOLOCK) on a.POID = cte.POID 
inner join MDivisionPoDetail c WITH (NOLOCK) on c.POID = a.POID and c.seq1 = a.seq1 and c.seq2 = a.Seq2
inner join Orders orders on c.poid = orders.id
inner join Factory d WITH (NOLOCK) on orders.FactoryID = d.id
cross apply (select m.Refno
                    ,m.ColorID 
             from dbo.PO_Supp_Detail m WITH (NOLOCK) 
             where m.id = a.POID and m.seq1 = a.seq1 and m.seq2 = a.seq2 ) x
where LInvQty > 0 ", sqlBuyerDelivery));
            }
            else
            {
                sqlCmd.Append(string.Format(@"
select  orders.factoryID
        ,rtrim(c.BLocation)
        ,a.POID
        ,a.seq1
        ,a.seq2
        , x.Refno 
        ,x.ColorID 
        ,c.LInvQty 
        ,a.ETA
        ,aging = DATEDIFF(day,a.eta,convert(date,getdate())) 
        ,remark = '' 
from Inventory a WITH (NOLOCK) 
inner join MDivisionPoDetail c WITH (NOLOCK) on c.POID = a.POID and c.seq1 = a.seq1 and c.seq2 = a.Seq2
inner join Orders orders on c.poid = orders.id
inner join Factory d WITH (NOLOCK) on orders.FactoryID = d.id
cross apply (select d.Refno
                    ,d.ColorID 
             from dbo.PO_Supp_Detail d WITH (NOLOCK) 
             where d.id = a.POID and d.seq1 = a.seq1 and d.seq2 = a.seq2 ) x
where LInvQty > 0 "));

            }

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(deadline1) || !MyUtility.Check.Empty(deadline2))
            {
                if (!MyUtility.Check.Empty(deadline1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= a.deadline", Convert.ToDateTime(deadline1).ToString("d")));
                if (!MyUtility.Check.Empty(deadline2))
                    sqlCmd.Append(string.Format(@" and a.deadline <= '{0}'", Convert.ToDateTime(deadline2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(spno1) && !MyUtility.Check.Empty(spno2))
            {
                //若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and a.Poid >= @spno1 and a.Poid <= @spno2");
                sp_spno1.Value = spno1.PadRight(10, '0');
                sp_spno2.Value = spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(spno1))
            {
                //只有 sp1 輸入資料
                sqlCmd.Append(" and a.Poid like @spno1 ");
                sp_spno1.Value = spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(spno2))
            {
                //只有 sp2 輸入資料
                sqlCmd.Append(" and a.Poid like @spno2 ");
                sp_spno2.Value = spno2 + "%";
                cmds.Add(sp_spno2);
            }
            if (!MyUtility.Check.Empty(eta1) || !MyUtility.Check.Empty(eta2))
            {
                if (!MyUtility.Check.Empty(eta1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= a.eta", Convert.ToDateTime(eta1).ToString("d")));
                if (!MyUtility.Check.Empty(eta2))
                    sqlCmd.Append(string.Format(@" and a.eta <= '{0}'", Convert.ToDateTime(eta2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and d.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and orders.FactoryID = @Factory");
                sp_factory.Value = mdivision;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(refno1) && !MyUtility.Check.Empty(refno2))
            {
                //Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and x.refno >= @refno1 and x.refno <= @refno2");
                sp_refno1.Value = refno1;
                sp_refno2.Value = refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }
            else if (!MyUtility.Check.Empty(refno1))
            {
                //只輸入 Refno1
                sqlCmd.Append(" and x.refno like @refno1");
                sp_refno1.Value = refno1 + "%";
                cmds.Add(sp_refno1);
            }
            else if (!MyUtility.Check.Empty(refno2))
            {
                //只輸入 Refno2
                sqlCmd.Append(" and x.refno like @refno2");
                sp_refno2.Value = refno2 + "%";
                cmds.Add(sp_refno2);
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

            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R10.xltx", 2);
            return true;
        }
    }
}
