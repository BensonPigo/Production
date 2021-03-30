using Ict;
using Newtonsoft.Json;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation
{
    /// <inheritdoc/>
    public class Gensong_AutoWHFabric
    {
        private static readonly string GensongSuppID = "3A0174";
        private static readonly string moduleName = "AutoWHFabric";
        private static readonly string suppAPIThread = "pms/GS_WebServices";
        private static readonly string SCIAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
        private static readonly string URL = GetSupplierUrl(GensongSuppID, moduleName);
        private AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS();

        /// <inheritdoc/>
        public static bool IsGensong_AutoWHFabricEnable => IsModuleAutomationEnable(GensongSuppID, moduleName);

        #region Reveive

        /// <summary>
        /// Sent Receive Detail New
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="formName">formName</param>
        public void SentReceive_Detail_New(DataTable dtDetail, string formName = "")
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            // 取得資料
            DataTable dtMaster = this.GetReceiveData(dtDetail, formName, "New");

            // 沒資料就return
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            //// Confirm後要上鎖物料
            // DualResult result;
            // if (formName == "P07" || formName == "P18")
            // {
            //    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtMaster, string.Empty, Prgs.UpdateFtyInventory_IO(99, null, true), out DataTable dt, "#TmpSource")))
            //    {
            //        MyUtility.Msg.WarningBox(result.Messages.ToString());
            //        return;
            //    }
            // }
            #region 記錄Confirmed後有傳給WMS的資料
            switch (formName)
            {
                case "P07":
                    if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "Receiving"))
                    {
                        return;
                    }

                    break;
                case "P18":
                    if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "TransferIn"))
                    {
                        return;
                    }

                    break;
            }
            #endregion

            this.SetAutoAutomationErrMsg("SentReceiving_DetailToVstrong", "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "Receiving_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Sent Receive_Detail Delete
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="formName">type</param>
        /// <param name = "status" > Status </ param >
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentReceive_Detail_Delete(DataTable dtDetail, string formName = "", string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            // 呼叫同個Class裡的Method,需要先new物件才行
            Gensong_AutoWHFabric callMethod = new Gensong_AutoWHFabric();

            // 取得資料
            DataTable dtMaster = callMethod.GetReceiveData(dtDetail, formName, status, isP99);
            DualResult result;

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                var dtTable = dtMaster.AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"]) && MyUtility.Check.Empty(x["CompleteTime"]));
                if (dtTable.Any())
                {
                    dtMaster = dtTable.CopyToDataTable();
                }
                else
                {
                    return true;
                }
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "Receiving_Detail");
            callMethod.SetAutoAutomationErrMsg("SentReceiving_DetailToGensong", string.Empty);

            // Call API傳送給WMS, 若回傳失敗就跳訊息並不能UnConfirmed
            if (!(result = WH_Auto_SendWebAPI(URL, callMethod.automationErrMsg.suppAPIThread, jsonBody, callMethod.automationErrMsg)))
            {
                string strMsg = string.Empty;
                switch (status)
                {
                    case "delete":
                        strMsg = "WMS system rejected the delete request, please reference below information：";
                        break;
                    case "Revise":
                        strMsg = "WMS system rejected the revise request, please reference below information：";
                        break;
                    case "UnConfirmed":
                        strMsg = "WMS system rejected the unconfirm request, please reference below information：";
                        break;
                }

                MyUtility.Msg.WarningBox(strMsg + Environment.NewLine + result.Messages.ToString());
                return false;
            }

            // 記錄UnConfirmed後有傳給WMS的資料
            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                switch (formName)
                {
                    case "P07":
                        if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "Receiving"))
                        {
                            return false;
                        }

                        break;
                    case "P18":
                        if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "TransferIn"))
                        {
                            return false;
                        }

                        break;
                }
            }

            // UnConfirm後要解鎖物料
            if (formName == "P07" || formName == "P18")
            {
                if (!(result = MyUtility.Tool.ProcessWithDatatable(dtMaster, string.Empty, Prgs.UpdateFtyInventory_IO(99, null, false), out DataTable dt, "#TmpSource")))
                {
                    MyUtility.Msg.WarningBox(result.Messages.ToString());
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Issue

        /// <summary>
        /// Sent Receive Detail New
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="formName">formName</param>
        public void SentIssue_Detail_New(DataTable dtDetail, string formName = "")
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            // 取得資料
            DataTable dtMaster = this.GetIssueData(dtDetail, formName, "New");

            // 沒資料就return
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region 記錄Confirmed後有傳給WMS的資料
            switch (formName)
            {
                case "P10":
                case "P13":
                case "P62":
                    if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "Issue"))
                    {
                        return;
                    }

                    break;
                case "P16":
                    if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "IssueLack"))
                    {
                        return;
                    }

                    break;
                case "P19":
                    if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "TransferOut"))
                    {
                        return;
                    }

                    break;
            }

            #endregion

            this.SetAutoAutomationErrMsg("SentIssue_DetailToGensong", "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "Issue_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Sent Receive_Detail Delete
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="formName">type</param>
        /// <param name = "status" > Status </ param >
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentIssue_Detail_Delete(DataTable dtDetail, string formName = "", string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Gensong_AutoWHFabric callMethod = new Gensong_AutoWHFabric();

            // 取得資料
            DataTable dtMaster = callMethod.GetIssueData(dtDetail, formName, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                var dtTable = dtMaster.AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"]) && MyUtility.Check.Empty(x["CompleteTime"]));
                if (dtTable.Any())
                {
                    dtMaster = dtTable.CopyToDataTable();
                }
                else
                {
                    return true;
                }
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "Issue_Detail");
            callMethod.SetAutoAutomationErrMsg("SentIssue_DetailToGensong", string.Empty);

            // Call API傳送給WMS, 若回傳失敗就跳訊息並不能UnConfirmed
            if (!(result = WH_Auto_SendWebAPI(URL, callMethod.automationErrMsg.suppAPIThread, jsonBody, callMethod.automationErrMsg)))
            {
                string strMsg = string.Empty;
                switch (status)
                {
                    case "delete":
                        strMsg = "WMS system rejected the delete request, please reference below information：";
                        break;
                    case "Revise":
                        strMsg = "WMS system rejected the revise request, please reference below information：";
                        break;
                    case "UnConfirmed":
                        strMsg = "WMS system rejected the unconfirm request, please reference below information：";
                        break;
                }

                MyUtility.Msg.WarningBox(strMsg + Environment.NewLine + result.Messages.ToString());
                return false;
            }

            // 記錄UnConfirmed後有傳給WMS的資料
            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                switch (formName)
                {
                    case "P10":
                    case "P13":
                    case "P62":
                        if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "Issue"))
                        {
                            return false;
                        }

                        break;
                    case "P16":
                        if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "IssueLack"))
                        {
                            return false;
                        }

                        break;
                    case "P19":
                        if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "TransferOut"))
                        {
                            return false;
                        }

                        break;
                }
            }

            return true;
        }
        #endregion

        /// <summary>
        /// WHClose To Gensong
        /// </summary>
        /// <param name="dtMaster">Master DataSource</param>
        public void SentWHCloseToGensongAutoWHFabric(DataTable dtMaster)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentWHCloseToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtMaster.AsEnumerable()
                .Select(dr => new
                {
                    POID = dr["POID"].ToString(),
                    WhseClose = MyUtility.Check.Empty(dr["WhseClose"]) ? null : ((DateTime?)dr["WhseClose"]).Value.ToString("yyyy/MM/dd"),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("WHClose", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        #region SubTransfer

        /// <summary>
        /// Sent SubTransfer Detail New
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        public void SentSubTransfer_Detail_New(DataTable dtDetail)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            // 取得資料
            DataTable dtMaster = this.GetSubTransfer_Detail(dtDetail, "New");

            // 沒資料就return
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region 記錄Confirmed後有傳給WMS的資料
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "SubTransfer"))
            {
                return;
            }

            #endregion

            this.SetAutoAutomationErrMsg("SentSubTransfer_DetailToGensong", "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "SubTransfer_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Sent SubTransfer Detail Delete
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name = "status" > Status </ param >
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentSubTransfer_Detail_Delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Gensong_AutoWHFabric callMethod = new Gensong_AutoWHFabric();

            // 取得資料
            DataTable dtMaster = callMethod.GetSubTransfer_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                var dtTable = dtMaster.AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"]) && MyUtility.Check.Empty(x["CompleteTime"]));
                if (dtTable.Any())
                {
                    dtMaster = dtTable.CopyToDataTable();
                }
                else
                {
                    return true;
                }
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "SubTransfer_Detail");
            callMethod.SetAutoAutomationErrMsg("SentSubTransfer_DetailToGensong", string.Empty);

            // Call API傳送給WMS, 若回傳失敗就跳訊息並不能UnConfirmed
            if (!(result = WH_Auto_SendWebAPI(URL, callMethod.automationErrMsg.suppAPIThread, jsonBody, callMethod.automationErrMsg)))
            {
                string strMsg = string.Empty;
                switch (status)
                {
                    case "delete":
                        strMsg = "WMS system rejected the delete request, please reference below information：";
                        break;
                    case "Revise":
                        strMsg = "WMS system rejected the revise request, please reference below information：";
                        break;
                    case "UnConfirmed":
                        strMsg = "WMS system rejected the unconfirm request, please reference below information：";
                        break;
                }

                MyUtility.Msg.WarningBox(strMsg + Environment.NewLine + result.Messages.ToString());
                return false;
            }

            // 記錄UnConfirmed後有傳給WMS的資料
            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "SubTransfer"))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        /// <summary>
        /// MtlLocation To Gensong
        /// </summary>
        /// <param name="dtMaster">Master DataSource</param>
        public void SentMtlLocationToGensongAutoWHFabric(DataTable dtMaster)
        {
            // ISP20201856 已移除此功能
            if (true)
            {
                return; // 暫未開放
            }

            /*
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentMtlLocationToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;
            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtMaster.AsEnumerable()
                .Select(dr => new
                {
                    ID = dr["ID"].ToString(),
                    StockType = dr["StockType"].ToString(),
                    Junk = (bool)dr["Junk"],
                    Description = dr["Description"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("MtlLocation", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
            */
        }

        /// <summary>
        /// RefnoRelaxtime To Gensong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        public void SentRefnoRelaxtimeToGensongAutoWHFabric(DataTable dtDetail)
        {
            /*
             *記得更新Function 也要一併更新到MES Automation.Dll檔
             */
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentRefnoRelaxtimeToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtDetail.AsEnumerable()
                .Select(s => new
                {
                    Refno = s["Refno"].ToString(),
                    FabricRelaxationID = s["FabricRelaxationID"].ToString(),
                    RelaxTime = (decimal)s["RelaxTime"],
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("RefnoRelaxtime", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Cutplan_Detail To Gensong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="isConfirmed">bool</param>
        public void SentCutplan_DetailToGensongAutoWHFabric(DataTable dtDetail, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentCutplan_DetailToGensong";
            string suppAPIThread = "Api/GensongAutoWHFabric/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select distinct
    [CutplanID] = cp2.ID
    ,[CutRef] = cp2.CutRef
    ,[CutNo] = cp2.CutNo
    ,[PoID] = wo.ID
    ,[Seq1] = wo.SEQ1
    ,[Seq2] = wo.SEQ2
    ,[Refno] = wo.Refno
    ,[Article] = Article.value
    ,[Color] = cp2.Colorid
    ,[SizeCode] = SizeCode.value
    ,cp2.WorkorderUkey
    ,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
    ,[CmdTime] = GETDATE()
    from  Production.dbo.Cutplan_Detail cp2
    inner join Production.dbo.Cutplan cp1 on cp2.id = cp1.id
    inner join Production.dbo.WorkOrder wo on cp2.WorkorderUkey = wo.Ukey
    outer apply(
        select value = STUFF((
            select CONCAT(',',SizeCode)
            from(
                select distinct SizeCode
                from Production.dbo.WorkOrder_Distribute
                where WorkOrderUkey = wo.Ukey
                )s
                for xml path('')
            ),1,1,'')
    ) SizeCode
    outer apply(
        select value = STUFF((
            select CONCAT(',',Article)
            from(
                select distinct Article
                from Production.dbo.WorkOrder_Distribute
                where WorkOrderUkey = wo.Ukey
                and Article !=''
                )s
            for xml path('')
            ),1,1,'')
    ) Article
    where exists(
        select 1 from Issue_Detail where cp2.ID = CutplanID
    )
    and exists(
        select 1 from #tmp s where s.ID = cp2.ID and s.WorkorderUkey = cp2.WorkorderUkey
    )
";
            DataTable dt = new DataTable();
            MyUtility.Tool.ProcessWithDatatable(dtDetail, null, sqlcmd, out dt);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dt.AsEnumerable()
                .Select(s => new
                {
                    CutplanID = s["CutplanID"].ToString(),
                    CutRef = s["CutRef"].ToString(),
                    CutNo = (decimal)s["CutNo"],
                    POID = s["POID"].ToString(),
                    Seq1 = s["Seq1"].ToString(),
                    Seq2 = s["Seq2"].ToString(),
                    Refno = s["Refno"].ToString(),
                    Article = s["Article"].ToString(),
                    ColorID = s["Color"].ToString(),
                    SizeCode = s["SizeCode"].ToString(),
                    WorkorderUkey = (long)s["WorkorderUkey"],
                    Status = s["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateGensongStructure("Cutplan_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        #region BorrowBack

        /// <summary>
        /// Sent BorrowBack Detail New
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="formName">formName</param>
        public void SentBorrowBack_Detail_New(DataTable dtDetail, string formName = "")
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            // 取得資料
            DataTable dtMaster = this.GetBorrowBack_Detail(dtDetail, "New");

            // 沒資料就return
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region 記錄Confirmed後有傳給WMS的資料
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "BorrowBack"))
            {
                return;
            }

            #endregion

            this.SetAutoAutomationErrMsg("SentBorrowBack_DetailToGensong", "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "BorrowBack_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Sent BorrowBack Detail Delete
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name = "status" > Status </ param >
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentBorrowBack_Detail_Delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            // 呼叫同個Class裡的Method,需要先new物件才行
            Gensong_AutoWHFabric callMethod = new Gensong_AutoWHFabric();

            // 取得資料
            DataTable dtMaster = callMethod.GetBorrowBack_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                var dtTable = dtMaster.AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"]) && MyUtility.Check.Empty(x["CompleteTime"]));
                if (dtTable.Any())
                {
                    dtMaster = dtTable.CopyToDataTable();
                }
                else
                {
                    return true;
                }
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "BorrowBack_Detail");
            callMethod.SetAutoAutomationErrMsg("SentBorrowBack_DetailToGensong", string.Empty);

            DualResult result;

            // Call API傳送給WMS, 若回傳失敗就跳訊息並不能UnConfirmed
            if (!(result = WH_Auto_SendWebAPI(URL, callMethod.automationErrMsg.suppAPIThread, jsonBody, callMethod.automationErrMsg)))
            {
                string strMsg = string.Empty;
                switch (status)
                {
                    case "delete":
                        strMsg = "WMS system rejected the delete request, please reference below information：";
                        break;
                    case "Revise":
                        strMsg = "WMS system rejected the revise request, please reference below information：";
                        break;
                    case "UnConfirmed":
                        strMsg = "WMS system rejected the unconfirm request, please reference below information：";
                        break;
                }

                MyUtility.Msg.WarningBox(strMsg + Environment.NewLine + result.Messages.ToString());
                return false;
            }

            // 記錄UnConfirmed後有傳給WMS的資料
            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "BorrowBack"))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region ReturnReceipt

        /// <summary>
        /// Sent ReturnReceipt Detail New
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="formName">formName</param>
        public void SentReturnReceipt_Detail_New(DataTable dtDetail, string formName = "")
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            // 取得資料
            DataTable dtMaster = this.GetReturnReceipt_Detail(dtDetail, "New");

            // 沒資料就return
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region 記錄Confirmed後有傳給WMS的資料
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "ReturnReceipt"))
            {
                return;
            }
            #endregion

            this.SetAutoAutomationErrMsg("SentReturnReceipt_DetailToGensong", "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "ReturnReceipt_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Sent ReturnReceipt Detail Delete
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name = "status" > Status </ param >
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentReturnReceipt_Detail_Delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            // 呼叫同個Class裡的Method,需要先new物件才行
            Gensong_AutoWHFabric callMethod = new Gensong_AutoWHFabric();

            // 取得資料
            DataTable dtMaster = callMethod.GetReturnReceipt_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                var dtTable = dtMaster.AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"]) && MyUtility.Check.Empty(x["CompleteTime"]));
                if (dtTable.Any())
                {
                    dtMaster = dtTable.CopyToDataTable();
                }
                else
                {
                    return true;
                }
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "ReturnReceipt_Detail");
            callMethod.SetAutoAutomationErrMsg("SentReturnReceipt_DetailToGensong", string.Empty);

            DualResult result;

            // Call API傳送給WMS, 若回傳失敗就跳訊息並不能UnConfirmed
            if (!(result = WH_Auto_SendWebAPI(URL, callMethod.automationErrMsg.suppAPIThread, jsonBody, callMethod.automationErrMsg)))
            {
                string strMsg = string.Empty;
                switch (status)
                {
                    case "delete":
                        strMsg = "WMS system rejected the delete request, please reference below information：";
                        break;
                    case "Revise":
                        strMsg = "WMS system rejected the revise request, please reference below information：";
                        break;
                    case "UnConfirmed":
                        strMsg = "WMS system rejected the unconfirm request, please reference below information：";
                        break;
                }

                MyUtility.Msg.WarningBox(strMsg + Environment.NewLine + result.Messages.ToString());
                return false;
            }

            // 記錄UnConfirmed後有傳給WMS的資料
            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "ReturnReceipt"))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        /// <summary>
        /// Sent LocationTrans Detail New
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="status">status</param>
        public void SentLocationTrans_Detail_New(DataTable dtDetail, string status = "")
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            // 取得資料
            string sqlcmd = $@"
