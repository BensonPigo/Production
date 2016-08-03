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
            bool dateRange1 = !this.dateRange1.HasValue, dateRange2 = !this.dateRange2.HasValue, dateRange3 = !this.dateRange3.HasValue, textbox1 = !this.textBox1.Text.Empty(), textbox2 = !this.textBox2.Text.Empty(), txtLocalSupp1 = !this.txtLocalSupp1.TextBox1.Text.Empty()
                , txtuser1 = !this.txtuser1.TextBox1.Text.Empty(), txtuser2 = !this.txtuser2.TextBox1.Text.Empty(), txtfactory1 = !this.txtfactory1.Text.Empty(), comboBox2 = !this.comboBox2.Text.Empty(), comboBox3 = !this.comboBox3.Text.Empty(), dateRange4 = !this.dateRange4.HasValue, dateRange5 = !this.dateRange5.HasValue;

            if (dateRange1 || dateRange2 || dateRange3 || textbox1 || textbox2 || txtLocalSupp1 || txtuser1 || txtuser2 || txtfactory1 || comboBox2 || comboBox3
               || dateRange4 || dateRange5)
            {
                MyUtility.Msg.ErrorBox("Please select at least one field entry");

                textBox1.Focus();

                return false;
            }


            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {





            return Result.True;
        }

        protected override bool ToExcel()
        {

            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }
            DateTime? debitdate1 = dateRange1.Value1;
            DateTime? debitdate2 = dateRange1.Value2;
            DateTime? aprdate1 = dateRange2.Value1;
            DateTime? aprdate2 = dateRange2.Value2;
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

            List<SqlParameter> lis = new List<SqlParameter>();
            string sqlWhere = "";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!this.textBox1.Text.Empty())
            {
                sqlWheres.Add("a.Id =@SDNo");
                lis.Add(new SqlParameter("@SDNo", SDNo));
            } 
            if (!this.dateRange1.Value1.Empty() && !this.dateRange1.Value2.Empty())
            {
                sqlWheres.Add("a.Issuedate between @debitdate1 and @debitdate2");
                lis.Add(new SqlParameter("@debitdate1", debitdate1));
                lis.Add(new SqlParameter("@debitdate2", debitdate2));
            } if (!this.dateRange1.Value1.Empty() && !this.dateRange1.Value2.Empty())
            {
                sqlWheres.Add("a.apvdate between @debitdate1 and @debitdate2");
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
            } if (!this.dateRange1.Value1.Empty() && !this.dateRange1.Value2.Empty())
            {
                sqlWheres.Add("a.apvdate between @debitdate1 and @debitdate2");
                lis.Add(new SqlParameter("@aprdate1", aprdate1));
                lis.Add(new SqlParameter("@aprdate2", aprdate2));
            } if (!this.dateRange1.Value1.Empty() && !this.dateRange1.Value2.Empty())
            {
                sqlWheres.Add("a.apvdate between @debitdate1 and @debitdate2");
                lis.Add(new SqlParameter("@aprdate1", aprdate1));
                lis.Add(new SqlParameter("@aprdate2", aprdate2));
            }
            #endregion

         
          
            lis.Add(new SqlParameter("@smr", smr));
            lis.Add(new SqlParameter("@status", status));
            lis.Add(new SqlParameter("@amtrevisedate1", amtrevisedate1));
            lis.Add(new SqlParameter("@amtrevisedate2", amtrevisedate2));
            lis.Add(new SqlParameter("@ReceiveDate1", ReceiveDate1));
            lis.Add(new SqlParameter("@ReceiveDate2", ReceiveDate2));
            lis.Add(new SqlParameter("@factoryid", factoryid));
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }
            string cmd = @"
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
			inner join LocalDebit_Detail b on a.id = b.id
			inner join dbo.LocalSupp s on a.localsuppid = s.ID
			left join dbo.Reason R on b.AddName = R.AddName
			left join dbo.Debit_Schedule D on a.ID = D.ID
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.Handle ) vs1
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.SMR ) vs2
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.addname ) vs3
			outer apply (select * from dbo.View_ShowName vs where vs.id = a.editname ) vs4;
		where (@SDNo = '' or a.ID = @SDNo) 
				
		and (@handle = '' or handle = @handle)
		and (@smr = '' or smr = @smr)
		and (@factoryid= '' or factoryid = @factoryid)
		and (@status = '' or Status = @status)
		and (@amtrevisedate1 is null or @amtrevisedate1 between @amtrevisedate1 and @amtrevisedate2)
		and (@ReceiveDate1 is null or @ReceiveDate1 between @ReceiveDate1 and @ReceiveDate2)
";
            if (comboBox4.SelectedIndex == 1)
                cmd += "order by  a.handle";
            else if (comboBox4.SelectedIndex == 2)
                cmd += "order by a.localsuppid";
            else cmd += "order by  a.id";

            DataTable dt;
            DualResult res = DBProxy.Current.Select("", cmd, lis, out dt);


            string xlt = @"Subcon_R36_DebitNote(LocalSupplier).xltx";
            SaveXltReportCls xl = new SaveXltReportCls(xlt);

            xl.dicDatas.Add("##name", dt);
            xl.Save(outpath, true);
            return base.ToExcel();
        }

    }
}
