using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;
using Sci.Production.Class;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P06
    /// </summary>
    public partial class P06 : Win.Tems.Input6
    {
        private string loginID = Env.User.UserID;
        private string keyWord = Env.User.Keyword;

        /// <summary>
        /// P06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select a.*,b.description,c.description as colordesc,
            b.threadTex,b.category,b.Localsuppid,newcone - newconebook as newConevar,usedcone - usedconebook as usedConevar,b.threadtypeid,
            (b.Localsuppid+'-'+(Select name from LocalSupp d WITH (NOLOCK) where b.localsuppid = d.id)) as supp
            from ThreadAdjust_Detail a WITH (NOLOCK) 
            left join Localitem b WITH (NOLOCK) on a.refno = b.refno 
            left join threadcolor c WITH (NOLOCK) on a.threadcolorid = c.id where a.id = '{0}'",
                masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings refno = Txtlocalitem.Celllocalitem.GetGridCell("Thread", null, "LocalSuppid,Supp,category,Description,ThreadTex,ThreadTypeid");
            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings thlocation = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();

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
                    this.CurrentDetailData["ThreadTex"] = refdr["ThreadTex"];
                    this.CurrentDetailData["LocalSuppid"] = refdr["LocalSuppid"];
                    this.CurrentDetailData["Supp"] = refdr["LocalSuppid"].ToString() + "-" + MyUtility.GetValue.Lookup("Name", refdr["LocalSuppid"].ToString(), "LocalSupp", "ID");
                    this.CurrentDetailData["ThreadTypeid"] = refdr["ThreadTypeid"];
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                }
                else
                {
                    this.CurrentDetailData["Description"] = string.Empty;
                    this.CurrentDetailData["Refno"] = string.Empty;
                    this.CurrentDetailData["category"] = string.Empty;
                    this.CurrentDetailData["ThreadTex"] = 0;
                    this.CurrentDetailData["LocalSuppid"] = string.Empty;
                    this.CurrentDetailData["Supp"] = string.Empty;
                    this.CurrentDetailData["ThreadTypeid"] = string.Empty;
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                }

                string sql = string.Format("Select newcone, usedcone from ThreadStock WITH (NOLOCK) where refno ='{1}' and threadcolorid = '{0}' and threadlocationid = '{2}' ", this.CurrentDetailData["threadColorid"], newvalue, this.CurrentDetailData["threadlocationid"]);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["newconebook"] = refdr["newcone"];
                    this.CurrentDetailData["usedconebook"] = refdr["usedcone"];
                    this.Reqty();
                }
                else
                {
                    this.CurrentDetailData["newconebook"] = 0;
                    this.CurrentDetailData["usedconebook"] = 0;
                    this.Reqty();
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

                string sql = string.Format("Select newcone,usedcone from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and threadlocationid = '{2}' ", this.CurrentDetailData["Refno"], newvalue, this.CurrentDetailData["threadlocationid"]);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["newconebook"] = refdr["newcone"];
                    this.CurrentDetailData["usedconebook"] = refdr["usedcone"];
                    this.Reqty();
                }
                else
                {
                    this.CurrentDetailData["newconebook"] = 0;
                    this.CurrentDetailData["usedconebook"] = 0;
                    this.Reqty();
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region location
            thlocation.CellValidating += (s, e) =>
                {
                    if (!this.EditMode || e.RowIndex == -1)
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

                    string sql = string.Format("Select newcone,usedcone from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and threadlocationid = '{2}'", this.CurrentDetailData["Refno"], this.CurrentDetailData["threadcolorid"], newvalue);
                    if (MyUtility.Check.Seek(sql, out refdr))
                    {
                        this.CurrentDetailData["newconebook"] = refdr["newcone"];
                        this.CurrentDetailData["usedconebook"] = refdr["usedcone"];
                        this.Reqty();
                    }
                    else
                    {
                        this.CurrentDetailData["newconebook"] = 0;
                        this.CurrentDetailData["usedconebook"] = 0;
                        this.Reqty();
                    }

                    if (MyUtility.Check.Seek(newvalue, "ThreadLocation", "ID"))
                    {
                        this.CurrentDetailData["ThreadLocationid"] = newvalue;
                    }
                    else
                    {
                        this.CurrentDetailData["ThreadLocationid"] = string.Empty;
                    }

                    this.CurrentDetailData.EndEdit();
                };

            #endregion
            #region Qty
            qty.CellValidating += (s, e) =>
                {
                    if (!this.EditMode || e.RowIndex == -1)
                    {
                        return;
                    }

                    if (this.detailgrid.Columns[e.ColumnIndex].HeaderText.ToString() == "New Cone")
                    {
                        this.CurrentDetailData["newCone"] = e.FormattedValue;
                    }

                    if (this.detailgrid.Columns[e.ColumnIndex].HeaderText.ToString() == "Used Cone")
                    {
                        this.CurrentDetailData["UsedCone"] = e.FormattedValue;
                    }

                    this.Reqty();
                    this.CurrentDetailData.EndEdit();
                };
            #endregion
            #region setup Grid

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20), settings: refno)
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(15), settings: thcolor)
            .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .CellThreadLocation("ThreadLocationid", header: "Location", width: Widths.AnsiChars(10), settings: thlocation)
            .Numeric("NewConebook", header: "New Cone\nperbooks", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
            .Numeric("NewCone", header: "New Cone", width: Widths.AnsiChars(5), integer_places: 5, settings: qty)
            .Numeric("NewConeVar", header: "New Cone\nVariance", width: Widths.AnsiChars(5), integer_places: 5, iseditable: false)

            .Numeric("UsedConebook", header: "Used cone\nperbooks", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
            .Numeric("UsedCone", header: "Used Cone", width: Widths.AnsiChars(5), integer_places: 5, settings: qty)
             .Numeric("UsedConeVar", header: "Used Cone\nVariance", width: Widths.AnsiChars(5), integer_places: 5, iseditable: false)
            .Text("ThreadTypeid", header: "Thread Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("ThreadTex", header: "Tex", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
            .Text("category", header: "Category", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("remark", header: "Remark", width: Widths.AnsiChars(60));
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

                if (MyUtility.Check.Empty(dr["ThreadLocationid"]))
                {
                    MyUtility.Msg.WarningBox("<Location> can not be empty!", "Warning");
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
                string id = MyUtility.GetValue.GetID(this.keyWord + "TA", "ThreadAdjust", (DateTime)this.CurrentMaintain["cDate"]);

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
            sql = string.Format("Update ThreadAdjust set Status = 'Confirmed',editname='{0}', editdate = GETDATE() where id='{1}'", this.loginID, this.CurrentMaintain["ID"].ToString());

            foreach (DataRow dr in this.DetailDatas)
            {
                checksql = string.Format("Select * from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadLocationid = '{1}' and threadcolorid = '{2}' ", dr["refno"], dr["threadlocationid"], dr["threadcolorid"]);
                if (MyUtility.Check.Seek(checksql))
                {
                    insertSql = insertSql + string.Format("Update ThreadStock set NewCone = NewCone+({0}),UsedCone = UsedCone + ({1}) where refno='{2}' and ThreadColorid = '{3}' and ThreadLocationid = '{4}';", (decimal)dr["NewCone"] - (decimal)dr["NewConeBook"], (decimal)dr["UsedCone"] - (decimal)dr["UsedConeBook"], dr["refno"], dr["ThreadColorid"], dr["ThreadLocationid"]);
                }
                else
                {
                    insertSql = insertSql + string.Format("insert ThreadStock(refno,threadcolorid,threadlocationid,newcone,usedcone,addName,AddDate) values('{0}','{1}','{2}','{3}',{4},'{5}',GetDate());", dr["refno"], dr["ThreadColorid"], dr["ThreadLocationid"], (decimal)dr["NewCone"] - (decimal)dr["NewConeBook"], (decimal)dr["UsedCone"] - (decimal)dr["UsedConeBook"], this.loginID);
                }
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

        private void BtnFind_Click(object sender, EventArgs e)
        {
            // 移到指定那筆
            string refno = this.txtRefnoLocation.Text;
            int index = this.detailgridbs.Find("Refno", refno);
            if (index == -1)
            {
                int index2 = this.detailgridbs.Find("ThreadLocationid", refno);
                if (index2 == -1)
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                }
                else
                {
                    this.detailgridbs.Position = index2;
                }
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DataTable detTable = (DataTable)this.detailgridbs.DataSource;
            Form p06_import = new P06_Import(detTable);
            p06_import.ShowDialog();
        }

        private void Reqty()
        {
            this.CurrentDetailData["newConeVar"] = (decimal)this.CurrentDetailData["newcone"] - (decimal)this.CurrentDetailData["newconebook"];
            this.CurrentDetailData["usedConeVar"] = (decimal)this.CurrentDetailData["usedcone"] - (decimal)this.CurrentDetailData["usedconebook"];
        }
    }
}
