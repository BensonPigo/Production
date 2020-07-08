using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_ProductionOutput_SewingDetail
    /// </summary>
    public partial class P01_ProductionOutput_SewingDetail : Sci.Win.Subs.Base
    {
        private string orderID;
        private string type;
        private string article;
        private string sizeCode;

        /// <summary>
        /// P01_ProductionOutput_SewingDetail
        /// </summary>
        /// <param name="orderID">string orderID</param>
        /// <param name="type">string type</param>
        /// <param name="article">string article</param>
        /// <param name="sizeCode">string sizeCode</param>
        public P01_ProductionOutput_SewingDetail(string orderID, string type, string article, string sizeCode)
        {
            this.InitializeComponent();
            this.orderID = orderID;
            this.type = type;
            this.article = article;
            this.sizeCode = sizeCode;
            if (this.type == "A")
            {
                this.Text = "Sewing Daily Output - " + this.orderID;
            }
            else
            {
                this.Text = "Sewing Daily Output - " + this.orderID + "(" + this.article + "-" + this.sizeCode + ")";
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 設定Grid1的顯示欄位
            this.gridSewingDailyOutput.IsEditingReadOnly = true;
            this.gridSewingDailyOutput.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridSewingDailyOutput)
                 .Date("OutputDate", header: "Date", width: Widths.AnsiChars(12))
                 .Text("SewingLineID", header: "Line#", width: Widths.AnsiChars(5))
                 .Text("ComboType", header: "*", width: Widths.AnsiChars(2))
                 .Numeric("QAQty", header: "Q'ty", width: Widths.AnsiChars(6));

            string sqlCmd;
            if (this.type == "A")
            {
                sqlCmd = string.Format(
                    @"select s.OutputDate,s.SewingLineID,sd.ComboType,sum(sd.QAQty) as QAQty
from SewingOutput_Detail sd WITH (NOLOCK) 
inner join SewingOutput s WITH (NOLOCK) on sd.ID = s.ID
where sd.OrderId = '{0}'
group by s.OutputDate,s.SewingLineID,sd.ComboType", this.orderID);
            }
            else if (this.type == "S")
            {
                sqlCmd = string.Format(
                    @"select s.OutputDate,s.SewingLineID,sdd.ComboType,sum(sdd.QAQty) as QAQty
from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
inner join SewingOutput s WITH (NOLOCK) on sdd.ID = s.ID
where sdd.OrderId = '{0}'
and sdd.Article = '{1}'
and sdd.SizeCode = '{2}'
group by s.OutputDate,s.SewingLineID,sdd.ComboType",
                    this.orderID,
                    this.article,
                    this.sizeCode);
            }
            else
            {
                sqlCmd = string.Format(
                    @"select s.OutputDate,s.SewingLineID,sdd.ComboType,sum(sdd.QAQty) as QAQty
from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
inner join SewingOutput s WITH (NOLOCK) on sdd.ID = s.ID
where sdd.OrderId = '{0}'
and sdd.Article = '{1}'
and sdd.SizeCode = '{2}'
and sdd.ComboType = '{3}'
group by s.OutputDate,s.SewingLineID,sdd.ComboType",
                    this.orderID,
                    this.article,
                    this.sizeCode,
                    this.type);
            }

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
