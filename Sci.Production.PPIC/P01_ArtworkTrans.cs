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


namespace Sci.Production.PPIC
{
    public partial class P01_ArtworkTrans : Sci.Win.Subs.Base
    {
        private string orderID;
        public P01_ArtworkTrans(string OrderID)
        {
            InitializeComponent();
            orderID = OrderID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataTable GridData;
            string sqlCmd = string.Format(@"select iif(a.POType = 'O','Subcon','Inhouse') as Type,a.ID,a.IssueDate,l.Abb,a.ArtworkTypeID,
ad.ArtworkId,ad.PatternCode,ad.PoQty,ad.Farmout,ad.Farmin,ad.ApQty
from ArtworkPO a WITH (NOLOCK) , ArtworkPO_Detail ad WITH (NOLOCK) , LocalSupp l WITH (NOLOCK) 
where a.ID = ad.ID
and a.LocalSuppID = l.ID
and ad.OrderID = '{0}'", orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out GridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query artwork trans data fail!!"+result.ToString());
            }

            listControlBindingSource1.DataSource = GridData;
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
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
