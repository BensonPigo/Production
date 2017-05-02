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
    public partial class P21 : Sci.Win.Tems.Input6
    {
        
        private string loginID = Sci.Env.User.UserID;
        string tmpId;

        private Dictionary<string, string> ResultCombo = new Dictionary<string, string>();

        public P21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
           this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
        }
        protected override void OnFormLoaded()
        {
 	            base.OnFormLoaded();
                #region Combox setting
                DataTable dtTeam;
                Ict.DualResult tResult;
                if (tResult = DBProxy.Current.Select(null, "select distinct team  from Cfa WITH (NOLOCK) ", out dtTeam))
                {
                    this.comboTeam.DataSource = dtTeam;
                    this.comboTeam.DisplayMember = "Team";
                    this.comboTeam.ValueMember = "Team";
                }
                else { ShowErr(tResult); }

                Dictionary<String, String> Stage_RowSource = new Dictionary<string, string>();
                Stage_RowSource.Add("I", "Comments/Roving");
                Stage_RowSource.Add("C", "Change Over");
                Stage_RowSource.Add("P", "Stagger");
                Stage_RowSource.Add("R", "Re-Stagger");
                Stage_RowSource.Add("F", "Final");                              
                Stage_RowSource.Add("B", "Buyer");
                comboInspectionStage.DataSource = new BindingSource(Stage_RowSource, null);
                comboInspectionStage.ValueMember = "Key";
                comboInspectionStage.DisplayMember = "Value";

                Dictionary<String, String> Result_RowSource = new Dictionary<string, string>();
                Result_RowSource.Add("P", "Pass");
                Result_RowSource.Add("F", "Fail");
                comboResult.DataSource = new BindingSource(Result_RowSource, null);
                comboResult.ValueMember = "Key";
                comboResult.DisplayMember = "Value";
                #endregion

        }
      
        protected override void OnDetailEntered()
        {
            List<SqlParameter> spam = new List<SqlParameter>();
            DataRow dr;
            string sql_cmd =
        @"SELECT a.ID,a.cDate,a.OrderID,a.FactoryID,a.InspectQty,
		a.DefectQty,a.SewingLineID,a.Team,a.GarmentOutput,a.Stage,a.CFA,a.Shift,a.Result,a.Remark,a.Status,
		b.StyleID,b.Dest,b.CustPONo,b.Qty	
		FROM [Production].[dbo].[Cfa] a WITH (NOLOCK) 
        left join Orders b WITH (NOLOCK) on a.OrderID=b.ID 
        where a.id=@id";
            spam.Add(new SqlParameter("@id", CurrentMaintain["ID"].ToString()));

            if (MyUtility.Check.Seek(sql_cmd,spam,out dr))
            {                
                this.txtStyle.Text = dr["StyleID"].ToString();
                this.txtDestination.Text = dr["dest"].ToString();
                this.txtPO.Text = dr["custPONo"].ToString();
                this.numOrderQty.Text = dr["qty"].ToString();            
               
                if (MyUtility.Check.Empty(this.numInspectQty.Text) || Convert.ToInt32(this.numInspectQty.Text)==0)
                {
                    this.numInspectQty.Text = "0";
                }
                else
                {
                    decimal sqrValue = MyUtility.Convert.GetDecimal(Convert.ToDouble(this.numDefectsQty.Text) / Convert.ToDouble(this.numInspectQty.Text));
                    //四捨五入到第3位
                    numSQR.Text = Math.Round(Convert.ToDouble(sqrValue), 3).ToString();
                }
                
            }          
            else
            {
                this.txtStyle.Text ="";
                this.txtDestination.Text = "";           
                this.numOrderQty.Text = "0";
                this.txtPO.Text = "";
                this.numInspectQty.Text = "0";   
                this.numSQR.Text = "0";   
            }            
            DataRow drStatus;

            if (MyUtility.Check.Seek(string.Format("select status from cfa WITH (NOLOCK) where id='{0}'", CurrentMaintain["ID"].ToString().Trim()), out drStatus))
            {
                this.labConfirm.Text = drStatus["status"].ToString();
            }

            base.OnDetailEntered();            
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select a.ID, a.GarmentDefectTypeid,
a.GarmentDefectCodeID,
b.Description,
a.Remark,
a.Qty,
a.Action,
[CFAAreaID] =d.Id,
d.Description as AreaDesc
from CFA_Detail a WITH (NOLOCK) 
left join GarmentDefectCode b WITH (NOLOCK) on b.ID=a.GarmentDefectCodeID
left join CFAArea d WITH (NOLOCK) on a.CFAAreaID=d.Id 
where a.ID='{0}'",
 masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings defectCode = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings defectQty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings AreaCode = new DataGridViewGeneratorTextColumnSettings();

            #region MousClick Event
            defectCode.CellMouseClick += (s, e) =>
            {
                DataRow drDesc;
                if (e.RowIndex == -1)  return;
                if (this.EditMode == false) return;
                if (e.Button== System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    string item_cmd = "  select ID,Description from GarmentDefectCode WITH (NOLOCK) ";
                    SelectItem item = new SelectItem(item_cmd, "15", dr["GarmentDefectCodeid"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult== DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["GarmentDefectCodeid"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select GarmentDefectTypeID,Description from GarmentDefectCode WITH (NOLOCK)  where id='{0}'", item.GetSelectedString());
                    if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                    {
                        dr["Description"] = drDesc["Description"];
                        dr["GarmentDefectTypeID"] = drDesc["GarmentDefectTypeID"];
                    }
                    else
                    {
                        dr["Description"] = "";
                        dr["GarmentDefectTypeID"] = "";
                    }
                }                 

            };
            defectCode.EditingMouseDown += (s, e) =>
            {
                DataRow drDesc;
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    string item_cmd = "  select ID,Description from GarmentDefectCode WITH (NOLOCK) ";
                    SelectItem item = new SelectItem(item_cmd, "15", dr["GarmentDefectCodeid"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["GarmentDefectCodeid"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select GarmentDefectTypeID,Description from GarmentDefectCode WITH (NOLOCK) where id='{0}'", item.GetSelectedString());
                    if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                    {
                        dr["Description"] = drDesc["Description"];
                        dr["GarmentDefectTypeID"] = drDesc["GarmentDefectTypeID"];
                    }
                    else
                    {
                        dr["Description"] = "";
                        dr["GarmentDefectTypeID"] = "";
                    }
                }
            };
            AreaCode.EditingMouseDown += (s, e) =>
            {
                DataRow drDesc;
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button== System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id,Description from CfaArea WITH (NOLOCK) ";
                    SelectItem item = new SelectItem(item_cmd, "10", dr["id"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["CFAAreaID"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select id,Description from CfaArea WITH (NOLOCK) where id='{0}'", item.GetSelectedString());
                    if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                    {
                        dr["AreaDesc"] = drDesc["Description"];
                    }
                    else
                    {
                        dr["AreaDesc"] = "";
                    }
                }
            };

            AreaCode.CellMouseClick += (s, e) =>
            {
                DataRow drDesc;
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id,Description from CfaArea WITH (NOLOCK) ";
                    SelectItem item = new SelectItem(item_cmd, "10", dr["id"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["CFAAreaID"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select id,Description from CfaArea WITH (NOLOCK) where id='{0}'", item.GetSelectedString());
                    if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                    {
                        dr["AreaDesc"] = drDesc["Description"];
                    }
                    else
                    {
                        dr["AreaDesc"] = "";
                    }
                }
            };
            #endregion

            #region Valid Event
            defectCode.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue)) return; // 沒資料 return
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                DataTable dt;
                DualResult result;
                string cmd = "  select * from GarmentDefectCode WITH (NOLOCK) where id =@id";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@id",e.FormattedValue));
                if (result=DBProxy.Current.Select(null,cmd,spam,out dt))
                {
                    if (dt.Rows.Count < 1)
                    {
                        MyUtility.Msg.WarningBox(string.Format("<Defect Code: {0}> is not exist",e.FormattedValue));
                        dr["GarmentDefectCodeid"] = "";
                        dr["Description"] = "";
                        dr["GarmentDefectTypeID"] = "";
                        dr.EndEdit();
                        e.Cancel = true; return;
                    }
                }
                DataRow drDesc;
                string sqlcmd = string.Format(@"select GarmentDefectTypeID,Description from GarmentDefectCode WITH (NOLOCK)  where id='{0}'", e.FormattedValue);
                if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                {
                    dr["GarmentDefectCodeid"] = e.FormattedValue;
                    dr["Description"] = drDesc["Description"];
                    dr["GarmentDefectTypeID"] = drDesc["GarmentDefectTypeID"];
                }
                else
                {
                    dr["Description"] = "";
                    dr["GarmentDefectTypeID"] = "";
                    dr.EndEdit();
                    e.Cancel = true; return;
                }

            };

            defectQty.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (Convert.ToInt32(e.FormattedValue) < 0)
                {
                    MyUtility.Msg.WarningBox("<No.Of Defects> cannot less than 0"); 
                    dr["Qty"] = "";
                    return;
                }
            };
            AreaCode.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;// 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue)) return; // 沒資料 return
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                DataRow drDesc;
                string sqlcmd = string.Format(@"select id,Description from CfaArea WITH (NOLOCK) where id='{0}'", e.FormattedValue);
                if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                {
                    dr["AreaDesc"] = drDesc["Description"];
                    dr["CFAAreaID"] = e.FormattedValue;                }
                else
                {
                    MyUtility.Msg.WarningBox(string.Format("<Area Code: {0}> is not exist",e.FormattedValue));
                    dr["CFAAreaID"] = "";
                    dr["AreaDesc"] = "";
                    dr.EndEdit();
                    e.Cancel = true; return;
                }
            };
            #endregion
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("GarmentDefectTypeID", header: "Defect Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("GarmentDefectCodeid", header: "Defect Code", width: Widths.AnsiChars(10),settings:defectCode)
            .Text("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("CFAAreaID", header: "Area Code", width: Widths.AnsiChars(10), settings:AreaCode)
            .Text("AreaDesc", header: "Area Desc", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(30))
            .Numeric("qty", header: "No.of Defects", width: Widths.AnsiChars(5),settings:defectQty)
            .Text("Action", header: "Action", width: Widths.AnsiChars(5));
            
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DataTable dt;
            string cmd = "select * from cfa WITH (NOLOCK) where orderid=@orderid order by cDate desc";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@orderid", this.txtSP.Text));
            DBProxy.Current.Select(null, cmd, spam, out dt);

                DualResult dResult;
                List<SqlParameter> spamEncode = new List<SqlParameter>();
                string updCmd = "  update Cfa  set status='Confirmed',EditName=@user,EditDate=GETDATE() where ID=@id ";
                spamEncode.Add(new SqlParameter("@id", CurrentMaintain["ID"].ToString()));
                spamEncode.Add(new SqlParameter("@user", loginID));

                if (dResult = DBProxy.Current.Execute(null, updCmd, spamEncode))
                {
                    string updOrders = "update orders set inspdate=@insdate,InspResult=@result,inspHandle=@cfa where id=@id";
                    List<SqlParameter> spamO = new List<SqlParameter>();
                    spamO.Add(new SqlParameter("@insdate", Convert.ToDateTime(dt.Rows[0]["cDate"]).ToShortDateString()));
                    spamO.Add(new SqlParameter("@result", dt.Rows[0]["Result"]));
                    spamO.Add(new SqlParameter("@cfa", dt.Rows[0]["Cfa"]));
                    spamO.Add(new SqlParameter("@id", this.txtSP.Text));
                    DBProxy.Current.Execute(null, updOrders, spamO);
                    this.RenewData();

                }
                OnDetailEntered();
                this.RenewData();
                EnsureToolbarExt();
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DualResult dResult;
            DataTable dt;
            string cmd = "select * from cfa WITH (NOLOCK) where orderid=@orderid order by cDate desc";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@orderid", this.txtSP.Text));
            DBProxy.Current.Select(null, cmd, spam, out dt);

            List<SqlParameter> spamAmend = new List<SqlParameter>();
            string updCmd = "  update Cfa set status='New',EditName=@user,EditDate=GETDATE() where ID=@id ";
            spamAmend.Add(new SqlParameter("@id", CurrentMaintain["ID"].ToString()));
            spamAmend.Add(new SqlParameter("@user", loginID));

            if (dResult = DBProxy.Current.Execute(null, updCmd, spamAmend))
            {
                string updOrders = "update orders set inspdate=@insdate,InspResult=@result,inspHandle=@cfa where id=@id";
                List<SqlParameter> spamO = new List<SqlParameter>();
                spamO.Add(new SqlParameter("@insdate", Convert.ToDateTime(dt.Rows[0]["cDate"]).ToShortDateString()));
                spamO.Add(new SqlParameter("@result", dt.Rows[0]["Result"]));
                spamO.Add(new SqlParameter("@cfa", dt.Rows[0]["Cfa"]));
                spamO.Add(new SqlParameter("@id", this.txtSP.Text));
                DBProxy.Current.Execute(null, updOrders, spamO);
                this.RenewData();
            }
            OnDetailEntered();
            this.RenewData();
            EnsureToolbarExt();
        }
       
        // save 前檢查

        protected override bool ClickSaveBefore()
        {
          
            DataTable gridDT = (DataTable)this.detailgridbs.DataSource;
            DataTable afterDT = new DataTable();
            afterDT.Merge(gridDT, true);
            afterDT.AcceptChanges();

            if (MyUtility.Check.Empty(this.dateAuditDate.Text))
            {
                MyUtility.Msg.WarningBox("<Audit Date> cannot be empty", "Warning");
                this.dateAuditDate.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("<SP#> cannot be empty", "Warning");
                this.txtSP.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.txtsewingline.Text))
            {
                MyUtility.Msg.WarningBox("<Line#> cannot be empty", "Warning");
                this.txtsewingline.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.txtdropdownlistShift.Text))
            {
                MyUtility.Msg.WarningBox("<Shift> cannot be empty", "Warning");
                this.txtdropdownlistShift.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.comboTeam.Text))
            {
                MyUtility.Msg.WarningBox("<Team> cannot be empty", "Warning");
                this.comboTeam.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.comboInspectionStage.Text))
            {
                MyUtility.Msg.WarningBox("<InspectStage> cannot be empty", "Warning");
                this.comboInspectionStage.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.comboResult.Text))
            {
                MyUtility.Msg.WarningBox("<Result> cannot be empty", "Warning");
                this.comboResult.Select();
                return false;
            }
            foreach (DataRow dr in afterDT.Rows)
            {
                DataRow[] daArray = afterDT.Select(string.Format("GarmentDefectCodeid ='{0}'",MyUtility.Convert.GetString(dr["GarmentDefectCodeid"])));
                if (daArray.Length>1)
                {
                    MyUtility.Msg.WarningBox(string.Format("<Defect Code: {0}> is already exist! ", MyUtility.Convert.GetString(dr["GarmentDefectCodeid"])));
                    return false;
                }
            }

            int t = afterDT.Rows.Count;
            for (int i = t - 1; i >= 0; i--)
            {
                if (MyUtility.Check.Empty(afterDT.Rows[i]["GarmentDefectCodeid"]) &&
                    MyUtility.Check.Empty(afterDT.Rows[i]["qty"]))
                {
                    gridDT.Rows[i].Delete();
                    continue;
                }
                if (MyUtility.Check.Empty(afterDT.Rows[i]["GarmentDefectCodeid"]))
                {
                    MyUtility.Msg.WarningBox("<Defect Code> cannot be empty", "Warning");
                    return false;
                }
                if (MyUtility.Check.Empty(afterDT.Rows[i]["CFAAreaID"]))
                {
                    MyUtility.Msg.WarningBox("<Area Code> cannot be empty", "Warning");
                    return false;
                }
                if (MyUtility.Check.Empty(afterDT.Rows[i]["qty"]))
                {
                     MyUtility.Msg.WarningBox("<No. of Defects> cannot be empty", "Warning");
                    return false;
                }
            
           
            }

            //取單號
            if (this.IsDetailInserting)
            {
                 tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword+"CA", "CFA",(DateTime)Convert.ToDateTime(this.dateAuditDate.Text));
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }
            int qty = 0;
            int InQty = 0;          

            foreach (DataRow row in DetailDatas)
            {
                qty += Convert.ToInt32(row["Qty"]);
            }        
            CurrentMaintain["DefectQty"] = qty.ToString();
            InQty = Convert.ToInt32(this.numInspectQty.Text);
            if (qty > InQty)
            {
                MyUtility.Msg.WarningBox("<Defects Qty> cannot more than <Inspect Qty>", "Warning");
                return false;
            }
            return base.ClickSaveBefore();
        }
  

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";            
            CurrentMaintain["cDate"] = DateTime.Now;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["MDivisionid"] = Sci.Env.User.Keyword;
        }
        // edit前檢查
        protected override bool ClickEditBefore()
        {
            DataTable dt;
            string sql = string.Format(@"select * from cfa WITH (NOLOCK) where id='{0}'", CurrentMaintain["ID"].ToString().Trim());
            DBProxy.Current.Select(null,sql,out dt);
            if (dt.Rows[0]["status"].ToString().ToUpper()== "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed !! Can not be modified...", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            
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

        private void comboInspectionStage_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            switch (comboInspectionStage.Text)
            {
                case "Comments/Roving":
                    this.txtInspectionStage.Text = "Still in the line/no complete carton";
                        break;
                case "Change Over":
                        this.txtInspectionStage.Text = "0~10% complete cartons";
                        break;
                case "Stagger":
                        this.txtInspectionStage.Text = "11~79% complete cartons";
                        break;
                case "Re-Stagger":
                        this.txtInspectionStage.Text = "Re-inspection";
                        break;
                case "Final":
                        this.txtInspectionStage.Text = "80~100% complete cartons";
                    break;
                case "Buyer":
                    this.txtInspectionStage.Text = "Buyer inspector, third party inspection, CFA";
                    break;
                case "":
                     this.txtInspectionStage.Text = "";
                    break;
                                
            }
        }

        private void numGarmentOutput_TextChanged(object sender, EventArgs e)
        {
            this.numGarmentOutput.MaxLength = 3;
        }

        private void txtSP_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSP.Text))
            {
                this.txtSP.Text = "";
                this.txtStyle.Text = "";
                this.txtDestination.Text = "";
                this.txtFactory.Text = "";
                this.txtPO.Text = "";
                this.numOrderQty.Text = "0";
                this.numSQR.Text = "0";
                this.txtSP.Focus();
                return;
            }
            DataTable dt;   
            DualResult result;
            string sqlcmd = string.Format(@"select a.ID,a.FactoryID,a.StyleID,a.Dest,a.CustPONo,a.Qty from Orders a WITH (NOLOCK) 
where a.ID='{0}'", txtSP.Text);
            result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (result)
            {
                if (MyUtility.Check.Empty(dt) || dt.Rows.Count==0)
                {
                    MyUtility.Msg.WarningBox(string.Format("<SP#: {0}> Data is not found! ",this.txtSP.Text));
                    this.txtSP.Text = "";
                    this.txtStyle.Text = "";
                    this.txtDestination.Text = "";
                    this.txtFactory.Text = "";
                    this.txtPO.Text = ""; 
                    this.numOrderQty.Text = "0";
                    this.numSQR.Text = "0";
                    this.txtSP.Focus();
                    e.Cancel = true;  
                    return;
                }
                else
                {
                    this.txtStyle.Text = dt.Rows[0]["StyleID"].ToString();
                    this.txtDestination.Text = dt.Rows[0]["Dest"].ToString();
                    this.txtFactory.Text = dt.Rows[0]["FactoryID"].ToString();
                    this.txtPO.Text = dt.Rows[0]["CustPONo"].ToString();
                    this.numOrderQty.Text = dt.Rows[0]["Qty"].ToString();
                }
            }          
        }

    }
}
