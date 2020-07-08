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
    /// <summary>
    /// Logistic_P03
    /// </summary>
    public partial class P03 : Sci.Win.Tems.QueryForm
    {
        /// <summary>
        /// P03
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
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

        /// <summary>
        /// SelectDataTable_DefaultView_Sort
        /// </summary>
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

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.DataGridViewGeneratorCheckBoxColumnSettings col_chk = new Ict.Win.DataGridViewGeneratorCheckBoxColumnSettings();
            col_chk.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridReceiveDate.GetDataRow<DataRow>(e.RowIndex);
                dr["selected"] = e.FormattedValue;
                dr.EndEdit();
                int sint = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1" + (this.chkOnlyReqCarton.Checked ? "AND FtyReqReturnDate IS NOT NULL" : string.Empty)).Length;
                this.numSelectedCTNQty.Value = sint;
            };

            this.gridReceiveDate.IsEditingReadOnly = false;
            this.gridReceiveDate.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridReceiveDate)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: col_chk)
            .Date("ReceiveDate", header: "Receive Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("FtyReqReturnDate", header: "Request Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
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
            if (MyUtility.Check.Empty(this.txtSPNo.Text) && MyUtility.Check.Empty(this.txtPONo.Text) && MyUtility.Check.Empty(this.txtPackID.Text) && MyUtility.Check.Empty(this.dateTimePicker1.Text) && MyUtility.Check.Empty(this.dateTimePicker2.Text) && !this.dateReqDate.HasValue1 && !this.dateReqDate.HasValue2)
            {
                MyUtility.Msg.WarningBox("< SP# > or < PO# > or < PackID > or <Receive Date> or <Request Date> can not be empty!");
                return;
            }

            this.numSelectedCTNQty.Value = 0;
            this.numTotalCTNQty.Value = 0;
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(
                @"
select ID
        , selected
        , ReceiveDate
        , PackingListID
        , FtyReqReturnDate
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
        ,TransferCFADate
        ,CFAReturnClogDate
        ,SCICtnNo
		, ScanQty
		, ScanEditDate
		, ScanName
        , rn = ROW_NUMBER() over (order by PackingListID, OrderID, (RIGHT (REPLICATE ('0', 6) + rtrim (ltrim (CTNStartNo)), 6)))
        , rn1 = ROW_NUMBER() over (order by TRY_CONVERT (int, CTNStartNo), (RIGHT (REPLICATE ('0', 6) + rtrim (ltrim (CTNStartNo)), 6)))	
from (
    Select  distinct '' as ID
            , 1 as selected
            , b.ReceiveDate
            , b.FtyReqReturnDate
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
            , b.TransferCFADate 
            , b.CFAReturnClogDate 
            , b.SCICtnNo
			, [ScanQty]=ScanQty.Value
			, b.ScanEditDate
			, b.ScanName
    from PackingList a WITH (NOLOCK) 
    INNER JOIN  PackingList_Detail b WITH (NOLOCK)  ON a.Id = b.Id 
    LEFT JOIN  Orders c WITH (NOLOCK) ON b.OrderId = c.Id 
    LEFT JOIN  Country d WITH (NOLOCK) ON  c.Dest = d.ID 
	OUTER APPLY (
		SELECT [Value]=SUM(pd.ScanQty) 
		FROM PackingList_Detail pd
		WHERE pd.ID= a.Id 
				AND OrderID=b.OrderID 
				AND CTNStartNo=b.CTNStartNo  
				AND CTNStartNo=b.CTNStartNo 
				AND SCICtnNo=b.SCICtnNo
	)ScanQty
	where   b.CTNStartNo != '' 
	        and b.ReceiveDate is not null
            and b.TransferCFADate is null
            and b.CFAReturnClogDate is null
            and b.DisposeFromClog= 0
            and a.MDivisionID = '{0}' 
            and (a.Type = 'B' or a.Type = 'L')
", Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and b.OrderID = '{0}'", this.txtSPNo.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtPONo.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and c.CustPONo = '{0}'", this.txtPONo.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and a.ID = '{0}'", this.txtPackID.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.dateTimePicker1.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and b.ReceiveDate >= '{0}'", this.dateTimePicker1.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.dateTimePicker2.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and b.ReceiveDate <= '{0}'", this.dateTimePicker2.Text.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlCmd.Append(string.Format(@" and c.FtyGroup = '{0}'", this.txtfactory.Text.Trim()));
            }

            if (this.dateReqDate.HasValue1)
            {
                sqlCmd.Append(string.Format(@" and b.FtyReqReturnDate >= '{0}'", this.dateReqDate.Value1.Value.ToShortDateString()));
            }

            if (this.dateReqDate.HasValue2)
            {
                sqlCmd.Append(string.Format(@" and b.FtyReqReturnDate <= '{0}'", this.dateReqDate.Value2.Value.ToShortDateString()));
            }

            sqlCmd.Append(@"
) a
order by rn ");

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
            }

            this.listControlBindingSource1.DataSource = this.selectDataTable;
            this.numTotalCTNQty.Value = this.selectDataTable.Rows.Count;
            this.numSelectedCTNQty.Value = this.selectDataTable.Rows.Count;
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
            this.Grid_Filter();
        }

        // Import From Barcode
        private void BtnImportFromBarcode_Click(object sender, EventArgs e)
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
Select distinct '' as ID
    , 0 as selected
    , b.ReceiveDate
    , b.Id as PackingListID
    , b.OrderID
    , TRY_CONVERT(int,b.CTNStartNo) as 'CTNStartNo'
    , 0 as rn
    , 0 as rn1
    , c.CustPONo
    , c.StyleID
    , c.SeasonID
    , c.BrandID
    , c.Customize1
    , d.Alias
    , c.BuyerDelivery
    , b.ClogLocationId
    , '' as Remark
    , b.TransferCFADate
    , b.CFAReturnClogDate
    , b.CustCTN
    , [FtyGroup]=a.FactoryID
    , b.FtyReqReturnDate
    , b.SCICtnNo
	, [ScanQty]=b.ScanQty
	, b.ScanEditDate
	, b.ScanName
