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

namespace Sci.Production.Basic
{
    public partial class B08 : Sci.Win.Tems.Input1
    {
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        private void btnProdFabricType_Click(object sender, EventArgs e)
        {
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = CurrentMaintain["ID"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            //當CDCode_Content中沒有此CDCode資料時，就自動塞一筆空資料進去
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

            bool hasEditauthority = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "B08. CD Code", "CanEdit");
            Sci.Production.Basic.B08_ProductionFabricType rm = new Sci.Production.Basic.B08_ProductionFabricType(hasEditauthority, selectDataTable.Rows[0]);
            rm.ShowDialog(this);
            OnDetailEntered();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            //按鈕變色
            string sql = string.Format(@"select ID from CDCode_Content WITH (NOLOCK) where ID = '{0}' and 
                        (TopProductionType<>'' or TopFabricType<>'' or BottomProductionType<>'' or BottomFabricType<>'' or
                        InnerProductionType<>'' or InnerFabricType<>'' or OuterProductionType<>'' or OuterFabricType<>'')", CurrentMaintain["ID"].ToString());
            btnProdFabricType.ForeColor = MyUtility.Check.Seek(sql) ? Color.Blue : Color.Black;

        }


    }
}
