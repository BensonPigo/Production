using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tems;

namespace Sci.Production.Quality
{
    public partial class P40_History : P40
    {
        public P40_History(string ID)
        {
            InitializeComponent();

            this.DefaultFilter = $"ID = '{ID}'";
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string masterVersion = (e.Master == null) ? "" : e.Master["Version"].ToString();
            string cmd = string.Format(
@"
select 
ad.*,
[MainDefect] = asdMain.ID + '-' + asdMain.Name,
[SubDefect] = asdSub.SubID + '-' + asdSub.SubName,
asdSub.MtlTypeID,
asdSub.FabricType
from ADIDASComplain_Detail_History ad with (nolock)
left join ADIDASComplainDefect asdMain with (nolock) on ad.DefectMainID = asdMain.ID
left join ADIDASComplainDefect_Detail asdSub with (nolock) on  asdMain.ID = asdSub.ID and ad.DefectSubID = asdSub.SubID
where ad.ID = '{0}' and ad.Version = {1}
order by ad.SalesID,ad.Article,asdMain.ID + '-' + asdMain.Name,asdSub.SubID + '-' + asdSub.SubName,ad.OrderID
", masterID, masterVersion);
            this.DetailSelectCommand = cmd;
            return new DualResult(true);
        }
    }
}
