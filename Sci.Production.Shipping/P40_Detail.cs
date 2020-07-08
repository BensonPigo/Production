using System.Data;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P40_Detail
    /// </summary>
    public partial class P40_Detail : Sci.Win.Subs.Base
    {
        private DataRow masterData;
        private string nlCode;

        /// <summary>
        /// P40_Detail
        /// </summary>
        /// <param name="masterData">masterData</param>
        /// <param name="nLCode">nLCode</param>
        public P40_Detail(DataRow masterData, string nLCode)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.nlCode = nLCode;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridDetail.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("Refno", header: "Ref No.", width: Widths.AnsiChars(21), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 2, iseditingreadonly: true)
                .Text("WKNo", header: "WK No.", width: Widths.AnsiChars(30), iseditingreadonly: true);

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            sqlCmd.Append(@"with tmpExport
as (
");
            if (MyUtility.Convert.GetString(this.masterData["IsFtyExport"]).ToUpper() == "FALSE")
            {
                    sqlCmd.Append(string.Format(
                        @"select f.Refno,f.BrandID,f.Type,ed.UnitId,sum(ed.Qty+ed.Foc) as Qty,e.ID 
from Export e WITH (NOLOCK) 
inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
inner join PO_Supp_Detail psd WITH (NOLOCK) on ed.PoID = psd.ID and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2
inner join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
where {0}
and f.NLCode = '{1}'
group by f.Refno,f.BrandID,f.Type,ed.UnitId,e.ID",
                        MyUtility.Check.Empty(this.masterData["WKNo"]) ? "e.Blno = '" + MyUtility.Convert.GetString(this.masterData["BLNo"]) + "'" : "e.ID = '" + MyUtility.Convert.GetString(this.masterData["WKNo"]) + "'",
                        this.nlCode));
            }
            else
            {
                if (MyUtility.Convert.GetString(this.masterData["IsLocalPO"]).ToUpper() == "TRUE")
                {
                    sqlCmd.Append(string.Format(
                        @"select f.Refno,'' as BrandID,f.Category as Type,ed.UnitId,sum(ed.Qty) as Qty,e.ID 
from FtyExport e WITH (NOLOCK) 
inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
inner join LocalItem f WITH (NOLOCK) on ed.Refno = f.Refno
where {0}
and f.NLCode = '{1}'
group by f.Refno,f.Category,ed.UnitId,e.ID",
                        MyUtility.Check.Empty(this.masterData["WKNo"]) ? "e.Blno = '" + MyUtility.Convert.GetString(this.masterData["BLNo"]) + "'" : "e.ID = '" + MyUtility.Convert.GetString(this.masterData["WKNo"]) + "'",
                        this.nlCode));
                }
                else
                {
                    sqlCmd.Append(string.Format(
                        @"select f.Refno,f.BrandID,f.Type,ed.UnitId,sum(ed.Qty) as Qty,e.ID 
from FtyExport e WITH (NOLOCK) 
inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
inner join PO_Supp_Detail psd WITH (NOLOCK) on ed.PoID = psd.ID and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2
inner join Fabric f WITH (NOLOCK) on psd.SCIRefno = f.SCIRefno
where {0}
and f.NLCode = '{1}'
group by f.Refno,f.BrandID,f.Type,ed.UnitId,e.ID",
                        MyUtility.Check.Empty(this.masterData["WKNo"]) ? "e.Blno = '" + MyUtility.Convert.GetString(this.masterData["BLNo"]) + "'" : "e.ID = '" + MyUtility.Convert.GetString(this.masterData["WKNo"]) + "'",
                        this.nlCode));
                }
            }

            sqlCmd.Append(@"
),
tmpWKNo
as (
select RefNo,
(select CONCAT(ID,',') from tmpExport te where te.Refno = t.Refno order by te.ID for XML path('')) as WKNo
from tmpExport t
),
tmpDataSummary
as (
select Refno,BrandID,Type,UnitId,sum(Qty) as Qty
from tmpExport
group by Refno,BrandID,Type,UnitId
)
select distinct ts.*, tw.WKNo
from tmpDataSummary ts
inner join tmpWKNo tw on ts.Refno = tw.Refno");
            #endregion

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
