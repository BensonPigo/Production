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
using System.Data.SqlClient;

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

        private void open_QA_program(string this_inv, string inp_type,string receivingid) {
            string sql = @"
Select 
a.id,
a.poid,
SEQ1,
SEQ2,
Receivingid,
Refno,
SCIRefno,
Suppid,
ArriveQty,
InspDeadline,
Result,
PhysicalEncode,
WeightEncode,
ShadeBondEncode,
ContinuityEncode,
NonPhysical,
Physical,
TotalInspYds,
PhysicalDate,
Physical,
NonWeight, 
Weight,
WeightDate,
Weight,
NonShadebond,
Shadebond,
ShadebondDate,
shadebond,
NonContinuity,
Continuity,
ContinuityDate,
Continuity,
a.Status,ReplacementReportID,(seq1+seq2) as seq,
(Select weavetypeid from Fabric b WITH (NOLOCK) where b.SCIRefno =a.SCIrefno) as weavetypeid,
c.Exportid,c.whseArrival,dbo.getPass1(a.Approve) as approve1,approveDate,approve,
(Select d.colorid from PO_Supp_Detail d WITH (NOLOCK) Where d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2) as Colorid,
(Select ID+' - '+ AbbEn From Supp WITH (NOLOCK) Where a.suppid = supp.id) as SuppEn,
c.ExportID as Wkno,
NonOdor,
Odor,
OdorEncode,
OdorDate,
a.PhysicalInspector,a.WeightInspector,a.ShadeboneInspector,a.ContinuityInspector,a.OdorInspector  
From FIR a WITH (NOLOCK) Left join dbo.View_AllReceiving c WITH (NOLOCK) on c.id = a.receivingid
Where a.poid = @poid and a.seq1 = @seq1 and a.seq2 = @seq2 and c.InvNo = @InvNo and a.receivingid = @receivingid  order by seq1,seq2 ";
            List<SqlParameter> sqlPar = new List<SqlParameter>();
            sqlPar.Add(new SqlParameter("@poid", dr["id"].ToString()));
            sqlPar.Add(new SqlParameter("@seq1", dr["seq1"].ToString()));
            sqlPar.Add(new SqlParameter("@seq2", dr["seq2"].ToString()));            
            sqlPar.Add(new SqlParameter("@InvNo", this_inv.ToString()));
            sqlPar.Add(new SqlParameter("@receivingid", receivingid));

            DataTable dt;
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sql, sqlPar, out dt)))
            {
                MyUtility.Msg.ErrorBox(result.Description);
                return;
            }
            else if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found!!");
                return;
            }
            else
            {
                DataRow data = dt.Rows[0];

                if (inp_type.Equals("inspection")) {
                    var frm = new Sci.Production.Quality.P01_PhysicalInspection(false, data["ID"].ToString(), null, null, data);
                    frm.ShowDialog(this);
                    frm.Dispose();
                }
                else if (inp_type.Equals("weight")) {
                    var frm = new Sci.Production.Quality.P01_Weight(false, data["ID"].ToString(), null, null, data);
                    frm.ShowDialog(this);
                    frm.Dispose();
                }
                else if (inp_type.Equals("shadebond"))
                {
                    var frm = new Sci.Production.Quality.P01_ShadeBond(false, data["ID"].ToString(), null, null, data);
                    frm.ShowDialog(this);
                    frm.Dispose();
                }
                else if (inp_type.Equals("continuity"))
                {
                    var frm = new Sci.Production.Quality.P01_Continuity(false, data["ID"].ToString(), null, null, data);
                    frm.ShowDialog(this);
                    frm.Dispose();
                }
                else if (inp_type.Equals("Odor"))
                {
                    var frm = new Sci.Production.Quality.P01_Odor(false, data["ID"].ToString(), null, null, data);
                    frm.ShowDialog(this);
                    frm.Dispose();
                }
                else if (inp_type.Equals("crocking"))
                {
                    var frm = new Sci.Production.Quality.P03_Crocking(false, data["ID"].ToString(), null, null, data);
                    frm.ShowDialog(this);
                    frm.Dispose();
                }
                else if (inp_type.Equals("heat"))
                {
                    var frm = new Sci.Production.Quality.P03_Heat(false, data["ID"].ToString(), null, null, data);
                    frm.ShowDialog(this);
                    frm.Dispose();
                }
                else if (inp_type.Equals("wash"))
                {
                    var frm = new Sci.Production.Quality.P03_Wash(false, data["ID"].ToString(), null, null, data);
                    frm.ShowDialog(this);
                    frm.Dispose();
                }


            };

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
    when a.result !='' then a.result
	when a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1 and a.nonOdor=1 then 'N/A'
	else 'Blank'
    END as [Result]
