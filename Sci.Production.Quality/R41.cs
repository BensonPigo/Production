using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R41 : Sci.Win.Tems.PrintForm
    {
          public R41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.combo_Brand.SelectedIndex = 0;
            print.Enabled = false;
        }
          string Brand;
          string Year;
          DualResult result;
          DataTable dt;
          protected override bool ValidateInput()
          {
              Brand = combo_Brand.Text.ToString();
              Year = combo_Year.Text.ToString();
              return base.ValidateInput();
          }


          protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
          {
              List<SqlParameter> lis = new List<SqlParameter>();
              string sqlWhere = "";  string gb = ""; string ob = "";
              List<string> sqlWheres = new List<string>();

              if (Brand != "")
              {
                  sqlWheres.Add(" b.BrandID =@Brand");
                  lis.Add(new SqlParameter("@Brand", Brand));
              }
              if (Year != "")
              {
                  sqlWheres.Add("year(a.StartDate)=@Year");
                  lis.Add(new SqlParameter("@Year", Year));
              }
              gb = "group by YEAR(A.StartDate),MONTH(A.StartDate), B.DefectMainID,ad.Name, B.DefectSubID,ad2.SubName";
              ob = "order by YEAR,MONTH,QTY DESC";
              sqlWhere = string.Join(" and ", sqlWheres);
              if (!sqlWhere.Empty())
              {
                  sqlWhere = " where " + sqlWhere;
              }
              string sqlcmd = string.Format(@"SELECT YEAR(A.StartDate) AS Year
                                                    ,MONTH(A.StartDate) AS Month
                                                    ,B.DefectMainID
                                                    ,ad.Name 
                                                    ,B.DefectSubID
                                                    ,ad2.SubName
                                                    ,SUM(B.Qty) AS Qty
                                                    ,SUM(B.ValueinUSD) AS CompValue 
                                                    ,row_number() over (partition by YEAR(A.StartDate),MONTH(A.StartDate) 
					                                                    order by SUM(B.Qty) desc) as rnk
                                                    FROM dbo.ADIDASComplain A 
                                                    INNER JOIN dbo.ADIDASComplain_Detail B on b.ID = A.ID
                                                    left join (dbo.ADIDASComplainDefect ad inner join dbo.ADIDASComplainDefect_Detail ad2 
			                                                    on ad2.id = ad.ID)
                                                    on ad2.ID = b.DefectMainID  and ad2.SubID = b.DefectSubID" + " " + sqlWhere+" "+gb+" "+ob);
              result = DBProxy.Current.Select("", sqlcmd, lis, out dt);
              return base.OnAsyncDataLoad(e);
          }

          protected override bool OnToExcel(Win.ReportDefinition report)
          {
              var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
              saveDialog.ShowDialog();
              string outpath = saveDialog.FileName;
              if (outpath.Empty())
              {
                  return false;
              }
              return true;
          }

    }
}
