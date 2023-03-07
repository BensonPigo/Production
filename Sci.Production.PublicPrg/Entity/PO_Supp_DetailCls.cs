using System;
using System.Data;

namespace Sci.Production.Prg.Entity
{
    /// <summary>
    /// PO_Supp_Detail Class
    /// </summary>
    public class PO_Supp_DetailCls : BaseTableEntity
    {
        /// <summary>
        /// PO_Supp_Detail Construct
        /// </summary>
        public PO_Supp_DetailCls()
            : base()
        {

        }

        /// <summary>
        /// PO_Supp_Detail Construct
        /// </summary>
        /// <param name="row"> row of PO_Supp_Detail </param>
        public PO_Supp_DetailCls(DataRow row)
            : base(row)
        {
            mRow = row;
        }

        private DataRow mRow = null;

        public DataRow row
        {
            get { return mRow; }
        }

        private string mID;
        private string mSeq1;
        private string mSeq2;
        private string mRefNo;
        private string mSCIRefNo;
        private string mFabricType;
        private decimal mPrice;
        private decimal? mUsedQty;
        private decimal? mQty;
        private string mPOUnit;
        private bool? mComplete;
        private DateTime? mSystemETD;
        private DateTime? mCFMETD;
        private DateTime? mRevisedETD;
        private DateTime? mFinalETD;
        private DateTime? mEstETA;
        private string mShipModeID;
        private string mSMRLock;
        private DateTime? mSystemLock;
        private DateTime? mPrintDate;
        private string mPINO;
        private DateTime? mPIDate;
        private string mSuppColor;
        private string mRemark;
        private string mSpecial;
        private decimal? mWidth;
        private string mOrderIdList;
        private decimal? mStockQty;
        private decimal? mNetQty;
        private decimal? mLossQty;
        private decimal? mSystemNetQty;
        private bool? mSystemCreate;
        private decimal? mFOC;
        private bool? mJunk;
        private string mColorDetail;
        private DateTime? mShipETA;
        private decimal? mShipQty;
        private decimal? mShortage;
        private decimal? mShipFOC;
        private decimal? mApQty;
        private decimal? mInputQty;
        private decimal? mOutputQty;
        private string mAddName;
        private DateTime? mAddDate;
        private string mEditName;
        private DateTime? mEditDate;
        private string mFabricVer_Old;
        private string mFabricUkey_Old;
        private string mSpec;
        private string mOutputSeq1;
        private string mOutputSeq2;
        private string mFactoryID;
        private string mStockPOID;
        private string mStockSeq1;
        private string mStockSeq2;
        private long? mInventoryUkey;
        private string mColorID_Old;
        private string mRemark_Shell;
        private string mLongerLTReasonID;
        private string mPriceIrregularReasonID;
        private bool? mForFRC;
        private string mReplacementReportID;
        private decimal mFinalNeedQty;
        private string mComments_PR;
        private string mComments_MR;
        private bool mIsForOtherBrand;
        private string mToMtlPOID;
        private string mToMtlSeq1;
        private string mToMtlSeq2;
        private string mStockUnit;
        private decimal? mStockNetQty;
        private decimal? mStockLossQty;
        private decimal? mStockFOC;
        private decimal? mStockUnitQty;
        // BomSpec
        private string mSpecColor;
        private string mSpecSize;
        private string mSpecSizeUnit;
        private string mSpecZipperInsert;
        private string mSpecArticle;
        private string mSpecCOO;
        private string mSpecGender;
        private string mSpecCustomerSize;
        private string mSpecDecLabelSize;
        private string mSpecBrandFactoryCode;
        private string mSpecStyle;
        private string mSpecStyleLocation;
        private string mSpecSeason;
        private string mSpecCareCode;
        private string mSpecCustomerPO;

        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get { return this.mID; }
            set { this.mID = value; }
        }

        /// <summary>
        /// Seq1
        /// </summary>
        public string Seq1
        {
            get { return this.mSeq1; }
            set { this.mSeq1 = value; }
        }

