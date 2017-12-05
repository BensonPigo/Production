using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.SubProcess
{
    public partial class B04 : Sci.Win.Tems.Input6
    {
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        // Type 右鍵開窗
        private void TxtType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(
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
        /// ClickSave
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickSave()
        {
            this.txtType.ReadOnly = true;
            this.txtID.ReadOnly = true;
            return base.ClickSave();
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
                if (MyUtility.Check.Empty(dt.Rows[0]["Day"]))
                {
                    this.CurrentDetailData["Day"] = 1;
                    return;
                }

                int maxNo1 = dt.AsEnumerable().Select(numb => numb.Field<int>("Day")).Max();
                //List<int> Day = dt.AsEnumerable().Select(r => r.Field<int>("Day")).Distinct().ToList();
                //int maxNo2 = Day.Max();
                maxNo = MyUtility.Convert.GetInt(dt.Compute("Max(Day)", string.Empty));

                this.CurrentDetailData["Day"] = maxNo + 1;
            }
        }

    }
}

