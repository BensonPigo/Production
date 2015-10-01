using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Ict.Win;
using Ict;
using Ict.Data;
using Sci;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using System.Collections;
using Sci.Production.Class;

namespace Sci.Production.Miscellaneous
{
    public partial class P04 : Sci.Win.Tems.Input6
    {
        private string factory = Sci.Env.User.Factory;
        private string LoginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.DefaultFilter = string.Format("Factoryid = '{0}'", factory);
            InitializeComponent();
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            this.InsertDetailGridOnDoubleClick = false;
        }
        protected override void OnDetailGridSetup()
        {
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("TPEPOID", header: "Taipei PO No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("MiscPOID", header: "PO No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("MiscID", header: "Miscellaneous", width: Widths.AnsiChars(23), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Numeric("POQty", header: "PO Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Price", header: "Price", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Text("Unitid", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("OnRoad", header: "On Road", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2)
            .Date("Inspdeadline", header: "Inspect Lead Time", width: Widths.AnsiChars(10))
            .Text("MiscReqId", header: "Misc Requisition#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Departmentid", header: "Department", width: Widths.AnsiChars(13), iseditingreadonly: true);
            this.detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.Pink;
        }

        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
            @"SELECT a.*, b.description, b.unitid ,c.tpepoid
            FROM MiscIn_Detail a
            Left Join Misc b on a.miscid = b.id 
            Left join MiscPO_Detail c on c.id = a.miscpoid and c.seq1 = a.seq1 and c.seq2 = a.seq2
            WHERE a.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label6.Text = CurrentMaintain["Status"].ToString();
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            #region New default value
            CurrentMaintain["cDate"] = DateTime.Now;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["factoryid"] = factory;
            #endregion

        }
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record already Confirmed, you can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record already Confirmed, you can't modify", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["cDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("<Create Date> can not be empty!", "Warning");
                this.dateBox1.Focus();
                return false;
            }
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    this.CurrentDetailData.Delete();
                }
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("<Detail> can not be Empty!");
                return false;
            }

