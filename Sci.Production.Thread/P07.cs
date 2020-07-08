using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P07
    /// </summary>
    public partial class P07 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;

        /// <summary>
        /// P07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select  a.*
        , b.description
        , c.description as colordesc
        , b.category
        , newconebook = isnull(d.newCone, 0)
        , usedconebook = isnull(d.usedcone, 0)
from ThreadTransfer_Detail a WITH (NOLOCK) 
left join Localitem b WITH (NOLOCK) on a.refno = b.refno 
left join ThreadStock d WITH (NOLOCK) on d.refno = a.refno and d.threadColorid = a.threadColorid 
    and d.threadLocationid = a.Locationfrom 
left join threadcolor c WITH (NOLOCK) on a.threadcolorid = c.id where a.id = '{0}'",
                masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings refno = celllocalitem.GetGridCell("Thread", null, ",,category,Description");
            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings thlocationfrom = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings thlocationto = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings qty2 = new DataGridViewGeneratorNumericColumnSettings();

            #region Refno Cell
            refno.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
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
                    this.CurrentDetailData["category"] = refdr["category"];
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                }
                else
                {
                    this.CurrentDetailData["Description"] = string.Empty;
                    this.CurrentDetailData["Refno"] = string.Empty;
                    this.CurrentDetailData["category"] = string.Empty;
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                }

                string sql = string.Format("Select newcone, usedcone from ThreadStock WITH (NOLOCK) where refno ='{1}' and threadcolorid = '{0}' and threadlocationid = '{2}' ", this.CurrentDetailData["threadColorid"], newvalue, this.CurrentDetailData["locationfrom"]);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["newconebook"] = refdr["newcone"];
                    this.CurrentDetailData["usedconebook"] = refdr["usedcone"];
                }
                else
                {
                    this.CurrentDetailData["newconebook"] = 0;
                    this.CurrentDetailData["usedconebook"] = 0;
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region Color Cell
            thcolor.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
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

                string sql = string.Format("Select newcone,usedcone from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and threadlocationid = '{2}' ", this.CurrentDetailData["Refno"], newvalue, this.CurrentDetailData["locationfrom"]);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["newconebook"] = refdr["newcone"];
                    this.CurrentDetailData["usedconebook"] = refdr["usedcone"];
                }
                else
                {
                    this.CurrentDetailData["newconebook"] = 0;
                    this.CurrentDetailData["usedconebook"] = 0;
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region location
            thlocationfrom.EditingMouseDown += (s, e) =>
                {
                    if (!this.EditMode || e.RowIndex == -1)
                    {
                        return;
                    }

                    if (e.Button != System.Windows.Forms.MouseButtons.Right)
                    {
                        return;
                    }

                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Description from ThreadLocation WITH (NOLOCK) where junk = 0 order by ID", "10,40", this.CurrentDetailData["locationfrom"].ToString());
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = item.GetSelectedString();
                };
            thlocationfrom.CellValidating += (s, e) =>
                {
                    if (!this.EditMode || e.RowIndex == -1)
                    {
                        return;
                    }

                    string oldvalue = this.CurrentDetailData["locationfrom"].ToString();
                    string newvalue = e.FormattedValue.ToString();
                    if (oldvalue == newvalue)
                    {
                        return;
                    }

                    DataRow refdr;

                    string sql = string.Format("Select newcone,usedcone from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and threadlocationid = '{2}' ", this.CurrentDetailData["Refno"], this.CurrentDetailData["threadcolorid"], newvalue);
                    if (MyUtility.Check.Seek(sql, out refdr))
                    {
                        this.CurrentDetailData["newconebook"] = refdr["newcone"];
                        this.CurrentDetailData["usedconebook"] = refdr["usedcone"];
                    }
                    else
                    {
                        this.CurrentDetailData["newconebook"] = 0;
                        this.CurrentDetailData["usedconebook"] = 0;
                    }

                    if (MyUtility.Check.Seek(newvalue, "ThreadLocation", "ID"))
                    {
                        this.CurrentDetailData["locationfrom"] = newvalue;
                    }
                    else
                    {
                        this.CurrentDetailData["Locationfrom"] = string.Empty;
                        MyUtility.Msg.WarningBox(string.Format(@"<Location: {0}> not found.", newvalue));
                    }

                    this.CurrentDetailData.EndEdit();
                };
            #endregion
            #region location
            thlocationto.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button != System.Windows.Forms.MouseButtons.Right)
                {
                    return;
                }

                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Description from ThreadLocation WITH (NOLOCK) where junk = 0 order by ID", "10,40", this.CurrentDetailData["locationto"].ToString());
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                e.EditingControl.Text = item.GetSelectedString();
            };
            thlocationto.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                string oldvalue = this.CurrentDetailData["locationto"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Seek(newvalue, "ThreadLocation", "ID"))
                {
                    this.CurrentDetailData["locationto"] = newvalue;
                }
                else
                {
                    this.CurrentDetailData["Locationto"] = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format(@"<Location: {0}> not found", newvalue));
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion

            qty.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                decimal nc = MyUtility.Convert.GetDecimal(e.FormattedValue);
                if (nc > (decimal)this.CurrentDetailData["NewConebook"])
                {
                    this.CurrentDetailData["NewCone"] = 0;
                }

                this.CurrentDetailData.EndEdit();
            };
            qty2.CellValidating += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                decimal nc = MyUtility.Convert.GetDecimal(e.FormattedValue);
                if (nc > (decimal)this.CurrentDetailData["UsedConebook"])
                {
                    this.CurrentDetailData["UsedCone"] = 0;
                }

                this.CurrentDetailData.EndEdit();
            };
            #region setup Grid

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20), settings: refno)
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(15), settings: thcolor)
            .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Locationfrom", header: "Location (From)", width: Widths.AnsiChars(10), settings: thlocationfrom)
            .Numeric("NewConebook", header: "New Cone\nin Stock", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
            .Numeric("UsedConebook", header: "Used cone\nin Stock", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
            .Text("Locationto", header: "Location (To)", width: Widths.AnsiChars(10), settings: thlocationto)
            .Numeric("NewCone", header: "New Cone", width: Widths.AnsiChars(5), integer_places: 5, settings: qty)
            .Numeric("UsedCone", header: "Used Cone", width: Widths.AnsiChars(5), integer_places: 5, settings: qty2)
            .Text("category", header: "Category", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("remark", header: "Remark", width: Widths.AnsiChars(60));
            #endregion

            this.detailgrid.Columns["Refno"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ThreadColorid"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Locationfrom"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Locationto"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["NewCone"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["UsedCone"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["remark"].DefaultCellStyle.BackColor = Color.Pink;

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
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("<Date> can not be empty!", "Warning");
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

                if (MyUtility.Check.Empty(dr["Locationfrom"]))
                {
                    MyUtility.Msg.WarningBox("<Location(From)> can not be empty!", "Warning");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["Locationto"]))
                {
                    MyUtility.Msg.WarningBox("<Location(to)> can not be empty!", "Warning");
                    return false;
                }

                if ((decimal)dr["NewCone"] > (decimal)dr["NewConebook"])
                {
                    MyUtility.Msg.WarningBox("<NewCone> can not more than NewConeinStock !", "Warning");
                    return false;
                }

                if ((decimal)dr["UsedCone"] > (decimal)dr["UsedConebook"])
                {
                    MyUtility.Msg.WarningBox("<UsedCone> can not more than UsedConeinStock!", "Warning");
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
                string id = MyUtility.GetValue.GetID(this.keyWord + "TT", "ThreadTransfer", (DateTime)this.CurrentMaintain["cDate"]);

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
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            List<string> stockSqlinLi = new List<string>();
            List<string> stockSqlupLi = new List<string>();
            List<string> stockSqlQuLi = new List<string>();
            string checksql;
            string sql, insertSql = string.Empty;
            DataRow stdr;
            string msg1 = "New cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n",
                    msg2 = "Used cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n";

            bool lmsg1 = false;
            bool lmsg2 = false;
            sql = string.Format(
                @"
Update ThreadTransfer 
    set Status = 'Confirmed'
        , editname = '{0}'
        , editdate = GETDATE() 
where id='{1}'",
                this.loginID,
                this.CurrentMaintain["ID"].ToString());

            foreach (DataRow dr in this.DetailDatas)
            {
                checksql = string.Format(
                    @"
Select  a.*
        , d.newCone as newconebook
        , d.usedcone as usedconebook 
from ThreadTransfer_Detail a WITH (NOLOCK) 
left join ThreadStock d WITH (NOLOCK) on d.refno = a.refno 
    and d.threadColorid = a.threadColorid 
    and d.threadLocationid = a.Locationfrom 
where   d.refno ='{0}' 
        and threadLocationid = '{1}' 
        and d.threadcolorid = '{2}' ",
                    dr["refno"],
                    dr["locationfrom"],
                    dr["threadcolorid"]);

                // 找不到庫存或不夠不可confirm (這邊指的是轉出 Location 庫存)
                if (MyUtility.Check.Seek(checksql, out stdr))
                {
                    if ((decimal)stdr["NewConebook"] < (decimal)dr["NewCone"])
                    {
                        msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationfrom"]);
                        lmsg1 = true;
                    }

                    if ((decimal)stdr["UsedConebook"] < (decimal)dr["UsedCone"])
                    {
                        msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationfrom"]);
                        lmsg2 = true;
                    }

                    insertSql = insertSql + string.Format(
                        @"
Update ThreadStock 
    set NewCone = NewCone - ({0})
        , UsedCone = UsedCone - ({1}) 
where   refno = '{2}' 
        and ThreadColorid = '{3}' 
        and ThreadLocationid = '{4}';",
                        dr["NewCone"],
                        (decimal)dr["UsedCone"],
                        dr["refno"],
                        dr["ThreadColorid"],
                        dr["LocationFrom"]); // 扣庫存

                    checksql = string.Format(
                        @"
Select * 
from ThreadStock WITH (NOLOCK) 
where   refno ='{0}' 
        and threadLocationid = '{1}' 
        and threadcolorid = '{2}' ;",
                        dr["refno"],
                        dr["locationTo"],
                        dr["threadcolorid"]);

                    // 找不到庫存就新增 (這邊指的是轉入的 Location 庫存)
                    if (MyUtility.Check.Seek(checksql, out stdr))
                    {
                        insertSql = insertSql + string.Format(
                            @"
Update ThreadStock 
    set NewCone = NewCone + ({0})
        , UsedCone = UsedCone + ({1}) 
where   refno = '{2}' 
        and ThreadColorid = '{3}' 
        and ThreadLocationid = '{4}';",
                            dr["NewCone"],
                            dr["UsedCone"],
                            dr["refno"],
                            dr["ThreadColorid"],
                            dr["LocationTo"]);
                    }
                    else
                    {
                        insertSql = insertSql + string.Format(
                            @"
insert  ThreadStock(refno, threadcolorid, threadlocationid, newcone, usedcone, addName, AddDate) 
        values('{0}', '{1}', '{2}', '{3}', {4}, '{5}', GetDate());",
                            dr["refno"],
                            dr["ThreadColorid"],
                            dr["LocationTo"],
                            dr["NewCone"],
                            dr["UsedCone"],
                            this.loginID);
                    }
                }
                else
                {
                    /*
                     * 0 代表找不到庫存，所以庫存是 0
                     */
                    if ((decimal)dr["NewCone"] > 0)
                    {
                        msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationfrom"]);
                        lmsg1 = true;
                    }

                    if ((decimal)dr["UsedCone"] > 0)
                    {
                        msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["locationfrom"]);
                        lmsg2 = true;
                    }
                }
            }

            if (lmsg1 || lmsg2)
            {
                MyUtility.Msg.WarningBox(lmsg1 ? (lmsg2 ? msg1 + "\n\r" + msg2 : msg1) : (lmsg2 ? msg2 : string.Empty));
                return;
            }
            #region update Inqty,Status
            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, insertSql)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(insertSql, upResult);
                        return;
                    }

                    if (!(upResult = DBProxy.Current.Execute(null, sql)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sql, upResult);
                        return;
                    }

                    transactionscope.Complete();
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

            #endregion

        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            List<string> stockSqlinLi = new List<string>();
            List<string> stockSqlupLi = new List<string>();
            List<string> stockSqlQuLi = new List<string>();
            string checksql;
            string sql, insertSql = string.Empty;
            DataRow stdr;
            string msg1 = "New cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n",
                    msg2 = "Used cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n";

            bool lmsg1 = false;
            bool lmsg2 = false;
            sql = string.Format(
                @"
Update  ThreadTransfer 
    set Status = 'New'
        , editname = '{0}'
        , editdate = GETDATE() 
where id='{1}';",
                this.loginID,
                this.CurrentMaintain["ID"].ToString());

            foreach (DataRow dr in this.DetailDatas)
            {
                checksql = string.Format(
                    @"
Select * 
from ThreadStock WITH (NOLOCK) 
where   refno ='{0}' 
        and threadLocationid = '{1}' 
        and threadcolorid = '{2}' ",
                    dr["refno"],
                    dr["locationto"],
                    dr["threadcolorid"]);

                // 找不到庫存或不夠不可confirm
                if (MyUtility.Check.Seek(checksql, out stdr))
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

                    insertSql = insertSql + string.Format(
                        @"
Update ThreadStock 
    set NewCone = NewCone - ({0})
        , UsedCone = UsedCone - ({1}) 
where   refno='{2}' 
        and ThreadColorid = '{3}' 
        and ThreadLocationid = '{4}';",
                        dr["NewCone"],
                        (decimal)dr["UsedCone"],
                        dr["refno"],
                        dr["ThreadColorid"],
                        dr["locationto"]); // 扣庫存

                    checksql = string.Format(
                        @"
Select * 
from ThreadStock WITH (NOLOCK) 
where   refno ='{0}' 
        and threadLocationid = '{1}' 
        and threadcolorid = '{2}';",
                        dr["refno"],
                        dr["locationFrom"],
                        dr["threadcolorid"]);

                    // 找不到庫存就新增
                    if (MyUtility.Check.Seek(checksql, out stdr))
                    {
                        insertSql = insertSql + string.Format(
                            @"
Update ThreadStock 
    set NewCone = NewCone + ({0})
        , UsedCone = UsedCone + ({1}) 
where   refno='{2}' 
        and ThreadColorid = '{3}' 
        and ThreadLocationid = '{4}';",
                            dr["NewCone"],
                            dr["UsedCone"],
                            dr["refno"],
                            dr["ThreadColorid"],
                            dr["locationfrom"]);
                    }
                    else
                    {
                        insertSql = insertSql + string.Format(
                            @"
insert  ThreadStock (refno, threadcolorid, threadlocationid, newcone, usedcone, addName, AddDate) 
        values ('{0}', '{1}', '{2}', '{3}', {4}, {5}, GetDate());",
                            dr["refno"],
                            dr["ThreadColorid"],
                            dr["locationfrom"],
                            dr["NewCone"],
                            dr["UsedCone"],
                            this.loginID);
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

            if (lmsg1 || lmsg2)
            {
                MyUtility.Msg.WarningBox(lmsg1 ? (lmsg2 ? msg1 + "\n\r" + msg2 : msg1) : (lmsg2 ? msg2 : string.Empty));
                return;
            }
            #region update Inqty,Status
            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, insertSql)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(insertSql, upResult);
                        return;
                    }

                    if (!(upResult = DBProxy.Current.Execute(null, sql)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sql, upResult);
                        return;
                    }

                    transactionscope.Complete();
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

            #endregion

        }

        private void BtnImportFromStock_Click(object sender, EventArgs e)
        {
            DataTable detTable = (DataTable)this.detailgridbs.DataSource;
            Form p07_import = new Sci.Production.Thread.P07_Import(detTable);
            p07_import.ShowDialog();
        }
    }
}
