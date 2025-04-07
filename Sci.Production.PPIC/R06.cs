﻿using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R06
    /// </summary>
    public partial class R06 : Win.Tems.PrintForm
    {
        private DataTable _printData;
        private DateTime? _sciDate1;
        private DateTime? _sciDate2;
        private DateTime? _buyerDelDate1;
        private DateTime? _buyerDelDate2;
        private string _mDivision;
        private string _orderType;
        private string _factory;
        private string _category;
        private string _SPStart;
        private string _SPEnd;
        private int _excludeReplacement;
        private int _complection;
        private bool sewingMaterialComplete;
        private bool packingMaterialComplete;

        /// <summary>
        /// R06
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.comboM.SetDefalutIndex(true);
            this.comboFactory.SetDataSource(this.comboM.Text);
            this.comboM.Enabled = false;
            MyUtility.Tool.SetupCombox(this.comboOrderType, 1, 1, "Bulk,Sample,Bulk+Sample,Material");
            this.comboOrderType.Text = "Bulk+Sample";

        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) && MyUtility.Check.Empty(this.textSPStart.Text.Trim()) && MyUtility.Check.Empty(this.textSPEnd.Text.Trim()))
            {
                MyUtility.Msg.WarningBox("[SP#] and [SCI Delivery] can't all empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.textSPStart.Text.Trim()) && MyUtility.Check.Empty(this.textSPEnd.Text.Trim()))
            {
                MyUtility.Msg.WarningBox("SP# need between two values!");
                this.textSPEnd.Focus();
                return false;
            }
            else if (MyUtility.Check.Empty(this.textSPStart.Text.Trim()) && !MyUtility.Check.Empty(this.textSPEnd.Text.Trim()))
            {
                MyUtility.Msg.WarningBox("SP# need between two values!");
                this.textSPStart.Focus();
                return false;
            }

            this._SPStart = this.textSPStart.Text;
            this._SPEnd = this.textSPEnd.Text;
            this._sciDate1 = this.dateSCIDelivery.Value1;
            this._sciDate2 = this.dateSCIDelivery.Value2;
            this._buyerDelDate1 = this.dateBuyerDelivery.Value1;
            this._buyerDelDate2 = this.dateBuyerDelivery.Value2;
            this._mDivision = this.comboM.Text;
            this._orderType = this.comboOrderType.SelectedIndex == -1 ? string.Empty : this.comboOrderType.SelectedIndex == 0 ? "B" : this.comboOrderType.SelectedIndex == 1 ? "S" : this.comboOrderType.SelectedIndex == 2 ? "BS" : "M";
            this._excludeReplacement = this.checkExcludedReplacementItem.Checked ? 1 : 0;
            this._complection = this.checkPOMaterialCompletion.Checked ? 1 : 0;
            this._factory = this.comboFactory.Text;
            this._category = this.comboOrderType.Text;
            this.sewingMaterialComplete = this.chkSewingMaterialComplete.Checked;
            this.packingMaterialComplete = this.chkPackingMaterialComplete.Checked;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"
with tmpPO as (
    select ps.ID,ps.SEQ1,Qty,psd.ETA,s.ThirdCountry,isnull(s.AbbEN,'') as SuppAbb
    from PO_Supp ps WITH (NOLOCK) 
    inner join (
        select a.ID,a.SEQ1,sum(a.Qty) as Qty,max(a.ETA) as ETA 
        from 
        (
            select psd.ID,psd.SEQ1,psd.SEQ2,psd.FabricType,psd.ETA,
            IIF(psd.FabricType = 'F',psd.Qty-isnull((select sum(ed.Qty) from Export_Detail ed WITH (NOLOCK) where ed.PoID = psd.ID and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2),0),0) as Qty
            from PO_Supp_Detail psd WITH (NOLOCK) 
            where psd.Junk = 0
            and psd.Complete = 0
        {0}
        ) a
        group by a.ID,a.SEQ1
    ) psd on psd.ID = ps.ID and psd.SEQ1 = ps.SEQ1
    left join Supp s WITH (NOLOCK) on s.ID = ps.SuppID
),PrepareData1 as (
    select ID,ThirdCountry,SEQ1+'-'+SuppAbb as Seq,
    IIF(Qty > 0,IIF(ETA is null,'',CONVERT(VARCHAR(2),Month(ETA))+'/'+CONVERT(VARCHAR(2),DAY(ETA)))+'-'+CONVERT(VARCHAR(10),Qty)+isnull((select top 1 psd.POUnit from PO_Supp_Detail psd WITH (NOLOCK) 
    where psd.ID = tmpPO.ID and psd.SEQ1 = tmpPO.SEQ1 and psd.FabricType = 'F' and psd.Junk = 0 and psd.Complete = 0 and psd.POUnit <> '' {0}),''),'') as Qty
    from tmpPO
),PrepareData2 as (
    select ID,ThirdCountry,Seq+IIF(Qty = '','','('+Qty+')') as Seq
    from PrepareData1
)
select o.ID,o.StyleID,
Category = 
    (CASE WHEN o.Category = 'B' THEN 'Bulk'
        WHEN o.Category = 'S' THEN 'Sample'
        WHEN o.Category = 'O' THEN 'Other'
        WHEN o.Category = 'M' THEN 'Material'
        WHEN o.Category = 'T' THEN 'SMTL'
        END)
    ,o.SeasonID,o.SewInLine,o.LETA,o.KPILETA,o.PFETA,o.BuyerDelivery,o.SciDelivery,o.BrandID,o.CPU
    ,[ttlCPU] = o.Qty * o.CPU
    ,o.SewETA,o.PackETA,o.MDivisionID,o.FactoryID,dbo.getPass1(o.LocalMR) as LocalMR,
    dbo.getTPEPass1(o.MCHandle) as MCHandle,dbo.getTPEPass1(o.MRHandle) as MRHandle,
    dbo.getTPEPass1(o.SMR) as SMR,dbo.getTPEPass1(p.POSMR) as POSMR,
    dbo.getTPEPass1(p.POHandle) as POHandle,o.Qty,o.CPU*o.Qty*o.CPUFactor as tCPU,o.MTLComplete,
    isnull((select CONCAT(Seq,';') from PrepareData2 where ID = o.POID and ThirdCountry = 0 for XML PATH('')),'') as SeqNo,
    isnull((select CONCAT(Seq,';') from PrepareData2 where ID = o.POID and ThirdCountry = 1 for XML PATH('')),'') as Seq3rd
    , [Fab_ETA]=(select max(FinalETA) F_ETA from PO_Supp_Detail where id=p.ID  and FabricType='F')
    , [Acc_ETA]=(select max(FinalETA) A_ETA from PO_Supp_Detail where id=p.ID  and FabricType='A')
    , Order_Qty.Article  
	, [SewingMtlComplt] = isnull(f.SewingMtlComplt, '')
	, [PackingMtlComplt] = isnull(f.PackingMtlComplt, '')
from Orders o WITH (NOLOCK) 
left join PO p WITH (NOLOCK) on p.ID = o.POID
outer apply(
    select Article = 
	stuff((
	        select distinct concat(',', Article)
	        from Order_Qty oq with(nolock)
	        where oq.id = o.ID             
		    for xml path('')
	),1,1,'')
)Order_Qty
outer apply (
	select 
		[PackingMtlComplt] = max([PackingMtlComplt])
		, [SewingMtlComplt] = max([SewingMtlComplt])
	from 
	(
		select  f.ProductionType
			, [PackingMtlComplt] = iif(f.ProductionType = 'Packing' and sum(iif(f.ProductionType = 'Packing', 1, 0)) = sum(iif(f.ProductionType = 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
			, [SewingMtlComplt] = iif(f.ProductionType <> 'Packing' and sum(iif(f.ProductionType <> 'Packing', 1, 0)) = sum(iif(f.ProductionType <> 'Packing' and f.Complete = 1, 1, 0)), 'Y', '')
		from 
		(
			select f.ProductionType
				, psd.Complete
			from PO_Supp_Detail psd
			inner join PO_Supp_Detail_OrderList psdo on psd.ID = psdo.ID and psd.SEQ1 = psdo.SEQ1 and psd.SEQ2 = psdo.SEQ2
			outer apply (
				select [ProductionType] = iif(m.ProductionType = 'Packing', 'Packing', 'Sewing')
				from Fabric f
				left join MtlType m on f.MtlTypeID = m.ID
				where f.SCIRefno = psd.SCIRefno
			)f  
			where psdo.OrderID	= o.ID
			and psd.Junk = 0
		)f
		group by f.ProductionType
	)f
)f
where 1=1", this._excludeReplacement == 1 ? "and psd.SEQ1 not between '50' and '69'" : string.Empty));

            if (!MyUtility.Check.Empty(this._sciDate1))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this._sciDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this._sciDate2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this._sciDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this._buyerDelDate1))
            {
                sqlCmd.Append(string.Format(@" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this._buyerDelDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this._buyerDelDate2))
            {
                sqlCmd.Append(string.Format(@" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this._buyerDelDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this._mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", this._mDivision));
            }

            if (!MyUtility.Check.Empty(this._SPStart))
            {
                sqlCmd.Append(string.Format(" and o.id between '{0}' and '{1}'", this._SPStart, this._SPEnd));
            }

            if (!MyUtility.Check.Empty(this._factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", this._factory));
            }

            if (!MyUtility.Check.Empty(this._orderType))
            {
                if (this._orderType == "B")
                {
                    sqlCmd.Append(" and o.Category = 'B'");
                }
                else if (this._orderType == "S")
                {
                    sqlCmd.Append(" and o.Category = 'S'");
                }
                else if (this._orderType == "M")
                {
                    sqlCmd.Append(" and (o.Category = 'M' or o.Category = 'T')");
                }
                else
                {
                    sqlCmd.Append(" and (o.Category = 'B' or o.Category ='S')");
                }
            }

            if (this.sewingMaterialComplete)
            {
                sqlCmd.Append(" and f.SewingMtlComplt = 'Y'");
            }

            if (this.packingMaterialComplete)
            {
                sqlCmd.Append(" and f.PackingMtlComplt = 'Y'");
            }

            sqlCmd.Append(" order by o.ID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this._printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this._printData.Rows.Count);

            if (this._printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\PPIC_R06_MonthlyMaterialCompletion.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[2, 2] = string.Format("{0}~{1}", MyUtility.Check.Empty(this._sciDate1) ? string.Empty : Convert.ToDateTime(this._sciDate1).ToString("yyyy/MM/dd"), MyUtility.Check.Empty(this._sciDate2) ? string.Empty : Convert.ToDateTime(this._sciDate2).ToString("yyyy/MM/dd"));

            worksheet.Cells[2, 7] = this._mDivision;
            worksheet.Cells[2, 9] = this._factory;
            worksheet.Cells[2, 11] = this._category;
            worksheet.Cells[2, 16] = this._excludeReplacement == 1 ? "True" : "False";
            worksheet.Cells[2, 18] = this._complection == 1 ? "True" : "False";

            // 填內容值
            int intRowsStart = 4;
            object[,] objArray = new object[1, 34];
            foreach (DataRow dr in this._printData.Rows)
            {
                objArray[0, 0] = dr["ID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["Category"];
                objArray[0, 3] = dr["SeasonID"];
                objArray[0, 4] = dr["Article"];
                objArray[0, 5] = dr["SewInLine"];
                objArray[0, 6] = dr["LETA"];
                objArray[0, 7] = dr["KPILETA"];
                objArray[0, 8] = dr["PFETA"];
                objArray[0, 9] = dr["BuyerDelivery"];
                objArray[0, 10] = dr["SciDelivery"];
                objArray[0, 11] = dr["BrandID"];
                objArray[0, 12] = dr["CPU"];
                objArray[0, 13] = dr["ttlCPU"];
                objArray[0, 14] = dr["SeqNo"];
                objArray[0, 15] = dr["Seq3rd"];
                objArray[0, 16] = dr["SewETA"];
                objArray[0, 17] = dr["PackETA"];
                objArray[0, 18] = dr["Fab_ETA"];
                objArray[0, 19] = dr["Acc_ETA"];
                objArray[0, 20] = dr["MDivisionID"];
                objArray[0, 21] = dr["FactoryID"];
                objArray[0, 22] = dr["LocalMR"];
                objArray[0, 23] = dr["MCHandle"];
                objArray[0, 24] = dr["MRHandle"];
                objArray[0, 25] = dr["SMR"];
                objArray[0, 26] = dr["POHandle"];
                objArray[0, 27] = dr["POSMR"];
                objArray[0, 28] = dr["Qty"];
                objArray[0, 29] = dr["tCPU"];
                objArray[0, 30] = this._complection == 1 && MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper() == "TRUE" ? "Y" : MyUtility.Check.Empty(dr["SeqNo"]) && MyUtility.Check.Empty(dr["Seq3rd"]) ? "Y" : "N";
                objArray[0, 31] = MyUtility.Convert.GetString(dr["MTLComplete"]).ToUpper() == "FALSE" ? string.Empty : "Y";
                objArray[0, 32] = dr["SewingMtlComplt"];
                objArray[0, 33] = dr["PackingMtlComplt"];
                worksheet.Range[string.Format("A{0}:AH{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R06_MonthlyMaterialCompletion");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