        /// <summary>
        /// Seq2
        /// </summary>
        public string Seq2
        {
            get { return this.mSeq2; }
            set { this.mSeq2 = value; }
        }

        /// <summary>
        /// RefNo
        /// </summary>
        public string RefNo
        {
            get { return this.mRefNo; }
            set { this.mRefNo = value; }
        }

        /// <summary>
        /// SCIRefNo
        /// </summary>
        public string SCIRefNo
        {
            get { return this.mSCIRefNo; }
            set { this.mSCIRefNo = value; }
        }

        /// <summary>
        /// FabricType
        /// </summary>
        public string FabricType
        {
            get { return this.mFabricType; }
            set { this.mFabricType = value; }
        }

        /// <summary>
        /// Price
        /// </summary>
        public decimal Price
        {
            get { return this.mPrice; }
            set { this.mPrice = value; }
        }

        /// <summary>
        /// UsedQty
        /// </summary>
        public decimal? UsedQty
        {
            get { return this.mUsedQty; }
            set { this.mUsedQty = value; }
        }

        /// <summary>
        /// Qty
        /// </summary>
        public decimal? Qty
        {
            get { return this.mQty; }
            set { this.mQty = value; }
        }

        /// <summary>
        /// POUnit
        /// </summary>
        public string POUnit
        {
            get { return this.mPOUnit; }
            set { this.mPOUnit = value; }
        }

        /// <summary>
        /// Complete
        /// </summary>
        public bool? Complete
        {
            get { return this.mComplete; }
            set { this.mComplete = value; }
        }

        /// <summary>
        /// SystemETD
        /// </summary>
        public DateTime? SystemETD
        {
            get { return this.mSystemETD; }
            set { this.mSystemETD = value; }
        }

        /// <summary>
        /// CFMETD
        /// </summary>
        public DateTime? CFMETD
        {
            get { return this.mCFMETD; }
            set { this.mCFMETD = value; }
        }

        /// <summary>
        /// RevisedETD
        /// </summary>
        public DateTime? RevisedETD
        {
            get { return this.mRevisedETD; }
            set { this.mRevisedETD = value; }
        }

        /// <summary>
        /// FinalETD
        /// </summary>
        public DateTime? FinalETD
        {
            get { return this.mFinalETD; }
            set { this.mFinalETD = value; }
        }

        /// <summary>
        /// EstETA
        /// </summary>
        public DateTime? EstETA
        {
            get { return this.mEstETA; }
            set { this.mEstETA = value; }
        }

        /// <summary>
        /// ShipModeID
        /// </summary>
        public string ShipModeID
        {
            get { return this.mShipModeID; }
            set { this.mShipModeID = value; }
        }

        /// <summary>
        /// SystemLock
        /// </summary>
        public DateTime? SystemLock
        {
            get { return this.mSystemLock; }
            set { this.mSystemLock = value; }
        }

        /// <summary>
        /// PrintDate
        /// </summary>
        public DateTime? PrintDate
        {
            get { return this.mPrintDate; }
            set { this.mPrintDate = value; }
        }

        /// <summary>
        /// PINO
        /// </summary>
        public string PINO
        {
            get { return this.mPINO; }
            set { this.mPINO = value; }
        }

        /// <summary>
        /// PIDate
        /// </summary>
        public DateTime? PIDate
        {
            get { return this.mPIDate; }
            set { this.mPIDate = value; }
        }

        /// <summary>
        /// SuppColor
        /// </summary>
        public string SuppColor
        {
            get { return this.mSuppColor; }
            set { this.mSuppColor = value; }
        }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark
        {
            get { return this.mRemark; }
            set { this.mRemark = value; }
        }

        /// <summary>
        /// Special
        /// </summary>
        public string Special
        {
            get { return this.mSpecial; }
            set { this.mSpecial = value; }
        }

        /// <summary>
        /// Width
        /// </summary>
        public decimal? Width
        {
            get { return this.mWidth; }
            set { this.mWidth = value; }
        }

        /// <summary>
        /// OrderIdList
        /// </summary>
        public string OrderIdList
        {
            get { return this.mOrderIdList; }
            set { this.mOrderIdList = value; }
        }

