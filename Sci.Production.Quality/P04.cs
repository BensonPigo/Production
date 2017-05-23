﻿using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Data.SqlClient;

namespace Sci.Production.Quality
{
    public partial class P04 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string Factory = Sci.Env.User.Keyword;

          
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {                
            InitializeComponent();         
            DefaultFilter = string.Format("MDivisionid='{0}'",Factory);                     
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.detailgrid.AutoResizeColumns();          
            DataTable dt ;
            DualResult result;
            string cmd = "select * from dbo.GetSCI(@poid,'')";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@poid", this.displayFirstSP.Text));
            if (result = DBProxy.Current.Select(null, cmd, spam, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["MinSciDelivery"] == DBNull.Value) dateEarliestSCIDlv.Text = "";
                    else dateEarliestSCIDlv.Value = Convert.ToDateTime(dt.Rows[0]["MinSciDelivery"]);

                    if (dt.Rows[0]["MinBuyerDelivery"] == DBNull.Value) dateEarliestBuyerDlv.Text = "";
                    else dateEarliestBuyerDlv.Value = Convert.ToDateTime(dt.Rows[0]["MinBuyerDelivery"]);
                }
            }
            if (CurrentMaintain["Result"].ToString()=="P")
            {
                 txtLastResult.Text = "Pass";
            }
            else
            {
                txtLastResult.Text = "Fail";
            }
           
            
            //[Last Test Date]
            if (CurrentMaintain["date"] == DBNull.Value) dateLastTestDate.Text = "";
            else dateLastTestDate.Value = Convert.ToDateTime(CurrentMaintain["date"]);

            //[Earliest Inline]
            if (CurrentMaintain["SewingInline"] == DBNull.Value) dateEarliestInline.Text = "";
            else dateEarliestInline.Value = Convert.ToDateTime(CurrentMaintain["SewingInline"]);

            //[Earliest Offline]
            if (CurrentMaintain["SewingOffLine"] == DBNull.Value) dateEarliestOffline.Text = "";
            else dateEarliestOffline.Value = Convert.ToDateTime(CurrentMaintain["SewingOffLine"]);

            //[DeadLine]
            if (CurrentMaintain["Deadline"] == DBNull.Value) dateDeadLine.Text = "";
            else dateDeadLine.Value = Convert.ToDateTime(CurrentMaintain["Deadline"]);

