using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class P09 : Win.Tems.QueryForm
    {
        private DataTable dtMaster;
        private DataTable dtDetail;

        /// <inheritdoc/>
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region GridMain Setting
            this.Helper.Controls.Grid.Generator(this.gridMain)
               .Date("ScanDate", header: "Scan Date", iseditingreadonly: true)
               .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: true)
               .Text("CTNStartNo", header: "CTN#", iseditingreadonly: true)
               .Text("CartonQty", header: "Carton Qty", width: Widths.Auto(), iseditingreadonly: true)
               .Text("MDFailQty", header: "Discrepancy", width: Widths.Auto(), iseditingreadonly: true)
               .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
               .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: true)
               .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: true)
               .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: true)
               .Text("Alias", header: "Destination", width: Widths.Auto(), iseditingreadonly: true)
               .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: true)
               .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: true)
               .Text("Barcode", header: "Barcode", width: Widths.AnsiChars(15), iseditingreadonly: false)
               .Text("ReceivedBy", header: "Scan By", width: Widths.Auto(), iseditingreadonly: true)
               .Text("AddDate", header: "Scan Time", width: Widths.Auto(), iseditingreadonly: true)
               .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
               .Text("RepackOrderID", header: "Repack To SP#", width: Widths.AnsiChars(15), iseditable: false)
               .Text("RepackCtnStartNo", header: "Repack To CTN#", width: Widths.AnsiChars(6), iseditable: false);
            #endregion

            #region GridDetail Setting
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("Description", header: "Description", iseditingreadonly: true)
                .Numeric("Qty", header: "Discrepancy", iseditingreadonly: true)
                ;
            #endregion
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string dateTransfer1 = string.Empty, dateTransfer2 = string.Empty, packid = string.Empty, sp = string.Empty;
            string sqlwhere = string.Empty;
            if (this.dateTransfer.HasValue)
            {
                dateTransfer1 = this.dateTransfer.Value1.Value.ToShortDateString();
                dateTransfer2 = this.dateTransfer.Value2.Value.AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");
                sqlwhere += $@" and md.ScanDate between @TransferDate1 and @TransferDate2 ";
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                packid = this.txtPackID.Text;
                sqlwhere += $@" and (md.PackingListID = @packid or  pd.OrigID = @packid) ";
            }

            if (!MyUtility.Check.Empty(this.txtsp.Text))
            {
                sp = this.txtsp.Text;
                sqlwhere += $@" and (md.OrderID = @sp or pd.OrigOrderID = @sp) ";
            }

            this.ShowWaitMessage("Data Loading...");

            string sqlcmd = $@"
declare @TransferDate1  datetime = '{dateTransfer1}'
declare @TransferDate2  datetime = '{dateTransfer2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'

select md.ScanDate
	    , [PackingListID] = iif(isnull(pd.OrigID, '') = '', md.PackingListID, pd.OrigID)
	    , [CTNStartNo] = iif(isnull(pd.OrigCTNStartNo, '') = '', md.CTNStartNo, pd.OrigCTNStartNo)
	    , [CartonQty] = md.CartonQty
        , [MDFailQty] = md.MDFailQty
	    , [OrderID] = iif(isnull(pd.OrigOrderID, '') = '', md.OrderID, pd.OrigOrderID)
	    , o.CustPONo
	    , o.StyleID
	    , o.BrandID
	    , Country.Alias
	    , os.BuyerDelivery
	    , o.SciDelivery
	    , ReceivedBy = dbo.getPass1(md.AddName)
        , [AddDate] = format(md.AddDate, 'yyyy/MM/dd HH:mm:ss')
	    , [RepackPackID] = iif(pd.OrigID != '',pd.ID, pd.OrigID)
        , [RepackOrderID] = iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
        , [RepackCtnStartNo] = iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)
        , [Barcode] = STUFF((SELECT 
				            (
					            SELECT DISTINCT concat('/', pd.Barcode)
                                from PackingList_Detail pd with(nolock) 
                                where md.SCICtnNo = pd.SCICtnNo
		                                AND md.OrderID = pd.OrderID
		                                AND md.CTNStartNo = pd.CTNStartNo
                                        AND md.PackingListID = pd.id 
					            FOR XML PATH('')
				            )) ,1,1,'')
	    ,md.Ukey
into #tmp
from MDScan md with(nolock)
left join orders o with(nolock) on md.OrderID = o.ID
left join Country with(nolock) on Country.id = o.Dest
outer apply (
    select top 1 *
    from PackingList_Detail pd with(nolock) 
    where md.SCICtnNo = pd.SCICtnNo
		    AND md.OrderID = pd.OrderID
		    AND md.CTNStartNo = pd.CTNStartNo
            AND md.PackingListID = pd.id 
) PD
left join Order_QtyShip os on pd.OrderID = os.Id
							and pd.OrderShipmodeSeq = os.Seq

where 1=1
        {sqlwhere}

select * from #tmp
ORDER BY ScanDate,[PackingListID],[CTNStartNo],[OrderID]

select mdd.MDScanUKey
,[Description] = (select Description from PackingReason WHERE Type='MD' AND Junk=0 and id = mdd.PackingReasonID)
, mdd.Qty
from MDScan_Detail mdd
inner join #tmp md on mdd.MDScanUKey = md.Ukey

drop table #tmp

";
            DataSet datas = null;
            if (!SQL.Selects(string.Empty, sqlcmd, out datas))
            {
                MyUtility.Msg.WarningBox(sqlcmd, "DB error!!");
                return;
            }

            if (this.listControlBindingSource1.DataSource != null)
            {
                this.listControlBindingSource1.DataSource = null;
            }

            if (this.listControlBindingSource2.DataSource != null)
            {
                this.listControlBindingSource2.DataSource = null;
            }

            datas.Tables[0].AcceptChanges();
            datas.Tables[1].AcceptChanges();

            if (datas.Tables[0].Rows.Count == 0)
            {
                return;
            }

            this.dtMaster = datas.Tables[0];
            this.dtMaster.TableName = "Master";

            this.dtDetail = datas.Tables[1];
            this.dtDetail.TableName = "Detail";

            DataRelation relation = new DataRelation(
              "rel1",
              new DataColumn[] { this.dtMaster.Columns["Ukey"] },
              new DataColumn[] { this.dtDetail.Columns["MDScanUKey"] });

            datas.Relations.Add(relation);

            this.listControlBindingSource1.DataSource = datas;
            this.listControlBindingSource1.DataMember = "Master";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource2.DataMember = "rel1";
            this.gridMain.AutoResizeColumns();
            this.gridDetail.AutoResizeColumns();

            this.HideWaitMessage();
            this.gridMain.AutoResizeColumns();
        }
    }
}
