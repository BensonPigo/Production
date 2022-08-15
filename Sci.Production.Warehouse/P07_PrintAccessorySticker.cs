using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Ict;
using Sci.Data;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P07_PrintAccessorySticker : Win.Tems.QueryForm
    {
        private string exportID;

        /// <inheritdoc/>
        public P07_PrintAccessorySticker(string id, string exportID)
        {
            this.InitializeComponent();
            this.exportID = exportID;
            this.GridSetup();

            DualResult result;
            List<SqlParameter> pars = new List<SqlParameter>();

            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string cmdd = $@"
declare @Packages numeric(6,0)

select @Packages = isnull(sum(Packages), 0)
from Export with (nolock)
where MainExportID = (select MainExportID from Export with (nolock) where ID = '{this.exportID}')

select  r.POID,
        r.Seq1+' '+r.seq2 as SEQ,
        psd.refno,
        [Color] = Color.Value,
        SizeSpec= isnull(psdsS.SpecValue, ''),
        [Packages] = @Packages,
        r.Remark,
        [StickerQty] = 0,
        rec.WhseArrival
from dbo.Receiving_Detail r WITH (NOLOCK) 
left join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.id=r.POID and psd.SEQ1=r.Seq1 and psd.SEQ2=r.seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join Fabric f WITH (NOLOCK) ON psd.SCIRefNo=f.SCIRefNo
left join dbo.po p WITH (NOLOCK)  on  p.id = r.poid
left join dbo.View_WH_Orders o WITH (NOLOCK) on o.id = r.PoId
left join dbo.Receiving rec WITH (NOLOCK) on rec.id = r.ID
OUTER APPLY(
SELECT [Value]=
 	CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(psd.SuppColor = '' or psd.SuppColor is null, dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, '')), psd.SuppColor)
 		ELSE dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, ''))
 	END
) Color
where r.id= @ID and psd.FabricType = 'A'
";
            result = DBProxy.Current.Select(string.Empty, cmdd, pars, out dtDetail);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.gridSticker.DataSource = dtDetail;
            #endregion

        }

        private void GridSetup()
        {
            this.gridSticker.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridSticker)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("refno", header: "Refno", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Numeric("StickerQty", header: "Sticker Qty", width: Widths.AnsiChars(4))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (this.gridSticker.DataSource == null)
            {
                return;
            }

            var printSticker = ((DataTable)this.gridSticker.DataSource).AsEnumerable().Where(s => (int)s["StickerQty"] > 0);

            if (!printSticker.Any())
            {
                MyUtility.Msg.InfoBox("<Sticker Qty> needs to input greater than 0");
                return;
            }

            List<P07_PrintAccessorySticker_Data> p07_PrintAccessories = new List<P07_PrintAccessorySticker_Data>();

            foreach (var drPrintSticker in printSticker)
            {
                for (int i = 0; i < (int)drPrintSticker["StickerQty"]; i++)
                {
                    p07_PrintAccessories.Add(
                        new P07_PrintAccessorySticker_Data
                        {
                            POID = drPrintSticker["POID"].ToString(),
                            SEQ = drPrintSticker["SEQ"].ToString(),
                            Refno = drPrintSticker["Refno"].ToString(),
                            Color = drPrintSticker["Color"].ToString(),
                            SizeSpec = drPrintSticker["SizeSpec"].ToString(),
                            Packages = drPrintSticker["Packages"].ToString(),
                            Remark = drPrintSticker["Remark"].ToString(),
                            ArriveWHDate = MyUtility.Check.Empty(drPrintSticker["WhseArrival"]) ? string.Empty : ((DateTime)drPrintSticker["WhseArrival"]).ToString("yyyy/MM/dd"),
                        });
                }
            }

            // 指定是哪個 RDLC
            DualResult result;
            Type reportResourceNamespace = typeof(P07_PrintAccessorySticker_Data);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P07_PrintAccessorySticker.rdlc";
            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                return;
            }

            ReportDefinition report = new ReportDefinition();
            report.ReportDataSource = p07_PrintAccessories;
            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();
            return;
        }

        private void BtnCleanStickerQty_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in ((DataTable)this.gridSticker.DataSource).Rows)
            {
                row["StickerQty"] = 0;
            }
        }
    }
}
