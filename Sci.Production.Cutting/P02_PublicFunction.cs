using Ict;
using Ict.Win.UI;
using Sci.Data;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public static class P02_PublicFunction
    {
        private static DataTable GetPoSuppDetail(string sciRefno, string poid, Win.Forms.Base srcForm)
        {
            DataTable dtPoSuppDetail;
            string sqlcmd = $@"
Select SEQ1,SEQ2,Colorid 
From PO_Supp_Detail WITH (NOLOCK) 
Where id='{poid}' 
and SCIRefno ='{sciRefno}' 
and Junk != 1 
and seq1 not like '7%'
union all
select SEQ1,SEQ2,ColorID
from PO_Supp_Detail psd1
inner join Fabric f with (nolock) on psd1.SCIRefno = f.SCIRefno
inner join Brand b with (nolock) on b.id = f.BrandID
where exists (
	select 1
	from PO_Supp_Detail psd
	inner join Fabric with (nolock) on psd.SCIRefno = Fabric.SCIRefno
	inner join Brand with (nolock) on Brand.id = Fabric.BrandID
	where psd.SCIRefno = '{sciRefno}'
		and psd.ID = psd1.ID
		and psd.Junk != 1
		and f.Refno = Fabric.Refno
		and b.BrandGroup = Brand.BrandGroup)
and psd1.ID = '{poid}'
and psd1.Junk != 1
and psd1.seq1 like '7%'
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dtPoSuppDetail);

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

        public static void Seq1CellValidating(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid)
        {
            if (!srcForm.EditMode) { return; }
            // 右鍵彈出功能
            if (e.RowIndex == -1) return;
            DataRow dr = srcGrid.GetDataRow(e.RowIndex);
            string oldvalue = dr["seq1"].ToString();
            string newvalue = e.FormattedValue.ToString();
            if (oldvalue == newvalue) return;

            DataTable dtPoSuppDetail = GetPoSuppDetail(dr["SciRefno"].ToString(), poid, srcForm);
            if (dtPoSuppDetail == null)
            {
                return;
            }

            var checkSeqSrource = dtPoSuppDetail.AsEnumerable();

            bool isExistsSeq1 = checkSeqSrource.Where(srcDr => srcDr["Seq1"].ToString().Equals(newvalue)).Any();

            if (!isExistsSeq1)
            {
                dr["SEQ1"] = "";
                dr.EndEdit();
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("<SEQ1> : {0} data not found!", newvalue));
                return;
            }


            List<DataRow> resultListDr = checkSeqSrource.Where(srcDr => srcDr["Seq2"].ToString().Equals(dr["Seq2"].ToString()) &&
                                                                        srcDr["Seq1"].ToString().Equals(newvalue)).ToList();
            if (resultListDr.Count == 0)
            {
                MyUtility.Msg.WarningBox(string.Format("<SEQ1>:{0},<SEQ2>:{1} data not found!", newvalue, dr["SEQ2"]));
                dr["SEQ1"] = "";
                dr["Colorid"] = "";
                return;
            }

            DataRow resultDr = resultListDr[0];

            if (!MyUtility.Convert.GetString(resultDr["Colorid"]).EqualString(dr["Colorid"]) && !MyUtility.Check.Empty(dr["Colorid"].ToString()))
            {
                DialogResult DiaR = MyUtility.Msg.QuestionBox($@"Original assign colorID is {dr["Colorid"]}, but you locate colorID is {resultDr["Colorid"]} now , 
Do you want to continue? ");
                if (DiaR == DialogResult.No)
                {
                    dr["SEQ1"] = oldvalue;
                    dr.EndEdit();
                    return;
                }
            }
            dr["Colorid"] = resultDr["Colorid"];

            dr["SEQ1"] = newvalue;
            dr.EndEdit();
        }

        public static void Seq2CellValidating(object sender, Ict.Win.UI.DataGridViewCellValidatingEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid)
        {
            if (!srcForm.EditMode) { return; }
            // 右鍵彈出功能
            if (e.RowIndex == -1) return;
            DataRow dr = srcGrid.GetDataRow(e.RowIndex);
            string oldvalue = dr["seq2"].ToString();
            string newvalue = e.FormattedValue.ToString();
            if (oldvalue == newvalue) return;

            DataTable dtPoSuppDetail = GetPoSuppDetail(dr["SciRefno"].ToString(), poid, srcForm);
            if (dtPoSuppDetail == null)
            {
                return;
            }

            var checkSeqSrource = dtPoSuppDetail.AsEnumerable();

            bool isExistsSeq2 = checkSeqSrource.Where(srcDr => srcDr["Seq2"].ToString().Equals(newvalue)).Any();

            if (!isExistsSeq2)
            {
                dr["SEQ2"] = "";
                dr.EndEdit();
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("<SEQ2> : {0} data not found!", newvalue));
                return;
            }


            List<DataRow> resultListDr = checkSeqSrource.Where(srcDr => srcDr["Seq1"].ToString().Equals(dr["Seq1"].ToString()) &&
                                                                        srcDr["Seq2"].ToString().Equals(newvalue)).ToList();
            if (resultListDr.Count == 0)
            {
                MyUtility.Msg.WarningBox(string.Format("<SEQ1>:{0},<SEQ2>:{1} data not found!", dr["SEQ1"], newvalue));
                dr["SEQ2"] = "";
                dr["Colorid"] = "";
                return;
            }

            DataRow resultDr = resultListDr[0];

            if (!MyUtility.Convert.GetString(resultDr["Colorid"]).EqualString(dr["Colorid"]) && !MyUtility.Check.Empty(dr["Colorid"].ToString()))
            {
                DialogResult DiaR = MyUtility.Msg.QuestionBox($@"Original assign colorID is {dr["Colorid"]}, but you locate colorID is {resultDr["Colorid"]} now , 
Do you want to continue? ");
                if (DiaR == DialogResult.No)
                {
                    dr["SEQ2"] = oldvalue;
                    dr.EndEdit();
                    return;
                }
            }
            dr["Colorid"] = resultDr["Colorid"];

            dr["SEQ2"] = newvalue;
            dr.EndEdit();
        }

        public static void Seq1EditingMouseDown(object sender, DataGridViewEditingControlMouseEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Parent form 若是非編輯狀態就 return 
                if (!srcForm.EditMode) { return; }
                DataRow dr = srcGrid.GetDataRow(e.RowIndex);
                SelectItem sele;
                DataTable poTb;
                poTb = GetPoSuppDetail(dr["SciRefno"].ToString(), poid, srcForm);
                if (poTb == null)
                {
                    return;
                }

                sele = new SelectItem(poTb, "SEQ1,SEQ2,Colorid", "3,2,8@350,300", dr["SEQ1"].ToString(), false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                if (!MyUtility.Convert.GetString(sele.GetSelecteds()[0]["Colorid"]).EqualString(dr["Colorid"]) && !MyUtility.Check.Empty(dr["Colorid"].ToString()))
                {
                    DialogResult DiaR = MyUtility.Msg.QuestionBox($@"Original assign colorID is {dr["Colorid"]}, but you locate colorID is {sele.GetSelecteds()[0]["Colorid"]} now , 
Do you want to continue? ");
                    if (DiaR == DialogResult.No)
                    {
                        return;
                    }
                }
                dr["SEQ2"] = sele.GetSelecteds()[0]["SEQ2"];
                dr["Colorid"] = sele.GetSelecteds()[0]["Colorid"];
                e.EditingControl.Text = sele.GetSelectedString();
            }
        }

        public static void Seq2EditingMouseDown(object sender, DataGridViewEditingControlMouseEventArgs e, Win.Forms.Base srcForm, Grid srcGrid, string poid)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Parent form 若是非編輯狀態就 return 
                if (!srcForm.EditMode) { return; }
                DataRow dr = srcGrid.GetDataRow(e.RowIndex);
                SelectItem sele;
                DataTable poTb;
                poTb = GetPoSuppDetail(dr["SciRefno"].ToString(), poid, srcForm);
                if (poTb == null)
                {
                    return;
                }

                sele = new SelectItem(poTb, "SEQ1,SEQ2,Colorid", "3,2,8@350,300", dr["SEQ2"].ToString(), false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                if (!MyUtility.Convert.GetString(sele.GetSelecteds()[0]["Colorid"]).EqualString(dr["Colorid"]) && !MyUtility.Check.Empty(dr["Colorid"].ToString()))
                {
                    DialogResult DiaR = MyUtility.Msg.QuestionBox($@"Original assign colorID is {dr["Colorid"]}, but you locate colorID is {sele.GetSelecteds()[0]["Colorid"]} now , 
Do you want to continue? ");
                    if (DiaR == DialogResult.No)
                    {
                        return;
                    }
                }
                dr["SEQ1"] = sele.GetSelecteds()[0]["SEQ1"];
                dr["Colorid"] = sele.GetSelecteds()[0]["Colorid"];
                e.EditingControl.Text = MyUtility.Convert.GetString(sele.GetSelecteds()[0]["SEQ2"]);

            }
        }
    }
}
