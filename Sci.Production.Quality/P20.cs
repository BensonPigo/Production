using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Sci.Win.Tools;
using System.Data.SqlClient;
using Sci.Data;
using Ict;

namespace Sci.Production.Quality
{
    public partial class P20 : Win.Tems.Input6
    {
        string sql;
        DataRow ROW;
        bool isNew = false;

        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.GenComboBox();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "'";
        }

        private void GenComboBox()
        {
            Dictionary<string, string> dicComboBox = new Dictionary<string, string>();
            dicComboBox.Add("A", "A");
            dicComboBox.Add("B", "B");
            this.comboTeam.DataSource = new BindingSource(dicComboBox, null);
            this.comboTeam.ValueMember = "key";
            this.comboTeam.DisplayMember = "value";
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["CDate"] = DateTime.Now;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Mdivisionid"] = Env.User.Keyword;
            this.CurrentMaintain["Status"] = "New";
            this.displayCell.Text = string.Empty;
        }

        private void txtSP_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtSP.Text;

            if (string.IsNullOrWhiteSpace(textValue))
            {
                this.CurrentMaintain["sewinglineid"] = string.Empty;
                this.CurrentMaintain["OrderID"] = string.Empty;
                this.displayStyle.Text = string.Empty;
                this.displayDestination.Text = string.Empty;
                this.displayCell.Text = string.Empty;
                this.txtCPU.Text = "0";
                this.txtSP.Focus();
                this.dateBuyerDelivery.Text = string.Empty;
                return;
            }

            if (textValue != this.txtSP.OldValue)
            {
                DataTable dt;
                DualResult result;
                string cmd = string.Format(
                    @"
select o.id,o.styleid,o.dest,o.cpu ,o.BuyerDelivery
from Orders o WITH (NOLOCK) 
inner join Factory f WITH (NOLOCK) on o.FactoryID=f.ID
where o.ID='{0}' and o.FtyGroup='{1}'
and f.IsProduceFty=1 ", textValue, Env.User.Factory);

                if (result = DBProxy.Current.Select(null, cmd, out dt))
                {
                    if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0)
                    {
                        this.CurrentMaintain["sewinglineid"] = string.Empty;
                        this.CurrentMaintain["OrderID"] = string.Empty;
                        this.displayStyle.Text = string.Empty;
                        this.displayDestination.Text = string.Empty;
                        this.displayCell.Text = string.Empty;
                        this.dateBuyerDelivery.Text = string.Empty;
                        this.txtCPU.Text = "0";
                        this.txtSP.Focus();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< SP#: {0}> does not exist or Factory is not match !!", textValue));
                        return;
                    }

                    if (!MyUtility.Check.Empty(this.txtLine.Text) && !MyUtility.Check.Seek(string.Format("select 1 from Rft where orderID='{0}' and sewinglineid='{1}'", textValue, this.txtLine.Text)))
                    {
                        DialogResult buttonResult = MyUtility.Msg.WarningBox("This SP# doesn't belong to this line, please inform scheduler. Do you want to Continue?", "Warning", MessageBoxButtons.YesNo);
                        if (buttonResult == DialogResult.No)
                        {
                            this.txtSP.Text = this.CurrentMaintain["OrderID"].ToString();
                            return;
                        }

                        this.CurrentMaintain["sewinglineid"] = string.Empty;
                        this.CurrentMaintain["OrderID"] = textValue;
                        this.displayStyle.Text = string.Empty;
                        this.dateBuyerDelivery.Text = string.Empty;
                        this.displayDestination.Text = string.Empty;
                        this.displayCell.Text = string.Empty;
                        this.txtCPU.Text = "0";
                        this.txtLine.Focus();
                    }
                    else
                    {
                        this.displayStyle.Text = dt.Rows[0]["styleid"].ToString();
                        this.dateBuyerDelivery.Text = MyUtility.Check.Empty(dt.Rows[0]["BuyerDelivery"]) ? string.Empty : ((DateTime)dt.Rows[0]["BuyerDelivery"]).ToString("yyyy/MM/dd");
                        this.displayDestination.Text = MyUtility.Check.Empty(dt.Rows[0]["dest"].ToString()) ? string.Empty : dt.Rows[0]["dest"].ToString() + " - " + MyUtility.GetValue.Lookup("NameEN", dt.Rows[0]["dest"].ToString(), "dbo.Country", "ID");
                        this.txtCPU.Text = dt.Rows[0]["cpu"].ToString();
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("Error:" + result);
                }
            }
        }

