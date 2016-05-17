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
        #region Query QA Inspection header function QA 的表頭抓取共用程式
        /// <summary>
        /// 可取出Style,Season,Brand,Cutinline,earliestSciDelivery,MtlLeadTime from Orders,System
        /// </summary>
        /// <param name="poid"></param>
        /// <returns>DataRow</returns>
        public static DualResult QueryQaInspectionHeader(string poid, out DataRow orderDr)
        {
            
            DataTable queryTb;
            string query = string.Format("Select distinct a.styleid, a.seasonid,a.brandid,a.cutinline,a.category from Orders a Where a.poid ='{0}'", poid);
            DualResult dResult = DBProxy.Current.Select(null, query, out queryTb);
            if (dResult && queryTb.Rows.Count > 0) orderDr = queryTb.Rows[0];
            else orderDr = null;
            return dResult;
        }
        #endregion;
        #region 找QA 的Target Lead Time (SCIDelivery -System.MtlLeadTime)
        /// <summary>
        /// 找QA 的Target Lead Time (比較Cutinline跟SciDelv-system.MtlLeadTime找出比較小的日期)
        /// </summary>
        /// <param name="poid"></param>
        /// <returns>DataRow</returns>
        public static DateTime? GetTargetLeadTime(Object cu,Object del)
        {
            DateTime? cutinline,sciDelv;

            if (cu == DBNull.Value) cutinline = null;
            else cutinline = Convert.ToDateTime(cu);

            if (del == DBNull.Value) sciDelv = null;
            else sciDelv = Convert.ToDateTime(del);
            DateTime? TargetSciDel;
            double mtlLeadT = Convert.ToDouble(MyUtility.GetValue.Lookup("Select MtlLeadTime from System", null));
            if (MyUtility.Check.Empty(mtlLeadT))  TargetSciDel= sciDelv;
            else TargetSciDel = ((DateTime)sciDelv).AddDays(Convert.ToDouble(mtlLeadT));
            
            if (cutinline < TargetSciDel) return cutinline;
            else return TargetSciDel;
        }
        #endregion;
    }
}