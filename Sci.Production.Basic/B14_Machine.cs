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

namespace Sci.Production.Basic
{
    /// <summary>
    /// B14_Machine
    /// </summary>
    public partial class B14_Machine : Sci.Win.Subs.Base
    {
        private DataRow motherData;

        /// <summary>
        /// B14_Machine
        /// </summary>
        /// <param name="data">data</param>
        public B14_Machine(DataRow data)
        {
            this.InitializeComponent();
            this.motherData = data;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            // 撈Grid資料
            string selectCommand = string.Format(
                @"
select ID,Description 
from MachineType MT WITH (NOLOCK) LEFT JOIN Artworktype_Detail ATD WITH (NOLOCK) ON MT.ID=ATD.MachineTypeID
where ATD.ArtworkTypeID = '{0}'",
                this.motherData["ID"].ToString());

            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand, out selectDataTable);
            this.bindingSource1.DataSource = selectDataTable;

            // 設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid1)
                 .Text("ID", header: "ID", width: Widths.AnsiChars(9))
                 .Text("Description", header: "Description", width: Widths.AnsiChars(50));
        }
    }
}
