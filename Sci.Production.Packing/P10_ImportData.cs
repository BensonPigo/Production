using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Transactions;
using Sci;
using Sci.Production.PublicPrg;

namespace Sci.Production.Packing
{
    public partial class P10_ImportData : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        string idValue;
        public string newID = "";
        public string transDate;

        public P10_ImportData(string masterID)
        {
            InitializeComponent();
            idValue = masterID;
        }

        protected override void OnFormLoaded()
        {
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.grid1)
                 .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                 .Text("PackingListID", header: "PackId", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(4), iseditingreadonly: true)
                 .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
                 .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        //Find
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.textBox1.Text) && MyUtility.Check.Empty(this.textBox2.Text) && MyUtility.Check.Empty(this.textBox3.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > or < Order# > or < PackID > can not be empty!");
                return;
            }
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"Select Distinct '' as ID, 0 as selected, b.Id as PackingListID, b.OrderID, b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery 
                                                         from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) , Country d WITH (NOLOCK) 
                                                         where b.OrderId = c.Id 
                                                         and a.Id = b.Id 
                                                         and b.CTNStartNo != '' 
                                                         and ((b.ClogReturnId = '' and TransferToClogId = '') or b.ClogReturnId != '') 
                                                         and c.Dest = d.ID 
                                                         and a.MDivisionID = '{0}' and (a.Type = 'B' or a.Type = 'L') and c.MDivisionID = '{0}'", Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(this.textBox1.Text))
            {
                sqlCmd.Append(string.Format(" and a.OrderID = '{0}'", this.textBox1.Text.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(this.textBox2.Text))
            {
                sqlCmd.Append(string.Format(" and c.CustPONo = '{0}'", this.textBox2.Text.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(this.textBox3.Text))
            {
                sqlCmd.Append(string.Format(" and a.ID = '{0}'", this.textBox3.Text.ToString().Trim()));
            }

            DataTable selectDataTable;
            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, sqlCmd.ToString(), out selectDataTable))
            {
                if (selectDataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = selectDataTable;
        }

        //Save
        private void button3_Click(object sender, EventArgs e)
        {
            //檢查是否有勾選資料
            this.grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("No data need to import!");
                return;
            }

            //設定參數值
            if (MyUtility.Check.Empty(idValue))
            {
                string getID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "CS", "TransferToClog", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(getID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return;
                }
                newID = getID;
                transDate = DateTime.Today.ToString("d");
            }
            else
            {
                newID = idValue;
                transDate = Convert.ToDateTime(MyUtility.GetValue.Lookup("TransferDate", newID, "TransferToClog", "ID")).ToString("d");
            }

            //找出要新增的資料'
            DataTable ResultData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(dt, "Selected,PackingListID,OrderID,CTNStartNo", string.Format(@"select a.Selected,a.PackingListID,a.OrderID,a.CTNStartNo,iif(td.Id is null, 1, 0) as InsertData 
from #tmp a
left join TransferToClog_Detail td on td.Id = '{0}' and td.PackingListID = a.PackingListID and td.CTNStartNo = a.CTNStartNo
where a.Selected = 1", MyUtility.Convert.GetString(idValue)), out ResultData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("ProcessWithDatatable error.\r\n" + ex.ToString());
                return;
            }

            IList<string> insertCmds = new List<string>();
            IList<string> updateCmds = new List<string>();
            //組表身資料
            foreach (DataRow currentRecord in ResultData.Rows)
            {
                if (currentRecord["InsertData"].ToString() == "1")
                {
                    insertCmds.Add(string.Format(@"Insert into TransferToClog_Detail (Id,PackingListID,OrderID,CTNStartNo,AddName,AddDate) 
Values('{4}','{0}','{1}','{2}','{3}',GETDATE()); ", currentRecord["PackingListID"].ToString(), currentRecord["OrderID"].ToString(), currentRecord["CTNStartNo"].ToString(), Sci.Env.User.UserID, newID));
                    //要順便更新PackingList_Detail
                    updateCmds.Add(string.Format(@"update PackingList_Detail 
set TransferToClogID = '{3}', TransferDate = '{4}', ClogReceiveID = '', ReceiveDate = null, ClogLocationId = '', ClogReturnID = '', ReturnDate = null 
where ID = '{0}' and OrderID = '{1}' and CTNStartNo = '{2}'; ", currentRecord["PackingListID"].ToString(), currentRecord["OrderID"].ToString(), currentRecord["CTNStartNo"].ToString(), newID, transDate));
                }
            }

            //組表頭資料
            if (MyUtility.Check.Empty(idValue))
            {
                insertCmds.Add(string.Format(@"Insert into TransferToClog (Id,TransferDate,MDivisionID,AddName,AddDate) 
                Values('{2}','{3}','{0}','{1}',GETDATE());", Sci.Env.User.Keyword, Sci.Env.User.UserID, newID, transDate));
            }
            else
            {
                updateCmds.Add(string.Format("update TransferToClog set EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, newID));
            }
            
            //Update Orders的資料
                DataTable selectData;
                string sqlCmd = string.Format("select OrderID from TransferToClog_Detail where ID = '{0}' group by OrderID", newID);
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Update orders data fail!");
                }

            DualResult result1 = Result.True, result2 = Result.True;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (updateCmds.Count > 0)
                    {
                        result1 = Sci.Data.DBProxy.Current.Executes(null, updateCmds);
                    }
                    if(insertCmds.Count > 0)
                    {
                        result2 = Sci.Data.DBProxy.Current.Executes(null, insertCmds);
                    }   
                    DualResult prgResult = Prgs.UpdateOrdersCTN(selectData);

                    if (result1 && result2 && prgResult)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Save failed, Pleaes re-try");
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
                //系統會自動有回傳值
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }

        //Import From Barcode
        private void button2_Click(object sender, EventArgs e)
        {
            //設定只能選txt檔
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //開窗且有選擇檔案
            {
                //先將Grid的結構給開出來
                string selectCommand = @"Select distinct '' as ID, 0 as selected, b.Id as PackingListID, b.OrderID, b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery 
                                                             from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) , Country d WITH (NOLOCK) where 1=0";
                DataTable selectDataTable;
                DualResult selectResult;
                if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out selectDataTable)))
                {
                    MyUtility.Msg.WarningBox("Connection faile.!");
                    return;
                }
                listControlBindingSource1.DataSource = selectDataTable;

                //讀檔案
                using (StreamReader reader = new StreamReader(openFileDialog1.FileName, System.Text.Encoding.UTF8))
                {
                    DataRow seekData;
                    int insertCount = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        System.Diagnostics.Debug.WriteLine(line);
                        IList<string> sl = line.Split(" \t\r\n".ToCharArray());
                        if (sl[0] != "1")
                        {
                            MyUtility.Msg.WarningBox("Format is not correct!");
                            return;
                        }
                        else
                        {
                            DataRow dr = selectDataTable.NewRow();
                            dr["ID"] = "";
                            dr["selected"] = 0;
                            dr["PackingListID"] = sl[1].Substring(0, 13);
                            dr["CTNStartNo"] = sl[1].Substring(13);
                            string sqlCmd = string.Format(@"select OrderID 
                                                                                  from PackingList_Detail WITH (NOLOCK) 
                                                                                  where ID = '{0}' and CTNStartNo = '{1}' and TransferToClogID = ''
                                                                                   ", dr["PackingListID"].ToString(), dr["CTNStartNo"].ToString());
                            if (MyUtility.Check.Seek(sqlCmd, out seekData))
                            {
                                dr["OrderID"] = seekData["OrderID"].ToString().Trim();
                                sqlCmd = string.Format(@"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,a.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                             left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'", dr["OrderID"].ToString(), Sci.Env.User.Keyword);
                                if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                {
                                    dr["StyleID"] = seekData["StyleID"].ToString().Trim();
                                    dr["SeasonID"] = seekData["SeasonID"].ToString().Trim();
                                    dr["BrandID"] = seekData["BrandID"].ToString().Trim();
                                    dr["Customize1"] = seekData["Customize1"].ToString().Trim();
                                    dr["CustPONo"] = seekData["CustPONo"].ToString().Trim();
                                    dr["Alias"] = seekData["Alias"].ToString().Trim();
                                    dr["BuyerDelivery"] = seekData["BuyerDelivery"];

                                    selectDataTable.Rows.Add(dr);
                                    insertCount++;
                                }
                            }
                        }
                    }
                    if (insertCount == 0)
                    {
                        MyUtility.Msg.WarningBox("This data were all be transferred or order's M is not equal to login M.");
                        return;
                    }
                }
            }
        }
    }
}
