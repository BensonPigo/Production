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

namespace Sci.Production.PPIC
{
    public partial class P01_CuttingCombo : Sci.Win.Subs.Base
    {
        private string poID;
        public P01_CuttingCombo(string POID)
        {
            InitializeComponent();
            poID = POID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable GridData;
            string sqlCmd = string.Format("select CuttingSP,ID from Orders WITH (NOLOCK) where POID = '{0}'", poID);
            DualResult result = DBProxy.Current.Select(null,sqlCmd, out GridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!" + result.ToString());
            }
            listControlBindingSource1.DataSource = GridData;

            //設定Grid1的顯示欄位
            this.gridCuttingCombo.IsEditingReadOnly = true;
            this.gridCuttingCombo.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridCuttingCombo)
                .Text("CuttingSP", header: "Cutting SP#", width: Widths.AnsiChars(13))
                .Text("ID", header: "SP#", width: Widths.AnsiChars(15));
        }
    }
}
