using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
<<<<<<< HEAD
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using System.Transactions;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
=======
using System.IO.Compression;
using Sci.Data;
using Ict.Win;
>>>>>>> ISP20191302

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P25 : Sci.Win.Tems.QueryForm
    {
        private DataTable gridData;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        public P25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

<<<<<<< HEAD
<<<<<<< HEAD
        /// <inheritdoc/>
        protected override void OnFormLoaded()
=======
        private void btnSelecPath_Click(object sender, EventArgs e)
=======
        private void BtnSelecPath_Click(object sender, EventArgs e)
>>>>>>> ISP20191302 - 暫時commit
        {

            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            string dirPath = string.Empty;

            if (!MyUtility.Check.Empty(path.SelectedPath))
            {
                dirPath = path.SelectedPath;
            }
            else
            {
                dirPath = @"D:\TestPath";
            }

            this.txtPath.Text = dirPath;
        }

<<<<<<< HEAD
        private void btnSelectFile_Click(object sender, EventArgs e)
>>>>>>> ISP20191302
=======
        private void BtnSelectFile_Click(object sender, EventArgs e)
>>>>>>> ISP20191302 - 暫時commit
        {
            base.OnFormLoaded();

            // Grid設定
            this.grid.IsEditingReadOnly = false;

            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("Sel", header: "Sel", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("Id", header: "Packing Guide ID", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Ship mode seq", iseditingreadonly: true)
                .Text("CustPONo", header: "P.O No", iseditingreadonly: true)
                .Text("StyleID", header: "Style", iseditingreadonly: true)
                .Numeric("Qty", header: "Order Q'ty", decimal_places: 0, iseditingreadonly: true)
                .Text("ShipModeID", header: "Ship Mode", iseditingreadonly: true)
                .Numeric("CTNQty", header: "Total Cartons", decimal_places: 0, iseditingreadonly: true)
                ;
        }

        /// <inheritdoc/>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <inheritdoc/>
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (
                    MyUtility.Check.Empty(this.txtPGID_s.Text) &&
                    MyUtility.Check.Empty(this.txtPGID_e.Text) &&
                    MyUtility.Check.Empty(this.txtSP_s.Text) &&
                    MyUtility.Check.Empty(this.txtSP_e.Text) &&
                    MyUtility.Check.Empty(this.txtPOno.Text))
            {
                this.txtPGID_s.Focus();
                MyUtility.Msg.WarningBox("Packing Guide ID, SP#, P.O. No cannot all be empty.");
                return;
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            List<string> sqlWhere = new List<string>();
            StringBuilder sqlCmd = new StringBuilder();

            #region WHERE條件

            sqlParameters.Add(new SqlParameter("@MDivisionID", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(this.txtPGID_s.Text))
            {
                sqlWhere.Add(" p.ID >= @PGID_s ");
                sqlParameters.Add(new SqlParameter("@PGID_s", this.txtPGID_s.Text));
            }

<<<<<<< HEAD
            if (!MyUtility.Check.Empty(this.txtPGID_e.Text))
            {
                sqlWhere.Add(" p.ID <= @PGID_e ");
                sqlParameters.Add(new SqlParameter("@PGID_e", this.txtPGID_e.Text));
            }
=======
                    List<ZPL> zPL_Object = this.Get_ZPL_Object(dataList_String, custCTN_List);
>>>>>>> ISP20191302

<<<<<<< HEAD
            if (!MyUtility.Check.Empty(this.txtSP_s.Text))
            {
                sqlWhere.Add(" p.OrderID >= @SP_s ");
                sqlParameters.Add(new SqlParameter("@SP_s", this.txtSP_s.Text));
            }
=======
                    this.Mapping_PackingList_Detal(zPL_Object);
                    // 存檔至路徑
                    //var zip = ZipHelper.ZipData(dataList);
                    //System.IO.File.WriteAllBytes($@"{dirPath}\P25_Files.zip", zip);
>>>>>>> ISP20191302 - 暫時commit

            if (!MyUtility.Check.Empty(this.txtSP_e.Text))
            {
                sqlWhere.Add(" p.OrderID <= @SP_e ");
                sqlParameters.Add(new SqlParameter("@SP_e", this.txtSP_e.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPOno.Text))
            {
                sqlWhere.Add(" o.CustPONo = @POno ");
                sqlParameters.Add(new SqlParameter("@POno", this.txtPOno.Text));
            }
            #endregion

            #region SQL語法
            sqlCmd.Append($@"
SELECT  [Sel]=0
		,p.Id
		,p.OrderID
		,p.OrderShipmodeSeq
		,o.CustPONo
		,o.StyleID
		,oq.Qty
		,p.ShipModeID
		,p.CTNQty

		,o.CtnType
		,[OrderQty] = o.Qty
		,p.CTNStartNo
		,p.SpecialInstruction
		,p.Remark
FROM PackingGuide p WITH(NOLOCK)
INNER JOIN Orders o WITH(NOLOCK) ON p.OrderID = o.ID
INNER JOIN Order_QtyShip oq WITH(NOLOCK) ON oq.Id = o.ID
WHERE p.MDivisionID = @MDivisionID
").Append("AND" + sqlWhere.JoinToString(Environment.NewLine + "AND"));
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), sqlParameters, out this.gridData);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
            }
            else
            {
                if (this.gridData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            this.listControlBindingSource.DataSource = this.gridData;
        }

<<<<<<< HEAD
<<<<<<< HEAD
        /// <inheritdoc/>
        private void BtnToExcel_Click(object sender, EventArgs e)
=======
=======
        /// <inheritdoc/>
>>>>>>> ISP20191302 - 暫時commit
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("PackingListID", header: "Packing No. ", width: Widths.AnsiChars(15))
            .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(15))
            .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(5))
            .Text("SCICtnNo", header: "SCI Ctn No.", width: Widths.AnsiChars(20))
            .Text("Article", header: "Color Way ", width: Widths.AnsiChars(10))
            .Text("SizeCode", header: "Size ", width: Widths.AnsiChars(10))
            .Text("CustCTN", header: "Cust #", width: Widths.AnsiChars(30))
            ;
        }

        private void Mapping_PackingList_Detal(List<ZPL> zPL_Object)
        {

            DataTable resutDt = new DataTable();
            string sqlCmdAA = $@"
SELECT [PackingListID]=pd.ID
        ,pd.OrderID
        ,pd.CTNStartNo
        ,pd.SCICtnNo
        ,pd.Article
        ,pd.SizeCode
        ,pd.CustCTN
FROM PackingList_Detail pd
WHERE 1=0
";

            // 取得結構
            DBProxy.Current.Select(null, sqlCmdAA, out resutDt);

            #region 規則說明
            /*
ZPL對應PackingList_Detail時，可能會有以下情況：（請從ZPL為起點，去找屬於哪個PackingList_Detail）


  1.相同PackingList，相同訂單，不同箱號(CTNStartno)
例如：有100個ZPL檔 
→ 依據PackingList_Detail.OrderID、Article、CTNStartno等資訊，一個ZPL對應一個箱號寫入

(可能多個SzieCode集中在一個箱號，等ZPL格式確定會長什麼樣子再研究做法)

--------------------------------------------------------------------------------

  2.不同PackingList，相同訂單(SP#)，相同箱號(CTNStartno)
→ 依照OrderShipmodeSeq，從最小 & CustCTN為空的開始填入

例如：
SP#：19050463PP003、箱號：1，分屬兩個PackingList (ES2PG19010638 , ES2PG19040460)

先填入 ES2PG19010638 這個PackingList_Deatail的CustCTN，因為OrderShipmodeSeq是01 且 CustCTN為空

有100個ZPL檔，分屬19050463PP003這個訂單的兩個PackingList

--------------------------------------------------------------------------------

  3. CustPONo + StyleID 會對應到多個SP#
→ 找出ID最小的SP#，且Article、SizeCode、箱號相同的PackingList_Detail，都更新CustCTN

例如：
CustPONo='4202777060' 、 StyleID='1351662'，會對應到19101523PP004、19101523PP006兩個SP#



總結：
先區分有沒有多OrdersID(狀況3)
再區分有沒有多PackingList(狀況1或2)

因此對應方式有四種

             */
            #endregion

            zPL_Object.ForEach(singleZPL =>
            {
                DataTable tmpDt;
                string sqlCmd = string.Empty;
                bool isType3 = false;

                // 狀況3判斷
                sqlCmd = $@"

SELECT ID ,StyleID ,POID
FROM Orders 
WHERE CustPONo='{singleZPL.CustPONo}' AND StyleID='{singleZPL.StyleID}'

";

                DBProxy.Current.Select(null, sqlCmd, out tmpDt);

                if (tmpDt.Rows.Count > 1)
                {
                    isType3 = true;
                }

                if (isType3)
                {
                    #region 有3的狀況

                    sqlCmd = $@"

SELECT ID ,StyleID ,POID
INTO #tmoOrders
FROM Orders 
WHERE CustPONo='{singleZPL.CustPONo}' AND StyleID='{singleZPL.StyleID}'

DECLARE @MinOrderID varchar(20)

SELECT @MinOrderID = MIN(pd.OrderID)
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON pd.OrderID=o.ID 
WHERE 1=1
AND p.Type = 'B'
AND pd.Article = '{singleZPL.Article}'
AND pd.CTNStartNo = '{singleZPL.CTNStartNo}'
AND (
		pd.SizeCode in
		(
			SELECT TOP 1 SizeCode 
			FROM Order_SizeSpec 
			WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders) AND SizeSpec IN ('{singleZPL.SizeCode}')
		) 
		OR 
		pd.SizeCode='{singleZPL.SizeCode}'
	)
AND EXISTS (SELECT 1 FROM #tmoOrders WHERE ID = o.ID AND StyleID = o.StyleID)
;

SELECT   [PackingListID]=p.ID
        ,pd.OrderID
        ,pd.CTNStartNo
        ,pd.SCICtnNo
        ,pd.Article
        ,pd.SizeCode
        --,pd.CustCTN
        ,[CustCTN] = '{singleZPL.CustCTN}'
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON pd.OrderID=o.ID 
WHERE 1=1
AND p.Type = 'B'
AND pd.Article = '{singleZPL.Article}'
AND pd.CTNStartNo = '{singleZPL.CTNStartNo}'
AND (
		pd.SizeCode in
		(
			SELECT TOP 1 SizeCode 
			FROM Order_SizeSpec 
			WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders WHERE ID = @MinOrderID) AND SizeSpec IN ('{singleZPL.SizeCode}')
		) 
		OR 
		pd.SizeCode='{singleZPL.SizeCode}'
	)
AND EXISTS (SELECT 1 FROM #tmoOrders WHERE ID = o.ID AND StyleID = o.StyleID AND ID = @MinOrderID)

";
                    #endregion

                    foreach (DataRow dr in tmpDt.Rows)
                    {
                        string orderID = dr["ID"].ToString();

                        // 再判斷是不是 狀況2
                        #region 再判斷是不是 狀況2
                        sqlCmd = $@"

SELECT ID ,StyleID ,POID
INTO #tmoOrders
FROM Orders 
WHERE CustPONo='{singleZPL.CustPONo}' AND StyleID='{singleZPL.StyleID}'

SELECT pd.*
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' AND pd.CustCTN = ''
    AND pd.OrderID ='{orderID}'
    AND CTNStartNo='{singleZPL.CTNStartNo}' 
    AND Article = '{singleZPL.Article}'
    AND (
	    pd.SizeCode in
	    (
		    SELECT TOP 1 SizeCode 
		    FROM Order_SizeSpec 
		    WHERE SizeItem='S01' AND ID = '{orderID}' AND SizeSpec IN ('{singleZPL.SizeCode}')
	    ) 
	    OR 
	    pd.SizeCode='{singleZPL.SizeCode}'
        )

";
                        #endregion

                        DBProxy.Current.Select(null, sqlCmd, out tmpDt);

                        if (tmpDt.Rows.Count > 1)
                        {
                            #region 狀況2

                            string min_OrderShipmodeSeq_PackingListID = tmpDt.AsEnumerable().OrderByDescending(o => o["OrderShipmodeSeq"].ToString()).FirstOrDefault()["ID"].ToString();

                            sqlCmd = $@"

SELECT [PackingListID]=pd.ID
        ,pd.OrderID
        ,pd.CTNStartNo
        ,pd.SCICtnNo
        ,pd.Article
        ,pd.SizeCode
        --,pd.CustCTN
        ,[CustCTN] = '{singleZPL.CustCTN}'
FROM PackingList_Detail pd
WHERE ID='{min_OrderShipmodeSeq_PackingListID}' 
AND pd.OrderID ='{orderID}'
AND CTNStartNo='{singleZPL.CTNStartNo}' 
AND Article = '{singleZPL.Article}'  
 AND (
		pd.SizeCode in
		(
			SELECT TOP 1 SizeCode 
			FROM Order_SizeSpec 
			WHERE SizeItem='S01' AND ID ='{orderID}' AND SizeSpec IN ('{singleZPL.SizeCode}')
		) 
		OR 
		pd.SizeCode='{singleZPL.SizeCode}'
	)

";

                            DBProxy.Current.Select(null, sqlCmd, out tmpDt);

                            resutDt = this.InsertResultDt(resutDt, tmpDt);
                            #endregion
                        }
                        else
                        {
                            #region 狀況1

                            sqlCmd = $@"

SELECT   [PackingListID]=p.ID
        ,pd.OrderID
        ,pd.CTNStartNo
        ,pd.SCICtnNo
        ,pd.Article
        ,pd.SizeCode
        --,pd.CustCTN
        ,[CustCTN] = '{singleZPL.CustCTN}'
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON pd.OrderID=o.ID 
WHERE 1=1
AND p.Type = 'B'
AND pd.Article = '{singleZPL.Article}'
AND pd.CTNStartNo = '{singleZPL.CTNStartNo}'
AND (
		pd.SizeCode in
		(
			SELECT TOP 1 SizeCode 
			FROM Order_SizeSpec 
			WHERE SizeItem='S01' AND ID ='{orderID}' AND SizeSpec IN ('{singleZPL.SizeCode}')
		) 
		OR 
		pd.SizeCode='{singleZPL.SizeCode}'
	)
AND pd.OrderID ='{orderID}'


";


                            DBProxy.Current.Select(null, sqlCmd, out tmpDt);

                            resutDt = this.InsertResultDt(resutDt, tmpDt);
                            #endregion
                        }
                    }
                }
                else
                {
                    #region 判斷是不是 狀況2
                    sqlCmd = $@"

SELECT ID ,StyleID ,POID
INTO #tmoOrders
FROM Orders 
WHERE CustPONo='{singleZPL.CustPONo}' AND StyleID='{singleZPL.StyleID}'

SELECT pd.*
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' AND pd.CustCTN = ''
    AND pd.OrderID = (SELECT ID FROM #tmoOrders)
    AND CTNStartNo='{singleZPL.CTNStartNo}' 
    AND Article = '{singleZPL.Article}'
    AND (
	    pd.SizeCode in
	    (
		    SELECT TOP 1 SizeCode 
		    FROM Order_SizeSpec 
		    WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders) AND SizeSpec IN ('{singleZPL.SizeCode}')
	    ) 
	    OR 
	    pd.SizeCode='{singleZPL.SizeCode}'
        )

";
                    #endregion

                    DBProxy.Current.Select(null, sqlCmd, out tmpDt);

                    if (tmpDt.Rows.Count > 1)
                    {
                        #region 狀況2

                        string min_OrderShipmodeSeq_PackingListID = tmpDt.AsEnumerable().OrderBy(o => o["OrderShipmodeSeq"].ToString()).FirstOrDefault()["ID"].ToString();

                        sqlCmd = $@"

SELECT ID ,StyleID ,POID
INTO #tmoOrders
FROM Orders 
WHERE CustPONo='{singleZPL.CustPONo}' AND StyleID='{singleZPL.StyleID}'

SELECT [PackingListID]=pd.ID
        ,pd.OrderID
        ,pd.CTNStartNo
        ,pd.SCICtnNo
        ,pd.Article
        ,pd.SizeCode
        --,pd.CustCTN
        ,[CustCTN] = '{singleZPL.CustCTN}'
FROM PackingList_Detail pd
WHERE ID='{min_OrderShipmodeSeq_PackingListID}' 
AND pd.OrderID = (SELECT ID FROM #tmoOrders)
AND CTNStartNo='{singleZPL.CTNStartNo}' 
AND Article = '{singleZPL.Article}'  
 AND (
		pd.SizeCode in
		(
			SELECT TOP 1 SizeCode 
			FROM Order_SizeSpec 
			WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders) AND SizeSpec IN ('{singleZPL.SizeCode}')
		) 
		OR 
		pd.SizeCode='{singleZPL.SizeCode}'
	)

";

                        DBProxy.Current.Select(null, sqlCmd, out tmpDt);

                        resutDt = this.InsertResultDt(resutDt, tmpDt);

                        #endregion
                    }
                    else
                    {
                        #region 狀況1

                        sqlCmd = $@"

SELECT ID ,StyleID ,POID
INTO #tmoOrders
FROM Orders 
WHERE CustPONo='{singleZPL.CustPONo}' AND StyleID='{singleZPL.StyleID}'


SELECT   [PackingListID]=p.ID
        ,pd.OrderID
        ,pd.CTNStartNo
        ,pd.SCICtnNo
        ,pd.Article
        ,pd.SizeCode
        --,pd.CustCTN
        ,[CustCTN] = '{singleZPL.CustCTN}'
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON pd.OrderID=o.ID 
WHERE 1=1
AND p.Type = 'B'
AND pd.Article = '{singleZPL.Article}'
AND pd.CTNStartNo = '{singleZPL.CTNStartNo}'
AND (
		pd.SizeCode in
		(
			SELECT TOP 1 SizeCode 
			FROM Order_SizeSpec 
			WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders) AND SizeSpec IN ('{singleZPL.SizeCode}')
		) 
		OR 
		pd.SizeCode='{singleZPL.SizeCode}'
	)
AND EXISTS (SELECT 1 FROM #tmoOrders WHERE ID = o.ID AND StyleID = o.StyleID)
/*
UPDATE pd
SET pd.CustCTN='{singleZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN PackingList p ON p.ID=pd.ID
WHERE Ukey = @pdUkey AND CustCTN = '' AND p.Status='New'
*/

";

                        DBProxy.Current.Select(null, sqlCmd, out tmpDt);

                        resutDt = this.InsertResultDt(resutDt, tmpDt);
                        #endregion
                    }
                }

                //DBProxy.Current.Select(null, sqlCmd, out tmpDt);

                //this.InsertResultDt(resutDt, tmpDt);
            });

            this.listControlBindingSource1.DataSource = resutDt;
        }

        private DataTable InsertResultDt(DataTable target , DataTable source)
        {
            if (source.Rows.Count > 0)
            {
                foreach (DataRow item in target.Rows)
                {
                    target.Rows.Add(item);
                }
            }
            return target;
        }

        private List<ZPL> Get_ZPL_Object(Dictionary<string, string> dataList_String, List<string> custCTN_List)
>>>>>>> ISP20191302
        {
            this.grid.ValidateControl();
            this.grid.EndEdit();
            this.listControlBindingSource.EndEdit();
            DataTable gridData = (DataTable)this.listControlBindingSource.DataSource;

            if (gridData == null || gridData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

<<<<<<< HEAD
            DataRow[] selectedDatas = gridData.Select("Sel=1");

            if (selectedDatas.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please choose the data first.");
                return;
            }
=======
                string content = dataList_String[custCTNno];
>>>>>>> ISP20191302 - 暫時commit

            this.btnToExcel.Enabled = false;
            foreach (DataRow item in selectedDatas)
            {
                this.Print(item);
            }

            this.btnToExcel.Enabled = true;
        }

        private void Print(DataRow item)
        {
            string minCtnQty = "0";

            // 如果是單色混碼包裝，就先算出最少箱數
            if (item["CtnType"].ToString() == "2")
            {
                minCtnQty = MyUtility.GetValue.Lookup(string.Format("select isnull(min(ShipQty/QtyPerCTN),0) from PackingGuide_Detail WITH (NOLOCK) where Id = '{0}'", MyUtility.Convert.GetString(item["ID"])));
            }

            string sqlCmd = string.Format(
                @"
select pd.Article,pd.Color,pd.SizeCode,pd.QtyPerCTN,pd.ShipQty,
    IIF(pd.ShipQty=0 or pd.QtyPerCTN=0,0,pd.ShipQty/pd.QtyPerCTN)as CtnQty,
    o.CustCDID,o.StyleID,o.CustPONo,o.Customize1,c.Alias,oq.BuyerDelivery
from PackingGuide p WITH (NOLOCK) 
left join PackingGuide_Detail pd WITH (NOLOCK) on p.Id = pd.Id
left join Orders o WITH (NOLOCK) on o.ID = p.OrderID
left join Order_Article oa WITH (NOLOCK) on oa.id = o.ID and oa.Article = pd.Article
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = pd.SizeCode
left join Country c WITH (NOLOCK) on c.ID = o.Dest
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID and oq.Seq = p.OrderShipmodeSeq
where p.Id = '{0}'
order by oa.Seq,os.Seq", MyUtility.Convert.GetString(item["ID"]));
            DataTable printData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail! \r\n" + result.ToString());
                return;
            }

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!");
                return;
            }


            DataTable ctnDim, qtyCtn;
            sqlCmd = string.Format(
                @"
Declare @packinglistid VARCHAR(13),
		@refno VARCHAR(21), 
		@ctnstartno VARCHAR(6),
		@firstctnno VARCHAR(6),
		@lastctnno VARCHAR(6),
		@orirefnno VARCHAR(21),
		@insertrefno VARCHAR(13)

set @packinglistid = '{0}'

--建立暫存PackingList_Detail資料
DECLARE @tempPackingListDetail TABLE (
   RefNo VARCHAR(21),
   CTNNo VARCHAR(13)
)

--撈出PackingList_Detail
DECLARE cursor_PackingListDetail CURSOR FOR
	SELECT RefNo,CTNStartNo FROM PackingList_Detail WITH (NOLOCK) WHERE ID = @packinglistid and CTNQty > 0 ORDER BY Seq

--開始run cursor
OPEN cursor_PackingListDetail
--將第一筆資料填入變數
FETCH NEXT FROM cursor_PackingListDetail INTO @refno, @ctnstartno
SET @firstctnno = @ctnstartno
SET @lastctnno = @ctnstartno
SET @orirefnno = @refno
WHILE @@FETCH_STATUS = 0
BEGIN
	IF(@orirefnno <> @refno)
		BEGIN
			IF(@firstctnno = @lastctnno)
				BEGIN
					SET @insertrefno = @firstctnno
				END
			ELSE
				BEGIN
					SET @insertrefno = @firstctnno + '-' + @lastctnno
				END
			INSERT INTO @tempPackingListDetail (RefNo,CTNNo) VALUES (@orirefnno,@insertrefno)

			--數值重新記錄
			SET @orirefnno = @refno
			SET @firstctnno = @ctnstartno
			SET @lastctnno = @ctnstartno
		END
	ELSE
		BEGIN
			--紀錄箱號
			SET @lastctnno = @ctnstartno
		END

	FETCH NEXT FROM cursor_PackingListDetail INTO @refno, @ctnstartno
END
--最後一筆資料
--最後一筆資料
IF(@orirefnno <> '')
	BEGIN
		IF(@firstctnno = @lastctnno)
			BEGIN
				SET @insertrefno = @firstctnno
			END
		ELSE
			BEGIN
				SET @insertrefno = @firstctnno + '-' + @lastctnno
			END
		INSERT INTO @tempPackingListDetail (RefNo,CTNNo) VALUES (@orirefnno,@insertrefno)
	END
--關閉cursor與參數的關聯
CLOSE cursor_PackingListDetail
--將cursor物件從記憶體移除
DEALLOCATE cursor_PackingListDetail

select distinct t.RefNo,
Ctn = concat('(CTN#:',stuff((select concat(',',CTNNo) from @tempPackingListDetail where RefNo = t.RefNo for xml path('')),1,1,''),')')
into #tmp
from @tempPackingListDetail t
left join LocalItem l on l.RefNo = t.RefNo
order by RefNo

select distinct pd.RefNo, li.Description, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension, li.CtnUnit,a.Ctn
from PackingGuide_Detail pd WITH (NOLOCK) 
left join LocalItem li WITH (NOLOCK) on li.RefNo = pd.RefNo
left join LocalSupp ls WITH (NOLOCK) on ls.ID = li.LocalSuppid
outer apply(select Ctn from #tmp where Refno = pd.RefNo)a
where pd.ID = '{0}'", MyUtility.Convert.GetString(item["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out ctnDim);


            sqlCmd = string.Format(
                @"
select isnull(oq.Article,'') as Article,isnull(oq.SizeCode,'') as SizeCode,isnull(oq.Qty,0) as Qty
from Orders o WITH (NOLOCK) 
left join Order_QtyCTN oq WITH (NOLOCK) on o.ID = oq.Id
left join Order_Article oa WITH (NOLOCK) on o.ID = oa.id and oq.Article = oa.Article
left join Order_SizeCode os WITH (NOLOCK) on o.POID = os.Id and oq.SizeCode = os.SizeCode
where o.ID = '{0}'
order by oa.Seq,os.Seq", MyUtility.Convert.GetString(item["OrderID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out qtyCtn);

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Packing_P25.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            string ttlShipQty = MyUtility.GetValue.Lookup($"SElECT ISNULL(SUM(ShipQty),0) FROm PackingGuide_Detail WHERE ID='{item["ID"]}'");

            this.ShowWaitMessage("Starting to excel...");
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            string nameEN = MyUtility.GetValue.Lookup("NameEN", Sci.Env.User.Factory, "Factory ", "id");
            worksheet.Cells[1, 1] = nameEN;
            worksheet.Cells[3, 2] = item["ID"].ToString();
            worksheet.Cells[3, 9] = MyUtility.Check.Empty(printData.Rows[0]["BuyerDelivery"]) ? string.Empty : Convert.ToDateTime(printData.Rows[0]["BuyerDelivery"]).ToShortDateString();
            worksheet.Cells[3, 19] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(printData.Rows[0]["CustCDID"]);
            worksheet.Cells[6, 1] = MyUtility.Convert.GetString(item["OrderID"]);
            worksheet.Cells[6, 2] = MyUtility.Convert.GetString(printData.Rows[0]["StyleID"]);
            worksheet.Cells[6, 5] = MyUtility.Convert.GetString(printData.Rows[0]["Customize1"]);
            worksheet.Cells[6, 8] = MyUtility.Convert.GetString(printData.Rows[0]["CustPONo"]);
            worksheet.Cells[6, 11] = MyUtility.Convert.GetInt(item["CTNQty"]);
            worksheet.Cells[6, 13] = MyUtility.Convert.GetString(printData.Rows[0]["Alias"]);
            worksheet.Cells[6, 17] = item["OrderQty"].ToString();
            worksheet.Cells[6, 19] = ttlShipQty;
            worksheet.Cells[6, 20] = "=Q6-S6";
            int row = 8, ctnNum = MyUtility.Convert.GetInt(item["CTNStartNo"]), ttlCtn = 0;

            #region 先算出總共會有幾筆record
            int tmpCtnQty = 0;
            foreach (DataRow dr in printData.Rows)
            {
                int ctnQty = item["CtnType"].ToString() == "2" ? MyUtility.Convert.GetInt(minCtnQty) : MyUtility.Convert.GetInt(dr["CtnQty"]);
                int ctn = ctnQty == 0 ? 0 : (int)Math.Ceiling(MyUtility.Convert.GetDecimal(ctnQty) / 15);
                int ship = MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctnQty;
                tmpCtnQty = tmpCtnQty + ctn + (ship >= MyUtility.Convert.GetInt(dr["ShipQty"]) ? 0 : 1);
            }

            // 範本已先有258 row，不夠的話再新增
            if (tmpCtnQty > 258)
            {
                // Insert row
                for (int i = 1; i <= tmpCtnQty - 258; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A8:A8").EntireRow;
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A8:A8", Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                }
            }
<<<<<<< HEAD
            else
            {
                // 刪除多餘的Row
                if (tmpCtnQty < 258)
                {
                    // Insert row
                    for (int i = 1; i <= 258 - tmpCtnQty; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[8, Type.Missing];
                        rng.Select();
                        rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    }
                }
            }
            #endregion

            #region 寫入完整箱的資料
            foreach (DataRow dr in printData.Rows)
=======

            return list;
        }

        #region 類別定義

        public class ZipHelper
        {
            /// <summary>
            /// 將串入的ZPL檔案轉成Zip檔資料流存在記憶體
            /// </summary>
            public static byte[] ZipData(Dictionary<string, byte[]> data)
>>>>>>> ISP20191302
            {
                int ctnQty = item["CtnType"].ToString() == "2" ? MyUtility.Convert.GetInt(minCtnQty) : MyUtility.Convert.GetInt(dr["CtnQty"]);
                if (!MyUtility.Check.Empty(ctnQty))
                {
                    worksheet.Cells[row, 1] = MyUtility.Convert.GetString(dr["Article"]) + " " + MyUtility.Convert.GetString(dr["Color"]);
                    worksheet.Cells[row, 2] = MyUtility.Convert.GetString(dr["SizeCode"]);
                    worksheet.Cells[row, 3] = MyUtility.Convert.GetInt(dr["QtyPerCTN"]);
                    worksheet.Cells[row, 19] = MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctnQty;
                    ttlCtn = 0;
                    if (item["CtnType"].ToString() == "2")
                    {
                        ctnNum = MyUtility.Convert.GetInt(item["CTNStartNo"]);
                    }

                    for (int i = 1; i <= Math.Floor(MyUtility.Convert.GetDecimal(ctnQty - 1) / 15) + 1; i++)
                    {
                        for (int j = 1; j <= 15; j++)
                        {
                            ttlCtn++;
                            if (ttlCtn > MyUtility.Convert.GetInt(dr["CtnQty"]))
                            {
                                break;
                            }

                            worksheet.Cells[row, j + 3] = ctnNum;
                            ctnNum++;
                        }

                        row++;
                    }
                }
            }
            #endregion

            #region 處理餘箱部分
            int insertCTN = 1;
            foreach (DataRow dr in printData.Rows)
            {
                int ctnQty = item["CtnType"].ToString() == "2" ? MyUtility.Convert.GetInt(minCtnQty) : MyUtility.Convert.GetInt(dr["CtnQty"]);
                int remain = MyUtility.Convert.GetInt(dr["ShipQty"]) - (MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctnQty);
                if (remain > 0)
                {
                    worksheet.Cells[row, 1] = MyUtility.Convert.GetString(dr["Article"]) + " " + MyUtility.Convert.GetString(dr["Color"]);
                    worksheet.Cells[row, 2] = MyUtility.Convert.GetString(dr["SizeCode"]);
                    worksheet.Cells[row, 3] = remain;
                    if ((item["CtnType"].ToString() == "2" && insertCTN == 1) || item["CtnType"].ToString() != "2")
                    {
                        worksheet.Cells[row, 4] = ctnNum;
                        insertCTN = 2;
                    }

<<<<<<< HEAD
                    worksheet.Cells[row, 19] = remain;
                    if (item["CtnType"].ToString() != "2")
                    {
                        ctnNum++;
                    }

                    row++;
                }
            }
            #endregion

            int startIndex = 0;
            int endIndex = 0;
            int dataRow = 0;
            // Carton Dimension:
            StringBuilder ctnDimension = new StringBuilder();
            foreach (DataRow dr in ctnDim.Rows)
            {
                ctnDimension.Append(string.Format("{0} / {1} / {2} {3}, {4}  \r\n", MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["Description"]), MyUtility.Convert.GetString(dr["Dimension"]), MyUtility.Convert.GetString(dr["CtnUnit"]), MyUtility.Convert.GetString(dr["Ctn"])));
            }

            foreach (DataRow dr in qtyCtn.Rows)
            {
                if (!MyUtility.Check.Empty(dr["Article"]))
                {
                    ctnDimension.Append(string.Format("{0} -> {1} / {2}, ", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"])));
                }
            }
=======
        }

        public class P25_Object
        {
            public string PackingList_Detail_CustCTN { get; set; }

            public string PackingList_Detail_ID { get; set; }

            public string PackingList_Detail_OrderId { get; set; }

            public string PackingList_Detail_CTNStartNo { get; set; }

            public string PackingList_Detail_SCICtnNo { get; set; }

            public string PackingList_Detail_Article { get; set; }

            public string PackingList_Detail_SizeCode { get; set; }
        }

        public class ZPL
        {
            public string CustPONo { get; set; }
>>>>>>> ISP20191302

            string cds = ctnDimension.Length > 0 ? ctnDimension.ToString().Substring(0, ctnDimension.ToString().Length - 2) : string.Empty;
            string[] cdsab = cds.Split('\r');
            int cdsi = 0;
            int cdsl = 113;
            foreach (string cdsc in cdsab)
            {
                if (cdsc.Length > cdsl)
                {
                    int h = cdsc.Length / cdsl;
                    for (int i = 0; i < h; i++)
                    {
                        cdsi += 1;
                    }
                }
            }

            int cdinst = 0;
            cdsi += cdsab.Length - 2;
            if (cdsi > 0)
            {
                for (int i = 0; i < cdsi; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(row + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                    cdinst++;
                }
            }

            worksheet.Cells[row, 2] = ctnDimension.Length > 0 ? cds : string.Empty;
            row = row + cdinst + 2;
            worksheet.Cells[row, 1] = "Remark: " + MyUtility.Convert.GetString(item["Remark"]);

            // 填Special Instruction
            // 先取得Special Instruction總共有幾行
            string tmp = MyUtility.Convert.GetString(item["SpecialInstruction"]);

            string[] tmpab = tmp.Split('\r');
            int ctmpc = 0;
            int l = 113;
            foreach (string tmpc in tmpab)
            {
                if (tmpc.Length > l)
                {
                    int h = tmpc.Length / l;
                    ctmpc += h;
                }
            }

<<<<<<< HEAD
            for (int i = 1; ; i++)
            {
                if (i > 1)
                {
                    startIndex = endIndex + 2;
                }

                if (tmp.IndexOf("\r\n", startIndex) > 0)
                {
                    endIndex = tmp.IndexOf("\r\n", startIndex);
                }
                else
                {
                    dataRow = i + 2 + ctmpc;
                    break;
                }
            }

            row++;
            if (dataRow > 2)
            {
                for (int i = 3; i < dataRow; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(row + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    rngToInsert.RowHeight = 19.5;
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            // 判斷第一碼為"=" 就塞space ,避免excel 誤認=是計算函數
            if (MyUtility.Check.Empty(item["SpecialInstruction"]))
            {
                worksheet.Cells[row, 2] = MyUtility.Convert.GetString(item["SpecialInstruction"]);
            }
            else if (item["SpecialInstruction"].ToString().Substring(0, 1) == "=")
            {
                worksheet.Cells[row, 2] = "'" + MyUtility.Convert.GetString(item["SpecialInstruction"]);
            }
            else
            {
                worksheet.Cells[row, 2] = MyUtility.Convert.GetString(item["SpecialInstruction"]);
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Packing_P25");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion

            this.HideWaitMessage();
        }
=======
            public string CTNStartNo { get; set; }
        }
        #endregion
>>>>>>> ISP20191302
    }
}
