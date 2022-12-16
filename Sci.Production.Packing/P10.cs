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
using System.Data.SqlClient;
using Sci.Production.Prg;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P10
    /// </summary>
    public partial class P10 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// P10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                 .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                 .Text("PackingListID", header: "PackId", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(4), iseditingreadonly: true)
                 .Text("SIZE", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Qty", header: "Qty/Ctn", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
                 .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        private void Find()
        {
            if (MyUtility.Check.Empty(this.txtSP.Text) && MyUtility.Check.Empty(this.txtPO.Text) && MyUtility.Check.Empty(this.txtPackID.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > or < Order# > or < PackID > can not be empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(
                @"
select	ID
		, selected
		, PackingListID
		, OrderID
		, CTNStartNo
		, CustPONo
		, StyleID
		, SeasonID
		, BrandID
		, Customize1
		, Alias
		, BuyerDelivery 
        , SCICtnNo
        ,[SIZE]
        ,[Qty]
from (
    Select  Distinct ID = ''
            , selected = 1
            , PackingListID = b.Id 
            , b.OrderID
            , b.CTNStartNo
            , c.CustPONo
            , c.StyleID
            , c.SeasonID
            , c.BrandID
            , c.Customize1
            , d.Alias
            , c.BuyerDelivery 
            , orderByCTNStartNo = TRY_CONVERT(int, CTNStartNo)
            , b.SCICtnNo
            ,[SIZE] = size.val
            ,[Qty] =qty.val
    from PackingList a WITH (NOLOCK)
    inner join PackingList_Detail b WITH (NOLOCK) on a.Id = b.Id 
    left join Orders c WITH (NOLOCK) on b.OrderId = c.Id 
    left join Country d WITH (NOLOCK) on c.Dest = d.ID 
    outer apply
	(
		select val = stuff(
		(
			select concat('/',tmp.SizeCode) 
			from
			(
				select distinct SizeCode 
				from PackingList_Detail
				where CTNStartNo in(b.CTNStartNo) and id = a.id
			)tmp for xml path('')
		),1,1,'')
	)size
	outer apply
	(
		select val = stuff(
		(
			select concat('/',tmp.ShipQty) 
			from
			(
				select distinct ShipQty 
				from PackingList_Detail
				where CTNStartNo in(b.CTNStartNo) and id = a.id
			)tmp for xml path('')
		),1,1,'')
	)Qty
    where b.CTNStartNo != ''  
    and b.PackErrTransferDate is null
    and b.DisposeFromClog= 0 
    and ((
			[ReturnDate] is null and 
			[TransferDate] is null and 
			[PackErrTransferDate] is null and
			([DRYReceiveDate] is null or([DRYReceiveDate] is not null and [DRYTransferDate] is not null))
		) or [ReturnDate] is not null)
    and a.MDivisionID = '{0}' 
    and (a.Type = 'B' or a.Type = 'L')
	and b.CTNQty=1
", Env.User.Keyword));
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

            sqlCmd.Append(@"
) detail
ORDER BY Id, OrderID, orderByCTNStartNo, CTNSTartNo;");

            DataTable selectDataTable;
            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, sqlCmd.ToString(), out selectDataTable))
            {
                if (selectDataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!");
                    this.ControlButton4Text("Close");
                }
                else
                {
                    this.ControlButton4Text("Cancel");
                }
            }
            else
            {
                MyUtility.Msg.WarningBox(selectResult.Description);
                this.ControlButton4Text("Close");
            }

            this.listControlBindingSource1.DataSource = selectDataTable;
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        // Import From Barcode
        private void BtnImportFromBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                this.ShowWaitMessage("Data Loading...");

                // 設定只能選txt檔
                this.openFileDialog1.Filter = "txt files (*.txt)|*.txt";

                // 開窗且有選擇檔案
                if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 先將Grid的結構給開出來
                    string selectCommand = @"Select distinct '' as ID, 1 as selected, b.Id as PackingListID, b.OrderID, b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, c.Customize1, d.Alias, c.BuyerDelivery ,b.CustCTN,b.SCICtnNo,[SIZE] = '',[QTY] = ''
                                                             from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) , Country d WITH (NOLOCK) where 1=0";
                    DataTable selectDataTable;
                    DualResult selectResult;
                    if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out selectDataTable)))
                    {
                        MyUtility.Msg.WarningBox("Connection faile.!");
                        return;
                    }

                    this.listControlBindingSource1.DataSource = selectDataTable;

                    // 讀檔案
                    using (StreamReader reader = new StreamReader(this.openFileDialog1.FileName, Encoding.UTF8))
                    {
                        DataRow seekData;
                        DataTable loginMErr = selectDataTable.Clone();
                        DataTable transferErr = selectDataTable.Clone();
                        DataTable notFoundErr = selectDataTable.Clone();
                        int insertCount = 0;
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            System.Diagnostics.Debug.WriteLine(line);
                            IList<string> sl = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (sl.Count == 0 || sl[0] != "1")
                            {
                                MyUtility.Msg.WarningBox("Format is not correct!");
                                return;
                            }
                            else
                            {
                                DataRow dr = selectDataTable.NewRow();
                                if (sl[1].Length >= 13)
                                {

                                    dr["ID"] = string.Empty;
                                    dr["selected"] = 1;

                                    string sqlCmd = string.Empty;
                                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                                    if (sl.Count > 1 && sl[1].Length > 13)
                                    {
                                        sqlParameters.Add(new SqlParameter("@ID", sl[1].Substring(0, 13)));
                                        sqlParameters.Add(new SqlParameter("@CTNStartNo", sl[1].Substring(13, sl[1].Length - 13).TrimStart('^')));
                                        sqlParameters.Add(new SqlParameter("@SCICtnNo", sl[1].GetPackScanContent()));
                                        sqlCmd = @"
select  pd.OrderID
        , pd.OrderShipmodeSeq  
        , p.MDivisionID
        , pd.TransferDate
        , pd.SCICtnNo
        , pd.ID
        , pd.CTNStartNo
		,[SIZE] = size.val
		,[Qty] = qty.val
from    PackingList_Detail pd WITH (NOLOCK) 
inner join PackingList p WITH (NOLOCK) on pd.id = p.id
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.SizeCode) 
		from
		(
			select distinct SizeCode 
			from PackingList_Detail
			where CTNStartNo in(pd.CTNStartNo) and p.id = id
	)
	tmp for xml path('')),1,1,'')
)size
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.ShipQty) 
		from
		(
			select distinct ShipQty 
			from PackingList_Detail
			where CTNStartNo in(pd.CTNStartNo) and p.id = id
		)tmp for xml path('')
	),1,1,'')
)Qty
where   ((pd.ID = @ID and pd.CTNStartNo = @CTNStartNo) or pd.SCICtnNo = @SCICtnNo) 
        and pd.CTNQty > 0 
        and pd.DisposeFromClog= 0
        and ((
			[ReturnDate] is null and 
			[TransferDate] is null and 
			[PackErrTransferDate] is null and
			([DRYReceiveDate] is null or([DRYReceiveDate] is not null and [DRYTransferDate] is not null))
		) or [ReturnDate] is not null)
