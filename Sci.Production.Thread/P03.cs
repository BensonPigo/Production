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
    public partial class P03 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string defaultfilter = string.Format("MDivisionid = '{0}' ", keyWord);
            this.DefaultFilter = defaultfilter;
            InitializeComponent();
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.*,b.description,c.description as colordesc,
            b.threadTex,b.category,b.Localsuppid,b.Weight,b.axleweight,b.MeterToCone,
            (b.Localsuppid+'-'+(Select name from LocalSupp d WITH (NOLOCK) where b.localsuppid = d.id)) as supp
            from ThreadIncoming_Detail a WITH (NOLOCK) 
            left join Localitem b WITH (NOLOCK) on a.refno = b.refno 
            left join threadcolor c WITH (NOLOCK) on a.threadcolorid = c.id where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override bool OnGridSetup()
        {

            DataGridViewGeneratorTextColumnSettings refno = celllocalitem.GetGridCell("Thread", null, "LocalSuppid,supp,category,description,ThreadTex,,MeterToCone,Weight,AxleWeight");
            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings pcsused = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings totalweight = new DataGridViewGeneratorNumericColumnSettings();
            
            #region Refno Cell
            refno.CellValidating += (s, e) =>
            {

                if (!this.EditMode ) return;
                string oldvalue = CurrentDetailData["refno"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from Localitem WITH (NOLOCK) where refno = '{0}' and junk = 0", newvalue), out refdr))
                {
                    CurrentDetailData["Description"] = refdr["Description"];
                    CurrentDetailData["Refno"] = refdr["refno"];
                    CurrentDetailData["MeterToCone"] = refdr["MeterToCone"];
                    CurrentDetailData["category"] = refdr["category"];
                    CurrentDetailData["ThreadTex"] = refdr["ThreadTex"];
                    CurrentDetailData["LocalSuppid"] = refdr["LocalSuppid"];
                    CurrentDetailData["Supp"] = refdr["LocalSuppid"].ToString() + "-" + MyUtility.GetValue.Lookup("Name", refdr["LocalSuppid"].ToString(), "LocalSupp", "ID");
                    CurrentDetailData["Weight"] = refdr["Weight"];
                    CurrentDetailData["AxleWeight"] = refdr["AxleWeight"];
                    CurrentDetailData["TotalWeight"] = 0;
                    CurrentDetailData["NewCone"] = 0;
                    CurrentDetailData["UsedCone"] = 0;
                    CurrentDetailData["pcsused"] = 0;

                }
                else
                {
                    CurrentDetailData["Description"] = "";
                    CurrentDetailData["Refno"] = "";
                    CurrentDetailData["MeterToCone"] = 0;
                    CurrentDetailData["category"] = "";
                    CurrentDetailData["ThreadTex"] = 0;
                    CurrentDetailData["LocalSuppid"] = "";
                    CurrentDetailData["Supp"] = "";
                    CurrentDetailData["Weight"] = 0;
                    CurrentDetailData["AxleWeight"] = 0;
                    CurrentDetailData["TotalWeight"] = 0;
                    CurrentDetailData["NewCone"] = 0;
                    CurrentDetailData["UsedCone"] = 0;
                    CurrentDetailData["pcsused"] = 0;
                }
                string sql = string.Format("Select top(1) ThreadLocationid from ThreadStock WITH (NOLOCK) where refno ='{1}' and threadcolorid = '{0}' and mDivisionid ='{2}' ", CurrentDetailData["threadColorid"].ToString(), newvalue, keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    CurrentDetailData["ThreadLocationid"] = refdr["ThreadLocationid"];
                }
                else
                {
                    CurrentDetailData["ThreadLocationid"] = "";
                }
                CurrentDetailData.EndEdit();
            };
            #endregion
            #region Color Cell
            thcolor.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                string oldvalue = CurrentDetailData["threadcolorid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                
                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadColor WITH (NOLOCK) where id = '{0}' and junk = 0", newvalue), out refdr))
                {
                    CurrentDetailData["ThreadColorid"] = refdr["ID"];
                    CurrentDetailData["Colordesc"] = refdr["Description"];
                }
                else
                {
                    CurrentDetailData["ThreadColorid"] = "";
                    CurrentDetailData["Colordesc"] = "";

                }
                string sql = string.Format("Select top(1) ThreadLocationid from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and mDivisionid = '{2}'", CurrentDetailData["Refno"].ToString(), newvalue, keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    CurrentDetailData["ThreadLocationid"] = refdr["ThreadLocationid"];
                }
                else
                {
                    CurrentDetailData["ThreadLocationid"] = "";
                }
                CurrentDetailData.EndEdit();
            };
            #endregion
            #region pcsused
            pcsused.CellValidating += (s, e) =>
            {
                decimal newvalue = (decimal)e.FormattedValue;
                decimal netweight = (decimal)CurrentDetailData["Weight"] - (decimal)CurrentDetailData["AxleWeight"]; //線減掉軸心重
                decimal totalaxle = newvalue * (decimal)CurrentDetailData["AxleWeight"]; //總軸心重
                decimal totalnetweight = (decimal)CurrentDetailData["TotalWeight"] - totalaxle;//總重不含軸心

                CurrentDetailData["UsedCone"] = (netweight <= 0 || totalnetweight <= 0) ? 0 : Math.Floor(totalnetweight / netweight);
                CurrentDetailData["pcsused"] = newvalue;
                CurrentDetailData.EndEdit();
            };
            #endregion
            #region totalweight
            totalweight.CellValidating += (s, e) =>
            {
                decimal newvalue = (decimal)e.FormattedValue;
                decimal noofpsc = (decimal)CurrentDetailData["pcsused"]; //Used數量
                decimal netweight = (decimal)CurrentDetailData["Weight"] - (decimal)CurrentDetailData["AxleWeight"]; //線減掉軸心重
                decimal totalaxle = noofpsc * (decimal)CurrentDetailData["AxleWeight"]; //總軸心重
                decimal totalnetweight = newvalue - totalaxle;//總重不含軸心

                CurrentDetailData["UsedCone"] = (netweight <= 0 || totalnetweight<=0) ? 0 : Math.Floor(totalnetweight / netweight);
                CurrentDetailData["TotalWeight"] = newvalue;
                CurrentDetailData.EndEdit();
            };
            #endregion
            #region setup Grid

            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(10), settings: refno)
            .Text("Description", header: "Description", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(4), settings: thcolor)
            .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Numeric("ThreadTex", header: "Tex", width: Widths.AnsiChars(2), integer_places: 3, iseditingreadonly: true)
            .Text("category", header: "Category", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("supp", header: "Supplier", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Numeric("NewCone", header: "New\r\nCone", width: Widths.AnsiChars(2), integer_places: 3)
            .Numeric("pcsused", header: "No Of Psc. \nFor Used Cone", width: Widths.AnsiChars(5), integer_places: 3,settings : pcsused)
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
                MyUtility.Msg.WarningBox("Data is confirmed, can't be deleted.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't be modify", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["cDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("<In-coming Date> can not be empty!", "Warning");
                this.dateDate.Focus();
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
                if (MyUtility.Check.Empty(dr["ThreadLocationid"]) )
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
                string id = MyUtility.GetValue.GetID(keyWord + "TN", "ThreadIncoming", (DateTime)CurrentMaintain["cDate"]);

                if (string.IsNullOrWhiteSpace(id))
                {
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            #endregion

            return base.ClickSaveBefore();
        }
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
        protected override void OnDetailUIConvertToUpdate()
        {
            base.OnDetailUIConvertToUpdate();
            dateDate.ReadOnly = true;
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string updateThread = string.Format("update ThreadIncoming set Status = 'Confirmed' ,editname='{0}', editdate = GETDATE() where id='{1}' ;", loginID, CurrentMaintain["ID"].ToString());
            string insertsql = "";
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Seek(String.Format("Select * from ThreadStock WITH (NOLOCK) where refno ='{0}' and ThreadColorid = '{1}' and threadLocationid = '{2}' and mDivisionid ='{3}' ", dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), keyWord)))
                {
                    updateThread = updateThread + string.Format("update ThreadStock set UsedCone = UsedCone+ {0},NewCone = NewCone+ {1},editName ='{6}', editDate = getdate() where refno ='{2}' and ThreadColorid = '{3}' and threadLocationid = '{4}' and mDivisionid ='{5}' ;", dr["usedCone"], dr["newcone"], dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), keyWord, loginID);
                }
                else
                {
                    insertsql = insertsql + string.Format("Insert into ThreadStock(Refno,ThreadColorid,ThreadLocationid,NewCone,UsedCone,mDivisionid,addName,AddDate) values('{0}','{1}','{2}',{3},{4},'{5}','{6}',GetDate());",dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), dr["newcone"],dr["usedCone"],keyWord,loginID);
                }
            }
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updateThread)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(updateThread, upResult);
                        return;
                    }
                    if (!MyUtility.Check.Empty(insertsql))
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, insertsql)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(insertsql, upResult);
                            return;
                        }
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
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
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            string checksql;
            string updatesql = string.Format("update ThreadIncoming set Status = 'New' ,editname='{0}', editdate = GETDATE() where id='{1}' ;", loginID, CurrentMaintain["ID"].ToString());
            DataRow thdr;
            string msg1 = "New cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n", msg2 = "Used cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n";
            bool lmsg1 = false, lmsg2 = false;
            foreach (DataRow dr in DetailDatas)
            {
                checksql = string.Format("Select isnull(newCone,0) as newCone,isnull(UsedCone,0) as usedCone from ThreadStock WITH (NOLOCK) where refno='{0}' and threadcolorid = '{1}' and threadlocationid ='{2}' and mDivisionid ='{3}' ", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"], keyWord);
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
                updatesql = updatesql + string.Format("update ThreadStock set UsedCone = UsedCone-{0},NewCone = NewCone-{1},editName = '{6}',editDate = GetDate() where refno ='{2}' and ThreadColorid = '{3}' and threadLocationid = '{4}' and mDivisionid ='{5}' ;", dr["usedCone"], dr["newcone"], dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(),keyWord,loginID);
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
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(updatesql, upResult);
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
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
    }
}
