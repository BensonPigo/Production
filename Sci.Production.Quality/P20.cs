﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci.Win.Tools;
using System.Data.SqlClient;
using Sci.Data;
using Ict;
using System.Linq;


namespace Sci.Production.Quality
{
    public partial class P20 : Sci.Win.Tems.Input6
    {
        string sql;
        DataRow ROW;
        bool isNew = false;

        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            GenComboBox();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
        }

        private void GenComboBox()
        {
            //this.comboShift.Type = "shift";
            //this.comboTeam.Type = "Team";
            MyUtility.Tool.SetupCombox(comboTeam, 1, 1, "A,B");
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["CDate"] = DateTime.Now;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Mdivisionid"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
        }

        private void txtSP_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtSP.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtSP.OldValue)
            {
                DataTable dt; 
                DualResult result;
                string cmd = string.Format(@"select id,styleid,dest,cpu from Orders WITH (NOLOCK) where ID='{0}' and FactoryID='{1}'", textValue, Sci.Env.User.Factory);

                // 修改and to or 原因如文件上所示：編輯狀態下判斷若RFT.OrderID <> Order.ID 或　Order.Factoryid <> 登入工廠
                if (result = DBProxy.Current.Select(null, cmd, out dt))
                {                    
                    if (MyUtility.Check.Empty(dt) || dt.Rows.Count==0)
                    {
                        this.txtSP.Text = "";
                        displayStyle.Text = "";
                        displayDestination.Text = "";
                        this.txtCPU.Text = "0";
                        this.txtSP.Focus(); 
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< SP#: {0}> does not exist OR Factory is not match !!", textValue));
                        return;
                    }
                    else
                    {
                        displayStyle.Text = dt.Rows[0]["styleid"].ToString();
                        displayDestination.Text = MyUtility.Check.Empty(dt.Rows[0]["dest"].ToString()) ? "" : dt.Rows[0]["dest"].ToString() + " - " + MyUtility.GetValue.Lookup("NameEN", dt.Rows[0]["dest"].ToString(), "dbo.Country", "ID");
                        txtCPU.Text = dt.Rows[0]["cpu"].ToString();
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("Error:"+ result);
                }
            }
        }

        //refresh
        protected override void OnDetailEntered()
        {
            //add column 避免add沒有Description可使用
            DataTable dt = (DataTable)detailgridbs.DataSource;
            if (!dt.Columns.Contains("Description"))
            {
                 dt.Columns.Add("Description", typeof(string));
            }           

            DataRow dr;
            sql = string.Format(@"select B.StyleID , C.SewingCell , case when B.Dest is null then '' else B.Dest+'-'+D.NameEN end as Dest , B.CPU , [RFT_percentage]=isnull(Convert(varchar(50),Convert(FLOAT(50), round(((A.InspectQty-A.RejectQty)/ nullif(A.InspectQty, 0))*100,2))),0)
                                from Rft A WITH (NOLOCK) 
                                left join Orders B WITH (NOLOCK) on B.ID=A.OrderID
                                left join SewingLine C WITH (NOLOCK) on C.ID=A.SewinglineID and C.FactoryID=A.FactoryID
                                left join Country D WITH (NOLOCK) on D.ID=B.Dest
                                where A.ID={0}", CurrentMaintain["ID"].ToString().Trim());
            if (MyUtility.Check.Seek(sql, out dr))
            {
                this.displayStyle.Text = dr["StyleID"].ToString().Trim();
                this.displayCell.Text = dr["SewingCell"].ToString().Trim();
                this.displayDestination.Text = dr["Dest"].ToString().Trim();
                this.txtCPU.Text = MyUtility.Check.Empty(dr["CPU"].ToString()) ? "0" : dr["CPU"].ToString();
                this.txtRFT.Text = dr["RFT_percentage"].ToString().Trim();
            }

          
            DataRow drStatus;
            string Sql_status = string.Format(@"select Status from rft WITH (NOLOCK) where id='{0}'", CurrentMaintain["ID"].ToString().Trim());
            if (MyUtility.Check.Seek(Sql_status, out drStatus))
            {
                this.labConfirm.Text = drStatus["Status"].ToString();
            }
            base.OnDetailEntered();
        }

        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            DataTable dt = (DataTable)e.Details;
            dt.Columns.Add("Description", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                sql = string.Format(@"select Description from GarmentDefectCode WITH (NOLOCK) where ID='{0}'", dr["GarmentDefectCodeID"].ToString().Trim());
                if (MyUtility.Check.Seek(sql, out ROW))
                {
                    dr["Description"] = ROW["Description"];
                }
                else
                {
                    dr["Description"] = "";
                }
            }
            return base.OnRenewDataDetailPost(e);
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings GarmentDefectCodeIDCell = new DataGridViewGeneratorTextColumnSettings();

            #region MouseClick
            GarmentDefectCodeIDCell.CellMouseClick += (s, e) =>
            {
                DataRow drDesc;
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);

                    string item_cmd = @"  select distinct  b.GarmentDefectCodeID,a.GarmentDefectTypeID,a.Description from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID
 order by GarmentDefectCodeID,GarmentDefectTypeID
";

                    SelectItem item = new SelectItem(item_cmd, "10,10,25", dr["GarmentDefectCodeID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel) return;
                    dr["GarmentDefectCodeID"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select Description,a.GarmentDefectTypeID from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID where b.GarmentDefectCodeID='{0}'", item.GetSelectedString());
                    if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                    {
                        dr["Description"] = drDesc["Description"];
                        dr["GarmentDefectTypeid"] = drDesc["GarmentDefectTypeID"];
                    }
                    else
                    {

                        dr["Description"] = "";
                        dr["GarmentDefectTypeid"] = "";
                    }
                }
            };
            GarmentDefectCodeIDCell.EditingMouseDown += (s, e) =>
            {
                DataRow drDesc;
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
             
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);

                    string item_cmd = @"  select distinct  b.GarmentDefectCodeID,a.GarmentDefectTypeID,a.Description from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID
 order by GarmentDefectCodeID,GarmentDefectTypeID
";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["GarmentDefectCodeID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel) return;
                    dr["GarmentDefectCodeID"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select Description,a.GarmentDefectTypeID from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID where b.GarmentDefectCodeID='{0}'", item.GetSelectedString());
                    if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                    {
                        dr["Description"] = drDesc["Description"];
                        dr["GarmentDefectTypeid"] = drDesc["GarmentDefectTypeID"];
                    }
                    else
                    {

                        dr["Description"] = "";
                        dr["GarmentDefectTypeid"] = "";
                    }
                }
            };
            #endregion
            
            #region CellValidating
            GarmentDefectCodeIDCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return                
                var dr = this.CurrentDetailData;
                if (MyUtility.Check.Empty(e.FormattedValue.ToString()))
                {
                    dr["GarmentDefectCodeID"] = "";
                    dr["Description"] = "";
                    dr["GarmentDefectTypeid"] = "";
                    return;                
                }
                DataTable dt;
                DataRow drDesc;

                string cmd = "select ID from GarmentDefectCode WITH (NOLOCK) where ID=@ID";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@ID", e.FormattedValue));

                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    dr["GarmentDefectCodeID"] = "";
                    dr["Description"] = "";
                    dr["GarmentDefectTypeid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Defect Code: {0}> doesn't exist in Data!", e.FormattedValue));
                    return;
                }

                //帶出 Type and desc 資料
                string sqlcmd1 = string.Format(@"select Description,a.GarmentDefectTypeID from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID where b.GarmentDefectCodeID='{0}'", e.FormattedValue);

              
                if (MyUtility.Check.Seek(sqlcmd1, out drDesc))
                {
                    dr["GarmentDefectCodeID"] = e.FormattedValue;
                    dr["Description"] = drDesc["Description"];
                    dr["GarmentDefectTypeid"] = drDesc["GarmentDefectTypeID"];
                }
                else
                {
                    dr["Description"] = "";
                    dr["GarmentDefectTypeid"] = "";
                    dr.EndEdit();
                    e.Cancel = true; return;
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("GarmentDefectTypeid", header: "Defect Type", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("GarmentDefectCodeID", header: "Defect Code", width: Widths.AnsiChars(5), settings: GarmentDefectCodeIDCell)
                .Text("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), decimal_places: 0, integer_places: 5);

        }      

   
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            sql = string.Format(@"update Rft set Status='Confirmed' , editName='{0}', editDate='{1}'
                                      where ID='{2}'"
                                   , Sci.Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CurrentMaintain["ID"].ToString().Trim());
            DBProxy.Current.Execute(null, sql);
            
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            MyUtility.Msg.InfoBox("Are you sure you want to <Amend> this data !?");
            sql = string.Format(@"update Rft set Status='New' , editName='{0}', editDate='{1}'
                                      where ID='{2}'"
                                , Sci.Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CurrentMaintain["ID"].ToString().Trim());
            DBProxy.Current.Execute(null, sql);
            
        }

        // save前檢查
        protected override bool ClickSaveBefore()
        {
            decimal DefectQty = 0;

            #region 必輸檢查
            if (MyUtility.Check.Empty(CurrentMaintain["CDate"]))
            {
                dateDate.Focus();
                MyUtility.Msg.WarningBox("< Date >  can't be empty!", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                txtSP.Select();
                MyUtility.Msg.WarningBox("< SP# >  can't be empty!", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["SewinglineID"]))
            {
                txtLine.Select();
                MyUtility.Msg.WarningBox("< Line# >  can't be empty!", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Shift"]))
            {
                comboShift.Select();
                MyUtility.Msg.WarningBox("< Shift >  can't be empty!", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Team"]))
            {
                comboTeam.Select();
                MyUtility.Msg.WarningBox("< Team >  can't be empty!", "Warning");
                return false;
            }
            #endregion 必輸檢查

            foreach (DataRow row in DetailDatas) DefectQty += Convert.ToDecimal(row["Qty"]);
            CurrentMaintain["DefectQty"] = DefectQty;  //2.將表身的sum(RFT_detail.Qty) 加總回寫回表頭RFT.DefectQty

            //3.當RFT.RejectQty 有值時，若RFT.DefectQty 為空則Show Message 並Return 不可存檔
            if (!MyUtility.Check.Empty(CurrentMaintain["RejectQty"]) && Convert.ToDecimal(CurrentMaintain["DefectQty"]) == 0)
            {
                MyUtility.Msg.WarningBox("If RejectQty has value, then DefectQty must have value !!", "Warning");
                return false;
            }

            //4.當RFT.RejectQty大於RFT.DefectQty則Show Message 並Return 不可存檔
            if (Convert.ToDecimal(CurrentMaintain["RejectQty"]) > Convert.ToDecimal(CurrentMaintain["DefectQty"]))
            {
                MyUtility.Msg.WarningBox("RejectQty can not exceed DefectQty !!", "Warning");
                return false;
            }

            //5.當RFT.DefectQty大於RFT.InspQty則Show Message 並Return 不可存檔
            if (Convert.ToDecimal(CurrentMaintain["DefectQty"]) > Convert.ToDecimal(CurrentMaintain["InspectQty"]))
            {
                
                MyUtility.Msg.WarningBox("DefectQty can not exceed InspectQty !!", "Warning");
                return false;
            }
            DataTable detaildt = (DataTable)detailgridbs.DataSource;
            DataTable afterDT = new DataTable();
            //將刪除資料過的grid 重新丟進新datatable 並將資料以完全刪除來做判斷! 
            afterDT.Merge(detaildt, true);
            afterDT.AcceptChanges();
            if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["GarmentDefectCodeID"])))
            {
                MyUtility.Msg.WarningBox("<Defect Code> cannot be null!");
                return false;
            }
            foreach (DataRow dr in afterDT.Rows)
            {
                DataRow[] daArray = afterDT.Select(string.Format("GarmentDefectCodeID ='{0}'", MyUtility.Convert.GetString(dr["GarmentDefectCodeid"])));
                if (daArray.Length > 1)
                {
                    MyUtility.Msg.WarningBox(string.Format("<Defect Code: {0}> is already exist! ", MyUtility.Convert.GetString(dr["GarmentDefectCodeid"])));
                    return false;
                }
            }   
          
            DataTable dt;
            string sql = string.Format(@"select * from rft WITH (NOLOCK) where OrderID='{0}' and CDate='{1}' and SewinglineID = '{2}' 
  and FactoryID='{3}' and [Shift]='{4}' and Team='{5}' ", txtSP.Text,((DateTime)dateDate.Value).ToShortDateString(), txtLine.Text, displayFactory.Text, comboShift.SelectedValue, comboTeam.Text);
            DBProxy.Current.Select(null, sql, out dt);
            if (dt.Rows.Count > 0 && isNew)// 如果是新增,才判斷ＳＰ＃是否存在
            {
                MyUtility.Msg.WarningBox(" SP#,Shift,Team,Date,Factory can't be same in dataBase,please pick one least to change!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void txtLine_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataRow dr;
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || txtLine.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format(@"select distinct sewinglineid from Rft WITH (NOLOCK) where OrderID='{0}'", this.txtSP.Text), "10", this.txtLine.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.txtLine.Text = item.GetSelectedString();
            string sqlcmd = string.Format(@"select sewingcell from Sewingline WITH (NOLOCK) where id='{0}'", item.GetSelectedString());
            if (MyUtility.Check.Seek(sqlcmd,out dr))
            {
                this.displayCell.Text = dr["sewingcell"].ToString();
            }
            else
            {
                this.displayCell.Text = "";
            }            
        }

        private void txtLine_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtLine.Text;
            DataRow dr;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtLine.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(
                    @"select a.id,b.ID,b.SewingCell from Rft a WITH (NOLOCK) 
                    inner join SewingLine b on a.SewinglineID=b.ID
                    where orderid = '{0}' and b.ID='{1}' and b.factoryID='{2}'", this.txtSP.Text, textValue,Sci.Env.User.Factory),out dr))
                {
                    this.txtLine.Text = "";
                    this.displayCell.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Line# : {0} > not found !!", textValue));
                    return;
                }
                else
                {
                    this.displayCell.Text = dr["SewingCell"].ToString().Trim();
                }
            }
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {           
            DataTable dt;
            isNew = false;
            string sql = string.Format(@"select * from rft WITH (NOLOCK) where id='{0}'", CurrentMaintain["ID"].ToString().Trim());
            DBProxy.Current.Select(null, sql,out dt);
            if (dt.Rows[0]["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }
    

        protected override bool ClickNew()
        {
            
            this.displayStyle.Text = "";
            this.displayDestination.Text = "";
            this.txtRFT.Text="";
            this.txtCPU.Text = "0";
            this.isNew = true;
            return base.ClickNew();
        }
    }
}
