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
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        string artworktype, factory, subcon, poid, style, mdivision, bundleno1,bundleno2;
        DateTime? farmoutdate1, farmoutdate2, scidelivery1,scidelivery2;
        DataTable printData;

        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory", out factory);
            MyUtility.Tool.SetupCombox(cbbFactory, 1, factory);
            cbbFactory.Text = Sci.Env.User.Factory;
            txtMdivision1.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Farm Out Date can't empty!!");
                return false;
            }
            farmoutdate1 = dateRange1.Value1;
            farmoutdate2 = dateRange1.Value2;
            bundleno1 = txtBundleno1.Text;
            bundleno2 = txtBundleno2.Text;
            scidelivery1 = dateRange2.Value1;
            scidelivery2 = dateRange2.Value2;

            artworktype = txtartworktype_fty1.Text;
            mdivision = txtMdivision1.Text;
            factory = cbbFactory.Text;
            subcon = txtsubcon1.TextBox1.Text;
            poid = txtSPNO.Text;
            style = txtstyle1.Text;
            
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- Sql Command --
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"Select 
a.MDivisionID
,a.id
,c.POID
,b.Orderid
,c.StyleID
,d.Article
,a.artworktypeid
,f.LocalSuppID+'-'+(select abb from LocalSupp where id = f.LocalSuppID) supplier
,a.FactoryId
,b.ArtworkPoid
,e.BundleGroup
,b.BundleNo
,d.Colorid
,d.Sizecode
,b.Qty
,a.IssueDate farmoutDate
,(select max(farmin.IssueDate) from FarmIn inner join FarmIn_Detail on farmin.ID = FarmIn_Detail.ID where farmin.Status = 'Confirmed' and FarmIn_Detail.ArtworkPo_DetailUkey = b.ArtworkPo_DetailUkey) farminDate
from farmout a 
inner join farmout_detail b on a.id = b.id
inner join orders c on b.orderid = c.id
inner join ArtworkPO f on f.ID = b.ArtworkPoid
left join ( bundle d inner join Bundle_Detail e on e.id = d.ID) on e.BundleNo = b.BundleNo
where a.Status = 'Confirmed' and a.issuedate between '{0}' and '{1}'
", Convert.ToDateTime(farmoutdate1).ToString("d"), Convert.ToDateTime(farmoutdate2).ToString("d")));
            #endregion

            System.Data.SqlClient.SqlParameter sp_artworktype = new System.Data.SqlClient.SqlParameter();
            sp_artworktype.ParameterName = "@artworktype";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

            System.Data.SqlClient.SqlParameter sp_poid = new System.Data.SqlClient.SqlParameter();
            sp_poid.ParameterName = "@poid";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            System.Data.SqlClient.SqlParameter sp_bundleno1 = new System.Data.SqlClient.SqlParameter();
            sp_bundleno1.ParameterName = "@bundleno1";

            System.Data.SqlClient.SqlParameter sp_bundleno2 = new System.Data.SqlClient.SqlParameter();
            sp_bundleno2.ParameterName = "@bundleno2";

            System.Data.SqlClient.SqlParameter sp_scidelivery1 = new System.Data.SqlClient.SqlParameter();
            sp_scidelivery1.ParameterName = "@scidelivery1";

            System.Data.SqlClient.SqlParameter sp_scidelivery2 = new System.Data.SqlClient.SqlParameter();
            sp_scidelivery2.ParameterName = "@scidelivery2";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            if (!MyUtility.Check.Empty(bundleno1))
            {
                sqlCmd.Append(" and b.bundleno >= @bundleno1");
                sp_bundleno1.Value = bundleno1;
                cmds.Add(sp_bundleno1);
            }

            if (!MyUtility.Check.Empty(bundleno2))
            {
                sqlCmd.Append(" and b.bundleno <= @bundleno2");
                sp_bundleno2.Value = bundleno2;
                cmds.Add(sp_bundleno2);
            }

            if (!MyUtility.Check.Empty(scidelivery1))
            {
                sqlCmd.Append(" and c.scidelivery between @scidelivery1 and @scidelivery2");
                sp_scidelivery1.Value = scidelivery1;
                cmds.Add(sp_scidelivery1);
                sp_scidelivery2.Value = scidelivery2;
                cmds.Add(sp_scidelivery2);
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
                sqlCmd.Append(" and f.localsuppid = @subcon");
                sp_subcon.Value = subcon;
                cmds.Add(sp_subcon);
            }
            if (!MyUtility.Check.Empty(poid))
            {
                sqlCmd.Append(" and c.poid = @poid ");
                sp_poid.Value = poid;
                cmds.Add(sp_poid);
            }
            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(" and c.styleid = @style");
                sp_style.Value = style;
                cmds.Add(sp_style);
            }

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

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R06.xltx",2);
            return true;
        }
    }
}
