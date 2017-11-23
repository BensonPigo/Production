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
    /// <summary>
    /// P04
    /// </summary>
    public partial class P04 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;

        /// <summary>
        /// P04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string defaultfilter = string.Format("MDivisionid = '{0}' ", this.keyWord);
            this.DefaultFilter = defaultfilter;
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select a.*,b.description,c.description as colordesc,
            isnull(d.newcone,0) as stocknew, isnull(d.usedcone,0) as stockused
            from ThreadIssue_Detail a WITH (NOLOCK) 
            left join Localitem b WITH (NOLOCK) on a.refno = b.refno 
            left join threadcolor c WITH (NOLOCK) on a.threadcolorid = c.id 
            left join threadStock d WITH (NOLOCK) 
            on d.refno = a.refno and d.threadcolorid = a.threadcolorid and 
            d.threadlocationid = a.threadlocationid and d.mDivisionid = '{1}'
            where a.id = '{0}'",
                masterID,
                this.keyWord);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings refno = celllocalitem.GetGridCell("Thread", null, ",,,Description");
            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings threadlocaion = new DataGridViewGeneratorTextColumnSettings();

            #region Refno Cell
            refno.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                string oldvalue = this.CurrentDetailData["refno"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from Localitem WITH (NOLOCK) where refno = '{0}' and junk = 0", newvalue), out refdr))
                {
                    this.CurrentDetailData["Description"] = refdr["Description"];
                    this.CurrentDetailData["Refno"] = refdr["refno"];
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                }
                else
                {
                    this.CurrentDetailData["Description"] = string.Empty;
                    this.CurrentDetailData["Refno"] = string.Empty;
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                }

                string sql = string.Format("Select ThreadLocationid,newcone,usedcone from ThreadStock WITH (NOLOCK) where refno ='{1}' and threadcolorid = '{0}' and threadlocationid = '{2}' and mDivisionid ='{3}' ", this.CurrentDetailData["threadColorid"], newvalue, this.CurrentDetailData["threadLocationid"], this.keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["ThreadLocationid"] = refdr["ThreadLocationid"];
                    this.CurrentDetailData["stocknew"] = refdr["NewCone"];
                    this.CurrentDetailData["stockused"] = refdr["UsedCone"];
                }
                else
                {
                    this.CurrentDetailData["ThreadLocationid"] = string.Empty;
                    this.CurrentDetailData["stocknew"] = 0;
                    this.CurrentDetailData["stockused"] = 0;
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region Color Cell
            thcolor.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                string oldvalue = this.CurrentDetailData["threadcolorid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadColor WITH (NOLOCK) where id = '{0}' and junk = 0", newvalue), out refdr))
                {
                    this.CurrentDetailData["ThreadColorid"] = refdr["ID"];
                    this.CurrentDetailData["Colordesc"] = refdr["Description"];
                }
                else
                {
                    this.CurrentDetailData["ThreadColorid"] = string.Empty;
                    this.CurrentDetailData["Colordesc"] = string.Empty;
                }

                string sql = string.Format("Select ThreadLocationid,newcone,usedcone from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and threadlocationid = '{2}' and mDivisionid = '{3}'", this.CurrentDetailData["refno"], newvalue, this.CurrentDetailData["threadLocationid"], this.keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["ThreadLocationid"] = refdr["ThreadLocationid"];
                    this.CurrentDetailData["stocknew"] = refdr["NewCone"];
                    this.CurrentDetailData["stockused"] = refdr["UsedCone"];
                }
                else
                {
                    this.CurrentDetailData["ThreadLocationid"] = string.Empty;
                    this.CurrentDetailData["stocknew"] = 0;
                    this.CurrentDetailData["stockused"] = 0;
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region threadlocaion

            threadlocaion.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                string oldvalue = this.CurrentDetailData["threadlocationid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadLocation WITH (NOLOCK) where id = '{0}' and junk = 0", newvalue)))
                {
                    string sql = string.Format("Select ThreadLocationid,newcone,usedcone from ThreadStock WITH (NOLOCK) where refno ='{1}' and threadcolorid = '{0}' and threadlocationid = '{2}' and mDivisionid = '{3}'", this.CurrentDetailData["threadColorid"], this.CurrentDetailData["refno"], newvalue, this.keyWord);
                    if (MyUtility.Check.Seek(sql, out refdr))
                    {
                        this.CurrentDetailData["stocknew"] = refdr["NewCone"];
                        this.CurrentDetailData["stockused"] = refdr["UsedCone"];
                    }
                    else
                    {
                        this.CurrentDetailData["stocknew"] = 0;
                        this.CurrentDetailData["stockused"] = 0;
                    }

                    this.CurrentDetailData["ThreadLocationid"] = newvalue;
                }
                else
                {
                    this.CurrentDetailData["ThreadLocationid"] = string.Empty;
                    this.CurrentDetailData["stocknew"] = 0;
                    this.CurrentDetailData["stockused"] = 0;
                }

                this.CurrentDetailData.EndEdit();
             };
            #endregion
            #region setup Grid

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20), settings: refno)
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(15), settings: thcolor)
            .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .CellThreadLocation("ThreadLocationid", header: "Location", width: Widths.AnsiChars(10), settings: threadlocaion, isChangeSelItem: true)
            .Numeric("stocknew", header: "New Cone \nin Stock", width: Widths.AnsiChars(6), integer_places: 5, iseditingreadonly: true)
            .Numeric("stockused", header: "Used Cone \nin Stock", width: Widths.AnsiChars(6), integer_places: 5, iseditingreadonly: true)
            .Numeric("NewCone", header: "New Cone", width: Widths.AnsiChars(6), integer_places: 5)
            .Numeric("UsedCone", header: "Used Cone", width: Widths.AnsiChars(6), integer_places: 5)
            .Text("Remark", header: "Remarks", width: Widths.AnsiChars(30));
            #endregion

            this.detailgrid.Columns["Refno"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ThreadColorid"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ThreadLocationid"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["NewCone"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["UsedCone"].DefaultCellStyle.BackColor = Color.Pink;

            return base.OnGridSetup();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label7.Text = this.CurrentMaintain["Status"].ToString();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["cDate"] = DateTime.Now;
            this.CurrentMaintain["AddName"] = this.loginID;
            this.CurrentMaintain["AddDate"] = DateTime.Now;
            this.CurrentMaintain["mDivisionid"] = this.keyWord;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't be deleted.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't be modify", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["cDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("<Date> can not be empty!", "Warning");
                this.dateDate.Focus();
                return false;
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Refno"]))
                {
                    MyUtility.Msg.WarningBox("<Refno> can not be empty!", "Warning");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["ThreadColorid"]))
                {
                    MyUtility.Msg.WarningBox("<Thread Color> can not be empty!", "Warning");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["ThreadLocationid"]))
                {
                    MyUtility.Msg.WarningBox("<Location> can not be empty!", "Warning");
                    return false;
                }

                if ((decimal)dr["NewCone"] == 0 && (decimal)dr["UsedCone"] == 0)
                {
                    MyUtility.Msg.WarningBox("<New Cone> and <Used Cone> can not be empty!", "Warning");
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
                string id = MyUtility.GetValue.GetID(this.keyWord + "TS", "ThreadIssue", (DateTime)this.CurrentMaintain["cDate"]);

                if (string.IsNullOrWhiteSpace(id))
                {
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailUIConvertToUpdate()
        {
            base.OnDetailUIConvertToUpdate();
            this.dateDate.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            string updateThread = string.Format("update ThreadIssue set Status = 'New' ,editname='{0}', editdate = GETDATE() where id='{1}' ;", this.loginID, this.CurrentMaintain["ID"].ToString());
            string insertsql = string.Empty;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadStock WITH (NOLOCK) where refno ='{0}' and ThreadColorid = '{1}' and threadLocationid = '{2}' and mDivisionid = '{3}'", dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), this.keyWord)))
                {
                    updateThread = updateThread + string.Format("update ThreadStock set UsedCone = UsedCone+ {0},NewCone = NewCone+ {1},EditName ='{6}',editDate = GetDate() where refno ='{2}' and ThreadColorid = '{3}' and threadLocationid = '{4}' and mDivisionid ='{5}' ;", dr["usedCone"], dr["newcone"], dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), this.keyWord, this.loginID);
                }
                else
                {
                    insertsql = insertsql + string.Format("Insert into ThreadStock(Refno,ThreadColorid,ThreadLocationid,NewCone,UsedCone,mDivisionid,AddName,AddDate) values('{0}','{1}','{2}',{3},{4},{5},'{6}',GETDATE())", dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), dr["newcone"], dr["usedCone"], this.keyWord, this.loginID);
                }
            }

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updateThread)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(updateThread, upResult);
                        return;
                    }

                    if (!MyUtility.Check.Empty(insertsql))
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, insertsql)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(insertsql, upResult);
                            return;
                        }
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string checksql;
            string updatesql = string.Format("update ThreadIssue set Status = 'Confirmed' ,editname='{0}', editdate = GETDATE() where id='{1}' ;", this.loginID, this.CurrentMaintain["ID"].ToString());
            DataRow thdr;
            string msg1 = "New cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n", msg2 = "Used cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n";
            bool lmsg1 = false, lmsg2 = false;
            foreach (DataRow dr in this.DetailDatas)
            {
                checksql = string.Format("Select isnull(newCone,0) as newCone,isnull(UsedCone,0) as usedCone from ThreadStock WITH (NOLOCK) where refno='{0}' and threadcolorid = '{1}' and threadlocationid ='{2}' and mDivisionid ='{3}' ", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"], this.keyWord);
                if (MyUtility.Check.Seek(checksql, out thdr))
                {
                    if ((decimal)thdr["Newcone"] < (decimal)dr["NewCone"])
                    {
                        msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                        lmsg1 = true;
                    }

                    if ((decimal)thdr["UsedCone"] < (decimal)dr["UsedCone"])
                    {
                        msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                        lmsg2 = true;
                    }
                }
                else
                {
                    msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                    lmsg1 = true;
                    msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                    lmsg2 = true;
                }

                updatesql = updatesql + string.Format("update ThreadStock set UsedCone = UsedCone-{0},NewCone = NewCone-{1},editName = '{6}',editDate = GetDate() where refno ='{2}' and ThreadColorid = '{3}' and threadLocationid = '{4}' and mDivisionid ='{5}' ;", dr["usedCone"], dr["newcone"], dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), this.keyWord, this.loginID);
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

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(updatesql, upResult);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
        }
    }
}
