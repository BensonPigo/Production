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
using Sci.Production;

using Sci.Production.PublicPrg;
using System.Linq;
using System.Transactions;


namespace Sci.Production.Subcon
{
    public partial class P10 : Sci.Win.Tems.Input6
    {

        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "Mdivisionid = '" + Sci.Env.User.Keyword +"'";
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

            this.txtsubcon1.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon1.TextBox1.Text != this.txtsubcon1.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubcon1.TextBox1.Text, "LocalSupp", "ID");
                    CurrentMaintain["Paytermid"] = MyUtility.GetValue.Lookup("paytermid", this.txtsubcon1.TextBox1.Text, "LocalSupp", "ID");
                    ((DataTable)detailgridbs.DataSource).Rows.Clear();
                }
            };

        }

        private void txtartworktype_fty1_Validated(object sender, EventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();
                string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype where id='{0}'", o.Text));
                if (artworkunit == "") { artworkunit = "PCS"; }
                this.detailgrid.Columns[3].HeaderText = artworkunit; 
            }
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Mdivisionid"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
            CurrentMaintain["VatRate"] = 0;
            CurrentMaintain["Status"] = "New";
            ((DataTable)(detailgridbs.DataSource)).Rows[0].Delete();
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString().ToUpper() == "APPROVED")
            {
                MyUtility.Msg.WarningBox("Data is approved, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString()=="Approved")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("artworkap","remark",dr);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.ClickEditBefore();
        }

        // edit後，更新detail的farm in跟accu. ap qty
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            foreach (DataRow dr in DetailDatas)
            {
                dr["Farmin"] = MyUtility.GetValue.Lookup(string.Format("select farmin from artworkpo_detail where ukey = '{0}'", dr["artworkpo_detailukey"].ToString()));
                dr["accumulatedqty"] = MyUtility.GetValue.Lookup(string.Format("select apqty from artworkpo_detail where ukey = '{0}'", dr["artworkpo_detailukey"].ToString()));
                dr["balance"] = (decimal)dr["Farmin"] - (decimal)dr["accumulatedqty"];
            }
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["apqty"] = 0;
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查
            if (CurrentMaintain["LocalSuppID"]==DBNull.Value|| string.IsNullOrWhiteSpace(CurrentMaintain["LocalSuppID"].ToString()))
		    {
                MyUtility.Msg.WarningBox("< Suppiler >  can't be empty!","Warning");
                txtsubcon1.TextBox1.Focus();
                return false;
            }

            if (CurrentMaintain["issuedate"]==DBNull.Value|| string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
		    {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            if (CurrentMaintain["ArtworktypeId"]==DBNull.Value|| string.IsNullOrWhiteSpace(CurrentMaintain["ArtworktypeId"].ToString()))
		    {
                MyUtility.Msg.WarningBox("< Artwork Type >  can't be empty!", "Warning");
                txtartworktype_fty1.Focus();
                return false;
            }

            if (CurrentMaintain["CurrencyID"]==DBNull.Value|| string.IsNullOrWhiteSpace(CurrentMaintain["CurrencyID"].ToString()))
		    {
                MyUtility.Msg.WarningBox("< Currency >  can't be empty!", "Warning");
                return false;
            }

            if (CurrentMaintain["Handle"]==DBNull.Value|| string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
		    {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                txtuser1.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                txtmfactory1.Focus();
                return false;
            }
            #endregion

            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("apqty = 0"))
            {
                ((DataTable)detailgridbs.DataSource).Rows.Remove(row);
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號： 
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(factorykeyword + "FA", "artworkAP", (DateTime)CurrentMaintain["issuedate"]);
                if (MyUtility.Check.Empty(CurrentMaintain["id"]))
                {
                    MyUtility.Msg.WarningBox("Server is busy, Please re-try it again", "GetID() Failed");
                    return false;
                }
            }

            #region 加總明細金額至表頭
            string str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency where id = '{0}'", CurrentMaintain["currencyId"]),null);
            if (str == null || string.IsNullOrWhiteSpace(str))
            {
                MyUtility.Msg.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , can't save!", CurrentMaintain["currencyID"]), "Warning");
                return false;
            }
            int exact = int.Parse(str);
            object detail_a = ((DataTable)detailgridbs.DataSource).Compute("sum(amount)", "");
            CurrentMaintain["amount"] = MyUtility.Math.Round((decimal)detail_a, exact);
            CurrentMaintain["vat"] = MyUtility.Math.Round((decimal)detail_a * (decimal)CurrentMaintain["vatrate"] / 100, exact);
            #endregion
            
            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                (e.Details).Columns.Add("poqty", typeof(decimal));
                (e.Details).Columns.Add("balance", typeof(decimal));
                decimal poqty;
                foreach (DataRow dr in e.Details.Rows)
                {
                    poqty = 0m;
                    decimal.TryParse(MyUtility.GetValue.Lookup(string.Format("select poqty from artworkpo_detail where ukey = {0}", (long)dr["artworkpo_detailukey"])),out poqty);
                    dr["poqty"] = poqty;
                    dr["balance"] = (decimal)dr["farmin"] - (decimal)dr["accumulatedqty"];                    
                }
            }
 	         return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype where id='{0}'", CurrentMaintain["artworktypeid"])).ToString().Trim();
            if (artworkunit == "") { artworkunit = "PCS"; }
            this.detailgrid.Columns[3].HeaderText = artworkunit; 
            if (!(CurrentMaintain == null))
            {
                if (!(CurrentMaintain["amount"] == DBNull.Value) && !(CurrentMaintain["vat"] == DBNull.Value))
                {
                    decimal amount = (decimal)CurrentMaintain["amount"] + (decimal)CurrentMaintain["vat"];
                    numericBox4.Text = amount.ToString();
                }
            }
            txtsubcon1.Enabled = !this.EditMode || IsDetailInserting;
            txtartworktype_fty1.Enabled = !this.EditMode || IsDetailInserting;
            txtpayterm_fty1.Enabled =  !this.EditMode || IsDetailInserting;
            txtmfactory1.Enabled = !this.EditMode || IsDetailInserting;
            #region Status Label
            label25.Text = CurrentMaintain["status"].ToString();
            #endregion

            #region Batch Import, Special record button
            button4.Enabled = this.EditMode;

            #endregion

        }

        // Detail Grid 設定 & Detail Vaild
        protected override void OnDetailGridSetup()
        {
            #region qtygarment Valid
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    if ((decimal)e.FormattedValue > (decimal)CurrentDetailData["balance"] ||
                        (decimal)e.FormattedValue + (decimal)CurrentDetailData["accumulatedqty"] > (decimal)CurrentDetailData["Poqty"] )
                    {
                        MyUtility.Msg.WarningBox("can't over balance and can't over poqty", "Warning");
                        e.Cancel = true;
                        return;
                    }
                    CurrentDetailData["amount"] = (decimal)e.FormattedValue * (decimal)CurrentDetailData["price"];
                    CurrentDetailData["apqty"] = e.FormattedValue;
                }
            };
            #endregion

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Artworkpoid", header: "Artwork PO", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)   //1
            .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(8), iseditingreadonly: true)    //2
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(5), iseditingreadonly: true)    //3
            .Text("patterncode", header: "CutpartID", width: Widths.AnsiChars(10), iseditingreadonly: true) //4
            .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(15), iseditingreadonly: true)   //5
            .Numeric("price", header: "Price", width: Widths.AnsiChars(5), decimal_places: 4, integer_places: 4, iseditingreadonly: true)     //6
            .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //7
            .Numeric("farmin", header: "Farm In", width: Widths.AnsiChars(6), iseditingreadonly: true)    //8
            .Numeric("accumulatedqty", header: "Accu. Paid Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //9
            .Numeric("balance", header: "Balance", width: Widths.AnsiChars(6), iseditingreadonly: true)    //10
            .Numeric("apqty", header: "Qty", width: Widths.AnsiChars(6),settings:ns2)    //11
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 4, integer_places: 14);  //12
                   
            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.Pink; //qty
            #endregion
        }

        //Approve
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlcmd, sqlupd2 = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;
            #region 檢查po是否close了。
            sqlcmd = string.Format(@"select a.id from artworkpo a, artworkap_detail b 
                            where a.id = b.artworkpoid and a.closed = 1 and b.id = '{0}'", CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow drchk in datacheck.Rows)
                {
                    ids += drchk[0].ToString() + ",";
                }
                MyUtility.Msg.WarningBox(String.Format("These POID <{0}> already closed, can't Approve it", ids));
                return;
            }
            #endregion
            #region 檢查apqty是否超過poqty
            ids = "";
            foreach (var dr1 in DetailDatas)
            {
                sqlcmd = string.Format("select * from artworkpo_detail where ukey = '{0}'", dr1["artworkpo_detailukey"]);
                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    ShowErr(sqlcmd, result);
                    return;
                }
                if (datacheck.Rows.Count > 0)
                {
                    if ((decimal)dr1["apqty"] + (decimal)datacheck.Rows[0]["apqty"] > (decimal)datacheck.Rows[0]["poqty"]
                        || (decimal)dr1["apqty"] + (decimal)datacheck.Rows[0]["apqty"] > (decimal)datacheck.Rows[0]["farmin"])
                    {
                        ids += string.Format("{0}-{1}-{2}-{3}-{4} is over PO qty or Farm in", datacheck.Rows[0]["id"], datacheck.Rows[0]["orderid"], datacheck.Rows[0]["artworktypeid"], datacheck.Rows[0]["artworkid"], datacheck.Rows[0]["patterncode"]) + Environment.NewLine;
                    }
                }
            }
            if (!MyUtility.Check.Empty(ids))
            {
                MyUtility.Msg.WarningBox(ids);
                return;
            }
            #endregion
            #region 開始更新相關table資料
            sqlupd3 = string.Format("update artworkap set status='Approved', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            
            foreach (DataRow drchk in DetailDatas)
            {
                sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.apqty) qty
                                from artworkap a, artworkap_detail b
                                where a.id = b.id  and a.status = 'Approved' and b.artworkpo_detailukey ='{0}'
                                group by b.artworkpo_detailukey ", drchk["artworkpo_detailukey"]);

                if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                {
                    ShowErr(sqlcmd, result);
                    return;
                }

                if (datacheck.Rows.Count > 0)
                {
                        sqlupd2 += string.Format("update artworkpo_detail set apqty = {0} where ukey = '{1}';"
                            + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] + (decimal)drchk["apqty"], drchk["artworkpo_detailukey"]);
                }
                else
                {
                        sqlupd2 += string.Format("update artworkpo_detail set apqty = {0} where ukey = '{1}';"
                            + Environment.NewLine, (decimal)drchk["apqty"], drchk["artworkpo_detailukey"]);
                }
            }

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        ShowErr(sqlupd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Approve successful");
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
                this.RenewData();
                this.OnDetailEntered();
                this.EnsureToolbarExt();
            #endregion
        }
        
        //unApprove
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlcmd, sqlupd2 = "", sqlupd3 = "",ids = "";
            DualResult result, result2;
            DataTable datacheck;
            #region 檢查po是否close了。
            sqlcmd = string.Format(@"select a.id from artworkpo a, artworkap_detail b 
                            where a.id = b.artworkpoid and a.closed = 1 and b.id = '{0}'", CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck))) { ShowErr(sqlcmd, result); }
            if (datacheck.Rows.Count > 0)
            {
                foreach (DataRow drchk in datacheck.Rows)
                {
                    ids += drchk[0].ToString() + ",";
                }
                MyUtility.Msg.WarningBox(String.Format("These POID <{0}> already closed, can't UnApprove it", ids));
                return;
            }
            #endregion
            

            #region 開始更新相關table資料   
            
             sqlupd3 = string.Format("update artworkap set status='New',apvname='', apvdate = null , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
             
            
                foreach (DataRow drchk in DetailDatas)
                {
                    sqlcmd = string.Format(@"select b.artworkpo_detailukey, sum(b.apqty) qty
                                from artworkap a, artworkap_detail b
                                where a.id = b.id  and a.status ='Approved' and b.artworkpo_detailukey ='{0}'
                                group by b.artworkpo_detailukey ", drchk["artworkpo_detailukey"]);

                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
                    {
                        ShowErr(sqlcmd, result);
                        return;
                    }
                    if (datacheck.Rows.Count > 0)
                    {
                        sqlupd2 += string.Format("update artworkpo_detail set apqty = {0} where ukey = '{1}';"
                                + Environment.NewLine, (decimal)datacheck.Rows[0]["qty"] - (decimal)drchk["apqty"], drchk["artworkpo_detailukey"]);
                    }
                    else
                    {
                        sqlupd2 += string.Format("update artworkpo_detail set apqty = {0} where ukey = '{1}';"
                                + Environment.NewLine, 0m, drchk["artworkpo_detailukey"]);
                    }
                }
             
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        ShowErr(sqlupd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("UnApprove successful");
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
                this.RenewData();
                this.OnDetailEntered();
                this.EnsureToolbarExt();
            
            #endregion
        }

       // P10_ImportFromPO
        private void button4_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (MyUtility.Check.Empty(dr["localsuppid"]))
            {
                MyUtility.Msg.WarningBox("Please fill Supplier first!");
                txtsubcon1.TextBox1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(dr["artworktypeid"]))
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                txtartworktype_fty1.Focus();
                return;
            }
            var frm = new Sci.Production.Subcon.P10_ImportFromPO(dr, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.EditMode) return;
            var frm = new Sci.Production.Subcon.P01_BatchCreate("P01");
            frm.ShowDialog(this);
            ReloadDatas();
        }

        private void txtartworktype_fty1_Validating(object sender, CancelEventArgs e)
        {
            
        }


        

    }
}
