using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Tools
{
    public partial class AuthorityByPosition : Sci.Win.Tems.Input6
    {
        private string sqlCmd = string.Empty;
        private DualResult result = null;
        private DataTable dtDetailMenu = null;
        private DataTable dtDetail = null;
        private ITableSchema itsPassEdit = null;
        private DataTable dtPassEdit = null;
        private DataRow newRow = null;
        private string strChangeMemo = string.Empty;
        private string ID = string.Empty;

        Ict.Win.UI.DataGridViewCheckBoxColumn ckNew = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckEdit = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckDelete = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckPrint = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckConfirm = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckUnConfirm = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckSend = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckRecall = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckCheck = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckUnCheck = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckClose = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckUnClose = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckReceive = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckReturn = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckJunk = null;
        Ict.Win.UI.DataGridViewCheckBoxColumn ckUnJunk = null;

        public AuthorityByPosition(ToolStripMenuItem menuitem) : base(menuitem)
        {
            this.InitializeComponent();

            // 新增Update function List按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Update Function List";
            btn.Click += new EventHandler(this.BtnUpdate_Click);
            this.browsetop.Controls.Add(btn);
            btn.Size = new Size(165, 30);
            btn.Enabled = Sci.Env.User.IsAdmin;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            Int64 relationKey = (e.Master == null) ? -1 : (Int64)e.Master["PKey"];

            this.sqlCmd = string.Format(
                @"SELECT A.*, B.MenuNo, B.BarNo 
                                                FROM Pass2 as A
                                                LEFT JOIN (SELECT Menu.MenuNo, MenuDetail.BarNo, MenuDetail.PKey
                                                                FROM Menu, MenuDetail 
                                                                WHERE Menu.PKey = MenuDetail.UKey) AS B 
                                                                ON A.FKMenu = B.PKey
                                                WHERE A.FKPass0 = {0}
                                                ORDER BY MenuNo, BarNo", relationKey);

            this.DetailSelectCommand = this.sqlCmd;

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.GetMenuData();
            if (this.dtDetailMenu != null)
            {
                Dictionary<String, String> filterOptions = new Dictionary<String, String>();
                DataTable dtDistinct = this.dtDetailMenu.DefaultView.ToTable(true, new string[] { "MenuName" });
                foreach (DataRow dr in dtDistinct.Rows)
                {
                    filterOptions.Add(dr["MenuName"].ToString().TrimEnd().ToUpper(), dr["MenuName"].ToString());
                }

                filterOptions.Add(string.Empty, string.Empty);
                this.comboMenuFilter.DataSource = new BindingSource(filterOptions, null);
                this.comboMenuFilter.ValueMember = "Key";
                this.comboMenuFilter.DisplayMember = "Value";
                this.comboMenuFilter.SelectedValue = string.Empty;
            }

            this.comboMenuFilter.ReadOnly = false;
        }

        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "Position", width: Widths.AnsiChars(20))
                .CheckBox("IsAdmin", header: "Admin", width: Widths.AnsiChars(1))
                .Text("PKey", header: "Key", width: Widths.AnsiChars(10))
                .Text("Description", header: "Description", width: Widths.AnsiChars(30));
            return true;
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            Ict.Win.DataGridViewGeneratorTextColumnSettings tsPosition = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            tsPosition.CellMouseDoubleClick += (s, e) =>
                {
                    if (!this.EditMode)
                    {
                        var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        if (dr == null)
                        {
                            return;
                        }

                        Sci.Production.Tools.AuthorityByPosition_Setting frm = new Sci.Production.Tools.AuthorityByPosition_Setting((Int64)this.CurrentMaintain["PKey"], dr);
                        frm.ShowDialog(this);
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            this.RenewData();
                        }
                    }
                };

            Ict.Win.DataGridViewGeneratorTextColumnSettings tsUsed = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            tsUsed.CellMouseClick += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (this.CurrentDetailData["BarPrompt"].ToString().ToUpper() == "SWITCH FACTORY")
                    {
                        this.CurrentDetailData["Used"] = "Y";
                    }
                    else
                    {
                        this.CurrentDetailData["Used"] = this.CurrentDetailData["Used"].ToString().ToUpper() == "Y" ? string.Empty : "Y";
                    }
                }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("MenuName", header: "Menu", width: Widths.AnsiChars(10), settings: tsPosition, iseditingreadonly: true)
                .Text("BarPrompt", header: "Function", width: Widths.AnsiChars(30), settings: tsPosition, iseditingreadonly: true)
                .Text("Used", header: "Used", width: Widths.AnsiChars(1), settings: tsUsed, iseditingreadonly: true, alignment: DataGridViewContentAlignment.MiddleCenter)
                .CheckBox("CanNew", header: "New", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckNew)
                .CheckBox("CanEdit", header: "Edit", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckEdit)
                .CheckBox("CanDelete", header: "Delete", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckDelete)
                .CheckBox("CanPrint", header: "Print", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckPrint)
                .CheckBox("CanConfirm", header: "Confirm", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckConfirm)
                .CheckBox("CanUnConfirm", header: "UnConfirm", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckUnConfirm)
                .CheckBox("CanSend", header: "Send", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckSend)
                .CheckBox("CanRecall", header: "Recall", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckRecall)
                .CheckBox("CanCheck", header: "Check", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckCheck)
                .CheckBox("CanUnCheck", header: "UnCheck", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckUnCheck)
                .CheckBox("CanClose", header: "Close", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckClose)
                .CheckBox("CanUnClose", header: "UnClose", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckUnClose)
                .CheckBox("CanReceive", header: "Receive", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckReceive)
                .CheckBox("CanReturn", header: "Return", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckReturn)
                .CheckBox("CanJunk", header: "Junk", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckJunk)
                .CheckBox("CanUnJunk", header: "UnJunk", width: Widths.AnsiChars(1), trueValue: 1, falseValue: 0).Get(out this.ckUnJunk);

            for (int i = 0; i < this.detailgrid.ColumnCount; i++)
            {
                this.detailgrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();

            DataRow newRow = null;
            this.dtDetail = (DataTable)this.detailgridbs.DataSource;
            this.GetMenuData();
            foreach (DataRow dr in this.dtDetailMenu.Rows)
            {
                newRow = this.dtDetail.NewRow();
                newRow["MenuName"] = dr["MenuName"].ToString();
                newRow["BarPrompt"] = dr["BarPrompt"].ToString();
                newRow["FKMenu"] = (Int64)dr["PKey"];
                if (dr["BarPrompt"].ToString().ToUpper() == "SWITCH FACTORY")
                {
                    newRow["Used"] = "Y";
                }
                else
                {
                    newRow["Used"] = string.Empty;
                }

                newRow["CanNew"] = (bool)dr["CanNew"];
                newRow["CanEdit"] = (bool)dr["CanEdit"];
                newRow["CanDelete"] = (bool)dr["CanDelete"];
                newRow["CanPrint"] = (bool)dr["CanPrint"];
                newRow["CanConfirm"] = (bool)dr["CanConfirm"];
                newRow["CanUnConfirm"] = (bool)dr["CanUnConfirm"];
                newRow["CanSend"] = (bool)dr["CanSend"];
                newRow["CanRecall"] = (bool)dr["CanRecall"];
                newRow["CanCheck"] = (bool)dr["CanCheck"];
                newRow["CanUnCheck"] = (bool)dr["CanUnCheck"];
                newRow["CanClose"] = (bool)dr["CanClose"];
                newRow["CanUnClose"] = (bool)dr["CanUnClose"];
                newRow["CanReceive"] = (bool)dr["CanReceive"];
                newRow["CanReturn"] = (bool)dr["CanReturn"];
                newRow["CanJunk"] = (bool)dr["CanJunk"];
                this.dtDetail.Rows.Add(newRow);
            }

            this.detailgrid2bs.ResetBindings(false);

            this.LockCheckBox(string.Empty);
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.LockCheckBox(string.Empty);
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.LockCheckBox(string.Empty);
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Position > can't empty !");
                this.txtPosition.Focus();
                return false;
            }

            // 修改紀錄
            Sci.Win.UI.ChangeMemo frm = new Sci.Win.UI.ChangeMemo();
            frm.ShowDialog(this);
            this.strChangeMemo = frm.returnString;

            if (!(this.result = DBProxy.Current.GetTableSchema(null, "PassEdit_History", out this.itsPassEdit)))
            {
                MyUtility.Msg.ErrorBox(this.result.ToString());
                return false;
            }

            this.newRow = this.itsPassEdit.NewRow();
            this.newRow["ID"] = this.CurrentMaintain["ID"].ToString();
            this.newRow["TableName"] = "Pass0";
            this.newRow["AddName"] = Sci.Env.User.UserID;
            this.newRow["AddDate"] = DateTime.Now;
            this.newRow["Remark"] = this.strChangeMemo;

            this.dtDetail.DefaultView.RowFilter = string.Empty;
            this.comboMenuFilter.ReadOnly = false;
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            DBProxy.Current.Insert(null, this.itsPassEdit, this.newRow);
            return base.ClickSavePost();
        }

        protected override bool ClickDeleteBefore()
        {
            if (!(this.result = DBProxy.Current.GetTableSchema(null, "PassEdit_History", out this.itsPassEdit)))
            {
                MyUtility.Msg.ErrorBox(this.result.ToString());
                return false;
            }

            if (!(this.result = DBProxy.Current.Select(null, string.Format("SELECT * FROM PassEdit_History WHERE ID = '{0}'", this.CurrentMaintain["ID"].ToString()), out this.dtPassEdit)))
            {
                MyUtility.Msg.ErrorBox(this.result.ToString());
                return false;
            }

            return base.ClickDeleteBefore();
        }

        protected override DualResult ClickDeletePost()
        {
            foreach (DataRow dr in this.dtPassEdit.Rows)
            {
                DBProxy.Current.Delete(null, this.itsPassEdit, dr);
            }

            return base.ClickDeletePost();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.comboMenuFilter.SelectedValue = string.Empty;
            this.comboMenuFilter.ReadOnly = false;
            this.ID = this.CurrentMaintain["ID"].ToString();
        }

        // 檢查Position是否重複
        private void txtPosition_Validating(object sender, CancelEventArgs e)
        {
            // 先記錄原本的Position,排除自己後check輸入的是否存在DB
            if (this.ID.ToString().ToUpper() != this.txtPosition.Text.ToUpper())
            {
                if (MyUtility.Check.Seek(string.Format("SELECT ID FROM Pass0 WHERE ID = '{0}'", this.txtPosition.Text)))
                {
                    MyUtility.Msg.WarningBox(string.Format("<Position: {0}> has existed !", this.txtPosition.Text));
                    e.Cancel = true;
                }
            }
        }

        // Update Pass2
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Msg.QuestionBox("Do you want to do it ?") == DialogResult.Yes)
            {
                try
                {
                    // 執行批次修改Pass1 Position && Pass2
                    DualResult dResult;
                    dResult = DBProxy.Current.Execute(null, "exec Update_PassFunction");
                    if (!dResult.IsEmpty)
                    {
                        MyUtility.Msg.ErrorBox("Update pass2 failed, Pleaes re-try");
                        return;
                    }
                    else
                    {
                        MyUtility.Msg.InfoBox("Update successful");
                    }
                }
                catch (Exception ex)
                {
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }

                #region old Code

                // DataTable dtPass0 = null;
                // DataTable dtPass1 = null;
                // DataTable dtPass2 = null;
                // ITableSchema tsPass1 = null;
                // ITableSchema tsPass2 = null;
                ////DataRow newRow = null;
                ////DataRow[] drs = null;
                ////bool different = false;

                // if (!(result = DBProxy.Current.Select(null, "SELECT PKey, ID FROM Pass0", out dtPass0)))
                // {
                //    MyUtility.Msg.ErrorBox(result.ToString());
                //    return;
                // }
                // if (!(result = DBProxy.Current.Select(null, "SELECT * FROM Pass1", out dtPass1)))
                // {
                //    MyUtility.Msg.ErrorBox(result.ToString());
                //    return;
                // }
                // if (!(result = DBProxy.Current.Select(null, "SELECT * FROM Pass2", out dtPass2)))
                // {
                //    MyUtility.Msg.ErrorBox(result.ToString());
                //    return;
                // }
                // if (!(result = DBProxy.Current.GetTableSchema(null, "Pass1", out tsPass1)))
                // {
                //    MessageBox.Show(result.ToString());
                //    return;
                // }
                // if (!(result = DBProxy.Current.GetTableSchema(null, "Pass2", out tsPass2)))
                // {
                //    MessageBox.Show(result.ToString());
                //    return;
                // }
                //// 更新Pass1的Position
                // foreach (DataRow drPass0 in dtPass0.Rows)
                // {
                //    drs = dtPass1.Select(string.Format("FKPass0 = {0}", (Int64)drPass0["PKey"]));
                //    foreach (DataRow dr in drs)
                //    {
                //        if (drPass0["ID"].ToString() != dr["Position"].ToString())
                //        {
                //            dr["Position"] = drPass0["ID"].ToString();
                //        }
                //    }
                // }

                // GetMenuData();

                //// 刪除Pass2內不存在於Menu的資料
                // IList<DataRow> listPass2 = dtPass2.ToList<DataRow>();
                // foreach (DataRow drPass2 in listPass2)
                // {
                //    drs = dtDetailMenu.Select(string.Format("PKey = {0}", (Int64)drPass2["FKMenu"]));
                //    if (drs.Length == 0)
                //    {
                //        drPass2.Delete();
                //    }
                // }

                //// 新增Menu有但不存在於Pass2的資料 / 維護BarPrompt及權限設定與Menu同步
                // foreach (DataRow drPass0 in dtPass0.Rows)
                // {
                //    //DataRow[] dt = dtPass2.Select(string.Format("FKPass0 = {0}", (Int64)drPass0["PKey"]));
                //    foreach (DataRow drDetailMenu in dtDetailMenu.Rows)
                //    {
                //        drs = dtPass2.Select(string.Format("FKPass0 = {0} AND FKMenu = {1}", (Int64)drPass0["PKey"], (Int64)drDetailMenu["PKey"]));
                //       // drs = dt.CopyToDataTable().Select(string.Format("FKMenu = {0}", (Int64)drDetailMenu["PKey"]));
                //        if (drs.Length > 0)
                //        {
                //            foreach (DataRow dr in drs)
                //            {
                //                if (dr["MenuName"].ToString() != drDetailMenu["MenuName"].ToString()) dr["MenuName"] = drDetailMenu["MenuName"].ToString();
                //                if (dr["BarPrompt"].ToString() != drDetailMenu["BarPrompt"].ToString()) dr["BarPrompt"] = drDetailMenu["BarPrompt"].ToString();
                //                if (!(bool)drDetailMenu["CanNew"] && (bool)dr["CanNew"]) dr["CanNew"] = 0;
                //                if (!(bool)drDetailMenu["CanEdit"] && (bool)dr["CanEdit"]) dr["CanEdit"] = 0;
                //                if (!(bool)drDetailMenu["CanDelete"] && (bool)dr["CanDelete"]) dr["CanDelete"] = 0;
                //                if (!(bool)drDetailMenu["CanPrint"] && (bool)dr["CanPrint"]) dr["CanPrint"] = 0;
                //                if (!(bool)drDetailMenu["CanConfirm"] && (bool)dr["CanConfirm"]) dr["CanConfirm"] = 0;
                //                if (!(bool)drDetailMenu["CanUnConfirm"] && (bool)dr["CanUnConfirm"]) dr["CanUnConfirm"] = 0;
                //                if (!(bool)drDetailMenu["CanSend"] && (bool)dr["CanSend"]) dr["CanSend"] = 0;
                //                if (!(bool)drDetailMenu["CanRecall"] && (bool)dr["CanRecall"]) dr["CanRecall"] = 0;
                //                if (!(bool)drDetailMenu["CanCheck"] && (bool)dr["CanCheck"]) dr["CanCheck"] = 0;
                //                if (!(bool)drDetailMenu["CanUnCheck"] && (bool)dr["CanUnCheck"]) dr["CanUnCheck"] = 0;
                //                if (!(bool)drDetailMenu["CanClose"] && (bool)dr["CanClose"]) dr["CanClose"] = 0;
                //                if (!(bool)drDetailMenu["CanUnClose"] && (bool)dr["CanUnClose"]) dr["CanUnClose"] = 0;
                //                if (!(bool)drDetailMenu["CanReceive"] && (bool)dr["CanReceive"]) dr["CanReceive"] = 0;
                //                if (!(bool)drDetailMenu["CanReturn"] && (bool)dr["CanReturn"]) dr["CanReturn"] = 0;
                //                if (!(bool)drDetailMenu["CanJunk"] && (bool)dr["CanJunk"]) dr["CanJunk"] = 0;
                //            }
                //        }
                //        else
                //        {
                //            newRow = dtPass2.NewRow();
                //            newRow["FKPass0"] = (Int64)drPass0["PKey"];
                //            newRow["FKMenu"] = (Int64)drDetailMenu["PKey"];
                //            newRow["MenuName"] = drDetailMenu["MenuName"].ToString();
                //            newRow["BarPrompt"] = drDetailMenu["BarPrompt"].ToString();
                //            newRow["CanNew"] = (bool)drDetailMenu["CanNew"];
                //            newRow["CanEdit"] = (bool)drDetailMenu["CanEdit"];
                //            newRow["CanDelete"] = (bool)drDetailMenu["CanDelete"];
                //            newRow["CanPrint"] = (bool)drDetailMenu["CanPrint"];
                //            newRow["CanConfirm"] = (bool)drDetailMenu["CanConfirm"];
                //            newRow["CanUnConfirm"] = (bool)drDetailMenu["CanUnConfirm"];
                //            newRow["CanSend"] = (bool)drDetailMenu["CanSend"];
                //            newRow["CanRecall"] = (bool)drDetailMenu["CanRecall"];
                //            newRow["CanCheck"] = (bool)drDetailMenu["CanCheck"];
                //            newRow["CanUnCheck"] = (bool)drDetailMenu["CanUnCheck"];
                //            newRow["CanClose"] = (bool)drDetailMenu["CanClose"];
                //            newRow["CanUnClose"] = (bool)drDetailMenu["CanUnClose"];
                //            newRow["CanReceive"] = (bool)drDetailMenu["CanReceive"];
                //            newRow["CanReturn"] = (bool)drDetailMenu["CanReturn"];
                //            newRow["CanJunk"] = (bool)drDetailMenu["CanJunk"];
                //            dtPass2.Rows.Add(newRow);
                //        }
                //    }
                // }

                // TransactionScope _transactionscope = new TransactionScope();
                // using (_transactionscope)
                // {
                //    try
                //    {
                //        // 更新Pass1異動
                //        foreach (DataRow drPass1 in dtPass1.Rows)
                //        {
                //            if (drPass1.RowState == DataRowState.Modified)
                //            {
                //                result = DBProxy.Current.UpdateByChanged(null, tsPass1, drPass1, out different);
                //            }
                //            if (!result.IsEmpty)
                //            {
                //                _transactionscope.Dispose();
                //                MyUtility.Msg.ErrorBox("Update pass1 failed, Pleaes re-try");
                //                return;
                //            }
                //        }
                //        int count = 0;
                //        // 更新Pass2異動
                //        foreach (DataRow drPass2 in dtPass2.Rows)
                //        {
                //            if (drPass2.RowState == DataRowState.Deleted)
                //            {
                //                result = DBProxy.Current.Delete(null, tsPass2, drPass2);
                //                count++;
                //            }
                //            if (drPass2.RowState == DataRowState.Added)
                //            {
                //                result = DBProxy.Current.Insert(null, tsPass2, drPass2);
                //                count++;
                //            }
                //            if (drPass2.RowState == DataRowState.Modified)
                //            {
                //                result = DBProxy.Current.UpdateByChanged(null, tsPass2, drPass2, out different);
                //                count++;
                //            }

                // if (!result.IsEmpty)
                //            {
                //                MyUtility.Msg.InfoBox("連線數: "+count);
                //                _transactionscope.Dispose();
                //                MyUtility.Msg.ErrorBox("Update pass2 failed, Pleaes re-try");
                //                return;
                //            }
                //        }
                //        _transactionscope.Complete();
                //        _transactionscope.Dispose();
                //        MyUtility.Msg.InfoBox("Update successful");
                //    }
                //    catch (Exception ex)
                //    {
                //        _transactionscope.Dispose();
                //        MyUtility.Msg.ErrorBox("Update transaction error.\n" + ex);
                //    }
                // }
                #endregion

                this.ReloadDatas();
            }
        }

        // 根據DetailMenu權限控制Grid CheckBox Cell是否能編輯
        protected void LockCheckBox(String menuOption)
        {
            this.dtDetail = (DataTable)this.detailgridbs.DataSource;
            if (this.dtDetail == null)
            {
                return;
            }

            this.dtDetail.DefaultView.RowFilter = MyUtility.Check.Empty(menuOption) ? string.Empty : string.Format("MenuName = '{0}'", menuOption);
            if (this.dtDetail.DefaultView.Count == 0)
            {
                return;
            }

            this.GetMenuData();
            DataRow[] drs = null;
            int index = 0;
            foreach (DataRowView dr in this.dtDetail.DefaultView)
            {
                drs = this.dtDetailMenu.Select(string.Format("PKey = {0}", (Int64)dr["FKMenu"]));
                this.detailgrid.Rows[index].Cells[this.ckNew.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanNew"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckEdit.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanEdit"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckDelete.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanDelete"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckPrint.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanPrint"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckConfirm.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanConfirm"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckUnConfirm.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanUnConfirm"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckSend.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanSend"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckRecall.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanRecall"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckCheck.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanCheck"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckUnCheck.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanUnCheck"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckClose.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanClose"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckUnClose.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanUnClose"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckReceive.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanReceive"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckReturn.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanReturn"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckJunk.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanJunk"]) : true;
                this.detailgrid.Rows[index].Cells[this.ckUnJunk.Index].ReadOnly = (drs.Length > 0) ? !((bool)drs[0]["CanUnJunk"]) : true;
                index = index + 1;
            }
        }

        private void GetMenuData()
        {
            this.sqlCmd = @"SELECT Menu.MenuName, Menu.MenuNo, MenuDetail.*
                             FROM Menu, MenuDetail
                             WHERE Menu.PKey = MenuDetail.UKey AND MenuDetail.ForMISOnly = 0 AND MenuDetail.ObjectCode = 0
                             ORDER BY Menu.MenuNo, MenuDetail.BarNo";

            if (!(this.result = DBProxy.Current.Select(null, this.sqlCmd, out this.dtDetailMenu)))
            {
                MyUtility.Msg.ErrorBox(this.result.ToString());
                return;
            }
        }

        // Modify History
        private void btnModifyHistory_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            Sci.Production.Tools.AuthorityByPosition_History frm = new Sci.Production.Tools.AuthorityByPosition_History(true, this.CurrentMaintain["ID"].ToString(), null, null);
            frm.Show(this);
        }

        // ComboBox SelectChange
        private void comboMenuFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if (this.EditMode)
            // {
            this.LockCheckBox(this.comboMenuFilter.SelectedValue.ToString());

            // }
        }

        // Print
        protected override bool ClickPrint()
        {
            DataTable dtExcel;
            DualResult result;
            string cmd = @"
select b.ID
,a.MenuName
,BarPrompt
,Used
,[New] =		iif(CanNew=0,'','Y')
,[Edit]=		iif(CanEdit=0,'','Y')
,[Delete] =		iif(CanDelete=0,'','Y')
,[Print] =		iif(CanPrint=0,'','Y')
,[Confirm] =	iif(CanConfirm=0,'','Y')
,[UnConfirm] =	iif(CanUnConfirm=0,'','Y')
,[Send] =		iif(CanSend=0,'','Y')
,[Recall] =		iif(CanRecall=0,'','Y')
,[Check] =		iif(CanCheck=0,'','Y')
,[UnCheck] =	iif(CanUnCheck=0,'','Y')
,[Close] =		iif(CanClose=0,'','Y')
,[UnClose] =	iif(CanUnClose=0,'','Y')
,[Receive] =	iif(CanReceive=0,'','Y')
,[Return] =		iif(CanReturn=0,'','Y')
,[Junk] =		iif(CanJunk=0,'','Y')
from Pass2 a
inner join pass0 b on a.FKPass0=b.PKey
order by b.pkey,a.MenuName,BarPrompt";
            if (!(result = DBProxy.Current.Select(null, cmd, out dtExcel)))
            {
                return result;
            }

            if (MyUtility.Check.Empty(dtExcel) || dtExcel.Rows.Count < 1)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "AuthorityByPosition.xltx");
            Microsoft.Office.Interop.Excel._Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 1] = "(" + DBProxy.Current.DefaultModuleName + ")" + " Authority By Position";
            MyUtility.Excel.CopyToXls(dtExcel, string.Empty, "AuthorityByPosition.xltx", 2, false, null, objApp);

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("AuthorityByPosition");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion

            return base.ClickPrint();
        }
    }
}
