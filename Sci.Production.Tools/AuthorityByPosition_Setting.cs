using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using System.Linq;
using Ict;
using Ict.Win;
using Ict.Data;
using Sci;
using Sci.Win;
using Sci.Data;

namespace Sci.Production.Tools
{
    public partial class AuthorityByPosition_Setting : Sci.Win.Subs.Base
    {
        private DualResult result = null;
        private DataTable dtPass0 = null;
        private DataTable dtPass2 = null;
        private ITableSchema itsPass2 = null;
        private bool different = false;
        private DataTable dtMenuDetail = null;
        private DataRow[] drs = null;
        //private DataRow seekedData = null;
        private DataRow _refDr = null;
        private string sqlCmd = "";

        public AuthorityByPosition_Setting(Int64 pass0_PKey, DataRow refDr)
        {
            InitializeComponent();
            _refDr = refDr;

            sqlCmd = string.Format(@"SELECT Menu.MenuName, MenuDetail.*
                                                FROM Menu, MenuDetail 
                                                WHERE Menu.PKey = MenuDetail.UKey AND MenuDetail.PKey = {0}", (Int64)_refDr["FKMenu"]);
            if (result = DBProxy.Current.Select(null, sqlCmd, out dtMenuDetail))
            {
                this.checkNew.Enabled = (bool) dtMenuDetail.Rows[0]["CanNew"];
                this.checkEdit.Enabled = (bool) dtMenuDetail.Rows[0]["CanEdit"];
                this.checkDelete.Enabled = (bool) dtMenuDetail.Rows[0]["CanDelete"];
                this.checkPrint.Enabled = (bool) dtMenuDetail.Rows[0]["CanPrint"];
                this.checkConfirm.Enabled = (bool) dtMenuDetail.Rows[0]["CanConfirm"];
                this.checkUnConfirm.Enabled = (bool) dtMenuDetail.Rows[0]["CanUnConfirm"];
                this.checkSend.Enabled = (bool) dtMenuDetail.Rows[0]["CanSend"];
                this.checkRecall.Enabled = (bool) dtMenuDetail.Rows[0]["CanRecall"];
                this.checkCheck.Enabled = (bool) dtMenuDetail.Rows[0]["CanCheck"];
                this.checkUncheck.Enabled = (bool) dtMenuDetail.Rows[0]["CanUnCheck"];
                this.checkClose.Enabled = (bool) dtMenuDetail.Rows[0]["CanClose"];
                this.checkUnClose.Enabled = (bool) dtMenuDetail.Rows[0]["CanUnClose"];
                this.checkReceive.Enabled = (bool) dtMenuDetail.Rows[0]["CanReceive"];
                this.checkReturn.Enabled = (bool) dtMenuDetail.Rows[0]["CanReturn"];
                this.checkJunk.Enabled = (bool) dtMenuDetail.Rows[0]["CanJunk"];

                this.checkNew.Checked = this.checkNew.Enabled && Convert.ToBoolean(_refDr["CanNew"].ToString());
                this.checkEdit.Checked = this.checkEdit.Enabled && Convert.ToBoolean(_refDr["CanEdit"].ToString());
                this.checkDelete.Checked = this.checkDelete.Enabled && Convert.ToBoolean(_refDr["CanDelete"].ToString());
                this.checkPrint.Checked = this.checkPrint.Enabled && Convert.ToBoolean(_refDr["CanPrint"].ToString());
                this.checkConfirm.Checked = this.checkConfirm.Enabled && Convert.ToBoolean(_refDr["CanConfirm"].ToString());
                this.checkUnConfirm.Checked = this.checkUnConfirm.Enabled && Convert.ToBoolean(_refDr["CanUnConfirm"].ToString());
                this.checkSend.Checked = this.checkSend.Enabled && Convert.ToBoolean(_refDr["CanSend"].ToString());
                this.checkRecall.Checked = this.checkRecall.Enabled && Convert.ToBoolean(_refDr["CanRecall"].ToString());
                this.checkCheck.Checked = this.checkCheck.Enabled && Convert.ToBoolean(_refDr["CanCheck"].ToString());
                this.checkUncheck.Checked = this.checkUncheck.Enabled && Convert.ToBoolean(_refDr["CanUnCheck"].ToString());
                this.checkClose.Checked = this.checkClose.Enabled && Convert.ToBoolean(_refDr["CanClose"].ToString());
                this.checkUnClose.Checked = this.checkUnClose.Enabled && Convert.ToBoolean(_refDr["CanUnClose"].ToString());
                this.checkReceive.Checked = this.checkReceive.Enabled && Convert.ToBoolean(_refDr["CanReceive"].ToString());
                this.checkReturn.Checked = this.checkReturn.Enabled && Convert.ToBoolean(_refDr["CanReturn"].ToString());
                this.checkJunk.Checked = this.checkJunk.Enabled && Convert.ToBoolean(_refDr["CanJunk"].ToString());

            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                this.Close();
                return;
            }

            if (!(result = DBProxy.Current.Select(null, "SELECT * FROM Pass2", out dtPass2)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                this.Close();
                return;
            }

            if (result = DBProxy.Current.Select(null, "SELECT 0 AS Sele, PKey, ID, Description FROM Pass0 ORDER BY ID", out dtPass0))
            {
                drs = dtPass0.Select(string.Format("PKey = {0}", pass0_PKey));
                if (drs.Length > 0)
                {
                     drs[0]["Sele"] = 1;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                this.Close();
                return;
            }

            this.gridPositionAuthority.IsEditingReadOnly = false;
             Helper.Controls.Grid.Generator(this.gridPositionAuthority)
                 .CheckBox("Sele", header:"", width: Widths.AnsiChars(2), trueValue: 1, falseValue: 0)
                 .Text("ID", header: "Position", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true);

             listControlBindingSource1.DataSource = dtPass0;
             this.gridPositionAuthority.DataSource = listControlBindingSource1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(result = DBProxy.Current.GetTableSchema(null, "Pass2", out itsPass2)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }

            foreach (DataRow dr in dtPass0.Rows)
            {
                if ((int)dr["Sele"] == 1)
                {
                    drs = dtPass2.Select(string.Format("FKPass0 = {0} AND FKMenu = {1}", (Int64)dr["PKey"], (Int64)_refDr["FKMenu"]));
                    if (drs.Length > 0)
                    {
                        drs[0]["Used"] = "Y";
                        drs[0]["CanNew"] = this.checkNew.Value == "0" ? false : true;
                        drs[0]["CanEdit"] = this.checkEdit.Value == "0" ? false : true;
                        drs[0]["CanDelete"] = this.checkDelete.Value == "0" ? false : true;
                        drs[0]["CanPrint"] = this.checkPrint.Value == "0" ? false : true;
                        drs[0]["CanConfirm"] = this.checkConfirm.Value == "0" ? false : true;
                        drs[0]["CanUnConfirm"] = this.checkUnConfirm.Value == "0" ? false : true;
                        drs[0]["CanSend"] = this.checkSend.Value == "0" ? false : true;
                        drs[0]["CanRecall"] = this.checkRecall.Value == "0" ? false : true;
                        drs[0]["CanCheck"] = this.checkCheck.Value == "0" ? false : true;
                        drs[0]["CanUnCheck"] = this.checkUncheck.Value == "0" ? false : true;
                        drs[0]["CanClose"] = this.checkClose.Value == "0" ? false : true;
                        drs[0]["CanUnClose"] = this.checkUnClose.Value == "0" ? false : true;
                        drs[0]["CanReceive"] = this.checkReceive.Value == "0" ? false : true;
                        drs[0]["CanReturn"] = this.checkReturn.Value == "0" ? false : true;
                        drs[0]["CanJunk"] = this.checkJunk.Value == "0" ? false : true;
                    }
                    else
                    {
                        DataRow newRow = dtPass2.NewRow();
                        newRow["FKPass0"] = (Int64)dr["PKey"];
                        newRow["FKMenu"] = (Int64)_refDr["FKMenu"];
                        newRow["MenuName"] = dtMenuDetail.Rows[0]["MenuName"].ToString();
                        newRow["BarPrompt"] = dtMenuDetail.Rows[0]["BarPrompt"].ToString();
                        newRow["Used"] = "Y";
                        newRow["CanNew"] = this.checkNew.Value == "0" ? false : true;
                        newRow["CanEdit"] = this.checkEdit.Value == "0" ? false : true;
                        newRow["CanDelete"] = this.checkDelete.Value == "0" ? false : true;
                        newRow["CanPrint"] = this.checkPrint.Value == "0" ? false : true;
                        newRow["CanConfirm"] = this.checkConfirm.Value == "0" ? false : true;
                        newRow["CanUnConfirm"] = this.checkUnConfirm.Value == "0" ? false : true;
                        newRow["CanSend"] = this.checkSend.Value == "0" ? false : true;
                        newRow["CanRecall"] = this.checkRecall.Value == "0" ? false : true;
                        newRow["CanCheck"] = this.checkCheck.Value == "0" ? false : true;
                        newRow["CanUnCheck"] = this.checkUncheck.Value == "0" ? false : true;
                        newRow["CanClose"] = this.checkClose.Value == "0" ? false : true;
                        newRow["CanUnClose"] = this.checkUnClose.Value == "0" ? false : true;
                        newRow["CanReceive"] = this.checkReceive.Value == "0" ? false : true;
                        newRow["CanReturn"] = this.checkReturn.Value == "0" ? false : true;
                        newRow["CanJunk"] = this.checkJunk.Value == "0" ? false : true;
                        dtPass2.Rows.Add(newRow);
                    }
                }
            }

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    foreach (DataRow dr in dtPass2.Rows)
                    {
                        if (dr.RowState == DataRowState.Modified)
                        {
                            result = DBProxy.Current.UpdateByChanged(null, itsPass2, dr, out different);
                        }
                        if (dr.RowState == DataRowState.Added)
                        {
                            result = DBProxy.Current.Insert(null, itsPass2, dr);
                        }

                        if (!result.IsEmpty)
                        {
                            _transactionscope.Dispose();
                            MyUtility.Msg.ErrorBox("Update failed, Pleaes re-try");
                            return;
                        }
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Update successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    MyUtility.Msg.ErrorBox("Update transaction error.\n" + ex);
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.checkNew.Checked = false;
            this.checkEdit.Checked = false;
            this.checkDelete.Checked = false;
            this.checkPrint.Checked = false;
            this.checkConfirm.Checked = false;
            this.checkUnConfirm.Checked = false;
            this.checkSend.Checked = false;
            this.checkRecall.Checked = false;
            this.checkCheck.Checked = false;
            this.checkUncheck.Checked = false;
            this.checkClose.Checked = false;
            this.checkUnClose.Checked = false;
            this.checkReceive.Checked = false;
            this.checkReturn.Checked = false;
            this.checkJunk.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.checkNew.Checked = this.checkNew.Enabled;
            this.checkEdit.Checked = this.checkEdit.Enabled;
            this.checkDelete.Checked = this.checkDelete.Enabled;
            this.checkPrint.Checked = this.checkPrint.Enabled;
            this.checkConfirm.Checked = this.checkConfirm.Enabled;
            this.checkUnConfirm.Checked = this.checkUnConfirm.Enabled;
            this.checkSend.Checked = this.checkSend.Enabled;
            this.checkRecall.Checked = this.checkRecall.Enabled;
            this.checkCheck.Checked = this.checkCheck.Enabled;
            this.checkUncheck.Checked = this.checkUncheck.Enabled;
            this.checkClose.Checked = this.checkClose.Enabled;
            this.checkUnClose.Checked = this.checkUnClose.Enabled;
            this.checkReceive.Checked = this.checkReceive.Enabled;
            this.checkReturn.Checked = this.checkReturn.Enabled;
            this.checkJunk.Checked = this.checkJunk.Enabled;
        }
    }
}
