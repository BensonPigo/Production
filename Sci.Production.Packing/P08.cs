﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using System.Transactions;

namespace Sci.Production.Packing
{
    public partial class P08 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataTable gridData;

        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //Grid設定
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Type", header: "Packing Type", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Date("SewInLine", header: "Sewing Inline Date", iseditingreadonly: true)
                .Date("EstCTNBooking", header: "Carton Est. Booking", iseditingreadonly: true)
                .Date("EstCTNArrive", header: "Carton Est. Arrived", iseditingreadonly: true);
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(txtSPStart.Text) && MyUtility.Check.Empty(txtSPEnd.Text) && MyUtility.Check.Empty(dateSCIDelivery.Value1) && MyUtility.Check.Empty(dateSCIDelivery.Value2) && MyUtility.Check.Empty(dateSewingInlineDate.Value1) && MyUtility.Check.Empty(dateSewingInlineDate.Value2) && MyUtility.Check.Empty(dateCartonEstBooking.Value1) && MyUtility.Check.Empty(dateCartonEstBooking.Value2) && MyUtility.Check.Empty(dateCartonEstArrived.Value1) && MyUtility.Check.Empty(dateCartonEstArrived.Value2))
            {
                txtSPStart.Focus();
                MyUtility.Msg.WarningBox("< SP# > or < SCI Delivery > or < Sewing Inline Date > or < Carton Est. Booking > or < Carton Est. Arrived > can not empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select distinct 0 as Selected, pl.ID, iif(pl.Type = 'B','Bulk','Sample') as Type, pld.OrderID, o.SciDelivery, o.SewInLine, pl.EstCTNBooking, pl.EstCTNArrive
from PackingList pl WITH (NOLOCK) , PackingList_Detail pld WITH (NOLOCK) , Orders o WITH (NOLOCK) 
where pl.MDivisionID = @mdivisionid
and (pl.Type = 'B' or pl.Type = 'S')
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
            if (!MyUtility.Check.Empty(dateSCIDelivery.Value1))
            {
                sqlCmd.Append("\r\nand o.SciDelivery >= @scidelivery1");
            }
            if (!MyUtility.Check.Empty(dateSCIDelivery.Value2))
            {
                sqlCmd.Append("\r\nand o.SciDelivery <= @scidelivery2");
            }
            if (!MyUtility.Check.Empty(dateSewingInlineDate.Value1))
            {
                sqlCmd.Append("\r\nand o.SewInLine >= @sewinline1");
            }
            if (!MyUtility.Check.Empty(dateSewingInlineDate.Value2))
            {
                sqlCmd.Append("\r\nand o.SewInLine <= @sewinline2");
            }
            if (!MyUtility.Check.Empty(dateCartonEstBooking.Value1))
            {
                sqlCmd.Append("\r\nand pl.EstCTNBooking >= @estbooking1");
            }
            if (!MyUtility.Check.Empty(dateCartonEstBooking.Value2))
            {
                sqlCmd.Append("\r\nand pl.EstCTNBooking <= @estbooking2");
            }
            if (!MyUtility.Check.Empty(dateCartonEstArrived.Value1))
            {
                sqlCmd.Append("\r\nand pl.EstCTNArrive >= @estarrive1");
            }
            if (!MyUtility.Check.Empty(dateCartonEstArrived.Value2))
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
            sp4.Value = !MyUtility.Check.Empty(dateSCIDelivery.Value1) ? dateSCIDelivery.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
            sp5.ParameterName = "@scidelivery2";
            sp5.Value = !MyUtility.Check.Empty(dateSCIDelivery.Value2) ? dateSCIDelivery.Value2 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
            sp6.ParameterName = "@sewinline1";
            sp6.Value = !MyUtility.Check.Empty(dateSewingInlineDate.Value1) ? dateSewingInlineDate.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
            sp7.ParameterName = "@sewinline2";
            sp7.Value = !MyUtility.Check.Empty(dateSewingInlineDate.Value2) ? dateSewingInlineDate.Value2 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp8 = new System.Data.SqlClient.SqlParameter();
            sp8.ParameterName = "@estbooking1";
            sp8.Value = !MyUtility.Check.Empty(dateCartonEstBooking.Value1) ? dateCartonEstBooking.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp9 = new System.Data.SqlClient.SqlParameter();
            sp9.ParameterName = "@estbooking2";
            sp9.Value = !MyUtility.Check.Empty(dateCartonEstBooking.Value2) ? dateCartonEstBooking.Value2 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp10 = new System.Data.SqlClient.SqlParameter();
            sp10.ParameterName = "@estarrive1";
            sp10.Value = !MyUtility.Check.Empty(dateCartonEstArrived.Value1) ? dateCartonEstArrived.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp11 = new System.Data.SqlClient.SqlParameter();
            sp11.ParameterName = "@estarrive2";
            sp11.Value = !MyUtility.Check.Empty(dateCartonEstArrived.Value2) ? dateCartonEstArrived.Value2 : DateTime.Now;

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
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
            }
            else
            {
                if (gridData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable GridData = (DataTable)listControlBindingSource1.DataSource;
            
            if (GridData == null || GridData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            DataTable ExcelTable;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(GridData, "ID,Type,OrderId,SciDelivery,SewInLine,EstCTNBooking,EstCTNArrive", "select * from #tmp", out ExcelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }
            
            bool result = MyUtility.Excel.CopyToXls(ExcelTable, "", xltfile: "Packing_P08.xltx", headerRow: 2);
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }

        }

        //Approve
        private void btnApprove_Click(object sender, EventArgs e)
        {

            this.gridDetail.ValidateControl();
            this.gridDetail.EndEdit();
            listControlBindingSource1.EndEdit();
            if (gridData == null) return; if (gridData.Rows.Count == 0) return;
            DataTable detailData = (DataTable)listControlBindingSource1.DataSource;
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
                updateCmds.Add(string.Format(@"update PackingList 
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
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
            #endregion
        }

        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
