using System.Data;
using Ict;
using Sci.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// ComboRFIDProcessLocation
    /// </summary>
    public partial class ComboRFIDProcessLocation : Win.UI.ComboBox
    {
        /// <summary>
        /// Include Junk
        /// </summary>
        public bool IncludeJunk { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboRFIDProcessLocation"/> class.
        /// </summary>
        public ComboRFIDProcessLocation()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Set ComboBox Data
        /// </summary>
        /// <param name="isForReport">ALL 選項是否出現</param>
        public void SetDataSource(bool isForReport = true)
        {
            DataTable dtRFIDProcessLocation;
            DualResult result;
            #region SQL CMD
            string whereIncludeJunk = " where Junk = 0 ";
            if (this.IncludeJunk)
            {
                whereIncludeJunk = string.Empty;
            }

            string sqlcmd = string.Empty;
            if (isForReport)
            {
                sqlcmd = @"
                select [ID] = 'ALL'
                union all
				select [ID] = ''
                union all
                select ID from RFIDProcessLocation " + whereIncludeJunk;
            }
            else
            {
                sqlcmd = @"
				select [ID] = ''
                union all
                select ID from RFIDProcessLocation " + whereIncludeJunk;
            }
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
