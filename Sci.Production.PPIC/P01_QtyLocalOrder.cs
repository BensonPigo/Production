using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System.Transactions;

namespace Sci.Production.PPIC
{
    public partial class P01_QtyLocalOrder : Sci.Win.Subs.Base
    {
        DataTable SizeCode, Article, QtyBDown;
        string orderID;
        bool editable;
        int orderQty;
        MatrixHelper _matrix;
        public P01_QtyLocalOrder(string OrderID, bool Editable, int OrderQty)
        {
            InitializeComponent();
            orderID = OrderID;
            editable = Editable;
            orderQty = OrderQty;


            // 按鈕對應的事件方法.
            button1.Click += app_col_Click;
            button2.Click += app_row_Click;
            button3.Click += ins_col_Click;
            button4.Click += ins_row_Click;
            button5.Click += del_col_Click;
            button6.Click += del_row_Click;
            button7.Click += edit_Click;
            button8.Click += close_Click;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            _matrix = new MatrixHelper(this, grid1, listControlBindingSource1); // 建立 Matrix 物件
            _matrix.XMap.Name = "SizeCode";  // 對應到第三表格的 X 欄位名稱
            _matrix.YMap.Name = "Article";  // 對應到第三表格的 Y 欄位名稱

            _matrix.XUniqueKey = "SizeCode"; // X 軸不可重複的欄位名稱, 可多重欄位, 用","區隔.
            _matrix.YUniqueKey = "Article"; // Y 軸不可重複的欄位名稱, 可多重欄位, 用","區隔.

            _matrix
                .SetColDef("NewQty", width: Widths.AnsiChars(6))  // 第三表格對應的欄位名稱
                .AddXColDef("SizeCode")                             // X 要顯示的欄位名稱, 可設定多個.
                .AddYColDef("Total", header: "Total", width: Widths.AnsiChars(6))  // Y 要顯示的欄位名稱, 可設定多個.
                .AddYColDef("Article", header: "Colorway", width: Widths.UnicodeChars(8))
                ;
            #region Grid控制
            grid1.EditingControlShowing += (s, e) =>
                {
                    //Total欄位不可以被修改
                    if (grid1.CurrentCellAddress.X == 0)
                    {
                        e.Control.Enabled = false;
                    }
                    //限制欄位只能輸入數值
                    if (EditMode == true)
                    {
                        if (grid1.CurrentCellAddress.Y >= 1 && grid1.CurrentCellAddress.X >= 2)
                        {
                            ((Ict.Win.UI.TextBox)e.Control).InputRestrict = Ict.Win.UI.TextBoxInputsRestrict.Digit;
                        }
                    }
                };

            //Total欄位值顯是為黑色
            grid1.CellFormatting += (s, e) =>
                {
                    if (e.ColumnIndex == 0)
                    {
                        e.CellStyle.ForeColor = Color.Black;
                    }
                };
            
            //值修改後要加總回Total
            grid1.CellValueChanged += (s, e) =>
                {
                    DataRow dr = grid1.GetDataRow<DataRow>(e.RowIndex);
                    int sum = 0;
                    if (e.RowIndex >= 1)
                    {
                        foreach (DataGridViewColumn dg in grid1.Columns)
                        {
                            if (!_matrix.IsGridYColumn(dg.DisplayIndex))
                            {
                                sum += MyUtility.Convert.GetInt(dr[dg.DataPropertyName]);
                            }
                        }
                        dr[0] = sum;

                        grid1.InvalidateRow(e.RowIndex);
                    }
                };
            #endregion

            _matrix.IsXColEditable = true;  // X 顯示的欄位可否編輯?
            _matrix.IsYColEditable = true;  // Y 顯示的欄位可否編輯?

            DualResult result;
           
            UIConvertToView();

            if (!(result = Reload()))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }
        }

        protected override void OnFormDispose()
        {
            if (null != listControlBindingSource1) listControlBindingSource1.Dispose();
            base.OnFormDispose();
        }

        #region 應用程式
        private DualResult Reload()
        {
            DualResult result;

            _matrix.Clear();

            string sqlCmd = string.Format("select * from Order_SizeCode where ID = '{0}' order by Seq", orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out SizeCode);
            sqlCmd = string.Format("select *, isnull((select SUM(Qty) from Order_Qty where ID = oa.ID and Article = oa.Article),0) as Total from Order_Article oa where oa.ID = '{0}' order by Seq", orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out Article);
            sqlCmd = string.Format("select *,CONVERT(varchar(6),Qty) as NewQty from Order_Qty where ID = '{0}'", orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out QtyBDown);

            if (!(result = _matrix.Sets(QtyBDown, SizeCode, Article))) return result;

            return Result.True;
        }

