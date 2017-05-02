﻿using System;
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
    public partial class P12_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        //bool flag;
       // string poType;
        protected DataTable dtArtwork;

        public P12_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            String sp_b = this.txtSPNo.Text;

            if (string.IsNullOrWhiteSpace(sp_b))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                txtSPNo.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                string strSQLCmd = string.Format(@"
select  selected = 0
        , id = '' 
        , PoId = a.id 
        , a.Seq1
        , a.Seq2
        , seq = concat(a.seq1, ' ', a.Seq2)
        , a.FabricType
        , a.stockunit
        , [Description] = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) 
        , Roll = '' 
        , Dyelot = '' 
        , Qty = 0.00 
        , StockType = 'B' 
        , ftyinventoryukey = c.ukey 
        , location = dbo.Getlocation(c.ukey) 
        , balance = c.inqty-c.outqty + c.adjustqty 
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'B'
inner join Orders on c.poid = orders.id
inner join factory on orders.factoryID = factory.id
inner join fabric WITH (NOLOCK) on fabric.scirefno = a.scirefno
inner join mtltype WITH (NOLOCK) on mtltype.id = fabric.mtltypeid
Where a.id = '{0}' and c.lock = 0 and c.inqty-c.outqty + c.adjustqty > 0 
    and upper(dbo.mtltype.Issuetype) = 'PACKING' and factory.MDivisionID = '{1}'
", sp_b, Sci.Env.User.Keyword);

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

        protected override void OnFormLoaded()
        {

            base.OnFormLoaded();
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating = (s, e) =>
            {
                
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow temp = dtArtwork.Rows[e.RowIndex];
                    temp["qty"] = e.FormattedValue;
                    if (Convert.ToDecimal(e.FormattedValue) > 0)
                        temp["selected"] = true;
                }
            };

            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (gridImport.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["Selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balance"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }
                    dr.EndEdit();
                }
            };

            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("location", header: "Bulk Location", iseditingreadonly: true)      //2
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //3
                .Numeric("balance", header: "Stock Qty", iseditable: true, decimal_places: 2, integer_places: 10) //4
                .Numeric("qty", header: "Issue Qty", decimal_places: 2, integer_places: 10 ,settings: ns)  //5
               .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(40)); //6

            this.gridImport.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnImport_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
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
            dr2 = dtGridBS1.Select("qty > balance");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't more than Stock Qty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}'"
                    , tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString()));

                if (findrow.Length > 0)
                {
                    //findrow[0]["unitprice"] = tmp["unitprice"];
                    //findrow[0]["Price"] = tmp["Price"];
                    //findrow[0]["amount"] = tmp["amount"];
                    //findrow[0]["poqty"] = tmp["poqty"];
                    //findrow[0]["qtygarment"] = 1;
                    findrow[0]["Qty"] = tmp["Qty"];
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
