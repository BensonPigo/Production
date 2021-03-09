using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using Sci.Win.UI;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using System.Net;
using Sci.Win.Tools;
using Sci.Utility.Excel;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static partial class Prgs
    {
        #region CreateOrderCTNData

        /// <summary>
        /// CreateOrderCTNData(string)
        /// </summary>
        /// <param name="packingListID">PackingList ID</param>
        /// <returns>bool</returns>
        public static DualResult CreateOrderCTNData(string packingListID)
        {
            string sqlCmd;

            sqlCmd = string.Format(
                @"--宣告變數
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
	FROM PackingList_Detail WITH (NOLOCK) 
	WHERE OrderID in (SELECT Distinct OrderID FROM PackingList_Detail WITH (NOLOCK) WHERE ID = @packinglistid) GROUP BY OrderID, RefNo
OPEN cursor_PackingListGroup
FETCH NEXT FROM cursor_PackingListGroup INTO @orderid,@refno,@qtyperctn,@gmtqty,@ctnqty
WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @reccount = COUNT(ID) FROM Order_CTNData WITH (NOLOCK) WHERE ID = @orderid AND RefNo = @refno
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
	COMMIT TRANSACTION",
                packingListID,
                Env.User.UserID);
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            return result;
        }
        #endregion

        #region CheckPulloutComplete

        /// <summary>
        /// CheckPulloutComplete(string,string,string,string,int)
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <param name="orderShipmodeSeq">orderShipmodeSeq</param>
        /// <param name="article">article</param>
        /// <param name="sizeCode">sizeCode</param>
        /// <param name="packingQty">packingQty</param>
        /// <returns>bool</returns>
        public static bool CheckPulloutComplete(string orderID, string orderShipmodeSeq, string article, string sizeCode, int packingQty)
        {
            DataTable qty;
            DualResult result;
            string sqlCmd = $@"with PulloutQty
as
(select isnull(sum(pdd.ShipQty),0) as ShipQty
 from Pullout p WITH (NOLOCK) , Pullout_Detail pd WITH (NOLOCK) , Pullout_Detail_Detail pdd WITH (NOLOCK) 
 where p.Status != 'New'
 and p.id = pd.ID
 and pd.OrderID = '{orderID}'
 and pd.OrderShipmodeSeq = '{orderShipmodeSeq}'
 and p.ID = pdd.ID
 and pd.UKey = pdd.Pullout_DetailUKey
 and pdd.Article = '{article}'
 and pdd.SizeCode = '{sizeCode}'
),
InvadjQty
as
(select isnull(sum(iaq.DiffQty),0) as DiffQty
 from InvAdjust ia WITH (NOLOCK) , InvAdjust_Qty iaq WITH (NOLOCK) 
 where ia.OrderID = '{orderID}'
 and ia.OrderShipmodeSeq = '{orderShipmodeSeq}'
 and ia.ID = iaq.ID
 and iaq.Article = '{article}'
 and iaq.SizeCode = '{sizeCode}'
)

select isnull(oqd.Qty,0) as OrderQty,(select ShipQty from PulloutQty)+(select DiffQty from InvadjQty) as ShipQty
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
where oqd.Id = '{orderID}'
and oqd.Seq = '{orderShipmodeSeq}'
and oqd.Article = '{article}'
and oqd.SizeCode = '{sizeCode}'";

            result = DBProxy.Current.Select(null, sqlCmd, out qty);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query ship qty fail!");
                return false;
            }
            else
            {
                if (qty.Rows.Count <= 0)
                {
                    return false;
                }
                else
                {
                    return MyUtility.Convert.GetInt(qty.Rows[0]["OrderQty"]) >= MyUtility.Convert.GetInt(qty.Rows[0]["ShipQty"]) + packingQty;
                }
            }
        }
        #endregion

        #region Recaluate Carton Weight

        /// <summary>
        /// RecaluateCartonWeight(Datatable,DataRow)
        /// </summary>
        /// <param name="packingListDetaildata">packingList Detail data</param>
        /// <param name="packingListData">packingList Data</param>
        public static void RecaluateCartonWeight(DataTable packingListDetaildata, DataRow packingListData)
        {
            // PackingListDetaildata.Rows.Count <= 0)
            if (packingListDetaildata.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted).Count() == 0)
            {
                return;
            }

            DataTable weightData, tmpWeightData;
            DataRow[] weight;
            string message = string.Empty;
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, "select '' as BrandID, '' as StyleID, '' as SeasonID, Article, SizeCode, NW, NNW from Style_WeightData WITH (NOLOCK) where StyleUkey = ''", out weightData)))
            {
                MyUtility.Msg.WarningBox("Query 'weightData' schema fail!");
                return;
            }

            // 檢查是否所有的SizeCode都有存在Style_WeightData中
            foreach (DataRow dr in packingListDetaildata.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                string filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}'", packingListData["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length == 0)
                {
                    // 先將屬於此訂單的Style_WeightData給撈出來
                    string sqlCmd = $@"select a.ID as StyleID,a.BrandID,a.SeasonID,isnull(b.Article,'') as Article,isnull(b.SizeCode,'') as SizeCode,isnull(b.NW,0) as NW,isnull(b.NNW,0) as NNW
from Style a WITH (NOLOCK) 
left join Style_WeightData b WITH (NOLOCK) on b.StyleUkey = a.Ukey
where a.ID = '{dr["StyleID"].ToString()}' and a.BrandID = '{packingListData["BrandID"].ToString()}' and a.SeasonID = '{dr["SeasonID"].ToString()}'";
                    if (!(result = DBProxy.Current.Select(null, sqlCmd, out tmpWeightData)))
                    {
                        MyUtility.Msg.WarningBox("Query weight data fail!");
                        return;
                    }
                    else
                    {
                        foreach (DataRow tpd in tmpWeightData.Rows)
                        {
                            tpd.AcceptChanges();
                            tpd.SetAdded();
                            weightData.ImportRow(tpd);
                        }
                    }
                }

                filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}' and SizeCode = '{3}'", packingListData["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString(), dr["SizeCode"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length == 0)
                {
                    if (message.IndexOf("SP#:" + dr["OrderID"].ToString() + ", Size:" + dr["SizeCode"].ToString()) <= 0)
                    {
                        message = message + "SP#:" + dr["OrderID"].ToString() + ", Size:" + dr["SizeCode"].ToString() + "\r\n";
                    }
                }
            }

            if (!MyUtility.Check.Empty(message))
            {
                DialogResult buttonResult = MyUtility.Msg.WarningBox(message + " not in Style basic data, are you sure you want to  recalculate weight?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == DialogResult.No)
                {
                    return;
                }
            }

            double nw = 0, nnw = 0, ctnWeight = 0;
            string localItemWeight;
            DataTable tmpPacklistWeight;
            result = DBProxy.Current.Select(null, "select CTNStartNo, NW, NNW, GW from PackingList_Detail WITH (NOLOCK) where 1=0", out tmpPacklistWeight);
            DataRow tmpPacklistRow;

            string ctnNo = string.Empty;
            foreach (DataRow dr in packingListDetaildata.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    ctnNo = dr["CTNStartNo"].ToString();
                    break;
                }
            }

            // 依CtnStart#排序 來計算混尺碼重量
            if (packingListDetaildata.Columns.Contains("tmpKey") == false)
            {
                packingListDetaildata.Columns.Add("tmpKey", typeof(decimal));
            }

            int tmpkey = 0;
            foreach (DataRow dr in packingListDetaildata.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                dr["tmpKey"] = tmpkey;
                tmpkey++;
            }

            packingListDetaildata.DefaultView.Sort = "CTNStartNo,CTNQty desc";
            DataTable dtSort = packingListDetaildata.DefaultView.ToTable();
            foreach (DataRow dr in dtSort.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                if (!MyUtility.Check.Empty(dr["CTNQty"]) || ctnNo != dr["CTNStartNo"].ToString())
                {
                    if (ctnNo != dr["CTNStartNo"].ToString())
                    {
                        tmpPacklistRow = tmpPacklistWeight.NewRow();
                        tmpPacklistRow["CTNStartNo"] = ctnNo;
                        tmpPacklistRow["NW"] = nw;
                        tmpPacklistRow["NNW"] = nnw;
                        tmpPacklistRow["GW"] = nw + ctnWeight;
                        tmpPacklistWeight.Rows.Add(tmpPacklistRow);
                    }

                    ctnNo = dr["CTNStartNo"].ToString();
                    nw = 0;
                    nnw = 0;
                    localItemWeight = MyUtility.GetValue.Lookup("CtnWeight", MyUtility.Convert.GetString(dr["RefNo"]), "LocalItem", "RefNo");
                    ctnWeight = MyUtility.Math.Round(MyUtility.Convert.GetDouble(localItemWeight), 6);
                }

                string filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}' and Article = '{3}' and SizeCode = '{4}'", packingListData["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length > 0)
                {
                    nw = nw + (MyUtility.Convert.GetDouble(weight[0]["NW"]) * MyUtility.Convert.GetInt(dr["ShipQty"]));
                    nnw = nnw + (MyUtility.Convert.GetDouble(weight[0]["NNW"]) * MyUtility.Convert.GetInt(dr["ShipQty"]));
                    dr["NWPerPcs"] = MyUtility.Convert.GetDouble(weight[0]["NW"]);
                }
                else
                {
                    filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}' and SizeCode = '{3}'", packingListData["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString(), dr["SizeCode"].ToString());
                    weight = weightData.Select(filter);
                    if (weight.Length > 0)
                    {
                        nw = nw + (MyUtility.Convert.GetDouble(weight[0]["NW"]) * MyUtility.Convert.GetInt(dr["ShipQty"]));
                        nnw = nnw + (MyUtility.Convert.GetDouble(weight[0]["NNW"]) * MyUtility.Convert.GetInt(dr["ShipQty"]));
                        dr["NWPerPcs"] = MyUtility.Convert.GetDouble(weight[0]["NW"]);
                    }
                    else
                    {
                        dr["NWPerPcs"] = 0;
                    }
                }

                packingListDetaildata.Select($"tmpkey = {dr["tmpkey"]}")[0]["NWPerPcs"] = dr["NWPerPcs"];
            }

            if (packingListDetaildata.Columns.Contains("tmpKey") == true)
            {
                packingListDetaildata.Columns.Remove("tmpKey");
            }

            // 最後一筆資料也要寫入
            tmpPacklistRow = tmpPacklistWeight.NewRow();
            tmpPacklistRow["CTNStartNo"] = ctnNo;
            tmpPacklistRow["NW"] = nw;
            tmpPacklistRow["NNW"] = nnw;
            tmpPacklistRow["GW"] = nw + ctnWeight;
            tmpPacklistWeight.Rows.Add(tmpPacklistRow);

            // 將整箱重量回寫回表身Grid中CTNQty> 0的資料中
            foreach (DataRow dr in tmpPacklistWeight.Rows)
            {
                foreach (DataRow dr1 in packingListDetaildata.Rows)
                {
                    if (dr1.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (dr["CTNStartNo"].ToString() == dr1["CTNStartNo"].ToString())
                    {
                        if (!MyUtility.Check.Empty(dr1["CTNQty"]))
                        {
                            dr1["NW"] = dr["NW"];
                            dr1["NNW"] = dr["NNW"];
                            dr1["GW"] = dr["GW"];
                        }
                        else
                        {
                            dr1["NW"] = 0;
                            dr1["NNW"] = 0;
                            dr1["GW"] = 0;
                        }
                    }
                }
            }
        }
        #endregion

        #region Check Pullout Qty can't exceed Order Qty

        /// <summary>
        /// CheckPulloutQtyWithOrderQty(string)
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <returns>bool</returns>
        public static bool CheckPulloutQtyWithOrderQty(string packingListID)
        {
            string sqlCmd = string.Format(
                @"select OrderID,OrderShipmodeSeq,Article,SizeCode,sum(ShipQty) as ShipQty 
from PackingList_Detail WITH (NOLOCK) 
where ID = '{0}'
group by OrderID,OrderShipmodeSeq,Article,SizeCode", packingListID);
            DataTable queryData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out queryData);
            if (result)
            {
                string errMesg = string.Empty;
                foreach (DataRow dr in queryData.Rows)
                {
                    if (!CheckPulloutComplete(dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString(), MyUtility.Convert.GetInt(dr["ShipQty"])))
                    {
                        errMesg = errMesg + "SP No.:" + dr["OrderID"].ToString() + "Color Way: " + dr["Article"].ToString() + ", Size: " + dr["SizeCode"].ToString() + "\r\n";
                    }
                }

                if (!MyUtility.Check.Empty(errMesg))
                {
                    MyUtility.Msg.WarningBox("Pullout qty is more than order qty!\n\r" + errMesg);
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Query packinglist fail!");
                return false;
            }

            return true;
        }
        #endregion

        #region Check Packing Qty can't exceed Sewing Output Qty

        /// <summary>
        /// CheckPackingQtyWithSewingOutput(string)
        /// </summary>
        /// <param name="packingListID">string</param>
        /// <returns>bool</returns>
        public static bool CheckPackingQtyWithSewingOutput(string packingListID)
        {
            string sqlCmd = string.Format(
                @"with PackOrderID
as
(select distinct pld.OrderID
 from PackingList pl WITH (NOLOCK) , PackingList_Detail pld WITH (NOLOCK) 
 where pl.ID = '{0}'
 and pld.ID = pl.ID
),
PackedData
as
(select pld.OrderID,pld.Article,pld.SizeCode,sum(pld.ShipQty) as PackedShipQty
 from PackingList pl WITH (NOLOCK) , PackingList_Detail pld WITH (NOLOCK) , PackOrderID poid 
 where pld.OrderID = poid.OrderID
 and pl.ID = pld.ID
 and pl.Status = 'Confirmed'
 group by pld.OrderID,pld.Article,pld.SizeCode
),
PackingData
as
(select pld.OrderID,pld.Article,pld.SizeCode,sum(pld.ShipQty) as ShipQty
 from PackingList_Detail pld WITH (NOLOCK) 
 where pld.ID = '{0}'
 group by pld.OrderID,pld.Article,pld.SizeCode
),
InvadjQty
as
(select ia.OrderID,iaq.Article, iaq.SizeCode,sum(iaq.DiffQty) as DiffQty
 from InvAdjust ia WITH (NOLOCK) , InvAdjust_Qty iaq WITH (NOLOCK) , PackOrderID poid
 where ia.OrderID = poid.OrderID
 and ia.ID = iaq.ID
 group by ia.OrderID,iaq.Article, iaq.SizeCode
),
SewingData
as
(select a.OrderID,a.Article,a.SizeCode,MIN(a.QAQty) as QAQty
 from (select poid.OrderID,oq.Article,oq.SizeCode, sl.Location, isnull(sum(sodd.QAQty),0) as QAQty
	   from PackOrderID poid
	   left join Orders o WITH (NOLOCK) on o.ID = poid.OrderID
	   left join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
	   left join Order_Location sl WITH (NOLOCK) on sl.OrderID = o.ID
	   left join SewingOutput_Detail_Detail sodd WITH (NOLOCK) on sodd.OrderId = o.ID and sodd.Article = oq.Article  and sodd.SizeCode = oq.SizeCode and sodd.ComboType = sl.Location
	   group by poid.OrderID,oq.Article,oq.SizeCode, sl.Location) a
 group by a.OrderID,a.Article,a.SizeCode
)
select poid.OrderID,isnull(oq.Article,'') as Article,isnull(oq.SizeCode,'') as SizeCode, isnull(oq.Qty,0) as Qty, isnull(pedd.PackedShipQty,0)+isnull(pingd.ShipQty,0) as PackQty,isnull(iq.DiffQty,0) as DiffQty, isnull(sd.QAQty,0) as QAQty
from PackOrderID poid
left join Order_Qty oq WITH (NOLOCK) on oq.ID = poid.OrderID
left join PackedData pedd on pedd.OrderID = poid.OrderID and pedd.Article = oq.Article and pedd.SizeCode = oq.SizeCode
left join PackingData pingd on  pingd.OrderID = poid.OrderID and pingd.Article = oq.Article and pingd.SizeCode = oq.SizeCode
left join InvadjQty iq on  iq.OrderID = poid.OrderID and iq.Article = oq.Article and iq.SizeCode = oq.SizeCode
left join SewingData sd on  sd.OrderID = poid.OrderID and sd.Article = oq.Article and sd.SizeCode = oq.SizeCode
order by poid.OrderID,oq.Article,oq.SizeCode", packingListID);
            DataTable queryData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out queryData);
            if (result)
            {
                StringBuilder errMesg = new StringBuilder();
                foreach (DataRow dr in queryData.Rows)
                {
                    if (!MyUtility.Check.Empty(dr["PackQty"]))
                    {
                        if (MyUtility.Convert.GetInt(dr["PackQty"]) + MyUtility.Convert.GetInt(dr["DiffQty"]) > MyUtility.Convert.GetInt(dr["QAQty"]))
                        {
                            errMesg.Append("SP No.:" + MyUtility.Convert.GetString(dr["OrderID"]) + "Color Way: " + MyUtility.Convert.GetString(dr["Article"]) + ", Size: " + MyUtility.Convert.GetString(dr["SizeCode"]) + ", Qty: " + MyUtility.Convert.GetString(dr["Qty"]) + ", Ship Qty: " + MyUtility.Convert.GetString(dr["PackQty"]) + ", Sewing Qty:" + MyUtility.Convert.GetString(dr["QAQty"]) + (MyUtility.Check.Empty(dr["DiffQty"]) ? string.Empty : ", Adj Qty:" + MyUtility.Convert.GetString(dr["DiffQty"])) + "." + (MyUtility.Convert.GetInt(dr["PackQty"]) + MyUtility.Convert.GetInt(dr["DiffQty"]) > MyUtility.Convert.GetInt(dr["Qty"]) ? "   Pullout qty can't exceed order qty," : string.Empty) + " Pullout qty can't exceed sewing qty.\r\n");
                        }
                    }
                }

                if (errMesg.Length > 0)
                {
                    MyUtility.Msg.WarningBox(errMesg.ToString());
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Query sewing fail!");
                return false;
            }

            return true;
        }
        #endregion

        #region Query Packing List SQL Command

        /// <summary>
        /// QueryPackingListSQLCmd(string)
        /// </summary>
        /// <param name="packingListID">string packingListID</param>
        /// <returns>string</returns>
        public static string QueryPackingListSQLCmd(string packingListID)
        {
            return string.Format(
                @"
with AllOrderID as (
    select  Distinct OrderID
            , OrderShipmodeSeq
    from PackingList_Detail WITH (NOLOCK) 
    where ID = '{0}'
), 
AccuPKQty as (
    select  pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.SizeCode
            , sum(pd.ShipQty) as TtlShipQty
    from     PackingList_Detail pd WITH (NOLOCK) 
            , AllOrderID a
    where   ID != '{0}'
            and a.OrderID = pd.OrderID
            and a.OrderShipmodeSeq = pd.OrderShipmodeSeq
    group by pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.SizeCode
),
PulloutAdjQty as (
    select  ia.OrderID
            , ia.OrderShipmodeSeq
            , iaq.Article
            , iaq.SizeCode
            , sum(iaq.DiffQty) as TtlDiffQty
    from    InvAdjust ia WITH (NOLOCK) 
            , InvAdjust_Qty iaq WITH (NOLOCK) 
            , AllOrderID a
    where   ia.OrderID = a.OrderID
            and ia.OrderShipmodeSeq = a.OrderShipmodeSeq
            and ia.ID = iaq.ID
    group by ia.OrderID, ia.OrderShipmodeSeq, iaq.Article, iaq.SizeCode
),
PackQty as (
    select  OrderID
            , OrderShipmodeSeq
            , Article
            , SizeCode
            , sum(ShipQty) as ShipQty
    from    PackingList_Detail WITH (NOLOCK) 
    where   ID = '{0}'
    group by OrderID, OrderShipmodeSeq, Article, SizeCode
)
select  a.*
        , b.Description
        , isnull(oqd.Qty,0)-isnull(pd.TtlShipQty,0)+isnull(paq.TtlDiffQty,0)-pk.ShipQty as BalanceQty
        , o.StyleID
        , o.CustPONo
        , o.SeasonID
        , Factory = STUFF(( select CONCAT(',', FactoryID) 
                            from ( select distinct FactoryID 
                                   from orders o 
                                   where o.id in (a.OrderID)) s 
                            for xml path('')
                          ),1,1,'')
        , sortCTNNo = TRY_Convert(int , a.CTNStartNo)
        , sciDelivery = min(o.sciDelivery) over()
        , kpileta = min(o.kpileta) over()
        ,[Cancel] = (SELECT [Cancel] = CASE WHEN count(*) > 0 THEN 'Y' ELSE 'N' END
                        FROM  orders o 						
                        WHERE o.Junk = 1 AND a.OrderID=o.ID)
        ,o.CustCDID
        ,o.Dest
        ,o.OrderTypeID
        ,oqs.IDD
        ,[PrepackQtyShow] = IIF(a.PrepackQty = 0 , NULL , a.PrepackQty)
from PackingList_Detail a WITH (NOLOCK) 
left join LocalItem b WITH (NOLOCK) on b.RefNo = a.RefNo
left join AccuPKQty pd on a.OrderID = pd.OrderID 
                          and a.OrderShipmodeSeq = pd.OrderShipmodeSeq 
                          and pd.Article = a.Article 
                          and pd.SizeCode = a.SizeCode
left join PulloutAdjQty paq on a.OrderID = paq.OrderID 
                               and a.OrderShipmodeSeq = paq.OrderShipmodeSeq 
                               and paq.Article = a.Article 
                               and paq.SizeCode = a.SizeCode
left join PackQty pk on a.OrderID = pk.OrderID 
                        and a.OrderShipmodeSeq = pk.OrderShipmodeSeq 
                        and pk.Article = a.Article 
                        and pk.SizeCode = a.SizeCode
left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = a.OrderID 
                                                    and oqd.Seq = a.OrderShipmodeSeq 
                                                    and oqd.Article = a.Article 
                                                    and oqd.SizeCode = a.SizeCode
left join Order_QtyShip oqs with (nolock) on        oqs.ID  = a.OrderID and
                                                    oqs.Seq = a.OrderShipmodeSeq 
left join Orders o WITH (NOLOCK) on o.ID = a.OrderID
where a.id = '{0}'
order by a.Seq ASC,a.CTNQty DESC", packingListID);
        }
        #endregion

        /// <summary>
        /// Check Exists Order_QtyShip_Detail
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <param name="iNVNo">iNVNo</param>
        /// <param name="shipPlanID">shipPlanID</param>
        /// <param name="pulloutID">pulloutID</param>
        /// <param name="showmsg">showmsg</param>
        /// <returns>DualResult</returns>
        public static DualResult CheckExistsOrder_QtyShip_Detail(string packingListID = "", string iNVNo = "", string shipPlanID = "", string pulloutID = "", bool showmsg = true)
        {
            string where = string.Empty;
            if (!MyUtility.Check.Empty(packingListID))
            {
                where = $@"and p.id ='{packingListID}'";
            }
            else if (!MyUtility.Check.Empty(iNVNo))
            {
                where = $@"and p.INVNo ='{iNVNo}'";
            }
            else if (!MyUtility.Check.Empty(shipPlanID))
            {
                where = $@"and p.ShipPlanID ='{shipPlanID}'";
            }
            else if (!MyUtility.Check.Empty(pulloutID))
            {
                where = $@"and p.PulloutID ='{pulloutID}'";
            }

            string sqlCmd = $@"
select pd.OrderID,pd.OrderShipmodeSeq,pd.Article,pd.SizeCode,ShipQty=sum(pd.ShipQty)
into #tmpPacking
from(
	select distinct pd.OrderID,pd.OrderShipmodeSeq
	from PackingList p with(nolock)
	inner join PackingList_detail pd with(nolock) on p.id = pd.id
	where 1=1 {where}
)x
inner  join PackingList_detail pd with(nolock) on pd.OrderID = x.OrderID and pd.OrderShipmodeSeq =x.OrderShipmodeSeq
group by pd.OrderID,pd.OrderShipmodeSeq,pd.Article,pd.SizeCode

select oqd.ID,oqd.Seq,oqd.Article,oqd.SizeCode,oqd.Qty
into #tmpOrderShip
from(
	select distinct pd.OrderID,pd.OrderShipmodeSeq
	from PackingList p with(nolock)
	inner join PackingList_detail pd with(nolock) on p.id = pd.id
	where 1=1 {where}
)x
inner join Order_QtyShip_Detail oqd with(nolock) on oqd.ID = x.OrderID and oqd.Seq =x.OrderShipmodeSeq

----先將台北調整GROUP BY加總
SELECT  i.orderid ,iq.Article ,iq.SizeCode ,i.OrderShipmodeSeq ,[DiffQty]=SUM(ISNULL(DiffQty,0) )
INTO #TPEAdjust
FROM InvAdjust i WITH (NOLOCK)
INNER JOIN InvAdjust_Qty iq WITH (NOLOCK)  ON  iq.ID = i.id 
WHERE EXISTS(
	select oqd.ID, oqd.Article , oqd.SizeCode
	from #tmpOrderShip oqd
	WHERE i.orderid = oqd.Id AND iq.Article= oqd.Article AND iq.SizeCode= oqd.SizeCode AND i.OrderShipmodeSeq = oqd.Seq 
)
GROUP BY i.orderid ,iq.Article ,iq.SizeCode ,i.OrderShipmodeSeq
";
            string sqlA = sqlCmd + $@"
select distinct msg = concat(p.OrderID, ' (', p.OrderShipmodeSeq, ')',' - ', p.Article,' - ',p.SizeCode,' - Ship Qty:',sum(isnull(p.ShipQty,0) + ISNULL(t.DiffQty,0)),' - Order Qty : ',SUM(isnull(oqd.Qty,0)))
from #tmpPacking p
left join #tmpOrderShip oqd with(nolock) on oqd.id = p.OrderID and oqd.Seq = p.OrderShipmodeSeq and p.Article = oqd.Article and p.SizeCode = oqd.SizeCode
lEFT JOIN #TPEAdjust t ON t.OrderID= oqd.id AND t.OrderShipmodeSeq = oqd.Seq AND t.Article=oqd.Article AND t.SizeCode=oqd.SizeCode  ----出貨數必須加上台北端財務可能調整出貨數量，因此必須納入考量(若是減少則是負數，因此用加法即可)
where isnull(p.ShipQty,0) + ISNULL(t.DiffQty,0) > isnull(oqd.Qty,0)
GROUP BY p.OrderID,p.OrderShipmodeSeq,p.Article,p.SizeCode
";
            string sqlB = sqlCmd + $@"
select distinct msg = concat(p.OrderID, ' (', p.OrderShipmodeSeq, ')',' - ', p.Article,' - ',p.SizeCode,' - Ship Qty:',sum(isnull(p.ShipQty,0) + ISNULL(t.DiffQty,0)),' - Order Qty : ',SUM(isnull(oqd.Qty,0)))
from #tmpOrderShip oqd
left join #tmpPacking p with(nolock) on oqd.id = p.OrderID and oqd.Seq = p.OrderShipmodeSeq and p.Article = oqd.Article and p.SizeCode = oqd.SizeCode
lEFT JOIN #TPEAdjust t ON t.OrderID= oqd.id AND t.OrderShipmodeSeq = oqd.Seq AND t.Article=oqd.Article AND t.SizeCode=oqd.SizeCode  ----出貨數必須加上台北端財務可能調整出貨數量，因此必須納入考量(若是減少則是負數，因此用加法即可)
where isnull(p.ShipQty,0) + ISNULL(t.DiffQty,0) < isnull(oqd.Qty,0)
GROUP BY p.OrderID,p.OrderShipmodeSeq,p.Article,p.SizeCode
";

            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlA, out dt);
            if (!result)
            {
                if (showmsg)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                }

                return result;
            }

            // 不允許 Confirm
            if (dt.Rows.Count > 0)
            {
                var os = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["msg"])).ToList();
                string msg = @"Ship Qty>Order Qty, please check Q'ty Breakdown by Shipmode (Seq).
" + string.Join("\t", os);

                if (showmsg)
                {
                    MyUtility.Msg.WarningBox(msg);
                }

                return Ict.Result.F(msg);
            }

            result = DBProxy.Current.Select(null, sqlB, out dt);
            if (!result)
            {
                if (showmsg)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                }

                return result;
            }

            // 僅提示允許繼續 Confirm
            if (dt.Rows.Count > 0)
            {
                var os = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["msg"])).ToList();
                string msg = @"Ship Qty<Order Qty, please be sure this is Short Shipment before Save/Confirm the Packing List.
