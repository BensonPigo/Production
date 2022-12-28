using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P04_CartonSummary
    /// </summary>
    public partial class P04_CartonSummary : Win.Subs.Base
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
                @" 
select pd.RefNo
	, li.Description
	, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension
	, li.CtnUnit
	, sum(pd.CTNQty) as CTNQty
	, pd.QtyPerCTN
	, isnull(sum(pd.ShipQty),0) as ShipQty
from PackingList_Detail pd WITH (NOLOCK) 
left join LocalItem li WITH (NOLOCK) on li.RefNo = pd.RefNo
where pd.ID = '{0}'
group by pd.RefNo
	, li.Description
	, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4)
	, li.CtnUnit
	, pd.QtyPerCTN", this.packingListID);

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
