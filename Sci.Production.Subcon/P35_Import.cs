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
    public partial class P35_Import : Sci.Win.Subs.Base
    {
        DataRow dr_localAp;
        DataTable dt_localApDetail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        protected DataTable dtlocal;

        public P35_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_localAp = master;
            detail.ColumnsDecimalAdd("Amount", 0);
            dt_localApDetail = detail;
            this.Text += string.Format(" ( Categgory:{0} - Supplier:{1} )", dr_localAp["category"].ToString(), dr_localAp["localsuppid"].ToString());
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            String sp_b = this.txtSPNoStart.Text;
            String sp_e = this.txtSPNoEnd.Text;
            String poid_b = this.txtPOIDStart.Text;
            String poid_e = this.txtPOIDEnd.Text;
            string issuedate_b, issuedate_e, delivery_b, delivery_e;
            issuedate_b = null;
            issuedate_e = null;
            delivery_b = null;
            delivery_e = null;
            

            if (datePOIssueDate.Value1 != null) issuedate_b = this.datePOIssueDate.Text1;
            if (datePOIssueDate.Value2 != null) { issuedate_e = this.datePOIssueDate.Text2; }
            if (dateDelivery.Value1 != null) delivery_b = this.dateDelivery.Text1;
            if (dateDelivery.Value2 != null) { delivery_e = this.dateDelivery.Text2; }

            if ((MyUtility.Check.Empty(issuedate_b) && MyUtility.Check.Empty(issuedate_e)) &&
                (MyUtility.Check.Empty(delivery_b) && MyUtility.Check.Empty(delivery_e ))  &&
                MyUtility.Check.Empty(sp_b) && MyUtility.Check.Empty(sp_e) && MyUtility.Check.Empty(poid_b) && MyUtility.Check.Empty(poid_e))
            {
                MyUtility.Msg.WarningBox("< PO Issue Date > or < Delivery > or < PO ID > or < SP# > can't be empty!!");
                txtSPNoStart.Focus();
                return;
            }
            
            else
            {
                // 建立可以符合回傳的Cursor

                string strSQLCmd = string.Format(@"Select 1 as Selected
                                                                                ,b.id as localpoid
                                                                                , b.orderid
                                                                                ,b.refno
                                                                                ,b.threadcolorid
                                                                                ,[description]=dbo.getItemDesc(a.category,b.refno)
                                                                                ,b.qty as poqty
                                                                                ,b.unitid
                                                                                ,b.price
                                                                                ,b.inqty - apqty unpaid
                                                                                ,b.inqty - apqty as qty  --預帶待付款，[已收數]-[已付款]
                                                                                ,b.ukey localpo_detailukey
                                                                                ,'' id
                                                                                
                                                                                ,0.0 amount
                                                                                ,b.inqty
                                                                                ,b.apqty
                                                                                ,b.inqty - b.apqty AS balance
                                                                        from localpo a WITH (NOLOCK) , localpo_detail b WITH (NOLOCK) 
                                                                        where a.id = b.id and a.status='Approved' and b.apqty < inqty
                                                                        and a.category = '{0}' 
                                                                        and a.localsuppid = '{1}' and a.mdivisionid = '{2}'", dr_localAp["category"],
                                                                                                            dr_localAp["localsuppid"],Env.User.Keyword);
                if(!MyUtility.Check.Empty(sp_b)){strSQLCmd+= " and b.orderid between @sp1 and @sp2";}
                if (!MyUtility.Check.Empty(poid_b)) { strSQLCmd += " and b.id between @localpoid1 and  @localpoid2"; }
                if (!MyUtility.Check.Empty(delivery_b)) { strSQLCmd += string.Format(" and b.Delivery >= '{0}' ", delivery_b); }
                if (!MyUtility.Check.Empty(delivery_e)) { strSQLCmd += string.Format(" and b.Delivery <= '{0}' ", delivery_e); }
                if (!MyUtility.Check.Empty(issuedate_b)) { strSQLCmd += string.Format(" and a.issuedate >= '{0}' ", issuedate_b); }
                if (!MyUtility.Check.Empty(issuedate_e)) { strSQLCmd += string.Format(" and a.issuedate <= '{0}' ", issuedate_e); }
                strSQLCmd += " order by b.id,b.orderid,b.refno,b.threadcolorid";

                #region 準備sql參數資料
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@sp1";
                sp1.Value = sp_b;

                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                sp2.ParameterName = "@sp2";
                sp2.Value = sp_e;

                System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                sp3.ParameterName = "@localpoid1";
                sp3.Value = poid_b;

                System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                sp4.ParameterName = "@localpoid2";
                sp4.Value = poid_e;
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                cmds.Add(sp3);
                cmds.Add(sp4);
                #endregion

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd,cmds, out dtlocal))
                {
                    if (dtlocal.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtlocal;
                    this.gridImport.AutoResizeColumns();
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
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow ddr = gridImport.GetDataRow<DataRow>(e.RowIndex);
                    if ((decimal)e.FormattedValue > (decimal)ddr["unpaid"])
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Qty can't be more than unpaid");
                        return;
                    }

                    ddr["qty"] = e.FormattedValue;
                }
            };


            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("localpoid", header: "local PO", iseditingreadonly: true) //1
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))//2
                 .Text("refno", header: "Refno", iseditingreadonly: true)      //3

                 .Text("threadcolorid", header: "Color Shade", iseditingreadonly: true)//4
                .Text("description", header: "Description", iseditingreadonly: true)//5
                .Numeric("poqty", header: "PO Qty", iseditingreadonly: true)//6
                .Text("Unitid", header: "Unit", iseditingreadonly: true)//7
                .Numeric("Price", header: "PO Price", iseditable: true, decimal_places: 4, integer_places: 4)  //8

                .Numeric("unpaid", header: "UnPaid", iseditingreadonly: true)//9
                .Numeric("qty", header: "Qty", settings: ns);//10


            this.gridImport.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;  //Qty

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            gridImport.ValidateControl();
            
            DataTable dtImport = (DataTable)listControlBindingSource1.DataSource;
            
            if (MyUtility.Check.Empty(dtImport)|| dtImport.Rows.Count == 0) return;
            
            DataRow[] dr2 = dtImport.Select("qty = 0 and Selected = 1");
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
                    
                    DataRow[] findrow = dt_localApDetail.Select(string.Format("localpo_detailukey = '{0}' ", tmp["localpo_detailukey"].ToString()));

                    if (findrow.Length > 0)
                    {
                       
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["qty"] = tmp["qty"];

                    }
                    else
                    {
                        tmp["amount"] = Convert.ToDecimal(tmp["price"]) * Convert.ToDecimal(tmp["qty"]);
                        tmp["id"] = dr_localAp["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        dt_localApDetail.ImportRow(tmp);
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
