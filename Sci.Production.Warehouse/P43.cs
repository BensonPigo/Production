using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P43 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = string.Format("Type='O' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "O";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // 刪除前檢查

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
                        @"SP#: {0} SEQ#:{1} Roll#:{2}  
Original Qty and Current Qty can't be equal!!",
                        row["poid"].ToString().Trim(), row["Seq"].ToString().Trim(), row["Roll"].ToString().Trim()) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["reasonid"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} SEQ#:{1} Roll#:{2} ",
                        row["poid"], row["Seq"], row["Roll"]) + Environment.NewLine + "Reason can't be empty!!" + Environment.NewLine);
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

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "AO", "Adjust", (DateTime)this.CurrentMaintain["Issuedate"], 2, "ID", null);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickSaveAfter()
        {
            // P43存檔,塞值 Adjust_Detail.StockType='O' , MdivisionID
            DualResult result;
            string sqlUpd = string.Format(
                @"
update Adjust_detail
set StockType='O',
MDivisionID='{0}'
where id='{1}'", Env.User.Keyword, this.CurrentMaintain["ID"]);
            if (!(result = DBProxy.Current.Execute(null, sqlUpd)))
            {
                this.ShowErr(sqlUpd, result);
                return;
            }

            base.ClickSaveAfter();
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // grid 加工填值

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }
        }

        // 表身資料SQL Command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select 
AD2.id,
[Seq]= AD2.Seq1+' '+AD2.Seq2,
AD2.seq1,
AD2.seq2,
AD2.POID,
AD2.Roll,
AD2.Dyelot,
AD2.StockType,
AD2.MDivisionID,
Fa.Description,
AD2.QtyBefore,
AD2.QtyAfter,
[AdjustQty] = AD2.QtyAfter - AD2.QtyBefore,
Po3.StockUnit,
[location]= dbo.Getlocation(FTI.Ukey),
AD2.ReasonId,
[reason_nm]=Reason.Name,
AD2.Ukey,
ColorID =dbo.GetColorMultipleID(PO3.BrandId, PO3.ColorID)
from Adjust_Detail AD2
inner join PO_Supp_Detail PO3 on PO3.ID=AD2.POID 
inner join FtyInventory FTI on FTI.POID=AD2.POID and FTI.Seq1=AD2.Seq1
	and FTI.Seq2=AD2.Seq2 and FTI.Roll=AD2.Roll and FTI.StockType='O'
and PO3.SEQ1=AD2.Seq1 and PO3.SEQ2=AD2.Seq2 and FTI.Dyelot = AD2.Dyelot
outer apply (
	select Description from Fabric where SCIRefno=PO3.SCIRefno
) Fa
outer apply(select Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND ID= AD2.ReasonId and junk=0 ) Reason
where AD2.Id='{0}' ", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // 表身資料設定

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnDetailGridSetup()
        {
            #region -- Current Qty Vaild 判斷 --

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();

            ns.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode && (MyUtility.Convert.GetDecimal(e.FormattedValue) == MyUtility.Convert.GetDecimal(this.CurrentDetailData["QtyBefore"])))
                {
                    MyUtility.Msg.WarningBox(@"Current Qty cannot be equal Original Qty!!");
                    e.Cancel = true;
                    return;
                }

                if (this.EditMode && string.Compare(this.CurrentDetailData["qtyafter"].ToString(), e.FormattedValue.ToString()) != 0)
                {
                    this.CurrentDetailData["QtyAfter"] = e.FormattedValue;
                    this.CurrentDetailData["AdjustQty"] = (decimal)e.FormattedValue - (decimal)this.CurrentDetailData["qtybefore"];
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

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
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
and ReasonTypeID='Stock_Adjust' AND junk = 0", e.FormattedValue), out dr, null))
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
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("QtyBefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("QtyAfter", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum: 0, settings: ns)
            .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Text("StockUnit", header: "Unit", iseditingreadonly: true)
            .Text("location", header: "location", iseditingreadonly: true)
            .Text("reasonid", header: "Reason ID", settings: ts)
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20));
            #endregion 欄位設定
            this.detailgrid.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Confirm

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
SELECT AD2.poid, [Seq]= AD2.Seq1+' '+AD2.Seq2,
        AD2.Seq1,AD2.Seq2,
        AD2.Roll,AD2.Dyelot,
       [CheckQty] = (FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty) + ( AD2.qtyafter - AD2.qtybefore ) , 
       [FTYLobQty] = (FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty),
       [AdjustQty]= (AD2.qtyafter - AD2.qtybefore )       
