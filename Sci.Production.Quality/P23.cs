using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P23 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private string selectDataTable_DefaultView_Sort = string.Empty;
        private DataTable selectDataTable;
        private int progressCnt = 0;
        private DataTable dtError = new DataTable();

        /// <summary>
        /// P23
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;

            this.grid.CellValueChanged += (s, e) =>
            {
                if (this.grid.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    this.CalcCTNQty();
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid)
                 .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                 .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("StyleID", header: "Style#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
                 .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)

                 // .CellCFALocation("CFALocationID", header: "Location No", width: Widths.AnsiChars(10), M: Sci.Env.User.Keyword).Get(out this.col_location)
                 .Text("CFALocationID", header: "Location No", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .EditText("SaveRemark", header: "Save Remark", width: Widths.AnsiChars(15), iseditingreadonly: true);

            #region CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int rowIndex = 0;
            int columIndex = 0;
            this.grid.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.grid.Sorted += (s, e) =>
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
            #endregion
        }

        // Find
        private void Find()
        {
            string strTransferStart = this.dateTransferDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateTransferDate.Value1).ToString("yyyy/MM/dd");
            string strTransferEnd = this.dateTransferDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateTransferDate.Value2).ToString("yyyy/MM/dd");
            this.labProcessingBar.Text = "0/0";

            #region SqlParameter
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@OrderID", this.txtSPNo.Text));
            listSQLParameter.Add(new SqlParameter("@PoNo", this.txtPONo.Text));
            listSQLParameter.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            listSQLParameter.Add(new SqlParameter("@TransferStart", strTransferStart));
            listSQLParameter.Add(new SqlParameter("@TransferEnd", strTransferEnd));
            #endregion

            #region SQL Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(strTransferStart)
                && !MyUtility.Check.Empty(strTransferEnd))
            {
                listSQLFilter.Add("and p2.TransferCFADate between @TransferStart and @TransferEnd");
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                listSQLFilter.Add("and o.id = @OrderID");
            }

            if (!MyUtility.Check.Empty(this.txtPONo.Text))
            {
                listSQLFilter.Add("and o.CustPoNo= @PoNo");
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                listSQLFilter.Add("and p2.id= @PackID");
            }
            #endregion

            this.ShowWaitMessage("Data Loading....");

            #region Sql Command

            string strCmd = $@"
