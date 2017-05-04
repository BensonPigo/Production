using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Utility.Excel;
using Ict;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

namespace Sci.Production.Subcon
{
    public partial class R36 : Sci.Win.Tems.PrintForm
    {
        List<SqlParameter> lis;
        DataTable dt; string cmd;
        DateTime? debitdate1;DateTime? debitdate2;
        DateTime? aprdate1 ; DateTime? aprdate2;
        string SDNo1; string SDNo2;
        string Supplier;
        string handle ;
        string smr ;
        string status;
        string factoryid;
        DateTime? amtrevisedate1;DateTime? amtrevisedate2;
        DateTime? ReceiveDate1 ;DateTime? ReceiveDate2 ;     
        DataTable dtSummary; string cmdSummary;
        DataTable dtDetail; string cmdDetail;
        DataTable dtSchedule; string cmdSchedule;
        DateTime? SettledDate1; DateTime? SettledDate2;
        string payment;
        public R36(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select DISTINCT ftygroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            this.comboReportType.SelectedIndex = 0;
            this.comboStatus.SelectedIndex = 0;
            this.comboPaymentSettled.SelectedIndex = 0;
            this.comboOrderBy.SelectedIndex = 0;
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {
            bool dateRange1_Empty = !this.dateDebitDate.HasValue, dateRange2_Empty = !this.dateApprovedDate.HasValue, dateRange3_Empty = !this.dateSettledDate.HasValue, textbox1_Empty = this.txtSDNoStart.Text.Empty(), textbox2_Empty = this.txtSDNoEnd.Text.Empty(), txtLocalSupp1_Empty =this.txtLocalSuppSupplier.TextBox1.Text.Empty()
                , txtuser1_Empty = this.txtuserHandle.TextBox1.Text.Empty(), txtuser2_Empty = this.txtuserSMR.TextBox1.Text.Empty(), comboBox5_Empty = this.comboFactory.Text.Empty(), comboBox2_Empty = this.comboStatus.Text.Empty(), comboBox3_Empty = this.comboPaymentSettled.Text.Empty(), dateRange4_Empty = !this.dateAmtRevised.HasValue, dateRange5_Empty = !this.dateReceiveddate.HasValue;

            if (dateRange1_Empty && dateRange2_Empty && dateRange3_Empty && textbox1_Empty && textbox2_Empty && txtLocalSupp1_Empty && txtuser1_Empty && txtuser2_Empty && comboBox5_Empty && comboBox2_Empty && comboBox3_Empty
               && dateRange4_Empty && dateRange5_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select at least one field entry");

                txtSDNoStart.Focus();

                return false;
            }

            debitdate1 = dateDebitDate.Value1;
            debitdate2 = dateDebitDate.Value2;
            aprdate1 = dateApprovedDate.Value1;
            aprdate2 = dateApprovedDate.Value2;
            SDNo1= txtSDNoStart.Text.ToString();
            SDNo2 = txtSDNoEnd.Text.ToString();
            Supplier = txtLocalSuppSupplier.TextBox1.Text;
            handle = txtuserHandle.TextBox1.Text;
            smr = txtuserSMR.TextBox1.Text;
            status = comboStatus.SelectedItem.ToString();
            factoryid = comboFactory.Text.ToString();
            amtrevisedate1 = dateAmtRevised.Value1;
            amtrevisedate2 = dateAmtRevised.Value2;
            ReceiveDate1 = dateReceiveddate.Value1;
            ReceiveDate2 = dateReceiveddate.Value2;
            SettledDate1 = dateSettledDate.Value1;
            SettledDate2 = dateSettledDate.Value2;
            payment = comboPaymentSettled.SelectedItem.ToString();
           lis = new List<SqlParameter>();
           string sqlWhere = ""; string order = "";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.txtSDNoStart.Text.Empty())
            {
                sqlWheres.Add("a.Id between @SDNo1 and @SDNo2");
                lis.Add(new SqlParameter("@SDNo1", SDNo1));
                lis.Add(new SqlParameter("@SDNo2", SDNo2));
            } 
            if (!this.dateDebitDate.Value1.Empty())
            {
                sqlWheres.Add("a.Issuedate >= @debitdate1");
                lis.Add(new SqlParameter("@debitdate1", debitdate1));
            }
            if (!this.dateDebitDate.Value2.Empty())
            {
                sqlWheres.Add("a.Issuedate <= @debitdate2");
                lis.Add(new SqlParameter("@debitdate2", debitdate2));
            }
            if (!this.dateApprovedDate.Value1.Empty())
            {
                sqlWheres.Add("a.CfmDate >= @aprdate1");
                lis.Add(new SqlParameter("@aprdate1", aprdate1));
            }
            if (!this.dateApprovedDate.Value2.Empty())
            {
                sqlWheres.Add("a.CfmDate <= @aprdate2");
                lis.Add(new SqlParameter("@aprdate2", aprdate2));
            }
            if (!this.txtLocalSuppSupplier.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.localsuppid = @localsuppid");
                lis.Add(new SqlParameter("@localsuppid", Supplier));
            } if (!this.txtuserHandle.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.handle = @handle");
                lis.Add(new SqlParameter("@handle", handle));
            } if (!this.txtuserSMR.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.smr = @smr");
                lis.Add(new SqlParameter("@smr", smr));
            } if (!this.factoryid.Empty())
            {
                sqlWheres.Add("a.factoryid = @factoryid");
                lis.Add(new SqlParameter("@factoryid", factoryid));
            } if (!this.comboStatus.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("a.Status = @status");
                lis.Add(new SqlParameter("@status", status));
            } 
            if (!this.dateAmtRevised.Value1.Empty())
            {
                sqlWheres.Add("a.amtrevisedate >= @amtrevisedate1");
                lis.Add(new SqlParameter("@amtrevisedate1", amtrevisedate1));
            }
            if (!this.dateAmtRevised.Value2.Empty())
            {
                sqlWheres.Add("a.amtrevisedate <= @amtrevisedate2");
                lis.Add(new SqlParameter("@amtrevisedate2", amtrevisedate2));
            }
            if (!this.dateReceiveddate.Value1.Empty())
            {
                sqlWheres.Add("a.ReceiveDate >= @ReceiveDate1");
                lis.Add(new SqlParameter("@ReceiveDate1", ReceiveDate1));
            }
            if (!this.dateReceiveddate.Value2.Empty())
            {
                sqlWheres.Add("a.ReceiveDate <= @ReceiveDate2");
                lis.Add(new SqlParameter("@ReceiveDate2", ReceiveDate2));
            } 
            int needSettleData = 0;
            if (!this.dateSettledDate.Value1.Empty())
            {
                lis.Add(new SqlParameter("@SettledDate1", SettledDate1));
                needSettleData = 1;
            }
            if (!this.dateSettledDate.Value2.Empty())
            {
                lis.Add(new SqlParameter("@SettledDate2", SettledDate2));
                needSettleData = 1;
            } 
            if (this.comboPaymentSettled.Text == "Settled")
            {
                lis.Add(new SqlParameter("@payment", payment));
                needSettleData = 1;
            } if (this.comboPaymentSettled.Text == "Not Settled")
            {
                lis.Add(new SqlParameter("@payment", payment));
                needSettleData = 0;
            }
            if (this.comboPaymentSettled.Text == " ")
            {
                needSettleData = 1;
            }
          
            lis.Add(new SqlParameter("@NeedSettleData", needSettleData));

            if (this.comboOrderBy.Text == "By Handle")
            {
                order="order by  a.handle";
            }    
            else if (this.comboOrderBy.Text == "By Supp")
            {
                order="order by  a.localsuppid";
            }
            else if (this.comboOrderBy.Text == "By SD#")
            {
                order="order by  a.id";
            }
            #endregion
sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }
            #region --撈ListExcel資料--
            
