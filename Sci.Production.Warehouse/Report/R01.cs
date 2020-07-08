using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string season;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string mdivision;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string factory;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string orderby;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string spno1;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string spno2;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string fabrictype;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string refno1;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string refno2;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string style;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string country;

        // string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string supp;
        DateTime? sciDelivery1;
        DateTime? sciDelivery2;
        DateTime? suppDelivery1;
        DateTime? suppDelivery2;
        DateTime? eta1;
        DateTime? eta2;
        DateTime? ata1;
        DateTime? ata2;
        DataTable printData;

        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            this.comboFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Supplier,SP#");
            this.comboOrderBy.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateSuppDelivery.Value1) && MyUtility.Check.Empty(this.dateSuppDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateETA.Value1) && MyUtility.Check.Empty(this.datelabelFinalETA.Value2) &&
                MyUtility.Check.Empty(this.datelabelFinalETA.Value1) && MyUtility.Check.Empty(this.dateETA.Value2) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) && MyUtility.Check.Empty(this.txtSPNoEnd.Text)) &&
                (MyUtility.Check.Empty(this.txtRefnoStart.Text) && MyUtility.Check.Empty(this.txtRefnoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Supp Delivery > & < SCI Delivery > & < ETA > & < Final ETA >& < SP# > & < Refno > can't be empty!!");
                return false;
            }
            #region -- 擇一必輸的條件 --
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.suppDelivery1 = this.dateSuppDelivery.Value1;
            this.suppDelivery2 = this.dateSuppDelivery.Value2;
            this.eta1 = this.dateETA.Value1;
            this.eta2 = this.dateETA.Value2;
            this.ata1 = this.datelabelFinalETA.Value1;
            this.ata2 = this.datelabelFinalETA.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            this.refno1 = this.txtRefnoStart.Text;
            this.refno2 = this.txtRefnoEnd.Text;
            #endregion

            this.country = this.txtcountry.Text;
            this.supp = this.txtsupplier.TextBox1.Text;
            this.style = this.txtstyle.Text;
            this.season = this.txtseason.Text;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.fabrictype = this.comboFabricType.SelectedValue.ToString();
            this.orderby = this.comboOrderBy.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --
            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            System.Data.SqlClient.SqlParameter sp_season = new System.Data.SqlClient.SqlParameter();
            sp_season.ParameterName = "@season";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_refno1 = new System.Data.SqlClient.SqlParameter();
            sp_refno1.ParameterName = "@refno1";

            System.Data.SqlClient.SqlParameter sp_refno2 = new System.Data.SqlClient.SqlParameter();
            sp_refno2.ParameterName = "@refno2";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            if (!MyUtility.Check.Empty(this.sciDelivery1) || !MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(@"
;with cte as(
    select  distinct 
            o.mdivisionid
            ,o.POID
            ,o.StyleID
            ,o.SeasonID 
    from dbo.orders o WITH (NOLOCK) 
    where 1=1"));

                if (!MyUtility.Check.Empty(this.sciDelivery1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= o.SciDelivery ", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.sciDelivery2))
                {
                    sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
                }

                // if (!MyUtility.Check.Empty(spno1))
                // {
                //    sqlCmd.Append(" and o.id >= @spno1 and o.id <= @spno2");
                //    sp_spno1.Value = spno1;
                //    sp_spno2.Value = spno2;
                //    cmds.Add(sp_spno1);
                //    cmds.Add(sp_spno2);
                // }
                if (!MyUtility.Check.Empty(this.style))
                {
                    sqlCmd.Append(" and o.styleid = @style");
                    sp_style.Value = this.style;
                    cmds.Add(sp_style);
                }

                if (!MyUtility.Check.Empty(this.season))
                {
                    sqlCmd.Append(" and o.seasonid = @season");
                    sp_season.Value = this.season;
                    cmds.Add(sp_season);
                }

                sqlCmd.Append(")");
                sqlCmd.Append(string.Format(@"
select --isnull(d.mdivisionid,cte.mdivisionid)
        orders.FactoryID
        ,sp = a.id
        ,seq = concat(b.SEQ1, b.Seq2)
        --,b.SEQ2
        ,supp = a.suppid+'-'+c.AbbEN
        ,description = dbo.getMtlDesc(b.id,b.seq1,b.seq2,2,0) 
        ,MaterialType = case b.fabrictype when 'F' then 'Fabric'
                                          when 'A' then 'Accessory'
                                          else 'Other'
                        end
        ,OrderQty = b.Qty
        ,ShipQty = isnull(b.ShipQty, 0) + isnull(b.ShipFOC, 0)
        --,b.ShipFOC
        ,PoUnit = b.POUnit
        ,complete = iif(b.Complete=1,'Y','N') 
        --,b.Final ETD
        ,EstETA = b.FinalETA
        --,b.FinalETA
        ,ArrivedQty = d.InQty
        ,ReleasedQty = d.OutQty
        ,AdjustQty = d.AdjustQty
        ,Balance = d.InQty - d.OutQty + d.AdjustQty
        ,StockUnit = b.StockUnit
from cte 
inner join dbo.PO_Supp a WITH (NOLOCK) on a.id = cte.poid
inner join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id = a.id and b.SEQ1 = a.SEQ1
inner join Orders orders on b.id = orders.id
inner join Factory factory on orders.FactoryID = factory.id
inner join dbo.Supp c WITH (NOLOCK) on c.id = a.SuppID
left join dbo.MDivisionPoDetail d WITH (NOLOCK) on d.POID = b.ID and d.seq1 = b.seq1 and d.seq2 = b.SEQ2
where 1= 1 and c.ThirdCountry = 1"));
            }
            else
            {
                sqlCmd.Append(string.Format(@"
select --isnull(d.mdivisionid,(select orders.mdivisionid from dbo.orders WITH (NOLOCK) where id = a.id))
        orders.FactoryID
        ,sp = a.id
        ,seq = concat(b.SEQ1, ' - ', b.Seq2)
        --,b.SEQ2
        ,supp = a.suppid+'-'+c.AbbEN
        ,description = dbo.getMtlDesc(b.id,b.seq1,b.seq2,2,0) 
        ,MaterialType = case b.fabrictype when 'F' then 'Fabric'
                                          when 'A' then 'Accessory'
                                          else 'Other'
                        end
        ,OrderQty = b.Qty
        ,ShipQty = isnull(b.ShipQty, 0) + isnull(b.ShipFOC, 0)
        --,b.ShipFOC
        ,PoUnit = b.POUnit
        ,complete = iif(b.Complete=1,'Y','N') 
        --,b.Final ETD
        ,EstETA = b.FinalETA
        --,b.FinalETA
        ,ArrivedQty = d.InQty
        ,ReleasedQty = d.OutQty
        ,AdjustQty = d.AdjustQty
        ,Balance = d.InQty - d.OutQty + d.AdjustQty
        ,StockUnit = b.StockUnit
from dbo.PO_Supp a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id = a.id and b.SEQ1 = a.SEQ1
inner join Orders orders on b.id = orders.id
inner join Factory factory on orders.FactoryID = factory.id
inner join dbo.Supp c WITH (NOLOCK) on c.id = a.SuppID
left join dbo.MDivisionPoDetail d WITH (NOLOCK) on d.POID = b.ID and d.seq1 = b.seq1 and d.seq2 = b.SEQ2
where 1=1 and c.ThirdCountry = 1"));
            }

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.spno1) && !MyUtility.Check.Empty(this.spno2))
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and a.id >= @spno1 and a.id <= @spno2");
                sp_spno1.Value = this.spno1.PadRight(10, '0');
                sp_spno2.Value = this.spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(this.spno1))
            {
                // 只有 sp1 輸入資料
                sqlCmd.Append(" and a.id like @spno1 ");
                sp_spno1.Value = this.spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(this.spno2))
            {
                // 只有 sp2 輸入資料
                sqlCmd.Append(" and a.id like @spno2 ");
                sp_spno2.Value = this.spno2 + "%";
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(this.suppDelivery1) || !MyUtility.Check.Empty(this.suppDelivery2))
            {
                if (!MyUtility.Check.Empty(this.suppDelivery1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= b.finaletd", Convert.ToDateTime(this.suppDelivery1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.suppDelivery2))
                {
                    sqlCmd.Append(string.Format(@" and b.finaletd <= '{0}'", Convert.ToDateTime(this.suppDelivery2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.eta1) || !MyUtility.Check.Empty(this.eta2))
            {
                if (!MyUtility.Check.Empty(this.eta1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= b.ETA", Convert.ToDateTime(this.eta1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.eta2))
                {
                    sqlCmd.Append(string.Format(@" and b.ETA <= '{0}'", Convert.ToDateTime(this.eta2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.ata1) || !MyUtility.Check.Empty(this.ata2))
            {
                if (!MyUtility.Check.Empty(this.ata1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= b.FinalETA", Convert.ToDateTime(this.ata1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.ata2))
                {
                    sqlCmd.Append(string.Format(@" and b.FinalETA <= '{0}'", Convert.ToDateTime(this.ata2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.country))
            {
                sqlCmd.Append(string.Format(" and c.country = '(0}'", this.country));
            }

            if (!MyUtility.Check.Empty(this.supp))
            {
                sqlCmd.Append(string.Format(" and a.suppid = '{0}'", this.supp));
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and factory.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and orders.FactoryID = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.fabrictype))
            {
                sqlCmd.Append(string.Format(@" and b.FabricType = '{0}'", this.fabrictype));
            }

            if (!MyUtility.Check.Empty(this.refno1) && !MyUtility.Check.Empty(this.refno2))
            {
                // Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and b.refno >= @refno1 and b.refno <= @refno2");
                sp_refno1.Value = this.refno1;
                sp_refno2.Value = this.refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }
            else if (!MyUtility.Check.Empty(this.refno1))
            {
                // 只輸入 Refno1
                sqlCmd.Append(" and b.refno like @refno1");
                sp_refno1.Value = this.refno1 + "%";
                cmds.Add(sp_refno1);
            }
            else if (!MyUtility.Check.Empty(this.refno2))
            {
                // 只輸入 Refno2
                sqlCmd.Append(" and b.refno like @refno2");
                sp_refno2.Value = this.refno2 + "%";
                cmds.Add(sp_refno2);
            }

            if (this.checkIncludeCompleteItem.Checked == false)
            {
                sqlCmd.Append(" and b.complete = 0");
            }

            if (this.orderby.ToUpper().TrimEnd() == "SUPPLIER")
            {
                sqlCmd.Append(" ORDER BY A.SUPPID,B.ID,B.SEQ1,B.SEQ2 ");
            }
            else
            {
                sqlCmd.Append(" ORDER BY B.ID,B.SEQ1,B.SEQ2 ");
            }

            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R01.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R01.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            for (int i = 1; i <= this.printData.Rows.Count; i++)
            {
                string str = worksheet.Cells[i + 1, 4].Value;
                if (!MyUtility.Check.Empty(str))
                {
                    worksheet.Cells[i + 1, 4] = str.Trim();
                }
            }

            worksheet.Columns[4].ColumnWidth = 50;
            worksheet.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R01");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}