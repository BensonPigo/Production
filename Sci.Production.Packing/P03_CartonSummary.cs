using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P03_CartonSummary
    /// </summary>
    public partial class P03_CartonSummary : Win.Subs.Base
    {
        private string packingListID;

        /// <summary>
        /// P03_CartonSummary
        /// </summary>
        /// <param name="packingListID">PackingListID</param>
        public P03_CartonSummary(string packingListID)
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
                @"select pd.RefNo, li.LocalSuppid + '-' + ls.Abb as Supplier, li.Description, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension, li.CtnUnit, sum(pd.CTNQty) as TtlCTNQty
from PackingList_Detail pd WITH (NOLOCK) 
left join LocalItem li WITH (NOLOCK) on li.RefNo = pd.RefNo
left join LocalSupp ls WITH (NOLOCK) on ls.ID = li.LocalSuppid
where pd.ID = '{0}'
group by pd.RefNo, li.LocalSuppid + '-' + ls.Abb, li.Description, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4), li.CtnUnit",
                this.packingListID);

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, sqlCmd, out selectDataTable);
            this.listControlBindingSource1.DataSource = selectDataTable;

            this.gridCartonSummary.IsEditingReadOnly = true;
            this.gridCartonSummary.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridCartonSummary)
                 .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(13))
                 .Text("Supplier", header: "Supplier ID", width: Widths.AnsiChars(11))
                 .Text("Description", header: "Description", width: Widths.AnsiChars(20))
                 .Text("Dimension", header: "Dimension", width: Widths.AnsiChars(25))
                 .Text("CTNUnit", header: "Carton Unit")
                 .Numeric("TtlCTNQty", header: "Total Cartons");
        }
    }
}