";
                                    }

                                    if (MyUtility.Check.Seek(sqlCmd, sqlParameters, out seekData))
                                    {
                                        #region checkM & checkTransfer
                                        if (!seekData["MDivisionID"].ToString().EqualString(Env.User.Keyword))
                                        {
                                            loginMErr.Rows.Add(dr.ItemArray);
                                            continue;
                                        }

                                        if (!seekData["TransferDate"].ToString().Empty())
                                        {
                                            transferErr.Rows.Add(dr.ItemArray);
                                            continue;
                                        }
                                        #endregion
                                        dr["PackingListID"] = seekData["ID"].ToString().Trim();
                                        dr["CTNStartNo"] = seekData["CTNStartNo"].ToString().Trim();
                                        dr["OrderID"] = seekData["OrderID"].ToString().Trim();
                                        dr["SCICtnNo"] = seekData["SCICtnNo"].ToString().Trim();
                                        dr["SIZE"] = seekData["SIZE"].ToString().Trim();
                                        dr["QTY"] = seekData["Qty"].ToString().Trim();
                                        string seq = seekData["OrderShipmodeSeq"].ToString().Trim();
                                        sqlCmd = string.Format(
                                            @"
select  a.StyleID
        , a.SeasonID
        , a.BrandID
        , a.Customize1
        , a.CustPONo
        , b.Alias
        , oq.BuyerDelivery 
from Orders a WITH (NOLOCK) 
left join Country b WITH (NOLOCK) on b.ID = a.Dest
left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{1}'
where   a.ID = '{0}'",
                                            dr["OrderID"].ToString(),
                                            seq);
                                        if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                        {
                                            dr["StyleID"] = seekData["StyleID"].ToString().Trim();
                                            dr["SeasonID"] = seekData["SeasonID"].ToString().Trim();
                                            dr["BrandID"] = seekData["BrandID"].ToString().Trim();
                                            dr["Customize1"] = seekData["Customize1"].ToString().Trim();
                                            dr["CustPONo"] = seekData["CustPONo"].ToString().Trim();
                                            dr["Alias"] = seekData["Alias"].ToString().Trim();
                                            dr["BuyerDelivery"] = seekData["BuyerDelivery"];
                                        }

                                        selectDataTable.Rows.Add(dr);
                                        insertCount++;
                                    }
                                    else
                                    {
                                        sqlCmd = string.Empty;
                                        if (sl.Count > 1)
                                        {
                                            dr["ID"] = string.Empty;
                                            dr["selected"] = 1;
                                            dr["CustCTN"] = sl[1];
                                            sqlCmd = $@"
select  pd.OrderID
        , pd.OrderShipmodeSeq  
        , p.MDivisionID
        , pd.TransferDate
		,pd.id,pd.CTNStartNo
        ,pd.SCICtnNo
		,[size] = size.val
		,[qty] = qty.val
from    PackingList_Detail pd WITH (NOLOCK) 
inner join PackingList p WITH (NOLOCK) on pd.id = p.id
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.SizeCode) 
		from
		(
			select distinct SizeCode 
			from PackingList_Detail
			where CTNStartNo in(pd.CTNStartNo) and p.id = id
	)
	tmp for xml path('')),1,1,'')
)size
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.ShipQty) 
		from
		(
			select distinct ShipQty 
			from PackingList_Detail
			where CTNStartNo in(pd.CTNStartNo) and p.id = id
		)tmp for xml path('')
	),1,1,'')
)Qty
where   pd.CustCTN= '{dr["CustCTN"]}' 
        and pd.CTNQty > 0 
        and pd.DisposeFromClog= 0
        and ((
			[ReturnDate] is null and 
			[TransferDate] is null and 
			[PackErrTransferDate] is null and
			([DRYReceiveDate] is null or([DRYReceiveDate] is not null and [DRYTransferDate] is not null))
		) or [ReturnDate] is not null)
