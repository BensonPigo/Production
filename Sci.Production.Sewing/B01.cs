using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type = 'SO'");
        }

        protected override void OnDetailEntered()
        {
            this.txtID.ReadOnly = true;
            this.txtDescription.ReadOnly = true;
            this.chkJunk.ReadOnly = true;
            base.OnDetailEntered();
        }
    }
}