        /// <summary>
        /// StockQty
        /// </summary>
        public decimal? StockQty
        {
            get { return this.mStockQty; }
            set { this.mStockQty = value; }
        }

        /// <summary>
        /// NetQty
        /// </summary>
        public decimal? NetQty
        {
            get { return this.mNetQty; }
            set { this.mNetQty = value; }
        }

        /// <summary>
        /// LossQty
        /// </summary>
        public decimal? LossQty
        {
            get { return this.mLossQty; }
            set { this.mLossQty = value; }
        }

        /// <summary>
        /// SystemNetQty
        /// </summary>
        public decimal? SystemNetQty
        {
            get { return this.mSystemNetQty; }
            set { this.mSystemNetQty = value; }
        }

        /// <summary>
        /// SystemCreate
        /// </summary>
        public bool? SystemCreate
        {
            get { return this.mSystemCreate; }
            set { this.mSystemCreate = value; }
        }

        /// <summary>
        /// FOC
        /// </summary>
        public decimal? FOC
        {
            get { return this.mFOC; }
            set { this.mFOC = value; }
        }

        /// <summary>
        /// Junk
        /// </summary>
        public bool? Junk
        {
            get { return this.mJunk; }
            set { this.mJunk = value; }
        }

        /// <summary>
        /// ColorDetail
        /// </summary>
        public string ColorDetail
        {
            get { return this.mColorDetail; }
            set { this.mColorDetail = value; }
        }

        /// <summary>
        /// ShipETA
        /// </summary>
        public DateTime? ShipETA
        {
            get { return this.mShipETA; }
            set { this.mShipETA = value; }
        }

        /// <summary>
        /// ShipQty
        /// </summary>
        public decimal? ShipQty
        {
            get { return this.mShipQty; }
            set { this.mShipQty = value; }
        }

        /// <summary>
        /// Shortage
        /// </summary>
        public decimal? Shortage
        {
            get { return this.mShortage; }
            set { this.mShortage = value; }
        }

        /// <summary>
        /// ShipFOC
        /// </summary>
        public decimal? ShipFOC
        {
            get { return this.mShipFOC; }
            set { this.mShipFOC = value; }
        }

        /// <summary>
        /// ApQty
        /// </summary>
        public decimal? ApQty
        {
            get { return this.mApQty; }
            set { this.mApQty = value; }
        }

        /// <summary>
        /// InputQty
        /// </summary>
        public decimal? InputQty
        {
            get { return this.mInputQty; }
            set { this.mInputQty = value; }
        }

        /// <summary>
        /// OutputQty
        /// </summary>
        public decimal? OutputQty
        {
            get { return this.mOutputQty; }
            set { this.mOutputQty = value; }
        }

        /// <summary>
        /// AddName
        /// </summary>
        public string AddName
        {
            get { return this.mAddName; }
            set { this.mAddName = value; }
        }

        /// <summary>
        /// AddDate
        /// </summary>
        public DateTime? AddDate
        {
            get { return this.mAddDate; }
            set { this.mAddDate = value; }
        }

        /// <summary>
        /// EditName
        /// </summary>
        public string EditName
        {
            get { return this.mEditName; }
            set { this.mEditName = value; }
        }

        /// <summary>
        /// EditDate
        /// </summary>
        public DateTime? EditDate
        {
            get { return this.mEditDate; }
            set { this.mEditDate = value; }
        }

        /// <summary>
        /// FabricVer_Old
        /// </summary>
        public string FabricVer_Old
        {
            get { return this.mFabricVer_Old; }
            set { this.mFabricVer_Old = value; }
        }

        /// <summary>
        /// FabricUkey_Old
        /// </summary>
        public string FabricUkey_Old
        {
            get { return this.mFabricUkey_Old; }
            set { this.mFabricUkey_Old = value; }
        }

        /// <summary>
        /// Spec
        /// </summary>
        public string Spec
        {
            get { return this.mSpec; }
            set { this.mSpec = value; }
        }

