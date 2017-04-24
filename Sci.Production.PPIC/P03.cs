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

namespace Sci.Production.PPIC
{
    public partial class P03 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.DataGridViewGeneratorDateColumnSettings rcvDate = new Ict.Win.DataGridViewGeneratorDateColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings ftyRemark = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private bool needSave = false, alreadySave = false;
        private DataTable gridData;
        string f; int query;
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable facData = null;
            string factoryCmd = string.Format(@"
            select distinct sp.FactoryID
            from Style_ProductionKits sp WITH (NOLOCK) 
            left join Style s WITH (NOLOCK) on s.Ukey = sp.StyleUkey
            where sp.ReceiveDate is null
            and sp.MDivisionID ='{0}' ", Sci.Env.User.Keyword);
            DBProxy.Current.Select("", factoryCmd.ToString(), out facData);
            facData.Rows.Add(new string[] { "" });
            facData.DefaultView.Sort = "factoryid";
            this.comboFactory.DataSource = facData;
            this.comboFactory.ValueMember = "factoryid";
            this.EditMode = true;
            this.comboFactory.SelectedIndex = 0;
           
         
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
           
            QueryData();
           
            ftyRemark.CharacterCasing = CharacterCasing.Normal;
            rcvDate.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridProductionKitsConfirm.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && (Convert.ToDateTime(e.FormattedValue) > Convert.ToDateTime(DateTime.Today).AddDays(180) || Convert.ToDateTime(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddDays(-180)))
                {
                    MyUtility.Msg.WarningBox("< FTY MR Rcv date > is invalid, it exceeds +/-180 days!!");
                    dr["ReceiveDate"] = DBNull.Value;
                    e.Cancel = true;
                    return;
                }
            };
         
