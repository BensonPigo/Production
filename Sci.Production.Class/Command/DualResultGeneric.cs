using Ict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Class.Command
{
    public class DualResult<T>
    {
        /// <summary>
        /// 主要回傳結果，型態由殼層定義
        /// </summary>
        public T ExtendedData { get; set; }

        /// <summary>
        /// 儲存真正的DualResult，以供呼叫端地取用(例如BaseForm.ShowErr就會使用這個屬性)
        /// </summary>
        public DualResult InnerResult { get; set; }

        /// <summary>
        /// DualResult
        /// </summary>
        /// <inheritdoc />
        public DualResult(DualResult innerResult)
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
        /// <inheritdoc />
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
        /// <inheritdoc />
        public BaseResult.MessageInfos ToMessages()
        {
            return this.InnerResult.ToMessages();
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <inheritdoc />
        public string ToSimpleString()
        {
            return this.InnerResult.ToSimpleString();
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <inheritdoc />
        public override string ToString()
        {
            return this.InnerResult.ToString();
        }

        /// <summary>
        /// 隱含轉換布林結果
        /// </summary>
        /// <inheritdoc />
        public static implicit operator bool(DualResult<T> result)
        {
            return result.Result;
        }
    }
}