        /// <summary>
        /// OutputSeq1
        /// </summary>
        public string OutputSeq1
        {
            get { return this.mOutputSeq1; }
            set { this.mOutputSeq1 = value; }
        }

        /// <summary>
        /// OutputSeq2
        /// </summary>
        public string OutputSeq2
        {
            get { return this.mOutputSeq2; }
            set { this.mOutputSeq2 = value; }
        }

        /// <summary>
        /// FactoryID
        /// </summary>
        public string FactoryID
        {
            get { return this.mFactoryID; }
            set { this.mFactoryID = value; }
        }

        /// <summary>
        /// StockPOID
        /// </summary>
        public string StockPOID
        {
            get { return this.mStockPOID; }
            set { this.mStockPOID = value; }
        }

        /// <summary>
        /// StockSeq1
        /// </summary>
        public string StockSeq1
        {
            get { return this.mStockSeq1; }
            set { this.mStockSeq1 = value; }
        }

        /// <summary>
        /// StockSeq2
        /// </summary>
        public string StockSeq2
        {
            get { return this.mStockSeq2; }
            set { this.mStockSeq2 = value; }
        }

        /// <summary>
        /// InventoryUkey
        /// </summary>
        public long? InventoryUkey
        {
            get { return this.mInventoryUkey; }
            set { this.mInventoryUkey = value; }
        }

        /// <summary>
        /// ColorID_Old
        /// </summary>
        public string ColorID_Old
        {
            get { return this.mColorID_Old; }
            set { this.mColorID_Old = value; }
        }

        /// <summary>
        /// Remark_Shell
        /// </summary>
        public string Remark_Shell
        {
            get { return this.mRemark_Shell; }
            set { this.mRemark_Shell = value; }
        }

        /// <summary>
        /// LongerLTReasonID
        /// </summary>
        public string LongerLTReasonID
        {
            get { return this.mLongerLTReasonID; }
            set { this.mLongerLTReasonID = value; }
        }

        /// <summary>
        /// PriceIrregularReasonID
        /// </summary>
        public string PriceIrregularReasonID
        {
            get { return this.mPriceIrregularReasonID; }
            set { this.mPriceIrregularReasonID = value; }
        }

        /// <summary>
        /// ForFRC
        /// </summary>
        public bool? ForFRC
        {
            get { return this.mForFRC; }
            set { this.mForFRC = value; }
        }

        /// <summary>
        /// ReplacementReportID
        /// </summary>
        public string ReplacementReportID
        {
            get { return this.mReplacementReportID; }
            set { this.mReplacementReportID = value; }
        }

        /// <summary>
        /// FinalNeedQty
        /// </summary>
        public decimal FinalNeedQty
        {
            get { return this.mFinalNeedQty; }
            set { this.mFinalNeedQty = value; }
        }

        /// <summary>
        /// Comments_PR
        /// </summary>
        public string Comments_PR
        {
            get { return this.mComments_PR; }
            set { this.mComments_PR = value; }
        }

        /// <summary>
        /// Comments_MR
        /// </summary>
        public string Comments_MR
        {
            get { return this.mComments_MR; }
            set { this.mComments_MR = value; }
        }

        /// <summary>
        /// IsForOtherBrand
        /// </summary>
        public bool IsForOtherBrand
        {
            get { return this.mIsForOtherBrand; }
            set { this.mIsForOtherBrand = value; }
        }

        /// <summary>
        /// ToMtlPOID
        /// </summary>
        public string ToMtlPOID
        {
            get { return this.mToMtlPOID; }
            set { this.mToMtlPOID = value; }
        }

        /// <summary>
        /// ToMtlSeq1
        /// </summary>
        public string ToMtlSeq1
        {
            get { return this.mToMtlSeq1; }
            set { this.mToMtlSeq1 = value; }
        }

        /// <summary>
        /// ToMtlSeq2
        /// </summary>
        public string ToMtlSeq2
        {
            get { return this.mToMtlSeq2; }
            set { this.mToMtlSeq2 = value; }
        }

