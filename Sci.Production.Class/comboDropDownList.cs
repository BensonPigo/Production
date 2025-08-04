using Sci.Data;
using System.ComponentModel;
using System.Data;
using Ict;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// ComboDropDownList
    /// </summary>
    public partial class ComboDropDownList : Win.UI.ComboBox
    {
        /// <inheritdoc />
        public enum ComboDropDownList_Type
        {
            /// <inheritdoc />
            _None,

            /// <inheritdoc />
            AllowanceType,

            /// <inheritdoc />
            ArtworkCost,

            /// <inheritdoc />
            Brand_ElasticCircle,

            /// <inheritdoc />
            Brand_WKGroup,

            /// <inheritdoc />
            BuyMonth,

            /// <inheritdoc />
            Carrier_Detail_From,

            /// <inheritdoc />
            Carrier_Detail_Payer,

            /// <inheritdoc />
            CheckType,

            /// <inheritdoc />
            Classify,

            /// <inheritdoc />
            CludeType,

            /// <inheritdoc />
            ContainerCY,

            /// <inheritdoc />
            Continent,

            /// <inheritdoc />
            CutCombo,

            /// <inheritdoc />
            FabricItemType,

            /// <inheritdoc />
            FabricKind,

            /// <inheritdoc />
            FabricLTTypeA,

            /// <inheritdoc />
            FabricLTTypeF,

            /// <inheritdoc />
            FabricType,

            /// <inheritdoc />
            Factory_Type,

            /// <inheritdoc />
            Factory_Zone,

            /// <inheritdoc />
            Gender,

            /// <inheritdoc />
            GMCategory,

            /// <inheritdoc />
            ICRStatus,

            /// <inheritdoc />
            InvSettingType,

            /// <inheritdoc />
            InvtransType,

            /// <inheritdoc />
            KeyWrodcommonType,

            /// <inheritdoc />
            Location,

            /// <inheritdoc />
            LossType,

            /// <inheritdoc />
            LossUnit,

            /// <inheritdoc />
            MatchFabric,

            /// <inheritdoc />
            MaterialRule,

            /// <inheritdoc />
            MaterialRespons,

            /// <inheritdoc />
            MIADIDAS,

            /// <inheritdoc />
            MoldType,

            /// <inheritdoc />
            MtlCategory,

            /// <inheritdoc />
            OneTwoWay,

            /// <inheritdoc />
            ProdKitsDoc,

            /// <inheritdoc />
            ProdKitsHandle,

            /// <inheritdoc />
            ProductionUnit,

            /// <inheritdoc />
            PTAB,

            /// <inheritdoc />
            Responsible,

            /// <inheritdoc />
            SciDepartment,

            /// <inheritdoc />
            ShipHandleCheckMetho,

            /// <inheritdoc />
            ShipHandleType,

            /// <inheritdoc />
            ShipPlan_HC_From,

            /// <inheritdoc />
            ShipPlan_HC_Payer,

            /// <inheritdoc />
            ShipPlan_HC_PLACE,

            /// <inheritdoc />
            ShipPlan_HC_Status,

            /// <inheritdoc />
            ShipPlan_HC_To,

            /// <inheritdoc />
            ShipPlan_SS_From,

            /// <inheritdoc />
            ShipPlan_SS_To,

            /// <inheritdoc />
            ShipPlan_SS_Payer,

            /// <inheritdoc />
            StyleLining,

            /// <inheritdoc />
            StyleNEWCO,

            /// <inheritdoc />
            Surcharge,

            /// <inheritdoc />
            SystemType,

            /// <inheritdoc />
            Type,

            /// <inheritdoc />
            Target,

            /// <inheritdoc />
            WK_CombineType,

            /// <inheritdoc />
            WK_FormStatus,

            /// <inheritdoc />
            WKImpPackingType,

            /// <inheritdoc />
            Wk_InvoiceOf,

            /// <inheritdoc />
            WK_Payer,

            /// <inheritdoc />
            WK_TradeTerm,

            /// <inheritdoc />
            LLL_RemarkCalType,
        }

        private ComboDropDownList_Type _type;

        /// <inheritdoc />
        public ComboDropDownList_Type _Type
        {
            get
            {
                return this._type;
            }

            set
            {
                this._type = value;
                if (value != ComboDropDownList_Type._None)
                {
                    this.Type = value.ToString();
                }
            }
        }

        private bool addEmpty;

        /// <inheritdoc />
        [Category("Custom Properties")]
        [Description("是否插入空白選項")]
        public bool AddEmpty
        {
            get { return this.addEmpty; }
            set { this.addEmpty = value; }
        }

        private string type;
        private bool addAllItem = false;

        /// <summary>
        /// 是否有All的選項
        /// </summary>
        public bool AddAllItem
        {
            get
            {
                return this.addAllItem;
            }

            set
            {
                this.addAllItem = value;
                this.SetSource();
            }
        }

        /// <summary>
        /// Type
        /// </summary>
        [Category("Custom Properties")]
        public string Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                this.SetSource();
            }
        }

        public void SetSource(string conditions = "")
        {
            if (!Env.DesignTime)
            {
                string whereCondition = MyUtility.Check.Empty(conditions) ? string.Empty : $" and (Conditions = '' or Conditions = '{conditions}')";
                string unionAllItem = this.addAllItem ? "select [ID] = 'ALL', [Name] = 'ALL' , [Seq] = 0 union all" : string.Empty;
                string selectCommand = $@"
IF OBJECT_ID('View_DropDownList', 'V') IS NOT NULL
BEGIN
    -- 視圖存在的處理邏輯
    select *
    from (
        {unionAllItem}
        select  ID
                , Name = rtrim(Name)
                , Seq
        from View_DropDownList WITH (NOLOCK) 
        where Type = '{this.Type}' {whereCondition}
    ) a
order by Seq
END
ELSE
BEGIN
    select *
    from (
        {unionAllItem}
        select  ID
                , Name = rtrim(Name)
                , Seq
        from DropDownList WITH (NOLOCK) 
        where Type = '{this.Type}' {whereCondition}
    ) a
order by Seq
END



";

                DualResult returnResult;
                DataTable dropDownListTable = new DataTable();
                if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
                {
                    this.DataSource = dropDownListTable;
                    this.DisplayMember = "Name";
                    this.ValueMember = "ID";
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboDropDownList"/> class.
        /// </summary>
        public ComboDropDownList()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboDropDownList"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public ComboDropDownList(System.ComponentModel.IContainer container)
        {
            container.Add(this);

            this.InitializeComponent();
        }
    }
}
