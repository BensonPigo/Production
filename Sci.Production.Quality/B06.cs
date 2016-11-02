using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B06 : Sci.Win.Tems.Input1
    {
       public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();       
        }
      
       protected override void ClickNewAfter()
       {
           this.txtID.ReadOnly = false;
           this.txtDesc.ReadOnly = false;
           this.chkJunk.ReadOnly = false;
           base.ClickNewAfter();
       }
       protected override bool ClickEditBefore()
       {
           this.txtID.ReadOnly = true;
           this.txtDesc.ReadOnly = false;
           return base.ClickEditBefore();
           
       }
       protected override void ClickUndo()
       {
           this.txtID.ReadOnly = true;
           base.ClickUndo();
       }
       protected override bool ClickSaveBefore()
       {
           if (MyUtility.Check.Empty(this.txtID.Text))
           {
               MyUtility.Msg.WarningBox("<Area Code> cannot be empty! ");
               return false;
           }
           return base.ClickSaveBefore();
       }
       protected override Ict.DualResult ClickSave()
       {
           this.txtID.ReadOnly = true;
           
           return base.ClickSave();
       }
       
    }
}
