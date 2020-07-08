using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R06 : Win.Tems.PrintForm
    {
        // string reason, factory, stocktype, fabrictype, mdivisionid, shift;
        // int ordertypeindex;
        string factory;

        // string reason, factory, stocktype, fabrictype, mdivisionid, shift;
        // int ordertypeindex;
        string fabrictype;

        // string reason, factory, stocktype, fabrictype, mdivisionid, shift;
        // int ordertypeindex;
        string mdivisionid;

        // string reason, factory, stocktype, fabrictype, mdivisionid, shift;
        // int ordertypeindex;
        string shift;

        DateTime? issuedate1;
        DateTime? issuedate2;
        DateTime? approveDate1;
        DateTime? approveDate2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();

        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            this.comboFabricType.SelectedIndex = 0;
            this.txtdropdownlistShift.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2) &&
                MyUtility.Check.Empty(this.dateApproveDate.Value2) && MyUtility.Check.Empty(this.dateApproveDate.Value1))
            {
                MyUtility.Msg.WarningBox("< Issue date > , < Approve date > can't be empty!!");
                return false;
            }

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
                Convert.ToDateTime(this.issuedate1).ToString("d"),
                Convert.ToDateTime(this.issuedate2).ToString("d")));
            this.condition.Append(string.Format(
                @"Approve Date : {0} ~ {1}" + "   ",
                Convert.ToDateTime(this.approveDate1).ToString("d"),
                Convert.ToDateTime(this.approveDate2).ToString("d")));
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
                @"Fabric Type : {0}",
                this.comboFabricType.Text));

            return base.ValidateInput();
        }

        // 非同步取資料
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
            sqlCmd.Append(string.Format(@"
SELECT  --a.MDivisionID,
        a.factoryid
        ,a.OrderID
        ,style = (select styleid from dbo.orders WITH (NOLOCK) where id = a.orderid) 
        ,a.id
        ,cell = (select SewingCell from dbo.SewingLine WITH (NOLOCK) where id= a.SewingLineID and FactoryID = a.FactoryID) 
        ,a.SewingLineID
        --,b.seq1,b.seq2
        ,seq = concat(b.seq1, ' ', b.seq2)
        ,c.Refno
        ,content = (select t.MtlTypeID from dbo.fabric t WITH (NOLOCK) where t.SCIRefno = c.SCIRefno) 
        ,[description] = dbo.getMtlDesc(c.id,c.seq1,c.seq2,2,0) 
        ,c.SizeSpec
        ,c.ColorID
        ,b.FTYInQty
        --,c.POUnit,c.StockUnit
        ,productionQty = Round(dbo.GetUnitQty(c.POUnit, c.StockUnit, (isnull(c.NETQty,0)+isnull(c.LossQty,0))), 2)
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
FROM Lack a WITH (NOLOCK) 
inner join Lack_detail b WITH (NOLOCK) on a.id = b.id
inner join po_supp_detail c WITH (NOLOCK) on c.ID = a.poid and c.seq1 = B.Seq1 AND C.SEQ2 = B.Seq2
left join IssueLack il WITH (NOLOCK) on a.IssueLackId = il.id
where (a.Status ='Received' or a.Status = 'Confirmed') "));

            #region --- 條件組合  ---

            if (!MyUtility.Check.Empty(this.issuedate1) || !MyUtility.Check.Empty(this.issuedate2))
            {
                if (!MyUtility.Check.Empty(this.issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= a.issuedate", Convert.ToDateTime(this.issuedate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.issuedate2))
                {
                    sqlCmd.Append(string.Format(@" and a.issuedate <= '{0}'", Convert.ToDateTime(this.issuedate2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.approveDate1) || !MyUtility.Check.Empty(this.approveDate2))
            {
                if (!MyUtility.Check.Empty(this.approveDate1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= a.apvdate", Convert.ToDateTime(this.approveDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.approveDate2))
                {
                    sqlCmd.Append(string.Format(@" and a.apvdate <= '{0}'", Convert.ToDateTime(this.approveDate2).ToString("d")));
                }
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
                sqlCmd.Append(string.Format(@" and c.fabrictype = '{0}'", this.fabrictype));
            }

            sqlCmd.Append(string.Format(@" ORDER BY ApvDate "));
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R06.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R06.xltx", 4, showExcel: false, showSaveMsg: false, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format(
                @"
select  NameEN
from factory
where id = '{0}'", Sci.Env.User.Keyword));
            objSheets.Cells[2, 1] = @"Fabric/Accessory Lacking & Replacement Report";

            // Lacking Date (3, 2)
            objSheets.Cells[3, 2] = string.Format(
                @"{0} ~ {1}" + "   ",
                Convert.ToDateTime(this.issuedate1).ToString("d"),
                Convert.ToDateTime(this.issuedate2).ToString("d"));

            // Approve Date (3, 4)
            objSheets.Cells[3, 4] = string.Format(
                @"{0} ~ {1}" + "   ",
                Convert.ToDateTime(this.approveDate1).ToString("d"),
                Convert.ToDateTime(this.approveDate2).ToString("d"));

            // Shift (3, 6)
            objSheets.Cells[3, 6] = string.Format(
                @"{0}" + "   ",
                this.txtdropdownlistShift.Text);

            // Date (3, 11)
            objSheets.Cells[3, 11] = string.Format("{0:d}", DateTime.Now);

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
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R06");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private void txtMdivision_Validated(object sender, EventArgs e)
        {
            if (!this.txtMdivision.Text.EqualString(this.txtMdivision.OldValue))
            {
                this.txtfactory.Text = string.Empty;
            }
        }
    }
}
