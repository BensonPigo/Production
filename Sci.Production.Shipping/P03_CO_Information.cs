using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P03_CO_Information : Sci.Win.Subs.Base
    {
        private string _exportID;

        public P03_CO_Information(string exportID)
        {
            this.InitializeComponent();
            this._exportID = exportID;
        }

        protected override void OnFormLoaded()
        {
            #region QueryData
            DualResult result;
            DataTable dt;
            string sqlcmd = $@"
select distinct c.*
from CertOfOrigin c
inner join Export_Detail ed on ed.SuppID=c.SuppID and ed.FormXPayINV = c.FormXPayINV
where ed.ID='{this._exportID}'";

            if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt)))
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count > 0 && dt != null)
            {
                this.listControlBindingSource1.DataSource = dt;
            }
            #endregion

            base.OnFormLoaded();
            this.gridCO.DataSource = this.listControlBindingSource1;
            this.gridCO.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridCO)
                .Text("SuppID", header: "Supplier", width: Widths.AnsiChars(6))
                .Text("FormXPayINV", header: "Payment Invoice#", width: Widths.AnsiChars(16))
                .Text("COName", header: "Form C/O Name", width: Widths.AnsiChars(15))
                .Date("ReceiveDate", header: "Form Rcvd Date", width: Widths.AnsiChars(10))
                .Text("Carrier", header: "Carrier", width: Widths.AnsiChars(16))
                .Text("AWBNo", header: "AWB#", width: Widths.AnsiChars(20))
                .Date("SendDate", header: "Form Send Date", width: Widths.AnsiChars(10))
                ;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
