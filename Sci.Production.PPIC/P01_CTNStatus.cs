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
using Sci.Production.PublicPrg;

namespace Sci.Production.PPIC
{
    public partial class P01_CTNStatus : Sci.Win.Subs.Base
    {
        string orderID;
        bool canRecompute;
        public P01_CTNStatus(string OrderID, bool CanRecompute)
        {
            InitializeComponent();
            orderID = OrderID;
            canRecompute = CanRecompute;
            setCombo1Source();
            setCombo2Source();
            MyUtility.Tool.SetupCombox(comboSortby, 1, 1, ",Location,Ctn#,Packing List ID,Rec. Date");
            comboPackingListID.SelectedIndex = -1;
            comboCTN.SelectedIndex = -1;
            comboSortby.Text = "";
            labelSortby.Visible = false;
            comboSortby.Visible = false;
            btnRecompute.Enabled = CanRecompute;
        }

        private void setCombo1Source()
        {
            DataTable PackingID;
            string sqlCmd = string.Format("select distinct ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' and CTNStartNo <> '' and CTNQty > 0", orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out PackingID);
            if (!result) { MyUtility.Msg.ErrorBox("Query Packing ID fail !!"); }
            MyUtility.Tool.SetupCombox(comboPackingListID, 1, PackingID);
        }

        private void setCombo2Source()
        {
            DataTable CTN;
            string sqlCmd = string.Format(@"select CTNStartNo 
from (select CTNStartNo, MIN(Seq) as Seq from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' and CTNStartNo <> '' and CTNQty > 0 group by CTNStartNo) a
order by Seq", orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out CTN);
            if (!result) { MyUtility.Msg.ErrorBox("Query CTNStartNo fail !!"); }
            MyUtility.Tool.SetupCombox(comboCTN, 1, CTN);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable TransferDetail, CTNLastStatus;
            #region 組撈Transaction Detail的Sql
            string sqlCmd = string.Format(@"with Transferclog
as(
select t.PackingListID,t.CTNStartNo,'Send to clog' as Type,t.ID,t.TransferDate as TypeDate,'' as Location,t.AddDate as UpdateDate, isnull(pd.Seq,0) as Seq
from TransferToClog t WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = t.PackingListID and pd.OrderID = t.OrderID and pd.CTNStartNo = t.CTNStartNo
where t.OrderID = '{0}' and pd.CTNQty > 0
),
CReceive
as(
select c.PackingListId,c.CTNStartNo,'Receive' as Type,c.ID,c.ReceiveDate as TypeDate,c.ClogLocationId as Location,
c.AddDate UpdateDate, isnull(pd.Seq,0) as Seq
from ClogReceive c WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
where c.OrderId = '{0}' and pd.CTNQty > 0
),
CReturn 
as (
select c.PackingListId,c.CTNStartNo,'Return' as Type,c.ID,c.ReturnDate as TypeDate,'' as Location,
c.AddDate as UpdateDate, isnull(pd.Seq,0) as Seq
from ClogReturn c WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
where c.OrderId = '{0}' and pd.CTNQty > 0
)
select * from Transferclog
union all
select * from CReceive
union all
select * from CReturn
order by PackingListID,Seq,UpdateDate", orderID);
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out TransferDetail);
            if (!result) { MyUtility.Msg.ErrorBox("Query transfer detail fail!!" + result.ToString()); }
            listControlBindingSource1.DataSource = TransferDetail;

            sqlCmd = string.Format(@"select p.ID as PackingListID,pd.CTNStartNo,pd.TransferDate,pd.ReceiveDate,p.PulloutDate,pd.ClogLocationId,pd.Remark,pd.Seq
from PackingList p WITH (NOLOCK) ,PackingList_Detail pd WITH (NOLOCK) 
where pd.OrderID = '{0}' and pd.CTNStartNo <> '' and pd.CTNQty > 0 and p.ID = pd.ID
order by p.ID,pd.Seq", orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out CTNLastStatus);
            if (!result) { MyUtility.Msg.ErrorBox("Query last status fail!!" + result.ToString()); }

            listControlBindingSource2.DataSource = CTNLastStatus;

            //設定Grid1的顯示欄位
            this.gridTransactionDetali.IsEditingReadOnly = true;
            this.gridTransactionDetali.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridTransactionDetali)
                .Text("PackingListID", header: "Packing List ID", width: Widths.AnsiChars(15))
                .Text("CTNStartNo", header: "Ctn#", width: Widths.AnsiChars(6))
                .Text("Type", header: "Trans. Type", width: Widths.AnsiChars(12))                
                .Date("TypeDate", header: "Trans. Date", width: Widths.AnsiChars(10))
                .Text("Location", header: "Location", width: Widths.AnsiChars(8))
                .DateTime("UpdateDate", header: "Last update datetime", width: Widths.AnsiChars(20));

            //設定Grid2的顯示欄位
            this.gridLastStatus.IsEditingReadOnly = true;
            this.gridLastStatus.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.gridLastStatus)
                .Text("PackingListID", header: "Packing List ID", width: Widths.AnsiChars(15))
                .Text("CTNStartNo", header: "Ctn#", width: Widths.AnsiChars(6))
                .Date("TransferDate", header: "Trans. Date", width: Widths.AnsiChars(10))
                .Date("ReceiveDate", header: "Rec. Date", width: Widths.AnsiChars(10))
                .Date("PulloutDate", header: "Pull-out Date", width: Widths.AnsiChars(10))
                .Text("ClogLocationId", header: "Location", width: Widths.AnsiChars(8))
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(20));
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            labelSortby.Visible = e.TabPageIndex == 1;
            comboSortby.Visible = e.TabPageIndex == 1;
        }

