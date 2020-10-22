using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Sci.Data;
using Ict;
using Ict.Win;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P01_SpecialRecord : Win.Subs.Base
    {
        private DataTable dt_artworkpo_detail;
        private DataRow dr;
        private bool flag;
        private DataTable dtArtwork;
        private string poType;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <inheritdoc/>
        public P01_SpecialRecord()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public P01_SpecialRecord(DataRow data, DataTable detail, string fuc)
        {
            this.InitializeComponent();
            this.dt_artworkpo_detail = detail;
            this.dr = data;
            this.flag = fuc == "P01";
            if (this.flag)
            {
                this.poType = "O";
            }
            else
            {
                this.poType = "I";
            }

            this.Text += string.Format(" : {0}", this.dr["artworktypeid"].ToString());
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorNumericColumnSettings pos = new DataGridViewGeneratorNumericColumnSettings();
            pos.CellValidating += (s, e) =>
            {
                DataRow ddr = this.gridSpecialRecord.GetDataRow<DataRow>(e.RowIndex);
                ddr["poqty"] = (decimal)e.FormattedValue;
                ddr["Amount"] = Convert.ToDecimal(ddr["UnitPrice"].ToString()) * (decimal)e.FormattedValue;
            };

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    DataRow ddr = this.gridSpecialRecord.GetDataRow<DataRow>(e.RowIndex);
                    ddr["Price"] = (decimal)e.FormattedValue * 1;
                    ddr["Amount"] = (decimal)e.FormattedValue * (int)ddr["poqty"] * 1;
                    ddr["UnitPrice"] = (decimal)e.FormattedValue;
                };

            this.gridSpecialRecord.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridSpecialRecord.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridSpecialRecord)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Numeric("poqty", header: "PO QTY", settings: pos)
                .Numeric("qtygarment", header: "Qty/GMT", iseditingreadonly: true)
                .Numeric("UnitPrice", header: "UnitPrice", decimal_places: 4, settings: ns, iseditable: this.flag)
                .Numeric("Price", header: "Price/GMT", decimal_places: 4, iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", decimal_places: 2, iseditingreadonly: true);

            this.gridSpecialRecord.Columns["UnitPrice"].Visible = this.flag;
            this.gridSpecialRecord.Columns["UnitPrice"].DefaultCellStyle.BackColor = Color.Pink;  // UnitPrice
            this.gridSpecialRecord.Columns["poqty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridSpecialRecord.Columns["Price"].Visible = this.flag;
            this.gridSpecialRecord.Columns["Amount"].Visible = this.flag;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            string orderID = this.txtSPNo.Text;
            string poid = this.txtMotherSPNo.Text;

            if (string.IsNullOrWhiteSpace(orderID) && string.IsNullOrWhiteSpace(poid))
            {
                MyUtility.Msg.WarningBox("< SP# > or < Mother SP# > can't be empty!!");
                this.txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                string strSQLCmd = string.Format(
                    @"
Select  0 as Selected
        , '' as id
        , aaa.id as orderid
        , sum(bbb.qty) poqty 
        , unitprice = isnull(iif(ccc.isArtwork = 1,oa.Cost,bb.price),0)
        , price = isnull(iif(ccc.isArtwork = 1,oa.Cost,bb.price),0)
        , amount = isnull(sum(bbb.qty) * iif(ccc.isArtwork = 1,oa.Cost,bb.price),0)
        , 1 as qtygarment
        , 0.0000 as pricegmt
        , ccc.id as artworktypeid
        , rtrim(ccc.id) as artworkid
        , aaa.StyleID
        , sewinline = aaa.Sewinline
        , scidelivery = aaa.Scidelivery
		, aaa.POID
from orders aaa WITH (NOLOCK) 
inner join order_qty bbb WITH (NOLOCK) on aaa.id = bbb.id
left join dbo.View_Order_Artworks oa on oa.ID = aaa.ID AND OA.Article = bbb.Article AND OA.SizeCode=bbb.SizeCode and oa.ArtworkTypeID='{0}' 
left join dbo.Order_TmsCost ot WITH (NOLOCK) on ot.ID = oa.ID and ot.ArtworkTypeID = oa.ArtworkTypeID
inner join (
	Select   a.id orderid
			, c.id as artworktypeid
			, rtrim(c.id) as artwork
			, c.id as  patterncode 
	from orders a WITH (NOLOCK) 
		, artworktype  c WITH (NOLOCK) 
		, factory WITH (NOLOCK) 
        where c.id = '{0}'            
        and a.FactoryID = factory.id and factory.IsProduceFty = 1
        --and a.PulloutComplete = 0
		and a.Category  in ('B','S')", this.dr["artworktypeid"]);
                if (!string.IsNullOrWhiteSpace(orderID))
                {
                    strSQLCmd += string.Format(" and ((a.category='B' and c.isArtwork=0)  or (a.category !='B')) and a.ID = '{0}'", orderID);
                }

                if (!string.IsNullOrWhiteSpace(poid))
                {
                    strSQLCmd += string.Format(" and a.poid = '{0}'", poid);
                }

                if (this.poType.Equals("O"))
                {
                    strSQLCmd += $" and (( a.category = 'B' and c.IsSubprocess = 1 and c.isArtwork = 0 and c.Classify = 'O') or (a.category !='B'))";
                }

                strSQLCmd += " EXCEPT" +
                 "     select b1.orderid,a1.ArtworkTypeID, b1.ArtworkId,b1.PatternCode " +
                 "     from artworkpo a1 WITH (NOLOCK) ,ArtworkPO_Detail b1 WITH (NOLOCK) ";
                if (!string.IsNullOrWhiteSpace(poid))
                {
                    strSQLCmd += " ,orders c1";
                }

                strSQLCmd += "     where a1.id = b1.id ";
                if (!string.IsNullOrWhiteSpace(poid))
                {
                    strSQLCmd += " and b1.orderid=c1.id";
                }

                strSQLCmd += string.Format(
                    "     and a1.POType = '{1}'" +
                 "     and a1.ArtworkTypeID= '{0}'", this.dr["artworktypeid"], this.poType);
                if (!string.IsNullOrWhiteSpace(this.dr["id"].ToString()))
                {
                    strSQLCmd += string.Format("  and a1.id !='{0}'", this.dr["id"]);
                }

                if (!string.IsNullOrWhiteSpace(orderID))
                {
                    strSQLCmd += string.Format(" and b1.OrderID = '{0}'", poid);
                }

                if (!string.IsNullOrWhiteSpace(poid))
                {
                    strSQLCmd += string.Format(" and c1.poid = '{0}'", poid);
                }

                strSQLCmd += @"
) as aa on aaa.ID = aa.orderid
left join artworktype  ccc WITH (NOLOCK) on  ccc.ID = aa.artworktypeid
outer apply(select ott.price from Order_TmsCost ott where ott.artworktypeid = aa.artworktypeid and ott.id = aa.orderid)bb
group by bbb.id, ccc.id, aaa.id, aaa.StyleID, aaa.Sewinline, aaa.Scidelivery,ccc.isArtwork ,oa.Cost,bb.Price,aaa.POID ";

                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, out this.dtArtwork))
                {
                    if (this.dtArtwork.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }

                    this.listControlBindingSource1.DataSource = this.dtArtwork;
                }
                else
                {
                    this.ShowErr(strSQLCmd, result);
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1))
            {
                return;
            }

            if (dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("UnitPrice = 0 and Selected = 1");

            if (dr2.Length > 0 && this.flag)
            {
                MyUtility.Msg.WarningBox("UnitPrice of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected =  1");
            if (dr2.Length > 0)
            {
                // dtGridBS1.Rows
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = this.dt_artworkpo_detail.Select(string.Format("orderid = '{0}' and ArtworkId = '{1}' ", tmp["orderid"].ToString(), tmp["ArtworkId"].ToString()));

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
                        tmp["id"] = this.dr["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        this.dt_artworkpo_detail.ImportRow(tmp);
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtSPNo_Validating(object sender, CancelEventArgs e)
        {
            this.Val(sender, e);
        }

        private void Val(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((Win.UI.TextBox)sender).Text))
            {
                return;
            }

            if (!MyUtility.Check.Seek(string.Format("select POID from orders WITH (NOLOCK) where id='{0}'", ((Win.UI.TextBox)sender).Text), null))
            {
                MyUtility.Msg.WarningBox(string.Format("SP# ({0}) is not found!", ((Win.UI.TextBox)sender).Text));
                return;
            }

            string category = MyUtility.GetValue.Lookup(string.Format("select category from orders WITH (NOLOCK) where ID = '{0}' ", ((Win.UI.TextBox)sender).Text), null);
            string sqlCheckArtwork = string.Empty;
            if (this.poType.Equals("O"))
            {
                sqlCheckArtwork = $@"select id from artworktype WITH (NOLOCK) 
                                        where id = '{this.dr["artworktypeid"]}'
                                        EXCEPT select ID from artworktype WITH (NOLOCK) 
                                                where IsSubprocess = 1 and isArtwork = 0 and Classify = 'O'";
            }
            else
            {
                sqlCheckArtwork = $"select 1 from artworktype WITH (NOLOCK) where id='{this.dr["artworktypeid"]}' and isArtwork = 1";
            }

            bool isArtworkCheckNG = MyUtility.Check.Seek(sqlCheckArtwork);
            if (category != "S" && isArtworkCheckNG)
            {
                ((Win.UI.TextBox)sender).Text = string.Empty;
                e.Cancel = true;
                if (this.poType.Equals("O"))
                {
                    MyUtility.Msg.WarningBox("Bulk orders only allow Artwork which like RECOAT Garment ....!!", "Warning");
                }
                else
                {
                    MyUtility.Msg.WarningBox("Bulk orders only allow Artwork is like Bonding,GMT Wash, ....!!", "Warning");
                }

                return;
            }
        }

        private void TxtMotherSPNo_Validating(object sender, CancelEventArgs e)
        {
            this.Val(sender, e);
        }
    }
}
