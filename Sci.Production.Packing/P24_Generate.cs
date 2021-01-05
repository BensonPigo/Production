using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
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
              .CheckBox("NoStickerBasicSetting", header: "No Sticker Basic Setting", width: Widths.AnsiChars(20), iseditable: false)
              .CheckBox("StickerAlreadyExisted", header: "Sticker Already existed", width: Widths.AnsiChars(20), iseditable: false)
              .CheckBox("CtnInClog", header: "Ctn In Clog", width: Widths.AnsiChars(20), iseditable: false)
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
	WHERE s.BrandID=p.BrandID AND s.Category='PIC' AND IsDefault = 1
)comb
WHERE  (pu.Status != 'New' OR pu.Status IS NULL)
{where}


SELECT DISTINCT
     [Selected] = IIF(CompleteCtn.Val >= p.CTNQty AND CtnInClog.Val = 0 ,1 ,0) /* 預設勾選No Sticker Basic Setting = False 且 Ctn In Clog = False */
    ,p.ID
    ,p.BrandID
    ,p.CTNQty
    ,[NoStickerBasicSetting] = IIF(CompleteCtn.Val < p.CTNQty , 1, 0)
    ,[StickerAlreadyExisted]=IIF(EXISTS(SELECT 1 FROM ShippingMarkPic pic 
										    INNER JOIN ShippingMarkPic_Detail picD ON pic.Ukey= picD.ShippingMarkPicUkey
										    WHERE pic.PackingListID = p.ID) ,1 ,0)
	,[CtnInClog] = IIF(CtnInClog.Val > 0 , 1 ,0)
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
LEFT JOIN Pullout pu ON p.PulloutID = pu.ID
OUTER APPLY(
	SELECT [Val]=COUNT(b.CTNStartNo)
	FROM  #base b
	INNER JOIN  ShippingMarkPicture pict ON pict.BrandID = b.BrandID 
							AND pict.Category='PIC' 
							AND pict.CTNRefno = b.RefNo 
							AND pict.ShippingMarkCombinationUkey = b.ShippingMarkCombinationUkey

	INNER JOIN ShippingMarkPicture_Detail pictD ON pict.Ukey = pictD.ShippingMarkPictureUkey
	INNER JOIN ShippingMarkType t ON t.Ukey = pictD.ShippingMarkTypeUkey
	INNER JOIN ShippingMarkType_Detail td ON t.Ukey = td.ShippingMarkTypeUkey AND td.StickerSizeID = pictD.StickerSizeID
	WHERE td.TemplateName <> '' AND b.ID = p.ID
)CompleteCtn
OUTER APPLY(
	SELECT [Val] = COUNT(Ukey)
	FROM PackingList_Detail pdd
	WHERE pdd.ID = pd.ID AND pdd.ReceiveDate IS NOT NULL 
)CtnInClog
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

        private void BtnGenerate_Click(object sender, EventArgs e)
        {

        }
    }
}