,iif(isnull(c.ActualYds,0) = 0 ,0 ,round((c.ActualYds / a.ArriveQty)*100 ,2)) as InspRate
,c.ActualYds TotalInspYds 
,a.TotalDefectPoint
,CASE when a.Physical != '' then a.Physical
	  when a.Nonphysical=1 then  'N/A' else '' 
	  end AS [Physical]
,a.PhysicalDate
,[Weight]=IIF(a.nonWeight=1,'N/A',a.Weight)	
,a.WeightDate
,[ShadeBond]=IIF(a.nonShadebond=1,'N/A',a.ShadeBond)		
,a.ShadeBondDate
,[Continuity]=IIF(a.nonContinuity=1,'N/A',a.Continuity)		
,a.ContinuityDate
,[Odor]=IIF(a.nonOdor=1,'N/A',a.Odor)		
,a.OdorDate
,a.id
,a.ReceivingID
from dbo.FIR a WITH (NOLOCK) 
inner join dbo.View_AllReceiving b WITH (NOLOCK) on b.Id= a.ReceivingID
outer apply ( 
	select Id, isnull(sum(c.ActualYds),0) as ActualYds
	from FIR_Physical c
	where c.id = a.id
	group by c.ID 
)c
where a.POID='{0}' and a.Seq1 ='{1}' and a.seq2='{2}'", dr["id"], dr["seq1"], dr["seq2"]));

                selectResult1 = DBProxy.Current.Select(null, sqlcmd.ToString(), out dtFIR_AIR);
                if (selectResult1 == false) ShowErr(sqlcmd.ToString(), selectResult1);

                bsAIR_FIR.DataSource = dtFIR_AIR;

                #region  開窗
                //Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
                //ts1.CellMouseDoubleClick += (s, e) =>
                //{
                //    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                //    if (null == data) return;
                //    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                //    frm.ShowDialog(this);
                //};

                //Ict.Win.DataGridViewGeneratorNumericColumnSettings ns1 = new DataGridViewGeneratorNumericColumnSettings();
                //ns1.CellMouseDoubleClick += (s, e) =>
                //{
                //    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                //    if (null == data) return;
                //    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                //    frm.ShowDialog(this);
                //};

                //Ict.Win.DataGridViewGeneratorDateColumnSettings ds1 = new DataGridViewGeneratorDateColumnSettings();
                //ds1.CellMouseDoubleClick += (s, e) =>
                //{
                //    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                //    if (null == data) return;
                //    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                //    frm.ShowDialog(this);
                //};
                DataGridViewGeneratorNumericColumnSettings inspection = new DataGridViewGeneratorNumericColumnSettings();
                inspection.CellMouseDoubleClick += (s, e) =>
                {
                    open_QA_program(dtFIR_AIR.Rows[e.RowIndex]["InvNo"].ToString(), "inspection", dtFIR_AIR.Rows[e.RowIndex]["ReceivingID"].ToString());
                };

                DataGridViewGeneratorTextColumnSettings physical = new DataGridViewGeneratorTextColumnSettings();
                physical.CellMouseDoubleClick += (s, e) =>
                {
                    open_QA_program(dtFIR_AIR.Rows[e.RowIndex]["InvNo"].ToString(), "inspection", dtFIR_AIR.Rows[e.RowIndex]["ReceivingID"].ToString());
                };

                DataGridViewGeneratorTextColumnSettings weight = new DataGridViewGeneratorTextColumnSettings();
                weight.CellMouseDoubleClick += (s, e) =>
                {
                    open_QA_program(dtFIR_AIR.Rows[e.RowIndex]["InvNo"].ToString(), "weight", dtFIR_AIR.Rows[e.RowIndex]["ReceivingID"].ToString());
                };

                DataGridViewGeneratorTextColumnSettings shadebond = new DataGridViewGeneratorTextColumnSettings();
                shadebond.CellMouseDoubleClick += (s, e) =>
                {
                    open_QA_program(dtFIR_AIR.Rows[e.RowIndex]["InvNo"].ToString(), "shadebond", dtFIR_AIR.Rows[e.RowIndex]["ReceivingID"].ToString());

                };

                DataGridViewGeneratorTextColumnSettings continuity = new DataGridViewGeneratorTextColumnSettings();
                continuity.CellMouseDoubleClick += (s, e) =>
                {
                    open_QA_program(dtFIR_AIR.Rows[e.RowIndex]["InvNo"].ToString(), "continuity", dtFIR_AIR.Rows[e.RowIndex]["ReceivingID"].ToString());
                };


                DataGridViewGeneratorTextColumnSettings Odor = new DataGridViewGeneratorTextColumnSettings();
                Odor.CellMouseDoubleClick += (s, e) =>
                {
                    open_QA_program(dtFIR_AIR.Rows[e.RowIndex]["InvNo"].ToString(), "Odor", dtFIR_AIR.Rows[e.RowIndex]["ReceivingID"].ToString());
                };
                #endregion





                //設定gridFirAir的顯示欄位
                this.gridFirAir.IsEditingReadOnly = true;
                this.gridFirAir.DataSource = bsAIR_FIR;
                Helper.Controls.Grid.Generator(this.gridFirAir)
                     //.Text("MDivisionID", header: "M", width: Widths.AnsiChars(5))
                     .Text("InvNo", header: "Invoice#", width: Widths.AnsiChars(20))
                     .Text("ExportId", header: "Wk#", width: Widths.AnsiChars(15))
                     .Date("ETA", header: "ETA", width: Widths.AnsiChars(11))
                     .Numeric("ArriveQty", header: "Total" + Environment.NewLine + "Ship Qty", width: Widths.AnsiChars(6), integer_places: 9, decimal_places: 2)
                     .Text("Result", header: "Overall" + Environment.NewLine + "Result", width: Widths.AnsiChars(8))
                     .Numeric("InspRate", header: "% of" + Environment.NewLine + "Inspection", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, settings: inspection)
                     .Numeric("TotalInspYds", header: "Total" + Environment.NewLine + "Inspected YDS", width: Widths.AnsiChars(6), integer_places: 9, decimal_places: 2)
                     .Numeric("TotalDefectPoint", header: "Total Point" + Environment.NewLine + "Defects", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                     .Text("Physical", header: "Physical", width: Widths.AnsiChars(8), settings: physical)
                     .Date("PhysicalDate", header: "PhysicalDate", width: Widths.AnsiChars(13))
                     .Text("Weight", header: "Weight", width: Widths.AnsiChars(8), settings: weight)
                     .Date("WeightDate", header: "WeightDate", width: Widths.AnsiChars(13))
                     .Text("ShadeBond", header: "ShadeBond", width: Widths.AnsiChars(8), settings: shadebond)
                     .Date("ShadeBondDate", header: "ShadeBondDate", width: Widths.AnsiChars(13))
                     .Text("Continuity", header: "Continuity", width: Widths.AnsiChars(8),settings : continuity)
                     .Date("ContinuityDate", header: "ContinuityDate", width: Widths.AnsiChars(13))
                     .Text("Odor", header: "Odor", width: Widths.AnsiChars(8), settings: Odor)
                     .Date("OdorDate", header: "OdorDate", width: Widths.AnsiChars(13))
                     ;

                sqlcmd.Clear();
                sqlcmd.Append(string.Format(@"select c.MDivisionID,c.InvNo,c.ExportId
,iif(c.InvNo = '' or c.InvNo is null,c.ETA,(select export.eta from dbo.export WITH (NOLOCK) where export.id= c.exportid )) as [ETA]
,b.ArriveQty
,a.ReceiveSampleDate
,Crocking = iif(a.[Crocking]='','Blank',a.[Crocking])
,a.CrockingDate
,Heat = iif(a.Heat='','Blank',a.Heat)
,a.HeatDate
,Wash = iif(a.[Wash]='','Blank',a.[Wash])
,a.WashDate
,[Oven] = Oven.Result
,[OvenDate] = Oven.InspDate
,[ColorFastness] = ColorFastness.Result
,[ColorFastnessDate] = ColorFastness.InspDate
,a.id
,b.ReceivingID
from dbo.FIR_Laboratory a WITH (NOLOCK) 
inner join dbo.FIR b WITH (NOLOCK) on b.id = a.id
inner join dbo.View_AllReceiving c WITH (NOLOCK) on c.Id = b.ReceivingID
outer apply(select (case when  sum(iif(od.Result = 'Fail' ,1,0)) > 0 then 'Fail'
						 when  sum(iif(od.Result = 'Pass' ,1,0)) > 0 then 'Pass'
				         else 'Blank' end) as Result,
					MIN( ov.InspDate) as InspDate 
				from Oven ov
				inner join dbo.Oven_Detail od on od.ID = ov.ID
				where ov.POID=a.POID and od.SEQ1=a.Seq1 
				and seq2=a.Seq2 and ov.Status='Confirmed') as Oven
outer apply(select (case when  sum(iif(cd.Result = 'Fail' ,1,0)) > 0 then 'Fail'
						 when  sum(iif(cd.Result = 'Pass' ,1,0)) > 0 then 'Pass'
				         else 'Blank' end) as Result,
					MIN( CF.InspDate) as InspDate 
				from dbo.ColorFastness CF WITH (NOLOCK) 
				inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
				where CF.Status = 'Confirmed' and CF.POID=a.POID 
				and cd.SEQ1=a.Seq1 and cd.seq2=a.Seq2) as ColorFastness
where a.POID='{0}' and a.seq1='{1}' and a.seq2='{2}'", dr["id"], dr["seq1"], dr["seq2"]));
                DataTable dtFIR_Laboratory;
                selectResult1 = DBProxy.Current.Select(null, sqlcmd.ToString(), out dtFIR_Laboratory);
                if (selectResult1 == false) ShowErr(sqlcmd.ToString(), selectResult1);

                bsFIR_Laboratory.DataSource = dtFIR_Laboratory;
                #region  開窗
                //Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
                //ts2.CellMouseDoubleClick += (s, e) =>
                //{
                //    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                //    if (null == data) return;
                //    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                //    frm.ShowDialog(this);
                //};

                //Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
                //ns2.CellMouseDoubleClick += (s, e) =>
                //{
                //    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                //    if (null == data) return;
                //    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                //    frm.ShowDialog(this);
                //};

                //Ict.Win.DataGridViewGeneratorDateColumnSettings ds2 = new DataGridViewGeneratorDateColumnSettings();
                //ds2.CellMouseDoubleClick += (s, e) =>
                //{
                //    var data = this.gridFirAir.GetDataRow<DataRow>(e.RowIndex);
                //    if (null == data) return;
                //    var frm = new Sci.Production.Quality.P01_PhysicalInspection(long.Parse(data["id"].ToString()));
                //    frm.ShowDialog(this);
                //};

                DataGridViewGeneratorTextColumnSettings crocking = new DataGridViewGeneratorTextColumnSettings();
                crocking.CellMouseDoubleClick += (s, e) =>
                {
                    open_QA_program(dtFIR_AIR.Rows[e.RowIndex]["InvNo"].ToString(), "crocking", dtFIR_AIR.Rows[e.RowIndex]["ReceivingID"].ToString());
                };
                DataGridViewGeneratorTextColumnSettings heat = new DataGridViewGeneratorTextColumnSettings();
                heat.CellMouseDoubleClick += (s, e) =>
                {
                    open_QA_program(dtFIR_AIR.Rows[e.RowIndex]["InvNo"].ToString(), "heat", dtFIR_AIR.Rows[e.RowIndex]["ReceivingID"].ToString());
                };
                DataGridViewGeneratorTextColumnSettings wash = new DataGridViewGeneratorTextColumnSettings();
                wash.CellMouseDoubleClick += (s, e) =>
                {
                    open_QA_program(dtFIR_AIR.Rows[e.RowIndex]["InvNo"].ToString(), "wash", dtFIR_AIR.Rows[e.RowIndex]["ReceivingID"].ToString());
                };
                #endregion
                //設定gridFir_Laboratory的顯示欄位
                this.gridFir_Laboratory.IsEditingReadOnly = true;
                this.gridFir_Laboratory.DataSource = bsFIR_Laboratory;
                Helper.Controls.Grid.Generator(this.gridFir_Laboratory)
                    //.Text("MDivisionID", header: "M", width: Widths.AnsiChars(5))
                     .Text("InvNo", header: "Invoice#", width: Widths.AnsiChars(20))
                     .Text("ExportId", header: "Wk#", width: Widths.AnsiChars(15))
                     .Date("ETA", header: "ETA", width: Widths.AnsiChars(11))
                     .Numeric("ArriveQty", header: "Total" + Environment.NewLine + "Ship Qty", width: Widths.AnsiChars(6), integer_places: 9, decimal_places: 2)
                     .Date("ReceiveSampleDate", header: "Date", width: Widths.AnsiChars(13))
                     .Text("Crocking", header: "Crocking" + Environment.NewLine + "Test", width: Widths.AnsiChars(8),settings : crocking)
                     .Date("CrockingDate", header: "Crocking Date", width: Widths.AnsiChars(13))
                     .Text("Heat", header: "Heat" + Environment.NewLine + "Shrinkage", width: Widths.AnsiChars(8),settings : heat)
                     .Date("HeatDate", header: "Heat Date", width: Widths.AnsiChars(13))
                     .Text("Wash", header: "Wash" + Environment.NewLine + "Shrinkage", width: Widths.AnsiChars(8),settings : wash)
                     .Date("WashDate", header: "Wash Date", width: Widths.AnsiChars(13))
                     .Text("Oven", header: "Oven" + Environment.NewLine + "Test", width: Widths.AnsiChars(8), settings: wash)
                     .Date("OvenDate", header: "Oven Test Date", width: Widths.AnsiChars(13))
                     .Text("ColorFastness", header: "Color" + Environment.NewLine + "Fastness", width: Widths.AnsiChars(8), settings: wash)
                     .Date("ColorFastnessDate", header: "Color Fastness Date", width: Widths.AnsiChars(13))
                     ;
                     //.Date("ReceiveSampleDate", header: "Date", width: Widths.AnsiChars(13),settings:ds2)
                     //.Text("Crocking", header: "Crocking" + Environment.NewLine + "Test", width: Widths.AnsiChars(8), settings: ts2)
                     //.Date("CrockingDate", header: "Crocking Date", width: Widths.AnsiChars(13), settings: ds2)
                     //.Text("Heat", header: "Heat" + Environment.NewLine + "Shrinkage", width: Widths.AnsiChars(8), settings: ts2)
                     //.Date("HeatDate", header: "Heat Date", width: Widths.AnsiChars(13), settings: ds2)
                     //.Text("Wash", header: "Wash" + Environment.NewLine + "Shrinkage", width: Widths.AnsiChars(8), settings: ts2)
                     //.Date("WashDate", header: "Wash Date", width: Widths.AnsiChars(13), settings: ds2)
                     
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
 from dbo.AIR a WITH (NOLOCK) 
inner join dbo.View_AllReceiving b WITH (NOLOCK) on b.Id= a.ReceivingID
where a.POID='{0}' and a.Seq1 ='{1}' and a.seq2='{2}'", dr["id"], dr["seq1"], dr["seq2"]));
                DataTable dtFIR_Laboratory;
                selectResult1 = DBProxy.Current.Select(null, sqlcmd.ToString(), out dtFIR_Laboratory);
                if (selectResult1 == false) ShowErr(sqlcmd.ToString(), selectResult1);

                bsAIR_FIR.DataSource = dtFIR_Laboratory;

                //設定Grid1的顯示欄位
                this.gridFirAir.IsEditingReadOnly = true;
                this.gridFirAir.DataSource = bsAIR_FIR;
                Helper.Controls.Grid.Generator(this.gridFirAir)
                     //.Text("MDivisionID", header: "M", width: Widths.AnsiChars(5))
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
