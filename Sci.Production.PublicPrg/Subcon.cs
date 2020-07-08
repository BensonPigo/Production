using System;
using Sci.Data;
using Ict;

namespace Sci.Production.PublicPrg
{
    public static partial class Prgs
    {
        #region GetItemDesc

        /// <summary>
        /// GetItemDesc()
        /// </summary>
        /// <param name="String Category"></param>
        /// <param name="String Refno"></param>
        /// <returns>String Desc</returns>
        public static string GetItemDesc(string category, string refno)
        {
            string desc = MyUtility.GetValue.Lookup(string.Format(
                @"
                    select description from localitem WITH (NOLOCK) where refno = '{0}' and category = '{1}'", refno.Replace("'", "''"), category));
            string[] descs = desc.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (descs.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return descs[0];
            }
        }
        #endregion

        public static bool CheckNeedPlanningP03Quote(string artworktypeID)
        {
            string sqlCheckNeedPlanningP03Quote = $@"
select 1 from ArtworkType with (nolock) 
            where ID = '{artworktypeID}' and 
                        ((IsArtwork = 1 and IsPrice = 1) or (IsArtwork = 0 and UseArtwork = 1))";

            return MyUtility.Check.Seek(sqlCheckNeedPlanningP03Quote);
        }

        public static bool CheckIsArtworkorUseArtwork(string artworktypeID)
        {
            string sqlCheckIsArtworkorUseArtwork = $@"
select 1 from ArtworkType with (nolock) 
            where ID = '{artworktypeID}' and (IsArtwork = 1 or UseArtwork = 1)";

            return MyUtility.Check.Seek(sqlCheckIsArtworkorUseArtwork);
        }

        public static DualResult UpdateArtworkReq_DetailArtworkPOID(string artworkPOID)
        {
            string sqlUpdateArtworkReq_Detail = $@"
update ard  set ard.ArtworkPOID = '{artworkPOID}'
    from ArtworkReq_Detail ard
    where exists(select 1   from ArtworkPO_Detail apd
                            where   apd.ID = '{artworkPOID}' and
                                    apd.ArtworkReqID = ard.ID and 
                                    apd.OrderID = ard.OrderID and
                                    apd.ArtworkId = ard.ArtworkId and
                                    apd.PatternCode = ard.PatternCode and
                                    apd.PatternDesc = ard.PatternDesc )

";
            return DBProxy.Current.Execute(null, sqlUpdateArtworkReq_Detail);
        }

        public enum CallFormAction
        {
            Save = 1,
            Confirm = 2,
        }

        public static DualResult CheckLocalSupp_BankStatus(string suppID, CallFormAction callFormAction)
        {
            string sqlCheck = $@"select top 1 Status from dbo.LocalSupp_Bank where ID = '{suppID}' order by Adddate desc";
            string localSupp_BankStatus = MyUtility.GetValue.Lookup(sqlCheck);
            string hintMsg = string.Empty;
            switch (localSupp_BankStatus)
            {
                case "Confirmed":
                    return new DualResult(true);
                default:
                    if (callFormAction == CallFormAction.Save)
                    {
                        hintMsg = "The bank account did not be approved yet, please ask FIN team check the data or it cannot go to next step.";
                        MyUtility.Msg.InfoBox(hintMsg);
                        return new DualResult(true, hintMsg);
                    }
                    else
                    {
                        hintMsg = "The bank account did not be approved yet, please ask FIN team check the data in [Basic B04] first!";
                        MyUtility.Msg.WarningBox(hintMsg);
                        return new DualResult(false, hintMsg);
                    }
            }
        }
    }
}
