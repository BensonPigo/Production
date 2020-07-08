using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// P01_DetailData
    /// </summary>
    public partial class P01_DetailData : Win.Subs.Base
    {
        /// <summary>
        /// P01_DetailData
        /// </summary>
        /// <param name="showDt">showDt</param>
        public P01_DetailData(DataTable showDt)
        {
            this.InitializeComponent();
            this.listControlBindingSource1.DataSource = showDt;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("ID", header: "SP#", iseditingreadonly: true)
                .Text("Category", header: "Category", iseditingreadonly: true)
                .Text("EType", header: "EType", iseditingreadonly: true)
                .Text("FactoryID", header: "Factory", iseditingreadonly: true)
                .Text("BrandID", header: "Brand", iseditingreadonly: true)
                .Text("StyleID", header: "Style", iseditingreadonly: true)
                .Text("SeasonID", header: "Season", iseditingreadonly: true)
                .Text("CdCodeID", header: "CD Code", iseditingreadonly: true)
                .Text("CPU", header: "CPU/PC", iseditingreadonly: true)
                .Text("CPU_EType", header: "CPU/EType", iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", iseditingreadonly: true)
                .Text("SciDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Text("BuyerDelivery", header: "Buyer" + Environment.NewLine + "Delivery", iseditingreadonly: true)
                .Text("Multi_Delivery", header: "Multi-Delivery", iseditingreadonly: true)
                .EditText("QtyDelivery", header: "Qty-Delivery", iseditingreadonly: true)
                .Text("SewInLine", header: "Inline", iseditingreadonly: true)
                .Text("SewOffLine", header: "Offline", iseditingreadonly: true)
                .EditText("LineNum", header: "Line#", iseditingreadonly: true)
                .Text("SewingComplete", header: "Sewing" + Environment.NewLine + "Complete", iseditingreadonly: true)
                .Text("PdnDays", header: "Pdn. Days", iseditingreadonly: true)
                .Text("StdOutput", header: "Std. Output" + Environment.NewLine + "(A)", iseditingreadonly: true)
                .Text("DaysSinceInline", header: "Days since" + Environment.NewLine + "Inline", iseditingreadonly: true)
                .Text("AccuStdOutput", header: "Accu. Std." + Environment.NewLine + "Output (B)", iseditingreadonly: true)
                .Text("AccuActOutput", header: "Accu. Act." + Environment.NewLine + "Output (C)", iseditingreadonly: true)
                .Numeric("VarianceQty", header: "Variance Qty" + Environment.NewLine + "(C-B)", iseditingreadonly: true)
                .Text("VarianceDays", header: "Variance Days" + Environment.NewLine + "((C-B)/A)", iseditingreadonly: true)
                .Text("DaysToDelivery", header: "Days to" + Environment.NewLine + "Delivery (D)", iseditingreadonly: true)
                .Text("DaysNeedForProd", header: "Days need for" + Environment.NewLine + "Prod. (E)", iseditingreadonly: true)
                .Text("PostSewingDays", header: "Post Sewing" + Environment.NewLine + "Days (F)", iseditingreadonly: true)
                .Text("Variance", header: "Variance" + Environment.NewLine + "(D-E-F)", iseditingreadonly: true)
                .Text("PotentialDelayRisk", header: "Potential" + Environment.NewLine + "Delay Risk", iseditingreadonly: true)
                .Text("SMR", header: "SMR", iseditingreadonly: true)
                .Text("MRHandle", header: "MR", iseditingreadonly: true);
            this.grid1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <inheritdoc/>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            this.grid1.DataSource = null;
            this.listControlBindingSource1.DataSource = null;
        }
    }
}
