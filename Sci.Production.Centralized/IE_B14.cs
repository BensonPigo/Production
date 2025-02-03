using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B14
    /// </summary>
    public partial class IE_B14 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// IE_B14
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public IE_B14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Insert.Visible = false;
            this.gridicon.Remove.Visible = false;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string pass1Source = DBProxy.Current.DefaultModuleName.Contains("testing") ? "Production.dbo.pass1" : "trade.dbo.pass1";
            this.DetailSelectCommand = string.Format(
                @"
select  i.*,
        [AddNameDesc] = iif(i.AddName = '', '', (i.AddName + '-' + p1.Name)),
        [EditNameDesc] = iif(i.EditName = '', '', (i.EditName + '-' + p2.Name))
from ProductionTPE.dbo.IEReason i with (nolock)
left join {1} p1 with (nolock) on i.AddName = p1.ID
left join {1} p2 with (nolock) on i.EditName = p2.ID
where   i.Type = '{0}'
", masterID,
                pass1Source);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ID", header: "ID", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: false)
                .CheckBox("Junk", header: "Junk", width: Widths.AnsiChars(5))
                .Text("AddNameDesc", header: "Add Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("EditNameDesc", header: "Edit Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                ;

            this.detailgrid.RowsAdded += this.Detailgrid_RowsAdded;

            this.detailgrid.Columns["Junk"].DefaultCellStyle.BackColor = Color.Pink;

            this.detailgrid.EditingControlShowing += this.DataGridView_EditingControlShowing;
        }

        private void DataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var dataGridView = sender as DataGridView;
            if (dataGridView == null)
            {
                return;
            }

            var textBox = e.Control as TextBox;
            if (this.CurrentMaintain["ID"].ToString() != "LL")
            {
                if (textBox != null)
                {
                    textBox.KeyPress -= this.TextBox_False_KeyPress;
                    textBox.KeyPress += this.TextBox_False_KeyPress;
                }

                return;
            }

            if (dataGridView.CurrentCell.ColumnIndex == 1)
            {
                if (textBox != null)
                {
                    textBox.KeyPress -= this.TextBox_KeyPress;
                    textBox.KeyPress += this.TextBox_KeyPress;
                }
            }
            else
            {
                if (textBox != null)
                {
                    textBox.KeyPress -= this.TextBox_False_KeyPress;
                    textBox.KeyPress += this.TextBox_False_KeyPress;
                }
            }
        }

        private void TextBox_False_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.createby.Text = this.CurrentMaintain["AddDate"].ToString();
            this.editby.Text = this.CurrentMaintain["EditDate"].ToString();
        }

        private void Detailgrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataRow sourceRow = this.detailgrid.GetDataRow(e.RowIndex);

            if (sourceRow.RowState == DataRowState.Added)
            {
                this.detailgrid.Rows[e.RowIndex].Cells["Description"].ReadOnly = false;
                this.detailgrid.Rows[e.RowIndex].Cells["Description"].Style.BackColor = Color.Pink;
            }
            else
            {
                this.detailgrid.Rows[e.RowIndex].Cells["Description"].ReadOnly = true;
                this.detailgrid.Rows[e.RowIndex].Cells["Description"].Style.BackColor = Color.White;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            bool isDetailDescEmpty = this.DetailDatas.Any(s => MyUtility.Check.Empty(s["Description"]));

            if (isDetailDescEmpty)
            {
                MyUtility.Msg.WarningBox("[Description] cannot be empty.");
                return false;
            }

            // 新增的資料取號
            int startNewID = this.DetailDatas.Any(s => MyUtility.Check.Empty(s["ID"])) ? this.DetailDatas.Select(s => MyUtility.Convert.GetInt(s["ID"])).Max() + 1 : 1;
            foreach (DataRow drNew in this.DetailDatas.Where(s => MyUtility.Check.Empty(s["ID"])))
            {
                drNew["ID"] = startNewID.ToString().PadLeft(5, '0');
                startNewID++;
            }

            return base.ClickSaveBefore();
        }
    }
}
