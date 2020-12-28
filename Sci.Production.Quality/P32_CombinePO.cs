using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P32_CombinePO : Win.Tems.QueryForm
    {
        private string CFAInspectionRecordID;

        private DataTable MasterCFAInspectionRecord_OrderSEQ;
        private DataRow MasterCFAInspectionRecord;

        /// <inheritdoc/>
        public P32_CombinePO(bool canedit, string cFAInspectionRecordID, DataTable cFAInspectionRecord_OrderSEQ, DataRow cFAInspectionRecord)
        {
            this.InitializeComponent();
            this.CFAInspectionRecordID = cFAInspectionRecordID;
            this.MasterCFAInspectionRecord_OrderSEQ = cFAInspectionRecord_OrderSEQ;
            this.MasterCFAInspectionRecord = cFAInspectionRecord;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = false;

            DataGridViewGeneratorTextColumnSettings col_OrderID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Seq = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Carton = new DataGridViewGeneratorTextColumnSettings();

            #region col_OrderID
            col_OrderID.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow selectedRow = this.grid.GetDataRow(e.RowIndex);
                    string orderID = e.FormattedValue.ToString();
                    string seq = MyUtility.Convert.GetString(selectedRow["Seq"]);
                    string stage = MyUtility.Convert.GetString(this.MasterCFAInspectionRecord["stage"]);
                    string where = string.Empty;
                    if (MyUtility.Check.Empty(orderID))
                    {
                        return;
                    }

                    List<SqlParameter> paras = new List<SqlParameter>
                    {
                        new SqlParameter("@ID", orderID),
                    };

                    if (stage.ToUpper() == "3RD PARTY")
                    {
                        where = $@"AND EXISTS(
    SELECT 1
    FROM Order_QtyShip oq WITH(NOLOCK)
    WHERE oq.ID = o.ID AND oq.CFAIs3rdInspect = 1
)
";
                    }

                    #region OrderID檢查
                    bool exists = MyUtility.Check.Seek(
                        $@"
SELECT 1 
FROM Orders o
WHERE o.ID=@ID 
AND o.Finished = 0
AND o.Category IN('B', 'S', 'G')
{where}
",
                        paras);
                    if (!exists)
                    {
                        MyUtility.Msg.InfoBox("Data not found!!");
                        e.Cancel = true;
                        return;
                    }

                    bool isSameM = MyUtility.Check.Seek($"SELECT 1 FROM Orders WHERE ID=@ID AND FtyGroup = '{Env.User.Factory}'", paras);

                    if (!isSameM)
                    {
                        MyUtility.Msg.InfoBox("Factory is different!!");
                        e.Cancel = true;
                        return;
                    }
                    #endregion

                    DualResult result;

                    paras.Add(new SqlParameter("@Seq", seq));

                    string cmd = string.Empty;

                    cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID
";

                    result = DBProxy.Current.Select(null, cmd, paras, out DataTable dt);

                    if (result)
                    {
                        if (dt.Rows.Count == 0)
                        {
                            // = 0
                            if (!MyUtility.Check.Empty(seq))
                            {
                                MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                                e.Cancel = true;
                            }
                        }
                        else if (dt.Rows.Count > 1)
                        {
                            // > 1
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "ID,Seq", "15,10", orderID, "ID,Seq")
                            {
                                Width = 600,
                            };
                            DialogResult dresult = item.ShowDialog();
                            if (dresult == DialogResult.OK)
                            {
                                IList<DataRow> selectedDatas = item.GetSelecteds();
                                seq = selectedDatas[0]["Seq"].ToString();
                                selectedRow["Seq"] = seq;
                                selectedRow["orderID"] = orderID;
                            }
                        }
                        else if (dt.Rows.Count == 1)
                        {
                            seq = dt.Rows[0]["Seq"].ToString();
                            selectedRow["Seq"] = seq;
                            selectedRow["orderID"] = orderID;
                        }
                    }
                    else
                    {
                        string msg = string.Empty;

                        foreach (var message in result.Messages)
                        {
                            msg += message + "\r\n";
                        }

                        MyUtility.Msg.WarningBox("DB Query Error : " + msg);
                    }

                    selectedRow.EndEdit();
                }
            };

            col_OrderID.CellMouseClick += (s, e) =>
            {
                // 第二筆資料才開始允許開窗
                if (e.RowIndex > 0 && this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenWindow(e.RowIndex);
                }
            };

            col_OrderID.EditingMouseDown += (s, e) =>
            {
                // 第二筆資料才開始允許開窗
                if (e.RowIndex > 0 && this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenWindow(e.RowIndex);
                }
            };
            #endregion

            #region col_Seq
            col_Seq.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow selectedRow = this.grid.GetDataRow(e.RowIndex);
                    string orderID = MyUtility.Convert.GetString(selectedRow["OrderID"]);
                    string seq = e.FormattedValue.ToString();

                    // SP Seq都不為空才驗證
                    if (MyUtility.Check.Empty(orderID) || MyUtility.Check.Empty(seq))
                    {
                        return;
                    }

                    DualResult result;

                    string cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID AND Seq = @Seq
