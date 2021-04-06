using Ict;
using Newtonsoft.Json;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sci.Production.Automation.UtilityAutomation;

namespace Sci.Production.Automation
{
    /// <inheritdoc/>
    public class Vstrong_AutoWHAccessory
    {
        private static readonly string VstrongSuppID = "3A0196";
        private static readonly string moduleName = "AutoWHAccessory";
        private static readonly string SCIAPIThread = "api/VstrongAutoWHAccessory/SentDataByApiTag";
        private static readonly string suppAPIThread = "snpvsinterface/services/pmstowms";
        private static readonly string URL = GetSupplierUrl(VstrongSuppID, moduleName);
        private AutomationErrMsgPMS automationErrMsg = new AutomationErrMsgPMS();

        /// <inheritdoc/>
        public static bool IsVstrong_AutoWHAccessoryEnable => IsModuleAutomationEnable(VstrongSuppID, moduleName);

        #region Receive

        /// <summary>
        /// Sent Receive_Detail New
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="formName">form Name</param>
        public void SentReceive_Detail_New(DataTable dtDetail, string formName = "")
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 取得資料
            DataTable dtMaster = this.GetReceiveData(dtDetail, formName, "New");

            // 沒資料就return
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            // Confirm後要上鎖物料
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
                case "P08":
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
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();

            // 取得資料
            DataTable dtMaster = callMethod.GetReceiveData(dtDetail, formName, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "Receiving_Detail");
            callMethod.SetAutoAutomationErrMsg("SentReceiving_DetailToVstrong", string.Empty);

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
                    case "P08":
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

            return true;
        }
        #endregion

        #region Issue