" + string.Join("\t", os);

                MyUtility.Msg.WarningBox(msg);
                return Ict.Result.True;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// Is Cancel Order
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <returns>bool</returns>
        public static bool IsCancelOrder(string orderID)
        {
            return MyUtility.Check.Seek($"select 1 from orders with (nolock) where id = '{orderID}' and Junk = 1");
        }

        /// <summary>
        /// Compare Order PackingQty
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <param name="plID">plID</param>
        /// <param name="curAddPlQty">curAddPlQty</param>
        /// <returns>DualResult</returns>
        public static DualResult CompareOrderQtyPackingQty(string orderID, string plID, int curAddPlQty)
        {
            // ISP20200981 只要訂單為 Cancel 則跳過 Packing 與訂單總量比對的判斷
            if (IsCancelOrder(orderID))
            {
                return new DualResult(true);
            }

            string sqlGetData = $@"
----1. 除了目前正在編輯的 PL 以外其他的 PL
select  p.ID, [ShipQty] = isnull(sum(isnull(pd.ShipQty,0)),0)
into #tmpPL
from PackingList p with (nolock)
inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
where pd.OrderID = '{orderID}' and p.ID <> '{plID}'
group by p.ID

----2. 台北調整的數量
SELECT  i.OrderID  ,[DiffQty]=SUM(ISNULL(iq.DiffQty,0) )
INTO #TPEAdjust
FROM InvAdjust i WITH (NOLOCK)
INNER JOIN InvAdjust_Qty iq WITH (NOLOCK)  ON  iq.ID = i.id 
WHERE i.OrderID = '{orderID}'
GROUP BY i.OrderID

----3. 訂單總數量 / Packing 總數量 - 台北調整數量
select Qty ,[PLQty] = (select isnull(sum(isnull(ShipQty,0)),0) from #tmpPL) + isnull (tpe.DiffQty, 0)
from Orders o with (nolock)
left join #TPEAdjust tpe on o.ID = tpe.OrderID
where ID = '{orderID}'

select * from #tmpPL
";
            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtResults);

            if (!result)
            {
                return result;
            }

            int orderQty = MyUtility.Convert.GetInt(dtResults[0].Rows[0]["Qty"]);
            int plQty = MyUtility.Convert.GetInt(dtResults[0].Rows[0]["PLQty"]) + curAddPlQty;

            if (orderQty >= plQty)
            {
                return new DualResult(true);
            }

            string plQtyListString = dtResults[1].AsEnumerable().Select(s => $"{s["ID"]} - {s["ShipQty"]}").JoinToString(Environment.NewLine);
            if (curAddPlQty > 0)
            {
                plQtyListString += Environment.NewLine + $"{plID} - {curAddPlQty}";
            }

            string errorMsg = $@"
Ttl Packing Qty ({plQty}) cannot exceed Order Qty ({orderQty}).
Please check below packing list.
{plQtyListString}
";
            return new DualResult(false, errorMsg);
        }

        /// <summary>
        /// 確認Order的訂單數量是否超過PackingList_Detail 的總和(不分Type、PackingListID)
        /// </summary>
        /// <param name="packingListID">被Confirm的PackingList ID</param>
        /// <returns>DualResult</returns>
        public static DualResult CheckOrderQty_ShipQty(string packingListID = "")
        {
            string sqlchk = $@"
---- 1. 加總表身各OrderID的訂單、出貨數量

SELECT DISTINCT pd.OrderID
INTO #OrderList
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.id=pd.ID
WHERE p.ID='{packingListID}'

SELECT 
[PackingListID] = pd.ID
,[OrderID]= o.ID
,[OrderQty]=o.Qty 
,[ShipQty]=SUM(pd.ShipQty)
INTO #tmp
FROM PackingList_Detail pd
INNER JOIN Orders o on pd.OrderID = o.ID and o.Junk = 0
WHERE pd.OrderID IN (SELECT OrderID FROM #OrderList)
GROUP BY pd.ID, o.ID,o.Qty 

----2. 找出台北調整的數量
SELECT  i.OrderID  ,[DiffQty]=SUM(ISNULL(iq.DiffQty,0) )
INTO #TPEAdjust
FROM InvAdjust i WITH (NOLOCK)
INNER JOIN InvAdjust_Qty iq WITH (NOLOCK)  ON  iq.ID = i.id 
WHERE i.OrderID IN (  SELECT OrderID FROM #tmp	)
GROUP BY i.OrderID

----3. 整合判斷，找出出貨數量 > 訂單數量的PackingList
----Summary
SELECT t.OrderID,[TtlOrderQty]=t.OrderQty, [TtlShipQty] = SUM((t.ShipQty + ISNULL(a.DiffQty,0)))
INTO #Summary
FROM #tmp t
LEFT JOIN #TPEAdjust a ON t.OrderID = a.OrderID
GROUP BY t.OrderID,t.OrderQty
HAVING t.OrderQty < SUM((t.ShipQty + ISNULL(a.DiffQty,0)))

SELECT * FROM #Summary

----By Order
SELECT t.PackingListID,t.OrderID, [TtlShipQty] = (t.ShipQty + ISNULL(a.DiffQty,0))
FROM #tmp t
LEFT JOIN #TPEAdjust a ON t.OrderID = a.OrderID
WHERE t.OrderID IN (SELECT OrderiD FROM #Summary)

DROP TABLE #OrderList,#tmp,#TPEAdjust,#Summary
";

            DataTable[] dtchk;
            DualResult dualResult = DBProxy.Current.Select(null, sqlchk, out dtchk);
            if (!dualResult)
            {
                return dualResult;
            }

            string msgSum = string.Empty;
            if (dtchk[0].Rows.Count > 0)
            {
                foreach (DataRow item in dtchk[0].AsEnumerable().ToList())
                {
                    string orderID = item["OrderID"].ToString();
                    string allOrderQty = item["TtlOrderQty"].ToString();
                    string allShipQty = item["TtlShipQty"].ToString();

                    string msg = string.Empty;
                    msg += $@"
SP# {orderID}
Ttl Packing Qty ({allShipQty}) cannot exceed Order Qty ({allOrderQty}).
Please check below packing list.
";

                    foreach (DataRow dr in dtchk[1].AsEnumerable().Where(o => o["OrderID"].ToString() == orderID).ToList())
                    {
                        string packingListID1 = dr["PackingListID"].ToString();
                        string ttlShipQty = dr["TtlShipQty"].ToString();

                        string msg2 = $"PackingList {packingListID1} - {ttlShipQty}" + Environment.NewLine;

                        msg += msg2;
                    }

                    msgSum += msg;
                }

                MyUtility.Msg.WarningBox(msgSum);
                return Ict.Result.F(msgSum);
            }

            return Ict.Result.True;
        }

        #region Query Packing List Print out Pacging List Report Data

        /// <summary>
        /// QueryPackingListReportData(string,DataTable,DataTable,DataTable,DataTable,DataTable,DataTable,string)
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <param name="reportType">reportType</param>
        /// <param name="printData">printData</param>
        /// <param name="ctnDim">ctnDim</param>
        /// <param name="qtyBDown">qtyBDown</param>
        /// <returns>QueryPackingListReportData</returns>
        public static DualResult QueryPackingListReportData(string packingListID, string reportType, out DataTable printData, out DataTable ctnDim, out DataTable qtyBDown)
        {
            ctnDim = null;
            qtyBDown = null;
            string sqlCmd = string.Format(
                @"

    select  OrderID
            , OrderShipmodeSeq
            , Article
            , Color
            , SizeCode
            , QtyPerCTN
            , NW
            , GW
            , NNW
            , NWPerPcs
            , min(Seq) as MinSeq
            , max(Seq) as MaxSeq 
            , count(*) as Ctns
    into #temp
    from PackingList_Detail WITH (NOLOCK) 
    where ID = '{0}'
    group by OrderID, OrderShipmodeSeq, Article, Color, SizeCode, QtyPerCTN
             , NW, GW, NNW, NWPerPcs

select  t.*
        , o.StyleID
        , o.Customize1
        , o.CustPONo
        , c.Alias
        , oq.EstPulloutDate
        , SizeSpec = iif(osso.SizeSpec is null, isnull(oss.SizeSpec,''),osso.SizeSpec)
        , MarkFront = isnull(o.MarkFront,'')
        , MarkBack = isnull(o.MarkBack,'')
        , MarkLeft = isnull(o.MarkLeft,'')
        , MarkRight = isnull(o.MarkRight,'')
        , InClogQty = (select sum(CTNQty) 
                       from PackingList_Detail WITH (NOLOCK) 
                       where  Id = '{0}' 
                                and ReceiveDate is not null)
        , CTNStartNo = (select CTNStartNo 
                        from PackingList_Detail WITH (NOLOCK) 
                        where  ID = '{0}' 
                               and Seq = t.MinSeq) 
        , CTNEndNo = (select CTNStartNo 
                      from PackingList_Detail WITH (NOLOCK) 
                      where  ID = '{0}' 
                             and Seq = t.MaxSeq) 
        , SciDelivery = iif(o.SciDelivery = '', null, o.SciDelivery)
from #temp t
left join Orders o WITH (NOLOCK) on  o.ID = t.OrderID
left join Order_QtyShip oq WITH (NOLOCK) on  oq.Id = t.OrderID 
                                             and oq.Seq = t.OrderShipmodeSeq
left join Country c WITH (NOLOCK) on  c.ID = o.Dest
outer apply(
	select distinct oso.SizeSpec 
	from Order_SizeSpec_OrderCombo oso WITH (NOLOCK) 
	where oso.OrderComboID = o.OrderComboID and SizeItem = 'S01' and oso.SizeCode = t.SizeCode and oso.id = o.poid
) osso
outer apply(select os.SizeSpec from Order_SizeSpec os WITH (NOLOCK) where os.Id = o.POID and SizeItem = 'S01' and os.SizeCode = t.SizeCode) oss
order by MinSeq
drop table #temp
", packingListID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(
                @"Declare @packinglistid VARCHAR(13),
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

select distinct t.RefNo,l.Description, STR(l.CtnLength,8,4)+'\'+STR(l.CtnWidth,8,4)+'\'+STR(l.CtnHeight,8,4) as Dimension, l.CtnUnit, 
(select CTNNo+',' from @tempPackingListDetail where RefNo = t.RefNo for xml path(''))as Ctn,
l.CBM*(select sum(CTNQty) from PackingList_Detail WITH (NOLOCK) where ID = @packinglistid and Refno = t.RefNo) as TtlCBM
from @tempPackingListDetail t
left join LocalItem l on l.RefNo = t.RefNo
order by RefNo", packingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out ctnDim);
            if (!result)
            {
                return result;
            }

            sqlCmd = $@"DECLARE @packinglistid VARCHAR(13),
		@orderid VARCHAR(13),
		@sizecode VARCHAR(8),
		@article VARCHAR(8),
		@dataseq VARCHAR(2),
		@sizecount DECIMAL,
		@poid VARCHAR(13),
		@tmpdatalist VARCHAR(1024),
		@datalen INT,
		@tmpdata VARCHAR(9),
		@reporttype INT,
		@tmpdata2 VARCHAR(1024),
		@qty INT,
		@cbm FLOAT

SET @packinglistid = '{packingListID}'
SET @reporttype = {reportType} --1:for Adidas/UA/Saucony/NB, 2:for LLL/TNF

select distinct @orderid = OrderID from PackingList_Detail WITH (NOLOCK) where ID = @packinglistid
select @sizecount = count(distinct SizeCode) from PackingList_Detail WITH (NOLOCK) where ID = @packinglistid
select @poid = POID from Orders WITH (NOLOCK) where ID = @orderid

--撈出此次出貨的Size Code
DECLARE cursor_SizeData CURSOR FOR
	SELECT distinct rtrim(pd.SizeCode),os.Seq 
	FROM PackingList_Detail pd WITH (NOLOCK) 
	LEFT JOIN Order_SizeCode os WITH (NOLOCK) on os.Id = @poid and os.SizeCode = pd.SizeCode
	WHERE pd.ID = @packinglistid
	order by os.Seq

--撈出此次出貨的Article
DECLARE cursor_ArticleData CURSOR FOR
	SELECT distinct rtrim(pd.Article),oa.Seq 
	FROM PackingList_Detail pd WITH (NOLOCK) 
	LEFT JOIN Order_Article oa WITH (NOLOCK) on oa.id = pd.OrderID and oa.Article = pd.Article
	WHERE pd.ID = @packinglistid
	order by oa.Seq

--建立暫存PackingList_Detail資料
DECLARE @tempQtyBDown TABLE (
   DataList VARCHAR(1024)
)

--填入Size Code
SET @tmpdatalist = '        '
OPEN cursor_SizeData
FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
WHILE @@FETCH_STATUS = 0
BEGIN
	BEGIN
		SET @datalen = LEN(@sizecode)
		SET @tmpdata = IIF(@datalen = 1,'     ',IIF(@datalen = 2 or @datalen = 3,'    ',IIF(@datalen = 4,'   ',IIF(@datalen = 5 or @datalen = 6,'  ','')))) + @sizecode + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))
		SET @tmpdatalist = @tmpdatalist  + @tmpdata
	END
	FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
END
CLOSE cursor_SizeData

IF(@reporttype = 1)
	SET @tmpdata2 = '   Total'
ELSE
	SET @tmpdata2 = '   Total   TTL CTN QTY     TTL CNM'
SET @tmpdatalist = @tmpdatalist + @tmpdata2

INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)

SET @tmpdatalist = ''
DECLARE @i INT
SET @i = 0
WHILE (@i <= @sizecount)
BEGIN
	SET @tmpdatalist = @tmpdatalist + '-------- '

	SET @i = @i + 1
END
SET @tmpdatalist = @tmpdatalist + IIF(@reporttype = 1,'--------','-------- -------------- ------------')
INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)

--填入Break down
OPEN cursor_ArticleData
FETCH NEXT FROM cursor_ArticleData INTO @article, @dataseq
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @tmpdatalist = @article + REPLICATE(' ', 9 - len(@article))
	OPEN cursor_SizeData
	FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
	WHILE @@FETCH_STATUS = 0
	BEGIN
		select @qty = isnull (sum(ShipQty), 0) from PackingList_Detail WITH (NOLOCK) where Id = @packinglistid and Article = @article and SizeCode = @sizecode
		SET @datalen = len(@qty)
		SET @tmpdata = IIF(@datalen = 1,'    ',IIF(@datalen = 2 or @datalen = 3,'   ',IIF(@datalen = 4,'  ',IIF(@datalen = 5 or @datalen = 6,' ','')))) + CONVERT(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))
		SET @tmpdatalist = @tmpdatalist  + @tmpdata
			
		FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
	END
	CLOSE cursor_SizeData
	
	select @qty = sum(ShipQty) from PackingList_Detail WITH (NOLOCK) where Id = @packinglistid and Article = @article
	SET @datalen = len(@qty)
	SET @tmpdata2 = IIF(@datalen = 1,'    ',IIF(@datalen = 2 or @datalen = 3,'   ',IIF(@datalen = 4,'  ',IIF(@datalen = 5 or @datalen = 6,' ','')))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))
	IF(@reporttype = 2)
		BEGIN
			select @qty = isnull (Sum(CTNQty), 0) from PackingList_Detail WITH (NOLOCK) where Id = @packinglistid and Article = @article
			SET @datalen = len(@qty)
			SET @tmpdata2 = @tmpdata2 + IIF(@datalen = 1,'      ',IIF(@datalen = 2 or @datalen = 3,'     ',IIF(@datalen = 4 or @datalen = 5,'    ',IIF(@datalen = 6 or @datalen = 7,'   ',IIF(@datalen = 8 or @datalen = 9 or @datalen = 10,'  ',IIF(@datalen = 11 or @datalen = 12,' ','')))))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'        ',IIF(@datalen = 3 or @datalen = 4,'       ',IIF(@datalen = 5 or @datalen = 6,'      ',IIF(@datalen = 7 or @datalen = 8,'     ',IIF(@datalen = 9,'    ',IIF(@datalen = 10 or @datalen = 11,'   ',IIF(@datalen = 12 or @datalen = 13,'  ',' ')))))))

			select @cbm = sum(pd.CTNQty*l.CBM) from PackingList_Detail pd WITH (NOLOCK) left join LocalItem l WITH (NOLOCK) on l.RefNo = pd.RefNo where pd.ID = @packinglistid and pd.Article = @article
			SET @datalen = len(@cbm)
			SET @tmpdata2 = @tmpdata2 + IIF(@datalen = 1,'      ',IIF(@datalen = 2 or @datalen = 3,'     ',IIF(@datalen = 4 or @datalen = 5,'    ',IIF(@datalen = 6 or @datalen = 7,'   ',IIF(@datalen = 8 or @datalen = 9 or @datalen = 10,'  ',IIF(@datalen = 11 or @datalen = 12,' ','')))))) + convert(VARCHAR,@cbm) + IIF(@datalen = 1 or @datalen = 2,'        ',IIF(@datalen = 3 or @datalen = 4,'       ',IIF(@datalen = 5 or @datalen = 6,'      ',IIF(@datalen = 7 or @datalen = 8,'     ',IIF(@datalen = 9,'    ',IIF(@datalen = 10 or @datalen = 11,'   ',IIF(@datalen = 12 or @datalen = 13,'  ',' ')))))))
		END
	
	SET @tmpdatalist = @tmpdatalist + @tmpdata2

	INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)
	FETCH NEXT FROM cursor_ArticleData INTO @article, @dataseq