";

                                            if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                            {
                                                #region checkM & checkTransfer
                                                if (!seekData["MDivisionID"].ToString().EqualString(Env.User.Keyword))
                                                {
                                                    loginMErr.Rows.Add(dr.ItemArray);
                                                    continue;
                                                }

                                                if (!seekData["TransferDate"].ToString().Empty())
                                                {
                                                    transferErr.Rows.Add(dr.ItemArray);
                                                    continue;
                                                }
                                                #endregion

                                                dr["OrderID"] = seekData["OrderID"].ToString().Trim();
                                                dr["SCICtnNo"] = seekData["SCICtnNo"].ToString().Trim();
                                                string seq = seekData["OrderShipmodeSeq"].ToString().Trim();
                                                string packinglistid = seekData["id"].ToString().Trim();
                                                string cTNStartNo = seekData["CTNStartNo"].ToString().Trim();
                                                string size = seekData["SIZE"].ToString().Trim();
                                                string qty = seekData["Qty"].ToString().Trim();

                                                sqlCmd = string.Format(
                                                    @"
select  a.StyleID
        , a.SeasonID
        , a.BrandID
        , a.Customize1
        , a.CustPONo
        , b.Alias
        , oq.BuyerDelivery 
from Orders a WITH (NOLOCK) 
left join Country b WITH (NOLOCK) on b.ID = a.Dest
left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{1}'
where   a.ID = '{0}'",
                                                    dr["OrderID"].ToString(),
                                                    seq);
                                                if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                                {
                                                    dr["StyleID"] = seekData["StyleID"].ToString().Trim();
                                                    dr["SeasonID"] = seekData["SeasonID"].ToString().Trim();
                                                    dr["BrandID"] = seekData["BrandID"].ToString().Trim();
                                                    dr["Customize1"] = seekData["Customize1"].ToString().Trim();
                                                    dr["CustPONo"] = seekData["CustPONo"].ToString().Trim();
                                                    dr["Alias"] = seekData["Alias"].ToString().Trim();
                                                    dr["BuyerDelivery"] = seekData["BuyerDelivery"];
                                                    dr["packinglistid"] = packinglistid.Trim();
                                                    dr["CTNStartNo"] = cTNStartNo.Trim();
                                                    dr["SIZE"] = size.Trim();
                                                    dr["QTY"] = qty.Trim();
                                                }

                                                selectDataTable.Rows.Add(dr);
                                                insertCount++;
                                            }
                                            else
                                            {
                                                notFoundErr.Rows.Add(dr.ItemArray);
                                            }
                                        }
                                        else
                                        {
                                            notFoundErr.Rows.Add(dr.ItemArray);
                                        }
                                    }
                                }
                                else
                                {
                                    string sqlCmd = string.Empty;
                                    if (sl.Count > 1)
                                    {
                                        dr["ID"] = string.Empty;
                                        dr["selected"] = 1;
                                        dr["CustCTN"] = sl[1];
                                        sqlCmd = $@"
select  pd.OrderID
        , pd.OrderShipmodeSeq  
        , p.MDivisionID
        , pd.TransferDate
        , pd.TransferDate
		,pd.id,pd.CTNStartNo
        ,pd.SCICtnNo
        ,[size] = size.val
		,[qty] = qty.val
from    PackingList_Detail pd WITH (NOLOCK) 
inner join PackingList p WITH (NOLOCK) on pd.id = p.id
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.SizeCode) 
		from
		(
			select distinct SizeCode 
			from PackingList_Detail
			where CTNStartNo in(pd.CTNStartNo) and p.id = id
	)
	tmp for xml path('')),1,1,'')
)size
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.ShipQty) 
		from
		(
			select distinct ShipQty 
			from PackingList_Detail
			where CTNStartNo in(pd.CTNStartNo) and p.id = id
		)tmp for xml path('')
	),1,1,'')
)Qty
where   pd.CustCTN= '{dr["CustCTN"]}' 
        and pd.CTNQty > 0 
        and pd.DisposeFromClog= 0
        and ((
			[ReturnDate] is null and 
			[TransferDate] is null and 
			[PackErrTransferDate] is null and
			([DRYReceiveDate] is null or([DRYReceiveDate] is not null and [DRYTransferDate] is not null))
		) or [ReturnDate] is not null)