from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) , Country d WITH (NOLOCK) where 1=0";

                DualResult selectResult;
                if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out this.selectDataTable)))
                {
                    MyUtility.Msg.WarningBox("Connection faile.!");
                    return;
                }

                this.listControlBindingSource1.DataSource = this.selectDataTable;

                // 讀檔案
                using (StreamReader reader = new StreamReader(this.openFileDialog1.FileName, System.Text.Encoding.UTF8))
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
                            DataRow dr = this.selectDataTable.NewRow();
                            if (sl.Count > 1 && sl[1].Length > 13)
                            {
                                dr["ID"] = string.Empty;
                                dr["selected"] = 0;
                                dr["PackingListID"] = sl[1].Substring(0, 13);
                                dr["CTNStartNo"] = MyUtility.Convert.GetInt(sl[1].Substring(13));
                                string sqlCmd = string.Format(
                                    @"
select 
pd.OrderID
,pd.OrderShipmodeSeq
,pd.ReceiveDate
,pd.FtyReqReturnDate
,pd.ReturnDate
,pd.ClogLocationId
,p.MDivisionID
,pd.TransferCFADate
,pd.CFAReturnClogDate 
,p.FactoryID
,pd.SCICtnNo
,[ScanQty]=ScanQty.Value
,pd.ScanEditDate
,pd.ScanName
from PackingList_Detail pd WITH (NOLOCK)
inner join PackingList p (NOLOCK) on pd.id = p.id
OUTER APPLY (
	SELECT [Value]=SUM(ppd.ScanQty) 
	FROM PackingList_Detail ppd
	WHERE ppd.ID= p.Id 
			AND ppd.OrderID=pd.OrderID 
			AND ppd.CTNStartNo=pd.CTNStartNo  
			AND ppd.CTNStartNo=pd.CTNStartNo 
			AND ppd.SCICtnNo=pd.SCICtnNo
)ScanQty
where pd.ID = '{0}' 
and CTNStartNo = '{1}' 
and pd.CTNQty > 0 
and pd.DisposeFromClog= 0",
                                    dr["PackingListID"].ToString(),
                                    dr["CTNStartNo"].ToString());
                                if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                {
                                    if (MyUtility.Check.Empty(seekData["ReturnDate"]))
                                    {
                                        if (MyUtility.Check.Empty(seekData["ReceiveDate"]))
                                        {
                                            dr["Remark"] = dr["Remark"] + "This carton not yet send to Clog.";
                                        }
                                        else
                                        {
                                            dr["selected"] = 1;
                                        }
                                    }
                                    else
                                    {
                                        dr["Remark"] = dr["Remark"] + "This carton has been return.";
                                    }

                                    if (!(MyUtility.Check.Empty(seekData["TransferCFADate"]) && !MyUtility.Check.Empty(seekData["ReceiveDate"]) && MyUtility.Check.Empty(seekData["CFAReturnClogDate"])))
                                    {
                                        dr["Remark"] = dr["Remark"] + "This carton does not exist Clog!";
                                    }

                                    if (seekData["MDivisionID"].ToString().ToUpper() != Sci.Env.User.Keyword)
                                    {
                                        dr["Remark"] = dr["Remark"] + "The order's M is not equal to login M.";
                                    }

                                    dr["OrderID"] = seekData["OrderID"];
                                    dr["SCICtnNo"] = seekData["SCICtnNo"];
                                    dr["ClogLocationId"] = seekData["ClogLocationId"];
                                    dr["ReceiveDate"] = seekData["ReceiveDate"];
                                    dr["TransferCFADate"] = seekData["TransferCFADate"];
                                    dr["CFAReturnClogDate"] = seekData["CFAReturnClogDate"];
                                    dr["FtyReqReturnDate"] = seekData["FtyReqReturnDate"];
                                    dr["FtyGroup"] = seekData["FactoryID"];
                                    dr["ScanQty"] = seekData["ScanQty"];
                                    dr["ScanEditDate"] = seekData["ScanEditDate"];
                                    dr["ScanName"] = seekData["ScanName"];
                                    string seq = MyUtility.Convert.GetString(seekData["OrderShipmodeSeq"]).Trim();
                                    sqlCmd = string.Format(
                                        @"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,oq.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                            left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{2}'
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'",
                                        MyUtility.Convert.GetString(dr["OrderID"]),
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
                                    dr["CustCTN"] = sl[1];
                                    sqlCmd = $@"
select pd.OrderID
,pd.OrderShipmodeSeq
,pd.ReceiveDate
,pd.ReturnDate
,pd.ClogLocationId
,p.MDivisionID
,pd.TransferCFADate
,pd.CFAReturnClogDate
,pd.id
,pd.CTNStartNo
,pd.FtyReqReturnDate
,p.FactoryID
,pd.SCICtnNo
,[ScanQty]=ScanQty.Value
,pd.ScanEditDate
,pd.ScanName
from PackingList_Detail pd WITH (NOLOCK)  
inner join PackingList p (NOLOCK) on pd.id = p.id
OUTER APPLY (
	SELECT [Value]=SUM(ppd.ScanQty) 
	FROM PackingList_Detail ppd
	WHERE ppd.ID= p.Id 
			AND ppd.OrderID=pd.OrderID 
			AND ppd.CTNStartNo=pd.CTNStartNo  
			AND ppd.CTNStartNo=pd.CTNStartNo 
			AND ppd.SCICtnNo=pd.SCICtnNo
)ScanQty
where pd.CustCTN = '{dr["CustCTN"]}' and pd.CTNQty > 0 and pd.DisposeFromClog= 0";
                                    if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                    {
                                        if (MyUtility.Check.Empty(seekData["ReturnDate"]))
                                        {
                                            if (MyUtility.Check.Empty(seekData["ReceiveDate"]))
                                            {
                                                dr["Remark"] = dr["Remark"] + "This carton not yet send to Clog.";
                                            }
                                            else
                                            {
                                                dr["selected"] = 1;
                                            }
                                        }
                                        else
                                        {
                                            dr["Remark"] = dr["Remark"] + "This carton has been return.";
                                        }

                                        if (!(MyUtility.Check.Empty(seekData["TransferCFADate"]) && !MyUtility.Check.Empty(seekData["ReceiveDate"]) && MyUtility.Check.Empty(seekData["CFAReturnClogDate"])))
                                        {
                                            dr["Remark"] = dr["Remark"] + "This carton does not exist Clog!";
                                        }

                                        if (seekData["MDivisionID"].ToString().ToUpper() != Sci.Env.User.Keyword)
                                        {
                                            dr["Remark"] = dr["Remark"] + "The order's M is not equal to login M.";
                                        }

                                        string packinglistid = seekData["id"].ToString().Trim();
                                        string cTNStartNo = seekData["CTNStartNo"].ToString().Trim();
                                        dr["OrderID"] = seekData["OrderID"];
                                        dr["SCICtnNo"] = seekData["SCICtnNo"];
                                        dr["ClogLocationId"] = seekData["ClogLocationId"];
                                        dr["ReceiveDate"] = seekData["ReceiveDate"];
                                        dr["TransferCFADate"] = seekData["TransferCFADate"];
                                        dr["CFAReturnClogDate"] = seekData["CFAReturnClogDate"];
                                        dr["FtyReqReturnDate"] = seekData["FtyReqReturnDate"];
                                        dr["FtyGroup"] = seekData["FactoryID"];
                                        dr["ScanQty"] = seekData["ScanQty"];
                                        dr["ScanEditDate"] = seekData["ScanEditDate"];
                                        dr["ScanName"] = seekData["ScanName"];
                                        string seq = MyUtility.Convert.GetString(seekData["OrderShipmodeSeq"]).Trim();
                                        sqlCmd = string.Format(
                                            @"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,oq.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                            left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{2}'
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'",
                                            MyUtility.Convert.GetString(dr["OrderID"]),
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
                                        dr["Remark"] = dr["Remark"] + "This carton is not in packing list.";
                                    }
                                }

                                if (dr["Remark"].ToString().Trim() != string.Empty)
                                {
                                    dr["selected"] = 0;
                                }

                                this.selectDataTable.Rows.Add(dr);
                            }
                            else
                            {
                                dr["CustCTN"] = sl[1];
                                string sqlCmd = $@"
select pd.OrderID
,pd.OrderShipmodeSeq
,pd.ReceiveDate
,pd.ReturnDate
,pd.ClogLocationId
,p.MDivisionID
,pd.TransferCFADate
,pd.CFAReturnClogDate
,pd.id
,pd.CTNStartNo
,pd.FtyReqReturnDate
,p.FactoryID
,pd.SCICtnNo
,[ScanQty]=ScanQty.Value
,pd.ScanEditDate
,pd.ScanName
from PackingList_Detail pd WITH (NOLOCK)  
inner join PackingList p (NOLOCK) on pd.id = p.id
OUTER APPLY (
	SELECT [Value]=SUM(ppd.ScanQty) 
	FROM PackingList_Detail ppd
	WHERE ppd.ID= p.Id 
			AND ppd.OrderID=pd.OrderID 
			AND ppd.CTNStartNo=pd.CTNStartNo  
			AND ppd.CTNStartNo=pd.CTNStartNo 
			AND ppd.SCICtnNo=pd.SCICtnNo
)ScanQty
where pd.CustCTN = '{dr["CustCTN"]}' and pd.CTNQty > 0 and pd.DisposeFromClog= 0";
                                if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                {
                                    if (MyUtility.Check.Empty(seekData["ReturnDate"]))
                                    {
                                        if (MyUtility.Check.Empty(seekData["ReceiveDate"]))
                                        {
                                            dr["Remark"] = dr["Remark"] + "This carton not yet send to Clog.";
                                        }
                                        else
                                        {
                                            dr["selected"] = 1;
                                        }
                                    }
                                    else
                                    {
                                        dr["Remark"] = dr["Remark"] + "This carton has been return.";
                                    }

                                    if (!(MyUtility.Check.Empty(seekData["TransferCFADate"]) && !MyUtility.Check.Empty(seekData["ReceiveDate"]) && MyUtility.Check.Empty(seekData["CFAReturnClogDate"])))
                                    {
                                        dr["Remark"] = dr["Remark"] + "This carton does not exist Clog!";
                                    }

                                    if (seekData["MDivisionID"].ToString().ToUpper() != Sci.Env.User.Keyword)
                                    {
                                        dr["Remark"] = dr["Remark"] + "The order's M is not equal to login M.";
                                    }

                                    string packinglistid = seekData["id"].ToString().Trim();
                                    string cTNStartNo1 = seekData["CTNStartNo"].ToString().Trim();
                                    dr["OrderID"] = seekData["OrderID"];
                                    dr["SCICtnNo"] = seekData["SCICtnNo"];
                                    dr["ClogLocationId"] = seekData["ClogLocationId"];
                                    dr["ReceiveDate"] = seekData["ReceiveDate"];
                                    dr["TransferCFADate"] = seekData["TransferCFADate"];
                                    dr["CFAReturnClogDate"] = seekData["CFAReturnClogDate"];
                                    dr["FtyReqReturnDate"] = seekData["FtyReqReturnDate"];
                                    dr["FtyGroup"] = seekData["FactoryID"];
                                    dr["ScanQty"] = seekData["ScanQty"];
                                    dr["ScanEditDate"] = seekData["ScanEditDate"];
                                    dr["ScanName"] = seekData["ScanName"];
                                    string seq = MyUtility.Convert.GetString(seekData["OrderShipmodeSeq"]).Trim();
                                    sqlCmd = string.Format(
                                        @"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,oq.BuyerDelivery 
                                                                            from Orders a WITH (NOLOCK) 
                                                                            left join Country b WITH (NOLOCK) on b.ID = a.Dest
                                                                            left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{2}'
                                                                            where a.ID = '{0}' and a.MDivisionID = '{1}'",
                                        MyUtility.Convert.GetString(dr["OrderID"]),
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
                                    dr["Remark"] = dr["Remark"] + "This carton is not in packing list.";
                                }

                                if (dr["Remark"].ToString().Trim() != string.Empty)
                                {
                                    dr["selected"] = 0;
                                }

                                this.selectDataTable.Rows.Add(dr);
                            }
                        }
                    }

                    this.ControlButton4Text("Cancel");
                }
            }

            this.Countselectcount();
            this.Grid_Filter();
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

            DataRow[] selectedData = this.chkOnlyReqCarton.Checked ? dt.Select("Selected = 1 AND FtyReqReturnDate IS NOT NULL") : dt.Select("Selected = 1");
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

                if (!(MyUtility.Check.Empty(dr["TransferCFADate"]) && !MyUtility.Check.Empty(dr["ReceiveDate"]) && MyUtility.Check.Empty(dr["CFAReturnClogDate"])))
                {
                    MyUtility.Msg.WarningBox($@"<CNT#:{dr["PackingListID"]}{dr["CTNStartNo"]}> does not exist Clog!");
                    return;
                }
            }

            string sql = $@"