select lt2.Id
,lt2.POID
,lt2.Seq1
,lt2.Seq2
,lt2.Roll
,lt2.Dyelot
,lt2.FromLocation
,lt2.ToLocation
,po3.Refno
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
	            , IIF(isnull(po3.SuppColor,'') = '', dbo.GetColorMultipleID(o.BrandID,po3.ColorID),po3.SuppColor)
	            , dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[Barcode] = Barcode.value
,lt2.Ukey
,lt2.StockType
,[Qty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,[CmdTime] = GETDATE()
from LocationTrans_detail lt2
inner join #tmp lt on lt.Id=lt2.Id
left join FtyInventory f on lt2.FtyInventoryUkey = f.ukey
left join PO_Supp_Detail po3 on po3.ID = lt2.POID and po3.SEQ1 = lt2.Seq1
    and po3.SEQ2 = lt2.Seq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo = Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey
)Barcode
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= lt2.POID and psd.SEQ1 = lt2.Seq1
    and psd.SEQ2 = lt2.Seq2 and psd.FabricType='F'
)
and exists(
    select 1
	from MtlLocation ml
    inner join dbo.SplitString(lt2.ToLocation,',') sp on sp.Data = ml.ID
	where  ml.IsWMS =1 
	union all
    select 1
	from MtlLocation ml
    inner join dbo.SplitString(lt2.FromLocation,',') sp on sp.Data = ml.ID
	where  ml.IsWMS =1 
)
";
            DataTable dt = new DataTable();
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, sqlcmd, out dt)))
            {
                return;
            }

            // 沒資料就return
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            #region 記錄Confirmed後有傳給WMS的資料
            if (!PublicPrg.Prgs.SentToWMS(dt, true, "LocationTrans"))
            {
                return;
            }

            #endregion

            this.SetAutoAutomationErrMsg("SentLocationTrans_DetailToGensong", "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dt, "LocationTrans_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        #region Adjust

        /// <summary>
        /// Sent Adjust Detail New
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        public void SentAdjust_Detail_New(DataTable dtDetail)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            // 取得資料
            DataTable dtMaster = this.GetAdjust_Detail(dtDetail, "New");

            // 沒資料就return
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region 記錄Confirmed後有傳給WMS的資料
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "Adjust"))
            {
                return;
            }

            #endregion

            this.SetAutoAutomationErrMsg("SentAdjust_DetailToGensong", "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "Adjust_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Sent Adjust Detail Delete
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name = "status" > Status </ param >
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentAdjust_Detail_Delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(GensongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            // 呼叫同個Class裡的Method,需要先new物件才行
            Gensong_AutoWHFabric callMethod = new Gensong_AutoWHFabric();

            // 取得資料
            DataTable dtMaster = callMethod.GetAdjust_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                var dtTable = dtMaster.AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"]) && MyUtility.Check.Empty(x["CompleteTime"]));
                if (dtTable.Any())
                {
                    dtMaster = dtTable.CopyToDataTable();
                }
                else
                {
                    return true;
                }
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "Adjust_Detail");
            callMethod.SetAutoAutomationErrMsg("SentAdjust_DetailToGensong", string.Empty);

            DualResult result;

            // Call API傳送給WMS, 若回傳失敗就跳訊息並不能UnConfirmed
            if (!(result = WH_Auto_SendWebAPI(URL, callMethod.automationErrMsg.suppAPIThread, jsonBody, callMethod.automationErrMsg)))
            {
                string strMsg = string.Empty;
                switch (status)
                {
                    case "delete":
                        strMsg = "WMS system rejected the delete request, please reference below information：";
                        break;
                    case "Revise":
                        strMsg = "WMS system rejected the revise request, please reference below information：";
                        break;
                    case "UnConfirmed":
                        strMsg = "WMS system rejected the unconfirm request, please reference below information：";
                        break;
                }

                MyUtility.Msg.WarningBox(strMsg + Environment.NewLine + result.Messages.ToString());
                return false;
            }

            // 記錄UnConfirmed後有傳給WMS的資料
            if (string.Compare(status, "UnConfirmed", true) == 0)
            {
                if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "Adjust"))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 資料邏輯層

        private string GetJsonBody(DataTable dtDetail, string type)
        {
            string jsonBody = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Gensong_AutoWHFabric callMethod = new Gensong_AutoWHFabric();
            dynamic bodyObject = new ExpandoObject();
            switch (type)
            {
                case "Receiving_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                    .Select(dr => new
                    {
                        ID = dr["ID"].ToString(),
                        InvNo = dr["InvNo"].ToString(),
                        PoId = dr["PoId"].ToString(),
                        Seq1 = dr["Seq1"].ToString(),
                        Seq2 = dr["Seq2"].ToString(),
                        Refno = dr["Refno"].ToString(),
                        Color = dr["Color"].ToString(),
                        Roll = dr["Roll"].ToString(),
                        Dyelot = dr["Dyelot"].ToString(),
                        StockUnit = dr["StockUnit"].ToString(),
                        StockQty = (decimal)dr["StockQty"],
                        PoUnit = dr["PoUnit"].ToString(),
                        ShipQty = (decimal)dr["ShipQty"],
                        Weight = (decimal)dr["Weight"],
                        StockType = dr["StockType"].ToString(),
                        Barcode = dr["Barcode"].ToString(),
                        Ukey = (long)dr["Ukey"],
                        IsInspection = (bool)dr["IsInspection"],
                        ETA = MyUtility.Check.Empty(dr["ETA"]) ? null : ((DateTime?)dr["ETA"]).Value.ToString("yyyy/MM/dd"),
                        WhseArrival = MyUtility.Check.Empty(dr["WhseArrival"]) ? null : ((DateTime?)dr["WhseArrival"]).Value.ToString("yyyy/MM/dd"),
                        Status = dr["Status"].ToString(),
                        CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    });
                    break;
                case "Issue_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(dr => new
                   {
                       Id = dr["id"].ToString(),
                       Type = dr["Type"].ToString(),
                       CutPlanID = dr["CutPlanID"].ToString(),
                       EstCutdate = MyUtility.Check.Empty(dr["EstCutdate"]) ? null : ((DateTime?)dr["EstCutdate"]).Value.ToString("yyyy/MM/dd"),
                       SpreadingNoID = dr["SpreadingNoID"].ToString(),
                       PoId = dr["PoId"].ToString(),
                       Seq1 = dr["Seq1"].ToString(),
                       Seq2 = dr["Seq2"].ToString(),
                       Roll = dr["Roll"].ToString(),
                       Dyelot = dr["Dyelot"].ToString(),
                       Barcode = dr["Barcode"].ToString(),
                       NewBarcode = dr["NewBarcode"].ToString(),
                       Qty = (decimal)dr["Qty"],
                       Ukey = (long)dr["Ukey"],
                       Status = dr["Status"].ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;
                case "Adjust_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                        .Select(dr => new
                        {
                            Id = dr["id"].ToString(),
                            PoId = dr["PoId"].ToString(),
                            Seq1 = dr["Seq1"].ToString(),
                            Seq2 = dr["Seq2"].ToString(),
                            StockType = dr["StockType"].ToString(),
                            QtyBefore = (decimal)dr["QtyBefore"],
                            QtyAfter = (decimal)dr["QtyAfter"],
                            Barcode = dr["Barcode"].ToString(),
                            Ukey = (long)dr["Ukey"],
                            Status = dr["Status"].ToString(),
                            CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        });
                    break;
                case "SubTransfer_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                    .Select(dr => new
                    {
                        ID = dr["ID"].ToString(),
                        Type = dr["Type"].ToString(),
                        FromPOID = dr["FromPOID"].ToString(),
                        FromSeq1 = dr["FromSeq1"].ToString(),
                        FromSeq2 = dr["FromSeq2"].ToString(),
                        FromRoll = dr["FromRoll"].ToString(),
                        FromDyelot = dr["FromDyelot"].ToString(),
                        FromStockType = dr["FromStockType"].ToString(),
                        FromBarcode = dr["FromBarcode"].ToString(),
                        FromLocation = dr["FromLocation"].ToString(),
                        ToPOID = dr["ToPOID"].ToString(),
                        ToSeq1 = dr["ToSeq1"].ToString(),
                        ToSeq2 = dr["ToSeq2"].ToString(),
                        ToRoll = dr["ToRoll"].ToString(),
                        ToDyelot = dr["ToDyelot"].ToString(),
                        ToStockType = dr["ToStockType"].ToString(),
                        ToBarcode = dr["ToBarcode"].ToString(),
                        ToLocation = dr["ToLocation"].ToString(),
                        Refno = dr["Refno"].ToString(),
                        Color = dr["Color"].ToString(),
                        Qty = (decimal)dr["Qty"],
                        Ukey = (long)dr["Ukey"],
                        Status = dr["Status"].ToString(),
                        CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    });
                    break;
                case "ReturnReceipt_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(dr => new
                   {
                       ID = dr["ID"].ToString(),
                       POID = dr["POID"].ToString(),
                       Seq1 = dr["Seq1"].ToString(),
                       Seq2 = dr["Seq2"].ToString(),
                       Roll = dr["Roll"].ToString(),
                       Dyelot = dr["Dyelot"].ToString(),
                       Qty = (decimal)dr["Qty"],
                       StockType = dr["StockType"].ToString(),
                       Ukey = (long)dr["Ukey"],
                       Barcode = dr["Barcode"].ToString(),
                       Status = dr["Status"].ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;
                case "BorrowBack_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(dr => new
                   {
                       ID = dr["ID"].ToString(),
                       FromPOID = dr["FromPOID"].ToString(),
                       FromSeq1 = dr["FromSeq1"].ToString(),
                       FromSeq2 = dr["FromSeq2"].ToString(),
                       FromRoll = dr["FromRoll"].ToString(),
                       FromDyelot = dr["FromDyelot"].ToString(),
                       FromStockType = dr["FromStockType"].ToString(),
                       FromBarcode = dr["FromBarcode"].ToString(),
                       FromLocation = dr["FromLocation"].ToString(),
                       ToPOID = dr["ToPOID"].ToString(),
                       ToSeq1 = dr["ToSeq1"].ToString(),
                       ToSeq2 = dr["ToSeq2"].ToString(),
                       ToRoll = dr["ToRoll"].ToString(),
                       ToDyelot = dr["ToDyelot"].ToString(),
                       ToStockType = dr["ToStockType"].ToString(),
                       ToBarcode = dr["ToBarcode"].ToString(),
                       ToLocation = dr["ToLocation"].ToString(),
                       Refno = dr["Refno"].ToString(),
                       Color = dr["Color"].ToString(),
                       Qty = (decimal)dr["Qty"],
                       Ukey = (long)dr["Ukey"],
                       Status = dr["Status"].ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;
                case "LocationTrans_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(s => new
                   {
                       ID = s["ID"].ToString(),
                       POID = s["POID"].ToString(),
                       Seq1 = s["Seq1"].ToString(),
                       Seq2 = s["Seq2"].ToString(),
                       Roll = s["Roll"].ToString(),
                       Dyelot = s["Dyelot"].ToString(),
                       FromLocation = s["FromLocation"].ToString(),
                       ToLocation = s["ToLocation"].ToString(),
                       Refno = s["Refno"].ToString(),
                       Color = s["Color"].ToString(),
                       Qty = (decimal)s["Qty"],
                       Barcode = s["Barcode"].ToString(),
                       Ukey = (long)s["Ukey"],
                       StockType = s["StockType"].ToString(),
                       Status = s["Status"].ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;
            }

            return jsonBody = JsonConvert.SerializeObject(callMethod.CreateGensongStructure(type, bodyObject));
        }

        private DataTable GetReceiveData(DataTable dtDetail, string formName, string status, bool isP99 = false)
        {
            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster = new DataTable();
            string strBody = isP99 ? "inner join #tmp s on rd.ukey = s.ukey " : "inner join #tmp s on rd.ID = s.Id ";
            string strQty = isP99 ? "s.Qty" : "rd.StockQty";
            #region 取得資料

            switch (formName)
            {
                case "P07":
                    sqlcmd = $@"
SELECT [ID] = rd.id
,[InvNo] = r.InvNo
,[PoId] = rd.Poid
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = po3.Refno
,[Color] = Color.Value
,[Roll] = rd.Roll
,[Dyelot] = rd.Dyelot
,[StockUnit] = rd.StockUnit
,[StockQty] = {strQty}
,[PoUnit] = rd.PoUnit
,[ShipQty] = rd.ShipQty
,[Weight] = rd.Weight
,[StockType] = rd.StockType
,[Ukey] = rd.Ukey
,[IsInspection] = convert(bit, 0)
,[ETA] = r.ETA
,[WhseArrival] = r.WhseArrival
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,[Barcode] = Barcode.value
,rd.SentToWMS,rd.CompleteTime
FROM Production.dbo.Receiving_Detail rd
inner join Production.dbo.Receiving r on rd.id = r.id
{Environment.NewLine + strBody}
inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= rd.PoId 
	and po3.SEQ1=rd.Seq1 and po3.SEQ2=rd.Seq2
left join Production.dbo.FtyInventory f on f.POID = rd.PoId
	and f.Seq1=rd.Seq1 and f.Seq2=rd.Seq2 
	and f.Dyelot = rd.Dyelot and f.Roll = rd.Roll
	and f.StockType = rd.StockType
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN Fabric.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') 
            THEN IIF(po3.SuppColor = '',dbo.GetColorMultipleID(po3.BrandID,po3.ColorID), po3.SuppColor)
            ELSE dbo.GetColorMultipleID(po3.BrandID,po3.ColorID)
	 END
)Color
outer apply(
	select value = min(fb.Barcode)
	from FtyInventory_Barcode fb 
	where fb.Ukey = f.Ukey
)Barcode
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
	and FabricType='F'
)
";
                    break;
                case "P18":
                    strQty = isP99 ? "s.Qty" : "rd.Qty";
                    sqlcmd = $@"
SELECT [ID] = rd.id
,[InvNo] = isnull(r.InvNo,'')
,[PoId] = rd.Poid
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = po3.Refno
,[Color] = po3.ColorID
,[Roll] = rd.Roll
,[Dyelot] = rd.Dyelot
,[StockUnit] = dbo.GetStockUnitBySPSeq(rd.POID,rd.Seq1,rd.Seq2)
,[StockQty] = rd.Qty
,[PoUnit] = po3.PoUnit
,[ShipQty] = rd.Qty
,[Weight] = rd.Weight
,[StockType] = rd.StockType
,[Ukey] = rd.Ukey
,[IsInspection] = convert(bit, 0)
,[ETA] = null
,[WhseArrival] = r.IssueDate
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,[Barcode] = Barcode.value
,rd.SentToWMS,rd.CompleteTime
FROM Production.dbo.TransferIn_Detail rd
inner join Production.dbo.TransferIn r on rd.id = r.id
{Environment.NewLine + strBody}
inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= rd.PoId 
	and po3.SEQ1=rd.Seq1 and po3.SEQ2=rd.Seq2
outer apply(
	select value = min(fb.Barcode)
	from FtyInventory_Barcode fb 
	inner join FtyInventory f on f.Ukey = fb.Ukey
	where f.POID = rd.POID
	and f.Seq1 = rd.Seq1 and f.Seq2= rd.Seq2
	and f.Roll = rd.Roll and f.Dyelot = rd.Dyelot
	and f.StockType = rd.StockType
)Barcode
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
	and FabricType='F'
)
";
                    break;
            }

            if (string.Compare(status, "New", true) != 0)
            {
                sqlcmd += Environment.NewLine + @" and rd.SentToWMS = 1";
            }

            if (!MyUtility.Check.Empty(sqlcmd))
            {
                if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, sqlcmd, out dtMaster)))
                {
                    MyUtility.Msg.WarningBox(result.Messages.ToString());
                }
            }

            #endregion

            return dtMaster;
        }

        private DataTable GetIssueData(DataTable dtDetail, string formName, string status, bool isP99 = false)
        {
            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster = new DataTable();
            string strBody = isP99 ? "inner join #tmp s on i2.ukey = s.ukey " : "inner join #tmp s on i2.ID = s.Id ";
            string strQty = isP99 ? "s.Qty" : "i2.Qty";

            #region 取得資料

            switch (formName)
            {
                case "P10":
                case "P13":
                case "P62":
                    sqlcmd = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = '{formName}'
,[CutPlanID] = isnull(i.CutplanID,'')
,[EstCutdate] = c.EstCutdate
,[SpreadingNoID] = isnull(c.SpreadingNoID,'')
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Roll] = i2.Roll
,[Dyelot] = i2.Dyelot
,[Barcode] = Barcode.value
,[NewBarcode] = NewBarcode.value
,[Ukey] = i2.ukey
,CmdTime = GetDate()
,[Qty] = {strQty}
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,i2.SentToWMS,i2.CompleteTime
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.Id=i.Id
{strBody}
left join Production.dbo.Cutplan c on c.ID = i.CutplanID
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey
)Barcode
outer apply(
	select value = fb.Barcode
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey and fb.TransactionID = i2.Id
)NewBarcode
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
";
                    break;
                case "P16":
                    sqlcmd = $@"