FROM    FtyInventory FTI
inner join Adjust_detail AD2 on FTI.POID=AD2.POID 
and FTI.Seq1=AD2.Seq1
and FTI.Seq2=AD2.Seq2 
and FTI.Roll=AD2.Roll
and FTI.Dyelot = AD2.Dyelot
WHERE FTI.StockType='O' and AD2.ID = '{0}' ", this.CurrentMaintain["id"]);
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
                        if (MyUtility.Convert.GetDecimal(tmp["CheckQty"]) >= 0)
                        {
                            #region 更新表頭狀態資料 and 數量
                            // 更新FtyInventory
                            // 20171006 mantis 7895 增加roll dyelot條件
                            string sqlupdHeader = string.Format(
                                @"
                            update FtyInventory  
                            set  AdjustQty = AdjustQty + ({0}) 
                            where POID = '{1}' AND SEQ1='{2}' AND SEQ2='{3}' and StockType='O' and Roll = '{4}' and Dyelot = '{5}'
                            ", MyUtility.Convert.GetDecimal(tmp["AdjustQty"]), tmp["Poid"], tmp["seq1"].ToString(), tmp["seq2"], tmp["Roll"], tmp["Dyelot"]);

                            // 更新Adjust
                            sqlupdHeader = sqlupdHeader + string.Format(
                                @"
                            update Adjust
                            set Status='Confirmed',
                            editname = '{0}' , editdate = GETDATE() where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);
                            if (!(result = DBProxy.Current.Execute(null, sqlupdHeader)))
                            {
                                this.ShowErr(sqlupdHeader, result);
                                return;
                            }
                            #endregion
                        }
                        else
                        {
                            ids += string.Format(
                                "SP#: {0} SEQ#:{1} Roll#:{2} Dyelot:{3}'s balance: {4} is less than Adjust qty: {5}" + Environment.NewLine + "Balacne Qty is not enough!!",
                                tmp["poid"],
                                tmp["Seq"],
                                tmp["Roll"],
                                tmp["Dyelot"],
                                tmp["FTYLobQty"],
                                tmp["AdjustQty"]) + Environment.NewLine;
                        }
                    }

                    if (!MyUtility.Check.Empty(ids))
                    {
                        MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                        return;
                    }

                    // 更新MDivisionPoDetail
                    this.UpdMDivisionPoDetail();

                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
            }
            #endregion 檢查負數庫存
        }

        // UnConfirm

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
SELECT AD2.poid, [Seq]= AD2.Seq1+' '+AD2.Seq2,
        AD2.Seq1,AD2.Seq2,
        AD2.Roll,AD2.Dyelot,
       [CheckQty] = (FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty) - ( AD2.qtyafter - AD2.qtybefore ) , 
       [FTYLobQty] = (FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty),
       [AdjustQty]= (AD2.qtyafter - AD2.qtybefore )       
FROM    FtyInventory FTI
inner join Adjust_detail AD2 on FTI.POID=AD2.POID 
and FTI.Seq1=AD2.Seq1
and FTI.Seq2=AD2.Seq2 
and FTI.Roll=AD2.Roll
and FTI.Dyelot = AD2.Dyelot
WHERE FTI.StockType='O' and AD2.ID = '{0}' ", this.CurrentMaintain["id"]);
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
                        if (MyUtility.Convert.GetDecimal(tmp["CheckQty"]) >= 0)
                        {
                            #region 更新表頭狀態資料 and 數量
                            // 更新FtyInventory
                            // 20171006 mantis 7895 增加roll dyelot條件
                            string sqlupdHeader = string.Format(
                                @"
                            update FtyInventory  
                            set  AdjustQty = AdjustQty - ({0})
                            where POID = '{1}' AND SEQ1='{2}' AND SEQ2='{3}' and StockType='O'  and Roll = '{4}' and Dyelot = '{5}'
                            ", MyUtility.Convert.GetDecimal(tmp["AdjustQty"]), tmp["Poid"], tmp["seq1"].ToString(), tmp["seq2"], tmp["Roll"], tmp["Dyelot"]);

                            // 更新Adjust
                            sqlupdHeader = sqlupdHeader + string.Format(
                                @"
                            update Adjust
                            set Status='New',
                            editname = '{0}' , editdate = GETDATE() where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);
                            if (!(result = DBProxy.Current.Execute(null, sqlupdHeader)))
                            {
                                this.ShowErr(sqlupdHeader, result);
                                return;
                            }
                            #endregion
                        }
                        else
                        {
                            ids += string.Format(
                                "SP#: {0} SEQ#:{1} Roll#:{2} Dyelot:{3}'s balance: {4} is less than Adjust qty: {5}" + Environment.NewLine + "Balacne Qty is not enough!!",
                                tmp["poid"], tmp["Seq"], tmp["Roll"], tmp["Dyelot"], tmp["FTYLobQty"], tmp["AdjustQty"]) + Environment.NewLine;
                        }
                    }

                    if (!MyUtility.Check.Empty(ids))
                    {
                        MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                        return;
                    }

                    // 更新MDivisionPoDetail
                    this.UpdMDivisionPoDetail();

                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
            }
            #endregion 檢查負數庫存
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            var frm = new P43_Import(this.CurrentMaintain, dt);
            frm.ShowDialog(this);
            this.RenewData();
            dt.DefaultView.Sort = "poid,seq,Roll";
        }

        private void BtnFind_Click(object sender, EventArgs e)
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

        private void UpdMDivisionPoDetail()
        {
            DataTable datacheck;
            DualResult result;
            string sqlcmd = string.Format(
                @"
SELECT 
    md.POID
    ,md.Seq1
    ,md.seq2
    ,[FTYLobQty] = sum(FTI.InQty - FTI.OutQty + FTI.AdjustQty - FTI.ReturnQty)
FROM FtyInventory FTI
inner join Adjust_detail AD2 on FTI.POID=AD2.POID 
and FTI.Seq1=AD2.Seq1
and FTI.Seq2=AD2.Seq2 
and FTI.Roll=AD2.Roll
and FTI.Dyelot = AD2.Dyelot
inner join MDivisionPoDetail md on FTI.POID=md.POID and fti.Seq1=md.Seq1 and fti.Seq2=md.Seq2
WHERE FTI.StockType='O' and AD2.ID = '{0}'
group by md.POID,md.Seq1,md.Seq2", this.CurrentMaintain["id"].ToString());
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    if (!(result = DBProxy.Current.Execute(null, string.Format(
                        @"
Update MDivisionPoDetail
set LobQty = {0}
where POID = '{1}'
and Seq1 = '{2}' and Seq2 = '{3}' ",
                        MyUtility.Convert.GetDecimal(dr["FTYLobQty"]),
                        dr["poid"].ToString(),
                        dr["seq1"].ToString(),
                        dr["seq2"].ToString()))))
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
            }
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P43", this);
        }
    }
}
