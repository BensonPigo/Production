using Ict.Win;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Quality
{
    public partial class P15 : Win.Tems.Input6
    {
        // 宣告Context Menu Item
        ToolStripMenuItem add;

        // 宣告Context Menu Item
        ToolStripMenuItem edit;

        // 宣告Context Menu Item
        ToolStripMenuItem delete;

        public P15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.ContextMenuStrip = this.detailgridmenus;
            this.InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = "Type = 'B'";
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmd = string.Format(
@"select 
    md.No,
    md.ReportNo,
    [SubmitDate] = Format(md.SubmitDate,'yyyy/MM/dd'), 
    [Artwork] = Stuff((select distinct concat( ',',mdd.ArtworkTypeID)   from MockupWash_Detail_detail mdd where mdd.ReportNo = md.ReportNo FOR XML PATH('')),1,1,''),
    md.Result,
    [ReceivedDate] = Format(md.ReceivedDate,'yyyy/MM/dd'),
    [ReleasedDate] = Format(md.ReleasedDate,'yyyy/MM/dd'),
    md.Technician,
    [TechnicianName] = TechnicianName.Name_Extno,
    [MRName] = MRName.Name_Extno,
    [LastEditName] = LastEditName.Name + ' ' + Format(md.EditDate,'yyyy/MM/dd HH:mm:ss')
from MockupWash_Detail md WITH (NOLOCK) 
outer apply (select Name_Extno from View_ShowName where id = md.Technician) TechnicianName
outer apply (select Name_Extno from View_ShowName where id = md.MR) MRName
outer apply (select Name from Pass1 where id = md.EditName) LastEditName
where ID = '{0}'",
masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.detailgridmenus.Items.Clear(); // 清空原有的Menu Item
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Create New Test", onclick: (s, e) => this.CreateNewTest()).Get(out this.add);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Record's detail", onclick: (s, e) => this.EditThisDetail()).Get(out this.edit);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Delete this Record's detail", onclick: (s, e) => this.DeleteThisDetail()).Get(out this.delete);
        }

        // Context Menu選擇Create New test
        private void CreateNewTest()
        {
            P15_Detail callNewDetailForm = new P15_Detail(true, this.CurrentMaintain["ID"].ToString(), string.Empty, null, "New");
            callNewDetailForm.ShowDialog(this);
            callNewDetailForm.Dispose();
            this.RenewData();
            this.OnDetailEntered();
        }

        // Context Menu選擇Edit This Record's Detail
        private void EditThisDetail()
        {
            P15_Detail callNewDetailForm = new P15_Detail(true, this.CurrentMaintain["ID"].ToString(), this.CurrentDetailData["ReportNo"].ToString(), null, "Edit");
            callNewDetailForm.ShowDialog(this);
            callNewDetailForm.Dispose();
            this.RenewData();
            this.OnDetailEntered();
        }

        // Context Menu選擇Delete This Record's Detail
        private void DeleteThisDetail()
        {
            if (MyUtility.Msg.QuestionBox("Do you want to delete the data?", buttons: MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string delelete_cmd = $@"delete MockupWash_Detail where ReportNo = '{this.CurrentDetailData["ReportNo"].ToString()}';
                                        delete MockupWash_Detail_Detail where ReportNo = '{this.CurrentDetailData["ReportNo"].ToString()}';";
                DualResult result = DBProxy.Current.Execute(null, delelete_cmd);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                this.RenewData();
            }
        }

        protected override bool OnGridSetup()
        {
            this.detailgrid.IsEditingReadOnly = false;

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("No", header: "No.", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("ReportNo", header: "Report No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SubmitDate", header: "Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Artwork", header: "Artwork", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("Result", header: "Result", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("ReceivedDate", header: "Received Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ReleasedDate", header: "Released Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Technician", header: "Technician", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("TechnicianName", header: "Technician Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("MRName", header: "MR Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("LastEditName", header: "Last Edit Name", width: Widths.AnsiChars(20), iseditingreadonly: true);

            this.detailgrid.CellDoubleClick += (s, e) =>
            {
                if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
                {
                    return;
                }

                P15_Detail callNewDetailForm = new P15_Detail(false, this.CurrentMaintain["ID"].ToString(), this.CurrentDetailData["ReportNo"].ToString(), null, "Query");
                callNewDetailForm.ShowDialog(this);
                callNewDetailForm.Dispose();
            };
            return base.OnGridSetup();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.EditMode)
            {
                this.detailgrid.ContextMenuStrip = null;
            }
            else
            {
                this.detailgrid.ContextMenuStrip = this.detailgridmenus;
            }
        }

        private void txtArticle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string styleUkey;
            styleUkey = MyUtility.GetValue.Lookup($"select ukey from style where ID = '{this.CurrentMaintain["StyleID"].ToString()}' and BrandID = '{this.CurrentMaintain["BrandID"].ToString()}' and SeasonID = '{this.CurrentMaintain["SeasonID"].ToString()}'");
            if (MyUtility.Check.Empty(styleUkey))
            {
                MyUtility.Msg.WarningBox("<Style#><Season><Brand> not found");
                return;
            }

            Win.Tools.SelectItem item;
            string selectCommand = $"select Article,ArticleName from Style_Article WITH (NOLOCK) where StyleUkey = {styleUkey}";

            item = new Win.Tools.SelectItem(selectCommand, "11,33", this.Text);
            item.Width = 520;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtArticle.Text = item.GetSelectedString();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtbrand.ReadOnly = true;
            this.txtstyle.ReadOnly = true;
            this.txtseason.ReadOnly = true;
        }

        private void txtArticle_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtArticle.Text))
            {
                return;
            }

            if (!MyUtility.Check.Seek($"select 1 from Style_Article WITH (NOLOCK) where Article = '{this.txtArticle.Text}' and StyleUkey = (select ukey from style where ID = '{this.CurrentMaintain["StyleID"]}' and BrandID = '{this.CurrentMaintain["BrandID"]}' and SeasonID = '{this.CurrentMaintain["SeasonID"]}')"))
            {
                MyUtility.Msg.WarningBox("Article not found!");
                e.Cancel = true;
                return;
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Type"] = "B";
        }

        protected override bool ClickSaveBefore()
        {
            // 檢查表頭Style#, Season, Brand, Article / Colorway, T1/SubconName必須輸入
            #region 檢查空白欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["StyleID"]))
            {
                MyUtility.Msg.WarningBox("<Style#> can not be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SeasonID"]))
            {
                MyUtility.Msg.WarningBox("<Season> can not be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("<Brand> can not be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Article"]))
            {
                MyUtility.Msg.WarningBox("<Article / Colorway> can not be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["T1Subcon"]))
            {
                MyUtility.Msg.WarningBox("<T1/SubconName> can not be empty.");
                return false;
            }
            #endregion

            #region 檢查欄位正確性

            // Style,Season,Brand
            if (!MyUtility.Check.Seek($"select 1 from Style WITH (NOLOCK) where ID = '{this.CurrentMaintain["StyleID"]}' and BrandID = '{this.CurrentMaintain["BrandID"]}' and SeasonID = '{this.CurrentMaintain["SeasonID"]}'"))
            {
                MyUtility.Msg.WarningBox("Style#, Season, Brand not found!");
                return false;
            }

            // Article
            if (!MyUtility.Check.Seek($"select 1 from Style_Article WITH (NOLOCK) where Article = '{this.txtArticle.Text}' and StyleUkey = (select ukey from style where ID = '{this.CurrentMaintain["StyleID"]}' and BrandID = '{this.CurrentMaintain["BrandID"]}' and SeasonID = '{this.CurrentMaintain["SeasonID"]}')"))
            {
                MyUtility.Msg.WarningBox("Article not found!");
                return false;
            }

            #endregion

            #region 取得表頭ID
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "WA", "MockupWash", DateTime.Now);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["ID"] = tmpId;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickDeletePost()
        {
            DualResult result;
            result = DBProxy.Current.Execute(null, $"delete MockupWash_Detail_Detail where ID = '{this.CurrentMaintain["ID"]}'");
            if (!result)
            {
                this.ShowErr(result);
            }

            return base.ClickDeletePost();
        }
    }
}
