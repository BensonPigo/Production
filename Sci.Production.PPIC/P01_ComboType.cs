using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P01_ComboType : Sci.Win.Tems.QueryForm
    {
        private string OrderID;

        /// <inheritdoc/>
        public P01_ComboType(string id)
        {
            this.InitializeComponent();
            this.OrderID = id;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlcmd = $@"select * from Order_Location where orderid = '{this.OrderID}'";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Location", header: "Combo Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("Rate", header: "Rate(%)", width: Widths.AnsiChars(8), iseditingreadonly: true)
            ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
