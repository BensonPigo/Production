using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
namespace Sci.Production.Subcon
{
    public partial class P01_BatchApprove : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        Action aa;
        public P01_BatchApprove(Action aa)
        {
            InitializeComponent();
            this.EditMode = true;
            this.aa = aa;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();            

            this.gridArtworkPO.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridArtworkPO.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridArtworkPO)
                 .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk) 
                 .Text("ID", header: "PO#", width: Widths.AnsiChars(15), iseditingreadonly :true)
                 .Text("FactoryId", header: "Factory", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Date("IssueDate", header: "Issue Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("LocalSuppID", header: "Supplier", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Date("Delivery", header: "Delivery Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("CurrencyId", header: "Currency", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true)
                 .Numeric("VatRate", header: "Vat Rate (%)", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true)
                 .Numeric("TotalAmount", header: "Total Amount", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true);

            this.gridArtworkPODetail.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridArtworkPODetail.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.gridArtworkPODetail)                 
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("StyleID", header: "Style", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("CostStitch", header: "Cost"+ Environment.NewLine +"(PCS/Stitch)", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("Stitch", header: "PCS/Stitch", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("PatternCode", header: "Curpart ID", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Numeric("UnitPrice", header: "Unit Price", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true)
                 .Numeric("QtyGarment", header: "Qty/GMT", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 4, iseditingreadonly: true);

            Query();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void Query()
        {            
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
                where a.[Status] = 'Locked'");

            if (!SQL.Selects("", sqlCmd, out ds)){
                MyUtility.Msg.WarningBox(sqlCmd, "DB error!!");
                return;
            }

            relation = new DataRelation("rel1"
                , new DataColumn[] { ds.Tables[0].Columns["ID"] }
                , new DataColumn[] { ds.Tables[1].Columns["ID"] }
           );

            ds.Relations.Add(relation);
            ds.Tables[0].TableName = "Master";
            ds.Tables[1].TableName = "Detail";

            listControlBindingSource1.DataSource = ds;
            listControlBindingSource1.DataMember = "Master";
            listControlBindingSource2.DataSource = listControlBindingSource1;
            listControlBindingSource2.DataMember = "rel1";
            this.gridArtworkPO.AutoResizeColumns();
            this.gridArtworkPODetail.AutoResizeColumns();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)listControlBindingSource1.DataSource;
            DataTable dt = ds.Tables["Master"];
            DualResult result;
            string sqlcmd = string.Empty;

            if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0) return;

            var query = dt.AsEnumerable().Where(x => x["Selected"].EqualString("1")).Select(x => x.Field<string>("ID"));
            if (query.Count() == 0) {
                MyUtility.Msg.WarningBox("Please select data first.", "Warning");
                return;
            }

            sqlcmd = string.Format(@"update ArtworkPO 
                    set [Status]='Approved', apvname='{0}', apvdate=GETDATE(), editname='{0}', editdate=GETDATE()  
                    where [Status] = 'Locked' and ID in ('{1}')", Env.User.UserID, string.Join("','", query.ToList()));

            if (!(result = DBProxy.Current.Execute(null, sqlcmd))) {
                ShowErr(sqlcmd, result);
                return;
            }

            Query();
            MyUtility.Msg.WarningBox("Sucessful");
            this.aa();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable printData;
            string sqlCmd = string.Format(@"
                select p.MDivisionID
	                ,p.FactoryId
	                ,p.ID
	                ,p.IssueDate
	                ,p.Delivery 
	                ,p.LocalSuppID
	                ,p.ArtworkTypeID
	                ,pd.OrderID
	                ,pd.ArtworkId
	                ,pd.PatternDesc
	                ,pd.PoQty
	                ,p.CurrencyID
	                ,pd.UnitPrice
	                ,pd.POQty * pd.UnitPrice as [PO Amt]
	                ,p.VatRate * 1.0 as [Vat Rate (%)]
	                ,(pd.POQty * pd.UnitPrice * 1.0) * p.VatRate / 100 as [Vat Amt]
	                ,(pd.POQty * pd.UnitPrice) + ((pd.POQty * pd.UnitPrice * 1.0) * p.VatRate / 100) as [Total]
	                ,p.Remark 
	                ,p.InternalRemark
                into #tmp_po
                from ArtworkPO p
                inner join ArtworkPO_Detail pd on p.ID= pd.ID  
                where p.status = 'Locked' 

                select a.OrderID, a.ID, o.Qty, ot.Price, o.qty * isnull(ot.Price,0) as detialSum, o.SewInLine,o.SciDelivery,o.StyleID, a.ArtworkTypeID
                into #tmp_dprice
                from #tmp_po a 
                left join Orders o on a.OrderID = o.ID
                left join Order_TMSCost ot on a.OrderID = ot.ID and a.ArtworkTypeID = ot.ArtworkTypeID

                select ID, ArtworkTypeID, isnull(sum(Qty),0) totalQty
                into #tmp_Mprice
                from #tmp_dprice
                where Price is not null
                group by ID, ArtworkTypeID

                select p.MDivisionID as [M]
	                ,p.FactoryId as [Factory]
	                ,p.ID as [P/O #]
	                ,p.IssueDate as [Date]
	                ,p.Delivery 
	                ,p.LocalSuppID as [Supplier]
	                ,p.ArtworkTypeID as [ArtworkType]
	                ,p.OrderID as [SP#]
	                ,d.SewInLine as [Sewing Inline]
	                ,d.SciDelivery as [SCI Delivery]
	                ,d.StyleID as [Style#]
	                ,p.ArtworkId as [Pattern]
	                ,p.PatternDesc as [Cutparts]
	                ,p.PoQty as [Q'ty]
	                ,p.CurrencyID as [Currency]
	                ,p.UnitPrice as [Unit Prc]
	                ,cast(iif(m.totalQty = 0, 0.0, detialSum*1.0/totalQty*1.0) as decimal(10,4)) as [Std. Price]
	                ,p.[PO Amt]
	                ,p.[Vat Rate (%)]
	                ,p.[Vat Amt]
	                ,p.[Total]
	                ,p.Remark 
	                ,p.InternalRemark as [Internal Remark]
                from #tmp_po p
                inner join #tmp_Mprice m on p.ID = m.ID and p.ArtworkTypeID = m.ArtworkTypeID
                inner join #tmp_dprice d on p.OrderID = d.OrderID and p.ArtworkTypeID =d.ArtworkTypeID 
                order by p.ID,p.OrderID

                drop table #tmp_po,#tmp_dprice,#tmp_Mprice
            ");
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_P01_BatchApprove.xltx");
        }
    }
}
