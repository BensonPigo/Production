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
    public partial class P09 : Sci.Win.Tems.QueryForm
    {
        DataTable FtyGroup;
        IList<string> comboBox2_RowSource = new List<string>();
        BindingSource comboxbs2;
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DualResult result;
            #region Combox
            if (result = DBProxy.Current.Select(null, "select distinct FtyGroup from Factory where Junk = 0 ", out FtyGroup))
            {
                this.comboBox1.DataSource = FtyGroup;
                this.comboBox1.DisplayMember = "FtyGroup";
                this.comboBox1.ValueMember = "FtyGroup";
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query Combox fail\r\n"+result.ToString());
            }
            comboBox1.SelectedIndex = -1;

            comboBox2_RowSource.Add("Shipped");
            comboBox2_RowSource.Add("Not Ship");
            comboBox2_RowSource.Add("All");
            comboxbs2 = new BindingSource(comboBox2_RowSource, null);
            comboBox2.DataSource = comboxbs2;
            comboBox2.SelectedIndex = 0;
            #endregion 

            grid1.DataSource = listControlBindingSource1;
            grid1.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13))
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(2))
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(3))
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(15))
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(15))
                .Text("CustCDID", header: "Cust#", width: Widths.AnsiChars(15))
                .Text("Alias", header: "Dest", width: Widths.AnsiChars(13))
                .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(5))
                .Date("BuyerDelivery", header: "Buyer Delivery")
                .Date("PulloutDate", header: "Extended")
                .Text("OSReason", header: "Outstanding Reason", width: Widths.AnsiChars(20))
                .Text("OSRemark", header: "Outstanding Remark", width: Widths.AnsiChars(20));
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("< Buyer Delivery > can not empty!");
                dateRange1.TextBox1.Focus();
                return;
            }
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"with tempData 
as 
(select o.ID,oq.Seq,o.StyleID,o.BrandID,o.FactoryID,o.CustPONo,o.Customize1,o.CustCDID,
 c.Alias,oq.Qty,oq.BuyerDelivery,oq.EstPulloutDate,
 (select max(PulloutDate) from Pullout_Detail where OrderID = o.ID and OrderShipmodeSeq = oq.Seq and ShipQty > 0) as PulloutDate,
 oq.OutstandingReason+' - '+isnull(r.Name,'') as OSReason,o.OutstandingRemark 
 from Orders o 
 left join Order_QtyShip oq on oq.Id = o.ID 
 left join Country c on c.ID = o.Dest 
 left join Reason r on r.ReasonTypeID = 'Delivery_OutStand' and r.ID = oq.OutstandingReason 
 where o.IsForecast = 0 
 and o.LocalOrder = 0 
 and oq.BuyerDelivery >= '{0}' 
 and oq.BuyerDelivery <= '{1}' ", Convert.ToDateTime(dateRange1.Value1).ToString("d"),Convert.ToDateTime(dateRange1.Value2).ToString("d")));
            if (!MyUtility.Check.Empty(txtbrand1.Text))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'",txtbrand1.Text));
            }

            if (!MyUtility.Check.Empty(comboBox1.SelectedValue))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'",comboBox1.SelectedValue.ToString().Trim()));
            }
            sqlCmd.Append(@") 
select ID,Seq,StyleID,BrandID,FactoryID,CustPONo,Customize1,CustCDID,Alias,Qty,
BuyerDelivery,iif(PulloutDate is null,EstPulloutDate,PulloutDate) as PulloutDate,
OSReason,OutstandingRemark 
from tempData 
where iif(PulloutDate is null,EstPulloutDate,PulloutDate) > BuyerDelivery
and iif(PulloutDate is null,EstPulloutDate,PulloutDate) is not null ");
            if (comboBox2.SelectedIndex != -1)
            {
                if (comboBox2.SelectedValue.ToString() == "Shipped")
                {
                    sqlCmd.Append("and PulloutDate is not null");
                }
                else
                {
                    if (comboBox2.SelectedValue.ToString() == "Not Ship")
                    {
                        sqlCmd.Append("and PulloutDate is null");
                    }
                }
            }
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, Convert.ToString(sqlCmd), out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //To Excel
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //Close
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
