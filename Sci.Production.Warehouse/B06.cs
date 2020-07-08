using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class B06 : Sci.Win.Tems.Input1
    {
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = " ReasonTypeID ='Stock_Remove'";
        }
    }
}
