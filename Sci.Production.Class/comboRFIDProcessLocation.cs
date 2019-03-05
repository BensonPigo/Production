using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Class
{
    public partial class comboRFIDProcessLocation : Sci.Win.UI.ComboBox
    {
        private bool includeJunk = false;
        public bool IncludeJunk
        {
            get { return this.includeJunk; }
            set { this.includeJunk = value; }
        }

        public comboRFIDProcessLocation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set ComboBox Data
        /// </summary>
        public void setDataSource()
        {
            DataTable dtRFIDProcessLocation;
            DualResult result;
            #region SQL CMD
            string whereIncludeJunk = " where Junk = 0 ";
            if (this.includeJunk)
            {
                whereIncludeJunk = string.Empty;
            }
            string sqlcmd = @"
                select [ID] = ''
                union
                select ID from RFIDProcessLocation " + whereIncludeJunk;
            #endregion
            result = DBProxy.Current.Select(null, sqlcmd, out dtRFIDProcessLocation);
            if (result)
            {
                this.DataSource = dtRFIDProcessLocation;
                this.ValueMember = "ID";
                this.DisplayMember = "ID";
            }
        }
    }
}