        // refresh
        protected override void OnDetailEntered()
        {
            // add column 避免add沒有Description可使用
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            if (!dt.Columns.Contains("Description"))
            {
                 dt.Columns.Add("Description", typeof(string));
            }

            DataRow dr;
            this.sql = string.Format(
                @"
select B.StyleID 
, C.SewingCell 
, case when B.Dest is null then '' else B.Dest+'-'+D.NameEN end as Dest 
, B.CPU 
, [RFT_percentage] = isnull(Convert(varchar(50),Convert(FLOAT(50), round(((A.InspectQty-A.RejectQty)/ nullif(A.InspectQty, 0))*100,2))),0)
, B.BuyerDelivery
                                from Rft A WITH (NOLOCK) 
                                left join Orders B WITH (NOLOCK) on B.ID=A.OrderID
                                left join SewingLine C WITH (NOLOCK) on C.ID=A.SewinglineID and C.FactoryID=A.FactoryID
                                left join Country D WITH (NOLOCK) on D.ID=B.Dest
                                where A.ID={0}", this.CurrentMaintain["ID"].ToString().Trim());
            if (MyUtility.Check.Seek(this.sql, out dr))
            {
                this.displayStyle.Text = dr["StyleID"].ToString().Trim();
                this.displayCell.Text = dr["SewingCell"].ToString().Trim();
                this.displayDestination.Text = dr["Dest"].ToString().Trim();
                this.txtCPU.Text = MyUtility.Check.Empty(dr["CPU"].ToString()) ? "0" : dr["CPU"].ToString();
                this.txtRFT.Text = dr["RFT_percentage"].ToString().Trim();
                this.dateBuyerDelivery.Text = MyUtility.Check.Empty(dr["BuyerDelivery"]) ? string.Empty : ((DateTime)dr["BuyerDelivery"]).ToString("yyyy/MM/dd");
            }
            else
            {
                this.displayStyle.Text = string.Empty;
                this.displayCell.Text = string.Empty;
                this.displayDestination.Text = string.Empty;
                this.txtCPU.Text = "0";
                this.txtRFT.Text = string.Empty;
                this.dateBuyerDelivery.Text = string.Empty;
            }

            base.OnDetailEntered();
        }

        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            DataTable dt = (DataTable)e.Details;
            dt.Columns.Add("Description", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                this.sql = string.Format(@"select Description from GarmentDefectCode WITH (NOLOCK) where ID='{0}'", dr["GarmentDefectCodeID"].ToString().Trim());
                if (MyUtility.Check.Seek(this.sql, out this.ROW))
                {
                    dr["Description"] = this.ROW["Description"];
                }
                else
                {
                    dr["Description"] = string.Empty;
                }
            }

            return base.OnRenewDataDetailPost(e);
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings GarmentDefectCodeIDCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings QtyCell = new DataGridViewGeneratorNumericColumnSettings();

            #region MouseClick
            GarmentDefectCodeIDCell.CellMouseClick += (s, e) =>
            {
                DataRow drDesc;
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
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                    string item_cmd = @"  select distinct  b.GarmentDefectCodeID,a.GarmentDefectTypeID,a.Description from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID
 order by GarmentDefectCodeID,GarmentDefectTypeID
";

                    SelectItem item = new SelectItem(item_cmd, "10,10,25", dr["GarmentDefectCodeID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["GarmentDefectCodeID"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select Description,a.GarmentDefectTypeID from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID where b.GarmentDefectCodeID='{0}'", item.GetSelectedString());
                    if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                    {
                        dr["Description"] = drDesc["Description"];
                        dr["GarmentDefectTypeid"] = drDesc["GarmentDefectTypeID"];
                    }
                    else
                    {
                        dr["Description"] = string.Empty;
                        dr["GarmentDefectTypeid"] = string.Empty;
                    }
                }
            };
            GarmentDefectCodeIDCell.EditingMouseDown += (s, e) =>
            {
                DataRow drDesc;
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
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                    string item_cmd = @"  select distinct  b.GarmentDefectCodeID,a.GarmentDefectTypeID,a.Description from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID
 order by GarmentDefectCodeID,GarmentDefectTypeID
";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["GarmentDefectCodeID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["GarmentDefectCodeID"] = item.GetSelectedString();
                    string sqlcmd = string.Format(@"select Description,a.GarmentDefectTypeID from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID where b.GarmentDefectCodeID='{0}'", item.GetSelectedString());
                    if (MyUtility.Check.Seek(sqlcmd, out drDesc))
                    {
                        dr["Description"] = drDesc["Description"];
                        dr["GarmentDefectTypeid"] = drDesc["GarmentDefectTypeID"];
                    }
                    else
                    {
                        dr["Description"] = string.Empty;
                        dr["GarmentDefectTypeid"] = string.Empty;
                    }
                }
            };
            #endregion

