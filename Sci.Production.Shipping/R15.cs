using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class R15 : Win.Tems.PrintForm
    {
        private DateTime? dateAddDate1;
        private DateTime? dateAddDate2;
        private DateTime? dateApproveDate1;
        private DateTime? dateApproveDate2;
        private string strBrand;
        private string strStatus;
        private DataTable dtPrintTable;

        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboStatus, 2, 1, ",,New,New,Confirmed,Confirmed");
            this.comboStatus.SelectedIndex = 0;
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            this.dateAddDate1 = this.dateAddDate.Value1;
            this.dateAddDate2 = this.dateAddDate.Value2;
            this.dateApproveDate1 = this.dateApproveDate.Value1;
            this.dateApproveDate2 = this.dateApproveDate.Value2;
            this.strStatus = this.comboStatus.Text;
            this.strBrand = this.txtbrand.Text;
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            StringBuilder sqlcmd = new StringBuilder();
            #region SQL Command
            sqlcmd.Append($@"
select 
sc.ID
,s.Description
,s.BrandID
,s.UnitID
,s.AccountID
,[AccountName] = (select name from SciFMS_AccountNO where id = s.AccountID)
,sc.Status
,s.Junk
,sc.AddDate
,[ApproveDate] = case when sc.Status='Confirmed' then sc.EditDate else null end
,[ApproveBy] = case when sc.Status='Confirmed' then sc.EditName + '-' + (select name from pass1 where id = sc.EditName) else null end
,[Supplier_old] = case when old.ChooseSupp =1 then old.LocalSuppID1
					   when old.ChooseSupp =2 then old.LocalSuppID2
					   when old.ChooseSupp =3 then old.LocalSuppID3
					   when old.ChooseSupp =4 then old.LocalSuppID4 end
,[Currency_old] = case when old.ChooseSupp =1 then old.CurrencyID1
					   when old.ChooseSupp =2 then old.CurrencyID2
					   when old.ChooseSupp =3 then old.CurrencyID3
					   when old.ChooseSupp =4 then old.CurrencyID4 end
,[Price_old] =    case when old.ChooseSupp =1 then old.Price1
					   when old.ChooseSupp =2 then old.Price2
					   when old.ChooseSupp =3 then old.Price3
					   when old.ChooseSupp =4 then old.Price4 end
,[Supplier_New] = case when sc.ChooseSupp =1 then sc.LocalSuppID1
					   when sc.ChooseSupp =2 then sc.LocalSuppID2
					   when sc.ChooseSupp =3 then sc.LocalSuppID3
					   when sc.ChooseSupp =4 then sc.LocalSuppID4 end
,[Currency_New] = case when sc.ChooseSupp =1 then sc.CurrencyID1
					   when sc.ChooseSupp =2 then sc.CurrencyID2
					   when sc.ChooseSupp =3 then sc.CurrencyID3
					   when sc.ChooseSupp =4 then sc.CurrencyID4 end
,[Price_New] =    case when sc.ChooseSupp =1 then sc.Price1
					   when sc.ChooseSupp =2 then sc.Price2
					   when sc.ChooseSupp =3 then sc.Price3
					   when sc.ChooseSupp =4 then sc.Price4 end
,[QuotDate_New] = case when sc.ChooseSupp =1 then sc.QuotDate1
					   when sc.ChooseSupp =2 then sc.QuotDate2
					   when sc.ChooseSupp =3 then sc.QuotDate3
					   when sc.ChooseSupp =4 then sc.QuotDate4 end
,[Supplier_Other1] = iif(sc.ChooseSupp = 1,sc.LocalSuppID2,sc.LocalSuppID1) 
,[Currency_Other1] = iif(sc.ChooseSupp = 1,sc.CurrencyID2,sc.CurrencyID1)
,[Price_Other1] =	 iif(sc.ChooseSupp = 1,sc.Price2,sc.Price1)
,[QuotDate_Other1] = iif(sc.ChooseSupp = 1,sc.QuotDate2,sc.QuotDate1)
,[Supplier_Other2] = iif(sc.ChooseSupp in (3,4),sc.LocalSuppID2,sc.LocalSuppID3)
,[Currency_Other2] = iif(sc.ChooseSupp in (3,4),sc.CurrencyID2,sc.CurrencyID3)
,[Price_Other2] =	 iif(sc.ChooseSupp in (3,4),sc.Price2,sc.Price3)
,[QuotDate_Other2] = iif(sc.ChooseSupp in (3,4),sc.QuotDate2,sc.QuotDate3)
,[Supplier_Other3] = iif(sc.ChooseSupp = 4,sc.LocalSuppID3,sc.LocalSuppID4)
,[Currency_Other3] = iif(sc.ChooseSupp = 4,sc.CurrencyID3,sc.CurrencyID4)
,[Price_Other3] =	 iif(sc.ChooseSupp = 4,sc.Price3,sc.Price4)
,[QuotDate_Other3] = iif(sc.ChooseSupp = 4,sc.QuotDate3,sc.QuotDate4)
from ShipExpense s
inner join ShipExpense_CanVass sc on rtrim(ltrim(s.ID)) = rtrim(ltrim(sc.ID))
outer apply(		    
	select top 1 * 
	from ShipExpense_CanVass t
	where Status='Confirmed'
	and exists(select * from ShipExpense_CanVass where Ukey = sc.Ukey and t.ID=ID AND EditDate > T.EditDate)
	and t.Ukey != sc.Ukey
	order by EditDate desc
)Old				   
where 1=1");

            #region 組Where條件
            if (!this.dateAddDate.Value1.Empty() && !this.dateAddDate.Value2.Empty())
            {
                sqlcmd.Append($@" and CONVERT(date,sc.AddDate) between '{Convert.ToDateTime(this.dateAddDate1).ToString("d")}' and '{Convert.ToDateTime(this.dateAddDate2).ToString("d")}'");
            }

            if (!this.dateApproveDate.Value1.Empty() && !this.dateApproveDate.Value2.Empty())
            {
                sqlcmd.Append($@" and CONVERT(date,sc.EditDate) between '{Convert.ToDateTime(this.dateApproveDate1).ToString("d")}' and '{Convert.ToDateTime(this.dateApproveDate2).ToString("d")}' and sc.Status='Confirmed'");
            }

            if (!MyUtility.Check.Empty(this.strBrand))
            {
                sqlcmd.Append($@" and s.BrandID = '{this.strBrand}'");
            }

            if (!MyUtility.Check.Empty(this.strStatus))
            {
                sqlcmd.Append($@" and sc.Status  = '{this.strStatus}'");
            }

            if (!this.chkJunk.Checked)
            {
                sqlcmd.Append(" and s.junk = 0");
            }

            sqlcmd.Append(" order by sc.ID, sc.AddDate");
            #endregion
            DualResult result;
            if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd.ToString(), out this.dtPrintTable)))
            {
                return result;
            }

            #endregion

            return Ict.Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtPrintTable == null || this.dtPrintTable.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            this.SetCount(this.dtPrintTable.Rows.Count);
            MyUtility.Excel.CopyToXls(this.dtPrintTable, string.Empty, "Shipping_R15.xltx", 2, true, null, null);
            return true;
        }
    }
}
