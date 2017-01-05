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
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            QueryData();

            ftyRemark.CharacterCasing = CharacterCasing.Normal;
            rcvDate.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && (Convert.ToDateTime(e.FormattedValue) > Convert.ToDateTime(DateTime.Today).AddDays(180) || Convert.ToDateTime(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddDays(-180)))
                {
                    MyUtility.Msg.WarningBox("< FTY MR Rcv date > is invalid, it exceeds +/-180 days!!");
                    dr["ReceiveDate"] = DBNull.Value;
                    e.Cancel = true;
                    return;
                }
            };
         
            grid1.DataSource = listControlBindingSource1;
            grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
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

            this.grid1.Columns[6].DefaultCellStyle.BackColor = Color.LightYellow;
            this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.LightYellow;
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
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
            QueryData();          
            
        }

        private void QueryData()
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select sp.*,s.ID as StyleID,s.SeasonID,
(select Name from Reason where ReasonTypeID = 'ProductionKits' and ID = sp.DOC) as ReasonName,
isnull((sp.MRHandle+' '+(select Name+' #'+ExtNo from TPEPass1 where ID = sp.MRHandle)),sp.MRHandle) as MRName,
isnull((sp.SMR+' '+(select Name+' #'+ExtNo from TPEPass1 where ID = sp.SMR)),sp.SMR) as SMRName,
isnull((sp.PoHandle+' '+(select Name+' #'+ExtNo from TPEPass1 where ID = sp.PoHandle)),sp.PoHandle) as POHandleName,
isnull((sp.POSMR+' '+(select Name+' #'+ExtNo from TPEPass1 where ID = sp.POSMR)),sp.POSMR) as POSMRName,
iif(sp.IsPF = 1,'Y','N') as CPF
from Style_ProductionKits sp
left join Style s on s.Ukey = sp.StyleUkey
where sp.ReceiveDate is null
and sp.MDivisionID = '{0}' ", Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                sqlCmd.Append(string.Format(" and s.ID = '{0}'",textBox1.Text));
            }

            if (!MyUtility.Check.Empty(textBox2.Text))
            {
                sqlCmd.Append(string.Format(" and s.SeasonID = '{0}'",textBox2.Text));
            }

            if (!MyUtility.Check.Empty(dateBox1.Value))
            {
                sqlCmd.Append(string.Format(" and sp.SendDate = '{0}'",Convert.ToDateTime(dateBox1.Value).ToString("d")));
            }
            sqlCmd.Append(@" order by FactoryID, StyleID");

            
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
            alreadySave = false;
        }

        //Batch update
        private void button2_Click(object sender, EventArgs e)
        {
           foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Check.Empty(dateBox2.Value))
                {
                    dr["ReceiveDate"] = DBNull.Value;
                }
                else
                {
                    dr["ReceiveDate"] = dateBox2.Value;
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
            this.grid1.ValidateControl();
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
            this.grid1.ValidateControl();
            Sci.Production.PPIC.P03_Detail DoForm = new Sci.Production.PPIC.P03_Detail();
            DoForm.Set(false, ((DataTable)listControlBindingSource1.DataSource).ToList(), grid1.GetDataRow(grid1.GetSelectedRowIndex())); DoForm.ShowDialog(this);
        }
    }
}
