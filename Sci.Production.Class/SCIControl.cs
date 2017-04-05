using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci;
using Sci.Utility;
using Sci.Win;
using Sci.Win.UI;
using Ict;
using Ict.Win;
using Ict.Win.UI;
using System.Data.SqlClient;
using Sci.Data;
using Sci.Production.Class.Commons;
using System.Drawing;
using System.Collections;
//using System.Web.UI.Design;

//請有興趣使用但是目前沒有那個領域的控制項，而想加新東西的人；
//或是目前有那個領域東西，但是一些通用行為不支援的情況
//找Evaon，不要自己加，感謝
namespace Sci.Production.Class
{
    /// <summary>
    /// 用來給IdRestricter使用的介面
    /// </summary>
    public interface IIdBindableControl : IEquatable<object>
    {
        /// <summary>
        /// 可以被設定資料繫結的節點
        /// </summary>
        [Bindable(true)]
        string IDBinding { get; set; }
    }

    /// <summary>
    /// <para>用來規範有Lock的資料表的Pickup行為，包含開窗是否包含Locked，Validating時候是否允許Locked</para>
    /// <para>(Style)</para>
    /// </summary>
    public interface ILockEffectiveSwitch
    {
        /// <summary>
        /// 是否要於開窗中，顯示Lock的資料
        /// </summary>
        bool? DialogIncludeLocked { get; set; }
        /// <summary>
        /// 是否要於Validating中，允許Lock的資料
        /// </summary>
        bool? ValidatingIncludeLocked { get; set; }
    }

    /// <summary>
    /// <para>用來規範有Junk的資料表的Pickup行為，包含開窗是否包含Junked，Validating時候是否允許Junked</para>
    /// <para>(Style)</para>
    /// </summary>
    public interface IJunkEffectiveSwitch
    {
        /// <summary>
        /// 是否要於開窗中，顯示Junk的資料
        /// </summary>
        bool? DialogIncludeJunked { get; set; }
        /// <summary>
        /// 是否要於Validating中，允許Junk的資料
        /// </summary>
        bool? ValidatingIncludeJunked { get; set; }
    }

    #region Setting

    /// <summary>
    /// 開窗的欄位設定
    /// </summary>
    public class PopupColumnSetting
    {
        /// <summary>
        /// 是否為主鍵
        /// </summary>
        public bool IsID { get; set; }
        /// <summary>
        /// 開窗的欄位標題列文字
        /// </summary>
        public string HeaderText { get; set; }
        /// <summary>
        /// 開窗的資料欄位對應
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 開窗的欄位寬度(5大約可容納5個字元)
        /// </summary>
        public int ColumnWidth { get; set; }
        /// <summary>
        /// 要顯示小數多少位
        /// </summary>
        public int? Decimals { get; set; }
        /// <summary>
        /// 開窗的欄位設定
        /// </summary>
        /// <param name="columnName">開窗的資料欄位對應</param>
        /// <param name="headerText">開窗的欄位標題列文字</param>
        /// <param name="columnWidth">開窗的欄位寬度(5大約可容納5個字元)</param>
        /// <param name="isPK">是否為主鍵</param>
        /// <param name="decimals"></param>
        public PopupColumnSetting(string columnName, string headerText = null, int columnWidth = 30, bool isPK = false, int? decimals = null)
        {
            this.IsID = isPK;
            this.ColumnName = columnName;
            this.HeaderText = headerText == null ? columnName : headerText;
            this.ColumnWidth = columnWidth;
            this.Decimals = decimals;
        }
    }

    /// <summary>
    /// 開窗的欄位設定，包含單一欄位設定陣列，還有PK是哪一個
    /// </summary>
    public class PopupColumnSettings : List<PopupColumnSetting>
    {
        /// <summary>
        /// 如果沒有要透過KeyIn方式輸入資料再透過Validating驗證輸入的話，只靠開窗的欄位，可以用UKey作為代替主鍵，預設false，若設定為true就會自動於開窗完成後取用UKey欄位存在UKeyBinding屬性中
        /// </summary>
        public bool UsingUKeyForPopup { get; set; }

        private PopupColumnSetting _ID = null;
        /// <summary>
        /// 第一個(或唯一的)主鍵的欄位名稱
        /// </summary>
        public PopupColumnSetting ID
        {
            get
            {
                if (_ID == null)
                    _ID = this.FirstOrDefault(setting => setting.IsID);
                return _ID;
            }
        }

        private List<PopupColumnSetting> _PKCollection = null;
        /// <summary>
        /// 全部的(或唯一的)主鍵欄位名稱
        /// </summary>
        public List<PopupColumnSetting> PKCollection
        {
            get
            {
                if (_PKCollection == null)
                    _PKCollection = this.Where(setting => setting.IsID).ToList();
                return _PKCollection;
            }
        }

        /// <summary>
        /// 主鍵(不確定為什麼會假設一律都單一主鍵)
        /// </summary>
        public string PrimaryKey
        {
            get
            {
                return this.FirstOrDefault(setting => setting.IsID).ColumnName;
            }
            set
            {
                var pkSetting = this.FirstOrDefault(setting => setting.ColumnName == value);
                if (pkSetting != null)
                {
                    this.Where(setting => setting.ColumnName == value).ToList().ForEach(setting => setting.IsID = false);
                    pkSetting.IsID = true;
                }
                throw new IndexOutOfRangeException("沒有這個欄位");
            }
        }

        /// <summary>
        /// 是否要將結果進行排序
        /// </summary>
        public string SelectDialogOrderBy { get; set; }

        /// <summary>
        /// 開窗的欄位設定，包含單一欄位設定陣列，還有PK是哪一個
        /// </summary>
        public PopupColumnSettings()
        {
            this.UsingUKeyForPopup = false;
        }

        /// <summary>
        /// 開窗的欄位設定，包含單一欄位設定陣列，還有PK是哪一個
        /// </summary>
        /// <param name="source"></param>
        public PopupColumnSettings(IList<PopupColumnSetting> source)
        {
            base.AddRange(source);
        }

        /// <summary>
        /// 開窗的欄位設定，包含單一欄位設定陣列，還有PK是哪一個
        /// </summary>
        /// <param name="source"></param>
        public PopupColumnSettings(params PopupColumnSetting[] source)
        {
            base.AddRange(source);
        }

        /// <summary>
        /// 用自己的設定取回第一(或唯一)個主鍵鍵值
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public string GetID(DataRow row)
        {
            return row.Field<string>(this.ID.ColumnName);
        }

        /// <summary>
        /// 用自己的設定取回全部(或唯一)的主鍵鍵值
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public object[] GetPKValueCollection(DataRow row)
        {
            return this.PKCollection.Select(setting => row[setting.ColumnName]).ToArray();
        }
    }

    /// <summary>
    /// <para>會在Popup前，將資料做加工</para>
    /// <para>例如Select("xxxx").CopyToDataTable()形成一個僅有Local端需要的資料集合來顯示</para>
    /// <para>這樣可以讓CacheCore有比較集中的儲存單元，依照使用端的各自需求來做子集合分組，不用每一組都各自獨立一個CacheCore</para>
    /// <para>進而促使靜態獨體的CacheCore能夠減少種類與增加可用情境</para>
    /// </summary>
    public class CacheCoreDataFilterSetting : IJunkEffectiveSwitch, ILockEffectiveSwitch
    {
        /// <summary>
        /// 用於資料篩選的屬性集合，內容由繼承端決定
        /// </summary>
        public List<CacheCoreDataFilterBase> Items { get; set; }

        /// <summary>
        /// 要如何篩選是否需要某一個資料列
        /// </summary>
        public Func<DataRow, bool> Predictor { get; set; }

        /// <summary>
        /// 利用DialogIncludeJunked + DialogIncludeLocked，疊上Predictor，產生一個新的DataTable副本，用於開窗
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <returns></returns>
        internal DataTable GetDialogDataSource(DataTable sourceTable)
        {
            this.EnsureDynamicValue();
            Func<DataRow, bool> junkPredictor = (r) => true;
            Func<DataRow, bool> lockPredictor = (r) => true;
            Func<DataRow, bool> localPredictor = (r) => true;

            //Include Junk 相關
            if (this.DialogIncludeJunked.HasValue)
            {
                if (sourceTable.Columns.Contains("Junk") == false ||
                    sourceTable.Columns["Junk"].DataType != typeof(bool))
                    throw new ArgumentException("with [DialogIncludeJunked] property set, Cached Datasource must contains 'Junk'[bool] column");
                if (this.DialogIncludeJunked.Value == false)
                    junkPredictor = (row) => row.Field<bool?>("Junk").GetValueOrDefault(false) == false;
            }

            //Include Lock 相關
            if (this.DialogIncludeLocked.HasValue)
            {
                if (sourceTable.Columns.Contains("Lock") == false ||
                    sourceTable.Columns["Lock"].DataType != typeof(bool))
                    throw new ArgumentException("with [DialogIncludeLock] property set, Cached Datasource must contains 'Lock'[bool] column");
                if (this.DialogIncludeLocked.Value == false)
                    lockPredictor = (row) => row.Field<bool?>("Lock").GetValueOrDefault(false) == false;
            }


            //Local predictor 相關
            if (this.Predictor != null)
                localPredictor = this.Predictor;

            this.Items.ToList().ForEach(item => item.EnsureDynamicValue());

            //return
            return sourceTable.AsEnumerable()
                .Where(junkPredictor)
                .Where(lockPredictor)
                .Where(row => this.Items.All(item =>
                {
                    if (item.IsPrimaryKey || item.Value != null)
                        return item.Comparer.Compare(row[item.MappingFieldName], item.Value) == 0;
                    else
                        return true;
                }))
                .Where(localPredictor)
                .TryCopyToDataTable(sourceTable);
        }

        /// <summary>
        /// 利用ValidatingIncludeJunked + ValidatingIncludeLocked，疊上Predictor，驗證資料是否存在
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="pks"></param>
        /// <returns></returns>
        internal DataRow FindRowsWhenValidating(DataTable sourceTable, object[] pks)
        {
            this.EnsureDynamicValue();
            Func<DataRow, bool> junkPredictor = (r) => true;
            Func<DataRow, bool> lockPredictor = (r) => true;
            Func<DataRow, bool> localPredictor = (r) => true;

            //Include Junk 相關
            if (this.ValidatingIncludeJunked.HasValue)
            {
                if (sourceTable.Columns.Contains("Junk") == false ||
                    sourceTable.Columns["Junk"].DataType != typeof(bool))
                    throw new ArgumentException("with [DialogIncludeJunked] property set, Cached Datasource must contains 'Junk'[bool] column");
                if (this.ValidatingIncludeJunked.Value == false)
                    junkPredictor = (row) => row.Field<bool?>("Junk").GetValueOrDefault(false) == false;
            }

            //Include Lock 相關
            if (this.ValidatingIncludeLocked.HasValue)
            {
                if (sourceTable.Columns.Contains("Lock") == false ||
                    sourceTable.Columns["Lock"].DataType != typeof(bool))
                    throw new ArgumentException("with [DialogIncludeLock] property set, Cached Datasource must contains 'Lock'[bool] column");
                if (this.ValidatingIncludeLocked.Value == false)
                    lockPredictor = (row) => row.Field<bool?>("Lock").GetValueOrDefault(false) == false;
            }

            //Local predictor 相關
            if (this.Predictor != null)
                localPredictor = this.Predictor;

            //return
            var findResult = sourceTable.Rows.Find(pks);
            if (findResult == null)
                return null;

            if (junkPredictor(findResult) == false ||
                lockPredictor(findResult) == false ||
                localPredictor(findResult) == false ||
                this.Items.All(item =>
                {
                    item.EnsureDynamicValue();
                    if (item.IsPrimaryKey || item.Value != null)
                        return item.Comparer.Compare(findResult[item.MappingFieldName], item.Value) == 0;
                    else
                        return true;
                }) == false)
                return null;
            else
                return findResult;
        }

        internal static CacheCoreDataFilterSetting Dummy = new CacheCoreDataFilterSetting();

        /// <summary>
        /// 是否開窗選單中要包含Junk的資料
        /// </summary>
        public bool? DialogIncludeJunked { get; set; }

        /// <summary>
        /// 是否於直接帶值的驗證過程中要允許Junk的資料
        /// </summary>
        public bool? ValidatingIncludeJunked { get; set; }

        /// <summary>
        /// 是否開窗選單中要包含Lock的資料
        /// </summary>
        public bool? DialogIncludeLocked { get; set; }

        /// <summary>
        /// 是否於直接帶值的驗證過程中要允許Lock的資料
        /// </summary>
        public bool? ValidatingIncludeLocked { get; set; }

        /// <summary>
        /// 控制CacheCore裡面的資料，那些可以被顯示出來(或使用於資料驗證)
        /// </summary>
        public CacheCoreDataFilterSetting()
        {
            this.Items = new List<CacheCoreDataFilterBase>();
        }

        /// <summary>
        /// 用Control的文字值，轉換為用於DataTable.Rows.Find方法使用來定位出唯一的一筆紀錄的object陣列
        /// </summary>
        /// <param name="singlePK"></param>
        /// <returns></returns>
        internal protected object[] GetFullPK(object singlePK)
        {
            this.EnsureDynamicValue();
            if (this.TextToPKArray == null)
                return new object[] { singlePK };
            else
                return this.TextToPKArray(singlePK);
        }

        /// <summary>
        /// 繼承端可以決定要不要自訂用於找DataRow的PK值，會拿去給DataTable.Rows.Find使用，預設是null，代表單一文字當作PK去找，如果使用端採用複合主鍵，一定要實做這個方法
        /// </summary>
        public Func<object, object[]> TextToPKArray { get; set; }

        /// <summary>
        /// 命令所有Items呼叫EnsureDynamicValue()
        /// </summary>
        private void EnsureDynamicValue()
        {
            this.Items.ForEach(item => item.EnsureDynamicValue());
        }
    }

    #endregion

    #region Event Handler

    /// <summary>
    /// 基本事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MultiSelectDialogEventHandler(MultiSelectDialog sender, EventArgs e);

    /// <summary>
    /// 泛型事件
    /// </summary>
    /// <typeparam name="TEventArg"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MultiSelectDialogEventHandler<TEventArg>(MultiSelectDialog sender, TEventArg e) where TEventArg : EventArgs;

    #endregion

    #region Dialog

    /// <summary>
    /// 繼承SelectItem2，提供多選功能，以及開放Grid的存取接口
    /// </summary>
    public class MultiSelectDialog : Sci.Win.Tools.SelectItem2
    {
        /// <summary>
        /// Grid
        /// </summary>
        public Grid Grid
        {
            get
            {
                return base.grid;
            }
        }
        /// <summary>
        /// 當欄位雙點的時候觸發
        /// </summary>
        public event MultiSelectDialogEventHandler<DataGridViewCellEventArgs> CellDoubleClick;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordsource"></param>
        /// <param name="headercaptions"></param>
        /// <param name="defaults"></param>
        public MultiSelectDialog(string recordsource, string headercaptions, string defaults)
            : base(recordsource, headercaptions, defaults)
        {
            base.grid.CellDoubleClick += grid_CellDoubleClick;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordsource"></param>
        /// <param name="headercaptions"></param>
        /// <param name="columnwidths"></param>
        /// <param name="defaults"></param>
        /// <param name="columndecimals"></param>
        public MultiSelectDialog(string recordsource, string headercaptions, string columnwidths, string defaults, string columndecimals = null)
            : base(recordsource, headercaptions, columnwidths, defaults, columndecimals)
        {
            base.grid.CellDoubleClick += grid_CellDoubleClick;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="columns"></param>
        /// <param name="headercaptions"></param>
        /// <param name="columnwidths"></param>
        /// <param name="defaults"></param>
        /// <param name="columndecimals"></param>
        public MultiSelectDialog(DataTable datas, string columns, string headercaptions, string columnwidths, string defaults, string columndecimals = null)
            : base(datas, columns, headercaptions, columnwidths, defaults, columndecimals)
        {
            base.grid.CellDoubleClick += grid_CellDoubleClick;
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.CellDoubleClick != null)
                this.CellDoubleClick(this, e);
        }
    }

    #endregion

    #region Base

    /// <summary>
    /// BasePickIdBox
    /// </summary>
    public abstract class BasePickIdBox<TKeyValue, TLookupValue> : Sci.Win.UI.TextBox, IIdBindableControl
    {
        #region desinger

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Size = new System.Drawing.Size(78, 22);
            this.ResumeLayout(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 用於快取資料的提供與重載，也負責做開窗動作，因為是PickBox與GriCell的共同行為
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected BaseDataCacheCore<TKeyValue, TLookupValue> DefaultCacheCore { get; set; }

        /// <summary>
        /// 自訂的一組快取設定，包含MyCacheLoader, MyPopupColumnSettings, MyLookup，如果其中有未指定的，會轉呼叫DeafultCacheCore的方法
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomizeCacheCore<TKeyValue, TLookupValue> CustomizeCacheCore { get; set; }

        /// <summary>
        /// 自動選取CacheCore，有指派CustomizeCacheCore優先
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal BaseDataCacheCore<TKeyValue, TLookupValue> AutoCacheCore
        {
            get
            {
                if (Env.DesignTime)
                    return null;
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyCacheLoader == null)
                        this.CustomizeCacheCore.MyCacheLoader = this.DefaultCacheCore.LoadCache;

                    if (this.CustomizeCacheCore.MyLookup == null)
                        this.CustomizeCacheCore.MyLookup = new Func<DataRow, TLookupValue>(row => this.DefaultCacheCore.Lookup(row));

                    return this.CustomizeCacheCore;
                }
                else
                    return this.DefaultCacheCore;
            }
        }

        /// <summary>
        /// 開窗的設定值，雖然開窗動作是PickBox與GriCell的共同行為，但是所開出來的窗體包含欄位，卻可以有所不同
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PopupColumnSettings PopupSettings { get; set; }

        /// <summary>
        /// 開窗資料如何篩選
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual CacheCoreDataFilterSetting DataFilterSetting { get; set; }

