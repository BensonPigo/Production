using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using TECIT.TFORMer;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P27_Generate : Win.Tems.QueryForm
    {
        private string packingListID;
        private string brandID;
        private bool includeAlreadyGenerated;
        private DateTime? SCIDelivery_s;
        private DateTime? SCIDelivery_e;

        /// <inheritdoc/>
        public P27_Generate()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            DataGridViewGeneratorCheckBoxColumnSettings col_Selected = new DataGridViewGeneratorCheckBoxColumnSettings();

            col_Selected.HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None;
            col_Selected.CellEditable += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetInt(dr["CTNQty"]) != MyUtility.Convert.GetInt(dr["CompleteCtn"]))
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
              .Numeric("CompleteCtn", header: "CTN Qty" + Environment.NewLine + "Stamp Setting Cmplt", width: Widths.AnsiChars(10), iseditingreadonly: false)
              .CheckBox("AlreadyGenerateStampFile", header: "Already Generate Stamp File", width: Widths.AnsiChars(20), iseditable: false)
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

            if (!this.includeAlreadyGenerated)
            {
                where_include = $@"
AND IIF(EXISTS(SELECT 1 FROM ShippingMarkStamp stmp 
										    INNER JOIN ShippingMarkStamp_Detail stmpD ON stmp.PackingListID= stmpD.PackingListID
										    WHERE stmp.PackingListID = p.ID) ,1 ,0) = 0
";
            }

            #endregion

            #region SQL
            string cmd = $@"
SELECT DISTINCT
 p.ID
,p.BrandID
,pd.CTNStartNo
,pd.RefNo
,[ShippingMarkCombinationUkey] = ISNULL(c.StampCombinationUkey,comb.Ukey)
,p.CTNQty
INTO #base
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
INNER JOIN CustCD c ON c.ID = p.CustCDID
LEFT JOIN Pullout pu ON p.PulloutID = pu.ID
OUTER APPLY(
	SELECT *
	FROM ShippingMarkCombination s
	WHERE s.BrandID=p.BrandID AND s.Category='HTML' AND IsDefault = 1
)comb
WHERE  (pu.Status != 'New' OR pu.Status IS NULL)
{where}


SELECT DISTINCT
     [Selected] = IIF(CompleteCtn.Val =p.CTNQty ,1 ,0)
    ,p.ID
    ,p.BrandID
    ,[CompleteCtn] = CompleteCtn.Val
    ,p.CTNQty
    ,[AlreadyGenerateStampFile]=IIF(EXISTS(SELECT 1 FROM ShippingMarkStamp stmp 
										    INNER JOIN ShippingMarkStamp_Detail stmpD ON stmp.PackingListID= stmpD.PackingListID
										    WHERE stmp.PackingListID = p.ID) ,1 ,0)
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
LEFT JOIN Pullout pu ON p.PulloutID = pu.ID
OUTER APPLY(
	SELECT [Val]=COUNT(b.CTNStartNo)
	FROM  #base b
	INNER JOIN  ShippingMarkPicture pict ON pict.BrandID = b.BrandID 
							AND pict.Category='HTML' 
							AND pict.CTNRefno = b.RefNo 
							AND pict.ShippingMarkCombinationUkey = b.ShippingMarkCombinationUkey

	INNER JOIN ShippingMarkPicture_Detail pictD ON pict.Ukey = pictD.ShippingMarkPictureUkey
	INNER JOIN ShippingMarkType t ON t.Ukey = pictD.ShippingMarkTypeUkey
	INNER JOIN ShippingMarkType_Detail td ON t.Ukey = td.ShippingMarkTypeUkey AND td.StickerSizeID = pictD.StickerSizeID
	WHERE td.TemplateName <> '' AND b.ID = p.ID
)CompleteCtn
WHERE  (pu.Status != 'New' OR pu.Status IS NULL)
{where}
{where_include}

ORDER BY p.ID

