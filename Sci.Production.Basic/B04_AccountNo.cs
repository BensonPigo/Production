using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Basic
{
    public partial class B04_AccountNo : Sci.Win.Subs.Input4
    {
        public B04_AccountNo(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            this.displayBox1.Text = this.KeyValue1;
            this.displayBox2.Text = myUtility.Lookup("Abb", this.KeyValue1, "LocalSupp", "ID");
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("ArtworkType", header: "Type", width: Widths.AnsiChars(10))
                .Text("AccountNo", header: "Account No", width: Widths.AnsiChars(10));

            return true;
        }

        protected override void OnRequeryPost(DataTable datas)
        {
            string selectCommand = string.Format("select a.ID as ArtworkType, a.Seq, b.AccountNo from (select ID,Seq from ArtworkType where IsSubprocess = 1 or Classify = 'P' or Seq BETWEEN 'A' and 'Z') a left join (select ArtworkTypeID,AccountNo from LocalSupp_AccountNo where ID = '{0}') b on a.ID = b.ArtworkTypeID order by a.Seq", this.KeyValue1);
            Ict.DualResult returnResult;
            DataTable ArtworkTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out ArtworkTable))
            {
                base.OnRequeryPost(ArtworkTable);
            }
        }
    }
}
