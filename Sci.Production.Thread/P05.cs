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
    public partial class P05 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.DefaultFilter = string.Format("mDivisionid = '{0}'", keyWord);


            InitializeComponent();
            Dictionary<String, String> comboBox1_RowSource2 = new Dictionary<string, string>();
            comboBox1_RowSource2.Add("Fordward", "Fordward");
            comboBox1_RowSource2.Add("Backward", "Backward");

            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource2, null);
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.*,b.description,c.description as colordesc,
            b.threadTex,b.category,b.Localsuppid,newcone - newconebook as newConevar,usedcone - usedconebook as usedConevar,b.threadtypeid,
            (b.Localsuppid+'-'+(Select name from LocalSupp d where b.localsuppid = d.id)) as supp
            from ThreadInventory_Detail a 
            left join Localitem b on a.refno = b.refno 
            left join threadcolor c on a.threadcolorid = c.id where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override bool OnGridSetup()
        {

            DataGridViewGeneratorTextColumnSettings refno = celllocalitem.GetGridCell("Thread");
            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings thlocation = new DataGridViewGeneratorTextColumnSettings();
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
                    CurrentDetailData["ThreadTex"] = refdr["ThreadTex"];
                    CurrentDetailData["LocalSuppid"] = refdr["LocalSuppid"];
                    CurrentDetailData["Supp"] = refdr["LocalSuppid"].ToString() + "-" + MyUtility.GetValue.Lookup("Name", refdr["LocalSuppid"].ToString(), "LocalSupp", "ID");
                    CurrentDetailData["NewCone"] = 0;
                    CurrentDetailData["UsedCone"] = 0;

                }
                else
                {
                    CurrentDetailData["Description"] = "";
                    CurrentDetailData["Refno"] = "";
                    CurrentDetailData["category"] = "";
                    CurrentDetailData["ThreadTex"] = 0;
                    CurrentDetailData["LocalSuppid"] = "";
                    CurrentDetailData["Supp"] = "";
                    CurrentDetailData["NewCone"] = 0;
                    CurrentDetailData["UsedCone"] = 0;
                }
                string sql = string.Format("Select newcone, usedcone from ThreadStock where refno ='{1}' and threadcolorid = '{0}' and threadlocationid = '{2}' and mDivisionid = '{3}'", CurrentDetailData["threadColorid"], newvalue, CurrentDetailData["threadlocationid"],keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    CurrentDetailData["newconebook"] = refdr["newcone"];
                    CurrentDetailData["usedconebook"] = refdr["usedcone"];
                    reqty();
                }
                else
                {
                    CurrentDetailData["newconebook"] = 0;
                    CurrentDetailData["usedconebook"] = 0;
                    reqty();
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
                string sql = string.Format("Select newcone,usedcone from ThreadStock where refno ='{0}' and threadcolorid = '{1}' and threadlocationid = '{2}' and mDivisionid = '{3}'", CurrentDetailData["Refno"], newvalue, CurrentDetailData["threadlocationid"],keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    CurrentDetailData["newconebook"] = refdr["newcone"];
                    CurrentDetailData["usedconebook"] = refdr["usedcone"];
                    reqty();
                }
                else
                {
                    CurrentDetailData["newconebook"] = 0;
                    CurrentDetailData["usedconebook"] = 0;
                    reqty();
                }
                CurrentDetailData.EndEdit();
            };
            #endregion
            #region location
            thlocation.CellValidating += (s, e) =>
                {
                    if (!this.EditMode ||e.RowIndex==-1) return;
                    string oldvalue = CurrentDetailData["threadlocationid"].ToString();
                    string newvalue = e.FormattedValue.ToString();
                    if (oldvalue == newvalue) return;
                    DataRow refdr;

                    string sql = string.Format("Select newcone,usedcone from ThreadStock where refno ='{0}' and threadcolorid = '{1}' and threadlocationid = '{2}' and mDivisionid = '{3}'", CurrentDetailData["Refno"], CurrentDetailData["threadcolorid"], newvalue,keyWord);
                    if (MyUtility.Check.Seek(sql, out refdr))
                    {
                        CurrentDetailData["newconebook"] = refdr["newcone"];
                        CurrentDetailData["usedconebook"] = refdr["usedcone"];
                        reqty();
                    }
                    else
                    {
                        CurrentDetailData["newconebook"] = 0;
                        CurrentDetailData["usedconebook"] = 0;
                        reqty();
                    }
                    if(MyUtility.Check.Seek(newvalue,"ThreadLocation","ID")) CurrentDetailData["ThreadLocationid"] = newvalue;
                    else CurrentDetailData["ThreadLocationid"] = "";
                    CurrentDetailData.EndEdit();
                };
            

            #endregion
            #region Qty
            qty.CellValidating += (s, e) =>
                {
                    if (!this.EditMode || e.RowIndex == -1) return;

                    if (detailgrid.Columns[e.ColumnIndex].HeaderText.ToString() == "New Cone") CurrentDetailData["newCone"] = e.FormattedValue;
                    if (detailgrid.Columns[e.ColumnIndex].HeaderText.ToString() == "Used Cone") CurrentDetailData["UsedCone"] = e.FormattedValue;
                    reqty();
                    CurrentDetailData.EndEdit();
                };
            #endregion
            #region setup Grid

            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20), settings: refno)
            .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(15), settings: thcolor)
            .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .CellThreadLocation("ThreadLocationid", header: "Location", width: Widths.AnsiChars(10), settings: thlocation)
            .Numeric("NewConebook", header: "New Cone\nperbooks", width: Widths.AnsiChars(5), integer_places: 3,iseditingreadonly:true)
            .Numeric("NewCone", header: "New Cone", width: Widths.AnsiChars(5), integer_places: 5, settings: qty)
            .Numeric("NewConeVar", header: "New Cone\nVariance", width: Widths.AnsiChars(5), integer_places: 5)

            .Numeric("UsedConebook", header: "Used cone\nperbooks", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: 
            true)
            .Numeric("UsedCone", header: "Used Cone", width: Widths.AnsiChars(5), integer_places: 5, settings: qty)
             .Numeric("UsedConeVar", header: "Used CSone\nVariance", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
            .Text("ThreadTypeid", header: "Thread Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("ThreadTex", header: "Tex", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
            .Text("category", header: "Category", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true);
            #endregion

            this.detailgrid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[2].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[4].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[6].DefaultCellStyle.BackColor = Color.Pink;
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
            CurrentMaintain["StockType"] = "Fordward";
            
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
                if (MyUtility.Check.Empty(dr["ThreadLocationid"]) )
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
                string id = MyUtility.GetValue.GetID(keyWord + "TV", "ThreadInventory", (DateTime)CurrentMaintain["cDate"]);

                if (string.IsNullOrWhiteSpace(id))
                {
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            #endregion

            return base.ClickSaveBefore();
        }
        protected override bool ClickNewBefore()
        {
            try
            {
                if (MyUtility.Check.Seek(string.Format("Select id from ThreadInventory where status = 'New' and mDivisionid ='{0}'", keyWord)))
                {
                    MyUtility.Msg.WarningBox("Some inventory not yet confirm.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowErr("Commit transaction error.", ex);
                return false;
            }
            return base.ClickNewBefore();
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
            bool linsertad = false;
            string id = MyUtility.GetValue.GetID(keyWord + "TA", "ThreadAdjust", DateTime.Now);
            string insertAd = string.Format("Insert Into ThreadAdjust(ID,cDate,Status,mDivisionid,ThreadInventoryid,AddName,AddDate) values('{0}',getDate(),'Confirmed','{1}','{2}','{3}',GETDate());", id, keyWord, CurrentMaintain["ID"], loginID);
            
            sql = String.Format("Update ThreadInventory set Status = 'Confirmed',editname='{0}', editdate = GETDATE() where id='{1}'", loginID, CurrentMaintain["ID"].ToString());

            foreach (DataRow dr in this.DetailDatas)
            {
                checksql = string.Format("Select * from ThreadStock where refno ='{0}' and threadLocationid = '{1}' and threadcolorid = '{2}' and mDivisionid = '{3}'", dr["refno"], dr["threadlocationid"], dr["threadcolorid"], keyWord);
                if (MyUtility.Check.Seek(checksql))
                {
                    insertSql = insertSql + string.Format("Update ThreadStock set NewCone = NewCone+({0}),UsedCone = UsedCone + ({1}) where refno='{2}' and mDivisionid = '{3}' and ThreadColorid = '{4}' and ThreadLocationid = '{5}';", (decimal)dr["NewCone"] - (decimal)dr["NewConeBook"], (decimal)dr["UsedCone"] - (decimal)dr["UsedConeBook"], dr["refno"], keyWord, dr["ThreadColorid"], dr["ThreadLocationid"]);
                }
                else
                {
                    insertSql = insertSql + string.Format("insert ThreadStock(refno,mDivisionid,threadcolorid,threadlocationid,newcone,usedcone,addName,AddDate) values('{0}','{1}','{2}','{3}',{4},{5},'{6}',GetDate())", dr["refno"],keyWord,dr["ThreadColorid"], dr["ThreadLocationid"],(decimal)dr["NewCone"] - (decimal)dr["NewConeBook"], (decimal)dr["UsedCone"] - (decimal)dr["UsedConeBook"],loginID );
                }
                if ((decimal)dr["NewCone"] - (decimal)dr["NewConeBook"] != 0 || (decimal)dr["UsedCone"] - (decimal)dr["UsedConeBook"] != 0)
                {
                    insertAd = insertAd + string.Format("Insert into ThreadAdjust_Detail(ID,Refno,ThreadColorid,ThreadLocationid,NewConeBook,NewCone,UsedConeBook,UsedCone) Values('{0}','{1}','{2}','{3}',{4},{5},{6},{7});", id, dr["refno"], dr["threadcolorid"], dr["ThreadLocationid"], dr["NewConeBook"], dr["NewCone"], dr["UsedConeBook"], dr["UsedCone"]);
                    sql = String.Format("Update ThreadInventory set Status = 'Confirmed',editname='{0}', editdate = GETDATE(),ThreadAdjustid = '{2}' where id='{1}'", loginID, CurrentMaintain["ID"].ToString(), id);
                    linsertad = true;
                }
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
                    if (linsertad)
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, insertAd)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(insertAd, upResult);
                            return;
                        }
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
 
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in DetailDatas)
            {
                dr["NewCone"] = dr["NewConeBook"];
                dr["UsedCone"] = dr["UsedConeBook"];
                dr["NewConevar"] = 0;
                dr["UsedConeVar"] = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //移到指定那筆
            string refno = textBox1.Text;
            int index = detailgridbs.Find("Refno", refno);
            if (index == -1)
            {
                int index2 = detailgridbs.Find("ThreadLocationid", refno);
                if (index2 == -1) MyUtility.Msg.WarningBox("Data was not found!!");
                else detailgridbs.Position = index2;
            }
            else
            { detailgridbs.Position = index; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable detTable = ((DataTable)this.detailgridbs.DataSource);
            Form P05_import = new Sci.Production.Thread.P05_Import(detTable);
            P05_import.ShowDialog();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue.ToString() == comboBox1.OldValue.ToString()) return;
            DialogResult diresult = MyUtility.Msg.QuestionBox("The detail grid will be cleared, are you sure change type?");
            if (diresult == DialogResult.No)
            {
                return;
            }
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }
        }
        private void reqty()
        {
            CurrentDetailData["newConeVar"] = (decimal)CurrentDetailData["newcone"] - (decimal)CurrentDetailData["newconebook"];
            CurrentDetailData["usedConeVar"] = (decimal)CurrentDetailData["usedcone"] - (decimal)CurrentDetailData["usedconebook"];
        }
    }
}
