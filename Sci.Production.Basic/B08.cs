using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B08
    /// </summary>
    public partial class B08 : Win.Tems.Input1
    {
        /// <summary>
        /// B08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private void BtnProdFabricType_Click(object sender, EventArgs e)
        {
            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = this.CurrentMaintain["ID"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            // 當CDCode_Content中沒有此CDCode資料時，就自動塞一筆空資料進去
            string selectCommand = "select * from CDCode_Content WITH (NOLOCK) where ID = @id";
            DataTable selectDataTable;
            DualResult selectResult = DBProxy.Current.Select(null, selectCommand, cmds, out selectDataTable);
            if (selectDataTable.Rows.Count == 0)
            {
                string insertCommand = "insert into CDCode_Content(ID) values (@id); ";
                DualResult result;
                if (result = DBProxy.Current.Execute(null, insertCommand, cmds))
                {
                    selectResult = DBProxy.Current.Select(null, selectCommand, cmds, out selectDataTable);
                }
                else
                {
                    MyUtility.Msg.WarningBox("Insert data fail!! Please click button again. \n" + result.ToString());
                    return;
                }
            }

            bool hasEditauthority = PublicPrg.Prgs.GetAuthority(Env.User.UserID, "B08. CD Code", "CanEdit");
            B08_ProductionFabricType rm = new B08_ProductionFabricType(hasEditauthority, selectDataTable.Rows[0]);
            rm.ShowDialog(this);
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // 按鈕變色
            string sql = string.Format(
                @"select ID from CDCode_Content WITH (NOLOCK) where ID = '{0}' and 
                        (TopProductionType<>'' or TopFabricType<>'' or BottomProductionType<>'' or BottomFabricType<>'' or
                        InnerProductionType<>'' or InnerFabricType<>'' or OuterProductionType<>'' or OuterFabricType<>'')", this.CurrentMaintain["ID"].ToString());
            this.btnProdFabricType.ForeColor = MyUtility.Check.Seek(sql) ? Color.Blue : Color.Black;
        }
    }
}
