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

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);
                    ddr["Price"] = (decimal)e.FormattedValue * 1;
                    ddr["Amount"] = (decimal)e.FormattedValue * (int)ddr["poqty"];
                };

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = gridBS1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Numeric("poqty", header: "PO QTY", iseditingreadonly: true)
                .Numeric("qtygarment", header: "Qty/GMT", iseditingreadonly: true)
                .Numeric("UnitPrice", header: "UnitPrice", settings: ns,iseditable:flag)
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", iseditingreadonly: true);

            this.grid1.Columns[4].Visible = flag;
            this.grid1.Columns[4].DefaultCellStyle.BackColor = Color.Pink;  //UnitPrice
            this.grid1.Columns[5].Visible = flag;
            this.grid1.Columns[6].Visible = flag;

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

        private void button1_Click(object sender, EventArgs e)
        {
            string orderID = this.textBox1.Text;
            String poid = this.textBox2.Text;

            if (string.IsNullOrWhiteSpace(orderID) && string.IsNullOrWhiteSpace(poid))
            {
                MessageBox.Show("< SP# > or < Mother SP# > can't be empty!!");
                textBox1.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor


                string strSQLCmd = string.Format("Select 0 as Selected, '' as id, aaa.id as orderid, sum(bbb.qty) poqty ,0.0000 as unitprice,0.0000 as price,1 as qtygarment,0.0000 as pricegmt," +
                 " 0.0000 as amount ,ccc.id as artworktypeid, rtrim(ccc.id) as artworkid, ccc.id as  patterncode  ,"+
                 " ccc.id as  patterndesc "+
                 " from orders aaa, order_qty bbb, artworktype  ccc "+
                 " ,(Select a.id orderid,c.id as artworktypeid, rtrim(c.id) as artwork, c.id as  patterncode "+
	             "     from orders a, artworktype  c "+
	             "     where c.id = '{0}'", dr["artworktypeid"]);
	             if (!string.IsNullOrWhiteSpace(orderID)) { strSQLCmd += string.Format(" and ((a.category='B' and c.isArtwork=0)  or (a.category !='B')) and a.ID = '{0}'", orderID); }
                 if (!string.IsNullOrWhiteSpace(poid)) { strSQLCmd += string.Format(" and a.poid = '{0}'", poid); }
                strSQLCmd +=string.Format(" EXCEPT"+
	             "     select b1.orderid,a1.ArtworkTypeID, b1.ArtworkId,b1.PatternCode "+
	             "     from artworkpo a1,ArtworkPO_Detail b1"+
	             "     where a1.id = b1.id "+
	             "     and a1.POType = '{1}'"+
	             "     and a1.ArtworkTypeID= '{0}'", dr["artworktypeid"],poType);
	             if (!string.IsNullOrWhiteSpace(orderID)) { strSQLCmd += string.Format(" and b1.OrderID = '{0}'", poid); }
                 if (!string.IsNullOrWhiteSpace(poid)) { strSQLCmd += string.Format(" and b1.poid = '{0}'", poid); }
                 strSQLCmd += "     ) as aa" +
                 " where aaa.id = bbb.id" +
                 " and aaa.ID = aa.orderid" +
                 " and ccc.ID= aa.artworktypeid" +
                 " group by bbb.id,ccc.id,aaa.id";


                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MessageBox.Show("Data not found!!"); }
                    gridBS1.DataSource = dtArtwork;
                }
                else { ShowErr(strSQLCmd,result); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gridBS1.EndEdit();
            DataTable dtGridBS1 = (DataTable)gridBS1.DataSource;
            if (dtGridBS1.Rows.Count==0)return;
            DataRow[] dr2 = dtGridBS1.Select("UnitPrice = 0 and Selected = 1");
            

            if (dr2.Length > 0 && flag)
            {
                MessageBox.Show("UnitPrice of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("Selected =  1");
            if (dr2.Length > 0)
            {
                foreach (DataRow tmp in dtGridBS1.Rows)
                {
                    DataRow[] findrow = dt_artworkpo_detail.Select(string.Format("orderid = '{0}' and ArtworkId = '{1}' and patterncode = '{2}'", tmp["orderid"].ToString(), tmp["ArtworkId"].ToString(), tmp["patterncode"].ToString()));

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

                        tmp["id"] = dr["id"];
                        dt_artworkpo_detail.ImportRow(tmp);
                        
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((Sci.Win.UI.TextBox)sender).Text)) return;

            if (!myUtility.Seek(string.Format("select id from orders where id='{0}'", ((Sci.Win.UI.TextBox)sender).Text), null))
            {
                MessageBox.Show(string.Format("SP# ({0}) is not found!",((Sci.Win.UI.TextBox)sender).Text));
                return;
            }

            string cat = myUtility.Lookup(string.Format("select category from orders where  id= '{0}'", ((Sci.Win.UI.TextBox)sender).Text), null);
            string isArtwork = myUtility.Lookup(string.Format("select isArtwork from artworktype where id='{0}'", dr["artworktypeid"]), null);

            if (cat=="B" && isArtwork.ToUpper()=="TRUE")
            {
                MessageBox.Show("Bulk orders only allow Artwork is like Bonding,GMT Wash, ....!!", "Warning");
                ((Sci.Win.UI.TextBox)sender).Text="";
                e.Cancel = true;
                return;
            }

        }
    }
}
