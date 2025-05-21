using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R04 : Win.Tems.PrintForm
    {
        private string factory;
        private string brand;
        private string bulkFactory;
        private string stkFactory;
        private string operation;
        private string fabricType;
        private DateTime? cfmdate1;
        private DateTime? cfmdate2;
        private DataTable printData;
        private StringBuilder condition = new StringBuilder();

        /// <inheritdoc/>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboFabricType.Type = "Pms_FabricType";
        }

        // 驗證輸入條件

        /// <inheritdoc/>
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
            this.factory = this.txtStkfactory.Text;
            this.brand = this.txtbrand.Text;
            this.operation = this.txtdropdownlistOperation.SelectedValue.ToString();
            this.fabricType = this.comboFabricType.SelectedValue.ToString();
            this.bulkFactory = this.txtBulkfactory.Text;
            this.stkFactory = this.txtStkfactory.Text;

            this.condition.Clear();
            this.condition.Append(string.Format(
                @"Issue Date : {0} ~ {1}" + "   ",
                Convert.ToDateTime(this.cfmdate1).ToString("yyyy/MM/dd"),
                Convert.ToDateTime(this.cfmdate2).ToString("yyyy/MM/dd")));
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

        /// <inheritdoc/>
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
            sqlCmd.Append(@"
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
		,[Material Type]=concat(FabricType.Name, '-' + a.MtlTypeID)
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
                where Inventory.Ukey = a.InventoryUkey) 
        ,cuttingInline = (select min(cutting.CutForPlanningInLine) 
                          from Cutting WITH (NOLOCK) 
                          where id = a.InventoryPOID) 
        ,[Bulk Fty] = BulkFty.v
        ,Factory_ArrivedQty = iif((a.type='1' or a.type='4') ,e.InQty, 0.00) 
        ,productionQty = Round(dbo.GetUnitQty(d.POUnit, d.StockUnit, (isnull(d.NETQty,0.000) + isnull(d.LossQty,0.000))), 2)
        ,bulkBalance = case a.type
                            when '1' then e.InQty - e.OutQty + e.AdjustQty - e.ReturnQty - e.LInvQty + (select sum(iif(stocktype='B',outqty, 0-outqty)) 
                                                                                          from dbo.FtyInventory WITH (NOLOCK) 
                                                                                          where FtyInventory.POID = a.InventoryPOID 
                                                                                                and FtyInventory.Seq1 = a.InventorySeq1 and FtyInventory.seq2 = a.InventorySeq2 )
                            when '4' then e.InQty - e.OutQty + e.AdjustQty - e.ReturnQty - e.LInvQty + (select sum(iif(stocktype='B',outqty, 0-outqty)) 
                                                                                          from dbo.FtyInventory WITH (NOLOCK) 
                                                                                          where FtyInventory.POID = a.InventoryPOID 
                                                                                                and FtyInventory.Seq1 = a.InventorySeq1 and FtyInventory.seq2 = a.InventorySeq2 )
                            when '6' then e.InQty - e.OutQty + e.AdjustQty - e.ReturnQty - e.LInvQty
                            else 0.00
                       end 
        ,InventorySpSeq = a.InventoryPOID+'-'+a.InventorySeq1+'-'+a.InventorySeq2 
        ,[Stk Brand] = a.BrandID
        ,InventorySpSeason = Orders.SeasonID
        ,InventoryProjectID = (select ProjectID from dbo.orders WITH (NOLOCK) where id = a.InventoryPOID) 
        ,InventoryFactoryID = a.FactoryID
        ,MR_Input = Round(dbo.GetUnitQty(d.POUnit, d.StockUnit, d.InputQty), 2)
        ,InventoryInQty = (select sum(inqty) 
                           from ftyinventory WITH (NOLOCK) 
                           where poid = a.InventoryPOID and seq1 = a.InventorySeq1 
                                 and seq2 = a.InventorySeq2 and StockType ='I' ) 
        ,a.Refno
        ,ColorID = irsC.SpecValue
        ,SizeSpec = irsS.SpecValue
        ,[Qty] = iif (d.StockUnit is not null and d.StockUnit != ''
						, Round(dbo.GetUnitQty(a.UnitID, d.StockUnit, a.Qty), 2)
						, Round(dbo.GetUnitQty(a.UnitID, BulkPSD.StockUnit, a.Qty), 2))
		,[InventoryQty]=IIF((a.type='2' or a.type='6'), ISNULL( Inventory70.LInvQty ,0)  ,ISNULL( InventoryInv.LInvQty ,0) )
        ,[UnitID] = iif (d.StockUnit is not null and d.StockUnit != '', d.StockUnit, BulkPSD.StockUnit)
        ,e.BLocation
        ,reason = a.ReasonID +'-'+(select ReasonEN from InvtransReason WITH (NOLOCK) where id = a.ReasonID) 
        ,handle = dbo.getTPEPass1((select PoHandle 
									from Inventory WITH (NOLOCK) 
									where Inventory.Ukey = a.InventoryUkey))
