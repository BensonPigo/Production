using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.IO;
using System.Transactions;
using Sci.Production.PublicPrg;
using System.Linq;
using Sci.Win.Tools;
using System.Data.SqlClient;
using System.Drawing;
using Sci.Production.CallPmsAPI;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P15
    /// </summary>
    public partial class P15 : Win.Tems.QueryForm
    {

        /// <summary>
        /// P15
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P15(ToolStripMenuItem menuitem)
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

        private string selectDataTable_DefaultView_Sort = string.Empty;

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridImport.IsEditingReadOnly = false;

            DataGridViewGeneratorCheckBoxColumnSettings col_chk = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_chk.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);
                dr["selected"] = e.FormattedValue;
                dr.EndEdit();
                int sint = ((DataTable)this.gridImport.DataSource).Select("selected = 1").Length;
                this.numSelectedPLQty.Value = sint;
            };

            DataGridViewGeneratorTextColumnSettings colPackId = new DataGridViewGeneratorTextColumnSettings();

            colPackId.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    string packID = this.gridImport.CurrentRow.Cells["ID"].Value.ToString();

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

                    DualResult result = PackingA2BWebAPI.GetDataBySql(this.comboReceiveFrom.Text, sqlGetPackDetail, out dtPAckDetail);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    MyUtility.Msg.ShowMsgGrid_LockScreen(dtPAckDetail, caption: "PackingList Detail");
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridImport)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: col_chk)
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
            .Numeric("CBM", header: "TTL. CBM.", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
        }

        private void GridStyleChange()
        {
            DataTable dtGridData = (DataTable)this.gridImport.DataSource;

            if (dtGridData == null)
            {
                return;
            }

            bool isShowRemark = dtGridData.AsEnumerable().Any(s => !MyUtility.Check.Empty(s["Remark"]));

            if (isShowRemark)
            {
                this.gridImport.Columns["Remark"].Visible = true;
            }
            else
            {
                this.gridImport.Columns["Remark"].Visible = false;
                return;
            }

            foreach (DataGridViewRow gridRow in this.gridImport.Rows)
            {
                if (!gridRow.Cells["Remark"].Value.ToString().ToUpper().Contains("SUCCESSFULLY"))
                {
                    gridRow.Cells["Remark"].Style.ForeColor = Color.Red;
                    isShowRemark = true;
                }
                else
                {
                    gridRow.Cells["Remark"].Style.ForeColor = gridRow.Cells["ID"].Style.ForeColor;
                }
            }
        }

        // Find
        private void Find()
        {
            this.numSelectedPLQty.Value = 0;
            this.numTotalPLQty.Value = 0;

            string sqlWhere = string.Empty;
            List<PackingA2BWebAPI_Model.SqlPar> listPar = new List<PackingA2BWebAPI_Model.SqlPar>();

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlWhere += " and pd.OrderID = @SPNo";
                listPar.Add(new PackingA2BWebAPI_Model.SqlPar("@SPNo", this.txtSPNo.Text, "string"));
            }

            if (!MyUtility.Check.Empty(this.txtPONo.Text))
            {
                sqlWhere += " and o.CustPoNo = @CustPoNo";
                listPar.Add(new PackingA2BWebAPI_Model.SqlPar("@CustPoNo", this.txtPONo.Text, "string"));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlWhere += " and pd.ID = @PackID";
                listPar.Add(new PackingA2BWebAPI_Model.SqlPar("@PackID", this.txtPackID.Text, "string"));
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlWhere += " and o.FactoryID = @FactoryID";
                listPar.Add(new PackingA2BWebAPI_Model.SqlPar("@FactoryID", this.txtfactory.Text, "string"));
            }

            if (this.dateBuyerDelivery.HasValue1)
            {
                sqlWhere += " and o.BuyerDelivery  >= @BuyerDeliveryFrom";
                listPar.Add(new PackingA2BWebAPI_Model.SqlPar("@BuyerDeliveryFrom", this.dateBuyerDelivery.Value1, "DateTime"));
            }

            if (this.dateBuyerDelivery.HasValue2)
            {
                sqlWhere += " and o.BuyerDelivery  <= @BuyerDeliveryTo";
                listPar.Add(new PackingA2BWebAPI_Model.SqlPar("@BuyerDeliveryTo", this.dateBuyerDelivery.Value2, "DateTime"));
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
        pd.DisposeFromClog
from PackingList p with (nolock)
inner join PackingList_Detail pd with (nolock) on pd.ID = p.ID
inner join Orders o with (nolock) on o.ID = pd.OrderID
inner join Country c with (nolock) on o.Dest = c.ID
left join   Pullout pu with (nolock) on pu.ID = p.PulloutID
where   p.Type = 'B' and
        (pu.Status = 'New' or pu.Status is null) and
        p.PLCtnTrToRgCodeDate is not null and
        p.PLCtnRecvFMRgCodeDate is null and
        o.FactoryID in (select F.ID from Factory F where F.Junk = 0 and F.IsProduceFty = 1 and F.NegoRegion = '{regionCode}')
        {sqlWhere}
order by p.ID, pd.OrderID
";

            DataTable dtResult;

            PackingA2BWebAPI_Model.DataBySql dataBySql = new PackingA2BWebAPI_Model.DataBySql()
            {
                SqlString = sqlGetData,
                SqlParameter = listPar,
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

                    int dryReceiveDateNotNull = s.Where(detail => !MyUtility.Check.Empty(detail["DRYReceiveDate"]) &&
                                                                  !MyUtility.Check.Empty(detail["DRYTransferDate"])).Count();

                    if (allDetailCnt == dryReceiveDateNotNull)
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

                dtResultGroup.Rows.Add(dr);
            }

            this.gridImport.DataSource = dtResultGroup;
            this.numTotalPLQty.Value = dtResultGroup.Rows.Count;
        }

        private void ButtonFind_Click(object sender, EventArgs e)
        {
            this.Find();
            this.GridStyleChange();
        }

        // Save
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            DataRow[] drSelecteds = ((DataTable)this.gridImport.DataSource).Select("selected = 1");

            if (drSelecteds.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first");
                return;
            }

            DataTable dtCheckResult;
            DualResult result;
            string wherePackID = drSelecteds.Select(s => $"'{s["ID"].ToString()}'").JoinToString(",");

            string sqlCheckPackStatus = $@"
