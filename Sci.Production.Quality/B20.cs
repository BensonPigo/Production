﻿using System;
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

namespace Sci.Production.Quality
{
    public partial class B20 : Sci.Win.Tems.Input1
    {
        public B20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            btnSettingSort.Enabled = this.Perm.Edit;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(5))
            .Text("ID", header: "Defect Code", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("LocalDescription", header: "Local Desc", width: Widths.AnsiChars(20), iseditingreadonly: true)
            ;
            this.grid1.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;

        }
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.grid1 != null)
            {
                if (EditMode)
                {
                    this.grid1.IsEditingReadOnly = false;
                }
                else
                {
                    this.grid1.IsEditingReadOnly = true;
                }
            }
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sql = $@"select * from GarmentDefectCode where GarmentDefectTypeID  = '{this.CurrentMaintain["ID"]}' order by seq";
            DataTable dt;
            DBProxy.Current.Select(null, sql, out dt);
            this.listControlBindingSource1.Position = -1;
            this.listControlBindingSource1.DataSource = dt;
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtDefectType.ReadOnly = true;
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(txtDefectType.Text))
            {
                this.txtDefectType.Focus();
                MyUtility.Msg.WarningBox("<Defect Type> cannot be empty! ");
                return false;
            }
            if (this.IsDetailInserting)
            {
                int seq = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup("select max(seq) from GarmentDefectType"));
                seq++;
                if (seq > 255)
                {
                    seq = 255;
                }
                CurrentMaintain["seq"] = seq;
            }
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            string update = string.Empty;
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w => w.RowState == DataRowState
            .Modified))
            {
                update += $@" update GarmentDefectCode set seq = '{dr["seq"]}' , editname = '{Sci.Env.User.UserID}',editDate = getdate() where  GarmentDefectTypeID  = '{this.CurrentMaintain["ID"]}' and id = '{dr["id"]}'; ";
            }
            DBProxy.Current.Execute(null, update);
            return base.ClickSavePost();
        }
        private void btnSettingSort_Click(object sender, EventArgs e)
        {
            B20_SettingSort f = new B20_SettingSort();
            f.ShowDialog();
            this.ReloadDatas();
        }
    }
}
