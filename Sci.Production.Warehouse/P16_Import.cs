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
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P16_Import : Sci.Win.Subs.Base
    {
        public Sci.Win.Tems.Base P16;
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
       // bool flag;
       // string poType;
        protected DataTable dtlack, dtftyinventory;
        string Type;
        public P16_Import(DataRow master, DataTable detail,string type, string title)
        {
            this.Text = title.ToString();
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            Type = type;
        }

        private void sum_checkedqty()
        {
            listControlBindingSource2.EndEdit();
            Object localPrice = dtftyinventory.Compute("Sum(qty)", "selected = 1");
            this.displayTotalQty.Value = localPrice.ToString();
        }

        private void grid1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            gridLack_Detail.ValidateControl();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder strSQLCmd = new StringBuilder();

            #region -- 抓lack的資料 --
            //grid1
            strSQLCmd.Append(string.Format(@"
select poid = rtrim(a.POID) 
	   , b.seq1
	   , b.seq2
	   , seq = concat(Ltrim(Rtrim(b.seq1)), ' ', b.Seq2)
	   , [description] = dbo.getMtlDesc(a.poid,b.seq1,b.seq2,2,0)
	   , b.RequestQty
from dbo.lack a WITH (NOLOCK) 
inner join dbo.Lack_Detail b WITH (NOLOCK) on a.ID = b.ID
LEFT JOIN Orders o ON o.ID = a.POID
where a.id = '{0}'
AND o.Category <> 'A';
", dr_master["requestid"]));
            strSQLCmd.Append(Environment.NewLine); // 換行

            //grid2
            strSQLCmd.Append(string.Format(@"
select selected = 0
	   , id = '' 
	   , poid = rtrim(c.PoId) 
	   , c.Seq1
	   , c.Seq2
	   , seq = concat(Ltrim(Rtrim(c.seq1)), ' ', c.Seq2)
	   , Roll = Rtrim(Ltrim(c.Roll))
	   , c.Dyelot
	   , Qty = 0.00
	   , StockType = 'B' 
	   , ftyinventoryukey = c.ukey
	   , location = dbo.Getlocation(c.ukey)
	   , balance = c.inqty - c.outqty + c.adjustqty
	   , stockunit = (select stockunit 
	   				  from po_supp_detail WITH (NOLOCK) 
	   				  where id = c.poid 
	   				  		and seq1 = c.seq1 
	   				  		and seq2 = c.seq2)
	   , [description] = dbo.getMtlDesc(c.poid,c.seq1,c.seq2,2,0) 
from dbo.Lack_Detail a WITH (NOLOCK) 
inner join dbo.Lack b WITH (NOLOCK) on b.ID = a.ID
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = b.POID 
											   and c.seq1 = a.seq1 
											   and c.seq2  = a.seq2 
											   and c.stocktype = 'B'
LEFT JOIN Orders o ON o.ID=c.PoId
Where a.id = '{0}' 
	  and c.lock = 0 
	  AND o.Category!='A'
", dr_master["requestid"])); // 
           //判斷LACKING
            //
            if (Type != "Lacking")
            { strSQLCmd.Append(" and (c.inqty-c.outqty + c.adjustqty) > 0"); }
           // string AA = strSQLCmd.ToString();
            #endregion

            P16.ShowWaitMessage("Data Loading....");

            DataSet data;
            DBProxy.Current.DefaultTimeout = 1200;
            try
            {
                if (!SQL.Selects("", strSQLCmd.ToString(), out data))
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

            P16.HideWaitMessage();
            dtlack = data.Tables[0];
            dtftyinventory = data.Tables[1];

            if (dtlack.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            dtlack.TableName = "dtlack";
            dtftyinventory.TableName = "dtftyinventory";

            DataRelation relation = new DataRelation("rel1"
                , new DataColumn[] { dtlack.Columns["poid"], dtlack.Columns["seq"] }
                , new DataColumn[] { dtftyinventory.Columns["poid"], dtftyinventory.Columns["seq"] }
                );
            data.Relations.Add(relation);

            dtlack.Columns.Add("issueqty", typeof(decimal), "sum(child.qty)");
            dtlack.Columns.Add("balance", typeof(decimal), "RequestQty - issueqty");

            listControlBindingSource1.DataSource = data;
            listControlBindingSource1.DataMember = "dtlack";
            listControlBindingSource2.DataSource = listControlBindingSource1;
            listControlBindingSource2.DataMember = "rel1";

            #region -- Grid1 Setting --
            this.gridlack.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridlack.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridlack)
                .Text("Poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) //2
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //3
                .Numeric("RequestQty", header: "Request Qty", iseditable: true, decimal_places: 2, integer_places: 10) //4
                .Numeric("Issueqty", header: "Accu. Issue Qty", decimal_places: 2, integer_places: 10)  //5
                .Numeric("balance", header: "Balance Qty", iseditable: true, decimal_places: 2, integer_places: 10) //6
                ;
            #endregion
            #region --  Grid2 Setting --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        gridLack_Detail.GetDataRow(e.RowIndex)["qty"] = e.FormattedValue;
                        if (Type != "Lacking")
                        {
                            if ((decimal)e.FormattedValue > (decimal)gridLack_Detail.GetDataRow(e.RowIndex)["balance"])
                            {
                               e.Cancel = true;
                               MyUtility.Msg.WarningBox("Issue qty can't be more than Stock qty!!");
                               return;
                            }
                        }
                        gridLack_Detail.GetDataRow(e.RowIndex)["selected"] = true;
                        this.sum_checkedqty();
                    }
                };

            this.gridLack_Detail.CellValueChanged += (s, e) =>
            {
                if (gridLack_Detail.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = gridLack_Detail.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balance"];
                    }else if (Convert.ToBoolean(dr["selected"]) == false){
                        dr["qty"] = 0;
                    }
                    dr.EndEdit();
                    this.sum_checkedqty();
                }
            };

            this.gridLack_Detail.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridLack_Detail.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.gridLack_Detail)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) //1
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) //3
                .Numeric("qty", header: "Issue Qty", decimal_places: 2, integer_places: 10, settings: ns)  //4
                .Text("location", header: "Bulk Location", iseditingreadonly: true)      //5
                ;
            this.gridLack_Detail.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            gridLack_Detail.ValidateControl();
            if (MyUtility.Check.Empty(dtftyinventory) || dtftyinventory.Rows.Count == 0) return;

            DataRow[] dr2 = dtftyinventory.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtftyinventory.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtftyinventory.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow;

                //判斷為P15(副料)呼叫還是P16(主料)呼叫
                if (this.Text.ToString().Contains("P15"))
                {
                    findrow = dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                      && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"])
                                                      && row["seq2"].EqualString(tmp["seq2"].ToString())).ToArray();
                }
                else
                {
                    findrow = dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                          && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"])
                                                                          && row["seq2"].EqualString(tmp["seq2"].ToString()) && row["roll"].EqualString(tmp["roll"])
                                                                          && row["dyelot"].EqualString(tmp["dyelot"])).ToArray();
                }

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
