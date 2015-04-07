using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtbuyer : Sci.Win.UI.TextBox
    {
        private DataTable dtbuyer = new DataTable();
        public txtbuyer()
        {
            
            //Data.DBProxy.Current.Select("", "select id,name from buyer where junk != 1", out dtbuyer);
            //this.d


        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Sci.Win.UI.TextBox TextBox1
        {
            get { return this; }
        }

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string TextBox1Binding
        {
            set { this.Text = value; }
            get { return this.Text; }
        }

    }
}
