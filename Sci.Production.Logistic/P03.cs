using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.IO;
using System.Transactions;
using Sci.Production.PublicPrg;
using System.Linq;

namespace Sci.Production.Logistic
{
    public partial class P03 : Sci.Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePicker2.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd 08:00");
            this.dateTimePicker2.Text = DateTime.Now.ToString("yyyy/MM/dd 12:00");
        }

        private DataTable selectDataTable;
        private string selectDataTable_DefaultView_Sort = string.Empty;

        public string SelectDataTable_DefaultView_Sort
        {
            get
            {
                return this.selectDataTable_DefaultView_Sort;
            }

            set
            {
                this.selectDataTable_DefaultView_Sort = value;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridReceiveDate.IsEditingReadOnly = false;
            this.gridReceiveDate.DataSource = this.listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.gridReceiveDate)
                 .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                 .Date("ReceiveDate", header: "Receive Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("PackingListID", header: "PackId", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("FtyGroup", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(4), iseditingreadonly: true)
                 .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
                 .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: true);

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int rowIndex = 0;
            int columIndex = 0;
            this.gridReceiveDate.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.gridReceiveDate.Sorted += (s, e) =>
            {
                if ((rowIndex == -1) & (columIndex == 4))
                {
                    this.listControlBindingSource1.DataSource = null;

                    if (this.selectDataTable_DefaultView_Sort == "DESC")
                    {
                        this.selectDataTable.DefaultView.Sort = "rn1 DESC";
                        this.selectDataTable_DefaultView_Sort = string.Empty;
                    }
                    else
                    {
                        this.selectDataTable.DefaultView.Sort = "rn1 ASC";
                        this.selectDataTable_DefaultView_Sort = "DESC";
                    }

                    this.listControlBindingSource1.DataSource = this.selectDataTable;
                    return;
                }
            };
        }

        private void Find()
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text) && MyUtility.Check.Empty(this.txtPONo.Text) && MyUtility.Check.Empty(this.txtPackID.Text) && MyUtility.Check.Empty(this.dateTimePicker1.Text) && MyUtility.Check.Empty(this.dateTimePicker2.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > or < Order# > or < PackID > or <Receive Date> can not be empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(
                @"
select  ID
        , selected
        , ReceiveDate
        , PackingListID
        , FtyGroup
        , OrderID
        , CTNStartNo
	    , CustPONo
        , StyleID
        , SeasonID
        , BrandID
        , Customize1
        , Alias
        , BuyerDelivery
        , ClogLocationId
        , Remark 
        , rn = ROW_NUMBER() over (order by PackingListID, OrderID, (RIGHT (REPLICATE ('0', 6) + rtrim (ltrim (CTNStartNo)), 6)))
        , rn1 = ROW_NUMBER() over (order by TRY_CONVERT (int, CTNStartNo), (RIGHT (REPLICATE ('0', 6) + rtrim (ltrim (CTNStartNo)), 6)))	
from (
    Select  distinct '' as ID
            , 1 as selected
            , b.ReceiveDate
            , a.Id as PackingListID
            , c.FtyGroup
            , b.OrderID
            , b.CTNStartNo
	        , c.CustPONo
            , c.StyleID
            , c.SeasonID
            , c.BrandID
            , c.Customize1
            , d.Alias
            , c.BuyerDelivery
            , b.ClogLocationId
            , '' as Remark 
    from PackingList a WITH (NOLOCK) 
         , PackingList_Detail b WITH (NOLOCK) 
         , Orders c WITH (NOLOCK) 
         , Country d WITH (NOLOCK) 
         , TransferToClog t WITH (NOLOCK)
	where   b.OrderId = c.Id 
	        and a.Id = b.Id 
	        and b.CTNStartNo != '' 
	        and b.ReceiveDate is not null
	        and c.Dest = d.ID 
            and a.MDivisionID = '{0}' 
            and (a.Type = 'B' or a.Type = 'L') 
            and c.MDivisionID = '{0}'
            and a.id = t.PackingListID
", Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(@" 
            and b.OrderID = '{0}'", this.txtSPNo.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtPONo.Text))
            {
                sqlCmd.Append(string.Format(@" 
            and c.CustPONo = '{0}'", this.txtPONo.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(@" 
            and a.ID = '{0}'", this.txtPackID.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.dateTimePicker1.Text))
            {
                sqlCmd.Append(string.Format(@" 
            and b.ReceiveDate >= '{0}'", this.dateTimePicker1.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.dateTimePicker2.Text))
            {
                sqlCmd.Append(string.Format(@" 
            and b.ReceiveDate <= '{0}'", this.dateTimePicker2.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlCmd.Append(string.Format(@" and c.FtyGroup = '{0}'", this.txtfactory.Text.Trim()));
            }

            sqlCmd.Append(@"
) a
order by rn ");

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
                    this.ControlButton4Text("Cancel");
                }
            }

            this.listControlBindingSource1.DataSource = this.selectDataTable;
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        // Import From Barcode
        private void BtnImportFromBarcode_Click(object sender, EventArgs e)
        {
            // 設定只能選txt檔
            this.openFileDialog1.Filter = "txt files (*.txt)|*.txt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 先將Grid的結構給開出來
                string selectCommand = @"
Select distinct '' as ID, 0 as selected,b.ReceiveDate, b.Id as PackingListID, b.OrderID, 
TRY_CONVERT(int,b.CTNStartNo) as 'CTNStartNo'
,0 as rn
,0 as rn1
, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery, b.ClogLocationId, '' as Remark 
from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) , Country d WITH (NOLOCK) where 1=0";

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
                        IList<string> sl = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sl[0] != "3")
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
                            string sqlCmd = string.Format(@"
select pd.OrderID,pd.OrderShipmodeSeq,pd.ReceiveDate,pd.ReturnDate,pd.ClogLocationId,p.MDivisionID
from PackingList_Detail pd WITH (NOLOCK)  inner join PackingList p (NOLOCK) on pd.id = p.id
where pd.ID = '{0}' and CTNStartNo = '{1}' and pd.CTNQty > 0",
                                dr["PackingListID"].ToString(), dr["CTNStartNo"].ToString());
                            if (MyUtility.Check.Seek(sqlCmd, out seekData))
                            {
                                if (MyUtility.Check.Empty(seekData["ReturnDate"]))
                                {
                                    if (MyUtility.Check.Empty(seekData["ReceiveDate"]))
                                    {
                                        dr["Remark"] = "This carton not yet send to Clog.";
                                    }
                                    else
                                    {
                                        dr["selected"] = 1;
                                    }
                                }
                                else
                                {
                                    dr["Remark"] = "This carton has been return.";
                                }

                                if (seekData["MDivisionID"].ToString().ToUpper() != Sci.Env.User.Keyword)
                                {
                                    dr["Remark"] = "The order's M is not equal to login M.";
                                }

                                dr["OrderID"] = seekData["OrderID"];
                                dr["ClogLocationId"] = seekData["ClogLocationId"];
                                dr["ReceiveDate"] = seekData["ReceiveDate"];
                                string seq = MyUtility.Convert.GetString(seekData["OrderShipmodeSeq"]).Trim();
                                sqlCmd = string.Format(@"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,oq.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                            left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{2}'
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'", MyUtility.Convert.GetString(dr["OrderID"]), Sci.Env.User.Keyword, seq);
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

                            this.selectDataTable.Rows.Add(dr);
                        }
                    }

                    this.ControlButton4Text("Cancel");
                }
            }
        }

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 檢查是否有勾選資料
            this.gridReceiveDate.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first!");
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
                insertCmds.Add(string.Format(@"insert into ClogReturn(ReturnDate,MDivisionID,PackingListID,OrderID,CTNStartNo, AddDate)
values (GETDATE(),'{0}','{1}','{2}','{3}',GETDATE());", Sci.Env.User.Keyword, MyUtility.Convert.GetString(dr["PackingListID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["CTNStartNo"])));
                //要順便更新PackingList_Detail
                updateCmds.Add(string.Format(@"update PackingList_Detail 
set TransferDate = null, ReceiveDate = null, ClogLocationId = '', ReturnDate = GETDATE()
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
                        this.ControlButton4Text("Close");
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
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            if (dt.AsEnumerable().Any(row => !row["Selected"].EqualDecimal(1)))
            {
                this.listControlBindingSource1.DataSource = dt.AsEnumerable().Where(row => !row["Selected"].EqualDecimal(1)).CopyToDataTable();
            }
            else
            {
                this.listControlBindingSource1.DataSource = null;
            }
        }

        // Cancel
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ControlButton4Text(string showText)
        {
            btnClose.Text = showText;
        }
    }
}
