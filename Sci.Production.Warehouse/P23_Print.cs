using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Win;
using System;
using System.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P23_Print : Win.Tems.PrintForm
    {
        private DataRow CurrentMaintain;

        /// <inheritdoc/>
        public P23_Print(DataRow currentMaintain)
        {
            this.InitializeComponent();
            this.Text = "P23 " + currentMaintain["ID"].ToString();
            this.CurrentMaintain = currentMaintain;

            DataTable dtPMS_FabricQRCode_LabelSize;
            DualResult result = DBProxy.Current.Select(null, "select ID, Name from dropdownlist where Type = 'PMS_Fab_LabSize' order by Seq", out dtPMS_FabricQRCode_LabelSize);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.comboType.DisplayMember = "Name";
            this.comboType.ValueMember = "ID";
            this.comboType.DataSource = dtPMS_FabricQRCode_LabelSize;

            this.comboType.SelectedValue = MyUtility.GetValue.Lookup("select PMS_FabricQRCode_LabelSize from system");
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool ToPrint()
        {
            if (this.radioTransferSlip.Checked)
            {
                this.TransferSlip();
            }
            else if (this.radioQRCodeSticker.Checked)
            {
                P22_Print.QRCodeSticker(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), this.comboType.Text);
            }

            return true;
        }

        private void TransferSlip()
        {
            if (!MyUtility.Check.Seek($"select NameEN from MDivision where id = '{Env.User.Keyword}'", out DataRow dr))
            {
                MyUtility.Msg.WarningBox("Data not found!", "Title");
                return;
            }

            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", MyUtility.Convert.GetString(dr["NameEN"])));
            report.ReportParameters.Add(new ReportParameter("ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            report.ReportParameters.Add(new ReportParameter("Remark", MyUtility.Convert.GetString(this.CurrentMaintain["Remark"])));
            report.ReportParameters.Add(new ReportParameter("issuedate", ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["issuedate"])).ToShortDateString()));
            report.ReportParameters.Add(new ReportParameter("Factory", MyUtility.Convert.GetString(this.CurrentMaintain["MdivisionID"])));

            #region -- 撈表身資料 --
            string sqlcmd = $@"
select  t.frompoid
        ,t.fromseq1 + '-' +t.fromseq2 as SEQ
        ,t.topoid,t.toseq1  + '-' +t.toseq2 as TOSEQ
        ,[desc] = IIF ((p.ID = lag(p.ID,1,'')over (order by p.ID,p.seq1,p.seq2) 
                        and (p.seq1 = lag(p.seq1,1,'') over (order by p.ID,p.seq1,p.seq2)) 
                        and (p.seq2 = lag(p.seq2,1,'')over (order by p.ID,p.seq1,p.seq2))
                       ) 
                       , ''
                       , dbo.getMtlDesc(t.FromPOID,t.FromSeq1,t.FromSeq2,2,0)
                      )
        , t.fromroll
        , t.fromdyelot
        , p.StockUnit
        , [BULKLOCATION] = dbo.Getlocation(fi.ukey) 
        , fi.ContainerCode
        , t.Tolocation
        , t.ToContainerCode
        , t.Qty
        , [Total] = sum(t.Qty) OVER (PARTITION BY t.frompoid ,t.FromSeq1,t.FromSeq2 )         
from dbo.Subtransfer_detail t WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id= t.FromPOID 
                                                and p.SEQ1 = t.FromSeq1 
                                                and p.seq2 = t.FromSeq2 
left join dbo.FtyInventory FI on t.fromPoid = fi.poid 
                                 and t.fromSeq1 = fi.seq1 
                                 and t.fromSeq2 = fi.seq2
                                 and t.fromRoll = fi.roll 
                                 and t.fromStocktype = FI.stocktype
                                 and t.fromDyelot = FI.Dyelot
where t.id = '{this.CurrentMaintain["ID"]}'
order by t.frompoid,SEQ,BULKLOCATION,t.fromroll,t.FromDyelot
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtDetail);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!", "Detail");
                return;
            }

            // 傳 list 資料
            report.ReportDataSource = dtDetail.AsEnumerable()
                .Select(row1 => new P23_PrintData()
                {
                    StockSP = row1["frompoid"].ToString().Trim(),
                    StockSEQ = row1["SEQ"].ToString().Trim(),
                    IssueSP = row1["topoid"].ToString().Trim(),
                    SEQ = row1["TOSEQ"].ToString().Trim(),
                    DESC = row1["desc"].ToString().Trim(),
                    Roll = row1["fromroll"].ToString().Trim(),
                    DYELOT = row1["fromdyelot"].ToString().Trim(),
                    Unit = row1["StockUnit"].ToString().Trim(),
                    BULKLOCATION = row1["BULKLOCATION"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                    INVENTORYLOCATION = row1["Tolocation"].ToString().Trim() + Environment.NewLine + row1["ToContainerCode"].ToString().Trim(),
                    QTY = Convert.ToDecimal(row1["Qty"].ToString()),
                    TotalQTY = row1["Total"].ToString().Trim(),
                }).ToList();

            #endregion

            result = ReportResources.ByEmbeddedResource(typeof(P23_PrintData), "P23_Print.rdlc", out IReportResource reportresource);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }

            report.ReportResource = reportresource;
            new Win.Subs.ReportView(report) { MdiParent = this.MdiParent }.Show();
        }
    }
}
