using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// ToPlace
    /// </summary>
    public partial class TxtToPlace : Win.UI.TextBox
    {
        public readonly string DefaultText = "LOADING";

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtToPlace"/> class.
        /// </summary>
        public TxtToPlace()
        {
            this.Width = 150;
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql;

            sql = $"select [id] = '{this.DefaultText}' union all select distinct id from Production.dbo.SewingLine WITH (NOLOCK)";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "100", string.Empty, true, ",")
            {
                Width = 300,
            };
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
            string str = this.Text;

            if (str == this.DefaultText || MyUtility.Check.Empty(str))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                DataTable dtAllSewingLine;

                DualResult result = DBProxy.Current.Select(null, $"select [ID] = '{this.DefaultText}' union all select distinct ID from Production.dbo.SewingLine WITH (NOLOCK)", out dtAllSewingLine);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.GetException().ToString());
                    return;
                }

                foreach (string line in str.Split(','))
                {
                    if (!dtAllSewingLine.AsEnumerable().Any(s => s["ID"].ToString().ToUpper() == line.ToUpper()))
                    {
                        MyUtility.Msg.WarningBox($"{line} not exists");
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }
    }
}