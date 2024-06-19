using Ict;
using Ict.Win.UI;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public partial class CuttingWorkOrder
    {
        /// <summary>
        /// 根據Cutting Poid，取得所有可用的Seq、Refno、Color
        /// </summary>
        private static DataTable dt_AllSeqRefnoColor;

        #region Cutting P02 專用

        /// <inheritdoc/>
        public static void SpEditingMouseDown(object sender, DataGridViewEditingControlMouseEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid, string workType)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Parent form 若是非編輯狀態，且Cutting.WorkType != 2 就 return
                if (!srcForm.EditMode || workType != "2")
                {
                    return;
                }

                DataRow dr = srcGrid.GetDataRow(e.RowIndex);
                SelectItem sele;

                string cmd = $@"SELECT ID FROM Orders WHERE POID = '{poid}' AND Junk=0";

                DBProxy.Current.Select(null, cmd, out DataTable dtSP);

                if (dtSP == null)
                {
                    return;
                }

                sele = new SelectItem(dtSP, "ID", "20", MyUtility.Convert.GetString(dr["OrderID"]), false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["OrderID"] = sele.GetSelecteds()[0]["ID"];
                e.EditingControl.Text = sele.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        public static bool SpCellValidating(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid, string workType)
        {
            if (!srcForm.EditMode || workType != "2")
            {
                return true;
            }

            // 右鍵彈出功能
            if (e.RowIndex == -1)
            {
                return true;
            }

            DataRow dr = srcGrid.GetDataRow(e.RowIndex);
            string oldvalue = dr["OrderID"].ToString();
            string newvalue = e.FormattedValue.ToString();
            if (oldvalue == newvalue)
            {
                return true;
            }

            string cmd = $@"SELECT ID FROM Orders WHERE POID = '{poid}' AND Junk=0 AND ID = @ID ";
            DBProxy.Current.Select(null, cmd, new List<SqlParameter>() { new SqlParameter("@ID", newvalue) }, out DataTable dtSP);

            if (dtSP == null || dtSP.Rows.Count == 0)
            {
                dr["OrderID"] = string.Empty;
                dr.EndEdit();
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("<SP#> : {0} data not found!", newvalue));
                return false;
            }

            dr["OrderID"] = newvalue;
            dr.EndEdit();
            return true;
        }

        /// <inheritdoc/>
        public static void ArticleEditingMouseDown(object sender, DataGridViewEditingControlMouseEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid, string workType)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Parent form 若是非編輯狀態就 return
                if (!srcForm.EditMode)
                {
                    return;
                }

                DataRow dr = srcGrid.GetDataRow(e.RowIndex);
                SelectItem sele;

                string cmd = string.Empty;

                if (MyUtility.Convert.GetString(dr["Order_EachconsUkey"]) != "0" && !MyUtility.Check.Empty(dr["Order_EachconsUkey"]))
                {
                    cmd = $@"SELECT Article FROM Order_EachCons_Article WHERE Order_EachConsUkey = {dr["Order_EachconsUkey"]}";
                }
                else if (workType == "2")
                {
                    cmd = $@"SELECT Article FROM Order_Article WHERE ID='{dr["OrderID"]}' GROUP BY Article";
                }
                else if (workType == "1")
                {
                    cmd = $@"
SELECT Article 
FROM Order_Article
INNER JOIN Orders ON Orders.ID= Order_Article.ID
WHERE 1=1
AND Orders.POID = '{poid}'
GROUP BY Article";
                }

                DBProxy.Current.Select(null, cmd, out DataTable dtArticle);

                if (dtArticle == null)
                {
                    return;
                }

                sele = new SelectItem(dtArticle, "Article", "20", MyUtility.Convert.GetString(dr["Article"]), false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["Article"] = sele.GetSelecteds()[0]["Article"];
                e.EditingControl.Text = sele.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        public static bool ArticleCellValidating(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid, string workType)
        {
            if (!srcForm.EditMode)
            {
                return true;
            }

            if (e.RowIndex == -1)
            {
                return true;
            }

            DataRow dr = srcGrid.GetDataRow(e.RowIndex);
            string oldvalue = dr["Article"].ToString();
            string newvalue = e.FormattedValue.ToString();
            if (oldvalue == newvalue)
            {
                return true;
            }

            string cmd = string.Empty;

            if (MyUtility.Convert.GetString(dr["Order_EachconsUkey"]) != "0" && !MyUtility.Check.Empty(dr["Order_EachconsUkey"]))
            {
                cmd = $@"SELECT Article FROM Order_EachCons_Article WHERE Order_EachConsUkey = {dr["Order_EachconsUkey"]} AND Article = @Article";
            }
            else if (workType == "2")
            {
                cmd = $@"SELECT Article FROM Order_Article WHERE ID='{dr["OrderID"]}' AND Article = @Article GROUP BY Article";
            }
            else if (workType == "1")
            {
                cmd = $@"
SELECT Article 
FROM Order_Article
INNER JOIN Orders ON Orders.ID= Order_Article.ID
WHERE 1=1
AND Orders.POID = '{poid}'
AND Article = @Article
GROUP BY Article";
            }

            DBProxy.Current.Select(null, cmd, new List<SqlParameter>() { new SqlParameter("@Article", newvalue) }, out DataTable dtArticle);

            if (dtArticle == null || dtArticle.Rows.Count == 0)
            {
                dr["Article"] = string.Empty;
                dr.EndEdit();
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("<Article> : {0} data not found!", newvalue));
                return false;
            }

            dr["Article"] = newvalue;
            dr.EndEdit();
            return true;
        }
        #endregion

        #region Batch Assign 的Seq驗證，由於無法取得單一筆Fabric Code，所以需要

        /// <summary>
        /// 根據Cutting Poid，取得所有可用的Seq、Refno、Color，找過一次之後存在全域變數，後續校驗繼續用
        /// </summary>
        /// <param name="id">Cutting.ID</param>
        /// <returns>DataTable</returns>
        public static DataTable GetAllSeqRefnoColor(string id)
        {
            string sqlcmd = $@"
SELECT
    psd.SEQ1
   ,psd.SEQ2
   ,psd.Refno
   ,ColorID = ISNULL(psdc.SpecValue, '')
   ,psd.SCIRefno
FROM PO_Supp_Detail psd WITH (NOLOCK)
INNER JOIN PO_Supp_Detail_Spec psdc WITH (NOLOCK) ON psdc.ID = psd.id AND psdc.seq1 = psd.seq1 AND psdc.seq2 = psd.seq2 AND psdc.SpecColumnID = 'Color'
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
WHERE psd.ID = '{id}'
AND psd.Junk = 0
AND EXISTS (
    SELECT 1
    FROM Order_BOF WITH (NOLOCK)
    INNER JOIN Fabric WITH (NOLOCK) ON Fabric.SCIRefno = Order_BOF.SCIRefno
    WHERE Order_BOF.Id = psd.ID
    AND Fabric.BrandRefNo = f.BrandRefNo
	AND EXISTS(
		SELECT FabricCode
		FROM Order_FabricCode ofc WITH(NOLOCK)
		WHERE ofc.ID =  psd.ID AND ofc.FabricCode = Order_BOF.FabricCode
	)
)
";

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            // 撈過一次之後存進全域變數
            if (MyUtility.Check.Empty(dt_AllSeqRefnoColor) || dt_AllSeqRefnoColor.Rows.Count == 0)
            {
                dt_AllSeqRefnoColor = dt.Copy();
            }
            else if (!dt.AsEnumerable().Where(o => o["ID"].ToString() == id).Any())
            {
                dt_AllSeqRefnoColor = dt.Copy();
            }

            return dt;
        }

        /// <summary>
        /// 設定 所有可用的Seq、Refno、Color 的全域變數DataTable
        /// </summary>
        /// <param name="id">Cutting.ID</param>
        public static void SetGolobalAllSeqRefnoColor(string id)
        {
            // 根據POID，找出所有 Seq、fabricCode、refno、colorID
            if (MyUtility.Check.Empty(dt_AllSeqRefnoColor) || dt_AllSeqRefnoColor.Rows.Count == 0)
            {
                GetAllSeqRefnoColor(id);
            }
            else if (!dt_AllSeqRefnoColor.AsEnumerable().Where(o => o["ID"].ToString() == id).Any())
            {
                GetAllSeqRefnoColor(id);
            }
        }

        /// <summary>
        /// 根據Cutting Poid，取得所有可用的Seq、Refno、Color，然後開窗
        /// </summary>
        /// <param name="id">Cutting.ID</param>
        /// <returns>SelectItem</returns>
        public static SelectItem PopupAllSeqRefnoColor(string id)
        {
            // 根據POID，找出所有 Seq、fabricCode、refno、colorID
            SetGolobalAllSeqRefnoColor(id);

            DataTable dt = dt_AllSeqRefnoColor.Copy();
            SelectItem selectItem = new SelectItem(dt, "Seq1,Seq2,Refno,ColorID", "3,2,20,3@500,300", string.Empty, false, ",", headercaptions: "Seq1,Seq2,Refno,Color");
            DialogResult result = selectItem.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            return selectItem;
        }

        /// <summary>
        /// 根據Cutting Poid，取得所有可用的Seq、Refno、Color，再進行Filter
        /// </summary>
        /// <param name="id">根據Cutting Poid</param>
        /// <param name="seq1">seq1</param>
        /// <param name="seq2">seq2</param>
        /// <param name="refno">refno</param>
        /// <param name="colorID">colorID</param>
        /// <returns>DataTable</returns>
        public static DataTable GetFilterAllSeqRefnoColor(string id, string seq1, string seq2, string refno, string colorID)
        {
            string filter = "1=1";
            if (!seq1.IsNullOrWhiteSpace())
            {
                filter += $" AND Seq1 = '{seq1}'";
            }

            if (!seq2.IsNullOrWhiteSpace())
            {
                filter += $" AND Seq2 = '{seq2}'";
            }

            if (!refno.IsNullOrWhiteSpace())
            {
                filter += $" AND Refno = '{refno}'";
            }

            if (!colorID.IsNullOrWhiteSpace())
            {
                filter += $" AND ColorID = '{colorID}'";
            }

            SetGolobalAllSeqRefnoColor(id);
            DataTable dt = dt_AllSeqRefnoColor.Copy();

            return dt.Select(filter).TryCopyToDataTable(dt);
        }

        /// <summary>
        /// Batch Assign的時候會批次寫入Seq，逐筆回DB檢查太慢，使用暫存的DataTable檢查
        /// </summary>
        /// <param name="id">Cutting POID</param>
        /// <param name="fabricCode">當前DateRow的fabricCode</param>
        /// <param name="seq1">當前DateRow的seq1</param>
        /// <param name="seq2">當前DateRow的seq2</param>
        /// <param name="refno">當前DateRow的refno</param>
        /// <param name="colorID">當前DateRow的colorID</param>
        /// <param name="dt">dt</param>
        /// <returns>結果</returns>
        public static bool ValidatingSeqWithoutFabricCode(string id, string fabricCode, string seq1, string seq2, string refno, string colorID, out DataTable dt)
        {
            SetGolobalAllSeqRefnoColor(id);

            // 以全域變數的DataTable來校驗，避免多次回DB撈
            dt = dt_AllSeqRefnoColor.Copy();

            dt.AsEnumerable().Where(o => o["ID"].ToString() == id
            && o["FabricCode"].ToString() == fabricCode
            && o["Seq1"].ToString() == seq1
            && o["Seq2"].ToString() == seq2
            && o["Refno"].ToString() == refno
            && o["ColorID"].ToString() == colorID).TryCopyToDataTable(dt);

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            return true;
        }
        #endregion
    }
}