        /// <summary>
        /// BomSpec - Color
        /// </summary>
        public string SpecColor
        {
            get { return this.mSpecColor; }
            set { this.mSpecColor = value; }
        }

        /// <summary>
        /// BomSpec - Size
        /// </summary>
        public string SpecSize
        {
            get { return this.mSpecSize; }
            set { this.mSpecSize = value; }
        }

        /// <summary>
        /// BomSpec - SizeUnit
        /// </summary>
        public string SpecSizeUnit
        {
            get { return this.mSpecSizeUnit; }
            set { this.mSpecSizeUnit = value; }
        }

        /// <summary>
        /// BomSpec - ZipperInsert
        /// </summary>
        public string SpecZipperInsert
        {
            get { return this.mSpecZipperInsert; }
            set { this.mSpecZipperInsert = value; }
        }

        /// <summary>
        /// BomSpec - Article
        /// </summary>
        public string SpecArticle
        {
            get { return this.mSpecArticle; }
            set { this.mSpecArticle = value; }
        }

        /// <summary>
        /// BomSpec - COO
        /// </summary>
        public string SpecCOO
        {
            get { return this.mSpecCOO; }
            set { this.mSpecCOO = value; }
        }

        /// <summary>
        /// BomSpec - Gender
        /// </summary>
        public string SpecGender
        {
            get { return this.mSpecGender; }
            set { this.mSpecGender = value; }
        }

        /// <summary>
        /// BomSpec - CustomerSize
        /// </summary>
        public string SpecCustomerSize
        {
            get { return this.mSpecCustomerSize; }
            set { this.mSpecCustomerSize = value; }
        }

        /// <summary>
        /// BomSpec - DecLabelSize
        /// </summary>
        public string SpecDecLabelSize
        {
            get { return this.mSpecDecLabelSize; }
            set { this.mSpecDecLabelSize = value; }
        }

        /// <summary>
        /// BomSpec - BrandFactoryCode
        /// </summary>
        public string SpecBrandFactoryCode
        {
            get { return this.mSpecBrandFactoryCode; }
            set { this.mSpecBrandFactoryCode = value; }
        }

        /// <summary>
        /// BomSpec - Style
        /// </summary>
        public string SpecStyle
        {
            get { return this.mSpecStyle; }
            set { this.mSpecStyle = value; }
        }

        /// <summary>
        /// BomSpec - StyleLocation
        /// </summary>
        public string SpecStyleLocation
        {
            get { return this.mSpecStyleLocation; }
            set { this.mSpecStyleLocation = value; }
        }

        /// <summary>
        /// BomSpec - Season
        /// </summary>
        public string SpecSeason
        {
            get { return this.mSpecSeason; }
            set { this.mSpecSeason = value; }
        }

        /// <summary>
        /// BomSpec - CareCode
        /// </summary>
        public string SpecCareCode
        {
            get { return this.mSpecCareCode; }
            set { this.mSpecCareCode = value; }
        }

        /// <summary>
        /// BomSpec - CustomerPO
        /// </summary>
        public string SpecCustomerPO
        {
            get { return this.mSpecCustomerPO; }
            set { this.mSpecCustomerPO = value; }
        }

        /// <summary>
        /// StockUnit
        /// </summary>
        public string StockUnit
        {
            get { return this.mStockUnit; }
            set { this.mStockUnit = value; }
        }

        /// <summary>
        /// StockNetQty
        /// </summary>
        public decimal? StockNetQty
        {
            get { return this.mStockNetQty; }
            set { this.mStockNetQty = value; }
        }

        /// <summary>
        /// StockLossQty
        /// </summary>
        public decimal? StockLossQty
        {
            get { return this.mStockLossQty; }
            set { this.mStockLossQty = value; }
        }

        /// <summary>
        /// StockFOC
        /// </summary>
        public decimal? StockFOC
        {
            get { return this.mStockFOC; }
            set { this.mStockFOC = value; }
        }

        /// <summary>
        /// StockQty
        /// </summary>
        public decimal? StockUnitQty
        {
            get { return this.mStockUnitQty; }
            set { this.mStockUnitQty = value; }
        }
    }
}
