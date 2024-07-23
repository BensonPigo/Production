using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.CallPmsAPI;
using Sci.Production.Class.Command;
using Sci.Production.PublicPrg;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P10_UpdatePulloutDate
    /// </summary>
    public partial class P10_UpdatePulloutDate : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_pulldate;
        private DataRow masterDate;

        /// <summary>
        /// P10_UpdatePulloutDate
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P10_UpdatePulloutDate(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterDate = masterData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridUpdatePulloutDate.IsEditingReadOnly = false;
            this.gridUpdatePulloutDate.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridUpdatePulloutDate)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("INVNo", header: "GB#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("PulloutDate", header: "Pullout Date").Get(out this.col_pulldate)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Text("IDD", header: "Intended Delivery", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Status", header: "Packing Status", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Numeric("CTNQty", header: "CTN Qty")
                .Numeric("ClogQty", header: "CTN Qty at C-Log")
                .Date("InspDate", header: "est. Inspection Date", iseditingreadonly: true)
                .Text("InspStatus", header: "Inspection Status", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.gridUpdatePulloutDate.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridUpdatePulloutDate.GetDataRow<DataRow>(e.RowIndex);

                    // 輸入的Pullout date或原本的Pullout date的Pullout Report如果已經Confirmed的話，就不可以被修改
                    if (this.gridUpdatePulloutDate.Columns[e.ColumnIndex].DataPropertyName == this.col_pulldate.DataPropertyName)
                    {
                        if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["PulloutDate"]))
                        {
                            if (!MyUtility.Check.Empty(dr["PulloutDate"]) && this.CheckPullout((DateTime)MyUtility.Convert.GetDate(dr["PulloutDate"]), MyUtility.Convert.GetString(dr["MDivisionID"])))
                            {
                                this.PulloutMsg(dr, (DateTime)MyUtility.Convert.GetDate(dr["PulloutDate"]));
                                e.Cancel = true;
                                dr.EndEdit();
                                return;
                            }

                            if (!MyUtility.Check.Empty(e.FormattedValue) && this.CheckPullout((DateTime)MyUtility.Convert.GetDate(e.FormattedValue), MyUtility.Convert.GetString(dr["MDivisionID"])))
                            {
                                this.PulloutMsg(dr, (DateTime)MyUtility.Convert.GetDate(e.FormattedValue));
                                e.Cancel = true;
                                dr.EndEdit();
                                return;
                            }
                        }
                    }
                }
            };

            string sqlCmd = @"select 
0 as Selected,
p.INVNo,
p.ID,
iif(p.OrderID='',(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.id) a for xml path('')),p.OrderID) as OrderID,
iif(p.type = 'B',(select BuyerDelivery from Order_QtyShip WITH (NOLOCK) where ID = p.OrderID and Seq = p.OrderShipmodeSeq),(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.ID) a, Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq)) as BuyerDelivery,
p.PulloutDate,
p.Status,
p.CTNQty,
p.InspDate,
p.InspStatus,
(select isnull(sum(CTNQty),0) from PackingList_Detail pd where pd.ID = p.ID and pd.ReceiveDate is not null) as ClogQty,
p.MDivisionID,
[IDD] = STUFF ((select distinct CONCAT (',', Format(oqs.IDD, 'yyyy/MM/dd')) 
                            from PackingList_Detail pd WITH (NOLOCK) 
                            inner join Order_QtyShip oqs with (nolock) on oqs.ID = pd.OrderID and oqs.Seq = pd.OrderShipmodeSeq
                            where pd.ID = p.id and oqs.IDD is not null
                            for xml path('')
                          ), 1, 1, ''),
[OrderShipmodeSeq] = STUFF ((select CONCAT (',', cast (a.OrderShipmodeSeq as nvarchar)) 
                                from (
                                    select pd.OrderShipmodeSeq 
                                    from PackingList_Detail pd WITH (NOLOCK) 
                                    left join AirPP ap With (NoLock) on pd.OrderID = ap.OrderID
                                                                        and pd.OrderShipmodeSeq = ap.OrderShipmodeSeq
                                    where pd.ID = p.id
                                    group by pd.OrderID, pd.OrderShipmodeSeq, ap.ID
                                ) a 
                                for xml path('')
                            ), 1, 1, ''),
