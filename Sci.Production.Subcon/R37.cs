using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R37 : Win.Tems.PrintForm
    {
        private string reportType;
        private List<SqlParameter> list;
        private DataTable dtList; private string cmd;
        private DataTable dt; private string cmdDt;
        private DateTime? DebDate1; private DateTime? DebDate2;
        private DateTime? ConDate1; private DateTime? ConDate2;
        private DateTime? SettDate1; private DateTime? SettDate2;
        private string DebitNo1; private string DebitNo2;
        private string handle; private string smr;
        private string fac; private string Pay;

        public R37(ToolStripMenuItem menuitem)
            : base(menuitem)
         {
            this.InitializeComponent();
            DataTable factory = null;
            string sqlcmd = @"select DISTINCT BrandID FROM DBO.Debit WITH (NOLOCK) ";
            DBProxy.Current.Select(string.Empty, sqlcmd, out factory);
            factory.Rows.Add(new string[] { string.Empty });
            factory.DefaultView.Sort = "BrandID";
            this.comboFactory.DataSource = factory;
            this.comboFactory.ValueMember = "BrandID";
            this.comboFactory.DisplayMember = "BrandID";
            this.comboFactory.SelectedIndex = 0;
            this.comboFactory.Text = Env.User.Factory;
            this.comboPaymentSettled.SelectedIndex = 0;
            this.comboReportType.SelectedIndex = 0;
            this.print.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.DebDate1 = this.dateDebitDate.Value1;
            this.DebDate2 = this.dateDebitDate.Value2;
            this.ConDate1 = this.dateConfirmDate.Value1;
            this.ConDate2 = this.dateConfirmDate.Value2;
            this.SettDate1 = this.dateSettledDate.Value1;
            this.SettDate2 = this.dateSettledDate.Value2;
            this.DebitNo1 = this.txtDebitNoStart.Text.ToString();
            this.DebitNo2 = this.txtDebitNoEnd.Text.ToString();
            this.handle = this.txttpeuser_caneditHandle.TextBox1.Text;
            this.smr = this.txttpeuser_caneditSMR.TextBox1.Text;
            this.fac = this.comboFactory.Text.ToString();
            this.Pay = this.comboPaymentSettled.SelectedItem.ToString();

            this.reportType = this.comboReportType.Text;

            this.list = new List<SqlParameter>();
            string sqlWhere = string.Empty;
            string sqlHaving = string.Empty;
            List<string> sqlWheres = new List<string>();

            #region --組WHERE--

            if (!this.dateDebitDate.Value1.Empty())
            {
                sqlWheres.Add("a.Issuedate >= @DebDate1");
                this.list.Add(new SqlParameter("@DebDate1", this.DebDate1));
            }

            if (!this.dateDebitDate.Value2.Empty())
            {
                sqlWheres.Add("a.Issuedate <= @DebDate2");
                this.list.Add(new SqlParameter("@DebDate2", this.DebDate2.Value.AddDays(1).AddSeconds(-1)));
            }

            if (!this.dateConfirmDate.Value1.Empty())
            {
                sqlWheres.Add("a.cfmdate >= @ConDate1");
                this.list.Add(new SqlParameter("@ConDate1", this.ConDate1));
            }

            if (!this.dateConfirmDate.Value2.Empty())
            {
                sqlWheres.Add("a.cfmdate <= @ConDate2");
                this.list.Add(new SqlParameter("@ConDate2", this.ConDate2.Value.AddDays(1).AddSeconds(-1)));
            }

            if (!this.txtDebitNoStart.Text.Empty())
            {
                sqlWheres.Add("a.Id between @DebNo1 and @DebNo2");
                this.list.Add(new SqlParameter("@DebNo1", this.DebitNo1));
                this.list.Add(new SqlParameter("@DebNo2", this.DebitNo2));
            }

            if (!this.txttpeuser_caneditHandle.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.handle = @handle");
                this.list.Add(new SqlParameter("@handle", this.handle));
            }

            if (!this.txttpeuser_caneditSMR.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.smr = @smr");
                this.list.Add(new SqlParameter("@smr", this.smr));
            }

            if (!this.comboFactory.Text.ToString().Empty())
            {
                sqlWheres.Add("a.BrandID  = @factory");
                this.list.Add(new SqlParameter("@factory", this.fac));
            }

            if (this.comboPaymentSettled.Text == "Settled")
            {
                // sqlWheres.Add("ISNULL(IsSettled.Val , 0) = 1");
                sqlWheres.Add(" a.Settled = 'Y' ");
            }

            if (this.comboPaymentSettled.Text == "Not Settled")
            {
                // sqlWheres.Add("ISNULL(IsSettled.Val , 0) = 0");
                sqlWheres.Add(" a.Settled = '' ");
            }

            if (!this.dateSettledDate.Value1.Empty())
            {
                sqlHaving += $@" AND IIF(a.IsSubcon=1,
					IIF(ISNULL(IsSettled.Val , 0) = 1,MaxVoucherDate.VoucherDate,NULL), 
					a.SettleDate
				) >= @SettledDate1 ";
                this.list.Add(new SqlParameter("@SettledDate1", this.SettDate1));
            }

            if (!this.dateSettledDate.Value2.Empty())
            {
                sqlHaving += $@" AND IIF(a.IsSubcon=1,
					IIF(ISNULL(IsSettled.Val , 0) = 1,MaxVoucherDate.VoucherDate,NULL), 
					a.SettleDate
				) <= @SettledDate2";
                this.list.Add(new SqlParameter("@SettledDate2", this.SettDate2.Value.AddDays(1).AddSeconds(-1)));
            }
            #endregion

            sqlWhere = string.Join(" and ", sqlWheres);

            #region --撈List資料--
            this.cmd = string.Format(@"
SELECT distinct a.ID
, [Subcon DBC]=Iif(a.IsSubcon=1,'Y','N')
,a.Issuedate
,a.BrandID
,title='Debit Memo List (Taipei)'
,a.SendFrom
,a.Attn
,a.CC
,a.subject
,a.Handle + '-' + ISNULL(vs1.Name_Extno ,'') [Handle]
,a.SMR +'-'+ ISNULL(vs2.Name_Extno,'') [SMR]
,a.CurrencyID
,a.Amount
,a.Received
,a.Cfm+ '-' + ISNULL(vs3.Name_Extno,'') [cfm]
,a.CfmDate
,[Voucher No.]=IIF(a.IsSubcon=1,
				(	SELECT (
						STUFF(
						(
							SELECT CHAR(10)+ ds.VoucherID 
							FROM debit_schedule ds WITH (NOLOCK) 
							LEFT JOIN FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
							WHERE  ISNULL(ds.VoucherID,'')<>'' AND v.VoucherDate IS NOT NULL AND ds.ID=a.ID
							ORDER BY v.VoucherDate
							FOR XML PATH('')
						), 1, 1, '') 
					)
				), 
				a.VoucherFactory)
,[Voucher Date]= IIF(a.IsSubcon=1,
					(	SELECT (
						STUFF(
						(
							SELECT CHAR(10) + CONVERT(varchar, v.VoucherDate, 111)
							FROM debit_schedule ds WITH (NOLOCK) 
							LEFT JOIN FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
							WHERE  ISNULL(ds.VoucherID,'')<>'' AND v.VoucherDate  IS NOT NULL AND ds.ID=a.ID
							ORDER BY v.VoucherDate
							FOR XML PATH('')
						), 1, 1, '') 
					)
				),
					Cast((SELECT VoucherDate FROM SciFMS_Voucher WHERE ID=(SELECT VoucherFactory FROM Debit WHERE ID=a.ID)) as varchar)
				)
,[Settled Date]=IIF(a.IsSubcon=1,
					IIF(ISNULL(IsSettled.Val , 0) = 1,MaxVoucherDate.VoucherDate,NULL), 
					a.SettleDate
				)
FROM  Debit a WITH (NOLOCK) 
outer apply (select * from dbo.View_ShowName_TPE vs where vs.id = a.Handle ) vs1
outer apply (select * from dbo.View_ShowName_TPE vs where vs.id = a.SMR ) vs2
outer apply (select * from dbo.View_ShowName_TPE vs where vs.id = a.cfm ) vs3  
outer apply ( 
	SELECT [VoucherDate]=MAX(v.VoucherDate)
	FROM Debit_Schedule ds WITH (NOLOCK) 
	left join FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
	WHERE  ISNULL(ds.VoucherID,'')!=''
			AND v.VoucherDate IS NOT NULL
			AND ds.ID=a.ID 
) MaxVoucherDate
OUTER APPLY(
	SELECT [Amount]=Sum(Amount)
	FROM Debit_Schedule
	WHERE ID=a.ID AND VoucherID <> ''
)Debit_Schedule_Amount
OUTER APPLY(
	SELECT [Val]=IIF(a.IsSubcon=1,
	(
		SELECT IIF(Sum(ds.Amount) = (ld.Amount+ld.Tax) ,1 ,0 )
		FROM LocalDebit ld
		LEFT JOIN Debit_Schedule ds ON ds.ID=ld.ID 
		LEFT JOIN FinanceEn.dbo.Voucher as v2 on v2.id = ds.VoucherID
		WHERE ld.ID = a.ID AND ISNULL(ds.VoucherID,'') <> '' AND v2.VoucherDate IS NOT NULL
		GROUP BY ld.Amount ,ld.Tax
	),
	(SELECT IIF(a.VoucherFactory != '' ,1 ,0)
	))
)IsSettled
where a.type='F' and 
" + sqlWhere + ' ' + sqlHaving);
            #endregion

            #region --撈Detail List資料--
            this.cmdDt = string.Format(@"
SELECT distinct a.ID
, [Subcon DBC]=Iif(a.IsSubcon=1,'Y','N')
,a.Issuedate
,a.BrandID
,title='Debit Memo List (Taipei)'
,a.SendFrom
,a.Attn
,a.CC
,a.subject
,a.Handle + '-' + ISNULL(vs1.Name_Extno ,'') [Handle]
,a.SMR +'-'+ ISNULL(vs2.Name_Extno,'') [SMR]
,a.CurrencyID
,a.Amount
,a.Received
,a.Cfm+ '-' + ISNULL(vs3.Name_Extno,'') [cfm]
,a.CfmDate
,[Voucher No.]=IIF(a.IsSubcon=1,
				(	SELECT (
						STUFF(
						(
							SELECT CHAR(10)+ ds.VoucherID 
							FROM debit_schedule ds WITH (NOLOCK) 
							LEFT JOIN FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
							WHERE  ISNULL(ds.VoucherID,'')<>'' AND v.VoucherDate IS NOT NULL AND ds.ID=a.ID
							ORDER BY v.VoucherDate
							FOR XML PATH('')
						), 1, 1, '') 
					)
				), 
				a.VoucherFactory)
,[Voucher Date]= IIF(a.IsSubcon=1,
					(	SELECT (
						STUFF(
						(
							SELECT CHAR(10) + CONVERT(varchar, v.VoucherDate, 111)
							FROM debit_schedule ds WITH (NOLOCK) 
							LEFT JOIN FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
							WHERE  ISNULL(ds.VoucherID,'')<>'' AND v.VoucherDate IS NOT NULL AND ds.ID=a.ID
							ORDER BY v.VoucherDate
							FOR XML PATH('')
						), 1, 1, '') 
					)
				),
					Cast((SELECT VoucherDate FROM SciFMS_Voucher WHERE ID=(SELECT VoucherFactory FROM Debit WHERE ID=a.ID)) as varchar)
				)
,[Settled Date]=IIF(a.IsSubcon=1,
					IIF(ISNULL(IsSettled.Val , 0) = 1,MaxVoucherDate.VoucherDate,NULL), 
					a.SettleDate
				)
,dd.OrderID
,dd.Qty
,dd.Amount
,dd.reasonid+'-'+dd.ReasonNM [reasonid]
FROM  Debit a WITH (NOLOCK) 
left join Debit_Detail dd on a.ID = dd.ID
outer apply (select * from dbo.View_ShowName_TPE vs where vs.id = a.Handle ) vs1
outer apply (select * from dbo.View_ShowName_TPE vs where vs.id = a.SMR ) vs2  
outer apply (select * from dbo.View_ShowName_TPE vs where vs.id = a.cfm ) vs3 
outer apply ( 
	SELECT [VoucherDate]=MAX(v.VoucherDate)
	FROM Debit_Schedule ds WITH (NOLOCK) 
	left join FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
	WHERE  ISNULL(ds.VoucherID,'')!=''
			AND v.VoucherDate IS NOT NULL
			AND ds.ID=a.ID 
) MaxVoucherDate
OUTER APPLY(
	SELECT [Amount]=Sum(Amount)
	FROM Debit_Schedule
	WHERE ID=a.ID AND VoucherID <> ''
)Debit_Schedule_Amount
OUTER APPLY(
	SELECT [Val]=IIF(a.IsSubcon=1,
	(
		SELECT IIF(Sum(ds.Amount) = (ld.Amount+ld.Tax) ,1 ,0 )
		FROM LocalDebit ld
		LEFT JOIN Debit_Schedule ds ON ds.ID = ld.ID 
		LEFT JOIN FinanceEn.dbo.Voucher as v2 on v2.id = ds.VoucherID
		WHERE ld.ID = a.ID AND ISNULL(ds.VoucherID,'') <> '' AND v2.VoucherDate IS NOT NULL
		GROUP BY ld.Amount ,ld.Tax
	),
	(SELECT IIF(a.VoucherFactory != '' ,1 ,0)
	))
)IsSettled

where a.type='F' and " + sqlWhere + ' ' + sqlHaving);
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;

            switch (this.reportType)
            {
                case "List":
                    res = DBProxy.Current.Select(string.Empty, this.cmd, this.list, out this.dtList);
                    break;
                case "Detail List":
                    res = DBProxy.Current.Select(string.Empty, this.cmdDt, this.list, out this.dt);
                    break;
                default:
                    res = DBProxy.Current.Select(string.Empty, this.cmd, this.list, out this.dtList); // 預設
                    break;
            }

            return res;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            switch (this.reportType)
            {
                case "List":
                    if (this.dtList == null || this.dtList.Rows.Count == 0)
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }

                    break;
                case "Detail List":
                    if (this.dt == null || this.dt.Rows.Count == 0)
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }

                    break;
                default:
                    if (this.dtList == null || this.dtList.Rows.Count == 0) // 預設
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }

                    break;
            }

            if ("List".EqualString(this.comboReportType.Text))
            {
                string d1 = MyUtility.Check.Empty(this.DebDate1) ? string.Empty : Convert.ToDateTime(this.DebDate1).ToString("yyyy/MM/dd");
                string d2 = MyUtility.Check.Empty(this.DebDate2) ? string.Empty : Convert.ToDateTime(this.DebDate2).ToString("yyyy/MM/dd");
                string d3 = MyUtility.Check.Empty(this.ConDate1) ? string.Empty : Convert.ToDateTime(this.ConDate1).ToString("yyyy/MM/dd");
                string d4 = MyUtility.Check.Empty(this.ConDate2) ? string.Empty : Convert.ToDateTime(this.ConDate2).ToString("yyyy/MM/dd");
                string d5 = MyUtility.Check.Empty(this.SettDate1) ? string.Empty : Convert.ToDateTime(this.SettDate1).ToString("yyyy/MM/dd");
                string d6 = MyUtility.Check.Empty(this.SettDate2) ? string.Empty : Convert.ToDateTime(this.SettDate2).ToString("yyyy/MM/dd");
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R37_List.xltx");
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
                objSheets.Cells[2, 2] = d1 + " ~ " + d2;
                objSheets.Cells[2, 4] = d3 + " ~ " + d4;
                objSheets.Cells[2, 7] = d5 + " ~ " + d6;
                objSheets.Cells[2, 9] = this.DebitNo1 + " ~ " + this.DebitNo2;
                objSheets.Cells[2, 11] = this.handle;
                objSheets.Cells[2, 13] = this.smr;
                objSheets.Cells[2, 17] = this.fac;
                objSheets.Cells[2, 19] = this.Pay;
                MyUtility.Excel.CopyToXls(this.dtList, string.Empty, "Subcon_R37_List.xltx", 3, true, null, objApp);

                Marshal.ReleaseComObject(objSheets);
                return true;
            }
            else if ("Detail List".EqualString(this.comboReportType.Text))
            {
                string d1 = MyUtility.Check.Empty(this.DebDate1) ? string.Empty : Convert.ToDateTime(this.DebDate1).ToString("yyyy/MM/dd");
                string d2 = MyUtility.Check.Empty(this.DebDate2) ? string.Empty : Convert.ToDateTime(this.DebDate2).ToString("yyyy/MM/dd");
                string d3 = MyUtility.Check.Empty(this.ConDate1) ? string.Empty : Convert.ToDateTime(this.ConDate1).ToString("yyyy/MM/dd");
                string d4 = MyUtility.Check.Empty(this.ConDate2) ? string.Empty : Convert.ToDateTime(this.ConDate2).ToString("yyyy/MM/dd");
                string d5 = MyUtility.Check.Empty(this.SettDate1) ? string.Empty : Convert.ToDateTime(this.SettDate1).ToString("yyyy/MM/dd");
                string d6 = MyUtility.Check.Empty(this.SettDate2) ? string.Empty : Convert.ToDateTime(this.SettDate2).ToString("yyyy/MM/dd");
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R37_DetailList.xltx");
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
                objSheets.Cells[2, 2] = d1 + " ~ " + d2;
                objSheets.Cells[2, 4] = d3 + " ~ " + d4;
                objSheets.Cells[2, 7] = d5 + " ~ " + d6;
                objSheets.Cells[2, 9] = this.DebitNo1 + " ~ " + this.DebitNo2;
                objSheets.Cells[2, 11] = this.handle;
                objSheets.Cells[2, 13] = this.smr;
                objSheets.Cells[2, 17] = this.fac;
                objSheets.Cells[2, 19] = this.Pay;
                MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Subcon_R37_DetailList.xltx", 3, true, null, objApp);

                Marshal.ReleaseComObject(objSheets);
                return true;
            }

            return true;
        }
    }
}
