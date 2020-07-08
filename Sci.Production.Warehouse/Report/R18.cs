using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R18 : Win.Tems.PrintForm
    {
        DataTable dt;

        private string category;

        public R18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text)
                && MyUtility.Check.Empty(this.txtRefno.Text)
                && MyUtility.Check.Empty(this.txtLocation.Text)
                && MyUtility.Check.Empty(this.dateRangeBuyerDelivery.Value1)
                && MyUtility.Check.Empty(this.dateRangeBuyerDelivery.Value2)
                && MyUtility.Check.Empty(this.dateRangeSciDelivery.Value1)
                && MyUtility.Check.Empty(this.dateRangeSciDelivery.Value2)
                && MyUtility.Check.Empty(this.txtsupplier.TextBox1.Text))
            {
                MyUtility.Msg.WarningBox("SP#, Ref#, Location, Buyer Delivery, SCI Delivery, Supplier can't be empty!!");
                return false;
            }

            this.category = string.Empty;
            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                this.category = this.comboCategory.SelectedValue.ToString();
            }

            return base.ValidateInput();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.dt.Rows.Count);
            DualResult result = Result.True;
            if (this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return result;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R18_Material_Tracking.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Warehouse_R18_Material_Tracking.xltx", 1, showExcel: false, showSaveMsg: true, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            for (int i = 1; i <= this.dt.Rows.Count; i++)
            {
                string str = worksheet.Cells[i + 1, 20].Value;
                if (!MyUtility.Check.Empty(str))
                {
                    worksheet.Cells[i + 1, 20] = str.Trim();
                }
            }

            objApp.Columns.AutoFit();
            objApp.Rows.AutoFit();
            worksheet.Columns[20].ColumnWidth = 88;

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R18_Material_Tracking");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return false;
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            // return base.OnAsyncDataLoad(e);
            string spno = this.txtSPNo.Text.TrimEnd();
            string style = this.txtStyle.Text;
            string refno = this.txtRefno.Text;
            string location = this.txtLocation.Text.TrimEnd();
            string buyerDeliveryStart = this.dateRangeBuyerDelivery.Value1.ToString().EqualString(string.Empty) ? string.Empty : ((DateTime)this.dateRangeBuyerDelivery.Value1).ToString("yyyy/MM/dd");
            string buyerDeliveryEnd = this.dateRangeBuyerDelivery.Value2.ToString().EqualString(string.Empty) ? string.Empty : ((DateTime)this.dateRangeBuyerDelivery.Value2).ToString("yyyy/MM/dd");
            string sciDeliveryStart = this.dateRangeSciDelivery.Value1.ToString().EqualString(string.Empty) ? string.Empty : ((DateTime)this.dateRangeSciDelivery.Value1).ToString("yyyy/MM/dd");
            string sciDeliveryEnd = this.dateRangeSciDelivery.Value2.ToString().EqualString(string.Empty) ? string.Empty : ((DateTime)this.dateRangeSciDelivery.Value2).ToString("yyyy/MM/dd");
            string supplier = this.txtsupplier.TextBox1.Text;
            string colorid = this.txtColor.Text;
            string factoryid = this.txtfactory.Text;
            string sizespec = this.txtSizeCode.Text;
            bool chkbalance = this.checkBalanceQty.Checked;

            DualResult result = Result.True;
            StringBuilder sqlcmd = new StringBuilder();
            #region sql command
            sqlcmd.Append(string.Format(
                @"
with cte as (
    select  isnull(x.FactoryID,y.FactoryID) factoryid
            , pd.id
            , pd.seq1
            , pd.seq2
            , supplier = (select supp.id+'-'+supp.AbbEN 
                          from dbo.supp WITH (NOLOCK) 
                          where po.suppid = supp.id)
            , styleid = isnull(x.StyleID,y.StyleID) 
            , SeasonID = ISNULL(x.SeasonID,y.SeasonID) 
            , brandid = ISNULL(x.BrandID,y.BrandID) 
            , x.Category
            , [ExFactoryDate] = x.SciDelivery
            , pd.ETA
            , pd.FinalETA
            , x.BuyerDelivery
            , x.SciDelivery
            , pd.Complete
            , pd.Refno
            , pd.Width
            , pd.ColorID
            , pd.SizeSpec
            , [description] = isnull(ltrim(rtrim(dbo.getMtlDesc(pd.id,pd.seq1,pd.seq2,2,0))), '') 
            , y.Deadline
            , pd.Qty
            , pd.ShipQty
            , pd.InputQty
            , pd.OutputQty
            , taipeiBalance = pd.InputQty - pd.OutputQty
            , mpd.InQty
            , mpd.OutQty
            , mpd.AdjustQty
            , balanceqty = mpd.InQty-mpd.OutQty-mpd.AdjustQty
            , mpd.ALocation
            , mpd.LInvQty
            , mpd.BLocation
            , mpd.LObQty
            , wkno = ( select t.id+',' 
                       from (select distinct Export.id 
                             from dbo.Export WITH (NOLOCK) 
                             inner join dbo.Export_Detail WITH (NOLOCK) on Export_Detail.id = Export.id  
                             where export.Junk=0 and poid = pd.id and seq1 = pd.seq1 and seq2 = pd.seq2)t 
                       for xml path(''))
            , blno = (select t.Blno+',' 
                      from (select distinct Blno 
                            from dbo.Export WITH (NOLOCK) 
                            inner join dbo.Export_Detail WITH (NOLOCK) on Export_Detail.id = Export.id  
                            where export.Junk=0 and poid = pd.id and seq1 = pd.seq1 and seq2 = pd.seq2 and Blno !='')t 
                      for xml path(''))
    from dbo.PO_Supp_Detail pd WITH (NOLOCK) 
    left join dbo.Po_Supp po with (NoLock) on pd.ID = po.ID
                                               and pd.Seq1 = po.Seq1
    left join dbo.MDivisionPoDetail mpd WITH (NOLOCK) on mpd.POID = pd.id 
                                                          and mpd.seq1 = pd.seq1 
                                                          and mpd.seq2= pd.SEQ2
    outer apply
    (
        select  o.FactoryID
                , o.StyleID
                , o.SeasonID
                , o.BrandID
                , o.Category
                , o.SciDelivery 
                , o.BuyerDelivery
        from dbo.orders o WITH (NOLOCK) 
        inner join factory on o.FactoryID = factory.id
        where o.id = pd.id
    ) x
    outer apply
    (
        select  i.FactoryID
                , i.StyleID
                , i.SeasonID
                , i.BrandID
                , deadline = max(i.Deadline)  
        from dbo.Inventory i WITH (NOLOCK) 
        where i.POID = pd.id 
              and i.Seq1 = pd.seq1 
              and i.seq2 = pd.SEQ2
        group by i.FactoryID,i.StyleID,i.SeasonID,i.BrandID
    ) y", Env.User.Keyword));
            if (!MyUtility.Check.Empty(location))
            {
                sqlcmd.Append(@"
    inner join (
                select  distinct 
                        fi.poid
                        ,fi.seq1
                        ,fi.seq2 
                from dbo.FtyInventory fi WITH (NOLOCK) 
                inner join dbo.FtyInventory_Detail fid WITH (NOLOCK) on fid.Ukey = fi.Ukey 
                where fid.MtlLocationid=''
    ) q on q.POID = pd.ID 
           and q.seq1 = pd.seq1 
           and q.seq2 = pd.SEQ2");
            }

            sqlcmd.Append(string.Format(@" where 1=1 "));

            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append(string.Format(@" and pd.id='{0}'", spno));
            }

            if (!MyUtility.Check.Empty(refno))
            {
                sqlcmd.Append(string.Format(@" and pd.Refno ='{0}'", refno));
            }

            if (MyUtility.Check.Empty(buyerDeliveryStart) == false)
            {
                sqlcmd.Append($" and '{buyerDeliveryStart}' <= x.BuyerDelivery");
            }

            if (MyUtility.Check.Empty(buyerDeliveryEnd) == false)
            {
                sqlcmd.Append($" and x.BuyerDelivery <= '{buyerDeliveryEnd}'");
            }

            if (MyUtility.Check.Empty(sciDeliveryStart) == false)
            {
                sqlcmd.Append($" and '{sciDeliveryStart}' <= x.SciDelivery");
            }

            if (MyUtility.Check.Empty(sciDeliveryEnd) == false)
            {
                sqlcmd.Append($" and x.SciDelivery <= '{sciDeliveryEnd}'");
            }

            if (MyUtility.Check.Empty(supplier) == false)
            {
                sqlcmd.Append($" and po.SuppID = '{supplier}'");
            }

            if (!MyUtility.Check.Empty(colorid))
            {
                sqlcmd.Append(string.Format(@" and pd.ColorID ='{0}'", colorid));
            }

            if (!MyUtility.Check.Empty(sizespec))
            {
                sqlcmd.Append(string.Format(@" and pd.SizeSpec = '{0}'", sizespec));
            }

            sqlcmd.Append(@"
) select * from cte where 1= 1 ");
            if (chkbalance)
            {
                sqlcmd.Append(" and balanceqty > 0");
            }

            if (!MyUtility.Check.Empty(this.category))
            {
                sqlcmd.Append($"and Category in ({this.category})");
            }

            if (!MyUtility.Check.Empty(style))
            {
                sqlcmd.Append(string.Format(@" and StyleID ='{0}'", style));
            }

            if (!MyUtility.Check.Empty(factoryid))
            {
                sqlcmd.Append(string.Format(@" and FactoryID ='{0}'", factoryid));
            }

            #endregion

            try
            {
                DBProxy.Current.DefaultTimeout = 600;
                result = DBProxy.Current.Select(null, sqlcmd.ToString(), out this.dt);
                DBProxy.Current.DefaultTimeout = 30;
                if (!result)
                {
                    return result;
                }
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
