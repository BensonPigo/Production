using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using System.Linq;

using Ict;
//using Ict.Win;
//using Sci.Data;
//using Sci.Production.PublicPrg;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Transactions;
//using System.Windows.Forms;


namespace Sci.Production.PPIC
{
    public partial class B08 : Sci.Win.Tems.Input6
    {
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {

            string sqlCommand = "select UseAPS from factory where ID = '" + Sci.Env.User.Factory + "'";
            string useAPS = MyUtility.GetValue.Lookup(sqlCommand, null);
            if (useAPS.ToUpper() == "TRUE")
            {
                IsSupportCopy = false;
                IsSupportDelete = false;
                IsSupportEdit = false;
                IsSupportNew = false;
            }

            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "' OR FactoryID = 'ALL' ";

            //按Enter時自動跳到下一筆Record，若以已沒有資料了就新增一筆Record
            detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    this.OnDetailGridInsert();
                }
            };

        }

        protected override void OnDetailGridSetup()
        {
            
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Numeric("Day",header:"Day", iseditingreadonly:true)
                .Numeric("Efficiency", header: "Efficiency (%)",iseditingreadonly: false);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["ID"] = DBNull.Value;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;

            //Detail資料自動新增10筆Record，Day填入1~10
            this.OnDetailGridInsert();
            this.OnDetailGridInsert();
            this.OnDetailGridInsert();
            this.OnDetailGridInsert();
            this.OnDetailGridInsert();
            this.OnDetailGridInsert();
            this.OnDetailGridInsert();
            this.OnDetailGridInsert();
            this.OnDetailGridInsert();
            this.OnDetailGridInsert();

        }

        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index); //先給一個NewKey
            int maxkey;

            object comput = ((DataTable)detailgridbs.DataSource).Compute("Max(Day)", "");
            if (comput == DBNull.Value) maxkey = 1;
            else maxkey = Convert.ToInt32(comput);
            maxkey = maxkey + 1;
            CurrentDetailData["Day"] = maxkey;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
            this.txtFactory.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 1.檢查Id跟Factory不可為空值
            if (MyUtility.Check.Empty(CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< ID > can not be empty!");
                this.txtID.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["FactoryID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Factory > can not be empty!");
                this.txtFactory.Focus();
                return false;
            }
            #endregion

            #region 2.表身資料從後面開始檢查，將效率為0的自動刪除，直到有效率值才停止
            DataTable dtDetail = (DataTable)detailgridbs.DataSource;
            for (int i = dtDetail.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt16(dtDetail.Rows[i]["Efficiency"]) == 0)
                    ((DataTable)detailgridbs.DataSource).Rows[i].Delete();
                else
                    break;
            }
            #endregion

            #region 3.檢查表身資料不可以有Efficiency為0，若有則出訊息告知，且不可存檔
            foreach (DataRow dr in dtDetail.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (Convert.ToInt16(dr["Efficiency"]) == 0)
                    {
                        MyUtility.Msg.WarningBox("< Efficiency (%) > can not be 0 !!");
                        return false;
                    }
                }
            }
            #endregion

            #region 4.檢查表身資料不可全為空值，若有則出訊息告知，且不可存檔
            int DetailCount = dtDetail.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count();
            if (DetailCount == 0)
            {
                MyUtility.Msg.WarningBox("Detail can not be empty!");
                return false;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        private void txtFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            string sqlCmd = @"SELECT 'ALL' as FTYGroup
                            UNION
                            select distinct FTYGroup from Factory where FTYGroup<>'' and Junk=0";
            item = new Sci.Win.Tools.SelectItem(sqlCmd, "10", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            txtFactory.Text = item.GetSelectedString();
        }

        private void txtFactory_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && txtFactory.OldValue != txtFactory.Text)
            {
                if (!MyUtility.Check.Empty(txtFactory.Text))
                {
                    string sqlCmd = @"select * from 
                                    (SELECT 'ALL' as FTYGroup  UNION
                                    select distinct FTYGroup from Factory where FTYGroup<>'' and Junk=0) tmp
                                    where tmp.FTYGroup='{0}'";
                    if (!MyUtility.Check.Seek(string.Format(sqlCmd, txtFactory.Text)))
                    {
                        MyUtility.Msg.WarningBox(string.Format("Factory:{0} not found !!", txtFactory.Text));
                        CurrentMaintain["FactoryID"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }


    }
}
