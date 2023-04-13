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
    /// <summary>
    /// ComboShift
    /// </summary>
    public partial class ComboShift : Win.UI.ComboBox
    {

        /// <summary>
        /// ComboShift
        /// </summary>
        public ComboShift()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Set ComboBox Data
        /// </summary>
        public void SetDataSource()
        {
            DataTable dt;
            DualResult result;
            #region SQL CMD

            string sqlcmd = string.Empty;

            sqlcmd = @"SELECT '' AS SHIFT
                     UNION ALL
                     select SHIFT from shift
                     GROUP BY SHIFT
                     ";
            #endregion
            result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out dt);
            if (result)
            {
                this.DataSource = dt;
                this.ValueMember = "SHIFT";
                this.DisplayMember = "SHIFT";
            }
        }
    }
}
