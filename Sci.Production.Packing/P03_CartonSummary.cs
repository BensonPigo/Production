using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Packing
{
    public partial class P03_CartonSummary : Sci.Win.Subs.Base
    {
        string packingListID;
        public P03_CartonSummary( string PackingListID)
        {
            InitializeComponent();
            this.packingListID = PackingListID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string sqlCmd = string.Format(@"select pd.RefNo, li.LocalSuppid + '-' + ls.Abb as Supplier, li.Description, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension, li.CtnUnit, sum(pd.CTNQty) as TtlCTNQty
from PackingList_Detail pd
left join LocalItem li on li.RefNo = pd.RefNo
left join LocalSupp ls on ls.ID = li.LocalSuppid
where pd.ID = '{0}'
group by pd.RefNo, li.LocalSuppid + '-' + ls.Abb, li.Description, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4), li.CtnUnit", packingListID);

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, sqlCmd, out selectDataTable);
            listControlBindingSource1.DataSource = selectDataTable;

            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(13))
                 .Text("Supplier", header: "Supplier ID", width: Widths.AnsiChars(11))
                 .Text("Description", header: "Description", width: Widths.AnsiChars(20))
                 .Text("Dimension", header: "Dimension", width: Widths.AnsiChars(25))
                 .Text("CTNUnit", header: "Carton Unit")
                 .Numeric("TtlCTNQty", header: "Total Cartons");
        }
    }
}
