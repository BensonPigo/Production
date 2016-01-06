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
 from Pullout p, Pullout_Detail pd, Pullout_Detail_Detail pdd
 where p.Status != 'New'
 and p.id = pd.ID
 and pd.OrderID = '{0}'
 and pd.OrderShipmodeSeq = '{1}'
 and p.ID = pdd.ID
 and pd.UKey = pdd.UKey
 and pdd.Article = '{2}'
 and pdd.SizeCode = '{3}'
),
InvadjQty
as
(select isnull(sum(iaq.DiffQty),0) as DiffQty
 from InvAdjust ia, InvAdjust_Qty iaq
 where ia.OrderID = '{0}'
 and ia.OrderShipmodeSeq = '{1}'
 and ia.ID = iaq.ID
 and iaq.Article = '{2}'
 and iaq.SizeCode = '{3}'
)

select isnull(oqd.Qty,0) as OrderQty,(select ShipQty from PulloutQty)+(select DiffQty from InvadjQty) as ShipQty
from Order_QtyShip_Detail oqd
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
            if (PackingListDetaildata.Rows.Count <= 0)
            {
                return;
            }
            DataTable weightData, tmpWeightData;
            DataRow[] weight;
            string message = "";
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, "select '' as BrandID, '' as StyleID, '' as SeasonID, Article, SizeCode, NW, NNW from Style_WeightData where StyleUkey = ''", out weightData)))
            {
                MyUtility.Msg.WarningBox("Query 'weightData' schema fail!");
                return;
            }

            //檢查是否所有的SizeCode都有存在Style_WeightData中
            foreach (DataRow dr in PackingListDetaildata.Rows)
            {
                string filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}'", PackingListData["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length == 0)
                {
                    //先將屬於此訂單的Style_WeightData給撈出來
                    string sqlCmd = string.Format(@"select a.ID as StyleID,a.BrandID,a.SeasonID,isnull(b.Article,'') as Article,isnull(b.SizeCode,'') as SizeCode,isnull(b.NW,0) as NW,isnull(b.NNW,0) as NNW
from Style a
left join Style_WeightData b on b.StyleUkey = a.Ukey
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
            result = DBProxy.Current.Select(null, "select CTNStartNo, NW, NNW, GW from PackingList_Detail where 1=0", out tmpPacklistWeight);
            DataRow tmpPacklistRow;
            string ctnNo = PackingListDetaildata.Rows[0]["CTNStartNo"].ToString();

            foreach (DataRow dr in PackingListDetaildata.Rows)
            {
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
                    localItemWeight = MyUtility.GetValue.Lookup("Weight", MyUtility.Convert.GetString(dr["RefNo"]), "LocalItem", "RefNo");
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
from PackingList_Detail
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
 from PackingList pl, PackingList_Detail pld
 where pl.ID = '{0}'
 and pld.ID = pl.ID
),
PackedData
as
(select pld.OrderID,pld.Article,pld.SizeCode,sum(pld.ShipQty) as PackedShipQty
 from PackingList pl, PackingList_Detail pld, PackOrderID poid
 where pld.OrderID = poid.OrderID
 and pl.ID = pld.ID
 and pl.Status = 'Confirmed'
 group by pld.OrderID,pld.Article,pld.SizeCode
),
PackingData
as
(select pld.OrderID,pld.Article,pld.SizeCode,sum(pld.ShipQty) as ShipQty
 from PackingList_Detail pld
 where pld.ID = '{0}'
 group by pld.OrderID,pld.Article,pld.SizeCode
),
InvadjQty
as
(select ia.OrderID,iaq.Article, iaq.SizeCode,sum(iaq.DiffQty) as DiffQty
 from InvAdjust ia, InvAdjust_Qty iaq, PackOrderID poid
 where ia.OrderID = poid.OrderID
 and ia.ID = iaq.ID
 group by ia.OrderID,iaq.Article, iaq.SizeCode
),
SewingData
as
(select a.OrderID,a.Article,a.SizeCode,MIN(a.QAQty) as QAQty
 from (select poid.OrderID,oq.Article,oq.SizeCode, sl.Location, isnull(sum(sodd.QAQty),0) as QAQty
	   from PackOrderID poid
	   left join Orders o on o.ID = poid.OrderID
	   left join Order_Qty oq on oq.ID = o.ID
	   left join Style_Location sl on sl.StyleUkey = o.StyleUkey
	   left join SewingOutput_Detail_Detail sodd on sodd.OrderId = o.ID and sodd.Article = oq.Article  and sodd.SizeCode = oq.SizeCode and sodd.ComboType = sl.Location
	   group by poid.OrderID,oq.Article,oq.SizeCode, sl.Location) a
 group by a.OrderID,a.Article,a.SizeCode
)
select poid.OrderID,isnull(oq.Article,'') as Article,isnull(oq.SizeCode,'') as SizeCode, isnull(oq.Qty,0) as Qty, isnull(pedd.PackedShipQty,0)+isnull(pingd.ShipQty,0) as PackQty,isnull(iq.DiffQty,0) as DiffQty, isnull(sd.QAQty,0) as QAQty
from PackOrderID poid
left join Order_Qty oq on oq.ID = poid.OrderID
left join PackedData pedd on pedd.Article = oq.Article and pedd.SizeCode = oq.SizeCode
left join PackingData pingd on pingd.Article = oq.Article and pingd.SizeCode = oq.SizeCode
left join InvadjQty iq on iq.Article = oq.Article and iq.SizeCode = oq.SizeCode
left join SewingData sd on sd.Article = oq.Article and sd.SizeCode = oq.SizeCode
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
            return string.Format(@"with AllOrderID
as
(select Distinct OrderID,OrderShipmodeSeq
 from PackingList_Detail
 where ID = '{0}'
),
AccuPKQty
as
(select pd.OrderID,pd.OrderShipmodeSeq,pd.Article,pd.SizeCode,sum(pd.ShipQty) as TtlShipQty
 from PackingList_Detail pd, AllOrderID a
 where ID != '{0}'
 and a.OrderID = pd.OrderID
 and a.OrderShipmodeSeq = pd.OrderShipmodeSeq
 group by pd.OrderID,pd.OrderShipmodeSeq,pd.Article,pd.SizeCode
),
PulloutAdjQty
as
(select ia.OrderID,ia.OrderShipmodeSeq,iaq.Article,iaq.SizeCode,sum(iaq.DiffQty) as TtlDiffQty
 from InvAdjust ia, InvAdjust_Qty iaq, AllOrderID a
 where ia.OrderID = a.OrderID
 and ia.OrderShipmodeSeq = a.OrderShipmodeSeq
 and ia.ID = iaq.ID
 group by ia.OrderID,ia.OrderShipmodeSeq,iaq.Article,iaq.SizeCode
),
PackQty
as
(select OrderID,OrderShipmodeSeq,Article,SizeCode,sum(ShipQty) as ShipQty
 from PackingList_Detail
 where ID = '{0}'
 group by OrderID,OrderShipmodeSeq,Article,SizeCode
)
select a.*, b.Description, oqd.Qty-isnull(pd.TtlShipQty,0)+isnull(paq.TtlDiffQty,0)-pk.ShipQty as BalanceQty,o.StyleID,o.CustPONo,o.SeasonID
from PackingList_Detail a
left join LocalItem b on b.RefNo = a.RefNo
left join AccuPKQty pd on a.OrderID = pd.OrderID and a.OrderShipmodeSeq = pd.OrderShipmodeSeq and pd.Article = a.Article and pd.SizeCode = a.SizeCode
left join PulloutAdjQty paq on a.OrderID = paq.OrderID and a.OrderShipmodeSeq = paq.OrderShipmodeSeq and paq.Article = a.Article and paq.SizeCode = a.SizeCode
left join PackQty pk on a.OrderID = pk.OrderID and a.OrderShipmodeSeq = pk.OrderShipmodeSeq and pk.Article = a.Article and pk.SizeCode = a.SizeCode
left join Order_QtyShip_Detail oqd on oqd.Id = a.OrderID and oqd.Seq = a.OrderShipmodeSeq and oqd.Article = a.Article and oqd.SizeCode = a.SizeCode
left join Orders o on o.ID = a.OrderID
where a.id = '{0}'
order by a.Seq", packingListID);
        }
        #endregion
    }
}
