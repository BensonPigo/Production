using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P01_QtyCTN : Sci.Win.Subs.Base
    {
        DataRow masterData;
        public P01_QtyCTN(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
            this.Text = Text + " (" + MyUtility.Convert.GetString(masterData["ID"]) + ")";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlCmd = string.Format("select * from Order_SizeCode where ID = '{0}' order by Seq",MyUtility.Convert.GetString(masterData["POID"]));
            DataTable headerData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out headerData);
            StringBuilder pivot = new StringBuilder();
            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            var gen = Helper.Controls.Grid.Generator(this.grid1);
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                    pivot.Append(string.Format("[{0}],", MyUtility.Convert.GetString(dr["SizeCode"])));
                }
            }

            //撈Grid資料
            sqlCmd = string.Format(@"with tmpData
as (
select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
from Order_QtyCTN oq
left join Order_Article oa on oa.ID = '{0}' and oa.Article = oq.Article
where oq.ID = '{0}'
)

select *,ROW_NUMBER() OVER (ORDER BY Seq) as rno
from tmpData
pivot( sum(Qty)
for SizeCode in ({1})
) a;", MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Check.Empty(pivot.ToString()) ? "" : pivot.ToString().Substring(0, pivot.ToString().Length-1));
            DataTable gridData;
            result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            listControlBindingSource1.DataSource = gridData;
        }

        public void CreateGrid(IDataGridViewGenerator gen, string datatype, string propname, string header, IWidth width)
        {
            CreateGridCol(gen, datatype
                , propname: propname
                , header: header
                , width: width
            );
        }

        private void CreateGridCol(IDataGridViewGenerator gen, string datatype
            , string propname = null, string header = null, IWidth width = null, bool? iseditingreadonly = null
            , int index = -1)
        {
            switch (datatype)
            {
                case "int":
                    gen.Numeric(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
                case "string":
                default:
                    gen.Text(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
            }
        }
    }
}
