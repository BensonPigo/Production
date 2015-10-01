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
    public partial class P05 : Sci.Win.Tems.Input6
    {
        private string factory = Sci.Env.User.Factory;
        private string LoginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        public P05(ToolStripMenuItem menuitem)
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
            .Text("MiscPOID", header: "PO No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("MiscID", header: "Miscellaneous", width: Widths.AnsiChars(23), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Numeric("InQty", header: "In Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("PassQty", header: "Pass Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(5))
            .Date("Inspdeadline", header: "Inspect Lead Time", width: Widths.AnsiChars(10), iseditingreadonly: true);
            this.detailgrid.Columns[6].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            string incomingid = (e.Master == null) ? "" : e.Master["MiscInid"].ToString();
            this.DetailSelectCommand = string.Format(
            @"SELECT a.*, b.description,d.inqty,cast(null as Datetime) as Inspdeadline
            FROM MiscInsp_Detail a
            Left Join Misc b on a.miscid = b.id
            Left join Miscpo_Detail d on d.id = a.miscpoid and d.seq1 = a.seq1 and d.seq2 = a.seq2
            Left join Miscin_Detail c on c.id = '{1}' and c.miscpoid = a.miscpoid and c.seq1 = a.seq1 and c.seq2 = a.seq2
            WHERE a.ID = '{0}'", masterID,incomingid);
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
            CurrentMaintain["Handle"] = LoginID;
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
            if (!MyUtility.Check.Seek(string.Format("Select * from MiscIn Where id='{0}' and status ='Confirmed'", CurrentMaintain["MiscinID"].ToString())))
            {
                MyUtility.Msg.WarningBox("<In-Coming#> status not yet Confirmed");
                return false;
            }
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["PassQty"]))
                {
                    this.CurrentDetailData.Delete();
                    continue;
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
                string keyWord = factory+"MN";
                string id = MyUtility.GetValue.GetID(keyWord, "MiscInsp", (DateTime)CurrentMaintain["cDate"]);

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
            //判斷MiscIn Staus 為Confirm
            if (!MyUtility.Check.Seek(string.Format("Select * from MiscIn Where id='{0}' and status ='Confirmed'", CurrentMaintain["MiscinID"].ToString())))
            {
                MyUtility.Msg.WarningBox("<In-Coming#> Status wasn't Confirmed");
                return;
            }
            string sql, msg, insertSql = "";
 
            bool bolmsg = false;
            msg = "<Pass qty> + <Acc.Pass Qty> can not exceed In qty! Please see below\nPOID,SEQ1,SEQ2\n";
            DataTable checkTable;

            foreach (DataRow dr in this.DetailDatas)
            {
                //回寫Miscpo InSpQty
                insertSql = insertSql + string.Format("Update MiscPO_Detail set InspQty = InspQty+{0} where id='{1}' and seq1 = '{2}' and seq2 = '{3}';", dr["PassQty"], dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());

                //判斷PassQty 是否超過InQty
                sql = string.Format("Select InQty,PassQty from MiscPO_Detail a where a.ID = '{0}' and SEQ1 = '{1}' and seq2 = '{2}' ", dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                DualResult result = DBProxy.Current.Select(null, sql, out checkTable);
                if (result)
                {
                    DataRow drCheckTable = checkTable.Rows[0];
                    if ((decimal)dr["InQty"] < (decimal)drCheckTable["PassQty"] + (decimal)dr["QTY"])
                    {
                        msg = msg + string.Format("{0},{1},{2}\n", dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                        bolmsg = true;
                    }
                 }
            }
            //變更表頭Confirm
            string updSql = string.Format("update MiscInsp set Status = 'Confirmed' ,editname='{0}', editdate = GETDATE() where id='{1}';", LoginID, CurrentMaintain["ID"].ToString());
            #region update Inqty,Status
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!bolmsg)
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
                    if (!bolmsg)
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

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

           
            string sql, msg,insertSql = "";
            msg = "AP Qty can not exceed Acc.Insp qty.Please see below\nPOID,SEQ1,SEQ2\n";
            DataTable checkTable;
            bool bolmsg = false;
            foreach (DataRow dr in this.DetailDatas)
            {
                //update PartPo_Detail
                insertSql = insertSql + string.Format("Update MiscPO_Detail set InspQty = InspQty-{0} where id='{1}' and seq1 = '{2}' and seq2 = '{3}';", dr["PassQty"], dr["MiscPOID"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());

                
                sql = string.Format("Select ApQty,InspQty,Inspect from MiscPO_Detail b left join Misc c on c.id = b.miscid where b.id = '{0}' and SEQ1 = '{1}' and seq2 = '{2}'", dr["Miscpoid"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString());
                DualResult result = DBProxy.Current.Select(null, sql, out checkTable);
                if (result)
                {
                    DataRow drCheckTable = checkTable.Rows[0]; //APQty 不可多於InQty
                    if ((drCheckTable["Inspect"].ToString() == "True" && (decimal)drCheckTable["ApQty"] > (decimal)drCheckTable["InspQty"] - (decimal)dr["PassQty"]))
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
            //update MiscIn
            string updSql = string.Format("update MiscInsp set Status = 'New' ,editname='{0}', editdate = GETDATE() where id='{1}';", LoginID, CurrentMaintain["ID"].ToString());

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
            Form P05_Import = new Sci.Production.Miscellaneous.P05_Import(detTable,CurrentMaintain["Miscinid"].ToString());
            P05_Import.ShowDialog();
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
            @"select a.*, b.description 
            from MiscIn_detail a,Misc b,miscpo_Detail c,MiscIn d 
            where a.id= '{0}' and  a.miscid = b.id and b.Inspect=1 and c.miscid = a.miscid 
            and a.MiscPOID = c.id and a.seq1 = c.seq1 and a.seq2 = c.seq2 and c.inqty-c.inspqty>0 and d.id = a.id and d.factoryid = '{1}'", textBox3.Text,factory);
            DataTable dt;
            DualResult duresult = DBProxy.Current.Select(null, sql, out dt);
            if (duresult)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataTable detTable = ((DataTable)this.detailgridbs.DataSource);
                    DataRow nDr = detTable.NewRow();
                    nDr["MiscPoid"] = dr["MiscpoID"];
                    nDr["SEQ1"] = dr["SEQ1"];
                    nDr["SEQ2"] = dr["SEQ2"];
                    nDr["InQty"] = dr["Qty"];
                    nDr["MiscID"] = dr["MiscID"];
                    nDr["description"] = dr["description"];
                    nDr["PassQty"] = dr["Qty"];
                    nDr["InspDeadLine"] = dr["InspDeadLine"];
                    detTable.Rows.Add(nDr);
                }
            }
        }
    }
}
