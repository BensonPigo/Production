﻿using System;
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
             this.txtDefectType.ReadOnly = true;
             base.ClickEditAfter();           
                         
         }
         protected override bool ClickNewBefore()
         {
             this.txtDefectType.Text = "";
             return base.ClickNewBefore();
         }
         protected override void ClickNewAfter()
         {
             this.txtDefectType.ReadOnly = true;
             base.ClickNewAfter();
         }
         protected override bool ClickSaveBefore()
         {

             #region 必輸檢查
             if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
             {
                 MyUtility.Msg.WarningBox("< Defect code > can not be empty!");
                 this.txtDefectcode.Focus();
                 return false;
             }
             if (MyUtility.Check.Empty(CurrentMaintain["Description"]))
             {
                 MyUtility.Msg.WarningBox("< Description > can not be empty!");
                 this.editDescription.Focus();
                 return false;
             }

             #endregion

             DataTable dtCode;
             string SQLCmd = "select ID,junk from GarmentDefectType where ID=@ID ";
             System.Data.SqlClient.SqlParameter sq1 = new System.Data.SqlClient.SqlParameter();
             sq1.ParameterName = "@ID";
             sq1.Value = (this.txtDefectcode.Text).ToString().Substring(0, 1);
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
                 this.txtDefectcode.Focus();
                 return false;
             }
             if (dtCode.Rows[0][1].ToString()=="True")
             {
                 MyUtility.Msg.WarningBox("The defect code is a junk ");
                 this.txtDefectcode.Focus();
                 return false;
             }

             return base.ClickSaveBefore();
         }         
        
         private void textBox1_Validated(object sender, EventArgs e)
         {
             if (this.txtDefectcode.Text != "")
             {
                 this.txtDefectType.Text = (this.txtDefectcode.Text).ToString().Substring(0, 1);
                 
             }
         }

    }
}
