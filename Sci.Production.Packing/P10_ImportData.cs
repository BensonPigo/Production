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
                 .Text("PackingListID", header: "PackId", width: Widths.AnsiChars(13), iseditingreadonly: true)
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
            if (string.IsNullOrWhiteSpace(this.textBox1.Text.ToString()) && string.IsNullOrWhiteSpace(this.textBox2.Text.ToString()) && string.IsNullOrWhiteSpace(this.textBox3.Text.ToString()))
            {
                MessageBox.Show("< SP# > or < Order# > or < PackID > can not be empty!");
                return;
            }

            string selectCommand = @"Select Distinct '' as ID, 0 as selected, b.Id as PackingListID, b.OrderID, b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery 
                                                         from PackingList a, PackingList_Detail b, Orders c, Country d 
                                                         where a.OrderId = c.Id 
                                                         and a.Id = b.Id 
                                                         and b.CTNStartNo != '' 
                                                         and ((b.ClogReturnId = '' and TransferToClogId = '') or b.ClogReturnId != '') 
                                                         and c.Dest = d.ID 
                                                         and a.FactoryID = '" + Sci.Env.User.Factory + "' and (a.Type = 'B' or a.Type = 'L') and c.FTYGroup = '" + Sci.Env.User.Factory + "'";
            if (!string.IsNullOrWhiteSpace(this.textBox1.Text.ToString()))
            {
                selectCommand = selectCommand + string.Format(" and a.OrderID = '{0}'", this.textBox1.Text.ToString().Trim());
            }
            if (!string.IsNullOrWhiteSpace(this.textBox2.Text.ToString()))
            {
                selectCommand = selectCommand + string.Format(" and c.CustPONo = '{0}'", this.textBox2.Text.ToString().Trim());
            }
            if (!string.IsNullOrWhiteSpace(this.textBox3.Text.ToString()))
            {
                selectCommand = selectCommand + string.Format(" and a.ID = '{0}'", this.textBox3.Text.ToString().Trim());
            }

            DataTable selectDataTable;
            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, selectCommand, out selectDataTable))
            {
                if (selectDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = selectDataTable;
        }

        //全選的CheckBox
        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (null != col_chk)
            {
                this.grid1.SetCheckeds(col_chk);
                if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                {
                    if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                }
            }
        }

        //全不選的CheckBox
        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (null != col_chk)
            {
                this.grid1.SetUncheckeds(col_chk);
                if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                {
                    if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                }
            }
        }

        //Save
        private void button3_Click(object sender, EventArgs e)
        {
            string sqlCommand;
            bool insertData;
            string sqlInsertMaster = "";
            string sqlInsert = "";
            string sqlUpdatePackingList = "";
            DateTime nowTime = DateTime.Now;

            //檢查是否有勾選資料
            this.grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MessageBox.Show("No data need to import!");
                return;
            }

            //設定參數值
            if (string.IsNullOrWhiteSpace(idValue))
            {
                string getID = MyUtility.GetValue.GetID(ProductionEnv.Keyword + "CS", "TransferToClog", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(getID))
                {
                    MessageBox.Show("GetID fail, please try again!");
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

            //組表身資料
            foreach (DataRow currentRecord in selectedData)
            {
                insertData = true;
                if (!string.IsNullOrWhiteSpace(idValue))
                {
                    sqlCommand = string.Format(@"select ID
                                                                              from TransferToClog_Detail  
                                                                              where ID = '{0}' and PackingListID = '{1}' and CTNStartNo = '{2}'
                                                                              ", idValue, currentRecord["PackingListID"].ToString(), currentRecord["CTNStartNo"].ToString());
                    if (MyUtility.Check.Seek(sqlCommand, null))
                    {
                        insertData = false;
                    }
                }

                if (insertData)
                {
                    sqlInsert = sqlInsert + "Insert into TransferToClog_Detail (Id,PackingListID,OrderID,CTNStartNo,AddName,AddDate)\r\n ";
                    sqlInsert = sqlInsert + string.Format("Values('{5}','{0}','{1}','{2}','{3}','{4}');\r\n ", currentRecord["PackingListID"].ToString(), currentRecord["OrderID"].ToString(), currentRecord["CTNStartNo"].ToString(), Sci.Env.User.UserID, nowTime.ToString("yyyy/MM/dd HH:mm:ss"), newID);
                    //要順便更新PackingList_Detail
                    sqlUpdatePackingList = sqlUpdatePackingList + string.Format(@"update PackingList_Detail 
                                                                                                                                  set TransferToClogID = '{3}', TransferDate = '{4}', ClogReceiveID = '', ReceiveDate = null, ClogLocationId = '', ClogReturnID = '', ReturnDate = null 
                                                                                                                                  where ID = '{0}' and OrderID = '{1}' and CTNStartNo = '{2}'; ", currentRecord["PackingListID"].ToString(), currentRecord["OrderID"].ToString(), currentRecord["CTNStartNo"].ToString(), newID, transDate);
                }
            }

            //組表頭資料
            if (string.IsNullOrWhiteSpace(idValue))
            {
                sqlInsertMaster = sqlInsertMaster + "Insert into TransferToClog (Id,TransferDate,FactoryID,AddName,AddDate)\r\n ";
                sqlInsertMaster = sqlInsertMaster + string.Format("Values('{3}','{4}','{0}','{1}','{2}');\r\n ", Sci.Env.User.Factory, Sci.Env.User.UserID, nowTime.ToString("yyyy/MM/dd HH:mm:ss"), newID, transDate);
            }
            else
            {
                sqlInsertMaster = string.Format("update TransferToClog set EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, nowTime.ToString("yyyy/MM/dd HH:mm:ss"), newID);
            }

            if (!string.IsNullOrWhiteSpace(sqlInsert))
            {
                DualResult result1, result2, result3;
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        result1 = Sci.Data.DBProxy.Current.Execute(null, sqlInsertMaster);
                        result2 = Sci.Data.DBProxy.Current.Execute(null, sqlInsert);
                        result3 = Sci.Data.DBProxy.Current.Execute(null, sqlUpdatePackingList);

                        if (result1 && result2 && result3)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            MessageBox.Show("Save failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }

                //Update Orders的資料
                DataTable selectData;
                string sqlCmd = string.Format("select OrderID from TransferToClog_Detail where ID = '{0}' group by OrderID", newID);
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
                if (!result)
                {
                    MessageBox.Show("Update orders data fail!");
                }

                bool prgResult, lastResult = false;
                foreach (DataRow currentRow in selectData.Rows)
                {
                    prgResult = Prgs.UpdateOrdersCTN(currentRow["OrderID"].ToString());
                    if (!prgResult)
                    {
                        lastResult = true;
                    }

                }
                if (lastResult)
                {
                    MessageBox.Show("Update orders data fail!");
                }

                //系統會自動有回傳值
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("No data need to import!");
                return;
            }
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
                                                             from PackingList a, PackingList_Detail b, Orders c, Country d where 1=0";
                DataTable selectDataTable;
                DualResult selectResult;
                if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out selectDataTable)))
                {
                    MessageBox.Show("Connection faile.!");
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
                            MessageBox.Show("Format is not correct!");
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
                                                                                  from PackingList_Detail
                                                                                  where ID = '{0}' and CTNStartNo = '{1}' and TransferToClogID = ''
                                                                                   ", dr["PackingListID"].ToString(), dr["CTNStartNo"].ToString());
                            if (MyUtility.Check.Seek(sqlCmd, out seekData))
                            {
                                dr["OrderID"] = seekData["OrderID"].ToString().Trim();
                                sqlCmd = string.Format(@"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,a.BuyerDelivery 
                                                                            from Orders a
                                                                             left join Country b on b.ID = a.Dest
                                                                            where a.ID = '{0}' and a.FtyGroup = '{1}'", dr["OrderID"].ToString(), Sci.Env.User.Factory);
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
                        MessageBox.Show("This data were all be transferred or order factory is not equal to login factory.");
                        return;
                    }
                }
            }
        }
    }
}
