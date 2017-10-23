using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using System.Transactions;

namespace Sci.Production.Shipping
{
    public partial class P02_ImportFromPO : Sci.Win.Subs.Base
    {
        DataRow masterData;
        public P02_ImportFromPO(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
            displayCategory.Value = "Material";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.DataGridViewGeneratorTextColumnSettings ctnno = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings receiver = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
            //CTNNo要Trim掉空白字元
            ctnno.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);
                    dr["CTNNo"] = MyUtility.Convert.GetString(e.FormattedValue).Trim();
                }
            };
            receiver.CharacterCasing = CharacterCasing.Normal;
            this.gridImport.IsEditingReadOnly = false;
            gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Seq1", header: "Seq1#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Seq2", header: "Seq2#", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(10), settings: receiver)
                .Text("Leader", header: "Team Leader", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CTNNo", header: "CTN No.", width: Widths.AnsiChars(5), settings: ctnno)
                .Numeric("Price", header: "Price", decimal_places: 4, iseditingreadonly: true)
                .Numeric("Qty", header: "Q'ty", integer_places: 6, decimal_places: 2, maximum: 999999.99m, minimum: 0m)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("NW", header: "N.W. (kg)", integer_places: 5, decimal_places: 2, maximum: 99999.99m, minimum: 0m);

            gridImport.Columns["Selected"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            gridImport.Columns["Receiver"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            gridImport.Columns["CTNNo"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            gridImport.Columns["Qty"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            gridImport.Columns["NW"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
        }

        //Find Now
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(txtSPNo.Text))
            {
                txtSPNo.Focus();
                MyUtility.Msg.WarningBox("SP# can't empty!!");
                return;
            }
            if (MyUtility.Convert.GetString(txtSPNo.Text).IndexOf("'") != -1)
            {
                txtSPNo.Text = txtSPNo.Text.Replace("'","");
            }

            if (MyUtility.Convert.GetString(txtSEQ1.Text).IndexOf("'") != -1)
            {
                txtSEQ1.Text = txtSEQ1.Text.Replace("'", "");
            }

            if (MyUtility.Convert.GetString(txtSEQ2.Text).IndexOf("'") != -1)
            {
                txtSEQ2.Text = txtSEQ2.Text.Replace("'", "");
            }

            string sqlCmd = string.Format(@"select 0 as Selected, a.*, iif(a.POQty-a.ExpressQty>0,a.POQty-a.ExpressQty,0) as Qty
from (
select psd.ID,psd.SEQ1,psd.SEQ2,psd.Price,psd.POUnit as UnitID,isnull(o.BrandID,'') as BrandID,
isnull(t.Name,'') as Leader, o.SMR  as LeaderID,ps.SuppID,ps.SuppID+'-'+s.AbbEN as Supplier,psd.Qty as POQty,
(select isnull(sum(ed.Qty),0) from Express_Detail ed WITH (NOLOCK) where ed.OrderID = psd.ID and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2) as ExpressQty,
'' as Receiver,'' as CTNNo, 0.0 as NW
from PO_Supp_Detail psd WITH (NOLOCK) 
left join PO_Supp ps WITH (NOLOCK) on psd.ID = ps.ID and psd.SEQ1 = ps.SEQ1
left join Supp s WITH (NOLOCK) on ps.SuppID = s.ID
left join Orders o WITH (NOLOCK) on psd.ID = o.ID
left join TPEPass1 t WITH (NOLOCK) on o.SMR = t.ID
left join factory WITH (NOLOCK)  on o.FactoryID=Factory.ID
where Factory.IsProduceFty=1 and psd.ID = '{0}'{1}{2}) a", txtSPNo.Text, MyUtility.Check.Empty(txtSEQ1.Text) ? "" : " and psd.SEQ1 = '" + txtSEQ1.Text+"'",
                               MyUtility.Check.Empty(txtSEQ2.Text) ? "" : " and psd.SEQ2 = '" + txtSEQ2.Text+"'");
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error." + result.ToString());
                return;
            }
            if(selectData.Rows.Count<=0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            listControlBindingSource1.DataSource = selectData;
        }

        //Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            listControlBindingSource1.EndEdit();

            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data, can't update!");
                return;
            }

            DataRow[] selectedRow = dt.Select("Selected = 1");
            if (selectedRow.Length <= 0)
            {
                MyUtility.Msg.WarningBox("No data need to update!!");
                return;
            }

            if (MyUtility.Check.Empty(txtRemark.Text))
            {
                txtRemark.Focus();
                MyUtility.Msg.WarningBox("Remark can't empty!!");
                return;
            }

            DataTable checkData;
            //檢查重複資料
            try
            {
                MyUtility.Tool.ProcessWithDatatable(dt, "Selected,ID,Seq1,Seq2", string.Format("select e.ID,e.Seq1,e.Seq2 from #tmp e inner join Express_Detail ed on ed.ID = '{0}' and ed.OrderID = e.ID and ed.Seq1 = e.Seq1 and ed.Seq2 = e.Seq2 where e.Selected = 1",MyUtility.Convert.GetString(masterData["ID"])), out checkData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Check duplicate data error.\r\n" + ex.ToString());
                return;
            }
            if (checkData.Rows.Count > 0)
            {
                StringBuilder msgStr = new StringBuilder();
                foreach (DataRow dr in checkData.Rows)
                {
                    msgStr.Append(string.Format("SP#:{0}, Seq1#:{1}, Seq2#:{2}\r\n", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"])));
                }
                msgStr.Append("Data already exists!!");
                MyUtility.Msg.WarningBox(msgStr.ToString());
                return;
            }

            StringBuilder chkQty = new StringBuilder();
            IList<string> insertCmds = new List<string>();

            foreach (DataRow dr in selectedRow)
            {
                if (MyUtility.Check.Empty(dr["Receiver"]))
                {
                    MyUtility.Msg.WarningBox("Receiver can't empty!!");
                    return;
                }

                if (MyUtility.Check.Empty(dr["CTNNo"]))
                {
                    MyUtility.Msg.WarningBox("CTN No. can't empty!!");
                    return;
                }

                if (MyUtility.Check.Empty(dr["NW"]))
                {
                    MyUtility.Msg.WarningBox("N.W. (kg) can't empty!!");
                    return;
                }

                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    chkQty.Append(string.Format("SP#:{0}, Seq1#:{1}, Seq2#:{2}\r\n", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"])));
                }

                insertCmds.Add(string.Format(@"insert into Express_Detail(ID,OrderID,Seq1,Seq2,Qty,NW,CTNNo,Category,SuppID,Price,UnitID,Receiver,BrandID,Leader,Remark,InCharge,AddName,AddDate)
 values('{0}','{1}','{2}','{3}',{4},{5},'{6}','4','{7}',{8},'{9}','{10}','{11}','{12}','{13}','{14}','{14}',GETDATE());",
                                            MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"]), MyUtility.Convert.GetString(dr["Qty"]),
                                            MyUtility.Convert.GetString(dr["NW"]), MyUtility.Convert.GetString(dr["CTNNo"]), MyUtility.Convert.GetString(dr["SuppID"]), MyUtility.Convert.GetString(dr["Price"]), MyUtility.Convert.GetString(dr["UnitID"]), MyUtility.Convert.GetString(dr["Receiver"]), MyUtility.Convert.GetString(dr["BrandID"]),
                                            MyUtility.Convert.GetString(dr["LeaderID"]), txtRemark.Text, Sci.Env.User.UserID));
            }

            //Qty不可為0
            if (chkQty.Length > 0)
            {
                MyUtility.Msg.WarningBox("Q'ty can't be 0!!\r\n"+chkQty.ToString());
                return;
            }

            //檢查Total Qty是否有超過PO Qty
            try
            {
                MyUtility.Tool.ProcessWithDatatable(dt, "Selected,ID,Seq1,Seq2,Qty,POQty", @"select a.ID,a.Seq1,a.Seq2
from (
select e.ID,e.Seq1,e.Seq2,e.POQty,
e.Qty+(select isnull(sum(ed.Qty),0) as Qty from Express_Detail ed WITH (NOLOCK) where ed.OrderID = e.ID and ed.Seq1 = e.Seq1 and ed.Seq2 = e.Seq2) as TtlQty
from #tmp e
where e.Selected = 1) a
where a.TtlQty > a.POQty", out checkData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Check total qty error.\r\n" + ex.ToString());
                return;
            }
            if (checkData == null || checkData.Rows.Count > 0)
            {
                StringBuilder msgStr = new StringBuilder();
                foreach (DataRow dr in checkData.Rows)
                {
                    msgStr.Append(string.Format("SP#:{0}, Seq1#:{1}, Seq2#:{2}\r\n", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"])));
                }
                msgStr.Append("Total Qty > PO Qty, pls check!!");
                MyUtility.Msg.WarningBox(msgStr.ToString());
                return;
            }
            DualResult result1, result2;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    result1 = DBProxy.Current.Executes(null, insertCmds);
                    result2 = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(masterData["ID"])));
                    if (result1 && result2)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Update failed, Pleaes re-try");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            MyUtility.Msg.InfoBox("Update complete!!");
        }
    }
}
