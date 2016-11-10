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
    public partial class P21 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string Factory = Sci.Env.User.Keyword;

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
                if (tResult = DBProxy.Current.Select(null, "select distinct team  from Cfa", out dtTeam))
                {
                    this.Team_combo.DataSource = dtTeam;
                    this.Team_combo.DisplayMember = "Team";
                    this.Team_combo.ValueMember = "Team";
                }
                else { ShowErr(tResult); }

                Dictionary<String, String> Stage_RowSource = new Dictionary<string, string>();
                Stage_RowSource.Add("I", "Comments/Roving");
                Stage_RowSource.Add("C", "Change Over");
                Stage_RowSource.Add("P", "Stagger");
                Stage_RowSource.Add("R", "Re-Stagger");
                Stage_RowSource.Add("F", "Final");                              
                Stage_RowSource.Add("B", "Buyer");
                InspectStage_combo.DataSource = new BindingSource(Stage_RowSource, null);
                InspectStage_combo.ValueMember = "Key";
                InspectStage_combo.DisplayMember = "Value";

                Dictionary<String, String> Result_RowSource = new Dictionary<string, string>();
                Result_RowSource.Add("P", "Pass");
                Result_RowSource.Add("F", "Fail");
                Result_combo.DataSource = new BindingSource(Result_RowSource, null);
                Result_combo.ValueMember = "Key";
                Result_combo.DisplayMember = "Value";
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
		 FROM [Production].[dbo].[Cfa] a
        left join Orders b on a.OrderID=b.ID where a.id=@id";
            spam.Add(new SqlParameter("@id", CurrentMaintain["ID"].ToString()));

            if (MyUtility.Check.Seek(sql_cmd,spam,out dr))
            {
                this.Audit_Date.Text = Convert.ToDateTime(dr["cDate"]).ToShortDateString();
                this.SP_text.Text = dr["orderID"].ToString();
                this.Style_text.Text = dr["StyleID"].ToString();
                this.Des_text.Text = dr["dest"].ToString();
                this.Factory_text.Text = dr["FactoryID"].ToString();
                this.PO_text.Text = dr["custPONo"].ToString();
                this.orderQty_text.Text = dr["qty"].ToString();
                this.InspectQty_text.Text = dr["InspectQty"].ToString();
                this.DefectsQty_text.Text = dr["DefectQty"].ToString();
                this.Line_text.Text = dr["SewingLineID"].ToString();
                this.Garment_text.Text = dr["GarmentOutput"].ToString();
                this.CFA1_text.Text = dr["CFA"].ToString();
                this.Remark_text.Text = dr["Remark"].ToString();                
               
                if (MyUtility.Check.Empty(this.InspectQty_text.Text) || Convert.ToInt32(this.InspectQty_text.Text)==0)
                {
                    this.InspectQty_text.Text = "0";
                }
                else
                {
                    SQR_text.Text = (Convert.ToDouble(this.DefectsQty_text.Text) / Convert.ToDouble(this.InspectQty_text.Text)).ToString();
                    //四捨五入到第3位
                    SQR_text.Text = Math.Round(Convert.ToDouble(this.SQR_text.Text),3).ToString();
                }
                
            }          
            else
            {
                this.Audit_Date.Text = "";         this.SP_text.Text = "";              this.Style_text.Text ="";
                this.Des_text.Text = "";           this.Factory_text.Text = "";         this.PO_text.Text = "";
                this.orderQty_text.Text = "0";      this.InspectQty_text.Text = "0";      this.DefectsQty_text.Text = "0";
                this.Line_text.Text = "";          this.Garment_text.Text = "";         this.CFA1_text.Text = "";          this.Remark_text.Text = "";
            }
            #region btnEncode
            Encode_btn.Enabled = !this.EditMode;
            //if (MyUtility.Check.Empty(CurrentMaintain)) Encode_btn.Enabled = false;
            //if (CurrentMaintain["status"].ToString().Trim() == "Confirmed") Encode_btn.Text = "Amend";
            //else Encode_btn.Text = "Encode";
            #endregion
           

            base.OnDetailEntered();
        }
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select a.ID, a.GarmentDefectTypeid,
a.GarmentDefectCodeID,
b.Description,
c.Remark,
a.Qty,
a.Action,
[CFAAreaID] =d.Id,
d.Description as AreaDesc
from CFA_Detail a
left join GarmentDefectCode b on b.ID=a.GarmentDefectCodeID
left join Cfa c on a.ID=c.ID
left join CFAArea d on a.CFAAreaID=d.Id 
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
                    string item_cmd = "  select ID from GarmentDefectCode";
                    SelectItem item = new SelectItem(item_cmd, "15", dr["GarmentDefectCodeid"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult== DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["GarmentDefectCodeid"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select GarmentDefectTypeID,Description from GarmentDefectCode  where id='{0}'", item.GetSelectedString());
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
                    string item_cmd = "  select ID from GarmentDefectCode";
                    SelectItem item = new SelectItem(item_cmd, "15", dr["GarmentDefectCodeid"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["GarmentDefectCodeid"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select GarmentDefectTypeID,Description from GarmentDefectCode  where id='{0}'", item.GetSelectedString());
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
                    string item_cmd = "select id,Description from CfaArea";
                    SelectItem item = new SelectItem(item_cmd, "10", dr["id"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["CFAAreaID"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select id,Description from CfaArea where id='{0}'", item.GetSelectedString());
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
                    string item_cmd = "select id,Description from CfaArea";
                    SelectItem item = new SelectItem(item_cmd, "10", dr["id"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["CFAAreaID"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select id,Description from CfaArea where id='{0}'", item.GetSelectedString());
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
                if (this.EditMode == false) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                DataTable dt;
                DualResult result;
                string cmd = "  select * from GarmentDefectCode where id =@id";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@id",e.FormattedValue));
                if (result=DBProxy.Current.Select(null,cmd,spam,out dt))
                {
                    if (dt.Rows.Count < 1)
                    {
                        MyUtility.Msg.InfoBox("<Defect Code> is not exist");
                        dr["GarmentDefectCodeid"] = "";
                        return;
                    }
                }

            };

            defectQty.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (Convert.ToInt32(e.FormattedValue) < 0)
                {
                    MyUtility.Msg.InfoBox("<No.Of Defects> cannot less than 0"); 
                    dr["Qty"] = "";
                    return;
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

        private void Encode_btn_Click(object sender, EventArgs e)
        {
            DataTable dt;
            string cmd = "select * from cfa where orderid=@orderid order by cDate desc";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@orderid", this.SP_text.Text));
            DBProxy.Current.Select(null, cmd, spam, out dt);

            if (this.Encode_btn.Text=="Encode")
            {                

                DualResult dResult; 
                List<SqlParameter> spamEncode = new List<SqlParameter>();
                string updCmd = "  update Cfa  set status='Confirmed',EditName=@user,EditDate=GETDATE() where ID=@id ";
                spamEncode.Add(new SqlParameter("@id", CurrentMaintain["ID"].ToString()));
                spamEncode.Add(new SqlParameter("@user", loginID));               

                if (dResult= DBProxy.Current.Execute(null,updCmd,spamEncode))
                {
                    string updOrders = "update orders set inspdate=@insdate,InspResult=@result,inspHandle=@cfa where id=@id";
                    List<SqlParameter> spamO = new List<SqlParameter>();
                    spamO.Add(new SqlParameter("@insdate", Convert.ToDateTime(dt.Rows[0]["cDate"]).ToShortDateString()));
                    spamO.Add(new SqlParameter("@result", dt.Rows[0]["Result"]));
                    spamO.Add(new SqlParameter("@cfa", dt.Rows[0]["Cfa"]));
                    spamO.Add(new SqlParameter("@id", this.SP_text.Text));
                    DBProxy.Current.Execute(null, updOrders, spamO);

                }
                this.Encode_btn.Text = "Amend";

                
            }
            // Amend
            else
            {
                DualResult dResult;
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
                    spamO.Add(new SqlParameter("@id", this.SP_text.Text));
                    DBProxy.Current.Execute(null, updOrders, spamO);

                }
                this.Encode_btn.Text = "Encode";
            }

            OnDetailEntered();            
        }
        // save 前檢查
        protected override bool ClickSaveBefore()
        {
          
            DataTable gridDT = (DataTable)this.detailgridbs.DataSource;
            DataTable afterDT = new DataTable();
            afterDT.Merge(gridDT, true);
            afterDT.AcceptChanges();


            if (MyUtility.Check.Empty(this.Audit_Date.Text))
            {
                MyUtility.Msg.WarningBox("<Audit Date> cannot be empty", "Warning");
                this.Audit_Date.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.SP_text.Text))
            {
                MyUtility.Msg.WarningBox("<SP#> cannot be empty", "Warning");
                this.SP_text.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.Line_text.Text))
            {
                MyUtility.Msg.WarningBox("<Line#> cannot be empty", "Warning");
                this.Line_text.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.txtdropdownlist1.Text))
            {
                MyUtility.Msg.WarningBox("<Shift> cannot be empty", "Warning");
                this.txtdropdownlist1.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.Team_combo.Text))
            {
                MyUtility.Msg.WarningBox("<Team> cannot be empty", "Warning");
                this.Team_combo.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.InspectStage_combo.Text))
            {
                MyUtility.Msg.WarningBox("<InspectStage> cannot be empty", "Warning");
                this.InspectStage_combo.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.Result_combo.Text))
            {
                MyUtility.Msg.WarningBox("<Result> cannot be empty", "Warning");
                this.Result_combo.Select();
                return false;
            }
            //((DataTable)this.detailgridbs.DataSource)

            foreach (DataRow dr in afterDT.Rows)
            {
                if (MyUtility.Check.Empty(dr["GarmentDefectCodeid"]))
                {
                    MyUtility.Msg.WarningBox("<Defect Code> cannot be empty", "Warning");
                    return false;
                }
                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    MyUtility.Msg.WarningBox("<No. of Defects> cannot be empty", "Warning");
                    return false;
                }
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword, "CFA",(DateTime)Convert.ToDateTime(this.Audit_Date.Text));
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }
            int qty = 0;
            int InQty = 0; 

            DataTable dt = (DataTable)detailgridbs.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                qty = qty + Convert.ToInt32(dt.Rows[i]["Qty"]);
            }
            this.DefectsQty_text.Text = qty.ToString();           
            InQty = Convert.ToInt32(this.InspectQty_text.Text);
            if (qty > InQty)
            {
                MyUtility.Msg.WarningBox("<Defects Qty> cannot more than <Inspect Qty>", "Warning");
                return false;
            }
            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {                  
            
            base.ClickSaveAfter();
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";            
            CurrentMaintain["cDate"] = DateTime.Now;
            this.Factory_text.Text = Sci.Env.User.Keyword;
            this.Audit_Date.Text = Convert.ToDateTime(DateTime.Now).ToShortDateString();
        }
        // edit前檢查
        protected override bool ClickEditBefore()
        {
            DataTable dt;
            string sql = string.Format(@"select * from cfa where id='{0}'", CurrentMaintain["ID"].ToString().Trim());
            DBProxy.Current.Select(null,sql,out dt);
            if (dt.Rows[0]["status"].ToString().ToUpper()== "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
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
        protected override DualResult ClickSave()
        {
            DualResult dresult;
            string updCmd = "update Cfa set cDate=@cDate,OrderID=@orderID,InspectQty=@InsQty,SewingLineID=@line,CFA=@cfa,Remark=@Remark,DefectQty=@DefectQty where id=@id ";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@cDate", Audit_Date.Text));
            spam.Add(new SqlParameter("@orderID", SP_text.Text));
            spam.Add(new SqlParameter("@InsQty", InspectQty_text.Text));
            spam.Add(new SqlParameter("@line", Line_text.Text));
            spam.Add(new SqlParameter("@cfa", CFA1_text.Text));
            spam.Add(new SqlParameter("@Remark", Remark_text.Text));
            spam.Add(new SqlParameter("@id", CurrentMaintain["id"]));
            spam.Add(new SqlParameter("@DefectQty", DefectsQty_text.Text));
            if (dresult = DBProxy.Current.Execute(null, updCmd, spam))
            {
                MyUtility.Msg.InfoBox("save successful");
            }
       
            
            return base.ClickSave();
        }

        private void InspectStage_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            switch (InspectStage_combo.Text)
            {
                case "Comments/Roving":
                    this.txtStageInfo.Text = "Still in the line/no complete carton";
                        break;
                case "Change Over":
                        this.txtStageInfo.Text = "0~10% complete cartons";
                        break;
                case "Stagger":
                        this.txtStageInfo.Text = "11~79% complete cartons";
                        break;
                case "Re-Stagger":
                        this.txtStageInfo.Text = "Re-inspection";
                        break;
                case "Final":
                        this.txtStageInfo.Text = "80~100% complete cartons";
                    break;
                case "Buyer":
                    this.txtStageInfo.Text = "Buyer inspector, third party inspection, CFA";
                    break;
                case "":
                     this.txtStageInfo.Text = "";
                    break;
                                
            }
        }

        private void SP_text_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.SP_text.Text))
            {
                return;
            }
            DataTable dt;
            DualResult result;
            string sqlcmd = string.Format(@"select a.OrderID,a.FactoryID,b.StyleID,b.Dest,b.CustPONo,b.Qty from Cfa a
inner join Orders b on a.OrderID=b.ID 
where a.OrderID='{0}'", SP_text.Text);
            result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (result)
            {
                if (dt.Rows.Count>0)
                {
                    this.Style_text.Text = dt.Rows[0]["StyleID"].ToString();
                    this.Des_text.Text = dt.Rows[0]["Dest"].ToString();
                    this.Factory_text.Text = dt.Rows[0]["FactoryID"].ToString();
                    this.PO_text.Text = dt.Rows[0]["CustPONo"].ToString();
                    this.orderQty_text.Text = dt.Rows[0]["Qty"].ToString();
                }
            }
        }

    }
}
