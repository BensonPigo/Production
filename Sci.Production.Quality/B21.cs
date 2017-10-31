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

namespace Sci.Production.Quality
{
    public partial class B21 : Sci.Win.Tems.Input1
    {
        DualResult result;
         public B21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();                    
        }

         protected override void ClickEditAfter()
         {
             this.txtDefectcode.ReadOnly = true;         
             base.ClickEditAfter();                                    
         }        

         protected override bool ClickNewBefore()
         {
             this.txtDefectcode.ReadOnly = false;         
             return base.ClickNewBefore();
         }

         protected override bool ClickSaveBefore()
         {
             #region 必輸檢查
             if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
             {
                 this.txtDefectcode.Focus();
                 MyUtility.Msg.WarningBox("< Defect code > can not be empty!");
                 return false;
             }
             if (MyUtility.Check.Empty(CurrentMaintain["Description"]))
             {
                 this.editDescription.Focus();
                 MyUtility.Msg.WarningBox("< Description > can not be empty!");
                 return false;
             }

             #endregion
             return base.ClickSaveBefore();
         }       

         private void txtDefectcode_Validating(object sender, CancelEventArgs e)
         {
            string firstword;
             if (MyUtility.Check.Empty(this.txtDefectcode.Text))
             {
                 txtDefectType.Text = "";
                 return;
             }
            else
            {
                firstword = this.txtDefectcode.Text.Substring(0, 1);
            }
             DataTable dtCode;
             string SQLCmd = string.Format(@"select ID,junk from GarmentDefectType where ID='{0}' ", firstword); 
             if (!(result = DBProxy.Current.Select(null, SQLCmd, out dtCode)))
             {
                 MyUtility.Msg.ErrorBox(result.ToString());
                 return ;
             }
             else
             {
                 if (MyUtility.Check.Empty(dtCode) || dtCode.Rows.Count == 0)
                 {
                     this.txtDefectcode.Text = "";
                     this.txtDefectcode.Focus();
                     e.Cancel = true;
                     MyUtility.Msg.WarningBox(string.Format("<The first word: {0}> does not exist in <GarmentDefectType-ID>", firstword));
                     return;
                 }
                 else if (dtCode.Rows[0]["junk"].ToString() == "1")
                 {
                     this.txtDefectcode.Text = "";
                     this.txtDefectcode.Focus();
                     e.Cancel = true;
                     MyUtility.Msg.WarningBox(string.Format("<Defect code: {0}> junk is true,cannot use it !"), firstword);
                     return;
                 }                           
             }
         }
         
         private void txtDefectcode_Validated(object sender, EventArgs e)
         {
             if (!MyUtility.Check.Empty(this.txtDefectcode.Text))
             {
                 this.txtDefectType.Text = this.txtDefectcode.Text.ToString().Substring(0, 1);
             }
         }

         #region 解決EditBox BindSouce後,會清空txtDefectType
         private void editDescription_Leave(object sender, EventArgs e)
         {
             if (!MyUtility.Check.Empty(this.txtDefectcode.Text))
             {
                 this.txtDefectType.Text = this.txtDefectcode.Text.ToString().Substring(0, 1);
             }
         }

         private void detailcont_MouseDown(object sender, MouseEventArgs e)
         {
             if (!MyUtility.Check.Empty(this.txtDefectcode.Text))
             {
                 this.txtDefectType.Text = this.txtDefectcode.Text.ToString().Substring(0, 1);
             }
         }
        #endregion
    }
}
