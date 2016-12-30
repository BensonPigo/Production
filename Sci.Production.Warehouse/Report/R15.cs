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
    public partial class R15 : Sci.Win.Tems.PrintForm
    {
        string reason, mdivision, stocktype, spno1, spno2;
        DateTime? issueDate1, issueDate2;
        DataTable printData;

        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) ) 
            {
                MyUtility.Msg.WarningBox("< Issue Date > can't be empty!!");
                return false;
            }

            issueDate1 = dateRange1.Value1;
            issueDate2 = dateRange1.Value2;
            mdivision = txtMdivision1.Text;
            reason = txtwhseReason1.TextBox1.Text;
            spno1 = txtSpno1.Text;
            spno2 = txtSpno2.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select a.MDivisionID, a.Id,a.IssueDate,b.POID,b.seq1
,b.seq2,b.roll,b.dyelot,c.Refno,c.ColorID,c.SizeSpec
,iif(c.FabricType='F','Fabric',iif(c.FabricType='A','Accessory',c.fabrictype)) fabrictype
,b.Qty,c.StockUnit,dbo.getPass1(a.EditName) editname,a.Remark
,a.WhseReasonID+'-'+ISNULL((select d.Description from whsereason d WHERE d.id = a. whsereasonid),'')
from issue as a
inner join issue_detail b on a.id = b.id
inner join po_supp_detail c on c.id = b.poid and c.seq1 = b.seq1 and c.seq2 =b.seq2
where a.type = 'D' AND a.Status = 'Confirmed' and a.issuedate between '{0}' and '{1}'
", Convert.ToDateTime(issueDate1).ToString("d")
 , Convert.ToDateTime(issueDate2).ToString("d")
 , stocktype));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(reason))
            {
                sqlCmd.Append(string.Format(@" and A.WhseReasonID = '{0}'", reason));
            }

            if (!MyUtility.Check.Empty(spno1))
            {
                sqlCmd.Append(" and b.poid >= @spno1 and b.poid <= @spno2");
                sp_spno1.Value = spno1;
                sp_spno2.Value = spno2;
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R15.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R15.xltx", 2, true, null, objApp);
            objApp.Columns.AutoFit();
            objApp.Rows.AutoFit();

            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
