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
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 0;
        }
        protected override bool ValidateInput()
        {
            bool dateRange1_Empty = !this.dateRange_Debit.HasValue, dateRange2_Empty = !this.dateRange_Approve.HasValue, dateRange3_Empty = !this.dateRange3.HasValue, textbox1_Empty = this.textBox1.Text.Empty(), textbox2_Empty = this.textBox2.Text.Empty(), txtLocalSupp1_Empty =this.txtLocalSupp1.TextBox1.Text.Empty()
                , txtuser1_Empty = this.txtuser1.TextBox1.Text.Empty(), txtuser2_Empty =this.txtuser2.TextBox1.Text.Empty(), txtfactory1_Empty = this.txtfactory1.Text.Empty(), comboBox2_Empty = this.comboBox2.Text.Empty(), comboBox3_Empty =this.comboBox3.Text.Empty(), dateRange4_Empty =!this.dateRange4.HasValue, dateRange5_Empty = !this.dateRange5.HasValue;

            if (dateRange1_Empty && dateRange2_Empty && dateRange3_Empty && textbox1_Empty && textbox2_Empty && txtLocalSupp1_Empty && txtuser1_Empty && txtuser2_Empty && txtfactory1_Empty && comboBox2_Empty && comboBox3_Empty
               && dateRange4_Empty && dateRange5_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select at least one field entry");

                textBox1.Focus();

                return false;
            }

            debitdate1 = dateRange_Debit.Value1;
            debitdate2 = dateRange_Debit.Value2;
            aprdate1 = dateRange_Approve.Value1;
            aprdate2 = dateRange_Approve.Value2;
            SDNo1= textBox1.Text.ToString();
            SDNo2 = textBox2.Text.ToString();
            Supplier = txtLocalSupp1.TextBox1.Text;
            handle = txtuser1.TextBox1.Text;
            smr = txtuser2.TextBox1.Text;
            status = comboBox2.SelectedItem.ToString();
            factoryid = txtfactory1.Text;
            amtrevisedate1 = dateRange4.Value1;
            amtrevisedate2 = dateRange4.Value2;
            ReceiveDate1 = dateRange5.Value1;
            ReceiveDate2 = dateRange5.Value2;
            SettledDate1 = dateRange3.Value1;
            SettledDate2 = dateRange3.Value2;
            payment = comboBox3.SelectedItem.ToString();
           lis = new List<SqlParameter>();
           string sqlWhere = ""; string order = "";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.textBox1.Text.Empty())
            {
                sqlWheres.Add("a.Id between @SDNo1 and @SDNo2");
                lis.Add(new SqlParameter("@SDNo1", SDNo1));
                lis.Add(new SqlParameter("@SDNo2", SDNo2));
            } 
            if (!this.dateRange_Debit.Value1.Empty() && !this.dateRange_Debit.Value2.Empty())
            {
                sqlWheres.Add("a.Issuedate between @debitdate1 and @debitdate2");
                lis.Add(new SqlParameter("@debitdate1", debitdate1));
                lis.Add(new SqlParameter("@debitdate2", debitdate2));
            } if (!this.dateRange_Approve.Value1.Empty() && !this.dateRange_Approve.Value2.Empty())
            {
                sqlWheres.Add("a.CfmDate between @aprdate1 and @aprdate2");
                lis.Add(new SqlParameter("@aprdate1", aprdate1));
                lis.Add(new SqlParameter("@aprdate2", aprdate2));
            } if (!this.txtLocalSupp1.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.localsuppid = @localsuppid");
                lis.Add(new SqlParameter("@localsuppid", Supplier));
            } if (!this.txtuser1.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.handle = @handle");
                lis.Add(new SqlParameter("@handle", handle));
            } if (!this.txtuser2.TextBox1.Text.Empty())
            {
                sqlWheres.Add("a.smr = @smr");
                lis.Add(new SqlParameter("@smr", smr));
            } if (!this.txtfactory1.Text.Empty())
            {
                sqlWheres.Add("a.factoryid = @factoryid");
                lis.Add(new SqlParameter("@factoryid", factoryid));
            } if (!this.comboBox2.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("a.Status = @status");
                lis.Add(new SqlParameter("@status", status));
            } if (!this.dateRange4.Value1.Empty() && !this.dateRange4.Value2.Empty())
            {
                sqlWheres.Add("a.amtrevisedate between @amtrevisedate1 and @amtrevisedate2");
                lis.Add(new SqlParameter("@amtrevisedate1", amtrevisedate1));
                lis.Add(new SqlParameter("@amtrevisedate2", amtrevisedate2));
            } if (!this.dateRange5.Value1.Empty() && !this.dateRange5.Value2.Empty())
            {
                sqlWheres.Add("a.ReceiveDate between @ReceiveDate1 and @ReceiveDate2");
                lis.Add(new SqlParameter("@ReceiveDate1", ReceiveDate1));
                lis.Add(new SqlParameter("@ReceiveDate2", ReceiveDate2));
            } 
            int needSettleData = 0;
            if (!this.dateRange3.Value1.Empty() && !this.dateRange3.Value2.Empty())
            {
                lis.Add(new SqlParameter("@SettledDate1", SettledDate1));
                lis.Add(new SqlParameter("@SettledDate2", SettledDate2));
                needSettleData = 1;
            } if (this.comboBox3.Text == "Settled")
            {
                lis.Add(new SqlParameter("@payment", payment));
                needSettleData = 1;
            } if (this.comboBox3.Text == "Not Settled")
            {
                lis.Add(new SqlParameter("@payment", payment));
                needSettleData = 0;
            }
            lis.Add(new SqlParameter("@NeedSettleData", needSettleData));
            if (this.comboBox4.Text == "By Handle")
            {
                order="order by  a.handle";
            }    
            else if (this.comboBox4.Text == "By Supp")
            {
                order="order by  a.localsuppid";
            }
            else if (this.comboBox4.Text == "By SD#")
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
--declare @debitdate1 datetime = '2015-05-01'
--declare @debitdate2 datetime = '2015-05-21'
--declare @aprdate1 datetime = '2015-05-11'
--declare @aprdate2 datetime = '2015-05-30'
--declare @localsuppid varchar(8) = 'G008B'
--declare @handle Char(10) = ''
--declare @smr Char(10) = ''
--declare @factoryid Char(8) = ''
--declare @amtrevisedate1 datetime =  '2016-08-03'
--declare @amtrevisedate2 datetime =  '2016-08-04'
--declare @ReceiveDate1 datetime =  '2016-09-03'
--declare @ReceiveDate2 datetime = '2016-09-23'
--declare @status varchar(30) = 'new'
--declare @SDNo char(13) = ''
--declare @orderby varchar(30) = 'a.handle'

