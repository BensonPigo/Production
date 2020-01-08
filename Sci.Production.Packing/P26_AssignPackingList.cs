using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using static Sci.Production.Packing.P26;

namespace Sci.Production.Packing
{
    public partial class P26_AssignPackingList : Sci.Win.Tems.Base
    {
        private List<MappingModel> _MappingModels;
        private DataTable _P25Dt;
        private DataTable GridDt = new DataTable();
        public bool canConvert = false;
        private string _UploadType = string.Empty;

        public P26_AssignPackingList(List<MappingModel> MappingModels, DataTable P25Dt , string UploadType)
        {
            this.InitializeComponent();
            this._MappingModels = MappingModels;
            this._P25Dt = P25Dt;
            this._UploadType = UploadType;
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "FileName", DataType = typeof(string) });
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "PackingListID", DataType = typeof(string) });
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            foreach (var mappingModel in this._MappingModels)
            {
                DataRow dr = this.GridDt.NewRow();
                dr["FileName"] = mappingModel.FileName;
                dr["PackingListID"] = mappingModel.PackingListID;
                this.GridDt.Rows.Add(dr);
            }

            this.listControlBindingSource1.DataSource = this.GridDt;

            this.grid2.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid2)
.Text("FileName", header: "File Name ", width: Widths.AnsiChars(35))
.Text("PackingListID", header: "PackingList#", width: Widths.AnsiChars(15), iseditingreadonly: false)
;
        }

        private void btnProcessing_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                DualResult result;
                List<string> notMapping_FileName = new List<string>();
                List<string> existsCustCTNList = new List<string>();

                #region 檢查表格內的每一個檔案，裡面的每一張ZPL有沒有Mapping
                foreach (DataRow dr in dt.Rows)
                {
                    string fileName = dr["FileName"].ToString();
                    string packingListID = dr["PackingListID"].ToString();
                    bool isAnyNotMapping = false;

                    MappingModel current = this._MappingModels.Where(o => o.FileName == fileName).FirstOrDefault();

                    foreach (var ZPL in current.ZPL_Content)
                    {
                        // 檢查Usee是否有輸入不對的PackingList ID
                        if (current.IsMixed)
                        {
                            if (!this.checkPackingList_Count(current.IsMixed, ZPL.CustPONo, ZPL.StyleID, packingListID, ZPL.Article, ZPL.CTNStartNo, ZPL.ShipQty, ZPL.SizeCode, ZPL.Size_Qty_List))
                            {
                                isAnyNotMapping = true;
                            }
                        }
                        else
                        {
                            if (!this.checkPackingList_Count(current.IsMixed, ZPL.CustPONo, ZPL.StyleID, packingListID, ZPL.Article, ZPL.CTNStartNo, ZPL.ShipQty, ZPL.SizeCode, ZPL.Size_Qty_List))
                            {
                                isAnyNotMapping = true;
                            }
                        }
                        bool existsCustCTN = MyUtility.Check.Seek($"SELECT 1 FROM PackingList_Detail WHERE CustCTN='{ZPL.CustCTN}' ");

                        if (existsCustCTN)
                        {
                            isAnyNotMapping = true;
                            existsCustCTNList.Add(ZPL.CustCTN);
                        }
                    }

                    if (isAnyNotMapping)
                    {
                        notMapping_FileName.Add(fileName);
                    }

                }
                #endregion

                // 失敗狀況
                if (notMapping_FileName.Count > 0)
                {

                    // 上一層表格填入結果
                    foreach (var fileName in notMapping_FileName.Distinct())
                    {
                        this._P25Dt.AsEnumerable().Where(o => o["FileName"].ToString() == fileName).FirstOrDefault()["Result"] = "Fail";
                    }

                    string msg = "PackingList# does not Mapping.";

                    if (existsCustCTNList.Count > 0)
                    {
                        msg += Environment.NewLine + "CustCTN existed : " + string.Join(" , ", existsCustCTNList);
                    }

                    MyUtility.Msg.InfoBox(msg);
                    this.canConvert = false;
                    return;
                }

                // 成功狀況
                else
                {
                    #region 把每一個檔案，的每一張ZPL都寫入PackingList_Detail

                    string updateCmd = string.Empty;
                    int i = 0;
                    int ii = 0;
                    int iii = 0;
                    List<string> fileNamess = new List<string>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        string fileName = dr["FileName"].ToString();
                        string packingListID = dr["PackingListID"].ToString();
                        // bool IsMixed

                        fileNamess.Add(fileName);

                        MappingModel current = this._MappingModels.Where(o => o.FileName == fileName).FirstOrDefault();

                        string cmd = string.Empty;
                        List<string> sqlMixed = new List<string>();

                        foreach (var ZPL in current.ZPL_Content)
                        {

                            if (this._UploadType == "ZPL")
                            {
                                if (current.IsMixed)
                                {
                                    sqlMixed.Clear();

                                    cmd += $@"
----1. 整理Mapping的資料
SELECT ID ,StyleID ,POID
INTO #tmoOrders{i}
FROM Orders 
WHERE CustPONo='{ZPL.CustPONo}' AND StyleID='{ZPL.StyleID}'
";
                                    foreach (var data in ZPL.Size_Qty_List)
                                    {
                                        cmd += $@"

SELECT CTNStartNo,[CartonCount]=COUNT(pd.Ukey)
INTO #tmpCount{ii}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders0)
    AND pd.CustCTN='' 
    AND Article = '{ZPL.Article}'
	AND ( SizeCode='{data.Size}' OR SizeCode in(
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders0) AND SizeSpec IN ('{data.Size}')
	        )  )
	AND pd.ShipQty={data.Qty}
GROUP BY CTNStartNo
";
                                        ii++;
                                    }

                                    cmd += $@"
SELECT a.CTNStartNo,[CartonCount]=SUM(CartonCount) 
INTO #tmpMappingCartonNo{i}
FROM (
";
                                    foreach (var data in ZPL.Size_Qty_List)
                                    {
                                        sqlMixed.Add($@"
	SELECT  *
	FROM #tmpCount{iii}
");
                                        iii++;
                                    }

                                    cmd += string.Join(" UNION ALL" + Environment.NewLine, sqlMixed);
                                    cmd += $@"

)a
GROUP BY CTNStartNo";
                                    cmd += $@"

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,o.CustCDID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
	AND p.ID='{packingListID}'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders{i})
    AND pd.CustCTN='' 
    AND Article = '{ZPL.Article}'
    AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo{i}) 
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN
UPDATE pd
SET pd.CustCTN='{ZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.CTNStartNo=pd.CTNStartNo AND pd.ID=t.ID


----3. 寫入ShippingMarkPic、ShippingMarkPic_Detail資料
IF NOT EXISTS( SELECT 1 FROM ShippingMarkPic WHERE PackingListID='{packingListID}')
BEGIN
	INSERT INTO ShippingMarkPic
		([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

	SELECT [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
	FROM ShippingMarkPicture s
	INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CustCD=t.CustCDID AND s.CTNRefno=t.RefNo AND s.Side='D'
	INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
END

----ShippingMarkPic_Detail  (Seq=1)
IF EXISTS(
    SELECT 1 FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=1 AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
)
BEGIN
    DELETE FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=1 AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
END

INSERT INTO [dbo].[ShippingMarkPic_Detail]
            ([ShippingMarkPicUkey]
            ,[SCICtnNo]
            ,[FileName])
        VALUES
            ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=1 AND Side='D' )
            ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
            ,'{ZPL.CustCTN}' 
 			)


----ShippingMarkPic_Detail  (Seq=2)
IF EXISTS(
    SELECT 1 FROM ShippingMarkPic_Detail
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=2 AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
)
BEGIN
    DELETE FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=2 AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
END

INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=2 AND Side='D' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)


--DROP TABLE #tmoOrders{i},#tmpMappingCartonNo{i},#tmp{i}
----------------------------------------------------------------------------------------------------
";
                                }
                                else
                                {
                                    cmd += $@"
----1. 整理Mapping的資料
SELECT ID ,StyleID ,POID
INTO #tmpOrders{i}
FROM Orders 
WHERE CustPONo='{ZPL.CustPONo}' AND StyleID='{ZPL.StyleID}'

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,o.CustCDID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
	AND p.ID='{packingListID}'
    AND pd.CustCTN='' 
    AND pd.OrderID = (SELECT ID FROM #tmpOrders{i})
    AND Article = '{ZPL.Article}'
    AND pd.ShipQty={ZPL.ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders{i}) AND SizeSpec IN ('{ZPL.SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{ZPL.SizeCode}'
        )
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN
UPDATE pd
SET pd.CustCTN='{ZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey


----3. 寫入ShippingMarkPic、ShippingMarkPic_Detail資料
IF NOT EXISTS( SELECT 1 FROM ShippingMarkPic WHERE PackingListID='{packingListID}')
BEGIN
	INSERT INTO ShippingMarkPic
		([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

	SELECT [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
	FROM ShippingMarkPicture s
	INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CustCD=t.CustCDID AND s.CTNRefno=t.RefNo AND s.Side='D'
	INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
END


----ShippingMarkPic_Detail  (Seq=1)
IF EXISTS(
    SELECT 1 FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=1 AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
)
BEGIN
    DELETE FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=1 AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
END

INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=1 AND Side='D' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)



----ShippingMarkPic_Detail  (Seq=2)
IF EXISTS(
    SELECT 1 FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=2 AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
)
BEGIN
    DELETE FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=2 AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
END

INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=2 AND Side='D' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)

DROP TABLE #tmpOrders{i},#tmp{i}
";
                                }
                            }

                            if (this._UploadType == "PDF")
                            {
                                if (current.IsMixed)
                                {

                                    foreach (var data in ZPL.Size_Qty_List)
                                    {
                                        sqlMixed.Add($@"
		( SizeCode='{data.Size}' OR SizeCode in(
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders{i}) AND SizeSpec IN ('{data.Size}')
	        )  
			AND pd.ShipQty={data.Qty})
");
                                    }

                                    cmd = $@"

SELECT ID ,StyleID ,POID
INTO #tmoOrders{i}
FROM Orders 
WHERE CustPONo='{ZPL.CustPONo}' AND StyleID='{ZPL.StyleID}'

----1. 整理Mapping的資料
SELECT CTNStartNo,[CartonCount]=COUNT(pd.Ukey)
INTO #tmpMappingCartonNo{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders{i})
    AND pd.CustCTN='' 
    AND Article = '{ZPL.Article}'
	AND (
";
                                    cmd += string.Join("		OR" + Environment.NewLine, sqlMixed);
                                    cmd += $@"
		)
GROUP BY CTNStartNo
HAVING COUNT(pd.Ukey)={ZPL.Size_Qty_List.Count}

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,o.CustCDID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
	AND p.ID='{packingListID}'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders{i})
    AND pd.CustCTN='' 
    AND Article = '{ZPL.Article}'
    AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo{i}) 
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN
UPDATE pd
SET pd.CustCTN='{ZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.CTNStartNo=pd.CTNStartNo AND pd.ID=t.ID


----3. 寫入ShippingMarkPic、ShippingMarkPic_Detail資料
IF NOT EXISTS( SELECT 1 FROM ShippingMarkPic WHERE PackingListID='{packingListID}')
BEGIN
	INSERT INTO ShippingMarkPic
		([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

	SELECT [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
	FROM ShippingMarkPicture s
	INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CustCD=t.CustCDID AND s.CTNRefno=t.RefNo AND s.Side='D'
	INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
END



INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=1 AND Side='D' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)



INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=2 AND Side='D' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)


DROP TABLE #tmoOrders{i},#tmpMappingCartonNo{i},#tmp{i}

";
                                }
                                else
                                {

                                    cmd += $@"
----1. 整理Mapping的資料
SELECT ID ,StyleID ,POID
INTO #tmpOrders{i}
FROM Orders 
WHERE CustPONo='{ZPL.CustPONo}' AND StyleID='{ZPL.StyleID}'

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,o.CustCDID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
	AND p.ID='{packingListID}'
    AND pd.CustCTN='' 
    AND pd.OrderID = (SELECT ID FROM #tmpOrders{i})
    AND Article = '{ZPL.Article}'
    AND pd.ShipQty={ZPL.ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders{i}) AND SizeSpec IN ('{ZPL.SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{ZPL.SizeCode}'
        )
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN
UPDATE pd
SET pd.CustCTN='{ZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey


----3. 寫入ShippingMarkPic、ShippingMarkPic_Detail資料
IF NOT EXISTS( SELECT 1 FROM ShippingMarkPic WHERE PackingListID='{packingListID}')
BEGIN
	INSERT INTO ShippingMarkPic
		([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

	SELECT [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
	FROM ShippingMarkPicture s
	INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CustCD=t.CustCDID AND s.CTNRefno=t.RefNo AND s.Side='D'
	INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
END



INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=1 AND Side='D' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)



INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Seq=2 AND Side='D' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)

DROP TABLE #tmpOrders{i},#tmp{i}
";
                                }
                            }

                            i++;
                        }

                        updateCmd += Environment.NewLine + "---------" + Environment.NewLine + cmd;

                    }

                    // 成功狀況
                    using (TransactionScope transactionscope = new TransactionScope())
                    {
                        if (!(result = DBProxy.Current.Execute(null, updateCmd.ToString())))
                        {
                            transactionscope.Dispose();
                            this.canConvert = false;
                            this.ShowErr(result);
                        }
                        else
                        {
                            transactionscope.Complete();
                            transactionscope.Dispose();

                            //this._P25Dt.AsEnumerable().Where(o => o["FileName"].ToString() == fileName).FirstOrDefault()["Result"] = "Pass";

                            List<DataRow> dl = this._P25Dt.AsEnumerable().Where(o => fileNamess.Contains(o["FileName"].ToString())).ToList();//.FirstOrDefault()["Result"] = "Pass";
                            foreach (DataRow dr in dl)
                            {
                                dr["Result"] = "Pass";
                            }


                            this.canConvert = true;
                        }
                    }
                    #endregion
                    MyUtility.Msg.InfoBox("Mapping successful!");
                    this.Close();

                }

            }
            catch (Exception exp)
            {
                this.ShowErr(exp);
            }
        }

        /// <summary>
        /// 檢查User輸入的PL#是否正確
        /// </summary>
        private bool checkPackingList_Count(bool IsMixed,string CustPONo, string StyleID, string PackingListID, string Article, string CTNStartNo, string ShipQty, string SizeCode , List<SizeObject> Size_Qty_List)
        {
            string cmd = string.Empty;
            List<string> sqlMixed = new List<string>();
            DataTable[] tmpDts;

            if (IsMixed)
            {
                cmd = $@"

SELECT ID ,StyleID ,POID
INTO #tmoOrders
FROM Orders 
WHERE CustPONo='{CustPONo}' AND StyleID='{StyleID}'
";

                int i = 0;
                foreach (var data in Size_Qty_List)
                {
                    cmd += $@"

SELECT  CTNStartNo,[CartonCount]=COUNT(pd.Ukey)
INTO #tmpCount{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
WHERE p.Type ='B'
    AND p.Id='{PackingListID}'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders)
    AND pd.CustCTN='' 
    AND Article = '{Article}'
	AND ( SizeCode='{data.Size}' OR SizeCode in(
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders) AND SizeSpec IN ('{data.Size}')
	        ))  
	AND pd.ShipQty={data.Qty}
GROUP BY CTNStartNo

";
                    i++;
                }

                cmd += $@"
