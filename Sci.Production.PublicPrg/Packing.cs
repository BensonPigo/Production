using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sci.Data;
using Sci;
using Ict;
using Ict.Win;

namespace Sci.Production.PublicPrg
{
    public static partial class Prgs
    {
        #region CreateOrderCTNData
        /// <summary>
        /// CreateOrderCTNData(string)
        /// </summary>
        /// <param name="packingListID"></param>
        /// <returns>bool</returns>
        public static bool CreateOrderCTNData(string packingListID)
        {
            string sqlCmd;

            sqlCmd = string.Format(@"--宣告變數
DECLARE @packinglistid VARCHAR(13),
		@addname VARCHAR(10)
SET @packinglistid = '{0}'
SET @addname = '{1}'

--建立tmpe table存放要全部的資料
DECLARE @tempAllData TABLE (
   ID VARCHAR(13),
   RefNo VARCHAR(20)
)

--宣告變數
DECLARE @orderid VARCHAR(13),
		@refno VARCHAR(20),
		@qtyperctn SMALLINT,
		@gmtqty INT,
		@ctnqty INT,
		@reccount INT

SET XACT_ABORT ON;
BEGIN TRANSACTION
--Insert & Update資料
DECLARE cursor_PackingListGroup CURSOR FOR
	SELECT OrderID AS ID, RefNo, MAX(QtyPerCTN) AS QtyPerCTN, SUM(ShipQty) AS GMTQty, SUM(CTNQty) AS CTNQty 
	FROM PackingList_Detail 
	WHERE OrderID in (SELECT Distinct OrderID FROM PackingList_Detail WHERE ID = @packinglistid) GROUP BY OrderID, RefNo
OPEN cursor_PackingListGroup
FETCH NEXT FROM cursor_PackingListGroup INTO @orderid,@refno,@qtyperctn,@gmtqty,@ctnqty
WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @reccount = COUNT(ID) FROM Order_CTNData WHERE ID = @orderid AND RefNo = @refno
	IF @reccount = 0
		BEGIN
			INSERT INTO Order_CTNData(ID,RefNo,QtyPerCTN,GMTQty,CTNQty,AddName,AddDate)
				VALUES (@orderid,@refno,@qtyperctn,@gmtqty,@ctnqty,@addname, GETDATE())
		END
	ELSE
		BEGIN
			UPDATE Order_CTNData
			SET QtyPerCTN = @qtyperctn,
				GMTQty = @gmtqty,
				CTNQty = @ctnqty,
				EditName = @addname,
				EditDate = GETDATE()
			WHERE ID = @orderid AND RefNo = @refno
		END

	INSERT INTO @tempAllData (ID,RefNo)
		VALUES (@orderid,@refno)
	FETCH NEXT FROM cursor_PackingListGroup INTO @orderid,@refno,@qtyperctn,@gmtqty,@ctnqty
END

--關閉cursor與參數的關聯
CLOSE cursor_PackingListGroup
--將cursor物件從記憶體移除
DEALLOCATE cursor_PackingListGroup

--Delete Data
DECLARE cursor_DeleteData CURSOR FOR
	SELECT ID,RefNo FROM @tempAllData
OPEN cursor_DeleteData
FETCH NEXT FROM cursor_DeleteData INTO @orderid,@refno
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM Order_CTNData 
	WHERE ID = @orderid AND RefNo in (SELECT RefNo 
									  FROM (SELECT distinct ocd.RefNo as RefNo, tad.RefNo as nRefNo 
											FROM Order_CTNData ocd 
											LEFT JOIN @tempAllData tad ON tad.ID = ocd.ID AND tad.RefNo = ocd.RefNo
											WHERE ocd.ID = @orderid) a
									  WHERE a.nRefNo IS NULL)
	FETCH NEXT FROM cursor_DeleteData INTO @orderid,@refno
END
CLOSE cursor_DeleteData
DEALLOCATE cursor_DeleteData

IF @@ERROR <> 0
	ROLLBACK TRANSACTION
ELSE
	COMMIT TRANSACTION", packingListID, Sci.Env.User.UserID);
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
