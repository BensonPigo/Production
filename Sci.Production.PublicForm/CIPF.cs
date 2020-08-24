using Ict;
using Ict.Win;
using System;
using System.Data;
using Sci.Data;

namespace Sci.Production.PublicForm
{
    /// <summary>
    /// CIPF
    /// </summary>
    public partial class CIPF : Win.Subs.Input4
    {
        private DataTable Detaildt;

        // private DataTable Sumdt;
        private long Ukey;

        /// <summary>
        /// CIPF
        /// </summary>
        /// <param name="ukey">ukey</param>
        public CIPF(long ukey)
        {
            this.InitializeComponent();
            this.Ukey = ukey;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
               .Text("ArtworkTypeID", header: "ArtWork Type", width: Widths.AnsiChars(24), iseditingreadonly: true)
               .Numeric("ProSMV", header: "ProductionSMV", decimal_places: 2, iseditingreadonly: true)
               .Numeric("ProTMS", header: "ProductionTMS", decimal_places: 0, iseditingreadonly: true)
               ;
            return true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlcmd = $@"
select ArtworkTypeID = ArtworkTypeID +' - '+ 
	case when CIPF = 'C' then 'Cutting'
		 when CIPF = 'I' then 'Inspection'
		 when CIPF = 'P' then 'Pressing'
		 when CIPF = 'F' then 'Packing' end
	,isd.IETMSUkey,isd.StyleUkey,isd.ProSMV,ProTMS = ceiling(isd.ProTMS),isd.ProPrice,isd.CIPF
from [IETMS_Summary_detail] isd 
where [IETMSUkey] = '{this.Ukey}'";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.Detaildt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.grid1.DataSource = this.Detaildt;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            // if (this.grid1.RowCount <= 0)// && this.grid2.RowCount <= 0)
            // {
            //    return;
            // }

            // try
            // {
            //    // Sheet1-Detail
            //    Sxrc sxc = new Sxrc();
            //    Excel.Application app = sxc.ExcelApp;
            //    Excel.Worksheet wks = app.ActiveWorkbook.Sheets[1];
            //    wks.Name = "Detail";
            //    wks.Cells[1, 1] = "##tbl";

            // Sxrc.xltRptTable xrt = new Sxrc.xltRptTable(this.Detaildt);
            //    xrt.boAutoFitColumn = true;

            // xrt.lisColumnInfo.Add(new SaveXltReportCls.xlsColumnInfo("SMV") { NumberFormate = "###0.0000" });
            //    xrt.lisColumnInfo.Add(new SaveXltReportCls.xlsColumnInfo("ProductionSMV") { NumberFormate = "###0.0000" });
            //    sxc.dicDatas.Add("##tbl", xrt);

                //// Sheet2-Sum
                // wks = app.ActiveWorkbook.Sheets.Add(Type.Missing, app.ActiveWorkbook.Sheets[1]);
                // wks.Name = "Sum";
                // wks.Cells[1, 1] = "##tbl2";
                // xrt = new SaveXltReportCls.xltRptTable(this.Sumdt);
                // xrt.boAutoFitColumn = true;

                // xrt.lisColumnInfo.Add(new SaveXltReportCls.xlsColumnInfo("SMV") { NumberFormate = "###0.0000" });
                // xrt.lisColumnInfo.Add(new SaveXltReportCls.xlsColumnInfo("ProductionSMV") { NumberFormate = "###0.0000" });
                // sxc.dicDatas.Add("##tbl2", xrt);

                // sxc.boOpenFile = true;
                // sxc.Save();
            // }
            // catch (Exception ex)
            // {
            //    this.ShowErr(ex);
            // }
        }
    }
}
