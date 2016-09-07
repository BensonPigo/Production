using System;
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


namespace Sci.Production.Quality
{
    public partial class P20 : Sci.Win.Tems.Input6
    {
        string sql;
        DataRow ROW;


        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            GenComboBox();
        }

        private void GenComboBox()
        {
            this.comboShift.Type = "shift";
            this.comboTeam.Type = "Team";
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["CDate"] = DateTime.Now;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
        }

        private void txtSP_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtSP.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtSP.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(@"select id from Orders where ID='{0}' and FactoryID='{1}'", textValue, Sci.Env.User.Factory)))
                {
                    MyUtility.Msg.WarningBox(string.Format("< SP# > is not exist OR Factory is not match !!", textValue));
                    this.txtSP.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        //refresh
        protected override void OnDetailEntered()
        {
            DataRow dr;
            sql = string.Format(@"select B.StyleID , C.SewingCell , case when B.Dest is null then '' else B.Dest+'-'+D.NameEN end as Dest , B.CPU , Convert(varchar(50),Convert(FLOAT(50), round(((A.InspectQty-A.RejectQty)/ nullif(A.InspectQty, 0))*100,2))) as RFT_percentage
                                from Rft A
                                left join Orders B on B.ID=A.OrderID
                                left join SewingLine C on C.ID=A.SewinglineID and C.FactoryID=A.FactoryID
                                left join Country D on D.ID=B.Dest
                                where A.ID={0}", CurrentMaintain["ID"].ToString().Trim());
            if (MyUtility.Check.Seek(sql, out dr))
            {
                this.DisplayStyle.Text = dr["StyleID"].ToString().Trim();
                this.DisplayCell.Text = dr["SewingCell"].ToString().Trim();
                this.DisplayDest.Text = dr["Dest"].ToString().Trim();
                this.NumCPU.Text = dr["CPU"].ToString().Trim();
                this.NumRFT.Text = dr["RFT_percentage"].ToString().Trim();
            }

            #region btnEncode
            btnEncode.Enabled = this.EditMode;
            if (MyUtility.Check.Empty(CurrentMaintain)) btnEncode.Enabled = false;
            if (CurrentMaintain["status"].ToString().Trim() == "Confirmed") btnEncode.Text = "Amend";
            else btnEncode.Text = "Encode";
            #endregion

            base.OnDetailEntered();
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings GarmentDefectCodeIDCell = new DataGridViewGeneratorTextColumnSettings();

            #region MouseClick
            GarmentDefectCodeIDCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    var dr = this.CurrentDetailData;
                    string item_cmd = "Select ID from GarmentDefectCode ";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["GarmentDefectCodeID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel) return;
                    dr["GarmentDefectCodeID"] = item.GetSelectedString();
                }
            };
            #endregion

            #region CellValidating
            GarmentDefectCodeIDCell.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (e.FormattedValue.ToString().Trim() == "") return;
                DataTable dt;
                var dr = this.CurrentDetailData;
                string cmd = "select ID from GarmentDefectCode where ID=@ID";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@ID", e.FormattedValue));

                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.InfoBox("<Defect Code> doesn't exist in Data!");
                    dr["GarmentDefectCodeID"] = "";
                    return;
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("GarmentDefectTypeid", header: "Defect Type", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("GarmentDefectCodeID", header: "Defect Code", width: Widths.AnsiChars(5), settings: GarmentDefectCodeIDCell)
                .Text("Description", header: "Desctiption", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), decimal_places: 0, integer_places: 5);

        }

        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            DataTable dt = (DataTable)e.Details;
            dt.Columns.Add("Description", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                sql = string.Format(@"select Description from GarmentDefectCode where ID='{0}'" , dr["GarmentDefectCodeID"].ToString().Trim());
                if (MyUtility.Check.Seek(sql, out ROW)) dr["Description"] = ROW["Description"];
            }
            return base.OnRenewDataDetailPost(e);
        }

        //[Encode][Amend]
        private void btnEncode_Click(object sender, EventArgs e)
        {
            if (btnEncode.Text == "Encode")
            {
                sql = string.Format(@"update Rft set Status='Confirmed' , editName='{0}', editDate='{1}'
                                      where ID='{2}'"
                                    , Sci.Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CurrentMaintain["ID"].ToString().Trim());
                DBProxy.Current.Execute(null, sql);
                btnEncode.Text = "Amend";
            }
            else if (btnEncode.Text == "Amend")
            {
                sql = string.Format(@"update Rft set Status='New' , editName='{0}', editDate='{1}'
                                      where ID='{2}'"
                                    , Sci.Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CurrentMaintain["ID"].ToString().Trim());
                DBProxy.Current.Execute(null, sql);
                btnEncode.Text = "Encode";
            }
        }

        // save前檢查
        protected override bool ClickSaveBefore()
        {
            decimal DefectQty = 0;

            #region 必輸檢查
            if (MyUtility.Check.Empty(CurrentMaintain["CDate"]))
            {
                MyUtility.Msg.WarningBox("< Date >  can't be empty!", "Warning");
                CDate.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("< SP# >  can't be empty!", "Warning");
                CDate.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["SewinglineID"]))
            {
                MyUtility.Msg.WarningBox("< Line# >  can't be empty!", "Warning");
                CDate.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Shift"]))
            {
                MyUtility.Msg.WarningBox("< Shift >  can't be empty!", "Warning");
                CDate.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Team"]))
            {
                MyUtility.Msg.WarningBox("< Team >  can't be empty!", "Warning");
                CDate.Focus();
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

            return base.ClickSaveBefore();
        }

        private void txtLine_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || txtLine.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select distinct id from SewingLine", "10", this.txtLine.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.txtLine.Text = item.GetSelectedString();
        }

        private void txtLine_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtLine.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtLine.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(@"select id from SewingLine where id = '{0}'", textValue)))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Line# : {0} > not found !!", textValue));
                    this.txtLine.Text = "";
                    e.Cancel = true;
                    return;
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
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }


    }
}
