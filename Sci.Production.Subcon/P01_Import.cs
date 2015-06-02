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
        private void button1_Click(object sender, EventArgs e)
        {
            string apvdate_b, apvdate_e, sciDelivery_b, sciDelivery_e, Inline_b, Inline_e;
            apvdate_b = null;
            apvdate_e = null;
            sciDelivery_b = null;
            sciDelivery_e = null;
            Inline_b = null;
            Inline_e = null;

            if (dateRange1.Value1 != null) apvdate_b = this.dateRange1.Text1;
            if (dateRange1.Value2 != null) { apvdate_e = this.dateRange1.Text2; }
            if (dateRange2.Value1 != null) sciDelivery_b = this.dateRange2.Text1;
            if (dateRange2.Value2 != null) { sciDelivery_e = this.dateRange2.Text2; }
            if (dateRange3.Value1 != null) Inline_b = this.dateRange3.Text1;
            if (dateRange3.Value2 != null) { Inline_e = this.dateRange3.Text2; }


            String sp_b = this.textBox1.Text;
            String sp_e = this.textBox2.Text;

            if ((apvdate_b == null && apvdate_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                (Inline_b == null && Inline_e == null) &&
                string.IsNullOrWhiteSpace(sp_b) && string.IsNullOrWhiteSpace(sp_e))
            {
                MessageBox.Show("< Approve Date > or < SCI Delivery > or < Inline Date > or < SP# > can't be empty!!");
                dateRange1.Focus1();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                string strSQLCmd = @"select 0 as Selected, '' as id, q.id as orderid ,sum(q.qty) poqty
                                            , artwk.ArtworkTypeID,artwk.ArtworkID,artwk.PatternCode,o.SewInLIne,o.SciDelivery
                                             ,oa.Stitch as coststitch,oa.Stitch,oa.PatternDesc,1 as qtygarment,oa.Cost
                                            , oa.Cost unitprice, oa.Cost as  price, sum(q.qty)*cost as amount
                                    from orders o,order_qty q, order_artwork oa,
                                            (select c.ID,c.ArtworkTypeID,d.ArtworkID,d.PatternCode
                                                from orders b, Order_TmsCost c, Order_Artwork d
                                                where b.id= c.id and b.ID=d.id and c.ArtworkTypeID = d.ArtworkTypeID and c.apvdate is not null and c.localsuppid !=''";

                                            strSQLCmd += string.Format("     and c.ArtworkTypeID = '{0}'", dr_artworkpo["artworktypeid"]);
                                            if (poType == "O") { strSQLCmd += "     and ((b.Category = 'B' and c.InhouseOSP='O' and c.price > 0) or (b.category !='B'))"; }
                                            if (!(dateRange2.Value1 == null)) { strSQLCmd += string.Format(" and b.SciDelivery between '{0}' and '{1}'", sciDelivery_b, sciDelivery_e); }
                                            if (!(dateRange1.Value1 == null)) { strSQLCmd += string.Format(" and c.ApvDate between '{0}' and '{1}'", apvdate_b, apvdate_e); }
                                            if (!(dateRange3.Value1 == null)) { strSQLCmd += string.Format(" and not (c.ArtworkInLine > '{0}' or c.ArtworkOffLine < '{1}') ", Inline_b, Inline_e); }
                                            if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("     and b.ID between '{0}' and '{1}'", sp_b, sp_e); }
                            
                                            strSQLCmd += string.Format(@" except
                                                  select b1.orderid,a1.ArtworkTypeID, b1.ArtworkId,b1.PatternCode
                                                  from artworkpo a1,ArtworkPO_Detail b1
                                                  where a1.id = b1.id
                                                  and a1.POType = '{0}'", poType);
                                            if (!(string.IsNullOrWhiteSpace(dr_artworkpo["id"].ToString()))) { strSQLCmd += string.Format("  and a1.id !='{0}'", dr_artworkpo["id"]); }
                                            if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format(" and b1.orderID between '{0}' and '{1}'", sp_b, sp_e); }
                                            strSQLCmd += string.Format(" and a1.ArtworkTypeID= '{0}') as artwk" , dr_artworkpo["artworktypeid"]);
                strSQLCmd += @" where o.ID = q.ID and o.id = oa.id 
                                and o.ID = artwk.ID and oa.ArtworkTypeID = artwk.ArtworkTypeID 
                                and oa.artworkid = artwk.artworkid and oa.PatternCode = artwk.PatternCode";

                if (!(string.IsNullOrWhiteSpace(sp_b))) { strSQLCmd += string.Format("  and o.ID between '{0}' and '{1}'", sp_b, sp_e); }
                if (!(dateRange2.Value1 == null)) { strSQLCmd += string.Format(" and o.SciDelivery between '{0}' and '{1}'", sciDelivery_b, sciDelivery_e); }

                strSQLCmd += " group by q.id,artwk.ArtworkTypeID,artwk.ArtworkID,artwk.PatternCode,o.SewInLIne,o.SciDelivery,oa.Stitch,oa.Cost,oa.PatternDesc";

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MessageBox.Show("Data not found!!"); }
                    gridBS1.DataSource = dtArtwork;
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
                DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);
                ddr["Price"] = (decimal)e.FormattedValue * (int)ddr["qtygarment"];
                ddr["Amount"] = (decimal)e.FormattedValue * (int)ddr["poqty"] * (int)ddr["qtygarment"];
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);
                ddr["Price"] = (decimal)e.FormattedValue * (decimal)ddr["UnitPrice"];
                ddr["Amount"] = (decimal)e.FormattedValue * (int)ddr["poqty"] * (decimal)ddr["UnitPrice"];
            };

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = gridBS1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Numeric("poqty", header: "PO QTY", iseditingreadonly: true)
                .Date("sewinline", header: "Sewinline", iseditingreadonly: true)
                .Date("SciDelivery", header: "SciDelivery", iseditingreadonly: true)
                .Text("artworkid", header: "Artwork", iseditingreadonly: true)      //5
                .Numeric("coststitch)", header: "Cost(Pcs/Stitch)", iseditingreadonly: true)
                .Numeric("Stitch", header: "Stitch", iseditable: true)    //7
                .Text("PatternCode", header: "Cutpart Id", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", iseditingreadonly: true)
                .Numeric("qtygarment", header: "Qty/GMT", iseditable: true, integer_places: 2, settings: ns2) //10
                .Numeric("Cost", header: "Cost(USD)", settings: ns, iseditingreadonly: true, decimal_places: 4, integer_places: 4)  //11
                .Numeric("UnitPrice", header: "Unit Price", settings: ns, iseditable: true, decimal_places: 4, integer_places: 4)  //12
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true, decimal_places: 4, integer_places: 5)  //13
                .Numeric("Amount", header: "Amount", iseditingreadonly: true, decimal_places: 4, integer_places: 14);  //14


            this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;  //Qty/GMT
            this.grid1.Columns[12].DefaultCellStyle.BackColor = Color.Pink;  //UnitPrice

            this.grid1.Columns[11].Visible = flag;
            this.grid1.Columns[12].Visible = flag;
            this.grid1.Columns[13].Visible = flag;
            this.grid1.Columns[14].Visible = flag;


            // 全選
            checkBox1.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetCheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };

            // 全不選
            checkBox2.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetUncheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            gridBS1.EndEdit();
            DataTable dtGridBS1 = (DataTable)gridBS1.DataSource;
            if (dtGridBS1.Rows.Count == 0) return;
            DataRow[] dr2 = dtGridBS1.Select("UnitPrice = 0 and Selected = 1");


            if (dr2.Length > 0 && flag)
            {
                MessageBox.Show("UnitPrice of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length > 0)
            {
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
                MessageBox.Show("Please select rows first!", "Warnning");
                return;
            }
            this.Close();
        }
    }
}
