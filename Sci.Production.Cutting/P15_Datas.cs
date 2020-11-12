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
            public string ToneChar { get; set; }

            /// <inheritdoc/>
            public string POID { get; set; }

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

            /// <inheritdoc/>
            public int Startno { get; set; }

            /// <inheritdoc/>
            public int BuundleGroup { get; set; }

            /// <summary>
            /// P15 Create 準備寫入資料過程, 紀錄當前這筆已處理過
            /// </summary>
            public bool Ran { get; set; }

            /// <inheritdoc/>
            public long StyleUkey { get; set; }
        }

        /// <inheritdoc/>
        public class ArticleSize
        {
            /// <inheritdoc/>
            public long Ukey { get; set; }

            /// <inheritdoc/>
            public int No { get; set; }

            /// <inheritdoc/>
            public int Iden { get; set; }

            /// <inheritdoc/>
            public long Pkey { get; set; }

            /// <inheritdoc/>
            public string Cutref { get; set; }

            /// <inheritdoc/>
            public string POID { get; set; }

            /// <inheritdoc/>
            public string OrderID { get; set; }

            /// <inheritdoc/>
            public string Article { get; set; }

            /// <inheritdoc/>
            public string SizeCode { get; set; }

            /// <inheritdoc/>
            public string IsEXCESS { get; set; }

            /// <inheritdoc/>
            public string ColorID { get; set; }

            /// <inheritdoc/>
            public string Fabriccombo { get; set; }

            /// <inheritdoc/>
            public string FabricPanelCode { get; set; }

            /// <inheritdoc/>
            public string Ratio { get; set; }

            /// <inheritdoc/>
            public int Cutno { get; set; }

            /// <inheritdoc/>
            public string Sewingline { get; set; }

            /// <inheritdoc/>
            public string SewingCell { get; set; }

            /// <inheritdoc/>
            public string Item { get; set; }

            /// <inheritdoc/>
            public int Qty { get; set; }

            /// <inheritdoc/>
            public int Cutoutput { get; set; }

            /// <inheritdoc/>
            public int RealCutOutput { get; set; }

            /// <inheritdoc/>
            public int TotalParts { get; set; }

            /// <inheritdoc/>
            public int Startno { get; set; }

            /// <inheritdoc/>
            public long StyleUkey { get; set; }
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
        }
    }
}
