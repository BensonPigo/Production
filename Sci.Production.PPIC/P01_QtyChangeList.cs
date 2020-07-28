using Sci.Data;
using Sci.Win.Tems;
using System.Collections.Generic;
using System.Data;
using Ict.Win;
using Ict;
using System.Data.SqlClient;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P01_QtyChangeList : QueryForm
    {
        private string orderid;

        /// <inheritdoc/>
        public P01_QtyChangeList(string orderid)
        {
            this.InitializeComponent();
            this.SetGrid();
            this.orderid = orderid;
        }

        private void SetGrid()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(14))
                .Text("ID", header: "Order Application", width: Widths.AnsiChars(14))
                .Text("Reason", header: "Reason", width: Widths.AnsiChars(30))
                .Text("ToSP", header: "To New SP#", width: Widths.AnsiChars(14))
                .Text("NeedProduction", header: "Cancel Still need prod.", width: Widths.AnsiChars(4))
                .Text("FromSP", header: "From SP#", width: Widths.AnsiChars(14))
                ;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable dt = new DataTable();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@orderid", this.orderid));

            string cmd = @"
SELECT OrderID, ID, rs.Name as Reason, iif(NeedProduction = 1, 'V', '') as NeedProduction, oa.ToSP, oa.FromSP
FROM (
    select OrderID, ID, ReasonID, NeedProduction, ToOrderID as ToSP, '' as FromSP from OrderChangeApplication
    where OrderID = @orderid and not (Status='Junk'or isnull(GMCheck,0)=1)
    UNION
    select ToOrderID, ID, ReasonID, NeedProduction, '' as ToSP, OrderID as FromSP from OrderChangeApplication
    where ToOrderID = @orderid and not (Status='Junk'or isnull(GMCheck,0)=1)
) oa
OUTER APPLY (SELECT ID + '-' + Name as Name FROM Reason WHERE ReasonTypeID = 'OMQtychange' AND ID = oa.ReasonID) rs
order by oa.ID";

            DualResult result = DBProxy.Current.Select("Production", cmd, parameters, out dt);
            if (result == false)
            {
                this.ShowErr(result);
            }

            this.grid.DataSource = dt;
        }
    }
}
