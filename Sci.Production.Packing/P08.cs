using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P08
    /// </summary>
    public partial class P08 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable gridData;

        /// <summary>
        /// P08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Type", header: "Packing Type", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Date("SewInLine", header: "Sewing Inline Date", iseditingreadonly: true)
                .Date("EstCTNBooking", header: "Carton Est. Booking", iseditingreadonly: true)
                .Date("EstCTNArrive", header: "Carton Est. Arrived", iseditingreadonly: true);
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPStart.Text) && MyUtility.Check.Empty(this.txtSPEnd.Text) && MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) && MyUtility.Check.Empty(this.dateSewingInlineDate.Value1) && MyUtility.Check.Empty(this.dateSewingInlineDate.Value2) && MyUtility.Check.Empty(this.dateCartonEstBooking.Value1) && MyUtility.Check.Empty(this.dateCartonEstBooking.Value2) && MyUtility.Check.Empty(this.dateCartonEstArrived.Value1) && MyUtility.Check.Empty(this.dateCartonEstArrived.Value2))
            {
                this.txtSPStart.Focus();
                MyUtility.Msg.WarningBox("< SP# > or < SCI Delivery > or < Sewing Inline Date > or < Carton Est. Booking > or < Carton Est. Arrived > can not empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select distinct 0 as Selected, pl.id,
	Type = case when pl.Type = 'B' then 'Bulk'
	                   when pl.Type = 'S' then 'Sample'
	                   when pl.Type = 'L' then 'Local'
		               end, 
    pld.OrderID, o.SciDelivery, o.SewInLine, pl.EstCTNBooking, pl.EstCTNArrive
from PackingList pl WITH (NOLOCK) , PackingList_Detail pld WITH (NOLOCK) , Orders o WITH (NOLOCK) 
where pl.MDivisionID = @mdivisionid
and (pl.Type = 'B' or pl.Type = 'S' or pl.Type = 'L')
and pl.ApvToPurchase = 0
and pl.EstCTNBooking is not null
and pl.EstCTNArrive is not null
and pld.ID = pl.ID
and o.ID = pld.OrderID
and o.Junk = 0");
            #region 組條件
            if (!MyUtility.Check.Empty(this.txtSPStart.Text))
            {
                sqlCmd.Append("\r\nand o.ID >= @orderid1");
            }

            if (!MyUtility.Check.Empty(this.txtSPEnd.Text))
            {
                sqlCmd.Append("\r\nand o.ID <= @orderid2");
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                sqlCmd.Append("\r\nand o.SciDelivery >= @scidelivery1");
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                sqlCmd.Append("\r\nand o.SciDelivery <= @scidelivery2");
            }

            if (!MyUtility.Check.Empty(this.dateSewingInlineDate.Value1))
            {
                sqlCmd.Append("\r\nand o.SewInLine >= @sewinline1");
            }

            if (!MyUtility.Check.Empty(this.dateSewingInlineDate.Value2))
            {
                sqlCmd.Append("\r\nand o.SewInLine <= @sewinline2");
            }

            if (!MyUtility.Check.Empty(this.dateCartonEstBooking.Value1))
            {
                sqlCmd.Append("\r\nand pl.EstCTNBooking >= @estbooking1");
            }

            if (!MyUtility.Check.Empty(this.dateCartonEstBooking.Value2))
            {
                sqlCmd.Append("\r\nand pl.EstCTNBooking <= @estbooking2");
            }

            if (!MyUtility.Check.Empty(this.dateCartonEstArrived.Value1))
            {
                sqlCmd.Append("\r\nand pl.EstCTNArrive >= @estarrive1");
            }

            if (!MyUtility.Check.Empty(this.dateCartonEstArrived.Value2))
            {
                sqlCmd.Append("\r\nand pl.EstCTNArrive <= @estarrive2");
            }
            #endregion

            #region 準備sql參數資料
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@mdivisionid", Sci.Env.User.Keyword);
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@orderid1", this.txtSPStart.Text);
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@orderid2", this.txtSPEnd.Text);

            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp4.ParameterName = "@scidelivery1";
            sp4.Value = !MyUtility.Check.Empty(this.dateSCIDelivery.Value1) ? this.dateSCIDelivery.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
            sp5.ParameterName = "@scidelivery2";
            sp5.Value = !MyUtility.Check.Empty(this.dateSCIDelivery.Value2) ? this.dateSCIDelivery.Value2 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
            sp6.ParameterName = "@sewinline1";
            sp6.Value = !MyUtility.Check.Empty(this.dateSewingInlineDate.Value1) ? this.dateSewingInlineDate.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
            sp7.ParameterName = "@sewinline2";
            sp7.Value = !MyUtility.Check.Empty(this.dateSewingInlineDate.Value2) ? this.dateSewingInlineDate.Value2 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp8 = new System.Data.SqlClient.SqlParameter();
            sp8.ParameterName = "@estbooking1";
            sp8.Value = !MyUtility.Check.Empty(this.dateCartonEstBooking.Value1) ? this.dateCartonEstBooking.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp9 = new System.Data.SqlClient.SqlParameter();
            sp9.ParameterName = "@estbooking2";
            sp9.Value = !MyUtility.Check.Empty(this.dateCartonEstBooking.Value2) ? this.dateCartonEstBooking.Value2 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp10 = new System.Data.SqlClient.SqlParameter();
            sp10.ParameterName = "@estarrive1";
            sp10.Value = !MyUtility.Check.Empty(this.dateCartonEstArrived.Value1) ? this.dateCartonEstArrived.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp11 = new System.Data.SqlClient.SqlParameter();
            sp11.ParameterName = "@estarrive2";
            sp11.Value = !MyUtility.Check.Empty(this.dateCartonEstArrived.Value2) ? this.dateCartonEstArrived.Value2 : DateTime.Now;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);
            cmds.Add(sp5);
            cmds.Add(sp6);
            cmds.Add(sp7);
            cmds.Add(sp8);
            cmds.Add(sp9);
            cmds.Add(sp10);
            cmds.Add(sp11);
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
            }
            else
            {
                if (this.gridData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;

            if (gridData == null || gridData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            DataTable excelTable;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(gridData, "ID,Type,OrderId,SciDelivery,SewInLine,EstCTNBooking,EstCTNArrive", "select * from #tmp", out excelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }

            bool result = MyUtility.Excel.CopyToXls(excelTable, string.Empty, xltfile: "Packing_P08.xltx", headerRow: 2);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }
        }

        // Approve
        private void BtnApprove_Click(object sender, EventArgs e)
        {
            this.gridDetail.ValidateControl();
            this.gridDetail.EndEdit();
            this.listControlBindingSource1.EndEdit();
            if (this.gridData == null)
            {
                return;
            }

            if (this.gridData.Rows.Count == 0)
            {
                return;
            }

            DataTable detailData = (DataTable)this.listControlBindingSource1.DataSource;
            DataRow[] dr = detailData.Select("Selected = 1");
            if (dr.Length <= 0)
            {
                MyUtility.Msg.WarningBox("Please select at least one data!");
                return;
            }

            #region 更新資料到PackingList.ApvToPurchase
            IList<string> updateCmds = new List<string>();
            foreach (DataRow currentRow in dr)
            {
                updateCmds.Add(string.Format(
                    @"update PackingList 
                                                   set ApvToPurchase = 1, ApvToPurchaseDate = GETDATE()
                                                   where ID = '{0}' and ApvToPurchase = 0;", currentRow["ID"].ToString()));
            }

            if (updateCmds.Count > 0)
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Executes(null, updateCmds);
                        if (result)
                        {
                            transactionScope.Complete();
                            transactionScope.Dispose();
                            MyUtility.Msg.InfoBox("Approve completed!");
                            foreach (DataRow currentRow in dr)
                            {
                                currentRow.Delete();
                            }
                        }
                        else
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("Approve failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
            #endregion
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