from dbo.invtrans a WITH (NOLOCK) 
inner join InventoryRefno b WITH (NOLOCK) on b.id = a.InventoryRefnoId
left join PO_Supp_Detail d WITH (NOLOCK) on d.ID = a.InventoryPOID and d.SEQ1 = a.InventorySeq1 and d.seq2 =  a.InventorySeq2
left join Orders orders with (nolock) on d.id = orders.id
left join PO_Supp_Detail BulkPSD WITH (NOLOCK) on BulkPSD.ID = a.seq70poid and BulkPSD.SEQ1 = a.seq70seq1 and BulkPSD.seq2 =  a.seq70seq2
left join Orders BulkO WITH (NOLOCK) on a.seq70poid = BulkO.ID
left join Factory factory on orders.FactoryID = factory.id
left join MDivisionPoDetail e WITH (NOLOCK) on e.POID = A.InventoryPOID AND E.SEQ1 = A.InventorySeq1 AND E.Seq2 = A.InventorySeq2
left join InventoryRefno_Spec irsC on irsC.InventoryRefNoID = b.id and irsC.SpecColumnID = 'Color'
left join InventoryRefno_Spec irsS on irsS.InventoryRefNoID = b.id and irsS.SpecColumnID = 'Size'
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
outer apply (
    select v = 
            case when a.TransferFactory <> '' then a.TransferFactory 
                 when exists(select 1 from orders o where o.id = a.poid) then (select FactoryID from orders o where o.id = a.poid)
                 when exists(select 1 from orders o where o.id = a.InventoryPOID) then (select FactoryID from orders o where o.id = a.InventoryPOID)
                 else ''
            end
) BulkFty
where (orders.FactoryID in (select id from Factory) or BulkO.FactoryID in (select id from Factory))	 ");
            string whereStr = string.Empty;

            if (!MyUtility.Check.Empty(this.cfmdate1))
            {
                whereStr += string.Format(" AND '{0} 00:00:00.000' <= a.ConfirmDate ", Convert.ToDateTime(this.cfmdate1).ToString("yyyy/MM/dd"));
            }

            if (!MyUtility.Check.Empty(this.cfmdate2))
            {
                whereStr += string.Format(" AND a.ConfirmDate <= '{0} 23:59:59.999' ", Convert.ToDateTime(this.cfmdate2).ToString("yyyy/MM/dd"));
            }

            sqlCmd.Append(whereStr);
            #region --- 條件組合  ---

            if (!MyUtility.Check.Empty(this.bulkFactory))
            {
                sqlCmd.Append(" and (BulkFty.v  =  @Bulkfactory or BulkO.FtyGroup  =  @Bulkfactory)");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@Bulkfactory", this.bulkFactory));
            }

            if (!MyUtility.Check.Empty(this.stkFactory))
            {
                sqlCmd.Append(" and (a.FactoryID  =  @stkfactory or orders.FtyGroup  =  @stkfactory)");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@stkfactory", this.stkFactory));
            }

            //if (!MyUtility.Check.Empty(this.factory))
            //{
            //    sqlCmd.Append(" and (orders.FactoryID  =  @factory or a.TransferFactory =  @factory)");
            //    sp_factory.Value = this.factory;
            //    cmds.Add(sp_factory);
            //}

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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R04.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = this.condition.ToString();   // 條件字串寫入excel
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R04.xltx", 3, true, null, objApp);      // 將datatable copy to excel

            Marshal.ReleaseComObject(objSheets);
            return true;
        }

        private void TexBulkFactorytBox_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"Select DISTINCT FtyGroup as Factory from Production.dbo.Factory WITH (NOLOCK) where IsProduceFty = 1 order by FtyGroup";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "Factory", "10", string.Empty, "Factory");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtBulkfactory.Text = item.GetSelectedString();
        }

        private void TxtStkFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"Select DISTINCT FtyGroup as Factory from Production.dbo.Factory WITH (NOLOCK) where IsProduceFty = 1 order by FtyGroup";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "Factory", "10", string.Empty, "Factory");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtStkfactory.Text = item.GetSelectedString();
        }
    }
}
