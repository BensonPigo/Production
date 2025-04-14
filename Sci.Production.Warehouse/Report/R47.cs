using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R47 : Win.Tems.PrintForm
    {
        private List<SqlParameter> listSqlPara = new List<SqlParameter>();
        private string sqlWhere = string.Empty;
        private DataTable printData;

        /// <inheritdoc/>
        public R47(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlWhere = string.Empty;
            this.listSqlPara.Clear();

            if (MyUtility.Check.Empty(this.txtSP.Text) &&
                MyUtility.Check.Empty(this.unrollStartTime.Value1) && MyUtility.Check.Empty(this.unrollStartTime.Value2) &&
                MyUtility.Check.Empty(this.unrollEndTime.Value1) && MyUtility.Check.Empty(this.unrollEndTime.Value2) &&
                MyUtility.Check.Empty(this.RelaxationStartTime.Value1) && MyUtility.Check.Empty(this.RelaxationStartTime.Value2) &&
                MyUtility.Check.Empty(this.RelaxationEndTime.Value1) && MyUtility.Check.Empty(this.RelaxationEndTime.Value2))
            {
                MyUtility.Msg.WarningBox("Unroll Start Time, Unroll End Time, Relaxation Start Time and Relaxation End Time cannot all be empty.");
                return false;
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                this.listSqlPara.Add(new SqlParameter("@POID", this.txtSP.Text));
                this.sqlWhere += " and fur.POID  = @POID";
            }

            if (!MyUtility.Check.Empty(this.unrollStartTime.Value1))
            {
                this.listSqlPara.Add(new SqlParameter("@UnrollStartTime_Value1", ((DateTime)this.unrollStartTime.Value1).ToString("yyyy/MM/dd")));
                this.sqlWhere += " and UnrollStartTime  >= CAST(@UnrollStartTime_Value1 AS DATE)";
            }

            if (!MyUtility.Check.Empty(this.unrollStartTime.Value2))
            {
                this.listSqlPara.Add(new SqlParameter("@UnrollStartTime_Value2", ((DateTime)this.unrollStartTime.Value2).ToString("yyyy/MM/dd")));
                this.sqlWhere += " and UnrollStartTime < DATEADD(DAY, 1, CAST(@UnrollStartTime_Value2 AS DATE))";
            }

            if (!MyUtility.Check.Empty(this.unrollEndTime.Value1))
            {
                this.listSqlPara.Add(new SqlParameter("@UnrollEndTime_Value1", ((DateTime)this.unrollEndTime.Value1).ToString("yyyy/MM/dd")));
                this.sqlWhere += " and UnrollEndTime  >= CAST(@UnrollEndTime_Value1 AS DATE) ";
            }

            if (!MyUtility.Check.Empty(this.unrollEndTime.Value2))
            {
                this.listSqlPara.Add(new SqlParameter("@UnrollEndTime_Value2", ((DateTime)this.unrollEndTime.Value2).ToString("yyyy/MM/dd")));
                this.sqlWhere += " and UnrollEndTime < DATEADD(DAY, 1, CAST(@UnrollEndTime_Value2 AS DATE))";
            }

            if (!MyUtility.Check.Empty(this.RelaxationStartTime.Value1))
            {
                this.listSqlPara.Add(new SqlParameter("@RelaxationStartTime_Value1", ((DateTime)this.RelaxationStartTime.Value1).ToString("yyyy/MM/dd")));
                this.sqlWhere += " and RelaxationStartTime  >= CAST(@RelaxationStartTime_Value1 AS DATE)";
            }

            if (!MyUtility.Check.Empty(this.RelaxationStartTime.Value2))
            {
                this.listSqlPara.Add(new SqlParameter("@RelaxationStartTime_Value2", ((DateTime)this.RelaxationStartTime.Value2).ToString("yyyy/MM/dd")));
                this.sqlWhere += " and RelaxationStartTime < DATEADD(DAY, 1, CAST(@RelaxationStartTime_Value2 AS DATE))";
            }

            if (!MyUtility.Check.Empty(this.RelaxationEndTime.Value1))
            {
                this.listSqlPara.Add(new SqlParameter("@RelaxationEndTime_Value1", ((DateTime)this.RelaxationEndTime.Value1).ToString("yyyy/MM/dd")));
                this.sqlWhere += " and RelaxationEndTime  >= CAST(@RelaxationEndTime_Value1 AS DATE)";
            }

            if (!MyUtility.Check.Empty(this.RelaxationEndTime.Value2))
            {
                this.listSqlPara.Add(new SqlParameter("@RelaxationEndTime_Value2", ((DateTime)this.RelaxationEndTime.Value2).ToString("yyyy/MM/dd")));
                this.sqlWhere += " and RelaxationEndTime < DATEADD(DAY, 1, CAST(@RelaxationEndTime_Value2 AS DATE))";
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Empty;
            sqlCmd = $@"
            select 
                [SP#] = fur.POID, 
                Seq = concat (fur.seq1, ' ', fur.seq2),
                fur.Roll,
                fur.Dyelot,
                fur.Barcode,
                rr.FabricRelaxationID,
                NeedUnroll = iif (fr.NeedUnroll = 1, 'Y', ''),
                fr.Relaxtime, 
                fur.UnrollStartTime,
                fur.UnrollEndTime, 
                fur.RelaxationStartTime, 
                fur.RelaxationEndTime, 
                UnrollScanner = dbo.getPass1 (fur.UnrollScanner), 
                [UnrollMachine] = MIOT.MachineID,
                fur.UnrollActualQty, 
                Location = dbo.Getlocation(fi.ukey),
                fi.Tone,
                IsAdvance = IIF(fur.IsAdvance = 1, 'Y', ''),
                fur.UnrollRemark
            from Fabric_UnrollandRelax fur
            left join PO_Supp_Detail psd on fur.POID = psd.ID
                                        and fur.Seq1 = psd.SEQ1
                                        and fur.Seq2 = psd.SEQ2
            left join [ExtendServer].ManufacturingExecution.dbo.RefnoRelaxtime rr on psd.Refno = rr.Refno
            left join [ExtendServer].ManufacturingExecution.dbo.FabricRelaxation fr on rr.FabricRelaxationID = fr.ID
            left join [ExtendServer].ManufacturingExecution.dbo.MachineIoT MIOT with (nolock) on MIOT.Ukey = fur.MachineIoTUkey and MIOT.MachineIoTType= 'unroll'
            inner join dbo.FtyInventory fi WITH (NOLOCK) on fi.POID = fur.poid 
                                                            and fi.seq1 = fur.seq1 
                                                            and fi.seq2 = fur.SEQ2 
                                                            and fi.Roll = fur.Roll 
                                                            and fi.Dyelot = fur.Dyelot 
                                                            and fi.StockType = fur.StockType
            where 
            1=1
            {this.sqlWhere}
            order by fur.POID, fur.Seq1, fur.Seq2, fur.Roll, fur.Dyelot";

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), this.listSqlPara, out this.printData);
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
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            this.ShowWaitMessage("Excel Processing...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R47.xltx"); // 預先開啟excel app

            MyUtility.Excel.CopyToXls(this.printData, null, "Warehouse_R47.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R47");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
