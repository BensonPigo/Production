using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Planning
{
    /// <summary>
    /// B03_Copy
    /// </summary>
    public partial class B03_Copy : Win.Subs.Base
    {
        private DataRow data;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// B03_Copy
        /// </summary>
        /// <param name="dr">PlanningB03_Copy</param>
        public B03_Copy(DataRow dr)
        {
            this.InitializeComponent();
            this.data = dr;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string strSQLCmd = string.Format(
                @"select 1 as Selected,*
FROM DBO.Style_Artwork WITH (NOLOCK) WHERE StyleUkey={1} and ukey!= {0}",
                this.data["ukey"],
                this.data["styleukey"]);

            DataTable dtArtwork;

            DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
            {
                if (dtArtwork.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = dtArtwork;
            }
            else
            {
                this.ShowErr(strSQLCmd, result);
            }

            this.gridCopyTo.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridCopyTo.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridCopyTo)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
            .Text("Ukey", header: "Ukey", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("artworktypeid", header: "Artwork Type", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("artworkid", header: "Artwork", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("artworkname", header: "Artwork Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("patterncode", header: "Cut Part", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("patterndesc", header: "Cut Part Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("tms", header: "TMS", width: Widths.AnsiChars(8), integer_places: 10, iseditingreadonly: true)
            .Numeric("qty", header: "QTY", width: Widths.AnsiChars(8), integer_places: 10, iseditingreadonly: true)
            .Text("unit", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("cost", header: "Cost", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
            .Text("remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCommit_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            this.gridCopyTo.ValidateControl();

            DataTable dtImport = (DataTable)this.listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtImport.Select("Selected = 1 ");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to commit data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            IList<SqlCommandText> updateCmds = new List<SqlCommandText>();

            for (int i = 0; i < dr2.Length; i++)
            {
                string txttmp = string.Format(
                    @"
merge dbo.style_artwork_quot as t
using (select {0} ukey,localsuppid,currencyid,price,oven,wash,mockup,priceApv,styleukey from dbo.style_artwork_quot WITH (NOLOCK) where styleukey={1} and ukey = {2}) as s
on t.localsuppid = s.localsuppid and t.styleukey = s.styleukey and t.ukey =s.ukey
when matched then
update 
set price = s.price,currencyid = s.currencyid,oven = s.oven, wash = s.wash, mockup = s.mockup, priceapv = s.priceapv
when not matched then
insert (ukey,localsuppid,currencyid,price,oven,wash,mockup,priceApv,styleukey)
values (s.ukey,s.localsuppid,s.currencyid,s.price,s.oven,s.wash,mockup,s.priceApv,s.styleukey)
when not matched by source and t.ukey = {0} then
delete ;",
                    dr2[i]["ukey"],
                    this.data["styleukey"],
                    this.data["ukey"]);
                SqlCommandText tmp = new SqlCommandText(txttmp, null);
                updateCmds.Add(tmp);
            }

            DualResult result;
            if (!(result = DBProxy.Current.Executes(null, updateCmds)))
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Approve data successful.");
            this.Close();
        }
    }
}
