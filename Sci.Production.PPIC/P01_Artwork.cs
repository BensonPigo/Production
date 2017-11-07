using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_Artwork
    /// </summary>
    public partial class P01_Artwork : Sci.Win.Subs.Input4
    {
        /// <summary>
        /// P01_Artwork
        /// </summary>
        /// <param name="canedit">bool canedit</param>
        /// <param name="keyvalue1">string keyvalue1</param>
        /// <param name="keyvalue2">string keyvalue2</param>
        /// <param name="keyvalue3">string keyvalue3</param>
        /// <param name="styleid">string styleid</param>
        /// <param name="seasonid">string seasoni</param>
        public P01_Artwork(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string styleid, string seasonid)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.Text = "Artwork <" + styleid + "-" + seasonid + ">";
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20))
                .Text("Article", header: "Article", width: Widths.AnsiChars(8))
                .Text("PatternCode", header: "Cut Part", width: Widths.AnsiChars(10))
                .Text("PatternDesc", header: "Description", width: Widths.AnsiChars(20))
                .Text("ArtworkID", header: "Pattern#", width: Widths.AnsiChars(15))
                .Text("ArtworkName", header: "Pattern Description", width: Widths.AnsiChars(30))
                .Numeric("Qty", header: string.Empty, width: Widths.AnsiChars(5))
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8))
                .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5))
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(8), decimal_places: 4)
                .Numeric("Cost", header: "Cost", width: Widths.AnsiChars(8), decimal_places: 4)
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(30))
                .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(30))
                .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(30));

            return true;
        }

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            string sqlCmd = string.Format(
                @"select oa.ArtworkTypeID,a.ArtworkUnit 
from Order_Artwork oa WITH (NOLOCK) 
left join ArtworkType a WITH (NOLOCK) on oa.ArtworkTypeID = a.ID
where oa.ID = '{0}'", this.KeyValue1);
            DataTable artworkUnit;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out artworkUnit);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Query unit fail!!\r\n" + result.ToString());
            }

            datas.Columns.Add("UnitID");
            datas.Columns.Add("CreateBy");
            datas.Columns.Add("EditBy");
            foreach (DataRow gridData in datas.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                if (gridData["EditDate"] != System.DBNull.Value)
                {
                    gridData["EditBy"] = gridData["EditName"].ToString() + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                }

                DataRow[] findrow = artworkUnit.Select(string.Format("ArtworkTypeID = '{0}'", gridData["ArtworkTypeID"].ToString()));
                if (findrow.Length > 0)
                {
                    gridData["UnitID"] = findrow[0]["ArtworkUnit"].ToString();
                }

                gridData.AcceptChanges();
            }
        }
    }
}
