using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtthreadlocation
    /// </summary>
    public partial class Txtthreadlocation : Win.UI.TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Txtthreadlocation"/> class.
        /// </summary>
        public Txtthreadlocation()
        {
            this.Size = new System.Drawing.Size(90, 23);
            this.IsSupportSytsemContextMenu = false;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            if (e.IsHandled)
            {
                return;
            }

            string keyword = Env.User.Keyword;
            string sql = "select ID,Description from ThreadLocation WITH (NOLOCK) order by ID";
            DataTable tbthLocation;
            DBProxy.Current.Select("Production", sql, out tbthLocation);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbthLocation, "ID,Description", "10,40", this.Text, "ID,Description");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            e.IsHandled = true;
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string keyword = Env.User.Keyword;
            string str = this.Text;
            if (!MyUtility.Check.Empty(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(str, "ThreadLocation", "id") == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Thread Location : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
