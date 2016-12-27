using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.IE
{
    public partial class P01_ArtworkSummary : Sci.Win.Subs.Base
    {
        private string tableName;
        //private DataRow masterData;
        private long id;
        DataGridViewGeneratorNumericColumnSettings tms = new DataGridViewGeneratorNumericColumnSettings();
        public P01_ArtworkSummary(string TableName, long ID)
        {
            InitializeComponent();
            tableName = TableName;
            id = ID;
            //masterData = MasterData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            tms.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("ArtworkTypeID", header: "Artwork", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("TMS", header: "TMS", decimal_places: 4, iseditingreadonly: true, settings: tms);

            string sqlCmd = string.Format(@"select isnull(mt.ArtworkTypeID,'') as ArtworkTypeID, sum(td.SMV) as TMS
from {0} td
left join MachineType mt on td.MachineTypeID = mt.ID
where td.ID = {1} and mt.ArtworkTypeID !=''
group by mt.ArtworkTypeID", tableName, id.ToString());
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Artwork fail\r\n" + result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
        }
    }
}
