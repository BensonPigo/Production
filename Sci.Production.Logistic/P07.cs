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
        private DataTable dtError = new DataTable();
        private int progressCnt = 0;
        private int threadCnt = 0;
        private System.ComponentModel.BackgroundWorker[] workers;
        private DataTable selectDataTable;
        private int completeCnt = 0;
        private bool cancelWorker;

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
            this.labProcessingBar.Text = "0/0";

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
            DualResult result = DBProxy.Current.Select(string.Empty, strCmd, listSQLParameter, out DataTable dtDBSource);

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
            this.completeCnt = 0;
            this.progressCnt = 0;
            this.cancelWorker = false;

            if (MyUtility.Check.Empty(dt))
            {
                MyUtility.Msg.InfoBox("No data need to import!");
                return;
            }

            this.dtError = dt.Clone();

            if (dt.AsEnumerable().Any(row => row["Selected"].EqualDecimal(1)) == false)
            {
                MyUtility.Msg.InfoBox("Please select data first!");
                return;
            }

            this.selectDataTable = dt.AsEnumerable().Where(r => MyUtility.Convert.GetInt(r["Selected"]) == 1).OrderBy(row => row.Field<string>("OrderID")).ToList().CopyToDataTable();

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
                int batchSize = 200;

                for (int i = 0; i < this.threadCnt; i++)
                {
                    int remainingRows = rowCnt - processedRows;
                    int rowsToProcess = System.Math.Min(batchSize, remainingRows);
                    this.workers[i].RunWorkerAsync(new object[] { this.selectDataTable, processedRows, rowsToProcess });

                    // 更新處理行數
                    processedRows += rowsToProcess;
                }
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

        private StringBuilder UpdateData(DataRow dr)
        {
            StringBuilder singleWarningmsg = new StringBuilder();
            string checkPackSql = $@"
select p2.ReceiveDate ,p2.TransferCFADate ,p.Status 
from PackingList_detail p2
inner join PackingList p1 on p2.id=p1.id
left join pullout p on p1.PulloutID = p.id
where p2.id='{dr["id"].ToString().Trim()}' 
and p2.CTNStartNo='{dr["CTNStartNo"].ToString().Trim()}' 
and p2.DisposeFromClog= 0
and p1.PLCtnTrToRgCodeDate is null
and p2.CFAReturnClogDate is null
";
            if (!MyUtility.Check.Seek(checkPackSql, null, out DataRow drPackResult))
            {
                singleWarningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> does not exist!" + Environment.NewLine);
            }
            else
            {
                if (MyUtility.Check.Empty(drPackResult["ReceiveDate"]))
                {
                    singleWarningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}>This CTN# has been return." + Environment.NewLine);
                }
                else if (!MyUtility.Check.Empty(drPackResult["TransferCFADate"]))
                {
                    singleWarningmsg.Append($@"<CTN#:{dr["id"]}{dr["CTNStartNo"]}> has been transferred to CFA!" + Environment.NewLine);
                }
                else if (drPackResult["Status"].ToString().Trim().ToUpper() == "CONFIRMED" || drPackResult["Status"].ToString().Trim().ToUpper() == "LOCKED")
                {
                    singleWarningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> Already pullout!" + Environment.NewLine);
                }

                // 代表都沒錯,可以單筆進行更新新增
                else
                {
                    IList<string> cmds = new List<string>();
                    cmds.Add(
                   $@"
update PackingList_Detail 
set TransferCFADate = CONVERT(varchar(100), GETDATE(), 111)
, ClogReceiveCFADate = null
, ClogLocationID  = '2CFA'
where id='{dr["id"].ToString().Trim()}' 
and CTNStartNo='{dr["CTNStartNo"].ToString().Trim()}' 
and DisposeFromClog= 0

insert into TransferToCFA(TransferDate,MDivisionID,OrderID,PackingListID,CTNStartNo,AddName,AddDate,OrigloactionID,SCICtnNo)
values(CONVERT(varchar(100), GETDATE(), 111),'{Env.User.Keyword}','{dr["OrderID"].ToString().Trim()}','{dr["ID"].ToString().Trim()}','{dr["CTNStartNo"].ToString().Trim()}','{Env.User.UserID}',GETDATE(),'{dr["ClogLocationId"]}','{dr["SCICtnNo"]}')
            ");

                    DualResult result1 = Ict.Result.True;
                    DualResult prgResult = Ict.Result.True;

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

                            // 使用lock來避免相同SP#互相Deadlocked
                            lock (prgResult = Prgs.UpdateOrdersCTN(dr["OrderID"].ToString()))
                            {
                            }

                            if (prgResult == false)
                            {
                                transactionScope.Dispose();
                                singleWarningmsg.Append(prgResult.ToString() + Environment.NewLine);
                            }
                            else
                            {
                                // 代表以新增庫存
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

            return singleWarningmsg;
        }

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
                    // 這裡才是真正中斷backgroundworker執行緒操作
                    if (this.cancelWorker == true)
                    {
                        e.Cancel = true;
                        return;
                    }

                    StringBuilder singleWarningmsg = new StringBuilder();

                    // 資料邏輯判斷 + 更新
                    singleWarningmsg = this.UpdateData(dr);

                    // 更新進度條
                    this.progressCnt++;

                    double barPercentage = System.Math.Abs(MyUtility.Convert.GetDouble(this.progressCnt) / this.selectDataTable.Rows.Count) * 100;
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

        private void BackgroundDownloadSticker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.completeCnt++;
            if (this.completeCnt == this.threadCnt)
            {
                // 檢查是否有勾選資料
                this.gridDetail.ValidateControl();
                this.listControlBindingSource1.EndEdit();

                if (e.Cancelled)
                {
                    MyUtility.Msg.WarningBox("Operation has been cancelled.");
                    this.listControlBindingSource1.DataSource = null;
                    return;
                }

                // 既然都全部完成了, 補回多執行緒會有漏算的數量產生
                this.progressCnt = this.selectDataTable.Rows.Count;
                this.labProcessingBar.Text = $"{this.selectDataTable.Rows.Count}/{this.selectDataTable.Rows.Count}";

                // 使用Find撈出的全部資料
                DataTable dt =
                        (DataTable)this.listControlBindingSource1.DataSource;

                if (this.dtError.Rows.Count > 0)
                {
                    DataTable dtCheck = dt.Clone();

                    // 有錯誤訊息的,再重跑一次!
                    foreach (DataRow dr in this.dtError.Rows)
                    {
                        StringBuilder singleWarningmsg = new StringBuilder();

                        // 資料邏輯判斷 + 更新
                        singleWarningmsg = this.UpdateData(dr);
                        if (singleWarningmsg.ToString().Length > 0)
                        {
                            DataRow drError = dtCheck.NewRow();
                            dr["Remark"] = singleWarningmsg;
                            dr.CopyTo(drError);
                            dtCheck.Rows.Add(drError);
                        }
                    }

                    // 這代表ReUpdate後錯誤還沒消掉,就直接顯示在畫面上
                    if (dtCheck.Rows.Count > 0)
                    {
                        MyUtility.Msg.WarningBox("Some carton cannot receive, please refer to field <Remark>.");

                        if (this.gridDetail.Rows.Cast<DataGridViewRow>().Any(row => !row.Cells["Selected"].Value.ToString().Equals("1")))
                        {
                            /*
                             沒勾選的放table #1
                             有錯誤的放table #2
                             再將2者合併一起, 畫面只會顯示沒勾的+有錯誤的
                             最後再將Selected清空
                             */

                            DataTable dtCopy = dt.AsEnumerable().Where(r => MyUtility.Convert.GetInt(r["selected"]) == 0).ToList().CopyToDataTable();
                            dtCopy.Merge(dtCheck, true, MissingSchemaAction.AddWithKey);
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
                            foreach (DataRow dr in dtCheck.Rows)
                            {
                                dr["Selected"] = false;
                            }

                            this.listControlBindingSource1.DataSource = dtCheck;
                        }
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

                        MyUtility.Msg.InfoBox("Complete!!");
                    }
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

                    MyUtility.Msg.InfoBox("Complete!!");
                }

                this.backgroundDownloadSticker.ReportProgress(0);

                // 先把UI介面鎖住
                this.SetInterfaceLocked(false);
            }
        }

        private void SetInterfaceLocked(bool isLocked)
        {
            // 鎖住或解鎖 UI 介面
            this.BtnFind.Enabled = !isLocked;
            this.btnImportFromBarcode.Enabled = !isLocked;
            this.btnImportFromBarcode.Enabled = !isLocked;
            this.BtnSave.Enabled = !isLocked;
            this.BtnClose.Enabled = !isLocked;
            this.chkCFA.Enabled = !isLocked;

            // 或者顯示一個等待光標等
            Cursor.Current = isLocked ? Cursors.WaitCursor : Cursors.Default;
        }

        private void P07_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.workers != null)
            {
                foreach (var item in this.workers.ToList())
                {
                    if (item.IsBusy)
                    {
                        this.cancelWorker = true;
                    }
                }
            }
        }
    }
}
