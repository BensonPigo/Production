using Ict.Win;

namespace Sci.Production.Cutting
{
    public partial class P02_PatternPanel : Win.Subs.Input8A
    {
        public P02_PatternPanel()
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.prev.Visible = false;
            this.next.Visible = false;
        }

        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
              .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(2))
              .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.AnsiChars(2));
            return true;
        }
    }
}
