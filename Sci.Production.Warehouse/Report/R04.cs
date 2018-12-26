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
        //string reason, factory, brand, mdivisionid, operation;
        //int ordertypeindex;
        string  factory, brand, mdivisionid, operation;
    
        DateTime? cfmdate1, cfmdate2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();

        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateCFMDate.Value1) && MyUtility.Check.Empty(dateCFMDate.Value2))
            {
                MyUtility.Msg.WarningBox("< CFM date > can't be empty!!");
                return false;
            }

            cfmdate1 = dateCFMDate.Value1;
            cfmdate2 = dateCFMDate.Value2;
            mdivisionid = txtMdivision.Text;
            factory = txtfactory.Text;
            brand = txtbrand.Text;
            operation = txtdropdownlistOperation.SelectedValue.ToString();

            condition.Clear();
            condition.Append(string.Format(@"Issue Date : {0} ~ {1}" + "   "
                , Convert.ToDateTime(cfmdate1).ToString("d")
                , Convert.ToDateTime(cfmdate2).ToString("d")));
            condition.Append(string.Format(@"M : {0}" + "   "
                , mdivisionid));
            condition.Append(string.Format(@"Factory : {0}" + "   "
                , factory));
            condition.Append(string.Format(@"Brand : {0}"
                , brand));
            condition.Append(string.Format(@"Operation : {0}" + "   ", operation));

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
            sqlCmd.Append(string.Format(@"
select  operation = case a.type 
                        when '1' then 'Input' 
                        when '2' then 'Output' 
                        when '3' then 'Transfer' 
                        when '4' then 'Adjust' 
                        when '5' then 'Obsolete' 
                        when '6' then 'Return' 
                        when 'R' then 'Recover' 
                    end 
        ,a.ConfirmDate
        ,bulkSP = iif((a.type='2' or a.type='6') , a.seq70poid+'-'+a.seq70seq1+'-'+a.seq70seq2 
                                                 , a.InventoryPOID+'-'+a.InventorySeq1+'-'+a.InventorySeq2) 
        ,bulkProjectID = (select ProjectID 
                          from dbo.orders WITH (NOLOCK) 
                          where id = iif((a.type='2' or a.type='6') , a.seq70poid    
                                                                    ,a.InventoryPOID)) 
        ,bulkCategory = (select Category 
                         from dbo.orders WITH (NOLOCK) 
                         where id = iif((a.type='2' or a.type='6') , a.seq70poid     
                                                                   , a.InventoryPOID)) 
        ,ETA = (select eta 
                from Inventory WITH (NOLOCK) 
                where Inventory.Ukey = a.InventoryUkey and Inventory.POID=d.ID and Inventory.Seq1=d.SEQ1 and Inventory.Seq2=d.SEQ2 and Inventory.MDivisionID=Factory.MDivisionID and (Inventory.FactoryID=orders.FactoryID or Inventory.FactoryID=a.TransferFactory ) and Inventory.UnitID=d.POUnit and Inventory.ProjectID=orders.ProjectID and Inventory.InventoryRefnoID=a.InventoryRefnoId) 
        ,cuttingInline = (select min(cutting.CutInLine) 
                          from Cutting WITH (NOLOCK) 
                          where id = a.InventoryPOID) 
        ,bulkFactory = (select FactoryID 
                        from dbo.orders WITH (NOLOCK) 
                        where id = iif((a.type='2' or a.type='6') , a.seq70poid    
                                                                  , a.InventoryPOID)) 
        ,Factory_ArrivedQty = iif((a.type='1' or a.type='4') ,e.InQty, 0.00) 
        ,productionQty = Round(dbo.GetUnitQty(d.POUnit, d.StockUnit, (isnull(d.NETQty,0.000) + isnull(d.LossQty,0.000))), 2)
        ,bulkBalance = case a.type
                            when '1' then e.InQty - e.OutQty + e.AdjustQty - e.LInvQty + (select sum(iif(stocktype='B',outqty, 0-outqty)) 
                                                                                          from dbo.FtyInventory WITH (NOLOCK) 
                                                                                          where FtyInventory.POID = a.InventoryPOID 
                                                                                                and FtyInventory.Seq1 = a.InventorySeq1 and FtyInventory.seq2 = a.InventorySeq2 )
                            when '4' then e.InQty - e.OutQty + e.AdjustQty - e.LInvQty + (select sum(iif(stocktype='B',outqty, 0-outqty)) 
                                                                                          from dbo.FtyInventory WITH (NOLOCK) 
                                                                                          where FtyInventory.POID = a.InventoryPOID 
                                                                                                and FtyInventory.Seq1 = a.InventorySeq1 and FtyInventory.seq2 = a.InventorySeq2 )
                            when '6' then e.InQty - e.OutQty + e.AdjustQty - e.LInvQty
                            else 0.00
                       end 
        ,InventorySpSeq = a.InventoryPOID+'-'+a.InventorySeq1+'-'+a.InventorySeq2 
        ,InventoryProjectID = (select ProjectID from dbo.orders WITH (NOLOCK) where id = a.InventoryPOID) 
        ,InventoryFactoryID = (select FactoryID from dbo.orders WITH (NOLOCK) where id = a.InventoryPOID) 
        ,MR_Input = Round(dbo.GetUnitQty(d.POUnit, d.StockUnit, d.InputQty), 2)
        ,InventoryInQty = (select sum(inqty) 
                           from ftyinventory WITH (NOLOCK) 
                           where poid = a.InventoryPOID and seq1 = a.InventorySeq1 
                                 and seq2 = a.InventorySeq2 and StockType ='I' ) 
        ,a.Refno
        ,b.ColorID
        ,b.SizeSpec
        ,Qty = Round(dbo.GetUnitQty(d.POUnit, d.StockUnit, a.Qty), 2)
        ,a.UnitID
        ,reason = a.ReasonID +'-'+(select ReasonEN from InvtransReason WITH (NOLOCK) where id = a.ReasonID) 
        ,handle = dbo.getTPEPass1((select PoHandle from Inventory WITH (NOLOCK) where Inventory.Ukey = a.InventoryUkey and Inventory.POID=d.ID and Inventory.Seq1=d.SEQ1 and Inventory.Seq2=d.SEQ2 and Inventory.MDivisionID=Factory.MDivisionID and (Inventory.FactoryID=orders.FactoryID or Inventory.FactoryID=a.TransferFactory ) and Inventory.UnitID=d.POUnit and Inventory.ProjectID=orders.ProjectID and Inventory.InventoryRefnoID=a.InventoryRefnoId)) 
from dbo.invtrans a WITH (NOLOCK) 
inner join InventoryRefno b WITH (NOLOCK) on b.id = a.InventoryRefnoId
inner join PO_Supp_Detail d WITH (NOLOCK) on d.ID = a.InventoryPOID and d.SEQ1 = a.InventorySeq1 and d.seq2 =  a.InventorySeq2
inner join Orders orders on d.id = orders.id
inner join Factory factory on orders.FactoryID = factory.id
left join MDivisionPoDetail e WITH (NOLOCK) on e.POID = A.InventoryPOID AND E.SEQ1 = A.InventorySeq1 AND E.Seq2 = A.InventorySeq2
where "
 ));
            string whereStr = "";

            if(!MyUtility.Check.Empty(cfmdate1))
                whereStr += string.Format(" '{0} 00:00:00.000' <= a.ConfirmDate ", Convert.ToDateTime(cfmdate1).ToString("d"));
            if(!MyUtility.Check.Empty(cfmdate2))
                whereStr += ((MyUtility.Check.Empty(whereStr)) ? "" : " and ") + string.Format(" a.ConfirmDate <= '{0} 23:59:59.999' ", Convert.ToDateTime(cfmdate2).ToString("d"));

            sqlCmd.Append(whereStr);
            #region --- 條件組合  ---

            if (!MyUtility.Check.Empty(mdivisionid))
            {
                sqlCmd.Append(" and factory.MDivisionid = @mdivision");
                sp_mdivision.Value = mdivisionid;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and (orders.FactoryID  =  @factory or a.TransferFactory =  @factory)");
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
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = condition.ToString();   // 條件字串寫入excel
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R04.xltx", 3, true, null, objApp);      // 將datatable copy to excel

            Marshal.ReleaseComObject(objSheets);
            return true;
        }

        private void txtMdivision_Validated(object sender, EventArgs e)
        {
            if (!txtMdivision.Text.EqualString(txtMdivision.OldValue))
            {
                this.txtfactory.Text = "";
            }
        }
    }
}
