using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
namespace Sci.Production.Warehouse
{
    public partial class P07_Sticker : Sci.Win.Tems.QueryForm
    {

        BindingList<P07_Sticker_Data> Data = new BindingList<P07_Sticker_Data>();

        public P07_Sticker()
        {
            InitializeComponent();
            this.grid1.DataSource = this.Data;
            this.GridSetup();
        }

        void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("POID", header: "SP #", width: Widths.AnsiChars(14),iseditable:true)
                .Text("PINO", header: "PI #", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("Packages", header: "P'Kgs", width: Widths.AnsiChars(4), iseditingreadonly: false)
                .Text("BoxID", header: "Box No.", width: Widths.AnsiChars(5), iseditingreadonly: false)
                .Text("CarNo", header: "C/No", width: Widths.AnsiChars(9), iseditingreadonly: false)
                .Text("FinishDate", header: "Finish Date", width: Widths.AnsiChars(9), iseditingreadonly: false)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(7), iseditingreadonly: false)
                ;

        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            List<P07_Sticker_Data> tmpData = Data.Where(data => data.selected).ToList();
        }
    }
}
