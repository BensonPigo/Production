using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class P23 : Sci.Win.Tems.QueryForm
    {
        private DataTable dtDBSource;

        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            DataTable packingReason;

            DBProxy.Current.Select(null, "SELECT [Reason]=ID+'-'+Description  ,[ID]='' FROM PackingReason WHERE Type='FG' AND Junk=0  ORDEr BY ID ", out packingReason);

            MyUtility.Tool.SetupCombox(this.comboReason, 1, packingReason);
            this.comboReason.SelectedIndex = 0;

            DataGridViewGeneratorTextColumnSettings reasonSetting = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings reasonNameSetting = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings selectSetting = new DataGridViewGeneratorCheckBoxColumnSettings();

            selectSetting.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                if ((bool)e.FormattedValue == false)
                {
                    dr["FtyReqReturnReason"] = null;
                    dr["ReasonName"] = null;
                }

                dr["selected"] = e.FormattedValue;
                dr.EndEdit();
            };

            #region Reason事件
            reasonSetting.EditingMouseDown += (s, e) =>
            {
                if (this.listControlBindingSource.DataSource == null || e.Button != MouseButtons.Right)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                string sqlCmd = "SELECT ID,Description FROM PackingReason WHERE Type='FG' AND Junk=0 ";

                // DBProxy.Current.Select(null, sqlCmd, out dt);
                SelectItem item = new SelectItem(sqlCmd, "10,20", string.Empty);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["FtyReqReturnReason"] = item.GetSelectedString();
                dr["ReasonName"] = MyUtility.GetValue.Lookup($"SELECT Description FROM PackingReason WHERE Type='FG' AND Junk=0 AND ID ='{item.GetSelectedString()}'");
            };
            reasonSetting.CellValidating += (s, e) =>
             {
                 if (this.listControlBindingSource.DataSource == null)
                 {
                     return;
                 }

                 DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                 string oldvalue = MyUtility.Convert.GetString(dr["FtyReqReturnReason"]);
                 string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                 if (oldvalue == newvalue)
                 {
                     return;
                 }

                 if (MyUtility.Check.Empty(newvalue))
                 {
                     dr["FtyReqReturnReason"] = newvalue;
                     dr["ReasonName"] = null;
                     return;
                 }

                 List<SqlParameter> paras = new List<SqlParameter>();
                 paras.Add(new SqlParameter("@ID", newvalue));
                 bool exists = !MyUtility.Check.Empty(MyUtility.GetValue.Lookup($"SELECT ID FROM PackingReason WHERE Type='FG' AND Junk=0 AND ID=@ID", paras));

                 if (!exists)
                 {
                     MyUtility.Msg.WarningBox($"<Reason ID> : {newvalue} does not exists! ");
                     dr["FtyReqReturnReason"] = oldvalue;
                     return;
                 }
                 else
                 {
                     dr["FtyReqReturnReason"] = newvalue;
                     dr["ReasonName"] = MyUtility.GetValue.Lookup($"SELECT Description FROM PackingReason WHERE Type='FG' AND Junk=0 AND ID ='{newvalue}'");
                 }
             };
            #endregion

            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
               .CheckBox("selected", header: string.Empty, trueValue: 1, falseValue: 0, iseditable: true, settings: selectSetting)
               .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("SeasonID", header: "Season", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Article", header: "Colorway", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Text("Color", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("QtyPerCTN", header: "Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Alias", header: "Destination", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Date("SciDelivery", header: "SCI Delivery", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("ClogLocationID", header: "Location No", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("FtyReqReturnReason", header: "Reason", width: Widths.AnsiChars(15), settings: reasonSetting)
               .Text("ReasonName", header: "Reason Name", width: Widths.AnsiChars(15))
               .Date("FtyReqReturnDate", header: "Request Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Date("ReceiveDate", header: "Receive Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Date("ReturnDate", header: "Return Date", width: Widths.AnsiChars(13), iseditingreadonly: true);

            this.grid.Columns["FtyReqReturnReason"].DefaultCellStyle.BackColor = Color.Pink;

            for (int i = 0; i < this.grid.Columns.Count; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data Loading...");
            this.Find();
            this.HideWaitMessage();
        }

        private void Find()
        {
            string strSciDeliveryStart = this.dateRangeSCIDelivery.Value1.Empty() ? string.Empty : ((DateTime)this.dateRangeSCIDelivery.Value1).ToString("yyyy/MM/dd");
            string strSciDeliveryEnd = this.dateRangeSCIDelivery.Value2.Empty() ? string.Empty : ((DateTime)this.dateRangeSCIDelivery.Value2).ToString("yyyy/MM/dd");
            #region SqlParameter
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@OrderID", this.txtSPNo.Text));
            listSQLParameter.Add(new SqlParameter("@PoNo", this.txtPoNo.Text));
            listSQLParameter.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            listSQLParameter.Add(new SqlParameter("@SciDeliveryStart", strSciDeliveryStart));
            listSQLParameter.Add(new SqlParameter("@SciDeliveryEnd", strSciDeliveryEnd));
            #endregion

            #region SQL Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(strSciDeliveryStart)
                && !MyUtility.Check.Empty(strSciDeliveryEnd))
            {
                listSQLFilter.Add("AND o.SciDelivery between @SciDeliveryStart and @SciDeliveryEnd");
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                listSQLFilter.Add("AND pd.OrderID = @OrderID");
            }

            if (!MyUtility.Check.Empty(this.txtPoNo.Text))
            {
                listSQLFilter.Add("AND o.CustPoNo= @PoNo");
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                listSQLFilter.Add("AND pd.id= @PackID");
            }
            #endregion

            #region Sql Command

            string strCmd = $@"
SELECT  
	[Selected]=IIF(pd.FtyReqReturnDate IS NULL ,0,1)
	,pd.ID
	,PD.CTNStartNo
	,orders.OrderID
	,o.CustPoNo
	,o.StyleID
	,o.SeasonID
	,o.BrandID
	,pd.Article
	,pd.Color
	,size.SizeCode
	,qty.QtyPerCTN
	,c.Alias
	,o.BuyerDelivery
	,pd.ClogLocationID
	,pd.Remark
	,pd.FtyReqReturnReason
    ,[ReasonName]=pr.Description
	,pd.FtyReqReturnDate
	,pd.ReceiveDate
	,pd.ReturnDate
    ,o.SciDelivery
    ,pd.Ukey

FROM PackingList_Detail pd
INNER JOIN PackingList p WITH (NOLOCK) on p.id=pd.id
LEFT JOIN Pullout po WITH (NOLOCK) on po.ID=p.PulloutID
LEFT JOIN Orders o ON o.ID= pd.OrderID
INNER JOIN Country c ON c.ID=o.Dest 
LEFT JOIN PackingReason pr ON pr.Type='FG' AND pr.ID=pd.FtyReqReturnReason
----------
OUTER APPLY(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID from PackingList_Detail p WITH (NOLOCK)
			where p.orderid=pd.orderid and p.DisposeFromClog= 0
		) o1
		for xml path('')
	),1,1,'')
) orders
-----------
OUTER APPLY(
	select sizecode = stuff((
		select concat('/',Sizecode)
		from (
			select distinct sizecode 
			from PackingList_Detail p WITH (NOLOCK)
			where p.id=pd.id AND p.CTNStartNo = pd.CTNStartNo			
            and p.DisposeFromClog= 0
		) s
		outer apply (
			select seq from Order_SizeCode WITH (NOLOCK)
			where sizecode = s.sizecode and id=o.poid
		)s2
		order by s2.Seq 
		for xml path('')
	),1,1,'')
) size
-------------
OUTER APPLY(
	select QtyPerCTN = stuff((
		select concat('/',QtyPerCTN)
		from (
			select distinct QtyPerCTN,sizecode 
			from PackingList_Detail p WITH (NOLOCK)
			where p.id=pd.id and p.CTNStartNo = pd.CTNStartNo			
            and p.DisposeFromClog= 0
		) q
		outer apply (
			select seq from Order_SizeCode WITH (NOLOCK)
			where sizecode = q.sizecode and id=o.poid
		)s2
		order by s2.Seq 
		for xml path('')
	),1,1,'')
) qty
-------------
WHERE pd.CTNStartNo<>''
     AND p.Mdivisionid='{Sci.Env.User.Keyword}'
     AND p.Type in ('B','L')
     AND pd.ReceiveDate is not null
     AND pd.DisposeFromClog= 0
     AND pd.TransferCFADate is null
     AND (po.Status ='New' or po.Status IS NULL)
     AND pd.CTNQty=1
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}