select a.ID, vs1.Name_Extno as Handle, vs2.Name_Extno as SMR,a.LocalSuppID + s.Abb as SupplierByPay, 
		( select stuff(s.SPList ,1,1,'')
		  from (
			   select (select  ','+Orderid 
			   from dbo.LocalDebit_Detail 
			   for xml path('')
			   ) as SPList
		  ) as s) [SPList],
		  ( select stuff(R.ReasonList ,1,1,'')
		  from (
			   select (select  ','+Name 
			   from dbo.Reason 
			   for xml path('')
			   ) as ReasonList
		  ) as R) [ReasonList]
		,a.Description ,a.exchange,a.currencyid,a.amount,a.tax,a.taxrate,a.amtrevisedate,a.amtrevisename,a.receivedate
		,a.receivename,a.cfmdate,a.cfmname,V.VoucherID,a.printdate,a.status,a.statuseditdate,
		vs3.Name_Extno as addname,a.adddate,vs4.Name_Extno as edit,a.editdate
		from DBO.LocalDebit a 
			inner join dbo.LocalSupp s on a.localsuppid = s.ID
			left join dbo.Reason R on a.AddName = R.AddName
		    outer apply (select * from dbo.Debit_Schedule vs where a.id =  vs.ID ) V
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.addname ) vs3
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.editname ) vs4" + sqlWhere + ' ' + order);
            #endregion
           #region --撈SummaryExcel資料--
           cmdSummary = string.Format(@"
select a.ID,a.Status,a.issuedate,a.factoryid, vs1.Name_Extno as Handle, vs2.Name_Extno as SMR,a.LocalSuppID + s.Abb as SupplierByPay,a.TaipeiCurrencyID, 
	   a.TaipeiAMT,a.exchange,a.currencyid,a.amount,a.Tax,a.taxrate,a.amount+a.tax as ttlAmount,vs3.Name_Extno as AmtRN,
	   a.AmtReviseDate,vs4.Name_Extno as AccRH,a.ReceiveDate,vs5.Name_Extno as AccAH,a.cfmdate,
	   cur_schedule.VoucherID[VoucherNo],cur_schedule.VoucherDate,cur_schedule.VoucherDate[Settled Date],
	   b1.ttlCA,b1.ttlAddition,ttl.ttlSA,ttl.ttlRA
		from DBO.LocalDebit a 
			inner join dbo.LocalSupp s on a.localsuppid = s.ID
			inner join LocalDebit_Detail b on a.id = b.id
			left join dbo.Reason R on a.AddName = R.AddName
		   	outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.AmtReviseName ) vs3
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.ReceiveName ) vs4
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.cfmName ) vs5
			outer apply (Select [ttlCA] = Sum(b.amount) ,[ttlAddition] = sum(b.Addition)
				from localdebit_detail b where b.ID = a.id  ) b1
			outer apply (Select [ttlRA] = Sum(iif(isnull(dsch.VoucherID,'')!='',dsch.amount,0)) ,
						[ttlSA] = sum(dsch.amount)
				from debit_schedule dsch where dsch.ID = a.id  ) ttl
	OUTER APPLY( 
		select top 1 * 
		from(
			SELECT  
				ds.VoucherID,v.VoucherDate,
				 Amount=sum(ds.Amount) 
					over (order by ds.IssueDate ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
			FROM debit_schedule ds 
			left join FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
			WHERE  @NeedSettleData=1 and ds.ID = a.ID and isnull(ds.VoucherID,'')!=''		
		) as tmpSum
		where tmpSum.VoucherDate is not null and tmpSum.Amount >= a.Amount+a.Tax
	)AS cur_schedule
" + sqlWhere + ' ' + order);
           #endregion
           #region --撈DetailExcel資料--
           cmdDetail = string.Format(@"
select a.ID,a.Status,a.issuedate,a.factoryid, vs1.Name_Extno as Handle, vs2.Name_Extno as SMR,a.LocalSuppID + s.Abb as SupplierByPay,a.TaipeiCurrencyID, 
	   a.TaipeiAMT,a.exchange,a.currencyid,a.amount,a.Tax,a.taxrate,a.amount+a.tax as ttlAmount,vs3.Name_Extno as AmtRN,
	   a.AmtReviseDate,vs4.Name_Extno as AccRH,a.ReceiveDate,vs5.Name_Extno as AccAH,a.cfmdate,
	   cur_schedule.VoucherID[VoucherNo],cur_schedule.VoucherDate,cur_schedule.VoucherDate[Settled Date],
	   b.Orderid,b.qty,b.UnitID,b.Amount,b.Addition,b.taipeiReason,R.name,b.Description
		from DBO.LocalDebit a 
			inner join dbo.LocalSupp s on a.localsuppid = s.ID
			inner join LocalDebit_Detail b on a.id = b.id
			left join dbo.Reason R on b.Reasonid = R.id and R.ReasonTypeID = 'DebitNote_Factory'
		    outer apply (select * from dbo.Debit_Schedule vs where a.id =  vs.ID ) V
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
			FROM debit_schedule ds 
			left join FinanceEn.dbo.Voucher as v on v.id = ds.VoucherID
			WHERE  @NeedSettleData=1 and ds.ID = a.ID and isnull(ds.VoucherID,'')!=''			
		) as tmpSum
		where tmpSum.VoucherDate is not null and tmpSum.Amount >= a.Amount+a.Tax
	)AS cur_schedule
