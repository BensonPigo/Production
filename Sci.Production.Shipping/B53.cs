using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B53
    /// </summary>
    public partial class B53 : Sci.Win.Tems.Input1
    {
        private string editName;
        private DateTime? editDate;

        /// <summary>
        /// B53
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B53(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.labelWeight.Text = string.Format("Weight\r\n(kgs/{0})", MyUtility.Convert.GetString(this.CurrentMaintain["UnitID"]));
            this.txtGoodsDescription.Text = MyUtility.GetValue.Lookup(string.Format("select GoodsDescription from KHGoodsHSCode WITH (NOLOCK) where NLCode = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["NLCode"])));
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.editName = MyUtility.Convert.GetString(this.CurrentMaintain["EditName"]);
            this.editDate = MyUtility.Convert.GetDate(this.CurrentMaintain["EditDate"]);
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.editDescription.ReadOnly = true;
            this.txtSubconSupplier.TextBox1.ReadOnly = true;
            this.checkJunk.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.CurrentMaintain["NLCodeEditName"] = Sci.Env.User.UserID;
            this.CurrentMaintain["NLCodeEditDate"] = DateTime.Now;

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult ClickSavePost()
        {
            string updateCmd;
            if (MyUtility.Check.Empty(this.editDate))
            {
                updateCmd = string.Format("update LocalItem Set EditName = '{0}', EditDate = null where RefNo = '{1}';", this.editName, MyUtility.Convert.GetString(this.CurrentMaintain["RefNo"]));
            }
            else
            {
                updateCmd = string.Format("update LocalItem Set EditName = '{0}', EditDate = '{1}' where RefNo = '{2}';", this.editName, Convert.ToDateTime(this.editDate).ToString("yyyy/MM/dd HH:mm:ss"), MyUtility.Convert.GetString(this.CurrentMaintain["RefNo"]));
            }

            return DBProxy.Current.Execute(null, updateCmd);
        }

        // Good's Description
        private void TxtGoodsDescription_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(
                    @"select g.GoodsDescription, g.Category, isnull(cd.UnitID,'') as UnitID, g.NLCode
from KHGoodsHSCode g WITH (NOLOCK) 
left join KHContract_Detail cd WITH (NOLOCK) on g.NLCode = cd.NLCode
where g.Junk = 0
and cd.ID in (select ID from (select ID,MAX(StartDate) as MaxDate from KHContract WITH (NOLOCK) where Status = 'Confirmed' group by ID) a)
and Category <> 'MACHINERY'
order by GoodsDescription",
                    "50,10,8,0",
                    this.Text,
                    false,
                    ",",
                    headercaptions: "Good's Description,Category,Unit,");

                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                IList<DataRow> selectedData = item.GetSelecteds();
                this.txtGoodsDescription.Text = item.GetSelectedString();
                this.CurrentMaintain["NLCode"] = selectedData[0]["NLCode"];
                this.CurrentMaintain["CustomsUnit"] = selectedData[0]["UnitID"];
            }
        }

        // Good's Description
        private void TxtGoodsDescription_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtGoodsDescription.OldValue != this.txtGoodsDescription.Text)
            {
                if (MyUtility.Check.Empty(this.txtGoodsDescription.Text))
                {
                    this.txtGoodsDescription.Text = string.Empty;
                    this.CurrentMaintain["NLCode"] = string.Empty;
                    this.CurrentMaintain["CustomsUnit"] = string.Empty;
                }
                else
                {
                    DataRow nLCodeDate;
                    if (MyUtility.Check.Seek(string.Format(@"select GoodsDescription,NLCode from KHGoodsHSCode WITH (NOLOCK) where GoodsDescription = '{0}'", this.txtGoodsDescription.Text), out nLCodeDate))
                    {
                        this.txtGoodsDescription.Text = this.txtGoodsDescription.Text;
                        this.CurrentMaintain["NLCode"] = nLCodeDate["NLCode"];
                        this.CurrentMaintain["CustomsUnit"] = MyUtility.GetValue.Lookup(string.Format("select TOP(1) UnitID from KHContract_Detail WITH (NOLOCK) where NLCode = '{0}' and ID in (select ID from (select ID,MAX(StartDate) as MaxDate from KHContract WITH (NOLOCK) where Status = 'Confirmed' group by ID) a)", MyUtility.Convert.GetString(nLCodeDate["NLCode"])));
                    }
                    else
                    {
                        this.txtGoodsDescription.Text = string.Empty;
                        this.CurrentMaintain["NLCode"] = string.Empty;
                        this.CurrentMaintain["CustomsUnit"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("The Good's Description is not in the Contract!!");
                        return;
                    }
                }
            }
        }
    }
}
