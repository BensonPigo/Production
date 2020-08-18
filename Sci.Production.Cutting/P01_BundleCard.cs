using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P01_BundleCard : Win.Tems.QueryForm
    {
        private readonly string cutid;
        private readonly string M;

        /// <inheritdoc/>
        public P01_BundleCard(string cID, string m)
        {
            this.InitializeComponent();
            this.cutid = cID;
            this.M = m;
            this.Requery();
            this.GridSetup();
        }

        private void Requery()
        {
            string sqlcmd = $@"
Select
	bd.BundleNo,
	OrderID=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = bd.BundleNo order by OrderID for XML RAW)),
	b.Cdate,
	b.CutRef,
	b.PatternPanel,
	b.Cutno,
	bd.SizeCode,
	bd.BundleGroup,
	bd.Qty,
	bd.PrintDate	
from Bundle b WITH (NOLOCK)
inner join Bundle_Detail bd WITH (NOLOCK) on b.id = bd.Id
inner join Orders o WITH (NOLOCK) on b.Orderid = o.ID and b.MDivisionID = o.MDivisionID
where b.POID = '{this.cutid}' and b.MDivisionID  = '{this.M}'
order by bd.BundleNo
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable gridtb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridBundleCard.DataSource = gridtb;
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.gridBundleCard)
                .Text("BundleNo", header: "Bundle No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Date("Cdate", header: "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CutRef", header: "Cut Ref", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PatternPanel", header: "Comb", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("Cutno", header: "Cut No.", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("BundleGroup", header: "Bundle Group", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .DateTime("PrintDate", header: "PrintDate", width: Widths.AnsiChars(8), iseditingreadonly: true);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
