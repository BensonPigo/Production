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
using Sci.Production.PublicPrg;

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
        bool isNeedPlanningB03Quote = false;
        bool IsSintexSubcon = false;
        string apvdate_b, apvdate_e, sciDelivery_b, sciDelivery_e, sp_b, sp_e;
        string titleStitch = "";

        public P01_Import(DataRow master, DataTable detail, string fuc, bool isNeedPlanningB03Quote = false)
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

            this.Text += string.Format(" : {0}", dr_artworkpo["LocalSuppID"].ToString());

            this.isNeedPlanningB03Quote = isNeedPlanningB03Quote;
            this.IsSintexSubcon = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select IsSintexSubcon from LocalSupp with (nolock) where ID = '{master["localsuppid"]}'"));
            this.gridBatchImport.RowPostPaint += GridBatchImport_RowPostPaint;

            this.titleStitch = MyUtility.GetValue.Lookup($"select iif(artworkunit='','PCS',artworkunit) from artworktype WITH (NOLOCK) where id='{master["ArtworkTypeID"]}'");
        }

        private void GridBatchImport_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.DetalGridCellEditChange(e.RowIndex);
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {

            this.apvdate_b = null;
            this.apvdate_e = null;
            this.sciDelivery_b = null;
            this.sciDelivery_e = null;

            if (dateApproveDate.Value1 != null) apvdate_b = this.dateApproveDate.Text1;
            if (dateApproveDate.Value2 != null) { apvdate_e = this.dateApproveDate.Text2; }
            if (dateSCIDelivery.Value1 != null) sciDelivery_b = this.dateSCIDelivery.Text1;
            if (dateSCIDelivery.Value2 != null) { sciDelivery_e = this.dateSCIDelivery.Text2; }


            this.sp_b = this.txtSPNoStart.Text;
            this.sp_e = this.txtSPNoEnd.Text;

            if ((apvdate_b == null && apvdate_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                string.IsNullOrWhiteSpace(sp_b) && string.IsNullOrWhiteSpace(sp_e))
            {
                MyUtility.Msg.WarningBox("< Approve Date > or < SCI Delivery > or < Inline Date > or < SP# > can't be empty!!");
                dateApproveDate.Focus1();
                return;
            }

            string strSQLCmd = string.Empty;
            if (isNeedPlanningB03Quote)
            {
                if (this.IsSintexSubcon && Prgs.CheckIsArtworkorUseArtwork(MyUtility.Convert.GetString(this.dr_artworkpo["artworktypeid"])))
                {
                    strSQLCmd = this.QuoteIsSintexSubcon();
                }
                else
                {
                    strSQLCmd = this.QuoteFromPlanningB03();
                }
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
                ddr["Amount"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["PoQty"]) * Convert.ToInt32(ddr["qtygarment"]);
                ddr.EndEdit();
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                DataRow ddr = gridBatchImport.GetDataRow<DataRow>(e.RowIndex);
                ddr["Price"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToDecimal(ddr["UnitPrice"]);
                ddr["Amount"] = Convert.ToDecimal(e.FormattedValue) * Convert.ToInt32(ddr["PoQty"]) * Convert.ToDecimal(ddr["UnitPrice"]);
                ddr.EndEdit();
            };

           
            this.gridBatchImport.Font = new Font("Arial", 9);
            this.gridBatchImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridBatchImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridBatchImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("ArtworkTypeID", header: "Artwork Type", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Numeric("OrderQty", header: "Order Qty", iseditingreadonly: true)
                .Numeric("IssueQty", header: "Accu. PO Qty", iseditingreadonly: true)
                .Numeric("PoQty", header: "Req. Qty", iseditingreadonly: true)
                .Date("sewinline", header: "Sewinline", iseditingreadonly: true)
                .Date("SciDelivery", header: "SciDelivery", iseditingreadonly: true)
                .Text("Article", header: "Article", iseditingreadonly: true)
                .Text("artworkid", header: "Artwork", iseditingreadonly: true)      //5
                .Numeric("coststitch", header: "Cost" + Environment.NewLine + "(Pcs/Stitch)", iseditingreadonly: true)
                .Numeric("Stitch", header: this.titleStitch, iseditable: true, iseditingreadonly: true)    //7
                .Text("PatternCode", header: "Cut Part", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cut Part Name", iseditingreadonly: true)
                .Numeric("qtygarment", header: "Qty/GMT", iseditable: true, integer_places: 2, settings: ns2, iseditingreadonly: true) //10
                .Numeric("Cost", header: "Cost(USD)", settings: ns, iseditingreadonly: true, decimal_places: 4, integer_places: 4)  //11
                .Numeric("UnitPrice", header: "Unit Price", settings: ns, iseditable: true, decimal_places: 4, integer_places: 4)  //12
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true, decimal_places: 4, integer_places: 5)  //13
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 4, integer_places: 14);  //14


            //this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            //this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;  //Qty/GMT
            this.gridBatchImport.Columns["UnitPrice"].DefaultCellStyle.BackColor = Color.Pink;  //UnitPrice
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
                        findrow[0]["poqty"] = tmp["PoQty"];
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

        private string QuoteFromPlanningB03()
        {
            string strSQLCmd = string.Empty;
            strSQLCmd = string.Format(@"
select  Selected = 0
        , sao.LocalSuppId
        , id = ''
        , orderid = ard.OrderID
        , OrderQty = sum(q.qty)  
        , IssueQty.IssueQty 
        , [PoQty] = ard.ReqQty
        , ar.ArtworkTypeID
        , ard.ArtworkID
        , ard.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = oa.qty
        , ard.Stitch 
        , ard.PatternDesc
        , ard.QtyGarment
        , Cost = iif(at.isArtwork = 1,vsa.Cost,sao.Price)
        , unitprice = sao.Price
        , price = sao.Price
        , amount = iif((sum(q.qty)-IssueQty.IssueQty) < 0 ,0 ,(sum(q.qty)-IssueQty.IssueQty) *  sao.Price )
        , Style = o.StyleID
		, o.POID
        , [ArtworkReqID] = ar.ID
        , oa.Article
from  orders o WITH (NOLOCK) 
inner join order_qty q WITH (NOLOCK) on q.id = o.ID
inner join dbo.View_Order_Artworks oa on oa.ID = o.ID AND OA.Article = Q.Article AND OA.SizeCode=Q.SizeCode
inner join dbo.View_Style_Artwork vsa on	vsa.StyleUkey = o.StyleUkey and vsa.Article = oa.Article and vsa.ArtworkID = oa.ArtworkID and
														vsa.ArtworkName = oa.ArtworkName and vsa.ArtworkTypeID = oa.ArtworkTypeID and vsa.PatternCode = oa.PatternCode and
														vsa.PatternDesc = oa.PatternDesc 
inner join ArtworkReq_Detail ard with (nolock) on   ard.OrderId = o.ID and 
                                                    ard.ArtworkID = oa.ArtworkID and 
                                                    ard.PatternCode = oa.PatternCode and 
                                                    ard.PatternDesc = oa.PatternDesc and
                                                    ard.ArtworkPOID = ''
inner join ArtworkReq ar WITH (NOLOCK) on ar.ID = ard.ID and ar.ArtworkTypeID = vsa.ArtworkTypeID and ar.Status = 'Approved'
inner join Style_Artwork_Quot sao with (nolock) on sao.Ukey = vsa.StyleArtworkUkey and sao.PriceApv = 'Y' and sao.Price > 0
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= oa.PatternCode
) IssueQty
where f.IsProduceFty=1
--and o.PulloutComplete = 0
and o.category  in ('B','S')
and o.MDivisionID='{0}' and oa.ArtworkTypeID = '{1}' and sao.LocalSuppId = '{2}' and o.Junk=0
and (o.category !='B')
", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"], dr_artworkpo["localsuppid"]);

            if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", apvdate_b); }
            if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate <= '{0}' and ar.Exceed = 0) or (ar.MgApvDate <= '{0}' and ar.Exceed = 1)) ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }

            strSQLCmd += @" group by ard.OrderID,sao.LocalSuppID,o.SewInLIne,o.SciDelivery,
