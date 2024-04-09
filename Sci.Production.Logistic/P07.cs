using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Sci.Production.PublicPrg;
using System.Transactions;
using System.Linq;
using Ict.Win;
using Sci.Production.Prg;
using static Sci.MyUtility;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic P08
    /// </summary>
    public partial class P07 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P07(ToolStripMenuItem menuitem)
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
                 .CheckBox("Selected", header: string.Empty, iseditable: true, trueValue: 1, falseValue: 0)
                 .Text("CFANeedInsp", header: "CFA", width: Widths.AnsiChars(3), iseditingreadonly: false)
                 .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("SeasonID", header: "Season", width: Widths.AnsiChars(7), iseditingreadonly: true)
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Alias", header: "Destination", width: Widths.AnsiChars(7), iseditingreadonly: true)
                 .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(12), iseditingreadonly: true)
                 .Text("ClogLocationID", header: "Location No", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Size", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Qty", header: "Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("PCCTN", header: "PC/CTN", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            string strSciDeliveryStart = this.dateSciDelivery.Value1.Empty() ? string.Empty : ((DateTime)this.dateSciDelivery.Value1).ToString("yyyy/MM/dd");
            string strSciDeliveryEnd = this.dateSciDelivery.Value2.Empty() ? string.Empty : ((DateTime)this.dateSciDelivery.Value2).ToString("yyyy/MM/dd");
            string strReceiveDateStart = this.dateReceiveDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateReceiveDate.Value1).ToString("yyyy/MM/dd");
            string strReceiveDateEnd = this.dateReceiveDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateReceiveDate.Value2).ToString("yyyy/MM/dd");

            #region SqlParameter
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@OrderID", this.txtSP.Text));
            listSQLParameter.Add(new SqlParameter("@PoNo", this.txtPO.Text));
            listSQLParameter.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            listSQLParameter.Add(new SqlParameter("@SciDeliveryStart", strSciDeliveryStart));
            listSQLParameter.Add(new SqlParameter("@SciDeliveryEnd", strSciDeliveryEnd));
            listSQLParameter.Add(new SqlParameter("@ReceiveDateStart", strReceiveDateStart));
            listSQLParameter.Add(new SqlParameter("@ReceiveDateEnd", strReceiveDateEnd));
            #endregion

            #region SQL Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(strSciDeliveryStart)
                && !MyUtility.Check.Empty(strSciDeliveryEnd))
            {
                listSQLFilter.Add("and o.SciDelivery between @SciDeliveryStart and @SciDeliveryEnd");
            }

            if (!MyUtility.Check.Empty(strReceiveDateStart)
               && !MyUtility.Check.Empty(strReceiveDateEnd))
            {
                listSQLFilter.Add("and p2.ReceiveDate between @ReceiveDateStart and @ReceiveDateEnd");
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                listSQLFilter.Add("and p2.OrderID = @OrderID");
            }

            if (!MyUtility.Check.Empty(this.txtPO.Text))
            {
                listSQLFilter.Add("and o.CustPoNo= @PoNo");
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                listSQLFilter.Add("and p2.id= @PackID");
            }

            #endregion

            this.ShowWaitMessage("Data Loading...");

            #region Sql Command

            string strCmd = $@"
select distinct
[selected] = 0
,[CFANeedInsp] = iif(CFANeedInsp=1,'Y','')
,p2.ID
,p2.CTNStartNo
,o1.OrderID 
,o.CustPONo
,o.StyleID
,o.SeasonID
,o.BrandID
,c.Alias
,o.BuyerDelivery
,p2.ClogLocationId
,p2.remark
,p2.SCICtnNo
,[Size] = size.val
,[Qty] = Qty.val
,[PCCTN] = QtyPer.val
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
left join orders o WITH (NOLOCK) on o.id	= p2.orderid
left join Country c WITH (NOLOCK) on c.id=o.dest
outer apply(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID from PackingList_Detail pd WITH (NOLOCK)
			where p2.orderid=pd.orderid
		) o1
		for xml path('')
	),1,1,'')
) o1
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.SizeCode) 
		from
		(
			select SizeCode 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
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
			select ShipQty 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
		)tmp for xml path('')
	),1,1,'')
)Qty
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.QtyPerCTN) 
		from
		(
			select QtyPerCTN 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
		)tmp for xml path('')
	),1,1,'')
)QtyPer

