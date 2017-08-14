﻿using System;
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
        #region Query QA Inspection header function QA 的表頭抓取共用程式
        /// <summary>
        /// 可取出Style,Season,Brand,Cutinline,earliestSciDelivery,MtlLeadTime from Orders,System
        /// </summary>
        /// <param name="poid"></param>
        /// <returns>DataRow</returns>
        public static DualResult QueryQaInspectionHeader(string poid, out DataRow orderDr)
        {
            
            DataTable queryTb;
            string query = string.Format("Select distinct a.styleid, a.seasonid,a.brandid,a.cutinline,a.category from Orders a WITH (NOLOCK) Where a.poid ='{0}'", poid);
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

            if (cu == DBNull.Value||cu.Empty()) cutinline = null;
            else cutinline = Convert.ToDateTime(cu);

            if (del == DBNull.Value) sciDelv = null;
            else sciDelv = Convert.ToDateTime(del);
            DateTime? TargetSciDel;
            double mtlLeadT = Convert.ToDouble(MyUtility.GetValue.Lookup("Select MtlLeadTime from System WITH (NOLOCK) ", null));
            if (sciDelv == null) return null;
            if (MyUtility.Check.Empty(mtlLeadT)) TargetSciDel = sciDelv; 
            else TargetSciDel = ((DateTime)sciDelv).AddDays(Convert.ToDouble(mtlLeadT));
            
            if (cutinline < TargetSciDel) return cutinline;
            else return TargetSciDel;
        }
        #endregion;
        #region 判斷Physical OverallResult, Status
        /// <summary>
        /// 判斷並回寫Physical OverallResult, Status string[0]=Result, string[1]=status
        /// </summary>
        /// <param name ="ID"></param>
        /// <returns></returns>
        public static string[] GetOverallResult_Status(object fir_id)
        {
            DataRow maindr;
            MyUtility.Check.Seek(string.Format("Select * from Fir WITH (NOLOCK) Where id={0}", fir_id), out maindr);
            string allResult = "";
            string status = "New";

            #region 新改的邏輯
            //判斷Result是Pass的唯一狀況
            if (
                (maindr["Physical"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["Nonphysical"])) &&
                (maindr["Weight"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonWeight"])) &&
                (maindr["ShadeBond"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonShadeBond"])) &&
                (maindr["Continuity"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonContinuity"]))
                )
            {
                maindr["Result"] = "Pass";
                maindr["Status"] = "Confirmed";
            }
            //判斷Result 是空值
            else if (
                (MyUtility.Check.Empty(maindr["Physical"]) && !MyUtility.Convert.GetBool(maindr["Nonphysical"])) ||
                (MyUtility.Check.Empty(maindr["Weight"]) && !MyUtility.Convert.GetBool(maindr["NonWeight"])) ||
                (MyUtility.Check.Empty(maindr["ShadeBond"]) && !MyUtility.Convert.GetBool(maindr["NonShadeBond"])) ||
                (MyUtility.Check.Empty(maindr["Continuity"]) && !MyUtility.Convert.GetBool(maindr["NonContinuity"])))
            {
                maindr["Result"] = "";
            }
            else
            {
                maindr["Result"] = "Fail";
            }                
            #endregion

            #region 原本舊的邏輯
            ////當(FIR.Physical 有值或FIR.nonphysical=T) 且(FIR.Weight有值或FIR.nonWeight=T)且(FIR.Shadebond或FIR.nonShade=T)且(FIR.Continuity有值或FIR.noncontinuity=T) 才回寫Fir.Result，只要其中一個FIR.Physical, FIR.Weight, FIR.Shadebond, FIR.Continuity 的值為’F’，Fir.Result 就回寫’F’ 
            //if ((!MyUtility.Check.Empty(maindr["Physical"]) || MyUtility.Convert.GetBool(maindr["Nonphysical"])) 
            //    && (!MyUtility.Check.Empty(maindr["Weight"]) || MyUtility.Convert.GetBool(maindr["NonWeight"]))
            //    && (!MyUtility.Check.Empty(maindr["ShadeBond"]) || MyUtility.Convert.GetBool(maindr["NonShadeBond"])) 
            //    && (!MyUtility.Check.Empty(maindr["Continuity"]) || MyUtility.Convert.GetBool(maindr["NonContinuity"])))
            //{
            //    if (maindr["Physical"].ToString() == "Fail" || 
            //        maindr["Weight"].ToString() == "Fail" || 
            //        maindr["ShadeBond"].ToString() == "Fail" || 
            //        maindr["Continuity"].ToString() == "Fail") 
            //        allResult = "Fail";
            //    else allResult = "Pass";

            //    status = "Confirmed";
            //}
            #endregion

            string[] re_str = { allResult, status };
            return re_str;
        }
        #endregion
        #region 判斷FIR_Laboratory OverallResult
        /// <summary>
        /// 判斷並回寫FIR_Laboratory OverallResult, string[0]=Result
        /// </summary>
        /// <param name ="ID"></param>
        /// <returns></returns>
        public static string[] GetOverallResult_Lab(object fir_id)
        {
            DataRow maindr;
            MyUtility.Check.Seek(string.Format("Select * from FIR_Laboratory WITH (NOLOCK) Where id={0}", fir_id), out maindr);
            string allResult = "";

            //當(FIR_Laboratory.Crocking 有值或FIR_Laboratory.nonCrocking=T) 且(FIR_Laboratory.Wash有值或FIR_Laboratory.nonWash=T)且(FIR_Laboratory.Heat或FIR_Laboratory.nonHeat=T) 才回寫FIR_Laboratory.Result，只要其中一個FIR_Laboratory.Crocking, FIR_Laboratory.Wash, FIR_Laboratory.Heat 的值為’F’，Fir.Result 就回寫’F’ 
            if ((!MyUtility.Check.Empty(maindr["Crocking"]) || MyUtility.Convert.GetBool(maindr["nonCrocking"]))
                && (!MyUtility.Check.Empty(maindr["Wash"]) || MyUtility.Convert.GetBool(maindr["nonWash"]))
                && (!MyUtility.Check.Empty(maindr["Heat"]) || MyUtility.Convert.GetBool(maindr["nonHeat"])))
            {
                if (maindr["Crocking"].ToString() == "Fail" ||
                    maindr["Wash"].ToString() == "Fail" ||
                    maindr["Heat"].ToString() == "Fail")
                    allResult = "Fail";
                else allResult = "Pass";              
            }
            string[] re_str = { allResult };
            return re_str;
        }
        #endregion
        /// <summary>
        /// Double Click後將Result替換成相反結果(Pass<=>Fail)
        /// </summary>
        /// <returns></returns>
        public class cellResult : DataGridViewGeneratorTextColumnSettings
        {
            public static DataGridViewGeneratorTextColumnSettings GetGridCell()
            {
                cellResult Result = new cellResult();
                Result.CellMouseDoubleClick += (s, e) =>
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) return;
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    if (dr["Result"].ToString().ToUpper() == "PASS")
                    {
                        dr["Result"] = "Fail";

                    }
                    else dr["Result"] = "Pass";
                   
                };
                return Result;
            }
        }
    }
}