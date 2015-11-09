using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.IE
{
    public partial class P01 : Sci.Win.Tems.Input6
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        Ict.Win.DataGridViewGeneratorTextColumnSettings operation = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings machine = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings mold = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings frequency = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings smvsec = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private string styleID, seasonID, brandID, comboType;
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
            this.detailgrid.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        }

        public P01(string StyleID, string BrandID, string SeasonID, string ComboType)
        {
            InitializeComponent();
            DefaultFilter = string.Format("StyleID = '{0}' and BrandID = '{1}' {2} {3}", StyleID, BrandID, SeasonID == null ? "" : "and SeasonID = '" + SeasonID + "'", ComboType == null ? "" : "and ComboType = '" + ComboType + "'");
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select 0 as Selected, td.*,o.DescEN as OperationDescEN,o.MtlFactorID as OperationMtlFactorID
from TimeStudy_Detail td
left join Operation o on td.OperationID = o.ID
where td.ID = '{0}'
order by td.Seq", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "T,B,I,O");
            MyUtility.Tool.SetupCombox(comboBox2, 1, 1, "Estimate,Initial,Prelim,Final");
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            displayBox2.Value = MyUtility.GetValue.Lookup(string.Format("select CdCodeID from Style where ID = '{0}' and SeasonID = '{1}' and BrandID = '{2}'", CurrentMaintain["StyleID"].ToString(), CurrentMaintain["SeasonID"].ToString(), CurrentMaintain["BrandID"].ToString()));
            bool canEdit = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P01. Factory GSD", "CanEdit");
            button1.Enabled = !this.EditMode && CurrentMaintain != null && canEdit;
            button2.Enabled = !this.EditMode && CurrentMaintain != null && canEdit;
            button3.Enabled = !this.EditMode && CurrentMaintain != null;
            button8.Enabled = !this.EditMode && CurrentMaintain != null;
            button5.Enabled = CurrentMaintain != null;
            button6.Enabled = CurrentMaintain != null;
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Ict.Win.DataGridViewGeneratorTextColumnSettings seq = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            #region Seq & Operation Code & Frequency & SMV & M/C & Attachment按右鍵與Validating
            #region Seq的Valid
            seq.CellValidating += (s, e) =>
            {
                if (EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue) || (e.FormattedValue.ToString() != dr["Seq"].ToString()))
                    {
                        string oldValue = MyUtility.Check.Empty(dr["Seq"]) ? "" : dr["Seq"].ToString();
                        dr["Seq"] = MyUtility.Check.Empty(e.FormattedValue) ? "" : e.FormattedValue.ToString().Trim().PadLeft(4, '0');
                        dr.EndEdit();
                    }
                }
            };
            #endregion
            #region Operation Code
            operation.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                            Sci.Production.IE.P01_SelectOperationCode callNextForm = new Sci.Production.IE.P01_SelectOperationCode();
                            DialogResult result = callNextForm.ShowDialog(this);
                            if (result == System.Windows.Forms.DialogResult.OK)
                            {
                                dr["OperationID"] = callNextForm.p01SelectOperationCode["ID"].ToString();
                                dr["OperationDescEN"] = callNextForm.p01SelectOperationCode["DescEN"].ToString();
                                dr["MachineTypeID"] = callNextForm.p01SelectOperationCode["MachineTypeID"].ToString();
                                dr["Mold"] = callNextForm.p01SelectOperationCode["MoldID"].ToString();
                                dr["OperationMtlFactorID"] = callNextForm.p01SelectOperationCode["MtlFactorID"].ToString();
                                dr["SeamLength"] = callNextForm.p01SelectOperationCode["SeamLength"].ToString();
                                dr["SMV"] = MyUtility.Convert.GetDecimal(callNextForm.p01SelectOperationCode["SMV"]) * 60;
                                dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(callNextForm.p01SelectOperationCode["SMV"]);
                                dr["Frequency"] = 1;
                                dr.EndEdit();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
            };

            operation.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["OperationID"].ToString())
                    {
                        if (e.FormattedValue.ToString().Substring(0, 2) == "--")
                        {
                            dr["OperationID"] = e.FormattedValue.ToString();
                            dr["OperationDescEN"] = "";
                            dr["MachineTypeID"] = "";
                            dr["Mold"] = "";
                            dr["OperationMtlFactorID"] = "";
                            dr["Frequency"] = 0;
                            dr["SeamLength"] = 0;
                            dr["SMV"] = 0;
                            dr["IETMSSMV"] = 0;
                        }
                        else
                        {
                            //sql參數
                            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                            sp1.ParameterName = "@id";
                            sp1.Value = e.FormattedValue.ToString();

                            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                            cmds.Add(sp1);

                            string sqlCmd = "select DescEN,SMV,MachineTypeID,SeamLength,Mold,MtlFactorID from Operation where CalibratedCode = 1 and ID = @id";
                            DataTable opData;
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out opData);
                            if (result)
                            {
                                if (opData.Rows.Count <= 0)
                                {
                                    MyUtility.Msg.WarningBox(string.Format("< OperationCode: {0} > not found!!!", e.FormattedValue.ToString()));
                                    ChangeToEmptyData(dr);
                                }
                                else
                                {
                                    dr["OperationID"] = e.FormattedValue.ToString();
                                    dr["OperationDescEN"] = opData.Rows[0]["DescEN"].ToString();
                                    dr["MachineTypeID"] = opData.Rows[0]["MachineTypeID"].ToString();
                                    dr["Mold"] = opData.Rows[0]["Mold"].ToString();
                                    dr["OperationMtlFactorID"] = opData.Rows[0]["MtlFactorID"].ToString();
                                    dr["Frequency"] = 1;
                                    dr["SeamLength"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SeamLength"]);
                                    dr["SMV"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SMV"]) * 60;
                                    dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SMV"]);
                                }
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n"+result.ToString());
                                ChangeToEmptyData(dr);
                            }
                        }
                        dr.EndEdit();
                    }
                }
            };
            #endregion
            #region Frequency
            frequency.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["Frequency"]))
                    {
                        if (!MyUtility.Check.Empty(dr["OperationID"]) && dr["OperationID"].ToString().Substring(0, 2) != "--")
                        {
                            dr["Frequency"] = e.FormattedValue.ToString();
                            string smv = MyUtility.GetValue.Lookup(string.Format("select SMV from Operation where ID = '{0}'", dr["OperationID"].ToString()));
                            if (smv == "")
                            {
                                dr["SMV"] = 0;
                                dr["IETMSSMV"] = 0;
                            }
                            else
                            {
                                dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(smv) * MyUtility.Convert.GetDecimal(dr["Frequency"]);
                                dr["SMV"] = MyUtility.Convert.GetDecimal(smv) * MyUtility.Convert.GetDecimal(dr["Frequency"]) * 60;
                            }
                        }

                        dr.EndEdit();
                    }
                }
            };
            #endregion
            #region SMV
            smvsec.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["SMV"]))
                    {
                        dr["SMV"] = e.FormattedValue.ToString();
                        if (MyUtility.Convert.GetDecimal(e.FormattedValue) == 0)
                        {
                            dr["PcsPerHour"] = 0;
                        }
                        else
                        {
                            dr["PcsPerHour"] = MyUtility.Convert.GetDouble(dr["SMV"]) == 0 ? 0 : MyUtility.Math.Round((3600.0 / MyUtility.Convert.GetDouble(dr["SMV"])), 1);
                        }
                        dr.EndEdit();
                    }
                }
            };
            #endregion
            #region M/C
            machine.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = "select ID,Description from MachineType where Junk = 0";
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", dr["MachineTypeID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            machine.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["MachineTypeID"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("select ID,Description from MachineType where Junk = 0 and ID = '{0}'", e.FormattedValue.ToString())))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< M/C: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["MachineTypeID"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            #endregion
            #region Attachment
            mold.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = "select ID,DescEN from Mold where Junk = 0";
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,15", dr["Mold"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            IList<DataRow> selectData = item.GetSelecteds();
                            dr["Mold"] = item.GetSelectedString();
                        }
                    }
                }
            };

            mold.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Mold"]))
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                        sp1.ParameterName = "@id";
                        sp1.Value = MyUtility.Convert.GetString(e.FormattedValue);

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);

                        DataTable moldData;
                        string sqlCmd = "select ID,DescEN from Mold where Junk = 0 and ID = @id";
                        DualResult result = DBProxy.Current.Select(null,sqlCmd,cmds,out moldData);
                        if (result)
                        {
                            if (moldData.Rows.Count <= 0)
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Attachment: {0} > not found!!!", MyUtility.Convert.GetString(e.FormattedValue)));
                                dr["Mold"] = "";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dr["Mold"] = MyUtility.Convert.GetString(e.FormattedValue);
                            }
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n"+result.ToString());
                            dr["Mold"] = "";
                        }
                    }
                }
            };
            #endregion
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(4), settings: seq)
                .Text("OperationID", header: "Operation code", width: Widths.AnsiChars(13), settings: operation)
                .EditText("OperationDescEN", header: "Operation Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(30))
                .Numeric("Frequency", header: "Frequency", integer_places: 2, decimal_places: 2, maximum: 99.99M, minimum: 0, settings: frequency)
                .Text("OperationMtlFactorID", header: "Factor", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("SMV", header: "SMV (sec)", integer_places: 4, decimal_places: 4, maximum: 9999.9999M, minimum: 0, settings: smvsec)
                .Text("MachineTypeID", header: "M/C", width: Widths.AnsiChars(8), settings: machine)
                .Text("Mold", header: "Attachment", width: Widths.AnsiChars(8), settings: mold)
                .Numeric("PcsPerHour", header: "Pcs/hr", integer_places: 5, decimal_places: 1, iseditingreadonly: true)
                .Numeric("Sewer", header: "Sewer", integer_places: 2, decimal_places: 1, iseditingreadonly: true)
                .Numeric("IETMSSMV", header: "Std. SMV", integer_places: 3, decimal_places: 4, iseditingreadonly: true)
                .Numeric("SeamLength", header: "Sewing length", integer_places: 7, decimal_places: 2, iseditingreadonly: true);
        }

        private void ChangeToEmptyData(DataRow dr)
        {
            dr["OperationID"] = "";
            dr["OperationDescEN"] = "";
            dr["MachineTypeID"] = "";
            dr["Mold"] = "";
            dr["OperationMtlFactorID"] = "";
            dr["Frequency"] = 0;
            dr["SeamLength"] = 0;
            dr["SMV"] = 0;
            dr["IETMSSMV"] = 0;
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Phase"] = "Estimate";
            CurrentMaintain["Version"] = "01";
        }

        protected override bool ClickCopyBefore()
        {
            Sci.Production.IE.P01_Copy callNextForm = new Sci.Production.IE.P01_Copy(CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                styleID = callNextForm.P01CopyStyleData["ID"].ToString();
                seasonID = callNextForm.P01CopyStyleData["SeasonID"].ToString();
                brandID = callNextForm.P01CopyStyleData["BrandID"].ToString();
                comboType = callNextForm.P01CopyStyleData["Location"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            CurrentMaintain["StyleID"] = styleID;
            CurrentMaintain["SeasonID"] = seasonID;
            CurrentMaintain["BrandID"] = brandID;
            CurrentMaintain["ComboType"] = comboType;
            CurrentMaintain["Version"] = "01";
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Check.Seek(string.Format(@"select ID from SewingOutput_Detail where OrderId in (select ID from Orders where StyleID = '{0}' and BrandID = '{1}' and SeasonID = '{2}')", CurrentMaintain["StyleID"].ToString(), CurrentMaintain["BrandID"].ToString(), CurrentMaintain["SeasonID"].ToString())))
            {
                MyUtility.Msg.WarningBox("Sewing output > 0, can't be deleted!!");
                return false;
            }
            return true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["StyleID"]))
            {
                MyUtility.Msg.WarningBox("Style can't empty");
                textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SeasonID"]))
            {
                MyUtility.Msg.WarningBox("Season can't empty");
                txtseason1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can't empty");
                textBox2.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ComboType"]))
            {
                MyUtility.Msg.WarningBox("Combo type can't empty");
                comboBox1.Focus();
                return false;
            }
            #endregion
            #region 檢查輸入的資料是否存在系統
            if (!MyUtility.Check.Seek(string.Format("select sl.Location from Style s, Style_Location sl where s.ID = '{0}' and s.SeasonID = '{1}' and s.BrandID = '{2}' and s.Ukey = sl.StyleUkey and sl.Location = '{3}'", CurrentMaintain["StyleID"].ToString(), CurrentMaintain["SeasonID"].ToString(), CurrentMaintain["BrandID"].ToString(), CurrentMaintain["ComboType"].ToString())))
            {
                MyUtility.Msg.WarningBox("This style not correct, can't save");
                return false;
            }
            #endregion
            #region 檢查表身不可為空
            if (((DataTable)detailgridbs.DataSource).DefaultView.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty");
                return false;
            }
            #endregion
            //回寫表頭的Total Sewing Time與表身的Sewer
            decimal ttlSewingTime = MyUtility.Convert.GetDecimal(((DataTable)detailgridbs.DataSource).Compute("sum(SMV)", ""));
            CurrentMaintain["TotalSewingTime"] = MyUtility.Convert.GetInt(ttlSewingTime);
            decimal allSewer = MyUtility.Check.Empty(CurrentMaintain["NumberSewer"]) ? 0.0m : MyUtility.Convert.GetDecimal(CurrentMaintain["NumberSewer"]);
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["Sewer"] = ttlSewingTime == 0 ? 0 : MyUtility.Math.Round(allSewer * (MyUtility.Convert.GetDecimal(dr["SMV"]) / ttlSewingTime), 1);
                }
            }
            return base.ClickSaveBefore();
        }

        //Style
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@brandid";
            sp1.Value = MyUtility.Convert.GetString(CurrentMaintain["BrandID"]);

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string sqlCmd = "select ID,SeasonID,Description,BrandID,UKey from Style where Junk = 0 order by ID";
            if (!MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                sqlCmd = "select ID,SeasonID,Description,BrandID,UKey from Style where Junk = 0 and BrandID = @brandid order by ID";
            }

            DataTable styleData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return;
            }

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(styleData, "ID,SeasonID,Description,BrandID", "16,10,50,8", this.Text,headercaptions: "Style,Season,Description,Brand");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            IList<DataRow> selectedData = item.GetSelecteds();
            CurrentMaintain["StyleID"] = item.GetSelectedString();
            CurrentMaintain["SeasonID"] = (selectedData[0])["SeasonID"].ToString();
            CurrentMaintain["BrandID"] = (selectedData[0])["BrandID"].ToString();

            sqlCmd = string.Format("select Location from Style_Location where StyleUkey = {0}", MyUtility.Convert.GetInt((selectedData[0])["UKey"]).ToString());
            DataTable LocationData;
            result = DBProxy.Current.Select(null,sqlCmd,out LocationData);
            if (result)
            {
                if (LocationData.Rows.Count == 1)
                {
                    CurrentMaintain["ComboType"] = LocationData.Rows[0]["Location"].ToString();
                }
            }
        }

        //Brand
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "SELECT Id,NameCH,NameEN FROM Brand WHERE Junk=0  ORDER BY Id";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlWhere, "10,50,50", this.Text, false, ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            CurrentMaintain["BrandID"] = item.GetSelectedString();
        }

        //Art. Sum
        private void button5_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01_ArtworkSummary callNextForm = new Sci.Production.IE.P01_ArtworkSummary("TimeStudy_Detail", Convert.ToInt64(CurrentMaintain["ID"]));
            DialogResult result = callNextForm.ShowDialog(this);
        }

        //Sketch
        private void button6_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01_Sketch callNextForm = new Sci.Production.IE.P01_Sketch(CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
        }

        //New Version
        private void button1_Click(object sender, EventArgs e)
        {
            //將現有資料寫入TimeStudyHistory,TimeStudyHistory_History，並將現有資料的Version+1
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to create new version?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult == System.Windows.Forms.DialogResult.Yes)
            {
                string executeCmd = string.Format(@"insert into TimeStudyHistory (StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion)
select StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion from TimeStudy where ID = {0}

declare @id bigint
select @id = @@IDENTITY

insert into TimeStudyHistory_Detail(ID,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV)
select @id,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV from TimeStudy_Detail where ID = {0}

update TimeStudy 
set Version = (select iif(isnull(max(Version),0)+1 < 10,'0'+cast(isnull(max(Version),0)+1 as varchar),cast(max(Version)+1as varchar)) from TimeStudy where ID = {0}) ,
    AddName = '{1}',
	AddDate = GETDATE(),
	EditName = '',
	EditDate = null
where ID = {0}",CurrentMaintain["ID"].ToString(),Sci.Env.User.UserID);
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Execute(null, executeCmd);
                        if (result)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Click new version  failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MyUtility.Msg.ErrorBox("Connection transaction error.\r\n" + ex.ToString());
                        return;
                    }
                }
                RenewData();
                ClickEdit();
            }
        }

        //New Status
        private void button2_Click(object sender, EventArgs e)
        {
            if (CurrentMaintain["Phase"].ToString() == "Final")
            {
                MyUtility.Msg.WarningBox("Can't change status!!");
                return;
            }
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to create new status?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult == System.Windows.Forms.DialogResult.Yes)
            {
                string executeCmd = string.Format(@"insert into TimeStudyHistory (StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion)
select StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion from TimeStudy where ID = {0}

declare @id bigint
select @id = @@IDENTITY

insert into TimeStudyHistory_Detail(ID,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV)
select @id,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV from TimeStudy_Detail where ID = {0}

declare @phase varchar(10)
select @phase = isnull(Phase,'') from TimeStudy where ID = {0}

update TimeStudy 
set Phase = iif(@phase = 'Estimate','Initial',iif(@phase = 'Initial','Prelim',iif(@phase = 'Prelim','Final','Estimate'))),
	Version = '01',
	EditName = '{1}',
	EditDate = GETDATE()
where ID = {0}", CurrentMaintain["ID"].ToString(), Sci.Env.User.UserID);
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Execute(null, executeCmd);
                        if (result)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Click new version  failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MyUtility.Msg.ErrorBox("Connection transaction error.\r\n" + ex.ToString());
                        return;
                    }
                }
                RenewData();
                ClickEdit();
            }
        }

        //Copy from style std. GSD
        private void button7_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(textBox1.Text) || MyUtility.Check.Empty(comboBox1.Text) || MyUtility.Check.Empty(textBox2.Text) || MyUtility.Check.Empty(txtseason1.Text))
            {
                MyUtility.Msg.WarningBox("< Style > or < Combo Type > or < Season > or < Brand > can't empty!!");
                return;
            }
            //表身有資料時，就必須先問是否要將表身資料全部刪除
            if (((DataTable)detailgridbs.DataSource).DefaultView.Count > 0)
            {
                DialogResult confirmResult;
                confirmResult = MyUtility.Msg.QuestionBox("Detail data have operation code now! Are you sure you want to erase it?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
                if (confirmResult != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            #region 設定sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = textBox1.Text;
            sp2.ParameterName = "@seasonid";
            sp2.Value = txtseason1.Text;
            sp3.ParameterName = "@brandid";
            sp3.Value = textBox2.Text;
            sp4.ParameterName = "@location";
            sp4.Value = comboBox1.Text;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);
            #endregion

            DataTable ietmsData;
            string sqlCmd = @"select id.SEQ,id.OperationID,o.DescEN as OperationDescEN,id.Annotation,
iif(round(id.SMV*(isnull(m.Rate,0)/100+1)*id.Frequency*60,3) = 0,0,round(3600/round(id.SMV*(isnull(m.Rate,0)/100+1)*id.Frequency*60,3),1)) as PcsPerHour,
id.Frequency as Sewer,o.MachineTypeID,id.Frequency,
id.SMV*(isnull(m.Rate,0)/100+1)*id.Frequency as IETMSSMV,id.Mold,o.MtlFactorID as OperationMtlFactorID,
round(id.SMV*(isnull(m.Rate,0)/100+1)*id.Frequency*60,3) as SMV, id.SeamLength,s.IETMSID,s.IETMSVersion
from Style s
inner join IETMS i on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id on i.IETMSUkey = id.IETMSUkey
left join Operation o on id.OperationID = o.ID
left join MtlFactor m on o.MtlFactorID = m.ID and m.Type = 'F'
where s.ID = @id and s.SeasonID = @seasonid and s.BrandID = @brandid and id.Location = @location
order by id.SEQ";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out ietmsData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query ietms fail!\r\n"+result.ToString());
                return;
            }
            //刪除原有資料
            foreach (DataRow dr in DetailDatas)
            {
                dr.Delete();
            }

            //將IETMS_Detail資料寫入表身
            foreach (DataRow dr in ietmsData.Rows)
            {
                dr.AcceptChanges();
                dr.SetAdded();
                ((DataTable)detailgridbs.DataSource).ImportRow(dr);
            }
            if (ietmsData.Rows.Count > 0)
            {
                CurrentMaintain["IETMSID"] = ietmsData.Rows[0]["IETMSID"].ToString();
                CurrentMaintain["IETMSVersion"] = ietmsData.Rows[0]["IETMSVersion"].ToString();
            }
        }

        //History
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01_History callNextForm = new Sci.Production.IE.P01_History(CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
        }

        //Std. GSD List
        private void button8_Click(object sender, EventArgs e)
        {
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = CurrentMaintain["StyleID"].ToString();
            sp2.ParameterName = "@seasonid";
            sp2.Value = CurrentMaintain["SeasonID"].ToString();
            sp3.ParameterName = "@brandid";
            sp3.Value = CurrentMaintain["BrandID"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);

            string sqlCmd = "select Ukey from Style where ID = @id and SeasonID = @seasonid and BrandID = @brandid";
            DataTable styleUkey;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleUkey);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return;
            }

            if (styleUkey.Rows.Count > 0)
            {
                Sci.Production.PublicForm.StdGSDList callNextForm = new Sci.Production.PublicForm.StdGSDList(MyUtility.Convert.GetLong(styleUkey.Rows[0]["UKey"]));
                DialogResult dresult = callNextForm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.WarningBox("Style not found!!");
            }
        }

        //Insert、Append
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            if (index == -1)
            {
                int seq = 0;
                if (((DataTable)detailgridbs.DataSource).DefaultView.Count > 1)
                {
                    seq = MyUtility.Convert.GetInt(((DataTable)detailgridbs.DataSource).Compute("max(seq)", ""));
                }
                CurrentDetailData["Seq"] = MyUtility.Convert.GetString(seq + 10).PadLeft(4, '0');
            }
            else
            {
                DataRow dr = DetailDatas[index + 1];
                CurrentDetailData["Seq"] = dr["Seq"];
                int seq = MyUtility.Convert.GetInt(dr["Seq"]);
                for (int i = index + 1; i < DetailDatas.Count; i++)
                {
                    seq += 10;
                    DetailDatas[i]["Seq"] = MyUtility.Convert.GetString(seq).PadLeft(4, '0');
                }
            }
        }

        //Copy
        private void button4_Click(object sender, EventArgs e)
        {
            //將要Copy的資料記錄起來
            List<DataRow> listDr = new List<DataRow>();
            DataRow lastRow=null;
            int index = -1,lastIndex=0;
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                index++;
                if (dr["Selected"].ToString() == "1")
                {
                    dr["Selected"] = 0;
                    lastIndex = index;
                    lastRow = dr;
                    listDr.Add(dr);
                }
            }
            detailgrid.ValidateControl();
            if (listDr.Count <= 0)
            {
                return;
            }
            //將要Copy的資料塞進DataTable中
            int newIndex = lastIndex;
            foreach (DataRow dr in listDr)
            {
                DataRow newRow = ((DataTable)detailgridbs.DataSource).NewRow();
                newRow.ItemArray = dr.ItemArray;
                ((DataTable)detailgridbs.DataSource).Rows.InsertAt(newRow, ++newIndex);
            }

            int seq = MyUtility.Convert.GetInt(lastRow["Seq"]);
            for (int i = lastIndex + 1; i < DetailDatas.Count; i++)
            {
                seq += 10;
                DetailDatas[i]["Seq"] = MyUtility.Convert.GetString(seq).PadLeft(4, '0');
            }
        }

    }
}
