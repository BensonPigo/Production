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

namespace Sci.Production.Warehouse
{
    public partial class P60_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag;
        string poType;
        protected DataTable dtArtwork;

        public P60_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime? issueDate1, issueDate2;
            String localpoid = this.textBox1.Text;
            issueDate1 = dateRange1.Value1;
            issueDate2 = dateRange1.Value2;
            String spno1 = txtSpno1.Text;
            String spno2 = txtSpno2.Text;
            String category = txtartworktype_fty1.Text;

            if (MyUtility.Check.Empty(localpoid) && MyUtility.Check.Empty(spno1) && MyUtility.Check.Empty(spno2) && MyUtility.Check.Empty(issueDate1))
            {
                MyUtility.Msg.WarningBox("< Local Po# > < SP# > < Issue Date > can't be empty at the same time!!");
                textBox1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                #region -- sql parameters declare --
                System.Data.SqlClient.SqlParameter sp_localpoid = new System.Data.SqlClient.SqlParameter();
                sp_localpoid.ParameterName = "@localpoid";
                
                System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
                sp_spno1.ParameterName = "@spno1";

                System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
                sp_spno2.ParameterName = "@spno2";

                System.Data.SqlClient.SqlParameter sp_category = new System.Data.SqlClient.SqlParameter();
                sp_category.ParameterName = "@category";

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                #endregion

                string strSQLCmd = string.Format(@"select 0 as selected ,'' id, a.id as LocalPoId
,a.Category
,b.OrderId 
,b.Refno
,b.ThreadColorID
,dbo.getItemDesc(a.category,b.refno) as [Description]
,b.Qty as poqty
,b.Qty - b.InQty as onRoad
,b.Qty - b.InQty as Qty
,b.Ukey as localpo_detailUkey
,b.UnitId
,b.Price
from dbo.LocalPO a inner join dbo.LocalPO_Detail b on b.id = a.Id
Where b.Qty - b.InQty >0
and mdivisionid = '{0}' and a.status = 'Approved' and a.LocalSuppID = '{1}'", Sci.Env.User.Keyword,dr_master["localsuppid"]);

                if (!MyUtility.Check.Empty(localpoid))
                {
                    strSQLCmd += " and a.id = @localpoid";
                    sp_localpoid.Value = localpoid;
                    cmds.Add(sp_localpoid);
                }

                if (!MyUtility.Check.Empty(issueDate1))
                {
                    strSQLCmd += string.Format(@" and a.issuedate between '{0}' and '{1}'",
                    Convert.ToDateTime(issueDate1).ToString("d"), Convert.ToDateTime(issueDate2).ToString("d"));
                }
                if (!MyUtility.Check.Empty(spno1))
                {
                    strSQLCmd +=" and b.orderid >= @spno1 and b.orderid <= @spno2";
                    sp_spno1.Value = spno1;
                    sp_spno2.Value = spno2;
                    cmds.Add(sp_spno1);
                    cmds.Add(sp_spno2);
                }
                if (!MyUtility.Check.Empty(category))
                {
                    strSQLCmd +=" and a.category = @category";
                    sp_category.Value = category;
                    cmds.Add(sp_category);
                }


                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd,cmds, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    detailBS.DataSource = dtArtwork;
                }
                else { ShowErr(strSQLCmd, result); }
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- QTY 不可超過 On Road --

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = grid1.GetDataRow(grid1.GetSelectedRowIndex());
                    if (decimal.Parse(e.FormattedValue.ToString()) > decimal.Parse(dr["onRoad"].ToString()))
                    {
                        MyUtility.Msg.WarningBox("Qty can't be over on road qty!!");
                        e.Cancel = true;
                    }
                }
            };
            #endregion 

            Ict.Win.UI.DataGridViewNumericBoxColumn col_qty = new Ict.Win.UI.DataGridViewNumericBoxColumn();
            Ict.Win.UI.DataGridViewTextBoxColumn col_remark = new Ict.Win.UI.DataGridViewTextBoxColumn();

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = detailBS;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("OrderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(12))
                .Text("ThreadColorID", header: "Color Shade", iseditingreadonly: true, width: Widths.AnsiChars(12))
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Numeric("poqty", header: "PO Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))
                .Text("unitid", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Numeric("price", header: "PO Price", decimal_places: 4, integer_places: 8, iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("onRoad", header: "On Road", decimal_places: 2, integer_places: 6, iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("Qty", header: "Qty", decimal_places: 2, integer_places: 6, settings: ns, width: Widths.AnsiChars(6)).Get(out col_qty)
                .Text("remark", header: "Remark", width:Widths.AnsiChars(20)).Get(out col_remark)
               ; 

            col_qty.DefaultCellStyle.BackColor = Color.Pink;
            col_remark.DefaultCellStyle.BackColor = Color.Pink;

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

            grid1.ValidateControl();

            DataTable dtGridBS1 = (DataTable)detailBS.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0 )
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("localpoid = '{0}' and localpo_detailUkey = {1} "
                    , tmp["localpoid"].ToString(), tmp["localpo_detailUkey"].ToString()));

                if (findrow.Length > 0)
                {
                    findrow[0]["onroad"] = tmp["onroad"];
                    findrow[0]["poqty"] = tmp["poqty"];
                    findrow[0]["qty"] = tmp["qty"];
                }
                else
                {
                    tmp["id"] = dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }

            
            this.Close();
        }
    }
}