        /// <summary>
        /// Issue_Detail To Vstrong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="formName">formName</param>
        /// <param name="status">status</param>
        public void SentIssue_Detail_New(DataTable dtDetail, string formName = "", string status = "")
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster = this.GetIssueData(dtDetail, formName, status);
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region Confirmed 後記錄那些資料有傳給WMS
            switch (formName)
            {
                case "P11":
                case "P12":
                case "P13":
                case "P33":
                    if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "Issue"))
                    {
                        return;
                    }

                    break;
                case "P15":
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

            string apiThread = "SentIssue_DetailToVstrong";
            this.SetAutoAutomationErrMsg(apiThread, "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "Issue_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentIssue_Detail_delete
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="formName">formName</param>
        /// <param name="status">status</param>
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentIssue_Detail_delete(DataTable dtDetail, string formName = "", string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();

            // 取得資料
            DataTable dtMaster = callMethod.GetIssueData(dtDetail, formName, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "Issue_Detail");
            callMethod.SetAutoAutomationErrMsg("SentIssue_DetailToVstrong");

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
                    case "P11":
                    case "P12":
                    case "P13":
                    case "P33":
                        if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "Issue"))
                        {
                            return false;
                        }

                        break;
                    case "P15":
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

        #region RemoveC

        /// <summary>
        /// Issue_Detail To Vstrong
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="formName">formName</param>
        /// <param name="status">status</param>
        public void SentRemoveC_Detail_New(DataTable dtDetail, string formName = "", string status = "")
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster = this.GetRemoveC_Detail(dtDetail, status);
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region Confirmed 後記錄那些資料有傳給WMS
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "Adjust"))
            {
                return;
            }

            #endregion

            string apiThread = "SentRemoveC_DetailToVstrong";
            this.SetAutoAutomationErrMsg(apiThread, "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "RemoveC_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentIssue_Detail_delete
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="formName">formName</param>
        /// <param name="status">status</param>
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentRemoveC_Detail_delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();

            // 取得資料
            DataTable dtMaster = callMethod.GetRemoveC_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "RemoveC_Detail");
            callMethod.SetAutoAutomationErrMsg("SentRemoveC_DetailToVstrong");

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

        #region SubTransfer

        /// <summary>
        /// SubTransfer_Detail New
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="status">status</param>
        public void SentSubTransfer_Detail_New(DataTable dtDetail, string status = "")
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster = this.GetSubTransfer_Detail(dtDetail, status);
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region Confirmed 後記錄那些資料有傳給WMS
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "SubTransfer"))
            {
                return;
            }

            #endregion

            string apiThread = "SentSubTransfer_DetailToVstrong";
            this.SetAutoAutomationErrMsg(apiThread, "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "SubTransfer_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// SentSubTransfer_Detail delete
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="status">status</param>
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentSubTransfer_Detail_delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();

            // 取得資料
            DataTable dtMaster = callMethod.GetSubTransfer_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "SubTransfer_Detail");
            callMethod.SetAutoAutomationErrMsg("SentSubTransfer_DetailToVstrong");

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

        #region ReturnReceipt

        /// <summary>
        /// ReturnReceipt_Detail New
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="status">status</param>
        public void SentReturnReceipt_Detail_New(DataTable dtDetail, string status = "")
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster = this.GetReturnReceipt_Detail(dtDetail, status);
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region Confirmed 後記錄那些資料有傳給WMS
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "ReturnReceipt"))
            {
                return;
            }

            #endregion

            string apiThread = "SentReturnReceiptToVstrong";
            this.SetAutoAutomationErrMsg(apiThread, "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "ReturnReceipt_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// ReturnReceipt_Detail delete
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="status">status</param>
        /// <param name = "isP99" > is P99 </ param >
        /// <returns>bool</returns>
        public static bool SentReturnReceipt_Detail_delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();

            // 取得資料
            DataTable dtMaster = callMethod.GetReturnReceipt_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "ReturnReceipt_Detail");
            callMethod.SetAutoAutomationErrMsg("SentReturnReceiptToVstrong");

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
        /// WHClose To Vstrong
        /// </summary>
        /// <param name="dtMaster">Master DataSource</param>
        public void SentWHCloseToVstrongAutoWHAccessory(DataTable dtMaster)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName))
            {
                return;
            }

            string apiThread = "SentWHCloseToVstrong";
            this.SetAutoAutomationErrMsg(apiThread);

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dtMaster.AsEnumerable()
                .Select(dr => new
                {
                    POID = dr["POID"].ToString(),
                    WhseClose = MyUtility.Check.Empty(dr["WhseClose"]) ? null : ((DateTime?)dr["WhseClose"]).Value.ToString("yyyy/MM/dd"),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("WHClose", bodyObject));
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        #region BorrowBack

        /// <summary>
        /// BorrowBack_Detail New
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="status">status</param>
        public void SentBorrowBack_Detail_New(DataTable dtDetail, string status = "")
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster = this.GetBorrowBack_Detail(dtDetail, status);
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region Confirmed 後記錄那些資料有傳給WMS
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "BorrowBack"))
            {
                return;
            }
            #endregion

            string apiThread = "SentBorrowBackToVstrong";
            this.SetAutoAutomationErrMsg(apiThread, "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "BorrowBack_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// BorrowBack_Detail delete
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="status">status</param>
        /// <param name="isP99">isP99</param>
        /// <returns>bool</returns>
        public static bool SentBorrowBack_Detail_delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();

            // 取得資料
            DataTable dtMaster = callMethod.GetBorrowBack_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "BorrowBack_Detail");
            callMethod.SetAutoAutomationErrMsg("SentBorrowBackToVstrong");

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

        #region Adjust

        /// <summary>
        /// Adjust_Detail New
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="status">status</param>
        public void SentAdjust_Detail_New(DataTable dtDetail, string status = "")
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster = this.GetAdjust_Detail(dtDetail, status);
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region Confirmed 後記錄那些資料有傳給WMS
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "Adjust"))
            {
                return;
            }

            #endregion

            string apiThread = "SentAdjust_DetailToVstrong";
            this.SetAutoAutomationErrMsg(apiThread, "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "Adjust_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// Adjust_Detail delete
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="status">status</param>
        /// <param name="isP99">isP99</param>
        /// <returns>bool</returns>
        public static bool SentAdjust_Detail_delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;
            string sqlcmd = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();

            // 取得資料
            DataTable dtMaster = callMethod.GetAdjust_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "Adjust_Detail");
            callMethod.SetAutoAutomationErrMsg("SentAdjust_DetailToVstrong");

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

        #region IssueReturn

        /// <summary>
        /// IssueReturn_Detail New
        /// </summary>
        /// <param name="dtDetail">Detail DataSource</param>
        /// <param name="status">status</param>
        public void SentIssueReturn_Detail_New(DataTable dtDetail, string status = "")
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return;
            }

            string sqlcmd = string.Empty;
            DataTable dtMaster = this.GetIssueReturn_Detail(dtDetail, status);
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            #region Confirmed 後記錄那些資料有傳給WMS
            if (!PublicPrg.Prgs.SentToWMS(dtMaster, true, "IssueReturn"))
            {
                return;
            }

            #endregion

            string apiThread = "SentIssueReturn_DetailToVstrong";
            this.SetAutoAutomationErrMsg(apiThread, "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dtMaster, "IssueReturn_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// IssueReturn_Detail delete
        /// </summary>
        /// <param name="dtDetail">dtDetail</param>
        /// <param name="status">status</param>
        /// <param name="isP99">isP99</param>
        /// <returns>bool</returns>
        public static bool SentIssueReturn_Detail_delete(DataTable dtDetail, string status = "", bool isP99 = false)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtDetail.Rows.Count <= 0)
            {
                return true;
            }

            DualResult result;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();

            // 取得資料
            DataTable dtMaster = callMethod.GetIssueReturn_Detail(dtDetail, status, isP99);

            // 如果沒資料,代表不須傳給WMS還是可以unConfirmed, 所以不須回傳false
            if (dtMaster == null || dtMaster.Rows.Count <= 0)
            {
                return true;
            }

            // DataTable轉化為JSON
            string jsonBody = callMethod.GetJsonBody(dtMaster, "IssueReturn_Detail");
            callMethod.SetAutoAutomationErrMsg("SentIssueReturn_DetailToVstrong");

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
                if (!PublicPrg.Prgs.SentToWMS(dtMaster, false, "IssueReturn"))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        /// <summary>
        /// LocationTrans To Vstrong
        /// </summary>
        /// <param name="dtMaster">dtMaster</param>
        /// <param name="status">status</param>
        public void SentLocationTrans_detail_New(DataTable dtMaster, string status = "")
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string sqlcmd = $@"
select lt2.Id
,lt2.POID
,lt2.Seq1
,lt2.Seq2
,lt2.FromLocation
,lt2.ToLocation
,po3.Refno
,[StockUnit] = dbo.GetStockUnitBySpSeq (f.POID, f.seq1, f.seq2)
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
    ,IIF(Isnull(po3.SuppColor,'')='',dbo.GetColorMultipleID(o.BrandID,po3.ColorID),po3.SuppColor)
    ,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[SizeCode] = po3.SizeSpec
