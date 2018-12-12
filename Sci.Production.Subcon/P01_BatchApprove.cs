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
        public P01_BatchApprove()
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            Query();

            this.gridArtworkPO.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridArtworkPO.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridArtworkPO)
                 .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk) 
                 .Text("ID", header: "A/P#", width: Widths.AnsiChars(15), iseditingreadonly :true)
                 .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Date("IssueDate", header: "Issue Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("LocalSuppID", header: "Supplier", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Date("Delivery", header: "Delivery Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Numeric("VatRate", header: "Vat Rate (%)", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Numeric("TotalAmount", header: "Total Amount", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true);

            this.gridArtworkPODetail.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridArtworkPODetail.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.gridArtworkPODetail)                 
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("StyleID", header: "Style", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Text("ArtworkID", header: "Artwork", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("CostStich", header: "Cost"+ Environment.NewLine +"(PCS/Stitch)", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("Stich", header: "PCS/Stitch", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Text("Curpart ID", header: "PatternCode", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Numeric("UnitPrice", header: "Unit Price", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Numeric("QtyGarment", header: "Qty/GMT", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true)
                 .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0, iseditingreadonly: true);

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
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {

        }
    }
}
