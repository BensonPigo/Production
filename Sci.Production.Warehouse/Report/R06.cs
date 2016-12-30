using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        string reason, factory, stocktype, fabrictype, mdivisionid, shift;
        int ordertypeindex;
        DateTime? issuedate1, issuedate2, approveDate1, approveDate2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();

        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
            txtfactory1.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(cbbFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            cbbFabricType.SelectedIndex = 0;
            txtdropdownlist1.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange2.Value1))
            {
                MyUtility.Msg.WarningBox("< Issue date > , < Approve date > can't be empty!!");
                return false;
            }

            issuedate1 = dateRange1.Value1;
            issuedate2 = dateRange1.Value2;
            approveDate1 = dateRange2.Value1;
            approveDate2 = dateRange2.Value2;
            fabrictype = cbbFabricType.SelectedValue.ToString();
            mdivisionid = txtMdivision1.Text;
            factory = txtfactory1.Text;
            shift = txtdropdownlist1.SelectedValue.ToString();

            condition.Clear();
            condition.Append(string.Format(@"Issue Date : {0} ~ {1}" + "   "
                , Convert.ToDateTime(issuedate1).ToString("d")
                , Convert.ToDateTime(issuedate2).ToString("d")));
            condition.Append(string.Format(@"Approve Date : {0} ~ {1}" + "   "
                , Convert.ToDateTime(approveDate1).ToString("d")
                , Convert.ToDateTime(approveDate2).ToString("d")));
            condition.Append(string.Format(@"M : {0}" + "   "
                , txtMdivision1.Text));
            condition.Append(string.Format(@"Factory : {0}" + "   "
                , txtfactory1.Text));
            condition.Append(string.Format(@"Shift : {0}" + "   "
                , txtdropdownlist1.Text));
            condition.Append(string.Format(@"Fabric Type : {0}"
                , cbbFabricType.Text));

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@mdivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"SELECT a.MDivisionID,a.factoryid,a.OrderID
,(select styleid from dbo.orders where id = a.orderid) style
,a.id, (select SewingCell from dbo.SewingLine where id= a.SewingLineID and FactoryID = a.FactoryID) cell
,a.SewingLineID
,b.seq1,b.seq2
,c.Refno
,(select t.MtlTypeID from dbo.fabric t where t.SCIRefno = c.SCIRefno) content
,dbo.getMtlDesc(c.id,c.seq1,c.seq2,2,0) [description]
,c.SizeSpec
,c.ColorID
,b.WhseInQty
--,c.POUnit,c.StockUnit
,(isnull(c.NETQty,0)+isnull(c.LossQty,0))
    * (select v.RateValue from dbo.View_Unitrate v where FROM_U = c.POUnit and TO_U = c.StockUnit) productionQty
,b.FTYInQty
,b.RequestQty
,iif(a.type='L','Lacking','Replacement') sisType
,(select PPICReason.Description from dbo.PPICReason where type= iif(a.type='L','FL','AL') and PPICReason.ID = b.PPICReasonID) reason
,b.IssueQty
,a.IssueLackDT
,a.ApvDate
,iif(a.Status ='Received',a.EditDate,null) servedDate
,dbo.getPass1(a.ApplyName) handle
FROM Lack a
inner join Lack_detail b on a.id = b.id
inner join po_supp_detail c on c.ID = a.poid and c.seq1 = B.Seq1 AND C.SEQ2 = B.Seq2
where (a.Status ='Received' or a.Status = 'Confirmed') "));

            #region --- 條件組合  ---

            if (!MyUtility.Check.Empty(issuedate1))
            {
                sqlCmd.Append(string.Format(@" and a.issuedate between '{0}' and '{1}'"
                , Convert.ToDateTime(issuedate1).ToString("d")
                 , Convert.ToDateTime(issuedate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(approveDate1))
            {
                sqlCmd.Append(string.Format(@" and a.apvdate between '{0}' and '{1}'"
                , Convert.ToDateTime(approveDate1).ToString("d")
                 , Convert.ToDateTime(approveDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(mdivisionid))
            {
                sqlCmd.Append(" and A.MDivisionid = @mdivision");
                sp_mdivision.Value = mdivisionid;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and A.factoryid = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(shift))
            {
                sqlCmd.Append(string.Format(@" and a.shift = '{0}'", shift));
            }

            if (!MyUtility.Check.Empty(fabrictype))
            {
                sqlCmd.Append(string.Format(@" and c.fabrictype = '{0}'", fabrictype));
            }

            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
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
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R06.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R06.xltx", 3, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = condition.ToString();   // 條件字串寫入excel
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