,[MtlType] = Fabric.MtlTypeID
,lt2.Ukey
,lt2.StockType
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,[Qty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
,[CmdTime] = GETDATE()
from LocationTrans_detail lt2
inner join #tmp lt on lt.Id=lt2.Id
left join FtyInventory f on lt2.FtyInventoryUkey = f.ukey
left join PO_Supp_Detail po3 on po3.ID = lt2.POID and po3.SEQ1 = lt2.Seq1
    and po3.SEQ2 = lt2.Seq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo = Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= lt2.POID and psd.SEQ1 = lt2.Seq1
    and psd.SEQ2 = lt2.Seq2 and psd.FabricType='A'
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
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, sqlcmd, out dt)))
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
                return;
            }

            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            #region Confirmed 後記錄那些資料有傳給WMS
            if (!PublicPrg.Prgs.SentToWMS(dt, true, "LocationTrans"))
            {
                return;
            }

            #endregion

            string apiThread = "SentLocationTrans_DetailToVstrong";
            this.SetAutoAutomationErrMsg(apiThread, "New");

            // 將DataTable 轉成Json格式
            string jsonBody = this.GetJsonBody(dt, "LocationTrans_Detail");

            // Call API傳送給WMS
            SendWebAPI(GetSciUrl(), this.automationErrMsg.suppAPIThread, jsonBody, this.automationErrMsg);
        }

        /// <summary>
        /// IssueReturn_Detail To Vstrong
        /// </summary>
        /// <param name="dtMaster">dtMaster</param>
        /// <param name="isConfirmed">bool</param>
        public void SentIssueReturn_DetailToVstrongAutoWHAccessory(DataTable dtMaster, bool isConfirmed)
        {
            if (!IsModuleAutomationEnable(VstrongSuppID, moduleName) || dtMaster.Rows.Count <= 0)
            {
                return;
            }

            string apiThread = "SentIssueReturn_DetailToVstrong";
            string suppAPIThread = "Api/VstrongAutoWHAccessory/SentDataByApiTag";
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = suppAPIThread;

            string sqlcmd = $@"
select ir2.Id
,ir2.POID
,ir2.Seq1
,ir2.Seq2
,po3.Refno
,[StockUnit] = dbo.GetStockUnitBySpSeq (f.POID, f.seq1, f.seq2)
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
    ,IIF(Isnull(po3.SuppColor,'')='',dbo.GetColorMultipleID(o.BrandID,po3.ColorID),po3.SuppColor)
    ,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[SizeCode] = po3.SizeSpec
,[MtlType] = Fabric.MtlTypeID
,ir2.Ukey
,ir2.StockType
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
,[Qty] = ir2.Qty
,[CmdTime] = GETDATE()
from IssueReturn_Detail ir2
inner join #tmp lt on lt.Id=ir2.Id
left join FtyInventory f on ir2.FtyInventoryUkey = f.ukey
left join PO_Supp_Detail po3 on po3.ID = ir2.POID and po3.SEQ1 = ir2.Seq1
and po3.SEQ2 = ir2.Seq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= ir2.POID and psd.SEQ1 = ir2.Seq1
    and psd.SEQ2 = ir2.Seq2 and psd.FabricType='A'
)

