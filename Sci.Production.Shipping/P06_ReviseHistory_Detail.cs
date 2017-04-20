using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P06_ReviseHistory_Detail : Sci.Win.Subs.Base
    {
        DataRow masterData;
        public P06_ReviseHistory_Detail(DataRow masterData)
        {
            InitializeComponent();
            this.masterData = masterData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            displaySPNo.Value = MyUtility.Convert.GetString(masterData["OrderID"]);
            displayStatusOld.Value = MyUtility.Convert.GetString(masterData["OldStatusExp"]);
            displayStatusRevised.Value = MyUtility.Convert.GetString(masterData["NewStatusExp"]);
            displayRevisedStatus.Value = MyUtility.Convert.GetString(masterData["ReviseStatus"]);
            numShipQtyOld.Value = MyUtility.Convert.GetInt(masterData["OldShipQty"]);
            numShipQtyRevised.Value = MyUtility.Convert.GetInt(masterData["NewShipQty"]);

            this.gridReviseHistoryDetail.IsEditingReadOnly = true;
            gridReviseHistoryDetail.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridReviseHistoryDetail)
                .Text("Article", header: "Color way", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("OldShipQty", header: "Old Q'ty", width: Widths.AnsiChars(6))
                .Numeric("NewShipQty", header: "New Q'ty", width: Widths.AnsiChars(6));

            string sqlCmd = string.Format(@"select * 
from Pullout_Revise_Detail prd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = prd.OrderID
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = prd.SizeCode
where Pullout_ReviseReviseKey = {0}
order by os.Seq", MyUtility.Convert.GetString(masterData["ReviseKey"]));

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            listControlBindingSource1.DataSource = gridData;
        }
    }
}
