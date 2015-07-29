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
        private string useFunction;
        [Category("Custom Properties")]
        public string UseFunction
        {
            set 
            {
                this.useFunction = value;
                if (!Env.DesignTime)
                {
                    string sqlCMD;
                    if (MyUtility.Check.Empty(this.useFunction))
                    {
                        sqlCMD = "select ID from ShipMode order by ID";
                    }
                    else
                    {
                        sqlCMD = string.Format("select ID from ShipMode where UseFunction like '%{0}%' <> 0", useFunction.Trim());
                    }
                    Ict.DualResult cbResult;
                    DataTable ShipModeTable = new DataTable();
                    if (cbResult = DBProxy.Current.Select(null, sqlCMD, out ShipModeTable))
                    {
                        this.DataSource = ShipModeTable;
                        this.DisplayMember = "ID";
                        this.ValueMember = "ID";
                    }
                }
            }
            get { return this.useFunction; }
        }

        public txtshipmode()
        {
            this.Size = new System.Drawing.Size(80, 24);
        }
    }
}
