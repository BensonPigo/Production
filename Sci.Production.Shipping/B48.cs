using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B48 : Win.Tems.Input1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="B48"/> class.
        /// </summary>
        /// <param name="menuitem"></param>
        public B48(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "Type ='AQ'";
        }
    }
}
