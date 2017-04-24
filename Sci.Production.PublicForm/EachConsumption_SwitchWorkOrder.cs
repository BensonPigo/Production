﻿using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Transactions;


namespace Sci.Production.PublicForm
{
    public partial class EachConsumption_SwitchWorkOrder : Sci.Win.Subs.Base
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword; 
        private string cuttingid;

        public EachConsumption_SwitchWorkOrder(string cutid)
        {
            InitializeComponent();
            cuttingid = cutid;

            //589:CUTTING_P01_EachConsumption_SwitchWorkOrder，(1) 若Orders.IsMixmarker=true則只能選第1個選項，第2個選項要disable。
            string sql = string.Format("SELECT IsMixmarker FROM Orders WITH (NOLOCK) WHERE ID='{0}'", cuttingid);
            bool IsMixmarker = Convert.ToBoolean(MyUtility.GetValue.Lookup(sql));
            if (IsMixmarker) BYSP.Enabled = false;

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
                worktype = "1";
            else
                worktype = "2";

            #region 檢核

            #region 若只要有一筆不存在BOF就不可轉
            cmd = string.Format(@"Select * from Order_EachCons a WITH (NOLOCK) Left join Order_Bof b WITH (NOLOCK) on a.id = b.id and a.FabricCode = b.FabricCode Where a.id = '{0}' and b.id is null", cuttingid);
            DataTable bofnullTb;
            DualResult worRes = DBProxy.Current.Select(null, cmd, out bofnullTb);
            if (!worRes)
            {
                ShowErr(cmd, worRes);
                return;
            }
            if (bofnullTb.Rows.Count != 0)
            {
                MyUtility.Msg.WarningBox("Can't find BOF data !!", "Warning");
                return;
            }
            #endregion

            #region 若Cutplanid有值就不可刪除重轉
            worRes = DBProxy.Current.Select(null, string.Format("Select id from workorder WITH (NOLOCK) where id = '{0}' and cutplanid  != '' ", cuttingid), out workorder);
            if (!worRes)
            {
                ShowErr(worRes);
                return;
            }
            if (workorder.Rows.Count != 0)
            {
                MyUtility.Msg.WarningBox("The Work Order already created cutplan, you cann't re-switch to Work Order !!", "Warning");
                return;
            }
            else
            {
                DialogResult buttonResult = MyUtility.Msg.WarningBox("Data exists, do you want to over-write work order data?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == DialogResult.No) return;
            }
            #endregion

            #endregion

            #region transaction
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    cmd = string.Format(
                    @"Delete Workorder where id='{0}';
                      Delete WorkOrder_Distribute where id='{0}';
                      Delete WorkOrder_SizeRatio where id='{0}';
                      Delete WorkOrder_Estcutdate where id='{0}';
                      Delete WorkOrder_PatternPanel where id='{0}'", cuttingid);
                    if (!(worRes = DBProxy.Current.Execute(null, cmd)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(cmd, worRes);
                        return;
                    }

                    string exswitch = string.Format("exec dbo.usp_switchWorkorder '{0}','{1}','{2}','{3}'", worktype, cuttingid, keyWord, loginID);
                    DualResult dResult = DBProxy.Current.Execute(null, exswitch);
                    if (!dResult)
                    {
                        _transactionscope.Dispose();
                        ShowErr(exswitch, dResult);
                        return;
                    }
                    
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Switch successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            #endregion

            this.Close();
        }


    }
}
