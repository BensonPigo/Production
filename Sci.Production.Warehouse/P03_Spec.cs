using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P03_Spec : Win.Tems.QueryForm
    {
        private string poid;
        private string seq1;
        private string seq2;

        /// <inheritdoc/>
        public P03_Spec(string poid, string seq1, string seq2, string refno)
        {
            this.InitializeComponent();
            this.poid = poid;
            this.seq1 = seq1;
            this.seq2 = seq2;
            this.displaySeq.Text = seq1 + "-" + seq2;
            this.displayRefno.Text = refno;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("Name", header: "Spec Column", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("SpecValue", header: "Value", width: Widths.AnsiChars(15), iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.grid2)
                .Text("Name", header: "Spec Column", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("SpecValue", header: "Value", width: Widths.AnsiChars(15), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"
Select	psds.ID
		, psds.Seq1
		, psds.Seq2
		, bt.Name
		, bt.Seq
		, bt.IsInformationSpec
		, psds.SpecValue
from  BomType bt
left join (
	select psds.*
	from PO_Supp_Detail_Spec psds
	where psds.id = '{this.poid}'
			and psds.Seq1 = '{this.seq1}'
			and psds.Seq2 = '{this.seq2}'
) psds on psds.SpecColumnID = bt.ID
where bt.IsInformationSpec = 0
order by bt.Seq
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource1.DataSource = dt;

            sqlcmd = $@"
Select	psds.ID
		, psds.Seq1
		, psds.Seq2
		, bt.Name
		, bt.Seq
		, bt.IsInformationSpec
		, psds.SpecValue
from  BomType bt
left join (
	select psds.*
	from PO_Supp_Detail_Spec psds
	where psds.id = '{this.poid}'
			and psds.Seq1 = '{this.seq1}'
			and psds.Seq2 = '{this.seq2}'
) psds on psds.SpecColumnID = bt.ID
where bt.IsInformationSpec = 1
order by bt.Seq
";
            result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt2);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource2.DataSource = dt2;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
