using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtartworktype_fty : Sci.Win.UI.TextBox
    {
        private string m_type = string.Empty;
        private string m_subprocess = string.Empty;

        [Category("Custom Properties")]
        [Description("Classify 值需用單引號包起來；逗點分格。例如：'I','A','S'或單一個值'P'")]
        public string cClassify
        {
            get { return this.m_type; }
            set { this.m_type = value; }
        }

        [Category("Custom Properties")]
        [Description("IsSubprocess 填Y或不填")]
        public string cSubprocess
        {
            get { return this.m_subprocess; }
            set { this.m_subprocess = value; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "Where Junk = 0";
            string sqlCmd = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.cClassify))
            {
                sqlWhere = sqlWhere + " And Classify in (" + this.cClassify + ")";
            }

            if (!string.IsNullOrWhiteSpace(this.cSubprocess))
            {
                if (this.cSubprocess == "Y")
                {
                    sqlWhere = sqlWhere + " And IsSubprocess =1 ";
                }
                else
                {
                    sqlWhere = sqlWhere + " And IsSubprocess =0 ";
                }
            }

            sqlCmd = "select ID, Abbreviation from ArtworkType WITH (NOLOCK)" + sqlWhere + " order by Seq";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "20,4", this.Text, false, ",");
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

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string sqlWhere = string.Format("Where Junk = 0 and id='{0}'", str);
                string sqlCmd = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.cClassify))
                {
                    sqlWhere = sqlWhere + " And Classify in (" + this.cClassify + ")";
                }

                if (!string.IsNullOrWhiteSpace(this.cSubprocess))
                {
                    if (this.cSubprocess == "Y")
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

        public txtartworktype_fty()
        {
            this.Size = new System.Drawing.Size(140, 23);
        }
    }
}
