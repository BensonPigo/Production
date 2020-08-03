using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private string artworktype;
        private string factory;
        private string subcon;
        private string spno;
        private string style;
        private string orderby;
        private string mdivision;
        private string brandID;
        private DateTime? issuedate1;
        private DateTime? issuedate2;
        private DataTable printData;

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Issue date,Supplier");
            this.comboOrderBy.SelectedIndex = 0;
            this.txtMdivisionM.Text = Env.User.Keyword;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // if (MyUtility.Check.Empty(dateRange1.Value1))
            // {
            //    MyUtility.Msg.WarningBox("Issue Date can't empty!!");
            //    return false;
            // }
            this.issuedate1 = this.dateIssueDate.Value1;
            this.issuedate2 = this.dateIssueDate.Value2;

            // IssueDate 為必輸條件
            if (MyUtility.Check.Empty(this.issuedate1) || MyUtility.Check.Empty(this.issuedate2))
            {
                MyUtility.Msg.InfoBox("Issue Date can't empty!!");
                return false;
            }

            this.artworktype = this.txtartworktype_ftyArtworkType.Text;
            this.mdivision = this.txtMdivisionM.Text;
            this.factory = this.comboFactory.Text;
            this.subcon = this.txtsubconSupplier.TextBox1.Text;
            this.spno = this.txtSPNO.Text;
            this.style = this.txtstyle.Text;
            this.orderby = this.comboOrderBy.Text;
            this.brandID = this.txtbrand.Text;

            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"Select a.mdivisionid
                                        ,a.FactoryId
                                        ,a.ID
                                        ,a.IssueDate
                                        ,a.LocalSuppID+'-'+(select abb from localsupp WITH (NOLOCK) where id = a.localsuppid) as localsupp
                                        ,a.ArtworkTypeID
                                        ,a.Delivery
                                        ,a.InternalRemark
                                        ,b.OrderID
                                        ,c.BrandID
                                        ,c.SewInLine
                                        ,c.SciDelivery
                                        ,c.StyleID
                                        ,b.Article
                                        ,b.ArtworkId
                                        ,RTrim(b.PatternCode) + '-' + b.PatternDesc
                                        ,b.SizeCode
                                        ,b.PoQty
                                        ,b.UnitPrice
                                        ,b.PoQty*b.UnitPrice as poamt
                                        ,b.Farmout
                                        ,b.Farmin
                                        ,b.Farmout - b.Farmin as variance
                                        ,b.ApQty
                                        ,ap_balance = isnull(b.PoQty,0) - isnull(b.ApQty,0)
                                        ,ap_amt = (isnull(b.PoQty,0) - isnull(b.ApQty,0)) * b.UnitPrice
                                        ,a.Status
                                        from artworkpo a WITH (NOLOCK) 
                                        inner join  artworkpo_detail b WITH (NOLOCK) on a.id = b.ID
                                        inner join  orders c WITH (NOLOCK) on b.OrderID = c.ID
                                        where 1=1 "));

            if (!MyUtility.Check.Empty(this.issuedate1))
            {
                sqlCmd.Append(string.Format(" and a.issuedate >= '{0}' ", Convert.ToDateTime(this.issuedate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.issuedate2))
            {
                sqlCmd.Append(string.Format(" and a.issuedate <= '{0}' ", Convert.ToDateTime(this.issuedate2).ToString("d")));
            }

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

            System.Data.SqlClient.SqlParameter sp_brandID = new System.Data.SqlClient.SqlParameter();
            sp_brandID.ParameterName = "@brandID";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlCmd.Append(" and a.artworktypeid = @artworktype");
                sp_artworktype.Value = this.artworktype;
                cmds.Add(sp_artworktype);
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and a.factoryid = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.subcon))
            {
                sqlCmd.Append(" and a.localsuppid = @subcon");
                sp_subcon.Value = this.subcon;
                cmds.Add(sp_subcon);
            }

            if (!MyUtility.Check.Empty(this.spno))
            {
                sqlCmd.Append(" and c.id = @spno ");
                sp_spno.Value = this.spno;
                cmds.Add(sp_spno);
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(" and c.styleid = @style");
                sp_style.Value = this.style;
                cmds.Add(sp_style);
            }

            if (!MyUtility.Check.Empty(this.brandID))
            {
                sqlCmd.Append(" and c.BrandID = @brandID");
                sp_brandID.Value = this.brandID;
                cmds.Add(sp_brandID);
            }

            if (this.checkOutstanding.Checked)
            {
                sqlCmd.Append(" and b.Farmin - b.ApQty > 0 and a.status!='Closed' ");
            }

            if (this.orderby.ToUpper() == "ISSUE DATE")
            {
                sqlCmd.Append(" order by a.mdivisionid, a.FactoryId, a.ID, a.issuedate ");
            }
            else
            {
                sqlCmd.Append(" order by a.mdivisionid, a.FactoryId, a.ID, a.localsuppid ");
            }

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

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R01.xltx");
            return true;
        }
    }
}
