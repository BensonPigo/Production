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
             this.textBox1.ReadOnly = true;
             this.textBox3.ReadOnly = true;
             base.ClickEditAfter();           
                         
         }
         protected override bool ClickNewBefore()
         {
             this.textBox3.Text = "";
             return base.ClickNewBefore();
         }
         protected override void ClickNewAfter()
         {
             this.textBox3.ReadOnly = true;
             base.ClickNewAfter();
         }
         private void chk_typeCodeID(object sender, CancelEventArgs e)
         {

             DataTable dtCode;
             string SQLCmd = "select ID from GarmentDefectType where ID=@ID";
             System.Data.SqlClient.SqlParameter sq1 = new System.Data.SqlClient.SqlParameter();
             sq1.ParameterName = "@ID";
             sq1.Value = (this.textBox1).ToString().Substring(1, 1);
             IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
             cmds.Add(sq1);

             if (!(result = DBProxy.Current.Select(null, SQLCmd, cmds, out dtCode)))
             {
                 MyUtility.Msg.ErrorBox(result.ToString());
                 e.Cancel = true;
                 return;
             }
             if (dtCode.Rows.Count == 0)
             {
                 MyUtility.Msg.WarningBox("The first word does not exist in <GarmentDefectType>");
                 e.Cancel = true;
                 return;
             }

         }
         protected override bool ClickSaveBefore()
         {

             #region 必輸檢查
             if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
             {
                 MyUtility.Msg.WarningBox("< Defect code > can not be empty!");
                 this.textBox1.Focus();
                 return false;
             }
             if (MyUtility.Check.Empty(CurrentMaintain["Description"]))
             {
                 MyUtility.Msg.WarningBox("< Description > can not be empty!");
                 this.editBox1.Focus();
                 return false;
             }

             #endregion

             DataTable dtCode;
             string SQLCmd = "select ID,junk from GarmentDefectType where ID=@ID ";
             System.Data.SqlClient.SqlParameter sq1 = new System.Data.SqlClient.SqlParameter();
             sq1.ParameterName = "@ID";
             sq1.Value = (this.textBox1.Text).ToString().Substring(0, 1);
             string aa = sq1.Value.ToString();
             IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
             cmds.Add(sq1);

             if (!(result = DBProxy.Current.Select(null, SQLCmd, cmds, out dtCode)))
             {
                 MyUtility.Msg.ErrorBox(result.ToString());
                 return false;
             }
             if (dtCode.Rows.Count == 0)
             {
                 MyUtility.Msg.WarningBox("The first word does not exist in <GarmentDefectType-ID>");
                 this.textBox1.Focus();
                 return false;
             }
             if (dtCode.Rows[0][1].ToString()=="True")
             {
                 MyUtility.Msg.WarningBox("The defect code is a junk ");
                 this.textBox1.Focus();
                 return false;
             }

             return base.ClickSaveBefore();
         }
         
         private void textBox1_TextChanged(object sender, EventArgs e)
         {
             if (this.textBox1.Text !="")
             {
                 this.textBox3.Text = (this.textBox1.Text).ToString().Substring(0, 1);    
                 
             }
             
         }

         //private void textBox2_Click(object sender, EventArgs e)
         //{
         //    if (this.textBox1.Text != "")
         //    {
         //        this.textBox3.Text = (this.textBox1.Text).ToString().Substring(0, 1);

         //    }
         //}
         //private void textBox2_KeyUp(object sender, KeyEventArgs e)
         //{
          
         //    if (this.textBox1.Text != "")
         //    {
         //        this.textBox3.Text = (this.textBox1.Text).ToString().Substring(0, 1);

         //    }
         //}
    }
}