[PLFromRgCode] = '{1}'
, CutOffDate = cast(g.CutOffDate as Date)
, p.ShippingReasonIDForTypeCO
, Description = s.Description
from PackingList p WITH (NOLOCK) 
left join GMTBooking g WITH (NOLOCK) on p.INVNo = g.ID
left join ShippingReason s WITH (NOLOCK) on s.Type = 'CO' and s.ID = p.ShippingReasonIDForTypeCO
where p.ShipPlanID = '{0}'";
            DataTable gridData;
            string shipPlanID = MyUtility.Convert.GetString(this.masterDate["ID"]);
            DualResult result = DBProxy.Current.Select(null, string.Format(sqlCmd, shipPlanID, string.Empty), out gridData);

            if (!result)
            {
                MyUtility.Msg.ErrorBox("Loading error\r\n" + result.ToString());
                return;
            }

            List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByShipPlanID(this.masterDate["ID"].ToString());
            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtGridDataA2B;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, string.Format(sqlCmd, shipPlanID, plFromRgCode), out dtGridDataA2B);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Loading error\r\n" + result.ToString());
                    return;
                }

                dtGridDataA2B.MergeTo(ref gridData);
            }

            this.listControlBindingSource1.DataSource = gridData;
        }

        // Pullout Date的Validating
        private void DatePulloutDate_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.datePulloutDate.Value) && this.datePulloutDate.OldValue != this.datePulloutDate.Value)
            {
                if (this.CheckPullout((DateTime)MyUtility.Convert.GetDate(this.datePulloutDate.Value), MyUtility.Convert.GetString(Env.User.Keyword)))
                {
                    this.PulloutMsg(null, (DateTime)MyUtility.Convert.GetDate(this.datePulloutDate.Value));
                    this.datePulloutDate.Value = null;
                }
            }
        }

        // 檢查Pullout report是否已經Confirm
        private bool CheckPullout(DateTime pulloutDate, string mdivisionid)
        {
            if (MyUtility.Check.Empty(mdivisionid))
            {
                return MyUtility.Check.Seek(string.Format("select ID from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and Status = 'New'", Convert.ToDateTime(pulloutDate).ToString("yyyy/MM/dd")));
            }
            else
            {
                return MyUtility.Check.Seek(string.Format("select ID from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}' and Status <> 'New'", Convert.ToDateTime(pulloutDate).ToString("yyyy/MM/dd"), mdivisionid));
            }
        }

        // Process Pullout Date Message
        private void PulloutMsg(DataRow dr, DateTime dt)
        {
            MyUtility.Msg.WarningBox("Pullout date:" + Convert.ToDateTime(dt).ToString("yyyy/MM/dd") + " already exist pullout report and have been confirmed, can't modify!");
            if (dr != null)
            {
                dr["PulloutDate"] = dr["PulloutDate"];
            }
        }

        // Update Pullout Date
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            StringBuilder warningMsg = new StringBuilder();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = dt.Select("Selected = 1");
            foreach (DataRow dr in drfound)
            {
                string drPulloutDate = MyUtility.Check.Empty(dr["PulloutDate"]) ? string.Empty : Convert.ToDateTime(dr["PulloutDate"]).ToString("yyyy/MM/dd");

                if (!MyUtility.Check.Empty(dr["PulloutDate"]) && this.CheckPullout(Convert.ToDateTime(dr["PulloutDate"]), MyUtility.Convert.GetString(dr["MDivisionID"])))
                {
                    warningMsg.Append(string.Format("GB#: {0},  Packing No.: {1},  SP#: {2}, Pullout Date:{3}\r\n", MyUtility.Convert.GetString(dr["INVNo"]), MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["OrderID"]), drPulloutDate));
                    continue;
                }

                if (!MyUtility.Check.Empty(this.datePulloutDate.Value) && this.CheckPullout(Convert.ToDateTime(this.datePulloutDate.Value), MyUtility.Convert.GetString(dr["MDivisionID"])))
                {
                    warningMsg.Append(string.Format("GB#: {0},  Packing No.: {1},  SP#: {2}, Pullout Date:{3}\r\n", MyUtility.Convert.GetString(dr["INVNo"]), MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["OrderID"]), drPulloutDate));
                    continue;
                }

                if (MyUtility.Check.Empty(this.datePulloutDate.Value))
                {
                    dr["PulloutDate"] = DBNull.Value;
                }
                else
                {
                    dr["PulloutDate"] = this.datePulloutDate.Value;
                }
            }

            if (warningMsg.Length > 0)
            {
                MyUtility.Msg.WarningBox("Below record's pullout report already confirmed, can't be update pullout date!\r\n" + warningMsg.ToString());
            }
        }

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            IList<string> updateCmds = new List<string>();
            this.gridUpdatePulloutDate.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            Dictionary<string, List<string>> dicUpdCmdA2B = new Dictionary<string, List<string>>();

            DataTable disp = new DataTable();
            disp.Columns.Add("GB#");
            disp.Columns.Add("Packing No.");
            disp.Columns.Add("SP#");
            disp.Columns.Add("Seq");
            disp.Columns.Add("Pullout Date");

            // PulloutDate日期重複判斷
            var duplicItem = dt.ExtNotDeletedRows()
              .GroupBy(item => new
              {
                  pulloutDate = item["PulloutDate"],
              })
              .Select(item => new
              {
                  item.Key,
                  cnt = item.Count(),
              })
              .Select(item => item);
            if (duplicItem.Count() > 1)
            {
                foreach (DataRow dr2 in dt.Rows)
                {
                    DataRow dispdr = disp.NewRow();
                    dispdr["GB#"] = dr2["INVNo"];
                    dispdr["Packing No."] = dr2["ID"];
                    dispdr["SP#"] = dr2["OrderID"];
                    dispdr["Seq"] = dr2["OrderShipmodeSeq"];
                    dispdr["Pullout Date"] = MyUtility.Convert.GetDate(dr2["PulloutDate"]).ToYYYYMMDD();
                    disp.Rows.Add(dispdr);
                }

                var m = new Win.UI.MsgGridForm(disp, "The following Pullout Date needs to be the same", "Pullout Date needs to be the same", null, MessageBoxButtons.OK)
                {
                    Width = 650,
                };
                m.grid1.Columns[0].Width = 170;
                m.grid1.Columns[1].Width = 120;
                m.grid1.Columns[2].Width = 130;
                m.grid1.Columns[3].Width = 60;
                m.grid1.Columns[4].Width = 130;
                m.text_Find.Width = 140;
                m.btn_Find.Location = new Point(150, 6);
                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;

                m.ShowDialog();

                return;
            }

            this.CheckPulloutputIDD(dt);

            bool needReason = dt.Select("CutOffDate < PulloutDate").Any();

            if (needReason)
            {
                var frm = new P10_PulloutDateReason(dt);
                DialogResult returnResult = frm.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                string updatePackingListCmd = string.Empty;
                string updateGMT_DetailCmd = string.Empty;
                string updateShippingReason = needReason ? string.Format(",ShippingReasonIDForTypeCO = '{0}'", MyUtility.Convert.GetString(dr["ShippingReasonIDForTypeCO"])) : string.Empty;
                if (MyUtility.Check.Empty(dr["PulloutDate"]))
                {
                    updatePackingListCmd = string.Format("update PackingList set PulloutDate = null where ID = '{0}';", MyUtility.Convert.GetString(dr["ID"]));
                    updateGMT_DetailCmd = string.Format("update GMTBooking_Detail set PulloutDate = null where PackingListID = '{0}';", MyUtility.Convert.GetString(dr["ID"]));
                }
                else
                {
                    updatePackingListCmd = string.Format("update PackingList set PulloutDate = '{0}' {2} where ID = '{1}';", Convert.ToDateTime(dr["PulloutDate"]).ToString("yyyyMMdd"), MyUtility.Convert.GetString(dr["ID"]), updateShippingReason);
                    updateGMT_DetailCmd = string.Format("update GMTBooking_Detail set PulloutDate = '{0}' where PackingListID = '{1}';", Convert.ToDateTime(dr["PulloutDate"]).ToString("yyyyMMdd"), MyUtility.Convert.GetString(dr["ID"]));
                }

                string plFromRgCode = dr["PLFromRgCode"].ToString();

                if (MyUtility.Check.Empty(plFromRgCode))
                {
                    updateCmds.Add(updatePackingListCmd);
                }
                else
                {
                    if (!dicUpdCmdA2B.ContainsKey(plFromRgCode))
                    {
                        dicUpdCmdA2B.Add(plFromRgCode, new List<string>());
                    }

                    dicUpdCmdA2B[plFromRgCode].Add(updatePackingListCmd);
                    updateCmds.Add(updateGMT_DetailCmd);
                }
            }

            updateCmds.Add($"UPDATE ShipPlan Set EditDate=GETDATE(),EditName='{Env.User.UserID}' where ID = '{this.masterDate["ID"]}'");

            DualResult result;
            if (updateCmds.Count != 0)
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    result = DBProxy.Current.Executes(null, updateCmds);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.ErrorBox(result.ToString());
                        return;
                    }

                    foreach (KeyValuePair<string, List<string>> itemdicUpdCmdA2B in dicUpdCmdA2B)
                    {
                        result = PackingA2BWebAPI.ExecuteBySql(itemdicUpdCmdA2B.Key, itemdicUpdCmdA2B.Value.JoinToString(" "));
                        if (!result)
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.ErrorBox(result.ToString());
                            return;
                        }
                    }

                    transactionScope.Complete();
                }

                string listID = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["ID"])).JoinToString(",");
                Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(listID, string.Empty))
                           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

                // 因為會傳圖片，拆成單筆 PackingListNo 轉出，避免一次傳出的容量過大超過api大小限制
                foreach (DataRow dr in dt.Rows)
                {
                    #region ISP20201607 資料交換 - Gensong
                    if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
                    {
                        // 不透過Call API的方式，自己組合，傳送API
                        Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(MyUtility.Convert.GetString(dr["ID"]), string.Empty))
                            .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    #endregion
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void CheckPulloutputIDD(DataTable dtCheck)
        {
            if (dtCheck.Rows.Count == 0)
            {
                return;
            }

            string sqlGetSPAndSeq = $@"
alter table #tmp alter column ID varchar(13)

select  distinct pd.OrderID, pd.OrderShipmodeSeq, t.PulloutDate
from PackingList_Detail pd with (nolock)
inner join #tmp t on t.ID = pd.ID
";
            DataTable dtResult;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtCheck, "ID,PulloutDate", sqlGetSPAndSeq, out dtResult);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            var listCheckA2B = dtCheck.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["PLFromRgCode"]));

            if (listCheckA2B.Any())
            {
                var listCheckA2BByPLFromRgCode = listCheckA2B.GroupBy(s => s["PLFromRgCode"].ToString())
                            .Select(s => new
                            {
                                PLFromRgCode = s.Key,
                                TmpTable = JsonConvert.SerializeObject(s.CopyToDataTable()),
                            });
                foreach (var plFromRgCodeItem in listCheckA2BByPLFromRgCode)
                {
                    PackingA2BWebAPI_Model.DataBySql dataBySql = new PackingA2BWebAPI_Model.DataBySql()
                    {
                        SqlString = sqlGetSPAndSeq,
                        TmpTable = plFromRgCodeItem.TmpTable,
                        TmpCols = "ID,PulloutDate",
                    };

                    result = PackingA2BWebAPI.GetDataBySql(plFromRgCodeItem.PLFromRgCode, dataBySql, out DataTable dtResultA2B);

                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString());
                        return;
                    }

                    dtResultA2B.MergeTo(ref dtResult);
                }
            }

            if (dtResult.Rows.Count > 0)
            {
                #region 檢查傳入的SP 維護的IDD與PulloutputDate是否都為同一天(沒維護不判斷)
                List<Order_QtyShipKey> listOrder_QtyShipKey = dtResult.AsEnumerable().Select(s => new Order_QtyShipKey
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
}
