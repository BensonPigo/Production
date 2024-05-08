using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class IE_B05 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public IE_B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridIcon1.Insert.Visible = false;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            if (this.EditMode == true)
            {
                this.gridDetail.IsEditingReadOnly = false;
            }
            else
            {
                this.gridDetail.IsEditingReadOnly = true;
            }

            string sql = string.Format("select * from [MachineType_ThreadRatio] where ID='{0}'", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sql, this.ConnectionName))
            {
                this.btnThreadRatio.ForeColor = Color.Blue;
            }
            else
            {
                this.btnThreadRatio.ForeColor = DefaultForeColor;
            }

            string sqlQuery = $@"
select * 
from ProductionTPE.dbo.MachineType_Detail
where ID = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Select("Trade", sqlQuery, out DataTable dt);
            if (result == false)
            {
                this.ShowErr(result);
                this.listControlBindingSource1.DataSource = null;
                return;
            }
            else
            {
                this.listControlBindingSource1.DataSource = dt;
            }
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.gridDetail != null)
            {
                if (this.EditMode == true)
                {
                    this.gridDetail.IsEditingReadOnly = false;
                }
                else
                {
                    this.gridDetail.IsEditingReadOnly = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.EditMode == true)
            {
                this.gridDetail.IsEditingReadOnly = false;
            }
            else
            {
                this.gridDetail.IsEditingReadOnly = true;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region 新增Copy from Other Factory
            Win.UI.Button btnCopyfrom = new Win.UI.Button();
            btnCopyfrom.Text = "Copy From Other Factory";
            btnCopyfrom.Size = new Size(190, 30);
            btnCopyfrom.Click += new EventHandler(this.BtnCopyFrom_Click);
            this.browsetop.Controls.Add(btnCopyfrom);

            #endregion

            #region Factory
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();

            // 右鍵開窗
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentMaintain == null || e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);

                    string item_cmd = "select ID from [Trade].dbo.Factory where IsSCI = 1";
                    DBProxy.Current.Select("Trade", item_cmd, out DataTable dt);
                    SelectItem item = new SelectItem(dt, "ID", "13", dr["FactoryID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["FactoryID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            ts.CellValidating += (s, e) =>
            {
                if (this.CurrentMaintain == null || e.RowIndex == -1 || this.EditMode == false)
                {
                    return;
                }

                DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
                string oldvalue = dr["FactoryID"].ToString();
                string newvalue = e.FormattedValue.ToString();

                if (oldvalue.Equals(newvalue))
                {
                    return;
                }

                string sqlcmd = $@"select ID from [Trade].dbo.Factory where IsSCI = 1 and id = '{newvalue}'";
                if (MyUtility.Check.Seek(sqlcmd, "Trade"))
                {
                    dr["FactoryID"] = newvalue;
                    dr.EndEdit();
                }
                else
                {
                    dr["FactoryID"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($@"<Factory: {e.FormattedValue}> doesn't exist in Data!");
                }
            };

            #endregion

            DataGridViewGeneratorCheckBoxColumnSettings isSubprocessSetting = new DataGridViewGeneratorCheckBoxColumnSettings();
            isSubprocessSetting.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
                int newvalue = Convert.ToInt16(e.FormattedValue);
                dr["IsSubprocess"] = newvalue;
                dr["IsNonSewingLine"] = newvalue;
                dr.EndEdit();
            };

            // Set Grid
            this.gridDetail.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
             .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(13), settings: ts, iseditingreadonly: false)
                .CheckBox("IsSubprocess", header: "Subprocess", width: Widths.AnsiChars(15), iseditable: true, trueValue: true, falseValue: false, settings: isSubprocessSetting)
                .CheckBox("IsNonSewingLine", header: "Non-Sewing Line", width: Widths.AnsiChars(17), iseditable: true, trueValue: true, falseValue: false)
                .CheckBox("IsNotShownInP01", header: "Not shown in P01", width: Widths.AnsiChars(17), iseditable: true, trueValue: true, falseValue: false)
                .CheckBox("IsNotShownInP03", header: "Not shown in P03", width: Widths.AnsiChars(17), iseditable: true, trueValue: true, falseValue: false)
                .CheckBox("IsNotShownInP05", header: "Not shown in P05", width: Widths.AnsiChars(17), iseditable: true, trueValue: true, falseValue: false)
                .CheckBox("IsNotShownInP06", header: "Not shown in P06", width: Widths.AnsiChars(17), iseditable: true, trueValue: true, falseValue: false)
            ;
        }

        private void BtnCopyFrom_Click(object sender, EventArgs e)
        {
            IE_B05_CopyFromOtherFactory callForm = new IE_B05_CopyFromOtherFactory();
            callForm.ShowDialog(this);
            this.ReloadDatas();
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("detail row is empty!");
                return false;
            }

            #region 存檔前判斷
            var upd_list = dt.AsEnumerable().Where(x => x["FactoryID"].ToString().Empty()).ToList();
            if (upd_list.Count > 0)
            {
                MyUtility.Msg.WarningBox("Factory cannot be empty!");
                return false;
            }

            if (dt.AsEnumerable().Where(o => MyUtility.Convert.GetBool(o["IsSubprocess"]) && !MyUtility.Convert.GetBool(o["IsNonSewingLine"])).Any())
            {
                MyUtility.Msg.InfoBox("Non-Sewing Line has to be checked since Subprocess is checked.");
                return false;
            }
            #endregion
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            // 修改表身資料,不寫入表頭EditName and EditDate
            ITableSchema dtSchema;
            var ok = DBProxy.Current.GetTableSchema("Trade", "MachineType", out dtSchema);
            dtSchema.IsSupportEditDate = false;
            dtSchema.IsSupportEditName = false;

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            #region 手動存檔
            if (dt != null && dt.Rows.Count > 0)
            {
                string sqlUpdate = $@"
merge ProductionTPE.dbo.MachineType_Detail as t
using #tmp as s
on t.id = s.id and t.FactoryID = s.FactoryID
when matched then update set 
		t.IsSubprocess = s.IsSubprocess,
		t.IsNonSewingLine = s.IsNonSewingLine,
		t.IsNotShownInP01 = s.IsNotShownInP01,
		t.IsNotShownInP03 = s.IsNotShownInP03,
        t.IsNotShownInP05 = s.IsNotShownInP05,
        t.IsNotShownInP06 = s.IsNotShownInP06
when not matched by target then
	insert([ID]
      ,[FactoryID]
      ,[IsSubprocess]
      ,[IsNonSewingLine]
      ,[IsNotShownInP01]
      ,[IsNotShownInP03]
      ,[IsNotShownInP05]
      ,[IsNotShownInP06])
	values(
	   s.[ID]
      ,s.[FactoryID]
      ,s.[IsSubprocess]
      ,s.[IsNonSewingLine]
      ,s.[IsNotShownInP01]
      ,s.[IsNotShownInP03]
      ,s.[IsNotShownInP05]
      ,s.[IsNotShownInP06])
when not matched by source and t.ID = '{this.CurrentMaintain["ID"]}' then
delete;
;	
";
                SqlConnection sqlConn = null;
                DBProxy.Current.OpenConnection("Trade", out sqlConn);
                DualResult result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlUpdate, out DataTable data, conn: sqlConn);
                if (result == false)
                {
                    return Ict.Result.F(result.ToString());
                }
            }

            #endregion
            return Ict.Result.True;
        }

        private void BtnThreadRatio_Click(object sender, EventArgs e)
        {
            IE_B05_ThreadRatio callNextForm = new IE_B05_ThreadRatio(this.CurrentMaintain["ID"].ToString());
            DialogResult result = callNextForm.ShowDialog(this);
        }

        private void GridIcon1_AppendClick(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            this.gridDetail.ValidateControl();
            this.listControlBindingSource1.EndEdit();

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow drData = dt.NewRow();
            drData["ID"] = this.CurrentMaintain["ID"].ToString();
            drData["IsSubprocess"] = false;
            drData["IsNonSewingLine"] = false;
            drData["IsNotShownInP01"] = false;
            drData["IsNotShownInP03"] = false;
            drData["IsNotShownInP05"] = false;
            drData["IsNotShownInP06"] = false;
            dt.Rows.InsertAt(drData, 0);
            dt.AcceptChanges();
        }

        private void GridIcon1_RemoveClick(object sender, EventArgs e)
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            this.gridDetail.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow drSelect = this.gridDetail.GetDataRow(this.listControlBindingSource1.Position);
            if (drSelect != null)
            {
                drSelect.Delete();
                dt.AcceptChanges();
            }
        }
    }
}
