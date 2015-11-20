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
            MyUtility.Tool.SetupCombox(comboBox3, 1, 1, ",Location,Ctn#,Packing List ID,Rec. Date");
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.Text = "";
            label3.Visible = false;
            comboBox3.Visible = false;
            button1.Enabled = CanRecompute;
        }

        private void setCombo1Source()
        {
            DataTable PackingID;
            string sqlCmd = string.Format("select distinct ID from PackingList_Detail where OrderID = '{0}' and CTNStartNo <> '' and CTNQty > 0", orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out PackingID);
            if (!result) { MyUtility.Msg.ErrorBox("Query Packing ID fail !!"); }
            MyUtility.Tool.SetupCombox(comboBox1, 1, PackingID);
        }

        private void setCombo2Source()
        {
            DataTable CTN;
            string sqlCmd = string.Format(@"select CTNStartNo 
from (select CTNStartNo, MIN(Seq) as Seq from PackingList_Detail where OrderID = '{0}' and CTNStartNo <> '' and CTNQty > 0 group by CTNStartNo) a
order by Seq", orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out CTN);
            if (!result) { MyUtility.Msg.ErrorBox("Query CTNStartNo fail !!"); }
            MyUtility.Tool.SetupCombox(comboBox2, 1, CTN);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable TransferDetail, CTNLastStatus;
            #region 組撈Transaction Detail的Sql
            string sqlCmd = string.Format(@"with Transferclog
as(
select td.PackingListID,td.CTNStartNo,'Send to clog' as Type,td.Id as TypeID,t.TransferDate as TypeDate,'' as Location,td.AddName as UpdateName,td.AddDate as UpdateDate, isnull(pd.Seq,0) as Seq
from TransferToClog_Detail td 
left join TransferToClog t on t.Id = td.Id
left join PackingList_Detail pd on pd.ID = td.PackingListID and pd.OrderID = td.OrderID and pd.CTNStartNo = td.CTNStartNo
where td.OrderID = '{0}'
),
CReceive
as(
select cd.PackingListId,CD.CTNStartNo,'Receive' as Type,cd.Id as TypeID,c.ReceiveDate as TypeDate,cd.ClogLocationId as Location,
iif(cd.EditDate is null,cd.AddName,cd.EditName) as UpdateName,iif(cd.EditDate is null,cd.AddDate,cd.EditDate) as UpdateDate, isnull(pd.Seq,0) as Seq
from ClogReceive_Detail cd
left join ClogReceive c on cd.ID = c.ID
left join PackingList_Detail pd on pd.ID = cd.PackingListID and pd.OrderID = cd.OrderID and pd.CTNStartNo = cd.CTNStartNo
where cd.OrderId = '{0}'
),
CReturn 
as (
select cd.PackingListId,CD.CTNStartNo,'Return' as Type,cd.Id as TypeID,c.ReturnDate as TypeDate,'' as Location,
cd.AddName as UpdateName,cd.AddDate as UpdateDate, isnull(pd.Seq,0) as Seq
from ClogReturn_Detail cd
left join ClogReturn c on cd.ID = c.ID
left join PackingList_Detail pd on pd.ID = cd.PackingListID and pd.OrderID = cd.OrderID and pd.CTNStartNo = cd.CTNStartNo
where cd.OrderId = '{0}'
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
from PackingList p,PackingList_Detail pd 
where pd.OrderID = '{0}' and pd.CTNStartNo <> '' and pd.CTNQty > 0 and p.ID = pd.ID
order by p.ID,pd.Seq", orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out CTNLastStatus);
            if (!result) { MyUtility.Msg.ErrorBox("Query last status fail!!" + result.ToString()); }

            listControlBindingSource2.DataSource = CTNLastStatus;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("PackingListID", header: "Packing List ID", width: Widths.AnsiChars(15))
                .Text("CTNStartNo", header: "Ctn#", width: Widths.AnsiChars(6))
                .Text("Type", header: "Trans. Type", width: Widths.AnsiChars(12))
                .Text("TypeID", header: "Trans. No.", width: Widths.AnsiChars(15))
                .Date("TypeDate", header: "Trans. Date", width: Widths.AnsiChars(10))
                .Text("Location", header: "Location", width: Widths.AnsiChars(8))
                .Text("UpdateName", header: "Last update id", width: Widths.AnsiChars(10))
                .DateTime("UpdateDate", header: "Last update datetime", width: Widths.AnsiChars(20));

            //設定Grid2的顯示欄位
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
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
            label3.Visible = e.TabPageIndex == 1;
            comboBox3.Visible = e.TabPageIndex == 1;
        }

        //Packing List ID
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilter();
        }

        //CTN#
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilter();
        }

        //Sort by
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortBy();
        }

        private void SetFilter()
        {
            StringBuilder filterString = new StringBuilder();
            filterString.Append("1=1");
            if (comboBox1.SelectedIndex != -1)
            {
                filterString.Append(string.Format(" and PackingListID = '{0}'", comboBox1.SelectedValue.ToString()));
                
            }
            if (comboBox2.SelectedIndex != -1)
            {
                filterString.Append(string.Format(" and CTNStartNo = '{0}'", comboBox2.SelectedValue.ToString()));
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
            if (comboBox3.SelectedIndex != -1)
            {
                DataTable Grid2 = (DataTable)listControlBindingSource2.DataSource;
                switch (comboBox3.SelectedValue.ToString())
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
        private void button1_Click(object sender, EventArgs e)
        {
            bool prgResult = Prgs.UpdateOrdersCTN(orderID);
            if (!prgResult)
            {
                MyUtility.Msg.WarningBox("Recompute fail, pls try again!!");
            }
        }
    }
}