        /// <summary>
        /// 自動選取PopupSettings，有指派CustomizeCacheCore優先
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal PopupColumnSettings AutoPopupSettings
        {
            get
            {
                if (Env.DesignTime)
                    return null;
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyPopupColumnSettings == null)
                        this.CustomizeCacheCore.MyPopupColumnSettings = this.PopupSettings;
                    return this.CustomizeCacheCore.MyPopupColumnSettings;

                }
                else
                    return this.PopupSettings;
            }
        }

        /// <summary>
        /// 是否要將開窗的表格做預先排序
        /// </summary>
        [Category("SCIC.BasePickIdBox")]
        [Description("如果需要將開窗結果做預排序，會用於DataView的Sort屬性，可接受字樣舉例：\"Seq\", \"Seq Asc\", \"Seq Desc\", \"ID, Seq asc\"")]
        public string SelectDialogOrderBy
        {
            get
            {
                if (Env.DesignTime)
                    return null;
                return this.AutoPopupSettings.SelectDialogOrderBy;
            }
            set
            {
                if (Env.DesignTime)
                    return;
                this.AutoPopupSettings.SelectDialogOrderBy = value;
            }
        }

        /// <summary>
        /// 真正用於開窗與回查的主鍵，透過複寫Lookup可以讓他跟textbox1.Text不同，但是只有單向 (參考UKey屬性說明)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ID { get; set; }

        /// <summary>
        /// <para>提供外部做資料繫結，連動base.Text。</para>
        /// </summary>
        [Bindable(true)]
        [Category("SCIC.BasePickIdBox")]
        [Description("提供資料繫結節點，連動base.Text")]
        public string IDBinding
        {
            set
            {
                this.ID = value;
                base.Text = value;
                if (Env.DesignTime)
                    return;

                this.ReValidate();
            }
            get { return base.Text; }
        }

        private long? _UKeyBinding = null;
        /// <summary>
        /// <para>提供外部做資料繫結</para>
        /// <para>set的時候會連帶呼叫 this.Lookup來設定this.textBox1.Text 和 this.displayBox1.Text</para>
        /// </summary>
        [Bindable(true)]
        [Browsable(false)]
        [Category("SCIC-BasePickLookupBox")]
        [Description("提供外部做資料繫結，set的時候會連帶呼叫 this.Lookup來設定this.textBox1.Text 和 this.displayBox1.Text")]
        public long? UKeyBinding
        {
            set
            {
                if (Env.DesignTime == true)
                    return;
                if (this._UKeyBinding != value)
                {
                    this._UKeyBinding = value;
                    var row = this.AutoCacheCore.CachedDataSource.Select("UKey = " + value).FirstOrDefault();
                    this.Text = this.ID = (row == null) ?
                        null :
                        Convert.ToString(row[this.AutoPopupSettings.ID.ColumnName]);
                }
            }
            get { return this._UKeyBinding; }
        }

        /// <summary>
        /// <para>提供外部做資料繫結，連動base.Text。</para>
        /// </summary>
        [Bindable(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("SCIC.BasePickIdBox")]
        [Description("提供資料繫結節點，連動IdTextBox.BackColor和DisplayBox.BackColor")]
        public string BackColorName
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    if (this.ReadOnly)
                        this.BackColor = VFPColor.Blue_183_227_255;
                    else
                        this.BackColor = Color.Empty;
                }
                else
                {
                    var oldColor = this.BackColor;
                    try
                    {
                        this.BackColor = Color.FromName(value);
                    }
                    catch (Exception)
                    {
                        this.BackColor = oldColor;
                    }
                }
            }
            get
            {
                return this.BackColor.Name;
            }
        }

        /// <summary>
        /// 最後一次開窗選到的資料列
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataRow LastSelectedRow { get; set; }

        /// <summary>
        /// 讓內部在Show WarnningBox的時候可以有點區別性的文字，讓UI端知道現在這個訊息視窗大概是來自哪邊
        /// </summary>
        [Bindable(true)]
        [Category("SCIC.BasePickIdBox")]
        public string PickName { get; set; }

        /// <summary>
        /// 讓內部在Show WarnningBox的時候可以有點區別性的文字，讓UI端知道現在這個訊息視窗大概是來自哪邊
        /// </summary>
        [Category("SCIC-BasePickIdBox")]
        [Description("轉呼叫PopUpMode屬性，決定何種情況下會進行開窗")]
        public new TextBoxPopUpMode PopUpMode
        {
            get
            {
                return base.PopUpMode;
            }
            set
            {
                base.PopUpMode = value;
            }
        }

        /// <summary>
        /// 當開窗或驗證完成時候，會將Lookup到的結果放入此控制項內
        /// </summary>
        [Category("SCIC.BasePickIdBox")]
        [Description("當開窗或驗證完成時候，會將Lookup到的結果放入此控制項內")]
        public ITextBox DisplayBox { get; set; }

        #endregion

        /// <summary>
        /// BasePickIdBox
        /// </summary>
        public BasePickIdBox()
        {
            this.InitializeComponent();
            this.PickName = "Pickup ID";
            this.DataFilterSetting = new CacheCoreDataFilterSetting();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            if (Env.DesignTime)
                return;

            if (e.IsHandled == true)
                return;

            var selectedRow = this.AutoCacheCore.ShowPopupDialog(this.AutoPopupSettings, this.ID, this.DataFilterSetting);

            if (selectedRow == null)
                return;

            this.LastSelectedRow = selectedRow;

            //命令popup setting用自己的設定值取回主鍵值，放給IDBinding屬性
            //接著觸動IDBinding_set裡面的連動程序，進而更新Display欄位
            //但如果是有使用UKey作為代替主鍵，那就改用UKeyBinding
            if (this.AutoPopupSettings.UsingUKeyForPopup)
                this.UKeyBinding = this.LastSelectedRow.Field<long?>("UKey");
            else
                this.IDBinding = (string)this.AutoPopupSettings.GetID(selectedRow);

            if (this.DisplayBox != null)
                this.DisplayBox.Text = Convert.ToString(this.AutoCacheCore.Lookup(selectedRow));

            this.TryBindingWrite();

            this.ValidateControl();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            if (Env.DesignTime)
                return;

            if (this.OldValue == this.Text)
            {
                base.OnValidating(e);
                return;
            }
            string textValue = this.Text;

            if (this.AutoCacheCore.CachedDataSource.PrimaryKey.Length == 1 &&
                this.AutoCacheCore.CachedDataSource.PrimaryKey[0].ColumnName.CompareTo("UKey", true) == 0)
            {
                //把UKey當作PK的資料表，無法透過所輸入的文字方塊做Validate，所以這邊直接跳過
                base.OnValidating(e);
                return;
            }

            this.LastSelectedRow = null;

            if (string.IsNullOrEmpty(textValue))
                return;

            var fullPK = this.DataFilterSetting.GetFullPK(textValue);
            var findResult = this.AutoCacheCore.FindRowByValidatingSetting(this.DataFilterSetting, fullPK);
            if (findResult == null)
            {
                MyUtility.Msg.ErrorBox(string.Format("< {0}: {1} > not found!!!", this.PickName, textValue), "Error");
                e.Cancel = true;
                return;
            }

            this.LastSelectedRow = findResult;
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());

            if (this.DisplayBox != null)
            {
                this.DisplayBox.Text = Convert.ToString(this.AutoCacheCore.Lookup(findResult));
            }
            base.OnValidating(e);
        }

        #region DataBinding warpper

        private IEnumerable<Binding> GetAllDataBinding()
        {
            return this.DataBindings.Cast<Binding>();
        }

        /// <summary>
        /// 嘗試將全部的DataBinding做ReadValue動作
        /// </summary>
        public void TryBindingRead()
        {
            this.GetAllDataBinding().ToList().ForEach(bd => bd.ReadValue());
        }

        /// <summary>
        /// 嘗試將全部的DataBinding做WriteValue動作
        /// </summary>
        public void TryBindingWrite()
        {
            this.GetAllDataBinding().ToList().ForEach(bd => bd.WriteValue());
        }

        #endregion

        /// <summary>
        /// 當繼承端有一些前置條件改變時，可呼叫此方法，來重新檢查輸入的值是否存在於新條件下的CacheCore內，若不存在，會清空
        /// </summary>
        protected void ReValidate()
        {
            if (this.AutoCacheCore.CachedDataSource.PrimaryKey.Length == 1 &&
                this.AutoCacheCore.CachedDataSource.PrimaryKey[0].ColumnName.CompareTo("UKey", true) == 0)
            {
                //把UKey當作PK的資料表，無法透過所輸入的文字方塊做Validate，所以這邊直接跳過
                return;
            }
            if (string.IsNullOrWhiteSpace(base.Text) ||
                (this.AutoCacheCore.CachedDataSource.PrimaryKey.Length != 1 &&
                this.DataFilterSetting.TextToPKArray == null)
                )
            {
                base.Text = null;
                if (this.DisplayBox != null)
                    this.DisplayBox.Text = null;
                return;
            }

            var pks = this.DataFilterSetting.GetFullPK(base.Text);
            var refindResult = this.AutoCacheCore.FindRowByValidatingSetting(this.DataFilterSetting, pks);
            if (refindResult == null)
            {
                base.Text = null;
                if (this.DisplayBox != null)
                    this.DisplayBox.Text = null;
            }
            else if (this.DisplayBox != null)
            {
                this.DisplayBox.Text = Convert.ToString(this.AutoCacheCore.Lookup(refindResult));
            }
        }
    }

    /// <summary>
    /// PickBox基底
    /// </summary>
    public abstract class BasePickLookupBox<TKeyValue, TLookupValue> : Sci.Win.UI._UserControl, IIdBindableControl
    {
        #region desinger

        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// ID TextBox
        /// </summary>
        public Win.UI.TextBox textBox1;
        /// <summary>
        /// Display TextBox
        /// </summary>
        public Win.UI.DisplayBox displayBox1;

        private void InitializeComponent()
        {
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(30, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textBox1_PopUp);
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // displayBox1
            // 
            this.displayBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.displayBox1.Location = new System.Drawing.Point(30, 0);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(227, 23);
            this.displayBox1.TabIndex = 1;
            // 
            // TxtCountry
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.displayBox1);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "PickupTextBox";
            this.Size = new System.Drawing.Size(257, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Properties

        /// <summary>
        /// 用於快取資料的提供與重載，也負責做開窗動作，因為是PickBox與GriCell的共同行為
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected BaseDataCacheCore<TKeyValue, TLookupValue> DefaultCacheCore { get; set; }

        /// <summary>
        /// 自訂的一組快取設定，包含MyCacheLoader, MyPopupColumnSettings, MyLookup，如果其中有未指定的，會轉呼叫DeafultCacheCore的方法
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomizeCacheCore<TKeyValue, TLookupValue> CustomizeCacheCore { get; set; }

        /// <summary>
        /// 自動選取CacheCore，有指派CustomizeCacheCore優先
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal BaseDataCacheCore<TKeyValue, TLookupValue> AutoCacheCore
        {
            get
            {
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyCacheLoader == null)
                        this.CustomizeCacheCore.MyCacheLoader = this.DefaultCacheCore.LoadCache;

                    if (this.CustomizeCacheCore.MyLookup == null)
                        this.CustomizeCacheCore.MyLookup = new Func<DataRow, TLookupValue>(row => this.DefaultCacheCore.Lookup(row));

                    return this.CustomizeCacheCore;
                }
                else
                    return this.DefaultCacheCore;
            }
        }

        /// <summary>
        /// 開窗的設定值，雖然開窗動作是PickBox與GriCell的共同行為，但是所開出來的窗體包含欄位，卻可以有所不同
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PopupColumnSettings PopupSettings { get; set; }

        /// <summary>
        /// 開窗資料如何篩選
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual CacheCoreDataFilterSetting DataFilterSetting { get; set; }

        /// <summary>
        /// 自動選取PopupSettings，有指派CustomizeCacheCore優先
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal PopupColumnSettings AutoPopupSettings
        {
            get
            {
                if (Env.DesignTime)
                    return null;
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyPopupColumnSettings == null)
                        this.CustomizeCacheCore.MyPopupColumnSettings = this.PopupSettings;
                    return this.CustomizeCacheCore.MyPopupColumnSettings;

                }
                else
                    return this.PopupSettings;
            }
        }

        /// <summary>
        /// 是否要將結果進行排序
        /// </summary>
        [Category("SCIC-BasePickLookupBox")]
        [Description("如果需要將開窗結果做預排序，會用於DataView的Sort屬性，可接受字樣舉例：\"Seq\", \"Seq Asc\", \"Seq Desc\", \"ID, Seq asc\"")]
        public string SelectDialogOrderBy
        {
            get
            {
                if (Env.DesignTime)
                    return string.Empty;
                return this.AutoPopupSettings.SelectDialogOrderBy;
            }
            set
            {
                if (Env.DesignTime)
                    return;
                this.AutoPopupSettings.SelectDialogOrderBy = value;
            }
        }

        /// <summary>
        /// <para>提供外部做資料繫結，連動this.textBox1.Text。</para>
        /// <para>set的時候會連帶呼叫 this.Lookup來設定this.displayBox1.Text</para>
        /// </summary>
        [Bindable(true)]
        [Browsable(false)]
        [Category("SCIC-BasePickLookupBox")]
        [Description("提供外部做資料繫結，連動this.textBox1.Text\r\nset的時候會連帶呼叫 this.Lookup來設定this.displayBox1.Text")]
        public string IDBinding
        {
            set
            {
                if (!Env.DesignTime)
                {
                    if (string.IsNullOrEmpty(value) == false)
                    {
                        if (textBox1.Text != value || String.IsNullOrWhiteSpace(this.displayBox1.Text))
                        {
                            var pk = this.DataFilterSetting.GetFullPK(value);
                            var row = this.AutoCacheCore.FindRowByPK(pk);
                            this.displayBox1.Text = (string)(object)this.AutoCacheCore.Lookup(row);
                        }
                    }
                    else
                        this.displayBox1.Text = null;
                }
                this.textBox1.Text = value;
            }
            get { return textBox1.Text; }
        }

        private long? _UKeyBinding = null;
        /// <summary>
        /// <para>提供外部做資料繫結</para>
        /// <para>set的時候會連帶呼叫 this.Lookup來設定this.textBox1.Text 和 this.displayBox1.Text</para>
        /// </summary>
        [Bindable(true)]
        [Browsable(false)]
        [Category("SCIC-BasePickLookupBox")]
        [Description("提供外部做資料繫結，set的時候會連帶呼叫 this.Lookup來設定this.textBox1.Text 和 this.displayBox1.Text")]
        public long? UKeyBinding
        {
            set
            {
                if (Env.DesignTime == true)
                    return;
                if (this._UKeyBinding != value)
                {
                    this._UKeyBinding = value;
                    var row = this.AutoCacheCore.CachedDataSource.Select("UKey = " + value).FirstOrDefault();
                    if (row == null)
                    {
                        this.textBox1.Text =
                            this.displayBox1.Text = string.Empty;
                    }
                    else
                    {
                        this.textBox1.Text = Convert.ToString(row[this.AutoPopupSettings.ID.ColumnName]);
                        this.textBox1.OldValue = this.textBox1.Text;
                        this.displayBox1.Text = (string)(object)this.AutoCacheCore.Lookup(row);
                    }
                }
            }
            get { return this._UKeyBinding; }
        }

        /// <summary>
        /// 提供外部做資料繫結，單純連動this.displayBox1.Text
        /// </summary>
        [Bindable(true)]
        [Browsable(false)]
        [Category("SCIC-BasePickLookupBox")]
        [Description("提供資料繫結節點，單純連動this.displayBox1.Text")]
        public string DisplayBinding
        {
            set { this.displayBox1.Text = value; }
            get { return this.displayBox1.Text; }
        }

        /// <summary>
        /// 讓內部在Show WarnningBox的時候可以有點區別性的文字，讓UI端知道現在這個訊息視窗大概是來自哪邊
        /// </summary>
        [Category("SCIC-BasePickLookupBox")]
        [Description("讓內部在Show WarnningBox的時候可以有點區別性的文字，讓UI端知道現在這個訊息視窗大概是來自哪邊")]
        public string PickName { get; set; }

        /// <summary>
        /// 讓內部在Show WarnningBox的時候可以有點區別性的文字，讓UI端知道現在這個訊息視窗大概是來自哪邊
        /// </summary>
        [Category("SCIC-BasePickLookupBox")]
        [Description("轉呼叫IDBox的PopUpMode屬性，決定何種情況下會進行開窗")]
        public TextBoxPopUpMode PopUpMode
        {
            get
            {
                return this.textBox1.PopUpMode;
            }
            set
            {
                this.textBox1.PopUpMode = value;
            }
        }

        /// <summary>
        /// 轉呼叫IDBox的ReadOnly屬性
        /// </summary>
        [Category("SCIC-BasePickLookupBox")]
        [Description("轉呼叫IDBox的ReadOnly屬性")]
        public bool ReadOnly
        {
            get
            {
                return this.textBox1.ReadOnly;
            }
            set
            {
                this.textBox1.ReadOnly = value;
            }
        }

        /// <summary>
        /// 最後一次開窗選取的資料列
        /// </summary>
        [Browsable(false)]
        public DataRow LastSelectedRow { get; set; }

        #endregion

        #region Warpper / force-cover Properties & Method

        /// <summary>
        /// 轉呼叫ID欄位的IsSupportEditMode
        /// </summary>
        [Category("SCIC-BasePickLookupBox")]
        [Description("連動IDTextBox.IsSupportEditMode")]
        public bool IsSupportEditMode
        {
            get
            {
                return this.textBox1.IsSupportEditMode;
            }
            set
            {
                this.textBox1.IsSupportEditMode = value;
            }
        }

        /// <summary>
        /// 轉呼叫ID欄位的Text
        /// </summary>
        [Category("SCIC-BasePickLookupBox")]
        [Description("連動IDTextBox.Text")]
        public new string Text
        {
            get
            {
                return this.textBox1.Text;
            }
            set
            {
                this.textBox1.Text = value;
            }
        }

        /// <summary>
        /// 轉呼叫ID欄位的OldValue
        /// </summary>
        [Category("SCIC-BasePickLookupBox")]
        [Description("連動IDTextBox.OldValue")]
        public string OldValue
        {
            get
            {
                return this.textBox1.OldValue;
            }
            set
            {
                this.textBox1.OldValue = value;
            }
        }

        /// <summary>
        /// 轉呼叫ID欄位的TFocus()
        /// </summary>
        public new void Focus()
        {
            this.textBox1.Focus();
        }

        #endregion

        /// <summary>
        /// PickBox基底
        /// </summary>
        public BasePickLookupBox()
        {
            this.InitializeComponent();
            this.PickName = "Pickup Value";
            this.DataFilterSetting = new CacheCoreDataFilterSetting();
        }

        /// <summary>
        /// 當使用者在Text欄位輸入並且觸動驗證時，將輸入結果轉換成對應的文字，放入Display欄位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            var ce = new CancelEventArgs();
            if (this.textBox1.OldValue == this.textBox1.Text)
                return;

            if (this.AutoCacheCore.CachedDataSource.PrimaryKey.Length == 1 &&
                this.AutoCacheCore.CachedDataSource.PrimaryKey[0].ColumnName.CompareTo("UKey", true) == 0)
            {
                //把UKey當作PK的資料表，無法透過所輸入的文字方塊做Validate，所以這邊直接跳過
                base.OnValidating(ce);
                if (ce.Cancel == true)
                    return;
                base.OnValidated(EventArgs.Empty);
                return;
            }
            string textValue = this.textBox1.Text;
            this.LastSelectedRow = null;
            if (string.IsNullOrEmpty(textValue))
            {
                this.displayBox1.Text = null;
                this.TryBindingWrite();
                return;
            }

            var fullPK = this.DataFilterSetting.GetFullPK(textValue);
            this.LastSelectedRow = this.AutoCacheCore.FindRowByValidatingSetting(this.DataFilterSetting, fullPK);
            if (this.LastSelectedRow == null)
            {
                this.textBox1.Text = "";
                MyUtility.Msg.ErrorBox(string.Format("< {0}: {1} > not found!!!", this.PickName, textValue), "Error");
                e.Cancel = true;
                return;
            }

            this.displayBox1.Text = (string)(object)this.AutoCacheCore.Lookup(this.LastSelectedRow);
            this.TryBindingWrite();
            base.OnValidating(ce);
            if (ce.Cancel == true)
                return;
            base.OnValidated(EventArgs.Empty);
        }

        /// <summary>
        /// 當使用者在Text欄位點右鍵，會開啟選擇方塊，接收回傳結果，如果有選，會呼叫ValidateControl更新Display欄位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (e.IsHandled == true)
                return;

            switch (this.textBox1.PopUpMode)
            {
                case TextBoxPopUpMode.EditMode:
                    if (this.ParentForm is Ict.Win.IForm)
                    {
                        if (((Sci.Win.IForm)this.ParentForm).EditMode == false)
                            return;
                    }
                    break;
                case TextBoxPopUpMode.EditModeAndNonReadOnly:
                    if (this.ParentForm is Ict.Win.IForm)
                    {
                        if (((Sci.Win.IForm)this.ParentForm).EditMode == false)
                            return;
                        else if (this.textBox1.ReadOnly == true)
                            return;
                    }
                    break;
                case TextBoxPopUpMode.EditModeAndReadOnly:
                    if (this.ParentForm is Ict.Win.IForm)
                    {
                        if (((Sci.Win.IForm)this.ParentForm).EditMode == false)
                            return;
                        else if (this.textBox1.ReadOnly == false)
                            return;
                    }
                    break;
                case TextBoxPopUpMode.NonReadOnly:
                    if (this.textBox1.ReadOnly == true)
                        return;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var selectedRow = this.AutoCacheCore.ShowPopupDialog(this.AutoPopupSettings, this.textBox1.Text.Trim(), this.DataFilterSetting);
            if (selectedRow == null)
                return;

            this.LastSelectedRow = selectedRow;
            //命令popup setting用自己的設定值取回主鍵值，放給IDBinding屬性
            //接著觸動IDBinding_set裡面的連動程序，進而更新Display欄位
            //但如果是有使用UKey作為代替主鍵，那就改用UKeyBinding
            if (this.AutoPopupSettings.UsingUKeyForPopup)
                this.UKeyBinding = this.LastSelectedRow.Field<long?>("UKey");
            else
                this.IDBinding = (string)this.AutoPopupSettings.GetID(this.LastSelectedRow);

            var ce = new CancelEventArgs();
            base.OnValidating(ce);
            if (ce.Cancel)
                return;
            this.TryBindingWrite();
            base.OnValidated(EventArgs.Empty);
        }

        /// <summary>
        /// 用指定的輸入字元上限，去調整textbox1的寬度，displaybox的寬度與左緣，並設定textbox1.MaxLength
        /// </summary>
        /// <param name="maxLength"></param>
        internal protected void SetInputMaxLengthAndWidth(int maxLength)
        {
            var needWidth = -1;
            using (var gra = this.textBox1.CreateGraphics())
            {
                var size = gra.MeasureString(new string('w', maxLength), this.textBox1.Font);
                needWidth = size.ToSize().Width + 5; //用N個'w'去量寬度，量出來的寬度再轉成整數寬度，並且多給他5(比較美觀)
            }
            var widthAdjust = needWidth - this.textBox1.Width; //可能正數可能負數喔
            this.textBox1.Width += widthAdjust;
            this.displayBox1.Width -= widthAdjust;
            this.displayBox1.Left += widthAdjust;
            this.textBox1.MaxLength = maxLength;
        }

        #region DataBinding warpper

        private IEnumerable<Binding> GetAllDataBinding()
        {
            return this.DataBindings.Cast<Binding>();
        }

        /// <summary>
        /// 嘗試將全部的DataBinding做ReadValue動作
        /// </summary>
        public void TryBindingRead()
        {
            this.GetAllDataBinding().ToList().ForEach(bd => bd.ReadValue());
        }

        /// <summary>
        /// 嘗試將全部的DataBinding做WriteValue動作
        /// </summary>
        public void TryBindingWrite()
        {
            this.GetAllDataBinding().ToList().ForEach(bd => bd.WriteValue());
        }

        #endregion

        /// <summary>
        /// 當繼承端有一些前置條件改變時，可呼叫此方法，來重新檢查輸入的值是否存在於新條件下的CacheCore內，若不存在，會清空
        /// </summary>
        protected void ReValidate()
        {
            if (this.AutoCacheCore.CachedDataSource.PrimaryKey.Length == 1 &&
                this.AutoCacheCore.CachedDataSource.PrimaryKey[0].ColumnName.CompareTo("UKey", true) == 0)
            {
                //把UKey當作PK的資料表，無法透過所輸入的文字方塊做Validate，所以這邊直接跳過
                return;
            }
            var pks = this.DataFilterSetting.GetFullPK(this.Text);
            var refindResult = this.AutoCacheCore.FindRowByValidatingSetting(this.DataFilterSetting, pks);
            if (refindResult == null)
            {
                this.textBox1.Text =
                    this.displayBox1.Text = null;
            }
            else
            {
                this.displayBox1.Text = Convert.ToString(this.AutoCacheCore.Lookup(refindResult));
            }
        }

        #region IDisposable

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            this.DefaultCacheCore = null;
            if (this.CustomizeCacheCore != null)
            {
                this.CustomizeCacheCore.Dispose();
                this.CustomizeCacheCore = null;
            }
        }

        #endregion
    }

    /// <summary>
    /// PickBox基底
    /// </summary>
    public abstract class BasePickDateBox : Sci.Win.UI.DateBox, IIdBindableControl
    {
        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler PickBoxSelected;
        private void OnPickBoxSelected()
        {
            if (this.PickBoxSelected != null)
                this.PickBoxSelected(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler PickBoxCleared;
        private void OnPickBoxCleared()
        {
            if (this.PickBoxCleared != null)
                this.PickBoxCleared(this, EventArgs.Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 用於快取資料的提供與重載，也負責做開窗動作，因為是PickBox與GriCell的共同行為
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected BaseDataCacheCore<string, DateTime?> DefaultCacheCore { get; set; }

        /// <summary>
        /// 自訂的一組快取設定，包含MyCacheLoader, MyPopupColumnSettings, MyLookup，如果其中有未指定的，會轉呼叫DeafultCacheCore的方法
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomizeCacheCore<string, DateTime?> CustomizeCacheCore { get; set; }

        /// <summary>
        /// 自動選取CacheCore，有指派CustomizeCacheCore優先
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal BaseDataCacheCore<string, DateTime?> AutoCacheCore
        {
            get
            {
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyCacheLoader == null)
                        this.CustomizeCacheCore.MyCacheLoader = this.DefaultCacheCore.LoadCache;

                    if (this.CustomizeCacheCore.MyLookup == null)
                        this.CustomizeCacheCore.MyLookup = new Func<DataRow, DateTime?>(row => this.DefaultCacheCore.Lookup(row));

                    return this.CustomizeCacheCore;
                }
                else
                    return this.DefaultCacheCore;
            }
        }

        /// <summary>
        /// 開窗的設定值，雖然開窗動作是PickBox與GriCell的共同行為，但是所開出來的窗體包含欄位，卻可以有所不同
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PopupColumnSettings PopupSettings { get; set; }

        /// <summary>
        /// 開窗資料如何篩選
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual CacheCoreDataFilterSetting DataFilterSettings { get; set; }

        /// <summary>
        /// 自動選取PopupSettings，有指派CustomizeCacheCore優先
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal PopupColumnSettings AutoPopupSettings
        {
            get
            {
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyPopupColumnSettings == null)
                        this.CustomizeCacheCore.MyPopupColumnSettings = this.PopupSettings;
                    return this.CustomizeCacheCore.MyPopupColumnSettings;

                }
                else
                    return this.PopupSettings;
            }
        }

        /// <summary>
        /// 是否要將結果進行排序
        /// </summary>
        [Category("SCIC.BasePickDateBox")]
        [Description("如果需要將開窗結果做預排序，會用於DataView的Sort屬性，可接受字樣舉例：\"Seq\", \"Seq Asc\", \"Seq Desc\", \"ID, Seq asc\"")]
        public string SelectDialogOrderBy
        {
            get
            {
                return this.AutoPopupSettings.SelectDialogOrderBy;
            }
            set
            {
                this.AutoPopupSettings.SelectDialogOrderBy = value;
            }
        }

        /// <summary>
        /// 所選擇的ID
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ID { get; set; }

        /// <summary>
        /// 提供外部做資料繫結，連動this.textBox1.Text。
        /// </summary>
        [Bindable(true)]
        [Category("SCIC.BasePickDateBox")]
        [Description("提供外部做資料繫結，連動this.ID\r\nset的時候會連帶呼叫 this.Lookup來設定base.Value")]
        public string IDBinding
        {
            set
            {
                if (!Env.DesignTime)
                {
                    if (string.IsNullOrEmpty(value))
                        base.Value = null;
                    else
                    {
                        var pk = this.DataFilterSettings.GetFullPK(value);
                        var row = this.AutoCacheCore.FindRowByPK(pk);
                        var findResult = this.AutoCacheCore.Lookup(row);
                        if (findResult.HasValue == false)
                            MyUtility.Msg.WarningBox("ID not found : " + value);
                        if (findResult != base.Value)
                        {
                            base.Value = findResult;
                            base.OldValue = findResult;
                        }
                    }
                }
                this.ID = value;
            }
            get { return ID; }
        }

        /// <summary>
        /// <para>提供外部做資料繫結，連動this.dateBox1.Value。</para>
        /// </summary>
        [Bindable(true)]
        [Category("SCIC.BasePickDateBox")]
        [Description("提供資料繫結節點，單純連動base.Value")]
        public DateTime? DateBinding
        {
            set { base.Value = value; }
            get { return base.Value; }
        }

        /// <summary>
        /// 讓內部在Show WarnningBox的時候可以有點區別性的文字，讓UI端知道現在這個訊息視窗大概是來自哪邊
        /// </summary>
        [Category("SCIC-BasePickDateBox")]
        [Description("讓內部在Show WarnningBox的時候可以有點區別性的文字，讓UI端知道現在這個訊息視窗大概是來自哪邊")]
        public string PickName { get; set; }

        #endregion

        /// <summary>
        /// PickupDateBox基底
        /// </summary>
        public BasePickDateBox()
        {
            this.PickName = "Pickup Value";
            this.DataFilterSettings = new CacheCoreDataFilterSetting();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnValidating");
            if (base.OldValue == base.Value)
                return;
            base.OnValidating(e);
            if (e.Cancel == true)
                return;

            if (base.Value == null)
            {
                this.ID = null;
                this.OnPickBoxCleared();
                return;
            }

            this.OpenSelect(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextMouseUp(MouseEventArgs e)
        {
            base.OnTextMouseUp(e);
            if (e.Button == MouseButtons.Right)
            {
                if (this.FindForm<Sci.Win.Tems.Base>().EditMode == false)
                    return;
                if (base.IsSupportEditMode == false)
                    return;

                this.OpenSelect(new CancelEventArgs());
            }
        }

        /// <summary>
        /// 當按下Esc時候，把值還原回去
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                base.Value = base.OldValue;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool DuringOpenSelect = false;
        private void OpenSelect(CancelEventArgs e)
        {
            if (this.AutoCacheCore.CachedDataSource == null)
                this.AutoCacheCore.ClearCacheAndReload();
            if (DuringOpenSelect)
                return;

            DuringOpenSelect = true;
            var oldValue = base.OldValue;
            var newValue = base.Value;
            try
            {
                using (var filteredDataSource = this.GetFilteredDataTable(this.AutoCacheCore.CachedDataSource))
                {
                    var selectResult = this.AutoCacheCore.ShowPopupDialog(this.AutoPopupSettings, null, filteredDataSource, this.DataFilterSettings);
                    if (selectResult != null)
                    {
                        var idFieldName = this.PopupSettings.ID.ColumnName;
                        var idValue = selectResult.Field<string>(idFieldName);
                        this.IDBinding = idValue;
                        this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                        //Raise event
                        this.OnPickBoxSelected();
                        this.NotifyInvalidate(this.Bounds);
                    }
                    else
                    {
                        base.OldValue = oldValue;
                        base.Value = newValue;
                        e.Cancel = true;
                    }
                }
            }
            finally
            {
                DuringOpenSelect = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected virtual DataTable GetFilteredDataTable(DataTable Data)
        {
            return Data.Copy();
        }

        #region IDisposable

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.DefaultCacheCore = null;
            if (this.CustomizeCacheCore != null)
            {
                this.CustomizeCacheCore.Dispose();
                this.CustomizeCacheCore = null;
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKeyValue"></typeparam>
    /// <typeparam name="TLookupValue"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CachedBondleGridCellEventHandler<TKeyValue, TLookupValue>(object sender, CachedBondleGridCellEventArg<TKeyValue, TLookupValue> e);

    /// <summary>
    /// 由具有CacheCore的GridCell拋出來的相關事件引數，包含有對應的CacheCore，最後選到的DataRow，以及RowIndex和ColumnIndex
    /// </summary>
    public class CachedBondleGridCellEventArg<TKeyValue, TLookupValue> : EventArgs
    {
        /// <summary>
        /// 關聯GridCellSetting當初設定的CacheCore
        /// </summary>
        public BaseDataCacheCore<TKeyValue, TLookupValue> DataCacheCore { get; set; }
        /// <summary>
        /// 開窗選定的資料列，或是輸入文字被驗證後找到的資料列(清空欄位後的驗證會讓此屬性為null)
        /// </summary>
        public DataRow SelectedRow { get; set; }
        /// <summary>
        /// 開窗選定的資料列，透過Lookup動作取回的值，可能是各種型態，看定義的TLookup型態為何(清空欄位後的驗證會讓此屬性為null)
        /// </summary>
        public object LookupResult { get; set; }
        /// <summary>
        /// 與所在Grid，利用RowIndex取回的DataRow
        /// </summary>
        public DataRow GridRowBoundItem { get; set; }
        /// <summary>
        /// 發生事件的RowIndex
        /// </summary>
        public int RowIndex { get; set; }
        /// <summary>
        /// 發生事件的儲存格的ColumnIndex
        /// </summary>
        public int ColumnIndex { get; set; }
        /// <summary>
        /// 新ID
        /// </summary>
        public object NewValue { get; set; }
        /// <summary>
        /// 原ID
        /// </summary>
        public object OldValue { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseGridCellSetting : BaseGridCellSetting<string, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idColumnName"></param>
        /// <param name="popupColumnSettings"></param>
        public BaseGridCellSetting(string idColumnName, PopupColumnSettings popupColumnSettings)
            : base(idColumnName, popupColumnSettings)
        {

        }
    }

    /// <summary>
    /// GridCellSetting基底
    /// </summary>
    public abstract class BaseGridCellSetting<TKeyValue, TLookupValue> : DataGridViewGeneratorTextColumnSettings
    {
        #region Events

        /// <summary>
        /// 代表使用者點了右鍵，並於開窗中，選定一筆資料，然後按下Select按鈕，此時已經將ID欄位放入所選定的值
        /// </summary>
        public event CachedBondleGridCellEventHandler<TKeyValue, TLookupValue> EditingMouseDownCompleted;

        private void OnEditingMouseDownCompleted(CachedBondleGridCellEventArg<TKeyValue, TLookupValue> e)
        {
            if (this.EditingMouseDownCompleted != null)
                this.EditingMouseDownCompleted(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public event CachedBondleGridCellEventHandler<TKeyValue, TLookupValue> CellValidatingComleted;

        private void OnCellValidatingComleted(CachedBondleGridCellEventArg<TKeyValue, TLookupValue> e)
        {
            if (this.CellValidatingComleted != null)
                this.CellValidatingComleted(this, e);
        }

        /// <summary>
        /// 當擁有的DataGridView改變時候觸發
        /// </summary>
        public event EventHandler OwnerGridChanged;

        /// <summary>
        /// 
        /// </summary>
        protected void OnOwnerGridChanged()
        {
            if (this.OwnerGridChanged != null)
                this.OwnerGridChanged(this, EventArgs.Empty);
        }
        #endregion

        #region Properties

        /// <summary>
        /// 開窗資料如何篩選
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CacheCoreDataFilterSetting DataFilterSetting { get; set; }

        /// <summary>
        /// 用於快取資料的提供與重載，也負責做開窗動作，因為是PickBox與GriCell的共同行為
        /// </summary>
        protected BaseDataCacheCore<TKeyValue, TLookupValue> DefaultCacheCore { get; set; }

        /// <summary>
        /// 自訂的一組快取設定，請務必完整設定，包含MyCacheLoader, MyPopupColumnSettings, MyLookup
        /// </summary>
        public CustomizeCacheCore<TKeyValue, TLookupValue> CustomizeCacheCore { get; set; }

        /// <summary>
        /// 自動選取CacheCore，有指派CustomizeCacheCore優先
        /// </summary>
        internal BaseDataCacheCore<TKeyValue, TLookupValue> AutoCacheCore
        {
            get
            {
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyCacheLoader == null)
                        this.CustomizeCacheCore.MyCacheLoader = this.DefaultCacheCore.LoadCache;

                    if (this.CustomizeCacheCore.MyLookup == null)
                        this.CustomizeCacheCore.MyLookup = new Func<DataRow, TLookupValue>(row => this.DefaultCacheCore.Lookup(row));

                    return this.CustomizeCacheCore;
                }
                else
                    return this.DefaultCacheCore;
            }
        }

        /// <summary>
        /// 開窗的設定值，雖然開窗動作是PickBox與GriCell的共同行為，但是所開出來的窗體包含欄位，卻可以有所不同
        /// </summary>
        public PopupColumnSettings PopupSettings { get; internal set; }

        /// <summary>
        /// 自動選取PopupSettings，有指派CustomizeCacheCore優先
        /// </summary>
        internal PopupColumnSettings AutoPopupSettings
        {
            get
            {
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyPopupColumnSettings == null)
                        this.CustomizeCacheCore.MyPopupColumnSettings = this.PopupSettings;
                    return this.CustomizeCacheCore.MyPopupColumnSettings;

                }
                else
                    return this.PopupSettings;
            }
        }

        /// <summary>
        /// 是否要將結果進行排序
        /// </summary>
        public string OrderBy
        {
            get
            {
                return this.AutoPopupSettings.SelectDialogOrderBy;
            }
            set
            {
                this.AutoPopupSettings.SelectDialogOrderBy = value;
            }
        }

        /// <summary>
        /// 在目前的Grid.DataRow裡面，哪一個欄位用於提供本Cell的資料繫結值
        /// </summary>
        public string IdColumnName { get; set; }

        private System.Windows.Forms.DataGridView _OwnerGrid = null;
        /// <summary>
        /// 與此物件做關聯(擁有者)的DataGridView物件，用於取得目前Row
        /// </summary>
        public System.Windows.Forms.DataGridView OwnerGrid
        {
            get
            {
                return this._OwnerGrid;
            }
            set
            {
                this._OwnerGrid = value;
                this.OnOwnerGridChanged();
            }
        }

        #endregion

        /// <summary>
        /// GridCellSetting基底
        /// </summary>
        /// <param name="idColumnName"></param>
        /// <param name="popupColumnSettings"></param>
        public BaseGridCellSetting(string idColumnName, PopupColumnSettings popupColumnSettings)
        {
            if (popupColumnSettings == null)
                throw new InvalidOperationException("popupColumnSettings can not be null");
            base.EditingMouseDown += this.EditingMouseDown;
            base.CellValidating += this.CellValidating;
            this.PopupSettings = popupColumnSettings;
            this.IdColumnName = idColumnName;
            this.DataFilterSetting = new CacheCoreDataFilterSetting();
        }

        /// <summary>
        /// 當在格子上點右鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private new void EditingMouseDown(object sender, Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var grid = ((DataGridViewColumn)sender).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                    return;

                //準備開窗
                var row = grid.GetDataRow<DataRow>(e.RowIndex);

                var defaultValues = row.IsNull(IdColumnName) ? null : row.Field<string>(IdColumnName);
                var selectedRow = this.AutoCacheCore.ShowPopupDialog(this.AutoPopupSettings, defaultValues, this.DataFilterSetting);

                //處理選擇結果
                if (selectedRow == null)
                    return;

                //放回資料列
                var newValue = this.PopupSettings.GetID(selectedRow);
                row[IdColumnName] = newValue;

                //命令grid更新格子
                grid.InvalidateCell(e.Cell);
                grid.InvalidateRow(e.RowIndex);

                //Raise event
                var arg = new CachedBondleGridCellEventArg<TKeyValue, TLookupValue>();
                arg.DataCacheCore = this.AutoCacheCore;
                arg.RowIndex = e.RowIndex;
                arg.ColumnIndex = e.ColumnIndex;
                arg.SelectedRow = selectedRow;
                arg.LookupResult = this.AutoCacheCore.Lookup(selectedRow);
                arg.OldValue = defaultValues;
                arg.NewValue = newValue;
                arg.GridRowBoundItem = row;

                this.OnEditingMouseDownCompleted(arg);
                ((Ict.Win.UI.DataGridViewTextBoxEditingControl)(grid.EditingControl)).Text = newValue;
            }
        }

        /// <summary>
        /// 當在格子裡面直接打字，欄位離開的驗證
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private new void CellValidating(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            var grid = ((DataGridViewColumn)sender).DataGridView;
            if (((DataGridViewColumn)sender).DataGridView.ReadOnly) { return; }
            var row = grid.GetDataRow<DataRow>(e.RowIndex);

            //確定新值
            var newValue = (string)e.FormattedValue;
            var oldValue = row[IdColumnName];

            if (string.Compare(newValue, row.Field<string>(IdColumnName)) == 0)
                return;

            var arg = new CachedBondleGridCellEventArg<TKeyValue, TLookupValue>();
            arg.DataCacheCore = this.AutoCacheCore;
            arg.SelectedRow = null;
            arg.LookupResult = null;
            arg.OldValue = oldValue;
            arg.NewValue = newValue;
            arg.RowIndex = e.RowIndex;
            arg.ColumnIndex = e.ColumnIndex;
            arg.GridRowBoundItem = row;
            if (string.IsNullOrEmpty(newValue))
            {
                this.OnCellValidatingComleted(arg);
                return;
            }

            //用輸入的值當PK去搜尋Cache
            var fullPK = this.DataFilterSetting.GetFullPK(newValue);
            arg.SelectedRow = this.DefaultCacheCore.FindRowByValidatingSetting(this.DataFilterSetting, fullPK);

            if (arg.SelectedRow == null)
            {
                MyUtility.Msg.ErrorBox(newValue + " is not found", "Error");
                e.Cancel = true;
            }
            else
            {
                e.FormattedValue = newValue = this.AutoPopupSettings.GetID(arg.SelectedRow); //因為使用者輸入可能不會管大小寫，這行會把輸入的字串換成實際存在的大小寫格式
                row[this.IdColumnName] = newValue;
                arg.LookupResult = this.AutoCacheCore.Lookup(arg.SelectedRow);
                this.OnCellValidatingComleted(arg);
                ((Ict.Win.UI.DataGridViewTextBoxEditingControl)(grid.EditingControl)).Text = newValue;
            }
        }

        /// <summary>
        /// 當繼承端有一些前置條件改變時，可呼叫此方法，來重新檢查輸入的值是否存在於新條件下的CacheCore內，若不存在，會清空
        /// </summary>
        protected void ReValidate()
        {
            if (this.OwnerGrid == null)
                return;

            if (this.OwnerGrid.CurrentRow == null)
                return;

            if (this.OwnerGrid.CurrentRow.DataBoundItem == null)
                return;

            var rowView = this.OwnerGrid.CurrentRow.DataBoundItem as DataRowView;

            var row = rowView.Row;

            var fieldValue = row.Field<string>(this.IdColumnName);

            var pks = this.DataFilterSetting.GetFullPK(fieldValue);
            var refindResult = this.AutoCacheCore.FindRowByValidatingSetting(this.DataFilterSetting, pks);
            CachedBondleGridCellEventArg<TKeyValue, TLookupValue> arg;
            if (refindResult == null)
            {
                fieldValue = null;
                row[this.IdColumnName] = null;
                arg = new CachedBondleGridCellEventArg<TKeyValue, TLookupValue>();
                arg.DataCacheCore = this.AutoCacheCore;
                arg.SelectedRow = null;
                arg.LookupResult = null;
                arg.OldValue = fieldValue;
                arg.NewValue = fieldValue;
                arg.RowIndex = -1;
                arg.ColumnIndex = -1;
                arg.GridRowBoundItem = row;
            }
            else
            {
                arg = new CachedBondleGridCellEventArg<TKeyValue, TLookupValue>();
                arg.DataCacheCore = this.AutoCacheCore;
                arg.SelectedRow = refindResult;
                arg.LookupResult = this.AutoCacheCore.Lookup(refindResult);
                arg.OldValue = fieldValue;
                arg.NewValue = fieldValue;
                arg.RowIndex = -1;
                arg.ColumnIndex = -1;
                arg.GridRowBoundItem = row;
            }
            row.BeginEdit();
            this.OnCellValidatingComleted(arg);
            row[this.IdColumnName] = fieldValue;
            row.EndEdit();
        }

        #region IDispose

        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {
            this.DefaultCacheCore = null;
            if (this.CustomizeCacheCore != null)
            {
                this.CustomizeCacheCore.Dispose();
                this.CustomizeCacheCore = null;
            }
        }

        #endregion
    }

    /// <summary>
    /// 多選文字方塊基底
    /// </summary>
    public abstract class BaseMultiplePickIdBox<TKeyValue, TLookupValue> : Sci.Win.UI.TextBox
    {
        #region Events
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler PickBoxSelected;
        private void OnPickBoxSelected()
        {
            if (this.PickBoxSelected != null)
                this.PickBoxSelected(this, EventArgs.Empty);
        }

        /// <summary>
        /// 這裡面是要給開出來的視窗用的事件，所以先堆積在Dictionary裡面，要開窗的時候在Attach上去
        /// </summary>
        private List<MultiSelectDialogEventHandler<DataGridViewCellEventArgs>> EventCollection = new List<MultiSelectDialogEventHandler<DataGridViewCellEventArgs>>();
        /// <summary>
        /// 當開窗的Gird發生CellDoubleClick的時候
        /// </summary>
        public event MultiSelectDialogEventHandler<DataGridViewCellEventArgs> PickDialogCellDoubleClick
        {
            add
            {
                if (this.EventCollection.Contains(value) == false)
                    this.EventCollection.Add(value);
            }
            remove
            {
                if (this.EventCollection.Contains(value))
                    this.EventCollection.Remove(value);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 用於快取資料的提供與重載，也負責做開窗動作，因為是PickBox與GriCell的共同行為
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected BaseDataCacheCore<TKeyValue, TLookupValue> DefaultCacheCore { get; set; }

        /// <summary>
        /// 自訂的一組快取設定，請務必完整設定，包含MyCacheLoader, MyPopupColumnSettings, MyLookup
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomizeCacheCore<TKeyValue, TLookupValue> CustomizeCacheCore { get; set; }

        /// <summary>
        /// 自動選取CacheCore，有指派CustomizeCacheCore優先
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal BaseDataCacheCore<TKeyValue, TLookupValue> AutoCacheCore
        {
            get
            {
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyCacheLoader == null)
                        this.CustomizeCacheCore.MyCacheLoader = this.DefaultCacheCore.LoadCache;

                    if (this.CustomizeCacheCore.MyLookup == null)
                        this.CustomizeCacheCore.MyLookup = new Func<DataRow, TLookupValue>(row => this.DefaultCacheCore.Lookup(row));

                    return this.CustomizeCacheCore;
                }
                else
                    return this.DefaultCacheCore;
            }
        }

        /// <summary>
        /// 開窗的設定值，雖然開窗動作是PickBox與GriCell的共同行為，但是所開出來的窗體包含欄位，卻可以有所不同
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PopupColumnSettings PopupSettings { get; set; }

        /// <summary>
        /// 開窗資料如何篩選
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual CacheCoreDataFilterSetting DataFilterSetting { get; internal set; }

        /// <summary>
        /// 自動選取PopupSettings，有指派CustomizeCacheCore優先
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal PopupColumnSettings AutoPopupSettings
        {
            get
            {
                if (this.CustomizeCacheCore != null)
                {
                    if (this.CustomizeCacheCore.MyPopupColumnSettings == null)
                        this.CustomizeCacheCore.MyPopupColumnSettings = this.PopupSettings;
                    return this.CustomizeCacheCore.MyPopupColumnSettings;

                }
                else
                    return this.PopupSettings;
            }
        }

        /// <summary>
        /// 是否要將結果進行排序
        /// </summary>
        [Category("SCIC-BaseMultiplePickIdBox")]
        [Description("如果需要將開窗結果做預排序，會用於DataView的Sort屬性，可接受字樣舉例：\"Seq\", \"Seq Asc\", \"Seq Desc\", \"ID, Seq asc\"")]
        public string OrderBy
        {
            get
            {
                return this.AutoPopupSettings.SelectDialogOrderBy;
            }
            set
            {
                this.AutoPopupSettings.SelectDialogOrderBy = value;
            }
        }

        /// <summary>
        /// 是否要點右鍵的時候開啟一個對話視窗，顯示當前文字方塊內的全部文字
        /// </summary>
        [Category("SCIC-BaseMultiplePickIdBox")]
        [Description("是否要當滑鼠雙點的時候，像是EditBox一樣的開一個小視窗出來，讓使用者看完整的字串(當所選項目很容易燒過文字方塊寬度時使用)，假如未指定，預設為true")]
        public bool? WithDisplayWindow { get; set; }

        #endregion

        /// <summary>
        /// 多選文字方塊基底
        /// </summary>
        public BaseMultiplePickIdBox()
        {
            this.SuspendLayout();
            this.DataFilterSetting = new CacheCoreDataFilterSetting();
            this.Size = new System.Drawing.Size(200, 22);
            this.IsSupportEditMode = false;
            this.PopUpMode = TextBoxPopUpMode.EditModeAndReadOnly;
            this.ReadOnly = true;
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (this.WithDisplayWindow.GetValueOrDefault(true) == true)
            {
                var frm = this.FindForm<Sci.Win.Forms.Base>();
                var frmEditMode = frm == null ? false : frm.EditMode;
                var selfEditable = base.ReadOnly == false;
                //不但要是Form編輯模式，自己還必須是可編輯狀態，才允許這個開窗可編輯
                using (var dlg = new Sci.Win.Tools.EditMemo(Text, null, frmEditMode && selfEditable, null))
                {
                    dlg.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// 打開選取方塊
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            if (((Sci.Win.Tems.Base)this.FindForm()).EditMode == false)
                return;
            string selecteds = this.Text;

            var selectedResults = this.AutoCacheCore.ShowPopupDialog2(this.AutoPopupSettings, this.Text, this.DataFilterSetting, this.EventCollection.ToArray());

            if (selectedResults == null)
                return;

            var idFieldName = this.PopupSettings.ID.ColumnName;
            this.Text = string.Join(",", selectedResults.Select(r =>
            {
                return r.Field<string>(idFieldName);
            }).ToArray());
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            this.ValidateControl();

            //Raise event
            this.OnPickBoxSelected();
        }

        #region DataBinding warpper

        private IEnumerable<Binding> GetAllDataBinding()
        {
            return this.DataBindings.Cast<Binding>();
        }

        /// <summary>
        /// 嘗試將全部的DataBinding做ReadValue動作
        /// </summary>
        public void TryBindingRead()
        {
            this.GetAllDataBinding().ToList().ForEach(bd => bd.ReadValue());
        }

        /// <summary>
        /// 嘗試將全部的DataBinding做WriteValue動作
        /// </summary>
        public void TryBindingWrite()
        {
            this.GetAllDataBinding().ToList().ForEach(bd => bd.WriteValue());
        }

        #endregion

        /// <summary>
        /// 釋放資源(CacheCore)
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.DefaultCacheCore = null;
            if (this.CustomizeCacheCore != null)
            {
                this.CustomizeCacheCore.Dispose();
                this.CustomizeCacheCore = null;
            }
        }

        /// <summary>
        /// 當繼承端有一些前置條件改變時，可呼叫此方法，來重新檢查輸入的值是否存在於新條件下的CacheCore內，若不存在，會清空
        /// </summary>
        protected void ReValidate()
        {
            if (this.AutoCacheCore.CachedDataSource.PrimaryKey.Length == 1 &&
                this.AutoCacheCore.CachedDataSource.PrimaryKey[0].ColumnName.CompareTo("UKey", true) == 0)
            {
                //把UKey當作PK的資料表，無法透過所輸入的文字方塊做Validate，所以這邊直接跳過
                return;
            }
            base.Text = base.Text.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(id =>
            {
                var fullPk = this.DataFilterSetting.GetFullPK(id);
                return this.AutoCacheCore.FindRowByPK(fullPk) != null;
            }).JoinToString(",");
        }
    }

    /// <summary>
    /// 負責去抓/重抓資料庫裡面的資料，暫存在記憶體中，還負責開啟選擇視窗
    /// </summary>
    public abstract class BaseDataCacheCore<TKeyValue, TLookupValue>
    {
        private DataTable _CachedDataSource = null;
        /// <summary>
        /// 快取資料來源
        /// </summary>
        public DataTable CachedDataSource
        {
            get
            {
                if (this._CachedDataSource == null)
                    this.ClearCacheAndReload();
                else if (this.IsCacheExpired)
                    this.ClearCacheAndReload();
                return this._CachedDataSource;
            }
        }

        /// <summary>
        /// 前一次的重抓Cache時間
        /// </summary>
        public DateTime? LastReloadTime { get; set; }

        /// <summary>
        /// Cache的有效期限
        /// </summary>
        public TimeSpan CacheExpireInterval { get; set; }

        /// <summary>
        /// BaseDataCacheCore
        /// </summary>
        public BaseDataCacheCore()
        {
            this.CacheExpireInterval = TimeSpan.FromMinutes(3);
        }

        /// <summary>
        /// 目前Cache在什麼時間後會被視為已過期
        /// </summary>
        public DateTime CacheExpiredAt
        {
            get
            {
                return LastReloadTime.HasValue ?
                    this.LastReloadTime.Value.Add(CacheExpireInterval) :
                    DateTime.MaxValue;
            }
        }

        /// <summary>
        /// 藉由比較現在時間與CacheExpiredAt屬性來判斷是否需要重新載入
        /// </summary>
        public bool IsCacheExpired
        {
            get
            {
                return DateTime.Now > this.CacheExpiredAt;
            }
        }

        /// <summary>
        /// 命令重抓快取，如果CustomizeLoadCacheMethod有值，則優先使用，否則呼叫衍伸類別的LoadCache()
        /// </summary>
        public void ClearCacheAndReload()
        {
            this.ClearCache();
            this._CachedDataSource = this.LoadCache();
            if (this._CachedDataSource == null)
                return; //CustomizeCacheCore有機會呼叫端發覺異常，而回傳null，這邊只要略過就可以了
            if (string.IsNullOrEmpty(this._CachedDataSource.TableName))
                throw new InvalidOperationException("請確認抓回來的Table有設定TableName，因為Lookup動作會需要");
            if (this._CachedDataSource.PrimaryKey == null ||
                this._CachedDataSource.PrimaryKey.Any() == false)
                throw new InvalidOperationException("請確認抓回來的Table有設定PrimaryKey，因為Lookup動作會需要");
            this.LastReloadTime = DateTime.Now;
        }

        /// <summary>
        /// <para>當目前快取的先決條件改變時，要呼叫此方法來清楚目前的快取內容</para>
        /// <para>與ClearCacheAndReload()不同的地方是，ClearCache不會立即呼叫衍伸類別的LoadCache，可以讓某些反覆頻繁的異動更順利的進行，不會因為一直呼叫Reload而有等待時間</para>
        /// </summary>
        public void ClearCache()
        {
            if (this._CachedDataSource != null)
                this._CachedDataSource.Dispose();
            this._CachedDataSource = null;
        }

        /// <summary>
        /// 重抓快取(衍伸類別實作)
        /// </summary>
        /// <returns></returns>
        internal protected abstract DataTable LoadCache();

        /// <summary>
        /// 用目前的CachedDataSource打開篩選視窗，給定要用來顯示的設定值，還有一開始要指定的值，視窗關閉後回傳所選的新值
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="defaultValue"></param>
        /// <param name="dataFactory"></param>
        /// <param name="onSelectedRowDoubleClickEvents"></param>
        /// <returns></returns>
        public DataRow[] ShowPopupDialog2(PopupColumnSettings settings, object defaultValue, CacheCoreDataFilterSetting dataFactory = null, params MultiSelectDialogEventHandler<DataGridViewCellEventArgs>[] onSelectedRowDoubleClickEvents)
        {
            return this.ShowPopupDialog2(settings, defaultValue, this.CachedDataSource, dataFactory, onSelectedRowDoubleClickEvents);
        }

        /// <summary>
        /// 用指定的DataTable打開篩選視窗，給定要用來顯示的設定值，還有一開始要指定的值，視窗關閉後回傳所選的新值
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="defaultValue"></param>
        /// <param name="datas"></param>
        /// <param name="dataFactory"></param>
        /// <param name="onSelectedRowDoubleClickEvents"></param>
        /// <returns></returns>
        public DataRow[] ShowPopupDialog2(PopupColumnSettings settings, object defaultValue, DataTable datas, CacheCoreDataFilterSetting dataFactory = null, params MultiSelectDialogEventHandler<DataGridViewCellEventArgs>[] onSelectedRowDoubleClickEvents)
        {
            DataTable filteredTable;
            using (filteredTable = dataFactory.GetDialogDataSource(datas))
            {
                var settingForDialog = settings.Where(sett => sett.ColumnWidth != 0);
                var columnNames = settingForDialog.Select(col => col.ColumnName).JoinToString(",");
                var columnHeaders = settingForDialog.Select(col => col.HeaderText).JoinToString(",");
                var columnWidths = settingForDialog.Select(col => col.ColumnWidth.ToString()).JoinToString(",");
                var columnDecimals = settingForDialog.Select(col => col.Decimals.ToString()).JoinToString(",");
                if (string.IsNullOrWhiteSpace(settings.SelectDialogOrderBy) == false)
                    filteredTable.DefaultView.Sort = settings.SelectDialogOrderBy;

                var dlg = new MultiSelectDialog(filteredTable, columnNames, columnHeaders, columnWidths, defaultValue.ToString(), columnDecimals);
                if (onSelectedRowDoubleClickEvents != null)
                    onSelectedRowDoubleClickEvents.ToList().ForEach(eve => dlg.CellDoubleClick += eve);

                var width = settingForDialog.Sum(setting => setting.ColumnWidth) * 11.7 + 30;
                dlg.Width = Convert.ToInt32(width).ReplaceIfSmaller(Screen.PrimaryScreen.Bounds.Width - 100);
                try
                {
                    var returnResult = dlg.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                        return null;
                    return dlg.GetSelecteds().ToArray();
                }
                finally
                {
                    if (onSelectedRowDoubleClickEvents != null)
                        onSelectedRowDoubleClickEvents.ToList().ForEach(eve => dlg.CellDoubleClick -= eve);
                }
            }
        }

        /// <summary>
        /// 用目前的CachedDataSource打開篩選視窗，給定要用來顯示的設定值，還有一開始要指定的值，視窗關閉後回傳所選的新值
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="defaultValue"></param>
        /// <param name="dataFactory"></param>
        /// <returns></returns>
        public DataRow ShowPopupDialog(PopupColumnSettings settings, object defaultValue, CacheCoreDataFilterSetting dataFactory = null)
        {
            return ShowPopupDialog(settings, defaultValue, this.CachedDataSource, dataFactory);
        }

        /// <summary>
        /// 用指定的DataTable打開篩選視窗，給定要用來顯示的設定值，還有一開始要指定的值，視窗關閉後回傳所選的新值
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="defaultValue"></param>
        /// <param name="datas"></param>
        /// <param name="dataFactory"></param>
        /// <returns></returns>
        public DataRow ShowPopupDialog(PopupColumnSettings settings, object defaultValue, DataTable datas, CacheCoreDataFilterSetting dataFactory = null)
        {
            DataTable filteredTable;
            if (dataFactory == null)
                dataFactory = CacheCoreDataFilterSetting.Dummy;
            using (filteredTable = dataFactory.GetDialogDataSource(datas))
            {
                var settingForDialog = settings.Where(sett => sett.ColumnWidth != 0);
                var columnNames = settingForDialog.Select(col => col.ColumnName).JoinToString(",");
                var columnHeaders = settingForDialog.Select(col => col.HeaderText).JoinToString(",");
                var columnWidths = settingForDialog.Select(col => col.ColumnWidth.ToString()).JoinToString(",");
                var columnDecimals = settingForDialog.Select(col => col.Decimals.ToString()).JoinToString(",");

                if (string.IsNullOrWhiteSpace(settings.SelectDialogOrderBy) == false)
                    filteredTable.DefaultView.Sort = settings.SelectDialogOrderBy;

                using (var dlg = new Sci.Win.Tools.SelectItem(filteredTable, columnNames, columnWidths, (defaultValue + ""), columnHeaders, columnDecimals))
                {
                    var width = settingForDialog.Sum(setting => setting.ColumnWidth) * 11.7 + 30;
                    dlg.Width = Convert.ToInt32(width).ReplaceIfSmaller(Screen.PrimaryScreen.Bounds.Width - 100);
                    var returnResult = dlg.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return null;
                    }
                    return dlg.GetSelecteds().FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// <para>用PK去找對應的DataRow，通常用在textbox被使用者輸入完成，觸動TextBox_Validation的時候會呼叫</para>
        /// </summary>
        /// <param name="dataFactory"></param>
        /// <param name="primaryKeys"></param>
        /// <returns></returns>
        public DataRow FindRowByValidatingSetting(CacheCoreDataFilterSetting dataFactory, params object[] primaryKeys)
        {
            if (dataFactory == null)
                return this.CachedDataSource.Rows.Find(primaryKeys);
            else
                return dataFactory.FindRowsWhenValidating(CachedDataSource, primaryKeys);
        }

        /// <summary>
        /// <para>用PK去找對應的DataRow，通常用在選擇視窗關閉後，想要抓同一個資料列的其他欄位</para>
        /// <para>或是textbox被使用者輸入完成，觸動TextBox_Validation的時候會呼叫</para>
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <returns></returns>
        public DataRow FindRowByPK(params object[] primaryKeys)
        {
            return CachedDataSource.Rows.Find(primaryKeys);
        }

        /// <summary>
        /// 以所提供字串換別的字串的強制繼承方法
        /// </summary>
        /// <param name="lookupContainer"></param>
        /// <returns></returns>
        public abstract TLookupValue Lookup(DataRow lookupContainer);

        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {
            if (this._CachedDataSource != null)
            {
                this._CachedDataSource.Dispose();
                this._CachedDataSource = null;
            }
        }
    }

    /// <summary>
    /// CustomizeCacheCore
    /// </summary>
    public class CustomizeCacheCore<TKeyValue, TLookupValue> : BaseDataCacheCore<TKeyValue, TLookupValue>
    {
        #region Properties

        /// <summary>
        /// 開窗欄位設定
        /// </summary>
        public PopupColumnSettings MyPopupColumnSettings { get; set; }

        /// <summary>
        /// 用來取回自訂Cache Table的方法
        /// </summary>
        public Func<DataTable> MyCacheLoader { get; set; }

        /// <summary>
        /// 用來將ID換成對應其他結果的方法
        /// </summary>
        public Func<DataRow, TLookupValue> MyLookup { get; set; }

        #endregion

        /// <summary>
        /// 重新抓取資料來源來當作Cache
        /// </summary>
        /// <returns></returns>
        internal protected override DataTable LoadCache()
        {
            if (MyCacheLoader == null)
                throw new NotImplementedException("MyCacheLoader is null");
            return this.MyCacheLoader();
        }

        /// <summary>
        /// 用單一主鍵值換別的欄位值
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public override TLookupValue Lookup(DataRow row)
        {
            if (MyLookup == null)
                throw new NotImplementedException("MyLookup is null");

            if (row == null)
                return default(TLookupValue);
            else
                return this.MyLookup(row);
        }
    }

    /// <summary>
    /// 讓控制項可以提供使用端設定固定值或是動態值，作為DataFilter物件的Predictor
    /// </summary>
    public abstract class CacheCoreDataFilterBase
    {
        /// <summary>
        /// 固定值，或者是由EnsureDynamicValue呼叫DynamicValue方法取回的瞬間值
        /// </summary>
        internal object Value;

        /// <summary>
        /// 取回Value的瞬間值
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Func<object> DynamicValue { get; set; }

        /// <summary>
        /// 是否為主鍵之一，如果是，在開窗篩選時候無論如何都會作為Predictor其中之一，不會略過
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPrimaryKey { get; private set; }

        /// <summary>
        /// 對應到資料列的哪一個欄位
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MappingFieldName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IComparer Comparer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isPrimaryKey"></param>
        /// <param name="mappingFieldName"></param>
        public CacheCoreDataFilterBase(bool isPrimaryKey, string mappingFieldName)
        {
            this.IsPrimaryKey = isPrimaryKey;
            this.MappingFieldName = mappingFieldName;
        }

        /// <summary>
        /// 重新呼叫一次DynamicValue所存方法(假如不是null)
        /// </summary>
        public void EnsureDynamicValue()
        {
            if (this.DynamicValue != null)
                this.Value = this.DynamicValue();
        }
    }

    /// <summary>
    /// 讓控制項可以提供使用端設定固定值或是動態值，作為DataFilter物件的Predictor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [TypeConverter(typeof(SCIControlConverter))]
    public class CacheCoreDataFilter<T> : CacheCoreDataFilterBase
    {
        /// <summary>
        /// 固定值，或者是由EnsureDynamicValue呼叫DynamicValue方法取回的瞬間值
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new T Value
        {
            get
            {
                return (T)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        /// <summary>
        /// 取得動態值的Func
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Func<T> DynamicValue
        {
            get
            {
                return base.DynamicValue as Func<T>;
            }
            set
            {
                if (value == null)
                    base.DynamicValue = null;
                else
                    base.DynamicValue = new Func<object>(() => value());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isPrimaryKey"></param>
        /// <param name="mappingFieldName"></param>
        public CacheCoreDataFilter(bool isPrimaryKey, string mappingFieldName)
            : base(isPrimaryKey, mappingFieldName)
        {
            if (default(T) is string)
                base.Comparer = (IComparer)StringComparer.OrdinalIgnoreCase;
            else
                base.Comparer = (IComparer)Comparer<T>.Default;
        }
    }

    /// <summary>
    /// 暫時當作萬用的屬性編輯器吧
    /// </summary>
    public class SCIControlConverter : System.ComponentModel.TypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(
                TypeDescriptor.GetProperties(typeof(CacheCoreDataFilter<string>)).Cast<PropertyDescriptor>().Where(item => item.IsBrowsable == true).ToArray());
        }
    }

    #endregion

    /// <summary>
    /// 延伸方法
    /// </summary>
    public static class Extension
    {
        internal static TReturnValue Lookup<TReturnValue>(this DataRow lookupRow, string returnFieldName)
        {
            if (lookupRow == null)
                return default(TReturnValue);
            if (string.IsNullOrEmpty(lookupRow.Table.TableName))
                throw new ArgumentException("CachedDataSource don't have tablename");
            if (lookupRow.Table.PrimaryKey == null || lookupRow.Table.PrimaryKey.Length == 0)
                throw new ArgumentException("CachedDataSource don't have primarykey setting");

            var tableName = lookupRow.Table.TableName;
            var idFieldsName = lookupRow.Table.PrimaryKey.Select(col => col.ColumnName).ToArray();
            var parameters = new List<System.Data.SqlClient.SqlParameter>();
            var sql = "Select " + returnFieldName + " from " + tableName + " Where " +
                string.Join(" and ", idFieldsName.Select((pkFieldName, idx) =>
                {
                    parameters.Add(new System.Data.SqlClient.SqlParameter("p" + idx.ToString(), lookupRow[pkFieldName]));
                    return pkFieldName + " = @p" + idx.ToString();
                }).ToArray());

            object scalar;
            if (SQL.ExecuteScalar("", sql, out scalar, parameters) == true)
                return (TReturnValue)scalar;
            else
                return default(TReturnValue);
        }

        internal static T FindForm<T>(this Control ctl) where T : Form
        {
            return (T)ctl.FindForm();
        }

        /// <summary>
        /// 比較目標文字，可以自己決定要不要IgnorCase
        /// </summary>
        /// <param name="source"></param>
        /// <param name="strB"></param>
        /// <param name="ignorCase"></param>
        /// <returns></returns>
        public static int CompareTo(this string source, string strB, bool ignorCase)
        {
            return string.Compare(source, strB, ignorCase);
        }

        /// <summary>
        /// 轉呼叫string.IsNullOrWhiteSpace(source)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// 當依照指定的方式做檢查，如果檢查結果是有資料，就把按鈕變藍色且粗體字，否則為正常黑色字體
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="checker">用於檢查有沒有資料的方法</param>
        public static void ChangeStyleByChecker(this Ict.Win.UI.Button btn, Func<bool> checker)
        {
            if (checker())
            {
                btn.Font = new Font(btn.Font, FontStyle.Bold);
                btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            }
            else
            {
                btn.Font = new Font(btn.Font, FontStyle.Regular);
                btn.ForeColor = System.Drawing.Color.Black;
            }
        }

        /// <summary>
        /// 把整列變成鵝黃色(Pending Remark Default color)
        /// </summary>
        /// <param name="gridRow"></param>
        public static void ChangePendingRemarkStyle(this DataGridViewRow gridRow)
        {
            gridRow.Cells.Cast<DataGridViewCell>().ToList().ForEach(cell => cell.Style.BackColor = VFPColor.Yellow_255_255_128);
        }

        /// <summary>
        /// 根據傳入的檢查方法來決定是否要把整列變成鵝黃色(Pending Remark Default color)
        /// </summary>
        /// <param name="gridRow"></param>
        /// <param name="checker"></param>
        public static void ChangePendingRemarkStyleByChecker(this DataGridViewRow gridRow, Func<bool> checker)
        {
            if (checker != null && checker())
                gridRow.Cells.Cast<DataGridViewCell>().ToList().ForEach(cell => cell.Style.BackColor = VFPColor.Yellow_255_255_128);
            else
                gridRow.Cells.Cast<DataGridViewCell>().ToList().ForEach(cell => cell.Style.BackColor = SystemColors.Control);
        }

        /// <summary>
        /// <para>繼承使用Field(T)，先把object轉T1，之後再轉T2，會需要做這個的原因是，隱含轉換子，不允許從object轉為其他任何類別</para>
        /// <para>ex: newRow.Field&lt;string, myClass&gt;("MR")</para>
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        //public static T2 Field<T1, T2>(this DataRow row, string columnName)
        //{
        //    return (T2)(dynamic)(T1)row[columnName];
        //}

        /// <summary>
        /// 將資料表的欄位與值拿來當作Dictionary的Key轉換成一個與資料表脫勾的字典物件供後續使用，初始目的是為了讓資料列裡面的值可以不會因為資料表被dispose而無法取用(multi-threading)
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDict(this DataRow row)
        {
            return row.Table.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row.IsNull(col.ColumnName) ? null : row[col.ColumnName]);
        }

        /// <summary>
        /// 如果資料列大於0，則會呼叫原本的CopyToDataTable，不然會用SrcTable做Clone
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="srcTableForSchema"></param>
        /// <returns></returns>
        public static DataTable TryCopyToDataTable(this IEnumerable<DataRow> rows, DataTable srcTableForSchema)
        {
            if (rows.Any())
                return rows.CopyToDataTable();
            else
                return srcTableForSchema.Clone();
        }

        /// <summary>
        /// 當發生String or binary data would be truncated錯誤時候，可用此方法觀察每個欄位的最長值是多少
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable FIndingStringTruncatedErrorInfo(this DataTable dt)
        {
            var columns = dt.Columns.Cast<DataColumn>().Select(col => new
            {
                Name = col.ColumnName,
                DataType = col.DataType,
            })
            .ToList();

            var result = new DataTable();
            result.ColumnsStringAdd("ColumnName");
            result.ColumnsStringAdd("DataTypeName");
            result.ColumnsIntAdd("MaxLength");
            columns
                .Select(colItem => new
                {
                    Name = colItem.Name,
                    DataType = colItem.DataType,
                    MaxLength = colItem.DataType.Name == "stirng" ?
                        dt.AsEnumerable().Select(row => row[colItem.Name].ToString().Length).Max() :
                        (object)DBNull.Value,
                })
                .ToList()
                .ForEach(item =>
                {
                    var newRow = result.NewRow();
                    newRow["ColumnName"] = item.Name;
                    newRow["DataTypeName"] = item.DataType.Name;
                    newRow["MaxLength"] = item.MaxLength;
                    result.Rows.Add(newRow);
                });
            return result;
        }

        /// <summary>
        /// 檢查當前物件，是否存在於檢查物件
        /// </summary>
        /// <param name="lookupValue"></param>
        /// <param name="lookupTargets"></param>
        /// <returns></returns>
        //public static bool IsOneOfThe<T>(this T lookupValue, params T[] lookupTargets)
        //{
        //    if (lookupValue is string)
        //        return IsOneOfThe((string)(dynamic)lookupValue, false, (string[])(dynamic)lookupTargets);
        //    else
        //    {
        //        return lookupTargets.Contains(lookupValue);
        //    }
        //}

        /// <summary>
        /// 檢查當前文字，是否存在於檢查目標(區分大小寫比較)
        /// </summary>
        /// <param name="lookupValue"></param>
        /// <param name="lookupTargets"></param>
        /// <returns></returns>
        public static bool IsOneOfThe(this string lookupValue, params string[] lookupTargets)
        {
            return IsOneOfThe(lookupValue, false, lookupTargets);
        }

        /// <summary>
        /// 檢查當前文字，是否存在於檢查目標(指定是否區分大小寫來比較)
        /// </summary>
        /// <param name="lookupValue"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="lookupTargets"></param>
        /// <returns></returns>
        public static bool IsOneOfThe(this string lookupValue, bool ignoreCase, params string[] lookupTargets)
        {
            if (lookupValue == null) return false;
            else if (lookupTargets == null) return false;
            else if (lookupTargets.Any() == false) return false;
            else return lookupTargets.Any(item => string.Compare(item, lookupValue, ignoreCase) == 0);
        }

        private static SqlConnection ObtainSqlConnection(Sci.Data.IDBProxy proxy)
        {
            SqlConnection cn;
            if (proxy.OpenConnection("", out cn) == false)
            {
                throw new ApplicationException("can't open Connection");
            }
            return cn;
        }

        /// <summary>
        /// <para>透過原本的Select做查詢把結果包成DualDisposableResult(DataTable)</para>
        /// <para>使用ExtendedData取出結果資料表。會把傳入的Args兩兩一組的轉為SqlParameter，所以如果傳入的Args不是偶數會跳Exeception</para>
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DualDisposableResult<DataTable> SelectEx(this Sci.Data.IDBProxy proxy, string sql, params object[] args)
        {
            return SelectEx(proxy, sql, false, args);
        }

        /// <summary>
        /// <para>透過原本的Select做查詢把結果包成DualDisposableResult(DataTable)</para>
        /// <para>使用ExtendedData取出結果資料表。會把傳入的Args兩兩一組的轉為SqlParameter，所以如果傳入的Args不是偶數會跳Exeception</para>
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="sql"></param>
        /// <param name="withSchema"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DualDisposableResult<DataTable> SelectEx(this Sci.Data.IDBProxy proxy, string sql, bool withSchema, params object[] args)
        {
            using (var cn = ObtainSqlConnection(proxy))
            {
                DualDisposableResult<DataTable> dr;
                var ps = new List<SqlParameter>();
                var argsClone = args.ToList();
                while (argsClone.Any())
                {
                    object v = argsClone[1];
                    ps.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                    argsClone = argsClone.Skip(2).ToList();
                }
                using (var adapter = new SqlDataAdapter(sql, cn))
                {
                    adapter.SelectCommand.Parameters.AddRange(ps.ToArray());

                    var dt = new DataTable();
                    try
                    {
                        adapter.Fill(dt);
                        if (withSchema)
                            adapter.FillSchema(dt, SchemaType.Source);
                        dr = new DualDisposableResult<DataTable>(new DualResult(true));
                        dr.ExtendedData = dt;
                    }
                    catch (Exception ex)
                    {
                        var mixEx = new AggregateException(ex.Message + "\r\nsql: " + sql, ex);
                        dr = new DualDisposableResult<DataTable>(new DualResult(false, mixEx));
                    }
                }
                return dr;
            }
        }

        /// <summary>
        /// 可以一次撈多個Table放在DataSet內回傳(透過ExtendData屬性)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxy"></param>
        /// <param name="sql"></param>
        /// <param name="withSchema"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DualDisposableResult<T> SelectEx<T>(this Sci.Data.IDBProxy proxy, string sql, bool withSchema, params object[] args) where T : DataSet, new()
        {
            using (var cn = ObtainSqlConnection(proxy))
            {
                DualDisposableResult<T> dr;
                var ps = new List<SqlParameter>();
                var argsClone = args.ToList();
                while (argsClone.Any())
                {
                    object v = argsClone[1];
                    ps.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                    argsClone = argsClone.Skip(2).ToList();
                }
                using (var adapter = new SqlDataAdapter(sql, cn))
                {
                    adapter.SelectCommand.Parameters.AddRange(ps.ToArray());

                    var tInstance = new T();
                    try
                    {
                        adapter.Fill(tInstance);
                        if (withSchema)
                            adapter.FillSchema(tInstance, SchemaType.Source);
                        dr = new DualDisposableResult<T>(new DualResult(true));
                        dr.ExtendedData = tInstance;
                    }
                    catch (Exception ex)
                    {
                        var mixEx = new AggregateException(ex.Message + "\r\nsql: " + sql, ex);
                        dr = new DualDisposableResult<T>(new DualResult(false, mixEx));
                    }
                }
                return dr;
            }
        }

        /// <summary>
        /// 將控制項的Text屬性回傳作為SqlParameter.SqlValue使用，如果是空字串或是null，就用newValue代替回傳，如果newValue是null就用DBNull.Value代替回傳
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="newValue"></param>
        /// <param name="trimBeforeCompare"></param>
        /// <returns></returns>
        public static object GetDBParameterValue(this System.Windows.Forms.TextBoxBase txt, object newValue = null, bool trimBeforeCompare = true)
        {
            if (txt == null)
                return newValue ?? DBNull.Value;
            if (trimBeforeCompare)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                    return newValue ?? DBNull.Value;
                else
                    return txt.Text.Trim();
            }
            else
            {
                if (string.IsNullOrEmpty(txt.Text))
                    return newValue ?? DBNull.Value;
                else
                    return txt.Text.Trim();
            }
        }

        /// <summary>
        /// 將控制項的Text屬性回傳作為SqlParameter.SqlValue使用，如果是空字串或是null，就用newValue代替回傳，如果newValue是null就用DBNull.Value代替回傳
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="newValue"></param>
        /// <param name="trimBeforeCompare"></param>
        /// <returns></returns>
        public static object GetDBParameterValue_Text(this System.Windows.Forms.ComboBox cbx, object newValue = null, bool trimBeforeCompare = true)
        {
            if (cbx == null)
                return newValue ?? DBNull.Value;
            if (trimBeforeCompare)
            {
                if (string.IsNullOrWhiteSpace(cbx.Text))
                    return newValue ?? DBNull.Value;
                else
                    return cbx.Text.Trim();
            }
            else
            {
                if (string.IsNullOrEmpty(cbx.Text))
                    return newValue ?? DBNull.Value;
                else
                    return cbx.Text.Trim();
            }
        }

        /// <summary>
        /// 將控制項的SelectedValue2屬性回傳作為SqlParameter.SqlValue使用，如果是空字串或是null，就用newValue代替回傳，如果newValue是null就用DBNull.Value代替回傳
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="newValue"></param>
        /// <param name="trimBeforeCompare"></param>
        /// <returns></returns>
        public static object GetDBParameterValue_SelectedValue2(this Ict.Win.UI.ComboBox cbx, object newValue = null, bool trimBeforeCompare = true)
        {
            if (cbx == null)
                return newValue ?? DBNull.Value;
            if (cbx.SelectedValue2 == null)
                return newValue ?? DBNull.Value;
            return newValue ?? DBNull.Value;
        }

        /// <summary>
        /// <para>透過原本的Lookup做查詢把結果包成DualResult(T)</para>
        /// <para>使用ExtendedData取出結果值。會把傳入的Args兩兩一組的轉為SqlParameter，所以如果傳入的Args不是偶數會跳Exeception</para>
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DualResult<T> LookupEx<T>(this Sci.Data.IDBProxy proxy, string sql, params object[] args)
        {
            DualResult<T> dr;
            DataTable dt;
            var ps = new List<SqlParameter>();
            var argsClone = args.ToList();
            while (argsClone.Any())
            {
                object v = argsClone[1];
                ps.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                argsClone = argsClone.Skip(2).ToList();
            }
            dr = new DualResult<T>(proxy.Select(null, sql, ps, out dt));
            if (dr == true)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0].ItemArray.Length > 0)
                    {
                        var value = dt.Rows[0][0];
                        if (value is T)
                            dr.ExtendedData = (T)value;
                        else
                            dr.ExtendedData = default(T);
                    }
                }
            }
            return dr;
        }

        /// <summary>
        /// <para>透過原本的Execute做查詢把結果包成DualResult</para>
        /// <para>會把傳入的Args兩兩一組的轉為SqlParameter，所以如果傳入的Args不是偶數會跳Exeception</para>
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DualResult ExecuteEx(this Sci.Data.IDBProxy proxy, string sql, params object[] args)
        {
            var ps = new List<SqlParameter>();
            var argsClone = args.ToList();
            while (argsClone.Any())
            {
                object v = argsClone[1];
                ps.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                argsClone = argsClone.Skip(2).ToList();
            }
            return new DualResult(proxy.Execute(null, sql, ps));
        }

        /// <summary>
        /// 使用原本的DataTable.AsEnumerable方法，但是加入可以用RowState來篩選的能力，也可以是反向篩選(主要拿來篩選非Delete的DataRow，因為實在是太常用了)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static EnumerableRowCollection<DataRow> AsEnumerable(this DataTable source, DataRowState filter, bool reverse = false)
        {
            return source.AsEnumerable()
                .Where(row =>
                {
                    var result = (row.RowState & filter) == row.RowState;
                    if (reverse)
                        return !result;
                    else
                        return result;
                });
        }

        /// <summary>
        /// 將資料列完整複製，主要是因為在多執行續當中，有機會出現DataTable被主執行續(或其他執行續給Dispose了，而導致別的執行續無法順利取值
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static DataRow GetDetachCopy(this DataRow row)
        {
            if (row == null)
                return null;
            else
            {
                lock (row.Table)
                {
                    var newRow = row.Table.NewRow();
                    newRow.ItemArray = row.ItemArray;
                    return newRow;
                }
            }
        }

        /// <summary>
        /// Set all column's SortMode to DataGridViewColumnSortMode.NotSortable
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static DataGridViewColumnCollection DisableSortable(this DataGridViewColumnCollection cols)
        {
            cols.Cast<DataGridViewColumn>().ToList().ForEach(col => col.SortMode = DataGridViewColumnSortMode.NotSortable);
            return cols;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static DataGridViewColumnCollection ReadOnly(this DataGridViewColumnCollection cols)
        {
            cols.Cast<DataGridViewColumn>().ToList().ForEach(col => col.ReadOnly = true);
            return cols;
        }

        /// <summary>
        /// 更新目前Grid內正在編輯的那一格(請記得先ValidateControl)
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static Ict.Win.UI.DataGridView InvalidCurrentRow(this Ict.Win.UI.DataGridView grid)
        {
            if (grid.CurrentCell == null)
                return grid;

            grid.InvalidateCell(grid.CurrentCell.ColumnIndex, grid.CurrentCell.RowIndex);
            return grid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static DataRow GetCurrentDataRow(this Ict.Win.UI.DataGridView grid)
        {
            if (grid.SelectedRows.Count == 0)
                return null;
            else
                return grid.GetDataRow(grid.SelectedRows[0].Index);
        }

        /// <summary>
        /// 用指定的分隔符號，透過string.Join方法將各項目串連起來做回傳
        /// </summary>
        /// <param name="items"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string JoinToString(this IEnumerable<string> items, string delimiter)
        {
            return string.Join(delimiter, items.ToArray());
        }

        /// <summary>
        /// 合併DateTime.ToString()的功能
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToStringEx(this DateTime? date, string format)
        {
            if (date.HasValue)
                return date.Value.ToString(format);
            else
                return null;
        }

        /// <summary>
        /// 用MsgGrid元件開啟，還會設定Header的BorderStyle
        /// </summary>
        /// <param name="table"></param>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        //public static DialogResult ShowMsgGrid(this DataTable table, string msg, string caption, params string[] columns)
        //{
        //    using (var dlg = new Win.UI.MsgGridForm(table
        //        , msg
        //        , caption
        //        , columns.JoinToString(",")))
        //    {
        //        UIClassPrg.SetGrid_HeaderBorderStyle(dlg.grid1);
        //        return dlg.ShowDialog();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<DataRow> ExtNotDeletedRows(this DataTable table)
        {
            return table
                .AsEnumerable()
                .Where(row => row.RowState != DataRowState.Deleted);
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆執行任務
        /// </summary>
        /// <param name="table"></param>
        /// <param name="job"></param>
        public static void ExtNotDeletedRowsForeach(this DataTable table, Action<DataRow> job)
        {
            ExtNotDeletedRows(table)
                .ToList()
                .ForEach(job);
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆執行任務(帶有序號)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="job"></param>
        public static void ExtNotDeletedRowsForeach(this DataTable table, Action<DataRow, int> job)
        {
            ExtNotDeletedRows(table)
                .Select((row, idx) =>
                {
                    job(row, idx);
                    return true;
                }).ToList();
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆轉換
        /// </summary>
        /// <param name="table"></param>
        /// <param name="job"></param>
        public static IEnumerable<T> ExtNotDeletedRowsSelect<T>(this DataTable table, Func<DataRow, T> job)
        {
            return ExtNotDeletedRows(table)
                .Select(job);
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆轉換(帶有序號)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="job"></param>
        public static IEnumerable<T> ExtNotDeletedRowsSelect<T>(this DataTable table, Func<DataRow, int, T> job)
        {
            return ExtNotDeletedRows(table)
                .Select(job);
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆轉換
        /// </summary>
        /// <param name="table"></param>
        /// <param name="job"></param>
        public static List<T> ExtNotDeletedRowsToList<T>(this DataTable table, Func<DataRow, T> job)
        {
            return ExtNotDeletedRowsSelect(table, job).ToList();
        }

        /// <summary>
        /// 常常用到：還沒有刪除的資料列，逐筆轉換(帶有序號)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="job"></param>
        public static List<T> ExtNotDeletedRowsToList<T>(this DataTable table, Func<DataRow, int, T> job)
        {
            return ExtNotDeletedRowsSelect(table, job).ToList();
        }
    }

    /// <summary>
    /// 純值的延伸方法
    /// </summary>
    public static class PrimitiveValueExtension
    {
        /// <summary>
        /// 加上一個數字
        /// </summary>
        /// <param name="value"></param>
        /// <param name="plusValue"></param>
        /// <returns></returns>
        public static int Plus(this int value, int plusValue)
        {
            return value + plusValue;
        }
        /// <summary>
        /// 加上一個數字
        /// </summary>
        /// <param name="value"></param>
        /// <param name="plusValue"></param>
        /// <returns></returns>
        public static float Plus(this float value, float plusValue)
        {
            return value + plusValue;
        }
        /// <summary>
        /// 加上一個數字
        /// </summary>
        /// <param name="value"></param>
        /// <param name="plusValue"></param>
        /// <returns></returns>
        public static long Plus(this long value, long plusValue)
        {
            return value + plusValue;
        }
        /// <summary>
        /// 加上一個數字
        /// </summary>
        /// <param name="value"></param>
        /// <param name="plusValue"></param>
        /// <returns></returns>
        public static double Plus(this double value, double plusValue)
        {
            return value + plusValue;
        }
        /// <summary>
        /// 加上一個數字
        /// </summary>
        /// <param name="value"></param>
        /// <param name="plusValue"></param>
        /// <returns></returns>
        public static decimal Plus(this decimal value, decimal plusValue)
        {
            return value + plusValue;
        }
        /// <summary>
        /// 文字轉數字
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int ConvertToInt32(this string text)
        {
            return Convert.ToInt32(text);
        }
        /// <summary>
        /// 如果目標比較值小於自己，則以目標值取代自己
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="compareValue"></param>
        /// <returns></returns>
        public static T ReplaceIfSmaller<T>(this T value, T compareValue) where T : IComparable
        {
            if (value == null)
                return compareValue;
            else
                return value.CompareTo(compareValue) > 0 ? compareValue : value;
        }
        /// <summary>
        /// 如果目標比較值大於自己，則以目標值取代自己
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="compareValue"></param>
        /// <returns></returns>
        public static T ReplaceIfGreater<T>(this T value, T compareValue) where T : IComparable
        {
            if (value == null)
                return compareValue;
            else
                return value.CompareTo(compareValue) < 0 ? compareValue : value;
        }
    }

    /// <summary>
    /// 延伸方法，內涵轉接器，讓Cell的建立更簡單
    /// </summary>
    public static class GridColumnSettingExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="propertyname"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="setting"></param>
        /// <param name="alignment"></param>
        /// <param name="characterCasing"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_MDivision(this IDataGridViewGenerator generator, string propertyname, string header = null, string name = null, IWidth width = null, bool? iseditable = null, PopupColumnSettings setting = null, DataGridViewContentAlignment? alignment = null, CharacterCasing? characterCasing = null)
        {
            //因為是text的cell，所以只看editable，不用管iseditingeditable
            if (setting == null)
                return generator.Text(propertyname, header, name, width, new CustomizeControl.MDivision.GridCellSettingMDivision(propertyname), iseditable, null, alignment);
            else
                return generator.Text(propertyname, header, name, width, new CustomizeControl.MDivision.GridCellSettingMDivision(propertyname, setting), iseditable, null, alignment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="propertyname"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="setting"></param>
        /// <param name="alignment"></param>
        /// <param name="characterCasing"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_CDCode(this IDataGridViewGenerator generator, string propertyname, string header = null, string name = null, IWidth width = null, bool? iseditable = null, PopupColumnSettings setting = null, DataGridViewContentAlignment? alignment = null, CharacterCasing? characterCasing = null)
        {
            //因為是text的cell，所以只看editable，不用管iseditingeditable
            if (setting == null)
                return generator.Text(propertyname, header, name, width, new CustomizeControl.CDCode.GridCellSettingCDCode(propertyname), iseditable, null, alignment);
            else
                return generator.Text(propertyname, header, name, width, new CustomizeControl.CDCode.GridCellSettingCDCode(propertyname, setting), iseditable, null, alignment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="propertyname"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="setting"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_Keyword(this IDataGridViewGenerator generator, string propertyname, string header = null, string name = null, IWidth width = null, bool? iseditable = null, PopupColumnSettings setting = null, DataGridViewContentAlignment? alignment = null)
        {
            //因為是text的cell，所以只看editable，不用管iseditingeditable
            Ict.Win.UI.DataGridViewTextBoxColumn col = null;
            try
            {
                if (setting == null)
                    return generator.Text(propertyname, header, name, width, new CustomizeControl.Keyword.GridCellSettingKeyword(propertyname), iseditable, null, alignment).Get(out col);
                else
                    return generator.Text(propertyname, header, name, width, new CustomizeControl.Keyword.GridCellSettingKeyword(propertyname, setting), iseditable, null, alignment).Get(out col);
            }
            finally
            {
                //Keyword 要分大小寫
                col.CharacterCasing = CharacterCasing.Normal;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="propertyname"></param>
        /// <param name="header"></param>
        /// <param name="mmsType"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="setting"></param>
        /// <param name="alignment"></param>
        /// <param name="cellValidatingCompleted"></param>
        /// <param name="editingControlMouseDownCompleted"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_MmsBrand(this IDataGridViewGenerator generator, string propertyname, string header = null, string mmsType = null, string name = null, IWidth width = null, bool? iseditable = null, PopupColumnSettings setting = null, DataGridViewContentAlignment? alignment = null,
            CachedBondleGridCellEventHandler<string, string> cellValidatingCompleted = null,
            CachedBondleGridCellEventHandler<string, string> editingControlMouseDownCompleted = null)
        {
            //因為是text的cell，所以只看editable，不用管iseditingeditable
            Ict.Win.UI.DataGridViewTextBoxColumn col = null;
            var _setting = (CustomizeControl.MmsBrand.GridCellSettingMmsBrand)null;
            if (setting == null)
                _setting = new CustomizeControl.MmsBrand.GridCellSettingMmsBrand(propertyname, mmsType);
            else
                _setting = new CustomizeControl.MmsBrand.GridCellSettingMmsBrand(propertyname, mmsType, setting);

            if (editingControlMouseDownCompleted != null)
                _setting.EditingMouseDownCompleted += editingControlMouseDownCompleted;
            if (cellValidatingCompleted != null)
                _setting.CellValidatingComleted += cellValidatingCompleted;
            generator.Text(propertyname, header, name, width, _setting, iseditable, null, alignment).Get(out col);

            //col.CharacterCasing = CharacterCasing.Normal;

            return generator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="propertyname"></param>
        /// <param name="mtlFactorType"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="setting"></param>
        /// <param name="alignment"></param>
        /// <param name="cellValidatingCompleted"></param>
        /// <param name="editingControlMouseDownCompleted"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_MtlFactor(this IDataGridViewGenerator generator, string propertyname, string mtlFactorType, string header = null, string name = null, IWidth width = null, bool? iseditable = null, PopupColumnSettings setting = null, DataGridViewContentAlignment? alignment = null,
            CachedBondleGridCellEventHandler<string, string> cellValidatingCompleted = null,
            CachedBondleGridCellEventHandler<string, string> editingControlMouseDownCompleted = null)
        {
            //因為是text的cell，所以只看editable，不用管iseditingeditable
            Ict.Win.UI.DataGridViewTextBoxColumn col = null;
            var _setting = (CustomizeControl.MtlFactor.GridCellSettingMtlFactor)null;
            if (setting == null)
                _setting = new CustomizeControl.MtlFactor.GridCellSettingMtlFactor(propertyname, mtlFactorType);
            else
                _setting = new CustomizeControl.MtlFactor.GridCellSettingMtlFactor(propertyname, mtlFactorType, setting);

            if (editingControlMouseDownCompleted != null)
                _setting.EditingMouseDownCompleted += editingControlMouseDownCompleted;
            if (cellValidatingCompleted != null)
                _setting.CellValidatingComleted += cellValidatingCompleted;
            generator.Text(propertyname, header, name, width, _setting, iseditable, null, alignment).Get(out col);

            col.CharacterCasing = CharacterCasing.Normal;

            return generator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="propertyname"></param>
        /// <param name="reasonType"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="setting"></param>
        /// <param name="alignment"></param>
        /// <param name="cellValidatingCompleted"></param>
        /// <param name="editingControlMouseDownCompleted"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_InvtransReason(this IDataGridViewGenerator generator, string propertyname, CustomizeControl.InvtransReason.InvtransReasonTypeEnum? reasonType, string header = null, string name = null, IWidth width = null, bool? iseditable = null, PopupColumnSettings setting = null, DataGridViewContentAlignment? alignment = null,
            CachedBondleGridCellEventHandler<string, string> cellValidatingCompleted = null,
            CachedBondleGridCellEventHandler<string, string> editingControlMouseDownCompleted = null)
        {
            //因為是text的cell，所以只看editable，不用管iseditingeditable
            Ict.Win.UI.DataGridViewTextBoxColumn col = null;
            var _setting = (CustomizeControl.InvtransReason.GridCellSettingInvtransReason)null;
            if (setting == null)
                _setting = new CustomizeControl.InvtransReason.GridCellSettingInvtransReason(propertyname, reasonType);
            else
                _setting = new CustomizeControl.InvtransReason.GridCellSettingInvtransReason(propertyname, reasonType, setting);

            if (editingControlMouseDownCompleted != null)
                _setting.EditingMouseDownCompleted += editingControlMouseDownCompleted;
            if (cellValidatingCompleted != null)
                _setting.CellValidatingComleted += cellValidatingCompleted;
            generator.Text(propertyname, header, name, width, _setting, iseditable, null, alignment).Get(out col);

            col.CharacterCasing = CharacterCasing.Normal;

            return generator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="propertyname"></param>
        /// <param name="header"></param>
        /// <param name="countryId"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="setting"></param>
        /// <param name="alignment"></param>
        /// <param name="cellValidatingCompleted"></param>
        /// <param name="editingControlMouseDownCompleted"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_Factory(this IDataGridViewGenerator generator,
            string propertyname, string header = null, string countryId = null, string name = null,
            IWidth width = null, bool? iseditable = null, PopupColumnSettings setting = null,
            DataGridViewContentAlignment? alignment = null,
            CachedBondleGridCellEventHandler<string, string> cellValidatingCompleted = null,
            CachedBondleGridCellEventHandler<string, string> editingControlMouseDownCompleted = null)
        {
            //因為是text的cell，所以只看iseditingeditable，不用管editable
            Ict.Win.UI.DataGridViewTextBoxColumn col = null;
            var _setting = (CustomizeControl.Factory.GridCellSettingFactory)null;
            if (setting == null)
                _setting = new CustomizeControl.Factory.GridCellSettingFactory(propertyname, countryId);
            else
                _setting = new CustomizeControl.Factory.GridCellSettingFactory(propertyname, countryId, setting);

            _setting.OwnerGrid = generator.Control;
            if (editingControlMouseDownCompleted != null)
                _setting.EditingMouseDownCompleted += editingControlMouseDownCompleted;
            if (cellValidatingCompleted != null)
                _setting.CellValidatingComleted += cellValidatingCompleted;
            generator.Text(propertyname, header, name, width, _setting, iseditable, null, alignment).Get(out col);

            return generator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="setting"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_Color(this IDataGridViewGenerator generator,
            CustomizeControl.Color.GridCellSettingColor setting,
            string header = null, string name = null, IWidth width = null, bool? iseditable = null,
            DataGridViewContentAlignment? alignment = null)
        {
            setting.OwnerGrid = generator.Control;
            generator.Text(setting.IdColumnName, header, name: name, width: width, settings: setting, iseditable: iseditable.GetValueOrDefault(false), alignment: alignment);
            return generator;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="columnSetting"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_Project(this IDataGridViewGenerator generator, CustomizeControl.Project.GridCellSettingProject columnSetting, string header = null, string name = null, IWidth width = null, bool? iseditable = null, DataGridViewContentAlignment? alignment = null)
        {
            columnSetting.OwnerGrid = generator.Control;
            generator.Text(columnSetting.IdColumnName, header, name: name, width: width, settings: columnSetting, iseditingreadonly: !iseditable.GetValueOrDefault(false), alignment: alignment);
            return generator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="columnSetting"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_SuppContact(this IDataGridViewGenerator generator, CustomizeControl.SuppContact.GridCellSettingSuppContact columnSetting, string header = null, string name = null, IWidth width = null, bool? iseditable = null, DataGridViewContentAlignment? alignment = null)
        {
            columnSetting.OwnerGrid = generator.Control;
            generator.Text(columnSetting.IdColumnName, header, name: name, width: width, settings: columnSetting, iseditingreadonly: !iseditable.GetValueOrDefault(false), alignment: alignment);
            return generator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="propertyname"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="setting"></param>
        /// <param name="alignment"></param>
        /// <param name="characterCasing"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_PatternAnnotationArtwork(this IDataGridViewGenerator generator, string propertyname, string header = null, string name = null, IWidth width = null, bool? iseditable = null, PopupColumnSettings setting = null, DataGridViewContentAlignment? alignment = null, CharacterCasing? characterCasing = null)
        {
            //因為是text的cell，所以只看editable，不用管iseditingeditable
            if (setting == null)
                return generator.Text(propertyname, header, name, width, new CustomizeControl.PatternAnnotationArtwork.GridCellSettingPatternAnnotationArtwork(propertyname), iseditable, null, alignment);
            else
                return generator.Text(propertyname, header, name, width, new CustomizeControl.PatternAnnotationArtwork.GridCellSettingPatternAnnotationArtwork(propertyname, setting), iseditable, null, alignment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="setting"></param>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="iseditable"></param>
        /// <param name="alignment"></param>
        /// <param name="characterCasing"></param>
        /// <returns></returns>
        public static IDataGridViewGenerator Text_Reason(this IDataGridViewGenerator generator, CustomizeControl.Reason.GridCellSettingReason setting = null
            , string header = null, string name = null, IWidth width = null, bool? iseditable = null, DataGridViewContentAlignment? alignment = null, CharacterCasing? characterCasing = null)
        {
            setting.OwnerGrid = generator.Control;
            generator.Text(setting.IdColumnName, header, name: name, width: width, settings: setting, iseditingreadonly: !iseditable.GetValueOrDefault(false), alignment: alignment);
            return generator;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// 依照分組數來切分來源內容，如果除不盡，會將餘數分攤給較前方的子群組
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="groupCount"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> GroupByGroupNumber<T>(this IEnumerable<T> source, int groupNumber)
        {
            if (source == null)
                return Enumerable.Empty<List<T>>();

            var totalCount = source.Count();
            if (totalCount == 0)
                return Enumerable.Empty<List<T>>();

            var subGroupSize = Convert.ToInt32(totalCount / groupNumber);
            var missingGroupCount = totalCount - (subGroupSize * groupNumber);
            if (missingGroupCount != 0)
                subGroupSize++;

            return GroupByChunkSize(source, subGroupSize);
        }

        /// <summary>
        /// 依照指定的數量去切分來源內容，不論切分後餘數是多少，只要大於0，就算是1，也會形成下一組
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> GroupByChunkSize<T>(this IEnumerable<T> source, int chunkSize)
        {
            while (source.Any())
            {
                yield return source.TakeWhile((a, idx) => idx < chunkSize).ToList();
                source = source.Skip(chunkSize);
            }
        }
    }

    #region Simple Helper

    /// <summary>
    /// 針對單一筆使用者，延伸抓取對應的欄位，通常用於非常零星且單一的片段資料取得，本物件請勿用於大量迴圈中(效能不佳)
    /// </summary>
    public class UserHelper : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// 針對單一筆使用者，延伸抓取對應的欄位，通常用於非常零星且單一的片段資料取得，本物件請勿用於大量迴圈中(效能不佳)
        /// </summary>
        /// <param name="userID"></param>
        public UserHelper(object userID)
        {
            if (userID == null || userID == DBNull.Value)
                this.UserID = null;
            else if (userID is string)
                this.UserID = (string)userID;
            else
                this.UserID = null;
        }

        private string _Email;
        /// <summary>
        /// 使用者的Email
        /// </summary>
        public string Email
        {
            get
            {
                if (_Email == null)
                {
                    if (string.IsNullOrEmpty(this.UserID))
                        return null;
                    _Email = DBProxy.Current.LookupEx<string>(
                    @"Select email From pass1 Where id = @id",
                    "id", this.UserID).ExtendedData ?? string.Empty;
                }
                return _Email;
            }
        }

        /// <summary>
        /// 字串與UserHelper的隱含轉換
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static explicit operator UserHelper(string id)
        {
            return new UserHelper(id);
        }

        /// <summary>
        /// UserHelper與字串的隱含轉換
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static implicit operator string(UserHelper helper)
        {
            return helper.UserID;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
    }

    /// <summary>
    /// 針對單一筆工廠，延伸抓取對應的欄位，通常用於非常零星且單一的片段資料取得，本物件請勿用於大量迴圈中(效能不佳)
    /// </summary>
    public class FactoryHelper : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public string FactoryID { get; private set; }

        /// <summary>
        /// 針對單一筆工廠，延伸抓取對應的欄位，通常用於非常零星且單一的片段資料取得，本物件請勿用於大量迴圈中(效能不佳)
        /// </summary>
        /// <param name="factoryID"></param>
        public FactoryHelper(object factoryID)
        {
            if (factoryID == null || factoryID == DBNull.Value)
                this.FactoryID = null;
            else if (factoryID is string)
                this.FactoryID = (string)factoryID;
            else
                this.FactoryID = null;
        }

        //private FactoryPrg.MailAddressInfo _PatternMarkerRegionMailInfo;
        /// <summary>
        /// Pattern/Marker的區域主管信箱資訊
        /// </summary>
        //public FactoryPrg.MailAddressInfo PatternMarkerRegionMailInfo
        //{
        //    get
        //    {
        //        if (this._PatternMarkerRegionMailInfo == null)
        //        {
        //            if (string.IsNullOrWhiteSpace(this.FactoryID))
        //                return null;
        //            this._PatternMarkerRegionMailInfo = FactoryPrg.GetFacRegionEmail(this.FactoryID, "P");
        //        }

        //        return this._PatternMarkerRegionMailInfo;
        //    }
        //}

        //private FactoryPrg.MailAddressInfo _IeRegionMailInfo;
        /// <summary>
        /// IE的區域主管信箱資訊
        /// </summary>
        //public FactoryPrg.MailAddressInfo IeRegionMailInfo
        //{
        //    get
        //    {
        //        if (this._IeRegionMailInfo == null)
        //        {
        //            if (string.IsNullOrWhiteSpace(this.FactoryID))
        //                return null;
        //            this._IeRegionMailInfo = FactoryPrg.GetFacRegionEmail(this.FactoryID, "I");
        //        }

        //        return this._IeRegionMailInfo;
        //    }
        //}

        /// <summary>
        /// 字串與FactoryHelper的隱含轉換
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static explicit operator FactoryHelper(string id)
        {
            return new FactoryHelper(id);
        }

        /// <summary>
        /// FactoryHelper與字串的隱含轉換
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static implicit operator string(FactoryHelper helper)
        {
            return helper.FactoryID;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
    }

    #endregion

    /// <summary>
    /// 延伸資料表(會跟主資料來源物件連動)提供LazyLoad能力
    /// </summary>
    public class ExtendedDataRetriver
    {
        /// <summary>
        /// 當呼叫DataLoader發生錯誤的時候觸發
        /// </summary>
        public event EventHandler LoadFail;
        /// <summary>
        /// 最後一次載入資料時候發生的錯誤
        /// </summary>
        public Exception LastException { get; private set; }
        /// <summary>
        /// 主畫面關聯的BindingSource，會在PositionChanged事件中自動Reset目前的Data
        /// </summary>
        public System.Windows.Forms.BindingSource BS { get; private set; }
        /// <summary>
        /// 外部決定的方法，用於把目前BindingSource裡面正指向的資料列，去查找延伸資料表
        /// </summary>
        public Func<DataRow, DataTable> DataLoader { get; set; }
        /// <summary>
        /// 目前的Data是否為已經呼叫過DataLoader的結果
        /// </summary>
        public bool DataLoaded { get; private set; }
        /// <summary>
        /// 用來讓不同執行續可以同時嘗試取用同一個ExtendDataRetriver而不會有重複LoadData的情況
        /// </summary>
        private object ThreadSafeLocker = new object();
        private DataTable _Data = null;
        /// <summary>
        /// 延伸資料表
        /// </summary>
        public DataTable Data
        {
            get
            {
                lock (ThreadSafeLocker)
                {
                    if (_Data == null && this.DataLoaded == false)
                    {
                        this.DataLoaded = true;
                        if (this.DataLoader == null)
                            return null;
                        try
                        {
                            _Data = this.DataLoader((DataRow)((DataRowView)BS.Current).Row);
                        }
                        catch (Exception ex)
                        {
                            this.LastException = ex;
                            if (this.LoadFail != null)
                                this.LoadFail(this, EventArgs.Empty);
                            return null;
                        }
                    }
                }
                return _Data;
            }
        }
        /// <summary>
        /// 第一列，因為高比例的延伸資料，都只是查出一筆，所以做這個屬性來當Warpper，如果沒有資料列在Data內，會回傳NewRow()結果
        /// </summary>
        public DataRow FirstRow
        {
            get
            {
                lock (this.ThreadSafeLocker)
                {
                    if (this.Data == null)
                        return null;
                    else
                        return (this.Data.Rows.FirstOrDefault<DataRow>() ?? this.Data.NewRow());
                }
            }
        }
        /// <summary>
        /// 判斷是否具有資料列
        /// </summary>
        public bool HasRecord
        {
            get
            {
                lock (this.ThreadSafeLocker)
                {
                    return this.Data != null && this.Data.Rows.Count > 0;
                }
            }
        }

        //public ExtendedDataRetriver(Sci.Win.Tems.Base form, Ict.Win.UI.ListControlBindingSource bs, Func<DataRow, DataTable> dataLoader)
        /// <summary>
        /// 延伸資料表(會跟主資料來源物件連動)提供LazyLoad能力
        /// </summary>
        /// <param name="form"></param>
        /// <param name="bs"></param>
        /// <param name="dataLoader"></param>
        public ExtendedDataRetriver(Sci.Win.Tems.Base form, System.Windows.Forms.BindingSource bs, Func<DataRow, DataTable> dataLoader)
        {
            this.BS = bs;
            this.DataLoader = dataLoader;
            this.DataLoaded = false;
            form.FormClosed += form_FormClosed;
            this.BS.PositionChanged += BS_PositionChanged;
        }

        private void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Reset();
            this.LastException = null;
            this.DataLoader = null;
        }

        private void BS_PositionChanged(object sender, EventArgs e)
        {
            this.Reset();
        }

        /// <summary>
        /// 重設延伸資料表，下一次叫用Data屬性時候會呼叫DataLoader重抓資料
        /// </summary>
        public void Reset()
        {
            lock (ThreadSafeLocker)
            {
                if (this._Data != null)
                {
                    this._Data.Dispose();
                    this._Data = null;
                }
                this.DataLoaded = false;
            }
        }
    }

    /// <summary>
    /// 將一群DataRetriver集中控管，理論上會需要Reset的時機點是相同的
    /// </summary>
    public class ExtendedDataRetriverController : ExtendedDataRetriverController<string>
    {
        /// <summary>
        /// ExtendedDataRetriver 群，以Form為單位集中控管
        /// </summary>
        /// <param name="form"></param>
        /// <param name="bs"></param>
        public ExtendedDataRetriverController(Sci.Win.Tems.Base form, Ict.Win.UI.ListControlBindingSource bs)
            : base(form, bs)
        {
        }
    }

    /// <summary>
    /// ExtendedDataRetriver 群，以Form為單位集中控管
    /// </summary>
    public class ExtendedDataRetriverController<TKey> : IReadOnlyDictionary<TKey, ExtendedDataRetriver>
    {
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<TKey, ExtendedDataRetriver> Items { get; set; }

        /// <summary>
        /// 主畫面關聯的BindingSource，會在PositionChanged事件中自動Reset目前的Data
        /// </summary>
        public Ict.Win.UI.ListControlBindingSource BS { get; protected set; }

        /// <summary>
        /// 主畫面
        /// </summary>
        public Sci.Win.Tems.Base Form { get; set; }

        /// <summary>
        /// ExtendedDataRetriver 群，以Form為單位集中控管
        /// </summary>
        /// <param name="form"></param>
        /// <param name="bs"></param>
        public ExtendedDataRetriverController(Sci.Win.Tems.Base form, Ict.Win.UI.ListControlBindingSource bs)
        {
            this.Form = form;
            this.BS = bs;
            this.Items = new Dictionary<TKey, ExtendedDataRetriver>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retriveTableMethod"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ExtendedDataRetriverController<TKey> AddExtendedRetriver(TKey name, Func<DataRow, DataTable> retriveTableMethod)
        {
            this.Items.Add(name, new ExtendedDataRetriver(this.Form, this.BS, retriveTableMethod));
            return this;
        }

        /// <summary>
        /// 把一些需要Reset所有DataRetriver的事件給近來(幫我盧Roger，請他加一個DataSavePost的Event給我用吧～)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whenRaiseThenResetAll"></param>
        public void AddResetTimeing<T>(EventHandler<T> whenRaiseThenResetAll)
        {
            whenRaiseThenResetAll += (sender, e) =>
            {
                this.Items.Values.ToList().ForEach(item => item.Reset());
            };
        }

        #region IReadOnlyDictionary<string, ExtendedDataRetriver>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return this.Items.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get { return this.Items.Keys; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out ExtendedDataRetriver value)
        {
            return this.Items.TryGetValue(key, out value);
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ExtendedDataRetriver> Values
        {
            get { return this.Items.Values; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ExtendedDataRetriver this[TKey key]
        {
            get { return this.Items[key]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return this.Items.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, ExtendedDataRetriver>> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)this.Items).GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// 合併原本的DualResult物件，並且讓裡面含有一個ExtendedData屬性來放置要回傳的主要結果(該結果必須有實作IDisposable)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DualDisposableResult<T> : IDisposable where T : class, IDisposable
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
        /// <param name="innerResult"></param>
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
        /// <returns></returns>
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
        /// <returns></returns>
        public BaseResult.MessageInfos ToMessages()
        {
            return this.InnerResult.ToMessages();
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns></returns>
        public string ToSimpleString()
        {
            return this.InnerResult.ToSimpleString();
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns></returns>
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
        /// <param name="result"></param>
        /// <returns></returns>
        public static implicit operator bool(DualDisposableResult<T> result)
        {
            return result.Result;
        }
    }

    /// <summary>
    /// 合併原本的DualResult物件，並且讓裡面含有一個ExtendedData屬性來放置要回傳的主要結果
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
        /// <param name="innerResult"></param>
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
        /// <returns></returns>
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
        /// <returns></returns>
        public BaseResult.MessageInfos ToMessages()
        {
            return this.InnerResult.ToMessages();
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns></returns>
        public string ToSimpleString()
        {
            return this.InnerResult.ToSimpleString();
        }

        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.InnerResult.ToString();
        }

        /// <summary>
        /// 隱含轉換布林結果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static implicit operator bool(DualResult<T> result)
        {
            return result.Result;
        }
    }

    namespace CustomizeControl
    {
        namespace MDivision
        {
            /// <summary>
            /// MDivision PickBox
            /// </summary>
            public class PickLookupBoxMDivision : BasePickLookupBox<string, string>
            {
                /// <summary>
                /// MDivision PickBox
                /// </summary>
                public PickLookupBoxMDivision()
                    : base()
                {
                    base.PopupSettings = CacheCoreMDivision.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreMDivision.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreMDivision.IDMaxLength);
                }
            }

            /// <summary>
            /// MDivision GridCellSetting
            /// </summary>
            public class GridCellSettingMDivision : BaseGridCellSetting
            {
                /// <summary>
                /// MDivision GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="setting"></param>
                public GridCellSettingMDivision(string propertyName, PopupColumnSettings setting = null)
                    : base(propertyName, setting ?? CacheCoreMDivision.DefaultColumnSettings)
                {
                    this.DefaultCacheCore = CacheCoreMDivision.Singleton;
                    base.MaxLength = CacheCoreMDivision.IDMaxLength;
                }
            }

            /// <summary>
            /// MDivision CacheCore
            /// </summary>
            public class CacheCoreMDivision : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 8;

                private static CacheCoreMDivision _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreMDivision Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreMDivision()
                {
                    _Singleton = new CacheCoreMDivision();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                {
                    new PopupColumnSetting("ID", columnWidth: 5, isPK: true),
                    new PopupColumnSetting("Name", columnWidth: 10),
                    new PopupColumnSetting("CountryID", columnWidth: 8),
                    new PopupColumnSetting("CountryName", columnWidth: 10),
                    new PopupColumnSetting("Manager3", headerText: "Manager", columnWidth: 25),
                }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"
Select MDivision.ID, MDivision.Name, MDivision.CountryID,
c.NameEN as CountryName,
m.Id as Manager1,
m.IdAndName as Manager2,
m.IdAndNameAndExt as Manager3
From MDivision
Left Join Country c on c.ID = MDivision.CountryID
Left Join GetName m on m.ID = MDivision.Manager";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "MDivision";
                    data.PrimaryKey = new[]
            {
                data.Columns["ID"],
            };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Name");
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }
            }
        }

        namespace MtlFactor
        {
            /// <summary>
            /// MtlFactor PickBox
            /// </summary>
            public class PickLookupBoxMtlFactor : BasePickLookupBox<string, string>
            {
                #region Restrictor - Type

                /// <summary>
                /// [不影響CustomizeCacheCore]用於代替限制器的純值，如果此屬性不為null，則使用Type來判定限制值
                /// </summary>
                [Category("SCIC-PickLookupBoxMtlFactor")]
                [Description("要用來篩選MtlFactor的Type")]
                public string FixedType
                {
                    get
                    {
                        if (this.CustomizeCacheCore == null)
                            return this.MyCacheCore.Type;
                        else
                            return null;
                    }
                    set
                    {
                        if (this.CustomizeCacheCore == null)
                            this.MyCacheCore.Type = value;
                    }
                }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreMtlFactor MyCacheCore
                {
                    get
                    {
                        return (CacheCoreMtlFactor)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// MtlFactor PickBox
                /// </summary>
                public PickLookupBoxMtlFactor()
                    : base()
                {
                    base.PopupSettings = CacheCoreMtlFactor.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreMtlFactor.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreMtlFactor.IDMaxLength);
                }
            }

            /// <summary>
            /// MtlFactor GridCellSetting
            /// </summary>
            public class GridCellSettingMtlFactor : BaseGridCellSetting
            {
                /// <summary>
                /// MtlFactor GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="mtlFactorType"></param>
                /// <param name="setting"></param>
                public GridCellSettingMtlFactor(string propertyName, string mtlFactorType, PopupColumnSettings setting = null)
                    : base(propertyName, setting ?? CacheCoreMtlFactor.DefaultColumnSettings)
                {
                    this.DefaultCacheCore = new CacheCoreMtlFactor()
                    {
                        Type = mtlFactorType,
                    };
                    base.MaxLength = CacheCoreMtlFactor.IDMaxLength;
                }
            }

            /// <summary>
            /// MtlFactor CacheCore
            /// </summary>
            public class CacheCoreMtlFactor : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 8;

                private static CacheCoreMtlFactor _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreMtlFactor Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                private string _TypeFilter;
                /// <summary>
                /// 如果此屬性不為null，則會被用於Where條件子句來限制抓取資料
                /// </summary>
                public string Type
                {
                    get { return _TypeFilter; }
                    set
                    {
                        this.ClearCache();
                        _TypeFilter = value;
                    }
                }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreMtlFactor()
                {
                    _Singleton = new CacheCoreMtlFactor();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 15, isPK: true),
                        new PopupColumnSetting("Rate", columnWidth: 15, decimals: 2),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {

                    if (string.IsNullOrWhiteSpace(this.Type))
                        throw new ArgumentException("請先設定FixedType屬性");
                    var sql = @"Select ID, Rate From MtlFactor Where Type = @Type";
                    var p = new[] { new SqlParameter("Type", this.Type) };
                    DataTable data;
                    SQL.Select("", sql, out data, p);
                    data.TableName = "MtlFactor";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row["Rate"].ToString();
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }
            }
        }

        namespace Country
        {
            /// <summary>
            /// Country PickBox
            /// </summary>
            public class PickLookupBoxCountry : BasePickLookupBox<string, string>
            {
                /// <summary>
                /// Country PickBox
                /// </summary>
                public PickLookupBoxCountry()
                    : base()
                {
                    base.PopupSettings = CacheCoreCountry.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreCountry.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreCountry.IDMaxLength);
                }
            }

            /// <summary>
            /// Country GridCellSetting
            /// </summary>
            public class GridCellSettingCountry : BaseGridCellSetting
            {
                /// <summary>
                /// Country GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="setting"></param>
                public GridCellSettingCountry(string propertyName, PopupColumnSettings setting = null)
                    : base(propertyName, setting ?? CacheCoreCountry.DefaultColumnSettings)
                {
                    this.DefaultCacheCore = CacheCoreCountry.Singleton;
                    base.MaxLength = CacheCoreCountry.IDMaxLength;
                }
            }

            /// <summary>
            /// Country CacheCore
            /// </summary>
            public class CacheCoreCountry : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 2;

                private static CacheCoreCountry _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreCountry Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreCountry()
                {
                    _Singleton = new CacheCoreCountry();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 3, isPK: true),
                        new PopupColumnSetting("NameEN", columnWidth: 20),
                        new PopupColumnSetting("NameCH", columnWidth: 20),
                        new PopupColumnSetting("Alias", columnWidth: 20),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ID, NameEN, NameCH, Alias From Country Where IsNull(Junk, 0) = 0";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Country";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("NameEN");
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }
            }
        }

        namespace DropDownList
        {
            /// <summary>
            /// PickIdBoxDropDownList
            /// </summary>
            public class PickIdBoxDropDownList : BasePickIdBox<string[], string>
            {
                #region Restrictor - Type
                private string _Type;
                /// <summary>
                /// DropdownList.Type欄位篩選
                /// </summary>
                [Category("SCIC-PickIdBoxDropDownList")]
                [Description("DropdownList.Type欄位篩選")]
                public string Type
                {
                    get
                    {
                        return this._Type;
                    }
                    set
                    {
                        if (this._Type != value)
                        {
                            this._Type = value;
                            if (Env.DesignTime)
                                return;
                            base.DataFilterSetting.Predictor = (row) => row.Field<string>("Type") == this._Type;
                            base.DataFilterSetting.TextToPKArray = (text) => new object[] { _Type, text };
                            base.AutoCacheCore.ClearCache();
                            base.ReValidate();
                        }
                    }
                }
                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreDropDownList MyCacheCore
                {
                    get
                    {
                        return (CacheCoreDropDownList)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxDropDownList()
                    : base()
                {
                    base.PopupSettings = CacheCoreDropDownList.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreDropDownList.Singleton;
                    this.Type = string.Empty;
                }
            }

            /// <summary>
            /// MultiPickIdDropDownList
            /// </summary>
            public class MultiPickIdDropDownList : BaseMultiplePickIdBox<string[], string>
            {
                #region Restrictor - Type
                private string _Type;
                /// <summary>
                /// DropdownList.Type欄位篩選
                /// </summary>
                [Category("SCIC-MultiPickIdDropDownList")]
                [Description("DropdownList.Type欄位篩選")]
                public string Type
                {
                    get
                    {
                        return this._Type;
                    }
                    set
                    {
                        if (this._Type != value)
                        {
                            this._Type = value;
                            if (Env.DesignTime)
                                return;
                            base.DataFilterSetting.Predictor = (row) => row.Field<string>("Type") == this._Type;
                            base.DataFilterSetting.TextToPKArray = (text) => new object[] { _Type, text };
                            base.AutoCacheCore.ClearCache();
                            base.ReValidate();
                        }
                    }
                }
                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreDropDownList MyCacheCore
                {
                    get
                    {
                        return (CacheCoreDropDownList)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// DropDownList MultiPickBox
                /// </summary>
                public MultiPickIdDropDownList()
                {
                    base.PopupSettings = CacheCoreDropDownList.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreDropDownList.Singleton;
                    this.Type = string.Empty;
                }
            }

            /// <summary>
            /// PickLookupBoxDropDownList
            /// </summary>
            public class PickLookupBoxDropDownList : BasePickLookupBox<string[], string>
            {
                #region Restrictor - Type
                private string _Type;
                /// <summary>
                /// DropdownList.Type欄位篩選
                /// </summary>
                [Category("SCIC-PickLookupBoxDropDownList")]
                [Description("DropdownList.Type欄位篩選")]
                public string Type
                {
                    get
                    {
                        return this._Type;
                    }
                    set
                    {
                        if (this._Type != value)
                        {
                            this._Type = value;
                            if (Env.DesignTime)
                                return;
                            base.DataFilterSetting.Predictor = (row) => row.Field<string>("Type") == this._Type;
                            base.DataFilterSetting.TextToPKArray = (text) => new object[] { _Type, text };
                            base.AutoCacheCore.ClearCache();
                            base.ReValidate();
                        }
                    }
                }
                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreDropDownList MyCacheCore
                {
                    get
                    {
                        return (CacheCoreDropDownList)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// PickLookupBoxDropDownList
                /// </summary>
                public PickLookupBoxDropDownList()
                    : base()
                {
                    base.PopupSettings = CacheCoreDropDownList.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreDropDownList.Singleton;
                    this.Type = string.Empty;
                }
            }

            /// <summary>
            /// DropDownList CacheCore
            /// </summary>
            public class CacheCoreDropDownList : BaseDataCacheCore<string[], string>
            {
                /// <summary>
                /// 獨體
                /// </summary>
                internal static CacheCoreDropDownList Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// DropDownList CacheCore
                /// </summary>
                static CacheCoreDropDownList()
                {
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", isPK: true, columnWidth: 10),
                        new PopupColumnSetting("Name", columnWidth: 10),
                        new PopupColumnSetting("Description", columnWidth: 40),
                    }.ToList());
                    Singleton = new CacheCoreDropDownList();
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select Type, ID, Name, Description, Seq From DropDownList";
                    using (var dr = DBProxy.Current.SelectEx(sql, true))
                    {
                        if (dr == false)
                            MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                        else
                            dr.DisposeExtendedData = false;
                        dr.ExtendedData.PrimaryKey = new[]
                        {
                            dr.ExtendedData.Columns["Type"],
                            dr.ExtendedData.Columns["ID"],
                        };
                        return dr.ExtendedData;
                    }
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Name");
                }
            }
        }

        namespace Reason
        {
            /// <summary>
            /// PickIdBoxReason
            /// </summary>
            public class PickIdBoxReason : BasePickIdBox<string[], string>
            {
                #region Filter - ReasonType

                /// <summary>
                /// ReasonType
                /// </summary>
                [Category("SCIC-PickIdBoxReason")]
                public CacheCoreDataFilter<string> ReasonTypeFilter { get; set; }

                #endregion

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxReason()
                    : base()
                {
                    this.MaxLength = CacheCoreReason.IDMaxLength;

                    base.PopupSettings = CacheCoreReason.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreReason.Singleton;

                    base.DataFilterSetting.Items.Add(this.ReasonTypeFilter = new CacheCoreDataFilter<string>(true, "ReasonTypeID"));
                    base.DataFilterSetting.TextToPKArray = (value) => new object[] { this.ReasonTypeFilter.Value, value };
                }
            }

            /// <summary>
            /// Reason PickBox
            /// </summary>
            public class PickLookupBoxReason : BasePickLookupBox<string[], string>
            {
                #region Filter - ReasonType

                /// <summary>
                /// ReasonType
                /// </summary>
                [Category("SCIC-PickLookupBoxReason")]
                [Browsable(true)]
                [Description("篩選ReasonType")]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
                public CacheCoreDataFilter<string> ReasonTypeFilter { get; set; }

                #endregion

                /// <summary>
                /// Reason PickBox
                /// </summary>
                public PickLookupBoxReason()
                    : base()
                {
                    this.SetInputMaxLengthAndWidth(CacheCoreReason.IDMaxLength);

                    base.PopupSettings = CacheCoreReason.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreReason.Singleton;

                    base.DataFilterSetting.Items.Add(this.ReasonTypeFilter = new CacheCoreDataFilter<string>(true, "ReasonTypeID"));
                    base.DataFilterSetting.TextToPKArray = (value) => new object[] { this.ReasonTypeFilter.Value, value };
                }
            }

            /// <summary>
            /// Reason MultiPickBox
            /// </summary>
            public class MultiPickIdReason : BaseMultiplePickIdBox<string[], string>
            {
                #region Filter - ReasonType

                /// <summary>
                /// ReasonType
                /// </summary>
                [Category("SCIC-MultiPickIdReason")]
                public CacheCoreDataFilter<string> ReasonTypeFilter { get; set; }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreReason MyCacheCore
                {
                    get
                    {
                        return (CacheCoreReason)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Reason MultiPickBox
                /// </summary>
                public MultiPickIdReason()
                {
                    base.PopupSettings = CacheCoreReason.DefaultColumnSettings;
                    this.DefaultCacheCore = new CacheCoreReason();

                    base.DataFilterSetting.Items.Add(this.ReasonTypeFilter = new CacheCoreDataFilter<string>(true, "ReasonTypeID"));
                    base.DataFilterSetting.TextToPKArray = (value) => new object[] { this.ReasonTypeFilter.Value, value };
                }
            }

            /// <summary>
            /// Reason GridCellSetting
            /// </summary>
            public class GridCellSettingReason : BaseGridCellSetting<string[], string>
            {
                #region Filter - ReasonType

                /// <summary>
                /// ReasonType
                /// </summary>
                [Category("SCIC-GridCellSettingReason")]
                public CacheCoreDataFilter<string> ReasonTypeFilter { get; set; }

                #endregion

                /// <summary>
                /// Reason GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="setting"></param>
                public GridCellSettingReason(string propertyName, PopupColumnSettings setting = null)
                    : base(propertyName, setting ?? CacheCoreReason.DefaultColumnSettings)
                {
                    base.DefaultCacheCore = CacheCoreReason.Singleton;
                    base.MaxLength = CacheCoreReason.IDMaxLength;

                    base.DataFilterSetting.Items.Add(this.ReasonTypeFilter = new CacheCoreDataFilter<string>(true, "ReasonTypeID"));
                    base.DataFilterSetting.TextToPKArray = (value) => new object[] { this.ReasonTypeFilter.Value, value };
                }
            }

            /// <summary>
            /// Reason CacheCore
            /// </summary>
            public class CacheCoreReason : BaseDataCacheCore<string[], string>
            {
                /// <summary>
                /// 
                /// </summary>
                public static int IDMaxLength = 5;

                /// <summary>
                /// 
                /// </summary>
                public static CacheCoreReason Singleton { get; set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// Reason CacheCore
                /// </summary>
                static CacheCoreReason()
                {
                    Singleton = new CacheCoreReason();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", isPK: true, columnWidth: 10),
                        new PopupColumnSetting("Name", columnWidth: 10),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ReasonTypeID, ID, Name, Remark Junk From Reason Order By ReasonTypeID, No";
                    var dr = DBProxy.Current.SelectEx(sql);
                    if (dr == false)
                    {
                        MyUtility.Msg.ErrorBox(dr.ToSimpleString());
                        var dump = new DataTable();
                        dump.ColumnsStringAdd("ReasonTypeID");
                        dump.ColumnsStringAdd("ID");
                        dump.ColumnsStringAdd("Name");
                        dump.ColumnsStringAdd("Remark");
                        dump.ColumnsBooleanAdd("Junk");
                        return dump;
                    }
                    var data = dr.ExtendedData;
                    data.TableName = "Reason";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ReasonTypeID"],
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Name");
                }
            }
        }

        namespace InvtransReason
        {
            /// <summary>
            /// 倉庫異動單的狀態
            /// </summary>
            public enum InvtransReasonTypeEnum
            {
                /// <summary>
                /// 入庫
                /// </summary>
                InvInput = 1,
                /// <summary>
                /// 出庫
                /// </summary>
                InvOutput = 2,
                /// <summary>
                /// 移庫
                /// </summary>
                InvTransfer = 3,
                /// <summary>
                /// 調整
                /// </summary>
                InvAdjust = 4,
                /// <summary>
                /// 報廢
                /// </summary>
                InvObsolate = 5,
                /// <summary>
                /// 退回
                /// </summary>
                InvReturn = 6,
            }

            /// <summary>
            /// PickIdBoxInvtransReason
            /// </summary>
            public class PickIdBoxInvtransReason : BasePickIdBox<string, string>
            {
                #region Restrictor - InvtransReasonType
                private InvtransReasonTypeEnum? _InvtransReasonType;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickIdBoxInvtransReason")]
                public InvtransReasonTypeEnum? InvtransReasonType
                {
                    get
                    {
                        return this._InvtransReasonType;
                    }
                    set
                    {
                        this._InvtransReasonType = value;
                        var typeCode = ((int)value).ToString();
                        base.DataFilterSetting.Predictor = (row) => (row.Field<string>("ID") ?? string.Empty).StartsWith(typeCode);
                    }
                }
                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreInvtransReason MyCacheCore
                {
                    get
                    {
                        return (CacheCoreInvtransReason)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxInvtransReason()
                    : base()
                {
                    base.PopupSettings = CacheCoreInvtransReason.DefaultColumnSettings;
                    this.MyCacheCore = new CacheCoreInvtransReason();
                }
            }

            /// <summary>
            /// InvtransReason PickBox
            /// </summary>
            public class PickLookupBoxInvtransReason : BasePickLookupBox<string, string>
            {
                #region Restrictor - InvtransReasonType
                private InvtransReasonTypeEnum? _InvtransReasonType;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickLookupBoxReason")]
                public InvtransReasonTypeEnum? InvtransReasonType
                {
                    get
                    {
                        return this._InvtransReasonType;
                    }
                    set
                    {
                        if (this._InvtransReasonType != value)
                        {
                            this._InvtransReasonType = value;
                            if (this._InvtransReasonType.HasValue)
                            {
                                var typeCode = ((int)value).ToString();
                                base.DataFilterSetting.Predictor = (row) => (row.Field<string>("ID") ?? string.Empty).StartsWith(typeCode);
                            }
                            else
                                base.DataFilterSetting.Predictor = (row) => true;
                        }
                    }
                }
                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore&lt;string, string&gt;) => CacheCoreReason
                /// </summary>
                private CacheCoreInvtransReason MyCacheCore
                {
                    get
                    {
                        return (CacheCoreInvtransReason)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// InvtransReason PickBox
                /// </summary>
                public PickLookupBoxInvtransReason()
                    : base()
                {
                    base.PopupSettings = CacheCoreInvtransReason.DefaultColumnSettings;
                    this.DefaultCacheCore = new CacheCoreInvtransReason();
                    this.SetInputMaxLengthAndWidth(CacheCoreInvtransReason.IDMaxLength);
                }
            }

            /// <summary>
            /// InvtransReason MultiPickBox
            /// </summary>
            public class MultiPickIdInvtransReason : BaseMultiplePickIdBox<string, string>
            {
                #region Restrictor - InvtransReasonType
                private InvtransReasonTypeEnum? _InvtransReasonType;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-MultiPickIdReason")]
                public InvtransReasonTypeEnum? InvtransReasonType
                {
                    get
                    {
                        return this._InvtransReasonType;
                    }
                    set
                    {
                        this._InvtransReasonType = value;
                        var typeCode = ((int)value).ToString();
                        base.DataFilterSetting.Predictor = (row) => (row.Field<string>("ID") ?? string.Empty).StartsWith(typeCode);
                    }
                }
                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreInvtransReason MyCacheCore
                {
                    get
                    {
                        return (CacheCoreInvtransReason)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// InvtransReason MultiPickBox
                /// </summary>
                public MultiPickIdInvtransReason()
                {
                    base.PopupSettings = CacheCoreInvtransReason.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreInvtransReason.Singleton;
                }
            }

            /// <summary>
            /// InvtransReason GridCellSetting
            /// </summary>
            public class GridCellSettingInvtransReason : BaseGridCellSetting
            {
                #region Restrictor - InvtransReasonType
                private InvtransReasonTypeEnum? _InvtransReasonType;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-GridCellSettingInvtransReason")]
                public InvtransReasonTypeEnum? InvtransReasonType
                {
                    get
                    {
                        return this._InvtransReasonType;
                    }
                    set
                    {
                        this._InvtransReasonType = value;
                        var typeCode = ((int)value).ToString();
                        base.DataFilterSetting.Predictor = (row) => (row.Field<string>("ID") ?? string.Empty).StartsWith(typeCode);
                    }
                }
                #endregion

                /// <summary>
                /// Keyword GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="invtransReasonType"></param>
                /// <param name="setting"></param>
                public GridCellSettingInvtransReason(string propertyName, InvtransReasonTypeEnum? invtransReasonType = null, PopupColumnSettings setting = null)
                    : base(propertyName, setting ?? CacheCoreInvtransReason.DefaultColumnSettings)
                {
                    this.DefaultCacheCore = CacheCoreInvtransReason.Singleton;
                    base.MaxLength = CacheCoreInvtransReason.IDMaxLength;
                    this.InvtransReasonType = invtransReasonType;
                }
            }

            /// <summary>
            /// InvtransReason CacheCore
            /// </summary>
            public class CacheCoreInvtransReason : BaseDataCacheCore<string, string>
            {
                /// <summary>
                /// 
                /// </summary>
                public static int IDMaxLength = 5;

                private static CacheCoreInvtransReason _Singleton = new CacheCoreInvtransReason();
                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreInvtransReason Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// InvtransReason CacheCore
                /// </summary>
                static CacheCoreInvtransReason()
                {
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", isPK: true, columnWidth: 6),
                        new PopupColumnSetting("ReasonEN", headerText: "Name", columnWidth: 200),
                        new PopupColumnSetting("ReasonCH", headerText: "Name", columnWidth: 200),
                    }.ToList());
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ID, ReasonEN, ReasonCH, Junk From InvtransReason";
                    using (var dr = DBProxy.Current.SelectEx(sql))
                    {
                        if (dr == false)
                            throw new ApplicationException("Fail to execute sql : " + sql);
                        else
                        {
                            dr.DisposeExtendedData = false;
                            var dt = dr.ExtendedData;
                            dt.TableName = "InvtransReason";
                            dt.PrimaryKey = new[]
                            {
                                dt.Columns["ID"],
                            };
                            return dt;
                        }
                    }
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("ReasonEN");
                }
            }
        }

        namespace PatternAnnotationArtwork
        {
            /// <summary>
            /// MDivision PickBox
            /// </summary>
            public class PickLookupBoxPatternAnnotationArtwork : BasePickLookupBox<string, string>
            {
                /// <summary>
                /// MDivision PickBox
                /// </summary>
                public PickLookupBoxPatternAnnotationArtwork()
                    : base()
                {
                    base.PopupSettings = CacheCorePatternAnnotationArtwork.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCorePatternAnnotationArtwork.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCorePatternAnnotationArtwork.IDMaxLength);
                }
            }

            /// <summary>
            /// MDivision GridCellSetting
            /// </summary>
            public class GridCellSettingPatternAnnotationArtwork : BaseGridCellSetting
            {
                /// <summary>
                /// MDivision GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                public GridCellSettingPatternAnnotationArtwork(string propertyName)
                    : this(propertyName, CacheCorePatternAnnotationArtwork.DefaultColumnSettings)
                {
                    base.MaxLength = CacheCorePatternAnnotationArtwork.IDMaxLength;
                }

                /// <summary>
                /// PatternAnnotationArtwork GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="setting"></param>
                public GridCellSettingPatternAnnotationArtwork(string propertyName, PopupColumnSettings setting)
                    : base(propertyName, setting)
                {
                    this.DefaultCacheCore = CacheCorePatternAnnotationArtwork.Singleton;
                    base.MaxLength = CacheCorePatternAnnotationArtwork.IDMaxLength;
                }
            }

            /// <summary>
            /// MDivision CacheCore
            /// </summary>
            public class CacheCorePatternAnnotationArtwork : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 20;

                private static CacheCorePatternAnnotationArtwork _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCorePatternAnnotationArtwork Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCorePatternAnnotationArtwork()
                {
                    _Singleton = new CacheCorePatternAnnotationArtwork();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 10, isPK: true),
                        new PopupColumnSetting("NameCH", columnWidth: 30),
                        new PopupColumnSetting("NameEN", columnWidth: 30),
                        new PopupColumnSetting("ArtworkTypeID", columnWidth: 10),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ID, NameCH, NameEN, LTrim(RTrim(ArtworkTypeID)) as ArtworkTypeID From Pattern_Annotation_Artwork Order By ID";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Pattern_Annotation_Artwork";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("ArtworkTypeID");
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }
            }
        }

        namespace Keyword
        {
            /// <summary>
            /// Keyword PickBox
            /// </summary>
            public class PickLookupBoxKeyword : BasePickLookupBox<string, string>
            {
                /// <summary>
                /// Keyword PickBox
                /// </summary>
                public PickLookupBoxKeyword()
                    : base()
                {
                    base.PopupSettings = CacheCoreKeyword.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreKeyword.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreKeyword.IDMaxLength);
                }
            }

            /// <summary>
            /// Keyword GridCellSetting
            /// </summary>
            public class GridCellSettingKeyword : BaseGridCellSetting
            {
                /// <summary>
                /// Keyword GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                public GridCellSettingKeyword(string propertyName)
                    : this(propertyName, CacheCoreKeyword.DefaultColumnSettings)
                {
                    base.MaxLength = CacheCoreKeyword.IDMaxLength;
                }

                /// <summary>
                /// Keyword GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="setting"></param>
                public GridCellSettingKeyword(string propertyName, PopupColumnSettings setting)
                    : base(propertyName, setting)
                {
                    this.DefaultCacheCore = CacheCoreKeyword.Singleton;
                    base.MaxLength = CacheCoreKeyword.IDMaxLength;
                }
            }

            /// <summary>
            /// Keyword CacheCore
            /// </summary>
            public class CacheCoreKeyword : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 30;

                private static CacheCoreKeyword _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                internal static CacheCoreKeyword Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreKeyword()
                {
                    _Singleton = new CacheCoreKeyword();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 32, isPK: true),
                        new PopupColumnSetting("Description", columnWidth: 60),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ID, Description, Junk From Keyword";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Keyword";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }
            }
        }

        namespace CDCode
        {
            /// <summary>
            /// CDCode PickBox
            /// </summary>
            public class PickLookupBoxCDCode : BasePickLookupBox<string, string>
            {
                /// <summary>
                /// 
                /// </summary>
                public PickLookupBoxCDCode()
                    : base()
                {
                    base.PopupSettings = CacheCoreCDCode.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreCDCode.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreCDCode.IDMaxLength);
                }
            }

            /// <summary>
            /// CDCode GridCellSetting
            /// </summary>
            public class GridCellSettingCDCode : BaseGridCellSetting
            {
                /// <summary>
                /// 
                /// </summary>
                /// <param name="propertyName"></param>
                public GridCellSettingCDCode(string propertyName)
                    : this(propertyName, CacheCoreCDCode.DefaultColumnSettings)
                {
                    base.MaxLength = CacheCoreCDCode.IDMaxLength;
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="setting"></param>
                public GridCellSettingCDCode(string propertyName, PopupColumnSettings setting)
                    : base(propertyName, setting)
                {
                    this.DefaultCacheCore = CacheCoreCDCode.Singleton;
                    base.MaxLength = CacheCoreCDCode.IDMaxLength;
                }
            }

            /// <summary>
            /// CDCode CachCore
            /// </summary>
            public class CacheCoreCDCode : BaseDataCacheCore<string, string>
            {
                /// <summary>
                /// 
                /// </summary>
                internal const int IDMaxLength = 6;

                private static CacheCoreCDCode _Singleton;
                internal static CacheCoreCDCode Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                static CacheCoreCDCode()
                {
                    _Singleton = new CacheCoreCDCode();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 6, isPK: true),
                        new PopupColumnSetting("Description", columnWidth: 30),
                        new PopupColumnSetting("Cpu", columnWidth: 4, decimals:3),
                        new PopupColumnSetting("ComboPcs", columnWidth: 10, headerText: "Unit"),
                    }.ToList());
                }

                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"
Select RTRIM(ID) as ID
, Description
, Cpu
, ComboPcs = Case ComboPcs 
        When 1 Then 'PCS' 
        When 2 Then 'SETS' 
        Else Convert(nvarchar(2), ComboPcs) 
        End
, Junk 
From CDCode
Order By ID";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "CDCode";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用CDCode.ID換CDCode.Description
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }

                /// <summary>
                /// 
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }
            }
        }

        namespace Supplier
        {
            /// <summary>
            /// Supplier PickBox
            /// </summary>
            public class PickLookupBoxSupplier : BasePickLookupBox<string, string>, IJunkEffectiveSwitch
            {
                /// <summary>
                /// 
                /// </summary>
                public PickLookupBoxSupplier()
                    : base()
                {
                    base.PopupSettings = CacheCoreSupplier.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreSupplier.Singleton;
                    var widthAdjust = -1;
                    using (var gra = this.textBox1.CreateGraphics())
                    {
                        var size = gra.MeasureString(new string('0', CacheCoreSupplier.IDMaxLength + 1), this.textBox1.Font);
                        widthAdjust = size.ToSize().Width - base.textBox1.Width;
                    }
                    base.textBox1.Width += widthAdjust;
                    base.displayBox1.Width -= widthAdjust;
                    base.displayBox1.Left += widthAdjust;
                    base.textBox1.MaxLength = CacheCoreSupplier.IDMaxLength;
                    this.SetInputMaxLengthAndWidth(CacheCoreSupplier.IDMaxLength);
                    if (Env.DesignTime == false)
                    {
                        base.DataFilterSetting.DialogIncludeJunked = false;
                        base.DataFilterSetting.ValidatingIncludeJunked = false;
                    }
                }

                /// <summary>
                /// 是否在開窗選擇的時候，要顯示Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxSupplier")]
                [Description("是否在開窗選擇的時候，要顯示Junk的資料")]
                public bool? DialogIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.DialogIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.DialogIncludeJunked = value;
                    }
                }

                /// <summary>
                /// 是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxSupplier")]
                [Description("是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料")]
                public bool? ValidatingIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.ValidatingIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.ValidatingIncludeJunked = value;
                    }
                }
            }

            /// <summary>
            /// Supplier MultiPickBox
            /// </summary>
            public class MultiPickIdBoxSupplier : BaseMultiplePickIdBox<string, string>
            {
                /// <summary>
                /// Supplier MultiPickBox
                /// </summary>
                public MultiPickIdBoxSupplier()
                {
                    base.PopupSettings = CacheCoreSupplier.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreSupplier.Singleton;
                }
            }

            /// <summary>
            /// Supplier CacheCore
            /// </summary>
            public class CacheCoreSupplier : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 6;
                private static CacheCoreSupplier _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreSupplier Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreSupplier()
                {
                    _Singleton = new CacheCoreSupplier();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 5, isPK: true),
                        new PopupColumnSetting("AbbEN", headerText: "Name", columnWidth: 10),
                        new PopupColumnSetting("CountryID", headerText: "Country", columnWidth: 8),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select RTRIM(ID) as ID, AbbEN, CountryID, Junk From Supp Order By ID";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Supp";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("AbbEN");
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }
            }
        }

        namespace Schedule
        {
            /// <summary>
            /// Schedule PickDateBox
            /// </summary>
            public class PickDateBoxSchedule : BasePickDateBox
            {
                /// <summary>
                /// Schedule PickDateBox
                /// </summary>
                public PickDateBoxSchedule()
                {
                    base.PopupSettings = CacheCoreSchedule.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreSchedule.Singleton;
                }

                /// <summary>
                /// 用日期反查所有的ID
                /// </summary>
                /// <param name="data"></param>
                /// <returns></returns>
                protected override DataTable GetFilteredDataTable(DataTable data)
                {
                    if (this.Value.HasValue)
                    {
                        var filteredRows = data.AsEnumerable().Where(row => row.Field<DateTime>("ScheduleDate") == this.Value.Value);
                        if (filteredRows.Any())
                            return filteredRows.CopyToDataTable();
                        else
                            return data.AsEnumerable().Where(row => row.Field<DateTime>("ScheduleDate") >= DateTime.Now.Date).CopyToDataTable();
                    }
                    else
                        return data.AsEnumerable().Where(row => row.Field<DateTime>("ScheduleDate") >= DateTime.Now.Date).CopyToDataTable();
                }
            }

            /// <summary>
            /// Schedule CacheCore
            /// </summary>
            public class CacheCoreSchedule : BaseDataCacheCore<string, DateTime?>
            {
                internal const int IDMaxLength = 13;
                private static CacheCoreSchedule _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                internal static CacheCoreSchedule Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreSchedule()
                {
                    _Singleton = new CacheCoreSchedule();
                    DefaultColumnSettings = new PopupColumnSettings(
                        new PopupColumnSetting("ID", columnWidth: 9, isPK: true),
                        new PopupColumnSetting("ScheduleDate", columnWidth: 10),
                        new PopupColumnSetting("LoadDate", columnWidth: 10),
                        new PopupColumnSetting("ExportCountry", columnWidth: 10),
                        new PopupColumnSetting("ExportPort", columnWidth: 12),
                        new PopupColumnSetting("ImportCountry", columnWidth: 10),
                        new PopupColumnSetting("ImportPort", columnWidth: 10),
                        new PopupColumnSetting("ShipModeID", columnWidth: 10),
                        new PopupColumnSetting("IncludeShipTerm", columnWidth: 10),
                        new PopupColumnSetting("ExcludeShipTerm", columnWidth: 10),
                        new PopupColumnSetting("CYCFS", columnWidth: 10),
                        new PopupColumnSetting("IncludeSupplier", columnWidth: 10),
                        new PopupColumnSetting("ExcludeSupplier", columnWidth: 10),
                        new PopupColumnSetting("Second", columnWidth: 5)
                    );
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"
Select 
    ID , 
    ScheduleDate, LoadDate, ExportCountry, ExportPort, ImportCountry, ImportPort, ShipModeID,
    IncludeShipTerm ,ExcludeShipTerm, CYCFS, IncludeSupplier, ExcludeSupplier, Second 
From dbo.Schedule 
Where junk = 0
Order by ID, LoadDate";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Schedule";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override DateTime? Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<DateTime?>("ScheduleDate");
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }
            }
        }

        namespace Factory
        {
            /// <summary>
            /// Factory PickBox
            /// </summary>
            public class PickLookupBoxFactory : BasePickLookupBox<string, string>
            {
                #region Restrictor - CountryID
                private string _CountryID;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickLookupBoxFactory")]
                [Bindable(true)]
                public string CountryID
                {
                    get
                    {
                        return this._CountryID;
                    }
                    set
                    {
                        if (string.Compare(this._CountryID, value) != 0)
                        {
                            this._CountryID = value;
                            if (Env.DesignTime)
                                return;
                            base.DataFilterSetting.Predictor = (row) => row.Field<string>("CountryID") == _CountryID;
                            base.AutoCacheCore.ClearCache();
                            base.ReValidate();
                        }
                    }
                }
                #endregion

                /// <summary>
                /// Factory PickBox
                /// </summary>
                public PickLookupBoxFactory()
                    : base()
                {
                    base.PopupSettings = CacheCoreFactory.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreFactory.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreFactory.IDMaxLength);
                }
            }

            /// <summary>
            /// Supplier MultiPickBox
            /// </summary>
            public class MultiPickIdBoxFactory : BaseMultiplePickIdBox<string, string>
            {
                #region Restrictor - CountryID
                private string _CountryID;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-MultiPickIdBoxFactory")]
                public string CountryID
                {
                    get
                    {
                        return this._CountryID;
                    }
                    set
                    {
                        if (string.Compare(this._CountryID, value) != 0)
                        {
                            this._CountryID = value;
                            if (Env.DesignTime)
                                return;
                            base.DataFilterSetting.Predictor = (row) => row.Field<string>("CountryID") == _CountryID;
                            this.Text = this.Text
                                .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                .Where(id => base.AutoCacheCore.FindRowByValidatingSetting(base.DataFilterSetting, base.DataFilterSetting.GetFullPK(id)) != null)
                                .JoinToString(",");
                        }
                    }
                }
                #endregion

                /// <summary>
                /// Supplier MultiPickBox
                /// </summary>
                public MultiPickIdBoxFactory()
                    : base()
                {
                    base.PopupSettings = CacheCoreFactory.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreFactory.Singleton;
                }
            }

            /// <summary>
            /// Factory GridCellSetting
            /// </summary>
            public class GridCellSettingFactory : BaseGridCellSetting
            {
                #region Restrictor - CountryID
                private string _CountryID;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-GridCellSettingFactory")]
                [Bindable(true)]
                public string CountryID
                {
                    get
                    {
                        return this._CountryID;
                    }
                    set
                    {
                        if (string.Compare(this._CountryID, value) != 0)
                        {
                            this._CountryID = value;
                            if (Env.DesignTime)
                                return;
                            if (string.IsNullOrWhiteSpace(value) == false)
                                base.DataFilterSetting.Predictor = (row) => row.Field<string>("CountryID") == _CountryID;
                            else
                                base.DataFilterSetting.Predictor = null;
                            base.ReValidate();
                        }
                    }
                }
                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreFactory MyCacheCore
                {
                    get
                    {
                        return (CacheCoreFactory)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Factory GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="countryId"></param>
                public GridCellSettingFactory(string propertyName, string countryId = null)
                    : this(propertyName, countryId, CacheCoreFactory.DefaultColumnSettings)
                {
                    base.MaxLength = CacheCoreFactory.IDMaxLength;
                }

                /// <summary>
                /// Factory GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="countryId"></param>
                /// <param name="setting"></param>
                public GridCellSettingFactory(string propertyName, string countryId, PopupColumnSettings setting)
                    : base(propertyName, setting)
                {
                    this.DefaultCacheCore = new CacheCoreFactory();
                    base.MaxLength = CacheCoreFactory.IDMaxLength;
                    this.CountryID = countryId;
                }
            }

            /// <summary>
            /// Supplier CacheCore
            /// </summary>
            public class CacheCoreFactory : BaseDataCacheCore<string, string>
            {
                /// <summary>
                /// 
                /// </summary>
                internal const int IDMaxLength = 8;

                private static CacheCoreFactory _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                internal static CacheCoreFactory Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreFactory()
                {
                    _Singleton = new CacheCoreFactory();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 15, isPK: true),
                        new PopupColumnSetting("CountryID", headerText: "Country", columnWidth: 8),
                        new PopupColumnSetting("NameEN", headerText: "Name", columnWidth: 30),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"
Select
    ID, CountryID, NameCH, NameEN
From Factory 
Where junk = 0 
Order By CountryID, ID";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Factory";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("NameEN");
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }
            }
        }

        namespace Brand
        {
            /// <summary>
            /// Brand PickBox
            /// </summary>
            public class PickLookupBoxBrand : BasePickLookupBox<string, string>
            {
                /// <summary>
                /// Brand PickBox
                /// </summary>
                public PickLookupBoxBrand()
                    : base()
                {
                    base.PopupSettings = CacheCoreBrand.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreBrand.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreBrand.IDMaxLength);
                }
            }

            /// <summary>
            /// Brand MultiPickBox
            /// </summary>
            public class MultiPickIdBoxBrand : BaseMultiplePickIdBox<string, string>
            {
                /// <summary>
                /// Brand MultiPickBox
                /// </summary>
                public MultiPickIdBoxBrand()
                    : base()
                {
                    base.PopupSettings = CacheCoreBrand.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreBrand.Singleton;
                }
            }

            /// <summary>
            /// Brand CacheCore
            /// </summary>
            public class CacheCoreBrand : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 8;

                private static CacheCoreBrand _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                internal static CacheCoreBrand Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreBrand()
                {
                    _Singleton = new CacheCoreBrand();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 6, isPK: true),
                        new PopupColumnSetting("NameEN", columnWidth: 42),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ID, NameEN From Brand Where IsNull(Junk, 0) = 0";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Brand";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("NameEN");
                }
            }
        }

        namespace Project
        {
            /// <summary>
            /// Project PickBox
            /// </summary>
            public class PickLookupBoxProject : BasePickLookupBox<string, string>, IJunkEffectiveSwitch
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                #region IJunkEffectiveSwitch

                /// <summary>
                /// 是否在開窗選擇的時候，要顯示Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("是否在開窗選擇的時候，要顯示Junk的資料")]
                public bool? DialogIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.DialogIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.DialogIncludeJunked = value;
                    }
                }

                /// <summary>
                /// 是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料")]
                public bool? ValidatingIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.ValidatingIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.ValidatingIncludeJunked = value;
                    }
                }

                #endregion

                /// <summary>
                /// 
                /// </summary>
                public PickLookupBoxProject()
                    : base()
                {
                    base.PopupSettings = CacheCoreProject.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreProject.Singleton;
                    var widthAdjust = -1;
                    using (var gra = this.textBox1.CreateGraphics())
                    {
                        var size = gra.MeasureString(new string('0', CacheCoreProject.IDMaxLength + 1), this.textBox1.Font);
                        widthAdjust = size.ToSize().Width - base.textBox1.Width;
                    }
                    base.textBox1.Width += widthAdjust;
                    base.displayBox1.Width -= widthAdjust;
                    base.displayBox1.Left += widthAdjust;
                    base.textBox1.MaxLength = CacheCoreProject.IDMaxLength;
                    this.SetInputMaxLengthAndWidth(CacheCoreProject.IDMaxLength);

                    if (Env.DesignTime)
                        return;

                    base.DataFilterSetting.DialogIncludeJunked = false;
                    base.DataFilterSetting.ValidatingIncludeJunked = false;
                    base.DataFilterSetting.TextToPKArray = text => new object[] { this.BrandFilter.Value, text };
                    base.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(true, "BrandID"));
                }
            }

            /// <summary>
            /// Project GridCellSetting
            /// </summary>
            public class GridCellSettingProject : BaseGridCellSetting
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                /// <summary>
                /// Project GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                public GridCellSettingProject(string propertyName)
                    : base(propertyName, CacheCoreProject.DefaultColumnSettings)
                {
                    base.MaxLength = CacheCoreProject.IDMaxLength;
                    base.DefaultCacheCore = CacheCoreProject.Singleton;
                    base.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(true, "BrandID"));
                    base.DataFilterSetting.TextToPKArray = text => new object[] { this.BrandFilter.Value, text };
                }
            }

            /// <summary>
            /// Project CacheCore
            /// </summary>
            public class CacheCoreProject : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 5;

                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreProject Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreProject()
                {
                    Singleton = new CacheCoreProject();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("BrandID", headerText: "Brand", columnWidth: 9),
                        new PopupColumnSetting("Id", headerText: "Project", isPK: true, columnWidth: 9),
                        new PopupColumnSetting("Description", headerText: "Desc", isPK: true, columnWidth: 30),
                        new PopupColumnSetting("InventoryDeadline", headerText: "inv Deadline", isPK: true, columnWidth: 15),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select BrandID, Id, Description, InventoryDeadline, Junk From Project Order by BrandID, ID";

                    DataTable data;
                    var dr = SQL.Select("", sql, out data);
                    data.TableName = "Project";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["BrandID"],
                        data.Columns["Id"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }
            }
        }

        namespace Phase
        {
            /// <summary>
            /// PickIdBoxPhase
            /// </summary>
            public class PickIdBoxPhase : BasePickIdBox<string, string>
            {
                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCorePhase MyCacheCore
                {
                    get
                    {
                        return (CacheCorePhase)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxPhase()
                    : base()
                {
                    base.PopupSettings = CacheCorePhase.DefaultColumnSettings;
                    this.MyCacheCore = new CacheCorePhase();
                }
            }

            /// <summary>
            /// Phase PickBox
            /// </summary>
            public class PickLookupBoxPhase : BasePickLookupBox<string, string>
            {
                /// <summary>
                /// Phase PickBox
                /// </summary>
                public PickLookupBoxPhase()
                    : base()
                {
                    base.PopupSettings = CacheCorePhase.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCorePhase.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCorePhase.IDMaxLength);
                }
            }

            /// <summary>
            /// Phase CacheCore
            /// </summary>
            public class CacheCorePhase : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 20;

                private static CacheCorePhase _Singleton;
                /// <summary>
                /// 獨體
                /// </summary>
                internal static CacheCorePhase Singleton
                {
                    get
                    {
                        return _Singleton;
                    }
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCorePhase()
                {
                    _Singleton = new CacheCorePhase();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 20, isPK: true),
                        new PopupColumnSetting("Description", columnWidth: 40),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ID, Description, UsedIEQuotation, IsBulk, ApplyPattern, ApplyMarker, ApplyIE From Phase Order by ID";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Phase";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }
            }
        }

        namespace Style
        {
            /// <summary>
            /// 可受Brand限制的Style選擇器
            /// </summary>
            public class PickIdBoxStyle : BasePickIdBox<string, string>
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxStyle()
                    : base()
                {
                    base.DefaultCacheCore = CacheCoreStyle.Singleton;
                    base.PopupSettings = CacheCoreStyle.DefaultColumnSetting;
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(true, "BrandID"));
                }
            }

            /// <summary>
            /// Style CacheCore
            /// </summary>
            public class CacheCoreStyle : BaseDataCacheCore<string, string>
            {
                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreStyle Singleton { get; private set; }

                /// <summary>
                /// Style CacheCore
                /// </summary>
                static CacheCoreStyle()
                {
                    Singleton = new CacheCoreStyle();
                    DefaultColumnSetting = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("BrandID", headerText: "Brand", columnWidth: 9),
                        new PopupColumnSetting("Id", headerText: "Style", isPK: true, columnWidth: 16),
                        new PopupColumnSetting("SeasonID", headerText: "Season", columnWidth: 12),
                        new PopupColumnSetting("Description", columnWidth: 30),
                        new PopupColumnSetting("CdCodeID", headerText: "CD Code", columnWidth: 7),
                        new PopupColumnSetting("UKey", headerText: "UKey", columnWidth: 0),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select UKey, BrandID, Id, SeasonID, Description, CdCodeID From Style Where IsNull(Junk, 0) = 0";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Style";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["UKey"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSetting { get; set; }
            }
        }

        namespace Season
        {
            /// <summary>
            /// PickIdBoxSeason
            /// </summary>
            public class PickIdBoxSeason : BasePickIdBox<string, string>
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickIdBoxSeason")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                /// <summary>
                /// Season PickBox
                /// </summary>
                public PickIdBoxSeason()
                {
                    if (Env.DesignTime)
                        return;
                    base.PopupSettings = CacheCoreSeason.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreSeason.Singleton;
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(true, "BrandID"));
                    base.DataFilterSetting.TextToPKArray = (text) => new object[] { this.BrandFilter.Value, text };
                }
            }

            /// <summary>
            /// Supplier MultiPickBox
            /// </summary>
            public class MultiPickIdBoxSeason : BaseMultiplePickIdBox<string, string>
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickIdBoxSeason")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                /// <summary>
                /// Supplier MultiPickBox
                /// </summary>
                public MultiPickIdBoxSeason()
                    : base()
                {
                    base.PopupSettings = CacheCoreSeason.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreSeason.Singleton;
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(true, "BrandID"));
                    base.DataFilterSetting.TextToPKArray = text => new object[] { this.BrandFilter.Value, text };
                }
            }

            /// <summary>
            /// Season CacheCore
            /// </summary>
            public class CacheCoreSeason : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 10;

                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreSeason Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreSeason()
                {
                    Singleton = new CacheCoreSeason();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("BrandID", headerText: "Brand", columnWidth: 9),
                        new PopupColumnSetting("Id", headerText: "Season", isPK: true, columnWidth: 16),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select BrandID, Id, Junk From Season Order by BrandID, ID";

                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Season";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["BrandID"],
                        data.Columns["Id"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Id");
                }
            }
        }

        namespace Color
        {
            /// <summary>
            /// PickIdBoxColor
            /// </summary>
            public class PickIdBoxColor : BasePickIdBox<string[], string>, IJunkEffectiveSwitch
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                #region IJunkEffectiveSwitch

                /// <summary>
                /// 是否在開窗選擇的時候，要顯示Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxColor")]
                [Description("是否在開窗選擇的時候，要顯示Junk的資料")]
                public bool? DialogIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.DialogIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.DialogIncludeJunked = value;
                    }
                }

                /// <summary>
                /// 是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxColor")]
                [Description("是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料")]
                public bool? ValidatingIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.ValidatingIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.ValidatingIncludeJunked = value;
                    }
                }

                #endregion

                /// <summary>
                /// Color PickBox
                /// </summary>
                public PickIdBoxColor()
                {
                    if (Env.DesignTime)
                        return;
                    base.PopupSettings = CacheCoreColor.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreColor.Singleton;
                    base.DataFilterSetting.DialogIncludeJunked = false;
                    base.DataFilterSetting.ValidatingIncludeJunked = false;
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(true, "BrandID"));
                    base.DataFilterSetting.TextToPKArray = (text) => new object[] { this.BrandFilter.Value, text };
                }
            }

            /// <summary>
            /// Color PickBox
            /// </summary>
            public class PickLookupBoxColor : BasePickLookupBox<string[], string>, IJunkEffectiveSwitch
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                #region IJunkEffectiveSwitch

                /// <summary>
                /// 是否在開窗選擇的時候，要顯示Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxColor")]
                [Description("是否在開窗選擇的時候，要顯示Junk的資料")]
                public bool? DialogIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.DialogIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.DialogIncludeJunked = value;
                    }
                }

                /// <summary>
                /// 是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxColor")]
                [Description("是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料")]
                public bool? ValidatingIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.ValidatingIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.ValidatingIncludeJunked = value;
                    }
                }

                #endregion

                /// <summary>
                /// 
                /// </summary>
                public PickLookupBoxColor()
                    : base()
                {
                    base.PopupSettings = CacheCoreColor.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreColor.Singleton;
                    base.SetInputMaxLengthAndWidth(CacheCoreColor.IDMaxLength);
                    base.DataFilterSetting.DialogIncludeJunked = false;
                    base.DataFilterSetting.ValidatingIncludeJunked = false;
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(true, "BrandID"));
                    base.DataFilterSetting.TextToPKArray = (text) => new object[] { this.BrandFilter.Value, text };
                }
            }

            /// <summary>
            /// Supplier MultiPickBox
            /// </summary>
            public class MultiPickIdBoxColor : BaseMultiplePickIdBox<string[], string>, IJunkEffectiveSwitch
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                #region IJunkEffectiveSwitch

                /// <summary>
                /// 是否在開窗選擇的時候，要顯示Junk的資料
                /// </summary>
                [Category("SCIC-MultiPickIdBoxColor")]
                [Description("是否在開窗選擇的時候，要顯示Junk的資料")]
                public bool? DialogIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.DialogIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.DialogIncludeJunked = value;
                    }
                }

                /// <summary>
                /// 是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料
                /// </summary>
                [Category("SCIC-MultiPickIdBoxColor")]
                [Description("是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料")]
                public bool? ValidatingIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.ValidatingIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.ValidatingIncludeJunked = value;
                    }
                }

                #endregion

                /// <summary>
                /// Supplier MultiPickBox
                /// </summary>
                public MultiPickIdBoxColor()
                    : base()
                {
                    base.PopupSettings = CacheCoreColor.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreColor.Singleton;
                    base.DataFilterSetting.DialogIncludeJunked = false;
                    base.DataFilterSetting.ValidatingIncludeJunked = true;
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(true, "BrandID"));
                    base.DataFilterSetting.TextToPKArray = text => new object[] { this.BrandFilter.Value, text };
                }
            }

            /// <summary>
            /// Color GridCellSetting
            /// </summary>
            public class GridCellSettingColor : BaseGridCellSetting<string[], string>
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-GridCellSettingColor")]
                [Description("用於LoadCache的篩選條件")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                /// <summary>
                /// Factory GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="setting"></param>
                public GridCellSettingColor(string propertyName, PopupColumnSettings setting = null)
                    : base(propertyName, setting ?? CacheCoreColor.DefaultColumnSettings)
                {
                    base.DefaultCacheCore = CacheCoreColor.Singleton;
                    base.MaxLength = CacheCoreColor.IDMaxLength;
                    base.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(true, "BrandID"));
                    base.DataFilterSetting.TextToPKArray = (text) => new object[] { this.BrandFilter.Value, text };
                }
            }

            /// <summary>
            /// Color CacheCore
            /// </summary>
            public class CacheCoreColor : BaseDataCacheCore<string[], string>
            {
                internal const int IDMaxLength = 6;

                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreColor Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreColor()
                {
                    Singleton = new CacheCoreColor();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("BrandID", headerText: "Brand", columnWidth: 9),
                        new PopupColumnSetting("Id", headerText: "Color", isPK: true, columnWidth: 7),
                        new PopupColumnSetting("Name", headerText: "Name", columnWidth: 30),
                        new PopupColumnSetting("UKey", headerText: "", columnWidth: 0),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select UKey, BrandID, Id, Name, varicolored, Junk From Color Order by BrandID, ID";

                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Color";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["BrandID"],
                        data.Columns["Id"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Name");
                }
            }
        }

        namespace SuppContact
        {
            /// <summary>
            /// PickIdBoxSuppContact
            /// </summary>
            public class PickIdBoxSuppContact : BasePickIdBox<string, string>
            {
                #region Restrictor - Supp

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickIdBoxSuppContact")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> SuppFilter { get; set; }

                #endregion

                /// <summary>
                /// SuppContact PickBox
                /// </summary>
                public PickIdBoxSuppContact()
                {
                    if (Env.DesignTime)
                        return;
                    base.PopupSettings = CacheCoreSuppContact.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreSuppContact.Singleton;
                    this.DataFilterSetting.Items.Add(this.SuppFilter = new CacheCoreDataFilter<string>(true, "ID"));
                    //this.DataFilterSetting.TextToPKArray = text => new object[] { this.SuppFilter.Value, text };
                }
            }

            /// <summary>
            /// SuppContact PickBox
            /// </summary>
            public class PickLookupBoxSuppContact : BasePickLookupBox<string, string>
            {
                #region Restrictor - Supp

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> SuppFilter { get; set; }

                #endregion

                /// <summary>
                /// 
                /// </summary>
                public PickLookupBoxSuppContact()
                    : base()
                {
                    base.PopupSettings = CacheCoreSuppContact.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreSuppContact.Singleton;
                    base.SetInputMaxLengthAndWidth(CacheCoreSuppContact.IDMaxLength);
                    this.DataFilterSetting.Items.Add(this.SuppFilter = new CacheCoreDataFilter<string>(true, "ID"));
                    //this.DataFilterSetting.TextToPKArray = text => new object[] { this.SuppFilter.Value, text };
                }
            }

            /// <summary>
            /// Supplier MultiPickBox
            /// </summary>
            public class MultiPickIdBoxSuppContact : BaseMultiplePickIdBox<string, string>
            {
                #region Restrictor - Supp

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> SuppFilter { get; set; }

                #endregion

                /// <summary>
                /// Supplier MultiPickBox
                /// </summary>
                public MultiPickIdBoxSuppContact()
                    : base()
                {
                    base.PopupSettings = CacheCoreSuppContact.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreSuppContact.Singleton;
                    this.DataFilterSetting.Items.Add(this.SuppFilter = new CacheCoreDataFilter<string>(true, "ID"));
                    //this.DataFilterSetting.TextToPKArray = text => new object[] { this.SuppFilter.Value, text };
                }
            }

            /// <summary>
            /// SuppContact GridCellSetting
            /// </summary>
            public class GridCellSettingSuppContact : BaseGridCellSetting
            {
                #region Restrictor - Supp

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-GridCellSettingSuppContact")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> SuppFilter { get; set; }

                #endregion

                /// <summary>
                /// SuppContact GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="suppIdGetter"></param>
                public GridCellSettingSuppContact(string propertyName, Func<string> suppIdGetter)
                    : base(propertyName, CacheCoreSuppContact.DefaultColumnSettings)
                {
                    base.DefaultCacheCore = CacheCoreSuppContact.Singleton;
                    base.MaxLength = CacheCoreSuppContact.IDMaxLength;
                    this.DataFilterSetting.Items.Add(this.SuppFilter = new CacheCoreDataFilter<string>(true, "ID"));
                    this.SuppFilter.DynamicValue = suppIdGetter;
                }
            }

            /// <summary>
            /// SuppContact CacheCore
            /// </summary>
            public class CacheCoreSuppContact : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 20;

                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreSuppContact Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreSuppContact()
                {
                    Singleton = new CacheCoreSuppContact();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", headerText: "Supp", columnWidth: 6),
                        new PopupColumnSetting("BrandID", headerText: "Brand", columnWidth: 8),
                        new PopupColumnSetting("Contact", headerText: "Contact", isPK: true, columnWidth: 20),
                        new PopupColumnSetting("Tel", headerText: "Tel.", columnWidth: 20),
                        new PopupColumnSetting("UKey", headerText: "", columnWidth: 0),
                    }.ToList());
                    DefaultColumnSettings.UsingUKeyForPopup = true;
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"
Select UKey, ID, Contact, BrandID, SuppDepartment, TEL, SCIDepartment, Email
From Supp_Contact
Order BY BrandID, SuppDepartment, Contact";

                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Supp_Contact";
                    data.PrimaryKey = new[]
                    {
                        //data.Columns["ID"],
                        //data.Columns["Contact"],
                        data.Columns["UKey"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("TEL");
                }
            }
        }

        namespace Unit
        {
            /// <summary>
            /// PickIdBoxUnit
            /// </summary>
            public class PickIdBoxUnit : BasePickIdBox<string[], string>
            {
                /// <summary>
                /// Unit PickBox
                /// </summary>
                public PickIdBoxUnit()
                {
                    if (Env.DesignTime)
                        return;
                    base.PopupSettings = CacheCoreUnit.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreUnit.Singleton;
                }
            }

            /// <summary>
            /// Unit PickBox
            /// </summary>
            public class PickLookupBoxUnit : BasePickLookupBox<string[], string>
            {
                /// <summary>
                /// 
                /// </summary>
                public PickLookupBoxUnit()
                    : base()
                {
                    base.PopupSettings = CacheCoreUnit.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreUnit.Singleton;
                    base.SetInputMaxLengthAndWidth(CacheCoreUnit.IDMaxLength);
                }
            }

            /// <summary>
            /// Supplier MultiPickBox
            /// </summary>
            public class MultiPickIdBoxUnit : BaseMultiplePickIdBox<string[], string>
            {
                /// <summary>
                /// Supplier MultiPickBox
                /// </summary>
                public MultiPickIdBoxUnit()
                    : base()
                {
                    base.PopupSettings = CacheCoreUnit.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreUnit.Singleton;
                }
            }

            /// <summary>
            /// Unit CacheCore
            /// </summary>
            public class CacheCoreUnit : BaseDataCacheCore<string[], string>
            {
                internal const int IDMaxLength = 8;

                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreUnit Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreUnit()
                {
                    Singleton = new CacheCoreUnit();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", headerText: "ID", isPK: true, columnWidth: 0),
                        new PopupColumnSetting("Description", headerText: "Description", columnWidth: 30),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ID, Description From Unit Order by ID";

                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Unit";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }
            }
        }

        namespace StyleSizeItem
        {
            using nsStyle = Sci.Production.Class.CustomizeControl.Style;

            /// <summary>
            /// 可受Brand限制的Style選擇器
            /// </summary>
            public class PickIdBoxStyleSizeItem : BasePickIdBox<string, string>
            {
                #region Restrictor - StyleUKey

                /// <summary>
                /// [不影響CustomizeCacheCore]用於代替限制器的純值，如果此屬性不為null，則使用RestrictorBrandID來判定限制值
                /// </summary>
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public long? FixedStyleUKey
                {
                    get
                    {
                        if (this.CustomizeCacheCore == null)
                            return this.MyCacheCore.StyleUKey;
                        else
                            return null;
                    }
                    set
                    {
                        if (this.CustomizeCacheCore == null)
                            this.MyCacheCore.StyleUKey = value;
                    }
                }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyleSizeItem
                /// </summary>
                private CacheCoreStyleSizeItem MyCacheCore
                {
                    get
                    {
                        return (CacheCoreStyleSizeItem)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxStyleSizeItem()
                    : base()
                {
                    this.MyCacheCore = new CacheCoreStyleSizeItem();
                    base.PopupSettings = this.MyCacheCore.ColumnSetting;
                }
            }

            /// <summary>
            /// PickLookupBoxStyleSizeItem
            /// </summary>
            public class PickLookupBoxStyleSizeItem : BasePickLookupBox<string, string>
            {
                #region Restrictor - StyleUKey

                /// <summary>
                /// [不影響CustomizeCacheCore]用於代替限制器的純值，如果此屬性不為null，則使用RestrictorBrandID來判定限制值
                /// </summary>
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public long? FixedStyleUKey
                {
                    get
                    {
                        if (this.CustomizeCacheCore == null)
                            return this.MyCacheCore.StyleUKey;
                        else
                            return null;
                    }
                    set
                    {
                        if (this.CustomizeCacheCore == null)
                            this.MyCacheCore.StyleUKey = value;
                    }
                }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyleSizeItem
                /// </summary>
                private CacheCoreStyleSizeItem MyCacheCore
                {
                    get
                    {
                        return (CacheCoreStyleSizeItem)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickLookupBoxStyleSizeItem()
                    : base()
                {
                    this.MyCacheCore = new CacheCoreStyleSizeItem();
                    base.PopupSettings = this.MyCacheCore.ColumnSetting;
                }
            }

            /// <summary>
            /// StyleSizeItem CacheCore
            /// </summary>
            public class CacheCoreStyleSizeItem : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 3;

                private long? _StyleUKeyFilter;
                /// <summary>
                /// 如果此屬性不為null，則會被用於Where條件子句來限制抓取資料
                /// </summary>
                public long? StyleUKey
                {
                    get { return _StyleUKeyFilter; }
                    set
                    {
                        this.ClearCache();
                        _StyleUKeyFilter = value;
                    }
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal PopupColumnSettings ColumnSetting { get; set; }

                /// <summary>
                /// StyleSizeItem CacheCore
                /// </summary>
                public CacheCoreStyleSizeItem()
                {
                    this.ColumnSetting = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("SizeItem", headerText: "SizeItem", columnWidth: 4, isPK: true),
                        new PopupColumnSetting("SizeUnit", headerText: "SizeUnit", columnWidth: 9),
                        new PopupColumnSetting("Description", headerText: "Description", columnWidth: 30),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    if (this.StyleUKey.HasValue)
                    {
                        var sql = @"Select SizeItem, SizeUnit, Description From Style_SizeItem Where StyleUKey = @StyleUKey Order By SizeItem";
                        var p = new[] { new SqlParameter("StyleUKey", this.StyleUKey.Value) };
                        DataTable data;
                        SQL.Select("", sql, out data, p);
                        data.TableName = "Style_SizeItem";
                        data.PrimaryKey = new[]
                        {
                            data.Columns["SizeItem"],
                        };
                        return data;
                    }
                    else
                    {
                        return null; //這是一定要選StyleUKey才能用的功能
                    }
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }
            }
        }

        namespace Order
        {
            /// <summary>
            /// PickIdBoxOrder
            /// </summary>
            public class PickIdBoxOrder : BasePickIdBox<string, string>, IJunkEffectiveSwitch
            {
                #region Restrictor - StyleUKey

                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickIdBoxOrder")]
                [Description("用於LoadCache的篩選條件")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<long?> StyleUKeyFilter { get; set; }

                #endregion

                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickIdBoxOrder")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                #region IJunkEffectiveSwitch

                /// <summary>
                /// 是否在開窗選擇的時候，要顯示Junk的資料
                /// </summary>
                [Category("SCIC-PickIdBoxOrder")]
                [Description("是否在開窗選擇的時候，要顯示Junk的資料")]
                public bool? DialogIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.DialogIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.DialogIncludeJunked = value;
                    }
                }

                /// <summary>
                /// 是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料
                /// </summary>
                [Category("SCIC-PickIdBoxOrder")]
                [Description("是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料")]
                public bool? ValidatingIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.ValidatingIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.ValidatingIncludeJunked = value;
                    }
                }

                #endregion

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxOrder()
                    : base()
                {
                    base.PopupSettings = CacheCoreOrder.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreOrder.Singleton;
                    this.DataFilterSetting.Items.Add(this.StyleUKeyFilter = new CacheCoreDataFilter<long?>(false, "StyleUKey"));
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(false, "BrandID"));
                }
            }

            /// <summary>
            /// Order PickBox
            /// </summary>
            public class PickLookupBoxOrder : BasePickLookupBox<string, string>, IJunkEffectiveSwitch
            {
                #region Restrictor - StyleUKey

                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickLookupBoxOrder")]
                [Description("用於LoadCache的篩選條件")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<long?> StyleUKeyFilter { get; set; }

                #endregion

                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxOrder")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                #region IJunkEffectiveSwitch

                /// <summary>
                /// 是否在開窗選擇的時候，要顯示Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxOrder")]
                [Description("是否在開窗選擇的時候，要顯示Junk的資料")]
                public bool? DialogIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.DialogIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.DialogIncludeJunked = value;
                    }
                }

                /// <summary>
                /// 是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料
                /// </summary>
                [Category("SCIC-PickLookupBoxOrder")]
                [Description("是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料")]
                public bool? ValidatingIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.ValidatingIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.ValidatingIncludeJunked = value;
                    }
                }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore&lt;string, string&gt;) => CacheCoreOrder
                /// </summary>
                private CacheCoreOrder MyCacheCore
                {
                    get
                    {
                        return (CacheCoreOrder)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Order PickBox
                /// </summary>
                public PickLookupBoxOrder()
                    : base()
                {
                    base.PopupSettings = CacheCoreOrder.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreOrder.Singleton;
                    base.SetInputMaxLengthAndWidth(CacheCoreOrder.IDMaxLength);
                    base.DataFilterSetting.Items.Add(this.StyleUKeyFilter = new CacheCoreDataFilter<long?>(false, "StyleUKey"));
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(false, "BrandID"));
                }
            }

            /// <summary>
            /// Order MultiPickBox
            /// </summary>
            public class MultiPickIdOrder : BaseMultiplePickIdBox<string, string>, IJunkEffectiveSwitch
            {
                #region Restrictor - StyleUKey

                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-MultiPickIdOrder")]
                [Description("用於LoadCache的篩選條件")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<long?> StyleUKeyFilter { get; set; }

                #endregion

                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-MultiPickIdOrder")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                #region IJunkEffectiveSwitch

                /// <summary>
                /// 是否在開窗選擇的時候，要顯示Junk的資料
                /// </summary>
                [Category("SCIC-MultiPickIdOrder")]
                [Description("是否在開窗選擇的時候，要顯示Junk的資料")]
                public bool? DialogIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.DialogIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.DialogIncludeJunked = value;
                    }
                }

                /// <summary>
                /// 是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料
                /// </summary>
                [Category("SCIC-MultiPickIdOrder")]
                [Description("是否在直接文字方塊內打字，並且進行Validating的時候，要允許Junk的資料")]
                public bool? ValidatingIncludeJunked
                {
                    get
                    {
                        return base.DataFilterSetting.ValidatingIncludeJunked;
                    }
                    set
                    {
                        base.DataFilterSetting.ValidatingIncludeJunked = value;
                    }
                }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreOrder_StyleRestricted
                /// </summary>
                private CacheCoreOrder MyCacheCore
                {
                    get
                    {
                        return (CacheCoreOrder)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Order MultiPickBox
                /// </summary>
                public MultiPickIdOrder()
                {
                    base.PopupSettings = CacheCoreOrder.DefaultColumnSettings;
                    base.DefaultCacheCore = CacheCoreOrder.Singleton;
                    base.DataFilterSetting.Items.Add(this.StyleUKeyFilter = new CacheCoreDataFilter<long?>(false, "StyleUKey"));
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(false, "BrandID"));
                }
            }

            /// <summary>
            /// Order CacheCore
            /// </summary>
            public class CacheCoreOrder : BaseDataCacheCore<string, string>
            {
                /// <summary>
                /// 
                /// </summary>
                public static int IDMaxLength = 13;

                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreOrder Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// Order CacheCore
                /// </summary>
                static CacheCoreOrder()
                {
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", isPK: true, columnWidth: 10),
                        new PopupColumnSetting("POID", columnWidth: 10),
                        new PopupColumnSetting("FactoryID", headerText:"Factory", columnWidth: 10),
                        new PopupColumnSetting("MRHandle", headerText:"MR", columnWidth: 10),
                        new PopupColumnSetting("SMR", columnWidth: 10),
                    }.ToList());
                    Singleton = new CacheCoreOrder();
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ID, FactoryID, POID, MRHandle, SMR, StyleUKey, Junk From Orders";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Orders";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用單一主鍵值換別的欄位值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("ID"); //Order沒有Name也沒有Description...不知道要換啥
                }
            }
        }

        namespace MmsBrand
        {
            /// <summary>
            /// PickIdBoxMmsBrand
            /// </summary>
            public class PickIdBoxMmsBrand : BasePickIdBox<string, string>
            {
                #region Restrictor - Type

                /// <summary>
                /// [不影響CustomizeCacheCore]用於代替限制器的純值，如果此屬性不為null，則使用RestrictorBrandID來判定限制值
                /// </summary>
                [Category("SCIC-PickIdBoxMmsBrand")]
                public string FixedMmsBrandTypeID
                {
                    get
                    {
                        if (this.CustomizeCacheCore == null)
                            return this.MyCacheCore.MmsBrandTypeID;
                        else
                            return null;
                    }
                    set
                    {
                        if (this.CustomizeCacheCore == null)
                            this.MyCacheCore.MmsBrandTypeID = value;
                    }
                }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreMmsBrand MyCacheCore
                {
                    get
                    {
                        return (CacheCoreMmsBrand)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxMmsBrand()
                    : base()
                {
                    base.PopupSettings = CacheCoreMmsBrand.DefaultColumnSettings;
                    this.MyCacheCore = new CacheCoreMmsBrand();
                }
            }

            /// <summary>
            /// MmsBrand GridCellSetting
            /// </summary>
            public class GridCellSettingMmsBrand : BaseGridCellSetting
            {
                #region Restrictor - Type

                /// <summary>
                /// [不影響CustomizeCacheCore]用於代替限制器的純值，如果此屬性不為null，則使用RestrictorBrandID來判定限制值
                /// </summary>
                [Category("SCIC-PickIdBoxMmsBrand")]
                public string FixedMmsBrandTypeID
                {
                    get
                    {
                        if (this.CustomizeCacheCore == null)
                            return this.MyCacheCore.MmsBrandTypeID;
                        else
                            return null;
                    }
                    set
                    {
                        if (this.CustomizeCacheCore == null)
                            this.MyCacheCore.MmsBrandTypeID = value;
                    }
                }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreMmsBrand MyCacheCore
                {
                    get
                    {
                        return (CacheCoreMmsBrand)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// MmsBrand GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="type"></param>
                public GridCellSettingMmsBrand(string propertyName, string type)
                    : this(propertyName, type, CacheCoreMmsBrand.DefaultColumnSettings)
                {
                    base.MaxLength = CacheCoreMmsBrand.IDMaxLength;
                    this.MyCacheCore.MmsBrandTypeID = type;
                }

                /// <summary>
                /// MmsBrand GridCellSetting
                /// </summary>
                /// <param name="propertyName"></param>
                /// <param name="type"></param>
                /// <param name="setting"></param>
                public GridCellSettingMmsBrand(string propertyName, string type, PopupColumnSettings setting)
                    : base(propertyName, setting)
                {
                    this.DefaultCacheCore = new CacheCoreMmsBrand();
                    base.MaxLength = CacheCoreMmsBrand.IDMaxLength;
                }
            }

            /// <summary>
            /// MmsBrand PickBox
            /// </summary>
            public class PickLookupBoxMmsBrand : BasePickLookupBox<string, string>
            {
                #region Restrictor - Type

                /// <summary>
                /// [不影響CustomizeCacheCore]用於代替限制器的純值，如果此屬性不為null，則使用FixedMmsBrandTypeID來判定限制值
                /// </summary>
                [Browsable(true)]
                [Category("SCIC-PickLookupBoxMmsBrand")]
                [Description("設定查找範圍的類型")]
                public string FixedMmsBrandTypeID
                {
                    get
                    {
                        if (this.CustomizeCacheCore == null)
                            return this.MyCacheCore.MmsBrandTypeID;
                        else
                            return null;
                    }
                    set
                    {
                        if (this.CustomizeCacheCore == null)
                            this.MyCacheCore.MmsBrandTypeID = value;
                    }
                }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore&lt;string, string&gt;) => CacheCoreMmsBrand
                /// </summary>
                private CacheCoreMmsBrand MyCacheCore
                {
                    get
                    {
                        return (CacheCoreMmsBrand)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// MmsBrand PickBox
                /// </summary>
                public PickLookupBoxMmsBrand()
                    : base()
                {
                    base.PopupSettings = CacheCoreMmsBrand.DefaultColumnSettings;
                    this.DefaultCacheCore = new CacheCoreMmsBrand();
                    this.SetInputMaxLengthAndWidth(CacheCoreMmsBrand.IDMaxLength);
                }
            }

            /// <summary>
            /// MmsBrand MultiPickBox
            /// </summary>
            public class MultiPickIdMmsBrand : BaseMultiplePickIdBox<string, string>
            {
                #region Restrictor - Type

                /// <summary>
                /// 
                /// </summary>
                [Category("SCIC-MultiPickIdMmsBrand")]
                [Description("要用來篩選MmsBrand的種類文字")]
                public string FixedMmsBrandTypeID
                {
                    get
                    {
                        if (this.CustomizeCacheCore == null)
                            return this.MyCacheCore.MmsBrandTypeID;
                        else
                            return null;
                    }
                    set
                    {
                        if (this.CustomizeCacheCore == null)
                            this.MyCacheCore.MmsBrandTypeID = value;
                    }
                }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreStyle_BrandRestricted
                /// </summary>
                private CacheCoreMmsBrand MyCacheCore
                {
                    get
                    {
                        return (CacheCoreMmsBrand)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// MmsBrand MultiPickBox
                /// </summary>
                public MultiPickIdMmsBrand()
                {
                    base.PopupSettings = CacheCoreMmsBrand.DefaultColumnSettings;
                    this.DefaultCacheCore = new CacheCoreMmsBrand();
                }
            }

            /// <summary>
            /// MmsBrand CacheCore
            /// </summary>
            public class CacheCoreMmsBrand : BaseDataCacheCore<string, string>
            {
                /// <summary>
                /// 
                /// </summary>
                public static int IDMaxLength = 10;

                private string _MmsBrandTypeIDFilter;
                /// <summary>
                /// 如果此屬性不為null，則會被用於Where條件子句來限制抓取資料
                /// </summary>
                public string MmsBrandTypeID
                {
                    get { return _MmsBrandTypeIDFilter; }
                    set
                    {
                        this.ClearCache();
                        _MmsBrandTypeIDFilter = value;
                    }
                }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// MmsBrand CacheCore
                /// </summary>
                static CacheCoreMmsBrand()
                {
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", isPK: true, columnWidth: 10),
                        new PopupColumnSetting("Name", columnWidth: 50),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    if (string.IsNullOrEmpty(this.MmsBrandTypeID))
                        throw new ArgumentException("請先設定MultiPickIdMmsBrand.FixedMmsBrandTypeID屬性");
                    var sql = @"Select ID, Name From MmsBrand Where Type = @MmsBrandTypeID";
                    var p = new[] { new SqlParameter("MmsBrandTypeID", this.MmsBrandTypeID) };
                    DataTable data;
                    SQL.Select("", sql, out data, p);
                    data.TableName = "MmsBrand";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用找到的DataRow取出一個要用於顯示名稱欄位的值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Name");
                }
            }
        }

        namespace User
        {
            /// <summary>
            /// User PickBox
            /// </summary>
            public class PickLookupBoxUser : BasePickLookupBox<string, string>
            {
                //#region Restrictor - SystemType
                //private TxtUser.SystemTypeList? _SystemType;
                ///// <summary>
                ///// 用於LoadCache的篩選條件
                ///// </summary>
                //[Category("SCIC-PickLookupBoxUser")]
                ////public TxtUser.SystemTypeList? SystemType
                ////{
                ////    get
                ////    {
                ////        return this._SystemType;
                ////    }
                ////    set
                ////    {
                ////        this._SystemType = value;
                ////        switch (value.GetValueOrDefault(TxtUser.SystemTypeList.All))
                ////        {
                ////            case TxtUser.SystemTypeList.All:
                ////                base.DataFilterSetting.Predictor = (row) => true;
                ////                break;
                ////            case TxtUser.SystemTypeList.Production:
                ////                base.DataFilterSetting.Predictor = (row) => row.Field<bool?>("Production").GetValueOrDefault(false) == true;
                ////                break;
                ////            case TxtUser.SystemTypeList.Sample:
                ////                base.DataFilterSetting.Predictor = (row) => row.Field<bool?>("Sample").GetValueOrDefault(false) == true;
                ////                break;
                ////            case TxtUser.SystemTypeList.Accounting:
                ////                base.DataFilterSetting.Predictor = (row) => row.Field<bool?>("Accounting").GetValueOrDefault(false) == true;
                ////                break;
                ////            case TxtUser.SystemTypeList.FromFactory:
                ////                base.DataFilterSetting.Predictor = (row) => row.Field<bool?>("FromFactory").GetValueOrDefault(false) == true;
                ////                break;
                ////            default:
                ////                break;
                ////        }
                ////    }
                ////}
                //#endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreUser
                /// </summary>
                private CacheCoreUser MyCacheCore
                {
                    get
                    {
                        return (CacheCoreUser)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// User PickBox
                /// </summary>
                public PickLookupBoxUser()
                    : base()
                {
                    base.PopupSettings = CacheCoreUser.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreUser.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreUser.IDMaxLength);
                }
            }

            /// <summary>
            /// User CacheCore
            /// </summary>
            public class CacheCoreUser : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 10;

                /// <summary>
                /// 獨體
                /// </summary>
                internal static CacheCoreUser Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreUser()
                {
                    Singleton = new CacheCoreUser();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("ID", columnWidth: 32, isPK: true),
                        new PopupColumnSetting("Name", columnWidth: 40),
                        new PopupColumnSetting("ExtNo", columnWidth: 6),
                        new PopupColumnSetting("Factory", columnWidth: 6),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select ID, Name, ExtNo, EMail, Factory, Production, Sample, Accounting, FromFactory From Pass1 Where Resign is null order by ID";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Pass1";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["ID"],
                    };
                    return data;
                }

                /// <summary>
                /// 用找到的DataRow取出一個要用於顯示名稱欄位的值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Name");
                }
            }
        }

        //複合主鍵+二階篩選
        //PickIdBoxPhrase
        //PickLookupBoxPhrase
        namespace Phrase
        {
            /// <summary>
            /// 
            /// </summary>
            public enum PhraseTypeEnum
            {
                /// <summary>
                /// 
                /// </summary>
                大貨出口嘜頭,
                /// <summary>
                /// 
                /// </summary>
                物料出口片語檔,
                /// <summary>
                /// 
                /// </summary>
                物料出口交貨地點_櫃,
                /// <summary>
                /// 
                /// </summary>
                物料出口嘜頭,
                /// <summary>
                /// 
                /// </summary>
                採購常用片語檔,
                /// <summary>
                /// 
                /// </summary>
                採購嘜頭
            }

            /// <summary>
            /// PickIdBoxPhrase
            /// </summary>
            public class PickIdBoxPhrase : BasePickIdBox<string[], string>
            {
                #region Restrictor - SystemType
                private PhraseTypeEnum? _PhraseType;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickIdBoxPhrase")]
                public PhraseTypeEnum? PhraseType
                {
                    get
                    {
                        return this._PhraseType;
                    }
                    set
                    {
                        if (this._PhraseType != value)
                        {
                            this._PhraseType = value;
                            if (Env.DesignTime)
                                return;
                            switch (value.GetValueOrDefault(PhraseTypeEnum.物料出口嘜頭))
                            {
                                case PhraseTypeEnum.大貨出口嘜頭:
                                case PhraseTypeEnum.物料出口片語檔:
                                case PhraseTypeEnum.採購嘜頭:
                                case PhraseTypeEnum.物料出口嘜頭:
                                    {
                                        var phraseTypeName = value.GetValueOrDefault(PhraseTypeEnum.物料出口嘜頭).ToString();
                                        base.DataFilterSetting.Predictor = (row) => row.Field<string>("PhraseTypeName") == phraseTypeName;
                                        base.DataFilterSetting.TextToPKArray = (text) => new object[] { phraseTypeName, text };
                                        break;
                                    }
                                case PhraseTypeEnum.物料出口交貨地點_櫃:
                                    {
                                        var phraseTypeName = "物料出口交貨地點/櫃";
                                        base.DataFilterSetting.Predictor = (row) => row.Field<string>("PhraseTypeName") == phraseTypeName; //這比較特別，因為她有斜線，沒辦法打在enum的項目名稱上，只好這邊獨立加工
                                        base.DataFilterSetting.TextToPKArray = (text) => new object[] { phraseTypeName, text };
                                        break;
                                    }
                                default:
                                    throw new NotImplementedException();
                            }
                            base.AutoCacheCore.ClearCache();
                            base.ReValidate();
                        }
                    }
                }
                #endregion

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxPhrase()
                    : base()
                {
                    base.PopupSettings = CacheCorePhrase.DefaultColumnSettings;
                    this.DefaultCacheCore = new CacheCorePhrase();
                    this.MaxLength = CacheCorePhrase.IDMaxLength;
                }
            }

            /// <summary>
            /// Phrase PickBox
            /// </summary>
            public class PickLookupBoxPhrase : BasePickLookupBox<string[], string>
            {
                #region Restrictor - SystemType
                private PhraseTypeEnum? _PhraseType;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickLookupBoxPhrase")]
                public PhraseTypeEnum? PhraseType
                {
                    get
                    {
                        return this._PhraseType;
                    }
                    set
                    {
                        if (this._PhraseType != value)
                        {
                            this._PhraseType = value;
                            if (Env.DesignTime)
                                return;
                            switch (value.GetValueOrDefault(PhraseTypeEnum.物料出口嘜頭))
                            {
                                case PhraseTypeEnum.大貨出口嘜頭:
                                case PhraseTypeEnum.物料出口片語檔:
                                case PhraseTypeEnum.採購嘜頭:
                                case PhraseTypeEnum.物料出口嘜頭:
                                    {
                                        var phraseTypeName = value.GetValueOrDefault(PhraseTypeEnum.物料出口嘜頭).ToString();
                                        base.DataFilterSetting.Predictor = (row) => row.Field<string>("PhraseTypeName") == phraseTypeName;
                                        base.DataFilterSetting.TextToPKArray = (text) => new object[] { phraseTypeName, text };
                                        break;
                                    }
                                case PhraseTypeEnum.物料出口交貨地點_櫃:
                                    {
                                        var phraseTypeName = "物料出口交貨地點/櫃";
                                        base.DataFilterSetting.Predictor = (row) => row.Field<string>("PhraseTypeName") == phraseTypeName; //這比較特別，因為她有斜線，沒辦法打在enum的項目名稱上，只好這邊獨立加工
                                        base.DataFilterSetting.TextToPKArray = (text) => new object[] { phraseTypeName, text };
                                        break;
                                    }
                                default:
                                    throw new NotImplementedException();
                            }
                            base.AutoCacheCore.ClearCache();
                            base.ReValidate();
                        }
                        this._PhraseType = value;

                    }
                }
                #endregion

                /// <summary>
                /// Phrase PickBox
                /// </summary>
                public PickLookupBoxPhrase()
                    : base()
                {
                    base.PopupSettings = CacheCorePhrase.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCorePhrase.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCorePhrase.IDMaxLength);
                }
            }

            /// <summary>
            /// Phrase CacheCore
            /// </summary>
            public class CacheCorePhrase : BaseDataCacheCore<string[], string>
            {
                internal const int IDMaxLength = 10;

                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCorePhrase Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCorePhrase()
                {
                    Singleton = new CacheCorePhrase();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("Seq", columnWidth: 10),
                        new PopupColumnSetting("Name", columnWidth: 30, isPK: true),
                        new PopupColumnSetting("Description", columnWidth: 100),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select PhraseTypeName, Name, Seq, Junk, Description From Phrase Order by PhraseTypeName asc, Seq Asc";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Phrase";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["PhraseTypeName"],
                        data.Columns["Name"],
                    };
                    return data;
                }

                /// <summary>
                /// 用找到的DataRow取出一個要用於顯示名稱欄位的值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }
            }
        }

        namespace Fabric
        {
            /// <summary>
            /// Fabric PickBox
            /// </summary>
            public class PickLookupBoxFabric : BasePickLookupBox<string, string>
            {
                #region Restrictor - Brand

                /// <summary>
                /// 用於LoadCache的篩選條件(BrandID是主鍵之一)
                /// </summary>
                [Category("SCIC-PickLookupBoxProject")]
                [Description("用於LoadCache的篩選條件(主鍵之一，所以必輸)")]
                [Browsable(false)]
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public CacheCoreDataFilter<string> BrandFilter { get; set; }

                #endregion

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreFabric
                /// </summary>
                private CacheCoreFabric MyCacheCore
                {
                    get
                    {
                        return (CacheCoreFabric)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Fabric PickBox
                /// </summary>
                public PickLookupBoxFabric()
                    : base()
                {
                    base.PopupSettings = CacheCoreFabric.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreFabric.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreFabric.IDMaxLength);
                    this.DataFilterSetting.Items.Add(this.BrandFilter = new CacheCoreDataFilter<string>(false, "BrandID"));
                }
            }

            /// <summary>
            /// Fabric CacheCore
            /// </summary>
            public class CacheCoreFabric : BaseDataCacheCore<string, string>
            {
                internal const int IDMaxLength = 26;

                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreFabric Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreFabric()
                {
                    Singleton = new CacheCoreFabric();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("SCIRefno", columnWidth: 20, isPK: true),
                        new PopupColumnSetting("BrandID", columnWidth: 13),
                        new PopupColumnSetting("Refno", columnWidth: 20),
                        new PopupColumnSetting("Description", columnWidth: 20),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"Select SCIRefno, BrandID, Refno, Description, Junk From Fabric order by BrandID, SCIRefno";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Fabric";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["SCIRefno"],
                    };
                    return data;
                }

                /// <summary>
                /// 用找到的DataRow取出一個要用於顯示名稱欄位的值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }
            }
        }

        namespace Fabric_Supp
        {
            /// <summary>
            /// 
            /// </summary>
            public enum FabricTypeEnum
            {
                /// <summary>
                /// 
                /// </summary>
                Fabric = 0,
                /// <summary>
                /// 
                /// </summary>
                Accessory = 1,
                /// <summary>
                /// 
                /// </summary>
                Other = 2,
            }

            /// <summary>
            /// PickIdBoxFabric_Supp
            /// </summary>
            public class PickIdBoxFabric_Supp : BasePickIdBox<string[], string>
            {
                #region Restrictor - Refno
                private string _Refno = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickIdBoxFabric_Supp")]
                public string Refno
                {
                    get
                    {
                        return this._Refno;
                    }
                    set
                    {
                        if (this._Refno != value)
                        {
                            this._Refno = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion
                #region Restrictor - SCIRefno
                private string _SCIRefno = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickIdBoxFabric_Supp")]
                public string SCIRefno
                {
                    get
                    {
                        return this._SCIRefno;
                    }
                    set
                    {
                        if (this._SCIRefno != value)
                        {
                            this._SCIRefno = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion
                #region Restrictor - Brand
                private string _BrandID = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickIdBoxFabric_Supp")]
                public string BrandID
                {
                    get
                    {
                        return this._BrandID;
                    }
                    set
                    {
                        if (this._BrandID != value)
                        {
                            this._BrandID = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion
                #region Restrictor - Type
                private FabricTypeEnum? _Type = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickIdBoxFabric_Supp")]
                public FabricTypeEnum? Type
                {
                    get
                    {
                        return this._Type;
                    }
                    set
                    {
                        if (this._Type != value)
                        {
                            this._Type = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion
                #region Restrictor - Supp
                private string _SuppID = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickIdBoxFabric_Supp")]
                [Description("用於開窗的條件限制(是複合主鍵之一)")]
                [Bindable(true)]
                public string SuppID
                {
                    get
                    {
                        return this._SuppID;
                    }
                    set
                    {
                        if (this._SuppID != value)
                        {
                            this._SuppID = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion

                /// <summary>
                /// 由SCIRefno和BrandID和Type交織而成的條件
                /// </summary>
                private void ResetDataFIlterPredictor()
                {
                    var localPredictor = new List<Func<DataRow, bool>>();
                    if (string.IsNullOrEmpty(this._Refno) == false)
                        localPredictor.Add((row) => row.Field<string>("Refno") == this._Refno);
                    if (string.IsNullOrEmpty(this._SCIRefno) == false)
                        localPredictor.Add((row) => row.Field<string>("SCIRefno") == this._SCIRefno);
                    if (string.IsNullOrEmpty(this._BrandID) == false)
                        localPredictor.Add((row) => row.Field<string>("BrandID") == this._BrandID);
                    if (this._Type.HasValue)
                        localPredictor.Add((row) => row.Field<string>("Type") == this._BrandID);
                    if (string.IsNullOrEmpty(this._SuppID) == false)
                        localPredictor.Add((row) => row.Field<string>("SuppID") == this._SuppID);
                    base.DataFilterSetting.Predictor = (row) => localPredictor.All(method => method(row));
                }

                /// <summary>
                /// Style PickBox
                /// </summary>
                public PickIdBoxFabric_Supp()
                    : base()
                {
                    if (Env.DesignTime)
                        return;
                    base.PopupSettings = CacheCoreFabric_Supp.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreFabric_Supp.Singleton;
                    this.MaxLength = CacheCoreFabric_Supp.IDMaxLength;
                }
            }

            /// <summary>
            /// Fabric_Supp PickBox
            /// </summary>
            public class PickLookupBoxFabric_Supp : BasePickLookupBox<string[], string>
            {
                #region Restrictor - Refno
                private string _Refno = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickLookupBoxFabric_Supp")]
                public string Refno
                {
                    get
                    {
                        return this._Refno;
                    }
                    set
                    {
                        if (this._Refno != value)
                        {
                            this._Refno = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion
                #region Restrictor - SCIRefno
                private string _SCIRefno = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickLookupBoxFabric_Supp")]
                public string SCIRefno
                {
                    get
                    {
                        return this._SCIRefno;
                    }
                    set
                    {
                        if (this._SCIRefno != value)
                        {
                            this._SCIRefno = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion
                #region Restrictor - Brand
                private string _BrandID = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickLookupBoxFabric_Supp")]
                public string BrandID
                {
                    get
                    {
                        return this._BrandID;
                    }
                    set
                    {
                        if (this._BrandID != value)
                        {
                            this._BrandID = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion
                #region Restrictor - Type
                private FabricTypeEnum? _Type = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickLookupBoxFabric_Supp")]
                public FabricTypeEnum? Type
                {
                    get
                    {
                        return this._Type;
                    }
                    set
                    {
                        if (this._Type != value)
                        {
                            this._Type = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion
                #region Restrictor - Supp
                private string _SuppID = null;
                /// <summary>
                /// 用於LoadCache的篩選條件
                /// </summary>
                [Category("SCIC-PickLookupBoxFabric_Supp")]
                [Description("用於開窗的條件限制(是複合主鍵之一)")]
                [Bindable(true)]
                public string SuppID
                {
                    get
                    {
                        return this._SuppID;
                    }
                    set
                    {
                        if (this._SuppID != value)
                        {
                            this._SuppID = value;
                            if (Env.DesignTime == true)
                                return;
                            this.ResetDataFIlterPredictor();
                            base.ReValidate();
                        }
                    }
                }
                #endregion

                /// <summary>
                /// 由SCIRefno和BrandID和Type交織而成的條件
                /// </summary>
                private void ResetDataFIlterPredictor()
                {
                    var localPredictor = new List<Func<DataRow, bool>>();
                    if (string.IsNullOrEmpty(this._Refno) == false)
                        localPredictor.Add((row) => row.Field<string>("Refno") == this._Refno);
                    if (string.IsNullOrEmpty(this._SCIRefno) == false)
                        localPredictor.Add((row) => row.Field<string>("SCIRefno") == this._SCIRefno);
                    if (string.IsNullOrEmpty(this._BrandID) == false)
                        localPredictor.Add((row) => row.Field<string>("BrandID") == this._BrandID);
                    if (this._Type.HasValue)
                        localPredictor.Add((row) =>
                        {
                            switch (this._Type.Value)
                            {
                                case FabricTypeEnum.Fabric:
                                    return row.Field<string>("Type") == "F";
                                case FabricTypeEnum.Accessory:
                                    return row.Field<string>("Type") == "A";
                                case FabricTypeEnum.Other:
                                    return row.Field<string>("Type").IsOneOfThe("F", "A");
                                default:
                                    throw new NotImplementedException();
                            }
                        });
                    if (string.IsNullOrEmpty(this._SuppID) == false)
                        localPredictor.Add((row) => row.Field<string>("SuppID") == this._SuppID);
                    base.DataFilterSetting.Predictor = (row) => localPredictor.All(method => method(row));
                }

                /// <summary>
                /// 提供簡單轉型呼叫 DefaultCacheCore (BaseDataCacheCore[string, string]) => CacheCoreFabric_Supp
                /// </summary>
                private CacheCoreFabric_Supp MyCacheCore
                {
                    get
                    {
                        return (CacheCoreFabric_Supp)this.DefaultCacheCore;
                    }
                    set
                    {
                        this.DefaultCacheCore = value;
                    }
                }

                /// <summary>
                /// Fabric_Supp PickBox
                /// </summary>
                public PickLookupBoxFabric_Supp()
                    : base()
                {
                    base.PopupSettings = CacheCoreFabric_Supp.DefaultColumnSettings;
                    this.DefaultCacheCore = CacheCoreFabric_Supp.Singleton;
                    this.SetInputMaxLengthAndWidth(CacheCoreFabric_Supp.IDMaxLength);
                    base.DataFilterSetting.TextToPKArray = (text) => new[] { this.SuppID, text };
                }
            }

            /// <summary>
            /// Fabric_Supp CacheCore
            /// </summary>
            public class CacheCoreFabric_Supp : BaseDataCacheCore<string[], string>
            {
                internal const int IDMaxLength = 26;

                /// <summary>
                /// 獨體
                /// </summary>
                public static CacheCoreFabric_Supp Singleton { get; private set; }

                /// <summary>
                /// 預設的開窗欄位設定
                /// </summary>
                internal static PopupColumnSettings DefaultColumnSettings { get; set; }

                /// <summary>
                /// 設定獨體與預設的開窗欄位設定
                /// </summary>
                static CacheCoreFabric_Supp()
                {
                    if (Env.DesignTime)
                        return;
                    Singleton = new CacheCoreFabric_Supp();
                    DefaultColumnSettings = new PopupColumnSettings(new[]
                    {
                        new PopupColumnSetting("Refno", headerText:"Ref#", columnWidth: 25),
                        new PopupColumnSetting("itemtype",headerText:"ItemType", columnWidth: 5),
                        new PopupColumnSetting("Description",headerText:"Description", columnWidth: 30),
                        new PopupColumnSetting("Width",headerText:"Width", columnWidth: 5),
                        new PopupColumnSetting("Weight",headerText:"Weight", columnWidth: 5),
                        new PopupColumnSetting("WEIGHTM2",headerText:"Weight(G/M2)", columnWidth: 5),
                        new PopupColumnSetting("Supplier",headerText:"Supplier", columnWidth: 5),
                        new PopupColumnSetting("CurrencyID",headerText:"Currency", columnWidth: 10),
                        new PopupColumnSetting("POUnit",headerText:"Po Unit", columnWidth: 3),
                        new PopupColumnSetting("BrandID",headerText:"Brand", columnWidth: 8),
                        new PopupColumnSetting("SuppID", columnWidth: 0),
                        new PopupColumnSetting("SCIRefno", columnWidth: 0, isPK: true),
                    }.ToList());
                }

                /// <summary>
                /// 重新抓取資料來源來當作Cache
                /// </summary>
                /// <returns></returns>
                internal protected override DataTable LoadCache()
                {
                    var sql = @"
Select 
	fs.SCIRefno
	, fs.SuppID
	, fs.POUnit
    , f.Refno
	, f.Description
	, f.Width
	, f.Weight
	, f.WEIGHTM2
    , Concat(s.id, '-', s.AbbCH) as Supplier
    , s.CurrencyID
	, fs.BrandID
	, f.Type
	, fs.ItemType
    , ddl.Name as ItemName
	, fs.Junk
	, fs.Lock
From Fabric_Supp fs with(nolock)
    Left Join Fabric f with(nolock) on f.SCIRefno = fs.SCIRefno
    Left Join Supp s with(nolock) on s.ID = fs.SuppID
    Left Join DropDownList ddl on ddl.Type = 'FabricItemType' and ddl.ID = fs.ItemType
";
                    DataTable data;
                    SQL.Select("", sql, out data);
                    data.TableName = "Fabric_Supp";
                    data.PrimaryKey = new[]
                    {
                        data.Columns["SuppID"],
                        data.Columns["SCIRefno"],
                    };
                    return data;
                }

                /// <summary>
                /// 用找到的DataRow取出一個要用於顯示名稱欄位的值
                /// </summary>
                /// <param name="row"></param>
                /// <returns></returns>
                public override string Lookup(DataRow row)
                {
                    if (row == null)
                        return null;
                    return row.Field<string>("Description");
                }
            }
        }
    }

    /// <summary>
    /// 是否要做額外的Validation
    /// </summary>
    public enum UnitTypeEnum
    {
        /// <summary>
        /// Inch以文字的型態
        /// </summary>
        InchText,
        /// <summary>
        /// Inch以數值的型態
        /// </summary>
        InchDecimal,
        /// <summary>
        /// CM
        /// </summary>
        CM
    }

    /// <summary>
    /// 可以設定輸入模式為Inch或是CM的文字方塊，請使用InputType來確定輸入的驗證行為
    /// </summary>
    public class TextBoxUnitRestriction : Sci.Win.UI.TextBox
    {
        private UnitTypeEnum? _InputType;
        /// <summary>
        /// 是否要做額外的Validation(事先定義好的)
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("決定要以哪一種方式來呈現值以及驗證輸入")]
        public UnitTypeEnum? InputType
        {
            get
            {
                return this._InputType;
            }
            set
            {
                this._InputType = value;
                switch (value.GetValueOrDefault(UnitTypeEnum.InchText))
                {
                    case UnitTypeEnum.InchText:
                        base.Text = this.InchValue.ToInchText();
                        break;
                    case UnitTypeEnum.InchDecimal:
                        base.Text = this.InchValue.ToInchDecimal().ToString(this.FloatPartFormat ?? "0.00");
                        break;
                    case UnitTypeEnum.CM:
                        base.Text = this.InchValue.ToCmDecimal().ToString(this.FloatPartFormat ?? "0.00");
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("浮點數的Inch值")]
        public Inch InchValue { get; set; }

        /// <summary>
        /// 當使用Cm或是InchDecimal為InputType的時候，會使用這個屬性來Format結果字串
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("浮點數的位數")]
        public string FloatPartFormat { get; set; }

        /// <summary>
        /// 目前控制項，以CM為單位來呈現的值(及時換算自InchValue)
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("浮點數的CM值")]
        public string CMValue
        {
            get
            {
                if (this.InchValue.Piece.HasValue == false)
                    return null;
                else
                    return this.InchValue.ToCmDecimal().ToString(this.FloatPartFormat ?? "0.000");
            }
        }

        /// <summary>
        /// 目前控制項，以Inch為單位來呈現的值
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("把核心值轉換為Inch單位的數值字串")]
        public string InchDecimal
        {
            get
            {
                return this.InchValue.Piece.HasValue == false ?
                    "" :
                    this.InchValue.ToInchDecimal().ToString(this.FloatPartFormat ?? "0.000");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("把核心值轉換為Inch單位的文字表示式")]
        public string InchText
        {
            get
            {
                return this.InchValue.Piece.HasValue == false ?
                    "" :
                    this.InchValue.ToInchText();
            }
        }

        /// <summary>
        /// 可以設定輸入模式為Inch或是CM的文字方塊，請使用InputType來確定輸入的驗證行為
        /// </summary>
        public TextBoxUnitRestriction()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            int v;
            if (this.InputType.GetValueOrDefault(UnitTypeEnum.InchText) == UnitTypeEnum.InchText && int.TryParse(this.Text, out v) == true && v.ToString() == this.Text)
                this.Text += "\"";
            switch (this.InputType.GetValueOrDefault(UnitTypeEnum.InchText))
            {
                case UnitTypeEnum.InchText:
                    if (this.TryValidateForInchText(this.Text) == false)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("invalid value");
                    }
                    base.Text = this.InchValue.ToInchText();
                    break;
                case UnitTypeEnum.InchDecimal:
                    if (this.TryValidateForInchDecimal(this.Text) == false)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("invalid value");
                    }
                    base.Text = this.InchValue.ToInchDecimal().ToString(this.FloatPartFormat ?? "0.000");
                    break;
                case UnitTypeEnum.CM:
                    if (this.TryValidateForCm(this.Text) == false)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("invalid value");
                    }
                    base.Text = this.InchValue.ToCmDecimal().ToString(this.FloatPartFormat ?? "0.000");
                    break;
                default:
                    throw new NotImplementedException();
            }
            //如果上面已經驗證失敗，就不用做後續驗證了喔
            if (e.Cancel == false)
                base.OnValidating(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.Handled == true)
                return;
            if ((Keys)e.KeyChar == Keys.Back) return;
            if ((Keys)e.KeyChar == Keys.Enter) return;
            var allowedCharacters = "0123456789".IndexOf(e.KeyChar.ToString()) >= 0;
            if (allowedCharacters == false)
            {
                var inputType = this.InputType.GetValueOrDefault(UnitTypeEnum.InchText);
                switch (inputType)
                {
                    case UnitTypeEnum.InchText:
                        if ("/\"".IndexOf(e.KeyChar.ToString()) < 0)
                            e.Handled = true;
                        break;
                    case UnitTypeEnum.InchDecimal:
                    case UnitTypeEnum.CM:
                        if (e.KeyChar.ToString() != ".")
                            e.Handled = true;
                        break;
                }
            }
            base.OnKeyPress(e);
        }

        /// <summary>
        /// 以Inch的格式，驗證輸入值是否合於格式，若否則return false，若是，會在return true之前，把CmValue和InchValue放入驗證後的結果值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool TryValidateForInchText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                this.InchValue = new Inch();
            }
            else
            {
                var newInchPiece = Inch.FromInchText(value);
                if (newInchPiece.Piece == null)
                    return false;
                this.InchValue = newInchPiece;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool TryValidateForInchDecimal(string value)
        {
            decimal v;
            if (string.IsNullOrWhiteSpace(value))
            {
                this.InchValue = new Inch();
            }
            else if (decimal.TryParse(value, out v) == false)
                return false;
            else
            {
                var newInchPiece = Inch.FromInchDecimal(v);
                if (newInchPiece.Piece == null)
                    return false;
                this.InchValue = newInchPiece;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool TryValidateForCm(string value)
        {
            decimal v;
            if (string.IsNullOrWhiteSpace(value))
                this.InchValue = new Inch();
            else if (decimal.TryParse(value, out v) == false)
                return false;
            else
            {
                var newInchPiece = Inch.FromCmDecimal(v);
                if (newInchPiece.Piece.HasValue == false)
                    return false;
                this.InchValue = newInchPiece;
            }
            return true;
        }
    }

    /// <summary>
    /// 代表Inch的值，提供轉換(To/From) InchText 與 CM 的功能
    /// </summary>
    public struct Inch
    {
        private const decimal TransRate = 2.54m;
        private const decimal InchPieceUnit = 0.125m;  // 1m / 8m

        /// <summary>
        /// 吋的數值，會被FromInchDecimal和ToInchDecimal使用
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// 如果這個執行個體是從FromCmValue過來，會存在這邊，如果被ToCmDecimal，會先看這邊有沒有值，有的話直接回傳，沒有才去Value做運算
        /// </summary>
        public decimal? CmValue { get; set; }

        private int? _Piece;
        /// <summary>
        /// 八分之一吋，作為最小單位會被FromInchText和ToInchText使用
        /// </summary>
        public int? Piece
        {
            get
            {
                if (this._Piece.HasValue == false)
                {
                    if (Value.HasValue == false)
                        return null;
                    this._Piece = DecimalToPiece(this.Value.Value);
                }
                return this._Piece;
            }
            set
            {
                this._Piece = value;
                this.Value = PieceToDecimal(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToInchText()
        {
            return PieceToText(this.Piece.GetValueOrDefault(0));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal ToInchDecimal()
        {
            return this.Value.GetValueOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal ToCmDecimal()
        {
            if (CmValue.HasValue)
                return this.CmValue.Value;
            else
                return this.Piece.GetValueOrDefault(0) * InchPieceUnit * TransRate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Inch FromCmText(string text)
        {
            decimal v;
            if (decimal.TryParse(text, out v) == true)
                return FromCmDecimal(v);
            else
                return new Inch();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmValue"></param>
        /// <returns></returns>
        public static Inch FromCmDecimal(decimal? cmValue)
        {
            if (cmValue.HasValue == false)
                return new Inch();
            else
                return new Inch() { Value = cmValue / TransRate, CmValue = cmValue };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inchValue"></param>
        /// <returns></returns>
        public static Inch FromInchDecimal(decimal? inchValue)
        {
            return new Inch() { Value = inchValue };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inchText"></param>
        /// <returns></returns>
        public static Inch FromInchText(string inchText)
        {
            return new Inch() { Piece = TextToPiece(inchText) };
        }

        private static int? DecimalToPiece(decimal? value)
        {
            if (value.HasValue == false)
                return null;

            var intPart = (int)(value.Value / 1);
            var floatPart = value.Value - intPart;
            var pieceOfFloat = floatPart > (InchPieceUnit * 7) ?
                8 :
                Enumerable.Range(1, 7)
                    .FirstOrDefault(idx =>
                        floatPart > ((idx - 1) * InchPieceUnit) &&
                        floatPart <= (idx * InchPieceUnit));

            return (intPart * 8) + pieceOfFloat;
        }

        private static decimal? PieceToDecimal(decimal? piece)
        {
            if (piece.HasValue == false)
                return null;
            else
                return piece.Value * InchPieceUnit;
        }

        private static string PieceToText(decimal piece)
        {
            if (piece < 0)
                return "0";
            if (piece >= 8)
            {
                var intPart = (int)(piece / 8);
                var floatPart = piece % 8;
                if (intPart != 0 && floatPart != 0)
                    return string.Format("{0}{1}", (int)(piece / 8), PieceToText(piece % 8));
                else if (floatPart == 0)
                    return string.Format("{0}\"", intPart);
                else
                    return string.Empty;
            }
            else
            {
                switch (Convert.ToInt32(piece))
                {
                    case 0:
                        return "";
                    case 1:
                        return "\"1/8";
                    case 2:
                        return "\"1/4";
                    case 3:
                        return "\"3/8";
                    case 4:
                        return "\"1/2";
                    case 5:
                        return "\"5/8";
                    case 6:
                        return "\"3/4";
                    case 7:
                        return "\"7/8";
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static int? TextToPiece(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;
            if (text.Count(ch => ch == '\"') != 1)
                return null;

            var part = text.Split('\"');

            var floatPiece = 0;
            switch (part[1])
            {
                case "":
                    floatPiece = 0;
                    break;
                case "1/8":
                    floatPiece = 1;
                    break;
                case "2/8":
                case "1/4":
                    floatPiece = 2;
                    break;
                case "3/8":
                    floatPiece = 3;
                    break;
                case "4/8":
                case "2/4":
                case "1/2":
                    floatPiece = 4;
                    break;
                case "5/8":
                    floatPiece = 5;
                    break;
                case "6/8":
                case "3/4":
                    floatPiece = 6;
                    break;
                case "7/8":
                    floatPiece = 7;
                    break;
                default:
                    return null;
            }
            return Convert.ToInt32("0" + part[0].ToString()) * 8 + floatPiece;
        }
    }

    /// <summary>
    /// 任務收集器 取代List of action，提供RunSync或RunAsunc方法，可以綁訂一個TabPage作為延遲執行的條件，可以綁訂一個BindingSource，作為自動Reset包含任務的觸發器
    /// </summary>
    public class TaskEater : List<Action>
    {
        private List<Action<Action<Action>>> newTaskList = new List<Action<Action<Action>>>();

        /// <summary>
        /// 
        /// </summary>
        public bool RunWithAsync { get; set; }

        private System.Windows.Forms.TabPage TabPage;

        /// <summary>
        /// 用於把目前的Task列表，分執行續去執行
        /// </summary>
        private System.Threading.Thread TaskFlodingThread;

        private List<System.Threading.Thread> RunningThread;

        /// <summary>
        /// 提供最後一次呼叫RunTasks的時候的Token
        /// </summary>
        private int AsyncToken = 0;

        /// <summary>
        /// <para>(選擇性)設定與哪一個TabPage相關，如果有設定，會於該Tab被切換到的時候自動呼叫RunAsyncornize，並請於Add的反覆呼叫最末端，叫用RunIfTabPageAreSelected方法</para>
        /// <para>(選擇性)設定與哪一個BindingSource相關，如果有設定，會於BindingSouce的PositionChange時清空任務</para>
        /// </summary>
        /// <param name="tabPage"></param>
        /// <param name="bs"></param>
        public TaskEater(System.Windows.Forms.TabPage tabPage = null, Ict.Win.UI.ListControlBindingSource bs = null)
        {
            this.RunWithAsync = true;
            if (tabPage != null)
            {
                this.TabPage = tabPage;
                (tabPage.Parent as Sci.Win.UI.TabControl).SelectedIndexChanged += (a, b) =>
                {
                    if ((tabPage.Parent as Sci.Win.UI.TabControl).SelectedTab == tabPage)
                    {
                        if (this.RunWithAsync)
                        {
                            if (this.RunningThread != null)
                                return;
                            this.RunAsyncornize(tabPage.FindForm());
                        }
                        else
                            this.RunSyncornize();
                    }
                };
            }

            if (bs != null)
            {
                bs.PositionChanged += (a, b) => this.ResetTasks();
            }
        }

        /// <summary>
        /// 新增任務
        /// </summary>
        /// <param name="taskMethod"></param>
        /// <returns></returns>
        public new TaskEater Add(Action taskMethod)
        {
            base.Add(taskMethod);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskMethod"></param>
        /// <returns></returns>
        public TaskEater Add(Action<Action<Action>> taskMethod)
        {
            newTaskList.Add(taskMethod);
            return this;
        }

        /// <summary>
        /// 會清空任務集合，如果正在非同步進行中的任務，會被中斷，可以省略後面尚未進行的任務單元
        /// </summary>
        public void ResetTasks()
        {
            base.Clear();
            this.newTaskList.Clear();
            this.AbortRunning();
        }

        private void AbortRunning()
        {
            if (this.RunningThread != null)
            {
                var ts = this.RunningThread;
                this.RunningThread = null;
                ts.AsParallel().ForAll(thread =>
                {
                    if (thread.IsAlive)
                        thread.Abort();
                    thread = null;
                });
            }
        }

        private void StopFlodingTasks()
        {
            if (this.TaskFlodingThread != null)
            {
                if (this.TaskFlodingThread.IsAlive)
                    this.TaskFlodingThread.Abort();
                this.TaskFlodingThread = null;
            }
        }

        /// <summary>
        /// 立即以非同步方式呼叫任務，請確保任務彼此能夠於不同執行續內進行
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="withClearTasks"></param>
        public void RunAsyncornize(Form frm, bool withClearTasks = true)
        {
            this.StopFlodingTasks();
            this.AbortRunning();

            var newToken = new Random().Next(100, 999); ;
            this.AsyncToken = newToken;
            //用來發配任務的執行續
            var threadStart = new System.Threading.ParameterizedThreadStart(this.AsyncFlodingTasks);
            this.TaskFlodingThread = new System.Threading.Thread(threadStart);
            this.TaskFlodingThread.IsBackground = true;

            var ts = this.ToArray().Cast<object>().Concat(this.newTaskList.Cast<object>()).ToArray();
            if (withClearTasks)
                base.Clear();

            this.TaskFlodingThread.Start(new object[] { ts, newToken, frm });
        }

        private void AsyncFlodingTasks(object context)
        {
            var parameters = (object[])context;
            var taskToFlood = (object[])parameters[0];
            var passInToken = (int)parameters[1];
            var frm = (Form)parameters[2];
            var runningThreads = new List<System.Threading.Thread>();
            var frmInvoker = new Action<Action>((act) =>
            {
                //frm.BeginInvoke(act);
                frm.Invoke(act);
            });
            foreach (var task in taskToFlood)
            {
                if ((int)passInToken != this.AsyncToken)
                {
                    this.AbortRunning();
                    System.Threading.Thread.CurrentThread.Abort();
                    return;
                }
                else
                {
                    var t = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                    {
                        if (task is Action)
                        {
                            try
                            {
                                frm.Invoke((Action)task);
                            }
                            catch (System.Threading.ThreadAbortException)
                            {
                            }
                        }
                        else if (task is Action<Action<Action>>)
                        {
                            try
                            {
                                ((Action<Action<Action>>)task)(frmInvoker);
                            }
                            catch (System.Threading.ThreadAbortException)
                            {
                            }
                        }
                        else
                            throw new NotImplementedException();
                        if (this.RunningThread == null)
                        {
                            if (this.AsyncRunningFinish != null)
                                frm.Invoke((Action)(() => this.AsyncRunningFinish(this, EventArgs.Empty)));
                        }
                        else
                        {
                            lock (this.RunningThread)
                            {
                                if (this.RunningThread != null)
                                {
                                    if (this.RunningThread.Any(threadInQueue => threadInQueue.ThreadState == System.Threading.ThreadState.Running) == false)
                                    {
                                        if (this.AsyncRunningFinish != null)
                                            frm.Invoke((Action)(() => this.AsyncRunningFinish(this, EventArgs.Empty)));
                                    }
                                }
                            }
                        }
                    }));
                    t.IsBackground = true;
                    t.Start();
                    if (this.AsyncRunningBegin != null)
                        frm.Invoke((Action)(() => this.AsyncRunningBegin(this, EventArgs.Empty)));
                    runningThreads.Add(t);
                }
            }
            this.RunningThread = runningThreads;
            runningThreads = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler AsyncRunningBegin;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler AsyncRunningFinish;

        /// <summary>
        /// 立即以同步方式呼叫任務
        /// </summary>
        /// <param name="withClearTasks"></param>
        public void RunSyncornize(bool withClearTasks = true)
        {
            this.ForEach(task => task());
            if (withClearTasks)
                base.Clear();
            this.newTaskList.ForEach(task =>
            {
                var syncInvoker = new Action<Action>((act) =>
                {
                    act();
                });
                task(syncInvoker);
            });
            if (withClearTasks)
                this.newTaskList.Clear();
        }

        /// <summary>
        /// 如果於建構子有指定TablPage，請於Add()的反覆呼叫最末端，加上這個呼叫，會立即檢查目前頁籤是否為自己所綁定的頁籤，如果是，會立刻進行任務，如果不是則不動作
        /// </summary>
        public void RunIfTabPageAreSelected()
        {
            if (this.TabPage != null && ((this.TabPage.FindForm() as Sci.Win.IForm).EditMode || (this.TabPage.Parent as Sci.Win.UI.TabControl).SelectedTab == this.TabPage))
            {
                if (RunWithAsync)
                    this.RunAsyncornize(this.TabPage.FindForm());
                else
                    this.RunSyncornize();
            }
        }
    }

    /// <summary>
    /// 專門用來顯示AddName+AddDate或是EditName+EditDate的顯示格，因為一定是Readonly，加上格是就那幾種，所以做一個快取在裡面
    /// </summary>
    public class ModifyInfoDisplayer : Ict.Win.UI.DisplayBox
    {
        #region static

        private static CustomizeCacheCore<string, string> GetNameCachCore { get; set; }

        static ModifyInfoDisplayer()
        {
            GetNameCachCore = new CustomizeCacheCore<string, string>()
            {
                MyCacheLoader = () =>
                {
                    using (var dr = DBProxy.Current.SelectEx("Select * from GetName"))
                    {
                        if (dr == false)
                            throw new ApplicationException("Can't load GetName(View)");
                        else
                        {
                            dr.DisposeExtendedData = false;
                            var dt = dr.ExtendedData;
                            dt.PrimaryKey = new[]
                            {
                                dt.Columns["ID"],
                            };
                            dt.TableName = "GetName";
                            return dr.ExtendedData;
                        }
                    }
                },
            };
        }

        #endregion

        /// <summary>
        /// 要顯示Add資訊還是Modify資訊(預設Add)
        /// </summary>
        public enum ModifyInfoEnum
        {
            /// <summary>
            /// 
            /// </summary>
            AddInfo,
            /// <summary>
            /// 
            /// </summary>
            EditInfo,
        }

        /// <summary>
        /// 如果要顯示Date部分，要如何顯示
        /// </summary>
        public enum DateInfoEnum
        {
            /// <summary>
            /// 2016/04/21
            /// </summary>
            yyyyMMdd,
            /// <summary>
            /// 2016/04/21 16:30
            /// </summary>
            yyyyMMdd_HHmm,
            /// <summary>
            /// 2016/04/21 16:30:52
            /// </summary>
            yyyyMMdd_HHmmss,
        }

        private ModifyInfoEnum? _DisplayFor;
        /// <summary>
        /// 要顯示Add資訊還是Modify資訊
        /// </summary>
        [Category("SCIC-ModifyInfoDisplayer")]
        [Description("要顯示AddName+AddDate還是EditName+EditDate")]
        public ModifyInfoEnum? DisplayFor
        {
            get
            {
                return this._DisplayFor;
            }
            set
            {
                this._DisplayFor = value;
                this.TrySetupDatabindingAsNeeded();
            }
        }

        private UserPrg.NameType? _NameDisplayStyle;
        /// <summary>
        /// 如果要顯示Name部分，要如何顯示，沿用UserPrg.NameType
        /// </summary>
        [Category("SCIC-ModifyInfoDisplayer")]
        [Description("如何顯示使用者資訊的格式")]
        public UserPrg.NameType? NameDisplayStyle
        {
            get
            {
                return this._NameDisplayStyle;
            }
            set
            {
                this._NameDisplayStyle = value;
                this.RefrashText();
            }
        }

        private DateInfoEnum? _DateDisplayStyle;
        /// <summary>
        /// 如果要顯示Date部分，要如何顯示
        /// </summary>
        [Category("SCIC-ModifyInfoDisplayer")]
        [Description("如何顯示日期的格式")]
        public DateInfoEnum? DateDisplayStyle
        {
            get
            {
                return this._DateDisplayStyle;
            }
            set
            {
                this._DateDisplayStyle = value;
                this.RefrashText();
            }
        }

        private System.Windows.Forms.BindingSource _DataSource;
        /// <summary>
        /// 資料來源
        /// </summary>
        [Category("SCIC-ModifyInfoDisplayer")]
        [Description("資料來源，如果AutoSetupDataBinding設定為true，會在ParentChange的時候動設定DataBinding，取用這個屬性裡面的Add/Edit資訊")]
        public System.Windows.Forms.BindingSource DataSource
        {
            get
            {
                return this._DataSource;
            }
            set
            {
                this._DataSource = value;
                this.TrySetupDatabindingAsNeeded();
            }
        }

        /// <summary>
        /// 專門用來顯示AddName+AddDate或是EditName+EditDate的顯示格
        /// </summary>
        public ModifyInfoDisplayer()
        {

        }

        private bool? _AutoSetupDataBinding;
        /// <summary>
        /// 是否要自動產生Databinding(預設True)
        /// </summary>
        [Category("SCIC-ModifyInfoDisplayer")]
        [Description("是否要在ParentChange的時候自動設定DataBinding，取用這個屬性裡面的Add/Edit資訊，必須搭配DataSource屬性使用")]
        public bool? AutoSetupDataBinding
        {
            get
            {
                return this._AutoSetupDataBinding;
            }
            set
            {
                this._AutoSetupDataBinding = value;
                this.TrySetupDatabindingAsNeeded();
            }
        }

        private string _BindingName;
        /// <summary>
        /// 可以做為資料繫結的屬性，會連動base.Text
        /// </summary>
        [Bindable(true)]
        [Category("SCIC-ModifyInfoDisplayer")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("資料繫結點(使用者ID)，通常來自於DataBindging的RaedValue行為，但也可以手動給值(若如此，請記得AutoSetupDataBinding設定為false)")]
        public string BindingName
        {
            get
            {
                return this._BindingName;
            }
            set
            {
                _BindingName = value;
                this.RefrashText();
            }
        }

        private DateTime? _BindingDate;
        /// <summary>
        /// 可以做為資料繫結的屬性，會連動base.Text
        /// </summary>
        [Bindable(true)]
        [Category("SCIC-ModifyInfoDisplayer")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("資料繫結點(編輯時間)，通常來自於DataBindging的RaedValue行為，但也可以手動給值(若如此，請記得AutoSetupDataBinding設定為false)")]
        public DateTime? BindingDate
        {
            get
            {
                return this._BindingDate;
            }
            set
            {
                _BindingDate = value;
                this.RefrashText();
            }
        }

        private string DateFormat { get; set; }

        /// <summary>
        /// [ReadOnly]將BindingName的值，依照NameDisplayStyle的屬性來格式化成要顯示的文字
        /// </summary>
        [Category("SCIC-ModifyInfoDisplayer")]
        [Browsable(false)]
        public string FormattedNameText
        {
            get
            {
                if (Env.DesignTime == true)
                    return string.Empty;
                if (this.NameDisplayStyle.HasValue == false)
                    return null;
                var row = GetNameCachCore.FindRowByPK(this.BindingName);
                if (row == null)
                    return null;
                switch (this.NameDisplayStyle.Value)
                {
                    case UserPrg.NameType.nameOnly:
                        return row.Field<string>("NameOnly");
                    case UserPrg.NameType.nameAndExt:
                        return row.Field<string>("NameAndExt");
                    case UserPrg.NameType.idAndName:
                        return row.Field<string>("IdAndName");
                    case UserPrg.NameType.idAndNameAndExt:
                        return row.Field<string>("IdAndNameAndExt");
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// [ReadOnly]將BindingDate的值，依照DateDisplayStyle的屬性來格式化成要顯示的文字
        /// </summary>
        [Category("SCIC-ModifyInfoDisplayer")]
        [Browsable(false)]
        public string FormattedDateText
        {
            get
            {
                if (Env.DesignTime == true)
                    return string.Empty;
                if (this.DateDisplayStyle.HasValue == false)
                    return null;
                if (this.BindingDate.HasValue == false)
                    return null;
                switch (this.DateDisplayStyle.Value)
                {
                    case DateInfoEnum.yyyyMMdd:
                        return this.BindingDate.Value.ToString("yyyy/MM/dd");
                    case DateInfoEnum.yyyyMMdd_HHmm:
                        return this.BindingDate.Value.ToString("yyyy/MM/dd HH:mm");
                    case DateInfoEnum.yyyyMMdd_HHmmss:
                        return this.BindingDate.Value.ToString("yyyy/MM/dd HH:mm:ss");
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// 重新整理要顯示的文字
        /// </summary>
        private void RefrashText()
        {

            base.Text = new string[] 
                {
                    this.FormattedNameText,
                    this.FormattedDateText
                }
                .Where(item => string.IsNullOrWhiteSpace(item) == false)
                .JoinToString(" ");
        }

        /// <summary>
        /// 依照AutoSetupDataBinding來決定要不要自動建立DataBinding
        /// </summary>
        private void TrySetupDatabindingAsNeeded()
        {
            if (this.AutoSetupDataBinding == true)
                this.DoAutoSetupDataBinding();
        }

        /// <summary>
        /// 是否已經自動建立過DataBinding
        /// </summary>
        private bool DatabindingGenerated = false;

        /// <summary>
        /// 檢查相關數性是否都已齊全，若是，則自動新增DataBinding
        /// </summary>
        private void DoAutoSetupDataBinding()
        {
            if (DatabindingGenerated == true)
                return;

            if (this.DataSource == null)
                return;

            if (this.DisplayFor.HasValue == false)
                return;

            if (Env.DesignTime == true)
                return;

            switch (this.DisplayFor.GetValueOrDefault(ModifyInfoEnum.AddInfo))
            {
                case ModifyInfoEnum.AddInfo:
                    {
                        DatabindingGenerated = true;
                        this.DataBindings.Add(new Binding("BindingName", this.DataSource, "AddName"));
                        var binding = new Binding("BindingDate", this.DataSource, "AddDate", true);
                        binding.Format += (a, b) =>
                        {
                            if (b.Value == DBNull.Value)
                                b.Value = null;
                        };
                        this.DataBindings.Add(binding);
                    }
                    break;
                case ModifyInfoEnum.EditInfo:
                    {
                        DatabindingGenerated = true;
                        this.DataBindings.Add(new Binding("BindingName", this.DataSource, "EditName"));
                        var binding = new Binding("BindingDate", this.DataSource, "EditDate", true);
                        binding.Format += (a, b) =>
                        {
                            if (b.Value == DBNull.Value)
                                b.Value = null;
                        };
                        this.DataBindings.Add(binding);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}