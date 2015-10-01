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
using Sci.Win.Tools;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;



namespace Sci.Production.Miscellaneous
{
    public partial class P03 : Sci.Win.Tems.Input6
    {
        private string factory = Sci.Env.User.Factory;
        private string loginID = Sci.Env.User.UserID;
        private string keyword = Sci.Env.User.Keyword;
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            string defaultfilter = string.Format("PurchaseFrom='T' and factoryid = '{0}'", factory);
            this.DefaultFilter = defaultfilter;
            this.InsertDetailGridOnDoubleClick = false;
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region grid1

            this.grid1.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.grid1)
            .Text("Currencyid", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 9, decimal_places: 4, iseditingreadonly: true);
            #endregion

        }
        protected override void OnDetailUIConvertToMaintain()
        {
            base.OnDetailUIConvertToMaintain();
            txtuser2.TextBox1.ReadOnly = true;
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label10.Text = CurrentMaintain["Status"].ToString();
            this.button2.Enabled = CurrentMaintain["Status"].ToString()=="Approved";
            string mastergridSql = string.Format(@"Select CurrencyId, isnull(Sum(Price * Qty),0) as Amount 
                from Miscpo_Detail a 
                join [Production].[dbo].Supp b on b.id=a.Suppid 
                where a.id = '{0}' group by currencyid", CurrentMaintain["ID"].ToString());
            DataTable masterDt;
            DualResult dtResult = DBProxy.Current.Select(null, mastergridSql, out masterDt);
            listControlBindingSource1.DataSource = masterDt;
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
        protected override void ClickUnconfirm()
        {

            base.ClickUnconfirm();
            if (!MyUtility.Check.Empty(CurrentMaintain["TransToTPE"]))
            {
                MyUtility.Msg.WarningBox("Already transferred to Taipei, you can't unapprove");
                return;
            }
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
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();    
            //先將Export_detail撈取出POID,SEQ,SEQ2資料再跟表頭Export join Group,之後才跟最外層的PartPO_Detail Left Join
            this.DetailSelectCommand = string.Format(
                @"Select 0 as sel,g.*,h.*,i.Description,i.MiscBrandid+'-'+j.Name as MiscBrand,
                g.Qty as ReqQty,g.Qty *g.Price as amount,k.Currencyid,i.inspect
                from MiscPO_Detail g
                left join 
                    (Select min(e.eta) as eta,min(e.WhseArrival) as WhseArrival,
                    d.poid,d.seq1,d.seq2,d.price as tpeprice,d.currencyid,d.tpeamount    
                    from 
                        (select a.id,a.poid,a.seq1,a.seq2,a.price,c.currencyid,a.price*b.qty as tpeamount 
                        from [Production].[dbo].export_Detail a
                        join [Production].[dbo].Supp c on c.id = a.suppid,
                        MiscPO_Detail b 
                        where a.poid = b.tpepoid and a.seq1 = b.seq1 and a.seq2 = b.seq2 and b.id='{0}') as d,
                [Production].[dbo].Export e
                Where d.id = e.id group by d.poid,d.seq1,d.seq2,d.price,d.currencyid,d.tpeamount) as h
                on h.poid = g.tpepoid and h.seq1 = g.seq1 and h.seq2 = g.seq2 
                left join Misc i on g.Miscid = i.id 
                left join [Production].[dbo].Supp k on k.id=g.Suppid
                left join MiscBrand j on j.id = i.MiscBrandid
                Where g.ID = '{0}' order by g.seq1,g.seq2", masterID);           
            return base.OnDetailSelectCommandPrepare(e);
        }
        //新增之後給初始值
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Factoryid"] = factory;
            CurrentMaintain["cdate"] = DateTime.Today;
            CurrentMaintain["PurchaseFrom"] = "T";
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Handle"] = loginID;
        }   
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (CurrentMaintain["Status"].ToString() == "Approved" || CurrentMaintain["Status"].ToString() == "Closed")
            {
                this.detailgrid.IsEditingReadOnly = true;
                this.dateBox1.ReadOnly = true;
                this.txtuser1.TextBox1.ReadOnly = true;
                this.txtuser2.TextBox1.ReadOnly = true;
                this.gridicon.Remove.Enabled = false;
                this.button1.Enabled = false;
            }
            this.gridicon.Append.Enabled = false;
            this.gridicon.Insert.Enabled = false;
            
        }
        protected override bool ClickSaveBefore()
        {
            //確認不可空白欄位
            if (string.IsNullOrWhiteSpace(CurrentMaintain["cDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("<Create Date> can not be empty!");
                this.dateBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("<Handle> can not be empty!");
                this.txtuser1.TextBox1.Focus();
                return false;
            }
            #region SEQ2
            if(CurrentMaintain["Status"].ToString()=="New")
            {
                string maxSeq2= "";
                //填入SEQ2
                foreach (DataRow  dr in this.DetailDatas)
                {
                    maxSeq2 =  MyUtility.GetValue.GetSeq(maxSeq2);
                    dr["SEQ2"] = maxSeq2;
                    if (maxSeq2 == "**")
                    {
                        string msg = string.Format("<SEQ1> : {0},<SEQ2> over 'Z9', please create new PO for '{1}'", dr["SEQ1"].ToString(), dr["PartReqID"].ToString());
                        MyUtility.Msg.WarningBox(msg, "PartReq# too much");
                        return false;
                    }            
                }
            }
            #endregion
            #region 填入ID
            if (this.IsDetailInserting)
            {
                string keyWord = keyword + "MT";
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
        //
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings cellamount = new DataGridViewGeneratorNumericColumnSettings();
            cellamount.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (e.FormattedValue == null) return;
                CurrentDetailData["Amount"] = (decimal)e.FormattedValue * (decimal)CurrentDetailData["Price"];
                CurrentDetailData["Qty"] = (decimal)e.FormattedValue;
                CurrentDetailData.EndEdit();
            };

            Helper.Controls.Grid.Generator(this.detailgrid)
                .CheckBox("Junk", header: "Cancel", iseditable: false)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Miscid", header: "Miscellaneous", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("MiscBrand", header: "Miscellaneous Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Currencyid", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Price", header: "Unit Price", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
                .Numeric("reqQty", header: "Req. Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2, settings: cellamount)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 13, decimal_places: 3, iseditingreadonly: true)
                .Numeric("InQty", header: "In Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
                .Numeric("InspQty", header: "Insp. Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
                .CheckBox("Inspect", header: "Need to Inspect", iseditable: true)
                .Text("MiscReqId", header: "Misc Requisition#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("eta", header: "ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("WhseArrival", header: "Act.Arr.Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("TPEPOID", header: "Taipei PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("tpeCurrency", header: "Currency(Taipei)", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("TpePrice", header: "Price(Taipei)", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
                .Numeric("tpeamount", header: "Amount(Taipei)", width: Widths.AnsiChars(10), integer_places: 13, decimal_places: 3, iseditingreadonly: true);

            this.detailgrid.Columns[8].DefaultCellStyle.BackColor = Color.Pink;

        }
        protected override bool ClickSavePost()
        {
            string updatePO="";
            DataTable delTb, sourTb = DetailDatas.CopyToDataTable();

            MyUtility.Tool.ProcessWithDatatable(sourTb, "MiscID,id", string.Format("select a.id,a.Miscid,b.id as id2 from Miscreq_Detail a left join #tmp b on a.Miscpoid = b.id and a.Miscid = b.miscid  where a.Miscpoid='{0}'", CurrentMaintain["ID"].ToString()), out delTb);
            foreach (DataRow deldr in delTb.Rows)
            {
                if (deldr["id2"].ToString() == "")
                {
                    updatePO = updatePO + string.Format("update MiscReq_Detail set Miscpoid = '' where id = '{0}' and Miscid = '{1}';", deldr["ID"].ToString(), deldr["Miscid"].ToString());
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
            {
                DataTable detTable = ((DataTable)this.detailgridbs.DataSource);
                Form P03_import = new Sci.Production.Miscellaneous.P03_Import(detTable, CurrentMaintain["LocalSuppid"].ToString());
                P03_import.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.grid.RowCount == 0)
            {
                MyUtility.Msg.WarningBox("There are no data");
                return;
            }
            DataTable dt;
            string sql = string.Format(@"Select a.Junk,a.SEQ2,a.miscreqid,a.departmentid,a.MiscID,
                                        b.Description,a.miscBrandid+'-'+c.Name as MiscBrand,
                                        a.Qty,a.Unitid,e.Currencyid,
                                        a.Price,a.Qty * a.price as Amount
                                        from MiscPo_Detail a 
                                        left join Misc b on b.id = a.Miscid
                                        left join MiscBrand c on c.id = a.Miscbrandid
                                        left join [Production].[dbo].[Supp] e on e.id = a.Suppid 
                                        left join MiscReq f on f.id = a.Miscreqid
                                        where a.id='{0}'
                                        order by seq2", CurrentMaintain["ID"].ToString());
            DualResult dResult = DBProxy.Current.Select(null, sql, out dt);
            if (dResult)
            {
                SaveFileDialog dlg;
                if (MyUtility.Excel.SaveXlsFile("MiscellaneousPurchaseOrder", out dlg)) //呼叫存檔的Excel路徑，傳入預設檔名與SaveFileDialog
                {
                    DualResult result = MyUtility.Excel.CopyToXls(dt, dlg.FileName); //傳入DataTable ,路徑Copy 出Excel
                    if (result)
                    {
                        MyUtility.Excel.XlsAutoFit(dlg.FileName, "Miscellaneous_P03.xlt",5, false);//將Excel 加入範本

                        Microsoft.Office.Interop.Excel.Application oleApp = MyUtility.Excel.ConnectExcel(dlg.FileName);
                        oleApp.Visible = false;
                        oleApp.DisplayAlerts = false;
                        oleApp.Workbooks.Open(dlg.FileName);
                        oleApp.Cells[2, 3].value = CurrentMaintain["ID"].ToString();
                        oleApp.Cells[2, 8].value = CurrentMaintain["cdate"].ToString();
                        oleApp.Cells[2, 12].value = factory;
                        oleApp.Cells[2, 2].value = MyUtility.GetValue.Lookup("Name", CurrentMaintain["Handle"].ToString(), "Pass1", "ID");
                        oleApp.Cells[3, 8].value = MyUtility.GetValue.Lookup("Name", CurrentMaintain["Approve"].ToString(), "Pass1", "ID");
                        oleApp.Cells[3, 12].value = CurrentMaintain["ApproveDate"].ToString();
                        oleApp.Cells[4, 3].value = CurrentMaintain["Remark"].ToString();
                        oleApp.Workbooks[1].Save();
                        oleApp.Workbooks.Close();
                        oleApp.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oleApp);
                        Marshal.FinalReleaseComObject(oleApp);
                        oleApp = null;
                        string fileNameExt = dlg.FileName.Substring(dlg.FileName.LastIndexOf("\\") + 1);
                        DataRow seekdr;
                        if (MyUtility.Check.Seek("select * from mailto where Id='001'", out seekdr))
                        {
                            string mailto = seekdr["ToAddress"].ToString();
                            string cc = seekdr["ccAddress"].ToString();
                            string content = seekdr["content"].ToString();
                            string subject = "<"+factory+">Miscellaneous REQ#:"+CurrentMaintain["ID"].ToString();

                            var email = new MailTo(Env.Cfg.MailFrom, mailto, cc, subject + "-" + fileNameExt, dlg.FileName,
content, false, false);
                            email.ShowDialog(this);
                        }
                    }
                    else { MyUtility.Msg.WarningBox(result.ToMessages().ToString(), "Warning"); }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Connect Fail");
            }
        }
    }
}
