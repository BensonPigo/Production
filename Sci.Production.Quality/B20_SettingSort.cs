﻿using Ict.Win;
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

namespace Sci.Production.Quality
{
    public partial class B20_SettingSort : Sci.Win.Subs.Base
    {
        public B20_SettingSort()
        {
            InitializeComponent();
        }
        
        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            this.EditMode = true;
            this.grid1.IsEditingReadOnly = false;
            base.OnLoad(e);
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(5))
            .Text("ID", header: "Defect Type", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("LocalDescription", header: "Local Desc", width: Widths.AnsiChars(13), iseditingreadonly: true)
            ;
            this.grid1.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;

            string sql = $@"select * from GarmentDefectType order by seq";
            DataTable dt;
            DBProxy.Current.Select(null, sql, out dt);
            this.listControlBindingSource1.DataSource = dt;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string update = string.Empty;
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(w=>w.RowState == DataRowState
            .Modified))
            {
                update += $@" update GarmentDefectType set seq = '{dr["seq"]}' , editname = '{Sci.Env.User.UserID}',editDate = getdate() where id = '{dr["id"]}'; ";
            }
            DBProxy.Current.Execute(null, update);
            MyUtility.Msg.InfoBox("Success!");
            this.Close();
        }
    }
}
