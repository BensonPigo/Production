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

namespace Sci.Production.Shipping
{
    public partial class P05_UnconfirmHistory : Sci.Win.Subs.Base
    {
        private string GMTBookingID;

        public P05_UnconfirmHistory(string _GMTBookingID)
        {
            this.InitializeComponent();
            this.GMTBookingID = _GMTBookingID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
            .Date("AddDate", header: "Unconfirm Date", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("Name", header: "Reason", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("Remark", header: "Reason Description", width: Widths.AnsiChars(40), iseditingreadonly: true)
            ;

            this.Query();
        }

        private void Query()
        {
            string cmd = string.Empty;

            cmd = $@"
SELECT g.AddDate ,r.Name,g.Remark
FROM GMTBooking_History g
LEFT JOIN Reason r ON g.ReasonID=r.ID 
WHERE g.ID='{this.GMTBookingID}'
AND g.HisType='GBUnCFM' AND r.ReasonTypeID='GMTBooking_UnCFM'
ORDER BY g.AddDate
";
            DataTable gridData;
            if (DBProxy.Current.Select(null, cmd, out gridData))
            {
                this.listControlBindingSource1.DataSource = gridData;
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query data fail!!");
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
