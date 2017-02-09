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
using System.Runtime.InteropServices;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R18 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;

        int selectindex = 0;
        public R18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
            cbxCategory.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(tbxSP.Text) &&
                MyUtility.Check.Empty(tbxRefno.Text) &&
                MyUtility.Check.Empty(tbxLocation.Text))
            {
                MyUtility.Msg.WarningBox("SP#, Ref#, Location can't be empty!!");
                return false;
            }
            selectindex = cbxCategory.SelectedIndex;
            return base.ValidateInput();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            SetCount(dt.Rows.Count);
            DualResult result = Result.True;
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return result;
            }


            
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R18_Material_Tracking.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(dt, "", "Warehouse_R18_Material_Tracking.xltx", 1, showExcel: false, showSaveMsg: true, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet= objApp.Sheets[1];
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                string str = worksheet.Cells[i + 1, 18].Value;
                if(!MyUtility.Check.Empty(str))
                    worksheet.Cells[i + 1, 18] = str.Trim();
            }

            worksheet.Columns[18].ColumnWidth = 88;
            objApp.Visible = true;

            objApp.Columns.AutoFit();
            objApp.Rows.AutoFit();                     


            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放worksheet
            this.HideWaitMessage();
            return false;

        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            //return base.OnAsyncDataLoad(e);
            String spno = tbxSP.Text.TrimEnd();
            String style = tbxStyle.Text;
            String refno = tbxRefno.Text;
            String location = tbxLocation.Text.TrimEnd();
            String colorid = tbxColor.Text;
            String factoryid = txtmfactory1.Text;
            String sizespec = tbxSizeCode.Text;

            bool chkbalance = checkBox1.Checked;
            

            DualResult result = Result.True;
            StringBuilder sqlcmd = new StringBuilder();
            #region sql command
            sqlcmd.Append(string.Format(@"with cte as 
(
select 
isnull(x.FactoryID,y.FactoryID) factoryid,
pd.id,
pd.seq1,
pd.seq2,
(select supp.id+'-'+supp.AbbEN from dbo.supp inner join dbo.po_supp on po_supp.suppid = supp.id where po_supp.id = pd.id and seq1 = pd.seq1) as supplier,
isnull(x.StyleID,y.StyleID) styleid,
ISNULL(x.SeasonID,y.SeasonID) SeasonID,
ISNULL(x.BrandID,y.BrandID) brandid,
x.Category,
x.SciDelivery,
pd.ETA,
pd.FinalETA,
pd.Complete,
pd.Refno,
pd.Width,
pd.ColorID,
pd.SizeSpec,
isnull(ltrim(rtrim(dbo.getMtlDesc(pd.id,pd.seq1,pd.seq2,2,0))), '') [description],
y.Deadline,
pd.Qty,
pd.ShipQty,
pd.InputQty,
pd.OutputQty,
pd.InputQty - pd.OutputQty as taipeiBalance,
mpd.InQty,
mpd.OutQty,
mpd.AdjustQty,
mpd.InQty-mpd.OutQty-mpd.AdjustQty as balanceqty,
mpd.ALocation,
mpd.LInvQty,
mpd.BLocation,
mpd.LObQty
,( select t.id+',' from (select distinct Export.id from dbo.Export inner join dbo.Export_Detail on Export_Detail.id = Export.id  where export.Junk=0 and poid = pd.id and seq1 = pd.seq1 and seq2 = pd.seq2)t for xml path('')) as wkno
,( select t.Blno+',' from (select distinct Blno from dbo.Export inner join dbo.Export_Detail on Export_Detail.id = Export.id  where export.Junk=0 and poid = pd.id and seq1 = pd.seq1 and seq2 = pd.seq2 and Blno !='')t for xml path('')) as blno
from dbo.PO_Supp_Detail pd
inner join dbo.MDivisionPoDetail mpd on mpd.POID = pd.id and mpd.seq1 = pd.seq1 
and mpd.seq2= pd.SEQ2 and mpd.MDivisionID ='{0}'
outer apply
(
	select o.FactoryID,o.StyleID,o.SeasonID,o.BrandID,o.Category,o.SciDelivery from dbo.orders o where o.id = pd.id
) x
outer apply
(
	select i.FactoryID,i.StyleID,i.SeasonID,i.BrandID,max(i.Deadline) deadline from dbo.Inventory i 
    where i.POID = pd.id and i.Seq1= pd.seq1 and i.seq2 = pd.SEQ2
	group by i.FactoryID,i.StyleID,i.SeasonID,i.BrandID
) y", Env.User.Keyword));
            if (!MyUtility.Check.Empty(location))
            {
                sqlcmd.Append(@"
inner join (select distinct fi.MDivisionid,fi.poid,fi.seq1,fi.seq2 from dbo.FtyInventory fi 
			inner join dbo.FtyInventory_Detail fid on fid.Ukey = fi.Ukey 
			where fid.MtlLocationid='') q
on q.MDivisionID = mpd.MDivisionID and q.POID = pd.ID and q.seq1 = pd.seq1 and q.seq2 = pd.SEQ2");
            }

            sqlcmd.Append(string.Format(@" where 1=1 and mpd.MDivisionID='{0}'", Env.User.Keyword));

            if (!MyUtility.Check.Empty(spno)) sqlcmd.Append(string.Format(@" and pd.id='{0}'", spno));
            if (!MyUtility.Check.Empty(refno)) sqlcmd.Append(string.Format(@" and pd.Refno ='{0}'", refno));
            if (!MyUtility.Check.Empty(colorid)) sqlcmd.Append(string.Format(@" and pd.ColorID ='{0}'", colorid));
            if (!MyUtility.Check.Empty(sizespec)) sqlcmd.Append(string.Format(@" and pd.SizeSpec = '{0}'", sizespec));
            
            sqlcmd.Append(@") select * from cte where 1= 1 ");
            if (chkbalance) sqlcmd.Append(" and balanceqty > 0");
            switch (selectindex)
            {
                case 0:
                    sqlcmd.Append(@" and (Category = 'B' or Category = 'S')");
                    break;
                case 1:
                    sqlcmd.Append(@" and Category = 'B' ");
                    break;
                case 2:
                    sqlcmd.Append(@" and (Category = 'S')");
                    break;
                case 3:
                    sqlcmd.Append(@" and (Category = 'M' )");
                    break;
            }

            if (!MyUtility.Check.Empty(style)) sqlcmd.Append(string.Format(@" and StyleID ='{0}'", style));
            if (!MyUtility.Check.Empty(factoryid)) sqlcmd.Append(string.Format(@" and FactoryID ='{0}'", factoryid));

            #endregion

            try
            {
                DBProxy.Current.DefaultTimeout = 600;
                result = DBProxy.Current.Select(null, sqlcmd.ToString(), out dt);
                DBProxy.Current.DefaultTimeout = 30;
                if (!result) return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private void toexcel_Click(object sender, EventArgs e)
        {

        }

        private void R18_Load(object sender, EventArgs e)
        {

        }
    }
}
