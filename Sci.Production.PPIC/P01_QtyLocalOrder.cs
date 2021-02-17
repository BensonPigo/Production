using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System.Transactions;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_QtyLocalOrder
    /// </summary>
    public partial class P01_QtyLocalOrder : Win.Subs.Base
    {
        private DataTable SizeCode;
        private DataTable Article;
        private DataTable QtyBDown;
        private string orderID;
        private bool editable;
        private int orderQty;
        private MatrixHelper _matrix;

        /// <summary>
        /// P01_QtyLocalOrder
        /// </summary>
        /// <param name="orderID">string OrderID</param>
        /// <param name="editable">bool Editable</param>
        /// <param name="orderQty">int OrderQty</param>
        public P01_QtyLocalOrder(string orderID, bool editable, int orderQty)
        {
            this.InitializeComponent();
            this.orderID = orderID;
            this.editable = editable;
            this.orderQty = orderQty;

            // 按鈕對應的事件方法.
            this.btnVerticalAdd.Click += this.App_col_Click;
            this.btnHorizontalAdd.Click += this.App_row_Click;
            this.btnVerticalInsert.Click += this.Ins_col_Click;
            this.btnHorizontalInsert.Click += this.Ins_row_Click;
            this.btnVerticalDelete.Click += this.Del_col_Click;
            this.btnHorizontalDelete.Click += this.Del_row_Click;
            this.btnEdit.Click += this.Edit_Click;
            this.btnClose.Click += this.Close_Click;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this._matrix = new MatrixHelper(this, this.gridLocalOrder, this.listControlBindingSource1); // 建立 Matrix 物件
            this._matrix.XMap.Name = "SizeCode";  // 對應到第三表格的 X 欄位名稱
            this._matrix.YMap.Name = "Article";  // 對應到第三表格的 Y 欄位名稱

            this._matrix.XUniqueKey = "SizeCode"; // X 軸不可重複的欄位名稱, 可多重欄位, 用","區隔.
            this._matrix.YUniqueKey = "Article"; // Y 軸不可重複的欄位名稱, 可多重欄位, 用","區隔.

            this._matrix
                .SetColDef("NewQty", width: Widths.AnsiChars(6)) // 第三表格對應的欄位名稱
                .AddXColDef("SizeCode") // X 要顯示的欄位名稱, 可設定多個.
                .AddYColDef("Total", header: "Total", width: Widths.AnsiChars(6)) // Y 要顯示的欄位名稱, 可設定多個.
                .AddYColDef("Article", header: "Colorway", width: Widths.UnicodeChars(8))
                ;
            #region Grid控制
            this.gridLocalOrder.EditingControlShowing += (s, e) =>
                {
                    // Total欄位不可以被修改
                    if (this.gridLocalOrder.CurrentCellAddress.X == 0)
                    {
                        e.Control.Enabled = false;
                    }

                    // 限制欄位只能輸入數值
                    if (this.EditMode == true)
                    {
                        if (this.gridLocalOrder.CurrentCellAddress.Y >= 1 && this.gridLocalOrder.CurrentCellAddress.X >= 2)
                        {
                            ((Ict.Win.UI.TextBox)e.Control).InputRestrict = Ict.Win.UI.TextBoxInputsRestrict.Digit;
                        }
                    }
                };

            // Total欄位值顯是為黑色
            this.gridLocalOrder.CellFormatting += (s, e) =>
                {
                    if (e.ColumnIndex == 0)
                    {
                        e.CellStyle.ForeColor = Color.Black;
                    }
                };

            // 值修改後要加總回Total
            this.gridLocalOrder.CellValueChanged += (s, e) =>
                {
                    DataRow dr = this.gridLocalOrder.GetDataRow<DataRow>(e.RowIndex);
                    int sum = 0;
                    if (e.RowIndex >= 1)
                    {
                        foreach (DataGridViewColumn dg in this.gridLocalOrder.Columns)
                        {
                            if (!this._matrix.IsGridYColumn(dg.DisplayIndex))
                            {
                                sum += MyUtility.Convert.GetInt(dr[dg.DataPropertyName]);
                            }
                        }

                        dr[0] = sum;

                        this.gridLocalOrder.InvalidateRow(e.RowIndex);
                    }
                };
            #endregion

            this._matrix.IsXColEditable = true;  // X 顯示的欄位可否編輯?
            this._matrix.IsYColEditable = true;  // Y 顯示的欄位可否編輯?

            DualResult result;

            this.UIConvertToView();

            if (!(result = this.Reload()))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }
        }

        /// <inheritdoc/>
        protected override void OnFormDispose()
        {
            if (this.listControlBindingSource1 != null)
            {
                this.listControlBindingSource1.Dispose();
            }

            base.OnFormDispose();
        }

        #region 應用程式
        private DualResult Reload()
        {
            DualResult result;

            this._matrix.Clear();

            string sqlCmd = string.Format("select * from Order_SizeCode WITH (NOLOCK) where ID = '{0}' order by Seq", this.orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out this.SizeCode);
            sqlCmd = string.Format("select *, isnull((select SUM(Qty) from Order_Qty WITH (NOLOCK) where ID = oa.ID and Article = oa.Article),0) as Total from Order_Article oa WITH (NOLOCK) where oa.ID = '{0}' order by Seq", this.orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out this.Article);
            sqlCmd = string.Format("select *,CONVERT(varchar(6),Qty) as NewQty from Order_Qty WITH (NOLOCK) where ID = '{0}'", this.orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out this.QtyBDown);

            if (!(result = this._matrix.Sets(this.QtyBDown, this.SizeCode, this.Article)))
            {
                return result;
            }

            return Ict.Result.True;
        }

        private void EnsureButtons()
        {
            if (this.EditMode)
            {
                this.btnEdit.Text = "Save";
                this.btnClose.Text = "Undo";
            }
            else
            {
                this.btnEdit.Text = "Edit";
                this.btnClose.Text = "Close";
            }

            if (!this.editable)
            {
                this.btnVerticalAdd.Visible = false;
                this.btnHorizontalAdd.Visible = false;
                this.btnVerticalInsert.Visible = false;
                this.btnHorizontalInsert.Visible = false;
                this.btnVerticalDelete.Visible = false;
                this.btnHorizontalDelete.Visible = false;
                this.btnEdit.Visible = false;
            }
        }

        private void ToMaintain()
        {
            this.UIConvertToMaintain();
            this.OnAcceptChanges();
        }

        private DualResult DoSave(out bool cancel)
        {
            cancel = false;
            DualResult result;

            if (!this.DoValidate())
            {
                cancel = true;
                return Ict.Result.True;
            }

            DataTable datas, xdatas, ydatas;
            if (!(result = this._matrix.GetDatas(out datas, out xdatas, out ydatas)))
            {
                return result;
            }

            int qty = 0;
            foreach (DataRow dr in datas.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    dr["ID"] = this.orderID;
                }

                if (dr.RowState != DataRowState.Deleted)
                {
                    qty = qty + MyUtility.Convert.GetInt(dr["NewQty"]);
                    if (MyUtility.Convert.GetInt(dr["Qty"]) != MyUtility.Convert.GetInt(dr["NewQty"]))
                    {
                        dr["Qty"] = MyUtility.Convert.GetInt(dr["NewQty"]);
                    }
                }
            }

            int seq = 0;
            foreach (DataRow dr in xdatas.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    ++seq;
                    if (dr.RowState == DataRowState.Added)
                    {
                        dr["ID"] = this.orderID;
                    }

                    if (MyUtility.Convert.GetString(dr["Seq"]) != seq.ToString("00"))
                    {
                        dr["Seq"] = seq.ToString("00");
                    }
                }
            }

            seq = 0;
            foreach (DataRow dr in ydatas.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    ++seq;
                    if (dr.RowState == DataRowState.Added)
                    {
                        dr["ID"] = this.orderID;
                    }

                    if (MyUtility.Convert.GetString(dr["Seq"]) != MyUtility.Convert.GetString(seq))
                    {
                        dr["Seq"] = MyUtility.Convert.GetString(seq);
                    }
                }
            }

            ITableSchema tableschema, xtableschema, ytableschema;
            if (!(result = DBProxy.Current.GetTableSchema(null, "Order_SizeCode", out xtableschema)))
            {
                return result;
            }

            if (!(result = DBProxy.Current.GetTableSchema(null, "Order_Article", out ytableschema)))
            {
                return result;
            }

            if (!(result = DBProxy.Current.GetTableSchema(null, "Order_Qty", out tableschema)))
            {
                return result;
            }

            // 準備Order_QtyShip, Order_QtyShip_Detail資料
            IList<string> insertCmds = new List<string>();
            IList<string> updateCmds = new List<string>();
            IList<string> deleteCmds = new List<string>();
            if (this.orderQty != qty)
            {
                updateCmds.Add(string.Format("update Orders set Qty = {0} where ID = '{1}'", MyUtility.Convert.GetString(qty), this.orderID));
            }

            if (MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", this.orderID)))
            {
                string ttlQty = MyUtility.GetValue.Lookup(string.Format("select Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", this.orderID));
                if (MyUtility.Convert.GetInt(ttlQty) != qty)
                {
                    // updateCmds.Add(string.Format("update Order_QtyShip set Qty = {0}, EditName = '{1}', EditDate = GETDATE() where ID = '{2}';", MyUtility.Convert.GetString(qty), Sci.Env.User.UserID, orderID));
                    updateCmds.Add(string.Format("update Order_QtyShip set Qty = {0}, OriQty = {0}, EditName = '{1}', EditDate = GETDATE() where ID = '{2}';", MyUtility.Convert.GetString(qty), Env.User.UserID, this.orderID));
                }

                DataTable order_QtyShip_Detail;
                if (!(result = DBProxy.Current.Select(null, string.Format("select * from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}'", this.orderID), out order_QtyShip_Detail)))
                {
                    return result;
                }

                foreach (DataRow dr in datas.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(dr["Qty"]))
                    {
                        DataRow[] queryData = order_QtyShip_Detail.Select(string.Format("Article = '{0}' and SizeCode = '{1}'", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"])));
                        if (queryData.Length > 0)
                        {
                            if (MyUtility.Convert.GetInt(queryData[0]["Qty"]) != MyUtility.Convert.GetInt(dr["Qty"]))
                            {
                                // updateCmds.Add(string.Format("update Order_QtyShip_Detail set Qty = {0}, EditName = '{1}', EditDate = GETDATE() where UKey = {2};", MyUtility.Convert.GetString(dr["Qty"]), Sci.Env.User.UserID, MyUtility.Convert.GetString(queryData[0]["UKey"])));
                                updateCmds.Add(string.Format("update Order_QtyShip_Detail set Qty = {0}, OriQty = {0}, EditName = '{1}', EditDate = GETDATE() where UKey = {2};", MyUtility.Convert.GetString(dr["Qty"]), Env.User.UserID, MyUtility.Convert.GetString(queryData[0]["UKey"])));
                            }
                        }
                        else
                        {
                            // insertCmds.Add(string.Format("insert into Order_QtyShip_Detail(ID,Seq,Article,SizeCode,Qty,AddName,AddDate,UKey) values ('{0}','01','{1}','{2}',{3},'{4}',GETDATE(),(select MIN(UKey)-1 from Order_QtyShip_Detail));", orderID, MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"]), Sci.Env.User.UserID));
                            insertCmds.Add(string.Format("insert into Order_QtyShip_Detail(ID,Seq,Article,SizeCode,Qty,OriQty,AddName,AddDate,UKey) values ('{0}','01','{1}','{2}',{3},{3},'{4}',GETDATE(),(select isnull(MIN(UKey),0)-1 from Order_QtyShip_Detail));", this.orderID, MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"]), Env.User.UserID));
                        }
                    }
                }

                foreach (DataRow dr in order_QtyShip_Detail.Rows)
                {
                    DataRow[] queryData = datas.Select(string.Format("Article = '{0}' and SizeCode = '{1}' and Qty > 0", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"])));
                    if (queryData.Length <= 0)
                    {
                        deleteCmds.Add(string.Format("delete Order_QtyShip_Detail where UKey = {0};", MyUtility.Convert.GetString(dr["UKey"])));
                    }
                }
            }
            else
            {
                insertCmds.Add(string.Format("insert into Order_QtyShip (ID,Seq,ShipmodeID,BuyerDelivery,Qty,AddName,AddDate) values ('{0}','01',(select ShipModeList from Orders where ID = '{0}'),(select BuyerDelivery from Orders where ID = '{0}'),{1},'{2}',GETDATE());", this.orderID, MyUtility.Convert.GetString(qty), Env.User.UserID));
                foreach (DataRow dr in datas.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(dr["Qty"]))
                    {
                        insertCmds.Add(string.Format("insert into Order_QtyShip_Detail(ID,Seq,Article,SizeCode,Qty,AddName,AddDate,UKey) values ('{0}','01','{1}','{2}',{3},'{4}',GETDATE(),(select isnull(MIN(UKey),0)-1 from Order_QtyShip_Detail));", this.orderID, MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"]), Env.User.UserID));
                    }
                }
            }

            updateCmds.Add(string.Format(
                @"
update oqs
	set oqs.CFAIs3rdInspect = 1
from Order_QtyShip oqs
inner join Orders o on oqs.Id = o.ID
inner join CustCD c on o.BrandID = c.BrandID and o.CustCDID = c.ID
where exists (
	select 1 
	from Orders o
	inner join CustCD c on o.BrandID = c.BrandID and o.CustCDID = c.ID
	where c.Need3rdInspect = 1
	and o.ID = oqs.Id
)
and oqs.Id = '{0}'
",
                this.orderID));

            TransactionScope transactionscope;
            try
            {
                transactionscope = Utils.CreateTransactionScope();
            }
            catch (Exception ex)
            {
                return new DualResult(false, "Create transaction error.", ex);
            }

            using (transactionscope)
            {
                if (!(result = DBProxy.Current.Batch(this.ConnectionName, tableschema, datas.ToList())))
                {
                    return result;
                }

                if (!(result = DBProxy.Current.Batch(this.ConnectionName, xtableschema, xdatas.ToList())))
                {
                    return result;
                }

                if (!(result = DBProxy.Current.Batch(this.ConnectionName, ytableschema, ydatas.ToList())))
                {
                    return result;
                }

                if (insertCmds.Count > 0)
                {
                    if (!(result = DBProxy.Current.Executes(null, insertCmds)))
                    {
                        return result;
                    }
                }

                if (updateCmds.Count > 0)
                {
                    if (!(result = DBProxy.Current.Executes(null, updateCmds)))
                    {
                        return result;
                    }
                }

                if (deleteCmds.Count > 0)
                {
                    if (!(result = DBProxy.Current.Executes(null, deleteCmds)))
                    {
                        return result;
                    }
                }

                try
                {
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    return new DualResult(false, "Commit transaction error.", ex);
                }
            }

            if (Sunrise_FinishingProcesses.IsSunrise_FinishingProcessesEnable)
            {
                Task.Run(() => new Sunrise_FinishingProcesses().SentOrdersToFinishingProcesses(this.orderID, "Orders,Order_QtyShip,Order_SizeCode,Order_Qty"))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
               Task.Run(() => new Gensong_FinishingProcesses().SentOrdersToFinishingProcesses(this.orderID, "Orders,Order_QtyShip"))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion
            return Ict.Result.True;
        }

        private void DoUndo()
        {
            this.UIConvertToView();
            this.OnRejectChanges();
        }

        private bool DoValidate()
        {
            if (!this._matrix.DoMatrixValidate())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// OnAcceptChanges
        /// </summary>
        internal virtual void OnAcceptChanges()
        {
        }

        /// <summary>
        /// OnRejectChanges
        /// </summary>
        internal virtual void OnRejectChanges()
        {
        }

        private void UIConvertToView()
        {
            this.OnUIConvertToView();
        }

        /// <summary>
        /// OnUIConvertToView
        /// </summary>
        protected virtual void OnUIConvertToView()
        {
            this.EditMode = false;
            this.EnsureButtons();

            this._matrix.IsMatrixEditable = false;
        }

        private void UIConvertToMaintain()
        {
            this.OnUIConvertToMaintain();
        }

        /// <summary>
        /// OnUIConvertToMaintain
        /// </summary>
        protected virtual void OnUIConvertToMaintain()
        {
            this.EditMode = true;
            this.EnsureButtons();
            this._matrix.IsMatrixEditable = true;
        }
        #endregion

        private void App_col_Click(object sender, EventArgs e)
        {
            this.gridLocalOrder.EndEdit();
            DualResult result;
            if (!(result = this._matrix.XAppend()))
            {
                this.ShowErr(result);
            }
        }

        private void App_row_Click(object sender, EventArgs e)
        {
            this.gridLocalOrder.EndEdit();
            DualResult result;
            if (!(result = this._matrix.YAppend()))
            {
                this.ShowErr(result);
            }
        }

        private void Ins_col_Click(object sender, EventArgs e)
        {
            this.gridLocalOrder.EndEdit();
            DualResult result;
            if (!(result = this._matrix.XInsert()))
            {
                this.ShowErr(result);
            }
        }

        private void Ins_row_Click(object sender, EventArgs e)
        {
            this.gridLocalOrder.EndEdit();
            DualResult result;
            if (!(result = this._matrix.YInsert()))
            {
                this.ShowErr(result);
            }
        }

        private void Del_col_Click(object sender, EventArgs e)
        {
            this.gridLocalOrder.EndEdit();
            DualResult result;
            if (!(result = this._matrix.XDelete()))
            {
                this.ShowErr(result);
            }
        }

        private void Del_row_Click(object sender, EventArgs e)
        {
            this.gridLocalOrder.EndEdit();
            DualResult result;
            if (!(result = this._matrix.YDelete()))
            {
                this.ShowErr(result);
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            DualResult result;

            if (!this.EditMode)
            {
                this.ToMaintain();
            }
            else
            {
                bool cancel;
                if (!(result = this.DoSave(out cancel)))
                {
                    this.ShowErr(result);
                    return;
                }

                if (cancel)
                {
                    return;
                }

                this.UIConvertToView();

                if (!(result = this.Reload()))
                {
                    this.ShowErr(result);
                }
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            DualResult result;

            if (this.EditMode)
            {
                this.DoUndo();

                if (!(result = this.Reload()))
                {
                    this.ShowErr(result);
                }
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
