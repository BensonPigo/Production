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

namespace Sci.Production.Shipping
{
    public partial class P10_UpdatePulloutDate : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        Ict.Win.UI.DataGridViewDateBoxColumn col_pulldate;
        private DataRow masterDate;
        public P10_UpdatePulloutDate(DataRow MasterData)
        {
            InitializeComponent();
            masterDate = MasterData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlCmd = string.Format(@"select 0 as Selected,p.INVNo as GMTBookingID,p.ID as PackingListID,iif(p.OrderID='',(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd where pd.ID = p.id) a for xml path('')),p.OrderID) as OrderID,
iif(p.type = 'B',(select BuyerDelivery from Order_QtyShip where ID = p.OrderID and Seq = p.OrderShipmodeSeq),(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = p.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq)) as BuyerDelivery,
p.PulloutDate,p.Status,p.CTNQty,p.InspDate,p.InspStatus,(select isnull(sum(CTNQty),0) from PackingList_Detail pd where pd.ID = p.ID and pd.ReceiveDate is not null) as ClogQty,p.MDivisionID
from PackingList p
where p.ShipPlanID = '{0}'", MyUtility.Convert.GetString(masterDate["ID"]));
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Loading error\r\n" + result.ToString());
            }

            listControlBindingSource1.DataSource = gridData;

            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("GMTBookingID", header: "GB#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("PackingListID", header: "Packing No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("PulloutDate", header: "Pullout Date").Get(out col_pulldate)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Text("Status", header: "Packing Status", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Numeric("CTNQty", header: "CTN Qty")
                .Numeric("ClogQty", header: "CTN Qty at C-Log")
                .Date("InspDate", header: "est. Inspection Date", iseditingreadonly: true)
                .Text("InspStatus", header: "Inspection Status", width: Widths.AnsiChars(10), iseditingreadonly: true);

            grid1.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    //輸入的Pullout date或原本的Pullout date的Pullout Report如果已經Confirmed的話，就不可以被修改
                    if (grid1.Columns[e.ColumnIndex].DataPropertyName == col_pulldate.DataPropertyName)
                    {
                        if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["PulloutDate"]))
                        {
                            if (!MyUtility.Check.Empty(dr["PulloutDate"]) && CheckPullout((DateTime)MyUtility.Convert.GetDate(dr["PulloutDate"]), MyUtility.Convert.GetString(dr["MDivisionID"])))
                            {
                                PulloutMsg(dr, (DateTime)MyUtility.Convert.GetDate(dr["PulloutDate"]));
                                e.Cancel = true;
                                dr.EndEdit();
                                return;
                            }
                            if (!MyUtility.Check.Empty(e.FormattedValue) && CheckPullout((DateTime)MyUtility.Convert.GetDate(e.FormattedValue), MyUtility.Convert.GetString(dr["MDivisionID"])))
                            {
                                PulloutMsg(dr, (DateTime)MyUtility.Convert.GetDate(e.FormattedValue));
                                e.Cancel = true;
                                dr.EndEdit();
                                return;
                            }
                        }
                    }
                }
            };
        }

        //Pullout Date的Validating
        private void dateBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(dateBox1.Value) && dateBox1.OldValue != dateBox1.Value)
            {
                if (CheckPullout((DateTime)MyUtility.Convert.GetDate(dateBox1.Value), ""))
                {
                    PulloutMsg(null, (DateTime)MyUtility.Convert.GetDate(dateBox1.Value));
                    dateBox1.Value = null;
                }
            }
        }

        //檢查Pullout report是否已經Confirm
        private bool CheckPullout(DateTime pulloutDate, string mdivisionid)
        {
            if (MyUtility.Check.Empty(mdivisionid))
            {
                return MyUtility.Check.Seek(string.Format("select ID from Pullout where PulloutDate = '{0}' and Status = 'New'", Convert.ToDateTime(pulloutDate).ToString("d")));
            }
            else
            {
                return MyUtility.Check.Seek(string.Format("select ID from Pullout where PulloutDate = '{0}' and MDivisionID = '{1}' and Status <> 'New'", Convert.ToDateTime(pulloutDate).ToString("d"), mdivisionid));
            }
        }

        //Process Pullout Date Message
        private void PulloutMsg(DataRow dr, DateTime dt)
        {
            MyUtility.Msg.WarningBox("Pullout date:" + Convert.ToDateTime(dt).ToString("d") + " already exist pullout report and have been confirmed, can't modify!");
            if (dr != null)
            {
                dr["PulloutDate"] = dr["PulloutDate"];
            }
        }

        //Update Pullout Date
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            StringBuilder warningMsg = new StringBuilder();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] drfound = dt.Select("Selected = 1");
            foreach (DataRow dr in drfound)
            {
                if (!MyUtility.Check.Empty(dr["PulloutDate"]) && CheckPullout(Convert.ToDateTime(dr["PulloutDate"]), MyUtility.Convert.GetString(dr["MDivisionID"])))
                {
                    warningMsg.Append(string.Format("GB#: {0},  Packing No.: {1},  SP#: {2}, Pullout Date:{3}\r\n", MyUtility.Convert.GetString(dr["GMTBookingID"]), MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), Convert.ToDateTime(dr["PulloutDate"]).ToString("d")));
                    continue;
                }
                if (!MyUtility.Check.Empty(dateBox1.Value) && CheckPullout(Convert.ToDateTime(dateBox1.Value), MyUtility.Convert.GetString(dr["MDivisionID"])))
                {
                    warningMsg.Append(string.Format("GB#: {0},  Packing No.: {1},  SP#: {2}, Pullout Date:{3}\r\n", MyUtility.Convert.GetString(dr["GMTBookingID"]), MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), Convert.ToDateTime(dr["PulloutDate"]).ToString("d")));
                    continue;
                }

                if (MyUtility.Check.Empty(dateBox1.Value))
                {
                    dr["PulloutDate"] = DBNull.Value;
                }
                else
                {
                    dr["PulloutDate"] = dateBox1.Value;
                }
            }
            if (warningMsg.Length > 0)
            {
                MyUtility.Msg.WarningBox("Below record's pullout report already confirmed, can't be update pullout date!\r\n" + warningMsg.ToString());
            }
        }

        //Save
        private void button1_Click(object sender, EventArgs e)
        {
            IList<string> updateCmds = new List<string>();
            grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
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

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
