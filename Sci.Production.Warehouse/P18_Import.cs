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
    public partial class P18_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtImportData;

        public P18_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
            dr_master = master;
            dt_detail = detail;
            this.EditMode = true;
        }

        //Button Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String transid = this.txtTransferOutID.Text.TrimEnd();


            if (string.IsNullOrWhiteSpace(transid))
            {
                MyUtility.Msg.WarningBox("< Transaction ID# > can't be empty!!");
                txtTransferOutID.Focus();
                return;
            }
            strSQLCmd.Append(string.Format(@"select 0 as selected,null as ukey,'' as id
,b.POID,b.seq1,b.seq2
,concat(Ltrim(Rtrim(b.seq1)), ' ', b.Seq2) as seq
,b.Roll,b.Dyelot,b.StockType,b.Qty 
,dbo.getmtldesc(b.poid,b.seq1,b.seq2,2,0) [description]
,'' location
,psd.StockUnit
from TransferOut a WITH (NOLOCK) 
inner join TransferOut_Detail b WITH (NOLOCK) on b.id = a.id
inner join PO_Supp_Detail psd WITH (NOLOCK) on b.POID = psd.id and b.Seq1 = psd.Seq1 and b.Seq2 = psd.Seq2
where a.status='Confirmed' and a.id='{0}'", transid)); // 


            this.ShowWaitMessage("Data Loading....");
            Ict.DualResult result;
            if (!(result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtImportData)))
            {
                ShowErr(strSQLCmd.ToString(), result);
            }
            else
            {
                if (dtImportData.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }

                listControlBindingSource1.DataSource = dtImportData;
                dtImportData.DefaultView.Sort = "poid,seq1,seq2,dyelot,roll";
            }
            this.HideWaitMessage();

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Location 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentrow = grid1.GetDataRow(grid1.GetSelectedRowIndex());
                    Sci.Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation(currentrow["Stocktype"].ToString(), currentrow["Location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    currentrow["Location"] = item.GetSelectedString();
                }
            };
            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = grid1.GetDataRow(e.RowIndex);
                    dr["location"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WITH (NOLOCK) WHERE StockType='{0}'", dr["stocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["location"].ToString().Split(',').Distinct().ToArray();
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
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }
                    trueLocation.Sort();
                    dr["location"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion Location 右鍵開窗

            #region StockType setting
            Ict.Win.DataGridViewGeneratorComboBoxColumnSettings sk = new DataGridViewGeneratorComboBoxColumnSettings();
            sk.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow CurrentDetailData = grid1.GetDataRow(e.RowIndex);
                    CurrentDetailData["stocktype"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WITH (NOLOCK) WHERE StockType='{0}'", CurrentDetailData["stocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
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
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }
                    trueLocation.Sort();
                    CurrentDetailData["location"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.UI.DataGridViewTextBoxColumn txt_roll;
            Ict.Win.UI.DataGridViewTextBoxColumn txt_location;

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //1
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: false).Get(out txt_roll)    //3
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: true)    //4
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)    //5
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //6
            .ComboBox("stocktype", header: "Stock Type", iseditable: true, settings : sk).Get(out cbb_stocktype)//7
            .Text("Location", header: "Location", settings: ts2, iseditingreadonly: false).Get(out txt_location)    //8
            ;

            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            cbb_stocktype.DefaultCellStyle.BackColor = Color.Pink;
            txt_location.DefaultCellStyle.BackColor = Color.Pink;
            txt_roll.DefaultCellStyle.BackColor = Color.Pink;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            grid1.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                         && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"])
                                                                         && row["seq2"].EqualString(tmp["seq2"].ToString()) && row["roll"].EqualString(tmp["roll"])
                                                                         && row["dyelot"].EqualString(tmp["dyelot"]) && row["stockType"].EqualString(tmp["stockType"])).ToArray();
                if (findrow.Length > 0)
                {
                    findrow[0]["roll"] = tmp["roll"];
                    findrow[0]["location"] = tmp["location"];
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

        private void txtTransferOutID_Validating(object sender, CancelEventArgs e)
        {
            //if (MyUtility.Check.Empty(txtTransferOutID.Text))
            //    return;
            //if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from dbo.transferout WITH (NOLOCK) where status='Confirmed' and id ='{0}')"
            //        , this.txtTransferOutID.Text), null))
            //{
            //    MyUtility.Msg.WarningBox("< Transfer out ID > is not found!!");
            //    e.Cancel = true;
            //    return;
            //}
        }
    }
}
