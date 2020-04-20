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
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R06
    /// </summary>
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        private DateTime? date1;
        private DateTime? date2;
        private DateTime? apvDate1;
        private DateTime? apvDate2;
        private DateTime? VoucherDate1;
        private DateTime? VoucherDate2;
        private string blno1;
        private string blno2;
        private string supplier;
        private string mDivision;
        private string rateType;
        private int orderby;
        private DataTable printData;

        /// <summary>
        /// R06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateDate.Value1 = new DateTime(DateTime.Now.Year, 1, 1); // 預設帶入登入系統當年的第一天
            this.dateDate.Value2 = DateTime.Today;
            this.dateApvDate.Value2 = DateTime.Today;
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Sci.Env.User.Keyword;

            MyUtility.Tool.SetupCombox(this.comboOrderby, 1, 1, "M,B/L No.");
            MyUtility.Tool.SetupCombox(this.comboRateType, 2, 1, ",Original currency,FX,Fixed exchange rate,KP,KPI exchange rate");
            this.comboOrderby.SelectedIndex = 0;
            this.radioDetail.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDate.Value1) && MyUtility.Check.Empty(this.dateDate.Value2) && MyUtility.Check.Empty(this.dateApvDate.Value1) && MyUtility.Check.Empty(this.dateApvDate.Value2))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            this.mDivision = this.comboM.Text;
            this.date1 = this.dateDate.Value1;
            this.date2 = this.dateDate.Value2;
            this.apvDate1 = this.dateApvDate.Value1;
            this.apvDate2 = this.dateApvDate.Value2;
            this.VoucherDate1 = this.dateVoucherDate.Value1;
            this.VoucherDate2 = this.dateVoucherDate.Value2;
            this.blno1 = this.txtBLNoStart.Text;
            this.blno2 = this.txtBLNoEnd.Text;
            this.supplier = this.txtSubconSupplier.TextBox1.Text;
            this.orderby = this.comboOrderby.SelectedIndex;
            this.rateType = this.comboRateType.SelectedValue.ToString();

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            if (this.radioDetail.Checked == true)
            {
                sqlCmd.Append($@"select	s.Type,
        s.SubType,
		Supplier = s.LocalSuppID+'-'+ISNULL(ls.Abb,''),
		s.ID,
		s.VoucherID,
		s.CDate,
		s.[ApvDate],
		s.[MDivisionID],
		s.[CurrencyID],
		[Amount] = s.[Amount] * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID, 'USD', s.CDate)),
		s.[BLNo],
		s.[Remark],
		s.[InvNo],
		ExportINV =  Stuff((select distinct iif(WKNo = '','',concat( '/',WKNo)) + iif(InvNo = '','',concat( '/',InvNo) )   
                            from ShareExpense she 
                            where she.ShippingAPID = s.ID 
                                  and she.Junk != 1
                            FOR XML PATH('')),1,1,'') ,
		sd.[ShipExpenseID],
		se.Description,
		sd.[Qty],
		se.UnitID,
		sd.[CurrencyID],
		sd.[Price],
		sd.[Rate],
		[Amount] = sd.[Amount] * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID, 'USD', s.CDate)),
		sd.[Remark],
		se.AccountID,
		an.Name
from ShippingAP s WITH (NOLOCK)
inner join ShippingAP_Detail sd WITH (NOLOCK) ON s.ID = sd.ID
left join ShipExpense se  WITH (NOLOCK) ON sd.ShipExpenseID = se.ID
left join SciFMS_AccountNo an on an.ID = se.AccountID 
left join LocalSupp ls WITH (NOLOCK) on s.LocalSuppID = ls.ID
where s.Status = 'Approved'");
            }
            else if (this.radioByInvWK.Checked)
            {
                sqlCmd.Append($@"select
        s.Type,
        s.SubType,
		Supplier = s.LocalSuppID+'-'+ISNULL(ls.Abb,''),
		s.ID,
		s.VoucherID,
        s.VoucherDate,
		[APDate]=s.CDate,
		s.[ApvDate],
		s.[MDivisionID],
		s.[CurrencyID],
		[APAmt]=s.[Amount] * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate)),
		s.[BLNo],
		s.[Remark],
		[Invoice ]= s.InvNo,
		[ExportINV] =  case
                            when sh.WKNo = '' and sh.InvNo != '' then sh.InvNo
                            when sh.WKNo != '' and sh.InvNo = '' then sh.WKNo
                            when sh.WKNo != '' and sh.InvNo != '' then Concat (sh.WKNo, '/', sh.InvNo)
                            else ''
                        end
		,[CurrencyID]= ISNULL(sh.CurrencyID , ShippingAP_Deatai.CurrencyID)
		,[Amount]= ISNULL(sh.Amount , ShippingAP_Deatai.Amount) * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
		,[AccountID]= ISNULL(sh.AccountID , ShippingAP_Deatai.AccountID)
		,[AccountName]= ISNULL(an.Name, ShippingAP_Deatai.AccountName)
