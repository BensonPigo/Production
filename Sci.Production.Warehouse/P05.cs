using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P05 : Sci.Win.Tems.QueryForm
    {


        private string para_Wkno;
        private string para_Spno;
        private string para_Seq1;
        private string para_Seq2;

        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.Query();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Helper.Controls.Grid.Generator(this.grid)
            .Text("ID", header: "WK#", width: Widths.AnsiChars(11))  //0
            .Text("POID", header: "SP#", width: Widths.AnsiChars(6))
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(9), iseditable: false)
            .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(7) )    //3
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(7))    //4
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(7))
            .Numeric("BalanceQty", header: "Balance Qty", width: Widths.AnsiChars(7))
            ;
        }


        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtWkNo.Text))
                para_Wkno = $"AND ed.ID = '{this.txtWkNo.Text}'";
            else
                para_Wkno = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSpNo.Text))
                para_Spno = $"AND ed.POID = '{this.txtSpNo.Text}'";
            else
                para_Spno = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSeq1.Text))
                para_Seq1 = $"AND ed.seq1 = '{this.txtSeq1.Text}'";
            else
                para_Seq1 = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSeq2.Text))
                para_Seq2 = $"AND ed.seq2 = '{this.txtSeq2.Text}'";
            else
                para_Seq2 = string.Empty;

            Query();

        }

        private void Query()
        {
            DataTable dt;
            string sqlCmd = string.Empty;

            sqlCmd += $@"
SELECT 
    ed.ID
    ,ed.PoID
    ,ed.Seq1
    ,ed.Seq2
    ,f.Roll
    ,f.Dyelot
    ,[BalanceQty]=f.InQty-f.OutQty+ f.AdjustQty
    ,[Location]=dbo.Getlocation(f.ukey)

FROM Export_Detail ed
INNER JOIN FtyInventory f ON f.POID= ed.POID
AND f.SEQ1= ed.SEQ1
AND f.SEQ2= ed.SEQ2
AND f.InQty- f.OutQty+ f.AdjustQty>0
WHERE NOT EXISTS (SELECT 1 FROM Orders o WHERE o.POID=ed.PoID)
AND ed.PoID!='' AND Len(ed.poid) <13
{para_Wkno}
{para_Spno}
{para_Seq1}
{para_Seq2}

ORDER BY ed.id DESC, ed.poid , ed.seq1, ed.seq2, f.Roll, f.Dyelot
";

            DBProxy.Current.Select(null, sqlCmd, out dt);
            if (MyUtility.Check.Empty(dt))
            {
                MyUtility.Msg.WarningBox("Data Not Found.");
                this.listControlBindingSource1.DataSource = null;
                return;
            }
            this.listControlBindingSource1.DataSource = dt;
        }
    }
}
