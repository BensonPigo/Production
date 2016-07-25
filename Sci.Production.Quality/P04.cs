using Ict;
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

        private Dictionary<string, string> ResultCombo = new Dictionary<string, string>();
          
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
                   
            InitializeComponent();
            ResultCombo.Add("P", "Pass");
            ResultCombo.Add("F", "Fail");            
            DefaultFilter = string.Format("MDivisionid='{0}'",Factory);                     
          
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataTable dt ;
            DualResult result;
            string cmd = "select * from dbo.GetSCI(@poid,'')";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@poid", this.First_sp_Text.Text));
            if (result = DBProxy.Current.Select(null, cmd, spam, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    this.Early_SCI_Text.Text = dt.Rows[0]["MinSciDelivery"].ToString();
                    this.Early_Buyer_Text.Text = dt.Rows[0]["MinBuyerDelivery"].ToString();
                }
            }
            DataTable datas = (DataTable)detailgridbs.DataSource;
        }
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {                 

            DataTable dt = (DataTable)e.Details;

            dt.Columns.Add("Send", typeof(string));
            dt.Columns.Add("Receive", typeof(string));
            dt.Columns.Add("NewKey", typeof(int));
            dt.Columns.Add("LastEditName", typeof(string));
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {

                dr["NewKey"] = i;
                dr["Send"] = "";
                dr["Receive"] = "";
                dr["LastEditName"] = dt.Rows[i]["EditName"].ToString() + " - " + dt.Rows[i]["EditDate"].ToString();
                i++;
            }
            return base.OnRenewDataDetailPost(e);
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


            #region MouseClick 事件

            inspectorCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);    
                    string scalecmd = @"select id,name from Pass1 where Resign is not null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Inspector"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Inspector"] =item1.GetSelectedString(); //將選取selectitem value帶入GridView
                }
            };
            SendCell.EditingMouseClick += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);    
                if (MyUtility.Check.Empty(dr["Send"]))
                {
                    dr["Send"] = loginID;
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                }
            };
            SenderCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    string scalecmd = @"select id,name from Pass1 where Resign is not null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Sender"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Sender"] = item1.GetSelectedString(); //將選取selectitem value帶入GridView
                }
            };
            ReceiveCell.CellMouseClick += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);    
                if (MyUtility.Check.Empty(dr["Receive"]))
                {
                    dr["Receive"] = loginID;
                    dr["EditName"] = loginID;
                    dr["EditDate"] = DateTime.Now.ToShortDateString();
                }
            };
            ReceiverCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);    
                    string scalecmd = @"select id,name from Pass1 where Resign is not null";
                    SelectItem item1 = new SelectItem(scalecmd, "15,15", dr["Receiver"].ToString());
                    DialogResult result = item1.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Receiver"] = item1.GetSelectedString(); //將選取selectitem value帶入GridView
                }
            };

            #endregion

            #region Valid 事件
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
            inspectorCell.CellValidating += (s, e) =>
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);    
                    if (dr.RowState == DataRowState.Modified)
                    {
                        DataTable dt;
                        string cmd = "select * from pass1 where id=@id and Resign is not null";
                        List<SqlParameter> spam = new List<SqlParameter>();
                        spam.Add(new SqlParameter("@id", e.FormattedValue));
                        DualResult result;
                        if (result = DBProxy.Current.Select(null, cmd, spam, out dt))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                dr["EditName"] = loginID;
                                dr["EditDate"] = DateTime.Now.ToShortDateString();
                                dr["inspector"] = e.FormattedValue;
                            }
                        }
                    } 
                
                };
            CommentsCell.CellValidating += (s, e) =>
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Remark"]))
                    {
                        dr["EditName"] = loginID;
                        dr["EditDate"] = DateTime.Now.ToShortDateString();
                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            dr["Remark"] = e.FormattedValue;
                        }                        
                    }
                };
        
            SenderCell.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);    
                if (dr.RowState == DataRowState.Modified)
                {
                    DataTable dt;
                    string cmd = "select * from pass1 where id=@id and Resign is not null";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@id", e.FormattedValue));
                    DualResult result;
                    if (result = DBProxy.Current.Select(null, cmd, spam, out dt))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            dr["EditName"] = loginID;
                            dr["EditDate"] = DateTime.Now.ToShortDateString();
                            dr["sender"] = e.FormattedValue;
                        }
                    }
                }               
            };
            ReceiverCell.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);    
                if (dr.RowState == DataRowState.Modified)
                {
                    DataTable dt;
                    string cmd = "select * from pass1 where id=@id and Resign is not null";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@id", e.FormattedValue));                   
                    DualResult result;
                    if (result = DBProxy.Current.Select(null, cmd, spam, out dt) )
                    {
                        if (dt.Rows.Count > 0 )
                        {
                            dr["EditName"] = loginID;
                            dr["EditDate"] = DateTime.Now.ToShortDateString();
                            dr["Receiver"] = e.FormattedValue;
                        }
                    }
                   
                }     
                    
            };
            #endregion
            Ict.Win.UI.DataGridViewComboBoxColumn ResultComboCell;// 一定要加Ict.Win.UI 不然會跟C#原生的有所衝突

            
            Helper.Controls.Grid.Generator(this.detailgrid)
            .ComboBox("Result", header: "Result", width: Widths.AnsiChars(10), settings: ResultValid).Get(out ResultComboCell)
            .Text("No", header: "No. Of Test", width: Widths.AnsiChars(5),iseditingreadonly:true)
            .Date("Inspdate", header: "Test Date", width: Widths.AnsiChars(10),settings:inspDateCell)
            .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10),settings:inspectorCell)
            .Text("Inspector", header: "Inspector Name", width: Widths.AnsiChars(10),iseditingreadonly:true)
            .Text("Remark", header: "Comments", width: Widths.AnsiChars(10),settings:CommentsCell)
            .Text("Send", header: "Send", width: Widths.AnsiChars(10),iseditingreadonly:true,settings:SendCell)// source empty
            .Text("Sender", header: "Sender", width: Widths.AnsiChars(10),settings:SenderCell)
            .Date("SendDate", header: "Send Date", width: Widths.AnsiChars(10),iseditingreadonly:true)
            .Text("Receive", header: "Receive", width: Widths.AnsiChars(5),iseditingreadonly:true,settings:ReceiveCell)// source empty
            .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(5),settings:ReceiverCell)
            .Date("ReceiveDate", header: "Receive Date", width: Widths.AnsiChars(10),iseditingreadonly:true)
            .Text("AddName", header: "Add Name", width: Widths.AnsiChars(5),iseditingreadonly:true)// addName + addDate
            .Text("LastEditName", header: "Last Edit Name", width: Widths.AnsiChars(5),iseditingreadonly:true);//editName + editDate

            ResultComboCell.DataSource = new BindingSource(ResultCombo, null);
            ResultComboCell.ValueMember = "Key";
            ResultComboCell.DisplayMember = "Value";
        }
        protected override void ClickNewAfter()
        {
            CurrentMaintain["No"] = (int)CurrentMaintain["No"]+1;
            base.ClickNewAfter();
        }
        protected override void OnDetailGridInsert(int index = -1)
        {
            int MaxNo = 0;
            DataTable dt = (DataTable)detailgridbs.DataSource;
             MaxNo = Convert.ToInt32(dt.Compute("Max(No)", string.Empty));
            base.OnDetailGridInsert(0);
            //dt.Rows[index]["no"].ToString() = 2;
            CurrentDetailData["No"] = MaxNo+1;
            
           
           
            
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
            if (this.SP_Text.Text=="" || MyUtility.Check.Empty(this.SP_Text.Text))
            {
                MyUtility.Msg.WarningBox("SP# cannot be empty !! ");
                return false;
            }
            return base.ClickSaveBefore();
        }
        protected override DualResult ClickSave()
        {

            DualResult upResult = new DualResult(true);
            string update_cmd = ""; 
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {               
                if (dr.RowState == DataRowState.Deleted)
                {                   
                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From GarmentTest_Detail Where id =@id and no=@no";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@no", dr["NO", DataRowVersion.Original]));
                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    continue;
                }
            }                

            DataTable dt = (DataTable)detailgridbs.DataSource;
            DataTable dtResult;
            string maxNo = dt.Compute("MAX(NO)", "").ToString();
            string cmd = "select * from GarmentTest_Detail where id=@id and no=@no";
            List<SqlParameter> spm = new List<SqlParameter>();
            spm.Add(new SqlParameter("@id", dt.Rows[0]["ID"]));
            spm.Add(new SqlParameter("@no", maxNo));
            if (upResult =DBProxy.Current.Select(null,cmd,spm,out dtResult))
            {
                //this.Last_Result_Text.Text = dtResult.Rows[0]["result"].ToString();
                //this.Last_Date_Text.Text = dtResult.Rows[0]["inspdate"].ToString();
                //this.Comment_text.Text = dtResult.Rows[0]["remark"].ToString();
             

            }            
                    
            return base.ClickSave();
        }

        private void SP_Text_Validated(object sender, EventArgs e)
        {
            DataTable dt;
            DualResult result;
            string cmd = @"select b.* from Orders a
left join GarmentTest b on a.ID=b.OrderID and a.StyleID=b.StyleID and a.SeasonID=b.SeasonID and a.BrandID=b.BrandID
left join Order_Qty c on a.ID=c.ID and c.Article=b.Article where a.id=@orderID";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@orderID", this.SP_Text.Text));
            if (result = DBProxy.Current.Select(null, cmd, spam, out dt))
            {
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.InfoBox("The OrderID is not verify");
                    SP_Text.Text = "";
                    return;
                }
            }
        }
     
        private void Send_mail_btn_Click(object sender, EventArgs e)
        {
            Send_Mail();
        }
        private void Send_Mail()
        {
            DataTable dt;
              
            string mailto = "";
            string subject = "Garment Test - Style #:" + style_text.Text + ", Season :" + Season_Text.Text;
            string content = "Garment Test - Style #:" + style_text.Text + ", Season :" + Season_Text.Text + " had been sent, please receive and confirm";
            var email = new MailTo(Sci.Env.User.MailAddress, mailto, Sci.Env.User.MailAddress, subject, null, content.ToString(), false, true);
            //var email = new MailTo("willy.wei@sportscity.com", "willy.wei@sportscity.com", "willy.wei@sportscity.com", subject, null, content.ToString(), false, true);
            email.ShowDialog(this);            
          
        }
    }
}
