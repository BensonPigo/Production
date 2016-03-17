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
    public partial class R14 : Sci.Win.Tems.PrintForm
    {
        string reason, factory, stocktype, fabrictype, ordertype;
        int ordertypeindex;
        DateTime? eta1, eta2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();

        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtfactory1.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(cbbFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            cbbFabricType.SelectedIndex = 0;
            txtdropdownlist1.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("< WK# ETA > can't be empty!!");
                return false;
            }

            eta1 = dateRange1.Value1;
            eta2 = dateRange1.Value2;
            ordertypeindex = txtdropdownlist1.SelectedIndex;
            fabrictype = cbbFabricType.SelectedValue.ToString();
            factory = txtfactory1.Text;
            switch (ordertypeindex)
            {
                case 0:
                    ordertype = "('B')";
                    break;
                case 1:
                    ordertype = "('S')";
                    break;
                case 2:
                    ordertype = "('M')";
                    break;
                case 3:
                    ordertype = "('B','S')";
                    break;
                case 4:
                    ordertype = "('B','S')";
                    break;
                case 5:
                    ordertype = "('B','S','M')";
                    break;
            }
            condition.Clear();
            condition.Append(string.Format(@"ETA : {0} ~ {1}" + Environment.NewLine
                , Convert.ToDateTime(eta1).ToString("d")
                , Convert.ToDateTime(eta2).ToString("d")));
            condition.Append(string.Format(@"Fabric Type : {0}" + Environment.NewLine
                , cbbFabricType.Text));
            condition.Append(string.Format(@"Factory : {0}" + Environment.NewLine
                ,txtfactory1.Text));
            condition.Append(string.Format(@"Order Type : {0}" + Environment.NewLine
                , txtdropdownlist1.Text));
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select 
b.id as wkno_a, 
b.poid as order_a, 
b.seq1 + b.seq2 as seq_a, 
b.refno ,
c.ColorID,
c.SizeSpec,
c.StockUnit,
(b.qty + b.foc)  * v.RateValue shipqty,
iif ((b.qty + b.foc)  * v.RateValue > isnull(x.qty,0),'V','') over1,
isnull(x.qty,0) received_qty,
iif ((b.qty + b.foc)  * v.RateValue < isnull(x.qty,0),'V','') over2
from dbo.export A INNER JOIN dbo.export_detail B ON B.ID = A.ID
inner join dbo.PO_Supp_Detail c on c.ID = b.PoID and c.seq1 = b.seq1 and c.seq2 =  b.seq2 
inner join dbo.View_Unitrate v on v.FROM_U = c.POUnit and v.TO_U = c.StockUnit
outer apply ( select sum(b1.StockQty) qty from dbo.Receiving a1 inner join dbo.Receiving_Detail b1 on b1.id = a1.Id
where a1.ExportId = a.id and b1.PoId = b.PoID and b1.seq1 = b.seq1 and b1.seq2 = b.seq2 and a1.Status = 'Confirmed') x
inner join dbo.orders d on d.id = b.poid
WHERE  A.Eta BETWEEN '{0}' and '{1}' and  D.Category in {2}
", Convert.ToDateTime(eta1).ToString("d")
 , Convert.ToDateTime(eta2).ToString("d")
 , ordertype));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and d.factoryid = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
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
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R14.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R14.xltx", 2, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = condition.ToString();   // 條件字串寫入excel
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
