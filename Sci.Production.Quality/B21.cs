using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using Ict;
using Sci.Data;

namespace Sci.Production.Quality
{
    public partial class B21 : Win.Tems.Input1
    {
        DualResult result;

        public B21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultOrder = "DefectTypeID,id";
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
             if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
             {
                 this.txtDefectcode.Focus();
                 MyUtility.Msg.WarningBox("< Defect code > can not be empty!");
                 return false;
             }

             if (MyUtility.Check.Empty(this.CurrentMaintain["Description"]))
             {
                 this.editDescription.Focus();
                 MyUtility.Msg.WarningBox("< Description > can not be empty!");
                 return false;
             }

            #endregion

             if (this.IsDetailInserting)
            {
                int seq = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($"select max(seq) from GarmentDefectCode where GarmentDefectTypeID = '{this.CurrentMaintain["GarmentDefectTypeID"]}' "));
                seq++;
                if (seq > 255)
                {
                    seq = 255;
                }

                this.CurrentMaintain["seq"] = seq;
            }

             return base.ClickSaveBefore();
         }

        private void TxtDefectcode_Validating(object sender, CancelEventArgs e)
         {
            string firstword;
            if (MyUtility.Check.Empty(this.txtDefectcode.Text))
             {
                 this.txtDefectType.Text = string.Empty;
                 return;
             }
            else
            {
                firstword = this.txtDefectcode.Text.Substring(0, 1);
            }

            DataTable dtCode;
            string SQLCmd = string.Format(@"select ID,junk from GarmentDefectType where ID='{0}' ", firstword);
            if (!(this.result = DBProxy.Current.Select(null, SQLCmd, out dtCode)))
             {
                 MyUtility.Msg.ErrorBox(this.result.ToString());
                 return;
             }
             else
             {
                 if (MyUtility.Check.Empty(dtCode) || dtCode.Rows.Count == 0)
                 {
                     this.txtDefectcode.Text = string.Empty;
                     this.txtDefectcode.Focus();
                     e.Cancel = true;
                     MyUtility.Msg.WarningBox(string.Format("<The first word: {0}> does not exist in <GarmentDefectType-ID>", firstword));
                     return;
                 }
                 else if (dtCode.Rows[0]["junk"].ToString() == "1")
                 {
                     this.txtDefectcode.Text = string.Empty;
                     this.txtDefectcode.Focus();
                     e.Cancel = true;
                     MyUtility.Msg.WarningBox(string.Format("<Defect code: {0}> junk is true,cannot use it !"), firstword);
                     return;
                 }
             }
         }

        private void TxtDefectcode_Validated(object sender, EventArgs e)
         {
             if (!MyUtility.Check.Empty(this.txtDefectcode.Text))
             {
                 this.txtDefectType.Text = this.txtDefectcode.Text.ToString().Substring(0, 1);
             }
         }

         #region 解決EditBox BindSouce後,會清空txtDefectType
        private void EditDescription_Leave(object sender, EventArgs e)
         {
             if (!MyUtility.Check.Empty(this.txtDefectcode.Text))
             {
                 this.txtDefectType.Text = this.txtDefectcode.Text.ToString().Substring(0, 1);
             }
         }

        private void Detailcont_MouseDown(object sender, MouseEventArgs e)
         {
             if (!MyUtility.Check.Empty(this.txtDefectcode.Text))
             {
                 this.txtDefectType.Text = this.txtDefectcode.Text.ToString().Substring(0, 1);
             }
         }
        #endregion
    }
}
