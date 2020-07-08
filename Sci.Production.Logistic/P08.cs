using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P09
    /// </summary>
    public partial class P08 : Sci.Win.Tems.QueryForm
    {
        private string selectDataTable_DefaultView_Sort = string.Empty;
        private DataTable selectDataTable;

        /// <summary>
        /// P09
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P08(ToolStripMenuItem menuitem)
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
            TextBox a = new TextBox();
            a.Text = Sci.Env.User.Keyword;
            this.txtcloglocationLocationNo.MDivisionObjectName = a;

            DataGridViewGeneratorTextColumnSettings clogLocation = CellClogLocation.GetGridCell(Sci.Env.User.Keyword);
            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                 .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                 .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
                 .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("ClogLocationID", header: "Location No", width: Widths.Auto(true), settings: clogLocation)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.grid.Columns["ClogLocationID"].DefaultCellStyle.BackColor = Color.Pink;
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
                listSQLFilter.Add("and p2.CFAReturnClogDate between @TransferStart and @TransferEnd");
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

            int intChkUpdateOriLocation = 0;
            if (this.chkUpdateOriLocation.Checked)
            {
                intChkUpdateOriLocation = 1;
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
,[ClogLocationID] = iif(1= {intChkUpdateOriLocation},ToCfa.OrigloactionID, '')
,p2.remark
,p2.SCICtnNo
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
outer apply (
	select top 1 OrigloactionID from TransferToCFA
	where PackingListID=p2.ID
	and CTNStartNo=p2.CTNStartNo
	order by AddDate desc
)ToCfa
where p2.CTNStartNo<>''
and p1.Mdivisionid='{Sci.Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.DisposeFromClog= 0
and p2.CFAReturnClogDate is not null
and p2.ClogReceiveCFADate is null
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
b.CTNStartNo, c.CustPONo, c.StyleID, c.SeasonID, c.BrandID, d.Alias, c.BuyerDelivery, b.ClogLocationID, b.Remark ,b.SCICtnNo
from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) , Country d WITH (NOLOCK) where 1=0";
                int intChkUpdateOriLocation = 0;
                if (this.chkUpdateOriLocation.Checked)
                {
                    intChkUpdateOriLocation = 1;
                }

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
,[ClogLocationID] = iif(1= {intChkUpdateOriLocation},ToCfa.OrigloactionID, cl.ID)
,p2.remark
,p2.SCICtnNo
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
outer apply (select ID from  ClogLocation WITH (NOLOCK) where ID = '{sl[1]}' and MDivisionID ='{Sci.Env.User.Keyword}') as cl
outer apply (
	select top 1 OrigloactionID from TransferToCFA
	where PackingListID=p2.ID
	and CTNStartNo=p2.CTNStartNo
	order by AddDate desc
)ToCfa
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
and p1.Mdivisionid='{Sci.Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.CFAReturnClogDate is not null
and p2.DisposeFromClog= 0
and p2.ClogReceiveCFADate is null
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
                                    dr["ClogLocationID"] = seekData["ClogLocationID"];
                                    dr["remark"] = seekData["remark"];
                                    this.selectDataTable.Rows.Add(dr);
                                    insertCount++;
                                }
                                else
                                {
                                    sqlCmd = $@"
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
,[ClogLocationID] = iif(1= {intChkUpdateOriLocation},ToCfa.OrigloactionID, cl.ID)
,p2.remark
,p2.SCICtnNo
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
outer apply (select ID from  ClogLocation WITH (NOLOCK) where ID = '{sl[1]}' and MDivisionID ='{Sci.Env.User.Keyword}') as cl
outer apply (
	select top 1 OrigloactionID from TransferToCFA
	where PackingListID=p2.ID
	and CTNStartNo=p2.CTNStartNo
	order by AddDate desc
)ToCfa
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
and p1.Mdivisionid='{Sci.Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.CFAReturnClogDate is not null
and p2.DisposeFromClog= 0
and p2.ClogReceiveCFADate is null
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
                                        dr["ClogLocationID"] = seekData["ClogLocationID"];
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
,[ClogLocationID] = iif(1= {intChkUpdateOriLocation},ToCfa.OrigloactionID, cl.ID)
,p2.remark
,p2.SCICtnNo
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
outer apply (select ID from  ClogLocation WITH (NOLOCK) where ID = '{sl[1]}' and MDivisionID ='{Sci.Env.User.Keyword}') as cl
outer apply (
	select top 1 OrigloactionID from TransferToCFA
	where PackingListID=p2.ID
	and CTNStartNo=p2.CTNStartNo
	order by AddDate desc
)ToCfa
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
and p1.Mdivisionid='{Sci.Env.User.Keyword}'
--and p1.Type in ('B','L')
--and p2.CFAReturnClogDate is not null
--and p2.ClogReceiveCFADate is null
--and (po.Status ='New' or po.Status is null)
and p2.CustCTN='{sl[2]}'
and p2.DisposeFromClog= 0
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
                                    dr["ClogLocationID"] = seekData["ClogLocationID"];
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
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private void BtnSave_Click(object sender, EventArgs e)
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
select p.Status ,p2.CFAReturnClogDate,p2.ClogReceiveCFADate
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
                    if (MyUtility.Check.Empty(drSelect["CFAReturnClogDate"]))
                    {
                        warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> not yet returned to Clog!" + Environment.NewLine);
                    }
                    else if (drSelect["Status"].ToString().Trim().ToUpper() == "CONFIRMED" || drSelect["Status"].ToString().Trim().ToUpper() == "LOCKED")
                    {
                        warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> Already pullout!" + Environment.NewLine);
                    }
                    else if (!MyUtility.Check.Empty(drSelect["ClogReceiveCFADate"]))
                    {
                        warningmsg.Append($@"<CNT#: {dr["id"]}{dr["CTNStartNo"]}> This CTN# has been received." + Environment.NewLine);
                    }
                    else
                    {
                        updateCmds.Add($@"
update PackingList_Detail 
set ClogReceiveCFADate = CONVERT(varchar(100), GETDATE(), 111)
, CFAReturnClogDate = null , ClogLocationID = '{dr["ClogLocationID"]}'
, CFALocationId=null
where id='{dr["id"].ToString().Trim()}' and CTNStartNo='{dr["CTNStartNo"].ToString().Trim()}' 
and DisposeFromClog= 0
");

                        insertCmds.Add($@"
insert into ClogReceiveCFA ( ReceiveDate,MDivisionID,OrderID,PackingListID,CTNStartNo,AddName,AddDate,SCICtnNo)
values(CONVERT(varchar(100), GETDATE(), 111),'{Sci.Env.User.Keyword}','{dr["OrderID"].ToString().Trim()}','{dr["ID"].ToString().Trim()}','{dr["CTNStartNo"].ToString().Trim()}','{Sci.Env.User.UserID}',GETDATE(),'{dr["SCICtnNo"].ToString()}')
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

                    if (updateCmds.Count > 0 && insertCmds.Count > 0)
                    {
                        DualResult prgResult = Prgs.UpdateOrdersCTN(selectData);

                        if (result1 && result2 && prgResult)
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

            if (warningmsg.ToString().Length > 0)
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
            }

            this.HideWaitMessage();
        }

        private void BtnUpdateAll_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            string location = this.txtcloglocationLocationNo.Text.Trim();
            int pos = this.listControlBindingSource1.Position;     // 記錄目前指標位置
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first!");
                return;
            }

            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            foreach (DataRow currentRecord in selectedData)
            {
                currentRecord["ClogLocationId"] = location;
                currentRecord.EndEdit();
            }

            this.grid.SuspendLayout();
            this.listControlBindingSource1.Position = pos;
            this.grid.DataSource = null;
            this.grid.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource1.Position = pos;
            this.grid.ResumeLayout();
        }

        private void chkUpdateOriLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (this.grid.RowCount == 0 || this.grid == null)
            {
                return;
            }

            this.Find();
        }
    }
}