where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.ReceiveDate is not null
and p2.TransferCFADate is null
and p2.CFAReturnClogDate is null
and p2.DisposeFromClog= 0
and (po.Status = 'New' or po.Status is null)
and p1.PLCtnTrToRgCodeDate is null
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
order by p2.ID,p2.CTNStartNo";

            #endregion
            DataTable dtDBSource;
            DualResult result = DBProxy.Current.Select(string.Empty, strCmd, listSQLParameter, out dtDBSource);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (dtDBSource.Rows.Count < 1)
            {
                this.listControlBindingSource1.DataSource = null;
                MyUtility.Msg.InfoBox("Data not found !");
            }
            else
            {
                this.listControlBindingSource1.DataSource = dtDBSource;
                this.Grid_Filter();
            }

            this.HideWaitMessage();
        }

        // Import From Barcode
        private void BtnImportFromBarcode_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data Loading...");

            // 設定只能選txt檔
            this.openFileDialog1.Filter = "txt files (*.txt)|*.txt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 先將Grid的結構給開出來
                string selectCommand = @"
Select distinct '' as CFANeedInsp, 1 as selected, b.Id, b.OrderID, b.CTNStartNo,
c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, d.Alias,
c.BuyerDelivery , b.ClogLocationId , b.remark, b.CustCTN,b.SCICtnNo,[Size] = '',[Qty] = '',[PCCTN] = ''
from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) 
, Orders c WITH (NOLOCK) , Country d WITH (NOLOCK) where 1=0";
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
                    DataTable notFoundErr = selectDataTable.Clone();
                    int insertCount = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 每一行的第一個字母必須是1
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

                            // PackingID+CTN# 是連起來的ex: MA2PG180105821 前13碼是PackID 13碼後都是CTN#
                            if (sl[1].Length >= 13)
                            {
                                string sqlCmd = $@"

select distinct
[selected] = 1
,CFANeedInsp
,p2.ID
,p2.CTNStartNo
,o1.OrderID 
,o.CustPONo
,o.StyleID
,o.SeasonID
,o.BrandID
,c.Alias
,o.BuyerDelivery
,p2.ClogLocationId
,p2.remark
,p2.SCICtnNo
,[Size] = size.val
,[Qty] = Qty.val
,[PCCTN] = QtyPer.val
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
left join orders o WITH (NOLOCK) on o.id	= p2.orderid
left join Country c WITH (NOLOCK) on c.id=o.dest
outer apply(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID from PackingList_Detail pd WITH (NOLOCK)
			where p2.orderid=pd.orderid
		) o1
		for xml path('')
	),1,1,'')
) o1
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.SizeCode) 
		from
		(
			select SizeCode 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
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
			select ShipQty 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
		)tmp for xml path('')
	),1,1,'')
)Qty
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.QtyPerCTN) 
		from
		(
			select QtyPerCTN 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
		)tmp for xml path('')
	),1,1,'')
)QtyPer

