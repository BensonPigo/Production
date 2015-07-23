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
        private DataRow seekedData = null;
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
                //dtMenuDetail.PrimaryKey = new DataColumn[] { dtMenuDetail.Columns["PKey"] };
                this.checkBox1.Enabled = (bool) dtMenuDetail.Rows[0]["CanNew"];
                this.checkBox2.Enabled = (bool) dtMenuDetail.Rows[0]["CanEdit"];
                this.checkBox3.Enabled = (bool) dtMenuDetail.Rows[0]["CanDelete"];
                this.checkBox4.Enabled = (bool) dtMenuDetail.Rows[0]["CanPrint"];
                this.checkBox5.Enabled = (bool) dtMenuDetail.Rows[0]["CanConfirm"];
                this.checkBox6.Enabled = (bool) dtMenuDetail.Rows[0]["CanUnConfirm"];
                this.checkBox7.Enabled = (bool) dtMenuDetail.Rows[0]["CanSend"];
                this.checkBox8.Enabled = (bool) dtMenuDetail.Rows[0]["CanRecall"];
                this.checkBox9.Enabled = (bool) dtMenuDetail.Rows[0]["CanCheck"];
                this.checkBox10.Enabled = (bool) dtMenuDetail.Rows[0]["CanUnCheck"];
                this.checkBox11.Enabled = (bool) dtMenuDetail.Rows[0]["CanClose"];
                this.checkBox12.Enabled = (bool) dtMenuDetail.Rows[0]["CanUnClose"];
                this.checkBox13.Enabled = (bool) dtMenuDetail.Rows[0]["CanReceive"];
                this.checkBox14.Enabled = (bool) dtMenuDetail.Rows[0]["CanReturn"];
                this.checkBox15.Enabled = (bool) dtMenuDetail.Rows[0]["CanJunk"];

                this.checkBox1.Checked = this.checkBox1.Enabled && Convert.ToBoolean(_refDr["CanNew"].ToString());
                this.checkBox2.Checked = this.checkBox2.Enabled && Convert.ToBoolean(_refDr["CanEdit"].ToString());
                this.checkBox3.Checked = this.checkBox3.Enabled && Convert.ToBoolean(_refDr["CanDelete"].ToString());
                this.checkBox4.Checked = this.checkBox4.Enabled && Convert.ToBoolean(_refDr["CanPrint"].ToString());
                this.checkBox5.Checked = this.checkBox5.Enabled && Convert.ToBoolean(_refDr["CanConfirm"].ToString());
                this.checkBox6.Checked = this.checkBox6.Enabled && Convert.ToBoolean(_refDr["CanUnConfirm"].ToString());
                this.checkBox7.Checked = this.checkBox7.Enabled && Convert.ToBoolean(_refDr["CanSend"].ToString());
                this.checkBox8.Checked = this.checkBox8.Enabled && Convert.ToBoolean(_refDr["CanRecall"].ToString());
                this.checkBox9.Checked = this.checkBox9.Enabled && Convert.ToBoolean(_refDr["CanCheck"].ToString());
                this.checkBox10.Checked = this.checkBox10.Enabled && Convert.ToBoolean(_refDr["CanUnCheck"].ToString());
                this.checkBox11.Checked = this.checkBox11.Enabled && Convert.ToBoolean(_refDr["CanClose"].ToString());
                this.checkBox12.Checked = this.checkBox12.Enabled && Convert.ToBoolean(_refDr["CanUnClose"].ToString());
                this.checkBox13.Checked = this.checkBox13.Enabled && Convert.ToBoolean(_refDr["CanReceive"].ToString());
                this.checkBox14.Checked = this.checkBox14.Enabled && Convert.ToBoolean(_refDr["CanReturn"].ToString());
                this.checkBox15.Checked = this.checkBox15.Enabled && Convert.ToBoolean(_refDr["CanJunk"].ToString());

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

            this.grid1.IsEditingReadOnly = false;
             Helper.Controls.Grid.Generator(this.grid1)
                 .CheckBox("Sele", header:"", width: Widths.AnsiChars(2), trueValue: 1, falseValue: 0)
                 .Text("ID", header: "Position", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true);

             listControlBindingSource1.DataSource = dtPass0;
             this.grid1.DataSource = listControlBindingSource1;
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
                        drs[0]["CanNew"] = this.checkBox1.Value == "0" ? false : true;
                        drs[0]["CanEdit"] = this.checkBox2.Value == "0" ? false : true;
                        drs[0]["CanDelete"] = this.checkBox3.Value == "0" ? false : true;
                        drs[0]["CanPrint"] = this.checkBox4.Value == "0" ? false : true;
                        drs[0]["CanConfirm"] = this.checkBox5.Value == "0" ? false : true;
                        drs[0]["CanUnConfirm"] = this.checkBox6.Value == "0" ? false : true;
                        drs[0]["CanSend"] = this.checkBox7.Value == "0" ? false : true;
                        drs[0]["CanRecall"] = this.checkBox8.Value == "0" ? false : true;
                        drs[0]["CanCheck"] = this.checkBox9.Value == "0" ? false : true;
                        drs[0]["CanUnCheck"] = this.checkBox10.Value == "0" ? false : true;
                        drs[0]["CanClose"] = this.checkBox11.Value == "0" ? false : true;
                        drs[0]["CanUnClose"] = this.checkBox12.Value == "0" ? false : true;
                        drs[0]["CanReceive"] = this.checkBox13.Value == "0" ? false : true;
                        drs[0]["CanReturn"] = this.checkBox14.Value == "0" ? false : true;
                        drs[0]["CanJunk"] = this.checkBox15.Value == "0" ? false : true;
                    }
                    else
                    {
                        //seekedData = dtMenuDetail.Rows.Find((Int64)_refDr["FKMenu"]);

                        DataRow newRow = dtPass2.NewRow();
                        newRow["FKPass0"] = (Int64)dr["PKey"];
                        newRow["FKMenu"] = (Int64)_refDr["FKMenu"];
                        newRow["MenuName"] = dtMenuDetail.Rows[0]["MenuName"].ToString();
                        newRow["BarPrompt"] = dtMenuDetail.Rows[0]["BarPrompt"].ToString();
                        newRow["Used"] = "Y";
                        newRow["CanNew"] = this.checkBox1.Value == "0" ? false : true;
                        newRow["CanEdit"] = this.checkBox2.Value == "0" ? false : true;
                        newRow["CanDelete"] = this.checkBox3.Value == "0" ? false : true;
                        newRow["CanPrint"] = this.checkBox4.Value == "0" ? false : true;
                        newRow["CanConfirm"] = this.checkBox5.Value == "0" ? false : true;
                        newRow["CanUnConfirm"] = this.checkBox6.Value == "0" ? false : true;
                        newRow["CanSend"] = this.checkBox7.Value == "0" ? false : true;
                        newRow["CanRecall"] = this.checkBox8.Value == "0" ? false : true;
                        newRow["CanCheck"] = this.checkBox9.Value == "0" ? false : true;
                        newRow["CanUnCheck"] = this.checkBox10.Value == "0" ? false : true;
                        newRow["CanClose"] = this.checkBox11.Value == "0" ? false : true;
                        newRow["CanUnClose"] = this.checkBox12.Value == "0" ? false : true;
                        newRow["CanReceive"] = this.checkBox13.Value == "0" ? false : true;
                        newRow["CanReturn"] = this.checkBox14.Value == "0" ? false : true;
                        newRow["CanJunk"] = this.checkBox15.Value == "0" ? false : true;
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
            this.checkBox1.Checked = false;
            this.checkBox2.Checked = false;
            this.checkBox3.Checked = false;
            this.checkBox4.Checked = false;
            this.checkBox5.Checked = false;
            this.checkBox6.Checked = false;
            this.checkBox7.Checked = false;
            this.checkBox8.Checked = false;
            this.checkBox9.Checked = false;
            this.checkBox10.Checked = false;
            this.checkBox11.Checked = false;
            this.checkBox12.Checked = false;
            this.checkBox13.Checked = false;
            this.checkBox14.Checked = false;
            this.checkBox15.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.checkBox1.Checked = this.checkBox1.Enabled;
            this.checkBox2.Checked = this.checkBox2.Enabled;
            this.checkBox3.Checked = this.checkBox3.Enabled;
            this.checkBox4.Checked = this.checkBox4.Enabled;
            this.checkBox5.Checked = this.checkBox5.Enabled;
            this.checkBox6.Checked = this.checkBox6.Enabled;
            this.checkBox7.Checked = this.checkBox7.Enabled;
            this.checkBox8.Checked = this.checkBox8.Enabled;
            this.checkBox9.Checked = this.checkBox9.Enabled;
            this.checkBox10.Checked = this.checkBox10.Enabled;
            this.checkBox11.Checked = this.checkBox11.Enabled;
            this.checkBox12.Checked = this.checkBox12.Enabled;
            this.checkBox13.Checked = this.checkBox13.Enabled;
            this.checkBox14.Checked = this.checkBox14.Enabled;
            this.checkBox15.Checked = this.checkBox15.Enabled;
        }
    }
}
