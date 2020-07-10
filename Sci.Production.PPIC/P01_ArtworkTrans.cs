using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_ArtworkTrans
    /// </summary>
    public partial class P01_ArtworkTrans : Win.Subs.Base
    {
        private string orderID;

        /// <summary>
        /// P01_ArtworkTrans
        /// </summary>
        /// <param name="orderID">string OrderID</param>
        public P01_ArtworkTrans(string orderID)
        {
            this.InitializeComponent();
            this.orderID = orderID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataTable gridData;
            string sqlCmd = string.Format(
                @"select iif(a.POType = 'O','Subcon','Inhouse') as Type,a.ID,a.IssueDate,l.Abb,a.ArtworkTypeID,
ad.ArtworkId,ad.PatternCode,ad.PoQty,ad.Farmout,ad.Farmin,ad.ApQty
from ArtworkPO a WITH (NOLOCK) , ArtworkPO_Detail ad WITH (NOLOCK) , LocalSupp l WITH (NOLOCK) 
where a.ID = ad.ID
and a.LocalSuppID = l.ID
and ad.OrderID = '{0}'", this.orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query artwork trans data fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("Type", header: "Type", width: Widths.AnsiChars(7))
                .Text("ID", header: "ID", width: Widths.AnsiChars(15))
                .Date("IssueDate", header: "Issue date", width: Widths.AnsiChars(10))
                .Text("Abb", header: "Supplier", width: Widths.AnsiChars(12))
                .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20))
                .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(20))
                .Text("PatternCode", header: "Cutpart Id", width: Widths.AnsiChars(10))
                .Numeric("PoQty", header: "Qty", width: Widths.AnsiChars(6))
                .Numeric("Farmout", header: "Farm Out", width: Widths.AnsiChars(6))
                .Numeric("Farmin", header: "Farm In", width: Widths.AnsiChars(6))
                .Numeric("ApQty", header: "AP Qty", width: Widths.AnsiChars(6));
        }
    }
}
