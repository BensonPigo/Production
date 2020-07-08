using System;
using System.Data;
using System.Linq;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P01_BatchApprove : Sci.Win.Subs.Base
    {
        Action delegateAct;

        public P01_BatchApprove(Action reload)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.delegateAct = reload;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridArtworkPO.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridArtworkPO.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridArtworkPO)
                 .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                 .Text("ID", header: "PO#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("FactoryId", header: "Factory", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Date("IssueDate", header: "Issue Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("LocalSuppID", header: "Supplier", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Date("Delivery", header: "Delivery Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("CurrencyId", header: "Currency", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true)
                 .Numeric("VatRate", header: "Vat Rate (%)", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true)
                 .Numeric("TotalAmount", header: "Total Amount", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true);

            this.gridArtworkPODetail.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridArtworkPODetail.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridArtworkPODetail)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("StyleID", header: "Style", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("CostStitch", header: "Cost" + Environment.NewLine + "(PCS/Stitch)", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("Stitch", header: "PCS/Stitch", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("PatternCode", header: "Curpart ID", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Numeric("UnitPrice", header: "Unit Price", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true)
                 .Numeric("QtyGarment", header: "Qty/GMT", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true);

            this.Query();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Query()
        {
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource2.DataSource = null;
            DataSet ds = null;
            DataRelation relation;
            string sqlCmd = string.Empty;

            sqlCmd = string.Format(@"
                select 0 Selected
                    ,ID
	                ,FactoryId
	                ,IssueDate
	                ,ArtworkTypeID
	                ,LocalSuppID
	                ,Delivery
	                ,CurrencyId
	                ,Amount
	                ,VatRate
	                ,Amount+Vat as TotalAmount
                from ArtworkPO
                where [Status] = 'Locked'
                      and POTYPE='O'

                select a.ID
	                ,ad.OrderID  
	                ,o.StyleID 
	                ,ad.PoQty
	                ,ad.ArtworkId
	                ,ad.CostStitch
	                ,ad.Stitch
	                ,ad.PatternCode
	                ,ad.UnitPrice
	                ,ad.QtyGarment
	                ,ad.Amount
                from ArtworkPO a
                inner join ArtworkPO_Detail ad on a.ID = ad.ID 
                left join Orders o on ad.OrderID = o.ID
                where a.[Status] = 'Locked'
                      and a.POTYPE = 'O'");

            if (!SQL.Selects(string.Empty, sqlCmd, out ds))
            {
                MyUtility.Msg.WarningBox(sqlCmd, "DB error!!");
                return;
            }

            relation = new DataRelation(
                "rel1",
                new DataColumn[] { ds.Tables[0].Columns["ID"] },
                new DataColumn[] { ds.Tables[1].Columns["ID"] });

            ds.Relations.Add(relation);
            ds.Tables[0].TableName = "Master";
            ds.Tables[1].TableName = "Detail";

            this.listControlBindingSource1.DataSource = ds;
            this.listControlBindingSource1.DataMember = "Master";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource2.DataMember = "rel1";
            this.gridArtworkPO.AutoResizeColumns();
            this.gridArtworkPODetail.AutoResizeColumns();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)this.listControlBindingSource1.DataSource;
            DataTable dt = ds.Tables["Master"];
            DualResult result;
            string sqlcmd = string.Empty;

            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0)
            {
                return;
            }

            var query = dt.AsEnumerable().Where(x => x["Selected"].EqualString("1")).Select(x => x.Field<string>("ID"));
            if (query.Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first.", "Warning");
                return;
            }

            sqlcmd = string.Format(
                @"update ArtworkPO 
                    set [Status]='Approved', apvname='{0}', apvdate=GETDATE(), editname='{0}', editdate=GETDATE()  
                    where [Status] = 'Locked' and ID in ('{1}') and POTYPE='O'", Env.User.UserID, string.Join("','", query.ToList()));

            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            this.Query();
            MyUtility.Msg.WarningBox("Sucessful");
            this.delegateAct();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable printData;
            string sqlCmd = string.Format(@"
select ap.MDivisionID as [M]
	,ap.FactoryId as [Factory]
	,ap.ID as [P/O #]
	,ap.IssueDate as [Date]
	,ap.Delivery 
	,ap.LocalSuppID as [Supplier]
	,ap.ArtworkTypeID as [ArtworkType]
	,apo.OrderID as [SP#]
	,o.SewInLine as [Sewing Inline]
	,o.SciDelivery as [SCI Delivery]
	,o.StyleID as [Style#]
	,apo.ArtworkId as [Pattern]
	,apo.PatternDesc as [Cutparts]
	,apo.PoQty as [Q'ty]
	,ap.CurrencyID as [Currency]
	,apo.UnitPrice as [Unit Prc]
	,isnull(tmscost.stdPrice,0.0) [Std. Price]
	,apo.POQty * apo.UnitPrice as [PO Amt]
	,ap.VatRate * 1.0 as [Vat Rate (%)]
	,(apo.POQty * apo.UnitPrice * 1.0) * ap.VatRate / 100 as [Vat Amt]
	,(apo.POQty * apo.UnitPrice) + ((apo.POQty * apo.UnitPrice * 1.0) * ap.VatRate / 100) as [Total]
	,ap.Remark 
	,ap.InternalRemark as [Internal Remark]
from ArtworkPO ap
inner join ArtworkPO_Detail apo on ap.id = apo.id
left join orders o on apo.OrderID = o.ID
outer apply (
	select stdPrice = isnull(sum (oA.Qty * ot.Price) / sum (oa.Qty),0.000)
	from orders o
	inner join orders oA on o.POID = oa.POID
	inner join Order_TmsCost ot on oA.id = ot.ID
									and ot.ArtworkTypeID = ap.ArtworkTypeID
	where o.id = apo.OrderID
) tmscost
where ap.status = 'Locked' 
and ap.POTYPE='O'
order by ap.ID, apo.OrderID ");
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            MyUtility.Excel.CopyToXls(printData, string.Empty, "Subcon_P01_BatchApprove.xltx");
        }
    }
}
