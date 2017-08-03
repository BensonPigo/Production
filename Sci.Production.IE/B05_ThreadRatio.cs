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
    public partial class B05_ThreadRatio : Sci.Win.Subs.Base
    {
        private string id;
        public B05_ThreadRatio(string ID)
        {
            InitializeComponent();
            id = ID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridArtworkSummary.IsEditingReadOnly = true;
            this.gridArtworkSummary.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.gridArtworkSummary)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("ThreadLocation", header: "Thread Location", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Numeric("UseRatioNumeric", header: "UseRatioNumeric", decimal_places: 2, iseditingreadonly: true)
                .Numeric("Allowance", header: "Start End Loss", decimal_places: 2, iseditingreadonly: true);

            string sqlCmd = string.Format("select * from MachineType_ThreadRatio where ID='{0}'", id);
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query ThreadRatio fail\r\n" + result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
        }
    }
}
