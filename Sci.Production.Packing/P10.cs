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
using System.IO;
using System.Transactions;
using Sci.Production.PublicPrg;

namespace Sci.Production.Packing
{
    public partial class P10 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.gridDetail)
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
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSP.Text) && MyUtility.Check.Empty(this.txtPO.Text) && MyUtility.Check.Empty(this.txtPackID.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > or < Order# > or < PackID > can not be empty!");
                return;
            }
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"Select Distinct '' as ID, 1 as selected, b.Id as PackingListID, b.OrderID, b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery 
                                                         from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) , Country d WITH (NOLOCK) 
                                                         where b.OrderId = c.Id 
                                                         and a.Id = b.Id 
                                                         and b.CTNStartNo != '' 
                                                         and ((b.ReturnDate is null and TransferDate is null) or b.ReturnDate is not null) 
                                                         and c.Dest = d.ID 
                                                         and a.MDivisionID = '{0}' and (a.Type = 'B' or a.Type = 'L') and c.MDivisionID = '{0}'", Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlCmd.Append(string.Format(" and b.OrderID = '{0}'", this.txtSP.Text.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(this.txtPO.Text))
            {
                sqlCmd.Append(string.Format(" and c.CustPONo = '{0}'", this.txtPO.Text.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(" and a.ID = '{0}'", this.txtPackID.Text.ToString().Trim()));
            }

            DataTable selectDataTable;
            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, sqlCmd.ToString(), out selectDataTable))
            {
                if (selectDataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    ControlButton4Text("Close");
                }
                else
                {
                    ControlButton4Text("Cancel");
                }
            }
            listControlBindingSource1.DataSource = selectDataTable;
        }

        //Import From Barcode
        private void btnImportFromBarcode_Click(object sender, EventArgs e)
        {
            //設定只能選txt檔
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //開窗且有選擇檔案
            {
                //先將Grid的結構給開出來
                string selectCommand = @"Select distinct '' as ID, 1 as selected, b.Id as PackingListID, b.OrderID, b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery 
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
                    DataTable MissData = selectDataTable.Clone();
                    int insertCount = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        System.Diagnostics.Debug.WriteLine(line);
                        IList<string> sl = line.Split(" \t\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (sl.Count == 0 || sl[0] != "1")
                        {
                            MyUtility.Msg.WarningBox("Format is not correct!");
                            return;
                        }
                        else
                        {
                            DataRow dr = selectDataTable.NewRow();
                            dr["ID"] = "";
                            dr["selected"] = 1;                          
                            dr["PackingListID"] = sl[1].Substring(0, 13);
                            dr["CTNStartNo"] = sl[1].Substring(13,sl[1].Length-13);                           
                           
                            string sqlCmd = string.Format(@"select OrderID,OrderShipmodeSeq  
                                                                                  from PackingList_Detail WITH (NOLOCK) 
                                                                                  where ID = '{0}' and CTNStartNo = '{1}' and CTNQty > 0 and TransferDate is null
                                                                                   ", dr["PackingListID"].ToString(), dr["CTNStartNo"].ToString());
                            if (MyUtility.Check.Seek(sqlCmd, out seekData))
                            {
                                dr["OrderID"] = seekData["OrderID"].ToString().Trim();
                                string seq = seekData["OrderShipmodeSeq"].ToString().Trim();
                                sqlCmd = string.Format(@"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,oq.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                            left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{2}'
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'", dr["OrderID"].ToString(), Sci.Env.User.Keyword, seq);
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
                                else
                                {
                                    MissData.Rows.Add(dr.ItemArray);
                                }
                            }
                            else
                            {
                                MissData.Rows.Add(dr.ItemArray);
                            }
                        }
                    }
                    if (insertCount == 0)
                    {
                        MyUtility.Msg.WarningBox("All data were transferred or order's M is not equal to login M.");
                        return;
                    }
                    if (MissData.Rows.Count > 0)
                    {
                        StringBuilder warningmsg = new StringBuilder();
                        warningmsg.Append("Some data were transferred or order's M is not equal to login M." + Environment.NewLine);
                        foreach (DataRow missData in MissData.Rows)
                        {
                            warningmsg.Append(string.Format(@"PackingListID: {0} CTN#: {1} " + Environment.NewLine, missData["PackingListID"], missData["CTNStartNo"]));
                        }
                        MyUtility.Msg.WarningBox(warningmsg.ToString());
                    }
                    ControlButton4Text("Cancel");
                }
            }
        }

        //Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            //檢查是否有勾選資料
            this.gridDetail.ValidateControl();
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt)) return;
            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("No data need to import!");
                return;
            }

            IList<string> insertCmds = new List<string>();
            IList<string> updateCmds = new List<string>();
            //組要Insert進TransferToClog的資料
            foreach (DataRow dr in selectedData)
            {
                insertCmds.Add(string.Format(@"insert into TransferToClog(TransferDate,MDivisionID,PackingListID,OrderID,CTNStartNo, AddDate)
values (GETDATE(),'{0}','{1}','{2}','{3}',GETDATE());", Sci.Env.User.Keyword, MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["CTNStartNo"])));
                //要順便更新PackingList_Detail
                updateCmds.Add(string.Format(@"update PackingList_Detail 
set TransferDate = GETDATE(), ReceiveDate = null, ClogLocationId = '', ReturnDate = null 
where ID = '{0}' and OrderID = '{1}' and CTNStartNo = '{2}'; ", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["CTNStartNo"])));
            }

            //Update Orders的資料
            DataTable selectData = null;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(dt, "Selected,OrderID", @"select distinct OrderID
from #tmp a
where a.Selected = 1", out selectData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Prepare update orders data fail!\r\n" + ex.ToString());
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
                    if (insertCmds.Count > 0)
                    {
                        result2 = Sci.Data.DBProxy.Current.Executes(null, insertCmds);
                    }
                    DualResult prgResult = Prgs.UpdateOrdersCTN(selectData);

                    if (result1 && result2 && prgResult)
                    {
                        transactionScope.Complete();
                        transactionScope.Dispose();
                        ControlButton4Text("Close");
                        MyUtility.Msg.InfoBox("Complete!!");
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
        }

        //Close/Cancel
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ControlButton4Text(string showText)
        {
            btnClose.Text = showText;
        }
    }
}
