﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Transactions;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Logistic
{
    public partial class P02_ImportFromBarCode : Sci.Win.Subs.Base
    {
        IList<P02_FileInfo> filelists = new List<P02_FileInfo>();
        DataTable grid2Data;
        IList<DataRow> groupData = new List<DataRow>();
        IList<DataRow> TransferIDData = new List<DataRow>();

        public P02_ImportFromBarCode()
        {
            InitializeComponent();

            filelists.Add(new P02_FileInfo(null, null));
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            listControlBindingSource1.DataSource = filelists;
            this.gridFileList.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.gridFileList)
                .Text("Filename", header: "File Name", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Button("Get File", null, header: "Get File", width: Widths.AnsiChars(13), onclick: eh_getfile);

            listControlBindingSource2.DataSource = grid2Data;
            this.gridLocationNo.DataSource = listControlBindingSource2;
            this.gridLocationNo.IsEditingReadOnly = true;

            Helper.Controls.Grid.Generator(this.gridLocationNo)
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10))
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13))
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6))
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10))
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10))
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10))
                .Text("Alias", header: "Destination#", width: Widths.AnsiChars(10))
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10))
                .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(13))
                .Text("ClogReceiveID", header: "Received ID", width: Widths.AnsiChars(13))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(15));
        }

        //Get File開窗選檔案
        private void eh_getfile(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //開窗且有選擇檔案
            {
                ((P02_FileInfo)(listControlBindingSource1.Current)).Fullfilename = openFileDialog1.FileName;
                ((P02_FileInfo)(listControlBindingSource1.Current)).Filename = openFileDialog1.SafeFileName;
            }
        }

        //Append
        private void button1_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.Add(new P02_FileInfo(null, null));
            listControlBindingSource1.MoveLast();
            this.eh_getfile(sender, e);
        }

        //Delete
        private void button2_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.Position != -1)
            {
                listControlBindingSource1.RemoveCurrent();
            }
        }

        //Import Data
        private void button3_Click(object sender, EventArgs e)
        {
            //清空Grid資料
            if (grid2Data != null)
            {
                grid2Data.Clear();
            }
            groupData.Clear();

            #region 檢查所有檔案格式是否正確
            string errorMsg = "";
            int count = 0;
            foreach (P02_FileInfo dr in (IList<P02_FileInfo>)listControlBindingSource1.DataSource)
            {
                if (!MyUtility.Check.Empty(dr.Filename))
                {
                    using (StreamReader reader = new StreamReader(dr.Fullfilename, System.Text.Encoding.UTF8))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            System.Diagnostics.Debug.WriteLine(line);
                            IList<string> sl = line.Split(" \t\r\n".ToCharArray());
                            if (sl[0] != "2")
                            {
                                errorMsg = errorMsg + dr.Filename.Trim() + "\r\n";
                                break;
                            }
                        }
                    }
                }
                count++;
            }
            #endregion

            #region 如果都沒有資料的話出訊息告知且不做任何動作
            if (count == 0)
            {
                MyUtility.Msg.WarningBox("File list is empty!");
                return;
            }
            #endregion

            #region 若有格式不正確的就出訊息告之使用者哪些檔案格式錯誤且不做任何動作
            if (!MyUtility.Check.Empty(errorMsg))
            {
                MyUtility.Msg.WarningBox("File Name: \r\n" + errorMsg + "Format is not correct!");
                return;
            }
            #endregion

            #region 準備結構
            string selectCommand = @"select a.ClogLocationId,a.OrderID,a.CTNStartNo,b.StyleID,b.SeasonID,b.BrandID,b.Customize1,b.CustPONo,c.Alias,
                                                            b.BuyerDelivery,a.ID as PackingListID,a.TransferToClogId,a.ClogReceiveID,'' as ID,'' as Remark, b.MDivisionID, 1 as InsertData 
                                                            from PackingList_Detail a WITH (NOLOCK) , Orders b WITH (NOLOCK) , Country c WITH (NOLOCK)  where 1=0";
            DataTable groupTable;
            DualResult selectResult;
            if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out grid2Data)))
            {
                MyUtility.Msg.WarningBox("Connection faile!\r\n"+selectResult.ToString());
                return;
            }

            selectCommand = "Select ID, ReceiveDate, MDivisionID from ClogReceive WITH (NOLOCK) where 1 = 0";
            if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out groupTable)))
            {
                MyUtility.Msg.WarningBox("Connection faile!\r\n" + selectResult.ToString());
                return;
            }

            #endregion

            #region 檢查通過後開始讀檔
            DataRow seekPacklistData, seekClogReceiveData, seekOrderdata;
            int insertCount = 0;
            int recordCount;
            DataRow[] findRow;
            foreach (P02_FileInfo dr in (IList<P02_FileInfo>)listControlBindingSource1.DataSource)
            {
                if (!MyUtility.Check.Empty(dr.Filename))
                {
                    using (StreamReader reader = new StreamReader(dr.Fullfilename, System.Text.Encoding.UTF8))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            System.Diagnostics.Debug.WriteLine(line);
                            IList<string> sl = line.Split(" \t\r\n".ToCharArray());
                            //如果有資料重複就不再匯入重複的資料
                            findRow = grid2Data.Select(string.Format("PackingListID = '{0}' and CTNStartNo = '{1}'", sl[2].Substring(0, 13), sl[2].Substring(13).Trim()));
                            if (findRow.Length == 0)
                            {
                                DataRow dr1 = grid2Data.NewRow();
                                dr1["ID"] = "";
                                dr1["ClogLocationId"] = sl[1];
                                dr1["PackingListID"] = sl[2].Substring(0, 13);
                                dr1["CTNStartNo"] = sl[2].Substring(13);
                                //dr1["MDivisionID"] = sl[2].Substring(0, 3);
                                dr1["MDivisionID"] = Sci.Env.User.Keyword;
                                dr1["InsertData"] = 1;
                                string sqlCmd = string.Format(@"select OrderID, TransferToClogID, ClogReceiveID 
                                                                                  from PackingList_Detail WITH (NOLOCK) 
                                                                                  where ID = '{0}' and CTNStartNo = '{1}' and CTNQty = 1", dr1["PackingListID"].ToString(), dr1["CTNStartNo"].ToString());
                                if (MyUtility.Check.Seek(sqlCmd, out seekPacklistData))
                                {
                                    dr1["OrderID"] = seekPacklistData["OrderID"].ToString().Trim();
                                    dr1["ClogReceiveID"] = seekPacklistData["ClogReceiveID"].ToString().Trim();
                                    dr1["TransferToClogId"] = seekPacklistData["TransferToClogID"].ToString().Trim();
                                    if (MyUtility.Check.Empty(dr1["ClogReceiveID"]))
                                    {
                                        if (MyUtility.Check.Empty(seekPacklistData["TransferToClogID"]))
                                        {
                                            dr1["Remark"] = "This carton not yet transfer to clog.";
                                        }
                                        else
                                        {
                                            sqlCmd = string.Format(@"select a.ID 
                                                                                    from ClogReceive a WITH (NOLOCK) , ClogReceive_Detail b 
                                                                                    where a.ID = b.ID and b.PackingListID = '{0}' and b.CTNStartNo = '{1}'  and a.Status = 'New'
                                                                                    ", dr1["PackingListID"].ToString(), dr1["CTNStartNo"].ToString());
                                            if (MyUtility.Check.Seek(sqlCmd, out seekClogReceiveData))
                                            {
                                                dr1["ClogReceiveID"] = seekClogReceiveData["ID"].ToString().Trim();
                                                dr1["Remark"] = "This carton already in clog, but not yet encode.";
                                                dr1["InsertData"] = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dr1["Remark"] = "This carton already in clog.";
                                        dr1["InsertData"] = 0;
                                    }
                                    sqlCmd = string.Format(@"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,a.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                             left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            where a.ID = '{0}'", dr1["OrderID"].ToString());
                                    if (MyUtility.Check.Seek(sqlCmd, out seekOrderdata))
                                    {
                                        dr1["StyleID"] = seekOrderdata["StyleID"].ToString().Trim();
                                        dr1["SeasonID"] = seekOrderdata["SeasonID"].ToString().Trim();
                                        dr1["BrandID"] = seekOrderdata["BrandID"].ToString().Trim();
                                        dr1["Customize1"] = seekOrderdata["Customize1"].ToString().Trim();
                                        dr1["CustPONo"] = seekOrderdata["CustPONo"].ToString().Trim();
                                        dr1["Alias"] = seekOrderdata["Alias"].ToString().Trim();
                                        dr1["BuyerDelivery"] = seekOrderdata["BuyerDelivery"];
                                    }
                                }
                                else
                                {
                                    dr1["Remark"] = "This carton is not in packing list.";
                                }
                                grid2Data.Rows.Add(dr1);
                                insertCount++;

                                if (MyUtility.Check.Empty(dr1["ClogReceiveID"]) && (int)dr1["InsertData"] == 1)
                                {
                                    recordCount = groupData.Where(x => x["MDivisionID"].ToString() == dr1["MDivisionID"].ToString()).Count();
                                    if (recordCount == 0)
                                    {
                                        DataRow dr2 = groupTable.NewRow();
                                        dr2["ID"] = "";
                                        dr2["ReceiveDate"] = DBNull.Value;
                                        dr2["MDivisionID"] = dr1["MDivisionID"];
                                        groupData.Add(dr2);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            listControlBindingSource2.DataSource = null;
            listControlBindingSource2.DataSource = grid2Data;
            this.numTotalCartonScanned.Value = insertCount;
            #endregion
        }

        //To Excel
        private void button4_Click(object sender, EventArgs e)
        {
            DataTable ExcelTable;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource2.DataSource, "ClogLocationId,OrderId,CTNStartNo,StyleID,SeasonID,BrandID,Customize1,CustPONo,Alias,BuyerDelivery,PackingListID,ClogReceiveID,Remark", "select * from #tmp", out ExcelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }

            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            // Display OpenFileDialog by calling ShowDialog method ->ShowDialog()
            // Get the selected file name and CopyToXls
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            {
                // Open document
                bool result = MyUtility.Excel.CopyToXls(ExcelTable, dlg.FileName, xltfile: "Logistic_P02_ImportFromBarCode.xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            else
            {
                return;
            }
        }

        //Save
        private void button5_Click(object sender, EventArgs e)
        {
            //檢查Receive Date不可為空值，若為空值則出訊息告知且不做任何動作
            if (MyUtility.Check.Empty(this.dateReceiveDate.Value))
            {
                MyUtility.Msg.WarningBox("Receive date can't empty!");
                return;
            }
            
            //將Append、Delete、Import Data這三個按鈕都改成Disable，Cancel按鈕的字樣改成Close
            this.btnAppend.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnImportData.Enabled = false;
            this.btnCancel.Text = "Close";

            string newID = "";
            string sqlInsertMaster, sqlInsertDetail;
            string lostCTN = "";
            DataTable selectDataTable;
            DualResult selectResult;
            string sqlCmd = "select TransferToClogId from ClogReceive_Detail where 1 = 0";
            if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out selectDataTable)))
            {
                MyUtility.Msg.WarningBox("Connection faile!");
                return;
            }

            foreach (DataRow dr in groupData)
            {
                if (MyUtility.Check.Empty(dr["ID"]))
                {
                    newID = MyUtility.GetValue.GetID(dr["MDivisionID"].ToString().Trim() + "CR", "ClogReceive", Convert.ToDateTime(this.dateReceiveDate.Value), 2, "Id", null);
                    if (MyUtility.Check.Empty(newID))
                    {
                        MyUtility.Msg.WarningBox("GetID fail, please try again!");
                        return;
                    }

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        bool detailAllSuccess = true;
                        try
                        {
                            sqlInsertMaster = @"insert into ClogReceive(ID, ReceiveDate, MDivisionID,Status, AddName, AddDate) 
                                                               values(@id, @receiveDate, @mdivisionid, @status, @addName, @addDate)";

                            #region 準備Master sql參數資料
                            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                            sp1.ParameterName = "@id";
                            sp1.Value = newID;

                            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                            sp2.ParameterName = "@receiveDate";
                            sp2.Value = this.dateReceiveDate.Value;

                            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                            sp3.ParameterName = "@mdivisionid";
                            sp3.Value = dr["MDivisionID"].ToString().Trim();

                            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                            sp4.ParameterName = "@addName";
                            sp4.Value = Sci.Env.User.UserID;

                            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                            sp5.ParameterName = "@addDate";
                            sp5.Value = DateTime.Now;

                            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                            sp6.ParameterName = "@status";
                            sp6.Value = "New";

                            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                            cmds.Add(sp1);
                            cmds.Add(sp2);
                            cmds.Add(sp3);
                            cmds.Add(sp4);
                            cmds.Add(sp5);
                            cmds.Add(sp6);
                            #endregion
                            DualResult result1 = Sci.Data.DBProxy.Current.Execute(null, sqlInsertMaster, cmds);

                            #region 宣告Detail sql參數
                            IList<System.Data.SqlClient.SqlParameter> detailcmds = new List<System.Data.SqlClient.SqlParameter>();
                            System.Data.SqlClient.SqlParameter detail1 = new System.Data.SqlClient.SqlParameter();
                            System.Data.SqlClient.SqlParameter detail2 = new System.Data.SqlClient.SqlParameter();
                            System.Data.SqlClient.SqlParameter detail3 = new System.Data.SqlClient.SqlParameter();
                            System.Data.SqlClient.SqlParameter detail4 = new System.Data.SqlClient.SqlParameter();
                            System.Data.SqlClient.SqlParameter detail5 = new System.Data.SqlClient.SqlParameter();
                            System.Data.SqlClient.SqlParameter detail6 = new System.Data.SqlClient.SqlParameter();
                            System.Data.SqlClient.SqlParameter detail7 = new System.Data.SqlClient.SqlParameter();
                            System.Data.SqlClient.SqlParameter detail8 = new System.Data.SqlClient.SqlParameter();
                            detail1.ParameterName = "@id";
                            detail2.ParameterName = "@transferToClogId";
                            detail3.ParameterName = "@packingListId";
                            detail4.ParameterName = "@orderId";
                            detail5.ParameterName = "@ctnStartNo";
                            detail6.ParameterName = "@clogLocationId";
                            detail7.ParameterName = "@addName";
                            detail8.ParameterName = "@addDate";
                            detailcmds.Add(detail1);
                            detailcmds.Add(detail2);
                            detailcmds.Add(detail3);
                            detailcmds.Add(detail4);
                            detailcmds.Add(detail5);
                            detailcmds.Add(detail6);
                            detailcmds.Add(detail7);
                            detailcmds.Add(detail8);
                            #endregion
                            foreach (DataRow dr1 in grid2Data.Rows)
                            {
                                if (dr1["MDivisionID"].ToString().Trim() == dr["MDivisionID"].ToString().Trim())
                                {
                                    dr1["ID"] = newID; //將ID寫入Grid2的Received ID欄位
                                    dr1["ClogReceiveID"] = newID;
                                    sqlInsertDetail = @"insert into ClogReceive_Detail (ID, TransferToClogId, PackingListId, OrderId, CTNStartNo, ClogLocationId, AddName, AddDate)
                                                                     values (@id,@transferToClogId,@packingListId,@orderId,@ctnStartNo,@clogLocationId,@addName,@addDate)";
                                    #region 準備Detail sql參數資料
                                    detail1.Value = newID;
                                    detail2.Value = dr1["TransferToClogId"].ToString().Trim();
                                    detail3.Value = dr1["PackingListId"].ToString().Trim();
                                    detail4.Value = dr1["OrderId"].ToString().Trim();
                                    detail5.Value = dr1["CTNStartNo"].ToString().Trim();
                                    detail6.Value = dr1["ClogLocationId"].ToString().Trim();
                                    detail7.Value = Sci.Env.User.UserID;
                                    detail8.Value = DateTime.Now;
                                    #endregion
                                    DualResult result2 = Sci.Data.DBProxy.Current.Execute(null, sqlInsertDetail, detailcmds);

                                    if (!result2)
                                    {
                                        detailAllSuccess = false;
                                        break;
                                    }
                                    if (!MyUtility.Check.Empty(dr1["Remark"].ToString()))
                                    {
                                        lostCTN = lostCTN + string.Format("PACK ID: '{0}'  SP#:'{1}'  CTN#:'{2}' \r\n", dr1["PackingListId"].ToString(), dr1["OrderId"].ToString(), dr1["CTNStartNo"].ToString().Trim());
                                    }

                                    //記錄此次Import的所有TransferToClogID
                                    if (!MyUtility.Check.Empty(dr1["TransferToClogId"].ToString()))
                                    {
                                        int recordCount = TransferIDData.Where(x => x["TransferToClogId"].ToString() == dr1["TransferToClogId"].ToString()).Count();
                                        if (recordCount == 0)
                                        {
                                            DataRow dr2 = selectDataTable.NewRow();
                                            dr2["TransferToClogId"] = dr1["TransferToClogId"].ToString();
                                            TransferIDData.Add(dr2);
                                        }
                                    }
                                }
                            }

                            if (result1 && detailAllSuccess)
                            {
                                transactionScope.Complete();
                                transactionScope.Dispose();
                                dr["ID"] = newID;
                                dr["ReceiveDate"] = this.dateReceiveDate.Value;
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
            }

            #region 存檔完成後要出訊息告知使用者匯入的資料中整份的TransferToClogId還缺少哪箱子沒有送達Clog或者多送哪些箱子到Clog(即在TransferToClog中找不到的)
            bool lackMsg = false;
            if (!MyUtility.Check.Empty(lostCTN))
            {
                lostCTN = "Wrong Rev# \r\n" + lostCTN + "\r\nLacking Rev# \r\n";
                lackMsg = true;
            }
            else
            {
                lostCTN = "Lacking Rev# \r\n";
            }

            DataTable transferToClogData;
            foreach (DataRow findDataRow in TransferIDData)
            {
                sqlCmd = string.Format(@"select a.PackingListID, a.OrderID, a.CTNStartNo, isnull(b.Id,'') as ReceiveID 
                                                            from TransferToClog_Detail a 
                                                            left Join ClogReceive_Detail b 
                                                                on b.TransferToClogId = a.ID 
                                                                and b.PackingListId = a.PackingListId 
                                                                and b.OrderId = a.OrderId 
                                                                and b.CTNStartNo = a.CTNStartNo 
                                                            where a.ID = '{0}'", findDataRow["TransferToClogId"].ToString().Trim());

                if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out transferToClogData)))
                {
                    MyUtility.Msg.WarningBox("Connection faile!");
                    return;
                }
                foreach (DataRow transferRow in transferToClogData.Rows)
                {
                    if (transferRow["ReceiveID"].ToString().Trim() == "")
                    {
                        lostCTN = lostCTN + string.Format("Trans. Slip#: '{0}'  PACK ID:'{1}'  SP#:'{2}'  CTN#:'{3}' \r\n", findDataRow["TransferToClogId"].ToString(), transferRow["PackingListId"].ToString(), transferRow["OrderId"].ToString(), transferRow["CTNStartNo"].ToString().Trim());
                        lackMsg = true;
                    }
                }
            }

            if (lackMsg)
            {
                MyUtility.Msg.WarningBox(lostCTN);
            }
            else
            {
                MyUtility.Msg.InfoBox("Import complete!");
            }
            #endregion
        }

        // Cancel, Close
        private void button6_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Text == "Cancel")
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            this.Close();
        }
    }
}
