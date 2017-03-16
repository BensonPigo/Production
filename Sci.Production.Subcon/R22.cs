﻿using System;
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
    public partial class R22 : Sci.Win.Tems.PrintForm
    {
        string category, factory, subcon, mdivision;
        DateTime? issueDate1, issueDate2, approveDate1, approveDate2;
        DataTable printData;

        public R22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
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
            category = txtartworktype_fty1.Text;
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
                sqlCmd.Append(string.Format(@"Select distinct a.MDivisionID
                                                    ,a.FactoryId
                                                    ,a.LocalSuppID
	                                                ,d.Abb
	                                                ,a.Category
	                                                ,a.CurrencyID
	                                                ,sum(a.Amount + a.Vat) Amt
	                                                ,a.PaytermID+'-' +(select Name from PayTerm WITH (NOLOCK) where id = a.paytermid) payterm
                                             from LocalAP a WITH (NOLOCK) 
                                             left join LocalSupp d WITH (NOLOCK) on a.LocalSuppID=d.ID
                                             where  a.issuedate between '{0}' and '{1}'
                                                    and a.apvdate between '{2}' and '{3}'"
                                                     , Convert.ToDateTime(issueDate1).ToString("d"), Convert.ToDateTime(issueDate2).ToString("d")
                                                     , Convert.ToDateTime(approveDate1).ToString("d"), Convert.ToDateTime(approveDate2).ToString("d")));
                #endregion
            }
            else
            {
                #region -- List Sql Command --
                sqlCmd.Append(string.Format(@"Select distinct a.MDivisionID
                                                     ,c.FactoryID
	                                                 ,a.FactoryId
	                                                 ,a.LocalSuppID
	                                                 ,d.Abb
	                                                 ,a.id
	                                                 ,a.IssueDate
	                                                 ,a.ApvDate
	                                                 ,vs1.Name_Extno Handle
	                                                 ,a.CurrencyID
	                                                 ,a.Amount+a.Vat APAmount
	                                                 ,a.Category
	                                                 ,b.LocalPoId
	                                                 ,b.OrderId
 	                                                 ,b.OldSeq1
	                                                 ,b.OldSeq2                                                           
	                                                 ,b.Refno
	                                                 ,dbo.getItemDesc(a.Category,b.Refno) Description
	                                                 ,b.ThreadColorID
	                                                 ,b.UnitID
	                                                 ,b.Price
	                                                 ,b.Qty
                                                     ,b.price*b.qty amount
                                                     ,e.Amount+e.vat poamt
	                                                 ,vs2.Name POHandle
	                                                 ,e.IssueDate
	                                                 ,a.InvNo
                                            from LocalAP a WITH (NOLOCK) 
                                            inner join LocalAP_Detail b WITH (NOLOCK) on b.id=a.id
                                            left join Orders c WITH (NOLOCK) on b.OrderId=c.ID
                                            left join localsupp d WITH (NOLOCK) on a.LocalSuppID=d.ID
                                            left join localpo e WITH (NOLOCK) on b.LocalPoId=e.Id 
                                            outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
											outer apply (
                                                (select name = concat(name, ' Ext.', ExtNo)
                                                from TPEPass1
                                                where id = (select PoHandle from Po where id = b.OrderId))
                                            ) vs2
                                            where  a.issuedate between '{0}' and '{1}'
                                             and a.apvdate between '{2}' and '{3}'"
                    , Convert.ToDateTime(issueDate1).ToString("d"), Convert.ToDateTime(issueDate2).ToString("d")
                    , Convert.ToDateTime(approveDate1).ToString("d"), Convert.ToDateTime(approveDate2).ToString("d")));
                #endregion
            }
            System.Data.SqlClient.SqlParameter sp_category = new System.Data.SqlClient.SqlParameter();
            sp_category.ParameterName = "@category";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

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

            if (this.checkBox1.Checked)
            {
                sqlCmd.Append(@" group by a.MDivisionID, a.FactoryID, a.LocalSuppID, d.Abb, a.Category, a.CurrencyID, a.PayTermID
                                order by a.category, a.currencyid, a.factoryid, a.LocalSuppID");
            }
            else
            {
                sqlCmd.Append(@" order by a.Currencyid, a.LocalSuppId, a.Id");
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
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R22_LocalPaymentSummary.xltx", 2);
            else
                MyUtility.Excel.CopyToXls(printData, "", "Subcon_R22_LocalPaymentList.xltx", 2);
            return true;
        }
    }
}
