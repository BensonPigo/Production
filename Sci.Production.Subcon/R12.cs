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
    public partial class R12 : Sci.Win.Tems.PrintForm
    {
        string artworktype, factory, subcon, mdivision, orderby;
        DateTime? APdate1, APdate2;
        DataTable printData;

        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory", out factory);
            MyUtility.Tool.SetupCombox(cbbFactory, 1, factory);
            cbbFactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(cbbOrderBy, 1, 1, "Supplier,Handle");
            cbbOrderBy.SelectedIndex = 0;
            txtMdivision1.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("AP Date can't empty!!");
                return false;
            }
            APdate1 = dateRange1.Value1;
            APdate2 = dateRange1.Value2;
           
            artworktype = txtartworktype_fty1.Text;
            mdivision = txtMdivision1.Text;
            factory = cbbFactory.Text;
            subcon = txtsubcon1.TextBox1.Text;
            orderby = cbbOrderBy.Text;
            
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- Sql Command --
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"Select a.MDivisionID
,a.FactoryID
,a.LocalSuppID
,(select abb from LocalSupp where id = a.LocalSuppID) supplier
,a.Id
,a.IssueDate
,dbo.getpass1(a.Handle) handle
,a.CurrencyID
,a.Amount
,a.ArtworkTypeID
,b.ArtworkPoID
,b.OrderID
,(select orders.StyleID from orders where id = b.OrderID) style
,dbo.getPass1(c.Handle) pohandle
,c.Amount+c.Vat poAmount
,c.IssueDate poDate
,a.InvNo
from artworkap a inner join artworkap_detail b on a.id = b.id 
left join ArtworkPO c on c.ID = b.ArtworkPoID
where  a.ApvDate is null and a.issuedate between '{0}' and '{1}'
", Convert.ToDateTime(APdate1).ToString("d"), Convert.ToDateTime(APdate2).ToString("d")));
            #endregion

            System.Data.SqlClient.SqlParameter sp_artworktype = new System.Data.SqlClient.SqlParameter();
            sp_artworktype.ParameterName = "@artworktype";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

             IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

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
            if (!MyUtility.Check.Empty(subcon))
            {
                sqlCmd.Append(" and a.localsuppid = @subcon");
                sp_subcon.Value = subcon;
                cmds.Add(sp_subcon);
            }

            if (orderby.ToUpper() == "SUPPLIER")
                sqlCmd.Append(" order by a.localsuppid ");
            else
                sqlCmd.Append(" order by a.handle ");

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

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R12.xltx",2);
            return true;
        }
    }
}
