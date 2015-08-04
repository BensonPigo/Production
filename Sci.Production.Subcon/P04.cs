using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.PublicPrg;
using System.Linq;
using System.Transactions;

namespace Sci.Production.Subcon
{
    public partial class P04 : Sci.Win.Tems.Input6
    {
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("FactoryID = '{0}'", Sci.Env.User.Factory);
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["bundleno"] = "";
            CurrentDetailData["qty"] = 0;
        }

        private void txtartworktype_fty1_Validated(object sender, EventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();
            }
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
            CurrentMaintain["Status"] = "New";

        }

        // delete前檢查

        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is Confirmed, can't be deleted.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString() == "Confirmed")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("farmout", "remark", dr);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            detailgridbs.EndEdit();

            #region 必輸檢查
            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            if (CurrentMaintain["ArtworktypeId"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["ArtworktypeId"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Artwork Type >  can't be empty!", "Warning");
                txtartworktype_fty1.Focus();
                return false;
            }

            if (CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                txtuser1.TextBox1.Focus();
                return false;
            }
            #endregion

            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("qty = 0"))
            {
                ((DataTable)detailgridbs.DataSource).Rows.Remove(row);
            }


            if (((DataTable)detailgridbs.DataSource).Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            DataRow[] drchk = ((DataTable)detailgridbs.DataSource).Select("[orderid] ='' or [artworkpoid] ='' or [orderid] is null or [artworkpoid] is null ");
            if (drchk.Length > 0)
            {
                MyUtility.Msg.WarningBox("Detail of SP# & POID can't be empty", "Warning");
                return false;
            }


            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)
            if (this.IsDetailInserting)
            {
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "FI", "FarmOut", (DateTime)CurrentMaintain["issuedate"]);
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                (e.Details).Columns.Add("StyleId", typeof(String));
                (e.Details).Columns.Add("Variance", typeof(decimal));
                (e.Details).Columns.Add("BalQty", typeof(decimal));

                foreach (DataRow dr in e.Details.Rows)
                {
                    dr["Variance"] = (decimal)dr["artworkpoqty"] - (decimal)dr["onhand"];
                    dr["BalQty"] = (decimal)dr["artworkpoqty"] - (decimal)dr["onhand"] - (decimal)dr["qty"];
                    DataTable order_dt;
                    DBProxy.Current.Select(null, string.Format("select styleid, sewinline, scidelivery from orders where id='{0}'", dr["orderid"].ToString()), out order_dt);
                    if (order_dt.Rows.Count == 0)
                        continue;
                    dr["StyleId"] = order_dt.Rows[0]["styleid"].ToString();

                }
            }
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            txtartworktype_fty1.Enabled = !this.EditMode || IsDetailInserting;
            #region Status Label
            label25.Text = CurrentMaintain["Status"].ToString();
            #endregion
            #region Batch Import
            button2.Enabled = this.EditMode;
            #endregion

        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region orderid帶styleid
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellValidating += (s, e) =>
            {

                if (!(this.EditMode) || this.IsDetailInserting) return;
                if (!(MyUtility.Check.Empty(e.FormattedValue)) && e.FormattedValue.ToString().TrimEnd() != ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex)["orderid"].ToString().TrimEnd())
                {
                    CurrentDetailData["styleid"] = MyUtility.Check.Empty(string.Format("select styleid from orders where id ='{0}'", e.FormattedValue));
                    CurrentDetailData["artworkid"] = "";
                    CurrentDetailData["patterndesc"] = "";
                    CurrentDetailData["artworkpoqty"] = 0;
                    CurrentDetailData["onhand"] = 0;
                    CurrentDetailData["ArtworkPo_DetailUkey"] = DBNull.Value;
                    CurrentDetailData["qty"] = 0;
                }
            };
            #endregion

            #region POID#右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(CurrentMaintain["artworktypeid"]))
                    {
                        MyUtility.Msg.WarningBox("Please fill Artwork Type first");
                        this.txtartworktype_fty1.Focus();
                        return;
                    }

                    if (MyUtility.Check.Empty(dr["OrderID"]))
                    {
                        MyUtility.Msg.WarningBox("Please fill SP# first");
                        return;
                    }
                    if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
                    {

                        string sqlcmd = string.Format(@"SELECT B.id
                                                        ,b.artworktypeid
                                                        ,b.artworkid
                                                        ,b.patterncode
                                                        ,b.patterndesc
                                                        ,b.poqty
                                                        ,b.farmout
                                                        ,b.ukey
                                                        FROM ArtworkPO A, ArtworkPO_Detail B
                                                        WHERE A.ID = B.ID
                                                        AND A.ApvName IS NOT NULL
                                                        AND A.Closed = 0
                                                        AND B.OrderID ='{0}'
                                                        AND B.ArtworkTypeID = '{1}' order by B.ID", dr["OrderID"].ToString(), CurrentMaintain["artworktypeid"].ToString());
                        Sci.Win.Tools.SelectItem item
                            = new Sci.Win.Tools.SelectItem(sqlcmd, "13,13,13,13,15,5,5,0", dr["artworkpoID"].ToString(), "POID,Artwork Type,Artwork,Cutpart,Cutpart Name,PoQty,FarmOut,Ukey");
                        item.Width = 1024;
                        item.Height = 480;

                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) { return; }
                        dr["artworkpoID"] = item.GetSelectedString();
                        IList<DataRow> selectedData = item.GetSelecteds();
                        if (selectedData.Count > 0)
                        {
                            dr["artworkid"] = (selectedData[0])["artworkid"].ToString();
                            dr["patterncode"] = (selectedData[0])["patterncode"].ToString();
                            dr["patterndesc"] = (selectedData[0])["patterndesc"].ToString();
                            dr["artworkpoqty"] = (selectedData[0])["poqty"].ToString();
                            dr["onhand"] = (selectedData[0])["farmout"];
                            dr["ArtworkPo_DetailUkey"] = (selectedData[0])["ukey"].ToString();
                            dr["qty"] = (decimal)(selectedData[0])["poqty"] - (decimal)(selectedData[0])["farmout"];
                        }
                    }
                }
            };
            #endregion
            MyUtility.Tool.SetGridFrozen(this.detailgrid);
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("BundleNo", header: "Bundle No", width: Widths.AnsiChars(10), iseditingreadonly: true)  //0
            .CellOrderId("OrderID", header: "SP#", width: Widths.AnsiChars(13), settings: ts)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(6), iseditingreadonly: false)    //2
            .Text("ArtworkPoID", header: "POID", settings: ts4, iseditingreadonly: true, width: Widths.AnsiChars(13))   //3
            .Text("ArtworkId", header: "Artwork", iseditingreadonly: false)   //4
            .Text("PatternCode", header: "Cutpart ID", iseditingreadonly: false)    //5
            .Text("PatternDesc", header: "Cutpart Name", iseditingreadonly: false)//6
            .Numeric("ArtworkPoQty", header: "P/O Qty", width: Widths.AnsiChars(5), iseditingreadonly: false)    //7
            .Numeric("OnHand", header: "Accum. Qty", width: Widths.AnsiChars(5), iseditingreadonly: false) //8
            .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(5), iseditingreadonly: false)   //9
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5))     //10
            .Numeric("BalQty", header: "Bal. Qty", width: Widths.AnsiChars(5), iseditingreadonly: true);  //11

            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns[3].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (!(Prgs.GetAuthority(Env.User.UserID) && CurrentMaintain["handle"].ToString() != Env.User.UserID))
            {
                MyUtility.Msg.WarningBox("Only Handle & Leader have authority to Confirm");
                return;
            }
            String sqlcmd, sqlcmd2 = "", sqlcmd3 = "";
            DualResult result, result2;
            DataTable datacheck;
            sqlcmd = string.Format(@"select a.id from artworkpo a, farmin_detail b 
                            where a.id = b.artworkpoid and a.status = 'Closed' and b.id = '{0}'", CurrentMaintain["id"]);

            string ids = "", bundlenos = "";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    ids += dr[0].ToString() + ",";
                }
                MyUtility.Msg.WarningBox(String.Format("These POID <{0}> already closed, can't Confirmed", ids));
                return;
            }


            // 提示是否超過Farm out qty 

            ids = "";
            foreach (var dr in DetailDatas)
            {
                sqlcmd = string.Format("select * from artworkpo_detail where ukey = '{0}'", dr["artworkpo_detailukey"]);
                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    ShowErr(sqlcmd, result);
                    return;
                }
                if (datacheck.Rows.Count > 0)
                {
                    if ((decimal)dr["qty"] + (decimal)datacheck.Rows[0]["farmin"] > (decimal)datacheck.Rows[0]["farmout"])
                    {
                        ids += string.Format("{0}-{1}-{2}-{3}-{4} is over Farm out qty", datacheck.Rows[0]["id"], datacheck.Rows[0]["orderid"], datacheck.Rows[0]["artworktypeid"], datacheck.Rows[0]["artworkid"], datacheck.Rows[0]["patterncode"]) + Environment.NewLine;
                    }
                }
            }
            if (!MyUtility.Check.Empty(ids))
            {
                MyUtility.Msg.WarningBox(ids);
                return;
            }

            // update farmout status

            sqlcmd3 = string.Format("update Farmin set status = 'Confirmed' , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);


            // update artworkpo_detail farmin
            DataTable detailgroup;
            sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.qty) qty
                                    from farmIn_detail b
                                    where b.id ='{0}'
                                    group by b.artworkpo_detailukey ", CurrentMaintain["id"]);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out detailgroup)))
            {
                ShowErr(sqlcmd, result);
                return;
            }

            if (detailgroup.Rows.Count > 0)
            {
                foreach (DataRow dr in detailgroup.Rows)
                {
                    sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.qty) qty
                                    from farmin a, farmIn_detail b
                                    where a.id = b.id  and a.Status ='Confirmed' and b.artworkpo_detailukey ='{0}'
                                    group by b.artworkpo_detailukey ", dr["artworkpo_detailukey"]);

                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                    {
                        ShowErr(sqlcmd, result);
                        return;
                    }
                    if (datacheck.Rows.Count > 0)
                    {
                        sqlcmd2 += string.Format("update artworkpo_detail set farmIn = {0} where ukey = '{1}';"
                            + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] + (decimal)dr["qty"], dr["artworkpo_detailukey"]);
                    }
                    else
                    {
                        sqlcmd2 += string.Format("update artworkpo_detail set farmIn = {0} where ukey = '{1}';"
                           + Environment.NewLine, (decimal)dr["qty"], dr["artworkpo_detailukey"]);

                    }
                }
            }

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlcmd3)))
                    {
                        ShowErr(sqlcmd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlcmd2)))
                    {
                        ShowErr(sqlcmd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Confirm successful");
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            RenewData();
            OnDetailEntered();
            this.EnsureToolbarExt();
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!(Prgs.GetAuthority(Env.User.UserID) &&
                                CurrentMaintain["handle"].ToString() != Env.User.UserID))
            {
                MyUtility.Msg.WarningBox("Only Handle & Leader have authority to UnConfirm");
                return;
            }

            String sqlcmd, sqlcmd2 = "", sqlcmd3 = "";
            DualResult result, result2;
            DataTable datacheck;
            sqlcmd = string.Format(@"select a.id from artworkpo a, farmin_detail b 
                            where a.id = b.artworkpoid and a.Status = 'Closed' and b.id = '{0}'", CurrentMaintain["id"]);


            string ids = "", bundlenos = "";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow dr in datacheck.Rows)
                {
                    ids += dr[0].ToString() + ",";
                }
                MyUtility.Msg.WarningBox(String.Format("These POID <{0}> already closed, can't unConfirmed", ids));
                return;
            }

            // 提示是否不低於ap qty

            ids = "";
            foreach (var dr in DetailDatas)
            {
                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    ShowErr(sqlcmd, result);
                    return;
                }
                if (datacheck.Rows.Count > 0)
                {
                    if ((decimal)datacheck.Rows[0]["farmin"] - (decimal)dr["qty"] < (decimal)datacheck.Rows[0]["apqty"])
                    {
                        ids += string.Format("{0}-{1}-{2}-{3}-{4} can't less AP qty {5}", datacheck.Rows[0]["id"], datacheck.Rows[0]["orderid"], datacheck.Rows[0]["artworktypeid"], datacheck.Rows[0]["artworkid"], datacheck.Rows[0]["patterncode"], datacheck.Rows[0]["apqty"]) + Environment.NewLine;
                    }
                }
            }
            if (!MyUtility.Check.Empty(ids))
            {
                MyUtility.Msg.WarningBox(ids);
                return;
            }

            // update farmout status

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to amend it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            sqlcmd3 = string.Format("update FarmIn set status = 'New', editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);


            // update artworkpo_detail farmin
            DataTable detailgroup;
            sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.qty) qty
                                    from farmIn_detail b
                                    where b.id ='{0}'
                                    group by b.artworkpo_detailukey ", CurrentMaintain["id"]);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out detailgroup)))
            {
                ShowErr(sqlcmd, result);
                return;
            }

            if (detailgroup.Rows.Count > 0)
            {
                foreach (DataRow dr in detailgroup.Rows)
                {
                    sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.qty) qty
                                    from farmin a, farmIn_detail b
                                    where a.id = b.id  and a.status ='Confirmed' and b.artworkpo_detailukey ='{0}'
                                    group by b.artworkpo_detailukey ", dr["artworkpo_detailukey"]);

                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                    {
                        ShowErr(sqlcmd, result);
                        return;
                    }
                    if (datacheck.Rows.Count > 0)
                    {

                        sqlcmd2 += string.Format("update artworkpo_detail set farmIn = {0} where ukey = '{1}';"
                            + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] - (decimal)dr["qty"], dr["artworkpo_detailukey"]);
                    }
                    else
                    {

                        sqlcmd2 += string.Format("update artworkpo_detail set farmIn = {0} where ukey = '{1}';"
                            + Environment.NewLine, 0m, dr["artworkpo_detailukey"]);

                    }
                }
            }

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlcmd3)))
                    {
                        ShowErr(sqlcmd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlcmd2)))
                    {
                        ShowErr(sqlcmd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            RenewData();
            OnDetailEntered();
            this.EnsureToolbarExt();
        }

    }
}
