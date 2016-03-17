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
        string    mdivision, orderby, spno1, spno2, refno1, refno2;
        DateTime? deadline1, deadline2, buyerDelivery1, buyerDelivery2, eta1, eta2;
        DataTable printData;

        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory", out factory);
            txtMdivision1.Text = Sci.Env.User.Keyword;

        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) &&
                MyUtility.Check.Empty(dateRange2.Value1) &&
                MyUtility.Check.Empty(dateRange3.Value1) &&
                (MyUtility.Check.Empty(txtSpno1.Text) || MyUtility.Check.Empty(txtSpno2.Text)))
            {
                MyUtility.Msg.WarningBox("< Dead Line > & < Buyer Delivery > & < SP# > & < ETA > can't be empty!!");
                return false;
            }

            deadline1 = dateRange1.Value1;
            deadline2 = dateRange1.Value2;
            buyerDelivery1 = dateRange2.Value1;
            buyerDelivery2 = dateRange2.Value2;
            spno1 = txtSpno1.Text;
            spno2 = txtSpno2.Text;
            eta1 = dateRange3.Value1;
            eta2 = dateRange3.Value2;
            mdivision = txtMdivision1.Text;
            refno1 = txtRefno1.Text;
            refno2 = txtRefno2.Text;

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

            System.Data.SqlClient.SqlParameter sp_refno1 = new System.Data.SqlClient.SqlParameter();
            sp_refno1.ParameterName = "@refno1";

            System.Data.SqlClient.SqlParameter sp_refno2 = new System.Data.SqlClient.SqlParameter();
            sp_refno2.ParameterName = "@refno2";


            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                sqlCmd.Append(string.Format(@";with cte as 
(
	select poid from dbo.orders o 
	where o.BuyerDelivery between '{0}' and '{1}' group by POID
)
select rtrim(c.BLocation),a.POID,a.seq1,a.seq2, x.Refno ,x.ColorID ,c.LInvQty ,a.ETA
,DATEDIFF(day,a.eta,convert(date,getdate())) aging,'' remark
from cte
inner join Inventory a on a.POID = cte.POID 
inner join MDivisionPoDetail c on c.POID = a.POID and c.seq1 = a.seq1 and c.seq2 = a.Seq2
inner join Factory d on d.id = a.FactoryID
cross apply (select m.Refno,m.ColorID from dbo.PO_Supp_Detail m 
            where m.id = a.POID and m.seq1 = a.seq1 and m.seq2 = a.seq2 ) x
where LInvQty > 0 ", Convert.ToDateTime(buyerDelivery1).ToString("d"), Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }
            else
            {
                sqlCmd.Append(string.Format(@"select rtrim(c.BLocation),a.POID,a.seq1,a.seq2, x.Refno ,x.ColorID ,c.LInvQty ,a.ETA
,DATEDIFF(day,a.eta,convert(date,getdate())) aging,'' remark
from Inventory a
inner join MDivisionPoDetail c on c.POID = a.POID and c.seq1 = a.seq1 and c.seq2 = a.Seq2
inner join Factory d on d.id = a.FactoryID
cross apply (select d.Refno,d.ColorID from dbo.PO_Supp_Detail d where d.id = a.POID and d.seq1 = a.seq1 and d.seq2 = a.seq2 ) x
where LInvQty > 0 "));

            }

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(deadline1))
            {
                sqlCmd.Append(string.Format(@" and a.deadline between '{0}' and '{1}'",
                    Convert.ToDateTime(deadline1).ToString("d"), Convert.ToDateTime(deadline2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(spno1))
            {
                sqlCmd.Append(" and A.POID >= @spno1 and A.POID <= @spno2");
                sp_spno1.Value = spno1;
                sp_spno2.Value = spno2;
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            if (!MyUtility.Check.Empty(eta1))
            {
                sqlCmd.Append(string.Format(@" and a.eta between '{0}' and '{1}'"
                , Convert.ToDateTime(eta1).ToString("d"), Convert.ToDateTime(eta2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and d.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(refno1))
            {
                sqlCmd.Append(" and x.refno >= @refno1 and p.refno <= @refno2");
                sp_refno1.Value = refno1;
                sp_refno2.Value = refno2;
                cmds.Add(sp_refno1);
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