";
            DataTable dt = new DataTable();
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, sqlcmd, out dt)))
            {
                return;
            }

            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }

            dynamic bodyObject = new ExpandoObject();
            bodyObject = dt.AsEnumerable()
                .Select(s => new
                {
                    ID = s["ID"].ToString(),
                    POID = s["POID"].ToString(),
                    Seq1 = s["Seq1"].ToString(),
                    Seq2 = s["Seq2"].ToString(),
                    StockType = s["StockType"].ToString(),
                    Ukey = (long)s["Ukey"],
                    Refno = s["Refno"].ToString(),
                    StockUnit = s["StockUnit"].ToString(),
                    Color = s["Color"].ToString(),
                    SizeCode = s["SizeCode"].ToString(),
                    MtlType = s["MtlType"].ToString(),
                    Qty = (decimal)s["Qty"],
                    Status = s["Status"].ToString(),
                    CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                });

            string jsonBody = JsonConvert.SerializeObject(this.CreateVstrongStructure("IssueReturn_Detail", bodyObject));
            SendWebAPI(GetSciUrl(), suppAPIThread, jsonBody, this.automationErrMsg);
        }

        #region 資料邏輯層
        private string GetJsonBody(DataTable dtDetail, string type)
        {
            string jsonBody = string.Empty;

            // 呼叫同個Class裡的Method,需要先new物件才行
            Vstrong_AutoWHAccessory callMethod = new Vstrong_AutoWHAccessory();
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
                        Refno = dr["Refno"].ToString().TrimEnd(),
                        StockUnit = dr["StockUnit"].ToString(),
                        StockQty = (decimal)dr["StockQty"],
                        PoUnit = dr["PoUnit"].ToString(),
                        ShipQty = (decimal)dr["ShipQty"],
                        Color = dr["Color"].ToString().TrimEnd(),
                        SizeCode = dr["SizeCode"].ToString().TrimEnd(),
                        Weight = (decimal)dr["Weight"],
                        StockType = dr["StockType"].ToString(),
                        MtlType = dr["MtlType"].ToString(),
                        Ukey = (long)dr["Ukey"],
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
                       ID = dr["ID"].ToString(),
                       Type = dr["Type"].ToString(),
                       PoId = dr["PoId"].ToString(),
                       Seq1 = dr["Seq1"].ToString(),
                       Seq2 = dr["Seq2"].ToString(),
                       Color = dr["Color"].ToString().TrimEnd(),
                       SizeCode = dr["SizeCode"].ToString().TrimEnd(),
                       StockType = dr["StockType"].ToString(),
                       Qty = (decimal)dr["Qty"],
                       StockPoId = dr["StockPoId"].ToString(),
                       StockSeq1 = dr["StockSeq1"].ToString(),
                       StockSeq2 = dr["StockSeq2"].ToString(),
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
                            Ukey = (long)dr["Ukey"],
                            StockType = dr["StockType"].ToString(),
                            QtyBefore = (decimal)dr["QtyBefore"],
                            QtyAfter = (decimal)dr["QtyAfter"],
                            Status = dr["Status"].ToString(),
                            CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        });
                    break;
                case "RemoveC_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                        .Select(dr => new
                        {
                            Id = dr["id"].ToString(),
                            PoId = dr["PoId"].ToString(),
                            Seq1 = dr["Seq1"].ToString(),
                            Seq2 = dr["Seq2"].ToString(),
                            Qty = (decimal)dr["Qty"],
                            Ukey = (long)dr["Ukey"],
                            StockType = dr["StockType"].ToString(),
                            Status = dr["Status"].ToString(),
                            CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        });
                    break;
                case "SubTransfer_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                    .Select(dr => new
                    {
                        Id = dr["id"].ToString(),
                        Type = dr["Type"].ToString(),
                        FromPoId = dr["FromPoId"].ToString(),
                        FromSeq1 = dr["FromSeq1"].ToString(),
                        FromSeq2 = dr["FromSeq2"].ToString(),
                        FromStockType = dr["FromStockType"].ToString(),
                        FromLocation = dr["FromLocation"].ToString(),
                        ToPoId = dr["ToPoId"].ToString(),
                        ToSeq1 = dr["ToSeq1"].ToString(),
                        ToSeq2 = dr["ToSeq2"].ToString(),
                        ToStockType = dr["ToStockType"].ToString(),
                        ToLocation = dr["ToLocation"].ToString(),
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
                       Qty = (decimal)dr["Qty"],
                       StockType = dr["StockType"].ToString(),
                       Ukey = (long)dr["Ukey"],
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
                       FromStockType = dr["FromStockType"].ToString(),
                       FromLocation = dr["FromLocation"].ToString(),
                       ToPOID = dr["ToPOID"].ToString(),
                       ToSeq1 = dr["ToSeq1"].ToString(),
                       ToSeq2 = dr["ToSeq2"].ToString(),
                       ToStockType = dr["ToStockType"].ToString(),
                       ToLocation = dr["ToLocation"].ToString(),
                       Refno = dr["Refno"].ToString(),
                       StockUnit = dr["StockUnit"].ToString(),
                       Color = dr["Color"].ToString(),
                       SizeCode = dr["SizeCode"].ToString(),
                       MtlType = dr["MtlType"].ToString(),
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
                       FromLocation = s["FromLocation"].ToString(),
                       ToLocation = s["ToLocation"].ToString(),
                       Refno = s["Refno"].ToString(),
                       StockUnit = s["StockUnit"].ToString(),
                       Color = s["Color"].ToString(),
                       SizeCode = s["SizeCode"].ToString(),
                       MtlType = s["MtlType"].ToString(),
                       Qty = (decimal)s["Qty"],
                       Ukey = (long)s["Ukey"],
                       StockType = s["StockType"].ToString(),
                       Status = s["Status"].ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                   });
                    break;

                case "IssueReturn_Detail":
                    bodyObject = dtDetail.AsEnumerable()
                   .Select(s => new
                   {
                       ID = s["ID"].ToString(),
                       POID = s["POID"].ToString(),
                       Seq1 = s["Seq1"].ToString(),
                       Seq2 = s["Seq2"].ToString(),
                       StockType = s["StockType"].ToString(),
                       Ukey = (long)s["Ukey"],
                       Refno = s["Refno"].ToString(),
                       StockUnit = s["StockUnit"].ToString(),
                       Color = s["Color"].ToString(),
                       SizeCode = s["SizeCode"].ToString(),
                       MtlType = s["MtlType"].ToString(),
                       Qty = (decimal)s["Qty"],
                       Status = s["Status"].ToString(),
                       CmdTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
                   });
                    break;
            }

            return jsonBody = JsonConvert.SerializeObject(callMethod.CreateVstrongStructure(type, bodyObject));
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
                case "P08":
                    sqlcmd = $@"SELECT 
 [ID] = rd.id
