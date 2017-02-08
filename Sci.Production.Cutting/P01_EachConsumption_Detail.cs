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

        protected override Ict.DualResult OnRequery(out DataTable datas)
        {
            datas = null;
            string sqlCmd = string.Format(@"
Select Order_EachCons_Color.*, Color.Name as ColorDesc
From dbo.Order_EachCons_Color
Left Join dbo.Orders On Orders.ID = Order_EachCons_Color.ID
Left Join dbo.Color  On Color.BrandId = Orders.BrandID And Color.ID = Order_EachCons_Color.ColorID
Where Order_EachCons_Color.ID = '{0}'
and Order_EachConsUkey='{1}'
and ColorID='{2}'
Order by Order_EachCons_Color.ColorID"
                , this.KeyValue1, this.KeyValue2, this.KeyValue3);
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out datas))) return result;
            return Result.True;
        }
    }
}
