using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Planning
{
    public partial class B03_Copy : Sci.Win.Subs.Base
    {
        DataRow data;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public B03_Copy(DataRow dr)
        {
            InitializeComponent();
            data = dr;
        }
        
        // Grid 設定
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string strSQLCmd = string.Format(@"select 1 as Selected,*
FROM DBO.Style_Artwork WHERE StyleUkey={1} and ukey!= {0}", data["ukey"], data["styleukey"]);
                
                DataTable dtArtwork;

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtArtwork;
                }
                else { ShowErr(strSQLCmd, result); }


            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("Ukey", header: "Ukey", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("artworktypeid", header: "Artwork Type", width: Widths.AnsiChars(16), iseditingreadonly: true)  //2
            .Text("article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)  //3
            .Text("artworkid", header: "Artwork", width: Widths.AnsiChars(10), iseditingreadonly: true)  //4
            .Text("artworkname", header: "Artwork Name", width: Widths.AnsiChars(20), iseditingreadonly: true)  //5
            .Text("patterncode", header: "Cut Part", width: Widths.AnsiChars(6), iseditingreadonly: true)  //6
            .Text("patterndesc", header: "Cut Part Name", width: Widths.AnsiChars(20), iseditingreadonly: true)  //7
            .Numeric("tms", header: "TMS", width: Widths.AnsiChars(8), integer_places: 10, iseditingreadonly: true)  //8
            .Numeric("qty", header: "QTY", width: Widths.AnsiChars(8), integer_places: 10, iseditingreadonly: true)  //9
            .Text("unit", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)  //10
            .Numeric("cost", header: "Cost", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 10, iseditingreadonly: true)  //11
            .Text("remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)  //12
                 ;//10

            // 全選
            checkBox1.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetCheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };

            // 全不選
            checkBox2.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetUncheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Commit
        private void btnApprove_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            grid1.ValidateControl();

            DataTable dtImport = (DataTable)listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0) return;

            DataRow[] dr2 = dtImport.Select("Selected = 1 ");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to commit data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;

            IList<SqlCommandText> updateCmds = new List<SqlCommandText>();

            for (int i = 0; i < dr2.Length; i++)
			{
                SqlCommandText tmp = new SqlCommandText(string.Format(@"merge dbo.style_artwork_quot as t
using (select {0} ukey,localsuppid,currencyid,price,oven,wash,mockup,priceApv,styleukey from dbo.style_artwork_quot where styleukey={1} and ukey = {2}) as s
on t.localsuppid = s.localsuppid and t.styleukey = s.styleukey and t.ukey =s.ukey
when matched then
update 
set price = s.price,currencyid = s.currencyid,oven = s.oven, wash = s.wash, mockup = s.mockup, priceapv = s.priceapv
when not matched then
insert (ukey,localsuppid,currencyid,price,oven,wash,mockup,priceApv,styleukey)
values (s.ukey,s.localsuppid,s.currencyid,s.price,s.oven,s.wash,mockup,s.priceApv,s.styleukey)
when not matched by source and t.ukey = {0} then
delete ;", dr2[i]["ukey"], data["styleukey"],data["ukey"]), null);
                updateCmds.Add(tmp);
			}

            DualResult result;
            if (!(result = DBProxy.Current.Executes(null, updateCmds)))
            {
                ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Approve data successful.");
            this.Close();
        }
    }

   
}
