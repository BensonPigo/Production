using System.Data;
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
            this.InitializeComponent();
        }

        /// <summary>
        /// Set ComboBox Data
        /// </summary>
        public void setDataSource(bool isForReport = true)
        {
            DataTable dtRFIDProcessLocation;
            DualResult result;
            #region SQL CMD
            string whereIncludeJunk = " where Junk = 0 ";
            if (this.includeJunk)
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
