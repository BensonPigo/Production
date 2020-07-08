using System.Data;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_QtyCTN
    /// </summary>
    public partial class P01_QtyCTN : Sci.Win.Subs.Base
    {
        private DataRow masterData;

        /// <summary>
        /// P01_QtyCTN
        /// </summary>
        /// <param name="masterData">DataRow MasterData</param>
        public P01_QtyCTN(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.Text = this.Text + " (" + MyUtility.Convert.GetString(this.masterData["ID"]) + ")";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlCmd = string.Format("select * from Order_SizeCode WITH (NOLOCK) where ID = '{0}' order by Seq", MyUtility.Convert.GetString(this.masterData["POID"]));
            DataTable headerData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out headerData);
            StringBuilder pivot = new StringBuilder();

            // 設定Grid1的顯示欄位
            this.gridBreakDownperPavckingMethod.IsEditingReadOnly = true;
            this.gridBreakDownperPavckingMethod.DataSource = this.listControlBindingSource1;
            var gen = this.Helper.Controls.Grid.Generator(this.gridBreakDownperPavckingMethod);
            this.CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                    pivot.Append(string.Format("[{0}],", MyUtility.Convert.GetString(dr["SizeCode"])));
                }
            }

            // 凍結欄位
            this.gridBreakDownperPavckingMethod.Columns[0].Frozen = true;

            // 撈Grid資料
            sqlCmd = string.Format(
                @"
with tmpData
as (
select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
from Order_QtyCTN oq WITH (NOLOCK) 
left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
where oq.ID = '{0}'
)

select *,ROW_NUMBER() OVER (ORDER BY Seq) as rno
from tmpData
pivot( sum(Qty)
for SizeCode in ({1})
) a;",
                MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            DataTable gridData;
            result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            this.listControlBindingSource1.DataSource = gridData;
        }

        /// <summary>
        /// CreateGrid
        /// </summary>
        /// <param name="gen">IDataGridViewGenerator gen</param>
        /// <param name="datatype">string datatype</param>
        /// <param name="propname">string propname</param>
        /// <param name="header">string header</param>
        /// <param name="width">IWidth width</param>
        public void CreateGrid(IDataGridViewGenerator gen, string datatype, string propname, string header, IWidth width)
        {
            this.CreateGridCol(
                gen,
                datatype,
                propname: propname,
                header: header,
                width: width);
        }

        private void CreateGridCol(IDataGridViewGenerator gen, string datatype, string propname = null, string header = null, IWidth width = null, bool? iseditingreadonly = null, int index = -1)
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
