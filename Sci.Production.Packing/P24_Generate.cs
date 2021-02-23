using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using TECIT.TFORMer;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P24_Generate : Win.Tems.QueryForm
    {
        private string packingListID;
        private string brandID;
        private bool alreadyExistedPackinglist;
        private DateTime? SCIDelivery_s;
        private DateTime? SCIDelivery_e;
        private List<P24_GenerateResult> P24_GenerateResults;

        /// <inheritdoc/>
        public P24_Generate()
        {
            this.InitializeComponent();

            string licensee = MyUtility.Convert.GetString(ConfigurationManager.AppSettings["TFORMer_Licensee"]);
            string licenseKey = MyUtility.Convert.GetString(ConfigurationManager.AppSettings["TFORMer_LicenseKey"]);

            TFORMer.License(licensee, LicenseKind.Workgroup, 1, licenseKey);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            DataGridViewGeneratorCheckBoxColumnSettings col_Selected = new DataGridViewGeneratorCheckBoxColumnSettings
            {
                HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None,
            };
            col_Selected.CellEditable += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetBool(dr["NoStickerBasicSetting"]) || MyUtility.Convert.GetBool(dr["CtnInClog"]))
                {
                    e.IsEditable = false;
                }
                else
                {
                    e.IsEditable = true;
                }
            };

            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
              .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: col_Selected)
              .Text("ID", header: "Packing No", width: Widths.AnsiChars(15), iseditingreadonly: true)
              .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
              .Numeric("CTNQty", header: "CTN Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
              .CheckBox("NoStickerBasicSetting", header: "No Sticker Basic Setting", trueValue: 1, falseValue: 0, width: Widths.AnsiChars(20), iseditable: false)
              .CheckBox("StickerAlreadyExisted", header: "Sticker Already existed", trueValue: 1, falseValue: 0, width: Widths.AnsiChars(20), iseditable: false)
              .CheckBox("CtnInClog", header: "Ctn In Clog", width: Widths.AnsiChars(20), trueValue: 1, falseValue: 0, iseditable: false)
          ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (!this.CheckInput())
            {
                return;
            }

            this.Query();
        }

        private bool CheckInput()
        {
            if (MyUtility.Check.Empty(this.txtPackingListID.Text) && !this.dateSCIDev.HasValue1 && !this.dateSCIDev.HasValue2)
            {
                MyUtility.Msg.WarningBox("<Packing No>, <SCI Delivery> cannot all be empty.");
                return false;
            }

            this.packingListID = this.txtPackingListID.Text;
            this.brandID = this.txtBrand.Text;
            this.SCIDelivery_s = this.dateSCIDev.Value1;
            this.SCIDelivery_e = this.dateSCIDev.Value2;
            this.alreadyExistedPackinglist = this.chkExistedPacking.Checked;

            return true;
        }

        private void Query()
        {
            DataTable dt;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string where = string.Empty;
            string where_include = string.Empty;
            #region WHERE
            if (!MyUtility.Check.Empty(this.packingListID))
            {
                where += $@"AND p.ID=@ID" + Environment.NewLine;
                parameters.Add(new SqlParameter("@ID", this.packingListID));
            }

            if (!MyUtility.Check.Empty(this.brandID))
            {
                where += $@"AND p.BrandID=@BrandID" + Environment.NewLine;
                parameters.Add(new SqlParameter("@BrandID", this.brandID));
            }

            if (this.SCIDelivery_s.HasValue && this.SCIDelivery_e.HasValue)
            {
                where += $@"AND o.SCIDelivery BETWEEN @SCIDelivery_s AND @SCIDelivery_e" + Environment.NewLine;
                where += $@"AND o.Category IN ('B', 'S')" + Environment.NewLine;
                parameters.Add(new SqlParameter("@SCIDelivery_s", this.SCIDelivery_s.Value));
                parameters.Add(new SqlParameter("@SCIDelivery_e", this.SCIDelivery_e.Value));
            }

            if (this.alreadyExistedPackinglist)
            {
                where_include = $@"
AND IIF(EXISTS(SELECT 1 FROM ShippingMarkPic pic 
										    INNER JOIN ShippingMarkPic_Detail picD ON  pic.Ukey= picD.ShippingMarkPicUkey
										    WHERE pic.PackingListID = p.ID) ,1 ,0) = 1
";
            }

            #endregion

            #region SQL
            string cmd = $@"
--找出哪個箱子種類包含混尺碼
SELECT [PackingListID]=pd.ID ,pd.SCICtnNo
INTO #Mix
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
{where}
AND (SELECT COUNT(qq.Ukey) FROM PackingList_Detail qq 
		where qq.ID = p.ID 
		AND qq.OrderID = pd.OrderID 
		AND qq.CTNStartNo = pd.CTNStartNo
		AND qq.Article = pd.Article 
		AND qq.SizeCode <> pd.SizeCode 
		and qq.Ukey != pd.Ukey) > 0

--找出貼標種類
SELECT DISTINCT
 p.ID
,p.BrandID
,pd.CTNStartNo
,pd.RefNo
,[ShippingMarkCombinationUkey] = IIF( IsMixed.Val = 0 
	,ISNULL(c.StickerCombinationUkey,comb.Ukey )
	,ISNULL(c.StickerCombinationUkey_MixPack ,comb.Ukey)
)
,p.CTNQty
INTO #base
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
INNER JOIN CustCD c ON c.ID = p.CustCDID
LEFT JOIN Pullout pu ON p.PulloutID = pu.ID
OUTER APPLY(
	SELECT [Val] = IIF( 
		EXISTS(
				SELECT 1 FROM #Mix pp 
				WHERE pd.ID = pp.PackingListID AND pd.SCICtnNo = pp.SCICtnNo 
		)
		,1
		,0
	)
	
)IsMixed
OUTER APPLY(
	SELECT *
	FROM ShippingMarkCombination s
	WHERE s.BrandID=p.BrandID AND s.Category='PIC' AND IsDefault = 1 AND IsMixPack = IsMixed.Val
)comb
WHERE  (pu.Status NOT IN ('Confirmed', 'Locked') OR pu.Status IS NULL)
{where}

SELECT DISTINCT
     [Selected] = IIF(CompleteCtn.Val >= p.CTNQty AND CtnInClog.Val = 0 ,1 ,0) /* 預設勾選No Sticker Basic Setting = False 且 Ctn In Clog = False */
    ,p.ID
    ,p.BrandID
    ,p.CTNQty
    ,[NoStickerBasicSetting] = CAST( IIF(CompleteCtn.Val < p.CTNQty , 1, 0) as bit)
    ,[StickerAlreadyExisted] = CAST( IIF(EXISTS(SELECT 1 FROM ShippingMarkPic pic 
										    INNER JOIN ShippingMarkPic_Detail picD ON pic.Ukey= picD.ShippingMarkPicUkey
										    WHERE pic.PackingListID = p.ID) ,1 ,0)  as bit)
	,[CtnInClog] = CAST( IIF(CtnInClog.Val > 0 , 1 ,0)  as bit)
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
LEFT JOIN Pullout pu ON p.PulloutID = pu.ID
OUTER APPLY(
	----檢查的邏輯順序：Packing B03 > B06 > B05 > B05 Detail 的TemplateName
	SELECT [Val]=COUNT(b.CTNStartNo)
	FROM  #base b
	INNER JOIN  ShippingMarkPicture pict ON pict.BrandID = b.BrandID 
							AND pict.Category='PIC' 
							AND pict.CTNRefno = b.RefNo 
							AND pict.ShippingMarkCombinationUkey = b.ShippingMarkCombinationUkey

	INNER JOIN ShippingMarkPicture_Detail pictD ON pict.Ukey = pictD.ShippingMarkPictureUkey
	INNER JOIN ShippingMarkType t ON t.Ukey = pictD.ShippingMarkTypeUkey
	LEFT JOIN ShippingMarkType_Detail td ON t.Ukey = td.ShippingMarkTypeUkey AND td.StickerSizeID = pictD.StickerSizeID
	WHERE b.ID = p.ID
	AND t.FromTemplate = 1
	AND td.TemplateName <> ''
)CompleteCtn
OUTER APPLY(
	SELECT [Val] = COUNT(Ukey)
	FROM PackingList_Detail pdd
	WHERE pdd.ID = pd.ID AND pdd.ReceiveDate IS NOT NULL 
)CtnInClog
WHERE  (pu.Status NOT IN ('Confirmed', 'Locked') OR pu.Status IS NULL)
{where}
{where_include}

ORDER BY p.ID

DROP TABLE #base,#Mix
";
            #endregion

            DualResult r = DBProxy.Current.Select(null, cmd, parameters, out dt);

            if (!r)
            {
                this.ShowErr(r);
                this.grid.DataSource = null;
                return;
            }

            this.grid.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            DataTable dataTable;
            this.P24_GenerateResults = new List<P24_GenerateResult>();

            if (!MyUtility.Check.Empty(this.grid.DataSource))
            {
                dataTable = (DataTable)this.grid.DataSource;
            }
            else
            {
                return;
            }

            DataRow[] selecteds = dataTable.Select("Selected=1");

            if (selecteds.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first.");
                return;
            }

            this.ShowWaitMessage("Processing...");

            Sci.Production.Packing.P26.Result t = new P26.Result();
            this.Generate(selecteds, ref t, false, string.Empty);
            this.ShowResult();

            this.HideWaitMessage();
        }

        /// <summary>
        /// 基本設定 和 Clog檢查
        /// </summary>
        /// <param name="selecteds">selecteds</param>
        /// <returns>ss</returns>
        public DataRow[] CheckBasicSettingClog(DataRow[] selecteds)
        {
            List<string> packingListIDs = selecteds.Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct().ToList();
            DataTable dtrBasicSetting;
            DataTable ctnDt;

            #region SQL
            string cmd = $@"

--找出哪個箱子種類包含混尺碼
SELECT [PackingListID]=pd.ID ,pd.SCICtnNo
INTO #Mix
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
AND  p.ID  IN ('{packingListIDs.JoinToString("','")}')
AND (SELECT COUNT(qq.Ukey) FROM PackingList_Detail qq 
		where qq.ID = p.ID 
		AND qq.OrderID = pd.OrderID 
		AND qq.CTNStartNo = pd.CTNStartNo
		AND qq.Article = pd.Article 
		AND qq.SizeCode <> pd.SizeCode 
		and qq.Ukey != pd.Ukey) > 0

----找出貼標種類
SELECT DISTINCT
 p.ID
,p.BrandID
,pd.CTNStartNo
,pd.RefNo
,[ShippingMarkCombinationUkey] = IIF( IsMixed.Val = 0 
	,ISNULL(c.StickerCombinationUkey,comb.Ukey )
	,ISNULL(c.StickerCombinationUkey_MixPack ,comb.Ukey)
)
,p.CTNQty
INTO #base
FROM PackingList p WITH(NOLOCK)
INNER JOIN PackingList_Detail pd WITH(NOLOCK) ON p.ID = pd.ID
INNER JOIN Orders o WITH(NOLOCK) ON o.ID = pd.OrderID
INNER JOIN CustCD c WITH(NOLOCK) ON c.ID = p.CustCDID
LEFT JOIN Pullout pu WITH(NOLOCK) ON p.PulloutID = pu.ID
OUTER APPLY(
	SELECT [Val] = IIF( 
		EXISTS(
				SELECT 1 FROM #Mix pp 
				WHERE pd.ID = pp.PackingListID AND pd.SCICtnNo = pp.SCICtnNo 
		)
		,1
		,0
	)
	
)IsMixed
OUTER APPLY(
	SELECT *
	FROM ShippingMarkCombination s
	WHERE s.BrandID=p.BrandID AND s.Category='PIC' AND IsDefault = 1 AND IsMixPack = IsMixed.Val
)comb
WHERE  (pu.Status NOT IN ('Confirmed', 'Locked') OR pu.Status IS NULL)
AND p.ID IN ('{packingListIDs.JoinToString("','")}')

SELECT b.*
FROM  #base b
LEFT JOIN  ShippingMarkPicture pict WITH(NOLOCK) ON pict.BrandID = b.BrandID 
						AND pict.Category='PIC' 
						AND pict.CTNRefno = b.RefNo 
						AND pict.ShippingMarkCombinationUkey = b.ShippingMarkCombinationUkey

LEFT JOIN ShippingMarkPicture_Detail pictD WITH(NOLOCK) ON pict.Ukey = pictD.ShippingMarkPictureUkey
LEFT JOIN ShippingMarkType t WITH(NOLOCK) ON t.Ukey = pictD.ShippingMarkTypeUkey
LEFT JOIN ShippingMarkType_Detail td WITH(NOLOCK) ON t.Ukey = td.ShippingMarkTypeUkey AND td.StickerSizeID = pictD.StickerSizeID
WHERE 
(t.FromTemplate= 1  AND (td.TemplateName = '' OR td.TemplateName IS NULL)  )----沒有上傳範本
OR pict.ShippingMarkCombinationUkey IS NULL ----Combination 沒有設定
OR t.Ukey  IS NULL  ----MarkType 沒有設定

DROP TABLE #base, #Mix
";

            DualResult r = DBProxy.Current.Select(null, cmd, out dtrBasicSetting);

            if (!r)
            {
                this.ShowErr(r);
                this.grid.DataSource = null;
                return null;
            }

            cmd = $@"
SELECT DISTINCT
	 p.ID
    ,p.BrandID
    ,p.CTNQty
	,pd.CTNStartNo
	,pd.ReceiveDate
FROM PackingList p WITH(NOLOCK)
INNER JOIN PackingList_Detail pd WITH(NOLOCK) ON p.ID = pd.ID
INNER JOIN Orders o WITH(NOLOCK) ON o.ID = pd.OrderID
LEFT JOIN Pullout pu WITH(NOLOCK) ON p.PulloutID = pu.ID
WHERE  (pu.Status NOT IN ('Confirmed', 'Locked') OR pu.Status IS NULL)
AND p.ID IN ('{packingListIDs.JoinToString("','")}')
--AND pd.ReceiveDate IS NOT NULL

";
            r = DBProxy.Current.Select(null, cmd, out ctnDt);

            if (!r)
            {
                this.ShowErr(r);
                this.grid.DataSource = null;
                return null;
            }

            #endregion

            // 基本檔缺少
            var noStickerBasicSetting = dtrBasicSetting.AsEnumerable()
                .Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct().ToList();

            // 至少有一個紙箱在成品倉
            var ctnInClog = ctnDt.AsEnumerable().Where(o => !MyUtility.Check.Empty(o["ReceiveDate"])).ToList();
            var paskings = ctnInClog.Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct();

            // 基本檔缺少的Packing不要進行後續動作，並記錄料號(Refno)
            foreach (var packingListID in noStickerBasicSetting)
            {
                this.P24_GenerateResults.Add(new P24_GenerateResult()
                {
                    PackingListID = packingListID,
                    IsSuccess = false,
                    BasicSettingError = dtrBasicSetting.AsEnumerable()
                        .Where(o => MyUtility.Convert.GetString(o["ID"]) == packingListID)
                        .Select(o => MyUtility.Convert.GetString(o["Refno"])).Distinct().ToList(),
                    Message = "Sticker basic setting not yet complete",
                });

                selecteds = selecteds.Where(o => MyUtility.Convert.GetString(o["ID"]) != packingListID).ToArray();
            }

            // 有紙箱在成品倉的Packing不要進行後續動作，並找出箱子號CTNStartNo
            foreach (var packingListID in paskings)
            {
                var generateResults = this.P24_GenerateResults.Where(o => o.PackingListID == packingListID);

                // 需檢查該Packing ID是否已經存在
                if (!generateResults.Any())
                {
                    this.P24_GenerateResults.Add(new P24_GenerateResult()
                    {
                        PackingListID = packingListID,
                        CtnError = ctnInClog.AsEnumerable()
                            .Where(o => MyUtility.Convert.GetString(o["ID"]) == packingListID)
                            .Select(o => MyUtility.Convert.GetString(o["CTNStartNo"])).Distinct().ToList(),
                        IsSuccess = false,
                        Message = "Carton already in Clog",
                    });
                }
                else
                {
                    var data = generateResults.FirstOrDefault();
                    List<string> ctnError = ctnInClog.AsEnumerable()
                            .Where(o => MyUtility.Convert.GetString(o["ID"]) == packingListID)
                            .Select(o => MyUtility.Convert.GetString(o["CTNStartNo"])).Distinct().ToList();

                    data.CtnError = ctnError;
                }

                selecteds = selecteds.Where(o => MyUtility.Convert.GetString(o["ID"]) != packingListID).ToArray();
            }

            return selecteds;
        }

        /// <inheritdoc/>
        public void Generate(DataRow[] selecteds, ref Sci.Production.Packing.P26.Result p26Result, bool isP26Check, string callFrom = "")
        {
            if (callFrom == "P26")
            {
                this.P24_GenerateResults = new List<P24_GenerateResult>();
            }

            // 第一階段檢查：基礎設定
            selecteds = this.CheckBasicSettingClog(selecteds);

            if (selecteds == null)
            {
                this.ShowResult();
                return;
            }

            List<string> packingListIDs = selecteds.Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct().ToList();
            List<P24_Template> tList = new List<P24_Template>();

            // 取得每一箱的資訊、以及對應的範本名稱
            #region SQL
            string cmd = $@"
--找出哪個箱子種類包含混尺碼
SELECT [PackingListID]=pd.ID ,pd.SCICtnNo
INTO #Mix
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
AND  p.ID IN ('{packingListIDs.JoinToString("','")}')
AND (SELECT COUNT(qq.Ukey) FROM PackingList_Detail qq 
		where qq.ID = p.ID 
		AND qq.OrderID = pd.OrderID 
		AND qq.CTNStartNo = pd.CTNStartNo
		AND qq.Article = pd.Article 
		AND qq.SizeCode <> pd.SizeCode 
		and qq.Ukey != pd.Ukey) > 0

----找出貼標種類
SELECT DISTINCT
     p.ID
    ,p.BrandID
    ,pd.OrderID
    ,pd.CTNStartNo
    ,pd.RefNo
    ,pd.SCICtnNo
    ,[ShippingMarkCombinationUkey] = IIF( IsMixed.Val = 0 
	    ,ISNULL(c.StickerCombinationUkey,comb.Ukey )
	    ,ISNULL(c.StickerCombinationUkey_MixPack ,comb.Ukey)
     )
    ,p.CTNQty
INTO #base
FROM PackingList p WITH(NOLOCK)
INNER JOIN PackingList_Detail pd WITH(NOLOCK) ON p.ID = pd.ID
INNER JOIN Orders o WITH(NOLOCK) ON o.ID = pd.OrderID
INNER JOIN CustCD c WITH(NOLOCK) ON c.ID = p.CustCDID
LEFT JOIN Pullout pu WITH(NOLOCK) ON p.PulloutID = pu.ID
OUTER APPLY(
	SELECT [Val] = IIF( 
		EXISTS(
				SELECT 1 FROM #Mix pp 
				WHERE pd.ID = pp.PackingListID AND pd.SCICtnNo = pp.SCICtnNo 
		)
		,1
		,0
	)
	
)IsMixed
OUTER APPLY(
	SELECT *
	FROM ShippingMarkCombination s
	WHERE s.BrandID=p.BrandID AND s.Category='PIC' AND IsDefault = 1 AND IsMixPack = IsMixed.Val
)comb
WHERE  (pu.Status NOT IN ('Confirmed', 'Locked')OR pu.Status IS NULL)
AND p.ID IN ('{packingListIDs.JoinToString("','")}')


SELECT DISTINCT [PackingListID]=b.ID   ----對應ShippingMarkPic和的 [PackingListID]
    ,b.BrandID
    ,b.OrderID
    ,b.RefNo
    ,b.CTNStartNo
    ,td.TemplateName
	,t.FromTemplate
	----對應ShippingMarkPic_Detail
    ,b.SCICtnNo
	,b.ShippingMarkCombinationUkey
	,[ShippingMarkTypeUkey]=t.Ukey
	,pictD.Side
	,pictD.Seq
	,pictD.FromBottom
	,pictD.FromRight
	,st.Width
	,st.Length
FROM  #base b
INNER JOIN  ShippingMarkPicture pict WITH(NOLOCK) ON pict.BrandID = b.BrandID 
						AND pict.Category='PIC' 
						AND pict.CTNRefno = b.RefNo 
						AND pict.ShippingMarkCombinationUkey = b.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkPicture_Detail pictD WITH(NOLOCK) ON pict.Ukey = pictD.ShippingMarkPictureUkey
INNER JOIN ShippingMarkType t WITH(NOLOCK) ON t.Ukey = pictD.ShippingMarkTypeUkey
LEFT JOIN ShippingMarkType_Detail td WITH(NOLOCK) ON t.Ukey = td.ShippingMarkTypeUkey AND td.StickerSizeID = pictD.StickerSizeID
INNER JOIN StickerSize st WITH(NOLOCK) ON st.ID = pictD.StickerSizeID
--WHERE td.TemplateName <> ''

DROP TABLE #base, #Mix
";
            #endregion

            DualResult r = DBProxy.Current.Select(null, cmd, out DataTable dt);

            if (!r)
            {
                this.ShowErr(r);
                return;
            }

            // 在這邊Return，表示沒有需要轉出HTML，因此不需要提示訊息
            if (dt.Rows.Count == 0)
            {
                return;
            }

            // 組合出物件方便使用
            foreach (DataRow dr in dt.Rows)
            {
                string templatePath = MyUtility.GetValue.Lookup("SELECT ShippingMarkTemplatePath FROM System");
                P24_Template obj = new P24_Template()
                {
                    PackingListID = MyUtility.Convert.GetString(dr["PackingListID"]),
                    BrandID = MyUtility.Convert.GetString(dr["BrandID"]),
                    OrderID = MyUtility.Convert.GetString(dr["OrderID"]),
                    Refno = MyUtility.Convert.GetString(dr["RefNo"]),
                    CTNStartNo = MyUtility.Convert.GetString(dr["CTNStartNo"]),
                    TemplatePath = templatePath + MyUtility.Convert.GetString(dr["TemplateName"]),
                    SCICtnNo = MyUtility.Convert.GetString(dr["SCICtnNo"]),
                    ShippingMarkCombinationUkey = MyUtility.Convert.GetLong(dr["ShippingMarkCombinationUkey"]),
                    ShippingMarkTypeUkey = MyUtility.Convert.GetLong(dr["ShippingMarkTypeUkey"]),
                    Side = MyUtility.Convert.GetString(dr["Side"]),
                    Seq = MyUtility.Convert.GetInt(dr["Seq"]),
                    FromBottom = MyUtility.Convert.GetDouble(dr["FromBottom"]),
                    FromRight = MyUtility.Convert.GetDouble(dr["FromRight"]),
                    Width = MyUtility.Convert.GetInt(dr["Width"]),
                    Length = MyUtility.Convert.GetInt(dr["Length"]),
                    FromTemplate = MyUtility.Convert.GetBool(dr["FromTemplate"]),

                    DefineColumns = new List<DefineColumn>(),
                };

                tList.Add(obj);
            }

            // 取得要替換的欄位的值
            List<P24_Template> nList = this.GetTemplateFieldValue(tList.Where(o => o.FromTemplate).ToList());
            if (MyUtility.Check.Empty(nList) || nList.Count == 0)
            {
                if (callFrom == "P26")
                {
                    foreach (var item in this.P24_GenerateResults)
                    {
                        if (!item.IsSuccess)
                        {
                            string tmp;
                            if (item.InformationError != null && item.InformationError.Any())
                            {
                                tmp = "Shipping mark information not maintained in PMS yet : " + item.InformationError.Distinct().OrderBy(o => o).JoinToString(",");
                                p26Result.ResultMsg = tmp;
                            }
                        }
                    }
                }

                return;
            }

            // 若是事前檢查，則無需後續動作
            if (isP26Check)
            {
                return;
            }

            // 轉出HTML檔
            List<P24_Template> bList = this.TransferFile(nList);

            // 全部轉檔成功才UPDATE DB
            if (MyUtility.Check.Empty(bList))
            {
                return;
            }

            // 剛剛被排除掉的SSCC要加回來
            bList.AddRange(tList.Where(o => !o.FromTemplate));

            bool success = this.UpdateDatabese(bList, callFrom);

            if (success)
            {
                #region ISP20201690 資料交換 - Sunrise
                foreach (var packingListID in tList.Select(o => o.PackingListID).Distinct())
                {
                    Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(packingListID, string.Empty))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }
                #endregion

                #region ISP20201607 資料交換 - Gensong
                foreach (var packingListID in tList.Select(o => o.PackingListID).Distinct())
                {
                    Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(packingListID, string.Empty))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }
                #endregion

                if (callFrom == string.Empty)
                {
                    //MyUtility.Msg.InfoBox("Success!!");
                    this.Query();
                }
            }
        }

        // 取得範本欄位對應DB的值
        private List<P24_Template> GetTemplateFieldValue(List<P24_Template> p24_Templates)
        {
            try
            {
                // DISTINCT 出有多少種範本，避免開啟太多次
                List<string> templatePaths = p24_Templates.Select(o => o.TemplatePath).Distinct().ToList();
                List<string> deletePacking = new List<string>();

                foreach (var templatePath in templatePaths)
                {
                    // 取得使用相同範本的箱子
                    List<P24_Template> sameTemplateDatas = p24_Templates.Where(o => o.TemplatePath == templatePath).ToList();

                    if (!System.IO.File.Exists(templatePath))
                    {
                        MyUtility.Msg.WarningBox("Template Path not found!!");
                        return null;
                    }

                    // 開啟範本
                    Repository repository = new Repository(templatePath, false, true)
                    {
                        AutoSave = false,
                    };

                    DataField field = repository.GlobalProject.FirstDataField;
                    List<string> templateFields = new List<string>();

                    // 取得範本所有設定的欄位
                    while (field != null)
                    {
                        templateFields.Add(field.Name);

                        field = field.Next;
                    }

                    // 範本用完關閉
                    repository.Dispose();

                    // 以每一箱的資訊，取得這一箱標籤範本所設定的欄位，及其對應的值
                    foreach (P24_Template sameTemplateData in sameTemplateDatas)
                    {
                        DataTable dt;
                        string cmd = $@"
SELECt * 
FROM dbo.[Get_ShippingMarkTemplate_DefineColumn]('{sameTemplateData.PackingListID}' ,'{sameTemplateData.OrderID}' ,'{sameTemplateData.CTNStartNo}' ,'{sameTemplateData.Refno}' )
WHERE ID IN ('{templateFields.JoinToString("','")}')
";
                        DBProxy.Current.Select(null, cmd, out dt);
                        List<DefineColumn> defineColumns = new List<DefineColumn>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            DefineColumn col = new DefineColumn()
                            {
                                ColumnName = MyUtility.Convert.GetString(dr["ID"]),
                                Value = MyUtility.Convert.GetString(dr["Value"]),
                                ChkEmpty = MyUtility.Convert.GetBool(dr["ChkEmpty"]),
                            };
                            sameTemplateData.DefineColumns.Add(col);

                            // 範本檔中使用的資訊若有空則記錄下來
                            if (MyUtility.Convert.GetBool(dr["ChkEmpty"]) && MyUtility.Check.Empty(dr["Value"]))
                            {
                                var generateResults = this.P24_GenerateResults.Where(o => o.PackingListID == sameTemplateData.PackingListID);
                                if (!generateResults.Any())
                                {
                                    // 有問題的Packing裝起來，等一下要刪掉，以避免進行後續動作
                                    deletePacking.Add(sameTemplateData.PackingListID);

                                    // 若該Packing是第一次加入，則初始化FailItem
                                    List<string> informationError = new List<string>() { sameTemplateData.CTNStartNo };
                                    this.P24_GenerateResults.Add(new P24_GenerateResult()
                                    {
                                        PackingListID = sameTemplateData.PackingListID,
                                        InformationError = informationError,
                                        IsSuccess = false,
                                        Message = "Shipping mark information not maintained in PMS yet.",
                                    });
                                }
                                else
                                {
                                    var data = generateResults.FirstOrDefault();

                                    // 若該Packing是第一次加入，則找出現有的fileItem，加入箱號即可
                                    List<string> informationError = data.InformationError;
                                    informationError.Add(sameTemplateData.CTNStartNo);
                                    data.InformationError = informationError;
                                }
                            }
                        }
                    }
                }

                p24_Templates = p24_Templates.Where(o => !deletePacking.Where(x => x == o.PackingListID).Any()).ToList();

                return p24_Templates;
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
                return null;
            }
        }

        // 產出HTML檔
        private List<P24_Template> TransferFile(List<P24_Template> p24_Templates)
        {
            string shippingMarkPath = MyUtility.GetValue.Lookup("select ShippingMarkPath from System");
            try
            {
                foreach (P24_Template oneCarton in p24_Templates)
                {
                    if (!oneCarton.FromTemplate)
                    {
                        continue;
                    }

                    Job job = new Job() { RepositoryName = oneCarton.TemplatePath, PrinterType = PrinterType.HtmlFile };

                    JobDataRecordSet jobdata = new JobDataRecordSet();
                    Record record = new Record();

                    foreach (DefineColumn oneColumn in oneCarton.DefineColumns)
                    {
                        record.Data.Add(oneColumn.ColumnName, oneColumn.Value);
                    }

                    jobdata.Records.Add(record);
                    job.JobData = jobdata;

                    // FileName目前固定，不同的是路徑
                    string fileName = "HTML.html";
                    string htmlFilePath = shippingMarkPath + oneCarton.PackingListID + @"\" + oneCarton.SCICtnNo + @"\Sticker\" + oneCarton.ShippingMarkTypeUkey.ToString() + @"\";

                    oneCarton.FilePath = htmlFilePath + @"HTML_Pages\";
                    oneCarton.FileName = "page_1.html";

                    if (!System.IO.Directory.Exists(htmlFilePath))
                    {
                        System.IO.Directory.CreateDirectory(htmlFilePath);
                    }

                    job.OutputName = htmlFilePath + fileName;
                    job.Print();

                    job.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
                return null;
            }

            return p24_Templates;
        }

        // DB寫入Packing P24資料
        private bool UpdateDatabese(List<P24_Template> p24_Templates, string callFrom)
        {
            string headInsert = string.Empty;
            string bodyInsert = string.Empty;
            int dPI = MyUtility.Convert.GetInt(ConfigurationManager.AppSettings["TFORMer_HTML_DPI"]);

            // 表頭
            foreach (var packingListID in p24_Templates.Select(o => o.PackingListID).Distinct())
            {
                // 確認是否為Overwrite
                bool p24Existsed = MyUtility.Check.Seek($"SELECT 1 FROM ShippingMarkPic WHERE PackingListID = '{packingListID}'");

                if (p24Existsed)
                {
                    this.P24_GenerateResults.Add(new P24_GenerateResult()
                    {
                        PackingListID = packingListID,
                        Message = "Overwrite success",
                    });
                }
                else
                {
                    this.P24_GenerateResults.Add(new P24_GenerateResult()
                    {
                        PackingListID = packingListID,
                        Message = "Upload success",
                    });
                }

                if (callFrom == string.Empty)
                {
                    headInsert += $@"
---刪除舊有資料
DELETE FROM ShippingMarkPic_Detail
WHERE ShippingMarkPicUkey IN (SELECT Ukey FROM ShippingMarkPic WHERE PackingListID = '{packingListID}')

DELETE FROM ShippingMarkPic
WHERE PackingListID = '{packingListID}'

INSERT INTO ShippingMarkPic (PackingListID ,AddDate ,AddName)
        VALUES ('{packingListID}' ,GETDATE() ,'{Sci.Env.User.UserID}')
";
                }

                headInsert += $@"
IF NOT EXISTS(SELECT 1 FROM ShippingMarkPic WHERE PackingListID = '{packingListID}' )
BEGIN
    INSERT INTO ShippingMarkPic (PackingListID ,AddDate ,AddName)
         VALUES ('{packingListID}' ,GETDATE() ,'{Sci.Env.User.UserID}')
END
ELSE
BEGIN
    UPDATE ShippingMarkPic 
    SET EditDate=GETDATE() ,EditName='{Sci.Env.User.UserID}' 
    WHERE PackingListID = '{packingListID}'
END
;
";
            }

            // 表身：透過P26先寫入圖片再產生HTML，因此不可以刪掉表身
            foreach (P24_Template p24_Template in p24_Templates)
            {
                bodyInsert += $@"
IF EXISTS(
    SELECT 1 FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey = (SELECT  Ukey FROM ShippingMarkPic WHERE PackingListID = '{p24_Template.PackingListID}') 
    AND SCICtnNo = '{p24_Template.SCICtnNo}'
    AND ShippingMarkTypeUkey = '{p24_Template.ShippingMarkTypeUkey}'
)
BEGIN
    ----- ShippingMarkType.FromTemplate = 1 才能有HTML
    UPDATE ShippingMarkPic_Detail
    SET FilePath = '{p24_Template.FilePath}'
        ,FileName = '{p24_Template.FileName}'
        ,DPI = {dPI}
        ,Image = NULL
        ,IsSSCC = (SELECT IsSSCC FROM ShippingMarkType WHERE Ukey = {p24_Template.ShippingMarkTypeUkey})
    WHERE ShippingMarkPicUkey = (SELECT  Ukey FROM ShippingMarkPic WHERE PackingListID = '{p24_Template.PackingListID}') 
    AND SCICtnNo = '{p24_Template.SCICtnNo}'
    AND ShippingMarkTypeUkey = '{p24_Template.ShippingMarkTypeUkey}'
    AND ShippingMarkTypeUkey IN (SELECT Ukey FROM ShippingMarkType t WHERE t.FromTemplate = 1)

    ---- ShippingMarkType.FromTemplate = 0 的FilePath FileName清空
    UPDATE ShippingMarkPic_Detail
    SET FilePath = ''
        ,FileName = ''
        ,DPI = 0
        ,IsSSCC = (SELECT IsSSCC FROM ShippingMarkType WHERE Ukey = {p24_Template.ShippingMarkTypeUkey})
    WHERE ShippingMarkPicUkey = (SELECT  Ukey FROM ShippingMarkPic WHERE PackingListID = '{p24_Template.PackingListID}') 
    AND SCICtnNo = '{p24_Template.SCICtnNo}'
    AND ShippingMarkTypeUkey IN (SELECT Ukey FROM ShippingMarkType t WHERE t.FromTemplate = 0)
END
ELSE
BEGIN
    INSERT INTO ShippingMarkPic_Detail
               (ShippingMarkPicUkey, IsSSCC, SCICtnNo ,ShippingMarkCombinationUkey ,ShippingMarkTypeUkey ,FilePath ,FileName ,Side ,Seq ,FromRight ,FromBottom ,Width ,Length ,DPI)
         VALUES
               ( (SELECT  Ukey FROM ShippingMarkPic WHERE PackingListID = '{p24_Template.PackingListID}') 
               ,(SELECT IsSSCC FROM ShippingMarkType WHERE Ukey = {p24_Template.ShippingMarkTypeUkey})
               ,'{p24_Template.SCICtnNo}'
               ,{p24_Template.ShippingMarkCombinationUkey}
               ,{p24_Template.ShippingMarkTypeUkey}
               ,'{p24_Template.FilePath}'
               ,'{p24_Template.FileName}'
               ,'{p24_Template.Side}'
               ,{p24_Template.Seq}
               ,{p24_Template.FromRight}
               ,{p24_Template.FromBottom}
               ,{p24_Template.Width}
               ,{p24_Template.Length}
               ,{dPI} )
END
;
";
            }

            using (TransactionScope transactionScope = new TransactionScope())
            {
                DualResult r;
                if (MyUtility.Check.Empty(headInsert))
                {
                    transactionScope.Dispose();
                    return false;
                }

                r = DBProxy.Current.Execute(null, headInsert);
                if (!r)
                {
                    this.ShowErr(r);
                    transactionScope.Dispose();
                    return false;
                }

                r = DBProxy.Current.Execute(null, bodyInsert);

                if (!r)
                {
                    transactionScope.Dispose();

                    // 由於有包transaction，因此不是全部成功就是全部失敗
                    foreach (var item in this.P24_GenerateResults.Where(o => p24_Templates.Where(x => x.PackingListID == o.PackingListID).Any()))
                    {
                        item.IsSuccess = true;
                        item.Message = "Database Error: " + r.GetException().Message;
                    }

                    this.ShowErr(r);
                    return false;
                }

                // 由於有包transaction，因此不是全部成功就是全部失敗
                foreach (var item in this.P24_GenerateResults.Where(o => p24_Templates.Where(x => x.PackingListID == o.PackingListID).Any()))
                {
                    item.IsSuccess = true;
                }

                transactionScope.Complete();
                transactionScope.Dispose();
            }

            return true;
        }

        /// <inheritdoc/>
        public void ShowResult()
        {
            DataTable dt = new DataTable();
            dt.ColumnsStringAdd("PackingListID");
            dt.ColumnsStringAdd("Reault");
            dt.ColumnsStringAdd("Message");

            foreach (var item in this.P24_GenerateResults)
            {
                DataRow dr = dt.NewRow();
                dr["PackingListID"] = item.PackingListID;
                dr["Reault"] = item.IsSuccess ? "Success" : "Fail";
                string msg = string.Empty;
                List<string> msgList = new List<string>();
                if (item.IsSuccess)
                {
                    msgList.Add(item.Message);
                }
                else
                {
                    string tmp;
                    if (item.BasicSettingError != null && item.BasicSettingError.Any())
                    {
                        tmp = "Sticker basic setting not yet complete : " + item.BasicSettingError.Distinct().OrderBy(o => o).JoinToString(",");
                        msgList.Add(tmp);
                    }

                    if (item.CtnError != null && item.CtnError.Any())
                    {
                        tmp = "Carton already in Clog : " + item.CtnError.Distinct().OrderBy(o => o).JoinToString(",");
                        msgList.Add(tmp);
                    }

                    if (item.InformationError != null && item.InformationError.Any())
                    {
                        tmp = "Shipping mark information not maintained in PMS yet : " + item.InformationError.Distinct().OrderBy(o => o).JoinToString(",");
                        msgList.Add(tmp);
                    }
                }

                dr["Message"] = msgList.JoinToString(Environment.NewLine);

                dt.Rows.Add(dr);
            }

            MsgGridForm form = new MsgGridForm(dt, "Check below message.", "P24 Generate Results");

            form.grid1.Columns[0].Width = 150;
            form.grid1.Columns[1].Width = 200;
            form.grid1.Columns[2].Width = 400;
            form.Width = 800;
            form.grid1.RowHeadersVisible = true;
            form.ControlBox = true;
            form.ShowDialog();
        }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public class P24_Template
    {
        /// <inheritdoc/>
        public string PackingListID { get; set; }

        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public string OrderID { get; set; }

        /// <inheritdoc/>
        public string Refno { get; set; }

        /// <inheritdoc/>
        public string CTNStartNo { get; set; }

        /// <inheritdoc/>
        public string SCICtnNo { get; set; }

        /// <inheritdoc/>
        public long ShippingMarkCombinationUkey { get; set; }

        /// <inheritdoc/>
        public long ShippingMarkTypeUkey { get; set; }

        /// <inheritdoc/>
        public string FileName { get; set; }

        /// <inheritdoc/>
        public string FilePath { get; set; }

        /// <inheritdoc/>
        public string Side { get; set; }

        /// <inheritdoc/>
        public int Seq { get; set; }

        /// <inheritdoc/>
        public double FromRight { get; set; }

        /// <inheritdoc/>
        public double FromBottom { get; set; }

        /// <inheritdoc/>
        public int Width { get; set; }

        /// <inheritdoc/>
        public int Length { get; set; }

        /// <inheritdoc/>
        public string TemplatePath { get; set; }

        /// <inheritdoc/>
        public bool FromTemplate { get; set; }

        /// <inheritdoc/>
        public List<DefineColumn> DefineColumns { get; set; }
    }

    /// <summary>
    /// Generate結果收集
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public class P24_GenerateResult
    {
        /// <inheritdoc/>
        public string PackingListID { get; set; }

        /// <inheritdoc/>
        public bool IsSuccess { get; set; }

        /// <inheritdoc/>
        public List<string> BasicSettingError { get; set; }

        public List<string> InformationError { get; set; }

        public List<string> CtnError { get; set; }

        /// <inheritdoc/>
        public string Message { get; set; }
    }
}
