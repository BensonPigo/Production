using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class B10 : Sci.Win.Tems.Input7
    {
        private DataTable Dt;

        public B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Numeric("Seq", header: "Seq", width: Widths.AnsiChars(4), maximum: 255)
            .Text("ID", header: "SubProcess", width: Widths.AnsiChars(24), iseditingreadonly: true)
            .Text("artworktypeid", header: "Artwork Type", width: Widths.AnsiChars(35), iseditingreadonly: true)
            ;

            for (int i = 0; i < this.grid1.ColumnCount; i++)
            {
                this.grid1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.Querry();
        }

        private void Querry()
        {
            string sqlcmd = $@"
SELECT s.seq, 
       s.id, 
       s.artworktypeid
FROM   subprocess s WITH (nolock) 
WHERE  s.junk = 0 
       AND s.isselection = 1
ORDER  BY s.seq ASC
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.Dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = this.Dt;
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.grid1 != null)
            {
                if (this.EditMode)
                {
                    this.grid1.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;
                    this.grid1.IsEditingReadOnly = false;
                }
                else
                {
                    this.grid1.Columns["Seq"].DefaultCellStyle.BackColor = Color.White;
                    this.grid1.IsEditingReadOnly = true;
                }

                this.Querry();
            }
        }

        protected override DualResult ClickSave()
        {
            // ProcessWithDatatable無法解析tinyint, 先換成int
            DataTable dtCloned = this.Dt.Clone();
            dtCloned.Columns[0].DataType = typeof(int);
            foreach (DataRow row in this.Dt.Rows)
            {
                dtCloned.ImportRow(row);
            }

            string sqlcmd = $@"
update s set s.seq  = t.seq
from subprocess s
inner join #tmp t on t.ID = s.ID
where s.seq <> t.seq
";
            DataTable dt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtCloned, string.Empty, sqlcmd, out dt);
            if (!result)
            {
                return result;
            }

            return base.ClickSave();
        }

        private void PictureBoxup_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.UpDataGridView(this.grid1);
            }
        }

        private void PictureBoxdown_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.DownDataGridView(this.grid1);
            }
        }

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="dataGridView"></param>
        public void UpDataGridView(Win.UI.Grid dataGridView)
        {
            try
            {
                this.grid1.ValidateControl();
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;
                    if (index > 0)
                    {
                        // 交換Seq值
                        decimal seqOri = MyUtility.Convert.GetDecimal(this.Dt.Rows[index]["Seq"]);
                        decimal seqTo = MyUtility.Convert.GetDecimal(this.Dt.Rows[index - 1]["Seq"]);
                        this.Dt.Rows[index]["Seq"] = seqTo;
                        this.Dt.Rows[index - 1]["Seq"] = seqOri;
                        this.Dt.AcceptChanges();

                        // 交換整筆row
                        DataRow newdata = this.Dt.NewRow();
                        newdata.ItemArray = this.Dt.Rows[index].ItemArray;
                        this.Dt.Rows.RemoveAt(index);
                        this.Dt.Rows.InsertAt(newdata, index - 1);
                        this.Dt.AcceptChanges();
                        dataGridView.Rows[index - 1].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="dataGridView"></param>
        public void DownDataGridView(Win.UI.Grid dataGridView)
        {
            try
            {
                this.grid1.ValidateControl();
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;
                    if (index >= 0 & (dataGridView.RowCount - 1) != index)
                    {
                        // 交換Seq值
                        decimal seqOri = MyUtility.Convert.GetDecimal(this.Dt.Rows[index]["Seq"]);
                        decimal seqTo = MyUtility.Convert.GetDecimal(this.Dt.Rows[index + 1]["Seq"]);
                        this.Dt.Rows[index]["Seq"] = seqTo;
                        this.Dt.Rows[index + 1]["Seq"] = seqOri;
                        this.Dt.AcceptChanges();

                        // 交換整筆row
                        DataRow newdata = this.Dt.NewRow();
                        newdata.ItemArray = this.Dt.Rows[index].ItemArray;
                        this.Dt.Rows.RemoveAt(index);
                        this.Dt.Rows.InsertAt(newdata, index + 1);
                        this.Dt.AcceptChanges();
                        dataGridView.Rows[index + 1].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
