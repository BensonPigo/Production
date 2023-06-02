using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public partial class DgToPlace : Win.UI.ComboBox
    {
        /// <inheritdoc/>
        public DgToPlace(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public void SetDefalutIndex()
        {
            DataTable dt;
            DualResult result;
            #region SQL CMD

            string sqlcmd = string.Empty;
            sqlcmd = @"
            select ID = ''
            union
            select distinct ID from WHIssueToPlace where junk = 0
            union
            select distinct ID from SewingLine where Junk = 0";
            #endregion
            result = DBProxy.Current.Select("Production", sqlcmd, out dt);

            if (!result)
            {
                return;
            }

            this.DataSource = dt;
            this.ValueMember = "ID";
            this.DisplayMember = "ID";
        }
    }
}