END
CLOSE cursor_ArticleData

SET @tmpdatalist = ''
SET @i = 0
WHILE (@i <= @sizecount)
BEGIN
	SET @tmpdatalist = @tmpdatalist + '-------- '

	SET @i = @i + 1
END
SET @tmpdatalist = @tmpdatalist + IIF(@reporttype = 1,'--------','-------- -------------- ------------')
INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)

SET @tmpdatalist = 'TTL.     '
OPEN cursor_SizeData
FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
WHILE @@FETCH_STATUS = 0
BEGIN
	select @qty = sum(ShipQty) from PackingList_Detail WITH (NOLOCK) where Id = @packinglistid and SizeCode = @sizecode
	SET @datalen = len(@qty)
	SET @tmpdata2 = IIF(@datalen = 1,'    ',IIF(@datalen = 2 or @datalen = 3,'   ',IIF(@datalen = 4,'  ',IIF(@datalen = 5 or @datalen = 6,' ','')))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))
	SET @tmpdatalist = @tmpdatalist  + @tmpdata2

	FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
END
CLOSE cursor_SizeData
select @qty = sum(ShipQty) from PackingList_Detail WITH (NOLOCK) where Id = @packinglistid
SET @datalen = len(@qty)
SET @tmpdata2 = IIF(@datalen = 1,'    ',IIF(@datalen = 2 or @datalen = 3,'   ',IIF(@datalen = 4,'  ',IIF(@datalen = 5 or @datalen = 6,' ','')))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))

IF(@reporttype = 2)
BEGIN
	select @qty = Sum(CTNQty) from PackingList_Detail WITH (NOLOCK) where Id = @packinglistid
	SET @datalen = len(@qty)
	SET @tmpdata2 = @tmpdata2 + IIF(@datalen = 1,'      ',IIF(@datalen = 2 or @datalen = 3,'     ',IIF(@datalen = 4 or @datalen = 5,'    ',IIF(@datalen = 6 or @datalen = 7,'   ',IIF(@datalen = 8 or @datalen = 9 or @datalen = 10,'  ',IIF(@datalen = 11 or @datalen = 12,' ','')))))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'        ',IIF(@datalen = 3 or @datalen = 4,'       ',IIF(@datalen = 5 or @datalen = 6,'      ',IIF(@datalen = 7 or @datalen = 8,'     ',IIF(@datalen = 9,'    ',IIF(@datalen = 10 or @datalen = 11,'   ',IIF(@datalen = 12 or @datalen = 13,'  ',' ')))))))

	select @cbm = sum(pd.CTNQty*l.CBM) from PackingList_Detail pd WITH (NOLOCK) left join LocalItem l WITH (NOLOCK) on l.RefNo = pd.RefNo where pd.ID = @packinglistid
	SET @datalen = len(@cbm)
	SET @tmpdata2 = @tmpdata2 + IIF(@datalen = 1,'      ',IIF(@datalen = 2 or @datalen = 3,'     ',IIF(@datalen = 4 or @datalen = 5,'    ',IIF(@datalen = 6 or @datalen = 7,'   ',IIF(@datalen = 8 or @datalen = 9 or @datalen = 10,'  ',IIF(@datalen = 11 or @datalen = 12,' ','')))))) + convert(VARCHAR,@cbm) + IIF(@datalen = 1 or @datalen = 2,'        ',IIF(@datalen = 3 or @datalen = 4,'       ',IIF(@datalen = 5 or @datalen = 6,'      ',IIF(@datalen = 7 or @datalen = 8,'     ',IIF(@datalen = 9,'    ',IIF(@datalen = 10 or @datalen = 11,'   ',IIF(@datalen = 12 or @datalen = 13,'  ',' ')))))))
END
SET @tmpdatalist = @tmpdatalist  + @tmpdata2
INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)

DEALLOCATE cursor_SizeData
DEALLOCATE cursor_ArticleData

