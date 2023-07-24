using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Text;
using System.Data.SqlTypes;
using System.Globalization;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R48 : Win.Tems.PrintForm
    {
        private string spno1;
        private string spno2;
        private string strM;
        private string strFactory;
        private string strWhere;
        private DataTable printData;

        /// <inheritdoc/>
        public R48(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.spno1 = this.txtSPNo1.Text;
            this.spno2 = this.txtSPNo2.Text;
            this.strM = this.txtMdivision.Text;
            this.strFactory = this.txtfactory.Text;
            this.strWhere = string.Empty;

            // 任一筆有值但不是兩個都有值, 就只需篩選單張訂單
            if ((MyUtility.Check.Empty(this.spno1) || MyUtility.Check.Empty(this.spno2)) && (!MyUtility.Check.Empty(this.spno1) || !MyUtility.Check.Empty(this.spno2)))
            {
                string spno = MyUtility.Check.Empty(this.spno1) ? this.spno2 : this.spno1;
                this.strWhere += $@" and loi.PoID = '{spno}'";
            }

            if (!MyUtility.Check.Empty(this.spno1) && !MyUtility.Check.Empty(this.spno2))
            {
                this.strWhere += $@" and loi.POID >='{this.spno1}' and loi.POID <= '{this.spno2}'";
            }

            if (!MyUtility.Check.Empty(this.strM))
            {
                this.strWhere += $@" and o.MDivisionID = '{this.strM}'";
            }

            if (!MyUtility.Check.Empty(this.strFactory))
            {
                this.strWhere += $@" and o.FactoryID = '{this.strFactory}'";
            }

            if (this.checkQty.Checked)
            {
                this.strWhere += @" and (loi.InQty - loi.OutQty + loi.AdjustQty) > 0 ";
            }

            return base.ValidateInput();
        }

        private string SQLCmd_Detail()
        {
            string sqlCmd = $@"
select  loi.POID,
        [Seq] = CONCAT(loi.Seq1,' ',loi.Seq2),
		loi.Roll,
        loi.Dyelot,
		[MaterialType] = CASE lom.FabricType 
					WHEN 'F' then CONCAT('Fabric-',lom.MtlType)
					WHEN 'A' THEN CONCAT('Accessory-',lom.MtlType) ELSE '' END,
        lom.Refno,
		lom.[Desc],
		lom.Color,        
        lom.Unit,
        loi.Tone,
		loi.InQty,
		loi.OutQty,
		loi.AdjustQty,        
        [BalanceQty] = loi.InQty - loi.OutQty + loi.AdjustQty,        
        [Location] = Location.val
from    LocalOrderInventory loi with (nolock)
left join LocalOrderMaterial lom with (nolock) on loi.Poid = lom.Poid and loi.Seq1 = lom.Seq1 and loi.Seq2 = lom.Seq2
left join Orders o with (nolock) on o.ID=loi.POID
outer apply (
	SELECT val =  Stuff((select distinct concat( ',',MtlLocationID)   
	from LocalOrderInventory_Location loil with (nolock)
	WHERE loil.LocalOrderInventoryUkey	 = loi.Ukey
	FOR XML PATH('')),1,1,'')  
) Location
where  1=1
{this.strWhere}
";
            return sqlCmd;
        }

        private string SQLCmd_Summary()
        {
            string sqlCmd = $@"
select  loi.POID,
        [Seq] = CONCAT(loi.Seq1,' ',loi.Seq2),		
		[MaterialType] = CASE lom.FabricType 
					WHEN 'F' then CONCAT('Fabric-',lom.MtlType)
					WHEN 'A' THEN CONCAT('Accessory-',lom.MtlType) ELSE '' END,
        lom.Refno,
		lom.[Desc],
		lom.Color,        
        lom.Unit,
		InQty = round(sum(loi.InQty),2),
		OutQty = round(sum(loi.OutQty),2),
		AdjustQty = round(sum(loi.AdjustQty),2),        
        [BalanceQty] = round(sum(loi.InQty - loi.OutQty + loi.AdjustQty),2),        
        [Location] = Location.val
from    LocalOrderInventory loi with (nolock)
left join  LocalOrderMaterial lom with (nolock) on loi.Poid = lom.Poid and loi.Seq1 = lom.Seq1 and loi.Seq2 = lom.Seq2
left join Orders o with (nolock) on o.ID=loi.POID
outer apply (
	SELECT val =  Stuff((select distinct concat( ',',MtlLocationID)   
	from LocalOrderInventory_Location loil with (nolock)
	WHERE loil.LocalOrderInventoryUkey	 = loi.Ukey
	FOR XML PATH('')),1,1,'')  
) Location
where  1=1
{this.strWhere}
GROUP BY loi.POID,loi.Seq1,loi.Seq2,lom.FabricType,lom.MtlType,lom.Refno,lom.[Desc],lom.Color,lom.Unit,Location.val
";
            return sqlCmd;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = string.Empty;
            if (this.radioDetail.Checked)
            {
                sqlcmd = this.SQLCmd_Detail();
            }
            else
            {
                sqlcmd = this.SQLCmd_Summary();
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);
            if (this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            try
            {
                this.ShowWaitMessage("Excel Processing...");
                string fileName = this.radioDetail.Checked ? "Warehouse_R48_Detail.xltx" : "Warehouse_R48_Summary.xltx";

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + fileName); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, fileName, 1, false, null, objApp);

                objApp.Cells.EntireColumn.AutoFit();
                objApp.Cells.EntireRow.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R48");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);

                strExcelName.OpenFile();
                #endregion
                this.HideWaitMessage();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
