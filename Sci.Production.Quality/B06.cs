using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B06 : Sci.Win.Tems.Input1
    {
       public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

       protected override void ClickNewAfter()
       {
           this.txtAreaCode.ReadOnly = false;
           this.editDescription.ReadOnly = false;
           this.checkJunk.ReadOnly = false;
           base.ClickNewAfter();
       }

       protected override bool ClickEditBefore()
       {
           this.txtAreaCode.ReadOnly = true;
           this.editDescription.ReadOnly = false;
           return base.ClickEditBefore();
       }

       protected override void ClickUndo()
       {
           this.txtAreaCode.ReadOnly = true;
           base.ClickUndo();
       }

       protected override bool ClickSaveBefore()
       {
           if (MyUtility.Check.Empty(this.txtAreaCode.Text))
           {
               MyUtility.Msg.WarningBox("<Area Code> cannot be empty! ");
               return false;
           }

           return base.ClickSaveBefore();
       }

       protected override Ict.DualResult ClickSave()
       {
           this.txtAreaCode.ReadOnly = true;

           return base.ClickSave();
       }
    }
}
