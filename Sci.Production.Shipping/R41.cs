using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R41
    /// </summary>
    public partial class R41 : Win.Tems.PrintForm
    {
        private DataTable printImport;
        private DataTable printExport;
        private DataTable printAdjust;
        private DateTime? date1;
        private DateTime? date2;
        private string nlCode;
        private string contractNo;
        private string type;

        /// <summary>
        /// R41
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboType, 1, 1, ",Import,Export,Adjust");
            this.comboType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // if (MyUtility.Check.Empty(dateRange1.Value1))
            // {
            //    MyUtility.Msg.WarningBox("Date can't empty!!");
            //    dateRange1.TextBox1.Focus();
            //    return false;
            // }
            this.date1 = this.dateDate.Value1;
            this.date2 = this.dateDate.Value2;
            this.nlCode = this.txtNLCode.Text;
            this.contractNo = this.txtContractNo.Text;
            this.type = this.comboType.SelectedIndex == -1 || this.comboType.SelectedIndex == 0 ? string.Empty : this.comboType.SelectedIndex == 1 ? "Import" : this.comboType.SelectedIndex == 2 ? "Export" : "Adjust";
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCondition = new StringBuilder();
            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCondition.Append(string.Format(" and v.CDate >= '{0}' ", Convert.ToDateTime(this.date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCondition.Append(string.Format(" and v.CDate <= '{0}' ", Convert.ToDateTime(this.date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.nlCode))
            {
                sqlCondition.Append(string.Format(" and vd.NLCode = '{0}'", this.nlCode));
            }

            if (!MyUtility.Check.Empty(this.contractNo))
            {
                sqlCondition.Append(string.Format(" and v.VNContractID = '{0}'", this.contractNo));
            }

            DualResult result;
            if (this.type == "Import" || this.type == string.Empty)
            {
                if (this.rdSummary.Checked)
                {
                    result = this.QueryImport(sqlCondition.ToString());
                    if (!result)
                    {
                        return new DualResult(false, "Query import data fail\r\n" + result.ToString());
                    }
                }

                if (this.rdDetail.Checked)
                {
                    result = this.QueryImportDetail(sqlCondition.ToString());
                    if (!result)
                    {
                        return new DualResult(false, "Query import data fail\r\n" + result.ToString());
                    }
                }
            }

            if (this.type == "Export" || this.type == string.Empty)
            {
                if (this.rdSummary.Checked)
                {
                    result = this.QueryExport(sqlCondition.ToString());
                    if (!result)
                    {
                        return new DualResult(false, "Query import data fail\r\n" + result.ToString());
                    }
                }

                if (this.rdDetail.Checked)
                {
                    result = this.QueryExportDetail(sqlCondition.ToString());
                    if (!result)
                    {
                        return new DualResult(false, "Query import data fail\r\n" + result.ToString());
                    }
                }
            }

            if (this.type == "Adjust" || this.type == string.Empty)
            {
                if (this.rdSummary.Checked)
                {
                    result = this.QueryAdjust(sqlCondition.ToString());
                    if (!result)
                    {
                        return new DualResult(false, "Query import data fail\r\n" + result.ToString());
                    }
                }

                if (this.rdDetail.Checked)
                {
                    result = this.QueryAdjustDetail(sqlCondition.ToString());
                    if (!result)
                    {
                        return new DualResult(false, "Query import data fail\r\n" + result.ToString());
                    }
                }
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if ((this.type == "Import" || this.type == string.Empty)
                 && (this.printImport == null || this.printImport.Rows.Count <= 0))
            {
                MyUtility.Msg.WarningBox("Import data not found!");
            }

            if ((this.type == "Export" || this.type == string.Empty)
                 && (this.printExport == null || this.printExport.Rows.Count <= 0))
            {
                MyUtility.Msg.WarningBox("Export data not found!");
            }

            if ((this.type == "Adjust" || this.type == string.Empty)
                 && (this.printAdjust == null || this.printAdjust.Rows.Count <= 0))
            {
                MyUtility.Msg.WarningBox("Adjust data not found!");
            }

            this.ShowWaitMessage("Starting EXCEL...");
            bool result;
            if ((this.type == "Import" || this.type == string.Empty)
                 && this.printImport != null
                 && this.printImport.Rows.Count > 0)
            {
                if (this.rdSummary.Checked)
                {
                    result = MyUtility.Excel.CopyToXls(this.printImport, Sci.Production.Class.MicrosoftFile.GetName("Shipping_R41_Import"), xltfile: "Shipping_R41_Import.xltx", headerRow: 1, showSaveMsg: false);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }

                if (this.rdDetail.Checked)
                {
                    result = MyUtility.Excel.CopyToXls(this.printImport, Sci.Production.Class.MicrosoftFile.GetName("Shipping_R41_Import_Detail"), xltfile: "Shipping_R41_Import_Detail.xltx", headerRow: 1, showSaveMsg: false);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }
            }

            if ((this.type == "Export" || this.type == string.Empty)
                 && this.printExport != null
                 && this.printExport.Rows.Count > 0)
            {
                if (this.rdSummary.Checked)
                {
                    result = MyUtility.Excel.CopyToXls(this.printExport, Sci.Production.Class.MicrosoftFile.GetName("Shipping_R41_Export"), xltfile: "Shipping_R41_Export.xltx", headerRow: 1, showSaveMsg: false);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }

                if (this.rdDetail.Checked)
                {
                    result = MyUtility.Excel.CopyToXls(this.printExport, Sci.Production.Class.MicrosoftFile.GetName("Shipping_R41_Export_Detail"), xltfile: "Shipping_R41_Export_Detail.xltx", headerRow: 1, showSaveMsg: false);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }
            }

            if ((this.type == "Adjust" || this.type == string.Empty)
                 && this.printAdjust != null
                 && this.printAdjust.Rows.Count > 0)
            {
                if (this.rdSummary.Checked)
                {
                    result = MyUtility.Excel.CopyToXls(this.printAdjust, Sci.Production.Class.MicrosoftFile.GetName("Shipping_R41_Adjust"), xltfile: "Shipping_R41_Adjust.xltx", headerRow: 1, showSaveMsg: false);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }

                if (this.rdDetail.Checked)
                {
                    result = MyUtility.Excel.CopyToXls(this.printAdjust, Sci.Production.Class.MicrosoftFile.GetName("Shipping_R41_Adjust_Detail"), xltfile: "Shipping_R41_Adjust_Detail.xltx", headerRow: 1, showSaveMsg: false);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    }
                }
            }

            this.HideWaitMessage();
            return true;
        }

        // 查詢Import資料
        private DualResult QueryImport(string sqlCondition)
        {
            string sqlCmd = string.Format(
                @"select v.ID,v.CDate,v.VNContractID,v.DeclareNo,IIF(v.BLNo='',v.WKNo,v.BLNo) as BLWK,vd.NLCode,vd.HSCode,vd.Qty,vd.UnitID,vd.Remark
from VNImportDeclaration v WITH (NOLOCK) 
inner join VNImportDeclaration_Detail vd WITH (NOLOCK) on v.ID = vd.ID
where 1=1 {0} and v.Status = 'Confirmed'
order by v.ID", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printImport);
            return result;
        }

        // 查詢Export資料
        private DualResult QueryExport(string sqlCondition)
        {
            string sqlCmd = string.Format(
                @"select v.ID,v.CDate,v.VNContractID,v.DeclareNo,v.InvNo,ed.StyleID,ed.SeasonID,ed.BrandID,ed.ExportQty,
isnull(vd.NLCode,'') as NLCode,isnull(vd.HSCode,'') as HSCode,isnull(vd.Qty,0) as Usage,
isnull(vd.UnitID,'') as UnitID,isnull(vd.Waste,0) as Waste,
Round(ed.ExportQty*isnull(vd.Qty,0)*(1+isnull(vd.Waste,0)),3) as Total,
IIF(v.Status = 'Junked','Y','') as Cancel
from VNExportDeclaration v WITH (NOLOCK) 
inner join VNExportDeclaration_Detail ed WITH (NOLOCK) on v.ID = ed.ID
left join VNConsumption c WITH (NOLOCK) on c.VNContractID = v.VNContractID and c.CustomSP = ed.CustomSP
left join VNConsumption_Detail vd WITH (NOLOCK) on c.ID = vd.ID
left join VNContract_Detail vcd WITH (NOLOCK) on vcd.ID = v.VNContractID and vcd.NLCode = vd.NLCode
where 1=1 {0} and (v.Status = 'Confirmed' or v.Status = 'Junked')
order by v.ID", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printExport);
            return result;
        }

        // 查詢Adjust資料
        private DualResult QueryAdjust(string sqlCondition)
        {
            string sqlCmd = string.Format(
                @"select v.CDate,v.VNContractID,v.DeclareNo,vd.NLCode,isnull(cd.HSCode,'') as HSCode,vd.Qty,isnull(cd.UnitID,'') as UnitID,v.Remark
from VNContractQtyAdjust v WITH (NOLOCK) 
inner join VNContractQtyAdjust_Detail vd WITH (NOLOCK) on v.ID =vd.ID
left join VNContract_Detail cd WITH (NOLOCK) on cd.ID = v.VNContractID and cd.NLCode = vd.NLCode
where 1=1  {0} and v.Status = 'Confirmed'
order by v.CDate", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printAdjust);
            return result;
        }

        // 查詢ImportDetail資料
        private DualResult QueryImportDetail(string sqlCondition)
        {
            string sqlCmd = string.Format(
                @"
select v.ID,v.CDate,vdd.BrandID,vdd.Refno,
	FabricType= case when vdd.FabricType = 'F' then 'Fabric'
					 when vdd.FabricType = 'A' then 'Accessory' 
					 when vdd.FabricType = 'L' then (select Category from LocalItem l with(nolock) where ltrim(l.RefNo) = vdd.Refno)
					 end ,
	v.VNContractID,v.DeclareNo,
	BLWK=IIF(v.BLNo='',v.WKNo,v.BLNo),vdd.NLCode,vd.HSCode,vdd.Qty,vd.UnitID,vd.Remark
from VNImportDeclaration v WITH (NOLOCK) 
inner join VNImportDeclaration_Detail vd WITH (NOLOCK) on v.ID = vd.ID
inner join VNImportDeclaration_Detail_Detail vdd WITH (NOLOCK) on v.ID = vdd.ID and vd.NLCode = vdd.NLCode
where 1=1 {0} and v.Status = 'Confirmed'
order by v.ID", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printImport);
            return result;
        }

        // 查詢ExportDetail資料
        private DualResult QueryExportDetail(string sqlCondition)
        {
            string sqlCmd = string.Format(
                @"
select v.ID,v.CDate,vdd.RefNo,
	FabricType= iif(vdd.FabricType = 'L', li.Category,dbo.GetMaterialTypeDesc(vdd.FabricType)),
	v.VNContractID,v.DeclareNo,v.InvNo,ed.StyleID,ed.SeasonID,ed.BrandID,ed.ExportQty,
	isnull(vd.NLCode,'') as NLCode,isnull(vd.HSCode,'') as HSCode,isnull(vdd.Qty,0) as Usage,
	isnull(vd.UnitID,'') as UnitID,isnull(vd.Waste,0) as Waste,
	Round(ed.ExportQty*isnull(vdd.Qty,0)*(1+isnull(vd.Waste,0)),3) as Total,
	IIF(v.Status = 'Junked','Y','') as Cancel
from VNExportDeclaration v WITH (NOLOCK) 
inner join VNExportDeclaration_Detail ed WITH (NOLOCK) on v.ID = ed.ID
left join VNConsumption c WITH (NOLOCK) on c.VNContractID = v.VNContractID and c.CustomSP = ed.CustomSP
left join VNConsumption_Detail vd WITH (NOLOCK) on c.ID = vd.ID
left join VNConsumption_Detail_Detail vdd WITH (NOLOCK) on vdd.id = c.id and vdd.NLCode = vd.NLCode
left join VNContract_Detail vcd WITH (NOLOCK) on vcd.ID = v.VNContractID and vcd.NLCode = vd.NLCode
outer apply (select Category from LocalItem l with(nolock) where ltrim(l.RefNo) = vdd.Refno) li
where 1=1 {0} and (v.Status = 'Confirmed' or v.Status = 'Junked')
order by v.ID", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printExport);
            return result;
        }

        // 查詢AdjustDetail資料
        private DualResult QueryAdjustDetail(string sqlCondition)
        {
            string sqlCmd = string.Format(
                @"
select v.CDate,v.VNContractID,vdd.BrandID,Reason =concat(v.ReasonID,'-',sr.Description),vdd.Refno,
	FabricType= iif(vdd.FabricType = 'L', li.Category,dbo.GetMaterialTypeDesc(vdd.FabricType)),
	v.DeclareNo,vdd.NLCode,isnull(cd.HSCode,'') as HSCode,vdd.Qty,isnull(cd.UnitID,'') as UnitID,v.Remark
from VNContractQtyAdjust v WITH (NOLOCK) 
inner join VNContractQtyAdjust_Detail vd WITH (NOLOCK) on v.ID =vd.ID
inner join VNContractQtyAdjust_Detail_Detail vdd WITH (NOLOCK) on v.ID = vdd.ID and vd.NLCode = vdd.NLCode
left join VNContract_Detail cd WITH (NOLOCK) on cd.ID = v.VNContractID and cd.NLCode = vd.NLCode
left join ShippingReason sr WITH (NOLOCK) on sr.id = v.ReasonID and type = 'AQ'
outer apply (select Category from LocalItem l with(nolock) where ltrim(l.RefNo) = vdd.Refno) li
where 1=1  {0} and v.Status = 'Confirmed'
order by v.CDate", sqlCondition);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printAdjust);
            return result;
        }

        private void TxtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(@"select id,startdate,EndDate from [Production].[dbo].[VNContract]", "20,10,10", this.Text, false, ",", headercaptions: "Contract No, Start Date, End Date");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtContractNo.Text = item.GetSelectedString();
        }
    }
}
