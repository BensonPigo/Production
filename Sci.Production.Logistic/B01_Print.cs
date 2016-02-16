using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Logistic
{
    public partial class B01_Print : Sci.Win.Tems.PrintForm
    {
        DataRow masterData;
        string code1, code2;
        DataTable printData;
        public B01_Print(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
            textBox1.Text = MyUtility.Convert.GetString(masterData["ID"]);
            textBox2.Text = MyUtility.Convert.GetString(masterData["ID"]);
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            code1 = textBox1.Text;
            code2 = textBox2.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format("select ID from ClogLocation where MDivisionID = '{0}' and Junk = 0", Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(code1))
            {
                sqlCmd.Append(string.Format(" and ID >= '{0}'", code1));
            }
            if (!MyUtility.Check.Empty(code2))
            {
                sqlCmd.Append(string.Format(" and ID <= '{0}'", code2));
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                return result;
            }
            
            e.Report.ReportDataSource = printData;

            return Result.True;
        }
    }
}
