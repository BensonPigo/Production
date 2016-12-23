using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R37 : Sci.Win.Tems.PrintForm
    {     
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
            string sqlcmd = (@"select DISTINCT BrandID FROM DBO.Debit");
            DBProxy.Current.Select("", sqlcmd, out factory);
            factory.Rows.Add(new string[] { "" });
            factory.DefaultView.Sort = "BrandID";
            this.combFac.DataSource = factory;
            this.combFac.ValueMember = "BrandID";
            this.combFac.DisplayMember = "BrandID";
            this.combFac.SelectedIndex = 0;
            this.combFac.Text = Sci.Env.User.Factory;
            this.comboPay.SelectedIndex = 0;
            this.comboReport.SelectedIndex = 0;
            print.Enabled = false;
        }

        protected override bool ValidateInput()
        {

            DebDate1 = DebDate.Value1;
            DebDate2 = DebDate.Value2;
            ConDate1 = ConDate.Value1;
            ConDate2 = ConDate.Value2;
            SettDate1 = SettDate.Value1;
            SettDate2 = SettDate.Value2;
            DebitNo1 = DebNo1.Text.ToString();
            DebitNo2 = DebNo2.Text.ToString();
            handle = Han.TextBox1.Text;
            smr = txttSMR.TextBox1.Text;
            fac = combFac.Text.ToString();
            Pay = comboPay.SelectedItem.ToString();
            list = new List<SqlParameter>();
            string sqlWhere = ""; string sqlHaving = "";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            
            if (!this.DebDate.Value1.Empty() && !this.DebDate.Value2.Empty())
            {
                sqlWheres.Add("a.Issuedate between @DebDate1 and @DebDate2");
                list.Add(new SqlParameter("@DebDate1", DebDate1));
                list.Add(new SqlParameter("@DebDate2", DebDate2));
            }if (!this.ConDate.Value1.Empty() && !this.ConDate.Value2.Empty())
            {
                sqlWheres.Add("a.cfmdate between @ConDate1 and @ConDate2");
                list.Add(new SqlParameter("@ConDate1", ConDate1));
                list.Add(new SqlParameter("@ConDate2", ConDate2));
            }if (!this.DebNo1.Text.Empty())
            {
                sqlWheres.Add("a.Id between @DebNo1 and @DebNo2");
                list.Add(new SqlParameter("@DebNo1", DebitNo1));
                list.Add(new SqlParameter("@DebNo2", DebitNo2));
            } if (!this.Han.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.handle = @handle");
                list.Add(new SqlParameter("@handle", handle));
            } if (!this.txttSMR.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.smr = @smr");
                list.Add(new SqlParameter("@smr", smr));
            } if (!this.combFac.Text.ToString().Empty())
            {
                sqlWheres.Add("a.BrandID  = @factory");
                list.Add(new SqlParameter("@factory", fac));
            } if (this.comboPay.Text == "Settled")
            {
                list.Add(new SqlParameter("@payment", Pay));
            } if (!this.SettDate.Value1.Empty() && !this.SettDate.Value2.Empty())
            {
                sqlHaving = "and finalVoucher.[Settled Date] between @SettledDate1 and @SettledDate2";
                list.Add(new SqlParameter("@SettledDate1", SettDate1));
                list.Add(new SqlParameter("@SettledDate2", SettDate2));
            }
            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            #region --撈List資料--
            cmd = string.Format(@"SELECT a.ID, [Subcon DBC]=Iif(a.IsSubcon=1,'Y','N'),a.Issuedate,a.BrandID,title='Debit Memo List (Taipei)',a.SendFrom,a.Attn,a.CC,
	               a.subject,a.Handle + '-' + vs1.Name_Extno[Handle],a.SMR+'-'+vs2.Name_Extno[SMR],a.CurrencyID,a.Amount,a.Received,
	               a.Cfm,a.CfmDate
	               ,finalVoucher.[Voucher No.]
	               ,finalVoucher.[Voucher Date]
	               ,finalVoucher.[Settled Date]
               FROM  Debit a
	               outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
	               outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2  
	               outer apply ( 
		            select top 1 tmpSum.*
		            from dbo.LocalDebit LocDeb 
		            left join Debit a
		            outer apply (
					            SELECT  
						            ds.VoucherID,v.VoucherDate,
						             Amount=sum(ds.Amount) over (order by ds.IssueDate ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
					            FROM debit_schedule ds 
					            left join FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
					            WHERE  isnull(ds.VoucherID,'')!=''			
				            )tmpSum
				            on a.ID = LocDeb.id 
				            where a.ID = LocDeb.id and tmpSum.Amount >= LocDeb.Amount+LocDeb.Tax
					            ) Cur_Debit5
		             outer apply(select deb.VoucherFactory,VoucherDate,SettleDate from dbo.Debit deb where deb.VoucherFactory = VoucherID ) n
					 outer apply(select [Voucher No.] = IIF(a.IsSubcon=1,Cur_Debit5.VoucherID, n.VoucherFactory)
	               ,[Voucher Date] = IIF(a.IsSubcon=1,Cur_Debit5.VoucherDate, n.VoucherDate)
	               ,[Settled Date] = IIF(a.IsSubcon=1,Cur_Debit5.VoucherDate, n.SettleDate) ) as finalVoucher
		            where a.type='F' and " + sqlWhere + ' ' + sqlHaving);
            #endregion
            #region --撈Detail List資料--
            cmdDt = string.Format(@"SELECT a.ID, [Subcon DBC]=Iif(a.IsSubcon=1,'Y','N'),a.Issuedate,a.BrandID,title='Debit Memo Detail List (Taipei)',a.SendFrom,a.Attn,a.CC,
	                   a.subject,a.Handle + '-' + vs1.Name_Extno[Handle],a.SMR+'-'+vs2.Name_Extno[SMR],a.CurrencyID,a.Amount,a.Received,
	                   a.Cfm,a.CfmDate
	                   ,finalVoucher.[Voucher No.]
	                   ,finalVoucher.[Voucher Date]
	                   ,finalVoucher.[Settled Date]
	                   ,dd.OrderID,dd.Qty,dd.Amount,dd.reasonid+'-'+dd.ReasonNM [reasonid]
                    FROM  Debit a
	                   left join Debit_Detail dd on a.ID = dd.ID
	                   outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
	                   outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2  
	                   outer apply ( 
		                select top 1 tmpSum.*
		                from dbo.LocalDebit LocDeb 
		                left join Debit a
		                outer apply (
					                SELECT  
						                ds.VoucherID,v.VoucherDate,
						                 Amount=sum(ds.Amount) over (order by ds.IssueDate ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
					                FROM debit_schedule ds 
					                left join FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
					                WHERE  isnull(ds.VoucherID,'')!=''			
				                )tmpSum
				                on a.ID = LocDeb.id 
				                where a.ID = LocDeb.id and tmpSum.Amount >= LocDeb.Amount+LocDeb.Tax
					                ) Cur_Debit5
		                 outer apply(select deb.VoucherFactory,VoucherDate,SettleDate from dbo.Debit deb where deb.VoucherFactory = VoucherID ) n
                         outer apply(select [Voucher No.] = IIF(a.IsSubcon=1,Cur_Debit5.VoucherID, n.VoucherFactory)
	                               ,[Voucher Date] = IIF(a.IsSubcon=1,Cur_Debit5.VoucherDate, n.VoucherDate)
	                               ,[Settled Date] = IIF(a.IsSubcon=1,Cur_Debit5.VoucherDate, n.SettleDate) ) as finalVoucher
		                where a.type='F' and " + sqlWhere + ' ' + sqlHaving);
            #endregion
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select("", cmd, list, out dtList);
            if (!res)
            {
                return res;
            }
            res = DBProxy.Current.Select("", cmdDt, list, out dt);
            if (!res)
            {
                return res;
            }
            return res;
        }
       
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dtList == null || dtList.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            } if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            if ("List".EqualString(this.comboReport.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R37_List.xltx");
                string d1 = (MyUtility.Check.Empty(DebDate1)) ? "" : Convert.ToDateTime(DebDate1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(DebDate2)) ? "" : Convert.ToDateTime(DebDate2).ToString("yyyy/MM/dd");
                string d3 = (MyUtility.Check.Empty(ConDate1)) ? "" : Convert.ToDateTime(ConDate1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(ConDate2)) ? "" : Convert.ToDateTime(ConDate2).ToString("yyyy/MM/dd");
                string d5 = (MyUtility.Check.Empty(SettDate1)) ? "" : Convert.ToDateTime(SettDate1).ToString("yyyy/MM/dd");
                string d6 = (MyUtility.Check.Empty(SettDate2)) ? "" : Convert.ToDateTime(SettDate2).ToString("yyyy/MM/dd");
                xl.dicDatas.Add("##Debitdate", d1 + "~" + d2);
                xl.dicDatas.Add("##ConfirmDate", d3 + "~" + d4);
                xl.dicDatas.Add("##SettledDate", d5 + "~" + d6);
                xl.dicDatas.Add("##DebitN", DebitNo1 + "~" + DebitNo2);
                xl.dicDatas.Add("##Handle", handle);
                xl.dicDatas.Add("##SMR", smr);
                xl.dicDatas.Add("##Fac", fac);
                xl.dicDatas.Add("##Pay", Pay);
                xl.dicDatas.Add("##Deb", dtList);
                xl.Save(outpath, false);
            }
            else if ("Detail List".EqualString(this.comboReport.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R37_DetailList.xltx");
                string d1 = (MyUtility.Check.Empty(DebDate1)) ? "" : Convert.ToDateTime(DebDate1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(DebDate2)) ? "" : Convert.ToDateTime(DebDate2).ToString("yyyy/MM/dd");
                string d3 = (MyUtility.Check.Empty(ConDate1)) ? "" : Convert.ToDateTime(ConDate1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(ConDate2)) ? "" : Convert.ToDateTime(ConDate2).ToString("yyyy/MM/dd");
                string d5 = (MyUtility.Check.Empty(SettDate1)) ? "" : Convert.ToDateTime(SettDate1).ToString("yyyy/MM/dd");
                string d6 = (MyUtility.Check.Empty(SettDate2)) ? "" : Convert.ToDateTime(SettDate2).ToString("yyyy/MM/dd");
                xl.dicDatas.Add("##Debitdate", d1 + "~" + d2);
                xl.dicDatas.Add("##ConfirmDate", d3 + "~" + d4);
                xl.dicDatas.Add("##SettledDate", d5 + "~" + d6);
                xl.dicDatas.Add("##DebitN", DebitNo1 + "~" + DebitNo2);
                xl.dicDatas.Add("##Handle", handle);
                xl.dicDatas.Add("##SMR", smr);
                xl.dicDatas.Add("##Fac", fac);
                xl.dicDatas.Add("##Pay", Pay);
                xl.dicDatas.Add("##Deb", dt);
                xl.Save(outpath, false);
            }
            
            return true;
        }

       
    }
}
