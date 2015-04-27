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
    public partial class txtsubprocess : Sci.Win.UI.ComboBox
    {
        public txtsubprocess()
        {
            string sqlCmd = "Select ID, ArtworkTypeId From Subprocess  where junk=0 and IsProcess=1 Order By ID";
            Ict.DualResult cbResult;
            DataTable SubprocessTable = new DataTable();
            if (cbResult = DBProxy.Current.Select(null, sqlCmd, out SubprocessTable))
            {
                this.DataSource = SubprocessTable;
                this.DisplayMember = "ArtworkTypeId";
                this.ValueMember = "ID";
                this.Size = new System.Drawing.Size(20, 80);
            }
        }
    }
}
