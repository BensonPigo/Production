using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Logistic
{
    public partial class P05_ImportData : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataTable plData, gbData, detailData, detail2Data;
        DataRow masterData;
        DataRelation relation;

        public P05_ImportData(DataRow MasterData, DataTable DetailData, DataTable Detail2Data)
        {
            InitializeComponent();
            txtshipmode1.SelectedValue = "";
            masterData = MasterData;
            detailData = DetailData;
            detail2Data = Detail2Data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            grid1.DataSource = listControlBindingSource1;
            grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("ID", header: "GB#", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Forwarder", header: "Forwarder", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .DateTime("CutOffdate", header: "Cut-off Date/Time", iseditingreadonly: true);
            grid1.SelectionChanged += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(grid1.GetSelectedRowIndex());
                if (dr != null)
                {
                    string filter = string.Format("InvNo = '{0}'", dr["ID"].ToString());
                    plData.DefaultView.RowFilter = filter;
                }
            };

            grid2.DataSource = listControlBindingSource2;
            grid2.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid2)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(13))
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13))
                .Date("BuyerDelivery", header: "Delivery");
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            #region 組SQL
            string sqlCmd = @"select distinct 1 as Selected,g.ID,g.BrandID,g.ShipModeID,(g.Forwarder+' - '+ls.Abb) as Forwarder,g.CutOffDate,g.CYCFS,
      g.SONo,g.ForwarderWhseID,iif(g.Status='Confirmed','GB Confirmed',iif(g.SOCFMDate is null,'','S/O Confirmed')) as Status,
	  g.TotalCTNQty,g.TotalCBM,(select isnull(sum(pd.CTNQty),0) from PackingList p,PackingList_Detail pd where p.INVNo = g.ID and p.ID = pd.ID and pd.ReceiveDate is not null) as ClogCTNQty
	  from GMTBooking g, Order_QtyShip oq, PackingList p, PackingList_Detail pd,LocalSupp ls
	  where g.ShipPlanID = ''
	  and g.SOCFMDate is not null
	  and g.ID = p.INVNo
	  and p.ID = pd.ID
	  and pd.OrderID = oq.Id
	  and pd.OrderShipmodeSeq = oq.Seq
	  and g.Forwarder = ls.ID";
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and g.id >= '{0}'", textBox1.Text);
            }
            if (!MyUtility.Check.Empty(textBox2.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and g.id <= '{0}'", textBox2.Text);
            }
            if (!MyUtility.Check.Empty(dateRange1.Value1))
            {
                sqlCmd = sqlCmd + string.Format(" and oq.BuyerDelivery >= '{0}'", Convert.ToDateTime(dateRange1.Value1).ToString("d"));
            }
            if (!MyUtility.Check.Empty(dateRange1.Value2))
            {
                sqlCmd = sqlCmd + string.Format(" and oq.BuyerDelivery <= '{0}'", Convert.ToDateTime(dateRange1.Value2).ToString("d"));
            }
            if (!MyUtility.Check.Empty(dateBox1.Value))
            {
                sqlCmd = sqlCmd + string.Format(" and g.CutOffDate >= '{0}' and g.CutOffDate < '{1}'", Convert.ToDateTime(dateRange1.Value1).ToString("d"), (Convert.ToDateTime(dateRange1.Value1).AddDays(1)).ToString("d"));
            }
            if (!MyUtility.Check.Empty(txtshipmode1.SelectedValue))
            {
                sqlCmd = sqlCmd + string.Format(" and g.ShipModeID = '{0}'", txtshipmode1.SelectedValue.ToString());
            }
            if (!MyUtility.Check.Empty(txtbrand1.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and g.BrandID = '{0}'", txtbrand1.Text);
            }
            if (!MyUtility.Check.Empty(txtsubcon1.TextBox1.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and g.Forwarder = '{0}'", txtsubcon1.TextBox1.Text);
            }
            if (!MyUtility.Check.Empty(textBox3.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and pd.OrderID = '{0}'", textBox3.Text);
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gbData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query GB error:" + result.ToString());
            }

            string allID = "";
            foreach (DataRow dr in gbData.Rows)
            {
                allID = allID + string.Format("'{0}',", dr["ID"].ToString());
            }

            if (allID != "")
            {
                sqlCmd = string.Format(@"select distinct pd.ID, pd.OrderID,oq.BuyerDelivery,p.INVNo
from PackingList p,PackingList_Detail pd,Order_QtyShip oq
where p.INVNo in ({0})
and p.id = pd.ID
and pd.OrderID = oq.Id
and pd.OrderShipmodeSeq = oq.Seq", allID.Substring(0, allID.Length - 1));
            }
            result = DBProxy.Current.Select(null, sqlCmd, out plData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query PL error:" + result.ToString());
            }
            listControlBindingSource1.DataSource = gbData;
            listControlBindingSource2.DataSource = plData;
            if (gbData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
            }
        }

        //Import Data
        private void button2_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
            if (MyUtility.Check.Empty(masterData["Remark"]))
            {
                masterData["Remark"] = string.Format(@"GB#:{0}~{1}, Buyer Delivery:{2}~{3}, Cut-off Date:{4}, Ship Mode:{5}, Brand:{6}, Forwarder:{7}, SP#:{8}",
                    MyUtility.Check.Empty(textBox1.Text) ? " " : textBox1.Text, MyUtility.Check.Empty(textBox2.Text) ? " " : textBox2.Text,
                    MyUtility.Check.Empty(dateRange1.Value1) ? " " : Convert.ToDateTime(dateRange1.Value1).ToString("d"), MyUtility.Check.Empty(dateRange1.Value2) ? " " : Convert.ToDateTime(dateRange1.Value2).ToString("d"),
                    MyUtility.Check.Empty(dateBox1.Value) ? " " : Convert.ToDateTime(dateBox1.Value).ToString("d"), MyUtility.Check.Empty(txtshipmode1.SelectedValue) ? " " : txtshipmode1.SelectedValue.ToString(),
                    MyUtility.Check.Empty(txtbrand1.Text) ? " " : txtbrand1.Text, MyUtility.Check.Empty(txtsubcon1.TextBox1.Text) ? " " : txtsubcon1.TextBox1.Text, MyUtility.Check.Empty(textBox3.Text) ? " " : textBox3.Text);
            }

            string allID = "";
            DataRow[] dra = gbData.Select("Selected = 1");
            if (dra.Length > 0)
            {
                foreach (DataRow dr in dra)
                {
                    DataRow[] findrow = detailData.Select(string.Format("ID = '{0}'", dr["ID"].ToString()));
                    if (findrow.Length == 0)
                    {
                        dr.AcceptChanges();
                        dr.SetAdded();
                        detailData.ImportRow(dr);
                    }
                    allID = allID + string.Format("'{0}',", dr["ID"].ToString());
                }
            }

            if (allID != "")
            {
                string sqlCmd = string.Format(@"select p.ID,
iif(p.OrderID='',(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd where pd.ID = p.id) a for xml path('')),p.OrderID) as OrderID,
iif(p.type = 'B',(select BuyerDelivery from Order_QtyShip where ID = p.OrderID and Seq = p.OrderShipmodeSeq),(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = p.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq)) as BuyerDelivery,
p.Status,p.CTNQty,p.CBM,(select sum(CTNQty) from PackingList_Detail pd where pd.ID = p.ID and pd.ClogReceiveID != '') as ClogCTNQty,
p.InspDate,p.InspStatus,p.PulloutDate,p.InvNo
from PackingList p
where p.InvNo in ({0})", allID.Substring(0,allID.Length-1));
                DataTable packData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out packData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Import query packinglist error:"+result.ToString());
                    return;
                }

                foreach(DataRow dr in packData.Rows)
                {
                    DataRow[] findrow = detail2Data.Select(string.Format("ID = '{0}'", dr["ID"].ToString()));
                    if (findrow.Length == 0)
                    {
                        dr.AcceptChanges();
                        dr.SetAdded();
                        detail2Data.ImportRow(dr);
                    }
                }
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