";

                    List<SqlParameter> paras = new List<SqlParameter>
                    {
                        new SqlParameter("@ID", orderID),
                        new SqlParameter("@Seq", seq),
                    };

                    result = DBProxy.Current.Select(null, cmd, paras, out DataTable dt);
                    if (dt.Rows.Count == 0)
                    {
                        // 驗證失敗清空
                        MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                        selectedRow["Seq"] = string.Empty;
                    }

                    selectedRow.EndEdit();
                }
            };

            col_Seq.CellMouseClick += (s, e) =>
            {
                // 第二筆資料才開始允許開窗
                if (e.RowIndex > 0 && this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenWindow(e.RowIndex);
                }
            };

            col_Seq.EditingMouseDown += (s, e) =>
            {
                // 第二筆資料才開始允許開窗
                if (e.RowIndex > 0 && this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenWindow(e.RowIndex);
                }
            };
            #endregion

            #region col_Carton
            col_Carton.CellValidating += (s, e) =>
            {
                string currentCarton = e.FormattedValue.ToString();
                DataRow selectedRow = this.grid.GetDataRow(e.RowIndex);

                if (this.EditMode && currentCarton.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
                {
                    string stage = MyUtility.Convert.GetString(this.MasterCFAInspectionRecord["stage"]);

                    if (stage.ToUpper() != "3RD PARTY")
                    {
                        selectedRow["Carton"] = string.Empty;
                        selectedRow.EndEdit();
                        return;
                    }

                    string orderID = MyUtility.Convert.GetString(selectedRow["OrderID"]);
                    string seq = MyUtility.Convert.GetString(selectedRow["seq"]);

                    // SP Seq都不為空才驗證
                    if (MyUtility.Check.Empty(orderID) || MyUtility.Check.Empty(seq))
                    {
                        return;
                    }

                    // Sample單直接給空白
                    bool isSample = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"SELECT  IIF(Category='S','True','False') FROM Orders WHERE ID = '{orderID}' "));
                    if (isSample)
                    {
                        selectedRow["Carton"] = string.Empty;
                        selectedRow.EndEdit();
                        return;
                    }

                    List<string> cartons = currentCarton.Split(',').Where(o => !MyUtility.Check.Empty(o)).Distinct().ToList();
                    List<string> errorCartons = new List<string>();

                    foreach (var carton in cartons)
                    {
                        DataTable dt;
                        string sqlCmd = $@"
SELECT * 
FROM PackingList_Detail pd
WHERE OrderID = @OrderID
AND OrderShipmodeSeq = @Seq
AND CTNStartNo = @CTNStartNo
AND (StaggeredCFAInspectionRecordID = @ID OR StaggeredCFAInspectionRecordID = '')

";
                        if (this.MasterCFAInspectionRecord["Stage"].ToString() == "Final" || this.MasterCFAInspectionRecord["Stage"].ToString().ToLower() == "3rd party")
                        {
                            sqlCmd = $@"
SELECT * 
FROM PackingList_Detail pd
WHERE OrderID = @OrderID
AND OrderShipmodeSeq = @Seq
AND CTNStartNo = @CTNStartNo

";
                        }

                        List<SqlParameter> paras = new List<SqlParameter>();
                        paras.Add(new SqlParameter("@OrderID", orderID));
                        paras.Add(new SqlParameter("@Seq", seq));
                        paras.Add(new SqlParameter("@CTNStartNo", carton));
                        paras.Add(new SqlParameter("@ID", this.CFAInspectionRecordID));

                        DualResult r = DBProxy.Current.Select(null, sqlCmd, paras, out dt);
                        if (!r)
                        {
                            this.ShowErr(r);
                        }
                        else
                        {
                            if (dt.Rows.Count == 0)
                            {
                                errorCartons.Add(carton);
                            }
                        }
                    }

                    if (errorCartons.Count > 0)
                    {
                        MyUtility.Msg.WarningBox($"CTN# NOT Found : " + errorCartons.JoinToString(","));
                        selectedRow["Carton"] = string.Empty;
                    }
                    else
                    {
                        selectedRow["Carton"] = cartons.JoinToString(",");
                    }
                }

                if (!currentCarton.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
                {
                    selectedRow["Carton"] = string.Empty;
                }

                selectedRow.EndEdit();
            };

            col_Carton.CellMouseClick += (s, e) =>
            {
                // 第二筆資料才開始允許開窗
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenCartonWindow(e.RowIndex);
                }
            };

            col_Carton.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    this.OpenCartonWindow(e.RowIndex);
                }
            };
            #endregion

            this.grid.DataSource = this.MasterCFAInspectionRecord_OrderSEQ.Copy();
            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: col_OrderID)
            .Text("Seq", header: "SEQ", width: Widths.AnsiChars(5), iseditingreadonly: false, settings: col_Seq)
            .Text("Carton", header: "Inspected Carton", width: Widths.AnsiChars(50), iseditingreadonly: false, settings: col_Carton)
            ;

            this.EditModeToggle();
        }

        private void OpenCartonWindow(int rowIndex)
        {
            DataRow topRow = this.grid.GetDataRow(0);
            DataRow currentRow = this.grid.GetDataRow(rowIndex);

            string orderID = MyUtility.Convert.GetString(currentRow["OrderID"]);
            string seq = MyUtility.Convert.GetString(currentRow["seq"]);
            string currentCarton = MyUtility.Convert.GetString(currentRow["Carton"]);
            string stage = MyUtility.Convert.GetString(this.MasterCFAInspectionRecord["stage"]);

            if (stage.ToUpper() != "3RD PARTY")
            {
                return;
            }

            // SP Seq都不為空才驗證
            if (MyUtility.Check.Empty(orderID) || MyUtility.Check.Empty(seq))
            {
                return;
            }

            bool isSample = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"SELECT  IIF(Category='S','True','False') FROM Orders WHERE ID = '{orderID}' "));
            if (isSample)
            {
                return;
            }

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@OrderID", orderID));
            paras.Add(new SqlParameter("@Seq", seq));

            #region SQL
            string sqlCmd = $@"

