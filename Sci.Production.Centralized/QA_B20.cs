using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class QA_B20 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public QA_B20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.ConnectionName = "ProductionTPE";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorNumericColumnSettings seq = new DataGridViewGeneratorNumericColumnSettings();
            seq.CellEditable += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                if (!this.IsDetailInserting && !MyUtility.Check.Empty(dr["ID"]))
                {
                    e.IsEditable = false;
                }
            };

            this.btnSettingSort.Enabled = this.Perm.Edit;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Numeric("Seq", header: "Seq", width: Widths.AnsiChars(5), minimum: 1, maximum: 255)
            .Text("ID", header: "Defect Code", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("LocalDescription", header: "Local Desc", width: Widths.AnsiChars(20), iseditingreadonly: true)
            ;
            this.grid1.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.grid1 != null)
            {
                if (this.EditMode)
                {
                    this.grid1.IsEditingReadOnly = false;
                }
                else
                {
                    this.grid1.IsEditingReadOnly = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sql = $@"select * from GarmentDefectCode where GarmentDefectTypeID  = '{this.CurrentMaintain["ID"]}' order by seq";
            DBProxy.Current.Select(this.ConnectionName, sql, out DataTable dt);
            this.listControlBindingSource1.Position = -1;
            this.listControlBindingSource1.DataSource = dt;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.checkJunk.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtDefectType.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtDefectType.Text))
            {
                this.txtDefectType.Focus();
                MyUtility.Msg.WarningBox("<Defect Type> cannot be empty! ");
                return false;
            }

            if (this.IsDetailInserting)
            {
                int seq = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup("select max(seq) from GarmentDefectType"));
                seq++;
                if (seq > 255)
                {
                    seq = 255;
                }

                this.CurrentMaintain["seq"] = seq;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            string update = string.Empty;
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w => w.RowState == DataRowState
            .Modified))
            {
                update += $@" update GarmentDefectCode set seq = '{dr["seq"]}' , editname = '{Env.User.UserID}',editDate = getdate() where  GarmentDefectTypeID  = '{this.CurrentMaintain["ID"]}' and id = '{dr["id"]}'; ";
            }

            DBProxy.Current.Execute(this.ConnectionName, update);
            return base.ClickSavePost();
        }

        private void BtnSettingSort_Click(object sender, EventArgs e)
        {
            QA_B20_SettingSort f = new QA_B20_SettingSort();
            f.ShowDialog();
            this.ReloadDatas();
        }
    }
}
