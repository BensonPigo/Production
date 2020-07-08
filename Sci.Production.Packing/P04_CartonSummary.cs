using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P04_CartonSummary
    /// </summary>
    public partial class P04_CartonSummary : Sci.Win.Subs.Base
    {
        private string packingListID;

        /// <summary>
        /// P04_CartonSummary
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        public P04_CartonSummary(string packingListID)
        {
            this.InitializeComponent();
            this.packingListID = packingListID;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string sqlCmd = string.Format(
                @"with PackData
as
(select distinct OrderID 
 from PackingList_Detail WITH (NOLOCK) 
 where id = '{0}'
),
SummaryData
as
(
select li.RefNo,pld.QtyPerCTN,li.Description,li.CtnUnit,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension, isnull(sum(pld.ShipQty),0) as ShipQty, isnull(sum(pld.CTNQty),0) as CTNQty
from PackData pd
left join PackingList_Detail pld WITH (NOLOCK) on pld.OrderID = pd.OrderID
left join LocalItem li WITH (NOLOCK) on li.RefNo = pld.RefNo
group by li.RefNo,pld.QtyPerCTN,li.Description,li.CtnUnit,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4)
)
select * from SummaryData where CTNQty > 0 and RefNo is not null", this.packingListID);

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, sqlCmd, out selectDataTable);
            this.listControlBindingSource1.DataSource = selectDataTable;

            this.gridCartonSummary.IsEditingReadOnly = true;
            this.gridCartonSummary.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridCartonSummary)
                 .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(13))
                 .Text("Description", header: "Description", width: Widths.AnsiChars(20))
                 .Text("Dimension", header: "Dimension", width: Widths.AnsiChars(25))
                 .Text("CTNUnit", header: "Carton Unit")
                 .Numeric("QtyPerCTN", header: "Q'ty/Carton(A)")
                 .Numeric("ShipQty", header: "Gmt Q'ty")
                 .Numeric("CTNQty", header: "Carton Q'ty");
        }
    }
}