oa.qty,IssueQty.IssueQty, o.StyleID, o.StyleID,iif(at.isArtwork = 1,vsa.Cost,sao.Price),sao.Price , o.POID,
		ar.ID,ard.ReqQty,ard.ArtworkID,ar.ArtworkTypeID,ard.PatternCode,ard.Stitch,ard.PatternDesc,ard.QtyGarment, oa.Article
";

            return strSQLCmd;
        }

        private string QuoteFromTmsCost()
        {
            string strSQLCmd = string.Empty;

            strSQLCmd = $@"
select  Selected = 0
        , ot.LocalSuppID
        , id = ''
        , orderid = ard.OrderID
        , OrderQty = o.qty 
        , IssueQty.IssueQty 
        , [PoQty] = ard.ReqQty
        , ar.ArtworkTypeID
        , ard.ArtworkID
        , ard.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = 1
        , ard.Stitch
        , ard.PatternDesc
        , ard.QtyGarment
        , Cost = ot.Price
        , unitprice = ot.Price
        , price = ot.Price * isnull (ot.Qty, 1)
        , amount = iif(o.qty-IssueQty.IssueQty < 0, 0, (o.qty-IssueQty.IssueQty) * ot.Price * isnull (ot.Qty, 1)) 
        , Style = o.StyleID
        , [ArtworkReqID] = ar.ID
        , [Article] = (SELECT Stuff((select concat( ',',Article)   from Order_Article with (nolock) where ID = o.ID FOR XML PATH('')),1,1,'') )
from ArtworkReq ar WITH (NOLOCK) 
inner join ArtworkReq_Detail ard with (nolock) on ar.ID = ard.ID and ard.ArtworkPOID = ''
inner join orders o WITH (NOLOCK) on ard.OrderID = o.ID
inner join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = o.ID and ot.ArtworkTypeID = ar.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= ard.PatternCode
) IssueQty
where   
f.IsProduceFty=1
and o.category  in ('B','S')
and ar.Status = 'Approved'
";

            strSQLCmd += string.Format(" and o.MDivisionID='{0}' and ar.ArtworkTypeID like '{1}%' and o.Junk=0 ", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"]);
            if (poType == "O")
            {
                strSQLCmd += @"  and ((o.Category = 'B' and ot.InhouseOSP='O' and ot.price > 0) or o.category !='B')";
            }
            else
            {
                strSQLCmd += $" and ot.InhouseOSP = 'I'";
            }
            if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", apvdate_b); }
            if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate <= '{0}' and ar.Exceed = 0) or (ar.MgApvDate <= '{0}' and ar.Exceed = 1)) ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }


            return strSQLCmd;
        }

        private string QuoteIsSintexSubcon()
        {
            string strSQLCmd = string.Empty;
            strSQLCmd = string.Format(@"
select  Selected = 0
        , ot.LocalSuppId
        , id = ''
        , orderid = ard.OrderID
        , OrderQty = sum(q.qty)  
        , IssueQty.IssueQty 
        , [PoQty] = ard.ReqQty
        , ar.ArtworkTypeID
        , ard.ArtworkID
        , ard.PatternCode
        , o.SewInLIne
        , o.SciDelivery
        , coststitch = oa.qty
        , ard.Stitch
        , ard.PatternDesc
        , ard.QtyGarment
        , Cost = oa.Cost
        , unitprice = oa.Cost
        , price = oa.Cost
        , amount = iif((sum(q.qty)-isnull(IssueQty.IssueQty,0)) < 0 ,0 ,(sum(q.qty)-isnull(IssueQty.IssueQty,0)) *  isnull(oa.Cost,0) )
        , Style = o.StyleID
        , [ArtworkReqID] = ar.ID
        , OA.Article
from  orders o WITH (NOLOCK)
inner join order_qty q WITH (NOLOCK) on q.id = o.ID
inner join dbo.View_Order_Artworks oa on oa.ID = o.ID AND OA.Article = Q.Article AND OA.SizeCode=Q.SizeCode
inner join ArtworkReq_Detail ard with (nolock) on   ard.OrderId = o.ID and 
                                                    ard.ArtworkID = oa.ArtworkID and 
                                                    ard.PatternCode = oa.PatternCode and 
                                                    ard.PatternDesc = oa.PatternDesc and
                                                    ard.ArtworkPOID = ''
inner join ArtworkReq ar WITH (NOLOCK) on ar.ID = ard.ID and ar.ArtworkTypeID = oa.ArtworkTypeID  and ar.Status = 'Approved'
inner join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = oa.ID and ot.ArtworkTypeID = oa.ArtworkTypeID
left join ArtworkType at WITH (NOLOCK) on at.id = oa.ArtworkTypeID
inner join factory f WITH (NOLOCK) on o.factoryid=f.id
left join LocalSupp ls with (nolock) on ls.id = ot.LocalSuppID
outer apply (
        select IssueQty = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD, ArtworkPO A
        where AD.ID = A.ID and A.Status = 'Approved' and OrderID = o.ID and ad.PatternCode= oa.PatternCode
) IssueQty
where f.IsProduceFty=1
and o.category  in ('B','S')
and o.MDivisionID='{0}' and oa.ArtworkTypeID = '{1}' and ot.LocalSuppId = '{2}' and o.Junk=0
and ((o.Category = 'B' and  ot.InhouseOSP='O' and ot.price > 0) or (o.category !='B'))
", Sci.Env.User.Keyword, dr_artworkpo["artworktypeid"], dr_artworkpo["localsuppid"]);

            if (!(dateSCIDelivery.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery >= '{0}' ", sciDelivery_b); }
            if (!(dateSCIDelivery.Value2 == null)) { strSQLCmd += string.Format(" and o.SciDelivery <= '{0}' ", sciDelivery_e); }
            if (!(dateApproveDate.Value1 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate >= '{0}' and ar.Exceed = 0) or (ar.MgApvDate >= '{0}' and ar.Exceed = 1)) ", apvdate_b); }
            if (!(dateApproveDate.Value2 == null)) { strSQLCmd += string.Format(" and ((ar.DeptApvDate <= '{0}' and ar.Exceed = 0) or (ar.MgApvDate <= '{0}' and ar.Exceed = 1)) ", apvdate_e); }
            if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and o.ID between '{0}' and '{1}'", sp_b, sp_e); }

            strSQLCmd += @" group by ard.OrderID,ot.LocalSuppID,o.SewInLIne,o.SciDelivery,oa.qty,IssueQty.IssueQty, o.StyleID, o.StyleID,oa.Cost,
ard.ReqQty,ard.ArtworkID,ar.ArtworkTypeID,ard.PatternCode,ard.Stitch,ard.PatternDesc,ard.QtyGarment,ar.ID,OA.Article ";

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

            if (!isSampleOrder && isNeedPlanningB03Quote)
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
