﻿using System;
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
    /// <summary>
    /// P02_ImportFromFOCPackingList
    /// </summary>
    public partial class P02_ImportFromFOCPackingList : Sci.Win.Subs.Base
    {
        private DataRow masterData;

        /// <summary>
        /// P02_ImportFromFOCPackingList
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P02_ImportFromFOCPackingList(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.DataGridViewGeneratorTextColumnSettings ctnno = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings receiver = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

            // CTNNo要Trim掉空白字元
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
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
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

            this.gridImport.Columns["CTNNo"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["NW"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["Price"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["UnitID"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["Receiver"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
        }

        // Find Now
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtFOCPL.Text))
            {
                this.txtFOCPL.Focus();
                MyUtility.Msg.WarningBox("FOC PL# can't empty!!");
                return;
            }

            // 檢查FOC PL#是否正確
            if (!this.CheckPLNo(this.txtFOCPL.Text))
            {
                return;
            }

            string sqlCmd = string.Format(
                @"select pd.ID,pd.OrderID,o.SeasonID,o.StyleID,'Sample' as Category,
'' as CTNNo, 0.0 as NW, 0.0 as Price,pd.ShipQty,o.StyleUnit as UnitID,'' as Receiver,o.SMR as LeaderID,
t.Name as Leader, o.BrandID, [dbo].[getBOFMtlDesc](o.StyleUkey) as Description
from PackingList_Detail pd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
left join TPEPass1 t WITH (NOLOCK) on o.SMR = t.ID
left join factory WITH (NOLOCK)  on o.FactoryID=Factory.ID
where pd.ID = '{0}'
and Factory.IsProduceFty=1", this.txtFOCPL.Text);
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error." + result.ToString());
                return;
            }

            if (selectData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!!");
            }

            this.listControlBindingSource1.DataSource = selectData;
        }

        // 檢查FOC PL#是否正確
        private bool CheckPLNo(string pLNo)
        {
            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", pLNo);

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            DataTable packListData;
            string sqlCmd = "select ExpressID from PackingList WITH (NOLOCK) where ID = @id and Type = 'F'";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out packListData);
            if (result && packListData.Rows.Count > 0)
            {
                if (!MyUtility.Check.Empty(packListData.Rows[0]["ExpressID"]))
                {
                    MyUtility.Msg.WarningBox("The FOC PL# already be assign HC#, so can't assign again!!");
                    return false;
                }
            }
            else
            {
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                }
                else
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                return false;
            }

            return true;
        }

        // Update
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            this.listControlBindingSource1.EndEdit();

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data, can't update!");
                return;
            }

            // 檢查FOC PL#是否正確
            if (!this.CheckPLNo(MyUtility.Convert.GetString(dt.Rows[0]["ID"])))
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

                insertCmds.Add(string.Format(
                    @"insert into Express_Detail(ID,OrderID,Seq1,SeasonID,StyleID,Description,Qty,NW,CTNNo,Category,DutyNo,Price,UnitID,Receiver,BrandID,Leader,InCharge,AddName,AddDate)
 values('{0}','{1}',(select ISNULL(RIGHT(REPLICATE('0',3)+CAST(MAX(CAST(Seq1 as int))+1 as varchar),3),'001')
from Express_Detail where ID = '{0}' and Seq2 = ''),'{2}','{3}','{4}',{5},{6},'{7}','1','{8}',{9},'{10}','{11}','{12}','{13}','{14}','{14}',GETDATE());",
                                            MyUtility.Convert.GetString(this.masterData["ID"]),
                                            MyUtility.Convert.GetString(dr["OrderID"]),
                                            MyUtility.Convert.GetString(dr["SeasonID"]),
                                            MyUtility.Convert.GetString(dr["StyleID"]),
                                            MyUtility.Convert.GetString(dr["Description"]),
                                            MyUtility.Convert.GetString(dr["ShipQty"]),
                                            MyUtility.Convert.GetString(dr["NW"]),
                                            MyUtility.Convert.GetString(dr["CTNNo"]),
                                            MyUtility.Convert.GetString(dr["ID"]),
                                            MyUtility.Convert.GetString(dr["Price"]),
                                            MyUtility.Convert.GetString(dr["UnitID"]),
                                            MyUtility.Convert.GetString(dr["Receiver"]),
                                            MyUtility.Convert.GetString(dr["BrandID"]),
                                            MyUtility.Convert.GetString(dr["LeaderID"]),
                                            Sci.Env.User.UserID));
            }

            string pulloutdate = "Null";
            if (!MyUtility.Check.Empty(this.masterData["ShipDate"]))
            {
                pulloutdate = "'" + ((DateTime)this.masterData["ShipDate"]).ToString("d") + "'";
            }
            else
            {
                pulloutdate = "Null";
            }

            insertCmds.Add($"update PackingList set ExpressID = '{this.masterData["ID"]}',pulloutdate={pulloutdate} where ID = '{dt.Rows[0]["ID"]}'");
            DualResult result1, result2;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    result1 = DBProxy.Current.Executes(null, insertCmds);
                    result2 = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.masterData["ID"])));
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
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Update complete!!");
        }
    }
}
