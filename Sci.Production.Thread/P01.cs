using Ict.Win;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01 : Win.Tems.Input8
    {
        private string factory = Sci.Env.User.Factory;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // button1.Enabled = Sci.Production.PublicPrg.Prgs.GetAuthority(loginID, "P01.Thread Color Combination", "CanEdit");
            this.DoSubForm = new P01_Operation();
        }

        private void Buttoncell(object sender, DataGridViewCellEventArgs e)
        {
            DataTable detTable = (DataTable)this.detailgridbs.DataSource;

            P01_Detail p01_Detail = new P01_Detail(this.CurrentMaintain, this.CurrentDetailData, Sci.Production.PublicPrg.Prgs.GetAuthority(this.loginID, "P01.Thread Color Combination", "CanEdit"));
            p01_Detail.ShowDialog();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            this.SubDetailSelectCommand = string.Format(
                @"
select  Operationid
        , DescEN
        , SeamLength  
        ,Frequency
from ThreadColorComb_Operation a WITH (NOLOCK)
     , Operation b WITH (NOLOCK)
where b.Id = a.OperationId and a.id='{0}'",
                this.CurrentDetailData["id"]);

            return base.OnSubDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings operation = new DataGridViewGeneratorTextColumnSettings();

            operation.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this.OpenSubDetailPage();
                }
            };

            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
           .Text("ThreadCombID", header: "Thread Combination", width: Widths.Auto(true), iseditingreadonly: true, settings: operation)
           .Text("MachineTypeid", header: "Machine Type", width: Widths.Auto(true), iseditingreadonly: true)
           .Numeric("ConsPC", header: "Cons/PC (CM)", integer_places: 8, decimal_places: 2, iseditingreadonly: true)

           // .Numeric("Length", header: "Length", width: Widths.Auto(true), integer_places: 9, decimal_places: 2, iseditingreadonly: true)
           .Button(header: "Color Combination", onclick: new EventHandler<DataGridViewCellEventArgs>(this.Buttoncell));
            #region Button也可這樣寫

            // .Button(header: "Color Combination", onclick: (s,e)=>{
            //        if (EditMode)
            // {

            // DataTable detTable = ((DataTable)this.detailgridbs.DataSource);
            //    Form P01_Detail = new Sci.Production.Thread.P01_Detail(CurrentMaintain["Ukey"].ToString(), CurrentMaintain["id"].ToString(), CurrentMaintain["seasonid"].ToString(), CurrentMaintain["brandid"].ToString());
            //    P01_Detail.ShowDialog();
            // }});
            #endregion
            this.btnGenerate.Enabled = Sci.Production.PublicPrg.Prgs.GetAuthority(this.loginID, "P01.Thread Color Combination", "CanEdit");
            this.btnGenerate.Visible = Sci.Production.PublicPrg.Prgs.GetAuthority(this.loginID, "P01.Thread Color Combination", "CanEdit");
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            DataTable detTable = (DataTable)this.detailgridbs.DataSource;
            Form p01_Generate = new P01_Generate(this.CurrentMaintain["Ukey"].ToString(), this.CurrentMaintain["id"].ToString(), this.CurrentMaintain["seasonid"].ToString(), this.CurrentMaintain["brandid"].ToString());
            p01_Generate.ShowDialog();
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override bool ClickCopy()
        {
            P01_CopyTo p01_CopyTo = new P01_CopyTo(this.CurrentMaintain);
            p01_CopyTo.ShowDialog();

            // return base.ClickCopy();
            this.RenewData();
            return true;
        }
    }
}
