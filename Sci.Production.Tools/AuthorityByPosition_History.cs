using System;
using System.Data;
using Ict.Win;

namespace Sci.Production.Tools
{
    public partial class AuthorityByPosition_History : Win.Subs.Input4
    {
        // private DataTable dtPass1 = null;
        // private DualResult result = null;
        public AuthorityByPosition_History(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
        }

        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
               .Text("Modifier", header: "Modifier", width: Widths.AnsiChars(20), iseditingreadonly: true)
               .DateTime("AddDate", header: "Modify Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.MMddyyyyHHmmss)
               .EditText("Remark", header: "Remark", width: Widths.AnsiChars(30), iseditingreadonly: false)
               .Text("Editby", header: "Edit by", width: Widths.AnsiChars(40), iseditingreadonly: true);

            return true;
        }

        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);

            datas.Columns.Add("Modifier");
            datas.Columns.Add("Editby");
            foreach (DataRow dr in datas.Rows)
            {
                dr["Modifier"] = Sci.Production.PublicPrg.Prgs.GetAddOrEditBy(dr["AddName"], format: (int)Sci.Production.PublicPrg.Prgs.Pass1Format.NameExt);
                dr["Editby"] = Sci.Production.PublicPrg.Prgs.GetAddOrEditBy(dr["EditName"], dateColumn: dr["EditDate"], format: (int)Sci.Production.PublicPrg.Prgs.Pass1Format.IDNameExtDateTime);
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }
    }
}
