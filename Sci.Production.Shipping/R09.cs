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
    /// R09
    /// </summary>
    public partial class R09 : Sci.Win.Tems.PrintForm
    {
        private DateTime? arrivePortDate1;
        private DateTime? arrivePortDate2;
        private DateTime? doxRcvdDate1;
        private DateTime? doxRcvdDate2;
        private DateTime? apApvDate1;
        private DateTime? apApvDate2;
        private string shipMode;
        private string forwarder;
        private int reportType;
        private DataTable printData;
        private DataTable accnoData;

        /// <summary>
        /// R09
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.radioListbyWKNo.Checked = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtshipmode.SelectedIndex = -1;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.arrivePortDate1 = this.dateArrivePortDate.Value1;
            this.arrivePortDate2 = this.dateArrivePortDate.Value2;
            this.doxRcvdDate1 = this.dateDoxRcvdDate.Value1;
            this.doxRcvdDate2 = this.dateDoxRcvdDate.Value2;
            this.apApvDate1 = this.dateAPApvDate.Value1;
            this.apApvDate2 = this.dateAPApvDate.Value2;
            this.shipMode = this.txtshipmode.Text;
            this.forwarder = this.txtForwarder.Text;
            this.reportType = this.radioListbyWKNo.Checked ? 1 : 2;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            if (this.reportType == 1)
            {
                #region 組SQL Command
                sqlCmd.Append(@"with ExportData
as (
select e.InvNo,'Material' as Type,e.ID as WKNo,'' as FtyWKNo,e.ShipModeID,
e.CYCFS,e.Packages,e.Blno,e.WeightKg,e.Cbm,e.Forwarder+'-'+isnull(supp.AbbEN,'') as Forwarder,
e.PortArrival,e.DocArrival,se.CurrencyID,se.Amount,se.AccountID
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join Export e WITH (NOLOCK) on se.WKNo = e.ID
left join Supp WITH (NOLOCK) on supp.ID = e.Forwarder
where s.Type = 'IMPORT'");
                if (!MyUtility.Check.Empty(this.arrivePortDate1))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival >= '{0}'", Convert.ToDateTime(this.arrivePortDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.arrivePortDate2))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival <= '{0}'", Convert.ToDateTime(this.arrivePortDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.doxRcvdDate1))
                {
                    sqlCmd.Append(string.Format(" and e.DocArrival >= '{0}'", Convert.ToDateTime(this.doxRcvdDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.doxRcvdDate2))
                {
                    sqlCmd.Append(string.Format(" and e.DocArrival <= '{0}'", Convert.ToDateTime(this.doxRcvdDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.apApvDate1))
                {
                    sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.apApvDate2))
                {
                    sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.shipMode))
                {
                    sqlCmd.Append(string.Format(" and e.ShipModeID = '{0}'", this.shipMode));
                }

                if (!MyUtility.Check.Empty(this.forwarder))
                {
                    sqlCmd.Append(string.Format(" and e.Forwarder = '{0}'", this.forwarder));
                }

                sqlCmd.Append(@"),
FtyExportData
as (
select fe.InvNo,IIF(fe.Type = 1,'3rd Country',IIF(fe.Type = 2,'Transfer In','Local Purchase')) as Type,'' as WKNo,fe.ID as FtyWKNo,fe.ShipModeID,
fe.CYCFS,fe.Packages,fe.Blno,fe.WeightKg,fe.Cbm,fe.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,
fe.PortArrival,fe.DocArrival,se.CurrencyID,se.Amount,se.AccountID
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
left join FtyExport fe WITH (NOLOCK) on se.InvNo = fe.ID
left join LocalSupp ls WITH (NOLOCK) on ls.ID = fe.Forwarder
where fe.Type <> 3");
                if (!MyUtility.Check.Empty(this.arrivePortDate1))
                {
                    sqlCmd.Append(string.Format(" and fe.PortArrival >= '{0}'", Convert.ToDateTime(this.arrivePortDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.arrivePortDate2))
                {
                    sqlCmd.Append(string.Format(" and fe.PortArrival <= '{0}'", Convert.ToDateTime(this.arrivePortDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.doxRcvdDate1))
                {
                    sqlCmd.Append(string.Format(" and fe.DocArrival >= '{0}'", Convert.ToDateTime(this.doxRcvdDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.doxRcvdDate2))
                {
                    sqlCmd.Append(string.Format(" and fe.DocArrival <= '{0}'", Convert.ToDateTime(this.doxRcvdDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.apApvDate1))
                {
                    sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.apApvDate2))
                {
                    sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.shipMode))
                {
                    sqlCmd.Append(string.Format(" and fe.ShipModeID = '{0}'", this.shipMode));
                }

                if (!MyUtility.Check.Empty(this.forwarder))
                {
                    sqlCmd.Append(string.Format(" and fe.Forwarder = '{0}'", this.forwarder));
                }
                #endregion
                string queryAccount = string.Format(
                    "{0}{1}",
                    sqlCmd.ToString(),
                    string.Format(@") select distinct a.* from (
select AccountID as Accno from ExportData where AccountID not in ('61012001','61012002','61012003','61012004','61012005')
union
select AccountID from FtyExportData where AccountID not in ('61012001','61012002','61012003','61012004','61012005')
) a where Accno!='' order by Accno"));
                DualResult result = DBProxy.Current.Select(null, queryAccount, out this.accnoData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                StringBuilder allAccno = new StringBuilder();
                allAccno.Append("[61012001],[61012002],[61012003],[61012004],[61012005]");
                foreach (DataRow dr in this.accnoData.Rows)
                {
                    allAccno.Append(string.Format(",[{0}]", MyUtility.Convert.GetString(dr["Accno"])));
                }

                sqlCmd.Append(string.Format(
                    @"),
tmpAllData
as (
select InvNo,Type,WKNo,FtyWKNo,ShipModeID,CYCFS,Packages,Blno,WeightKg,Cbm,Forwarder,
PortArrival,DocArrival,CurrencyID,AccountID,Amount from ExportData
union all
select InvNo,Type,WKNo,FtyWKNo,ShipModeID,CYCFS,Packages,Blno,WeightKg,Cbm,Forwarder,
PortArrival,DocArrival,CurrencyID,AccountID,Amount from FtyExportData)

select * from tmpAllData
PIVOT (SUM(Amount)
FOR AccountID IN ({0})) a", allAccno.ToString()));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            else
            {
                #region 組SQL Command
                sqlCmd.Append(@"with ExportData
as (
select e.InvNo,'Material' as Type,s.MDivisionID,e.Consignee,e.ID as WKNo,'' as FtyWKNo,e.ShipModeID,
e.CYCFS,e.Packages,e.Blno,e.WeightKg,e.Cbm,e.Forwarder+'-'+isnull(supp.AbbEN,'') as Forwarder,
e.PortArrival,e.DocArrival,se.AccountID+'-'+isnull(a.Name,'') as AccountNo,se.Amount,se.CurrencyID,se.ShippingAPID,
s.CDate,CONVERT(DATE,s.ApvDate) as ApvDate,s.VoucherID,s.SubType
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
inner join Export e WITH (NOLOCK) on se.WKNo = e.ID
left join Supp WITH (NOLOCK) on supp.ID = e.Forwarder
left join [FinanceEN].dbo.AccountNo a on a.ID = se.AccountID
where s.Type = 'IMPORT'");
                if (!MyUtility.Check.Empty(this.arrivePortDate1))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival >= '{0}'", Convert.ToDateTime(this.arrivePortDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.arrivePortDate2))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival <= '{0}'", Convert.ToDateTime(this.arrivePortDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.doxRcvdDate1))
                {
                    sqlCmd.Append(string.Format(" and e.DocArrival >= '{0}'", Convert.ToDateTime(this.doxRcvdDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.doxRcvdDate2))
                {
                    sqlCmd.Append(string.Format(" and e.DocArrival <= '{0}'", Convert.ToDateTime(this.doxRcvdDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.apApvDate1))
                {
                    sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.apApvDate2))
                {
                    sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.shipMode))
                {
                    sqlCmd.Append(string.Format(" and e.ShipModeID = '{0}'", this.shipMode));
                }

                if (!MyUtility.Check.Empty(this.forwarder))
                {
                    sqlCmd.Append(string.Format(" and e.Forwarder = '{0}'", this.forwarder));
                }

                sqlCmd.Append(@"),
FtyExportData
as (
select fe.InvNo,IIF(fe.Type = 1,'3rd Country',IIF(fe.Type = 2,'Transfer In','Local Purchase')) as Type,s.MDivisionID,fe.Consignee,'' as WKNo,fe.ID as FtyWKNo,fe.ShipModeID,
fe.CYCFS,fe.Packages,fe.Blno,fe.WeightKg,fe.Cbm,fe.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,
fe.PortArrival,fe.DocArrival,se.AccountID+'-'+isnull(a.Name,'') as AccountNo,se.Amount,se.CurrencyID,se.ShippingAPID,
s.CDate,CONVERT(DATE,s.ApvDate) as ApvDate,s.VoucherID,s.SubType
from ShippingAP s WITH (NOLOCK) 
inner join ShareExpense se WITH (NOLOCK) on se.ShippingAPID = s.ID
left join FtyExport fe WITH (NOLOCK) on se.InvNo = fe.ID
left join LocalSupp ls WITH (NOLOCK) on ls.ID = fe.Forwarder
left join [FinanceEN].dbo.AccountNo a on a.ID = se.AccountID
where fe.Type <> 3");
                if (!MyUtility.Check.Empty(this.arrivePortDate1))
                {
                    sqlCmd.Append(string.Format(" and fe.PortArrival >= '{0}'", Convert.ToDateTime(this.arrivePortDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.arrivePortDate2))
                {
                    sqlCmd.Append(string.Format(" and fe.PortArrival <= '{0}'", Convert.ToDateTime(this.arrivePortDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.doxRcvdDate1))
                {
                    sqlCmd.Append(string.Format(" and fe.DocArrival >= '{0}'", Convert.ToDateTime(this.doxRcvdDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.doxRcvdDate2))
                {
                    sqlCmd.Append(string.Format(" and fe.DocArrival <= '{0}'", Convert.ToDateTime(this.doxRcvdDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.apApvDate1))
                {
                    sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apApvDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.apApvDate2))
                {
                    sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apApvDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.shipMode))
                {
                    sqlCmd.Append(string.Format(" and fe.ShipModeID = '{0}'", this.shipMode));
                }

                if (!MyUtility.Check.Empty(this.forwarder))
                {
                    sqlCmd.Append(string.Format(" and fe.Forwarder = '{0}'", this.forwarder));
                }

                sqlCmd.Append(@")
select * from ExportData
union all
select * from FtyExportData");
                #endregion
                DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
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
            string strXltName = Sci.Env.Cfg.XltPathDir + (this.reportType == 1 ? "\\Shipping_R09_ShareExpenseImportByWK.xltx" : "\\Shipping_R09_ShareExpenseImportByWKByFee.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            if (this.reportType == 1)
            {
                int i = 0;
                foreach (DataRow dr in this.accnoData.Rows)
                {
                    i++;
                    worksheet.Cells[1, 19 + i] = MyUtility.GetValue.Lookup(string.Format("select Name from [FinanceEN].dbo.AccountNo where ID = '{0}'", MyUtility.Convert.GetString(dr["Accno"])));
                }

                worksheet.Cells[1, 19 + i + 1] = "Total Import Fee";
                string excelSumCol = PublicPrg.Prgs.GetExcelEnglishColumnName(19 + i);
                string excelColumn = PublicPrg.Prgs.GetExcelEnglishColumnName(19 + i + 1);

                // 填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, 19 + i + 1];
                foreach (DataRow dr in this.printData.Rows)
                {
                    objArray[0, 0] = dr["InvNo"];
                    objArray[0, 1] = dr["Type"];
                    objArray[0, 2] = dr["WKNo"];
                    objArray[0, 3] = dr["FtyWKNo"];
                    objArray[0, 4] = dr["ShipModeID"];
                    objArray[0, 5] = dr["CYCFS"];
                    objArray[0, 6] = dr["Packages"];
                    objArray[0, 7] = dr["Blno"];
                    objArray[0, 8] = dr["WeightKg"];
                    objArray[0, 9] = dr["Cbm"];
                    objArray[0, 10] = dr["Forwarder"];
                    objArray[0, 11] = dr["PortArrival"];
                    objArray[0, 12] = dr["DocArrival"];
                    objArray[0, 13] = dr["CurrencyID"];
                    objArray[0, 14] = MyUtility.Check.Empty(dr["61012001"]) ? 0 : dr["61012001"];
                    objArray[0, 15] = MyUtility.Check.Empty(dr["61012002"]) ? 0 : dr["61012002"];
                    objArray[0, 16] = MyUtility.Check.Empty(dr["61012003"]) ? 0 : dr["61012003"];
                    objArray[0, 17] = MyUtility.Check.Empty(dr["61012004"]) ? 0 : dr["61012004"];
                    objArray[0, 18] = MyUtility.Check.Empty(dr["61012005"]) ? 0 : dr["61012005"];
                    i = 0;
                    foreach (DataRow ddr in this.accnoData.Rows)
                    {
                        i++;
                        objArray[0, 18 + i] = MyUtility.Check.Empty(dr[18 + i]) ? 0 : dr[18 + i];
                    }

                    objArray[0, 18 + i + 1] = string.Format("=SUM(O{0}:{1}{0})", intRowsStart, excelSumCol);
                    worksheet.Range[string.Format("A{0}:{1}{0}", intRowsStart, excelColumn)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            else
            {
                // 填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, 23];
                foreach (DataRow dr in this.printData.Rows)
                {
                    objArray[0, 0] = dr["InvNo"];
                    objArray[0, 1] = dr["Type"];
                    objArray[0, 2] = dr["MDivisionID"];
                    objArray[0, 3] = dr["Consignee"];
                    objArray[0, 4] = dr["WKNo"];
                    objArray[0, 5] = dr["FtyWKNo"];
                    objArray[0, 6] = dr["ShipModeID"];
                    objArray[0, 7] = dr["CYCFS"];
                    objArray[0, 8] = dr["Packages"];
                    objArray[0, 9] = dr["Blno"];
                    objArray[0, 10] = dr["WeightKg"];
                    objArray[0, 11] = dr["Cbm"];
                    objArray[0, 12] = dr["Forwarder"];
                    objArray[0, 13] = dr["PortArrival"];
                    objArray[0, 14] = dr["DocArrival"];
                    objArray[0, 15] = dr["AccountNo"];
                    objArray[0, 16] = dr["Amount"];
                    objArray[0, 17] = dr["CurrencyID"];
                    objArray[0, 18] = dr["ShippingAPID"];
                    objArray[0, 19] = dr["CDate"];
                    objArray[0, 20] = dr["ApvDate"];
                    objArray[0, 21] = dr["VoucherID"];
                    objArray[0, 22] = dr["SubType"];

                    worksheet.Range[string.Format("A{0}:W{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(this.reportType == 1 ? "Shipping_R09_ShareExpenseImportByWK" : "Shipping_R09_ShareExpenseImportByWKByFee");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        // Forwarder
        private void TxtForwarder_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand;
            selectCommand = @"select ID,Abb from LocalSupp WITH (NOLOCK) 
union all
select ID,AbbEN from Supp WITH (NOLOCK) 
order by ID";

            DataTable tbSelect;
            DBProxy.Current.Select(null, selectCommand, out tbSelect);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbSelect, "ID,Abb", "9,13", this.Text, false, ",", "ID,Abb");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selected = item.GetSelecteds();
            this.txtForwarder.Text = item.GetSelectedString();
            this.displayBox1.Value = MyUtility.Convert.GetString(selected[0]["Abb"]);
        }

        // Forwarder
        private void TxtForwarder_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtForwarder.OldValue != this.txtForwarder.Text)
            {
                if (!MyUtility.Check.Empty(this.txtForwarder.Text))
                {
                    DataRow inputData;
                    string sql = string.Format(
                        @"select * from (
select ID,Abb from LocalSupp WITH (NOLOCK) 
union all
select ID,AbbEN from Supp WITH (NOLOCK) ) a
where a.ID = '{0}'", this.txtForwarder.Text);
                    if (!MyUtility.Check.Seek(sql, out inputData))
                    {
                        this.txtForwarder.Text = string.Empty;
                        this.displayBox1.Value = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Forwarder: {0} > not found!!!", this.txtForwarder.Text));
                        return;
                    }
                    else
                    {
                        this.txtForwarder.Text = this.txtForwarder.Text;
                        this.displayBox1.Value = MyUtility.Convert.GetString(inputData["Abb"]);
                    }
                }
                else
                {
                    this.txtForwarder.Text = string.Empty;
                    this.displayBox1.Value = string.Empty;
                }
            }
        }
    }
}
