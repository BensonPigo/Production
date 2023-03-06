using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <summary>
    /// P01_ShadeBandStock
    /// </summary>
    public partial class P01_ShadeBandStock : Win.Tems.QueryForm
    {
        private string poID;
        private ListControlBindingSource bindShadeboneMain = new ListControlBindingSource();
        private ListControlBindingSource bindShadeboneDetail = new ListControlBindingSource();
        public bool existsData = true;

        /// <summary>
        /// P01_ShadeBandStock
        /// </summary>
        /// <param name="poID">poID</param>
        public P01_ShadeBandStock(string poID)
        {
            this.poID = poID;
            if (!this.Query())
            {
                this.existsData = false;
                return;
            }

            this.InitializeComponent();
            this.gridShadeboneDetail.DataSource = this.bindShadeboneDetail;
            DataTable dtShadebondMain = (DataTable)this.bindShadeboneMain.DataSource;

            this.comboSeqFilter.Items.AddRange(dtShadebondMain.AsEnumerable().Select(s => s["Seq"].ToString()).ToArray());
            this.comboSeqFilter.SelectedIndex = 0;

            this.displayStockFromSP.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "StockPOID", true));
            this.displayStockFromSeq.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "StockSeq", true));
            this.displaySP.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "POID", true));
            this.displayColor.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "ColorID", true));
            this.dateArriveWH.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "WhseArrival", true));
            this.displayWKNo.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "ExportId", true));
            this.txtLocalSupp1.DataBindings.Add(new Binding("TextBox1Binding", this.bindShadeboneMain, "Suppid", true));
            this.displayArriveQty.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "ArriveQty", true));
            this.dateLastInspection.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "ShadeBondDate", true));
            this.displayStyle.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "StyleID", true));
            this.displayRefno.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "Refno", true));
            this.displayResult.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "ShadeBond", true));
            this.displayShadebandInspector.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "ShadeboneInspector", true));
            this.displayBrand.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "BrandId", true));
            this.displaySCIRefno.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "SCIRefno", true));
            this.displaySCIRefnoDesc.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "Description", true));
            this.checkNonShadebond.DataBindings.Add(new Binding("Value", this.bindShadeboneMain, "nonShadebond", true));
            this.txtApprover.DataBindings.Add(new Binding("TextBox1Binding", this.bindShadeboneMain, "Approve", true));
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridShadeboneDetail)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("TicketYds", header: "Ticket Yds", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Scale", header: "Scale", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Result", header: "Result", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Inspdate", header: "Insp.Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Name", header: "Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(25), iseditingreadonly: true);
        }

        private bool Query()
        {
            string sqlQuery = $@"
select  [Seq] = CONCAT(psd.Seq1, '-', psd.Seq2),
        psd.StockPOID,
        [StockSeq] = CONCAT(psd.StockSeq1, '-', psd.StockSeq2),
        [POID] = psd.ID,
        psd.ColorID,
        r.WhseArrival,
        r.ExportId,
        f.Suppid,
        f.ArriveQty,
        f.ShadeBondDate,
        o.StyleID,
        f.Refno,
        f.ShadeBond,
        f.ShadeboneInspector,
        o.BrandID,
        f.SCIRefno,
        f.Approve,
        f.nonShadebond,
        f.ID,
        Fabric.Description
into #tmpShadebondMain
from PO_Supp_Detail psd with (nolock)
inner join FIR f with (nolock) on f.POID = psd.StockPOID and f.SEQ1 = psd.StockSeq1 and f.SEQ2 = psd.StockSeq2 
inner join Fabric with (nolock) on Fabric.SCIRefno = f.SCIRefno
inner join Receiving r with (nolock) on r.id = f.ReceivingID
inner join Orders o with (nolock) on o.ID = psd.ID
where psd.ID = '{this.poID}'

select * from #tmpShadebondMain

select  fs.ID,
        fs.Roll,
        fs.Dyelot,
        fs.TicketYds,
        fs.Scale,
        fs.Result,
        fs.Tone,
        fs.Inspdate,
        fs.Inspector,
        p.Name,
        fs.Remark
from    FIR_Shadebone fs with (nolock)
left join Pass1 p with (nolock) on p.ID =  fs.Inspector
where   fs.ID  in (select ID from #tmpShadebondMain)
order by fs.Roll, fs.Dyelot

drop table #tmpShadebondMain
";

            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null, sqlQuery, out dtResults);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (dtResults[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data found");
                return false;
            }

            DataTable dtShadebondMain = dtResults[0];
            DataTable dtShadebondDetail = dtResults[1];

            this.bindShadeboneMain.DataSource = dtShadebondMain;
            this.bindShadeboneDetail.DataSource = dtShadebondDetail;
            return true;
        }

        private void ComboSeqFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.comboSeqFilter.Text))
            {
                return;
            }

            this.bindShadeboneMain.Filter = $"Seq = '{this.comboSeqFilter.Text}'";
            this.bindShadeboneDetail.Filter = $" ID = '{((DataRowView)this.bindShadeboneMain.Current)["ID"]}'";
            var filterResult = this.bindShadeboneDetail.List.Cast<DataRowView>();

            this.comboDyelotFilter.Items.Clear();
            this.comboDyelotFilter.Items.Add("ALL");

            var filterResultDyelot = filterResult.Select(s => s["Dyelot"].ToString()).Distinct();

            if (filterResultDyelot.Any(s => !MyUtility.Check.Empty(s)))
            {
                this.comboDyelotFilter.Items.AddRange(filterResultDyelot.ToArray());
            }

            this.comboDyelotFilter.SelectedIndex = 0;
        }

        private void ComboDyelotFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.comboDyelotFilter.Text))
            {
                return;
            }

            this.bindShadeboneDetail.Filter = $" ID = '{((DataRowView)this.bindShadeboneMain.Current)["ID"]}'";

            if (this.comboDyelotFilter.Text != "ALL")
            {
                this.bindShadeboneDetail.Filter += $" and Dyelot = '{this.comboDyelotFilter.Text}'";
            }

            this.RefreshDetailSummaryInfo();
        }

        private void RefreshDetailSummaryInfo()
        {
            var filterResult = this.bindShadeboneDetail.List.Cast<DataRowView>();
            this.displayCountRoll.Text = filterResult.Select(s => s["Roll"].ToString()).Distinct().Count().ToString();
            this.displayCountDyelot.Text = filterResult.Select(s => s["Dyelot"].ToString()).Distinct().Count().ToString();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
