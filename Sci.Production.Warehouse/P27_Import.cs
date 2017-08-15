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
    public partial class P27_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtInventory;

        public P27_Import(DataRow master, DataTable detail)
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
            String refno = this.txtRef.Text.TrimEnd();
            String color = this.txtColor.Text.TrimEnd();
            String location = this.txtLocation.Text.TrimEnd();
            String StockType = MyUtility.Convert.GetString(dr_master["StockType"]);

            #region 判斷畫面條件, 至少其中一個有填
            if (string.IsNullOrWhiteSpace(sp) && string.IsNullOrWhiteSpace(refno) && string.IsNullOrWhiteSpace(color) && string.IsNullOrWhiteSpace(location))
            {
                MyUtility.Msg.WarningBox("<SP#> <Ref#> <Color> <Location> can’t be all empty!");
                txtSP.Focus();
                return;
            }
            #endregion

            #region  query sql command
            strSQLCmd.Append(string.Format(@"
select selected = 0
    ,id = ''
    ,PoId = LI.OrderID
    ,LI.Refno
    ,Color = LI.ThreadColorID
    ,L.Description
    ,Qty = case when '{0}' = 'B'then LI.InQty-LI.OutQty+LI.AdjustQty
				when '{0}' = 'O'then LI.LobQty
		   End
	,FromLocation = case when '{0}' = 'B'then LI.ALocation
						 when '{0}' = 'O'then LI.CLocation
					End
	,ToLocation = ''
from LocalInventory LI WITH (NOLOCK)
left join LocalItem L WITH (NOLOCK) on L.RefNo = LI.Refno
where 1=1", StockType));
            #endregion

            #region sql搜尋條件
            if (!MyUtility.Check.Empty(sp))
                strSQLCmd.Append(string.Format(@" and LI.OrderID = '{0}' ", sp));
            if (!MyUtility.Check.Empty(refno))
                strSQLCmd.Append(string.Format(@" and LI.Refno = '{0}' ", refno));
            if (!MyUtility.Check.Empty(color))
                strSQLCmd.Append(string.Format(@" and LI.ThreadColorID = '{0}' ", color));
            if (!MyUtility.Check.Empty(location))
                strSQLCmd.Append(string.Format(@" and '{0}' in (select data from dbo.SplitString(LI.ALocation,','))", location));
            #endregion

            #region Execute
            this.ShowWaitMessage("Data Loading....");
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtInventory))
            {
                if (dtInventory.Rows.Count == 0) MyUtility.Msg.WarningBox("Data not found!!");
                listControlBindingSource1.DataSource = dtInventory;
            }
            else { ShowErr(strSQLCmd.ToString(), result); }
            this.HideWaitMessage();
            #endregion
        }
        //Form Load
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region -- ToLocation 右鍵開窗 --
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = string.Format(@"
select id
       , [Description] 
from   dbo.MtlLocation WITH (NOLOCK) 
where  StockType='B' and junk != '1'");

                    DataRow dr = gridImport.GetDataRow(gridImport.GetSelectedRowIndex());
                    Sci.Win.Tools.SelectItem2 selectSubcons = new Win.Tools.SelectItem2(sqlcmd, "ID,Desc", "13,30", dr["ToLocation"].ToString(), null, null, null);

                    DialogResult result = selectSubcons.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["ToLocation"] = selectSubcons.GetSelectedString();

                    if (!MyUtility.Check.Empty(dr["ToLocation"]))
                        dr["ToLocation"] = string.Join(",", selectSubcons.GetSelectedList().ToArray());
                    else
                        dr["ToLocation"] = "";
                    selectSubcons.Empty();
                }
            };
            #endregion
            #region -- 欄位設定 --
            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("PoId", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) //1
                .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(18)) //2
                .Text("Color", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(8)) //3
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) //4
                .Numeric("QtyBefore", header: "Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //5
                .EditText("FromLocation", header: "FromLocation", iseditingreadonly: true, width: Widths.AnsiChars(10)) //6
                .EditText("ToLocation", header: "ToLocation", iseditingreadonly: true, width: Widths.AnsiChars(20), settings: ts) //7
               ;
            #endregion
            #region 可編輯欄位的顏色
            this.gridImport.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
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

            dr2 = dtGridBS1.Select("Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("PoId = '{0}' and Refno = '{1}' and Color = '{2}' ", tmp["PoId"], tmp["Refno"], tmp["Color"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["Description"] = tmp["Description"];
                    findrow[0]["Qty"] = tmp["Qty"];
                    findrow[0]["FromLocation"] = tmp["FromLocation"];
                    findrow[0]["ToLocation"] = tmp["ToLocation"];
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

        private void txtLocation_Validating(object sender, CancelEventArgs e)
        {
            if (txtLocation.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format(@"
select 1 
where exists(
    select * 
    from    dbo.MtlLocation WITH (NOLOCK) 
    where   StockType='B' 
            and id = '{0}'
            and junk != '1'
)", txtLocation.Text), null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
            }
        }
        //Location  右鍵
        private void txtLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format(@"
select  id
        , [Description] 
from    dbo.MtlLocation WITH (NOLOCK) 
where   StockType='B'
        and junk != '1'"), "13,50", txtLocation.Text, "ID,Desc");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtLocation.Text = item.GetSelectedString();
        }

        private void txtToLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode) return;
            string sqlcmd = string.Format(@"
select id
       , [Description] 
from   dbo.MtlLocation WITH (NOLOCK) 
where  StockType='B' and junk != '1'");

            Sci.Win.Tools.SelectItem2 selectSubcons = new Win.Tools.SelectItem2(sqlcmd,"ID,Desc", "13,30", txtToLocation.Text, null, null, null);

            DialogResult result = selectSubcons.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtToLocation.Text = selectSubcons.GetSelectedString();
            if (!MyUtility.Check.Empty(txtToLocation.Text))
                txtToLocation.Text = string.Join(",", selectSubcons.GetSelectedList().ToArray());
            else
                txtToLocation.Text = "";
            selectSubcons.Empty();
        }

        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            string ToLocation = txtToLocation.Text;

            if (dtInventory == null || dtInventory.Rows.Count == 0) return;
            DataRow[] drfound = dtInventory.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["ToLocation"] = ToLocation;
            }
        }
        //Close
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
