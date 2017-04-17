using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
namespace Sci.Win
{
    public partial class SUBBASE : Sci.Win.Subs.Base
    {
        public SUBBASE()
        {
            InitializeComponent();

            Helper.Controls.Grid.Generator(grid)
                .CheckBox().Get(out col_chk)
                .Text("id", header: "id")
                .Text("name", header: "name")
                ;

            chk_yes.Click += (s, e) =>
            {
                grid.SetCheckeds(col_chk);
            };
            chk_no.Click += (s, e) =>
            {
                grid.SetUncheckeds(col_chk);
            };
            get.Click += (s, e) =>
            {
                var datas = grid.GetCheckeds(col_chk);
                ShowInfo(datas.Count.ToString());
            };
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DualResult result;

            DataTable datas;
            if (!(result = Sci.Data.DBProxy.Current.Select(null, "SELECT * FROM accno", out datas)))
            {
                ShowErr(result);
            }
            else gridbs.DataSource = datas;
        }

        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
    }
}
