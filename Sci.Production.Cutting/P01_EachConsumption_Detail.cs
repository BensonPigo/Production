using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Cutting
{
    public partial class P01_EachConsumption_Detail : Sci.Win.Subs.Input4Plus
    {
        public P01_EachConsumption_Detail()
        {
            InitializeComponent();
        }

        public P01_EachConsumption_Detail(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
        }

        protected override Ict.DualResult OnRequery(out DataTable datas)
        {
            datas = null;
            string sqlCmd = "Select Order_EachCons_Color.*, Color.Name as ColorDesc" +
                            "  From dbo.Order_EachCons_Color" +
                            "  Left Join dbo.Orders" +
                            "    On Orders.ID = Order_EachCons_Color.ID" +
                            "  Left Join dbo.Color" +
                            "    On     Color.BrandId = Orders.BrandID" +
                            "       And Color.ID = Order_EachCons_Color.ColorID" +
                            " Where Order_EachCons_Color.ID = '" + this.KeyValue1 + "'" +
                            " Order by Order_EachCons_Color.ColorID";
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out datas))) return result;
            return Result.True;
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("ColorID", header: "Color ID", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("ColorDesc", header: "Color Description", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("SizeList", header: "Ratio", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.Numeric(6), maximum: 999999, minimum: 0, decimal_places: 0)
                .Numeric("Layer", header: "Layer", width: Widths.Numeric(5), maximum: 99999, minimum: 0, decimal_places: 0)
                .Numeric("CutQty", header: "Cut Q'ty", width: Widths.Numeric(6), maximum: 999999, minimum: 0, decimal_places: 0)
                .Numeric("Variance", header: "Variance", width: Widths.Numeric(6), maximum: 999999, minimum: 0, decimal_places: 0)
                .Numeric("YDS", header: "Cons.(YDS)", width: Widths.Numeric(6), maximum: 9999, minimum: 0, decimal_places: 2);

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Article", header: "Article#", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.Numeric(6), maximum: 999999, minimum: 0, decimal_places: 0)
                .Numeric("Layer", header: "Layer", width: Widths.Numeric(5), maximum: 99999, minimum: 0, decimal_places: 0)
                .Numeric("CutQty", header: "Cut Q'ty", width: Widths.Numeric(6), maximum: 999999, minimum: 0, decimal_places: 0)
                .Numeric("Variance", header: "Variance", width: Widths.Numeric(6), maximum: 999999, minimum: 0, decimal_places: 0);

            return true;
        }

        private void btnOrderBy_Click(object sender, EventArgs e)
        {
            if (this.btnOrderBy.Text == "Order by Size,Article")
            {
                this.DefaultDetailOrder = "SizeCode, Article";
                this.btnOrderBy.Text = "Order by Article,Size";    
            }
            else
            {
                this.DefaultDetailOrder = "Article, SizeCode";
                this.btnOrderBy.Text = "Order by Size,Article";
            }

        }
    }
}
