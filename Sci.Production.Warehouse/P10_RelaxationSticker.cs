using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P10_RelaxationSticker : Sci.Win.Subs.Base
    {
        private string strIssueID;

        public P10_RelaxationSticker(string issueID)
        {
            this.InitializeComponent();
            this.strIssueID = issueID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region set Grid Columns
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, trueValue: 1, falseValue: 0)
                .Text("SPNO", header: "SP#", iseditingreadonly: true)
                .Text("Seq", header: "Seq#", iseditingreadonly: true)
                .Text("Refno", header: "Refno", iseditingreadonly: true)
                .Text("Color", header: "Color", iseditingreadonly: true)
                .Text("Roll", header: "Roll", iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 2, iseditingreadonly: true);
            #endregion

            for (int i = 0; i < this.grid1.Columns.Count; i++)
            {
                this.grid1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            string sqlcmd = $@"
select 
Sel = 0
 , RowNo = Row_Number() over (order by psd.Refno,isd.POID,isd.Roll)
 , SPNo = isd.POID
 , Seq = Concat (isd.Seq1, '-', isd.Seq2)
 , Roll = isd.Roll
 , Dyelot = isd.Dyelot
 , Refno = isnull (psd.Refno, '')
 , Color = isnull (psd.ColorID, '')
 , Qty = StockList.Qty
from Issue_Detail isd
left join Orders o on o.ID=isd.POID
left join Po_Supp_Detail psd on isd.POID = psd.ID
 and isd.Seq1 = psd.SEQ1
 and isd.Seq2 = psd.SEQ2
outer apply(
    select SUM(FI.InQty) AS Qty 
    from FtyInventory fi 
    where fi.POID=isd.POID and fi.Seq1= isd.Seq1 and fi.Seq2 = isd.Seq2 
    and fi.Roll = isd.Roll and fi.Dyelot = isd.Dyelot
    and StockType in ('B','I')
) as StockList
where isd.ID = '{this.strIssueID}'
order by RowNo
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt);
            if (result == false)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource.DataSource = dt;
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
select NewRowNo = Row_Number() over (order by RowNo), *
from #tmp
order by NewRowNo";
                result = MyUtility.Tool.ProcessWithDatatable(dtPrint, string.Empty, strDtSortSQL, out dtPrint);

                if (result == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                List<P10_RelaxationSticker_Data> listData = dtPrint.AsEnumerable().Select(row => new P10_RelaxationSticker_Data()
                {
                    RowNo = Convert.ToInt32(row["NewRowNo"]),
                    SPNo = row["SPNo"].ToString().Trim(),
                    Seq = row["Seq"].ToString().Trim(),
                    Refno = row["Refno"].ToString().Trim(),
                    Color = row["Color"].ToString().Trim(),
                    Roll = row["Roll"].ToString().Trim(),
                    Dyelot = row["Dyelot"].ToString().Trim(),
                    Qty = Convert.ToDouble(row["Qty"]),
                }).ToList();

                ReportDefinition report = new ReportDefinition
                {
                    ReportDataSource = listData,
                };

                // 指定是哪個 RDLC
                Type ReportResourceNamespace = typeof(P10_RelaxationSticker_Data);
                Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
                string ReportResourceName = "P10_RelaxationSticker_Print.rdlc";

                Ict.Win.IReportResource reportresource;

                if ((result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)) == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Sci.Win.Subs.ReportView(report);
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
