using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P11
    /// </summary>
    public partial class P11 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// P11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("ID", header: "Garment Booking", width: Widths.AnsiChars(30))
            ;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.disExVoucherID.Text = this.CurrentMaintain["ExVoucherID"].ToString();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Garment Booking cannot be empty !");
                return false;
            }

            List<string> ls = new List<string>();
            foreach (DataRow dr in this.DetailDatas)
            {
                if (!MyUtility.Check.Seek($@"select 1 from GMTBooking where id = '{dr["id"]}'"))
                {
                    ls.Add(MyUtility.Convert.GetString(dr["id"]));
                }
            }

            if (ls.Count > 0)
            {
                MyUtility.Msg.WarningBox($@"Garment Booking does not exist, please check again!
{string.Join(", ", ls)}");
                return false;
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
        }

        /// <inheritdoc/>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                return new DualResult(false, $"ID({this.CurrentMaintain["ID"].ToString()}) is empty,Please inform MIS to handle this issue");
            }

            string updatesql = $@"update GMTBooking set BIRID = null  where BIRID = '{this.CurrentMaintain["ID"]}'  ";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                return result;
            }

            IList<string> updateCmds = new List<string>();

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified)
                {
                    updateCmds.Add($@"update GMTBooking set BIRID = '{this.CurrentMaintain["ID"]}' where ID = '{dr["id"]}';");
                }

                if (dr.RowState == DataRowState.Deleted)
                {
                    updateCmds.Add($@"update GMTBooking set BIRID = null where ID = '{dr["id", DataRowVersion.Original]}';");
                }
            }

            if (updateCmds.Count != 0)
            {
                result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, result.ToString());
                    return failResult;
                }
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override DualResult ClickDelete()
        {
            string updatesql = $@"update GMTBooking set BIRID = null  where BIRID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                return result;
            }

            return base.ClickDelete();
        }

        protected override void ClickConfirm()
        {
            string sqlupdate = $@"
update BIRInvoice set Status='Approved', Approve='{Sci.Env.User.UserID}', ApproveDate=getdate(), EditName='{Sci.Env.User.UserID}', EditDate=getdate()
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlupdate);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            base.ClickConfirm();
        }

        protected override void ClickUnconfirm()
        {
            string sqlchk = $@"select 1 from BIRInvoice  where ExVoucherID !='' and id = '{this.CurrentMaintain["ID"]}'";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Cannot unconfirm because already created voucher no");
                return;
            }

            string sqlupdate = $@"
update BIRInvoice set Status='New', Approve='{Sci.Env.User.UserID}', ApproveDate=getdate(), EditName='{Sci.Env.User.UserID}', EditDate=getdate()
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlupdate);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            base.ClickUnconfirm();
        }

        protected override bool ClickDeleteBefore()
        {
            string sqlchk = $@"select 1 from BIRInvoice  where ExVoucherID is not null and id = '{this.CurrentMaintain["ID"]}' and status = 'Approved' ";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Already approved, cannot delete!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        private void Btnimport_Click(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["InvSerial"]))
            {
                MyUtility.Msg.WarningBox("Invoice Serial cannot be empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand cannot be empty!");
                return;
            }

            string sqlchk = $@"select 1 from BIRInvoice b where b.InvSerial = '{this.CurrentMaintain["InvSerial"]}' and b.BrandID = '{this.CurrentMaintain["BrandID"]}'";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Already has this reocrd!");
                return;
            }

            string sqlcmd = $@"
select *
from GMTBooking with(nolock)
where isnull(BIRID,0) = 0
and BrandID = '{this.CurrentMaintain["BrandID"]}'
and InvSerial like '{this.CurrentMaintain["InvSerial"]}%'
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
                this.ShowErr("Import error!");
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                dr.AcceptChanges();
                dr.SetAdded();
                ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P11_Print p11_Print = new P11_Print(this.CurrentMaintain, this.DetailDatas);
            p11_Print.ShowDialog();
            return base.ClickPrint();
        }

        private void BtnBatchApprove(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P11_BatchApprove callNextForm = new P11_BatchApprove(this.Reload);
            callNextForm.ShowDialog(this);
        }

        public void Reload()
        {
            this.ReloadDatas();
            this.RenewData();
        }
    }
}
