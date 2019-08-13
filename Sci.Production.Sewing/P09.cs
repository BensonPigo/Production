﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class P09 : Sci.Win.Tems.QueryForm
    {
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            string selectCommand = string.Format("select ID = '',IDName = '' union all select ID,  rtrim(ID)+'- '+rtrim(Name)  as IDName from DropDownList WITH (NOLOCK) where Type = '{0}' ", "Pms_DRYTransferTo");
            Ict.DualResult returnResult;
            DataTable dropDownListTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
            {
                this.txtdropdownlist1.DataSource = dropDownListTable;
                this.txtdropdownlist1.DisplayMember = "IDName";
                this.txtdropdownlist1.ValueMember = "ID";
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("TransferTo", header: "Transfer To", width: Widths.Auto(), iseditingreadonly: true)
            .Text("TransferDate", header: "Transfer Date", iseditingreadonly: true)
            .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: true)
            .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Qty", header: "Qty", width: Widths.Auto(), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Alias", header: "Destination", width: Widths.Auto(), iseditingreadonly: true)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: true)
            .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: true)
            .Text("TransferBy", header: "Transfer By", width: Widths.Auto(), iseditingreadonly: true)
            .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackOrderID", header: "Repack To SP #", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackCtnStartNo", header: "Repack To CTN #", width: Widths.AnsiChars(6), iseditable: false);
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string dateTransfer1 = string.Empty, dateTransfer2 = string.Empty, packid = string.Empty, sp = string.Empty, transferTo = string.Empty;
            string sqlwhere = string.Empty;
            if (this.dateTransfer.HasValue)
            {
                dateTransfer1 = this.dateTransfer.Value1.Value.ToShortDateString();
                dateTransfer2 = this.dateTransfer.Value2.Value.AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");
                sqlwhere += $@" and dr.TransferDate between @TransferDate1 and @TransferDate2 ";
            }

            if (!MyUtility.Check.Empty(this.txtdropdownlist1.Text))
            {
                transferTo = MyUtility.Convert.GetString(this.txtdropdownlist1.SelectedValue);
                sqlwhere += $@" and dr.TransferTo = @TransferTo";
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                packid = this.txtPackID.Text;
                sqlwhere += $@" and (pd.ID = @packid or  pd.OrigID = @packid) ";
            }

            if (!MyUtility.Check.Empty(this.txtsp.Text))
            {
                sp = this.txtsp.Text;
                sqlwhere += $@" and (pd.OrderID = @sp or pd.OrigOrderID = @sp) ";
            }

            this.ShowWaitMessage("Data Loading...");

            string sqlcmd = $@"
declare @TransferDate1  datetime = '{dateTransfer1}'
declare @TransferDate2  datetime = '{dateTransfer2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'
declare @TransferTo nvarchar(20) = '{transferTo}'

select DISTINCT
	dr.TransferTo
	,[TransferDate]=CONVERT(varchar, dr.TransferDate, 111) +' '+ LEFT(CONVERT(varchar, dr.TransferDate, 108),5)
    --,dr.TransferDate
	,[PackingListID] = iif(pd.OrigID = '' OR pd.OrigID IS NULL  ,dr.PackingListID , pd.OrigID)
	,[CTNStartNo] = iif(pd.OrigCTNStartNo = '' OR pd.OrigCTNStartNo IS NULL    ,dr.CTNStartNo   , pd.OrigCTNStartNo)
	,[Qty]=ISNULL(Sum(pd.QtyPerCTN),0)
	,[OrderID] = iif(pd.OrigOrderID = '' OR pd.OrigOrderID IS NULL ,dr.OrderID, pd.OrigOrderID)
	,o.CustPONo
	,o.StyleID
	,o.BrandID
	,Country.Alias
	,o.BuyerDelivery
	,o.SciDelivery
	,TransferBy = dbo.getPass1(dr.AddName)
    , [RepackPackID] = iif(pd.OrigID != '',pd.ID, pd.OrigID)
    , [RepackOrderID] = iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
    , [RepackCtnStartNo] = iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)
from DRYTransfer dr with(nolock)
left join orders o with(nolock) on dr.OrderID = o.ID
left join Country with(nolock) on Country.id = o.Dest
left join PackingList_Detail pd WITH (NOLOCK) on pd.SCICtnNo = dr.SCICtnNo
													AND dr.OrderID = pd.OrderID
													AND  pd.CTNStartNo = dr.CTNStartNo AND dr.OrderID = pd.OrderID AND dr.PackingListID=pd.id 
where 1=1
{sqlwhere}
GROUP BY dr.TransferTo
		,dr.TransferDate
		, iif(pd.OrigID = '' OR pd.OrigID IS NULL  ,dr.PackingListID , pd.OrigID)
		, iif(pd.OrigCTNStartNo = '' OR pd.OrigCTNStartNo IS NULL    ,dr.CTNStartNo   , pd.OrigCTNStartNo)
		, iif(pd.OrigOrderID = '' OR pd.OrigOrderID IS NULL ,dr.OrderID, pd.OrigOrderID)
		,o.CustPONo
		,o.StyleID
		,o.BrandID
		,Country.Alias
		,o.BuyerDelivery
		,o.SciDelivery
		,dr.AddName 
        ,iif(pd.OrigID != '',pd.ID, pd.OrigID)
        ,iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
        ,iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)
 ORDER BY dr.TransferTo,[PackingListID],[CTNStartNo],[OrderID]

";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
            }

            this.HideWaitMessage();
            this.listControlBindingSource1.DataSource = dt;
            this.grid1.AutoResizeColumns();
        }
    }
}
