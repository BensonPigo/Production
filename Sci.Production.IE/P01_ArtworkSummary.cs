using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01_ArtworkSummary
    /// </summary>
    public partial class P01_ArtworkSummary : Win.Subs.Base
    {
        private string tableName;

        // private DataRow masterData;
        private long id;
        private DataGridViewGeneratorNumericColumnSettings tms = new DataGridViewGeneratorNumericColumnSettings();

        /// <summary>
        /// P01_ArtworkSummary
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="id">ID</param>
        public P01_ArtworkSummary(string tableName, long id)
        {
            this.InitializeComponent();
            this.tableName = tableName;
            this.id = id;

            // masterData = MasterData;
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridArtworkSummary.IsEditingReadOnly = true;
            this.gridArtworkSummary.DataSource = this.listControlBindingSource1;
            this.tms.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            this.Helper.Controls.Grid.Generator(this.gridArtworkSummary)
                .Text("ArtworkTypeID", header: "Artwork", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("TMS", header: "TMS", decimal_places: 4, iseditingreadonly: true, settings: this.tms);

            string sqlCmd = string.Format(
                @"
select isnull(mt.ArtworkTypeID,'') as ArtworkTypeID, sum(td.SMV) as TMS
from {0} td WITH (NOLOCK) 
left join MachineType mt WITH (NOLOCK) on td.MachineTypeID = mt.ID
--LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON MT.ID=ATD.MachineTypeID
where td.ID = {1} --and ATD.ArtworkTypeID !=''
group by mt.ArtworkTypeID",
                this.tableName,
                this.id.ToString());
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Artwork fail\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