select distinct 
[Id] = i2.Id
,[Type] = '{formName}'
,[CutPlanID] = ''
,[EstCutdate] = null
,[SpreadingNoID] = ''
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Roll] = i2.Roll
,[Dyelot] = i2.Dyelot
,[Barcode] = Barcode.value
,[NewBarcode] = NewBarcode.value
,[Ukey] = i2.Ukey
,[Qty] = {strQty}
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,CmdTime = GetDate()
,i2.SentToWMS,i2.CompleteTime
from Production.dbo.IssueLack_Detail i2
inner join Production.dbo.IssueLack i on i2.id = i.id
{strBody}
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1 = i2.Seq1
	and f.Seq2 = i2.Seq2 and f.Roll = i2.Roll and f.Dyelot = i2.Dyelot
    and f.StockType = i2.StockType
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey
)Barcode
outer apply(
	select value = fb.Barcode
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey and fb.TransactionID = i2.Id
)NewBarcode
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
";
                    break;
                case "P19":
                    sqlcmd = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = '{formName}'
,[CutPlanID] = ''
,[EstCutdate] = null
,[SpreadingNoID] = ''
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Roll] = i2.Roll
,[Dyelot] = i2.Dyelot
,[Barcode] = Barcode.value
,[NewBarcode] = NewBarcode.value
,[Ukey] = i2.ukey
,[Qty] = {strQty}
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,CmdTime = GetDate()
,i2.SentToWMS,i2.CompleteTime
from Production.dbo.TransferOut_Detail i2
inner join Production.dbo.TransferOut i on i2.id = i.id
{strBody}
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey
)Barcode
outer apply(
	select value = fb.Barcode
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey and fb.TransactionID = i2.Id
)NewBarcode
where 1=1
and exists(
		select 1 from Production.dbo.PO_Supp_Detail 
		where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
		and FabricType='F'
	)
