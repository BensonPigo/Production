using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;

namespace Sci.Production.Warehouse
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;
        public R02(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
            //this.ReportResourceName = "DCLC NAME";
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateRange1.TextBox1.Value))
            {
                MyUtility.Msg.WarningBox("Issue date can't be empty!!");
                return false;
            }
            return base.ValidateInput();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            DualResult result = Result.True;
            SaveFileDialog sfldg = new SaveFileDialog();
            MyUtility.Excel.SaveXlsFile("Warehouse_R02_Scarp_List", out sfldg);
            result = MyUtility.Excel.CopyToXls(dt, sfldg.FileName);
            if (result)
            {
                MyUtility.Excel.XlsAutoFit(sfldg.FileName, "Warehouse_R02.xlt");
                return true;
            }
            else
                return false;
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            //return base.OnAsyncDataLoad(e);
            DualResult result = Result.True;
            
            try
            {
                string sqlcmd =string.Format(@"select a.issuedate, b.poid, b.seq1,b.seq2,'' as description, sum(b.qty) qty, 
(select stockunit from po_supp_detail where id= b.poid and seq1 = b.seq1 and seq2 = b.seq2) as unit
from scrap a inner join scrap_detail b on a.id = b.id
where a.Status = 'Approved' and a.type='A' 
and a.issuedate between '{0}' and '{1}'
group by a.issuedate , b.poid, b.seq1,b.Seq2
",dateRange1.Text1,dateRange1.Text2);
                result = DBProxy.Current.Select(null, sqlcmd, out dt);
                if (!result) return result;
                foreach (DataRow dr in dt.Rows)
                {
                    dr[4] = PublicPrg.Prgs.GetMtlDesc(dr["poid"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString(), 3);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
