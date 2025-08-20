using Ict;
using Ict.Win;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.CallPmsAPI;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.CallPmsAPI.PackingA2BWebAPI_Model;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P06
    /// </summary>
    public partial class P06 : Win.Tems.Input8
    {
        private string id;
        private DataGridViewGeneratorNumericColumnSettings shipqty = new DataGridViewGeneratorNumericColumnSettings();
        private ITableSchema revisedTS;
        private ITableSchema revised_detailTS;
        private DataTable PulloutReviseData;
        private DataTable PulloutReviseDetailData;

        /// <summary>
        /// P06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
            this.DoSubForm = new P06_ShipQtyDetail();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridicon.Insert.Visible = true;
            this.gridicon.Append.Visible = true;
            this.gridicon.Remove.Visible = true;

            #region 取TableSchema & 結構
            DBProxy.Current.GetTableSchema(null, "Pullout_Revise", out this.revisedTS);
            DBProxy.Current.GetTableSchema(null, "Pullout_Revise_Detail", out this.revised_detailTS);

            string sqlCmd = "select * from Pullout_Revise WITH (NOLOCK) where 1=0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.PulloutReviseData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Get Pullout_Revise structure fail!!\r\n" + result.ToString());
                return;
            }

            sqlCmd = "select * from Pullout_Revise_Detail WITH (NOLOCK) where 1=0";
            result = DBProxy.Current.Select(null, sqlCmd, out this.PulloutReviseDetailData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Get Pullout_Revise_Detail structure fail!!\r\n" + result.ToString());
                return;
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select pd.*,o.StyleID,o.BrandID,o.Dest,
Variance = (
	pd.OrderQty 
	- isnull((select sum(ShipQty) from Pullout_Detail WITH (NOLOCK) where OrderID = pd.OrderID),0)
	- isnull ((select sum(DiffQty) 
	from InvAdjust_Qty iq WITH (NOLOCK) 
	inner join InvAdjust i WITH (NOLOCK) on iq.ID = i.id 
	where i.orderid = pd.OrderID), 0)
),
case pd.Status 
when 'P' then 'Partial'
when 'C' then 'Complete'
when 'E' then 'Exceed'
when 'S' then 'Shortage'
else ''
end as StatusExp,
[PLFromRgCode] = isnull(gd.PLFromRgCode, '')
from Pullout_Detail pd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
left join GMTBooking_Detail gd with (nolock) on pd.INVNo = gd.ID and pd.PackingListID = gd.PackingListID
where pd.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "0" : MyUtility.Convert.GetString(e.Detail["UKey"]);

            this.SubDetailSelectCommand = string.Format(
                @"select pdd.*,oqd.Qty,(oqd.Qty-pdd.ShipQty) as Variance
from Pullout_Detail_Detail pdd WITH (NOLOCK) 
left join Pullout_Detail pd WITH (NOLOCK) on pd.UKey = pdd.Pullout_DetailUKey
left join Orders o WITH (NOLOCK) on o.ID = pdd.OrderID
left join Order_SizeCode os WITH (NOLOCK) on o.POID = os.Id and os.SizeCode = pdd.SizeCode
left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pdd.OrderID and oqd.Seq = pd.OrderShipmodeSeq and oqd.Article = pdd.Article and oqd.SizeCode = pdd.SizeCode
where pdd.Pullout_DetailUKey = {0}
order by os.Seq", masterID);
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label6.Visible = MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "LOCKED" ? true : false;
            this.btnHistory.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Pullout_History WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnRevisedHistory.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Pullout_Revise WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.shipqty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.DoSubForm.IsSupportDelete = false;
                    this.DoSubForm.IsSupportNew = false;
                    this.DoSubForm.IsSupportUpdate = false;
                    this.DoSubForm.EditMode = false;
                    this.OpenSubDetailPage();
                }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Order Shipmode Seq", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("ShipModeSeqQty", header: "Order Q'ty by Seq", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Ship Q’ty", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: this.shipqty)
                .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ExtText("StatusExp", header: "Status", width: Widths.AnsiChars(10), charCasing: CharacterCasing.Normal, iseditingreadonly: true)
                .Text("ShipmodeID", header: "Shipping Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("PackingListID", header: "Packing#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("INVNo", header: "Invoice No.", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("Remark", header: "Remarks", width: Widths.AnsiChars(30));
        }

        private void Status_Change()
        {
            DataTable dr = (DataTable)this.detailgridbs.DataSource;
            int i = 0;
            foreach (DataRow dr1 in dr.Rows)
            {
                switch (dr1["Status"].ToString().ToUpper())
                {
                    case "C":
                        this.detailgrid.Rows[i].Cells[9].Value = "Complete";
                        break;
                    case "P":
                        this.detailgrid.Rows[i].Cells[9].Value = "Partial";
                        break;
                    case "S":
                        this.detailgrid.Rows[i].Cells[9].Value = "Shortage";
                        break;
                    case "E":
                        this.detailgrid.Rows[i].Cells[9].Value = "Exceed";
                        break;
                    default:
                        this.detailgrid.Rows[i].Cells[9].Value = string.Empty;
                        break;
                }

                i++;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickNewBefore()
        {
            P06_Append callNextForm = new P06_Append();
            DialogResult dr = callNextForm.ShowDialog(this);

            // 當Form:P06_Append是按OK時，要新增一筆資料進Cursor
            if (dr == DialogResult.OK)
            {
                // 檢查此日期是否已存在資料庫
                if (MyUtility.Check.Seek(string.Format("select ID from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}'", Convert.ToDateTime(callNextForm.PulloutDate).ToString("yyyy/MM/dd"), Env.User.Keyword)))
                {
                    MyUtility.Msg.WarningBox(string.Format("Pull-out date:{0} already exists!!", callNextForm.PulloutDate.ToAppDateFormatString()));
                    return false;
                }

                // 檢查此日期是否小於System.PullLock
                if (callNextForm.PulloutDate <= (DateTime)MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select PullLock from System WITH (NOLOCK) ")))
                {
                    MyUtility.Msg.WarningBox("Pull-out date can't be earlier than pull-out lock date!!");
                    return false;
                }

                string newID = MyUtility.GetValue.GetID(Env.User.Keyword + "PO", "Pullout", callNextForm.PulloutDate, 2, "Id", null);
                string insertCmd = string.Format(
                    @"insert into Pullout(ID,PulloutDate,MDivisionID,FactoryID,Status,AddName,AddDate)
values('{0}','{1}','{2}','{3}','New','{4}',GETDATE());",
                    newID,
                    Convert.ToDateTime(callNextForm.PulloutDate).ToString("yyyy/MM/dd"),
                    Env.User.Keyword,
                    Env.User.Factory,
                    Env.User.UserID);

                DualResult result = DBProxy.Current.Execute(null, insertCmd);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Insert data fail, pls try again.\r\n" + result.ToString());
                    return false;
                }

                this.ReloadDatas();
                this.id = newID;

                // 點了排序不一定會在最後一筆
                int position = this.gridbs.Find("ID", this.id);
                this.gridbs.Position = position;

                // 因為新增資料一定會在最後一筆，所以直接把指標移至最後一筆
                // gridbs.MoveLast();
                // 模擬按Edit行為，強制讓畫面進入Detai頁籤，所以要將EditName與EditDate值給清空
                this.toolbar.cmdEdit.PerformClick();
                this.CurrentMaintain["EditName"] = string.Empty;
                this.CurrentMaintain["EditDate"] = DBNull.Value;
                this.editby.Value = string.Empty;
                this.ReviseData();
            }

            return false;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            // 如有問題請查詢：ISP20230067
            // #region  表身任一筆Orders.ID的Orders.GMTComplete 不可為 'S'
            // bool gMTCompleteCheck = this.GMTCompleteCheck();
            // if (!gMTCompleteCheck)
            // {
            //    return false;
            // }
            // #endregion
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() != "NEW")
            {
                MyUtility.Msg.WarningBox("This pullout report already confirmed, can't be edit!!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            string ttlShipQty = MyUtility.GetValue.Lookup($"select Sum(ShipQty) from Pullout_Detail WITH (NOLOCK) where ID = '{this.CurrentMaintain["ID"]}'");
            bool existsPullout_History = MyUtility.Check.Seek($"select 1 from Pullout_History WITH (NOLOCK) where ID = '{this.CurrentMaintain["ID"]}'");
            bool existsPullout_Revise = MyUtility.Check.Seek($"select 1 from Pullout_Revise WITH (NOLOCK) where ID = '{this.CurrentMaintain["ID"]}'");

            if (MyUtility.Check.Empty(ttlShipQty))
            {
                ttlShipQty = "0";
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New" || !MyUtility.Check.Empty(this.CurrentMaintain["SendToTPE"]) || ttlShipQty != "0" || existsPullout_History || existsPullout_Revise)
            {
                MyUtility.Msg.WarningBox("This pullout has transaction record, it cannot be delete!!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        private DualResult GetA2BPulloutCTNQtyForOrder(List<string> listPackID, out DataTable dtResult)
        {
            dtResult = new DataTable();
            if (listPackID.Count == 0)
            {
                return new DualResult(true);
            }

            string wherePackID = listPackID.Distinct().Select(s => $"'{s}'").JoinToString(",");

            // 抓取A2B的資料
            string sqlGetGMTBooking_Detail = $@"
select distinct PLFromRgCode, PackingListID 
from GMTBooking_Detail with (nolock)
where   PackingListID in ({wherePackID})
";

            DataTable dtGMTBooking_Detail;
            DualResult result = DBProxy.Current.Select(null, sqlGetGMTBooking_Detail, out dtGMTBooking_Detail);

            if (!result)
            {
                return result;
            }

            if (dtGMTBooking_Detail.Rows.Count == 0)
            {
                return new DualResult(true);
            }

            string sqlGetPulloutCTNQty = @"
select  pd.OrderID, PulloutCTNQty =isnull(sum(pd.CTNQty),0), [PLFromRgCode] = '{1}'
from PackingList_Detail pd, PackingList p
where   pd.OrderID in (select distinct OrderID from PackingList_Detail with (nolock) where ID in ({0})) and
        pd.ID = p.ID and
        p.PulloutID <> ''
group by pd.OrderID
";

            var listPackIdByPLFromRgCode = dtGMTBooking_Detail.AsEnumerable()
                                        .GroupBy(s => s["PLFromRgCode"].ToString())
                                        .Select(s => new
                                        {
                                            PLFromRgCode = s.Key,
                                            wherePackIdForA2B = s.Select(groupByItem => $"'{groupByItem["PackingListID"].ToString()}'").JoinToString(","),
                                        });

            foreach (var plFromRgCodeItem in listPackIdByPLFromRgCode)
            {
                DataTable dtResultA2B;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCodeItem.PLFromRgCode, string.Format(sqlGetPulloutCTNQty, plFromRgCodeItem.wherePackIdForA2B, plFromRgCodeItem.PLFromRgCode), out dtResultA2B);
                if (!result)
                {
                    return result;
                }

                if (dtResult.Rows.Count == 0)
                {
                    dtResult = dtResultA2B;
                }
                else
                {
                    dtResultA2B.MergeTo(ref dtResult);
                }
            }

            return new DualResult(true);
        }

        private DualResult UpdateOrdersPulloutCTNQty()
        {
            // 取得A2B資料
            List<string> listPackID = this.DetailDatas.Select(s => s["PackingListID"].ToString()).ToList();
            DataTable dtA2BPulloutCTNQty;
            DualResult result = this.GetA2BPulloutCTNQtyForOrder(listPackID, out dtA2BPulloutCTNQty);
            if (!result)
            {
                return result;
            }

            string updateCmd = string.Empty;

            if (dtA2BPulloutCTNQty.Rows.Count > 0)
            {
                updateCmd = $@"
alter table #tmp alter column OrderID varchar(13)
alter table #tmp alter column PLFromRgCode varchar(10)
alter table #tmp alter column PulloutCTNQty int

update Orders set PulloutCTNQty = (select isnull(sum(pd.CTNQty),0)
								   from PackingList_Detail pd, PackingList p
								   where pd.OrderID = Orders.ID 
								   and pd.ID = p.ID
								   and p.PulloutID <> '') +
                                  (select isnull(sum(t.PulloutCTNQty), 0)
                                   from #tmp t
                                   where t.OrderID = Orders.ID )  
where ID in (select distinct OrderID from Pullout_Detail where ID = '{this.CurrentMaintain["ID"]}');

select  t.PLFromRgCode, t.OrderID, o.PulloutCTNQty
from Orders o with (nolock)
inner join  #tmp t on o.ID = t.OrderID
";
                DataTable dtUpdateA2BInfo;

                result = MyUtility.Tool.ProcessWithDatatable(dtA2BPulloutCTNQty, null, updateCmd, out dtUpdateA2BInfo);

                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update orders fail!!\r\n" + result.ToString());
                    return failResult;
                }

                // 將 PulloutCTNQty 更新回A2B的其他DB
                if (dtUpdateA2BInfo.Rows.Count > 0)
                {
                    var updateList = dtUpdateA2BInfo.AsEnumerable()
                                                    .GroupBy(s => s["PLFromRgCode"].ToString())
                                                    .Select(s => new
                                                    {
                                                        PLFromRgCode = s.Key,
                                                        UpdateCmd = s.Select(updItem => $"update Orders set PulloutCTNQty = {updItem["PulloutCTNQty"]} where ID = '{updItem["OrderID"]}';").JoinToString(" "),
                                                    });

                    foreach (var updItem in updateList)
                    {
                        result = PackingA2BWebAPI.ExecuteBySql(updItem.PLFromRgCode, updItem.UpdateCmd);

                        if (!result)
                        {
                            return result;
                        }
                    }
                }
            }
            else
            {
                updateCmd = $@"
update Orders set PulloutCTNQty = (select isnull(sum(pd.CTNQty),0)
								   from PackingList_Detail pd, PackingList p
								   where pd.OrderID = Orders.ID 
								   and pd.ID = p.ID
								   and p.PulloutID <> '')
where ID in (select distinct OrderID from Pullout_Detail where ID = '{this.CurrentMaintain["ID"]}');
";
                result = DBProxy.Current.Execute(null, updateCmd);

                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update orders fail!!\r\n" + result.ToString());
                    return failResult;
                }
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            // 刪除後將Pullout箱數回寫回Orders.PulloutCTNQty
            DualResult result = this.UpdateOrdersPulloutCTNQty();

            if (!result)
            {
                return result;
            }

            return base.ClickDeletePost();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.CheckIDD();
            this.CheckPulloutputIDD();
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            DualResult result;
            if (!MyUtility.Check.Empty(this.CurrentMaintain["SendToTPE"]))
            {
                Dictionary<string, IList<string>> dicUpdataCmdByPLFromRgCode = new Dictionary<string, IList<string>>();

                foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Unchanged)
                    {
                        continue;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        dr["AddName"] = Sci.Env.User.UserID;
                        dr["AddDate"] = DateTime.Now;
                        result = this.WriteRevise("Missing", dr);
                        if (!result)
                        {
                            return result;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        string cmd = $@"
SELECT * 
FROM Pullout_Detail WITH(NOLOCK)
WHERE Ukey = {MyUtility.Convert.GetString(dr["Ukey"])}
";
                        DataTable dataTable;
                        DBProxy.Current.Select(null, cmd, out dataTable);

                        DataRow dbRow = dataTable.Rows[0];

                        if (MyUtility.Convert.GetString(dbRow["PulloutDate"]) != MyUtility.Convert.GetString(dr["PulloutDate"]) ||
                            MyUtility.Convert.GetString(dbRow["OrderID"]) != MyUtility.Convert.GetString(dr["OrderID"]) ||
                            MyUtility.Convert.GetString(dbRow["OrderShipmodeSeq"]) != MyUtility.Convert.GetString(dr["OrderShipmodeSeq"]) ||
                            MyUtility.Convert.GetString(dbRow["ShipQty"]) != MyUtility.Convert.GetString(dr["ShipQty"]) ||
                            MyUtility.Convert.GetString(dbRow["OrderQty"]) != MyUtility.Convert.GetString(dr["OrderQty"]) ||
                            MyUtility.Convert.GetString(dbRow["ShipModeSeqQty"]) != MyUtility.Convert.GetString(dr["ShipModeSeqQty"]) ||
                            MyUtility.Convert.GetString(dbRow["Status"]) != MyUtility.Convert.GetString(dr["Status"]) ||
                            MyUtility.Convert.GetString(dbRow["PackingListID"]) != MyUtility.Convert.GetString(dr["PackingListID"]) ||
                            MyUtility.Convert.GetString(dbRow["PackingListType"]) != MyUtility.Convert.GetString(dr["PackingListType"]) ||
                            MyUtility.Convert.GetString(dbRow["INVNo"]) != MyUtility.Convert.GetString(dr["INVNo"]) ||
                            MyUtility.Convert.GetString(dbRow["ShipmodeID"]) != MyUtility.Convert.GetString(dr["ShipmodeID"]) ||
                            MyUtility.Convert.GetString(dbRow["Remark"]) != MyUtility.Convert.GetString(dr["Remark"]) ||
                            MyUtility.Convert.GetString(dbRow["ReviseDate"]) != MyUtility.Convert.GetString(dr["ReviseDate"]))
                        {
                            dr["EditName"] = Sci.Env.User.UserID;
                            dr["EditDate"] = DateTime.Now;
                        }

                        bool isSubDetailDataChanged = false;
                        DataTable subDetailData;
                        this.GetSubDetailDatas(dr, out subDetailData);
                        foreach (DataRow tdr in subDetailData.Rows)
                        {
                            if (tdr.RowState != DataRowState.Unchanged)
                            {
                                isSubDetailDataChanged = true;
                                break;
                            }
                        }

                        if (isSubDetailDataChanged || dr["ShipModeID", DataRowVersion.Original].EqualString(dr["ShipModeID"]) == false)
                        {
                            result = this.WriteRevise("Revise", dr);
                            if (!result)
                            {
                                return result;
                            }

                            #region update PulloutID 到PackingList
                            string packingListID;
                            string pulloutID;
                            if (MyUtility.Check.Empty(dr["PackingListID"]))
                            {
                                packingListID = MyUtility.Convert.GetString(dr["PackingListID", DataRowVersion.Original]);
                                pulloutID = string.Empty;
                            }
                            else
                            {
                                packingListID = MyUtility.Convert.GetString(dr["PackingListID"]);
                                pulloutID = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
                            }

                            // 如果pulloutID被清空, 那PulloutStatus也要清空
                            string updatePklst = string.Empty;
                            if (MyUtility.Check.Empty(pulloutID))
                            {
                                updatePklst = $@"Update PackingList set pulloutID = '',PulloutStatus = '' where id='{packingListID}';";
                            }
                            else
                            {
                                updatePklst = $@"Update PackingList set pulloutID = '{pulloutID}' where id='{packingListID}';";
                            }

                            string plFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByPackID(packingListID);

                            if (!dicUpdataCmdByPLFromRgCode.ContainsKey(plFromRgCode))
                            {
                                dicUpdataCmdByPLFromRgCode.Add(plFromRgCode, new List<string>());
                            }

                            dicUpdataCmdByPLFromRgCode[plFromRgCode].Add(updatePklst);
                            #endregion
                        }
                    }
                    else if (dr.RowState == DataRowState.Deleted)
                    {
                        result = this.WriteRevise("Delete", dr);
                        if (!result)
                        {
                            return result;
                        }

                        #region update PulloutID 到PackingList
                        string updatePklst = $@"Update PackingList set pulloutID = '',PulloutStatus = '' where id='{dr["PackingListID", DataRowVersion.Original]}';";

                        string plFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByPackID(dr["PackingListID", DataRowVersion.Original].ToString());

                        if (!dicUpdataCmdByPLFromRgCode.ContainsKey(plFromRgCode))
                        {
                            dicUpdataCmdByPLFromRgCode.Add(plFromRgCode, new List<string>());
                        }

                        dicUpdataCmdByPLFromRgCode[plFromRgCode].Add(updatePklst);
                        #endregion
                    }
                }

                foreach (KeyValuePair<string, IList<string>> updateCmdItem in dicUpdataCmdByPLFromRgCode.OrderBy(s => s.Key))
                {
                    // PLFromRgCode 空白表示是local非A2B
                    if (MyUtility.Check.Empty(updateCmdItem.Key))
                    {
                        result = DBProxy.Current.Executes(null, updateCmdItem.Value);
                    }
                    else
                    {
                        result = PackingA2BWebAPI.ExecuteBySql(updateCmdItem.Key, updateCmdItem.Value.JoinToString(" "));
                    }

                    if (!result)
                    {
                        return result;
                    }
                }
            }

            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            DualResult result;

            #region 存檔前檢查是否有重複的表身資料
            string cmd = $@"
--ClickSavePost尚未Commit，因此使用 WITH(NOLOCK) 檢視Commit後的Pullout_Detail會不會有重複的資料
SELECT [ID]
      ,[OrderID]
      ,[OrderShipmodeSeq]
      ,[PackingListID]
      ,[Count]=COUNT([UKey])
FROM Pullout_Detail WITH(NOLOCK)
WHERE ID ='{this.CurrentMaintain["ID"]}' 
      and [PackingListID] != ''
GROUP BY [ID]
      ,[OrderID]
      ,[OrderShipmodeSeq]
      ,[PackingListID]
HAVING COUNT([UKey]) > 1

";

            bool hasDuplicate = MyUtility.Check.Seek(cmd);
            if (hasDuplicate)
            {
                return new DualResult(false, "Detail data is not lastest, please click <Undo> and <Refresh> data.");
            }
            #endregion

            using (TransactionScope scope = new TransactionScope())
            {
                if (this.updatePackinglist.Trim() != string.Empty)
                {
                    result = DBProxy.Current.Execute(null, this.updatePackinglist);
                    if (!result)
                    {
                        scope.Dispose();
                        DualResult failResult = new DualResult(false, "Update Packinglist fail!!\r\n" + result.ToString());
                        return failResult;
                    }
                }

                // 將Pullout箱數回寫回Orders.PulloutCTNQty
                result = this.UpdateOrdersPulloutCTNQty();

                if (!result)
                {
                    scope.Dispose();
                    return result;
                }

                // update A2B Packing Pullout Info
                foreach (KeyValuePair<string, List<string>> itemUpdateA2B in this.dicUpdatePackinglistA2B)
                {
                    result = PackingA2BWebAPI.ExecuteBySql(itemUpdateA2B.Key, itemUpdateA2B.Value.JoinToString(" "));
                    if (!result)
                    {
                        scope.Dispose();
                        return result;
                    }
                }

                // 最後檢查 每一筆 pullout_detail 對應的 Packing_Detai.PulloutID,PulloutStatus 是否正確
                string sqlCmd = $@"
SELECT DISTINCT pld.ID
FROM Packinglist_detail pld WITH(NOLOCK)
INNER JOIN PackingList pl WITH(NOLOCK) ON pl.ID = pld.ID
INNER JOIN Pullout_Detail pod WITH(NOLOCK) ON pl.ID = pod.PackingListID
INNER JOIN Pullout p WITH(NOLOCK) ON p.ID = pod.ID
WHERE pod.ID = '{this.CurrentMaintain["ID"]}'
AND pl.PulloutID <> pod.ID
";
                DBProxy.Current.Select(null, sqlCmd, out DataTable dtCheckPulloutID);
                if (dtCheckPulloutID.Rows.Count > 0)
                {
                    scope.Dispose();
                    string pkIds = dtCheckPulloutID.AsEnumerable().Select(s => s["ID"].ToString()).JoinToString(",");
                    string msg = $@"Packing list update failed. Please report to the IT department.
Packing List: {pkIds}";
                    return new DualResult(false, msg);
                }

                scope.Complete();
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                return false;
            }

            string sqlCmd = string.Format(
                @"select pd.OrderID,o.StyleID,o.CustPONo,o.BrandID,c.NameEN,o.StyleUnit,o.Qty,pd.ShipQty,
(select isnull(sum(ShipQty),0) from Pullout_Detail WITH (NOLOCK) where OrderID = pd.OrderID) as TtlShipQty,
(select isnull(sum(CTNQty),0) from PackingList_Detail WITH (NOLOCK) where ID = pd.PackingListID and OrderID = pd.OrderID) as CtnQty,
pd.Remark,pd.ShipmodeID,pd.INVNo,
[PLFromRgCode] = isnull(gd.PLFromRgCode, ''),
pd.PackingListID
from Pullout_Detail pd WITH (NOLOCK) 
left join GMTBooking_Detail gd with (nolock) on pd.INVNo = gd.ID and pd.PackingListID = gd.PackingListID
left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
where pd.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DataTable excelData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out excelData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            var listExcelDataA2B = excelData.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["PLFromRgCode"]));
            if (listExcelDataA2B.Any())
            {
                string sqlGetA2BCTNQty = @"
alter table #tmp alter column OrderID varchar(13)
alter table #tmp alter column PackingListID varchar(13)

select  t.PackingListID, t.OrderID, [CTNQty] = isnull(sum(CTNQty),0)
from PackingList_Detail pd WITH (NOLOCK) 
inner join  #tmp t on pd.ID = t.PackingListID and pd.OrderID = t.OrderID
group by t.PackingListID, t.OrderID
";
                foreach (string plFromRgCode in listExcelDataA2B.Select(s => s["PLFromRgCode"].ToString()).Distinct())
                {
                    DataTable tmpDataByPLFromRgCode = listExcelDataA2B.Where(s => s["PLFromRgCode"].ToString() == plFromRgCode).CopyToDataTable();

                    DataBySql dataBySql = new DataBySql()
                    {
                        SqlString = sqlGetA2BCTNQty,
                        TmpCols = "PackingListID,OrderID",
                        TmpTable = JsonConvert.SerializeObject(tmpDataByPLFromRgCode),
                    };

                    DataTable dtResultA2B;

                    result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, dataBySql, out dtResultA2B);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return false;
                    }

                    foreach (DataRow dr in dtResultA2B.Rows)
                    {
                        DataRow[] drA2Bs = excelData.Select($"PackingListID = '{dr["PackingListID"]}' and OrderID = '{dr["OrderID"]}'");
                        if (drA2Bs.Length == 0)
                        {
                            continue;
                        }

                        foreach (DataRow updateDr in drA2Bs)
                        {
                            updateDr["CTNQty"] = dr["CTNQty"];
                        }
                    }
                }
            }

            DataTable ttlQty;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(excelData, "OrderId,Qty", "select isnull(sum(a.Qty),0) as TtlQty from (select distinct OrderId,Qty from #tmp) a", out ttlQty);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Prepare data error.\r\n" + ex.ToString());
                return false;
            }

            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P06.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = MyUtility.Convert.GetString(this.CurrentMaintain["MDivisionID"]);
            worksheet.Cells[3, 2] = Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("yyyy/MM/dd");
            worksheet.Cells[3, 12] = DateTime.Now.ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));

            int intRowsStart = 5;
            int dataRowCount = excelData.Rows.Count;
            int rownum = 0;
            object[,] objArray = new object[1, 13];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = excelData.Rows[i];
                rownum = intRowsStart + i;
                objArray[0, 0] = dr["OrderID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["CustPONo"];
                objArray[0, 3] = dr["BrandID"];
                objArray[0, 4] = dr["NameEN"];
                objArray[0, 5] = dr["StyleUnit"];
                objArray[0, 6] = dr["Qty"];
                objArray[0, 7] = dr["ShipQty"];
                objArray[0, 8] = dr["TtlShipQty"];
                objArray[0, 9] = dr["CtnQty"];
                objArray[0, 10] = dr["Remark"];
                objArray[0, 11] = dr["ShipmodeID"];
                objArray[0, 12] = dr["INVNo"];

                worksheet.Range[string.Format("A{0}:M{0}", rownum)].Value2 = objArray;
            }

            worksheet.Range[string.Format("A5:M{0}", rownum)].Borders.Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A5:M{0}", rownum)].Borders.LineStyle = 1;
            worksheet.Cells[rownum + 1, 6] = "TTL:";
            worksheet.Cells[rownum + 1, 7] = ttlQty.Rows[0]["TtlQty"];
            worksheet.Cells[rownum + 1, 8] = excelData.Compute("sum(ShipQty)", string.Empty);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_P06");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            // 如有問題請查詢：ISP20230067
            // #region  表身任一筆Orders.ID的Orders.GMTComplete 不可為 'S'
            // bool gMTCompleteCheck = this.GMTCompleteCheck();
            // if (!gMTCompleteCheck)
            // {
            //    return;
            // }
            // #endregion
            if (MyUtility.Convert.GetDate(this.CurrentMaintain["PulloutDate"]) > DateTime.Today)
            {
                MyUtility.Msg.WarningBox("Pullout date can't greater than today!");
                return;
            }

            // 判斷 Packing List 的 Ship Mode 是否與業務設定的一致
            if (!this.CheckShipMode())
            {
                return;
            }

            if (!Prgs.CheckExistsOrder_QtyShip_Detail(pulloutID: MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
            {
                return;
            }

            // 有Cancel Order 不能confirmed
            bool errchk = false;
            string strErrmsg = Prgs.ChkCancelOrder(this.CurrentMaintain["id"].ToString());
            if (!MyUtility.Check.Empty(strErrmsg))
            {
                MyUtility.Msg.WarningBox(strErrmsg);
                return;
            }

            if (errchk)
            {
                MyUtility.Msg.WarningBox(strErrmsg.ToString());
                return;
            }

            // 模擬按Edit行為
            this.toolbar.cmdEdit.PerformClick();

            // 當Revise有錯誤時就不做任何事
            if (!this.ReviseData())
            {
                this.toolbar.cmdUndo.PerformClick();
                return;
            }

            // 模擬按Edit行為
            this.toolbar.cmdSave.PerformClick();

            this.OnDetailEntered();

            #region 檢查Variance是否<0, 若<0則不能confirm 這段
            StringBuilder errmsg = new StringBuilder();
            errmsg.Append("Cannot confirm this Pullout!!\r\n");
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (MyUtility.Convert.GetDecimal(dr["Variance"]) < 0)
                {
                    errchk = true;
                    errmsg.Append(string.Format("Please check <SP#> {0}, Variance:{1}\r\n", MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetDecimal(dr["Variance"])));
                }
            }

            #endregion

            // 檢查表身資料不可為空
            if (((DataTable)this.detailgridbs.DataSource).Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Details can't empty!!");
                return;
            }

            #region 檢查clog是否有收完CFA箱子
            DataTable dtCfa;
            DualResult resultCFA;
            StringBuilder warningmsg = new StringBuilder();
            string strSqlcmd =
                   $@"
SELECT DISTINCT c.OrderID,c.ID as PackingListID,c.CTNStartNo 
FROM Pullout a
inner join PackingList b on a.ID=b.PulloutID
inner join PackingList_Detail c on b.ID=c.ID
where a.ID='{this.CurrentMaintain["id"]}' and a.Status='New'
and c.CFAReturnClogDate is not null
and c.ClogReceiveCFADate is null
";

            resultCFA = DBProxy.Current.Select(null, strSqlcmd, out dtCfa);

            if (!resultCFA)
            {
                this.ShowErr(resultCFA);
                return;
            }

            List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByPulloutID(this.CurrentMaintain["id"].ToString());
            string strSqlcmdA2B = $@"
select DISTINCT pd.OrderID, pd.ID as PackingListID, pd.CTNStartNo
from    PackingList p with (nolock)
inner join PackingList_Detail pd on p.ID = pd.ID
where   p.PulloutID = '{this.CurrentMaintain["id"]}' and
        pd.CFAReturnClogDate is not null and
        pd.ClogReceiveCFADate is null
";

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtCfaA2B;

                resultCFA = PackingA2BWebAPI.GetDataBySql(plFromRgCode, strSqlcmdA2B, out dtCfaA2B);

                if (!resultCFA)
                {
                    this.ShowErr(resultCFA);
                    return;
                }

                dtCfaA2B.MergeTo(ref dtCfa);
            }

            if (dtCfa.Rows.Count > 0)
            {
                warningmsg.Append("Below <OrderID>+<PackingID>+<CTNNo> has been returned from CFA but not yet received from Clog!!\r\n");
                foreach (DataRow dr in dtCfa.Rows)
                {
                    warningmsg.Append($@"<{dr["OrderID"]}>+<{dr["PackingListID"]}>+<{dr["CTNStartNo"]}>" + Environment.NewLine);
                }

                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return;
            }
            #endregion

            #region 檢查Clog已送出給CFA，但CFA還沒退回給Clog
            string sqlcmd = $@"
