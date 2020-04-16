using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class R26 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;
        DateTime? ConfirmedDate1;
        DateTime? ConfirmedDate2;
        string CfmUser;
        string M;
        string Factory;
        string Brand;
        string WK;
        string SP1;
        string SP2;
        bool WHP21only;

        public R26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            ConfirmedDate1 = dateConfirmedDate.Value1;
            ConfirmedDate2 = dateConfirmedDate.Value2;
            CfmUser = txtuser1.TextBox1.Text; 
            M = txtMdivision1.Text;
            Factory = txtfactory1.Text;
            Brand = txtbrand1.Text;
            WK = txtWK.Text;
            SP1 = txtSP1.Text;
            SP2 = txtSP2.Text;
            WHP21only = chkWHP21only.Checked;

            if (MyUtility.Check.Empty(ConfirmedDate1) && MyUtility.Check.Empty(ConfirmedDate2))
            {
                MyUtility.Msg.WarningBox("Confirmed Date date can't be empty!!");
                return false;
            }

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region where
            List<SqlParameter> lis = new List<SqlParameter>();

            string where = "and cast(L.EditDate as date)  between @ConfirmedDate1 and @ConfirmedDate2";
            lis.Add(new SqlParameter("@ConfirmedDate1", ConfirmedDate1));
            lis.Add(new SqlParameter("@ConfirmedDate2", ConfirmedDate2));

            if (!MyUtility.Check.Empty(CfmUser))
            {
                where += "\r\n" + "and L.EditName = @CfmUser";
                lis.Add(new SqlParameter("@CfmUser", CfmUser));
            }
            if (!MyUtility.Check.Empty(M))
            {
                where += "\r\n" + "and L.MDivisionID = @M";
                lis.Add(new SqlParameter("@M", M));
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                where += "\r\n" + "and L.FactoryID = @Factory";
                lis.Add(new SqlParameter("@Factory", Factory));
            }
            if (!MyUtility.Check.Empty(Brand))
            {
                where += "\r\n" + "and O.BrandID = @Brand";
                lis.Add(new SqlParameter("@Brand", Brand));
            }
            if (!MyUtility.Check.Empty(WK))
            {
                where += "\r\n" + "and R.ExportId = @WK";
                lis.Add(new SqlParameter("@WK", WK));
            }
            if (!MyUtility.Check.Empty(SP1))
            {
                where += "\r\n" + "and LD.Poid >= @SP1";
                lis.Add(new SqlParameter("@SP1", SP1));
            }
            if (!MyUtility.Check.Empty(SP2))
            {
                where += "\r\n" + "and LD.Poid <= @SP2";
                lis.Add(new SqlParameter("@SP2", SP2));
            }
            if (WHP21only)
            {
                where += "\r\n" + "and L.Remark Like '%Create from P21.'";
            }
            #endregion

            string sqlcmd = $@"
select
	L.EditDate,
	L.ID,
	LD.Poid,
	SEQ1 = CONCAT(LD.Seq1, '-' + LD.Seq2),
	R.ExportId,
	LD.Roll,
	LD.Dyelot,
	PSD.Refno,
	PSD.ColorID,
	RD.ActualQty,
	Location = STUFF((
				select concat(',',MtlLocationID)
				from FtyInventory_Detail fd with(nolock)
				where fd.Ukey = FI.Ukey
				for xml path('')
			), 1, 1,''),
	RD.Weight,
	RD.ActualWeight,
	Differential = isnull(RD.ActualWeight, 0)- isnull(RD.Weight, 0),
	L.Remark,
	L.EditName,
	L.EditDate,
	MCHandle = CONCAT(MCHandle, '-'+ TP.Name)
from LocationTrans L
inner join LocationTrans_detail LD on L.id=LD.ID
inner join orders O on LD.Poid=O.ID
inner join PO_Supp_Detail PSD on LD.Poid=PSD.ID and LD.Seq1=PSD.SEQ1 and LD.Seq2=PSD.SEQ2
left join FtyInventory FI on LD.FtyInventoryUkey=FI.UKEY
left join TPEPass1 TP on O.MCHandle=TP.ID
left join Receiving_Detail RD on LD.Poid=RD.PoId and LD.Seq1=RD.Seq1 and LD.Seq2=RD.Seq2 and LD.Roll=RD.Roll and LD.Dyelot=RD.Dyelot
left join Receiving R on RD.ID=R.Id
where L.status='Confirmed'
{where}
";
            return DBProxy.Current.Select(null, sqlcmd, lis, out dt);
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(dt.Rows.Count); // 顯示筆數

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.HideWaitMessage();
                return false;
            }

            Excel.Application objApp = new Excel.Application();
            Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R26.xltx", objApp);

            com.WriteTable(dt, 2);

            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
