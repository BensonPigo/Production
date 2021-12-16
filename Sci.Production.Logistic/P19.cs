#pragma warning disable SA1652 // Enable XML documentation output
using System;
#pragma warning restore SA1652 // Enable XML documentation output
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Win.Tools;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Sci.Production.CallPmsAPI;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P19
    /// </summary>
    public partial class P19 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P19
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P19(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            string sqlGetTransTo = @"
select  distinct su.SystemName
from    SystemWebAPIURL su with (nolock)
inner join System s with (nolock) on s.Region = su.CountryID and s.Rgcode <> su.SystemName
where su.Junk = 0 
order by su.SystemName
";
            DataTable dtTransTo;
            DualResult result = DBProxy.Current.Select(null, sqlGetTransTo, out dtTransTo);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.comboReceiveFrom.DataSource = dtTransTo;
            this.comboReceiveFrom.DisplayMember = "SystemName";
            this.comboReceiveFrom.ValueMember = "SystemName";
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorTextColumnSettings colPackId = new DataGridViewGeneratorTextColumnSettings();

            colPackId.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    string packID = this.gridReceiveSisterFty.CurrentRow.Cells["ID"].Value.ToString();

                    string sqlGetPackDetail = $@"
select [ID] as [Pack ID]
      ,[OrderID] as [SP#]
      ,[OrderShipmodeSeq] AS [Seq]
      ,[RefNo] as [Ref No.]
      ,[CTNStartNo] as [CTN#]
      ,[Article] AS [ColorWay]
      ,[Color] AS [Color]
      ,[SizeCode] as [Size] --如有多筆, 中間請依/隔開 排序依 PackingList_Detail.Seq
      ,[QtyPerCTN] as [PC/Ctn] --如有多筆, 中間請依/隔開 排序依 PackingList_Detail.Seq
      ,[ShipQty] as [Qty] --sum()進行加總
      ,[NW] as [N.W./CTN.]--sum()進行加總
      ,[GW] as [G.W./CTN.]--sum()進行加總
      ,[NNW] AS [N.N.W./CTN.]--sum()進行加總
      ,[NWPerPcs] as [N.W./Pcs]--sum()進行加總
      ,[TransferDate] as [Production Transfer to Clog Date] 
      ,[ReceiveDate] as [Clog Received From Production Date] 
      ,[ClogLocationId] as [Clog Location]
	   from PackingList_Detail with (nolock)
        where   ID = '{packID}' and
                CTNQty = 1
        order by Seq
";

                    DataTable dtPAckDetail;

                    DualResult result = DBProxy.Current.Select(null, sqlGetPackDetail, out dtPAckDetail);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    MyUtility.Msg.ShowMsgGrid_LockScreen(dtPAckDetail, caption: "PackingList Detail");
                }
            };

            // Grid設定
            this.gridReceiveSisterFty.IsEditingReadOnly = false;
            this.gridReceiveSisterFty.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridReceiveSisterFty)
                .Date("PLCtnRecvFMRgCodeDate", header: "Receive Date", iseditingreadonly: true)
                .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: colPackId)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CTNQty", header: "TTL. CTNs.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("NW", header: "TTL. N.W.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("GW", header: "TTL. G.W.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("NNW", header: "TTL. N.N.W.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CBM", header: "TTL. CBM.", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        private void Find()
        {

            string sqlWhere = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlWhere += " and pd.OrderID = @SPNo";
                listPar.Add(new SqlParameter("@SPNo", this.txtSPNo.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPONo.Text))
            {
                sqlWhere += " and o.CustPoNo = @CustPoNo";
                listPar.Add(new SqlParameter("@CustPoNo", this.txtPONo.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlWhere += " and pd.ID = @PackID";
                listPar.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            }

            if (!MyUtility.Check.Empty(this.txtFactory.Text))
            {
                sqlWhere += " and o.FactoryID = @FactoryID";
                listPar.Add(new SqlParameter("@FactoryID", this.txtFactory.Text));
            }

            if (this.dateBuyerDelivery.HasValue1)
            {
                sqlWhere += " and o.BuyerDelivery  >= @BuyerDeliveryFrom";
                listPar.Add(new SqlParameter("@BuyerDeliveryFrom", this.dateBuyerDelivery.Value1));
            }

            if (this.dateBuyerDelivery.HasValue2)
            {
                sqlWhere += " and o.BuyerDelivery  <= @BuyerDeliveryTo";
                listPar.Add(new SqlParameter("@BuyerDeliveryTo", this.dateBuyerDelivery.Value2));
            }

            if (this.dateReceive.HasValue1)
            {
                sqlWhere += " and p.PLCtnRecvFMRgCodeDate  >= @PLCtnRecvFMRgCodeDateFrom";
                listPar.Add(new SqlParameter("@PLCtnRecvFMRgCodeDateFrom", this.dateReceive.Value1));
            }

            if (this.dateReceive.HasValue2)
            {
                sqlWhere += " and p.PLCtnRecvFMRgCodeDate  <= @PLCtnRecvFMRgCodeDateTo";
                listPar.Add(new SqlParameter("@PLCtnRecvFMRgCodeDateTo", this.dateReceive.Value2));
            }

            string regionCode = MyUtility.GetValue.Lookup("select RgCode from system with (nolock)");

            string sqlGetData = $@"
select	[Remark] = '',
        [Fail] = 0,
        [selected] = 0,
        p.ID,
		o.FactoryID,
		pd.OrderID,
		o.StyleID,
		o.SeasonID,
		o.BrandID,
		o.CustPONo,
        c.Alias,
		o.BuyerDelivery,
		p.CTNQty,
		p.NW,
		p.GW,
		p.NNW,
		p.CBM,
        pd.DRYReceiveDate,
        pd.DRYTransferDate,
        pd.ReceiveDate,
        pd.DisposeFromClog,
        p.PLCtnRecvFMRgCodeDate
from PackingList p with (nolock)
inner join PackingList_Detail pd with (nolock) on pd.ID = p.ID
inner join Orders o with (nolock) on o.ID = pd.OrderID
inner join Country c with (nolock) on o.Dest = c.ID
left join   Pullout pu with (nolock) on pu.ID = p.PulloutID
where   p.Type = 'B' and
        (pu.Status = 'New' or pu.Status is null) and
        p.PLCtnTrToRgCodeDate is not null and
        p.PLCtnRecvFMRgCodeDate is not null and
        o.FactoryID in (select F.ID from Factory F where F.Junk = 0 and F.IsProduceFty = 1 and F.NegoRegion = '{regionCode}')
        {sqlWhere}
order by p.ID, pd.OrderID
";

            DataTable dtResult;

            PackingA2BWebAPI_Model.DataBySql dataBySql = new PackingA2BWebAPI_Model.DataBySql()
            {
                SqlString = sqlGetData,
                SqlParameter = listPar.ToListSqlPar(),
            };

            DualResult result = PackingA2BWebAPI.GetDataBySql(this.comboReceiveFrom.Text, dataBySql, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data found");
                return;
            }

            var listResultGroup = dtResult.AsEnumerable().GroupBy(s => s["ID"].ToString())
                .Where(s =>
                {
                    if (s.Any(detail => MyUtility.Check.Empty(detail["ReceiveDate"]) ||
                                    (bool)detail["DisposeFromClog"]))
                    {
                        return false;
                    }

                    int allDetailCnt = s.Count();
                    int dryReceiveDateNullCnt = s.Where(detail => MyUtility.Check.Empty(detail["DRYReceiveDate"])).Count();

                    if (allDetailCnt == dryReceiveDateNullCnt)
                    {
                        return true;
                    }

                    int dryReceiveDateNotNullCnt = s.Where(detail => !MyUtility.Check.Empty(detail["DRYReceiveDate"]) &&
                                                                  !MyUtility.Check.Empty(detail["DRYTransferDate"])).Count();

                    if (allDetailCnt == dryReceiveDateNotNullCnt)
                    {
                        return true;
                    }

                    return false;
                });

            DataTable dtResultGroup = dtResult.Clone();

            foreach (var item in listResultGroup)
            {
                DataRow dr = dtResultGroup.NewRow();

                dr["selected"] = 0;
                dr["ID"] = item.Key;
                dr["FactoryID"] = item.Select(s => s["FactoryID"].ToString()).Distinct().JoinToString(",");
                dr["OrderID"] = item.Select(s => s["OrderID"].ToString()).Distinct().JoinToString(",");
                dr["StyleID"] = item.Select(s => s["StyleID"].ToString()).Distinct().JoinToString(",");
                dr["SeasonID"] = item.Select(s => s["SeasonID"].ToString()).Distinct().JoinToString(",");
                dr["BrandID"] = item.Select(s => s["BrandID"].ToString()).Distinct().JoinToString(",");
                dr["CustPONo"] = item.Select(s => s["CustPONo"].ToString()).Distinct().JoinToString(",");
                dr["Alias"] = item.Select(s => s["Alias"].ToString()).Distinct().JoinToString(",");
                dr["BuyerDelivery"] = item.Select(s => Convert.ToDateTime(s["BuyerDelivery"]).ToString("yyyy/MM/dd")).Distinct().JoinToString(",");
                dr["CTNQty"] = item.First()["CTNQty"];
                dr["NW"] = item.First()["NW"];
                dr["GW"] = item.First()["GW"];
                dr["NNW"] = item.First()["NNW"];
                dr["CBM"] = item.First()["CBM"];
                dr["PLCtnRecvFMRgCodeDate"] = item.First()["PLCtnRecvFMRgCodeDate"];

                dtResultGroup.Rows.Add(dr);
            }

            this.gridReceiveSisterFty.DataSource = dtResultGroup;
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlPopup = $@"select F.ID 
from Factory F 
where F.Junk = 0  
 and F.NegoRegion = (select RgCode from system with (nolock)) 
 and F.IsProduceFty = 0
";
            SelectItem selectItem = new SelectItem(sqlPopup, "15", null);

            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            this.txtFactory.Text = selectItem.GetSelectedString();
        }

        private void TxtFactory_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtFactory.Text))
            {
                return;
            }

            string sqlCheck = $@"select 1
from Factory F 
where F.Junk = 0  
 and F.NegoRegion = (select RgCode from system with (nolock)) 
 and F.IsProduceFty = 0
 and ID = '{this.txtFactory.Text}'
";

            bool isExists = MyUtility.Check.Seek(sqlCheck);

            if (!isExists)
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Factory Not Found");
                return;
            }
        }

        private void ComboTransferTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtFactory.Text = string.Empty;
        }
    }
}
