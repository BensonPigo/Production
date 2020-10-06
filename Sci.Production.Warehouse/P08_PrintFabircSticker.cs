using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P08_PrintFabircSticker : Win.Subs.Base
    {
        private string ID;

        /// <summary>
        /// Initializes a new instance of the <see cref="P08_PrintFabircSticker"/> class.
        /// </summary>
        /// <param name="rID">Receiving_Detail.ID</param>
        public P08_PrintFabircSticker(string rID)
        {
            this.InitializeComponent();
            this.ID = rID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder selectCommand1 = new StringBuilder();
            selectCommand1.Append(string.Format(
                @"
select [Select] = cast(0 as bit)
	, [sPOID] = rd.PoId
	, [sSeq] = CONCAT(rd.seq1, ' ', rd.Seq2)
	, [sRoll] = rd.Roll
	, [sDyelot] = rd.Dyelot
	, [SP] = concat (rd.POID, ' ', rd.Seq1, ' ', rd.Seq2)
	, [Ref] = psd.Refno
	, [Color] =  case 
					when f.MtlTypeID = 'SP THREAD' and ThreadColor.SuppColor is not null 
						THEN iif(ISNULL(ThreadColor.SuppColor,'') = '', '', ThreadColor.SuppColor) 
					when f.MtlTypeID = 'SP THREAD' and ThreadColor.SuppColor is null 
						THEN psd.SuppColor
					else  isnull (psd.ColorID, '') 
				  end
	, [Qty] = rd.StockQty
	, [Roll] = rd.Roll
	, [FabWidth] = f.Width
	, [Dyelot] = rd.Dyelot
	, [CutWidth] = psd.SizeSpec
	, [CutType] = psd.Special
	, [ExpSlice] = ''
	, [ActSlice] = ''
from Receiving_Detail rd
left join Orders o on o.ID=rd.POID
left join Po_Supp_Detail psd on rd.POID = psd.ID
								and rd.Seq1 = psd.SEQ1
								and rd.Seq2 = psd.SEQ2
left join Fabric f on psd.SCIRefno = f.SCIRefno
left join Color c WITH (NOLOCK) on f.BrandID = c.BrandId and psd.ColorID = c.ID 
outer apply(
			SELECT DISTINCT pp.SuppColor
			FROM po_supp_detail pp
			WHERE pp.ID = psd.StockPOID AND pp.Seq1 = psd.StockSeq1 AND pp.Seq2 = psd.StockSeq2
		)
ThreadColor
where rd.id = '{0}'", this.ID));

            DualResult result = DBProxy.Current.Select(null, selectCommand1.ToString(), out DataTable dt);

            if (result == false)
            {
                this.ShowErr(selectCommand1.ToString(), result);
            }

            this.bindingSource1.DataSource = dt;

            // 設定Grid1的顯示欄位
            this.grid.DataSource = this.bindingSource1;
            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
                 .CheckBox("Select", header: string.Empty, width: Widths.AnsiChars(5), iseditable: true, trueValue: true, falseValue: false)
                 .Text("sPOID", header: "SP#", width: Widths.AnsiChars(14), iseditingreadonly: true)
                 .Text("sSeq", header: "SEQ", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("sRoll", header: "Roll#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("sDyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.bindingSource1.DataSource;
            var query = dt.AsEnumerable().Where(x => x.Field<bool>("Select"));
            if (!query.Any())
            {
                this.ShowErr("no data select");
                return;
            }

            List<P08_PrintFabircSticker_PrintData> printData = query.Select((dr, index) => new P08_PrintFabircSticker_PrintData()
            {
                Idx = MyUtility.Convert.GetInt(index + 1),
                SP = dr["SP"].ToString(),
                Ref = dr["Ref"].ToString(),
                Color = dr["Color"].ToString(),
                Qty = MyUtility.Convert.GetInt(MyUtility.Convert.GetDecimal(dr["Qty"])),
                Roll = dr["Roll"].ToString(),
                FabWidth = MyUtility.Convert.GetDecimal(dr["FabWidth"]),
                Dyelot = dr["Dyelot"].ToString(),
                CutWidth = dr["CutWidth"].ToString(),
                CutType = dr["CutType"].ToString(),
                ExpSlice = dr["ExpSlice"].ToString(),
                ActSlice = dr["ActSlice"].ToString(),
            }).ToList();

            ReportDefinition report = new ReportDefinition()
            {
                ReportDataSource = printData,
            };

            Type reportResourceNamespace = typeof(P08_PrintFabircSticker_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P08_PrintFabircSticker.rdlc";
            DualResult result;
            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                this.ShowErr(result);
                return;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }
    }
}
