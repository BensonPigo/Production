using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class R15 : Win.Tems.PrintForm
    {
        private string reason;
        private string mdivision;
        private string factory;
        private string stocktype = string.Empty;
        private string spno1;
        private string spno2;
        private DateTime? issueDate1;
        private DateTime? issueDate2;
        private DataTable printData;
        private bool isAutomation;

        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2))
            {
                MyUtility.Msg.WarningBox("< Issue Date > can't be empty!!");
                return false;
            }

            this.issueDate1 = this.dateIssueDate.Value1;
            this.issueDate2 = this.dateIssueDate.Value2;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.reason = this.txtwhseReasonCode.TextBox1.Text;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;

            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string chkAutomation = $@"select 1 from System where Automation = 1";
            DataTable chkDT;
            DualResult result = DBProxy.Current.Select(null, chkAutomation, out chkDT);

            if (chkDT.Rows != null && chkDT.Rows.Count > 0)
            {
                this.isAutomation = true;
            }
            else
            {
                this.isAutomation = false;
            }

            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@Factory";

            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"
select  
        IssueID=a.Id
        ,M=a.MDivisionID 
        ,Factory=orders.FactoryID
        ,CuttingID=a.CutplanID
        ,IssueDate=a.IssueDate
        ,RequestReason = a.WhseReasonID+'-'+ISNULL((select d.Description 
											        from whsereason d WITH (NOLOCK) 
											        WHERE d.id = a.whsereasonid and d.Type = 'IR')
										           ,'')
        ,Encoder = dbo.getPass1(a.EditName) 
        {(this.isAutomation ? ", a.ToPlace " : string.Empty)}
        ,Remark=a.Remark
        ,ApvDate=a.EditDate
        ,a.Status
        ,Createby = dbo.getPass1_ExtNo(a.AddName)
        ,SP=b.POID
        ,b.SEQ1
        ,b.SEQ2
        ,b.Roll
        ,b.Dyelot
        ,Description = dbo.getMtlDesc(b.POID, b.Seq1, b.Seq2, 2, 0)
        ,psd.StockUnit
        ,psd.Refno
        ,psd.ColorID
        ,psd.SizeSpec
        ,MaterialType = Concat (iif(psd.FabricType='F','Fabric',iif(psd.FabricType='A','Accessory',iif(psd.FabricType='O','Orher',psd.FabricType))), '-', Fabric.MtlTypeID)
        ,IssueQty = b.Qty
        ,BulkLocation = dbo.Getlocation(f.Ukey)
from issue as a WITH (NOLOCK) 
inner join issue_detail b WITH (NOLOCK) on a.id = b.id
inner join Orders orders on b.POID = orders.id
left join po_supp_detail psd WITH (NOLOCK) on psd.id = b.poid and psd.seq1 = b.seq1 and psd.seq2 =b.seq2
left join FtyInventory f with (nolock) on f.POID = b.POID and f.SEQ1 = b.Seq1 and f.SEQ2 = b.Seq2  and f.Roll = b.Roll and f.Dyelot = b.Dyelot
left join Fabric  with (nolock) on Fabric.SCIRefno = psd.SCIRefno
where a.type = 'D' AND a.Status = 'Confirmed' 
");

            if (!MyUtility.Check.Empty(this.issueDate1))
            {
                sqlCmd.Append(string.Format(" and '{0}' <= a.issuedate", Convert.ToDateTime(this.issueDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.issueDate2))
            {
                sqlCmd.Append(string.Format(" and a.issuedate <= '{0}'", Convert.ToDateTime(this.issueDate2).ToString("yyyy/MM/dd")));
            }
            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and orders.FactoryID = @Factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.reason))
            {
                sqlCmd.Append(string.Format(@" and A.WhseReasonID = '{0}'", this.reason));
            }

            if (!MyUtility.Check.Empty(this.spno1) && !MyUtility.Check.Empty(this.spno2))
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and b.Poid >= @spno1 and b.Poid <= @spno2");
                sp_spno1.Value = this.spno1.PadRight(10, '0');
                sp_spno2.Value = this.spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(this.spno1))
            {
                // 只有 sp1 輸入資料
                sqlCmd.Append(" and b.Poid like @spno1 ");
                sp_spno1.Value = this.spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(this.spno2))
            {
                // 只有 sp2 輸入資料
                sqlCmd.Append(" and b.Poid like @spno2 ");
                sp_spno2.Value = this.spno2 + "%";
                cmds.Add(sp_spno2);
            }

            #endregion

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            bool s = false;
            if (this.isAutomation)
            {
                s = MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R15_Automation.xltx", 2, true, null, null);
            }
            else
            {
                s = MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R15.xltx", 2, true, null, null);
            }

            return true;
        }
    }
}
