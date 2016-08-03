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
            bool dateRange1_Empty = !this.dateRange_Debit.HasValue, dateRange2_Empty = !this.dateRange_Approve.HasValue, dateRange3_Empty = !this.dateRange3.HasValue, textbox1_Empty = !this.textBox1.Text.Empty(), textbox2_Empty = !this.textBox2.Text.Empty(), txtLocalSupp1_Empty = !this.txtLocalSupp1.TextBox1.Text.Empty()
                , txtuser1_Empty = !this.txtuser1.TextBox1.Text.Empty(), txtuser2_Empty = !this.txtuser2.TextBox1.Text.Empty(), txtfactory1_Empty = !this.txtfactory1.Text.Empty(), comboBox2_Empty = !this.comboBox2.Text.Empty(), comboBox3_Empty = !this.comboBox3.Text.Empty(), dateRange4_Empty = !this.dateRange4.HasValue, dateRange5_Empty = !this.dateRange5.HasValue;

            if (dateRange1_Empty && dateRange2_Empty && dateRange3_Empty && textbox1_Empty && textbox2_Empty && txtLocalSupp1_Empty && txtuser1_Empty && txtuser2_Empty && txtfactory1_Empty && comboBox2_Empty && comboBox3_Empty
               && dateRange4_Empty && dateRange5_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select at least one field entry");

                textBox1.Focus();

                return false;
            }
           
            DateTime? debitdate1 = dateRange_Debit.Value1;
            DateTime? debitdate2 = dateRange_Debit.Value2;
            DateTime? aprdate1 = dateRange_Approve.Value1;
            DateTime? aprdate2 = dateRange_Approve.Value2;
            string SDNo = textBox1.Text.ToString();
            string localsuppid = txtLocalSupp1.TextBox1.Text;
            string handle = txtuser1.TextBox1.Text;
            string smr = txtuser2.TextBox1.Text;
            string status = comboBox2.SelectedItem.ToString();
            string factoryid = txtfactory1.Text;
            DateTime? amtrevisedate1 = dateRange4.Value1;
            DateTime? amtrevisedate2 = dateRange4.Value2;
            DateTime? ReceiveDate1 = dateRange5.Value1;
            DateTime? ReceiveDate2 = dateRange5.Value2;
           lis = new List<SqlParameter>();
           string sqlWhere = ""; string order = "";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.textBox1.Text.Empty())
            {
                sqlWheres.Add("a.Id =@SDNo");
                lis.Add(new SqlParameter("@SDNo", SDNo));
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
                lis.Add(new SqlParameter("@localsuppid", localsuppid));
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
		,a.receivename,a.cfmdate,a.cfmname,D.VoucherID,a.printdate,a.status,a.statuseditdate,
		vs3.Name_Extno as addname,a.adddate,vs4.Name_Extno as edit,a.editdate
		from DBO.LocalDebit a 
			inner join dbo.LocalSupp s on a.localsuppid = s.ID
			left join dbo.Reason R on a.AddName = R.AddName
			left join dbo.Debit_Schedule D on a.ID = D.ID
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.addname ) vs3
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.editname ) vs4" + sqlWhere +' '+ order);
            return base.ValidateInput();
        }
        
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
      
           
          

            //if (comboBox4.SelectedIndex == 1)
            //    cmd += "order by  a.handle";
            //else if (comboBox4.SelectedIndex == 2)
            //    cmd += "order by a.localsuppid";
            //else if (comboBox4.SelectedIndex == 3) 
            //    cmd += "order by  a.id";

            DualResult res;
            res = DBProxy.Current.Select("", cmd, lis, out dt);
            return res;
        }
       
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dt == null || dt.Rows.Count == 0)
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
                x1.dicDatas.Add("##SD", dt);
                x1.Save(outpath, false);
            }
            else if ("Detail".EqualString(this.comboBox1.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R36_DebitNoteDetial(LocalSupplier).xltx");
                x1.dicDatas.Add("##SD", dt);
                x1.Save(outpath, false);
            }
            else if ("Debit Schedule Detail".EqualString(this.comboBox1.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R36_DebitScheduleDetail(LocalSupplier).xltx");
                x1.dicDatas.Add("##SD", dt);
                x1.Save(outpath, false);
            }
            return true;
        }
      

    }
}
