using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtMultiSubcon : Sci.Win.UI.TextBox
    {
        [Category("Custom Properties")]
        [Description("串sql條件值使用，會將值用單引號先包起來。例：'G001','G002','G003'")]
        public string Subcons { set; get; }

        public txtMultiSubcon()
        {
            this.Size = new System.Drawing.Size(450, 23);
            this.ReadOnly = true;
            this.IsSupportEditMode = false;
            //this.Text = "";
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Right)
            {
                string sqlcmd = string.Format(@"select id,abb from dbo.LocalSupp ");

                Sci.Win.Tools.SelectItem2 selectSubcons = new Win.Tools.SelectItem2(sqlcmd,
                                "Supp ID,Supp Abb", "13,60", this.Text);
                selectSubcons.Width = 800;
                DialogResult result = selectSubcons.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.Text = selectSubcons.GetSelectedString();
                if (!MyUtility.Check.Empty(this.Text))
                    Subcons = "'" + string.Join("','", selectSubcons.GetSelectedList().ToArray()) + "'";
                else
                    Subcons = "";
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
