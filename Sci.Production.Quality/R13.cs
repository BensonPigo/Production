using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R13 : Win.Tems.PrintForm
    {
        private string wkNO;
        private string receivingID;
        private string arrDate;
        private string report_Type;

        /// <inheritdoc/>
        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.txtWK.Text) || MyUtility.Check.Empty(this.txtReceivingID.Text) || MyUtility.Check.Empty(this.dateArrWH.Text))
            {
                MyUtility.Msg.InfoBox("WK# , Receiving ID, Arrive W/H Date can't be empty!!");
                return false;
            }

            this.wkNO = this.txtWK.Text;
            this.receivingID = this.txtReceivingID.Text;
            this.arrDate = this.dateArrWH.Text;

            if (this.radio4Slot.Checked)
            {
                this.report_Type = "4Slot";
            }
            else if (this.radio8Slot.Checked)
            {
                this.report_Type = "8Slot";
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult r = new DualResult(true);
            string cmd = string.Empty;

            DataTable head;

            cmd = $@"
SELECT 
    a.id,
    a.poid,
    a.SEQ1,
    a.SEQ2,
    Receivingid,
    a.Refno,
    a.SCIRefno,
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
    a.Status,
    ReplacementReportID,
    (a.seq1+a.seq2) as seq,
    (Select weavetypeid from Fabric b WITH (NOLOCK) where b.SCIRefno =a.SCIrefno) as weavetypeid,
    c.Exportid,
    [whseArrival] = isnull(c.whseArrival, ti.IssueDate),
    dbo.getPass1(a.Approve) as approve1,
    approveDate,
    approve,
    d.ColorID,
    (Select ID+' - '+ AbbEn From Supp WITH (NOLOCK) Where a.suppid = supp.id) as SuppEn,
    c.ExportID as Wkno
    ,cn.name
    ,a.nonOdor
    ,a.Odor
    ,a.OdorEncode
    ,a.OdorDate
    ,a.nonMoisture
    ,a.Moisture
    ,a.MoistureDate
    ,[PhysicalInspector] = (select name from pass1 where id = a.PhysicalInspector)
    ,[WeightInspector] = (select name from pass1 where id = a.WeightInspector)
    ,[ShadeboneInspector] = (select name from pass1 where id = a.ShadeboneInspector)
    ,[ContinuityInspector] = (select name from pass1 where id = a.ContinuityInspector)
    ,[OdorInspector] = (select name from pass1 where id = a.OdorInspector)
	,Moisture,
	MoistureDate ,
	MaterialCompositionGrp,
	MaterialCompositionItem,
	MoistureStandardDesc,
	MoistureStandard1,
	MoistureStandard2,
	MoistureStandard1_Comparison,
	MoistureStandard2_Comparison
FROM FIR a WITH (NOLOCK) 
Left join Receiving c WITH (NOLOCK) on c.ID = a.ReceivingID
Left join TransferIn ti WITH (NOLOCK) on ti.id = a.receivingid
inner join PO_Supp_Detail d WITH (NOLOCK) on d.id = a.poid and d.seq1 = a.seq1 and d.seq2 = a.seq2
outer apply(select name from color WITH (NOLOCK) where color.id = d.colorid and color.BrandId = d.BrandId)cn
WHERE c.ExportID='{this.wkNO}'
AND c.WhseArrival='{this.arrDate}'
AND a.ReceivingID='{this.receivingID}'
";

            r = DBProxy.Current.Select(string.Empty, cmd, out head);
            if (!r)
            {
                this.ShowErr(r);
                return r;
            }

            foreach (DataRow mainDr in head.Rows)
            {
                string txtsupplier = string.Empty;
                string displayRefno = string.Empty;


            }

            if (this.report_Type == "4Slot")
            {

                DualResult result = DBProxy.Current.Select(string.Empty, cmd, out DataTable dt_title);

                // 抓Invo,ETA 資料
                cmd = $"select id,Eta from Export where id='{this.wkNO}' ";
                result = DBProxy.Current.Select(string.Empty, cmd, out DataTable dt_Exp);
                if (!result)
                {
                    this.ShowErr(result);
                    return result;
                }


                // 變數區
                string title = dt_title.Rows.Count == 0 ? string.Empty : dt_title.Rows[0]["NameEN"].ToString();
                string suppid = this.txtsupplier.TextBox1.Text + " - " + this.txtsupplier.DisplayBox1.Text;
                string invno = dt_Exp.Rows.Count == 0 ? string.Empty : dt_Exp.Rows[0]["ID"].ToString();
                string brandID = MyUtility.GetValue.Lookup($"SELECT BrandID FROM Orders WHERE ID = '{this.displaySP.Text}'");

            }

            if (this.report_Type == "8Slot")
            {

            }

            return r;
        }
    }
}