order by pd.ID,pd.Seq";
            #endregion

            DualResult result = DBProxy.Current.Select(string.Empty, strCmd, listSQLParameter, out this.dtDBSource);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (this.dtDBSource.Rows.Count < 1)
            {
                this.listControlBindingSource.DataSource = null;
                MyUtility.Msg.InfoBox("Data not found !");
            }
            else
            {
                DataTable dt = this.dtDBSource.Copy();
                this.listControlBindingSource.DataSource = dt;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource.DataSource == null)
            {
                return;
            }

            #region 宣告
            DataTable dt = (DataTable)this.listControlBindingSource.DataSource;
            string updateSqlCmd = string.Empty;
            string insertLog = string.Empty;
            DualResult resule;

            // 用於寫入FtyReqReturnClog的清單
            List<string> orderIds = new List<string>();
            Dictionary<string, Dictionary<string, string>> ins_FtyReqReturnClog_List = new Dictionary<string, Dictionary<string, string>>();
            #endregion

            this.ShowWaitMessage("Data Processing ...");

            try
            {
                #region 先針對有勾選驗證

                DataRow[] selectedRows_NoReason = dt.Select("selected=1 AND FtyReqReturnReason='' ");

                if (selectedRows_NoReason.Length > 0)
                {
                    MyUtility.Msg.WarningBox($"<Pack ID:{selectedRows_NoReason[0]["ID"]} ,CTN#:{selectedRows_NoReason[0]["CTNStartNo"]}> Reason can’t empty!!");
                    this.HideWaitMessage();
                    return;
                }
                #endregion

                #region 比對DB和Form的資料，看誰有被異動

                DataTable selectData = null;
                MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, @"
            --select distinct a.* 
            --FROM #tmp a
            --INNER JOIN PackingList_Detail b on a.id=b.id and a.ctnstartno=b.ctnstartno
            --where a.CFANeedInsp <> b.CFANeedInsp 
SELECT DISTINCT a.* 
FROM #tmp a
INNER JOIN PackingList_Detail b ON a.Ukey=b.Ukey and b.DisposeFromClog= 0
WHERE (b.FtyReqReturnDate IS NULL AND a.Selected = 1)   --若FtyReqReturnDate IS NULL，表示不應該勾選，但在FORM上的被勾選了   = 有異動
OR (b.FtyReqReturnDate IS NOT NULL AND a.Selected = 0)  --若FtyReqReturnDate IS NOT NULL，表示應該勾選，但在FORM上的沒勾選了 = 有異動
OR a.FtyReqReturnReason <> b.FtyReqReturnReason  --Reason異動
", out selectData);

                if (selectData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox($"No data changes!!");
                    this.HideWaitMessage();
                    return;
                }

                #endregion

                #region 開始組成SQL

                int identity = 0;
                foreach (DataRow dr in selectData.Rows)
                {
                    bool selected = MyUtility.Convert.GetInt(dr["Selected"]) == 1;
                    string ftyReqReturnDate = selected ? "GETDATE()" : "NULL";

                    updateSqlCmd = updateSqlCmd + $@"
UPDATE PackingList_Detail 
SET FtyReqReturnDate = {ftyReqReturnDate},FtyReqReturnReason='{dr["FtyReqReturnReason"]}'
WHERE id='{dr["id"]}' AND CTNStartNo ='{dr["CTNStartNo"]}' and DisposeFromClog= 0;
" + Environment.NewLine;

                    // 現在這個表身要拆開看，變成OrderID為表頭，該OrderID所在的表身為表身（後面以內表身稱之）
                    // 所以，若表身一筆資料，有2個不同的OrderID，則寫入FtyReqReturnClog 時會寫入兩筆，這兩筆的FtyReqReturnClog.RequestID 會不一樣，但其他欄位一模一樣

                    // 1. 開始整理內表身的資料，因為會一樣，所以不用跑回圈
                    Dictionary<string, string> data_FtyReqReturnClog = new Dictionary<string, string>();

                    data_FtyReqReturnClog.Add("Reason", dr["FtyReqReturnReason"].ToString());
                    data_FtyReqReturnClog.Add("PackingListID", dr["ID"].ToString());
                    data_FtyReqReturnClog.Add("CTNStartNo", dr["CTNStartNo"].ToString());
                    data_FtyReqReturnClog.Add("AddName", Sci.Env.User.UserID);
                    data_FtyReqReturnClog.Add("MDivisionID", Sci.Env.User.Keyword);

                    // SP#如有多筆則寫入PackingList_Detail.CTNQty=1的那筆
                    string[] arry_Orderid = dr["OrderID"].ToString().Split(',');

                    if (arry_Orderid.Length > 1)
                    {
                        for (int i = 0; i <= arry_Orderid.Length - 1; i++)
                        {
                            string spNo = MyUtility.GetValue.Lookup($"SELECT DISTINCT OrderID FROM PackingList_Detail WHERE OrderID = '{arry_Orderid[i]}' AND CTNQty=1 and DisposeFromClog= 0");
                            if (!MyUtility.Check.Empty(spNo))
                            {
                                data_FtyReqReturnClog.Add("OrderID", spNo);
                                break;
                            }
                        }
                    }
                    else
                    {
                        data_FtyReqReturnClog.Add("OrderID", dr["OrderID"].ToString());
                    }

                    // 2. 會不一樣的只有表頭的OrderID，因此在這裡跑
                    for (int i = 0; i <= arry_Orderid.Length - 1; i++)
                    {
                        if (!orderIds.Contains(arry_Orderid[i]))
                        {
                            orderIds.Add(arry_Orderid[i]);
                        }

                        ins_FtyReqReturnClog_List.Add(arry_Orderid[i] + "_" + identity.ToString(), data_FtyReqReturnClog);
                        identity++;
                    }

                    // 結論：若一個表身有多個SP#，則FtyReqReturnClog會寫入多筆RequestID不同，其他皆相同的資料
                }

                string[] keyArry = ins_FtyReqReturnClog_List.Keys.ToArray();
                string[] requestID_List = MyUtility.GetValue.GetBatchID(Sci.Env.User.Factory + "FG", "FtyReqReturnClog", DateTime.Now, 2, "RequestID", batchNumber: orderIds.Count).ToArray();

                int index = 0;
                foreach (string orderid in orderIds)
                {
                    foreach (string key in keyArry)
                    {
                        if (key.Contains(orderid + "_"))
                        {
                            Dictionary<string, string> singleRow = ins_FtyReqReturnClog_List[key];
                            insertLog = insertLog + $@"
                        INSERT INTO [FtyReqReturnClog]
                                   ([RequestID]
                                   ,[RequestDate]
                                   ,[Reason]
                                   ,[MDivisionID]
                                   ,[OrderID]
                                   ,[PackingListID]
                                   ,[CTNStartNo]
                                   ,[AddName]
                                   ,[AddDate])
                             VALUES
                                   ('{requestID_List[index]}'
                                   ,GETDATE()
                                   ,'{singleRow["Reason"]}'
                                   ,'{singleRow["MDivisionID"]}'
                                   ,'{singleRow["OrderID"]}'
                                   ,'{singleRow["PackingListID"]}'
                                   ,'{singleRow["CTNStartNo"]}'
                                   ,'{singleRow["AddName"]}'
                                   ,GETDATE() );" + Environment.NewLine;
                        }
                    }

                    index++;
                }

                #endregion

                #region 執行SQL

                if (updateSqlCmd.Length > 0 && insertLog.Length > 0)
                {
                    using (TransactionScope transactionscope = new TransactionScope())
                    {
                        if (!(resule = DBProxy.Current.Execute(null, updateSqlCmd.ToString())))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(updateSqlCmd.ToString(), resule);
                            return;
                        }

                        if (!(resule = DBProxy.Current.Execute(null, insertLog.ToString())))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(insertLog.ToString(), resule);
                            return;
                        }

                        transactionscope.Complete();
                        transactionscope.Dispose();
                        MyUtility.Msg.InfoBox("Save successful!");
                    }
                }

                #endregion

                this.Find();
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
            finally
            {
                this.HideWaitMessage();
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClickPrint();
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        protected override bool ClickPrint()
        {
            if (this.listControlBindingSource.DataSource == null)
            {
                return false;
            }

            #region 宣告
            DataTable dt = (DataTable)this.listControlBindingSource.DataSource;
            #endregion

            this.ShowWaitMessage("Data Processing ...");

            // 確認有打勾的選項
            DataRow[] selectedRows = dt.Select("selected=1");

            if (selectedRows.Length == 0)
            {
                MyUtility.Msg.WarningBox($"Please choose request return cartons and click save first!!");
                this.HideWaitMessage();
                return false;
            }

            #region 比對DB和Form的資料，檢驗是否有異動 且 沒存檔

            DataTable selectData = null;
            MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, @"
SELECT DISTINCT a.* 
FROM #tmp a
INNER JOIN PackingList_Detail b ON a.Ukey=b.Ukey and b.DisposeFromClog= 0
WHERE (b.FtyReqReturnDate IS NULL AND a.Selected = 1)   --若FtyReqReturnDate IS NULL，表示不應該勾選，但在FORM上的被勾選了   = 有異動
OR (b.FtyReqReturnDate IS NOT NULL AND a.Selected = 0)  --若FtyReqReturnDate IS NOT NULL，表示應該勾選，但在FORM上的沒勾選了 = 有異動
", out selectData);

            // 有異動，但沒click Save
            if (selectData.Rows.Count > 0)
            {
                MyUtility.Msg.WarningBox($"Please click save first!!");
                this.HideWaitMessage();
                return false;
            }
            #endregion

            List<string> orderidList = new List<string>();

            // 將Form的資料整理成報表需要的樣子

            // 1.OrderID 清單，一個Order ID就會有一個Sheet
            foreach (DataRow dr in selectedRows)
            {
                string[] arry_Orderid = dr["OrderID"].ToString().Split(',');

                for (int i = 0; i <= arry_Orderid.Length - 1; i++)
                {
                    if (!orderidList.Contains(arry_Orderid[i]))
                    {
                        orderidList.Add(arry_Orderid[i]);
                    }
                }
            }

            Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Packing_P23.xltx");

            // 幾個OrderId就Copy幾次Sheet
            x1.CopySheet.Add(1, orderidList.Count - 1);

            // 一個Sheet是做一個DataTable，ControlNo是Key值，因此 SheetName 跟著ControlNo變化
            x1.VarToSheetName = "##ControlNo";

            // 2.根據OrderId 塞對應的資料，XltRptTable可以塞多個DataTable，但是##參數要給Index
            int index = 0;
            foreach (var orderId in orderidList)
            {
                string indexStr = index.ToString();

                DataTable sameOrderIdRow = selectedRows.CopyToDataTable().AsEnumerable().Where(o => o["OrderId"].ToString() == orderId).CopyToDataTable();

                // 唯一會不同的只有CTNStartNo
                int cntCount = sameOrderIdRow.Rows.Count;

                // 定義最後用來塞Excel的DataTable
                DataTable finalData = new DataTable();
                finalData.Columns.Add("ControlNo", typeof(string));
                finalData.Columns.Add("SP", typeof(string));
                finalData.Columns.Add("Line", typeof(string));
                finalData.Columns.Add("DeliveryDate", typeof(string));
                finalData.Columns.Add("Name", typeof(string));
                finalData.Columns.Add("Date", typeof(string));
                finalData.Columns.Add("Factory", typeof(string));
                finalData.Columns.Add("Total", typeof(string));
                finalData.Columns.Add("PO", typeof(string));
                finalData.Columns.Add("Style", typeof(string));
                finalData.Columns.Add("Boxof", typeof(string));
                finalData.Columns.Add("Destination", typeof(string));
                for (int i = 0; i <= cntCount - 1; i++)
                {
                    finalData.Columns.Add("CTNStartNo_" + (i + 1).ToString(), typeof(string));
                }

                DataRow nRow = finalData.NewRow();

                nRow["ControlNo"] = MyUtility.GetValue.Lookup($"SELECT MAX(RequestID)FROM FtyReqReturnClog WHERE OrderID='{orderId}'");
                nRow["SP"] = orderId;
                nRow["Line"] = MyUtility.GetValue.Lookup($"SELECT SewLine FROM Orders WHERE ID='{orderId}'");
                nRow["DeliveryDate"] = Convert.ToDateTime(sameOrderIdRow.Rows[0]["BuyerDelivery"]).ToShortDateString();
                nRow["Name"] = MyUtility.GetValue.Lookup($"SELECT dbo.getPass1_ExtNo('{Sci.Env.User.UserID}')");
                nRow["Date"] = DateTime.Now.ToString("yyyy/mm/dd");
                nRow["Factory"] = MyUtility.GetValue.Lookup($"SELECT FtyGroup FROM Orders WHERE ID='{orderId}'");
                nRow["Total"] = cntCount;
                nRow["PO"] = sameOrderIdRow.Rows[0]["CustPONo"].ToString();
                nRow["Style"] = sameOrderIdRow.Rows[0]["StyleID"].ToString();
                nRow["Boxof"] = MyUtility.GetValue.Lookup($"SELECT TotalCTN FROM Orders WHERE ID='{orderId}'");
                nRow["Destination"] = sameOrderIdRow.Rows[0]["Alias"].ToString();

                for (int i = 0; i <= cntCount - 1; i++)
                {
                    nRow["CTNStartNo_" + (i + 1).ToString()] = sameOrderIdRow.Rows[i]["CTNStartNo"].ToString();
                }

                finalData.Rows.Add(nRow);

                Sci.Utility.Excel.SaveXltReportCls.XltRptTable xDt = new Sci.Utility.Excel.SaveXltReportCls.XltRptTable(finalData);
                xDt.ShowHeader = false;

                x1.DicDatas.Add("##ControlNo" + indexStr, finalData.Rows[0]["ControlNo"]);
                x1.DicDatas.Add("##SP" + indexStr, finalData.Rows[0]["SP"]);
                x1.DicDatas.Add("##Line" + indexStr, finalData.Rows[0]["Line"]);
                x1.DicDatas.Add("##DeliveryDate" + indexStr, Convert.ToDateTime(finalData.Rows[0]["DeliveryDate"]).ToShortDateString());
                x1.DicDatas.Add("##Name" + indexStr, finalData.Rows[0]["Name"]);
                x1.DicDatas.Add("##Date" + indexStr, finalData.Rows[0]["Date"]);
                x1.DicDatas.Add("##Factory" + indexStr, finalData.Rows[0]["Factory"]);
                x1.DicDatas.Add("##Total" + indexStr, finalData.Rows[0]["Total"]);
                x1.DicDatas.Add("##PO" + indexStr, finalData.Rows[0]["PO"]);
                x1.DicDatas.Add("##Style" + indexStr, finalData.Rows[0]["Style"]);
                x1.DicDatas.Add("##Boxof" + indexStr, finalData.Rows[0]["Boxof"]);
                x1.DicDatas.Add("##Destination" + indexStr, finalData.Rows[0]["Destination"]);

                // 填入CTNStartNo
                for (int i = 0; i <= cntCount - 1; i++)
                {
                    x1.DicDatas.Add("##CTNStartNo_" + (i + 1).ToString() + indexStr, finalData.Rows[0]["CTNStartNo_" + (i + 1).ToString()]);
                }

                index++;
            }

            x1.Save(Sci.Production.Class.MicrosoftFile.GetName("Packing_P23"));

            this.HideWaitMessage();
            return true;
        }

        private void BtnColse_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnUpdateAllReason_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource.DataSource == null)
            {
                return;
            }

            #region 宣告
            DataTable dt = (DataTable)this.listControlBindingSource.DataSource;
            string reasonId = string.Empty;
            string reasonName = string.Empty;
            #endregion

            try
            {
                this.ShowWaitMessage("Data Processing ...");

                // 確認有打勾的選項
                DataRow[] selectedRows = dt.Select("selected=1");

                if (selectedRows.Length == 0)
                {
                    MyUtility.Msg.WarningBox($"Please select data first!");
                    return;
                }

                if (!MyUtility.Check.Empty(this.comboReason.Text))
                {
                    reasonId = this.comboReason.Text.Split('-')[0];
                    reasonName = this.comboReason.Text.Split('-')[1];
                }

                foreach (DataRow row in dt.Rows)
                {
                    if (row["selected"].ToString() == "1")
                    {
                        row["FtyReqReturnReason"] = reasonId;
                        row["ReasonName"] = reasonName;
                    }
                }

                this.listControlBindingSource.DataSource = dt;
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
            finally
            {
                this.HideWaitMessage();
            }
        }

        private void grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.listControlBindingSource.DataSource == null || e.ColumnIndex != 0)
            {
                return;
            }

            #region 宣告
            DataTable dt = (DataTable)this.listControlBindingSource.DataSource;
            #endregion
            DataRow[] selectedRows = dt.Select("selected=1 ");
            DataRow[] selectedRows_withReason = dt.Select("selected=1 AND FtyReqReturnReason <>'' ");
            if (selectedRows.Length > 0)
            {
                if (selectedRows_withReason.Length > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        dt.Rows[i]["FtyReqReturnReason"] = null;
                        dt.Rows[i]["ReasonName"] = null;
                        dt.Rows[i]["selected"] = 0;
                    }
                }

                // else
                // {
                //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                //    {
                //        dt.Rows[i]["FtyReqReturnReason"] = null;
                //        dt.Rows[i]["ReasonName"] = null;
                //        dt.Rows[i]["selected"] = 1;
                //    }
                // }
            }
            else
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    dt.Rows[i]["FtyReqReturnReason"] = null;
                    dt.Rows[i]["ReasonName"] = null;
                    dt.Rows[i]["selected"] = 0;
                }
            }

            this.grid.ValidateControl();
        }
    }
}