,[InvNo] = iif('{formName}' = 'P07', r.InvNo, '')
,[PoId] = rd.Poid
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = po3.Refno
,[StockUnit] = iif('{formName}' = 'P07',rd.StockUnit,dbo.GetStockUnitBySPSeq(rd.PoId,rd.Seq1,rd.Seq2))
,[StockQty] = {strQty}
,[PoUnit] = iif('{formName}' = 'P07',rd.PoUnit,'')
,[ShipQty] = iif('{formName}' = 'P07',rd.ShipQty,0.00)
,[Color] = Color.Value
,[SizeCode] = po3.SizeSpec
,[Weight] = iif('{formName}' = 'P07',rd.Weight,0.00)
,[StockType] = rd.StockType
,[MtlType] = Fabric.MtlTypeID
,[Ukey] = rd.Ukey
,[ETA] = iif('{formName}' = 'P07',r.ETA,null)
,[WhseArrival] = iif('{formName}' = 'P07',r.WhseArrival,null)
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
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
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
	and FabricType='A'
)
";
                    break;
                case "P18":
                    strQty = isP99 ? "s.Qty" : "rd.Qty";
                    sqlcmd = $@"
SELECT 
 [ID] = rd.id
,[InvNo] = r.InvNo
,[PoId] = rd.Poid
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = po3.Refno
,[StockUnit] = dbo.GetStockUnitBySPSeq(rd.POID,rd.Seq1,rd.Seq2)
,[StockQty] = {strQty}
,[PoUnit] = ''
,[ShipQty] = 0.00
,[Color] =  Color.Value
,[SizeCode] = po3.SizeSpec
,[Weight] = rd.Weight
,[StockType] = rd.StockType
,[MtlType] = Fabric.MtlTypeID
,[Ukey] = rd.Ukey
,[ETA] = null
,[WhseArrival] = r.IssueDate
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
FROM Production.dbo.TransferIn_Detail rd
inner join TransferIn r on rd.ID=r.Id
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
	 CASE WHEN Fabric.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF( isnull(po3.SuppColor,'') = '',dbo.GetColorMultipleID(po3.BrandID,po3.ColorID),po3.SuppColor)
		 ELSE dbo.GetColorMultipleID(po3.BrandID,po3.ColorID)
	 END
)Color
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
	and FabricType='A'
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
                case "P11":
                case "P12":
                case "P13":
                case "P33":
                    sqlcmd = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = '{formName}'
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Color] = po3.ColorID
,[SizeCode] = po3.SizeSpec
,[StockType] = i2.StockType
,[Qty] = {strQty}
,[StockPOID] = po3.StockPOID
,[StockSeq1] = po3.StockSeq1
,[StockSeq2] = po3.StockSeq2
,[Ukey] = i2.ukey
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,CmdTime = GetDate()
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.id = i.id
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
	and FabricType='A'
)
";
                    break;
                case "P15":
                    sqlcmd = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = '{formName}'
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Color] = po3.ColorID
,[SizeCode] = po3.SizeSpec
,[StockType] = i2.StockType
,[Qty] = {strQty}
,[StockPOID] = po3.StockPOID
,[StockSeq1] = po3.StockSeq1
,[StockSeq2] = po3.StockSeq2
,[Ukey] = i2.ukey
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,CmdTime = GetDate()
from Production.dbo.IssueLack_Detail i2
inner join Production.dbo.IssueLack i on i2.id = i.id
{strBody}
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
left join PO_Supp_Detail po3 on po3.ID = i2.POID
	and po3.SEQ1 = i2.Seq1 and po3.SEQ2 = i2.Seq2
