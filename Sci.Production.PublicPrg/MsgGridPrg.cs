using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Linq;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public partial class MsgGridPrg : Sci.Win.Tems.QueryForm
    {
        private DataTable data = new DataTable();

        /// <inheritdoc/>
        public MsgGridPrg(DataTable dt, string msg)
        {
            this.InitializeComponent();
            this.data = dt;
            this.labMsg.Text = msg;
            this.listControlBindingSource1.DataSource = this.data;
            this.SetupGrid();
        }

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

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
