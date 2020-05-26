using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Sci;
using Ict;
using Ict.Win;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using Sci.Win.UI;
using System.Drawing;

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
	COMMIT TRANSACTION", packingListID, Sci.Env.User.UserID);
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }

            return true;
        }
        #endregion

        #region CheckPulloutComplete
        /// <summary>
        /// CheckPulloutComplete(string,string,string,string,int)
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="orderShipmodeSeq"></param>
        /// <param name="article"></param>
        /// <param name="sizeCode"></param>
        /// <param name="packingQty"></param>
        /// <returns>bool</returns>
        public static bool CheckPulloutComplete(string orderID, string orderShipmodeSeq, string article, string sizeCode, int packingQty)
        {
            DataTable qty;
            DualResult result;
            string sqlCmd = string.Format(@"with PulloutQty
as
(select isnull(sum(pdd.ShipQty),0) as ShipQty
 from Pullout p WITH (NOLOCK) , Pullout_Detail pd WITH (NOLOCK) , Pullout_Detail_Detail pdd WITH (NOLOCK) 
 where p.Status != 'New'
 and p.id = pd.ID
 and pd.OrderID = '{0}'
 and pd.OrderShipmodeSeq = '{1}'
 and p.ID = pdd.ID
 and pd.UKey = pdd.Pullout_DetailUKey
 and pdd.Article = '{2}'
 and pdd.SizeCode = '{3}'
),
InvadjQty
as
(select isnull(sum(iaq.DiffQty),0) as DiffQty
 from InvAdjust ia WITH (NOLOCK) , InvAdjust_Qty iaq WITH (NOLOCK) 
 where ia.OrderID = '{0}'
 and ia.OrderShipmodeSeq = '{1}'
 and ia.ID = iaq.ID
 and iaq.Article = '{2}'
 and iaq.SizeCode = '{3}'
)

select isnull(oqd.Qty,0) as OrderQty,(select ShipQty from PulloutQty)+(select DiffQty from InvadjQty) as ShipQty
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
where oqd.Id = '{0}'
and oqd.Seq = '{1}'
and oqd.Article = '{2}'
and oqd.SizeCode = '{3}'", orderID, orderShipmodeSeq, article, sizeCode);

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
                    return (MyUtility.Convert.GetInt(qty.Rows[0]["OrderQty"]) >= MyUtility.Convert.GetInt(qty.Rows[0]["ShipQty"]) + packingQty);
                }
            }
        }
        #endregion

        #region Recaluate Carton Weight
        /// <summary>
        /// RecaluateCartonWeight(Datatable,DataRow)
        /// </summary>
        /// <param name="PackingListDetaildata"></param>
        /// <param name="PackingListData"></param>
        /// <returns></returns>
        public static void RecaluateCartonWeight(DataTable PackingListDetaildata, DataRow PackingListData)
        {
            if (PackingListDetaildata.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted).Count() == 0)// PackingListDetaildata.Rows.Count <= 0)
            {
                return;
            }
            DataTable weightData, tmpWeightData;
            DataRow[] weight;
            string message = "";
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, "select '' as BrandID, '' as StyleID, '' as SeasonID, Article, SizeCode, NW, NNW from Style_WeightData WITH (NOLOCK) where StyleUkey = ''", out weightData)))
            {
                MyUtility.Msg.WarningBox("Query 'weightData' schema fail!");
                return;
            }

            //檢查是否所有的SizeCode都有存在Style_WeightData中
            foreach (DataRow dr in PackingListDetaildata.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;

                string filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}'", PackingListData["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length == 0)
                {
                    //先將屬於此訂單的Style_WeightData給撈出來
                    string sqlCmd = string.Format(@"select a.ID as StyleID,a.BrandID,a.SeasonID,isnull(b.Article,'') as Article,isnull(b.SizeCode,'') as SizeCode,isnull(b.NW,0) as NW,isnull(b.NNW,0) as NNW
from Style a WITH (NOLOCK) 
left join Style_WeightData b WITH (NOLOCK) on b.StyleUkey = a.Ukey
where a.ID = '{0}' and a.BrandID = '{1}' and a.SeasonID = '{2}'", dr["StyleID"].ToString(), PackingListData["BrandID"].ToString(), dr["SeasonID"].ToString());
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

                filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}' and SizeCode = '{3}'", PackingListData["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString(), dr["SizeCode"].ToString());
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
                if (buttonResult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            double nw = 0, nnw = 0, ctnWeight = 0;
            string localItemWeight;
            DataTable tmpPacklistWeight;
            result = DBProxy.Current.Select(null, "select CTNStartNo, NW, NNW, GW from PackingList_Detail WITH (NOLOCK) where 1=0", out tmpPacklistWeight);
            DataRow tmpPacklistRow;

            string ctnNo = "";
            foreach (DataRow dr in PackingListDetaildata.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    ctnNo = dr["CTNStartNo"].ToString();
                    break;
                }
            }

            // 依CtnStart#排序 來計算混尺碼重量 
            if (PackingListDetaildata.Columns.Contains("tmpKey") == false) {
                PackingListDetaildata.Columns.Add("tmpKey", typeof(decimal));
            }

            int tmpkey = 0;
            foreach (DataRow dr in PackingListDetaildata.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;
                dr["tmpKey"] = tmpkey;
                tmpkey++;
            }
            PackingListDetaildata.DefaultView.Sort = "CTNStartNo,CTNQty desc";
            DataTable dtSort = PackingListDetaildata.DefaultView.ToTable();
            foreach (DataRow dr in dtSort.Rows)
            {
                if(dr.RowState == DataRowState.Deleted)
                    continue;

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

                string filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}' and Article = '{3}' and SizeCode = '{4}'", PackingListData["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length > 0)
                {
                    nw = nw + MyUtility.Convert.GetDouble(weight[0]["NW"]) * MyUtility.Convert.GetInt(dr["ShipQty"]);
                    nnw = nnw + MyUtility.Convert.GetDouble(weight[0]["NNW"]) * MyUtility.Convert.GetInt(dr["ShipQty"]);
                    dr["NWPerPcs"] = MyUtility.Convert.GetDouble(weight[0]["NW"]);
                }
                else
                {
                    filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}' and SizeCode = '{3}'", PackingListData["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString(), dr["SizeCode"].ToString());
                    weight = weightData.Select(filter);
                    if (weight.Length > 0)
                    {
                        nw = nw + MyUtility.Convert.GetDouble(weight[0]["NW"]) * MyUtility.Convert.GetInt(dr["ShipQty"]);
                        nnw = nnw + MyUtility.Convert.GetDouble(weight[0]["NNW"]) * MyUtility.Convert.GetInt(dr["ShipQty"]);
                        dr["NWPerPcs"] = MyUtility.Convert.GetDouble(weight[0]["NW"]);
                    }
                    else
                    {
                        dr["NWPerPcs"] = 0;
                    }
                }
                PackingListDetaildata.Select($"tmpkey = {dr["tmpkey"]}")[0]["NWPerPcs"] = dr["NWPerPcs"];
            }

            if (PackingListDetaildata.Columns.Contains("tmpKey") == true)
            {
                PackingListDetaildata.Columns.Remove("tmpKey");
            }

            //最後一筆資料也要寫入
            tmpPacklistRow = tmpPacklistWeight.NewRow();
            tmpPacklistRow["CTNStartNo"] = ctnNo;
            tmpPacklistRow["NW"] = nw;
            tmpPacklistRow["NNW"] = nnw;
            tmpPacklistRow["GW"] = nw + ctnWeight;
            tmpPacklistWeight.Rows.Add(tmpPacklistRow);

            //將整箱重量回寫回表身Grid中CTNQty> 0的資料中
            foreach (DataRow dr in tmpPacklistWeight.Rows)
            {
                foreach (DataRow dr1 in PackingListDetaildata.Rows)
                {
                    if (dr1.RowState == DataRowState.Deleted)
                        continue;

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
        /// <param name="packingListID"></param>
        /// <returns>bool</returns>
        public static bool CheckPulloutQtyWithOrderQty(string packingListID)
        {
            string sqlCmd = string.Format(@"select OrderID,OrderShipmodeSeq,Article,SizeCode,sum(ShipQty) as ShipQty 
from PackingList_Detail WITH (NOLOCK) 
where ID = '{0}'
group by OrderID,OrderShipmodeSeq,Article,SizeCode", packingListID);
            DataTable queryData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out queryData);
            if (result)
            {
                string errMesg = "";
                foreach (DataRow dr in queryData.Rows)
                {
                    if (!Prgs.CheckPulloutComplete(dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString(), MyUtility.Convert.GetInt(dr["ShipQty"])))
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
        /// <param name="packingListID"></param>
        /// <returns>bool</returns>
        public static bool CheckPackingQtyWithSewingOutput(string packingListID)
        {
            string sqlCmd = string.Format(@"with PackOrderID
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
                            errMesg.Append("SP No.:" + MyUtility.Convert.GetString(dr["OrderID"]) + "Color Way: " + MyUtility.Convert.GetString(dr["Article"]) + ", Size: " + MyUtility.Convert.GetString(dr["SizeCode"]) + ", Qty: " + MyUtility.Convert.GetString(dr["Qty"]) + ", Ship Qty: " + MyUtility.Convert.GetString(dr["PackQty"]) + ", Sewing Qty:" + MyUtility.Convert.GetString(dr["QAQty"]) + ((MyUtility.Check.Empty(dr["DiffQty"]) ? "" : ", Adj Qty:" + MyUtility.Convert.GetString(dr["DiffQty"]))) + "." + (MyUtility.Convert.GetInt(dr["PackQty"]) + MyUtility.Convert.GetInt(dr["DiffQty"]) > MyUtility.Convert.GetInt(dr["Qty"]) ? "   Pullout qty can't exceed order qty," : "") + " Pullout qty can't exceed sewing qty.\r\n");
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
        /// <param name="packingListID"></param>
        /// <returns>string</returns>
        public static string QueryPackingListSQLCmd(string packingListID)
        {
            return string.Format(@"
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
left join Orders o WITH (NOLOCK) on o.ID = a.OrderID
where a.id = '{0}'
order by a.Seq ASC,a.CTNQty DESC", packingListID);
        }
        #endregion

        public static DualResult CheckExistsOrder_QtyShip_Detail(string packingListID = "", string INVNo = "", string ShipPlanID = "", string PulloutID = "", bool showmsg = true)
        {
            string where = string.Empty;
            if (!MyUtility.Check.Empty(packingListID))
                where = $@"and p.id ='{packingListID}'";
            else if (!MyUtility.Check.Empty(INVNo))
                where = $@"and p.INVNo ='{INVNo}'";
            else if (!MyUtility.Check.Empty(ShipPlanID))
                where = $@"and p.ShipPlanID ='{ShipPlanID}'";
            else if (!MyUtility.Check.Empty(PulloutID))
                where = $@"and p.PulloutID ='{PulloutID}'";

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
select distinct msg = concat(oqd.ID, ' (', oqd.Seq, ')')
from #tmpPacking p
left join #tmpOrderShip oqd with(nolock) on oqd.id = p.OrderID and oqd.Seq = p.OrderShipmodeSeq and p.Article = oqd.Article and p.SizeCode = oqd.SizeCode
lEFT JOIN #TPEAdjust t ON t.OrderID= oqd.id AND t.OrderShipmodeSeq = oqd.Seq AND t.Article=oqd.Article AND t.SizeCode=oqd.SizeCode  ----出貨數必須加上台北端財務可能調整出貨數量，因此必須納入考量(若是減少則是負數，因此用加法即可)
where isnull(p.ShipQty,0) + ISNULL(t.DiffQty,0) > isnull(oqd.Qty,0)
";
            string sqlB = sqlCmd + $@"
select distinct msg = concat(oqd.ID, ' (', oqd.Seq, ')')
from #tmpOrderShip oqd
left join #tmpPacking p with(nolock) on oqd.id = p.OrderID and oqd.Seq = p.OrderShipmodeSeq and p.Article = oqd.Article and p.SizeCode = oqd.SizeCode
lEFT JOIN #TPEAdjust t ON t.OrderID= oqd.id AND t.OrderShipmodeSeq = oqd.Seq AND t.Article=oqd.Article AND t.SizeCode=oqd.SizeCode  ----出貨數必須加上台北端財務可能調整出貨數量，因此必須納入考量(若是減少則是負數，因此用加法即可)
where isnull(p.ShipQty,0) + ISNULL(t.DiffQty,0) < isnull(oqd.Qty,0)
";

            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlA, out dt);
            if (!result)
            {
                if (showmsg)
                    MyUtility.Msg.WarningBox(result.ToString());
                return result;
            }

            // 不允許 Confirm
            if (dt.Rows.Count > 0)
            {
                var os = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["msg"])).ToList();
                string msg = @"Ship Qty>Order Qty, please check Q'ty Breakdown by Shipmode (Seq).
" + string.Join("\t", os);

                if (showmsg)
                    MyUtility.Msg.WarningBox(msg);

                return Result.F(msg);
            }

            result = DBProxy.Current.Select(null, sqlB, out dt);
            if (!result)
            {
                if (showmsg)
                    MyUtility.Msg.WarningBox(result.ToString());
                return result;
            }

            // 僅提示允許繼續 Confirm
            if (dt.Rows.Count > 0)
            {
                var os = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["msg"])).ToList();
                string msg = @"Ship Qty<Order Qty, please be sure this is Short Shipment before Save/Confirm the Packing List.
" + string.Join("\t", os);
                
                MyUtility.Msg.WarningBox(msg);
                return Result.True;
            }

            return Result.True;
        }

        public static DualResult CompareOrderQtyPackingQty(string orderID, string plID, int curAddPlQty)
        {
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

            int orderQty =  MyUtility.Convert.GetInt(dtResults[0].Rows[0]["Qty"]);
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
        /// <returns></returns>
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
INNER JOIN Orders o on pd.OrderID = o.ID
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
            string msgSum = "";
            if (dtchk[0].Rows.Count > 0 )
            {
                foreach (DataRow item in dtchk[0].AsEnumerable().ToList())
                {
                    string OrderID = item["OrderID"].ToString();
                    string AllOrderQty = item["TtlOrderQty"].ToString();
                    string AllShipQty = item["TtlShipQty"].ToString();

                    string msg = "";
                    msg += $@"
SP# {OrderID}
Ttl Packing Qty ({AllShipQty}) cannot exceed Order Qty ({AllOrderQty}).
Please check below packing list.
";

                    foreach (DataRow dr in dtchk[1].AsEnumerable().Where(o=>o["OrderID"].ToString()== OrderID).ToList())
                    {
                        string PackingListID = dr["PackingListID"].ToString();
                        string TtlShipQty = dr["TtlShipQty"].ToString();

                        string msg2 = $"PackingList {PackingListID} - {TtlShipQty}" + Environment.NewLine;

                        msg += msg2;
                    }
                    msgSum += msg;
                }
                MyUtility.Msg.WarningBox(msgSum);
                return Result.F(msgSum);
            }

            return Result.True;
        }

        #region Query Packing List Print out Pacging List Report Data
        /// <summary>
        /// QueryPackingListReportData(string,DataTable,DataTable,DataTable,DataTable,DataTable,DataTable,string)
        /// </summary>
        /// <param name="Packing List ID"></param>
        /// <param name="Report Type"></param>
        /// <param name="Empty DataTable"></param>
        /// <param name="Empty DataTable"></param>
        /// <param name="Empty DataTable"></param>

        /// <param name="string"></param>
        /// <returns>DualResult</returns>
        public static DualResult QueryPackingListReportData(string PackingListID, string ReportType, out DataTable printData, out DataTable ctnDim, out DataTable qtyBDown)
        {
            printData = null;
            ctnDim = null;
            qtyBDown = null;
            string sqlCmd = string.Format(@"

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
", PackingListID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(@"Declare @packinglistid VARCHAR(13),
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
order by RefNo", PackingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out ctnDim);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(@"DECLARE @packinglistid VARCHAR(13),
		@orderid VARCHAR(13),
		@sizecode VARCHAR(8),
		@article VARCHAR(8),
		@dataseq VARCHAR(2),
		@sizecount DECIMAL,
		@poid VARCHAR(13),
		@tmpdatalist VARCHAR(160),
		@datalen INT,
		@tmpdata VARCHAR(9),
		@reporttype INT,
		@tmpdata2 VARCHAR(160),
		@qty INT,
		@cbm FLOAT

SET @packinglistid = '{0}'
SET @reporttype = {1} --1:for Adidas/UA/Saucony/NB, 2:for LLL/TNF

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
   DataList VARCHAR(160)
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

select * from @tempQtyBDown", PackingListID, ReportType);
            result = DBProxy.Current.Select(null, sqlCmd, out qtyBDown);
            return result;
        }
        #endregion

        #region Packing List data write in excel -- Packing List Report
        /// <summary>
        /// PackingListToExcel_PackingListReport(string,DataRow,string,DataTable,DataTable,DataTable)
        /// </summary>
        /// <param name="excel file name"></param>
        /// <param name="Packing List Data"></param>
        /// <param name="Report Type"></param>
        /// <param name="Print Data"></param>
        /// <param name="Carton Dimension"></param>
        /// <param name="Qty B'Down"></param>
        /// <returns></returns>
        //public static void PackingListToExcel_PackingListReport(string XltxName, DataRow PLdr, string ReportType, DataTable PrintData, DataTable CtnDim, DataTable QtyBDown)
        //{
        public static void PackingListToExcel_PackingListReport(string XltxName, DataTable PLdt, string ReportType, DataSet PrintData, DataSet CtnDim, DataSet QtyBDown)
        {
            #region Check Multiple
            bool boolMultiple = XltxName.EqualString("\\Packing_P03_PackingListReport_Multiple.xltx") ? true : false;
            #endregion

            string strXltName = Sci.Env.Cfg.XltPathDir + XltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
                return;

#if DEBUG
            excel.Visible = true;
#endif

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            for (int i = 1; i < PLdt.Rows.Count; i++)
            {
                worksheet.Copy(Type.Missing, worksheet);
            }

            int cntWorkSheet = 1;
            foreach (DataRow dr in PLdt.Rows)
            {
                #region Get Sci Delivery
                bool notExistsID = PrintData.Tables[dr["ID"].ToString()] == null;
                if (notExistsID)
                {
                    continue;
                }

                //防止 Sheet Name 重複
                bool existsSheetName = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet existsSheet in excel.Workbooks[1].Worksheets)
                {
                    if (string.Compare(existsSheet.Name, dr["id"].ToString()) == 0)
                    {
                        existsSheetName = true;
                    }
                }
                DataRow[] getMinDelivery = PrintData.Tables[dr["ID"].ToString()].Select("SciDelivery = min(SciDelivery)");
                string strSciDelivery = "";
                if (getMinDelivery.Length > 0)
                    strSciDelivery = string.Format("{0:yyyy/MM/dd}", getMinDelivery[0]["SciDelivery"]);
                #endregion

                #region Column & Row Index
                int bodyRowIndex = (boolMultiple) ? 7 : 9
                    , bodyRowStartIndex = (boolMultiple) ? 7 : 9
                    , bodyRowEndIndex = (boolMultiple) ? 29 : 31
                    , titleMRow = 1, titleMColumn = 1
                    , titlePakingListNoRow = 3, titlePakingListNoColumn = 3
                    , titleSciDeliveryRow = 3, titleSciDeliveryColumn = (boolMultiple) ? 18 : 14
                    , titleSPRow = 5, titleSPColumn = 1
                    , titleStyleRow = 5, titleStyleColumn = 3
                    , titleOrderNoRow = 5, titleOrderNoColumn = 6
                    , titlePoNoRow = 5, titlePoNoColumn = 10
                    , titleInvoiceRow = 5, titleInvoiceColumn = (boolMultiple) ? 1 : 13
                    , titleCustCDRow = (boolMultiple) ? 5 : 7, titleCustCDColumn = (boolMultiple) ? 4 : 1
                    , titleShipModeRow = (boolMultiple) ? 5 : 7, titleShipModeColumn = (boolMultiple) ? 7 : 3
                    , titleInClogRow = (boolMultiple) ? 5 : 7, titleInClogColumn = (boolMultiple) ? 10 : 6
                    , titleDestinationRow = (boolMultiple) ? 5 : 7, titleDestinationColumn = (boolMultiple) ? 14 : 10
                    , titleShipmentDateRow = (boolMultiple) ? 5 : 7, titleShipmentDateColumn = (boolMultiple) ? 18 : 13
                    , bodySPColumn = 1
                    , bodyStyleColumn = 2
                    , bodyOrderNoColumn = 3
                    , bodyPoNoColumn = 4
                    , bodyCtn1Column = (boolMultiple) ? 5 : 1
                    , bodyCtn2Column = (boolMultiple) ? 6 : 2
                    , bodyCtnsColumn = (boolMultiple) ? 7 : 3
                    , bodyColorColumn = (boolMultiple) ? 8 : 4
                    , bodySizeColumn = (boolMultiple) ? 9 : 5
                    , bodyCustSizeColumn = (boolMultiple) ? 10 : 6
                    , bodyPcCtnsColumn = (boolMultiple) ? 11 : 7
                    , bodyQtyColumn = (boolMultiple) ? 12 : 8
                    , bodyNWColumn = (boolMultiple) ? 13 : 9
                    , bodyGWColumn = (boolMultiple) ? 14 : 10
                    , bodyNNWColumn = (boolMultiple) ? 15 : 11
                    , bodyNWPcsColumn = (boolMultiple) ? 16 : 12
                    , bodyTTLNWColumn = (boolMultiple) ? 17 : 13
                    , bodyTTLGWColumn = (boolMultiple) ? 18 : 14
                    , bodyTTLNNWColumn = (boolMultiple) ? 19 : 15
                    , SippingMarkBDColumn = (boolMultiple) ? 11 : 8
                    ;
                #endregion
                #region workRange
                // strWorkRange 取 A8 & A10 是因為框線設定
                string strWorkRange = (boolMultiple) ? "A8:A8" : "A10:A10"
                        , strColumnsRange = (boolMultiple) ? "A{0}:S{0}" : "A{0}:O{0}";
                #endregion

                worksheet = excel.ActiveWorkbook.Worksheets[cntWorkSheet];
                worksheet.Select();
                if (existsSheetName)
                {
                    continue;
                }
                worksheet.Name = dr["id"].ToString();
                string NameEN = MyUtility.GetValue.Lookup("NameEN", Sci.Env.User.Factory, "Factory ", "id");
                cntWorkSheet++;
                #region Set Title
                // 單筆SP 限定
                if (!boolMultiple)
                {
                    worksheet.Cells[titleSPRow, titleSPColumn] = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["OrderID"]);
                    worksheet.Cells[titleStyleRow, titleStyleColumn] = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["StyleID"]);
                    worksheet.Cells[titleOrderNoRow, titleOrderNoColumn] = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["Customize1"]);
                    worksheet.Cells[titlePoNoRow, titlePoNoColumn] = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["CustPONo"]);
                }
                worksheet.Cells[titleMRow, titleMColumn] = NameEN;
                worksheet.Cells[titlePakingListNoRow, titlePakingListNoColumn] = MyUtility.Convert.GetString(dr["ID"]);
                worksheet.Cells[titleSciDeliveryRow, titleSciDeliveryColumn] = strSciDelivery;
                worksheet.Cells[titleInvoiceRow, titleInvoiceColumn] = MyUtility.Convert.GetString(dr["INVNo"]);
                worksheet.Cells[titleCustCDRow, titleCustCDColumn] = MyUtility.Convert.GetString(dr["CustCDID"]);
                worksheet.Cells[titleShipModeRow, titleShipModeColumn] = MyUtility.Convert.GetString(dr["ShipModeID"]);
                worksheet.Cells[titleInClogRow, titleInClogColumn] = (MyUtility.Check.Empty(MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["InClogQty"])) ? "0" : MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["InClogQty"])) + " / " + MyUtility.Convert.GetString(dr["CTNQty"]) + "   ( " + MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(PrintData.Tables[dr["ID"].ToString()].Rows[0]["InClogQty"]) / MyUtility.Convert.GetDecimal(dr["CTNQty"]), 4) * 100) + "% )";
                worksheet.Cells[titleDestinationRow, titleDestinationColumn] = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["Alias"]);
                worksheet.Cells[titleShipmentDateRow, titleShipmentDateColumn] = MyUtility.Check.Empty(PrintData.Tables[dr["ID"].ToString()].Rows[0]["EstPulloutDate"]) ? "  /  /    " : Convert.ToDateTime(PrintData.Tables[dr["ID"].ToString()].Rows[0]["EstPulloutDate"]).ToString("d");
                #endregion

                #region Set Body
                //當要列印的筆數超過22筆，就要插入Row，因為範本只留22筆記錄的空間
                if (PrintData.Tables[dr["ID"].ToString()].Rows.Count > 22)
                {
                    for (int i = 1; i <= PrintData.Tables[dr["ID"].ToString()].Rows.Count - 22; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(strWorkRange, Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);
                    }
                }

                string ctnStartNo = "XXXXXX";
                foreach (DataRow drBody in PrintData.Tables[dr["ID"].ToString()].Rows)
                {
                    //因為有箱號不連續的問題，所以直接時用來源資料組好的數量，不用箱號箱減
                    //int ctns = MyUtility.Convert.GetInt(dr["CTNEndNo"]) - MyUtility.Convert.GetInt(dr["CTNStartNo"]) + 1;
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
                    worksheet.Cells[bodyRowIndex, bodyNWPcsColumn] = MyUtility.Check.Empty(drBody["NWPerPcs"]) ? "" : MyUtility.Convert.GetString(drBody["NWPerPcs"]);

                    ctnStartNo = MyUtility.Convert.GetString(drBody["CTNStartNo"]);
                    bodyRowIndex++;
                }

                worksheet.Range[String.Format(strColumnsRange, bodyRowIndex)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format(strColumnsRange, bodyRowIndex)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;
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

                foreach (DataRow drCtn in CtnDim.Tables[dr["ID"].ToString()].Rows)
                {
                    ctnDimension.Append(string.Format("{0} - {1} - {2} {3}, (CTN#:{4}){5}  \r\n"
                        , MyUtility.Convert.GetString(drCtn["RefNo"])
                        , MyUtility.Convert.GetString(drCtn["Description"])
                        , MyUtility.Convert.GetString(drCtn["Dimension"])
                        , MyUtility.Convert.GetString(drCtn["CtnUnit"])
                        , MyUtility.Check.Empty(drCtn["Ctn"]) ? "" : MyUtility.Convert.GetString(drCtn["Ctn"]).Substring(0, MyUtility.Convert.GetString(drCtn["Ctn"]).Length - 1)
                        , ReportType == "1" ? ", ttlCBM:" + MyUtility.Convert.GetString(drCtn["TtlCBM"]) : ""));
                }

                string cds = ctnDimension.Length > 0 ? ctnDimension.ToString().Substring(0, ctnDimension.ToString().Length - 2) : "";
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
                    Microsoft.Office.Interop.Excel.Range rangeRowCD = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[bodyRowIndex, System.Type.Missing];
                    rangeRowCD.RowHeight = 19.5 * (i + 1);
                    Marshal.ReleaseComObject(rangeRowCD);

                }
                worksheet.Cells[bodyRowIndex, 3] = ctnDimension.Length > 0 ? ctnDimension.ToString() : "";
                #endregion

                //Remarks
                bodyRowIndex++;
                worksheet.Cells[bodyRowIndex, 3] = MyUtility.Convert.GetString(dr["Remark"]);

                #region Color/Size Breakdown

                bodyRowIndex = bodyRowIndex + 2;
                if (QtyBDown.Tables[dr["ID"].ToString()].Rows.Count > 5)
                {
                    Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(bodyRowIndex))).EntireRow;
                    for (int i = 1; i <= QtyBDown.Tables[dr["ID"].ToString()].Rows.Count - 5; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}", MyUtility.Convert.GetString(bodyRowIndex + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                        Marshal.ReleaseComObject(rngToInsert);
                    }
                    Marshal.ReleaseComObject(rngToCopy);
                }
                foreach (DataRow drQtyBDown in QtyBDown.Tables[dr["ID"].ToString()].Rows)
                {
                    worksheet.Cells[bodyRowIndex, 1] = MyUtility.Convert.GetString(drQtyBDown["DataList"]);
                    bodyRowIndex++;
                }

                #endregion

                #region Shipment mark
                //Shipment mark
                bodyRowIndex = bodyRowIndex + 3;
                worksheet.Cells[bodyRowIndex, 1] = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["MarkFront"]);

                worksheet.Cells[bodyRowIndex, SippingMarkBDColumn] = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["MarkBack"]);

                string[] marks = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["MarkFront"]).Split('\r');
                string[] marks2 = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["MarkBack"]).Split('\r');
                int m = marks.Length + formarks(marks);
                int m2 = marks2.Length + formarks(marks2);
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

                worksheet.Cells[bodyRowIndex, 1] = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["MarkLeft"]);
                worksheet.Cells[bodyRowIndex, SippingMarkBDColumn] = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["MarkRight"]);

                string[] marks3 = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["MarkLeft"]).Split('\r');
                string[] marks4 = MyUtility.Convert.GetString(PrintData.Tables[dr["ID"].ToString()].Rows[0]["MarkRight"]).Split('\r');
                int m3 = marks3.Length + formarks(marks3);
                int m4 = marks4.Length + formarks(marks4);
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
            
            //MyUtility.Msg.WaitClear();
            excel.Columns.AutoFit();
            excel.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(boolMultiple ? "Packing_P03_PackingListReport_Multiple" : "Packing_P03_PackingGuideReport");
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

        private static int formarks(string[] marks)
        {
            int b = 0;
            int L = 63;
            foreach (string item in marks)
            {
                if (item.Length > L)
                {
                    int h = item.Length / L;
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
        /// <param name="Packing List ID"></param>
        /// <param name="Empty DataTable"></param>
        /// <param name="Empty DataTable"></param>
        /// <param name="Empty DataTable"></param>
        /// <param name="Empty DataTable"></param>
        /// <param name="Empty DataTable"></param>
        /// <param name="Empty DataTable"></param>
        /// <param name="string"></param>
        /// <returns>DualResult</returns>
        public static DualResult QueryPackingGuideReportData(string PackingListID, out DataTable printData, out DataTable ctnDim, out DataTable qtyCtn, out DataTable articleSizeTtlShipQty, out DataTable printGroupData, out DataTable clipData, out string specialInstruction)
        {
            printData = null;
            ctnDim = null;
            qtyCtn = null;
            articleSizeTtlShipQty = null;
            printGroupData = null;
            clipData = null;
            specialInstruction = MyUtility.GetValue.Lookup (string.Format (@"
select top 1 Packing = isnull(o.Packing, '') 
from PackingList_Detail pd WITH (NOLOCK) 
     , Orders o WITH (NOLOCK) 
where   pd.ID = '{0}' 
        and pd.OrderID = o.ID", PackingListID));

            string sqlCmd = string.Format(@"
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
order by pd.Seq", PackingListID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(@"
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
order by RefNo", PackingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out ctnDim);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(@"
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
where pd.ID = '{0}'", PackingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out qtyCtn);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(@"
select  Article
        , SizeCode
        , TtlShipQty = sum(ShipQty) 
from PackingList_Detail WITH (NOLOCK) 
where ID = '{0}' 
group by Article, SizeCode", PackingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out articleSizeTtlShipQty);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(@"
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
order by a.MinSeq", PackingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out printGroupData);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(@"
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
        and UPPER(c.SourceFile) like '%.JPG'", PackingListID);
            result = DBProxy.Current.Select(null, sqlCmd, out clipData);
            return result;
        }
        #endregion

        #region Packing List data write in excel -- Packing Guide Report
        /// <summary>
        /// PackingListToExcel_PackingGuideReport(string,DataTable,DataTable,DataTable,DataTable,DataTable,DataTable,DataRow,int,string)
        /// </summary>
        /// <param name="excel file name"></param>
        /// <param name="Print Data"></param>
        /// <param name="Carton Dimension"></param>
        /// <param name="Qty/per CTN"></param>
        /// <param name="Total Ship Qty group by Article/Size"></param>
        /// <param name="Print Data group by Article/Size"></param>
        /// <param name="CustCD Clip .JPG file></param>
        /// <param name="Packing List Data"></param>
        /// <param name="Order Qty"></param>
        /// <param name="Special Instruction"></param>
        /// <returns></returns>
        public static void PackingListToExcel_PackingGuideReport(string XltxName, DataTable PrintData, DataTable CtnDim, DataTable QtyCtn, DataTable ArticleSizeTtlShipQty, DataTable PrintGroupData, DataTable ClipData, DataRow PacklistData, int OrderQty, string SpecialInstruction, bool visRow3)
        {            
            string strXltName = Sci.Env.Cfg.XltPathDir + XltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            //MyUtility.Msg.WaitWindows("Starting to excel...");
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            string NameEN = MyUtility.GetValue.Lookup("NameEN", Sci.Env.User.Factory, "Factory ", "id");
            worksheet.Cells[1, 1] = NameEN;
            worksheet.Cells[3, 3] = MyUtility.Convert.GetString(PacklistData["ID"]);
            worksheet.Cells[3, 20] = DateTime.Today;
            worksheet.Cells[5, 1] = MyUtility.Convert.GetString(PrintData.Rows[0]["Factory"]);
            worksheet.Cells[5, 2] = MyUtility.Convert.GetString(PrintData.Rows[0]["OrderID"]);
            worksheet.Cells[5, 4] = PrintData.Rows[0]["BuyerDelivery"];
            worksheet.Cells[5, 6] = MyUtility.Convert.GetString(PrintData.Rows[0]["StyleID"]);
            worksheet.Cells[5, 8] = MyUtility.Convert.GetString(PrintData.Rows[0]["Customize1"]);
            worksheet.Cells[5, 10] = MyUtility.Convert.GetString(PrintData.Rows[0]["CustPONo"]);
            worksheet.Cells[5, 12] = MyUtility.Convert.GetInt(PrintData.Rows[0]["CTNQty"]);
            worksheet.Cells[5, 14] = MyUtility.Convert.GetString(PrintData.Rows[0]["CustCD"]);
            worksheet.Cells[5, 16] = MyUtility.Convert.GetString(PrintData.Rows[0]["DestAlias"]);
            worksheet.Cells[5, 18] = OrderQty;
            worksheet.Cells[5, 20] = MyUtility.Convert.GetInt(PacklistData["ShipQty"]);
            worksheet.Cells[5, 21] = "=R5-T5";

            int groupRec = PrintGroupData.Rows.Count, excelRow = 6, printRec = 1, printCtnCount = 0;
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
                
                if (MyUtility.Check.Empty(PrintGroupData.Rows[i]["CTNQty"]))
                {
                    excelRow++;
                    if (excelRow >= chk1) //若資料會超過262筆，就先插入一筆Record，最後再把多的資料刪除
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(excelRow)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);
                    }
                    if (article != MyUtility.Convert.GetString(PrintGroupData.Rows[i]["Article"]))
                    {
                        article = MyUtility.Convert.GetString(PrintGroupData.Rows[i]["Article"]);
                        worksheet.Cells[excelRow, 1] = MyUtility.Convert.GetString(PrintGroupData.Rows[i]["Article"]) + ' ' + MyUtility.Convert.GetString(PrintGroupData.Rows[i]["Color"]);
                    }
                    worksheet.Cells[excelRow, 2] = MyUtility.Convert.GetString(PrintGroupData.Rows[i]["SizeCode"]);
                    worksheet.Cells[excelRow, 3] = MyUtility.Convert.GetString(PrintGroupData.Rows[i]["SizeSpec"]);
                    worksheet.Cells[excelRow, 4] = MyUtility.Convert.GetString(PrintGroupData.Rows[i]["QtyPerCTN"]);
                    if (articleSize.ToString().IndexOf(string.Format("{0}{1}", MyUtility.Convert.GetString(PrintGroupData.Rows[i]["Article"]), MyUtility.Convert.GetString(PrintGroupData.Rows[i]["SizeCode"]))) < 0)
                    {
                        worksheet.Cells[excelRow, 20] = MyUtility.Convert.GetString(PrintGroupData.Rows[i]["OQty"]);
                        worksheet.Cells[excelRow, 21] = MyUtility.Convert.GetString(PrintGroupData.Rows[i]["TtlShipQty"]);
                        articleSize.Append(string.Format("{0}{1},", MyUtility.Convert.GetString(PrintGroupData.Rows[i]["Article"]), MyUtility.Convert.GetString(PrintGroupData.Rows[i]["SizeCode"])));
                    }
                }
                else
                {
                    printRec = 1;

                    DataRow[] printList = PrintData.Select(string.Format("Article = '{0}' and SizeCode = '{1}' and Seq > '{2}' and QtyPerCTN = {3}", MyUtility.Convert.GetString(PrintGroupData.Rows[i]["Article"]), MyUtility.Convert.GetString(PrintGroupData.Rows[i]["SizeCode"]), seq, MyUtility.Convert.GetString(PrintGroupData.Rows[i]["QtyPerCTN"])), "Seq");
                    foreach (DataRow dr in printList)
                    {
                        if (printRec == 1)
                        {
                            excelRow++;
                            if (excelRow >= chk1) //若資料會超過262筆，就先插入一筆Record，最後再把多的資料刪除
                            {
                                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(excelRow)), Type.Missing).EntireRow;
                                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                                Marshal.ReleaseComObject(rngToInsert);
                            }
                            if (article != MyUtility.Convert.GetString(dr["Article"])||size != MyUtility.Convert.GetString(dr["SizeCode"]) || qtyPerCTN != MyUtility.Convert.GetInt(dr["QtyPerCTN"]))
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

            //刪除多餘的Row
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
            
            //Carton Dimension:
            excelRow++;
            StringBuilder ctnDimension = new StringBuilder();
            foreach (DataRow dr in CtnDim.Rows)
            {
                ctnDimension.Append(string.Format("{0} / {1} / {2} {3}, {4}  \r\n", MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["Description"]), MyUtility.Convert.GetString(dr["Dimension"]), MyUtility.Convert.GetString(dr["CtnUnit"]), MyUtility.Convert.GetString(dr["CTN"])));
            }

            foreach (DataRow dr in QtyCtn.Rows)
            {
                if (!MyUtility.Check.Empty(dr["Article"]))
                {
                    ctnDimension.Append(string.Format("{0} -> {1} / {2}, ", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"])));
                }
            }
            string cds = ctnDimension.Length > 0 ? ctnDimension.ToString().Substring(0, ctnDimension.ToString().Length - 2) : "";
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
                Microsoft.Office.Interop.Excel.Range rangeRowCD = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[excelRow, System.Type.Missing];
                rangeRowCD.RowHeight = 19.5 * (i + 1);
                Marshal.ReleaseComObject(rangeRowCD);
            }
            worksheet.Cells[excelRow, 3] = ctnDimension.Length > 0 ? ctnDimension.ToString().Substring(0, ctnDimension.ToString().Length - 2) : "";
            
            //填Remarks
            excelRow = excelRow + 2;
            worksheet.Cells[excelRow, 2] = MyUtility.Convert.GetString(PacklistData["Remark"]);
            //填Special Instruction
            //先取得Special Instruction總共有幾行
            int dataRow = 0;

            string tmp = MyUtility.Convert.GetString(SpecialInstruction);

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
            #region 舊寫法SpecialInstruction 有幾行資料,就多加幾行空白
            /*
             *原本寫法是SpecialInstruction 有幾行資料,就多加幾行空白
             */
            //for (int i = 1; ; i++)
            //{
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

            //        break;
            //    }
            //}
            //
            #endregion

            //調整寫法, 只需要多加兩行空白即可
            dataRow = 2 + ctmpc;
            excelRow++;
           
            if (dataRow > 2)
            {
                for (int i = 3; i < dataRow; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(excelRow + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    rngToInsert.RowHeight = 19.5 ;
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }
            //因為SpecialInstruction中有參雜=等特殊字元，在開頭加單引號強迫轉為字串避免<Exception from HRESULT: 0x800A03EC>問題發生
            worksheet.Cells[excelRow, 3] = "'" + SpecialInstruction;

            excelRow = excelRow + (dataRow > 2 ? dataRow - 1 : 2);
            
            //貼圖
            int picCount = 0;
            excelRow = excelRow + 5;
            foreach (DataRow dr in ClipData.Rows)
            {
                if (picCount >= 4)
                {
                    break;
                }
                picCount++;
                string excelRng = picCount % 2 == 1 ? (picCount > 2 ? string.Format("A{0}", MyUtility.Convert.GetString(excelRow + 30)) : string.Format("A{0}", MyUtility.Convert.GetString(excelRow))) : (picCount > 2 ? string.Format("K{0}", MyUtility.Convert.GetString(excelRow + 30)) : string.Format("K{0}", MyUtility.Convert.GetString(excelRow)));
                Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(excelRng, Type.Missing);
                rngToInsert.Select();
                float PicLeft, PicTop;
                PicLeft = Convert.ToSingle(rngToInsert.Left);
                PicTop = Convert.ToSingle(rngToInsert.Top);
                string targetFile = Env.Cfg.ClipDir + "\\" + MyUtility.Convert.GetString(dr["Year"]) + Convert.ToString(dr["Month"]).PadLeft(2, '0') + "\\" + MyUtility.Convert.GetString(dr["TableName"]) + MyUtility.Convert.GetString(dr["PKey"]) + MyUtility.Convert.GetString(dr["SourceFile"]).Substring(MyUtility.Convert.GetString(dr["SourceFile"]).LastIndexOf('.'));
                worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, PicLeft, PicTop, 450, 400);
                Marshal.ReleaseComObject(rngToInsert);
            }

            if (!visRow3)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[3, Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Packing_P03_PackingGuideReport");
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
        /// <param name="Packing List ID"></param>
        /// <param name="CartonNo"></param>
        /// <param name="CartonNo"></param>
        /// <param name="Empty DataTable"></param>
        /// <returns>DualResult</returns>
        public static DualResult PackingBarcodePrint(string packingListID, string ctnStartNo, string ctnEndNo, out DataTable printBarcodeData)
        {
            printBarcodeData = null;
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
        public static void PackingListToExcel_PackingMDFormReport(string XltxName, DataRow masterdata, DataTable[] PrintData)
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + XltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            decimal pageCT = 0;

            #region 先準備複製幾頁
            foreach (DataTable DT in PrintData)
            {
                decimal pcount = Math.Ceiling(((decimal)DT.Rows.Count / 50));
                pageCT += pcount;
            }
            if (pageCT > 1)
            {
                for (int i = 0; i < pageCT; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[1]);
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 1]);
                        worksheet1.Copy(worksheetn);
                    }
                }
            }
            #endregion
            string Alias = MyUtility.GetValue.Lookup($@"select Alias from Country where id = '{masterdata["Dest"]}'");
            int page = 1;
            foreach (DataTable DT in PrintData) // by custpono
            {
                decimal pcount = Math.Ceiling(((decimal)DT.Rows.Count /50));
                for (int i = 0; i < pcount; i++)
                {
                    #region sqlcommand
                    string orderids = $@"
select OrderID = stuff((
	select concat('/',a.OrderID)
	from (
		select distinct pd.OrderID
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
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
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
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
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
	)a
	for xml path('')
),1,1,'')
";
                    string BuyerDelivery = $@"
select BuyerDelivery = stuff((
	select concat('/',FORMAT(a.BuyerDelivery,'MM/dd/yyyy'))
	from (
		select distinct o.BuyerDelivery
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
	)a
	order by BuyerDelivery
	for xml path('')
),1,1,'')
";
                    #endregion
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[page];
                    worksheet.Cells[1, 3] = MyUtility.Convert.GetString(DT.Rows[0]["CustPONo"]) +" of "+ MyUtility.Convert.GetString(masterdata["CTNQty"]);
                    worksheet.Cells[5, 2] = MyUtility.GetValue.Lookup(orderids);
                    worksheet.Cells[6, 2] = MyUtility.GetValue.Lookup(styles);
                    worksheet.Cells[7, 2] = DT.Rows[0]["CustPONo"];
                    worksheet.Cells[8, 2] = MyUtility.GetValue.Lookup(colors);
                    worksheet.Cells[5, 6] = masterdata["CTNQty"];
                    worksheet.Cells[6, 6] = MyUtility.Convert.GetInt(masterdata["ShipQty"]).ToString("#,#.#")+" pcs.";
                    worksheet.Cells[8, 6] = MyUtility.GetValue.Lookup(BuyerDelivery);
                    worksheet.Cells[5, 8] = Alias;
                    for (int j = i * 50, k = 10; j < DT.Rows.Count && j < (i + 1) * 50; j++, k++)
                    {
                        worksheet.Cells[k, 2] = DT.Rows[j]["CTNStartNo"];
                        worksheet.Cells[k, 3] = DT.Rows[j]["SizeCode"];
                        worksheet.Cells[k, 4] = DT.Rows[j]["ShipQty"];
                    }

                    page++;
                }
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Packing_P03_PackingMDFormReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            //Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion 
        }
        #endregion

        #region MD Form Report
        public static DualResult QueryPackingMDform(string PackingListID, out DataTable[] printData)
        {
            string orderidsql = $@"select o.CustPONo from PackingList_Detail pd left join orders o on o.id = pd.OrderID where pd.id = '{PackingListID}' group by CustPONo order by CustPONo";
            DataTable CustPONoDT;
            DualResult result;
            result = DBProxy.Current.Select(null, orderidsql, out CustPONoDT);
            if (!result)
            {
                printData = null;
                return result;
            }

            StringBuilder sqlCmd = new StringBuilder();
            for (int i = 0; i < CustPONoDT.Rows.Count; i++)
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
where pd.id = '{PackingListID}' and o.CustPONo = '{CustPONoDT.Rows[i]["CustPONo"]}'
group by CTNStartNo,a.SizeCode,b.ShipQty,o.CustPONo
order by min(pd.seq)
");

            }
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            return result;
        }
        #endregion

        #region Packing Carton Weighing Report
        public static void PackingListToExcel_PackingCartonWeighingReport(string XltxName, DataRow masterdata, DataTable[] PrintData)
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + XltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            decimal pageCT = 0;

            #region 先準備複製幾頁
            foreach (DataTable DT in PrintData)
            {
                decimal pcount = Math.Ceiling(((decimal)DT.Rows.Count / 100));
                pageCT += pcount;
            }
            if (pageCT > 1)
            {
                for (int i = 0; i < pageCT; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[1]);
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 1]);
                        worksheet1.Copy(worksheetn);
                    }
                }
            }
            #endregion
            string Alias = MyUtility.GetValue.Lookup($@"select Alias from Country where id = '{masterdata["Dest"]}'");
            int page = 1;
            foreach (DataTable DT in PrintData) // by custpono
            {
                decimal pcount = Math.Ceiling(((decimal)DT.Rows.Count / 100));
                for (int i = 0; i < pcount; i++)
                {
                    #region sqlcommand
                    string orderids = $@"
select OrderID = stuff((
	select concat('/',a.OrderID)
	from (
		select distinct pd.OrderID
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
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
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
	)a
	order by StyleID
	for xml path('')
),1,1,'')";
                    string SeasonIDs = $@"
select SeasonID = stuff((
	select concat('/',a.SeasonID)
	from (
		select distinct o.SeasonID
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
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
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
	)a
	for xml path('')
),1,1,'')
";
                    string BuyerDelivery = $@"
select BuyerDelivery = stuff((
	select concat('/',FORMAT(a.BuyerDelivery,'MM/dd/yyyy'))
	from (
		select distinct o.BuyerDelivery
		from PackingList_Detail pd inner join orders o on o.id = pd.OrderID
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
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
		where pd.id = '{masterdata["id"]}' and o.CustPONo = '{DT.Rows[0]["CustPONo"]}'
	)a
	order by Kit
	for xml path('')
),1,1,'') ";
                    #endregion
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[page];
                    worksheet.Cells[1, 2] = MyUtility.Convert.GetString(DT.Rows[0]["CustPONo"]) + " box of " + MyUtility.Convert.GetString(masterdata["CTNQty"]);
                    worksheet.Cells[3, 2] = masterdata["FactoryID"];
                    worksheet.Cells[4, 2] = DT.Rows[0]["CustPONo"];
                    worksheet.Cells[5, 2] = MyUtility.GetValue.Lookup(orderids);
                    worksheet.Cells[6, 2] = MyUtility.GetValue.Lookup(styles);
                    worksheet.Cells[7, 2] = masterdata["BrandID"];
                    worksheet.Cells[3, 7] = MyUtility.GetValue.Lookup(SeasonIDs);
                    worksheet.Cells[4, 7] = MyUtility.GetValue.Lookup(colors);
                    worksheet.Cells[5, 7] = MyUtility.GetValue.Lookup(BuyerDelivery);
                    worksheet.Cells[6, 7] = masterdata["ShipModeID"];
                    worksheet.Cells[7, 7] = masterdata["CustCDID"];
                    worksheet.Cells[8, 7] = MyUtility.GetValue.Lookup(kit);
                    for (int j = i * 100, k = 10; j < DT.Rows.Count && j < (i + 1) * 100; j++, k++)
                    {
                        if (k < 10 + 50)
                        {
                            worksheet.Cells[k, 1] = DT.Rows[j]["CTNStartNo"];
                            worksheet.Cells[k, 2] = DT.Rows[j]["SizeCode"];
                            worksheet.Cells[k, 3] = DT.Rows[j]["ShipQty"];
                        }
                        else
                        {
                            worksheet.Cells[k - 50, 6] = DT.Rows[j]["CTNStartNo"];
                            worksheet.Cells[k - 50, 7] = DT.Rows[j]["SizeCode"];
                            worksheet.Cells[k - 50, 8] = DT.Rows[j]["ShipQty"];
                        }
                    }

                    page++;
                }
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Packing_P03_PackingMDFormReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            //Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion 
        }
        #endregion

        #region Carton Weighing Form
        public static DualResult QueryPackingCartonWeighingForm(string PackingListID, out DataTable[] printData)
        {
            string orderidsql = $@"select o.CustPONo from PackingList_Detail pd left join orders o on o.id = pd.OrderID where pd.id = '{PackingListID}' group by CustPONo order by CustPONo";
            DataTable CustPONoDT;
            DualResult result;
            result = DBProxy.Current.Select(null, orderidsql, out CustPONoDT);
            if (!result)
            {
                printData = null;
                return result;
            }

            StringBuilder sqlCmd = new StringBuilder();
            for (int i = 0; i < CustPONoDT.Rows.Count; i++)
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
where pd.id = '{PackingListID}' and o.CustPONo = '{CustPONoDT.Rows[i]["CustPONo"]}'
group by CTNStartNo,a.SizeCode,b.ShipQty,o.CustPONo
order by min(pd.seq)
");

            }
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            return result;
        }
        #endregion

        #region Get SCICtnNo P03/P04/P05
        public static bool GetSCICtnNo(DataTable dt,string id, string type)
        {
            if (type.EqualString("IsDetailInserting"))
            {
                string sciCtnNo = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + string.Empty, "PackingList_Detail", DateTime.Today, 3, "SCICtnNo", null);
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

                string sciCtnNo = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + string.Empty, "PackingList_Detail", DateTime.Today, 3, "SCICtnNo", null);
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
        public static bool PackingP02CreateSCICtnNo(string id)
        {
            DataTable packinglist_detaildt;
            string sqlpld = $@"select Ukey,SCICtnNo,id,OrderID,OrderShipmodeSeq,CTNStartNo,Article,SizeCode from PackingList_Detail where id = '{id}' order by seq";
            DualResult result = DBProxy.Current.Select(null, sqlpld, out packinglist_detaildt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }
            
            if (!PublicPrg.Prgs.GetSCICtnNo(packinglist_detaildt, id, "IsDetailInserting"))
            {
                return false;
            }

            string sqlCreateSCICtnNo = $@"
update pd2 set 
	SCICtnNo = pd.SCICtnNo
from  PackingList_Detail pd2
inner join #tmp pd  on pd2.Ukey	= pd.Ukey				
";
            DataTable dt;
            result = MyUtility.Tool.ProcessWithDatatable(packinglist_detaildt, "SCICtnNo,Ukey", sqlCreateSCICtnNo, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }

            return true;
        }
        #endregion

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

        // 檢查OrderID+Seq不可以重複建立
        public static bool P03CheckDouble_SpSeq(string orderid, string seq, string packID)
        {
            if (MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", orderid, seq, packID)))
            {
                MyUtility.Msg.WarningBox("SP No:" + orderid + ", Seq:" + seq + " already exist in packing list, can't be create again!");
                return false;
            }

            return true;
        }

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
                                      Seq = r1.Field<string>("OrderShipmodeSeq")
                                  }
                                  into g
                                  select new
                                  {
                                      SP = g.Key.SP,
                                      Seq = g.Key.Seq
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
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@ttlShipQty";
                    sp1.Value = MyUtility.Convert.GetInt(summaryData.Rows[0]["ShipQty"]) + MyUtility.Convert.GetInt(currentMaintain["ShipQty"]);

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@ttlCTNQty";
                    sp2.Value = MyUtility.Convert.GetInt(summaryData.Rows[0]["CTNQty"]) + MyUtility.Convert.GetInt(currentMaintain["CTNQty"]);

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@ttlNW";
                    sp3.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NW"]) + MyUtility.Convert.GetDouble(currentMaintain["NW"]), 2);

                    System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                    sp4.ParameterName = "@ttlNNW";
                    sp4.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NNW"]) + MyUtility.Convert.GetDouble(currentMaintain["NNW"]), 2);

                    System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                    sp5.ParameterName = "@ttlGW";
                    // ISP20181015 GW抓到小數點後3位
                    sp5.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["GW"]) + MyUtility.Convert.GetDouble(currentMaintain["GW"]), 3);

                    System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                    sp6.ParameterName = "@ttlCBM";
                    // ISP20181015 CBM抓到小數點後4位
                    sp6.Value = MyUtility.Convert.GetDouble(summaryData.Rows[0]["CBM"]) + MyUtility.Convert.GetDouble(currentMaintain["CBM"]);

                    System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
                    sp7.ParameterName = "@INVNo";
                    sp7.Value = currentMaintain["INVNo"].ToString();

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    cmds.Add(sp4);
                    cmds.Add(sp5);
                    cmds.Add(sp6);
                    cmds.Add(sp7);
                    #endregion

                    result = Sci.Data.DBProxy.Current.Execute(null, updateCmd, cmds);
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
                decimal BookingVW = 0;
                decimal APPEstAmtVW = 0;
                string sqlcmd = $@"
