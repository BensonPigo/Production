using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B06 : Win.Tems.Input1
    {
       public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
       {
           this.txtAreaCode.ReadOnly = false;
           this.editDescription.ReadOnly = false;
           this.checkJunk.ReadOnly = false;
           base.ClickNewAfter();
       }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
       {
           this.txtAreaCode.ReadOnly = true;
           this.editDescription.ReadOnly = false;
           return base.ClickEditBefore();
       }

        /// <inheritdoc/>
        protected override void ClickUndo()
       {
           this.txtAreaCode.ReadOnly = true;
           base.ClickUndo();
       }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
       {
           if (MyUtility.Check.Empty(this.txtAreaCode.Text))
           {
               MyUtility.Msg.WarningBox("<Area Code> cannot be empty! ");
               return false;
           }

           return base.ClickSaveBefore();
       }

        /// <inheritdoc/>
        protected override Ict.DualResult ClickSave()
       {
           this.txtAreaCode.ReadOnly = true;

           return base.ClickSave();
       }
    }
}