SELECT a.CTNStartNo,[CartonCount]=SUM(CartonCount) 
INTO #tmpMappingCartonNo
FROM (
";
                i = 0;
                foreach (var data in Size_Qty_List)
                {
                    sqlMixed.Add($@"
	SELECT  *
	FROM #tmpCount{i}
");
                    i++;
                }

                cmd += string.Join(" UNION ALL" + Environment.NewLine, sqlMixed);

                cmd += $@"

)a
GROUP BY CTNStartNo

----SQL檢查對應到幾個PackingList
SELECT [PackingListID]=pd.ID ,[PackingList_Ukey]=pd.Ukey
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
    AND p.Id='{PackingListID}'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders)
    AND pd.CustCTN='' 
    AND Article = '{Article}'
    AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo) 
	AND pd.SCICtnNo <> ''

----ShippingMarkPicture
SELECT DISTINCT o.BrandID ,o.CustCDID ,pd.RefNo
INTO #tmp
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
    AND p.Id='{PackingListID}'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders)
    AND pd.CustCTN='' 
    AND Article = '{Article}'
    AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo) 
	AND pd.SCICtnNo <> ''



SELECT IsSSCC
FROM ShippingMarkPicture s
INNER JOIN #tmp t ON s.BrandID=t.BrandID AND s.CustCD=t.CustCDID AND s.CTNRefno=t.RefNo AND s.Side='D'
WHERE IsSSCC=0
UNION 
SELECT IsSSCC
FROM ShippingMarkPicture s
INNER JOIN #tmp t ON s.BrandID=t.BrandID AND s.CustCD=t.CustCDID AND s.CTNRefno=t.RefNo AND s.Side='D'
WHERE IsSSCC=1

