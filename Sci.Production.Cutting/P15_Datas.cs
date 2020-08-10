using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public static partial class BatchCreateData
    {
        /// <inheritdoc/>
        public class NoofBundle
        {
            /// <inheritdoc/>
            public long Ukey { get; set; }

            /// <inheritdoc/>
            public int No { get; set; }

            /// <inheritdoc/>
            public int Iden { get; set; }

            /// <inheritdoc/>
            public int Tone { get; set; }

            /// <inheritdoc/>
            public string Article { get; set; }

            /// <inheritdoc/>
            public string SizeCode { get; set; }

            /// <inheritdoc/>
            public int Qty { get; set; }

            /// <summary>
            /// P15 紀錄要合併的組別. 同數字表示完全一樣: Ukey, Article, Size & 左下資料表
            /// </summary>
            public int Dup { get; set; }

            /// <summary>
            /// P15 Create 準備寫入資料過程, 紀錄當前這筆已處理過
            /// </summary>
            public bool Ran { get; set; }

            /// <summary>
            /// P15 紀錄寫入的 BuundleGroup ,以便合併組找回, 寫入相同的 BuundleGroup
            /// </summary>
            public int BuundleGroup { get; set; }
        }

        /// <inheritdoc/>
        public class Pattern
        {
            /// <inheritdoc/>
            public string Cutref { get; set; }

            /// <inheritdoc/>
            public string Poid { get; set; }

            /// <inheritdoc/>
            public long Ukey { get; set; }

            /// <inheritdoc/>
            public int Iden { get; set; }

            /// <inheritdoc/>
            public string PatternCode { get; set; }

            /// <inheritdoc/>
            public string PatternDesc { get; set; }

            /// <inheritdoc/>
            public string Location { get; set; }

            /// <inheritdoc/>
            public int Parts { get; set; }

            /// <inheritdoc/>
            public bool Ispair { get; set; }

            /// <inheritdoc/>
            public string Art { get; set; }

            /// <inheritdoc/>
            public string NoBundleCardAfterSubprocess_String { get; set; }

            /// <inheritdoc/>
            public string PostSewingSubProcess_String { get; set; }

            /// <summary>
            /// P15 Create 準備寫入資料過程, 紀錄當前這筆已處理過
            /// </summary>
            public bool Ran { get; set; }
        }

        /// <inheritdoc/>
        public class StartNo_SP
        {
            /// <inheritdoc/>
            public string OrderID { get; set; }

            /// <inheritdoc/>
            public int Startno { get; set; }
        }
    }
}
