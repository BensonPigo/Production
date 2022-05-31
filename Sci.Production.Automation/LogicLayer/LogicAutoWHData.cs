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

                if (fabricType == "A")
                {
                    if (detailTableName == WHTableName.SubTransfer_Detail || detailTableName == WHTableName.BorrowBack_Detail)
                    {
                        whereIsWMSTo = $@"
    and ml.StockType = sd.FromStockType
	union all
	select 1 from MtlLocation ml 
	inner join dbo.SplitString(sd.ToLocation,',') sp on sp.Data = ml.ID and ml.StockType = sd.ToStockType
	where ml.IsWMS = 1";
                    }
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
                        if (fabricType == "A")
                        {
                            whereWMS = $@"
and exists(
	select 1
	from FtyInventory_Detail fd 
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where fd.Ukey = f.Ukey
	and ml.IsWMS = 1
{whereIsWMSTo}
)";
                        }

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
    ,[SizeCode] = psd.SizeSpec
";

            if (fabricType == "F")
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
    ,sd.Weight
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit})
    ,[IsInspection] = convert(bit, 0)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
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
    ,sd.Weight
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit})
    ,[IsInspection] = convert(bit, 0)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
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
    ,[FromBarcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit})
    ,sd.ToPOID
    ,sd.ToSeq1
    ,sd.ToSeq2
    ,sd.ToRoll
    ,sd.ToDyelot
    ,sd.ToStockType
    ,sd.ToLocation
    ,[ToBarcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'T', fto.Ukey, 0, {fromNewBarcodeBit})
    ,sd.Qty
    ,sd.SentToWMS
    ,sd.CompleteTime

    ,psd.Refno
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
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
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit})
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
";
                        break;

                    // 退料(廠商)
                    case WHTableName.ReturnReceipt_Detail:
                        columns += $@"
    ,sd.Qty
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit})
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

                        otherTables += $@"
outer apply (
	select [Tone] = isnull(MAX(fs.Tone), '')
	from FIR with (nolock) 
	left join FIR_Shadebone fs with (nolock) on FIR.ID = fs.ID and fs.Roll = f.Roll and fs.Dyelot = f.Dyelot
	where FIR.poid = f.poid and FIR.seq1 = f.seq1 and FIR.seq2 = f.seq2
) ShadeboneTone
";
                        columns += $@"
    ,[Type] = '{formName}'
    ,sd.Qty
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey, 0, {fromNewBarcodeBit})
    ,[NewBarcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'T', f.Ukey, 0, {fromNewBarcodeBit})
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
    ,ShadeboneTone.Tone
";

                        break;

                    // 調整
                    case WHTableName.Adjust_Detail:
                        columns += $@"
    ,sd.QtyBefore
    ,sd.QtyAfter
    ,[Barcode] = dbo.GetWHBarcodeToGensong('{formName}', sd.ID, sd.Ukey, '{action}', 'F', f.Ukey,
	    isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) {(action == EnumStatus.Confirm ? "-" : "+")} (isnull(sd.QtyAfter, 0) - isnull(sd.QtyBefore, 0))-- 帶入調整前的balanceQty
        , {fromNewBarcodeBit})
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
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
";
                        break;
                }
            }

            if (fabricType == "A")
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
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
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
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
";
                        break;

                    // 退料
                    case WHTableName.IssueReturn_Detail:
                        columns += $@"
    ,sd.Qty
    ,StockUnit = dbo.GetStockUnitBySpSeq (sd.POID, sd.seq1, sd.seq2)
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
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
    ,[SizeCode] = psd.SizeSpec
    ,[StockUnit] = dbo.GetStockUnitBySPSeq(f.PoId, f.Seq1 ,f.Seq2)
    ,[MtlType] = Fabric.MtlTypeID
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
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
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
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
    ,[Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, psd.ColorID, Fabric.MtlTypeID, psd.SuppColor)
    ,[Description] = dbo.getMtlDesc(psd.ID, psd.Seq1, psd.Seq2, 2, 0)
";
                        break;
                }
            }

            string sqlcmd;
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
        public static bool SendWebAPI_Status(EnumStatus statusAPI, string url, AutomationErrMsgPMS automationErrMsg, string jsonBody)
        {
            switch (statusAPI)
            {
                case EnumStatus.Lock:
                    DualResult result = WH_Auto_SendWebAPI(url, automationErrMsg.suppAPIThread, jsonBody, automationErrMsg);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("WMS system rejected the lock request, please reference below information：" + Environment.NewLine + result.Messages.ToString());
                        return false;
                    }

                    return true;

                case EnumStatus.New:
                    SendWebAPI(GetSciUrl(), automationErrMsg.suppAPIThread, jsonBody, automationErrMsg);
                    break;

                case EnumStatus.Delete:
                case EnumStatus.Revise:
                case EnumStatus.UnLock:
                    WH_Auto_SendWebAPI(url, automationErrMsg.suppAPIThread, jsonBody, automationErrMsg, reSented: true);
                    break;
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
                autoRecord.automationCreateRecordUkey = new List<string>();
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