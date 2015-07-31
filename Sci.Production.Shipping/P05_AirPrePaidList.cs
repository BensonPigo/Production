using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;

namespace Sci.Production.Shipping
{
    public partial class P05_AirPrePaidList : Sci.Win.Subs.Base
    {
        private string gmtBookingID;
        private DualResult result;
        private DataTable gridData;
        public P05_AirPrePaidList(string GMTBookingID)
        {
            InitializeComponent();
            gmtBookingID = GMTBookingID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlCmd = string.Format(@"select p.ID as PackID,p.OrderID,p.OrderShipmodeSeq,isnull(a.ID,'') as AirPPID
from (select distinct pd.ID,pd.OrderID,pd.OrderShipmodeSeq 
      from PackingList p, PackingList_Detail pd
	  where p.INVNo = '{0}' and p.ID = pd.ID) p
left join AirPP a on a.OrderID = p.OrderID and a.OrderShipmodeSeq = p.OrderShipmodeSeq", gmtBookingID);

            if (result = DBProxy.Current.Select(null, sqlCmd, out gridData))
            {
                listControlBindingSource1.DataSource = gridData;
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query data fail!!");
            }

            //Grid設定
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("PackID", header: "Packing No.", width: Widths.AnsiChars(13))
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13))
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2))
                .Text("AirPPID", header: "Air Pre-Paid No.", width: Widths.AnsiChars(13));
        }

        //Close
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
