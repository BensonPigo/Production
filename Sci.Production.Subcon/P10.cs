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
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;


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
           
            this.txtsubconSupplier.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier.TextBox1.Text != this.txtsubconSupplier.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier.TextBox1.Text, "LocalSupp", "ID");
                    CurrentMaintain["Paytermid"] = MyUtility.GetValue.Lookup("paytermid", this.txtsubconSupplier.TextBox1.Text, "LocalSupp", "ID");
                    ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
                }
            };
            
        }

        private void txtartworktype_ftyArtworkType_Validated(object sender, EventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
                string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", o.Text));
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
            if (CurrentMaintain["Status"].ToString().ToUpper() == "APPROVED")
            {
                MyUtility.Msg.WarningBox("Data is approved, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["status"].ToString() == "Approved")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("artworkap", "remark", CurrentMaintain);
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
            //foreach (DataRow dr in DetailDatas)
            //{
            //    var v = MyUtility.GetValue.Lookup(string.Format("select farmin from artworkpo_detail where ukey = '{0}'", dr["artworkpo_detailukey"].ToString()));
            //    decimal accQty;
            //    Decimal.TryParse(v, out accQty);
            //    dr["Farmin"] = accQty;
            //    var v2 = MyUtility.GetValue.Lookup(string.Format("select apqty from artworkpo_detail where ukey = '{0}'", dr["artworkpo_detailukey"].ToString()));
            //    decimal accQty2;
            //    Decimal.TryParse(v2, out accQty2);
            //    dr["accumulatedqty"] = accQty2;
            //    //無此資料行且結果必=0
            //    //dr["balance"] = (decimal)dr["Farmin"] - (decimal)dr["accumulatedqty"];
            //}
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
                txtsubconSupplier.TextBox1.Focus();
                return false;
            }

            if (CurrentMaintain["issuedate"]==DBNull.Value|| string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
		    {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateIssueDate.Focus();
                return false;
            }

            if (CurrentMaintain["ArtworktypeId"]==DBNull.Value|| string.IsNullOrWhiteSpace(CurrentMaintain["ArtworktypeId"].ToString()))
		    {
                MyUtility.Msg.WarningBox("< Artwork Type >  can't be empty!", "Warning");
                txtartworktype_ftyArtworkType.Focus();
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
                txtuserHandle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                txtmfactory.Focus();
                return false;
            }

            if (CurrentMaintain["PayTermid"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["PayTermid"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Terms >  can't be empty!", "Warning");
                txtpayterm_ftyTerms.Focus();
                return false;
            }
            #endregion

            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("apqty = 0"))
            {
                row.Delete();
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            #region 表身的來源subconp01單號是否CONFIRM。
            string chkp01 =
                @"
select distinct ap.id
from ArtworkPO ap with(nolock) 
inner join #tmp t on t.artworkpoid = ap.id
where  ap.status = 'New'
";
            DataTable dt;
            DualResult result;
            if(result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, "Artworkpoid", chkp01, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    StringBuilder chkp01comfirmed = new StringBuilder();
                    foreach (DataRow dr in dt.Rows)
                    {
                        chkp01comfirmed.Append(string.Format("Please confirm [Subcon][P01]:{0} first !!\r\n", dr["id"]));
                    }
                    MyUtility.Msg.WarningBox(chkp01comfirmed.ToString());
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return false;
            }

            #endregion

            //取單號： 
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "FA", "artworkAP", (DateTime)CurrentMaintain["issuedate"]);
                if (MyUtility.Check.Empty(CurrentMaintain["id"]))
                {
                    MyUtility.Msg.WarningBox("Server is busy, Please re-try it again", "GetID() Failed");
                    return false;
                }
            }

            #region 加總明細金額至表頭
            string str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", CurrentMaintain["currencyId"]), null);
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

        //組表身資料
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            string cmdsql = string.Format(@"
select a.* 
, b.PoQty
, [balance]=a.Farmin-a.AccumulatedQty
from ArtworkAP_detail a
inner join artworkpo_detail b on a.ArtworkPo_DetailUkey=b.Ukey
where a.id='{0}'
", masterID);
            this.DetailSelectCommand = cmdsql;
            return base.OnDetailSelectCommandPrepare(e);
        }             

        void addBalance()
        {
            DataTable Details = (DataTable)this.detailgridbs.DataSource;
            if (Details.Columns.Contains("balance")) return;

            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                (Details).Columns.Add("poqty", typeof(decimal));
                (Details).Columns.Add("balance", typeof(decimal));
                decimal poqty;
                foreach (DataRow dr in Details.Rows)
                {
                    poqty = 0m;
                    decimal.TryParse(MyUtility.GetValue.Lookup(string.Format("select poqty from artworkpo_detail WITH (NOLOCK) where ukey = {0}", (long)dr["artworkpo_detailukey"])), out poqty);
                    dr["poqty"] = poqty;
                    dr["balance"] = (decimal)dr["farmin"] - (decimal)dr["accumulatedqty"];
                }
            }
        }
        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", CurrentMaintain["artworktypeid"])).ToString().Trim();
            if (artworkunit == "") { artworkunit = "PCS"; }
            this.detailgrid.Columns[3].HeaderText = artworkunit; 
            if (!(CurrentMaintain == null))
            {
                if (!(CurrentMaintain["amount"] == DBNull.Value) && !(CurrentMaintain["vat"] == DBNull.Value))
                {
                    decimal amount = (decimal)CurrentMaintain["amount"] + (decimal)CurrentMaintain["vat"];
                    numTotal.Text = amount.ToString();
                }
            }
            txtsubconSupplier.Enabled = !this.EditMode || IsDetailInserting;
            txtartworktype_ftyArtworkType.Enabled = !this.EditMode || IsDetailInserting;
            txtpayterm_ftyTerms.Enabled =  !this.EditMode || IsDetailInserting;
            txtmfactory.Enabled = !this.EditMode || IsDetailInserting;
            dateApprovedDate.ReadOnly = true;
            #region Status Label
            label25.Text = CurrentMaintain["status"].ToString();
            #endregion

            #region Batch Import, Special record button
            btnImportFromPO.Enabled = this.EditMode;

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
                    addBalance();
                    if ((decimal)e.FormattedValue > (decimal)CurrentDetailData["balance"]||
                        (decimal)e.FormattedValue + (decimal)CurrentDetailData["accumulatedqty"] > (decimal)CurrentDetailData["PoQty"])
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("can't over balance and can't over poqty", "Warning");
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
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(12), iseditingreadonly: true, decimal_places: 2, integer_places: 14);  //12
                   
            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns["apqty"].DefaultCellStyle.BackColor = Color.Pink; //qty
            #endregion
        }

        //Approve
        protected override void ClickConfirm()
        {
            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlcmd, sqlupd2 = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            #region 須檢核其來源單[P10]狀態為CONFIRM。
            string check_p10status = string.Format(
                @"
select distinct ap.id
from ArtworkAP aa with(nolock)
inner join ArtworkAP_detail aad with(nolock) on aad.id = aa.id
inner join ArtworkPO ap with(nolock)on ap.id = aad.ArtworkPoid
where ap.status = 'New' and aa.Id ='{0}'",
                CurrentMaintain["id"]);
            DataTable chktb;
            if (result = DBProxy.Current.Select(null, check_p10status, out chktb))
            {
                if (chktb.Rows.Count > 0)
                {
                    string p10id = "";
                    foreach (DataRow drr in chktb.Rows)
                    {
                        p10id += drr["id"].ToString();
                    }
                    string chkp10msg = string.Format("Please confirm [Subcon][P01]:{0} first !!", p10id);
                    MyUtility.Msg.WarningBox(chkp10msg);
                    return;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }
            #endregion

            #region 檢查po是否close了。
            sqlcmd = string.Format(@"select a.id from artworkpo a WITH (NOLOCK) , artworkap_detail b WITH (NOLOCK) 
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
                sqlcmd = string.Format("select * from artworkpo_detail WITH (NOLOCK) where ukey = '{0}'", dr1["artworkpo_detailukey"]);
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
                                from artworkap a WITH (NOLOCK) , artworkap_detail b WITH (NOLOCK) 
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
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Approve successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            #endregion
            base.ClickConfirm();
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
            sqlcmd = string.Format(@"select a.id from artworkpo a WITH (NOLOCK) , artworkap_detail b WITH (NOLOCK) 
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
                                from artworkap a WITH (NOLOCK) , artworkap_detail b WITH (NOLOCK) 
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
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnApprove successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
              
            
            #endregion
        }

       // P10_ImportFromPO
        private void btnImportFromPO_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (MyUtility.Check.Empty(dr["localsuppid"]))
            {
                MyUtility.Msg.WarningBox("Please fill Supplier first!");
                txtsubconSupplier.TextBox1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(dr["artworktypeid"]))
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                txtartworktype_ftyArtworkType.Focus();
                return;
            }
            var frm = new Sci.Production.Subcon.P10_ImportFromPO(dr, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        //print
        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string Issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string Invoice = row["invno"].ToString();
            string Remarks = row["Remark"].ToString();
          

            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result;
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", Invoice));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remarks", Remarks));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", Issuedate));
            

            #endregion
            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string sqlcmd = @"
select F.nameEn
,AddressEN = REPLACE(REPLACE(F.AddressEN,Char(13),''),Char(10),'')
,F.Tel
,ap.LocalSuppID+'-'+L.name AS Supplier
,L.Address
,L.tel
,ap.ID
,A.ArtworkPoID
,A.OrderID
,A.ArtworkId
,A.PatternDesc
,A.Price
,A.ApQty
,format(A.Amount,'#,###,###,##0.00')Amount
,ap.PayTermID+'-'+P.name as Terms
,LOB.AccountNo
,LOB.AccountName
,LOB.BankName
,LOB.CountryID+'/'+LOB.City as Country
,LOB.SWIFTCode
,ap.Handle+CHAR(13)+CHAR(10)+pas.name as PreparedBy
,format(ap.Amount,'#,###,###,##0.00') as Total
,format(ap.Vat,'#,###,###,##0.00') as Vat
,format(ap.Amount+ap.Vat,'#,###,###,##0.00') as GrandTotal
,ap.currencyid as Currency
from DBO.artworkap ap WITH (NOLOCK) 
LEFT JOIN dbo.factory F WITH (NOLOCK) 
    ON  F.ID = ap.factoryid
LEFT JOIN dbo.LocalSupp L WITH (NOLOCK) 
    ON  L.ID = ap.LocalSuppID
LEFT JOIN dbo.Artworkap_Detail A WITH (NOLOCK) 
    ON  A.ID = ap.ID
LEFT JOIN dbo.LocalSupp_Bank LOB WITH (NOLOCK) 
    ON  IsDefault = 1 and LOB.ID = ap.LocalSuppID
LEFT JOIN DBO.PayTerm P WITH (NOLOCK) 
    ON P.ID = ap.PayTermID
LEFT JOIN DBO.Pass1 pas WITH (NOLOCK) 
    ON pas.ID = ap.Handle 
where ap.ID= @ID";
            result = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result) { this.ShowErr(sqlcmd, result); }
            string RptTitle = dtDetail.Rows[0]["nameEn"].ToString();
            string AddressEN = dtDetail.Rows[0]["AddressEN"].ToString();
            string TEL = dtDetail.Rows[0]["Tel"].ToString();
            string Supplier = dtDetail.Rows[0]["Supplier"].ToString();
            string Address = dtDetail.Rows[0]["Address"].ToString();
            string LTEL = dtDetail.Rows[0]["tel"].ToString();
            string Barcode = dtDetail.Rows[0]["ID"].ToString();
            string BarcodeView = dtDetail.Rows[0]["ID"].ToString();
            string Terms = dtDetail.Rows[0]["Terms"].ToString();
            string ACNO = dtDetail.Rows[0]["AccountNo"].ToString();
            string ACNAME = dtDetail.Rows[0]["AccountName"].ToString();
            string BankName = dtDetail.Rows[0]["BankName"].ToString();
            string Country = dtDetail.Rows[0]["Country"].ToString();
            string SWIFCode = dtDetail.Rows[0]["SWIFTCode"].ToString();
            string PreparedBy = dtDetail.Rows[0]["PreparedBy"].ToString();
            string Total = dtDetail.Rows[0]["Total"].ToString();
            string VAT = dtDetail.Rows[0]["Vat"].ToString();
            string GrandTotal = dtDetail.Rows[0]["GrandTotal"].ToString();
            string Currency = dtDetail.Rows[0]["Currency"].ToString();
           
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TEL", TEL));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", Supplier));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Address", Address));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("LTEL", LTEL));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", Barcode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("BarcodeView", BarcodeView));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Terms", Terms));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ACNO", ACNO));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ACNAME", ACNAME));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("BankName", BankName));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Country", Country));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SWIFCode", SWIFCode));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("PreparedBy", PreparedBy));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Total", Total));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("VAT", VAT));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("GrandTotal", GrandTotal));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Currency", Currency));

            if (!AddressEN.EndsWith(Environment.NewLine))
            {
               report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddressEN", AddressEN + Environment.NewLine));
            }
            else 
            { 
               report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddressEN", AddressEN));
            }
       
            // 傳 list 資料            
            List<P10_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P10_PrintData()
                {
                    POID = row1["ArtworkPoID"].ToString(),
                    OrderID = row1["OrderID"].ToString(),
                    Pattem = row1["ArtworkId"].ToString(),
                    CutPart = row1["PatternDesc"].ToString(),
                    Price = row1["Price"].ToString(),
                    Qty = row1["ApQty"].ToString(),
                    Amt = row1["Amount"].ToString()
                    
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P10_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P10_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();

            return true;
        }
    }
}
