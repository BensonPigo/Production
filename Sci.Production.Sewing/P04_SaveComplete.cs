using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P04_SaveComplete
    /// </summary>
    public partial class P04_SaveComplete : Sci.Win.Tems.QueryForm
    {
        /// <summary>
        /// P04_SaveComplete
        /// </summary>
        /// <param name="dtSaveComplete">dtSaveComplete</param>
        public P04_SaveComplete(DataTable dtSaveComplete)
        {
            this.InitializeComponent();
            this.listControlBindingSource1.DataSource = dtSaveComplete;
        }

        /// <inheritdoc/>
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
