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
            if (flag == false)
            {
                MyUtility.Msg.WarningBox("This Data ranage already cover existed data");
                return false;
            }
            return base.ClickSaveBefore();
        }
    }
}