            gridProductionKitsConfirm.DataSource = listControlBindingSource1;
            gridProductionKitsConfirm.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.gridProductionKitsConfirm)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .EditText("Article", header: "Colorway", width: Widths.AnsiChars(22), iseditingreadonly: true)
                .Text("ReasonName", header: "Doc", width: Widths.AnsiChars(22), iseditingreadonly: true)
                .Date("SendDate", header: "TW Send date", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("ReceiveDate", header: "FTY MR Rcv date", width: Widths.AnsiChars(8), settings: rcvDate)
                .Text("FtyRemark", header: "Remark for factory", width: Widths.AnsiChars(15), settings: ftyRemark)
                .Date("ProvideDate", header: "Provide date", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI Delivery", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("MRName", header: "MR", width: Widths.AnsiChars(38), iseditingreadonly: true)
                .Text("SMRName", header: "SMR", width: Widths.AnsiChars(38), iseditingreadonly: true)
                .Text("POHandleName", header: "PO Handle", width: Widths.AnsiChars(38), iseditingreadonly: true)
                .Text("POSMRName", header: "PO SMR", width: Widths.AnsiChars(38), iseditingreadonly: true);

            this.gridProductionKitsConfirm.Columns["ReceiveDate"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridProductionKitsConfirm.Columns["FtyRemark"].DefaultCellStyle.BackColor = Color.LightYellow;
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            this.gridProductionKitsConfirm.ValidateControl();
            listControlBindingSource1.EndEdit();
            f = comboFactory.SelectedValue.ToString();
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    needSave = true;
                    break;
                }
            }

            if (needSave && !alreadySave)
            {
                DialogResult buttonResult = MyUtility.Msg.QuestionBox("Do you want to save data?","Confirm", MessageBoxButtons.OKCancel);
                if (buttonResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (!SaveData())
                    {
                        return;
                    }
                }
                needSave = false;
            }
             query = 1;
            QueryData();
            this.gridProductionKitsConfirm.AutoResizeColumn(0);
            this.gridProductionKitsConfirm.AutoResizeColumn(1);
            this.gridProductionKitsConfirm.AutoResizeColumn(2);
            this.gridProductionKitsConfirm.AutoResizeColumn(4);
            this.gridProductionKitsConfirm.AutoResizeColumn(5);
            this.gridProductionKitsConfirm.AutoResizeColumn(6);
            this.gridProductionKitsConfirm.AutoResizeColumn(7);
            this.gridProductionKitsConfirm.AutoResizeColumn(8);
            this.gridProductionKitsConfirm.AutoResizeColumn(9);
            this.gridProductionKitsConfirm.AutoResizeColumn(10);
            this.gridProductionKitsConfirm.AutoResizeColumn(11);
            this.gridProductionKitsConfirm.AutoResizeColumn(12);
            this.gridProductionKitsConfirm.AutoResizeColumn(13);
            this.gridProductionKitsConfirm.AutoResizeColumn(14);
            this.gridProductionKitsConfirm.AutoResizeColumn(15);
        }

        private void QueryData()
        {
            
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select sp.*,s.ID as StyleID,s.SeasonID,
(select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'ProductionKits' and ID = sp.DOC) as ReasonName,
isnull((sp.MRHandle+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.MRHandle)),sp.MRHandle) as MRName,
isnull((sp.SMR+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.SMR)),sp.SMR) as SMRName,
isnull((sp.PoHandle+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.PoHandle)),sp.PoHandle) as POHandleName,
isnull((sp.POSMR+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.POSMR)),sp.POSMR) as POSMRName,
iif(sp.IsPF = 1,'Y','N') as CPF
from Style_ProductionKits sp WITH (NOLOCK) 
left join Style s WITH (NOLOCK) on s.Ukey = sp.StyleUkey
where sp.ReceiveDate is null and sp.SendDate is null
and sp.MDivisionID = '{0}' ", Sci.Env.User.Keyword));
            
            if (!MyUtility.Check.Empty(txtStyleNo.Text))
            {
               sqlCmd.Append(string.Format(" and s.ID = '{0}'",txtStyleNo.Text));   
            }

            if (!MyUtility.Check.Empty(txtSeason.Text))
            {
               sqlCmd.Append(string.Format(" and s.SeasonID = '{0}'", txtSeason.Text));
            }

            if (!MyUtility.Check.Empty(dateSendDate.Value))
            {
               sqlCmd.Append(string.Format(" and sp.SendDate = '{0}'",Convert.ToDateTime(dateSendDate.Value).ToString("d")));               
            }
         
            if (query == 1)
            {
                if (f == "") { }
                if (!this.comboFactory.Text.ToString().Empty()&& f !="")
                {
                    sqlCmd.Append(string.Format(" and sp.FactoryID = '{0}'", f));
                }
            }
             sqlCmd.Append(@" order by FactoryID, StyleID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            
            if (!result )
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (gridData.Rows.Count == 0 )
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
            alreadySave = false;
            query = 0;
        }

        //Batch update
        private void button2_Click(object sender, EventArgs e)
        {
           foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Check.Empty(dateFactoryReceiveDate.Value))
                {
                    dr["ReceiveDate"] = DBNull.Value;
                }
                else
                {
                    dr["ReceiveDate"] = dateFactoryReceiveDate.Value;
                }
            }
        }

        //Save
        private void button3_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private bool SaveData()
        {
            IList<string> updateCmds = new List<string>();
            this.gridProductionKitsConfirm.ValidateControl();
            listControlBindingSource1.EndEdit();
            StringBuilder cmds = new StringBuilder();
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    cmds.Clear();
                    cmds.Append(string.Format(@"update Style_ProductionKits set ReceiveDate = {0}, FtyRemark = '{1}'", MyUtility.Check.Empty(dr["ReceiveDate"]) ? "null" : "'"+Convert.ToDateTime(dr["ReceiveDate"]).ToString("d")+"'", dr["FtyRemark"].ToString()));
                    if (!MyUtility.Check.Empty(dr["ReceiveDate"]))
                    {
                        cmds.Append(string.Format(@", FtyHandle = '{0}', FtyLastDate = GETDATE()", Sci.Env.User.UserID));
                    }
                    cmds.Append(string.Format(" where Ukey = {0}", dr["UKey"].ToString()));

                    updateCmds.Add(cmds.ToString());
                }
            }
            if (updateCmds.Count != 0)
            {
                DualResult result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Save Fail!" + result.ToString());
                    return false;
                }
            }
            alreadySave = true;
            return true;
        }

        //Close
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //View Detail
        private void button5_Click(object sender, EventArgs e)
        {
            this.gridProductionKitsConfirm.ValidateControl();
            Sci.Production.PPIC.P03_Detail DoForm = new Sci.Production.PPIC.P03_Detail();
            DoForm.Set(false, ((DataTable)listControlBindingSource1.DataSource).ToList(), gridProductionKitsConfirm.GetDataRow(gridProductionKitsConfirm.GetSelectedRowIndex())); DoForm.ShowDialog(this);
        }
    }
}
