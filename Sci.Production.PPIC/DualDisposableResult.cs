using Ict;
using System;

// 請有興趣使用但是目前沒有那個領域的控制項，而想加新東西的人；
// 或是目前有那個領域東西，但是一些通用行為不支援的情況
// 找Evaon，不要自己加，感謝
namespace Sci.Production.Class
{
    /// <summary>
    /// 合併原本的DualResult物件，並且讓裡面含有一個ExtendedData屬性來放置要回傳的主要結果(該結果必須有實作IDisposable)
    /// </summary>
    /// <typeparam name="T">IDisposable</typeparam>
    public class DualDisposableResult<T> : IDisposable
        where T : class, IDisposable
    {
        /// <summary>
        /// 是否要隨著Dispose事件，連帶把ExtendedData也呼叫Dispose()，預設為true，代表殼物件Dispoes時，會連帶Dispose ExtendedData
        /// </summary>
        public bool? DisposeExtendedData { get; set; }

        /// <summary>
        /// 主要回傳結果，型態由殼層定義，有實作IDisposable
        /// </summary>
        public T ExtendedData { get; set; }

        /// <summary>
        /// 儲存真正的DualResult，以供呼叫端地取用(例如BaseForm.ShowErr就會使用這個屬性)
        /// </summary>
        public DualResult InnerResult { get; set; }

        /// <summary>
        /// DualDisposableResult
        /// </summary>
        /// <param name="innerResult">DualResult</param>
        public DualDisposableResult(DualResult innerResult)
        {
            this.InnerResult = innerResult;
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public string Description
        {
            get
            {
                return this.InnerResult.Description;
            }
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns>Exception</returns>
        public Exception GetException()
        {
            return this.InnerResult.GetException();
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.InnerResult.IsEmpty;
            }
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public BaseResult.MessageInfos Messages
        {
            get
            {
                return this.InnerResult.Messages;
            }
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public IResult Parent
        {
            get
            {
                return this.InnerResult.Parent;
            }
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public bool Result
        {
            get
            {
                return this.InnerResult.Result;
            }
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public int StatusCode
        {
            get
            {
                return this.InnerResult.StatusCode;
            }
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public string StatusDesc
        {
            get
            {
                return this.InnerResult.StatusDesc;
            }
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns>BaseResult.MessageInfos</returns>
        public BaseResult.MessageInfos ToMessages()
        {
            return this.InnerResult.ToMessages();
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns>string</returns>
        public string ToSimpleString()
        {
            return this.InnerResult.ToSimpleString();
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return this.InnerResult.ToString();
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public void Dispose()
        {
            if (this.DisposeExtendedData.GetValueOrDefault(true) == true)
            {
                if (this.ExtendedData != null)
                {
                    this.ExtendedData.Dispose();
                    this.ExtendedData = null;
                }
            }
        }

        /// <summary>
        /// 隱含轉換布林結果
        /// </summary>
        /// <param name="result"><![CDATA[DualDisposableResult<T> result]]></param>
        /// <returns>bool</returns>
        public static implicit operator bool(DualDisposableResult<T> result)
        {
            return result.Result;
        }
    }
}