where i.Type = 'R' and i.FabricType='A'
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='A'
)
";
                    break;
                case "P19":
                    sqlcmd = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = '{formName}'
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Color] = po3.ColorID
,[SizeCode] = po3.SizeSpec
,[StockType] = i2.StockType
,[Qty] = {strQty}
,[StockPOID] = po3.StockPOID
,[StockSeq1] = po3.StockSeq1
,[StockSeq2] = po3.StockSeq2
,[Ukey] = i2.ukey
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,CmdTime = GetDate()
from Production.dbo.TransferOut_Detail i2
inner join Production.dbo.TransferOut i on i2.id = i.id
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
	and FabricType='A'
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

        private DataTable GetRemoveC_Detail(DataTable dtDetail, string status, bool isP99 = false)
        {
            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster;
            string strBody = isP99 ? "inner join #tmp s on i2.ukey = s.ukey " : "inner join #tmp s on i2.ID = s.Id ";
            string strQty = isP99 ? "i2.QtyBefore - s.Qty" : "i2.QtyBefore - i2.QtyAfter";

            #region 取得資料

            sqlcmd = $@"
select distinct 
 [Id] = i2.Id 
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Qty]  = {strQty}
,[Ukey] = i2.ukey
,i2.StockType
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,CmdTime = GetDate()
from Production.dbo.Adjust_Detail i2
inner join Production.dbo.Adjust i on i2.id = i.id
{strBody}
left join Production.dbo.FtyInventory f on f.POID = i2.POID and f.Seq1=i2.Seq1
	and f.Seq2=i2.Seq2 and f.Roll=i2.Roll and f.Dyelot=i2.Dyelot
    and f.StockType = i2.StockType
