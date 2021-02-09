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
    /// <summary>
    /// P19_FabricSticker
    /// </summary>
    public partial class P19_FabricSticker : Win.Subs.Base
    {
        private object strSubTransferID;
        private string remark;

        /// <summary>
        /// P19_FabricSticker
        /// </summary>
        /// <param name="strSubTransferID">strSubTransferID</param>
        /// <param name="remark">remark</param>
        public P19_FabricSticker(object strSubTransferID, string remark)
        {
            this.InitializeComponent();
            this.strSubTransferID = strSubTransferID;
            this.remark = remark;
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
                .Text("Refno", header: "RefNo", iseditingreadonly: true)
                .Text("Color", header: "Color", iseditingreadonly: true)
                .Text("Roll", header: "Roll", iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", iseditingreadonly: true)
                .Numeric("StockQty", header: "Stock Qty", iseditingreadonly: true)
                .Text("StockType", header: "Stock Type", iseditingreadonly: true)
                .Text("FtyLocation", header: "Location", iseditingreadonly: true)
                ;

            for (int i = 0; i < this.grid1.Columns.Count; i++)
            {
                this.grid1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            #endregion

            #region Set Grid Datas
            List<SqlParameter> listSqlParameters = new List<SqlParameter>();
            listSqlParameters.Add(new SqlParameter("@ID", this.strSubTransferID));

            string strQuerySql = @"
select Sel = 0
        , RowNo = Row_Number() over (order by trsd.POID, trsd.Seq1, trsd.Seq2)
        , SPNo = trsd.POID
	    , Seq = Concat (trsd.Seq1, '-', trsd.Seq2)
	    , Refno = psd.Refno
	    , Color = psd.ColorID
	    , Roll = trsd.Roll
	    , Dyelot = trsd.Dyelot
	    , ToFactory = trs.ToMDivisionId      
	    , StockUnit = psd.StockUnit
	    , location = dbo.Getlocation(trsd.FtyInventoryUkey)
	    , Qty = trsd.Qty
        , [StockQty] = (isnull(fi.InQty, 0) - isnull(fi.OutQty, 0) + isnull(fi.AdjustQty, 0) - isnull(fi.ReturnQty, 0))
        , [StockType] = case    when trsd.StockType = 'I' then 'Inventory'
                                when trsd.StockType = 'B' then 'Bulk'
                                else trsd.StockType end
        , [FtyLocation] = FtyLocation.val
from TransferOut_Detail trsd
left join TransferOut trs on trs.Id=trsd.ID
left join Po_Supp_Detail psd on trsd.POID = psd.ID
								and trsd.Seq1 = psd.SEQ1
								and trsd.Seq2 = psd.SEQ2
left join FtyInventory fi with (nolock) on  fi.POID = trsd.POID and 
                                            fi.Seq1 = trsd.Seq1 and 
                                            fi.Seq2 = trsd.Seq2 and 
                                            fi.Roll = trsd.Roll and
                                            fi.Dyelot = trsd.Dyelot and
                                            fi.StockType = trsd.StockType
outer apply(
            SELECT [val] =  Stuff((select concat( ',', fid.MtlLocationID) 
                                    from FtyInventory_Detail fid with (nolock) 
                                    where fid.Ukey = fi.Ukey
                                    FOR XML PATH('')),1,1,'') ) FtyLocation
where trsd.ID = @ID
order by RowNo";

            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(string.Empty, strQuerySql, listSqlParameters, out dtResult);
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

                List<P19_FabricSticker_PrintData> listData = dtPrint.AsEnumerable().Select(row => new P19_FabricSticker_PrintData()
                {
                    RowNo = Convert.ToInt32(row["NewRowNo"]),
                    SPNo = row["SPNo"].ToString().Trim(),
                    Seq = row["Seq"].ToString().Trim(),
                    Refno = row["Refno"].ToString().Trim(),
                    Roll = row["Roll"].ToString().Trim(),
                    Dyelot = row["Dyelot"].ToString().Trim(),
                    ToFactory = row["ToFactory"].ToString().Trim(),
                    Location = row["location"].ToString().Trim(),
                    Color = row["Color"].ToString().Trim(),
                    StockUnit = row["StockUnit"].ToString().Trim(),
                    Qty = Convert.ToDouble(row["Qty"]),
                    Remark = this.remark,
                }).ToList();

                ReportDefinition report = new ReportDefinition();
                report.ReportDataSource = listData;

                // 指定是哪個 RDLC
                Type reportResourceNamespace = typeof(P23_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P19_FabricSticker_Print.rdlc";

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
                frm.Show();
                #endregion
            }
            else
            {
                MyUtility.Msg.InfoBox("Select data first.");
            }
        }
    }
}
