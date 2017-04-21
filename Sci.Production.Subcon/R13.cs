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
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            txtMdivisionM.Text = Sci.Env.User.Keyword;

            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            int year = DateTime.Today.Year;
            this.dateIssueDate.Value1 = DateTime.Today.AddDays(-day + 1);
            this.dateIssueDate.Value2 = DateTime.Now;
            this.dateApproveDate.Value1 = DateTime.Today.AddDays(-day + 1);
            this.dateApproveDate.Value2 = DateTime.Today.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);
            
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateIssueDate.Value1) && MyUtility.Check.Empty(dateIssueDate.Value2)|| MyUtility.Check.Empty(dateApproveDate.Value1)  && MyUtility.Check.Empty(dateApproveDate.Value2))
            {
                MyUtility.Msg.WarningBox("< Issue Date > & < Approve Date > can't empty!!");
                return false;
            }
            issueDate1 = dateIssueDate.Value1;
            issueDate2 = dateIssueDate.Value2;
            approveDate1 = dateApproveDate.Value1;
            approveDate2 = dateApproveDate.Value2;
            artworktype = txtartworktype_ftyArtworkType.Text;
            mdivision = txtMdivisionM.Text;
            factory = comboFactory.Text;
            subcon = txtsubconSupplier.TextBox1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            
            StringBuilder sqlCmd = new StringBuilder();
            if (this.checkSummary.Checked)
            {
                #region -- Summary Sql Command --
                sqlCmd.Append(string.Format(@"Select a.MDivisionID
		                                            ,a.FactoryID
                                                    ,a.LocalSuppID
                                                    ,b.Abb
		                                            ,a.ArtworkTypeID
                                                    ,a.CurrencyID
                                                    ,sum(a.Amount + a.Vat) Amt
                                                    ,a.PayTermID+'-' +(select Name from PayTerm WITH (NOLOCK) where id = a.paytermid) payterm
                                            from ArtworkAP a WITH (NOLOCK) 
                                            left join LocalSupp b WITH (NOLOCK) on a.LocalSuppID=b.ID
                                            where 1=1"));
                #endregion
            }
            else
            {
                #region -- List Sql Command --
                sqlCmd.Append(string.Format(@"Select distinct a.MDivisionID
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
                                                    ,[poAmount]=SUM(f.Amount) OVER (PARTITION BY c.ArtworkPoID)
                                                    ,e.IssueDate
                                                    ,a.InvNo
                                             from ArtworkAP a WITH (NOLOCK) 
                                             left join LocalSupp b WITH (NOLOCK) on a.LocalSuppID=b.ID
                                             inner join ArtworkAP_Detail c WITH (NOLOCK) on a.Id=c.ID
                                             left join Orders d WITH (NOLOCK) on c.OrderID=d.id
                                             left join ArtworkPO e WITH (NOLOCK) on e.id=c.ArtworkPoID
                                             inner join ArtworkPO_Detail f WITH (NOLOCK) on c.ArtworkPoID=f.id and c.ArtworkPo_DetailUkey=f.Ukey
                                            where  1=1"));
                #endregion
            }
            System.Data.SqlClient.SqlParameter sp_issueDate1 = new System.Data.SqlClient.SqlParameter();
            sp_issueDate1.ParameterName = "@issueDate1";

            System.Data.SqlClient.SqlParameter sp_issueDate2 = new System.Data.SqlClient.SqlParameter();
            sp_issueDate2.ParameterName = "@issueDate2";

            System.Data.SqlClient.SqlParameter sp_approveDate1 = new System.Data.SqlClient.SqlParameter();
            sp_approveDate1.ParameterName = "@approveDate1";

            System.Data.SqlClient.SqlParameter sp_approveDate2 = new System.Data.SqlClient.SqlParameter();
            sp_approveDate2.ParameterName = "@approveDate2";

            System.Data.SqlClient.SqlParameter sp_artworktype = new System.Data.SqlClient.SqlParameter();
            sp_artworktype.ParameterName = "@artworktype";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            if (!MyUtility.Check.Empty(issueDate1))
            {
                sqlCmd.Append(" and a.issuedate >= @issueDate1");
                sp_issueDate1.Value = issueDate1;
                cmds.Add(sp_issueDate1);
            }

            if (!MyUtility.Check.Empty(issueDate2))
            {
                sqlCmd.Append(" and a.issuedate <= @issueDate2");
                sp_issueDate2.Value = issueDate2;
                cmds.Add(sp_issueDate2);
            }
            if (!MyUtility.Check.Empty(approveDate1))
            {
                sqlCmd.Append(" and a.apvdate >= @approveDate1");
                sp_approveDate1.Value = approveDate1;
                cmds.Add(sp_approveDate1);
            }

            if (!MyUtility.Check.Empty(approveDate2))
            {
                sqlCmd.Append(" and a.apvdate <= @approveDate2");
                sp_approveDate2.Value = approveDate2;
                cmds.Add(sp_approveDate2);
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
                sqlCmd.Append(" and a.localsuppid = @subcon");
                sp_subcon.Value = subcon;
                cmds.Add(sp_subcon);
            }

            if (this.checkSummary.Checked)
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

            if (checkSummary.Checked)
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R13_SubconPaymentSummary.xltx", 2);
            else
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R13_SubconPaymentList.xltx", 2);
            return true;
        }
    }
}
