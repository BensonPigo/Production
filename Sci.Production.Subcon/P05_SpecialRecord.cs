using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;

namespace Sci.Production.Subcon
{
    public partial class P05_SpecialRecord : Sci.Win.Subs.Base
    {
        DataTable dt_artworkpo_detail;
        DataRow dr;
        protected DataTable dtArtwork;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        public P05_SpecialRecord()
        {
            InitializeComponent();
        }
        public P05_SpecialRecord(DataRow data, DataTable detail)
        {
            InitializeComponent();
            dt_artworkpo_detail = detail;
            dr = data;

            this.Text += string.Format(" : {0}", dr["artworktypeid"].ToString());
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();


            this.gridSpecialRecord.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridSpecialRecord.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridSpecialRecord)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Numeric("OrderQty", header: "Qrder Qty", iseditingreadonly: true)
                .Numeric("AccReqQty", header: "Accu. Req QTY", iseditingreadonly: true)
                .Numeric("ReqQty", header: "Req QTY")
                .Numeric("qtygarment", header: "Qty/GMT", iseditingreadonly: true);

            this.gridSpecialRecord.Columns["ReqQty"].DefaultCellStyle.BackColor = Color.Pink;

        }

        private void btnFindNow_Click(object sender, EventArgs e)
        {
            string orderID = this.txtSPNo.Text;
            string poid = this.txtMotherSPNo.Text;

            if (string.IsNullOrWhiteSpace(orderID) && string.IsNullOrWhiteSpace(poid))
            {
                MyUtility.Msg.WarningBox("< SP# > or < Mother SP# > can't be empty!!");
                txtSPNo.Focus();
                return;
            }
            else
            {
                string sqlwhere = string.Empty;
                if (!MyUtility.Check.Empty(orderID))
                {
                    sqlwhere += string.Format(" and o.ID = '{0}' ", orderID);
                }
                if (!MyUtility.Check.Empty(poid))
                {
                    sqlwhere += string.Format(" and o.poid = '{0}' ", poid);
                }


                string strSQLCmd = $@"
--get special record
select
        0 as Selected
        , '' as id
        , [orderid] = o.id
        , [ReqQty] =  iif(o.qty-(ReqQty.value + PoQty.value) < 0, 0, o.qty- (ReqQty.value + PoQty.value))
        , [qtygarment] = 1 
        , [artworktypeid] = at.ID
        , [artworkid] = at.ID
        , o.StyleID
        , sewinline = o.Sewinline
        , scidelivery = o.Scidelivery
		, o.POID
        , [OrderQty] = o.Qty
        , [ExceedQty] = 0
        , [AccReqQty] = isnull(ReqQty.value,0) + isnull(PoQty.value,0)
        , [Stitch] = 1
from orders o with (nolock)
cross join ArtworkType at with (nolock)
outer apply (
        select value = ISNULL(sum(ReqQty),0)
        from ArtworkReq_Detail AD, ArtworkReq a
        where ad.ID=a.ID
		and a.ArtworkTypeID = at.ID
		and OrderID = o.ID 
        and ad.PatternCode= ''
        and ad.PatternDesc = ''
        and ad.ArtworkID = '{dr["artworktypeid"]}'
        and a.id != '{dr["id"]}'
        and a.status != 'Closed'
) ReqQty
outer apply (
        select value = ISNULL(sum(PoQty),0)
        from ArtworkPO_Detail AD,ArtworkPO A
        where a.ID=ad.ID
		and a.ArtworkTypeID = at.ID
		and OrderID = o.ID 
        and ad.PatternCode= ''
        and ad.PatternDesc = ''
        and ad.ArtworkID = '{dr["artworktypeid"]}'
		and ad.ArtworkReqID=''
) PoQty
where (o.Junk=0 or o.Junk=1 and o.NeedProduction=1) and o.Qty > 0 and
	(
	(o.Category = 'B' and at.IsSubprocess = 1 and at.isArtwork = 0 and at.Classify = 'O' and at.ID = '{dr["artworktypeid"]}') 
	or 
	(o.category = 'S' and at.ID = '{dr["artworktypeid"]}')
	)
    {sqlwhere}
 ";


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

        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1))
            {
                return;
            }
            if (dtGridBS1.Rows.Count == 0) return;
            DataRow[] dr2 = dtGridBS1.Select("ReqQty = 0 and Selected = 1");


            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("ReqQty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected =  1");
            if (dr2.Length > 0)
            {
                foreach (DataRow tmp in dr2)//dtGridBS1.Rows
                {
                    DataRow[] findrow = dt_artworkpo_detail.Select(string.Format("orderid = '{0}' and ArtworkId = '{1}' ", tmp["orderid"].ToString(), tmp["ArtworkId"].ToString()));
                    decimal exceedQty = MyUtility.Convert.GetDecimal(tmp["ReqQty"]) + MyUtility.Convert.GetDecimal(tmp["AccReqQty"]) - MyUtility.Convert.GetDecimal(tmp["OrderQty"]);

                    decimal finalExceedQty = exceedQty < 0 ? 0 : exceedQty;
                    if (findrow.Length > 0)
                    {
                        findrow[0]["ReqQty"] = tmp["ReqQty"];
                        findrow[0]["ExceedQty"] = finalExceedQty;
                        findrow[0]["qtygarment"] = 1;
                        findrow[0]["StyleID"] = tmp["StyleID"];
                        findrow[0]["Sewinline"] = tmp["Sewinline"];
                        findrow[0]["SciDelivery"] = tmp["SciDelivery"];
                        findrow[0]["Stitch"] = tmp["Stitch"];
                    }
                    else
                    {

                        tmp["id"] = dr["id"];
                        tmp["Stitch"] = 1;
                        tmp["ExceedQty"] = exceedQty < 0 ? 0 : exceedQty;
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        dt_artworkpo_detail.ImportRow(tmp);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
            val(sender, e);
        }

        private void val(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((Sci.Win.UI.TextBox)sender).Text)) return;

            if (!MyUtility.Check.Seek(string.Format("select POID from orders WITH (NOLOCK) where id='{0}'", ((Sci.Win.UI.TextBox)sender).Text), null))
            {
                MyUtility.Msg.WarningBox(string.Format("SP# ({0}) is not found!", ((Sci.Win.UI.TextBox)sender).Text));
                return;
            }

            string category = MyUtility.GetValue.Lookup(string.Format("select category from orders WITH (NOLOCK) where ID = '{0}' ", ((Sci.Win.UI.TextBox)sender).Text), null);
            string sqlCheckArtwork = string.Empty;
            sqlCheckArtwork = $@"select id from artworktype WITH (NOLOCK) 
                                        where id = '{dr["artworktypeid"]}'
                                        EXCEPT select ID from artworktype WITH (NOLOCK) 
                                                where IsSubprocess = 1 and isArtwork = 0 and Classify = 'O'";


            bool isArtworkCheckNG = MyUtility.Check.Seek(sqlCheckArtwork);
            if (category != "S" && isArtworkCheckNG)
            {
                ((Sci.Win.UI.TextBox)sender).Text = "";
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Bulk orders only allow Artwork which like RECOAT Garment ....!!", "Warning");

                return;
            }
        }

        private void txtMotherSPNo_Validating(object sender, CancelEventArgs e)
        {
            val(sender, e);
        }
    }
}
