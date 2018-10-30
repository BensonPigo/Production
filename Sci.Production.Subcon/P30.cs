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
    public partial class P30 : Sci.Win.Tems.Input6
    {
        public static DataTable dtPadBoardInfo;

        public P30(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "mdivisionid = '" + Sci.Env.User.Keyword + "'";

            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

            this.txtsubconSupplier.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier.TextBox1.Text != this.txtsubconSupplier.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier.TextBox1.Text, "LocalSupp", "ID");
                    if (detailgridbs.DataSource != null && ((DataTable)detailgridbs.DataSource).Rows.Count > 0)
                    {
                        ((DataTable)detailgridbs.DataSource).Rows.Clear();
                    }
                }
            };

        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab) && !MyUtility.Check.Empty(CurrentMaintain))
            {
                this.toolbar.cmdConfirm.Enabled = !this.EditMode && Sci.Production.PublicPrg.Prgs.GetAuthority(Env.User.UserID) && CurrentMaintain["status"].ToString() == "New";
                this.toolbar.cmdUnconfirm.Enabled = !this.EditMode && Sci.Production.PublicPrg.Prgs.GetAuthority(Env.User.UserID) && CurrentMaintain["status"].ToString() == "Approved";
                this.toolbar.cmdClose.Enabled = !this.EditMode && Sci.Production.PublicPrg.Prgs.GetAuthority(Env.User.UserID) && CurrentMaintain["status"].ToString() == "Approved";
                this.toolbar.cmdUnclose.Enabled = !this.EditMode && Sci.Production.PublicPrg.Prgs.GetAuthority(Env.User.UserID) && CurrentMaintain["status"].ToString() == "Closed";
            }
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Mdivisionid"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["VatRate"] = 0;
            CurrentMaintain["Status"] = "New";
            txtmfactory.ReadOnly = true;  //新增時[factory]預設唯讀
            //((DataTable)(detailgridbs.DataSource)).Rows[0].Delete();
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["status"].ToString().ToUpper() == "APPROVED")
            {
                MyUtility.Msg.WarningBox("Data is approved, can't delete.", "Warning");
                return false;
            }

            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = CurrentMaintain["id"].ToString();

            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
            paras.Add(sp1);

            string sqlcmd;
            sqlcmd = "select fd.ID from LocalPO_Detail ad WITH (NOLOCK) , LocalAP_Detail fd WITH (NOLOCK) where ad.Ukey = fd.LocalPo_DetailUkey and ad.id = @id";

            DataTable dt;
            DBProxy.Current.Select(null, sqlcmd, paras, out dt);
            if (dt.Rows.Count > 0)
            {
                string ids = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ids += dt.Rows[i][0].ToString() + ";";
                }
                MyUtility.Msg.WarningBox(string.Format("Below AP {0} refer to details data, can't delete.", ids), "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (CurrentMaintain["status"].ToString().ToUpper() == "APPROVED")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("Localpo", "remark", CurrentMaintain);
                frm.ShowDialog(this);

                this.RenewData();

                //[Apv. Date]格式調整，僅顯示YYYY/MM/DD
                if (!(CurrentMaintain["ApvDate"] == DBNull.Value)) displayApvDate.Text = Convert.ToDateTime(CurrentMaintain["ApvDate"]).ToShortDateString();
                else displayApvDate.Text = "";
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {

            #region 必輸檢查
            if (CurrentMaintain["LocalSuppID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["LocalSuppID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Suppiler >  can't be empty!", "Warning");
                txtsubconSupplier.TextBox1.Focus();
                return false;
            }

            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateIssueDate.Focus();
                return false;
            }

            if (CurrentMaintain["Category"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Category"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Category >  can't be empty!", "Warning");
                txtartworktype_ftyCategory.Focus();
                return false;
            }

            if (CurrentMaintain["CurrencyID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["CurrencyID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Currency >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                txtmfactory.Focus();
                return false;
            }
            foreach (DataRow Ddr in DetailDatas)
            {
                if (MyUtility.Check.Empty(Ddr["delivery"]))
                {
                    MessageBox.Show("Delivery can not any empty.");
                    return false;
                }
                if (MyUtility.Check.Empty(Ddr["orderid"]))
                {
                    MyUtility.Msg.InfoBox("SP# can't be empty.");
                    return false;
                }
            }

            #endregion

            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("qty =0 or refno =' '"))
            {
                row.Delete();
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }


            //Issue ISP20180084 LocalPO_Detail RequestID,OrderID,RefNo需為unique 只針對CARTON
            if (this.CurrentMaintain["category"].ToString().ToUpper().TrimEnd().Equals("CARTON"))
            {
                DataTable resulttb;
                string check_sql = $@"select lpd.ID,a.RequestId,a.OrderId,a.Refno from #TmpSource a inner join LocalPO_Detail lpd WITH (NOLOCK) on a.RequestID = lpd.RequestID and  a.OrderID = lpd.OrderID and a.RefNo = lpd.RefNo and lpd.ID <> '{CurrentMaintain["ID"]}' and a.RequestID <> ''";

                DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, "", check_sql, out resulttb, "#TmpSource");
                if (!result)
                {
                    ShowErr(result);
                    return false;
                }

                //有重複資料
                if (resulttb.Rows.Count > 0)
                {
                    var m = new Sci.Win.UI.MsgGridForm(resulttb, "The following SP#,Refno,RequestID has been imported:", "Warning", null, MessageBoxButtons.OK);

                    m.Width = 600;
                    m.grid1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    m.grid1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    m.grid1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    m.grid1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    m.text_Find.Width = 140;
                    m.btn_Find.Location = new Point(150, 6);
                    m.btn_Find.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                    m.ShowDialog();

                    return false;
                }
            }
            //Issue ISP20180084 end

            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'LocalPO1'), 'LocalPO', IssueDate, 2)
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "LP", "Localpo", (DateTime)CurrentMaintain["issuedate"]);
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
        protected override DualResult ClickSave()
        {
            String sqlupd2 = "", ids = "";
            DualResult result2;

            #region 檢查表身明細requestid是否已有回寫過poid
            DataTable dt = (DataTable)detailgridbs.DataSource;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    if (dr["requestid"].ToString() != "")
                    {
                        string chk = string.Format("select distinct LocalPOID from packinglist where id = '{0}' and isnull(LocalPOID,'') != ''", dr["requestid"].ToString());
                        if (MyUtility.Check.Seek(chk))
                        {
                            ids += string.Format("Request ID: {0} is already in LocalPO : {1}" + Environment.NewLine, dr["requestid"], dr["POID"]);
                        }
                    }
                }
                if (ids != "")
                {
                    return Result.F("Below request id already be created in Local PO, can't approve it!!" + Environment.NewLine + ids);
                }
            }
            #endregion

            #region 檢查明細requestid是否已有回寫poid
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {

                    if (dr["requestid"].ToString() != "")
                    {
                        string chk = string.Format(@"select ThreadRequisition_Detail.OrderID,ThreadRequisition_Detail.Refno,ThreadRequisition_Detail.ThreadColorID,ThreadRequisition_Detail.POID 
from ThreadRequisition_Detail WITH (NOLOCK)
where ThreadRequisition_Detail.OrderID = '{0}'
and ThreadRequisition_Detail.Refno = '{1}'
and ThreadRequisition_Detail.ThreadColorID = '{2}'
and isnull(ThreadRequisition_Detail.POID, '') != '' ", dr["requestid"].ToString(), dr["Refno"].ToString(), dr["ThreadColorID"].ToString());
                        if (MyUtility.Check.Seek(chk))
                        {
                            ids += string.Format("Request ID: {0} , Refno: {1} , Color: {2} is already in LocalPO : {3}" + Environment.NewLine, dr["requestid"], dr["Refno"], dr["ThreadColorID"], dr["POID"]);
                        }
                    }
                }
                if (ids != "")
                {
                    return Result.F("Below request id already be created in Local PO, can't approve it!!" + Environment.NewLine + ids);
                }
            }
            #endregion

            #region 開始更新相關table資料
            if ((CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "SP_THREAD" || CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "EMB_THREAD"))
            {
                //針對表身資料將ThreadRequisition_Detail.poid塞值
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        if (dr["requestid", DataRowVersion.Original].ToString() != "")
                        {
                            sqlupd2 += string.Format(@"update ThreadRequisition_Detail set POID='' " +
                                    "where OrderID='{0}' and Refno='{1}' and ThreadColorID='{2}'; "
                                    , dr["requestid", DataRowVersion.Original].ToString(), dr["refno", DataRowVersion.Original].ToString(), dr["threadcolorid", DataRowVersion.Original].ToString());
                        }
                    }
                }
            }

            if ((CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "SP_THREAD" || CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "EMB_THREAD"))
            {
                //針對表身資料將ThreadRequisition_Detail.poid塞值
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified)
                    {
                        if (dr["requestid"].ToString() != "")
                        {
                            sqlupd2 += string.Format(@"update ThreadRequisition_Detail set POID='{0}' " +
                                    "where OrderID='{1}' and Refno='{2}' and ThreadColorID='{3}'; "
                                    , CurrentMaintain["id"].ToString(), dr["requestid"].ToString(), dr["refno"].ToString(), dr["threadcolorid"].ToString());
                        }
                    }
                }
            }
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!MyUtility.Check.Empty(sqlupd2))
                    {
                        if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                        {
                            _transactionscope.Dispose();
                            return result2;
                        }
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    DualResult er = Result.F("Commit transaction error.", ex);
                    return er;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            #endregion

            #region 天地板

            if (dtPadBoardInfo != null)
            {
                if (dtPadBoardInfo.Rows.Count > 0)
                {
                    //根據不同供應商，寫入LocalPO
                    List<string> SupplyList = dtPadBoardInfo.AsEnumerable().Select(o => o.Field<string>("localsuppid")).Distinct().ToList();

                    TransactionScope _transactionscope2 = new TransactionScope();
                    using (_transactionscope2)
                    {
                        try
                        {
                            //用於顯示MessageBox
                            List<string> msg_Id_List = new List<string>();
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            StringBuilder sql = new StringBuilder();

                            #region 新增LocalPO（表頭)

                            #region 新增LocalPO_Detail（表身)

                            int IdCount = 1;
                            foreach (string Supplyer in SupplyList)
                            {
                                //↑↑↑↑供應商↑↑↑↑

                                dt = dtPadBoardInfo.AsEnumerable().Where(o => o.Field<string>("localsuppid") == Supplyer).CopyToDataTable();

                                //單號
                                //string ID = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "LP", "Localpo",DateTime.Now,2);
                                string head = this.CurrentMaintain["ID"].ToString().Substring(0, 9);
                                int idNumber = Convert.ToInt32(this.CurrentMaintain["ID"].ToString().Substring(9, 4));
                                string ID = head + string.Format("{0:0000}", idNumber + IdCount);
                                IdCount++;
                                msg_Id_List.Add(ID);

                                //採購物料總數(不知道寫哪個欄位，因此先註解)
                                // int totalQty = dt.AsEnumerable().Sum(o => Convert.ToInt32(o.GetValue("Qty")));
                                Decimal amount = 0;

                                foreach (DataRow item in dt.Rows)
                                {
                                    //加總金額
                                    amount += MyUtility.Convert.GetDecimal(item["Price"]) * MyUtility.Convert.GetDecimal(item["Qty"]);

                                    parameters.Clear();
                                    parameters.Add(new SqlParameter("@Id", ID));
                                    parameters.Add(new SqlParameter("@OrderId", item["OrderId"]));
                                    parameters.Add(new SqlParameter("@Refno", item["Refno"]));
                                    parameters.Add(new SqlParameter("@ThreadColorID", item["ThreadColorID"]));
                                    parameters.Add(new SqlParameter("@Price", item["Price"]));
                                    parameters.Add(new SqlParameter("@Qty", item["Qty"]));
                                    parameters.Add(new SqlParameter("@UnitId", item["UnitId"]));
                                    parameters.Add(new SqlParameter("@RequestID", item["RequestID"]));
                                    parameters.Add(new SqlParameter("@Delivery", item["Delivery"]));
                                    parameters.Add(new SqlParameter("@Remark", item["Remark"]));
                                    parameters.Add(new SqlParameter("@POID", item["POID"]));
                                    parameters.Add(new SqlParameter("@BuyerID", item["BuyerID"]));

                                    sql.Clear();
                                    sql.Append(" INSERT INTO [dbo].[LocalPO_Detail]" + Environment.NewLine);
                                    sql.Append("                    ([Id]" + Environment.NewLine);
                                    sql.Append("                    ,[OrderId]" + Environment.NewLine);
                                    sql.Append("                    ,[Refno]" + Environment.NewLine);
                                    sql.Append("                    ,[ThreadColorID]" + Environment.NewLine);
                                    sql.Append("                    ,[Price]" + Environment.NewLine);
                                    sql.Append("                    ,[Qty]" + Environment.NewLine);
                                    sql.Append("                    ,[UnitId]" + Environment.NewLine);
                                    sql.Append("                    ,[RequestID]" + Environment.NewLine);
                                    sql.Append("                    ,[Delivery]" + Environment.NewLine);
                                    sql.Append("                    ,[Remark]" + Environment.NewLine);
                                    sql.Append("                    ,[POID]" + Environment.NewLine);
                                    sql.Append("                    ,[BuyerID])" + Environment.NewLine);

                                    sql.Append(" VALUES" + Environment.NewLine);
                                    sql.Append(" (@ID" + Environment.NewLine);
                                    sql.Append(" ,@OrderId" + Environment.NewLine);
                                    sql.Append(" ,@Refno" + Environment.NewLine);
                                    sql.Append(" ,@ThreadColorID" + Environment.NewLine);
                                    sql.Append(" ,@Price" + Environment.NewLine);
                                    sql.Append(" ,@Qty" + Environment.NewLine);
                                    sql.Append(" ,@UnitId" + Environment.NewLine);
                                    sql.Append(" ,@RequestID" + Environment.NewLine);
                                    sql.Append(" ,@Delivery" + Environment.NewLine);
                                    sql.Append(" ,@Remark" + Environment.NewLine);
                                    sql.Append(" ,@POID" + Environment.NewLine);

                                    sql.Append(" ,@BuyerID)" + Environment.NewLine);

                                    Sci.Data.DBProxy.Current.Execute(null, sql.ToString(), parameters);

                                }

                                #endregion


                                #region SqlParameter設定
                                parameters.Clear();

                                string CurrencyId = MyUtility.GetValue.Lookup($"SELECT DISTINCT CurrencyID FROM LocalSupp WHERE ID='{Supplyer}'");
                                decimal VatRate = Convert.ToDecimal(this.CurrentMaintain["VatRate"]);
                                decimal Vat = (VatRate/100) * amount;
                                //decimal total = amount + Vat;

                                parameters.Add(new SqlParameter("@Id", ID));
                                parameters.Add(new SqlParameter("@MDivisionID", this.CurrentMaintain["MDivisionID"]));
                                parameters.Add(new SqlParameter("@FactoryId", this.CurrentMaintain["FactoryId"]));
                                parameters.Add(new SqlParameter("@LocalSuppID", Supplyer));
                                parameters.Add(new SqlParameter("@Category", this.CurrentMaintain["Category"]));

                                parameters.Add(new SqlParameter("@IssueDate", this.CurrentMaintain["IssueDate"]));
                                parameters.Add(new SqlParameter("@Remark", this.CurrentMaintain["Remark"]));
                                parameters.Add(new SqlParameter("@CurrencyId", CurrencyId));
                                parameters.Add(new SqlParameter("@Amount", amount));
                                parameters.Add(new SqlParameter("@VatRate", VatRate));

                                parameters.Add(new SqlParameter("@Vat", Vat));
                                parameters.Add(new SqlParameter("@InternalRemark", this.CurrentMaintain["InternalRemark"]));
                                parameters.Add(new SqlParameter("@ApvName", this.CurrentMaintain["ApvName"]));
                                parameters.Add(new SqlParameter("@ApvDate", this.CurrentMaintain["ApvDate"]));
                                parameters.Add(new SqlParameter("@AddName", Sci.Env.User.UserID));

                                parameters.Add(new SqlParameter("@AddDate", DateTime.Now));
                                parameters.Add(new SqlParameter("@EditName", DBNull.Value));
                                parameters.Add(new SqlParameter("@EditDate", DBNull.Value));
                                parameters.Add(new SqlParameter("@Status", this.CurrentMaintain["Status"]));
                                #endregion

                                #region SQL設定

                                sql.Clear();
                                sql.Append(" INSERT INTO [dbo].[LocalPO]" + Environment.NewLine);
                                sql.Append("             ([Id]" + Environment.NewLine);
                                sql.Append("             ,[MDivisionID]" + Environment.NewLine);
                                sql.Append("             ,[FactoryId]" + Environment.NewLine);
                                sql.Append("             ,[LocalSuppID]" + Environment.NewLine);
                                sql.Append("             ,[Category]" + Environment.NewLine);
                                sql.Append("             ,[IssueDate]" + Environment.NewLine);
                                sql.Append("             ,[Remark]" + Environment.NewLine);
                                sql.Append("             ,[CurrencyId]" + Environment.NewLine);
                                sql.Append("             ,[Amount]" + Environment.NewLine);
                                sql.Append("             ,[VatRate]" + Environment.NewLine);
                                sql.Append("             ,[Vat]" + Environment.NewLine);
                                sql.Append("             ,[InternalRemark]" + Environment.NewLine);
                                sql.Append("             ,[ApvName]" + Environment.NewLine);
                                sql.Append("             ,[ApvDate]" + Environment.NewLine);
                                sql.Append("             ,[AddName]" + Environment.NewLine);
                                sql.Append("             ,[AddDate]" + Environment.NewLine);
                                sql.Append("             ,[EditName]" + Environment.NewLine);
                                sql.Append("             ,[EditDate]" + Environment.NewLine);
                                sql.Append("             ,[Status])" + Environment.NewLine);
                                sql.Append(" VALUES" + Environment.NewLine);
                                sql.Append(" (@ID" + Environment.NewLine);
                                sql.Append(" ,@MDivisionID" + Environment.NewLine);
                                sql.Append(" ,@FactoryId" + Environment.NewLine);
                                sql.Append(" ,@LocalSuppID" + Environment.NewLine);
                                sql.Append(" ,@Category" + Environment.NewLine);
                                sql.Append(" ,@IssueDate" + Environment.NewLine);
                                sql.Append(" ,@Remark" + Environment.NewLine);
                                sql.Append(" ,@CurrencyId" + Environment.NewLine);
                                sql.Append(" ,@Amount" + Environment.NewLine);
                                sql.Append(" ,@VatRate" + Environment.NewLine);
                                sql.Append(" ,@Vat" + Environment.NewLine);
                                sql.Append(" ,@InternalRemark" + Environment.NewLine);
                                sql.Append(" ,@ApvName" + Environment.NewLine);
                                sql.Append(" ,@ApvDate" + Environment.NewLine);
                                sql.Append(" ,@AddName" + Environment.NewLine);
                                sql.Append(" ,@AddDate" + Environment.NewLine);
                                sql.Append(" ,@EditName" + Environment.NewLine);
                                sql.Append(" ,@EditDate" + Environment.NewLine);
                                sql.Append(" ,@Status)" + Environment.NewLine);
                                #endregion

                                Sci.Data.DBProxy.Current.Execute(null, sql.ToString(), parameters);
                            }

                            #endregion
                            _transactionscope2.Complete();
                            _transactionscope2.Dispose();

                            MyUtility.Msg.InfoBox("Auto create purchase order :" + string.Join(",", msg_Id_List.ToArray()));
                        }
                        catch (Exception ex)
                        {
                            _transactionscope2.Dispose();
                            ShowErr("Commit transaction error.", ex);
                        }
                    }
                    _transactionscope2.Dispose();
                    _transactionscope2 = null;
                }

            }
            #endregion

            return base.ClickSave();
        }

        protected override DualResult ClickDelete()
        {
            DataTable dt = (DataTable)detailgridbs.DataSource;
            String sqlupd2 = "";
            DualResult result2;
            if ((CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "SP_THREAD" || CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "EMB_THREAD"))
            {
                //針對表身資料將ThreadRequisition_Detail.poid塞值
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["requestid", DataRowVersion.Original].ToString() != "")
                    {
                        sqlupd2 += string.Format(@"update ThreadRequisition_Detail set POID='' " +
                                "where OrderID='{0}' and Refno='{1}' and ThreadColorID='{2}'; "
                                , dr["requestid", DataRowVersion.Original].ToString(), dr["refno", DataRowVersion.Original].ToString(), dr["threadcolorid", DataRowVersion.Original].ToString());
                    }
                }
            }
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!MyUtility.Check.Empty(sqlupd2))
                    {
                        if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                        {
                            _transactionscope.Dispose();
                            return result2;
                        }
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    DualResult er = Result.F("Commit transaction error.", ex);
                    return er;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            return base.ClickDelete();
        }
        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {

            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            DataTable detailDt = (DataTable)detailgridbs.DataSource;
            if (!(CurrentMaintain == null))
            {
                if (!(CurrentMaintain["amount"] == DBNull.Value) && !(CurrentMaintain["vat"] == DBNull.Value))
                {
                    decimal amount = (decimal)CurrentMaintain["amount"] + (decimal)CurrentMaintain["vat"];
                    numTotal.Text = amount.ToString();
                }
                //[Apv. Date]格式調整，僅顯示YYYY/MM/DD
                if (!(CurrentMaintain["ApvDate"] == DBNull.Value)) displayApvDate.Text = Convert.ToDateTime(CurrentMaintain["ApvDate"]).ToShortDateString();
                else displayApvDate.Text = "";
            }
            txtsubconSupplier.Enabled = !this.EditMode || IsDetailInserting;
            txtartworktype_ftyCategory.Enabled = !this.EditMode || IsDetailInserting;
            txtmfactory.Enabled = !this.EditMode || IsDetailInserting;
            #region Status Label
            label25.Text = CurrentMaintain["status"].ToString();
            #endregion

            #region Batch Import, Special record button
            btnImportThread.Enabled = this.EditMode;
            btnBatchUpdateDellivery.Enabled = this.EditMode;
            #endregion

            detailDt.AcceptChanges();
            calttlqty();
        }

        protected override void OnDetailDetached()
        {
            //使用者可能會多次 Import，dtPadBoardInfo 清空的時間點應該是在每一次進入 Detail 時
            dtPadBoardInfo = new DataTable();
            base.OnDetailDetached();
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["qty"] = 0;
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region SP# Vaild 判斷此sp#的cateogry存在 order_tmscost
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            DataRow dr;
            ts4.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                DataRow drr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    CurrentDetailData["orderid"] = "";
                    CurrentDetailData["factoryid"] = "";
                    CurrentDetailData["poid"] = "";
                    CurrentDetailData["StyleID"] = "";
                    CurrentDetailData["SciDelivery"] = DBNull.Value;
                    CurrentDetailData["sewinline"] = DBNull.Value;
                    CurrentDetailData["BuyerID"] = "";
                    return;
                }
                if (e.FormattedValue.ToString() == drr["orderid"].ToString()) return;

                if (!this.EditMode && (CurrentMaintain["status"].ToString().ToUpper() == "Approved"))
                {
                    if (MyUtility.Check.Seek(string.Format(@"
select price 
from order_tmscost ot WITH (NOLOCK) 
left join orders o on o.id = ot.id
inner join factory WITH (NOLOCK) on o.FactoryID = factory.id
outer apply (
	select ShipQty= isnull(sum(ShipQty),0)  from Pullout_Detail where OrderID=ot.ID
) pd
outer apply(
	select DiffQty= isnull(SUM(isnull(DiffQty ,0)),0) 
	from InvAdjust I
	left join InvAdjust_Qty IQ on I.ID=IQ.ID
	where OrderID=ot.ID
) inv
where ot.id = '{0}'
 and artworktypeid = '{1}' and o.Category in ('B','S','T')
and factory.IsProduceFty = 1
and (o.Qty-pd.ShipQty-inv.DiffQty <> 0 or o.Category='T')  "
                        , e.FormattedValue, CurrentMaintain["category"]), out dr, null))
                    {
                        if ((decimal)dr["price"] == 0m)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("TmsCost price is Zero", "Warning");
                            return;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("SP# is not in Order_TmsCost", "Data not found");
                        return;
                    }
                }
                if (MyUtility.Check.Seek(string.Format(@"
select FactoryID,POID,StyleID,SciDelivery,sewinline,Brand.BuyerID
from orders  WITH (NOLOCK)  
inner join factory WITH (NOLOCK) on orders.FactoryID = factory.id
inner join Brand WITH (NOLOCK) on orders.BrandID = Brand.ID
outer apply (
	select ShipQty= isnull(sum(ShipQty),0)  from Pullout_Detail where OrderID=orders.ID
) pd
outer apply(
	select DiffQty= isnull(SUM(isnull(DiffQty ,0)),0) 
	from InvAdjust I
	left join InvAdjust_Qty IQ on I.ID=IQ.ID
	where OrderID=orders.ID
) inv
where orders.id = '{0}' and orders.MDivisionID='{1}' 
and orders.Category  in ('B','S','T') and orders.Junk=0 and Finished=0
and factory.IsProduceFty = 1 
and (orders.Qty-pd.ShipQty-inv.DiffQty <> 0 or orders.Category='T')
 "
                    , e.FormattedValue, Sci.Env.User.Keyword), out dr, null))
                {
                    CurrentDetailData["orderid"] = e.FormattedValue;
                    CurrentDetailData["factoryid"] = dr["FactoryID"];
                    CurrentDetailData["poid"] = dr["POID"];
                    CurrentDetailData["StyleID"] = dr["StyleID"];
                    CurrentDetailData["SciDelivery"] = dr["SciDelivery"];
                    CurrentDetailData["sewinline"] = dr["sewinline"];
                    CurrentDetailData["BuyerID"] = dr["BuyerID"];
                    this.CurrentDetailData["price"] = this.GetPrice(CurrentDetailData["RefNo"].ToString(), CurrentDetailData["BuyerID"].ToString(), CurrentDetailData["Threadcolorid"].ToString());

                    if (!MyUtility.Check.Empty(CurrentDetailData["price"]) && !MyUtility.Check.Empty(CurrentDetailData["Qty"]))
                    {
                        CurrentDetailData["amount"] = MyUtility.Convert.GetDecimal(CurrentDetailData["price"]) * MyUtility.Convert.GetDecimal(CurrentDetailData["Qty"]);
                    }
                }
                else
                {
                    CurrentDetailData["orderid"] = "";
                    CurrentDetailData["factoryid"] = "";
                    CurrentDetailData["poid"] = "";
                    CurrentDetailData["StyleID"] = "";
                    CurrentDetailData["SciDelivery"] = DBNull.Value;
                    CurrentDetailData["sewinline"] = DBNull.Value;
                    CurrentDetailData["BuyerID"] = "";
                    MyUtility.Msg.ErrorBox("< SP# :" + e.FormattedValue + " > not found!!!");
                    return;
                }

            };
            #endregion

            #region Refno 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    if (!MyUtility.Check.Empty(CurrentDetailData["Requestid"]))
                    {
                        return;
                    }
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                         (string.Format(@"Select refno,description, localsuppid,unitid,price
                                                    from localItem WITH (NOLOCK) where category ='{0}' and  localsuppid = '{1}' order by refno", CurrentMaintain["category"], CurrentMaintain["localsuppid"])
                                                                                                                                  , "15,30,8,8,10", "", null, "0,0,0,0,4");
                    item.Size = new System.Drawing.Size(795, 535);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    IList<DataRow> x = item.GetSelecteds();
                    CurrentDetailData["refno"] = x[0][0];
                    CurrentDetailData["unitid"] = x[0][3];
                    CurrentDetailData.EndEdit();
                }
            };
            ts.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue) || !this.EditMode) return;
                if (!MyUtility.Check.Seek(string.Format(@"select refno,unitid,price from localitem WITH (NOLOCK) 
                                                                      where refno = '{0}' and category ='{1}'and localsuppid = '{2}'"
                                                                , e.FormattedValue.ToString(), CurrentMaintain["category"], CurrentMaintain["localsuppid"])
                                                                , out dr, null))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Data not found!", "Ref#");
                    return;
                }
                else
                {
                    CurrentDetailData["refno"] = dr[0];
                    CurrentDetailData["unitid"] = dr[1];
                    this.CurrentDetailData["price"] = this.GetPrice(CurrentDetailData["RefNo"].ToString(), CurrentDetailData["BuyerID"].ToString(), CurrentDetailData["Threadcolorid"].ToString());

                    if (!MyUtility.Check.Empty(CurrentDetailData["price"]) && !MyUtility.Check.Empty(CurrentDetailData["Qty"]))
                    {
                        CurrentDetailData["amount"] = MyUtility.Convert.GetDecimal(CurrentDetailData["price"]) * MyUtility.Convert.GetDecimal(CurrentDetailData["Qty"]);
                    }
                }
                CurrentDetailData.EndEdit();
            };
            #endregion

            #region Color shase 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    if (!(CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "SP_THREAD"
                       || CurrentMaintain["category"].ToString().ToUpper().TrimEnd() == "EMB_THREAD")
                       || !MyUtility.Check.Empty(CurrentDetailData["Requestid"]))
                        return;
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                        (@"Select ID,description from threadcolor WITH (NOLOCK) where JUNK=0 order by ID", "10,45", null);
                    item.Size = new System.Drawing.Size(630, 535);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    CurrentDetailData["Threadcolorid"] = item.GetSelectedString();
                    CurrentDetailData.EndEdit();
                }
            };
            ts2.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue) || !(CurrentMaintain["category"].ToString().ToUpper() == "SP_THREAD" || CurrentMaintain["category"].ToString().ToUpper() == "EMB_THREAD"))
                {
                    //e.Cancel = true;
                    return;
                }
                if (!MyUtility.Check.Seek(string.Format(@"select junk from ThreadColor WITH (NOLOCK) 
                                                                      where id = '{0}' and junk=0 "
                                                                , e.FormattedValue.ToString())
                                                                , out dr, null))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Data not found!", "Color Shade");
                    return;
                }

                CurrentDetailData["threadColorid"] = e.FormattedValue;
                this.CurrentDetailData["price"] = this.GetPrice(CurrentDetailData["RefNo"].ToString(), CurrentDetailData["BuyerID"].ToString(), CurrentDetailData["Threadcolorid"].ToString());

                if (!MyUtility.Check.Empty(CurrentDetailData["price"]) && !MyUtility.Check.Empty(CurrentDetailData["Qty"]))
                {
                    CurrentDetailData["amount"] = MyUtility.Convert.GetDecimal(CurrentDetailData["price"]) * MyUtility.Convert.GetDecimal(CurrentDetailData["Qty"]);
                }
                CurrentDetailData.EndEdit();
            };
            #endregion

            #region Qty Valid
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["amount"] = (decimal)CurrentDetailData["price"] * (decimal)e.FormattedValue;
                    CurrentDetailData["qty"] = e.FormattedValue;
                    CurrentDetailData.EndEdit();
                }
            };
            #endregion

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.EditingMouseDoubleClick += (s, e) =>
            {
                if (EditMode) return;
                var frm = new Sci.Production.Subcon.P30_InComingList(CurrentDetailData["Ukey"].ToString());
                DialogResult result = frm.ShowDialog(this);
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns3 = new DataGridViewGeneratorNumericColumnSettings();
            ns3.EditingMouseDoubleClick += (s, e) =>
            {
                if (EditMode) return;
                var frm = new Sci.Production.Subcon.P30_AccountPayble(CurrentDetailData["Ukey"].ToString());
                DialogResult result = frm.ShowDialog(this);
            };

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("factoryid", header: "Order Factory", iseditingreadonly: true)  //0
            .Text("POID", header: "MasterSP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //1
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), settings: ts4)  //2
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)  //3
            .Date("SciDelivery", header: "Sci Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)   //4
            .Date("sewinline", header: "SewInLine", width: Widths.AnsiChars(10), iseditingreadonly: true)   //5
            .Text("refno", header: "Ref#", width: Widths.AnsiChars(20), settings: ts).Get(out col_Ref)    //6
            .Text("threadColorid", header: "Color Shade", settings: ts2).Get(out col_color)    //7
            .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)   //8
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(6), decimal_places: 0, integer_places: 6, settings: ns).Get(out col_Qty)    //9
            .Text("Unitid", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)   //10
            .Numeric("price", header: "Price", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 4, iseditingreadonly: true) //11
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(9), iseditingreadonly: true, decimal_places: 2, integer_places: 14)  //12
            .Numeric("std_price", header: "Standard Price", width: Widths.AnsiChars(6), decimal_places: 3, integer_places: 4, iseditingreadonly: true) //13
            .Date("delivery", header: "Delivery", width: Widths.AnsiChars(10)) //14
            .Text("Requestid", header: "Request ID", width: Widths.AnsiChars(13), iseditingreadonly: true) //15
            .Numeric("inqty", header: "In Qty", width: Widths.AnsiChars(6), decimal_places: 0, integer_places: 6, iseditingreadonly: true, settings: ns2) //16
            .Numeric("apqty", header: "AP Qty", width: Widths.AnsiChars(6), decimal_places: 0, integer_places: 6, iseditingreadonly: true, settings: ns3) //17
            .Text("remark", header: "Remark", width: Widths.AnsiChars(25)) //18
            .Text("BuyerID", header: "Buyer", width: Widths.AnsiChars(6), iseditingreadonly: true) //19
            ;
            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns["orderid"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["refno"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["threadColorid"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["delivery"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["remark"].DefaultCellStyle.BackColor = Color.Pink;

            #endregion
            this.detailgrid.RowEnter += detailgrid_RowEnter;
            change_record();
        }

        Ict.Win.UI.DataGridViewTextBoxColumn col_Ref;
        Ict.Win.UI.DataGridViewTextBoxColumn col_color;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
        private void detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || EditMode == false) { return; }
            var data = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null) { return; }

            if (!MyUtility.Check.Empty(data["Requestid"]))
            {
                col_Ref.IsEditingReadOnly = true;
                col_color.IsEditingReadOnly = true;
                col_Qty.IsEditingReadOnly = true;
            }
            else
            {
                col_Ref.IsEditingReadOnly = false;
                col_color.IsEditingReadOnly = false;
                col_Qty.IsEditingReadOnly = false;
            }
        }

        private decimal GetPrice(string refNo, string buyerID, string threadColorID)
        {
            string sqlCmd = $@"select Price from LocalItem_ThreadBuyerColorGroupPrice with (nolock) where Refno = @Refno and BuyerID = @BuyerID and ThreadColorGroupID = (select ThreadColorGroupID from ThreadColor with (nolock) where id = @threadColorID)";
            string sqlCmd2 = $@"select Price from LocalItem_ThreadBuyerColorGroupPrice with (nolock) where Refno = @Refno and BuyerID = '' and ThreadColorGroupID = (select ThreadColorGroupID from ThreadColor with (nolock) where id = @threadColorID)";
            string sqlCmd3 = $@"select Price from LocalItem with (nolock) where Refno = @Refno";
            List<SqlParameter> sqlPar = new List<SqlParameter>() {  new SqlParameter("@Refno", refNo),
                                                                    new SqlParameter("@BuyerID", buyerID),
                                                                    new SqlParameter("@threadColorID", threadColorID)
                                                                  };
            DataTable resultDt;
            DualResult result;
            result = DBProxy.Current.Select(null, sqlCmd, sqlPar, out resultDt);
            if (result)
            {
                if (resultDt.Rows.Count > 0)
                {
                    return (decimal)resultDt.Rows[0][0];
                }
            }
            else
            {
                this.ShowErr(result);
                return 0;
            }

            result = DBProxy.Current.Select(null, sqlCmd2, sqlPar, out resultDt);
            if (result)
            {
                if (resultDt.Rows.Count > 0)
                {
                    return (decimal)resultDt.Rows[0][0];
                }
            }
            else
            {
                this.ShowErr(result);
                return 0;
            }

            result = DBProxy.Current.Select(null, sqlCmd3, sqlPar, out resultDt);
            if (result)
            {
                if (resultDt.Rows.Count > 0)
                {
                    return (decimal)resultDt.Rows[0][0];
                }
            }
            else
            {
                this.ShowErr(result);
                return 0;
            }
            return 0;
        }

        private void change_record()
        {
            col_Ref.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Requestid"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;

                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            col_color.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Requestid"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;

                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            col_Qty.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Requestid"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;

                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

        }
        // import thread or carton request
        private void btnImportThread_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            //if (MyUtility.Check.Empty(dr["localsuppid"]))
            //{
            //    MyUtility.Msg.WarningBox("Please fill Supplier first!");
            //    txtsubconSupplier.TextBox1.Focus();
            //    return;
            //}
            if (MyUtility.Check.Empty(dr["category"]))
            {
                MyUtility.Msg.WarningBox("Please fill category first!");
                txtartworktype_ftyCategory.Focus();
                return;
            }
            DataTable dg = (DataTable)detailgridbs.DataSource;
            if (dg.Columns["std_price"] == null) dg.Columns.Add("std_price", typeof(Decimal));
            var frm = new Sci.Production.Subcon.P30_Import(dr, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();

            calttlqty();
        }

        private void txtartworktype_ftyCategory_Validated(object sender, EventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                if (detailgridbs.DataSource != null && ((DataTable)detailgridbs.DataSource).Rows.Count > 0)
                {
                    ((DataTable)detailgridbs.DataSource).Rows.Clear();
                }
            }
        }

        //Approve
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            string sqlupd3 = string.Format("update Localpo set status='Approved', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                               "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            DualResult result;
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
        }

        //unApprove
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable dt = (DataTable)detailgridbs.DataSource;
            DataRow[] drs = dt.Select("apqty > 0");
            if (drs.Length != 0)
            {
                MyUtility.Msg.WarningBox("Detail data has AP Qty, can't unApprove!", "Warning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlupd3 = "";
            DualResult result;

            sqlupd3 = string.Format(@"update Localpo set status='New',apvname='', apvdate = null , editname = '{0}' 
                                                    , editdate = GETDATE() where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

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
        }

        // Close
        protected override void ClickClose()
        {
            base.ClickClose();
            string sqlupd3 = string.Format("update Localpo set status='Closed', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                              "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            DualResult result;
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

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Close successful");
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
        }

        //Unclose
        protected override void ClickUnclose()
        {
            base.ClickUnclose();

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to UnClose it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            String sqlupd3 = "";
            DualResult result;

            sqlupd3 = string.Format(@"update Localpo set status='Approved',apvname='', apvdate = null , editname = '{0}' 
                                                    , editdate = GETDATE() where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

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

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnClose successful");
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
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            string category = (e.Master == null) ? "" : MyUtility.GetValue.Lookup($@"select category from LocalPO
where id='{e.Master["ID"].ToString()}' ");

            this.DetailSelectCommand = $@"
select [Selected] = 0,[Amount]= isnull(loc2.Price*loc2.Qty,0),[std_price]=isnull(std.std_price,0),*,o.factoryid,o.sewinline,loc.description
from localpo_detail loc2 WITH (NOLOCK) 
left join orders o WITH (NOLOCK) on loc2.orderid = o.id
left join localitem loc WITH (NOLOCK) on loc.refno = loc2.refno 
outer apply(
	select [std_price]=round(sum(a.qty*b.Price)/iif(isnull(sum(a.qty),0)=0,1,isnull(sum(a.qty),0)),3) 
	from orders a WITH (NOLOCK) 
	inner join Order_TmsCost b WITH (NOLOCK) on b.id = a.ID
	where a.poid = loc2.POID and b.ArtworkTypeID='{category}'
)std
Where loc2.id = '{masterID}' order by loc2.orderid,loc2.refno,threadcolorid
";

            return base.OnDetailSelectCommandPrepare(e);

        }

        protected override bool ClickNewBefore()
        {
            //            this.DetailSelectCommand = string.Format(@"select * ,0.0 as amount,orders.factoryid,orders.sewinline,localitem.description
            //                                                        from localpo_detail 
            //                                                            inner join orders on localpo_detail.orderid = orders.id
            //                                                            inner join localitem on localitem.refno = localpo_detail.refno 
            //                                                        where 1=2 order by orderid,localpo_detail.refno,threadcolorid ");
            this.DetailSelectCommand = string.Format(@"select * ,0.0 as amount,orders.factoryid,orders.sewinline,localitem.description
                                                        from localpo_detail WITH (NOLOCK) 
                                                            left join orders WITH (NOLOCK) on localpo_detail.orderid = orders.id
                                                            left join localitem WITH (NOLOCK) on localitem.refno = localpo_detail.refno 
                                                        where 1=2 order by orderid,localpo_detail.refno,threadcolorid ");
            return base.ClickNewBefore();
        }

        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString().Trim();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString().Trim();

            P30_Print callPrintForm = new P30_Print(row, id, issuedate);
            callPrintForm.ShowDialog(this);

            return true;
        }

        private void btnBatchUpdateDellivery_Click(object sender, EventArgs e)
        {
            //int deleteIndex = 0;
            foreach (DataGridViewRow dr in this.detailgrid.Rows)
            {
                DataRow row = ((DataRowView)dr.DataBoundItem).Row;
                if (row["selected"].Equals(1))
                {
                    if (this.dateDeliveryDate.Value != null)
                    {
                        row["Delivery"] = (DateTime)dateDeliveryDate.Value;
                    }

                    if (!MyUtility.Check.Empty(this.txtBuyer.Text))
                    {
                        if (!this.txtBuyer.Text.Equals(row["BuyerID"]))
                        {
                            row["BuyerID"] = this.txtBuyer.Text;
                            row["price"] = this.GetPrice(row["RefNo"].ToString(), row["BuyerID"].ToString(), row["Threadcolorid"].ToString());

                            if (!MyUtility.Check.Empty(row["price"]) && !MyUtility.Check.Empty(row["Qty"]))
                            {
                                row["amount"] = MyUtility.Convert.GetDecimal(row["price"]) * MyUtility.Convert.GetDecimal(row["Qty"]);
                            }
                        }
                    }
                }

            }
        }

        protected override void OnDetailGridAppendClick()
        {
            base.OnDetailGridAppendClick();
            this.CurrentDetailData["RequestID"] = this.CurrentDetailData["RequestID"].Equals(DBNull.Value) ? "" : this.CurrentDetailData["RequestID"];

        }

        void calttlqty()
        {
            if (DetailDatas.Count > 0)
            {
                numttlqty.Value = MyUtility.Convert.GetDecimal(((DataTable)detailgridbs.DataSource).Compute("sum(qty)", ""));
            }
        }

        private void txtBuyer_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                   (@"Select ID,NameEN  from Buyer WITH (NOLOCK) where JUNK=0 order by ID", "10,45", null);
            item.Size = new System.Drawing.Size(630, 535);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.txtBuyer.Text = item.GetSelectedString();
        }

        private void txtBuyer_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Seek($@"select 1 from Buyer WITH (NOLOCK) where junk=0 and id='{this.txtBuyer.Text}'"))
            {
                MyUtility.Msg.WarningBox("data not found!");
                e.Cancel = true;
            }
        }

        private void txtartworktype_ftyCategory_TextChanged(object sender, EventArgs e)
        {
            if (this.txtartworktype_ftyCategory.Text.EqualString("SP_Thread")
                || this.txtartworktype_ftyCategory.Text.EqualString("EMB_Thread"))
            {
                this.GridUniqueKey = "orderid,refno,threadcolorid";
            }
            else
            {
                this.GridUniqueKey = "orderid,refno,threadcolorid,Requestid";
            }
        }
    }
}

