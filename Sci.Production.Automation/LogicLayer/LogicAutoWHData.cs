using Ict;
using Sci.Data;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation.LogicLayer
{
    /// <inheritdoc/>
    public class LogicAutoWHData
    {
        /// PMS的action對應廠商statusAPI: Confrim(New),Unconfrim(Delete),Delete(Delete),Update(Revise)
        /// <param name="dtDetail">表身資訊,需要有ukey</param>
        /// <param name="formName">P10...P99</param>
        /// <param name="statusAPI">給廠商的動作指令 New/Delete/Revise/Lock/Unlock</param>
        /// <param name="action">PMS 的操作 Confrim, Unconfrim (P99) Delete, Update</param>
        /// <param name="fabricType">F/A</param>
        /// <inheritdoc/>
        public static DataTable GetWHData(DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, string fabricType, bool fromNewBarcode = false, bool isP99 = false)
        {
            string sqlcmd = GetWHDataSqlcmd(dtDetail, formName, statusAPI, action, fabricType, fromNewBarcode, isP99);
            DataTable dtMaster;
            DualResult result;
            if (isP99)
            {
                WHTableName detailTableName = Prgs.GetWHDetailTableName(formName);
                DataTable copyDetail = dtDetail.Copy();

                // Issue 部分程式第 2 層是 Issue_Summary,第3層才是 Issue_Detail
                if (copyDetail.Columns.Contains("Issue_DetailUkey"))
                {
                    if (copyDetail.Columns.Contains("Ukey"))
                    {
                        copyDetail.Columns.Remove("Ukey");
                    }

                    copyDetail.Columns["Issue_DetailUkey"].ColumnName = "Ukey";
                }

                result = MyUtility.Tool.ProcessWithDatatable(copyDetail, string.Empty, sqlcmd, out dtMaster);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                }
            }
            else
            {
                result = DBProxy.Current.Select("Production", sqlcmd, out dtMaster);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                }
            }

            return dtMaster;
        }

        /// <inheritdoc/>
        public static string GetWHDataSqlcmd(DataTable dtDetail, string formName, EnumStatus statusAPI, EnumStatus action, string fabricType, bool fromNewBarcode = false, bool isP99 = false)
        {
            string sqlcmd;
            string ukeys;
            if (dtDetail.Columns.Contains("Issue_DetailUkey"))
            {
                ukeys = dtDetail.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["Issue_DetailUkey"])).ToList().JoinToString(",");
            }
            else
            {
                ukeys = dtDetail.AsEnumerable().Select(row => MyUtility.Convert.GetString(row["Ukey"])).ToList().JoinToString(",");
            }

            string mainTable = Prgs.GetWHMainTableName(formName);
            WHTableName detailTableName = Prgs.GetWHDetailTableName(formName);
            string columns = string.Empty;
            string otherTables = string.Empty;
            string psd_FtyDt = Prgs.GetWHjoinPSD_Fty(detailTableName);
            string whereWMS = string.Empty;
            int fromNewBarcodeBit = 0; // 影響傳給廠商 Barcode. 欄位 先註解起來 // fromNewBarcode ? 1 : 0;
            string headerAlias = isP99 ? "sd." : "s.";

            if (statusAPI == EnumStatus.New)
            {
                string whereIsWMSTo = string.Empty;
                if (detailTableName == WHTableName.SubTransfer_Detail || detailTableName == WHTableName.BorrowBack_Detail)
                {
                    whereIsWMSTo = $@"
    and ml.StockType = sd.FromStockType
	union all
	select 1 from MtlLocation ml 
	inner join dbo.SplitString(sd.ToLocation,',') sp on sp.Data = ml.ID and ml.StockType = sd.ToStockType
	where ml.IsWMS = 1";
                }

                switch (detailTableName)
                {
                    case WHTableName.ReturnReceipt_Detail:
                    case WHTableName.Issue_Detail:
                    case WHTableName.IssueLack_Detail:
                    case WHTableName.TransferOut_Detail:
                    case WHTableName.SubTransfer_Detail:
                    case WHTableName.BorrowBack_Detail:
                    case WHTableName.Adjust_Detail:
                    case WHTableName.RemoveC_Detail:
                    case WHTableName.Stocktaking_Detail:
                        whereWMS = $@"
and exists(
	select 1
	from FtyInventory_Detail fd 
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where fd.Ukey = f.Ukey
	and ml.IsWMS = 1
{whereIsWMSTo}
)";

                        break;
                    case WHTableName.LocationTrans_Detail:
                        whereWMS = $@"
and exists(
    select 1
	from MtlLocation ml
    inner join dbo.SplitString(sd.ToLocation, ',') sp on sp.Data = ml.ID
	where ml.IsWMS =1 
	union all
    select 1
	from MtlLocation ml
    inner join dbo.SplitString(sd.FromLocation, ',') sp on sp.Data = ml.ID
	where ml.IsWMS =1 
)";
                        break;
                }
            }
            else
            {
                whereWMS = Environment.NewLine + $" and sd.SentToWMS = 1 and sd.CompleteTime is null";
            }

            columns = $@"
    ,sd.Poid
    ,sd.Seq1
    ,sd.Seq2
    ,sd.Roll
    ,sd.Dyelot
    ,sd.StockType
    ,sd.SentToWMS
    ,sd.CompleteTime

    ,Fabric.WeaveTypeID
    ,[MtlType] = Fabric.MtlTypeID
    ,psd.Refno
    ,[SizeCode] = isnull(psdsS.SpecValue ,'')
