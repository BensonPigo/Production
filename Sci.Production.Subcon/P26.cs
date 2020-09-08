using Ict;
using Ict.Win;
using Sci.Data;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P26 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.lbl_queryfor.Text = "Sub Process";
            string querySql = @"select '' union select Id from SubProcess where IsRFIDProcess=1";
            DBProxy.Current.Select(null, querySql, out DataTable queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("StartProcess = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("BundleNo", header: "Bundle#", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13))
                .Text("SubprocessId", header: "Artwork", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Patterncode", header: "PTN Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("PatternDesc", header: "PTN Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ReceiveName", header: "Recv. Name", width: Widths.AnsiChars(12))
                .DateTime("ReceiveDate", header: "Recv. Date", width: Widths.AnsiChars(20));
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select 
	BTD.BundleNo
	,BTD.orderid
    ,OrderIDs = dbo.GetSinglelineSP((select distinct OrderID from Bundle_Detail_Order where BundleNo = bd.BundleNo order by OrderID for XML RAW))
	,S.SubprocessId
	,BD.Patterncode
	,BD.PatternDesc
	,BTD.ReceiveName
	,BTD.ReceiveDate
from BundleTrack_detail BTD 
LEFT JOIN Bundle_Detail BD ON BD.BundleNo = BTD.BundleNo
OUTER APPLY(
	SELECT SubprocessId = STUFF((
		SELECT CONCAT('+',SubprocessId )
		FROM Bundle_Detail_Art BDA
		WHERE BDA.Bundleno = BTD.BundleNo
		FOR XML PATH('')
	),1,1,'')
)S
WHERE BTD.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool ClickNew()
        {
            var frm = new P26_ImportBarcode();
            frm.ShowDialog(this);
            this.ReloadDatas();
            return true;
        }
    }
}
