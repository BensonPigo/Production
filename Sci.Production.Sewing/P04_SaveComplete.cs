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
    public partial class P04_SaveComplete : Sci.Win.Tems.QueryForm
    {
        public P04_SaveComplete(DataTable dtSaveComplete)
        {
            InitializeComponent();
            this.listControlBindingSource1.DataSource = dtSaveComplete;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridSaveComplete)
                .Text("ID", header: "SP", iseditingreadonly: true)
                .Text("OrderIDFrom", header: "From SP", iseditingreadonly: true)
                .Text("StyleLocation", header: "*", iseditingreadonly: true)
                .Text("Article", header: "Article", iseditingreadonly: true)
                .Text("SizeCode", header: "Size", iseditingreadonly: true)
                .Text("ToSPExcess", header: "RemoveQty", iseditingreadonly: true);

        

            for (int i = 0; i < this.gridSaveComplete.Columns.Count; i++) {
                this.gridSaveComplete.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
