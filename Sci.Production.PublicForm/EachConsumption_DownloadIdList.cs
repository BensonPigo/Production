using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PublicForm
{
    public partial class EachConsumption_DownloadIdList : Win.Subs.Input4
    {
        public EachConsumption_DownloadIdList()
        {
            this.InitializeComponent();
        }

        public EachConsumption_DownloadIdList(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
        }

        protected override DualResult OnRequery(out DataTable datas)
        {
            datas = null;
            string sqlCmd = string.Format(
                @"
Select ID, MarkerDownloadID, 
FabricCombo = (Select FabricCombo+',' 
                From Order_EachCons as tmp WITH (NOLOCK) 
                Where tmp.ID = Order_EachCons.ID
                And IsNull(tmp.MarkerDownloadID, '') 
                    = IsNull(Order_EachCons.MarkerDownloadID, '')
                Group by FabricCombo Order by FabricCombo for XML path(''))
From Order_EachCons WITH (NOLOCK) 
Where Order_EachCons.ID = '{0}'
Group by ID, MarkerDownloadID", this.KeyValue1);
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out datas)))
            {
                return result;
            }

            return Result.True;
        }

        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("MarkerDownloadID", header: "Marker Download ID", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("FabricCombo", header: "Fabric Combo", width: Widths.AnsiChars(25), iseditingreadonly: true);

            return true;
        }
    }
}
