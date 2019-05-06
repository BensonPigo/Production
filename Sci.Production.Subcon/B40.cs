using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.Subcon
{
    public partial class B40 : Sci.Win.Tems.Input1
    {
        IList<DataRow> Subprocesslist;
        public B40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            comboload();
            this.comboRFIDProcessLocation.setDataSource();
        }

        private void comboload()
        {            
            DataTable dtSubprocessID;
            //DualResult Result;
            //if (Result = DBProxy.Current.Select(null, "select ID from Subprocess WITH (NOLOCK) where Junk = '0'", out dtSubprocessID))
            //{
            //    this.comboSubprocess.DataSource = dtSubprocessID;
            //    this.comboSubprocess.DisplayMember = "ID";
            //}
            //else { ShowErr(Result); }

            Dictionary<String, String> comboType_RowSource = new Dictionary<string, string>();
            comboType_RowSource.Add("1", "In");
            comboType_RowSource.Add("2", "Out");
            comboType_RowSource.Add("3", "In/Out");
            comboStockType.DataSource = new BindingSource(comboType_RowSource, null);
            comboStockType.ValueMember = "Key";
            comboStockType.DisplayMember = "Value";

            this.comboMDivision.setDefalutIndex();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            Subprocesslist = null;
            string sqlsubprocess = $@"select ProcessIDs = stuff((select concat(',',ProcessID)from RFIDReader_SubProcess with(nolock) where RFIDReaderID = '{this.CurrentMaintain["ID"]}' for xml path('')),1,1,'')";
            this.txtSubprocess.Text = MyUtility.GetValue.Lookup(sqlsubprocess);
        }

        protected override void ClickCopyAfter()
        {
            txtID.ReadOnly = false;
        }

        protected override bool ClickNew()
        {
            txtID.ReadOnly = false;
            return base.ClickNew();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(txtID.Text))
            {
                MyUtility.Msg.WarningBox("ID can not empty!");
                return false;
            }
            if (MyUtility.Check.Empty(txtSubprocess.Text)|| MyUtility.Check.Empty(comboStockType.Text))
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

        protected override DualResult ClickSave()
        {
            if (Subprocesslist != null)
            {

                DataTable sourceDt = Subprocesslist.CopyToDataTable();
                System.Data.DataColumn newColumn = new System.Data.DataColumn("RFIDReaderID", typeof(System.String));
                newColumn.DefaultValue = this.CurrentMaintain["ID"];
                sourceDt.Columns.Add(newColumn);
                string in_update = $@"
select RFIDReaderID,ID into #tmp2 from #tmp
delete RFIDReader_SubProcess where RFIDReaderID = '{this.CurrentMaintain["ID"]}'
insert RFIDReader_SubProcess select * from #tmp2
;
";
                DataTable dt;
                DualResult result = MyUtility.Tool.ProcessWithDatatable(sourceDt, string.Empty, in_update, out dt);
                if (!result)
                {
                    return result;
                }
            }

            return base.ClickSave();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            txtID.ReadOnly = true;
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.txtID.ReadOnly = true;
        }
        
        private void txtSewingLine_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.txtSewingLine.Text = this.SelectSewingLine(this.txtSewingLine.Text);

        }

        private string SelectSewingLine(string line)
        {
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@FactoryID", Sci.Env.User.Factory));

            string sql = "Select Distinct ID From SewingLine WITH (NOLOCK) WHERE Junk != 1  AND FactoryID = @FactoryID ";

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, listSQLParameter, "3", line, false, ",");
            item.Width = 300;
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

        private void txtSewingLine_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtSewingLine.Text))
            {
                DataRow dr;

                List<SqlParameter> listSQLParameter = new List<SqlParameter>();
                listSQLParameter.Add(new SqlParameter("@FactoryID", Sci.Env.User.Factory));
                listSQLParameter.Add(new SqlParameter("@ID", this.txtSewingLine.Text));

                string sqlcmd ="Select Distinct ID From SewingLine WITH (NOLOCK) WHERE Junk != 1  AND FactoryID =@FactoryID AND ID=@ID ";
                if (MyUtility.Check.Seek(sqlcmd, listSQLParameter, out dr) == false)
                {
                    MyUtility.Msg.WarningBox(string.Format("< Sewing Line ID : {0} > not found!!!", this.txtSewingLine.Text));
                    this.txtSewingLine.Text = string.Empty;
                    this.txtSewingLine.Focus();
                    return;
                }
            }
        }

        private void btnSetPanelCutcell_Click(object sender, EventArgs e)
        {
            string id = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            var callfrm = new B40_RFIDReaderSetting(this.Perm.Edit, id, null, null,this.CurrentMaintain);
            callfrm.ShowDialog();
        }

        private void txtSubprocess_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select ID from Subprocess WITH (NOLOCK) where Junk = '0'";
            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(sqlWhere, headercaptions: "Subprocess ID", columnwidths: "30", defaults: this.txtSubprocess.Text, defaultValueColumn: "ID");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) return;
            Subprocesslist = item.GetSelecteds();
            this.txtSubprocess.Text = item.GetSelectedString();
        }
    }
}
