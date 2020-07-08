using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B48 : Sci.Win.Tems.Input1
    {
        public B48(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "Type ='AQ'";
        }
    }
}