----記錄哪些箱號有混尺碼
SELECT ID,OrderID,OrderShipmodeSeq,CTNStartNo
		,[ArticleCount]=COUNT(DISTINCT Article)
		,[SizeCodeCount]=COUNT(DISTINCT SizeCode)
INTO #MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID = @OrderID
AND OrderShipmodeSeq = @Seq
AND (pd.StaggeredCFAInspectionRecordID = '{this.CFAInspectionRecordID}' OR pd.StaggeredCFAInspectionRecordID = '')
GROUP BY ID,OrderID,OrderShipmodeSeq,CTNStartNo
HAVING COUNT(DISTINCT Article) > 1 OR COUNT(DISTINCT SizeCode) > 1


SELECT * FROM (
    ----不是混尺碼的正常做
	SELECT [CTN#]=CTNStartNo
		,Article 
		,[Size]=SizeCode 
		,[Qty]=SUM(ShipQty) 
	FROM PackingList_Detail pd
	WHERE pd.OrderID= @OrderID
	AND OrderShipmodeSeq =  @Seq
	AND (pd.StaggeredCFAInspectionRecordID = '{this.CFAInspectionRecordID}' OR pd.StaggeredCFAInspectionRecordID = '')
	AND NOT EXISTS(
		SELECT  *  
		FROM #MixCTNStartNo t 
		WHERE t.ID = pd.ID AND t.OrderID = pd.OrderID 
		AND t.OrderShipmodeSeq=pd.OrderShipmodeSeq AND t.CTNStartNo=pd.CTNStartNo
	)
	GROUP BY CTNStartNo,Article ,SizeCode
	UNION
    ----混尺碼分開處理
	SELECt [CTN#]=t.CTNStartNo
		,[Article]=MixArticle.Val 
		,[Size]=MixSizeCode.Val
		,[Qty]=ShipQty.Val
	FROM #MixCTNStartNo t
	OUTER APPLY(
		SELECT  [Val]=  STUFF((
			SELECT DISTINCT ','+Article  
			FROM PackingList_Detail pd
			WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
			AND (pd.StaggeredCFAInspectionRecordID = '{this.CFAInspectionRecordID}' OR pd.StaggeredCFAInspectionRecordID = '')
		FOR XML PATH(''))
		,1,1,'')
	)MixArticle
	OUTER APPLY(
		SELECT  [Val]=  STUFF((
			SELECT DISTINCT ','+SizeCode  
			FROM PackingList_Detail pd
			WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
			AND (pd.StaggeredCFAInspectionRecordID = '{this.CFAInspectionRecordID}' OR pd.StaggeredCFAInspectionRecordID = '')
		FOR XML PATH(''))
		,1,1,'')
	)MixSizeCode
	OUTER APPLY(
		SELECT  [Val]=SUM(pd.ShipQty)
		FROM PackingList_Detail pd
		WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
		    AND (pd.StaggeredCFAInspectionRecordID = '{this.CFAInspectionRecordID}' OR pd.StaggeredCFAInspectionRecordID = '')
	)ShipQty
) a
ORDER BY Cast([CTN#] as int)


DROP TABLE #MixCTNStartNo 

";

            if (this.MasterCFAInspectionRecord["Stage"].ToString() == "Final" || this.MasterCFAInspectionRecord["Stage"].ToString().ToLower() == "3rd party")
            {
                sqlCmd = $@"

----記錄哪些箱號有混尺碼
SELECT ID,OrderID,OrderShipmodeSeq,CTNStartNo
		,[ArticleCount]=COUNT(DISTINCT Article)
		,[SizeCodeCount]=COUNT(DISTINCT SizeCode)
INTO #MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID = @OrderID
AND OrderShipmodeSeq = @Seq
GROUP BY ID,OrderID,OrderShipmodeSeq,CTNStartNo
HAVING COUNT(DISTINCT Article) > 1 OR COUNT(DISTINCT SizeCode) > 1


SELECT * FROM (
    ----不是混尺碼的正常做
	SELECT [CTN#]=CTNStartNo
		,Article 
		,[Size]=SizeCode 
		,[Qty]=SUM(ShipQty) 
	FROM PackingList_Detail pd
	WHERE pd.OrderID= @OrderID
	AND OrderShipmodeSeq =  @Seq
	AND NOT EXISTS(
		SELECT  *  
		FROM #MixCTNStartNo t 
		WHERE t.ID = pd.ID AND t.OrderID = pd.OrderID 
		AND t.OrderShipmodeSeq=pd.OrderShipmodeSeq AND t.CTNStartNo=pd.CTNStartNo
	)
	GROUP BY CTNStartNo,Article ,SizeCode
	UNION
    ----混尺碼分開處理
	SELECt [CTN#]=t.CTNStartNo
		,[Article]=MixArticle.Val 
		,[Size]=MixSizeCode.Val
		,[Qty]=ShipQty.Val
	FROM #MixCTNStartNo t
	OUTER APPLY(
		SELECT  [Val]=  STUFF((
			SELECT DISTINCT ','+Article  
			FROM PackingList_Detail pd
			WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
		FOR XML PATH(''))
		,1,1,'')
	)MixArticle
	OUTER APPLY(
		SELECT  [Val]=  STUFF((
			SELECT DISTINCT ','+SizeCode  
			FROM PackingList_Detail pd
			WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
		FOR XML PATH(''))
		,1,1,'')
	)MixSizeCode
	OUTER APPLY(
		SELECT  [Val]=SUM(pd.ShipQty)
		FROM PackingList_Detail pd
		WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
	)ShipQty
) a
ORDER BY Cast([CTN#] as int)


DROP TABLE #MixCTNStartNo 

";
            }

            #endregion

            DataTable dt;
            DBProxy.Current.Select(null, sqlCmd, paras, out dt);
            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(dt, "CTN#,Article,Size,Qty", "CTN#,Article,Size,Qty", "3,15,20,5", currentCarton, null, null, null);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.OK)
            {
                currentRow["Carton"] = item.GetSelectedString();
            }

            currentRow.EndEdit();
        }

        private void OpenWindow(int rowIndex)
        {
            DataRow topRow = this.grid.GetDataRow(0);
            DataRow currentRow = this.grid.GetDataRow(rowIndex);
            string orderID = MyUtility.Convert.GetString(topRow["OrderID"]);
            string stage = MyUtility.Convert.GetString(this.MasterCFAInspectionRecord["stage"]);
            string where = string.Empty;

            if (stage.ToUpper() != "3RD PARTY")
            {
                where = "AND oq.CFAIs3rdInspect = 0";
            }
            else
            {
                where = "AND oq.CFAIs3rdInspect = 1";
            }

            DataTable dt;
            string cmd = $@"
SELECT [OrderID]=o.ID ,oq.Seq
FROM Orders o 
INNER JOIN Order_QtyShip oq ON o.ID =oq.ID
WHERE o.Finished = 0 AND
EXISTS
(
	SELECT 1
	FROM Orders
	WHERE ID='{orderID}'
	AND Ftygroup =o.Ftygroup AND SeasonID =o.SeasonID AND BrandID =o.BrandID  AND StyleID =o.StyleID
)
{where}
";
            DBProxy.Current.Select(null, cmd, out dt);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "OrderID,Seq", "15,10", orderID, "OrderID,Seq")
            {
                Width = 600,
            };
            DialogResult dresult = item.ShowDialog();
            if (dresult == DialogResult.OK)
            {
                IList<DataRow> selectedDatas = item.GetSelecteds();
                currentRow["OrderID"] = selectedDatas[0]["OrderID"].ToString();
                currentRow["Seq"] = selectedDatas[0]["Seq"].ToString();
            }

            topRow.EndEdit();
        }

        /// <summary>
        /// 取得資料來源
        /// </summary>
        /// <param name="cFAInspectionRecordID">cFAInspectionRecordID</param>
        /// <returns>Grid Data Source</returns>
        public DataTable Get_CFAInspectionRecord_OrderSEQ(string cFAInspectionRecordID)
        {
            string cmd = $@" SELECT * FROM CFAInspectionRecord_OrderSEQ WHERE ID = '{cFAInspectionRecordID}'";
            DataTable dt;
            DualResult r = DBProxy.Current.Select(null, cmd, out dt);

            if (!r)
            {
                this.ShowErr(r);
                return null;
            }

            return dt;
        }

        private void BtnEditSave_Click(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                this.EditModeToggle();
            }
            else
            {
                this.EditModeToggle();
                this.SaveClick();
                this.grid.DataSource = null;
                this.grid.DataSource = this.MasterCFAInspectionRecord_OrderSEQ.Copy();
                this.Close();
            }
        }

        private void BtnCloseUndo_Click(object sender, EventArgs e)
        {
            switch (this.btnCloseUndo.Text)
            {
                case "Close":
                    this.Close();
                    break;
                case "Undo":
                    // this.EditModeToggle();
                    // this.UndoClick();
                    this.Close();
                    break;
                default:
                    break;
            }
        }

        private void SaveClick()
        {
            try
            {
                DataTable gridDt = (DataTable)this.grid.DataSource;

                this.MasterCFAInspectionRecord_OrderSEQ.Clear();
                foreach (DataRow dr in gridDt.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(o["OrderID"]) && !MyUtility.Check.Empty(o["Seq"])))
                {
                    string currentOrderID = MyUtility.Convert.GetString(dr["OrderID"]);
                    string currentSeq = MyUtility.Convert.GetString(dr["Seq"]);

                    // 不存在則加入
                    if (!this.MasterCFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted &&
                            MyUtility.Convert.GetString(o["OrderID"]) == currentOrderID &&
                            MyUtility.Convert.GetString(o["Seq"]) == currentSeq).Any())
                    {
                        // 只有3RD PARTY 可以存入Carton
                        string stage = MyUtility.Convert.GetString(this.MasterCFAInspectionRecord["stage"]);

                        if (stage.ToUpper() != "3RD PARTY")
                        {
                            dr["Carton"] = string.Empty;
                        }

                        this.MasterCFAInspectionRecord_OrderSEQ.ImportRow(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void UndoClick()
        {
            this.grid.DataSource = null;
            this.grid.DataSource = this.MasterCFAInspectionRecord_OrderSEQ.Copy();
        }

        /// <summary>
        /// 切換EditMode，以及按鈕文字、Visiable切換
        /// </summary>
        private void EditModeToggle()
        {
            if (this.EditMode)
            {
                this.EditMode = false;
                this.btnAppend.Visible = false;
                this.btnDelete.Visible = false;
                this.btnEditSave.Text = "Edit";
                this.btnCloseUndo.Text = "Close";
            }
            else
            {
                this.EditMode = true;
                this.btnAppend.Visible = true;
                this.btnDelete.Visible = true;
                this.btnEditSave.Text = "Save";
                this.btnCloseUndo.Text = "Undo";
            }
        }

        private void BtnAppend_Click(object sender, EventArgs e)
        {
            DataTable gridDt = (DataTable)this.grid.DataSource;

            DataRow nRow = gridDt.NewRow();
            nRow["ID"] = this.MasterCFAInspectionRecord_OrderSEQ;
            gridDt.Rows.Add(nRow);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.grid.Rows.Count > 0)
            {
                int currentRowIndex = this.grid.CurrentRow.Index;
                this.grid.Rows.RemoveAt(currentRowIndex);
            }
        }
    }
}
