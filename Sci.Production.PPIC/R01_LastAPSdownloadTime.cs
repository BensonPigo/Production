using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R01_LastAPSdownloadTime
    /// </summary>
    public partial class R01_LastAPSdownloadTime : Sci.Win.Forms.Base
    {
        /// <summary>
        /// R01_LastAPSdownloadTime
        /// </summary>
        public R01_LastAPSdownloadTime()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnPreFormLoaded
        /// </summary>
        protected override void OnPreFormLoaded()
        {
            base.OnPreFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridLastDownloadAPSDate)
               .Text("ID", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .DateTime("LastDownloadAPSDate", header: "Last APS Download Time", width: Widths.AnsiChars(15), iseditingreadonly: true);

            string sqlGetData = $@"
select ID,LastDownloadAPSDate
from Factory
where   UseAPS = 1 and
        Junk = 0
order by ID";

            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridLastDownloadAPSDate.DataSource = dtResult;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
