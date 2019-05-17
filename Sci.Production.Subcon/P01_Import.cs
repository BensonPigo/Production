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
        bool isNeedPlanningP03Quote = false;
        string apvdate_b, apvdate_e, sciDelivery_b, sciDelivery_e, Inline_b, Inline_e, sp_b, sp_e;

        public P01_Import(DataRow master, DataTable detail, string fuc, bool isNeedPlanningP03Quote = false)
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

            this.isNeedPlanningP03Quote = isNeedPlanningP03Quote;
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {

            this.apvdate_b = null;
            this.apvdate_e = null;
            this.sciDelivery_b = null;
            this.sciDelivery_e = null;
            this.Inline_b = null;
            this.Inline_e = null;

            if (dateApproveDate.Value1 != null) apvdate_b = this.dateApproveDate.Text1;
            if (dateApproveDate.Value2 != null) { apvdate_e = this.dateApproveDate.Text2; }
            if (dateSCIDelivery.Value1 != null) sciDelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }
            if (dateInlineDate.Value1 != null) Inline_b = this.dateInlineDate.Text1;
            if (dateInlineDate.Value2 != null) { Inline_e = this.dateInlineDate.Text2; }


            this.sp_b = this.txtSPNoStart.Text;
            this.sp_e = this.txtSPNoEnd.Text;

            if ((apvdate_b == null && apvdate_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                (Inline_b == null && Inline_e == null) &&
                string.IsNullOrWhiteSpace(sp_b) && string.IsNullOrWhiteSpace(sp_e))
            {
                MyUtility.Msg.WarningBox("< Approve Date > or < SCI Delivery > or < Inline Date > or < SP# > can't be empty!!");
                dateApproveDate.Focus1();
                return;
            }

            string strSQLCmd = string.Empty;
            if (isNeedPlanningP03Quote)
            {
                strSQLCmd = this.QuoteFromPlanningP03();
            }
            else
            {
                strSQLCmd = this.QuoteFromTmsCost();
            }

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
            {
                if (dtArtwork.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtArtwork;
            }
            else { ShowErr(strSQLCmd, result); }

            foreach (DataGridViewRow dr in this.gridBatchImport.Rows)
            {
                this.DetalGridCellEditChange(dr.Index);
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
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 4, integer_places: 14);  //14


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
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;
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
                        ids.Append(string.Format("{0},", tmp["orderid"].ToString()));
                    }
                }
                if (yns)
                {
                    DialogResult dResult = MyUtility.Msg.QuestionBox(string.Format("{0} sub-process subcon supplier is different with {1}. Do you want to continue?", ids.ToString(), dr_artworkpo["localsuppid"].ToString().ToUpper()));
                    if (dResult == System.Windows.Forms.DialogResult.No) return;
                }
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = dt_artworkpoDetail.Select($@"orderid = '{tmp["orderid"].ToString()}' and ArtworkId = '{tmp["ArtworkId"].ToString()}' and patterncode = '{tmp["patterncode"].ToString()}' and cost='{tmp["Cost"]}'");

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

        private string QuoteFromPlanningP03()
        {
            string strSQLCmd = string.Empty;
            strSQLCmd = string.Format(@"
select  Selected = 0
        , sao.LocalSuppId
        , id = ''
        , orderid = q.id
        , OrderQty = sum(q.qty)  
        , IssueQty.IssueQty 
        , poqty = iif(sum(q.qty)-IssueQty.IssueQty < 0, 0, sum(q.qty)-IssueQty.IssueQty)
        , oa.ArtworkTypeID
        , oa.ArtworkID
        , oa.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = oa.qty
        , Stitch = oa.qty 
        , oa.PatternDesc
        , qtygarment = 1
        , Cost = iif(at.isArtwork = 1,vsa.Cost,sao.Price)
        , unitprice = sao.Price
        , price = sao.Price
        , amount = iif((sum(q.qty)-IssueQty.IssueQty) < 0 ,0 ,(sum(q.qty)-IssueQty.IssueQty) *  sao.Price )
        , Style = o.StyleID
from  orders o WITH (NOLOCK) 
inner join order_qty q WITH (NOLOCK) on q.id = o.ID
inner join dbo.View_Order_Artworks oa on oa.ID = o.ID AND OA.Article = Q.Article AND OA.SizeCode=Q.SizeCode
inner join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = oa.ID and ot.ArtworkTypeID = oa.ArtworkTypeID
inner join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = o.StyleUkey and vsa.Article = oa.Article and vsa.ArtworkID = oa.ArtworkID and
														vsa.ArtworkName = oa.ArtworkName and vsa.ArtworkTypeID = oa.ArtworkTypeID and vsa.PatternCode = oa.PatternCode and
														vsa.PatternDesc = oa.PatternDesc 
inner join Style_Artwork_Quot sao with (nolock) on sao.Ukey = vsa.StyleArtworkUkey and sao.PriceApv = 'Y' and sao.Price > 0
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= oa.PatternCode
) IssueQty
where f.IsProduceFty=1
and o.PulloutComplete = 0
and o.category  in ('B','S')
");

            strSQLCmd += string.Format(" and o.MDivisionID='{0}' and oa.ArtworkTypeID = '{1}' and sao.LocalSuppId = '{2}' and o.Junk=0 ", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"], dr_artworkpo["localsuppid"]);

            if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ot.ApvDate >= '{0}' ", apvdate_b); }
            if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ot.ApvDate <= '{0}' ", apvdate_e); }
            if (!(dateInlineDate.Value1 == null)) { strSQLCmd += string.Format(" and ot.ArtworkInLine <= '{0}' ", Inline_b); }
            if (!(dateInlineDate.Value2 == null)) { strSQLCmd += string.Format(" and ot.ArtworkOffLine >= '{0}' ", Inline_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }

            strSQLCmd += " group by q.id,sao.LocalSuppID,oa.ArtworkTypeID,oa.ArtworkID,oa.PatternCode,o.SewInLIne,o.SciDelivery,oa.qty,oa.PatternDesc,IssueQty.IssueQty, o.StyleID, o.StyleID,iif(at.isArtwork = 1,vsa.Cost,sao.Price),sao.Price";

            return strSQLCmd;
        }

        private string QuoteFromTmsCost()
        {
            string strIsArtwork = MyUtility.GetValue.Lookup(string.Format("select isartwork from artworktype WITH (NOLOCK) where id = '{0}'", dr_artworkpo["artworktypeid"].ToString()), null);
            string strSQLCmd = string.Empty;
            if (strIsArtwork.EqualString("true"))
            {
                strSQLCmd = string.Format(@"
select  Selected = 0
        , ot.LocalSuppID
        , id = ''
        , orderid = q.id
        , OrderQty = sum(q.qty)  
        , IssueQty.IssueQty 
        , poqty = iif(sum(q.qty)-IssueQty.IssueQty < 0, 0, sum(q.qty)-IssueQty.IssueQty)
        , oa.ArtworkTypeID
        , oa.ArtworkID
        , oa.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = oa.qty
        , Stitch = oa.qty 
        , oa.PatternDesc
        , qtygarment = 1
        , Cost = oa.Cost
        , unitprice = oa.Cost
        , price = oa.Cost
        , amount = iif(sum(q.qty)-IssueQty.IssueQty < 0 ,0 ,(sum(q.qty)-IssueQty.IssueQty)*oa.Cost) 
        , Style = o.StyleID
from orders o WITH (NOLOCK) 
inner join order_qty q WITH (NOLOCK) on q.id = o.ID
inner join dbo.View_Order_Artworks oa on oa.ID = o.ID AND OA.Article = Q.Article AND OA.SizeCode=Q.SizeCode
inner join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = oa.ID and ot.ArtworkTypeID = oa.ArtworkTypeID
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= oa.PatternCode
) IssueQty
where   1=1 
and f.IsProduceFty=1
and o.PulloutComplete = 0
and o.category  in ('B','S')
");

                strSQLCmd += string.Format(" and o.MDivisionID='{0}' and oa.ArtworkTypeID = '{1}' and o.Junk=0 ", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"]);
                if (poType == "O")
                {
                    strSQLCmd += @"     and ((o.Category = 'B' and  (ot.InhouseOSP='O' and ot.price > 0)  and 
                                                                    ((at.isArtwork = 1) or 
                                                                    (at.isArtwork = 0 and ot.Price > 0))) 
                                        or (o.category !='B'))";
                }
                if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
                if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
                if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ot.ApvDate >= '{0}' ", apvdate_b); }
                if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ot.ApvDate <= '{0}' ", apvdate_e); }
                if (!(dateInlineDate.Value1 == null)) { strSQLCmd += string.Format(" and ot.ArtworkInLine <= '{0}' ", Inline_b); }
                if (!(dateInlineDate.Value2 == null)) { strSQLCmd += string.Format(" and ot.ArtworkOffLine >= '{0}' ", Inline_e); }
                if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }

                strSQLCmd += " group by q.id,ot.LocalSuppID,oa.ArtworkTypeID,oa.ArtworkID,oa.PatternCode,o.SewInLIne,o.SciDelivery,oa.qty,oa.Cost,oa.PatternDesc,IssueQty.IssueQty, o.StyleID,at.isArtwork";
            }
            else
            {
                strSQLCmd = @"
select  Selected = 0
        , ot.LocalSuppID
        , id = ''
        , orderid = o.id
        , OrderQty = o.qty 
        , IssueQty.IssueQty 
        , poqty = iif (o.qty-IssueQty.IssueQty < 0, 0, o.qty-IssueQty.IssueQty)
        , ot.ArtworkTypeID
        , ArtworkID = ot.ArtworkTypeID
        , PatternCode = ot.ArtworkTypeID
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = 1
        , Stitch = 1 
        , PatternDesc = ot.ArtworkTypeID
        , qtygarment = isnull (ot.Qty, 1)
        , Cost = ot.Price
        , unitprice = ot.Price
        , price = ot.Price * isnull (ot.Qty, 1)
        , amount = iif(o.qty-IssueQty.IssueQty < 0, 0, (o.qty-IssueQty.IssueQty) * ot.Price * isnull (ot.Qty, 1)) 
        , Style = o.StyleID
from orders o WITH (NOLOCK) 
inner join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = o.ID
left join ArtworkType at WITH (NOLOCK) on at.id = ot.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= ot.ArtworkTypeID
) IssueQty
where   1=1 
and f.IsProduceFty=1
and o.category  in ('B','S')
and o.PulloutComplete = 0
";

                strSQLCmd += string.Format(" and o.MDivisionID='{0}' and ot.ArtworkTypeID = '{1}' and o.Junk=0 ", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"]);
                if (poType == "O")
                {
                    strSQLCmd += @"     and ((o.Category = 'B' and  (ot.InhouseOSP='O' and ot.price > 0)  and 
                                                                    ((at.isArtwork = 1) or 
                                                                    (at.isArtwork = 0 and ot.Price > 0))) 
                                        or (o.category !='B'))";
                }
                if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
                if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
                if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ot.ApvDate >= '{0}' ", apvdate_b); }
                if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ot.ApvDate <= '{0}' ", apvdate_e); }
                if (!(dateInlineDate.Value1 == null)) { strSQLCmd += string.Format(" and ot.ArtworkInLine <= '{0}' ", Inline_b); }
                if (!(dateInlineDate.Value2 == null)) { strSQLCmd += string.Format(" and ot.ArtworkOffLine >= '{0}' ", Inline_e); }
                if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }

            }
            return strSQLCmd;
        }

        private void DetalGridCellEditChange(int index)
        {

            #region 檢查Qty欄位是否可編輯
            string spNo = this.gridBatchImport.GetDataRow(index)["orderid"].ToString();

            string sqlCheckSampleOrder = $@"
select 1
from orders with (nolock)
where id = '{spNo}' and Category = 'S'
";
            bool isSampleOrder = MyUtility.Check.Seek(sqlCheckSampleOrder);

            if (!isSampleOrder && isNeedPlanningP03Quote)
            {
                this.gridBatchImport.Rows[index].Cells["unitprice"].ReadOnly = true;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Black;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.BackColor = Color.White; //Unit Price
            }
            else
            {
                this.gridBatchImport.Rows[index].Cells["unitprice"].ReadOnly = false;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.ForeColor = Color.Red;
                this.gridBatchImport.Rows[index].Cells["unitprice"].Style.BackColor = Color.Pink; //Unit Price
            }

            #endregion
        }
    }
}
