using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02_BatchPrint
    /// </summary>
    public partial class P02_BatchPrint : Win.Tems.PrintForm
    {
        private string receiver;
        private string incharge;
        private string ctnno;
        private string orderid;
        private string seq1;
        private string seq2;
        private DataRow masterData;
        private DataTable printData;

        /// <summary>
        /// P02_BatchPrint
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P02_BatchPrint(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
        }

        private void TxtReceiver_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("select distinct Receiver from Express_Detail WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ID"]));
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "20", this.txtReceiver.Text, false, ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtReceiver.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.receiver = this.txtReceiver.Text;
            this.incharge = this.txtUserInCharge.TextBox1.Text;
            this.ctnno = this.txtCNo.Text;
            this.orderid = this.txtSPNo.Text;
            this.seq1 = this.txtSeqStart.Text;
            this.seq2 = this.txtSeqEnd.Text;
            this.ReportResourceName = "P02_BatchPrint.rdlc";

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(
                @"select OrderID,Seq1,Seq2,BrandID,Receiver,'*'+ID+OrderID+Seq1+Seq2+Category+'*' as BarCode 
from Express_Detail WITH (NOLOCK) 
where ID = '{0}'
Order by CTNNo,Seq1,seq2", MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Check.Empty(this.receiver) ? string.Empty : " and Receiver = '" + this.receiver + "'",
                MyUtility.Check.Empty(this.incharge) ? string.Empty : " and InCharge = '" + this.incharge + "'",
                MyUtility.Check.Empty(this.ctnno) ? string.Empty : " and CTNNo = '" + this.ctnno + "'",
                MyUtility.Check.Empty(this.orderid) ? string.Empty : " and OrderID = '" + this.orderid + "'",
                MyUtility.Check.Empty(this.seq1) ? string.Empty : " and Seq1 >= '" + this.seq1 + "'",
                MyUtility.Check.Empty(this.seq2) ? string.Empty : " and Seq1 <= '" + this.seq2 + "'");

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            e.Report.ReportDataSource = this.printData;
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("hcNo", MyUtility.Convert.GetString(this.masterData["ID"])));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("awbNo", MyUtility.Convert.GetString(this.masterData["BLNo"])));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("from", (MyUtility.Convert.GetString(this.masterData["FromTag"]) == "1" ? "Factory" : "Brand") + "(" + MyUtility.Convert.GetString(this.masterData["FromSite"]) + ")"));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("to", (MyUtility.Convert.GetString(this.masterData["ToTag"]) == "1" ? "SCI" : MyUtility.Convert.GetString(this.masterData["ToTag"]) == "2" ? "Factory" : MyUtility.Convert.GetString(this.masterData["ToTag"]) == "3" ? "Supplier" : "Brand") + "(" + MyUtility.Convert.GetString(this.masterData["ToSite"]) + ")"));

            return Ict.Result.True;
        }
    }
}