select pd.ID,[Remark] = 'This PL carton not yet all transfer to Clog!'
from PackingList_Detail pd with (nolock)
where   pd.ID in ({wherePackID}) and pd.ReceiveDate is null
union all
select  pd.ID,[Remark] = 'This PL Cartons Dehumidifying Room has been received.'
from PackingList_Detail pd with (nolock)
where pd.ID in ({wherePackID}) and pd.DRYReceiveDate is not null and pd.DRYTransferDate is null
union all
select  p.ID,[Remark] = 'This PL Already pullout!'
from PackingList p with (nolock)
where   p.ID in ({wherePackID}) and
        exists(select 1 from Pullout pu with (nolock) where pu.ID = p.PulloutID and pu.Status in ('Confirmed', 'Locked'))
union all
select  p.ID,[Remark] = 'This PL not yet transfer to shipping factory!!'
from PackingList p with (nolock)
where   p.ID in ({wherePackID}) and p.PLCtnTrToRgCodeDate is null
union all
select  p.ID,[Remark] = 'This PL already received from shipping factory!!'
from PackingList p with (nolock)
where   p.ID in ({wherePackID}) and p.PLCtnRecvFMRgCodeDate is not null
";

            result = PackingA2BWebAPI.GetDataBySql(this.comboReceiveFrom.Text, sqlCheckPackStatus, out dtCheckResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            foreach (DataRow drSelected in drSelecteds)
            {
                drSelected["Remark"] = $"Successfully received from {this.comboReceiveFrom.Text}!!";
                drSelected["Fail"] = 0;
            }

            if (dtCheckResult.Rows.Count > 0)
            {
                var listCheckResultGroup = dtCheckResult.AsEnumerable().GroupBy(s => s["ID"].ToString());
                foreach (var groupItem in listCheckResultGroup)
                {
                    DataRow drWarning = drSelecteds.Where(s => s["ID"].ToString() == groupItem.Key).First();
                    drWarning["Remark"] = groupItem.Select(s => s["Remark"].ToString()).JoinToString(Environment.NewLine);
                    drWarning["Fail"] = 1;
                }
            }

            var canUpdateRow = drSelecteds.Where(s => MyUtility.Convert.GetInt(s["Fail"]) == 0);
            if (canUpdateRow.Any())
            {
                string canUpdatePackIDs = canUpdateRow.Select(s => $"'{s["ID"].ToString()}'").JoinToString(",");
                string updSql = $"update PackingList set PLCtnRecvFMRgCodeDate  = GetDate() where ID in ({canUpdatePackIDs})";

                result = PackingA2BWebAPI.ExecuteBySql(this.comboReceiveFrom.Text, updSql);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Save Success");

            this.GridStyleChange();
        }

        // Cancel
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridImport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.Countselectcount();
        }

        private void Countselectcount()
        {
            this.gridImport.ValidateControl();
            DataGridViewColumn column = this.gridImport.Columns["Selected"];
            if (!MyUtility.Check.Empty(column) && !MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                int sint = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").Length;
                this.numSelectedPLQty.Value = sint;
                this.numTotalPLQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count;
            }
        }

        private void Txtfactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlPopUpFactory = $@"
select F.ID 
from Factory F 
where F.Junk = 0  
 and F.NegoRegion = (select RgCode from system with (nolock)) 
 and F.IsProduceFty = 0
";
            SelectItem selectItem = new SelectItem(sqlPopUpFactory, null, null);

            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            this.txtfactory.Text = selectItem.GetSelectedString();
        }

        private void Txtfactory_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtfactory.Text))
            {
                return;
            }

            bool isExistsFactory = MyUtility.Check.Seek($@"select 1
from Factory F 
where F.Junk = 0  
 and F.NegoRegion = (select RgCode from system with (nolock)) 
 and F.IsProduceFty = 0
 and ID = '{this.txtfactory.Text}'");

            if (!isExistsFactory)
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Factory not Exists");
                return;
            }
        }

        private void ComboTransferTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtfactory.Text = string.Empty;
        }
    }
}
