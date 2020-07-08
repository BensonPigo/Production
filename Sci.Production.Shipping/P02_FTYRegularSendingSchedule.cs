using System;
using System.Data;
using System.Windows.Forms;
using Sci.Andy.ExtensionMethods;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P31_FTYRegularSendingSchedule
    /// </summary>
    internal partial class P02_FTYRegularSendingSchedule : Win.Subs.Base
    {
        /// <summary>
        /// P31_FTYRegularSendingSchedule
        /// </summary>
        public P02_FTYRegularSendingSchedule()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.SetupGrid();
        }

        private void SetupGrid()
        {
            string strToID = MyUtility.GetValue.Lookup(@"
select v = stuff((select Concat (',[', fs.ToID, ']')
				  from (
					select distinct fs.ToID
					from FactoryExpress_SendingSchedule fs
					where Junk != 1
				  ) fs				  
				  order by ToID
				  for xml path(''))
				 , 1, 1, '')");

            if (strToID.IsEmpty())
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return;
            }
            else
            {
                DataTable dtSendSch;
                string strSQL = $@"
select *
from (
	select	Country
			, RegionCode
			, ToID
			, RegularSend = stuff(Concat(iif (fs.Sun = 1, '/Sun', '')
										 , iif (fs.Mon = 1, '/Mon', '')
										 , iif (fs.Tue = 1, '/Tue', '')
										 , iif (fs.Wed = 1, '/Wed', '')
										 , iif (fs.Thu = 1, '/Thu', '')
										 , iif (fs.Fri = 1, '/Fri', '')
										 , iif (fs.Sat = 1, '/Sat', ''))
								  , 1, 1, '')
	from FactoryExpress_SendingSchedule fs
	where Junk != 1
) t
pivot (
	Max(RegularSend)
	for ToID in ({strToID})
) p
order by Country, RegionCode;";
                DualResult result = DBProxy.Current.Select(null, strSQL, out dtSendSch);

                if (result == false)
                {
                    this.ShowErr(result);
                }
                else
                {
                    foreach (DataColumn col in dtSendSch.Columns)
                    {
                        this.Helper.Controls.Grid.Generator(this.grid1)
                            .Text(col.ColumnName, header: col.ColumnName, width: Widths.AnsiChars(6), iseditingreadonly: true);
                        this.grid1.Columns[col.ColumnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    this.grid1.SetColumnsSortMode(false);
                    this.grid1.DataSource = dtSendSch;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
        }

        private void BtnUndoClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
