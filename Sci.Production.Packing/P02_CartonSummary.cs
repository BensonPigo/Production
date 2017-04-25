﻿using System;
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
    public partial class P02_CartonSummary : Sci.Win.Subs.Base
    {
        string orderID;
        public P02_CartonSummary( string orderID)
        {
            InitializeComponent();
            this.orderID = orderID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string sqlCmd = string.Format(@"select pd.RefNo, li.LocalSuppid + '-' + ls.Abb as Supplier, li.Description, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension, li.CtnUnit, sum(pd.CTNQty) as TtlCTNQty
from PackingList_Detail pd WITH (NOLOCK) 
left join LocalItem li WITH (NOLOCK) on li.RefNo = pd.RefNo
left join LocalSupp ls WITH (NOLOCK) on ls.ID = li.LocalSuppid
where pd.OrderID = '{0}'
group by pd.RefNo, li.LocalSuppid + '-' + ls.Abb, li.Description, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4), li.CtnUnit", orderID);

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, sqlCmd, out selectDataTable);
            listControlBindingSource1.DataSource = selectDataTable;

            this.gridCartonSummary.IsEditingReadOnly = true;
            this.gridCartonSummary.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.gridCartonSummary)
                 .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(13))
                 .Text("Supplier", header: "Supplier ID", width: Widths.AnsiChars(11))
                 .Text("Description", header: "Description", width: Widths.AnsiChars(20))
                 .Text("Dimension", header: "Dimension", width: Widths.AnsiChars(25))
                 .Text("CTNUnit", header: "Carton Unit")
                 .Numeric("TtlCTNQty", header: "Total Cartons");
        }
    }
}
