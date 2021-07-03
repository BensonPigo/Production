using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P63_CreateSeq : Sci.Win.Subs.Base
    {
        private DataTable dtResultImportData;
        private bool boolImport = false;
        private bool boolIsCreate;
        private DataRow drDetail;

        /// <inheritdoc/>
        public P63_CreateSeq(bool isCreate, DataRow dr = null)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.boolIsCreate = isCreate;
            this.drDetail = dr;
        }

        private void Initial()
        {
            this.Text = this.boolIsCreate ? "P63. Create Seq" : "P63. Seq(Modify)";
            this.btnCreate.Text = this.boolIsCreate ? "Create" : "Save";
            this.txtSP.ReadOnly = this.boolIsCreate ? false : true;
            this.txtSeq.ReadOnly = this.boolIsCreate ? false : true;
            this.txtSP.Text = this.boolIsCreate ? string.Empty : this.drDetail["POID"].ToString();
            this.txtSeq.Text = this.boolIsCreate ? string.Empty : this.drDetail["Seq"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Initial();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            string sqlInsert = string.Empty;
            switch (this.boolIsCreate)
            {
                case true:
                    if (MyUtility.Check.Empty(this.txtSP.Text) || MyUtility.Check.Empty(this.txtSeq.Text))
                    {
                        MyUtility.Msg.WarningBox("SP# and Seq cannot be empty.");
                        return;
                    }

                    string sqlSP = $@"select 1 from orders where id='{this.txtSP.Text}'";
                    if (!MyUtility.Check.Seek(sqlSP))
                    {
                        MyUtility.Msg.WarningBox($"SP# :{this.txtSP.Text} cannot be found!");
                        return;
                    }

                    string sqlExists = $@"select 1 from SemiFinished where PoID='{this.txtSP.Text}' and Seq = '{this.txtSeq.Text}'";
                    if (MyUtility.Check.Seek(sqlExists))
                    {
                        MyUtility.Msg.WarningBox($"SP# :{this.txtSP.Text},Seq: {this.txtSeq.Text} already existed.");
                        return;
                    }

                    sqlInsert = $@"
insert into SemiFinished(POID,Seq,Color,Unit,[Desc],AddName,AddDate)
values('{this.txtSP.Text}', '{this.txtSeq.Text}'
, '{this.txtColor.Text}', '{this.txtUnit.Text}'
, '{this.EditDesc.Text}', '{Sci.Env.User.UserID}', GETDATE())";
                    break;
                case false:
                    sqlInsert = $@"
update SemiFinished
set Color = '{this.txtColor.Text}'
,Unit = '{this.txtUnit.Text}'
,[Desc] = '{this.EditDesc.Text}'
,EditName = '{Sci.Env.User.UserID}'
,EditDate = GETDATE()
where POID = '{this.txtSP.Text}'
and Seq = '{this.txtSeq.Text}'
";
                    break;
            }

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlInsert)))
            {
                this.ShowErr(result);
                return;
            }

            this.dtResultImportData = new DataTable();
            DataRow dr;

            // 建立欄位
            this.dtResultImportData.Columns.Add("POID", typeof(string));
            this.dtResultImportData.Columns.Add("Seq", typeof(string));
            this.dtResultImportData.Columns.Add("Desc", typeof(string));
            this.dtResultImportData.Columns.Add("Color", typeof(string));
            this.dtResultImportData.Columns.Add("Unit", typeof(string));

            dr = this.dtResultImportData.NewRow();
            dr["POID"] = this.txtSP.Text;
            dr["Seq"] = this.txtSeq.Text;
            dr["Desc"] = this.EditDesc.Text;
            dr["Color"] = this.txtColor.Text;
            dr["Unit"] = this.txtUnit.Text;
            this.dtResultImportData.Rows.Add(dr);

            if (this.dtResultImportData != null && this.dtResultImportData.Rows.Count != 0)
            {
                this.boolImport = true;
                this.Close();
            }
        }

        /// <inheritdoc/>
        public bool GetBoolImport()
        {
            return this.boolImport;
        }

        /// <inheritdoc/>
        public DataTable GetResultImportDatas()
        {
            return this.dtResultImportData;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
