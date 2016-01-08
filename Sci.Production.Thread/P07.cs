using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    public partial class P07 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.DefaultFilter = string.Format("mDivisionid = '{0}'", keyWord);
            InitializeComponent();
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.*,b.description,c.description as colordesc,
            b.category,d.newCone as newconebook,d.usedcone as usedconebook
            from ThreadTransfer_Detail a 
            left join Localitem b on a.refno = b.refno 
            left join ThreadStock d on d.refno = a.refno and d.threadColorid = a.threadColorid and d.threadLocationid = 
                a.Locationfrom and d.mDivisionid = '{1}'
            left join threadcolor c on a.threadcolorid = c.id where a.id = '{0}'", masterID,keyWord);
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override bool OnGridSetup()
        {

            DataGridViewGeneratorTextColumnSettings refno = celllocalitem.GetGridCell("Thread");
            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings thlocationfrom = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings thlocationto = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();

            #region Refno Cell
            refno.CellValidating += (s, e) =>
            {

                if (!this.EditMode || e.RowIndex == -1) return;
                string oldvalue = CurrentDetailData["refno"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from Localitem where refno = '{0}' and junk = 0", newvalue), out refdr))
                {
                    CurrentDetailData["Description"] = refdr["Description"];
                    CurrentDetailData["Refno"] = refdr["refno"];
                    CurrentDetailData["category"] = refdr["category"];
                    CurrentDetailData["NewCone"] = 0;
                    CurrentDetailData["UsedCone"] = 0;

                }
                else
                {
                    CurrentDetailData["Description"] = "";
                    CurrentDetailData["Refno"] = "";
                    CurrentDetailData["category"] = "";
                    CurrentDetailData["NewCone"] = 0;
                    CurrentDetailData["UsedCone"] = 0;
                }
                string sql = string.Format("Select newcone, usedcone from ThreadStock where refno ='{1}' and threadcolorid = '{0}' and threadlocationid = '{2}' and mDivisionid = '{3}'", CurrentDetailData["threadColorid"], newvalue, CurrentDetailData["locationfrom"],keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    CurrentDetailData["newconebook"] = refdr["newcone"];
                    CurrentDetailData["usedconebook"] = refdr["usedcone"];

                }
                else
                {
                    CurrentDetailData["newconebook"] = 0;
                    CurrentDetailData["usedconebook"] = 0;

                }
                CurrentDetailData.EndEdit();
            };
            #endregion
            #region Color Cell
            thcolor.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1) return;
                string oldvalue = CurrentDetailData["threadcolorid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                
                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadColor where id = '{0}' and junk = 0", newvalue), out refdr))
                {
                    CurrentDetailData["ThreadColorid"] = refdr["ID"];
                    CurrentDetailData["Colordesc"] = refdr["Description"];
                }
                else
                {
                    CurrentDetailData["ThreadColorid"] = "";
                    CurrentDetailData["Colordesc"] = "";

                }
                string sql = string.Format("Select newcone,usedcone from ThreadStock where refno ='{0}' and threadcolorid = '{1}' and threadlocationid = '{2}' and mDivisionid = '{3}'", CurrentDetailData["Refno"], newvalue, CurrentDetailData["locationfrom"],keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    CurrentDetailData["newconebook"] = refdr["newcone"];
                    CurrentDetailData["usedconebook"] = refdr["usedcone"];

                }
                else
                {
                    CurrentDetailData["newconebook"] = 0;
                    CurrentDetailData["usedconebook"] = 0;

                }
                CurrentDetailData.EndEdit();
            };
            #endregion
            #region location
            thlocationfrom.EditingMouseDown += (s, e) =>
                {
                    if (!EditMode ||e.RowIndex == -1) return;
                    if (e.Button != System.Windows.Forms.MouseButtons.Right) return;
                    
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select ID,Description from ThreadLocation where mDivisionid = '{0}' and junk = 0 order by ID", keyWord), "10,40",CurrentDetailData["locationfrom"].ToString());
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = item.GetSelectedString();
                };
            thlocationfrom.CellValidating += (s, e) =>
                {
                    if (!this.EditMode ||e.RowIndex==-1) return;
                    string oldvalue = CurrentDetailData["locationfrom"].ToString();
                    string newvalue = e.FormattedValue.ToString();
                    if (oldvalue == newvalue) return;
                    DataRow refdr;

                    string sql = string.Format("Select newcone,usedcone from ThreadStock where refno ='{0}' and threadcolorid = '{1}' and threadlocationid = '{2}' and mDivisionid = '{3}'", CurrentDetailData["Refno"], CurrentDetailData["threadcolorid"], newvalue,keyWord);
                    if (MyUtility.Check.Seek(sql, out refdr))
                    {
                        CurrentDetailData["newconebook"] = refdr["newcone"];
                        CurrentDetailData["usedconebook"] = refdr["usedcone"];
       
                    }
                    else
                    {
                        CurrentDetailData["newconebook"] = 0;
                        CurrentDetailData["usedconebook"] = 0;
            
                    }
                    if (MyUtility.Check.Seek(newvalue, "ThreadLocation", "ID")) CurrentDetailData["locationfrom"] = newvalue;
                    else
                    {
                        MyUtility.Msg.WarningBox("<Location> not found");
                        CurrentDetailData["Locationfrom"] = "";
                    }
                    CurrentDetailData.EndEdit();
                };
            #endregion
            #region location
            thlocationto.EditingMouseDown += (s, e) =>
            {
                if (!EditMode || e.RowIndex == -1) return;
                if (e.Button != System.Windows.Forms.MouseButtons.Right) return;
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select ID,Description from ThreadLocation where mDivisionid = '{0}' and junk = 0 order by ID", keyWord), "10,40", CurrentDetailData["locationto"].ToString());
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }
                e.EditingControl.Text = item.GetSelectedString();
            };
            thlocationto.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1) return;
                string oldvalue = CurrentDetailData["locationto"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                if (MyUtility.Check.Seek(newvalue, "ThreadLocation", "ID")) CurrentDetailData["locationto"] = newvalue;
                else
                {
                    MyUtility.Msg.WarningBox("<Location> not found");
                    CurrentDetailData["Locationto"] = "";
                }
                CurrentDetailData.EndEdit();
            };
            #endregion      
            #region setup Grid

            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20), settings: refno)
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(15), settings: thcolor)
            .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Locationfrom", header: "Location (From)", width: Widths.AnsiChars(10), settings: thlocationfrom)
            .Text("Locationto", header: "Location (To)", width: Widths.AnsiChars(10), settings: thlocationto)
            .Numeric("NewConebook", header: "New Cone\nin Stock", width: Widths.AnsiChars(5), integer_places: 3,iseditingreadonly:true)
            .Numeric("NewCone", header: "New Cone", width: Widths.AnsiChars(5), integer_places: 5, settings: qty)
             .Numeric("UsedConebook", header: "Used cone\nin Stock", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: 
            true)
            .Numeric("UsedCone", header: "Used Cone", width: Widths.AnsiChars(5), integer_places: 5, settings: qty)
            .Text("category", header: "Category", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("remark", header: "Remark", width: Widths.AnsiChars(60));
            #endregion

            this.detailgrid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[2].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[4].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[9].DefaultCellStyle.BackColor = Color.Pink;

            return base.OnGridSetup();
        }
        
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label7.Text = CurrentMaintain["Status"].ToString();
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["cDate"] = DateTime.Now;
            CurrentMaintain["AddName"] = loginID;
            CurrentMaintain["AddDate"] = DateTime.Now;
            CurrentMaintain["mDivisionid"] = keyWord;
            
        }
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record already confrimed, you can't delete", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record already confrimed, you can't modify", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["cDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("<Date> can not be empty!", "Warning");
                this.dateBox1.Focus();
                return false;
            }
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Refno"]) )
                {
                    MyUtility.Msg.WarningBox("<Refno> can not be empty!", "Warning");
                    return false;
                }
                if (MyUtility.Check.Empty(dr["ThreadColorid"]))
                {
                    MyUtility.Msg.WarningBox("<Thread Color> can not be empty!", "Warning");
                    return false;
                }
                if (MyUtility.Check.Empty(dr["Locationfrom"]) )
                {
                    MyUtility.Msg.WarningBox("<Location(From)> can not be empty!", "Warning");
                    return false;
                }
                if (MyUtility.Check.Empty(dr["Locationto"]))
                {
                    MyUtility.Msg.WarningBox("<Location(to)> can not be empty!", "Warning");
                    return false;
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
                string id = MyUtility.GetValue.GetID(keyWord + "TT", "ThreadTransfer", (DateTime)CurrentMaintain["cDate"]);

                if (string.IsNullOrWhiteSpace(id))
                {
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            #endregion

            return base.ClickSaveBefore();
        }
        protected override void OnDetailUIConvertToUpdate()
        {
            base.OnDetailUIConvertToUpdate();
            dateBox1.ReadOnly = true;
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            List<string> stockSqlinLi = new List<string>();
            List<string> stockSqlupLi = new List<string>();
            List<string> stockSqlQuLi = new List<string>();
            string checksql;
            string sql, insertSql = "";
            DataRow stdr;
            string msg1 = "New cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n", msg2 = "Used cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n";
 
            bool lmsg1 = false;
            bool lmsg2 = false;
            sql = String.Format("Update ThreadTransfer set Status = 'Confirmed',editname='{0}', editdate = GETDATE() where id='{1}'", loginID, CurrentMaintain["ID"].ToString());

            foreach (DataRow dr in this.DetailDatas)
            {
                checksql = string.Format("Select * from ThreadStock where refno ='{0}' and threadLocationid = '{1}' and threadcolorid = '{2}' and mDivisionid = '{3}'", dr["refno"], dr["locationfrom"], dr["threadcolorid"], keyWord);
                if (MyUtility.Check.Seek(checksql, out stdr))//找不到庫存或不夠不可confirm
                {
                    if ((decimal)stdr["Newcone"] < (decimal)dr["NewCone"])
                    {
                        msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationfrom"]);
                        lmsg1 = true;
                    }
                    if ((decimal)stdr["UsedCone"] < (decimal)dr["UsedCone"])
                    {
                        msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationfrom"]);
                        lmsg2 = true;
                    }
                    insertSql = insertSql + string.Format("Update ThreadStock set NewCone = NewCone-({0}),UsedCone = UsedCone - ({1}) where refno='{2}' and mDivisionid = '{3}' and ThreadColorid = '{4}' and ThreadLocationid = '{5}';", dr["NewCone"], (decimal)dr["UsedCone"], dr["refno"], keyWord, dr["ThreadColorid"], dr["LocationFrom"]); ///扣庫存

                    checksql = string.Format("Select * from ThreadStock where refno ='{0}' and threadLocationid = '{1}' and threadcolorid = '{2}' and mDivisionid = '{3}'", dr["refno"], dr["locationTo"], dr["threadcolorid"], keyWord);
                    if (MyUtility.Check.Seek(checksql, out stdr)) //找不到庫存就新增
                    {
                        insertSql = insertSql + string.Format("Update ThreadStock set NewCone = NewCone+({0}),UsedCone = UsedCone + ({1}) where refno='{2}' and mDivisionid = '{3}' and ThreadColorid = '{4}' and ThreadLocationid = '{5}';", dr["NewCone"], dr["UsedCone"], dr["refno"], keyWord, dr["ThreadColorid"], dr["LocationTo"]);
                    }
                    else
                    {
                        insertSql = insertSql + string.Format("insert ThreadStock(refno,mDivisionid,threadcolorid,threadlocationid,newcone,usedcone,addName,AddDate) values('{0}','{1}','{2}','{3}',{4},{5},'{6}',GetDate())", dr["refno"], keyWord, dr["ThreadColorid"], dr["LocationTo"],dr["NewCone"], dr["UsedCone"], loginID);                 
                    }
                }
                else
                {
                    if ((decimal)stdr["Newcone"] < (decimal)dr["NewCone"])
                    {
                        msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationfrom"]);
                        lmsg1 = true;
                    }
                    if ((decimal)stdr["UsedCone"] < (decimal)dr["UsedCone"])
                    {
                        msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationfrom"]);
                        lmsg2 = true;
                    }


                }

            }
            if (lmsg1)
            {
                MyUtility.Msg.WarningBox(msg1);
                return;
            }
            if (lmsg2)
            {
                MyUtility.Msg.WarningBox(msg2);
                return;
            }
            #region update Inqty,Status
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, insertSql)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(insertSql, upResult);
                        return;
                    }
                    if (!(upResult = DBProxy.Current.Execute(null, sql)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sql, upResult);
                        return;
                    }
                    _transactionscope.Complete();
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
            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            List<string> stockSqlinLi = new List<string>();
            List<string> stockSqlupLi = new List<string>();
            List<string> stockSqlQuLi = new List<string>();
            string checksql;
            string sql, insertSql = "";
            DataRow stdr;
            string msg1 = "New cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n", msg2 = "Used cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n";

            bool lmsg1 = false;
            bool lmsg2 = false;
            sql = String.Format("Update ThreadTransfer set Status = 'New',editname='{0}', editdate = GETDATE() where id='{1}'", loginID, CurrentMaintain["ID"].ToString());

            foreach (DataRow dr in this.DetailDatas)
            {
                checksql = string.Format("Select * from ThreadStock where refno ='{0}' and threadLocationid = '{1}' and threadcolorid = '{2}' and mDivisionid = '{3}'", dr["refno"], dr["locationto"], dr["threadcolorid"], keyWord);
                if (MyUtility.Check.Seek(checksql, out stdr))//找不到庫存或不夠不可confirm
                {
                    if ((decimal)stdr["Newcone"] < (decimal)dr["NewCone"])
                    {
                        msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationto"]);
                        lmsg1 = true;
                    }
                    if ((decimal)stdr["UsedCone"] < (decimal)dr["UsedCone"])
                    {
                        msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationto"]);
                        lmsg2 = true;
                    }
                    insertSql = insertSql + string.Format("Update ThreadStock set NewCone = NewCone-({0}),UsedCone = UsedCone - ({1}) where refno='{2}' and mDivisionid = '{3}' and ThreadColorid = '{4}' and ThreadLocationid = '{5}';", dr["NewCone"], (decimal)dr["UsedCone"], dr["refno"], keyWord, dr["ThreadColorid"], dr["locationto"]); ///扣庫存

                    checksql = string.Format("Select * from ThreadStock where refno ='{0}' and threadLocationid = '{1}' and threadcolorid = '{2}' and mDivisionid = '{3}'", dr["refno"], dr["locationFrom"], dr["threadcolorid"], keyWord);
                    if (MyUtility.Check.Seek(checksql, out stdr)) //找不到庫存就新增
                    {
                        insertSql = insertSql + string.Format("Update ThreadStock set NewCone = NewCone+({0}),UsedCone = UsedCone + ({1}) where refno='{2}' and mDivisionid = '{3}' and ThreadColorid = '{4}' and ThreadLocationid = '{5}';", dr["NewCone"], dr["UsedCone"], dr["refno"], keyWord, dr["ThreadColorid"], dr["locationfrom"]);
                    }
                    else
                    {
                        insertSql = insertSql + string.Format("insert ThreadStock(refno,mDivisionid,threadcolorid,threadlocationid,newcone,usedcone,addName,AddDate) values('{0}','{1}','{2}','{3}',{4},{5},'{6}',GetDate())", dr["refno"], keyWord, dr["ThreadColorid"], dr["locationfrom"], dr["NewCone"], dr["UsedCone"], loginID);
                    }
                }
                else
                {
                    if ((decimal)stdr["Newcone"] < (decimal)dr["NewCone"])
                    {
                        msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                        lmsg1 = true;
                    }
                    if ((decimal)stdr["UsedCone"] < (decimal)dr["UsedCone"])
                    {
                        msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                        lmsg2 = true;
                    }


                }

            }
            if (lmsg1)
            {
                MyUtility.Msg.WarningBox(msg1);
                return;
            }
            if (lmsg2)
            {
                MyUtility.Msg.WarningBox(msg2);
                return;
            }
            #region update Inqty,Status
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, insertSql)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(insertSql, upResult);
                        return;
                    }
                    if (!(upResult = DBProxy.Current.Execute(null, sql)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sql, upResult);
                        return;
                    }
                    _transactionscope.Complete();
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
            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DataTable detTable = ((DataTable)this.detailgridbs.DataSource);
            Form P07_import = new Sci.Production.Thread.P07_Import(detTable);
            P07_import.ShowDialog();
        }
    }
}
