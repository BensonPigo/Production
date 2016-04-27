using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P03_InspectionList : Sci.Win.Subs.Base
    {
        DataRow dr;
        public P03_InspectionList(DataRow data)
        {
            InitializeComponent();
            dr = data;
            this.Text += string.Format(" [Fabric Type : {0} SP# : {3} Seq : {1}-{2}]", dr["fabrictype2"], dr["seq1"], dr["seq2"], dr["id"]);
        }

        protected override void OnFormLoaded()
        {
            StringBuilder sqlcmd = new StringBuilder();
            DataTable dtFIR_AIR;
            DualResult selectResult1;
            base.OnFormLoaded();
            if (dr["fabrictype"].ToString().ToUpper() == "F")
            {
                sqlcmd.Append(string.Format(@"select b.MDivisionID,b.InvNo,b.ExportId,b.ETA,a.ArriveQty
,CASE 
	when a.result = 'P' then 'Pass'
	when a.result = 'L' then 'L/G'
	when a.result = 'R' then 'Reject'
	when a.result = 'F' then 'Fail' 
	when a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1 then 'N/A'
	else ''
END as [Result]
,iif(a.arriveqty = 0 ,null,round(a.TotalInspYds/a.ArriveQty * 100,2)) InspRate
,a.TotalInspYds
,a.TotalDefectPoint
,CASE when a.Physical = 'P' then 'Pass' 
		when a.Physical = 'F' then 'Fail' 
		when a.Nonphysical=1 then  'N/A' else '' end AS [Physical]
,a.PhysicalDate
,CASE when a.Weight = 'P' then 'Pass' 
		when a.Weight = 'F' then 'Fail' 
		when a.nonWeight=1 then  'N/A' else '' end  AS [Weight]
,a.WeightDate
,CASE when a.ShadeBond = 'P' then 'Pass' 
		when a.ShadeBond = 'F' then 'Fail' 
		when a.nonShadebond=1 then  'N/A' else '' end AS [ShadeBond]
,a.ShadeBondDate
,CASE when a.Continuity = 'P' then 'Pass' 
		when a.Continuity = 'F' then 'Fail' 
		when a.nonContinuity=1 then  'N/A' else '' end AS [Continuity]
,a.ContinuityDate
,a.id
 from dbo.FIR a inner join dbo.Receiving b on b.Id= a.ReceivingID
where a.POID='{0}' and a.Seq1 ='{1}' and a.seq2='{2}'", dr["id"], dr["seq1"], dr["seq2"]));

                selectResult1 = DBProxy.Current.Select(null, sqlcmd.ToString(), out dtFIR_AIR);
                if (selectResult1 == false) ShowErr(sqlcmd.ToString(), selectResult1);

                bsAIR_FIR.DataSource = dtFIR_AIR;

                #region  開窗
                Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
                ts1.CellMouseDoubleClick += (s, e) =>
                {
                    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                    if (null == data) return;
                    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                    frm.ShowDialog(this);
                };

                Ict.Win.DataGridViewGeneratorNumericColumnSettings ns1 = new DataGridViewGeneratorNumericColumnSettings();
                ns1.CellMouseDoubleClick += (s, e) =>
                {
                    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                    if (null == data) return;
                    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                    frm.ShowDialog(this);
                };

                Ict.Win.DataGridViewGeneratorDateColumnSettings ds1 = new DataGridViewGeneratorDateColumnSettings();
                ds1.CellMouseDoubleClick += (s, e) =>
                {
                    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                    if (null == data) return;
                    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                    frm.ShowDialog(this);
                };
                #endregion

                //設定gridFirAir的顯示欄位
                this.gridFirAir.IsEditingReadOnly = true;
                this.gridFirAir.DataSource = bsAIR_FIR;
                Helper.Controls.Grid.Generator(this.gridFirAir)
                     .Text("MDivisionID", header: "M", width: Widths.AnsiChars(5))
                     .Text("InvNo", header: "Invoice#", width: Widths.AnsiChars(20))
                     .Text("ExportId", header: "Wk#", width: Widths.AnsiChars(15))
                     .Date("ETA", header: "ETA", width: Widths.AnsiChars(11))
                     .Numeric("ArriveQty", header: "Total" + Environment.NewLine + "Ship Qty", width: Widths.AnsiChars(6), integer_places: 9, decimal_places: 2)
                     .Text("Result", header: "Overall" + Environment.NewLine + "Result", width: Widths.AnsiChars(8))
                     .Numeric("InspRate", header: "% of" + Environment.NewLine + "Inspection", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, settings:ns1)
                     .Numeric("TotalInspYds", header: "Total" + Environment.NewLine + "Inspected YDS", width: Widths.AnsiChars(6), integer_places: 9, decimal_places: 2, settings: ns1)
                     .Numeric("TotalDefectPoint", header: "Total Point" + Environment.NewLine + "Defects", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0, settings: ns1)
                     .Text("Physical", header: "Physical", width: Widths.AnsiChars(8), settings: ts1)
                     .Date("PhysicalDate", header: "PhysicalDate", width: Widths.AnsiChars(13), settings: ds1)
                     .Text("Weight", header: "Weight", width: Widths.AnsiChars(8), settings: ts1)
                     .Date("WeightDate", header: "WeightDate", width: Widths.AnsiChars(13), settings: ds1)
                     .Text("ShadeBond", header: "ShadeBond", width: Widths.AnsiChars(8), settings: ts1)
                     .Date("ShadeBondDate", header: "ShadeBondDate", width: Widths.AnsiChars(13), settings: ds1)
                     .Text("Continuity", header: "Continuity", width: Widths.AnsiChars(8), settings: ts1)
                     .Date("ContinuityDate", header: "ContinuityDate", width: Widths.AnsiChars(13), settings: ds1)
                     ;

                sqlcmd.Clear();
                sqlcmd.Append(string.Format(@"select c.MDivisionID,c.InvNo,c.ExportId
,iif(c.InvNo = '' or c.InvNo is null,c.ETA,(select export.eta from dbo.export where export.id= c.exportid )) as [ETA]
,b.ArriveQty
,a.ReceiveSampleDate
,CASE WHEN a.[Crocking] = 'P' THEN 'Pass'
WHEN A.[Crocking] = 'F' THEN 'Failed'
WHEN A.[Crocking] = 'N' THEN 'N/A'
ELSE ''
END AS [Crocking]
,a.CrockingDate
,CASE WHEN a.Heat = 'P' THEN 'Pass'
WHEN A.Heat = 'F' THEN 'Failed'
WHEN A.Heat = 'N' THEN 'N/A'
ELSE ''
END AS [Heat]
,a.HeatDate
,CASE WHEN a.[Wash] = 'P' THEN 'Pass'
WHEN A.[Wash] = 'F' THEN 'Failed'
WHEN A.[Wash] = 'N' THEN 'N/A'
ELSE ''
END AS [Wash]
,a.WashDate
,a.id
from dbo.FIR_Laboratory a 
inner join dbo.FIR b on b.id = a.id
inner join dbo.Receiving c on c.Id = b.ReceivingID
where a.POID='{0}' and a.seq1='{1}' and a.seq2='{2}'", dr["id"], dr["seq1"], dr["seq2"]));
                DataTable dtFIR_Laboratory;
                selectResult1 = DBProxy.Current.Select(null, sqlcmd.ToString(), out dtFIR_Laboratory);
                if (selectResult1 == false) ShowErr(sqlcmd.ToString(), selectResult1);

                bsFIR_Laboratory.DataSource = dtFIR_Laboratory;
                #region  開窗
                Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
                ts2.CellMouseDoubleClick += (s, e) =>
                {
                    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                    if (null == data) return;
                    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                    frm.ShowDialog(this);
                };

                Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
                ns2.CellMouseDoubleClick += (s, e) =>
                {
                    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                    if (null == data) return;
                    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                    frm.ShowDialog(this);
                };

                Ict.Win.DataGridViewGeneratorDateColumnSettings ds2 = new DataGridViewGeneratorDateColumnSettings();
                ds2.CellMouseDoubleClick += (s, e) =>
                {
                    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                    if (null == data) return;
                    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                    frm.ShowDialog(this);
                };
                #endregion
                //設定gridFir_Laboratory的顯示欄位
                this.gridFir_Laboratory.IsEditingReadOnly = true;
                this.gridFir_Laboratory.DataSource = bsAIR_FIR;
                Helper.Controls.Grid.Generator(this.gridFir_Laboratory)
                    .Text("MDivisionID", header: "M", width: Widths.AnsiChars(5))
                     .Text("InvNo", header: "Invoice#", width: Widths.AnsiChars(20))
                     .Text("ExportId", header: "Wk#", width: Widths.AnsiChars(15))
                     .Date("ETA", header: "ETA", width: Widths.AnsiChars(11))
                     .Numeric("ArriveQty", header: "Total" + Environment.NewLine + "Ship Qty", width: Widths.AnsiChars(6), integer_places: 9, decimal_places: 2)

                     .Date("ReceiveSampleDate", header: "Date", width: Widths.AnsiChars(13),settings:ds2)
                     .Text("Crocking", header: "Crocking" + Environment.NewLine + "Test", width: Widths.AnsiChars(8), settings: ts2)
                     .Date("CrockingDate", header: "Crocking Date", width: Widths.AnsiChars(13), settings: ds2)
                     .Text("Heat", header: "Heat" + Environment.NewLine + "Shrinkage", width: Widths.AnsiChars(8), settings: ts2)
                     .Date("HeatDate", header: "Heat Date", width: Widths.AnsiChars(13), settings: ds2)
                     .Text("Wash", header: "Wash" + Environment.NewLine + "Shrinkage", width: Widths.AnsiChars(8), settings: ts2)
                     .Date("WashDate", header: "Wash Date", width: Widths.AnsiChars(13), settings: ds2);
            }
            else
            {
                sqlcmd.Append(string.Format(@"select b.MDivisionID,b.InvNo,b.ExportId,b.ETA
,CASE 
	when a.result = 'P' then 'Pass'
	when a.result = 'L' then 'L/G'
	when a.result = 'R' then 'Reject'
	when a.result = 'F' then 'Fail' 
	else  '' 
END as [Result]--,a.seq1,a.seq2
,a.InspDate
,dbo.getPass1(a.Inspector) Inspector
,a.InspQty
,a.RejectQty
,a.Defect
,a.id
 from dbo.AIR a inner join dbo.Receiving b on b.Id= a.ReceivingID
where a.POID='{0}' and a.Seq1 ='{1}' and a.seq2='{2}'", dr["id"], dr["seq1"], dr["seq2"]));
                DataTable dtFIR_Laboratory;
                selectResult1 = DBProxy.Current.Select(null, sqlcmd.ToString(), out dtFIR_Laboratory);
                if (selectResult1 == false) ShowErr(sqlcmd.ToString(), selectResult1);

                bsAIR_FIR.DataSource = dtFIR_Laboratory;

                //設定Grid1的顯示欄位
                this.gridFirAir.IsEditingReadOnly = true;
                this.gridFirAir.DataSource = bsAIR_FIR;
                Helper.Controls.Grid.Generator(this.gridFirAir)
                     .Text("MDivisionID", header: "M", width: Widths.AnsiChars(5))
                     .Text("InvNo", header: "Invoice#", width: Widths.AnsiChars(20))
                     .Text("ExportId", header: "Wk#", width: Widths.AnsiChars(15))
                     .Date("ETA", header: "ETA", width: Widths.AnsiChars(11))
                     .Text("Result", header: "Result", width: Widths.AnsiChars(8))
                     .Date("InspDate", header: "Inspection" + Environment.NewLine + "Date", width: Widths.AnsiChars(13))
                     .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(13))
                     .Numeric("InspQty", header: "Inspected" + Environment.NewLine + "Qty", width: Widths.AnsiChars(6), integer_places: 9, decimal_places: 2)
                     .Numeric("RejectQty", header: "Rejected" + Environment.NewLine + "Qty", width: Widths.AnsiChars(6), integer_places: 9, decimal_places: 2)
                     .Text("Defect", header: "Defect Type", width: Widths.AnsiChars(30))
                     
                     ;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
