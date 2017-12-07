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
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R15 : Sci.Win.Tems.PrintForm
    {
        string reason, mdivision, factory, stocktype="", spno1, spno2;
        DateTime? issueDate1, issueDate2;
        DataTable printData;

        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateIssueDate.Value1) && MyUtility.Check.Empty(dateIssueDate.Value2)) 
            {
                MyUtility.Msg.WarningBox("< Issue Date > can't be empty!!");
                return false;
            }

            issueDate1 = dateIssueDate.Value1;
            issueDate2 = dateIssueDate.Value2;
            mdivision = txtMdivision.Text;
            factory = txtfactory.Text;
            reason = txtwhseReasonCode.TextBox1.Text;
            spno1 = txtSPNoStart.Text;
            spno2 = txtSPNoEnd.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@Factory";

            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
select  a.MDivisionID 
        ,orders.FactoryID
        , a.Id
        ,a.IssueDate
        ,b.POID
        ,b.seq1
        ,b.seq2
        ,b.roll
        ,b.dyelot
        ,c.Refno
        ,c.ColorID
        ,c.SizeSpec
        ,fabrictype = iif(c.FabricType='F', 'Fabric'
                                          , iif(c.FabricType='A','Accessory'
                                                                ,c.fabrictype)) 
        ,b.Qty
        ,c.StockUnit
        ,editname = dbo.getPass1(a.EditName) 
        ,a.Remark
        ,a.WhseReasonID+'-'+ISNULL((select d.Description 
                                    from whsereason d WITH (NOLOCK) 
                                    WHERE d.id = a.whsereasonid and d.Type = 'IR')
                                   ,'')
from issue as a WITH (NOLOCK) 
inner join issue_detail b WITH (NOLOCK) on a.id = b.id
inner join Orders orders on b.POID = orders.id
left join po_supp_detail c WITH (NOLOCK) on c.id = b.poid and c.seq1 = b.seq1 and c.seq2 =b.seq2
where a.type = 'D' AND a.Status = 'Confirmed' 
", stocktype));

            if (!MyUtility.Check.Empty(issueDate1))
                sqlCmd.Append(string.Format(" and '{0}' <= a.issuedate", Convert.ToDateTime(issueDate1).ToString("d")));
            if (!MyUtility.Check.Empty(issueDate2))
                sqlCmd.Append(string.Format(" and a.issuedate <= '{0}'", Convert.ToDateTime(issueDate2).ToString("d")));
            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and orders.FactoryID = @Factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(reason))
            {
                sqlCmd.Append(string.Format(@" and A.WhseReasonID = '{0}'", reason));
            }

            if (!MyUtility.Check.Empty(spno1) && !MyUtility.Check.Empty(spno2))
            {
                //若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and b.Poid >= @spno1 and b.Poid <= @spno2");
                sp_spno1.Value = spno1.PadRight(10, '0');
                sp_spno2.Value = spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(spno1))
            {
                //只有 sp1 輸入資料
                sqlCmd.Append(" and b.Poid like @spno1 ");
                sp_spno1.Value = spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(spno2))
            {
                //只有 sp2 輸入資料
                sqlCmd.Append(" and b.Poid like @spno2 ");
                sp_spno2.Value = spno2 + "%";
                cmds.Add(sp_spno2);
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

            bool s = MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R15.xltx", 2, true, null, null);
            return true;
        }
    }
}