            #region 填入ID
            if (this.IsDetailInserting)
            {
                string keyWord = factory+"MC";
                string id = MyUtility.GetValue.GetID(keyWord, "MiscIn", (DateTime)CurrentMaintain["cDate"]);

                if (string.IsNullOrWhiteSpace(id))
                {
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            #endregion
            return base.ClickSaveBefore();
        }



        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            List<string> stockSqlinLi = new List<string>();
            List<string> stockSqlupLi = new List<string>();
            List<string> stockSqlQuLi = new List<string>();
            string sql, msg, msg2, insertSql = "", updateIn = "";
 
            bool bolmsg = false;
            bool bolmsg2 = false;
            msg = "<In qty> + <Acc.In Qty> can not exceed po qty! Please see below\nPOID,SEQ1,SEQ2\n";
            msg2 = "Purchase Order <Status> is not Approved! Please see below\nPOID,SEQ1,SEQ2\n";
            DataTable checkTable;

            foreach (DataRow dr in this.DetailDatas)
            {
                insertSql = insertSql + string.Format("Update MiscPO_Detail set InQty = InQty+{0} where id='{1}' and seq1 = '{2}' and seq2 = '{3}';", dr["Qty"], dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());


                sql = string.Format("Select Qty - InQty as OnRoad,InQty,status from MiscPO b ,MiscPO_Detail a where a.ID = '{0}' and SEQ1 = '{1}' and seq2 = '{2}' and a.id = b.id", dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                DualResult result = DBProxy.Current.Select(null, sql, out checkTable);
                if (result)
                {
                    DataRow drCheckTable = checkTable.Rows[0];
                    if ((decimal)dr["POQty"] < (decimal)drCheckTable["InQty"] + (decimal)dr["QTY"])
                    {
                        msg = msg + string.Format("{0},{1},{2}\n", dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                        bolmsg = true;
                    }
                    if (drCheckTable["Status"].ToString() != "Approved")
                    {
                        msg2= msg2 + string.Format("{0},{1},{2}\n", dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                        bolmsg2 = true;
                    }
                    updateIn = updateIn + string.Format("Update Miscin_Detail set OnRoad = {0} where  id='{1}' and seq1 = '{2}' and seq2 = '{3}';", drCheckTable["OnRoad"], dr["id"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                }
            }
            string updSql = string.Format("update MiscIn set Status = 'Confirmed' ,editname='{0}', editdate = GETDATE() where id='{1}';", LoginID, CurrentMaintain["ID"].ToString());
            #region update Inqty,Status
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updateIn)))
                    {
                        ShowErr(updateIn, upResult);
                        return;
                    }
                    if (!bolmsg && !bolmsg2)
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, insertSql)))
                        {
                            ShowErr(insertSql, upResult);
                            return;
                        }
                        if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                        {
                            ShowErr(updSql, upResult);
                            return;
                        }
                     }
                    _transactionscope.Complete();
                    if (!bolmsg && !bolmsg2)
                    {
                        MyUtility.Msg.WarningBox("Successfully");
                    }
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            #endregion

            if (bolmsg)
            {
                MyUtility.Msg.WarningBox(msg, "Warning");
            }
            if (bolmsg2)
            {
                MyUtility.Msg.WarningBox(msg2, "Warning");
            }

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            

            List<string> stockSqlupLi = new List<string>();
           
            string sql, msg,msg2, insertSql = "";
            msg = "AP Qty and Inspection Qty can not exceed Acc.In qty.Please see below\nPOID,SEQ1,SEQ2\n";
            msg2 = "Purchase Order already closed, you can't unconfirm. Please see below\nPOID,SEQ1,SEQ2\n";
            DataTable checkTable;
            bool bolmsg = false;
            bool bolmsg2 = false;
            foreach (DataRow dr in this.DetailDatas)
            {

                //update PartPo_Detail
                insertSql = insertSql + string.Format("Update MiscPO_Detail set InQty = InQty-{0} where id='{1}' and seq1 = '{2}' and seq2 = '{3}';", dr["Qty"], dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());

                
                sql = string.Format("Select InQty,ApQty,InspQty,Status,Inspect from Miscpo a ,MiscPO_Detail b left join Misc c on c.id = b.miscid where a.id = '{0}' and a.id = b.id and SEQ1 = '{1}' and seq2 = '{2}'", dr["Miscpoid"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                DualResult result = DBProxy.Current.Select(null, sql, out checkTable);
                if (result)
                {
                    DataRow drCheckTable = checkTable.Rows[0]; //APQty 不可多於InQty
                    if (drCheckTable["Status"].ToString() == "Closed")//只要明細有一筆的PO為Close 則不可Amend
                    {
                        msg2 = msg2 + string.Format("{0},{1},{2}\n", dr["Miscpoid"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                        bolmsg2 = true;
                    }
                    if ((drCheckTable["Inspect"].ToString() == "False" && (decimal)drCheckTable["ApQty"] > (decimal)drCheckTable["InQty"] - (decimal)dr["Qty"])) //若不需要檢驗則判斷InQty不可小於ApQty
                    {
                        msg = msg + string.Format("{0},{1},{2}\n", dr["Miscpoid"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                        bolmsg = true;
                    }
                    if ((drCheckTable["Inspect"].ToString() == "True" && (decimal)drCheckTable["InspQty"] > (decimal)drCheckTable["InQty"] - (decimal)dr["Qty"]))//若需要檢驗則判斷InQty不可小於InspQty
                    {
                        msg = msg + string.Format("{0},{1},{2}\n", dr["Miscpoid"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                        bolmsg = true;
                    }
                }
            }
            if (bolmsg)
            {
                MyUtility.Msg.WarningBox(msg, "Warning");
                return;
            }
            if (bolmsg2)
            {
                MyUtility.Msg.WarningBox(msg2, "Warning");
                return;
            }
            //update MiscIn
            string updSql = string.Format("update MiscIn set Status = 'New' ,editname='{0}', editdate = GETDATE() where id='{1}';", LoginID, CurrentMaintain["ID"].ToString());

            #region update Inqty,Status
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, insertSql)))
                    {
                        ShowErr(insertSql, upResult);
                        return;
                    }

                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        ShowErr(updSql, upResult);
                        return;
                    }
                    
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            #endregion

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable detTable = ((DataTable)this.detailgridbs.DataSource);
            Form P04_Import = new Sci.Production.Miscellaneous.P04_Import(detTable,CurrentMaintain["cDate"]);
            P04_Import.ShowDialog();
        }

        //不判斷是否存在Export 但只要有就要帶出值於Grid
        private void textBox3_Validated(object sender, EventArgs e)
        {
            base.OnValidated(e);
            if (textBox3.OldValue == textBox3.Text) return;
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }
            string sql = string.Format(
            @"Select d.ID,d.miscid,d.Qty-d.inQty,d.Qty as poqty,d.Qty-d.inQty as onroad,e.inspleadtime,tpepoid,
            a.SEQ1,a.SEQ2,a.unitid,e.description,d.price,d.DepartmentID,d.miscreqid,e.inspect
            from Export_Detail a,
                (Select b.ID,tpepoid,seq1,seq2,Miscid,InQty,qty,price,c.DepartmentID,miscreqid
                from Miscpo b, Miscpo_Detail c 
                where b.id = c.id and b.status='Approved' and b.factoryid='{1}' and qty-inqty>0) as d 
            Left join Misc e on e.id = d.Miscid  
            Where a.ID = '{0}' and POType = 'M' and fabrictype='O' and d.tpepoid=a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2 ", this.CurrentMaintain["ExportID"].ToString(), factory);
            DataTable dt;
            DualResult duresult = DBProxy.Current.Select(null, sql, out dt);
            if (duresult)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataTable detTable = ((DataTable)this.detailgridbs.DataSource);
                    DataRow nDr = detTable.NewRow();
                    nDr["MiscPoid"] = dr["ID"];
                    nDr["SEQ1"] = dr["SEQ1"];
                    nDr["SEQ2"] = dr["SEQ2"];
                    nDr["Qty"] = dr["Qty"];
                    nDr["OnRoad"] = dr["OnRoad"];
                    nDr["MiscID"] = dr["MiscID"];
                    nDr["description"] = dr["description"];
                    nDr["POQty"] = dr["POQty"];
                    nDr["Price"] = dr["Price"];
                    nDr["miscreqid"] = dr["miscreqid"];
                    nDr["Unitid"] = dr["Unitid"];
                    nDr["departmentid"] = dr["departmentid"];
                    nDr["TPEPOID"] = dr["TPEPOID"];
                    if (dr["Inspect"].ToString() == "True")
                    {
                        if (MyUtility.Check.Empty(dr["InspLeadTime"]))
                        {
                            double nDl = Convert.ToDouble(MyUtility.GetValue.Lookup("select MiscInspdate from System"));
                            nDr["InspDeadline"] = ((DateTime)CurrentMaintain["cDate"]).AddDays(nDl);
                        }
                        else //若Leadtime 非空就加上為空就用System
                        {
                            nDr["InspDeadline"] = ((DateTime)CurrentMaintain["cDate"]).AddDays(((double)((decimal)dr["InspLeadTime"])));
                        }
 
                    }
                    detTable.Rows.Add(nDr);
                }
            }
        }
    }
}