SELECT DISTINCT
    c.OrderID
   ,c.ID
   ,c.CTNStartNo
FROM Pullout a WITH (NOLOCK)
INNER JOIN PackingList b WITH (NOLOCK) ON a.ID = b.PulloutID
INNER JOIN PackingList_Detail c WITH (NOLOCK) ON b.ID = c.ID
WHERE a.ID = '{this.CurrentMaintain["id"]}'
AND a.Status = 'New'
AND ((c.CFAReturnClogDate IS NOT NULL　AND c.ClogReceiveCFADate IS NULL)
　　　OR c.TransferCFADate IS NOT NULL)
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtchk);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtchk.Rows.Count > 0)
            {
                string msgchk = "Below <OrderID>+<PackingID>+<CTNNo> has been sent to CFA from Clog, but has not yet returned to Clog.\r\n";
                foreach (DataRow dr in dtchk.Rows)
                {
                    msgchk += $"<{dr["OrderID"]}>+<{dr["ID"]}>+<{dr["CTNStartNo"]}>\r\n";
                }

                MyUtility.Msg.WarningBox(msgchk);
                return;
            }

            #endregion

            #region 檢查 Clog or CFA 退回給 factory(PackingList.detail.ReceiveDate為空)
            sqlcmd = $@"
SELECT DISTINCT
    c.OrderID
   ,c.ID
   ,c.CTNStartNo