declare @MDivisionID as varchar(8) = '{Sci.Env.User.Keyword}'
	, @Userid As nvarchar(10) = '{Sci.Env.User.UserID}'

insert into ClogReturn(ReturnDate,MDivisionID,PackingListID,OrderID,CTNStartNo, AddDate,AddName,SCICtnNo)
select GETDATE() ReturnDate
	,@MDivisionID MDivisionID
	,PackingListID PackingListID
	,OrderID OrderID
	,CTNStartNo CTNStartNo
	,GETDATE() AddDate
	,@Userid AddName
	,SCICtnNo SCICtnNo
from #tmp ;


update pd 
set TransferDate = null
, ReceiveDate = null
, ClogLocationId = ''
, ReturnDate = GETDATE()
, ClogReceiveCFADate =null
, ScanQty = 0 
, ScanEditDate = null
, ScanName = ''
, Lacking = 0
, ActCTNWeight = null
from PackingList_Detail pd
inner join #tmp t on pd.ID = t.PackingListID and pd.CTNStartNo = t.CTNStartNo 
where pd.DisposeFromClog= 0 ;


INSERT INTO [PackingScan_History]([MDivisionID],[PackingListID],[OrderID],[CTNStartNo],[SCICtnNo]
	,[DeleteFrom],[ScanQty],[ScanEditDate],[ScanName],[AddName],[AddDate],[LackingQty])
