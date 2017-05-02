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
using Sci.Production.PublicPrg;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P24_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtScrap;

        public P24_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.txtSPNo.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                txtSPNo.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor
                #region -- SQL Command --
                strSQLCmd.Append(string.Format(@"
select 	selected = 0   
		, id = '' 
		, FromFtyinventoryUkey = c.ukey
		, fromPoId = a.id
		, fromseq1 = a.Seq1
		, fromseq2 = a.Seq2 
        , fromFactoryID = orders.FactoryID
		, fromseq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
		, Description = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0)
		, fromRoll = c.Roll
		, fromDyelot = c.Dyelot 
		, fromStocktype = c.StockType 
		, balance = c.inqty-c.outqty + c.adjustqty 
		, qty = 0.00 
		, a.FabricType
		, a.stockunit
		, a.InputQty
		, topoid = a.id 
		, toseq1 = a.SEQ1 
		, toseq2 = a.SEQ2 
		, toroll = c.Roll 
		, todyelot = c.Dyelot 
        , toFactoryID = orders.FactoryID
		, toStocktype = 'O' 
		, Fromlocation = dbo.Getlocation(c.ukey)
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
inner join Orders on c.Poid = orders.id
inner join Factory on orders.FactoryID = factory.id
Where c.lock = 0 and c.InQty-c.OutQty+c.AdjustQty > 0 and c.stocktype = 'I'
    and factory.MDivisionID = '{0}'", Sci.Env.User.Keyword));
                #endregion

                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@sp1";

                System.Data.SqlClient.SqlParameter seq1 = new System.Data.SqlClient.SqlParameter();
                seq1.ParameterName = "@seq1";

                System.Data.SqlClient.SqlParameter seq2 = new System.Data.SqlClient.SqlParameter();
                seq2.ParameterName = "@seq2";

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(@" and a.id = @sp1 ");
                    sp1.Value = sp;
                    cmds.Add(sp1);
                }

                if (!txtSeq.checkEmpty(showErrMsg: false))
                {
                    strSQLCmd.Append(@" and a.seq1 = @seq1 and a.seq2 = @seq2");
                    seq1.Value = txtSeq.seq1;
                    seq2.Value = txtSeq.seq2;
                    cmds.Add(seq1);
                    cmds.Add(seq2);
                }

                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), cmds, out dtScrap))
                {
                    if (dtScrap.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    else
                    {
                        dtScrap.DefaultView.Sort = "fromseq1,fromseq2,fromlocation,fromdyelot";
                    }
                    listControlBindingSource1.DataSource = dtScrap;
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                this.HideWaitMessage();
            }
        }
        //Form Load
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- Transfer Qty Valid --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["selected"] = true;
                    }
                };
            #endregion

            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;

            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (gridImport.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
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

            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("frompoid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) //1
                .Text("fromseq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                .Text("fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //3
                .Text("fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //4
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) //5
                .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) //6
                .Numeric("balance", header: "Inventory" + Environment.NewLine + "Qty", iseditable: false, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //7
                .Numeric("Qty", header: "Scrap" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))  //8
                .Text("fromlocation", header: "From Location", width: Widths.AnsiChars(30), iseditingreadonly: true)    //9
               ;

            this.gridImport.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;

        }

        //Close
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
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
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Scrap Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["fromftyinventoryukey"].EqualString(tmp["fromftyinventoryukey"])
                                       && row["topoid"].EqualString(tmp["topoid"].ToString()) && row["toseq1"].EqualString(tmp["toseq1"])
                                       && row["toseq2"].EqualString(tmp["toseq2"].ToString()) && row["toroll"].EqualString(tmp["toroll"])
                                       && row["todyelot"].EqualString(tmp["todyelot"]) && row["tostocktype"].EqualString(tmp["tostocktype"])).ToArray();

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

        //SP# Valid
        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
//            string sp = textBox1.Text.TrimEnd();

//            if (MyUtility.Check.Empty(sp)) return;

//            if (txtSeq1.checkEmpty(showErrMsg: false))
//            {
//                if (!MyUtility.Check.Seek(string.Format(@"
//select 1 where exists(select * 
//                      from ftyinventory WITH (NOLOCK) 
//                      inner join Orders on  ftyinventory.poid = Orders.id
//                      inner join Factory on Orders.factoryID = factory.id
//                      where ftyinventory.poid ='{0}' and factory.MDivisionID = '{1}')"
//                    , sp, Sci.Env.User.Keyword), null))
//                {
//                    MyUtility.Msg.WarningBox("SP# is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//            }
//            else
//            {
//                if (!MyUtility.Check.Seek(string.Format(@"
//select 1 where exists(select * 
//                      from mdivisionpodetail WITH (NOLOCK) 
//                      inner join Orders on mdivisionpodetail.POID = Orders.id
//                      inner join Factory on Orders.Factory = Factory.ID
//                      where ftyinventory.poid ='{0}' and seq1 = '{1}' and seq2 = '{2}'
//                        and Factory.MDivisionID = '{3}')", sp, txtSeq1.seq1, txtSeq1.seq2, Sci.Env.User.Keyword), null))
//                {
//                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
//                    e.Cancel = true;
//                    return;
//                }
//            }

        }

        //Seq Valid
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
//            string sp = textBox1.Text.TrimEnd();
//            if (MyUtility.Check.Empty(sp) || txtSeq1.checkEmpty()) return;

//            if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from po_supp_detail WITH (NOLOCK) where id ='{0}' 
//                        and seq1 = '{1}' and seq2 = '{2}')", sp, txtSeq1.seq1, txtSeq1.seq2), null))
//            {
//                MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
//                e.Cancel = true;
//                return;
//            }

        }

    }
}
