using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R06 : Win.Tems.PrintForm
    {
        private string factory;
        private string fabrictype;
        private string mdivisionid;
        private string shift;
        private DateTime? requestdate1;
        private DateTime? requestdate2;
        private DateTime? issuedate1;
        private DateTime? issuedate2;
        private DateTime? approveDate1;
        private DateTime? approveDate2;
        private DataTable printData;
        private StringBuilder condition = new StringBuilder();

        /// <inheritdoc/>
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
            this.txtfactory.Text = Env.User.Factory;
            MyUtility.Tool.SetupCombox(this.comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            this.comboFabricType.SelectedIndex = 0;
            this.txtdropdownlistShift.SelectedIndex = 0;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2) &&
                MyUtility.Check.Empty(this.dateApproveDate.Value1) && MyUtility.Check.Empty(this.dateApproveDate.Value2) &&
                MyUtility.Check.Empty(this.dateRequest.Value1) && MyUtility.Check.Empty(this.dateRequest.Value2)
                )
            {
                MyUtility.Msg.WarningBox("< Issue date > , < Approve date > , <Request date> can't be empty!!");
                return false;
            }

            this.requestdate1 = this.dateRequest.Value1;
            this.requestdate2 = this.dateRequest.Value2;
            this.issuedate1 = this.dateIssueDate.Value1;
            this.issuedate2 = this.dateIssueDate.Value2;
            this.approveDate1 = this.dateApproveDate.Value1;
            this.approveDate2 = this.dateApproveDate.Value2;
            this.fabrictype = this.comboFabricType.SelectedValue.ToString();
            this.mdivisionid = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.shift = this.txtdropdownlistShift.SelectedValue.ToString();

            this.condition.Clear();
            this.condition.Append(string.Format(
                @"Issue Date : {0} ~ {1}" + "   ",
                Convert.ToDateTime(this.issuedate1).ToString("yyyy/MM/dd"),
                Convert.ToDateTime(this.issuedate2).ToString("yyyy/MM/dd")));
            this.condition.Append(string.Format(
                @"Approve Date : {0} ~ {1}" + "   ",
                Convert.ToDateTime(this.approveDate1).ToString("yyyy/MM/dd"),
                Convert.ToDateTime(this.approveDate2).ToString("yyyy/MM/dd")));
            this.condition.Append(string.Format(
                @"M : {0}" + "   ",
                this.txtMdivision.Text));
            this.condition.Append(string.Format(
                @"Factory : {0}" + "   ",
                this.txtfactory.Text));
            this.condition.Append(string.Format(
                @"Shift : {0}" + "   ",
                this.txtdropdownlistShift.Text));
            this.condition.Append(string.Format(
                @"Material Type : {0}",
                this.comboFabricType.Text));

            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@mdivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
SELECT
        a.factoryid
        ,a.OrderID
        ,style = (select styleid from dbo.orders WITH (NOLOCK) where id = a.orderid) 
        ,a.id
        ,cell = (select SewingCell from dbo.SewingLine WITH (NOLOCK) where id= a.SewingLineID and FactoryID = a.FactoryID) 
        ,a.SewingLineID
        ,seq = concat(b.seq1, ' ', b.seq2)
        , psd.Refno
        ,content = (select t.MtlTypeID from dbo.fabric t WITH (NOLOCK) where t.SCIRefno =  psd.SCIRefno) 
        ,[description] = dbo.getMtlDesc( psd.id, psd.seq1, psd.seq2,2,0) 
        , SizeSpec= isnull(psdsS.SpecValue, '')
        , ColorID = isnull(psdsC.SpecValue, '')
        ,b.FTYInQty
        ,productionQty = Round(dbo.GetUnitQty( psd.POUnit,  psd.StockUnit, (isnull( psd.NETQty,0)+isnull( psd.LossQty,0))), 2)
        ,b.WhseInQty
        ,b.RequestQty
        ,sisType = iif(a.type='L','Lacking','Replacement') 
        ,reason = (select PPICReason.Description 
                   from dbo.PPICReason 
                   where type= iif(a.FabricType='F','FL','AL') and PPICReason.ID = b.PPICReasonID) 
        ,b.IssueQty
        ,a.IssueLackDT
        ,a.ApvDate
        ,servedDate = iif(a.Status ='Received',a.EditDate,null) 
        ,handle = dbo.getPass1(a.ApplyName) 
        ,[CloseDate] = iif(il.status = 'Closed',il.editdate,null)
        ,[MTRtime] =  replace(iif(il.status = 'Closed',Str(Datediff(DAY,il.ApvDate,il.editdate)) + 'D' +  Str(Datediff(HOUR,il.ApvDate,il.editdate) % 24) +'H' + Str(Datediff(Minute,il.ApvDate,il.editdate) % 60) + 'M',null),' ','')
        , il.Remark
