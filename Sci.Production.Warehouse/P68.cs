using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
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

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P68 : Win.Tems.QueryForm
    {
        private List<SqlParameter> listsqlParameter = new List<SqlParameter>();
        private string strSQLWher = string.Empty;
        private DataTable dt_Head = new DataTable();
        private DataTable dt_Detail = new DataTable();

        /// <inheritdoc/>
        public P68(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.txtmultifactory1.MDivisionID = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.cbStatus, 1, 1, "All,Preparing,Unroll,Relaxation,Dispatched,Received");
            this.grid2.DataSource = this.bindingDetail;
            this.cutingDate.Value1 = DateTime.Now;
            this.cutingDate.Value2 = DateTime.Now;
            this.txtmultifactory1.Text = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            #region 上面的Grid
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("ID", header: "Cutplan ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Factory", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("POID", header: "POID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("CutCell", header: "Cut\r\nCell", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Date("EstCutdate", header: "Cutting Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("FabricRelaxationID", header: "Relaxation", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Text("NeedUnroll", header: "Need\r\nUnroll", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Relaxtime", header: "Relaxtime\r\n(Hours)", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Status", header: "Status", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Numeric("RequestCons", header: "Request\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
            .Numeric("PreparingCons", header: "Preparing\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
            .Numeric("UnrollCons", header: "Unroll\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
            .Numeric("RelaxationCons", header: "Relaxation\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
            .Numeric("DispatchedCons", header: "Dispatched\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
            .Numeric("ReceivedCons", header: "Received\r\nCons", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
            ;
            #endregion

            #region 下面的Grid
            this.Helper.Controls.Grid.Generator(this.grid2)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(7), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("IssueQty", header: "Issue Qty", width: Widths.AnsiChars(9), decimal_places: 2, iseditingreadonly: true)
            .DateTime("UnrollStartTime", header: "Unroll\r\nStart Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("UnrollEndTime", header: "Unroll\r\nEnd Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("RelaxationStartTime", header: "Relax\r\nStart Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("RelaxationEndTime", header: "Relax\r\nEnd Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("DispatchTime", header: "Dispatch Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .DateTime("FactoryReceivedTime", header: "Factory\r\nReceive Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
            ;
            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listsqlParameter.Clear();
            this.strSQLWher = string.Empty;
            if (MyUtility.Check.Empty (this.cutingDate.Value1) && MyUtility.Check.Empty(this.txtCutplanID.Text))
            {
                MyUtility.Msg.WarningBox("<Cutting Date> and <Cutplan ID> cannot all be empty.");
                return;
            }

            if (!MyUtility.Check.Empty(this.cutingDate.Value1))
            {
                this.listsqlParameter.Add(new SqlParameter("@CuttingDateStart", this.cutingDate.Value1));
                this.listsqlParameter.Add(new SqlParameter("@CuttingDateEnd", this.cutingDate.Value2));
                this.strSQLWher += " and cp.EstCutdate  >= CAST(@CuttingDateStart AS DATE) AND cp.EstCutdate < DATEADD(DAY, 1, CAST(@CuttingDateEnd AS DATE))";
            }

            if (!MyUtility.Check.Empty(this.txtCutplanID.Text))
            {
                this.listsqlParameter.Add(new SqlParameter("@CutPlanID", this.txtCutplanID.Text));
                this.strSQLWher += " and cp.ID = @CutPlanID";
            }

            if (!MyUtility.Check.Empty(this.txtCutCell_Value1.Text))
            {
                this.listsqlParameter.Add(new SqlParameter("@CutCellStart", this.txtCutCell_Value1.Text));
                this.strSQLWher += " and cp.CutCellID >= @CutCellStart";
            }

            if (!MyUtility.Check.Empty(this.txtCutCell_Value2.Text))
            {
                this.listsqlParameter.Add(new SqlParameter("@CutCellEnd", this.txtCutCell_Value2.Text));
                this.strSQLWher += " and cp.CutCellID <= @CutCellEnd";
            }

            if (!MyUtility.Check.Empty(this.txtmultifactory1.Text))
            {
                this.strSQLWher += $" and o.FactoryID in('{string.Join("','", this.txtmultifactory1.Text.Split(',').ToList())}')";
            }

            if (!MyUtility.Check.Empty(this.txtPOID.Text))
            {
                this.listsqlParameter.Add(new SqlParameter("@POID", this.txtPOID.Text));
                this.strSQLWher += " and cp.POID  = @POID";
            }

            string sqlCmd = $@"
			select cp.ID
					, o.FactoryID
					, cp.POID
					, cp.CutCellID
					, cp.EstCutdate
					, psd.Refno
					, [Color] = psdsC.SpecValue
					, rfrt.FabricRelaxationID
					, frlx.NeedUnroll
					, frlx.Relaxtime
					, [Request Cons] = sum (cpdc.Cons)
			into #CutList
			from Cutplan cp
			inner join Cutplan_Detail_Cons cpdc on cp.ID = cpdc.ID
			inner join PO_Supp_Detail psd on cpdc.POID = psd.ID
											and cpdc.SEQ1 = psd.SEQ1
											and cpdc.SEQ2 = psd.SEQ2
			inner join PO_Supp_Detail_Spec psdsC on psd.ID = psdsC.ID
													and psd.SEQ1 = psdsC.Seq1
													and psd.SEQ2 = psdsC.Seq2
													and psdsC.SpecColumnID = 'Color'
			left join SciMES_RefnoRelaxtime rfrt on psd.Refno = rfrt.Refno
			left join SciMES_FabricRelaxation frlx on rfrt.FabricRelaxationID = frlx.ID
			left join Orders o on cp.POID = o.ID
			where cp.Status = 'Confirmed'
			{this.strSQLWher}
			group by cp.ID, o.FactoryID, cp.POID, cp.CutCellID, cp.EstCutdate, psd.Refno, psdsC.SpecValue, rfrt.FabricRelaxationID, frlx.NeedUnroll, frlx.Relaxtime

			/*
				發料準備清單
			*/
			select cl.ID, cl.Refno, cl.Color, cl.NeedUnroll, cl.Relaxtime
					, isud.POID, isud.Seq1, isud.Seq2, isud.Roll, isud.Dyelot, isud.Qty
					, fur.UnrollStartTime, fur.UnrollEndTime
					, [UnrollDone] = IIF (cl.NeedUnroll = 1 and UnrollStatus != '', 1, 0)
					, fur.RelaxationStartTime, fur.RelaxationEndTime
					, [RelaxationDone] = IIF (cl.Relaxtime > 0 and fur.RelaxationStartTime is not null, 1, 0)
					, mmd.DispatchTime, mmd.FactoryReceivedTime
			into #issueDtl
			from #CutList cl
			inner join Issue isu on cl.ID = isu.CutplanID
			inner join Issue_Detail isud on isu.Id = isud.Id
			inner join PO_Supp_Detail psd on isud.POID = psd.ID
											and isud.Seq1= psd.SEQ1
											and isud.Seq2 = psd.SEQ2
											and cl.Refno = psd.Refno
			inner join PO_Supp_Detail_Spec psdsC on psd.ID = psdsC.ID
													and psd.SEQ1 = psdsC.Seq1
													and psd.SEQ2 = psdsC.Seq2
													and psdsC.SpecColumnID = 'Color'
													and cl.Color = psdsC.SpecValue
			inner join WHBarcodeTransaction wbt on isud.Id = wbt.TransactionID
													and isud.ukey = wbt.TransactionUkey
													and wbt.Action = 'Confirm'
			left join Fabric_UnrollandRelax fur on wbt.To_NewBarcode = fur.Barcode
			left join M360MINDDispatch mmd on isud.M360MINDDispatchUkey = mmd.Ukey
			where isu.Status = 'Confirmed'


			/*
				計算各狀態已完成數
				此為彙整總表
			*/
			select cl.ID
					, [Factory] = cl.FactoryID
					, cl.POID
					, [CutCell] = cl.CutCellID
					, cl.EstCutdate
					, cl.Refno
					, cl.Color
					, cl.FabricRelaxationID
					, [NeedUnroll] = IIF (cl.NeedUnroll = 1, 'Y', '')
					, cl.Relaxtime
					, [Status] = case
									when Received.Cons >= Floor (cl.[Request Cons]) then 'Received'
									when Dispatched.Cons >= Floor (cl.[Request Cons]) then 'Dispatched'
									when Relaxation.Cons >= Floor (cl.[Request Cons]) then 'Relaxation'
									when Unroll.Cons >= Floor (cl.[Request Cons]) then 'Unroll'
									else 'Preparing'
								end
					, [RequestCons] = cl.[Request Cons]
					, [PreparingCons] = isnull (Preparing.Cons, 0)
					, [UnrollCons] = isnull (Unroll.Cons, 0)
					, [RelaxationCons] = isnull (Relaxation.Cons, 0)
					, [DispatchedCons] = isnull (Dispatched.Cons, 0)
					, [ReceivedCons] = isnull (Received.Cons, 0)
			from #CutList cl
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
			) Preparing
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
						and idt.UnrollDone = 1
			) Unroll
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
						and idt.RelaxationDone = 1
			) Relaxation
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
						and idt.DispatchTime is not null
			) Dispatched
			outer apply (
				select Cons = SUM (Qty)
				from #issueDtl idt
				where cl.ID = idt.ID
						and cl.Refno = idt.Refno
						and cl.Color = idt.Color
						and idt.FactoryReceivedTime is not null
			) Received
			order by cl.ID, cl.Refno, cl.Color
			/*
				顯示清單
			*/
			select 
			idt.ID, 
			idt.Refno, 
			idt.Color, 
			[Seq] = concat (idt.Seq1, ' ', idt.Seq2),
			idt.Roll,
			idt.Dyelot,
			[IssueQty] = idt.Qty, 
			idt.UnrollStartTime, 
			idt.UnrollEndTime, 
			idt.RelaxationStartTime, 
			idt.RelaxationEndTime, 
			idt.DispatchTime,
			idt.FactoryReceivedTime
			from #issueDtl idt
			order by idt.ID, idt.Refno, idt.Color, idt.Seq1, idt.Seq2, idt.Roll, idt.Dyelot

			drop table #CutList, #issueDtl";

            DualResult dualResult = DBProxy.Current.Select(null, sqlCmd, this.listsqlParameter, out DataTable[] dt);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.dt_Head = dt[0];
            this.dt_Detail = dt[1];
            if (this.dt_Head.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
            }

            this.grid1.DataSource = this.cbStatus.Text == "All" ? this.dt_Head : this.dt_Head.Select($"Status = '{this.cbStatus.Text}'").TryCopyToDataTable(this.dt_Head);
            this.bindingDetail.DataSource = this.dt_Detail;
        }

        private void Grid1_SelectionChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.grid1.DataSource))
            {
                return;
            }

            if (this.dt_Head == null || this.dt_Head.Rows.Count == 0)
            {
                return;
            }

            if (this.dt_Detail == null || this.dt_Detail.Rows.Count == 0)
            {
                return;
            }

            DataRow dr_Value = this.grid1.GetDataRow(this.grid1.GetSelectedRowIndex());

            if (dr_Value != null)
            {
                string strID = string.IsNullOrEmpty(dr_Value["ID"].ToString()) ? string.Empty : dr_Value["ID"].ToString();
                string strRefno = string.IsNullOrEmpty(dr_Value["Refno"].ToString()) ? string.Empty : dr_Value["Refno"].ToString();
                string strColor = string.IsNullOrEmpty(dr_Value["Color"].ToString()) ? string.Empty : dr_Value["Color"].ToString();
                this.bindingDetail.Filter = $"ID = '{strID}' and Refno = '{strRefno}' and Color = '{strColor}'";
            }
            else
            {
                this.bindingDetail.Filter = $"ID = '' and Refno = '' and Color = ''";
            }
        }

        private void CbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dt_Head.Rows.Count == 0)
            {
                return;
            }

            if (this.cbStatus.Text != "All")
            {
                this.grid1.DataSource = this.dt_Head.Select($"Status = '{this.cbStatus.Text}'").TryCopyToDataTable(this.dt_Head); 
            }
            else
            {
                this.grid1.DataSource = this.dt_Head;
            }
        }
    }
}
