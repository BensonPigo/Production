using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Sci.Production.Warehouse
{
    public partial class P39 : Win.Tems.Input6
    {
        public P39(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // 寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select ALD.id
    ,ALD.PoId
    ,ALD.Refno
    ,ALD.Color
    ,L.Description
    ,ALD.QtyBefore
    ,ALD.QtyAfter
    ,AdjustLocalqty = isnull(ALD.QtyAfter,0.00) - isnull(ALD.QtyBefore,0.00) 
    ,LI.ALocation
    ,ALD.ReasonId
    ,reason_nm = r.Name
    ,ALD.StockType
from dbo.AdjustLocal_Detail ALD WITH (NOLOCK) 
left join LocalInventory LI WITH (NOLOCK) on LI.OrderID = ALD.PoId and LI.Refno = ALD.Refno and LI.ThreadColorID = ALD.Color
left join LocalItem L WITH (NOLOCK) on L.RefNo = LI.Refno
left join Reason r WITH (NOLOCK) on r.ID = ALD.ReasonId AND r.ReasonTypeID = 'Stock_Adjust'
Where ALD.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region --Vaild Current Qty 不可等於Original qty 且 Adjust Qty = Current Qty -Original Qty
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                decimal qtybefore = MyUtility.Convert.GetDecimal(this.CurrentDetailData["qtybefore"]);
                decimal qtyafterOld = MyUtility.Convert.GetDecimal(this.CurrentDetailData["qtyafter"]);
                decimal qtyafterNew = MyUtility.Convert.GetDecimal(e.FormattedValue);
                if (qtyafterOld == qtyafterNew)
                {
                    return;
                }

                if (qtybefore == qtyafterNew)
                {
                    MyUtility.Msg.WarningBox("Current Qty cant' equal Original Qty!");
                    e.Cancel = true;
                    return;
                }

                this.CurrentDetailData["qtyafter"] = qtyafterNew;
                this.CurrentDetailData["AdjustLocalqty"] = qtyafterNew - qtybefore;
            };
            #endregion

            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
                    DualResult result = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result)
                    {
                        this.ShowErr(sqlcmd, result);
                        return;
                    }

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(poitems, "ID,Name", "5,150", this.CurrentDetailData["reasonid"].ToString(), "ID,Name");
                    DialogResult result2 = item.ShowDialog();
                    if (result2 == DialogResult.Cancel)
                    {
                        return;
                    }

                    IList<DataRow> x = item.GetSelecteds();
                    this.CurrentDetailData["reasonid"] = x[0]["id"];
                    this.CurrentDetailData["reason_nm"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr;
                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["reasonid"] = string.Empty;
                        this.CurrentDetailData["reason_nm"] = string.Empty;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(string.Format(@"select Name from Reason WITH (NOLOCK) where id = '{0}' and ReasonTypeID='Stock_Adjust' AND junk = 0", e.FormattedValue), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            this.CurrentDetailData["reasonid"] = e.FormattedValue;
                            this.CurrentDetailData["reason_nm"] = dr["name"];
                        }
                    }
                }
            };
            #endregion Seq 右鍵開窗

            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(13), iseditingreadonly: true) // 1
            .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true) // 3
            .Numeric("qtybefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 4
            .Numeric("qtyafter", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns) // 5
            .Numeric("AdjustLocalqty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 6
            .Text("ALocation", header: "Location", iseditingreadonly: true) // 7
            .Text("reasonid", header: "Reason ID", settings: ts) // 8
            .Text("reason_nm", header: "Reason Name", width: Widths.AnsiChars(15), iseditingreadonly: true) // 10
            ;
            #endregion 欄位設定
            #region 可編輯欄位的顏色
            this.detailgrid.Columns["qtyafter"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label25.Text = this.CurrentMaintain["status"].ToString();
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "A";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            #region 檢查明細至少存在一筆資料
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }
            #endregion

            #region 全部檢查完再Message有問題的detail資料。
            StringBuilder warningmsg = new StringBuilder();
            foreach (DataRow row in this.DetailDatas)
            {
                // 檢查所有明細資料的current qty 不可等於 original qty
                if (MyUtility.Convert.GetDecimal(row["qtybefore"]) == MyUtility.Convert.GetDecimal(row["qtyafter"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Refno#: {1} Color: {2} Original Qty and Current Qty can't be equal!!",
                        row["poid"], row["Refno"], row["Color"]) + Environment.NewLine);
                }

                // 檢查所有明細資料都有填入reason
                if (MyUtility.Check.Empty(row["reasonid"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Refno#: {1} Color: {2} Reason can't be empty!!",
                        row["poid"], row["Refno"], row["Color"]) + Environment.NewLine);
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }
            #endregion

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "LB", "AdjustLocal", (DateTime)this.CurrentMaintain["Issuedate"], 2, "ID", null);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            DualResult result;

            #region 每一筆檢查庫存是否足夠
            StringBuilder warningmsg = new StringBuilder();
            DataTable datacheck;
            string sqlcmd = string.Format(
                @"
select ALD.PoId,ALD.Refno,ALD.Color,balance = (LI.InQty-LI.OutQty+LI.AdjustQty)+(ALD.QtyAfter-ALD.QtyBefore),LI.AdjustQty
from AdjustLocal_Detail ALD WITH (NOLOCK) 
left join LocalInventory LI WITH (NOLOCK) on LI.OrderID = ALD.PoId and LI.Refno = ALD.Refno and LI.ThreadColorID = ALD.Color
where ALD.Id = '{0}' and (LI.InQty-LI.OutQty+LI.AdjustQty)+(ALD.QtyAfter-ALD.QtyBefore) < 0", this.CurrentMaintain["id"].ToString());
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow chkdr in datacheck.Rows)
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Refno#: {1} Color: {2}'s balance: {3} is less than Adjust qty: {4} Balacne Qty is not enough!!
",
                        chkdr["PoId"].ToString(), chkdr["Refno"].ToString(), chkdr["Color"].ToString(), chkdr["balance"].ToString(), chkdr["AdjustQty"].ToString()));
                }

                this.ShowErr(warningmsg.ToString());
                return;
            }
            #endregion

            #region Update LocalInventory set AdjustQty= AdjustQty-( AdjustLocal_detail.QtyAfter- QtyBefore)
            string sqlupd3 = string.Format(
                @"
update LI
set LI.AdjustQty = LI.AdjustQty +(ALD.QtyAfter- ALD.QtyBefore)
from AdjustLocal_Detail ALD 
left join LocalInventory LI on LI.OrderID = ALD.PoId and LI.Refno = ALD.Refno and LI.ThreadColorID = ALD.Color
where ALD.Id = '{0}' and (LI.InQty-LI.OutQty+LI.AdjustQty)+(ALD.QtyAfter-ALD.QtyBefore)>=0;

update AdjustLocal set status='Confirmed' where id = '{0}'",
                this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
            {
                this.ShowErr(sqlupd3, result);
                return;
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
            #endregion
        }

        // Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            DualResult result;

            #region 每一筆檢查庫存是否足夠
            StringBuilder warningmsg = new StringBuilder();
            DataTable datacheck;
            string sqlcmd = string.Format(
                @"
select ALD.PoId,ALD.Refno,ALD.Color,balance = (LI.InQty-LI.OutQty+LI.AdjustQty)+(ALD.QtyAfter-ALD.QtyBefore),LI.AdjustQty
from AdjustLocal_Detail ALD WITH (NOLOCK) 
left join LocalInventory LI WITH (NOLOCK) on LI.OrderID = ALD.PoId and LI.Refno = ALD.Refno and LI.ThreadColorID = ALD.Color
where ALD.Id = '{0}' and (LI.InQty-LI.OutQty+LI.AdjustQty)-(ALD.QtyAfter-ALD.QtyBefore) < 0", this.CurrentMaintain["id"].ToString());
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow chkdr in datacheck.Rows)
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Refno#: {1} Color: {2}'s balance: {3} is less than Adjust qty: {4} Balacne Qty is not enough!!\r\n",
                        chkdr["PoId"].ToString(), chkdr["Refno"].ToString(), chkdr["Color"].ToString(), chkdr["balance"].ToString(), chkdr["AdjustQty"].ToString()));
                }

                this.ShowErr(warningmsg.ToString());
                return;
            }
            #endregion

            #region Update LocalInventory set AdjustQty= AdjustQty-( AdjustLocal_detail.QtyAfter- QtyBefore)
            string sqlupd3 = string.Format(
                @"
update LI
set LI.AdjustQty = LI.AdjustQty -(ALD.QtyAfter- ALD.QtyBefore)
from AdjustLocal_Detail ALD 
left join LocalInventory LI on LI.OrderID = ALD.PoId and LI.Refno = ALD.Refno and LI.ThreadColorID = ALD.Color
where ALD.Id = '{0}' and (LI.InQty-LI.OutQty+LI.AdjustQty)-(ALD.QtyAfter-ALD.QtyBefore)>=0;

update AdjustLocal set status='New' where id = '{0}'",
                this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
            {
                this.ShowErr(sqlupd3, result);
                return;
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
            #endregion

        }

        // Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new P39_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Find
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("poid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }
    }
}
