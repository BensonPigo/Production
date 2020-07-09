using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtSpreadingNo
    /// </summary>
    public partial class TxtSpreadingNo : Win.UI.TextBox
    {
        private string Where = "WHERE 1=1";

        /// <summary>
        /// SpreadingNo.MDivision
        /// </summary>
        [Category("Custom Properties")]
        public string MDivision { get; set; } = string.Empty;

        /// <summary>
        /// SpreadingNo.Junk
        /// </summary>
        [Category("Custom Properties")]
        public bool IncludeJunk { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtSpreadingNo"/> class.
        /// </summary>
        public TxtSpreadingNo()
        {
            this.Width = 45;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql;
            if (!string.IsNullOrWhiteSpace(this.MDivision))
            {
                this.Where += $"AND MDivisionID = '{this.MDivision}'";
            }

            // 不包含Junk，因此去除Junk = 1 的資料
            if (!this.IncludeJunk)
            {
                this.Where += $"AND junk = 0 ";
            }

            sql = "select distinct id,CutCell= CutCellID from SpreadingNo WITH (NOLOCK) " + this.Where;
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, string.Empty, this.Text, false, ",");
            item.Width = 300;
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string newValue = this.Text;
            bool isJunked = false;
            if (!string.IsNullOrWhiteSpace(newValue) && newValue != this.OldValue)
            {
                string tmp = null;
                if (!string.IsNullOrWhiteSpace(this.MDivision) && this.IncludeJunk)
                {
                    tmp = MyUtility.GetValue.Lookup($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND MDivisionid='{this.MDivision}'");
                }
                else if (string.IsNullOrWhiteSpace(this.MDivision) && this.IncludeJunk)
                {
                    tmp = MyUtility.GetValue.Lookup($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' ");
                }
                else if (!string.IsNullOrWhiteSpace(this.MDivision) && !this.IncludeJunk)
                {
                    tmp = MyUtility.GetValue.Lookup($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND MDivisionid='{this.MDivision}' AND Junk=0");
                }
                else if (string.IsNullOrWhiteSpace(this.MDivision) && !this.IncludeJunk)
                {
                    tmp = MyUtility.GetValue.Lookup($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND MDivisionid='{this.MDivision}' AND Junk=0");
                }

                if (!string.IsNullOrWhiteSpace(this.MDivision))
                {
                    isJunked = MyUtility.Check.Seek($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND MDivisionid='{this.MDivision}' AND Junk=1");
                }
                else
                {
                    isJunked = MyUtility.Check.Seek($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND Junk=1");
                }

                if (string.IsNullOrWhiteSpace(tmp))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;

                    if (isJunked)
                    {
                        MyUtility.Msg.WarningBox(string.Format("Spreading No already junk, you can't choose!!"));
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Spreading No > : {0} not found!!!", newValue));
                    }

                    return;
                }
            }
        }
    }
}