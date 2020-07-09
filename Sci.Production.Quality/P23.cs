using Ict;
using Ict.Win;
using Sci.Data;
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
    public partial class P23 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private string selectDataTable_DefaultView_Sort = string.Empty;
        private DataTable selectDataTable;

        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

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
                    this.calcCTNQty();
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
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: true);

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
            this.calcCTNQty();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private void btnImportFromBarcode_Click(object sender, EventArgs e)
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
where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.CFAReceiveDate  is null
and p2.TransferCFADate is not null
and p2.DisposeFromClog= 0
and (po.Status ='New' or po.Status is null)
and p2.id='{sl[2].Substring(0, 13)}'
and p2.CTNStartNo='{sl[2].Substring(13, sl[2].Length - 13)}'
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

                this.calcCTNQty();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 檢查是否有勾選資料
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            this.ShowWaitMessage("Data Processing...");
            DataRow drSelect;

            StringBuilder warningmsg = new StringBuilder();
            IList<string> insertCmds = new List<string>();
            IList<string> updateCmds = new List<string>();
            foreach (DataRow dr in selectedData)
            {
                if (!MyUtility.Check.Seek(
                    $@"
select p2.TransferCFADate ,p.Status ,p2.CFAReceiveDate
from PackingList_detail p2
inner join PackingList p1 on p2.id=p1.id
left join pullout p on p1.PulloutID = p.id
where p2.id='{dr["id"].ToString().Trim()}' 
and p2.CTNStartNo='{dr["CTNStartNo"].ToString().Trim()}' and p2.DisposeFromClog= 0", out drSelect))
                {
                    warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> does not exist!" + Environment.NewLine);
                    continue;
                }
                else
                {
                    if (MyUtility.Check.Empty(drSelect["TransferCFADate"]))
                    {
                        warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> Not yet transferred to CFA!" + Environment.NewLine);
                    }
                    else if (!MyUtility.Check.Empty(drSelect["CFAReceiveDate"]))
                    {
                        warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> This CTN# has been received!" + Environment.NewLine);
                    }
                    else if (drSelect["Status"].ToString().Trim().ToUpper() == "CONFIRMED" || drSelect["Status"].ToString().Trim().ToUpper() == "LOCKED")
                    {
                        warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> Already pullout!" + Environment.NewLine);
                    }
                    else
                    {
                        updateCmds.Add($@"
update PackingList_Detail 
set CFAReceiveDate  = CONVERT(varchar(100), GETDATE(), 111), CFAInspDate  = CONVERT(varchar(100), GETDATE(), 111),
    CFALocationID='{dr["CFALocationID"].ToString().Trim()}', ClogLocationID = ''
where id='{dr["id"].ToString().Trim()}' and CTNStartNo='{dr["CTNStartNo"].ToString().Trim()}'
and DisposeFromClog= 0
");
                        insertCmds.Add($@"
insert into CFAReceive(ReceiveDate,MDivisionID,OrderID,PackingListID,CTNStartNo,AddName,AddDate,SCICtnNo,CFALocationID)
values(CONVERT(varchar(100), GETDATE(), 111),'{Env.User.Keyword}','{dr["OrderID"].ToString().Trim()}','{dr["ID"].ToString().Trim()}','{dr["CTNStartNo"].ToString().Trim()}','{Env.User.UserID}',GETDATE(),'{dr["SCICtnNo"].ToString()}','{dr["CFALocationID"].ToString()}')
");
                    }
                }
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

                    if (updateCmds.Count > 0 && insertCmds.Count > 0)
                    {
                        DualResult prgResult = Prgs.UpdateOrdersCTN(selectData);

                        if (result1 && result2 && prgResult)
                        {
                            transactionScope.Complete();
                            transactionScope.Dispose();
                            MyUtility.Msg.InfoBox("Complete!!");
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

            if (warningmsg.ToString().Length > 0)
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
            }

            if (dt.AsEnumerable().Any(row => !row["Selected"].EqualDecimal(1)))
            {
                this.listControlBindingSource1.DataSource = dt.AsEnumerable().Where(row => !row["Selected"].EqualDecimal(1)).CopyToDataTable();
            }
            else
            {
                this.listControlBindingSource1.DataSource = null;
            }

            this.HideWaitMessage();
            this.calcCTNQty();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void calcCTNQty()
        {
            if (this.listControlBindingSource1.DataSource != null)
            {
                this.grid.ValidateControl();
                this.numTTLCTNQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count;
                this.numSelectQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").Length;
            }
        }
    }
}
