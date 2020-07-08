using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P03_SaveComplete
    /// </summary>
    public partial class P03_SaveComplete : Sci.Win.Tems.QueryForm
    {
        /// <summary>
        /// P03_SaveComplete
        /// </summary>
        /// <param name="dtSaveComplete">dtSaveComplete</param>
        public P03_SaveComplete(DataTable dtSaveComplete)
        {
            this.InitializeComponent();
            this.listControlBindingSource1.DataSource = dtSaveComplete;
        }

        /// <inheritdoc/>
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

            for (int i = 0; i < this.gridSaveComplete.Columns.Count; i++)
            {
                this.gridSaveComplete.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
