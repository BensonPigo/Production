using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R19
    /// </summary>
    public partial class R19 : Win.Tems.PrintForm
    {
        private DataTable PrintTable;
        private string eta_s;
        private string eta_e;
        private string wk_s;
        private string wk_e;
        private string factory;
        private string consignee;
        private string shipMode;

        /// <summary>
        /// R19
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R19(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dateETA.HasValue1 || !this.dateETA.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <ETA> first!!");
                return false;
            }

            this.eta_s = this.dateETA.Value1.Value.ToString("yyyy/MM/dd");
            this.eta_e = this.dateETA.Value2.Value.ToString("yyyy/MM/dd");
            this.wk_s = this.txtWKno_s.Text;
            this.wk_e = this.txtWKno_e.Text;
            this.factory = this.txtscifactory.Text;
            this.consignee = this.txtConsignee.Text;
            this.shipMode = this.txtshipmode.Text;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlCmd = string.Empty;

            #region Where 條件
            List<SqlParameter> paras = new List<SqlParameter>();
            string where = $@"e.Eta between '{this.eta_s}'AND '{this.eta_e}' 
and exists (select 1 from Factory where e.FactoryID = id and IsProduceFty = 1) ";

            if (!MyUtility.Check.Empty(this.wk_s))
            {
                paras.Add(new SqlParameter("@wk_s", this.wk_s));
                where += "AND e.ID >= @wk_s " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.wk_e))
            {
                paras.Add(new SqlParameter("@wk_e", this.wk_e));
                where += "AND e.ID <= @wk_e " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                where += $"AND e.FactoryID = '{this.factory}' " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.consignee))
            {
                paras.Add(new SqlParameter("@Consignee", this.consignee));
                where += "AND e.Consignee = @Consignee " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.shipMode))
            {
                where += $"AND e.ShipModeID = '{this.shipMode}' " + Environment.NewLine;
            }
            #endregion

            #region SQL
            sqlCmd = $@"


SELECT e.ID
,e.Blno
,vd.ID
,vd.DeclareNo
,e.FactoryID
,e.Consignee
,e.ShipModeID
,MainCYCFS.CYCFS
,e.InvNo
,[Payer] = (case when  e.Payer= 'S' then 'By Sci Taipei Office(Sender)'
			when Payer= 'M' then 'By Mill(Sender)'
			when Payer= 'F' then 'By Factory(Receiver)'
			else '' end)
,e.Vessel
,e.ExportPort
,e.ExportCountry
,e.Packages
,e.NetKg
,e.WeightKg
,e.CBM
,e.Eta
,e.PackingArrival
,e.DocArrival
,e.PortArrival
,e.WhseArrival
,[NoImportCharge] =	IIF(e.NoImportCharges=1,'Y','N')
,[Replacement] = IIF(e.Replacement=1,'Y','N')
,[Delay] =	IIF(e.Delay=1,'Y','N')
,[NonDeclare] =	IIF(e.NonDeclare=1,'Y','N')

,[DoorToDoorDelivery] =IIF(Dtdd.DoorToDoorDelivery=1,'Y','N')
,[SQCS] = IIF(e.SQCS=1,'Y','N')
,[RemarkFromTPE] =ISNULL(e.Remark,'')
,[RemarkToTPE] =	ISNULL(e.Remark_Factory,'')

FROM Export e WITH(NOLOCK)
LEFT JOIN VNImportDeclaration vd WITH(NOLOCK) ON e.Blno = vd.Blno AND vd.IsFtyExport = 0
OUTER APPLY(
	SELECT  CYCFS from Export a WITH (NOLOCK) where a.ID =  e.MainExportID
)MainCYCFS
OUTER APPLY(
	SELECT 1 as [DoorToDoorDelivery]
	FROM Door2DoorDelivery 
	WHERE ExportPort = e.ExportPort
			and ExportCountry =e.ExportCountry
			and ImportCountry = e.ImportCountry
			and ShipModeID = e.ShipModeID
			and Vessel =e.Vessel
	UNION 
	SELECT 1 as [DoorToDoorDelivery]
	FROM Door2DoorDelivery
	WHERE ExportPort = e.ExportPort
			and ExportCountry =e.ExportCountry
			and ImportCountry = e.ImportCountry
			and ShipModeID = e.ShipModeID
			and Vessel  =''

)Dtdd
WHERE {where}
ORDER BY e.FactoryID, e.ID
";
            #endregion

            return DBProxy.Current.Select(null, sqlCmd, paras, out this.PrintTable);
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.SetCount(this.PrintTable.Rows.Count);
            if (this.PrintTable.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel processing...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Shipping_R19.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.PrintTable, string.Empty, "Shipping_R19.xltx", 1, false, null, objApp); // 將datatable copy to excel

            #region Save & Show Excel

            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);

            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
