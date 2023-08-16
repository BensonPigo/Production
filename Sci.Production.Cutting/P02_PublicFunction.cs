using Ict;
using Ict.Win.UI;
using Sci.Data;
using Sci.Win.Tools;
using Sci.Win.UI;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public static class P02_PublicFunction
    {
        private static DataTable GetPoSuppDetail(string SCIRefno, string poid, Win.Forms.Base srcForm)
        {
            string sqlcmd = $@"
select psd.SEQ1, psd.SEQ2, ColorID = isnull(psdsC.SpecValue ,''),psd.SCIRefno,psd.Refno
from PO_Supp_Detail psd
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join Fabric f on f.SCIRefno = psd.SCIRefno
where psd.ID = '{poid}'
and exists(
	select 1 from Fabric ff
	where ff.SCIRefno = '{SCIRefno}'
	and ff.BrandRefNo = f.BrandRefNo
)
and psd.Junk = 0

";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtPoSuppDetail);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.Description);
                return null;
            }

            if (dtPoSuppDetail.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data found!");
                return null;
            }

            return dtPoSuppDetail;
        }

        /// <inheritdoc/>
        public static bool Seq1CellValidating(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid)
        {
            if (!srcForm.EditMode)
            {
                return true;
            }

            // 右鍵彈出功能
            if (e.RowIndex == -1)
            {
                return true;
            }

            DataRow dr = srcGrid.GetDataRow(e.RowIndex);
            string oldvalue = dr["seq1"].ToString();
            string newvalue = e.FormattedValue.ToString();
            if (oldvalue == newvalue)
            {
                return true;
            }

            DataTable dtPoSuppDetail = GetPoSuppDetail(dr["scirefno"].ToString(), poid, srcForm);
            if (dtPoSuppDetail == null)
            {
                return false;
            }

            var checkSeqSrource = dtPoSuppDetail.AsEnumerable();

            bool isExistsSeq1 = checkSeqSrource.Where(srcDr => srcDr["Seq1"].ToString().Equals(newvalue)).Any();

            if (!isExistsSeq1)
            {
                dr["SEQ1"] = string.Empty;
                dr.EndEdit();
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("<SEQ1> : {0} data not found!", newvalue));
                return false;
            }

            List<DataRow> resultListDr = checkSeqSrource.Where(srcDr => srcDr["Seq2"].ToString().Equals(dr["Seq2"].ToString()) &&
                                                                        srcDr["Seq1"].ToString().Equals(newvalue)).ToList();
            if (resultListDr.Count == 0)
            {
                MyUtility.Msg.WarningBox(string.Format("<SEQ1>:{0},<SEQ2>:{1} data not found!", newvalue, dr["SEQ2"]));
                dr["SEQ1"] = string.Empty;
                dr["Colorid"] = string.Empty;
                return false;
            }

            DataRow resultDr = resultListDr[0];

            if (!MyUtility.Convert.GetString(resultDr["Colorid"]).EqualString(dr["Colorid"]) && !MyUtility.Check.Empty(dr["Colorid"].ToString()))
            {
                DialogResult diaR = MyUtility.Msg.QuestionBox($@"Original assign colorID is {dr["Colorid"]}, but you locate colorID is {resultDr["Colorid"]} now , 
Do you want to continue? ");
                if (diaR == DialogResult.No)
                {
                    dr["SEQ1"] = oldvalue;
                    dr.EndEdit();
                    return true;
                }
            }

            dr["Colorid"] = resultDr["Colorid"];
            dr["SEQ1"] = newvalue;
            dr["Refno"] = resultDr["Refno"];
            dr.EndEdit();
            return true;
        }

        /// <inheritdoc/>
        public static bool Seq2CellValidating(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid)
        {
            if (!srcForm.EditMode)
            {
                return true;
            }

            // 右鍵彈出功能
            if (e.RowIndex == -1)
            {
                return true;
            }

            DataRow dr = srcGrid.GetDataRow(e.RowIndex);
            string oldvalue = dr["seq2"].ToString();
            string newvalue = e.FormattedValue.ToString();
            if (oldvalue == newvalue)
            {
                return true;
            }

            DataTable dtPoSuppDetail = GetPoSuppDetail(dr["scirefno"].ToString(), poid, srcForm);
            if (dtPoSuppDetail == null)
            {
                return false;
            }

            var checkSeqSrource = dtPoSuppDetail.AsEnumerable();

            bool isExistsSeq2 = checkSeqSrource.Where(srcDr => srcDr["Seq2"].ToString().Equals(newvalue)).Any();

            if (!isExistsSeq2)
            {
                dr["SEQ2"] = string.Empty;
                dr.EndEdit();
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("<SEQ2> : {0} data not found!", newvalue));
                return false;
            }

            List<DataRow> resultListDr = checkSeqSrource.Where(srcDr => srcDr["Seq1"].ToString().Equals(dr["Seq1"].ToString()) &&
                                                                        srcDr["Seq2"].ToString().Equals(newvalue)).ToList();
            if (resultListDr.Count == 0)
            {
                MyUtility.Msg.WarningBox(string.Format("<SEQ1>:{0},<SEQ2>:{1} data not found!", dr["SEQ1"], newvalue));
                dr["SEQ2"] = string.Empty;
                dr["Colorid"] = string.Empty;
                return false;
            }

            DataRow resultDr = resultListDr[0];

            if (!MyUtility.Convert.GetString(resultDr["Colorid"]).EqualString(dr["Colorid"]) && !MyUtility.Check.Empty(dr["Colorid"].ToString()))
            {
                DialogResult diaR = MyUtility.Msg.QuestionBox($@"Original assign colorID is {dr["Colorid"]}, but you locate colorID is {resultDr["Colorid"]} now , 
Do you want to continue? ");
                if (diaR == DialogResult.No)
                {
                    dr["SEQ2"] = oldvalue;
                    dr.EndEdit();
                    return true;
                }
            }

            dr["Colorid"] = resultDr["Colorid"];
            dr["Refno"] = resultDr["Refno"];
            dr["SEQ2"] = newvalue;
            dr.EndEdit();
            return true;
        }

        /// <inheritdoc/>
        public static void Seq1EditingMouseDown(object sender, DataGridViewEditingControlMouseEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid)
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
                DataTable poTb;
                poTb = GetPoSuppDetail(dr["scirefno"].ToString(), poid, srcForm);
                if (poTb == null)
                {
                    return;
                }

                sele = new SelectItem(poTb, "SEQ1,SEQ2,Colorid,Refno", "3,2,8,20@500,300", dr["SEQ1"].ToString(), false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (!MyUtility.Convert.GetString(sele.GetSelecteds()[0]["Colorid"]).EqualString(dr["Colorid"]) && !MyUtility.Check.Empty(dr["Colorid"].ToString()))
                {
                    DialogResult diaR = MyUtility.Msg.QuestionBox($@"Original assign colorID is {dr["Colorid"]}, but you locate colorID is {sele.GetSelecteds()[0]["Colorid"]} now , 
Do you want to continue? ");
                    if (diaR == DialogResult.No)
                    {
                        return;
                    }
                }

                dr["SEQ2"] = sele.GetSelecteds()[0]["SEQ2"];
                dr["Colorid"] = sele.GetSelecteds()[0]["Colorid"];
                dr["Refno"] = sele.GetSelecteds()[0]["Refno"];
                dr["SCIRefno"] = sele.GetSelecteds()[0]["SCIRefno"];
                e.EditingControl.Text = sele.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        public static void Seq2EditingMouseDown(object sender, DataGridViewEditingControlMouseEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid)
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
                DataTable poTb;
                poTb = GetPoSuppDetail(dr["scirefno"].ToString(), poid, srcForm);
                if (poTb == null)
                {
                    return;
                }

                sele = new SelectItem(poTb, "SEQ1,SEQ2,Colorid,Refno", "3,2,8,20@500,300", dr["SEQ2"].ToString(), false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (!MyUtility.Convert.GetString(sele.GetSelecteds()[0]["Colorid"]).EqualString(dr["Colorid"]) && !MyUtility.Check.Empty(dr["Colorid"].ToString()))
                {
                    DialogResult diaR = MyUtility.Msg.QuestionBox($@"Original assign colorID is {dr["Colorid"]}, but you locate colorID is {sele.GetSelecteds()[0]["Colorid"]} now , 
Do you want to continue? ");
                    if (diaR == DialogResult.No)
                    {
                        return;
                    }
                }

                dr["SEQ1"] = sele.GetSelecteds()[0]["SEQ1"];
                dr["Colorid"] = sele.GetSelecteds()[0]["Colorid"];
                dr["refno"] = sele.GetSelecteds()[0]["refno"];
                e.EditingControl.Text = MyUtility.Convert.GetString(sele.GetSelecteds()[0]["SEQ2"]);
            }
        }
    }
}
