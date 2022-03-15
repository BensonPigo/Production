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
        s.refno,
        [Color] = Color.Value,
        s.SizeSpec,
        [Packages] = @Packages,
        r.Remark,
        [StickerQty] = 0
 from dbo.Receiving_Detail r WITH (NOLOCK) 
 left join dbo.PO_Supp_Detail s WITH (NOLOCK) on s.id=r.POID and s.SEQ1=r.Seq1 and s.SEQ2=r.seq2
 left join Fabric f WITH (NOLOCK) ON s.SCIRefNo=f.SCIRefNo
 left join dbo.po p WITH (NOLOCK)  on  p.id = r.poid
 left join dbo.View_WH_Orders o WITH (NOLOCK) on o.id = r.PoId
 left join dbo.Receiving rec WITH (NOLOCK) on rec.id = r.ID
 OUTER APPLY(
  SELECT [Value]=
 	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(s.SuppColor = '' or s.SuppColor is null, dbo.GetColorMultipleID(o.BrandID,s.ColorID), s.SuppColor)
 		 ELSE dbo.GetColorMultipleID(o.BrandID,s.ColorID)
 	 END
 ) Color
 where r.id= @ID and s.FabricType = 'A'";
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
                // this.ShowException(result);
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
    }
}
