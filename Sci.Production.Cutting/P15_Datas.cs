namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public static partial class BatchCreateData
    {
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line
        public class NoofBundle
        {
            public long Ukey { get; set; }
            public int No { get; set; }
            public int Iden { get; set; }
            public int PrintGroup { get; set; }
            public int Tone { get; set; }
            public string ToneChar { get; set; }
            public string POID { get; set; }
            public string Article { get; set; }
            public string SizeCode { get; set; }
            public int Qty { get; set; }

            /// <summary>
            /// P15 紀錄要合併的組別. 同數字表示完全一樣: Ukey, Article, Size & 左下資料表
            /// </summary>
            public int Dup { get; set; }
            public int Startno { get; set; }
            public int BuundleGroup { get; set; }
            public string SubCut { get; set; }
            public long StyleUkey { get; set; }
            public string Dyelot { get; set; }
        }

        public class ArticleSize
        {
            public long Ukey { get; set; }
            public int No { get; set; }
            public int Iden { get; set; }
            public long Pkey { get; set; }
            public string Cutref { get; set; }
            public string POID { get; set; }
            public string OrderID { get; set; }
            public string Article { get; set; }
            public string SizeCode { get; set; }
            public string IsEXCESS { get; set; }
            public string ColorID { get; set; }
            public string Fabriccombo { get; set; }
            public string FabricPanelCode { get; set; }
            public string Ratio { get; set; }
            public int Cutno { get; set; }
            public string Sewingline { get; set; }
            public string SewingCell { get; set; }
            public string Item { get; set; }
            public int Qty { get; set; }
            public int Cutoutput { get; set; }
            public int RealCutOutput { get; set; }
            public int TotalParts { get; set; }
            public int Startno { get; set; }
            public long StyleUkey { get; set; }
        }

        public class Pattern
        {
            public string Cutref { get; set; }
            public string Poid { get; set; }
            public long Ukey { get; set; }
            public string PatternCode { get; set; }
            public string PatternDesc { get; set; }
            public string Location { get; set; }
            public int Parts { get; set; }
            public bool Ispair { get; set; }
            public string Art { get; set; }
            public string NoBundleCardAfterSubprocess_String { get; set; }
            public string PostSewingSubProcess_String { get; set; }
            public bool IsMain { get; set; }
            public int CombineSubprocessGroup { get; set; }
            public bool RFIDScan { get; set; }
        }
#pragma warning restore SA1516 // Elements should be separated by blank line
#pragma warning restore SA1600 // Elements should be documented
    }
}
