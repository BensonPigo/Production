using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_ProductionOutput_LoadingoutputDetail
    /// </summary>
    public partial class P01_ProductionOutput_LoadingoutputDetail : Sci.Win.Subs.Base
    {
        private string orderID;
        private string type;
        private string article;
        private string sizeCode;

        /// <summary>
        /// P01_ProductionOutput_LoadingoutputDetail
        /// </summary>
        /// <param name="orderID">string orderID</param>
        /// <param name="type">string type</param>
        /// <param name="article">string article</param>
        /// <param name="sizeCode">string sizeCode</param>
        public P01_ProductionOutput_LoadingoutputDetail(string orderID, string type, string article, string sizeCode)
        {
            this.InitializeComponent();
            this.orderID = orderID;
            this.type = type;
            this.article = article;
            this.sizeCode = sizeCode;
            this.Text = "Loading Output Output - " + this.orderID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 設定Grid1的顯示欄位
            this.gridSewingDailyOutput.IsEditingReadOnly = true;
            this.gridSewingDailyOutput.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridSewingDailyOutput)
                 .DateTime("InComing", header: "InComing", width: Widths.AnsiChars(20))
                 .DateTime("OutGoing", header: "OutGoing", width: Widths.AnsiChars(20))
                 .Text("BundleNo", header: "BundleNo", width: Widths.AnsiChars(12))
                 .Text("FabricPanelCode", header: "FabricPanelCode", width: Widths.AnsiChars(2))
                 .Text("Patterncode", header: "PanelCode", width: Widths.AnsiChars(2))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(4));

            string sqlCmd;
            if (this.type == "A")
            {
                sqlCmd = string.Format(
                    @"select BIO.InComing, BIO.OutGoing, BIO.BundleNo, B.FabricPanelCode, BD.Patterncode, BD.Qty
from Bundle B
left join Bundle_Detail BD on BD.Id=B.ID
left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
where Orderid='{0}' and BIO.SubProcessId='LOADING' and isnull(BIO.RFIDProcessLocationID,'') = ''
order by BIO.InComing", this.orderID);
            }
            else
            {
                sqlCmd = string.Format(
                    @"select BIO.InComing, BIO.OutGoing, BIO.BundleNo, B.FabricPanelCode, BD.Patterncode, BD.Qty
from Bundle B
left join Bundle_Detail BD on BD.Id=B.ID
left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
where Orderid='{0}' and BIO.SubProcessId='LOADING' and isnull(BIO.RFIDProcessLocationID,'') = '' and B.Article='{1}' and BD.Sizecode='{2}' 
order by BIO.InComing",
                    this.orderID,
                    this.article,
                    this.sizeCode);
            }

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
