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
                Stage_RowSource.Add("I", "In-Line");
                Stage_RowSource.Add("P", "Prefinal");
                Stage_RowSource.Add("F", "Final");
                InspectStage_combo.DataSource = new BindingSource(Stage_RowSource, null);
                InspectStage_combo.ValueMember = "Key";
                InspectStage_combo.DisplayMember = "Value";


                DataTable dtShift;
                Ict.DualResult sResult;
                if (sResult = DBProxy.Current.Select(null, "select distinct Shift from Cfa ", out dtShift))
                {
                    this.Shift_combo.DataSource = dtShift;
                    this.Shift_combo.DisplayMember = "Shift";
                    this.Shift_combo.ValueMember = "Shift";
                }
                else { ShowErr(sResult); }

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
                if (dr["status"].ToString() == "Confirmed")
                {
                    this.Encode_btn.Text = "Encode";
                }
                else this.Encode_btn.Text = "Amend";
               
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
                this.orderQty_text.Text = "";      this.InspectQty_text.Text = "";      this.DefectsQty_text.Text = "";
                this.Line_text.Text = "";          this.Garment_text.Text = "";         this.CFA1_text.Text = "";          this.Remark_text.Text = "";
            }
            
           

            base.OnDetailEntered();
        }
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings defectCode = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings defectQty = new DataGridViewGeneratorNumericColumnSettings();

            #region MousClick Event
            defectCode.CellMouseClick += (s, e) =>
            {
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
                        MyUtility.Msg.InfoBox("Garment Defect CodeID is not exist");
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
            .Text("Invoice", header: "Description", width: Widths.AnsiChars(10),iseditingreadonly:true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(5))
            .Numeric("qty", header: "No.of Defects", width: Widths.AnsiChars(5),settings:defectQty)
            .Text("", header: "Action", width: Widths.AnsiChars(5));
            
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
                string updCmd = "  update Cfa  set status='New',EditName=@user,EditDate=GETDATE() where ID=@id ";
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
                
            }
            // Amend
            else
            {
                DualResult dResult;
                List<SqlParameter> spamAmend = new List<SqlParameter>();
                string updCmd = "  update Cfa set status='Confirmed',EditName=@user,EditDate=GETDATE() where ID=@id ";
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
            }

            OnDetailEntered();            
        }
        // save 前檢查
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.Audit_Date.Text))
            {
                MyUtility.Msg.WarningBox("<Audit Date> cannot be empty", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(this.SP_text.Text))
            {
                MyUtility.Msg.WarningBox("<SP#> cannot be empty", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(this.Line_text.Text))
            {
                MyUtility.Msg.WarningBox("<Line#> cannot be empty", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(this.Shift_combo.Text))
            {
                MyUtility.Msg.WarningBox("<Shift> cannot be empty", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(this.Team_combo.Text))
            {
                MyUtility.Msg.WarningBox("<Team> cannot be empty", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(this.InspectStage_combo.Text))
            {
                MyUtility.Msg.WarningBox("<InspectStage> cannot be empty", "Warning");
                return false;
            }
            if (MyUtility.Check.Empty(this.Result_combo.Text))
            {
                MyUtility.Msg.WarningBox("<Result> cannot be empty", "Warning");
                return false;
            }
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (MyUtility.Check.Empty(dr["GarmentDefectCodeid"]))
                {
                    MyUtility.Msg.WarningBox("<Defect Code> cannot be empty", "Warning");
                    return false;
                }
                if (MyUtility.Check.Empty(dr["Qty"]) && dr["Qty"] == "")
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
            if (CurrentMaintain["status"] == "CONFIRMED")
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
            string updCmd = "update Cfa set cDate=@cDate,OrderID=@orderID,InspectQty=@InsQty,SewingLineID=@line,CFA=@cfa,Remark=@Remark where id=@id ";
            List<SqlParameter> spam = new List<SqlParameter>();
            spam.Add(new SqlParameter("@cDate",Audit_Date.Text));
            spam.Add(new SqlParameter("@orderID", SP_text.Text));
            spam.Add(new SqlParameter("@InsQty", InspectQty_text.Text));
            spam.Add(new SqlParameter("@line", Line_text.Text));
            spam.Add(new SqlParameter("@cfa", CFA1_text.Text));
            spam.Add(new SqlParameter("@Remark", Remark_text.Text));
            spam.Add(new SqlParameter("@id", CurrentMaintain["id"]));
            if (dresult=DBProxy.Current.Execute(null, updCmd, spam))
            {
                MyUtility.Msg.InfoBox("save successful");
            }
            
            return base.ClickSave();
        }

    }
}
