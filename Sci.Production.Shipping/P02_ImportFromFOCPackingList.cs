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
    public partial class P02_ImportFromFOCPackingList : Sci.Win.Subs.Base
    {
        DataRow masterData;
        public P02_ImportFromFOCPackingList(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.DataGridViewGeneratorTextColumnSettings ctnno = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings receiver = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            //CTNNo要Trim掉空白字元
            ctnno.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    dr["CTNNo"] = e.FormattedValue.ToString().Trim();
                }
            };
            receiver.CharacterCasing = CharacterCasing.Normal;
            this.grid1.IsEditingReadOnly = false;
            grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CTNNo", header: "CTN No.", width: Widths.AnsiChars(5), settings: ctnno)
                .Numeric("NW", header: "N.W. (kg)", integer_places: 5, decimal_places: 2, maximum: 99999.99m, minimum: 0m)
                .Numeric("Price", header: "Price", integer_places: 6, decimal_places: 4, maximum: 999999.9999m, minimum: 0m)
                .Numeric("ShipQty", header: "Q'ty", decimal_places: 2, iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8))
                .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(10), settings: receiver)
                .Text("Leader", header: "Team Leader", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true);

            grid1.Columns[4].DefaultCellStyle.BackColor = Color.FromArgb(255,255,191);
            grid1.Columns[5].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            grid1.Columns[6].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            grid1.Columns[8].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            grid1.Columns[9].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
        }

        //Find Now
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(textBox1.Text))
            {
                MyUtility.Msg.WarningBox("FOC PL# can't empty!!");
                textBox1.Focus();
                return;
            }

            //檢查FOC PL#是否正確
            if (!CheckPLNo(textBox1.Text))
            {
                return;
            }

            string sqlCmd = string.Format(@"select pd.ID,pd.OrderID,o.SeasonID,o.StyleID,'Sample' as Category,
'' as CTNNo, 0 as NW, 0 as Price,pd.ShipQty,o.StyleUnit as UnitID,'' as Receiver,
t.Name as Leader, o.BrandID, [dbo].[getBOFMtlDesc](o.StyleUkey) as Description
from PackingList_Detail pd
left join Orders o on pd.OrderID = o.ID
left join TPEPass1 t on o.SMR = t.ID
where pd.ID = '{0}'", textBox1.Text);
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error." + result.ToString());
                return;
            }
            listControlBindingSource1.DataSource = selectData;
        }

        //檢查FOC PL#是否正確
        private bool CheckPLNo(string PLNo)
        {
            DataRow PackListData;
            if (MyUtility.Check.Seek(string.Format("select ExpressID from PackingList where ID = '{0}' and Type = 'F'", PLNo), out PackListData))
            {
                if (!MyUtility.Check.Empty(PackListData["ExpressID"]))
                {
                    MyUtility.Msg.WarningBox("The FOC PL# already be assign HC#, so can't assign again!!");
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Data not found!!");
                return false;
            }
            return true;
        }

        //Update
        private void button2_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            listControlBindingSource1.EndEdit();

            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data, can't update!");
                return;
            }
            //檢查FOC PL#是否正確
            if (!CheckPLNo(dt.Rows[0]["ID"].ToString()))
            {
                return;
            }
            IList<string> insertCmds = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
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
                if (MyUtility.Check.Empty(dr["UnitID"]))
                {
                    MyUtility.Msg.WarningBox("Unit can't empty!!");
                    return;
                }
                if (MyUtility.Check.Empty(dr["Receiver"]))
                {
                    MyUtility.Msg.WarningBox("Receiver can't empty!!");
                    return;
                }

                insertCmds.Add(string.Format(@"insert into Express_Detail(ID,OrderID,Seq1,SeasonID,StyleID,Description,Qty,NW,CTNNo,Category,DutyNo,Price,UnitID,Receiver,BrandID,Leader,InCharge,AddName,AddDate)
 values('{0}','{1}',(select RIGHT(REPLICATE('0',3)+CAST(MAX(CAST(Seq1 as int))+1 as varchar),3)
from Express_Detail where ID = '{0}' and Seq2 = ''),'{2}','{3}','{4}',{5},{6},'{7}','1','{8}',{9},'{10}','{11}','{12}','{13}','{14}','{14}',GETDATE());",
                                            masterData["ID"].ToString(), dr["OrderID"].ToString(), dr["SeasonID"].ToString(), dr["StyleID"].ToString(), dr["Description"].ToString(), dr["ShipQty"].ToString(),
                                            dr["NW"].ToString(), dr["CTNNo"].ToString(), dr["ID"].ToString(), dr["Price"].ToString(), dr["UnitID"].ToString(), dr["Receiver"].ToString(), dr["BrandID"].ToString(),
                                            dr["Leader"].ToString(),Sci.Env.User.UserID));
            }

            insertCmds.Add(string.Format("update PackingList set ExpressID = '{0}' where ID = '{1}'", masterData["ID"].ToString(), dt.Rows[0]["ID"].ToString()));
            DualResult result1, result2;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    result1 = DBProxy.Current.Executes(null, insertCmds);
                    result2 = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(masterData["ID"].ToString()));
                    if (result1 && result2)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Update failed, Pleaes re-try");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            MyUtility.Msg.InfoBox("Update complete!!");
        }
    }
}
