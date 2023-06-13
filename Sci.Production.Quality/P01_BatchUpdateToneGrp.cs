using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_BatchUpdateToneGrp : Win.Tems.QueryForm
    {
        private DataRow drMainDr;
        private DataTable dataTable = new DataTable();

        /// <inheritdoc/>
        public P01_BatchUpdateToneGrp(DataRow mainDr)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.drMainDr = mainDr;
            this.txtSP.Text = MyUtility.Convert.GetString(mainDr["ID"]);
            string sqlcmd = $@"
            DECLARE @ID as varchar(25) = '{MyUtility.Convert.GetString(mainDr["ID"])}'
            SELECT RefColor = IIF(f.Refno = '', 'NULL', isnull(f.Refno,'NULL')) + ' / ' +  IIF(psdsC.SpecValue  = '', 'NULL', isnull(psdsC.SpecValue ,'NULL'))
            FROM FIR f
            INNER JOIN PO_Supp_Detail psd WITH (NOLOCK) ON f.POID = psd.ID AND f.SEQ1 = psd.SEQ1 AND f.SEQ2 = psd.SEQ2
            left join Receiving r with (nolock) on r.id = f.ReceivingID
            left JOIN Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) ON psdsC.ID = psd.ID AND psdsC.SEQ1 = psd.SEQ1 AND psdsC.SEQ2 = psd.SEQ2 AND psdsC.SpecColumnID = 'Color'
            WHERE psd.ID = @ID 
            union
            SELECT RefColor = IIF(f.Refno = '', 'NULL', isnull(f.Refno,'NULL')) + ' / ' +  IIF(psdsC.SpecValue  = '', 'NULL', isnull(psdsC.SpecValue ,'NULL'))
            from FIR f
            inner join PO_Supp_Detail psd with (nolock) on (f.POID = psd.StockPOID and f.SEQ1 = psd.StockSeq1 and f.SEQ2 = psd.StockSeq2)
            inner join Receiving r with (nolock) on r.id = f.ReceivingID
            inner JOIN Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
            where psd.ID = @ID      
            ";
            DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            MyUtility.Tool.SetupCombox(this.cbRefColor, 1, dt);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorTextColumnSettings col_toneGrp = new DataGridViewGeneratorTextColumnSettings();
            col_toneGrp.CellEditable += (s, e) =>
            {
                DataRow dr = this.grids.GetDataRow(e.RowIndex);
                e.IsEditable = MyUtility.Convert.GetString(dr["IsStock"]) == "V" ? true : false;
            };
            this.grids.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grids)
                .Text("WK", header: "WK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("StockFromSP", header: "Stock From SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StockFromSEQ", header: "Stock From SEQ", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Ref", header: "REF#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Color", header: "Color#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("TicketYds", header: "Ticket Yds", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Scale", header: "Scale", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Result", header: "Result", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("ToneGrp", header: "Tone/Grp", width: Widths.AnsiChars(5), settings: col_toneGrp)
                .Text("InspDate", header: "Insp.Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Name", header: "Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(25));
            this.Find();
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private void Find()
        {
            if (MyUtility.Check.Empty(this.cbRefColor.Text))
            {
                return;
            }

            string[] parts = this.cbRefColor.Text.Split('/');  // 使用 "/" 字元分割字串
            string strRef = parts[0].Trim();  // 取得第一部分並去除前後空白
            string strColor = parts[1].Trim();  // 取得第二部分並去除前後空白
            string sqlcmd = $@"
            DECLARE @ID as varchar(25) = '{MyUtility.Convert.GetString(this.drMainDr["ID"])}'
            DECLARE @Ref as varchar(25) = '{strRef}'
            DECLARE @Color as varchar(25) = '{strColor}'
            Select 
            * 
            into #tmpShadebondMain
            from
            (
	            select  
			            psd.ID,
			            [WK#] = r.ExportId,
			            [Seq] = CONCAT(psd.Seq1, '-', psd.Seq2),
			            psd.Seq1,
			            psd.Seq2 ,
			            [StockFromSP#] = psd.StockPOID,
			            [StockFromSEQ] = CONCAT(psd.StockSeq1, '-', psd.StockSeq2),
			            [Ref#]=f.Refno,
			            [Color#] = isnull(psdsC.SpecValue ,''),
			            [fID] =f.ID,
			            f.ReceivingID,
						psd.StockSeq1,
						psd.StockSeq2,
			            [IsStock] = 'X'
	            from  FIR  f with (nolock)
	            inner join PO_Supp_Detail psd with (nolock) on (f.POID = psd.ID and f.SEQ1 = psd.SEQ1 and f.SEQ2 = psd.SEQ2)
	            left join Receiving r with (nolock) on r.id = f.ReceivingID
	            left JOIN Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	            where psd.ID = @ID and psd.Refno = @Ref and psdsC.SpecValue = @Color
	            union
		            select  
			            psd.ID,
			            [WK#] = r.ExportId,
			            [Seq] = CONCAT(psd.Seq1, '-', psd.Seq2),
			            psd.Seq1,
			            psd.Seq2 ,
			            [StockFromSP#] = psd.StockPOID,
			            [StockFromSEQ] = CONCAT(psd.StockSeq1, '-', psd.StockSeq2),
			            [Ref#]=f.Refno,
			            [Color#] = isnull(psdsC.SpecValue ,''),
			            [fID] = f.ID,
			            f.ReceivingID,
						psd.StockSeq1,
						psd.StockSeq2,
			            [IsStock] = 'V'
	            from  FIR  f with (nolock)
	            inner join PO_Supp_Detail psd with (nolock) on (f.POID = psd.StockPOID and f.SEQ1 = psd.StockSeq1 and f.SEQ2 = psd.StockSeq2)
	            inner join Receiving r with (nolock) on r.id = f.ReceivingID
	            inner JOIN Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	            where psd.ID = @ID and psd.Refno = @Ref and psdsC.SpecValue = @Color
            )tmp

            select  * from #tmpShadebondMain

			select
			*
			from
			(
				select  
				[WK] = t.WK# ,
				[SEQ] = Seq ,
				[StockFromSP] = t.StockFromSP# ,
				[StockFromSEQ] = t.StockFromSEQ ,
				[Ref] = t.Ref# ,
				[Color] = t.Color# ,
				[Roll] = fs.Roll ,
				[Dyelot] = fs.Dyelot ,
				[TicketYds] = fs.TicketYds ,
				[Scale] = fs.Scale ,
				[Reult] = fs.Result ,
				[ToneGrp]  = fi.tone ,
				[InspDate] = fs.Inspdate ,
				[Inspector] = fs.Inspector ,
				[Name] = p.[Name],
				[Remark] = fi.Remark,
				[StockType] = rd.StockType ,
				[RefColor] =  IIF(t.Ref# = '', 'NULL', isnull(t.Ref#,'NULL')) + ' / ' +  IIF(t.Color#  = '', 'NULL', isnull(t.Color# ,'NULL')),
				[POID] = iif(t.IsStock = 'V' ,t.StockFromSP#,t.ID),
				[Seq1] = iif(t.IsStock = 'V',t.StockSeq1,t.Seq1) ,
				[Seq2] = iif(t.IsStock = 'V' , t.StockSeq2 , t.Seq2),
				[IsStock] = t.IsStock 
				from FIR_Shadebone fs with (nolock)
				left join Pass1 p with (nolock) on p.ID =  fs.Inspector
				inner join #tmpShadebondMain t on t.fID = fs.ID
				inner join Receiving_Detail rd on rd.PoId = iif(t.IsStock = 'V' ,t.StockFromSP#,t.ID) and rd.Seq1 = iif(t.IsStock = 'V',t.StockSeq1,t.Seq1) and rd.Seq2 = iif(t.IsStock = 'V' , t.StockSeq2 , t.Seq2) and rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
				inner join FtyInventory fi with(nolock) on fi.POID = rd.PoId and fi.Seq1 = rd.Seq1 and fi.Seq2 = rd.Seq2 and fi.Roll = rd.Roll and fi.Dyelot = rd.Dyelot and fi.StockType = rd.StockType
				union
				select  
				[WK] = t.WK# ,
				[SEQ] = Seq ,
				[StockFromSP] = t.StockFromSP# ,
				[StockFromSEQ] = t.StockFromSEQ ,
				[Ref] = t.Ref# ,
				[Color] = t.Color# ,
				[Roll] = fs.Roll ,
				[Dyelot] = fs.Dyelot ,
				[TicketYds] = fs.TicketYds ,
				[Scale] = fs.Scale ,
				[Reult] = fs.Result ,
				[ToneGrp]  = fi.tone ,
				[InspDate] = fs.Inspdate ,
				[Inspector] = fs.Inspector ,
				[Name] = p.[Name],
				[Remark] = fi.Remark,
				[StockType] = rd.StockType ,
				[RefColor] =  IIF(t.Ref# = '', 'NULL', isnull(t.Ref#,'NULL')) + ' / ' +  IIF(t.Color#  = '', 'NULL', isnull(t.Color# ,'NULL')),
				[POID] = iif(t.IsStock = 'V' ,t.StockFromSP#,t.ID),
				[Seq1] = iif(t.IsStock = 'V',t.StockSeq1,t.Seq1) ,
				[Seq2] = iif(t.IsStock = 'V' , t.StockSeq2 , t.Seq2),
				[IsStock] = t.IsStock
				from FIR_Shadebone fs with (nolock)
				left join Pass1 p with (nolock) on p.ID =  fs.Inspector
				inner join #tmpShadebondMain t on t.fID = fs.ID
				inner join TransferIn_Detail rd on rd.PoId = iif(t.IsStock = 'V' ,t.StockFromSP#,t.ID) and rd.Seq1 = iif(t.IsStock = 'V',t.StockSeq1,t.Seq1) and rd.Seq2 = iif(t.IsStock = 'V' , t.StockSeq2 , t.Seq2) and rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
				inner join FtyInventory fi with(nolock) on fi.POID = rd.PoId and fi.Seq1 = rd.Seq1 and fi.Seq2 = rd.Seq2 and fi.Roll = rd.Roll and fi.Dyelot = rd.Dyelot and fi.StockType = rd.StockType
			)tmp1
			order by Roll,Dyelot
            drop table #tmpShadebondMain";

            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dt);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.dataTable.Clear();
            if (dt[1] == null)
            {
                return;
            }

            this.dataTable = dt[1];
            this.grids.DataSource = dt[1];
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.dataTable.Rows.Count == 0)
            {
                return;
            }

            string sqlCmd = $@"update f
                               set f.Remark = t.Remark,f.Tone = t.ToneGrp
                               from #tmp t with(nolock)
                               inner join FtyInventory f with(nolock) on t.poid = f.poid and 
                                                                         t.seq1 = f.seq1 and
                                                                         t.seq2 = f.seq2 and
                                                                         t.Roll = f.Roll and
                                                                         t.Dyelot = f.Dyelot and 
                                                                         f.StockType = t.StockType";

            DualResult dualResult = MyUtility.Tool.ProcessWithDatatable(this.dataTable, null, sqlCmd, out DataTable dataTable);
            if (!dualResult)
            {
                MyUtility.Msg.ErrorBox(dualResult.ToString());
                return;
            }

        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