";

            if (fabricType == "F")
            {
                if (detailTableName == WHTableName.LocalOrderReceiving_Detail ||
                    detailTableName == WHTableName.LocalOrderIssue_Detail ||
                    detailTableName == WHTableName.LocalOrderAdjust_Detail ||
                    detailTableName == WHTableName.LocalOrderLocationTrans_Detail)
                {
                    string strWhereWMS = string.Empty;

                    if (statusAPI == EnumStatus.New)
                    {
                        if (detailTableName == WHTableName.LocalOrderLocationTrans_Detail)
                        {
                            strWhereWMS = $@"
                            exists
                            (
                                select 1
	                            from MtlLocation ml
                                inner join dbo.SplitString(b.ToLocation, ',') sp on sp.Data = ml.ID
	                            where ml.IsWMS =1 
	                            union all
                                select 1
	                            from MtlLocation ml
                                inner join dbo.SplitString(b.FromLocation, ',') sp on sp.Data = ml.ID
	                            where ml.IsWMS =1 
                            ) and";
                        }
                        else if (detailTableName == WHTableName.LocalOrderReceiving_Detail)
                        {
                            strWhereWMS = string.Empty;
                        }
                        else
                        {
                            strWhereWMS = $@"                            
                            EXISTS
                            (
	                            SELECT 1 
	                            FROM MtlLocation ml
	                            INNER join LocalOrderInventory_Location loil ON ml.ID = loil.MtlLocationID
	                            WHERE loil.LocalOrderInventoryUkey = loi.Ukey and ml.IsWMS = 1
                            ) and";
                        }
                    }
                    else
                    {
                        strWhereWMS = $" b.SentToWMS = 1 and b.CompleteTime is null " + Environment.NewLine + " and ";
                    }

                    columns = string.Empty;
                    switch (detailTableName)
                    {
                        // 收料
                        case WHTableName.LocalOrderReceiving_Detail:
                            return sqlcmd = $@"
                            SELECT 
                            [ID] = b.ID,
                            [InvNo] = '',
                            [POID] = b.POID,
                            [Seq1] = b.Seq1,
                            [Seq2] = b.Seq2,
                            [WeaveType] = lom.WeaveType,
                            [Refno] = lom.Refno,
                            [Color] = lom.Color,
                            [Roll] = b.Roll,
                            [Dyelot] =b.Dyelot,
                            [StockUnit] = lom.Unit,
                            [StockQty] = b.Qty,
                            [PoUnit] = lom.Unit,
                            [ShipQty] = b.Qty,
                            [Weight] = b.[Weight],
                            [StockType] = 'B',
                            [Barcode] = IIF(wt.To_NewBarcodeSeq <> '',Concat (wt.To_NewBarcode, '-', wt.To_NewBarcodeSeq),wt.To_NewBarcode ),
                            [Ukey] = b.Ukey,
                            [IsInspection] = 0,
                            [ETA] = NULL,
                            [WhseArrival] = a.WhseArrival,
                            [Status] = 'New',
                            [CmdTime] = GETDATE()
                            FROM LocalOrderReceiving a
                            LEFT JOIN LocalOrderReceiving_Detail b ON a.id = b.ID
                            LEFT JOIN WHBarcodeTransaction wt WITH(NOLOCK) ON wt.[Function] = 'P70' AND wt.TransactionID = b.ID AND wt.TransactionUkey = b.UKey AND wt.Action = 'Confirm'
                            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = b.POID AND lom.Seq1 = b.Seq1 AND lom.Seq2 = b.Seq2
                            LEFT JOIN LocalOrderInventory loi WITH(NOLOCK) ON loi.POID = b.POID AND 
                                                                                loi.Seq1 = b.Seq1 AND 
                                                                                loi.Seq2 = b.Seq2 AND 
                                                                                loi.Roll = b.Roll AND
                                                                                loi.Dyelot = b.Dyelot AND
                                                                                loi.StockType = b.StockType
                            WHERE {strWhereWMS} lom.FabricType = 'F' and  b.Ukey IN ({ukeys})";

                        // 發料
                        case WHTableName.LocalOrderIssue_Detail:
                            return sqlcmd = $@"
                            SELECT 
                            [ID] = b.ID,
                            [Type] = 'P71',
                            [CutplanID] = '',
                            [EstCutDate] = NULL,
                            [SpreadingNoID] = '',
                            [POID] = b.POID,
                            [Seq1] = b.Seq1,
                            [Seq2] = b.Seq2,
                            [WeaveType] = lom.WeaveType,
                            [Roll] = b.Roll,
                            [Dyelot] =b.Dyelot,
                            [Barcode] = IIF(wt.From_OldBarcodeSeq <> '',Concat (wt.From_OldBarcode, '-', wt.From_OldBarcodeSeq),wt.From_OldBarcode ),
                            [NewBarcode] = IIF(wt.To_NewBarcodeSeq <> '',Concat (wt.To_NewBarcode, '-', wt.To_NewBarcodeSeq),wt.To_NewBarcode ),
                            [Description] = lom.[Desc],
                            [Qty] = b.Qty,
                            [Ukey] = b.Ukey,
                            [Status] = 'New',
                            [CmdTime] = GETDATE()
                            FROM LocalOrderIssue  a
                            LEFT JOIN  LocalOrderIssue_Detail  b ON a.id = b.ID
                            LEFT JOIN WHBarcodeTransaction wt WITH(NOLOCK) ON wt.[Function] = 'P71' AND wt.TransactionID = b.ID AND wt.TransactionUkey = b.UKey AND wt.Action = 'Confirm'
                            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = b.POID AND lom.Seq1 = b.Seq1 AND lom.Seq2 = b.Seq2
                            LEFT JOIN LocalOrderInventory loi WITH(NOLOCK) ON loi.POID = b.POID AND 
                                                                                loi.Seq1 = b.Seq1 AND 
                                                                                loi.Seq2 = b.Seq2 AND 
                                                                                loi.Roll = b.Roll AND
                                                                                loi.Dyelot = b.Dyelot AND
                                                                                loi.StockType = b.StockType      
                            WHERE 
                            {strWhereWMS}
                            lom.FabricType = 'F' and
                            b.Ukey IN ({ukeys})";

                        // 調整
                        case WHTableName.LocalOrderAdjust_Detail:
                            return sqlcmd = $@"
                            SELECT 
                            [ID] = b.ID,
                            [POID] = b.POID,
                            [Seq1] = b.Seq1,
                            [Seq2] = b.Seq2,
                            [WeaveType] = lom.WeaveType,
                            [Roll] = b.Roll,
                            [Dyelot] =b.Dyelot,
                            [StockType] = b.StockType,
                            [QtyBefore] =b.QtyBefore,
                            [QtyAfter] = b.QtyAfter,
                            [Barcode] = IIF((loi.InQty - loi.OutQty + loi.AdjustQty) > 0,
                                        IIF(wt.From_OldBarcodeSeq <> '', wt.From_OldBarcode + '-' + wt.From_OldBarcodeSeq, wt.From_OldBarcode),
                                        IIF(wt.From_NewBarcodeSeq <> '', wt.From_NewBarcode + '-' + wt.From_NewBarcodeSeq, wt.From_NewBarcode)),
                            [Ukey] = b.Ukey,
                            [Status] = 'New',
                            [CmdTime] = GETDATE()
                            FROM LocalOrderAdjust  a
                            LEFT JOIN  LocalOrderAdjust_Detail b ON a.id = b.ID
                            LEFT JOIN WHBarcodeTransaction wt WITH(NOLOCK) ON wt.[Function] = 'P72' AND wt.TransactionID = b.ID AND wt.TransactionUkey = b.UKey AND wt.Action = 'Confirm'
                            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = b.POID AND lom.Seq1 = b.Seq1 AND lom.Seq2 = b.Seq2
                            LEFT JOIN LocalOrderInventory loi WITH(NOLOCK) ON loi.POID = b.POID AND 
                                                                                loi.Seq1 = b.Seq1 AND 
                                                                                loi.Seq2 = b.Seq2 AND 
                                                                                loi.Roll = b.Roll AND
                                                                                loi.Dyelot = b.Dyelot AND
                                                                                loi.StockType = b.StockType                        
                            WHERE 
                            {strWhereWMS}
                            lom.FabricType = 'F' and
                            b.Ukey IN ({ukeys})";

                        // 調整庫存位置
                        case WHTableName.LocalOrderLocationTrans_Detail:
                            return sqlcmd = $@"
                            SELECT 
                            [ID] = b.ID,
                            [POID] = b.POID,
                            [Seq1] = b.Seq1,
                            [Seq2] = b.Seq2,
                            [WeaveType] = lom.WeaveType,
                            [Roll] = b.Roll,
                            [Dyelot] = b.Dyelot,
                            [Refno] = lom.Refno,
                            [Color] = lom.Color,
                            [FromLocation] = b.FromLocation,
                            [ToLocation] = b.ToLocation,
                            [Barcode] = IIF(loi.BarcodeSeq <> '', Concat (loi.Barcode, '-', loi.BarcodeSeq),loi.Barcode),
                            [Qty] = b.Qty,
                            [Description] = lom.[Desc],
                            [StockType] = b.StockType,
                            [Ukey] = b.Ukey,
                            [Status] = 'New',
                            [CmdTime] = GETDATE()
                            FROM LocalOrderLocationTrans a
                            LEFT JOIN  LocalOrderLocationTrans_Detail b ON a.id = b.ID
                            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = b.POID AND lom.Seq1 = b.Seq1 AND lom.Seq2 = b.Seq2
                            LEFT JOIN LocalOrderInventory loi WITH(NOLOCK) ON loi.POID = b.POID AND 
                                                                                loi.Seq1 = b.Seq1 AND 
                                                                                loi.Seq2 = b.Seq2 AND 
                                                                                loi.Roll = b.Roll AND
                                                                                loi.Dyelot = b.Dyelot AND
                                                                                loi.StockType = b.StockType
                            WHERE 
                            {strWhereWMS}
                            lom.FabricType = 'F' and
                            b.Ukey IN ({ukeys})";
                    }
                }
                else
                {
                    switch (detailTableName)
                {
                    // 收料
                    case WHTableName.Receiving_Detail:
                        columns += $@"
    ,[InvNo] = {headerAlias}InvNo
    ,[ETA] = {headerAlias}ETA
    ,[WhseArrival] = {headerAlias}WhseArrival
    ,sd.StockQty
    ,sd.StockUnit
    ,sd.PoUnit
    ,sd.ShipQty
    ,[Weight] = iif(sd.ActualWeight = 0, sd.Weight , sd.ActualWeight)
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit},0)
    ,[IsInspection] = convert(bit, 0)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
";
                        break;
                    case WHTableName.TransferIn_Detail:
                        columns += $@"
    ,[InvNo] = {headerAlias}InvNo
    ,[ETA] = null
    ,[WhseArrival] = {headerAlias}IssueDate
    ,{headerAlias}IssueDate
    ,StockQty = sd.Qty
    ,sd.Qty
    ,StockUnit = dbo.GetStockUnitBySPSeq(sd.POID, sd.Seq1, sd.Seq2)
    ,psd.PoUnit
    ,ShipQty = sd.Qty
    ,[Weight] = iif(sd.ActualWeight = 0, sd.Weight , sd.ActualWeight)
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit},0)
    ,[IsInspection] = convert(bit, 0)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