FROM Lack a WITH (NOLOCK) 
inner join Lack_detail b WITH (NOLOCK) on a.id = b.id
inner join po_supp_detail psd WITH (NOLOCK) on  psd.ID = a.poid and  psd.seq1 = B.Seq1 AND  psd.SEQ2 = B.Seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join IssueLack il WITH (NOLOCK) on a.IssueLackId = il.id
where (a.Status ='Received' or a.Status = 'Confirmed')
");

            #region --- 條件組合  ---

            if (!MyUtility.Check.Empty(this.requestdate1) || !MyUtility.Check.Empty(this.requestdate2))
            {
                if (!MyUtility.Check.Empty(this.requestdate1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= a.issuedate", Convert.ToDateTime(this.requestdate1).ToString("yyyy/MM/dd")));
                }

                if (!MyUtility.Check.Empty(this.requestdate2))
                {
                    sqlCmd.Append(string.Format(@" and a.issuedate <= '{0}'", Convert.ToDateTime(this.requestdate2).ToString("yyyy/MM/dd")));
                }
            }

            if (!MyUtility.Check.Empty(this.issuedate1) || !MyUtility.Check.Empty(this.issuedate2))
            {
                if (!MyUtility.Check.Empty(this.issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= il.issuedate", Convert.ToDateTime(this.issuedate1).ToString("yyyy/MM/dd")));
                }

                if (!MyUtility.Check.Empty(this.issuedate2))
                {
                    sqlCmd.Append(string.Format(@" and il.issuedate <= '{0}'", Convert.ToDateTime(this.issuedate2).ToString("yyyy/MM/dd")));
                }
            }

            if (!MyUtility.Check.Empty(this.approveDate1) || !MyUtility.Check.Empty(this.approveDate2))
            {
                if (!MyUtility.Check.Empty(this.approveDate1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= a.apvdate", Convert.ToDateTime(this.approveDate1).ToString("yyyy/MM/dd")));
                }

                if (!MyUtility.Check.Empty(this.approveDate2))
                {
                    sqlCmd.Append(string.Format(@" and a.apvdate <= '{0}'", Convert.ToDateTime(this.approveDate2).ToString("yyyy/MM/dd")));
                }
            }

            if (!MyUtility.Check.Empty(this.txtSP1) || !MyUtility.Check.Empty(this.txtSP2))
            {
                sqlCmd.Append(string.Format($@" and a.OrderID >= '{this.txtSP1.Text.PadRight(10, '0').ToString()}' and a.OrderID <= '{this.txtSP2.Text.PadRight(10, 'Z').ToString()}' "));
            }

            if (!MyUtility.Check.Empty(this.mdivisionid))
            {
                sqlCmd.Append(" and A.MDivisionid = @mdivision");
                sp_mdivision.Value = this.mdivisionid;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and A.factoryid = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.shift))
            {
                sqlCmd.Append(string.Format(@" and a.shift = '{0}'", this.shift));
            }

            if (!MyUtility.Check.Empty(this.fabrictype))
            {
                sqlCmd.Append(string.Format(@" and  psd.fabrictype = '{0}'", this.fabrictype));
            }

            sqlCmd.Append(string.Format(@" ORDER BY ApvDate "));
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R06.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R06.xltx", 4, showExcel: false, showSaveMsg: false, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format(
                @"
select  NameEN
from factory
where id = '{0}'", Env.User.Keyword));
            objSheets.Cells[2, 1] = @"Fabric/Accessory Lacking & Replacement Report";

            // Request Date (3, 2)
            objSheets.Cells[3, 2] = string.Format(
                @"{0} ~ {1}" + "   ",
              MyUtility.Check.Empty(this.requestdate1) ? string.Empty : Convert.ToDateTime(this.requestdate1).ToString("yyyy/MM/dd"),
              MyUtility.Check.Empty(this.requestdate2) ? string.Empty : Convert.ToDateTime(this.requestdate2).ToString("yyyy/MM/dd"));

            // Lacking Date (3, 7)
            objSheets.Cells[3, 7] = string.Format(
                @"{0} ~ {1}" + "   ",
              MyUtility.Check.Empty(this.issuedate1) ? string.Empty : Convert.ToDateTime(this.issuedate1).ToString("yyyy/MM/dd"),
              MyUtility.Check.Empty(this.issuedate2) ? string.Empty : Convert.ToDateTime(this.issuedate2).ToString("yyyy/MM/dd"));

            // Approve Date (3, 13)
            objSheets.Cells[3, 13] = string.Format(
                @"{0} ~ {1}" + "   ",
              MyUtility.Check.Empty(this.approveDate1) ? string.Empty : Convert.ToDateTime(this.approveDate1).ToString("yyyy/MM/dd"),
              MyUtility.Check.Empty(this.approveDate2) ? string.Empty : Convert.ToDateTime(this.approveDate2).ToString("yyyy/MM/dd"));

            // Shift (3, 18)
            objSheets.Cells[3, 18] = string.Format(
                @"{0}" + "   ",
                this.txtdropdownlistShift.Text);

            // Date (3, 24)
            objSheets.Cells[3, 24] = string.Format("{0:d}", DateTime.Now);

            this.ShowWaitMessage("Excel Processing...");
            for (int i = 1; i <= this.printData.Rows.Count; i++)
            {
                string str = objSheets.Cells[i + 4, 12].Value;
                if (!MyUtility.Check.Empty(str))
                {
                    objSheets.Cells[i + 4, 12] = str.Trim();
                }
            }

            objSheets.Columns.AutoFit();
            objSheets.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R06");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private void TxtMdivision_Validated(object sender, EventArgs e)
        {
            if (!this.txtMdivision.Text.EqualString(this.txtMdivision.OldValue))
            {
                this.txtfactory.Text = string.Empty;
            }
        }
    }
}