from ShippingAP s WITH (NOLOCK)
left join ShareExpense sh WITH (NOLOCK) ON s.ID = sh.ShippingAPID
                                           and sh.Junk != 1
left join SciFMS_AccountNo an on an.ID = sh.AccountID 
left join LocalSupp ls WITH (NOLOCK) on s.LocalSuppID = ls.ID

OUTER APPLY(
	SELECT se.AccountID,[AccountName]=a.Name,sd.CurrencyID,[Amount]=SUM(Amount)
	FROM ShippingAP_Detail sd
	left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
	left join SciFMS_AccountNO a on a.ID = se.AccountID
	WHERE sd.ID=s.ID
	----若ShareExpense有資料，就不必取表身加總的值
	AND NOT EXISTS (SELECT 1 FROM ShareExpense WHERE ShippingAPID=sd.ID and Junk != 1)
	GROUP BY se.AccountID,a.Name,sd.CurrencyID
)ShippingAP_Deatai

where s.Status = 'Approved'");
            }
            else if (this.radioByAPP.Checked)
            {
                sqlCmd.Append($@"
select
        s.Type
        ,s.SubType
		,Supplier = s.LocalSuppID+'-'+ISNULL(ls.Abb,'')
		,s.ID
		,s.VoucherID
        ,s.VoucherDate
		,[APDate]=s.CDate
		,s.[ApvDate]
		,s.[MDivisionID]
		,s.[CurrencyID]
		,[APAmt]=s.[Amount] * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID,'USD', s.CDate))
		,s.[BLNo]
		,s.[Remark]
		,[Invoice ]= s.InvNo
		,[ExportINV] =  sp.InvNo
		,[CurrencyID]= sp.CurrencyID
		,[Amount]= iif(isnull(s.APPExchageRate,0) = 0, 0,
            (isnull(sp.AmtFty,0) + isnull(sp.AmtOther,0)) / iif('{this.rateType}' = '' ,1 ,s.APPExchageRate))
		,[AccountID]= ISNULL(sp.AccountID , ShippingAP_Deatai.AccountID)
		,[AccountName]= ISNULL(an.Name, ShippingAP_Deatai.AccountName)
        ,[SPNO] = air.OrderID
        ,[AirNO] = sp.AirPPID
		,[ShipQty] = isnull(PackingList_Detail.ShipQty,0)
		,[NW] = isnull(sp.NW,0)
		,[shipMode] = p.ShipModeID
from ShippingAP s WITH (NOLOCK)
left join LocalSupp ls WITH (NOLOCK) on s.LocalSuppID = ls.ID
left join ShareExpense_APP sp WITH (NOLOCK) on s.ID = sp.ShippingAPID
											and sp.Junk != 1