        //Packing List ID
        private void comboPackingListID_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilter();
        }

        //CTN#
        private void comboCTN_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilter();
        }

        //Sort by
        private void comboSortby_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortBy();
        }

        private void SetFilter()
        {
            StringBuilder filterString = new StringBuilder();
            filterString.Append("1=1");
            if (comboPackingListID.SelectedIndex != -1)
            {
                filterString.Append(string.Format(" and PackingListID = '{0}'", comboPackingListID.SelectedValue.ToString()));
                
            }
            if (comboCTN.SelectedIndex != -1)
            {
                filterString.Append(string.Format(" and CTNStartNo = '{0}'", comboCTN.SelectedValue.ToString()));
            }

            if (((DataTable)listControlBindingSource1.DataSource) != null)
            {
                ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = filterString.ToString();
            }
            if (((DataTable)listControlBindingSource2.DataSource) != null)
            {
                ((DataTable)listControlBindingSource2.DataSource).DefaultView.RowFilter = filterString.ToString();
            }
                    
        }

        private void SortBy()
        {
            if (comboSortby.SelectedIndex != -1)
            {
                DataTable Grid2 = (DataTable)listControlBindingSource2.DataSource;
                switch (comboSortby.SelectedValue.ToString())
                {
                    case "Location":
                        Grid2.DefaultView.Sort = "ClogLocationId";
                        break;
                    case "Ctn#":
                        Grid2.DefaultView.Sort = "CTNStartNo";
                        break;
                    case "Packing List ID":
                        Grid2.DefaultView.Sort = "PackingListID";
                        break;
                    case "Rec. Date":
                        Grid2.DefaultView.Sort = "ReceiveDate";
                        break;
                    default:
                        if (Grid2 != null)
                        {
                            Grid2.DefaultView.Sort = "PackingListID,Seq";
                        }
                        break;
                }
            }
        }

        //Recompute
        private void btnRecompute_Click(object sender, EventArgs e)
        {
            bool prgResult = Prgs.UpdateOrdersCTN(orderID);
            if (!prgResult)
            {
                MyUtility.Msg.WarningBox("Recompute fail, pls try again!!");
            }
        }
    }
}
