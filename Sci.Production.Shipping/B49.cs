using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class B49 : Win.Tems.Input1
    {
        public B49(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "NeedDeclare = 1";
        }

        protected override void OnFormLoaded()
        {
            this.txtSubconSupplier.TextBox1.ReadOnly = true;
            this.txtSubconSupplier.TextBox1.IsSupportEditMode = false;
            base.OnFormLoaded();
        }

        private void txtCustomerCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
order by NLCode",
                "5,11,8",
                this.Text,
                false,
                ",",
                headercaptions: "Customs Code, HSCode, Unit");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selectedData = item.GetSelecteds();
            this.CurrentMaintain["NLCode"] = item.GetSelectedString();
            this.CurrentMaintain["HSCode"] = selectedData[0]["HSCode"];
            this.CurrentMaintain["CustomsUnit"] = selectedData[0]["UnitID"];
            this.CurrentMaintain.EndEdit();
        }

        private void txtCustomerCode_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtCustomerCode.OldValue != this.txtCustomerCode.Text)
            {
                if (MyUtility.Check.Empty(this.txtCustomerCode.Text))
                {
                    this.CurrentMaintain["NLCode"] = string.Empty;
                    this.CurrentMaintain["HSCode"] = string.Empty;
                    this.CurrentMaintain["CustomsUnit"] = string.Empty;
                }
                else
                {
                    DataRow nLCodeDate;
                    if (MyUtility.Check.Seek(
                        string.Format(
                            @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
and NLCode = '{0}'", this.txtCustomerCode.Text), out nLCodeDate))
                    {
                        this.CurrentMaintain["NLCode"] = this.txtCustomerCode.Text;
                        this.CurrentMaintain["HSCode"] = nLCodeDate["HSCode"];
                        this.CurrentMaintain["CustomsUnit"] = nLCodeDate["UnitID"];
                    }
                    else
                    {
                        this.CurrentMaintain["NLCode"] = string.Empty;
                        this.CurrentMaintain["HSCode"] = string.Empty;
                        this.CurrentMaintain["CustomsUnit"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("The Customs Code is not in the Contract!!");
                        return;
                    }
                }

                this.CurrentMaintain.EndEdit();
            }
        }

        protected override DualResult ClickSave()
        {
            // 修改表身資料,不寫入表頭EditName and EditDate
            ITableSchema pass1Schema;
            var ok = DBProxy.Current.GetTableSchema("Machine", "Misc", out pass1Schema);
            pass1Schema.IsSupportEditDate = false;
            pass1Schema.IsSupportEditName = false;

            decimal pcslength = MyUtility.Check.Empty(this.CurrentMaintain["PcsLength"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["PcsLength"]);
            decimal pcsWidth = MyUtility.Check.Empty(this.CurrentMaintain["PcsWidth"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["PcsWidth"]);
            decimal pcsKG = MyUtility.Check.Empty(this.CurrentMaintain["PcsKg"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["PcsKg"]);
            decimal miscRate = MyUtility.Check.Empty(this.CurrentMaintain["MiscRate"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["MiscRate"]);

            string strUpdate = $@"
update Misc
set UsageUnit = '{this.CurrentMaintain["UsageUnit"]}'
,NLCode  = '{this.CurrentMaintain["NLCode"]}'
,CustomsUnit = '{this.CurrentMaintain["CustomsUnit"]}'
,HSCode = '{this.CurrentMaintain["HSCode"]}'
,PcsLength = '{pcslength}'
,PcsWidth = '{pcsWidth}'
,PcsKg = '{pcsKG}'
,MiscRate = {miscRate}
,NLCodeEditDate = '{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}'
,NLCodeEditName = '{Env.User.UserID}'
where id='{this.CurrentMaintain["ID"]}'
";
            DualResult result;
            if (!(result = DBProxy.Current.Execute("Machine", strUpdate)))
            {
                this.ShowErr(result);
            }

            return Ict.Result.True;
        }
    }
}
