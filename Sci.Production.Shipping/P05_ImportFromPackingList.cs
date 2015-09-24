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
    public partial class P05_ImportFromPackingList : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataRow masterData;
        private string sqlCmd, allPackID;
        private DualResult result;
        private DataTable gridData, detailData,selectData;

        public P05_ImportFromPackingList(DataRow MasterData, DataTable DetailData)
        {
            InitializeComponent();
            txtmultifactory1.Text = Sci.Env.User.FactoryList;
            this.masterData = MasterData;
            this.detailData = DetailData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = false;
            grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("ID", header: "Packing#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CustCDID", header: "CustCD", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Date("SDPDate", header: "SDP Date", iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Delivery", iseditingreadonly: true)
                .Numeric("ShipQty", header: "Q'ty", iseditingreadonly: true)
                .Numeric("CTNQty", header: "ttl CTNs", iseditingreadonly: true)
                .Numeric("NW", header: "N.W.", decimal_places: 3, iseditingreadonly: true)
                .Numeric("NNW", header: "N.N.W.", decimal_places: 3, iseditingreadonly: true);
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            #region 組SQL語法
            sqlCmd = string.Format(@"with BulkPack
as
(select iif(p.CustCDID = 'ASIA',1,0) as Selected,p.id,p.OrderID,p.CustCDID,oq.SDPDate,oq.BuyerDelivery,p.ShipQty,p.CTNQty,p.NW,p.NNW,
'Y' as GMTBookingLock,p.FactoryID,p.CargoReadyDate,p.PulloutDate,p.GW,p.CBM,p.Status,p.InspDate,(select sum(CTNQty) from PackingList_Detail pd where pd.ID = p.ID and pd.ClogReceiveID != '') as ClogCTNQty
 from PackingList p, Order_QtyShip oq
 where p.Type = 'B'
 and '{1}' like '%'+rtrim(p.FactoryID)+'%'
 and p.INVNo = ''
 and p.ShipModeID = '{2}'
 and p.BrandID = '{3}'
 and p.Dest = '{4}'
 and p.CustCDID = '{0}'
 and p.OrderID = oq.Id
 and p.OrderShipmodeSeq = oq.Seq", masterData["CustCDID"].ToString(),txtmultifactory1.Text,masterData["ShipModeID"].ToString(),masterData["BrandID"].ToString(),masterData["Dest"].ToString());
            if (!MyUtility.Check.Empty(dateRange1.Value1))
            {
                sqlCmd = sqlCmd + string.Format(" and oq.SDPDate >= '{0}'",Convert.ToDateTime(dateRange1.Value1).ToString("d"));
            }
            if (!MyUtility.Check.Empty(dateRange1.Value2))
            {
                sqlCmd = sqlCmd + string.Format(" and oq.SDPDate <= '{0}'",Convert.ToDateTime(dateRange1.Value2).ToString("d"));
            }
            if (!MyUtility.Check.Empty(dateRange2.Value1))
            {
                sqlCmd = sqlCmd + string.Format(" and oq.BuyerDelivery >= '{0}'",Convert.ToDateTime(dateRange2.Value1).ToString("d"));
            }
            if (!MyUtility.Check.Empty(dateRange2.Value2))
            {
                sqlCmd = sqlCmd + string.Format(" and oq.BuyerDelivery <= '{0}'",Convert.ToDateTime(dateRange2.Value2).ToString("d"));
            }
            sqlCmd = sqlCmd + string.Format(@"),
IniSamplePack
as
(select iif(p.CustCDID = 'ASIA',1,0) as Selected,p.id,p.OrderID,p.CustCDID,oq.SDPDate,oq.BuyerDelivery,p.ShipQty,p.CTNQty,p.NW,p.NNW,
 'Y' as GMTBookingLock,p.FactoryID,p.CargoReadyDate,p.PulloutDate,p.GW,p.CBM,p.Status,p.InspDate,0 as ClogCTNQty
 from PackingList p
 left join PackingList_Detail pd on pd.ID = p.ID
 left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
 where p.INVNo = '' and p.Type = 'S' and '{1}' like '%'+rtrim(p.FactoryID)+'%' and p.Dest = '{2}' and p.ShipModeID = '{3}'
),
InvalidData
as
(select distinct ID
 from IniSamplePack
 where 1=1", masterData["CustCDID"].ToString(), txtmultifactory1.Text, masterData["Dest"].ToString(), masterData["ShipModeID"].ToString());
            if (!MyUtility.Check.Empty(dateRange1.Value1))
            {
                sqlCmd = sqlCmd + string.Format(" and SDPDate < '{0}'",Convert.ToDateTime(dateRange1.Value1).ToString("d"));
            }
            if (!MyUtility.Check.Empty(dateRange1.Value2))
            {
                sqlCmd = sqlCmd + string.Format(" and SDPDate > '{0}'",Convert.ToDateTime(dateRange1.Value2).ToString("d"));
            }
            if (!MyUtility.Check.Empty(dateRange2.Value1))
            {
                sqlCmd = sqlCmd + string.Format(" and BuyerDelivery < '{0}'",Convert.ToDateTime(dateRange2.Value1).ToString("d"));
            }
            if (!MyUtility.Check.Empty(dateRange2.Value2))
            {
                sqlCmd = sqlCmd + string.Format(" and BuyerDelivery > '{0}'",Convert.ToDateTime(dateRange2.Value2).ToString("d"));
            }

            sqlCmd = sqlCmd + @"),
PackSample
as
(select Selected,ID,CustCDID,min(SDPDate) as SDPDate,min(BuyerDelivery) as BuyerDelivery,ShipQty,CTNQty,NW,NNW,
 GMTBookingLock,FactoryID,CargoReadyDate,PulloutDate,GW,CBM,Status,InspDate,ClogCTNQty
 from IniSamplePack 
 where id not in (select ID from InvalidData where ID is not null)
 group by Selected,ID,CustCDID,ShipQty,CTNQty,NW,NNW,GMTBookingLock,FactoryID,CargoReadyDate,PulloutDate,GW,CBM,Status,InspDate,ClogCTNQty
),
SamplePack
as
(select ps.Selected,ps.ID,((select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd where pd.ID = ps.ID) a for xml path(''))) as OrderID,
 ps.CustCDID,ps.SDPDate,ps.BuyerDelivery,ps.ShipQty,ps.CTNQty,ps.NW,ps.NNW,ps.GMTBookingLock,ps.FactoryID,ps.CargoReadyDate,ps.PulloutDate,ps.GW,ps.CBM,ps.Status,ps.InspDate,ps.ClogCTNQty
 from PackSample ps
)
select * from BulkPack
union all
select * from SamplePack";
            #endregion

            result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (result)
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //Import
        private void button2_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
            gridData = (DataTable)listControlBindingSource1.DataSource;
            if (gridData.Rows.Count > 0)
            {
                allPackID = "";
                DataRow[] dr = gridData.Select("Selected = 1");
                if (dr.Length > 0)
                {
                    //檢查相同SP#不同PackingList#不可以匯入到同一張GarmentBooking中
                    foreach (DataRow currentRow in dr)
                    {
                        allPackID = allPackID + "'" + currentRow["ID"].ToString() + "',";
                    }

                    foreach (DataRow currentRow in detailData.Rows)
                    {
                        allPackID = allPackID + "'" + currentRow["ID"].ToString() + "',";
                    }

                    if (allPackID != "")
                    {
                        sqlCmd = string.Format(@"select distinct b.OrderID,b.ID
from (select OrderID,COUNT(OrderID) as CNT
      from (select distinct ID,OrderID from PackingList_Detail where ID in ({0})) a
      group by OrderID
      Having COUNT(OrderID) > 1) a, PackingList_Detail b
where b.ID in ({0})
and a.OrderID = b.OrderID", allPackID.Substring(0, allPackID.Length - 1));
                        result = DBProxy.Current.Select(null, sqlCmd, out selectData);
                        if (result)
                        {
                            if (selectData.Rows.Count > 0)
                            {
                                allPackID = "";
                                foreach (DataRow currentRow in selectData.Rows)
                                {
                                    allPackID = allPackID + "SP#:" + currentRow["OrderID"].ToString() + "   Packing#:" + currentRow["ID"].ToString() + "\r\n";
                                }
                                MyUtility.Msg.WarningBox("Can't import SP# with different Packing# in the same Inv# !!\r\n" + allPackID);
                                return;
                            }
                        }
                        else
                        {
                            MyUtility.Msg.ErrorBox(result.ToString());
                            return;
                        }
                    }

                    foreach (DataRow currentRow in dr)
                    {
                        DataRow[] findrow = detailData.Select(string.Format("OrderID = '{0}'", currentRow["ID"].ToString()));
                        if (findrow.Length == 0)
                        {
                            currentRow.AcceptChanges();
                            currentRow.SetAdded();
                            detailData.ImportRow(currentRow);
                        }
                    }
                }
            }
            MyUtility.Msg.InfoBox("Import finished!");
            this.Close();
        }

        //Close
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