select distinct
[selected] = 0 
,p2.ID
,p2.CTNStartNo
,[OrderID] = o1.OrderID 
,o.CustPONo
,o.StyleID
,o.SeasonID
,o.BrandID
,c.Alias
,o.BuyerDelivery
,p2.remark
,p2.SCICtnNo
,[CFALocationID] = 'CFA'
,[SaveRemark] = ''
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
left join orders o WITH (NOLOCK) on o.id	= p2.orderid
left join Country c WITH (NOLOCK) on c.id=o.dest
outer apply(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID 
            from PackingList_Detail pd WITH (NOLOCK)
			where p2.ID = pd.ID
		) o1
		for xml path('')
	),1,1,'')
) o1
where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.DisposeFromClog= 0
and p2.TransferCFADate is not null
and p2.CFAReceiveDate  is null
and (po.Status ='New' or po.Status is null)
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
order by p2.ID,p2.CTNStartNo";
            #endregion
            DualResult result = DBProxy.Current.Select(string.Empty, strCmd, listSQLParameter, out this.selectDataTable);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (this.selectDataTable.Rows.Count < 1)
            {
                this.listControlBindingSource1.DataSource = null;
                MyUtility.Msg.InfoBox("Data not found !");
            }
            else
            {
                this.listControlBindingSource1.DataSource = this.selectDataTable;
            }

            this.HideWaitMessage();
            this.CalcCTNQty();
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private void BtnImportFromBarcode_Click(object sender, EventArgs e)
        {
            // 設定只能選txt檔
            this.openFileDialog1.Filter = "txt files (*.txt)|*.txt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 先將Grid的結構給開出來
                string selectCommand = @"
Select distinct  0 as selected, b.ID , b.OrderID, 
b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, d.Alias, c.BuyerDelivery, '' as Remark ,b.SCICtnNo,
b.CFALocationID
,SaveRemark = ''
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
                    DataTable notFoundErr = this.selectDataTable.Clone();
                    int insertCount = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 每一行的第一個字母必須是2
                        System.Diagnostics.Debug.WriteLine(line);
                        IList<string> sl = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sl.Count == 0 || sl[0] != "2")
                        {
                            MyUtility.Msg.WarningBox("Format is not correct!");
                            return;
                        }
                        else
                        {
                            DataRow dr = this.selectDataTable.NewRow();

                            // PackingID+CTN# 是連起來的ex: MA2PG180105821 前13碼是PackID 13碼後都是CTN#
                            if (sl[2].Length >= 13)
                            {
                                string sqlCmd = $@"

select distinct
[selected] = 1
,p2.ID
,p2.CTNStartNo
,o1.OrderID 
,o.CustPONo
,o.StyleID
,o.SeasonID
,o.BrandID
,c.Alias
,o.BuyerDelivery
,p2.remark
,p2.SCICtnNo
,[CFALocationID] = 'CFA'
,[SaveRemark] = ''
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
left join orders o WITH (NOLOCK) on o.id	= p2.orderid
left join Country c WITH (NOLOCK) on c.id=o.dest
outer apply(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID 
            from PackingList_Detail pd WITH (NOLOCK)
			where p2.ID = pd.ID
		) o1
		for xml path('')
	),1,1,'')
) o1
where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.CFAReceiveDate  is null
and p2.TransferCFADate is not null
and p2.DisposeFromClog= 0
and (po.Status ='New' or po.Status is null)
and ((p2.id='{sl[2].Substring(0, 13)}' and p2.CTNStartNo='{sl[2].Substring(13).TrimStart('^')}') or p2.SCICtnNo = '{sl[2].GetPackScanContent()}')
order by p2.ID,p2.CTNStartNo
";
                                if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                {
                                    dr["selected"] = 1;
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
                                    dr["CFALocationID"] = seekData["CFALocationID"];
                                    dr["remark"] = seekData["remark"];
                                    dr["SaveRemark"] = seekData["SaveRemark"];
                                    this.selectDataTable.Rows.Add(dr);
                                    insertCount++;
                                }
                                else
                                {
                                    sqlCmd = $@"
select distinct
[selected] = 1
,p2.ID
,p2.CTNStartNo
,o1.OrderID 
,o.CustPONo
,o.StyleID
,o.SeasonID
,o.BrandID
,c.Alias
,o.BuyerDelivery
,p2.remark
,p2.SCICtnNo
,[CFALocationID] = 'CFA'
,[SaveRemark] = ''
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
left join orders o WITH (NOLOCK) on o.id	= p2.orderid
left join Country c WITH (NOLOCK) on c.id=o.dest
outer apply(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID 
            from PackingList_Detail pd WITH (NOLOCK)
			where p2.ID = pd.ID
		) o1
		for xml path('')
	),1,1,'')
) o1
where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.CFAReceiveDate  is null
and p2.DisposeFromClog= 0
and p2.TransferCFADate is not null
and (po.Status ='New' or po.Status is null)
and p2.CustCTN='{sl[2]}'
order by p2.ID,p2.CTNStartNo
";
                                    if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                    {
                                        dr["selected"] = 1;
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
                                        dr["CFALocationID"] = seekData["CFALocationID"];
                                        dr["remark"] = seekData["remark"];
                                        dr["SaveRemark"] = seekData["SaveRemark"];
                                        this.selectDataTable.Rows.Add(dr);
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
,p2.ID
,p2.CTNStartNo
,o1.OrderID 
,o.CustPONo
,o.StyleID
,o.SeasonID
,o.BrandID
,c.Alias
,o.BuyerDelivery
,p2.remark
,p2.SCICtnNo
,[CFALocationID] = 'CFA'
,[SaveRemark] = ''
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
left join orders o WITH (NOLOCK) on o.id	= p2.orderid
left join Country c WITH (NOLOCK) on c.id=o.dest
outer apply(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID 
            from PackingList_Detail pd WITH (NOLOCK)
			where p2.ID = pd.ID
		) o1
		for xml path('')
	),1,1,'')
) o1
where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.CFAReceiveDate  is null
and p2.DisposeFromClog= 0
and p2.TransferCFADate is not null
and (po.Status ='New' or po.Status is null)
and p2.CustCTN='{sl[2]}'
order by p2.ID,p2.CTNStartNo
";
                               if (MyUtility.Check.Seek(sqlCmd, out seekData))
                                {
                                    dr["selected"] = 1;
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
                                    dr["CFALocationID"] = seekData["CFALocationID"];
                                    dr["remark"] = seekData["remark"];
                                    dr["SaveRemark"] = seekData["SaveRemark"];
                                    this.selectDataTable.Rows.Add(dr);
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
                        MyUtility.Msg.WarningBox("All data is not found !");
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

                this.CalcCTNQty();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 檢查是否有勾選資料
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                MyUtility.Msg.InfoBox("No data need to import!");
                return;
            }

            this.selectDataTable = dt.AsEnumerable().Where(r => MyUtility.Convert.GetInt(r["selected"]) == 1).ToList().CopyToDataTable();
            if (this.selectDataTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            this.ShowWaitMessage("Data Processing...");
            if (!this.backgroundDownloadSticker.IsBusy)
            {
                if (this.selectDataTable == null || this.selectDataTable.Rows.Count == 0)
                {
                    return;
                }

                this.progressBarProcessing.Maximum = this.selectDataTable.Rows.Count;

                // 先把UI介面鎖住
                this.SetInterfaceLocked(true);
                this.backgroundDownloadSticker.RunWorkerAsync();
            }

            this.HideWaitMessage();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CalcCTNQty()
        {
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataGridViewColumn column = this.grid.Columns["Selected"];
            if (!MyUtility.Check.Empty(column) && !MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                int selectCnt = this.grid.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["selected"].Value.ToString().Equals("1")).Count();
                this.numSelectQty.Value = selectCnt;
                this.numTTLCTNQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count;
            }
            else
            {
                this.numSelectQty.Value = 0;
                this.numTTLCTNQty.Value = 0;
            }
        }

        private void BackgroundDownloadSticker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                DataTable dt = this.selectDataTable;
                this.dtError = dt.Clone();
                StringBuilder warningmsg = new StringBuilder();
                this.backgroundDownloadSticker.ReportProgress(0);
                string sqlUpdate = string.Empty;

                this.progressCnt = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    warningmsg.Clear();
                    string checkPackSql = $@"
