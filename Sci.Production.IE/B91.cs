using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// B91
    /// </summary>
    public partial class B91 : Win.Tems.Input1
    {
        /// <summary>
        /// B91
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B91(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnDetailEntered()
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.CurrentMaintain["Type"].Empty())
            {
                MyUtility.Msg.WarningBox("Type cannot be empty!!");
                return false;
            }

            if (this.CurrentMaintain["TypeGroup"].Empty())
            {
                MyUtility.Msg.WarningBox("TypeGroup cannot be empty!!");
                return false;
            }

            if (this.CurrentMaintain["Code"].Empty())
            {
                MyUtility.Msg.WarningBox("Code cannot be empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void TxtType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string type = this.txtType.Text.ToString();
            string sql = $@"SELECT Type, TypeGroup FROM IEReasonTypeGroup";
            if (!type.Empty())
            {
                sql += $" Where Type = '{type}'";
            }

            sql += " order by Type, TypeGroup" + Environment.NewLine;
            DBProxy.Current.Select("Production", sql, out DataTable dt);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                dt,
                "Type,TypeGroup",
                "20,20",
                this.txtType.Text)
            {
                Width = 650,
            };

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["Type"] = item.GetSelectedString();
            this.CurrentMaintain["TypeGroup"] = item.GetSelecteds()[0]["TypeGroup"].ToString().TrimEnd();
            this.ValidateControl();
        }
    }
}
