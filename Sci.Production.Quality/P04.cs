using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.Tools;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Quality
{
    public partial class P04 : Win.Tems.Input6
    {
        private readonly string loginID = Env.User.UserID;
        private readonly string Factory = Env.User.Keyword;

        // 宣告Context Menu Item
        private ToolStripMenuItem edit;

        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionid='{0}'", this.Factory);

            // this.detailgrid.ContextMenuStrip = detailgridmenus;
            this.detailgrid.ContextMenuShowing += new EventHandler<ContextMenuShowingEventArgs>(this.Detailgrid_ContextMenuShowing);
        }

        /// <inheritdoc/>
        protected override DetailGridContextMenuMode CurrentDetailGridContextMenuMode()
        {
            if (!this.EditMode)
            {
                return DetailGridContextMenuMode.Editable;
            }

            return DetailGridContextMenuMode.None;
        }

        private void Detailgrid_ContextMenuShowing(object sender, ContextMenuShowingEventArgs e)
        {
            if (this.EditMode)
            {
                foreach (ToolStripItem m in this.detailgridmenus.Items)
                {
                    m.Visible = false;
                }
            }
            else
            {
                foreach (ToolStripItem m in this.detailgridmenus.Items)
                {
                    m.Visible = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.detailgridmenus.Items.Clear(); // 清空原有的Menu Item

            // Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Create New Test", onclick: (s, e) => CreateNewTest()).Get(out add);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Record's detail", onclick: (s, e) => this.EditThisDetail()).Get(out this.edit);

            // Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Delete this Record's detail", onclick: (s, e) => DeleteThisDetail()).Get(out delete);
            base.OnFormLoaded();
        }

        private void EditThisDetail()
        {
            if (this.EditMode)
            {
                return;
            }

            if (this.DetailDatas.Count == 0)
            {
                return;
            }

            P04_Detail callNewDetailForm = new P04_Detail(this.EditMode, this.CurrentMaintain, this.CurrentDetailData);
            callNewDetailForm.ShowDialog(this);
            callNewDetailForm.Dispose();
            this.RenewData();
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // this.detailgrid.AutoResizeColumns();
            DataTable dt;
            DualResult result;
            string cmd = "select * from dbo.GetSCI(@poid,'')";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@poid", this.displayFirstSP.Text));
            if (result = DBProxy.Current.Select(null, cmd, spam, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["MinSciDelivery"] == DBNull.Value)
                    {
                        this.dateEarliestSCIDlv.Text = string.Empty;
                    }
                    else
                    {
                        this.dateEarliestSCIDlv.Value = Convert.ToDateTime(dt.Rows[0]["MinSciDelivery"]);
                    }

                    if (dt.Rows[0]["MinBuyerDelivery"] == DBNull.Value)
                    {
                        this.dateEarliestBuyerDlv.Text = string.Empty;
                    }
                    else
                    {
                        this.dateEarliestBuyerDlv.Value = Convert.ToDateTime(dt.Rows[0]["MinBuyerDelivery"]);
                    }
                }
            }

            if (this.CurrentMaintain["Result"].ToString() == "P")
            {
                this.txtLastResult.Text = "Pass";
            }
            else
            {
                this.txtLastResult.Text = "Fail";
            }

            // [Last Test Date]
            if (this.CurrentMaintain["date"] == DBNull.Value)
            {
                this.dateLastTestDate.Text = string.Empty;
            }
            else
            {
                this.dateLastTestDate.Value = Convert.ToDateTime(this.CurrentMaintain["date"]);
            }

            // [Earliest Inline]
            if (this.CurrentMaintain["SewingInline"] == DBNull.Value)
            {
                this.dateEarliestInline.Text = string.Empty;
            }
            else
            {
                this.dateEarliestInline.Value = Convert.ToDateTime(this.CurrentMaintain["SewingInline"]);
            }

            // [Earliest Offline]
            if (this.CurrentMaintain["SewingOffLine"] == DBNull.Value)
            {
                this.dateEarliestOffline.Text = string.Empty;
            }
            else
            {
                this.dateEarliestOffline.Value = Convert.ToDateTime(this.CurrentMaintain["SewingOffLine"]);
            }

            // [DeadLine]
            if (this.CurrentMaintain["Deadline"] == DBNull.Value)
            {
                this.dateDeadLine.Text = string.Empty;
            }
            else
            {
                this.dateDeadLine.Value = Convert.ToDateTime(this.CurrentMaintain["Deadline"]);
            }

            DataTable datas = (DataTable)this.detailgridbs.DataSource;
        }

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            DataTable dt = (DataTable)e.Details;
            DataRow dr_showname;

            dt.Columns.Add("Send", typeof(string));
            dt.Columns.Add("Receive", typeof(string));
            dt.Columns.Add("NewKey", typeof(int));
            dt.Columns.Add("LastEditName", typeof(string));
            dt.Columns.Add("Showname", typeof(string));
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", dt.Rows[i]["Inspector"]), out dr_showname))
                {
                    dr["Showname"] = dr_showname["Name_Extno"];
                }

                dr["NewKey"] = i;
                dr["Send"] = string.Empty;
                dr["Receive"] = string.Empty;
                dr["AddName"] = dt.Rows[i]["AddName"].ToString() + " - " + ((DateTime)MyUtility.Convert.GetDate(dt.Rows[i]["AddDATE"])).ToString("yyyy/MM/dd HH:mm:ss");
                if (!MyUtility.Check.Empty(dt.Rows[i]["EditName"]))
                {
                    dr["LastEditName"] = dt.Rows[i]["EditName"].ToString() + " - " + ((DateTime)MyUtility.Convert.GetDate(dt.Rows[i]["EditDate"])).ToString("yyyy/MM/dd HH:mm:ss");
                }

                i++;
            }

            return base.OnRenewDataDetailPost(e);
        }

        private void BtnReceive(object sender, EventArgs e)
        {
            if (this.EditMode == true)
            {
                return;
            }

            this.CurrentDetailData["Receiver"] = this.loginID;
            this.CurrentDetailData["ReceiveDate"] = DateTime.Now.ToShortDateString();
            DualResult result;
            string sqlcmd = string.Format(@"update Garmenttest_Detail set Receiver ='{0}',ReceiveDate='{1}' where id='{2}' ", this.loginID, DateTime.Now.ToShortDateString(), this.CurrentMaintain["ID"]);
            result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("ErrorMsg: " + result);
                return;
            }
        }

        private void BtnSend(object sender, EventArgs e)
        {
            if (this.EditMode == true)
            {
                return;
            }

            this.CurrentDetailData["Sender"] = this.loginID;
            this.CurrentDetailData["SendDate"] = DateTime.Now.ToShortDateString();
            DualResult result;
            string sqlcmd = string.Format(@"update Garmenttest_Detail set Sender ='{0}',SendDate='{1}' where id='{2}' ", this.loginID, DateTime.Now.ToShortDateString(), this.CurrentMaintain["ID"]);
            result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("ErrorMsg: " + result);
                return;
            }

            this.Send_Mail();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorDateColumnSettings inspDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings inspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings commentsCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings sendCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings senderCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings receiveCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings receiverCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings resultValid = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings resultComboCell = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings mtlTypeIDComboCell = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings sizeCell = new DataGridViewGeneratorTextColumnSettings();

            Dictionary<string, string> resultCombo = new Dictionary<string, string>();
            resultCombo.Add("P", "Pass");
            resultCombo.Add("F", "Fail");
            resultComboCell.DataSource = new BindingSource(resultCombo, null);
            resultComboCell.ValueMember = "Key";
            resultComboCell.DisplayMember = "Value";

            Dictionary<string, string> mtlTypeCombo = new Dictionary<string, string>();
            mtlTypeCombo.Add(string.Empty, string.Empty);
            mtlTypeCombo.Add("KNIT", "KNIT");
            mtlTypeCombo.Add("WOVEN", "WOVEN");
            mtlTypeIDComboCell.DataSource = new BindingSource(mtlTypeCombo, null);
            mtlTypeIDComboCell.ValueMember = "Key";
            mtlTypeIDComboCell.DisplayMember = "Value";

            mtlTypeIDComboCell.CellEditable += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["MtlTypeID"]))
                {
                    e.IsEditable = true;
                }
                else
                {
                    e.IsEditable = false;
                }
            };

            #region inspDateCell
            inspDateCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                dr["EditName"] = this.loginID;
                dr["EditDate"] = DateTime.Now.ToShortDateString();
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["inspdate"] = e.FormattedValue;
                }
            };
            #endregion

            #region inspectorCell

            inspectorCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr_showname;
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    string scalecmd = @"select id,name from Pass1 WITH (NOLOCK) where Resign is null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Inspector"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Inspector"] = item1.GetSelectedString(); // 將選取selectitem value帶入GridView
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", item1.GetSelectedString()), out dr_showname))
                    {
                        dr["Showname"] = dr_showname["Name_Extno"];
                    }
                }
            };
            inspectorCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr_showname;
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    string scalecmd = @"select id,name from Pass1 WITH (NOLOCK) where Resign is null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Inspector"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Inspector"] = item1.GetSelectedString(); // 將選取selectitem value帶入GridView
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", item1.GetSelectedString()), out dr_showname))
                    {
                        dr["Showname"] = dr_showname["Name_Extno"];
                    }
                }
            };
            inspectorCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return; // 沒資料 return
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                DataRow dr_cmd;
                DataRow dr_showname;
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["inspector"] = string.Empty;
                    dr["Showname"] = string.Empty;
                    return; // 沒資料 return
                }

                string cmd = string.Format(@"select * from pass1 WITH (NOLOCK) where id='{0}' and Resign is null", e.FormattedValue);

                if (MyUtility.Check.Seek(cmd, out dr_cmd))
                {
                    dr["EditName"] = this.loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    dr["inspector"] = e.FormattedValue;
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", e.FormattedValue), out dr_showname))
                    {
                        dr["Showname"] = dr_showname["Name_Extno"];
                    }
                }
                else
                {
                    dr["EditName"] = this.loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    dr["inspector"] = string.Empty;
                    dr["Showname"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Inspector: {0}> not found!!!", e.FormattedValue));
                    return;
                }

                this.CurrentDetailData.EndEdit();
                this.Update_detailgrid_CellValidated(e.RowIndex);
            };
            #endregion

            #region CommentsCell
            commentsCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Remark"]))
                {
                    dr["EditName"] = this.loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Remark"] = e.FormattedValue;
                    }
                }
            };
            #endregion

            #region SendCell
            sendCell.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode == true && MyUtility.Check.Empty(dr["SendDate"]))
                {
                    e.IsEditable = false;
                }
            };
            sendCell.EditingMouseClick += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Send"]))
                {
                    dr["Send"] = this.loginID;
                    dr["SendDate"] = DateTime.Now;
                    dr["EditName"] = this.loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                }
            };
            #endregion

            #region SenderCell
            #endregion

            #region ReceiveCell
            receiveCell.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode == true && MyUtility.Check.Empty(dr["ReceiveDate"]))
                {
                    e.IsEditable = false;
                }
            };

            receiveCell.CellMouseClick += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Receive"]))
                {
                    dr["Receive"] = this.loginID;
                    dr["ReceiveDate"] = DateTime.Now;
                    dr["EditName"] = this.loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                }
            };
            #endregion

            #region ReceiverCell
            #endregion

            #region ResultValid
            resultValid.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode == true && (MyUtility.Check.Empty(dr["SendDate"]) || MyUtility.Check.Empty(dr["ReceiveDate"])))
                {
                    e.IsEditable = false;
                }
            };
            resultValid.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Result"]))
                {
                    dr["EditName"] = this.loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Result"] = e.FormattedValue;
                    }
                }
            };

            #endregion

            #region SizeComboCell
            sizeCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string sql = $@"select distinct(sizecode) from Order_Qty where id = '{this.CurrentMaintain["orderid"]}'";

                // DataTable sizecodedt;
                // DualResult sizecoderesult = DBProxy.Current.Select(null, sql, out sizecodedt);
                SelectItem item = new SelectItem(sql, "10,10", dr["SizeCode"].ToString());
                DialogResult dresult = item.ShowDialog();
                if (dresult == DialogResult.Cancel)
                {
                    return;
                }

                dr["SizeCode"] = item.GetSelectedString();
                dr.EndEdit();
            };

            sizeCell.CellValidating += (s, e) =>
             {
                 if (e.RowIndex == -1)
                 {
                     return;
                 }

                 if (!this.EditMode)
                 {
                     return;
                 }

                 DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                 string sql = $@"select sizecode from Order_Qty where id = '{this.CurrentMaintain["orderid"]}' and sizecode =  '{e.FormattedValue}' ";
                 string sizecode = MyUtility.GetValue.Lookup(sql);
                 if (MyUtility.Check.Empty(sizecode))
                 {
                     dr["SizeCode"] = string.Empty;
                 }
                 else
                 {
                     dr["SizeCode"] = sizecode;
                 }

                 dr.EndEdit();
             };

            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("No", header: "No. Of Test", integer_places: 8, decimal_places: 0, iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(6), settings: sizeCell)
            .Date("Inspdate", header: "Test Date", width: Widths.AnsiChars(10), settings: inspDateCell)
            .ComboBox("MtlTypeID", header: "Material" + Environment.NewLine + "Type", width: Widths.AnsiChars(10), settings: mtlTypeIDComboCell)
            .ComboBox("Result", header: "Result", width: Widths.AnsiChars(10), settings: resultComboCell) // .Get(out ResultComboCell)
            .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(18), settings: inspectorCell)
            .Text("Showname", header: "Inspector Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Remark", header: "Comments", width: Widths.AnsiChars(10), settings: commentsCell)
            .Button("Send", null, header: "Send", width: Widths.AnsiChars(5), onclick: this.BtnSend)
            .Text("Sender", header: "Sender", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("SendDate", header: "Send Date", width: Widths.AnsiChars(10), iseditingreadonly: true)

            // 將Receive換成button,按Receive之後將登入帳號填入Receiver、Receive填入今天的日期 20161020
            .Button("Receive", null, header: "Receive", width: Widths.AnsiChars(5), onclick: this.BtnReceive)
            .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Date("ReceiveDate", header: "Receive Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("AddName", header: "Add Name", width: Widths.AnsiChars(25), iseditingreadonly: true) // addName + addDate
            .Text("LastEditName", header: "Last Edit Name", width: Widths.AnsiChars(25), iseditingreadonly: true); // editName + editDate
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.CurrentMaintain["No"] = (int)this.CurrentMaintain["No"] + 1;
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = 1)
        {
            base.OnDetailGridInsert(index);
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            int maxNo;
            if (dt.Rows.Count == 0)
            {
                maxNo = 0;
                base.OnDetailGridInsert(0);
                this.CurrentDetailData["No"] = maxNo + 1;
            }
            else
            {
                maxNo = Convert.ToInt32(dt.Compute("Max(No)", string.Empty));
                this.CurrentDetailData["No"] = maxNo + 1;
            }
        }

        // delete前檢查

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = this.grid.GetDataRow<DataRow>(this.grid.GetSelectedRowIndex());
            if (!MyUtility.Check.Empty(dr["SendDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("SendDate is existed, can not delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // Edit 前檢查

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.txtSP.Text == string.Empty || MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("SP# cannot be empty !! ");
                return false;
            }

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Check.Empty(dr["MtlTypeID"]))
                    {
                        MyUtility.Msg.WarningBox("Material Type cannot be empty !! ");
                        return false;
                    }
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            DualResult upResult = new DualResult(true);
            string update_cmd = string.Empty;
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    if (!MyUtility.Check.Empty(dr["senddate", DataRowVersion.Original]))
                    {
                        return new DualResult(false, "SendDate is existed, can not delete.", "Warning");
                    }

                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From GarmentTest_Detail WITH (NOLOCK) Where id =@id and no=@no";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@no", dr["NO", DataRowVersion.Original]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                }
            }

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    string delete3sub = $@"
Delete GarmentTest_Detail_Shrinkage  where id = '{this.CurrentMaintain["ID", DataRowVersion.Original]}' and NO = '{dr["NO", DataRowVersion.Original]}'
Delete GarmentTest_Detail_Twisting where id = '{this.CurrentMaintain["ID", DataRowVersion.Original]}' and NO = '{dr["NO", DataRowVersion.Original]}'
Delete GarmentTest_Detail_Apperance where id = '{this.CurrentMaintain["ID", DataRowVersion.Original]}' and NO = '{dr["NO", DataRowVersion.Original]}'
";
                    DBProxy.Current.Execute(null, delete3sub);
                }
                else
                {
                    if (MyUtility.Check.Empty(dr["Status"]))
                    {
                        dr["Status"] = "New";
                    }

                    if (!MyUtility.Check.Seek($"select 1 from GarmentTest_Detail_Shrinkage with(nolock) where id = '{this.CurrentMaintain["ID"]}' and NO = '{dr["NO"]}'"))
                    {
                        List<SqlParameter> spam = new List<SqlParameter>();
                        spam.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"]));
                        spam.Add(new SqlParameter("@NO", dr["NO"]));
                        string insertShrinkage = $@"
select sl.Location
into #Location1
from GarmentTest gt with(nolock)
inner join style s with(nolock) on s.id = gt.StyleID
inner join Style_Location sl with(nolock) on sl.styleukey = s.ukey
where gt.id = @ID and sl.Location !='B'
group by sl.Location
order by sl.Location desc
CREATE TABLE #type1([type] [varchar](20),seq numeric(6,0))
insert into #type1 values('Chest Width',1)
insert into #type1 values('Sleeve Width',2)
insert into #type1 values('Sleeve Length',3)
insert into #type1 values('Back Length',4)
insert into #type1 values('Hem Opening',5)
---
select distinct sl.Location
into #Location2
from GarmentTest gt with(nolock)
inner join style s with(nolock) on s.id = gt.StyleID
inner join Style_Location sl with(nolock) on sl.styleukey = s.ukey
where gt.id = @ID and sl.Location ='B'
CREATE TABLE #type2([type] [varchar](20),seq numeric(6,0))
insert into #type2 values('Waistband (relax)',1)
insert into #type2 values('Hip Width',2)
insert into #type2 values('Thigh Width',3)
insert into #type2 values('Side Seam',4)
insert into #type2 values('Leg Opening',5)
INSERT INTO [dbo].[GarmentTest_Detail_Shrinkage]([ID],[No],[Location],[Type],[seq])
select @ID,@NO,* from #Location1,#type1
INSERT INTO [dbo].[GarmentTest_Detail_Shrinkage]([ID],[No],[Location],[Type],[seq])
select @ID,@NO,* from #Location2,#type2

INSERT INTO [dbo].[GarmentTest_Detail_Twisting]([ID],[No],[Location])
select @ID,@NO,* from #Location1
INSERT INTO [dbo].[GarmentTest_Detail_Twisting]([ID],[No],[Location])
select @ID,@NO,* from #Location2

INSERT INTO [dbo].[GarmentTest_Detail_Apperance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Printing / Heat Transfer',1)
INSERT INTO [dbo].[GarmentTest_Detail_Apperance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Label',2)
INSERT INTO [dbo].[GarmentTest_Detail_Apperance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Zipper / Snap Button / Button / Tie Cord',3)
INSERT INTO [dbo].[GarmentTest_Detail_Apperance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Discoloration (colour change )',4)
INSERT INTO [dbo].[GarmentTest_Detail_Apperance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Colour Staining',5)
INSERT INTO [dbo].[GarmentTest_Detail_Apperance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Pilling',6)
INSERT INTO [dbo].[GarmentTest_Detail_Apperance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Shrinkage & Twisting',7)
INSERT INTO [dbo].[GarmentTest_Detail_Apperance]([ID],[No],[Type],[Seq])
values (@ID,@NO,'Appearance of garment after wash',8)
";
                        DBProxy.Current.Execute(null, insertShrinkage, spam);
                    }

                    #region 寫入GarmentTest_Detail_FGWT

                    // 取Location
                    List<string> locations = MyUtility.GetValue.Lookup($@"

SELECT STUFF(
	(select DISTINCT ',' + sl.Location
	from Style s
	INNER JOIN Style_Location sl ON s.Ukey = sl.StyleUkey 
	where s.id='{this.CurrentMaintain["StyleID"]}' AND s.BrandID='{this.CurrentMaintain["BrandID"]}' AND s.SeasonID='{this.CurrentMaintain["SeasonID"]}'
	FOR XML PATH('')
	) 
,1,1,'')").Split(',').ToList();

                    List<FGWT> fGWTs = new List<FGWT>();

                    string mtlTypeID = MyUtility.Convert.GetString(dr["MtlTypeID"]);
                    bool containsT = locations.Contains("T");
                    bool containsB = locations.Contains("B");

                    // 若只有B則寫入Bottom的項目+ALL的項目，若只有T則寫入TOP的項目+ALL的項目，若有B和T則寫入Top+ Bottom的項目+ALL的項目
                    if (containsT && containsB)
                    {
                        fGWTs = GetDefaultFGWT_Old(false, false, true, mtlTypeID);
                    }
                    else if (containsT)
                    {
                        fGWTs = GetDefaultFGWT_Old(containsT, false, false, mtlTypeID);
                    }
                    else
                    {
                        fGWTs = GetDefaultFGWT_Old(false, containsB, false, mtlTypeID);
                    }

                    string garmentTest_Detail_ID = MyUtility.Convert.GetString(dr["ID"]);
                    string garmentTest_Detail_No = MyUtility.Convert.GetString(dr["NO"]);

                    StringBuilder insertCmd = new StringBuilder();
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    int idx = 0;

                    // 組合INSERT SQL
                    foreach (var fGWT in fGWTs)
                    {
                        string location = string.Empty;

                        switch (fGWT.Location)
                        {
                            case "Top":
                                location = "T";
                                break;
                            case "Bottom":
                                location = "B";
                                break;
                            case "Top+Bottom":
                                location = "S";
                                break;
                            default:
                                break;
                        }

                        if (fGWT.Scale == null)
                        {
                            insertCmd.Append($@"

INSERT INTO GarmentTest_Detail_FGWT
           (ID, No, Location, Type ,TestDetail ,Criteria )
     VALUES
           ( {garmentTest_Detail_ID}
           , {garmentTest_Detail_No}
           , @Location{idx}
           , @Type{idx}
           , @TestDetail{idx}
           , @Criteria{idx} )

");
                        }
                        else
                        {
                            insertCmd.Append($@"

INSERT INTO GarmentTest_Detail_FGWT
           (ID, No, Location, Type ,Scale,TestDetail)
     VALUES
           ( {garmentTest_Detail_ID}
           , {garmentTest_Detail_No}
           , @Location{idx}
           , @Type{idx}
           , ''
           , @TestDetail{idx})

");
                        }

                        parameters.Add(new SqlParameter($"@Location{idx}", location));
                        parameters.Add(new SqlParameter($"@Type{idx}", fGWT.Type));
                        parameters.Add(new SqlParameter($"@TestDetail{idx}", fGWT.TestDetail));
                        parameters.Add(new SqlParameter($"@Criteria{idx}", fGWT.Criteria));
                        idx++;
                    }

                    // 找不到才Insert
                    /*
                    if (!MyUtility.Check.Seek($"SELECT 1 FROM GarmentTest_Detail_FGWT WHERE ID ='{garmentTest_Detail_ID}' AND NO='{garmentTest_Detail_No}'"))
                    {
                        DualResult r = DBProxy.Current.Execute(null, insertCmd.ToString(), parameters);
                        if (!r)
                        {
                            this.ShowErr(r);
                        }
                    }
                    */
                    #endregion

                    #region 寫入GarmentTest_Detail_FGPT

                    List<FGPT> fGPTs = new List<FGPT>();

                    bool isRugbyFootBall = MyUtility.Check.Seek($@"select 1 from Style s where s.id='{this.CurrentMaintain["StyleID"]}' AND s.BrandID='{this.CurrentMaintain["BrandID"]}' AND s.SeasonID='{this.CurrentMaintain["SeasonID"]}' AND s.ProgramID like '%FootBall%'");
                    bool isLining = MyUtility.Check.Seek($@"select 1 from Style s where s.id='{this.CurrentMaintain["StyleID"]}' AND s.BrandID='{this.CurrentMaintain["BrandID"]}' AND s.SeasonID='{this.CurrentMaintain["SeasonID"]}' AND s.Description  like '%with lining%' ");

                    // 若只有B則寫入Bottom的項目+ALL的項目，若只有T則寫入TOP的項目+ALL的項目，若有B和T則寫入Top+ Bottom的項目+ALL的項目
                    if (containsT && containsB)
                    {
                        fGPTs = GetDefaultFGPT(false, false, true, isRugbyFootBall, isLining, "S");
                    }
                    else if (containsT)
                    {
                        fGPTs = GetDefaultFGPT(containsT, false, false, isRugbyFootBall, isLining, "T");
                    }
                    else
                    {
                        fGPTs = GetDefaultFGPT(false, containsB, false, isRugbyFootBall, isLining, "B");
                    }

                    insertCmd.Clear();
                    parameters.Clear();
                    idx = 0;

                    foreach (var fGPT in fGPTs)
                    {
                        string location = string.Empty;

                        switch (fGPT.Location)
                        {
                            case "Top":
                                location = "T";
                                break;
                            case "Bottom":
                                location = "B";
                                break;
                            case "Full": // Top+Bottom = Full
                                location = "S";
                                break;
                            default:
                                location = fGPT.Location;
                                break;
                        }

                        insertCmd.Append($@"

INSERT INTO GarmentTest_Detail_FGPT
           (ID,No,Location,Type,TestDetail,TestUnit,Criteria,TestName)
     VALUES
           ( {garmentTest_Detail_ID}
           , {garmentTest_Detail_No}
           , @Location{idx}
           , @Type{idx}
           , @TestDetail{idx}
           , @TestUnit{idx}
           , @Criteria{idx}  
           , @TestName{idx})

");
                        parameters.Add(new SqlParameter($"@Location{idx}", location));
                        parameters.Add(new SqlParameter($"@Type{idx}", fGPT.Type));
                        parameters.Add(new SqlParameter($"@TestDetail{idx}", fGPT.TestDetail));
                        parameters.Add(new SqlParameter($"@TestUnit{idx}", fGPT.TestUnit));
                        parameters.Add(new SqlParameter($"@Criteria{idx}", fGPT.Criteria));
                        parameters.Add(new SqlParameter($"@TestName{idx}", fGPT.TestName));
                        idx++;
                    }

                    // 找不到才Insert
                    if (!MyUtility.Check.Seek($"SELECT 1 FROM GarmentTest_Detail_FGPT WHERE ID ='{garmentTest_Detail_ID}' AND NO='{garmentTest_Detail_No}'"))
                    {
                        DualResult r = DBProxy.Current.Execute(null, insertCmd.ToString(), parameters);
                        if (!r)
                        {
                            this.ShowErr(r);
                        }
                    }
                    #endregion
                }
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            if (dt.Rows.Count > 0)
            {
                string maxNo = dt.Compute("MAX(NO)", string.Empty).ToString();
                string where = string.Format("NO='{0}'", maxNo);
                DataRow detailRow = dt.Select(where)[0];

                this.CurrentMaintain["Result"] = detailRow["Result"];
                this.CurrentMaintain["Date"] = detailRow["inspdate"];
                this.CurrentMaintain["Remark"] = detailRow["remark"];
            }
            else
            {
                this.CurrentMaintain["Result"] = string.Empty;
                this.CurrentMaintain["Date"] = DBNull.Value;
                this.CurrentMaintain["Remark"] = string.Empty;
            }

            return base.ClickSave();
        }

        private void TxtSP_Validated(object sender, EventArgs e)
        {
            DataTable dt;
            DualResult result;
            string cmd = @"select b.* from Orders a WITH (NOLOCK) 
left join GarmentTest b WITH (NOLOCK) on a.ID=b.OrderID and a.StyleID=b.StyleID and a.SeasonID=b.SeasonID and a.BrandID=b.BrandID and a.FactoryID=b.MDivisionid
left join Order_Qty c WITH (NOLOCK) on a.ID=c.ID and c.Article=b.Article where a.id=@orderID";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@orderID", this.txtSP.Text));
            if (result = DBProxy.Current.Select(null, cmd, spam, out dt))
            {
                if (dt.Rows.Count <= 0)
                {
                    this.txtSP.Text = string.Empty;
                    this.txtSP.Focus();
                    MyUtility.Msg.WarningBox(string.Format("<The OrderID: {0}> is not verify", this.txtSP.Text));
                    return;
                }
            }
        }

        private void Update_detailgrid_CellValidated(int rowIndex)
        {
            this.detailgrid.InvalidateRow(rowIndex);
        }

        private void Send_Mail()
        {
            string mailto = string.Empty;
            string mailcc = string.Empty;
            string subject = "Garment Test - Style #:" + this.displayStyle.Text + ", Season :" + this.displaySeason.Text;
            string content = "Garment Test - Style #:" + this.displayStyle.Text + ", Season :" + this.displaySeason.Text + " had been sent, please receive and confirm";
            var email = new MailTo(Env.Cfg.MailFrom, mailto, mailcc, subject, null, content.ToString(), false, true);
            email.ShowDialog(this);
        }
    }
}