select pd.TransferCFADate, P.MDivisionID,pl.Status,pd.CFAReceiveDate
from PackingList_Detail pd WITH (NOLOCK)
inner join PackingList p (NOLOCK) on pd.id = p.id
left join pullout pl on p.PulloutID = pl.id
where pd.ID = '{dr["id"].ToString().Trim()}' 
and pd.CTNStartNo = '{dr["CTNStartNo"].ToString().Trim()}'
and pd.DisposeFromClog = 0
";
                    if (!MyUtility.Check.Seek(checkPackSql, null, out DataRow drPackResult))
                    {
                        warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> does not exist!" + Environment.NewLine);
                    }
                    else
                    {
                        if (MyUtility.Check.Empty(drPackResult["TransferCFADate"]))
                        {
                            warningmsg.Append($@"<CNT#: {dr["ID"]}{dr["CTNStartNo"]}> Not yet transferred to CFA!" + Environment.NewLine);
                        }
                        else if (!MyUtility.Check.Empty(drPackResult["CFAReceiveDate"]))
                        {
                            warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> This CTN# has been received!" + Environment.NewLine);
                        }
                        else if (drPackResult["Status"].ToString().Trim().ToUpper() == "CONFIRMED" || drPackResult["Status"].ToString().Trim().ToUpper() == "LOCKED")
                        {
                            warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> Already pullout!" + Environment.NewLine);
                        }
                        else if (drPackResult["MDivisionID"].ToString().ToUpper() != Env.User.Keyword.ToString().ToUpper())
                        {
                            warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> The order's M is not equal to login M." + Environment.NewLine);
                        }

                        // 代表都沒錯,可以單筆進行更新新增
                        else
                        {
                            sqlUpdate += $@"
update PackingList_Detail 
set CFAReceiveDate  = CONVERT(varchar(100), GETDATE(), 111), CFAInspDate  = CONVERT(varchar(100), GETDATE(), 111),
    CFALocationID='{dr["CFALocationID"].ToString().Trim()}', ClogLocationID = ''
where id='{dr["id"].ToString().Trim()}' and CTNStartNo='{dr["CTNStartNo"].ToString().Trim()}'
and DisposeFromClog= 0
";

                            string[] orderList = dr["OrderID"].ToString().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string order in orderList)
                            {
                                sqlUpdate += $@"
insert into CFAReceive(ReceiveDate,MDivisionID,OrderID,PackingListID,CTNStartNo,AddName,AddDate,SCICtnNo,CFALocationID)
values(CONVERT(varchar(100), GETDATE(), 111),'{Env.User.Keyword}','{order.Trim()}','{dr["ID"].ToString().Trim()}','{dr["CTNStartNo"].ToString().Trim()}','{Env.User.UserID}',GETDATE(),'{dr["SCICtnNo"].ToString()}','{dr["CFALocationID"].ToString()}')
;
";
                            }

                            DataTable selectOrdersData = null;
                            try
                            {
                                // 用Splitstring 是因為OrderID 有可能是多個用/組出來的
                                DBProxy.Current.Select(string.Empty,
$@"
select [Selected] = 1,[OrderID] = Data
from SplitString('{MyUtility.Convert.GetString(dr["OrderID"])}','/')
", out selectOrdersData);
                            }
                            catch (Exception ex)
                            {
                                e.Result = "Prepare update orders data fail!\r\n" + ex.ToString();
                            }

                            DualResult result1 = Ict.Result.True;

                            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 5, 0)))
                            {
                                try
                                {
                                    result1 = DBProxy.Current.Execute(null, sqlUpdate);

                                    if (result1 == false)
                                    {
                                        transactionScope.Dispose();
                                        e.Result = result1.ToString();
                                        this.backgroundDownloadSticker.ReportProgress(0);
                                        return;
                                    }

                                    DualResult prgResult = Prgs.UpdateOrdersCTN(selectOrdersData);

                                    if (prgResult == false)
                                    {
                                        transactionScope.Dispose();
                                        e.Result = prgResult.ToString();
                                        this.backgroundDownloadSticker.ReportProgress(0);
                                        return;
                                    }

                                    transactionScope.Complete();
                                    transactionScope.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    transactionScope.Dispose();
                                    e.Result = "Commit transaction error." + ex;
                                    this.backgroundDownloadSticker.ReportProgress(0);
                                    return;
                                }
                            }
                        }
                    }

                    if (warningmsg.ToString().Length > 0)
                    {
                        DataRow drError = this.dtError.NewRow();
                        dr["SaveRemark"] = warningmsg;
                        dr.CopyTo(drError);
                        this.dtError.Rows.Add(drError);
                    }

                    // 更新進度條
                    this.progressCnt++;
                    this.backgroundDownloadSticker.ReportProgress(this.progressCnt, string.Empty);
                }

                this.backgroundDownloadSticker.ReportProgress(0);
            }
            catch (Exception ex)
            {
                this.backgroundDownloadSticker.ReportProgress(0, ex.ToString());
                e.Result = ex.ToString();
            }

            this.backgroundDownloadSticker.ReportProgress(0);
        }

        private void BackgroundDownloadSticker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.progressBarProcessing.Value = e.ProgressPercentage;
            this.labProcessingBar.Text = $"{this.progressCnt}/{this.progressBarProcessing.Maximum}";
        }

        private void BackgroundDownloadSticker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            // 檢查是否有勾選資料
            this.listControlBindingSource1.EndEdit();

            // 使用Find撈出的全部資料
            DataTable dt =
                    (DataTable)this.listControlBindingSource1.DataSource;
            if (this.dtError.Rows.Count > 0)
            {
                MyUtility.Msg.WarningBox("Some carton cannot receive, please refer to field <Save Remark>.");

                if (dt.AsEnumerable().Any(row => !row["Selected"].EqualDecimal(1)))
                {
                    /*
                     沒勾選的放table #1
                     有錯誤的放table #2
                     再將2者合併一起, 畫面只會顯示沒勾的+有錯誤的
                     最後再將Selected清空
                     */

                    DataTable dtCopy = dt.AsEnumerable().Where(row => !row["Selected"].EqualDecimal(1)).CopyToDataTable();
                    dtCopy.Merge(this.dtError, true, MissingSchemaAction.AddWithKey);
                    foreach (DataRow dr in dtCopy.Rows)
                    {
                        if (MyUtility.Check.Empty(dr["Selected"]))
                        {
                            dr["SaveRemark"] = string.Empty;
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

                ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.Sort = @" Id, CTNSTartNo";
            }
            else if (e.Result != null)
            {
                MyUtility.Msg.WarningBox("error Msg: " + e.Result.ToString());
                this.listControlBindingSource1.DataSource = null;
            }
            else
            {
                if (dt.AsEnumerable().Any(row => !row["selected"].EqualDecimal(1)))
                 {
                    this.listControlBindingSource1.DataSource = dt.AsEnumerable().Where(row => !row["selected"].EqualDecimal(1)).CopyToDataTable();
                }
                else
                {
                    this.listControlBindingSource1.DataSource = null;
                }

                MyUtility.Msg.InfoBox("Complete!!");
            }

            this.CalcCTNQty();

            // 把UI介解除鎖定
            this.SetInterfaceLocked(false);
        }

        private void SetInterfaceLocked(bool isLocked)
        {
            // 鎖住或解鎖 UI 介面
            this.btnFind.Enabled = !isLocked;
            this.btnImportFromBarcode.Enabled = !isLocked;
            this.btnSave.Enabled = !isLocked;
            this.btnClose.Enabled = !isLocked;

            // 顯示WaitCursor
            Cursor.Current = isLocked ? Cursors.WaitCursor : Cursors.Default;
        }
    }
}
