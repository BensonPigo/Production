using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P31_ByCarton : Sci.Win.Forms.Base
    {

        private string _OrderID = "";
        private string _OrderShipmodeSeq = "";

        public P31_ByCarton(string OrderID , string OrderShipmodeSeq)
        {
            InitializeComponent();

            this._OrderID = OrderID;
            this._OrderShipmodeSeq = OrderShipmodeSeq;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DualResult res;
            DataTable dt;

            string cmd = $@"

----記錄哪些箱號有混尺碼
SELECT ID,OrderID,OrderShipmodeSeq,CTNStartNo
		,[ArticleCount]=COUNT(DISTINCT Article)
		,[SizeCodeCount]=COUNT(DISTINCT SizeCode)
INTO #MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID LIKE '{this._OrderID}' 
AND OrderShipmodeSeq ='{this._OrderShipmodeSeq}'
GROUP BY ID,OrderID,OrderShipmodeSeq,CTNStartNo
HAVING COUNT(DISTINCT Article) > 1 OR COUNT(DISTINCT SizeCode) > 1


----記錄混尺碼每個箱號的MAX(ReceiveDate)  --如果是NULL，後續沒有辦法比較，因此給個預設值代表NULL
SELECT pd.ID,pd.OrderID,pd.OrderShipmodeSeq,CTNStartNo,[ReceiveDate]=ISNULL(MAX(ReceiveDate),'1999/07/20')
INTO #Mix_MAX_ReceiveDate
FROM PackingList_Detail pd
WHERE pd.OrderID= '{this._OrderID}'
AND OrderShipmodeSeq = '{this._OrderShipmodeSeq}'
AND EXISTS(
	SELECT  1
	FROM #MixCTNStartNo t 
	WHERE t.ID = pd.ID AND t.OrderID = pd.OrderID 
	AND t.OrderShipmodeSeq=pd.OrderShipmodeSeq AND t.CTNStartNo=pd.CTNStartNo
)
GROUP BY pd.ID,pd.OrderID,CTNStartNo,pd.OrderShipmodeSeq



SELECT * FROM (
    ----不是混尺碼的正常做
    SELECT DISTINCT  pd.CTNStartNo
		    ,pd.Article 
		    ,pd.SizeCode 
			,[ShipQty]=SUM(pd.ShipQty)
		    ,[Result]=CASE WHEN Result.val=1 THEN 'Pass' WHEN Result.val=0 THEN 'Fail' ELSE '' END	
		    ,[ReceiveDate]=Max(pd.ReceiveDate)	
	FROM PackingList_Detail pd
    LEFT JOIN CFAInspectionRecord  CFA  ON pd.OrderID=cfa.OrderID AND pd.OrderShipmodeSeq = cfa.SEQ AND CFA.Carton = pd.CTNStartNo AND CFA.Status='Confirmed' 
    OUTER APPLY (
	    SELECT val=SUM(IIF(Result='Pass',1,0))
	    FROM CFAInspectionRecord cfa2 
	    WHERE OrderID=pd.OrderID AND Seq=pd.OrderShipmodeSeq AND Carton = pd.CTNStartNo AND Status='Confirmed' 
    )Result
	WHERE pd.OrderID= '{this._OrderID}'
	AND pd.OrderShipmodeSeq = '{this._OrderShipmodeSeq}'
	AND NOT EXISTS(
		SELECT  *  
		FROM #MixCTNStartNo t 
		WHERE t.ID = pd.ID AND t.OrderID = pd.OrderID 
		AND t.OrderShipmodeSeq=pd.OrderShipmodeSeq AND t.CTNStartNo=pd.CTNStartNo
	)
	GROUP BY CTNStartNo,Article ,SizeCode , Result.val
	UNION
    ----混尺碼分開處理
	SELECt t.CTNStartNo
		,[Article]=MixArticle.Val 
		,[SizeCode]=MixSizeCode.Val
		,[ShipQty]=ShipQty.Val
		,[Result]=Result.Result
		,[ReceiveDate]=IIF(t.ReceiveDate='1999/07/20',NULL,t.ReceiveDate)
	FROM #Mix_MAX_ReceiveDate t
	OUTER APPLY(
		SELECT  [Val]=  STUFF((
			SELECT DISTINCT ','+Article  
			FROM PackingList_Detail pd
			WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.OrderShipmodeSeq = t.OrderShipmodeSeq
			AND pd.CTNStartNo = t.CTNStartNo
			AND ISNULL(pd.ReceiveDate,'1999/07/20') = t.ReceiveDate
		FOR XML PATH(''))
		,1,1,'')
	)MixArticle
	OUTER APPLY(
		SELECT  [Val]=  STUFF((
			SELECT DISTINCT ','+SizeCode  
			FROM PackingList_Detail pd
			WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.OrderShipmodeSeq = t.OrderShipmodeSeq
			AND pd.CTNStartNo = t.CTNStartNo
			AND ISNULL(pd.ReceiveDate,'1999/07/20') = t.ReceiveDate   --要把預設值換回NULL
		FOR XML PATH(''))
		,1,1,'')
	)MixSizeCode
	OUTER APPLY(
		SELECT  [Val]=SUM(pd.ShipQty)
		FROM PackingList_Detail pd
		WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.OrderShipmodeSeq = t.OrderShipmodeSeq
			AND pd.CTNStartNo = t.CTNStartNo
			AND ISNULL(pd.ReceiveDate,'1999/07/20') = t.ReceiveDate
	)ShipQty
	OUTER APPLY(
		SELECT TOP 1 [Result]=CASE WHEN Result.val=1 THEN 'Pass' WHEN Result.val=0 THEN 'Fail' ELSE '' END	
		FROM PackingList_Detail pd
        LEFT JOIN CFAInspectionRecord  CFA  ON pd.OrderID=cfa.OrderID AND pd.OrderShipmodeSeq = cfa.SEQ AND CFA.Carton = pd.CTNStartNo AND CFA.Status='Confirmed' 
        OUTER APPLY (
	        SELECT val=SUM(IIF(Result='Pass',1,0))
	        FROM CFAInspectionRecord cfa2 
	        WHERE OrderID=pd.OrderID AND Seq=pd.OrderShipmodeSeq AND Carton = pd.CTNStartNo AND Status='Confirmed' 
        )Result
		WHERE pd.ID = t.ID 
		AND pd.OrderID = t.OrderID 
			AND pd.OrderShipmodeSeq = t.OrderShipmodeSeq
		AND pd.CTNStartNo = t.CTNStartNo
			AND ISNULL(pd.ReceiveDate,'1999/07/20') = t.ReceiveDate
	)Result
) a
ORDER BY Cast(CTNStartNo as int)


DROP TABLE #MixCTNStartNo ,#Mix_MAX_ReceiveDate
";

            res = DBProxy.Current.Select(null, cmd, out dt);
            if (!res)
            {
                this.ShowErr(res);
            }
            this.listControlBindingSource.DataSource = dt;

            this.grid.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid)
                 .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Article", header: "Article", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("SizeCode", header: "Size", width: Widths.AnsiChars(25), iseditingreadonly: true)
                 .Numeric("ShipQty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 0, iseditingreadonly: true)
                 .Text("Result", header: "Staggered Result", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Date("ReceiveDate", header: "CLOG"+Environment.NewLine+ "received", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 ;

        }
    }
}
