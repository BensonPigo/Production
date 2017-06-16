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
using System.IO;
using System.Transactions;
using Sci.Production.PublicPrg;

namespace Sci.Production.Logistic
{
    public partial class P02 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm";
            dateTimePicker2.CustomFormat = "yyyy/MM/dd HH:mm";
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd 08:00");
            dateTimePicker2.Text = DateTime.Now.ToString("yyyy/MM/dd 12:00");
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            TextBox a = new TextBox();
            a.Text = Sci.Env.User.Keyword;
            this.txtcloglocationLocationNo.MDivisionObjectName = a;

            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.gridImport)
                 .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                 .Date("TransferDate", header: "Transfer Date", iseditingreadonly: true)
                 .Text("PackingListID", header: "PackId", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(4), iseditingreadonly: true)
                 .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
                 .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10))
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: true);
        }

        //Find
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text) && MyUtility.Check.Empty(this.txtPONo.Text) && MyUtility.Check.Empty(this.txtPackID.Text) && MyUtility.Check.Empty(this.dateTimePicker1.Text) && MyUtility.Check.Empty(this.dateTimePicker2.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > or < Order# > or < PackID > or <Receive Date> can not be empty!");
                return;
            }
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"Select Distinct '' as ID, 0 as selected,b.TransferDate, b.Id as PackingListID, b.OrderID, b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery,'' as ClogLocationId,'' as Remark 
from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) , Country d WITH (NOLOCK), TransferToClog t WITH (NOLOCK)
where b.OrderId = c.Id 
and a.Id = b.Id 
and b.CTNStartNo != '' 
and b.TransferDate is not null
and b.ReceiveDate is null
and c.Dest = d.ID 
and a.MDivisionID = '{0}' and (a.Type = 'B' or a.Type = 'L') and c.MDivisionID = '{0}'
and a.id = t.PackingListID", Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(" and b.OrderID = '{0}'", this.txtSPNo.Text.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(this.txtPONo.Text))
            {
                sqlCmd.Append(string.Format(" and c.CustPONo = '{0}'", this.txtPONo.Text.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(" and a.ID = '{0}'", this.txtPackID.Text.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(this.dateTimePicker1.Text))
            {
                sqlCmd.Append(string.Format(" and t.AddDate >= '{0}'", this.dateTimePicker1.Text.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(this.dateTimePicker2.Text))
            {
                sqlCmd.Append(string.Format(" and t.AddDate <= '{0}'", this.dateTimePicker2.Text.ToString().Trim()));
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
        private void button2_Click(object sender, EventArgs e)
        {
            //設定只能選txt檔
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //開窗且有選擇檔案
            {
                //先將Grid的結構給開出來
                string selectCommand = @"Select distinct '' as ID, 0 as selected, b.TransferDate, b.Id as PackingListID, b.OrderID, b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery, b.ClogLocationId, '' as Remark 
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
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        System.Diagnostics.Debug.WriteLine(line);
                        IList<string> sl = line.Split(" \t\r\n".ToCharArray());
                        if (sl[0] != "2")
                        {
                            MyUtility.Msg.WarningBox("Format is not correct!");
                            return;
                        }
                        else
                        {
                            DataRow dr = selectDataTable.NewRow();
                            try
                            {
                                dr["ID"] = "";
                                dr["selected"] = 0;
                                dr["PackingListID"] = sl[2].Substring(0, 13);
                                dr["CTNStartNo"] = sl[2].Substring(13);
                                dr["ClogLocationId"] = sl[1];
                            }
                            catch (Exception)
                            {
                                MyUtility.Msg.WarningBox("Format is not correct!");
                                return;
                            }

                            string sqlCmd = string.Format(@"
select pd.OrderID,pd.OrderShipmodeSeq,TransferDate,ReceiveDate ,p.MDivisionID
from PackingList_Detail pd WITH (NOLOCK) inner join PackingList p (NOLOCK) on pd.id = p.id
where pd.ID = '{0}' and pd.CTNStartNo = '{1}' and pd.CTNQty > 0"
                                , dr["PackingListID"].ToString(), dr["CTNStartNo"].ToString());
                            if (MyUtility.Check.Seek(sqlCmd, out seekData))
                            {
                                if (MyUtility.Check.Empty(seekData["ReceiveDate"]))
                                {
                                    if (MyUtility.Check.Empty(seekData["TransferDate"]))
                                    {
                                        dr["Remark"] = "This carton not yet transfer to clog.";
                                    }
                                    else
                                    {
                                        dr["selected"] = 1;
                                    }
                                }
                                else
                                {
                                    dr["Remark"] = "This carton already in clog.";
                                }
                                dr["OrderID"] = seekData["OrderID"];
                                dr["TransferDate"] = seekData["TransferDate"];

                                string seq = seekData["OrderShipmodeSeq"].ToString().Trim();
                                if (seekData["MDivisionID"].ToString().ToUpper() != Sci.Env.User.Keyword)
                                {
                                    dr["Remark"] = "The order's M is not equal to login M.";
                                }

                                sqlCmd = string.Format(@"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,oq.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                            left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{2}'
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'", dr["OrderID"].ToString(), Sci.Env.User.Keyword, seq);
                                if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                {
                                    dr["StyleID"] = seekData["StyleID"];
                                    dr["SeasonID"] = seekData["SeasonID"];
                                    dr["BrandID"] = seekData["BrandID"];
                                    dr["Customize1"] = seekData["Customize1"];
                                    dr["CustPONo"] = seekData["CustPONo"];
                                    dr["Alias"] = seekData["Alias"];
                                    dr["BuyerDelivery"] = seekData["BuyerDelivery"];
                                }
                            }
                            else
                            {
                                dr["Remark"] = "This carton is not in packing list.";
                            }
                            if (dr["Remark"].ToString().Trim() != "")
                            {
                                dr["selected"] = 0;
                            }
                            selectDataTable.Rows.Add(dr);
                        }
                    }
                    ControlButton4Text("Cancel");
                }
            }
        }

        //Save
        private void button3_Click(object sender, EventArgs e)
        {
            //檢查是否有勾選資料
            this.gridImport.ValidateControl();
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                MyUtility.Msg.WarningBox("No data need to import!");
                return;
            }
            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("No data need to import!");
                return;
            }
            foreach (DataRow dr in selectedData)
            {
                if (dr["Remark"].ToString().Trim() != "")
                {
                    MyUtility.Msg.WarningBox("Some data cannot be received, please check again.");
                    return;
                }
            }

            IList<string> insertCmds = new List<string>();
            IList<string> updateCmds = new List<string>();
            //組要Insert進TransferToClog的資料
            foreach (DataRow dr in selectedData)
            {
                insertCmds.Add(string.Format(@"insert into ClogReceive(ReceiveDate,MDivisionID,PackingListID,OrderID,CTNStartNo,ClogLocationId, AddDate)
values (GETDATE(),'{0}','{1}','{2}','{3}','{4}',GETDATE());", Sci.Env.User.Keyword, MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["CTNStartNo"]), MyUtility.Convert.GetString(dr["ClogLocationId"])));
                //要順便更新PackingList_Detail
                updateCmds.Add(string.Format(@"update PackingList_Detail 
set ReceiveDate = GETDATE(), ClogLocationId = '{3}', ReturnDate = null 
where ID = '{0}' and OrderID = '{1}' and CTNStartNo = '{2}'; ", MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["CTNStartNo"]), MyUtility.Convert.GetString(dr["ClogLocationId"])));
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

        //Cancel
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Update All Location
        private void button5_Click(object sender, EventArgs e)
        {
            string location = this.txtcloglocationLocationNo.Text.Trim();
            int pos = this.listControlBindingSource1.Position;     //記錄目前指標位置
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first!");
                return;
            }

            foreach (DataRow currentRecord in dt.Rows)
            {
                currentRecord["ClogLocationId"] = location;
            }
            this.listControlBindingSource1.Position = pos;
            gridImport.SuspendLayout();
            this.gridImport.DataSource = null;
            this.gridImport.DataSource = listControlBindingSource1;
            this.listControlBindingSource1.Position = pos;
            gridImport.ResumeLayout();
        }

        private void ControlButton4Text(string showText)
        {
            btnClose.Text = showText;
        }
    }
}
