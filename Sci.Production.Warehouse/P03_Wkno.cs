using System;
using System.Data;
using Ict.Win;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P03_Wkno : Win.Subs.Base
    {
        private DataRow dr;

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public P03_Wkno(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.Text += string.Format(" ({0}-{1}- {2})", this.dr["id"].ToString(),
this.dr["seq1"].ToString(),
this.dr["seq2"].ToString());
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string selectCommand1 = $@"
SELECT   a.id
       , e.Packages
       , a.ETD
	   , a.ETA
	   , a.WhseArrival
	   , b.qty
	   , b.foc
	   , a.vessel
	   , a.ShipModeID
       , a.Blno
	   , con.Container
       , a.CYCFS
       , a.Cbm
FROM Export a WITH (NOLOCK) 
INNER JOIN Export_detail b WITH (NOLOCK)  ON a.ID = b.ID
OUTER APPLY(
	SELECT [Container]=STUFF((

		SELECT DISTINCT ','+ContainerType+ '-' +ContainerNo 
		FROM Export_ShipAdvice_Container c  WITH (NOLOCK)
		WHERE c.Export_DetailUkey = b.Ukey
		FOR XML PATH('')

	),1,1,'')
)con
outer apply (
    select [Packages] = sum(e.Packages)
    from Export e with (nolock) 
    where e.Blno = a.Blno
)e
WHERE b.poid = '{this.dr["id"]}'
And b.seq1 = '{this.dr["Seq1"]}'
And b.seq2 = '{this.dr["Seq2"]}'
";
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out DataTable selectDataTable1);
            if (!selectResult1)
            {
                this.ShowErr(selectCommand1, selectResult1);
                return;
            }

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridWkno.IsEditingReadOnly = true;
            this.gridWkno.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridWkno)
                 .Text("id", header: "WK#", width: Widths.AnsiChars(16))
                 .Numeric("Packages", header: "Packages", width: Widths.AnsiChars(8), integer_places: 5, decimal_places: 0)
                 .Date("ETD", header: "ETD", width: Widths.AnsiChars(11))
                 .Date("ETA", header: "ETA", width: Widths.AnsiChars(11))
                 .Date("WhseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(12))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 6, decimal_places: 4)
                 .Numeric("Foc", header: "FOC", width: Widths.AnsiChars(10), integer_places: 6, decimal_places: 4)
                 .Text("CYCFS", header: "CY/CFS", width: Widths.AnsiChars(8))
                 .Numeric("CBM", header: "CBM", width: Widths.AnsiChars(10), integer_places: 10, decimal_places: 3)
                 .Text("Vessel", header: "Vessel Name", width: Widths.AnsiChars(20))
                 .Text("Shipmodeid", header: "Ship Mode", width: Widths.AnsiChars(6))
                 .Text("Blno", header: "B/L No.", width: Widths.AnsiChars(20))
                 .Text("Container", header: "ContainerType & No", width: Widths.AnsiChars(20));
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
