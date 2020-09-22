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
    public partial class R17 : Win.Tems.PrintForm
    {
        private DataTable dt;

        private int selectindex = 0;

        public R17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.comboStockType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1)
                && MyUtility.Check.Empty(this.dateSCIDelivery.Value2)
                && MyUtility.Check.Empty(this.txtSPNo.Text)
                && MyUtility.Check.Empty(this.dateETA.TextBox1.Value)
                && MyUtility.Check.Empty(this.dateETA.TextBox2.Value)
                && MyUtility.Check.Empty(this.txtMtlLocationStart.Text)
                && MyUtility.Check.Empty(this.txtLocationEnd.Text))
            {
                MyUtility.Msg.WarningBox("SP#, SCI Delivery, ETA, Location can't be empty!!");
                return false;
            }

            this.selectindex = this.comboStockType.SelectedIndex;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.dt.Rows.Count);
            DualResult result = Ict.Result.True;
            if (this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return result;
            }

            // MyUtility.Excel.CopyToXls(dt,"","Warehouse_R17_Location_List.xltx");
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R17_Location_List.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Warehouse_R17_Location_List.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            this.ShowWaitMessage("Excel Processing...");

            for (int i = 1; i <= this.dt.Rows.Count; i++)
            {
                if (!((string)((Excel.Range)objSheets.Cells[i + 1, 12]).Value).Empty())
                {
                    objSheets.Cells[i + 1, 12] = ((string)((Excel.Range)objSheets.Cells[i + 1, 12]).Value).Trim();
                }
            }

          // objSheets.Columns[12].ColumnWidth = 50;
            objSheets.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R17_Location_List");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return false;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            // return base.OnAsyncDataLoad(e);
            string spno = this.txtSPNo.Text.TrimEnd();
            string locationStart = this.txtMtlLocationStart.Text;
            string locationEnd = this.txtLocationEnd.Text;
            string factory = this.txtfactory.Text;
            bool chkbalance = this.checkBalanceQty.Checked;
            string locationFilte = string.Empty;
            string eta1 = string.Empty;
            string eta2 = string.Empty;
            if (!MyUtility.Check.Empty(this.dateETA.TextBox1.Value))
            {
                eta1 = this.dateETA.TextBox1.Text;
            }

            if (!MyUtility.Check.Empty(this.dateETA.TextBox2.Value))
            {
                eta2 = this.dateETA.TextBox2.Text;
            }

            if (locationStart.Empty() == false && locationEnd.Empty() == false)
            {
                locationFilte = string.Format("b.mtllocationid between '{0}' and '{1}'", locationStart, locationEnd);
            }
            else if (locationStart.Empty() == true && locationEnd.Empty() == false)
            {
                locationFilte = locationFilte = string.Format("b.mtllocationid < '{0}'", locationEnd);
            }
            else if (locationStart.Empty() == false && locationEnd.Empty() == true)
            {
                locationFilte = locationFilte = string.Format("'{0}' < b.mtllocationid", locationStart);
            }

            DualResult result = Ict.Result.True;
            StringBuilder sqlcmd = new StringBuilder();
            #region sql command
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2)) // SCI Delivery empty
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
        Receive.ETA,
        MaterialType = case when p.FabricType = 'F'then 'Fabric' 
							when p.FabricType = 'A'then 'Accessory' else 'All'end,
        Category = DropDownList.Name,
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
outer apply(
	select distinct name from DropDownList where Type='Pms_MtlCategory'
	and SUBSTRING(ID,2,1)= orders.Category
    and name !='ALL'
)DropDownList
OUTER APPLY(
	select r.ETA
	from Receiving_Detail rd
	inner join Receiving r on rd.id=r.id
	WHERE rd.Roll = a.Roll and rd.Dyelot=a.Dyelot
	and a.POID=rd.PoId and a.Seq1=rd.Seq1 and a.Seq2=rd.Seq2
)Receive
where   1=1");
                    if (!MyUtility.Check.Empty(spno))
                    {
                        sqlcmd.Append(string.Format(
                            @"
        And a.Poid like '{0}%'", spno));
                    }

                    if (!this.txtSeq.CheckSeq1Empty())
                    {
                        sqlcmd.Append(string.Format(
                            @"
        and a.seq1 = '{0}'", this.txtSeq.Seq1));
                    }

                    if (!this.txtSeq.CheckSeq2Empty())
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and a.seq2 = '{0}'", this.txtSeq.Seq2));
                    }

                    if (chkbalance)
                    {
                        sqlcmd.Append(@" 
        And a.inqty- a.outqty + a.adjustqty > 0");
                    }

                    if (!MyUtility.Check.Empty(factory))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and orders.FactoryID = '{0}'", factory));
                    }

                    if (!MyUtility.Check.Empty(eta1))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and Receive.ETA >= '{0}'", eta1));
                    }

                    if (!MyUtility.Check.Empty(eta2))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and Receive.ETA <= '{0}'", eta2));
                    }

                    switch (this.selectindex)
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
                    sqlcmd.Append(string.Format(
                        @"
select distinct 
        Factory		= orders.Factoryid,
        sp			= a.Poid,
        seq1		= a.seq1,
        seq2		= a.seq2,
        Refno		= p.Refno,
        Receive.ETA,
        MaterialType = case when p.FabricType = 'F'then 'Fabric' 
							when p.FabricType = 'A'then 'Accessory' else 'All'end,
        Category = DropDownList.Name,
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
outer apply(
	select distinct name from DropDownList where Type='Pms_MtlCategory'
	and SUBSTRING(ID,2,1)= orders.Category
    and name !='ALL'
)DropDownList
OUTER APPLY(
	select r.ETA
	from Receiving_Detail rd
	inner join Receiving r on rd.id=r.id
	WHERE rd.Roll = a.Roll and rd.Dyelot=a.Dyelot
	and a.POID=rd.PoId and a.Seq1=rd.Seq1 and a.Seq2=rd.Seq2
)Receive
where   1=1 
        And {0} ", locationFilte));
                    if (!MyUtility.Check.Empty(spno))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        And a.Poid like '{0}%'", spno));
                    }

                    if (!this.txtSeq.CheckSeq1Empty())
                    {
                        sqlcmd.Append(string.Format(
                            @"
        and a.seq1 = '{0}'", this.txtSeq.Seq1));
                    }

                    if (!this.txtSeq.CheckSeq2Empty())
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and a.seq2 = '{0}'", this.txtSeq.Seq2));
                    }

                    if (chkbalance)
                    {
                        sqlcmd.Append(@" 
        And a.inqty- a.outqty + a.adjustqty > 0");
                    }

                    if (!MyUtility.Check.Empty(factory))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and orders.FactoryID = '{0}'", factory));
                    }

                    if (!MyUtility.Check.Empty(eta1))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and Receive.ETA >= '{0}'", eta1));
                    }

                    if (!MyUtility.Check.Empty(eta2))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and Receive.ETA <= '{0}'", eta2));
                    }

                    switch (this.selectindex)
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
        Receive.ETA,
        MaterialType = case when p.FabricType = 'F'then 'Fabric' 
							when p.FabricType = 'A'then 'Accessory' else 'All'end,
        Category = DropDownList.Name,
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
outer apply(
	select distinct name from DropDownList where Type='Pms_MtlCategory'
	and SUBSTRING(ID,2,1)= orders.Category
    and name !='ALL'
)DropDownList
OUTER APPLY(
	select r.ETA
	from Receiving_Detail rd
	inner join Receiving r on rd.id=r.id
	WHERE rd.Roll = a.Roll and rd.Dyelot=a.Dyelot
	and a.POID=rd.PoId and a.Seq1=rd.Seq1 and a.Seq2=rd.Seq2
)Receive
where   1=1"));

                    if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and '{0}' <= orders.scidelivery", Convert.ToDateTime(this.dateSCIDelivery.Value1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and orders.scidelivery <= '{0}'", Convert.ToDateTime(this.dateSCIDelivery.Value2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(spno))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        And a.Poid like '{0}%'", spno));
                    }

                    if (!this.txtSeq.CheckSeq1Empty())
                    {
                        sqlcmd.Append(string.Format(
                            @"
        and a.seq1 = '{0}'", this.txtSeq.Seq1));
                    }

                    if (!this.txtSeq.CheckSeq2Empty())
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and a.seq2 = '{0}'", this.txtSeq.Seq2));
                    }

                    if (chkbalance)
                    {
                        sqlcmd.Append(@" 
        And a.inqty- a.outqty + a.adjustqty > 0");
                    }

                    if (!MyUtility.Check.Empty(factory))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and orders.FactoryID = '{0}'", factory));
                    }

                    if (!MyUtility.Check.Empty(eta1))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and Receive.ETA >= '{0}'", eta1));
                    }

                    if (!MyUtility.Check.Empty(eta2))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and Receive.ETA <= '{0}'", eta2));
                    }

                    switch (this.selectindex)
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
                    sqlcmd.Append(string.Format(
                        @"
select distinct
        Factory		= orders.factoryid,
        sp			= a.Poid,
        seq1		= a.seq1,
        seq2		= a.seq2,
        Refno		= p.Refno,
        Receive.ETA,
        MaterialType = case when p.FabricType = 'F'then 'Fabric' 
							when p.FabricType = 'A'then 'Accessory' else 'All'end,
        Category = DropDownList.Name,
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
outer apply(
	select distinct name from DropDownList where Type='Pms_MtlCategory'
	and SUBSTRING(ID,2,1)= orders.Category
    and name !='ALL'
)DropDownList
OUTER APPLY(
	select r.ETA
	from Receiving_Detail rd
	inner join Receiving r on rd.id=r.id
	WHERE rd.Roll = a.Roll and rd.Dyelot=a.Dyelot
	and a.POID=rd.PoId and a.Seq1=rd.Seq1 and a.Seq2=rd.Seq2
)Receive
where   1=1
        And {0} ", locationFilte));

                    if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and '{0}' <= orders.scidelivery", Convert.ToDateTime(this.dateSCIDelivery.Value1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and orders.scidelivery <= '{0}'", Convert.ToDateTime(this.dateSCIDelivery.Value2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(spno))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        And a.Poid like '{0}%'", spno));
                    }

                    if (!this.txtSeq.CheckSeq1Empty())
                    {
                        sqlcmd.Append(string.Format(
                            @"
        and a.seq1 = '{0}'", this.txtSeq.Seq1));
                    }

                    if (!this.txtSeq.CheckSeq2Empty())
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and a.seq2 = '{0}'", this.txtSeq.Seq2));
                    }

                    if (chkbalance)
                    {
                        sqlcmd.Append(@" And a.inqty- a.outqty + a.adjustqty > 0");
                    }

                    if (!MyUtility.Check.Empty(factory))
                    {
                        sqlcmd.Append(string.Format(@" and orders.FactoryId = '{0}'", factory));
                    }

                    if (!MyUtility.Check.Empty(eta1))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and Receive.ETA >= '{0}'", eta1));
                    }

                    if (!MyUtility.Check.Empty(eta2))
                    {
                        sqlcmd.Append(string.Format(
                            @" 
        and Receive.ETA <= '{0}'", eta2));
                    }

                    switch (this.selectindex)
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
    }
}
