using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtartworktype_fty
    /// </summary>
    public partial class Txtartworktype_fty : Win.UI.TextBox
    {
        /// <summary>
        /// Classify 值需用單引號包起來；逗點分格。例如：'I','A','S'或單一個值'P'
        /// </summary>
        [Category("Custom Properties")]
        [Description("Classify 值需用單引號包起來；逗點分格。例如：'I','A','S'或單一個值'P'")]
        public string CClassify { get; set; } = string.Empty;

        /// <summary>
        /// IsSubprocess 填Y或不填
        /// </summary>
        [Category("Custom Properties")]
        [Description("IsSubprocess 填Y或不填")]
        public string CSubprocess { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Txtartworktype_fty"/> class.
        /// </summary>
        public Txtartworktype_fty()
        {
            this.Size = new System.Drawing.Size(140, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "Where Junk = 0";
            string sqlCmd = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.CClassify))
            {
                sqlWhere = sqlWhere + " And Classify in (" + this.CClassify + ")";
            }

            if (!string.IsNullOrWhiteSpace(this.CSubprocess))
            {
                if (this.CSubprocess == "Y")
                {
                    sqlWhere = sqlWhere + " And IsSubprocess =1 ";
                }
                else
                {
                    sqlWhere = sqlWhere + " And IsSubprocess =0 ";
                }
            }

            sqlCmd = "select ID, Abbreviation from ArtworkType WITH (NOLOCK)" + sqlWhere + " order by Seq";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "20,4", this.Text, false, ",");
            item.Size = new System.Drawing.Size(435, 510);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
            base.OnPopUp(e);
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string sqlWhere = string.Format("Where Junk = 0 and id='{0}'", str);
                string sqlCmd = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.CClassify))
                {
                    sqlWhere = sqlWhere + " And Classify in (" + this.CClassify + ")";
                }

                if (!string.IsNullOrWhiteSpace(this.CSubprocess))
                {
                    if (this.CSubprocess == "Y")
                    {
                        sqlWhere = sqlWhere + " And IsSubprocess =1 ";
                    }
                    else
                    {
                        sqlWhere = sqlWhere + " And IsSubprocess =0 ";
                    }
                }

                sqlCmd = "select ID, Abbreviation from ArtworkType WITH (NOLOCK)" + sqlWhere;

                if (MyUtility.Check.Seek(sqlCmd) == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Artworktype : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
