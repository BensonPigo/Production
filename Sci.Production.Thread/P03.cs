using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Data;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P03
    /// </summary>
    public partial class P03 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;

        /// <summary>
        /// P03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select a.*,b.description,c.description as colordesc,
            b.threadTex,b.category,b.Localsuppid,b.Weight,b.axleweight,b.MeterToCone,
            (b.Localsuppid+'-'+(Select name from LocalSupp d WITH (NOLOCK) where b.localsuppid = d.id)) as supp
            from ThreadIncoming_Detail a WITH (NOLOCK) 
            left join Localitem b WITH (NOLOCK) on a.refno = b.refno 
            left join threadcolor c WITH (NOLOCK) on a.threadcolorid = c.id where a.id = '{0}'",
                masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings refno = celllocalitem.GetGridCell("Thread", null, "LocalSuppid,supp,category,description,ThreadTex,,MeterToCone,Weight,AxleWeight");
            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings pcsused = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings totalweight = new DataGridViewGeneratorNumericColumnSettings();

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
                    this.CurrentDetailData["MeterToCone"] = refdr["MeterToCone"];
                    this.CurrentDetailData["category"] = refdr["category"];
                    this.CurrentDetailData["ThreadTex"] = refdr["ThreadTex"];
                    this.CurrentDetailData["LocalSuppid"] = refdr["LocalSuppid"];
                    this.CurrentDetailData["Supp"] = refdr["LocalSuppid"].ToString() + "-" + MyUtility.GetValue.Lookup("Name", refdr["LocalSuppid"].ToString(), "LocalSupp", "ID");
                    this.CurrentDetailData["Weight"] = refdr["Weight"];
                    this.CurrentDetailData["AxleWeight"] = refdr["AxleWeight"];
                    this.CurrentDetailData["TotalWeight"] = 0;
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                    this.CurrentDetailData["pcsused"] = 0;
                }
                else
                {
                    this.CurrentDetailData["Description"] = string.Empty;
                    this.CurrentDetailData["Refno"] = string.Empty;
                    this.CurrentDetailData["MeterToCone"] = 0;
                    this.CurrentDetailData["category"] = string.Empty;
                    this.CurrentDetailData["ThreadTex"] = 0;
                    this.CurrentDetailData["LocalSuppid"] = string.Empty;
                    this.CurrentDetailData["Supp"] = string.Empty;
                    this.CurrentDetailData["Weight"] = 0;
                    this.CurrentDetailData["AxleWeight"] = 0;
                    this.CurrentDetailData["TotalWeight"] = 0;
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                    this.CurrentDetailData["pcsused"] = 0;
                }

                string sql = string.Format("Select top(1) ThreadLocationid from ThreadStock WITH (NOLOCK) where refno ='{1}' and threadcolorid = '{0}' ", this.CurrentDetailData["threadColorid"].ToString(), newvalue);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["ThreadLocationid"] = refdr["ThreadLocationid"];
                }
                else
                {
                    this.CurrentDetailData["ThreadLocationid"] = string.Empty;
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

                string sql = string.Format("Select top(1) ThreadLocationid from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' ", this.CurrentDetailData["Refno"].ToString(), newvalue);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["ThreadLocationid"] = refdr["ThreadLocationid"];
                }
                else
                {
                    this.CurrentDetailData["ThreadLocationid"] = string.Empty;
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region pcsused
            pcsused.CellValidating += (s, e) =>
            {
                decimal newvalue = (decimal)e.FormattedValue;
                decimal netweight = (decimal)this.CurrentDetailData["Weight"] - (decimal)this.CurrentDetailData["AxleWeight"]; // 線減掉軸心重
                decimal totalaxle = newvalue * (decimal)this.CurrentDetailData["AxleWeight"]; // 總軸心重
                decimal totalnetweight = (decimal)this.CurrentDetailData["TotalWeight"] - totalaxle; // 總重不含軸心

                this.CurrentDetailData["UsedCone"] = (netweight <= 0 || totalnetweight <= 0) ? 0 : Math.Floor(totalnetweight / netweight);
                this.CurrentDetailData["pcsused"] = newvalue;
                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region totalweight
            totalweight.CellValidating += (s, e) =>
            {
                decimal newvalue = (decimal)e.FormattedValue;
                decimal noofpsc = (decimal)this.CurrentDetailData["pcsused"]; // Used數量
                decimal netweight = (decimal)this.CurrentDetailData["Weight"] - (decimal)this.CurrentDetailData["AxleWeight"]; // 線減掉軸心重
                decimal totalaxle = noofpsc * (decimal)this.CurrentDetailData["AxleWeight"]; // 總軸心重
                decimal totalnetweight = newvalue - totalaxle; // 總重不含軸心

                this.CurrentDetailData["UsedCone"] = (netweight <= 0 || totalnetweight <= 0) ? 0 : Math.Floor(totalnetweight / netweight);
                this.CurrentDetailData["TotalWeight"] = newvalue;
                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region setup Grid

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(10), settings: refno)
            .Text("Description", header: "Description", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(4), settings: thcolor)
            .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Numeric("ThreadTex", header: "Tex", width: Widths.AnsiChars(2), integer_places: 3, iseditingreadonly: true)
            .Text("category", header: "Category", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("supp", header: "Supplier", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Numeric("NewCone", header: "New\r\nCone", width: Widths.AnsiChars(2), integer_places: 3)
            .Numeric("pcsused", header: "No Of Psc. \nFor Used Cone", width: Widths.AnsiChars(5), integer_places: 3, settings: pcsused)
            .Numeric("TotalWeight", header: "Total used \nWeight (G.W.)", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, settings: totalweight)
            .Numeric("Weight", header: "Weight Of \nOne Cone(G.W.)", width: Widths.AnsiChars(12), integer_places: 5, decimal_places: 6, iseditingreadonly: true)
            .Numeric("AxleWeight", header: "Center Of \nAxle weight", width: Widths.AnsiChars(10), integer_places: 5, decimal_places: 4, iseditingreadonly: true)
            .Numeric("UsedCone", header: "Used\r\ncone", width: Widths.AnsiChars(2), integer_places: 5, iseditingreadonly: true)
            .CellThreadLocation("ThreadLocationid", header: "Location", width: Widths.AnsiChars(8));
            #endregion

            this.detailgrid.Columns["Refno"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ThreadColorid"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["NewCone"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["pcsused"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["TotalWeight"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ThreadLocationid"].DefaultCellStyle.BackColor = Color.Pink;

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
                MyUtility.Msg.WarningBox("<In-coming Date> can not be empty!", "Warning");
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
                string id = MyUtility.GetValue.GetID(this.keyWord + "TN", "ThreadIncoming", (DateTime)this.CurrentMaintain["cDate"]);

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
        protected override void OnDetailGridDataInserted(DataRow data)
        {
            base.OnDetailGridDataInserted(data);
            data["MeterToCone"] = 0;
            data["ThreadTex"] = 0;
            data["Weight"] = 0;
            data["AxleWeight"] = 0;
            data["TotalWeight"] = 0;
            data["pcsused"] = 0;
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
            string updateThread = string.Format("update ThreadIncoming set Status = 'Confirmed' ,editname='{0}', editdate = GETDATE() where id='{1}' ;", this.loginID, this.CurrentMaintain["ID"].ToString());
            string insertsql = string.Empty;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadStock WITH (NOLOCK) where refno ='{0}' and ThreadColorid = '{1}' and threadLocationid = '{2}' ", dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString())))
                {
                    updateThread = updateThread + string.Format("update ThreadStock set UsedCone = UsedCone+ {0},NewCone = NewCone+ {1},editName ='{5}', editDate = getdate() where refno ='{2}' and ThreadColorid = '{3}' and threadLocationid = '{4}' ;", dr["usedCone"], dr["newcone"], dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), this.loginID);
                }
                else
                {
                    insertsql = insertsql + string.Format("Insert into ThreadStock(Refno,ThreadColorid,ThreadLocationid,NewCone,UsedCone,addName,AddDate) values('{0}','{1}','{2}',{3},{4},'{5}',GetDate());", dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), dr["newcone"], dr["usedCone"], this.loginID);
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
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            string checksql;
            string updatesql = string.Format("update ThreadIncoming set Status = 'New' ,editname='{0}', editdate = GETDATE() where id='{1}' ;", this.loginID, this.CurrentMaintain["ID"].ToString());
            DataRow thdr;
            string msg1 = "New cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n", msg2 = "Used cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n";
            bool lmsg1 = false, lmsg2 = false;
            foreach (DataRow dr in this.DetailDatas)
            {
                checksql = string.Format("Select isnull(newCone,0) as newCone,isnull(UsedCone,0) as usedCone from ThreadStock WITH (NOLOCK) where refno='{0}' and threadcolorid = '{1}' and threadlocationid ='{2}' ", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
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

                updatesql = updatesql + string.Format("update ThreadStock set UsedCone = UsedCone-{0},NewCone = NewCone-{1},editName = '{5}',editDate = GetDate() where refno ='{2}' and ThreadColorid = '{3}' and threadLocationid = '{4}';", dr["usedCone"], dr["newcone"], dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), this.loginID);
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
