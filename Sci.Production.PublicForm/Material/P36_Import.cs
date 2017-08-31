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
    public partial class P36_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtScrap;

        public P36_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.txtSP.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                txtSP.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor
                #region -- SQL Command --
                strSQLCmd.Append(string.Format(@"
select  selected = 0 
        , id = '' 
        , FromFtyinventoryUkey = c.ukey
        , fromPoId = a.id 
        , fromseq1 = a.Seq1 
        , fromseq2 = a.Seq2
        , fromseq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) 
        , [Description] = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0)
        , fromRoll = c.Roll 
        , fromDyelot = c.Dyelot
        , fromFactoryID = Orders.FactoryID
        , fromStocktype = c.StockType
        , balance = c.inqty - c.outqty + c.adjustqty 
        , qty = 0.00 
        , location = dbo.Getlocation(c.ukey)
        , a.FabricType
        , a.stockunit
        , a.InputQty
        , topoid = a.id 
        , toseq1 = a.SEQ1
        , toseq2 = a.SEQ2
        , toroll = c.Roll
        , todyelot = c.Dyelot
        , toFactoryID = Orders.FactoryID
        , toStocktype = 'I' 
        , tolocation = '' 
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
inner join dbo.Orders on c.Poid = Orders.id
inner join Factory on Orders.FactoryID = Factory.ID
Where   c.lock = 0 
        and c.InQty-c.OutQty+c.AdjustQty > 0 
        and c.stocktype = 'O' 
        and Factory.MDivisionID = '{0}'", Sci.Env.User.Keyword));
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
                    strSQLCmd.Append(@" 
        and a.id = @sp1 ");
                    sp1.Value = sp;
                    cmds.Add(sp1);
                }

                seq1.Value = txtSeq.seq1;
                seq2.Value = txtSeq.seq2;
                cmds.Add(seq1);
                cmds.Add(seq2);
                if (!txtSeq.checkSeq1Empty())
                {
                    strSQLCmd.Append(@"
        and a.seq1 = @seq1");
                }
                if (!txtSeq.checkSeq2Empty())
                {
                    strSQLCmd.Append(@" 
        and a.seq2 = @seq2");
                }

                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), cmds, out dtScrap))
                {
                    if (dtScrap.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    else
                    {
                        dtScrap.DefaultView.Sort = "fromseq1,fromseq2,location,fromdyelot";
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
            #region -- toLocation 右鍵開窗 --

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["tostocktype"].ToString()
                        , gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = gridImport.GetDataRow(e.RowIndex);
                    dr["ToLocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", dr["ToStocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["ToLocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found"); 
                    }
                    trueLocation.Sort();
                    dr["ToLocation"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
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
                .Numeric("balance", header: "Balance" + Environment.NewLine + "Qty", iseditable: false, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //7
                .Numeric("Qty", header: "Transfer" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))  //8
                .Text("tolocation", header: "Location", iseditingreadonly: false, settings: ts2, width: Widths.AnsiChars(20))    //9
               ;

            this.gridImport.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["tolocation"].DefaultCellStyle.BackColor = Color.Pink;
            //this.grid1.Columns[].DefaultCellStyle.BackColor = Color.Pink;
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
                MyUtility.Msg.WarningBox("Transfer Qty of selected row can't be zero!", "Warning");
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
                    findrow[0]["tolocation"] = tmp["tolocation"];
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

        //Update All
        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            //grid1.ValidateControl();
            listControlBindingSource1.EndEdit();

            if (dtScrap == null || dtScrap.Rows.Count == 0) return;
            DataRow[] drfound = dtScrap.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["tolocation"] = displayLocation.Text;
            }
        }

        private void displayLocation_MouseDown(object sender, MouseEventArgs e)
        {
            #region Location 右鍵開窗

            if (this.EditMode && e.Button == MouseButtons.Right)
            {
                Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation("I", displayLocation.Text);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                displayLocation.Text = item.GetSelectedString();
            }

            #endregion Location 右鍵開窗
        }
    }
}
