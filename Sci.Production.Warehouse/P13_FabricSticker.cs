using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P13_FabricSticker : Win.Subs.Base
    {
        private object strSubTransferID;

        public P13_FabricSticker(object strSubTransferID)
        {
            this.InitializeComponent();
            this.strSubTransferID = strSubTransferID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Set Grid Columns
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0)
                .Text("SPNo", header: "SP#", iseditingreadonly: true)
                .Text("Seq", header: "Seq#", iseditingreadonly: true)
                .Text("Refno", header: "Refno", iseditingreadonly: true)
                .Text("Color", header: "Color", iseditingreadonly: true)
                .Text("Roll", header: "Roll", iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true)
                .Text("Qty", header: "Issue Qty", iseditingreadonly: true)
                ;

            for (int i = 0; i < this.grid1.Columns.Count; i++)
            {
                this.grid1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            #endregion

            #region Set Grid Datas
            List<SqlParameter> listSqlParameters = new List<SqlParameter>();
            listSqlParameters.Add(new SqlParameter("@ID", this.strSubTransferID));
            string sqlcmd = @"
select Sel = 0
       , RowNo = Row_Number() over (order by isd.POID, isd.Seq1, isd.Seq2)
       , SPNo = isd.POID
	   , Seq = Concat (isd.Seq1, '-', isd.Seq2)
	   , Refno = isnull (psd.Refno, '')
	   , Color = case when f.MtlTypeID = 'SP THREAD' and ThreadColor.SuppColor is not null 
							THEN iif(ISNULL(ThreadColor.SuppColor,'') = '', '', ThreadColor.SuppColor) 
					  when f.MtlTypeID = 'SP THREAD' and ThreadColor.SuppColor is null 
							THEN psd.SuppColor
				 else  isnull (psd.ColorID, '') end
	   , Roll = isd.Roll
	   , Roll = isd.Roll
	   , Dyelot = isd.Dyelot
	   , Qty = isd.Qty
	   , Style = o.StyleID
       , StockUnit = psd.StockUnit
	   , Location = isnull(dbo.Getlocation(isd.FtyInventoryUkey),'')
from Issue_Detail isd
left join Orders o on o.ID=isd.POID
left join Po_Supp_Detail psd on isd.POID = psd.ID
								and isd.Seq1 = psd.SEQ1
								and isd.Seq2 = psd.SEQ2
left join Fabric f on psd.SCIRefno = f.SCIRefno
left join Color c WITH (NOLOCK) on f.BrandID = c.BrandId and psd.ColorID = c.ID 
outer apply(
			SELECT DISTINCT pp.SuppColor
			FROM po_supp_detail pp
			WHERE pp.ID = psd.StockPOID AND pp.Seq1 = psd.StockSeq1 AND pp.Seq2 = psd.StockSeq2
		)
ThreadColor
where isd.ID = @ID
order by RowNo";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, listSqlParameters, out dtResult);
            this.listControlBindingSource.DataSource = dtResult;

            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            #endregion
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DualResult result;
            DataTable dtPrint = (DataTable)this.listControlBindingSource.DataSource;
            if (dtPrint != null
                && dtPrint.AsEnumerable().Any(row => Convert.ToBoolean(row["Sel"])))
            {
                #region Print
                dtPrint = dtPrint.AsEnumerable().Where(row => Convert.ToBoolean(row["Sel"])).CopyToDataTable();

                string strDtSortSQL = @"
select NewRowNo = Row_Number() over (order by RowNo)
       , *
from #tmp
order by NewRowNo";

                result = MyUtility.Tool.ProcessWithDatatable(dtPrint, string.Empty, strDtSortSQL, out dtPrint);

                if (result == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                List<P13_FabricSticker_PrintData> listData = dtPrint.AsEnumerable().Select(row => new P13_FabricSticker_PrintData()
                {
                    RowNo = Convert.ToInt32(row["NewRowNo"]),
                    SPNo = row["SPNo"].ToString().Trim(),
                    Seq = row["Seq"].ToString().Trim(),
                    Refno = row["Refno"].ToString().Trim(),
                    Color = row["Color"].ToString().Trim(),
                    Roll = row["Roll"].ToString().Trim(),
                    Dyelot = row["Dyelot"].ToString().Trim(),
                    Style = row["Style"].ToString().Trim(),
                    Location = row["Location"].ToString().Trim(),
                    StockUnit = row["StockUnit"].ToString().Trim(),
                    Qty = Convert.ToDouble(row["Qty"]),
                }).ToList();

                ReportDefinition report = new ReportDefinition();
                report.ReportDataSource = listData;

                // 指定是哪個 RDLC
                Type reportResourceNamespace = typeof(P13_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P13_FabricSticker_Print.rdlc";

                IReportResource reportresource;

                if ((result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)) == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report);
                frm.MdiParent = this.MdiParent;
                frm.ShowDialog();

                // 關閉視窗
                if (frm.DialogResult == DialogResult.Cancel)
                {
                    this.Close();
                }
                #endregion
            }
            else
            {
                MyUtility.Msg.InfoBox("Select data first.");
            }
        }
    }
}
