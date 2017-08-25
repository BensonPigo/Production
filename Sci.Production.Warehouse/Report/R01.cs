using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        //string season, mdivision, factory, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2, style, country, supp;
        string season, mdivision, factory, orderby, spno1, spno2, fabrictype, refno1, refno2, style, country, supp;
        DateTime? sciDelivery1, sciDelivery2, suppDelivery1, suppDelivery2, eta1, eta2, ata1, ata2;
        DataTable printData;

        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            comboFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(comboOrderBy, 1, 1, "Supplier,SP#");
            comboOrderBy.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateSCIDelivery.Value1) && MyUtility.Check.Empty(dateSCIDelivery.Value2) &&
                MyUtility.Check.Empty(dateSuppDelivery.Value1) && MyUtility.Check.Empty(dateSuppDelivery.Value2) &&
                MyUtility.Check.Empty(dateETA.Value1) && MyUtility.Check.Empty(datelabelFinalETA.Value2) &&
                MyUtility.Check.Empty(datelabelFinalETA.Value1) && MyUtility.Check.Empty(dateETA.Value2) &&
                (MyUtility.Check.Empty(txtSPNoStart.Text) && MyUtility.Check.Empty(txtSPNoEnd.Text)) &&
                (MyUtility.Check.Empty(txtRefnoStart.Text) && MyUtility.Check.Empty(txtRefnoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Supp Delivery > & < SCI Delivery > & < ETA > & < Final ETA >& < SP# > & < Refno > can't be empty!!");
                return false;
            }
            #region -- 擇一必輸的條件 --
            sciDelivery1 = dateSCIDelivery.Value1;
            sciDelivery2 = dateSCIDelivery.Value2;
            suppDelivery1 = dateSuppDelivery.Value1;
            suppDelivery2 = dateSuppDelivery.Value2;
            eta1 = dateETA.Value1;
            eta2 = dateETA.Value2;
            ata1 = datelabelFinalETA.Value1;
            ata2 = datelabelFinalETA.Value2;
            spno1 = txtSPNoStart.Text;
            spno2 = txtSPNoEnd.Text;
            refno1 = txtRefnoStart.Text;
            refno2 = txtRefnoEnd.Text;
            #endregion

            country = txtcountry.Text;
            supp = txtsupplier.TextBox1.Text;
            style = txtstyle.Text;
            season = txtseason.Text;
            mdivision = txtMdivision.Text;
            factory = txtfactory.Text;
            fabrictype = comboFabricType.SelectedValue.ToString();
            orderby = comboOrderBy.Text;

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
            if (!MyUtility.Check.Empty(sciDelivery1) || !MyUtility.Check.Empty(sciDelivery2))
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

                if (!MyUtility.Check.Empty(sciDelivery1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= o.SciDelivery ",Convert.ToDateTime(sciDelivery1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(sciDelivery2))
                {
                    sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery2).ToString("d")));
                }
                //if (!MyUtility.Check.Empty(spno1))
                //{
                //    sqlCmd.Append(" and o.id >= @spno1 and o.id <= @spno2");
                //    sp_spno1.Value = spno1;
                //    sp_spno2.Value = spno2;
                //    cmds.Add(sp_spno1);
                //    cmds.Add(sp_spno2);
                //}
                if (!MyUtility.Check.Empty(style))
                {
                    sqlCmd.Append(" and o.styleid = @style");
                    sp_style.Value = style;
                    cmds.Add(sp_style);
                }
                if (!MyUtility.Check.Empty(season))
                {
                    sqlCmd.Append(" and o.seasonid = @season");
                    sp_season.Value = season;
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
            if (!MyUtility.Check.Empty(spno1) && !MyUtility.Check.Empty(spno2))
            {
                //若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and a.id >= @spno1 and a.id <= @spno2");
                sp_spno1.Value = spno1.PadRight(10, '0');
                sp_spno2.Value = spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }else if (!MyUtility.Check.Empty(spno1))
            {
                //只有 sp1 輸入資料
                sqlCmd.Append(" and a.id like @spno1 ");
                sp_spno1.Value = spno1 + "%";
                cmds.Add(sp_spno1);
            }else if (!MyUtility.Check.Empty(spno2))
            {
                //只有 sp2 輸入資料
                sqlCmd.Append(" and a.id like @spno2 ");
                sp_spno2.Value = spno2 + "%";
                cmds.Add(sp_spno2);
            }
            
            if (!MyUtility.Check.Empty(suppDelivery1) || !MyUtility.Check.Empty(suppDelivery2))
            {
                if(!MyUtility.Check.Empty(suppDelivery1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= b.finaletd", Convert.ToDateTime(suppDelivery1).ToString("d")));
                if(!MyUtility.Check.Empty(suppDelivery2))
                    sqlCmd.Append(string.Format(@" and b.finaletd <= '{0}'",Convert.ToDateTime(suppDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(eta1) || !MyUtility.Check.Empty(eta2))
            {
                if (!MyUtility.Check.Empty(eta1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= b.ETA", Convert.ToDateTime(eta1).ToString("d")));
                if (!MyUtility.Check.Empty(eta2))
                    sqlCmd.Append(string.Format(@" and b.ETA <= '{0}'", Convert.ToDateTime(eta2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(ata1) || !MyUtility.Check.Empty(ata2))
            {
                if(!MyUtility.Check.Empty(ata1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= b.FinalETA", Convert.ToDateTime(ata1).ToString("d")));
                if (!MyUtility.Check.Empty(ata2))
                    sqlCmd.Append(string.Format(@" and b.FinalETA <= '{0}'", Convert.ToDateTime(ata2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(country))
            {
                sqlCmd.Append(string.Format(" and c.country = '(0}'",country));
            }

            if (!MyUtility.Check.Empty(supp))
            {
                sqlCmd.Append(string.Format(" and a.suppid = '{0}'",supp));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and factory.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and orders.FactoryID = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(fabrictype))
            {
                sqlCmd.Append(string.Format(@" and b.FabricType = '{0}'", fabrictype));
            }

            if (!MyUtility.Check.Empty(refno1) && !MyUtility.Check.Empty(refno2))
            {
                //Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and b.refno >= @refno1 and b.refno <= @refno2");
                sp_refno1.Value = refno1;
                sp_refno2.Value = refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }else if (!MyUtility.Check.Empty(refno1))
            {
                //只輸入 Refno1
                sqlCmd.Append(" and b.refno like @refno1");
                sp_refno1.Value = refno1 + "%";
                cmds.Add(sp_refno1);
            }else if (!MyUtility.Check.Empty(refno2))
            {
                //只輸入 Refno2
                sqlCmd.Append(" and b.refno like @refno2");
                sp_refno2.Value = refno2 + "%";
                cmds.Add(sp_refno2);
            }

            if (checkIncludeCompleteItem.Checked == false)
            {
                sqlCmd.Append(" and b.complete = 0");
            }

            if (orderby.ToUpper().TrimEnd() == "SUPPLIER")
            {
                sqlCmd.Append(" ORDER BY A.SUPPID,B.ID,B.SEQ1,B.SEQ2 ");
            }
            else
            {
                sqlCmd.Append(" ORDER BY B.ID,B.SEQ1,B.SEQ2 ");
            }

            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
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
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R01.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R01.xltx", 1, showExcel: false, showSaveMsg: false, excelApp : objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            for (int i = 1; i <= printData.Rows.Count; i++) { 
                string str = worksheet.Cells[i + 1, 4].Value;
                if(!MyUtility.Check.Empty(str))
                    worksheet.Cells[i + 1, 4] = str.Trim(); 

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