";
                        break;

                    // 轉料
                    case WHTableName.SubTransfer_Detail:
                    case WHTableName.BorrowBack_Detail:
                        columns = $@"
    ,sd.FromPOID
    ,sd.FromSeq1
    ,sd.FromSeq2
    ,sd.FromRoll
    ,sd.FromDyelot
    ,sd.FromStockType
    ,[FromLocation] = dbo.Getlocation(f.Ukey)
    ,[FromBarcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit},sd.Qty)
    ,sd.ToPOID
    ,sd.ToSeq1
    ,sd.ToSeq2
    ,sd.ToRoll
    ,sd.ToDyelot
    ,sd.ToStockType
    ,sd.ToLocation
    ,[ToBarcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'T', fto.Ukey, 0, {fromNewBarcodeBit},sd.Qty)
    ,sd.Qty
    ,sd.SentToWMS
    ,sd.CompleteTime

    ,psd.Refno
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
    ,Fabric.WeaveTypeID
";
                        if (detailTableName == WHTableName.SubTransfer_Detail)
                        {
                            columns += $@"
    , {headerAlias}Type";
                        }

                        break;

                    // 退料
                    case WHTableName.IssueReturn_Detail:
                        columns += $@"
    ,sd.Qty
    ,StockUnit = dbo.GetStockUnitBySpSeq (sd.POID, sd.seq1, sd.seq2)
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit},0)
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
";
                        break;

                    // 退料(廠商)
                    case WHTableName.ReturnReceipt_Detail:
                        columns += $@"
    ,sd.Qty
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit},0)
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
";
                        break;

                    // 發料
                    case WHTableName.Issue_Detail:
                    case WHTableName.IssueLack_Detail:
                    case WHTableName.TransferOut_Detail:
                        if (detailTableName == WHTableName.Issue_Detail)
                        {
                            otherTables = $@"
left join Production.dbo.Cutplan c on c.ID = {headerAlias}CutplanID
";
                            columns += $@"
    ,[CutPlanID] = isnull({headerAlias}CutplanID, '')
    ,[EstCutdate] = c.EstCutdate
    ,[SpreadingNoID] = isnull(c.SpreadingNoID,'')
";
                        }
                        else
                        {
                            columns += $@"
    ,[CutPlanID] = ''
    ,[EstCutdate] = Null
    ,[SpreadingNoID] = ''
";
                        }

                        columns += $@"
    ,[Type] = '{formName}'
    ,sd.Qty
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit},0)
    ,[NewBarcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'T', f.Ukey, 0, {fromNewBarcodeBit},0)
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
    ,f.Tone