" + sqlWhere + ' ' + order);
           #endregion
           #region --撈ScheduleExcel資料--
           cmdSchedule = string.Format(@"select a.ID,a.Status,a.issuedate,a.factoryid, vs1.Name_Extno as Handle, vs2.Name_Extno as SMR,a.LocalSuppID + s.Abb as SupplierByPay,a.TaipeiCurrencyID, 
	   a.TaipeiAMT,a.exchange,a.currencyid,a.amount,a.Tax,a.taxrate,a.amount+a.tax as ttlAmount,vs3.Name_Extno as AmtRN,
	   a.AmtReviseDate,vs4.Name_Extno as AccRH,a.ReceiveDate,vs5.Name_Extno as AccAH,a.cfmdate,
	   cur_schedule.VoucherID[VoucherNo],cur_schedule.VoucherDate,cur_schedule.VoucherDate[Settled Date],
	   c.issuedate,c.amount,c.VoucherId,V.VoucherDate,c.addDate,vs6.Name_Extno as SCN,c.editdate,vs7.Name_Extno as SEN
	  
		from DBO.LocalDebit a 
			inner join dbo.LocalSupp s on a.localsuppid = s.ID
			inner join dbo.debit_schedule c on a.id = c.id
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
			FROM debit_schedule ds 
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
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            if ("Debit Note List".EqualString(this.comboBox1.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R36_DebitNote(LocalSupplier).xltx");
                xl.dicDatas.Add("##SD", dt);
                xl.Save(outpath, false);
            }

            else if ("Summary".EqualString(this.comboBox1.Text))
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
                x1.dicDatas.Add("##SD", dtSummary);
                x1.Save(outpath, false);
            }
            else if ("Detail".EqualString(this.comboBox1.Text))
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
                x1.dicDatas.Add("##SD", dtDetail);
                x1.Save(outpath, false);
            }
            else if ("Debit Schedule Detail".EqualString(this.comboBox1.Text))
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
                x1.dicDatas.Add("##SD", dtSchedule);
                x1.Save(outpath, false);
            }
            return true;
        }
      

    }
}
