using System;
using System.Collections.Generic;
using System.Data;
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
    /// <summary>
    /// Logistic_P02
    /// </summary>
    public partial class P02 : Win.Tems.QueryForm
    {
        private DataTable selectDataTable = new DataTable();

        /// <summary>
        /// P02
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePicker2.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd 08:00");
            this.dateTimePicker2.Text = DateTime.Now.ToString("yyyy/MM/dd 12:00");
        }

        private string selectDataTable_DefaultView_Sort = string.Empty;

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            TextBox a = new TextBox
            {
                Text = Env.User.Keyword,
            };
            this.txtcloglocationLocationNo.MDivisionObjectName = a;

            DataGridViewGeneratorCheckBoxColumnSettings col_chk = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_chk.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);
                dr["selected"] = e.FormattedValue;
                dr.EndEdit();
                int sint = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").Length;
                this.numSelectedCTNQty.Value = sint;
            };
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: col_chk)
            .Date("TransferDate", header: "Transfer Date", iseditingreadonly: true)
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
            .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10))
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: true);

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int rowIndex = 0;
            int columIndex = 0;
            this.gridImport.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.gridImport.Sorted += (s, e) =>
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

        // Find
        private void Find()
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text) && MyUtility.Check.Empty(this.txtPONo.Text) && MyUtility.Check.Empty(this.txtPackID.Text) && MyUtility.Check.Empty(this.dateTimePicker1.Text) && MyUtility.Check.Empty(this.dateTimePicker2.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > or < PO# > or < PackID > or <Transfer Date> can not be empty!");
                return;
            }

            this.numSelectedCTNQty.Value = 0;
            this.numTotalCTNQty.Value = 0;
            #region SQL Filte

            // 若有輸入 TransferSlipNo 必須增加條件對應至 PackingList_Detail 【OrderID, CtnNum】
            string strTransferSlipNoFilte = string.Format(@"and t.TransferSlipNo = '{0}' and b.OrderID = t.OrderID and b.CTNStartNo = t.CTNStartNo", this.txtTransferSlipNo.Text.Trim());

            Dictionary<string, string> dicSqlFilte = new Dictionary<string, string>
            {
                { "PackingList ID", !MyUtility.Check.Empty(this.txtPackID.Text) ? string.Format("and a.ID = '{0}'", this.txtPackID.Text.ToString().Trim()) : string.Empty },
                { "Order ID", !MyUtility.Check.Empty(this.txtSPNo.Text) ? string.Format("and c.ID = '{0}'", this.txtSPNo.Text.ToString().Trim()) : string.Empty },
                { "Orders CustPONo", !MyUtility.Check.Empty(this.txtPONo.Text) ? string.Format("and c.CustPONo = '{0}'", this.txtPONo.Text.ToString().Trim()) : string.Empty },
                { "Orders FtyGroup", !MyUtility.Check.Empty(this.txtfactory.Text) ? string.Format(@"and c.FtyGroup = '{0}'", this.txtfactory.Text.Trim()) : string.Empty },
                { "TransferToClog AddDate Start", !MyUtility.Check.Empty(this.dateTimePicker1.Text) ? string.Format("where t.AddDate >= cast('{0}' as datetime)", this.dateTimePicker1.Text.ToString().Trim()) : string.Empty },
                { "TransferToClog AddDate End", !MyUtility.Check.Empty(this.dateTimePicker2.Text) ? string.Format("and t.AddDate <= cast('{0}' as datetime)", this.dateTimePicker2.Text.ToString().Trim()) : string.Empty },
                { "TransferToClog TransferSlipNo", !MyUtility.Check.Empty(this.txtTransferSlipNo.Text) ? strTransferSlipNoFilte : string.Empty },
            };
            #endregion

            string sqlCmd = string.Format(
                @"
SELECT *
into #tmp_TransferToClog
FROM TransferToClog t WITH (NOLOCK)
-- TransferToClog AddDate Start --
{5}
-- TransferToClog AddDate End --
{6}

select *
       , rn = ROW_NUMBER() over(order by PackingListID,OrderID,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
       , rn1 = ROW_NUMBER() over(order by TRY_CONVERT(int, CTNStartNo) ,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
from (
      Select Distinct '' as ID
             , 0 as selected
             , b.TransferDate
             , a.Id as PackingListID
             , b.OrderID
             , b.CTNStartNo
             , c.CustPONo
             , c.StyleID
             , c.SeasonID
             , c.BrandID
             , c.Customize1
             , d.Alias
             , c.BuyerDelivery
             ,'' as ClogLocationId
             ,c.FtyGroup
             ,'' as Remark
             , b.SCICtnNo
	  from PackingList a WITH (NOLOCK) 
	  inner join #tmp_TransferToClog t WITH (NOLOCK) on a.id = t.PackingListID
	  inner join PackingList_Detail b WITH (NOLOCK) on a.Id = b.Id
	  left join Orders c WITH (NOLOCK) on b.OrderId = c.Id 
  	  left join Country d WITH (NOLOCK) on c.Dest = d.ID 
      where a.MDivisionID = '{0}'
            -- PackingList ID --
            {1}
            -- Order ID --
            {2}
            -- Orders CustPONo --
            {3}
            -- Orders FtyGroup --
            {4} 
            -- TransferToClog TransferSlipNo --
            {7}
            and b.CTNStartNo != '' 
            and b.CTNQty = 1
            and b.TransferDate is not null
            and b.ReceiveDate is null 
            and b.DisposeFromClog= 0 
            and (a.Type = 'B' or a.Type = 'L')
)X 
order by rn

drop table #tmp_TransferToClog
", Env.User.Keyword,
                dicSqlFilte["PackingList ID"],
                dicSqlFilte["Order ID"],
                dicSqlFilte["Orders CustPONo"],
                dicSqlFilte["Orders FtyGroup"],
                dicSqlFilte["TransferToClog AddDate Start"],
                dicSqlFilte["TransferToClog AddDate End"],
                dicSqlFilte["TransferToClog TransferSlipNo"]);

            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.selectDataTable))
            {
                if (this.selectDataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    this.ControlButton4Text("Close");
                }
                else
                {
                    this.ControlButton4Text("Cancel");
                }

                this.listControlBindingSource1.DataSource = this.selectDataTable;
                this.numTotalCTNQty.Value = this.selectDataTable.Rows.Count;
            }
            else
            {
                MyUtility.Msg.ErrorBox(selectResult.Messages.ToString());
            }
        }

        private void ButtonFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        // Import From Barcode
        private void ButtonImport_Click(object sender, EventArgs e)
        {
            // 設定只能選txt檔
            this.openFileDialog1.Filter = "txt files (*.txt)|*.txt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.numSelectedCTNQty.Value = 0;
                this.numTotalCTNQty.Value = 0;

                // 先將Grid的結構給開出來
                string selectCommand = @"
Select distinct '' as ID, 0 as selected, b.TransferDate, b.Id as PackingListID, b.OrderID, 
TRY_CONVERT(int,b.CTNStartNo) as 'CTNStartNo', 
0 as rn,
0 as rn1,
c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery, b.ClogLocationId, '' as Remark ,b.CustCTN,b.SCICtnNo
from PackingList a WITH (NOLOCK) , 
PackingList_Detail b WITH (NOLOCK) , 
Orders c WITH (NOLOCK) , 
Country d WITH (NOLOCK) 
where 1=0";
                DualResult selectResult;
                if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out DataTable selectDataTable)))
                {
                    MyUtility.Msg.WarningBox("Connection faile.!");
                    return;
                }

                this.listControlBindingSource1.DataSource = selectDataTable;

                // 讀檔案
                using (StreamReader reader = new StreamReader(this.openFileDialog1.FileName, System.Text.Encoding.UTF8))
                {
                    DataRow seekData;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        System.Diagnostics.Debug.WriteLine(line);
                        IList<string> sl = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sl[0] != "2")
                        {
                            MyUtility.Msg.WarningBox("Format is not correct!");
                            return;
                        }
                        else
                        {
                            DataRow dr = selectDataTable.NewRow();
                            if (sl.Count > 2 && sl[2].Length > 13)
                            {
                                dr["ID"] = string.Empty;
                                dr["selected"] = 0;
                                dr["PackingListID"] = sl[2].Substring(0, 13);
                                dr["CTNStartNo"] = MyUtility.Convert.GetInt(sl[2].Substring(13));
                                dr["ClogLocationId"] = sl[1];

                                string sqlCmd = string.Format(
                                    @"
select pd.OrderID,pd.OrderShipmodeSeq,TransferDate,ReceiveDate ,p.MDivisionID,pd.SCICtnNo
from PackingList_Detail pd WITH (NOLOCK) 
inner join PackingList p (NOLOCK) on pd.id = p.id
where pd.ID = '{0}' and pd.CTNStartNo = '{1}' and pd.CTNQty > 0 and pd.DisposeFromClog= 0",
                                    dr["PackingListID"].ToString(),
                                    dr["CTNStartNo"].ToString());
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
                                    dr["SCICtnNo"] = seekData["SCICtnNo"];
                                    dr["TransferDate"] = seekData["TransferDate"];

                                    string seq = seekData["OrderShipmodeSeq"].ToString().Trim();
                                    if (seekData["MDivisionID"].ToString().ToUpper() != Env.User.Keyword)
                                    {
                                        dr["Remark"] = "The order's M is not equal to login M.";
                                    }

                                    sqlCmd = string.Format(
                                        @"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,oq.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                            left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{2}'
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'",
                                        dr["OrderID"].ToString(),
                                        Env.User.Keyword,
                                        seq);
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
                                    dr["CustCTN"] = sl[2];
                                    dr["ID"] = string.Empty;
                                    dr["selected"] = 0;
                                    dr["ClogLocationId"] = sl[1];
                                    sqlCmd = $@"
select pd.OrderID,pd.OrderShipmodeSeq,TransferDate,ReceiveDate ,p.MDivisionID,pd.id,pd.CTNStartNo,pd.SCICtnNo
from PackingList_Detail pd WITH (NOLOCK) 
inner join PackingList p (NOLOCK) on pd.id = p.id
where pd.CustCTN = '{dr["CustCTN"]}' and pd.CTNQty > 0 and pd.DisposeFromClog= 0";

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
                                        dr["SCICtnNo"] = seekData["SCICtnNo"];
                                        dr["TransferDate"] = seekData["TransferDate"];

                                        string seq = seekData["OrderShipmodeSeq"].ToString().Trim();
                                        if (seekData["MDivisionID"].ToString().ToUpper() != Env.User.Keyword)
                                        {
                                            dr["Remark"] = "The order's M is not equal to login M.";
                                        }

                                        string packinglistid = seekData["id"].ToString().Trim();
                                        string cTNStartNo = seekData["CTNStartNo"].ToString().Trim();
                                        sqlCmd = string.Format(
                                            @"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,oq.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                            left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{2}'
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'",
                                            dr["OrderID"].ToString(),
                                            Env.User.Keyword,
                                            seq);
                                        if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                        {
                                            dr["packinglistid"] = packinglistid.Trim();
                                            dr["CTNStartNo"] = cTNStartNo.Trim();
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
                                }

                                if (dr["Remark"].ToString().Trim() != string.Empty)
                                {
                                    dr["selected"] = 0;
                                }

                                selectDataTable.Rows.Add(dr);
                            }
                            else
                            {
                                dr["CustCTN"] = sl[2];
                                dr["ID"] = string.Empty;
                                dr["selected"] = 0;
                                dr["ClogLocationId"] = sl[1];
                                string sqlCmd = $@"
select pd.OrderID,pd.OrderShipmodeSeq,TransferDate,ReceiveDate ,p.MDivisionID,pd.id,pd.CTNStartNo,pd.SCICtnNo
from PackingList_Detail pd WITH (NOLOCK) 
inner join PackingList p (NOLOCK) on pd.id = p.id
where pd.CustCTN = '{dr["CustCTN"]}' and pd.CTNQty > 0 and pd.DisposeFromClog= 0";

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
                                    dr["SCICtnNo"] = seekData["SCICtnNo"];
                                    dr["TransferDate"] = seekData["TransferDate"];

                                    string packinglistid = seekData["id"].ToString().Trim();
                                    string cTNStartNo1 = seekData["CTNStartNo"].ToString().Trim();
                                    string seq = seekData["OrderShipmodeSeq"].ToString().Trim();
                                    if (seekData["MDivisionID"].ToString().ToUpper() != Env.User.Keyword)
                                    {
                                        dr["Remark"] = "The order's M is not equal to login M.";
                                    }

                                    sqlCmd = string.Format(
                                        @"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,oq.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                            left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{2}'
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'",
                                        dr["OrderID"].ToString(),
                                        Env.User.Keyword,
                                        seq);
                                    if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                    {
                                        dr["packinglistid"] = packinglistid.Trim();
                                        dr["CTNStartNo"] = cTNStartNo1.Trim();
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

                                if (dr["Remark"].ToString().Trim() != string.Empty)
                                {
                                    dr["selected"] = 0;
                                }

                                selectDataTable.Rows.Add(dr);
                            }
                        }
                    }

                    this.ControlButton4Text("Cancel");
                }
            }

            this.Countselectcount();
        }

        // Save
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            // 檢查是否有勾選資料
            this.gridImport.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
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
                if (dr["Remark"].ToString().Trim() != string.Empty)
                {
                    MyUtility.Msg.WarningBox("Some data cannot be received, please check again.");
                    return;
                }
            }

            IList<string> insertCmds = new List<string>();
            IList<string> updateCmds = new List<string>();

            // 組要Insert進TransferToClog的資料
            foreach (DataRow dr in selectedData)
            {
                insertCmds.Add(string.Format(
                    @"
insert into ClogReceive (
    ReceiveDate
    , MDivisionID
    , PackingListID
    , OrderID
    , CTNStartNo
    , ClogLocationId
    , AddDate
    , AddName
    , SCICtnNo
) values (
    GETDATE()
    , '{0}'
    , '{1}'
    , '{2}'
    , '{3}'
    , '{4}'
    , GETDATE()
    , '{5}'
    , '{6}'
);",
                    Env.User.Keyword,
                    MyUtility.Convert.GetString(dr["PackingListID"]),
                    MyUtility.Convert.GetString(dr["OrderID"]),
                    MyUtility.Convert.GetString(dr["CTNStartNo"]),
                    MyUtility.Convert.GetString(dr["ClogLocationId"]),
                    Env.User.UserID,
                    MyUtility.Convert.GetString(dr["SCICtnNo"])));

                // 要順便更新PackingList_Detail
                updateCmds.Add(string.Format(
                    @"
update PackingList_Detail 
set ReceiveDate = GETDATE()
    , ClogLocationId = '{3}'
    , ReturnDate = null 
where   ID = '{0}' 
        and OrderID = '{1}' 
        and CTNStartNo = '{2}'
        and DisposeFromClog= 0 ; ",
                    MyUtility.Convert.GetString(dr["PackingListID"]),
                    MyUtility.Convert.GetString(dr["OrderID"]),
                    MyUtility.Convert.GetString(dr["CTNStartNo"]),
                    MyUtility.Convert.GetString(dr["ClogLocationId"])));
            }

            // Update Orders的資料
            DataTable selectData = null;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(
                    dt,
                    "Selected,OrderID",
                    @"select distinct OrderID from #tmp a where a.Selected = 1",
                    out selectData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Prepare update orders data fail!\r\n" + ex.ToString());
            }

            DualResult result1 = Ict.Result.True, result2 = Ict.Result.True;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (updateCmds.Count > 0)
                    {
                        result1 = DBProxy.Current.Executes(null, updateCmds);
                    }

                    if (insertCmds.Count > 0)
                    {
                        result2 = DBProxy.Current.Executes(null, insertCmds);
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

            this.Countselectcount();
        }

        // Cancel
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Update All Location
        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            string location = this.txtcloglocationLocationNo.Text.Trim();
            int pos = this.listControlBindingSource1.Position;     // 記錄目前指標位置
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
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
            this.gridImport.SuspendLayout();
            this.gridImport.DataSource = null;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource1.Position = pos;
            this.gridImport.ResumeLayout();
        }

        private void ControlButton4Text(string showText)
        {
            this.btnClose.Text = showText;
        }

        private void GridImport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.Countselectcount();
        }

        private void Countselectcount()
        {
            this.gridImport.ValidateControl();
            DataGridViewColumn column = this.gridImport.Columns["Selected"];
            if (!MyUtility.Check.Empty(column) && !MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                int sint = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").Length;
                this.numSelectedCTNQty.Value = sint;
                this.numTotalCTNQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count;
            }
        }
    }
}
