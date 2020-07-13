using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Subcon
{
    public partial class P30_Qty : Win.Tems.QueryForm
    {
        private DataRow DataRow;

        public P30_Qty(DataRow dataRow)
        {
            this.InitializeComponent();
            this.grid1.AutoGenerateColumns = true;
            this.grid2.AutoGenerateColumns = true;
            this.DataRow = dataRow;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("ID", header: "ID", iseditingreadonly: true, width: Widths.AnsiChars(16))
            .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(16))
            .Text("OrderShipmodeSeq", header: "Seq", iseditingreadonly: true, width: Widths.AnsiChars(3))
            .Text("refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(20))
            .Numeric("qty", header: "Qty", iseditingreadonly: true, width: Widths.AnsiChars(6))
            ;
            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("ID", header: "ID", iseditingreadonly: true, width: Widths.AnsiChars(16))
            .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(16))
            .Text("refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(20))
            .Numeric("qty", header: "Qty", iseditingreadonly: true, width: Widths.AnsiChars(6))
            ;
            string sqlcmd1 = $@"
select pd.ID,pd.OrderID,pd.OrderShipmodeSeq,pd.RefNo,Qty=sum(pd.CTNQty)
from PackingList_Detail pd WITH (NOLOCK)
where 1=1
and id = '{this.DataRow["requestid"]}' -- requestid = 前一層PackingList.ID
and pd.OrderId = '{this.DataRow["OrderId"]}'
and pd.Refno = '{this.DataRow["Refno"]}'
group by pd.ID,pd.OrderID,pd.OrderShipmodeSeq,pd.RefNo
";
            DataTable dt1;
            DualResult result = DBProxy.Current.Select(null, sqlcmd1, out dt1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt1;

            string sqlcmd2 = $@"
select lpd.ID,lpd.OrderId,lpd.Refno,Qty = sum(qty)                    
from LocalPo_Detail lpd WITH (NOLOCK) 
where lpd.OrderId = '{this.DataRow["OrderId"]}' and lpd.Refno = '{this.DataRow["Refno"]}'
group by lpd.ID,lpd.OrderId,lpd.Refno
";
            DataTable dt2;
            result = DBProxy.Current.Select(null, sqlcmd2, out dt2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource2.DataSource = dt2;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