            #region CellValidating
            GarmentDefectCodeIDCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                var dr = this.CurrentDetailData;
                if (MyUtility.Check.Empty(e.FormattedValue.ToString()))
                {
                    dr["GarmentDefectCodeID"] = string.Empty;
                    dr["Description"] = string.Empty;
                    dr["GarmentDefectTypeid"] = string.Empty;
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
                    dr["GarmentDefectCodeID"] = string.Empty;
                    dr["Description"] = string.Empty;
                    dr["GarmentDefectTypeid"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Defect Code: {0}> doesn't exist in Data!", e.FormattedValue));
                    return;
                }

                // 帶出 Type and desc 資料
                string sqlcmd1 = string.Format(@"select Description,a.GarmentDefectTypeID from GarmentDefectCode a WITH (NOLOCK) inner join Rft_Detail b WITH (NOLOCK) on a.id=b.GarmentDefectCodeID where b.GarmentDefectCodeID='{0}'", e.FormattedValue);

                if (MyUtility.Check.Seek(sqlcmd1, out drDesc))
                {
                    dr["GarmentDefectCodeID"] = e.FormattedValue;
                    dr["Description"] = drDesc["Description"];
                    dr["GarmentDefectTypeid"] = drDesc["GarmentDefectTypeID"];
                }
                else
                {
                    dr["Description"] = string.Empty;
                    dr["GarmentDefectTypeid"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
            };

            QtyCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                // DataRow dr = this.CurrentDetailData;
                int totalQty = 0;

                for (int i = 0; i < this.detailgrid.Rows.Count; i++)
                {
                    totalQty += Convert.ToInt32(this.detailgrid.Rows[i].Cells["Qty"].EditedFormattedValue);
                }

                // foreach (DataGridViewRow item in this.detailgrid.Rows)
                // {
                //    //totalQty +=Convert.ToInt32(item["Qty"]);
                //    DataRow row = ((DataRowView)item.DataBoundItem).Row;
                //    totalQty += Convert.ToInt32(row["Qty"]);
                //    row.EndEdit();
                // }
                this.CurrentMaintain["DefectQty"] = totalQty;
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("GarmentDefectTypeid", header: "Defect Type", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("GarmentDefectCodeID", header: "Defect Code", width: Widths.AnsiChars(5), settings: GarmentDefectCodeIDCell)
                .Text("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), decimal_places: 0, integer_places: 5, settings: QtyCell);
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            this.sql = string.Format(
                @"update Rft set Status='Confirmed' , editName='{0}', editDate='{1}'
                                      where ID='{2}'",
                Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.CurrentMaintain["ID"].ToString().Trim());
            DBProxy.Current.Execute(null, this.sql);
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to <Amend> this data !?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == DialogResult.No)
            {
                return;
            }

            this.sql = string.Format(
                @"update Rft set Status='New' , editName='{0}', editDate='{1}'
                                      where ID='{2}'",
                Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.CurrentMaintain["ID"].ToString().Trim());
            DBProxy.Current.Execute(null, this.sql);
        }

        // save前檢查
        protected override bool ClickSaveBefore()
        {
            decimal DefectQty = 0;

            #region 必輸檢查
            if (MyUtility.Check.Empty(this.CurrentMaintain["CDate"]))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("< Date >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]))
            {
                this.txtSP.Select();
                MyUtility.Msg.WarningBox("< SP# >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SewinglineID"]))
            {
                this.txtLine.Select();
                MyUtility.Msg.WarningBox("< Line# >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Shift"]))
            {
                this.comboShift.Select();
                MyUtility.Msg.WarningBox("< Shift >  can't be empty!", "Warning");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Team"]))
            {
                this.comboTeam.Select();
                MyUtility.Msg.WarningBox("< Team >  can't be empty!", "Warning");
                return false;
            }
            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                DefectQty += Convert.ToDecimal(row["Qty"]);
            }

            this.CurrentMaintain["DefectQty"] = DefectQty;  // 2.將表身的sum(RFT_detail.Qty) 加總回寫回表頭RFT.DefectQty

            // 3.當RFT.RejectQty 有值時，若RFT.DefectQty 為空則Show Message 並Return 不可存檔
            if (!MyUtility.Check.Empty(this.CurrentMaintain["RejectQty"]) && Convert.ToDecimal(this.CurrentMaintain["DefectQty"]) == 0)
            {
                MyUtility.Msg.WarningBox("If RejectQty has value, then DefectQty must have value !!", "Warning");
                return false;
            }

            // 4.當RFT.RejectQty大於RFT.DefectQty則Show Message 並Return 不可存檔
            if (Convert.ToDecimal(this.CurrentMaintain["RejectQty"]) > Convert.ToDecimal(this.CurrentMaintain["DefectQty"]))
            {
                MyUtility.Msg.WarningBox("RejectQty can not exceed DefectQty !!", "Warning");
                return false;
            }

            // 5.當RFT.DefectQty大於RFT.InspQty則Show Message 並Return 不可存檔
            // 2019/05/20 ISP20190590 拔掉
            // if (Convert.ToDecimal(CurrentMaintain["DefectQty"]) > Convert.ToDecimal(CurrentMaintain["InspectQty"]))
            // {

            // MyUtility.Msg.WarningBox("DefectQty can not exceed InspectQty !!", "Warning");
            //    return false;
            // }
            DataTable detaildt = (DataTable)this.detailgridbs.DataSource;
            for (int i = detaildt.Rows.Count - 1; i >= 0; i--)
            {
                if (MyUtility.Convert.GetDecimal(detaildt.Rows[i]["Qty"]) == 0)
                {
                    detaildt.Rows[i].Delete();
                }
            }

            DataTable afterDT = new DataTable();

            // 將刪除資料過的grid 重新丟進新datatable 並將資料以完全刪除來做判斷!
            afterDT.Merge(detaildt, true);
            afterDT.AcceptChanges();

            // if (afterDT.AsEnumerable().Any(row => MyUtility.Check.Empty(row["GarmentDefectCodeID"])))
            // {
            //    MyUtility.Msg.WarningBox("<Defect Code> cannot be null!");
            //    return false;
            // }
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
            string sql = string.Format(
                @"select * from rft WITH (NOLOCK) where OrderID='{0}' and CDate='{1}' and SewinglineID = '{2}' 
  and FactoryID='{3}' and [Shift]='{4}' and Team='{5}' ", this.txtSP.Text, ((DateTime)this.dateDate.Value).ToShortDateString(), this.txtLine.Text, this.displayFactory.Text, this.comboShift.SelectedValue, this.comboTeam.Text);
            DBProxy.Current.Select(null, sql, out dt);
            if (dt.Rows.Count > 0 && this.isNew) // 如果是新增,才判斷ＳＰ＃是否存在
            {
                MyUtility.Msg.WarningBox(" SP#,Shift,Team,Date,Factory can't be same in dataBase,please pick one least to change!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = this.grid.GetDataRow<DataRow>(this.grid.GetSelectedRowIndex());
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
            this.isNew = false;
            string sql = string.Format(@"select * from rft WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["ID"].ToString().Trim());
            DBProxy.Current.Select(null, sql, out dt);
            if (dt.Rows[0]["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        protected override bool ClickNew()
        {
            this.displayStyle.Text = string.Empty;
            this.displayDestination.Text = string.Empty;
            this.txtRFT.Text = string.Empty;
            this.txtCPU.Text = "0";
            this.isNew = true;
            return base.ClickNew();
        }

        private void txtLine_Validated(object sender, EventArgs e)
        {
            this.displayCell.Text = this.txtLine.Cell;
        }
    }
}