DROP TABLE #base
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
            this.includeAlreadyGenerated = this.chkIncludeAlreadyGenerated.Checked;

            return true;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            DataTable dataTable;
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
            this.Generate(selecteds);
            this.HideWaitMessage();
        }

        private void Generate(DataRow[] selecteds)
        {
            List<string> packingListIDs = selecteds.Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct().ToList();
            List<P27_Template> tList = new List<P27_Template>();

            // 取得每一箱的資訊、以及對應的範本名稱
            #region SQL
            string cmd = $@"
SELECT DISTINCT
     p.ID
    ,p.BrandID
    ,pd.OrderID
    ,pd.CTNStartNo
    ,pd.RefNo
    ,pd.SCICtnNo
    ,[ShippingMarkCombinationUkey] = ISNULL(c.StampCombinationUkey,comb.Ukey)
    ,p.CTNQty
INTO #base
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
INNER JOIN CustCD c ON c.ID = p.CustCDID
LEFT JOIN Pullout pu ON p.PulloutID = pu.ID
OUTER APPLY(
	SELECT *
	FROM ShippingMarkCombination s
	WHERE s.BrandID=p.BrandID AND s.Category='HTML' AND IsDefault = 1
)comb
WHERE  (pu.Status != 'New' OR pu.Status IS NULL)
AND p.ID IN ('{packingListIDs.JoinToString("','")}')


SELECT DISTINCT [PackingListID]=b.ID   ----對應ShippingMarkStamp和ShippingMarkStamp_Detail的 [PackingListID]
    ,b.BrandID
    ,b.OrderID
    ,b.RefNo
    ,b.CTNStartNo
    ,td.TemplateName
	
	----對應ShippingMarkStamp_Detail
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
INNER JOIN  ShippingMarkPicture pict ON pict.BrandID = b.BrandID 
						AND pict.Category='HTML' 
						AND pict.CTNRefno = b.RefNo 
						AND pict.ShippingMarkCombinationUkey = b.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkPicture_Detail pictD ON pict.Ukey = pictD.ShippingMarkPictureUkey
INNER JOIN ShippingMarkType t ON t.Ukey = pictD.ShippingMarkTypeUkey
INNER JOIN ShippingMarkType_Detail td ON t.Ukey = td.ShippingMarkTypeUkey AND td.StickerSizeID = pictD.StickerSizeID
INNER JOIN StickerSize st ON st.ID = pictD.StickerSizeID
WHERE td.TemplateName <> ''
";
            #endregion

            DualResult r = DBProxy.Current.Select(null, cmd, out DataTable dt);

            if (!r)
            {
                this.ShowErr(r);
                return;
            }

            // 組合出物件方便使用
            foreach (DataRow dr in dt.Rows)
            {
                string templatePath = MyUtility.GetValue.Lookup("SELECT ShippingMarkTemplatePath FROM System");
                P27_Template obj = new P27_Template()
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

                    DefineColumns = new List<DefineColumn>(),
                };

                tList.Add(obj);
            }

            // 取得要替換的欄位的值
            List<P27_Template> nList = this.GetTemplateFieldValue(tList);
            if (MyUtility.Check.Empty(nList))
            {
                return;
            }

            // 轉出HTML檔
            List<P27_Template> bList = this.TransferFile(nList);

            // 全部轉檔成功才UPDATE DB
            if (MyUtility.Check.Empty(bList))
            {
                return;
            }

            bool success = this.UpdateDatabese(bList);

            if (success)
            {
                MyUtility.Msg.InfoBox("Success!!");
                this.Query();
            }
        }

        // 取得範本欄位對應DB的值
        private List<P27_Template> GetTemplateFieldValue(List<P27_Template> p27_Templates)
        {
            try
            {
                // DISTINCT 出有多少種範本，避免開啟太多次
                List<string> templatePaths = p27_Templates.Select(o => o.TemplatePath).Distinct().ToList();

                foreach (var templatePath in templatePaths)
                {
                    // 取得使用相同範本的箱子
                    List<P27_Template> sameTemplateDatas = p27_Templates.Where(o => o.TemplatePath == templatePath).ToList();

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
                    foreach (P27_Template sameTemplateData in sameTemplateDatas)
                    {
                        DataTable dt;
                        string cmd = $@"
SELECt * 
FROM dbo.[Get_ShippingMarkTemplate_DefineColumn]('{sameTemplateData.PackingListID}' ,'{sameTemplateData.OrderID}' ,'{sameTemplateData.SCICtnNo}' ,'{sameTemplateData.CTNStartNo}' ,'{sameTemplateData.Refno}' )
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
                            };
                            sameTemplateData.DefineColumns.Add(col);
                        }
                    }
                }

                // 取得欄位沒資料的Packing List ID
                var hasEmptyDatas = p27_Templates.Where(o => o.DefineColumns.Where(x => MyUtility.Check.Empty(x.Value)).Any()).Select(o => o.PackingListID).Distinct().ToList();

                if (hasEmptyDatas.Any())
                {
                    MyUtility.Msg.WarningBox("Please check below packing list shipping mark information in PMS." + Environment.NewLine + hasEmptyDatas.JoinToString(Environment.NewLine));
                    return null;
                }
                else
                {
                    return p27_Templates;
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
                return null;
            }
        }

        // 產出HTML檔
        private List<P27_Template> TransferFile(List<P27_Template> p27_Templates)
        {
            string shippingMarkPath = MyUtility.GetValue.Lookup("select ShippingMarkPath from System");
            try
            {
                foreach (P27_Template oneCarton in p27_Templates)
                {
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
                    string htmlFilePath = shippingMarkPath + oneCarton.PackingListID + @"\" + oneCarton.SCICtnNo + @"\Stamp\" + oneCarton.ShippingMarkTypeUkey.ToString() + @"\";

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

            return p27_Templates;
        }

        // DB寫入Packing P27資料
        private bool UpdateDatabese(List<P27_Template> p27_Templates)
        {
            string headInsert = string.Empty;
            string bodyInsert = string.Empty;

            // 表頭
            foreach (var packingListID in p27_Templates.Select(o => o.PackingListID).Distinct())
            {
                headInsert += $@"
IF NOT EXISTS(SELECT 1 FROM ShippingMarkStamp WHERE PackingListID = '{packingListID}' )
BEGIN
    INSERT INTO ShippingMarkStamp (PackingListID ,AddDate ,AddName)
         VALUES ('{packingListID}' ,GETDATE() ,'{Sci.Env.User.UserID}')
END
ELSE
BEGIN
    UPDATE ShippingMarkStamp 
    SET EditDate=GETDATE() ,EditName='{Sci.Env.User.UserID}' 
    WHERE PackingListID = '{packingListID}'
END
;
";
            }

            // 表身
            foreach (P27_Template p27_Template in p27_Templates)
            {
                bodyInsert += $@"DELETE FROM ShippingMarkStamp_Detail WHERE PackingListID = '{p27_Template.PackingListID}' AND SCICtnNo ='{p27_Template.SCICtnNo}' AND ShippingMarkTypeUkey={p27_Template.ShippingMarkTypeUkey} ;";
                bodyInsert += $@"

INSERT INTO ShippingMarkStamp_Detail
           (PackingListID ,SCICtnNo ,ShippingMarkCombinationUkey ,ShippingMarkTypeUkey ,FilePath ,FileName ,Side ,Seq ,FromRight ,FromBottom ,Width ,Length)
     VALUES
           ('{p27_Template.PackingListID}'
           ,'{p27_Template.SCICtnNo}'
           ,{p27_Template.ShippingMarkCombinationUkey}
           ,{p27_Template.ShippingMarkTypeUkey}
           ,'{p27_Template.FilePath}'
           ,'{p27_Template.FileName}'
           ,'{p27_Template.Side}'
           ,{p27_Template.Seq}
           ,{p27_Template.FromRight}
           ,{p27_Template.FromBottom}
           ,{p27_Template.Width}
           ,{p27_Template.Length})
;
";
            }

            using (TransactionScope transactionScope = new TransactionScope())
            {
                DualResult r;
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
                    this.ShowErr(r);
                    transactionScope.Dispose();
                    return false;
                }

                transactionScope.Complete();
                transactionScope.Dispose();
            }

            return true;
        }
    }

    /// <inheritdoc/>
    public class P27_Template
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
        public List<DefineColumn> DefineColumns { get; set; }
    }

    /// <inheritdoc/>
    public class DefineColumn
    {
        /// <inheritdoc/>
        public string ColumnName { get; set; }

        /// <inheritdoc/>
        public string Value { get; set; }
    }
}