";

                        break;

                    // 調整
                    case WHTableName.Adjust_Detail:
                        columns += $@"
    ,sd.QtyBefore
    ,sd.QtyAfter
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey,
	    isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) {(action == EnumStatus.Confirm ? "-" : "+")} (isnull(sd.QtyAfter, 0) - isnull(sd.QtyBefore, 0))-- 帶入調整前的balanceQty
        , {fromNewBarcodeBit}
        ,0)
";
                        break;

                    // 盤點
                    case WHTableName.Stocktaking_Detail:
                        columns += $@"
    ,sd.QtyBefore
    ,f.Barcode
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
";
                        break;

                    case WHTableName.LocationTrans_Detail:
                        columns += $@"
    ,sd.FromLocation
    ,sd.ToLocation
    ,f.Barcode
    ,[Qty] = isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
";
                        break;
                }
                }
            }

            if (fabricType == "A")
            {
                if (detailTableName == WHTableName.LocalOrderIssue_Detail ||
                    detailTableName == WHTableName.LocalOrderReceiving_Detail ||
                    detailTableName == WHTableName.LocalOrderAdjust_Detail ||
                    detailTableName == WHTableName.LocalOrderLocationTrans_Detail)
                {
                    string strWhereWMS = string.Empty;

                    if (statusAPI == EnumStatus.New)
                    {
                        if (detailTableName == WHTableName.LocalOrderLocationTrans_Detail)
                        {
                            strWhereWMS = $@"
                            exists
                            (
                                select 1
	                            from MtlLocation ml
                                inner join dbo.SplitString(b.ToLocation, ',') sp on sp.Data = ml.ID
	                            where ml.IsWMS =1 
	                            union all
                                select 1
	                            from MtlLocation ml
                                inner join dbo.SplitString(b.FromLocation, ',') sp on sp.Data = ml.ID
	                            where ml.IsWMS =1 
                            ) and";
                        }
                        else if (detailTableName == WHTableName.LocalOrderReceiving_Detail)
                        {
                            strWhereWMS = string.Empty;
                        }
                        else
                        {
                            strWhereWMS = $@"                            
                            EXISTS
                            (
	                            SELECT 1 
	                            FROM MtlLocation ml
	                            INNER join LocalOrderInventory_Location loil ON ml.ID = loil.MtlLocationID
	                            WHERE loil.LocalOrderInventoryUkey = loi.Ukey and ml.IsWMS = 1
                            ) and";
                        }
                    }
                    else
                    {
                        strWhereWMS = $" b.SentToWMS = 1 and b.CompleteTime is null " + Environment.NewLine + " and ";
                    }

                    columns = string.Empty;
                    switch (detailTableName)
                    {
                        // 收料
                        case WHTableName.LocalOrderReceiving_Detail:
                            return sqlcmd = $@"
                            SELECT 
                            [ID] = b.ID,
                            [InvNo] = '',
                            [POID] = b.POID,
                            [Seq1] = b.Seq1,
                            [Seq2] = b.Seq2,
                            [Refno] = lom.Refno,
                            [StockUnit] = lom.Unit,
                            [StockQty] = b.Qty,
                            [PoUnit] = lom.Unit,
                            [ShipQty] = b.Qty,
                            [Color] = lom.Color,
                            [SizeCode] = lom.SizeCode,
                            [Weight] = b.[Weight],
                            [StockType] = 'B',
                            [MtlType] = lom.MtlType,
                            [Ukey] = b.Ukey,
                            [ETA] = NULL,
                            [WhseArrival] = a.WhseArrival,
                            [Status] = 'New',
                            [CmdTime] = GETDATE()
                            FROM LocalOrderReceiving a
                            LEFT JOIN LocalOrderReceiving_Detail b ON a.id = b.ID
                            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = b.POID AND lom.Seq1 = b.Seq1 AND lom.Seq2 = b.Seq2
                            LEFT JOIN LocalOrderInventory loi WITH(NOLOCK) ON loi.POID = b.POID AND 
                                                                                loi.Seq1 = b.Seq1 AND 
                                                                                loi.Seq2 = b.Seq2 AND 
                                                                                loi.Roll = b.Roll AND
                                                                                loi.Dyelot = b.Dyelot AND
                                                                                loi.StockType = b.StockType
                            WHERE {strWhereWMS} lom.FabricType = 'A' and b.Ukey IN ({ukeys}) 
                            ";

                        // 發料
                        case WHTableName.LocalOrderIssue_Detail:
                            return sqlcmd = $@"
                            SELECT 
                            [ID] = b.ID,
                            [Type] = 'P71',
                            [POID] = b.POID,
                            [Seq1] = b.Seq1,
                            [Seq2] = b.Seq2,
                            [Color] = lom.Color,
                            [SizeCode] = lom.SizeCode,
                            [StockType] = b.StockType,
                            [Qty] = b.Qty,
                            [StockPOID] = '',
                            [StockSeq1] = '',
                            [StockSeq2] = '',
                            [Ukey] = b.Ukey,
                            [Status] = 'New',
                            [CmdTime] = GETDATE()
                            FROM LocalOrderIssue  a
                            LEFT JOIN  LocalOrderIssue_Detail  b ON a.id = b.ID
                            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = b.POID AND lom.Seq1 = b.Seq1 AND lom.Seq2 = b.Seq2
                            LEFT JOIN LocalOrderInventory loi WITH(NOLOCK) ON loi.POID = b.POID AND 
                                                                                loi.Seq1 = b.Seq1 AND 
                                                                                loi.Seq2 = b.Seq2 AND 
                                                                                loi.Roll = b.Roll AND
                                                                                loi.Dyelot = b.Dyelot AND
                                                                                loi.StockType = b.StockType
                            WHERE
                            {strWhereWMS}
                            lom.FabricType = 'A' and
                            b.Ukey IN ({ukeys})";

                        // 調整
                        case WHTableName.LocalOrderAdjust_Detail:
                            return sqlcmd = $@"
                            SELECT 
                            [ID] = b.ID,
                            [POID] = b.POID,
                            [Seq1] = b.Seq1,
                            [Seq2] = b.Seq2,
                            [StockType] = b.StockType,
                            [QtyBefore] =b.QtyBefore,
                            [QtyAfter] = b.QtyAfter,
                            [Ukey] = b.Ukey,
                            [Status] = 'New',
                            [CmdTime] = GETDATE()
                            FROM LocalOrderAdjust  a
                            LEFT JOIN  LocalOrderAdjust_Detail b ON a.id = b.ID
                            LEFT JOIN WHBarcodeTransaction wt WITH(NOLOCK) ON wt.[Function] = 'P72' AND wt.TransactionID = b.ID AND wt.TransactionUkey = b.UKey AND wt.Action = 'Confirm'
                            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = b.POID AND lom.Seq1 = b.Seq1 AND lom.Seq2 = b.Seq2
                            LEFT JOIN LocalOrderInventory loi WITH(NOLOCK) ON loi.POID = b.POID AND 
                                                                                loi.Seq1 = b.Seq1 AND 
                                                                                loi.Seq2 = b.Seq2 AND 
                                                                                loi.Roll = b.Roll AND
                                                                                loi.Dyelot = b.Dyelot AND
                                                                                loi.StockType = b.StockType
                            WHERE 
                            {strWhereWMS}
                            lom.FabricType = 'A' and
                            b.Ukey IN ({ukeys})";
                        case WHTableName.LocalOrderLocationTrans_Detail:
                            return sqlcmd = $@"
                            SELECT 
                            [ID] = b.ID,
                            [POID] = b.POID,
                            [Seq1] = b.Seq1,
                            [Seq2] = b.Seq2,
                            [FromLocation] = b.FromLocation,
                            [ToLocation] = b.ToLocation,
                            [Refno] = lom.Refno,
                            [StockUnit] = lom.Unit,
                            [Color] = lom.Color,
                            [SizeCode] = lom.SizeCode,
                            [MtlType] = lom.MtlType,
                            [Qty] = b.Qty,
                            [StockType] = b.StockType,
                            [Ukey] = b.Ukey,
                            [Status] = 'New',
                            [CmdTime] = GETDATE()
                            FROM LocalOrderLocationTrans a
                            LEFT JOIN  LocalOrderLocationTrans_Detail b ON a.id = b.ID
                            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = b.POID AND lom.Seq1 = b.Seq1 AND lom.Seq2 = b.Seq2
                            LEFT JOIN LocalOrderInventory loi WITH(NOLOCK) ON loi.POID = b.POID AND 
                                                                                loi.Seq1 = b.Seq1 AND 
                                                                                loi.Seq2 = b.Seq2 AND 
                                                                                loi.Roll = b.Roll AND
                                                                                loi.Dyelot = b.Dyelot AND
                                                                                loi.StockType = b.StockType
                            WHERE 
                            {strWhereWMS}
                            lom.FabricType = 'A' and
                            b.Ukey IN ({ukeys})";
                    }
                }
                else
                {
                    switch (detailTableName)
                    {
                        // 收料
                        case WHTableName.Receiving_Detail:
                            columns += $@"
    ,[InvNo] = iif('{formName}' = 'P07', {headerAlias}InvNo, '')
    ,[ETA] = iif('{formName}' = 'P07', {headerAlias}ETA, null)
    ,[WhseArrival] = iif('{formName}' = 'P07', {headerAlias}WhseArrival, null)
    ,sd.StockQty
    ,[StockUnit] = iif('{formName}' = 'P07', sd.StockUnit,dbo.GetStockUnitBySPSeq(sd.PoId, sd.Seq1 ,sd.Seq2))
    ,[PoUnit] = iif('{formName}' = 'P07', sd.PoUnit, '')
    ,[ShipQty] = iif('{formName}' = 'P07', sd.ShipQty, 0.00)
    ,[Weight] = iif('{formName}' = 'P07', sd.Weight, 0.00)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
";
                            break;
                        case WHTableName.TransferIn_Detail:
                            columns += $@"
    ,[InvNo] = {headerAlias}InvNo
    ,[ETA] = null
    ,[WhseArrival] = {headerAlias}IssueDate
    ,{headerAlias}IssueDate
    ,StockQty = sd.Qty
    ,sd.Qty
    ,StockUnit = dbo.GetStockUnitBySPSeq(sd.POID, sd.Seq1, sd.Seq2)
    ,[PoUnit] = ''
    ,ShipQty = 0.0
    ,sd.Weight
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
";
                            break;

                        // 退料
                        case WHTableName.IssueReturn_Detail:
                            columns += $@"
    ,sd.Qty
    ,StockUnit = dbo.GetStockUnitBySpSeq (sd.POID, sd.seq1, sd.seq2)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
";
                            break;

                        // 退料(廠商)
                        case WHTableName.ReturnReceipt_Detail:
                            columns += $@"
    ,sd.Qty
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
";
                            break;

                        // 轉料
                        case WHTableName.SubTransfer_Detail:
                        case WHTableName.BorrowBack_Detail:
                            columns = $@"
    ,sd.FromPOID
    ,sd.FromSeq1
    ,sd.FromSeq2
    ,sd.FromRoll
    ,sd.FromDyelot
    ,sd.FromStockType
    ,[FromLocation] = dbo.Getlocation(f.Ukey)
    ,sd.ToPOID
    ,sd.ToSeq1
    ,sd.ToSeq2
    ,sd.ToRoll
    ,sd.ToDyelot
    ,sd.ToStockType
    ,sd.ToLocation
    ,sd.Qty
    ,sd.SentToWMS
    ,sd.CompleteTime

    ,psd.Refno
    ,[SizeCode] = isnull(psdsS.SpecValue ,'')
    ,[StockUnit] = dbo.GetStockUnitBySPSeq(f.PoId, f.Seq1 ,f.Seq2)
    ,[MtlType] = Fabric.MtlTypeID
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
";
                            if (detailTableName == WHTableName.SubTransfer_Detail)
                            {
                                columns += $@"
    , {headerAlias}Type";
                            }

                            break;

                        // 發料
                        case WHTableName.Issue_Detail:
                        case WHTableName.IssueLack_Detail:
                        case WHTableName.TransferOut_Detail:
                            columns += $@"
    ,[Type] = '{formName}'
    ,sd.Qty
    ,[StockUnit] = dbo.GetStockUnitBySPSeq(sd.PoId, sd.Seq1 ,sd.Seq2)
    ,psd.StockPOID
    ,psd.StockSeq1
    ,psd.StockSeq2
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
";
                            break;

                        // 調整
                        case WHTableName.Adjust_Detail:
                            columns += $@"
    ,sd.QtyBefore
    ,sd.QtyAfter
    ,[Qty] = sd.QtyBefore - sd.QtyAfter
";

                            break;

                        // 盤點
                        case WHTableName.Stocktaking_Detail:
                            columns += $@"
    ,sd.QtyBefore
";
                            break;

                        case WHTableName.LocationTrans_Detail:
                            columns += $@"
    ,sd.FromLocation
    ,sd.ToLocation
    ,[Qty] = isnull(f.InQty,0) -isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
    ,[StockUnit] = dbo.GetStockUnitBySPSeq(sd.PoId, sd.Seq1 ,sd.Seq2)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
";
                            break;
                    }
                }
            }


            if (isP99)
            {
                sqlcmd = $@"
select
    [ID] = sd.ID
    ,[Ukey] = sd.Ukey
{columns}

FROM #tmp sd
{psd_FtyDt}
left join Fabric WITH (NOLOCK) ON Fabric.SCIRefNo = psd.SCIRefNo
{otherTables}
where psd.FabricType = '{fabricType}'
{whereWMS}
";
            }
            else
            {
                sqlcmd = $@"
select
    [ID] = sd.ID
    ,[Ukey] = sd.Ukey
{columns}

FROM Production.dbo.{detailTableName} sd
inner join Production.dbo.{mainTable} s on sd.id = s.id
{psd_FtyDt}
left join Fabric WITH (NOLOCK) ON Fabric.SCIRefNo = psd.SCIRefNo
{otherTables}
where sd.Ukey in ({ukeys})
and psd.FabricType = '{fabricType}'
{whereWMS}
";
            }

            return sqlcmd;
        }

        /// <inheritdoc/>
        public static WHTableName GetDetailNameforAPI(string formName)
        {
            WHTableName detailTableName = Prgs.GetWHDetailTableName(formName);
            switch (detailTableName)
            {
                case WHTableName.TransferIn_Detail:
                    return WHTableName.Receiving_Detail;
                case WHTableName.IssueLack_Detail:
                case WHTableName.TransferOut_Detail:
                    return WHTableName.Issue_Detail;
            }

            if (formName == "P45" || formName == "P48")
            {
                return WHTableName.RemoveC_Detail;
            }

            return detailTableName;
        }

        /// <inheritdoc/>
        public static DualResult SendWebAPI_Status(EnumStatus statusAPI, string url, AutomationErrMsgPMS automationErrMsg, string jsonBody, bool isShowMsg = true)
        {
            switch (statusAPI)
            {
                case EnumStatus.Lock:
                    DualResult result = WH_Auto_SendWebAPI(url, automationErrMsg.suppAPIThread, jsonBody, automationErrMsg);
                    if (!result)
                    {
                        if (isShowMsg)
                        {
                            MyUtility.Msg.WarningBox("WMS system rejected the lock request, please reference below information：" + Environment.NewLine + result.Messages.ToString());
                        }

                        return result;
                    }

                    return new DualResult(true);

                case EnumStatus.New:
                case EnumStatus.Delete:
                case EnumStatus.Revise:
                case EnumStatus.UnLock:
                    WH_Auto_SendWebAPI(url, automationErrMsg.suppAPIThread, jsonBody, automationErrMsg, reSented: true);
                    break;
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        public static bool SentandUpdatebyAutomationCreateRecord(string formName, EnumStatus statusAPI, EnumStatus action, AutoRecord autoRecord, string url)
        {
            foreach (string ukey in autoRecord.automationCreateRecordUkey)
            {
                if (!LogicAutoWHData.GetDataByAutomationCreateRecord(ukey, out DataRow dr))
                {
                    return false;
                }

                string jsonBody = MyUtility.Convert.GetString(dr["Json"]);
                AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS
                {
                    apiThread = MyUtility.Convert.GetString(dr["apiThread"]),
                    suppAPIThread = MyUtility.Convert.GetString(dr["suppAPIThread"]),
                    moduleName = MyUtility.Convert.GetString(dr["moduleName"]),
                    suppID = MyUtility.Convert.GetString(dr["suppID"]),
                };

                if (!LogicAutoWHData.SendWebAPI_Status(statusAPI, url, automationErrMsg, jsonBody))
                {
                    return false;
                }

                if (!LogicAutoWHData.DeleteAutomationCreateRecord(automationErrMsg, ukey))
                {
                    return false;
                }
            }

            // 記錄 Confirmed/UnConfirmed 後有傳給WMS的資料
            string ukeys = autoRecord.wh_Detail_Ukey.JoinToString(",");
            if (statusAPI != EnumStatus.Lock && statusAPI != EnumStatus.UnLock)
            {
                PublicPrg.Prgs.SentToWMS(null, action == EnumStatus.Confirm, formName, ukeys);
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool SaveAutomationCreateRecord(AutomationErrMsgPMS automationErrMsg, string jsonBody, AutoRecord autoRecord)
        {
            AutomationCreateRecord automationCreateRecord = new AutomationCreateRecord(automationErrMsg, jsonBody);
            try
            {
                DBProxy._OpenConnection("Production", out SqlConnection sqlConnection);
                automationCreateRecord.SaveAutomationCreateRecord(sqlConnection);
                autoRecord.automationCreateRecordUkey.Add(automationCreateRecord.ukey);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.ToString());
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static bool DeleteAutomationCreateRecord(AutomationErrMsgPMS automationErrMsg, string ukey)
        {
            if (automationErrMsg == null)
            {
                automationErrMsg = new AutomationErrMsgPMS();
            }

            AutomationCreateRecord automationCreateRecord = new AutomationCreateRecord(automationErrMsg);
            automationCreateRecord.ukey = ukey;
            try
            {
                DBProxy._OpenConnection("Production", out SqlConnection sqlConnection);
                automationCreateRecord.DeleteAutomationCreateRecord(sqlConnection);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.ToString());
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static void DeleteAutomationCreateRecordAll(AutomationErrMsgPMS automationErrMsg, List<AutoRecord> autoRecordList)
        {
            foreach (var item in autoRecordList)
            {
                foreach (var ukey in item.automationCreateRecordUkey)
                {
                    LogicAutoWHData.DeleteAutomationCreateRecord(automationErrMsg, ukey);
                }
            }
        }

        /// <inheritdoc/>
        public static bool GetDataByAutomationCreateRecord(string ukey, out DataRow dr)
        {
            string sqlcmd = @"select * from AutomationCreateRecord where ukey = @ukey";
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@Ukey", ukey) };
            if (!MyUtility.Check.Seek(sqlcmd, parameters, out dr, "Production"))
            {
                // 正常流程一定存在不應該走這
                MyUtility.Msg.WarningBox("AutomationCreateRecord not found");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public static object CreateStructure(string tableName, object structureID)
        {
            Dictionary<string, object> resultObj = new Dictionary<string, object>
            {
                { "TableArray", new string[] { tableName } },
            };

            Dictionary<string, object> dataStructure = new Dictionary<string, object>
            {
                { tableName, structureID },
            };
            resultObj.Add("DataTable", dataStructure);

            return resultObj;
        }
    }
}