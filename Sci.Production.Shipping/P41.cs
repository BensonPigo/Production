using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P41
    /// </summary>
    public partial class P41 : Win.Tems.Input6
    {
        private DataGridViewGeneratorTextColumnSettings customsp = new DataGridViewGeneratorTextColumnSettings();

        /// <summary>
        /// /P41
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"select * from VNExportDeclaration_Detail WITH (NOLOCK) where ID = '{0}' order by OrderID,Article,SizeCode", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region Custom SP#的Validating與按右鍵
            this.customsp.EditingMouseDown += (s, e) =>
                {
                    if (this.EditMode)
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            if (e.RowIndex != -1)
                            {
                                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                                string sqlCmd = string.Format(
                                    @"select c.CustomSP,(c.Qty-c.PulloutQty) as Balance
from VNConsumption c WITH (NOLOCK) 
where c.VNContractID = '{0}'
and c.StyleID = '{1}'
and c.Category = '{2}'
and c.BrandID = '{3}'
and exists (select 1 from VNConsumption_Article ca WITH (NOLOCK) where ca.ID = c.ID and ca.Article = '{4}')
and exists (select 1 from VNConsumption_SizeCode cs WITH (NOLOCK) where cs.ID = c.ID and cs.SizeCode = '{5}')
order by c.CustomSP",
                                    MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                                    MyUtility.Convert.GetString(dr["StyleID"]),
                                    MyUtility.Convert.GetString(dr["Category"]),
                                    MyUtility.Convert.GetString(dr["BrandID"]),
                                    MyUtility.Convert.GetString(dr["Article"]),
                                    MyUtility.Convert.GetString(dr["SizeCode"]));

                                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10,10", dr["CustomSP"].ToString().Trim());
                                DialogResult returnResult = item.ShowDialog();
                                if (returnResult == DialogResult.Cancel)
                                {
                                    return;
                                }

                                dr["CustomSP"] = item.GetSelectedString();
                            }
                        }
                    }
                };

            this.customsp.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        if (MyUtility.Convert.GetString(dr["CustomSP"]) != MyUtility.Convert.GetString(e.FormattedValue))
                        {
                            string sqlCmd = string.Format(
                                @"select c.CustomSP,(c.Qty-c.PulloutQty) as Balance
from VNConsumption c WITH (NOLOCK) 
where c.VNContractID = '{0}'
and c.StyleID = '{1}'
and c.Category = '{2}'
and c.BrandID = '{3}'
and c.CustomSP = '{6}'
and exists (select 1 from VNConsumption_Article ca WITH (NOLOCK) where ca.ID = c.ID and ca.Article = '{4}')
and exists (select 1 from VNConsumption_SizeCode cs WITH (NOLOCK) where cs.ID = c.ID and cs.SizeCode = '{5}')
order by c.CustomSP",
                                MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                                MyUtility.Convert.GetString(dr["StyleID"]),
                                MyUtility.Convert.GetString(dr["Category"]),
                                MyUtility.Convert.GetString(dr["BrandID"]),
                                MyUtility.Convert.GetString(dr["Article"]),
                                MyUtility.Convert.GetString(dr["SizeCode"]),
                                MyUtility.Convert.GetString(e.FormattedValue));

                            if (!MyUtility.Check.Seek(sqlCmd))
                            {
                                dr["CustomSP"] = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("Custom SP# not found!!");
                                return;
                            }
                        }
                    }
                }
            };
            #endregion
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8), settings: this.customsp)
                .Text("Article", header: "Color way", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("ExportQty", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.numQty.Value = 0;
            this.numNW.Value = 0;
            this.numGW.Value = 0;
            this.numCMP.Value = 0;
            string sqlCmd;
            if (!MyUtility.Check.Empty(this.CurrentMaintain["InvNo"]))
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["DataFrom"]) == "PACKINGLIST")
                {
                    sqlCmd = string.Format(
                        @"select sum(NW) as NW,sum(GW) as GW,sum(ShipQty) as ShipQty,(select sum(ROUND(ed.ExportQty*c.CPU*c.VNMultiple,2))
from VNExportDeclaration_Detail ed WITH (NOLOCK) 
inner join VNConsumption c WITH (NOLOCK) on c.CustomSP = ed.CustomSP
where ed.ID = '{0}'
and c.VNContractID = '{1}') as CMP 
from PackingList WITH (NOLOCK) where INVNo = '{2}'",
                        MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]));
                }
                else
                {
                    sqlCmd = string.Format(
                        @"select TotalNW as NW,TotalGW as GW,TotalShipQty as ShipQty,(select sum(ROUND(ed.ExportQty*c.CPU*c.VNMultiple,2))
from VNExportDeclaration_Detail ed WITH (NOLOCK) 
inner join VNConsumption c WITH (NOLOCK) on c.CustomSP = ed.CustomSP
where ed.ID = '{0}'
and c.VNContractID = '{1}') as CMP 
from GMTBooking WITH (NOLOCK) where ID = '{2}'",
                        MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]));
                }

                DataTable tmpData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out tmpData);
                if (result && tmpData.Rows.Count > 0)
                {
                    this.numQty.Value = MyUtility.Convert.GetDecimal(tmpData.Rows[0]["ShipQty"]);
                    this.numNW.Value = MyUtility.Convert.GetDecimal(tmpData.Rows[0]["NW"]);
                    this.numGW.Value = MyUtility.Convert.GetDecimal(tmpData.Rows[0]["GW"]);
                    this.numCMP.Value = MyUtility.Convert.GetDecimal(tmpData.Rows[0]["CMP"]);
                }
            }

            if (this.EditMode)
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
                {
                    this.dateDate.ReadOnly = true;
                    this.txtInvNo.ReadOnly = true;
                    this.txtContractNo.ReadOnly = true;
                    this.txtPortofExport.ReadOnly = true;
                    this.btnReCalculate.Enabled = true;
                    this.detailgrid.IsEditingReadOnly = true;
                }
                else
                {
                    this.txtCustomdeclareno.ReadOnly = true;
                }

                this.detailgrid.EnsureStyle();
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["CDate"] = DateTime.Today;
            this.CurrentMaintain["VNContractID"] = MyUtility.GetValue.Lookup("select top 1 ID from VNContract WITH (NOLOCK) where StartDate <= GETDATE() and EndDate >= GETDATE() and Status = 'Confirmed'");
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "JUNKED")
            {
                MyUtility.Msg.WarningBox("This record already junked, can't edit!!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't delete!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["DeclareNo"]))
            {
                MyUtility.Msg.WarningBox("Already have Custom declare no. ,can't delete!!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["CDate"]))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["VNContractID"]))
            {
                this.txtContractNo.Focus();
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["InvNo"]))
            {
                this.txtInvNo.Focus();
                MyUtility.Msg.WarningBox("Inv No. can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["VNExportPortID"]))
            {
                this.txtPortofExport.Focus();
                MyUtility.Msg.WarningBox("Port of Export can't empty!!");
                return false;
            }

            #endregion

            #region 檢查表身Custom SP#不可為空值與表身資料不可為空
            int recCount = 0;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["CustomSP"]))
                {
                    MyUtility.Msg.WarningBox("Custom SP# can't empty!!");
                    return false;
                }

                recCount++;
            }

            if (recCount == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }
            #endregion

            // Get ID
            if (this.IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(Env.User.Keyword + "ED", "VNExportDeclaration", Convert.ToDateTime(this.CurrentMaintain["CDate"]), 2, "ID", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = newID;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P41_Print callPurchaseForm = new P41_Print();
            callPurchaseForm.ShowDialog(this);
            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["DeclareNo"]))
            {
                MyUtility.Msg.WarningBox("This record already have Custom declare no., can't Junk!!");
                return;
            }

            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Junk > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == DialogResult.No)
            {
                return;
            }

            IList<string> updateCmds = new List<string>();
            string sqlCmd = string.Format(
                @"select ed.CustomSP,sum(ed.ExportQty) as ExportQty
from VNExportDeclaration_Detail ed WITH (NOLOCK) 
where ed.ID = '{0}'
group by ed.CustomSP", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable updateData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out updateData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sum total export qty fail!!");
                return;
            }

            foreach (DataRow dr in updateData.Rows)
            {
                updateCmds.Add(string.Format("update VNConsumption set PulloutQty = PulloutQty-{0} where CustomSP = '{1}' and VNContractID = '{2}';", MyUtility.Convert.GetString(dr["ExportQty"]), MyUtility.Convert.GetString(dr["CustomSP"]), MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"])));
            }

            updateCmds.Add(string.Format("update VNExportDeclaration set EditDate = GETDATE(), EditName = '{0}', Status = 'Junked' where ID = '{1}';", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Junk fail!!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            if (MyUtility.Convert.GetString(this.CurrentMaintain["DataFrom"]) == "PACKINGLIST")
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from PackingList WITH (NOLOCK) where INVNo = '{0}' and Status = 'Confirmed'", MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]))))
                {
                    MyUtility.Msg.WarningBox("Packing FOC not yet confirmed, this declaration can't confirm!!");
                    return;
                }
            }
            else
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from GMTBooking WITH (NOLOCK) where ID = '{0}' and Status = 'Confirmed'", MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]))))
                {
                    MyUtility.Msg.WarningBox("Garment booking not yet confirmed, this declaration can't confirm!!");
                    return;
                }
            }

            // 檢查CustomSP的Balance Qty是否會小於0
            string sqlCmd = string.Format(
                @"select CustomSP 
from (select ed.CustomSP,sum(ed.ExportQty) as ExportQty, c.Qty-c.PulloutQty as RemainQty
	  from VNExportDeclaration_Detail ed WITH (NOLOCK) 
	  inner join VNConsumption c WITH (NOLOCK) on c.CustomSP = ed.CustomSP
	  where c.VNContractID = '{0}'
	  and ed.ID = '{1}'
	  group by ed.CustomSP,c.Qty-c.PulloutQty) a
where a.RemainQty - a.ExportQty < 0",
                MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DataTable checkData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out checkData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Check CustomSp Balance Qty fail!!");
                return;
            }

            if (checkData.Rows.Count > 0)
            {
                StringBuilder errMsg = new StringBuilder();
                errMsg.Append("Below Custom SP#'s balance will become minus.\r\n");
                foreach (DataRow dr in checkData.Rows)
                {
                    errMsg.Append(string.Format("{0}\r\n", MyUtility.Convert.GetString(dr["CustomSP"])));
                }

                MyUtility.Msg.WarningBox(errMsg.ToString());
                return;
            }

            // 檢查報關數量是否跟Garment Booking數量一致，若不一致會請User確認是否要繼續做Confirmed
            string qty = MyUtility.GetValue.Lookup(string.Format(
                @"select (isnull((select sum(pd.ShipQty)
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd on p.ID = pd.ID
where p.INVNo = '{0}'),0)-
isnull((select sum(ExportQty) as ExportQty from VNExportDeclaration_Detail WITH (NOLOCK) where ID = '{1}'),0)) as BalanceQty",
                MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]),
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            if (qty != "0")
            {
                DialogResult buttonResult = MyUtility.Msg.WarningBox("Declaration Qty is not equal to Garment Booking Qty. Are you sure you want to < Confirm > this data?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == DialogResult.No)
                {
                    return;
                }
            }

            IList<string> updateCmds = new List<string>();
            sqlCmd = string.Format(
                @"select ed.CustomSP,sum(ed.ExportQty) as ExportQty
from VNExportDeclaration_Detail ed WITH (NOLOCK) 
where ed.ID = '{0}'
group by ed.CustomSP", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable updateData;
            result = DBProxy.Current.Select(null, sqlCmd, out updateData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sum total export qty fail!!");
                return;
            }

            foreach (DataRow dr in updateData.Rows)
            {
                updateCmds.Add(string.Format("update VNConsumption set PulloutQty = PulloutQty+{0} where CustomSP = '{1}' and VNContractID = '{2}';", MyUtility.Convert.GetString(dr["ExportQty"]), MyUtility.Convert.GetString(dr["CustomSP"]), MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"])));
            }

            updateCmds.Add(string.Format("update VNExportDeclaration set EditDate = GETDATE(), EditName = '{0}', Status = 'Confirmed' where ID = '{1}';", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail!!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            IList<string> updateCmds = new List<string>();
            string sqlCmd = string.Format(
                @"select ed.CustomSP,sum(ed.ExportQty) as ExportQty
from VNExportDeclaration_Detail ed WITH (NOLOCK) 
where ed.ID = '{0}'
group by ed.CustomSP", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable updateData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out updateData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sum total export qty fail!!");
                return;
            }

            foreach (DataRow dr in updateData.Rows)
            {
                updateCmds.Add(string.Format("update VNConsumption set PulloutQty = PulloutQty-{0} where CustomSP = '{1}' and VNContractID = '{2}';", MyUtility.Convert.GetString(dr["ExportQty"]), MyUtility.Convert.GetString(dr["CustomSP"]), MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"])));
            }

            updateCmds.Add(string.Format("update VNExportDeclaration set EditDate = GETDATE(), EditName = '{0}', Status = 'New' where ID = '{1}';", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail!!\r\n" + result.ToString());
                return;
            }
        }

        // Contract No.
        private void TxtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("select ID from VNContract WITH (NOLOCK) where StartDate <= {0} and EndDate >= {0} and Status = 'Confirmed'", MyUtility.Check.Empty(this.CurrentMaintain["CDate"]) ? "GETDATE()" : "'" + Convert.ToDateTime(this.CurrentMaintain["CDate"]).ToString("d") + "'");
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtContractNo.Text = item.GetSelectedString();
        }

        // Contract No.
        private void TxtContractNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtContractNo.Text) && this.txtContractNo.Text != this.txtContractNo.OldValue)
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}'", this.txtContractNo.Text)))
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where  ID = '{0}' and StartDate <= {1} and EndDate >= {1} and Status = 'Confirmed'", this.txtContractNo.Text, MyUtility.Check.Empty(this.CurrentMaintain["CDate"]) ? "GETDATE()" : "'" + Convert.ToDateTime(this.CurrentMaintain["CDate"]).ToString("d") + "'")))
                    {
                        this.txtContractNo.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("This Contract No. can't use.");
                        return;
                    }
                }
                else
                {
                    this.txtContractNo.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    return;
                }
            }
        }

        // Port of Export
        private void TxtPortofExport_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Name from VNExportPort WITH (NOLOCK) where Junk = 0", "10,50", this.Text, false, ",", headercaptions: "Code,Name");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtPortofExport.Text = item.GetSelectedString();
        }

        // Port of Export
        private void TxtPortofExport_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtPortofExport.Text) && this.txtPortofExport.Text != this.txtPortofExport.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from VNExportPort WITH (NOLOCK) where ID = '{0}'", this.txtPortofExport.Text)))
                {
                    this.txtPortofExport.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Data not found!!");
                    return;
                }
            }
        }

        // Port of Export
        private void TxtPortofExport_TextChanged(object sender, EventArgs e)
        {
            this.displayPortofExport.Text = MyUtility.GetValue.Lookup("Name", this.txtPortofExport.Text, "VNExportPort", "ID");
        }

        // Inv No.
        private void TxtInvNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtInvNo.Text != this.txtInvNo.OldValue)
            {
                foreach (DataRow dr in this.DetailDatas)
                {
                    dr.Delete();
                }

                if (!MyUtility.Check.Empty(this.txtInvNo.Text))
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from GMTBooking WITH (NOLOCK) where ID = '{0}'", this.txtInvNo.Text)))
                    {
                        if (!MyUtility.Check.Seek(string.Format("select ID from PackingList WITH (NOLOCK) where INVNo = '{0}'", this.txtInvNo.Text)))
                        {
                            this.txtInvNo.Text = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!!");
                            return;
                        }
                        else
                        {
                            this.CurrentMaintain["InvNo"] = this.txtInvNo.Text;
                            this.CurrentMaintain["DataFrom"] = "PACKINGLIST";
                        }
                    }
                    else
                    {
                        this.CurrentMaintain["InvNo"] = this.txtInvNo.Text;
                        this.CurrentMaintain["DataFrom"] = "GMTBOOKING";
                    }

                    if (MyUtility.Check.Seek(string.Format("select ID from VNExportDeclaration WITH (NOLOCK) where ID <> '{0}' and InvNo = '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]))))
                    {
                        this.CurrentMaintain["InvNo"] = string.Empty;
                        this.CurrentMaintain["DataFrom"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("This <Inv No.> already created!!");
                        return;
                    }
                }

                this.GenDetailData();
            }
        }

        // 產生Detail資料
        private void GenDetailData()
        {
            #region 組撈資料SQL
            string sqlCmd = string.Format(
                @"Declare @invno VARCHAR(25),
		@contractid VARCHAR(15)

SET @invno = '{0}'
SET @contractid = '{1}'

DECLARE cursor_packingdata CURSOR FOR
select pd.OrderID,isnull(o.StyleID,'') as StyleID,pd.Article,pd.SizeCode,sum(pd.ShipQty) as TtlShipQty,
isnull(o.Category,'') as Category,isnull(o.SeasonID,'') as SeasonID,isnull(o.BrandID,'') as BrandID,isnull(o.StyleUkey,0) as StyleUKey
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
where p.INVNo = @invno
group by pd.OrderID,pd.Article,pd.SizeCode,o.Category,o.StyleID,o.SeasonID,o.BrandID,o.StyleUkey

DECLARE @tempCustomSP TABLE (
   CustomSP VARCHAR(8),
   BalanceQty INT
)

DECLARE @tempPackingList TABLE (
   OrderID VARCHAR(13),
   StyleID VARCHAR(15),
   SeasonID VARCHAR(10),
   BrandID VARCHAR(8),
   Category VARCHAR(1),
   CustomSP VARCHAR(8),
   Article VARCHAR(8),
   SizeCode VARCHAR(8),
   ExportQty INT,
   StyleUKey BIGINT
)

DECLARE @orderid VARCHAR(13),
		@article VARCHAR(8),
		@sizecode VARCHAR(8),
		@category VARCHAR(1),
		@styleid VARCHAR(15),
		@seasonid VARCHAR(10),
		@brandid VARCHAR(8),
		@ttlshipqty INT,
        @styleukey BIGINT,
		@consumptioncustomsp VARCHAR(8),
		@consumptionremindqty INT,
		@cursorbalance INT,
		@customsp VARCHAR(8)

OPEN cursor_packingdata
FETCH NEXT FROM cursor_packingdata INTO @orderid,@styleid,@article,@sizecode,@ttlshipqty,@category,@seasonid,@brandid,@styleukey
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE cursor_consumption CURSOR FOR
	select c.CustomSP,c.Qty-c.PulloutQty as RemindQty
	from VNConsumption c WITH (NOLOCK) 
	where c.VNContractID = @contractid
	and c.Qty-c.PulloutQty > 0
	and c.Category = @category
	and c.StyleID = @styleid
	and exists (select 1 from VNConsumption_Article cc WITH (NOLOCK) where cc.ID = c.ID and cc.Article = @article)
	and exists (select 1 from VNConsumption_SizeCode cs WITH (NOLOCK) where cs.ID = c.ID and cs.SizeCode  = @sizecode)
	order by c.CustomSP

	SET @customsp = ''

	OPEN cursor_consumption
	FETCH NEXT FROM cursor_consumption INTO @consumptioncustomsp,@consumptionremindqty
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @consumptionremindqty > @ttlshipqty
			BEGIN
				select @cursorbalance = BalanceQty from @tempCustomSP where CustomSP = @consumptioncustomsp
				IF @cursorbalance is not null
					BEGIN
						IF @cursorbalance >= @ttlshipqty
							BEGIN
								SET @customsp = @consumptioncustomsp
								BREAK
							END
					END
				ELSE
					BEGIN
						INSERT @tempCustomSP (CustomSP,BalanceQty) VALUES (@consumptioncustomsp,@consumptionremindqty - @ttlshipqty)
						SET @customsp = @consumptioncustomsp
						BREAK
					END
			END
		FETCH NEXT FROM cursor_consumption INTO @consumptioncustomsp,@consumptionremindqty
	END
	CLOSE cursor_consumption
	DEALLOCATE cursor_consumption

	INSERT INTO @tempPackingList (OrderID,StyleID,SeasonID,BrandID,Category,CustomSP,Article,SizeCode,ExportQty,StyleUKey)
	VALUES (@orderid,@styleid,@seasonid,@brandid,@category,@customsp,@article,@sizecode,@ttlshipqty,@styleukey)
	
	FETCH NEXT FROM cursor_packingdata INTO @orderid,@styleid,@article,@sizecode,@ttlshipqty,@category,@seasonid,@brandid,@styleukey
END
CLOSE cursor_packingdata
DEALLOCATE cursor_packingdata

select * from @tempPackingList",
                MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]),
                MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]));

            #endregion
            DataTable tmpDetail;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out tmpDetail);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail, Please try again re-calculate.");
                return;
            }

            foreach (DataRow dr in tmpDetail.Rows)
            {
                DataRow newRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
                newRow["OrderID"] = dr["OrderID"];
                newRow["StyleID"] = dr["StyleID"];
                newRow["SeasonID"] = dr["SeasonID"];
                newRow["BrandID"] = dr["BrandID"];
                newRow["Category"] = dr["Category"];
                newRow["CustomSP"] = dr["CustomSP"];
                newRow["Article"] = dr["Article"];
                newRow["SizeCode"] = dr["SizeCode"];
                newRow["ExportQty"] = dr["ExportQty"];
                newRow["StyleUKey"] = dr["StyleUKey"];

                ((DataTable)this.detailgridbs.DataSource).Rows.Add(newRow);
            }
        }

        // Re-Calculate
        private void BtnReCalculate_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["InvNo"]))
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["DataFrom"]) == "PACKINGLIST")
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from PackingList WITH (NOLOCK) where INVNo = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]))))
                    {
                        MyUtility.Msg.WarningBox("Invoice No. not exiest!!");
                        return;
                    }
                }
                else
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from GMTBooking WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]))))
                    {
                        MyUtility.Msg.WarningBox("Invoice No. not exiest!!");
                        return;
                    }
                }

                foreach (DataRow dr in this.DetailDatas)
                {
                    dr.Delete();
                }

                this.GenDetailData();
            }
        }
    }
}