";

                                        if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                        {
                                            #region checkM & checkTransfer
                                            if (!seekData["MDivisionID"].ToString().EqualString(Env.User.Keyword))
                                            {
                                                loginMErr.Rows.Add(dr.ItemArray);
                                                continue;
                                            }

                                            if (!seekData["TransferDate"].ToString().Empty())
                                            {
                                                transferErr.Rows.Add(dr.ItemArray);
                                                continue;
                                            }
                                            #endregion

                                            dr["OrderID"] = seekData["OrderID"].ToString().Trim();
                                            dr["SCICtnNo"] = seekData["SCICtnNo"].ToString().Trim();
                                            string size = seekData["SIZE"].ToString().Trim();
                                            string qty = seekData["Qty"].ToString().Trim();
                                            string packinglistid = seekData["id"].ToString().Trim();
                                            string cTNStartNo = seekData["CTNStartNo"].ToString().Trim();
                                            string seq = seekData["OrderShipmodeSeq"].ToString().Trim();
                                            sqlCmd = string.Format(
                                                @"
select  a.StyleID
        , a.SeasonID
        , a.BrandID
        , a.Customize1
        , a.CustPONo
        , b.Alias
        , oq.BuyerDelivery 
from Orders a WITH (NOLOCK) 
left join Country b WITH (NOLOCK) on b.ID = a.Dest
left join Order_QtyShip oq WITH (NOLOCK) on oq.ID = a.ID and oq.Seq = '{1}'
where   a.ID = '{0}'",
                                                dr["OrderID"].ToString(),
                                                seq);
                                            if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                            {
                                                dr["StyleID"] = seekData["StyleID"].ToString().Trim();
                                                dr["SeasonID"] = seekData["SeasonID"].ToString().Trim();
                                                dr["BrandID"] = seekData["BrandID"].ToString().Trim();
                                                dr["Customize1"] = seekData["Customize1"].ToString().Trim();
                                                dr["CustPONo"] = seekData["CustPONo"].ToString().Trim();
                                                dr["Alias"] = seekData["Alias"].ToString().Trim();
                                                dr["BuyerDelivery"] = seekData["BuyerDelivery"];
                                                dr["packinglistid"] = packinglistid.Trim();
                                                dr["CTNStartNo"] = cTNStartNo.Trim();
                                                dr["SIZE"] = size.Trim();
                                                dr["QTY"] = qty.Trim();
                                            }

                                            insertCount++;
                                            selectDataTable.Rows.Add(dr);
                                        }
                                        else
                                        {
                                            notFoundErr.Rows.Add(dr.ItemArray);
                                        }
                                    }
                                    else
                                    {
                                        notFoundErr.Rows.Add(dr.ItemArray);
                                    }
                                }
                            }
                        }

                        this.HideWaitMessage();

                        if (insertCount == 0)
                        {
                            MyUtility.Msg.WarningBox("All data were not found or order's M is not equal to login M or transferred.");
                            return;
                        }

                        StringBuilder warningmsg = new StringBuilder();
                        if (notFoundErr.Rows.Count > 0)
                        {
                            warningmsg.Append("Data not found." + Environment.NewLine);
                            foreach (DataRow dr in notFoundErr.Rows)
                            {
                                warningmsg.Append(string.Format(@"PackingListID: {0} CTN#: {1} " + Environment.NewLine, dr["PackingListID"], dr["CTNStartNo"]));
                            }
                        }

                        if (loginMErr.Rows.Count > 0)
                        {
                            if (warningmsg.ToString().Length > 0)
                            {
                                warningmsg.Append(Environment.NewLine);
                            }

                            warningmsg.Append("Order's M is not equal to login M." + Environment.NewLine);
                            foreach (DataRow dr in loginMErr.Rows)
                            {
                                warningmsg.Append(string.Format(@"PackingListID: {0} CTN#: {1} " + Environment.NewLine, dr["PackingListID"], dr["CTNStartNo"]));
                            }
                        }

                        if (transferErr.Rows.Count > 0)
                        {
                            if (warningmsg.ToString().Length > 0)
                            {
                                warningmsg.Append(Environment.NewLine);
                            }

                            warningmsg.Append("Data transferred." + Environment.NewLine);
                            foreach (DataRow dr in transferErr.Rows)
                            {
                                warningmsg.Append(string.Format(@"PackingListID: {0} CTN#: {1} " + Environment.NewLine, dr["PackingListID"], dr["CTNStartNo"]));
                            }
                        }

                        if (warningmsg.ToString().Length > 0)
                        {
                            MyUtility.Msg.WarningBox(warningmsg.ToString());
                        }

                        this.ControlButton4Text("Cancel");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 檢查是否有勾選資料
            this.gridDetail.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("No data need to import!");
                return;
            }

            IList<string> insertCmds = new List<string>();
            IList<string> updateCmds = new List<string>();

            // 組要Insert進TransferToClog的資料
            foreach (DataRow dr in selectedData)
            {
                insertCmds.Add(string.Format(
                    @"insert into TransferToClog(TransferDate,MDivisionID,PackingListID,OrderID,CTNStartNo, AddDate,AddName,SCICtnNo)
values (GETDATE(),'{0}','{1}','{2}','{3}',GETDATE(),'{4}','{5}');",
                    Env.User.Keyword,
                    MyUtility.Convert.GetString(dr["PackingListID"]),
                    MyUtility.Convert.GetString(dr["OrderID"]),
                    MyUtility.Convert.GetString(dr["CTNStartNo"]),
                    Env.User.UserID,
                    MyUtility.Convert.GetString(dr["SCICtnNo"])));

                // 要順便更新PackingList_Detail
                updateCmds.Add(string.Format(
                    @"update PackingList_Detail 
set TransferDate = GETDATE(), ReceiveDate = null, ClogLocationId = '', ReturnDate = null 
where ID = '{0}' and OrderID = '{1}' and CTNStartNo = '{2}' and DisposeFromClog= 0
; ",
                    MyUtility.Convert.GetString(dr["PackingListID"]),
                    MyUtility.Convert.GetString(dr["OrderID"]),
                    MyUtility.Convert.GetString(dr["CTNStartNo"])));

                // 也要順便更新Orders.LastCTNTransDate
                updateCmds.Add(string.Format(
                   @"
update o
set o.LastCTNTransDate = GETDATE()
from Orders o
inner join PackingList_Detail pd on pd.OrderID = o.ID
where pd.ID = '{0}' and pd.OrderID = '{1}'
; ",
                   MyUtility.Convert.GetString(dr["PackingListID"]),
                   MyUtility.Convert.GetString(dr["OrderID"])));
            }

            foreach (string packingListID in selectedData.Select(s => s["PackingListID"].ToString()).Distinct())
            {
                updateCmds.Add($"update PackingList set CannotModify = 1 where ID = '{packingListID}'");
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
        }

        // Close/Cancel
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ControlButton4Text(string showText)
        {
            this.btnClose.Text = showText;
        }
    }
}
