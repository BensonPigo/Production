using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// PPIC_P14
    /// </summary>
    public partial class P14 : Win.Tems.Input1
    {
        private string M = Sci.Env.User.Keyword;
        private DualResult result;

        /// <summary>
        /// PPIC_P14
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"MdivisionID='{this.M}'";
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.labelStatus.Text = this.CurrentMaintain["status"].ToString();
            this.txtApproveDate.Text = MyUtility.Check.Empty(this.CurrentMaintain["ApproveDate"]) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["ApproveDate"])).ToString("yyyy/MM/dd HH:mm");
            this.txtConfirmDate.Text = MyUtility.Check.Empty(this.CurrentMaintain["ConfirmDate"]) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["ConfirmDate"])).ToString("yyyy/MM/dd HH:mm");
            this.txtTPEEditDate.Text = MyUtility.Check.Empty(this.CurrentMaintain["TPEEditDate"]) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["TPEEditDate"])).ToString("yyyy/MM/dd HH:mm");
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).EqualString("Sent"))
            {
                this.btnmail.Enabled = true;
            }
            else
            {
                this.btnmail.Enabled = false;
            }

            #region 依狀態調整Label 名稱
            if (string.Compare(this.CurrentMaintain["Status"].ToString(), "Reject", true) == 0)
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["ApproveName"]) && MyUtility.Check.Empty(this.CurrentMaintain["ConfirmName"]))
                {
                    this.labPurchaseApvName.Text = "Reject Name";
                    this.labPurchaseApvDate.Text = "Reject Date";
                    this.labPlanningConfName.Text = "Approve Name";
                    this.labPlanningConfDate.Text = "Approve Date";
                }
                else if (!MyUtility.Check.Empty(this.CurrentMaintain["ApproveName"]) && !MyUtility.Check.Empty(this.CurrentMaintain["ConfirmName"]))
                {
                    this.labPurchaseApvName.Text = "Approve Name";
                    this.labPurchaseApvDate.Text = "Approve Date";
                    this.labPlanningConfName.Text = "Reject Name";
                    this.labPlanningConfDate.Text = "Reject Date";
                }
            }
            else
            {
                this.labPurchaseApvName.Text = "Approve Name";
                this.labPurchaseApvDate.Text = "Approve Date";
                this.labPlanningConfName.Text = "Approve Name";
                this.labPlanningConfDate.Text = "Approve Date";
            }
            #endregion
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MdivisionID"] = this.M;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            string spNo = this.txtOrderID.Text;
            DataTable dtSP;
            if (MyUtility.Check.Empty(this.txtOrderID.Text))
            {
                MyUtility.Msg.WarningBox("<OrderID> cannot be empty!");
                this.txtOrderID.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.dateNewKPI.Value))
            {
                MyUtility.Msg.WarningBox("<New KPILETA> cannot be empty!");
                this.dateNewKPI.Focus();
                return false;
            }

            if (!MyUtility.Check.Seek(
    $@"
select ID,KPILETA from orders 
where id = '{spNo}'
and Junk = 0
and MDivisionID = '{this.M}' and (category = 'S' or (category = 'B' and localorder = 0))
"))
            {
                MyUtility.Msg.WarningBox($"<OrderID {spNo}> not found!");
                this.txtOrderID.Focus();
                return false;
            }

            // 新增模式, 檢查如果SPNO已存在,Status必須要Confirmed or Reject才能新增
            if (this.IsDetailInserting)
            {
                if (this.result = DBProxy.Current.Select(null, $@"select Status from ChangeKPILETARequest where orderid='{spNo}' and MdivisionID='{this.M}'", out dtSP))
                {
                    if (!MyUtility.Check.Empty(dtSP) || dtSP.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtSP.Rows)
                        {
                            if ((dr["Status"].ToString().ToUpper() != "CONFIRMED") && (dr["Status"].ToString().ToUpper() != "Reject"))
                            {
                                MyUtility.Msg.WarningBox("This SP# already have request but not yet confirmed. You can’t create new request !!");
                                this.txtOrderID.Focus();
                                return false;
                            }
                        }
                    }
                }
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "KR", " ChangeKPILETARequest ", DateTime.Today, 3, "Id", null);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// ClickDeleteBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickDeleteBefore()
        {
            string status = this.CurrentMaintain["Status"].ToString();
            if (status != "New")
            {
                MyUtility.Msg.WarningBox($"Data has been {status}, cannot delete!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <summary>
        /// ClickEditBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickEditBefore()
        {
            string status = this.CurrentMaintain["Status"].ToString();
            if (status != "New")
            {
                MyUtility.Msg.WarningBox($"Data has been {status}, cannot modify!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <summary>
        /// ClickSend
        /// </summary>
        protected override void ClickSend()
        {
            base.ClickSend();
            if (!(this.result = DBProxy.Current.Execute(null, $@"update ChangeKPILETARequest set status='Sent' where id='{this.CurrentMaintain["ID"]}'")))
            {
                MyUtility.Msg.WarningBox("update status fail!!\r\n" + this.result.ToString());
                return;
            }

            this.CurrentMaintain["Status"] = "Sent";
            this.Mail();
        }

        private void TxtOrderID_Validating(object sender, CancelEventArgs e)
        {
            string spNo = this.txtOrderID.Text;
            DataRow dr;
            if (!this.EditMode)
            {
                return;
            }

            // 檢查OrderID 不能被Junk,相同的M,category=S or B單 ,非Local單
            if (!MyUtility.Check.Seek(
                $@"
select ID,KPILETA from orders 
where id = '{spNo}'
and Junk = 0
and MDivisionID = '{this.M}' and (category = 'S' or (category = 'B' and localorder = 0))
", out dr))
            {
                MyUtility.Msg.WarningBox($"<OrderID {spNo}> not found!");
                this.txtOrderID.Focus();
                return;
            }
            else
            {
                this.CurrentMaintain["OrderID"] = dr["ID"];
                this.CurrentMaintain["OldKPILETA"] = dr["KPILETA"];
            }
        }

        private void Btnmail_Click(object sender, EventArgs e)
        {
            this.Mail();
        }

        private void Mail()
        {
            string mrHandle = MyUtility.GetValue.Lookup($"select EMail from TPEPass1 where ID=(select MRHandle from Orders WHERE ID='{this.CurrentMaintain["Orderid"]}')");
            string smr = MyUtility.GetValue.Lookup($"select EMail from TPEPass1 where ID=(select SMR from Orders WHERE ID='{this.CurrentMaintain["Orderid"]}')");
            string poid = MyUtility.GetValue.Lookup($"select poid from orders where id = '{this.CurrentMaintain["Orderid"]}'");
            string poHandle = MyUtility.GetValue.Lookup($"select EMail from TPEPass1 where ID=(select POHandle from PO WITH (NOLOCK) where ID='{poid}')");
            string poSMR = MyUtility.GetValue.Lookup($"select EMail from TPEPass1 where ID=(select POSMR from PO WITH (NOLOCK) where ID='{poid}')");
            string toAddress = mrHandle + ";" + smr + ";" + poHandle + ";" + poSMR;
            string ccAddress = "Planning@sportscity.com.tw ; " + Sci.Env.User.MailAddress;
            string subject = $" Change Order KPI LETA Request – Request#:{this.CurrentMaintain["id"]} , SP#:{this.CurrentMaintain["Orderid"]} ";
            string content = $@"Hi All, 
Please help to change KPI LETA to {MyUtility.Convert.GetDate(this.CurrentMaintain["NewKPILETA"]).Value.ToShortDateString()} from {MyUtility.Convert.GetDate(this.CurrentMaintain["OldKPILETA"]).Value.ToShortDateString()}

Thank you.";
            var email = new MailTo(Sci.Env.Cfg.MailFrom, toAddress, ccAddress, subject, null, content, false, true);
            email.ShowDialog(this);
        }
    }
}
