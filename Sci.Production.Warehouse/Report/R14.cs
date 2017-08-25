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
        //string reason, factory, stocktype, fabrictype, ordertype;
        string  factory, fabrictype, ordertype;
        int ordertypeindex;
        DateTime? eta1, eta2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();

        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtfactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            comboFabricType.SelectedIndex = 0;
            txtdropdownlistOrderType.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateWKNoETA.Value1) && MyUtility.Check.Empty(dateWKNoETA.Value2))
            {
                MyUtility.Msg.WarningBox("< WK# ETA > can't be empty!!");
                return false;
            }

            eta1 = dateWKNoETA.Value1;
            eta2 = dateWKNoETA.Value2;
            ordertypeindex = txtdropdownlistOrderType.SelectedIndex;
            fabrictype = comboFabricType.SelectedValue.ToString();
            factory = txtfactory.Text;
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
                , comboFabricType.Text));
            condition.Append(string.Format(@"Factory : {0}" + Environment.NewLine
                , txtfactory.Text));
            condition.Append(string.Format(@"Order Type : {0}" + Environment.NewLine
                , txtdropdownlistOrderType.Text));
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
            sqlCmd.Append(string.Format(@"
select  d.FactoryID
        ,wkno_a = b.id 
        ,order_a = b.poid
        ,seq_a = b.seq1 + b.seq2 
        ,b.refno 
        ,c.ColorID
        ,c.SizeSpec
        ,c.StockUnit
        ,shipqty = v.ShipQty
        ,over1 = iif (v.ShipQty > isnull(x.qty,0),'V','') 
        ,received_qty = isnull(x.qty,0) 
        ,over2 = iif (v.ShipQty < isnull(x.qty,0),'V','') 
from dbo.export A WITH (NOLOCK) 
inner join dbo.export_detail B WITH (NOLOCK) ON B.ID = A.ID
inner join dbo.PO_Supp_Detail c WITH (NOLOCK) on c.ID = b.PoID and c.seq1 = b.seq1 and c.seq2 =  b.seq2 
outer apply (
    select ShipQty = Round(dbo.GetUnitQty(c.POUnit, c.StockUnit, (b.qty + b.foc)), 2)
) v
outer apply ( 
    select sum(b1.StockQty) qty 
    from dbo.Receiving a1 WITH (NOLOCK) 
    inner join dbo.Receiving_Detail b1 WITH (NOLOCK) on b1.id = a1.Id
    where a1.ExportId = a.id and b1.PoId = b.PoID and b1.seq1 = b.seq1 and b1.seq2 = b.seq2 and a1.Status = 'Confirmed'
    ) x
inner join dbo.orders d WITH (NOLOCK) on d.id = b.poid
WHERE  D.Category in {0}", ordertype));

            if (!MyUtility.Check.Empty(eta1))
                sqlCmd.Append(string.Format(" and '{0}' <= a.eta", Convert.ToDateTime(eta1).ToString("d")));
            if (!MyUtility.Check.Empty(eta2))
                sqlCmd.Append(string.Format(" and a.eta <= '{0}'", Convert.ToDateTime(eta2).ToString("d")));
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

            sqlCmd.Append(" order by b.id, b.poid, b.seq1, b.seq2, b.refno, c.colorid, c.sizeSpec, c.stockunit");
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
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = condition.ToString();   // 條件字串寫入excel
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R14.xltx", 2, true, null, objApp);      // 將datatable copy to excel

            Marshal.ReleaseComObject(objSheets);
            return true;
        }
    }
}
