using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R37 : Sci.Win.Tems.PrintForm
    {     
        List<SqlParameter> lis;
        DataTable dtList; string cmd;
        DataTable dt; string cmdDt;
        DateTime? DebDate1; DateTime? DebDate2;
        DateTime? ConDate1; DateTime? ConDate2;
        DateTime? SettDate1; DateTime? SettDate2;
        public R37()
        {
           
            InitializeComponent();
            this.combFac.SelectedIndex = 0;
            this.comboPay.SelectedIndex = 0;
            this.comboReport.SelectedIndex = 0;
            
        }

        protected override bool ValidateInput()
        {

            DebDate1 = DebDate.Value1;
            DebDate2 = DebDate.Value2;
            ConDate1 = ConDate.Value1;
            ConDate2 = ConDate.Value2;
            SettDate1 = SettDate.Value1;
            SettDate2 = SettDate.Value2;
            string sqlWhere = ""; 
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            
            if (!this.DebDate.Value1.Empty() && !this.DebDate.Value2.Empty())
            {
                sqlWheres.Add("a.Issuedate between @debitdate1 and @debitdate2");
                lis.Add(new SqlParameter("@debitdate1", DebDate1));
                lis.Add(new SqlParameter("@debitdate2", DebDate2));
            } if (!this.ConDate.Value1.Empty() && !this.ConDate.Value2.Empty())
            {
                sqlWheres.Add("a.Issuedate between @debitdate1 and @debitdate2");
                lis.Add(new SqlParameter("@ConDate1", ConDate1));
                lis.Add(new SqlParameter("@ConDate2", ConDate2));
            }if (!this.SettDate.Value1.Empty() && !this.SettDate.Value2.Empty())
            {
                sqlWheres.Add("having a.SettleDate between @debitdate1 and @debitdate2");
                lis.Add(new SqlParameter("@SettledDate1", SettDate1));
                lis.Add(new SqlParameter("@SettledDate2", SettDate2));
            } 
           
            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
           
            lis = new List<SqlParameter>();
            cmd = string.Format(@"" + sqlWhere + ' ');
            cmdDt = string.Format(@"" + sqlWhere + ' ');
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select("", cmd, lis, out dtList);
            if (!res)
            {
                return res;
            }
            res = DBProxy.Current.Select("", cmdDt, lis, out dt);
            if (!res)
            {
                return res;
            }
            return base.OnAsyncDataLoad(e);
        }
       
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dtList == null || dtList.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            } if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            if ("List".EqualString(this.comboReport.Text))
            {
                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Subcon_R37_List.xltx");
                string d1 = (MyUtility.Check.Empty(DebDate1)) ? "" : Convert.ToDateTime(DebDate1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(DebDate2)) ? "" : Convert.ToDateTime(DebDate2).ToString("yyyy/MM/dd");
                string d3 = (MyUtility.Check.Empty(ConDate1)) ? "" : Convert.ToDateTime(ConDate1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(ConDate2)) ? "" : Convert.ToDateTime(ConDate2).ToString("yyyy/MM/dd");
                string d5 = (MyUtility.Check.Empty(SettDate1)) ? "" : Convert.ToDateTime(SettDate1).ToString("yyyy/MM/dd");
                string d6 = (MyUtility.Check.Empty(SettDate2)) ? "" : Convert.ToDateTime(SettDate2).ToString("yyyy/MM/dd");
                xl.dicDatas.Add("##DebiteDate", d1 + "~" + d2);// 8/8/2016 確認EXCEL-A1:Debit No.: 是否打錯
                xl.dicDatas.Add("##ConfirmDate", d3 + "~" + d4);
                xl.dicDatas.Add("##SettledDate", d5 + "~" + d6);
                xl.dicDatas.Add("##SD", dt);
                xl.Save(outpath, false);
            }
            
            return base.OnToExcel(report);
        }

       
    }
}
