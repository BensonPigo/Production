using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P01_EachCons_SwitchWorkOrder : Sci.Win.Subs.Base
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword; 
        private string cuttingid;
        public P01_EachCons_SwitchWorkOrder(string cutid)
        {
            InitializeComponent();
            cuttingid = cutid;
        }

        private void Cancel_But_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OK_But_Click(object sender, EventArgs e)
        {
            DataTable workorder; 
            string cmd="";
            string worktype;
            if (Combination.Checked)
            {
                worktype = "1";
            }
            else
            {
                worktype = "2";
            }
            DualResult worRes = DBProxy.Current.Select(null,string.Format("Select id from workorder where id = '{0}' and cutplanid  != '' ",cuttingid),out workorder);
            if (!worRes)
            {
                ShowErr(worRes);
                return;
            }
            if (workorder.Rows.Count != 0) //若Cutplanid 有值就不可刪除重轉
            {
                MyUtility.Msg.WarningBox("The Work Order already created cutplan, you cann't re-switch to Work Order","Warning");
                return;
            }
            cmd = string.Format(
                    @"Delete Workorder where id='{0}';
                      Delete WorkOrder_Distribute where id='{0}';
                      Delete WorkOrder_SizeRatio where id='{0}';
                      Delete WorkOrder_Estcutdate where id='{0}';
                      Delete WorkOrder_PatternPanel where id='{0}'", cuttingid);
            worRes = DBProxy.Current.Execute(null, cmd);
            if (!worRes)
            {
                ShowErr(cmd, worRes);
                return;
            }

            //若只要有一筆不存在BOF 就不可轉
            cmd = string.Format(@"Select * from Order_EachCons a Left join Order_Bof b on a.id = b.id and a.FabricCode = b.FabricCode Where a.id = '{0}' and b.id is null",cuttingid);

            DataTable bofnullTb;

            worRes = DBProxy.Current.Select(null, cmd, out bofnullTb);
            if (!worRes)
            {
                ShowErr(cmd, worRes);
                return;
            }
            if (bofnullTb.Rows.Count != 0)
            {
                MyUtility.Msg.WarningBox("The Work Order already created cutplan, you cann't re-switch to Work Order", "Warning");
                return;
            }

            string exswitch = string.Format("exec dbo.usp_switchWorkorder '{0}','{1}','{2}','{3}'", worktype, cuttingid, keyWord, loginID);
            DualResult dResult = DBProxy.Current.Execute(null, exswitch);
            if (!dResult)
            {
                ShowErr(exswitch, dResult);
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("Switch Finished");
                this.Close();
            }

        }



    }
}
