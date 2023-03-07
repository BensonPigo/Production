﻿using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R26 : Win.Tems.PrintForm
    {
        private DataTable dt;
        private DateTime? ConfirmedDate1;
        private DateTime? ConfirmedDate2;
        private string CfmUser;
        private string M;
        private string Factory;
        private string Brand;
        private string WK;
        private string SP1;
        private string SP2;
        private bool WHP21only;

        /// <inheritdoc/>
        public R26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.ConfirmedDate1 = this.dateConfirmedDate.Value1;
            this.ConfirmedDate2 = this.dateConfirmedDate.Value2;
            this.CfmUser = this.txtuser1.TextBox1.Text;
            this.M = this.txtMdivision1.Text;
            this.Factory = this.txtfactory1.Text;
            this.Brand = this.txtbrand1.Text;
            this.WK = this.txtWK.Text;
            this.SP1 = this.txtSP1.Text;
            this.SP2 = this.txtSP2.Text;
            this.WHP21only = this.chkWHP21only.Checked;

            if (MyUtility.Check.Empty(this.ConfirmedDate1) && MyUtility.Check.Empty(this.ConfirmedDate2))
            {
                MyUtility.Msg.WarningBox("Confirmed Date date can't be empty!!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region where
            List<SqlParameter> lis = new List<SqlParameter>();

            string where = "and cast(L.EditDate as date)  between @ConfirmedDate1 and @ConfirmedDate2";
            lis.Add(new SqlParameter("@ConfirmedDate1", this.ConfirmedDate1));
            lis.Add(new SqlParameter("@ConfirmedDate2", this.ConfirmedDate2));

            if (!MyUtility.Check.Empty(this.CfmUser))
            {
                where += "\r\n" + "and L.EditName = @CfmUser";
                lis.Add(new SqlParameter("@CfmUser", this.CfmUser));
            }

            if (!MyUtility.Check.Empty(this.M))
            {
                where += "\r\n" + "and L.MDivisionID = @M";
                lis.Add(new SqlParameter("@M", this.M));
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                where += "\r\n" + "and L.FactoryID = @Factory";
                lis.Add(new SqlParameter("@Factory", this.Factory));
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                where += "\r\n" + "and O.BrandID = @Brand";
                lis.Add(new SqlParameter("@Brand", this.Brand));
            }

            if (!MyUtility.Check.Empty(this.WK))
            {
                where += "\r\n" + "and R.ExportId = @WK";
                lis.Add(new SqlParameter("@WK", this.WK));
            }

            if (!MyUtility.Check.Empty(this.SP1))
            {
                where += "\r\n" + "and LD.Poid >= @SP1";
                lis.Add(new SqlParameter("@SP1", this.SP1));
            }

            if (!MyUtility.Check.Empty(this.SP2))
            {
                where += "\r\n" + "and LD.Poid <= @SP2";
                lis.Add(new SqlParameter("@SP2", this.SP2));
            }

            if (this.WHP21only)
            {
                where += "\r\n" + "and L.Remark Like '%Create from P21.'";
            }
            #endregion

            string sqlcmd = $@"
select
	EditDate = cast(L.EditDate as date),
	L.ID,
	LD.Poid,
	SEQ1 = CONCAT(LD.Seq1, '-' + LD.Seq2),
	R.ExportId,
	LD.Roll,
	LD.Dyelot,
	PSD.Refno,
	ColorID = isnull(psdsC.SpecValue, ''),
	X.Qty,
    LD.fromLocation,
	Location = STUFF((
				select concat(',',MtlLocationID)
				from FtyInventory_Detail fd with(nolock)
				where fd.Ukey = FI.Ukey
				for xml path('')
			), 1, 1,''),
	X.Weight,
	X.ActualWeight,
	Differential = isnull(X.ActualWeight, 0)- isnull(X.Weight, 0),
	L.Remark,
	EditName=dbo.getPass1(L.EditName),
	L.EditDate,
	MCHandle = dbo.getPass1(MCHandle)
from LocationTrans L
inner join LocationTrans_detail LD on L.id=LD.ID
inner join orders O on LD.Poid=O.ID
inner join PO_Supp_Detail PSD on LD.Poid=PSD.ID and LD.Seq1=PSD.SEQ1 and LD.Seq2=PSD.SEQ2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join FtyInventory FI on LD.FtyInventoryUkey=FI.UKEY
outer  apply(
	select top 1 R.ExportId,Qty=RD.ActualQty,RD.Weight,RD.ActualWeight
	from Receiving_Detail RD
	inner join Receiving R on RD.ID=R.Id
	where LD.Poid=RD.PoId and LD.Seq1=RD.Seq1 and LD.Seq2=RD.Seq2 and LD.Roll=RD.Roll and LD.Dyelot=RD.Dyelot
)R
outer  apply(
	select top 1  *
	from(
		select R.Qty, R.Weight, R.ActualWeight

		union all
		select TD.Qty,TD.Weight,TD.ActualWeight
		from TransferIn_Detail TD
		where LD.Poid=TD.PoId and LD.Seq1=TD.Seq1 and LD.Seq2=TD.Seq2 and LD.Roll=TD.Roll and LD.Dyelot=TD.Dyelot
	)x
	where Qty is not null
)X
where L.status='Confirmed'
{where}
";
            return DBProxy.Current.Select(null, sqlcmd, lis, out this.dt);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dt.Rows.Count); // 顯示筆數

            if (this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.HideWaitMessage();
                return false;
            }

            Excel.Application objApp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Warehouse_R26.xltx", objApp);
            com.UseInnerFormating = false;
            com.WriteTable(this.dt, 2);

            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