where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.ReceiveDate is not null
and p2.DisposeFromClog= 0
and p2.TransferCFADate is null
and p2.CFAReturnClogDate is null
and (po.Status = 'New' or po.Status is null)
and ((p2.id='{sl[1].Substring(0, 13)}' and p2.CTNStartNo='{sl[1].Substring(13).TrimStart('^')}') or p2.SCICtnNo = '{sl[1].GetPackScanContent()}')
order by p2.ID,p2.CTNStartNo
";
                                if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                {
                                    dr["selected"] = 1;
                                    dr["CFANeedInsp"] = (bool)seekData["CFANeedInsp"] ? "Y" : string.Empty;
                                    dr["ID"] = seekData["ID"].ToString().Trim();
                                    dr["CTNStartNo"] = seekData["CTNStartNo"];
                                    dr["OrderID"] = seekData["OrderID"];
                                    dr["SCICtnNo"] = seekData["SCICtnNo"];
                                    dr["CustPONo"] = seekData["CustPONo"];
                                    dr["StyleID"] = seekData["StyleID"];
                                    dr["SeasonID"] = seekData["SeasonID"];
                                    dr["BrandID"] = seekData["BrandID"];
                                    dr["Alias"] = seekData["Alias"];
                                    dr["BuyerDelivery"] = seekData["BuyerDelivery"];
                                    dr["ClogLocationId"] = seekData["ClogLocationId"];
                                    dr["remark"] = seekData["remark"];
                                    dr["Size"] = seekData["Size"];
                                    dr["Qty"] = seekData["Qty"];
                                    dr["PCCTN"] = seekData["PCCTN"];
                                    selectDataTable.Rows.Add(dr);
                                    insertCount++;
                                }
                                else
                                {
                                    sqlCmd = $@"
select distinct
[selected] = 1
,CFANeedInsp
,p2.ID
,p2.CTNStartNo
,o1.OrderID 
,o.CustPONo
,o.StyleID
,o.SeasonID
,o.BrandID
,c.Alias
,o.BuyerDelivery
,p2.ClogLocationId
,p2.remark
,p2.SCICtnNo
,[Size] = size.val
,[Qty] = Qty.val
,[PCCTN] = QtyPer.val
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
left join orders o WITH (NOLOCK) on o.id	= p2.orderid
left join Country c WITH (NOLOCK) on c.id=o.dest
outer apply(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID from PackingList_Detail pd WITH (NOLOCK)
			where p2.orderid=pd.orderid
		) o1
		for xml path('')
	),1,1,'')
) o1
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.SizeCode) 
		from
		(
			select SizeCode 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
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
			select ShipQty 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
		)tmp for xml path('')
	),1,1,'')
)Qty
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.QtyPerCTN) 
		from
		(
			select QtyPerCTN 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
		)tmp for xml path('')
	),1,1,'')
)QtyPer
where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.ReceiveDate is not null
and p2.TransferCFADate is null
and p2.DisposeFromClog= 0
and p2.CFAReturnClogDate is null
and (po.Status = 'New' or po.Status is null)
and p2.CustCTN='{sl[1]}'
order by p2.ID,p2.CTNStartNo
";
                                    if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                    {
                                        dr["selected"] = 1;
                                        dr["CFANeedInsp"] = (bool)seekData["CFANeedInsp"] ? "Y" : string.Empty;
                                        dr["ID"] = seekData["ID"].ToString().Trim();
                                        dr["CTNStartNo"] = seekData["CTNStartNo"];
                                        dr["OrderID"] = seekData["OrderID"];
                                        dr["SCICtnNo"] = seekData["SCICtnNo"];
                                        dr["CustPONo"] = seekData["CustPONo"];
                                        dr["StyleID"] = seekData["StyleID"];
                                        dr["SeasonID"] = seekData["SeasonID"];
                                        dr["BrandID"] = seekData["BrandID"];
                                        dr["Alias"] = seekData["Alias"];
                                        dr["BuyerDelivery"] = seekData["BuyerDelivery"];
                                        dr["ClogLocationId"] = seekData["ClogLocationId"];
                                        dr["remark"] = seekData["remark"];
                                        dr["Size"] = seekData["Size"];
                                        dr["Qty"] = seekData["Qty"];
                                        dr["PCCTN"] = seekData["PCCTN"];
                                        selectDataTable.Rows.Add(dr);
                                        insertCount++;
                                    }
                                    else
                                    {
                                        notFoundErr.Rows.Add(dr.ItemArray);
                                    }
                                }
                            }
                            else
                            {
                               string sqlCmd = $@"
select distinct
[selected] = 1
,CFANeedInsp
,p2.ID
,p2.CTNStartNo
,o1.OrderID 
,o.CustPONo
,o.StyleID
,o.SeasonID
,o.BrandID
,c.Alias
,o.BuyerDelivery
,p2.ClogLocationId
,p2.remark
,p2.SCICtnNo
,[Size] = size.val
,[Qty] = Qty.val
,[PCCTN] = QtyPer.val
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
left join orders o WITH (NOLOCK) on o.id	= p2.orderid
left join Country c WITH (NOLOCK) on c.id=o.dest
outer apply(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID from PackingList_Detail pd WITH (NOLOCK)
			where p2.orderid=pd.orderid
		) o1
		for xml path('')
	),1,1,'')
) o1
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.SizeCode) 
		from
		(
			select SizeCode 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
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
			select ShipQty 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
		)tmp for xml path('')
	),1,1,'')
)Qty
outer apply
(
	select val = stuff(
	(
		select concat('/',tmp.QtyPerCTN) 
		from
		(
			select QtyPerCTN 
			from PackingList_Detail
			where  CTNStartNo in(p2.CTNStartNo) and p1.ID = ID
		)tmp for xml path('')
	),1,1,'')
)QtyPer
where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.ReceiveDate is not null
and p2.TransferCFADate is null
and p2.DisposeFromClog= 0
and p2.CFAReturnClogDate is null
and (po.Status = 'New' or po.Status is null)
and p2.CustCTN='{sl[1]}'
order by p2.ID,p2.CTNStartNo
";
                               if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                {
                                    dr["selected"] = 1;
                                    dr["CFANeedInsp"] = (bool)seekData["CFANeedInsp"] ? "Y" : string.Empty;
                                    dr["ID"] = seekData["ID"].ToString().Trim();
                                    dr["CTNStartNo"] = seekData["CTNStartNo"];
                                    dr["OrderID"] = seekData["OrderID"];
                                    dr["SCICtnNo"] = seekData["SCICtnNo"];
                                    dr["CustPONo"] = seekData["CustPONo"];
                                    dr["StyleID"] = seekData["StyleID"];
                                    dr["SeasonID"] = seekData["SeasonID"];
                                    dr["BrandID"] = seekData["BrandID"];
                                    dr["Alias"] = seekData["Alias"];
                                    dr["BuyerDelivery"] = seekData["BuyerDelivery"];
                                    dr["ClogLocationId"] = seekData["ClogLocationId"];
                                    dr["remark"] = seekData["remark"];
                                    dr["Size"] = seekData["Size"];
                                    dr["Qty"] = seekData["Qty"];
                                    dr["PCCTN"] = seekData["PCCTN"];
                                    selectDataTable.Rows.Add(dr);
                                    insertCount++;
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
                            warningmsg.Append(string.Format(@"PackingListID: {0} CTN#: {1} " + Environment.NewLine, dr["ID"], dr["CTNStartNo"]));
                        }
                    }

                    if (warningmsg.ToString().Length > 0)
                    {
                        MyUtility.Msg.WarningBox(warningmsg.ToString());
                    }
                }
            }

            this.HideWaitMessage();
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

            string sqlUpdate = $@"
