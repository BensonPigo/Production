using Ict;
using Ict.Win;
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

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class P05_MachineSummary : Sci.Win.Subs.Base
    {
        private DataTable[] dataTables;

        /// <inheritdoc/>
        public P05_MachineSummary(string almID = "")
        {
            this.InitializeComponent();
            this.LoadData(almID == string.Empty ? "0" : almID);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("MachineTypeID", header: "ST/MC", width: Widths.AnsiChars(10))
            .Text("MasterPlusGroup", header: "MC Group", width: Widths.AnsiChars(10))
            .Numeric("Count", header: "Count", decimal_places: 0, integer_places: 0, iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Item", header: "Item", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("No", header: "No.\r\nof Station", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Detail", header: "Detail", width: Widths.AnsiChars(30), iseditingreadonly: true);

            this.txtMachine.Value = MyUtility.Convert.GetInt(this.dataTables[0].AsEnumerable().Select(row => (int)row["Count"]).Sum());
            this.txtAttachment.Value = MyUtility.Convert.GetInt(this.dataTables[1].Rows[0]["AttachmentCount"]);
            this.txtTemplate.Value = MyUtility.Convert.GetInt(this.dataTables[1].Rows[0]["TemplateCount"]);

            this.grid.DataSource = this.dataTables[0];
            this.grid1.DataSource = this.dataTables[2];
        }

        private void LoadData(string almID = "")
        {
            /* 修改時記得參照P05_Print公式 */
            string sqlcmd = $@"
				DECLARE @ALMID bigint = {almID}
				-- Excel [Line Mapping] Machine 區塊共用資料
				select MachineTypeID, MasterPlusGroup, No, Seq, Attachment, SewingMachineAttachmentID, Template, concatString.Value
				into #main
				from AutomatedLineMapping_Detail
				-- 將Attachment, SewingMachineAttachmentID, Template用逗號拆分後，重新組成字串
				outer apply (
					select Value = isnull((
						select ltrim(rtrim(Data))
						from
						(
							select Data from dbo.SplitString(Attachment, ',')
							union all
							select Data from dbo.SplitString(SewingMachineAttachmentID, ',')
							union all
							select Data from dbo.SplitString(Template, ',')
							union all
							select [Data] = ThreadComboID
						) tmp
						order by tmp.Data FOR XML PATH('')
					), '')
				) concatString
				where ID = @ALMID
				and IsNonSewingLine = 0
				and PPA != 'C'
				and MachineTypeID not like 'MM%'

				-- Excel [Line Mapping] Machine 表格
				select item.MachineTypeID, item.MasterPlusGroup, getCount.Count
				from (
					select MachineTypeID, MasterPlusGroup
					from #main
					group by MachineTypeID, MasterPlusGroup
				) item
				-- 計數 (No + Value相同視為1個)
				outer apply (
					select [Count] = COUNT(distinct concat(main.No, main.Value))
					from #main main
					where main.MachineTypeID = item.MachineTypeID
					and main.MasterPlusGroup = item.MasterPlusGroup
				) getCount
				order by item.MachineTypeID, item.MasterPlusGroup

				-- Excel [Line Mapping] Attachment Count / Template Count
				select AttachmentCount = (
					select count(*) c from (
						select MachineTypeID, MasterPlusGroup, No, Value
						from #main
						where Attachment != ''
						group by MachineTypeID, MasterPlusGroup, No, Value
					) tmp)
				, TemplateCount = (
					select count(*) c from (
						select MachineTypeID, MasterPlusGroup, No, Value
						from #main
						where Template != ''
						group by MachineTypeID, MasterPlusGroup, No, Value
					) tmp)

				-- Excel [Line Mapping] Item 表格
				select Item = List.Data, main.No, Detail = REPLACE(getDetail.Value, ',', CHAR(13) + CHAR(10))
				from #main main
				outer apply (select tmpString = CONCAT(Attachment, iif(Template = '', '', ','), Template)) combine
				outer apply (select * from dbo.SplitString(combine.tmpString, ',')) List
				outer apply (
					select Value = isnull(STUFF((
						select ',' + ID
						from SewingMachineAttachment
						where ID in (select Data From dbo.SplitString(SewingMachineAttachmentID, ','))
						and MoldID = List.Data
						FOR XML PATH('')
					),1,1,''), '')
				) getDetail 
				where combine.tmpString != '' and List.Data != ''
				Order by main.No, Seq, List.no

				drop table #main

            ";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dataTables);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                return;
            }
        }
    }
}
