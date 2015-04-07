using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;

namespace Sci.Production.Class
{
    public partial class txtshipmode : Sci.Win.UI.ComboBox
    {
        public txtshipmode()
        {
            string sqlCMD = "Select ID, Name From ShipMode Order By ID";
            Ict.DualResult cbResult;
            DataTable ShipmentTable = new DataTable();
            if (cbResult = DBProxy.Current.Select(null, sqlCMD, out ShipmentTable))
            {
                this.DataSource = ShipmentTable;
                this.DisplayMember = "ID";
                this.ValueMember = "ID";
                this.Size = new System.Drawing.Size(80, 24);
            }
        }
    }
}