            DataTable datas = (DataTable)detailgridbs.DataSource;
        }

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
                dr["Send"] = "";
                dr["Receive"] = "";
                dr["AddName"] = dt.Rows[i]["AddName"].ToString() + " - " + dt.Rows[i]["AddDate"].ToString();
                dr["LastEditName"] = dt.Rows[i]["EditName"].ToString() + " - " + dt.Rows[i]["EditDate"].ToString();
                i++;
            }
            return base.OnRenewDataDetailPost(e);
        }

        private void btnReceive(object sender, EventArgs e)
        {
            if (this.EditMode==true)
            {
                return;
            }
            CurrentDetailData["Receiver"] = loginID;
            CurrentDetailData["ReceiveDate"] = DateTime.Now.ToShortDateString();
            DualResult result;
            string sqlcmd = string.Format(@"update Garmenttest_Detail set Receiver ='{0}',ReceiveDate='{1}' where id='{2}' ", loginID, DateTime.Now.ToShortDateString(), CurrentMaintain["ID"]);
            result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("ErrorMsg: " + result);
                return;
            }

        }

        private void btnSend(object sender, EventArgs e)
        {
            if (this.EditMode==true)
            {                
                return;
            }
            CurrentDetailData["Sender"] = loginID;
            CurrentDetailData["SendDate"] = DateTime.Now.ToShortDateString();
            DualResult result;
            string sqlcmd = string.Format(@"update Garmenttest_Detail set Sender ='{0}',SendDate='{1}' where id='{2}' ", loginID,DateTime.Now.ToShortDateString(),CurrentMaintain["ID"]);
            result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result) {
                MyUtility.Msg.WarningBox("ErrorMsg: "+result);
                return;  }

            Send_Mail();
        }

        protected override void OnDetailGridSetup()
        {
            
            
            DataGridViewGeneratorDateColumnSettings inspDateCell = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings inspectorCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings CommentsCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings SendCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings SenderCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ReceiveCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ReceiverCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings ResultValid = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings ResultComboCell = new DataGridViewGeneratorComboBoxColumnSettings();          
            
            

            Dictionary<string, string> ResultCombo = new Dictionary<string, string>();
            ResultCombo.Add("P", "Pass");
            ResultCombo.Add("F", "Fail");
            ResultComboCell.DataSource = new BindingSource(ResultCombo, null);
            ResultComboCell.ValueMember = "Key";
            ResultComboCell.DisplayMember = "Value";
      

            #region inspDateCell
            inspDateCell.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);

                dr["EditName"] = loginID;
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
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr_showname;
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    string scalecmd = @"select id,name from Pass1 WITH (NOLOCK) where Resign is null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Inspector"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Inspector"] = item1.GetSelectedString(); //將選取selectitem value帶入GridView
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", item1.GetSelectedString()), out dr_showname))
                    {
                        dr["Showname"] = dr_showname["Name_Extno"];
                    }
                }
            };
            inspectorCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr_showname;
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    string scalecmd = @"select id,name from Pass1 WITH (NOLOCK) where Resign is null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Inspector"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Inspector"] = item1.GetSelectedString(); //將選取selectitem value帶入GridView                    
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", item1.GetSelectedString()), out dr_showname))
                    {
                        dr["Showname"] = dr_showname["Name_Extno"];
                    }
                }
            };
            inspectorCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue)) return; // 沒資料 return               

                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                DataRow dr_cmd;
                DataRow dr_showname;
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["inspector"] = "";
                    dr["Showname"] = "";
                    return; // 沒資料 return
                }
                string cmd = string.Format(@"select * from pass1 WITH (NOLOCK) where id='{0}' and Resign is null", e.FormattedValue);                    
                    
                if (MyUtility.Check.Seek(cmd,out dr_cmd))
                {
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    dr["inspector"] = e.FormattedValue;
                    if (MyUtility.Check.Seek(string.Format(@"select * from view_ShowName where id ='{0}'", e.FormattedValue), out dr_showname))
                    {
                        dr["Showname"] = dr_showname["Name_Extno"];
                    }   
                }
                else
                {
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    dr["inspector"] = "";
                    dr["Showname"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Inspector: {0}> not found!!!", e.FormattedValue));
                    return;
                }                
                CurrentDetailData.EndEdit();
                this.update_detailgrid_CellValidated(e.RowIndex);
            };
            #endregion

            #region CommentsCell
            CommentsCell.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Remark"]))
                {
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Remark"] = e.FormattedValue;
                    }
                }
            };
            #endregion

            #region SendCell
            SendCell.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode == true && MyUtility.Check.Empty(dr["SendDate"])) e.IsEditable = false;
            };
            SendCell.EditingMouseClick += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Send"]))
                {
                    dr["Send"] = loginID;
                    dr["SendDate"] = DateTime.Now;
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                }
            };
            #endregion

            #region SenderCell
            #endregion

            #region ReceiveCell
            ReceiveCell.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode == true && MyUtility.Check.Empty(dr["ReceiveDate"])) e.IsEditable = false;
            };

            ReceiveCell.CellMouseClick += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Receive"]))
                {
                    dr["Receive"] = loginID;
                    dr["ReceiveDate"] = DateTime.Now;
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                }
            };
            #endregion

            #region ReceiverCell
            #endregion

            #region ResultValid
            ResultValid.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode == true && (MyUtility.Check.Empty(dr["SendDate"]) || MyUtility.Check.Empty(dr["ReceiveDate"]))) e.IsEditable = false;
            };
            ResultValid.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Result"]))
                {
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Result"] = e.FormattedValue;
                    }
                }
            };
           
            #endregion

            #region ResultComboCell
            //ResultComboCell.CellEditable += (s, e) =>
            //{
            //    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
            //    if (this.EditMode == true && (MyUtility.Check.Empty(dr["SendDate"]) || MyUtility.Check.Empty(dr["ReceiveDate"]))) e.IsEditable = false;
            //};
            
            #endregion


            

           // Ict.Win.UI.DataGridViewComboBoxColumn ResultComboCell;// 一定要加Ict.Win.UI 不然會跟C#原生的有所衝突
            
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("No", header: "No. Of Test", integer_places: 8, decimal_places: 0, iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Date("Inspdate", header: "Test Date", width: Widths.AnsiChars(10), settings: inspDateCell)
            .ComboBox("Result", header: "Result", width: Widths.AnsiChars(10), settings: ResultComboCell)//.Get(out ResultComboCell)
            .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10),settings:inspectorCell)
            .Text("Showname", header: "Inspector Name", width: Widths.AnsiChars(20),iseditingreadonly:true)
            .Text("Remark", header: "Comments", width: Widths.AnsiChars(10),settings:CommentsCell)
            .Button("Send", null, header: "Send", width: Widths.AnsiChars(5), onclick: btnSend)            
            .Text("Sender", header: "Sender", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("SendDate", header: "Send Date", width: Widths.AnsiChars(10),iseditingreadonly:true)
             //將Receive換成button,按Receive之後將登入帳號填入Receiver、Receive填入今天的日期 20161020
            .Button("Receive", null, header: "Receive", width: Widths.AnsiChars(5), onclick: btnReceive)            
            .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(5),iseditingreadonly:true)
            .Date("ReceiveDate", header: "Receive Date", width: Widths.AnsiChars(10),iseditingreadonly:true)
            .Text("AddName", header: "Add Name", width: Widths.AnsiChars(25),iseditingreadonly:true)// addName + addDate
            .Text("LastEditName", header: "Last Edit Name", width: Widths.AnsiChars(25),iseditingreadonly:true);//editName + editDate
            
            
        }
        protected override void ClickNewAfter()
        {
            CurrentMaintain["No"] = (int)CurrentMaintain["No"]+1;
            base.ClickNewAfter();
        }

        protected override void OnDetailGridInsert(int index = 1)
        {
            base.OnDetailGridInsert(index);
            DataTable dt = (DataTable)detailgridbs.DataSource;
           
            int MaxNo;
            if (dt.Rows.Count == 0)
            {
                MaxNo = 0;
                base.OnDetailGridInsert(0);
                CurrentDetailData["No"] = MaxNo + 1;
            }
            else
            {                
                MaxNo = Convert.ToInt32(dt.Compute("Max(No)", ""));
                CurrentDetailData["No"] = MaxNo+1 ;
            }
            
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (!MyUtility.Check.Empty(dr["SendDate"].ToString()))
            {
                MyUtility.Msg.WarningBox("SendDate is existed, can not delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        //Edit 前檢查
        protected override bool ClickSaveBefore()
        {
            if (this.txtSP.Text=="" || MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("SP# cannot be empty !! ");
                return false;
            }
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSave()
        {

            DualResult upResult = new DualResult(true);
            bool DELETE = false;
            string update_cmd = ""; 
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {               
                if (dr.RowState == DataRowState.Deleted)
                {
                    DELETE = true;

                    if (!MyUtility.Check.Empty(dr["senddate", DataRowVersion.Original])) return new DualResult(false, "SendDate is existed, can not delete.", "Warning");

                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From GarmentTest_Detail WITH (NOLOCK) Where id =@id and no=@no";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@no", dr["NO", DataRowVersion.Original]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    continue;
                }
             
            }
                

            DataTable dt = (DataTable)detailgridbs.DataSource;
            if (DELETE) dt.AcceptChanges();
            if (dt.Rows.Count > 0)
            {
                string maxNo = dt.Compute("MAX(NO)", "").ToString();
                string where = string.Format("NO='{0}'", maxNo);
                DataRow DetailRow = dt.Select(where)[0];

                CurrentMaintain["Result"] = DetailRow["Result"];
                CurrentMaintain["Date"] = DetailRow["inspdate"];
                CurrentMaintain["Remark"] = DetailRow["remark"];
                
            }
            else
            {
                CurrentMaintain["Result"] = "";
                CurrentMaintain["Date"] = DBNull.Value;
                CurrentMaintain["Remark"] = "";
            }
           

            return base.ClickSave();

          
        }

        private void txtSP_Validated(object sender, EventArgs e)
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
                    txtSP.Text = "";
                    this.txtSP.Focus();
                    MyUtility.Msg.WarningBox(string.Format("<The OrderID: {0}> is not verify", this.txtSP.Text));
                    return;
                }
            }
        }

        void update_detailgrid_CellValidated(int RowIndex)
        {
            this.detailgrid.InvalidateRow(RowIndex);
        }
      
        private void Send_Mail()
        {           
              
            string mailto = "";
            string mailcc = "";
            string subject = "Garment Test - Style #:" + displayStyle.Text + ", Season :" + displaySeason.Text;
            string content = "Garment Test - Style #:" + displayStyle.Text + ", Season :" + displaySeason.Text + " had been sent, please receive and confirm";
            var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, mailcc, subject, null, content.ToString(), false, true);
            email.ShowDialog(this);            
          
        }
    }
}
