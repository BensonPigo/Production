using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;
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
            string sqlCmd = string.Format(
                @"select 
0 as Selected,
p.INVNo as GMTBookingID,
p.ID as PackingListID,
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
                          ), 1, 1, '') 
from PackingList p WITH (NOLOCK) 
where p.ShipPlanID = '{0}'", MyUtility.Convert.GetString(this.masterDate["ID"]));
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Loading error\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;

            this.gridUpdatePulloutDate.IsEditingReadOnly = false;
            this.gridUpdatePulloutDate.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridUpdatePulloutDate)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("GMTBookingID", header: "GB#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("PackingListID", header: "Packing No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
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
                return MyUtility.Check.Seek(string.Format("select ID from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and Status = 'New'", Convert.ToDateTime(pulloutDate).ToString("d")));
            }
            else
            {
                return MyUtility.Check.Seek(string.Format("select ID from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}' and Status <> 'New'", Convert.ToDateTime(pulloutDate).ToString("d"), mdivisionid));
            }
        }

        // Process Pullout Date Message
        private void PulloutMsg(DataRow dr, DateTime dt)
        {
            MyUtility.Msg.WarningBox("Pullout date:" + Convert.ToDateTime(dt).ToString("d") + " already exist pullout report and have been confirmed, can't modify!");
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
                string drPulloutDate = MyUtility.Check.Empty(dr["PulloutDate"]) ? string.Empty : Convert.ToDateTime(dr["PulloutDate"]).ToString("d");

                if (!MyUtility.Check.Empty(dr["PulloutDate"]) && this.CheckPullout(Convert.ToDateTime(dr["PulloutDate"]), MyUtility.Convert.GetString(dr["MDivisionID"])))
                {
                    warningMsg.Append(string.Format("GB#: {0},  Packing No.: {1},  SP#: {2}, Pullout Date:{3}\r\n", MyUtility.Convert.GetString(dr["GMTBookingID"]), MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), drPulloutDate));
                    continue;
                }

                if (!MyUtility.Check.Empty(this.datePulloutDate.Value) && this.CheckPullout(Convert.ToDateTime(this.datePulloutDate.Value), MyUtility.Convert.GetString(dr["MDivisionID"])))
                {
                    warningMsg.Append(string.Format("GB#: {0},  Packing No.: {1},  SP#: {2}, Pullout Date:{3}\r\n", MyUtility.Convert.GetString(dr["GMTBookingID"]), MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), drPulloutDate));
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

            this.CheckPulloutputIDD(dt);

            foreach (DataRow dr in dt.Rows)
            {
                if (MyUtility.Check.Empty(dr["PulloutDate"]))
                {
                    updateCmds.Add(string.Format("update PackingList set PulloutDate = null where ID = '{0}';", MyUtility.Convert.GetString(dr["PackingListID"])));
                }
                else
                {
                    updateCmds.Add(string.Format("update PackingList set PulloutDate = '{0}' where ID = '{1}';", Convert.ToDateTime(dr["PulloutDate"]).ToString("d"), MyUtility.Convert.GetString(dr["PackingListID"])));
                }
            }

            updateCmds.Add($"UPDATE ShipPlan Set EditDate=GETDATE(),EditName='{Env.User.UserID}'");

            DualResult result;
            if (updateCmds.Count != 0)
            {
                result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return;
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
alter table #tmp alter column PackingListID varchar(13)

select  distinct pd.OrderID, pd.OrderShipmodeSeq, t.PulloutDate
from PackingList_Detail pd with (nolock)
inner join #tmp t on t.PackingListID = pd.ID
";
            DataTable dtResult;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtCheck, "PackingListID,PulloutDate", sqlGetSPAndSeq, out dtResult);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
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
