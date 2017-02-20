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

namespace Sci.Production.Packing
{
    public partial class P01_CTNData : Sci.Win.Subs.Base
    {
        private DataRow masterData;
        public P01_CTNData(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable CTNData;
            string sqlCmd = string.Format(@"select oc.RefNo,oc.QtyPerCTN,oc.GMTQty,oc.CTNQty,isnull(li.Description,'') as Description, isnull(li.CtnUnit,'') as CtnUnit,
isnull((STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4)),'') as Dimension,
isnull((ls.ID+'-'+ls.Abb),'') as Supplier,
isnull((select sum(ld.Qty) from LocalPO l WITH (NOLOCK) , LocalPO_Detail ld WITH (NOLOCK) where l.Category = 'CARTON' and l.Id = ld.Id and ld.OrderId = oc.ID and ld.Refno = oc.RefNo),0) as POQty
from Order_CTNData oc WITH (NOLOCK) 
left join LocalItem li WITH (NOLOCK) on oc.RefNo = li.RefNo
left join LocalSupp ls WITH (NOLOCK) on li.LocalSuppid = ls.ID
where oc.ID = '{0}'
order by oc.RefNo", masterData["ID"].ToString());

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out CTNData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Order_CTNData fail!!");
            }
            listControlBindingSource1.DataSource = CTNData;
            //設定Grid的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("RefNo", header: "Ref No.", width: Widths.AnsiChars(20))
                .EditText("Description", header: "Description", width: Widths.AnsiChars(30))
                .Text("Dimension", header: "Dimension", width: Widths.AnsiChars(30))
                .Text("CtnUnit", header: "Carton Unit", width: Widths.AnsiChars(8))
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(20))
                .Numeric("QtyPerCTN", header: "Qty/Carton", width: Widths.AnsiChars(3))
                .Numeric("GMTQty", header: "Garment Qty", width: Widths.AnsiChars(5))
                .Numeric("CTNQty", header: "Carton Qty", width: Widths.AnsiChars(5))
                .Numeric("POQty", header: "PO Q'ty", width: Widths.AnsiChars(6));
        }
    }
}
