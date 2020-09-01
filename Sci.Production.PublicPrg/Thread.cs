using System;
using System.Collections.Generic;
using System.Data;
using Sci.Data;
using Ict;
using System.Transactions;

namespace Sci.Production.PublicPrg
{
    public static partial class Prgs
    {
        #region Thread Issue Confirm Update Sql
        public static DualResult ThreadIssueConfirm(IList<DataRow> detailDatas, string mainSql, bool checkLocationStock = true)
        {
            string updatesql = mainSql;

            string checksql;
            DataRow thdr;
            string msg1 = "New cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n", msg2 = "Used cone stock is not enough, \nplease see below <Refno>,<Color>,<Location>\n";
            bool lmsg1 = false, lmsg2 = false;
            foreach (DataRow dr in detailDatas)
            {
                if (checkLocationStock)
                {
                    checksql = string.Format("Select isnull(newCone,0) as newCone,isnull(UsedCone,0) as usedCone from ThreadStock WITH (NOLOCK) where refno='{0}' and threadcolorid = '{1}' and threadlocationid ='{2}' ", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                    if (MyUtility.Check.Seek(checksql, out thdr))
                    {
                        if ((decimal)thdr["Newcone"] < (decimal)dr["NewCone"])
                        {
                            msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                            lmsg1 = true;
                        }

                        if ((decimal)thdr["UsedCone"] < (decimal)dr["UsedCone"])
                        {
                            msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                            lmsg2 = true;
                        }
                    }
                    else
                    {
                        msg1 = msg1 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                        lmsg1 = true;
                        msg2 = msg2 + string.Format("<{0}>,<{1}>,<{2}>\n", dr["refno"], dr["Threadcolorid"], dr["threadlocationid"]);
                        lmsg2 = true;
                    }
                }

                updatesql = updatesql + string.Format("update ThreadStock set UsedCone = UsedCone-{0},NewCone = NewCone-{1},editName = '{5}',editDate = GetDate() where refno ='{2}' and ThreadColorid = '{3}' and threadLocationid = '{4}' ;", dr["usedCone"], dr["newcone"], dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), Env.User.UserID);
            }

            if (lmsg1)
            {
                return new DualResult(false, msg1);
            }

            if (lmsg2)
            {
                return new DualResult(false, msg2);
            }

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        transactionscope.Dispose();
                        return upResult;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    return new DualResult(false, "Commit transaction error.", ex);
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
            return new DualResult(true);
        }

        #endregion
        #region Thread Issue UnConfirm Update Sql
        public static DualResult ThreadIssueUnConfirm(IList<DataRow> detailDatas, string mainSql)
        {
            string updateThread = mainSql;
            string insertsql = string.Empty;
            foreach (DataRow dr in detailDatas)
            {
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadStock WITH (NOLOCK) where refno ='{0}' and ThreadColorid = '{1}' and threadLocationid = '{2}'", dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString())))
                {
                    updateThread = updateThread + string.Format("update ThreadStock set UsedCone = UsedCone+ {0},NewCone = NewCone+ {1},EditName ='{5}',editDate = GetDate() where refno ='{2}' and ThreadColorid = '{3}' and threadLocationid = '{4}' ;", dr["usedCone"], dr["newcone"], dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), Env.User.UserID);
                }
                else
                {
                    insertsql = insertsql + string.Format("Insert into ThreadStock(Refno,ThreadColorid,ThreadLocationid,NewCone,UsedCone,AddName,AddDate) values('{0}','{1}','{2}',{3},{4},{5},GETDATE())", dr["refno"].ToString(), dr["threadColorid"].ToString(), dr["threadlocationid"].ToString(), dr["newcone"], dr["usedCone"], Env.User.UserID);
                }
            }

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updateThread)))
                    {
                        transactionscope.Dispose();
                        return upResult;
                    }

                    if (!MyUtility.Check.Empty(insertsql))
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, insertsql)))
                        {
                            transactionscope.Dispose();
                            return upResult;
                        }
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    return new DualResult(false, "Commit transaction error.", ex);
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
            return new DualResult(true);
        }
        #endregion
   }
}
