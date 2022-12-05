using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class P08 : Win.Tems.QueryForm
    {
        private DataTable dt = new DataTable();
        private DataTable dtDetail = new DataTable();

        /// <inheritdoc/>
        public P08(ToolStripMenuItem menuitem)
           : base(menuitem)
        {
            this.InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.BeforeSave())
            {
                this.Save();
            }
        }

        private void TxtScanCartonsBarcode_Validating(object sender, CancelEventArgs e)
        {
            this.Find(this.txtScanCartonsBarcode.Text);
        }

        private void Find(string cartonsBarcode)
        {
            if (MyUtility.Check.Empty(cartonsBarcode))
            {
                return;
            }

            string packingListID = string.Empty;
            string cTNStarNo = string.Empty;

            if (cartonsBarcode.Length > 13)
            {
                packingListID = cartonsBarcode.Substring(0, 13);
                cTNStarNo = cartonsBarcode.Substring(13, cartonsBarcode.Length - 13).TrimStart('^');
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@cartonsBarcode", cartonsBarcode));
            sqlParameters.Add(new SqlParameter("@sciCtnNo", cartonsBarcode.GetPackScanContent()));
            sqlParameters.Add(new SqlParameter("@packingListID", packingListID));
            sqlParameters.Add(new SqlParameter("@cTNStarNo", cTNStarNo));
            sqlParameters.Add(new SqlParameter("@M", Env.User.Keyword));

            string sqlcmd =
                @"
select MDFailQty = max (pd.MDFailQty)
	,pd.OrderID
	,o.CustPONo
	,o.StyleID
	,o.SeasonID
	,o.BrandID
	,c.Alias
	,[BuyerDelivery] = format(os.BuyerDelivery, 'yyyy/MM/dd')
	,[CartonQty] = sum(pd.ShipQty) * iif(ol.LocationQty = 0, sl.LocationQty, ol.LocationQty)
    ,p.ID
    ,pd.CTNStartNo
    ,pd.SCICtnNo
from PackingList p with(nolock)
inner join PackingList_Detail pd with(nolock) on p.ID = pd.ID
inner join orders o with(nolock) on pd.OrderID = o.ID
left join Country c with(nolock) on c.id = o.Dest
left join Order_QtyShip os with(nolock) on pd.OrderID = os.Id
										and pd.OrderShipmodeSeq = os.Seq
outer apply
(
	select [LocationQty] = count(distinct Location)
	from Order_Location with(nolock)
	where OrderId = pd.OrderID
)ol
outer apply
(
	select [LocationQty] = count(distinct Location)
	from Style_Location with(nolock)
	where StyleUkey = o.StyleUkey
)sl
where ((pd.ID = @packingListID and pd.CTNStartNo = @cTNStarNo)
        or pd.CustCTN = @cartonsBarcode
        or pd.SCICtnNo = @sciCtnNo)
		and p.MDivisionID =@M
		and p.Type in ('B','L') 
        and pd.DisposeFromClog = 0 
		and pd.TransferDate  is null 
group by pd.MDFailQty,pd.OrderID,o.CustPONo,o.StyleID,o.SeasonID,o.BrandID
	,c.Alias,os.BuyerDelivery,ol.LocationQty, sl.LocationQty,p.ID
    ,pd.CTNStartNo,pd.SCICtnNo
";
            this.ShowWaitMessage("Data Loading....");
            DualResult result = DBProxy.Current.Select(null, sqlcmd, sqlParameters, out this.dt);
            this.HideWaitMessage();
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dt.Rows.Count == 0)
            {
                this.txtScanCartonsBarcode.Text = string.Empty;
                this.numericBoxDiscrepancy.Value = 0;
                this.numericBoxDiscrepancy.ReadOnly = false;
                MyUtility.Msg.WarningBox("Datas not found!");
                return;
            }

            foreach (DataRow row in this.dt.Rows)
            {
                this.displaySP.Text = MyUtility.Convert.GetString(row["OrderID"]);
                this.displayPO.Text = MyUtility.Convert.GetString(row["CustPONo"]);
                this.displayStyle.Text = MyUtility.Convert.GetString(row["StyleID"]);
                this.displaySeason.Text = MyUtility.Convert.GetString(row["SeasonID"]);
                this.displayBrand.Text = MyUtility.Convert.GetString(row["BrandID"]);
                this.displayDestination.Text = MyUtility.Convert.GetString(row["Alias"]);
                this.displayBuyerDelivery.Text = MyUtility.Convert.GetString(row["BuyerDelivery"]);
                this.displayCartonQty.Text = MyUtility.Convert.GetString(row["CartonQty"]);
                this.numericBoxDiscrepancy.Text = MyUtility.Convert.GetString(row["MDFailQty"]);
                this.numericBoxDiscrepancy.Maximum = MyUtility.Convert.GetInt(row["CartonQty"]);
            }

            string sqlcmdChk = $@"
select 1    from MDScan_Detail md     where md.MDScanUKey = (        select top 1 m.Ukey        from MDScan m        where exists(			select 1 from PackingList_Detail pd 			where (pd.ID = '{this.dt.Rows[0]["ID"]}'             and pd.CTNStartNo = '{this.dt.Rows[0]["CTNStartNo"]}')			and  pd.ID = m.PackingListID and pd.OrderID = m.OrderID  and pd.CTNStartNo = m.CTNStartNo and pd.MDStatus <> 'Pass')        order by m.AddDate desc    )
";

            if (MyUtility.Check.Seek(sqlcmdChk))
            {
                this.numericBoxDiscrepancy.ReadOnly = true;
            }
            else
            {
                this.numericBoxDiscrepancy.ReadOnly = false;
            }

            this.dtDetail = new DataTable();
        }

        private bool BeforeSave()
        {
            if (MyUtility.Check.Empty(this.txtScanCartonsBarcode.Text))
            {
                MyUtility.Msg.WarningBox("Scan Cartons Barcode is not empty");
                return false;
            }

            if (MyUtility.Convert.GetInt(this.numericBoxDiscrepancy.Text) > MyUtility.Convert.GetInt(this.displayCartonQty.Text))
            {
                MyUtility.Msg.WarningBox("Discrepancy cannot exceed Carton Qty");
                return false;
            }

            if (this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            return true;
        }

        private void Save()
        {
            DataRow dr = this.dt.Rows[0];
            string strMDStatus = this.numericBoxDiscrepancy.Value > 0 ? "Hold" : "Pass";
            string mDScan_Ukey = string.Empty;
            string sqlcmd = $@"
update PackingList_Detail
set MDFailQty = {this.numericBoxDiscrepancy.Text}, MDScanDate = getdate(), MDScanName = '{Env.User.UserID}'
,MDStatus  = '{strMDStatus}'
where ID = '{dr["ID"]}' and CTNStartNo = '{dr["CTNStartNo"]}';

insert into MDScan([ScanDate], [MDivisionID], [OrderID], [PackingListID], [CTNStartNo], [AddName], [AddDate], [SCICtnNo], [MDFailQty], [CartonQty], [DataRemark])
values(getdate(), '{Env.User.Keyword}', '{dr["OrderID"]}', '{dr["ID"]}', '{dr["CTNStartNo"]}', '{Env.User.UserID}', getdate(), '{dr["SCICtnNo"]}', {this.numericBoxDiscrepancy.Text}, {dr["CartonQty"]}, 'Create from PMS');

declare @MDScan_Ukey bigint
select @MDScan_Ukey = @@IDENTITY

select MDScan_Ukey = @MDScan_Ukey

update o
set o.MdRoomScanDate  = GETDATE()
from Orders o
inner join PackingList_Detail pd on pd.OrderID = o.ID
where pd.ID = '{dr["ID"]}' and pd.OrderID = '{dr["OrderID"]}'; 
                ";
            DualResult result = DBProxy.Current.Select("Production", sqlcmd, out DataTable dtUkey);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtUkey != null && dtUkey.Rows.Count > 0)
            {
                mDScan_Ukey = dtUkey.Rows[0]["MDScan_Ukey"].ToString();
            }

            // 取得Detail資料
            if (this.dtDetail != null && this.dtDetail.Rows.Count > 0 && mDScan_Ukey != string.Empty)
            {
                string sqlDetail = string.Empty;
                foreach (DataRow drDetail in this.dtDetail.Rows)
                {
                    sqlDetail += $@"
insert into MDScan_Detail(MDScanUKey,PackingReasonID,Qty)
values('{mDScan_Ukey}','{drDetail["PackingReasonID"]}',{drDetail["Qty"]})
";
                }

                result = DBProxy.Current.Execute("Production", sqlDetail);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Save Completed");
            this.Init();
        }

        private void Init()
        {
            this.txtScanCartonsBarcode.Text = string.Empty;
            this.numericBoxDiscrepancy.Text = string.Empty;
            this.displaySP.Text = string.Empty;
            this.displayPO.Text = string.Empty;
            this.displayStyle.Text = string.Empty;
            this.displaySeason.Text = string.Empty;
            this.displayBrand.Text = string.Empty;
            this.displayDestination.Text = string.Empty;
            this.displayBuyerDelivery.Text = string.Empty;
            this.displayCartonQty.Text = string.Empty;
            this.numericBoxDiscrepancy.ReadOnly = false;
            this.dtDetail = new DataTable();
            this.dt.Clear();
        }

        private void BtnDetail_Click(object sender, EventArgs e)
        {
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            P08_Detail callform = new P08_Detail(this.dt.Rows[0], this.dtDetail);
            callform.ShowDialog();
            int ttlQty = callform.ttlDiscrepancy;
            this.dtDetail = callform.dtDetail;
            if (!MyUtility.Check.Empty(ttlQty))
            {
                this.numericBoxDiscrepancy.Value = ttlQty;
                this.numericBoxDiscrepancy.ReadOnly = true;
            }
            else
            {
                this.numericBoxDiscrepancy.Value = ttlQty;
                this.numericBoxDiscrepancy.ReadOnly = false;
            }
        }
    }
}