FROM Pullout a WITH (NOLOCK)
INNER JOIN PackingList b WITH (NOLOCK) ON a.ID = b.PulloutID
INNER JOIN PackingList_Detail c WITH (NOLOCK) ON b.ID = c.ID
WHERE a.ID = '{this.CurrentMaintain["id"]}'
AND a.Status = 'New'
AND c.ReceiveDate is null
AND b.type in ('B', 'L')
";
            result = DBProxy.Current.Select(null, sqlcmd, out dtchk);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtchk.Rows.Count > 0)
            {
                string msgchk = "Below <OrderID>+<PackingID>+<CTNNo> has not yet received by Clog.\r\n";
                foreach (DataRow dr in dtchk.Rows)
                {
                    msgchk += $"<{dr["OrderID"]}>+<{dr["ID"]}>+<{dr["CTNStartNo"]}>\r\n";
                }

                MyUtility.Msg.WarningBox(msgchk);
                return;
            }

            #endregion

            IList<string> updateCmds = new List<string>();
            updateCmds.Add(string.Format("update Pullout set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}';", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            // 更新 PackingList.PulloutStatus
            updateCmds.Add($@"update PackingList set PulloutStatus = 'Confirmed' where ID in (select PackingListID from Pullout_Detail where ID = '{this.CurrentMaintain["ID"]}');");
            if (!MyUtility.Check.Empty(this.CurrentMaintain["SendToTPE"]))
            {
                updateCmds.Add(string.Format(
                    @"insert into Pullout_History(ID, HisType, NewValue, AddName, AddDate) values ('{0}','Status','Confirmed','{1}',GETDATE());",
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    Env.User.UserID));
            }

            updateCmds.Add(
                    $@"
declare  @updateOrderInfo table
	(
		OrderID varchar(13),
        ActPulloutDate date,
        PulloutComplete bit,
        PulloutCmplDate date
	)

update o
set ActPulloutDate = ActPulloutDate.value
	, PulloutComplete = PulloutComplete.value
    , PulloutCmplDate = CAST(GetDate() AS DATE)
output	inserted.ID,
	    inserted.ActPulloutDate,
        inserted.PulloutComplete,
        inserted.PulloutCmplDate
		into @updateOrderInfo
from Orders o
outer apply (
	SELECT value =  Max(p.pulloutdate)
	FROM pullout_detail pd inner join pullout p on pd.id = p.id 
	WHERE  pd.orderid = o.id 
			AND (pd.status = 'C' or pd.ShipQty > 0)
			AND p.status != 'New'
) ActPulloutDate
outer apply (
	select value = CASE
						WHEN (
			                SELECT SUM(ShipQty)
			                FROM
			                (
                                ---- 自己這張以外的已出貨數 加總
				                SELECT [ShipQty]=ISNULL( SUM(pd.ShipQty),0)
				                FROM Pullout p
				                INNER JOIN Pullout_Detail pd ON pd.ID=p.ID
				                WHERE OrderID = o.ID
				                        AND p.Status <> 'New'
				                        AND p.ID != '{this.CurrentMaintain["ID"]}'
				                UNION ALL
                                ---- 當下這筆，因為還沒confirm，在上面 <> 'New'的條件下找不到，要額外列進來計算
				                SELECT [ShipQty]=ISNULL( SUM(pd.ShipQty),0)
				                FROM Pullout p
				                INNER JOIN Pullout_Detail pd ON pd.ID=p.ID
				                WHERE OrderID = o.ID
				                        AND p.ID='{this.CurrentMaintain["ID"]}'
			                ) t
			                ) >= o.Qty THEN 1
                        WHEN o.GMTComplete = 'C' and o.PulloutComplete = 1 THEN 1
		                ELSE 0 
		                END
) PulloutComplete
where	exists (
			select 1
			from Pullout_Detail pd
			where pd.ID = '{this.CurrentMaintain["ID"]}'
				  and pd.OrderID = o.ID 
                )


select  distinct
        uo.OrderID,
        uo.ActPulloutDate,
        uo.PulloutComplete,
        uo.PulloutCmplDate,
        gd.PLFromRgCode
from    Pullout_Detail pd with (nolock)   
inner join  @updateOrderInfo uo on uo.OrderID = pd.OrderID
inner join  GMTBooking_Detail gd with (nolock) on gd.ID = pd.INVNo and gd.PackingListID = pd.PackingListID
where   pd.ID = '{this.CurrentMaintain["ID"]}'

");
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    result = DBProxy.Current.Select(null, updateCmds.JoinToString(" "), out DataTable dtNeedUpdateA2BOrders);
                    if (!result)
                    {
                        scope.Dispose();
                        MyUtility.Msg.WarningBox("Confirmed fail!!\r\n" + result.ToString());
                        return;
                    }

                    if (dtNeedUpdateA2BOrders.Rows.Count > 0)
                    {
                        string sqlUpdOrdersA2B = @"
alter table #tmp alter column OrderID varchar(13)

update o set o.ActPulloutDate = t.ActPulloutDate,
        o.PulloutComplete = t.PulloutComplete,
        o.PulloutCmplDate = t.PulloutCmplDate
from    Orders o 
inner join  #tmp t on t.OrderID = o.ID
";

                        var groupNeedUpdateA2BOrders = dtNeedUpdateA2BOrders.AsEnumerable().GroupBy(s => s["PLFromRgCode"].ToString());
                        foreach (var groupA2BItem in groupNeedUpdateA2BOrders)
                        {
                            DataTable dtUpdTmp = groupA2BItem.CopyToDataTable();
                            DataBySql dataBySql = new DataBySql()
                            {
                                SqlString = sqlUpdOrdersA2B,
                                TmpTable = JsonConvert.SerializeObject(dtUpdTmp),
                            };

                            result = PackingA2BWebAPI.ExecuteBySql(groupA2BItem.Key, dataBySql);
                            if (!result)
                            {
                                scope.Dispose();
                                MyUtility.Msg.WarningBox("Confirmed fail!!\r\n" + result.ToString());
                                return;
                            }
                        }
                    }

                    // 更新PackingList.PulloutStatus
                    if (!(result = this.UpdatePacking_PulloutStatus()))
                    {
                        scope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    this.ShowErr(ex);
                    return;
                }
            }

            #region ISP20200757 資料交換 - Sunrise
            if (PMSUtilityAutomation.IsSunrise_FinishingProcessesEnable)
            {
                string listOrderID = this.DetailDatas.Select(s => s["OrderID"].ToString()).JoinToString(",");
                Task.Run(() => new Sunrise_FinishingProcesses().SentOrdersToFinishingProcesses(listOrderID, "Orders,Order_QtyShip,Order_Qty"))
                               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion

            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                string listOrderID = this.DetailDatas.Select(s => s["OrderID"].ToString()).JoinToString(",");
                Task.Run(() => new Gensong_FinishingProcesses().SentOrdersToFinishingProcesses(listOrderID, "Orders,Order_QtyShip"))
                       .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            IList<string> sqlCmds = new List<string>();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["SendToTPE"]))
            {
                Win.UI.SelectReason callReason = new Win.UI.SelectReason("Pullout_Delay");
                DialogResult dResult = callReason.ShowDialog(this);
                if (dResult == DialogResult.OK)
                {
                    sqlCmds.Add(string.Format(
                        @"insert into Pullout_History(ID, HisType, NewValue, ReasonID,Remark, AddName, AddDate) values ('{0}','Status','Unconfirmed','{1}','{2}','{3}',GETDATE());",
                        MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                        callReason.ReturnReason,
                        callReason.ReturnRemark,
                        Env.User.UserID));
                }
                else
                {
                    return;
                }
            }

            sqlCmds.Add(string.Format(
                "update Pullout set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}';",
                Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            string selectCmd = string.Format(
                @"with PulloutOrder
as
(
select distinct OrderID 
from Pullout_Detail WITH (NOLOCK) 
where ID = '{0}' and ShipQty > 0
),
PulloutDate
as
(
select pd.OrderID, max(pd.PulloutDate) as PulloutDate from Pullout_Detail pd WITH (NOLOCK) , Pullout p WITH (NOLOCK) 
where exists (select 1 from PulloutOrder where OrderID = pd.OrderID)
and pd.ID <> '{0}'
and pd.ID = p.ID
and p.Status <> 'New'
group by pd.OrderID
)
select po.OrderID,pd.PulloutDate
from PulloutOrder po 
left join PulloutDate pd on pd.OrderID = po.OrderID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable updateOrderData;
            DualResult result = DBProxy.Current.Select(null, selectCmd, out updateOrderData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query update order data fail!! Please try again.\r\n" + result.ToString());
                return;
            }

            sqlCmds.Add(@"
declare  @updateOrderInfo table
	(
		OrderID varchar(13),
        ActPulloutDate date,
        PulloutComplete bit,
        PulloutCmplDate date
	)
");

            foreach (DataRow dr in updateOrderData.Rows)
            {
                string actPulloutDate = MyUtility.Check.Empty(dr["PulloutDate"]) ? "null" : "'" + Convert.ToDateTime(dr["PulloutDate"]).ToString("yyyy/MM/dd") + "'";
                string orderid = MyUtility.Convert.GetString(dr["OrderID"]);
                sqlCmds.Add($@"


update Orders set 
ActPulloutDate = {actPulloutDate}, 
PulloutComplete = case when exists(select 1 from Order_Finish ox where ox.ID = orders.id) 
                       then pulloutcomplete 
                       when orders.GMTComplete = 'C' and orders.PulloutComplete = 1 then 1
                       else 0 end,
PulloutCmplDate  = Cast(GetDate() as date)
output	inserted.ID,
	    inserted.ActPulloutDate,
        inserted.PulloutComplete,
        inserted.PulloutCmplDate
		into @updateOrderInfo
where ID = '{orderid}'
;
");
            }

            sqlCmds.Add($@"
select  distinct
        uo.OrderID,
        uo.ActPulloutDate,
        uo.PulloutComplete,
        uo.PulloutCmplDate,
        gd.PLFromRgCode
from    Pullout_Detail pd with (nolock)   
inner join  @updateOrderInfo uo on uo.OrderID = pd.OrderID
inner join  GMTBooking_Detail gd with (nolock) on gd.ID = pd.INVNo and gd.PackingListID = pd.PackingListID
where   pd.ID = '{this.CurrentMaintain["ID"]}'
");

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    result = DBProxy.Current.Select(null, sqlCmds.JoinToString(Environment.NewLine), out DataTable dtNeedUpdateA2BOrders);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Amend fail!!\r\n" + result.ToString());
                        return;
                    }

                    if (dtNeedUpdateA2BOrders.Rows.Count > 0)
                    {
                        string sqlUpdOrdersA2B = @"
alter table #tmp alter column OrderID varchar(13)

update o set o.ActPulloutDate = t.ActPulloutDate,
        o.PulloutComplete = t.PulloutComplete,
        o.PulloutCmplDate = t.PulloutCmplDate
from    Orders o 
inner join  #tmp t on t.OrderID = o.ID
";
                        var groupNeedUpdateA2BOrders = dtNeedUpdateA2BOrders.AsEnumerable().GroupBy(s => s["PLFromRgCode"].ToString());
                        foreach (var groupA2BItem in groupNeedUpdateA2BOrders)
                        {
                            DataTable dtUpdTmp = groupA2BItem.CopyToDataTable();
                            DataBySql dataBySql = new DataBySql()
                            {
                                SqlString = sqlUpdOrdersA2B,
                                TmpTable = JsonConvert.SerializeObject(dtUpdTmp),
                            };

                            result = PackingA2BWebAPI.ExecuteBySql(groupA2BItem.Key, dataBySql);
                            if (!result)
                            {
                                scope.Dispose();
                                MyUtility.Msg.WarningBox("Confirmed fail!!\r\n" + result.ToString());
                                return;
                            }
                        }
                    }

                    // 更新PackingList.PulloutStatus
                    if (!(result = this.UpdatePacking_PulloutStatus()))
                    {
                        scope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    this.ShowErr(ex);
                    return;
                }
            }

            #region ISP20200757 資料交換 - Sunrise
            if (PMSUtilityAutomation.IsSunrise_FinishingProcessesEnable)
            {
                string listOrderID = this.DetailDatas.Select(s => s["OrderID"].ToString()).JoinToString(",");
                Task.Run(() => new Sunrise_FinishingProcesses().SentOrdersToFinishingProcesses(listOrderID, "Orders,Order_QtyShip"))
                                   .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion

            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                string listOrderID = this.DetailDatas.Select(s => s["OrderID"].ToString()).JoinToString(",");
                Task.Run(() => new Gensong_FinishingProcesses().SentOrdersToFinishingProcesses(listOrderID, "Orders,Order_QtyShip"))
                           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion
        }

        private DualResult UpdatePacking_PulloutStatus()
        {
            #region 準備更新字串
            string sqlUpdate = string.Empty;
            string curDBStatus = MyUtility.GetValue.Lookup($"select Status from Pullout with(nolock) where ID='{this.CurrentMaintain["ID"]}' ");
            this.dicUpdatePackinglistA2B = new Dictionary<string, List<string>>();

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                string plFromRgCode = dr["PLFromRgCode"].ToString();
                string updatePackinglistCmd = $@"
Update PackingList
set PulloutStatus = '{curDBStatus}'
where id = '{dr["PackingListID"]}' and pulloutID = '{this.CurrentMaintain["id"].ToString()}';
";
                if (MyUtility.Check.Empty(plFromRgCode))
                {
                    sqlUpdate += updatePackinglistCmd;
                }
                else
                {
                    this.dicUpdatePackinglistA2B.AddSqlCmdByPLFromRgCode(plFromRgCode, updatePackinglistCmd);
                }
            }
            #endregion

            #region 開始更新
            DualResult result;
            if (sqlUpdate.Trim() != string.Empty)
            {
                result = DBProxy.Current.Execute(null, sqlUpdate);
                if (!result)
                {
                    return result;
                }
            }

            // update A2B Packing Pullout Info
            foreach (KeyValuePair<string, List<string>> itemUpdateA2B in this.dicUpdatePackinglistA2B)
            {
                result = PackingA2BWebAPI.ExecuteBySql(itemUpdateA2B.Key, itemUpdateA2B.Value.JoinToString(" "));
                if (!result)
                {
                    return result;
                }
            }
            #endregion

            return new DualResult(true);
        }

        // History
        private void BtnHistory_Click(object sender, EventArgs e)
        {
            // 610: SHIPPING_P06_ReviseHistory_Revised History，出現錯誤訊息
            // Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("Pullout_History", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "Status", reasonType: "Pullout_Delay", caption: "History Pullout Confirm/Unconfirm", haveRemark: true, customerGridFormatTable: "HisType", moduleName: "Pullout");
            Win.UI.ShowHistory callNextForm = new Win.UI.ShowHistory("Pullout_History", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "Status", reasonType: "Pullout_Delay", caption: "History Pullout Confirm/Unconfirm", haveRemark: true, customerGridFormatTable: "HisType", moduleName: "Shipping");
            callNextForm.ShowDialog(this);
        }

        // Revised History
        private void BtnRevisedHistory_Click(object sender, EventArgs e)
        {
            P06_ReviseHistory callNextForm = new P06_ReviseHistory(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        private string updatePackinglist = string.Empty; // 用來先在ReviseData()準備更新Packinglist.pulloutid的SQL, 在save才執行
        private Dictionary<string, List<string>> dicUpdatePackinglistA2B = new Dictionary<string, List<string>>();

        // Revise from ship plan and FOC/LO packing list
        private bool ReviseData()
        {
            DataTable dtNeedCheckA2BPacking;
            DualResult result;
            string sqlGetA2BPAckingByPulloutDate = $@"
select PLFromRgCode, PackingListID 
from GMTBooking_Detail with (nolock) 
where PulloutDate = '{Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("yyyyMMdd")}'";

            result = DBProxy.Current.Select(null, sqlGetA2BPAckingByPulloutDate, out dtNeedCheckA2BPacking);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            this.updatePackinglist = string.Empty;
            this.dicUpdatePackinglistA2B = new Dictionary<string, List<string>>();
            #region 檢查資料是否有還沒做Confirmed的
            string pulloutDate = Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("yyyyMMdd");
            string pulloutID = MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? "XXXXXXXXXX" : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);

            StringBuilder msgString = new StringBuilder();
            string sqlCmd = string.Format(
                @"
select distinct ID from PackingList WITH (NOLOCK) 
where PulloutDate = '{0}' 
and MDivisionID = '{1}'
and Status = 'New' 
and (Type = 'F' or Type = 'L')",
                Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("yyyy/MM/dd"),
                Env.User.Keyword);

            DataTable packlistData;

            result = DBProxy.Current.Select(null, sqlCmd, out packlistData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query packing list fail!\r\n" + result.ToString());
                return false;
            }

            if (packlistData != null && packlistData.Rows.Count > 0)
            {
                msgString.Append("Packing List ID: " + string.Join(",", packlistData.AsEnumerable().Select(row => row["ID"].ToString())));
            }

            sqlCmd = string.Format(
                @"
select distinct p.ID from PackingList p WITH (NOLOCK) where p.PulloutDate = '{0}' and p.MDivisionID = '{1}'and p.ShipPlanID != ''and p.Status = 'New'",
                Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("yyyy/MM/dd"),
                Env.User.Keyword);

            DataTable packDataconfirm;
            result = DBProxy.Current.Select(null, sqlCmd, out packDataconfirm);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query ship plan fail!\r\n" + result.ToString());
                return false;
            }

            if (dtNeedCheckA2BPacking.Rows.Count > 0)
            {
                string sqlCheckPackConfirmA2B = @"
select distinct ID 
from PackingList WITH (NOLOCK) 
where ID in ({0})
and Status = 'New' 
and (Type = 'F' or Type = 'L')
union
select distinct ID 
from PackingList  WITH (NOLOCK) 
where   ID in ({0}) and
        ShipPlanID != '' and 
        Status = 'New'
";
                var listA2BPackingIdByPLFromRgCode = dtNeedCheckA2BPacking.AsEnumerable()
                                                        .GroupBy(s => s["PLFromRgCode"].ToString())
                                                        .Select(s => new
                                                        {
                                                            PLFromRgCode = s.Key,
                                                            WherePackID = s.Select(itemA2B => $"'{itemA2B["PackingListID"]}'").JoinToString(","),
                                                        });

                foreach (var itemCheckA2B in listA2BPackingIdByPLFromRgCode)
                {
                    result = PackingA2BWebAPI.GetDataBySql(
                        itemCheckA2B.PLFromRgCode,
                        string.Format(sqlCheckPackConfirmA2B, itemCheckA2B.WherePackID),
                        out DataTable dtResultA2B);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return false;
                    }

                    dtResultA2B.MergeTo(ref packDataconfirm);
                }
            }

            if (packDataconfirm != null && packDataconfirm.Rows.Count > 0)
            {
                if (msgString.Length > 0)
                {
                    msgString.Append("\r\n");
                }

                msgString.Append("Packing List ID:" + string.Join(", ", packDataconfirm.AsEnumerable().Select(row => row["ID"].ToString())));
            }

            if (msgString.Length > 0)
            {
                MyUtility.Msg.WarningBox(string.Format("Below data not yet confirm!!\r\n{0}", msgString.ToString()));
                return false;
            }

            sqlCmd = string.Format(
                @"
select distinct p.ShipPlanID
from PackingList p WITH (NOLOCK) 
left join ShipPlan s WITH (NOLOCK) on s.ID = p.ShipPlanID
where p.PulloutDate = '{0}' 
and p.MDivisionID = '{1}'
and p.ShipPlanID != ''
and s.Status != 'Confirmed'
union
select  distinct g.ShipPlanID
from    GMTBooking g with (nolock)
inner join  GMTBooking_Detail gd with (nolock) on g.ID = gd.ID
inner join ShipPlan s WITH (NOLOCK) on s.ID = g.ShipPlanID
where   gd.PulloutDate = '{0}'  and
        s.Status != 'Confirmed'
",
                Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("yyyy/MM/dd"),
                Env.User.Keyword);

            DataTable shipPlanData;
            result = DBProxy.Current.Select(null, sqlCmd, out shipPlanData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query ship plan fail!\r\n" + result.ToString());
                return false;
            }

            if (shipPlanData != null && shipPlanData.Rows.Count > 0)
            {
                if (msgString.Length > 0)
                {
                    msgString.Append("\r\n");
                }

                msgString.Append("Ship Plan ID:" + string.Join(", ", shipPlanData.AsEnumerable().Select(row => row["ShipPlanID"].ToString())));
            }

            if (msgString.Length > 0)
            {
                MyUtility.Msg.WarningBox(string.Format("Below data not yet confirm!!\r\n{0}", msgString.ToString()));
                return false;
            }
            #endregion

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                dr["OrderQty"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($"select qty from orders where id = '{dr["orderid"]}' "));
                int ttlshipqty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($@"	
select isnull((select sum(ShipQty) from Pullout_Detail WITH (NOLOCK) where OrderID = '{dr["orderid"]}'),0)
	+isnull ((select sum(DiffQty) 
	from InvAdjust_Qty iq WITH (NOLOCK) 
	inner join InvAdjust i WITH (NOLOCK) on iq.ID = i.id 
	where i.orderid = '{dr["orderid"]}'), 0)"));
                dr["Variance"] = MyUtility.Convert.GetInt(dr["OrderQty"]) - ttlshipqty;
            }

            // 先取A2B資料
            string sqlGetA2BPackID = $@"
select  gd.PLFromRgCode,
        gd.PackingListID
from    GMTBooking g with (nolock)
inner join  GMTBooking_Detail gd with (nolock) on g.ID = gd.ID
inner join ShipPlan s WITH (NOLOCK) on s.ID = g.ShipPlanID
where   gd.PulloutDate = '{pulloutDate}'  and
        s.Status = 'Confirmed'
";
            DataTable dtPackIdBaseA2B;
            result = DBProxy.Current.Select(null, sqlGetA2BPackID, out dtPackIdBaseA2B);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            // 撈所有符合條件的Packing List資料
            #region 組SQL

            DataTable dtPulloutA2B = new DataTable();
            if (dtPackIdBaseA2B.Rows.Count > 0)
            {
                string sqlGetPulloutA2B = @"
select  pd.ID as PackingListID
            , p.Type
            , p.ShipModeID
            , pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.SizeCode
            , o.Qty as OrderQty
            , oq.Qty as OrderShipQty
            , oqd.Qty as SeqQty
            , sum(pd.ShipQty) as Shipqty
            , p.INVNo
            , o.StyleID
            , o.BrandID
            , o.Dest
    from PackingList p WITH (NOLOCK) 
    left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
                                                and oq.Seq = pd.OrderShipmodeSeq
    left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pd.OrderID 
                                                        and oqd.Seq = pd.OrderShipmodeSeq 
                                                        and oqd.Article = pd.Article 
                                                        and oqd.SizeCode = pd.SizeCode
    where (p.Type = 'B' or p.Type = 'S')
          and p.ID in ({0})
          and o.junk = 0
          and (  p.PulloutID = '' or p.PulloutID = '{1}') -- 20161220 willy 避免如果原本有資料,之後修改資料會清空shipQty問題
    group by pd.ID, p.Type, p.ShipModeID, pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.SizeCode, o.Qty
          , oq.Qty, oqd.Qty, p.INVNo, o.StyleID, o.BrandID, o.Dest
";

                var listPackIdByPLFromRgCode = dtPackIdBaseA2B.AsEnumerable()
                                                .GroupBy(s => s["PLFromRgCode"].ToString())
                                                .Select(s => new
                                                {
                                                    PLFromRgCode = s.Key,
                                                    WherePackID = s.Select(groupItem => $"'{groupItem["PackingListID"]}'").JoinToString(","),
                                                });

                foreach (var itemA2B in listPackIdByPLFromRgCode)
                {
                    result = PackingA2BWebAPI.GetDataBySql(
                        itemA2B.PLFromRgCode,
                        string.Format(sqlGetPulloutA2B, itemA2B.WherePackID, pulloutID),
                        out DataTable dtResultA2B);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return false;
                    }

                    if (dtPulloutA2B.Rows.Count == 0)
                    {
                        dtPulloutA2B = dtResultA2B;
                    }
                    else
                    {
                        dtResultA2B.MergeTo(ref dtPulloutA2B);
                    }
                }
            }

            string sqlUnionA2B = string.Empty;

            if (dtPulloutA2B.Rows.Count > 0)
            {
                sqlUnionA2B = $@"
union all
select  t.PackingListID
        , t.Type
        , t.ShipModeID
        , t.OrderID
        , t.OrderShipmodeSeq
        , t.Article
        , t.SizeCode
        , t.OrderQty
        , t.OrderShipQty
        , t.SeqQty
        , t.Shipqty
        , t.INVNo
        , t.StyleID
        , t.BrandID
        , t.Dest
from #tmp t
inner join Orders o with (nolock) on o.ID = t.OrderID
where o.MDivisionID = '{Env.User.Keyword}'

";
            }

            sqlCmd = string.Format(
                @"
with ShipPlanData as (
    select  pd.ID as PackingListID
            , p.Type
            , p.ShipModeID
            , pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.SizeCode
            , o.Qty as OrderQty
            , oq.Qty as OrderShipQty
            , oqd.Qty as SeqQty
            , sum(pd.ShipQty) as Shipqty
            , p.INVNo
            , o.StyleID
            , o.BrandID
            , o.Dest
    from PackingList p WITH (NOLOCK) 
    left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    left join ShipPlan s WITH (NOLOCK) on s.ID = p.ShipPlanID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
                                                and oq.Seq = pd.OrderShipmodeSeq
    left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pd.OrderID 
                                                        and oqd.Seq = pd.OrderShipmodeSeq 
                                                        and oqd.Article = pd.Article 
                                                        and oqd.SizeCode = pd.SizeCode
    where (p.Type = 'B' or p.Type = 'S')
          and s.Status = 'Confirmed'
          and p.MDivisionID = '{0}'
          and p.PulloutDate = '{1}'
          and o.junk = 0
          and (  p.PulloutID = '' or p.PulloutID = '{2}') -- 20161220 willy 避免如果原本有資料,之後修改資料會清空shipQty問題
    group by pd.ID, p.Type, p.ShipModeID, pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.SizeCode, o.Qty
          , oq.Qty, oqd.Qty, p.INVNo, o.StyleID, o.BrandID, o.Dest
    {3}
),
FLPacking as (
    select  pd.ID as PackingListID
            , p.Type
            , p.ShipModeID
            , pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.SizeCode
            , o.Qty as OrderQty
            , oq.Qty as OrderShipQty
            , oqd.Qty as SeqQty
            , sum(pd.ShipQty) as Shipqty
            , p.INVNo
            , o.StyleID
            , o.BrandID
            , o.Dest
    from PackingList p WITH (NOLOCK) 
    left join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
                                                and oq.Seq = pd.OrderShipmodeSeq
    left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pd.OrderID 
                                                        and oqd.Seq = pd.OrderShipmodeSeq 
                                                        and oqd.Article = pd.Article 
                                                        and oqd.SizeCode = pd.SizeCode
    where   (p.Type = 'F' or p.Type = 'L')
            and p.Status = 'Confirmed'
            and p.MDivisionID = '{0}'
            and p.PulloutDate = '{1}'
            and o.junk = 0
            and (p.PulloutID = '' or p.PulloutID='{2}')  -- 20170918 aaron 避免如果原本有資料,之後修改資料會清空shipQty問題
    group by pd.ID, p.Type, p.ShipModeID, pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.SizeCode
             , o.Qty, oq.Qty, oqd.Qty, p.INVNo, o.StyleID, o.BrandID, o.Dest 
),
AllPackData as (
    select * 
    from ShipPlanData

    union all
    select * 
    from FLPacking
),
SummaryData as (
    select  PackingListID
            , Type
            , ShipModeID
            , OrderID
            , OrderShipmodeSeq
            , '' as Article
            , '' as SizeCode
            , OrderQty
            , OrderShipQty
            , 0 as SeqQty
            , sum(Shipqty) as Shipqty
            , INVNo
            , StyleID
            , BrandID
            , Dest 
    from AllPackData
    group by PackingListID, Type, ShipModeID, OrderID, OrderShipmodeSeq, OrderQty, OrderShipQty
             , INVNo, StyleID, BrandID, Dest 
)
select  'D' as DataType
        , *
        , 0 as AllShipQty 
from AllPackData

union all
select  'S' as DataType
        , *
        , AllShipQty = (isnull ((select sum(ShipQty) 
                                 from Pullout_Detail WITH (NOLOCK) 
                                 where ID <> '{2}'  
                                       and OrderID = SummaryData.OrderID), 0) 
                        + isnull ((select sum(DiffQty) 
                                   from InvAdjust_Qty iq WITH (NOLOCK) 
                                   inner join InvAdjust i WITH (NOLOCK) on iq.ID = i.id 
                                   inner join SummaryData b on i.OrderID = b.OrderID), 0))  
from SummaryData",
                Env.User.Keyword,
                Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("yyyy/MM/dd"),
                MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? "XXXXXXXXXX" : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                sqlUnionA2B);

            #endregion
            DataTable allPackData;
            if (dtPulloutA2B.Rows.Count > 0)
            {
                result = MyUtility.Tool.ProcessWithDatatable(dtPulloutA2B, null, sqlCmd, out allPackData);
            }
            else
            {
                result = DBProxy.Current.Select(null, sqlCmd, out allPackData);
            }

            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            this.detailgridbs.SuspendBinding();

            #region 檢查現有資料的異動

            // foreach (DataRow dr in DetailDatas)
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                DataRow[] packData = allPackData.Select(string.Format("DataType = 'S' and PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"])));
                string plFromRgCode = dr["PLFromRgCode"].ToString();

                // 存在表身，但資料已被修改，就必須把第3層資料刪除，表身資料的Ship Qty改成0
                if (packData.Length <= 0)
                {
                    // 刪除第3層資料
                    DataTable subDetailData;
                    this.GetSubDetailDatas(dr, out subDetailData);
                    List<DataRow> drArray = new List<DataRow>();
                    foreach (DataRow ddr in subDetailData.Rows)
                    {
                        drArray.Add(ddr);
                    }

                    foreach (DataRow ddr in drArray)
                    {
                        ddr.Delete();
                    }

                    // 清空PackingList.pulloutID, 也一併清空PulloutStatus
                    string updatePackinglistCmd = $@"
Update PackingList 
set pulloutID = '',
PulloutStatus = ''
where id='{dr["PackingListID"]}' and pulloutID = '{this.CurrentMaintain["id"].ToString()}'; ";
                    if (MyUtility.Check.Empty(plFromRgCode))
                    {
                        this.updatePackinglist += updatePackinglistCmd;
                    }
                    else
                    {
                        this.dicUpdatePackinglistA2B.AddSqlCmdByPLFromRgCode(plFromRgCode, updatePackinglistCmd);
                    }

                    #region 合併計算 ShipQty
                    int sumShipQty = 0;
                    if (allPackData.AsEnumerable().Any(row => row["OrderID"].EqualString(dr["OrderID"]) && row["DataType"].EqualString("S")))
                    {
                        sumShipQty = MyUtility.Convert.GetInt(allPackData.AsEnumerable().Where(row => row["OrderID"].EqualString(dr["OrderID"]) && row["DataType"].EqualString("S")).CopyToDataTable().Compute("sum(ShipQty)", null));
                    }

                    string strAllShipQty = string.Format(
                        @"
select AllShipQty = (isnull ((select sum(ShipQty) 
                             from Pullout_Detail WITH (NOLOCK) 
                             where ID <> '{0}' and OrderID = '{1}'), 0) --此orderid,非此單的Pullout_Detail.ShipQty
                          + isnull ((select sum(DiffQty) 
                                from InvAdjust_Qty iq WITH (NOLOCK) 
                                inner join InvAdjust i WITH (NOLOCK) on iq.ID = i.id 
                                where i.OrderID = '{1}'), 0))",
                        MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? "XXXXXXXXXX" : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                        dr["OrderID"]);

                    int allShipQty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(strAllShipQty));
                    int totalShipQty = sumShipQty + allShipQty;
                    #endregion
                    #region 判斷 Status
                    string newStatus = string.Empty;
                    if (dr["Status"].ToString().ToUpper() == "SHORTAGE" || dr["Status"].ToString().ToUpper() == "S")
                    {
                        newStatus = "S";
                    }
                    else
                    {
                        int orderQty = MyUtility.Convert.GetInt(dr["OrderQty"]);
                        newStatus = totalShipQty == orderQty ? "C" : totalShipQty > orderQty ? "E" : "P";
                    }
                    #endregion

                    dr["ShipQty"] = 0;
                    dr["Status"] = newStatus;
                    dr["StatusExp"] = this.GetStatusName(newStatus);
                    dr["PackingListID"] = string.Empty;
                    dr["INVNo"] = string.Empty;
                    dr["ReviseDate"] = DateTime.Now;
                }
                else
                {
                    #region 判斷此筆PackingListID→packinglist.pulloutid是否和此單一樣
                    string chkpulloutid = string.Format(@"select pulloutid from packinglist where id = '{0}'", dr["PackingListID"]);
                    string pulloutid = string.Empty;
                    if (MyUtility.Check.Empty(plFromRgCode))
                    {
                        pulloutid = MyUtility.GetValue.Lookup(chkpulloutid);
                    }
                    else
                    {
                        DataRow drResultA2B;
                        PackingA2BWebAPI.PackingA2BResult packingA2BResult = PackingA2BWebAPI.SeekBySql(plFromRgCode, chkpulloutid, out drResultA2B);
                        if (!packingA2BResult)
                        {
                            return packingA2BResult;
                        }

                        if (packingA2BResult.isDataExists)
                        {
                            pulloutid = drResultA2B["pulloutid"].ToString();
                        }
                    }

                    if (pulloutid == string.Empty || pulloutid == this.CurrentMaintain["id"].ToString())
                    {
                        string updatePackinglistCmd = $@"
Update PackingList 
set pulloutID = '{this.CurrentMaintain["ID"]}' ,
PulloutStatus = '{this.CurrentMaintain["Status"]}'
where id='{dr["PackingListID"]}'; ";
                        if (MyUtility.Check.Empty(plFromRgCode))
                        {
                            this.updatePackinglist += updatePackinglistCmd;
                        }
                        else
                        {
                            this.dicUpdatePackinglistA2B.AddSqlCmdByPLFromRgCode(plFromRgCode, updatePackinglistCmd);
                        }
                    }
                    else
                    {
                        dr["PackingListID"] = string.Empty;
                        dr["INVNo"] = string.Empty;
                        dr["ShipQty"] = 0;

                        // 刪除第3層資料
                        DataTable subDetailData2;
                        this.GetSubDetailDatas(dr, out subDetailData2);
                        List<DataRow> drArray2 = new List<DataRow>();
                        foreach (DataRow ddr in subDetailData2.Rows)
                        {
                            drArray2.Add(ddr);
                        }

                        foreach (DataRow ddr in drArray2)
                        {
                            ddr.Delete();
                        }

                        // PackingListIDu,也要一併清空PackingList.pulloutID + PulloutStatus
                        string updatePackinglistCmd = $@"
Update PackingList 
set pulloutID = ''
,PulloutStatus = ''
where id='{dr["PackingListID"]}' and pulloutID = '{this.CurrentMaintain["id"]}'; ";
                        if (MyUtility.Check.Empty(plFromRgCode))
                        {
                            this.updatePackinglist += updatePackinglistCmd;
                        }
                        else
                        {
                            this.dicUpdatePackinglistA2B.AddSqlCmdByPLFromRgCode(plFromRgCode, updatePackinglistCmd);
                        }
                    }
                    #endregion
                    #region 合併計算 ShipQty
                    int sumShipQty = MyUtility.Convert.GetInt(allPackData.AsEnumerable().Where(row => row["OrderID"].EqualString(dr["OrderID"]) && row["DataType"].EqualString("S")).CopyToDataTable().Compute("sum(ShipQty)", null));
                    int totalShipQty = sumShipQty + MyUtility.Convert.GetInt(packData[0]["AllShipQty"]);
                    #endregion
                    #region 判斷 Status
                    string newStatus = string.Empty;
                    if (dr["Status"].ToString().ToUpper() == "SHORTAGE" || dr["Status"].ToString().ToUpper() == "S")
                    {
                        newStatus = "S";
                    }
                    else
                    {
                        int orderQty = MyUtility.Convert.GetInt(packData[0]["OrderQty"]);
                        newStatus = totalShipQty == orderQty ? "C" : totalShipQty > orderQty ? "E" : "P";
                    }
                    #endregion

                    // shipQty,OrderQty,InvNo 修改過才會更換資料
                    if (MyUtility.Convert.GetString(dr["ShipmodeID"]).EqualString(MyUtility.Convert.GetString(packData[0]["ShipmodeID"])) == false
                        || MyUtility.Convert.GetInt(dr["ShipQty"]) != MyUtility.Convert.GetInt(packData[0]["ShipQty"])
                        || MyUtility.Convert.GetInt(dr["OrderQty"]) != MyUtility.Convert.GetInt(packData[0]["OrderQty"])
                        || MyUtility.Convert.GetString(dr["INVNo"]) != MyUtility.Convert.GetString(packData[0]["INVNo"])
                        || MyUtility.Convert.GetString(dr["ShipModeSeqQty"]) != MyUtility.Convert.GetString(packData[0]["OrderShipQty"]))
                    {
                        dr["ShipQty"] = packData[0]["ShipQty"];
                        dr["OrderQty"] = packData[0]["OrderQty"];
                        dr["ShipModeSeqQty"] = packData[0]["OrderShipQty"];
                        dr["Status"] = newStatus;
                        dr["StatusExp"] = this.GetStatusName(newStatus);
                        dr["INVNo"] = packData[0]["INVNo"];
                        dr["ShipmodeID"] = packData[0]["ShipmodeID"];
                        dr["ReviseDate"] = DateTime.Now;
                        dr["Variance"] = MyUtility.Convert.GetInt(packData[0]["OrderQty"]) - totalShipQty;
                    }

                    // 不管資料有無修改,都會重新更新status資料
                    dr["Status"] = newStatus;
                    dr["StatusExp"] = this.GetStatusName(newStatus);

                    // 取出第3層資料，比對是否有異動
                    DataTable subDetailData;
                    this.GetSubDetailDatas(dr, out subDetailData);
                    List<DataRow> drArray = new List<DataRow>();
                    foreach (DataRow ddr in subDetailData.Rows)
                    {
                        drArray.Add(ddr);
                    }

                    foreach (DataRow ddr in drArray)
                    {
                        DataRow[] pulloutSubDetail = allPackData.Select(string.Format("DataType = 'D' and PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}' and Article = '{3}' and SizeCode = '{4}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"]), MyUtility.Convert.GetString(ddr["Article"]), MyUtility.Convert.GetString(ddr["SizeCode"])));
                        if (pulloutSubDetail.Length <= 0)
                        {
                            if (dr.RowState == DataRowState.Unchanged)
                            {
                                dr["StatusExp"] = this.GetStatusName(newStatus);
                            }

                            ddr.Delete();
                            dr["ReviseDate"] = DateTime.Now;
                        }
                        else
                        {
                            if (MyUtility.Convert.GetInt(ddr["ShipQty"]) != MyUtility.Convert.GetInt(pulloutSubDetail[0]["Shipqty"]))
                            {
                                if (dr.RowState == DataRowState.Unchanged)
                                {
                                    dr["StatusExp"] = this.GetStatusName(newStatus);
                                }

                                ddr["ShipQty"] = pulloutSubDetail[0]["Shipqty"];
                                dr["ReviseDate"] = DateTime.Now;
                            }
                        }
                    }

                    #region 檢查有被撈出來但不存在Pullout_Detail_Detail中
                    DataRow[] newPackData = allPackData.Select(string.Format("DataType = 'D' and PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"])));
                    if (newPackData.Length > 0)
                    {
                        this.GetSubDetailDatas(dr, out subDetailData);

                        foreach (DataRow ddr in newPackData)
                        {
                            // DataRow[] CurrentPulloutData = SubDetailData.Select(string.Format("ID = '{0}' and Pullout_DetailUkey = '{1}' and OrderID = '{2}' and Article ='{3}'", MyUtility.Convert.GetString(ddr["PackingListID"]), MyUtility.Convert.GetString(ddr["OrderID"]), MyUtility.Convert.GetString(ddr["OrderShipmodeSeq"])));
                            DataRow[] p_Detail_detail = subDetailData.Select(string.Format(
                                "ID = '{0}' and OrderID = '{1}' and Article = '{2}' and SizeCode = '{3}' ",
                                this.CurrentMaintain["ID"],
                                MyUtility.Convert.GetString(ddr["OrderID"]),
                                MyUtility.Convert.GetString(ddr["Article"]),
                                MyUtility.Convert.GetString(ddr["SizeCode"])));

                            if (p_Detail_detail.Length == 0 || MyUtility.Convert.GetInt(p_Detail_detail[0]["ShipQty"]) != MyUtility.Convert.GetInt(ddr["ShipQty"]))
                            {
                                if (dr.RowState == DataRowState.Unchanged)
                                {
                                    dr["StatusExp"] = this.GetStatusName(newStatus);
                                }

                                #region 新增一筆資料到Pullout_Detail_Detail
                                DataRow ndr = subDetailData.NewRow();
                                ndr["ID"] = this.CurrentMaintain["ID"];
                                ndr["Pullout_DetailUKey"] = dr["UKey"];
                                ndr["OrderID"] = dr["OrderID"];
                                ndr["Article"] = ddr["Article"];
                                ndr["SizeCode"] = ddr["SizeCode"];
                                ndr["ShipQty"] = ddr["ShipQty"];
                                ndr["Qty"] = ddr["SeqQty"];
                                ndr["Variance"] = MyUtility.Convert.GetInt(ddr["SeqQty"]) - MyUtility.Convert.GetInt(ddr["ShipQty"]);
                                subDetailData.Rows.Add(ndr);
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            #endregion

            // 若在P10 修改了PulloutDate，Pullout必須跟著更新，因此不能限定PulloutID，為避免出意外，不改在allPackData的SQL，另外開一個查詢
            #region 組SQL
            DataTable dtPulloutA2BForPullOut = new DataTable();
            if (dtPackIdBaseA2B.Rows.Count > 0)
            {
                string sqlGetPulloutA2B = @"
select  pd.ID as PackingListID
            , p.Type
            , p.ShipModeID
            , pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.SizeCode
            , o.Qty as OrderQty
            , oq.Qty as OrderShipQty
            , oqd.Qty as SeqQty
            , sum(pd.ShipQty) as Shipqty
            , p.INVNo
            , o.StyleID
            , o.BrandID
            , o.Dest
            , [PLFromRgCode] = '{1}'
    from PackingList p WITH (NOLOCK) 
    left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
                                                and oq.Seq = pd.OrderShipmodeSeq
    left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pd.OrderID 
                                                        and oqd.Seq = pd.OrderShipmodeSeq 
                                                        and oqd.Article = pd.Article 
                                                        and oqd.SizeCode = pd.SizeCode
    where (p.Type = 'B' or p.Type = 'S')
          and o.junk = 0
          and p.ID in ({0})
          and (p.PulloutID = '' or p.PulloutID='{2}')
    group by pd.ID, p.Type, p.ShipModeID, pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.SizeCode, o.Qty
          , oq.Qty, oqd.Qty, p.INVNo, o.StyleID, o.BrandID, o.Dest
";

                var listPackIdByPLFromRgCode = dtPackIdBaseA2B.AsEnumerable()
                                                .GroupBy(s => s["PLFromRgCode"].ToString())
                                                .Select(s => new
                                                {
                                                    PLFromRgCode = s.Key,
                                                    WherePackID = s.Select(groupItem => $"'{groupItem["PackingListID"]}'").JoinToString(","),
                                                });

                foreach (var itemA2B in listPackIdByPLFromRgCode)
                {
                    result = PackingA2BWebAPI.GetDataBySql(
                        itemA2B.PLFromRgCode,
                        string.Format(sqlGetPulloutA2B, itemA2B.WherePackID, itemA2B.PLFromRgCode, pulloutID),
                        out DataTable dtResultA2B);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return false;
                    }

                    if (dtPulloutA2BForPullOut.Rows.Count == 0)
                    {
                        dtPulloutA2BForPullOut = dtResultA2B;
                    }
                    else
                    {
                        dtResultA2B.MergeTo(ref dtPulloutA2BForPullOut);
                    }
                }
            }

            string sqlUnionA2BForPullOut = string.Empty;

            if (dtPulloutA2BForPullOut.Rows.Count > 0)
            {
                sqlUnionA2BForPullOut = $@"
union all
select  t.PackingListID
        , t.Type
        , t.ShipModeID
        , t.OrderID
        , t.OrderShipmodeSeq
        , t.Article
        , t.SizeCode
        , t.OrderQty
        , t.OrderShipQty
        , t.SeqQty
        , t.Shipqty
        , t.INVNo
        , t.StyleID
        , t.BrandID
        , t.Dest
        , t.PLFromRgCode
from #tmp t
inner join Orders o with (nolock) on o.ID = t.OrderID
where o.MDivisionID = '{Env.User.Keyword}'
";
            }

            string sqlCmd_2 = string.Format(
                @"
with ShipPlanData as (
    select  pd.ID as PackingListID
            , p.Type
            , p.ShipModeID
            , pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.SizeCode
            , o.Qty as OrderQty
            , oq.Qty as OrderShipQty
            , oqd.Qty as SeqQty
            , sum(pd.ShipQty) as Shipqty
            , p.INVNo
            , o.StyleID
            , o.BrandID
            , o.Dest
            , [PLFromRgCode] = ''
    from PackingList p WITH (NOLOCK) 
    left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    left join ShipPlan s WITH (NOLOCK) on s.ID = p.ShipPlanID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
                                                and oq.Seq = pd.OrderShipmodeSeq
    left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pd.OrderID 
                                                        and oqd.Seq = pd.OrderShipmodeSeq 
                                                        and oqd.Article = pd.Article 
                                                        and oqd.SizeCode = pd.SizeCode
    where (p.Type = 'B' or p.Type = 'S')
          and s.Status = 'Confirmed'
          and o.junk = 0
          and p.MDivisionID = '{0}'
          and p.PulloutDate = '{1}'
    group by pd.ID, p.Type, p.ShipModeID, pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.SizeCode, o.Qty
          , oq.Qty, oqd.Qty, p.INVNo, o.StyleID, o.BrandID, o.Dest
{3}
),
FLPacking as (
    select  pd.ID as PackingListID
            , p.Type
            , p.ShipModeID
            , pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.SizeCode
            , o.Qty as OrderQty
            , oq.Qty as OrderShipQty
            , oqd.Qty as SeqQty
            , sum(pd.ShipQty) as Shipqty
            , p.INVNo
            , o.StyleID
            , o.BrandID
            , o.Dest
            , [PLFromRgCode] = ''
    from PackingList p WITH (NOLOCK) 
    left join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
                                                and oq.Seq = pd.OrderShipmodeSeq
    left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pd.OrderID 
                                                        and oqd.Seq = pd.OrderShipmodeSeq 
                                                        and oqd.Article = pd.Article 
                                                        and oqd.SizeCode = pd.SizeCode
    where   (p.Type = 'F' or p.Type = 'L')
            and p.Status = 'Confirmed'
            and o.junk = 0
            and p.MDivisionID = '{0}'
            and p.PulloutDate = '{1}'
    group by pd.ID, p.Type, p.ShipModeID, pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.SizeCode
             , o.Qty, oq.Qty, oqd.Qty, p.INVNo, o.StyleID, o.BrandID, o.Dest 
),
AllPackData as (
    select * 
    from ShipPlanData

    union all
    select * 
    from FLPacking
),
SummaryData as (
    select  PackingListID
            , Type
            , ShipModeID
            , OrderID
            , OrderShipmodeSeq
            , '' as Article
            , '' as SizeCode
            , OrderQty
            , OrderShipQty
            , 0 as SeqQty
            , sum(Shipqty) as Shipqty
            , INVNo
            , StyleID
            , BrandID
            , Dest 
            , PLFromRgCode
    from AllPackData
    group by PackingListID, Type, ShipModeID, OrderID, OrderShipmodeSeq, OrderQty, OrderShipQty
             , INVNo, StyleID, BrandID, Dest, PLFromRgCode
)
select  'D' as DataType
        , *
        , 0 as AllShipQty 
from AllPackData

union all
select  'S' as DataType
        , *
        , AllShipQty = (isnull ((select sum(ShipQty) 
                                 from Pullout_Detail WITH (NOLOCK) 
                                 where ID <> '{2}'  
                                       and OrderID = SummaryData.OrderID), 0) 
                        + isnull ((select sum(DiffQty) 
                                   from InvAdjust_Qty iq WITH (NOLOCK) 
                                   inner join InvAdjust i WITH (NOLOCK) on iq.ID = i.id 
                                   inner join SummaryData b on i.OrderID = b.OrderID), 0))  
from SummaryData",
                Env.User.Keyword,
                Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("yyyy/MM/dd"),
                MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? "XXXXXXXXXX" : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                sqlUnionA2BForPullOut);

            #endregion

            DataTable allPackData_ForPullOut;
            if (dtPulloutA2BForPullOut.Rows.Count > 0)
            {
                result = MyUtility.Tool.ProcessWithDatatable(dtPulloutA2BForPullOut, null, sqlCmd_2, out allPackData_ForPullOut);
            }
            else
            {
                result = DBProxy.Current.Select(null, sqlCmd_2, out allPackData_ForPullOut);
            }

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            #region 新增資料
            DataRow[] allSumPackData = allPackData_ForPullOut.Select("DataType = 'S'");
            if (allSumPackData.Length > 0)
            {
                foreach (DataRow dr in allSumPackData)
                {
                    string plFromRgCode = dr["PLFromRgCode"].ToString();
                    DataRow[] pullDetail = null;
                    if (this.detailgridbs != null && ((DataTable)this.detailgridbs.DataSource).Rows.Count > 0)
                    {
                        DataTable dt = (DataTable)this.detailgridbs.DataSource;
                        pullDetail = dt.Select(string.Format("PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"])));
                    }

                    if (pullDetail == null || pullDetail.Length <= 0)
                    {
                        #region 合併計算 ShipQty
                        int sumShipQty = MyUtility.Convert.GetInt(allPackData_ForPullOut.AsEnumerable().Where(row => row["OrderID"].EqualString(dr["OrderID"]) && row["DataType"].EqualString("S")).CopyToDataTable().Compute("sum(ShipQty)", null));
                        #endregion
                        int totalShipQty = sumShipQty + MyUtility.Convert.GetInt(dr["AllShipQty"]);
                        string newStatus = totalShipQty == MyUtility.Convert.GetInt(dr["OrderQty"]) ? "C" : totalShipQty > MyUtility.Convert.GetInt(dr["OrderQty"]) ? "E" : "P";

                        DataRow detailNewRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
                        detailNewRow["ID"] = this.CurrentMaintain["ID"];
                        detailNewRow["PulloutDate"] = this.CurrentMaintain["PulloutDate"];
                        detailNewRow["OrderID"] = dr["OrderID"];
                        detailNewRow["OrderShipmodeSeq"] = dr["OrderShipmodeSeq"];
                        detailNewRow["ShipQty"] = dr["ShipQty"];
                        detailNewRow["OrderQty"] = dr["OrderQty"];
                        detailNewRow["ShipModeSeqQty"] = dr["OrderShipQty"];
                        detailNewRow["Status"] = newStatus;
                        detailNewRow["StatusExp"] = this.GetStatusName(newStatus);
                        detailNewRow["PackingListID"] = dr["PackingListID"];
                        detailNewRow["PackingListType"] = dr["Type"];
                        detailNewRow["INVNo"] = dr["INVNo"];
                        detailNewRow["ShipmodeID"] = dr["ShipmodeID"];
                        detailNewRow["StyleID"] = dr["StyleID"];
                        detailNewRow["BrandID"] = dr["BrandID"];
                        detailNewRow["Dest"] = dr["Dest"];
                        detailNewRow["Variance"] = MyUtility.Convert.GetInt(dr["OrderQty"]) - totalShipQty;
                        detailNewRow["PLFromRgCode"] = dr["PLFromRgCode"];
                        ((DataTable)this.detailgridbs.DataSource).Rows.Add(detailNewRow);

                        #region update PulloutID 到PackingList
                        string updatePackinglistCmd = $@"
Update PackingList 
set pulloutID = '{this.CurrentMaintain["ID"]}',
PulloutStatus = '{this.CurrentMaintain["Status"]}' 
where id='{dr["PackingListID"]}'; ";
                        if (MyUtility.Check.Empty(plFromRgCode))
                        {
                            this.updatePackinglist += updatePackinglistCmd;
                        }
                        else
                        {
                            this.dicUpdatePackinglistA2B.AddSqlCmdByPLFromRgCode(plFromRgCode, updatePackinglistCmd);
                        }
                        #endregion

                        #region 新增資料到Pullout_Detail_Detail
                        DataRow[] allSubDetail = allPackData_ForPullOut.Select(string.Format("DataType = 'D' and PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"])));
                        if (allSubDetail.Length > 0)
                        {
                            foreach (DataRow ddr in allSubDetail)
                            {
                                DataTable subDetailData;
                                this.GetSubDetailDatas(detailNewRow, out subDetailData);
                                DataRow ndr = subDetailData.NewRow();
                                ndr["ID"] = this.CurrentMaintain["ID"];
                                ndr["OrderID"] = ddr["OrderID"];
                                ndr["Article"] = ddr["Article"];
                                ndr["SizeCode"] = ddr["SizeCode"];
                                ndr["ShipQty"] = ddr["ShipQty"];
                                ndr["Qty"] = ddr["SeqQty"];
                                ndr["Variance"] = MyUtility.Convert.GetInt(ddr["SeqQty"]) - MyUtility.Convert.GetInt(ddr["ShipQty"]);
                                subDetailData.Rows.Add(ndr);
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            this.detailgridbs.ResumeBinding(); // detailgridbs.SuspendBinding();之後要做
            this.detailgrid.ValidateControl();

            // this.status_Change();
            return true;
        }

        // Revise from ship plan and FOC/LO packing list
        private void BtnRevise_Click(object sender, EventArgs e)
        {
            // 先Load第3層資料
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                DataTable subDetailData;
                this.GetSubDetailDatas(dr, out subDetailData);
            }

            if (this.ReviseData())
            {
                MyUtility.Msg.InfoBox("Revise completed!");

                this.detailgrid.ValidateControl();
            }
        }

        private string GetStatusName(string status)
        {
            return status == "P" ? "Partial" : status == "C" ? "Complete" : status == "E" ? "Exceed" : status == "S" ? "Shortage" : string.Empty;
        }

        private DualResult WriteRevise(string status, DataRow dr)
        {
            DataRow reviseRow = this.PulloutReviseData.NewRow();
            DualResult result = this.WritePulloutRevise(dr, reviseRow, status);
            if (!result)
            {
                return result;
            }

            // to Detail
            result = this.WritePulloutReviseDetail(dr, MyUtility.Convert.GetString(reviseRow["ReviseKey"]));
            if (!result)
            {
                return result;
            }

            return Ict.Result.True;
        }

        private DualResult WritePulloutRevise(DataRow dr, DataRow reviseRow, string type)
        {
            reviseRow["ID"] = dr["ID"];
            reviseRow["Type"] = type == "Delete" ? "D" : type == "Revise" ? "R" : "M";
            reviseRow["OrderID"] = dr["OrderID"];
            reviseRow["OldShipQty"] = type == "Missing" ? 0 : dr["ShipQty", DataRowVersion.Original];
            reviseRow["NewShipQty"] = type == "Delete" ? 0 : dr["ShipQty"];
            reviseRow["OldStatus"] = type == "Missing" ? string.Empty : dr["Status", DataRowVersion.Original];
            reviseRow["NewStatus"] = dr["Status"];
            reviseRow["PackingListID"] = dr["PackingListID"];
            reviseRow["Remark"] = dr["Remark"];
            reviseRow["Pullout_DetailUKey"] = dr["UKey"]; // Pullout_Revise沒有ukey
            reviseRow["INVNo"] = dr["INVNo"];
            reviseRow["OldShipModeID"] = type == "Missing" ? string.Empty : dr["ShipModeID", DataRowVersion.Original];
            reviseRow["ShipModeID"] = dr["ShipModeID"];
            reviseRow["AddName"] = Env.User.UserID;
            reviseRow["AddDate"] = DateTime.Now;

            return DBProxy.Current.Insert(null, this.revisedTS, reviseRow);
        }

        private DualResult WritePulloutReviseDetail(DataRow dr, string identityValue)
        {
            DataTable subDetailData;
            this.GetSubDetailDatas(dr, out subDetailData);
            foreach (DataRow ddr in subDetailData.Rows)
            {
                string type = ddr.RowState == DataRowState.Added ? "Missing" : ddr.RowState == DataRowState.Deleted ? "Delete" : string.Empty;
                DataRow reviseDetailRow = this.PulloutReviseDetailData.NewRow();
                reviseDetailRow["ID"] = type == "Delete" ? ddr["ID", DataRowVersion.Original] : ddr["ID"];
                reviseDetailRow["Pullout_DetailUKey"] = type == "Delete" ? ddr["Pullout_DetailUKey", DataRowVersion.Original] : ddr["Pullout_DetailUKey"];
                reviseDetailRow["Pullout_ReviseReviseKey"] = identityValue;
                reviseDetailRow["OrderID"] = type == "Delete" ? ddr["OrderID", DataRowVersion.Original] : ddr["OrderID"];
                reviseDetailRow["Article"] = type == "Delete" ? ddr["Article", DataRowVersion.Original] : ddr["Article"];
                reviseDetailRow["SizeCode"] = type == "Delete" ? ddr["SizeCode", DataRowVersion.Original] : ddr["SizeCode"];
                reviseDetailRow["OldShipQty"] = type == "Missing" ? 0 : ddr["ShipQty", DataRowVersion.Original];
                reviseDetailRow["NewShipQty"] = type == "Delete" ? 0 : ddr["ShipQty"];
                DualResult result = DBProxy.Current.Insert(null, this.revised_detailTS, reviseDetailRow);
                if (!result)
                {
                    return result;
                }
            }

            return Ict.Result.True;
        }

        // 檢查表身的ShipMode與表頭的ShipMode要相同 & ShipModeID 不存在Order_QtyShip 就return
        private bool CheckShipMode()
        {
            StringBuilder msg = new StringBuilder();

            var dtShipMode = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(s => s.RowState != DataRowState.Deleted);
            if (dtShipMode == null || dtShipMode.Count() == 0)
            {
                return true;
            }

            DualResult result;
            DataTable dtCheckResult;
            string strSql;
            DataRow drPackingShipModeCheckResult;
            foreach (DataRow dr in dtShipMode)
            {
                string plFromRgCode = dr["PLFromRgCode"].ToString();
                #region 檢查Packing List 的ship mode
                strSql = $"select ShipModeID from PackingList with (nolock) where ID = '{dr["PackingListID"]}' and ShipModeID <> '{dr["ShipModeID"]}'";
                bool isPackListShipModeInconsistent = false;

                if (MyUtility.Check.Empty(plFromRgCode))
                {
                    isPackListShipModeInconsistent = MyUtility.Check.Seek(strSql, out drPackingShipModeCheckResult);
                }
                else
                {
                    PackingA2BWebAPI.PackingA2BResult resultA2B = PackingA2BWebAPI.SeekBySql(plFromRgCode, strSql, out drPackingShipModeCheckResult);
                    if (!resultA2B)
                    {
                        this.ShowErr(resultA2B);
                        return false;
                    }

                    isPackListShipModeInconsistent = resultA2B.isDataExists;
                }

                if (isPackListShipModeInconsistent)
                {
                    msg.Append(string.Format("Packing#:{0},   Shipping Mode:{1}\r\n", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(drPackingShipModeCheckResult["ShipModeID"])));
                    continue;
                }
                #endregion

                #region 檢查Order_QtyShip 的ship mode
                strSql = $@"
select distinct oq.ID,oq.Seq,oq.ShipmodeID
from PackingList p  with (nolock)
inner join PackingList_Detail pd with (nolock) on p.ID=pd.ID
inner join Order_QtyShip oq with (nolock) on oq.id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
inner join Orders o with (nolock) on oq.ID = o.ID
where p.id='{dr["PackingListID"]}' and p.ShipModeID  <> oq.ShipmodeID and o.Category <> 'S'
";

                if (MyUtility.Check.Empty(plFromRgCode))
                {
                    result = DBProxy.Current.Select(null, strSql, out dtCheckResult);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return result;
                    }
                }
                else
                {
                    result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, strSql, out dtCheckResult);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return result;
                    }
                }

                if (dtCheckResult.Rows.Count > 0)
                {
                    foreach (DataRow drError in dtCheckResult.Rows)
                    {
                        msg.Append($"Order ID : {drError["ID"]},   Seq : {drError["Seq"]},   Shipping Mode : {drError["ShipmodeID"]}\r\n");
                    }
                }
                #endregion
            }

            if (msg.Length > 0)
            {
                MyUtility.Msg.WarningBox("Shipping mode is inconsistent!!\r\n" + msg.ToString());
                return false;
            }

            return true;
        }

        private void CheckIDD()
        {
            if (this.DetailDatas.Count == 0)
            {
                return;
            }
            #region 檢查傳入的SP 維護的IDD是否都為同一天(沒維護度不判斷)
            List<Order_QtyShipKey> listOrder_QtyShipKey = this.DetailDatas
                .Where(s => !MyUtility.Check.Empty(s["ShipQty"]))
                .Select(s => new Order_QtyShipKey
                {
                    SP = s["OrderID"].ToString(),
                    Seq = s["OrderShipmodeSeq"].ToString(),
                }).ToList();

            Prgs.CheckIDDSame(listOrder_QtyShipKey);
            #endregion
        }

        private void CheckPulloutputIDD()
        {
            if (this.DetailDatas.Count == 0)
            {
                return;
            }

            #region 檢查傳入的SP 維護的IDD與PulloutputDate是否都為同一天(沒維護不判斷)
            List<Order_QtyShipKey> listOrder_QtyShipKey = this.DetailDatas
                .Where(s => !MyUtility.Check.Empty(s["ShipQty"]))
                .Select(s => new Order_QtyShipKey
                {
                    SP = s["OrderID"].ToString(),
                    Seq = s["OrderShipmodeSeq"].ToString(),
                    PulloutDate = MyUtility.Convert.GetDate(s["PulloutDate"]),
                }).ToList();

            Prgs.CheckIDDSamePulloutDate(listOrder_QtyShipKey);
            #endregion

        }
    }
}