using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P46 : Win.Tems.Input6
    {
        public P46(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = string.Format("Type='R' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "R";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // 刪除前檢查
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
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

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Convert.GetDecimal(row["QtyAfter"]) == MyUtility.Convert.GetDecimal(row["QtyBefore"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Refno#: {1} Color: {2} 
Original Qty and Current Qty can't be equal!!",
                        row["poid"].ToString().Trim(), row["Refno"].ToString().Trim(), row["Color"].ToString().Trim()) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["reasonid"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Refno#: {1} Color: {2} ",
                        row["poid"], row["Refno"], row["Color"]) + Environment.NewLine + "Reason can't be empty!!" + Environment.NewLine);
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            // P46存檔,塞值 AdjustLocal_Detail.StockType='R' , MdivisionID
            DualResult result;
            string sqlUpd = string.Format(
                @"
update AdjustLocal_detail
set StockType='R',
MDivisionID='{0}'
where id='{1}'", Sci.Env.User.Keyword, this.CurrentMaintain["ID"]);
            if (!(result = DBProxy.Current.Execute(null, sqlUpd)))
            {
                this.ShowErr(sqlUpd, result);
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "LM", "AdjustLocal", (DateTime)this.CurrentMaintain["Issuedate"], 2, "ID", null);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // 表身資料SQL Command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select 
AL2.id,
AL2.POID,
AL2.Refno,
AL2.Color,
AL2.StockType,
AL2.MDivisionID,
Litem.Description,
AL2.QtyBefore,
AL2.QtyAfter,
[RemoveQty] = AL2.QtyBefore - AL2.QtyAfter,
Linv.CLocation,
AL2.ReasonId,
[reason_nm]=Reason.Name,
AL2.Ukey
from AdjustLocal_Detail AL2
outer apply (select Description from LocalItem where refno=AL2.Refno) Litem
outer apply (select CLocation from LocalInventory where OrderID=AL2.POID and Refno=AL2.Refno and ThreadColorID=AL2.Color) Linv
outer apply(select Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND ID= AL2.ReasonId and junk=0 ) Reason
where AL2.Id='{0}' ", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // 表身資料設定
        protected override void OnDetailGridSetup()
        {
            #region -- Current Qty Vaild 判斷 --

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();

            ns.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode && (MyUtility.Convert.GetDecimal(e.FormattedValue) >= MyUtility.Convert.GetDecimal(this.CurrentDetailData["QtyBefore"])))
                {
                    MyUtility.Msg.WarningBox("Current Qty cannot >= Original Qty!");
                    e.Cancel = true;
                    return;
                }

                if (this.EditMode && string.Compare(this.CurrentDetailData["qtyafter"].ToString(), e.FormattedValue.ToString()) != 0)
                {
                    this.CurrentDetailData["qtyafter"] = e.FormattedValue;
                    this.CurrentDetailData["RemoveQty"] = (decimal)this.CurrentDetailData["qtybefore"] - (decimal)e.FormattedValue;
                }
            };

            #endregion
            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                        poitems,
                        "ID,Name",
                        "5,150",
                        this.CurrentDetailData["reasonid"].ToString(),
                        "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = item.GetSelecteds();

                    this.CurrentDetailData["reasonid"] = x[0]["id"];
                    this.CurrentDetailData["reason_nm"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["reasonid"] = string.Empty;
                        this.CurrentDetailData["reason_nm"] = string.Empty;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            @"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Remove' AND junk = 0", e.FormattedValue), out dr, null))
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
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true) // 0
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true) // 1
            .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true) // 2
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 3
            .Numeric("QtyBefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 4
            .Numeric("QtyAfter", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum: 0, settings: ns) // 5
            .Numeric("RemoveQty", header: "Remove Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 6
            .Text("CLocation", header: "Location", iseditingreadonly: true) // 7
            .Text("reasonid", header: "Reason ID", settings: ts) // 8
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 9
            ;
            #endregion 欄位設定
            this.detailgrid.Columns["qtyafter"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
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

            string ids = string.Empty, sqlcmd = string.Empty;
            DualResult result, result2;
            DataTable datacheck;

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
SELECT AL2.poid, 
       AL2.refno, 
       AL2.color, 
       Linv.lobqty + ( AL2.qtyafter - AL2.qtybefore ) balanceQty, 
       ( AL2.qtyafter - AL2.qtybefore )               AdjustQty 
FROM   adjustlocal_detail AL2 
       INNER JOIN localinventory Linv 
               ON AL2.refno = Linv.refno 
                  AND AL2.poid = Linv.orderid 
                  AND AL2.color = Linv.threadcolorid 
WHERE  AL2.id = '{0}' ", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        if (MyUtility.Convert.GetDecimal(tmp["balanceQty"]) >= 0)
                        {
                            #region 更新表頭狀態資料
                            string sqlupdHeader = string.Format(@"update AdjustLocal set status='Confirmed', editname = '{0}' , editdate = GETDATE() where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);
                            if (!(result = DBProxy.Current.Execute(null, sqlupdHeader)))
                            {
                                this.ShowErr(sqlupdHeader, result);
                                return;
                            }
                            #endregion
                            #region 更新數量
                            string sqlupdqty = string.Format(@"update LocalInventory set LObQty ='{0}' where  refno='{1}' and orderid='{2}' and ThreadColorID='{3}'", tmp["balanceQty"], tmp["Refno"], tmp["poid"], tmp["color"]);

                            if (!(result = DBProxy.Current.Execute(null, sqlupdqty)))
                            {
                                this.ShowErr(sqlupdHeader, result);
                                return;
                            }
                            #endregion
                        }
                        else
                        {
                            ids += string.Format(
                                "SP#: {0} Refno#:{1} Color:{2}'s balance: {3} is less than Adjust qty: {4}" + Environment.NewLine + "Balacne Qty is not enough!!",
                                tmp["poid"], tmp["refno"], tmp["color"], tmp["balanceqty"], tmp["AdjustQty"]);
                        }
                    }

                    if (!MyUtility.Check.Empty(ids))
                    {
                        MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                        return;
                    }

                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
            }
            #endregion 檢查負數庫存

        }

        // UnConfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string ids = string.Empty, sqlcmd = string.Empty;
            DualResult result, result2;
            DataTable datacheck;

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
SELECT AL2.poid, 
       AL2.refno, 
       AL2.color, 
       Linv.lobqty - ( AL2.qtyafter - AL2.qtybefore ) balanceQty, 
       ( AL2.qtyafter - AL2.qtybefore )               AdjustQty 
FROM   adjustlocal_detail AL2 
       INNER JOIN localinventory Linv 
               ON AL2.refno = Linv.refno 
                  AND AL2.poid = Linv.orderid 
                  AND AL2.color = Linv.threadcolorid 
WHERE  AL2.id = '{0}' ", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        if (MyUtility.Convert.GetDecimal(tmp["balanceQty"]) >= 0)
                        {
                            #region 更新表頭狀態資料
                            string sqlupdHeader = string.Format(@"update AdjustLocal set status='New', editname = '{0}' , editdate = GETDATE() where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);
                            if (!(result = DBProxy.Current.Execute(null, sqlupdHeader)))
                            {
                                this.ShowErr(sqlupdHeader, result);
                                return;
                            }
                            #endregion
                            #region 更新數量
                            string sqlupdqty = string.Format(@"update LocalInventory set LObQty ='{0}' where refno='{1}' and orderid='{2}' and ThreadColorID='{3}'", tmp["balanceQty"], tmp["Refno"], tmp["poid"], tmp["color"]);
                            if (!(result = DBProxy.Current.Execute(null, sqlupdqty)))
                            {
                                this.ShowErr(sqlupdHeader, result);
                                return;
                            }
                            #endregion

                        }
                        else
                        {
                            ids += string.Format(
                                "SP#: {0} Refno#:{1} Color:{2}'s balance: {3} is less than Adjust qty: {4}" + Environment.NewLine + "Balacne Qty is not enough!!",
                                tmp["poid"], tmp["refno"], tmp["color"], tmp["balanceqty"], tmp["AdjustQty"]);
                        }
                    }

                    if (!MyUtility.Check.Empty(ids))
                    {
                        MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                        return;
                    }

                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
            }
            #endregion 檢查負數庫存
        }

        // Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new P46_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

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
