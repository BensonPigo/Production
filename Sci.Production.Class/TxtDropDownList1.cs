using System.ComponentModel;
using Sci.Win.UI;
using System.Windows.Forms;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtDropDownList
    /// </summary>
    public partial class TxtDropDownList : Win.UI.TextBox
    {
        /// <summary>
        /// Type
        /// </summary>
        [Category("Custom Properties")]
        public string Type { get; set; }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            #region SQL CMD
            string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{this.Type}' 
order by Seq";
            #endregion
            SelectItem item = new SelectItem(sqlcmd, "ID,Name", this.Text, false, null);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            #region SQL CMD
            string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{this.Type}' 
and id ='{str}'
order by Seq";
            #endregion
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(sqlcmd) == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Return TO : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
