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
    public partial class R04 : Sci.Win.Tems.PrintForm
    {
        string reason, factory, brand, mdivisionid, operation;
        int ordertypeindex;
        DateTime? cfmdate1, cfmdate2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();

        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
            txtfactory1.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("< CFM date > can't be empty!!");
                return false;
            }

            cfmdate1 = dateRange1.Value1;
            cfmdate2 = dateRange1.Value2;
            mdivisionid = txtMdivision1.Text;
            factory = txtfactory1.Text;
            brand = txtbrand1.Text;
            operation = txtdropdownlist1.SelectedValue.ToString();

            condition.Clear();
            condition.Append(string.Format(@"Issue Date : {0} ~ {1}" + "   "
                , Convert.ToDateTime(cfmdate1).ToString("d")
                , Convert.ToDateTime(cfmdate2).ToString("d")));
            condition.Append(string.Format(@"M : {0}" + "   "
                , mdivisionid));
            condition.Append(string.Format(@"Factory : {0}" + "   "
                , brand));
            condition.Append(string.Format(@"Brand : {0}"
                , txtbrand1.Text));
            condition.Append(string.Format(@"Operation : {0}" + "   ", txtdropdownlist1.Text));

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

            System.Data.SqlClient.SqlParameter sp_brand = new System.Data.SqlClient.SqlParameter();
            sp_brand.ParameterName = "@brand";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select 
case a.type when '1' then 'Input' 
when '2' then 'Output' 
when '3' then 'Transfer' 
when '4' then 'Adjust' 
when '5' then 'Obsolete' 
when '6' then 'Return' end as operation
,a.ConfirmDate
,iif((a.type='2' or a.type='6') , a.seq70poid+'-'+a.seq70seq1+'-'+a.seq70seq2 ,a.InventoryPOID+'-'+a.InventorySeq1+'-'+a.InventorySeq2) bulkSP
,(select ProjectID from dbo.orders where id = iif((a.type='2' or a.type='6') , a.seq70poid 	,a.InventoryPOID)) bulkProjectID
,(select Category from dbo.orders where id = iif((a.type='2' or a.type='6') , a.seq70poid 	,a.InventoryPOID)) bulkCategory
,(select eta from Inventory where ukey = a.InventoryUkey) ETA
,(select min(cutting.CutInLine) from Cutting where id = a.InventoryPOID) cuttingInline
,(select FactoryID from dbo.orders where id = iif((a.type='2' or a.type='6') , a.seq70poid 	,a.InventoryPOID)) bulkFactory
,iif((a.type='1' or a.type='4') ,e.InQty,0.00) Factory_ArrivedQty
,(isnull(d.NETQty,0.000) + isnull(d.LossQty,0.000))* v.RateValue productionQty
,case a.type
when '1' then e.InQty - e.OutQty + e.AdjustQty - e.LInvQty + (select sum(iif(stocktype='B',outqty,0-outqty)) from dbo.FtyInventory where FtyInventory.MDivisionID = c.MDivisionID and FtyInventory.POID = a.InventoryPOID and FtyInventory.Seq1 = a.InventorySeq1 and FtyInventory.seq2 = a.InventorySeq2 )
when '4' then e.InQty - e.OutQty + e.AdjustQty - e.LInvQty + (select sum(iif(stocktype='B',outqty,0-outqty)) from dbo.FtyInventory where FtyInventory.MDivisionID = c.MDivisionID and FtyInventory.POID = a.InventoryPOID and FtyInventory.Seq1 = a.InventorySeq1 and FtyInventory.seq2 = a.InventorySeq2 )
when '6' then e.InQty - e.OutQty + e.AdjustQty - e.LInvQty
else 0.00
end bulkBalance
,a.InventoryPOID+'-'+a.InventorySeq1+'-'+a.InventorySeq2 InventorySpSeq
,(select ProjectID from dbo.orders where id = a.InventoryPOID) InventoryProjectID
,(select FactoryID from dbo.orders where id = a.InventoryPOID) InventoryFactoryID
,d.InputQty * v.RateValue MR_Input
,(select sum(inqty) from ftyinventory where ftyinventory.MDivisionID = c.MDivisionID and poid = a.InventoryPOID and seq1 = a.InventorySeq1 and seq2 = a.InventorySeq2 and StockType ='I' ) InventoryInQty
,a.Refno
,b.ColorID
,b.SizeSpec
,a.Qty* v.RateValue Qty
,a.UnitID
,a.ReasonID +'-'+(select ReasonEN from InvtransReason where id = a.ReasonID) reason
,dbo.getTPEPass1((select PoHandle from Inventory where Ukey = a.InventoryUkey)) handle
from dbo.invtrans a
inner join InventoryRefno b on b.id = a.InventoryRefnoId
inner join Factory c on c.id = a.FactoryID
inner join PO_Supp_Detail d on d.ID = a.InventoryPOID and d.SEQ1 = a.InventorySeq1 and d.seq2 =  a.InventorySeq2
inner join dbo.View_Unitrate v on v.FROM_U = d.POUnit and v.TO_U = d.StockUnit
left join MDivisionPoDetail e on e.MDivisionID = c.MDivisionID and e.POID = A.InventoryPOID AND E.SEQ1 = A.InventorySeq1 AND E.Seq2 = A.InventorySeq2
where a.ConfirmDate between '{0} 00:00:00.000' and '{1} 23:59:59.999'"
, Convert.ToDateTime(cfmdate1).ToString("d")
 , Convert.ToDateTime(cfmdate2).ToString("d")
 ));

            #region --- 條件組合  ---

            if (!MyUtility.Check.Empty(mdivisionid))
            {
                sqlCmd.Append(" and c.MDivisionid = @mdivision");
                sp_mdivision.Value = mdivisionid;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and (c.FTYGroup  =  @factory or a.TransferFactory =  @factory)");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(" and A.Brandid = @brand");
                sp_brand.Value = brand;
                cmds.Add(sp_brand);
            }

            if (!MyUtility.Check.Empty(operation))
            {
                sqlCmd.Append(string.Format(@" and a.type = '{0}'", operation.TrimEnd()));
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
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R04.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R04.xltx", 3, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = condition.ToString();   // 條件字串寫入excel
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