           cmd= string.Format(@"
    select a.ID, vs1.Name_Extno as Handle, vs2.Name_Extno as SMR,a.LocalSuppID+'-'+ s.Abb as SupplierByPay
        ,(SELECT Orderid + ',' from LocalDebit_Detail LDD where LDD.ID=a.id FOR XML PATH('')) [SPList]
        ,(SELECT distinct R.Name + ',' from LocalDebit_Detail LDD 
        left join Reason R on R.ReasonTypeID='DebitNote_Factory' and R.ID=LDD.Reasonid
        where LDD.ID=a.id FOR XML PATH('')) [ReasonList]
		,a.Description ,a.exchange,a.currencyid,a.amount,a.tax,a.taxrate,a.amtrevisedate,a.amtrevisename,a.receivedate
		,a.receivename,a.cfmdate,a.cfmname,V.VoucherID,a.printdate,a.status,a.statuseditdate,
		vs3.Name_Extno as addname,a.adddate,vs4.Name_Extno as edit,a.editdate
        INTO #TEMP
		from DBO.LocalDebit a WITH (NOLOCK) 
			left join dbo.LocalSupp s WITH (NOLOCK) on a.localsuppid = s.ID
			left join dbo.Reason R WITH (NOLOCK) on a.AddName = R.AddName
		    outer apply (select * from dbo.Debit_Schedule vs WITH (NOLOCK) where a.id =  vs.ID ) V
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.addname ) vs3
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.editname ) vs4" + sqlWhere + ' ' + order + ' ' + @"SELECT DISTINCT * FROM #TEMP");
            #endregion
           #region --撈SummaryExcel資料--
           cmdSummary = string.Format(@"
select a.ID,a.Status,a.issuedate,a.factoryid, vs1.Name_Extno as Handle, vs2.Name_Extno as SMR,a.LocalSuppID+'-'+ s.Abb as SupplierByPay,a.TaipeiCurrencyID, 
	   a.TaipeiAMT,a.exchange,a.currencyid,a.amount,a.Tax,a.taxrate,a.amount+a.tax as ttlAmount,vs3.Name_Extno as AmtRN,
	   a.AmtReviseDate,vs4.Name_Extno as AccRH,a.ReceiveDate,vs5.Name_Extno as AccAH,a.cfmdate,
	   cur_schedule.VoucherID[VoucherNo],cur_schedule.VoucherDate,cur_schedule.VoucherDate[Settled Date],
	   b1.ttlCA,b1.ttlAddition,ttl.ttlSA,ttl.ttlRA
        into #tmp
		from DBO.LocalDebit a WITH (NOLOCK) 
			left join dbo.LocalSupp s WITH (NOLOCK) on a.localsuppid = s.ID
			inner join LocalDebit_Detail b WITH (NOLOCK) on a.id = b.id
			left join dbo.Reason R WITH (NOLOCK) on a.AddName = R.AddName
		   	outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.AmtReviseName ) vs3
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.ReceiveName ) vs4
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.cfmName ) vs5
			outer apply (Select [ttlCA] = Sum(b.amount) ,[ttlAddition] = sum(b.Addition)
				from localdebit_detail b WITH (NOLOCK) where b.ID = a.id  ) b1
			outer apply (Select [ttlRA] = Sum(iif(isnull(dsch.VoucherID,'')!='',dsch.amount,0)) ,
						[ttlSA] = sum(dsch.amount)
				from debit_schedule dsch WITH (NOLOCK) where dsch.ID = a.id  ) ttl
	OUTER APPLY( 
		select top 1 * 
		from(
			SELECT  
				ds.VoucherID,v.VoucherDate,
				 Amount=sum(ds.Amount) 
					over (order by ds.IssueDate ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
			FROM debit_schedule ds WITH (NOLOCK) 
			left join FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
			WHERE  @NeedSettleData=1 and ds.ID = a.ID and isnull(ds.VoucherID,'')!=''		
		) as tmpSum
		where tmpSum.VoucherDate is not null and tmpSum.Amount >= a.Amount+a.Tax
	)AS cur_schedule
" + sqlWhere + ' ' + order + ' ' + @"SELECT DISTINCT * FROM #tmp");
           #endregion
           #region --撈DetailExcel資料--
           cmdDetail = string.Format(@"
select a.ID,a.Status,a.issuedate,a.factoryid, vs1.Name_Extno as Handle, vs2.Name_Extno as SMR,a.LocalSuppID+'-'+ s.Abb as SupplierByPay,a.TaipeiCurrencyID, 
	   a.TaipeiAMT,a.exchange,a.currencyid,[amount1]=a.amount,a.Tax,a.taxrate,a.amount+a.tax as ttlAmount,vs3.Name_Extno as AmtRN,
	   a.AmtReviseDate,vs4.Name_Extno as AccRH,a.ReceiveDate,vs5.Name_Extno as AccAH,a.cfmdate,
	   cur_schedule.VoucherID[VoucherNo],cur_schedule.VoucherDate,cur_schedule.VoucherDate[Settled Date],
	   b.Orderid,b.qty,b.UnitID,[amount2]=b.Amount,b.Addition,b.taipeiReason,R.name,b.Description
        into #tmp1
		from DBO.LocalDebit a WITH (NOLOCK) 
			left join dbo.LocalSupp s WITH (NOLOCK) on a.localsuppid = s.ID
			inner join LocalDebit_Detail b WITH (NOLOCK) on a.id = b.id
			left join dbo.Reason R WITH (NOLOCK) on b.Reasonid = R.id and R.ReasonTypeID = 'DebitNote_Factory'
		    outer apply (select * from dbo.Debit_Schedule vs WITH (NOLOCK) where a.id =  vs.ID ) V
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.AmtReviseName ) vs3
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.ReceiveName ) vs4
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.cfmName ) vs5
			OUTER APPLY( 
		select top 1 * 
		from(
			SELECT  
				ds.VoucherID,v.VoucherDate,
				 Amount=sum(ds.Amount) 
					over (order by ds.IssueDate ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
			FROM debit_schedule ds WITH (NOLOCK) 
			left join FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
			WHERE  @NeedSettleData=1 and ds.ID = a.ID and isnull(ds.VoucherID,'')!=''			
		) as tmpSum
		where tmpSum.VoucherDate is not null and tmpSum.Amount >= a.Amount+a.Tax
	)AS cur_schedule
" + sqlWhere + ' ' + order + ' ' + @"SELECT DISTINCT * FROM #tmp1");
           #endregion
           #region --撈ScheduleExcel資料--
           cmdSchedule = string.Format(@"select a.ID,a.Status,a.issuedate,a.factoryid, vs1.Name_Extno as Handle, vs2.Name_Extno as SMR,a.LocalSuppID+'-'+ s.Abb as SupplierByPay,a.TaipeiCurrencyID, 
	   a.TaipeiAMT,a.exchange,a.currencyid,a.amount,a.Tax,a.taxrate,a.amount+a.tax as ttlAmount,vs3.Name_Extno as AmtRN,
	   a.AmtReviseDate,vs4.Name_Extno as AccRH,a.ReceiveDate,vs5.Name_Extno as AccAH,a.cfmdate,
	   cur_schedule.VoucherID[VoucherNo],cur_schedule.VoucherDate,cur_schedule.VoucherDate[Settled Date],
	   c.issuedate,c.amount,c.VoucherId,V.VoucherDate,c.addDate,vs6.Name_Extno as SCN,c.editdate,vs7.Name_Extno as SEN
	  
		from DBO.LocalDebit a WITH (NOLOCK) 
			inner join dbo.debit_schedule c WITH (NOLOCK) on a.id = c.id
            outer apply(select * from LocalSupp s WITH (NOLOCK) where a.localsuppid = s.ID)s
		    outer apply (select VoucherDate from FinanceEn.dbo.Voucher Fv where Fv.id = c.VoucherID ) V
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.AmtReviseName ) vs3
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.ReceiveName ) vs4
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.cfmName ) vs5
			outer apply (select * from dbo.View_ShowName vs where vs.id = c.addName ) vs6
			outer apply (select * from dbo.View_ShowName vs where vs.id = c.editname ) vs7
			OUTER APPLY( 
		select top 1 * 
		from(
			SELECT  
				ds.VoucherID,v.VoucherDate,
				 Amount=sum(ds.Amount) 
					over (order by ds.IssueDate ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
			FROM debit_schedule ds WITH (NOLOCK) 
			left join FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
			WHERE  @NeedSettleData=1 and ds.ID = a.ID and isnull(ds.VoucherID,'')!=''			
		) as tmpSum
		where tmpSum.VoucherDate is not null and tmpSum.Amount >= a.Amount+a.Tax
	)AS cur_schedule" + sqlWhere + ' ' + order);
            #endregion
           return base.ValidateInput();
        }
        
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select("", cmd, lis, out dt);
            if (!res)
            {
                return res;
            }
            res = DBProxy.Current.Select("", cmdSummary, lis, out dtSummary);
            if (!res)
            {
                return res;
            }
           res = DBProxy.Current.Select("", cmdDetail, lis, out dtDetail);
           if (!res)
           {
               return res;
           }
           res = DBProxy.Current.Select("", cmdSchedule, lis, out dtSchedule);
      
            return res;
        }
       
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            } if (dtSummary == null || dtSummary.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);

            if ("Debit Note List".EqualString(this.comboReportType.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R36_DebitNote(LocalSupplier).xltx");
                Sci.Utility.Excel.SaveXltReportCls.xltRptTable dt1 = new SaveXltReportCls.xltRptTable(dt);
                dt1.boAutoFitRow = true;
                x1.dicDatas.Add("##SD", dt1);
                dt1.ShowHeader = false;
                x1.Save();
                return true;
            }

            else if ("Summary".EqualString(this.comboReportType.Text))
            {
                
                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R36_DebitNote&ScheduleSummary(LocalSupplier).xltx");
                string d1 = (MyUtility.Check.Empty(debitdate1)) ? "" : Convert.ToDateTime(debitdate1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(debitdate2)) ? "" : Convert.ToDateTime(debitdate2).ToString("yyyy/MM/dd");
                string d3 = (MyUtility.Check.Empty(aprdate1)) ? "" : Convert.ToDateTime(aprdate1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(aprdate2)) ? "" : Convert.ToDateTime(aprdate2).ToString("yyyy/MM/dd");
                string d5 = (MyUtility.Check.Empty(SettledDate1)) ? "" : Convert.ToDateTime(SettledDate1).ToString("yyyy/MM/dd");
                string d6 = (MyUtility.Check.Empty(SettledDate2)) ? "" : Convert.ToDateTime(SettledDate2).ToString("yyyy/MM/dd");
               
                x1.dicDatas.Add("##DebiteDate", d1 + "~" + d2);
                x1.dicDatas.Add("##ApprovedDate", d3+"~"+d4);
                x1.dicDatas.Add("##SettledDate", d5 + "~" + d6);
                x1.dicDatas.Add("##SDNO",SDNo1+ "~" + SDNo2);
                x1.dicDatas.Add("##Supplier", Supplier);
                x1.dicDatas.Add("##Handle", handle);
                x1.dicDatas.Add("##SMR", smr);
                x1.dicDatas.Add("##FACTORY", factoryid);
                x1.dicDatas.Add("##Status", status);
                x1.dicDatas.Add("##PaymentSettled", payment);
                Sci.Utility.Excel.SaveXltReportCls.xltRptTable dtSummary1 = new SaveXltReportCls.xltRptTable(dtSummary);
                dtSummary1.boAutoFitColumn = true;
                x1.dicDatas.Add("##SD", dtSummary1);
                dtSummary1.ShowHeader = false;
                x1.Save();
                return true;
            }
            else if ("Detail".EqualString(this.comboReportType.Text))
            {
                
                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R36_DebitNoteDetail(LocalSupplier).xltx");
                string d1 = (MyUtility.Check.Empty(debitdate1)) ? "" : Convert.ToDateTime(debitdate1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(debitdate2)) ? "" : Convert.ToDateTime(debitdate2).ToString("yyyy/MM/dd");
                string d3 = (MyUtility.Check.Empty(aprdate1)) ? "" : Convert.ToDateTime(aprdate1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(aprdate2)) ? "" : Convert.ToDateTime(aprdate2).ToString("yyyy/MM/dd");
                string d5 = (MyUtility.Check.Empty(SettledDate1)) ? "" : Convert.ToDateTime(SettledDate1).ToString("yyyy/MM/dd");
                string d6 = (MyUtility.Check.Empty(SettledDate2)) ? "" : Convert.ToDateTime(SettledDate2).ToString("yyyy/MM/dd");
                x1.dicDatas.Add("##DebiteDate", d1 + "~" + d2);
                x1.dicDatas.Add("##ApprovedDate", d3 + "~" + d4);
                x1.dicDatas.Add("##SettledDate", d5 + "~" + d6);
                x1.dicDatas.Add("##SDNO", SDNo1 + "~" + SDNo2);
                x1.dicDatas.Add("##Supplier", Supplier);
                x1.dicDatas.Add("##Handle", handle);
                x1.dicDatas.Add("##SMR", smr);
                x1.dicDatas.Add("##FACTORY", factoryid);
                x1.dicDatas.Add("##Status", status);
                x1.dicDatas.Add("##PaymentSettled", payment);
                //SaveXltReportCls.xltRptTable xdt = new SaveXltReportCls.xltRptTable(dtDetail);
                //xdt.boAutoFitColumn = true;
                Sci.Utility.Excel.SaveXltReportCls.xltRptTable dtDetail1 = new SaveXltReportCls.xltRptTable(dtDetail);
                dtDetail1.boAutoFitColumn = true;
                x1.dicDatas.Add("##SD", dtDetail1);
                dtDetail1.ShowHeader = false;

                x1.Save();
                return true;
            }
            else if ("Debit Schedule Detail".EqualString(this.comboReportType.Text))
            {
               
                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R36_DebitScheduleDetail(LocalSupplier).xltx");
                string d1 = (MyUtility.Check.Empty(debitdate1)) ? "" : Convert.ToDateTime(debitdate1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(debitdate2)) ? "" : Convert.ToDateTime(debitdate2).ToString("yyyy/MM/dd");
                string d3 = (MyUtility.Check.Empty(aprdate1)) ? "" : Convert.ToDateTime(aprdate1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(aprdate2)) ? "" : Convert.ToDateTime(aprdate2).ToString("yyyy/MM/dd");
                string d5 = (MyUtility.Check.Empty(SettledDate1)) ? "" : Convert.ToDateTime(SettledDate1).ToString("yyyy/MM/dd");
                string d6 = (MyUtility.Check.Empty(SettledDate2)) ? "" : Convert.ToDateTime(SettledDate2).ToString("yyyy/MM/dd");
                x1.dicDatas.Add("##DebiteDate", d1 + "~" + d2);
                x1.dicDatas.Add("##ApprovedDate", d3 + "~" + d4);
                x1.dicDatas.Add("##SettledDate", d5 + "~" + d6);
                x1.dicDatas.Add("##SDNO", SDNo1 + "~" + SDNo2);
                x1.dicDatas.Add("##Supplier", Supplier);
                x1.dicDatas.Add("##Handle", handle);
                x1.dicDatas.Add("##SMR", smr);
                x1.dicDatas.Add("##FACTORY", factoryid);
                x1.dicDatas.Add("##Status", status);
                x1.dicDatas.Add("##PaymentSettled", payment);
                //SaveXltReportCls.xltRptTable xdt = new SaveXltReportCls.xltRptTable(dtSchedule);
                //xdt.boAutoFitColumn = true;
                Sci.Utility.Excel.SaveXltReportCls.xltRptTable dtSchedule1 = new SaveXltReportCls.xltRptTable(dtSchedule);
                dtSchedule1.boAutoFitColumn = true;
                x1.dicDatas.Add("##SD", dtSchedule1);
                dtSchedule1.ShowHeader = false;
                x1.Save();
                return true;
            }
            return true;
        }
    }
}
