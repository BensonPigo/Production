using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P02_BatchPrint : Sci.Win.Tems.PrintForm
    {
        string receiver,incharge,ctnno,orderid,seq1,seq2;
        DataRow masterData;
        DataTable printData;
        public P02_BatchPrint(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("select distinct Receiver from Express_Detail where ID = '{0}'", MyUtility.Convert.GetString(masterData["ID"]));
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "20", textBox1.Text, false, ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox1.Text = item.GetSelectedString();
        }

        protected override bool ValidateInput()
        {
            receiver = textBox1.Text;
            incharge= txtuser1.TextBox1.Text;
            ctnno = textBox2.Text;
            orderid = textBox3.Text;
            seq1 = textBox4.Text;
            seq2 = textBox5.Text;
            ReportResourceName = "P02_BatchPrint.rdlc";

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(@"select OrderID,Seq1,Seq2,BrandID,Receiver,'*'+ID+OrderID+Seq1+Seq2+Category+'*' as BarCode 
from Express_Detail 
where ID = '{0}'
Order by CTNNo,Seq1,seq2", MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Check.Empty(receiver) ? "" : " and Receiver = '" + receiver + "'", MyUtility.Check.Empty(incharge) ? "" : " and InCharge = '" + incharge + "'",
                 MyUtility.Check.Empty(ctnno)?"":" and CTNNo = '"+ctnno+"'",MyUtility.Check.Empty(orderid)?"":" and OrderID = '"+orderid+"'",
                 MyUtility.Check.Empty(seq1)?"":" and Seq1 >= '"+seq1+"'",MyUtility.Check.Empty(seq2)?"":" and Seq1 <= '"+seq2+"'");

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            e.Report.ReportDataSource = printData;
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("hcNo", MyUtility.Convert.GetString(masterData["ID"])));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("awbNo", MyUtility.Convert.GetString(masterData["BLNo"])));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("from", (MyUtility.Convert.GetString(masterData["FromTag"]) == "1" ? "Factory" : "Brand") + "(" + MyUtility.Convert.GetString(masterData["FromSite"])+")"));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("to", (MyUtility.Convert.GetString(masterData["ToTag"]) == "1" ? "SCI" : MyUtility.Convert.GetString(masterData["ToTag"]) == "2" ? "Factory" : MyUtility.Convert.GetString(masterData["ToTag"]) == "3"?"Supplier":"Brand") + "(" + MyUtility.Convert.GetString(masterData["ToSite"]) + ")"));

            return Result.True;
        }

    }
}
