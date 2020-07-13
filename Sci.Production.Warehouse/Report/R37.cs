using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R37 : Win.Tems.PrintForm
    {
        string factory;
        string spno1;
        string spno2;
        string Stocktype;
        string Reason;
        string mdivision;
        DateTime? issueDelivery1;
        DateTime? issueDelivery2;
        DataTable printData;

        public R37(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add(string.Empty, "ALL");
            comboBox1_RowSource.Add("B", "Bulk");
            comboBox1_RowSource.Add("I", "Inventory");
            this.comboStockType.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboStockType.ValueMember = "Key";
            this.comboStockType.DisplayMember = "Value";

            this.comboStockType.SelectedIndex = 0;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Combo DataSource設定
            DataTable dtReason;
            DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, @"
select [ReasonDesc]='ALL','' as id
union all
select [ReasonDesc]=id+'-'+Description,ID  from WhseReason where Type='RR'", out dtReason))
            {
                this.comboReason.DataSource = dtReason;
                this.comboReason.DisplayMember = "ReasonDesc";
                this.comboReason.ValueMember = "ID";
            }
            else
            {
                this.ShowErr(cbResult);
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            this.issueDelivery1 = this.dateIssueDelivery.Value1;
            this.issueDelivery2 = this.dateIssueDelivery.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.Stocktype = this.comboStockType.SelectedValue.ToString();
            this.Reason = this.comboReason.SelectedValue.ToString();

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region -- sql parameters declare --
            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_stocktype = new System.Data.SqlClient.SqlParameter();
            sp_stocktype.ParameterName = "@stocktype";

            System.Data.SqlClient.SqlParameter sp_reason = new System.Data.SqlClient.SqlParameter();
            sp_reason.ParameterName = "@reason";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion
            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.Append(@"
select 
 rr.Id
, rr.IssueDate
, [Refund_Reason] = rr.WhseReasonId+wr.Description 
, [Action] = rr.ActionID + wr1.Description
, rrd.poid	
, rrd.Seq1
, rrd.Seq2
, rrd.Roll
, rrd.Dyelot
, [Description] = dbo.getMtlDesc(rrd.poid,rrd.seq1,rrd.seq2,2,0)
, [Unit]= dbo.GetStockUnitBySPSeq(rrd.POID,rrd.Seq1,rrd.Seq2)
, rrd.Qty
, [StockType]= case when rrd.StockType='B' then 'Bulk'
when rrd.StockType='I' then 'Inventory' else  rrd.StockType end
, [Location] = dbo.Getlocation(ft.ukey)
from ReturnReceipt rr with(nolock)
left join ReturnReceipt_Detail rrd with(nolock) on rr.Id=rrd.Id
left join WhseReason wr with(nolock) on rr.WhseReasonId=wr.ID and wr.Type='RR'
left join WhseReason wr1 with(nolock) on rr.ActionID=wr1.ID and wr1.Type='RA'
left join FtyInventory ft with(nolock) on rrd.POID=ft.POID and ft.Seq1=rrd.Seq1 and ft.Seq2=rrd.Seq2
where 1=1 ");

            #region 條件組合
            if (!MyUtility.Check.Empty(this.issueDelivery1))
            {
                sqlcmd.Append(string.Format(@" and '{0}' <= rr.issueDate ", Convert.ToDateTime(this.issueDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.issueDelivery2))
            {
                sqlcmd.Append(string.Format(@" and rr.issueDate <= '{0}' ", Convert.ToDateTime(this.issueDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.spno1) && !MyUtility.Check.Empty(this.spno2))
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlcmd.Append(" and rrd.poid >= @spno1 and rrd.poid <= @spno2 ");
                sp_spno1.Value = this.spno1.PadRight(10, '0');
                sp_spno2.Value = this.spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(this.spno1))
            {
                // 只有 sp1 輸入資料
                sqlcmd.Append(" and rrd.poid like @spno1 ");
                sp_spno1.Value = this.spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(this.spno2))
            {
                // 只有 sp2 輸入資料
                sqlcmd.Append(" and rrd.poid like @spno2 ");
                sp_spno2.Value = this.spno2 + "%";
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlcmd.Append(" and rr.mdivisionid = @MDivision ");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlcmd.Append(" and rr.FactoryID = @factory ");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.Stocktype))
            {
                sqlcmd.Append(" and rrd.StockType in ( @StockType) ");
                sp_stocktype.Value = this.Stocktype;
                cmds.Add(sp_stocktype);
            }

            if (!MyUtility.Check.Empty(this.Reason))
            {
                sqlcmd.Append(" and rr.WhseReasonId in ( @reason) ");
                sp_reason.Value = this.Reason;
                cmds.Add(sp_reason);
            }

            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlcmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Quary data fail\r\n" + result.ToString());
            }

            return Ict.Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (MyUtility.Check.Empty(this.printData) || this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數
            this.SetCount(this.printData.Rows.Count);
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R37.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R37.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R37");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();

            return true;
        }
    }
}
