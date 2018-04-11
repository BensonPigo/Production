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

            foreach (DataRow dr in PackingListDetaildata.Rows)
            {
                if(dr.RowState == DataRowState.Deleted)
                    continue;

                if (!MyUtility.Check.Empty(dr["CTNQty"]))
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
	   left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey
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
        , oqd.Qty-isnull(pd.TtlShipQty,0)+isnull(paq.TtlDiffQty,0)-pk.ShipQty as BalanceQty
        , o.StyleID
        , o.CustPONo
        , o.SeasonID
        , Factory = STUFF(( select CONCAT(',', FtyGroup) 
                            from ( select distinct FtyGroup 
                                   from orders o 
                                   where o.id in (a.OrderID)) s 
                            for xml path('')
                          ),1,1,'')
        , sortCTNNo = TRY_Convert(int , a.CTNStartNo)
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
order by a.Seq", packingListID);
        }
        #endregion

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
        public static void PackingListToExcel_PackingListReport(string XltxName, DataRow PLdr, string ReportType, DataTable PrintData, DataTable CtnDim, DataTable QtyBDown)
        {
            #region Check Multiple
            bool boolMultiple = XltxName.EqualString("\\Packing_P03_PackingListReport_Multiple.xltx") ? true : false;
            #endregion
            #region Column & Row Index
            int bodyRowIndex            = (boolMultiple) ? 7    : 9
                , bodyRowStartIndex     = (boolMultiple) ? 7    : 9
                , bodyRowEndIndex       = (boolMultiple) ? 29   : 31
                , titleMRow             = 1                         , titleMColumn              = 1
                , titlePakingListNoRow  = 3                         , titlePakingListNoColumn   = 3
                , titleSciDeliveryRow   = 3                         , titleSciDeliveryColumn    = (boolMultiple) ? 18 : 14
                , titleSPRow            = 5                         , titleSPColumn             = 1
                , titleStyleRow         = 5                         , titleStyleColumn          = 3
                , titleOrderNoRow       = 5                         , titleOrderNoColumn        = 6
                , titlePoNoRow          = 5                         , titlePoNoColumn           = 10
                , titleInvoiceRow       = 5                         , titleInvoiceColumn        = (boolMultiple) ? 1 : 13
                , titleCustCDRow        = (boolMultiple) ? 5 : 7    , titleCustCDColumn         = (boolMultiple) ? 4 : 1
                , titleShipModeRow      = (boolMultiple) ? 5 : 7    , titleShipModeColumn       = (boolMultiple) ? 7 : 3
                , titleInClogRow        = (boolMultiple) ? 5 : 7    , titleInClogColumn         = (boolMultiple) ? 10 : 6
                , titleDestinationRow   = (boolMultiple) ? 5 : 7    , titleDestinationColumn    = (boolMultiple) ? 14 : 10
                , titleShipmentDateRow  = (boolMultiple) ? 5 : 7    , titleShipmentDateColumn   = (boolMultiple) ? 18 : 13
                , bodySPColumn          = 1
                , bodyStyleColumn       = 2
                , bodyOrderNoColumn     = 3
                , bodyPoNoColumn        = 4
                , bodyCtn1Column        = (boolMultiple) ? 5 : 1
                , bodyCtn2Column        = (boolMultiple) ? 6 : 2
                , bodyCtnsColumn        = (boolMultiple) ? 7 : 3
                , bodyColorColumn       = (boolMultiple) ? 8 : 4
                , bodySizeColumn        = (boolMultiple) ? 9 : 5
                , bodyCustSizeColumn    = (boolMultiple) ? 10 : 6
                , bodyPcCtnsColumn      = (boolMultiple) ? 11 : 7
                , bodyQtyColumn         = (boolMultiple) ? 12 : 8
                , bodyNWColumn          = (boolMultiple) ? 13 : 9
                , bodyGWColumn          = (boolMultiple) ? 14 : 10
                , bodyNNWColumn         = (boolMultiple) ? 15 : 11
                , bodyNWPcsColumn       = (boolMultiple) ? 16 : 12
                , bodyTTLNWColumn       = (boolMultiple) ? 17 : 13
                , bodyTTLGWColumn       = (boolMultiple) ? 18 : 14
                , bodyTTLNNWColumn      = (boolMultiple) ? 19 : 15
                , SippingMarkBDColumn   = (boolMultiple) ? 11 : 8
                ;
            #endregion
            #region workRange
            // strWorkRange 取 A8 & A10 是因為框線設定
            string strWorkRange = (boolMultiple) ? "A8:A8" : "A10:A10"
                    , strColumnsRange = (boolMultiple) ? "A{0}:S{0}" : "A{0}:O{0}";
            #endregion
            #region Get Sci Delivery
            DataRow[] getMinDelivery = PrintData.Select("SciDelivery = min(SciDelivery)");
            string strSciDelivery = "";
            if (getMinDelivery.Length > 0)
                strSciDelivery = string.Format("{0:yyyy/MM/dd}", getMinDelivery[0]["SciDelivery"]);
            #endregion

            string strXltName = Sci.Env.Cfg.XltPathDir + XltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            //MyUtility.Msg.WaitWindows("Starting to excel...");
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            string NameEN = MyUtility.GetValue.Lookup("NameEN", Sci.Env.User.Factory, "Factory ", "id");
            #region Set Title
            // 單筆SP 限定
            if (!boolMultiple)
            {
                worksheet.Cells[titleSPRow, titleSPColumn] = MyUtility.Convert.GetString(PrintData.Rows[0]["OrderID"]);
                worksheet.Cells[titleStyleRow, titleStyleColumn] = MyUtility.Convert.GetString(PrintData.Rows[0]["StyleID"]);
                worksheet.Cells[titleOrderNoRow, titleOrderNoColumn] = MyUtility.Convert.GetString(PrintData.Rows[0]["Customize1"]);
                worksheet.Cells[titlePoNoRow, titlePoNoColumn] = MyUtility.Convert.GetString(PrintData.Rows[0]["CustPONo"]);
            }
            worksheet.Cells[titleMRow, titleMColumn] = NameEN;
            worksheet.Cells[titlePakingListNoRow, titlePakingListNoColumn] = MyUtility.Convert.GetString(PLdr["ID"]);
            worksheet.Cells[titleSciDeliveryRow, titleSciDeliveryColumn] = strSciDelivery;
            worksheet.Cells[titleInvoiceRow, titleInvoiceColumn] = MyUtility.Convert.GetString(PLdr["INVNo"]);
            worksheet.Cells[titleCustCDRow, titleCustCDColumn] = MyUtility.Convert.GetString(PLdr["CustCDID"]);
            worksheet.Cells[titleShipModeRow, titleShipModeColumn] = MyUtility.Convert.GetString(PLdr["ShipModeID"]);
            worksheet.Cells[titleInClogRow, titleInClogColumn] = (MyUtility.Check.Empty(MyUtility.Convert.GetString(PrintData.Rows[0]["InClogQty"])) ? "0" : MyUtility.Convert.GetString(PrintData.Rows[0]["InClogQty"])) + " / " + MyUtility.Convert.GetString(PLdr["CTNQty"]) + "   ( " + MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(PrintData.Rows[0]["InClogQty"]) / MyUtility.Convert.GetDecimal(PLdr["CTNQty"]), 4) * 100) + "% )";
            worksheet.Cells[titleDestinationRow, titleDestinationColumn] = MyUtility.Convert.GetString(PrintData.Rows[0]["Alias"]);
            worksheet.Cells[titleShipmentDateRow, titleShipmentDateColumn] = MyUtility.Check.Empty(PrintData.Rows[0]["EstPulloutDate"]) ? "  /  /    " : Convert.ToDateTime(PrintData.Rows[0]["EstPulloutDate"]).ToString("d");
            #endregion

            #region Set Body
            //當要列印的筆數超過22筆，就要插入Row，因為範本只留22筆記錄的空間
            if (PrintData.Rows.Count > 22)
            {
                for (int i = 1; i <= PrintData.Rows.Count - 22; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(strWorkRange, Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            string ctnStartNo = "XXXXXX";
            foreach (DataRow dr in PrintData.Rows)
            {
                //因為有箱號不連續的問題，所以直接時用來源資料組好的數量，不用箱號箱減
                //int ctns = MyUtility.Convert.GetInt(dr["CTNEndNo"]) - MyUtility.Convert.GetInt(dr["CTNStartNo"]) + 1;
                int ctns = (int)dr["Ctns"];
                if (ctnStartNo != MyUtility.Convert.GetString(dr["CTNStartNo"]))
                {                                  
                    worksheet.Cells[bodyRowIndex, bodyCtn1Column] = MyUtility.Convert.GetString(dr["CTNStartNo"]);
                    worksheet.Cells[bodyRowIndex, bodyCtnsColumn] = MyUtility.Convert.GetString(ctns);
                    worksheet.Cells[bodyRowIndex, bodyNWColumn] = MyUtility.Convert.GetString(dr["NW"]);
                    worksheet.Cells[bodyRowIndex, bodyGWColumn] = MyUtility.Convert.GetString(dr["GW"]);
                    worksheet.Cells[bodyRowIndex, bodyNNWColumn] = MyUtility.Convert.GetString(dr["NNW"]);
                    worksheet.Cells[bodyRowIndex, bodyTTLNWColumn] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(dr["NW"]) * ctns);
                    worksheet.Cells[bodyRowIndex, bodyTTLGWColumn] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(dr["GW"]) * ctns);
                    worksheet.Cells[bodyRowIndex, bodyTTLNNWColumn] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(dr["NNW"]) * ctns);

                    // 多筆 SP 限定
                    if (boolMultiple)
                    {
                        worksheet.Cells[bodyRowIndex, bodySPColumn] = dr["OrderID"].ToString();
                        worksheet.Cells[bodyRowIndex, bodyStyleColumn] = dr["StyleID"].ToString();
                        worksheet.Cells[bodyRowIndex, bodyOrderNoColumn] = dr["Customize1"].ToString();
                        worksheet.Cells[bodyRowIndex, bodyPoNoColumn] = dr["CustPONo"].ToString();
                    }

                    if (MyUtility.Convert.GetString(dr["CTNStartNo"]) != MyUtility.Convert.GetString(dr["CTNEndNo"]))
                    {
                        worksheet.Cells[bodyRowIndex, bodyCtn2Column] = MyUtility.Convert.GetString(dr["CTNEndNo"]);
                    }
                }
                worksheet.Cells[bodyRowIndex, bodyColorColumn] = MyUtility.Convert.GetString(dr["Article"]) + " " + MyUtility.Convert.GetString(dr["Color"]);
                worksheet.Cells[bodyRowIndex, bodySizeColumn] = MyUtility.Convert.GetString(dr["SizeCode"]);
                worksheet.Cells[bodyRowIndex, bodyCustSizeColumn] = MyUtility.Convert.GetString(dr["SizeSpec"]);
                worksheet.Cells[bodyRowIndex, bodyPcCtnsColumn] = MyUtility.Convert.GetString(dr["QtyPerCTN"]);
                worksheet.Cells[bodyRowIndex, bodyQtyColumn] = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctns);
                worksheet.Cells[bodyRowIndex, bodyNWPcsColumn] = MyUtility.Check.Empty(dr["NWPerPcs"]) ? "" : MyUtility.Convert.GetString(dr["NWPerPcs"]);

                ctnStartNo = MyUtility.Convert.GetString(dr["CTNStartNo"]);
                bodyRowIndex++;
            }

            worksheet.Range[String.Format(strColumnsRange, bodyRowIndex)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[String.Format(strColumnsRange, bodyRowIndex)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;
            worksheet.Cells[bodyRowIndex, 2] = "Total";
            if (bodyRowIndex >= bodyRowStartIndex)
            {
                worksheet.Cells[bodyRowIndex, bodyCtnsColumn] = string.Format("=SUM(INDIRECT(ADDRESS({0},{1})):INDIRECT(ADDRESS({2},{1})))", bodyRowStartIndex ,bodyCtnsColumn, MyUtility.Convert.GetString(bodyRowIndex - 1));
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

            //Carton Dimension:
            bodyRowIndex++;
            StringBuilder ctnDimension = new StringBuilder();

            foreach (DataRow dr in CtnDim.Rows)
            {
                ctnDimension.Append(string.Format("{0} - {1} - {2} {3}, (CTN#:{4}){5}  \r\n"
                    , MyUtility.Convert.GetString(dr["RefNo"])
                    , MyUtility.Convert.GetString(dr["Description"])
                    , MyUtility.Convert.GetString(dr["Dimension"])
                    , MyUtility.Convert.GetString(dr["CtnUnit"])
                    , MyUtility.Check.Empty(dr["Ctn"]) ? "" : MyUtility.Convert.GetString(dr["Ctn"]).Substring(0, MyUtility.Convert.GetString(dr["Ctn"]).Length - 1)
                    , ReportType == "1" ? ", ttlCBM:" + MyUtility.Convert.GetString(dr["TtlCBM"]) : ""));
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

            //Remarks
            bodyRowIndex++;
            worksheet.Cells[bodyRowIndex, 3] = MyUtility.Convert.GetString(PLdr["Remark"]);

            // Color/Size Breakdown
            bodyRowIndex = bodyRowIndex + 2;
            if (QtyBDown.Rows.Count > 5)
            {
                Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(bodyRowIndex))).EntireRow;
                for (int i = 1; i <= QtyBDown.Rows.Count - 5; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}", MyUtility.Convert.GetString(bodyRowIndex + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                    Marshal.ReleaseComObject(rngToInsert);
                }
                Marshal.ReleaseComObject(rngToCopy);
            }
            foreach (DataRow dr in QtyBDown.Rows)
            {
                worksheet.Cells[bodyRowIndex, 1] = MyUtility.Convert.GetString(dr["DataList"]);
                bodyRowIndex++;
            }


            //Shipment mark
            bodyRowIndex = bodyRowIndex + 3;
            worksheet.Cells[bodyRowIndex, 1] = MyUtility.Convert.GetString(PrintData.Rows[0]["MarkFront"]);

            worksheet.Cells[bodyRowIndex, SippingMarkBDColumn] = MyUtility.Convert.GetString(PrintData.Rows[0]["MarkBack"]);

            string[] marks = MyUtility.Convert.GetString(PrintData.Rows[0]["MarkFront"]).Split('\r');
            string[] marks2 = MyUtility.Convert.GetString(PrintData.Rows[0]["MarkBack"]).Split('\r');
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

            worksheet.Cells[bodyRowIndex, 1] = MyUtility.Convert.GetString(PrintData.Rows[0]["MarkLeft"]);
            worksheet.Cells[bodyRowIndex, SippingMarkBDColumn] = MyUtility.Convert.GetString(PrintData.Rows[0]["MarkRight"]);

            string[] marks3 = MyUtility.Convert.GetString(PrintData.Rows[0]["MarkLeft"]).Split('\r');
            string[] marks4 = MyUtility.Convert.GetString(PrintData.Rows[0]["MarkRight"]).Split('\r');
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
        public static void PackingListToExcel_PackingGuideReport(string XltxName, DataTable PrintData, DataTable CtnDim, DataTable QtyCtn, DataTable ArticleSizeTtlShipQty, DataTable PrintGroupData, DataTable ClipData, DataRow PacklistData, int OrderQty, string SpecialInstruction)
        {            
            string strXltName = Sci.Env.Cfg.XltPathDir + XltxName;
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            //MyUtility.Msg.WaitWindows("Starting to excel...");
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            string NameEN = MyUtility.GetValue.Lookup("NameEN", Sci.Env.User.Factory, "Factory ", "id");
            worksheet.Cells[1, 1] = NameEN;
            worksheet.Cells[4, 1] = MyUtility.Convert.GetString(PrintData.Rows[0]["Factory"]);
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(PrintData.Rows[0]["OrderID"]);
            worksheet.Cells[4, 4] = PrintData.Rows[0]["BuyerDelivery"];
            worksheet.Cells[4, 6] = MyUtility.Convert.GetString(PrintData.Rows[0]["StyleID"]);
            worksheet.Cells[4, 8] = MyUtility.Convert.GetString(PrintData.Rows[0]["Customize1"]);
            worksheet.Cells[4, 10] = MyUtility.Convert.GetString(PrintData.Rows[0]["CustPONo"]);
            worksheet.Cells[4, 12] = MyUtility.Convert.GetInt(PrintData.Rows[0]["CTNQty"]);
            worksheet.Cells[4, 14] = MyUtility.Convert.GetString(PrintData.Rows[0]["CustCD"]);
            worksheet.Cells[4, 16] = MyUtility.Convert.GetString(PrintData.Rows[0]["DestAlias"]);
            worksheet.Cells[4, 18] = OrderQty;
            worksheet.Cells[4, 20] = MyUtility.Convert.GetInt(PacklistData["ShipQty"]);
            worksheet.Cells[4, 21] = "=R4-T4";

            int groupRec = PrintGroupData.Rows.Count, excelRow = 5, printRec = 1, printCtnCount = 0;

            string seq = "000000", article = "XXXX0000", size = "XXXX0000";
            int qtyPerCTN = -1;
            StringBuilder articleSize = new StringBuilder();

            for (int i = 0; ; i++)
            {
                if (i >= groupRec)
                {
                    break;
                }
                if (excelRow >= 262) //若資料會超過262筆，就先插入一筆Record，最後再把多的資料刪除
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(excelRow)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
                if (MyUtility.Check.Empty(PrintGroupData.Rows[i]["CTNQty"]))
                {
                    excelRow++;
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
            if (excelRow >= 263)
            {
                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[string.Format("A{0}:A{0}", MyUtility.Convert.GetString(excelRow + 1)), Type.Missing];
                rng.Select();
                rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                Marshal.ReleaseComObject(rng);
            }
            else
            {
                for (int i = excelRow + 1; i <= 263; i++)
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
from PackingList_Detail pd WITH (NOLOCK) 
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

    }
}
