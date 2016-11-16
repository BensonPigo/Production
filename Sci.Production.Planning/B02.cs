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


namespace Sci.Production.Planning
{
    public partial class B02 : Sci.Win.Tems.Input1
    {
        bool NeedCheck = false;  //存檔時是否要檢核。新增時要檢核。修改時，若[BEGIN][END]有異動，才要檢核。
        bool BeginEndChange = false;  //紀錄[BEGIN][END]是否有異動。

        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["BeginStitch"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Begin Stitch > can not be empty!");
                this.numericBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["EndStitch"].ToString()))
            {
                MyUtility.Msg.WarningBox("< End Stitch > can not be empty!");
                this.numericBox2.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Batchno"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Batch Number > can not be empty!");
                this.numericBox3.Focus();
                return false;
            }

            #region 新增時要檢核。修改時，若[BEGIN][END]有異動，才要檢核。
            if (this.IsDetailInserting)  //新增存檔
            {
                NeedCheck = true;
            }
            else if (this.IsDetailUpdating)  //修改存檔
            {
                if (BeginEndChange) NeedCheck = true;
            }
            if (!NeedCheck) return true;
            #endregion

            string s1 = "SELECT * FROM [Production].[dbo].[EmbBatch] where  not((BeginStitch > @begin  and BeginStitch > @end) or (EndStitch <  @begin and EndStitch < @end))";

            #region 準備sql參數資料
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@begin";
            sp1.Value = CurrentMaintain["beginstitch"].ToString();

            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            sp2.ParameterName = "@end";
            sp2.Value = CurrentMaintain["Endstitch"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
           
            #endregion

            bool flag = false;
            Sci.Data.DBProxy.Current.Exists(null, s1, cmds,out flag);
            if (flag)
            {
                MyUtility.Msg.WarningBox("This Data ranage already cover existed data");
                return false;
            }
            return base.ClickSaveBefore();
        }

        private void numericBox1_Validating(object sender, CancelEventArgs e)
        {
            string Value = this.numericBox1.Text;
            if (Value != this.numericBox1.OldValue.ToString()) BeginEndChange = true;
        }

        private void numericBox2_Validating(object sender, CancelEventArgs e)
        {
            string Value = this.numericBox2.Text;
            if (Value != this.numericBox2.OldValue.ToString()) BeginEndChange = true;
        }



    }
}
