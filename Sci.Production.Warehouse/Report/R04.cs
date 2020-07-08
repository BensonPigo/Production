using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R04 : Win.Tems.PrintForm
    {
        // string reason, factory, brand, mdivisionid, operation;
        // int ordertypeindex;
        string factory;

        // string reason, factory, brand, mdivisionid, operation;
        // int ordertypeindex;
        string brand;

        // string reason, factory, brand, mdivisionid, operation;
        // int ordertypeindex;
        string mdivisionid;

        // string reason, factory, brand, mdivisionid, operation;
        // int ordertypeindex;
        string operation;

        // string reason, factory, brand, mdivisionid, operation;
        // int ordertypeindex;
        string fabricType;

        DateTime? cfmdate1;
        DateTime? cfmdate2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();

        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.comboFabricType.Type = "Pms_FabricType";
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if ((MyUtility.Check.Empty(this.dateCFMDate.Value1) && MyUtility.Check.Empty(this.dateCFMDate.Value2))
                &&
                (MyUtility.Check.Empty(this.txtSpStart.Text) && MyUtility.Check.Empty(this.txtSpStart.Text)))
            {
                MyUtility.Msg.WarningBox("< CFM date > and < Bulk SP > can't be all empty!!");
                return false;
            }

            this.cfmdate1 = this.dateCFMDate.Value1;
            this.cfmdate2 = this.dateCFMDate.Value2;
            this.mdivisionid = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.brand = this.txtbrand.Text;
            this.operation = this.txtdropdownlistOperation.SelectedValue.ToString();
            this.fabricType = this.comboFabricType.SelectedValue.ToString();

            this.condition.Clear();
            this.condition.Append(string.Format(
                @"Issue Date : {0} ~ {1}" + "   ",
                Convert.ToDateTime(this.cfmdate1).ToString("d"),
                Convert.ToDateTime(this.cfmdate2).ToString("d")));
            this.condition.Append(string.Format(
                @"M : {0}" + "   ",
                this.mdivisionid));
            this.condition.Append(string.Format(
                @"Factory : {0}" + "   ",
                this.factory));
            this.condition.Append(string.Format(
                @"Brand : {0}",
                this.brand));
            this.condition.Append(string.Format(@"Operation : {0}" + "   ", this.operation));

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
		,[OrderType]=orders.OrderTypeID
		,[FabricType]=FabricType.Name
        ,bulkProjectID = (select ProjectID 
                          from dbo.orders WITH (NOLOCK) 
                          where id = iif((a.type='2' or a.type='6') , a.seq70poid    
                                                                    ,a.InventoryPOID)) 
        --,bulkCategory = (select Category 
        --                 from dbo.orders WITH (NOLOCK) 
        --                 where id = iif((a.type='2' or a.type='6') , a.seq70poid     
        --                                                           , a.InventoryPOID)) 
		,bulkCategory = Category.Name

        ,ETA = (select eta 
                from Inventory WITH (NOLOCK) 
                where Inventory.Ukey = a.InventoryUkey and Inventory.POID=d.ID and Inventory.Seq1=d.SEQ1 and Inventory.Seq2=d.SEQ2 and Inventory.MDivisionID=Factory.MDivisionID and (Inventory.FactoryID=orders.FactoryID or Inventory.FactoryID=a.TransferFactory ) and Inventory.UnitID=d.POUnit and Inventory.ProjectID=orders.ProjectID and Inventory.InventoryRefnoID=a.InventoryRefnoId) 
        ,cuttingInline = (select min(cutting.CutInLine) 
                          from Cutting WITH (NOLOCK) 
                          where id = a.InventoryPOID) 
        ,bulkFactory = iif((a.type='2' or a.type='6') and BulkFty1.FactoryID is null, a.TransferFactory, BulkFty1.FactoryID)
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
		,[InventoryQty]=IIF((a.type='2' or a.type='6'), ISNULL( Inventory70.LInvQty ,0)  ,ISNULL( InventoryInv.LInvQty ,0) )
        ,a.UnitID
        ,e.BLocation
        ,reason = a.ReasonID +'-'+(select ReasonEN from InvtransReason WITH (NOLOCK) where id = a.ReasonID) 
        ,handle = dbo.getTPEPass1((select PoHandle from Inventory WITH (NOLOCK) where Inventory.Ukey = a.InventoryUkey and Inventory.POID=d.ID and Inventory.Seq1=d.SEQ1 and Inventory.Seq2=d.SEQ2 and Inventory.MDivisionID=Factory.MDivisionID and (Inventory.FactoryID=orders.FactoryID or Inventory.FactoryID=a.TransferFactory ) and Inventory.UnitID=d.POUnit and Inventory.ProjectID=orders.ProjectID and Inventory.InventoryRefnoID=a.InventoryRefnoId)) 
from dbo.invtrans a WITH (NOLOCK) 
inner join InventoryRefno b WITH (NOLOCK) on b.id = a.InventoryRefnoId
inner join PO_Supp_Detail d WITH (NOLOCK) on d.ID = a.InventoryPOID and d.SEQ1 = a.InventorySeq1 and d.seq2 =  a.InventorySeq2
inner join Orders orders on d.id = orders.id
inner join Factory factory on orders.FactoryID = factory.id
left join MDivisionPoDetail e WITH (NOLOCK) on e.POID = A.InventoryPOID AND E.SEQ1 = A.InventorySeq1 AND E.Seq2 = A.InventorySeq2
Outer APPLY(
    SELECT Name
    FROM DropDownList
    WHERE Type='Pms_FabricType' AND REPLACE(ID,'''','') = a.FabricType
)FabricType 
OUTER APPLY(
	SELECT Name
	FROM DropDownList
	WHERE Type='Pms_MtlCategory'   
	AND REPLACE(ID,'''','') =  (select Category 
								 from dbo.orders WITH (NOLOCK) 
								 where id = iif((a.type='2' or a.type='6') , a.seq70poid     
																		   , a.InventoryPOID)) 
)Category
Outer Apply(
	select FactoryID 
	from dbo.orders WITH (NOLOCK) 
	where id = iif((a.type='2' or a.type='6') , a.seq70poid    
												, a.InventoryPOID)
)BulkFty1
OUTER APPLY(
	SELECT LInvQty
	FROM MDivisionPoDetail
	WHERE POID=a.seq70poid  AND SEQ1=a.seq70seq1 AND SEQ2=a.seq70seq2
)Inventory70
OUTER APPLY(
	SELECT LInvQty
	FROM MDivisionPoDetail
	WHERE POID=a.InventoryPOID  AND SEQ1=a.InventorySeq1 AND SEQ2=a.InventorySeq2
)InventoryInv

where 1=1 "));
            string whereStr = string.Empty;

            if (!MyUtility.Check.Empty(this.cfmdate1))
            {
                whereStr += string.Format(" AND '{0} 00:00:00.000' <= a.ConfirmDate ", Convert.ToDateTime(this.cfmdate1).ToString("d"));
            }

            if (!MyUtility.Check.Empty(this.cfmdate2))
            {
                whereStr += string.Format(" AND a.ConfirmDate <= '{0} 23:59:59.999' ", Convert.ToDateTime(this.cfmdate2).ToString("d"));
            }

            sqlCmd.Append(whereStr);
            #region --- 條件組合  ---

            if (!MyUtility.Check.Empty(this.mdivisionid))
            {
                sqlCmd.Append(" AND factory.MDivisionid = @mdivision");
                sp_mdivision.Value = this.mdivisionid;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and (orders.FactoryID  =  @factory or a.TransferFactory =  @factory)");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(" and A.Brandid = @brand");
                sp_brand.Value = this.brand;
                cmds.Add(sp_brand);
            }

            if (!MyUtility.Check.Empty(this.operation))
            {
                sqlCmd.Append(string.Format(@" AND a.type = '{0}'", this.operation.TrimEnd()));
            }

            // 如果Type = 2 或 6 ，會抓Seq70 Poid這個欄位放在 [bulkSP] 顯示
            if (!MyUtility.Check.Empty(this.txtSpStart.Text))
            {
                if (this.operation.TrimEnd() == "2" || this.operation.TrimEnd() == "6")
                {
                    sqlCmd.Append($@" AND a.seq70poid >= '{this.txtSpStart.Text}' ");
                }
                else
                {
                    sqlCmd.Append($@" AND a.InventoryPOID >= '{this.txtSpStart.Text}' ");
                }
            }

            if (!MyUtility.Check.Empty(this.txtSpEnd.Text))
            {
                if (this.operation.TrimEnd() == "2" || this.operation.TrimEnd() == "6")
                {
                    sqlCmd.Append($@"AND a.seq70poid <= '{this.txtSpEnd.Text}' ");
                }
                else
                {
                    sqlCmd.Append($@"AND a.InventoryPOID <= '{this.txtSpEnd.Text}' ");
                }
            }

            if (!MyUtility.Check.Empty(this.fabricType))
            {
                sqlCmd.Append($@"AND a.FabricType IN ({this.fabricType}) ");
            }
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R04.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = this.condition.ToString();   // 條件字串寫入excel
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R04.xltx", 3, true, null, objApp);      // 將datatable copy to excel

            Marshal.ReleaseComObject(objSheets);
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
