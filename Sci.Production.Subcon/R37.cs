using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R37 : Sci.Win.Tems.PrintForm
    {
        string ReportType;
        List<SqlParameter> list;
        DataTable dtList; string cmd;
        DataTable dt; string cmdDt;
        DateTime? DebDate1; DateTime? DebDate2;
        DateTime? ConDate1; DateTime? ConDate2;
        DateTime? SettDate1; DateTime? SettDate2;
        string DebitNo1; string DebitNo2;
        string handle; string smr;
        string fac; string Pay;
        public R37(ToolStripMenuItem menuitem)
            : base(menuitem) 
         {
            InitializeComponent();
            DataTable factory = null;
            string sqlcmd = (@"select DISTINCT BrandID FROM DBO.Debit WITH (NOLOCK) ");
            DBProxy.Current.Select("", sqlcmd, out factory);
            factory.Rows.Add(new string[] { "" });
            factory.DefaultView.Sort = "BrandID";
            this.comboFactory.DataSource = factory;
            this.comboFactory.ValueMember = "BrandID";
            this.comboFactory.DisplayMember = "BrandID";
            this.comboFactory.SelectedIndex = 0;
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.comboPaymentSettled.SelectedIndex = 0;
            this.comboReportType.SelectedIndex = 0;
            print.Enabled = false;
        }

        protected override bool ValidateInput()
        {

            DebDate1 = dateDebitDate.Value1;
            DebDate2 = dateDebitDate.Value2;
            ConDate1 = dateConfirmDate.Value1;
            ConDate2 = dateConfirmDate.Value2;
            SettDate1 = dateSettledDate.Value1;
            SettDate2 = dateSettledDate.Value2;
            DebitNo1 = txtDebitNoStart.Text.ToString();
            DebitNo2 = txtDebitNoEnd.Text.ToString();
            handle = txttpeuser_caneditHandle.TextBox1.Text;
            smr = txttpeuser_caneditSMR.TextBox1.Text;
            fac = comboFactory.Text.ToString();
            Pay = comboPaymentSettled.SelectedItem.ToString();

            ReportType = this.comboReportType.Text;

            list = new List<SqlParameter>();
            string sqlWhere = ""; string sqlHaving = "";
            List<string> sqlWheres = new List<string>();
            
            #region --組WHERE--
            
            if (!this.dateDebitDate.Value1.Empty())
            {
                sqlWheres.Add("a.Issuedate >= @DebDate1");
                list.Add(new SqlParameter("@DebDate1", DebDate1));
            }
            if (!this.dateDebitDate.Value2.Empty())
            {
                sqlWheres.Add("a.Issuedate <= @DebDate2");
                list.Add(new SqlParameter("@DebDate2", DebDate2.Value.AddDays(1).AddSeconds(-1)));
            }
            if (!this.dateConfirmDate.Value1.Empty())
            {
                sqlWheres.Add("a.cfmdate >= @ConDate1");
                list.Add(new SqlParameter("@ConDate1", ConDate1));
            }
            if (!this.dateConfirmDate.Value2.Empty())
            {
                sqlWheres.Add("a.cfmdate <= @ConDate2");
                list.Add(new SqlParameter("@ConDate2", ConDate2.Value.AddDays(1).AddSeconds(-1)));
            }
            if (!this.txtDebitNoStart.Text.Empty())
            {
                sqlWheres.Add("a.Id between @DebNo1 and @DebNo2");
                list.Add(new SqlParameter("@DebNo1", DebitNo1));
                list.Add(new SqlParameter("@DebNo2", DebitNo2));
            } if (!this.txttpeuser_caneditHandle.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.handle = @handle");
                list.Add(new SqlParameter("@handle", handle));
            } if (!this.txttpeuser_caneditSMR.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.smr = @smr");
                list.Add(new SqlParameter("@smr", smr));
            }
            if (!this.comboFactory.Text.ToString().Empty())
            {
                sqlWheres.Add("a.BrandID  = @factory");
                list.Add(new SqlParameter("@factory", fac));
            }
            if (this.comboPaymentSettled.Text == "Settled")
            {
                //sqlWheres.Add("ISNULL(IsSettled.Val , 0) = 1");
                sqlWheres.Add(" a.Settled = 'Y' ");
            }
            if (this.comboPaymentSettled.Text == "Not Settled")
            {
                //sqlWheres.Add("ISNULL(IsSettled.Val , 0) = 0");
                sqlWheres.Add(" a.Settled = '' ");
            }
            if (!this.dateSettledDate.Value1.Empty())
            {
                sqlHaving += $@" AND IIF(a.IsSubcon=1,
					IIF(ISNULL(IsSettled.Val , 0) = 1,MaxVoucherDate.VoucherDate,NULL), 
					a.SettleDate
				) >= @SettledDate1 ";
                list.Add(new SqlParameter("@SettledDate1", SettDate1));
            }
            if (!this.dateSettledDate.Value2.Empty())
            {
                sqlHaving += $@" AND IIF(a.IsSubcon=1,
					IIF(ISNULL(IsSettled.Val , 0) = 1,MaxVoucherDate.VoucherDate,NULL), 
					a.SettleDate
				) <= @SettledDate2";
                list.Add(new SqlParameter("@SettledDate2", SettDate2.Value.AddDays(1).AddSeconds(-1)));
            }
            #endregion

            sqlWhere = string.Join(" and ", sqlWheres);

            #region --撈List資料--
            cmd = string.Format(@"
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
            cmdDt = string.Format(@"
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

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;

            switch (ReportType)
            {
                case "List":
                    res = DBProxy.Current.Select("", cmd, list, out dtList);
                    break;
                case "Detail List":
                    res = DBProxy.Current.Select("", cmdDt, list, out dt);
                    break;
                default:
                    res = DBProxy.Current.Select("", cmd, list, out dtList); //預設
                    break;
            }

            return res;
        }
       
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            switch (ReportType)
            {
                case "List":
                    if (dtList == null || dtList.Rows.Count == 0)
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }
                    break;
                case "Detail List":
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }
                    break;
                default:
                    if (dtList == null || dtList.Rows.Count == 0) //預設
                    {
                        MyUtility.Msg.ErrorBox("Data not found");
                        return false;
                    }
                    break;
            }

            if ("List".EqualString(this.comboReportType.Text))
            {
                string d1 = (MyUtility.Check.Empty(DebDate1)) ? "" : Convert.ToDateTime(DebDate1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(DebDate2)) ? "" : Convert.ToDateTime(DebDate2).ToString("yyyy/MM/dd");
                string d3 = (MyUtility.Check.Empty(ConDate1)) ? "" : Convert.ToDateTime(ConDate1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(ConDate2)) ? "" : Convert.ToDateTime(ConDate2).ToString("yyyy/MM/dd");
                string d5 = (MyUtility.Check.Empty(SettDate1)) ? "" : Convert.ToDateTime(SettDate1).ToString("yyyy/MM/dd");
                string d6 = (MyUtility.Check.Empty(SettDate2)) ? "" : Convert.ToDateTime(SettDate2).ToString("yyyy/MM/dd");
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R37_List.xltx");
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
                objSheets.Cells[2, 2] = d1 + " ~ " + d2;
                objSheets.Cells[2, 4] = d3 + " ~ " + d4;
                objSheets.Cells[2, 7] = d5 + " ~ " + d6;
                objSheets.Cells[2, 9] = DebitNo1 + " ~ " + DebitNo2;
                objSheets.Cells[2, 11] = handle;
                objSheets.Cells[2, 13] = smr;
                objSheets.Cells[2, 17] = fac;
                objSheets.Cells[2, 19] = Pay;
                MyUtility.Excel.CopyToXls(dtList, "", "Subcon_R37_List.xltx", 3, true, null, objApp);

                Marshal.ReleaseComObject(objSheets);
                return true;
            }
            else if ("Detail List".EqualString(this.comboReportType.Text))
            {
                string d1 = (MyUtility.Check.Empty(DebDate1)) ? "" : Convert.ToDateTime(DebDate1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(DebDate2)) ? "" : Convert.ToDateTime(DebDate2).ToString("yyyy/MM/dd");
                string d3 = (MyUtility.Check.Empty(ConDate1)) ? "" : Convert.ToDateTime(ConDate1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(ConDate2)) ? "" : Convert.ToDateTime(ConDate2).ToString("yyyy/MM/dd");
                string d5 = (MyUtility.Check.Empty(SettDate1)) ? "" : Convert.ToDateTime(SettDate1).ToString("yyyy/MM/dd");
                string d6 = (MyUtility.Check.Empty(SettDate2)) ? "" : Convert.ToDateTime(SettDate2).ToString("yyyy/MM/dd");
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R37_DetailList.xltx");
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
                objSheets.Cells[2, 2] = d1 + " ~ " + d2;
                objSheets.Cells[2, 4] = d3 + " ~ " + d4;
                objSheets.Cells[2, 7] = d5 + " ~ " + d6;
                objSheets.Cells[2, 9] = DebitNo1 + " ~ " + DebitNo2;
                objSheets.Cells[2, 11] = handle;
                objSheets.Cells[2, 13] = smr;
                objSheets.Cells[2, 17] = fac;
                objSheets.Cells[2, 19] = Pay;
                MyUtility.Excel.CopyToXls(dt, "", "Subcon_R37_DetailList.xltx", 3, true, null, objApp);

                Marshal.ReleaseComObject(objSheets);
                return true;
            }
            
            return true;
        }
    }
}
