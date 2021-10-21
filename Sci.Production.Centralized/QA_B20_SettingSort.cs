using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class QA_B20_SettingSort : Win.Subs.Base
    {
        /// <inheritdoc/>
        public QA_B20_SettingSort()
        {
            this.InitializeComponent();
            this.ConnectionName = "ProductionTPE";
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            this.EditMode = true;
            this.grid1.IsEditingReadOnly = false;
            base.OnLoad(e);

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Numeric("Seq", header: "Seq", width: Widths.AnsiChars(5), minimum: 1, maximum: 255)
            .Text("ID", header: "Defect Type", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(13), iseditingreadonly: true)
            ;
            this.grid1.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;

            string sql = $@"select * from GarmentDefectType order by seq";
            DBProxy.Current.Select(this.ConnectionName, sql, out DataTable dt);
            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string update = string.Empty;
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w => w.RowState == DataRowState
            .Modified))
            {
                update += $@" update GarmentDefectType set seq = '{dr["seq"]}' , editname = '{Env.User.UserID}',editDate = getdate() where id = '{dr["id"]}'; ";
            }

            DBProxy.Current.Execute(this.ConnectionName, update);
            MyUtility.Msg.InfoBox("Success!");
            this.Close();
        }
    }
}