left join PO_Supp_Detail po3 on po3.ID = i2.POID
	and po3.SEQ1 = i2.Seq1 and po3.SEQ2 = i2.Seq2
where 1 = 1 
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
	and FabricType='A'
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
)";
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
,s.Type
,sd.FromPOID,sd.FromSeq1,sd.FromSeq2,sd.FromStockType
,[FromLocation] = Fromlocation.listValue
,sd.ToPOID,sd.ToSeq1,sd.ToSeq2,sd.ToStockType
,[ToLocation] = sd.ToLocation
,[Qty] = {strQty}
,sd.Ukey
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,CmdTime = GetDate()
from Production.dbo.SubTransfer_Detail sd
inner join Production.dbo.SubTransfer s on s.id = sd.id
{strBody}
left join FtyInventory FI on sd.fromPoid = fi.poid 
    and sd.fromSeq1 = fi.seq1 and sd.fromSeq2 = fi.seq2 
    and sd.fromDyelot = fi.Dyelot and sd.fromRoll = fi.roll 
    and sd.fromStocktype = fi.stocktype
outer apply(
	select listValue = Stuff((
			select concat(',',MtlLocationID)
			from (
					select	distinct fd.MtlLocationID
					from dbo.FtyInventory_Detail fd
					where fd.Ukey = fi.Ukey
				) s
			for xml path ('')
		) , 1, 1, '')
)Fromlocation
where 1=1
and exists(
    select 1 from Production.dbo.PO_Supp_Detail
    where id = sd.ToPOID and seq1=sd.ToSeq1 and seq2=sd.ToSeq2
    and FabricType='A'
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
,[Qty] = {strQty}
,rrd.StockType
,rrd.Ukey
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,[CmdTime] = GETDATE()
from ReturnReceipt_Detail rrd
inner join Production.dbo.ReturnReceipt rr on rr.id = rrd.id
{strBody}
left join FtyInventory f on rrd.FtyInventoryUkey = f.ukey
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= rrd.POID and psd.SEQ1 = rrd.Seq1
    and psd.SEQ2 = rrd.Seq2 and psd.FabricType='A'
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
,bb2.FromPOID,bb2.FromSeq1,bb2.FromSeq2,bb2.FromStockType
,[FromLocation] = Fromlocation.listValue
,bb2.ToPOID,bb2.ToSeq1,bb2.ToSeq2,bb2.ToStockType
,[ToLocation] = bb2.ToLocation
,po3.Refno
,[StockUnit] = dbo.GetStockUnitBySpSeq (fi.POID, fi.seq1, fi.seq2)
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
    ,IIF(Isnull(po3.SuppColor,'')='',dbo.GetColorMultipleID(o.BrandID,po3.ColorID),po3.SuppColor)
    ,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[SizeCode] = po3.SizeSpec
,[MtlType] = Fabric.MtlTypeID
,[Qty] = {strQty}
,bb2.Ukey
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,CmdTime = GetDate()
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
where 1=1
and exists(
    select 1 from Production.dbo.PO_Supp_Detail
    where id = bb2.ToPOID and seq1=bb2.ToSeq1 and seq2=bb2.ToSeq2
    and FabricType='A'
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
,[QtyAfter] = {strQty}
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,CmdTime = GetDate()
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
	and FabricType='A'
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

        private DataTable GetIssueReturn_Detail(DataTable dtDetail, string status, bool isP99 = false)
        {
            DualResult result;
            string sqlcmd = string.Empty;
            DataTable dtMaster = new DataTable();
            string strBody = isP99 ? "inner join #tmp s on ir2.ukey = s.ukey " : "inner join #tmp s on ir2.ID = s.Id ";
            string strQty = isP99 ? "s.Qty" : "ir2.Qty";

            #region 取得資料

            sqlcmd = $@"
select ir2.Id
,ir2.POID
,ir2.Seq1
,ir2.Seq2
,po3.Refno
,[StockUnit] = dbo.GetStockUnitBySpSeq (f.POID, f.seq1, f.seq2)
,[Color] = isnull(IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
    ,IIF(Isnull(po3.SuppColor,'')='',dbo.GetColorMultipleID(o.BrandID,po3.ColorID),po3.SuppColor)
    ,dbo.GetColorMultipleID(o.BrandID,po3.ColorID)),'') 
,[SizeCode] = po3.SizeSpec
,[MtlType] = Fabric.MtlTypeID
,ir2.Ukey
,ir2.StockType
,[Status] = iif('{status}' = 'UnConfirmed', 'Delete' ,'{status}')
,[Qty] = {strQty}
,[CmdTime] = GETDATE()
from IssueReturn_Detail ir2
inner join Production.dbo.IssueReturn i on ir2.id = i.id
{strBody}
left join FtyInventory f on ir2.FtyInventoryUkey = f.ukey
left join PO_Supp_Detail po3 on po3.ID = ir2.POID and po3.SEQ1 = ir2.Seq1
and po3.SEQ2 = ir2.Seq2
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
LEFT JOIN Orders o WITH (NOLOCK) ON o.ID = po3.ID
where 1=1
and exists(
    select 1
    from PO_Supp_Detail psd
    where psd.ID= ir2.POID and psd.SEQ1 = ir2.Seq1
    and psd.SEQ2 = ir2.Seq2 and psd.FabricType='A'
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
)";
            }
            else
            {
                sqlcmd += Environment.NewLine + @" and ir2.SentToWMS = 1";
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

        #endregion

        private object CreateVstrongStructure(string tableName, object structureID)
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

        private void SetAutoAutomationErrMsg(string apiThread, string type = "")
        {
            this.automationErrMsg.apiThread = apiThread;
            this.automationErrMsg.suppAPIThread = type == "New" ? SCIAPIThread : suppAPIThread;
            this.automationErrMsg.moduleName = moduleName;
            this.automationErrMsg.suppID = VstrongSuppID;
        }
    }
}
