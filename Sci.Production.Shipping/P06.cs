﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P06
    /// </summary>
    public partial class P06 : Sci.Win.Tems.Input8
    {
        private string id;
        private DateTime pulloutDate;
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings shipqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings status = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private ITableSchema revisedTS;
        private ITableSchema revised_detailTS;
        private DataTable PulloutReviseData;
        private DataTable PulloutReviseDetailData;

        /// <summary>
        /// P06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            this.DoSubForm = new P06_ShipQtyDetail();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridicon.Insert.Visible = true;
            this.gridicon.Append.Visible = true;
            this.gridicon.Remove.Visible = true;

            #region 取TableSchema & 結構
            DBProxy.Current.GetTableSchema(null, "Pullout_Revise", out this.revisedTS);
            DBProxy.Current.GetTableSchema(null, "Pullout_Revise_Detail", out this.revised_detailTS);

            string sqlCmd = "select * from Pullout_Revise WITH (NOLOCK) where 1=0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.PulloutReviseData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Get Pullout_Revise structure fail!!\r\n" + result.ToString());
                return;
            }

            sqlCmd = "select * from Pullout_Revise_Detail WITH (NOLOCK) where 1=0";
            result = DBProxy.Current.Select(null, sqlCmd, out this.PulloutReviseDetailData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Get Pullout_Revise_Detail structure fail!!\r\n" + result.ToString());
                return;
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"select pd.*,o.StyleID,o.BrandID,o.Dest,
Variance = (
	pd.OrderQty 
	- isnull((select sum(ShipQty) from Pullout_Detail WITH (NOLOCK) where OrderID = pd.OrderID),0)
	- isnull ((select sum(DiffQty) 
	from InvAdjust_Qty iq WITH (NOLOCK) 
	inner join InvAdjust i WITH (NOLOCK) on iq.ID = i.id 
	where i.orderid = pd.OrderID), 0)
),
case pd.Status 
when 'P' then 'Partial'
when 'C' then 'Complete'
when 'E' then 'Exceed'
when 'S' then 'Shortage'
else ''
end as StatusExp
from Pullout_Detail pd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
where pd.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "0" : MyUtility.Convert.GetString(e.Detail["UKey"]);

            this.SubDetailSelectCommand = string.Format(
                @"select pdd.*,oqd.Qty,(oqd.Qty-pdd.ShipQty) as Variance
from Pullout_Detail_Detail pdd WITH (NOLOCK) 
left join Pullout_Detail pd WITH (NOLOCK) on pd.UKey = pdd.Pullout_DetailUKey
left join Orders o WITH (NOLOCK) on o.ID = pdd.OrderID
left join Order_SizeCode os WITH (NOLOCK) on o.POID = os.Id and os.SizeCode = pdd.SizeCode
left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pdd.OrderID and oqd.Seq = pd.OrderShipmodeSeq and oqd.Article = pdd.Article and oqd.SizeCode = pdd.SizeCode
where pdd.Pullout_DetailUKey = {0}
order by os.Seq", masterID);
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label6.Visible = MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "LOCKED" ? true : false;
            this.btnHistory.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Pullout_History WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
            this.btnRevisedHistory.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Pullout_Revise WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.shipqty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this.DoSubForm.IsSupportDelete = false;
                    this.DoSubForm.IsSupportNew = false;
                    this.DoSubForm.IsSupportUpdate = false;
                    this.DoSubForm.EditMode = false;
                    this.OpenSubDetailPage();
                }
            };
            this.status.EditingTextChanged += (s, e) =>
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(dr["Status"]) == "C")
                    {
                        dr["StatusExp"] = dr["StatusExp"];
                        return;
                    }

                    if (!MyUtility.Check.Empty(e.DataGridView.Rows[e.RowIndex].Cells[9].EditedFormattedValue))
                    {
                        switch (e.DataGridView.Rows[e.RowIndex].Cells[9].EditedFormattedValue.ToString().ToUpper().Substring(0, 1))
                        {
                            case "C":
                                dr["StatusExp"] = "Complete";
                                dr["Status"] = "C";
                                dr.EndEdit();
                                break;
                            case "P":
                                dr["StatusExp"] = "Partial";
                                dr["Status"] = "P";
                                dr.EndEdit();
                                break;
                            case "S":
                                dr["StatusExp"] = "Shortage";
                                dr["Status"] = "S";
                                dr.EndEdit();
                                break;
                            default:
                                dr["StatusExp"] = dr["StatusExp"];
                                break;
                        }
                    }
                };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Order Shipmode Seq", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("ShipModeSeqQty", header: "Order Q'ty by Seq", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Ship Q’ty", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: this.shipqty)
                .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .ExtText("StatusExp", header: "Status", width: Widths.AnsiChars(10), charCasing: CharacterCasing.Normal, settings: this.status)
                .Text("ShipmodeID", header: "Shipping Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("PackingListID", header: "Packing#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("INVNo", header: "Invoice No.", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("Remark", header: "Remarks", width: Widths.AnsiChars(30));
        }

        private void Status_Change()
        {
            DataTable dr = (DataTable)this.detailgridbs.DataSource;
            int i = 0;
            foreach (DataRow dr1 in dr.Rows)
            {
                switch (dr1["Status"].ToString().ToUpper())
                {
                    case "C":
                        this.detailgrid.Rows[i].Cells[9].Value = "Complete";
                        break;
                    case "P":
                        this.detailgrid.Rows[i].Cells[9].Value = "Partial";
                        break;
                    case "S":
                        this.detailgrid.Rows[i].Cells[9].Value = "Shortage";
                        break;
                    case "E":
                        this.detailgrid.Rows[i].Cells[9].Value = "Exceed";
                        break;
                    default:
                        this.detailgrid.Rows[i].Cells[9].Value = string.Empty;
                        break;
                }

                i++;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickNewBefore()
        {
            Sci.Production.Shipping.P06_Append callNextForm = new Sci.Production.Shipping.P06_Append();
            DialogResult dr = callNextForm.ShowDialog(this);

            // 當Form:P06_Append是按OK時，要新增一筆資料進Cursor
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // 檢查此日期是否已存在資料庫
                if (MyUtility.Check.Seek(string.Format("select ID from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}'", Convert.ToDateTime(callNextForm.PulloutDate).ToString("d"), Sci.Env.User.Keyword)))
                {
                    MyUtility.Msg.WarningBox(string.Format("Pull-out date:{0} already exists!!", callNextForm.PulloutDate.ToAppDateFormatString()));
                    return false;
                }

                // 檢查此日期是否小於System.PullLock
                if (callNextForm.PulloutDate <= (DateTime)MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select PullLock from System WITH (NOLOCK) ")))
                {
                    MyUtility.Msg.WarningBox("Pull-out date can't be earlier than pull-out lock date!!");
                    return false;
                }

                if (this.CurrentDataRow != null)
                {
                    string newID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "PO", "Pullout", callNextForm.PulloutDate, 2, "Id", null);
                    string insertCmd = string.Format(
                        @"insert into Pullout(ID,PulloutDate,MDivisionID,FactoryID,Status,AddName,AddDate)
values('{0}','{1}','{2}','{3}','New','{4}',GETDATE());",
                        newID,
                        Convert.ToDateTime(callNextForm.PulloutDate).ToString("d"),
                        Sci.Env.User.Keyword,
                        Sci.Env.User.Factory,
                        Sci.Env.User.UserID);

                    DualResult result = DBProxy.Current.Execute(null, insertCmd);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Insert data fail, pls try again.\r\n" + result.ToString());
                        return false;
                    }

                    this.id = newID;
                    this.pulloutDate = callNextForm.PulloutDate;
                    DataRow newrow = this.CurrentDataRow.Table.NewRow();
                    newrow["ID"] = newID;
                    newrow["PulloutDate"] = callNextForm.PulloutDate;

                    // newrow["MDivisionID"] = Sci.Env.User.Keyword;
                    newrow["Status"] = "New";
                    newrow["AddName"] = Sci.Env.User.UserID;
                    newrow["AddDate"] = DateTime.Now;

                    this.CurrentDataRow.Table.Rows.Add(newrow);
                    newrow.AcceptChanges();

                    // 點了排序不一定會在最後一筆
                    int position = this.gridbs.Find("ID", newrow["ID"].ToString());
                    this.gridbs.Position = position;

                    // 因為新增資料一定會在最後一筆，所以直接把指標移至最後一筆
                    // gridbs.MoveLast();
                    // 模擬按Edit行為，強制讓畫面進入Detai頁籤，所以要將EditName與EditDate值給清空
                    this.toolbar.cmdEdit.PerformClick();
                    this.CurrentMaintain["EditName"] = string.Empty;
                    this.CurrentMaintain["EditDate"] = DBNull.Value;
                    this.editby.Value = string.Empty;
                    this.ReviseData();
                }
            }

            return false;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() != "NEW")
            {
                MyUtility.Msg.WarningBox("This pullout report already confirmed, can't be edit!!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Check.Seek(string.Format("select ID from Pullout_Detail WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("The pullout has detail data, it can't be deleted!!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            DualResult result;
            if (!MyUtility.Check.Empty(this.CurrentMaintain["SendToTPE"]))
            {
                foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    if (dr.RowState != DataRowState.Unchanged)
                    {
                        if (dr.RowState == DataRowState.Added)
                        {
                                result = this.WriteRevise("Missing", dr);
                                if (!result)
                                {
                                    return result;
                                }
                        }
                        else if (dr.RowState == DataRowState.Modified)
                        {
                            bool t = false;
                            DataTable subDetailData;
                            this.GetSubDetailDatas(dr, out subDetailData);
                            foreach (DataRow tdr in subDetailData.Rows)
                            {
                                if (tdr.RowState != DataRowState.Unchanged)
                                {
                                    t = true;
                                }
                            }

                            if (t)
                            {
                                result = this.WriteRevise("Revise", dr);

                                #region update PulloutID 到PackingList
                                string updatePklst = string.Format(@"Update PackingList set pulloutID = '{0}' where id='{1}'", this.CurrentMaintain["ID"], dr["PackingListID"]);
                                DBProxy.Current.Execute(null, updatePklst);
                                #endregion

                                if (!result)
                                {
                                    return result;
                                }
                            }
                        }
                        else if (dr.RowState == DataRowState.Deleted)
                        {
                                result = this.WriteRevise("Delete", dr);

                                #region update PulloutID 到PackingList
                                string updatePklst = string.Format(@"Update PackingList set pulloutID = '' where id='{0}'", dr["PackingListID"]);
                                DBProxy.Current.Execute(null, updatePklst);
                                #endregion

                                if (!result)
                                {
                                    return result;
                                }
                        }
                    }
                }
            }



            return base.ClickSave();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            DualResult result;
            if (this.updatePackinglist.Trim() != string.Empty)
            {
                result = DBProxy.Current.Execute(null, this.updatePackinglist);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update Packinglist fail!!\r\n" + result.ToString());
                    return failResult;
                }
            }

            // 將Pullout箱數回寫回Orders.PulloutCTNQty
            string updateCmd = string.Format(
                @"update Orders set PulloutCTNQty = (select isnull(sum(pd.CTNQty),0)
								   from PackingList_Detail pd, PackingList p
								   where pd.OrderID = Orders.ID 
								   and pd.ID = p.ID
								   and p.PulloutID <> '')
where ID in (select distinct OrderID from Pullout_Detail where ID = '{0}');", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Update orders fail!!\r\n" + result.ToString());
                return failResult;
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                return false;
            }

            string sqlCmd = string.Format(
                @"select pd.OrderID,o.StyleID,o.CustPONo,o.BrandID,c.NameEN,o.StyleUnit,o.Qty,pd.ShipQty,
(select isnull(sum(ShipQty),0) from Pullout_Detail WITH (NOLOCK) where OrderID = pd.OrderID) as TtlShipQty,
(select isnull(sum(CTNQty),0) from PackingList_Detail WITH (NOLOCK) where ID = pd.PackingListID and OrderID = pd.OrderID) as CtnQty,
pd.Remark,pd.ShipmodeID,pd.INVNo
from Pullout_Detail pd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
where pd.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DataTable excelData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out excelData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            DataTable ttlQty;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(excelData, "OrderId,Qty", "select isnull(sum(a.Qty),0) as TtlQty from (select distinct OrderId,Qty from #tmp) a", out ttlQty);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Prepare data error.\r\n" + ex.ToString());
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P06.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = MyUtility.Convert.GetString(this.CurrentMaintain["MDivisionID"]);
            worksheet.Cells[3, 2] = Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("d");
            worksheet.Cells[3, 12] = DateTime.Now.ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));

            int intRowsStart = 5;
            int dataRowCount = excelData.Rows.Count;
            int rownum = 0;
            object[,] objArray = new object[1, 13];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = excelData.Rows[i];
                rownum = intRowsStart + i;
                objArray[0, 0] = dr["OrderID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["CustPONo"];
                objArray[0, 3] = dr["BrandID"];
                objArray[0, 4] = dr["NameEN"];
                objArray[0, 5] = dr["StyleUnit"];
                objArray[0, 6] = dr["Qty"];
                objArray[0, 7] = dr["ShipQty"];
                objArray[0, 8] = dr["TtlShipQty"];
                objArray[0, 9] = dr["CtnQty"];
                objArray[0, 10] = dr["Remark"];
                objArray[0, 11] = dr["ShipmodeID"];
                objArray[0, 12] = dr["INVNo"];

                worksheet.Range[string.Format("A{0}:M{0}", rownum)].Value2 = objArray;
            }

            worksheet.Range[string.Format("A5:M{0}", rownum)].Borders.Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A5:M{0}", rownum)].Borders.LineStyle = 1;
            worksheet.Cells[rownum + 1, 6] = "TTL:";
            worksheet.Cells[rownum + 1, 7] = ttlQty.Rows[0]["TtlQty"];
            worksheet.Cells[rownum + 1, 8] = excelData.Compute("sum(ShipQty)", string.Empty);

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P06");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (MyUtility.Convert.GetDate(this.CurrentMaintain["PulloutDate"]) > DateTime.Today)
            {
                MyUtility.Msg.WarningBox("Pullout date can't greater than today!");
                return;
            }

            // 模擬按Edit行為
            this.toolbar.cmdEdit.PerformClick();

            // 當Revise有錯誤時就不做任何事
            if (!this.ReviseData())
            {
                this.toolbar.cmdUndo.PerformClick();
                return;
            }

            // 模擬按Edit行為
            this.toolbar.cmdSave.PerformClick();

            this.OnDetailEntered();

            #region 檢查Variance是否<0, 若<0則不能confirm 這段
            StringBuilder errmsg = new StringBuilder();
            errmsg.Append("Cannot confirm this Pullout!!\r\n");
            bool errchk = false;
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (MyUtility.Convert.GetDecimal(dr["Variance"]) < 0)
                {
                    errchk = true;
                    errmsg.Append(string.Format("Please check <SP#> {0}, Variance:{1}\r\n", MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetDecimal(dr["Variance"])));
                }
            }

            if (errchk)
            {
                MyUtility.Msg.WarningBox(errmsg.ToString());
                return;
            }
            #endregion

            // 檢查表身資料不可為空
            if (((DataTable)this.detailgridbs.DataSource).Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Details can't empty!!");
                return;
            }

            IList<string> updateCmds = new List<string>();
            updateCmds.Add(string.Format("update Pullout set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}';", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            if (!MyUtility.Check.Empty(this.CurrentMaintain["SendToTPE"]))
            {
                updateCmds.Add(string.Format(
                    @"insert into Pullout_History(ID, HisType, NewValue, AddName, AddDate) values ('{0}','Status','Confirmed','{1}',GETDATE());",
                        MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                        Sci.Env.User.UserID));
            }

            string sqlCmd = string.Format("select distinct OrderID from Pullout_Detail WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable pullOrder;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out pullOrder);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return;
            }

            foreach (DataRow dr in pullOrder.Rows)
            {
                updateCmds.Add(string.Format(
                    @"
UPDATE orders 
SET    actpulloutdate = (SELECT Max(p.pulloutdate) 
FROM   pullout_detail pd, 
    pullout p 
WHERE  pd.orderid = orders.id 
    AND pd.id = p.id 
    AND p.status = 'Confirmed'), 
pulloutcomplete = Iif((
	SELECT Count(p.id)
	FROM   pullout_detail pd, 
			pullout p 
	WHERE  pd.orderid = orders.id 
			AND pd.id = p.id 
			AND p.status = 'Confirmed' 
			AND pd.status = 'C'
) > 0, 1,iif((
SELECT Count(p.id)
	FROM   pullout_detail pd, 
			pullout p 
	WHERE  pd.orderid = orders.id 
			AND pd.id = p.id 
			AND p.status = 'Confirmed' 
			AND pd.status = 'S')>0,1,0)
)
WHERE  id = '{0}' ", MyUtility.Convert.GetString(dr["OrderID"])));
            }

            result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirmed fail!!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            IList<string> sqlCmds = new List<string>();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["SendToTPE"]))
            {
                Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Pullout_Delay");
                DialogResult dResult = callReason.ShowDialog(this);
                if (dResult == System.Windows.Forms.DialogResult.OK)
                {
                    sqlCmds.Add(string.Format(
                        @"insert into Pullout_History(ID, HisType, NewValue, ReasonID,Remark, AddName, AddDate) values ('{0}','Status','Unconfirmed','{1}','{2}','{3}',GETDATE());",
                        MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                        callReason.ReturnReason,
                        callReason.ReturnRemark,
                        Sci.Env.User.UserID));
                }
                else
                {
                    return;
                }
            }

            sqlCmds.Add(string.Format(
                "update Pullout set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}';",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            string selectCmd = string.Format(
                @"with PulloutOrder
as
(
select distinct OrderID 
from Pullout_Detail WITH (NOLOCK) 
where ID = '{0}' and ShipQty > 0
),
PulloutDate
as
(
select pd.OrderID, max(pd.PulloutDate) as PulloutDate from Pullout_Detail pd WITH (NOLOCK) , Pullout p WITH (NOLOCK) 
where exists (select 1 from PulloutOrder where OrderID = pd.OrderID)
and pd.ID <> '{0}'
and pd.ID = p.ID
and p.Status <> 'New'
group by pd.OrderID
)
select po.OrderID,pd.PulloutDate
from PulloutOrder po 
left join PulloutDate pd on pd.OrderID = po.OrderID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable updateOrderData;
            DualResult result = DBProxy.Current.Select(null, selectCmd, out updateOrderData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query update order data fail!! Please try again.\r\n" + result.ToString());
                return;
            }

            foreach (DataRow dr in updateOrderData.Rows)
            {
                sqlCmds.Add(string.Format(
                    "update Orders set ActPulloutDate = {0}, PulloutComplete = 0 where ID = '{1}';",
                    MyUtility.Check.Empty(dr["PulloutDate"]) ? "null" : "'" + Convert.ToDateTime(dr["PulloutDate"]).ToString("d") + "'",
                    MyUtility.Convert.GetString(dr["OrderID"])));
            }

            result = DBProxy.Current.Executes(null, sqlCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Amend fail!!\r\n" + result.ToString());
                return;
            }
        }

        // History
        private void BtnHistory_Click(object sender, EventArgs e)
        {
            // 610: SHIPPING_P06_ReviseHistory_Revised History，出現錯誤訊息
            // Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("Pullout_History", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "Status", reasonType: "Pullout_Delay", caption: "History Pullout Confirm/Unconfirm", haveRemark: true, customerGridFormatTable: "HisType", moduleName: "Pullout");
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("Pullout_History", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "Status", reasonType: "Pullout_Delay", caption: "History Pullout Confirm/Unconfirm", haveRemark: true, customerGridFormatTable: "HisType", moduleName: "Shipping");
            callNextForm.ShowDialog(this);
        }

        // Revised History
        private void BtnRevisedHistory_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P06_ReviseHistory callNextForm = new Sci.Production.Shipping.P06_ReviseHistory(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        private string updatePackinglist; // 用來先在ReviseData()準備更新Packinglist.pulloutid的SQL, 在save才執行

        // Revise from ship plan and FOC/LO packing list
        private bool ReviseData()
        {
            this.updatePackinglist = string.Empty;
            #region 檢查資料是否有還沒做Confirmed的
            StringBuilder msgString = new StringBuilder();
            string sqlCmd = string.Format(
                @"
select distinct ID from PackingList WITH (NOLOCK) 
where PulloutDate = '{0}' 
and MDivisionID = '{1}'
and Status = 'New' 
and (Type = 'F' or Type = 'L')",
                Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("d"),
                Sci.Env.User.Keyword);

            DataTable packlistData;
            DualResult result;
            result = DBProxy.Current.Select(null, sqlCmd, out packlistData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query packing list fail!\r\n" + result.ToString());
                return false;
            }

            if (packlistData != null && packlistData.Rows.Count > 0)
            {
                msgString.Append("Packing List ID: " + string.Join(",", packlistData.AsEnumerable().Select(row => row["ID"].ToString())));
            }

            sqlCmd = string.Format(
                @"
select distinct p.ShipPlanID
from PackingList p WITH (NOLOCK) 
left join ShipPlan s WITH (NOLOCK) on s.ID = p.ShipPlanID
where p.PulloutDate = '{0}' 
and p.MDivisionID = '{1}'
and p.ShipPlanID != ''
and (s.Status != 'Confirmed' or p.Status = 'New')",
                Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("d"),
                Sci.Env.User.Keyword);

            DataTable shipPlanData;
            result = DBProxy.Current.Select(null, sqlCmd, out shipPlanData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query ship plan fail!\r\n" + result.ToString());
                return false;
            }

            if (shipPlanData != null && shipPlanData.Rows.Count > 0)
            {
                if (msgString.Length > 0)
                {
                    msgString.Append("\r\n");
                }

                msgString.Append("Ship Plan ID:" + string.Join(", ", shipPlanData.AsEnumerable().Select(row => row["ShipPlanID"].ToString())));
            }

            if (msgString.Length > 0)
            {
                MyUtility.Msg.WarningBox(string.Format("Below data not yet confirm!!\r\n{0}", msgString.ToString()));
                return false;
            }
            #endregion

            // 撈所有符合條件的Packing List資料
            #region 組SQL
            sqlCmd = string.Format(
                @"
with ShipPlanData as (
    select  pd.ID as PackingListID
            , p.Type
            , p.ShipModeID
            , pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.SizeCode
            , o.Qty as OrderQty
            , oq.Qty as OrderShipQty
            , oqd.Qty as SeqQty
            , sum(pd.ShipQty) as Shipqty
            , p.INVNo
            , o.StyleID
            , o.BrandID
            , o.Dest
    from PackingList p WITH (NOLOCK) 
    left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    left join ShipPlan s WITH (NOLOCK) on s.ID = p.ShipPlanID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
                                                and oq.Seq = pd.OrderShipmodeSeq
    left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pd.OrderID 
                                                        and oqd.Seq = pd.OrderShipmodeSeq 
                                                        and oqd.Article = pd.Article 
                                                        and oqd.SizeCode = pd.SizeCode
    where (p.Type = 'B' or p.Type = 'S')
          and s.Status = 'Confirmed'
          and p.MDivisionID = '{0}'
          and p.PulloutDate = '{1}'
          and (  p.PulloutID = '' or p.PulloutID = '{2}') -- 20161220 willy 避免如果原本有資料,之後修改資料會清空shipQty問題
    group by pd.ID, p.Type, p.ShipModeID, pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.SizeCode, o.Qty
          , oq.Qty, oqd.Qty, p.INVNo, o.StyleID, o.BrandID, o.Dest
),
FLPacking as (
    select  pd.ID as PackingListID
            , p.Type
            , p.ShipModeID
            , pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.SizeCode
            , o.Qty as OrderQty
            , oq.Qty as OrderShipQty
            , oqd.Qty as SeqQty
            , sum(pd.ShipQty) as Shipqty
            , p.INVNo
            , o.StyleID
            , o.BrandID
            , o.Dest
    from PackingList p WITH (NOLOCK) 
    left join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
    left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
                                                and oq.Seq = pd.OrderShipmodeSeq
    left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = pd.OrderID 
                                                        and oqd.Seq = pd.OrderShipmodeSeq 
                                                        and oqd.Article = pd.Article 
                                                        and oqd.SizeCode = pd.SizeCode
    where   (p.Type = 'F' or p.Type = 'L')
            and p.Status = 'Confirmed'
            and p.MDivisionID = '{0}'
            and p.PulloutDate = '{1}'
            and (p.PulloutID = '' or p.PulloutID='{2}')  -- 20170918 aaron 避免如果原本有資料,之後修改資料會清空shipQty問題
    group by pd.ID, p.Type, p.ShipModeID, pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.SizeCode
             , o.Qty, oq.Qty, oqd.Qty, p.INVNo, o.StyleID, o.BrandID, o.Dest 
),
AllPackData as (
    select * 
    from ShipPlanData

    union all
    select * 
    from FLPacking
),
SummaryData as (
    select  PackingListID
            , Type
            , ShipModeID
            , OrderID
            , OrderShipmodeSeq
            , '' as Article
            , '' as SizeCode
            , OrderQty
            , OrderShipQty
            , 0 as SeqQty
            , sum(Shipqty) as Shipqty
            , INVNo
            , StyleID
            , BrandID
            , Dest 
    from AllPackData
    group by PackingListID, Type, ShipModeID, OrderID, OrderShipmodeSeq, OrderQty, OrderShipQty
             , INVNo, StyleID, BrandID, Dest 
)
select  'D' as DataType
        , *
        , 0 as AllShipQty 
from AllPackData

union all
select  'S' as DataType
        , *
        , AllShipQty = (isnull ((select sum(ShipQty) 
                                 from Pullout_Detail WITH (NOLOCK) 
                                 where ID <> '{2}'  
                                       and OrderID = SummaryData.OrderID), 0) 
                        + isnull ((select sum(DiffQty) 
                                   from InvAdjust_Qty iq WITH (NOLOCK) 
                                   inner join InvAdjust i WITH (NOLOCK) on iq.ID = i.id 
                                   inner join SummaryData b on i.OrderID = b.OrderID), 0))  
from SummaryData",
                Sci.Env.User.Keyword,
                Convert.ToDateTime(this.CurrentMaintain["PulloutDate"]).ToString("d"),
                MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? "XXXXXXXXXX" : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            #endregion
            DataTable allPackData;
            result = DBProxy.Current.Select(null, sqlCmd, out allPackData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            this.detailgridbs.SuspendBinding();

            #region 檢查現有資料的異動

            // foreach (DataRow dr in DetailDatas)
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                DataRow[] packData = allPackData.Select(string.Format("DataType = 'S' and PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"])));

                // 存在表身，但資料已被修改，就必須把第3層資料刪除，表身資料的Ship Qty改成0
                if (packData.Length <= 0)
                {
                    // 刪除第3層資料
                    DataTable subDetailData;
                    this.GetSubDetailDatas(dr, out subDetailData);
                    List<DataRow> drArray = new List<DataRow>();
                    foreach (DataRow ddr in subDetailData.Rows)
                    {
                        drArray.Add(ddr);
                    }

                    foreach (DataRow ddr in drArray)
                    {
                        ddr.Delete();
                    }

                    // 清空PackingList.pulloutID
                    this.updatePackinglist += string.Format(@"Update PackingList set pulloutID = '' where id='{0}' and pulloutID = '{1}'; ", dr["PackingListID"], this.CurrentMaintain["id"].ToString());

                    #region 合併計算 ShipQty
                    int sumShipQty = 0;
                    if (allPackData.AsEnumerable().Any(row => row["OrderID"].EqualString(dr["OrderID"]) && row["DataType"].EqualString("S")))
                    {
                        sumShipQty = MyUtility.Convert.GetInt(allPackData.AsEnumerable().Where(row => row["OrderID"].EqualString(dr["OrderID"]) && row["DataType"].EqualString("S")).CopyToDataTable().Compute("sum(ShipQty)", null));
                    }

                    string strAllShipQty = string.Format(
                        @"
select AllShipQty = (isnull ((select sum(ShipQty) 
                             from Pullout_Detail WITH (NOLOCK) 
                             where ID <> '{0}' and OrderID = '{1}'), 0) --此orderid,非此單的Pullout_Detail.ShipQty
                          + isnull ((select sum(DiffQty) 
                                from InvAdjust_Qty iq WITH (NOLOCK) 
                                inner join InvAdjust i WITH (NOLOCK) on iq.ID = i.id 
                                where i.OrderID = '{1}'), 0))",
                        MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? "XXXXXXXXXX" : MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                        dr["OrderID"]);

                    int allShipQty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(strAllShipQty));
                    int totalShipQty = sumShipQty + allShipQty;
                    #endregion
                    #region 判斷 Status
                    string newStatus = string.Empty;
                    if (dr["Status"].ToString().ToUpper() == "SHORTAGE" || dr["Status"].ToString().ToUpper() == "S")
                    {
                        newStatus = "S";
                    }
                    else
                    {
                        int orderQty = MyUtility.Convert.GetInt(dr["OrderQty"]);
                        newStatus = totalShipQty == orderQty ? "C" : totalShipQty > orderQty ? "E" : "P";
                    }
                    #endregion

                    dr["ShipQty"] = 0;
                    dr["Status"] = newStatus;
                    dr["StatusExp"] = this.GetStatusName(newStatus);
                    dr["PackingListID"] = string.Empty;
                    dr["INVNo"] = string.Empty;
                    dr["ReviseDate"] = DateTime.Now;
                }
                else
                {
                    #region 判斷此筆PackingListID→packinglist.pulloutid是否和此單一樣
                    string chkpulloutid = string.Format(@"select pulloutid from packinglist where id = '{0}'", dr["PackingListID"]);
                    string pulloutid = MyUtility.GetValue.Lookup(chkpulloutid);
                    if (pulloutid == string.Empty || pulloutid == this.CurrentMaintain["id"].ToString())
                    {
                        this.updatePackinglist += string.Format(@"Update PackingList set pulloutID = '{0}' where id='{1}'; ", this.CurrentMaintain["ID"], dr["PackingListID"]);
                    }
                    else
                    {
                        dr["PackingListID"] = string.Empty;
                        dr["INVNo"] = string.Empty;
                        dr["ShipQty"] = 0;

                        // 刪除第3層資料
                        DataTable subDetailData2;
                        this.GetSubDetailDatas(dr, out subDetailData2);
                        List<DataRow> drArray2 = new List<DataRow>();
                        foreach (DataRow ddr in subDetailData2.Rows)
                        {
                            drArray2.Add(ddr);
                        }

                        foreach (DataRow ddr in drArray2)
                        {
                            ddr.Delete();
                        }
                    }
                    #endregion
                    #region 合併計算 ShipQty
                    int sumShipQty = MyUtility.Convert.GetInt(allPackData.AsEnumerable().Where(row => row["OrderID"].EqualString(dr["OrderID"]) && row["DataType"].EqualString("S")).CopyToDataTable().Compute("sum(ShipQty)", null));
                    int totalShipQty = sumShipQty + MyUtility.Convert.GetInt(packData[0]["AllShipQty"]);
                    #endregion
                    #region 判斷 Status
                    string newStatus = string.Empty;
                    if (dr["Status"].ToString().ToUpper() == "SHORTAGE" || dr["Status"].ToString().ToUpper() == "S")
                    {
                        newStatus = "S";
                    }
                    else
                    {
                        int orderQty = MyUtility.Convert.GetInt(packData[0]["OrderQty"]);
                        newStatus = totalShipQty == orderQty ? "C" : totalShipQty > orderQty ? "E" : "P";
                    }
                    #endregion

                    // shipQty,OrderQty,InvNo 修改過才會更換資料
                    if (MyUtility.Convert.GetInt(dr["ShipQty"]) != MyUtility.Convert.GetInt(packData[0]["ShipQty"]) || MyUtility.Convert.GetInt(dr["OrderQty"]) != MyUtility.Convert.GetInt(packData[0]["OrderQty"]) || MyUtility.Convert.GetString(dr["INVNo"]) != MyUtility.Convert.GetString(packData[0]["INVNo"]))
                    {
                        dr["ShipQty"] = packData[0]["ShipQty"];
                        dr["OrderQty"] = packData[0]["OrderQty"];
                        dr["ShipModeSeqQty"] = packData[0]["OrderShipmodeSeq"];
                        dr["Status"] = newStatus;
                        dr["StatusExp"] = this.GetStatusName(newStatus);
                        dr["INVNo"] = packData[0]["INVNo"];
                        dr["ShipmodeID"] = packData[0]["ShipmodeID"];
                        dr["ReviseDate"] = DateTime.Now;
                        dr["Variance"] = MyUtility.Convert.GetInt(packData[0]["OrderQty"]) - totalShipQty;
                    }

                    // 不管資料有無修改,都會重新更新status資料
                    dr["Status"] = newStatus;
                    dr["StatusExp"] = this.GetStatusName(newStatus);

                    // 取出第3層資料，比對是否有異動
                    DataTable subDetailData;
                    this.GetSubDetailDatas(dr, out subDetailData);
                    List<DataRow> drArray = new List<DataRow>();
                    foreach (DataRow ddr in subDetailData.Rows)
                    {
                        drArray.Add(ddr);
                    }

                    foreach (DataRow ddr in drArray)
                    {
                        DataRow[] pulloutSubDetail = allPackData.Select(string.Format("DataType = 'D' and PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}' and Article = '{3}' and SizeCode = '{4}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"]), MyUtility.Convert.GetString(ddr["Article"]), MyUtility.Convert.GetString(ddr["SizeCode"])));
                        if (pulloutSubDetail.Length <= 0)
                        {
                            if (dr.RowState == DataRowState.Unchanged)
                            {
                                dr["StatusExp"] = this.GetStatusName(newStatus);
                            }

                            ddr.Delete();
                            dr["ReviseDate"] = DateTime.Now;
                        }
                        else
                        {
                            if (MyUtility.Convert.GetInt(ddr["ShipQty"]) != MyUtility.Convert.GetInt(pulloutSubDetail[0]["Shipqty"]))
                            {
                                if (dr.RowState == DataRowState.Unchanged)
                                {
                                    dr["StatusExp"] = this.GetStatusName(newStatus);
                                }

                                ddr["ShipQty"] = pulloutSubDetail[0]["Shipqty"];
                                dr["ReviseDate"] = DateTime.Now;
                            }
                        }
                    }

                    #region 檢查有被撈出來但不存在Pullout_Detail_Detail中
                    DataRow[] newPackData = allPackData.Select(string.Format("DataType = 'D' and PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"])));
                    if (newPackData.Length > 0)
                    {
                        this.GetSubDetailDatas(dr, out subDetailData);

                        foreach (DataRow ddr in newPackData)
                        {
                            // DataRow[] CurrentPulloutData = SubDetailData.Select(string.Format("ID = '{0}' and Pullout_DetailUkey = '{1}' and OrderID = '{2}' and Article ='{3}'", MyUtility.Convert.GetString(ddr["PackingListID"]), MyUtility.Convert.GetString(ddr["OrderID"]), MyUtility.Convert.GetString(ddr["OrderShipmodeSeq"])));
                            DataRow[] p_Detail_detail = subDetailData.Select(string.Format(
                                "ID = '{0}' and OrderID = '{1}' and Article = '{2}' and SizeCode = '{3}' ",
                                this.CurrentMaintain["ID"],
                                MyUtility.Convert.GetString(ddr["OrderID"]),
                                MyUtility.Convert.GetString(ddr["Article"]),
                                MyUtility.Convert.GetString(ddr["SizeCode"])));

                            if (p_Detail_detail.Length == 0 || MyUtility.Convert.GetInt(p_Detail_detail[0]["ShipQty"]) != MyUtility.Convert.GetInt(ddr["ShipQty"]))
                            {
                                if (dr.RowState == DataRowState.Unchanged)
                                {
                                    dr["StatusExp"] = this.GetStatusName(newStatus);
                                }

                                #region 新增一筆資料到Pullout_Detail_Detail
                                DataRow ndr = subDetailData.NewRow();
                                ndr["ID"] = this.CurrentMaintain["ID"];
                                ndr["Pullout_DetailUKey"] = dr["UKey"];
                                ndr["OrderID"] = dr["OrderID"];
                                ndr["Article"] = ddr["Article"];
                                ndr["SizeCode"] = ddr["SizeCode"];
                                ndr["ShipQty"] = ddr["ShipQty"];
                                ndr["Qty"] = ddr["SeqQty"];
                                ndr["Variance"] = MyUtility.Convert.GetInt(ddr["SeqQty"]) - MyUtility.Convert.GetInt(ddr["ShipQty"]);
                                subDetailData.Rows.Add(ndr);
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region 新增資料
            DataRow[] allSumPackData = allPackData.Select("DataType = 'S'");
            if (allSumPackData.Length > 0)
            {
                foreach (DataRow dr in allSumPackData)
                {
                    DataRow[] pullDetail = null;
                    if (this.detailgridbs != null && ((DataTable)this.detailgridbs.DataSource).Rows.Count > 0)
                    {
                        DataTable dt = (DataTable)this.detailgridbs.DataSource;
                        pullDetail = dt.Select(string.Format("PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"])));
                    }

                    if (pullDetail == null || pullDetail.Length <= 0)
                    {
                        #region 合併計算 ShipQty
                        int sumShipQty = MyUtility.Convert.GetInt(allPackData.AsEnumerable().Where(row => row["OrderID"].EqualString(dr["OrderID"]) && row["DataType"].EqualString("S")).CopyToDataTable().Compute("sum(ShipQty)", null));
                        #endregion
                        int totalShipQty = sumShipQty + MyUtility.Convert.GetInt(dr["AllShipQty"]);
                        string newStatus = totalShipQty == MyUtility.Convert.GetInt(dr["OrderQty"]) ? "C" : totalShipQty > MyUtility.Convert.GetInt(dr["OrderQty"]) ? "E" : "P";

                        DataRow detailNewRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
                        detailNewRow["ID"] = this.CurrentMaintain["ID"];
                        detailNewRow["PulloutDate"] = this.CurrentMaintain["PulloutDate"];
                        detailNewRow["OrderID"] = dr["OrderID"];
                        detailNewRow["OrderShipmodeSeq"] = dr["OrderShipmodeSeq"];
                        detailNewRow["ShipQty"] = dr["ShipQty"];
                        detailNewRow["OrderQty"] = dr["OrderQty"];
                        detailNewRow["ShipModeSeqQty"] = dr["OrderShipQty"];
                        detailNewRow["Status"] = newStatus;
                        detailNewRow["StatusExp"] = this.GetStatusName(newStatus);
                        detailNewRow["PackingListID"] = dr["PackingListID"];
                        detailNewRow["PackingListType"] = dr["Type"];
                        detailNewRow["INVNo"] = dr["INVNo"];
                        detailNewRow["ShipmodeID"] = dr["ShipmodeID"];
                        detailNewRow["StyleID"] = dr["StyleID"];
                        detailNewRow["BrandID"] = dr["BrandID"];
                        detailNewRow["Dest"] = dr["Dest"];
                        detailNewRow["Variance"] = MyUtility.Convert.GetInt(dr["OrderQty"]) - totalShipQty;
                        ((DataTable)this.detailgridbs.DataSource).Rows.Add(detailNewRow);

                        #region update PulloutID 到PackingList
                        this.updatePackinglist += string.Format(@"Update PackingList set pulloutID = '{0}' where id='{1}'; ", this.CurrentMaintain["ID"], dr["PackingListID"]);
                        #endregion

                        #region 新增資料到Pullout_Detail_Detail
                        DataRow[] allSubDetail = allPackData.Select(string.Format("DataType = 'D' and PackingListID = '{0}' and OrderID = '{1}' and OrderShipmodeSeq = '{2}'", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"])));
                        if (allSubDetail.Length > 0)
                        {
                            foreach (DataRow ddr in allSubDetail)
                            {
                                DataTable subDetailData;
                                this.GetSubDetailDatas(detailNewRow, out subDetailData);
                                DataRow ndr = subDetailData.NewRow();
                                ndr["ID"] = this.CurrentMaintain["ID"];
                                ndr["OrderID"] = ddr["OrderID"];
                                ndr["Article"] = ddr["Article"];
                                ndr["SizeCode"] = ddr["SizeCode"];
                                ndr["ShipQty"] = ddr["ShipQty"];
                                ndr["Qty"] = ddr["SeqQty"];
                                ndr["Variance"] = MyUtility.Convert.GetInt(ddr["SeqQty"]) - MyUtility.Convert.GetInt(ddr["ShipQty"]);
                                subDetailData.Rows.Add(ndr);
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            this.detailgridbs.ResumeBinding(); // detailgridbs.SuspendBinding();之後要做
            this.detailgrid.ValidateControl();

            // this.status_Change();
            return true;
        }

        // Revise from ship plan and FOC/LO packing list
        private void BtnRevise_Click(object sender, EventArgs e)
        {
            // 先Load第3層資料
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                DataTable subDetailData;
                this.GetSubDetailDatas(dr, out subDetailData);
            }

            if (this.ReviseData())
            {
                MyUtility.Msg.InfoBox("Revise completed!");

                this.detailgrid.ValidateControl();
            }
        }

        private string GetStatusName(string status)
        {
            return status == "P" ? "Partial" : status == "C" ? "Complete" : status == "E" ? "Exceed" : status == "S" ? "Shortage" : string.Empty;
        }

        private DualResult WriteRevise(string status, DataRow dr)
        {
            DataRow reviseRow = this.PulloutReviseData.NewRow();
            DualResult result = this.WritePulloutRevise(dr, reviseRow, status);
            if (!result)
            {
                return result;
            }

            // to Detail
            result = this.WritePulloutReviseDetail(dr, MyUtility.Convert.GetString(reviseRow["ReviseKey"]));
            if (!result)
            {
                return result;
            }

            return Result.True;
        }

        private DualResult WritePulloutRevise(DataRow dr, DataRow reviseRow, string type)
        {
            reviseRow["ID"] = dr["ID"];
            reviseRow["Type"] = type == "Delete" ? "D" : type == "Revise" ? "R" : "M";
            reviseRow["OrderID"] = dr["OrderID"];
            reviseRow["OldShipQty"] = type == "Missing" ? 0 : dr["ShipQty", DataRowVersion.Original];
            reviseRow["NewShipQty"] = type == "Delete" ? 0 : dr["ShipQty"];
            reviseRow["OldStatus"] = type == "Missing" ? string.Empty : dr["Status", DataRowVersion.Original];
            reviseRow["NewStatus"] = dr["Status"];
            reviseRow["PackingListID"] = dr["PackingListID"];
            reviseRow["Remark"] = dr["Remark"];
            reviseRow["Pullout_DetailUKey"] = dr["UKey"]; // Pullout_Revise沒有ukey
            reviseRow["INVNo"] = dr["INVNo"];
            reviseRow["ShipModeID"] = dr["ShipModeID"];
            reviseRow["AddName"] = Sci.Env.User.UserID;
            reviseRow["AddDate"] = DateTime.Now;

            return DBProxy.Current.Insert(null, this.revisedTS, reviseRow);
        }

        private DualResult WritePulloutReviseDetail(DataRow dr, string identityValue)
        {
            DataTable subDetailData;
            this.GetSubDetailDatas(dr, out subDetailData);
            foreach (DataRow ddr in subDetailData.Rows)
            {
                string type = ddr.RowState == DataRowState.Added ? "Missing" : ddr.RowState == DataRowState.Deleted ? "Delete" : string.Empty;
                DataRow reviseDetailRow = this.PulloutReviseDetailData.NewRow();
                reviseDetailRow["ID"] = type == "Delete" ? ddr["ID", DataRowVersion.Original] : ddr["ID"];
                reviseDetailRow["Pullout_DetailUKey"] = type == "Delete" ? ddr["Pullout_DetailUKey", DataRowVersion.Original] : ddr["Pullout_DetailUKey"];
                reviseDetailRow["Pullout_ReviseReviseKey"] = identityValue;
                reviseDetailRow["OrderID"] = type == "Delete" ? ddr["OrderID", DataRowVersion.Original] : ddr["OrderID"];
                reviseDetailRow["Article"] = type == "Delete" ? ddr["Article", DataRowVersion.Original] : ddr["Article"];
                reviseDetailRow["SizeCode"] = type == "Delete" ? ddr["SizeCode", DataRowVersion.Original] : ddr["SizeCode"];
                reviseDetailRow["OldShipQty"] = type == "Missing" ? 0 : ddr["ShipQty", DataRowVersion.Original];
                reviseDetailRow["NewShipQty"] = type == "Delete" ? 0 : ddr["ShipQty"];
                DualResult result = DBProxy.Current.Insert(null, this.revised_detailTS, reviseDetailRow);
                if (!result)
                {
                    return result;
                }
            }

            return Result.True;
        }
    }
}