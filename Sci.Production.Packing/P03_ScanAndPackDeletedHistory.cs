﻿using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class P03_ScanAndPackDeletedHistory : Sci.Win.Tems.Base
    {
        private string PackingListID = string.Empty;

        public P03_ScanAndPackDeletedHistory(string _PackingListID)
        {
            InitializeComponent();
            this.PackingListID = _PackingListID;
        }

        protected override void OnFormLoaded()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("DeleteFrom", header: "Delete From", width: Widths.AnsiChars(10))
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6))
                .Text("OrderID", header: "Order ID", width: Widths.AnsiChars(18))
                .Numeric("ScanQty", header: "Scan Qty", width: Widths.AnsiChars(6) ,decimal_places:0)
                .Text("ScanName", header: "Scan Name", width: Widths.AnsiChars(25))
                .DateTime("ScanDate", header: "Scan Date", width: Widths.AnsiChars(25))
                .Numeric("LackingQty", header: "Lacking Qty", width: Widths.AnsiChars(6), decimal_places: 0)
                .Text("DeletedBy", header: "Deleted By", width: Widths.AnsiChars(25))
                .DateTime("DeletedDate", header: "Deleted Date", width: Widths.AnsiChars(25));

            DataTable dt;
            string cmd = $@"
SELECT 
	 [DeleteFrom]=ph.DeleteFrom
	,ph.CTNStartNo
	,[OrderID]=ph.OrderID
	,[ScanQty]= ph.ScanQty
	,[LackingQty]= ph.LackingQty
	,[ScanName]= ph.ScanName+'-'+ (select Name from pass1 where id=ph.ScanName)
	,[ScanDate]= ph.ScanEditDate
	,[DeletedBy]=ph.AddName +'-'+ (select Name from pass1 where id=ph.AddName)
	,[DeletedDate]=ph.AddDate
FROM PackingScan_History ph
WHERE PackingListID='{this.PackingListID}'
";

            DBProxy.Current.Select(null, cmd, out dt);

            this.listControlBindingSource1.DataSource = dt;

            base.OnFormLoaded();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
