using Ict.Win;
using System;
using System.Data;
using System.Linq;

namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class JustGrid : Sci.Win.Tems.QueryForm
    {
        private DataTable data = new DataTable();

        /// <summary>
        /// 建立單純顯示用的Grid
        /// </summary>
        /// <param name="formTitle">form標題</param>
        /// <param name="dt">Datatable</param>
        /// <param name="formWidth">視窗寬度</param>
        public JustGrid(string formTitle, DataTable dt, int formWidth = 580)
        {
            this.InitializeComponent();
            this.data = dt;
            this.Text = formTitle;
            this.Width = formWidth;
            this.bindingSource1.DataSource = this.data;
            this.SetupGrid();
        }

        /// <summary>
        /// 依照傳入的Dttable column建立Grid
        /// </summary>
        public void SetupGrid()
        {
            foreach (DataColumn dc in this.data.Columns)
            {
                if (dc.DataType == typeof(long) || dc.DataType == typeof(int))
                {
                    this.Helper.Controls.Grid.Generator(this.grid1)
                        .Numeric(dc.ColumnName, header: dc.ColumnName, width: Widths.Auto(false));
                }
                else if (dc.DataType == typeof(decimal) || dc.DataType == typeof(double))
                {
                    this.Helper.Controls.Grid.Generator(this.grid1)
                        .Numeric(dc.ColumnName, header: dc.ColumnName, decimal_places: 2, width: Widths.Auto(false));
                }
                else if (dc.DataType == typeof(DateTime))
                {
                    this.Helper.Controls.Grid.Generator(this.grid1)
                        .DateTime(dc.ColumnName, header: dc.ColumnName, width: Widths.Auto(false));
                }
                else if (dc.DataType == typeof(bool))
                {
                    this.Helper.Controls.Grid.Generator(this.grid1)
                        .CheckBox(dc.ColumnName, header: dc.ColumnName, width: Widths.Auto(false));
                }
                else
                {
                    var length = this.data.AsEnumerable().Where(o => o[dc.ColumnName] != DBNull.Value).Select(t => t.Field<string>(dc.ColumnName).Length);
                    if (length.Any() && length.Max() > 0)
                    {
                        bool hasNewLine = this.data.AsEnumerable().Where(o => o[dc.ColumnName] != DBNull.Value && o[dc.ColumnName].ToString().Contains(Environment.NewLine)).Any();
                        if (hasNewLine)
                        {
                            this.Helper.Controls.Grid.Generator(this.grid1)
                                .EditText(dc.ColumnName, header: dc.ColumnName, width: Widths.AnsiChars(length.Max()));
                        }
                        else
                        {
                            this.Helper.Controls.Grid.Generator(this.grid1)
                                .Text(dc.ColumnName, header: dc.ColumnName, width: Widths.AnsiChars(length.Max()));
                        }
                    }
                    else
                    {
                        this.Helper.Controls.Grid.Generator(this.grid1)
                            .Text(dc.ColumnName, header: dc.ColumnName, width: Widths.Auto(false));
                    }
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
