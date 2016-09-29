using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class B04 : Sci.Win.Tems.Input1
    {
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        //存檔前檢查
        protected override bool ClickSaveBefore()
        {
            DataTable reason;
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"SELECT max ([ID])
  FROM [Production].[dbo].[Reason]
  WHERE ID <'a' AND  ReasonTypeID like 'damage%'"));
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out reason);

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Name"].ToString()))
            {
                MyUtility.Msg.WarningBox("< editBox1 > can not be empty!");
                this.editBox1.Focus();
                return false;
            }
            
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                CurrentMaintain["ID"] = int.Parse(reason.Rows[0][0].ToString()) + 1;
            }
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ReasonTypeID"].ToString()))
            {
                CurrentMaintain["ReasonTypeID"] = "Damage Reason";

            }
            return base.ClickSaveBefore();
        }
    }
}