select @MDivisionID [MDivisionID]
    ,t.PackingListID [PackingListID]
    ,t.OrderID [OrderID]
    ,t.CTNStartNo [CTNStartNo]
    ,t.SCICtnNo [SCICtnNo]
    ,'Clog P03' [DeleteFrom]
    ,t.ScanQty [ScanQty]
    ,t.ScanEditDate [ScanEditDate]
    ,t.ScanName [ScanName]
    ,@Userid [AddName]
    ,GETDATE() [AddDate]
    ,[LackingQty] = ( ISNULL( (
                                SELECT SUM(pd.ShipQty)
                                FROM PackingList_Detail pd
                                WHERE  pd.ID=t.PackingListID AND pd.CTNStartNo=t.CTNStartNo) 
                            ,0) 
                       - ScanQty
                    )
from #tmp t ;

 ----LackingQty計算規則詳見：ISP20191801
";

            // Update Orders的資料
            DataTable selectOrdersData = null;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(
                    dt,
                    "Selected,OrderID",
                    @"select distinct OrderID from #tmp a where a.Selected = 1",
                    out selectOrdersData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Prepare update orders data fail!\r\n" + ex.ToString());
            }

            DataTable resulttb;
            DualResult result1 = Result.True, result2 = Result.True;

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    // 主要Insert進TransferToClog的資料
                    result1 = MyUtility.Tool.ProcessWithDatatable(selectedData.CopyToDataTable(), "PackingListID,OrderID,CTNStartNo,SCICtnNo,ScanQty,ScanEditDate,ScanName", sql, out resulttb, "#tmp");

                    if (result1 == false)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox(result1.ToString());
                        return;
                    }

                    DualResult prgResult = Prgs.UpdateOrdersCTN(selectOrdersData);

                    if (prgResult == false)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox(prgResult.ToString());
                        return;
                    }

                    if (result2 == false)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox(result2.ToString());
                        return;
                    }

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    this.ControlButton4Text("Close");
                    MyUtility.Msg.InfoBox("Complete!!");
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
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ControlButton4Text(string showText)
        {
            this.btnClose.Text = showText;
        }

        private void GridReceiveDate_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.Countselectcount();
        }

        private void Countselectcount()
        {
            this.gridReceiveDate.ValidateControl();
            DataGridViewColumn column = this.gridReceiveDate.Columns["Selected"];
            if (!MyUtility.Check.Empty(column) && !MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                int sint = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").Length;
                this.numSelectedCTNQty.Value = sint;
                this.numTotalCTNQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count;
            }
        }

        private void ChkOnlyReqCarton_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void Grid_Filter()
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

            if (!MyUtility.Check.Empty(dt) && dt.Rows.Count > 0)
            {
                string filter = string.Empty;
                switch (this.chkOnlyReqCarton.Checked)
                {
                    case false:
                        if (MyUtility.Check.Empty(this.gridReceiveDate))
                        {
                            break;
                        }

                        filter = string.Empty;
                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        this.numSelectedCTNQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").Count();
                        break;

                    case true:
                        if (MyUtility.Check.Empty(this.gridReceiveDate))
                        {
                            break;
                        }

                        filter = " FtyReqReturnDate IS NOT NULL ";
                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;

                        this.numSelectedCTNQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1 AND FtyReqReturnDate IS NOT NULL ").Count();
                        break;
                }
            }
        }
    }
}
