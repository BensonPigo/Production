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
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        string artworktype, factory, subcon, spno, style, orderby, mdivision;
        DateTime? issuedate1, issuedate2;
        DataTable printData;

        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(cbbFactory, 1, factory);
            cbbFactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(cbbOrderBy, 1, 1, "Issue date,Supplier");
            cbbOrderBy.SelectedIndex = 0;
            txtMdivision1.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2))
            {
                MyUtility.Msg.WarningBox("Issue Date can't empty!!");
                return false;
            }
            issuedate1 = dateRange1.Value1;
            issuedate2 = dateRange1.Value2;

            artworktype = txtartworktype_fty1.Text;
            mdivision = txtMdivision1.Text;
            factory = cbbFactory.Text;
            subcon = txtsubcon1.TextBox1.Text;
            spno = txtSPNO.Text;
            style = txtstyle1.Text;
            orderby = cbbOrderBy.Text;
            
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- Sql Command --
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"Select 
a.mdivisionid
,a.FactoryId
,a.id
,a.IssueDate
,a.ArtworkTypeId
,b.ArtworkPoid
,d.LocalSuppID+'-'+(select s.Abb from LocalSupp s WITH (NOLOCK) where s.id = d.LocalSuppID) localsupp
,b.Orderid
,c.StyleID
,b.BundleNo
,b.ArtworkID
,RTrim(b.PatternCode)+'-'+b.PatternDesc pattern
,b.Qty
,d.delivery
from farmin a WITH (NOLOCK) 
inner join farmin_detail b WITH (NOLOCK) on b.ID = a.Id
inner join Orders c WITH (NOLOCK) on c.ID = b.Orderid
inner join ArtworkPO d WITH (NOLOCK) on d.ID = b.ArtworkPoid
where 1=1"));
            #endregion

            System.Data.SqlClient.SqlParameter sp_issuedate1 = new System.Data.SqlClient.SqlParameter();
            sp_issuedate1.ParameterName = "@issuedate1";

            System.Data.SqlClient.SqlParameter sp_issuedate2 = new System.Data.SqlClient.SqlParameter();
            sp_issuedate2.ParameterName = "@issuedate2";

            System.Data.SqlClient.SqlParameter sp_artworktype = new System.Data.SqlClient.SqlParameter();
            sp_artworktype.ParameterName = "@artworktype";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

            System.Data.SqlClient.SqlParameter sp_spno = new System.Data.SqlClient.SqlParameter();
            sp_spno.ParameterName = "@spno";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();


            if (!MyUtility.Check.Empty(issuedate1))
            {
                sqlCmd.Append(" and a.issuedate >= @issuedate1");
                sp_issuedate1.Value = issuedate1;
                cmds.Add(sp_issuedate1);
            }

            if (!MyUtility.Check.Empty(issuedate2))
            {
                sqlCmd.Append(" and a.issuedate <= @issuedate2");
                sp_issuedate2.Value = issuedate2;
                cmds.Add(sp_issuedate2);
            }

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
                sqlCmd.Append(" and d.localsuppid = @subcon");
                sp_subcon.Value = subcon;
                cmds.Add(sp_subcon);
            }
            if (!MyUtility.Check.Empty(spno))
            {
                sqlCmd.Append(" and c.id = @spno ");
                sp_spno.Value = spno;
                cmds.Add(sp_spno);
            }
            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(" and c.styleid = @style");
                sp_style.Value = style;
                cmds.Add(sp_style);
            }

            if (orderby.ToUpper() == "ISSUE DATE")
                sqlCmd.Append(" order by a.mdivisionid, a.FactoryId, a.ID, a.issuedate ");
            else
                sqlCmd.Append(" order by a.mdivisionid, a.FactoryId, a.ID, d.localsuppid ");

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

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R03.xltx");
            return true;
        }
    }
}
