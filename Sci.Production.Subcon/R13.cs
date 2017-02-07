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
    public partial class R13 : Sci.Win.Tems.PrintForm
    {
        string artworktype, factory, subcon, mdivision;
        DateTime? issueDate1, issueDate2, approveDate1, approveDate2;
        DataTable printData;

        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory", out factory);
            MyUtility.Tool.SetupCombox(cbbFactory, 1, factory);
            cbbFactory.Text = Sci.Env.User.Factory;
            txtMdivision1.Text = Sci.Env.User.Keyword;

            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            int year = DateTime.Today.Year;
            this.dateRange1.Value1 = DateTime.Today.AddDays(-day + 1);
            this.dateRange1.Value2 = DateTime.Now;
            this.dateRange2.Value1 = DateTime.Today.AddDays(-day + 1);
            this.dateRange2.Value2 = DateTime.Today.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);
            
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) || MyUtility.Check.Empty(dateRange2.Value1))
            {
                MyUtility.Msg.WarningBox("< Issue Date > & < Approve Date > can't empty!!");
                return false;
            }
            issueDate1 = dateRange1.Value1;
            issueDate2 = dateRange1.Value2;
            approveDate1 = dateRange2.Value1;
            approveDate2 = dateRange2.Value2;
            artworktype = txtartworktype_fty1.Text;
            mdivision = txtMdivision1.Text;
            factory = cbbFactory.Text;
            subcon = txtsubcon1.TextBox1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            
            StringBuilder sqlCmd = new StringBuilder();
            if (this.checkBox1.Checked)
            {
                #region -- Summary Sql Command --
                sqlCmd.Append(string.Format(@"Select a.MDivisionID
		                                            ,a.FactoryID
                                                    ,a.LocalSuppID
                                                    ,b.Abb
		                                            ,a.ArtworkTypeID
                                                    ,a.CurrencyID
                                                    ,sum(a.Amount + a.Vat) Amt
                                                    ,a.PayTermID+'-' +(select Name from PayTerm where id = a.paytermid) payterm
                                            from ArtworkAP a
                                            left join LocalSupp b on a.LocalSuppID=b.ID
                                            where  a.issuedate between '{0}' and '{1}'
                                                    and a.apvdate between '{2}' and '{3}'"
                                                  ,Convert.ToDateTime(issueDate1).ToString("d"), Convert.ToDateTime(issueDate2).ToString("d")
                                                  ,Convert.ToDateTime(approveDate1).ToString("d"), Convert.ToDateTime(approveDate2).ToString("d")));
                #endregion
            }
            else
            {
                #region -- List Sql Command --
                sqlCmd.Append(string.Format(@"Select a.MDivisionID
                                                    ,a.FactoryID
                                                    ,a.LocalSuppID
                                                    ,b.abb
                                                    ,a.id
                                                    ,a.IssueDate
                                                    ,a.ApvDate
                                                    ,a.CurrencyID
                                                    ,a.Amount+a.Vat apAmount
                                                    ,a.ArtworkTypeID
                                                    ,c.ArtworkPoID
                                                    ,c.OrderID
                                                    ,d.StyleID
                                                    ,dbo.getPass1(e.Handle) pohandle
                                                    ,f.Amount poAmount
                                                    ,e.IssueDate
                                                    ,a.InvNo
                                             from ArtworkAP a 
                                             left join LocalSupp b on a.LocalSuppID=b.ID
                                             inner join ArtworkAP_Detail c on a.Id=c.ID
                                             left join Orders d on c.OrderID=d.id
                                             left join ArtworkPO e on e.id=c.ArtworkPoID
                                             inner join ArtworkPO_Detail f on c.ArtworkPoID=f.id and c.ArtworkPo_DetailUkey=f.Ukey
                                            where  a.issuedate between '{0}' and '{1}'
                                            and a.apvdate between '{2}' and '{3}'"
                                                  , Convert.ToDateTime(issueDate1).ToString("d"), Convert.ToDateTime(issueDate2).ToString("d")
                                                  , Convert.ToDateTime(approveDate1).ToString("d"), Convert.ToDateTime(approveDate2).ToString("d")));
                #endregion
            }
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

            if (this.checkBox1.Checked)
            {
                sqlCmd.Append(@" group by a.MDivisionID, a.FactoryID, a.LocalSuppID, b.Abb, a.ArtworkTypeID, a.CurrencyID, a.PayTermID
                                 order by a.ArtworkTypeID, a.currencyid, a.factoryid, a.LocalSuppID");
            }
            else
            {
                sqlCmd.Append(@" order by a.Currencyid, a.LocalSuppId,a.Id");

            }

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

            if (checkBox1.Checked)
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R13_SubconPaymentSummary.xltx", 2);
            else
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R13_SubconPaymentList.xltx", 2);
            return true;
        }
    }
}
