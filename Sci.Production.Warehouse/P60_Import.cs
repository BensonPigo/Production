using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P60_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;

        /// <inheritdoc/>
        // bool flag;
        // string poType;
        protected DataTable dtArtwork;

        /// <inheritdoc/>
        public P60_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            DateTime? issueDate1, issueDate2;
            DateTime? deliveryDate1, deliveryDate2;
            string localpoid = this.txtLocalPO.Text;
            issueDate1 = this.datePOIssueDate.Value1;
            issueDate2 = this.datePOIssueDate.Value2;
            deliveryDate1 = this.dateRangeBuyerDelivery.Value1;
            deliveryDate2 = this.dateRangeBuyerDelivery.Value2;
            string spno1 = this.txtSPNoStart.Text;
            string spno2 = this.txtSPNoEnd.Text;
            string category = this.txtartworktype_ftyCategory.Text;

            if (MyUtility.Check.Empty(localpoid) && MyUtility.Check.Empty(spno1) && MyUtility.Check.Empty(spno2) && MyUtility.Check.Empty(issueDate1) && MyUtility.Check.Empty(deliveryDate1))
            {
                MyUtility.Msg.WarningBox("< Local Po# > < SP# > < Issue Date > < Buyer Delivery > can't be empty at the same time!!");
                this.txtLocalPO.Focus();
                return;
            }

            if (!MyUtility.Check.Empty(spno1) && MyUtility.Check.Empty(spno2))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                this.txtSPNoEnd.Focus();
                return;
            }

            if (MyUtility.Check.Empty(spno1) && !MyUtility.Check.Empty(spno2))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                this.txtSPNoStart.Focus();
                return;
            }

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

            string strSQLCmd = string.Format(
                @"
select  1 as selected 
        , '' id
        , a.id as LocalPoId
        , a.Category
        , b.OrderId 
        , b.Refno
        , b.ThreadColorID
        , dbo.getItemDesc(a.category,b.refno) as [Description]
        , b.Qty as poqty
        , b.Qty - b.InQty as onRoad
        , b.Qty - b.InQty as Qty
        , b.Ukey as localpo_detailUkey
        , b.UnitId
        , b.Price
        , location = ''
        , remark = ''
		, o.BuyerDelivery
from dbo.LocalPO a WITH (NOLOCK) 
inner join dbo.LocalPO_Detail b WITH (NOLOCK) on b.id = a.Id
left JOIN dbo.Orders o  WITH (NOLOCK) on o.ID = b.OrderId
Where b.Qty - b.InQty >0
    and a.status = 'Approved' 
    and a.LocalSuppID = '{0}'", this.dr_master["localsuppid"]);

            if (!MyUtility.Check.Empty(localpoid))
            {
                strSQLCmd += " and a.id = @localpoid";
                sp_localpoid.Value = localpoid;
                cmds.Add(sp_localpoid);
            }

            if (!MyUtility.Check.Empty(issueDate1))
            {
                strSQLCmd += string.Format(
                    @" and a.issuedate between '{0}' and '{1}'",
                    Convert.ToDateTime(issueDate1).ToString("d"), Convert.ToDateTime(issueDate2).ToString("d"));
            }

            if (!MyUtility.Check.Empty(deliveryDate1))
            {
                strSQLCmd += string.Format(
                    @" and o.BuyerDelivery between '{0}' and '{1}'",
                    Convert.ToDateTime(deliveryDate1).ToString("d"), Convert.ToDateTime(deliveryDate2).ToString("d"));
            }

            if (!MyUtility.Check.Empty(spno1))
            {
                strSQLCmd += " and b.orderid >= @spno1 and b.orderid <= @spno2";
                sp_spno1.Value = spno1;
                sp_spno2.Value = spno2;
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(category))
            {
                strSQLCmd += " and a.category = @category";
                sp_category.Value = category;
                cmds.Add(sp_category);
            }

            DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd, cmds, out this.dtArtwork))
            {
                if (this.dtArtwork.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.detailBS.DataSource = this.dtArtwork;
            }
            else
            {
                this.ShowErr(strSQLCmd, result);
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- QTY 不可超過 On Road --

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex());
                    if (decimal.Parse(e.FormattedValue.ToString()) > decimal.Parse(dr["onRoad"].ToString()))
                    {
                         e.Cancel = true;
                         MyUtility.Msg.WarningBox("Qty can't be over on road qty!!");
                    }
                }
            };
            #endregion
            #region Location Setting
            DataGridViewGeneratorTextColumnSettings locationSet = new DataGridViewGeneratorTextColumnSettings();
            locationSet.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    dr["Location"] = e.FormattedValue;
                    string sqlcmd = @"
select * 
from MtlLocation
where	Junk != 1
		and StockType = 'B'";
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["Location"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueLocation.Sort();
                    dr["Location"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                    dr["selected"] = (!string.IsNullOrEmpty(dr["Location"].ToString())) ? 1 : 0;

                    this.gridImport.RefreshEdit();
                }
            };

            locationSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentrow = this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex());
                    Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation("B", currentrow["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    currentrow["location"] = item.GetSelectedString();
                    currentrow.EndEdit();
                }
            };
            #endregion

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.detailBS;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("LocalPoId", header: "Local PO#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("OrderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(12))
                .Text("ThreadColorID", header: "Color Shade", iseditingreadonly: true, width: Widths.AnsiChars(12))
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Numeric("poqty", header: "PO Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))
                .Text("unitid", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Numeric("price", header: "PO Price", decimal_places: 4, integer_places: 8, iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("onRoad", header: "On Road", decimal_places: 2, integer_places: 6, iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("Qty", header: "Qty", decimal_places: 2, integer_places: 6, settings: ns, width: Widths.AnsiChars(6))
                .Text("location", header: "Location", width: Widths.AnsiChars(20), settings: locationSet)
                .Text("remark", header: "Remark", width: Widths.AnsiChars(20))
               ;

            this.gridImport.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["location"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();

            DataTable dtGridBS1 = (DataTable)this.detailBS.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.Select(string.Format(
                    "localpoid = '{0}' and localpo_detailUkey = {1} ",
                    tmp["localpoid"].ToString(), tmp["localpo_detailUkey"].ToString()));

                if (findrow.Length > 0)
                {
                    findrow[0]["onroad"] = tmp["onroad"];
                    findrow[0]["poqty"] = tmp["poqty"];
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["location"] = tmp["location"];
                    findrow[0]["remark"] = tmp["remark"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }
    }
}
