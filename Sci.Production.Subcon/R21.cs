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
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        string category, factory, subcon, mdivision, orderby;
        DateTime? APdate1, APdate2;
        DataTable printData;

        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(comboOrderBy, 1, 1, "Supplier,Handle");
            comboOrderBy.SelectedIndex = 0;
            txtMdivisionM.Text = Sci.Env.User.Keyword;

            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            int year = DateTime.Today.Year;
            this.dateAPDate.Value1 = DateTime.Today.AddMonths(-month+1).AddDays(-day + 1);
            this.dateAPDate.Value2 = DateTime.Now;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateAPDate.Value1) && MyUtility.Check.Empty(dateAPDate.Value2))
            {
                MyUtility.Msg.WarningBox("AP Date can't empty!!");
                return false;
            }
            APdate1 = dateAPDate.Value1;
            APdate2 = dateAPDate.Value2;
           
            category = txtartworktype_ftyCategory.Text;
            mdivision = txtMdivisionM.Text;
            factory = comboFactory.Text;
            subcon = txtsubconSupplier.TextBox1.Text;
            orderby = comboOrderBy.Text;
            
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- Sql Command --
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
Select (select FactoryId from orders WITH (NOLOCK) where id = b.OrderId) order_factory
        ,a.MDivisionID
        ,a.FactoryID
        ,a.LocalSuppID
        ,(select abb from LocalSupp WITH (NOLOCK) where id = a.LocalSuppID) supplier
        ,a.Id
        ,a.Status
        ,a.IssueDate
        ,vs1.Name_Extno Handle
        ,a.CurrencyID
        --,a.Amount+a.vat apAmount
        -- mantis8356 改由detail抓資料
        ,round(PaAmount.Amount,isnull(cy.exact,2))+ round((PaAmount.Amount * a.VatRate/100),isnull(cy.exact,2)) apAmount
        ,a.Category
        ,b.OrderID
        ,b.Refno
        ,b.ThreadColorID
        ,b.UnitID
        ,b.Price
        ,b.Qty
        ,b.Price*b.Qty ApAmount
        ,b.LocalPoId
        ,vs2.Name POHandle
        ,PoAmount.Amount poAmount
        ,c.IssueDate poDate
        ,a.InvNo
from localap a WITH (NOLOCK) 
inner join LocalAP_Detail b on a.id = b.id 
left join LocalPO c on c.ID = b.LocalPoId
left join Currency cy WITH (NOLOCK) on a.CurrencyID = cy.id  
outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
outer apply (
    (select name = concat(name, ' Ext.', ExtNo)
    from TPEPass1
    where id = (select PoHandle from Po where id = b.OrderId))
) vs2
outer apply (
    select amount = sum(isnull(F.Price * F.Qty, 0.00)) 
    from LocalPo_Detail f
    where c.id = f.id and b.orderid = f.orderid    
) PoAmount
outer apply (
    select amount = sum(isnull(ld.Price * ld.Qty, 0.00)) 
    from LocalAP_Detail ld
    where a.id = ld.id
) PaAmount
where a.ApvDate is null and 1=1"));
            #endregion
            System.Data.SqlClient.SqlParameter sp_APdate1 = new System.Data.SqlClient.SqlParameter();
            sp_APdate1.ParameterName = "@APdate1";

            System.Data.SqlClient.SqlParameter sp_APdate2 = new System.Data.SqlClient.SqlParameter();
            sp_APdate2.ParameterName = "@APdate2";

            System.Data.SqlClient.SqlParameter sp_category = new System.Data.SqlClient.SqlParameter();
            sp_category.ParameterName = "@category";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

             IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

             if (!MyUtility.Check.Empty(APdate1))
             {
                 sqlCmd.Append(" and a.issuedate >= @APdate1");
                 sp_APdate1.Value = APdate1;
                 cmds.Add(sp_APdate1);
             }

             if (!MyUtility.Check.Empty(APdate2))
             {
                 sqlCmd.Append(" and a.issuedate <= @APdate2");
                 sp_APdate2.Value = APdate2;
                 cmds.Add(sp_APdate2);
             }

            if (!MyUtility.Check.Empty(category))
            {
                sqlCmd.Append(" and a.category = @category");
                sp_category.Value = category;
                cmds.Add(sp_category);
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
                sqlCmd.Append(" order by a.localsuppid, a.MDivisionID, a.FactoryID, a.Id, a.handle ");
            else
                sqlCmd.Append(" order by a.handle, a.MDivisionID, a.FactoryID, a.Id, a.LocalSuppID ");

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

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R21.xltx",2);
            return true;
        }
    }
}
