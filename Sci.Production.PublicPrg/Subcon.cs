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
        #region GetItemDesc
        /// <summary>
        /// GetItemDesc()
        /// </summary>
        /// <param name="String Category"></param>
        /// <param name="String Refno"></param>
        /// <returns>String Desc</returns>
        public static string GetItemDesc(string category, string refno)
        {
            string desc = MyUtility.GetValue.Lookup(string.Format(@"
                    select description from localitem WITH (NOLOCK) where refno = '{0}' and category = '{1}'", refno.Replace("'", "''"), category));
            string[] descs = desc.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (descs.Length == 0)
                return "";
            else
                return descs[0];
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
    }
    
}