        private void EnsureButtons()
        {
            if (EditMode)
            {
                button7.Text = "Save";
                button8.Text = "Undo";
            }
            else
            {
                button7.Text = "Edit";
                button8.Text = "Close";
            }
            if (!editable)
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
            }

        }

        private void ToMaintain()
        {
            UIConvertToMaintain();
            OnAcceptChanges();
        }

        private DualResult DoSave(out bool cancel)
        {
            cancel = false;
            DualResult result;

            if (!DoValidate())
            {
                cancel = true;
                return Result.True;
            }

            DataTable datas, xdatas, ydatas;
            if (!(result = _matrix.GetDatas(out datas, out xdatas, out ydatas))) return result;

            int qty = 0;
            foreach (DataRow dr in datas.Rows)
            {
                if (DataRowState.Added == dr.RowState)
                {
                    dr["ID"] = orderID;
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
                    if (DataRowState.Added == dr.RowState)
                    {
                        dr["ID"] = orderID;
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
                    if (DataRowState.Added == dr.RowState)
                    {
                        dr["ID"] = orderID;
                    }
                    if (MyUtility.Convert.GetString(dr["Seq"]) != MyUtility.Convert.GetString(seq))
                    {
                        dr["Seq"] = MyUtility.Convert.GetString(seq);
                    }
                }
            }

            ITableSchema tableschema, xtableschema, ytableschema;
            if (!(result = DBProxy.Current.GetTableSchema(null, "Order_SizeCode", out xtableschema))) return result;
            if (!(result = DBProxy.Current.GetTableSchema(null, "Order_Article", out ytableschema))) return result;
            if (!(result = DBProxy.Current.GetTableSchema(null, "Order_Qty", out tableschema))) return result;

            //準備Order_QtyShip, Order_QtyShip_Detail資料
            IList<string> insertCmds = new List<string>();
            IList<string> updateCmds = new List<string>();
            IList<string> deleteCmds = new List<string>();
            if (orderQty != qty)
            {
                updateCmds.Add(string.Format("update Orders set Qty = {0} where ID = '{1}'", MyUtility.Convert.GetString(qty), orderID));
            }
            if (MyUtility.Check.Seek(string.Format("select ID from Order_QtyShip where ID = '{0}'", orderID)))
            {
                string ttlQty = MyUtility.GetValue.Lookup(string.Format("select Qty from Order_QtyShip where ID = '{0}'", orderID));
                if (MyUtility.Convert.GetInt(ttlQty) != qty)
                {
                    //updateCmds.Add(string.Format("update Order_QtyShip set Qty = {0}, EditName = '{1}', EditDate = GETDATE() where ID = '{2}';", MyUtility.Convert.GetString(qty), Sci.Env.User.UserID, orderID));
                    updateCmds.Add(string.Format("update Order_QtyShip set Qty = {0}, OriQty = {0}, EditName = '{1}', EditDate = GETDATE() where ID = '{2}';", MyUtility.Convert.GetString(qty), Sci.Env.User.UserID, orderID));
                }
                DataTable Order_QtyShip_Detail;
                if (!(result = DBProxy.Current.Select(null, string.Format("select * from Order_QtyShip_Detail where ID = '{0}'", orderID), out Order_QtyShip_Detail))) return result;
                foreach (DataRow dr in datas.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(dr["Qty"]))
                    {
                        DataRow[] queryData = Order_QtyShip_Detail.Select(string.Format("Article = '{0}' and SizeCode = '{1}'", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"])));
                        if (queryData.Length > 0)
                        {
                            if (MyUtility.Convert.GetInt(queryData[0]["Qty"]) != MyUtility.Convert.GetInt(dr["Qty"]))
                            {
                                //updateCmds.Add(string.Format("update Order_QtyShip_Detail set Qty = {0}, EditName = '{1}', EditDate = GETDATE() where UKey = {2};", MyUtility.Convert.GetString(dr["Qty"]), Sci.Env.User.UserID, MyUtility.Convert.GetString(queryData[0]["UKey"])));
                                updateCmds.Add(string.Format("update Order_QtyShip_Detail set Qty = {0}, OriQty = {0}, EditName = '{1}', EditDate = GETDATE() where UKey = {2};", MyUtility.Convert.GetString(dr["Qty"]), Sci.Env.User.UserID, MyUtility.Convert.GetString(queryData[0]["UKey"])));
                            }
                        }
                        else
                        {
                            //insertCmds.Add(string.Format("insert into Order_QtyShip_Detail(ID,Seq,Article,SizeCode,Qty,AddName,AddDate,UKey) values ('{0}','01','{1}','{2}',{3},'{4}',GETDATE(),(select MIN(UKey)-1 from Order_QtyShip_Detail));", orderID, MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"]), Sci.Env.User.UserID));
                            insertCmds.Add(string.Format("insert into Order_QtyShip_Detail(ID,Seq,Article,SizeCode,Qty,OriQty,AddName,AddDate,UKey) values ('{0}','01','{1}','{2}',{3},{3},'{4}',GETDATE(),(select MIN(UKey)-1 from Order_QtyShip_Detail));", orderID, MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"]), Sci.Env.User.UserID));
                        }
                    }
                }
                foreach (DataRow dr in Order_QtyShip_Detail.Rows)
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
                insertCmds.Add(string.Format("insert into Order_QtyShip (ID,Seq,ShipmodeID,BuyerDelivery,Qty,AddName,AddDate) values ('{0}','01',(select ShipModeList from Orders where ID = '{0}'),(select BuyerDelivery from Orders where ID = '{0}'),{1},'{2}',GETDATE());", orderID, MyUtility.Convert.GetString(qty), Sci.Env.User.UserID));
                foreach (DataRow dr in datas.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(dr["Qty"]))
                    {
                        insertCmds.Add(string.Format("insert into Order_QtyShip_Detail(ID,Seq,Article,SizeCode,Qty,AddName,AddDate,UKey) values ('{0}','01','{1}','{2}',{3},'{4}',GETDATE(),(select MIN(UKey)-1 from Order_QtyShip_Detail));", orderID, MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"]), Sci.Env.User.UserID));
                    }
                }
            }

            TransactionScope transactionscope;
            try { transactionscope = Utils.CreateTransactionScope(); }
            catch (Exception ex) { return new DualResult(false, "Create transaction error.", ex); }
            using (transactionscope)
            {
                if (!(result = DBProxy.Current.Batch(ConnectionName, tableschema, datas.ToList()))) return result;
                if (!(result = DBProxy.Current.Batch(ConnectionName, xtableschema, xdatas.ToList()))) return result;
                if (!(result = DBProxy.Current.Batch(ConnectionName, ytableschema, ydatas.ToList()))) return result;
                if (insertCmds.Count > 0)
                {
                    if (!(result = DBProxy.Current.Executes(null, insertCmds))) return result;
                }
                if (updateCmds.Count > 0)
                {
                    if (!(result = DBProxy.Current.Executes(null, updateCmds))) return result;
                }
                if (deleteCmds.Count > 0)
                {
                    if (!(result = DBProxy.Current.Executes(null, deleteCmds))) return result;
                }

                try { transactionscope.Complete(); }
                catch (Exception ex) { return new DualResult(false, "Commit transaction error.", ex); }
            }

            return Result.True;
        }

        private void DoUndo()
        {
            UIConvertToView();
            OnRejectChanges();
        }

        private bool DoValidate()
        {
            if (!_matrix.DoMatrixValidate()) return false;

            return true;
        }

        virtual internal void OnAcceptChanges()
        {
        }

        virtual internal void OnRejectChanges()
        {
        }

        private void UIConvertToView()
        {
            OnUIConvertToView();
        }

        virtual protected void OnUIConvertToView()
        {
            EditMode = false;
            EnsureButtons();

            _matrix.IsMatrixEditable = false;
        }

        private void UIConvertToMaintain()
        {
            OnUIConvertToMaintain();
        }

        virtual protected void OnUIConvertToMaintain()
        {
            EditMode = true;
            EnsureButtons();

            _matrix.IsMatrixEditable = true;
        }
        #endregion

        void app_col_Click(object sender, EventArgs e)
        {
            grid1.EndEdit();
            DualResult result;
            if (!(result = _matrix.XAppend()))
            {
                ShowErr(result);
            }
        }

        void app_row_Click(object sender, EventArgs e)
        {
            grid1.EndEdit();
            DualResult result;
            if (!(result = _matrix.YAppend()))
            {
                ShowErr(result);
            }
        }

        void ins_col_Click(object sender, EventArgs e)
        {
            grid1.EndEdit();
            DualResult result;
            if (!(result = _matrix.XInsert()))
            {
                ShowErr(result);
            }
        }

        void ins_row_Click(object sender, EventArgs e)
        {
            grid1.EndEdit();
            DualResult result;
            if (!(result = _matrix.YInsert()))
            {
                ShowErr(result);
            }
        }

        void del_col_Click(object sender, EventArgs e)
        {
            grid1.EndEdit();
            DualResult result;
            if (!(result = _matrix.XDelete()))
            {
                ShowErr(result);
            }
        }

        void del_row_Click(object sender, EventArgs e)
        {
            grid1.EndEdit();
            DualResult result;
            if (!(result = _matrix.YDelete()))
            {
                ShowErr(result);
            }
        }

        void edit_Click(object sender, EventArgs e)
        {
            DualResult result;

            if (!EditMode)
            {
                ToMaintain();
            }
            else
            {
                bool cancel;
                if (!(result = DoSave(out cancel)))
                {
                    ShowErr(result);
                    return;
                }
                if (cancel) return;

                UIConvertToView();

                if (!(result = Reload()))
                {
                    ShowErr(result);
                }
            }
        }

        void close_Click(object sender, EventArgs e)
        {
            DualResult result;

            if (EditMode)
            {
                DoUndo();

                if (!(result = Reload()))
                {
                    ShowErr(result);
                }
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
