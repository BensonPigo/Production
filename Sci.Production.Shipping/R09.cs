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

namespace Sci.Production.Shipping
{
    public partial class R09 : Sci.Win.Tems.PrintForm
    {
        DateTime? arrivePortDate1, arrivePortDate2, doxRcvdDate1, doxRcvdDate2, apApvDate1, apApvDate2;
        string shipMode, forwarder;
        int reportType;
        DataTable printData, accnoData;
        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            txtshipmode1.SelectedIndex = -1;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            arrivePortDate1 = dateRange1.Value1;
            arrivePortDate2 = dateRange1.Value2;
            doxRcvdDate1 = dateRange2.Value1;
            doxRcvdDate2 = dateRange2.Value2;
            apApvDate1 = dateRange3.Value1;
            apApvDate2 = dateRange3.Value2;
            shipMode = txtshipmode1.Text;
            forwarder = textBox1.Text;
            reportType = radioButton1.Checked ? 1 : 2;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            if (reportType == 1)
            {
                #region 組SQL Command
                sqlCmd.Append(@"with ExportData
as (
select e.InvNo,'Material' as Type,e.ID as WKNo,'' as FtyWKNo,e.ShipModeID,
e.CYCFS,e.Packages,e.Blno,e.WeightKg,e.Cbm,e.Forwarder+'-'+isnull(supp.AbbEN,'') as Forwarder,
e.PortArrival,e.DocArrival,se.CurrencyID,se.Amount,se.AccountNo
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join Export e on se.WKNo = e.ID
left join Supp on supp.ID = e.Forwarder
where s.Type = 'IMPORT'");
                if (!MyUtility.Check.Empty(arrivePortDate1))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival >= '{0}'", Convert.ToDateTime(arrivePortDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(arrivePortDate2))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival <= '{0}'", Convert.ToDateTime(arrivePortDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(doxRcvdDate1))
                {
                    sqlCmd.Append(string.Format(" and e.DocArrival >= '{0}'", Convert.ToDateTime(doxRcvdDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(doxRcvdDate2))
                {
                    sqlCmd.Append(string.Format(" and e.DocArrival <= '{0}'", Convert.ToDateTime(doxRcvdDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(apApvDate1))
                {
                    sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(apApvDate2))
                {
                    sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(shipMode))
                {
                    sqlCmd.Append(string.Format(" and e.ShipModeID = '{0}'", shipMode));
                }
                if (!MyUtility.Check.Empty(forwarder))
                {
                    sqlCmd.Append(string.Format(" and e.Forwarder = '{0}'", forwarder));
                }

                sqlCmd.Append(@"),
FtyExportData
as (
select fe.InvNo,IIF(fe.Type = 1,'3rd Country',IIF(fe.Type = 2,'Transfer In','Local Purchase')) as Type,'' as WKNo,fe.ID as FtyWKNo,fe.ShipModeID,
fe.CYCFS,fe.Packages,fe.Blno,fe.WeightKg,fe.Cbm,fe.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,
fe.PortArrival,fe.DocArrival,se.CurrencyID,se.Amount,se.AccountNo
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
left join FtyExport fe on se.InvNo = fe.ID
left join LocalSupp ls on ls.ID = fe.Forwarder
where fe.Type <> 3");
                if (!MyUtility.Check.Empty(arrivePortDate1))
                {
                    sqlCmd.Append(string.Format(" and fe.PortArrival >= '{0}'", Convert.ToDateTime(arrivePortDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(arrivePortDate2))
                {
                    sqlCmd.Append(string.Format(" and fe.PortArrival <= '{0}'", Convert.ToDateTime(arrivePortDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(doxRcvdDate1))
                {
                    sqlCmd.Append(string.Format(" and fe.DocArrival >= '{0}'", Convert.ToDateTime(doxRcvdDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(doxRcvdDate2))
                {
                    sqlCmd.Append(string.Format(" and fe.DocArrival <= '{0}'", Convert.ToDateTime(doxRcvdDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(apApvDate1))
                {
                    sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(apApvDate2))
                {
                    sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(shipMode))
                {
                    sqlCmd.Append(string.Format(" and fe.ShipModeID = '{0}'", shipMode));
                }
                if (!MyUtility.Check.Empty(forwarder))
                {
                    sqlCmd.Append(string.Format(" and fe.Forwarder = '{0}'", forwarder));
                }
                #endregion
                string queryAccount = string.Format("{0}{1}", sqlCmd.ToString(), @") select distinct a.* from (
select AccountNo as Accno from ExportData where AccountNo not in ('61012001','61012002','61012003','61012004','61012005')
union
select AccountNo from FtyExportData where AccountNo not in ('61012001','61012002','61012003','61012004','61012005')
) a order by Accno");
                DualResult result = DBProxy.Current.Select(null, queryAccount, out accnoData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
                StringBuilder allAccno = new StringBuilder();
                allAccno.Append("[61012001],[61012002],[61012003],[61012004],[61012005]");
                foreach (DataRow dr in accnoData.Rows)
                {
                    allAccno.Append(string.Format(",[{0}]", MyUtility.Convert.GetString(dr["Accno"])));
                }
                sqlCmd.Append(string.Format(@"),
tmpAllData
as (
select InvNo,Type,WKNo,FtyWKNo,ShipModeID,CYCFS,Packages,Blno,WeightKg,Cbm,Forwarder,
PortArrival,DocArrival,CurrencyID,AccountNo,Amount from ExportData
union all
select InvNo,Type,WKNo,FtyWKNo,ShipModeID,CYCFS,Packages,Blno,WeightKg,Cbm,Forwarder,
PortArrival,DocArrival,CurrencyID,AccountNo,Amount from FtyExportData)

select * from tmpAllData
PIVOT (SUM(Amount)
FOR AccountNo IN ({0})) a", allAccno.ToString()));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
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
e.PortArrival,e.DocArrival,se.AccountNo+'-'+isnull(a.Name,'') as AccountNo,se.Amount,se.CurrencyID,se.ShippingAPID,
s.CDate,s.ApvDate,s.VoucherNo,s.SubType
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
inner join Export e on se.WKNo = e.ID
left join Supp on supp.ID = e.Forwarder
left join [Finance].dbo.AccountNo a on a.ID = se.AccountNo
where s.Type = 'IMPORT'");
                if (!MyUtility.Check.Empty(arrivePortDate1))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival >= '{0}'", Convert.ToDateTime(arrivePortDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(arrivePortDate2))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival <= '{0}'", Convert.ToDateTime(arrivePortDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(doxRcvdDate1))
                {
                    sqlCmd.Append(string.Format(" and e.DocArrival >= '{0}'", Convert.ToDateTime(doxRcvdDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(doxRcvdDate2))
                {
                    sqlCmd.Append(string.Format(" and e.DocArrival <= '{0}'", Convert.ToDateTime(doxRcvdDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(apApvDate1))
                {
                    sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(apApvDate2))
                {
                    sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(shipMode))
                {
                    sqlCmd.Append(string.Format(" and e.ShipModeID = '{0}'", shipMode));
                }
                if (!MyUtility.Check.Empty(forwarder))
                {
                    sqlCmd.Append(string.Format(" and e.Forwarder = '{0}'", forwarder));
                }

                sqlCmd.Append(@"),
FtyExportData
as (
select fe.InvNo,IIF(fe.Type = 1,'3rd Country',IIF(fe.Type = 2,'Transfer In','Local Purchase')) as Type,s.MDivisionID,fe.Consignee,'' as WKNo,fe.ID as FtyWKNo,fe.ShipModeID,
fe.CYCFS,fe.Packages,fe.Blno,fe.WeightKg,fe.Cbm,fe.Forwarder+'-'+isnull(ls.Abb,'') as Forwarder,
fe.PortArrival,fe.DocArrival,se.AccountNo+'-'+isnull(a.Name,'') as AccountNo,se.Amount,se.CurrencyID,se.ShippingAPID,
s.CDate,s.ApvDate,s.VoucherNo,s.SubType
from ShippingAP s
inner join ShareExpense se on se.ShippingAPID = s.ID
left join FtyExport fe on se.InvNo = fe.ID
left join LocalSupp ls on ls.ID = fe.Forwarder
left join [Finance].dbo.AccountNo a on a.ID = se.AccountNo
where fe.Type <> 3");
                if (!MyUtility.Check.Empty(arrivePortDate1))
                {
                    sqlCmd.Append(string.Format(" and fe.PortArrival >= '{0}'", Convert.ToDateTime(arrivePortDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(arrivePortDate2))
                {
                    sqlCmd.Append(string.Format(" and fe.PortArrival <= '{0}'", Convert.ToDateTime(arrivePortDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(doxRcvdDate1))
                {
                    sqlCmd.Append(string.Format(" and fe.DocArrival >= '{0}'", Convert.ToDateTime(doxRcvdDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(doxRcvdDate2))
                {
                    sqlCmd.Append(string.Format(" and fe.DocArrival <= '{0}'", Convert.ToDateTime(doxRcvdDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(apApvDate1))
                {
                    sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apApvDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(apApvDate2))
                {
                    sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apApvDate2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(shipMode))
                {
                    sqlCmd.Append(string.Format(" and fe.ShipModeID = '{0}'", shipMode));
                }
                if (!MyUtility.Check.Empty(forwarder))
                {
                    sqlCmd.Append(string.Format(" and fe.Forwarder = '{0}'", forwarder));
                }
                sqlCmd.Append(@")
select * from ExportData
union all
select * from FtyExportData");
                #endregion
                DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + (reportType == 1?"Shipping_R09_ShareExpenseImportByWK.xltx":"Shipping_R09_ShareExpenseImportByWKByFee.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            if (reportType == 1)
            {
                int i = 0;
                foreach (DataRow dr in accnoData.Rows)
                {
                    i++;
                    worksheet.Cells[1, 19 + i] = MyUtility.GetValue.Lookup(string.Format("select Name from [Finance].dbo.AccountNo where ID = '{0}'", MyUtility.Convert.GetString(dr["Accno"])));
                }
                worksheet.Cells[1, 19 + i + 1] = "Total Import Fee";
                string excelSumCol = PublicPrg.Prgs.GetExcelEnglishColumnName(19 + i);
                string excelColumn = PublicPrg.Prgs.GetExcelEnglishColumnName(19 + i + 1);
                //填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, 19 + i + 1];
                foreach (DataRow dr in printData.Rows)
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
                    foreach (DataRow ddr in accnoData.Rows)
                    {
                        i++;
                        objArray[0, 18 + i] = MyUtility.Check.Empty(dr[18 + i]) ? 0 : dr[18 + i];
                    }
                    objArray[0, 18 + i + 1] = string.Format("=SUM(O{0}:{1}{0})", intRowsStart, excelSumCol);
                    worksheet.Range[String.Format("A{0}:{1}{0}", intRowsStart, excelColumn)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            else
            {
                //填內容值
                int intRowsStart = 2;
                object[,] objArray = new object[1, 23];
                foreach (DataRow dr in printData.Rows)
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
                    objArray[0, 21] = dr["VoucherNo"];
                    objArray[0, 22] = dr["SubType"];

                    worksheet.Range[String.Format("A{0}:W{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            MyUtility.Msg.WaitClear();
            excel.Visible = true;
            return true;
        }

        //Forwarder
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand;
            selectCommand = @"select ID,Abb from LocalSupp
union all
select ID,AbbEN from Supp
order by ID";
            
            DataTable tbSelect;
            DBProxy.Current.Select(null, selectCommand, out tbSelect);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbSelect, "ID,Abb", "9,13", this.Text, false, ",", "ID,Abb");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            IList<DataRow> selected = item.GetSelecteds();
            this.textBox1.Text = item.GetSelectedString();
            displayBox1.Value = MyUtility.Convert.GetString(selected[0]["Abb"]);
        }

        //Forwarder
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.OldValue != textBox1.Text)
            {
                if (!MyUtility.Check.Empty(textBox1.Text))
                {
                    DataRow inputData;
                    string Sql = string.Format(@"select * from (
select ID,Abb from LocalSupp
union all
select ID,AbbEN from Supp) a
where a.ID = '{0}'", textBox1.Text);
                    if (!MyUtility.Check.Seek(Sql, out inputData))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Forwarder: {0} > not found!!!", textBox1.Text));
                        textBox1.Text = "";
                        displayBox1.Value = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        textBox1.Text = textBox1.Text;
                        displayBox1.Value = MyUtility.Convert.GetString(inputData["Abb"]);
                    }
                }
                else
                {
                    textBox1.Text = "";
                    displayBox1.Value = "";
                }
            }
        }
    }
}