select distinct Remark = 
	case when t3.PLCtnTrToRgCodeDate is not null then 'This PL already transfer to shipping factory, cannot transfer to CFA Inspection.'
		 when t2.ReceiveDate is null then 'This carton not yet Received!'
		 when t2.TransferCFADate is not null then 'This carton has been transferred to CFA!'
		 when t3.MdivisionID !='{Env.User.Keyword}' then 'The orders M is not equal to login M.'
		 when po.Status not in ('New','') then 'This carton Already pullout!'
	else '' end
,t.CFANeedInsp
,PackingListID = t2.ID
,t.CTNStartNo
,t.OrderID
,t.CustPONo
,t.StyleID
,t.SeasonID              
,t.BrandID
,t.Alias
,t.BuyerDelivery
,t.ClogLocationId
,t.Size
,t.Qty
,t.PCCTN
,t.SCICtnNo
,t3.MDivisionID
into #tmp2
from #tmp t
inner join PackingList_Detail t2 WITH (NOLOCK) on t2.id=t.ID and t2.CTNStartNo = t.CTNStartNo
inner join Packinglist t3 with (nolock) on t3.id=t2.id
left join Pullout po WITH (NOLOCK) on po.ID = t3.PulloutID
where  t2.CTNStartNo != '' 
	and t3.Type in ('B','L')	
	and t2.CFAReturnClogDate is null
	and t2.DisposeFromClog= 0
	and t3.PLCtnTrToRgCodeDate is null

