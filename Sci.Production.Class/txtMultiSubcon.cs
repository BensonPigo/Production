using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtMultiSubcon : Win.UI.TextBox
    {
        [Category("Custom Properties")]
        [Description("串sql條件值使用，會將值用單引號先包起來。例：'G001','G002','G003'")]
        public string Subcons { get; set; }

        public txtMultiSubcon()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
            this.IsSupportEditMode = false;

            // this.Text = "";
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Right)
            {
                string sqlcmd = string.Format(@"
select l.id , l.Abb 
from dbo.LocalSupp l WITH (NOLOCK) 
WHERE l.Junk=0
");

                Win.Tools.SelectItem2 selectSubcons = new Win.Tools.SelectItem2(
                    sqlcmd,
                    "Supp ID,Supp Abb", "10,15", this.Text, null, null, null);
                selectSubcons.Width = 410;
                DialogResult result = selectSubcons.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = selectSubcons.GetSelectedString();
                if (!MyUtility.Check.Empty(this.Text))
                {
                    this.Subcons = "'" + string.Join("','", selectSubcons.GetSelectedList().ToArray()) + "'";
                }
                else
                {
                    this.Subcons = string.Empty;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