";
                    break;
            }

            if (string.Compare(status, "New", true) == 0)
            {
                sqlcmd += Environment.NewLine + @"
and exists(
	select 1
	from FtyInventory_Detail fd 
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where f.Ukey = fd.Ukey
	and ml.IsWMS = 1
)";
            }
            else
            {
                sqlcmd += Environment.NewLine + @" and i2.SentToWMS = 1";
            }

            if (!MyUtility.Check.Empty(sqlcmd))
            {
                if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, sqlcmd, out dtMaster)))
                {
                    MyUtility.Msg.WarningBox(result.Messages.ToString());
                }
            }

            #endregion

            return dtMaster;
        }

        private DataTable GetSubTransfer_Detail(DataTable dtDetail, string status, bool isP99 = false)
        {
            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster;
            string strBody = isP99 ? "inner join #tmp s2 on sd.ukey = s2.ukey " : "inner join #tmp s2 on sd.ID = s2.Id ";
            string strQty = isP99 ? "s2.Qty" : "sd.Qty";

            #region 取得資料
            sqlcmd = $@"
select distinct
[ID] = sd.ID
,Type = 'D'
,sd.FromPOID,sd.FromSeq1,sd.FromSeq2,sd.FromRoll,sd.FromDyelot,sd.FromStockType
,[FromBarcode] = FromBarcode.value
,[FromLocation] = Fromlocation.listValue
,sd.ToPOID,sd.ToSeq1,sd.ToSeq2,sd.ToRoll,sd.ToDyelot,sd.ToStockType
,[ToBarcode] = ToBarcode.value
,[ToLocation] = sd.ToLocation
,po3.Refno
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
                 ,IIF(isnull(po3.SuppColor,'') = '',dbo.GetColorMultipleID(o.BrandID,po3.ColorID),po3.SuppColor)
                 ,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[Qty] = {strQty}
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,CmdTime = GetDate()
,sd.SentToWMS,sd.CompleteTime
,sd.ukey
from Production.dbo.SubTransfer_Detail sd
inner join Production.dbo.SubTransfer s on s.id = sd.id
{strBody}
left join FtyInventory FI on sd.fromPoid = fi.poid 
    and sd.fromSeq1 = fi.seq1 and sd.fromSeq2 = fi.seq2 
    and sd.fromDyelot = fi.Dyelot and sd.fromRoll = fi.roll 
    and sd.fromStocktype = fi.stocktype
left join PO_Supp_Detail po3 on po3.ID = sd.FromPOID and po3.SEQ1 = sd.FromSeq1
and po3.SEQ2 = sd.FromSeq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
outer apply(
	select listValue = Stuff((
			select concat(',',MtlLocationID)
			from (
					select 	distinct
						fd.MtlLocationID
					from FtyInventory_Detail fd
					where fd.Ukey = fi.Ukey
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = fi.Ukey
)FromBarcode
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	inner join FtyInventory toFi on toFi.Ukey = fb.Ukey
	where toFi.POID = sd.ToPOID
	and tofi.Seq1= sd.ToSeq1 and toFi.Seq2 = sd.ToSeq2
	and toFi.Roll = sd.ToRoll and toFi.Dyelot = sd.ToDyelot
	and toFi.StockType = sd.ToStockType
)ToBarcode
where 1=1
and exists(
    select 1 from Production.dbo.PO_Supp_Detail
    where id = sd.ToPOID and seq1=sd.ToSeq1 and seq2=sd.ToSeq2
    and FabricType='F'
)
";

            if (string.Compare(status, "New", true) == 0)
            {
                sqlcmd += Environment.NewLine + @"
and exists(
	select 1
	from MtlLocation ml 
	inner join dbo.SplitString(Fromlocation.listValue,',') sp on sp.Data = ml.ID
        and ml.StockType=sd.FromStockType
	where ml.IsWMS = 1
	union all
	select 1 from MtlLocation ml 
	inner join dbo.SplitString(sd.ToLocation,',') sp on sp.Data = ml.ID
	    and ml.StockType=sd.ToStockType
	where ml.IsWMS = 1
)";
            }
            else
            {
                sqlcmd += Environment.NewLine + @" and sd.SentToWMS = 1";
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, sqlcmd, out dtMaster)))
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
            }

            #endregion

            return dtMaster;
        }

        private DataTable GetReturnReceipt_Detail(DataTable dtDetail, string status, bool isP99 = false)
        {
            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster;
            string strBody = isP99 ? "inner join #tmp s on rrd.ukey = s.ukey " : "inner join #tmp s on rrd.ID = s.Id ";
            string strQty = isP99 ? "s.Qty" : "rrd.Qty";

            #region 取得資料
            sqlcmd = $@"
select rrd.Id
,rrd.POID
,rrd.Seq1
,rrd.Seq2
,rrd.Roll
,rrd.Dyelot
,rrd.StockType
,rrd.Ukey
,[Barcode] = Barcode.value
,[Qty] = {strQty}
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,[CmdTime] = GETDATE()
,rrd.SentToWMS,rrd.CompleteTime
from ReturnReceipt_Detail rrd
inner join Production.dbo.ReturnReceipt rr on rr.id = rrd.id
{strBody}
left join FtyInventory f on rrd.FtyInventoryUkey = f.ukey
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = f.Ukey
)Barcode
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= rrd.POID and psd.SEQ1 = rrd.Seq1
    and psd.SEQ2 = rrd.Seq2 and psd.FabricType='F'
)
";

            if (string.Compare(status, "New", true) == 0)
            {
                sqlcmd += Environment.NewLine + @"
and exists(
	select 1
	from FtyInventory_Detail fd
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where  fd.Ukey=f.Ukey
	and ml.IsWMS =1 
)";
            }
            else
            {
                sqlcmd += Environment.NewLine + @" and rrd.SentToWMS = 1";
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, sqlcmd, out dtMaster)))
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
            }

            #endregion

            return dtMaster;
        }

        private DataTable GetBorrowBack_Detail(DataTable dtDetail, string status, bool isP99 = false)
        {
            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster;
            string strBody = isP99 ? "inner join #tmp s on bb2.ukey = s.ukey " : "inner join #tmp s on bb2.ID = s.Id ";
            string strQty = isP99 ? "s.Qty" : "bb2.Qty";

            #region 取得資料
            sqlcmd = $@"
select distinct
[ID] = bb2.ID
,bb2.FromPOID,bb2.FromSeq1,bb2.FromSeq2,bb2.FromRoll,bb2.FromDyelot,bb2.FromStockType
,[FromBarcode] = FromBarcode.value
,[FromLocation] = Fromlocation.listValue
,bb2.ToPOID,bb2.ToSeq1,bb2.ToSeq2,bb2.ToRoll,bb2.ToDyelot,bb2.ToStockType
,[ToBarcode] = ToBarcode.value
,[ToLocation] = bb2.ToLocation
,[Qty] = {strQty}
,po3.Refno
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
                 ,IIF(isnull(po3.SuppColor,'') = '',dbo.GetColorMultipleID(o.BrandID,po3.ColorID),po3.SuppColor)
                 ,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,bb2.Ukey
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,CmdTime = GetDate()
,bb2.SentToWMS,bb2.CompleteTime
from Production.dbo.BorrowBack_Detail bb2
inner join Production.dbo.BorrowBack bb on bb.id = bb2.id
{strBody}
left join FtyInventory FI on bb2.FromPoid = Fi.Poid and bb2.FromSeq1 = Fi.Seq1 and bb2.FromSeq2 = Fi.Seq2 
    and bb2.FromRoll = Fi.Roll and bb2.FromDyelot = Fi.Dyelot and bb2.FromStockType = FI.StockType
left join PO_Supp_Detail po3 on po3.ID = bb2.FromPOID and po3.SEQ1 = bb2.FromSeq1
    and po3.SEQ2 = bb2.FromSeq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
outer apply(
	select listValue = Stuff((
			select concat(',',FromLocation)
			from (
					select 	distinct
						l.FromLocation
					from dbo.LocationTrans_detail l
					where l.FtyInventoryUkey = fi.Ukey
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	where fb.Ukey = fi.Ukey
)FromBarcode
outer apply(
	select value = min(fb.Barcode)
	from Production.dbo.FtyInventory_Barcode fb
	inner join FtyInventory toFi on toFi.Ukey = fb.Ukey
	where toFi.POID = bb2.ToPOID
	and tofi.Seq1= bb2.ToSeq1 and toFi.Seq2 = bb2.ToSeq2
	and toFi.Roll = bb2.ToRoll and toFi.Dyelot = bb2.ToDyelot
	and toFi.StockType = bb2.ToStockType
)ToBarcode
where 1=1
and exists(
    select 1 from Production.dbo.PO_Supp_Detail
    where id = bb2.ToPOID and seq1=bb2.ToSeq1 and seq2=bb2.ToSeq2
    and FabricType='F'
)
";

            if (string.Compare(status, "New", true) == 0)
            {
                sqlcmd += Environment.NewLine + @"
and exists(
	select 1
	from MtlLocation ml 
	inner join dbo.SplitString(Fromlocation.listValue,',') sp on sp.Data = ml.ID 
		and ml.StockType=bb2.FromStockType
	where ml.IsWMS = 1
union all
	select 1 
    from MtlLocation ml 
    inner join dbo.SplitString(bb2.ToLocation,',') sp on sp.Data = ml.ID 
	    and ml.StockType = bb2.FromStockType
	where ml.IsWMS = 1
)
";
            }
            else
            {
                sqlcmd += Environment.NewLine + @" and bb2.SentToWMS = 1";
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, sqlcmd, out dtMaster)))
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
            }

            #endregion

            return dtMaster;
        }

        private DataTable GetAdjust_Detail(DataTable dtDetail, string status, bool isP99 = false)
        {
            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster;
            string strBody = isP99 ? "inner join #tmp s on i2.ukey = s.ukey " : "inner join #tmp s on i2.ID = s.Id ";
            string strQty = isP99 ? "s.Qty" : "i2.QtyAfter";

            #region 取得資料
            sqlcmd = $@"
select distinct 
 [Id] = i2.Id 
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Ukey] = i2.ukey
,[StockType] = i2.StockType
,[QtyBefore] = i2.QtyBefore
,[Barcode] = f.Barcode
,[QtyAfter] = {strQty}
,[Status] = iif('{status}' = 'UnConfirmed', 'delete' ,'{status}')
,CmdTime = GetDate()
,i2.SentToWMS,i2.CompleteTime
from Production.dbo.Adjust_Detail i2
inner join Production.dbo.Adjust i on i.id = i2.id
{strBody}
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
left join PO_Supp_Detail po3 on po3.ID = i2.POID
	and po3.SEQ1 = i2.Seq1 and po3.SEQ2 = i2.Seq2
where 1=1 
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='F'
)
";
            if (string.Compare(status, "New", true) == 0)
            {
                sqlcmd += Environment.NewLine + @"
and exists(
	select 1
	from FtyInventory_Detail fd 
	inner join MtlLocation ml on ml.ID = fd.MtlLocationID
	where f.Ukey = fd.Ukey
	and ml.IsWMS = 1
)
";
            }
            else
            {
                sqlcmd += Environment.NewLine + @" and i2.SentToWMS = 1";
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, sqlcmd, out dtMaster)))
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
            }

            #endregion

            return dtMaster;
        }

        #endregion

        private void SetAutoAutomationErrMsg(string apiThread, string type = "")
        {
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = type == "New" ? SCIAPIThread : suppAPIThread;
            this.automationErrMsg.moduleName = moduleName;
            this.automationErrMsg.suppID = GensongSuppID;
        }

        private object CreateGensongStructure(string tableName, object structureID)
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
