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
    public partial class P10_ImportFromPO : Sci.Win.Subs.Base
    {
        DataRow dr_artworkAp;
        DataTable dt_artworkApDetail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        protected DataTable dtArtwork;

        public P10_ImportFromPO(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_artworkAp = master;
            dt_artworkApDetail = detail;
            this.Text += string.Format(" : {0} - {1}", dr_artworkAp["artworktypeid"].ToString(), dr_artworkAp["localsuppid"].ToString());
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            String sp_b = this.textBox1.Text;
            String sp_e = this.textBox2.Text;
            String poid_b = this.textBox4.Text;
            String poid_e = this.textBox3.Text;

            if (MyUtility.Check.Empty(sp_b) && MyUtility.Check.Empty(sp_e) && MyUtility.Check.Empty(poid_b) && MyUtility.Check.Empty(poid_e))
            {
                MyUtility.Msg.WarningBox("SP# and Artwork POID can't be emtpqy");
                textBox1.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor

                string strSQLCmd = string.Format(@"Select 1 as Selected
                                                                                ,b.id as artworkpoid
                                                                                , b.orderid
                                                                                ,b.artworkid
                                                                                ,b.stitch
                                                                                ,b.patterncode
                                                                                ,b.patterndesc
                                                                                ,b.unitprice
                                                                                ,b.qtygarment
                                                                                ,b.price
                                                                                ,b.poqty
                                                                                ,b.farmin
                                                                                ,b.apqty accumulatedqty
                                                                                ,b.farmin - b.apqty balance
                                                                                ,0 apqty
                                                                                ,b.ukey artworkpo_detailukey
                                                                                ,'' id
                                                                                ,0.0 amount
                                                                        from artworkpo a, artworkpo_detail b 
                                                                        where a.id = b.id and a.status='Approved' and b.apqty < farmin
                                                                        and a.artworktypeid = '{0}' 
                                                                        and a.localsuppid = '{1}' and a.mdivisionid='{2}'", dr_artworkAp["artworktypeid"],
                                                                                                            dr_artworkAp["localsuppid"]
                                                                                                            ,Sci.Env.User.Keyword);
                if(!MyUtility.Check.Empty(sp_b)){strSQLCmd+= " and b.orderid between @sp1 and @sp2";}
                if (!MyUtility.Check.Empty(poid_b)) { strSQLCmd += " and b.id between @artworkpoid1 and  @artworkpoid2"; }
                strSQLCmd += " order by b.id,b.ukey";

                #region 準備sql參數資料
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@sp1";
                sp1.Value = sp_b;

                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                sp2.ParameterName = "@sp2";
                sp2.Value = sp_e;

                System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                sp3.ParameterName = "@artworkpoid1";
                sp3.Value = poid_b;

                System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                sp4.ParameterName = "@artworkpoid2";
                sp4.Value = poid_e;
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                cmds.Add(sp3);
                cmds.Add(sp4);
                #endregion

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd,cmds, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtArtwork;
                }
                else { ShowErr(strSQLCmd, result); }
            }
            this.grid1.AutoResizeColumns();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);
                    if ((decimal)e.FormattedValue > (decimal)ddr["balance"])
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Qty can't be more than balance");
                        return;
                    }

                    if ((decimal)e.FormattedValue > ((decimal)ddr["poqty"] - (decimal)ddr["accumulatedqty"]))
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Total Qty can't be more than PO Qty");
                        return;
                    }
                    ddr["amount"] = (decimal)e.FormattedValue * (decimal)ddr["price"];
                    ddr["apqty"] = e.FormattedValue;
                }

            };


            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("artworkpoid", header: "Artwork PO", iseditingreadonly: true)
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                 .Text("artworkid", header: "Artwork", iseditingreadonly: true)      //3
                 .Numeric("Stitch", header: "Stitch", iseditable: true)    //4
                 .Text("PatternCode", header: "Cutpart Id", iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", iseditingreadonly: true)
                .Numeric("UnitPrice", header: "Unit Price", settings: ns, iseditable: true, decimal_places: 4, integer_places: 4)  //7
                .Numeric("qtygarment", header: "Qty/GMT", iseditable: true, integer_places: 2) //8
                .Numeric("Price", header: "Price/GMT", iseditingreadonly: true, decimal_places: 4, integer_places: 5)  //9
                .Numeric("poqty", header: "PO Qty", iseditingreadonly: true)
                .Numeric("farmin", header: "Farm In", iseditingreadonly: true)
                .Numeric("accumulatedqty", header: "Accu. Paid Qty", iseditingreadonly: true)
                .Numeric("Balance", header: "Balance", iseditingreadonly: true)
                .Numeric("apqty", header: "Qty",settings:ns)//14
                .Numeric("amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 4, integer_places: 14);

            this.grid1.Columns[14].DefaultCellStyle.BackColor = Color.Pink;  //Qty

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
            listControlBindingSource1.EndEdit();
            grid1.ValidateControl();
            
            DataTable dtImport = (DataTable)listControlBindingSource1.DataSource;
            
            if (dtImport.Rows.Count == 0) return;
            
            DataRow[] dr2 = dtImport.Select("apqty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtImport.Select("Selected = 1");
            if (dr2.Length > 0)
            {
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = dt_artworkApDetail.Select(string.Format("orderid = '{0}' and ArtworkId = '{1}' and patterncode = '{2}'", tmp["orderid"].ToString(), tmp["ArtworkId"].ToString(), tmp["patterncode"].ToString()));

                    if (findrow.Length > 0)
                    {
                       
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["poqty"] = tmp["poqty"];
                        findrow[0]["farmin"] = tmp["farmin"];
                        findrow[0]["accumulatedqty"] = tmp["accumulatedqty"];
                        findrow[0]["balance"] = tmp["balance"];
                        findrow[0]["apqty"] = tmp["apqty"];
                        findrow[0]["amount"] = tmp["amount"];
                    }
                    else
                    {
                        tmp["id"] = dr_artworkAp["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        dt_artworkApDetail.ImportRow(tmp);
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
