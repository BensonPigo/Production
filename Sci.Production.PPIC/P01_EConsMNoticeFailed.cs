using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_EConsMNoticeFailed
    /// </summary>
    public partial class P01_EConsMNoticeFailed : Sci.Win.Subs.Input4
    {
        private bool _mustEmpty;

        /// <inheritdoc/>
        public P01_EConsMNoticeFailed(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, bool mustEmpty = false)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.GridPopUp = false;
            this._mustEmpty = mustEmpty;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("KPIFailed", header: "KPI Failed", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("FailedComment", header: "MR Comments", width: Widths.AnsiChars(15), iseditingreadonly: false)
                .Date("ExpectApvDate", header: "Expect APV date", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Date("KPIDate", header: "KPI DATE (target date)", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ID", header: "Order SP", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("POID", header: "PO SP", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("BrandID", header: "Brand ", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("FactoryID", header: "Fty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(8), decimal_places: 0, iseditingreadonly: true)
                .Text("SMR", header: "SMR", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MR", header: "MR", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("SewInLIne", header: "In-Line Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MnorderApv2", header: "VAS/SHAS", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("GMTLT", header: "Garment L/T", width: Widths.AnsiChars(5), iseditingreadonly: true)
                ;

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery(out DataTable datas)
        {
            IList<SqlParameter> para = new List<SqlParameter>();

            string where = string.Empty;
            if (this.rdbEachCons.Checked)
            {
                where += " And o.ID = @ID";
                where += " And f.Type = 'EC'";
                para.Add(new SqlParameter("@ID", this.KeyValue1));
            }
            else
            {
                where += " And o.POID = @POID";
                where += " And f.Type = 'MN'";
                para.Add(new SqlParameter("@POID", this.KeyValue2));
            }

            var sqlCmd = $@"
select
    KPIFailed = iif(f.KPIFailed = 'N', 'Alread Failed', 'Failed Next Week') 
    , f.FailedComment
    , f.ExpectApvDate
    , f.KPIDate
    , o.ID
    , o.POID
    , o.BrandID
    , o.StyleID
    , o.FactoryID
    , o.Qty
    , SMR = GetSMR.IdAndName
    , MR = GetMR.IdAndName
    , o.SciDelivery
    , o.BuyerDelivery
    , o.SewInLIne
    , o.MnorderApv2
    , GetGMTLT.GMTLT
    , f.EditName
    , f.EditDate
    , f.Type
From Order_ECMNFailed f
Left Join Orders o on f.id	= o.ID
Outer Apply(Select GMTLT = dbo.GetStyleGMTLT(o.BrandID,o.StyleID,o.SeasonID,o.FactoryID)) as GetGMTLT
Left join GetName as GetSMR on GetSMR.ID = o.SMR
Left join GetName as GetMR on GetMR.ID = o.MRHandle
Where 1 = 1 {(this._mustEmpty ? "AND 1=0" : string.Empty)}
{where}
";
            var result = DBProxy.Current.Select(null, sqlCmd, para, out datas);
            if (!result)
            {
                this.ShowErr(result);
                datas = null;
            }

            return result;
        }

        private void Rdb_CheckedChanged(object sender, EventArgs e)
        {
            this.OnRequery();
        }
    }
}