select * from @tempQtyBDown";
            result = DBProxy.Current.Select(null, sqlCmd, out qtyBDown);
            return result;
        }
        #endregion

        #region Packing List data write in excel -- Packing List Report

        /// <summary>
        /// PackingListToExcel_PackingListReport(string,DataTable,string,DataTable,DataTable,DataTable)
        /// </summary>
        /// <param name="xltxName">xltx Name</param>
        /// <param name="pLdt">pLdt</param>
        /// <param name="reportType">reportType</param>
        /// <param name="printData">printData</param>
        /// <param name="ctnDim">ctnDim</param>
        /// <param name="qtyBDown">qtyBDown</param>
        public static void PackingListToExcel_PackingListReport(string xltxName, DataTable pLdt, string reportType, DataSet printData, DataSet ctnDim, DataSet qtyBDown)
        {
            #region Check Multiple
            bool boolMultiple = xltxName.EqualString("\\Packing_P03_PackingListReport_Multiple.xltx") ? true : false;
            #endregion

            string strXltName = Env.Cfg.XltPathDir + xltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

#if DEBUG
            excel.Visible = true;
#endif

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            for (int i = 1; i < pLdt.Rows.Count; i++)
            {
                worksheet.Copy(Type.Missing, worksheet);
            }

            int cntWorkSheet = 1;
            foreach (DataRow dr in pLdt.Rows)
            {
                #region Get Sci Delivery
                bool notExistsID = printData.Tables[dr["ID"].ToString()] == null;
                if (notExistsID)
                {
                    continue;
                }

                // 防止 Sheet Name 重複
                bool existsSheetName = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet existsSheet in excel.Workbooks[1].Worksheets)
                {
                    if (string.Compare(existsSheet.Name, dr["id"].ToString()) == 0)
                    {
                        existsSheetName = true;
                    }
                }

                DataRow[] getMinDelivery = printData.Tables[dr["ID"].ToString()].Select("SciDelivery = min(SciDelivery)");
                string strSciDelivery = string.Empty;
                if (getMinDelivery.Length > 0)
                {
                    strSciDelivery = string.Format("{0:yyyy/MM/dd}", getMinDelivery[0]["SciDelivery"]);
                }
                #endregion

                #region Column & Row Index
                int bodyRowIndex = boolMultiple ? 7 : 9,
                    bodyRowStartIndex = boolMultiple ? 7 : 9,
                    bodyRowEndIndex = boolMultiple ? 29 : 31,
                    titleMRow = 1, titleMColumn = 1,
                    titlePakingListNoRow = 3, titlePakingListNoColumn = 3,
                    titleSciDeliveryRow = 3, titleSciDeliveryColumn = boolMultiple ? 18 : 14,
                    titleSPRow = 5, titleSPColumn = 1,
                    titleStyleRow = 5, titleStyleColumn = 3,
                    titleOrderNoRow = 5, titleOrderNoColumn = 6,
                    titlePoNoRow = 5, titlePoNoColumn = 10,
                    titleInvoiceRow = 5, titleInvoiceColumn = boolMultiple ? 1 : 13,
                    titleCustCDRow = boolMultiple ? 5 : 7, titleCustCDColumn = boolMultiple ? 4 : 1,
                    titleShipModeRow = boolMultiple ? 5 : 7, titleShipModeColumn = boolMultiple ? 7 : 3,
                    titleInClogRow = boolMultiple ? 5 : 7, titleInClogColumn = boolMultiple ? 10 : 6,
                    titleDestinationRow = boolMultiple ? 5 : 7, titleDestinationColumn = boolMultiple ? 14 : 10,
                    titleShipmentDateRow = boolMultiple ? 5 : 7, titleShipmentDateColumn = boolMultiple ? 18 : 13,
                    bodySPColumn = 1,
                    bodyStyleColumn = 2,
                    bodyOrderNoColumn = 3,
                    bodyPoNoColumn = 4,
                    bodyCtn1Column = boolMultiple ? 5 : 1,
                    bodyCtn2Column = boolMultiple ? 6 : 2,
                    bodyCtnsColumn = boolMultiple ? 7 : 3,
                    bodyColorColumn = boolMultiple ? 8 : 4,
                    bodySizeColumn = boolMultiple ? 9 : 5,
                    bodyCustSizeColumn = boolMultiple ? 10 : 6,
                    bodyPcCtnsColumn = boolMultiple ? 11 : 7,
                    bodyQtyColumn = boolMultiple ? 12 : 8,
                    bodyNWColumn = boolMultiple ? 13 : 9,
                    bodyGWColumn = boolMultiple ? 14 : 10,
                    bodyNNWColumn = boolMultiple ? 15 : 11,
                    bodyNWPcsColumn = boolMultiple ? 16 : 12,
                    bodyTTLNWColumn = boolMultiple ? 17 : 13,
                    bodyTTLGWColumn = boolMultiple ? 18 : 14,
                    bodyTTLNNWColumn = boolMultiple ? 19 : 15,
                    sippingMarkBDColumn = boolMultiple ? 11 : 8
                    ;
                #endregion
                #region workRange

                // strWorkRange 取 A8 & A10 是因為框線設定
                string strWorkRange = boolMultiple ? "A8:A8" : "A10:A10",
                        strColumnsRange = boolMultiple ? "A{0}:S{0}" : "A{0}:O{0}";
                #endregion

                worksheet = excel.ActiveWorkbook.Worksheets[cntWorkSheet];
                worksheet.Select();
                if (existsSheetName)
                {
                    continue;
                }

                worksheet.Name = dr["id"].ToString();
                string nameEN = MyUtility.GetValue.Lookup("NameEN", Env.User.Factory, "Factory ", "id");
                cntWorkSheet++;
                #region Set Title

                // 單筆SP 限定
                if (!boolMultiple)
                {
                    worksheet.Cells[titleSPRow, titleSPColumn] = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["OrderID"]);
                    worksheet.Cells[titleStyleRow, titleStyleColumn] = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["StyleID"]);
                    worksheet.Cells[titleOrderNoRow, titleOrderNoColumn] = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["Customize1"]);
                    worksheet.Cells[titlePoNoRow, titlePoNoColumn] = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["CustPONo"]);
                }

                worksheet.Cells[titleMRow, titleMColumn] = nameEN;
                worksheet.Cells[titlePakingListNoRow, titlePakingListNoColumn] = MyUtility.Convert.GetString(dr["ID"]);
                worksheet.Cells[titleSciDeliveryRow, titleSciDeliveryColumn] = strSciDelivery;
                worksheet.Cells[titleInvoiceRow, titleInvoiceColumn] = MyUtility.Convert.GetString(dr["INVNo"]);
                worksheet.Cells[titleCustCDRow, titleCustCDColumn] = MyUtility.Convert.GetString(dr["CustCDID"]);
                worksheet.Cells[titleShipModeRow, titleShipModeColumn] = MyUtility.Convert.GetString(dr["ShipModeID"]);
                worksheet.Cells[titleInClogRow, titleInClogColumn] = (MyUtility.Check.Empty(MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["InClogQty"])) ? "0" : MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["InClogQty"])) + " / " + MyUtility.Convert.GetString(dr["CTNQty"]) + "   ( " + MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(printData.Tables[dr["ID"].ToString()].Rows[0]["InClogQty"]) / MyUtility.Convert.GetDecimal(dr["CTNQty"]), 4) * 100) + "% )";
                worksheet.Cells[titleDestinationRow, titleDestinationColumn] = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["Alias"]);
                worksheet.Cells[titleShipmentDateRow, titleShipmentDateColumn] = MyUtility.Check.Empty(printData.Tables[dr["ID"].ToString()].Rows[0]["EstPulloutDate"]) ? "  /  /    " : Convert.ToDateTime(printData.Tables[dr["ID"].ToString()].Rows[0]["EstPulloutDate"]).ToString("d");
                #endregion

                #region Set Body

                // 當要列印的筆數超過22筆，就要插入Row，因為範本只留22筆記錄的空間
                if (printData.Tables[dr["ID"].ToString()].Rows.Count > 22)
                {
                    for (int i = 1; i <= printData.Tables[dr["ID"].ToString()].Rows.Count - 22; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(strWorkRange, Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);
                    }
                }

                string ctnStartNo = "XXXXXX";
                foreach (DataRow drBody in printData.Tables[dr["ID"].ToString()].Rows)
                {
                    // 因為有箱號不連續的問題，所以直接時用來源資料組好的數量，不用箱號箱減
                    // int ctns = MyUtility.Convert.GetInt(dr["CTNEndNo"]) - MyUtility.Convert.GetInt(dr["CTNStartNo"]) + 1;
                    int ctns = (int)drBody["Ctns"];
                    if (ctnStartNo != MyUtility.Convert.GetString(drBody["CTNStartNo"]))
                    {
                        worksheet.Cells[bodyRowIndex, bodyCtn1Column] = MyUtility.Convert.GetString(drBody["CTNStartNo"]);
                        worksheet.Cells[bodyRowIndex, bodyCtnsColumn] = MyUtility.Convert.GetString(ctns);
                        worksheet.Cells[bodyRowIndex, bodyNWColumn] = MyUtility.Convert.GetString(drBody["NW"]);
                        worksheet.Cells[bodyRowIndex, bodyGWColumn] = MyUtility.Convert.GetString(drBody["GW"]);
                        worksheet.Cells[bodyRowIndex, bodyNNWColumn] = MyUtility.Convert.GetString(drBody["NNW"]);
                        worksheet.Cells[bodyRowIndex, bodyTTLNWColumn] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(drBody["NW"]) * ctns);
                        worksheet.Cells[bodyRowIndex, bodyTTLGWColumn] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(drBody["GW"]) * ctns);
                        worksheet.Cells[bodyRowIndex, bodyTTLNNWColumn] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(drBody["NNW"]) * ctns);

                        // 多筆 SP 限定
                        if (boolMultiple)
                        {
                            worksheet.Cells[bodyRowIndex, bodySPColumn] = drBody["OrderID"].ToString();
                            worksheet.Cells[bodyRowIndex, bodyStyleColumn] = drBody["StyleID"].ToString();
                            worksheet.Cells[bodyRowIndex, bodyOrderNoColumn] = drBody["Customize1"].ToString();
                            worksheet.Cells[bodyRowIndex, bodyPoNoColumn] = drBody["CustPONo"].ToString();
                        }

                        if (MyUtility.Convert.GetString(drBody["CTNStartNo"]) != MyUtility.Convert.GetString(drBody["CTNEndNo"]))
                        {
                            worksheet.Cells[bodyRowIndex, bodyCtn2Column] = MyUtility.Convert.GetString(drBody["CTNEndNo"]);
                        }
                    }

                    worksheet.Cells[bodyRowIndex, bodyColorColumn] = MyUtility.Convert.GetString(drBody["Article"]) + " " + MyUtility.Convert.GetString(drBody["Color"]);
                    worksheet.Cells[bodyRowIndex, bodySizeColumn] = MyUtility.Convert.GetString(drBody["SizeCode"]);
                    worksheet.Cells[bodyRowIndex, bodyCustSizeColumn] = MyUtility.Convert.GetString(drBody["SizeSpec"]);
                    worksheet.Cells[bodyRowIndex, bodyPcCtnsColumn] = MyUtility.Convert.GetString(drBody["QtyPerCTN"]);
                    worksheet.Cells[bodyRowIndex, bodyQtyColumn] = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(drBody["QtyPerCTN"]) * ctns);
                    worksheet.Cells[bodyRowIndex, bodyNWPcsColumn] = MyUtility.Check.Empty(drBody["NWPerPcs"]) ? string.Empty : MyUtility.Convert.GetString(drBody["NWPerPcs"]);

                    ctnStartNo = MyUtility.Convert.GetString(drBody["CTNStartNo"]);
                    bodyRowIndex++;
                }

                worksheet.Range[string.Format(strColumnsRange, bodyRowIndex)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; // 1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[string.Format(strColumnsRange, bodyRowIndex)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;
                worksheet.Cells[bodyRowIndex, 2] = "Total";
                if (bodyRowIndex >= bodyRowStartIndex)
                {
                    worksheet.Cells[bodyRowIndex, bodyCtnsColumn] = string.Format("=SUM(INDIRECT(ADDRESS({0},{1})):INDIRECT(ADDRESS({2},{1})))", bodyRowStartIndex, bodyCtnsColumn, MyUtility.Convert.GetString(bodyRowIndex - 1));
                    worksheet.Cells[bodyRowIndex, bodyQtyColumn] = string.Format("=SUM(INDIRECT(ADDRESS({0},{1})):INDIRECT(ADDRESS({2},{1})))", bodyRowStartIndex, bodyQtyColumn, MyUtility.Convert.GetString(bodyRowIndex - 1));
                    worksheet.Cells[bodyRowIndex, bodyTTLNWColumn] = string.Format("=SUM(INDIRECT(ADDRESS({0},{1})):INDIRECT(ADDRESS({2},{1})))", bodyRowStartIndex, bodyTTLNWColumn, MyUtility.Convert.GetString(bodyRowIndex - 1));
                    worksheet.Cells[bodyRowIndex, bodyTTLGWColumn] = string.Format("=SUM(INDIRECT(ADDRESS({0},{1})):INDIRECT(ADDRESS({2},{1})))", bodyRowStartIndex, bodyTTLGWColumn, MyUtility.Convert.GetString(bodyRowIndex - 1));
                    worksheet.Cells[bodyRowIndex, bodyTTLNNWColumn] = string.Format("=SUM(INDIRECT(ADDRESS({0},{1})):INDIRECT(ADDRESS({2},{1})))", bodyRowStartIndex, bodyTTLNNWColumn, MyUtility.Convert.GetString(bodyRowIndex - 1));
                }

                if (bodyRowIndex <= bodyRowEndIndex)
                {
                    bodyRowIndex = bodyRowEndIndex;
                }
                #endregion
                bodyRowIndex++;
                #region Carton Dimension

                StringBuilder ctnDimension = new StringBuilder();

                foreach (DataRow drCtn in ctnDim.Tables[dr["ID"].ToString()].Rows)
                {
                    ctnDimension.Append(string.Format(
                        "{0} - {1} - {2} {3}, (CTN#:{4}){5}  \r\n",
                        MyUtility.Convert.GetString(drCtn["RefNo"]),
                        MyUtility.Convert.GetString(drCtn["Description"]),
                        MyUtility.Convert.GetString(drCtn["Dimension"]),
                        MyUtility.Convert.GetString(drCtn["CtnUnit"]),
                        MyUtility.Check.Empty(drCtn["Ctn"]) ? string.Empty : MyUtility.Convert.GetString(drCtn["Ctn"]).Substring(0, MyUtility.Convert.GetString(drCtn["Ctn"]).Length - 1),
                        reportType == "1" ? ", ttlCBM:" + MyUtility.Convert.GetString(drCtn["TtlCBM"]) : string.Empty));
                }

                string cds = ctnDimension.Length > 0 ? ctnDimension.ToString().Substring(0, ctnDimension.ToString().Length - 2) : string.Empty;
                string[] cdsab = cds.Split('\r');
                int cdsi = 0;
                int cdsl = 150;
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

                cdsi += cdsab.Length - 1;
                for (int i = 1; i <= cdsi; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rangeRowCD = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[bodyRowIndex, Type.Missing];
                    rangeRowCD.RowHeight = 19.5 * (i + 1);
                    Marshal.ReleaseComObject(rangeRowCD);
                }

                worksheet.Cells[bodyRowIndex, 3] = ctnDimension.Length > 0 ? ctnDimension.ToString() : string.Empty;
                #endregion

                // Remarks
                bodyRowIndex++;
                worksheet.Cells[bodyRowIndex, 3] = MyUtility.Convert.GetString(dr["Remark"]);

                #region Color/Size Breakdown

                bodyRowIndex = bodyRowIndex + 2;
                if (qtyBDown.Tables[dr["ID"].ToString()].Rows.Count > 5)
                {
                    Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(bodyRowIndex))).EntireRow;
                    for (int i = 1; i <= qtyBDown.Tables[dr["ID"].ToString()].Rows.Count - 5; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}", MyUtility.Convert.GetString(bodyRowIndex + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                        Marshal.ReleaseComObject(rngToInsert);
                    }

                    Marshal.ReleaseComObject(rngToCopy);
                }

                foreach (DataRow drQtyBDown in qtyBDown.Tables[dr["ID"].ToString()].Rows)
                {
                    worksheet.Cells[bodyRowIndex, 1] = MyUtility.Convert.GetString(drQtyBDown["DataList"]);
                    bodyRowIndex++;
                }

                #endregion

                #region Shipment mark

                // Shipment mark
                bodyRowIndex = bodyRowIndex + 3;
                worksheet.Cells[bodyRowIndex, 1] = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["MarkFront"]);

                worksheet.Cells[bodyRowIndex, sippingMarkBDColumn] = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["MarkBack"]);

                string[] marks = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["MarkFront"]).Split('\r');
                string[] marks2 = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["MarkBack"]).Split('\r');
                int m = marks.Length + Formarks(marks);
                int m2 = marks2.Length + Formarks(marks2);
                int m1 = m > m2 ? m : m2;
                int df = 11;
                int add = (m1 - df) >= 0 ? m1 - df : 0;
                if (m1 > df)
                {
                    for (int i = 0; i < add; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range mark = worksheet.get_Range(string.Format("A{0}", MyUtility.Convert.GetString(bodyRowIndex + 1)), Type.Missing).EntireRow;
                        mark.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, mark.Copy(Type.Missing));
                        Marshal.ReleaseComObject(mark);
                    }
                }

                bodyRowIndex = bodyRowIndex + add + 13;

                worksheet.Cells[bodyRowIndex, 1] = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["MarkLeft"]);
                worksheet.Cells[bodyRowIndex, sippingMarkBDColumn] = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["MarkRight"]);

                string[] marks3 = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["MarkLeft"]).Split('\r');
                string[] marks4 = MyUtility.Convert.GetString(printData.Tables[dr["ID"].ToString()].Rows[0]["MarkRight"]).Split('\r');
                int m3 = marks3.Length + Formarks(marks3);
                int m4 = marks4.Length + Formarks(marks4);
                int m12 = m3 > m4 ? m3 : m4;
                int df2 = 11;
                int add2 = (m12 - df2) >= 0 ? m12 - df2 : 0;
                if (m12 > df2)
                {
                    for (int i = 0; i < add2; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range mark = worksheet.get_Range(string.Format("A{0}", MyUtility.Convert.GetString(bodyRowIndex + 1)), Type.Missing).EntireRow;
                        mark.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, mark.Copy(Type.Missing));
                        Marshal.ReleaseComObject(mark);
                    }
                }
                #endregion
            }

            // MyUtility.Msg.WaitClear();
            excel.Columns.AutoFit();
            excel.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(boolMultiple ? "Packing_P03_PackingListReport_Multiple" : "Packing_P03_PackingGuideReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }

        /// <summary>
        /// formarks
        /// </summary>
        /// <param name="marks">marks</param>
        /// <returns>int</returns>
        private static int Formarks(string[] marks)
        {
            int b = 0;
            int l = 63;
            foreach (string item in marks)
            {
                if (item.Length > l)
                {
                    int h = item.Length / l;
                    b += 1 + h;
                }
            }

            return b;
        }
        #endregion

        #region Query Packing List Print out Pacging Guide Report Data

        /// <summary>
        /// QueryPackingGuideReportData(string,DataTable,DataTable,DataTable,DataTable,DataTable,DataTable,string)
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <param name="printData">printData</param>
        /// <param name="ctnDim">ctnDim</param>
        /// <param name="qtyCtn">qtyCtn</param>
        /// <param name="articleSizeTtlShipQty">articleSizeTtlShipQty</param>
        /// <param name="printGroupData">printGroupData</param>
        /// <param name="clipData">clipData</param>
        /// <param name="specialInstruction">specialInstruction</param>
        /// <returns> DualResult QueryPackingGuideReportData</returns>
        public static DualResult QueryPackingGuideReportData(string packingListID, out DataTable printData, out DataTable ctnDim, out DataTable qtyCtn, out DataTable articleSizeTtlShipQty, out DataTable printGroupData, out DataTable clipData, out string specialInstruction)
        {
            ctnDim = null;
            qtyCtn = null;
            articleSizeTtlShipQty = null;
            printGroupData = null;
            clipData = null;
            specialInstruction = MyUtility.GetValue.Lookup(string.Format(
                @"
select top 1 Packing = isnull(o.Packing, '') 
from PackingList_Detail pd WITH (NOLOCK) 
     , Orders o WITH (NOLOCK) 
where   pd.ID = '{0}' 
        and pd.OrderID = o.ID", packingListID));

            string sqlCmd = string.Format(
                @"
select  pd.OrderID
        , StyleID = isnull(o.StyleID,'')
        , Customize1 = isnull(o.Customize1,'')
        , CustPONo = isnull(o.CustPONo,'')
        , p.CTNQty
        , DestAlias = isnull(c.Alias,'')
        , pd.Article
        , pd.Color
        , pd.SizeCode
        , pd.CTNStartNo
        , pd.QtyPerCTN
        , pd.ShipQty
        , pd.CTNQty
        , PackInstruction = isnull(o.Packing,'')
        , pd.Seq
        , SizeSpec = iif(osso.SizeSpec is null, isnull(oss.SizeSpec,''),osso.SizeSpec)
        , TtlShipQty = (select sum(ShipQty) 
                        from PackingList_Detail WITH (NOLOCK) 
                        where   Id = p.ID 
                                and Article = pd.Article 
                                and SizeCode = pd.SizeCode)
        , OQty = (select Qty 
                  from Order_QtyShip_Detail WITH (NOLOCK) 
                  where Id = pd.OrderID 
                        and Seq = pd.OrderShipmodeSeq 
                        and Article = pd.Article 
                        and SizeCode = pd.SizeCode)
        , Factory = o.FtyGroup
        , BuyerDelivery = Convert(date, o.BuyerDelivery, 120)
        , CustCD = o.CustCDID
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
outer apply(
	select distinct oso.SizeSpec 
	from Order_SizeSpec_OrderCombo oso WITH (NOLOCK) 
	where oso.OrderComboID = o.OrderComboID and SizeItem = 'S01' and oso.SizeCode = pd.SizeCode and oso.id = o.poid
) osso
outer apply(select os.SizeSpec from Order_SizeSpec os WITH (NOLOCK) where os.Id = o.POID and SizeItem = 'S01' and os.SizeCode = pd.SizeCode) oss
where p.ID = '{0}'
order by pd.Seq", packingListID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                return result;
            }

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

select distinct t.RefNo,l.Description, STR(l.CtnLength,8,4)+'\'+STR(l.CtnWidth,8,4)+'\'+STR(l.CtnHeight,8,4) as Dimension, l.CtnUnit, 
Ctn = concat('(CTN#:',stuff((select concat(',',CTNNo) from @tempPackingListDetail where RefNo = t.RefNo for xml path('')),1,1,''),')')
from @tempPackingListDetail t
left join LocalItem l on l.RefNo = t.RefNo
order by RefNo", packingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out ctnDim);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(
                @"
select  distinct Seq1 = oa.Seq 
        , Seq2 = os.Seq
        , Article = isnull(oq.Article, '')
        , SizeCode = isnull(oq.SizeCode, '')
        , Qty = isnull(oq.Qty,0)
from PackingList_Detail pd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
left join Order_QtyCTN oq WITH (NOLOCK) on o.ID = oq.Id
left join Order_Article oa WITH (NOLOCK) on  o.ID = oa.id 
                                             and oq.Article = oa.Article
left join Order_SizeCode os WITH (NOLOCK) on  o.POID = os.Id 
                                              and oq.SizeCode = os.SizeCode
where pd.ID = '{0}'", packingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out qtyCtn);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(
                @"
select  Article
        , SizeCode
        , TtlShipQty = sum(ShipQty) 
from PackingList_Detail WITH (NOLOCK) 
where ID = '{0}' 
group by Article, SizeCode", packingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out articleSizeTtlShipQty);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(
                @"
select  a.*
        , TtlShipQty = (select sum(ShipQty) 
                        from PackingList_Detail WITH (NOLOCK) 
                        where   Id = a.ID 
                                and Article = a.Article 
                                and SizeCode = a.SizeCode)
        , OQty = (select Qty 
                  from Order_QtyShip_Detail WITH (NOLOCK) 
                  where Id = a.OrderID 
                        and Seq = a.OrderShipmodeSeq 
                        and Article = a.Article 
                        and SizeCode = a.SizeCode)
from (
    select  pd.ID
            , pd.OrderID
            , pd.OrderShipmodeSeq
            , pd.Article
            , pd.Color
            , pd.SizeCode
            , SizeSpec = iif(osso.SizeSpec is null, isnull(oss.SizeSpec,''),osso.SizeSpec)
            , pd.QtyPerCTN
            , pd.CTNQty
            , MinSeq = min(pd.Seq)
            , MaxSeq = max(pd.Seq)
    from PackingList_Detail pd WITH (NOLOCK) 
    left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
    outer apply(
	    select distinct oso.SizeSpec 
	    from Order_SizeSpec_OrderCombo oso WITH (NOLOCK) 
	    where oso.OrderComboID = o.OrderComboID and SizeItem = 'S01' and oso.SizeCode = pd.SizeCode and oso.id = o.poid
    ) osso
    outer apply(select os.SizeSpec from Order_SizeSpec os WITH (NOLOCK) where os.Id = o.POID and SizeItem = 'S01' and os.SizeCode = pd.SizeCode) oss
    where pd.ID = '{0}'
    group by pd.ID, pd.OrderID, pd.OrderShipmodeSeq, pd.Article, pd.Color
             , pd.SizeCode, iif(osso.SizeSpec is null, isnull(oss.SizeSpec,''),osso.SizeSpec), pd.QtyPerCTN, pd.CTNQty
) a
order by a.MinSeq", packingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out printGroupData);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(
                @"
select  PKey = isnull(c.PKey,'')
        , TableName = isnull(c.TableName,'') 
        , SourceFile = isnull(c.SourceFile,'')
        , Year = YEAR(c.AddDate)
        , Month  = MONTH (c.AddDate)
from Clip c WITH (NOLOCK) 
     , PackingList p WITH (NOLOCK) 
where   p.ID = '{0}'
        and c.TableName = 'CustCD' 
        and c.UniqueKey = p.BrandID+p.CustCDID
        and UPPER(c.SourceFile) like '%.JPG'", packingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out clipData);
            return result;
        }
        #endregion

        #region Packing List data write in excel -- Packing Guide Report

        /// <summary>
        /// PackingListToExcel_PackingGuideReport(string,DataTable,DataTable,DataTable,DataTable,DataTable,DataTable,DataRow,int,string)
        /// </summary>
        /// <param name="xltxName">xltxName</param>
        /// <param name="printData">printData</param>
        /// <param name="ctnDim">ctnDim</param>
        /// <param name="qtyCtn">qtyCtn</param>
        /// <param name="articleSizeTtlShipQty">articleSizeTtlShipQty</param>
        /// <param name="printGroupData">printGroupData</param>
        /// <param name="clipData">clipData</param>
        /// <param name="packlistData">packlistData</param>
        /// <param name="orderQty">orderQty</param>
        /// <param name="specialInstruction">specialInstruction</param>
        /// <param name="visRow3">visRow3</param>
        public static void PackingListToExcel_PackingGuideReport(string xltxName, DataTable printData, DataTable ctnDim, DataTable qtyCtn, DataTable articleSizeTtlShipQty, DataTable printGroupData, DataTable clipData, DataRow packlistData, int orderQty, string specialInstruction, bool visRow3)
        {
            string strXltName = Env.Cfg.XltPathDir + xltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            // MyUtility.Msg.WaitWindows("Starting to excel...");
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            string nameEN = MyUtility.GetValue.Lookup("NameEN", Env.User.Factory, "Factory ", "id");
            worksheet.Cells[1, 1] = nameEN;
            worksheet.Cells[3, 3] = MyUtility.Convert.GetString(packlistData["ID"]);
            worksheet.Cells[3, 20] = DateTime.Today;
            worksheet.Cells[5, 1] = MyUtility.Convert.GetString(printData.Rows[0]["Factory"]);
            worksheet.Cells[5, 2] = MyUtility.Convert.GetString(printData.Rows[0]["OrderID"]);
            worksheet.Cells[5, 4] = printData.Rows[0]["BuyerDelivery"];
            worksheet.Cells[5, 6] = MyUtility.Convert.GetString(printData.Rows[0]["StyleID"]);
            worksheet.Cells[5, 8] = MyUtility.Convert.GetString(printData.Rows[0]["Customize1"]);
            worksheet.Cells[5, 10] = MyUtility.Convert.GetString(printData.Rows[0]["CustPONo"]);
            worksheet.Cells[5, 12] = MyUtility.Convert.GetInt(printData.Rows[0]["CTNQty"]);
            worksheet.Cells[5, 14] = MyUtility.Convert.GetString(printData.Rows[0]["CustCD"]);
            worksheet.Cells[5, 16] = MyUtility.Convert.GetString(printData.Rows[0]["DestAlias"]);
            worksheet.Cells[5, 18] = orderQty;
            worksheet.Cells[5, 20] = MyUtility.Convert.GetInt(packlistData["ShipQty"]);
            worksheet.Cells[5, 21] = "=R5-T5";
            int groupRec = printGroupData.Rows.Count, excelRow = 6, printCtnCount = 0;
            int chk1 = excelRow + 257, chk2 = excelRow + 258;
            string seq = "000000", article = "XXXX0000", size = "XXXX0000";
            int qtyPerCTN = -1;
            StringBuilder articleSize = new StringBuilder();

            for (int i = 0; ; i++)
            {
                if (i >= groupRec)
                {
                    break;
                }

                if (MyUtility.Check.Empty(printGroupData.Rows[i]["CTNQty"]))
                {
                    excelRow++;

                    // 若資料會超過262筆，就先插入一筆Record，最後再把多的資料刪除
                    if (excelRow >= chk1)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(excelRow)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);
                    }

                    if (article != MyUtility.Convert.GetString(printGroupData.Rows[i]["Article"]))
                    {
                        article = MyUtility.Convert.GetString(printGroupData.Rows[i]["Article"]);
                        worksheet.Cells[excelRow, 1] = MyUtility.Convert.GetString(printGroupData.Rows[i]["Article"]) + ' ' + MyUtility.Convert.GetString(printGroupData.Rows[i]["Color"]);
                    }

                    worksheet.Cells[excelRow, 2] = MyUtility.Convert.GetString(printGroupData.Rows[i]["SizeCode"]);
                    worksheet.Cells[excelRow, 3] = MyUtility.Convert.GetString(printGroupData.Rows[i]["SizeSpec"]);
                    worksheet.Cells[excelRow, 4] = MyUtility.Convert.GetString(printGroupData.Rows[i]["QtyPerCTN"]);
                    if (articleSize.ToString().IndexOf(string.Format("{0}{1}", MyUtility.Convert.GetString(printGroupData.Rows[i]["Article"]), MyUtility.Convert.GetString(printGroupData.Rows[i]["SizeCode"]))) < 0)
                    {
                        worksheet.Cells[excelRow, 20] = MyUtility.Convert.GetString(printGroupData.Rows[i]["OQty"]);
                        worksheet.Cells[excelRow, 21] = MyUtility.Convert.GetString(printGroupData.Rows[i]["TtlShipQty"]);
                        articleSize.Append(string.Format("{0}{1},", MyUtility.Convert.GetString(printGroupData.Rows[i]["Article"]), MyUtility.Convert.GetString(printGroupData.Rows[i]["SizeCode"])));
                    }
                }
                else
                {
                    int printRec = 1;
                    DataRow[] printList = printData.Select(string.Format("Article = '{0}' and SizeCode = '{1}' and Seq > '{2}' and QtyPerCTN = {3}", MyUtility.Convert.GetString(printGroupData.Rows[i]["Article"]), MyUtility.Convert.GetString(printGroupData.Rows[i]["SizeCode"]), seq, MyUtility.Convert.GetString(printGroupData.Rows[i]["QtyPerCTN"])), "Seq");
                    foreach (DataRow dr in printList)
                    {
                        if (printRec == 1)
                        {
                            excelRow++;

                            // 若資料會超過262筆，就先插入一筆Record，最後再把多的資料刪除
                            if (excelRow >= chk1)
                            {
                                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(excelRow)), Type.Missing).EntireRow;
                                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                                Marshal.ReleaseComObject(rngToInsert);
                            }

                            if (article != MyUtility.Convert.GetString(dr["Article"]) || size != MyUtility.Convert.GetString(dr["SizeCode"]) || qtyPerCTN != MyUtility.Convert.GetInt(dr["QtyPerCTN"]))
                            {
                                worksheet.Cells[excelRow, 2] = MyUtility.Convert.GetString(dr["SizeCode"]);
                                worksheet.Cells[excelRow, 3] = MyUtility.Convert.GetString(dr["SizeSpec"]);
                                size = MyUtility.Convert.GetString(dr["SizeCode"]);
                                qtyPerCTN = MyUtility.Convert.GetInt(dr["QtyPerCTN"]);
                                worksheet.Cells[excelRow, 4] = MyUtility.Convert.GetString(dr["QtyPerCTN"]);
                            }

                            if (article != MyUtility.Convert.GetString(dr["Article"]))
                            {
                                worksheet.Cells[excelRow, 1] = MyUtility.Convert.GetString(dr["Article"]) + ' ' + MyUtility.Convert.GetString(dr["Color"]);
                                article = MyUtility.Convert.GetString(dr["Article"]);
                            }
                        }

                        worksheet.Cells[excelRow, printRec + 4] = MyUtility.Convert.GetString(dr["CTNStartNo"]);
                        if (articleSize.ToString().IndexOf(string.Format("{0}{1}", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]))) < 0)
                        {
                            worksheet.Cells[excelRow, 20] = MyUtility.Convert.GetString(dr["OQty"]);
                            worksheet.Cells[excelRow, 21] = MyUtility.Convert.GetString(dr["TtlShipQty"]);
                            articleSize.Append(string.Format("{0}{1},", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"])));
                        }

                        seq = MyUtility.Convert.GetString(dr["Seq"]);
                        printRec++;
                        printCtnCount++;

                        if (printRec == 16)
                        {
                            printRec = 1;
                        }
                    }
                }
            }

            // 刪除多餘的Row
            if (excelRow >= chk2)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[excelRow + 1, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[excelRow + 1, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }
            else
            {
                for (int i = excelRow + 1; i <= chk2; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[excelRow + 1, Type.Missing];
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }
            }

            // Carton Dimension:
            excelRow++;
            StringBuilder ctnDimension = new StringBuilder();
            foreach (DataRow dr in ctnDim.Rows)
            {
                ctnDimension.Append(string.Format("{0} / {1} / {2} {3}, {4}  \r\n", MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["Description"]), MyUtility.Convert.GetString(dr["Dimension"]), MyUtility.Convert.GetString(dr["CtnUnit"]), MyUtility.Convert.GetString(dr["CTN"])));
            }

            foreach (DataRow dr in qtyCtn.Rows)
            {
                if (!MyUtility.Check.Empty(dr["Article"]))
                {
                    ctnDimension.Append(string.Format("{0} -> {1} / {2}, ", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"])));
                }
            }

            string cds = ctnDimension.Length > 0 ? ctnDimension.ToString().Substring(0, ctnDimension.ToString().Length - 2) : string.Empty;
            string[] cdsab = cds.Split('\r');
            int cdsi = 0;
            int cdsl = 150;
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

            cdsi += cdsab.Length - 1;

            for (int i = 1; i <= cdsi; i++)
            {
                Microsoft.Office.Interop.Excel.Range rangeRowCD = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[excelRow, Type.Missing];
                rangeRowCD.RowHeight = 19.5 * (i + 1);
                Marshal.ReleaseComObject(rangeRowCD);
            }

            worksheet.Cells[excelRow, 3] = ctnDimension.Length > 0 ? ctnDimension.ToString().Substring(0, ctnDimension.ToString().Length - 2) : string.Empty;

            // 填Remarks
            excelRow = excelRow + 2;
            worksheet.Cells[excelRow, 2] = MyUtility.Convert.GetString(packlistData["Remark"]);
            string tmp = MyUtility.Convert.GetString(specialInstruction);

            string[] tmpab = tmp.Split('\r');
            int ctmpc = 0;
            int l = 150;
            foreach (string tmpc in tmpab)
            {
                if (tmpc.Length > l)
                {
                    int h = tmpc.Length / l;
                    for (int i = 0; i < h; i++)
                    {
                        ctmpc += 1;
                    }
                }

                ctmpc += 1;
            }

            // 填Special Instruction
            // 先取得Special Instruction總共有幾行
            #region 舊寫法SpecialInstruction 有幾行資料,就多加幾行空白
            /*
             *原本寫法是SpecialInstruction 有幾行資料,就多加幾行空白
             */

            // for (int i = 1; ; i++)
            // {
            //    if (i > 1)
            //    {
            //        startIndex = endIndex + 2;
            //    }
            //    if (SpecialInstruction.IndexOf("\r\n", startIndex) > 0)
            //    {
            //        endIndex = SpecialInstruction.IndexOf("\r\n", startIndex);
            //    }
            //    else
            //    {

            // break;
            //    }
            // }
            //
            #endregion

            // 調整寫法, 只需要多加兩行空白即可
            int dataRow = 2 + ctmpc;
            excelRow++;

            if (dataRow > 2)
            {
                for (int i = 3; i < dataRow; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(excelRow + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    rngToInsert.RowHeight = 19.5;
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            // 因為SpecialInstruction中有參雜=等特殊字元，在開頭加單引號強迫轉為字串避免<Exception from HRESULT: 0x800A03EC>問題發生
            worksheet.Cells[excelRow, 3] = "'" + specialInstruction;

            excelRow = excelRow + (dataRow > 2 ? dataRow - 1 : 2);

            // 貼圖
            int picCount = 0;
            excelRow = excelRow + 5;
            foreach (DataRow dr in clipData.Rows)
            {
                if (picCount >= 4)
                {
                    break;
                }

                picCount++;
                string excelRng = picCount % 2 == 1 ? (picCount > 2 ? string.Format("A{0}", MyUtility.Convert.GetString(excelRow + 30)) : string.Format("A{0}", MyUtility.Convert.GetString(excelRow))) : (picCount > 2 ? string.Format("K{0}", MyUtility.Convert.GetString(excelRow + 30)) : string.Format("K{0}", MyUtility.Convert.GetString(excelRow)));
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(excelRng, Type.Missing);
                rngToInsert.Select();
                float picLeft, picTop;
                picLeft = Convert.ToSingle(rngToInsert.Left);
                picTop = Convert.ToSingle(rngToInsert.Top);
                string targetFile = Env.Cfg.ClipDir + "\\" + MyUtility.Convert.GetString(dr["Year"]) + Convert.ToString(dr["Month"]).PadLeft(2, '0') + "\\" + MyUtility.Convert.GetString(dr["TableName"]) + MyUtility.Convert.GetString(dr["PKey"]) + MyUtility.Convert.GetString(dr["SourceFile"]).Substring(MyUtility.Convert.GetString(dr["SourceFile"]).LastIndexOf('.'));
                worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, picLeft, picTop, 450, 400);
                Marshal.ReleaseComObject(rngToInsert);
            }

            if (!visRow3)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[3, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Packing_P03_PackingGuideReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }
        #endregion

        #region Packing Barcode Print

        /// <summary>
        /// PackingBarcodePrint(string,string,string,DataTable)
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <param name="ctnStartNo">ctnStartNo</param>
        /// <param name="ctnEndNo">ctnEndNo</param>
        /// <param name="printBarcodeData">printBarcodeData</param>
        /// <returns>DualResult</returns>
        public static DualResult PackingBarcodePrint(string packingListID, string ctnStartNo, string ctnEndNo, out DataTable printBarcodeData)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select  pd.ID
        , pd.OrderID
        , pd.CTNStartNo
        , CTNQty = (select CTNQty 
                    from PackingList WITH (NOLOCK) 
                    where ID = pd.ID)
        , PONo = isnull((select CustPONo from Orders WITH (NOLOCK) where ID = pd.OrderID),'')
		, SizeCode = case 
						when checkMixSize.value > 1 then 'Mix'
						else pd.SizeCode
					 end
		, ShipQty = case 
						when checkMixSize.value > 1 then TTLShipQty.value
						else pd.ShipQty
					 end
		, checkMixSize.value
        , o.BuyerDelivery
        , pd.SCICtnNo
		, pd.CustCTN
        , o.StyleID
from PackingList_Detail pd WITH (NOLOCK) 
left join orders o with(nolock) on o.id = pd.OrderID
outer apply (
	select value = count (1)
	from (
		select distinct checkPD.sizecode
		from PackingList_Detail checkPD With (NoLock)
		where pd.ID = checkPD.ID
			  and pd.CTNStartNo = checkPD.CTNStartNo
	) distinctSize
) checkMixSize
outer apply (
    select value = sum (ShipQty)
    from PackingList_Detail checkPD With (NoLock)
	where checkMixSize.value > 1
          and pd.ID = checkPD.ID
		  and pd.CTNStartNo = checkPD.CTNStartNo
) TTLShipQty
where pd.CTNQty > 0");
            if (!MyUtility.Check.Empty(packingListID))
            {
                sqlCmd.Append(string.Format(" and pd.ID = '{0}'", packingListID));
            }

            if (!MyUtility.Check.Empty(ctnStartNo))
            {
                sqlCmd.Append(string.Format(" and pd.CTNStartNo >= {0}", ctnStartNo));
            }

            if (!MyUtility.Check.Empty(ctnEndNo))
            {
                sqlCmd.Append(string.Format(" and pd.CTNStartNo <= {0}", ctnEndNo));
            }

            sqlCmd.Append(" order by pd.Seq");
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printBarcodeData);
            return result;
        }
        #endregion

        #region Packing MD Form Report

        /// <summary>
        /// PackingListToExcel_PackingMDFormReport
        /// </summary>
        /// <param name="xltxName">xltxName</param>
        /// <param name="masterdata">masterdata</param>
        /// <param name="printData">printData</param>
        public static void PackingListToExcel_PackingMDFormReport(string xltxName, DataRow masterdata, DataTable[] printData)
        {
            string strXltName = Env.Cfg.XltPathDir + xltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            decimal pageCT = 0;

            #region 先準備複製幾頁
            foreach (DataTable dT in printData)
            {
                decimal pcount = Math.Ceiling((decimal)dT.Rows.Count / 50);
                pageCT += pcount;
            }

            if (pageCT > 1)
            {
                for (int i = 0; i < pageCT; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[1];
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 1];
                        worksheet1.Copy(worksheetn);
                    }
                }
            }
            #endregion
            string alias = MyUtility.GetValue.Lookup($@"select Alias from Country where id = '{masterdata["Dest"]}'");
            int page = 1;

            // by custpono
            foreach (DataTable dT in printData)
            {
                decimal pcount = Math.Ceiling((decimal)dT.Rows.Count / 50);
                for (int i = 0; i < pcount; i++)
                {
                    #region sqlcommand
                    string orderids = $@"
select OrderID = stuff((
	select concat('/',a.OrderID)
	from (
		select distinct pd.OrderID
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	order by OrderID
	for xml path('')
),1,1,'')
";
                    string styles = $@"
select OrderID = stuff((
	select concat('/',a.StyleID)
	from (
		select distinct o.StyleID
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	order by StyleID
	for xml path('')
),1,1,'')";
                    string colors = $@"
select Color = stuff((
	select concat('/',a.Article,' ',a.Color)
	from (
		select distinct pd.Article,pd.Color
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	for xml path('')
),1,1,'')
";
                    string buyerDelivery = $@"
select BuyerDelivery = stuff((
	select concat('/',FORMAT(a.BuyerDelivery,'MM/dd/yyyy'))
	from (
		select distinct o.BuyerDelivery
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	order by BuyerDelivery
	for xml path('')
),1,1,'')
";
                    #endregion
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[page];
                    worksheet.Cells[1, 3] = MyUtility.Convert.GetString(dT.Rows[0]["CustPONo"]) + " of " + MyUtility.Convert.GetString(masterdata["CTNQty"]);
                    worksheet.Cells[5, 2] = MyUtility.GetValue.Lookup(orderids);
                    worksheet.Cells[6, 2] = MyUtility.GetValue.Lookup(styles);
                    worksheet.Cells[7, 2] = dT.Rows[0]["CustPONo"];
                    worksheet.Cells[8, 2] = MyUtility.GetValue.Lookup(colors);
                    worksheet.Cells[5, 6] = masterdata["CTNQty"];
                    worksheet.Cells[6, 6] = MyUtility.Convert.GetInt(masterdata["ShipQty"]).ToString("#,#.#") + " pcs.";
                    worksheet.Cells[8, 6] = MyUtility.GetValue.Lookup(buyerDelivery);
                    worksheet.Cells[5, 8] = alias;
                    for (int j = i * 50, k = 10; j < dT.Rows.Count && j < (i + 1) * 50; j++, k++)
                    {
                        worksheet.Cells[k, 2] = dT.Rows[j]["CTNStartNo"];
                        worksheet.Cells[k, 3] = dT.Rows[j]["SizeCode"];
                        worksheet.Cells[k, 4] = dT.Rows[j]["ShipQty"];
                    }

                    page++;
                }
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Packing_P03_PackingMDFormReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);

            // Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }
        #endregion

        #region MD Form Report

        /// <summary>
        /// QueryPackingMDform
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <param name="printData">printData</param>
        /// <returns>DualResult</returns>
        public static DualResult QueryPackingMDform(string packingListID, out DataTable[] printData)
        {
            string orderidsql = $@"select o.CustPONo from PackingList_Detail pd left join orders o on o.id = pd.OrderID where pd.id = '{packingListID}' group by CustPONo order by CustPONo";
            DataTable custPONoDT;
            DualResult result;
            result = DBProxy.Current.Select(null, orderidsql, out custPONoDT);
            if (!result)
            {
                printData = null;
                return result;
            }

            StringBuilder sqlCmd = new StringBuilder();
            for (int i = 0; i < custPONoDT.Rows.Count; i++)
            {
                sqlCmd.Append($@"
select CTNStartNo,a.SizeCode,b.ShipQty,o.CustPONo
from PackingList_Detail pd
left join orders o on o.id = pd.OrderID
outer apply(
	select SizeCode = stuff((
	select concat('/',SizeCode)
	from PackingList_Detail pd2
	where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo
	order by pd2.seq
	for xml path('')
	),1,1,'')
)a
outer apply(select ct=count(1)from PackingList_Detail pd2 where pd2.id=pd.id and pd2.CTNStartNo=pd.CTNStartNo and pd2.OrderID=pd.OrderID)b1
outer apply(
	select ShipQty = stuff((
	select iif(b1.ct>1 ,concat('/',SizeCode,' ',ShipQty), concat('/',ShipQty))
	from PackingList_Detail pd2
	where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo
	order by pd2.seq
	for xml path('')),1,1,'')
)b
where pd.id = '{packingListID}' and o.CustPONo = '{custPONoDT.Rows[i]["CustPONo"]}'
group by CTNStartNo,a.SizeCode,b.ShipQty,o.CustPONo
order by min(pd.seq)
");
            }

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            return result;
        }
        #endregion

        #region Packing Carton Weighing Report

        /// <summary>
        /// PackingListToExcel_PackingCartonWeighingReport
        /// </summary>
        /// <param name="xltxName">xltxName</param>
        /// <param name="masterdata">masterdata</param>
        /// <param name="printData">printData</param>
        public static void PackingListToExcel_PackingCartonWeighingReport(string xltxName, DataRow masterdata, DataTable[] printData)
        {
            string strXltName = Env.Cfg.XltPathDir + xltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            decimal pageCT = 0;

            #region 先準備複製幾頁
            foreach (DataTable dT in printData)
            {
                decimal pcount = Math.Ceiling((decimal)dT.Rows.Count / 100);
                pageCT += pcount;
            }

            if (pageCT > 1)
            {
                for (int i = 0; i < pageCT; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[1];
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 1];
                        worksheet1.Copy(worksheetn);
                    }
                }
            }
            #endregion
            string alias = MyUtility.GetValue.Lookup($@"select Alias from Country where id = '{masterdata["Dest"]}'");
            int page = 1;

            // by custpono
            foreach (DataTable dT in printData)
            {
                decimal pcount = Math.Ceiling((decimal)dT.Rows.Count / 100);
                for (int i = 0; i < pcount; i++)
                {
                    #region sqlcommand
                    string orderids = $@"
select OrderID = stuff((
	select concat('/',a.OrderID)
	from (
		select distinct pd.OrderID
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	order by OrderID
	for xml path('')
),1,1,'')
";
                    string styles = $@"
select StyleID = stuff((
	select concat('/',a.StyleID)
	from (
		select distinct o.StyleID
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	order by StyleID
	for xml path('')
),1,1,'')";
                    string seasonIDs = $@"
select SeasonID = stuff((
	select concat('/',a.SeasonID)
	from (
		select distinct o.SeasonID
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	order by SeasonID
	for xml path('')
),1,1,'')";
                    string colors = $@"
select Color = stuff((
	select concat('/',a.Article,' ',a.Color)
	from (
		select distinct pd.Article,pd.Color
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	for xml path('')
),1,1,'')
";
                    string buyerDelivery = $@"
select BuyerDelivery = stuff((
	select concat('/',FORMAT(a.BuyerDelivery,'MM/dd/yyyy'))
	from (
		select distinct o.BuyerDelivery
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	order by BuyerDelivery
	for xml path('')
),1,1,'')
";
                    string kit = $@"
select Kit = stuff((
	select concat('/',Kit)
	from (
		select distinct c.Kit,o.CustCDID
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		inner join CustCD c on c.ID=o.CustCDID and c.BrandID = o.BrandID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{dT.Rows[0]["CustPONo"]}'
	)a
	order by Kit
	for xml path('')
),1,1,'') ";
                    #endregion
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[page];
                    worksheet.Cells[1, 2] = MyUtility.Convert.GetString(dT.Rows[0]["CustPONo"]) + " box of " + MyUtility.Convert.GetString(masterdata["CTNQty"]);
                    worksheet.Cells[3, 2] = masterdata["FactoryID"];
                    worksheet.Cells[4, 2] = dT.Rows[0]["CustPONo"];
                    worksheet.Cells[5, 2] = MyUtility.GetValue.Lookup(orderids);
                    worksheet.Cells[6, 2] = MyUtility.GetValue.Lookup(styles);
                    worksheet.Cells[7, 2] = masterdata["BrandID"];
                    worksheet.Cells[3, 7] = MyUtility.GetValue.Lookup(seasonIDs);
                    worksheet.Cells[4, 7] = MyUtility.GetValue.Lookup(colors);
                    worksheet.Cells[5, 7] = MyUtility.GetValue.Lookup(buyerDelivery);
                    worksheet.Cells[6, 7] = masterdata["ShipModeID"];
                    worksheet.Cells[7, 7] = masterdata["CustCDID"];
                    worksheet.Cells[8, 7] = MyUtility.GetValue.Lookup(kit);
                    for (int j = i * 100, k = 10; j < dT.Rows.Count && j < (i + 1) * 100; j++, k++)
                    {
                        if (k < 10 + 50)
                        {
                            worksheet.Cells[k, 1] = dT.Rows[j]["CTNStartNo"];
                            worksheet.Cells[k, 2] = dT.Rows[j]["SizeCode"];
                            worksheet.Cells[k, 3] = dT.Rows[j]["ShipQty"];
                        }
                        else
                        {
                            worksheet.Cells[k - 50, 6] = dT.Rows[j]["CTNStartNo"];
                            worksheet.Cells[k - 50, 7] = dT.Rows[j]["SizeCode"];
                            worksheet.Cells[k - 50, 8] = dT.Rows[j]["ShipQty"];
                        }
                    }

                    page++;
                }
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Packing_P03_PackingMDFormReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);

            // Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }
        #endregion

        #region Carton Weighing Form

        /// <summary>
        /// QueryPackingCartonWeighingForm
        /// </summary>
        /// <param name="packingListID">packingListID</param>
        /// <param name="printData">printData</param>
        /// <returns>DualResult</returns>
        public static DualResult QueryPackingCartonWeighingForm(string packingListID, out DataTable[] printData)
        {
            string orderidsql = $@"select o.CustPONo from PackingList_Detail pd left join orders o on o.id = pd.OrderID where pd.id = '{packingListID}' group by CustPONo order by CustPONo";
            DataTable custPONoDT;
            DualResult result;
            result = DBProxy.Current.Select(null, orderidsql, out custPONoDT);
            if (!result)
            {
                printData = null;
                return result;
            }

            StringBuilder sqlCmd = new StringBuilder();
            for (int i = 0; i < custPONoDT.Rows.Count; i++)
            {
                sqlCmd.Append($@"
select CTNStartNo,a.SizeCode,b.ShipQty,o.CustPONo
from PackingList_Detail pd
left join orders o on o.id = pd.OrderID
outer apply(
	select SizeCode = stuff((
	select concat('/',SizeCode)
	from PackingList_Detail pd2
	where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo
	order by pd2.seq
	for xml path('')
	),1,1,'')
)a
outer apply(select ct=count(1)from PackingList_Detail pd2 where pd2.id=pd.id and pd2.CTNStartNo=pd.CTNStartNo and pd2.OrderID=pd.OrderID)b1
outer apply(
	select ShipQty = stuff((
	select iif(b1.ct>1 ,concat('/',SizeCode,' ',ShipQty), concat('/',ShipQty))
	from PackingList_Detail pd2
	where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo
	order by pd2.seq
	for xml path('')),1,1,'')
)b
where pd.id = '{packingListID}' and o.CustPONo = '{custPONoDT.Rows[i]["CustPONo"]}'
group by CTNStartNo,a.SizeCode,b.ShipQty,o.CustPONo
order by min(pd.seq)
");
            }

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            return result;
        }
        #endregion

        #region Get SCICtnNo P03/P04/P05

        /// <summary>
        /// GetSCICtnNo
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="id">string</param>
        /// <param name="type">type</param>
        /// <returns><see cref="GetSCICtnNo"/></returns>
        public static bool GetSCICtnNo(DataTable dt, string id, string type)
        {
            if (type.EqualString("IsDetailInserting"))
            {
                string sciCtnNo = MyUtility.GetValue.GetID(Env.User.Keyword + string.Empty, "PackingList_Detail", DateTime.Today, 3, "SCICtnNo", null);
                string sciCtnNoleft = sciCtnNo.Substring(0, 9);
                int sciNo = MyUtility.Convert.GetInt(sciCtnNo.Substring(9));
                var ctnlist = dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted)
                    .GroupBy(s => s["CTNStartNo"]).Select(group => new { CTNStartNo = group.Key });

                foreach (var item in ctnlist)
                {
                    string ctnStartNo = item.CTNStartNo.ToString();
                    foreach (DataRow dr in dt.AsEnumerable()
                        .Where(w => w.RowState != DataRowState.Deleted && MyUtility.Convert.GetString(w["CTNStartNo"]).EqualString(ctnStartNo)).ToList())
                    {
                        dr["SCICtnNo"] = sciCtnNo;
                    }

                    sciNo++;
                    sciCtnNo = sciCtnNoleft + sciNo.ToString().PadLeft(6, '0');
                }
            }
            else
            {
                string sqlcmd = $@"select distinct CTNStartNo,SCICtnNo from PackingList_Detail where id ='{id}'";
                DataTable ctnDt;
                DualResult result = DBProxy.Current.Select("Production", sqlcmd, out ctnDt);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return false;
                }
                else
                {
                    // 維持此ID原有的 (CTNStartNo對應SCICtnNo)
                    foreach (DataRow ctnDtrow in ctnDt.Rows)
                    {
                        foreach (DataRow dr in dt.AsEnumerable()
                            .Where(w => w.RowState != DataRowState.Deleted
                                && MyUtility.Convert.GetString(w["CTNStartNo"]).EqualString(MyUtility.Convert.GetString(ctnDtrow["CTNStartNo"]))))
                        {
                            dr["SCICtnNo"] = ctnDtrow["SCICtnNo"];
                        }
                    }
                }

                string sciCtnNo = MyUtility.GetValue.GetID(Env.User.Keyword + string.Empty, "PackingList_Detail", DateTime.Today, 3, "SCICtnNo", null);
                string sciCtnNoleft = sciCtnNo.Substring(0, 9);
                int sciNo = MyUtility.Convert.GetInt(sciCtnNo.Substring(9));
                var ctnlist = dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted
                    && !ctnDt.AsEnumerable().Any(r2 => MyUtility.Convert.GetString(w["CTNStartNo"]).EqualString(MyUtility.Convert.GetString(r2["CTNStartNo"]))))
                    .GroupBy(s => s["CTNStartNo"]).Select(group => new { CTNStartNo = group.Key });

                foreach (var item in ctnlist)
                {
                    string ctnStartNo = item.CTNStartNo.ToString();
                    foreach (DataRow dr in dt.AsEnumerable()
                        .Where(w => w.RowState != DataRowState.Deleted && MyUtility.Convert.GetString(w["CTNStartNo"]).EqualString(ctnStartNo)).ToList())
                    {
                        dr["SCICtnNo"] = sciCtnNo;
                    }

                    sciNo++;
                    sciCtnNo = sciCtnNoleft + sciNo.ToString().PadLeft(6, '0');
                }
            }

            return true;
        }
        #endregion

        #region

        /// <summary>
        /// PackingP02CreateSCICtnNo
        /// </summary>
        /// <param name="id">PackingList_Detail.ID</param>
        /// <returns>DualResult</returns>
        public static DualResult PackingP02CreateSCICtnNo(string id)
        {
            DataTable packinglist_detaildt;
            string sqlpld = $@"select Ukey,SCICtnNo,id,OrderID,OrderShipmodeSeq,CTNStartNo,Article,SizeCode from PackingList_Detail where id = '{id}' order by seq";
            DualResult result = DBProxy.Current.Select(null, sqlpld, out packinglist_detaildt);
            if (!result)
            {
                return result;
            }

            if (!GetSCICtnNo(packinglist_detaildt, id, "IsDetailInserting"))
            {
                return new DualResult(false, new Exception("GetSCICtnNo Error"));
            }

            string sqlCreateSCICtnNo = $@"
update pd2 set 
	SCICtnNo = pd.SCICtnNo
from  PackingList_Detail pd2
inner join #tmp pd  on pd2.Ukey	= pd.Ukey				
";
            DataTable dt;
            result = MyUtility.Tool.ProcessWithDatatable(packinglist_detaildt, "SCICtnNo,Ukey", sqlCreateSCICtnNo, out dt);
            return result;
        }
        #endregion

        /// <summary>
        /// 更新 PackingList的 EstCTNBooking 與 EstCTNArrive
        /// </summary>
        /// <param name="id">PackingList.ID</param>
        /// <param name="dtBooking">PackingList.EstCTNBooking (yyyy/MM/dd)</param>
        /// <param name="dtArrive">PackingList.EstCTNArrive (yyyy/MM/dd)</param>
        /// <returns>bool</returns>
        public static DualResult UpdPackingListCTNBookingAndArrive(string id, DateTime? dtBooking, DateTime? dtArrive)
        {
            DualResult result;
            if (id.Empty())
            {
                return new DualResult(false, "ID is empty");
            }

            string booking = dtBooking.HasValue ? "'" + dtBooking.Value.ToString("yyyy/MM/dd") + "'" : "NULL";
            string arrive = dtArrive.HasValue ? "'" + dtArrive.Value.ToString("yyyy/MM/dd") + "'" : "NULL";
            string sqlCmd = $@"
update PackingList 
    set EstCTNBooking = {booking}
        , EstCTNArrive = {arrive}
where ID = '{id}'";

            result = DBProxy.Current.Execute(null, sqlCmd);
            return result;
        }

        #region P03 Save Check
        private static StringBuilder Ctn_no_combine(string sP, string seq, DataTable detailDatas)
        {
            StringBuilder ctn_no = new StringBuilder();
            var cnt_list = from r2 in detailDatas.AsEnumerable()
                           where r2.Field<string>("OrderID") == sP &&
                                  r2.Field<string>("OrderShipmodeSeq") == seq
                           select new { cnt_no = r2.Field<string>("CTNStartNo") };
            foreach (var cnt in cnt_list)
            {
                ctn_no.Append("," + cnt.cnt_no);
            }

            ctn_no.Remove(0, 1);
            return ctn_no;
        }

        /// <summary>
        /// 檢查OrderID+Seq不可以重複建立
        /// </summary>
        /// <param name="orderid">orderid</param>
        /// <param name="seq">seq</param>
        /// <param name="packID">packID</param>
        /// <returns>bool</returns>
        public static bool P03CheckDouble_SpSeq(string orderid, string seq, string packID)
        {
            if (MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", orderid, seq, packID)))
            {
                MyUtility.Msg.WarningBox("SP No:" + orderid + ", Seq:" + seq + " already exist in packing list, can't be create again!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// P03 Save Check
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        /// <param name="detailDatas">detailDatas</param>
        /// <param name="detailGrid">detailGrid</param>
        /// <returns>bool</returns>
        public static bool P03SaveCheck(DataRow currentMaintain, DataTable detailDatas, Grid detailGrid = null)
        {
            DualResult result;
            #region 刪除表身SP No.或Qty為空白的資料
            for (int j = detailDatas.Rows.Count - 1; j >= 0; j--)
            {
                if (detailDatas.Rows[j].RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Check.Empty(detailDatas.Rows[j]["OrderID"]) || MyUtility.Check.Empty(detailDatas.Rows[j]["ShipQty"]))
                    {
                        detailDatas.Rows[j].Delete();
                    }
                }
            }
            #endregion

            var checkDetailListNoDeleted = detailDatas.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).OrderBy(u => u["ID"]).ThenBy(u => u["OrderShipmodeSeq"]);

            #region 檢查表頭的CustCD與表身所有SP的 Orders.custcdid是否相同
            DataTable dtCheckCustCD;
            List<SqlParameter> listCheckCustCDSqlParameter = new List<SqlParameter>();
            listCheckCustCDSqlParameter.Add(new SqlParameter("@CustCD", currentMaintain["CustCDID"]));
            string strCheckCustCD = @"
select OrderID
from #tmp
outer apply (
    select value = isnull (o.CustCDID, '')
    from Orders o
    where #tmp.OrderID = o.ID
) CustCD
where CustCD.value != @CustCD
      or CustCD.value is null";

            if (detailDatas.Rows.Count > 0)
            {
                result = MyUtility.Tool.ProcessWithDatatable(detailDatas, null, strCheckCustCD, out dtCheckCustCD, paramters: listCheckCustCDSqlParameter);
                if (result == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return false;
                }
                else if (dtCheckCustCD != null && dtCheckCustCD.Rows.Count > 0)
                {
                    MyUtility.Msg.WarningBox("CustCD are different, please check!");
                    return false;
                }
            }

            #endregion

            // 刪除表身SP No.或Qty為空白的資料，表身的CTN#, Ref No., Color Way與Size不可以為空值，計算CTNQty, ShipQty, NW, GW, NNW, CBM，重算表身Grid的Bal. Qty
            int i = 0, ctnQty = 0, shipQty = 0, ttlShipQty = 0, needPackQty = 0, count = 0;
            double nw = 0.0, gw = 0.0, nnw = 0.0, cbm = 0.0;
            string filter = string.Empty, sqlCmd;
            bool isNegativeBalQty = false;
            DataTable needPackData, tmpPackData;
            DualResult selectResult;
            DataRow[] detailData;

            // 準備needPackData的Schema
            sqlCmd = "select OrderID, OrderShipmodeSeq, Article, SizeCode, ShipQty as Qty from PackingList_Detail WITH (NOLOCK) where ID = ''";
            if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out needPackData)))
            {
                MyUtility.Msg.WarningBox("Query  schema fail!");
                return false;
            }

            foreach (DataRow dr in checkDetailListNoDeleted)
            {
                #region
                bool isAlreadyCreated = !P03CheckDouble_SpSeq(dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), currentMaintain["ID"].ToString());
                if (isAlreadyCreated)
                {
                    return false;
                }
                #endregion

                #region 表身的CTN#, Ref No., Color Way與Size不可以為空值
                if (MyUtility.Check.Empty(dr["CTNStartNo"]))
                {
                    MyUtility.Msg.WarningBox("< CTN# >  can't empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["RefNo"]))
                {
                    MyUtility.Msg.WarningBox("< Ref No. >  can't empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["Article"]))
                {
                    MyUtility.Msg.WarningBox("< ColorWay >  can't empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["SizeCode"]))
                {
                    MyUtility.Msg.WarningBox("< Size >  can't empty!");
                    return false;
                }
                #endregion

                #region 填入Seq欄位值
                i = i + 1;
                dr["Seq"] = Convert.ToString(i).PadLeft(6, '0');
                #endregion

                #region 計算CTNQty, ShipQty, NW, GW, NNW, CBM
                ctnQty = ctnQty + MyUtility.Convert.GetInt(dr["CTNQty"]);
                shipQty = shipQty + MyUtility.Convert.GetInt(dr["ShipQty"]);
                nw = MyUtility.Math.Round(nw + MyUtility.Convert.GetDouble(dr["NW"]), 3);
                gw = MyUtility.Math.Round(gw + MyUtility.Convert.GetDouble(dr["GW"]), 3);
                nnw = MyUtility.Math.Round(nnw + MyUtility.Convert.GetDouble(dr["NNW"]), 3);
                if (MyUtility.Check.Empty(dr["CTNQty"]) || MyUtility.Convert.GetInt(dr["CTNQty"]) > 0)
                {
                    // ISP20181015 CBM抓到小數點後4位
                    cbm = cbm + (MyUtility.Convert.GetDouble(MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo")) * MyUtility.Convert.GetInt(dr["CTNQty"]));
                }

                #endregion

                #region 重新計算表身每一個紙箱的材積重
                DataRow drLocalitem;
                string sqlLocalItem = $@"
select 
[BookingVW] = isnull(round(
	(CtnLength * CtnWidth * CtnHeight * POWER(rate.value,3)) /6000
,2),0)
,[APPEstAmtVW] = isnull(round(
	(CtnLength * CtnWidth * CtnHeight * POWER(rate.value,3)) /5000
,2),0)
from LocalItem
outer apply(
	select value = ( case when CtnUnit='MM' then 0.1 else dbo.getUnitRate(CtnUnit,'CM') end)
) rate
where RefNo='{dr["Refno"]}'";
                if (MyUtility.Check.Empty(dr["CTNQty"]) || MyUtility.Convert.GetInt(dr["CTNQty"]) > 0)
                {
                    if (MyUtility.Check.Seek(sqlLocalItem, out drLocalitem))
                    {
                        dr["AppBookingVW"] = MyUtility.Convert.GetDecimal(drLocalitem["BookingVW"]) * MyUtility.Convert.GetInt(dr["CTNQty"]);
                        dr["APPEstAmtVW"] = MyUtility.Convert.GetDecimal(drLocalitem["APPEstAmtVW"]) * MyUtility.Convert.GetInt(dr["CTNQty"]);
                    }
                }
                else
                {
                    dr["AppBookingVW"] = 0;
                    dr["APPEstAmtVW"] = 0;
                }

                #endregion

                #region 重算表身Grid的Bal. Qty

                // 目前還有多少衣服尚未裝箱
                // needPackQty = 0;
                filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                detailData = needPackData.Select(filter);
                if (detailData.Length <= 0)
                {
                    // 撈取此SP+Seq尚未裝箱的數量
                    sqlCmd = string.Format(
                        @"

select OrderID = oqd.Id
	   , OrderShipmodeSeq = oqd.Seq
	   , oqd.Article
	   , oqd.SizeCode
	   , Qty = (oqd.Qty - isnull(AccuShipQty.value, 0) - isnull(InvAdjustDiffQty.value, 0))
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
outer apply (
	select value = sum(pld.ShipQty)
	from PackingList pl With (NoLock)
	inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
	where pl.Status = 'Confirmed'
		  and pl.ID != '{0}'
		  and pld.OrderID = oqd.Id 
		  and pld.OrderShipmodeSeq = oqd.Seq
		  and pld.Article = oqd.Article
		  and pld.SizeCode = oqd.SizeCode		  
) AccuShipQty
outer apply (
	select value = sum (iaq.DiffQty)
	from InvAdjust ia WITH (NOLOCK)
	inner join InvAdjust_Qty iaq WITH (NOLOCK) on iaq.ID = ia.ID 
											 
	where ia.OrderID = oqd.ID 
		  and ia.OrderShipmodeSeq = oqd.Seq
		  and iaq.Article = oqd.Article 
		  and iaq.SizeCode = oqd.SizeCode
) InvAdjustDiffQty
where oqd.Id = '{1}'
	  and oqd.Seq = '{2}'",
                        currentMaintain["ID"].ToString(),
                        dr["OrderID"].ToString(),
                        dr["OrderShipmodeSeq"].ToString());
                    if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out tmpPackData)))
                    {
                        MyUtility.Msg.WarningBox("Query pack qty fail!");
                        return false;
                    }
                    else
                    {
                        foreach (DataRow tpd in tmpPackData.Rows)
                        {
                            tpd.AcceptChanges();
                            tpd.SetAdded();
                            needPackData.ImportRow(tpd);
                        }
                    }
                }

                needPackQty = 0;
                filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}' and Article = '{2}' and SizeCode = '{3}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString());
                detailData = needPackData.Select(filter);
                if (detailData.Length > 0)
                {
                    needPackQty = MyUtility.Convert.GetInt(detailData[0]["Qty"].ToString());
                }

                // 加總表身特定Article/SizeCode的Ship Qty數量
                detailData = detailDatas.Select(filter);
                ttlShipQty = 0;
                if (detailData.Length != 0)
                {
                    foreach (DataRow dDr in detailData)
                    {
                        ttlShipQty = ttlShipQty + MyUtility.Convert.GetInt(dDr["ShipQty"].ToString());
                    }
                }

                dr["BalanceQty"] = needPackQty - ttlShipQty;
                if (needPackQty - ttlShipQty < 0)
                {
                    isNegativeBalQty = true;
                    if (detailGrid != null)
                    {
                        detailGrid.Rows[count].DefaultCellStyle.BackColor = Color.Pink;
                    }
                }
                #endregion

                count = count + 1;
            }

            #region ship mode 有變更時 check Order_QtyShip
            StringBuilder chk_ship_err = new StringBuilder();
            StringBuilder chk_seq_null = new StringBuilder();
            var check_chip_list = from r1 in checkDetailListNoDeleted
                                  group r1 by new
                                  {
                                      SP = r1.Field<string>("OrderID"),
                                      Seq = r1.Field<string>("OrderShipmodeSeq"),
                                  }
                                  into g
                                  select new
                                  {
                                      SP = g.Key.SP,
                                      Seq = g.Key.Seq,
                                  };
            foreach (var chk_item in check_chip_list)
            {
                if (MyUtility.Check.Empty(chk_item.Seq))
                {
                    chk_seq_null.Append("<SP> " + chk_item.SP + " <CTN#> [" + Ctn_no_combine(chk_item.SP, chk_item.Seq, detailDatas) + "]  \r\n");
                }
            }

            if (chk_seq_null.Length > 0)
            {
                chk_seq_null.Insert(0, " Seq can not empty , please check again:  \r\n");

                MyUtility.Msg.WarningBox(chk_seq_null.ToString());
                return false;
            }
            #endregion

            if (detailDatas.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Detail cannot be empty");
                return false;
            }

            // 檢查Refno是否有改變，若有改變提醒使用者通知採購團隊紙箱號碼改變。
            DataView dataView = detailDatas.DefaultView;
            DataTable dataTableDistinct = dataView.ToTable(true, "OrderID", "RefNo");
            StringBuilder warningmsg = new StringBuilder();
            warningmsg.Append("Please inform Purchase Team that the Carton Ref No. has been changed.");

            foreach (DataRow dt in dataTableDistinct.Rows)
            {
                if (!MyUtility.Check.Seek(string.Format("select * from LocalPO_Detail where OrderId='{0}'", dt["OrderID"].ToString())))
                {
                    continue;
                }

                if (!MyUtility.Check.Seek(string.Format("select * from LocalPO_Detail where OrderId='{0}' and Refno='{1}'", dt["OrderID"].ToString(), dt["RefNo"].ToString())))
                {
                    warningmsg.Append(Environment.NewLine + string.Format("SP#：<{0}>, RefNo：<{1}>.", dt["OrderID"].ToString(), dt["RefNo"].ToString()));
                }
            }

            if (warningmsg.ToString() != "Please inform Purchase Team that the Carton Ref No. has been changed.")
            {
                MyUtility.Msg.InfoBox(warningmsg.ToString());
            }

            // CTNQty, ShipQty, NW, GW, NNW, CBM
            currentMaintain["CTNQty"] = ctnQty;
            currentMaintain["ShipQty"] = shipQty;
            currentMaintain["NW"] = nw;
            currentMaintain["GW"] = gw;
            currentMaintain["NNW"] = nnw;
            currentMaintain["CBM"] = cbm;

            if (isNegativeBalQty)
            {
                MyUtility.Msg.WarningBox("Quantity entered is greater than order quantity!!");
                return false;
            }

            // 表身重新計算後,再判斷CBM or GW 是不是0
            if (MyUtility.Check.Empty(currentMaintain["CBM"]) || MyUtility.Check.Empty(currentMaintain["GW"]))
            {
                MyUtility.Msg.WarningBox("Ttl CBM and Ttl GW can't be empty!!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// P03 Update GMT
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        /// <param name="detailDatas">detailDatas</param>
        /// <returns>DualResult</returns>
        public static DualResult P03_UpdateGMT(DataRow currentMaintain, DataTable detailDatas)
        {
            DualResult result = new DualResult(true);
            if (!MyUtility.Check.Empty(currentMaintain["INVNo"]))
            {
                string sqlCmd = string.Format(
                    @"
select 
 isnull(sum(ShipQty),0) as ShipQty
,isnull(sum(CTNQty),0) as CTNQty
,isnull(sum(NW),0) as NW
,isnull(sum(GW),0) as GW
,isnull(sum(NNW),0) as NNW
,isnull(sum(CBM),0) as CBM
from PackingList WITH (NOLOCK) 
where INVNo = '{0}'
and ID != '{1}'",
                    currentMaintain["INVNo"].ToString(),
                    currentMaintain["ID"].ToString());

                DataTable summaryData;
                if (result = DBProxy.Current.Select(null, sqlCmd, out summaryData))
                {
                    string updateCmd = @"update GMTBooking
set TotalShipQty = @ttlShipQty, TotalCTNQty = @ttlCTNQty, TotalNW = @ttlNW, TotalNNW = @ttlNNW, TotalGW = @ttlGW, TotalCBM = @ttlCBM
where ID = @INVNo";
                    #region 準備sql參數資料
                    SqlParameter sp1 = new SqlParameter
                    {
                        ParameterName = "@ttlShipQty",
                        Value = MyUtility.Convert.GetInt(summaryData.Rows[0]["ShipQty"]) + MyUtility.Convert.GetInt(currentMaintain["ShipQty"]),
                    };

                    SqlParameter sp2 = new SqlParameter
                    {
                        ParameterName = "@ttlCTNQty",
                        Value = MyUtility.Convert.GetInt(summaryData.Rows[0]["CTNQty"]) + MyUtility.Convert.GetInt(currentMaintain["CTNQty"]),
                    };

                    SqlParameter sp3 = new SqlParameter
                    {
                        ParameterName = "@ttlNW",
                        Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NW"]) + MyUtility.Convert.GetDouble(currentMaintain["NW"]), 2),
                    };

                    SqlParameter sp4 = new SqlParameter
                    {
                        ParameterName = "@ttlNNW",
                        Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NNW"]) + MyUtility.Convert.GetDouble(currentMaintain["NNW"]), 2),
                    };

                    SqlParameter sp5 = new SqlParameter
                    {
                        ParameterName = "@ttlGW",

                        // ISP20181015 GW抓到小數點後3位
                        Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["GW"]) + MyUtility.Convert.GetDouble(currentMaintain["GW"]), 3),
                    };

                    SqlParameter sp6 = new SqlParameter
                    {
                        ParameterName = "@ttlCBM",

                        // ISP20181015 CBM抓到小數點後4位
                        Value = MyUtility.Convert.GetDouble(summaryData.Rows[0]["CBM"]) + MyUtility.Convert.GetDouble(currentMaintain["CBM"]),
                    };

                    SqlParameter sp7 = new SqlParameter
                    {
                        ParameterName = "@INVNo",
                        Value = currentMaintain["INVNo"].ToString(),
                    };

                    IList<SqlParameter> cmds = new List<SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    cmds.Add(sp4);
                    cmds.Add(sp5);
                    cmds.Add(sp6);
                    cmds.Add(sp7);
                    #endregion

                    result = DBProxy.Current.Execute(null, updateCmd, cmds);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Update Garment Booking fail!\r\n" + result.ToString());
                        return failResult;
                    }
                }
                else
                {
                    DualResult failResult = new DualResult(false, "Select PackingList fail!!\r\n" + result.ToString());
                    return failResult;
                }

                #region 重新計算同Garment Booking 紙箱的材積總重
                DataRow dr;
                decimal bookingVW = 0;
                decimal aPPEstAmtVW = 0;
                string sqlcmd = $@"
select [BookingVW] = isnull(sum(p2.APPBookingVW),0)
,[APPEstAmtVW] = isnull(sum(p2.APPEstAmtVW),0)
from PackingList p1
inner join PackingList_Detail p2 on p1.ID=p2.ID
where p1.INVNo='{currentMaintain["INVNo"]}'
and p1.id !='{currentMaintain["ID"]}'";
                if (MyUtility.Check.Seek(sqlcmd, out dr))
                {
                    bookingVW = MyUtility.Convert.GetDecimal(dr["BookingVW"]);
                    aPPEstAmtVW = MyUtility.Convert.GetDecimal(dr["APPEstAmtVW"]);
                }

                bookingVW += MyUtility.Convert.GetDecimal(detailDatas.Compute("sum(APPBookingVW)", string.Empty));
                aPPEstAmtVW += MyUtility.Convert.GetDecimal(detailDatas.Compute("sum(APPEstAmtVW)", string.Empty));

                string updateSqlCmd = $@"
update GMTBooking
set TotalAPPBookingVW = {bookingVW}
,TotalAPPEstAmtVW = {aPPEstAmtVW}
where ID = '{currentMaintain["INVNo"]}'";

                if (!(result = DBProxy.Current.Execute(string.Empty, updateSqlCmd)))
                {
                    DualResult failResult = new DualResult(false, "Update Garment Booking fail!\r\n" + result.ToString());
                    return failResult;
                }
                #endregion

            }

            return result;
        }

        /// <summary>
        /// P03 Update Others
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        /// <returns>DualResult</returns>
        public static DualResult P03_UpdateOthers(DataRow currentMaintain)
        {
            DataTable orderData;
            string sqlCmd = string.Format("select distinct OrderID from PackingList_Detail WITH (NOLOCK) where ID = '{0}'", currentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out orderData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "select order list fail!\r\n" + result.ToString());
                return failResult;
            }

            result = UpdateOrdersCTN(orderData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Update Orders CTN fail!\r\n" + result.ToString());
                return failResult;
            }

            if (!CreateOrderCTNData(currentMaintain["ID"].ToString()))
            {
                DualResult failResult = new DualResult(false, "Create Order_CTN fail!\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }
        #endregion

        #region Running Change相關

        /// <summary>
        /// Basic B11 Running change 查詢
        /// </summary>
        /// <param name="custCDID">custCDID</param>
        /// <param name="brandID">brandID</param>
        /// <param name="stickerCombinationUkey">stickerCombinationUkey</param>
        /// <param name="stickerCombinationUkey_MixPack">stickerCombinationUkey_MixPack</param>
        /// <param name="stampCombinationUkey">stampCombinationUkey</param>
        /// <returns>List<DataTable></returns>
        public static void CustCD_RunningChange(string custCDID, string brandID, long stickerCombinationUkey, long stickerCombinationUkey_MixPack, long stampCombinationUkey)
        {
            DataTable custpono;
            DataTable custCD_Detail;
            List<DataTable> result = new List<DataTable>();

            string cmd = $@"
SELECT STUFF((
	SELECT ',' + ChangeType
	FROM (
		SELECT [ChangeType]=IIF( EXISTS(
		select 1
		from CustCD
		where id='{custCDID}' AND (ISNULL(StickerCombinationUkey,0) != {stickerCombinationUkey}  or ISNULL(StickerCombinationUkey_MixPack,0) != {stickerCombinationUkey_MixPack})
		),'Sticker','')
		UNION
		SELECT [ChangeType]=IIF( EXISTS(
		select 1
		from CustCD
		where id='{custCDID}' AND ISNULL(StampCombinationUkey ,0) != {stampCombinationUkey}
		),'Stamp','')
	) a
	WHERE ChangeType != ''
	FOR XML PATH('')
),1,1,'')
";
            string strChangeType = MyUtility.GetValue.Lookup(cmd);
            List<string> chageTypes = strChangeType.Split(',').ToList();

            // 根據Category，判斷需要列出哪些CustPONo
            string categorySql = string.Empty;

            foreach (var type in chageTypes)
            {
                if (type == "Sticker")
                {
                    categorySql += " inner join ShippingMarkPic a on a.PackingListID = p.ID";
                }

                if (type == "Stamp")
                {
                    categorySql += " inner join ShippingMarkStamp b on b.PackingListID = p.ID";
                }
            }

            cmd = $@"
select DISTINCT p.*
INTO #PackingList
from PackingList p
LEFT JOIN Pullout pu ON p.PulloutID = pu.ID
where CustCDID='{custCDID}' AND BrandID = '{brandID}'
AND (pu.Status IS NULL OR pu.Status = 'New')

select distinct o.CustPONo
from #PackingList p
{categorySql}
inner join PackingList_Detail pd on p.id = pd.ID
inner join Orders o on o.ID = pd.OrderID

drop table #PackingList

";

            // 準備Basic B11 Sheet的資料
            DBProxy.Current.Select(null, cmd, out custpono);
            result.Add(custpono);

            cmd = $@"
select 
	 BrandID, ID
	,[OldStickerCombinationUkey] = ( SELECT ID FROM ShippingMarkCombination WHERE Ukey=StickerCombinationUkey ) 
	,[OldStickerCombinationUkey_MixPack] = ( SELECT ID FROM ShippingMarkCombination WHERE Ukey=StickerCombinationUkey_MixPack )  
	,[OldStampCombinationUkey] =  ( SELECT ID FROM ShippingMarkCombination WHERE Ukey=StampCombinationUkey )   
	,[NewStickerCombinationUkey] = ( SELECT ID FROM ShippingMarkCombination WHERE Ukey={stickerCombinationUkey} )
	,[NewStickerCombinationUkey_MixPack] =  ( SELECT ID FROM ShippingMarkCombination WHERE Ukey={stickerCombinationUkey_MixPack} )
	,[NewStampCombinationUkey] = ( SELECT ID FROM ShippingMarkCombination WHERE Ukey={stampCombinationUkey} )
from CustCD
where id='{custCDID}'

";
            DBProxy.Current.Select(null, cmd, out custCD_Detail);
            result.Add(custCD_Detail);

            if (result[0].Rows.Count == 0)
            {
                return;
            }

            RunningChange_Excel(result, "Basic B11", chageTypes);
        }

        /// <summary>
        /// Subcon B01 Running change 查詢
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        public static void LocalItem_RunningChange(DataRow currentMaintain)
        {
            string cTNRefno = currentMaintain["Refno"].ToString();
            string cTNUnit = currentMaintain["cTNUnit"].ToString();
            decimal ctnHeight = MyUtility.Convert.GetDecimal(currentMaintain["ctnHeight"]);
            decimal ctnWidth = MyUtility.Convert.GetDecimal(currentMaintain["ctnWidth"]);

            DataTable custpono;
            DataTable b01_Detail;
            List<DataTable> result = new List<DataTable>();

            string cmd = string.Empty;

            // 根據Category，判斷需要列出哪些CustPONo
            string categorySql = string.Empty;

            cmd = $@"SELECT pic.PackingListID
INTO #Sticker_PackingID
FROM ShippingMarkPic pic
INNER JOIN ShippingMarkPic_Detail picd ON pic.Ukey = picd.ShippingMarkPicUkey
INNER JOIN ShippingMarkPicture b03 ON b03.ShippingMarkCombinationUkey = picd.ShippingMarkCombinationUkey 
WHERE b03.Category = 'PIC' AND b03.CTNRefno='{cTNRefno}' 

/*
SELECT Stamp.PackingListID
INTO #StampPackingID
FROM ShippingMarkStamp Stamp
INNER JOIN ShippingMarkStamp_Detail StampD ON Stamp.PackingListID = StampD.PackingListID
INNER JOIN ShippingMarkPicture b03 ON b03.ShippingMarkCombinationUkey = StampD.ShippingMarkCombinationUkey 
WHERE b03.Category = 'HTML' AND b03.CTNRefno='{cTNRefno}' 
*/
SELECT DISTINCT o.CustPONo
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
LEFT JOIN Pullout pu ON pu.ID = p.PulloutID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.ID IN (
	SELECT PackingListID
	FROM #Sticker_PackingID
)
AND (pu.Status IS NULL OR pu.Status='New')
/*UNION
SELECT o.CustPONo
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
LEFT JOIN Pullout pu ON pu.ID = p.PulloutID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.ID IN (
	SELECT PackingListID
	FROM #StampPackingID
)
AND (pu.Status IS NULL OR pu.Status='New')
*/
DROP TABLE #Sticker_PackingID --,#StampPackingID


";

            // 準備Basic B11 Sheet的資料
            DBProxy.Current.Select(null, cmd, out custpono);
            result.Add(custpono);

            cmd = $@"

DECLARE @AfterHeight as numeric(8,4) = {ctnHeight}
DECLARE @AfterWidth as numeric(8,4) = {ctnWidth}
DECLARE @UnitChange as numeric(5,2) = (select IIF(CtnUnit='Inch',25.4,1) from LocalItem where RefNo='{cTNRefno}' )

select 
a.CTNRefno
,(select Ctnheight from LocalItem where RefNo='{cTNRefno}' )
,(select CTNUnit from LocalItem where RefNo='{cTNRefno}' )
,[AfterChangeHeight]=@AfterHeight
,[AfterChangeUnit]='{cTNUnit}'
,a.BrandID
,IIF(a.Category = 'PIC', 'Sticker' , a.Category)
,[ShippingMarkCombination]=(select ID from ShippingMarkCombination where Ukey = a.ShippingMarkCombinationUkey)
,a.CTNRefno
,[IsMixPack]=(select IIF(IsMixPack=1,'Y','') from ShippingMarkCombination where Ukey = a.ShippingMarkCombinationUkey)
,[ShippingMarkType]=st.ID
,b.FromRight
,b.FromBottom  
,[IsHorizontal]=IIF( b.IsHorizontal = 1,'Y','')
,[OriIsOverCtnHt]=IIF( b.IsOverCtnHt = 1,'Y','')
,[OriNotAutomate]=IIF( b.NotAutomate = 1,'Y','')
,[AfterIsOverCtnHt]=
 IIF(
(
CASE WHEN b.IsHorizontal = 1 THEN IIF( ( b.FromBottom + ss.Width ) > (@AfterHeight * @UnitChange) , 1 , 0)
	  WHEN b.IsHorizontal = 0 THEN IIF( ( b.FromBottom + ss.Length ) > (@AfterHeight * @UnitChange), 1 , 0)
	  ELSE 0
 END
) = 1,'Y','')
,[AfterNotAutomate]=
 IIF(
(
 CASE WHEN b.IsHorizontal = 1 THEN IIF( ( b.FromBottom + ss.Width ) >  (@AfterHeight * @UnitChange) , 1 , 0)
	  WHEN b.IsHorizontal = 0 THEN IIF( ( b.FromBottom + ss.Length ) >  (@AfterHeight * @UnitChange) , 1 , 0)
	  ELSE 0
 END
) = 1,'Y','')
from ShippingMarkPicture a
inner join ShippingMarkPicture_Detail b on a.Ukey = b.ShippingMarkPictureUkey
inner join ShippingMarkType st on st.Ukey = b.ShippingMarkTypeUkey
inner join StickerSize ss on ss.ID = b.StickerSizeID
where CTNRefno='{cTNRefno}' 
AND a.Category = 'PIC'

";
            DBProxy.Current.Select(null, cmd, out b01_Detail);
            result.Add(b01_Detail);

            if (result[0].Rows.Count == 0)
            {
                return;
            }

            if (!b01_Detail.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["OriIsOverCtnHt"]) != MyUtility.Convert.GetString(o["AfterIsOverCtnHt"]) ||
            MyUtility.Convert.GetString(o["OriNotAutomate"]) != MyUtility.Convert.GetString(o["AfterNotAutomate"])).Any())
            {
                return;
            }

            RunningChange_Excel(result, "Subcon B01", new List<string>() { "Sticker" });
        }

        /// <summary>
        /// Packing B06  Running change 查詢
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        /// <param name="detailDataRows">detailDataRows</param>
        /// <param name="isDetailChange">isDetailChange</param>
        /// <param name="defaultData">defaultData</param>
        public static void ShippingMarkCombination_RunningChange(DataRow currentMaintain, List<DataRow> detailDataRows, bool isDetailChange, DataTable defaultData, bool IsDetailInserting)
        {

            DataTable[] tables;
            List<DataTable> result = new List<DataTable>();
            string category = currentMaintain["category"].ToString();
            string brandID = currentMaintain["brandID"].ToString();
            int shippingMarkCombinationUkey = MyUtility.Convert.GetInt(currentMaintain["Ukey"]);
            bool isDefault = MyUtility.Convert.GetBool(currentMaintain["isDefault"]);
            bool isMixPack = MyUtility.Convert.GetBool(currentMaintain["isMixPack"]);
            string strIsDefault = isDefault ? "Y" : string.Empty;
            string strIsMixPack = isMixPack ? "Y" : string.Empty;

            string cmd = string.Empty;

            // 根據Category，判斷需要列出哪些CustPONo
            string categorySql = string.Empty;

            #region SQL

            // 1.B06 Default改變的CustPONo
            cmd = $@"

DECLARE @ShippingMarkCombinationUkey as bigint = {shippingMarkCombinationUkey} -- input
DECLARE @IsDefault as bit = {(isDefault ? "1" : "0")} -- input
DECLARE @IsMixPack as bit = {(isMixPack ? "1" : "0")} -- input
DECLARE @BrandID as varchar(15)='{brandID}' -- input
DECLARE @Category as varchar(10)='{category}' -- input
DECLARE @IsDefaultChange as bit = 0


IF NOT EXISTS(
	select 1
	from ShippingMarkCombination
	where BrandID = @BrandID AND Category = @Category AND Ukey = @ShippingMarkCombinationUkey AND IsDefault = @IsDefault AND IsMixPack = @IsMixPack
)
BEGIN
	SET @IsDefaultChange = 1;
END
ELSE
BEGIN
	SET @IsDefaultChange = 0;
END

select TOP 1 * 
INTO #CustCD
from CustCD

select [CustPONo]='12345678911323456798'
INTO #CustPONo

DELETE FROM #CustCD
DELETE FROM #CustPONo

----若預設值有改變，則找出有使用預設值 的CustCD
IF @IsMixPack = 0 AND @Category = 'PIC' AND @IsDefaultChange = 1
BEGIN
	INSERT #CustCD
	select * 
	from CustCD
	where BrandID = @BrandID AND ( StickerCombinationUkey IS NULL OR StickerCombinationUkey = 0)
END

IF @IsMixPack = 1 AND @Category = 'PIC' AND @IsDefaultChange = 1
BEGIN
	INSERT #CustCD
	select * 
	from CustCD
	where BrandID = @BrandID AND (StickerCombinationUkey_MixPack IS NULL OR StickerCombinationUkey_MixPack  =0)
END

IF @Category = 'HTML'  AND @IsDefaultChange = 1
BEGIN
	INSERT #CustCD
	select * 
	from CustCD
	where BrandID = @BrandID AND (StampCombinationUkey IS NULL OR StampCombinationUkey  =0)
END


----根據上述CustCD，找出哪些已經產生P24 或P27資料 的Packing
IF @Category = 'PIC'  AND @IsDefaultChange = 1
BEGIN
	INSERT #CustPONo
	select distinct o.CustPONo
	from ShippingMarkPic a
	inner join PackingList p on a.PackingListID = p.ID
	inner join #CustCD c on p.BrandID = c.BrandID and p.CustCDID = c.ID
	inner join PackingList_Detail pd on p.id = pd.ID
	inner join Orders o on o.ID = pd.OrderID
END

IF @Category = 'HTML'  AND @IsDefaultChange = 1
BEGIN
	INSERT #CustPONo
	select distinct o.CustPONo
	from ShippingMarkStamp a
	inner join PackingList p on a.PackingListID = p.ID
	inner join #CustCD c on p.BrandID = c.BrandID and p.CustCDID = c.ID
	inner join PackingList_Detail pd on p.id = pd.ID
	inner join Orders o on o.ID = pd.OrderID
END


select * from #CustPONo
";

            // 2. 表身改變的CustPONo
            if (isDetailChange)
            {
                cmd += $@"
UNION
select distinct o.CustPONo
from PackingList_Detail pd
inner join Orders o on o.id = pd. OrderID
WHERe pd.ID IN (
	select distinct a.PackingListID
	from ShippingMarkPic a
	inner join ShippingMarkPic_Detail b on a.Ukey = b.ShippingMarkPicUkey
	WHERE b.ShippingMarkCombinationUkey = 6
) AND @Category='PIC'
UNION
select distinct o.CustPONo
from PackingList_Detail pd
inner join Orders o on o.id = pd. OrderID
WHERe pd.ID IN (
	select distinct a.PackingListID
	from ShippingMarkStamp_Detail a
	WHERE a.ShippingMarkCombinationUkey = 9
) AND @Category='HTML'
";
            }

            // 3.B06 Sheet 左邊資料
            cmd += $@"
----B06 Sheet 左邊資料
select [BrandID]='{defaultData.Rows[0]["BrandID"]}'
,[OriPIC] = '{defaultData.Rows[0]["OriPIC"]}'
,[OriPIC_Mix] = '{defaultData.Rows[0]["OriPIC_Mix"]}'
,[OriHTML] = '{defaultData.Rows[0]["OriHTML"]}'
,[NewPIC] = '{defaultData.Rows[0]["NewPIC"]}'
,[NewPIC_Mix] = '{defaultData.Rows[0]["NewPIC_Mix"]}'
,[NewHTML] = '{defaultData.Rows[0]["NewHTML"]}'
WHERE @IsDefaultChange = 1

DROP TABLE #CustCD,#CustPONo

";
            #endregion

            DBProxy.Current.Select(null, cmd, out tables);
            result.Add(tables[0]);
            result.Add(tables[1]);

            string tmpTable = string.Empty;

            #region SQL

            int count = 1;
            foreach (DataRow dr in detailDataRows)
            {
                string tmp = $@"
SELECT [BrandID]='{brandID}'
	,[Category]='{category}'
	,[ShippingMarkCombination]=(select ID from ShippingMarkCombination where Ukey = {shippingMarkCombinationUkey})
	,[IsMixPack]= '{strIsMixPack}'
	,[OriSeq]=NULL
	,[OriShippingMarkType]=''
	,[NewSeq] = {MyUtility.Convert.GetInt(dr["Seq"])}
	,[NewShippingMarkType] = (select ID from ShippingMarkType where Ukey = {MyUtility.Convert.GetInt(dr["ShippingMarkTypeUkey"])})
";

                tmpTable += tmp + Environment.NewLine;

                if (count == 1)
                {
                    tmpTable += "INTO #NewDetail" + Environment.NewLine;
                }

                if (detailDataRows.Count > count)
                {
                    tmpTable += "UNION" + Environment.NewLine;
                }

                count++;
            }

            if (!MyUtility.Check.Empty(tmpTable))
            {
                cmd = $@"
{tmpTable}

SELECT [BrandID]='{brandID}'
	,[Category]='{category}'
	,[ShippingMarkCombination]=(select ID from ShippingMarkCombination where Ukey = {shippingMarkCombinationUkey})
	,[IsMixPack]= '{strIsMixPack}'
	,[OriSeq]=b.Seq
	,[OriShippingMarkType]=(select ID from ShippingMarkType where Ukey = b.ShippingMarkTypeUkey) 
	,[NewSeq] = NULL
	,[NewShippingMarkType] = (select ID from ShippingMarkType where Ukey = 0)
INTO #OriDetail
FROM ShippingMarkCombination a
inner join ShippingMarkCombination_Detail b on a.Ukey = b.ShippingMarkCombinationUkey
where Ukey = {shippingMarkCombinationUkey}

select * from #OriDetail
select * from #NewDetail

DROP TABLE #NewDetail,#OriDetail


";
            }
            else
            {
                cmd = $@"

SELECT [BrandID]='{brandID}'
	,[Category]='{category}'
	,[ShippingMarkCombination]=(select ID from ShippingMarkCombination where Ukey = {shippingMarkCombinationUkey})
	,[IsMixPack]= '{strIsMixPack}'
	,[OriSeq]=b.Seq
	,[OriShippingMarkType]=(select ID from ShippingMarkType where Ukey = b.ShippingMarkTypeUkey) 
	,[NewSeq] = NULL
	,[NewShippingMarkType] = (select ID from ShippingMarkType where Ukey = 0)
INTO #OriDetail
FROM ShippingMarkCombination a
inner join ShippingMarkCombination_Detail b on a.Ukey = b.ShippingMarkCombinationUkey
where Ukey = {shippingMarkCombinationUkey}

----新表身是空的，因此只要取結構即可
select * from #OriDetail
select * from #OriDetail WHERE 1=0

DROP TABLE #OriDetail
";
            }
            #endregion

            DataTable[] b06_Details;
            DBProxy.Current.Select(null, cmd, out b06_Details);

            int rowCount = b06_Details[0].Rows.Count > b06_Details[1].Rows.Count ? b06_Details[0].Rows.Count : b06_Details[1].Rows.Count;
            DataTable final = b06_Details[0].Clone();

            for (int i = 0; i <= rowCount - 1; i++)
            {
                string brindID = string.Empty;
                string categorys = string.Empty;
                string shippingMarkCombination = string.Empty;
                string isMixPacks = string.Empty;
                string oriSeq = string.Empty;
                string oriShippingMarkType = string.Empty;
                string newSeq = string.Empty;
                string newShippingMarkType = string.Empty;

                // 通用欄位
                if (i <= b06_Details[0].Rows.Count - 1)
                {
                    brindID = b06_Details[0].Rows[0]["BrandID"].ToString();
                    categorys = b06_Details[0].Rows[0]["category"].ToString();
                    shippingMarkCombination = b06_Details[0].Rows[0]["shippingMarkCombination"].ToString();
                    isMixPacks = b06_Details[0].Rows[0]["isMixPack"].ToString();
                }
                else
                {
                    brindID = b06_Details[1].Rows[0]["BrandID"].ToString();
                    categorys = b06_Details[1].Rows[0]["category"].ToString();
                    shippingMarkCombination = b06_Details[1].Rows[0]["shippingMarkCombination"].ToString();

                    if (IsDetailInserting)
                    {
                        shippingMarkCombination = currentMaintain["ID"].ToString();
                    }

                    isMixPacks = b06_Details[1].Rows[0]["isMixPack"].ToString();
                }

                // ori
                if (i <= b06_Details[0].Rows.Count - 1)
                {
                    oriSeq = MyUtility.Convert.GetString(b06_Details[0].Rows[i]["oriSeq"]);
                    oriShippingMarkType = MyUtility.Convert.GetString(b06_Details[0].Rows[i]["oriShippingMarkType"]);
                }

                // new
                if (i <= b06_Details[1].Rows.Count - 1)
                {
                    newSeq = MyUtility.Convert.GetString(b06_Details[1].Rows[i]["newSeq"]);
                    newShippingMarkType = MyUtility.Convert.GetString(b06_Details[1].Rows[i]["newShippingMarkType"]);
                }

                DataRow dr = final.NewRow();

                switch (categorys)
                {
                    case "PIC":
                        categorys = "Sticker";
                        break;
                    case "HTML":
                        categorys = "Stamp";
                        break;
                    default:
                        break;
                }

                dr["BrandID"] = brindID;
                dr["Category"] = categorys;
                dr["ShippingMarkCombination"] = shippingMarkCombination;
                dr["IsMixPack"] = isMixPacks;

                if (MyUtility.Convert.GetInt(oriSeq) == 0)
                {
                    dr["oriSeq"] = DBNull.Value;
                }
                else
                {
                    dr["oriSeq"] = MyUtility.Convert.GetInt(oriSeq);
                }

                dr["oriShippingMarkType"] = oriShippingMarkType;
                dr["newSeq"] = MyUtility.Convert.GetInt(newSeq);

                if (MyUtility.Convert.GetInt(newSeq) == 0)
                {
                    dr["newSeq"] = DBNull.Value;
                }
                else
                {
                    dr["newSeq"] = MyUtility.Convert.GetInt(newSeq);
                }

                dr["newShippingMarkType"] = newShippingMarkType;

                final.Rows.Add(dr);
            }

            if (!isDetailChange)
            {
                final.Clear();
            }

            result.Add(final);

            string deleteColumn = string.Empty;

            if (tables[1].Rows.Count > 0 && final.Rows.Count > 0)
            {
                deleteColumn = string.Empty;
            }
            else if (tables[1].Rows.Count > 0 && final.Rows.Count == 0)
            {
                deleteColumn = "Right";
            }
            else if (tables[1].Rows.Count == 0 && final.Rows.Count > 0)
            {
                deleteColumn = "Left";
            }

            switch (category)
            {
                case "PIC":
                    category = "Sticker";
                    break;
                case "HTML":
                    category = "Stamp";
                    break;
                default:
                    break;
            }

            if (result[0].Rows.Count == 0)
            {
                return;
            }

            RunningChange_Excel_B06(result, deleteColumn, new List<string>() { category });
        }

        /// <summary>
        /// Packing B03  Running change 查詢
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        /// <param name="newDetailDataRows">newDetailDataRows</param>
        /// <param name="oriDetailDataRows">oriDetailDataRows</param>
        /// <param name="category">category</param>
        public static void ShippingMarkPicture_RunningChange(DataRow currentMaintain, List<DataRow> newDetailDataRows, List<ShippingMarkPicture_Detail> oriDetailDataRows, string category)
        {
            DataTable custpono;
            List<DataTable> result = new List<DataTable>();

            string shippingMarkPictureUkey = currentMaintain["Ukey"].ToString();

            string cmd = string.Empty;

            // 判斷需要列出哪些CustPONo
            if (category == "PIC")
            {
                cmd = $@"

SELECT  BrandID,Category,ShippingMarkCombinationUkey,CTNRefno
INTO #ShippingMarkPicture
FROM ShippingMarkPicture a
WHERE Ukey =  {shippingMarkPictureUkey} AND a.category = 'PIC'

select distinct o.CustPONo
from ShippingMarkPic_Detail a
inner join PackingList_Detail pd on a.SCICtnNo = pd.SCICtnNo
inner join PackingList p on p.ID = pd.ID
inner join #ShippingMarkPicture c ON c.BrandID = p.BrandID and c.ShippingMarkCombinationUkey = a.ShippingMarkCombinationUkey and c.CTNRefno = pd.RefNo
LEFT JOIN Pullout pu On pu.ID = p.PulloutID
inner join orders o ON o.ID = pd.OrderID
WHERE  (pu.Status = 'New'  or pu.Status IS NULL)

DROP TABLE #ShippingMarkPicture

";
            }

            if (category == "HTML")
            {
                cmd = $@"

SELECT  BrandID,Category,ShippingMarkCombinationUkey,CTNRefno
INTO #ShippingMarkPicture
FROM ShippingMarkPicture a
WHERE Ukey =  {shippingMarkPictureUkey}  AND a.category = 'HTML'

select distinct o.CustPONo
from ShippingMarkStamp_Detail a
inner join PackingList_Detail pd on a.SCICtnNo = pd.SCICtnNo
inner join PackingList p on p.ID = pd.ID
inner join #ShippingMarkPicture c ON c.BrandID = p.BrandID and c.ShippingMarkCombinationUkey = a.ShippingMarkCombinationUkey and c.CTNRefno = pd.RefNo
LEFT JOIN Pullout pu On pu.ID = p.PulloutID
inner join orders o ON o.ID = pd.OrderID
WHERE  (pu.Status = 'New'  or pu.Status IS NULL)

DROP TABLE #ShippingMarkPicture

";

            }

            DBProxy.Current.Select(null, cmd, out custpono);
            result.Add(custpono);

            int rowCount = newDetailDataRows.Count > oriDetailDataRows.Count ? newDetailDataRows.Count : oriDetailDataRows.Count;

            DataTable structure;
            cmd = $@"
select BrandID=''
    ,Category=''
    ,ShippingMarkCombination=''
    ,CTNRefno=''
    ,IsMixPack=''
    ,OriShippingMarkType=''
    ,OriSide=''
    ,OriFromRight=0
    ,OriFromBottom=0
    ,OriStickerSizeID=''
    ,OriIs2Side=''
    ,OriIsHorizontal=''
    ,OriIsOverCtnHt=''
    ,OriNotAutomate=''
    ,NewShippingMarkType=''
    ,NewSide=''
    ,NewFromRight=0
    ,NewFromBottom=0
    ,NewStickerSizeID=''
    ,NewIs2Side=''
    ,NewIsHorizontal=''
    ,NewIsOverCtnHt=''
    ,NewNotAutomate=''
    WHERE 1=0
";
            DBProxy.Current.Select(null, cmd, out structure);
            DataTable final = structure.Clone();

            switch (category)
            {
                case "PIC":
                    category = "Sticker";
                    break;
                case "HTML":
                    category = "Stamp";
                    break;
                default:
                    break;
            }

            for (int i = 0; i <= rowCount - 1; i++)
            {
                string brindID = string.Empty;
                string shippingMarkCombination = string.Empty;
                string cTNRefno = string.Empty;

                string isMixPacks = string.Empty;

                string oriMarkType = string.Empty;
                string oriSide = string.Empty;
                string oriFromRight = string.Empty;
                string oriFromBottom = string.Empty;
                string oriMarkSize = string.Empty;
                string oriIs2Side = string.Empty;
                string oriIsHorizontal = string.Empty;
                string oriIsOverCartonHeight = string.Empty;
                string oriNottoAutomate = string.Empty;

                string newMarkType = string.Empty;
                string newSide = string.Empty;
                string newFromRight = string.Empty;
                string newFromBottom = string.Empty;
                string newMarkSize = string.Empty;
                string newIs2Side = string.Empty;
                string newIsHorizontal = string.Empty;
                string newIsOverCartonHeight = string.Empty;
                string newNottoAutomate = string.Empty;

                // 通用欄位
                brindID = currentMaintain["BrandID"].ToString();

                cTNRefno = currentMaintain["CTNRefno"].ToString();
                shippingMarkCombination = MyUtility.GetValue.Lookup($"select ID from ShippingMarkCombination WHERE Ukey={currentMaintain["ShippingMarkCombinationUkey"].ToString()}");

                bool ismix = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"
SELECT IsMixPack
FROM ShippingMarkCombination WITH(NOLOCK)
WHERE Ukey = '{currentMaintain["ShippingMarkCombinationUkey"]}'

"));

                isMixPacks = ismix ? "Y" : string.Empty;

                // ori
                if (i <= oriDetailDataRows.Count - 1)
                {
                    oriMarkType = MyUtility.GetValue.Lookup($"select ID from ShippingMarkType WHERE Ukey={oriDetailDataRows[i].ShippingMarkTypeUkey}");
                    oriSide = oriDetailDataRows[i].Side;
                    oriFromRight = oriDetailDataRows[i].FromRight.ToString();
                    oriFromBottom = oriDetailDataRows[i].FromBottom.ToString();
                    oriMarkSize = MyUtility.GetValue.Lookup($@"select Size from StickerSize WHERE ID ='{oriDetailDataRows[i].StickerSizeID}' ");
                    oriIs2Side = oriDetailDataRows[i].Is2Side ? "Y" : string.Empty;
                    oriIsHorizontal = oriDetailDataRows[i].IsHorizontal ? "Y" : string.Empty;
                    oriIsOverCartonHeight = oriDetailDataRows[i].IsOverCtnHt ? "Y" : string.Empty;
                    oriNottoAutomate = oriDetailDataRows[i].NotAutomate ? "Y" : string.Empty;
                }

                if (i <= newDetailDataRows.Count - 1)
                {
                    newMarkType = MyUtility.GetValue.Lookup($"select ID from ShippingMarkType WHERE Ukey={newDetailDataRows[i]["ShippingMarkTypeUkey"]}");
                    newSide = MyUtility.Convert.GetString(newDetailDataRows[i]["Side"]);
                    newFromRight = MyUtility.Convert.GetString(newDetailDataRows[i]["FromRight"]);
                    newFromBottom = MyUtility.Convert.GetString(newDetailDataRows[i]["FromBottom"]);
                    newMarkSize = MyUtility.GetValue.Lookup($@"select Size from StickerSize WHERE ID ='{newDetailDataRows[i]["StickerSizeID"]}' ");
                    newIs2Side = MyUtility.Convert.GetBool(newDetailDataRows[i]["Is2Side"]) ? "Y" : string.Empty;
                    newIsHorizontal = MyUtility.Convert.GetBool(newDetailDataRows[i]["IsHorizontal"]) ? "Y" : string.Empty;
                    newIsOverCartonHeight = MyUtility.Convert.GetBool(newDetailDataRows[i]["IsOverCtnHt"]) ? "Y" : string.Empty;
                    newNottoAutomate = MyUtility.Convert.GetBool(newDetailDataRows[i]["NotAutomate"]) ? "Y" : string.Empty;
                }

                DataRow dr = final.NewRow();

                dr["BrandID"] = brindID;
                dr["Category"] = category;
                dr["ShippingMarkCombination"] = shippingMarkCombination;
                dr["CTNRefno"] = cTNRefno;
                dr["IsMixPack"] = isMixPacks;

                dr["OriShippingMarkType"] = oriMarkType;
                dr["OriSide"] = oriSide;
                dr["oriFromRight"] = MyUtility.Convert.GetInt(oriFromRight);
                dr["OriFromBottom"] = MyUtility.Convert.GetInt(oriFromBottom);
                dr["OriStickerSizeID"] = oriMarkSize;
                dr["OriIs2Side"] = oriIs2Side;
                dr["OriIsHorizontal"] = oriIsHorizontal;
                dr["OriIsOverCtnHt"] = oriIsOverCartonHeight;
                dr["OriNotAutomate"] = oriNottoAutomate;

                dr["NewShippingMarkType"] = newMarkType;
                dr["NewSide"] = newSide;
                dr["NewFromRight"] = MyUtility.Convert.GetInt(newFromRight);
                dr["NewFromBottom"] = MyUtility.Convert.GetInt(newFromBottom);
                dr["NewStickerSizeID"] = newMarkSize;
                dr["NewIs2Side"] = newIs2Side;
                dr["NewIsHorizontal"] = newIsHorizontal;
                dr["NewIsOverCtnHt"] = newIsOverCartonHeight;
                dr["NewNotAutomate"] = newNottoAutomate;

                final.Rows.Add(dr);
            }

            result.Add(final);

            if (result[0].Rows.Count == 0)
            {
                return;
            }

            RunningChange_Excel(result, "Packing B03", new List<string>() { category });
        }

        /// <summary>
        /// Packing B04  Running change 查詢
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        /// <param name="oriCurrentMaintain">oriCurrentMaintain</param>
        public static void StickerSize_RunningChange(DataRow currentMaintain, StickerSize oriCurrentMaintain)
        {
            DataTable[] tables;
            List<DataTable> result = new List<DataTable>();

            string stickerSizeID = currentMaintain["ID"].ToString();

            string cmd = string.Empty;

            #region SQL

            // 判斷需要列出哪些CustPONo
            cmd = $@"

select distinct a.Ukey
INTO #Ukeys
from ShippingMarkPicture a
inner join ShippingMarkPicture_Detail b on a.Ukey = b.ShippingMarkPictureUkey
inner join ShippingMarkType c on c.Ukey = b.ShippingMarkTypeUkey
inner join StickerSize s on s.ID = b.StickerSizeID
where s.ID = {stickerSizeID}

SELECT  BrandID,Category,ShippingMarkCombinationUkey,CTNRefno
INTO #ShippingMarkPicture_PIC
FROM ShippingMarkPicture a
WHERE Ukey IN (select ukey from #Ukeys) AND a.Category='PIC'

SELECT  BrandID,Category,ShippingMarkCombinationUkey,CTNRefno
INTO #ShippingMarkPicture_HTML
FROM ShippingMarkPicture a
WHERE Ukey  IN (select ukey from #Ukeys) AND a.Category='HTML'

select distinct o.CustPONo,Category='Sticker'
INTO #CustPONo_PIC
from ShippingMarkPic_Detail a
inner join PackingList_Detail pd on a.SCICtnNo = pd.SCICtnNo
inner join PackingList p on p.ID = pd.ID
inner join #ShippingMarkPicture_PIC c ON c.BrandID = p.BrandID and c.ShippingMarkCombinationUkey = a.ShippingMarkCombinationUkey and c.CTNRefno = pd.RefNo
LEFT JOIN Pullout pu On pu.ID = p.PulloutID
inner join orders o ON o.ID = pd.OrderID
WHERE  (pu.Status = 'New'  or pu.Status IS NULL)

select distinct o.CustPONo,Category='Stamp'
INTO #CustPONo_HTML
from ShippingMarkStamp_Detail a
inner join PackingList_Detail pd on a.SCICtnNo = pd.SCICtnNo
inner join PackingList p on p.ID = pd.ID
inner join #ShippingMarkPicture_HTML c ON c.BrandID = p.BrandID and c.ShippingMarkCombinationUkey = a.ShippingMarkCombinationUkey and c.CTNRefno = pd.RefNo
LEFT JOIN Pullout pu On pu.ID = p.PulloutID
inner join orders o ON o.ID = pd.OrderID
WHERE  (pu.Status = 'New'  or pu.Status IS NULL)

SELECT CustPONo
FROM #CustPONo_PIC
UNION
SELECT CustPONo
FROM #CustPONo_HTML

SELECT Category
FROM #CustPONo_PIC
UNION
SELECT Category
FROM #CustPONo_HTML

DROP TABLE #ShippingMarkPicture_PIC,#ShippingMarkPicture_HTML,#Ukeys,#CustPONo_HTML,#CustPONo_PIC

";
            #endregion

            DBProxy.Current.Select(null, cmd, out tables);
            result.Add(tables[0]);

            List<string> chageTypes = tables[1].AsEnumerable().Select(o => MyUtility.Convert.GetString(o["Category"])).ToList();

            DataTable final = new DataTable();
            final.ColumnsStringAdd("Size");
            final.ColumnsIntAdd("OriWidth");
            final.ColumnsIntAdd("OriLength");
            final.ColumnsIntAdd("NewWidth");
            final.ColumnsIntAdd("NewLength");

            DataRow dr = final.NewRow();
            dr["Size"] = currentMaintain["Size"].ToString();
            dr["OriWidth"] = oriCurrentMaintain.Width;
            dr["OriLength"] = oriCurrentMaintain.Length;
            dr["NewWidth"] = MyUtility.Convert.GetInt(currentMaintain["Width"]);
            dr["NewLength"] = MyUtility.Convert.GetInt(currentMaintain["Length"]);
            final.Rows.Add(dr);
            result.Add(final);

            if (result[0].Rows.Count == 0)
            {
                return;
            }

            RunningChange_Excel(result, "Packing B04", chageTypes);
        }

        /// <summary>
        /// 寄送RunningChange信件
        /// </summary>
        /// <param name="datas">datas</param>
        /// <param name="callFrom">callFrom</param>
        /// <param name="changeType">changeType</param>
        public static void RunningChange_Excel(List<DataTable> datas, string callFrom, List<string> changeType)
        {
            string sqlcmd = $@"SELECT * FROM MailTo WHERE ID='102' AND ToAddress != '' ";
            if (!MyUtility.Check.Seek(sqlcmd))
            {
                return;
            }

            List<string> totalFileList = new List<string>();
            List<string> excelFiles = new List<string>();

            // 附件報表處理
            DataTable poDatatable = datas[0];
            DataTable sheetData = datas[1];

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\RunningChange.xltx"); // 預先開啟excel app

            objApp.Visible = false;
            objApp.DisplayAlerts = false;

            Worksheet worksheet = objApp.ActiveWorkbook.Worksheets[3];

            switch (callFrom)
            {
                case "Basic B11":
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();
                    break;
                case "Subcon B01":
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();

                    break;
                case "Packing B06":
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();
                    break;
                case "Packing B03":
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[3];
                    worksheet.Delete();
                    break;
                case "Packing B04":
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    worksheet = objApp.ActiveWorkbook.Worksheets[2];
                    worksheet.Delete();
                    break;

                default:
                    break;
            }

            Worksheet custPoSheet = (Worksheet)objApp.ActiveWorkbook.Worksheets[1];
            Worksheet dataSheet = (Worksheet)objApp.ActiveWorkbook.Worksheets[2];

            if (poDatatable.Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(poDatatable, null, "RunningChange.xltx", headerRow: 1, excelApp: objApp, wSheet: custPoSheet, showExcel: false, showSaveMsg: false);//將datatable copy to excel
            }

            MyUtility.Excel.CopyToXls(sheetData, null, "RunningChange.xltx", headerRow: 2, excelApp: objApp, wSheet: dataSheet, showExcel: false, showSaveMsg: false);//將datatable copy to excel

            custPoSheet.Activate();

            int dataRowIndex = 0;
            switch (callFrom)
            {
                case "Packing B03":
                    dataRowIndex = sheetData.Rows.Count + 2;
                    dataSheet.GetRanges($"A3:A{dataRowIndex}").Merge();
                    dataSheet.GetRanges($"B3:B{dataRowIndex}").Merge();
                    dataSheet.GetRanges($"C3:C{dataRowIndex}").Merge();
                    dataSheet.GetRanges($"D3:D{dataRowIndex}").Merge();
                    dataSheet.GetRanges($"E3:E{dataRowIndex}").Merge();
                    break;
                case "Subcon B01":
                    dataRowIndex = sheetData.Rows.Count + 2;
                    dataSheet.GetRanges($"A3:A{dataRowIndex}").Merge();
                    dataSheet.GetRanges($"B3:B{dataRowIndex}").Merge();
                    dataSheet.GetRanges($"C3:C{dataRowIndex}").Merge();
                    dataSheet.GetRanges($"D3:D{dataRowIndex}").Merge();
                    dataSheet.GetRanges($"E3:E{dataRowIndex}").Merge();
                    break;
                default:
                    break;
            }

            #region Save Excel
            string excelFile = Sci.Production.Class.MicrosoftFile.GetName("RunningChange");
            excelFiles.Add(excelFile);
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(excelFile);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            #endregion

            Send_RunningChange_Mail(changeType, excelFiles);
        }

        /// <summary>
        /// Packing B06 寄送RunningChange信件
        /// </summary>
        /// <param name="datas">datas</param>
        /// <param name="deleteColumn">keepColumn</param>
        /// <param name="changeType">changeType</param>
        public static void RunningChange_Excel_B06(List<DataTable> datas, string deleteColumn, List<string> changeType)
        {
            string sqlcmd = $@"SELECT * FROM MailTo WHERE ID='102' AND ToAddress != '' ";
            if (!MyUtility.Check.Seek(sqlcmd))
            {
                return;
            }

            List<string> totalFileList = new List<string>();
            List<string> excelFiles = new List<string>();

            // 附件報表處理
            DataTable poDatatable = datas[0];
            DataTable sheetData_Left = datas[1];
            DataTable sheetData = datas[2];

            Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("RunningChange.xltx");

            SaveXltReportCls.XltRptTable xdt_All = new SaveXltReportCls.XltRptTable(sheetData)
            {
                ShowHeader = false,   // 表頭範本有了所以False
                BoAutoFitColumn = true,  // 自動調整欄寬
                BoAddNewRow = false,
            };

            int idx = sheetData.Rows.Count + 2;

            //Dictionary<string, string> merge1 = new Dictionary<string, string>();
            //Dictionary<string, string> merge2 = new Dictionary<string, string>();
            //Dictionary<string, string> merge3 = new Dictionary<string, string>();
            //Dictionary<string, string> merge4 = new Dictionary<string, string>();
            //merge1.Add("BrandID", $"4,{idx}");
            //merge2.Add("Category", $"5,{idx}");
            //merge3.Add("ShippingMarkCombination", $"6,{idx}");
            //merge4.Add("IsMixPack", $"7,{idx}");

            //xdt_All.LisTitleMerge.Add(merge1);
            //xdt_All.LisTitleMerge.Add(merge2);
            //xdt_All.LisTitleMerge.Add(merge3);
            //xdt_All.LisTitleMerge.Add(merge4);

            Microsoft.Office.Interop.Excel.Application excel = xl.ExcelApp;

            excel.Visible = false;
            excel.DisplayAlerts = false;

            Worksheet worksheet = excel.ActiveWorkbook.Worksheets[3];

            worksheet = excel.ActiveWorkbook.Worksheets[2];
            worksheet.Delete();
            worksheet = excel.ActiveWorkbook.Worksheets[2];
            worksheet.Delete();
            worksheet = excel.ActiveWorkbook.Worksheets[3];
            worksheet.Delete();
            worksheet = excel.ActiveWorkbook.Worksheets[3];
            worksheet.Delete();

            Worksheet custPoSheet = (Worksheet)excel.ActiveWorkbook.Worksheets[1];
            Worksheet dataSheet = (Worksheet)excel.ActiveWorkbook.Worksheets[2];

            // 填入資料
            MyUtility.Excel.CopyToXls(poDatatable, null, "RunningChange.xltx", headerRow: 1, excelApp: excel, wSheet: custPoSheet, showExcel: false, showSaveMsg: false);//將datatable copy to excel
            xl.DicDatas.Add("##SheetData", xdt_All);

            int dataRowIndex = sheetData.Rows.Count + 2;

            // 刪除不用的欄位
            if (deleteColumn == "Left")
            {
                dataSheet.GetRanges("A:C").EntireColumn.Delete();

                dataSheet.GetRanges($"A3:A{dataRowIndex}").Merge();
                dataSheet.GetRanges($"B3:B{dataRowIndex}").Merge();
                dataSheet.GetRanges($"C3:C{dataRowIndex}").Merge();
                dataSheet.GetRanges($"D3:D{dataRowIndex}").Merge();
            }
            else if (deleteColumn == "Right")
            {
                dataSheet.GetRanges("D:K").EntireColumn.Delete();

                // 填入資料
                dataSheet.Cells[1, 1] = $"{sheetData_Left.Rows[0]["BrandID"].ToString()} Default Combination";
                dataSheet.Cells[4, 1] = sheetData_Left.Rows[0]["OriPIC"].ToString();
                dataSheet.Cells[4, 2] = sheetData_Left.Rows[0]["OriPIC_Mix"].ToString();
                dataSheet.Cells[4, 3] = sheetData_Left.Rows[0]["OriHTML"].ToString();
                dataSheet.Cells[7, 1] = sheetData_Left.Rows[0]["NewPIC"].ToString();
                dataSheet.Cells[7, 2] = sheetData_Left.Rows[0]["NewPIC_Mix"].ToString();
                dataSheet.Cells[7, 3] = sheetData_Left.Rows[0]["NewHTML"].ToString();
            }
            else
            {
                // 全部保留

                // 填入資料
                dataSheet.Cells[1, 1] = $"{sheetData_Left.Rows[0]["BrandID"].ToString()} Default Combination";
                dataSheet.Cells[4, 1] = sheetData_Left.Rows[0]["OriPIC"].ToString();
                dataSheet.Cells[4, 2] = sheetData_Left.Rows[0]["OriPIC_Mix"].ToString();
                dataSheet.Cells[4, 3] = sheetData_Left.Rows[0]["OriHTML"].ToString();
                dataSheet.Cells[7, 1] = sheetData_Left.Rows[0]["NewPIC"].ToString();
                dataSheet.Cells[7, 2] = sheetData_Left.Rows[0]["NewPIC_Mix"].ToString();
                dataSheet.Cells[7, 3] = sheetData_Left.Rows[0]["NewHTML"].ToString();

                dataSheet.GetRanges($"D3:D{dataRowIndex}").Merge();
                dataSheet.GetRanges($"E3:E{dataRowIndex}").Merge();
                dataSheet.GetRanges($"F3:F{dataRowIndex}").Merge();
                dataSheet.GetRanges($"G3:G{dataRowIndex}").Merge();
            }

            custPoSheet.Activate();

            #region Save Excel
            string excelFile = Sci.Production.Class.MicrosoftFile.GetName("RunningChange");
            excelFiles.Add(excelFile);
            xl.BoOpenFile = false;
            xl.Save(excelFile);
            xl.FinishSave();
            #endregion

            Send_RunningChange_Mail(changeType, excelFiles);
        }

        /// <summary>
        /// 寄送Running Change信件
        /// </summary>
        /// <param name="changeType">Sticker 或 Stamp</param>
        /// <param name="totalFileList">附件路徑檔</param>
        public static void Send_RunningChange_Mail(List<string> changeType, List<string> totalFileList)
        {
            DataTable mailToInfo;
            string sqlcmd = $@"SELECT * FROM MailTo WHERE ID='102' AND ToAddress != '' ";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out mailToInfo);

            string userEmail = MyUtility.GetValue.Lookup($"select EMail from Pass1 where id='{Sci.Env.User.UserID}'");
            string toAddress = mailToInfo.Rows[0]["ToAddress"].ToString();
            string cCAddress = mailToInfo.Rows[0]["CcAddress"].ToString() + ";" + userEmail;
            string subject = mailToInfo.Rows[0]["Subject"].ToString();
            string content = mailToInfo.Rows[0]["Content"].ToString().Replace("{0}", changeType.JoinToString(" & "));
            string mailServer = ConfigurationManager.AppSettings["mailserver_ip"];
            string eMailID = ConfigurationManager.AppSettings["mailserver_account"];
            string eMailPwd = ConfigurationManager.AppSettings["mailserver_password"];
            ushort? smtpPort = MyUtility.Check.Empty(ConfigurationManager.AppSettings["mailserver_port"]) ? null : (ushort?)Convert.ToInt32(ConfigurationManager.AppSettings["mailserver_port"]); //25;
            string sendFrom = "foxpro@sportscity.com.tw";

            Sci.DB.TransferPms transferPMS = new Sci.DB.TransferPms();
            transferPMS.SetSMTP(mailServer, smtpPort, eMailID, eMailPwd);

            if (!MyUtility.Check.Empty(toAddress))
            {
                var mail = new MailMessage();

                mail.IsBodyHtml = true;
                mail.Subject = subject;

                var altView = AlternateView.CreateAlternateViewFromString(
                    content, null, System.Net.Mime.MediaTypeNames.Text.Html);

                mail.AlternateViews.Add(altView);

                foreach (var it in totalFileList)
                {
                    using (FileStream fileStream = new FileStream(
                        it,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite))
                    {
                        try
                        {
                            var ms = new MemoryStream();
                            fileStream.CopyTo(ms);
                            fileStream.Flush();
                            ms.Flush();
                            ms.Position = 0;
                            mail.Attachments.Add(new Attachment(ms, System.IO.Path.GetFileName(it)));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                foreach (var item in toAddress.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        mail.To.Add(item);
                    }
                }

                foreach (var item in cCAddress.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        mail.CC.Add(item);
                    }
                }

                mail.From = new MailAddress(sendFrom);

                SmtpClient smtp = new SmtpClient(mailServer);
                smtp.Credentials = new NetworkCredential(eMailID, eMailPwd); // 寄信帳密

                smtp.Send(mail);
            }
        }

        /// <summary>
        /// Packing B03表身
        /// </summary>
        public class ShippingMarkPicture_Detail
        {
            /// <inheritdoc/>
            public int ShippingMarkPictureUkey { get; set; }

            /// <inheritdoc/>
            public int ShippingMarkTypeUkey { get; set; }

            /// <inheritdoc/>
            public int Seq { get; set; }

            /// <inheritdoc/>
            public bool IsSSCC { get; set; }

            /// <inheritdoc/>
            public string Side { get; set; }

            /// <inheritdoc/>
            public int FromRight { get; set; }

            /// <inheritdoc/>
            public int FromBottom { get; set; }

            /// <inheritdoc/>
            public int StickerSizeID { get; set; }

            /// <inheritdoc/>
            public bool Is2Side { get; set; }

            /// <inheritdoc/>
            public bool IsHorizontal { get; set; }

            /// <inheritdoc/>
            public bool IsOverCtnHt { get; set; }

            /// <inheritdoc/>
            public bool NotAutomate { get; set; }
        }

        /// <summary>
        /// Packing B04表身
        /// </summary>
        public class StickerSize
        {
            /// <inheritdoc/>
            public int ID { get; set; }

            /// <inheritdoc/>
            public string Size { get; set; }

            /// <inheritdoc/>
            public int Width { get; set; }

            /// <inheritdoc/>
            public int Length { get; set; }
        }

        #endregion
    }
}