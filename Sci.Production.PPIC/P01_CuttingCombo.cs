using Ict;
using Ict.Win;
using Sci.Data;
using System.Data;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P01_CuttingCombo : Win.Subs.Base
    {
        private readonly string poID;

        /// <inheritdoc/>
        public P01_CuttingCombo(string pOID)
        {
            this.InitializeComponent();
            this.poID = pOID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 設定Grid1的顯示欄位
            this.gridCuttingCombo.IsEditingReadOnly = true;
            this.gridCuttingCombo.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridCuttingCombo)
                .Text("CuttingSP", header: "Cutting SP#", width: Widths.AnsiChars(13))
                .Text("ID", header: "SP#", width: Widths.AnsiChars(15))
                .Date("KPILETA", header: "KPI L/ETA(Material)", width: Widths.AnsiChars(12))
                .Date("PFETA", header: "PF ETA", width: Widths.AnsiChars(12))
                .Date("LETA", header: "SCHD L/ETA", width: Widths.AnsiChars(12))
                .Date("SewETA", header: "Sew. MTL ETA(SP)", width: Widths.AnsiChars(12))
                .Text("Colorway", header: "Colorway", width: Widths.AnsiChars(20));

            string sqlCmd = $@"
select CuttingSP,ID,KPILETA,PFETA,LETA,SewETA,oq.Colorway
from Orders o WITH (NOLOCK)
outer apply(
	select Colorway = STUFF((
		select distinct CONCAT(',', oq.Article)
		from Order_Qty oq
		where oq.ID = o.id and oq.Qty >0 
		order by CONCAT(',', oq.Article)
		for xml path('')
	),1,1,'')
)oq
where POID = '{this.poID}'";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!" + result.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
