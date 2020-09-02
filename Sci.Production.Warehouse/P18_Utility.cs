using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    internal static class P18_Utility
    {
        public static DualResult CheckDetailPOID(string poID, string fromFtyID, out string dataFrom)
        {
            dataFrom = "Po_Supp_Detail";
            string strCheckOrders = string.Format(
                @"
select  o.id
from Orders o
inner join dbo.Factory f on o.FactoryID = f.ID
where   o.id = '{0}'
        and f.MDivisionID = '{1}' 
", poID, Env.User.Keyword);

            string strCheckInventory = string.Format(
                @"
select  c.POID 
from Inventory c WITH (NOLOCK) 
inner join dbo.Orders o on c.POID = o.id
inner join dbo.Factory f on o.FactoryID = f.ID
where   c.POID = '{0}'
        and f.MDivisionID = '{1}' 
", poID, Env.User.Keyword);

            string strCheckInvtrans = string.Format(
                @"
select id from Invtrans where InventoryPOID = '{0}' and type = '3'   and FactoryID = '{1}'
", poID, fromFtyID);

            if (!MyUtility.Check.Seek(strCheckOrders) && !MyUtility.Check.Seek(strCheckInventory))
            {
                if (!MyUtility.Check.Seek(strCheckInvtrans))
                {
                    return new DualResult(false, "SP# Data not found!");
                }
                else
                {
                    dataFrom = "Invtrans";
                }
            }

            return new DualResult(true);
        }

        public static DualResult CheckDetailSeq(string seq, string fromFtyID, DataRow CurrentDetailData)
        {
            // check Seq Length
            string[] seqSplit = seq.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (seqSplit.Length < 2)
            {
                return new DualResult(false, "Seq format wrong!");
            }

            string seq1 = seqSplit[0];
            string seq2 = seqSplit[1];
            DataRow dr;
            if (CurrentDetailData["DataFrom"].Equals("Po_Supp_Detail"))
            {
                #region check Po_Supp_Detail seq 1 2
                bool isExistsPoSuppDetail = MyUtility.Check.Seek(
                    string.Format(
                    @"
select  pounit
        , stockunit = dbo.GetStockUnitBySPSeq (id, seq1, seq2)
        , fabrictype
        , qty
        , scirefno
        , [description] = dbo.getmtldesc(id,seq1,seq2,2,0)
        , [Fabric] = case when FabricType = 'F' then 'Fabric' 
                             when FabricType = 'A' then 'Accessory'
                        else '' end
from po_supp_detail WITH (NOLOCK) 
where   id = '{0}' 
        and seq1 ='{1}'
        and seq2 = '{2}'", CurrentDetailData["poid"], seq1, seq2), out dr, null);
                if (isExistsPoSuppDetail)
                {
                    CurrentDetailData["stockunit"] = dr["stockunit"];
                    CurrentDetailData["Description"] = dr["description"];
                    CurrentDetailData["fabrictype"] = dr["fabrictype"];
                    CurrentDetailData["Fabric"] = dr["Fabric"];
                }
                else
                {
                    if (!MyUtility.Check.Seek(
                        string.Format(
                        @"
select  poid = p.POID 
        , seq = left (p.seq1 + ' ', 3) + p.seq2
        , p.seq1
        , p.seq2
        , p.Refno
        , Description = (select f.DescDetail from fabric f WITH (NOLOCK) where f.SCIRefno = p.scirefno) 
        , p.scirefno
from dbo.Inventory p WITH (NOLOCK) 
where   poid = '{0}' 
        and seq1 = '{1}'
        and seq2 = '{2}' 
        and factoryid = '{3}'", CurrentDetailData["poid"],
                        seq1,
                        seq2,
                        fromFtyID), out dr, null))
                    {
                        return new DualResult(false, "Seq Data not found!");
                    }
                    else
                    {
                        CurrentDetailData["stockunit"] = string.Empty;
                        CurrentDetailData["Description"] = dr["description"];
                        CurrentDetailData["fabrictype"] = dr["fabrictype"];
                        CurrentDetailData["Fabric"] = string.Empty;
                    }
                }
                #endregion
            }
            else
            {
                #region check Invtrans seq 1 2
                if (!MyUtility.Check.Seek(
                    string.Format(
                    @"
select    fabrictype
from Invtrans WITH (NOLOCK) 
where   InventoryPOID = '{0}' 
        and InventorySeq1 ='{1}'
        and InventorySeq2 = '{2}' and type = '3' and FactoryID = '{3}'", CurrentDetailData["poid"], seq1, seq2, fromFtyID), out dr, null))
                {
                    return new DualResult(false, "Seq Data not found!");
                }
                else
                {
                    CurrentDetailData["stockunit"] = string.Empty;
                    CurrentDetailData["Description"] = string.Empty;
                    CurrentDetailData["fabrictype"] = dr["fabrictype"];
                }

                #endregion
            }

            CurrentDetailData["seq"] = seq;
            CurrentDetailData["seq1"] = seq1;
            CurrentDetailData["seq2"] = seq2;
            if (CurrentDetailData["fabrictype"].ToString().ToUpper() != "F")
            {
                // CurrentDetailData["Roll"] = "";
                // CurrentDetailData["Dyelot"] = "";
            }

            return new DualResult(true);
        }

        public static DualResult CheckDetailStockTypeLocation(string stockType, string curLocation, out string newLocation)
        {
            newLocation = string.Empty;
            string sqlcmd = string.Format(
                @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", stockType);
            DataTable dt;
            DBProxy.Current.Select(null, sqlcmd, out dt);
            string[] getLocation = curLocation.Split(',').Distinct().ToArray();
            bool selectId = true;
            List<string> errLocation = new List<string>();
            List<string> trueLocation = new List<string>();
            foreach (string location in getLocation)
            {
                if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                {
                    selectId &= false;
                    errLocation.Add(location);
                }
                else if (!location.EqualString(string.Empty))
                {
                    trueLocation.Add(location);
                }
            }

            // 去除錯誤的Location將正確的Location填回
            trueLocation.Sort();
            newLocation = string.Join(",", trueLocation.ToArray());

            if (!selectId)
            {
                return new DualResult(false, "Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!");
            }

            return new DualResult(true);
        }

        public static DualResult CheckRollExists(string TransferInID, DataRow checkRow)
        {
            DataRow dr;

            // 判斷 物料 是否為 布，布料才需要 Roll & Dyelot
            if (checkRow["fabrictype"].ToString().ToUpper() == "F")
            {
                string checkSql = string.Format(
                    @"
select  1
    From dbo.TransferIn TI
	inner join dbo.TransferIn_Detail TID on TI.ID = TID.ID
	where   POID = '{0}' 
            and Seq1 = '{1}' 
            and Seq2 = '{2}' 
            and Roll = '{3}' 
            and Dyelot = '{4}' 
            and TI.ID != '{5}' 
            and Status = 'Confirmed'
", checkRow["poid"], checkRow["seq1"], checkRow["seq2"], checkRow["roll"], checkRow["dyelot"], TransferInID);

                if (MyUtility.Check.Seek(checkSql, out dr, null))
                {
                    return new DualResult(false, string.Format(
                        @"
The Deylot of
<SP#>:{0}, <Seq>:{1}, <Roll>:{2}, <Deylot>:{3} already exists
", checkRow["poid"], checkRow["seq1"].ToString() + " " + checkRow["seq2"].ToString(), checkRow["roll"], checkRow["Dyelot"].ToString().Trim()));
                }
            }

            return new DualResult(true);
        }
    }
}