left join SciFMS_AccountNo an on an.ID = sp.AccountID 
left join AirPP air WITH (NOLOCK) on sp.AirPPID = air.ID
left join PackingList p WITH (NOLOCK) on sp.PackingListID = p.ID
OUTER APPLY (
	SELECT se.AccountID,[AccountName]=a.Name,sd.CurrencyID,[Amount]=SUM(Amount)
	FROM ShippingAP_Detail sd
	left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
	left join SciFMS_AccountNO a on a.ID = se.AccountID
	WHERE sd.ID=s.ID
	----若ShareExpense有資料，就不必取表身加總的值
	AND NOT EXISTS (SELECT 1 FROM ShareExpense WHERE ShippingAPID=sd.ID and Junk != 1)
	GROUP BY se.AccountID,a.Name,sd.CurrencyID
)ShippingAP_Deatai
OUTER APPLY (
	select [ShipQty] = sum(pd.ShipQty)
	from PackingList_Detail pd
	where pd.ID = sp.PackingListID
		and pd.OrderID = air.OrderID
)PackingList_Detail
where s.Status = 'Approved'");
            }
            else
            {
                sqlCmd.Append($@"
select s.Type
        ,s.SubType
        ,s.LocalSuppID+'-'+ISNULL(l.Abb,'') as Supplier
        ,s.ID
        ,s.VoucherID
        ,s.CDate
        ,CONVERT(DATE,s.ApvDate) as ApvDate
        ,s.MDivisionID
        ,s.CurrencyID
        ,[Amt] = (s.Amount + s.VAT) * iif('{this.rateType}' = '', 1, dbo.getRate('{this.rateType}', s.CurrencyID, 'USD', s.CDate))
        ,s.BLNo
        ,s.Remark
        ,s.InvNo
        ,ExportInv= iif( isnull(x1.InvNo,'') <>'' and isnull(x2.WKNo,'') <>'', x1.InvNo +'/'+x2.WKNo ,concat(x1.InvNo,x2.WKNo))
from ShippingAP s WITH (NOLOCK) 
left join LocalSupp l WITH (NOLOCK) on s.LocalSuppID = l.ID
outer apply(
	select InvNo= stuff((select CONCAT('/',InvNo) 
                 from (
                        select distinct InvNo 
                        from ShareExpense WITH (NOLOCK) 
                        where ShippingAPID = s.ID
                              and junk != 1
                 ) a 
                for xml path('')),1,1,'')
)x1
outer apply(
	select WKNo= stuff((select CONCAT('/',WKNo) 
                 from (
                        select distinct WKNo 
                        from ShareExpense WITH (NOLOCK) 
                        where ShippingAPID = s.ID
                              and junk != 1
                 ) a 
                for xml path('')),1,1,'')
)x2
where s.Status = 'Approved'");
            }

            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCmd.Append(string.Format(" and s.CDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCmd.Append(string.Format(" and s.CDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.apvDate1))
            {
                sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apvDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.apvDate2))
            {
                sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apvDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.VoucherDate1))
            {
                sqlCmd.Append(string.Format(" and CONVERT(DATE,s.VoucherDate) >= '{0}'", Convert.ToDateTime(this.VoucherDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.VoucherDate2))
            {
                sqlCmd.Append(string.Format(" and CONVERT(DATE,s.VoucherDate) <= '{0}'", Convert.ToDateTime(this.VoucherDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.blno1))
            {
                sqlCmd.Append(string.Format(" and s.BLNo >= '{0}'", this.blno1));
            }

            if (!MyUtility.Check.Empty(this.blno2))
            {
                sqlCmd.Append(string.Format(" and s.BLNo <= '{0}'", this.blno2));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.supplier))
            {
                sqlCmd.Append(string.Format(" and s.LocalSuppID = '{0}'", this.supplier));
            }

            if (this.orderby == 0)
            {
                sqlCmd.Append(" order by s.MDivisionID,s.ID");
            }
            else if (this.orderby == 1)
            {
                sqlCmd.Append(" order by s.BLNo,s.ID");
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = string.Empty;

            if (this.radioDetail.Checked)
            {
                strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentListDetail.xltx";
            }
            else if (this.radioByInvWK.Checked)
            {
                strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentListDetailByInvWK.xltx";
            }
            else if (this.radioByAPP.Checked)
            {
                strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentListDetailByAPP.xltx";
            }
            else
            {
                strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentList.xltx";
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            if (this.radioDetail.Checked == true)
            {
                Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentListDetail.xltx", excel);
                if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
                {
                    excel.ActiveWorkbook.Worksheets[1].Cells[1, 10] = excel.ActiveWorkbook.Worksheets[1].Cells[1, 10].Value + "\r\n(USD)";
                    excel.ActiveWorkbook.Worksheets[1].Cells[1, 22] = excel.ActiveWorkbook.Worksheets[1].Cells[1, 22].Value + "\r\n(USD)";
                }

                com.WriteTable(this.printData, 2);
            }
            else if (this.radioByInvWK.Checked == true)
            {
                Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentListDetailByInvWK.xltx", excel);
                if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
                {
                    excel.ActiveWorkbook.Worksheets[1].Cells[1, 11] = excel.ActiveWorkbook.Worksheets[1].Cells[1, 11].Value + "\r\n(USD)";
                    excel.ActiveWorkbook.Worksheets[1].Cells[1, 17] = excel.ActiveWorkbook.Worksheets[1].Cells[1, 17].Value + "\r\n(USD)";
                }

                com.WriteTable(this.printData, 2);
            }
            else if (this.radioByAPP.Checked == true)
            {
                Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentListDetailByAPP.xltx", excel);
                if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
                {
                    excel.ActiveWorkbook.Worksheets[1].Cells[1, 11] = excel.ActiveWorkbook.Worksheets[1].Cells[1, 11].Value + "\r\n(USD)";
                    excel.ActiveWorkbook.Worksheets[1].Cells[1, 17] = excel.ActiveWorkbook.Worksheets[1].Cells[1, 17].Value + "\r\n(USD)";
                }

                com.WriteTable(this.printData, 2);
            }
            else
            {
                // 填內容值
                int intRowsStart = 3;
                object[,] objArray = new object[1, 14];
                foreach (DataRow dr in this.printData.Rows)
                {
                    objArray[0, 0] = dr["Type"];
                    objArray[0, 1] = dr["SubType"];
                    objArray[0, 2] = dr["Supplier"];
                    objArray[0, 3] = dr["ID"];
                    objArray[0, 4] = dr["VoucherID"];
                    objArray[0, 5] = dr["CDate"];
                    objArray[0, 6] = dr["ApvDate"];
                    objArray[0, 7] = dr["MDivisionID"];
                    objArray[0, 8] = dr["CurrencyID"];
                    objArray[0, 9] = dr["Amt"];
                    objArray[0, 10] = dr["BLNo"];
                    objArray[0, 11] = dr["Remark"];
                    objArray[0, 12] = dr["InvNo"];
                    objArray[0, 13] = MyUtility.Check.Empty(dr["ExportInv"]) ? string.Empty : dr["ExportInv"];
                    worksheet.Range[string.Format("A{0}:N{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }

                if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
                {
                    worksheet.Cells[2, 10] = worksheet.Cells[2, 10].Value + "\r\n(USD)";
                }
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(this.radioDetail.Checked == true ? "Shipping_R06_PaymentListDetail" : "Shipping_R06_PaymentList");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
