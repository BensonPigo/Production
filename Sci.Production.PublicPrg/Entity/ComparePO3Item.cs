using System;
using System.Data;

namespace Sci.Production.Prg.Entity
{
    public class ComparePO3Item : PO_Supp_DetailCls
    {
        /// <summary>
        /// 比對狀態
        /// </summary>
        public ComparePO3StateEnu CompareState { get; set; } = ComparePO3StateEnu.None;

        public ComparePO3VersionEnu Version { get; set; } = ComparePO3VersionEnu.Old;

        private string mSuppID;

        /// <summary>
        /// SuppID
        /// </summary>
        public string SuppID
        {
            get { return this.mSuppID; }
            set { this.mSuppID = value; }
        }

        private string mShowCfmETD;

        /// <summary>
        /// ShowCfmETD
        /// </summary>
        public string ShowCfmETD
        {
            get { return this.mShowCfmETD; }
            set { this.mShowCfmETD = value; }
        }

        private string mShowRevisedETD;

        /// <summary>
        /// ShowRevisedETD
        /// </summary>
        public string ShowRevisedETD
        {
            get { return this.mShowRevisedETD; }
            set { this.mShowRevisedETD = value; }
        }

        private string mFirstETA;

        /// <summary>
        /// FirstETA
        /// </summary>
        public string FirstETA
        {
            get { return this.mFirstETA; }
            set { this.mFirstETA = value; }
        }

        private decimal? mBalanceQty;
        private decimal? mStockBalanceQty;

        /// <summary>
        /// BalanceQty
        /// </summary>
        public decimal? BalanceQty
        {
            get { return this.mBalanceQty; }
            set { this.mBalanceQty = value; }
        }

        /// <summary>
        /// StockBalanceQty
        /// </summary>
        public decimal? StockBalanceQty
        {
            get { return this.mStockBalanceQty; }
            set { this.mStockBalanceQty = value; }
        }

        public ComparePO3GroupItem GroupKey = new ComparePO3GroupItem();

        public ComparePO3Item(ComparePO3VersionEnu version)
        {
            this.Version = version;
        }

        public ComparePO3Item(DataRow row, ComparePO3VersionEnu version)
            : base(row)
        {
            this.Version = version;

            if (row.Table.Columns.Contains("SuppID"))
            {
                this.SuppID = row["SuppID"].ToString();
            }

            if (row.Table.Columns.Contains("ShowCfmETD"))
            {
                this.ShowCfmETD = row["ShowCfmETD"] == DBNull.Value ? string.Empty : Convert.ToDateTime(row["ShowCfmETD"]).ToShortDateString();
            }

            if (row.Table.Columns.Contains("ShowRevisedETD"))
            {
                this.ShowRevisedETD = row["ShowRevisedETD"] == DBNull.Value ? string.Empty : Convert.ToDateTime(row["ShowRevisedETD"]).ToShortDateString();
            }

            if (row.Table.Columns.Contains("FirstETA"))
            {
                this.FirstETA = row["FirstETA"] == DBNull.Value ? string.Empty : Convert.ToDateTime(row["FirstETA"]).ToShortDateString();
            }

            if (row.Table.Columns.Contains("StockUnit"))
            {
                this.StockUnit = row["StockUnit"].ToString();
            }

            this.GroupKey = new ComparePO3GroupItem();
            this.GroupKey.ID = this.ID;
            this.GroupKey.RefNo = this.RefNo;
            this.GroupKey.Color = this.SpecColor;
            this.GroupKey.Size = this.SpecSize;
            this.GroupKey.SizeUnit = this.SpecSizeUnit;
            this.GroupKey.ZipperInsert = this.SpecZipperInsert;
            this.GroupKey.Article = this.SpecArticle;
            this.GroupKey.COO = this.SpecCOO;
            this.GroupKey.Gender = this.SpecGender;
            this.GroupKey.CustomerSize = this.SpecCustomerSize;
            this.GroupKey.DecLabelSize = this.SpecDecLabelSize;
            this.GroupKey.BrandFactoryCode = this.SpecBrandFactoryCode;
            this.GroupKey.Style = this.SpecStyle;
            this.GroupKey.StyleLocation = this.SpecStyleLocation;
            this.GroupKey.Season = this.SpecSeason;
            this.GroupKey.CareCode = this.SpecCareCode;
            this.GroupKey.CustomerPO = this.SpecCustomerPO;
        }
    }

    public class ComparePO3GroupItem
    {
        public string ID { get; set; }
        public string RefNo { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string SizeUnit { get; set; }
        public string ZipperInsert { get; set; }
        public string Article { get; set; }
        public string COO { get; set; }
        public string Gender { get; set; }
        public string CustomerSize { get; set; }
        public string DecLabelSize { get; set; }
        public string BrandFactoryCode { get; set; }
        public string Style { get; set; }
        public string StyleLocation { get; set; }
        public string Season { get; set; }
        public string CareCode { get; set; }
        public string CustomerPO { get; set; }
    }

    /// <summary>
    /// 比對狀態的Enum
    /// </summary>
    public enum ComparePO3StateEnu
    {
        /// <summary>
        /// 比對結果為新增項目
        /// </summary>
        New,

        /// <summary>
        /// 比對為刪除項目
        /// </summary>
        Delete,

        /// <summary>
        /// 比對規格結果一致
        /// </summary>
        Same,

        /// <summary>
        /// 尚未比對
        /// </summary>
        None,
    }

    /// <summary>
    /// 版本的Enum
    /// </summary>
    public enum ComparePO3VersionEnu
    {
        /// <summary>
        /// 新轉出版本
        /// </summary>
        New,

        /// <summary>
        /// 當前版本，舊版本
        /// </summary>
        Old
    }
}
