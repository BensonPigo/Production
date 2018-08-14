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
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P15_Import : Sci.Win.Subs.Base
    {
        public Sci.Win.Tems.Base P15;
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
       // bool flag;
       // string poType;
        protected DataTable dtlack;
        string Type;
        public P15_Import(DataRow master, DataTable detail,string type, string title)
        {
            this.Text = title.ToString();
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            Type = type;
        }

        private void sum_checkedqty()
        {
            listControlBindingSource1.EndEdit();
            Object localPrice = dtlack.Compute("Sum(qty)", "selected = 1");
            this.displayTotalQty.Value = localPrice.ToString();
        }


        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder strSQLCmd = new StringBuilder();

            #region -- 抓lack的資料 --
            //grid1
            strSQLCmd.Append(string.Format(@"
select  selected = 0
        , id = ''
        ,poid = rtrim(a.POID) 
	    , b.seq1
	    , b.seq2
	    , seq = concat(Ltrim(Rtrim(b.seq1)), ' ', b.Seq2)
	    , [description] = dbo.getMtlDesc(a.poid,b.seq1,b.seq2,2,0)
	    , b.RequestQty
        , Stock = c.inqty - c.outqty + c.adjustqty
        , location = dbo.Getlocation(c.ukey)
        , Qty = 0.00
        , issueqty = 0
        , ftyinventoryukey = c.ukey
        , StockType = 'B' 
        , stockunit = (select stockunit 
	   				  from po_supp_detail WITH (NOLOCK) 
	   				  where id = c.poid 
	   				  		and seq1 = c.seq1 
	   				  		and seq2 = c.seq2)
from dbo.lack a WITH (NOLOCK) 
inner join dbo.Lack_Detail b WITH (NOLOCK) on a.ID = b.ID
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.POID 
											   and c.seq1 = b.seq1 
											   and c.seq2  = b.seq2 
											   and c.stocktype = 'B'
where a.id = '{0}' and c.lock = 0 ", dr_master["requestid"]));
            strSQLCmd.Append(Environment.NewLine); // 換行

           //判斷LACKING
            //
            if (Type != "Lacking")
            { strSQLCmd.Append(" and (c.inqty-c.outqty + c.adjustqty) > 0"); }
           // string AA = strSQLCmd.ToString();
            #endregion

            P15.ShowWaitMessage("Data Loading....");

            DataTable data;
            DBProxy.Current.DefaultTimeout = 1200;
            try
            {
                if (!SQL.Select("", strSQLCmd.ToString(), out data))
                {
                    ShowErr(strSQLCmd.ToString());
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DBProxy.Current.DefaultTimeout = 0;               
            }

            P15.HideWaitMessage();
            dtlack = data;

            if (dtlack.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            #region -- Grid1 Setting --

            this.gridlack.CellValueChanged += (s, e) =>
            {
                if (gridlack.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = gridlack.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balance"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }
                    dr.EndEdit();
                    this.sum_checkedqty();
                }
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    gridlack.GetDataRow(e.RowIndex)["qty"] = e.FormattedValue;
                    if (Type != "Lacking")
                    {
                        if ((decimal)e.FormattedValue > (decimal)gridlack.GetDataRow(e.RowIndex)["Stock"])
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Issue qty can't be more than Stock qty!!");
                            return;
                        }
                    }
                    gridlack.GetDataRow(e.RowIndex)["selected"] = true;
                    this.sum_checkedqty();
                }
            };
            
            dtlack.Columns.Add("balance", typeof(decimal), "RequestQty - qty");

            this.gridlack.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            listControlBindingSource1.DataSource = dtlack;
            this.gridlack.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridlack)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("Poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) //2
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //3
                .Numeric("Stock", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(8)) //3
                .Numeric("RequestQty", header: "Request Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(8)) //4
                .Numeric("qty", header: "Issue Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(8))  //4
                .Numeric("balance", header: "Balance Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(8)) //6
                .Text("location", header: "Bulk Location", iseditingreadonly: true)      //5
                ;
            this.gridlack.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            gridlack.ValidateControl();
            if (MyUtility.Check.Empty(dtlack) || dtlack.Rows.Count == 0) return;

            DataRow[] dr2 = dtlack.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtlack.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtlack.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow;

                //判斷為P15(副料)呼叫還是P15(主料)呼叫
                findrow = dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                      && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"])
                                                      && row["seq2"].EqualString(tmp["seq2"].ToString())).ToArray();
                

                if (findrow.Length > 0)
                {
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
