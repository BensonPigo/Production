using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_TMSAndCost
    /// </summary>
    public partial class P01_TMSAndCost : Sci.Win.Subs.Input4
    {
        /// <summary>
        /// P01_TMSAndCost
        /// </summary>
        /// <param name="canedit">bool canedit</param>
        /// <param name="keyvalue1">string keyvalue1</param>
        /// <param name="keyvalue2">string keyvalue2</param>
        /// <param name="keyvalue3">string keyvalue3</param>
        public P01_TMSAndCost(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.Text = "TMS & Cost (" + keyvalue1 + ")";
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
            Ict.Win.DataGridViewGeneratorNumericColumnSettings tms = new DataGridViewGeneratorNumericColumnSettings();
            Ict.Win.DataGridViewGeneratorNumericColumnSettings price = new DataGridViewGeneratorNumericColumnSettings();
            qty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            tms.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            price.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Seq", header: "Seq#", width: Widths.AnsiChars(4))
                .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20))
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), settings: qty)
                .Text("ArtworkUnit", header: "Unit", width: Widths.AnsiChars(8))
                .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5), settings: tms)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 3, settings: price)
                .Text("IsTtlTMS", header: "Ttl TMS", width: Widths.AnsiChars(8));

            return true;
        }

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            string sqlCmd = "select ID,IsTMS,IsPrice,IsTtlTMS,Classify from ArtworkType WITH (NOLOCK) ";
            DataTable artworkType;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out artworkType);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Query ArtworkType fail!!\r\n" + result.ToString());
            }

            datas.Columns.Add("isTms");
            datas.Columns.Add("isPrice");
            datas.Columns.Add("IsTtlTMS");
            datas.Columns.Add("Classify");
            datas.Columns.Add("NewData");
            foreach (DataRow gridData in datas.Rows)
            {
                DataRow[] findrow = artworkType.Select(string.Format("ID = '{0}'", gridData["ArtworkTypeID"].ToString()));
                if (findrow.Length > 0)
                {
                    gridData["isTms"] = findrow[0]["isTms"].ToString();
                    gridData["isPrice"] = findrow[0]["isPrice"].ToString();
                    gridData["IsTtlTMS"] = findrow[0]["IsTtlTMS"].ToString().ToUpper() == "TRUE" ? "Y" : "N";
                    gridData["Classify"] = findrow[0]["Classify"].ToString();
                    gridData["NewData"] = 0;
                }

                gridData.AcceptChanges();
            }

            #region 計算Ttl TMS
            sqlCmd = string.Format(
                @"select isnull(sum(TMS),0) as TtlTMS from Order_TmsCost ot WITH (NOLOCK) , ArtworkType at WITH (NOLOCK) 
where ot.ArtworkTypeID = at.ID
and at.IsTtlTMS = 1
and ot.ID = '{0}'", this.KeyValue1);
            this.numTTLTMS.Value = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlCmd));
            #endregion

            /*
            #region 撈新增的ArtworkType
            sqlCmd = string.Format(
                @"select a.* from (
select a.ID,a.Classify,a.Seq,a.ArtworkUnit,a.IsTMS,a.IsPrice,a.IsTtlTMS,isnull(ot.ID,'') as OrderID
from ArtworkType a WITH (NOLOCK)
left join Order_TmsCost ot WITH (NOLOCK) on a.ID = ot.ArtworkTypeID and ot.ID = '{0}'
where a.SystemType = 'T' and a.Junk = 0) a
where a.OrderID = ''", this.KeyValue1);

            DataTable tmpArtworkType;
            result = DBProxy.Current.Select(null, sqlCmd, out tmpArtworkType);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query LackArtworkType fail!!\r\n" + result.ToString());
            }

            DataRow newdr;
            foreach (DataRow dr in tmpArtworkType.Rows)
            {
                newdr = datas.NewRow();
                newdr["ID"] = this.KeyValue1;
                newdr["ArtworkTypeID"] = dr["ID"].ToString();
                newdr["Seq"] = dr["Seq"].ToString();
                newdr["Qty"] = 0;
                newdr["ArtworkUnit"] = dr["ArtworkUnit"].ToString();
                newdr["TMS"] = 0;
                newdr["Price"] = 0;
                newdr["AddName"] = Sci.Env.User.UserID;
                newdr["AddDate"] = DateTime.Now;
                newdr["isTms"] = dr["isTms"].ToString();
                newdr["isPrice"] = dr["isPrice"].ToString();
                newdr["IsTtlTMS"] = dr["IsTtlTMS"].ToString().ToUpper() == "TRUE" ? "Y" : "N";
                newdr["Classify"] = dr["Classify"].ToString();
                newdr["NewData"] = 1;
                datas.Rows.Add(newdr);
            }
            #endregion
            */

            datas.DefaultView.Sort = "Seq";
        }
    }
}
