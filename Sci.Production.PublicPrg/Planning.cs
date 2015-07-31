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
        #region GetWorkDate
        /// <summary>
        /// GetWorkDate()
        /// </summary>
        /// <param name="String factoryid"></param>
        /// <param name="int days"></param>
        /// <param name="DateTime basicdate"></param>
        /// <returns>datetime workdate</returns>
        public static DateTime GetWorkDate(string factoryid, int days, DateTime basicdate)
        {
            string sqlcmd = "";
            sqlcmd = string.Format(@"declare @days as int  = {0} ,@count as int = 0, @bascidate as date = '{1}';
                                                declare @fetchdate as date;

                                                while @days <> 0
                                                begin
	                                                if  DATEPART(WEEKDAY, dateadd(day, @count,@bascidate)) >1
	                                                begin
		                                                DECLARE _cursor CURSOR FOR
		                                                select h.HolidayDate from Holiday h
		                                                where h.FactoryID='{2}'
		                                                and h.HolidayDate = dateadd(day, @count,@bascidate);
		                                                OPEN _cursor;
		                                                FETCH NEXT FROM _cursor INTO @fetchdate; 
		                                                if @@FETCH_STATUS != 0
		                                                begin
			                                                if @days > 0
				                                                set @days-=1;
			                                                else
				                                                set @days+=1;
		                                                end
		                                                CLOSE _cursor;
		                                                DEALLOCATE _cursor;
	                                                end
                                                    if @days > 0
		                                                set @count+=1;
													if @days < 0
		                                                set @count-=1;
                                                end
                                                select dateadd(day, @count,@bascidate) as workdate", days, basicdate.ToShortDateString(), factoryid);

            return DateTime.Parse((MyUtility.GetValue.Lookup(sqlcmd, null)));
        

        }
        #endregion
        #region GetStdQ
        /// <summary>
        /// GetStdQ()
        /// </summary>
        /// <param name="String OrderID"></param>
        /// <returns>Int StdQ</returns>
        public static int GetStdQ(string orderid)
        {
            DataTable dt;
            string sqlcmd = "";
            sqlcmd = string.Format(@"WITH cte (DD,num, INLINE,OrderID,sewinglineid,FactoryID,stdq,ComboType) AS (  
      SELECT DATEDIFF(DAY,A.Inline,A.Offline)+1 AS DD
                    , 1 as num
                    , convert(date,A.Inline) inline 
                    ,A.OrderID
                    ,sewinglineid
                    ,a.FactoryID
                    ,(a.WorkHour / a.WorkDay * a.StandardOutput) stdq
                    ,a.ComboType
	  FROM SewingSchedule A WHERE ORDERID='{0}'
      UNION ALL  
      SELECT DD,num + 1, DATEADD(DAY,1,INLINE) ,ORDERID,sewinglineid,FactoryID,stdq,ComboType
	  FROM cte a where num < DD  AND ORDERID='{0}'
    )  
	select min(stdq) stdq
	from (
	 SELECT a.orderid,a.sewinglineid,a.ComboType,a.INLINE,sum(a.stdq) stdq, isnull(b.hours,0) workhours
	 FROM cte a left join WorkHour b on convert(date,a.inline) = b.date and a.sewinglineid = b.SewingLineID and a.FactoryID=b.FactoryID 
	 group by a.orderid,a.sewinglineid,a.ComboType,a.INLINE,b.Hours
	 having isnull(b.hours,0) > 0) tmp", orderid);
            DBProxy.Current.Select(null,sqlcmd,out dt);
            //return int.Parse(dt.Rows[0][0].ToString());
            return int.Parse(Math.Ceiling(decimal.Parse(MyUtility.GetValue.Lookup(sqlcmd, null))).ToString());
        }
        #endregion
    }

}
