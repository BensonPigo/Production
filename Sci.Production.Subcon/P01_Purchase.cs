using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;
using System.Linq;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P01_Purchase : Win.Subs.Base
    {
        private DataRow dr;
        private DataTable dtSPList;
        private DataTable dtQuantity;
        private DataTable dtAccumulated;
        private DataSet data = new DataSet();

        /// <inheritdoc/>
        public P01_Purchase(DataRow mainRow)
        {
            this.InitializeComponent();
            this.dr = mainRow;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string artworkPOID = this.dr["id"].ToString();
            string artworkTypeID = this.dr["ArtworkTypeID"].ToString();

            #region Grid1 - Sql command
            string selectCommand1
                = string.Format(
                    @"
select oq.ID
	, [Qty] = SUM(oq.Qty) * ot.TmsQty
	, ap.PoQty
from Order_Qty oq with (nolock)
outer apply (
	select TmsQty = SUM(Qty)
	from Order_TmsCost ot WITH (NOLOCK) 
	where ot.ID = oq.ID
)ot
outer apply (
	select apd.OrderID
		, [PoQty] = sum(apd.PoQty)
	from ArtworkPO ap WITH (NOLOCK) 
	inner join ArtworkPO_Detail apd WITH (NOLOCK) on ap.id=apd.id 
	where ap.ID = '{0}'
	and ap.ArtworkTypeID = '{1}'
	and ap.ApvDate is not null
	and apd.OrderID = oq.ID
	group by apd.OrderID
)ap
where exists (select 1 from ArtworkPO_Detail ad with (nolock) where ad.ID = '{0}' and ad.OrderID = oq.ID)
group by oq.ID, ot.TmsQty, ap.PoQty",
                    artworkPOID,
                    artworkTypeID);
            #endregion

            #region Grid2 - Sql Command

            string selectCommand2
                = string.Format(
                    @"
select oq.ID
	, oq.Article
	, oq.SizeCode 
	, [Qty] = SUM(oq.Qty) * ot.TmsQty
from Order_Qty oq WITH (NOLOCK) 
outer apply (
	select TmsQty = SUM(Qty)
	from Order_TmsCost ot WITH (NOLOCK) 
	where ot.ID = oq.ID
)ot
where exists (select 1 from ArtworkPO_Detail ad with (nolock) where ad.ID = '{0}' and ad.OrderID = oq.ID)
group by  oq.ID, oq.Article, oq.SizeCode, ot.TmsQty",
                    artworkPOID);

            #endregion

            #region Grid3 - Sql Command

            string selectCommand3
                = string.Format(
                    @"
select apd.OrderID
	, [Article] = iif(ISNULL(apd.Article,'') = '', '--', apd.Article)
	, [SizeCode] = iif(ISNULL(apd.SizeCode,'') = '', '--', apd.SizeCode)
	, [PoQty] = sum(apd.PoQty)
from ArtworkPO ap WITH (NOLOCK) 
inner join ArtworkPO_Detail apd WITH (NOLOCK) on ap.id=apd.id 
where ap.ID = '{0}'
and ap.ArtworkTypeID = '{1}'
and ap.ApvDate is not null
group by apd.OrderID, apd.Article, apd.SizeCode",
                    artworkPOID,
                    artworkTypeID);

            #endregion
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out this.dtSPList);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }

            DualResult selectResult2 = DBProxy.Current.Select(null, selectCommand2, out this.dtQuantity);
            if (selectResult2 == false)
            {
                this.ShowErr(selectCommand2, selectResult2);
            }

            DualResult selectResult3 = DBProxy.Current.Select(null, selectCommand3, out this.dtAccumulated);
            if (selectResult3 == false)
            {
                this.ShowErr(selectCommand3, selectResult3);
            }

            this.dtSPList.TableName = "dtSPList";
            this.dtQuantity.TableName = "dtQuantity";
            this.dtAccumulated.TableName = "dtAccumulated";

            this.data.Tables.Add(this.dtSPList);
            this.data.Tables.Add(this.dtQuantity);
            this.data.Tables.Add(this.dtAccumulated);

            if (this.dtSPList.Rows.Count == 0 || this.dtQuantity.Rows.Count == 0)
            {
                // MyUtility.Msg.ErrorBox("Data not found!!");
                return;
            }

            DataRelation relation = new DataRelation(
                                "Rol1",
                                new DataColumn[] { this.dtSPList.Columns["ID"] },
                                new DataColumn[] { this.dtQuantity.Columns["ID"] });

            DataRelation relation2 = new DataRelation(
                "Rol2",
                new DataColumn[] { this.dtSPList.Columns["ID"] },
                new DataColumn[] { this.dtAccumulated.Columns["OrderID"] });

            this.data.Relations.Add(relation);
            this.data.Relations.Add(relation2);

            this.bindingSource1.DataSource = this.data;
            this.bindingSource1.DataMember = "dtSPList";
            this.bindingSource2.DataSource = this.bindingSource1;
            this.bindingSource2.DataMember = "Rol1";
            this.bindingSource3.DataSource = this.bindingSource1;
            this.bindingSource3.DataMember = "Rol2";

            // 設定Grid1的顯示欄位
            this.gridSPList.IsEditingReadOnly = true;
            this.gridSPList.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridSPList)
                 .Text("ID", header: "SP#", width: Widths.AnsiChars(15))
                 .Numeric("Qty", header: "Ttl Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("PoQty", header: "Accumulated PO Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2);

            // 設定Grid2的顯示欄位
            this.gridQuantity.IsEditingReadOnly = true;
            this.gridQuantity.DataSource = this.bindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridQuantity)
                .Text("Article", header: "Article", width: Widths.AnsiChars(13))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(13))
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2);

            // 設定Grid3的顯示欄位
            this.gridAccumulated.IsEditingReadOnly = true;
            this.gridAccumulated.DataSource = this.bindingSource3;
            this.Helper.Controls.Grid.Generator(this.gridAccumulated)
                 .Text("Article", header: "Dyelot", width: Widths.AnsiChars(13))
                 .Text("SizeCode", header: "Rolls", width: Widths.AnsiChars(13))
                 .Numeric("PoQty", header: "Accumulated PO Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}