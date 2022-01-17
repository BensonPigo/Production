using Ict;
using Sci.Data;
using Sci.Production.Class;
using Sci.Utility.Excel;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class R19 : Sci.Win.Tems.PrintForm
    {
        private enum PrintReportType
        {
            Basic,
            Requsition,
        }

        private DataTable dtPrint;
        private SaveXltReportCls sxc;
        private PrintReportType printReportType;

        /// <inheritdoc/>
        public R19(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            string sql = $@"
                select ID = '{PrintReportType.Basic}', Name ='Pad Print Basic List'
                UNION 
                select ID = '{PrintReportType.Requsition}', Name ='Pad Print Requsition List'
                ";
            DataTable dt = DBProxy.Current.SelectEx(sql).ExtendedData;
            this.cboReportType.ValueMember = "ID";
            this.cboReportType.DisplayMember = "Name";
            this.cboReportType.DataSource = dt;
            this.cboReportType.SelectedIndex = 0;
        }

        private DataTable GetDropDownList(string type)
        {
            string sql = $@"
    Select ID, Name From DropDownList where Type = '{type}'
    Order by id";

            return DBProxy.Current.SelectEx(sql).ExtendedData;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateCreateDate.Value1.HasValue && !this.dateCreateDate.Value2.HasValue
                && this.txtLabelFor.Text.Empty()
                && this.txtRefno.Text.Empty())
            {
                MyUtility.Msg.ErrorBox("[Create Date], [Label For] and [Refno] can't be blank at the same time.");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;
            this.dtPrint = null;

            try
            {
                string where = string.Empty;
                List<SqlParameter> pars = new List<SqlParameter>();

                if (this.dateCreateDate.Value1 != null && this.dateCreateDate.Value2 != null)
                {
                    where = this.printReportType == PrintReportType.Basic
                        ? " and pm.AddDate BETWEEN @CreateDate1 AND @CreateDate2 "
                        : " and pprd.AddDate BETWEEN @CreateDate1 AND @CreateDate2 ";

                    pars.Add(new SqlParameter("@CreateDate1", ((DateTime)this.dateCreateDate.Value1).ToString("yyyy-MM-dd")));
                    pars.Add(new SqlParameter("@CreateDate2", ((DateTime)this.dateCreateDate.Value2).ToString("yyyy-MM-dd 23:59:59")));
                }

                if (!MyUtility.Check.Empty(this.txtbrand1.Text))
                {
                    where += " and pp.BrandID in (select Data From SplitString(@BrandID,','))";
                    pars.Add(new SqlParameter("@BrandID", this.txtbrand1.Text));
                }

                if (!MyUtility.Check.Empty(this.txtSupplier.TextBox1.Text))
                {
                    where += " and pp.SuppID = @SuppID";
                    pars.Add(new SqlParameter("@SuppID", this.txtSupplier.TextBox1.Text));
                }

                if (!MyUtility.Check.Empty(this.txtSeason.Text))
                {
                    where += " and pm.Season in (select Data From SplitString(@SeasonID,','))";
                    pars.Add(new SqlParameter("@SeasonID", this.txtSeason.Text));
                }

                if (!MyUtility.Check.Empty(this.txtRefno.Text))
                {
                    where += " and pp.Refno in (select Data From SplitString(@Refno,','))";
                    pars.Add(new SqlParameter("@Refno", this.txtRefno.Text));
                }

                if (!MyUtility.Check.Empty(this.txtSizePage.Text))
                {
                    where += " and pms.SizePage = @SizePage";
                    pars.Add(new SqlParameter("@SizePage", this.txtSizePage.Text));
                }

                if (!MyUtility.Check.Empty(this.txtMainSize.Text))
                {
                    where += " and pm.MainSize in (select Data From SplitString(@MainSize,','))";
                    pars.Add(new SqlParameter("@MainSize", this.txtMainSize.Text));
                }

                if (!MyUtility.Check.Empty(this.txtLabelFor.Text))
                {
                    where += " and pm.LabelFor in (select Data From SplitString(@LabelFor,','))";
                    pars.Add(new SqlParameter("@LabelFor", this.txtLabelFor.Text));
                }

                if (!MyUtility.Check.Empty(this.txtGender.Text))
                {
                    where += " and pm.Gender in (select Data From SplitString(@Gender,','))";
                    pars.Add(new SqlParameter("@Gender", this.txtGender.Text));
                }

                if (!MyUtility.Check.Empty(this.txtAgeGroup.Text))
                {
                    where += " and pm.AgeGroup in (select Data From SplitString(@AgeGroup,','))";
                    pars.Add(new SqlParameter("@AgeGroup", this.txtAgeGroup.Text));
                }

                if (!MyUtility.Check.Empty(this.txtSizeSpec.Text))
                {
                    where += " and pm.SizeSpec in (select Data From SplitString(@SizeSpec,','))";
                    pars.Add(new SqlParameter("@SizeSpec", this.txtSizeSpec.Text));
                }

                if (!MyUtility.Check.Empty(this.txtPart.Text))
                {
                    where += " and pm.Part in (select Data From SplitString(@Part,','))";
                    pars.Add(new SqlParameter("@Part", this.txtPart.Text));
                }

                if (!MyUtility.Check.Empty(this.txtRegion.Text))
                {
                    where += " and pm.Region in (select Data From SplitString(@Region,','))";
                    pars.Add(new SqlParameter("@Region", this.txtRegion.Text));
                }

                string column1 = string.Empty;
                string column2 = string.Empty;
                string join = string.Empty;
                if (this.printReportType == PrintReportType.Requsition)
                {
                    where += " and pprd.id is not null";
                    column1 = @", pprd.ID 
                                ,mmsd.ID+'-'+'('+mmsd.Seq1+'-'+mmsd.Seq2+')' ";
                    column2 = @",e.ID 
                                ,e.Eta 
                                ,e.ShipModeID ";
                    join = @"LEFT JOIN [dbo].SciMachine_MiscOtherPO_Detail mmsd on mmsd.MachineReqID = pprd.id and mmsd.Seq2 = pprd.Seq2
                            LEFT JOIN Export_Detail ed on ed.POID = mmsd.ID and mmsd.Seq2 = ed.Seq2 and ed.Seq1 = mmsd.Seq1 
                            LEFT JOIN Export e on e.id= ed.ID ";
                }

                var sqlCmd = string.Empty;

                sqlCmd = $@"
/*
Declare @CreateDate1 date = '{(DateTime)this.dateCreateDate.Value1:yyyy/MM/dd}'
Declare @CreateDate2 date = '{(DateTime)this.dateCreateDate.Value2:yyyy/MM/dd}'
Declare @BrandID Varchar(100) = '{this.txtbrand1.Text}'
Declare @SuppID Varchar(6) = '{this.txtSupplier.TextBox1.Text}'
Declare @SeasonID Varchar(100) = '{this.txtSeason.Text}'
Declare @MainSize Varchar(100) = '{this.txtMainSize.Text}'
Declare @LabelFor Varchar(100) =  '{this.txtLabelFor.Text}'
Declare @Refno Varchar(23) = '{this.txtRefno.Text}'
Declare @SizePage Varchar(100) = '{this.txtSizePage.Text}'
Declare @Gender Varchar(100) = '{this.txtGender.Text}'
Declare @AgeGroup Varchar(100) = '{this.txtAgeGroup.Text}'
Declare @SizeSpec Varchar(100) = '{this.txtSizeSpec.Text}'
Declare @Part Varchar(300) = '{this.txtPart.Text}'
Declare @Region Varchar(1000) = '{this.txtRegion.Text}'
*/

Select pm.MoldID
, pp.BrandID
, pm.Season
{column1}
, pprd.Price
, pp.Refno
, MainSize = isnull(ddlMainSize.Name, '')
, pms.SizePage
, pms.SourceSize
, Gender = isnull(ddlGender.Name, '')
, AgeGroup = isnull(ddlAgeGroup.Name, '')
, SizeSpec = isnull(ddlSizeSpec.Name, '')
, MappingSizePage = pms.SizePage
, Part = isnull(ddlPart.Name, '')
, pms.CustomerSize
, pms.MoldRef
, MadeIn = isnull(ddlMadeIn.Name, '')
{column2}
, pms.Version
, pms.Reason
, Junk = iif(pp.Junk = 1, 'Y', '')
From Padprint pp
Left join PadPrint_Mold pm on pp.Ukey = pm.PadPrint_ukey
Left join PadPrint_Mold_Spec pms on pms.PadPrint_ukey = pm.PadPrint_ukey and pms.MoldID = pm.MoldID
LEFT JOIN PadPrintReq_Detail pprd on pprd.PadPrint_Ukey = pp.Ukey and pprd.MoldID = pm.MoldID
{join}
Left join DropDownList ddlGender on ddlGender.ID = pm.Gender and ddlGender.Type = 'PadPrint_Gender'
Left join DropDownList ddlAgeGroup on ddlAgeGroup.ID = pm.AgeGroup and ddlAgeGroup.Type = 'PadPrint_AgeGroup'
Left join DropDownList ddlSizeSpec on ddlSizeSpec.ID = pm.SizeSpec and ddlSizeSpec.Type = 'PadPrint_SizeSpec'
Left join DropDownList ddlMainSize on ddlMainSize.ID = pm.MainSize and ddlMainSize.Type = 'PadPrint_MainSize'
Left join DropDownList ddlMadeIn on ddlMadeIn.ID = pm.MadeIn and ddlMadeIn.Type = 'PadPrint_MadeIn'
Left join DropDownList ddlPart on ddlPart.ID = pm.Part and ddlPart.Type = 'PadPrint_Part'
Where 1 = 1
{where}
Order by pm.MoldID, pp.BrandID
";

                var rowCount = 0;

                result = DBProxy.Current.Select(null, sqlCmd, pars, out this.dtPrint);
                if (!result)
                {
                    return result;
                }

                rowCount = this.dtPrint.Rows.Count;

                if (this.dtPrint != null && this.dtPrint.Rows.Count > 0)
                {
                    // 顯示筆數
                    this.SetCount(rowCount);
                    return this.TransferToExcel();
                }
                else
                {
                    return new DualResult(false, "Data not found.");
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex);
            }
        }

        private DualResult TransferToExcel()
        {
            try
            {
                string xltPath = Path.Combine(Env.Cfg.XltPathDir, "PPIC_R19.PadPrintBasicReport.xltx");
                if (this.printReportType == PrintReportType.Requsition)
                {
                    xltPath = Path.Combine(Env.Cfg.XltPathDir, "PPIC_R19.PadPrintRequsitionReport.xltx");
                }

                this.sxc = new SaveXltReportCls(xltPath);
                this.sxc.BoOpenFile = true;
                SaveXltReportCls.XltRptTable xrt = new SaveXltReportCls.XltRptTable(this.dtPrint);
                Excel.Worksheet wks = this.sxc.ExcelApp.Sheets[1];
                wks.Cells[2, 1].Value = "##tbl";
                xrt.BoAutoFitColumn = true;
                xrt.ShowHeader = false;
                this.sxc.DicDatas.Add("##tbl", xrt);
                this.sxc.Save();

                return Result.True;
            }
            catch (Exception ex)
            {
                if (this.sxc.ExcelApp != null)
                {
                    this.sxc.ExcelApp.DisplayAlerts = false;
                    this.sxc.ExcelApp.Quit();
                }

                return new DualResult(false, "Export excel error.", ex);
            }
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            return true;
        }

        private void TxtRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = $@"
            select distinct Refno From PadPrint";

            DataTable dt = DBProxy.Current.SelectEx(sql).ExtendedData;
            using (var dlg = new SelectItem2(dt, "Refno", "Refno", "10", this.txtRefno.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtRefno.Text = dlg.GetSelectedString();
                }
            }
        }

        private void TxtMainSize_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dt = this.GetDropDownList("PadPrint_MainSize");
            using (var dlg = new SelectItem2(dt, "ID,Name", "ID,Name", "5,10", this.txtMainSize.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtMainSize.Text = dlg.GetSelectedString();
                }
            }
        }

        private void TxtLabelFor_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dt = this.GetDropDownList("PadPrint_LabelFor");
            using (var dlg = new SelectItem2(dt, "ID,Name", "ID,Name", "5,10", this.txtLabelFor.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtLabelFor.Text = dlg.GetSelectedString();
                }
            }
        }

        private void TxtGender_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dt = this.GetDropDownList("PadPrint_Gender");
            using (var dlg = new SelectItem2(dt, "ID,Name", "ID,Name", "5,10", this.txtGender.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtGender.Text = dlg.GetSelectedString();
                }
            }
        }

        private void TxtAgeGroup_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dt = this.GetDropDownList("PadPrint_AgeGroup");
            using (var dlg = new SelectItem2(dt, "ID,Name", "ID,Name", "5,10", this.txtAgeGroup.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtAgeGroup.Text = dlg.GetSelectedString();
                }
            }
        }

        private void TxtSizeSpec_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dt = this.GetDropDownList("PadPrint_SizeSpec");
            using (var dlg = new SelectItem2(dt, "ID,Name", "ID,Name", "5,10", this.txtSizeSpec.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtSizeSpec.Text = dlg.GetSelectedString();
                }
            }
        }

        private void TxtPart_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dt = this.GetDropDownList("PadPrint_Part");
            using (var dlg = new SelectItem2(dt, "ID,Name", "ID,Name", "5,10", this.txtPart.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtPart.Text = dlg.GetSelectedString();
                }
            }
        }

        private void TxtRegion_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dt = this.GetDropDownList("PadPrint_Region");
            using (var dlg = new SelectItem2(dt, "ID,Name", "ID,Name", "5,10", this.txtRegion.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtRegion.Text = dlg.GetSelectedString();
                }
            }
        }

        private void CboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboReportType != null && this.cboReportType.SelectedValue2 != null)
            {
                var selectValue = this.cboReportType.SelectedValue2.ToString();
                this.printReportType = selectValue.EqualString(PrintReportType.Basic)
                    ? PrintReportType.Basic
                    : PrintReportType.Requsition;
            }
        }
    }
}
