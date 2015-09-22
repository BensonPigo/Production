using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Win;
using Sci.Data;
using Ict;
using Ict.Win;
using System.Linq;

namespace Sci.Production.Miscellaneous
{
    public partial class P02_Incoming : Sci.Win.Subs.Base
    {
        private string factory = Sci.Env.User.Factory,localSuppid;
        private DataTable gridTable;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable detTable;
        string id, seq1, seq2, miscid, miscreqid,sql;
        public P02_Incoming(string cid, string cseq1, string cseq2, string cmiscid, string cmiscreqid)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            id = cid;
            seq1 = cseq1;
            seq2 = cseq2;
            miscid = cmiscid;
            miscreqid = cmiscreqid;

            if (MyUtility.Check.Empty(id) && MyUtility.Check.Empty(seq1) && MyUtility.Check.Empty(seq2))
            {
                sql = string.Format(@"Select a.*,b.qty
                                    from MiscIn_Detail b,MiscIn a 
                                    where b.miscpoid = '{0}' and b.seq1 = '{1}' and b.seq2 = '{2}'  and a.id = b.id ",
                                     id, seq1, seq2);
            }
            else
            {
                sql = string.Format(@"Select a.*,b.qty 
                                    from MiscIn_Detail b,MiscIn a 
                                    where b.miscpoid = '{0}' and b.miscreqid = '{1}' 
                                    and b.miscid = '{2}' and a.id = b.id",
                                    id, miscreqid, miscid);
            }
            DualResult dResult =  DBProxy.Current.Select(null, sql, out gridTable);

            this.grid1.IsEditingReadOnly = true; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = gridTable;

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("id", header: "In-Coming#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("cDate", header: "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
                .Text("Status", header: "Status", width: Widths.AnsiChars(12), iseditingreadonly: true);
        }       
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
