using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P01_Import : Sci.Win.Subs.Base
    {
        DataRow dr_artworkpo;
        DataTable dt_artworkpoDetail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag;
        string poType;
        protected DataTable dtArtwork;

        public P01_Import(DataRow master, DataTable detail, string fuc)
        {
            InitializeComponent();
            dr_artworkpo = master;
            dt_artworkpoDetail = detail;
            flag = fuc == "P01";
            if (flag)
            {
                poType = "O";
                this.Text += " (Sub-con Purchase Order)";
            }
            else
            {
                poType = "I";
                this.Text += " (In-House Requisition)";
            }

            this.Text += string.Format(" : {0}", dr_artworkpo["artworktypeid"].ToString());
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            string apvdate_b, apvdate_e, sciDelivery_b, sciDelivery_e, Inline_b, Inline_e;
            apvdate_b = null;
            apvdate_e = null;
            sciDelivery_b = null;
            sciDelivery_e = null;
            Inline_b = null;
            Inline_e = null;

            if (dateApproveDate.Value1 != null) apvdate_b = this.dateApproveDate.Text1;
            if (dateApproveDate.Value2 != null) { apvdate_e = this.dateApproveDate.Text2; }
            if (dateSCIDelivery.Value1 != null) sciDelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }
            if (dateInlineDate.Value1 != null) Inline_b = this.dateInlineDate.Text1;
            if (dateInlineDate.Value2 != null) { Inline_e = this.dateInlineDate.Text2; }


            String sp_b = this.txtSPNoStart.Text;
            String sp_e = this.txtSPNoEnd.Text;

            if ((apvdate_b == null && apvdate_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                (Inline_b == null && Inline_e == null) &&
                string.IsNullOrWhiteSpace(sp_b) && string.IsNullOrWhiteSpace(sp_e))
            {
                MyUtility.Msg.WarningBox("< Approve Date > or < SCI Delivery > or < Inline Date > or < SP# > can't be empty!!");
                dateApproveDate.Focus1();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                string strSQLCmd = string.Format(@"
select  Selected = 0
        , ot.LocalSuppID
        , id = ''
        , orderid = q.id
        , OrderQty = sum(q.qty)  
        , IssueQty.IssueQty 
        , poqty = iif (sum(q.qty)-IssueQty.IssueQty < 0, 0, sum(q.qty)-IssueQty.IssueQty)
        , oa.ArtworkTypeID
        , oa.ArtworkID
        , oa.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = oa.qty
        , Stitch = oa.qty 
        , oa.PatternDesc
        , qtygarment = 1
        , Cost = iif(at.isArtwork = 1,oa.Cost,ot.Price)
        , unitprice = iif(at.isArtwork = 1,oa.Cost,ot.Price)
        , price = oa.Cost
        , amount = (sum(q.qty)-IssueQty.IssueQty)*iif(at.isArtwork = 1,oa.Cost,ot.Price)
        , Style = o.StyleID
from orders o WITH (NOLOCK) 
inner join order_qty q WITH (NOLOCK) on q.id = o.ID
inner join dbo.View_Order_Artworks oa on oa.ID = o.ID AND OA.Article = Q.Article AND OA.SizeCode=Q.SizeCode
inner join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = oa.ID and ot.ArtworkTypeID = oa.ArtworkTypeID
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= oa.PatternCode
) IssueQty
where   1=1 
        and o.category  in ('B','S')
");

                strSQLCmd += string.Format(" and o.MDivisionID='{0}' and oa.ArtworkTypeID = '{1}' and o.Junk=0 ", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"]);
                if (poType == "O") { strSQLCmd += "     and ((o.Category = 'B' and ot.InhouseOSP='O' and ot.price > 0) or (o.category !='B'))"; }
                if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
                if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
                if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ot.ApvDate >= '{0}' ", apvdate_b); }
                if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ot.ApvDate <= '{0}' ", apvdate_e); }
                if (!(dateInlineDate.Value1 == null)) { strSQLCmd += string.Format(" and ot.ArtworkInLine <= '{0}' ", Inline_b); }
                if (!(dateInlineDate.Value2 == null)) { strSQLCmd += string.Format(" and ot.ArtworkOffLine >= '{0}' ", Inline_e); }
                if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }

                strSQLCmd += " group by q.id,ot.LocalSuppID,oa.ArtworkTypeID,oa.ArtworkID,oa.PatternCode,o.SewInLIne,o.SciDelivery,oa.qty,oa.Cost,oa.PatternDesc,IssueQty.IssueQty, o.StyleID,at.isArtwork,ot.Price";

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtArtwork;
                }
                else { ShowErr(strSQLCmd, result); }
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                DataRow ddr = gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                ddr["UnitPrice"] = Convert.ToDecimal(e.FormattedValue);
                ddr["Price"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["qtygarment"]);
                ddr["Amount"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["poqty"]) * Convert.ToInt32(ddr["qtygarment"]);
                ddr.EndEdit(); 
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                DataRow ddr = gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                ddr["Price"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToDecimal(ddr["UnitPrice"]);
                ddr["Amount"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["poqty"]) * Convert.ToDecimal(ddr["UnitPrice"]);
                ddr.EndEdit(); 
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns3 = new DataGridViewGeneratorNumericColumnSettings();
            ns3.CellValidating += (s, e) =>
            {
                //DataTable temp = (DataTable)listControlBindingSource1.DataSource;
                //DataRow ddr = temp.Rows[e.RowIndex];
                DataRow ddr = gridBatchImport.GetDataRow<DataRow>(listControlBindingSource1.Position);
                Decimal SourcePoqty = Convert.ToDecimal(ddr["OrderQty"]) - Convert.ToDecimal(ddr["IssueQty"]);
                if ((decimal)e.FormattedValue > SourcePoqty)
                {
                    MyUtility.Msg.WarningBox("Po Qty can't be more than [Order Qty]-[Issue Qty]");
                    //e.Cancel = true;
                    //return;
                }                
                ddr["Amount"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["qtygarment"]) * Convert.ToDecimal(ddr["UnitPrice"]);
                ddr["poqty"] = Convert.ToDecimal(e.FormattedValue);
                ddr.EndEdit();                
            };
            this.gridBatchImport.Font = new Font("Arial", 9);
            this.gridBatchImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridBatchImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridBatchImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("LocalSuppID", header: "Supplier", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true)
                .Numeric("IssueQty", header: "Issue Qty", iseditingreadonly: true)
                .Numeric("poqty", header: "Po Qty", iseditable: true, settings: ns3)
                .Date("sewinline", header: "Sewinline", iseditingreadonly: true)
                .Date("SciDelivery", header: "SciDelivery", iseditingreadonly: true)
                .Text("artworkid", header: "Artwork", iseditingreadonly: true)      //5
                .Numeric("coststitch", header: "Cost" + Environment.NewLine + "(Pcs/Stitch)", iseditingreadonly: true)
                .Numeric("Stitch", header: "Pcs/" + Environment.NewLine + "Stitch", iseditable: true, iseditingreadonly: true)    //7
                .Text("PatternCode", header: "Cutpart Id", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", iseditingreadonly: true)
                .Numeric("qtygarment", header: "Qty/GMT", iseditable: true, integer_places: 2, settings: ns2, iseditingreadonly: true) //10
                .Numeric("Cost", header: "Cost(USD)", settings: ns, iseditingreadonly: true, decimal_places: 4, integer_places: 4)  //11
                .Numeric("UnitPrice", header: "Unit Price", settings: ns, iseditable: true, decimal_places: 4, integer_places: 4)  //12
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true, decimal_places: 4, integer_places: 5)  //13
                .Numeric("Amount", header: "Amount",width: Widths.AnsiChars(12),iseditingreadonly: true, decimal_places: 4, integer_places: 14);  //14


            //this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            //this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;  //Qty/GMT
            this.gridBatchImport.Columns["UnitPrice"].DefaultCellStyle.BackColor = Color.Pink;  //UnitPrice
            this.gridBatchImport.Columns["poqty"].DefaultCellStyle.BackColor = Color.Pink;  //poqty
            this.gridBatchImport.Columns["Cost"].Visible = flag;
            this.gridBatchImport.Columns["UnitPrice"].Visible = flag;
            this.gridBatchImport.Columns["Price"].Visible = flag;
            this.gridBatchImport.Columns["Amount"].Visible = flag;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1)|| dtGridBS1.Rows.Count == 0) return;
            DataRow[] dr2 = dtGridBS1.Select("UnitPrice = 0 and Selected = 1");
            
            if (dr2.Length > 0 && flag)
            {
                MyUtility.Msg.WarningBox("UnitPrice of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length > 0)
            {
                bool yns = false;
                StringBuilder ids = new StringBuilder();
                foreach (DataRow tmp in dr2)
                {
                    if (tmp["LocalSuppID"].ToString().ToUpper() != dr_artworkpo["localsuppid"].ToString().ToUpper())
                    {
                        yns = true;
                        ids.Append(string.Format("{0},",tmp["orderid"].ToString()));
                    }
                }
                if (yns)
                {
                    DialogResult dResult = MyUtility.Msg.QuestionBox(string.Format("{0} sub-process subcon supplier is different with {1}. Do you want to continue?", ids.ToString(), dr_artworkpo["localsuppid"].ToString().ToUpper()));
                    if (dResult == System.Windows.Forms.DialogResult.No) return;
                }
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = dt_artworkpoDetail.Select(string.Format("orderid = '{0}' and ArtworkId = '{1}' and patterncode = '{2}'", tmp["orderid"].ToString(), tmp["ArtworkId"].ToString(), tmp["patterncode"].ToString()));

                    if (findrow.Length > 0)
                    {
                        findrow[0]["unitprice"] = tmp["unitprice"];
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["amount"] = tmp["amount"];
                        findrow[0]["poqty"] = tmp["poqty"];
                        findrow[0]["qtygarment"] = 1;
                    }
                    else
                    {
                        tmp["id"] = dr_artworkpo["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        dt_artworkpoDetail.ImportRow(tmp);
                    }
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }
            this.Close();
        }
    }
}
