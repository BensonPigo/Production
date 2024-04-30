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
using Sci.Production.Prg;
using Sci.Win.Tools;
using System.Runtime.Remoting.Messaging;
using System.ComponentModel;
using System.Threading;
using Newtonsoft.Json.Bson;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P03
    /// </summary>
    public partial class P03 : Win.Tems.QueryForm
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
        private bool check_OnlyReqCarton;
        private string selectDataTable_DefaultView_Sort = string.Empty;
        private int threadCnt = 0;

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
            DataGridViewGeneratorCheckBoxColumnSettings col_chk = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Reason = new DataGridViewGeneratorTextColumnSettings();
            col_chk.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridReceiveDate.GetDataRow<DataRow>(e.RowIndex);
                dr["selected"] = e.FormattedValue;
                dr.EndEdit();

                int selectCnt = 0;
                if (this.chkOnlyReqCarton.Checked)
                {
                    selectCnt = this.gridReceiveDate.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["selected"].Value.ToString().Equals("1") && !MyUtility.Check.Empty(row.Cells["FtyReqReturnDate"].Value)).Count();
                }
                else
                {
                    selectCnt = this.gridReceiveDate.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["selected"].Value.ToString().Equals("1")).Count();
                }

                this.numSelectedCTNQty.Value = selectCnt;
            };

            col_Reason.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1 || e.Button != MouseButtons.Right)
                {
                    return;
                }

                DataRow dr = this.gridReceiveDate.GetDataRow(e.RowIndex);

                string sqlcmd = $@"select Description from ClogReason where Type = 'CL' and Junk = 0";

                SelectItem sele = new SelectItem(sqlcmd, "20", null) { Width = 333 };
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["ClogReasonID"] = MyUtility.GetValue.Lookup($@"select ID from ClogReason where [Description] = '{sele.GetSelectedString()}' and Type = 'CL' and Junk = 0 ", "Production");
                e.EditingControl.Text = sele.GetSelectedString();
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
            .Text("Remark", header: "Save Result", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Reason", header: "Reason", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: col_Reason)
            .Text("ClogReasonRemark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: false);

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
                    if (this.selectDataTable_DefaultView_Sort == "DESC")
                    {
                        this.selectDataTable.DefaultView.Sort = "rn1 DESC";
                        this.selectDataTable_DefaultView_Sort = string.Empty;
                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.Sort = " rn1 DESC";
                    }
                    else
                    {
                        this.selectDataTable.DefaultView.Sort = "rn1 ASC";
                        this.selectDataTable_DefaultView_Sort = "DESC";
                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.Sort = " rn1 ASC";
                    }

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

            this.labProcessingBar.Text = "0/0";
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
        , ClogReasonID
        , Reason
        , ClogReasonRemark
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
            , ClogReasonID = '' 
            , ClogReasonRemark = '' 
            , Reason = ''
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
            and a.PLCtnTrToRgCodeDate is null
", Env.User.Keyword));
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
            this.backgroundDownloadSticker.ReportProgress(0);
            this.labProcessingBar.Text = "0/0";
            this.progressCnt = 0;
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
    , Reason = ''
    , ClogReasonID = '' 
    , ClogReasonRemark = '' 
from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) , Country d WITH (NOLOCK) where 1=0";

                DualResult selectResult;
                if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out this.selectDataTable)))
                {
                    MyUtility.Msg.WarningBox("Connection faile.!");
                    return;
                }

                this.listControlBindingSource1.DataSource = this.selectDataTable;

                // 讀檔案
                using (StreamReader reader = new StreamReader(this.openFileDialog1.FileName, Encoding.UTF8))
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
                                string packingListID = sl[1].Substring(0, 13);
                                string ctnStartNo = sl[1].Substring(13).TrimStart('^');
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
,pd.ID
,pd.CTNStartNo
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
where ((pd.ID = '{0}' and CTNStartNo = '{1}') or pd.SCICtnNo = '{2}') 
and pd.CTNQty > 0 
and pd.DisposeFromClog= 0",
                                    packingListID,
                                    ctnStartNo,
                                    sl[1].GetPackScanContent());
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

                                    if (!MyUtility.Check.Empty(seekData["CFAReturnClogDate"]))
                                    {
                                        dr["Remark"] = dr["Remark"] + "This carton has CFA Return Clog Date!";
                                    }

                                    if (seekData["MDivisionID"].ToString().ToUpper() != Env.User.Keyword)
                                    {
                                        dr["Remark"] = dr["Remark"] + "The order's M is not equal to login M.";
                                    }

                                    dr["PackingListID"] = seekData["ID"];
                                    dr["CTNStartNo"] = seekData["CTNStartNo"];
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

                                        if (!MyUtility.Check.Empty(seekData["CFAReturnClogDate"]))
                                        {
                                            dr["Remark"] = dr["Remark"] + "This carton has CFA Return Clog Date!";
                                        }

                                        if (seekData["MDivisionID"].ToString().ToUpper() != Env.User.Keyword)
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

                                    if (!MyUtility.Check.Empty(seekData["CFAReturnClogDate"]))
                                    {
                                        dr["Remark"] = dr["Remark"] + "This carton has CFA Return Clog Date!";
                                    }

                                    if (seekData["MDivisionID"].ToString().ToUpper() != Env.User.Keyword)
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

        private System.ComponentModel.BackgroundWorker[] workers;

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 檢查是否有勾選資料
            this.gridReceiveDate.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            this.dtError = dt.Clone();
            this.check_OnlyReqCarton = this.chkOnlyReqCarton.Checked;
            this.completeCnt = 0;
            this.progressCnt = 0;

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No data need to import!");
                return;
            }

            if (dt.AsEnumerable().Any(row => row["Selected"].EqualDecimal(1)) == false)
            {
                MyUtility.Msg.InfoBox("Please select data first!");
                return;
            }

            if (this.chkOnlyReqCarton.Checked)
            {
                this.selectDataTable = dt.AsEnumerable().Where(r => MyUtility.Convert.GetInt(r["selected"]) == 1 && MyUtility.Check.Empty(r["FtyReqReturnDate"]) == false).ToList().CopyToDataTable();
            }
            else
            {
                this.selectDataTable = dt.AsEnumerable().Where(r => MyUtility.Convert.GetInt(r["selected"]) == 1).ToList().CopyToDataTable();
            }

            string returnMessage = string.Empty;
            if (this.selectDataTable.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data need to import!");
                return;
            }

            if (!this.backgroundDownloadSticker.IsBusy)
            {
                if (this.selectDataTable == null || this.selectDataTable.Rows.Count == 0)
                {
                    return;
                }

                int rowCnt = this.selectDataTable.Rows.Count;
                this.threadCnt = (rowCnt / 100) + (rowCnt % 100 == 0 ? 0 : 1);

                // 初始化 workers 陣列
                this.workers = new System.ComponentModel.BackgroundWorker[this.threadCnt];

                // 初始化 ProgressBar
                this.progressBarProcessing.Minimum = 0;
                this.progressBarProcessing.Maximum = 100;
                this.progressBarProcessing.Step = 1;

                // 先把UI介面鎖住
                this.SetInterfaceLocked(true);
                this.backgroundDownloadSticker.ReportProgress(0);

                // 初始化 BackgroundWorker
                for (int i = 0; i < this.threadCnt; i++)
                {
                    this.workers[i] = new System.ComponentModel.BackgroundWorker();
                    this.workers[i].WorkerReportsProgress = true;
                    this.workers[i].DoWork += this.BackgroundDownloadSticker_DoWork;
                    this.workers[i].ProgressChanged += this.BackgroundDownloadSticker_ProgressChanged;
                    this.workers[i].RunWorkerCompleted += this.BackgroundDownloadSticker_RunWorkerCompleted;
                }

                int processedRows = 0;
                int batchSize = 100;

                for (int i = 0; i < this.threadCnt; i++)
                {
                    int remainingRows = rowCnt - processedRows;
                    int rowsToProcess = Math.Min(batchSize, remainingRows);
                    this.workers[i].RunWorkerAsync(new object[] { this.selectDataTable, processedRows, rowsToProcess });

                    // 更新處理行數
                    processedRows += rowsToProcess;
                }
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
            this.listControlBindingSource1.EndEdit();
            DataGridViewColumn column = this.gridReceiveDate.Columns["Selected"];
            if (!MyUtility.Check.Empty(column) && !MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                int selectCnt = this.gridReceiveDate.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["selected"].Value.ToString().Equals("1")).Count();
                this.numSelectedCTNQty.Value = selectCnt;
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

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtReason.Text) && MyUtility.Check.Empty(this.txtRemark.Text))
            {
                return;
            }

            if ((DataTable)this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            this.gridReceiveDate.ValidateControl();
            DataTable dtGridIDD = (DataTable)this.listControlBindingSource1.DataSource;
            var id = MyUtility.GetValue.Lookup($@"select ID from ClogReason where [Description] = '{this.txtReason.Text}' and Type = 'CL' and Junk = 0 ", "Production");
            foreach (DataRow drIDD in dtGridIDD.Select("selected = 1"))
            {
                drIDD["ClogReasonID"] = id;
                drIDD["Reason"] = this.txtReason.Text;
                drIDD["ClogReasonRemark"] = this.txtRemark.Text;
            }
        }

        private void TxtReason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = $@"select Description from ClogReason  where Type = 'CL' and Junk = 0";
            SelectItem sele = new SelectItem(sqlcmd, "20", null, headercaptions: "Description") { Width = 333 };
            DialogResult result = sele.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtReason.Text = sele.GetSelectedString();
        }

        private DataTable dtError = new DataTable();
        private int progressCnt = 0;

        private void BackgroundDownloadSticker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                int startIndex = (int)((object[])e.Argument)[1];
                int count = (int)((object[])e.Argument)[2];

                // 抓取分割跑多執行緒的table區間
                DataTable dt = ((DataTable)((object[])e.Argument)[0]).AsEnumerable().Skip(startIndex).Take(count).CopyToDataTable();

                foreach (DataRow dr in dt.Rows)
                {
                    StringBuilder singleWarningmsg = new StringBuilder();
                    string checkPackSql = $@"
select pd.CFAReturnClogDate, pd.ReceiveDate, pd.TransferCFADate, pd.ReturnDate ,p.MDivisionID
from PackingList_Detail pd WITH (NOLOCK)
inner join PackingList p  WITH (NOLOCK) on p.id=pd.id
where pd.ID = '{dr["PackingListID"].ToString().Trim()}'
and pd.OrderID = '{dr["OrderID"].ToString().Trim()}'
and pd.CTNStartNo = '{dr["CTNStartNo"].ToString().Trim()}'
and pd.DisposeFromClog = 0
";
                    if (!MyUtility.Check.Seek(checkPackSql, null, out DataRow drPackResult))
                    {
                        singleWarningmsg.Append($@"<CNT#: {dr["PackingListID"]}{dr["CTNStartNo"]}> does not exist!" + Environment.NewLine);
                        continue;
                    }
                    else
                    {
                        if (!MyUtility.Check.Empty(drPackResult["ReturnDate"]))
                        {
                            singleWarningmsg.Append($@"<CNT#: {dr["PackingListID"]}{dr["CTNStartNo"]}>This CTN# has been return." + Environment.NewLine);
                        }
                        else if (MyUtility.Convert.GetString(dr["Reason"]).ToUpper() == "OTHER" && MyUtility.Check.Empty(dr["ClogReasonRemark"]))
                        {
                            singleWarningmsg.Append($@"Please fill in [Remark] since [Reason] is equal to ""Other"" for PackId：{dr["PackingListID"]}, SP#：{dr["OrderID"]}, CTN#：{dr["CTNStartNo"]}." + Environment.NewLine);
                        }
                        else if (!(MyUtility.Check.Empty(drPackResult["TransferCFADate"]) && !MyUtility.Check.Empty(drPackResult["ReceiveDate"]) && MyUtility.Check.Empty(drPackResult["CFAReturnClogDate"])))
                        {
                            singleWarningmsg.Append($@"<CTN#:{dr["PackingListID"]}{dr["CTNStartNo"]}> does not exist Clog!" + Environment.NewLine);
                        }
                        else if (!MyUtility.Check.Empty(drPackResult["CFAReturnClogDate"]))
                        {
                            singleWarningmsg.Append($@"<CTN#:{dr["PackingListID"]}> has CFA Return Clog Date!" + Environment.NewLine);
                        }
                        else if (MyUtility.Check.Seek($@"
            select ID 
            from PAckingList with (nolock) 
            where ID = '{dr["PackingListID"]}'
            and PLCtnTrToRgCodeDate is not null"))
                        {
                            singleWarningmsg.Append($@"<PL#:{dr["PackingListID"]} already transfer to shipping factory, cannot return to production." + Environment.NewLine);
                        }
                        else if (drPackResult["MDivisionID"].ToString().ToUpper() != Env.User.Keyword.ToUpper())
                        {
                            singleWarningmsg.Append($@"<CNT#: {dr["PackingListID"]}{dr["CTNStartNo"]}>The order's M is not equal to login M." + Environment.NewLine);
                        }

                        // 代表都沒錯,可以單筆進行更新新增
                        else
                        {
                            DateTime? scanEditDate = null;
                            if (!MyUtility.Check.Empty(dr["ScanEditDate"].ToString()) &&
                                DateTime.TryParse(dr["ScanEditDate"].ToString(), out DateTime tempDate))
                            {
                                scanEditDate = tempDate;
                            }

                            object dateTimeValue = scanEditDate.HasValue ? "'" + (object)scanEditDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "Null";
                            IList<string> cmds = new List<string>();
                            cmds.Add(
                           string.Format(
                           @"
            insert into ClogReturn(ReturnDate,MDivisionID,PackingListID,OrderID,CTNStartNo, AddDate,AddName,SCICtnNo,ClogReasonID,ClogReasonRemark)
            values (GETDATE(), '{0}', '{1}', '{2}', '{3}', GETDATE(), '{4}', '{5}', isnull('{6}',''), isnull('{7}',''));

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
                , ActCTNWeight = 0
                , DRYReceiveDate  = null
                , DRYTransferDate = null
            from PackingList_Detail pd
            where pd.ID = '{1}'
            and pd.CTNStartNo = '{3}'
            and pd.DisposeFromClog= 0 ;

            INSERT INTO [PackingScan_History]([MDivisionID],[PackingListID],[OrderID],[CTNStartNo],[SCICtnNo]
            	,[DeleteFrom],[ScanQty],[ScanEditDate],[ScanName],[AddName],[AddDate],[LackingQty])
            select '{0}' [MDivisionID]
                ,'{1}' [PackingListID]
                ,'{2}' [OrderID]
                ,'{3}' [CTNStartNo]
                ,'{5}' [SCICtnNo]
                ,'Clog P03' [DeleteFrom]
                ,{8} [ScanQty]
                , {9} [ScanEditDate]
                ,'{10}' [ScanName]
                ,'{4}' [AddName]
                ,GETDATE() [AddDate]
                ,[LackingQty] = ( ISNULL( (
                                            SELECT SUM(pd.ShipQty)
                                            FROM PackingList_Detail pd
                                            WHERE  pd.ID = '{1}' AND pd.CTNStartNo = '{3}') 
                                        ,0) 
                                );
            ",
                           Env.User.Keyword,
                           MyUtility.Convert.GetString(dr["PackingListID"]),
                           MyUtility.Convert.GetString(dr["OrderID"]),
                           MyUtility.Convert.GetString(dr["CTNStartNo"]),
                           Env.User.UserID,
                           MyUtility.Convert.GetString(dr["SCICtnNo"]),
                           MyUtility.Convert.GetString(dr["ClogReasonID"]),
                           MyUtility.Convert.GetString(dr["ClogReasonRemark"]),
                           MyUtility.Convert.GetInt(dr["ScanQty"]),
                           dateTimeValue,
                           MyUtility.Convert.GetString(dr["ScanName"])));

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
                                singleWarningmsg.Append($@"Prepare update orders data fail!\r\n" + ex.ToString() + Environment.NewLine);
                            }

                            DualResult result1 = Ict.Result.True;

                            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 5, 0)))
                            {
                                try
                                {
                                    result1 = DBProxy.Current.Executes(null, cmds);

                                    if (result1 == false)
                                    {
                                        transactionScope.Dispose();
                                        singleWarningmsg.Append(result1.ToString() + Environment.NewLine);
                                    }

                                    DualResult prgResult = Prgs.UpdateOrdersCTN(selectOrdersData);

                                    if (prgResult == false)
                                    {
                                        transactionScope.Dispose();
                                        singleWarningmsg.Append(prgResult.ToString() + Environment.NewLine);
                                    }

                                    transactionScope.Complete();
                                    transactionScope.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    transactionScope.Dispose();
                                    singleWarningmsg.Append("Commit transaction error." + ex + Environment.NewLine);
                                }
                            }
                        }
                    }

                    // 更新進度條
                    this.progressCnt++;

                    double barPercentage = Math.Abs(MyUtility.Convert.GetDouble(this.progressCnt) / this.selectDataTable.Rows.Count) * 100;
                    if (this.progressCnt == this.selectDataTable.Rows.Count)
                    {
                        ((System.ComponentModel.BackgroundWorker)sender).ReportProgress(MyUtility.Convert.GetInt(100));
                    }
                    else
                    {
                        ((System.ComponentModel.BackgroundWorker)sender).ReportProgress(MyUtility.Convert.GetInt(barPercentage));
                    }

                    if (singleWarningmsg.ToString().Length > 0)
                    {
                        DataRow drError = this.dtError.NewRow();
                        dr["Remark"] = singleWarningmsg;
                        dr.CopyTo(drError);
                        this.dtError.Rows.Add(drError);
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.ToString();
            }
        }

        private void BackgroundDownloadSticker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (this.selectDataTable != null && e.ProgressPercentage <= 100)
            {
                this.progressBarProcessing.Value = e.ProgressPercentage;
                this.labProcessingBar.Text = $"{this.progressCnt}/{this.selectDataTable.Rows.Count}";
            }
        }

        private void SetInterfaceLocked(bool isLocked)
        {
            // 鎖住或解鎖 UI 介面
            this.chkOnlyReqCarton.Enabled = !isLocked;
            this.btnFind.Enabled = !isLocked;
            this.btnImportFromBarcode.Enabled = !isLocked;
            this.btnSave.Enabled = !isLocked;
            this.btnClose.Enabled = !isLocked;
            this.pictureBox.Enabled = !isLocked;
            this.txtReason.Enabled = !isLocked;
            this.txtRemark.Enabled = !isLocked;

            // 或者顯示一個等待光標等
            Cursor.Current = isLocked ? Cursors.WaitCursor : Cursors.Default;
        }

        private int completeCnt = 0;

        private void BackgroundDownloadSticker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.completeCnt++;
            if (this.completeCnt == this.threadCnt)
            {
                // 檢查是否有勾選資料
                this.gridReceiveDate.ValidateControl();
                this.listControlBindingSource1.EndEdit();

                // 使用Find撈出的全部資料
                DataTable dt =
                        (DataTable)this.listControlBindingSource1.DataSource;

                if (this.dtError.Rows.Count > 0)
                {
                    MyUtility.Msg.WarningBox("Some carton cannot receive, please refer to field <Save Result>.");

                    if (this.gridReceiveDate.Rows.Cast<DataGridViewRow>().Any(row => !row.Cells["Selected"].Value.ToString().Equals("1")))
                    {
                        /*
                         沒勾選的放table #1
                         有錯誤的放table #2
                         再將2者合併一起, 畫面只會顯示沒勾的+有錯誤的
                         最後再將Selected清空
                         */

                        DataTable dtCopy = dt.AsEnumerable().Where(r => MyUtility.Convert.GetInt(r["selected"]) == 0).ToList().CopyToDataTable();
                        dtCopy.Merge(this.dtError, true, MissingSchemaAction.AddWithKey);
                        foreach (DataRow dr in dtCopy.Rows)
                        {
                            if (MyUtility.Check.Empty(dr["Selected"]))
                            {
                                dr["Remark"] = string.Empty;
                            }
                            else
                            {
                                dr["Selected"] = false;
                            }
                        }

                        this.listControlBindingSource1.DataSource = dtCopy;
                    }
                    else
                    {
                        foreach (DataRow dr in this.dtError.Rows)
                        {
                            dr["Selected"] = false;
                        }

                        this.listControlBindingSource1.DataSource = this.dtError;
                    }

                    ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.Sort = " rn ASC";
                }
                else
                {
                    if (dt.AsEnumerable().Where(r => MyUtility.Convert.GetInt(r["selected"]) == 0).ToList().Count() == 0)
                    {
                        this.listControlBindingSource1.DataSource = null;
                    }
                    else
                    {
                        DataTable newdt = dt.AsEnumerable().Where(r => MyUtility.Convert.GetInt(r["selected"]) == 0).ToList().CopyToDataTable();
                        this.listControlBindingSource1.DataSource = newdt;
                    }

                    this.ControlButton4Text("Close");
                    MyUtility.Msg.InfoBox("Complete!!");
                }

                this.backgroundDownloadSticker.ReportProgress(0);
                this.Countselectcount();

                // 先把UI介面鎖住
                this.SetInterfaceLocked(false);
            }
        }
    }
}