-- 顯示有問題的資料
select * from #tmp2 where Remark !=''

-- 要判斷是否可以更新,確保資料沒被新增錯誤的
insert into TransferToCFA(TransferDate,MDivisionID,OrderID,PackingListID,CTNStartNo,AddName,AddDate,OrigloactionID,SCICtnNo)
select distinct TransferDate = CONVERT(varchar(100), GETDATE(), 111)
    , MDivisionID = t.MdivisionID
    , OrderID = t.OrderID
    , PackingListID = t.PackingListID
    , CTNStartNo = t.CTNStartNo
    , AddName = '{Env.User.UserID}'
    , AddDate = GetDate()
    , ClogLocationId = t.ClogLocationId
    , SCICtnNo = t.SCICtnNo
from #tmp2 t
where t.Remark =''

update pd
set pd.TransferCFADate = CONVERT(varchar(100), GETDATE(), 111)
, pd.ClogReceiveCFADate = null
, pd.ClogLocationID  = '2CFA'
from PackingList_Detail pd
inner join PackingList p on pd.id = p.id
left join pullout pu on p.PulloutID = pu.id
where exists(
	select 1 from #tmp2 t
	where pd.id = t.PackingListID
	and pd.CTNStartNo = t.CTNStartNo
	and t.Remark =''
)

-- Update Orders的資料
select distinct OrderID from #tmp2 where Remark=''

drop table #tmp2
";
            DualResult result1 = Ict.Result.True, result2 = Ict.Result.True;
            DataTable[] dtCheck;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    DualResult result = MyUtility.Tool.ProcessWithDatatable(
                     selectedData.CopyToDataTable(),
                     string.Empty,
                     sqlUpdate,
                     out dtCheck);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result.ToString());
                        return;
                    }

                    if (dtCheck[1].Rows.Count > 0)
                    {
                        DualResult prgResult = Prgs.UpdateOrdersCTN(dtCheck[1]);

                        if (prgResult)
                        {
                            transactionScope.Complete();
                            transactionScope.Dispose();
                            MyUtility.Msg.InfoBox("Complete!!");

                            if (dt.AsEnumerable().Any(row => !row["Selected"].EqualDecimal(1)))
                            {
                                this.listControlBindingSource1.DataSource = dt.AsEnumerable().Where(row => !row["Selected"].EqualDecimal(1)).CopyToDataTable();
                            }
                            else
                            {
                                this.listControlBindingSource1.DataSource = null;
                            }
                        }
                        else
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("Save failed, Pleaes re-try");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            if (dtCheck[0] != null && dtCheck[0].Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid(dtCheck[0], "Update data failed, please check below Remark message.");
                m.grid1.Columns[0].Width = 270;
                m.TopMost = true;
                return;
            }
        }

        private void Grid_Filter()
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

            if (!MyUtility.Check.Empty(dt) && dt.Rows.Count > 0)
            {
                string filter = string.Empty;
                switch (this.chkCFA.Checked)
                {
                    case false:
                        if (MyUtility.Check.Empty(this.gridDetail))
                        {
                            break;
                        }

                        filter = string.Empty;
                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;

                    case true:
                        if (MyUtility.Check.Empty(this.gridDetail))
                        {
                            break;
                        }

                        filter = " CFANeedInsp= 'Y' ";
                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;
                }
            }
        }

        private void ChkCFA_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }
    }
}