";
            }
            else
            {
                cmd = $@"


SELECT ID ,StyleID ,POID
INTO #tmpOrders0
FROM Orders 
WHERE CustPONo='{CustPONo}' AND StyleID='{StyleID}'

SELECT [PackingListID]=pd.ID ,[PackingList_Ukey]=pd.Ukey
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID = (SELECT ID FROM #tmpOrders0)
    AND pd.CustCTN = ''
	AND p.ID='{PackingListID}'
    AND Article = '{Article}'
    AND pd.ShipQty={ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders0) AND SizeSpec IN ('{SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{SizeCode}'
        )


SELECT DISTINCT o.BrandID ,o.CustCDID ,pd.RefNo
INTO #tmp
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type = 'B'
AND p.ID='{PackingListID}'
AND pd.OrderID = (SELECT ID FROM #tmpOrders)
AND pd.CustCTN = ''
AND Article = '{Article}'
AND pd.ShipQty ={ShipQty}
AND(
    pd.SizeCode in
    (
        SELECT SizeCode

        FROM Order_SizeSpec

        WHERE SizeItem = 'S01' AND ID IN(SELECT POID FROM #tmpOrders) AND SizeSpec IN ('{SizeCode}')
        )

    OR

    pd.SizeCode = '{SizeCode}'
)
		


SELECT IsSSCC
FROM ShippingMarkPicture s
INNER JOIN #tmp t ON s.BrandID=t.BrandID AND s.CustCD=t.CustCDID AND s.CTNRefno=t.RefNo AND s.Side='D'
WHERE IsSSCC = 0
UNION
SELECT IsSSCC
FROM ShippingMarkPicture s
INNER JOIN #tmp t ON s.BrandID=t.BrandID AND s.CustCD=t.CustCDID AND s.CTNRefno=t.RefNo AND s.Side='D'
WHERE IsSSCC = 1

DROP TABLE #tmpOrders ,#tmp
";
            }

            DBProxy.Current.Select(null, cmd, out tmpDts);
            return tmpDts[0].Rows.Count > 0 && tmpDts[1].Rows.Count == 2;

        }
    }
}
