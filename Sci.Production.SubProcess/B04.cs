using Ict.Win;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// SubProcess_B04
    /// </summary>
    public partial class B04 : Win.Tems.Input6
    {
        /// <summary>
        /// B04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        // Type 右鍵開窗
        private void TxtType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                @"Select  Id,ArtworkTypeId  from  subprocess  where isselection=1 and Junk=0  order by Id ",
                "10,20",
                this.txtType.Text);
            DialogResult rtnResl = item.ShowDialog();
            if (rtnResl == DialogResult.Cancel)
            {
                return;
            }

            this.txtType.Text = item.GetSelectedString();
        }

        private void TxtType_Validating(object sender, CancelEventArgs e)
        {
            string txtValue = this.txtType.Text;
            if (MyUtility.Check.Empty(this.txtType.Text) || txtValue != this.txtType.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(
                    @"Select  Id from  subprocess  
where isselection=1 and Junk=0 and id='{0}'", txtValue)))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Type: {0}> not found !", txtValue));
                    this.txtType.Text = string.Empty;
                    return;
                }
            }
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            this.txtType.ReadOnly = false;
            this.txtID.ReadOnly = false;
            this.CurrentDetailData["Day"] = 1;
            base.ClickNewAfter();
        }

        /// <summary>
        /// ClickEditBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickEditBefore()
        {
            this.txtType.ReadOnly = true;
            this.txtID.ReadOnly = true;
            return base.ClickEditBefore();
        }

        /// <summary>
        /// ClickUndo
        /// </summary>
        protected override void ClickUndo()
        {
            this.txtType.ReadOnly = true;
            this.txtID.ReadOnly = true;
            base.ClickUndo();
        }

        /// <summary>
        /// ClickCopyAfter
        /// </summary>
        protected override void ClickCopyAfter()
        {
            this.txtType.ReadOnly = false;
            this.txtID.ReadOnly = false;
            base.ClickCopyAfter();
        }

        /// <summary>
        /// ClickSaveAfter
        /// </summary>
        protected override void ClickSaveAfter()
        {
            this.txtType.ReadOnly = true;
            this.txtID.ReadOnly = true;
            base.ClickSaveAfter();
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtType.Text))
            {
                this.txtType.Focus();
                MyUtility.Msg.WarningBox("Type cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtID.Text))
            {
                this.txtID.Focus();
                MyUtility.Msg.WarningBox("ID cannot be empty!");
                return false;
            }

            if (this.DetailDatas.Count < 1)
            {
                MyUtility.Msg.WarningBox("detail cannot be empty!");
                return false;
            }

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["Day"]) || MyUtility.Check.Empty(row["Efficiency"]))
                {
                    MyUtility.Msg.WarningBox("Day,Efficiency cannot be empty!");
                    return false;
                }

                if (MyUtility.Convert.GetDecimal(row["Efficiency"]) > 100)
                {
                    MyUtility.Msg.WarningBox(string.Format(@"<Efficiency% : {0}> cannot more than 100!", row["Efficiency"]));
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            #region 表身欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("Day", header: "Day", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 0, iseditingreadonly: true)
            .Numeric("Efficiency", header: "Efficiency(%)", width: Widths.AnsiChars(10), maximum: 100, decimal_places: 2);
            #endregion
        }

        /// <summary>
        /// OnDetailGridDelete
        /// </summary>
        protected override void OnDetailGridDelete()
        {
            base.OnDetailGridDelete();
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            int day = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                dr["day"] = day;
                day++;
            }
        }

        /// <summary>
        /// OnDetailGridInsert
        /// </summary>
        /// <param name="index">index</param>
        protected override void OnDetailGridInsert(int index = 1)
        {
            base.OnDetailGridInsert(index);
            int maxNo;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            if (dt.Rows.Count < 1)
            {
                maxNo = 0;
                base.OnDetailGridInsert(0);
                this.CurrentDetailData["Day"] = maxNo + 1;
            }
            else
            {
                int day = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    dr["day"] = day;
                    day++;
                }
            }
        }
    }
}
