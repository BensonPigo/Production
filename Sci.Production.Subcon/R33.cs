using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R33 : Sci.Win.Tems.PrintForm
    {
        string Factory, SewingStart, SewingEnd, SP;
        public R33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.print.Visible = false;
            
            //set ComboFactory
            DataTable dtFactory;
            DBProxy.Current.Select(null, "Select ID from Factory", out dtFactory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, dtFactory);
            comboFactory.Text = Sci.Env.User.Factory;
        }

        protected override bool ValidateInput()
        {
            #region set Data
            Factory = comboFactory.Text;
            SewingStart = ((DateTime)dateSewingDate.Value1).ToString("yyyy-MM-dd");
            SewingEnd = ((DateTime)dateSewingDate.Value2).ToString("yyyy-MM-dd");
            SP = txtSP.Text;
            #endregion 

            #region CheckSewingDate
            if (SewingStart.Empty() && SewingEnd.Empty())
            {
                MyUtility.Msg.InfoBox("Sewing Date can't be empty.");
                return false;
            }
            #endregion
            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {

            return Result.True;
        }

        protected override bool ToExcel()
        {
            base.ToExcel();
            return true;
        }
    }
}
