using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class B40 : Win.Tems.Input1
    {
        private IList<DataRow> Subprocesslist;
        private DataTable DT_RFIDReader_Panel;

        /// <inheritdoc/>
        public B40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Comboload();
            this.comboRFIDProcessLocation.SetDataSource(false);
        }

        private void Comboload()
        {
            Dictionary<string, string> comboType_RowSource = new Dictionary<string, string>
            {
                { "1", "In" },
                { "2", "Out" },
                { "3", "In/Out" },
                { "4", "Out/In" },
            };
            this.comboStockType.DataSource = new BindingSource(comboType_RowSource, null);
            this.comboStockType.ValueMember = "Key";
            this.comboStockType.DisplayMember = "Value";

            this.comboMDivision.SetDefalutIndex();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.Subprocesslist = null;

            string sqlcmd = $@"
select rp.RFIDReaderID,rp.PanelNo,rp.CutCellID,rp.AddDate,rp.EditDate,AddName=dbo.GetPass1(rp.AddName),EditName=dbo.GetPass1(rp.EditName)
from RFIDReader_Panel rp with(nolock)
where rp.RFIDReaderID ='{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.DT_RFIDReader_Panel);
            if (!result)
            {
                this.ShowErr(result);
            }

            string sqlsubprocess = $@"select ProcessIDs = stuff((select concat(',',ProcessID)from RFIDReader_SubProcess with(nolock) where RFIDReaderID = '{this.CurrentMaintain["ID"]}' for xml path('')),1,1,'')";
            this.txtSubprocess.Text = MyUtility.GetValue.Lookup(sqlsubprocess);
        }

        /// <inheritdoc/>
        protected override void ClickCopyAfter()
        {
            this.txtSubprocess.Text = string.Empty;
            this.txtID.ReadOnly = false;
        }

        /// <inheritdoc/>
        protected override bool ClickNew()
        {
            this.txtID.ReadOnly = false;
            return base.ClickNew();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtID.Text))
            {
                MyUtility.Msg.WarningBox("ID can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtSubprocess.Text) || MyUtility.Check.Empty(this.comboStockType.Text))
            {
                MyUtility.Msg.WarningBox("Sub-process and Stock Type can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["MDivisionID"]))
            {
                MyUtility.Msg.WarningBox("M can not empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            if (this.Subprocesslist != null)
            {
                DataTable sourceDt = this.Subprocesslist.CopyToDataTable();
                DataColumn newColumn = new DataColumn("RFIDReaderID", typeof(string))
                {
                    DefaultValue = this.CurrentMaintain["ID"],
                };
                sourceDt.Columns.Add(newColumn);
                string in_update = $@"
select RFIDReaderID,ID into #tmp2 from #tmp
delete RFIDReader_SubProcess where RFIDReaderID = '{this.CurrentMaintain["ID"]}'
insert RFIDReader_SubProcess select * from #tmp2
;
";
                DualResult result = MyUtility.Tool.ProcessWithDatatable(sourceDt, string.Empty, in_update, out DataTable dt);
                if (!result)
                {
                    return result;
                }
            }

            if (this.DT_RFIDReader_Panel != null)
            {
                foreach (DataRow dr in this.DT_RFIDReader_Panel.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        dr["RFIDReaderID"] = this.CurrentMaintain["ID"];
                    }
                }

                string mergeRFIDReader_Panel = $@"
merge RFIDReader_Panel t
using #tmp s on t.[RFIDReaderID] = s.[RFIDReaderID] and t.[PanelNo] = s.[PanelNo]
when matched then update set
	t.[CutCellID] = s.[CutCellID],
	t.[EditDate]=getdate(),
	t.[EditName]='{Env.User.UserID}'
when not matched by target then
	insert ([RFIDReaderID],[PanelNo],[CutCellID],[AddDate],[AddName])
	values(s.[RFIDReaderID],s.[PanelNo],s.[CutCellID],s.[AddDate],'{Env.User.UserID}')
when not matched by source and t.[RFIDReaderID] = '{this.CurrentMaintain["id"]}' then
	delete
;
";
                DualResult result = MyUtility.Tool.ProcessWithDatatable(this.DT_RFIDReader_Panel, string.Empty, mergeRFIDReader_Panel, out DataTable dt);
                if (!result)
                {
                    return result;
                }
            }

            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.txtID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.txtID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override DualResult ClickDelete()
        {
            string delete = $@"
delete RFIDReader_Panel where RFIDReaderID = '{this.CurrentMaintain["ID"]}'
delete RFIDReader_SubProcess where RFIDReaderID =  '{this.CurrentMaintain["ID"]}'
";
            DBProxy.Current.Execute(null, delete);
            return base.ClickDelete();
        }

        private void TxtSewingLine_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLine.Text = this.SelectSewingLine(this.txtSewingLine.Text);
        }

        private string SelectSewingLine(string line)
        {
            List<SqlParameter> listSQLParameter = new List<SqlParameter>
            {
                new SqlParameter("@FactoryID", Env.User.Factory),
            };

            string sql = "Select Distinct ID From SewingLine WITH (NOLOCK) WHERE Junk != 1  AND FactoryID = @FactoryID ";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, listSQLParameter, "3", line, false, ",")
            {
                Width = 300,
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return string.Empty;
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        private void TxtSewingLine_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtSewingLine.Text))
            {
                List<SqlParameter> listSQLParameter = new List<SqlParameter>
                {
                    new SqlParameter("@FactoryID", Env.User.Factory),
                    new SqlParameter("@ID", this.txtSewingLine.Text),
                };

                string sqlcmd = "Select Distinct ID From SewingLine WITH (NOLOCK) WHERE Junk != 1  AND FactoryID =@FactoryID AND ID=@ID ";
                if (!MyUtility.Check.Seek(sqlcmd, listSQLParameter))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Sewing Line ID : {0} > not found!!!", this.txtSewingLine.Text));
                    this.txtSewingLine.Text = string.Empty;
                    this.txtSewingLine.Focus();
                    return;
                }
            }
        }

        private void BtnSetPanelCutcell_Click(object sender, EventArgs e)
        {
            string id = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);

            var callfrm = new B40_RFIDReaderSetting(this.EditMode, id, null, null, this.CurrentMaintain, this.DT_RFIDReader_Panel);
            callfrm.ShowDialog();
            this.DT_RFIDReader_Panel = callfrm.DetailDT;
        }

        private void TxtSubprocess_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select ID from Subprocess WITH (NOLOCK) where Junk = '0'";
            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlWhere, headercaptions: "Subprocess ID", columnwidths: "30", defaults: this.txtSubprocess.Text, defaultValueColumn: "ID");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Subprocesslist = item.GetSelecteds();
            this.txtSubprocess.Text = item.GetSelectedString();
        }
    }
}
