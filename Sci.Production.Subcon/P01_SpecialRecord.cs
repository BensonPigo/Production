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
    public partial class P01_SpecialRecord : Sci.Win.Subs.Base
    {
        DataTable dt_artworkpo_detail;
        DataRow dr;
        bool flag;
        protected DataTable dtArtwork;
        string poType;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        public P01_SpecialRecord()
        {
            InitializeComponent();
        }
        public P01_SpecialRecord(DataRow data,DataTable detail,string fuc)
        {
            InitializeComponent();
            dt_artworkpo_detail = detail;
            dr = data;
            flag = fuc == "P01";
            if (flag)
                poType = "O";
            else
                poType = "I";

            this.Text += string.Format(" : {0}", dr["artworktypeid"].ToString());
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings pos = new DataGridViewGeneratorNumericColumnSettings();
            pos.CellValidating += (s, e) =>
            {
                DataRow ddr = gridSpecialRecord.GetDataRow<DataRow>(e.RowIndex);
                ddr["poqty"] = (decimal)e.FormattedValue;
                ddr["Amount"] = Convert.ToDecimal(ddr["UnitPrice"].ToString()) * (decimal)e.FormattedValue;
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    DataRow ddr = gridSpecialRecord.GetDataRow<DataRow>(e.RowIndex);
                    ddr["Price"] = (decimal)e.FormattedValue * 1;
                    ddr["Amount"] = (decimal)e.FormattedValue * (int)ddr["poqty"]*1;
                    ddr["UnitPrice"] = (decimal)e.FormattedValue;
                };

            this.gridSpecialRecord.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridSpecialRecord.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridSpecialRecord)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Numeric("poqty", header: "PO QTY", settings: pos)
                .Numeric("qtygarment", header: "Qty/GMT", iseditingreadonly: true)
                .Numeric("UnitPrice", header: "UnitPrice", decimal_places: 4, settings: ns, iseditable: flag)
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", iseditingreadonly: true);

            this.gridSpecialRecord.Columns["UnitPrice"].Visible = flag;
            this.gridSpecialRecord.Columns["UnitPrice"].DefaultCellStyle.BackColor = Color.Pink;  //UnitPrice
            this.gridSpecialRecord.Columns["poqty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridSpecialRecord.Columns["Price"].Visible = flag;
            this.gridSpecialRecord.Columns["Amount"].Visible = flag;

        }

        private void btnFindNow_Click(object sender, EventArgs e)
        {
            string orderID = this.txtSPNo.Text;
            String poid = this.txtMotherSPNo.Text;

            if (string.IsNullOrWhiteSpace(orderID) && string.IsNullOrWhiteSpace(poid))
            {
                MyUtility.Msg.WarningBox("< SP# > or < Mother SP# > can't be empty!!");
                txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor


                string strSQLCmd = string.Format(@"
Select  0 as Selected
        , '' as id
        , aaa.id as orderid
        , sum(bbb.qty) poqty 
        , unitprice = iif(ccc.isArtwork = 1,oa.Cost,ot.Price)
        , 0.0000 as price
        , 1 as qtygarment
        , 0.0000 as pricegmt
        , 0.0000 as amount 
        , ccc.id as artworktypeid
        , rtrim(ccc.id) as artworkid
        , patterncode = (select distinct PatternCode from view_order_artworks v where aaa.ID = v.id and ccc.ID = v.ArtworkTypeID)
        , patterndesc = (select distinct PatternDesc from view_order_artworks v where aaa.ID = v.id and ccc.ID = v.ArtworkTypeID)
        , Style = aaa.StyleID
        , sewinline = aaa.Sewinline
        , scidelivery = aaa.Scidelivery
from orders aaa WITH (NOLOCK) 
inner join order_qty bbb WITH (NOLOCK) on aaa.id = bbb.id
inner join dbo.View_Order_Artworks oa on oa.ID = aaa.ID AND OA.Article = bbb.Article AND OA.SizeCode=bbb.SizeCode
inner join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = oa.ID and ot.ArtworkTypeID = oa.ArtworkTypeID
,artworktype  ccc WITH (NOLOCK)
        , (Select   a.id orderid
                    , c.id as artworktypeid
                    , rtrim(c.id) as artwork
                    , c.id as  patterncode 
           from orders a WITH (NOLOCK) 
                , artworktype  c WITH (NOLOCK) 
                , factory WITH (NOLOCK) 
           where c.id = '{0}'            
           and a.FactoryID = factory.id and factory.IsProduceFty = 1
		   and a.Category  in ('B','S')", dr["artworktypeid"]);
	             if (!string.IsNullOrWhiteSpace(orderID)) { strSQLCmd += string.Format(" and ((a.category='B' and c.isArtwork=0)  or (a.category !='B')) and a.ID = '{0}'", orderID); }
                 if (!string.IsNullOrWhiteSpace(poid)) { strSQLCmd += string.Format(" and a.poid = '{0}'", poid); }
                strSQLCmd +=" EXCEPT"+
	             "     select b1.orderid,a1.ArtworkTypeID, b1.ArtworkId,b1.PatternCode "+
                 "     from artworkpo a1 WITH (NOLOCK) ,ArtworkPO_Detail b1 WITH (NOLOCK) ";
                 if (!string.IsNullOrWhiteSpace(poid)) { strSQLCmd += " ,orders c1"; }
                 strSQLCmd += "     where a1.id = b1.id ";
                 if (!string.IsNullOrWhiteSpace(poid)) { strSQLCmd += " and b1.orderid=c1.id"; }
	             strSQLCmd +=string.Format("     and a1.POType = '{1}'"+
	             "     and a1.ArtworkTypeID= '{0}'", dr["artworktypeid"],poType);
                if (!(string.IsNullOrWhiteSpace(dr["id"].ToString()))) { strSQLCmd += string.Format("  and a1.id !='{0}'", dr["id"]); }
	             if (!string.IsNullOrWhiteSpace(orderID)) { strSQLCmd += string.Format(" and b1.OrderID = '{0}'", poid); }
                 if (!string.IsNullOrWhiteSpace(poid)) { strSQLCmd += string.Format(" and c1.poid = '{0}'", poid); }
                 strSQLCmd += @"
           ) as aa
where  aaa.ID = aa.orderid and ccc.ID = aa.artworktypeid
group by bbb.id, ccc.id, aaa.id, aaa.StyleID, aaa.Sewinline, aaa.Scidelivery,ccc.isArtwork ,oa.Cost,ot.Price";


                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtArtwork;
                }
                else { ShowErr(strSQLCmd,result); }
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
            if (dtGridBS1.Rows.Count==0)return;
            DataRow[] dr2 = dtGridBS1.Select("UnitPrice = 0 and Selected = 1");
            

            if (dr2.Length > 0 && flag)
            {
                MyUtility.Msg.WarningBox("UnitPrice of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected =  1");
            if (dr2.Length > 0)
            {
                foreach (DataRow tmp in dr2)//dtGridBS1.Rows
                {
                    DataRow[] findrow = dt_artworkpo_detail.Select(string.Format("orderid = '{0}' and ArtworkId = '{1}' and patterncode = '{2}'", tmp["orderid"].ToString(), tmp["ArtworkId"].ToString(), tmp["patterncode"].ToString()));

                    if (findrow.Length > 0)
                    {
                        findrow[0]["unitprice"] = tmp["unitprice"];
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["amount"] = tmp["amount"];
                        findrow[0]["poqty"] = tmp["poqty"];
                        findrow[0]["qtygarment"] = 1;
                        findrow[0]["StyleID"] = tmp["StyleID"];
                        findrow[0]["Sewinline"] = tmp["Sewinline"];
                        findrow[0]["SciDelivery"] = tmp["SciDelivery"];
                    }
                    else
                    {

                        tmp["id"] = dr["id"];
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
            val(sender,e);
        }

        private void val(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((Sci.Win.UI.TextBox)sender).Text)) return;

            if (!MyUtility.Check.Seek(string.Format("select POID from orders WITH (NOLOCK) where id='{0}'", ((Sci.Win.UI.TextBox)sender).Text), null))
            {
                MyUtility.Msg.WarningBox(string.Format("SP# ({0}) is not found!", ((Sci.Win.UI.TextBox)sender).Text));
                return;
            }

            string cat = MyUtility.GetValue.Lookup(string.Format("select category from orders WITH (NOLOCK) where ID = '{0}' ", ((Sci.Win.UI.TextBox)sender).Text), null);
            string isArtwork = MyUtility.GetValue.Lookup(string.Format("select isArtwork from artworktype WITH (NOLOCK) where id='{0}'", dr["artworktypeid"]), null);

            if (cat != "S" && isArtwork.ToUpper() == "TRUE")
            {
                ((Sci.Win.UI.TextBox)sender).Text = "";
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Bulk orders only allow Artwork is like Bonding,GMT Wash, ....!!", "Warning");
                return;
            }
        }

        private void txtMotherSPNo_Validating(object sender, CancelEventArgs e)
        {
            val(sender,e);
        }
    }
}
