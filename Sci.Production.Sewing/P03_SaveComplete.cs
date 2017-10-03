using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class P03_SaveComplete : Sci.Win.Tems.QueryForm
    {
        public P03_SaveComplete(DataTable dtSaveComplete, DataTable dtCheckPacking)
        {
            InitializeComponent();
            this.listControlBindingSource1.DataSource = dtSaveComplete;
            this.listControlBindingSource2.DataSource = dtCheckPacking;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridSaveComplete)
                .Text("OrderID", header: "SP", iseditingreadonly: true)
                .Text("POID", header: "FromSP", iseditingreadonly: true)
                .Text("ComboType", header: "*", iseditingreadonly: true)
                .Text("Article", header: "Color Way", iseditingreadonly: true)
                .Text("SizeCode", header: "Size", iseditingreadonly: true)
                .Text("CanReceiveQty", header: "AssignQty", iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridCheckPacking)
                .Text("OrderID", header: "SP", iseditingreadonly: true)
                .Text("POID", header: "FromSP", iseditingreadonly: true)
                .Text("ComboType", header: "*", iseditingreadonly: true)
                .Text("Article", header: "Color Way", iseditingreadonly: true)
                .Text("SizeCode", header: "Size", iseditingreadonly: true)
                .Text("ToSPQty", header: "ToSPQty", iseditingreadonly: true)
                .Text("PackingLockQty", header: "PackingOverQty", iseditingreadonly: true);

            for (int i = 0; i < this.gridSaveComplete.Columns.Count; i++) {
                this.gridSaveComplete.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            for(int i = 0; i < this.gridCheckPacking.Columns.Count; i++) {
                this.gridCheckPacking.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
