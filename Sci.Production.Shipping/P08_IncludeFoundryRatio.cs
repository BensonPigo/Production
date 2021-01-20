using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P08_IncludeFoundryRatio : Win.Tems.QueryForm
    {
        private DataRow CurrentMaintain;

        /// <inheritdoc/>
        public P08_IncludeFoundryRatio(DataRow currentMaintain)
        {
            this.InitializeComponent();

            this.CurrentMaintain = currentMaintain;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable dtDetail;
            DataTable dtFinal;
            DualResult r;
            string sqlCmd = string.Empty;

            try
            {
                sqlCmd = $@"

SELECT  DISTINCT InvNo
INTO #Base
from ShareExpense se WITH (NOLOCK) 
INNER JOIN GMTBooking g ON se.InvNo = g.ID
where   ShippingAPID = '{this.CurrentMaintain["ID"]}'
and (Junk = 0 or Junk is null)
AND g.Foundry = 1

SELECT   b.InvNo
		,gf.FactoryGroup
		,[TtlRatio]=SUM(Ratio)
FROM #Base b
INNER JOIN GarmentInvoice_Foundry gf ON b.InvNo = gf.InvoiceNo
GROUP BY  b.InvNo ,gf.FactoryGroup

DROP TABLE #Base
";
                r = DBProxy.Current.Select(null, sqlCmd, out dtDetail);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dtDetail.Rows.Count == 0)
                {
                    return;
                }

                string factoryGroup = "[" + dtDetail.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["FactoryGroup"])).Distinct().JoinToString("],[") + "]";

                sqlCmd = $@"

SELECT  DISTINCT InvNo
INTO #Base
from ShareExpense se WITH (NOLOCK) 
INNER JOIN GMTBooking g ON se.InvNo = g.ID
where   ShippingAPID = '{this.CurrentMaintain["ID"]}'
and (Junk = 0 or Junk is null)
AND g.Foundry = 1

SELECT   [Invoice No] = b.InvNo
		,gf.FactoryGroup
		,[TtlRatio]=SUM(Ratio)
INTO #Data
FROM #Base b
INNER JOIN GarmentInvoice_Foundry gf ON b.InvNo = gf.InvoiceNo
GROUP BY  b.InvNo ,gf.FactoryGroup

SELECT *
FROM (
	SELECT   l.[Invoice No],l.FactoryGroup,l.TtlRatio
	FROM #Data l
) t 
PIVOT (
	-- 設定彙總欄位及方式
	SUM(TtlRatio) 
	-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
	FOR FactoryGroup IN ({factoryGroup})
) p;

DROP TABLE #Base,#Data
";

                DBProxy.Current.Select(null, sqlCmd, out dtFinal);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                var factoryGroups = dtDetail.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["FactoryGroup"])).Distinct().ToList();
                int ttlRatio = dtDetail.AsEnumerable().Sum(o => MyUtility.Convert.GetInt(o["TtlRatio"]));
                DataRow nRow = dtFinal.NewRow();
                nRow["Invoice No"] = "Summary %";
                foreach (string factory in factoryGroups)
                {
                    int ttlRatioByFactory = dtDetail.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FactoryGroup"]) == factory).Sum(o => MyUtility.Convert.GetInt(o["TtlRatio"]));

                    decimal summary = Math.Round(MyUtility.Convert.GetDecimal(ttlRatioByFactory * 1.0 / ttlRatio * 1.0) * 100, 0);

                    nRow[factory] = summary;
                }

                dtFinal.Rows.Add(nRow);

                this.grid1.AutoGenerateColumns = true;
                this.listControlBindingSource.DataSource = dtFinal;
                this.grid1.IsEditingReadOnly = true;
                this.grid1.ReadOnly = true;
                this.grid1.Columns[0].Width = 180;

                for (int i = 0; i <= this.grid1.Columns.Count - 1; i++)
                {
                    this.grid1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                int lastRow = dtFinal.Rows.Count;
                this.grid1.Rows[lastRow - 1].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