select [BookingVW] = isnull(sum(p2.APPBookingVW),0)
,[APPEstAmtVW] = isnull(sum(p2.APPEstAmtVW),0)
from PackingList p1
inner join PackingList_Detail p2 on p1.ID=p2.ID
where p1.INVNo='{currentMaintain["INVNo"]}'
and p1.id !='{currentMaintain["ID"]}'";
                if (MyUtility.Check.Seek(sqlcmd, out dr))
                {
                    BookingVW = MyUtility.Convert.GetDecimal(dr["BookingVW"]);
                    APPEstAmtVW = MyUtility.Convert.GetDecimal(dr["APPEstAmtVW"]);
                }

                BookingVW += MyUtility.Convert.GetDecimal(detailDatas.Compute("sum(APPBookingVW)", string.Empty));
                APPEstAmtVW += MyUtility.Convert.GetDecimal(detailDatas.Compute("sum(APPEstAmtVW)", string.Empty));

                string updateSqlCmd = $@"
update GMTBooking
set TotalAPPBookingVW = {BookingVW}
,TotalAPPEstAmtVW = {APPEstAmtVW}
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

            result = Prgs.UpdateOrdersCTN(orderData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Update Orders CTN fail!\r\n" + result.ToString());
                return failResult;
            }

            if (!Prgs.CreateOrderCTNData(currentMaintain["ID"].ToString()))
            {
                DualResult failResult = new DualResult(false, "Create Order_CTN fail!\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }
        #endregion
    }
}