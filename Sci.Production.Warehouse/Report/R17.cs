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
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R17 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;

        int selectindex = 0;
        public R17(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
            comboStockType.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) 
                && MyUtility.Check.Empty(dateSCIDelivery.Value2) 
                && MyUtility.Check.Empty(txtSPNo.Text)
                && MyUtility.Check.Empty(dateETA.TextBox1.Value)
                && MyUtility.Check.Empty(dateETA.TextBox2.Value)
                && MyUtility.Check.Empty(txtMtlLocationStart.Text) 
                && MyUtility.Check.Empty(txtLocationEnd.Text))
            {
                MyUtility.Msg.WarningBox("SP#, SCI Delivery, ETA, Location can't be empty!!");
                return false;
            }
            selectindex = comboStockType.SelectedIndex;
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

            //MyUtility.Excel.CopyToXls(dt,"","Warehouse_R17_Location_List.xltx");
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R17_Location_List.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(dt, "", "Warehouse_R17_Location_List.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            this.ShowWaitMessage("Excel Processing...");
            
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                if (!((string)((Excel.Range)objSheets.Cells[i + 1, 12]).Value).Empty())
                    objSheets.Cells[i + 1, 12] = ((string)((Excel.Range)objSheets.Cells[i + 1, 12]).Value).Trim();
            }
            objSheets.Columns[12].ColumnWidth = 50;
            objSheets.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R17_Location_List");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return false;
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            //return base.OnAsyncDataLoad(e);
            string spno = txtSPNo.Text.TrimEnd();
            string locationStart = txtMtlLocationStart.Text;
            string locationEnd = txtLocationEnd.Text;          
            string factory = txtfactory.Text;
            bool chkbalance = checkBalanceQty.Checked;
            string locationFilte = "";
            string eta1 = string.Empty;
            string eta2 = string.Empty;
            if (!MyUtility.Check.Empty(dateETA.TextBox1.Value))
            {
                eta1 = dateETA.TextBox1.Text;
            }

            if (!MyUtility.Check.Empty(dateETA.TextBox2.Value))
            {
                eta2 = dateETA.TextBox2.Text;
            }


            if (locationStart.Empty() == false && locationEnd.Empty() == false)
            {
                locationFilte = string.Format("b.mtllocationid between '{0}' and '{1}'", locationStart, locationEnd);
            } else if (locationStart.Empty() == true && locationEnd.Empty() == false)
            {
                locationFilte = locationFilte = string.Format("b.mtllocationid < '{0}'", locationEnd);
            } else if (locationStart.Empty() == false && locationEnd.Empty() == true)
            {
                locationFilte = locationFilte = string.Format("'{0}' < b.mtllocationid", locationStart);
            }

                DualResult result = Result.True;
            StringBuilder sqlcmd = new StringBuilder();
            #region sql command
            if (MyUtility.Check.Empty(dateSCIDelivery.Value1) && MyUtility.Check.Empty(dateSCIDelivery.Value2))    // SCI Delivery empty
            {
                if (MyUtility.Check.Empty(locationStart) && MyUtility.Check.Empty(locationEnd)) // Location empty
                {
                    sqlcmd.Append(@"
select distinct 
        Factory		= orders.Factoryid ,
        sp			= a.Poid,
        seq1		= a.seq1,
        seq2		= a.seq2,
        Refno		= p.Refno,
        p.eta,
        MaterialType = case when p.FabricType = 'F'then 'Fabric' 
							when p.FabricType = 'A'then 'Accessory' else 'All'end,
        location	= stuff((select ',' + cast(MtlLocationID as varchar) from (select MtlLocationID from FtyInventory_Detail WITH (NOLOCK) where ukey = a.ukey) t for xml path('')), 1, 1, ''),
        width		= p.Width,
        color		= p.ColorID,
        size		= p.SizeSpec,
        description	= dbo.getMtlDesc(A.Poid,A.SEQ1,A.SEQ2,2,0) ,
        roll		= a.Roll,
        dyelot		= a.Dyelot,
        sotckType	= case a.StockType
                        when 'b' then 'Bulk'
                        when 'i' then 'Inventory'
                        when 'o' then 'Scrap'
                      end,
        deadline	= (select max(Deadline) from dbo.Inventory i WITH (NOLOCK) 
				        where i.POID=a.Poid and i.seq1 =a.Seq1 and i.Seq2 =a.Seq2 and i.FactoryID = (select orders.Factoryid from orders WITH (NOLOCK) where orders.id = a.poid)),
        InQty		= a.InQty,
        OutQty		= a.OutQty,
        AdjustQty	= a.AdjustQty,
        Balance		= isnull(a.inqty, 0) - isnull(a.outqty, 0) + isnull(a.adjustqty, 0)
from dbo.FtyInventory a WITH (NOLOCK) 
inner join Orders on orders.id = a.poid
left join dbo.FtyInventory_Detail b WITH (NOLOCK) on a.Ukey = b.Ukey
inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id = a.Poid and p.seq1 = a.seq1 and p.seq2 = a.seq2
where   1=1");
                    if (!MyUtility.Check.Empty(spno)) 
                        sqlcmd.Append(string.Format(@"
        And a.Poid like '{0}%'", spno));

                    if (!txtSeq.checkSeq1Empty())
                    {
                        sqlcmd.Append(string.Format(@"
        and a.seq1 = '{0}'", txtSeq.seq1));
                    }
                    if (!txtSeq.checkSeq2Empty())
                    {
                        sqlcmd.Append(string.Format(@" 
        and a.seq2 = '{0}'", txtSeq.seq2));
                    }

                    if (chkbalance) 
                        sqlcmd.Append(@" 
        And a.inqty- a.outqty + a.adjustqty > 0");

                    if (!MyUtility.Check.Empty(factory))
                        sqlcmd.Append(string.Format(@" 
        and orders.FactoryID = '{0}'", factory));

                    if (!MyUtility.Check.Empty(eta1))
                    {
                        sqlcmd.Append(string.Format(@" 
        and p.ETA >= '{0}'", eta1));
                    }

                    if (!MyUtility.Check.Empty(eta2))
                    {
                        sqlcmd.Append(string.Format(@" 
        and p.ETA <= '{0}'", eta2));
                    }

                    switch (selectindex)
                    {
                        case 0:
                            sqlcmd.Append(@" 
        And (a.stocktype = 'B' or a.stocktype = 'I')");
                            break;
                        case 1:
                            sqlcmd.Append(@" 
        And a.stocktype = 'B'");
                            break;
                        case 2:
                            sqlcmd.Append(@" 
        And a.stocktype = 'I'");
                            break;
                    }
                }
                else
                {

                    sqlcmd.Append(string.Format(@"
select distinct 
        Factory		= orders.Factoryid,
        sp			= a.Poid,
        seq1		= a.seq1,
        seq2		= a.seq2,
        Refno		= p.Refno,
        p.eta,
        MaterialType = case when p.FabricType = 'F'then 'Fabric' 
							when p.FabricType = 'A'then 'Accessory' else 'All'end,
        location	= stuff((select ',' + cast(MtlLocationID as varchar) from (select MtlLocationID from FtyInventory_Detail WITH (NOLOCK) where ukey = a.ukey) t for xml path('')), 1, 1, ''),
        width		= p.Width,
        color		= p.ColorID,
        size		= p.SizeSpec,
        description	= dbo.getMtlDesc(A.Poid,A.SEQ1,A.SEQ2,2,0) ,
        roll		= a.Roll,
        dyelot		= a.Dyelot,
        sotckType	= case a.StockType
                        when 'b' then 'Bulk'
                        when 'i' then 'Inventory'
                        when 'o' then 'Scrap'
                      end,
        deadline	= (select max(Deadline) from dbo.Inventory i WITH (NOLOCK) 
				        where i.POID=a.Poid and i.seq1 =a.Seq1 and i.Seq2 =a.Seq2 and i.FactoryID = (select FactoryID from orders WITH (NOLOCK) where id = a.Poid)),
        InQty		= a.InQty,
        OutQty		= a.OutQty,
        AdjustQty	= a.AdjustQty,
        Balance		= isnull(a.inqty, 0) - isnull(a.outqty, 0) + isnull(a.adjustqty, 0)
from dbo.FtyInventory a WITH (NOLOCK) 
inner join Orders on orders.id = a.poid
left join dbo.FtyInventory_Detail b WITH (NOLOCK) on a.Ukey = b.Ukey
inner join dbo.PO_Supp_Detail p on p.id = a.Poid and p.seq1 = a.seq1 and p.seq2 = a.seq2
where   1=1 
        And {0} ", locationFilte));
                    if (!MyUtility.Check.Empty(spno)) 
                        sqlcmd.Append(string.Format(@" 
        And a.Poid like '{0}%'", spno));

                    if (!txtSeq.checkSeq1Empty())
                    {
                        sqlcmd.Append(string.Format(@"
        and a.seq1 = '{0}'", txtSeq.seq1));
                    }
                    if (!txtSeq.checkSeq2Empty())
                    {
                        sqlcmd.Append(string.Format(@" 
        and a.seq2 = '{0}'", txtSeq.seq2));
                    }

                    if (chkbalance) 
                        sqlcmd.Append(@" 
        And a.inqty- a.outqty + a.adjustqty > 0");

                    if(!MyUtility.Check.Empty(factory))
                        sqlcmd.Append(string.Format(@" 
        and orders.FactoryID = '{0}'", factory));

                    if (!MyUtility.Check.Empty(eta1))
                    {
                        sqlcmd.Append(string.Format(@" 
        and p.ETA >= '{0}'", eta1));
                    }

                    if (!MyUtility.Check.Empty(eta2))
                    {
                        sqlcmd.Append(string.Format(@" 
        and p.ETA <= '{0}'", eta2));
                    }

                    switch (selectindex)
                    {
                        case 0:
                            sqlcmd.Append(@" 
        And (a.stocktype = 'B' or a.stocktype = 'I')");
                            break;
                        case 1:
                            sqlcmd.Append(@" 
        And a.stocktype = 'B'");
                            break;
                        case 2:
                            sqlcmd.Append(@" 
        And a.stocktype = 'I'");
                            break;
                    }
                }
            }
            else
            {// 有下sci delivery 條件
                if (MyUtility.Check.Empty(locationStart) && MyUtility.Check.Empty(locationEnd))
                {
                    sqlcmd.Append(string.Format(@"
select distinct 
        Factory		= orders.factoryid,
        sp			= a.Poid,
        seq1		= a.seq1,
        seq2		= a.seq2,
        Refno		= p.Refno,
        p.eta,
        MaterialType = case when p.FabricType = 'F'then 'Fabric' 
							when p.FabricType = 'A'then 'Accessory' else 'All'end,
        location	= stuff((select ',' + cast(MtlLocationID as varchar) from (select MtlLocationID from FtyInventory_Detail WITH (NOLOCK) where ukey = a.ukey) t for xml path('')), 1, 1, ''),
        width		= p.Width,
        color		= p.ColorID,
        size		= p.SizeSpec,
        description	= dbo.getMtlDesc(A.Poid,A.SEQ1,A.SEQ2,2,0),
        roll		= a.Roll,
        dyelot		= a.Dyelot,
        sotckType	= case a.StockType
                        when 'b' then 'Bulk'
                        when 'i' then 'Inventory'
                        when 'o' then 'Scrap'
                      end,
        deadline	= (select max(Deadline) from dbo.Inventory i WITH (NOLOCK) 
				        where i.POID=a.Poid and i.seq1 =a.Seq1 and i.Seq2 =a.Seq2 and i.FactoryID = orders.Factoryid),
        InQty		= a.InQty,
        OutQty		= a.OutQty,
        AdjustQty	= a.AdjustQty,
        Balance		= isnull(a.inqty, 0) - isnull(a.outqty, 0) + isnull(a.adjustqty, 0)
from dbo.FtyInventory a WITH (NOLOCK) 
left join dbo.FtyInventory_Detail b WITH (NOLOCK) on a.Ukey = b.Ukey
inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id = a.Poid and p.seq1 = a.seq1 and p.seq2 = a.seq2
inner join dbo.orders WITH (NOLOCK) on orders.id = p.id
where   1=1"));

                    if (!MyUtility.Check.Empty(dateSCIDelivery.Value1))
                        sqlcmd.Append(string.Format(@" 
        and '{0}' <= orders.scidelivery", Convert.ToDateTime(dateSCIDelivery.Value1).ToString("d")));
                    if (!MyUtility.Check.Empty(dateSCIDelivery.Value2))
                        sqlcmd.Append(string.Format(@" 
        and orders.scidelivery <= '{0}'", Convert.ToDateTime(dateSCIDelivery.Value2).ToString("d")));

                    if (!MyUtility.Check.Empty(spno)) 
                        sqlcmd.Append(string.Format(@" 
        And a.Poid like '{0}%'", spno));

                    if (!txtSeq.checkSeq1Empty())
                    {
                        sqlcmd.Append(string.Format(@"
        and a.seq1 = '{0}'", txtSeq.seq1));
                    }
                    if (!txtSeq.checkSeq2Empty()) 
                        sqlcmd.Append(string.Format(@" 
        and a.seq2 = '{0}'", txtSeq.seq2));

                    if (chkbalance) 
                        sqlcmd.Append(@" 
        And a.inqty- a.outqty + a.adjustqty > 0");

                    if (!MyUtility.Check.Empty(factory))
                        sqlcmd.Append(string.Format(@" 
        and orders.FactoryID = '{0}'", factory));

                    if (!MyUtility.Check.Empty(eta1))
                    {
                        sqlcmd.Append(string.Format(@" 
        and p.ETA >= '{0}'", eta1));
                    }

                    if (!MyUtility.Check.Empty(eta2))
                    {
                        sqlcmd.Append(string.Format(@" 
        and p.ETA <= '{0}'", eta2));
                    }

                    switch (selectindex)
                    {
                        case 0:
                            sqlcmd.Append(@" 
        And (a.stocktype = 'B' or a.stocktype = 'I')");
                            break;
                        case 1:
                            sqlcmd.Append(@" 
        And a.stocktype = 'B'");
                            break;
                        case 2:
                            sqlcmd.Append(@" 
        And a.stocktype = 'I'");
                            break;
                    }
                }
                else
                {
                    sqlcmd.Append(string.Format(@"
select distinct
        Factory		= orders.factoryid,
        sp			= a.Poid,
        seq1		= a.seq1,
        seq2		= a.seq2,
        Refno		= p.Refno,
        p.eta,
        MaterialType = case when p.FabricType = 'F'then 'Fabric' 
							when p.FabricType = 'A'then 'Accessory' else 'All'end,
        location	= stuff((select ',' + cast(MtlLocationID as varchar) from (select MtlLocationID from FtyInventory_Detail WITH (NOLOCK) where ukey = a.ukey) t for xml path('')), 1, 1, ''),
        width		= p.Width,
        color		= p.ColorID,
        size		= p.SizeSpec,
        description	= dbo.getMtlDesc(A.Poid,A.SEQ1,A.SEQ2,2,0),
        roll		= a.Roll,
        dyelot		= a.Dyelot,
        sotckType	= case a.StockType
                        when 'b' then 'Bulk'
                        when 'i' then 'Inventory'
                        when 'o' then 'Scrap'
                      end,
        deadline	= (select max(Deadline) from dbo.Inventory i WITH (NOLOCK) 
				        where i.POID=a.Poid and i.seq1 =a.Seq1 and i.Seq2 =a.Seq2 and i.FactoryID = orders.Factoryid),
        InQty		= a.InQty,
        OutQty		= a.OutQty,
        AdjustQty	= a.AdjustQty,
        Balance		= isnull(a.inqty, 0) - isnull(a.outqty, 0) + isnull(a.adjustqty, 0)
from dbo.FtyInventory a WITH (NOLOCK) 
left join dbo.FtyInventory_Detail b WITH (NOLOCK) on a.Ukey = b.Ukey
inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id = a.Poid and p.seq1 = a.seq1 and p.seq2 = a.seq2
inner join dbo.orders WITH (NOLOCK) on orders.ID = p.ID
where   1=1
        And {0} ", locationFilte));

                    if (!MyUtility.Check.Empty(dateSCIDelivery.Value1))
                        sqlcmd.Append(string.Format(@" 
        and '{0}' <= orders.scidelivery", Convert.ToDateTime(dateSCIDelivery.Value1).ToString("d")));
                    if (!MyUtility.Check.Empty(dateSCIDelivery.Value2))
                        sqlcmd.Append(string.Format(@" 
        and orders.scidelivery <= '{0}'", Convert.ToDateTime(dateSCIDelivery.Value2).ToString("d")));

                    if (!MyUtility.Check.Empty(spno)) 
                        sqlcmd.Append(string.Format(@" 
        And a.Poid like '{0}%'", spno));

                    if (!txtSeq.checkSeq1Empty())
                    {
                        sqlcmd.Append(string.Format(@"
        and a.seq1 = '{0}'", txtSeq.seq1));
                    }
                    if (!txtSeq.checkSeq2Empty()) 
                        sqlcmd.Append(string.Format(@" 
        and a.seq2 = '{0}'", txtSeq.seq2));

                    if (chkbalance) 
                        sqlcmd.Append(@" And a.inqty- a.outqty + a.adjustqty > 0");

                    if (!MyUtility.Check.Empty(factory))
                        sqlcmd.Append(string.Format(@" and orders.FactoryId = '{0}'", factory));

                    if (!MyUtility.Check.Empty(eta1))
                    {
                        sqlcmd.Append(string.Format(@" 
        and p.ETA >= '{0}'", eta1));
                    }

                    if (!MyUtility.Check.Empty(eta2))
                    {
                        sqlcmd.Append(string.Format(@" 
        and p.ETA <= '{0}'", eta2));
                    }

                    switch (selectindex)
                    {
                        case 0:
                            sqlcmd.Append(@" And (a.stocktype = 'B' or a.stocktype = 'I')");
                            break;
                        case 1:
                            sqlcmd.Append(@" And a.stocktype = 'B'");
                            break;
                        case 2:
                            sqlcmd.Append(@" And a.stocktype = 'I'");
                            break;
                    }
                }
            }
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
    }
}
