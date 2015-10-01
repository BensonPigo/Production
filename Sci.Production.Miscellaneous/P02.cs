using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Win;
using Sci.Production.Class;
using System.Transactions;
namespace Sci.Production.Miscellaneous
{
    public partial class P02 : Sci.Win.Tems.Input6
    {
        private string factory = Sci.Env.User.Factory;
        private string loginID = Sci.Env.User.UserID;
        private string keyword = Sci.Env.User.Keyword;
        private int exact = 2;
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            string defaultfilter = string.Format("PurchaseFrom='L' and factoryid = '{0}'", factory);
            this.DefaultFilter = defaultfilter;
            this.InsertDetailGridOnDoubleClick = false;
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label10.Text = CurrentMaintain["Status"].ToString();

            if (!string.IsNullOrWhiteSpace(CurrentMaintain["Amount"].ToString()))
            {
                this.numericBox4.Value = (decimal)CurrentMaintain["Amount"] + (decimal)CurrentMaintain["Vat"];
            }
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            CurrentMaintain.AcceptChanges(); 
            CurrentMaintain["Approve"] = loginID;
            CurrentMaintain["ApproveDate"] = DateTime.Now;
            CurrentMaintain["Status"] = "Approved";
            updateMaster();
            this.RenewData();
            EnsureToolbarExt();

        }
        protected override void OnDetailUIConvertToMaintain()
        {
            base.OnDetailUIConvertToMaintain();
            txtuser2.TextBox1.ReadOnly = true;
        }
        protected override void ClickUnconfirm()
        {

            base.ClickUnconfirm();
            if (!this.DetailDatas.All(x => (decimal)x["InQty"] == 0))//判斷InQty不可 大於0的值
            {
                IEnumerable<DataRow> query = this.DetailDatas.Where(y => (decimal)y["InQty"] > 0);
                string seq = "SEQ1,SEQ2:\r";
                foreach (DataRow dr in query) //List 出InQty>0
                {
                    seq += dr["SEQ1"].ToString() + "," + dr["SEQ2"].ToString() + "\r";
                }
                MyUtility.Msg.WarningBox(String.Format("InQty > 0,You can't unapprove! \rPlease see below SEQ \r{0}", seq));                return;
            }
            CurrentMaintain.AcceptChanges(); //非編輯狀態下一定要先下更動DataRow的狀態改為原來初值
            CurrentMaintain["Approve"] = "";
            CurrentMaintain["ApproveDate"] = DBNull.Value;
            CurrentMaintain["Status"] = "New";
            updateMaster();
            this.RenewData();
            EnsureToolbarExt();
            //this.OnDetailEntered();
        }
        protected override DualResult OnRenewDataDetailPost(Win.Tems.InputMasterDetail.RenewDataPostEventArgs e)
        {
             return base.OnRenewDataDetailPost(e);
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"Select 0 as sel,a.*, b.Description,b.MiscBrandid+'-'+c.Name as MiscBrand,
                b.inspect,d.purchaseType,a.price * a.Qty as amount
                from miscpo_detail a
                left join misc b on b.id = a.miscid
                left join miscbrand c on c.id = a.miscbrandid
                left join miscreq d on a.miscreqid = d.id
                Where a.id='{0}' ", masterID);            
            return base.OnDetailSelectCommandPrepare(e);
        }
        //新增之後給初始值
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Factoryid"] = factory;
            CurrentMaintain["cdate"] = DateTime.Today;
            CurrentMaintain["PurchaseFrom"] = "L";
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Handle"] = loginID;
            CurrentMaintain["Vat"] = 0;
            CurrentMaintain["Vatrate"] = 0;
            CurrentMaintain["Amount"] = 0;
            this.numericBox4.Value = 0;
        }   
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (CurrentMaintain["Status"].ToString() == "Approved" || CurrentMaintain["Status"].ToString() == "Closed")
            {
                this.detailgrid.IsEditingReadOnly = true;
                this.dateBox1.ReadOnly = true;
                this.txtsubcon1.TextBox1.ReadOnly = true;
                this.txtuser1.TextBox1.ReadOnly = true;
                this.txtuser2.TextBox1.ReadOnly = true;
                this.numericBox2.ReadOnly = true;
                this.gridicon.Remove.Enabled = false;
                this.button1.Enabled = false;
            }
            this.gridicon.Append.Enabled = false;
            this.gridicon.Insert.Enabled = false;
            
        }
        protected override bool ClickSaveBefore()
        {
            //確認不可空白欄位
            decimal amount = 0;
            if (MyUtility.Check.Empty(CurrentMaintain["cDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("<Create Date> can not be empty!");
                this.dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["LocalSuppid"].ToString()))
            {
                MyUtility.Msg.WarningBox("<Supplier> can not be empty!");
                this.txtsubcon1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("<Handle> can not be empty!");
                this.txtuser1.TextBox1.Focus();
                return false;
            }

     

            #region 填入表頭Amount, Vat 並將表身丟入Dictionary 內，在將Dictionary的Key(即為PartReqid) 填入Value(即為SEQ1)
            //寫入Dictionary
            Dictionary<string, string> mcs = new Dictionary<string, string>();
            foreach (DataRow dr in this.DetailDatas)
            {
                //刪除PartID,PartBrandid,Price,Qty 其一為空的明細
                if (MyUtility.Check.Empty(dr["Price"]) || MyUtility.Check.Empty(dr["Qty"]) || MyUtility.Check.Empty(dr["Miscid"])) 
                {
                    this.CurrentDetailData.Delete();
                }
                else
                {
                    amount += (decimal)dr["Price"] * (decimal)dr["Qty"];
                }

                //寫入Dictionary
                if (mcs.ContainsKey(dr["MiscReqID"].ToString()))
                {
                    if (!MyUtility.Check.Empty(dr["SEQ1"].ToString()))
                    {
                        mcs[dr["MiscReqID"].ToString()] = dr["SEQ1"].ToString(); //Key 是PartReqid, Value是SEQ1
                    }
                }
                else
                {
                    mcs.Add(dr["MiscReqID"].ToString(), dr["SEQ1"].ToString());
                }

            }

            exact = Convert.ToInt32(MyUtility.GetValue.Lookup("Exact", CurrentMaintain["Currencyid"].ToString(), "Currency", "ID", "Production"));
            CurrentMaintain["Amount"] = MyUtility.Math.Round(amount, exact);
            getMasterTotal(); //comput amount

            #endregion 

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("<Detail> can not be Empty!");
                return false;
            }

            //Sort 找出最大SEQ1
            string maxSeq1 = mcs.Max(x => x.Value);
            //填入Dictionary 的SEQ1
            Dictionary<string, string> emcs = mcs.Where(x => MyUtility.Check.Empty(x.Value)).ToDictionary(x => x.Key, p => p.Value); //這方式叫泛型 將Dictionary 丟入另一個Dictionary 才可做修改資料填入SEQ1
            foreach (string s in emcs.Keys) //foreach 不可修改自身迴圈資料
            {
                if (MyUtility.Check.Empty(mcs[s]))
                {
                    maxSeq1 = MyUtility.GetValue.GetSeq(maxSeq1);
                    mcs[s] = maxSeq1;
                }
            }
            //將Dictionary 的Value 填入表身IList的SEQ1
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["SEQ1"].ToString()))
                {
                    dr["SEQ1"] = mcs[dr["MiscReqID"].ToString()];
                    if (mcs[dr["MiscReqID"].ToString()] == "**")
                    {
                        MyUtility.Msg.WarningBox("<Miscellaneous Requisition#> too much cause <SEQ1> over 'Z9', please create new PO", "Miscellaneous Requisition too much");
                        return false;
                    }
                }
            }
            //填入SEQ2
            foreach (string k in mcs.Keys)
            {
                IList<DataRow> list = this.DetailDatas.Where<DataRow>(x => x["MiscReqID"].ToString() == k).ToList<DataRow>();
                string maxSeq2 = list.Max(x => x["SEQ2"].ToString());
                foreach (DataRow dr in list)
                {
                    if (MyUtility.Check.Empty(dr["SEQ2"].ToString()))
                    {
                        
                        maxSeq2 =  MyUtility.GetValue.GetSeq(maxSeq2);
                        dr["SEQ2"] = maxSeq2;
                        if (maxSeq2 == "**")
                        {
                            string msg = string.Format("<SEQ1> : {0},<SEQ2> over 'Z9', please create new PO for '{1}'", dr["SEQ1"].ToString(), dr["MiscReqID"].ToString());
                            MyUtility.Msg.WarningBox(msg, "MiscReq# too much");
                            return false;
                        }
                    }
                }
            }
            #region 填入ID
            if (this.IsDetailInserting)
            {
                string keyWord = keyword + "ML";
                string id = MyUtility.GetValue.GetID(keyWord, "MiscPo");
                if (string.IsNullOrWhiteSpace(id))
                {
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            #endregion
            return base.ClickSaveBefore();

        }
        
        private void txtsubcon1_Validated(object sender, EventArgs e)
        {
            base.OnValidated(e);
            CurrentMaintain["currencyId"] = MyUtility.GetValue.Lookup("CurrencyID", CurrentMaintain["LocalSuppid"].ToString(), "LocalSupp", "ID", "Production");
        }

        private void numericBox2_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                getMasterTotal();
            }
        }
        //算表頭Amount與Vat
        private void getMasterTotal()
        {
            exact = Convert.ToInt32(MyUtility.GetValue.Lookup("Exact", CurrentMaintain["Currencyid"].ToString(), "Currency", "ID", "Production"));

            CurrentMaintain["Vat"] = MyUtility.Math.Round((decimal)CurrentMaintain["Amount"] * (decimal)CurrentMaintain["VatRate"] / 100, exact);
            this.numericBox4.Value = (decimal)CurrentMaintain["Amount"] + (decimal)CurrentMaintain["Vat"];
        }

        //
        protected override void OnDetailGridSetup()
        {
            
            DataGridViewGeneratorNumericColumnSettings accCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings inCell = new DataGridViewGeneratorNumericColumnSettings();

            accCell.EditingMouseDoubleClick += (s, e) =>

            {
                Form P02_Accountpayable = new Sci.Production.Miscellaneous.P02_Accountpayable(CurrentMaintain["id"].ToString(), CurrentDetailData["seq1"].ToString(), CurrentDetailData["seq2"].ToString(), CurrentDetailData["miscid"].ToString(), CurrentDetailData["miscreqid"].ToString());
                P02_Accountpayable.ShowDialog();
            };

            inCell.EditingMouseDoubleClick += (s, e) =>
            {
                Form P02_Incoming = new Sci.Production.Miscellaneous.P02_Incoming(CurrentMaintain["id"].ToString(), CurrentDetailData["seq1"].ToString(), CurrentDetailData["seq2"].ToString(), CurrentDetailData["miscid"].ToString(), CurrentDetailData["miscreqid"].ToString());
                P02_Incoming.ShowDialog();
            };
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("MiscID", header: "Miscellaneous ID", width: Widths.AnsiChars(23), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("MiscBrand", header: "Brand", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Projectid", header: "Project Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Text("UnitID", header: "PO Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Price", header: "Unit Price", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 4, 
            iseditingreadonly: true)            
            .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 13, decimal_places: 3, iseditingreadonly: true)
            .Text("Departmentid", header: "Department", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("PurchaseType", header: "Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .CheckBox("Inspect",header:"Need to Inspect", width:Widths.AnsiChars(1),trueValue : 1,falseValue: 0,iseditable:false)
            .Numeric("InQty", header: "In-coming Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true, settings: inCell)
            .Numeric("InspQty", header: "Insp. Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("ApQty", header: "AP Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2,settings: accCell, iseditingreadonly: true)
            .Text("MiscReqId", header: "Misc Requisition#", width: Widths.AnsiChars(13), iseditingreadonly: true);

        }
        protected override bool ClickSavePost()
        {
            string updatePO="";
            DataTable delTb, sourTb = DetailDatas.CopyToDataTable();

            MyUtility.Tool.ProcessWithDatatable(sourTb, "MiscId,id", string.Format("select a.id,a.Miscid,b.id as id2 from Miscreq_Detail a left join #tmp b on a.Miscpoid = b.id and a.Miscid = b.Miscid  where a.Miscpoid='{0}'", CurrentMaintain["ID"].ToString()), out delTb);
            foreach (DataRow deldr in delTb.Rows)
            {
                if (deldr["id2"].ToString() == "")
                {
                    updatePO = updatePO + string.Format("update MiscReq_Detail set Miscpoid = '' where id = '{0}' and Miscid = '{1}';", deldr["ID"].ToString(), deldr["miscid"].ToString());
                }
            }
                          
            foreach(DataRow dr in this.DetailDatas)
            {
                updatePO = updatePO + string.Format("Update MiscReq_Detail set Miscpoid = '{0}' where id='{1}' and Miscid = '{2}';", CurrentMaintain["ID"].ToString(), dr["MiscReqid"].ToString(), dr["Miscid"].ToString());
            }

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {

                    if (!(upResult = DBProxy.Current.Execute(null, updatePO)))
                    {
                        ShowErr(updatePO, upResult);
                        return false;
                    }
                            
                    _transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return false;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            return base.ClickSavePost();
        }
        //按下Junk 鈕
        protected override void ClickJunk()
        {
            base.ClickJunk();
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MyUtility.Msg.WarningBox("Are you sure junk this data?", "Warning", buttons);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                CurrentMaintain.AcceptChanges();
                CurrentMaintain["Status"] = "Junked";
                updateMaster();
                this.RenewData();
                EnsureToolbarExt();
            }
        }

        //按下Close 鈕
        protected override void ClickClose()
        {
            base.ClickClose();
            CurrentMaintain.AcceptChanges();
            CurrentMaintain["Status"] = "Closed";
            updateMaster();
            this.RenewData();
            EnsureToolbarExt();
        }
        protected override void ClickUnclose()
        {
            base.ClickUnclose();
            CurrentMaintain.AcceptChanges();
            CurrentMaintain["Status"] = "Approved";
            updateMaster();
            this.RenewData();
            EnsureToolbarExt();
        }

        //更新Table
        private void updateMaster()
        {
            CurrentMaintain["EditName"] = loginID;
            CurrentMaintain["EditDate"] = DateTime.Now;
            try
            {
                bool change;
                DualResult duResult;
                ITableSchema ts;
                duResult = DBProxy.Current.GetTableSchema(null, "MiscPO", out ts);
                string connname = null;
                duResult = DBProxy.Current.UpdateByChanged(connname, ts, CurrentMaintain, out change);
                if (duResult)
                {
                    MyUtility.Msg.WarningBox("Successfully");
                }
                else
                {
                    MyUtility.Msg.WarningBox("Failed, please re-click!");
                }
            }
            catch (Exception ex)
            {
                ShowErr(ex);
                return;
            }
            this.OnDetailEntered();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable detTable = ((DataTable)this.detailgridbs.DataSource);
            if (MyUtility.Check.Empty(CurrentMaintain["LocalSuppid"]))
            {
                MyUtility.Msg.WarningBox("Please entry <Supplier> first");
                txtsubcon1.TextBox1.Focus();
                return;
            }
            Form P02_import = new Sci.Production.Miscellaneous.P02_Import(detTable, CurrentMaintain["LocalSuppid"].ToString());
            P02_import.ShowDialog();
        }
    }
}
