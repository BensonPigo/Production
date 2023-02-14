using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Collections.Generic;
using Sci.Win.Tools;
using Sci.Production.Prg;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class R12 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private StringBuilder sqlcmd = new StringBuilder();
        private List<SqlParameter> sqlParameters = new List<SqlParameter>();

        /// <inheritdoc/>
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.comboMDivision1.SetDefalutIndex(true);
            this.comboFactory1.SetDataSource(this.comboMDivision1.Text);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlcmd.Clear();
            this.sqlParameters.Clear();

            if (MyUtility.Check.Empty(this.txtSeason.Text))
            {
                MyUtility.Msg.WarningBox("Season can't be blank.");
                return false;
            }

            string where = string.Empty;
            if (!this.txtSeason.Text.Empty())
            {
                string split = "'" + string.Join("','", this.txtSeason.Text.Split(',')) + "'";
                where += $"\r\nand o.SeasonID in (select ID from Season where SeasonSCIID in ({split}))";
            }

            if (!this.txtstyle1.Text.Empty())
            {
                where += "\r\nand o.StyleID = @StyleID";
                this.sqlParameters.Add(new SqlParameter("@StyleID", SqlDbType.VarChar) { Value = this.txtstyle1.Text });
            }

            if (!this.txtbrand1.Text.Empty())
            {
                where += "\r\nand o.BrandID = @BrandID";
                this.sqlParameters.Add(new SqlParameter("@BrandID", SqlDbType.VarChar) { Value = this.txtbrand1.Text });
            }

            if (!this.comboMDivision1.Text.Empty())
            {
                where += "\r\nand o.MDivisionID = @MDivisionID";
                this.sqlParameters.Add(new SqlParameter("@MDivisionID", SqlDbType.VarChar) { Value = this.comboMDivision1.Text });
            }

            if (!this.comboFactory1.Text.Empty())
            {
                where += "\r\nand o.FtyGroup = @FtyGroup";
                this.sqlParameters.Add(new SqlParameter("@FtyGroup", SqlDbType.VarChar) { Value = this.comboFactory1.Text });
            }

            this.sqlcmd.Append($@"
select m.BrandID
,m.SeasonID
,s.ProgramID
,s.CDCodeNew
,[ProductType] = (select Name from Reason where ReasontypeID = 'Style_Apparel_Type' and ID = s.ApparelType)
,smd.PhaseID
,m.MarkerNo
,s.ID
,mls.SizeCode
,mls.MarkerName
,sb.Refno
,ml.FabricPanelCode
,f.WeightM2
,mls.Qty
,ml.MarkerLength
,[MarkerLength_inch] = Cast(IIF(isnull(ml.MarkerLength, '') = '', 0, dbo.MarkerLengthToYDS(dbo.MarkerLengthSampleTOTrade(ml.MarkerLength,sb.MatchFabric))) as numeric(7,4))
,ml.Width
,ml.Efficiency
,ml.ConsPC
,ml.OneTwoWay
,[CutPertmeter] = ISNULL(ml.ActCuttingPerimeter,'')
,[CutPertmeter_inch] = 
Cast(
	case when isnull(ml.ActCuttingPerimeter, '') = '' then 0
	when isnull(ml.ActCuttingPerimeter, '') not like '%Y%""%' then 0　--不加判斷會掛掉
	else dbo.ActCuttingPerimeterToInch(ml.ActCuttingPerimeter) end
	as numeric(20,4)
)
,m.ActFtyMarker
,[CO] = (select Name from DropDownList where Type = 'NewCO_Type' and ID = s.NEWCO)
,[MarkMarker] = (select ID + '-' + Name from TPEPass1 where ID = m.MarkerName)
,[MR]  = (select ID + '-' + Name from TPEPass1 where ID = sm.MR)
,ActFinDate = m.ActFinDate
,o.StyleUkey
,destRank = DENSE_RANK() over (Partition by m.MarkerNo, sm.SizeGroup order by 
   case when smd.PhaseID = 'BULK' then 1 
        when smd.PhaseID = 'PP SAMPLE' then 2
        when smd.PhaseID = 'SIZE/S' then 3
    else 99 end)
into #tmp
from Orders o
inner join Style s on s.Ukey = o.StyleUkey
outer apply(
	select ActFinDate = MAX(ActFinDate),PhaseID
	from Marker m
	inner join SMNotice sm on m.ID = sm.ID
	inner join SMNotice_Detail smd on smd.ID = sm.ID and smd.Type='M'
	where m.StyleUkey = s.Ukey
	group by PhaseID
)MaxActFinDate
inner join Marker m on  m.ActFinDate = MaxActFinDate.ActFinDate
inner join Marker_ML ml on ml.MarkerUkey = m.UKey 
inner join Marker_ML_SizeQty mls on mls.MarkerUkey = m.UKey and mls.MarkerName = ml.MarkerName
inner join SMNotice sm on sm.ID = m.ID
inner join SMNotice_Detail smd on smd.ID = sm.ID and smd.Type = 'M' and  smd.PhaseID in ('BULK','SIZE/S','PP SAMPLE')
inner join Style_BOF sb on sb.StyleUkey = s.Ukey and sb.FabricCode = ml.FabricCode
left join Fabric f on f.SCIRefno = sb.SCIRefno
where  1=1
and smd.PhaseID in ('BULK','SIZE/S','PP SAMPLE')
{where}

select distinct BrandID ,SeasonID
,ProgramID
,CDCodeNew
,[ProductType]
,PhaseID
,MarkerNo
,ID
,SizeCode
,MarkerName
,Refno
,FabricPanelCode
,WeightM2
,Qty
,MarkerLength
,[MarkerLength_inch]
,Width
,Efficiency
,ConsPC
,OneTwoWay
,[CutPertmeter]
,[CutPertmeter_inch]
,ActFtyMarker
,[CO]
,[MarkMarker]
,[MR]
,ActFinDate
from #tmp where destRank = 1

drop table #tmp
");

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlcmd.ToString(), this.sqlParameters, out this.printData);
         }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.printData.Columns.Remove("ActFinDate");
            string filename = "Cutting_R12.xltx";
            Excel.Application excelapp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, filename, 1, false, null, excelapp, wSheet: excelapp.Sheets[1]);

            excelapp.Columns.AutoFit();
            string excelfile = Class.MicrosoftFile.GetName("Cutting_R12");
            excelapp.ActiveWorkbook.SaveAs(excelfile);
            excelapp.Visible = true;
            Marshal.ReleaseComObject(excelapp);
            return true;
        }

        private void txtSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = @"select ID from SeasonSCI where Junk = 0";
            SelectItem2 item = new SelectItem2(sqlcmd, "Season", this.txtSeason.Text);
            item.Width = 300;
            DialogResult dialogResult = item.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeason.Text = item.GetSelectedString();
        }
    }
}
